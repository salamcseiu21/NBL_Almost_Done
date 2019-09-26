using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Masters;
using NBL.Models.Logs;

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
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect employee Types", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(EmployeeType model)
        {
            try
            {
                CommandObj.CommandText = "spAddNewEmployeeType";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@TypeName", model.EmployeeTypeName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not add employee Type", exception);
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