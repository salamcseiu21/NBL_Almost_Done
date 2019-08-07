using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IOrderManager _iOrderManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IProductReturnManager _iProductReturnManager;
        public ProductController(IOrderManager iOrderManager, IReportManager iReportManager, IInventoryManager iInventoryManager,IProductManager iProductManager,IDeliveryManager iDeliveryManager,IProductReturnManager iProductReturnManager)
        {
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iDeliveryManager = iDeliveryManager;
            _iProductReturnManager = iProductReturnManager;
        }
        // GET: ResearchAndDevelopment/Product
        public ActionResult ProductLifeCycle()
        {
            return View();
        }

        public ActionResult ReturnProduct()
        {
            try
            {
                CreateTempReturnProductXmlFile();
                var user = (ViewUser) Session["user"];
                ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetGeneralRequisitionByUserId(user.UserId);
                ViewBag.DeliveryId = new SelectList(requisitions.ToList(), "DeliveryId", "DeliveryRef");
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public JsonResult AddReturnProductToXmalFile(FormCollection collection)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {
                int _count = 0;
                var collectionKeys = collection.AllKeys.ToList();
                foreach (string key in collectionKeys)
                {
                    if (collection[key] != "")
                    {
                        var first = key.IndexOf("_", StringComparison.Ordinal) + 1;
                        var last = key.LastIndexOf("_", StringComparison.Ordinal) + 1;
                        var quantity = Convert.ToInt32(collection[key]);
                        var productId = Convert.ToInt32(key.Substring(first, 3));
                        var product = _iProductManager.GetProductByProductId(productId);
                        var deliveryRef = key.Substring(0, first - 1);
                        var deliveryId = key.Substring(last, key.Length - last);
                        //var deliveredOrder = _iDeliveryManager.GetOrderByDeliveryId(Convert.ToInt32(deliveryId));
                        //var requisition = _iProductManager.GetRequsitions().ToList().Find(n => n.RequisitionId == requisitionId);
                        var filePath = GetTempReturnProductsXmlFilePath();
                        var xmlDocument = XDocument.Load(filePath);
                        if (quantity > 0)
                        {
                            xmlDocument.Element("Products")?.Add(
                                new XElement("Product", new XAttribute("Id", Guid.NewGuid()),
                                    new XElement("DeliveryId", deliveryId),
                                    new XElement("DeliveryRef", deliveryRef),
                                    new XElement("ProuctId", productId),
                                    new XElement("Quantity", quantity),
                                    new XElement("ProductName", product.ProductName)
                                    //new XElement("DeliveryDate", deliveredOrder.DeliveryDate)
                                ));
                            xmlDocument.Save(filePath);
                            _count++;
                        }

                    }
                }
                if (_count > 0)
                {
                    model.Message = "<p style='color:green'>Added Successfully</p>";
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);

                model.Message = "<p style='color:red'> Failed to add" + e.Message + "</p>";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

      
        public ActionResult ConfirmReturnEntry()
        {
            try
            {
                var products = GetProductFromXmlFile(GetTempReturnProductsXmlFilePath());
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ConfirmReturnEntry(FormCollection collection)
        {
            try
            {
                var products = GetProductFromXmlFile(GetTempReturnProductsXmlFilePath());
                //var deliveryId = products.FirstOrDefault().DeliveryId;
               // var clientId = _iDeliveryManager.GetChalanByDeliveryId(Convert.ToInt32(deliveryId)).ViewClient.ClientId;
                var user = (ViewUser)Session["user"];
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                ReturnModel model = new ReturnModel
                {
                    ReturnIssueByUserId = user.UserId,
                    Products = products.ToList(),
                    BranchId = branchId,
                    CompanyId = companyId,
                    ClientId = null,
                    Remarks = collection["Remarks"],
                    CurrentApprovalLevel = 1,
                    CurrentApproverRoleId = Convert.ToInt32(RoleEnum.RnDManager)
                };

                var result = _iProductReturnManager.SaveReturnProduct(model);
                if (result)
                {
                    RemoveAll();
                    return RedirectToAction("ReturnProduct");
                }
                ViewBag.Result = "Failed to save";
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //-----------------Delete single product from xml file------------------------
        public JsonResult DeleteProductFromTempReturn(string returnId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            var filePath = GetTempReturnProductsXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == returnId).Remove();
            xmlData.Save(filePath);
            model.Message = "<p style='color:red;'>Product Removed From lsit!</p>";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
       
        //-------------Delete all added product from xml file---------------
        public void RemoveAll()
        {
            var filePath = GetTempReturnProductsXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);
        }
      
        private IEnumerable<ReturnProduct> GetProductFromXmlFile(string filePath)
        {
            List<ReturnProduct> products = new List<ReturnProduct>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {

                var elementFirstAttribute = element.FirstAttribute.Value;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                var model = new ReturnProduct
                {
                    ReturnId = elementFirstAttribute,
                    DeliveryId = Convert.ToInt64(xElements[0].Value),
                    DeliveryRef = xElements[1].Value,
                    ProductId = Convert.ToInt32(xElements[2].Value),
                    Quantity = Convert.ToInt32(xElements[3].Value),
                    ProductName = xElements[4].Value
                  //  DeliveryDate = Convert.ToDateTime(xElements[5].Value)


                };
                products.Add(model);
            }
            return products;
        }
      
        //--------------Reading product form xml files---------------
        public PartialViewResult GetTempReturnProducts()
        {
            var filePath = GetTempReturnProductsXmlFilePath();

            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                IEnumerable<ReturnProduct> products = GetProductFromXmlFile(filePath);
                return PartialView("_ViewTempReturnProducts", products);
            }
            //if the file does not exists create the file
            System.IO.File.Create(filePath).Close();
            return PartialView("_ViewTempReturnProducts", new List<ReturnProduct>());
        }
     
        //-----------------Get Temp return file path------------
        private string GetTempReturnProductsXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Temp_Return_Product_Entry_By_" + branchId + user.UserId + "From_RnD.xml";
            var filePath = Server.MapPath("~/Areas/ResearchAndDevelopment/Files/Returns/" + fileName);
            return filePath;
        }
      
        //------------------Create Xml File-------------------

        private void CreateTempReturnProductXmlFile()
        {

            var filePath = GetTempReturnProductsXmlFilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }

        }
        public PartialViewResult DeliveryDetailsByDeliveryId(long deliveryId)
        {
            try
            {
                var models = _iDeliveryManager.GetDeliveredGeneralReqById(deliveryId);
                return PartialView("_ViewDeliveryDetailsByIdPartialPage", models);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        //--------------------------Product Status By Barcode---------------
        public ActionResult ProductStatus()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult ProductStatus(FormCollection collection)
        {
            var barcode = collection["BarCode"];
            var product = _iInventoryManager.GetProductLifeCycle(barcode);
            TempData["T"] = product;
            return View();
        }
    }
}