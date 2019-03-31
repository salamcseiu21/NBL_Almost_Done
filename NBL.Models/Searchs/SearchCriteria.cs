using System;

namespace NBL.Models.Searchs
{
  public class SearchCriteria
    {
        public int? BranchId { get; set; }
        public string ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
