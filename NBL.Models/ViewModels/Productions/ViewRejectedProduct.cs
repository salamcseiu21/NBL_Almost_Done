using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Productions;

namespace NBL.Models.ViewModels.Productions
{
    public class ViewRejectedProduct
    {
        public long RejectionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public string Barcode { get; set; }
        public string Notes { get; set; }
        public string NotesByQc { get; set; }
        [Display(Name = "Reason")]
        public int RejectionReasonId { get; set; }

        public int VerificationStatus { get; set; } 
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }
        public RejectionReason RejectionReason { get; set; }
    }
}
