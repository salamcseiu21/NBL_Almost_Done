using System.Collections.Generic;
using NBL.Models.EntityModels.Locations;

namespace NBL.DAL.Contracts
{
    public interface IDistrictGateway:IGateway<District>
    {
        IEnumerable<District> GetAllDistrictByDivistionId(int divisionId);
        IEnumerable<District> GetAllDistrictByRegionId(int regionId);
        IEnumerable<District> GetUnAssignedDistrictListByRegionId(int regionId);
    }
}
