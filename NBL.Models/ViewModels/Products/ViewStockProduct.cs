using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Products
{
   public class ViewStockProduct
    {
        public string ProductBarcode { get; set; }
        public long InventoryMasterId { get; set; } 
    }
}
