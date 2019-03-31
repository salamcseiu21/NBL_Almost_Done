using NBL.Models.EntityModels.Locations;

namespace NBL.Models.ViewModels
{
    public class ViewAssignedUpazilla
    {
        public int TerritoryDetailsId { get; set; }
        public int DistrictId { get; set; }
        public string UpazillaName { get; set; }
        public int UpazillaId { get; set; }
        public int TerritoryId { get; set; }
        public Territory Territory { get; set; }

    }
}