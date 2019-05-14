using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.Contracts
{
    public interface IMonth
    {
         int? January { get; set; }
         int? February { get; set; }
         int? March { get; set; }
         int? April { get; set; }
         int? May { get; set; }
         int? June { get; set; }
         int? July { get; set; }
         int? August { get; set; }
         int? September { get; set; }
         int? October { get; set; }
         int? November { get; set; }
         int? December { get; set; }
    }
}
