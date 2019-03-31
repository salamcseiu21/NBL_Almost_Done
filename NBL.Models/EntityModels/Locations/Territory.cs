using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.Locations
{
    public class Territory
    {
        public int TerritoryId { get; set; }
        [Required]
        [Display(Name = "Region")]

        public int RegionId { get; set; }
        [Required]
        public string Description { get; set; }

        public string Alias { get; set; }
        [Required]
        [Display(Name = "Territory Name")]
        public string TerritoryName { get; set; }
        public int AddedByUserId { get; set; } 
        public List<Upazilla> UpazillaList { get; set; }
        public Region Region { get; set; }

        public Territory()
        {
          Region=new Region();
        }
    }
}