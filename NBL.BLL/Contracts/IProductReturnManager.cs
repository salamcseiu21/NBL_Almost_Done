
using System.Collections.Generic;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels.Returns;

namespace NBL.BLL.Contracts
{
    public interface IProductReturnManager:IManager<ReturnModel>
    {
        bool SaveReturnProduct(ReturnModel returnModel);
        ICollection<ViewReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId);
        ViewReturnDetails GetReturnDetailsById(long salsesReturnDetailsId);
        bool ApproveReturnBySalesManager(ReturnModel returnModel);
        ICollection<ReturnModel> GetAllReturnsByStatus(int status);
        ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId);
        bool ReceiveSalesReturnProduct(ViewReturnReceiveModel model);
        ICollection<ViewReturnProductModel> GetSalesReturnProductListToTest();
        bool AddVerificationNoteToReturnsProduct(long returnRcvDetailsId, string notes, int userUserId);
        ICollection<ViewReturnProductModel> GetAllVerifiedSalesReturnProducts();
        bool ApproveReturnBySalesAdmin(ReturnModel returnModel,decimal lessAmount);
        ICollection<ReturnModel> GetAllReturnsByApprovarRoleId(int approverRoleId);
        ICollection<ReturnModel> GetAllFinalApprovedReturnsList();
    }
}
