using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Services
{
    public class ViewTestPolicy
    {
        public string ParameterName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public string TestCategory { get; set; }  
        public decimal AcceptableValue { get; set; }
        public string Remarks { get; set; }
        public decimal Ocv { get; set; }
        public decimal LoadVoltage { get; set; }
        public decimal SgDifference { get; set; }  
        

    }
}
