using System.Collections.Generic;
using NBL.Models.ViewModels;

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
   }
}
