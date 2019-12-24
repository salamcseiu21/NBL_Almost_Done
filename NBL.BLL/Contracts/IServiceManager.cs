using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Orders;
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
        ChargeReportModel GetChargeReprortByReceiveId(long id);
        DischargeReportModel GetDisChargeReprortByReceiveId(long id);
        ICollection<ViewSoldProduct> GetAllSoldProducts();
        bool SaveApprovalInformation(int userId, ForwardDetails forwardDetails);
        ICollection<Client> GetClientListByServiceForwardId(int forwardId);
        bool ForwardServiceBatteryToDeistributionPoint(long receiveId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByStatusAndBranchId(int status, int branchId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByStatus(int status);
        bool ReceiveServiceProductTemp(WarrantyBatteryModel warrantyBatteryModel);
        ViewReceivedServiceProduct GetDeliverableServiceProductById(long receivedId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarIdAndBranchId(int forwardId, int branchId);
        ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByBranchId(int branchId);
        ICollection<ViewSoldProduct> GetFolioListByBranchAndUserId(int branchId, int userId);
        bool UpdateCliaimedBatteryDeliveryStatus(long receiveId,DateTime deliveryDate);
       
    }
}
