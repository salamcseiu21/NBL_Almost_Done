using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Services
{
   public class ForwardDetails
    {
        public long ReceiveId { get; set; }
        public int ForwardFromId { get; set; }
        public int ForwardToId { get; set; }
        public DateTime ForwardDateTime { get; set; }
        public int UserId { get; set; }
        public string ForwardRemarks { get; set; } 
    }
}
