using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Reports
{
   public class UserWiseOrder
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalOrder { get; set; }
        public string EmployeeName { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeImage { set; get; }
        public string EmployeeSignature { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }   
    }
}
