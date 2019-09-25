using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using NBL.BLL;

namespace NBL
{
    public class MvcApplication :HttpApplication
    {
        readonly UserManager _userManager = new UserManager();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
           // SendMail();
        }



        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value)?.Name;
                        string roles = string.Empty;
                        var anUser = _userManager.GetUserByUserName(username);
                        roles = anUser.Roles;
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value)?.Name;
                        string roles = string.Empty;
                         var anUser = _userManager.GetUserByUserName(username);
                        roles = anUser.Roles;

                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }


        //private void SendMail()
        //{


        //    ////---------Send Mail ----------------
        //    //var aClient = _iClientManager.GetById(Convert.ToInt32(collection["ClientId"]));
        //    var body = $"Dear Abdus Salam Mail was send from Global aspx";
        //    var subject = $"New Receiable Create at {DateTime.Now}";
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress("salam@navana.com"));  // replace with valid value 
        //    message.Subject = subject;
        //    message.Body = string.Format(body);
        //    message.IsBodyHtml = true;
        //    using (var smtp = new SmtpClient())
        //    {
        //        smtp.Send(message);
        //    }
        //    ////------------End Send Mail-------------
        //}
    }
}
