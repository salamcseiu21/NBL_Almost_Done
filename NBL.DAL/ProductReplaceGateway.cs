using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels;

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
                CommandObj.Parameters.AddWithValue("@ReplaceNo", model.ReplaceRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.ReplaceRef);
                CommandObj.Parameters.AddWithValue("@BranchId", model.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
               
                var masterId = Convert.ToInt64(CommandObj.Parameters["@MasterId"].Value);
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
