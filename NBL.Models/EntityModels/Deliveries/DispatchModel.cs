
using System;
using System.Collections.Generic;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Models.EntityModels.Deliveries
{
    public class DispatchModel
    {
        public long DispatchId { get; set; }
        public long TripId { get; set; }
        public string  TripRef { get; set; }
        public string DispatchRef { get; set; } 
        public string TransactionRef { get; set; }
        public int DispatchByUserId { get; set; }
        public DateTime DispatchDate { get; set; }
        public int Status { get; set; }
        public string IsCanclled { get; set; }
        public DateTime SystemDateTime { get; set; }
        public ViewTripModel TripModel { get; set; }
        public int CompanyId { get; set; }  
        public ICollection<ScannedProduct> ScannedProducts { get; set; }
        public ICollection<ViewDispatchModel> DispatchModels { get; set; } 

    }
}
