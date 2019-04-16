using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Sales;

using NBL.Models.ViewModels.TransferProducts;
namespace NBL.BLL
{
    public class InventoryManager:IInventoryManager
    {

        private readonly  IInventoryGateway _iInventoryGateway;
        private readonly ICommonGateway _iCommonGateway;
        readonly CommonGateway _commonGateway = new CommonGateway();
      
        public InventoryManager(IInventoryGateway iInventoryGateway,ICommonGateway iCommonGateway)
        {
            _iInventoryGateway = iInventoryGateway;
            _iCommonGateway = iCommonGateway;
        }
      
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetStockProductByBranchAndCompanyId(branchId, companyId);
        }
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {
            return _iInventoryGateway.GetStockProductByCompanyId(companyId);
        }

        public ICollection<ReceiveProductViewModel> GetAllReceiveableListByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iInventoryGateway.GetAllReceiveableListByBranchAndCompanyId(branchId,companyId); 
        }

        public int ReceiveProduct(ViewDispatchModel model) 
        {
            
            return _iInventoryGateway.ReceiveProduct(model);
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByDeliveryRef(deliveryRef);
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            return _iInventoryGateway.GetStockQtyByBranchAndProductId(branchId,productId);
        }

        public string SaveDeliveredOrder(List<ScannedProduct> scannedProducts, Delivery aDelivery,int invoiceStatus,int orderStatus) 
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Distribution)).Code;
            aDelivery.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.SaveDeliveredOrder(scannedProducts, aDelivery, invoiceStatus, orderStatus);

            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        private long GetMaxVoucherNoByTransactionInfix(string infix)
        {
            var temp =_iInventoryGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        public string GenerateDeliveryReference(int maxRefNo)
        {

            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(Convert.ToInt32(ReferenceType.Distribution))).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference =DateTime.Now.Year.ToString().Substring(2,2)+refCode+ temp;
            return reference;
        }

        public ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByTripId(long tripId,int branchId)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByTripId(tripId,branchId);
        }

        public TransactionModel GetTransactionModelById(long id)
        {
            return _iInventoryGateway.GetTransactionModelById(id);
        }

        public int SaveScannedProduct(List<ScannedProduct> scannedProducts,int userId)
        {
            return _iInventoryGateway.SaveScannedProduct(scannedProducts, userId); 
        }

        public bool IsThisProductSold(string scannedBarCode)
        {
            var scannedProduct = _iInventoryGateway.IsThisProductSold(scannedBarCode);
            return scannedProduct != null;
        }

        public ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode)
        {
            return _iInventoryGateway.OldestProductByBarcode(scannedBarCode);
        }

        public bool IsThisProductDispachedFromFactory(string scannedBarCode)
        {
            var scannedProduct = _iInventoryGateway.IsThisProductDispachedFromFactory(scannedBarCode); 
            return scannedProduct != null;
        }

        public bool IsThisProductAlreadyInFactoryInventory(string scannedBarCode)
        {
            var scannedProduct = _iInventoryGateway.IsThisProductAlreadyInFactoryInventory(scannedBarCode); 
            return scannedProduct != null;
        }

        public ICollection<ViewFactoryStockModel> GetStockProductInFactory()
        {
            return _iInventoryGateway.GetStockProductInFactory();
        }

        public ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetStockProductInBranchByBranchAndCompanyId(branchId,companyId);
        }

        public ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory()
        {
            return _iInventoryGateway.GetAllProductTransactionFromFactory();
        }

        public ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode)
        {
            return _iInventoryGateway.GetProductLifeCycleByBarcode(productBarCode);
        }

        public IEnumerable<ViewProduct> GetAllProductsBarcode()
        {
            return _iInventoryGateway.GetAllProductsBarcode();
        }

        public bool CreateTrip(ViewCreateTripModel model)
        {
           var maxSlNo= _iInventoryGateway.GetMaxTripRefNoOfCurrentYear();
            model.TripRef = GenerateTripReference(maxSlNo);
            int rowAffected = _iInventoryGateway.CreateTrip(model);
            return rowAffected > 0;
        }

      
        private string GenerateTripReference(long maxRefNo) 
        {

            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(Convert.ToInt32(ReferenceType.Trip))).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }
        public IEnumerable<ViewTripModel> GetAllTrip()
        {
            return _iInventoryGateway.GetAllTrip();
        }

        public ViewDispatchModel GetDispatchByTripId(long tripId)
        {
            return _iInventoryGateway.GetDispatchByTripId(tripId); 
        }

        public ICollection<ViewDispatchModel> GetAllReceiveableItemsByTripAndBranchId(long tripId, int branchId)
        {
            return _iInventoryGateway.GetAllReceiveableItemsByTripAndBranchId(tripId,branchId);
        }
    }
}