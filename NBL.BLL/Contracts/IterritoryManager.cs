

using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface ITerritoryManager:IManager<Territory>
   {
       IEnumerable<Territory> GetTerritoryListByBranchId(int branchId);
       IEnumerable<Territory> GetTerritoryListByRegionId(int regionId);
       int AssignUpazillaToTerritory(Territory aTerritory);
       int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, ViewUser user);
   }
}
