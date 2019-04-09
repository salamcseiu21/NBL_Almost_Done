
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Security;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.ViewModels;

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
            Session["user"] = null;
            Session["Branches"] = null;
            ViewBag.Companies = _iCompanyManager.GetAll().ToList().OrderBy(n => n.CompanyId).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(FormCollection collection, string ReturnUrl)
        {
            User user = new User();
            string userName = collection["userName"];
            string password = collection["Password"];
            user.Password = password;
            user.UserName = userName;
            if (IsValid(user))
            {
                int companyId = Convert.ToInt32(collection["companyId"]);
                var company= _iCompanyManager.GetById(companyId);
                Session["CompanyId"] = companyId;
                Session["Company"] = company;
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                var anUser = _userManager.GetUserByUserNameAndPassword(user.UserName, user.Password);
                var employee = _iEmployeeManager.GetEmployeeById(anUser.EmployeeId);
                if (employee.EmployeeName!= null)
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
                anUser.IpAddress = GetLocalIPAddress();
                anUser.MacAddress = GetMacAddress().ToString();
                anUser.LogInDateTime = DateTime.Now;

                bool result = _userManager.ChangeLoginStatus(anUser, 1);

                Session["user"] = anUser;
                Session.Timeout = 180;
                switch (anUser.Roles)
                {
                    case "Super":
                        return RedirectToAction("Home", "Home", new { area = "SuperAdmin" });
                    case "Factory":
                        return RedirectToAction("Home", "Home", new { area = "Production" });
                    case "Editor":
                        return RedirectToAction("Home", "Home", new { area = "Editor" });
                    case "Corporate":
                        return RedirectToAction("Home", "OperationHead", new { area = "Corporate" });
                    case "CorporateSalesAdmin":
                        return RedirectToAction("Home", "SalesAdmin", new { area = "Corporate" });
                    case "R&D":
                        return RedirectToAction("Home", "Home", new { area = "ResearchAndDevelopment" });
                    case "QC":
                        return RedirectToAction("Home", "Home", new { area = "QC" });
                    default:
                        return RedirectToAction("Goto", "LogIn", new { area = "" });
                }
                

            }
            ViewBag.Message = "Invalid Login";
            ViewBag.Companies = _iCompanyManager.GetAll().ToList().OrderBy(n => n.CompanyId).ToList();
            return View();
        }

        private bool IsValid(User user)
        {
            bool isExits = false;
            var anUser=_userManager.GetUserByUserNameAndPassword(user.UserName,user.Password);
            if(anUser.UserName !=null)
            {
                Session["Branches"] = _iCommonManager.GetAssignedBranchesToUserByUserId(anUser.UserId).ToList();
                Session["user"] = anUser;
                isExits = true;
            }
            return isExits;
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
            var branch= _iBranchManager.GetById(branchId);
            Session["BranchId"] = branchId;
            Session["Branch"] = branch;
            var user = (ViewUser)Session["user"];
            switch (user.Roles)
            {
                case "Admin":
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                   
                case "User":
                    return RedirectToAction("Home", "SalesPerson", new { area = "Sales" });
                case "Nsm":
                    return RedirectToAction("Home", "Nsm", new { area = "Sales" });
                case "Distributor":
                    return RedirectToAction("Home", "Distributor", new { area = "Sales" });
                case "SalesAdmin":
                    return RedirectToAction("Home", "SalesAdmin", new { area = "Sales" });
                case "Super":
                    return RedirectToAction("Home", "Home", new { area = "SuperAdmin" });
               
                case "Accounts":
                    return RedirectToAction("Home", "Home", new { area = "AccountsAndFinance" });
                case "AccountExecutive":
                    return RedirectToAction("Home", "Home", new { area = "AccountExecutive" });
                case "Management":
                    return RedirectToAction("Home", "Home", new { area = "Management" });
                default:
                    return RedirectToAction("LogIn", "LogIn", new { area = "" });
            }
           
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var user = (ViewUser)Session["user"];
            user.LogOutDateTime = DateTime.Now;
            _userManager.ChangeLoginStatus(user, 0);
            return Redirect("~/Home/Index");

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
    }
}