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

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "AccountExecutive,AccountManager")]
    public class ReportsController : Controller
    {


        private readonly IReportManager _iReportManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;

        public ReportsController(IReportManager iReportManager,IAccountsManager iAccountsManager,IClientManager iClientManager,ICommonManager iCommonManager)
        {
            _iReportManager = iReportManager;
            _iAccountsManager = iAccountsManager;
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;

        }
        // GET: AccountsAndFinance/Reports
        //---------------------Client Report---------//


        public ActionResult ClientReoprt()
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
        public PartialViewResult GetClientReportBySearchCriteria(SearchCriteria searchCriteria)
        {
          
            var branchId = Convert.ToInt32(Session["BranchId"]);
            searchCriteria.BranchId = branchId;
            ICollection<ViewClientSummaryModel> summary = _iReportManager.GetClientReportBySearchCriteria(searchCriteria);
            return PartialView("_ClientSummaryPartialPage", summary);
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


        public ActionResult GeneralLedger()
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
        public PartialViewResult GetGeneralLedgerReportBySearchCriteria(SearchCriteria searchCriteria)
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);

          var account=_iCommonManager.GetSubSubSubAccountById(searchCriteria.SubSubSubAccountListId);
            searchCriteria.SubSubSubAccountCode = account.SubSubSubAccountCode;
            searchCriteria.BranchId = branchId;
            searchCriteria.CompanyId = companyId;
            IEnumerable<ViewLedgerModel> ledgers = _iReportManager.GetGeneralLedgerReportBySearchCriteria(searchCriteria);
            return PartialView("_ViewGeneralLedgerPartialPage", ledgers);
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
        public PartialViewResult GetCollectionListByDate(DateTime collectionDate)
        {
            SearchCriteria searchCriteria=new SearchCriteria();
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            searchCriteria.BranchId = Convert.ToInt32(Session["BranchId"]);
            searchCriteria.CompanyId = companyId;
            searchCriteria.UserId = 0;
            searchCriteria.StartDate = collectionDate;
            searchCriteria.EndDate = collectionDate;
            IEnumerable<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeBySearchCriteriaAndStatus(searchCriteria, 1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
        public PartialViewResult GetCollectionListByDateRange(SearchCriteria searchCriteria)
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            searchCriteria.BranchId = Convert.ToInt32(Session["BranchId"]);
            searchCriteria.CompanyId = companyId;
            searchCriteria.UserId = 0;
            IEnumerable<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeBySearchCriteriaAndStatus(searchCriteria, 1);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
    }
}