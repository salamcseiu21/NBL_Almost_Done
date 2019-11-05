using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Logs;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportManager _iReportManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IDeliveryManager _iDeliveryManager;

        public ReportController(IReportManager iReportManager,IBranchManager iBranchManager,IDeliveryManager iDeliveryManager)
        {
            _iReportManager = iReportManager;
            _iBranchManager = iBranchManager;
            _iDeliveryManager = iDeliveryManager;
        }
        // GET: Sales/Report
        public ActionResult AllDeliveredQtyForOrder()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var results = _iReportManager.GetTotalDeliveredQtyByBranchId(branchId);
                return View(results);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult AllDeliveredOrder()
        {
            try
            {
               
                var branchId = Convert.ToInt32(Session["BranchId"]);
                ViewBag.BranchName=_iBranchManager.GetById(branchId).BranchName;
                var results = _iReportManager.GetTotalDeliveredOrderListByDistributionPointId(branchId);
                return View(results);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

       
        public ActionResult OrderListByDate()
        {
            return View();
        }
      
        public PartialViewResult LoadOrderListByDate(DateTime deliveryDate)
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //var user = (ViewUser)Session["user"];
            // var orders = _iDeliveryManager.GetAllDeliveredOrdersByDistributionPointCompanyDateAndUserId(branchId, companyId, deliveryDate, user.UserId);
            var orders = _iDeliveryManager.GetAllDeliveredOrdersByDistributionPointCompanyDate(branchId, companyId, deliveryDate);
            return PartialView("_ViewDeliveredOrdersPartialPage", orders);
        }
    }
}