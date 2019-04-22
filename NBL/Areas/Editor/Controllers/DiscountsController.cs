using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class DiscountsController : Controller
    {
        
        private readonly ICommonManager _iCommonManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IProductManager _iProductManager;

        public DiscountsController(ICommonManager iCommonManager,IDiscountManager iDiscountManager,IProductManager iProductManager)
        {
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iProductManager = iProductManager;
        }
     
        public ActionResult AddDiscount()
        {
            ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
            foreach (ClientType clientType in _iCommonManager.GetAllClientType())
            {
                foreach (var product in _iProductManager.GetAllProducts())
                {
                    Discount model = new Discount
                    {
                        UpdateByUserId = 6,
                        DiscountPercent = 6,
                        ClientTypeId = clientType.ClientTypeId,
                        UpdateDate = DateTime.Now,
                        ProductId =product.ProductId,

                    };

                    _iDiscountManager.Add(model);

                }

            }
           
            return View();
        }
        [HttpPost]
        public ActionResult AddDiscount(Discount model)
        {

            if (ModelState.IsValid)
            {
                var anUser = (ViewUser)Session["user"];
                model.UpdateByUserId = anUser.UserId;
                bool result = _iDiscountManager.Add(model);
                ViewData["Message"] = result ? "Discount info Saved Successfully!!" : "Failed to Save!!";
                ModelState.Clear();
            }
            ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
            return View();

        }

    }
}