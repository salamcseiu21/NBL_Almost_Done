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

        public ReportController(IReportManager iReportManager,IBranchManager iBranchManager)
        {
            _iReportManager = iReportManager;
            _iBranchManager = iBranchManager;
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
    }
}