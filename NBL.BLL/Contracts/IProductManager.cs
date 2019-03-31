﻿using System.Collections.Generic;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;

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
       
    }
}
