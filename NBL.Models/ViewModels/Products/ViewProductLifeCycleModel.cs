using System;
using System.Web.UI.WebControls;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels.Orders;

namespace NBL.Models.ViewModels.Products
{
    public class ViewProductLifeCycleModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ComeIntoInventory { get; set; }
        public DateTime DispatchDate { get; set; }
        public DateTime ReceiveDate{ get; set; }
        public DateTime DistributioDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime ReturnDate { get; set; } 
        public int Age { get; set; }
        public int LifeTime { get; set; }
        public int RemainingLifeTime => LifeTime - Age;
        public int Status { get; set; }
        public Client Client { get; set; }
        public ViewOrder Order { get; set; }

        public ViewProductLifeCycleModel()
        {
            Client=new Client();
            Order=new ViewOrder();
        }
    }
}
