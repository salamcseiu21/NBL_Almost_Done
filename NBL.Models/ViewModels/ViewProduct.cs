using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.Models.ViewModels
{
    public class ViewProduct
    {
        public int BranchId { get; set; }

        public string ProductTypeName { get; set; }
        public string ProductBarCode { get; set; }
        public DateTime ProductionDate { get; set; }    
        public string ProductCategoryName { get; set; }
        [Display(Name = "Dealer Comision")]
        public decimal DealerComision { get; set; }
        public int TotalRe { get; set; }
        public int TotalSoldQty { get; set; } 
        public int TotalTr { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
        public int RequisitionQty { get; set; } 
        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
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
        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; }


    }
}