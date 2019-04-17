using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Productions
{
   public class RejectedProduct
    {
        public long RejectedProductId { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Notes { get; set; }
        public string NotesByQc { get; set; }
        [Required]
        [Display(Name = "Reason")]
        public int RejectionReasonId { get; set; }  
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }
        public RejectionReason RejectionReason { get; set; }    
       
    }
}
