using System;
namespace NBL.Models.Contracts
{
   public interface IAudit
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
