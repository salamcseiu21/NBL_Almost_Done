using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.ChartOfAccounts;
using NBL.Models.Logs;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "AccountExecutive,AccountManager")]
    public class ChartOfAccountController : Controller
    {

        private readonly IAccountsManager _iAccountsManager;

        // GET: Accounts/Account
        public ChartOfAccountController(IAccountsManager iAccountsManager)
        {
            
            _iAccountsManager = iAccountsManager;
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
                List<SubSubSubAccount> subSubSubAccountnts = _iAccountsManager.GetAllSubSubSubAccountList().ToList();
                return View(subSubSubAccountnts);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}