
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Masters;

namespace NBL.BLL.Contracts
{
    public interface IEmployeeTypeManager
    {
        IEnumerable<EmployeeType> GetAll();
        bool Add(EmployeeType model); 
    }
}
