
using System.Collections.Generic;
using NBL.Models.EntityModels.Returns;

namespace NBL.BLL.Contracts
{
    public interface IProductReturnManager:IManager<ReturnModel>
    {
        bool SaveReturnProduct(ReturnModel returnModel);
        ICollection<ReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId);
        ReturnDetails GetReturnDetailsById(int salsesReturnDetailsId);
        bool ApproveReturnByNsm(string remarks, long salesReturnId, int userUserId);
        ICollection<ReturnModel> GetAllReturnsByStatus(int status); 
    }
}
