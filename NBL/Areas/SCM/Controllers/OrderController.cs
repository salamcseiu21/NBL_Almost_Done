using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.SCM.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;

        public OrderController(IDeliveryManager iDeliveryManager, IInventoryManager iInventoryManager, IProductManager iProductManager, IClientManager iClientManager, IInvoiceManager iInvoiceManager, ICommonManager iCommonManager, IOrderManager iOrderManager, IBranchManager iBranchManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
            _iCommonManager = iCommonManager;
            _iOrderManager = iOrderManager;
            _iBranchManager = iBranchManager;
        }
        // GET: SCM/Order
        public ActionResult Home() 
        {
            return View();
        }
        public PartialViewResult All()
        {
            try
            {
               
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderWithClientInformationByCompanyId(companyId).ToList();
                ViewBag.Heading = "All Orders";
                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public PartialViewResult LatestOrders()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetLatestOrdersByCompanyId(companyId).ToList();
                ViewBag.Heading = "Latest Orders";
                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult PendingOrder()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);

                var orders = _iOrderManager.GetOrdersByCompanyIdAndStatus(companyId, Convert.ToInt32(OrderStatus.InvoicedOrApprovedbyAdmin)).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult ViewAllDeliveredOrders()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var orders = _iDeliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList().DistinctBy(n => n.TransactionRef).ToList();
                foreach (Delivery order in orders)
                {
                    var ord = _iOrderManager.GetOrderInfoByTransactionRef(order.TransactionRef);
                    order.Client = _iClientManager.GetById(ord.ClientId);
                }
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult Approve(int id)
        {
            try
            {
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", order.DistributionPointId);
                return View(order);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [HttpPost]
        public ActionResult Approve(int id, FormCollection collection)
        {
            try
            {

                var invoicebyId= _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
                //int branchId = Convert.ToInt32(Session["BranchId"]);
                //int companyId = Convert.ToInt32(Session["CompanyId"]);
                var anUser = (ViewUser)Session["user"];

                var order = _iOrderManager.GetOrderByOrderId(id);
                order.DistributionPointId = Convert.ToInt32(collection["DistributionPointId"]);
                order.Client = _iClientManager.GetById(order.ClientId);
                order.DistributionPointSetByUserId = anUser.UserId;
              
                //Invoice anInvoice = new Invoice
                //{
                //    InvoiceDateTime = DateTime.Now,
                //    CompanyId = companyId,
                //    BranchId = branchId,
                //    ClientId = order.ClientId,
                //    Amounts = order.Amounts,
                //    Discount = order.Discount,
                //    InvoiceByUserId = anUser.UserId,
                //    TransactionRef = order.OrederRef,
                //    ClientAccountCode = order.Client.SubSubSubAccountCode,
                //    Explanation = "Credit sale by " + anUser.UserId,
                //    DiscountAccountCode = _iOrderManager.GetDiscountAccountCodeByClintTypeId(order.Client.ClientTypeId)
                //};
               // string invoice = _iInvoiceManager.Save(order.OrderItems, anInvoice);
                order.Status = Convert.ToInt32(OrderStatus.DistributionPointSet);
                string result = _iOrderManager.ApproveOrderByScmManager(order);
                return RedirectToAction("PendingOrder");
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult PendingGeneralRequisitions()
        {
            var requisitions=_iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n=>n.Status.Equals(1) && n.IsFinalApproved.Equals("Y"));
           return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
        }

        public ActionResult GeneralRequisitionDetails(long id)
        {
            try
            {

                ICollection<ApprovalDetails> approval= _iCommonManager.GetAllApprovalDetailsByRequistionId(id);
                var model=new ViewGeneralRequisitionModel
                {
                    GeneralRequistionDetails = _iProductManager.GetGeneralRequisitionDetailsById(id),
                    GeneralRequisitionModel = _iProductManager.GetGeneralRequisitionById(id),
                    ApprovalDetails = approval

                };
                ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName","--Select--");
                return View(model);
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }
        [HttpPost]
        public ActionResult GeneralRequisitionDetails(long id,FormCollection collection)
        {
            try
            {
                var user = (ViewUser) Session["user"];
               var distributionPoint=Convert.ToInt32(collection["DistributionPointId"]);
                bool result = _iProductManager.ApproveGeneralRequisitionByScm(user.UserId,distributionPoint,id);
                var requisitions = _iProductManager.GetAllGeneralRequisitions().ToList().FindAll(n => n.Status.Equals(1) && n.IsFinalApproved.Equals("Y"));
                return PartialView("_ViewGeneralRequisitionListPartialPage", requisitions);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}