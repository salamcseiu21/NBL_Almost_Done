using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.DAL
{
    public class DiscountGateway:DbGateway,IDiscountGateway
    {

        public IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDiscountsByClientType";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientTypeId", clientTypeId);
                List<Discount> discounts = new List<Discount>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    discounts.Add(new Discount
                    {
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ClientType = new ClientType
                        {
                            ClientTypeName = reader["ClientTypeName"].ToString(),
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                        }
                    });
                }
                reader.Close();
                return discounts;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Discounts", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Discount> GetAllPendingDiscounts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllPendingDiscounts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<Discount> discounts = new List<Discount>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    discounts.Add(new Discount
                    {
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
                        ClientType = new ClientType
                        {
                            ClientTypeName = reader["ClientTypeName"].ToString(),
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                            DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"])
                        }
                    });
                }
                reader.Close();
                return discounts;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending Discounts", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(Discount discount)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddDiscount";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", discount.ProductId);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", discount.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@DiscountPercent", discount.DiscountPercent);
                CommandObj.Parameters.AddWithValue("@UpdatedByUserId", discount.UpdateByUserId);
                CommandObj.Parameters.AddWithValue("@UpdateDate", discount.UpdateDate);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not add discount", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public int Update(Discount model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Discount model)
        {
            throw new NotImplementedException();
        }

        public Discount GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Discount> GetAll()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDiscounts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<Discount> discounts = new List<Discount>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    discounts.Add(new Discount
                    {
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ApprovedDate = Convert.ToDateTime(reader["ApproveDate"]),
                        ClientType = new ClientType
                        {
                            ClientTypeName = reader["ClientTypeName"].ToString(),
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                            DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"])
                        }
                    });
                }
                reader.Close();
                return discounts;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Discounts", exception);
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