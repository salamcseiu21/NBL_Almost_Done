using System;
using System.Collections.Generic;
using NBL.Areas.Accounts.Models;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.Searchs;
using NBL.Models.SummaryModels;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Areas.AccountsAndFinance.BLL.Contracts
{
    public interface IAccountsManager
   {
       int SaveReceivable(Receivable receivable);
       IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId);
       ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId);
       int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals);
       IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId);
       int SaveVoucher(Voucher voucher);
       IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType);
       Voucher GetVoucherByVoucherId(int voucherId);
       IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId);
       IEnumerable<Voucher> GetVoucherList();
       IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId);
       IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<Voucher> GetPendingVoucherList();
       bool CancelVoucher(int voucherId, string reason, int userId);
       bool ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails, int userId);
       bool UpdateVoucher(Voucher voucher, List<VoucherDetails> voucherDetails);
       List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId);
       JournalVoucher GetJournalVoucherById(int journalId);
       bool CancelJournalVoucher(int voucherId, string reason, int userId);
       bool UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers);
       List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId);
       bool ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId);
       bool ApproveVat(Vat vat);
       bool ApproveDiscount(Discount discount);


       AccountSummary GetAccountSummaryOfCurrentMonth();
       AccountSummary GetAccountSummaryofCurrentMonthByCompanyId(int companyId);
       AccountSummary GetAccountSummaryofCurrentMonthByBranchAndCompanyId(int branchId, int companyId);

       bool ActiveReceivableCheque(ChequeDetails chequeDetails, Receivable aReceivable, Client aClient);
       ICollection<ViewLedgerModel> GetClientLedgerBySubSubSubAccountCode(string clientSubSubSubAccountCode);
       ICollection<Purpose> GetCreditPurposesFromXmlFile(string filePath);
       ICollection<CollectionModel> GetTotalCollectionByBranch(int branchId);
       CollectionModel GetCollectionAmountById(long id);
       ICollection<ViewReceivableDetails> GetActivetedReceivableListByBranch(int branchId);
       ViewReceivableDetails GetActivatedReceivableDetailsById(long chequeDetailsId);
       ICollection<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyIdUserId(int branchId, int companyId, int userId);
       ICollection<ChequeDetails> GetAllReceivableCheque(int branchId, int companyId, int userId, DateTime collectionDate);
       ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, DateTime collectionDate);
       IEnumerable<ChequeDetails> GetAllReceivableCheque(SearchCriteria searchCriteria);

       ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, int status);
       ICollection<ViewReceivableDetails> GetActivetedReceivableListByCompany(int companyId);
       bool CancelReceivable(int chequeDetailsId, string reason, int userId); 
   }
}
