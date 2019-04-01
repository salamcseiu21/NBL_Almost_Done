using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Returns
{
   public  class ViewEntryReturnModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string BarCode { get; set; } 
    }
}
