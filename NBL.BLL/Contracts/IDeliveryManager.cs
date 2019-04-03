
using System.Collections.Generic;

using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;

namespace NBL.BLL.Contracts
{
   public interface IDeliveryManager
   {
       int ChangeOrderStatusByManager(Order aModel);
       

        IEnumerable<Delivery> GetAllDeliveredOrders();
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId);
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,
            int deliveredByUserId);
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef);
       

        Delivery GetOrderByDeliveryId(int deliveryId);
       

        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(int deliveryId);
        

        IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId);
        

        ViewChalanModel GetChalanByDeliveryId(int deliveryId);

       ICollection<ViewDeliveredOrderModel> GetDeliveredOrderByClientId(int clientId);
       ICollection<ViewDeliveredOrderModel> GetDeliveryDetailsInfoByDeliveryId(long deliveryId);
   }
}
