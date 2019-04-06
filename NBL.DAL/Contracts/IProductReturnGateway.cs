using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Returns;

namespace NBL.DAL.Contracts
{
    public interface IProductReturnGateway:IGateway<ReturnModel> 
    {
        int SaveReturnProduct(ReturnModel returnModel);
        long GetMaxSalesReturnNoByYear(int year);
        long GetMaxSalesReturnRefByYear(int year);
        ICollection<ReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId);
        ReturnDetails GetReturnDetailsById(int salsesReturnDetailsId);
    }
}
