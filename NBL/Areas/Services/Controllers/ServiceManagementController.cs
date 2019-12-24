using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.Services.Controllers
{

    [Authorize(Roles = "ServiceManagement,GeneralServiceManagement")]
    public class ServiceManagementController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IReportManager _iReportManager;
        public ServiceManagementController(IInventoryManager iInventoryManager, ICommonManager iCommonManager, IServiceManager iServiceManager, IBranchManager iBranchManager,IReportManager iReportManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iServiceManager = iServiceManager;
            _iBranchManager = iBranchManager;
            _iReportManager = iReportManager;
        }

        // GET: Services/ServiceManagement
        [Authorize(Roles = "ServiceManagement,GeneralServiceManagement")]
        public ActionResult PendingList()
        {
            try

            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser) Session["user"];
                if (user.Roles.Equals("ServiceManagement"))
                {
                    var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.ServiceManagement)).ToList().FindAll(n=>n.ReceiveByBranchId==branchId);
                    return View(products);
                }
                else
                {
                    var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.GeneralServiceManagement)).ToList().FindAll(n => n.ReceiveByBranchId == branchId);
                    return View(products);
                }
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
                var user = (ViewUser) Session["user"];
                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "ServiceManagement",
                        "Details");
                ChargeReportModel chargeReport = _iServiceManager.GetChargeReprortByReceiveId(id);
                DischargeReportModel dischargeReportModel = _iServiceManager.GetDisChargeReprortByReceiveId(id); 
                var product = _iServiceManager.GetReceivedServiceProductById(id);
                product.ChargeReportModel = chargeReport;
                product.DischargeReportModel = dischargeReportModel;
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
        public ActionResult Details(FormCollection collection,long id)
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
                    ForwardToId = Convert.ToInt32(collection["ForwardToId"]),
                    ReceiveId = id,
                    ForwardRemarks =collection["ForwardRemarks"]
                };


                bool result = _iServiceManager.SaveApprovalInformation(user.UserId,forward);
                if (result)
                {
                    return RedirectToAction("PendingList");
                }

                var actionModel =
                    _iCommonManager.GetActionListModelByAreaControllerActionName("Services", "ServiceManagement",
                        "Details");
                ChargeReportModel chargeReport = _iServiceManager.GetChargeReprortByReceiveId(id);
                DischargeReportModel dischargeReportModel = _iServiceManager.GetDisChargeReprortByReceiveId(id);
                product.ChargeReportModel = chargeReport;
                product.DischargeReportModel = dischargeReportModel;
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModelsByUserAndActionId(user.UserId, actionModel.Id).ToList();
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        //----------------Warranty battery replace list------------------
        public ActionResult TypeChangedReplaceList() 
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            //--------------Status=3 in management approval for alternate type delivery-------------
            var products = _iServiceManager.GetReceivedServiceProductsByStatus(3).ToList().FindAll(n=>n.ReceiveByBranchId==branchId);
            return View(products);
        }
        public ActionResult ProductSummary()
        {
            try
            {

                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        public PartialViewResult GetProductionSaleReplaceByMonthYear(int year, int monthId)
        {
            var products = _iReportManager.GetProductionSalesRepalcesByMonthYear(monthId, year).ToList();
            return PartialView("_ViewProductionSalesReplacePartialPage", products);
        }

    }
}