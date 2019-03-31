
namespace NBL.Models.EntityModels.Requisitions
{
    public class RequisitionItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public int CategoryId { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public int Quantity { get; set; }   
    }
}
