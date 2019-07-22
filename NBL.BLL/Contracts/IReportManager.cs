using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Reports;

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
       
    }
}
