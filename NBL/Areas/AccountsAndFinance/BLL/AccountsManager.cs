using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.AccountsAndFinance.DAL.Contracts;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.Areas.Sales.DAL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.Enums;
using NBL.Models.Searchs;
using NBL.Models.SummaryModels;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Areas.AccountsAndFinance.BLL
{
    public class AccountsManager:IAccountsManager
    {
       private readonly IAccountGateway _iAccountGateway;
       private readonly InvoiceGateway _invoiceGateway = new InvoiceGateway();
       private readonly ICommonManager _iCommonManager;

        public AccountsManager(ICommonManager iCommonManager,IAccountGateway iAccountGateway)
        {
            _iCommonManager = iCommonManager;
            _iAccountGateway = iAccountGateway;
        }

        public int SaveReceivable(Receivable receivable)
        {
            int maxSl = GetMaxReceivableSerialNoOfCurrentYear();
            receivable.ReceivableRef = GenerateReceivableRef(maxSl);
            receivable.ReceivableNo = GenerateReceivableNo(maxSl);
            return _iAccountGateway.SaveReceivable(receivable);
        }

        private int GenerateReceivableNo(int maxSl)
        {
            int receivableNo = maxSl + 1;
            return receivableNo;
        }

        public IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
        }
        public ICollection<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyIdUserId(int branchId, int companyId, int userId)
        {
            return _iAccountGateway.GetAllReceivableChequeByBranchAndCompanyIdUserId(branchId, companyId,userId);
        }

        public ICollection<ChequeDetails> GetAllReceivableCheque(int branchId, int companyId, int userId, DateTime collectionDate)
        {
            return _iAccountGateway.GetAllReceivableCheque(branchId, companyId,userId,collectionDate);
        }

        public ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, DateTime collectionDate)
        {
            return _iAccountGateway.GetAllReceivableCheque(companyId,collectionDate);
        }

        public IEnumerable<ChequeDetails> GetAllReceivableCheque(SearchCriteria searchCriteria)
        {
            return _iAccountGateway.GetAllReceivableCheque(searchCriteria);
        }

        public ICollection<ChequeDetails> GetAllReceivableCheque(int companyId, int status)
        {
            return _iAccountGateway.GetAllReceivableCheque(companyId,status);
        }

        public ICollection<ViewReceivableDetails> GetActivetedReceivableListByCompany(int companyId)
        {
            return _iAccountGateway.GetActivetedReceivableListByCompany(companyId);
        }

        public bool CancelReceivable(int chequeDetailsId, string reason, int userId)
        {
            int rowAffected= _iAccountGateway.CancelReceivable(chequeDetailsId, reason, userId);
            return rowAffected > 0;
        }

        public ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId)
        {
            return _iAccountGateway.GetReceivableChequeByDetailsId(chequeDetailsId);
        }

        private string GenerateReceivableRef(int maxSl)
        {
            var refCode = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n=>n.Id==Convert.ToInt32(ReferenceType.AccountReceiveable)).Code;
            string reference = $"{DateTime.Now.Year.ToString().Substring(2, 2)}{refCode}{maxSl + 1}";
            return reference;
        }

        private int GetMaxReceivableSerialNoOfCurrentYear()
        {
          return _iAccountGateway.GetMaxReceivableSerialNoOfCurrentYear();
        }

        public bool ActiveReceivableCheque(ChequeDetails chequeDetails,Receivable aReceivable, Client aClient) 
        {
            aReceivable.VoucherNo = GetMaxVoucherNoByTransactionInfix("101");
            int rowAffected= _iAccountGateway.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
            return rowAffected > 0;
        }

        public ICollection<ViewLedgerModel> GetClientLedgerBySubSubSubAccountCode(string clientSubSubSubAccountCode)
        {
            return _iAccountGateway.GetClientLedgerBySubSubSubAccountCode(clientSubSubSubAccountCode);
        }

        public ICollection<Purpose> GetCreditPurposesFromXmlFile(string filePath)
        {
            List<Purpose> purposes = new List<Purpose>();
            var xmlData = XDocument.Load(filePath).Element("PurposeList")?.Elements();
            if (xmlData != null)
            {
                foreach (XElement element in xmlData)
                {
                    Purpose aPurpose = new Purpose();
                    var elementFirstAttribute = element.FirstAttribute.Value;
                    aPurpose.Serial = elementFirstAttribute;
                    var elementValue = element.Elements();
                    var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                    aPurpose.PurposeCode = xElements[0].Value;
                    aPurpose.Amounts = Convert.ToDecimal(xElements[1].Value);
                    aPurpose.PurposeName = xElements[2].Value;
                    aPurpose.Remarks = xElements[3].Value;
                    aPurpose.DebitOrCredit = xElements[4].Value;

                    purposes.Add(aPurpose);
                }
            }
            return purposes;
        }

        public ICollection<CollectionModel> GetTotalCollectionByBranch(int branchId)
        {
            return _iAccountGateway.GetTotalCollectionByBranch(branchId);
        }

        public CollectionModel GetCollectionAmountById(long id)
        {
            return _iAccountGateway.GetCollectionAmountById(id);
        }

        public ICollection<ViewReceivableDetails> GetActivetedReceivableListByBranch(int branchId)
        {
            return _iAccountGateway.GetActivetedReceivableListByBranch(branchId);
        }

        public ViewReceivableDetails GetActivatedReceivableDetailsById(long chequeDetailsId)
        {
            return _iAccountGateway.GetActivatedReceivableDetailsById(chequeDetailsId);
        }

        

        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _invoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        public int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals)
        {
            int maxSl = GetMaxJournalVoucherNoOfCurrentYear();
            string refCode = _iCommonManager.GetAllSubReferenceAccounts().ToList()
                .Find(n => n.Id == Convert.ToInt32(ReferenceType.JournalVoucher)).Code;
            aJournal.VoucherRef = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + (maxSl + 1);
            aJournal.VoucherNo = maxSl + 1;
            return _iAccountGateway.SaveJournalVoucher(aJournal,journals);
        }

        private int GetMaxJournalVoucherNoOfCurrentYear()
        {
            return _iAccountGateway.GetMaxJournalVoucherNoOfCurrentYear();
        }

        public IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iAccountGateway.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId)
        {
            return _iAccountGateway.GetAllJournalVouchersByCompanyId(companyId);
        }


        public int SaveVoucher(Voucher voucher)
        {
            int maxSl = GetMaxVoucherNoOfCurrentYearByVoucherType(voucher.VoucherType);
            voucher.VoucherRef = GenerateVoucherRef(maxSl,voucher.VoucherType);
            voucher.VoucherNo = maxSl+1;
            return _iAccountGateway.SaveVoucher(voucher);
        }
        private int GetMaxVoucherNoOfCurrentYearByVoucherType(int voucherType)
        {
            return _iAccountGateway.GetMaxVoucherNoOfCurrentYearByVoucherType(voucherType); 
        }

        private string GenerateVoucherRef(int maxSl,int voucherType)
        {
            string infix = "";
            switch (voucherType)
            {
                case 1:
                   
                    {
                      infix = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n=>n.Id==Convert.ToInt32(ReferenceType.CreditVoucher)).Code;
                      break;
                    }

                case 2:
                {
                    infix = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.DebitVoucher)).Code;
                    break;
                    }
                case 3:
                {
                    infix = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.ChequePaymentVoucher)).Code;
                    break;
                    }
                case 4:
                {
                    infix = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.ChequeReceiveVoucher)).Code;
                    break;
                    }
            }
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + infix + (maxSl + 1);
            return reference;
        }

        public IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType) 
        {
           return _iAccountGateway.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, voucherType);
        }


        public Voucher GetVoucherByVoucherId(int voucherId)
        {
            return _iAccountGateway.GetVoucheByVoucherId(voucherId);
        }

        public IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId)
        {
            return _iAccountGateway.GetVoucherDetailsByVoucherId(voucherId);
        }

        public IEnumerable<Voucher> GetVoucherList()
        {
            return _iAccountGateway.GetVoucherList();
        }

        public IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId)
        {
            return _iAccountGateway.GetVoucherListByCompanyId(companyId);
        }

        public IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetVoucherListByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<Voucher> GetPendingVoucherList()
        {
            return _iAccountGateway.GetPendingVoucherList();
        }
        public bool CancelVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected= _iAccountGateway.CancelVoucher(voucherId,reason,userId);
            return rowAffected>0;
        }

        public bool ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails,int userId)
        {
            int rowAffected = _iAccountGateway.ApproveVoucher(aVoucher, voucherDetails,userId);
            return rowAffected > 0;
        }

        public bool UpdateVoucher(Voucher voucher,List<VoucherDetails> voucherDetails)
        {
            int rowAffected = _iAccountGateway.UpdateVoucher(voucher,voucherDetails);
            return rowAffected > 0;
        }

        public List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId)
        {
            return _iAccountGateway.GetJournalVoucherDetailsById(journalVoucherId);
        }

        public JournalVoucher GetJournalVoucherById(int journalId)
        {
            return _iAccountGateway.GetJournalVoucherById(journalId);
        }

        public bool CancelJournalVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected = _iAccountGateway.CancelJournalVoucher(voucherId, reason, userId);
            return rowAffected > 0;
        }

        public bool UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers)
        {
            int rowAffected = _iAccountGateway.UpdateJournalVoucher(voucher, journalVouchers);
            return rowAffected > 0;
        }

        public List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetAllPendingJournalVoucherByBranchAndCompanyId(branchId, companyId);
        }

        public bool ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId)
        {
            int rowAffected = _iAccountGateway.ApproveJournalVoucher(aVoucher, voucherDetails, userId);
            return rowAffected > 0;
        }

        public bool ApproveVat(Vat vat)
        {
            return _iAccountGateway.ApproveVat(vat) > 0;
        }

        public bool ApproveDiscount(Discount discount)
        {
            return _iAccountGateway.ApproveDiscount(discount) > 0;
        }

        public AccountSummary GetAccountSummaryOfCurrentMonth()
        {
            return _iAccountGateway.GetAccountSummaryOfCurrentMonth();
        }

        public AccountSummary GetAccountSummaryofCurrentMonthByCompanyId(int companyId)
        {
            return _iAccountGateway.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
        }

        public  AccountSummary GetAccountSummaryofCurrentMonthByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetAccountSummaryofCurrentMonthByBranchAndCompanyId(branchId, companyId);
        }
    }
}