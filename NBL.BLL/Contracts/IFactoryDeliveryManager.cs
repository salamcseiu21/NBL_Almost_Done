using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL.Contracts
{
    public interface IFactoryDeliveryManager:IManager<Delivery>
    {
        string SaveDispatchInformation(DispatchModel dispatchModel);
    }
}
