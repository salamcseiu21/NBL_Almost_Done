
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Deliveries;

namespace NBL.Models.EntityModels.Deliveries
{
    public class ReceiveProductViewModel 
    {
        public long Id { get; set; }
        public long DeliveryId { get; set; }    
        public TransactionModel TransactionModel { get; set; } 
        public ICollection<ViewDispatchModel> DispatchModels { get; set; }  

        public long TripId { get; set; }
        public string TripRef { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
        public int ToBranchId { get; set; }
        public string Remarks { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public int CreatedByUserId { get; set; }
        public int DispatchByUserId { get; set; }
        public long DispatchId { get; set; }
        public string DispatchRef { get; set; } 
        public long DispatchItemsId { get; set; }
        public DateTime SystemDateTime { get; set; }    

    }
}
