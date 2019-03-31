using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;

namespace NBL.DAL
{
    public class DistrictGateway:DbGateway,IDistrictGateway
    {
        public IEnumerable<District> GetAllDistrictByDivistionId(int divisionId)
        {
          
                try
                {

                    List<District> districts = new List<District>();
                    ConnectionObj.Open();
                    CommandObj.CommandText = "spGetDistrictByDivisionId";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@DivistionId", divisionId);
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        districts.Add(new District
                        {
                            DistrictId = Convert.ToInt32(reader["DistrictId"]),
                            DistrictName = reader["DistrictName"].ToString(),
                            DivisionId = Convert.ToInt32(reader["DivisionId"])

                        });
                    }
                    reader.Close();
                    return districts;
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to collect districts", e);
                }
                finally
                {
                    ConnectionObj.Close();

                }
            
        }

        public IEnumerable<District> GetAllDistrictByRegionId(int regionId)
        {
            try
            {

                List<District> districts = new List<District>();
                ConnectionObj.Open();
                CommandObj.CommandText = "spGetAssignedDistrictByRegionId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionId", regionId);
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    districts.Add(new District
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"])

                    });
                }
                reader.Close();
                return districts;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect districts", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        public IEnumerable<District> GetUnAssignedDistrictListByRegionId(int regionId)
        {
            try
            {

                List<District> districts = new List<District>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetUnAssignedDistrictListByRegionId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RegionId", regionId);
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    districts.Add(new District
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"])

                    });
                }
                reader.Close();
                return districts;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect un assigned  districts by region id ", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        public int Add(District model)
        {
            throw new NotImplementedException();
        }

        public int Update(District model)
        {
            throw new NotImplementedException();
        }

        public int Delete(District model)
        {
            throw new NotImplementedException();
        }

        public District GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<District> GetAll()
        {
            try
            {

                List<District> districts = new List<District>();
                ConnectionObj.Open();
                CommandObj.CommandText = "spGetAllDistrict";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    districts.Add(new District
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"])

                    });
                }
                reader.Close();
                return districts;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect districts", e);
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