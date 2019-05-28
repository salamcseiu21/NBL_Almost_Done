using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesAdmin")]
    public class OrderSalesAdminController : Controller
    {
        // GET: Sales/OrderSalesAdmin
        private readonly IOrderManager _iOrderManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;
        private readonly IBranchManager _iBranchManager;

        public OrderSalesAdminController(IOrderManager iOrderManager, IClientManager iClientManager, IDeliveryManager iDeliveryManager, IInvoiceManager iInvoiceManager,IBranchManager iBranchManager)
        {
            _iOrderManager = iOrderManager;
            _iClientManager = iClientManager;
            _iDeliveryManager = iDeliveryManager;
            _iInvoiceManager = iInvoiceManager;
            _iBranchManager = iBranchManager;
        }
        public PartialViewResult All()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList();
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
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).ToList();
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
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, Convert.ToInt32(OrderStatus.ApprovedbyNsm)).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult DelayedOrders()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        //---Approved order by Accounts/Admin
        public ActionResult Approve(int id)
        {

            try
            {
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                //ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", order.DistributionPointId);
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

                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var anUser = (ViewUser)Session["user"];
                var order = _iOrderManager.GetOrderByOrderId(id);
               // order.DistributionPointId = Convert.ToInt32(collection["DistributionPointId"]);
                order.Client = _iClientManager.GetById(order.ClientId);
                decimal specialDiscount = Convert.ToDecimal(collection["Discount"]);
                Invoice anInvoice = new Invoice
                {
                    InvoiceDateTime = DateTime.Now,
                    CompanyId = companyId,
                    BranchId = branchId,
                    ClientId = order.ClientId,
                    Amounts = order.Amounts,
                    Discount = order.Discount,
                    SpecialDiscount = specialDiscount,
                    InvoiceByUserId = anUser.UserId,
                    TransactionRef = order.OrederRef,
                    ClientAccountCode = order.Client.SubSubSubAccountCode,
                    Explanation = "Credit sale by " + anUser.UserId,
                    DiscountAccountCode = _iOrderManager.GetDiscountAccountCodeByClintTypeId(order.Client.ClientTypeId)
                };
                string invoice = _iInvoiceManager.Save(order.OrderItems, anInvoice);
                order.Status = Convert.ToInt32(OrderStatus.InvoicedOrApprovedbyAdmin);
                order.SpecialDiscount = specialDiscount;
                order.AdminUserId = anUser.UserId;
                string result = _iOrderManager.ApproveOrderByAdmin(order);
                return RedirectToAction("PendingOrder");
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpGet]
        public ActionResult Invoice(int id)
        {

            try
            {
                var invocedOrder = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
                var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(invocedOrder.TransactionRef);
                IEnumerable<InvoiceDetails> details = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceId(id);
                var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

                ViewInvoiceModel model = new ViewInvoiceModel
                {
                    Client = client,
                    Order = orderInfo,
                    Invoice = invocedOrder,
                    InvoiceDetailses = details
                };
                return View(model);

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

        public ActionResult ViewTodaysDeliverdOrders()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var orders = _iDeliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public ActionResult InvoicedOrderList()
        {
            try
            {
                SummaryModel model = new SummaryModel();
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iInvoiceManager.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId, companyId, user.UserId).ToList();
                foreach (Invoice invoice in orders)
                {
                    var order = _iOrderManager.GetOrderInfoByTransactionRef(invoice.TransactionRef);
                    invoice.Client = _iClientManager.GetById(order.ClientId);
                }
                model.InvoicedOrderList = orders;
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Cancel(int id)
        {
            try
            {
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                return View(order);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {



            try
            {
                var user = (ViewUser)Session["user"];
                int orderId = Convert.ToInt32(collection["OrderId"]);
                var order = _iOrderManager.GetOrderByOrderId(orderId);
                order.Client = _iClientManager.GetById(order.ClientId);
                order.ResonOfCancel = collection["Reason"];
                order.CancelByUserId = user.UserId;
                order.Status = Convert.ToInt32(OrderStatus.CancelledbyAdmin);
                var status = _iOrderManager.CancelOrder(order);
                return status ? RedirectToAction("All") : RedirectToAction("Cancel", new { id = orderId });
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult Verify(int id)
        {
            try
            {
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                return View(order);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Verify(FormCollection collection)
        {
            try
            {
                int orderId = Convert.ToInt32(collection["OrderId"]);
                string notes = collection["VerificationNote"];
                var user = (ViewUser)Session["user"];
                bool result = _iOrderManager.UpdateVerificationStatus(orderId, notes, user.UserId);
                if (result)
                {
                    return RedirectToAction("PendingOrder");
                }
                return RedirectToAction("PendingOrder");
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult VerifyingOrders()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["branchId"]);
                int companyId = Convert.ToInt32(Session["companyId"]);
                var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
                return PartialView("_ViewVerifyingOrdersPartialPage", verifiedOrders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}