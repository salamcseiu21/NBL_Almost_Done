using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels.Returns;

namespace NBL.DAL.Contracts
{
    public interface IProductReturnGateway:IGateway<ReturnModel> 
    {
        int SaveReturnProduct(ReturnModel returnModel);
        long GetMaxSalesReturnNoByYear(int year);
        long GetMaxSalesReturnRefByYear(int year);
        ICollection<ViewReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId);
        ViewReturnDetails GetReturnDetailsById(long salsesReturnDetailsId);
        int ApproveReturnBySalesManager(ReturnModel returnModel);
        ICollection<ReturnModel> GetAllReturnsByStatus(int status);
        ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId);
        int ReceiveSalesReturnProduct(ViewReturnReceiveModel model);
        long GetMaxSalesReturnReceiveRefByYear(int year);
        ICollection<ViewReturnProductModel> GetSalesReturnProductListToTest();
        int AddVerificationNoteToReturnsProduct(long returnRcvDetailsId, string notes, int userUserId);
        ICollection<ViewReturnProductModel> GetAllVerifiedSalesReturnProducts();
        int ApproveReturnBySalesAdmin(ReturnModel returnModel, decimal lessAmount);
        ICollection<ReturnModel> GetAllReturnsByApprovarRoleId(int approverRoleId);
        ICollection<ReturnModel> GetAllFinalApprovedReturnsList();
    }
}
