using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;

namespace NBL.Models.ViewModels.Orders
{
    public class ViewOrder
    {

        public string BranchName { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string SubSubSubAccountCode  { get; set; }
        public int OrderId { get; set; }
        [Display(Name = "Client Id")]
        [Required]
        public int ClientId { get; set; }
        [Display(Name = "Order Slip No")]
        public string OrderSlipNo { get; set; }
        [Display(Name = "User")]
        public int UserId { get; set; }
        public string OrderBy { get; set; }
        [Display(Name = "Nsm User Id")]
        public int NsmUserId { get; set; }
        public string SalesManager { get; set; }
        [Display(Name = "Delivered Or Receive User Id")]
        public int DeliveredOrReceiveUserId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
         
        public Branch Branch { get; set; }
        public int CompanyId { get; set; }
        public decimal Amounts { get; set; }
        public decimal NetAmounts { get; set; }
        public decimal Discount { get; set; }
        public decimal SpecialDiscount { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime SysDate { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Approved By Nsm")]
        public DateTime ApprovedByNsmDateTime { get; set; }
        [Display(Name = "Admin User Id")]
        public int AdminUserId { get; set; }
        public string SalesAdmin { get; set; }
        [Display(Name = "Approved By Admin")]
        public DateTime ApprovedByAdminDateTime { get; set; }
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDateTime { get; set; }
        public int DeliveredByUserId { get; set; }
        public string OrederRef { get; set; }
        public string InvoiceRef { get; set; }
        public char Cancel { get; set; }
        public int VerificationStatus { get; set; } 
        public string ResonOfCancel { get; set; }
        public int CancelByUserId { get; set; }
        public decimal Vat { get; set; }
        public DateTime CancelDateTime { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }  
        public Client Client { get; set; }
        public User User { get; set; }
        public int DistributionPointId { get; set; }
        public int DistributionPointSetByUserId { get; set; } 
        public DateTime DistributionPointSetDateTime { get; set; } 

    }
}