﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using AutoMapper;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Logs;

namespace NBL.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager _userManager = new UserManager();
        private readonly ICompanyManager _iCompanyManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;

        public LoginController(IBranchManager iBranchManager,ICompanyManager iCompanyManager,ICommonManager iCommonManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iCompanyManager = iCompanyManager;
            _iCommonManager = iCommonManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: LogIn
        public ActionResult LogIn()
        {
           
            //var text = StringCipher.Encrypt(
            //     "Data Source=192.168.2.62; Initial Catalog=dbUniversalBusinessSolution1; User Id=sa; Password=Nbl&Cit&Navana;Integrated Security=false;MultipleActiveResultSets=true;",
            //     "salam_cse_10_R");
            //_iCommonManager.SaveEncriptedConString(text);
            Session["user"] = null;
            Session["Branches"] = null;
            Session["Roles"] = null;
            ViewBag.Companies = _iCompanyManager.GetAll().ToList().OrderBy(n => n.CompanyId).ToList();
            ViewBag.Roles = _iCommonManager.GetAllUserRoles();
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(FormCollection collection, string ReturnUrl)
        {


            try
            {
                //int roleId = Convert.ToInt32(collection["RoleId"]);
                User user = new User();
                string userName = collection["userName"];
                string password = collection["Password"];
                user.Password = password;
                user.UserName = userName;

                var userLocation = GetUserLocation();
                bool validUser = _userManager.ValidateUser(user);

                if (validUser)
                {
                    userLocation.IsValidLogin = 1;
                    var model = _userManager.GetUserByUserName(user.UserName);
                    var anUser = Mapper.Map<User, ViewUser>(model);
                    Session["Branches"] = _iCommonManager.GetAssignedBranchesToUserByUserId(anUser.UserId).ToList();
                    Session["Roles"] = _userManager.GetAssignedUserRolesByUserId(anUser.UserId);
                    anUser.IsGeneralRequisitionRight = _iCommonManager.GetFirstApprovalPathByUserId(anUser.UserId);
                    anUser.IsApprovalRight = _iCommonManager.GetFirstApprovalPathByApproverUserId(anUser.UserId);
                    Session["user"] = anUser;
                    int companyId = Convert.ToInt32(collection["companyId"]);
                    var company = _iCompanyManager.GetById(companyId);
                    Session["CompanyId"] = companyId;
                    Session["Company"] = company;
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    var employee = _iEmployeeManager.GetEmployeeById(anUser.EmployeeId);
                    if (employee.EmployeeName != null)
                    {
                        anUser.EmployeeImage = employee.EmployeeImage;
                        anUser.DesignationName = employee.DesignationName;
                        anUser.EmployeeName = employee.EmployeeName;
                    }
                    else
                    {
                        anUser.EmployeeImage = "Images/login_image.png";
                        anUser.EmployeeName = userName;
                    }
                    anUser.IpAddress = GetRealIpAddress();
                    anUser.MacAddress = GetMacAddress().ToString();
                    anUser.LogInDateTime = DateTime.Now;

                    bool result = _userManager.ChangeLoginStatus(anUser, 1, userLocation);
                    Session.Timeout = 180;
                    switch (anUser.Roles)
                    {

                        case "SuperUser":
                            return RedirectToAction("Home", "Home", new { area = "SuperAdmin" });
                        case "SystemAdmin":
                            return RedirectToAction("Home", "Home", new { area = "Editor" });
                        case "Corporate":
                        case "CorporateSalesAdmin":
                            return RedirectToAction("Home", "Home", new { area = "Corporate" });
                        default:
                            return RedirectToAction("Goto", "LogIn", new { area = "" });
                    }


                }
                ViewBag.Message = "Invalid Login";
                ViewBag.Companies = _iCompanyManager.GetAll().ToList().OrderBy(n => n.CompanyId).ToList();
                return View();
            }
            catch(Exception exception)
            {


                ViewWriteLogModel model = new ViewWriteLogModel
                {
                    Heading = exception.GetType().ToString(),
                    LogMessage = exception.StackTrace
                };
                Log.WriteErrorLog(model);
                return null;
            }
        }
        public ActionResult GoTo()
        {
            var user = Session["user"];
            if (user != null)
            {
               
               
                Session["user"] = user;
                return View();
            }
            return RedirectToAction("LogIn", "LogIn", new { area = "" });
        }
        [HttpPost]
        public ActionResult GoTo(FormCollection collection)
        {
            int branchId = Convert.ToInt32(collection["BranchId"]);
            int roleId = Convert.ToInt32(collection["RoleId"]);
            var roles = (List<ViewAssignedUserRole>)Session["Roles"];
            var user = (ViewUser)Session["user"];
            user.Roles = roles.Find(n => n.RoleId == roleId).RoleName;
            bool r = _iCommonManager.UpdateCurrentUserRole(user, roleId);
            var branch= _iBranchManager.GetById(branchId);
            Session["BranchId"] = branchId;
            Session["Branch"] = branch;
           
           
            switch (user.Roles)
            {
                case "Admin":
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                   
                case "SalesExecutive":
                case "SalesManager":
                case "DistributionManager":
                case "SalesAdmin":
                    return RedirectToAction("Home", "Home", new { area = "Sales" });
                            
                case "AccountExecutive": 
                case "AccountManager":
                    return RedirectToAction("Home", "Home", new { area = "AccountsAndFinance" });
                case "Management":
                    return RedirectToAction("Home", "Home", new { area = "Management" });
                case "ServiceExecutive":
                    return RedirectToAction("Home", "Home", new { area = "Services" });
                case "StoreManagerFactory":
                    return RedirectToAction("Home", "Home", new { area = "Production" });
                case "ProductionManager":
                    return RedirectToAction("Home", "ProductionManager", new { area = "Production" });
                case "DispatchManager":
                    return RedirectToAction("Home", "DispatchManager", new { area = "Production" });
                case "FqcExecutive":
                    return RedirectToAction("Home", "Qc", new { area = "Production" });
                case "PH":
                    return RedirectToAction("Home", "ProductionHead", new { area = "Production" });
                case "R&D":
                    return RedirectToAction("Home", "Home", new { area = "ResearchAndDevelopment" });
                case "SCMManager":
                    return RedirectToAction("Home", "Home", new { area = "SCM" });
                default:
                    return RedirectToAction("LogIn", "LogIn", new { area = "" });
            }
           
        }
        public ActionResult Logout()
        {
            var userLocation = GetUserLocation();
            userLocation.IsValidLogin = 1;
            FormsAuthentication.SignOut();
            var user = (ViewUser)Session["user"];
            user.LogOutDateTime = DateTime.Now;
            _userManager.ChangeLoginStatus(user, 0,userLocation);
            return Redirect("~/Home/Index");

        }

        //public ActionResult ChangePassword(int id)
        //{
        //    var user = _userManager.GetUserInformationByUserId(id);
        //    user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
        //    return View(user);
        //}

        //------------------ Change password------------------------
        public PartialViewResult ChangePassword(int id)
        {

            var user = _userManager.GetUserInformationByUserId(id);
            user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return PartialView("_ChangePasswordPartialPage", user);
        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {

                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        public string GetRealIpAddress()
        {
            var strIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            return strIpAddress;
        }



        public UserLocation GetUserLocation()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            string APIKey = "6e0f2fe166ad8d1e4d77974f9d3920f77f805fa7167a91de1b1620fc44842922";
            string url = string.Format("http://api.ipinfodb.com/v3/ip-city/?key={0}&ip={1}&format=json", APIKey, ipAddress);
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                var location = new JavaScriptSerializer().Deserialize<UserLocation>(json);

                //List<UserLocation> locations = new List<UserLocation>();
                //locations.Add(location);
                //var result = locations;
                //gvLocation.DataSource = locations;
                //gvLocation.DataBind();
                return location;
            }
        }
    }
}