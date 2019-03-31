using System;
using System.Collections.Generic;
namespace NBL.Models.ViewModels.TransferProducts
{
    public class ViewCreateTripModel
    {
        public string TripRef { get; set; } 
        public string Remarks { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public int CreatedByUserId { get; set; }
        public int RequisitionStatus { get; set; }
        public int TripStatus { get; set; }
        public ICollection<ViewTripModel> TripModels { get; set; }  
    }
}
