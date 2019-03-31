using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class RegionGateway:DbGateway,IRegionGateway
    {
        private readonly TerritoryGateway _territoryGateway=new TerritoryGateway();
       
        public IEnumerable<ViewRegion> GetRegionListWithDistrictInfo()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRegionListWithDistrictInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewRegion> regions = new List<ViewRegion>();
                while (reader.Read())
                {

                    regions.Add(new ViewRegion
                    {
                        RegionDetailsId =Convert.ToInt32(reader["RegionDetailsId"]),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        IsAssigned = reader["IsAssigned"].ToString(),
                        IsCurrent = reader["IsCurrent"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        District = new District
                        {
                            DistrictId = Convert.ToInt32(reader["DistrictId"]),
                            DistrictName = reader["DistrictName"].ToString()
                        }
                    });
                }
                reader.Close();
                return regions;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect regions", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public ViewRegion GetRegionDetailsById(int regionDetailsId)  
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRegionDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionDetailsId", regionDetailsId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewRegion region = null;
                if(reader.Read())
                {

                    region = new ViewRegion
                    {
                        RegionDetailsId = regionDetailsId,
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        District = new District
                        {
                            DistrictId = Convert.ToInt32(reader["DistrictId"]),
                            DistrictName = reader["DistrictName"].ToString()
                        }
                    };
                }
                reader.Close();
                return region;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect regions", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int AssignDristrictToRegion(Region aRegion)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                
                int rowAffected = 0;
                foreach (var item in aRegion.Districts)
                {
                   
                    CommandObj.Transaction = sqlTransaction;
                    CommandObj.CommandText = "spAddNewDistrictToRegion";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@DistrictId", item.DistrictId);
                    CommandObj.Parameters.AddWithValue("@RegionId", aRegion.RegionId);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    CommandObj.ExecuteNonQuery();
                    rowAffected += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                    CommandObj.Parameters.Clear();
                }
                sqlTransaction.Commit();
                return rowAffected;
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not assign division or dristict to regions", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Region> GetUnAssignedRegionList()
        {
            try
            {
                CommandObj.CommandText = "spGetUnAssignedRegionList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Region> regions = new List<Region>();
                while (reader.Read())
                {
                    regions.Add(new Region
                    {
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"])
                    });
                }
                reader.Close();
                return regions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect unassigned regions", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int AssignRegionToBranch(Branch branch, ViewUser user)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {

                int rowAffected = 0;
                foreach (var item in branch.RegionList)
                {

                    CommandObj.Transaction = sqlTransaction;
                    CommandObj.CommandText = "spAssignNewRegionToBranch";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@BranchId", branch.BranchId);
                    CommandObj.Parameters.AddWithValue("@RegionId", item.RegionId);
                    CommandObj.Parameters.AddWithValue("@UserId", user.UserId);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    CommandObj.ExecuteNonQuery();
                    rowAffected += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                    CommandObj.Parameters.Clear();
                }
                sqlTransaction.Commit();
                return rowAffected;
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not assign division or dristict to regions", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public List<Region> GetAssignedRegionListToBranchByBranchId(int branchId)
        {

            try
            {
                CommandObj.CommandText = "spGetAssignedRegionListToBranchByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Region> regions = new List<Region>();
                while (reader.Read())
                {
                    regions.Add(new Region
                    {
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        Division = new Division
                        {
                            DivisionId = Convert.ToInt32(reader["DivisionId"]),
                            DivisionName = reader["DivisionName"].ToString()
                        }
                    });
                }
                reader.Close();
                return regions;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect regions by Branch Id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Branch GetBranchInformationByRegionId(int regionId) 
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetBranchInformationByRegionId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionId", regionId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Branch branch = new Branch();
                if(reader.Read())
                {
                     branch = new Branch
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchEmail = reader["Email"].ToString(),
                        BranchPhone = reader["Phone"].ToString()

                    };
                }
                reader.Close();
                return branch;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect branch info by region Id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, ViewUser user)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_UnAssignDistrictFromRegion";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionDetailsId", regionDetails.RegionDetailsId);
                CommandObj.Parameters.AddWithValue("@Reason", reason);
                CommandObj.Parameters.AddWithValue("@UnAssignedByUserId", user.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
               // var parameters = CommandObj.Parameters; 
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                int result = UnAssignUpazilla(regionDetails);
                if (result > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (SqlException sqlException )
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not UnAssign district due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not UnAssign district",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UnAssignUpazilla(ViewRegion viewRegion)
        {
            CommandObj.CommandText = "UDSP_UnAssignUpazilla";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@DistrictId", viewRegion.DistrictId);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return rowAffected;
        }

        public int Add(Region aRegion) 
        {
            try
            {
                CommandObj.CommandText = "spAddNewRegion";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DivisionId", aRegion.DivisionId ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Description", aRegion.Description ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Alias", aRegion.Alias ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@RegionName", aRegion.RegionName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save Region info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(Region model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Region model)
        {
            throw new NotImplementedException();
        }

        public Region GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Region> GetAll()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllRegions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Region> regions = new List<Region>();
                while (reader.Read())
                {
                    regions.Add(new Region
                    {
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        Description = DBNull.Value.Equals(reader["Description"])?null: reader["Description"].ToString(),
                        IsAssigned = reader["IsAssigned"].ToString(),
                        IsCurrent = reader["IsCurrent"].ToString(),
                        Territories = _territoryGateway.GetTerritoryListByRegionId(Convert.ToInt32(reader["RegionId"])).ToList()

                    });
                }
                reader.Close();
                return regions;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect regions", e);
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