using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.Contracts;
using NBL.Models.EntityModels.Designations;
using NBL.Models.EntityModels.Employees;

namespace NBL.Models.EntityModels.Departments
{
    public class Department:IGetInformation
    {
        public int DepartmentId { get; set; }
        [Required]
        [Display(Name ="Code")]
        [StringLength(5, MinimumLength = 2)]
        public string DepartmentCode { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public bool DepartmentCodeInUse { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Designation> Designations { get; set; }

        public Department()
        {
            Employees=new List<Employee>();
            Designations=new List<Designation>();
        }

        public string GetBasicInformation()
        {
            return DepartmentName;
        }
        public string GetFullInformation()
        {
            return $"<strong>Code : </strong> {DepartmentCode} <br/><strong>Name : </strong>{DepartmentName}";
        }
    }
}