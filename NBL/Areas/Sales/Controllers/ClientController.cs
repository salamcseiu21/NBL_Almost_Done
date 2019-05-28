
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Logs;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "SalesExecutive")]
    public class ClientController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly DistrictGateway _districtGateway = new DistrictGateway();
        private readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        private readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        private readonly IClientManager _iClientManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryGateway _iTerritoryGateway;
        // GET: Sales/Client
        public ClientController(IClientManager iClientManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryGateway iTerritoryGateway)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryGateway = iTerritoryGateway;
        }
        public ActionResult All()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                return View(_iClientManager.GetClientByBranchId(branchId).ToList().FindAll(n => n.Active == "Y"));
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        // GET: Sales/Client/Profile/5



        // GET: Sales/Client/AddNewClient
        public ActionResult AddNewClient()
        {
            try
            {
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        // POST: Sales/Client/AddNewClient
        [HttpPost]
        public ActionResult AddNewClient(FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {

                var user = (ViewUser)Session["user"];
                Client client = new Client
                {
                    ClientName = collection["ClientName"],
                    Address = collection["Address"],
                    PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]),
                    ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]),
                    Phone = collection["phone"],
                    AlternatePhone = collection["AlternatePhone"],
                    Gender = collection["Gender"],
                    Fax = collection["Fax"],
                    Email = collection["Email"],
                    Website = collection["Website"],
                    UserId = user.UserId,
                    BranchId = branchId,
                    NationalIdNo = collection["NationalIdNo"],
                    TinNo = collection["TinNo"],
                    TerritoryId = Convert.ToInt32(collection["TerritoryId"]),
                    RegionId = Convert.ToInt32(collection["RegionId"]),
                    CompanyId = companyId

                };
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Photos"), image);
                    // file is uploaded
                    file.SaveAs(path);
                    client.ClientImage = "Images/Client/Photos/" + image;
                }
                else
                {
                    client.ClientImage = "";
                }
                if (ClientSignature != null)
                {
                    string ext = Path.GetExtension(ClientSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Signatures"), sign);
                    // file is uploaded
                    ClientSignature.SaveAs(path);
                    client.ClientSignature = "Images/Client/Signatures/" + sign;
                }
                else
                {
                    client.ClientSignature = "";
                }
                bool result = _iClientManager.Add(client);
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
                ViewBag.Message = result;
                return View();

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        // GET: Sales/Client/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _iClientManager.GetById(id);
                ViewBag.Territories = _iTerritoryGateway.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId).ToList();
                ViewBag.Districts = _districtGateway.GetAllDistrictByDivistionId(client.DivisionId??default(int));
                ViewBag.Upazillas = _upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId??default(int));
                ViewBag.PostOffices = _postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId??default(int));
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                return View(client);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }

        }

        // POST: Sales/Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                Client client = _iClientManager.GetById(id);
                client.ClientName = collection["ClientName"];
                client.Address = collection["Address"];
                client.PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]);
                client.ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]);
                client.Phone = collection["phone"];
                client.AlternatePhone = collection["AlternatePhone"];
                client.Gender = collection["Gender"];
                client.Fax = collection["Fax"];
                client.Email = collection["Email"];
                client.Website = collection["Website"];
                client.UserId = user.UserId;
                client.NationalIdNo = collection["NationalIdNo"];
                client.TinNo = collection["TinNo"];
                client.TerritoryId = Convert.ToInt32(collection["TerritoryId"]);
                client.RegionId = Convert.ToInt32(collection["RegionId"]);

                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string image = "cp" + Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Photos"), image);
                    // file is uploaded
                    file.SaveAs(path);
                    client.ClientImage = "Images/Client/Photos/" + image;
                }
                if (ClientSignature != null)
                {
                    string ext = Path.GetExtension(ClientSignature.FileName);
                    string sign = "cs" + Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Signatures"), sign);
                    // file is uploaded
                    ClientSignature.SaveAs(path);
                    client.ClientSignature = "Images/Client/Signatures/" + sign;
                }
                client.ClientId = id;
                bool result = _iClientManager.Update(client);
                return RedirectToAction("All");
            }
            catch(Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public JsonResult ClientEmailExists(string email)
        {

            Client client = _iClientManager.GetClientByEmailAddress(email);
            if (client.Email != null)
            {
                client.EmailInUse = true;
            }
            else
            {
                client.EmailInUse = false;
                client.Email = email;
            }
            return Json(client);
        }
       
    }
}