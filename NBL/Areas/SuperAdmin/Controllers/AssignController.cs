using NBL.Areas.SuperAdmin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class AssignController : Controller
    {
      
        private readonly IBranchManager _iBranchManager;
        private readonly UserManager _userManager = new UserManager();
        private readonly ICommonManager _iCommonManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly SuperAdminUserManager _superAdminUserManager = new SuperAdminUserManager();

        public AssignController(IBranchManager iBranchManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager)
        {
            _iBranchManager = iBranchManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
        }
        [HttpGet]
        public ActionResult AssignRegionToBranch()
        {

            var branches = _iBranchManager.GetAllBranches();
            ViewBag.AssignedRegions=_iBranchManager.GetAssignedRegionToBranchList();
            return View(branches);
        }

        
        [HttpPost]
        public ActionResult AssignRegionToBranch(FormCollection collection)
        {
            var user = (ViewUser)Session["user"];
            Branch branch = new Branch {BranchId = Convert.ToInt32(collection["BranchId"])};

            List<Region> regionList = new List<Region>();
            string[] tokens = collection["RegionId"].Split(',');
            for (int i = 0; i < tokens.Length; i++) 
            {
                regionList.Add(new Region { RegionId = Convert.ToInt32(tokens[i]) });
            }
            branch.RegionList = regionList;
            int rowAffected = _iRegionManager.AssignRegionToBranch(branch, user);
            var branches = _iBranchManager.GetAllBranches();
            ViewBag.AssignedRegions = _iBranchManager.GetAssignedRegionToBranchList();
            return View(branches);
        }


        [HttpGet]
        public ActionResult AssignDistrictToRegion()
        {

            var regions = _iRegionManager.GetAll();
            return View(regions);
        }
        [HttpPost]
        public ActionResult AssignDistrictToRegion(FormCollection collection)
        {
            Region aRegion = new Region {RegionId = Convert.ToInt32(collection["RegionId"])};

            List<District> districtList = new List<District>();
            string[] tokens = collection["DistrictId"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                districtList.Add(new District { DistrictId = Convert.ToInt32(tokens[i]) });
            }
            aRegion.Districts = districtList;
            int rowAffected = _iRegionManager.AssignDristrictToRegion(aRegion);
            var regions = _iRegionManager.GetAll();
            return View(regions);
        }


        [HttpGet]
        public ActionResult AssignUpazillaToTerritory()
        {

            var territories = _iTerritoryManager.GetAll();
            return View(territories);
        }
        [HttpPost]
        public ActionResult AssignUpazillaToTerritory(FormCollection collection)
        {
            Territory aTerritory = new Territory
            {
                TerritoryId = Convert.ToInt32(collection["TerritoryId"])
            };

            List<Upazilla> upazillaLsit = new List<Upazilla>();
            string[] tokens = collection["UpazillaId"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                upazillaLsit.Add(new Upazilla { UpazillaId = Convert.ToInt32(tokens[i]) });
            }
            aTerritory.UpazillaList = upazillaLsit;
            int rowAffected = _iTerritoryManager.AssignUpazillaToTerritory(aTerritory);
            if (rowAffected > 0)
            {
                
                return RedirectToAction("ViewTerritory", "Home", new { area = "SuperAdmin" });
            }
            var territories = _iTerritoryManager.GetAll();
            return View(territories);
        }

        [HttpGet]
        public ActionResult AssignBranchToUser()
        {
            var branches = _iBranchManager.GetAllBranches();
            return View(branches);
        }

        [HttpPost]
        public ActionResult AssignBranchToUser(FormCollection collection)
        {
            User anUser = new User {UserId = Convert.ToInt32(collection["UserId"])};
            List<Branch> branchList = new List<Branch>();
            string[] tokens = collection["SelectedBranch"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                branchList.Add(new Branch { BranchId = Convert.ToInt32(tokens[i]) });
            }
            TempData["Message"] = _superAdminUserManager.AssignBranchToUser(anUser, branchList);
            var branches = _iBranchManager.GetAllBranches();
            var roles = _iCommonManager.GetAllUserRoles().ToList();
            ViewBag.Roles = roles;
            return View(branches);
        }
        public ActionResult ViewUser()
        {
            var users = _userManager.GetAll.ToList();
            return View(users);

        }


        public ActionResult UnAssignDistrict()
        {

            return View(_iRegionManager.GetRegionListWithDistrictInfo());
        }
        [HttpPost]
        public JsonResult UnAssignDistrict(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int regionDetailsId = Convert.ToInt32(collection["RegionDetailsId"]);
                var regionDetails= _iRegionManager.GetRegionDetailsById(regionDetailsId);
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                int result = _iRegionManager.UnAssignDistrictFromRegion(regionDetails, reason, anUser);
                aModel.Message = result > 0 ? "<p style='color:green'>UnAssigned Successfull! </p>" : "<p style='color:red'>Failed to Un Assign</p>";
            }
            catch (Exception e)
            {
                aModel.Message = "<p style='color:red'>" + e.InnerException?.Message + "</p>";
            }
           
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UnAssignUpazilla() 
        {

            return View();
        }
        [HttpPost]
        public JsonResult UnAssignUpazilla(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int territoryDetailsId = Convert.ToInt32(collection["TerritoryDetailsId"]); 
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                int result = _iTerritoryManager.UnAssignUpazillaFromTerritory(territoryDetailsId, reason, anUser);
                aModel.Message = result > 0 ? "<p style='color:green'>UnAssigned Successfull! </p>" : "<p style='color:red'>Failed to Un Assign</p>";
            }
            catch (Exception e)
            {
                aModel.Message = "<p style='color:red'>" + e.InnerException?.Message + "</p>";
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
    }
}