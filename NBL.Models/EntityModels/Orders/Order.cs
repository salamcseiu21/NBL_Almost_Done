using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Orders;

namespace NBL.Models.EntityModels.Orders
{
    public class Order
    {
        public long OrderId { get; set; }
        [Display(Name = "Client Id")]
        [Required]
        public int ClientId { get; set; }
        [Display(Name = "Order Slip No")]
        public string OrderSlipNo { get; set; }
        [Display(Name = "User")]
        public int UserId { get; set; }
        [Display(Name = "Nsm User Id")]
        public int NsmUserId { get; set; }
        [Display(Name = "Delivered Or Receive User Id")]
        public int DeliveredOrReceiveUserId { get; set; } 
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public int CompanyId { get; set; }  
        public decimal Amounts { get; set; }
        public decimal NetAmounts { get; set; }
        public int Quantity { get; set; } 
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
        [Display(Name = "Approved By Admin")]
        public DateTime ApprovedByAdminDateTime { get; set; }  

        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDateTime { get; set; } 
        public int DeliveredByUserId { get; set; }
        public string OrederRef { get; set; }
        public char Cancel { get; set; }
        public string ResonOfCancel { get; set; }
        public int CancelByUserId { get; set; }
        public  decimal Vat { get; set; }
        public DateTime CancelDateTime { get; set; }
        public  IEnumerable<Product> Products { get; set; }
        public IEnumerable<TempOrderedProduct> TempOrderedProducts { get; set; }
        public Client Client { get; set; }
        public User User { get; set; } 
        public Order(List<Product> products,int clientId,DateTime date):this()
        {
            this.Products = products;
            this.ClientId = clientId;
            this.OrderDate = date;
        }
        public Order ()
            {
                Products=new List<Product>();
              TempOrderedProducts=new List<TempOrderedProduct>();
                Client=new Client();
                User=new User();
            }
    }
}