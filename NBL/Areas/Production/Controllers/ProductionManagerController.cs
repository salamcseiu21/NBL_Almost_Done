
using System;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "ProductionManager")]
    public class ProductionManagerController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;
        private readonly UserManager _userManager = new UserManager();
        private readonly IReportManager _iReportManager;

        public ProductionManagerController(IInventoryManager iInventoryManager, IReportManager iReportManager)
        {
            _iInventoryManager = iInventoryManager;
            _iReportManager = iReportManager;
        }
        // GET: Production/ProductionManager
        public ActionResult Home() 
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
    }
}