using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Models.ViewModels.Productions
{
    public class ViewDispatchChalan
    {
        public IEnumerable<ViewDispatchModel> DispatchDetails { get; set; }
        public DispatchModel DispatchModel { get; set; }
        public string Destination { get; set; }
        public ViewTripModel ViewTripModel { get; set; }
        public ICollection<ViewProduct> Products { get; set; }  
    }
}
