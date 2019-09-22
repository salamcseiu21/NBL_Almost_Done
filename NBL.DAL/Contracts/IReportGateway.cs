using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Reports;
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
       List<ViewProduct> GetStockProductBarcodeByBranchAndProductId(int branchId, int productId);
   }
}
