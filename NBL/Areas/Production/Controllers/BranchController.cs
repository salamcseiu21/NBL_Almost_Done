using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;

namespace NBL.Areas.Production.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        // GET: Factory/Branch
       private readonly IBranchManager _iBranchManager;

        public BranchController(IBranchManager iBranchManager)
        {
            _iBranchManager = iBranchManager;
        }
        [HttpPost] 

        public JsonResult BranchAutoComplete(string prefix)
        {
            int corporateBarachIndex = _iBranchManager.GetAllBranches().ToList().FindIndex(n => n.BranchName.Contains("Corporate"));
            var branches = _iBranchManager.GetAllBranches().ToList();
            branches.RemoveAt(corporateBarachIndex);
            var branchList = (from c in branches.ToList()
                where c.BranchName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.BranchName,
                    val = c.BranchId
                }).ToList();

            return Json(branchList);
        }

        public JsonResult GetBranchDetailsById(int branchId)
        {
          var branch= _iBranchManager.GetById(branchId);
          return Json(branch, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return View(branches);

        }

    }
}