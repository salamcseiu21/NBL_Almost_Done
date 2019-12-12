using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTransferModel
    {
        public long TransferId { get; set; }
        public string TransactionRef { get; set; } 
        public string FromBranch { get; set; } 
        public string ToBranch { get; set; } 
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransferDate { get; set; }

    }
}
