using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Transports;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Models.EntityModels.Deliveries
{
    public class Delivery
    {
        public long DeliveryId { get; set; }
        public long RequisitionId { get; set; } 
        public DateTime DeliveryDate { get; set; }
        public string DeliveryRef { get; set; }
        public string TransactionRef { get; set; }
        public long VoucherNo { get; set; }   
        public string InvoiceRef { get; set; }
        public int InvoiceId { get; set; }
        public bool? IsOwnTransport { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public int Status { get; set; }
        public int DeliveredByUserId { get; set; }
        public DateTime SysDateTime { get; set; }
        public int ToBranchId { get; set; }
        public int FromBranchId { get; set; }
        public Transport Transport { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public string ProductBarCode { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public string ClientInfo { get; set; }
        public Client Client { get; set; }
        public ViewTripModel TripModel { get; set; }
        public FinancialTransactionModel FinancialTransactionModel { get; set; }    
        public Delivery()
        {
            Transport=new Transport();
            Client=new Client();
        }

    }
}