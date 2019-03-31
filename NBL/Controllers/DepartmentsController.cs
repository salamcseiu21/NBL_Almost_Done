using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Departments;

namespace NBL.Controllers
{
    public class DepartmentsController : ApiController
    {
        private readonly DepartmentGateway _departmentGateway = new DepartmentGateway();
        // GET: api/Departments
        public IEnumerable<Department> Get()
        {
            return _departmentGateway.GetAll().ToList();
        }

        // GET: api/Departments/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Departments
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Departments/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Departments/5
        public void Delete(int id)
        {
        }
    }
}
