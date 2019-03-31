using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Masters;

namespace NBL.BLL
{
    public class EmployeeTypeManager:IEmployeeTypeManager
    {
       private readonly IEmployeeTypeGateway _iEmployeeTypeGateway;

        public EmployeeTypeManager(IEmployeeTypeGateway iEmployeeTypeGateway)
        {
            _iEmployeeTypeGateway = iEmployeeTypeGateway;
        }

        public IEnumerable<EmployeeType> GetAll()
        {
            return _iEmployeeTypeGateway.GetAll();
        }
    }
}