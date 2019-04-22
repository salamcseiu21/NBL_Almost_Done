
using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class TerritoryController : Controller
    {

       private readonly ITerritoryManager _iTerritoryManager;
       private readonly IRegionManager _iRegionManager;

        public TerritoryController(IRegionManager iRegionManager,ITerritoryManager iTerritoryManager)
        {
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
        }
        // GET: Editor/Territory
        public ActionResult All()
        {
            var territories = _iTerritoryManager.GetAll();
            return View(territories);
        }

        public ActionResult AddNewTerritory()
        {
            ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewTerritory(Territory model)
        {
            ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (ViewUser)Session["user"];
                    model.AddedByUserId = user.UserId;
                    var result = _iTerritoryManager.Add(model);
                    if (result)
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