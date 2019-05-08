using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Masters;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTransferProductDetails
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public long TransferId { get; set; }
        public long TransferItemId { get; set; }
        public ProductCategory ProductCategory { get; set; } 
    }
}
