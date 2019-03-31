using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.BarCodes
{
    public class PrintBarCodeModel
    {
        [Required]
        [Display(Name = "Production Date Code")]
        public int ProductionDateCodeId { get; set; }
        [Required]
        public int From { get; set; }
        [Required]
        public int To { get; set; }
        [Required]
        [Display(Name = "Line Number")]
        public int ProductionLineId { get; set; }
        public string ProductionLineNumber { get; set; } 
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Sub Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Product Name")]
        public long BarCodeMasterId { get; set; }   

    }
}
