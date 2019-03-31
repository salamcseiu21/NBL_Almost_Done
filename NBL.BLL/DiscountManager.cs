using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;

using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.BLL
{
    public class DiscountManager:IDiscountManager
    {
       private readonly IDiscountGateway _iDiscountGateway;

        public DiscountManager(IDiscountGateway iDiscountGateway)
        {
            _iDiscountGateway = iDiscountGateway;
        }

        public IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId) 
        {
            return _iDiscountGateway.GetAllDiscountsByClientTypeId(clientTypeId);
        }
        public IEnumerable<Discount> GetAllPendingDiscounts()
        {
            return _iDiscountGateway.GetAllPendingDiscounts();
        }

        public bool Add(Discount model)
        {
            return _iDiscountGateway.Add(model) > 0;
        }

        public bool Update(Discount model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Discount model)
        {
            throw new System.NotImplementedException();
        }

        public Discount GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Discount> GetAll()
        {
            return _iDiscountGateway.GetAll().ToList();
        }
    }
}