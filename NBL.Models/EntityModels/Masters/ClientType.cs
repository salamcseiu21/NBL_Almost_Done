using System.Collections.Generic;
using NBL.Models.EntityModels.Clients;

namespace NBL.Models.EntityModels.Masters 
{
    public class ClientType
    {
        public int ClientTypeId { get; set; }
        public string ClientTypeName { get; set; }
        public decimal DiscountPercent { get; set; }
        public List<Client> Clients { get; set; } 
    }
}