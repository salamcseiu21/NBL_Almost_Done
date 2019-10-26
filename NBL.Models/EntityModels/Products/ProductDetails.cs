using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.Products
{
    public class ProductDetails
    {

        public int ProductDetailsId { get; set; }   
        public DateTime UpdatedDate { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public ICollection<Product> Products { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? DealerDiscount { get; set; }
        public decimal? IndividualDiscount { get; set; }
        public decimal? CorporateDiscount { get; set; }
        public string CategoryName { get; set; }
        public bool HasWarrenty { get; set; }
        public string IsActive { get; set; } 

    }
}