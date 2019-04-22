using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "DistributionManager")]
    public class DistributorController : Controller
    {
        // GET: Sales/Distributor
        private readonly IClientManager _iClientManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IInvoiceManager _iInvoiceManager;

        public DistributorController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IInventoryManager iInventoryManager, IInvoiceManager iInvoiceManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iInventoryManager = iInventoryManager;
            _iInvoiceManager = iInvoiceManager;
        }
        public ActionResult Home()
        {
            SummaryModel model = new SummaryModel();
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId);
            model.Clients = clients;
            model.InvoicedOrderList = invoicedOrders;
            model.Orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            model.Products = products;
            return View(model);
        }

    }
}