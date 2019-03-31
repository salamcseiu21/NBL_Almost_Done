using System.Collections.Generic;
using System.Web.Http;
using NBL.DAL;
using NBL.Models.EntityModels.Products;

namespace NBL.Controllers.API
{
    public class ProductsController : ApiController
    {

        readonly  ProductGateway _productGateway=new ProductGateway();
        // GET: api/Products
        public IEnumerable<Product> Get()
        {
            return _productGateway.GetAll();
        }

        // GET: api/Products/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Products
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
        }
    }
}
