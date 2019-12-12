using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        private readonly IAccountsManager _iAccountsManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IReportManager _iReportManager;
        private readonly IClientManager _iClientManager;
        public ReportsController(IAccountsManager iAccountsManager,IBranchManager iBranchManager,IReportManager iReportManager,IClientManager iClientManager)
        {
            _iAccountsManager = iAccountsManager;
            _iBranchManager = iBranchManager;
            _iReportManager = iReportManager;
            _iClientManager = iClientManager;
        }
        // GET: Corporate/Reports
        public PartialViewResult GetCollectionByYear(int year)
        {
            ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByYearAndStatus(year, 1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
         
        public ActionResult BankStatement()
        {
           
            return View();
        }
        public PartialViewResult BankStatementByYear(int year)
        {
            var bankStatements = _iReportManager.GetBankStatementByYear(year);
            return PartialView("_ViewBankStatementPartialPage", bankStatements);
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

        //---------------------Client Report---------//
        

        public ActionResult ClientReoprt()
        {
            try
            {
                ViewBag.BranchId = _iBranchManager.GetAllBranches();
                return View(); 
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public PartialViewResult GetClientReportBySearchCriteria(SearchCriteria searchCriteria)
        {
            if (searchCriteria.BranchId == null)
            {
                searchCriteria.BranchId = 0;
            }
           
            ICollection<ViewClientSummaryModel> summary = _iReportManager.GetClientReportBySearchCriteria(searchCriteria);
            return PartialView("_ClientSummaryPartialPage",summary);
        }

        public ActionResult ClientLedger()
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
        [HttpPost]
        public PartialViewResult GetClientLedgerReportBySearchCriteria(SearchCriteria searchCriteria)
        {
            var client = _iClientManager.GetClientDeailsById(searchCriteria.ClientId);
            searchCriteria.SubSubSubAccountCode = client.SubSubSubAccountCode;
            IEnumerable<ViewLedgerModel> ledgers = _iAccountsManager.GetClientLedgerBySearchCriteria(searchCriteria);
            client.LedgerModels = ledgers.ToList();
            return PartialView("_ClientLedgerPartialPage", client);
        }

        public ActionResult ReplaceSummary()
        {
            try
            {
                var products = _iReportManager.GetTotalReplaceProductList().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }

        public ActionResult ProductSummary()
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

        public PartialViewResult GetProductionSaleReplaceByMonthYear(int year, int monthId)
        {
            var products = _iReportManager.GetProductionSalesRepalcesByMonthYear(monthId, year).ToList();
            return PartialView("_ViewProductionSalesReplacePartialPage", products);
        }
    }
}
