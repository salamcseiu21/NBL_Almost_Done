using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.EntityModels.Productions
{
   public class ProductionModel
    {
        public int UserId { get; set; }
        public ICollection<ScannedProduct> ScannedProducts { get; set; }
        public int TotalQuantity { get; set; }
        public string TransactionType { get; set; }
        public string TransactionRef { get; set; }
        public DateTime SystemDateTime { get; set; }    
    }
}
