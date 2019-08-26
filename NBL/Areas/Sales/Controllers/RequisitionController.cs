using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.Sales;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "DistributionManager")]
    public class RequisitionController : Controller
    {
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;

        public RequisitionController(IDeliveryManager iDeliveryManager, IInventoryManager iInventoryManager, IProductManager iProductManager, IClientManager iClientManager, IInvoiceManager iInvoiceManager, ICommonManager iCommonManager, IOrderManager iOrderManager, IBranchManager iBranchManager,IEmployeeManager iEmployeeManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
            _iCommonManager = iCommonManager;
            _iOrderManager = iOrderManager;
            _iBranchManager = iBranchManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: Sales/Requisition
        public ActionResult GeneralRequisitionList()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n => n.DistributionPointId == branchId).FindAll(n => n.Status.Equals(2)).ToList().FindAll(n => n.DeliveryStatus.Equals(0));
                return PartialView("_ViewGeneralRequisitionList", requisitions);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Delivery(long id)
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var stock = _iInventoryManager.GetStockProductInBranchByBranchAndCompanyId(branchId, companyId);
                Session["Branch_stock"] = stock;
                ICollection<ApprovalDetails> approval = _iCommonManager.GetAllApprovalDetailsByRequistionId(id);
                var model = new ViewGeneralRequisitionModel
                {
                    GeneralRequistionDetails = _iProductManager.GetGeneralRequisitionDetailsById(id),
                    GeneralRequisitionModel = _iProductManager.GetGeneralRequisitionById(id),
                    ApprovalDetails = approval

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
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {

                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var transport = collection["ownTransport"];
                bool isOwnTransport = transport != null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                var requisitionId = Convert.ToInt64(collection["RequisitionId"]);
                var requisition = _iProductManager.GetGeneralRequisitionById(requisitionId);
                var details = _iProductManager.GetGeneralRequisitionDetailsById(requisitionId);
               
                string fileName = "Scanned_GR_Product_List_For_" + requisitionId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

                var deliveredProductList = new List<ViewGeneralRequistionDetailsModel>();
                foreach (ScannedProduct product in barcodeList.DistinctBy(n => n.ProductId))
                {

                    var model = details.ToList().Find(n => n.ProductId.Equals(product.ProductId));
                    var up= _iProductManager.GetProductDetailsByProductId(product.ProductId).UnitPrice;
                    var qty = barcodeList.ToList().FindAll(n => n.ProductId == product.ProductId).Count;
                    model.Quantity = qty;
                    model.UnitPrice = up;
                    deliveredProductList.Add(model);
                }
                

                var grossAmount = deliveredProductList.Sum(n => (n.UnitPrice + n.Vat) * n.Quantity);
         

                var financialModel =
                    new FinancialTransactionModel
                    {
                        //--------Expence Dr -------------------
                        ExpenceCode = "2601011",
                        ExpenceAmount = grossAmount,
                        //--------Inventory Cr -------------------
                        InventoryCode = "3301011",
                        InventoryAmount = grossAmount


                    };


                var aDelivery = new Delivery
                {
                    IsOwnTransport = isOwnTransport,
                    TransactionRef = requisition.RequisitionRef,
                    InvoiceRef = requisition.RequisitionRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = companyId,
                    ToBranchId = branchId,
                    RequisitionId = requisitionId,
                    FromBranchId = branchId,
                    FinancialTransactionModel = financialModel
                };
                string result = _iInventoryManager.SaveDeliveredGeneralRequisition(barcodeList, aDelivery);
                if (result.StartsWith("S"))
                {
                    System.IO.File.Create(filePath).Close();
                    return RedirectToAction("GeneralRequisitionList");
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
        public void SaveScannedBarcodeToTextFile(string barcode, long requisitionId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewBranchStockModel> products = (List<ViewBranchStockModel>)Session["Branch_stock"];
               // var invoice = _iProductManager.GetGeneralRequisitionById(requisitionId);
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Scanned_GR_Product_List_For_" + requisitionId;
                var filePath = Server.MapPath("~/Files/" + fileName);
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


               //var list1=  products.FindAll(n => n.ProductId == productId);
                bool isInInventory = products.Select(n => n.ProductBarCode).Contains(scannedBarCode);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);

                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);
                //------------Get invoced products-------------
                var requisitionsById = _iProductManager.GetGeneralRequisitionDetailsById(requisitionId);
                List<InvoiceDetails> list = new List<InvoiceDetails>();
                
                bool isValied = requisitionsById.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = requisitionsById.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity) == barcodeList.FindAll(n => n.ProductId == productId).Count;
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
                Log.WriteErrorLog(exception);
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

        public PartialViewResult ViewScannedBarcodeList(long id)
        {
            try
            {
                List<ScannedProduct> products = new List<ScannedProduct>();
                string fileName = "Scanned_GR_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                return PartialView("_ViewScannedBarCodePartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult LoadDeliverableProduct(long requisitionId)
        {
            try
            {
                var details = _iProductManager.GetGeneralRequisitionDetailsById(requisitionId);
                return PartialView("_ViewGeneralRequisitionDetailsPartialPage", details);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public PartialViewResult LoadScannedProduct(long requisitionId)
        {

            try
            {
                string fileName = "Scanned_GR_Product_List_For_" + requisitionId;
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
        public ActionResult ViewGeneralRequisitionDetails(long reqisitionId)
        {
            try
            {
                var requisition = _iProductManager.GetGeneralRequisitionById(reqisitionId);
                return PartialView("_ModalRequisitionDeliveryPartialPage", requisition);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult DeliveredGRequsition()
        {

            List<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetAllDeliveredGRequsition().ToList();
            return View(requisitions);
        }
    }
}