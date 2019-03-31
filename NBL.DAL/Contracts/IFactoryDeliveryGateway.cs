using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.DAL.Contracts
{
    public interface IFactoryDeliveryGateway:IGateway<Delivery>
    {
        int SaveDispatchInformation(DispatchModel dispatchModel); 
       
    }
}
