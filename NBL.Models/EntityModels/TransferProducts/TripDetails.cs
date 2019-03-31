using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.TransferProducts
{
   public class TripDetails
    {
        public long TripDetailsId { get; set; }
        public long RequisitionId { get; set; }
        public long TripId { get; set; }
        public DateTime SystemDateTime { get; set; }
    }
}
