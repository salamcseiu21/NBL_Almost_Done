using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Models;
using NBL.Models.Logs;
using NBL.Models.Searchs;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        private readonly IAccountsManager _iAccountsManager;

        public ReportsController(IAccountsManager iAccountsManager)
        {
            _iAccountsManager = iAccountsManager;
        }
        // GET: Corporate/Reports
        public PartialViewResult GetCollectionByYear(int year)
        {
            ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByYearAndStatus(year, 1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }

        public ActionResult MonthlyReport()
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

        public ActionResult DailyReport() 
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
        public PartialViewResult GetCollectionByYearAndMonth(int year,int monthId)
        {
            ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByMonthYearAndStatus(monthId,year,1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }

        public PartialViewResult GetCollectionListByDate(DateTime collectionDate)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var collections = _iAccountsManager.GetAllReceivableCheque(companyId, collectionDate).ToList().FindAll(n=>n.ActiveStatus==1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }

        public PartialViewResult GetCollectionListByDateRange(SearchCriteria searchCriteria)
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            searchCriteria.BranchId = 0;
            searchCriteria.CompanyId = companyId;
            searchCriteria.UserId = 0;
            IEnumerable<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeBySearchCriteriaAndStatus(searchCriteria,1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
    }
}
