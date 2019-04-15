using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels.Deliveries;

namespace NBL.Models.ViewModels
{
   public class ViewInvoiceModel
    {
        public IEnumerable<InvoiceDetails> InvoiceDetailses { get; set; }
        public ICollection<ViewDeliveredOrderModel> DeliveryDetails { get; set; }
        public Delivery Delivery { get; set; }  
        public ViewClient Client { get; set; }
        public Order Order { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
