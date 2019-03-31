using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface ITerritoryGateway:IGateway<Territory>
    {
        IEnumerable<Territory> GetTerritoryListByBranchId(int branchId);
        IEnumerable<Territory> GetTerritoryListByRegionId(int regionId);
        int AssignUpazillaToTerritory(Territory aTerritory);
        int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, ViewUser user);
    }

}
