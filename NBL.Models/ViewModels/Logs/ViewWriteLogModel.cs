
using System;

namespace NBL.Models.ViewModels.Logs
{
    public class ViewWriteLogModel
    {
        public string LogId { get; set; }   
        public string Heading { get; set; }
        public string LogMessage { get; set; }
        public DateTime LogDateTime { get; set; }   
    }
}
