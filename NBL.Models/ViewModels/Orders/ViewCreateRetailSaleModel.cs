
using System.ComponentModel.DataAnnotations;
namespace NBL.Models.ViewModels.Orders
{
    public class ViewCreateRetailSaleModel
    {
        [Required]
        public string BarCode { get; set; }
    }
}
