using System.Collections.Generic;
using NBL.Models.EntityModels;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Replaces;

namespace NBL.DAL.Contracts
{
    public interface IProductReplaceGateway:IGateway<ReplaceModel>
    {
        int SaveReplacementInfo(ReplaceModel model);
        int GetMaxReplaceSerialNoByYear(int year);
        ICollection<ViewReplaceModel> GetAllReplaceListByBranchCompanyAndStatus(int branchId,int companyId,int status);
        ICollection<ViewReplaceModel> GetAllPendingReplaceListByBranchAndCompany(int branchId, int companyId);
        ViewReplaceModel GetReplaceById(long id);
        ICollection<ViewReplaceDetailsModel> GetReplaceProductListById(long id);
        ICollection<ViewProduct> GetDeliveredProductsByReplaceRef(string replaceRef);

        int Cancel(ViewReplaceModel replaceModel, int userId);
        ICollection<ViewReplaceModel> GetAllDeliveredReplaceListByBranchAndCompany(int branchId, int companyId);
    }
}
