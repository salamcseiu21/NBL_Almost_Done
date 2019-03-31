using System;
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface IUpazillaGateway
   {
       IEnumerable<Upazilla> GetAllUpazillaByDistrictId(int districtId);
       IEnumerable<Upazilla> GetUnAssignedUpazillaByTerritoryId(int territoryId);
       IEnumerable<Upazilla> GetAssignedUpazillaLsitByTerritoryId(int territoryId);
       IEnumerable<ViewAssignedUpazilla> GetAssignedUpazillaList();
   }
}
