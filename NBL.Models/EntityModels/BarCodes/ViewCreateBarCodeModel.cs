﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace NBL.Models.EntityModels.BarCodes
{
    public class ViewCreateBarCodeModel
    {
        [Required]
        [Display(Name = "Date Code")]
        public int ProductionDateCodeId { get; set; }
        [Display(Name = "Date")]
        [Required]
        public DateTime ProductionDate { get; set; }
        [Display(Name = "Shift")]
        [Required]
        public string ShiftNo { get; set; }  
        [Required]
        public int Total { get; set; }  
        [Required]
        [Display(Name = "Line Number")]
        public int ProductionLineId { get; set; }
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }

        public int GenerateByUserId { get; set; }   
        public ICollection<BarCodeModel> BarCodes { set; get; }

        public ViewCreateBarCodeModel()
        {
            BarCodes=new List<BarCodeModel>();
        }
    }
}
