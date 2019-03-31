using System.ComponentModel.DataAnnotations;
using NBL.Models.Contracts;
using NBL.Models.EntityModels.Departments;

namespace NBL.Models.EntityModels.Designations
{
    public class Designation:IGetInformation
    {
        public int DesignationId { get; set; }
        [Display(Name = "Designation Code")]
        [Required]
        [StringLength(5,MinimumLength = 2)]
        public string DesignationCode { get; set; } 
        [Display(Name = "Designation")]
        [Required]
        public string DesignationName { get; set; }
        public bool DesignationCodeInUse { get; set; }
        [Display(Name = "Department")] 
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public string GetBasicInformation()
        {
            return DesignationName;
        }

        public string GetFullInformation()
        {
            return $"Code : {DesignationCode} <br/>Name:{DesignationName}";
        }
    }
}