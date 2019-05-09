using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Logs;
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

        private readonly IInvoiceManager _iInvoiceManager;

        private readonly IClientManager _iClientManager;
        // GET: Factory/Delivery
        public DeliveryController(IProductManager iProductManager,IFactoryDeliveryManager iFactoryDeliveryManager,IBranchManager iBranchManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IInvoiceManager iInvoiceManager,IClientManager iClientManager)
        {
            _iProductManager = iProductManager;
            _iFactoryDeliveryManager = iFactoryDeliveryManager;
            _iBranchManager = iBranchManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
        }
        public ActionResult DeliverableTransferIssueList() 
        {
            IEnumerable<TransferIssue> issueList = _iProductManager.GetDeliverableTransferIssueList();
            var model=new ViewTransferIssueModel();
            var transferIssues = issueList as TransferIssue[] ?? issueList.ToArray();
            foreach (var issue in transferIssues)
            {
                model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
                model.TransferIssues = transferIssues.ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delivery(long id)
        {
             
            var trip = _iInventoryManager.GetAllTrip().ToList().Find(n=>n.TripId==id);
            var stock=_iInventoryManager.GetStockProductInFactory();
            Session["Factory_Stock"] = stock;
             
            return View(trip);
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode,long tripId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            ViewWriteLogModel log=new ViewWriteLogModel();
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

                //if (oldestProducts.Count > 0)
                //{
                //    model.Message = "<p style='color:red'>There are total "+oldestProducts.Count+" Old product of this type .Please deliver those first .. </p>";
                //    return Json(model, JsonRequestBehavior.AllowGet);
                //}
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
                log.Heading = exception.GetType().ToString();
                log.LogMessage = exception.StackTrace;
                Log.WriteErrorLog(log);
                model.Message = "<p style='color:red'>Invalid Barcode</p>";
             // return  Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                log.Heading = exception.GetType().ToString();
                log.LogMessage = exception.StackTrace;
                Log.WriteErrorLog(log);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
              //return  Json(model, JsonRequestBehavior.AllowGet);
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDispatchInformation(long tripId) 
        {
            var products = _iProductManager.GetDeliverableProductListByTripId(tripId);
            string fileName = "Deliverable_Product_For_" + tripId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            var scannedProducts = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            int dispatchByUserId = ((ViewUser)Session["user"]).UserId;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var viewTrip=_iInventoryManager.GetAllTrip().ToList().Find(n => n.TripId == tripId);

            DispatchModel model=new DispatchModel
            {
                DispatchByUserId = dispatchByUserId,
                CompanyId = companyId,
                TripModel = viewTrip,
                DispatchDate = DateTime.Now,
                ScannedProducts = scannedProducts,
                DispatchModels = products
            };


            if (scannedProducts.Count>0)
            {
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
           
            return RedirectToAction("Delivery",new {id= tripId });
        }

        public PartialViewResult ViewOrderDetails(int transferIssueId) 
        {

            TransferIssue model = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
            return PartialView("_ViewDeliveryModalPartialPage", model);
        }

        [HttpPost]
        public PartialViewResult LoadDeliverableProduct(long tripId)
        {
           
            var products = _iProductManager.GetDeliverableProductListByTripId(tripId).ToList();
           
            return PartialView("_ViewRequiredTripProductsPartialPage",products);
        }

        [HttpPost]
        public PartialViewResult LoadScannecdProduct(long tripId) 
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

        public ActionResult ViewScannedBarcodeList(long id)
        {

            List<ScannedProduct> products = new List<ScannedProduct>();
            string fileName = "Deliverable_Product_For_" + id;
            var filePath = Server.MapPath("~/Files/" + fileName);
            products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            return View(products);
        }

        public ActionResult TripList()
        {
            IEnumerable<ViewTripModel> tripModels = _iInventoryManager.GetAllTrip().ToList().FindAll(n=>n.Status.Equals(0));
            return View(tripModels);
        }

        //----------------------------Delivery to Client ---------------------
        public ActionResult DeliverableOrderList()
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

        public ActionResult OrderDelivery(long id)
        {
            return View();
        }
    }
}