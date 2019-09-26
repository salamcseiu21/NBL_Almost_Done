using System;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Payments;

namespace NBL.Models
{
    public class ChequeDetails:Payment
    {
        public int ChequeDetailsId { get; set; }
        public int ReceivableId { get; set; }
        public DateTime SysDateTime { get; set; }
        public string ReceivableRef { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ActiveStatus { get; set; }
        public DateTime ReceivableDateTime { get; set; }
        public string ClientInfo { get; set; }
        public string CollectionByBranch { get; set; }
        public string EntryByEmp { get; set; }
        public int Cancel { get; set; }
        public string CancelRemarks { get; set; }
        public int? CancelByUserId { get; set; }
        public string CollectionByEmp { get; set; }
        public DateTime? ActiveDate { get; set; }
        public int UserId { get; set; } 
    }
}