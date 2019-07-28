using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.FinanceModels
{
    public class CollectionModel
    {
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public long AccountDetailsId { get; set; }
        public long VoucherNo { get; set; }
        public DateTime ActiveDate { get; set; }
        public string CollectionMode { get; set; }  
    }
}
