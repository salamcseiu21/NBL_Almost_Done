using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.BarCodes
{
   public class ProductionLine
    {
        
        public int ProductionLineId { get; set; }
        [Display(Name = "Line Number")]
        public string LineNumber { get; set; }  
    }
}
