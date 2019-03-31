using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Locations;

namespace NBL.DAL
{
    public class PostOfficeGateway:DbGateway,IPostOfficeGateway
    {
        public IEnumerable<PostOffice> GetAllPostOfficeByUpazillaId(int upazillaId)
        {
            
                try
                {

                    List<PostOffice> postOffices = new List<PostOffice>();
                    ConnectionObj.Open();
                    CommandObj.CommandText = "spGetPostOfficeByUpazillaId";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@UpazillaId", upazillaId);
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        postOffices.Add(new PostOffice
                        {
                            PostOfficeId = Convert.ToInt32(reader["Id"]),
                            PostOfficeName = reader["PostOfficeName"].ToString(),
                            Code = reader["PostCode"].ToString(),
                            UpazillaId = Convert.ToInt32(reader["UpazillaId"])

                        });
                    }
                    reader.Close();
                    return postOffices;
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to collect postOffices", e);
                }
                finally
                {
                    ConnectionObj.Close();

                }
            }
        
    }
}