using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Logs;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportManager _iReportManager;

        public ReportController(IReportManager iReportManager)
        {
            _iReportManager = iReportManager;
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
    }
}