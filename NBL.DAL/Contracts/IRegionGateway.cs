using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IRegionGateway:IGateway<Region>
    {
        IEnumerable<ViewRegion> GetRegionListWithDistrictInfo();
        ViewRegion GetRegionDetailsById(int regionDetailsId);
        int AssignDristrictToRegion(Region aRegion);
        IEnumerable<Region> GetUnAssignedRegionList();
        int AssignRegionToBranch(Branch branch, ViewUser user);
        List<Region> GetAssignedRegionListToBranchByBranchId(int branchId);
        Branch GetBranchInformationByRegionId(int regionId);
        int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, ViewUser user);
        int UnAssignUpazilla(ViewRegion viewRegion);
    }
}
