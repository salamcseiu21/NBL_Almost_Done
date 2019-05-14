using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL.Contracts
{
    public interface IFactoryDeliveryManager:IManager<Delivery>
    {
        string SaveDispatchInformation(DispatchModel dispatchModel);
        DispatchModel GetDispatchByDispatchId(long dispatchId);
        ViewDispatchChalan GetDispatchChalanByDispatchId(long dispatchId);
        ICollection<ViewDispatchModel> GetDispatchDetailsByDispatchId(long dispatchId);
    }
}
