using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Identities;

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
                throw new Exception("Could not collect assigned branches", exception);
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