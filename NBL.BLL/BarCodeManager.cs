using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.BLL
{
    public class BarCodeManager:IBarCodeManager
    {

        readonly IBarCodeGateway _iBarCodeGateway;
        public BarCodeManager(IBarCodeGateway iBarCodeGateway)
        {
            _iBarCodeGateway = iBarCodeGateway;
        }

        public bool Add(BarCodeModel model)
        {
           int rowAffected= _iBarCodeGateway.Add(model);
            return rowAffected > 0;
        }

        public bool Update(BarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(BarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public BarCodeModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<BarCodeModel> GetAll()
        {
            return _iBarCodeGateway.GetAll();
        }

        public bool GenerateBarCode(BarCodeModel model)
        {

            return _iBarCodeGateway.GenerateBarCode(model) > 0;
        }

        public int GetMaxBarCodeSlByInfixAndByLineNo(string prefix,string lineNo)
        {
            return _iBarCodeGateway.GetMaxBarCodeSlByInfix(prefix,lineNo);
        }

       public ICollection<BarCodeModel> GetAllByProducitonDateCode(string dateCode)
        {
            return _iBarCodeGateway.GetAllByProducitonDateCode(dateCode);
        }

        public ICollection<BarCodeModel> GetBarCodesBySearchCriteria(PrintBarCodeModel model)
        {
            return _iBarCodeGateway.GetBarCodesBySearchCriteria(model);
        }

        public bool SaveBarCodes(ViewCreateBarCodeModel model)
        {
            int rowAffected = _iBarCodeGateway.SaveBarCodes(model);
            return rowAffected > 0;
        }

        public List<PrintBarCodeModel> GetTodaysProductionProductList(DateTime date)
        {
            return _iBarCodeGateway.GetTodaysProductionProductList(date);
        }

        public ICollection<BarCodeModel> GetAllBarCodeByInfix(string infix)
        {
            return _iBarCodeGateway.GetAllBarCodeByInfix(infix);
        }
    }
}
