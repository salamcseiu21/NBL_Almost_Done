using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Services;

namespace NBL.Areas.Services.Controllers
{
    [Authorize(Roles = "ServiceExecutive")]
    public class WarrantyBatteryController : Controller
    {
        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IPolicyManager _iPolicyManager;
        private readonly IClientManager _iClientManager;
        public WarrantyBatteryController(IInventoryManager iInventoryManager,ICommonManager iCommonManager,IServiceManager iServiceManager,IBranchManager iBranchManager,IPolicyManager iPolicyManager,IClientManager iClientManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iServiceManager = iServiceManager; 
            _iBranchManager = iBranchManager;
            _iPolicyManager = iPolicyManager;
            _iClientManager = iClientManager;
        }

        public ActionResult All()
        {

            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iServiceManager.GetReceivedServiceProductsByForwarIdAndBranchId(Convert.ToInt32(ForwardTo.Received),branchId).ToList();

                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }
        public ActionResult PrintChallan(long id)
        {

            var product = _iServiceManager.GetReceivedServiceProductById(id);
           var client= _iClientManager.GetClientDeailsById(product.ClientId);
            product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
            product.Client = client;
            return View(product);
        }
        public ActionResult ReplaceList()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.Replace)).ToList().FindAll(n=>n.ReceiveByBranchId==branchId);
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult MakeRplaceChalan()
        {
            ICollection<Client> clients = _iServiceManager.GetClientListByServiceForwardId(Convert.ToInt32(ForwardTo.Replace));
            ViewBag.ClientId = clients;
            return View();
        }
        [HttpPost]
        public ActionResult MakeRplaceChalan(FormCollection collection)
        {

            ICollection<Client> clients = _iServiceManager.GetClientListByServiceForwardId(Convert.ToInt32(ForwardTo.Replace));
            ViewBag.ClientId = clients;
            return View();
        }

        public JsonResult ForwardServiceBatteryToDeistributionPoint(long receiveId)
        {
            SuccessErrorModel model=new SuccessErrorModel();
            bool result = _iServiceManager.ForwardServiceBatteryToDeistributionPoint(receiveId);
            if(result)
            {
                model.Message= "<p class='text-green'>Successfully! Forward to Disribution Point</p>";
            }
            else
            {
                model.Message = "<p class='text-danger'>Failed to Forward to Disribution Point</p>";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
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
                        "DisChargeReport");

                var product = _iServiceManager.GetReceivedServiceProductById(id);

                var proudctHistory= _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ProductHistory = proudctHistory;
                product.ForwardToModels = _iCommonManager.GetAllForwardToModelsByUserAndActionId(user.UserId,actionModel.Id).ToList();

                int month = GetMonthDifference(Convert.ToDateTime(proudctHistory.SaleDate), product.ReceiveDatetime);
               

                ViewTestPolicy policy= _iPolicyManager.DischargeTestPolicyByProductIdCategoryIdAndMonth(proudctHistory.ProductId,3,month);
                product.RecBackupTime = policy.AcceptableValue;
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
                if(model.BackUpTime>=model.RecommendedBackUpTime)
                {
                    model.DischargeReport = "The battery was passed the Discharge test or backup test";
                }
                else
                {
                    model.DischargeReport = "The battery was failed the Discharge test or backup test";
                }
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
        private static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
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
                var model = new WarrantyBatteryModel
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
                 var age= (DateTime.Now - Convert.ToDateTime(product.DeliveryDate)).TotalDays;
                 var warrantyPeriod=  product.AgeLimitInDealerStock + product.LifeTime;

                model.HasWarranty = age>warrantyPeriod ? "N" : "Y";
                var branchId = Convert.ToInt32(Session["BranchId"]);
                model.EntryByUserId = user.UserId;
                model.ReceiveByBranchId = branchId;
                model.ReceiveDatetime=DateTime.Now;
                model.DelivaryRef = product.DeliveryRef;
                model.TransactionRef = product.OrderRef;
                model.Barcode = product.ProductBarCode;
                model.ClientId = product.ClientId;
                model.ProductId = product.ProductId;
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