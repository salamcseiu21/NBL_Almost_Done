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

        public HomeController(IInventoryManager iInventoryManager,IOrderManager iOrderManager,IInvoiceManager iInvoiceManager,IBranchManager iBranchManager)
        {
            _iInventoryManager = iInventoryManager;
            _iBranchManager = iBranchManager;
            _iInvoiceManager = iInvoiceManager;
            _iOrderManager = iOrderManager;
        }
        // GET: SCM/Home
        public ActionResult Home() 
        {
            Session.Remove("BranchId");
            Session.Remove("Branch");
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

        //------------------ Change password------------------------
        public PartialViewResult ChangePassword(int id)
        {

            var user = _userManager.GetUserInformationByUserId(id);
            user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return PartialView("_ChangePasswordPartialPage", user);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
            model.Password = StringCipher.Encrypt(model.Password, "salam_cse_10_R");
            bool result = _userManager.UpdatePassword(model);
            if (result)
                return RedirectToAction("Home");
            return RedirectToAction("ChangePassword");
        }
        //------------------Stock------------
        [HttpGet]
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
            return PartialView("_RptFactoryStockPartialPage", stock);
        }
        public PartialViewResult StockByBranch(int id)
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(id, companyId).ToList();
            var branch = _iBranchManager.GetAllBranches().ToList().Find(n => n.BranchId == id);
            SummaryModel model = new SummaryModel
            {
                Products = products,
                Branch = branch
            };

            return PartialView("_ViewStockProductInBranchPartialPage", model);
        }

        public ActionResult OrderListByBranch(int id)
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(id, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList();
           
            return PartialView("_ViewOrdersPartialPage", orders);
        }
        public PartialViewResult ProductionSummary()
        {
            var summaries = _iInventoryManager.GetProductionSummaries().ToList();
            return PartialView("_RptProductionSummaryPartialPage", summaries);
        }
    }
}