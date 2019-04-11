using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Departments;
using NBL.Models.EntityModels.Designations;
using NBL.Models.EntityModels.Identities;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class UserGateway:DbGateway,IUserGateway
    {

        public IEnumerable<User> GetAll
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllUser";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<User> users = new List<User>();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            ActiveStaus = Convert.ToInt32(reader["ActiveStatus"]),
                            EmployeeId = DBNull.Value.Equals(reader["EmployeeId"])? 0:Convert.ToInt32(reader["EmployeeId"]),
                            UserName = reader["UserName"].ToString(),
                            BlockStatus = Convert.ToInt32(reader["BlockStatus"]),
                            Roles = reader["RoleName"].ToString(),
                            Password = reader["UserPassword"].ToString(),
                            EmployeeName = DBNull.Value.Equals(reader["EmployeeName"])? null: reader["EmployeeName"].ToString(),
                            Phone = DBNull.Value.Equals(reader["Phone"])? null: reader["Phone"].ToString(),
                            Email = DBNull.Value.Equals(reader["EmailAddress"])?null: reader["EmailAddress"].ToString(),
                            UserRoleId = Convert.ToInt32(reader["RoleId"]),
                            PresentAddress = DBNull.Value.Equals(reader["PresentAddress"])? null: reader["PresentAddress"].ToString(),
                            JoiningDate = Convert.ToDateTime(reader["UserJoiningDate"])
                        });
                    }
                    reader.Close();
                    return users;
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not collect User informations", exception);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public User GetUserInformationByUserId(int userId)
        {
            try
            {
                CommandObj.CommandText = "spGetUserInformationByUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                User user = new User();
                if (reader.Read())
                {
                     user = new User
                    {
                        UserId = userId,
                        ActiveStaus = Convert.ToInt32(reader["ActiveStatus"]),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        UserName = reader["UserName"].ToString(),
                        BlockStatus = Convert.ToInt32(reader["BlockStatus"]),
                        Department = new Department
                        {
                          DepartmentCode  = reader["DepartmentCode"].ToString(),
                          DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                          DepartmentName = reader["DepartmentName"].ToString(),
                        },
                        Designation = new Designation
                        {
                          DesignationCode  = reader["DesignationCode"].ToString(),
                          DesignationName = reader["DesignationName"].ToString(),
                          DesignationId =Convert.ToInt32(reader["DesignationId"])
                        },
                        Password = reader["UserPassword"].ToString(),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["EmailAddress"].ToString(),
                        Roles = reader["RoleName"].ToString(),
                        UserRoleId = Convert.ToInt32(reader["RoleId"]),
                        PresentAddress = reader["PresentAddress"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["UserJoiningDate"])
                    };
                }
                reader.Close();
                return user;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect User informations by user id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

      

        public int AddNewUser(User user)
        {
            try
            {
                ConnectionObj.Open();
                CommandObj.CommandText = "spAddNewUser";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserName", user.UserName);
                CommandObj.Parameters.AddWithValue("@UserPassword", user.Password);
                CommandObj.Parameters.AddWithValue("@EmployeeId", user.EmployeeId);
                CommandObj.Parameters.AddWithValue("@AddedByUserId", user.AddedByUserId);
                CommandObj.Parameters.AddWithValue("@UseRoleId", user.UserRoleId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception e)
            {
                throw new Exception("Could not add user", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public User GetUserByUserName(string userName)
        {
            try
            {
                User user = null;
                ConnectionObj.Open();
                CommandObj.CommandText = "spGetUserByUserName";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserName", userName);
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                        Password = reader["UserPassword"].ToString(),
                        ActiveStaus = Convert.ToInt32(reader["ActiveStatus"]),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        JoiningDate = Convert.ToDateTime(reader["UserJoiningDate"]),
                        UserRoleId = Convert.ToInt32(reader["RoleId"]),
                        Roles = reader["Roles"].ToString(),
                        BlockStatus = Convert.ToInt32(reader["BlockStatus"]),
                        IpAddress =DBNull.Value.Equals(reader["IpAddress"])?null: reader["IpAddress"].ToString(),
                        MacAddress = DBNull.Value.Equals(reader["MacAddress"]) ? null : reader["MacAddress"].ToString(),
                        LogInDateTime = DBNull.Value.Equals(reader["LogInDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["LogInDateTime"]),
                        LogOutDateTime = DBNull.Value.Equals(reader["LogOutDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["LogOutDateTime"])
                    };

                }
                reader.Close();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find user by User Name", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public ViewUser GetUserByUserNameAndPassword(string userName, string password)
        {
            try
            {
                ViewUser user = new ViewUser();
                ConnectionObj.Open();
                CommandObj.CommandText = "spGetUserByUserNameAndPassword";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserName", userName);
                CommandObj.Parameters.AddWithValue("@Password", password);
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {

                    user = new ViewUser
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                        Password = reader["UserPassword"].ToString(),
                        ActiveStaus = Convert.ToInt32(reader["ActiveStatus"]),
                        BlockStatus = Convert.ToInt32(reader["BlockStatus"]),
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        UserRoleId = Convert.ToInt32(reader["RoleId"]),
                        IpAddress = reader["IpAddress"].ToString(),
                        MacAddress = reader["MacAddress"].ToString(),
                        LogInDateTime = Convert.ToDateTime(reader["LogInDateTime"]),
                        LogOutDateTime = Convert.ToDateTime(reader["LogOutDateTime"]),
                        Roles = reader["Roles"].ToString()
                    };

                }
                reader.Close();
                return user;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Log in failed due to sql exception",sqlException);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid login", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public bool ChangeLoginStatus(ViewUser user, int status)
        {

            try
            {
                ConnectionObj.Open();
                CommandObj.CommandText = "spChangeLoginStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", user.UserId);
                CommandObj.Parameters.AddWithValue("@IpAddress", user.IpAddress);
                CommandObj.Parameters.AddWithValue("@MacAddress", user.MacAddress);
                CommandObj.Parameters.AddWithValue("@LoginDateTime", user.LogInDateTime);
                CommandObj.Parameters.AddWithValue("@LogOutDateTime", user.LogOutDateTime);
                CommandObj.Parameters.AddWithValue("@ActiveStatus", status);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                if (rowAffected > 0)
                {
                    return true;
                }

                return false;
            }

            catch (Exception e)
            {
                throw new Exception("Could not change login status", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<User> GetAllUserForAutoComplete()
        {
            try
            {
                CommandObj.CommandText = "spGetAllUsersForAutoComplete";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<User> users = new List<User>();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                    });
                }
                reader.Close();
                return users;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect User names", exception);
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