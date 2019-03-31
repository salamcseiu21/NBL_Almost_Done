using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.Deliveries
{
   public class ViewDeliveryModel
    {
        public ICollection<InvoiceDetails> InvoiceDetailses { get; set; }
        public Client Client { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<ScannedProduct> ScannedBarCodes { get; set; }
    }
}
