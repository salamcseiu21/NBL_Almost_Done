
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
        bool ApproveReturnByNsm(string remarks, long salesReturnId, int userUserId);
        ICollection<ReturnModel> GetAllReturnsByStatus(int status);
        ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId);
        bool ReceiveSalesReturnProduct(ViewReturnReceiveModel model);
        ICollection<ViewReturnProductModel> GetSalesReturnProductListToTest();
        bool AddVerificationNoteToReturnsProduct(long returnRcvDetailsId, string notes, int userUserId);
        ICollection<ViewReturnProductModel> GetAllVerifiedSalesReturnProducts();
        bool ApproveReturnBySalesAdmin(string remarks, long salesReturnId, int userId,decimal lessAmount);  
    }
}
