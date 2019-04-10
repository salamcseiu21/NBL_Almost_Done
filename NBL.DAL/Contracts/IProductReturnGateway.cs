using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels.Returns;

namespace NBL.DAL.Contracts
{
    public interface IProductReturnGateway:IGateway<ReturnModel> 
    {
        int SaveReturnProduct(ReturnModel returnModel);
        long GetMaxSalesReturnNoByYear(int year);
        long GetMaxSalesReturnRefByYear(int year);
        ICollection<ReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId);
        ReturnDetails GetReturnDetailsById(int salsesReturnDetailsId);
        int ApproveReturnByNsm(string remarks, long salesReturnId, int userUserId);
        ICollection<ReturnModel> GetAllReturnsByStatus(int status);
        ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId);
        int ReceiveSalesReturnProduct(ViewReturnReceiveModel model);
        long GetMaxSalesReturnReceiveRefByYear(int year);   
    }
}
