using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace NBL.BLL.Contracts
{
   public interface ICommonManager
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
        bool SaveEncriptedConString(string chipartext);
        ICollection<RejectionReason> GetAllRejectionReason();
        bool UpdateCurrentUserRole(ViewUser user, int roleId);
        bool GetFirstApprovalPathByUserId(int requisitionByUserId);
        bool GetFirstApprovalPathByApproverUserId(int approverUserId);
        ICollection<RequisitionFor> GetAllRequisitionForList();
        ICollection<ApprovalAction> GetAllApprovalActionList();
        ICollection<ApprovalPathModel> GetAllApprovalPath();
        ICollection<ApprovalDetails> GetAllApprovalDetailsByRequistionId(long requisitionId);
        ICollection<PhysicalConditionModel> GetAllPhysicalConditions();
        ICollection<ServicingModel> GetAllServicingStatus();
        ICollection<ChargingStatusModel> GetAllCharginStatus();
    }
}
