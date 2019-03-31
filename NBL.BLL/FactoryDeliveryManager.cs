using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.Enums;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL
{
    public class FactoryDeliveryManager:IFactoryDeliveryManager
    {
        private readonly IFactoryDeliveryGateway _iFactoryDeliveryGateway;
        readonly InventoryGateway _inventoryGateway = new InventoryGateway();
        readonly CommonGateway _commonGateway = new CommonGateway();

        public FactoryDeliveryManager(IFactoryDeliveryGateway iFactoryDeliveryGateway)
        {
            _iFactoryDeliveryGateway = iFactoryDeliveryGateway;
        }
        public string SaveDispatchInformation(DispatchModel dispatchModel) 
        {
            int maxDispatchNo = _inventoryGateway.GetMaxDispatchRefNoOfCurrentYear();
            dispatchModel.DispatchRef = GenerateDispatchReference(maxDispatchNo); 
            int rowAffected = _iFactoryDeliveryGateway.SaveDispatchInformation(dispatchModel);
            if (rowAffected > 0)
                return "Saved Successfully!";
            return "Failed to Save";
        }
        /// <summary>
        /// id=4 stands dispatch from factory......
        /// </summary>
        /// <param name="maxDispatchNo"></param>
        /// <returns></returns>

        private string GenerateDispatchReference(int maxDispatchNo)
        {
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Dispatch)).Code;
            string temp = (maxDispatchNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }
        public bool Add(Delivery model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Delivery model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Delivery model)
        {
            throw new NotImplementedException();
        }

        public Delivery GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}