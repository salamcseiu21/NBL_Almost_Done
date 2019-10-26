using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace NBL.BLL.Contracts
{
   public interface IReportManager
    {
       
        IEnumerable<ViewClient> GetTopClients();
        IEnumerable<ViewClient> GetTopClientsByYear(int year);
        IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId);
        IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year);
        IEnumerable<ViewProduct> GetPopularBatteries();
        IEnumerable<ViewProduct> GetPopularBatteriesByYear(int year);
        IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId);
        IEnumerable<ViewProduct> GetPopularBatteriesByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
        ViewTotalOrder GetTotalOrderByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
        ViewTotalOrder GetTotalOrdersByCompanyIdAndYear(int companyId, int year);
        ViewTotalOrder GetTotalOrdersByYear(int year);
        ViewTotalProduction GetTotalProductionCompanyIdAndYear(int companyId, int year);
        ViewTotalDispatch GetTotalDispatchCompanyIdAndYear(int companyId, int year);
        ViewTotalOrder GetTotalOrdersByBranchCompanyAndYear(int branchid,int companyId, int year);
        bool IsValiedBarcode(string barcode);
        ICollection<ViewDisributedProduct> GetDistributedProductFromFactory(); 
        ICollection<ViewDisributedProduct> GetDistributedProductFromBranch();
        bool IsDistributedFromFactory(string barcode);
        bool IsDistributedFromBranch(string barcode);
        bool IsAllreadyUpdatedSaleDateInFactory(string barcode);
        bool IsAllreadyUpdatedSaleDateInBranch(string barcode); 
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
        ICollection<ViewProduct> GetStockProductBarcodeByBranchAndProductId(int branchId, int id);
        ICollection<ViewProduct> GetStockProductBarcodeByBranchId(int branchId);
        List<ViewProduct> GetStockProductToclientByClientIdWithBarcode(int clientId);
        ICollection<ViewReplaceSummary> GetTotalReplaceProductList();
        ViewTotalSaleValue GetTotalSaleValueByYear(int year);
        ViewTotalCollection GetTotalCollectionByYear(int year);
        decimal GetTotalSaleValueByYearAndMonth(int year, int month);
        ICollection<ViewDeliveredQuantityModel> GetTotalDeliveredQtyByBranchId(int branchId);
        ViewTotalDeliveredQuantity GetTotalDeliveredQuantityByYear(int year);
        ICollection<ViewClientSummaryModel> GetClientReportBySearchCriteria(SearchCriteria searchCriteria);
        ICollection<ViewStockProduct> GetStockProductBarcodeByBranchAndProductIdTemp(int branchId, int productId);
        bool InActiveProduct(int i, List<ViewStockProduct> stockBarcodList);
        ICollection<ViewDeliveredOrderModel> GetTotalDeliveredOrderListByDistributionPointId(int branchId);
        ICollection<SubSubSubAccount> GetBankStatementByYear(int year); 
    }
}
