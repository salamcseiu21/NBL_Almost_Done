
using System.Web.Mvc;

namespace NBL.Models.ViewModels.Employees
{
    public class CreateEmployeeViewModel
    {
        public SelectList EmployeeTypeId { get; set; }
        public SelectList BranchId { get; set; }
        public SelectList DepartmentId { get; set; }
        public SelectList DesignationId { get; set; }   
    }
}
