
using System.ComponentModel;

namespace NBL.Models.Enums
{
    public enum OrderStatus 
    {
        Pending = 0,
        ApprovedbySalesManager = 1,
        InvoicedOrApprovedbySalesAdmin = 2,
        PartiallyDelivered = 3,
        Delivered = 4,
        CancelledbySalesPerson = 5,
        CancelledbySalesManager = 6,
        CancelledbySalesAdmin = 7,
        DistributionPointSet=8,
        CancelByScm=9
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
