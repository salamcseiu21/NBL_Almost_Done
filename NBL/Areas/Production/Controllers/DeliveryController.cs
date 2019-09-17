using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Summaries;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "StoreManagerFactory")]
    public class DeliveryController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IFactoryDeliveryManager _iFactoryDeliveryManager;
        private readonly IBranchManager _iBranchManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IInvoiceManager _iInvoiceManager;

        private readonly IClientManager _iClientManager;
        // GET: Factory/Delivery
        public DeliveryController(IProductManager iProductManager,IFactoryDeliveryManager iFactoryDeliveryManager,IBranchManager iBranchManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IInvoiceManager iInvoiceManager,IClientManager iClientManager,IDeliveryManager iDeliveryManager)
        {
            _iProductManager = iProductManager;
            _iFactoryDeliveryManager = iFactoryDeliveryManager;
            _iBranchManager = iBranchManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
            _iDeliveryManager = iDeliveryManager;
        }
        public ActionResult DeliverableTransferIssueList() 
        {
            try
            {
                IEnumerable<TransferIssue> issueList = _iProductManager.GetDeliverableTransferIssueList();
                var model = new ViewTransferIssueModel();
                var transferIssues = issueList as TransferIssue[] ?? issueList.ToArray();
                foreach (var issue in transferIssues)
                {
                    model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                    model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
                    model.TransferIssues = transferIssues.ToList();
                }
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpGet]
        public ActionResult Delivery(long id)
        {

            try
            {
                var trip = _iInventoryManager.GetAllTrip().ToList().Find(n => n.TripId == id);
                var stock = _iInventoryManager.GetStockProductInFactory();
                Session["Factory_Stock"] = stock;

                return View(trip);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode,long tripId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
          
            try
            {
               
                var products = (List<ViewFactoryStockModel>) Session["Factory_Stock"];
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Deliverable_Product_For_" + tripId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedProducts(filePath);
                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Factory_Stock"] = products;
                    }
                }
                bool exists = barcodeList.Select(n=>n.ProductCode).Contains(scannedBarCode);
                bool isDeliveredBefore = _iInventoryManager.IsThisProductDispachedFromFactory(scannedBarCode);
                bool isInfactory = products.ToList().Select(n => n.ProductBarCode).Contains(scannedBarCode);
               // DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
               // var oldestProducts = products.ToList().FindAll(n=>n.ProductionDate<date && n.ProductId==productId).ToList();
                var issuedProducts = _iProductManager.GetDeliverableProductListByTripId(tripId);
              
                var isValied = Validator.ValidateProductBarCode(scannedBarCode);

                bool isContains = issuedProducts.Select(n => n.ProductId).Contains(productId);
                int reqQty = issuedProducts.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity);
                int scannedQty = barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(2, 3)) == productId).Count;
                bool isScannComplete =reqQty.Equals(scannedQty);
                bool isComplete = issuedProducts.Sum(n => n.Quantity).Equals(barcodeList.Count);

                if (!isContains)
                {
                    model.Message = "<p style='color:red'> Invalid Product Scanned.....</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }

               else if (exists)
                {
                    model.Message = "<p style='color:red'> Already Scanned.</p>";
                   // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                   //return Json(model, JsonRequestBehavior.AllowGet);
                }

               
                else if (isValied && !isDeliveredBefore && isInfactory && !isComplete)
                {
                   var result= _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                    if (result.Contains("Added"))
                    {
                        var p=  products.Find(n => n.ProductBarCode.Equals(scannedBarCode));
                        products.Remove(p);
                        Session["Factory_Stock"] = products;
                    }
                }
            }
            catch (FormatException exception)
            {
               
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>Invalid Barcode</p>";
             // return  Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
               
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
              //return  Json(model, JsonRequestBehavior.AllowGet);
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDispatchInformation(long tripId) 
        {
            try
            {
              
                string fileName = "Deliverable_Product_For_" + tripId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var scannedProducts = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

                if (scannedProducts.Count > 0)
                {
                    var products = _iProductManager.GetDeliverableProductListByTripId(tripId);
                    int dispatchByUserId = ((ViewUser)Session["user"]).UserId;
                    int companyId = Convert.ToInt32(Session["CompanyId"]);
                    var viewTrip = _iInventoryManager.GetAllTrip().ToList().Find(n => n.TripId == tripId);
                    viewTrip.Status = 2;
                    var tripQty = products.Sum(n => n.Quantity);
                    var deliveryQty = scannedProducts.Count;

                    if (tripQty != deliveryQty)
                    {
                        viewTrip.Status = 1;
                    }

                    DispatchModel model = new DispatchModel
                    {
                        DispatchByUserId = dispatchByUserId,
                        CompanyId = companyId,
                        TripModel = viewTrip,
                        DispatchDate = DateTime.Now,
                        ScannedProducts = scannedProducts,
                        DispatchModels = products

                    };

                    string result = _iFactoryDeliveryManager.SaveDispatchInformation(model);
                    if (result.StartsWith("Sa"))
                    {
                        System.IO.File.Create(filePath).Close();
                        //---------------Send mail to branch before redirect--------------
                        TempData["Dispatched"] = result;
                        return RedirectToAction("DeliverableTransferIssueList");
                    }
                    return RedirectToAction("Delivery", new { id = tripId });
                }

                return RedirectToAction("Delivery", new { id = tripId });
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public ActionResult Chalan(long dispatchId)
        {
            try
            {
                ViewDispatchChalan chalan = _iFactoryDeliveryManager.GetDispatchChalanByDispatchId(dispatchId);
               
                return View(chalan);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult DispatchedBarCodeList(long dispatchId)
        {
            try
            {
                ViewDispatchChalan chalan = _iFactoryDeliveryManager.GetDispatchChalanByDispatchId(dispatchId);

                return View(chalan);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public PartialViewResult ViewOrderDetails(int transferIssueId) 
        {


            try
            {
                TransferIssue model = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
                return PartialView("_ViewDeliveryModalPartialPage", model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
          
        }

        [HttpPost]
        public PartialViewResult LoadDeliverableProduct(long tripId)
        {

            try
            {
                var products = _iProductManager.GetDeliverableProductListByTripId(tripId).ToList();
                return PartialView("_ViewRequiredTripProductsPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public PartialViewResult LoadScannecdProduct(long tripId) 
        {
            try
            {
                List<ScannedProduct> products = new List<ScannedProduct>();
                string fileName = "Deliverable_Product_For_" + tripId;
                var filePath = Server.MapPath("~/Files/" + fileName);
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
        //---------------For dispatch----------
        public ActionResult ViewScannedBarcodeList(long id)
        {
            try
            {
                string fileName = "Deliverable_Product_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                return PartialView("_ViewScannedBarCodePartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult TripList()
        {
            try
            {
                IEnumerable<ViewTripModel> tripModels = _iInventoryManager.GetAllDeliverableTripList();
                return View(tripModels);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //----------------------------Delivery to Client ---------------------
        public ActionResult DeliverableOrderList()
        {
            try
            {
                SummaryModel model = new SummaryModel();
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByDistributionPoint(branchId).ToList();
                foreach (var invoice in invoicedOrders)
                {
                    invoice.Client = _iClientManager.GetById(invoice.ClientId);
                }
                model.InvoicedOrderList = invoicedOrders;
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult OrderDelivery(int id)
        {



            try
            {
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                var stock = _iInventoryManager.GetStockProductInFactory();
                Session["Factory_stock1"] = stock;
                var model = new ViewDeliveryModel
                {
                    Client = _iClientManager.GetById(invoice.ClientId),
                    InvoiceDetailses = invoicedOrders,
                    Invoice = invoice
                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        [HttpPost]
        public ActionResult OrderDelivery(FormCollection collection) 
        {
            try
            {

                var transport = collection["ownTransport"];
                bool isOwnTransport = transport != null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int invoiceId = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                IEnumerable<InvoiceDetails> details = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceId(invoiceId);
                var client = _iClientManager.GetClientDeailsById(invoice.ClientId);
                var deliveredQty = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef).Count;
                var remainingToDeliverQty = invoice.Quantity - deliveredQty;
                string fileName = "Scanned_Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();


                int invoiceStatus = Convert.ToInt32(InvoiceStatus.PartiallyDelivered);
                int orderStatus = Convert.ToInt32(OrderStatus.PartiallyDelivered);
                if (remainingToDeliverQty == barcodeList.Count)
                {
                    invoiceStatus = Convert.ToInt32(InvoiceStatus.Delivered);
                    orderStatus = Convert.ToInt32(OrderStatus.Delivered);
                }

                List<InvoiceDetails> deliveredProductList = new List<InvoiceDetails>();
                foreach (ScannedProduct product in barcodeList.DistinctBy(n => n.ProductId))
                {

                    var invoiceDetails = details.ToList().Find(n => n.ProductId.Equals(product.ProductId));
                    var qty = barcodeList.ToList().FindAll(n => n.ProductId == product.ProductId).Count;
                    invoiceDetails.Quantity = qty;
                    deliveredProductList.Add(invoiceDetails);
                }
                //-----------------Credit sale account code =1001021 ---------------
                //financialModel.InvoiceDiscountCode = "2102011";
                //financialModel.InvoiceDiscountAmount = (invoice.SpecialDiscount/invoice.Quantity)*barcodeList.Count;
                //-----------------Credit vat account code =2102013 ---------------
                //-----------------Credit invoice discount account code =2102012 ---------------

                //var up=   deliveredProductList.Sum(n => n.UnitPrice);
                // var discount = deliveredProductList.Sum(n => n.Discount);


                var grossAmount = deliveredProductList.Sum(n => (n.UnitPrice + n.Vat) * n.Quantity);
                var tradeDiscount = deliveredProductList.Sum(n => n.Discount * n.Quantity);
                var invoiceDiscount = (invoice.SpecialDiscount / invoice.Quantity) * barcodeList.Count;
                var grossDiscount = tradeDiscount + invoiceDiscount;
                var vat = deliveredProductList.Sum(n => n.Vat * n.Quantity);


                var financialModel =
                    new FinancialTransactionModel
                    {
                        //--------Dr -------------------
                        ClientCode = client.SubSubSubAccountCode,
                        ClientDrAmount = grossAmount - grossDiscount,
                        GrossDiscountAmount = grossDiscount,
                        GrossDiscountCode = "2102018",

                        //--------Cr -------------------
                        SalesRevenueCode = "1001021",
                        SalesRevenueAmount = grossAmount - vat,
                        VatCode = "2102013",
                        VatAmount = vat,

                        TradeDiscountCode = "2102012",
                        TradeDiscountAmount = tradeDiscount,
                        InvoiceDiscountAmount = invoiceDiscount,
                        InvoiceDiscountCode = "2102011"


                    };


                var aDelivery = new Delivery
                {
                    IsOwnTransport = isOwnTransport,
                    TransactionRef = invoice.TransactionRef,
                    InvoiceRef = invoice.InvoiceRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = invoice.CompanyId,
                    ToBranchId = invoice.BranchId,
                    InvoiceId = invoiceId,
                    FromBranchId = invoice.BranchId,
                    FinancialTransactionModel = financialModel
                };
                string result = _iInventoryManager.SaveDeliveredOrderFromFactory(barcodeList, aDelivery, invoiceStatus, orderStatus);
                if (result.StartsWith("S"))
                {
                    System.IO.File.Create(filePath).Close();
                    return RedirectToAction("DeliverableOrderList");
                }
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public void SaveScannedBarcode(string barcode, int invoiceId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewFactoryStockModel> products = (List<ViewFactoryStockModel>)Session["Factory_stock1"];
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Scanned_Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedProducts(filePath);

                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Factory_stock1"] = products;
                    }
                }

                // DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
                // var oldestProducts = products.ToList().FindAll(n => n.ProductionDate < date && n.ProductId == productId).ToList();
                bool isInInventory = products.Select(n => n.ProductBarCode).Contains(scannedBarCode);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);

                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);
                //------------Get invoced products-------------
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                List<InvoiceDetails> list = new List<InvoiceDetails>();
                var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

                foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
                {
                    var invoiceQty = invoiceDetailse.Quantity;
                    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Count;
                    if (invoiceQty != deliveredQty)
                    {
                        invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                        list.Add(invoiceDetailse);
                    }

                }
                bool isValied = list.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = list.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity) == barcodeList.FindAll(n => n.ProductId == productId).Count;
                if (isScannedBefore)
                {
                    model.Message = "<p style='color:red'> Already Scanned</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }

                //else if (oldestProducts.Count > 0)
                //{
                //    model.Message = "<p style='color:red'>There are total " + oldestProducts.Count + " Old product of this type .Please deliver those first .. </p>";
                //   // return Json(model, JsonRequestBehavior.AllowGet);
                //}
                else if (isSold)
                {
                    model.Message = "<p style='color:green'> This product Scanned for one of previous invoice... </p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isValied && isInInventory)
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                Log.WriteErrorLog(exception);
              
                // return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
             
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            // return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScannedBarcodeList(long id)
        {
            try
            {
                string fileName = "Scanned_Ordered_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                return PartialView("_ViewScannedBarCodePartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ViewInvoiceIdOrderDetails(int invoiceId)
        {
            try
            {
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                return PartialView("_ModalOrderDeliveryFromFactoryPartialPage", invoice);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public PartialViewResult LoadDeliverableProductToFactory(int invoiceId)
        {
            try
            {
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                string fileName = "Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    //if the file does not exists create the file
                    System.IO.File.Create(filePath).Close();
                }
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                List<InvoiceDetails> list = new List<InvoiceDetails>();

                var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

                foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
                {
                    var invoiceQty = invoiceDetailse.Quantity;
                    var deliveredQty = 0;
                    var viewProduct = deliveredProducts.ToList().Find(n => n.ProductId == invoiceDetailse.ProductId);
                    if (viewProduct != null)
                    {
                        deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Sum(n => n.Quantity);
                    }

                    if (invoiceQty != deliveredQty)
                    {
                        invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                        list.Add(invoiceDetailse);
                    }
                }

                return PartialView("_ViewLoadDeliverableProductPartialPage", list);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        [HttpPost]
        public PartialViewResult LoadScannedProduct(int invoiceId)
        {

            try
            {
                string fileName = "Scanned_Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                List<ScannedProduct> list = new List<ScannedProduct>();
                if (!System.IO.File.Exists(filePath))
                {
                    //if the file does not exists create the file
                    System.IO.File.Create(filePath).Close();
                }
                else
                {
                    list = _iProductManager.ScannedProducts(filePath);
                }
                return PartialView("_ViewLoadScannedProductPartialPage", list);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult DeliveredOrderList()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var user = (ViewUser)Session["user"];
                var orders = _iDeliveryManager.GetAllDeliveredOrdersByDistributionPointCompanyAndUserId(branchId, companyId, user.UserId).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult DeliveredBarCodeList(int deliveryId)
        {
            try
            {
                var chalan = _iDeliveryManager.GetChalanByDeliveryIdFromFactory(deliveryId);
                return View(chalan);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult OrderChalan(int deliveryId)
        {
            try
            {
                var chalan = _iDeliveryManager.GetChalanByDeliveryIdFromFactory(deliveryId);
                return View(chalan);

            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}