using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.TransferProducts
{
    public class Trip
    {
        public long TripId { get; set; }
        public string Name { get; set; }
        public int CreatedByUserId { get; set; }

    }
}
