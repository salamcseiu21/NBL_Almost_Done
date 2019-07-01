using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Scraps;

namespace NBL.BLL.Contracts
{
    public interface IScrapManager:IManager<ScrapModel>
    {
        bool SaveScrap(ScrapModel model);
        bool IsThisBarcodeExitsInScrapInventory(string barcode);
    }
}
