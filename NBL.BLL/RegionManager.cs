using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
  public class RegionManager:IRegionManager
    {
        private readonly IRegionGateway _iRegionGateway;
        private readonly ITerritoryGateway _iTerritoryGateway;
        private readonly DistrictGateway _districtGateway=new DistrictGateway();

        public RegionManager(IRegionGateway iRegionGateway,ITerritoryGateway iTerritoryGateway)
        {
            _iRegionGateway = iRegionGateway;
            _iTerritoryGateway = iTerritoryGateway;
        }
     

        public IEnumerable<ViewRegion> GetRegionListWithDistrictInfo()
        {
           return _iRegionGateway.GetRegionListWithDistrictInfo();
        }

        public ViewRegion GetRegionDetailsById(int regionDetailsId)
        {
            return _iRegionGateway.GetRegionDetailsById(regionDetailsId);
        }

        public int AssignDristrictToRegion(Region aRegion)
        {
            return _iRegionGateway.AssignDristrictToRegion(aRegion);
        }

        public IEnumerable<Region> GetUnAssignedRegionList()
        {
            return _iRegionGateway.GetUnAssignedRegionList();
        }

        public int AssignRegionToBranch(Branch branch, ViewUser user)
        {
           return  _iRegionGateway.AssignRegionToBranch(branch, user);
        }


        public List<Region> GetAssignedRegionListToBranchByBranchId(int branchId)
        {

            var regions = _iRegionGateway.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            foreach (var region in regions)
            {
                region.Territories = _iTerritoryGateway.GetTerritoryListByRegionId(region.RegionId).ToList();
                region.Districts = _districtGateway.GetAllDistrictByRegionId(region.RegionId).ToList();
            }
            return regions;
        }
        public Branch GetBranchInformationByRegionId(int regionId)
        {

            return _iRegionGateway.GetBranchInformationByRegionId(regionId);
        }

        public int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, ViewUser user)
        {
          return _iRegionGateway.UnAssignDistrictFromRegion(regionDetails, reason, user);
        }

        public bool Add(Region model)
        {
            return _iRegionGateway.Add(model)>0;
        }

        public bool Update(Region model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Region model)
        {
            throw new System.NotImplementedException();
        }

        public Region GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Region> GetAll()
        {
            var regions = _iRegionGateway.GetAll();
            foreach (var region in regions)
            {
                region.Territories = _iTerritoryGateway.GetTerritoryListByRegionId(region.RegionId).ToList();
            }
            return regions;
        }
    }
}
