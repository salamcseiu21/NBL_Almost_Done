using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.VatDiscounts
{
    public class Vat
    {
        public int VatId { get; set; }
        [Required(ErrorMessage = "Please! type product name ....")]
        [Display(Name = "Product Name")]
        
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Vat Amount")]
        public decimal VatAmount { get; set; }
        [Required]
        [Display(Name = "Updated At")]
        public DateTime UpdateDate { get; set; }
        public int UpdateByUserId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int ApprovedByUserId { get; set; }
        public string EntryStatus { get; set; }
        public string IsCurrent { get; set; }
        public DateTime SysDateTime { get; set; }
        public Product Product { get; set; }

        public Vat()
        {
                Product=new Product();
        }

    }
}