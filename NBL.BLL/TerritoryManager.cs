using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
   public class TerritoryManager:ITerritoryManager
    {
       private readonly  ITerritoryGateway _iTerritoryGateway;
       private readonly IUpazillaGateway _iUpazillaGateway;

        public TerritoryManager(ITerritoryGateway iTerritoryGateway,IUpazillaGateway iUpazillaGateway)
        {
            _iTerritoryGateway = iTerritoryGateway;
            _iUpazillaGateway = iUpazillaGateway;
        }

        public IEnumerable<Territory> GetTerritoryListByBranchId(int branchId)
        {
            var territories = _iTerritoryGateway.GetTerritoryListByBranchId(branchId);
            foreach (var territory in territories)
            {
                territory.UpazillaList = _iUpazillaGateway.GetAssignedUpazillaLsitByTerritoryId(territory.TerritoryId)
                    .ToList();
            }
            return territories;
        }
        public IEnumerable<Territory> GetTerritoryListByRegionId(int regionId)
        {
            return _iTerritoryGateway.GetTerritoryListByRegionId(regionId);
        }

        public int AssignUpazillaToTerritory(Territory aTerritory)
        {
            return _iTerritoryGateway.AssignUpazillaToTerritory(aTerritory);
        }

        public int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, ViewUser user)
        {
            return _iTerritoryGateway.UnAssignUpazillaFromTerritory(territoryDetailsId, reason, user);
        }

        public bool Add(Territory model)
        {
            return _iTerritoryGateway.Add(model)>0;
        }

        public bool Update(Territory model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Territory model)
        {
            throw new System.NotImplementedException();
        }

        public Territory GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Territory> GetAll()
        {
            var territories = _iTerritoryGateway.GetAll();
            foreach (var territory in territories)
            {
                territory.UpazillaList = _iUpazillaGateway.GetAssignedUpazillaLsitByTerritoryId(territory.TerritoryId)
                    .ToList();
            }
            return territories;
        }
    }
}
