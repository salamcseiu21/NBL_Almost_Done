using System;
namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTripDetailsModel
    {
        public long TripItemId { get; set; }
        public long TripId { get; set; }
        public int TripStatus { get; set; }
        public string IsCancelled { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public int CategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int Quantity { get; set; }
        public ViewBranch ToBranch { get; set; }
        public ViewTripModel ViewTripModel { get; set; }    
        public DateTime SystemDateTime { get; set; }

        public ViewTripDetailsModel()
        {
            ToBranch=new ViewBranch();
            ViewTripModel=new ViewTripModel();
        }
    }
}
