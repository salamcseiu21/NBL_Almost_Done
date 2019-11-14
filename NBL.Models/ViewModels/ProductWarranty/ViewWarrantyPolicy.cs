using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.ProductWarranty
{
   public class ViewWarrantyPolicy
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Warranty Period InDays")]
        [Range(0, 3000)]
        public int WarrantyPeriodInDays { get; set; }
        [Range(0, 3000)]
        [Display(Name = "Age Limit In Dealer Stock")]
        public int AgeLimitInDealerStock { get; set; } 
        [Required]
        [Display(Name = "Warranty From")]
        public string WarrantyFrom { get; set; }
        public string FromBatch { get; set; }
        public string ToBatch { get; set; }
        [Display(Name = "Client Name")]
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int IsActive { get; set; }
        public DateTime SystemDateTime { get; set; }
    }
}
