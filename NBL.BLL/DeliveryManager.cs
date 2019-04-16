using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;

namespace NBL.BLL
{
    public class DeliveryManager:IDeliveryManager
    {
        private readonly IDeliveryGateway _iDeliveryGateway;
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;

        public DeliveryManager(IOrderManager iOrderManager,IClientManager iClientManager,IDeliveryGateway iDeliveryGateway)
        {
            _iOrderManager = iOrderManager;
            _iClientManager = iClientManager;
            _iDeliveryGateway = iDeliveryGateway;
        }
        public int ChangeOrderStatusByManager(Order aModel) 
        {
            int rowAffected = _iDeliveryGateway.ChangeOrderStatusByManager(aModel);  
            return rowAffected;
        }

        public IEnumerable<Delivery> GetAllDeliveredOrders() 
        {
            return _iDeliveryGateway.GetAllDeliveredOrders();
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iDeliveryGateway.GetAllDeliveredOrdersByBranchAndCompanyId(branchId,companyId);
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,int deliveredByUserId)
        {
            var deliveredOrders =
                _iDeliveryGateway.GetAllDeliveredOrdersByBranchCompanyAndUserId(branchId, companyId, deliveredByUserId);
            foreach (Delivery delivery in deliveredOrders)
            {
                var order = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                delivery.Client = _iClientManager.GetById(order.ClientId);
            }

            return deliveredOrders;
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef)
        {
            return _iDeliveryGateway.GetAllDeliveredOrdersByInvoiceRef(invoiceRef);
        }
        public Delivery GetOrderByDeliveryId(long deliveryId) 
        {
            return _iDeliveryGateway.GetOrderByDeliveryId(deliveryId);
        }

        public IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(long deliveryId) 
        {
            return _iDeliveryGateway.GetDeliveredOrderDetailsByDeliveryId(deliveryId);
        }

        public IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductId(long deliveryId,int productId) 
        {
            return  _iDeliveryGateway.GetDeliveredProductsByDeliveryIdAndProductId(deliveryId,productId);
        }
        public IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId)
        {
            return _iDeliveryGateway.GetAllInvoiceOrderListByBranchId(branchId);
        }

        public ViewChalanModel GetChalanByDeliveryId(int deliveryId) 
        {
            Delivery delivery =GetOrderByDeliveryId(deliveryId);
            var details = GetDeliveredOrderDetailsByDeliveryId(deliveryId);
            foreach (DeliveryDetails deliveryDetailse in details)
            {
                deliveryDetailse.DeliveredProducts = GetDeliveredProductsByDeliveryIdAndProductId(deliveryId,deliveryDetailse.ProductId).ToList();
            }
            Order order = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            var client = _iClientManager.GetClientDeailsById(order.ClientId);
            var chalan = new ViewChalanModel
            {
                DeliveryDetailses = details,
                ViewClient = client
            };
            return chalan;
        }

        public ICollection<ViewDeliveredOrderModel> GetDeliveredOrderByClientId(int clientId)
        {
            return _iDeliveryGateway.GetDeliveredOrderByClientId(clientId);
        }

        public ICollection<ViewDeliveredOrderModel> GetDeliveryDetailsInfoByDeliveryId(long deliveryId)
        {
            return _iDeliveryGateway.GetDeliveryDetailsInfoByDeliveryId(deliveryId);
        }
    }
}