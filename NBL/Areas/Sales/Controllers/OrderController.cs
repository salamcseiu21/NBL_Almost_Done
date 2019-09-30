using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoMapper;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Products;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Logs;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Sales;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesExecutive")]
    public class OrderController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IReportManager _iReportManager;
        public OrderController(IClientManager iClientManager,IOrderManager iOrderManager,IInventoryManager iInventoryManager,IProductManager iProductManager,IDeliveryManager iDeliveryManager,IReportManager iReportManager)
        {
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iDeliveryManager = iDeliveryManager;
            _iReportManager = iReportManager;
        }
        public PartialViewResult All()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.UserId == user.UserId);
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
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList().FindAll(n=>n.UserId==user.UserId);
                ViewBag.Heading = "Latest Orders";
                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        
        public ActionResult Order()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser)Session["user"];
                CreateTempSalseOrderXmlFile(branchId, user.UserId);
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult Order(CreateOrderViewModel model)
        {
            try
            {
               
                //---------Get product by product id and client type id ---//
                var aProduct = _iProductManager.GetProductByProductAndClientTypeId(model.ProductId,model.ClientTypeId); 
                aProduct.Quantity = model.Quantity;
                AddProductToTempSalesOrderXmlFile(aProduct,model.ClientTypeId);
                
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        private void AddProductToTempSalesOrderXmlFile(Product aProduct,int clientTypeId)
        {

            try
            {
                var filePath = GetTempSalesOrderXmlFilePath();
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Root?.Elements().Where(n => n.Attribute("Id")?.Value == aProduct.ProductId.ToString()).Remove();
                xmlDocument.Save(filePath);

                 xmlDocument.Element("Products")?.Add(
                    new XElement("Product", new XAttribute("Id", aProduct.ProductId),
                        new XElement("ProductId", aProduct.ProductId),
                        new XElement("ProductName", aProduct.ProductName),
                        new XElement("Quantity", aProduct.Quantity),
                        new XElement("ClientTypeId", clientTypeId),
                        new XElement("UnitPrice", aProduct.UnitPrice),
                        new XElement("CategoryId", aProduct.CategoryId),
                        new XElement("SubSubSubAccountCode", aProduct.SubSubSubAccountCode),
                        new XElement("Vat", aProduct.Vat),
                        new XElement("VatId", aProduct.VatId),
                        new XElement("DiscountAmount", aProduct.DiscountAmount),
                        new XElement("DiscountId", aProduct.DiscountId),
                        new XElement("SalePrice", aProduct.SalePrice),
                        new XElement("ProductDetailsId", aProduct.ProductDetailsId)

                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
            }

        }

        
        [HttpPost]
        
        public void Update(string id, int quantity)
        {
            try
            {
                var filePath = GetTempSalesOrderXmlFilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Element("Products")?
                    .Elements("Product")?
                    .Where(n => n.Attribute("Id")?.Value == id).FirstOrDefault()
                    ?.SetElementValue("Quantity", quantity);
                xmlData.Save(filePath);

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
            }
        }


        [HttpPost]
        public void RemoveProductById(string id)
        {
            try
            {
                var filePath = GetTempSalesOrderXmlFilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
                xmlData.Save(filePath);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);

            }

        }

        public void RemoveAll()
        {

            try
            {
                var filePath = GetTempSalesOrderXmlFilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Remove();
                xmlData.Save(filePath);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
               

            }
        }

        /// <summary>
        /// Save Order to database---
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Confirm(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var branchId = Convert.ToInt32(Session["BranchId"]);
            try
            {
                var user = (ViewUser)Session["user"];
                var filePath = GetTempSalesOrderXmlFilePath();
                int clientId = Convert.ToInt32(collection["ClientId"]);
                var clientInfo= _iClientManager.GetClientDeailsById(clientId);
                int orderByUserId = user.UserId;
                decimal amount = Convert.ToDecimal(collection["Total"]);
                DateTime orderDate = Convert.ToDateTime(collection["OrderDate"]);
                List<Product> productList = GetTempOrderedProducts(filePath).ToList();
                var vat = productList.Sum(n => n.Vat * n.Quantity);
                var order = new Order
                {
                    BranchId = branchId,
                    DistributionPointId = branchId,
                    ClientId = clientId,
                    UserId = orderByUserId,
                    OrderDate = orderDate,
                    CompanyId = companyId,
                    Discount = productList.Sum(n => n.Quantity * n.DiscountAmount),
                    Products = productList,
                    SpecialDiscount = Convert.ToDecimal(collection["SD"]),
                    Vat = vat,
                    Amounts = productList.Sum(n=>(n.UnitPrice+n.Vat)*n.Quantity)
                };


                var result = _iOrderManager.Save(order);
                if (result > 0)
                {
                    Session["Orders"] = null;
                    RemoveAll();
                    aModel.Message = "<p class='text-green'>Order Submitted Successfully!!</p>";
                }

                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to Submit!!</p>";

                }

                //if (clientInfo.BranchId == 10)
                //{
                //    if (clientInfo.CreditLimit >= order.Amounts)
                //    {
                //        var result = _iOrderManager.Save(order);
                //        if (result > 0)
                //        {
                //            Session["Orders"] = null;
                //            RemoveAll();
                //            aModel.Message = "<p class='text-green'>Order Submitted Successfully!!</p>";
                //        }

                //        else
                //        {
                //            aModel.Message = "<p class='text-danger'>Failed to Submit!!</p>";

                //        }
                //    }
                //    else
                //    {
                //        aModel.Message = "<p class='text-danger'> Credit limit exceed!! </p>";
                //    }
                //}
                //else
                //{
                //    var result = _iOrderManager.Save(order);
                //    if (result > 0)
                //    {
                //        Session["Orders"] = null;
                //        RemoveAll();
                //        aModel.Message = "<p class='text-green'>Order Submitted Successfully!!</p>";
                //    }

                //    else
                //    {
                //        aModel.Message = "<p class='text-danger'>Failed to Submit!!</p>";

                //    }
                //}
               

            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
               
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProductList()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            var user = (ViewUser)Session["user"];
            string fileName = "Temp_Sales_Order_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/" + fileName);
            IEnumerable<Product> products = GetTempOrderedProducts(filePath);
            if (products.Count() != 0)
            {
                return Json(products, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<TempOrderedProduct>(), JsonRequestBehavior.AllowGet);
        }






      
        //--Cancel Order---

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
                order.ResonOfCancel = collection["Reason"];
                order.CancelByUserId = user.UserId;
                order.Status = Convert.ToInt32(OrderStatus.CancelledbySalesPerson);
                bool status = _iOrderManager.CancelOrder(order);
                return status ? RedirectToAction("All") : RedirectToAction("Cancel", orderId);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                Session["TOrders"] = order.OrderItems;
                return View(order);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                decimal amount = Convert.ToDecimal(collection["Amount"]);
                var dicount = Convert.ToDecimal(collection["Discount"]);
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Client = _iClientManager.GetById(order.ClientId);
                order.Status = Convert.ToInt32(OrderStatus.Pending);
                var orderItems = (IEnumerable<OrderItem>) Session["TOrders"];
                order.SpecialDiscount = dicount;
                order.Discount = orderItems.ToList().Sum(n=>n.Quantity*n.DiscountAmount);
                order.OrderDate = DateTime.Now;
                decimal vat = orderItems.Sum(n=>n.Vat*n.Quantity);
                order.Vat = vat;
                order.Amounts = amount+order.Discount;
                bool result = _iOrderManager.UpdateOrder(order);
                if (result)
                {
                    string r = _iOrderManager.UpdateOrderDetails(orderItems.ToList());
                }
                return RedirectToAction("PendingOrders");
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        //--Edit/Update order after submition 
        public void UpdateOrder(FormCollection collection)
        {
            try
            {
                var orderItems = (List<OrderItem>)Session["TOrders"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {
                    var anOrder = orderItems.Find(n => n.ProductId == pid);
                    orderItems.Remove(anOrder);
                    var result = _iOrderManager.DeleteProductFromOrderDetails(anOrder.OrderItemId); 
                    Session["TOrders"] = orderItems;
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
                        var anItem = orderItems.Find(n => n.ProductId == productId); 
                        if (anItem != null)
                        {
                            orderItems.Remove(anItem);
                            anItem.Quantity = qty;
                            orderItems.Add(anItem);
                            Session["TOrders"] = orderItems;
                           
                        }

                    }
                }
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }


        [HttpPost]
        public ActionResult AddNewItemToExistingOrder(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var items = (List<OrderItem>)Session["TOrders"];
            try
            {

                int clientId = Convert.ToInt32(collection["ClientId"]);
                Client aClient = _iClientManager.GetById(clientId);
                int productId = Convert.ToInt32(collection["ProductId"]);
                var aProduct = _iProductManager.GetProductByProductAndClientTypeId(productId, aClient.ClientTypeId);
                aProduct.Quantity = Convert.ToInt32(collection["Quantity"]);

                var item = items.Find(n => n.ProductId == productId);
                if (item != null)
                {
                    ViewBag.Result = "This product already is in the list!";
                }
                else
                {
                    bool rowAffected = _iOrderManager.AddNewItemToExistingOrder(aProduct,orderId);
                    if (rowAffected)
                    {
                        ViewBag.Result = "1 new Item added successfully!";
                    }
                    
                }

                return RedirectToAction("Edit", new { id = orderId } );

            }
            catch (Exception e)
            {
                
                Log.WriteErrorLog(e);
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                return RedirectToAction("Edit", new { id = orderId });
            }
        }


        public JsonResult GetOrderDetails()
        {
            if (Session["TOrders"] != null)
            {
                var orders = ((List<OrderItem>)Session["TOrders"]).ToList();
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClients(string term)
        {

            List<string> clients = _iClientManager.GetAll().ToList().Where(s => s.ClientName.StartsWith(term))
                  .Select(x => x.ClientName).ToList();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Error()
        {
            var eLogModel = new ViewWriteLogModel
            {
                Heading = "TEst Error",
                LogMessage = "Test"
            };
            Log.WriteErrorLog(eLogModel);
            return View("_ErrorPartial", new Exception());
        }

        public ActionResult OrderList()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().OrderByDescending(n => n.OrderId).ToList().FindAll(n => n.UserId == user.UserId);
                return View(orders);
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }

        public ActionResult PendingOrders() 
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, Convert.ToInt32(OrderStatus.Pending)).ToList().OrderByDescending(n => n.OrderId).ToList().FindAll(n => n.UserId == user.UserId);
                return View(orders);
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }

        public ActionResult DelayedOrders() 
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetDelayedOrdersToSalesPersonByBranchAndCompanyId(branchId, companyId).ToList().FindAll(n => n.UserId == user.UserId);
                return View(orders);
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }

        public ActionResult CancelledOrders()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetCancelledOrdersToSalesPersonByBranchCompanyUserId(branchId, companyId, user.UserId);
                return View(orders);
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }
        }
        public ActionResult OrderSlip(int id)
        {
            try
            {
                var orderSlip = _iOrderManager.GetOrderSlipByOrderId(id);
                var user = (ViewUser)Session["user"];
                orderSlip.ViewUser = user;
                return View(orderSlip);
            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
                return PartialView("_ErrorPartial", e);
            }

        }

        private string GetTempSalesOrderXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Temp_Sales_Order_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/" + fileName);
            return filePath;
        }

        private void CreateTempSalseOrderXmlFile(int branchId,int userId)
        {
            string fileName = "Temp_Sales_Order_By_" + branchId + userId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/" + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }
           

        }


        

        private IEnumerable<Product> GetTempOrderedProducts(string filePath)
        {
            List<Product> products = new List<Product>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                Product aProduct = new Product();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.ProductId = Convert.ToInt32(elementFirstAttribute);
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.UnitPrice = Convert.ToDecimal(xElements[4].Value);
                aProduct.CategoryId = Convert.ToInt32(xElements[5].Value);
                aProduct.SubSubSubAccountCode = xElements[6].Value;
                aProduct.Vat = Convert.ToDecimal(xElements[7].Value);
                aProduct.VatId = Convert.ToInt32(xElements[8].Value);
                aProduct.DiscountAmount = Convert.ToDecimal(xElements[9].Value);
                aProduct.DiscountId = Convert.ToInt32(xElements[10].Value);
                aProduct.SalePrice = Convert.ToDecimal(xElements[11].Value);
                aProduct.ProductDetailsId = Convert.ToInt32(xElements[12].Value);
                products.Add(aProduct);
            }
            return products;
        }

        public JsonResult GetClientById(int clientId)
        {
           //------------Remove all product added before on client select----
            RemoveAll();
            ViewClient client = _iClientManager.GetClientDeailsById(clientId);
            return Json(client, JsonRequestBehavior.AllowGet);
        }

     

        //-------------------Working here(2nd appril)--------------
        public JsonResult GetDeliveredOrderByClientId(int clientId)
        {
            var deliveriedOrders= _iDeliveryManager.GetDeliveredOrderByClientId(clientId).ToList(); 
            return Json(deliveriedOrders,JsonRequestBehavior.AllowGet);
        }
    }
}