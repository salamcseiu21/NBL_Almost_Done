using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class ProductController : Controller
    {

        private readonly ICommonManager _iCommonManager;
        private readonly ICompanyManager _iCompanyManager;
        private readonly IProductManager _iProductManager;


        public ProductController(ICompanyManager iCompanyManager,ICommonManager iCommonManager,IProductManager iProductManager)
        {
            _iCompanyManager = iCompanyManager;
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
        }
        // GET: Editor/Product
        public ActionResult AddProduct() 
        {
            var categories = _iCommonManager.GetAllProductCategory().ToList();
            var types = _iCommonManager.GetAllProductType().ToList();
            var companies = _iCompanyManager.GetAll().ToList();
            ViewBag.Types = types;
            ViewBag.Companies = companies;
            return View(categories);
        }
        [HttpPost]
        public ActionResult AddProduct(FormCollection collection, HttpPostedFileBase ProductImage)
        {
            try
            {

                Product aProduct = new Product
                {
                    CategoryId = Convert.ToInt32(collection["ProductCategoryId"]),
                    CompanyId = Convert.ToInt32(collection["CompanyId"]),
                    ProductTypeId = Convert.ToInt32(collection["ProductTypeId"]),
                    ProductName = collection["ProductName"],
                    Unit = collection["Unit"],
                    ProductAddedDate = Convert.ToDateTime(collection["ProductAddedDate"])
                };
                if (ProductImage != null)
                {
                    string ext = Path.GetExtension(ProductImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower() + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Areas/Editor/Images/Products"), image);
                    // file is uploaded
                    ProductImage.SaveAs(path);
                    aProduct.ProductImage = image;
                }

                string result = _iProductManager.Save(aProduct);
                TempData["Message"] = result;
                var categories = _iCommonManager.GetAllProductCategory().ToList();
                var types = _iCommonManager.GetAllProductType().ToList();
                var companies = _iCompanyManager.GetAll().ToList();
                ViewBag.Types = types;
                ViewBag.Companies = companies;
                return View(categories);
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                var categories = _iCommonManager.GetAllProductCategory().ToList();
                var types = _iCommonManager.GetAllProductType().ToList();
                var companies = _iCompanyManager.GetAll().ToList();
                ViewBag.Types = types;
                ViewBag.Companies = companies;
                return View(categories);
            }
        }

        public ActionResult AddProductDetails()
        {
            

            ViewBag.ProductId = new SelectList(_iProductManager.GetAllProducts(), "ProductId", "ProductName");
            return View();
        }
        [HttpPost]
        public ActionResult AddProductDetails(ViewCreateProductDetailsModel model)
        {
            try
            {
                var user = (ViewUser) Session["user"];
                model.UpdatedByUserId = user.UserId;
                bool result = _iProductManager.SaveProductDetails(model);
                if (result)
                {
                    TempData["Result"] = "Save Successfully!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["Result"] = "Failed to  Save";
                }
                ViewBag.ProductId = new SelectList(_iProductManager.GetAllProducts(), "ProductId", "ProductName");
                return View();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}