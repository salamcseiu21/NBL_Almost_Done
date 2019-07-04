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
        public WarrantyBatteryController(IInventoryManager iInventoryManager,ICommonManager iCommonManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
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
                    ChargingStatus = _iCommonManager.GetAllCharginStatus().ToList()
                    
                };
                return View(model);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                throw;
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
    }
}