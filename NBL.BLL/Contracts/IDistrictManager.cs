using System.Collections.Generic;
using NBL.Models.EntityModels.Locations;

namespace NBL.BLL.Contracts
{
   public interface IDistrictManager:IManager<District>
    {
        IEnumerable<District> GetAllDistrictByDivistionId(int divisionId);
        IEnumerable<District> GetAllDistrictByRegionId(int regionId);
        IEnumerable<District> GetUnAssignedDistrictListByRegionId(int regionId);
    }
}
