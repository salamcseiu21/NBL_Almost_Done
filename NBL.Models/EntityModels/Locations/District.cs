using System.Collections.Generic;

namespace NBL.Models.EntityModels.Locations 
{
    public class District
    {
       
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public List<Upazilla> Upazillas { get; set; }
        public District()
        {
            Division=new Division();
            Upazillas=new List<Upazilla>();
        }
        
    }
}