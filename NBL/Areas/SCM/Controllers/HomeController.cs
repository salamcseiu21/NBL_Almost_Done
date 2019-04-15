using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models.EntityModels.Securities;
using NBL.Models.EntityModels.Identities;

namespace NBL.Areas.SCM.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();
        // GET: SCM/Home
        public ActionResult Home() 
        {
            return View();
        }

        //------------------ Change password------------------------
        public PartialViewResult ChangePassword(int id)
        {

            var user = _userManager.GetUserInformationByUserId(id);
            user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return PartialView("_ChangePasswordPartialPage", user);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
            model.Password = StringCipher.Encrypt(model.Password, "salam_cse_10_R");
            bool result = _userManager.UpdatePassword(model);
            if (result)
                return RedirectToAction("Home");
            return RedirectToAction("ChangePassword");
        }
    }
}