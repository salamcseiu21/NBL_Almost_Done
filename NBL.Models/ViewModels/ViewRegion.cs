using System;
using NBL.Models.EntityModels.Locations;

namespace NBL.Models.ViewModels
{
    public class ViewRegion
    {
        public int RegionId { get; set; }
        public int RegionDetailsId { get; set; } 
        public string RegionName { get; set; }
        public int DistrictId { get; set; }
        public int DivisionId { get; set; }
        public string IsAssigned { get; set; }
        public string IsCurrent { get; set; }
        public DateTime SysDateTime { get; set; }
        public Division Division { get; set; }
        public District District { get; set; }
        public ViewRegion()
        {
            Division=new Division();
            District=new District();
        }
    }
}