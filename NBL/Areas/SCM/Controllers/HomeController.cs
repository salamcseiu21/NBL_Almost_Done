using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Securities;
using NBL.Models.EntityModels.Identities;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.SCM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();

        private readonly IInventoryManager _iInventoryManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IReportManager _iReportManager;

        public HomeController(IInventoryManager iInventoryManager,IOrderManager iOrderManager,IInvoiceManager iInvoiceManager,IBranchManager iBranchManager,IReportManager iReportManager)
        {
            _iInventoryManager = iInventoryManager;
            _iBranchManager = iBranchManager;
            _iInvoiceManager = iInvoiceManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
        }
        // GET: SCM/Home
        public ActionResult Home() 
        {
            try
            {
                //Session.Remove("BranchId");
                //Session.Remove("Branch");
                int companyId = Convert.ToInt32(Session["CompanyId"]);

                var branches = _iBranchManager.GetAllBranches().ToList().FindAll(n => n.BranchId != 13).ToList();
                foreach (ViewBranch branch in branches)
                {
                    branch.Orders = _iOrderManager.GetOrdersByBranchId(branch.BranchId).ToList();
                    branch.Products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branch.BranchId, companyId).ToList();
                }


                var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList();
                // var todaysInvoceOrders= _iInvoiceManager.GetInvoicedOrdersByCompanyIdAndDate(companyId,DateTime.Now).ToList();
                SummaryModel model = new SummaryModel
                {
                    Branches = branches,
                    InvoicedOrderList = invoicedOrders
                    //OrderListByDate = todaysInvoceOrders
                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

     
        //------------------Stock------------
        [HttpGet]
        public PartialViewResult Stock()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
                return PartialView("_RptFactoryStockPartialPage", stock);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public PartialViewResult TotalStock()
        {
            try
            {
                var stock = _iReportManager.GetTotalStock();
                return PartialView("_RptOveralStockPartialPage", stock);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public PartialViewResult StockByBranch(int id)
        {

            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(id, companyId).ToList();
                var products = _iInventoryManager.GetRequsitionVeStockProductQtyByDistributionCenter(id, companyId).ToList();
                var branch = _iBranchManager.GetAllBranches().ToList().Find(n => n.BranchId == id);
                SummaryModel model = new SummaryModel
                {
                    Products = products,
                    Branch = branch
                };

                return PartialView("_ViewStockProductInBranchPartialPage", model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult OrderListByBranch(int id)
        {

            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(id, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList();

                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public PartialViewResult ProductionSummary()
        {
            try
            {
                var summaries = _iInventoryManager.GetProductionSummaries().ToList();
                return PartialView("_RptProductionSummaryPartialPage", summaries);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}