
using System;

namespace NBL.Models.Validators
{
    public  class Validator
    {
        public static bool ValidateProductBarCode(string barCode)
        {
            if(IsValiedProductId(barCode) && barCode.Length==13)
            {
                return true;
            }
            return false;
        }

        private static bool IsValidDate(string str)
        {
            var d = Convert.ToInt32(str.Substring(0, 2));
            var m = Convert.ToInt32(str.Substring(2, 2));
            if(d<=31 && d>0 && m<=12 && m>0)
                return true;
            return false;
        }

        private static bool IsValiedProductId(string barCode) 
        {
            var charArray = barCode.ToCharArray();
            if(char.IsDigit(charArray[2]) && char.IsDigit(charArray[3]) && char.IsDigit(charArray[4]))
            {
                return true;
            }
            return false;
        }
    }
}
