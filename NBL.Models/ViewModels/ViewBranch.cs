using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Locations;
using NBL.Models.EntityModels.Orders;

namespace NBL.Models.ViewModels
{
    public class ViewBranch
    {
        public int BranchId { get; set; }

        [Display(Name = "Sub Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Branch Name")]
 
        public string BranchName { get; set; }
        public string Title { get; set; }
        [Display(Name = "Address")]
        public string BranchAddress { get; set; }
        [Display(Name = "Opening Date")]
        public DateTime BranchOpenigDate { get; set; }
        [Display(Name = "Phone")]
        public string BranchPhone { get; set; }
        [Display(Name = "E-mail")]
        [Required]
        public string BranchEmail { get; set; }
        public List<Region> RegionList { get; set; }
        public List<Client> Clients { get; set; }
        public List<Order> Orders { get; set; }
    }
}