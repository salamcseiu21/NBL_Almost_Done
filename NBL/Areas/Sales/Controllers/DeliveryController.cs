﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Others;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.Summaries;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
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

        public DeliveryController(IDeliveryManager iDeliveryManager,IInventoryManager iInventoryManager,IProductManager iProductManager,IClientManager iClientManager,IInvoiceManager iInvoiceManager,ICommonManager iCommonManager,IOrderManager iOrderManager,IBranchManager iBranchManager,IReportManager iReportManager)
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
        [Authorize(Roles = "DistributionManager")]
        public PartialViewResult OrderList()
        {
            try
            {
                SummaryModel model = new SummaryModel();
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByDistributionPoint(branchId).ToList();
                foreach (var invoice in invoicedOrders)
                {
                    invoice.Client = _iClientManager.GetById(invoice.ClientId);
                }
                model.InvoicedOrderList = invoicedOrders;
                return PartialView("_OrderListPartialPage", model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [Authorize(Roles = "DistributionManager")]
        public PartialViewResult LatestOrderList()
        {
            try
            {
                SummaryModel model = new SummaryModel();
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var invoicedOrders = _iInvoiceManager.GetLatestInvoicedOrdersByDistributionPoint(branchId).ToList();
                foreach (var invoice in invoicedOrders)
                {
                    invoice.Client = _iClientManager.GetById(invoice.ClientId);
                }
                model.InvoicedOrderList = invoicedOrders;
                return PartialView("_OrderListPartialPage", model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
            
        }
        [Authorize(Roles = "DistributionManager")]
        public PartialViewResult PartialDeliveredOrderList() 
        {
            SummaryModel model = new SummaryModel();
            int branchId = Convert.ToInt32(Session["BranchId"]);
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByDistributionPoint(branchId).ToList().FindAll(n=>n.InvoiceStatus==1);
            foreach (var invoice in invoicedOrders)
            {
                invoice.Client = _iClientManager.GetById(invoice.ClientId);
            }
            model.InvoicedOrderList = invoicedOrders;
            return PartialView("_OrderListPartialPage", model);

        }
       
        public ActionResult OrderSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel();
            //ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
        public PartialViewResult GetAllOrdersByBranchAndCompanyId()
        {
            
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<ViewOrder> orders;
            if(user.Roles.Equals("SalesExecutive"))
            {
                orders = _iOrderManager.GetOrdersByBranchCompanyAndUserId(branchId, companyId, user.UserId);
            }
            else
            {
                orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList();
            }
            
        
            return PartialView("_RptViewOrderListBydatePartialPage", orders);
        }
        [HttpPost]
        public PartialViewResult GetOrdersByBranchId(SearchCriteria searchCriteria)
        {

            var user = (ViewUser)Session["user"];
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var branchId = Convert.ToInt32(Session["BranchId"]);
            searchCriteria.BranchId = branchId;
            searchCriteria.CompanyId = companyId;
            if (user.Roles.Equals("SalesExecutive"))
            {
                searchCriteria.UserId = user.UserId;
            }
            else
            {
                searchCriteria.UserId = 0;
            }
            
                IEnumerable<ViewOrder> orders = _iOrderManager.GetOrdersByBranchCompanyAndDateRange(searchCriteria);
            return PartialView("_RptViewOrderListBydatePartialPage", orders);
        }

       
        [HttpGet]
        public ActionResult Delivery(int id)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
            var stock = _iInventoryManager.GetStockProductInBranchByBranchAndCompanyId(branchId,companyId);
            Session["Branch_stock"] = stock;
            var model=new ViewDeliveryModel
            {
                Client = _iClientManager.GetById(invoice.ClientId),
                InvoiceDetailses = invoicedOrders,
                Invoice = invoice
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var transport = collection["ownTransport"];
                bool isOwnTransport =transport!=null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int invoiceId = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                IEnumerable<InvoiceDetails> details = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceId(invoiceId);
                var client = _iClientManager.GetById(invoice.ClientId);
                var deliveredQty = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef).Count;
                var remainingToDeliverQty = invoice.Quantity - deliveredQty;
                string fileName = "Scanned_Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                    //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                

                int invoiceStatus = Convert.ToInt32(InvoiceStatus.PartiallyDelivered);
                int orderStatus = Convert.ToInt32(OrderStatus.PartiallyDelivered);
                if (remainingToDeliverQty == barcodeList.Count)
                {
                    invoiceStatus = Convert.ToInt32(InvoiceStatus.Delivered);
                    orderStatus = Convert.ToInt32(OrderStatus.Delivered);
                }

                List<InvoiceDetails> deliveredProductList=new List<InvoiceDetails>();
                foreach (ScannedProduct product in barcodeList.DistinctBy(n=>n.ProductId))
                {

                    var invoiceDetails= details.ToList().Find(n => n.ProductId.Equals(product.ProductId));
                    var qty = barcodeList.ToList().FindAll(n => n.ProductId == product.ProductId).Count;
                    invoiceDetails.Quantity = qty;
                    deliveredProductList.Add(invoiceDetails);
                }
                //-----------------Credit sale account code =1001021 ---------------
                //financialModel.InvoiceDiscountCode = "2102011";
                //financialModel.InvoiceDiscountAmount = (invoice.SpecialDiscount/invoice.Quantity)*barcodeList.Count;
                //-----------------Credit vat account code =2102013 ---------------
                //-----------------Credit invoice discount account code =2102012 ---------------

                //var up=   deliveredProductList.Sum(n => n.UnitPrice);
               // var discount = deliveredProductList.Sum(n => n.Discount);


                var grossAmount = deliveredProductList.Sum(n => (n.UnitPrice+n.Vat) * n.Quantity);
                var tradeDiscount = deliveredProductList.Sum(n => n.Discount * n.Quantity);
                var invoiceDiscount = (invoice.SpecialDiscount / invoice.Quantity) * barcodeList.Count;
                var grossDiscount = tradeDiscount + invoiceDiscount;
                var vat= deliveredProductList.Sum(n => n.Vat * n.Quantity);
                

                var financialModel =
                    new FinancialTransactionModel
                    {
                        //--------Dr -------------------
                        ClientCode = client.SubSubSubAccountCode,
                        ClientDrAmount = grossAmount-grossDiscount,
                        GrossDiscountAmount = grossDiscount,
                        GrossDiscountCode = "2102018",

                        //--------Cr -------------------
                        //SalesRevenueCode = "1001021" old,
                        SalesRevenueCode = "1001011",
                        SalesRevenueAmount = grossAmount-vat,
                        //VatCode = "2102013 test",
                        VatCode = "3108011",
                        VatAmount = vat,

                        TradeDiscountCode = "2102012",
                        TradeDiscountAmount = tradeDiscount,
                        InvoiceDiscountAmount = invoiceDiscount,
                        InvoiceDiscountCode = "2102011"
                       

                    };
               

                var aDelivery = new Delivery
                {
                    IsOwnTransport =isOwnTransport,
                    TransactionRef = invoice.TransactionRef,
                    InvoiceRef = invoice.InvoiceRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = invoice.CompanyId,
                    ToBranchId = invoice.BranchId,
                    DistributionPointId=branchId,
                    InvoiceId = invoiceId,
                    FromBranchId = invoice.BranchId,
                    FinancialTransactionModel = financialModel,
                    SpecialDiscount = invoiceDiscount
                };
                //----------SMS Buildder Model-----------
                var aModel = new MessageModel
                {
                    PhoneNumber = client.Phone.Replace("-", "").Trim(),
                    CustomerName = client.ClientName,
                    TotalQuantity = deliveredProductList.Sum(n => n.Quantity),
                    Amount = financialModel.ClientDrAmount,
                    TransactionDate = DateTime.Now,
                };
              
                aDelivery.MessageModel = aModel;
              
                if (client.IsConsiderCreditLimit==1)
                {
                    var netAmount = grossAmount - grossDiscount;
                    if (netAmount+client.Outstanding <= client.CreditLimit)
                    {
                        string result = _iInventoryManager.SaveDeliveredOrder(barcodeList, aDelivery, invoiceStatus, orderStatus);
                        aModel.MessageBody = aModel.GetMessageForDistribution();

                        if (result.StartsWith("S"))
                        {

                            System.IO.File.Create(filePath).Close();

                            //-----------Send SMS after successfull delivery inf save------------
                            var res = _iCommonManager.SendSms(aModel);

                            return RedirectToAction("LatestOrderList");
                        }
                    }
                    else
                    {
                        TempData["CreditLimit"] = "Credit Limit exceed!!";
                        return RedirectToAction("Delivery", new { id = invoice.InvoiceId });
                    }
                }
                else
                {

                    string result = _iInventoryManager.SaveDeliveredOrder(barcodeList, aDelivery, invoiceStatus, orderStatus);
                    aModel.MessageBody = aModel.GetMessageForDistribution();
                    if (result.StartsWith("S"))
                    {
                        

                        System.IO.File.Create(filePath).Close();

                        //-----------Send SMS after successfull delivery inf save------------
                        var res = _iCommonManager.SendSms(aModel);
                        return RedirectToAction("LatestOrderList");
                    }
                    return View();
                }
                
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                //return View("Delivery");
                throw new Exception();
            }
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode,int invoiceId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewBranchStockModel> products = (List<ViewBranchStockModel>) Session["Branch_stock"];
                var id = invoiceId;
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Scanned_Ordered_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedProducts(filePath);
               
                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Branch_stock"] = products;
                    }
                }


                // DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
                // var oldestProducts = products.ToList().FindAll(n => n.ProductionDate < date && n.ProductId == productId).ToList();
                bool isInInventory = products.Select(n => n.ProductBarCode).Contains(scannedBarCode);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);

                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);
                //------------Get invoced products-------------
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                List<InvoiceDetails> list = new List<InvoiceDetails>();
                var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

                foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
                {
                    var invoiceQty = invoiceDetailse.Quantity;
                    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Count;
                    if (invoiceQty != deliveredQty)
                    {
                        invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                        list.Add(invoiceDetailse);
                    }

                }
                bool isValied = list.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = list.ToList().FindAll(n=>n.ProductId==productId).Sum(n=>n.Quantity) == barcodeList.FindAll(n=>n.ProductId==productId).Count;
                 if (isScannedBefore)
                {
                    model.Message = "<p style='color:red'> Already Scanned</p>";
                   // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                   // return Json(model, JsonRequestBehavior.AllowGet);
                }

                //else if (oldestProducts.Count > 0)
                //{
                //    model.Message = "<p style='color:red'>There are total " + oldestProducts.Count + " Old product of this type .Please deliver those first .. </p>";
                //   // return Json(model, JsonRequestBehavior.AllowGet);
                //}
               else if (isSold)
                {
                    model.Message = "<p style='color:green'> This product Scanned for one of previous invoice... </p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
               else if(isValied && isInInventory)
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
               // return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
           // return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "DistributionManager")]
        public PartialViewResult ViewScannedBarcodeList(long id)
        {
            List<ScannedProduct> products = new List<ScannedProduct>();
            string fileName = "Scanned_Ordered_Product_List_For_" + id;
            var filePath = Server.MapPath("~/Files/" + fileName);
            products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            return PartialView("_ViewScannedBarCodePartialPage",products);
        }
        [Authorize(Roles = "DistributionManager")]
        public ActionResult ViewInvoiceIdOrderDetails(int invoiceId)
        {
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            return PartialView("_ModalOrderDeliveryPartialPage",invoice);
        }

      
        [HttpPost]
        public PartialViewResult LoadDeliverableProduct(int invoiceId)
        {
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            string fileName = "Ordered_Product_List_For_" + invoiceId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
            List<InvoiceDetails> list=new List<InvoiceDetails>();

            var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

            foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
            {
                var invoiceQty = invoiceDetailse.Quantity;
                var deliveredQty = 0;
                var viewProduct = deliveredProducts.ToList().Find(n => n.ProductId == invoiceDetailse.ProductId);
                if (viewProduct != null)
                {
                   deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Sum(n=>n.Quantity);
                }
                
                if (invoiceQty != deliveredQty)
                {
                    invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                    list.Add(invoiceDetailse);
                } 
            }

            return PartialView("_ViewLoadDeliverableProductPartialPage",list);
        }


        [HttpPost]
        public PartialViewResult LoadScannedProduct(int invoiceId)
        {
           // var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            string fileName = "Scanned_Ordered_Product_List_For_" + invoiceId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            List<ScannedProduct> list=new List<ScannedProduct>();
            if (!System.IO.File.Exists(filePath))
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            else
            {
                list = _iProductManager.ScannedProducts(filePath);
            }
            return PartialView("_ViewLoadScannedProductPartialPage", list);
        }

        public ActionResult Chalan(int deliveryId)
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
            return View(chalan);

        }

        public ActionResult DeliveredBarCodeList(int deliveryId) 
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
            return View(chalan);

        }
        [HttpGet]
        public ActionResult Invoice(int deliveryId)
        {
            var delivery= _iDeliveryManager.GetOrderByDeliveryId(deliveryId);
            var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId); 
            var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            var client = _iClientManager.GetClientDeailsById(delivery.ClientId);

            var model = new ViewInvoiceModel
            {
                Client = client,
                Order = orderInfo,
                Delivery = delivery,
                DeliveryDetails = deliveryDetails
            };
            return View(model);

        }
        public ActionResult DeleveredOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var user = (ViewUser) Session["user"];
            List<Delivery> orders;
            //if(user.Roles=="DistributionManager")
            //{
            //    orders = _iDeliveryManager.GetAllDeliveredOrdersByDistributionPointCompanyAndUserId(branchId, companyId, user.UserId).ToList();
            //}
            //else if (user.Roles.Equals("SalesExecutive"))
            //{
            //    orders = _iDeliveryManager.GetAllDeliveredOrdersByBranchAndCompany(branchId, companyId, user.UserId);
            //}
            //else
            //{


            //}
            orders = _iDeliveryManager.GetAllDeliveredOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            return View(orders);
        }
       
       

        //-----------------Transfer product -----------

        public ActionResult TransferList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<TransferRequisition> requisitions = _iProductManager.GetTransferRequsitionByStatus(1).ToList()
                .FindAll(n => n.RequisitionToBranchId == branchId);
            return View(requisitions);
        }

        public ActionResult DeliveryTransferProduct(long id)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iInventoryManager.GetStockProductInBranchByBranchAndCompanyId(branchId, companyId);
            Session["Branch_stock1"] = stock;

            var requisiton = _iProductManager.GetTransferRequsitionByStatus(1).ToList().ToList()
                .Find(n => n.TransferRequisitionId == id);
            ICollection<TransferRequisitionDetails> requisitions = _iProductManager.GetTransferRequsitionDetailsById(id).ToList();
            ViewTransferRequisition model = new ViewTransferRequisition
            {
                TtransferRequisitions = requisitions,
                Branch = _iBranchManager.GetAllBranches().ToList()
                    .Find(n => n.BranchId == requisiton.RequisitionByBranchId),
                TransferRequisition = requisiton
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeliveryTransferProduct(FormCollection collection,long transferRequisitionId)
        {
            try
            {
               
                var transport = collection["ownTransport"];
                bool isOwnTransport = transport != null;
                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                var requisition = _iProductManager.GetTransferRequsitionByStatus(1).ToList().Find(n => n.TransferRequisitionId == transferRequisitionId);
                IEnumerable<TransferRequisitionDetails> details = _iProductManager.GetTransferRequsitionDetailsById(transferRequisitionId);
               
                string fileName = "Requisition_Product_List_For_" + transferRequisitionId;
                var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);

                //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

                var aDelivery = new Delivery
                {
                    IsOwnTransport = isOwnTransport,
                    TransactionRef = requisition.TransferRequisitionRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    ToBranchId = requisition.RequisitionByBranchId,
                    FromBranchId = requisition.RequisitionToBranchId,
                    CompanyId = Convert.ToInt32(Session["CompanyId"])

            };

                var aModel = new TransferModel
                {
                    ScannedBarCodes = barcodeList,
                    Delivery = aDelivery,
                    TransferRequisition = requisition,
                    Detailses = details.ToList()
                };
                string result = _iInventoryManager.SaveTransferDeliveredProduct(aModel);
                if (result.StartsWith("S"))
                {
                    System.IO.File.Create(filePath).Close();
                    return RedirectToAction("TransferList");
                }
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                //return View("Delivery");
                throw new Exception();
            }
        }

        [HttpPost]
        public PartialViewResult ScannedProductsForRequisition(long requisitionId)
        {
            // var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            string fileName = "Requisition_Product_List_For_" + requisitionId;
            var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);
            List<ScannedProduct> list = new List<ScannedProduct>();
            if (!System.IO.File.Exists(filePath))
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            else
            {
                list = _iProductManager.ScannedProducts(filePath);
            }
            return PartialView("_ViewLoadScannedProductPartialPage", list);
        }


        public void SaveScannedBarcodeToTextFileForRequisition(string barcode, long requisitionId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                List<ViewBranchStockModel> products = (List<ViewBranchStockModel>)Session["Branch_stock1"];
                var id = requisitionId;
              
                string scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Requisition_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);
                var barcodeList = _iProductManager.ScannedProducts(filePath);

                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Branch_stock1"] = products;
                    }
                }

                // DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
                // var oldestProducts = products.ToList().FindAll(n => n.ProductionDate < date && n.ProductId == productId).ToList();
                bool isInInventory = products.Select(n => n.ProductBarCode).Contains(scannedBarCode);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);

                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);
                //------------Get invoced products-------------
                var requisitionDetailses = _iProductManager.GetTransferRequsitionDetailsById(requisitionId).ToList();
              //  List<InvoiceDetails> list = new List<InvoiceDetails>();
                //var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(requisition.TransferRequisitionRef);

                //foreach (var item in requisitionDetailses) 
                //{
                //    var rQuantity = item.Quantity; 
                //    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Count;
                //    if (invoiceQty != deliveredQty)
                //    {
                //        invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                //        list.Add(invoiceDetailse);
                //    }

                //}
                bool isValied = requisitionDetailses.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = requisitionDetailses.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity) == barcodeList.FindAll(n => n.ProductId == productId).Count;
                if (isScannedBefore)
                {
                    model.Message = "<p style='color:red'> Already Scanned</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                    // return Json(model, JsonRequestBehavior.AllowGet);
                }

                //else if (oldestProducts.Count > 0)
                //{
                //    model.Message = "<p style='color:red'>There are total " + oldestProducts.Count + " Old product of this type .Please deliver those first .. </p>";
                //   // return Json(model, JsonRequestBehavior.AllowGet);
                //}
                else if (isSold)
                {
                    model.Message = "<p style='color:green'> This product Scanned for one of previous invoice... </p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isValied && isInInventory)
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                // return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            // return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewTransferDetails(long requisitionId)
        {
            var requisition = _iProductManager.GetTransferRequsitionByStatus(1).ToList().Find(n => n.TransferRequisitionId == requisitionId);
            return PartialView("_ModalTransferDeliveryPartialPage", requisition);
        }

        public ActionResult DeliveredTransferList() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<ViewTransferModel> requisitions = _iProductManager.GetTransferListByBranchId(branchId).ToList();
            return View(requisitions);
        }

        //---------------------Order History----
        [Authorize(Roles = "SalesAdmin,SalesManager,SalesExecutive,CorporateSalesManager")]
        public ActionResult OrderHistory()
        {
            try
            {
                //  var orders = _iReportManager.GetOrderHistoriesByYear(DateTime.Now.Year);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ICollection<ViewOrderHistory> orderHistories = _iReportManager.GetOrderHistoriesByYearAndDistributionPointId(DateTime.Now.Year, branchId);
                return PartialView("_OrderHistoryPartialPage",orderHistories);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [Authorize(Roles = "SalesAdmin,SalesManager,SalesExecutive,CorporateSalesManager")]

        public ActionResult OrderHistoryDetails(int id)
        {
            var order = _iOrderManager.GetOrderHistoryByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);
            return PartialView("_OrderHistoryDetailsPartialPage", order);
        }

        [HttpPost]
        public JsonResult HideOrder(int id) 
        {
            var user = ((ViewUser)Session["user"]);
            SuccessErrorModel aModel = new SuccessErrorModel();
            var result = _iOrderManager.HideOrderByInvoiceId(id,user.UserId);
            aModel.Message = result ? "Hide order Successfully!" : "Failed to Hide order";
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
    }
}