using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Enums;
using NBL.Models.ViewModels;

namespace NBL.Areas.Sales.Controllers
{

    [Authorize(Roles = "Nsm")]
    public class OrderNsmController : Controller
    {
        // GET: Sales/OrderNsm
        private readonly IOrderManager _iOrderManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IClientManager _iClientManager;

        public OrderNsmController(IOrderManager iOrderManager, IInventoryManager iInventoryManager, IProductManager iProductManager, IClientManager iClientManager)
        {
            _iOrderManager = iOrderManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;

        }
        public PartialViewResult All()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewNsmOrdersPartialPage", orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }
        public ActionResult PendingOrder()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, Convert.ToInt32(OrderStatus.Pending)).ToList();
            return View(orders);

        }
        public ActionResult DelayedOrders()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
            return View(orders);
        }
        [HttpPost]
        public ActionResult AddNewItemToExistingOrder(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            List<OrderItem> orders = (List<OrderItem>)Session["TOrders"];
            try
            {
                var clientId = Convert.ToInt32(collection["ClientId"]);
                var client = _iClientManager.GetById(clientId);
                int productId = Convert.ToInt32(collection["ProductId"]);
                var aProduct = _iProductManager.GetProductByProductAndClientTypeId(productId, client.ClientTypeId);
                aProduct.Quantity = Convert.ToInt32(collection["Quantity"]);
                var orderItem = orders.Find(n => n.ProductId == productId);
                if (orderItem != null)
                {
                    ViewBag.Result = "This product already is in the list!";
                }
                else
                {
                    bool rowAffected = _iOrderManager.AddNewItemToExistingOrder(aProduct, orderId);
                    if (rowAffected)
                    {
                        ViewBag.Result = "1 new Item added successfully!";
                    }

                }

                //return RedirectToAction("Edit", orderId);
                return RedirectToAction("Edit", new { id = orderId });

            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                return RedirectToAction("Edit", orderId);
            }
        }
        //---Edit and approved the order-------
        public ActionResult Edit(int id)
        {

            var order = _iOrderManager.GetOrderByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);
            Session["TOrders"] = order.OrderItems.ToList();
            return View(order);

        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                decimal amount = Convert.ToDecimal(collection["Amount"]);
                var dicount = Convert.ToDecimal(collection["Discount"]);
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                var orderItems = (List<OrderItem>)Session["TOrders"];
                order.Discount = orderItems.Sum(n => n.Quantity * n.DiscountAmount);
                order.Vat = orderItems.Sum(n => n.Vat * n.Quantity);
                order.Amounts = amount + order.Discount;
                order.Status = Convert.ToInt32(OrderStatus.ApprovedbyNsm);
                order.SpecialDiscount = dicount;
                order.ApprovedByNsmDateTime = DateTime.Now;
                string r = _iOrderManager.UpdateOrderDetails(orderItems);
                order.NsmUserId = user.UserId;
                string result = _iOrderManager.ApproveOrderByNsm(order);
                return RedirectToAction("PendingOrder");
            }
            catch (Exception exception)
            {
                string messge = $"{exception.Message} </br> {exception.InnerException?.Message}";
                return View();
            }
        }
        public void Update(FormCollection collection)
        {
            try
            {
                var orderItems = (List<OrderItem>)Session["TOrders"];
                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var anItem = orderItems.Find(n => n.ProductId == pid);
                    orderItems.Remove(anItem);
                    var rowAffected = _iOrderManager.DeleteProductFromOrderDetails(anItem.OrderItemId);
                    if (rowAffected)
                    {
                        Session["TOrders"] = orderItems;
                    }

                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("product"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("product_Id_", "");
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var item = orderItems.Find(n => n.ProductId == productId);
                        if (item != null)
                        {
                            orderItems.Remove(item);
                            item.Quantity = qty;
                            orderItems.Add(item);
                            Session["TOrders"] = orderItems;
                        }

                    }
                }


            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }


        public JsonResult GetTempOrders()
        {
            if (Session["TOrders"] != null)
            {
                var orderItems = ((List<OrderItem>)Session["TOrders"]).ToList();
                return Json(orderItems, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<OrderItem>(), JsonRequestBehavior.AllowGet);
        }

        //--Cancel Order---


        public ActionResult Cancel(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);

            return View(order);
        }
        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            var user = (ViewUser)Session["user"];
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var order = _iOrderManager.GetOrderByOrderId(orderId);
            order.CancelByUserId = user.UserId;
            order.ResonOfCancel = collection["Reason"];
            order.Status = Convert.ToInt32(OrderStatus.CancelledbyNsm);
            var status = _iOrderManager.CancelOrder(order);
            return status ? RedirectToAction("PendingOrder") : RedirectToAction("Cancel", new { id = orderId });

        }
        public ActionResult OrderList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var user = (ViewUser)Session["user"];
            var orders = _iOrderManager.GetOrdersByBranchCompanyAndNsmUserId(branchId, companyId, user.UserId).ToList().OrderByDescending(n => n.OrderId).ToList().FindAll(n => n.Status < Convert.ToInt32(OrderStatus.InvoicedOrApprovedbyAdmin)).ToList();
            return View(orders);
        }

        [HttpGet]
        public ActionResult OrderSlip(int id)
        {
            var orderSlip = _iOrderManager.GetOrderSlipByOrderId(id);
            var user = (ViewUser)Session["user"];
            orderSlip.ViewUser = user;
            return View(orderSlip);
        }

        public JsonResult GetPendingOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList().FindAll(n => n.Status == Convert.ToInt32(OrderStatus.Pending)).ToList().Count;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            //var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var products = _iProductManager.GetAll();
            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }
    }
}