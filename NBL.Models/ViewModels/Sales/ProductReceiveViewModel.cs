
using System.Collections.Generic;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.ViewModels.Sales
{
    public class ProductReceiveViewModel
    {

        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
        public ICollection<TransactionModel> TransactionModels { get; set; }
    }
}
