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
    }
}