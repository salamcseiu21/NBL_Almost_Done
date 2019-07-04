using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
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
        public ICollection<ViewProduct> GetTotalReceiveProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetTotalReceiveProductByBranchAndCompanyId(branchId, companyId);
        }

        public ICollection<ViewProduct> GetTotalReceiveProductByCompanyId(int companyId)
        {
            return _iInventoryGateway.GetTotalReceiveProductByCompanyId(companyId);
        }
        public ICollection<ViewProduct> GetDeliveredProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetDeliveredProductByBranchAndCompanyId(branchId, companyId);
        }

        public List<ChartModel> GetTotalProductionCompanyIdAndYear(int companyId, int year)
        {
            return _iInventoryGateway.GetTotalProductionCompanyIdAndYear(companyId,year);
        }

        public ICollection<ChartModel> GetTotalDispatchCompanyIdAndYear(int companyId, int year)
        {
            return _iInventoryGateway.GetTotalDispatchCompanyIdAndYear(companyId,year);
        }

        public int GetProductStatusInFactoryByBarCode(string barcode)
        {
            return _iInventoryGateway.GetProductStatusInFactoryByBarCode(barcode);
        }

        public int GetProductStatusInBranchInventoryByBarCode(string barcode)
        {
            return _iInventoryGateway.GetProductStatusInBranchInventoryByBarCode(barcode);
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

        public ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByDispatchId(long dispatchId, int branchId)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByDispatchId(dispatchId,branchId);
        }

        public TransactionModel GetTransactionModelById(long id)
        {
            return _iInventoryGateway.GetTransactionModelById(id);
        }

        public int SaveScannedProduct(ProductionModel model)
        {
            var maxProductionNo = _iInventoryGateway.GetmaxProductionRefByYear(DateTime.Now.Year);
            var productionRef = GenerateProducitonRef(maxProductionNo);
            model.TransactionRef = productionRef;
            model.TransactionType = "RE";
            
            return _iInventoryGateway.SaveScannedProduct(model); 
        }

        private string GenerateProducitonRef(long maxProductionNo)
        {
            string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.ProductionNote));
            var sN = 1 + maxProductionNo;
            string ordSlipNo = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return ordSlipNo;
        }
        private string GetReferenceAccountCodeById(int subReferenceAccountId)
        {
            var code = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(subReferenceAccountId)).Code;
            return code;
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

        public ViewDispatchModel GetDispatchByDispatchId(long dispatchId)
        {
            return _iInventoryGateway.GetDispatchByDispatchId(dispatchId); 
        }

        public ICollection<ViewDispatchModel> GetAllReceiveableItemsByDispatchAndBranchId(long dispatchId, int branchId)
        {
            return _iInventoryGateway.GetAllReceiveableItemsByDispatchAndBranchId(dispatchId, branchId);
        }

        public ICollection<ProductionSummary> GetProductionSummaries()
        {
            return _iInventoryGateway.GetProductionSummaries();
        }

        public ICollection<ProductionSummary> GetProductionSummaryByMonth(DateTime dateTime)
        {
            return _iInventoryGateway.GetProductionSummaryByMonth(dateTime);
        }
        //-----------------------Replace---------------------
        public string SaveReplaceDeliveryInfo(List<ScannedProduct> scannedProducts, Delivery aDelivery, int replaceStatus)
        {
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Distribution)).Code;
            aDelivery.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.SaveReplaceDeliveryInfo(scannedProducts, aDelivery, replaceStatus);

            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public string SaveTransferDeliveredProduct(TransferModel aModel)
        {
            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aModel.Delivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.SaveTransferDeliveredProduct(aModel);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public ICollection<ViewTransferProductModel> GetAllTransferedListByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetAllTransferedListByBranchAndCompanyId(branchId,companyId);
        }

        public List<string> GetTransferReceiveableBarcodeList(long transferId)
        {
            return _iInventoryGateway.GetTransferReceiveableBarcodeList(transferId);
        }

        public int ReceiveTransferedProduct(TransferModel aModel)
        {
            int rowAffected = _iInventoryGateway.ReceiveTransferedProduct(aModel);
            return rowAffected;
        }

        public string SaveDeliveredOrderFromFactory(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus)
        {
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Distribution)).Code;
            aDelivery.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.SaveDeliveredOrderFromFactory(scannedProducts, aDelivery, invoiceStatus, orderStatus);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public string SaveDeliveredGeneralRequisition(List<ScannedProduct> scannedProducts, Delivery aDelivery)
        {
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Distribution)).Code;
            aDelivery.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.SaveDeliveredGeneralRequisition(scannedProducts, aDelivery);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public ICollection<object> GetStockProductByBranchCompanyIdAndSerachTerm(int branchId, int companyId, string searchTerm)
        {
            return _iInventoryGateway.GetStockProductByBranchCompanyIdAndSerachTerm(branchId,companyId,searchTerm);
        }

        public ICollection<object> GetFactoryStockProductBySearchTerm(string searchTerm)
        {
            return _iInventoryGateway.GetFactoryStockProductBySearchTerm(searchTerm);
        }

        public int GetStockProductQuantityInFactoryById(int productId)
        {
            return _iInventoryGateway.GetStockProductQuantityInFactoryById(productId);
        }

        public ICollection<ViewProduct> GetRequsitionVeStockProductQtyByDistributionCenter(int distributionCenterId, int companyId)
        {
            return _iInventoryGateway.GetRequsitionVeStockProductQtyByDistributionCenter(distributionCenterId,companyId);
        }

        public ICollection<ViewTripDetailsModel> GetTripItemsByTripId(long tripId)
        {
            return _iInventoryGateway.GetTripItemsByTripId(tripId);
        }

        public bool UpdateTripItemQuantity(long tripItemId,int quantity)
        {
            return _iInventoryGateway.UpdateTripItemQuantity(tripItemId,quantity)>0;
        }

        public ViewProductLifeCycleModel GetProductLifeCycle(string barcode)
        {
            return _iInventoryGateway.GetProductLifeCycle(barcode);
        }

        public ViewProductHistory GetProductHistoryByBarcode(string barcode)
        {
            return _iInventoryGateway.GetProductHistoryByBarcode(barcode);
        }
    }
}