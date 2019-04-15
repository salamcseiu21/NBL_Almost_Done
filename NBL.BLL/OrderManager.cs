using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Products;
using NBL.Models.Enums;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

namespace NBL.BLL
{
    public class OrderManager:IOrderManager
    {

       private readonly IOrderGateway _iOrderGateway;
       private readonly ICommonGateway _iCommonGateway;

        public OrderManager(IOrderGateway iOrderGateway,ICommonGateway iCommonGateway)
        {
            _iCommonGateway = iCommonGateway;
            _iOrderGateway = iOrderGateway;
        }

        public IEnumerable<Order> GetAll()
        {
            return _iOrderGateway.GetAll();
        } 

        public IEnumerable<Order> GetOrdersByBranchId(int branchId)
        {
            return _iOrderGateway.GetOrdersByBranchId(branchId);
        }
        public IEnumerable<ViewOrder> GetOrdersByCompanyId(int companyId)
        {
            var orders = _iOrderGateway.GetOrdersByCompanyId(companyId);
            foreach (var order in orders)
            {
                order.OrderItems = _iOrderGateway.GetOrderItemsByOrderId(order.OrderId);
            }
            return orders;
        }
        
        public IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId)
        {
           return _iOrderGateway.GetOrderListByClientId(clientId);
        }

