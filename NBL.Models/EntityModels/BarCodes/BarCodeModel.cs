using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.BarCodes
{
    public class BarCodeModel
    {
        public long BarCodeModelId { get; set; }    
        public string Barcode { get; set; }
        public int PrintByUserId { get; set; }  
        public int PrintStatus { get; set; }
        public DateTime SystemDateTime { get; set; }   
         
    }
}
