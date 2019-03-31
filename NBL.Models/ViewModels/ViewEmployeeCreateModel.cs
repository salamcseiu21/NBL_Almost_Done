using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Departments;
using NBL.Models.EntityModels.Designations;
using NBL.Models.EntityModels.Masters;

namespace NBL.Models.ViewModels
{
    class ViewEmployeeCreateModel
    {

        public int EmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Employee Type")]
        public int EmployeeTypeId { get; set; }
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Alternate Phone")]
        public string AlternatePhone { get; set; }
        public string Email { get; set; }
        public DateTime DoB { get; set; }
        [Display(Name = "Joining Date")]
        [Required]
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Image")]
        [Required]
        public string EmployeeImage { set; get; }
        [Display(Name = "Signature")]
        [Required]
        public string EmployeeSignature { get; set; }
        public string Notes { get; set; }
        [Display(Name = "NID")]
        [Required]
        public string NationalIdNo { get; set; }
        public int UserId { get; set; }
        public Department Department { get; set; }
        public Designation Designation { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public Branch Branch { get; set; }
        

        
    }
}
