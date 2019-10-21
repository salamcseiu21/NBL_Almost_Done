using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;

namespace NBL.Models.ViewModels.Deliveries
{
   public class ViewDeliveryModel
    {
        public ICollection<InvoiceDetails> InvoiceDetailses { get; set; }
        public Client Client { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<ScannedProduct> ScannedBarCodes { get; set; }
        public ICollection<DeliveryDetails> DeliveryDetailses { get; set; }
        public Delivery Delivery { get; set; }
        public ICollection<ViewClientStockProduct> ClientStockProducts { get; set; }

    }
}
