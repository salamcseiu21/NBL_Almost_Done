using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.Masters
{
    public class EmployeeType
    {
        public int EmployeeTypeId { get; set; }
        [Display(Name = "Employee Type Name")]
        [Required]
        [StringLength(150)]
        public string EmployeeTypeName { get; set; }

    }
}