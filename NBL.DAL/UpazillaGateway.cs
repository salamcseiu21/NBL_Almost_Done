using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class UpazillaGateway:DbGateway,IUpazillaGateway
    {

        public IEnumerable<Upazilla> GetAllUpazillaByDistrictId(int districtId)
        {
            
                try
                {

                    List<Upazilla> upazillas = new List<Upazilla>();
                    ConnectionObj.Open();
                    CommandObj.CommandText = "spGetUpazillaByDistrictId";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@DistrictId",districtId);
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        upazillas.Add(new Upazilla
                        {
                            DistrictId = Convert.ToInt32(reader["DistrictId"]),
                            UpazillaName = reader["UpazillaName"].ToString(),
                            UpazillaId = Convert.ToInt32(reader["UpazillaId"])

                        });
                    }
                    reader.Close();
                    return upazillas;
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to collect upazillas", e);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                   CommandObj.Parameters.Clear();
                }
            
        }
        public IEnumerable<Upazilla> GetUnAssignedUpazillaByTerritoryId(int territoryId) 
        {
            try
            {

                List<Upazilla> upazillas = new List<Upazilla>();
                CommandObj.CommandText = "UDSP_GetUnAssignedUpazillaByTerritoryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TerritoryId", territoryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    upazillas.Add(new Upazilla
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"])

                    });
                }
                reader.Close();
                return upazillas;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect un assigned upazilla list by Territory Id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }
        public IEnumerable<Upazilla> GetAssignedUpazillaLsitByTerritoryId(int territoryId) 
        {
            try
            {

                List<Upazilla> upazillas = new List<Upazilla>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAssignedUpazillaListByTerritoryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TerritoryId", territoryId);
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    upazillas.Add(new Upazilla
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"])

                    });
                }
                reader.Close();
                return upazillas;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect upazilla list by Territory Id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }
        public IEnumerable<ViewAssignedUpazilla> GetAssignedUpazillaList() 
        {
            try
            {

                List<ViewAssignedUpazilla> upazillaList = new List<ViewAssignedUpazilla>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAssignedUpazillaList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    upazillaList.Add(new ViewAssignedUpazilla
                    {
                        TerritoryDetailsId = Convert.ToInt32(reader["TerritoryDetailsId"]),
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        Territory = new Territory
                        {
                            TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                            TerritoryName = reader["TerritoryName"].ToString()
                        }
                    });
                }
                reader.Close();
                return upazillaList;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect upazilla list", e);
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