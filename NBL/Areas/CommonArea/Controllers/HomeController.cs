using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.CommonArea.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager _userManager = new UserManager();
        private readonly ICommonManager _iCommonManager;
        private readonly IProductManager _iProductManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;
        public HomeController(ICommonManager iCommonManager, IProductManager iProductManager,IBranchManager iBranchManager,IEmployeeManager iEmployeeManager)
        {
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
            _iBranchManager = iBranchManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: CommonArea/Home
        public ActionResult Home()
        {
            return View();
        }

        public PartialViewResult GeneralRequisitionList()
        {
            var user = (ViewUser)Session["user"];
            ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n=>n.RequisitionByUserId==user.UserId);
            return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
        }
        public PartialViewResult GeneralRequisitionListForApproval()
        {
            var user = (ViewUser)Session["user"];
            ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n => n.CurrentApproverUserId == user.UserId);
            return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
        }


        public PartialViewResult GeneralRequisitionDetails(long id)
        {
            ViewBag.Requisition=_iProductManager.GetGeneralRequisitionById(id);
            ViewBag.ApproverActionId = _iCommonManager.GetAllApprovalActionList().ToList();
            var details = _iProductManager.GetGeneralRequisitionDetailsById(id);
            return PartialView("_ViewGeneralRequisitionDetailsPartialPage", details);
        }

        [HttpPost]
        public ActionResult Approve(FormCollection collection)
        {
            var user = (ViewUser)Session["user"];
            try
            {
                
                var id = Convert.ToInt64(collection["RequisitionId"]);
                var actionId = Convert.ToInt32(collection["ApprovarActionId"]);
                var remarks = collection["Remarks"];
                var approval = new ApprovalDetails
                {
                    ApprovalActionId = actionId,
                    ApproverUserId = user.UserId,
                    Remarks = remarks,
                    GeneralRequisitionId = id
                };
                GeneralRequisitionModel model = _iProductManager.GetGeneralRequisitionById(id);
                var approvalLevel = _iCommonManager.GetAllApprovalPath().ToList()
                    .FindAll(n => n.UserId == model.RequisitionByUserId)
                    .Find(n => n.ApprovalLevel.Equals(model.CurrentApprovalLevel + 1));
               
                var nextApprovalLevel = 0;
                var nextApproverUser = 0;
                if (approvalLevel != null)
                {
                    nextApprovalLevel = approvalLevel.ApprovalLevel;
                    nextApproverUser = approvalLevel.ApproverUserId;
                }
                if (approvalLevel == null)
                {
                    model.IsFinalApproved = "Y";
                }
                if (actionId !=1)
                {
                    model.IsCancelled = "Y";
                }
                bool result = _iProductManager.ApproveGeneralRequisition(model, nextApproverUser, nextApprovalLevel,approval);

                ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllGeneralRequisitions()
                    .ToList().FindAll(n => n.CurrentApproverUserId == user.UserId);
                return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
            }
            catch (Exception exception)
            {
               
                ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllGeneralRequisitions()
                    .ToList().FindAll(n => n.CurrentApproverUserId == user.UserId);
                return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
            }

        }
        public ActionResult GeneralRequisition()
        {
            var user = (ViewUser)Session["user"];
            ViewCreateGeneralRequsitionModel model =
                new ViewCreateGeneralRequsitionModel { RequisitionByUserId = user.UserId };
            CreateTempRequisitionXmlFile();
            List<RequisitionFor> requisitionFors = _iCommonManager.GetAllRequisitionForList().ToList();
            ViewBag.RequisitionForId = new SelectList(requisitionFors, "RequisitionForId", "Description");
            return View(model);
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
            catch (Exception exception)
            {
                List<RequisitionFor> requisitionFors = _iCommonManager.GetAllRequisitionForList().ToList();
                ViewBag.RequisitionForId = new SelectList(requisitionFors, "RequisitionForId", "Description");
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

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }


        public void UpdateDuringApproval(string id, int quantity)
        {
            try
            {

              bool result=_iProductManager.UpdateGeneralRequisitionQuantity(id, quantity);


            }
            catch (Exception e)
            {

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
        [HttpPost]
        public void RemoveProductByIdDuringApproval(string id)
        {
            try
            {

                bool result = _iProductManager.RemoveProductByIdDuringApproval(id);


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }

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
            string filePath = GetGeneralRequisitionXmlFilePathByUserId(RequisitionByUserId);
            var products = GetProductFromXmalFile(filePath);
            return PartialView("_ViewGeneralRequsitionPartialPage", products);
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

        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }

        //------------------ Change password------------------------
        public PartialViewResult ChangePassword(int id)
        {

            var user = _userManager.GetUserInformationByUserId(id);
            user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return PartialView("_ChangePasswordPartialPage", user);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
           var user = _userManager.GetUserInformationByUserId(model.UserId);
           var emp = _iEmployeeManager.GetEmployeeById(user.EmployeeId);
            model.Password = StringCipher.Encrypt(model.Password, "salam_cse_10_R");
            model.PasswordChangeRequiredWithin = 30;
            bool result = _userManager.UpdatePassword(model);
            if (result)
            {
                //---------Send Mail ----------------
                var body = $"Dear {emp.EmployeeName}, your account password had been updated successfully!";
                var subject = $"Password Changed at {DateTime.Now}";
                var message = new MailMessage();
                message.To.Add(new MailAddress(emp.Email));  // replace with valid value 
                message.Bcc.Add("salam@navana.com");
                message.Subject = subject;
                message.Body = string.Format(body);
                message.IsBodyHtml = true;
                //message.Attachments.Add(new Attachment("E:/API/NBL/NBL/Images/bg1.jpg"));
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
                //------------End Send Mail-------------
                return RedirectToAction("LogIn", "Login", new { area = "" });
            }
                
            return RedirectToAction("ChangePassword");
        }
    }
}