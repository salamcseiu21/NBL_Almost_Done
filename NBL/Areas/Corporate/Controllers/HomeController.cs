﻿
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.FinanceModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IEmployeeManager _iEmployeeManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IVatManager _iVatManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IProductManager _iProductManager;
        private readonly UserManager _userManager=new UserManager();
        private readonly IFactoryDeliveryManager _iFactoryDeliveryManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IServiceManager _iServiceManager;
        public HomeController(IVatManager iVatManager,IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IDiscountManager iDiscountManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IAccountsManager iAccountsManager,IInvoiceManager iInvoiceManager,IDivisionGateway iDivisionGateway,IProductManager iProductManager,IFactoryDeliveryManager iFactoryDeliveryManager,IDeliveryManager iDeliveryManager,IServiceManager iServiceManager)
        {
            _iVatManager = iVatManager;
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iAccountsManager = iAccountsManager;
            _iInvoiceManager = iInvoiceManager;
            _iDivisionGateway = iDivisionGateway;
            _iProductManager = iProductManager;
            _iFactoryDeliveryManager = iFactoryDeliveryManager;
            _iDeliveryManager = iDeliveryManager;
            _iServiceManager = iServiceManager;
        }

        // GET: Corporate/Home
        public ActionResult Home()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year);
                var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList();

                var totalProduction = _iReportManager.GetTotalProductionCompanyIdAndYear(companyId, DateTime.Now.Year);
                var totalDispatch = _iReportManager.GetTotalDispatchCompanyIdAndYear(companyId, DateTime.Now.Year);
                var torders= _iReportManager.GetTotalOrdersByYear(DateTime.Now.Year);
                var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
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
                    InvoicedOrderList = invoicedOrders,
                    Production = totalProduction,
                    Dispatch = totalDispatch,
                    TotalOrder = torders,
                    AccountSummary = accountSummary,
                    TopClients = topClients,
                    TopProducts = topProducts
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

        public ActionResult SalesSummary()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            //int branchId = Convert.ToInt32(Session["BranchId"]);
           // var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year);
           // var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId, DateTime.Now.Year);
            //var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
            //var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId);
            //var orders = _iOrderManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            //var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId);
            //var pendingOrders = _iOrderManager.GetPendingOrdersByBranchAndCompanyId(branchId,companyId).ToList();
            //var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo();
            //var branches = _iBranchManager.GetAllBranches();
            var totalProduction = _iReportManager.GetTotalProductionCompanyIdAndYear(companyId, DateTime.Now.Year);
            var totalDispatch = _iReportManager.GetTotalDispatchCompanyIdAndYear(companyId, DateTime.Now.Year);
            var viewTotalCollection = _iReportManager.GetTotalCollectionByYear(DateTime.Now.Year);
            var viewTotalSaleValue = _iReportManager.GetTotalSaleValueByYear(DateTime.Now.Year);
           // accountSummary.TotalSaleValue = _iReportManager.GetTotalSaleValueByYearAndMonth(DateTime.Now.Year, DateTime.Now.Month);
            SummaryModel aModel = new SummaryModel
            {
                // Branches = branches.ToList(),
                //BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                //TopClients = topClients,
                //TopProducts = topProducts,
                //Clients = clients,
                // Products = products,
                // Orders = orders,
                //PendingOrders = pendingOrders,
                //Employees = employees,
               // AccountSummary = accountSummary,
                Dispatch = totalDispatch,
                Production = totalProduction,
                ViewTotalCollection = viewTotalCollection,
                ViewTotalSaleValue = viewTotalSaleValue,
                TotalDeliveredQuantity = _iReportManager.GetTotalDeliveredQuantityByYear(DateTime.Now.Year)

            };
            return View(aModel);
        }
        public PartialViewResult AllOrders()
        {
            try
            {
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
                foreach (ViewOrder order in orders)
                {
                    order.Client = _iClientManager.GetById(order.ClientId);
                }
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
                var orders = _iOrderManager.GetLatestOrdersByCompanyId(companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
                foreach (ViewOrder order in orders)
                {
                    order.Client = _iClientManager.GetById(order.ClientId);
                }
                ViewBag.Heading = "Latest Orders";
                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public PartialViewResult CurrentMonthOrders()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderWithClientInformationByCompanyId(companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
                foreach (ViewOrder order in orders)
                {
                    order.Client = _iClientManager.GetById(order.ClientId);
                }
                ViewBag.Heading = $"Current Month Orders ({DateTime.Now:MMMM})";
                return PartialView("_ViewOrdersPartialPage", orders);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        /// <summary>
        /// Get order Details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDetails(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);
            return View(order);
        }
        
        public PartialViewResult ViewDivision()
        {
            var divisions = _iDivisionGateway.GetAll().ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);

        }
        public PartialViewResult ViewRegion()
        {
            var regions = _iRegionManager.GetAll().ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            var territories = _iTerritoryManager.GetAll().ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);

        }
       
    

        public ActionResult ViewDepartment()
        {
            var departments = _iDepartmentManager.GetAll().ToList();
            foreach (var department in departments)
            { 
                department.Employees = _iEmployeeManager.GetEmpoyeeListByDepartmentId(department.DepartmentId).ToList();
            }
            return View(departments);
        }

        

        public PartialViewResult ProductWishVat()
        {

            var vats= _iVatManager.GetProductLatestVat();
            return PartialView("_ViewProductWishVatPartialPage",vats);
        }

        public PartialViewResult ViewDiscount()
        {
            var discounts = _iDiscountManager.GetAll().ToList();
            return PartialView("_ViewDiscountPartialPage", discounts);
        }

        public ActionResult BusinessArea()
        {
            var branches = _iBranchManager.GetAllBranches().ToList().Where(i => !i.BranchName.Contains("Corporate"));
            foreach (var branch in branches)
            {
                branch.RegionList = _iRegionManager.GetAssignedRegionListToBranchByBranchId(branch.BranchId);
            }
            return View(branches);


        }
        

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var search = Request["search[value]"];
            var colIndex = Convert.ToInt32(Request.Form.GetValues("order[0][column]").First());
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //  var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //  int colIndex = 0;

            //  if (sortColumn != null && sortColumn.Equals("OrederRef"))
            //  {
            //      colIndex = 0;
            //  }
            // else if (sortColumn != null && sortColumn.Equals("CommercialName"))
            //  {
            //      colIndex = 1;
            //  }
            //else  if (sortColumn != null && sortColumn.Equals("Quantity"))
            //  {
            //      colIndex = 2;
            //  }
            //else  if (sortColumn != null && sortColumn.Equals("Amounts"))
            //  {
            //      colIndex = 3;
            //  }
            //else  if (sortColumn != null && sortColumn.Equals("OrderDate"))
            //  {
            //      colIndex = 4;
            //  }
            // else if (sortColumn != null && sortColumn.Equals("StatusDescription"))
            //  {
            //      colIndex = 5;
            //  }



            SearchCriteriaModel aCriteriaModel=new SearchCriteriaModel
            {
                DisplayLength = Convert.ToInt32(length),
                DisplayStart = Convert.ToInt32(start),
                Search = search,
                SortColomnIndex = colIndex,
                SortDirection = sortColumnDir
            };

            var v=_iOrderManager.GetOrder(aCriteriaModel);
            int recordsTotal = _iOrderManager.GetAll().Count();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = v }, JsonRequestBehavior.AllowGet);
        }

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

        [HttpGet]
        public ActionResult ProductWiseTotalStock(int id) 
        {
            try
            {
                var product= _iProductManager.GetProductByProductId(id);
                ViewBag.Product = product;
                var stock = _iReportManager.ProductWiseTotalStock(id);
                return View(stock);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult TotalReceiveByBranch() 
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var receiveList = _iInventoryManager.GetTotalReceiveProductByCompanyId(companyId).ToList(); 
                return View(receiveList);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult BranchWishStock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            var branches = _iBranchManager.GetAllBranches().ToList().FindAll(n => n.BranchId != 13).ToList();
            foreach (ViewBranch branch in branches)
            {
                branch.Orders = _iOrderManager.GetOrdersByBranchId(branch.BranchId).ToList();
                branch.Products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branch.BranchId, companyId).ToList();
            }
            return View(branches);
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

        public PartialViewResult StockByBranch(int id)
        {

            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
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
        public ActionResult DispatchList()
        {
            try
            {
                List<ViewDispatchModel> dispatch = _iProductManager.GetAllDispatchList().ToList();
                return View(dispatch);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult DeliveredOrderList()
        {

            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //---------------------------Distribution point=13= Factory
                var orders = _iDeliveryManager.GetAllDeliveredOrdersByDistributionPointAndCompanyId(13, companyId).ToList();
                return View(orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult OrderChalan(int deliveryId)
        {
            try
            {
                Session["DeliveryFromBranch"] = _iBranchManager.GetAllBranches().FirstOrDefault(n => n.BranchId == 13);
                var chalan = _iDeliveryManager.GetChalanByDeliveryIdFromFactory(deliveryId);
                return View(chalan);

            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Chalan(long dispatchId)
        {
            try
            {
                Session["DeliveryFromBranch"] = _iBranchManager.GetAllBranches().FirstOrDefault(n => n.BranchId == 13);
                ViewDispatchChalan chalan = _iFactoryDeliveryManager.GetDispatchChalanByDispatchId(dispatchId);
                return View(chalan);
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

        public ActionResult SalesChart()
        {
            return View();
        }


        public PartialViewResult ViewClient()
        {
            try
            {
                var clients = _iClientManager.GetAllClientDetails().ToList();
                return PartialView("_ViewClientPartialPage", clients);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewClientProfile(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        public ActionResult SearchClient()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public PartialViewResult SearchClient(int clientId)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(clientId);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public PartialViewResult ClientDetails(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult DeliveryDetails(long id)
        {
            try
            {
                var deliveryDetails = _iDeliveryManager.GetDeliveredOrderDetailsByDeliveryId(id);
                var delivery = _iDeliveryManager.GetOrderByDeliveryId(id);
                ICollection<ViewClientStockProduct> products = _iDeliveryManager.GetClientStockProductAgeByDeliveryId(id);
                ViewDeliveryModel model = new ViewDeliveryModel
                {
                    DeliveryDetailses = deliveryDetails.ToList(),
                    Delivery = delivery,
                    Client = _iClientManager.GetById(delivery.Client.ClientId),
                    ClientStockProducts = products.ToList()
                };
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ProductDetails()
        {
            try
            {
                var products = _iReportManager.GetAllProductDetails().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult SoldProducts()
        {
            try
            {
                var products = _iServiceManager.GetAllSoldProducts().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public ActionResult ClientSummary()
        {
            var summary = _iClientManager.GetClientSummary();
            return View(summary);
        }

        public ActionResult BranchWishClientSummary(int id)
        {
            ViewBag.BranchName = Enum.GetName(typeof(BranchEnum), id);
            ICollection<ViewClientSummaryModel> summary = _iClientManager.GetClientSummaryByBranchId(id);
            return View(summary);
        }

        public PartialViewResult Supplier()
        {

            try
            {
                var suppliers = _iCommonManager.GetAllSupplier().ToList();
                return PartialView("_ViewSupplierPartialPage", suppliers);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }


        [HttpGet]
        public ActionResult CollectionList() 
        {
            try
            {
                ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByCompanyIdAndStatus(1,1);
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult CurrentMonthCollection()
        {
            try
            {
                ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByMonthYearAndStatus(DateTime.Now.Month,DateTime.Now.Year,1);
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult CurrentYearCollection()
        {
            try
            {
                ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByYearAndStatus(DateTime.Now.Year, 1);
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpGet]
        public ActionResult CollectionSummary()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var collections = _iAccountsManager.GetAllReceivableChequeByCompanyIdAndStatus(companyId, 1);
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult PendingCollectionList()
        {
            try
            {
                ICollection<ChequeDetails> collections = _iAccountsManager.GetAllReceivableChequeByCompanyIdAndStatus(1, 0);
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult OrderHistoryDetails(long id)
        {

           
            ViewOrder viewOrder=_iOrderManager.GetOrderByDeliveryId(id);
            var order = _iOrderManager.GetOrderHistoryByOrderId(viewOrder.OrderId);
            order.Client = _iClientManager.GetById(order.ClientId);
            return PartialView("_ViewOrderHistoryPartialPage", order);
        }

       
    }
}