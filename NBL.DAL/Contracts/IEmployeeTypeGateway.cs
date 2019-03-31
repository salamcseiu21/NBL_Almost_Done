using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Masters;

namespace NBL.DAL.Contracts
{
    public interface IEmployeeTypeGateway
    {
        IEnumerable<EmployeeType> GetAll();
    }
}
