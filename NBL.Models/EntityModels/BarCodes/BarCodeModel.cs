using System;
namespace NBL.Models.EntityModels.BarCodes
{
    public class BarCodeModel
    {
        public long BarCodeModelId { get; set; }    
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string BatchCode { get; set; }
        public string LineNumber { get; set; }  
        public int PrintByUserId { get; set; }  
        public int PrintStatus { get; set; }
        public DateTime SystemDateTime { get; set; }   
         
    }
}
