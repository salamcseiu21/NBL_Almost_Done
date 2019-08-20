using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.Areas.SuperAdmin.Models.ViewModels;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;
using NBL.Models.Logs;

namespace NBL.Areas.SuperAdmin.DAL
{
    public class SuperAdminUserGateway:DbGateway
    {
        public int AssignBranchToUser(User user, Branch branch)
        {
            
            
            try
            {
               
                    int rowAffected = 0;
                    ConnectionObj.Open();
                    CommandObj.CommandText = "spAssignBranchToUser";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.Clear();
                    CommandObj.Parameters.AddWithValue("@UserId", user.UserId);
                    CommandObj.Parameters.AddWithValue("@BranchId", branch.BranchId);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    CommandObj.ExecuteNonQuery();
                    rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                

                return rowAffected;
            }
            catch(Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not assign branch to user", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Branch> GetAssignedBranchByUserId(int userId)
        {
            try
            {
                CommandObj.CommandText = "spGetAssignedBranchToUserByUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                List<Branch> branchList = new List<Branch>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Branch aBranch = new Branch
                    {
                        BranchId=Convert.ToInt32(reader["BranchId"]),
                        BranchAddress=reader["BranchAddress"].ToString(),
                        BranchEmail=reader["Email"].ToString(),
                        BranchPhone=reader["Phone"].ToString(),
                        SubSubSubAccountCode=reader["SubSubSubAccountCode"].ToString(),
                        //RegionId=Convert.ToInt32(reader["RegionId"]),
                        BranchOpenigDate=Convert.ToDateTime(reader["BranchOpenigDate"]),
                        BranchName=reader["BranchName"].ToString()

                    };
                    branchList.Add(aBranch);

                }
                reader.Close();
                return branchList;
            }
            catch(Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect assigned branches", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int AssignRoleToUser(AssignRoleModel model)
        {
            try
            {
                ConnectionObj.Open();
                CommandObj.CommandText = "spAssignRoleToUser";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", model.BranchId);
                CommandObj.Parameters.AddWithValue("@RoleId", model.RoleId);
                CommandObj.Parameters.AddWithValue("@IsActive", model.ActiveStatus);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not assign role to user", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<User> GetAllUserWithRoles()
        {
            try
            {
                CommandObj.CommandText = "spGetAllUserWithRoles";
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
                        EmployeeId = DBNull.Value.Equals(reader["EmployeeId"]) ? 0 : Convert.ToInt32(reader["EmployeeId"]),
                        UserName = reader["UserName"].ToString(),
                        BlockStatus = Convert.ToInt32(reader["BlockStatus"]),
                        Roles = reader["RoleName"].ToString(),
                        Password = reader["UserPassword"].ToString(),
                        EmployeeName = DBNull.Value.Equals(reader["EmployeeName"]) ? null : reader["EmployeeName"].ToString(),
                        Phone = DBNull.Value.Equals(reader["Phone"]) ? null : reader["Phone"].ToString(),
                        Email = DBNull.Value.Equals(reader["EmailAddress"]) ? null : reader["EmailAddress"].ToString(),
                        UserRoleId = Convert.ToInt32(reader["RoleId"]),
                        PresentAddress = DBNull.Value.Equals(reader["PresentAddress"]) ? null : reader["PresentAddress"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["UserJoiningDate"]),
                        BranchName = reader["BranchName"].ToString()

                    });
                }
                reader.Close();
                return users;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
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
}