using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NBL.Models;
using NBL.Models.EntityModels.Employees;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IEmployeeManager:IManager<Employee>
    {
        IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo();
        IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId);
        ViewEmployee GetEmployeeById(int empId);
        int GetEmployeeMaxSerialNo();
        bool IsEmailAddressUnique(string email);
        bool CheckEmail(string email);
       
        IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId);
        ICollection<object> GetEmployeeListBySearchTerm(string searchTerm);
        ICollection<object> GetEmployeeListByDepartmentAndSearchTerm(int departmentId,string searchTerm,int branchId);
        bool UpdateEducationalInfo(EducationalInfo model);
        List<EducationalInfo> GetEducationalInfoByEmpId(int employeeId);
        bool TransferEmployee(int empId, int fromBranchId, int toBranchId, string remarks, ViewUser user); 
    }
}
