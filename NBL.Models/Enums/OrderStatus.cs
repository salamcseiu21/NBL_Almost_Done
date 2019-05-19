
namespace NBL.Models.Enums
{
    public enum OrderStatus 
    {
        Pending = 0,
        ApprovedbyNsm = 1,
        InvoicedOrApprovedbyAdmin = 2,
        PartiallyDelivered = 3,
        Delivered = 4,
        CancelledbySalesPerson = 5,
        CancelledbyNsm = 6,
        CancelledbyAdmin = 7,
        DistributionPointSet=8
    }

    public enum OrderStatus1
    {
        Pending = 0,
        ApprovedbyNsm = 1,
        InvoicedOrApprovedbyAdmin = 2,
        PartiallyDelivered = 3,
        Delivered = 4,
        CancelledbySalesPerson = 5,
        CancelledbyNsm = 6,
        CancelledbyAdmin = 7
    }
}
