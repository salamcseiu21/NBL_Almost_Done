using System;
using System.Collections.Generic;

namespace NBL.Models.EntityModels.Locations
{
    public class Division
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int AddedByUserId { get; set; }
        public DateTime AddedDateTime { get; set; }
        public List<District> Districts { get; set; }
        public List<Region> Regions { get; set; } 
        public Division()
        {
            Districts=new List<District>();
            Regions=new List<Region>();
        }
    }
}