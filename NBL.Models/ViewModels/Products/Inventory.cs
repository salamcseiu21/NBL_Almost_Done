using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Products
{
   public class Inventory
    {
        public long InventoryId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionRef { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; } 
    }
}
