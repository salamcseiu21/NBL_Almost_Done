using System.ComponentModel.DataAnnotations;

namespace NBL.Models.ViewModels.Orders
{
    public class TempOrderedProduct
    {

        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
      
        [Display(Name = "Sub Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }

        public int Quantity { get; set; }
        public int ProductDetailsId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int VatId { get; set; }
        public decimal Vat { get; set; }
        public int DiscountId { get; set; }
        public decimal DiscountAmount { get; set; }
        public int ClientId { get; set; }   
    }
}
