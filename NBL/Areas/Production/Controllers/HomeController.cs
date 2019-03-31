using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Production.Controllers
{

    [Authorize(Roles ="Factory")]
    public class HomeController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;

        public HomeController(IInventoryManager iInventoryManager)
        {
            _iInventoryManager = iInventoryManager;
        }
        // GET: Factory/Home
        public ActionResult Home()
        {
            Session.Remove("BranchId");
            Session.Remove("Branch");
            var model=new FactorySummaryModel
            {
                StockQuantity = _iInventoryManager.GetStockProductInFactory().Count,
                IssuedQuantity = 0,
                ReturnedQuantity = 0 
            };
            return View(model);
        }

        [HttpGet]
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
            return PartialView("_ViewStockProductInBranchPartialPage",stock);
        }
    }
}