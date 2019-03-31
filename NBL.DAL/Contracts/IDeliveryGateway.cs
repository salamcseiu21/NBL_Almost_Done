﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IDeliveryGateway:IGateway<Delivery>
    {
        int ChangeOrderStatusByManager(Order aModel);
        Delivery GetOrderByDeliveryId(int deliveryId);
        IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId);
        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(int deliveryId);
        IEnumerable<Delivery> GetAllDeliveredOrders();
        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId);

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,
            int deliveredByUserId);

        IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef);
        IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductId(long deliveryId, int productId);
    }
}
