using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesAdmin")]
    public class SalesAdminController : Controller
    {


        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;

        public SalesAdminController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager, IInvoiceManager iInvoiceManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iInvoiceManager = iInvoiceManager;
        }
        // GET: Sales/SalesAdmin
        public ActionResult Home()
        {
            SummaryModel model = new SummaryModel();
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList().FindAll(n => n.BranchId == branchId).ToList();
            var pendingOrders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().FindAll(n => n.Status == 1).ToList();
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var delayedOrders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
            var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
            model = new SummaryModel
            {
                InvoicedOrderList = orders,
                PendingOrders = pendingOrders,
                Clients = clients,
                Products = products,
                DelayedOrders = delayedOrders,
                VerifiedOrders = verifiedOrders
            };
            return View(model);
        }
    }
}