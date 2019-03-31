using System;

namespace NBL.Models
{
    public class ReferenceAccount
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
       
        public DateTime SysDateTime { get; set; } 
    }
}
