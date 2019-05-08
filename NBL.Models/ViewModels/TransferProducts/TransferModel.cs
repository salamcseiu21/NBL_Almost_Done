using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class TransferModel
    {

        public long TransferId { get; set; }
        public TransferRequisition TransferRequisition { get; set; } 
        public ICollection<TransferRequisitionDetails> Detailses { get; set; }
       
        public ICollection<ViewTransferProductDetails> Products { set; get; }
        public Delivery Delivery { get; set; } 
        public ICollection<ScannedProduct> ScannedBarCodes { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
      
        public ViewUser User { get; set; }
        public ViewTransferProductModel ViewTransferProductModel { get; set; } 
    }
}
