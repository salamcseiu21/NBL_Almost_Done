using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;

namespace NBL.DAL.Contracts
{
    public interface IDeliveryGateway:IGateway<Delivery>
    {
        int ChangeOrderStatusByManager(Order aModel);
        Delivery GetOrderByDeliveryId(long deliveryId);
        IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId);
        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(long deliveryId);
        IEnumerable<Delivery> GetAllDeliveredOrders();
        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId);

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,
            int deliveredByUserId);

        IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef);
        IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductId(long deliveryId, int productId);
        ICollection<ViewDeliveredOrderModel> GetDeliveredOrderByClientId(int clientId);
        ICollection<ViewDeliveredOrderModel> GetDeliveryDetailsInfoByDeliveryId(long deliveryId);
    }
}
