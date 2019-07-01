
using System.Collections.Generic;
using NBL.Models.EntityModels.Scraps;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.Scraps
{
    public class ViewCreateScrapModel
    {
        public ScrapModel ScrapModel { get; set; }
        public ICollection<ScannedProduct> ScannedProducts { get; set; }

    }
}
