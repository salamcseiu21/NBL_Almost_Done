using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.BLL.Contracts
{
    public interface IBarCodeManager:IManager<BarCodeModel>
    {
        bool GenerateBarCode(BarCodeModel model);
        int GetMaxBarCodeSlByInfix(string prefix);
        ICollection<BarCodeModel> GetAllByProducitonDateCode(string dateCode);
        ICollection<BarCodeModel> GetBarCodesBySearchCriteria(PrintBarCodeModel model);
        bool SaveBarCodes(ViewCreateBarCodeModel model);
        List<PrintBarCodeModel> GetTodaysProductionProductList();
    }
}
