using System;
using NBL.Models.EntityModels.Employees;

namespace NBL.Models
{
    public static class Generator
    {
        public static string GenerateAccountCode(string prefix, int lastSlNo)  
        {
            string subSubSubAccountCode = prefix + (lastSlNo + 1);
            return subSubSubAccountCode;
        }

        public static string GenerateEmployeeNo(Employee employee,int lastSn)
        {
            string empNo = $"NBL-{employee.EmployeeTypeId:D2}{employee.DepartmentId:D2}{employee.DesignationId:D2}{lastSn+1:D3}";
            return empNo;
        }

        public static DateTime GenerateDateFromBarCode(string scannedBarCode)
        {
            var str = scannedBarCode.Substring(3, 6);
            var dd = str.Substring(0, 2);
            var mm = str.Substring(2, 2);
            var yy = str.Substring(4, 2);
            var date = dd + "-" + mm + "-20" + yy;
            DateTime myDate = DateTime.Parse(date);
            return myDate;
        }


        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " LAKHES ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " HUNDRED ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIZTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NIENTY" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}