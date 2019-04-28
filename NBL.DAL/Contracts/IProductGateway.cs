using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.DAL.Contracts
{
    public interface IProductGateway
    {
        IEnumerable<Product> GetAll();
        IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId);
        Product GetProductByProductAndClientTypeId(int productId, int clientTypeId);
        int GetMaxTransferIssueNoOfCurrentYear();
        IEnumerable<TransferIssue> GetDeliverableTransferIssueList();
        int ApproveTransferIssue(TransferIssue transferIssue);
        int IssueProductToTransfer(TransferIssue aTransferIssue);
        IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id);
        IEnumerable<TransferIssue> GetTransferIssueList();
        int SaveTransferIssueDetails(List<Product> products, int transferIssueId);
        int GetProductMaxSerialNo();
        IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId);
        int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model);
        int SaveTransferDetails(List<TransactionModel> transactionModels, int inventoryMasterId);
        int Save(Product aProduct);
        ProductDetails GetProductDetailsByProductId(int productId);
        Product GetProductByProductId(int productId);
        int GetMaxProductionNoteNoByYear(int year);
        int SaveProductionNote(ProductionNote productionNote);
        IEnumerable<ViewProductionNoteModel> PendingProductionNote();
        TransferIssue GetTransferIssueById(int transerIssueId);
        TransferIssue GetDeliverableTransferIssueById(int transerIssueId);
        ICollection<ScannedProduct> GetScannedProductListFromTextFile(string filePath);
        bool AddProductToTextFile(string productCode, string filePath);
        bool AddProductToInventory(List<Product> products);
        List<Product> GetIssuedProductListById(int id);
        ScannedProduct GetProductByBarCode(string barCode);
        int SaveRequisitionInfo(ViewRequisitionModel aRequisitionModel);
        int GetMaxRequisitionNoOfCurrentYear();
        IEnumerable<ViewRequisitionModel> GetRequsitionsByStatus(int status);
        List<RequisitionModel> GetRequsitionDetailsById(long requisitionId);
        ICollection<ViewRequisitionModel> GetRequsitions();
        ICollection<ViewDispatchModel> GetDeliverableProductListByTripId(long tripId);
        int SaveMonthlyRequisitionInfo(MonthlyRequisitionModel model);
        ICollection<ViewMonthlyRequisitionModel> GetMonthlyRequsitions();
        int GetMaxMonnthlyRequisitionNoOfCurrentYear();
        ICollection<RequisitionItem> GetMonthlyRequsitionItemsById(long requisitionId);
        IEnumerable<Product> GetAllProducts();
        int SaveProductDetails(ViewCreateProductDetailsModel model);
        List<Product> GetAllProductionAbleProductByDateCode(string productionDateCode);

        List<Product> GetTempReplaceProducts(string filePath);
    }
}
