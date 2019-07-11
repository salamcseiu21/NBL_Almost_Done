using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Services;

namespace NBL.DAL.Contracts
{
   public interface IServiceGateway:IGateway<WarrantyBatteryModel>
    {
        int ReceiveServiceProduct(WarrantyBatteryModel product);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts();
        long GetMaxWarrantyProductReceiveSlNoByYear(int year);
        ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId);
    }
}
