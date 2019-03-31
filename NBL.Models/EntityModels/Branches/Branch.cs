using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Locations;
using NBL.Models.EntityModels.Orders;

namespace NBL.Models.EntityModels.Branches
{
    public class Branch:IGetInformation
    {
        public int BranchId { get; set; }

        [Display(Name = "Sub Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Branch Name")]
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string Title { get; set; } 
        [Display(Name = "Address")]
        [Required]
        public string BranchAddress { get; set; }
        [Display(Name = "Opening Date")]
        [Required]
        public DateTime BranchOpenigDate { get; set; }
        [Display(Name = "Phone")]
        [Required]
        public string BranchPhone { get; set; }
        [Display(Name = "E-mail")]
        [Required]
        public string BranchEmail { get; set; }
        public  ICollection<Region> RegionList { get; set; }
        public  ICollection<Client> Clients { get; set; }
        public  ICollection<Order> Orders { get; set; } 

        public Branch()
        {
            RegionList=new List<Region>();
            Clients=new List<Client>();
            Orders=new List<Order>();
        }

        public string GetBasicInformation()
        {
            return BranchName;
        }

        public string GetFullInformation()
        {
            return $"<strong style='font-size:25px'> {BranchName}</strong></br> <strong style='font-size:15px'>{Title} </strong><br/> {BranchAddress}";
        }
    }
}