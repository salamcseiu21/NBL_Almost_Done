using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Departments;
using NBL.Models.EntityModels.Designations;

namespace NBL.DAL 
{
    public class DepartmentGateway:DbGateway,IDepartmentGateway
    {

       
        readonly DesignationGateway _designationGateway=new DesignationGateway();
        public Department GetById(int id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDepartmentById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", id);
                Department aDepartment = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    aDepartment = new Department
                    {
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DepartmentCode = reader["DepartmentCode"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    };
                }
                reader.Close();
                return aDepartment;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Department by Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

       

        public ICollection<Department> GetAll()
        {
        try
        {
            CommandObj.CommandText = "spGetAllDepartment";
            CommandObj.CommandType = CommandType.StoredProcedure;
            List<Department> departments = new List<Department>();
            ConnectionObj.Open();
            SqlDataReader reader = CommandObj.ExecuteReader();
            while (reader.Read())
            {
                departments.Add(new Department
                {
                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                    DepartmentName = reader["DepartmentName"].ToString(),
                    DepartmentCode = reader["DepartmentCode"].ToString()
                });
            }
            reader.Close();
            return departments;
        }
    catch (Exception exception)
    {
    throw new Exception("Could not collect department", exception);
}
finally
{
ConnectionObj.Close();
CommandObj.Dispose();
CommandObj.Parameters.Clear();
}
        }

        public int Add(Department aDepartment)
        {
            try
            {
                CommandObj.CommandText = "spAddNewDepartment";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentCode", aDepartment.DepartmentCode);
                CommandObj.Parameters.AddWithValue("@DepartmentName", aDepartment.DepartmentName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Number == 2601)
                {
                    throw new Exception("Cannot insert duplicate key", sqlException);
                }
                throw new Exception("Could not Save department", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save department", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(Department aDepartment)
        {
            try
            {
                CommandObj.CommandText = "spUpdateDepartmentInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", aDepartment.DepartmentId);
               CommandObj.Parameters.AddWithValue("@DepartmentCode", aDepartment.DepartmentCode);
                CommandObj.Parameters.AddWithValue("@DepartmentName", aDepartment.DepartmentName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Update department information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Delete(Department model)
        {
            throw new NotImplementedException();
        }


        public List<Designation> GetAllDesignationByDepartmentId(int departmentId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDesignationByDepartmentId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", departmentId);
                List<Designation> designations=new List<Designation>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    designations.Add(new Designation
                    {
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        DesignationName = reader["DesignationName"].ToString(),
                        DesignationCode = reader["DesignationCode"].ToString()
                    });
                }
                reader.Close();
                return designations;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect designation by  department id due to Sql Exception ", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect designation by  department id ", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Department GetDepartmentByCode(string code)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeparmentByCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentCode", code);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Department aDepartment = new Department();
                if (reader.Read())
                {

                    aDepartment.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    aDepartment.DepartmentName = reader["DepartmentName"].ToString();
                    aDepartment.DepartmentCode = reader["DepartmentCode"].ToString();

                }
                reader.Close();
                return aDepartment;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect department by Code", exception);
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