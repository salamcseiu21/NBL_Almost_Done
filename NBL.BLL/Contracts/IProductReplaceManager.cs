using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels;

namespace NBL.BLL.Contracts
{
    public interface IProductReplaceManager:IManager<ReplaceModel>
    {
        bool SaveReplacementInfo(ReplaceModel model);
    }
}
