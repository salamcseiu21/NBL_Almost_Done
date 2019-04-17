using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Productions
{
   public class ProductVerificationModel
    {
        public long RejectionId { get; set; }
        public int VerifiedByUserId { get; set; }
        public string Notes { get; set; }
        public int QcPassorFailedStatus { get; set; }   
    }
}
