using System;

namespace NBL.Models.EntityModels.Identities
{
    public class UserRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}