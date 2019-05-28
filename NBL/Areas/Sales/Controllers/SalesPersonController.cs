using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.BLL.Contracts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesExecutive")]
    public class SalesPersonController : Controller
    {

        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IInventoryManager _iInventoryManager;

        public SalesPersonController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager, ICommonManager iCommonManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iCommonManager = iCommonManager;
            _iInventoryManager = iInventoryManager;
        }
        // GET: Sales/SalesPerson
        public ActionResult Home()
        {

            try
            {
                var user = (ViewUser)Session["user"];
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var delayedOrders = _iOrderManager.GetDelayedOrdersToSalesPersonByBranchAndCompanyId(branchId, companyId);
                var cancelledOrders = _iOrderManager.GetCancelledOrdersToSalesPersonByBranchCompanyUserId(branchId, companyId, user.UserId);
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.UserId == user.UserId);
                SummaryModel model = new SummaryModel
                {
                    Orders = orders,
                    Clients = clients,
                    PendingOrders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0),
                    DelayedOrders = delayedOrders,
                    Products = products,
                    CancelledOrders = cancelledOrders

                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}