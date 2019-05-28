using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesManager")]
    public class NsmController : Controller
    {
        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInventoryManager _iInventoryManager;
        // GET: Sales/Nsm
        public NsmController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
        }
        public ActionResult Home()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList().FindAll(n => n.Status == 4);
                var delayedOrders = _iOrderManager.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
                var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
                var pendingorders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList();
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);

                SummaryModel summary = new SummaryModel
                {
                    BranchId = branchId,
                    CompanyId = companyId,
                    Orders = orders,
                    Clients = clients,
                    DelayedOrders = delayedOrders,
                    PendingOrders = pendingorders,
                    Products = products,
                    VerifiedOrders = verifiedOrders
                };
                return View(summary);
            }
            catch (Exception  exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        
       
    }
}