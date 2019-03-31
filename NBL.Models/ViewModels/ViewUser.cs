using System;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.ViewModels
{
    public class ViewUser
    {
        public string UserName { set; get; }
        public string Password { get; set; }
        public int ActiveStaus { get; set; }
        public int BlockStatus { get; set; }
        public int UserRoleId { get; set; }
        public string Roles { get; set; }
        public int BranchId { get; set; } 
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public DateTime LogInDateTime { get; set; }
        public DateTime LogOutDateTime { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Designation")]
        public string DesignationName { get; set; } 
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Image")]
        public string EmployeeImage { set; get; }
        [Display(Name = "Signature")]
        public string EmployeeSignature { get; set; }
        public int UserId { get; set; }

    }
}