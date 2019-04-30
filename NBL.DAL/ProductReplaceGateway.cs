﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Replaces;

namespace NBL.DAL
{
    public class ProductReplaceGateway:DbGateway,IProductReplaceGateway
    {

        public int SaveReplacementInfo(ReplaceModel model)
        {

            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveReplacementInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", model.ClientId);
                CommandObj.Parameters.AddWithValue("@ReplaceRef", model.ReplaceRef);
                CommandObj.Parameters.AddWithValue("@ReplaceNo", model.ReplaceNo);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.ReplaceRef);
                CommandObj.Parameters.AddWithValue("@BranchId", model.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.Add("@ReplaceMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@ReplaceMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var masterId = Convert.ToInt64(CommandObj.Parameters["@ReplaceMasterId"].Value);
                rowAffected = SaveReplacementDetails(model, masterId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }

               return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save Replace Info",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxReplaceSerialNoByYear(int year)
        {
            try
            {
                int maxSl = 0;
                CommandObj.CommandText = "UDSP_GetMaxReplaceSerialNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSl = Convert.ToInt32(reader["MaxSl"]);
                }
                reader.Close();
                return maxSl;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max  Replace serial no by year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReplaceModel> GetAllReplaceListByBranchCompanyAndStatus(int branchId,int companyId,int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllReplaceListByBranchCompanyAndStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Status", status);
                List<ViewReplaceModel> models=new List<ViewReplaceModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewReplaceModel
                    {
                        ReplaceId = Convert.ToInt64(reader["ReplaceId"]),
                        ClientCode = reader["ClientCode"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientAddress = reader["ClientAddress"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ReplaceRef = reader["ReplaceRef"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    });
                }
                reader.Close();
                return models;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Replace list", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReplaceModel> GetAllPendingReplaceListByBranchAndCompany(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllPendingReplaceListByBranchAndCompany";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                List<ViewReplaceModel> models = new List<ViewReplaceModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewReplaceModel
                    {
                        ReplaceId = Convert.ToInt64(reader["ReplaceId"]),
                        ClientCode = reader["ClientCode"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientAddress = reader["ClientAddress"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ReplaceRef = reader["ReplaceRef"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    });
                }
                reader.Close();
                return models;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending Replace list", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewReplaceModel GetReplaceById(long id)
        {
            try
            {
                ViewReplaceModel model=null;
                CommandObj.CommandText = "UDSP_GetReplaceById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReplaceId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model=new ViewReplaceModel
                    {
                        ReplaceId = Convert.ToInt64(reader["ReplaceId"]),
                        ClientCode = reader["ClientCode"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientAddress = reader["ClientAddress"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ReplaceRef = reader["ReplaceRef"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Replace by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReplaceDetailsModel> GetReplaceProductListById(long id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReplaceProductListById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReplaceId", id);
                List<ViewReplaceDetailsModel> products=new List<ViewReplaceDetailsModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReplaceDetailsModel
                    {
                        ReplaceDetailsId = Convert.ToInt64(reader["ReplaceDetailsId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"])
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Replace product lsit by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetDeliveredProductsByReplaceRef(string replaceRef)
        {
            try
            {
                List<ViewProduct> products = new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_GetDeliveredProductsByReplaceRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReplaceRef", replaceRef);
                ConnectionObj.Open();

                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Delivered product by  replace ref", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveReplacementDetails(ReplaceModel model, long masterId)
        {
            int n = 0;
            foreach (var item in model.Products)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveReplacementDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@ReplaceId", masterId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@ExpiryDate", item.ExpiryDate);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                n += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return n;
        }

        public int Add(ReplaceModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(ReplaceModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(ReplaceModel model)
        {
            throw new NotImplementedException();
        }

        public ReplaceModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ReplaceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
