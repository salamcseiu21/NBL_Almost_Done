
using NBL.Models.EntityModels.Requisitions;

namespace NBL.Models.ViewModels.Requisitions
{
    public class ViewGeneralRequistionDetailsModel
    {
        public long GeneralRequisitionDetailsId { get; set; }
        public long GeneralRequisitionId { get; set; }
        public int RequisitionForId { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Vat { get; set; }
        public long DiscountId { get; set; }
        public decimal Discount { get; set; }

    }
}
