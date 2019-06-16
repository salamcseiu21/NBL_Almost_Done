using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Securities
{
    public class ViewLoginInfo
    {

        public string IpAddress { get; set; }
        public string MacAddress { get; set; }  
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public DateTime LoginDateTime { get; set; } 
    }
}
