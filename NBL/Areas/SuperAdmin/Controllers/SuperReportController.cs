using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles ="Super")]
    public class SuperReportController : Controller
    {

        // GET: SuperAdmin/SuperReport
       private readonly  IOrderManager _iOrderManager;

        public SuperReportController(IOrderManager iOrderManager)
        {
            _iOrderManager = iOrderManager;
        }
        public ActionResult SuperSalesReport()
        {
            var sales = _iOrderManager.GetAll().ToList();
            return View(sales);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtmlExcel)
        {
            var fileName = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 8) + ".xls";
            return File(Encoding.ASCII.GetBytes(GridHtmlExcel), "application/vnd.ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult Reoprt(FormCollection collection) 
        {

            string id = collection["ObjectName"];
            Session["DS"] = null;
            string reportName = "";
             if (id == "o")
            {
                reportName = "Report2";
                Session["DS"] = _iOrderManager.GetAll().ToList();

            }
            // string report = id; //or whatever data you need. you can fetch it from any data source
            Response.Redirect(@"~/Areas/SuperAdmin/Reports/ViewReport.aspx?id=" + reportName + "&name=" + id);
            return new EmptyResult();
        }
    }
}