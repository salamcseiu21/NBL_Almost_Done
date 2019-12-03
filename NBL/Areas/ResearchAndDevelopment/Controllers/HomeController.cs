using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Employees;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager _userManager=new UserManager();
        private readonly ICommonManager _iCommonManager;
        private readonly IProductManager _iProductManager;
        private readonly IEmployeeManager _iEmployeeManager;

        public HomeController(ICommonManager iCommonManager,IProductManager iProductManager,IEmployeeManager iEmployeeManager)
        {
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: ResearchAndDevelopment/Home
        public ActionResult Home()
        {
            return View();
        }

        public PartialViewResult GeneralRequisitionList()
        {
            try
            {
                ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllGeneralRequisitions();
                return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public PartialViewResult GeneralRequisitionDetails(long id)
        {
            try
            {
                var details = _iProductManager.GetGeneralRequisitionDetailsById(id);
                return PartialView("_ViewGeneralRequisitionDetailsPartialPage", details);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult GeneralRequisition()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                ViewCreateGeneralRequsitionModel model =
                    new ViewCreateGeneralRequsitionModel { RequisitionByUserId = user.UserId };
                CreateTempRequisitionXmlFile();
                List<RequisitionFor> requisitionFors = _iCommonManager.GetAllRequisitionForList().ToList();
                ViewBag.RequisitionForId = new SelectList(requisitionFors, "RequisitionForId", "Description");
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult GeneralRequisition(FormCollection collection)
        {

            var user = (ViewUser)Session["user"];
            var model = new ViewCreateGeneralRequsitionModel
            {
                RequisitionByUserId = user.UserId
            };
            try
            {
                
                var products = GetProductFromXmalFile(GetGeneralRequisitionXmlFilePath());
                if (products.Count() != 0)
                {
                    var xmlData = XDocument.Load(GetGeneralRequisitionXmlFilePath());
                    var requisition = new GeneralRequisitionModel
                    {
                        RequisitionModels = products.ToList(),
                        RequisitionByUserId = user.UserId,
                        Quantity = products.Sum(n => n.Quantity),
                        RequisitionDate = Convert.ToDateTime(collection["RequisitionDate"]),
                        RequisitionRemarks = collection["RequisitionRemarks"]

                    };
                    int rowAffected = _iProductManager.SaveGeneralRequisitionInfo(requisition);
                    if (rowAffected > 0)
                    {
                        xmlData.Root?.Elements().Remove();
                        xmlData.Save(GetGeneralRequisitionXmlFilePath());
                        TempData["message"] = "Requisition Create  Successfully!";
                    }
                    else
                    {
                        TempData["message"] = "Failed to create Requisition!";
                    }
                }

                List<RequisitionFor> requisitionFors = _iCommonManager.GetAllRequisitionForList().ToList();
                ViewBag.RequisitionForId = new SelectList(requisitionFors, "RequisitionForId", "Description");
                return View(model);
            }
            catch(Exception exception)
            {
                List<RequisitionFor> requisitionFors = _iCommonManager.GetAllRequisitionForList().ToList();
                ViewBag.RequisitionForId = new SelectList(requisitionFors, "RequisitionForId", "Description");
                Log.WriteErrorLog(exception);
                return View(model);
                
            }
        }
        [HttpPost]
        public JsonResult AddRequisitionProductToXmlFile(ViewCreateGeneralRequsitionModel model)
        {

            SuccessErrorModel msgSuccessErrorModel = new SuccessErrorModel();
            try
            {
                var filePath = GetGeneralRequisitionXmlFilePath();
                var id = model.RequisitionForId.ToString("D2") + model.ProductId;
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == model.ProductId);
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Products")?.Add(
                        new XElement("Product", new XAttribute("Id", id),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("Quantity", model.Quantity),
                        new XElement("RequisitionForId", model.RequisitionForId)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public void Update(string id, int quantity)
        {
            try
            {

                var filePath = GetGeneralRequisitionXmlFilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Element("Products")?
                    .Elements("Product")?
                    .Where(n => n.Attribute("Id")?.Value == id).FirstOrDefault()
                    ?.SetElementValue("Quantity", quantity);
                xmlData.Save(filePath);


            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
               
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }

        [HttpPost]
        public void RemoveProductById(string id)
        {
            var filePath = GetGeneralRequisitionXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
            xmlData.Save(filePath);

        }
        [HttpGet]
        public void RemoveAll()
        {
            var filePath = GetGeneralRequisitionXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);

        }

        public PartialViewResult LoadTempRequisitionProductList(int RequisitionByUserId)
        {
            try
            {
                string filePath = GetGeneralRequisitionXmlFilePathByUserId(RequisitionByUserId);
                var products = GetProductFromXmalFile(filePath);
                return PartialView("_ViewGeneralRequsitionPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        private string GetGeneralRequisitionXmlFilePathByUserId(int requisitionByUserId)
        {
           
            string fileName = "Requisition_Products_" + requisitionByUserId + ".xml";
            var filePath = Server.MapPath("~/Files/GeneralRequisition/" + fileName);
            return filePath;
        }

        private IEnumerable<RequisitionModel> GetProductFromXmalFile(string filePath)
        {
            List<RequisitionModel> products = new List<RequisitionModel>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                RequisitionModel aProduct = new RequisitionModel();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.Serial = elementFirstAttribute;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.RequisitionForId = Convert.ToInt32(xElements[3].Value);
                aProduct.RequisitionFor = _iCommonManager.GetAllRequisitionForList().ToList()
                    .Find(n => n.RequisitionForId.Equals(Convert.ToInt32(xElements[3].Value)));
                products.Add(aProduct);
            }

            return products;
        }
        private string GetGeneralRequisitionXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            string fileName = "Requisition_Products_" + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Files/GeneralRequisition/" + fileName);
            return filePath;
        }
        private void CreateTempRequisitionXmlFile()
        {
           
            var filePath = GetGeneralRequisitionXmlFilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }

        }

        //--------------------------Update User Profile----------------
        [HttpGet]
        public ActionResult UpdateBasicInfo(int id)
        {

            try
            {
                Employee employee = _iEmployeeManager.GetById(id);
                return View(employee);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }

        }

        [HttpPost]
        public ActionResult UpdateBasicInfo(int id, Employee emp, HttpPostedFileBase EmployeeImage, HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.GetById(id);
                anEmployee.EmployeeName = emp.EmployeeName;
                anEmployee.PresentAddress = emp.PresentAddress;
                anEmployee.Phone = emp.Phone;
                anEmployee.AlternatePhone = emp.AlternatePhone;
                anEmployee.Email = emp.Email;
                anEmployee.Gender = emp.Gender;
                anEmployee.Email = emp.Email;
                anEmployee.NationalIdNo = emp.NationalIdNo;
                anEmployee.UserId = user.UserId;
                anEmployee.DoB = emp.DoB;

                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/" + sign;
                }

                var result = _iEmployeeManager.Update(anEmployee);

                if (result)
                {
                    //TempData["Message"] = "Saved Successfully!";
                    return RedirectToAction("MyProfile", "Home", new { id = emp.EmployeeId });
                }

                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.GetById(id);
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult UpdateEducationalInfo(int id)
        {
            try
            {
                EducationalInfo educational = new EducationalInfo { EmployeeId = id };
                return View(educational);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        [HttpPost]
        public ActionResult UpdateEducationalInfo(int id, EducationalInfo model)
        {
            try
            {
                bool result = _iEmployeeManager.UpdateEducationalInfo(model);
                if (result)
                {
                    return RedirectToAction("MyProfile", "Home", new { id = model.EmployeeId });
                }
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        public ActionResult MyProfile(int id)
        {
            try
            {
                List<EducationalInfo> educationalInfos = _iEmployeeManager.GetEducationalInfoByEmpId(id);
                var employee = _iEmployeeManager.GetEmployeeById(id);
                employee.EducationalInfos = educationalInfos;
                return View(employee);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}