using System;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels
{
    public class ViewProductionNoteModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductionNoteNo { get; set; }
        public string ProductionNoteRef { get; set; }
        public DateTime ProductionNoteDate { get; set; }
        public int ProductionNoteByUserId { get; set; }
        public int Quantity { get; set; }
        public string EntryStatus { get; set; }
        public DateTime SysDateTime { get; set; }
        public Product Product { get; set; } 
    } 
}
