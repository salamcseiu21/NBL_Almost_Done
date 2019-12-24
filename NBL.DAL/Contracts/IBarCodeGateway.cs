using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.EntityModels.Products;
using NBL.Models.Searchs;

namespace NBL.DAL.Contracts
{
    public interface IBarCodeGateway:IGateway<BarCodeModel>
    {
        int GenerateBarCode(BarCodeModel model);
        int GetMaxBarCodeSlByInfix(string prefix,string lineNo);
        ICollection<BarCodeModel> GetAllByProducitonDateCode(string dateCode);
        ICollection<BarCodeModel> GetBarCodesBySearchCriteria(PrintBarCodeModel model); 
        int SaveBarCodes(ViewCreateBarCodeModel model);
        List<PrintBarCodeModel> GetTodaysProductionProductList(DateTime date);
        ICollection<BarCodeModel> GetAllBarCodeByInfix(string infix);
        BarCodeModel GetBarcodeByBatchCode(string batchCode);
        IEnumerable<BarCodeModel> GetBarcodeReportBySearchCriteria(SearchCriteria searchCriteria); 
    }
}
