﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ReturnController : Controller
    {
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IProductManager _iProductManager;
        private readonly IProductReturnManager _iProductReturnManager;
        public ReturnController(IDeliveryManager iDeliveryManager,IProductManager iProductManager,IProductReturnManager iProductReturnManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iProductManager = iProductManager;
            _iProductReturnManager = iProductReturnManager;
        }
        // GET: Sales/Return
        public ActionResult Home()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult Entry()
        {
            CreateTempReturnProductXmlFile();
            ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
            return View();
        }
        [HttpPost]
        public ActionResult Entry(ViewEntryReturnModel model) 
        {
            ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
            return View();
        }
        [Authorize(Roles = "User")]
        public ActionResult ConfirmReturnEntry()
        {
            var products= GetProductFromXmlFile(GetTempReturnProductsXmlFilePath());
            return View(products);
        }
        [HttpPost]
        public ActionResult ConfirmReturnEntry(FormCollection collection)
        {
            var products = GetProductFromXmlFile(GetTempReturnProductsXmlFilePath());
            var user = (ViewUser)Session["user"];
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId= Convert.ToInt32(Session["CompanyId"]);
            ReturnModel model = new ReturnModel
            {
                ReturnIssueByUserId = user.UserId,
                Products = products.ToList(),
                BranchId = branchId,
                CompanyId = companyId,
                Remarks = collection["Remarks"]
            };

            var result = _iProductReturnManager.SaveReturnProduct(model);
            if (result)
            {
                RemoveAll();
                return RedirectToAction("Entry");
            }
            ViewBag.Result = "Failed to save";
            return View();
        }
        [Authorize(Roles = "User")]
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
                        var deliveredOrder= _iDeliveryManager.GetOrderByDeliveryId(Convert.ToInt32(deliveryId));
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
                                    new XElement("ProductName", product.ProductName),
                                    new XElement("DeliveryDate", deliveredOrder.DeliveryDate)
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
                model.Message = "<p style='color:red'> Failed to add" + e.Message + "</p>";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "User")]

        public PartialViewResult DeliveryDetailsByDeliveryId(long deliveryId)
        {
            var models = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId);
            return PartialView("_ViewDeliveryDetailsByIdPartialPage", models);


        }
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
        //-------------Delete all added product from xml file---------------
        public void RemoveAll()
        {
            var filePath = GetTempReturnProductsXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);
        }
        [Authorize(Roles = "User")]
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
                    ProductName = xElements[4].Value,
                    DeliveryDate = Convert.ToDateTime(xElements[5].Value)


                };
                products.Add(model);
            }
            return products;
        }
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
        //-----------------Get Temp return file path------------
        private string GetTempReturnProductsXmlFilePath() 
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Temp_Return_Product_Entry_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/Returns/" + fileName);
            return filePath;
        }
        [Authorize(Roles = "User")]
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

        public ActionResult ReturnDetails(long salesReturnId)
        {
            List<ReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList(); 
            return View(models);
        }
        public ActionResult ViewAll()
        {
            var products = _iProductReturnManager.GetAll().ToList();
            return View(products);
        }



        //---------------------Approve By NSM---------------
        [Authorize(Roles = "Nsm")]
        public ActionResult ApproveByNsm(long salesReturnId)
        {
            ViewBag.SalesReturnId = salesReturnId;
            List<ReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
            return View(models);
        }
        [HttpPost]
        public ActionResult ApproveByNsm(FormCollection collection)
        {
            var user = (ViewUser)Session["user"];
            long salesReturnId = Convert.ToInt64(collection["salesReturnId"]);
            var remarks = collection["Remarks"];
            bool result = _iProductReturnManager.ApproveReturnByNsm(remarks, salesReturnId, user.UserId);
            if (result)
            {
                return RedirectToAction("ViewAll");
            }
           
            List<ReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
            return View(models);
        }
    }
}