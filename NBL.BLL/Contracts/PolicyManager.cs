using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;

namespace NBL.BLL.Contracts
{
   public class PolicyManager:IPolicyManager
    {


        private  readonly  IPolicyGateway _iPolicyGateway; 
        public PolicyManager(IPolicyGateway iPolicyGateway)
        {
            _iPolicyGateway = iPolicyGateway;
        }
        public bool AddProductWarrentPolicy(WarrantyPolicy model)
        {
            var rowAffected = _iPolicyGateway.AddProductWarrentPolicy(model);
            return rowAffected > 0;
        }
        public bool Add(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public bool Update(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public WarrantyPolicy GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyPolicy> GetAll()
        {
            throw new NotImplementedException();
        }

       
    }
}
