using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.BLL.Contracts
{
    public interface IBarCodeManager:IManager<BarCodeModel>
    {
        bool GenerateBarCode(BarCodeModel model);
        int GetMaxBarCodeSlByInfixAndByLineNo(string prefix,string lineNo);
        ICollection<BarCodeModel> GetAllByProducitonDateCode(string dateCode);
        ICollection<BarCodeModel> GetBarCodesBySearchCriteria(PrintBarCodeModel model);
        bool SaveBarCodes(ViewCreateBarCodeModel model);
        List<PrintBarCodeModel> GetTodaysProductionProductList(DateTime date);
        ICollection<BarCodeModel> GetAllBarCodeByInfix(string infix); 

    }
}
