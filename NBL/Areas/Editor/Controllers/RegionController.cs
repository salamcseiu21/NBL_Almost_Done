using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Locations;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class RegionController : Controller
    {
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        // GET: Editor/Region
        public RegionController(IRegionManager iRegionManager,IDivisionGateway iDivisionGateway)
        {
            _iRegionManager = iRegionManager;
            _iDivisionGateway = iDivisionGateway;
        }
        public ActionResult All()
        {
            var regions = _iRegionManager.GetAll();
            return View(regions);
        } 
        public ActionResult AddNewRegion()
        {
           ViewBag.DivisionId = new SelectList(_iDivisionGateway.GetAll(), "DivisionId", "DivisionName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewRegion(Region model)
        {
           // ViewBag.DivisionId = new SelectList(_iDivisionGateway.GetAll(), "DivisionId", "DivisionName");

            try
            {
                if (ModelState.IsValid)
                {
                    bool rowAffected = _iRegionManager.Add(model);
                    if (rowAffected)
                    {
                        ModelState.Clear();
                        return RedirectToAction("All");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return View();
            }
        }
       
    }
}