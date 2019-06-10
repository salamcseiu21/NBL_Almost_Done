using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Products
{
   public class ViewCreateProductDetailsModel
    {
        [Required]
        [Display(Name = "Product Name")]

        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } 
        public ICollection<Product> Products { get; set; }
        [Display(Name = "Unit Price")]
        [Required]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Updated At")]
        [Required]
        public DateTime UpdatedDate { get; set; }
        public int UpdatedByUserId { get; set; }    
        public DateTime SystemDateTime { get; set; }    
    }
}
