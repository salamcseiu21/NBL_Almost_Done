using System.Collections.Generic;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;

namespace NBL.Models.ViewModels
{
   public class ViewInvoiceModel
    {
        public IEnumerable<InvoiceDetails> InvoiceDetailses { get; set; }
        public ViewClient Client { get; set; }
        public Order Order { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
