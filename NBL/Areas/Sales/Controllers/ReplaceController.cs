
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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

        public ReplaceController(IProductManager iProductManager,IProductReplaceManager iProductReplaceManager,IInventoryManager iInventoryManager,IDeliveryManager iDeliveryManager)
        {
            _iProductManager = iProductManager;
            _iProductReplaceManager = iProductReplaceManager;
            _iInventoryManager = iInventoryManager;
            _iDeliveryManager = iDeliveryManager;
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
        [HttpPost]
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {

                var transport = collection["ownTransport"];
                bool isOwnTransport = transport != null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int replaceId = Convert.ToInt32(collection["ReplaceId"]);
                var replace = _iProductReplaceManager.GetReplaceById(replaceId);
                IEnumerable<ViewReplaceDetailsModel> details = _iProductReplaceManager.GetReplaceProductListById(replaceId);
                var deliveredQty = _iProductReplaceManager.GetDeliveredProductsByReplaceRef(replace.ReplaceRef).Count;
                var remainingToDeliverQty = replace.Quantity - deliveredQty;
                var filePath =GetTempReplaceProductXmlFilePath(replaceId);
                //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                int replaceStatus = 1;
                if (remainingToDeliverQty == barcodeList.Count)
                {
                    replaceStatus = 2;
                   
                }

                List<ViewReplaceDetailsModel> deliveredProductList = new List<ViewReplaceDetailsModel>();
                foreach (ScannedProduct product in barcodeList.DistinctBy(n => n.ProductId))
                {
                    var model = details.ToList().Find(n => n.ProductId.Equals(product.ProductId));
                    var qty = barcodeList.ToList().FindAll(n => n.ProductId == product.ProductId).Count;
                    model.Quantity = qty;
                    deliveredProductList.Add(model);
                }

                var aDelivery = new Delivery
                {
                    IsOwnTransport = isOwnTransport,
                    TransactionRef = replace.ReplaceRef,
                    InvoiceRef = replace.ReplaceRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = replace.CompanyId,
                    ToBranchId = replace.BranchId,
                    InvoiceId = replaceId,
                    FromBranchId = replace.BranchId 
                };
                string result = _iInventoryManager.SaveReplaceDeliveryInfo(barcodeList, aDelivery,replaceStatus);
                if (result.StartsWith("S"))
                {
                    System.IO.File.Create(filePath).Close();
                    return RedirectToAction("ViewAll");
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

        public ActionResult AllReplaceLsit()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<ReplaceReport> mReplaceReports = _iProductReplaceManager.GetAllReplaceListByBranchId(branchId);
            return View(mReplaceReports);
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
        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode, long replaceId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewBranchStockModel> products = (List<ViewBranchStockModel>)Session["Branch_stock"];
                var id = replaceId;
                var replace = _iProductReplaceManager.GetReplaceById(id); 
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                var filePath = GetTempReplaceProductXmlFilePath(replaceId);
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
                var replaceDetails = _iProductReplaceManager.GetReplaceProductListById(id).ToList();
                List<ViewReplaceDetailsModel> list = new List<ViewReplaceDetailsModel>();
                var deliveredProducts = _iProductReplaceManager.GetDeliveredProductsByReplaceRef(replace.ReplaceRef);

                foreach (var item in replaceDetails)
                {
                    var replaceQty = item.Quantity;
                    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == item.ProductId).Count;
                    if (replaceQty != deliveredQty)
                    {
                        item.Quantity = replaceQty - deliveredQty;
                        list.Add(item);
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
        public PartialViewResult LoadScannedProduct(long replaceId) 
        {

            try
            {
                var filePath = GetTempReplaceProductXmlFilePath(replaceId);
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


        private string GetTempReplaceProductXmlFilePath(long replaceId)
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Replaced_Product_List_For_" + branchId + user.UserId + "_"+replaceId;
            var filePath = Server.MapPath("~/Areas/Sales/Files/Replaces/" + fileName);
            return filePath;
        }

        public PartialViewResult ViewReplaceDetails(long replaceId)
        {
            try
            {
                var replace = _iProductReplaceManager.GetReplaceById(replaceId);
                return PartialView("_ModalReplaceDeliveryPartialPage", replace);
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