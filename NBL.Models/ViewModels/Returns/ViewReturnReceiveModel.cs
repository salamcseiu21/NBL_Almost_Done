using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.Returns
{
   public class ViewReturnReceiveModel
    {
        public long SalesReturnId { get; set; }
        public List<ReturnDetails> ReturnDetailses { get; set; }
        public int ReceiveByUserId { get; set; }
        public string TransactionRef { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }   
        public List<ScannedProduct> Products { get; set; }
        public ReturnModel ReturnModel { get; set; } 
    }
}
