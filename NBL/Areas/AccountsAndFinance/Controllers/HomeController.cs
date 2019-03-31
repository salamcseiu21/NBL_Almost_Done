﻿using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "Accounts")]
    public class HomeController : Controller
    {
        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IProductManager _iProductManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IReportManager _iReportManager;
        private readonly IAccountsManager _iAccountsManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IReportManager iReportManager,IEmployeeManager iEmployeeManager,IProductManager iProductManager,IAccountsManager iAccountsManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iReportManager = iReportManager;
            _iEmployeeManager = iEmployeeManager;
            _iProductManager = iProductManager;
            _iAccountsManager = iAccountsManager;
        }
        // GET: Accounts/Home
        public ActionResult Home()
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iReportManager.GetTopClients().ToList();
            var batteries = _iReportManager.GetPopularBatteries().ToList();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrderByBranchIdCompanyIdAndYear(branchId, companyId, DateTime.Now.Year);
            var accountSummary =_iAccountsManager.GetAccountSummaryofCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var branches = _iBranchManager.GetAllBranches();
            SummaryModel aModel = new SummaryModel
            {
                Branches = branches.ToList(),
                BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                AccountSummary = accountSummary,
                Clients = clients,
                Products = batteries

            };

            return View(aModel);
        }


        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }

        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _iClientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",client);

        }

        public PartialViewResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);
        }

        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}