using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;

namespace NBL.Models.EntityModels.TransferProducts
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public string TransactionRef { get; set; }
        public string DeliveryRef { get; set; }
        public int DeliveryId { get; set; }
        public DateTime TransactionDate { get; set; }   
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int StockQuantity { get; set; }
        public int UserId { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public decimal CostPrice { set; get; }
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public string ProductBarCode { get; set; }
        public string RecievedProductBarCodes { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }

    }
}