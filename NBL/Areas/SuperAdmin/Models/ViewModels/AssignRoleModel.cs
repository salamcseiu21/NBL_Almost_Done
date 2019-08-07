using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBL.Areas.SuperAdmin.Models.ViewModels
{
    public class AssignRoleModel
    {
        public long Id { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AssignedByUserId { get; set; }
        public int ActiveStatus { get; set; }

    }
}