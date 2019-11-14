using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.Services
{
   public class TestPolicyModel
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Test Category")]
        public int TestCategoryId { get; set; }
        [Required]
        public string Parameter { get; set; }
        [Required]
        [Display(Name = "Acceptable Value")]
        public decimal AcceptableValue { get; set; }
        [Required]
        public string Remarks { get; set; }
        public int UserId { get; set; }

    }
}
