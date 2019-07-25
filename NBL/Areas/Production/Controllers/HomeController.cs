using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Production.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;
        private readonly UserManager _userManager=new UserManager();
        private readonly IReportManager _iReportManager;

        public HomeController(IInventoryManager iInventoryManager,IReportManager iReportManager)
        {
            _iInventoryManager = iInventoryManager;
            _iReportManager = iReportManager;
        }
        // GET: Factory/Home
        public ActionResult Home()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var totalProduction = _iReportManager.GetTotalProductionCompanyIdAndYear(companyId, DateTime.Now.Year);
                var totalDispatch = _iReportManager.GetTotalDispatchCompanyIdAndYear(companyId, DateTime.Now.Year);
                var model = new FactorySummaryModel
                {
                    StockQuantity = _iInventoryManager.GetStockProductInFactory().Count,
                    IssuedQuantity = 0,
                    ReturnedQuantity = 0,
                    Production = totalProduction,
                    Dispatch = totalDispatch


                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpGet]
        public ActionResult Stock()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
                return PartialView("_RptFactoryStockPartialPage", stock);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ProductionSummary()
        {
            try
            {
                var summaries = _iInventoryManager.GetProductionSummaries().ToList();
                return PartialView("_RptProductionSummaryPartialPage", summaries);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ProductionSummaryByMonth()
        {
            try
            {
                var summaries = _iInventoryManager.GetProductionSummaryByMonth(DateTime.Now).ToList();
                return View(summaries);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //--------------------------Product Status By Barcode---------------
        public ActionResult ProductStatus()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ProductStatus(FormCollection collection)
        {
            var barcode = collection["BarCode"];
            var product = _iInventoryManager.GetProductLifeCycle(barcode);
            TempData["T"] = product;
            return View();
        }

    }
}