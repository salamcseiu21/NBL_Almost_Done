using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.Contracts;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.Models.EntityModels.Products
{
    public class Product:IGetInformation
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public string ScannedProductCodes { get; set; } 
        [Required]
        public int ProductTypeId { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Sub Sub Sub Account Code")] 
        public string SubSubSubAccountCode { get; set; }
        public string Unit { get; set; }
        [Display(Name = "Unit in Stock")]
        public int UnitInStock { get; set; }
        [Display(Name = "Product Added Date")]
        public DateTime ProductAddedDate { get; set; }
        public string ProductImage { get; set; } 
        public int Quantity { get; set; }
        public int ProductDetailsId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int VatId { get; set; }
        public decimal Vat { get; set; }
        public int DiscountId { get; set; }
        public decimal DiscountAmount { get; set; }
        public Discount Discount { get; set; }
        public decimal SubTotal => Quantity * SalePrice;

        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; } 
        public string GetBasicInformation()
        {
            return ProductName + "</br>" + SubSubSubAccountCode;
        }

        public string GetFullInformation()
        {
            return $"Product Name : {ProductName} </br> Code : {SubSubSubAccountCode} </br> Category : {ProductCategory.ProductCategoryName}";
           // return "Product Name:"+ ProductName + "</br> Code:" + SubSubSubAccountCode+"</br>Category:"+ProductCategory.ProductCategoryName;
        }
    }
}