using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;

namespace NBL.DAL
{
    public class DivisionGateway:DbGateway,IDivisionGateway
    {
       private readonly TerritoryGateway _territoryGateway=new TerritoryGateway();

        public int Add(Division model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Division model)
        {
            throw new NotImplementedException();
        }

        public ICollection<Division> GetAll()
        {
            try
            {

                List<Division> divisions = new List<Division>();
                CommandObj.CommandText = "spGetAllDivision";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    divisions.Add(new Division
                    {
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        DivisionName = reader["DivisionName"].ToString(),
                    });
                }
                reader.Close();
                ConnectionObj.Close();
                foreach (Division division in divisions)
                {
                    division.Regions = GetRegionListByDivisionId(division.DivisionId);
                }



                return divisions;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect divisions", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Division GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(Division model)
        {
            throw new NotImplementedException();
        }

        

        private List<Region> GetRegionListByDivisionId(int divisionId)  
        {
            try
            {
                List<Region> regions = new List<Region>();
                CommandObj.CommandText = "spGetRegionListByDivisionId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DivisionId", divisionId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    regions.Add(new Region
                    {
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        RegionName = reader["RegionName"].ToString(),
                        DivisionId = divisionId
                    });
                }
                reader.Close();
                foreach (Region region in regions)
                {
                    region.Territories = _territoryGateway.GetTerritoryListByRegionId(region.RegionId).ToList();
                }
                return regions;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect regions by Division Id", e);
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