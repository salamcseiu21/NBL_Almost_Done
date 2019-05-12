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

namespace NBL.DAL.Contracts
{
   public interface IInventoryGateway
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       int GetMaxDeliveryRefNoOfCurrentYear();
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       ICollection<ReceiveProductViewModel> GetAllReceiveableListByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(ViewDispatchModel model);
       int SaveReceiveProductDetails(ViewDispatchModel model, int inventoryId);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       int SaveDeliveredOrder(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus); 
       int SaveDeliveredOrderDetails(List<ScannedProduct> scannedProducts,Delivery aDelivery, int inventoryId, int deliveryId);
       ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByTripId(long tripId,int branchId);
       TransactionModel GetTransactionModelById(long id);
       int SaveScannedProduct(List<ScannedProduct> scannedProducts,int userId);
       ScannedProduct IsThisProductSold(string scannedBarCode);
       ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode);
       ScannedProduct IsThisProductDispachedFromFactory(string scannedBarCode);
       ScannedProduct IsThisProductAlreadyInFactoryInventory(string scannedBarCode);
       ICollection<ViewFactoryStockModel> GetStockProductInFactory();
       ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory();
       ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode);
       IEnumerable<ViewProduct> GetAllProductsBarcode();
       int CreateTrip(ViewCreateTripModel model);
       long GetMaxTripRefNoOfCurrentYear();
       IEnumerable<ViewTripModel> GetAllTrip();
       ViewDispatchModel GetDispatchByTripId(long tripId);
       ICollection<ViewDispatchModel> GetAllReceiveableItemsByTripAndBranchId(long tripId, int branchId);
       long GetMaxVoucherNoByTransactionInfix(string infix);
       ICollection<ProductionSummary> GetProductionSummaries();
       ICollection<ProductionSummary> GetProductionSummaryByMonth(DateTime dateTime);
       int SaveReplaceDeliveryInfo(List<ScannedProduct> scannedProducts, Delivery aDelivery, int replaceStatus);
       int SaveTransferDeliveredProduct(TransferModel aModel);
       ICollection<ViewTransferProductModel> GetAllTransferedListByBranchAndCompanyId(int branchId, int companyId);
       List<string> GetTransferReceiveableBarcodeList(long transferId);
       int ReceiveTransferedProduct(TransferModel aModel);
       int SaveDeliveredOrderFromFactory(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus);
       ICollection<ViewProduct> GetTotalReceiveProductByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewProduct> GetDeliveredProductByBranchAndCompanyId(int branchId, int companyId);
   }
}
