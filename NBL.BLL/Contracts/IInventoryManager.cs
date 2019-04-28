using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.BLL.Contracts
{
   public interface IInventoryManager
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       ICollection<ReceiveProductViewModel> GetAllReceiveableListByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(ViewDispatchModel model);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       string SaveDeliveredOrder(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus);
       string GenerateDeliveryReference(int maxRefNo);
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByTripId(long tripId,int branchId);
       TransactionModel GetTransactionModelById(long id);
       int SaveScannedProduct(List<ScannedProduct> scannedProducts,int userId);
       bool IsThisProductSold(string scannedBarCode);
       ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode);
       bool IsThisProductDispachedFromFactory(string scannedBarCode);
       bool IsThisProductAlreadyInFactoryInventory(string scannedBarCode);
       ICollection<ViewFactoryStockModel> GetStockProductInFactory();
       ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory();
       ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode);
       IEnumerable<ViewProduct> GetAllProductsBarcode();
       bool CreateTrip(ViewCreateTripModel model);
       IEnumerable<ViewTripModel> GetAllTrip();
        ViewDispatchModel GetDispatchByTripId(long tripId);
       ICollection<ViewDispatchModel> GetAllReceiveableItemsByTripAndBranchId(long tripId, int branchId);
       ICollection<ProductionSummary> GetProductionSummaries();
       ICollection<ProductionSummary> GetProductionSummaryByMonth(DateTime dateTime); 
   }
}
