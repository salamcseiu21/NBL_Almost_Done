﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Products
{
   public class ViewProductHistory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductCategoryName { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveredBy { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int ClientId { get; set; }   
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public string ClientCode { get; set; }
        public string ClientPhone { get; set; }
        public string ClientAddress { get; set; } 
        
        public string TerritoryName { get; set; }
        public string RegionName { get; set; }
        public string AssignedEmpName { get; set; }
        public string AssignedEmpPhone { get; set; } 
        
        public string OrderRef { get; set; }
        public string InvoiceRef { get; set; }
        public string DeliveryRef { get; set; }
        public string OrderFromBranch { get; set; }
        public string DeliveryFromBranch { get; set; }
        public int ProductAge { get; set; }
        public int LifeTime { get; set; }
        public int AgeLimitInDealerStock { get; set; } 
        public int RemainingLifeTime => LifeTime - Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(SaleDate).Date).TotalDays);

        public int StoreDuration => Convert.ToInt32((DeliveryDate.Date - ProductionDate.Date).TotalDays);
        public int SalesDuration => Convert.ToInt32((Convert.ToDateTime(SaleDate).Date - DeliveryDate.Date).TotalDays);

        public int ServiceDuration { get; set; } 
        public int CollectionDuration { get; set; }
        public string ReceiveRef { get; set; }
        public string Remarks { get; set; }
        public ICollection<ViewProductTransactionDetails> TransactionDetailses { get; set; }
          
        public ViewProductHistory()
        {
            TransactionDetailses=new List<ViewProductTransactionDetails>();
        }
    }
}
