using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Deliveries
{
   public class ViewDeliveryDetails
    {
        public long InventoryId { get; set; }
        public long InventoryDetailsId { get; set; } 
        public DateTime TransactionDate { get; set; }
        public DateTime? SaleDate { get; set; } 
        public string TransactionRef { get; set; }
        public string Barcode { get; set; }
        public DateTime? RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
    }
}
