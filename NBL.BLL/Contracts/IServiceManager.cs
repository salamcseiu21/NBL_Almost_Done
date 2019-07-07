using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL.Contracts
{
    public interface IServiceManager:IManager<WarrantyBatteryModel>
    {
        bool ReceiveServiceProduct(WarrantyBatteryModel product);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts();
    }
}
