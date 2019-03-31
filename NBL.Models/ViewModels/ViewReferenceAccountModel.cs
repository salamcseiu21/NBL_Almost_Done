using System;

namespace NBL.Models.ViewModels
{
    public class ViewReferenceAccountModel
    {
        public int Id { get; set; } 
        public int ReferenceAccount { get; set; } 
        public string Name { get; set; }
        public string Code { get; set; }
        public string ReferenceAccountCode { get; set; }    
        public DateTime SysDateTime { get; set; }
    }
}
