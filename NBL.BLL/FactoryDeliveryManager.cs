using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using Microsoft.Ajax.Utilities;
using ReferenceType = NBL.Models.Enums.ReferenceType;

namespace NBL.BLL
{
    public class FactoryDeliveryManager:IFactoryDeliveryManager
    {
        private readonly IFactoryDeliveryGateway _iFactoryDeliveryGateway;
        private readonly InventoryGateway _inventoryGateway = new InventoryGateway();
        private readonly CommonGateway _commonGateway = new CommonGateway();
        private readonly IBranchGateway _iBranchGateway;
        private readonly ICommonGateway _iCommonGateway;
        public FactoryDeliveryManager(IFactoryDeliveryGateway iFactoryDeliveryGateway,IBranchGateway iBranchGateway,ICommonGateway iCommonGateway)
        {
            _iFactoryDeliveryGateway = iFactoryDeliveryGateway;
            _iBranchGateway = iBranchGateway;
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

        public DispatchModel GetDispatchByDispatchId(long dispatchId)
        {
            return _iFactoryDeliveryGateway.GetDispatchByDispatchId(dispatchId);
        }

        public ViewDispatchChalan GetDispatchChalanByDispatchId(long dispatchId)
        {
            var destination = "";
            DispatchModel dispatch = GetDispatchByDispatchId(dispatchId);
            var viewTrip = _inventoryGateway.GetAllTrip().ToList().Find(n => n.TripId == dispatch.TripId);
            var details = GetDispatchDetailsByDispatchId(dispatchId);
            foreach (var model in details.ToList().OrderByDescending(n => n.ToBranchId).DistinctBy(n => n.ToBranchId))
            {
                var b = _iBranchGateway.GetById(model.ToBranchId);
                destination += b.BranchName+"-"+b.BranchAddress +",";
            }


            destination=destination.TrimEnd(',');
            var chalan = new ViewDispatchChalan
            {
                DispatchModel = dispatch,
                DispatchDetails = details,
                Destination = destination,
                ViewTripModel = viewTrip
            };
            return chalan;


        }

        public ICollection<ViewDispatchModel> GetDispatchDetailsByDispatchId(long dispatchId)
        {
            return _iFactoryDeliveryGateway.GetDispatchDetailsByDispatchId(dispatchId);
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



        public bool SaveDeliveredGeneralRequisition(List<ScannedProduct> scannedProducts, Delivery aDelivery)
        {
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Distribution)).Code;
            aDelivery.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);
            int maxRefNo = _inventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iFactoryDeliveryGateway.SaveDeliveredGeneralRequisition(scannedProducts, aDelivery);
            return rowAffected > 0;
        }

        private string GenerateDeliveryReference(int maxRefNo)
        {
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(Convert.ToInt32(ReferenceType.Distribution))).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }

        private long GetMaxVoucherNoByTransactionInfix(string infix)
        {
            var temp = _inventoryGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
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