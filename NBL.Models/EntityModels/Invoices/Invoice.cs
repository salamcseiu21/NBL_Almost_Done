using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Clients;

namespace NBL.Models.EntityModels.Invoices
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDateTime { get; set; }
        public string InvoiceRef { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionType { get; set; }  
        public int InvoiceNo { get; set; }
        public int InvoiceStatus { get; set; }
        public int InvoiceByUserId { get; set; }
        public DateTime SysDateTime { get; set; }
        public int VoucherNo { get; set; }
        public string Explanation { get; set; }
        public string ClientAccountCode { get; set; }
        public string DiscountAccountCode { get; set; }
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Client Id")]
        [Required]
        public int ClientId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public decimal Amounts { get; set; }
        public decimal NetAmounts { get; set; }
        public int Quantity { get; set; }   
        public decimal Discount { get; set; }
        public decimal SpecialDiscount { get; set; }
        public char Cancel { get; set; }
        public decimal Vat { get; set; }
        public Client Client { get; set; } 
        
    }
}