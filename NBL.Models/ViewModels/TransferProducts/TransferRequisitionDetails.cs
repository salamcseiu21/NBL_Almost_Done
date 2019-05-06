
namespace NBL.Models.ViewModels.TransferProducts
{
    public class TransferRequisitionDetails
    {
        public long TransferRequisitionId { get; set; }
        public int RequisitionByBranchId { get; set; }
        public long TransferRequisitionDetailsId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; } 
    }
}
