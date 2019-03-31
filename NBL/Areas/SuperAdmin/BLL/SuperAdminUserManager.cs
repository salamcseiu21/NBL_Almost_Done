using System.Collections.Generic;
using System.Linq;
using NBL.Areas.SuperAdmin.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;

namespace NBL.Areas.SuperAdmin.BLL
{
    public class SuperAdminUserManager
    {

        SuperAdminUserGateway gateway = new SuperAdminUserGateway();
        public string AssignBranchToUser(User user,List<Branch> branchList)
        {
            int rowAffected = 0;
            foreach (var branch in branchList)
            {
                bool isAssignedBefore = IsThisBranchAssignedBefore(branch,user);
                if(!isAssignedBefore)
                {
                    rowAffected += gateway.AssignBranchToUser(user, branch);
                }
               
            }  
            if (rowAffected > 0)
                return "Assigned sucessfully!";
            return "Already Assign before";
        }

        private bool IsThisBranchAssignedBefore(Branch branch,User user)
        {
          Branch aBranch= GetAssignedBranchByUserId(user.UserId).ToList().Find(n=>n.BranchId==branch.BranchId);
            if(aBranch!=null)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Branch> GetAssignedBranchByUserId(int userId)
        {
          return gateway.GetAssignedBranchByUserId(userId);
        }
    }
}