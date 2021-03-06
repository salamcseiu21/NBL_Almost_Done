﻿using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class VatsController : Controller
    {
        private readonly IVatManager _iVatManager;
        private readonly IProductManager _iProductManager;

        public VatsController(IVatManager iVatManager,IProductManager iProductManager)
        {
            _iVatManager = iVatManager;
            _iProductManager = iProductManager;
        }
      
        [HttpGet]
        public ActionResult AddVat()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddVat(Vat model)
        {
            if (ModelState.IsValid)
            {
                var user = (ViewUser) Session["user"];
                model.UpdateByUserId = user.UserId;
                if (_iVatManager.Add(model))
                {
                    ModelState.Clear();
                    ViewData["Message"] = "Vat info Saved successfully..!";
                }
            }
            return View();
        }

    }
}