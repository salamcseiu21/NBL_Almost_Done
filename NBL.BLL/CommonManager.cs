
using System;
using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Banks;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.MobileBankings;
using NBL.Models.EntityModels.Suppliers;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
   public class CommonManager:ICommonManager
   {
       private readonly ICommonGateway _iCommonGateway;
        public CommonManager(ICommonGateway iCommonGateway)
        {
            _iCommonGateway = iCommonGateway;
        }
        public IEnumerable<ClientType> GetAllClientType()
        {
            return _iCommonGateway.GetAllClientType();
        }

        public IEnumerable<ProductCategory> GetAllProductCategory()
        {
            return _iCommonGateway.GetAllProductCategory();
        }

        public IEnumerable<ProductType> GetAllProductType()
        {
            return _iCommonGateway.GetAllProductType();
        }

        public IEnumerable<Branch> GetAssignedBranchesToUserByUserId(int userId)
        {
            return _iCommonGateway.GetAssignedBranchesToUserByUserId(userId);
        }

        public IEnumerable<UserRole> GetAllUserRoles()
        {
            return _iCommonGateway.GetAllUserRoles();
        }

        public IEnumerable<PaymentType> GetAllPaymentTypes()
        {
            return _iCommonGateway.GetAllPaymentTypes();
        }

        public IEnumerable<TransactionType> GetAllTransactionTypes()
        {
            return _iCommonGateway.GetAllTransactionTypes();
        }

        public IEnumerable<Supplier> GetAllSupplier()
        {
            return _iCommonGateway.GetAllSupplier();
        }

        public IEnumerable<Bank> GetAllBank()
        {
            return _iCommonGateway.GetAllBank();
        }

        public IEnumerable<BankBranch> GetAllBankBranch()
        {
            return _iCommonGateway.GetAllBankBranch();
        }

        public IEnumerable<MobileBanking> GetAllMobileBankingAccount()
        {
            return _iCommonGateway.GetAllMobileBankingAccount();
        }

        public IEnumerable<SubSubSubAccount> GetAllSubSubSubAccounts()
        {
            return _iCommonGateway.GetAllSubSubSubAccounts();
        }

        public SubSubSubAccount GetSubSubSubAccountByCode(string accountCode)
        {
            return _iCommonGateway.GetSubSubSubAccountByCode(accountCode);
        }

        public Vat GetCurrentVatByProductId(int productId)
        {
           return _iCommonGateway.GetCurrentVatByProductId(productId);
        }

        public Discount GetCurrentDiscountByClientTypeId(int clientTypeId)
        {
            return _iCommonGateway.GetCurrentDiscountByClientTypeId(clientTypeId);
        }

        public IEnumerable<ReferenceAccount> GetAllReferenceAccounts()
        {
            return _iCommonGateway.GetAllReferenceAccounts();
        }

        public IEnumerable<ViewReferenceAccountModel> GetAllSubReferenceAccounts()
        {
            return _iCommonGateway.GetAllSubReferenceAccounts();
        }

        public IEnumerable<Status> GetAllStatus()
        {
            return _iCommonGateway.GetAllStatus();
        }

       public DateTime GenerateDateFromBarCode(string scannedBarCode)
       {
           return _iCommonGateway.GenerateDateFromBarCode(scannedBarCode);
       }

       public ICollection<string> GetAllTestBarcode()
       {
           return _iCommonGateway.GetAllTestBarcode();
       }

       public ICollection<ProductionDateCode> GetAllProductionDateCode()
       {
           return _iCommonGateway.GetAllProductionDateCode();
       }

       public ICollection<ProductionLine> GetAllProductionLines()
       {
           return _iCommonGateway.GetAllProductionLines();
       }

       public ICollection<ProductionDateCode> GetProductionDateCodeByMonthYear(string monthYear)
       {
           return _iCommonGateway.GetProductionDateCodeByMonthYear(monthYear);
       }

       public bool SaveEncriptedConString(string chipartext)
       {
           int rowAffected = _iCommonGateway.SaveEncriptedConString(chipartext);
           return rowAffected > 0;
       }
   }
}
