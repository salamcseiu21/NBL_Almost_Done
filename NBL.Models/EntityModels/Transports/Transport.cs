using NBL.Models.Contracts;

namespace NBL.Models.EntityModels.Transports
{
    public class Transport:IGetInformation
    {
        public int TransportId { get; set; }  
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public string GetBasicInformation()
        {
            return $"Transporation:{Transportation},Driver Name:{DriverName},Driver Phone:{DriverPhone},Vehicle No: {VehicleNo},Cost:{TransportationCost}";
        }

        public string GetFullInformation()
        {
            return $"<strong>Transporation:</strong> {Transportation} <br/><strong>Driver Name:</strong> {DriverName} <br/><strong>Driver Phone:</strong> {DriverPhone} <br/><strong>Vehicle No:</strong> {VehicleNo} <br/><strong>Cost:</strong> {TransportationCost}";
        }
    }
}