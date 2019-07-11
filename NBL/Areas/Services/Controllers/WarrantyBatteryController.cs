using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.Services.Controllers
{
    [Authorize(Roles = "ServiceExecutive")]
    public class WarrantyBatteryController : Controller
    {
        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IBranchManager _iBranchManager;
        public WarrantyBatteryController(IInventoryManager iInventoryManager,ICommonManager iCommonManager,IServiceManager iServiceManager,IBranchManager iBranchManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iServiceManager = iServiceManager;
            _iBranchManager = iBranchManager;
        }

        public ActionResult All()
        {

            try
            {
                var products = _iServiceManager.GetReceivedServiceProducts();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }

        public ActionResult Details(long id)
        {
            try
            {
                var product= _iServiceManager.GetReceivedServiceProductById(id);
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        // GET: Services/WarrantyBattery
        public ActionResult Receive()
        {
            try
            {
                WarrantyBatteryModel model = new WarrantyBatteryModel
                {
                    PhysicalConditions = _iCommonManager.GetAllPhysicalConditions().ToList(),
                    ServicingModels = _iCommonManager.GetAllServicingStatus().ToList(),
                    ChargingStatus = _iCommonManager.GetAllCharginStatus().ToList(),
                    ForwardToModels = _iCommonManager.GetAllForwardToModels().ToList(),
                    DistributionPoints = _iBranchManager.GetAllBranches().ToList()
                    
                    
                };
                return View(model);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                throw;
            }

        }
        [HttpPost]
        public ActionResult Receive(WarrantyBatteryModel model)
        {
            try
            {

                var product = _iInventoryManager.GetProductHistoryByBarcode(model.Barcode) ?? new ViewProductHistory();
                var user = (ViewUser) Session["user"];
                var branchId = Convert.ToInt32(Session["BranchId"]);
                model.EntryByUserId = user.UserId;
                model.ReportByEmployeeId = branchId;
                model.ReceiveDatetime=DateTime.Now;
                model.DelivaryRef = product.DeliveryRef;
                model.TransactionRef = product.OrderRef;
                model.Status = 0;
                var result = _iServiceManager.ReceiveServiceProduct(model);
                if (result)
                {
                    ModelState.Clear();
                    return RedirectToAction("All");
                }
                model.PhysicalConditions = _iCommonManager.GetAllPhysicalConditions().ToList();
                model.ServicingModels = _iCommonManager.GetAllServicingStatus().ToList();
                model.ChargingStatus = _iCommonManager.GetAllCharginStatus().ToList();
                return Receive(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return View(model);
            }
        }

        //--------------Return Json----------------
        [HttpPost]
        public JsonResult GetProductHistoryByBarcode(string barcode)
        {
            var product = _iInventoryManager.GetProductHistoryByBarcode(barcode) ?? new ViewProductHistory();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ServiceDurationByDate(DateTime dealerReceiveDate,string barcode)  
        {


            var product = _iInventoryManager.GetProductHistoryByBarcode(barcode) ?? new ViewProductHistory();
            product.ServiceDuration = Convert.ToInt32((dealerReceiveDate - Convert.ToDateTime(product.SaleDate)).TotalDays-1);
            product.CollectionDuration = Convert.ToInt32((Convert.ToDateTime(product.SaleDate)- dealerReceiveDate).TotalDays-1);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        //--------------Return partial View----------------
        //[HttpPost]
        //public PartialViewResult GetProductHistoryByBarcode(string barcode)
        //{
        //    var product = _iInventoryManager.GetProductHistoryByBarcode(barcode);
        //    return PartialView("_ViewProductHistoryPartialPage",product);

        //}

        //--------------Set data to Temp----------------
        //[HttpPost]
        //public void GetProductHistoryByBarcode(string barcode)
        //{
        //    var product = _iInventoryManager.GetProductHistoryByBarcode(barcode);
        //    //return PartialView("_ViewProductHistoryPartialPage", product);
        //    TempData["ProductHistory"] = product;
        //}

        //----------------Cell Condition Auto Complete------------------
        [HttpPost]
        public JsonResult CellConditionAutoComplete(string prefix)
        {
            ICollection<object> conditionList = _iCommonManager.GetCellConditionBySearchTerm(prefix);
            return Json(conditionList);
        }
    }
}