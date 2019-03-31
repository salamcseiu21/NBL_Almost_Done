
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize(Roles = "Corporate")]
    public class OperationHeadController : Controller
    {
        // GET: Corporate/OperationHead
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

        public OperationHeadController(IVatManager iVatManager, IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IReportManager iReportManager, IDepartmentManager iDepartmentManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager, ICommonManager iCommonManager, IDiscountManager iDiscountManager, IRegionManager iRegionManager, ITerritoryManager iTerritoryManager, IAccountsManager iAccountsManager, IInvoiceManager iInvoiceManager, IDivisionGateway iDivisionGateway, IProductManager iProductManager)
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

        // GET: Corporate/Home
        public ActionResult Home()
        {

            Session.Remove("BranchId");
            Session.Remove("Branch");

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _iBranchManager.GetAllBranches();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId, DateTime.Now.Year);
            var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
            var products = _iInventoryManager.GetStockProductByCompanyId(companyId);
            var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
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
                Orders = orders,
                TopProducts = topProducts,
                Clients = clients,
                Employees = employees,
                Products = products,
                AccountSummary = accountSummary

            };
            return View(summary);
        }
        public PartialViewResult BranchWiseSummary()
        {
            var branches = _iBranchManager.GetAllBranches().ToList().FindAll(n => n.BranchId != 9).ToList();
            foreach (ViewBranch branch in branches)
            {
                branch.Orders = _iOrderManager.GetOrdersByBranchId(branch.BranchId).ToList();
            }
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList();
            SummaryModel model = new SummaryModel
            {
                Branches = branches,
                InvoicedOrderList = invoicedOrders
            };
            return PartialView("_ViewOrderSummaryPartialPage", model);
        }
        public PartialViewResult OrderSummary(int branchId)
        {
            List<Order> model = _iOrderManager.GetOrdersByBranchId(branchId).ToList();
            foreach (Order order in model)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }
        /// <summary>
        /// Sales reports 
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel();
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SalesSummary(ViewOrderSearchModel model)
        {
            model.Orders = model.Orders.ToList().FindAll(n => n.BranchId == model.BranchId);
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOrders(int? branchId)
        {
            var orders = _iOrderManager.GetOrdersByBranchId(Convert.ToInt32(branchId)).ToList();
            return Json(new { data = orders }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel();
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
        /// <summary>
        /// Get All Orders 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult All()
        {
            List<Order> model = _iOrderManager.GetAll().ToList();
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }

        public PartialViewResult Vouchers()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _iAccountsManager.GetVoucherListByCompanyId(companyId);
            return PartialView("_ViewVouchersPartialPage", vouchers);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _iAccountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _iAccountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }

        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _iAccountsManager.GetAllJournalVouchersByCompanyId(companyId).ToList();
            return PartialView("_ViewJournalPartialPage", journals);
        }
    }
}