using System.ComponentModel;
using NBL.Models.Contracts;

namespace NBL.Models
{
    public class SubSubSubAccount:IGetInformation
    {
        public int SubSubSubAccountId { get; set; }
        [DisplayName("Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [DisplayName("Account Name")]
        public string SubSubSubAccountName { get; set; }
        public string SubSubSubAccountType { get; set; }


        public string GetBasicInformation()
        {
            return SubSubSubAccountName;
        }

        public string GetFullInformation()
        {
            return $"Account Name: {SubSubSubAccountName} <br/> Code:{SubSubSubAccountCode}";
        }
    }
}