using System;
using System.Linq;
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

        private readonly IReportManager _iReportManager;
        // GET: Sales/Nsm
        public NsmController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager,IReportManager iReportManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iReportManager = iReportManager;
        }
        public ActionResult Home()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList();
                var delayedOrders = _iOrderManager.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
                var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
                var pendingorders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList();
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
                var userWiseOrders = _iReportManager.UserWiseOrders().ToList().FindAll(n=>n.BranchId==branchId).OrderByDescending(n=>n.TotalOrder).ToList();
                var territoryWIshDelvieredQty = _iReportManager.GetTerritoryWishTotalSaleQtyByBranchId(branchId);

                SummaryModel summary = new SummaryModel
                {
                    BranchId = branchId,
                    CompanyId = companyId,
                    Orders = orders,
                    Clients = clients,
                    DelayedOrders = delayedOrders,
                    PendingOrders = pendingorders,
                    Products = products,
                    VerifiedOrders = verifiedOrders,
                    UserWiseOrders = userWiseOrders,
                    TerritoryWiseDeliveredPrducts = territoryWIshDelvieredQty.ToList()
                  
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