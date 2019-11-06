using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.FinanceModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Reports;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.Summaries;

namespace NBL.BLL
{
    public class ReportManager:IReportManager
    {
        private readonly IReportGateway _iReportGateway;
       private readonly IOrderManager _iOrderManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBarCodeManager _iBarCodeManager;
        private readonly IProductReplaceGateway _iProductReplaceGateway;

        public ReportManager(IOrderManager iOrderManager, IReportGateway iReportGateway, IInventoryManager iInventoryManager,IBarCodeManager iBarCodeManager,IProductReplaceGateway iProductReplaceGateway)
        {
            _iOrderManager = iOrderManager;
            _iReportGateway = iReportGateway;
            _iInventoryManager = iInventoryManager;
            _iBarCodeManager = iBarCodeManager;
            _iProductReplaceGateway = iProductReplaceGateway;
        }
        public IEnumerable<ViewClient> GetTopClients()
        {
            return _iReportGateway.GetTopClients();
        }

        public IEnumerable<ViewClient> GetTopClientsByYear(int year)
        {
            return _iReportGateway.GetTopClientsByYear(year);
        }

        public IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId)
        {
            return _iReportGateway.GetTopClientsByBranchId(branchId);
        }

        public IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year)
        {
            return _iReportGateway.GetTopClientsByBranchIdAndYear(branchId, year);
        }
        
        public IEnumerable<ViewProduct> GetPopularBatteries()
        {
            return _iReportGateway.GetPopularBatteries();
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByYear(int year)
        {
            return _iReportGateway.GetPopularBatteriesByYear(year);
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iReportGateway.GetPopularBatteriesByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchIdCompanyIdAndYear(int branchId, int companyId,int year)
        {
            return _iReportGateway.GetPopularBatteriesByBranchIdCompanyIdAndYear(branchId, companyId, year);
        }
        public ViewTotalOrder GetTotalOrderByBranchIdCompanyIdAndYear(int branchId,int companyId, int year)
        { 
           var totalOrders = _iOrderManager.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId, companyId,year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
           };
            return order;
        }
        public ViewTotalOrder GetTotalOrdersByBranchCompanyAndYear(int branchId, int companyId, int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId, companyId, year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }

       

        public ViewTotalOrder GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
        public ViewTotalOrder GetTotalOrdersByYear(int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByYear(year).ToArray();

            var order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
        //---------------Total Production -----------------------
        public ViewTotalProduction GetTotalProductionCompanyIdAndYear(int companyId, int year)
        {
            var totalProduction = _iInventoryManager.GetTotalProductionCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalProduction production = new ViewTotalProduction
            {
                January = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return production;
        }
        //----------------Total Dispatch ---------------
        public ViewTotalDispatch GetTotalDispatchCompanyIdAndYear(int companyId, int year)
        {
            var totalDispatch = _iInventoryManager.GetTotalDispatchCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalDispatch dispatch = new ViewTotalDispatch
            {
                January = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return dispatch;
        }


        //------------Total Sale value--------------
        public ViewTotalSaleValue GetTotalSaleValueByYear(int year)
        {
            ICollection<ChartModel> totalSaleValues = _iReportGateway.GetTotalSaleValueByYear(year).ToList();

            ViewTotalSaleValue saleValue = new ViewTotalSaleValue
            {
                January = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.TotalSaleValue,
                February = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.TotalSaleValue,
                March = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.TotalSaleValue,
                April = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.TotalSaleValue,
                May = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("May"))?.TotalSaleValue,
                June = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("June"))?.TotalSaleValue,
                July = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("July"))?.TotalSaleValue,
                August = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.TotalSaleValue,
                September = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.TotalSaleValue,
                October = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.TotalSaleValue,
                November = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.TotalSaleValue,
                December = totalSaleValues?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.TotalSaleValue
            };
            return saleValue;
        }
        //--------------Total collection value by year---------
        public ViewTotalCollection GetTotalCollectionByYear(int year)
        {
            ICollection<ChartModel> totalCollection = _iReportGateway.GetTotalCollectionByYear(year).ToList();

            ViewTotalCollection collection = new ViewTotalCollection
            {
                January = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.TotalCollectionValue,
                February = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.TotalCollectionValue,
                March = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.TotalCollectionValue,
                April = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.TotalCollectionValue,
                May = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("May"))?.TotalCollectionValue,
                June = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("June"))?.TotalCollectionValue,
                July = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("July"))?.TotalCollectionValue,
                August = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.TotalCollectionValue,
                September = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.TotalCollectionValue,
                October = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.TotalCollectionValue,
                November = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.TotalCollectionValue,
                December = totalCollection?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.TotalCollectionValue
            };
            return collection;
        }

        public decimal GetTotalSaleValueByYearAndMonth(int year, int month)
        {
            return _iReportGateway.GetTotalSaleValueByYearAndMonth(year,month);
        }

        public ICollection<ViewDeliveredQuantityModel> GetTotalDeliveredQtyByBranchId(int branchId)
        {
            return _iReportGateway.GetTotalDeliveredQtyByBranchId(branchId);
        }

        public ViewTotalDeliveredQuantity GetTotalDeliveredQuantityByYear(int year)
        { 
            ICollection<ChartModel> totalDelivery = _iReportGateway.GetTotalDeliveredQuantityByYear(year).ToList();

            ViewTotalDeliveredQuantity delivered = new ViewTotalDeliveredQuantity
            {
                January = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.TotalDeliveredQty,
                February = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.TotalDeliveredQty,
                March = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.TotalDeliveredQty,
                April = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.TotalDeliveredQty,
                May = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("May"))?.TotalDeliveredQty,
                June = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("June"))?.TotalDeliveredQty,
                July = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("July"))?.TotalDeliveredQty,
                August = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.TotalDeliveredQty,
                September = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.TotalDeliveredQty,
                October = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.TotalDeliveredQty,
                November = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.TotalDeliveredQty,
                December = totalDelivery?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.TotalDeliveredQty
            };
            return delivered;
        }

        public ICollection<ViewClientSummaryModel> GetClientReportBySearchCriteria(SearchCriteria searchCriteria)
        {
            return _iReportGateway.GetClientReportBySearchCriteria(searchCriteria);
        }

        public ICollection<ViewStockProduct> GetStockProductBarcodeByBranchAndProductIdTemp(int branchId, int productId)
        {
            return _iReportGateway.GetStockProductBarcodeByBranchAndProductIdTemp(branchId, productId);
        }

        public bool InActiveProduct(int branchId, List<ViewStockProduct> stockBarcodList)
        {
            int rowAffected = _iReportGateway.InActiveProduct(branchId,stockBarcodList);
            return rowAffected > 0;
        }

        public ICollection<ViewDeliveredOrderModel> GetTotalDeliveredOrderListByDistributionPointId(int branchId)
        {
            return _iReportGateway.GetTotalDeliveredOrderListByDistributionPointId(branchId);
        }

        public ICollection<SubSubSubAccount> GetBankStatementByYear(int year)
        {
            return _iReportGateway.GetBankStatementByYear(year);
        }

        public ICollection<ViewSubSubSubAccount> GetAllSubSubSubAccountList()
        {
            return _iReportGateway.GetAllSubSubSubAccountList();
        }

        public IEnumerable<ReplaceReport> GetReplaceListByDateAndDistributionPoint(DateTime deliveryDate, int distributionPoint)
        {
            return _iProductReplaceGateway.GetReplaceListByDateAndDistributionPoint(deliveryDate,distributionPoint);
        }

        public bool IsValiedBarcode(string barcode)
        {
            var model = _iBarCodeManager.GetAll().ToList().Select(n=>n.Barcode).ToList().Contains(barcode);
            return model;
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromFactory()
        {
            return _iReportGateway.GetDistributedProductFromFactory();
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromBranch()
        {
            return _iReportGateway.GetDistributedProductFromBranch();
        }

        public bool IsDistributedFromFactory(string barcode)
        {
          return  GetDistributedProductFromFactory(barcode)!=null;
        }

        public bool IsDistributedFromBranch(string barcode)
        {
            return GetDistributedProductFromBranch(barcode) != null;
        }

        public bool IsAllreadyUpdatedSaleDateInFactory(string barcode)
        {
            
            return GetDistributedProductFromFactory().ToList().Find(n => n.BarCode == barcode && n.IsSaleDateUpdated)!=null;
        }

        public bool IsAllreadyUpdatedSaleDateInBranch(string barcode)
        {
            return GetDistributedProductFromBranch().ToList().Find(n => n.BarCode == barcode && n.IsSaleDateUpdated) != null;
        }

        public ViewDisributedProduct GetDistributedProductFromFactory(string barcode)
        {
            return _iReportGateway.GetDistributedProductFromFactory(barcode); 
        }

        public ViewDisributedProduct GetDistributedProductFromBranch(string barcode)
        {
            return _iReportGateway.GetDistributedProductFromBranch(barcode); 
        }

        public ICollection<ViewProduct> GetTotalStock()
        {
            return _iReportGateway.GetTotalStock();
        }

        public ICollection<ViewLoginInfo> GetLoginHistoryByDate(DateTime date)
        {
            return _iReportGateway.GetLoginHistoryByDate(date);
        }

        public ICollection<OrderHistory> GetDistributionSetOrders()
        {
            return _iReportGateway.GetDistributionSetOrders();
        }

        public ICollection<UserWiseOrder> UserWiseOrders()
        {
            return _iReportGateway.UserWiseOrders();
        }

        public ViewProductHistory GetProductHistoryByBarCode(string barcode)
        {
            return _iReportGateway.GetProductHistoryByBarCode(barcode);
        }

        public ICollection<ViewOrderHistory> GetOrderHistoriesByYear(int year)
        {
            return _iReportGateway.GetOrderHistoriesByYear(year);
        }

        public ICollection<TerritoryWiseDeliveredQty> GetTerritoryWishTotalSaleQtyByBranchId(int branchId)
        {
            return _iReportGateway.GetTerritoryWishTotalSaleQtyByBranchId(branchId);
        }

        public ICollection<ViewClient> GetClientList()
        {
            return _iReportGateway.GetClientList();
        }

        public ICollection<ProductDetails> GetAllProductDetails()
        {
            return _iReportGateway.GetAllProductDetails();
        }

        public ViewEntityCount GetTotalEntityCount()
        {
            return _iReportGateway.GetTotalEntityCount();
        }

        public ICollection<ViewOrderHistory> GetOrderHistoriesByYearAndDistributionPointId(int year, int distributionPointId)
        {
            return _iReportGateway.GetOrderHistoriesByYearAndDistributionPointId(year,distributionPointId);
        }

        public ICollection<ViewProductTransactionDetails> GetProductTransactionDetailsByBarcode(string barcode)
        {
            return _iReportGateway.GetProductTransactionDetailsByBarcode(barcode);
        }

        public ICollection<ViewProduct> GetStockProductBarcodeByBranchAndProductId(int branchId, int productId)
        {
            return _iReportGateway.GetStockProductBarcodeByBranchAndProductId(branchId,productId);
        }
        public ICollection<ViewProduct> GetStockProductBarcodeByBranchId(int branchId)
        {
            return _iReportGateway.GetStockProductBarcodeByBranchId(branchId);
        }

        public List<ViewProduct> GetStockProductToclientByClientIdWithBarcode(int clientId)
        {
            return _iReportGateway.GetStockProductToclientByClientIdWithBarcode(clientId);
        }

        public ICollection<ViewReplaceSummary> GetTotalReplaceProductList()
        {
            return _iReportGateway.GetTotalReplaceProductList();
        }
    }
}