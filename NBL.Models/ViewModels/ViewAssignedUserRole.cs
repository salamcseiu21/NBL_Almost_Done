using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels
{
    public class ViewAssignedUserRole
    {
        public long AssignedId { get; set; } 
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }  
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Alias { get; set; }   
        public int ActiveStatus { get; set; } 

    }
}
