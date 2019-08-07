using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Returns
{
   public class ViewReturnDetails
    {
        public long SalsesReturnDetailsId { get; set; }
        public long SalesReturnId { get; set; }
        public long SalesReturnNo { get; set; }
        public string SalesReturnRef { get; set; }
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
        public int BranchId { get; set; }
        public decimal Amounts { get; set; }
        public int OrderByUserId { get; set; }
        public string SalesPersonName { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string OrderRef { get; set; }
        public decimal SepecialDiscount { get; set; } 
        public int NsmUserId { get; set; }
        public string NsmName { get; set; }
        public DateTime? ApproveByNsmDateTime { get; set; }
        public int SalesAdminUserId { get; set; }
        public string SalesAdminName { get; set; }
        public DateTime? ApproveBySalesAdminDateTime { get; set; }
        public int DeliveredByUserId { get; set; }
        public string DeliveryPerson { get; set; }
        public DateTime? DeliveredDateTime { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public int Quantity { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public decimal VatAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
