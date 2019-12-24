using System;

namespace NBL.Models.Searchs
{
  public class SearchCriteria
    {
        public int? BranchId { get; set; }
        public int? CompanyId { get; set; } 
        public string ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; } 
        public string SubSubSubAccountCode { get; set; }
        public int SubSubSubAccountListId { get; set; }
        public int MonthNo { get; set; }
        public int Year { get; set; }   
    }
}
