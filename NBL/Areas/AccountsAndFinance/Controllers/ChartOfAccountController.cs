using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.ChartOfAccounts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
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
        public ActionResult AddSubAccount() 
        {
            try
            {
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList();
                ViewBag.AccountHeadCode = new SelectList(accountHeads, "AccountHeadCode", "AccountHeadName");
                return View();
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
        public ActionResult AddSubSubAccount()
        {
            try
            {
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList();
                ViewBag.AccountHeadCode = new SelectList(accountHeads, "AccountHeadCode", "AccountHeadName");
                ViewBag.SubAccountCode = new SelectList(new List<SubAccount>(), "SubAccountCode", "SubAccountName");
                ViewBag.SubSubAccountCode = new SelectList(new List<SubSubAccount>(), "SubSubAccountCode", "SubSubAccountName");
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult AddSubSubAccount(SubSubAccount account)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                account.UserId = user.UserId;
                bool result = _iAccountsManager.AddSubSubAccount(account);
                if (result)
                {
                    return RedirectToAction("SubSubAccountList");
                }
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList();
                ViewBag.AccountHeadCode = new SelectList(accountHeads, "AccountHeadCode", "AccountHeadName");
                ViewBag.SubAccountCode = new SelectList(new List<SubAccount>(), "SubAccountCode", "SubAccountName");
                ViewBag.SubSubAccountCode = new SelectList(new List<SubSubAccount>(), "SubSubAccountCode", "SubSubAccountName");
                return View();
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

        public ActionResult AddSubSubSubAccount()
        {
            try
            {
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList();
                ViewBag.AccountHeadCode = new SelectList(accountHeads, "AccountHeadCode", "AccountHeadName");
                ViewBag.SubAccountCode = new SelectList(new List<SubAccount>(), "SubAccountCode", "SubAccountName");
                ViewBag.SubSubAccountCode = new SelectList(new List<SubSubAccount>(), "SubSubAccountCode", "SubSubAccountName");
                ViewBag.SubSubSubAccountCode = new SelectList(new List<SubSubSubAccount>(), "SubSubSubAccountCode", "SubSubSubAccountName");
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult AddSubSubSubAccount(SubSubSubAccount account)
        {
            try
            {

                var user = (ViewUser) Session["user"];
                account.UserId = user.UserId;
                account.SubSubSubAccountType = "Y";
                bool result = _iAccountsManager.AddSubSubSubAccount(account);
                if (result)
                {
                    return RedirectToAction("SubSubSubAccountList");
                }
                List<AccountHead> accountHeads = _iAccountsManager.GetAllChartOfAccountList().ToList();
                ViewBag.AccountHeadCode = new SelectList(accountHeads, "AccountHeadCode", "AccountHeadName");
                ViewBag.SubAccountCode = new SelectList(new List<SubAccount>(), "SubAccountCode", "SubAccountName");
                ViewBag.SubSubAccountCode = new SelectList(new List<SubSubAccount>(), "SubSubAccountCode", "SubSubAccountName");
                ViewBag.SubSubSubAccountCode = new SelectList(new List<SubSubSubAccount>(), "SubSubSubAccountCode", "SubSubSubAccountName");
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public JsonResult GetSubAccountByheadCode(string accountHeadCode)
        {
            var subAccounts = _iAccountsManager.GetAllSubAccountList().ToList().FindAll(n => n.AccountHeadCode == accountHeadCode).ToList();
            return Json(subAccounts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubSubAccountByheadCode(string subAccountCode)
        {
            var subSubAccounts = _iAccountsManager.GetAllSubSubAccountList().ToList().FindAll(n => n.SubAccountCode == subAccountCode).ToList();
            return Json(subSubAccounts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubSubSubAccountByheadCode(string subSubAccountCode)
        {
            var subSubSubAccounts = _iAccountsManager.GetAllSubSubSubAccountList().ToList().FindAll(n => n.SubSubAccountCode == subSubAccountCode).ToList();
            return Json(subSubSubAccounts, JsonRequestBehavior.AllowGet);
        }
    }
}