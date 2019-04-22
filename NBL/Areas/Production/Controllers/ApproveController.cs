using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "StoreManagerFactory")]
    public class ApproveController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IBranchManager _iBranchManager;
        // GET: Factory/Approve

        public ApproveController(IProductManager iProductManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iBranchManager = iBranchManager;
        }
        public ActionResult PendingTransferIssueList()
        {
            ViewTransferIssueModel model=new ViewTransferIssueModel();
            var issuedProducts = _iProductManager.GetTransferIssueList();
            model.TransferIssues = issuedProducts.ToList();
            foreach (var issue in issuedProducts)
            {
                model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
            }
            return View(model);
        }
        public ActionResult ApproveTransferIssue(int id)
        {
            var issue = _iProductManager.GetTransferIssueById(id);
            var issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            ViewTransferIssueDetailsModel model=new ViewTransferIssueDetailsModel
            {
                FromBranch = _iBranchManager.GetById(issue.FromBranchId),
                ToBranch = _iBranchManager.GetById(issue.ToBranchId),
                TransferIssue = issue,
                TransferIssueDetailses = issueDetails.ToList()
            };
          
            return View(model);
        }
        [HttpPost]
        public ActionResult ApproveTransferIssue(FormCollection collection,int id)
        {
            int transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
            int approveUserId = ((ViewUser)Session["user"]).UserId;
            DateTime approveDateTime = DateTime.Now;
            TransferIssue transferIssue = new TransferIssue
            {
                ApproveByUserId = approveUserId,
                TransferIssueId = transferIssueId,
                ApproveDateTime = approveDateTime
            };
            bool result = _iProductManager.ApproveTransferIssue(transferIssue);
            if (result)
            {
                return RedirectToAction("PendingTransferIssueList");
            }
            ViewBag.Transfer = _iProductManager.GetTransferIssueList().ToList().Find(n => n.TransferIssueId == transferIssueId);
            var issueDetails = _iProductManager.GetTransferIssueDetailsById(transferIssueId);
            return View(issueDetails);
        }

    }
}