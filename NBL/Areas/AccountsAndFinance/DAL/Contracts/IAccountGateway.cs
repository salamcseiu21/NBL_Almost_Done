﻿using System;
using System.Collections.Generic;
using NBL.Areas.Accounts.Models;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.Models;
using NBL.Models.EntityModels.ChartOfAccounts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.Searchs;
using NBL.Models.SummaryModels;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.FinanceModels;
using SubSubSubAccount = NBL.Models.EntityModels.ChartOfAccounts.SubSubSubAccount;

namespace NBL.Areas.AccountsAndFinance.DAL.Contracts
{
   public interface IAccountGateway
   {

       int SaveReceivable(Receivable receivable);


       ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId);
       IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId);
       int GetMaxVoucherNoOfCurrentYearByVoucherType(int voucherType);
       IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId);

       int GetMaxReceivableSerialNoOfCurrentYear();
       int ActiveReceivableCheque(ChequeDetails chequeDetails, Receivable aReceivable, Client aClient);
       int ActiveCheque(int chequeDetailsId);
        
        //--------Vouchers-------
       int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals);

       IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId, int companyId);


       IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId);


       int SaveVoucher(Voucher voucher);
       IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType);

       IEnumerable<Voucher> GetVoucherList();

       IEnumerable<Voucher> GetPendingVoucherList();

       IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId);


       IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId);

       Voucher GetVoucheByVoucherId(int voucherId);


       int CancelVoucher(int voucherId, string reason, int userId);

       int ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails, int userId);

       int UpdateVoucher(Voucher voucher, List<VoucherDetails> voucherDetails);
       int GetMaxJournalVoucherNoOfCurrentYear();


       List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId);

       JournalVoucher GetJournalVoucherById(int journalId);

       int CancelJournalVoucher(int voucherId, string reason, int userId);


       int UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers);



       List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId);


       int ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId);

       int ApproveVat(Vat vat);


       int ApproveDiscount(Discount discount);

       AccountSummary GetAccountSummaryOfCurrentMonth(); 
       AccountSummary GetAccountSummaryofCurrentMonthByCompanyId(int companyId);
       AccountSummary GetAccountSummaryofCurrentMonthByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewLedgerModel> GetClientLedgerBySubSubSubAccountCode(string clientSubSubSubAccountCode);
       ICollection<CollectionModel> GetTotalCollectionByBranch(int branchId);
       CollectionModel GetCollectionAmountById(long collectionId);
       ICollection<ViewReceivableDetails> GetActivetedReceivableListByBranch(int branchId);
       ViewReceivableDetails GetActivatedReceivableDetailsById(long chequeDetailsId);
       ICollection<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyIdUserId(int branchId, int companyId, int userId);
       ICollection<ChequeDetails> GetAllReceivableCheque(int branchId, int companyId, int userId, DateTime collectionDate);
       IEnumerable<ChequeDetails> GetAllReceivableCheque(SearchCriteria searchCriteria);
       ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, DateTime collectionDate);
       ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, int status);
       ICollection<ViewReceivableDetails> GetActivetedReceivableListByCompany(int companyId);
       int CancelReceivable(int chequeDetailsId, string reason, int userId);
       ICollection<AccountType> GetAllChartOfAccountType();
       ICollection<AccountHead> GetAllChartOfAccountList();
       ICollection<SubAccount> GetAllSubAccountList();
       ICollection<SubSubAccount> GetAllSubSubAccountList();
       ICollection<SubSubSubAccount> GetAllSubSubSubAccountList();
       ICollection<ChequeDetails> GetAllReceivableChequeByCompanyIdAndStatus(int companyId, int status);
       int CancelVat(Vat vat);
       int ApproveProductPrice(ViewUser anUser, int productDetailsId,int productId);
       int CancelUnitPriceAmount(ViewUser anUser, int productDetailsId);
       int UpdateReceivableCheque(ChequeDetails oldChequeByDetails, ChequeDetails newChequeDetails);
       ICollection<ChequeDetails> GetAllReceivableChequeByMonthYearAndStatus(int month, int year, int status);
       ICollection<ChequeDetails> GetAllReceivableChequeByYearAndStatus(int year, int status);
       IEnumerable<ChequeDetails> GetAllReceivableChequeBySearchCriteriaAndStatus(SearchCriteria searchCriteria, int status);
       long GetMaxOpeningBalanceRefNoOfCurrentYear();
       int SetClientOpeningBalance(OpeningBalanceModel model);
       IEnumerable<ViewLedgerModel> GetClientLedgerBySearchCriteria(SearchCriteria searchCriteria);
       int CancelDiscount(int discountId);
       int GetMaxSubSubSubAccountNoBySubSubAccountCode(string subSubAccountCode); 
       int AddSubSubSubAccount(SubSubSubAccount account);
       int GetMaxSubSubAccountNoBySubAccountCode(string subAccountCode);
       int AddSubSubAccount(SubSubAccount account);
       int GetMaxVoucherNoOfCurrentMonthByVoucherType(int voucherType);
       int GetMaxJournalVoucherNoOfCurrentMonth();
       IEnumerable<Voucher> GetVoucherListByBranchIdAndStatus(int branchId,int status);
   }
}