        public IEnumerable<ViewOrder> GetOrdersByBranchAndCompnayId(int branchId, int companyId)
        {
            var orders = _iOrderGateway.GetOrdersByBranchAndCompnayId(branchId, companyId);
            return orders;

        }
        public IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId)
        {
            var orders = _iOrderGateway.GetAllOrderWithClientInformationByCompanyId(companyId);
            foreach (var order in orders)
            {
                order.OrderItems = _iOrderGateway.GetOrderItemsByOrderId(order.OrderId);
            }
            return orders;
        }

        public IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId, int companyId)
        {
            return _iOrderGateway.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId);
        }

        public IEnumerable<ViewOrder> GetOrdersByBranchCompanyAndNsmUserId(int branchId, int companyId, int nsmUserId)
        {
            return _iOrderGateway.GetOrdersByBranchCompanyAndNsmUserId(branchId, companyId, nsmUserId);
        }
        public IEnumerable<ViewOrder> GetOrdersByNsmUserId(int nsmUserId)
        {
            return _iOrderGateway.GetOrdersByNsmUserId(nsmUserId);
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchIdCompanyIdAndStatus(int branchId, int companyId,int status)
        {
            return _iOrderGateway.GetOrdersByBranchIdCompanyIdAndStatus(branchId,companyId,status);
        }

        public IEnumerable<ViewOrder> GetPendingOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iOrderGateway.GetPendingOrdersByBranchAndCompanyId(branchId, companyId);
        }
        public IEnumerable<ViewOrder> GetLatestOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iOrderGateway.GetLatestOrdersByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<OrderDetails> GetOrderDetailsByOrderId(long orderId)
        {
            var orderModels = _iOrderGateway.GetOrderDetailsByOrderId(orderId);
            return orderModels;
        }

        
        public int Save(Order order)
        {
            int maxSl = _iOrderGateway.GetOrderMaxSerialNoByYear(DateTime.Now.Year);
            order.OrderSlipNo = GenerateOrderSlipNo(maxSl);
            order.OrederRef = GenerateOrderRefNo(maxSl);
            int rowAffected= _iOrderGateway.Save(order);
            return rowAffected;
        }

        private string GenerateOrderRefNo(int maxsl)
        {
            string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.Sales));
            int sN =maxsl+1;
            string reference = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return reference;
        }

        private string GenerateOrderSlipNo(int maxSl)
        {
            string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.Sales));
            int sN=1+maxSl;
            string ordSlipNo = DateTime.Now.Date.Year.ToString().Substring(2, 2)+refCode+sN;
            return ordSlipNo;
        }

        private string GetReferenceAccountCodeById(int subReferenceAccountId)   
        {
            var code=_iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n=>n.Id.Equals(subReferenceAccountId)).Code;
            return code;
        }

        public string ApproveOrderByNsm(ViewOrder order)
        {
            int rowAffected = _iOrderGateway.ApproveOrderByNsm(order);
            if (rowAffected > 0)
            {
                return "Approved by NSM Successfully!";
            }

            return "Failed to approve by NSM";
        }

        public string ApproveOrderByAdmin(ViewOrder order)
        {
            int rowAffected = _iOrderGateway.ApproveOrderByAdmin(order); 
            if (rowAffected > 0)
            {
                return "Approved by Admin Successfully!";
            }

            return "Failed to approve by Admin";
        }

        public ViewOrder GetOrderByOrderId(int orderId)
        {
            var order = _iOrderGateway.GetOrderByOrderId(orderId);
            order.OrderItems = _iOrderGateway.GetOrderItemsByOrderId(orderId);
            return order;
        }
      
        public bool CancelOrder(ViewOrder order)
        {
            return _iOrderGateway.CancelOrder(order)>0;
        }

        public IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId)
        {
            var orders= _iOrderGateway.GetLatestOrdersByCompanyId(companyId);
            foreach (ViewOrder order in orders)
            {
                order.OrderItems= _iOrderGateway.GetOrderItemsByOrderId(order.OrderId);
            }

            return orders.ToList();

        }

        public string UpdateOrderDetails(IEnumerable<OrderItem> orderItems)
        {
            int rowAffected = _iOrderGateway.UpdateOrderDetails(orderItems);
            return rowAffected > 0 ? "Updated Successfully!" : "Failed to Update";
        }

        public bool DeleteProductFromOrderDetails(long orderItemId) 
        {
            int rowAffected = _iOrderGateway.DeleteProductFromOrderDetails(orderItemId);
            return rowAffected > 0;
        }

        public string GetDiscountAccountCodeByClintTypeId(int typeId)
        {
            string discountCode= "26010";

            // 1=Individual or retail Customer
            // 2= Corporate coustomer 
            // 3=Dealer
            if(typeId==1)
            {
                discountCode += "21";
            }
            if (typeId == 3)
            {
                discountCode += "11";
            }
            if (typeId == 2)
            {
                discountCode += "31";
            }
            return discountCode;
        }

        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
            return _iOrderGateway.GetAllOrderDetails();
        }

        public Order GetOrderInfoByTransactionRef(string transactionRef)
        {
            return _iOrderGateway.GetOrderInfoByTransactionRef(transactionRef);
        }

        public bool AddNewItemToExistingOrder(Product aProduct,int orderId)
        {
            return _iOrderGateway.AddNewItemToExistingOrder(aProduct,orderId)>0;
        }

        public bool UpdateOrder(ViewOrder order)
        {
            int rowAffected = _iOrderGateway.UpdateOrder(order);
            return rowAffected > 0;
        }

        public IEnumerable<ChartModel> GetTotalOrdersOfCurrentYearByCompanyId(int companyId)
        {
            return _iOrderGateway.GetTotalOrdersOfCurrentYearByCompanyId(companyId);
        }
        public IEnumerable<ChartModel> GetTotalOrdersByBranchIdCompanyIdAndYear(int branchId, int companyId,int year) 
        {
            return _iOrderGateway.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId,companyId,year);
        }

        public IEnumerable<Order> GetOrdersByClientId(int clientId)
        {
           return _iOrderGateway.GetOrdersByClientId(clientId);
        }

        public List<Product> GetProductListByOrderId(int orderId)
        {
           return _iOrderGateway.GetProductListByOrderId(orderId);
        }

        public IEnumerable<ChartModel> GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            return _iOrderGateway.GetTotalOrdersByCompanyIdAndYear(companyId, year);
        }

        public IEnumerable<ChartModel> GetTotalOrdersByYear(int year)
        {
            return _iOrderGateway.GetTotalOrdersByYear(year);
        }

        public ViewOrderSlipModel GetOrderSlipByOrderId(int orderId)
        {
           var order =GetOrderByOrderId(orderId);
           var aModel=new ViewOrderSlipModel
           {
               ViewOrder = order
           };
            return aModel;
        }
        public IEnumerable<ViewOrder> GetDelayedOrdersToSalesPersonByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iOrderGateway.GetDelayedOrdersToSalesPersonByBranchAndCompanyId(branchId, companyId);
        }
        public IEnumerable<ViewOrder> GetDelayedOrdersToNsmByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iOrderGateway.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
        }

        public IEnumerable<ViewOrder> GetDelayedOrdersToAdminByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iOrderGateway.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
        }

        public bool UpdateVerificationStatus(int orderId, string verificationNote, int userUserId)
        {
            return _iOrderGateway.UpdateVerificationStatus(orderId,verificationNote,userUserId)>0;
        }

        public IEnumerable<ViewVerifiedOrderModel> GetVerifiedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iOrderGateway.GetVerifiedOrdersByBranchAndCompanyId(branchId,companyId);
        }

        string IOrderManager.GenerateOrderRefNo(int maxsl)
        {
            throw new NotImplementedException();
        }

        string IOrderManager.GenerateOrderSlipNo(int maxSl)
        {
            throw new NotImplementedException();
        }

        string IOrderManager.GetReferenceAccountCodeById(int subReferenceAccountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewOrder> GetOrder(SearchCriteriaModel searchCriteria)
        {
            return _iOrderGateway.GetOrder(searchCriteria);
        }

        public bool SaveSoldProductBarCode(RetailSale retail)
        {
            int rowAffected = _iOrderGateway.SaveSoldProductBarCode(retail); 
            return rowAffected > 0;
        }
    }


}