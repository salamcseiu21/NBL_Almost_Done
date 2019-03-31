using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;

namespace NBL.Models.ViewModels
{
    public class ViewChalanModel
    {
        public IEnumerable<DeliveryDetails> DeliveryDetailses { get; set; }
        public ViewClient ViewClient { get; set; }  

    }
}
