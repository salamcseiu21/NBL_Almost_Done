using System;

namespace NBL.Models.EntityModels.Deliveries
{
    public class DeliveryModel
    {
        public string Invoice { get; set; }
        public int InventoryId { get; set; }
        public int BranchId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Transactionid { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
