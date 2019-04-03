using System;
using System.ComponentModel.DataAnnotations;
namespace NBL.Models.ViewModels.Deliveries
{
   public class ViewDeliveredOrderModel
    {
        public long DeliveryId { get; set; }
        [Display(Name = "Delivered Quantity")]
        public int DeliveredQty { get; set; }
        [Display(Name = "Delivery Ref")]
        public string DeliveryRef { get; set; }
        public string InvoiceRef { get; set; }
        public long InvoiceId { get; set; }
        public string TransactionRef { get; set; }
        public int IsOwnTransporation { get; set; }
        public string Transportation { get; set; }
        public decimal TransportationCost { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string VehicleNo { get; set; }
        public int DeliveryStatus { get; set; } 
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public long OrderId { get; set; }
        public decimal Amounts { get; set; }
        public int OrderByUserId { get; set; }
        public string SalesPersonName { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int NsmUserId { get; set; }
        public string NsmName { get; set; }
        public DateTime ApproveByNsmDateTime { get; set; }
        public int SalesAdminUserId { get; set; }
        public string SalesAdminName { get; set; }
        public DateTime ApproveBySalesAdminDateTime { get; set; }
        public int DeliveredByUserId { get; set; }
        public string DistributorName { get; set; } 
        public DateTime DeliveredDateTime { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }   
    }
}
