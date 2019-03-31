using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class BranchGateway:DbGateway,IBranchGateway
    {
        //readonly  ClientManager _clientManager=new ClientManager();
        //readonly RegionManager _regionManager=new RegionManager();
        //readonly OrderManager _orderManager=new OrderManager();
      

        public int Update(Branch branch)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateBranchInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@BranchId", branch.BranchId);
                CommandObj.Parameters.AddWithValue("@BranchName", branch.BranchName);
                CommandObj.Parameters.AddWithValue("@Title", branch.Title);
                CommandObj.Parameters.AddWithValue("@BranchAddress", branch.BranchAddress);
                CommandObj.Parameters.AddWithValue("@BranchOpenigDate", branch.BranchOpenigDate);
                CommandObj.Parameters.AddWithValue("@Phone", branch.BranchPhone);
                CommandObj.Parameters.AddWithValue("@Email", branch.BranchEmail);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not Update branch info", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewAssignedRegion> GetAssignedRegionToBranchList()
        {
            try
            {
                CommandObj.CommandText = "spGetAssignedRegionToBranchList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewAssignedRegion> branches = new List<ViewAssignedRegion>();

                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var  o = new ViewAssignedRegion
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString()

                    };
                    branches.Add(o);

                }
                reader.Close();
                return branches;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not collect assigned lsit to branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxBranchSubSubSubAccountCode()
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "UDSP_GetMaxBranchSubSubSubAccountCode";
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
                throw new Exception("Could not get max sl no of Branch SubSubSub Account code",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewBranch> GetAllBranches()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewBranch> branches = new List<ViewBranch>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    branches.Add(new ViewBranch
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        BranchEmail = reader["Email"].ToString(),
                        BranchPhone = reader["Phone"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchOpenigDate = Convert.ToDateTime(reader["BranchOpenigDate"]),
                        Title = reader["Title"].ToString(),
                        //Clients = _clientManager.GetClientByBranchId(Convert.ToInt32(reader["BranchId"])).ToList(),
                        //RegionList = _regionManager.GetAssignedRegionListToBranchByBranchId(Convert.ToInt32(reader["BranchId"])),
                        //Orders =_orderManager.GetOrdersByBranchId(Convert.ToInt32(reader["BranchId"])).ToList()
                    });

                }
                reader.Close();
                return branches;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not collect branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(Branch branch)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddNewBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", branch.SubSubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@BranchName", branch.BranchName);
                CommandObj.Parameters.AddWithValue("@Title", branch.Title);
                CommandObj.Parameters.AddWithValue("@BranchAddress", branch.BranchAddress);
                CommandObj.Parameters.AddWithValue("@BranchOpenigDate", branch.BranchOpenigDate);
                CommandObj.Parameters.AddWithValue("@Phone", branch.BranchPhone);
                CommandObj.Parameters.AddWithValue("@Email", branch.BranchEmail);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not Save branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Delete(Branch model)
        {
            throw new NotImplementedException();
        }

        public Branch GetById(int id)
        {
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_GetBranchById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", id);
                Branch aBranch = new Branch();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    aBranch = new Branch
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        BranchEmail = reader["Email"].ToString(),
                        BranchPhone = reader["Phone"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchOpenigDate = Convert.ToDateTime(reader["BranchOpenigDate"]),
                        Title = reader["Title"].ToString()
                    };
                    reader.Close();
                }
                return aBranch;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not collect branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Dispose();
            }
        }
        public ICollection<Branch> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}