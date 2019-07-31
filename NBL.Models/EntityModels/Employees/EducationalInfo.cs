using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Employees
{
    public class EducationalInfo
    {
        public long EducationalInfoId { get; set; }
        [Required]
        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        [Display(Name = "Passing Year")]
        public int PassingYear { get; set; }
        [Required]
        [Display(Name = "Group/Subject")]
        public string GroupSubject { get; set; }
        [Required]
        [Display(Name = "Board Name")]
       
        public string BoardName { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; } 
        public DateTime UpdateDate { get; set; }
        public DateTime SystemDateTime { get; set; } 
    }
}
