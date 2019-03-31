using System;
using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;

namespace NBL.BLL
{
  public class DistrictManager:IDistrictManager
    {
        private readonly IDistrictGateway _iDistrictGateway;

        public DistrictManager(IDistrictGateway iDistrictGateway)
        {
            _iDistrictGateway = iDistrictGateway;
        }
        public bool Add(District model)
        {
            throw new NotImplementedException();
        }

        public bool Update(District model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(District model)
        {
            throw new NotImplementedException();
        }

        public District GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<District> GetAll()
        {
            return _iDistrictGateway.GetAll();
        }

        public IEnumerable<District> GetAllDistrictByDivistionId(int divisionId)
        {
           return _iDistrictGateway.GetAllDistrictByDivistionId(divisionId);
        }

        public IEnumerable<District> GetAllDistrictByRegionId(int regionId)
        {
            return _iDistrictGateway.GetAllDistrictByRegionId(regionId);
        }

        public IEnumerable<District> GetUnAssignedDistrictListByRegionId(int regionId)
        {
            return _iDistrictGateway.GetUnAssignedDistrictListByRegionId(regionId);
        }
    }
}
