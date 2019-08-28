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
        private readonly IReportManager _iReportManager;
        public OrderController(IDeliveryManager iDeliveryManager, IInventoryManager iInventoryManager, IProductManager iProductManager, IClientManager iClientManager, IInvoiceManager iInvoiceManager, ICommonManager iCommonManager, IOrderManager iOrderManager, IBranchManager iBranchManager,IReportManager iReportManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
            _iCommonManager = iCommonManager;
            _iOrderManager = iOrderManager;
            _iBranchManager = iBranchManager;
            _iReportManager = iReportManager;
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

                var orders = _iOrderManager.GetOrdersByCompanyIdAndStatus(companyId, Convert.ToInt32(OrderStatus.InvoicedOrApprovedbySalesAdmin)).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        public ActionResult ApprovedOrder()
        {
            try
            {

                TempData["ApproveMsg"] = "All Approved Orders";
                var orders = _iReportManager.GetDistributionSetOrders();
                return PartialView("_ViewApprovedOrdersByScm",orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult TodaysApprovedOrder()
        {
            try
            {
                TempData["ApproveMsg"] = "Today's Approved Orders";

                var orders = _iReportManager.GetDistributionSetOrders().ToList().FindAll(n=>n.DistributionPointSetDateTime.Date.Equals(DateTime.Now.Date));
                return PartialView("_ViewApprovedOrdersByScm", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }


        public ActionResult OrderDetails(string id)
        {
            try
            {
               
                ViewInvoiceModel anInvoicedOrder=new ViewInvoiceModel();
              var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(id).ToList();
              var details= invoicedOrders.FirstOrDefault();
              var invoice=  _iInvoiceManager.GetInvoicedOrderByInvoiceId(details.InvoiceId);
                anInvoicedOrder.InvoiceDetailses = invoicedOrders;
                anInvoicedOrder.Invoice = invoice;
                return View(anInvoicedOrder);
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

             
                var anUser = (ViewUser)Session["user"];
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.DistributionPointId = Convert.ToInt32(collection["DistributionPointId"]);
                order.Client = _iClientManager.GetById(order.ClientId);
                order.DistributionPointSetByUserId = anUser.UserId;
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
        //------------------Cancel Order by SCM --------------
        public ActionResult CancelOrder(int id)
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
        public ActionResult CancelOrder(int id, FormCollection collection)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                order.CancelByUserId = user.UserId;
                order.ResonOfCancel = collection["CancelRemarks"];
                order.Status = Convert.ToInt32(OrderStatus.CancelByScm);
                var status = _iOrderManager.CancelOrder(order);
                if (status)
                return RedirectToAction("PendingOrder");
                return View(order);
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