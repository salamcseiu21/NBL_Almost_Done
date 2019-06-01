using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.BLL
{
    public class ProductManager:IProductManager
    {
        private readonly  IProductGateway _iProductGateway;
        private readonly ICommonGateway _iCommonGateway;
       
        public ProductManager(IProductGateway iProductGateway,ICommonGateway iCommonGateway)
        {
            _iProductGateway = iProductGateway;
            _iCommonGateway = iCommonGateway;
        }

        public IEnumerable<Product> GetAll()
        {
           return _iProductGateway.GetAll().ToList();

        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _iProductGateway.GetAllProducts();
        }

        public IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId)
        {
          return  _iProductGateway.GetAllProductByBranchAndCompanyId(branchId,companyId);
        }
        public int GetProductMaxSerialNo()
        {
            return _iProductGateway.GetProductMaxSerialNo();
        }

        public IEnumerable<TransferIssue> GetDeliverableTransferIssueList()
        {
            return _iProductGateway.GetDeliverableTransferIssueList();
        }


        public IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId)
        {
            
            return _iProductGateway.GetAllProductsByProductCategoryId(productCategoryId); 
        }

        public int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model)
        {
            model.TransactionRef = GenerateInvoiceNo(model);
            int rowAffected = _iProductGateway.TransferProduct(transactionModels, model);
            return rowAffected;
        }

        private string GenerateInvoiceNo(TransactionModel model)
        {
            string temp = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0,5);
            string invoice = "TR" + model.FromBranchId+model.ToBranchId + DateTime.Now.Date.ToString("yyyy MMM dd").Replace(" ", "").ToUpper() + temp;
            return invoice;
        }
        public string Save(Product aProduct)
        {
            int lastSlNo = GetProductMaxSerialNo();
            string subSubSubAccountCode = Generator.GenerateAccountCode("2201",lastSlNo);
            aProduct.SubSubSubAccountCode = subSubSubAccountCode;
            int rowAffected = _iProductGateway.Save(aProduct);
            if (rowAffected > 0)
            {
                return "Saved Successfully!";
            }

            return "Failed To Save";
        }

        public bool ApproveTransferIssue(TransferIssue transferIssue)
        {
            int rowAffected = _iProductGateway.ApproveTransferIssue(transferIssue);
            if(rowAffected>0)
            return true;
            return false;

        }

        public int IssueProductToTransfer(TransferIssue aTransferIssue)
        {
            int maxTrNo = _iProductGateway.GetMaxTransferRequisitionNoOfCurrentYear();
            aTransferIssue.TransferIssueRef = GenerateTransferIssueRef(maxTrNo);
            int rowAffected = _iProductGateway.IssueProductToTransfer(aTransferIssue);
            return rowAffected;
        }
        /// <summary>
        /// id=3 stands for transfer issue from factory ...
        /// </summary>
        /// <param name="maxTrNo"></param>
        /// <returns></returns>
        private string GenerateTransferIssueRef(int maxTrNo)
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Transfer)).Code;
            string temp = (maxTrNo + 1).ToString();
            string reference=DateTime.Now.Year.ToString().Substring(2,2)+ refCode+temp;
            return reference;
        }
        private string GenerateRequisitionRef(int maxrNo)
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Requisition)).Code;
            string temp = (maxrNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }
        private string GenerateMonthlyRequisitionRef(int maxrNo)
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.MonthlyRequisition)).Code;
            string temp = (maxrNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }

        public ProductDetails GetProductDetailsByProductId(int productId)
        {
            return _iProductGateway.GetProductDetailsByProductId(productId);
        }

        public IEnumerable<TransferIssue> GetTransferIssueList() 
        {
            return _iProductGateway.GetTransferIssueList();
        }

        public TransferIssue GetTransferIssueById(int transferIssueId) 
        {
            return _iProductGateway.GetTransferIssueById(transferIssueId);
        }

        public TransferIssue GetDeliverableTransferIssueById(int transerIssueId)
        {
           return _iProductGateway.GetDeliverableTransferIssueById(transerIssueId);
        }

        public ICollection<ScannedProduct> GetScannedProductListFromTextFile(string filePath)
        {
           return _iProductGateway.GetScannedProductListFromTextFile(filePath);
        }

        public string AddProductToTextFile(string productCode, string filePath)
        {
            Product product = null;
            bool isScannedBefore = false;
            bool isValid = Validator.ValidateProductBarCode(productCode);
            if(isValid)
            {
                var productId = Convert.ToInt32(productCode.Substring(2, 3));
                product =GetProductByProductId(productId);
                var barcodeList = ScannedProducts(filePath);
                isScannedBefore=IsScannedBefore(barcodeList, productCode);
            }
            if(!isValid)
            {
                return "<p style='color:red'> Invalid Barcode </p>";
            }
            if(isScannedBefore)
            {
                return "<p style='color:red'> Already Scanned </p>";
            }
            if(product == null)
            {
                return "<p style='color:red'> Invalid Product </p>";
            }

             var result = _iProductGateway.AddProductToTextFile(productCode, filePath);
             return result ? "<p class='text-green'>Added Successfully!</p>" : "<p style='color:red'> Failed to Add </p>";
        }

        public bool AddProductToInventory(List<Product> products)
        {
            return _iProductGateway.AddProductToInventory(products);
        }

        public List<Product> GetIssuedProductListById(int id)
        {
            return _iProductGateway.GetIssuedProductListById(id);
        }

        public bool IsScannedBefore(List<ScannedProduct> barcodeList, string scannedBarCode)
        {
            return barcodeList.ToList().Select(n => n.ProductCode).Contains(scannedBarCode);
        }

        public IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id)
        {
            return _iProductGateway.GetTransferIssueDetailsById(id); 
        }

        public Product GetProductByProductAndClientTypeId(int productId, int clientTypeId)
        {
            return _iProductGateway.GetProductByProductAndClientTypeId(productId,clientTypeId);
        }

        public Product GetProductByProductId(int productId)
        {
            return _iProductGateway.GetProductByProductId(productId);
        }

        public bool SaveProductionNote(ProductionNote productionNote)
        {

            productionNote.ProductionNoteNo =DateTime.Now.Year.ToString().Substring(2,2)+GetMaxProductNoteNo(DateTime.Now.Year);
            productionNote.ProductionNoteRef= GenerateProductNoteRef(DateTime.Now.Year);
            int rowAffected = _iProductGateway.SaveProductionNote(productionNote);
            return rowAffected>0;
        }

        private string GenerateProductNoteRef(int year)
        {
            int maxNoteNo = _iProductGateway.GetMaxProductionNoteNoByYear(year);
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.ProductionNote)).Code;
            return $"{year.ToString().Substring(2, 2)}{refCode}{maxNoteNo+1}";
        }

        private int GetMaxProductNoteNo(int year) 
        {
            int maxNoteNo = _iProductGateway.GetMaxProductionNoteNoByYear(year)+1;
            return maxNoteNo;
        }

        public IEnumerable<ViewProductionNoteModel> PendingProductionNote()
        {
            return _iProductGateway.PendingProductionNote();
        }

        public List<ScannedProduct> ScannedProducts(string filePath)
        {
            List<ScannedProduct> barcodeList = new List<ScannedProduct>();
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                barcodeList =GetScannedProductListFromTextFile(filePath).ToList();
            }

            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            return barcodeList;
        }

        public ScannedProduct GetProductByBarCode(string barCode)
        {
           return _iProductGateway.GetProductByBarCode(barCode);
        }

        public int SaveRequisitionInfo(ViewRequisitionModel aRequisitionModel)
        {
            int maxTrNo = _iProductGateway.GetMaxRequisitionNoOfCurrentYear();
            aRequisitionModel.RequisitionRef = GenerateRequisitionRef(maxTrNo);
            int rowAffected = _iProductGateway.SaveRequisitionInfo(aRequisitionModel);
            return rowAffected;
           
        }

        public IEnumerable<ViewRequisitionModel> GetRequsitionsByStatus(int status)
        {
            return _iProductGateway.GetRequsitionsByStatus(status);
        }

        public List<RequisitionModel> GetRequsitionDetailsById(long requisitionId)
        {
            return _iProductGateway.GetRequsitionDetailsById(requisitionId);
        }

        public ICollection<ViewRequisitionModel> GetRequsitions()
        {
            return _iProductGateway.GetRequsitions();
        }
       

        public ICollection<ViewDispatchModel> GetDeliverableProductListByTripId(long tripId)
        {
            return _iProductGateway.GetDeliverableProductListByTripId(tripId);
        }

        public bool SaveMonthlyRequisitionInfo(MonthlyRequisitionModel model)
        {
            int maxTrNo = _iProductGateway.GetMaxMonnthlyRequisitionNoOfCurrentYear();
            model.RequisitionRef = GenerateMonthlyRequisitionRef(maxTrNo);
            int rowAffected = _iProductGateway.SaveMonthlyRequisitionInfo(model); 
            return rowAffected > 0;
        }

        public ICollection<ViewMonthlyRequisitionModel> GetMonthlyRequsitions()
        {
            return _iProductGateway.GetMonthlyRequsitions();
        }

        public ICollection<RequisitionItem> GetMonthlyRequsitionItemsById(long requisitionId)
        {
            return _iProductGateway.GetMonthlyRequsitionItemsById(requisitionId);
        }

        public bool SaveProductDetails(ViewCreateProductDetailsModel model)
        {
            int rowAffected = _iProductGateway.SaveProductDetails(model);
            return rowAffected > 0;
        }

        public List<Product> GetAllProductionAbleProductByDateCode(string productionDateCode)
        {
            return _iProductGateway.GetAllProductionAbleProductByDateCode(productionDateCode);
        }

        public List<Product> GetTempReplaceProducts(string filePath)
        {
            return _iProductGateway.GetTempReplaceProducts(filePath);
        }

        public IEnumerable<ViewRequisitionModel> GetPendingRequsitions()
        {
            return _iProductGateway.GetPendingRequsitions();
        }

        public ICollection<ViewDispatchModel> GetAllDispatchList()
        {
            return _iProductGateway.GetAllDispatchList();
        }

        public ICollection<ViewRequisitionModel> GetLatestRequisitions()
        {
            return _iProductGateway.GetLatestRequisitions();
        }

        public int SaveTransferRequisitionInfo(TransferRequisition aRequisitionModel)
        {
            int maxTrNo = _iProductGateway.GetMaxTransferRequisitionNoOfCurrentYear();
            aRequisitionModel.TransferRequisitionRef = GenerateTransferRequisitionRef(maxTrNo);
            return _iProductGateway.SaveTransferRequisitionInfo(aRequisitionModel);
        }

        public ICollection<TransferRequisition> GetTransferRequsitionByStatus(int status)
        {
            return _iProductGateway.GetTransferRequsitionByStatus(status);
        }

        public ICollection<TransferRequisitionDetails> GetTransferRequsitionDetailsById(long transferRequisitionId)
        {
            return _iProductGateway.GetTransferRequsitionDetailsById(transferRequisitionId);
        }

        public bool RemoveProductRequisitionProductById(long id)
        {
            int rowAffected = _iProductGateway.RemoveProductRequisitionProductById(id);
            return rowAffected > 0;
        }

        public bool UpdateRequisitionQuantity(long id, int quantity)
        {
            int rowAffected = _iProductGateway.UpdateRequisitionQuantity(id,quantity);
            return rowAffected > 0;
        }

        public bool ApproveRequisition(long id, ViewUser user)
        {
            int rowAffected = _iProductGateway.ApproveRequisition(id, user);
            return rowAffected > 0;
        }

        public List<ViewTransferProductDetails> TransferReceiveableDetails(long transferId)
        {
            return _iProductGateway.TransferReceiveableDetails(transferId);
        }

        public int SaveGeneralRequisitionInfo(GeneralRequisitionModel requisition)
        {
            var model= _iCommonGateway.GetFirstApprovalPathByUserId(requisition.RequisitionByUserId);
            int max = _iProductGateway.GetMaxGeneralRequisitionNoOfCurrentYear();
            requisition.RequisitionRef = GenerateGeneralRequisitionRef(max);
            requisition.CurrentApprovalLevel = model.ApprovalLevel;
            requisition.CurrentApproverUserId = model.ApproverUserId;
            return _iProductGateway.SaveGeneralRequisitionInfo(requisition);
        }

        private string GenerateGeneralRequisitionRef(int max)
        {
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.GeneralRequisition)).Code;
            string temp = (max + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }

        public int GetMaxGeneralRequisitionNoOfCurrentYear()
        {
            return _iProductGateway.GetMaxGeneralRequisitionNoOfCurrentYear();
        }

        public ICollection<ViewGeneralRequisitionModel> GetAllGeneralRequisitions()
        {
            return _iProductGateway.GetAllGeneralRequisitions();
        }

        public ICollection<ViewGeneralRequistionDetailsModel> GetGeneralRequisitionDetailsById(long requisitiionId)
        {
            return _iProductGateway.GetGeneralRequisitionDetailsById(requisitiionId);
        }

        public bool UpdateGeneralRequisitionQuantity(string id, int quantity)
        {
            int rowAffected = _iProductGateway.UpdateGeneralRequisitionQuantity(id,quantity);
            return rowAffected > 0;
        }

        public bool RemoveProductByIdDuringApproval(string id)
        {
            int rowAffected = _iProductGateway.RemoveProductByIdDuringApproval(id);
            return rowAffected > 0;
        }

        public GeneralRequisitionModel GetGeneralRequisitionById(long requisitiionId)
        {
            return _iProductGateway.GetGeneralRequisitionById(requisitiionId);
        }

        public bool ApproveGeneralRequisition(GeneralRequisitionModel model, int nextApproverUser, int nextApprovalLevel,ApprovalDetails approval)
        {
            int rowAffected = _iProductGateway.ApproveGeneralRequisition(model,nextApproverUser,nextApprovalLevel,approval);
            return rowAffected > 0;
        }

        public bool ApproveGeneralRequisitionByScm(int userId, int distributionPoint,long requisitiionId)
        {
            int rowAffected = _iProductGateway.ApproveGeneralRequisitionByScm(userId,distributionPoint,requisitiionId);
            return rowAffected > 0;
        }

        public ICollection<object> GetAllProductBySearchTerm(string searchTerm)
        {
            return _iProductGateway.GetAllProductBySearchTerm(searchTerm);
        }

        public IEnumerable<ViewSoldProduct> GetTempSoldBarcodesFromXmlFile(string filePath)
        {
           return _iProductGateway.GetTempSoldBarcodesFromXmlFile(filePath);
        }

        public bool AddBarCodeToTempSoldProductXmlFile(ViewDisributedProduct product, string barcode, string filePath)
        {
            int rowAffected =_iProductGateway.AddBarCodeToTempSoldProductXmlFile(product,barcode, filePath);
            return rowAffected > 0;
        }

        private string GenerateTransferRequisitionRef(int maxTrNo)
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Transfer)).Code;
            string temp = (maxTrNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }
    }
}