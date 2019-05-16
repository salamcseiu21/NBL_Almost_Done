
using System;
using System.Collections.Generic;
using System.Linq;
using NBL.Areas.Sales.DAL.Contracts;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Enums;
using NBL.Models.ViewModels;

namespace NBL.Areas.Sales.BLL
{
    public class InvoiceManager:IInvoiceManager
    {

        private readonly IInvoiceGateway _iInvoiceGateway;
        private readonly ICommonGateway _iCommonGateway;

        public InvoiceManager(ICommonGateway iCommonGateway, IInvoiceGateway iInvoiceGateway)  
        {
            _iCommonGateway = iCommonGateway;
            _iInvoiceGateway = iInvoiceGateway;
        }
        //-----------13-Sep-2018-----------
        public string Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice)
        {
            
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id==Convert.ToInt32(ReferenceType.Invoice)).Code;
            anInvoice.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxSl = _iInvoiceGateway.GetMaxInvoiceNoOfCurrentYear();
            anInvoice.InvoiceNo = _iInvoiceGateway.GetMaxInvoiceNo() + 1;
            anInvoice.InvoiceRef = GenerateInvoiceRef(maxSl);
           

            int rowAffected = _iInvoiceGateway.Save(orderItems, anInvoice);
            if (rowAffected > 0)
                return "Saved Invoice information Successfully!";
            return "Failed to Save";
        }

        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _iInvoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        private string GenerateInvoiceRef(int maxSl)
        {
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id== Convert.ToInt32(ReferenceType.Invoice)).Code;
            int sN = 1 + maxSl;
            string invoiceRef = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return invoiceRef;
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            var invoices = _iInvoiceGateway.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId);
           return invoices;
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchCompanyAndUserId(int branchId, int companyId,int invoiceByUserId)
        {
            return _iInvoiceGateway.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId, companyId, invoiceByUserId);
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId)
        {
            return _iInvoiceGateway.GetAllInvoicedOrdersByUserId(invoiceByUserId);
        }
        public IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId)
        {
            return _iInvoiceGateway.GetInvoicedRefferencesByClientId(clientId);
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId)
        {
            return _iInvoiceGateway.GetAllInvoicedOrdersByCompanyId(companyId);
        }
        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId) 
        {
            return _iInvoiceGateway.GetInvoicedOrderDetailsByInvoiceId(invoiceId);
        }
        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef)
        {
            return _iInvoiceGateway.GetInvoicedOrderDetailsByInvoiceRef(invoiceRef);
        }
        public Invoice GetInvoicedOrderByInvoiceId(int invoiceId)
        {
            return _iInvoiceGateway.GetInvoicedOrderByInvoiceId(invoiceId); 
        }

        public ICollection<ViewProduct> GetDeliveredProductsByInvoiceRef(string invoiceRef)
        {
            return _iInvoiceGateway.GetDeliveredProductsByInvoiceRef(invoiceRef);
        }

        public ICollection<Invoice> GetInvoicedOrdersByCompanyIdAndDate(int companyId, DateTime date)
        {
            return _iInvoiceGateway.GetInvoicedOrdersByCompanyIdAndDate(companyId,date);
        }

        public ICollection<Invoice> GetLatestInvoicedOrdersByDistributionPoint(int distributionPointId)
        {
            var invoices = _iInvoiceGateway.GetLatestInvoicedOrdersByDistributionPoint(distributionPointId);
            return invoices;
        }

        public ICollection<Invoice> GetAllInvoicedOrdersByDistributionPoint(int distributionPointId)
        {
            var invoices = _iInvoiceGateway.GetAllInvoicedOrdersByDistributionPoint(distributionPointId);
            return invoices;
        }

        public ICollection<Invoice> GetAllInvoicedOrdersByCompanyIdAndStatus(int companyId, int status)
        {
            var invoices = _iInvoiceGateway.GetAllInvoicedOrdersByCompanyIdAndStatus(companyId,status);
            return invoices;
        }
    }
}