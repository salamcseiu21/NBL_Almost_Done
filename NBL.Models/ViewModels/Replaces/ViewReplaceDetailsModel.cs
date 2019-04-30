using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Replaces
{
   public class ViewReplaceDetailsModel
    {
        public long ReplaceDetailsId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }    
    }
}
