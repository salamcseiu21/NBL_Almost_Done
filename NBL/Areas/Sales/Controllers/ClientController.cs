
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Locations;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Clients;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly DistrictGateway _districtGateway = new DistrictGateway();
        private readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        private readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        private readonly IClientManager _iClientManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryGateway _iTerritoryGateway;
        private readonly IDistrictManager _iDistrictManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IAccountsManager _iAccountsManager;
        // GET: Sales/Client
        public ClientController(IClientManager iClientManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryGateway iTerritoryGateway,IDistrictManager iDistrictManager,IBranchManager iBranchManager,IAccountsManager iAccountsManager)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryGateway = iTerritoryGateway;
            _iDistrictManager = iDistrictManager;
            _iBranchManager = iBranchManager;
            _iAccountsManager = iAccountsManager;
        }
        public ActionResult All()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ICollection<ViewClient> clients = _iClientManager.GetClientByOrderCountAndBranchId(branchId);
             
                return View(clients);
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        // GET: Sales/Client/Profile/5

        [Authorize(Roles = "SalesExecutive")]

        // GET: Sales/Client/AddNewClient
        public ActionResult AddNewClient()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ViewBag.BranchId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName",branchId);
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAll().ToList(), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
                ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
                ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
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
        public ActionResult AddNewClient(ViewCreateClientModel model, HttpPostedFileBase ClientImage, HttpPostedFileBase clientSignature)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {
                var user = (ViewUser)Session["user"];
                var client = Mapper.Map<ViewCreateClientModel, Client>(model);
                client.UserId = user.UserId;
                if (ClientImage != null)
                {


                    var ext = Path.GetExtension(ClientImage.FileName)?.ToLower();
                    if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
                    {
                        string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;

                        string path = Path.Combine(
                            Server.MapPath("~/Images/Client/Photos"), image);
                        // file is uploaded
                        ClientImage.SaveAs(path);
                        client.ClientImage = "Images/Client/Photos/" + image;
                    }

                }

                if (clientSignature != null)
                {
                    string ext = Path.GetExtension(clientSignature.FileName)?.ToLower();
                    if (ext == ".jpeg" || ext == ".jpg" || ext == ".png")
                    {
                        string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                        string path = Path.Combine(
                            Server.MapPath("~/Images/Client/Signatures"), sign);
                        // file is uploaded
                        clientSignature.SaveAs(path);
                        client.ClientSignature = "Images/Client/Signatures/" + sign;
                    }

                }
                bool result = _iClientManager.Add(client);
                if (result)
                {
                    ViewBag.Message = "Saved Successfully!";
                }

                ViewBag.BranchId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName",branchId);
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAll().ToList(), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
                ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
                ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
                return View();

            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
                return View();
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
        public ActionResult ViewClientProfile(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
    }
}