
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.ViewModels.Orders
{
    public class DeliveredRefModel
    {
        public int ClientId { get; set; }
        public string Reference { get; set; }
        [Required]
        [Display(Name = "Delivery Ref")]
        public long DeliveryId { get; set; }    
    }
}
