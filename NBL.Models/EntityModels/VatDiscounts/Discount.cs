using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Locations;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.VatDiscounts
{
    public class Discount
    {
        public int DiscountId { get; set; }
        [Required]
        [Display(Name = "Update Date")]
        public DateTime UpdateDate { get; set; }
        public int UpdateByUserId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int ApprovedByUserId { get; set; }
        public int ProductTypeId { get; set; }
        public int TerritoryId { get; set; }
        [Display(Name = "Client Type")]
        public int ClientTypeId { get; set; }
        [Required]
        [Display(Name = "Discount Percent")]
        public decimal DiscountPercent { get; set; }
        public string IsCurrent { get; set; }
        public DateTime SysDateTime { get; set; }
        public string EntryStatus { get; set; }
        public Territory Territory { get; set; }
        public ClientType ClientType { get; set; }
        public Product Product { get; set; } 
        [Required]
        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        public Discount()
        {
            Territory=new Territory();
            Product=new Product();
            ClientType=new ClientType();
        }
    }
}