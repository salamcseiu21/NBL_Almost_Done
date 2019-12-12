using System;
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Reports;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.Summaries;

namespace NBL.DAL.Contracts
{
   public interface IReportGateway
   {
       IEnumerable<ViewClient> GetTopClients();
       IEnumerable<ViewClient> GetTopClientsByYear(int year);
       IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId);
       IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year);
       IEnumerable<ViewProduct> GetPopularBatteries();
       IEnumerable<ViewProduct> GetPopularBatteriesByYear(int year);
       IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetPopularBatteriesByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
       ICollection<ViewDisributedProduct> GetDistributedProductFromFactory();
       ICollection<ViewDisributedProduct> GetDistributedProductFromBranch();
        ViewDisributedProduct GetDistributedProductFromFactory(string barcode);
        ViewDisributedProduct GetDistributedProductFromBranch(string barcode);
       ICollection<ViewProduct> GetTotalStock();
       ICollection<ViewLoginInfo> GetLoginHistoryByDate(DateTime date);
       ICollection<OrderHistory> GetDistributionSetOrders();
       ICollection<UserWiseOrder> UserWiseOrders();
       ViewProductHistory GetProductHistoryByBarCode(string barcode);
       ICollection<ViewOrderHistory> GetOrderHistoriesByYear(int year);
       ICollection<TerritoryWiseDeliveredQty> GetTerritoryWishTotalSaleQtyByBranchId(int branchId);
       ICollection<ViewClient> GetClientList();
       ICollection<ProductDetails> GetAllProductDetails();
       ViewEntityCount GetTotalEntityCount();
       ICollection<ViewOrderHistory> GetOrderHistoriesByYearAndDistributionPointId(int year, int distributionPointId);
       ICollection<ViewProductTransactionDetails> GetProductTransactionDetailsByBarcode(string barcode);
       ICollection<ViewProduct> GetStockProductBarcodeByBranchAndProductId(int branchId, int productId);
       ICollection<ViewProduct> GetStockProductBarcodeByBranchId(int branchId);
       List<ViewProduct> GetStockProductToclientByClientIdWithBarcode(int clientId);
       ICollection<ViewReplaceSummary> GetTotalReplaceProductList();

       ICollection<ChartModel> GetTotalSaleValueByYear(int year);
       ICollection<ChartModel> GetTotalCollectionByYear(int year);
       decimal GetTotalSaleValueByYearAndMonth(int year, int month);
       ICollection<ViewDeliveredQuantityModel> GetTotalDeliveredQtyByBranchId(int branchId);
       ICollection<ChartModel> GetTotalDeliveredQuantityByYear(int year);
       ICollection<ViewClientSummaryModel> GetClientReportBySearchCriteria(SearchCriteria searchCriteria);
       ICollection<ViewStockProduct> GetStockProductBarcodeByBranchAndProductIdTemp(int branchId, int productId);
       int InActiveProduct(int branchId, List<ViewStockProduct> stockBarcodList);
       ICollection<ViewDeliveredOrderModel> GetTotalDeliveredOrderListByDistributionPointId(int branchId);
       ICollection<SubSubSubAccount> GetBankStatementByYear(int year);
       ICollection<ViewSubSubSubAccount> GetAllSubSubSubAccountList();
       int IsAllreadyUpdatedSaleDate(string barcode);
       ICollection<ViewProductionSalesRepalce> GetProductionSalesRepalcesByMonthYear(int monthNo, int year);
       ViewDeliveryDetails GetDeliveryInfoByBarcode(string barcode);
       int IsDeliveryForReplace(string barcode);
       ViewDisributedProduct GetReplaceDistributedProduct(string barcode);
       ICollection<ViewProduct> ProductWiseTotalStock(int productId);
   }
}
