using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Branches;

namespace NBL.Models.ViewModels.TransferProducts
{
    public class ViewTransferProductModel
    {
        public int ProductId { get; set; }
        public string TransactionRef { get; set; }
        public long TransferId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransferDate { get; set; }
        public int TransferByUserId { get; set; }
        public int ApprovedByUserId { get; set; }
        public DateTime ApprovedDateTime { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int Status { get; set; }
        [Display(Name = "Delivered Quantity")]
        public int DeliveredQuantity { get; set; }
        [Display(Name = "Received Quantity")]
        public int ReceivedQuantity { get; set; }
        public DateTime DeliveredAt { get; set; }
        [Display(Name = "Delivered Status")]
        public int DeliveredStatus { get; set; }
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
    }
}
