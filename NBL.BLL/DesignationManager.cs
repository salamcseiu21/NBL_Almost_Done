using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Designations;

namespace NBL.BLL
{
    public class DesignationManager:IDesignationManager
    {
        readonly  IDesignationGateway _iDesignationGateway;

        public DesignationManager(IDesignationGateway iDesignationGateway)
        {
            _iDesignationGateway = iDesignationGateway;
        }

        public ICollection<Designation> GetAll()
        {
            return  _iDesignationGateway.GetAll();
        }

        public bool Add(Designation aDesignation)
        {
            int rowAffected = _iDesignationGateway.Add(aDesignation);
            return rowAffected > 0;
        }

        public bool Update(Designation aDesignation)
        {
            int rowAffected = _iDesignationGateway.Update(aDesignation);
            return rowAffected > 0;  
        }

        public bool Delete(Designation model)
        {
            throw new System.NotImplementedException();
        }

        public Designation GetDesignationByCode(string code)
        {
            return _iDesignationGateway.GetDesignationByCode(code);
        }
        public Designation GetById(int designationId)
        {
            return _iDesignationGateway.GetById(designationId);
        }
    }
}