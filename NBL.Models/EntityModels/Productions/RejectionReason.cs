using System;
namespace NBL.Models.EntityModels.Productions
{
    public  class RejectionReason
    {
        public int RejectionReasonId { get; set; }
        public string Reason { get; set; }
        public DateTime SystemDateTime { get; set; }
    }
}
