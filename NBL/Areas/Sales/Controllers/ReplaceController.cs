
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Sales;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "DistributionManager")]
    public class ReplaceController : Controller
    {

         private readonly IProductManager _iProductManager;
         private readonly IProductReplaceManager _iProductReplaceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IClientManager _iClientManager;
        public ReplaceController(IProductManager iProductManager,IProductReplaceManager iProductReplaceManager,IInventoryManager iInventoryManager,IDeliveryManager iDeliveryManager,IServiceManager iServiceManager,IClientManager iClientManager)
        {
            _iProductManager = iProductManager;
            _iProductReplaceManager = iProductReplaceManager;
            _iInventoryManager = iInventoryManager;
            _iDeliveryManager = iDeliveryManager;
            _iServiceManager = iServiceManager;
            _iClientManager = iClientManager;
        }


        // GET: Sales/Replace
        public ActionResult Home() 
        {
            return View();
        }
       
        public ActionResult Delivery(long id)
        {
            try
            {
                ViewReplaceModel model=new ViewReplaceModel();
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var stock = _iInventoryManager.GetStockProductInBranchByBranchAndCompanyId(branchId, companyId);
                var received = _iServiceManager.GetDeliverableServiceProductById(id);  
                Session["Branch_stock"] = stock;
                var client= _iClientManager.GetById(received.ClientId);
                var products = new List<ViewReplaceDetailsModel>
                {
                    new ViewReplaceDetailsModel
                    {
                        ExpiryDate = received.ExpiryDate,
                        ProductId = received.ProductId,
                        ProductName = received.ProductName,
                        Quantity = 1
                    }
                };
                model.ClientCode = client.SubSubSubAccountCode;
                model.ClientName = client.ClientName;
                model.ClientAddress = client.Address;
                model.Products = products;
                model.ReceiveId = id;
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var transport = collection["ownTransport"];
                bool isOwnTransport = transport != null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int receiveId = Convert.ToInt32(collection["ReceiveId"]);   
                var replace = _iProductReplaceManager.GetReplaceById(receiveId);
                var received = _iServiceManager.GetDeliverableServiceProductById(receiveId);
                received.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                var filePath =GetTempReplaceProductXmlFilePath(receiveId);
                //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                var aDelivery = new Delivery
                {
                    IsOwnTransport = isOwnTransport,
                    TransactionRef = received.ReceiveRef,
                    InvoiceRef = received.ReceiveRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = received.CompanyId,
                    ToBranchId = received.BranchId,
                    DistributionPointId = branchId,
                    InvoiceId = received.ReceiveId,
                    FromBranchId = received.BranchId 
                };
                string result = _iInventoryManager.SaveReplaceDeliveryInfo(barcodeList, aDelivery,2,replace);
                if (result.StartsWith("S"))
                {
                    System.IO.File.Create(filePath).Close();
                    return RedirectToAction("WarrantyReplaceList");
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
        public void SaveScannedBarcodeToTextFile(string barcode, long receiveId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewBranchStockModel> products = (List<ViewBranchStockModel>)Session["Branch_stock"];
               // var id = receiveId;
                // var replace = _iProductReplaceManager.GetReplaceById(id);
                var product = _iServiceManager.GetDeliverableServiceProductById(receiveId);
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                var filePath = GetTempReplaceProductXmlFilePath(receiveId);
                var barcodeList = _iProductManager.ScannedProducts(filePath);

                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Branch_stock"] = products;
                    }
                }

                // DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
                // var oldestProducts = products.ToList().FindAll(n => n.ProductionDate < date && n.ProductId == productId).ToList();
                bool isInInventory = products.Select(n => n.ProductBarCode).Contains(scannedBarCode);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);

                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);
                //------------Get invoced products-------------
               // var replaceDetails = _iProductReplaceManager.GetReplaceProductListById(id).ToList();
               // List<ViewReplaceDetailsModel> list = new List<ViewReplaceDetailsModel>();
                //var deliveredProducts = _iProductReplaceManager.GetDeliveredProductsByReplaceRef(product.ReceiveRef);

                var list = new List<ViewReplaceDetailsModel>
                {
                    new ViewReplaceDetailsModel
                    {
                        ExpiryDate = product.ExpiryDate,
                        ProductId = product.ProductId,
                        SaleDate = product.SaleDate,
                        ProductName = product.ProductName,
                        Quantity = 1
                    }
                };

                //foreach (var item in replaceDetails)
                //{
                //    var replaceQty = item.Quantity;
                //    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == item.ProductId).Count;
                //    if (replaceQty != deliveredQty)
                //    {
                //        item.Quantity = replaceQty - deliveredQty;
                //        list.Add(item);
                //    }

                //}
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

        [HttpPost]
        public PartialViewResult LoadScannedProduct(long receiveId)
        {

            try
            {
                var filePath = GetTempReplaceProductXmlFilePath(receiveId);
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


        private string GetTempReplaceProductXmlFilePath(long receiveId)
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Replaced_Product_List_For_" + branchId + user.UserId + "_" + receiveId;
            var filePath = Server.MapPath("~/Areas/Sales/Files/Replaces/" + fileName);
            return filePath;
        }
        public ActionResult DeliveredList()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //-------------Status=2 means Delivered.-------------
                ICollection<ViewReplaceModel> replace = _iProductReplaceManager.GetAllDeliveredReplaceListByBranchAndCompany(branchId, companyId).ToList();
                return View(replace);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        //----------------Warranty battery replace list------------------
        public ActionResult WarrantyReplaceList()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            //--------------Status=1 pending for delivery-------------
            var products = _iServiceManager.GetReceivedServiceProductsByStatusAndBranchId(1,branchId);
            return View(products);
        }

        public ActionResult WarrantyDelivery(long id)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var stock = _iInventoryManager.GetStockProductInBranchByBranchAndCompanyId(branchId, companyId);
                Session["Branch_stock"] = stock;
                ViewReplaceModel model = _iProductReplaceManager.GetReplaceById(id);
                List<ViewReplaceDetailsModel> products = _iProductReplaceManager.GetReplaceProductListById(id).ToList();
                model.Products = products; 
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult AllReplaceLsit()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<ReplaceReport> mReplaceReports = _iProductReplaceManager.GetAllReplaceListByBranchId(branchId);
            return View(mReplaceReports);
        }
        public ActionResult PrintChallan(long id)
        {
            ViewReplaceModel model = _iProductReplaceManager.GetReplaceById(id);
            List<ViewReplaceDetailsModel> products = _iProductReplaceManager.GetReplaceProductListById(id).ToList();
            model.Products = products;
            return View(model);
        }
        public ActionResult ViewAll()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //-------------Status=0 means pending.-------------
                var replace = _iProductReplaceManager.GetAllPendingReplaceListByBranchAndCompany(branchId, companyId).ToList();
                return View(replace);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        

        public PartialViewResult ViewReplaceDetails(long receiveId)
        {
            try
            {
                var product = _iServiceManager.GetDeliverableServiceProductById(receiveId);
                return PartialView("_ModalReplaceDeliveryPartialPage", product);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //public ActionResult DeliveredBarCodeList(int deliveryId)
        //{
        //    var challan = _iDeliveryManager.GetDeliveredReplaceBarcodeListbyDeliveryId(deliveryId); 
        //    return View(challan);

        //}

        public ActionResult ReplaceList()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<ReplaceReport> mReplaceReports = _iProductReplaceManager.GetTodaysReplaceListByBranchId(branchId);
            return View(mReplaceReports);

        }

        
    }
}