using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
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
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var totalProduction = _iReportManager.GetTotalProductionCompanyIdAndYear(companyId, DateTime.Now.Year);
            var totalDispatch = _iReportManager.GetTotalDispatchCompanyIdAndYear(companyId, DateTime.Now.Year); 
            var model=new FactorySummaryModel
            {
                StockQuantity = _iInventoryManager.GetStockProductInFactory().Count,
                IssuedQuantity = 0,
                ReturnedQuantity = 0 ,
                Production = totalProduction,
                Dispatch = totalDispatch
                

            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
            return View(stock);
        }

        //------------------ Change password------------------------
        public PartialViewResult ChangePassword(int id)
        {

            var user = _userManager.GetUserInformationByUserId(id);
            user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return PartialView("_ChangePasswordPartialPage", user);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
            model.Password = StringCipher.Encrypt(model.Password, "salam_cse_10_R");
            bool result = _userManager.UpdatePassword(model);
            if (result)
                return RedirectToAction("Home");
            return RedirectToAction("ChangePassword");
        }
        public ActionResult ProductionSummary()
        {
            var summaries = _iInventoryManager.GetProductionSummaries().ToList();
            return PartialView("_RptProductionSummaryPartialPage",summaries);
        }
        public ActionResult ProductionSummaryByMonth()
        {
            var summaries = _iInventoryManager.GetProductionSummaryByMonth(DateTime.Now).ToList();
            return View(summaries);
        }
    }
}