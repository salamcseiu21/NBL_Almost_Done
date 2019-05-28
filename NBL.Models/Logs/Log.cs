using System;
using System.IO;
using System.Web;
using System.Xml.Linq;
using NBL.Models.ViewModels.Logs;

namespace NBL.Models.Logs
{
   public class Log
    {
        //public static void WriteLog(string strLog)
        //   {
        //    string filePath = HttpContext.Current.Server.MapPath("/Logs")+"/Log_" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
        //    if (File.Exists(filePath))
        //    {
        //        ;
        //        //if the file is exists read the file
        //        using (StreamWriter w = File.AppendText(filePath))
        //        {
        //            w.WriteLine(strLog);
        //            w.Flush();
        //        }
        //    }
        //    else
        //    {
        //        //if the file does not exists create the file
        //        File.Create(filePath).Close();
        //        using (StreamWriter w = File.AppendText(filePath))
        //        {
        //            w.WriteLine(strLog);
        //            w.Flush();
        //        }
        //    }
            
        //}

        //public static void LogWrite(ViewWriteLogModel model)
        //{
        //    string filePath = HttpContext.Current.Server.MapPath("/Logs") + "/Log_" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
        //    if (!File.Exists(filePath))
        //        File.Create(filePath).Close();

        //    try
        //    {
        //        using (StreamWriter w = File.AppendText(filePath))
        //            AppendLog(model, w);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //}

        //private static void AppendLog(ViewWriteLogModel model, TextWriter txtWriter)
        //{
        //    try
        //    {
        //        txtWriter.Write(Environment.NewLine+ "Log Entry : ");
        //        txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
        //        txtWriter.WriteLine(":"+model.Heading);
        //        txtWriter.WriteLine("  :{0}", model.LogMessage);
        //        txtWriter.WriteLine("-------------------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception();
        //    }
        //}

        public static void WriteErrorLog(ViewWriteLogModel model)
        {
            var filePath = HttpContext.Current.Server.MapPath("~/Logs/" + "Error_log_Xml_file.xml");
            var xmlDocument = XDocument.Load(filePath);
            xmlDocument.Element("Errors")?.Add(
                new XElement("Error", new XAttribute("LogId", DateTime.Now.ToString("yy-MMM-dd")+Guid.NewGuid()),
                    new XElement("Heading", model.Heading),
                    new XElement("LogMessage", model.LogMessage),
                    new XElement("LogDateTime", DateTime.Now)
                ));
            xmlDocument.Save(filePath);
        }

        public static void WriteErrorLog(Exception model)
        {

            var filePath = HttpContext.Current.Server.MapPath("~/Logs/" + "Error_log_Xml_file.xml");
            var xmlDocument = XDocument.Load(filePath);
            xmlDocument.Element("Errors")?.Add(
                new XElement("Error", new XAttribute("LogId", DateTime.Now.ToString("yy-MMM-dd") + Guid.NewGuid()),
                    new XElement("Heading", model.GetType().ToString()),
                    new XElement("LogMessage", model.StackTrace),
                    new XElement("LogDateTime", DateTime.Now)
                ));
            xmlDocument.Save(filePath);
        }
    }
}
