using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;

namespace NBL.DAL.Contracts
{
    public interface IFactoryDeliveryGateway:IGateway<Delivery>
    {
        int SaveDispatchInformation(DispatchModel dispatchModel);
        DispatchModel GetDispatchByDispatchId(long dispatchId);
        ICollection<ViewDispatchModel> GetDispatchDetailsByDispatchId(long dispatchId);
        int SaveDeliveredGeneralRequisition(List<ScannedProduct> scannedProducts, Delivery aDelivery);
        ICollection<ViewProduct> GetDespatchedBarcodeByDespatchId(long dispatchId);
    }
}
