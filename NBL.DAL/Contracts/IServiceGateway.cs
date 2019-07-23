using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Services;

namespace NBL.DAL.Contracts
{
   public interface IServiceGateway:IGateway<WarrantyBatteryModel>
    {
        int ReceiveServiceProduct(WarrantyBatteryModel product);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts();
        long GetMaxWarrantyProductReceiveSlNoByYear(int year);
        ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarId(int forwardId);
        int ForwardServiceBattery(ForwardDetails model);
        int SaveCharegeReport(ChargeReportModel model);
        int SaveDischargeReport(DischargeReportModel model);
        ChargeReportModel GetChargeReprortByReceiveId(long id);
        DischargeReportModel GetDisChargeReprortByReceiveId(long id);
        ICollection<ViewSoldProduct> GetAllSoldProducts();
        int SaveApprovalInformation(int userId, ForwardDetails forwardDetails);
    }
}
