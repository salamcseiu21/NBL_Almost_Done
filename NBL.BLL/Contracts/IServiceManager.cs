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
        ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarId(int forwardId);
        bool ForwardServiceBattery(ForwardDetails model);
        bool SaveCharegeReport(ChargeReportModel model);
        bool SaveDischargeReport(DischargeReportModel model);
    }
}
