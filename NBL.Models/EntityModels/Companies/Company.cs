using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.Companies
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Logo { get; set; }
    }
}