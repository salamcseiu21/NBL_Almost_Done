
using NBL.Models.EntityModels.Returns;

namespace NBL.BLL.Contracts
{
    public interface IProductReturnManager:IManager<ReturnModel>
    {
        bool SaveReturnProduct(ReturnModel returnModel); 
    }
}
