using NBL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels;
using NBL.DAL.Contracts;
using System;
using NBL.Models.EntityModels.Branches;

namespace NBL.BLL
{
    public class BranchManager:IBranchManager
    {

        readonly IBranchGateway _iBranchGateway;
        public BranchManager(IBranchGateway iBranchGateway)
        {
            _iBranchGateway = iBranchGateway;
        }
       

        private string GenerateSubSubSubAccount(string prefix)
        {
            int maxSlNo = _iBranchGateway.GetMaxBranchSubSubSubAccountCode();
            return prefix + (maxSlNo+1);
        }

        public bool Add(Branch branch)
        {
            branch.SubSubSubAccountCode = GenerateSubSubSubAccount("1101");
            int rowAffected = _iBranchGateway.Add(branch);
            return rowAffected > 0;
        }

        public bool Update(Branch branch)
        {
            int rowAffected = _iBranchGateway.Update(branch);
            return rowAffected > 0;
        }

        public bool Delete(Branch model)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ViewBranch> GetAllBranches()
        {
            var branches = _iBranchGateway.GetAllBranches().ToList();
            return branches;
        }

        public List<ViewAssignedRegion> GetAssignedRegionToBranchList()
        {
           
            var items= _iBranchGateway.GetAssignedRegionToBranchList();
            return items.ToList();
        }

        public SelectList GetBranchSelectList()
        {
            var branches = from branch in GetAllBranches().ToList().Where(i => !i.BranchName.Contains("Corporate"))
                select new Branch
                {
                    BranchId = branch.BranchId,
                    BranchName = branch.BranchName,
                    BranchAddress = branch.BranchAddress
                };

            return new SelectList(branches, "BranchId", "BranchName");
        }

        public Branch GetById(int id)
        {
            return _iBranchGateway.GetById(id);
        }

        public ICollection<Branch> GetAll()
        {
            return _iBranchGateway.GetAll();
        }
    }
}