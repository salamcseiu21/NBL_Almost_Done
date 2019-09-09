using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ReturnController : Controller
    {
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IProductManager _iProductManager;
        private readonly IProductReturnManager _iProductReturnManager;
        private readonly IClientManager _iClientManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        public ReturnController(IDeliveryManager iDeliveryManager,IProductManager iProductManager,IProductReturnManager iProductReturnManager,IClientManager iClientManager,IOrderManager iOrderManager,IInvoiceManager iInvoiceManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iProductManager = iProductManager;
            _iProductReturnManager = iProductReturnManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iInvoiceManager = iInvoiceManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
        }
        // GET: Sales/Return
        public ActionResult Home()
        {
            return View();
        }

        [Authorize(Roles = "SalesExecutive")]
        public ActionResult Entry()
        {
            try
            {
                CreateTempReturnProductXmlFile();
                ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Entry(ViewEntryReturnModel model) 
        {
            try
            {
                ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [Authorize(Roles = "SalesExecutive")]
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
                var deliveryId = products.FirstOrDefault().DeliveryId;
                var clientId= _iDeliveryManager.GetChalanByDeliveryId(Convert.ToInt32(deliveryId)).ViewClient.ClientId; 
                var user = (ViewUser)Session["user"];
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                ReturnModel model = new ReturnModel
                {
                    ReturnIssueByUserId = user.UserId,
                    Products = products.ToList(),
                    BranchId = branchId,
                    CompanyId = companyId,
                    ClientId = clientId,
                    Remarks = collection["Remarks"],
                    CurrentApprovalLevel = 1,
                    CurrentApproverRoleId = Convert.ToInt32(RoleEnum.SalesManager)
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
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [Authorize(Roles = "SalesExecutive")]
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
                Log.WriteErrorLog(e);
             
                model.Message = "<p style='color:red'> Failed to add" + e.Message + "</p>";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "SalesExecutive")]

        public PartialViewResult DeliveryDetailsByDeliveryId(long deliveryId)
        {
            try
            {
                var models = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId);
                return PartialView("_ViewDeliveryDetailsByIdPartialPage", models);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }


        }
        [Authorize(Roles = "SalesExecutive")]
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
        [Authorize(Roles = "SalesExecutive")]
        //-------------Delete all added product from xml file---------------
        public void RemoveAll()
        {
            var filePath = GetTempReturnProductsXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);
        }
        [Authorize(Roles = "SalesExecutive")]
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
        [Authorize(Roles = "SalesExecutive")]
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
        [Authorize(Roles = "SalesExecutive")]
        //-----------------Get Temp return file path------------
        private string GetTempReturnProductsXmlFilePath() 
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Temp_Return_Product_Entry_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/Returns/" + fileName);
            return filePath;
        }
        [Authorize(Roles = "SalesExecutive")]
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
            try
            {
                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                return View(models);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ViewAll()
        {
            try
            {
                var returnModels = _iProductReturnManager.GetAll().ToList();
                return View(returnModels);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult GeneralRequisitionReturn()
        {
            try
            {
                var returnModels = _iProductReturnManager.GetAllGeneralReqReturnsByApprovarRoleId(Convert.ToInt32(RoleEnum.SalesAdmin)).ToList();
                return View(returnModels);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult PendingSalesReturns() 
        {
            try
            {
                ICollection<ReturnModel> products = _iProductReturnManager.GetAllReturnsByApprovarRoleId(Convert.ToInt32(RoleEnum.SalesManager));
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //---------------------Approve By sales Manager---------------
        [Authorize(Roles = "SalesManager")]
        public ActionResult ApproveByNsm(long salesReturnId)
        {
            try
            {

                ViewBag.ApproverActionId = _iCommonManager.GetAllApprovalActionList().ToList();
                ViewBag.SalesReturnId = salesReturnId;
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                ViewReturnModel returnModel = new ViewReturnModel
                {
                    ReturnDetailses = models,
                    ReturnModel = returnById,
                    
                };
                var firstOrdefault=models.FirstOrDefault();
                if(firstOrdefault != null)
                {
                    var delivery = _iDeliveryManager.GetOrderByDeliveryId(firstOrdefault.DeliveryId);
                    //var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
                    var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(firstOrdefault.DeliveryId);

                    // var invocedOrder = _iInvoiceManager.GetInvoicedOrderByInvoiceId(deliveryId);
                    var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                    //IEnumerable<InvoiceDetails> details = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceId(deliveryId);
                    var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                    ViewInvoiceModel model = new ViewInvoiceModel
                    {
                        Client = client,
                        Order = orderInfo,
                        Delivery = delivery,
                        DeliveryDetails = deliveryDetails
                    };

                    returnModel.InvoiceModel = model;
                }

                return View(returnModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ApproveByNsm(FormCollection collection)
        {
            try
            {
                var remarks = collection["Remarks"];
                var user = (ViewUser)Session["user"];
                long salesReturnId = Convert.ToInt64(collection["salesReturnId"]);
                var aproverActionId= Convert.ToInt32(collection["ApprovarActionId"]);
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);

                returnById.LastApproverDatetime=DateTime.Now;
                returnById.LastApproverRoleId = returnById.CurrentApproverRoleId;
                returnById.CurrentApprovalLevel = returnById.CurrentApprovalLevel + 1;
                returnById.CurrentApproverRoleId = Convert.ToInt32(RoleEnum.SalesAdmin);
                returnById.NotesByManager = remarks;
                returnById.SalesReturnId = salesReturnId;
                returnById.ApproveByManagerUserId = user.UserId;
                returnById.AproveActionId = aproverActionId;

                bool result = _iProductReturnManager.ApproveReturnBySalesManager(returnById);
                if (result)
                {
                    return RedirectToAction("PendingSalesReturns");
                }

                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                ViewReturnModel returnModel = new ViewReturnModel
                {
                    ReturnDetailses = models,
                   
                };
                return View(returnModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        //---------------------Approve By sales Admin---------------
        [Authorize(Roles = "SalesAdmin")]
        public ActionResult ApproveBySalesAdmin(long salesReturnId)
        {
            try
            {
                ViewBag.ApproverActionId = _iCommonManager.GetAllApprovalActionList().ToList();
                ViewBag.SalesReturnId = salesReturnId;
                var returnById=_iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                List<ViewReturnDetails> models;
                ViewReturnModel model = new ViewReturnModel();
                if (returnById.ClientId != null)
                {
                    models= _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                    var firstOrdefault = models.FirstOrDefault();
                    if (firstOrdefault != null)
                    {
                        var delivery = _iDeliveryManager.GetOrderByDeliveryId(firstOrdefault.DeliveryId);
                        var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(firstOrdefault.DeliveryId);
                        var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                        var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                        ViewInvoiceModel invoice = new ViewInvoiceModel
                        {
                            Client = client,
                            Order = orderInfo,
                            Delivery = delivery,
                            DeliveryDetails = deliveryDetails
                        };
                        model.InvoiceModel = invoice;

                    }
                }
                else
                {
                    models = _iProductReturnManager.GetGeneralReqReturnDetailsById(salesReturnId).ToList();
                }
                model.ReturnModel = returnById;
                model.ReturnDetailses = models;
             
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ApproveBySalesAdmin(FormCollection collection)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                long salesReturnId = Convert.ToInt64(collection["salesReturnId"]);
                var remarks = collection["Remarks"];
                var aproverActionId = Convert.ToInt32(collection["ApprovarActionId"]);
                var returnLessAmount = Convert.ToDecimal(collection["ReturnLessAmount"]);
                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                returnById.LastApproverDatetime = DateTime.Now;
                returnById.LastApproverRoleId = returnById.CurrentApproverRoleId;
                returnById.CurrentApprovalLevel = 0;
                returnById.CurrentApproverRoleId = 0;
                returnById.IsFinalApproved = 1;
                returnById.NotesByAdmin = remarks;
                returnById.SalesReturnId = salesReturnId;
                returnById.ApproveByAdminUserId = user.UserId;
                returnById.AproveActionId = aproverActionId;

                bool result = _iProductReturnManager.ApproveReturnBySalesAdmin(returnById,returnLessAmount);
                if (result)
                {
                    return RedirectToAction("ViewAll");
                }
                ViewReturnModel aModel = new ViewReturnModel {ReturnDetailses = models};
                return View(aModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [Authorize(Roles = "DistributionManager")]
        public ActionResult PendingReturnList()
        {
            try
            {
               // var products = _iProductReturnManager.GetAllReturnsByStatus(2).ToList();
                ICollection<ReturnModel> products=_iProductReturnManager.GetAllFinalApprovedReturnsList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

       


        [Authorize(Roles = "DistributionManager")]
        public ActionResult Receive(long id)
        {
            try
            {
                ViewBag.SalesReturnId = id;
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(id);
                List<ViewReturnDetails> models;
                ViewReturnModel model = new ViewReturnModel();
                if (returnById.ClientId != null)
                {
                    models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(id).ToList();
                    var firstOrdefault = models.FirstOrDefault();
                    if (firstOrdefault != null)
                    {
                        var delivery = _iDeliveryManager.GetOrderByDeliveryId(firstOrdefault.DeliveryId);
                        var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(firstOrdefault.DeliveryId);
                        var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                        var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                        ViewInvoiceModel invoice = new ViewInvoiceModel
                        {
                            Client = client,
                            Order = orderInfo,
                            Delivery = delivery,
                            DeliveryDetails = deliveryDetails
                        };
                        model.InvoiceModel = invoice;

                    }
                }
                else
                {
                    models = _iProductReturnManager.GetGeneralReqReturnDetailsById(id).ToList();
                }

                model.ReturnModel = returnById;
                model.ReturnDetailses = models;
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        [HttpPost]
        public ActionResult Receive(long salesReturnId, FormCollection collection)
        {

            try
            {
               
                var user = (ViewUser)Session["user"];
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
              
                List<ViewReturnDetails> newReturnDetailsList = new List<ViewReturnDetails>();
                var firstOrdefault = models.FirstOrDefault();

                if(firstOrdefault != null)
                {
                    var delivery = _iDeliveryManager.GetOrderByDeliveryId(firstOrdefault.DeliveryId);
                    var invoicedOrder = _iInvoiceManager.GetInvoicedOrderByInvoiceId(delivery.InvoiceId);
                    var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(firstOrdefault.DeliveryId);
                    var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                    var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                    int branchId = Convert.ToInt32(Session["BranchId"]);
                    var filePath = GetReceiveProductFilePath(salesReturnId, branchId);
                    //------------read Scanned barcode form text file---------
                    var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

                    var invoice = new ViewInvoiceModel
                    {
                        Order = orderInfo,
                        DeliveryDetails = deliveryDetails,
                        Client = client
                    };



                    foreach (ViewReturnDetails item in models)
                    {
                        item.UnitPrice = invoice.DeliveryDetails.ToList().Find(n => n.ProductId == item.ProductId)
                            .UnitPrice;
                        item.VatAmount = invoice.DeliveryDetails.ToList().Find(n => n.ProductId == item.ProductId)
                            .VatAmount;
                        item.DiscountAmount = invoice.DeliveryDetails.ToList().Find(n => n.ProductId == item.ProductId)
                            .UnitDiscount;
                        newReturnDetailsList.Add(item);
                    }


                    var grossAmount = newReturnDetailsList.Sum(n => (n.UnitPrice + n.VatAmount) * n.Quantity);
                    var tradeDiscount = newReturnDetailsList.Sum(n => n.DiscountAmount * n.Quantity);
                    var invoiceDiscount = (invoicedOrder.SpecialDiscount / invoicedOrder.Quantity) * newReturnDetailsList.Sum(n => n.Quantity);
                    var grossDiscount = tradeDiscount + invoiceDiscount;
                    var vat = newReturnDetailsList.Sum(n => n.VatAmount * n.Quantity);


                    var financialModel =
                        new FinancialTransactionModel
                        {
                            //--------Cr -------------------
                            ClientCode = invoice.Client.SubSubSubAccountCode,
                            ClientCrAmount = grossAmount - grossDiscount,
                            GrossDiscountAmount = grossDiscount,
                            GrossDiscountCode = "2102018",

                            //--------Dr -------------------
                            SalesRevenueCode = "2001021",
                            SalesRevenueAmount = grossAmount - vat,
                            VatCode = "2102013",
                            VatAmount = vat,


                            //--------------Sales Return---------
                            SalesReturnAmount = returnById.LessAmount,
                            SalesReturnCode = "1001022",
                            ClientDrAmount = returnById.LessAmount,


                            TradeDiscountCode = "2102012",
                            TradeDiscountAmount = tradeDiscount,
                            InvoiceDiscountAmount = invoiceDiscount,
                            InvoiceDiscountCode = "2102011"


                        };

                    var result = _iInventoryManager.ReceiveProduct(barcodeList,branchId,user.UserId,financialModel,returnById);

                    if(result)
                    {
                        System.IO.File.Create(filePath).Close();
                        return RedirectToAction("PendingReturnList");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }



        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode, long salesReturnId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
          
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                var returndetailsbyId= _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList().FirstOrDefault(n=>n.SalesReturnId==salesReturnId);
                var filePath = GetReceiveProductFilePath(salesReturnId, branchId);

                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                //------------Load receiveable product---------
                var receivesProductList = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                List<ViewProduct> products = _iDeliveryManager.GetDeliveredProductListByTransactionRef(returndetailsbyId.DeliveryRef).ToList();
                var receivesProductCodeList = products.Select(n=>n.ProductBarCode).ToList();
                var isvalid = Validator.ValidateProductBarCode(scannedBarCode);

                int requistionQtyByProductId = receivesProductList.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity);

                int scannedQtyByProductId = barcodeList
                    .FindAll(n => Convert.ToInt32(n.ProductCode.Substring(2, 3)) == productId).Count;

                bool isScannComplete = requistionQtyByProductId.Equals(scannedQtyByProductId);

                if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scanned Complete</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (!isvalid)
                {
                    model.Message = "<p style='color:red'> Invalid Barcode</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }

                else if (receivesProductCodeList.Contains(scannedBarCode))
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                // return Json(model, JsonRequestBehavior.AllowGet);
            }
            // return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult LoadReceiveableProduct(long salesReturnId)
        {
            try
            {
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                List<ViewReturnDetails> products;
                if (returnById.ClientId != null)
                {
                    products = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                }
                else
                {
                    products = _iProductReturnManager.GetGeneralReqReturnDetailsById(salesReturnId).ToList();
                }
                List<ViewDispatchModel> returnList=new List<ViewDispatchModel>();
                foreach (var item in products )  {
                     returnList.Add(new ViewDispatchModel
                     {
                         ProductName = item.ProductName,
                         ProductCategory = new ProductCategory
                         {
                             ProductCategoryName = item.ProductCategoryName
                         },
                         Quantity = item.Quantity
                     });
                }
                return PartialView("_ViewReceivalbeProductPartialPage", returnList);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        [HttpPost]
        public PartialViewResult LoadScannecdProduct(long salesReturnId)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var filePath = GetReceiveProductFilePath(salesReturnId,branchId);
                List<ScannedProduct> products = new List<ScannedProduct>();

                if (System.IO.File.Exists(filePath))
                {
                    //if the file is exists read the file
                    products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                }
                else
                {
                    //if the file does not exists create the file
                    System.IO.File.Create(filePath).Close();
                }
                return PartialView("_ViewScannedProductPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        private string GetReceiveProductFilePath(long salesReturnId, int branchId)
        {
            string fileName = "Received_Return_Product_For_" + salesReturnId + "_" + branchId;
            var filePath = Server.MapPath("~/Areas/Sales/Files/Returns/" + fileName);
            return filePath;
        }

        //--------------------receive general return product-------------------
        [Authorize(Roles = "DistributionManager")]
        public ActionResult PendingGeneralReturnList()
        {
            try
            {
                // var products = _iProductReturnManager.GetAllReturnsByStatus(2).ToList();
                ICollection<ReturnModel> products = _iProductReturnManager.GetAllFinalApprovedGeneralReturnsList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [Authorize(Roles = "DistributionManager")]
        public ActionResult ReceiveGeneralReturn(long id) 
        {
            try
            {
                ViewBag.SalesReturnId = id;
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(id);
                List<ViewReturnDetails> models;
                ViewReturnModel model = new ViewReturnModel();
                if (returnById.ClientId != null)
                {
                    models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(id).ToList();
                    var firstOrdefault = models.FirstOrDefault();
                    if (firstOrdefault != null)
                    {
                        var delivery = _iDeliveryManager.GetOrderByDeliveryId(firstOrdefault.DeliveryId);
                        var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(firstOrdefault.DeliveryId);
                        var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                        var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                        ViewInvoiceModel invoice = new ViewInvoiceModel
                        {
                            Client = client,
                            Order = orderInfo,
                            Delivery = delivery,
                            DeliveryDetails = deliveryDetails
                        };
                        model.InvoiceModel = invoice;

                    }
                }
                else
                {
                    models = _iProductReturnManager.GetGeneralReqReturnDetailsById(id).ToList();
                }

                model.ReturnModel = returnById;
                model.ReturnDetailses = models;
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        [HttpPost]
        public ActionResult ReceiveGeneralReturn(long salesReturnId, FormCollection collection)
        {

            try
            {

                var user = (ViewUser)Session["user"];
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                returnById.IsGeneralReturn = true;
                //List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                ICollection<ViewReturnDetails> returnDetailses = _iProductReturnManager.GetGeneralReturnDetailsByReturnId(salesReturnId);
                List<ViewReturnDetails> newReturnDetailsList = new List<ViewReturnDetails>();
                var firstOrdefault = returnDetailses.FirstOrDefault();

                if (firstOrdefault != null)
                {

                    int branchId = Convert.ToInt32(Session["BranchId"]);
                    var filePath = GetReceiveProductFilePath(salesReturnId, branchId);
                    //------------read Scanned barcode form text file---------
                    var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                    foreach (ViewReturnDetails item in returnDetailses)
                    {
                        item.UnitPrice = _iProductManager.GetProductDetailsByProductId(item.ProductId).UnitPrice;
                        newReturnDetailsList.Add(item);
                    }


                    var grossAmount = newReturnDetailsList.Sum(n => (n.UnitPrice + n.VatAmount) * n.Quantity);

                    var financialModel =
                        new FinancialTransactionModel
                        {
                            //--------Expence Cr -------------------
                            ExpenceCode = "2601011",
                            ExpenceAmount = grossAmount,
                            //--------Inventory Dr -------------------
                            InventoryCode = "3301011",
                            InventoryAmount = grossAmount


                        };

                    var result = _iInventoryManager.ReceiveProduct(barcodeList, branchId, user.UserId, financialModel, returnById);

                    if (result)
                    {
                        System.IO.File.Create(filePath).Close();
                        return RedirectToAction("PendingReturnList");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public void SaveGeneralReturnScannedBarcodeToTextFile(string barcode, long salesReturnId)
        {
            SuccessErrorModel model = new SuccessErrorModel();

            try
            {

                int branchId = Convert.ToInt32(Session["BranchId"]);
                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                var returndetailsbyId = _iProductReturnManager.GetGeneralReqReturnDetailsById(salesReturnId).FirstOrDefault();
                var filePath = GetReceiveProductFilePath(salesReturnId, branchId);
      
                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                //------------Load receiveable product---------
                var receivesProductList = _iProductReturnManager.GetGeneralReqReturnDetailsById(salesReturnId).ToList();
                List<ViewProduct> products = _iDeliveryManager.GetDeliveredProductListByTransactionRef(returndetailsbyId?.DeliveryRef).ToList();
                var receivesProductCodeList = products.Select(n => n.ProductBarCode).ToList();
                var isvalid = Validator.ValidateProductBarCode(scannedBarCode);

                int requistionQtyByProductId = receivesProductList.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity);

                int scannedQtyByProductId = barcodeList
                    .FindAll(n => Convert.ToInt32(n.ProductCode.Substring(2, 3)) == productId).Count;

                bool isScannComplete = requistionQtyByProductId.Equals(scannedQtyByProductId);

                if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scanned Complete</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (!isvalid)
                {
                    model.Message = "<p style='color:red'> Invalid Barcode</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }

                else if (receivesProductCodeList.Contains(scannedBarCode))
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                // return Json(model, JsonRequestBehavior.AllowGet);
            }
            // return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}