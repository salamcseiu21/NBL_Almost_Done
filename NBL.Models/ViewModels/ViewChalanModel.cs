using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.ViewModels.Replaces;

namespace NBL.Models.ViewModels
{
    public class ViewChalanModel
    {
        public IEnumerable<DeliveryDetails> DeliveryDetailses { get; set; }
        public ViewClient ViewClient { get; set; }
        public Delivery DeliveryInfo { get; set; }
        public ICollection<ViewReplaceDetailsModel> ReplaceDetailsModels { get; set; }

    }
}
