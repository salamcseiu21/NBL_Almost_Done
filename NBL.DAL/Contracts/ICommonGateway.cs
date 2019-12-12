
using System;
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Banks;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.MobileBankings;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.Services;
using NBL.Models.EntityModels.Suppliers;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface ICommonGateway
   {
       IEnumerable<ClientType> GetAllClientType();
       IEnumerable<ProductCategory> GetAllProductCategory();
       IEnumerable<ProductType> GetAllProductType();
       IEnumerable<Branch> GetAssignedBranchesToUserByUserId(int userId);
       IEnumerable<UserRole> GetAllUserRoles();
       IEnumerable<PaymentType> GetAllPaymentTypes();
       IEnumerable<TransactionType> GetAllTransactionTypes();
       IEnumerable<Supplier> GetAllSupplier();
       IEnumerable<Bank> GetAllBank();
       IEnumerable<BankBranch> GetAllBankBranch();
       IEnumerable<MobileBanking> GetAllMobileBankingAccount();
       IEnumerable<SubSubSubAccount> GetAllSubSubSubAccounts();
       SubSubSubAccount GetSubSubSubAccountByCode(string accountCode);
       Vat GetCurrentVatByProductId(int productId);
       Discount GetCurrentDiscountByClientTypeId(int clientTypeId);
       IEnumerable<ReferenceAccount> GetAllReferenceAccounts();
       IEnumerable<ViewReferenceAccountModel> GetAllSubReferenceAccounts();
       IEnumerable<Status> GetAllStatus();
       DateTime GenerateDateFromBarCode(string scannedBarCode);
       ICollection<string> GetAllTestBarcode();
       ICollection<ProductionDateCode> GetAllProductionDateCode();
       ICollection<ProductionLine> GetAllProductionLines();
       ICollection<ProductionDateCode> GetProductionDateCodeByMonthYear(string monthYear);
       int SaveEncriptedConString(string chipartext,string ip, string dbName, string userNmae, string password);
       ICollection<RejectionReason> GetAllRejectionReason();
       int UpdateCurrentUserRole(ViewUser user,int roleId);
       ICollection<RequisitionFor> GetAllRequisitionForList();
       ApprovalPathModel GetFirstApprovalPathByUserId(int requisitionByUserId);
       ApprovalPathModel GetFirstApprovalPathByApproverUserId(int approverUserId);
       ICollection<ApprovalAction> GetAllApprovalActionList();
       ICollection<ApprovalPathModel> GetAllApprovalPath();
       ICollection<ApprovalDetails> GetAllApprovalDetailsByRequistionId(long requisitionId);
       ICollection<ServicingModel> GetAllServicingStatus();
       ICollection<PhysicalConditionModel> GetAllPhysicalConditions();
       ICollection<ChargingStatusModel> GetAllCharginStatus();
       ICollection<CellCondition> GetAllCellConditions();
       ICollection<object> GetCellConditionBySearchTerm(string searchTerm);
       ICollection<ForwardToModel> GetAllForwardToModels();
       ICollection<ForwardToModel> GetAllForwardToModelsByUserAndActionId(int userId, long actionId);
       ViewActionListModel GetActionListModelByAreaControllerActionName(string area, string controller, string action);
       ICollection<ViewActionListModel> GetAllActionList();
       int UpdateReplaceTransactionRef(string receiveref, string replaceref);
       SubSubSubAccount GetSubSubSubAccountById(int subSubSubAccountId);
       ICollection<TestCategory> GetAllTestCategories();
       ICollection<object> GetAllSubSubSubAccountNameBySearchTerm(string prefix);
       ICollection<object> GetAllSubSubSubAccountNameBySearchTermAndAccountPrefix(string searchTerm, string accountPrefix);
   }
}
