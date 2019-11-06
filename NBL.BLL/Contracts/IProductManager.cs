using System.Collections.Generic;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.BLL.Contracts
{
    public interface IProductManager
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllProducts(); 
        IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId);
        int GetProductMaxSerialNo();
        IEnumerable<TransferIssue> GetDeliverableTransferIssueList();
        IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId);
        int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model);
        string Save(Product aProduct);
        bool ApproveTransferIssue(TransferIssue transferIssue);
        int IssueProductToTransfer(TransferIssue aTransferIssue);
        ProductDetails GetProductDetailsByProductId(int productId);
        IEnumerable<TransferIssue> GetTransferIssueList();
        IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id);
        Product GetProductByProductAndClientTypeId(int productId, int clientTypeId);
        Product GetProductByProductId(int productId);
        bool SaveProductionNote(ProductionNote productionNote);
        IEnumerable<ViewProductionNoteModel> PendingProductionNote();
        TransferIssue GetTransferIssueById(int transerIssueId);
        TransferIssue GetDeliverableTransferIssueById(int transerIssueId);
        ICollection<ScannedProduct> GetScannedProductListFromTextFile(string filePath);
        string AddProductToTextFile(string productCode, string filePath);
        bool AddProductToInventory(List<Product> products);
        List<Product> GetIssuedProductListById(int id);
        bool IsScannedBefore(List<ScannedProduct> barcodeList, string scannedBarCode);
        List<ScannedProduct> ScannedProducts(string filePath);
        ScannedProduct GetProductByBarCode(string barCode);
        int SaveRequisitionInfo(ViewRequisitionModel aRequisitionModel);
        IEnumerable<ViewRequisitionModel> GetRequsitionsByStatus(int status);
        List<RequisitionModel> GetRequsitionDetailsById(long requisitionId);
        ICollection<ViewRequisitionModel> GetRequsitions();
        ICollection<ViewDispatchModel> GetDeliverableProductListByTripId(long tripId); 
        bool SaveMonthlyRequisitionInfo(MonthlyRequisitionModel model);
        ICollection<ViewMonthlyRequisitionModel> GetMonthlyRequsitions();
        ICollection<RequisitionItem> GetMonthlyRequsitionItemsById(long requisitionId);
        bool SaveProductDetails(ViewCreateProductDetailsModel model);
        List<Product> GetAllProductionAbleProductByDateCode(string productionDateCode);
        List<Product> GetTempReplaceProducts(string filePath);
        IEnumerable<ViewRequisitionModel> GetPendingRequsitions();
        ICollection<ViewDispatchModel> GetAllDispatchList();
        ICollection<ViewRequisitionModel> GetLatestRequisitions();
        int SaveTransferRequisitionInfo(TransferRequisition aRequisitionModel);
        ICollection<TransferRequisition> GetTransferRequsitionByStatus(int status);
        ICollection<TransferRequisitionDetails> GetTransferRequsitionDetailsById(long transferRequisitionId);
        bool RemoveProductRequisitionProductById(long id);
        bool UpdateRequisitionQuantity(long id, int quantity);
        bool ApproveRequisition(long id, ViewUser user);
        List<ViewTransferProductDetails> TransferReceiveableDetails(long transferId);
        int SaveGeneralRequisitionInfo(GeneralRequisitionModel requisition);
        int GetMaxGeneralRequisitionNoOfCurrentYear();
        ICollection<ViewGeneralRequisitionModel> GetAllGeneralRequisitions();
        ICollection<ViewGeneralRequistionDetailsModel> GetGeneralRequisitionDetailsById(long requisitiionId);
        bool UpdateGeneralRequisitionQuantity(string id, int quantity);
        bool RemoveProductByIdDuringApproval(string id);
        GeneralRequisitionModel GetGeneralRequisitionById(long requisitiionId);
        bool ApproveGeneralRequisition(GeneralRequisitionModel model, int nextApproverUser, int nextApprovalLevel,ApprovalDetails approval);
        bool ApproveGeneralRequisitionByScm(int userId, int distributionPoint, long requisitiionId);
        bool ApproveGeneralRequisitionBySalesAdmin(int userUserId, long requisitiionId);
        ICollection<object> GetAllProductBySearchTerm(string searchTerm);
        IEnumerable<ViewSoldProduct> GetTempSoldBarcodesFromXmlFile(string filePath);
        bool AddBarCodeToTempSoldProductXmlFile(ViewDisributedProduct product, string barcode, string filePath);

        ICollection<ViewGeneralRequisitionModel> GetGeneralRequisitionByUserId(int userId);
        ICollection<ViewGeneralRequisitionModel> GetAllDeliveredGRequsition();
        IEnumerable<ViewProduct> GetAllPendingProductPriceListByStatus(int status);
        bool UpdateProductActivationStatus(int productId, string status);

        ICollection<ProductDetails> GetAllProductDetails();
        bool UpdateProductPriceVatDiscount(ProductDetails newProductDetails,ProductDetails oldProductDetails, int userId);
        bool CancelRequisition(long requisitionId, ViewUser user);
    }
}
