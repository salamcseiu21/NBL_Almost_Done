using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;

namespace NBL.Areas.Sales.DAL.Contracts
{
   public interface IInvoiceGateway
   {
       int GetMaxInvoiceNoOfCurrentYear();
       int GetMaxInvoiceNo();
       IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId);
       int GetMaxVoucherNoByTransactionInfix(string infix);
       int Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice);
       IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<Invoice> GetAllInvoicedOrdersByBranchCompanyAndUserId(int branchId, int companyId,
           int invoiceByUserId);
       IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId);
       IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId);
       IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId);
       IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef);
       Invoice GetInvoicedOrderByInvoiceId(int invoiceId);
       ICollection<ViewProduct> GetDeliveredProductsByInvoiceRef(string invoiceRef);
       ICollection<Invoice> GetInvoicedOrdersByCompanyIdAndDate(int companyId, DateTime date);
   }
}
