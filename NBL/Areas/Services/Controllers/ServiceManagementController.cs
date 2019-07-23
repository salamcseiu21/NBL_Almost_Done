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
using NBL.Models.ViewModels.Services;

namespace NBL.Areas.Services.Controllers
{

    [Authorize(Roles = "ServiceManagement")]
    public class ServiceManagementController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IBranchManager _iBranchManager;
        public ServiceManagementController(IInventoryManager iInventoryManager, ICommonManager iCommonManager, IServiceManager iServiceManager, IBranchManager iBranchManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iServiceManager = iServiceManager;
            _iBranchManager = iBranchManager; 
        }

        // GET: Services/ServiceManagement
        public ActionResult PendingList()
        {
            try
            {
               var products=_iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.ApprovalCommittee));
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

    }
}