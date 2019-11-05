using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Reports;

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
        IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyAndUserId(int distributionPointId, int companyId, int userId);
        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryIdFromFactory(int deliveryId);
        IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductIdFromFactory(int deliveryId, int productId);
        IEnumerable<ViewReplaceDetailsModel> GetDeliveredReplaceDetailsByDeliveryId(int deliveryId);
        IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyDateAndUserId(int branchId, int companyId, DateTime date, int userId);
        IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyDate(int branchId, int companyId, DateTime deliveryDate); 
        IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointAndCompanyId(int branchId, int companyId);
        ICollection<ViewProduct> GetDeliveredProductListByTransactionRef(string deliveryRef);
        IEnumerable<ViewDeliveredOrderModel> GetDeliveredGeneralReqById(long deliveryId);
        ICollection<Delivery> GetAllDeliveredOrdersByBranchAndCompany(int branchId, int companyId, int orderByUserId);
        ICollection<ViewClientStockProduct> GetClientStockProductAgeByDeliveryId(long deliveryId);
        ICollection<ViewClientStockReport> GetAllClientsByClientTypeId(int clientTypeId);
        ICollection<ViewDeliveredOrderModel> GetDeliveredOrderBySearchCriteria(SearchCriteria aCriteria);
      
    }
}
