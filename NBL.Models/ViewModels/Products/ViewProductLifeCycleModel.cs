using System;
namespace NBL.Models.ViewModels.Products
{
    public class ViewProductLifeCycleModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ComeIntoInventory { get; set; }
        public int Age { get; set; }
        public int LifeTime { get; set; }
        public int RemainingLifeTime => LifeTime - Age;
        public int Status { get; set; }  
    }
}
