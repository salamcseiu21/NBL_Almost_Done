
using System;
using System.Collections.Generic;

using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;

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
       

        Delivery GetOrderByDeliveryId(long deliveryId);
       

        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(long deliveryId);
        

        IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId);
        

        ViewChalanModel GetChalanByDeliveryId(int deliveryId);

       ICollection<ViewDeliveredOrderModel> GetDeliveredOrderByClientId(int clientId);
       ICollection<ViewDeliveredOrderModel> GetDeliveryDetailsInfoByDeliveryId(long deliveryId);
       ICollection<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyAndUserId(int branchId, int companyId, int userId);
       ICollection<Delivery> GetAllDeliveredOrdersByDistributionPointAndCompanyId(int branchId, int companyId);
        ViewChalanModel GetChalanByDeliveryIdFromFactory(int deliveryId);
       ViewChalanModel GetDeliveredReplaceBarcodeListbyDeliveryId(int deliveryId);
       ICollection<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyDateAndUserId(int branchId, int companyId, DateTime now, int userId);
       ICollection<ViewProduct> GetDeliveredProductListByTransactionRef(string deliveryRef);
       IEnumerable<ViewDeliveredOrderModel> GetDeliveredGeneralReqById(long deliveryId);
       List<Delivery> GetAllDeliveredOrdersByBranchAndCompany(int branchId, int companyId, int uorderByUserId);
       ICollection<ViewClientStockProduct> GetClientStockProductAgeByDeliveryId(long deliveryId); 
   }
}
