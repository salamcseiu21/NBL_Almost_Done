using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IBarCodeManager _iBarCodeManager;

        public ReportsController(IBarCodeManager iBarCodeManager)
        {
            _iBarCodeManager = iBarCodeManager;
        }
        // GET: ResearchAndDevelopment/Reports
        public ActionResult ViewBarcode()
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

        public PartialViewResult GetBarcodeReportBySearchCriteria(SearchCriteria searchCriteria)
        {

            var barCodes = _iBarCodeManager.GetBarcodeReportBySearchCriteria(searchCriteria).ToList(); 
            return PartialView("_ViewBarcodePartialPage", barCodes);
        }
    }
}