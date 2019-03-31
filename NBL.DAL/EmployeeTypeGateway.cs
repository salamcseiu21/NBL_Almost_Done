using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Masters;

namespace NBL.DAL
{
    public class EmployeeTypeGateway:DbGateway,IEmployeeTypeGateway
    {
        public IEnumerable<EmployeeType> GetAll()
        {
            try
            {
                CommandObj.CommandText = "spGetAllEmployeeType";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<EmployeeType> employeeTypes = new List<EmployeeType>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    employeeTypes.Add(new EmployeeType
                    {
                        EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                        EmployeeTypeName = reader["EmployeeTypeName"].ToString()
                    });
                }
                reader.Close();
                return employeeTypes;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect employee Types", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
    }
}