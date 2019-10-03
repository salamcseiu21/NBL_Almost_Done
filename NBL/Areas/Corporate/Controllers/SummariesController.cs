using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize(Roles = "Corporate")]
    public class SummariesController : Controller
    {


        private readonly IEmployeeManager _iEmployeeManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IVatManager _iVatManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IProductManager _iProductManager;

        public SummariesController(IVatManager iVatManager, IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IReportManager iReportManager, IDepartmentManager iDepartmentManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager, ICommonManager iCommonManager, IDiscountManager iDiscountManager, IRegionManager iRegionManager, ITerritoryManager iTerritoryManager, IAccountsManager iAccountsManager, IInvoiceManager iInvoiceManager, IDivisionGateway iDivisionGateway, IProductManager iProductManager)
        {
            _iVatManager = iVatManager;
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iAccountsManager = iAccountsManager;
            _iInvoiceManager = iInvoiceManager;
            _iDivisionGateway = iDivisionGateway;
            _iProductManager = iProductManager;
        }
        // GET: Corporate/Summaries
        public ActionResult Home()
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _iBranchManager.GetAllBranches();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId, DateTime.Now.Year);
            var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
            var products = _iInventoryManager.GetStockProductByCompanyId(companyId);
            //var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
            var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year).ToList();
            var clients = _iClientManager.GetAllClientDetails();
            var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList();
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            SummaryModel summary = new SummaryModel
            {
                Branches = branches.ToList(),
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TopClients = topClients,
                Orders = new List<ViewOrder>(),
                TopProducts = topProducts,
                Clients = clients,
                Employees = employees,
                Products = products,
                AccountSummary = accountSummary,
                ViewEntityCount = _iReportManager.GetTotalEntityCount()

            };
            return View(summary);
        }

        public PartialViewResult BranchWiseSummary()
        {
            var branches = _iBranchManager.GetAllBranches().ToList().FindAll(n => n.BranchId != 13).ToList();
            foreach (ViewBranch branch in branches.ToList().FindAll(n => n.BranchId != 14))
            {
                branch.Orders = _iOrderManager.GetOrdersByBranchId(branch.BranchId).ToList();
            }
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList();
            SummaryModel model = new SummaryModel
            {
                Branches = branches.ToList().FindAll(n => n.BranchId != 14),
                InvoicedOrderList = invoicedOrders
            };
            return PartialView("_ViewOrderSummaryPartialPage", model);
        }
    }
}