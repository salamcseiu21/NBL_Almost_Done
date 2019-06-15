using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Approval;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;
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
        private readonly IProductManager _iProductManager;
        private readonly ICommonManager _iCommonManager;

        public SalesAdminController(IBranchManager iBranchManager, IClientManager iClientManager, IOrderManager iOrderManager, IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager, IInvoiceManager iInvoiceManager,ICommonManager iCommonManager,IProductManager iProductManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iInvoiceManager = iInvoiceManager;
            _iProductManager = iProductManager;
            _iCommonManager = iCommonManager;
        }
        // GET: Sales/SalesAdmin
        public ActionResult Home()
        {
            try
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList().FindAll(n => n.BranchId == branchId).ToList();
                var pendingOrders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().FindAll(n => n.Status == 1).ToList();
                var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                var delayedOrders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
                var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
                var model = new SummaryModel
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
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult PendingGeneralRequisitions()
        {
            var requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n => n.Status.Equals(0) && n.IsFinalApproved.Equals("Y"));
           // return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
            return View(requisitions);
        }

        public ActionResult GeneralRequisitionDetails(long id)
        {
            try
            {

                ICollection<ApprovalDetails> approval = _iCommonManager.GetAllApprovalDetailsByRequistionId(id);
                var model = new ViewGeneralRequisitionModel
                {
                    GeneralRequistionDetails = _iProductManager.GetGeneralRequisitionDetailsById(id),
                    GeneralRequisitionModel = _iProductManager.GetGeneralRequisitionById(id),
                    ApprovalDetails = approval

                };
                ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", "--Select--");
                return View(model);
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }
        [HttpPost]
        public ActionResult GeneralRequisitionDetails(long id, FormCollection collection)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                //var distributionPoint = Convert.ToInt32(collection["DistributionPointId"]);
                 bool result = _iProductManager.ApproveGeneralRequisitionBySalesAdmin(user.UserId,id);
               // var requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n => n.Status.Equals(0) && n.IsFinalApproved.Equals("Y"));
               
                return RedirectToAction("PendingGeneralRequisitions");
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}