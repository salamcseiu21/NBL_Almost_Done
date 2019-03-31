using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Companies;

namespace NBL.BLL
{
    public class CompanyManager:ICompanyManager
    {
        readonly ICompanyGateway _iCompanyGateway;

        public CompanyManager(ICompanyGateway iCompanyGateway)
        {
            _iCompanyGateway = iCompanyGateway;
        }

        public ICollection<Company> GetAll()
        {
            return _iCompanyGateway.GetAll().ToList();
        }

        public bool Add(Company model)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Company model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Company model)
        {
            throw new System.NotImplementedException();
        }

        public Company GetById(int companyId)
        {
            return _iCompanyGateway.GetById(companyId);
        }
    }
}