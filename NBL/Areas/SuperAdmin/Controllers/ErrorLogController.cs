using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Logs;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class ErrorLogController : Controller
    {
        // GET: SuperAdmin/ErrorLog
        public ActionResult ErrorList()
        {
           
            var errorList= GetErrorListFromXmalFile(GetErrorLogFileXmlFilePath());
            return View(errorList);
        }

        private IEnumerable<ViewWriteLogModel> GetErrorListFromXmalFile(string filePath) 
        {
            List<ViewWriteLogModel> list = new List<ViewWriteLogModel>();
            var xmlData = XDocument.Load(filePath).Element("Errors")?.Elements();
            foreach (XElement element in xmlData)
            {
                ViewWriteLogModel aLogModel = new ViewWriteLogModel();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aLogModel.LogId = elementFirstAttribute;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aLogModel.Heading = xElements[0].Value;
                aLogModel.LogMessage = xElements[1].Value;
                aLogModel.LogDateTime = Convert.ToDateTime(xElements[2].Value);
                list.Add(aLogModel);
            }

            return list;
        }
        [HttpPost]
        public void RemoveErrorById(string id) 
        {

            var filePath = GetErrorLogFileXmlFilePath();
            var xmlData = XDocument.Load(GetErrorLogFileXmlFilePath());
            xmlData.Root?.Elements().Where(n => n.Attribute("LogId")?.Value == id).Remove();
            xmlData.Save(filePath);

        }

        private string GetErrorLogFileXmlFilePath()
        {
            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/" + "Error_log_Xml_file.xml");
            return filePath;
        }

        public ActionResult GenerateError()
        {
            try
            {
                int[] intArray2 = new int[5] { 1, 2, 3, 4, 5 };
                var t = 0;
                var x = 5 / t;
                var r = intArray2[80];
            }
            catch(Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
            return RedirectToAction("ErrorList");
        }
    }
}