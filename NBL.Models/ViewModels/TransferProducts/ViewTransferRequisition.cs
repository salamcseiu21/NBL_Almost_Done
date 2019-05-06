using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.ViewModels.TransferProducts
{
  public  class ViewTransferRequisition
    {
        public ICollection<TransferRequisitionDetails> TtransferRequisitions { set; get; }
        public ViewBranch Branch { get; set; }
        public TransferRequisition TransferRequisition { get; set; }
    }
}
