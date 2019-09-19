using System;
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Returns;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
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
        //------------Receive Sales Return Product----------------
       bool ReceiveProduct(List<ScannedProduct> barcodeList, int branchId, int userId, FinancialTransactionModel financialModel,ReturnModel returnModel);
        int GetStockQtyByBranchAndProductId(int branchId, int productId);
       string GenerateDeliveryReference(int maxRefNo);
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByDispatchId(long dispatchId, int branchId);
       TransactionModel GetTransactionModelById(long id);
       int SaveScannedProduct(ProductionModel model);
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
        ViewDispatchModel GetDispatchByDispatchId(long dispatchId);
       ICollection<ViewDispatchModel> GetAllReceiveableItemsByDispatchAndBranchId(long dispatchId, int branchId);
       ICollection<ProductionSummary> GetProductionSummaries();
       ICollection<ProductionSummary> GetProductionSummaryByMonth(DateTime dateTime);
       string SaveReplaceDeliveryInfo(List<ScannedProduct> barcodeList, Delivery aDelivery, int replaceStatus);

       string SaveTransferDeliveredProduct(TransferModel aModel);
       ICollection<ViewTransferProductModel> GetAllTransferedListByBranchAndCompanyId(int branchId, int companyId);
       List<string> GetTransferReceiveableBarcodeList(long transferId);
       int ReceiveTransferedProduct(TransferModel aModel);
       ICollection<ViewProduct> GetTotalReceiveProductByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewProduct> GetTotalReceiveProductByCompanyId(int companyId);
        ICollection<ViewProduct> GetDeliveredProductByBranchAndCompanyId(int branchId, int companyId);
       List<ChartModel> GetTotalProductionCompanyIdAndYear(int companyId, int year);
       ICollection<ChartModel> GetTotalDispatchCompanyIdAndYear(int companyId, int year);
       int GetProductStatusInFactoryByBarCode(string barcode);
       int GetProductStatusInBranchInventoryByBarCode(string barcode);
       string SaveDeliveredOrder(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus);
       string SaveDeliveredOrderFromFactory(List<ScannedProduct> barcodeList, Delivery aDelivery, int invoiceStatus, int orderStatus);
       string SaveDeliveredGeneralRequisition(List<ScannedProduct> barcodeList, Delivery aDelivery);
       ICollection<object> GetStockProductByBranchCompanyIdAndSerachTerm(int branchId, int companyId, string searchTerm);
       ICollection<object> GetFactoryStockProductBySearchTerm(string prefix);
       int GetStockProductQuantityInFactoryById(int productId);
       ICollection<ViewProduct> GetRequsitionVeStockProductQtyByDistributionCenter(int distributionCenterId, int companyId);
       ICollection<ViewTripDetailsModel> GetTripItemsByTripId(long tripId);
       bool UpdateTripItemQuantity(long tripItemId, int quantity);
       ViewProductLifeCycleModel GetProductLifeCycle(string barcode);
       ViewProductHistory GetProductHistoryByBarcode(string barcode);

       IEnumerable<ViewTripModel> GetAllDeliverableTripList();
       ICollection<Inventory> GetReceivedProductByBranchId(int branchId);
       ICollection<ViewProduct> GetReceivedProductBarcodeById(long inventoryId);
       ICollection<ViewProduct> GetReceivedProductById(long inventoryId); 
   }
}
