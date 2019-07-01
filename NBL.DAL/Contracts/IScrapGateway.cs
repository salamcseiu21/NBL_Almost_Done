using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Scraps;

namespace NBL.DAL.Contracts
{
    public interface IScrapGateway:IGateway<ScrapModel>
    {
        int SaveScrap(ScrapModel model);
        bool IsThisBarcodeExitsInScrapInventory(string barcode);
    }
}
