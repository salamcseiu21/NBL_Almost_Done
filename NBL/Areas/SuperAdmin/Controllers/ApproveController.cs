using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.ViewModels;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class ApproveController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly DistrictGateway _districtGateway = new DistrictGateway();
        private readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        private readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        private readonly IClientManager _iClientManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;

        public ApproveController(IClientManager iClientManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager)
        {
            _iClientManager = iClientManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iCommonManager = iCommonManager;
        }
        // GET: SuperAdmin/Approve
        public ActionResult ApproveClient() 
        {
            List<Client> clients = _iClientManager.GetPendingClients();
            return View(clients);
        }
        [HttpPost]
        public ActionResult ApproveClient(FormCollection collection)
        {

            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                var anUser = (ViewUser)Session["user"];
                int clientId = Convert.ToInt32(collection["ClientId"]);
                var aClient = _iClientManager.GetById(clientId);
                bool result = _iClientManager.ApproveClient(aClient,anUser);
                aModel.Message = result ? "<p class='text-green'> Client Approved Successfully!</p>" : "<p class='text-danger'> Failed to  Approve Client ! </p>";
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                aModel.Message = " <p style='color:red'>" + message + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _iClientManager.GetById(id);
                ViewBag.TerritoryId = new SelectList(_iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_districtGateway.GetAll(), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId??default(int)), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId??default(int)), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                return View(client);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Msg = e.InnerException.Message;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Edit(int id, Client model, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var  client = _iClientManager.GetById(id);
                client.ClientName = model.ClientName;
                client.Address = model.Address;
                client.PostOfficeId = model.PostOfficeId;
                client.ClientTypeId = model.ClientTypeId;
                client.Phone = model.Phone;
                client.AlternatePhone = model.AlternatePhone;
                client.Gender = model.Gender;
                client.Fax = model.Fax;
                client.Website = model.Website;
                client.Email = model.Email;
                client.CreditLimit = Convert.ToDecimal(model.CreditLimit);
                client.UserId = user.UserId;
                client.NationalIdNo = model.NationalIdNo;
                client.TinNo = model.TinNo;
                client.TerritoryId = Convert.ToInt32(model.TerritoryId);
                client.RegionId = Convert.ToInt32(model.RegionId);
                client.Active = "Y";
                client.ClientId = id;

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
              
                bool result = _iClientManager.Update(client);
                return RedirectToAction("ApproveClient");
            }
            catch
            {
                return View();
            }
        }
    }
}