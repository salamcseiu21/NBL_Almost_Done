using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Departments;
using NBL.Models.EntityModels.Designations;
using NBL.Models.EntityModels.Employees;
using NBL.Models.EntityModels.Masters;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class EmployeeGateway:DbGateway,IEmployeeGateway
    {
       

        public ICollection<Employee> GetAll()
        {
            try
            {
                CommandObj.CommandText = "spGetAllEmployee";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Employee> employees = new List<Employee>();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Email = reader["EmailAddress"].ToString(),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Gender = reader["Gender"].ToString(),
                        PermanentAddress = reader["PermanentAddress"].ToString(),
                        PresentAddress = reader["PresentAddress"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AlternatePhone"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        NationalIdNo = reader["EmployeeNationalIdNo"].ToString(),
                        EmployeeImage = reader["EmployeeImage"].ToString(),
                        EmployeeSignature = reader["EmployeeSignature"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        EmployeeNo = reader["EmployeeNo"].ToString()

                    });
                }
                reader.Close();
                return employees;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Employee Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo()
        {
            try
            {
                CommandObj.CommandText = "spGetEmployeesWithFullInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewEmployee> employees = new List<ViewEmployee>();
                while (reader.Read())
                {
                    employees.Add(new ViewEmployee
                    {
                        Email = reader["EmailAddress"].ToString(),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        EmployeeType = new EmployeeType
                        {
                            EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                            EmployeeTypeName = reader["EmployeeTypeName"].ToString()
                        },
                        Department = new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            DepartmentCode = reader["DepartmentCode"].ToString()
                        },
                        Designation = new Designation
                        {
                            DesignationId = Convert.ToInt32(reader["DesignationId"]),
                            DesignationName = reader["DesignationName"].ToString(),
                            DesignationCode = reader["DesignationCode"].ToString()
                        },
                        Branch = new Branch
                        {
                            BranchId = Convert.ToInt32(reader["BranchId"]),
                            BranchName = reader["BranchName"].ToString()
                        },
                        Gender = reader["Gender"].ToString(),
                        PermanentAddress = DBNull.Value.Equals(reader["PermanentAddress"]) ? null : reader["PermanentAddress"].ToString(),
                        PresentAddress = DBNull.Value.Equals(reader["PresentAddress"]) ? null : reader["PresentAddress"].ToString(),
                        Phone = DBNull.Value.Equals(reader["Phone"]) ? null : reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AlternatePhone"]) ? null : reader["AlternatePhone"].ToString(),
                        Notes = DBNull.Value.Equals(reader["Notes"]) ? null : reader["Notes"].ToString(),
                        NationalIdNo = DBNull.Value.Equals(reader["EmployeeNationalIdNo"]) ? null : reader["EmployeeNationalIdNo"].ToString(),
                        EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"]) ? null : reader["EmployeeImage"].ToString(),
                        EmployeeSignature = DBNull.Value.Equals(reader["EmployeeSignature"]) ? null : reader["EmployeeSignature"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                        EmployeeNo = DBNull.Value.Equals(reader["EmployeeNo"]) ? null : reader["EmployeeNo"].ToString()
                        
                    });
                }
                reader.Close();
                return employees;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Employee Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "spGetEmployeesWithFullInfoByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewEmployee> employees = new List<ViewEmployee>();
                while (reader.Read())
                {
                    employees.Add(new ViewEmployee
                    {
                        Email = reader["EmailAddress"].ToString(),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        EmployeeType = new EmployeeType
                        {
                            EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                            EmployeeTypeName = reader["EmployeeTypeName"].ToString()
                        },
                        Department = new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        },

                        Designation = new Designation
                        {
                            DesignationId = Convert.ToInt32(reader["DesignationId"]),
                            DesignationName = reader["DesignationName"].ToString(),
                        },
                        BranchId = branchId,
                        BranchName = reader["BranchName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        PermanentAddress =DBNull.Value.Equals(reader["PermanentAddress"])?null: reader["PermanentAddress"].ToString(),
                        PresentAddress = DBNull.Value.Equals(reader["PresentAddress"]) ? null : reader["PresentAddress"].ToString(),
                        Phone = DBNull.Value.Equals(reader["Phone"]) ? null : reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AlternatePhone"]) ? null : reader["AlternatePhone"].ToString(),
                        Notes = DBNull.Value.Equals(reader["Notes"]) ? null : reader["Notes"].ToString(),
                        NationalIdNo = DBNull.Value.Equals(reader["EmployeeNationalIdNo"]) ? null : reader["EmployeeNationalIdNo"].ToString(),
                        EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"]) ? null : reader["EmployeeImage"].ToString(),
                        EmployeeSignature = DBNull.Value.Equals(reader["EmployeeSignature"]) ? null : reader["EmployeeSignature"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                        EmployeeNo = DBNull.Value.Equals(reader["EmployeeNo"]) ? null : reader["EmployeeNo"].ToString()
                    });
                }
                reader.Close();
                return employees;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Employee Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewEmployee GetEmployeeById(int empId)
        {
            try
            {
                CommandObj.CommandText = "spGetEmployeeById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@EmployeeId", empId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewEmployee employee =new ViewEmployee();
                if (reader.Read())
                {
                    employee = new ViewEmployee
                    {
                        Email = reader["EmailAddress"].ToString(),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                        EmployeeTypeName = reader["EmployeeTypeName"].ToString(),
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DepartmentName = reader["DepartmentName"].ToString(),
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        DesignationName = reader["DesignationName"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        PermanentAddress = reader["PermanentAddress"].ToString(),
                        PresentAddress = reader["PresentAddress"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AlternatePhone"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        NationalIdNo = reader["EmployeeNationalIdNo"].ToString(),
                        EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"])? null:reader["EmployeeImage"].ToString(),
                        EmployeeSignature = DBNull.Value.Equals(reader["EmployeeSignature"]) ? null : reader["EmployeeSignature"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        EmployeeNo = reader["EmployeeNo"].ToString()
                    };
                    
                }
                reader.Close();
                return employee;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Employee Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(Employee anEmployee)
        {
            try
            {

               
                CommandObj.CommandText = "spAddNewEmployee";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@EmployeeTypeId", anEmployee.EmployeeTypeId);
                CommandObj.Parameters.AddWithValue("@DesignationId", anEmployee.DesignationId);
                CommandObj.Parameters.AddWithValue("@DepartmentId", anEmployee.DepartmentId);
                CommandObj.Parameters.AddWithValue("@BranchId", anEmployee.BranchId);
                CommandObj.Parameters.AddWithValue("@EmployeeNo", anEmployee.EmployeeNo);
                CommandObj.Parameters.AddWithValue("@EmployeeName", anEmployee.EmployeeName);
                CommandObj.Parameters.AddWithValue("@Gender", anEmployee.Gender);
                CommandObj.Parameters.AddWithValue("@PresentAddress", anEmployee.PresentAddress);
                CommandObj.Parameters.AddWithValue("@EmailAddress", anEmployee.Email);
                CommandObj.Parameters.AddWithValue("@Phone", anEmployee.Phone);
                CommandObj.Parameters.AddWithValue("@AlternatePhone", anEmployee.AlternatePhone ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeImage", anEmployee.EmployeeImage ??(object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeSignature", anEmployee.EmployeeSignature?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeNationalIdNo", anEmployee.NationalIdNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", anEmployee.SubSubSubAccountCode ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@UserId", anEmployee.UserId);
                CommandObj.Parameters.AddWithValue("@JoiningDate", anEmployee.JoiningDate);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save Employee", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Employee GetEmployeeByEmailAddress(string email)
        {
            try
            {

                Employee employee=new Employee();
                CommandObj.CommandText = "spGetEmployeeByEmailAddress";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Email", email);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                //int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                if (reader.Read())
                {
                    employee.Email = reader["EmailAddress"].ToString();
                    employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    employee.EmployeeName = reader["EmployeeName"].ToString();
                    employee.EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]);
                    employee.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    employee.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                    employee.BranchId = Convert.ToInt32(reader["BranchId"]);
                    employee.Gender = reader["Gender"].ToString();
                    employee.PermanentAddress = reader["PermanentAddress"].ToString();
                    employee.PresentAddress = reader["PresentAddress"].ToString();
                    employee.Phone = reader["Phone"].ToString();
                    employee.AlternatePhone = reader["AlternatePhone"].ToString();
                    employee.Notes = reader["Notes"].ToString();
                    employee.NationalIdNo = reader["EmployeeNationalIdNo"].ToString();
                    employee.EmployeeImage = reader["EmployeeImage"].ToString();
                    employee.EmployeeSignature = reader["EmployeeSignature"].ToString();

                }
               
                reader.Close();
                return employee;
            }
            catch (Exception e)
            {

                throw new Exception("Unable to Get Employee By Email", e);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int GetEmployeeMaxSerialNo()
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "spGetEmployeeMaxSerialNo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSlno = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return maxSlno;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not get  Employee max serial", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(Employee anEmployee)
        {
            try
            {
                CommandObj.CommandText = "spUpdateEmployeeInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@EmployeeTypeId", anEmployee.EmployeeTypeId);
                CommandObj.Parameters.AddWithValue("@DesignationId", anEmployee.DesignationId);
                CommandObj.Parameters.AddWithValue("@DepartmentId", anEmployee.DepartmentId);
                CommandObj.Parameters.AddWithValue("@BranchId", anEmployee.BranchId);
                CommandObj.Parameters.AddWithValue("@EmployeeNo", anEmployee.EmployeeNo);
                CommandObj.Parameters.AddWithValue("@EmployeeName", anEmployee.EmployeeName);
                CommandObj.Parameters.AddWithValue("@Gender", anEmployee.Gender);
                CommandObj.Parameters.AddWithValue("@PresentAddress", anEmployee.PresentAddress);
                CommandObj.Parameters.AddWithValue("@Phone", anEmployee.Phone);
                CommandObj.Parameters.AddWithValue("@Email", anEmployee.Email);
                CommandObj.Parameters.AddWithValue("@AlternatePhone", anEmployee.AlternatePhone?? anEmployee.AlternatePhone ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeImage", anEmployee.EmployeeImage ?? anEmployee.EmployeeImage ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeSignature", anEmployee.EmployeeSignature ?? anEmployee.EmployeeSignature ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@EmployeeNationalIdNo", anEmployee.NationalIdNo ?? anEmployee.NationalIdNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@JoiningDate", anEmployee.JoiningDate);
                CommandObj.Parameters.AddWithValue("@EmployeeId", anEmployee.EmployeeId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Update Employee Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Delete(Employee model)
        {
            throw new NotImplementedException();
        }
        public Employee GetById(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetEmpoyeeListByDepartmentId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", departmentId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Employee> employees = new List<Employee>();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Email = reader["EmailAddress"].ToString(),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        EmployeeTypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Gender = reader["Gender"].ToString(),
                        PermanentAddress = reader["PermanentAddress"].ToString(),
                        PresentAddress = reader["PresentAddress"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AlternatePhone"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        NationalIdNo = reader["EmployeeNationalIdNo"].ToString(),
                        EmployeeImage = reader["EmployeeImage"].ToString(),
                        EmployeeSignature = reader["EmployeeSignature"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString()
                    });
                }
                reader.Close();
                return employees;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Employee Information by Department Id", exception);
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