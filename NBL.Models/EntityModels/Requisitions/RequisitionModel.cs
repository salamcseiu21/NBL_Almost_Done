using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;

namespace NBL.Models.EntityModels.Requisitions
{
   public class RequisitionModel 
    {
        public long RequisitionId { get; set; } 
        public string Serial { get; set; } 
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int RequisitionQty { get; set; }
        public int PendingQty { get; set; }
        public int DeliveryQty { get; set; }
        public int ToBranchId { get; set; }
        public int FromBranchId { get; set; }
        public Branch ToBranch { get; set; }
        public int RequisitionForId { get; set; }
        public RequisitionFor RequisitionFor { get; set; }
        public RequisitionModel()
        {
            ToBranch=new Branch();
        }
    }
}
