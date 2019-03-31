using System;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.ViewModels
{
    public class ViewCreateProductionNoteModel
    {
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name is required")]
        public int ProductId { get; set; }
        public string ProductionNoteNo { get; set; } 
        public string ProductionNoteRef { get; set; }
        [Display(Name = "Date")]
        [Required]
        public DateTime ProductionNoteDate { get; set; }
        public int ProductionNoteByUserId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string EntryStatus { get; set; }
        public DateTime SysDateTime { get; set; }

    }
}
