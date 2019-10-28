using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.ChartOfAccounts;
using NBL.Models.Logs;
using SubSubSubAccount = NBL.Models.EntityModels.ChartOfAccounts.SubSubSubAccount;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "AccountExecutive,AccountManager")]
    public class ChartOfAccountController : Controller
    {

        private readonly IAccountsManager _iAccountsManager;
        private readonly IReportManager _iReportManager;

        // GET: Accounts/Account
        public ChartOfAccountController(IAccountsManager iAccountsManager,IReportManager iReportManager)
        {
            
            _iAccountsManager = iAccountsManager;
            _iReportManager = iReportManager;
        }


        public ActionResult AccountType()
        {
            try
            {

                List<AccountType> accountTypes = _iAccountsManager.GetAllChartOfAccountType().ToList();
                return View(accountTypes);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult AccountList()
        {
            try
            {
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList(); 
                return View(accountHeads);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult SubAccountList()
        {
            try
            {
                List<SubAccount> subAccounts = _iAccountsManager.GetAllSubAccountList().ToList();
                return View(subAccounts);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult SubSubAccountList()
        {
            try
            {
                List<SubSubAccount> subSubAccountnts = _iAccountsManager.GetAllSubSubAccountList().ToList();
                return View(subSubAccountnts);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult SubSubSubAccountList()
        {
            try
            {
               // List<SubSubSubAccount> subSubSubAccountnts = _iAccountsManager.GetAllSubSubSubAccountList().ToList();
                ICollection<ViewSubSubSubAccount> accounts = _iReportManager.GetAllSubSubSubAccountList().ToList();
                return View(accounts);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}