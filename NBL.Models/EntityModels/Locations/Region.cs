using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;

namespace NBL.Models.EntityModels.Locations
{
    public class Region
    {
        public int RegionId { get; set; }
        [Required]
        [Display(Name = "Region Name")]
        public string RegionName { get; set; }
        [Required]
        public string Description { get; set; }

        public string Alias { get; set; }   
        [Display(Name = "Division")]
        public int? DivisionId { get; set; }
        public string IsAssigned { get; set; }
        public string IsCurrent { get; set; }
        public DateTime SysDateTime { get; set; } 
        public Division Division { get; set; }
        public List<Territory> Territories { get; set; }
        public List<District> Districts { get; set; }
        public int BranchId { get; set; }   
        public Branch Branch { get; set; }
        public Region()
        {
            Districts=new List<District>();
            Division=new Division(); 
            Territories=new List<Territory>();
            Branch=new Branch();
        }

    }
}