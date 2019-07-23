using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
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

        public ActionResult ReplaceList()
        {
            try
            {
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.Replace));
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ReturnList()
        {
            try
            {
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.Return));
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ProductInChargeSection()
        {
            try
            {
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.Charge));
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


    

        public ActionResult ChargeReport(long id)
        {

            try
            {

                var user = (ViewUser)Session["user"];
                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "WarrantyBattery",
                        "ChargeReport");
                var product = _iServiceManager.GetReceivedServiceProductById(id);
                DischargeReportModel dischargeReportModel = _iServiceManager.GetDisChargeReprortByReceiveId(id);
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager
                    .GetAllForwardToModelsByUserAndActionId(user.UserId, actionModel.Id).ToList();
                product.DischargeReportModel = dischargeReportModel;
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [HttpPost]
        public ActionResult ChargeReport(ChargeReportModel model,long id)
        {

            try
            {

                var user = (ViewUser)Session["user"];
                var product = _iServiceManager.GetReceivedServiceProductById(id);
                ForwardDetails forward=new ForwardDetails
                {
                    UserId = user.UserId,
                    ForwardDateTime = DateTime.Now,
                    ForwardFromId = product.ForwardedToId,
                    ForwardToId = model.ForwardToId,
                    ReceiveId = model.BatteryReceiveId,
                    ForwardRemarks = model.ForwardRemarks
                };

                
                if (model.ForwardToId == product.ForwardedToId)
                {
                    model.ParentId = model.ForwardToId;
                }
                model.EntryByUserId = user.UserId;
                model.ForwardDetails = forward;


                bool result = _iServiceManager.SaveCharegeReport(model);
                if (result)
                {
                    return RedirectToAction("ProductInChargeSection");
                }
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModels().ToList();
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }


        public ActionResult ProductInDisChargeSection()
        {
            try
            {
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.DischargeTest));
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult DisChargeReport(long id)
        {

            try
            {
                var user = (ViewUser)Session["user"];
                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "WarrantyBattery",
                        "ChargeReport");

                var product = _iServiceManager.GetReceivedServiceProductById(id);
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModelsByUserAndActionId(user.UserId,actionModel.Id).ToList();
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        [HttpPost]
        public ActionResult DisChargeReport(DischargeReportModel model, long id)
        {

            try
            {

                var user = (ViewUser)Session["user"];
                var product = _iServiceManager.GetReceivedServiceProductById(id);
                ForwardDetails forward = new ForwardDetails
                {
                    UserId = user.UserId,
                    ForwardDateTime = DateTime.Now,
                    ForwardFromId = product.ForwardedToId,
                    ForwardToId = model.ForwardToId,
                    ReceiveId = model.BatteryReceiveId,
                    ForwardRemarks = model.ForwardRemarks
                };


                if (model.ForwardToId == product.ForwardedToId)
                {
                    model.ParentId = model.ForwardToId;
                }
                model.EntryByUserId = user.UserId;
                model.ForwardDetails = forward;


                bool result = _iServiceManager.SaveDischargeReport(model);
                if (result)
                {
                    return RedirectToAction("ProductInDisChargeSection");
                }
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModels().ToList();
                return View(product);
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


        public ActionResult Forward(long id)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "WarrantyBattery",
                        "Forward");

                var product = _iServiceManager.GetReceivedServiceProductById(id);
                product.ProductHistory =_iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager
                    .GetAllForwardToModelsByUserAndActionId(user.UserId, actionModel.Id).ToList();
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Forward(ForwardDetails model,long id)
        {
            try
            {
                var user = (ViewUser) Session["user"];
                model.ForwardDateTime=DateTime.Now;
                model.UserId = user.UserId;
                
                bool result = _iServiceManager.ForwardServiceBattery(model);
                if (result)
                {
                    return RedirectToAction("All");
                }

                var product = _iServiceManager.GetReceivedServiceProductById(id);
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModels().ToList();
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

                var user = (ViewUser)Session["user"];
                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "WarrantyBattery",
                        "Receive");
                WarrantyBatteryModel model = new WarrantyBatteryModel
                {
                    PhysicalConditions = _iCommonManager.GetAllPhysicalConditions().ToList(),
                    ServicingModels = _iCommonManager.GetAllServicingStatus().ToList(),
                    ChargingStatus = _iCommonManager.GetAllCharginStatus().ToList(),
                    ForwardToModels = _iCommonManager.GetAllForwardToModelsByUserAndActionId(user.UserId,actionModel.Id).ToList(),
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
                var user = (ViewUser)Session["user"];
                
                if (model.ForwardToId != Convert.ToInt32(ForwardTo.Received))
                {
                    model.ForwardDetails = new ForwardDetails
                    {
                        ForwardToId = model.ForwardToId,
                        ForwardFromId = Convert.ToInt32(ForwardTo.Received),
                        ForwardDateTime = DateTime.Now,
                        UserId = user.UserId,
                        ForwardRemarks = model.ForwardRemarks
                    };
                }
                else
                {
                    model.ForwardDetails = new ForwardDetails
                    {
                        ForwardToId = Convert.ToInt32(ForwardTo.Received),
                        ForwardFromId = Convert.ToInt32(ForwardTo.Received),
                        ForwardDateTime = DateTime.Now,
                        UserId = user.UserId,
                        ForwardRemarks = model.ForwardRemarks
                    };
                }

                var product = _iInventoryManager.GetProductHistoryByBarcode(model.Barcode) ?? new ViewProductHistory();
               
                var branchId = Convert.ToInt32(Session["BranchId"]);
                model.EntryByUserId = user.UserId;
                model.ReceiveByBranchId = branchId;
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