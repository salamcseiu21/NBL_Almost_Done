using AutoMapper;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Returns;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Clients;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;

namespace NBL.Models.AutoMapper
{
    class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<ViewClient, Client>();
            CreateMap<Client, ViewClient>();
            CreateMap<Branch, ViewBranch>();
            CreateMap<ViewBranch, Branch>();
            CreateMap<Order, ViewOrder>();
            CreateMap<ViewOrder, Order>();
            CreateMap<ViewCreateProductionNoteModel, ProductionNote>();
            CreateMap<ProductionNote, ViewCreateProductionNoteModel>();
            CreateMap<ViewCreateClientModel, Client>();
            CreateMap<Client, ViewCreateClientModel>();
            CreateMap<ViewCreateRetailSaleModel, RetailSale>();
            CreateMap<RetailSale, ViewCreateRetailSaleModel>();
            CreateMap<ReturnDetails, ViewDeliveredOrderModel>();
        }
    }
}
