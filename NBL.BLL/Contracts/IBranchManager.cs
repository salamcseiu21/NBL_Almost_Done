using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
    public interface IBranchManager:IManager<Branch>
    {
        IEnumerable<ViewBranch> GetAllBranches();
        List<ViewAssignedRegion> GetAssignedRegionToBranchList();
        SelectList GetBranchSelectList();
       
    }
}
