using System.Collections.Generic;
using NBL.Models.EntityModels.Masters;

namespace NBL.Models.ViewModels
{
   public class ViewReceivableCreateModel
    {
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<TransactionType> TransactionTypes { get; set; } 
    }
}
