using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Requisitions
{
    public class ViewCreateGeneralRequsitionModel
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Requisition For")]
        public DateTime RequisitionDate { get; set; }   
        public int RequisitionForId { get; set; }
        public int RequisitionByUserId { get; set; }
    }
}
