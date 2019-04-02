using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class TerritoryGateway:DbGateway,ITerritoryGateway
    {
        readonly  UpazillaGateway _upazillaGateway=new UpazillaGateway();
        
        public IEnumerable<Territory> GetTerritoryListByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "spGetTerritoryListByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Territory> territories = new List<Territory>();
                while (reader.Read())
                {
                    territories.Add(new Territory
                    {
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        TerritoryName = reader["TerritoryName"].ToString(),
                        Description =DBNull.Value.Equals(reader["Description"])?null:reader["Description"].ToString(),
                        Region = new Region
                        {
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            RegionName = reader["RegionName"].ToString(),
                            Description =DBNull.Value.Equals(reader["RDescription"])?null: reader["RDescription"].ToString()
                        }
                    });
                }
                reader.Close();
                return territories;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect territories by branch id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Territory> GetTerritoryListByRegionId(int regionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTerritoryListByRegionId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionId", regionId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Territory> territories = new List<Territory>();
                while (reader.Read())
                {
                    territories.Add(new Territory
                    {
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        TerritoryName = reader["TerritoryName"].ToString(),
                        Region = new Region
                        {
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            RegionName = reader["RegionName"].ToString()
                        }
                    });
                }
                reader.Close();
                return territories;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect territories by branch id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        
        public int AssignUpazillaToTerritory(Territory aTerritory)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {

                int rowAffected = 0;
                foreach (var item in aTerritory.UpazillaList)
                {

                    CommandObj.Transaction = sqlTransaction;
                    CommandObj.CommandText = "spAddNewUpazillaToTerritory";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@UpazillaId", item.UpazillaId);
                    CommandObj.Parameters.AddWithValue("@TerritoryId", aTerritory.TerritoryId);
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
                throw new Exception("Could not assign dristict or upazila to territory", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, ViewUser user)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UnAssignUpazillaFromTerritory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TerritoryDetailsId", territoryDetailsId);
                CommandObj.Parameters.AddWithValue("@Reason", reason);
                CommandObj.Parameters.AddWithValue("@UnAssignedByUserId", user.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new  Exception("Could not Unassign upazilla",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
            }
        }

        public int Add(Territory aTerritory)
        {
            try
            {
                CommandObj.CommandText = "spAddNewTerritory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionId", aTerritory.RegionId);
                CommandObj.Parameters.AddWithValue("@TerittoryName", aTerritory.TerritoryName);
                CommandObj.Parameters.AddWithValue("@Description", aTerritory.Description?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Alias", aTerritory.TerritoryName ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@AddedByUserId", aTerritory.AddedByUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save territory info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(Territory model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Territory model)
        {
            throw new NotImplementedException();
        }

        public Territory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Territory> GetAll()
        {
            try
            {
                CommandObj.CommandText = "spGetAllTerritories";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Territory> territories = new List<Territory>();
                while (reader.Read())
                {
                    territories.Add(new Territory
                    {
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        TerritoryName = reader["TerritoryName"].ToString(),
                        Description = DBNull.Value.Equals(reader["Description"])?null: reader["Description"].ToString(),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        Region = new Region
                        {
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            RegionName = reader["RegionName"].ToString()
                        },
                        UpazillaList = _upazillaGateway.GetAssignedUpazillaLsitByTerritoryId(Convert.ToInt32(reader["TerritoryId"])).ToList()
                    });
                }
                reader.Close();
                return territories;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect territories", e);
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