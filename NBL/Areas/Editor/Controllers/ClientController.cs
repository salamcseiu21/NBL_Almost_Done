using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Clients;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class ClientController : Controller
    {
        
        private readonly IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDistrictManager _iDistrictManager;
        private readonly IUpazillaGateway _iUpazillaGateway;
        private readonly IPostOfficeGateway _iPostOfficeGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;

        public ClientController(IClientManager iClientManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IDistrictManager iDistrictManager,IUpazillaGateway iUpazillaGateway,IPostOfficeGateway iPostOfficeGateway)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iDistrictManager = iDistrictManager;
            _iUpazillaGateway = iUpazillaGateway;
            _iPostOfficeGateway = iPostOfficeGateway;
        }

        public ActionResult Details(int id)
        {
            ViewClient client = _iClientManager.GetClientDeailsById(id);
            return View(client);

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
                throw;
            }

        }

        // GET: Sales/Client/AddNewClient
        public ActionResult AddNewClient()
        {
            ViewBag.RegionId= new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
            ViewBag.ClientTypeId=new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
            ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAll().ToList(), "DistrictId", "DistrictName");
            ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
            ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
            ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
            return View();

        }

        // POST: Sales/Client/AddNewClient
        [HttpPost]
        public ActionResult AddNewClient(ViewCreateClientModel model, HttpPostedFileBase ClientImage, HttpPostedFileBase clientSignature)
        {
            var branch = _iRegionManager.GetBranchInformationByRegionId(Convert.ToInt32(model.RegionId));
            try
            {
                var user = (ViewUser)Session["user"];
                var client = Mapper.Map<ViewCreateClientModel, Client>(model);
                client.UserId = user.UserId;
                client.BranchId = branch.BranchId;
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
                
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAll(), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
                ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
                ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
                return View();

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
                return View();
            }
        }

        public ActionResult UploadClientDocument()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadClientDocument(ClientAttachment model,HttpPostedFileBase document) 
        {
             
            if (document != null)
            {
                string fileExtension = Path.GetExtension(document.FileName);
                string doc = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + fileExtension;
                string path = Path.Combine(
                    Server.MapPath("~/ClientDocuments"), doc);
                // file is uploaded
                document.SaveAs(path);
                var anUser = (ViewUser)Session["User"];
                model.UploadedByUserId = anUser.UserId;
                model.FilePath = "ClientDocuments/" + doc;
                model.FileExtension = fileExtension;
                bool result = _iClientManager.UploadClientDocument(model);
                if (result)
                {
                    ViewBag.Message = "<p style='coler:green'> Uploaded Successfully!</p>";
                }
            }
            else
            {
                ViewBag.Message = "<p style='coler:red'> File upload failed</p>";
            }

            return View();
        }

        public ActionResult ViewClientDocuments()
        {
            IEnumerable<ClientAttachment> attachments = _iClientManager.GetClientAttachments();
            return View(attachments);
        }

        
        // GET: Sales/Client/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _iClientManager.GetById(id);
                ViewBag.TerritoryId = new SelectList(_iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAllDistrictByDivistionId(client.DivisionId?? default(int)),"DistrictId","DistrictName");
                ViewBag.UpazillaId = new SelectList(_iUpazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId?? default(int)), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_iPostOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId??default(int)), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId=new SelectList(_iRegionManager.GetAll(),"RegionId","RegionName");
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

        // POST: Sales/Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                Client client = _iClientManager.GetById(id);
                client.ClientName = collection["ClientName"];
                client.CommercialName= collection["CommercialName"];
                client.Address = collection["Address"];
                client.PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]);
                client.ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]);
                client.Phone = collection["phone"];
                client.AlternatePhone = collection["AlternatePhone"];
                client.Gender = collection["Gender"];
                client.Email = collection["Email"];
                client.CreditLimit = Convert.ToDecimal(collection["CreditLimit"]);
                client.UserId = user.UserId;
                client.NationalIdNo = collection["NationalIdNo"];
                client.TinNo = collection["TinNo"];
                client.TerritoryId = Convert.ToInt32(collection["TerritoryId"]);
                client.RegionId = Convert.ToInt32(collection["RegionId"]);
                Branch aBranch= _iRegionManager.GetBranchInformationByRegionId(Convert.ToInt32(collection["RegionId"]));
                client.Branch = aBranch;


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
                return RedirectToAction("ViewClient","Home");
            }
            catch(Exception exception)
            {
                Client client = _iClientManager.GetById(id);
                ViewBag.TerritoryId = new SelectList(_iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAllDistrictByDivistionId(client.DivisionId??default(int)), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(_iUpazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId??default(int)), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_iPostOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId??default(int)), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                ViewBag.ErrorMessage = exception.InnerException?.Message;
                return View(client);
              
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