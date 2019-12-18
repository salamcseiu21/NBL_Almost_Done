using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Others
{
   public class MessageModel
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; } 
        public string MessageBody { get; set; }  
        public string TransactionRef { get; set; }
        public int TotalQuantity { get; set; }
        public string ChequeNo { get; set; }  
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string GetMessageForDistribution() 
        {
            return
                $"Dear valued Customer with the Ref of:{TransactionRef},Total:{TotalQuantity} Pcs batteries sent to you and bill Amount is:{Amount} Tk at {TransactionDate} \r\n Navana Batteries Ltd.";
        }
        public string GetMessageForAccountReceivable()  
        {
            return
                $"Dear valued Customer with the Ref of:{TransactionRef} a cheque has been collected for the amount :{Amount} Tk at {TransactionDate} \r\n Navana Batteries Ltd.";
        }
        public string GetMessageForCashReceived() 
        {
            return
                $"Dear valued Customer with the Ref of:{TransactionRef} total :{Amount} Tk received at {TransactionDate} \r\n Navana Batteries Ltd.";
        }
    }
}
