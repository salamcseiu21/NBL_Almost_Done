using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Orders;

namespace NBL.Models.ViewModels.Orders
{
   public class ViewOrderSearchModel
    {
       // readonly OrderManager orderManager=new OrderManager();
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Category")]
        public string ProductCategory { get; set; }
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public ViewOrderSearchModel()
        {
            //Orders = orderManager.GetAll; 
        }
    }
}
