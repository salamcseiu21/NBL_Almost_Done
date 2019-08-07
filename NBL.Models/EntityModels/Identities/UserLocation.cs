namespace NBL.Models.EntityModels.Identities
{
    public class UserLocation
    {
        public string IPAddress { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string TimeZone { get; set; }
        public int IsValidLogin { get; set; }
    }
}