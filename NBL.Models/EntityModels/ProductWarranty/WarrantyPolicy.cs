
using System;

namespace NBL.Models.EntityModels.ProductWarranty
{
    public class WarrantyPolicy
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public int WarrantyPeriodInDays { get; set; }
        public int AgeLimitInDealerStock { get; set; }
        public string WarrantyFrom { get; set; }
        public string FromBatch { get; set; }
        public string ToBatch { get; set; }
        public int? ClientId { get; set; }
        public int IsActive { get; set; }
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }
    }
}
