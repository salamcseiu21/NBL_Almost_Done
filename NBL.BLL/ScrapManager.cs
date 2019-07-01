using System;
using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Scraps;

namespace NBL.BLL
{
    public class ScrapManager : IScrapManager
    {

        private readonly IScrapGateway _iScrapGateway;

       
        public ScrapManager(IScrapGateway iScrapGateway)
        {
            _iScrapGateway = iScrapGateway;
        }
        public bool SaveScrap(ScrapModel model)
        {

            return _iScrapGateway.SaveScrap(model) > 0;
        }

        public bool IsThisBarcodeExitsInScrapInventory(string barcode)
        {

            return _iScrapGateway.IsThisBarcodeExitsInScrapInventory(barcode);
        }


        public bool Add(ScrapModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ScrapModel model)
        {
            throw new NotImplementedException();
        }

        public ICollection<ScrapModel> GetAll()
        {
            throw new NotImplementedException();
        }

        

        public ScrapModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(ScrapModel model)
        {
            throw new NotImplementedException();
        }
    }
}
