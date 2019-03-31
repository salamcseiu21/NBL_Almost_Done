
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Designations;

namespace NBL.DAL.Contracts
{
    public interface IDesignationGateway:IGateway<Designation>
    {
        Designation GetDesignationByCode(string code);
        List<Designation> GetDesignationsByDepartmentId(int departmentId);


    }
}
