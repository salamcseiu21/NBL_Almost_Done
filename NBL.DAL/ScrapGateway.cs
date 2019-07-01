using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Scraps;
using NBL.Models.Logs;

namespace NBL.DAL
{
    public class ScrapGateway:DbGateway,IScrapGateway
    {


        public int SaveScrap(ScrapModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveScrapProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", model.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.TransactionRef);
                CommandObj.Parameters.AddWithValue("@TransactionType", model.TransactionType);
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                long masterId = Convert.ToInt32(CommandObj.Parameters["@MasterId"].Value);
                var rowAffected = SaveScrapProductToScrapInventory(model, masterId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                Log.WriteErrorLog(exception);
                throw new Exception("Could not save Scrap product Info",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public bool IsThisBarcodeExitsInScrapInventory(string barcode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_IsThisBarcodeExitsInScrapInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Barcode", barcode);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                int rowAffected = 0;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                }
                return rowAffected>0;
            }
            catch (Exception exception)
            {
             
                Log.WriteErrorLog(exception);
                throw new Exception("Could not found Scrap barcode", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }


        private int SaveScrapProductToScrapInventory(ScrapModel model, long masterId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in model.ScannedProducts)
            {

                CommandObj.CommandText = "UDSP_SaveScrapProductToScrapInventoryDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ScrapId", masterId);
                CommandObj.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2,3)));
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveScrapItemWithQuantity(model, masterId);
            }
            return n;
        }
        private int SaveScrapItemWithQuantity(ScrapModel model, long masterId)
        {
            int i = 0;

            foreach (var item in model.ScannedProducts.GroupBy(n => n.ProductId))
            {

                CommandObj.CommandText = "UDSP_SaveScrapItemWithQuantity";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", item.Key);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Count());
                CommandObj.Parameters.AddWithValue("@ScrapId", masterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }







        public int Add(ScrapModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(ScrapModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(ScrapModel model)
        {
            throw new NotImplementedException();
        }

        public ScrapModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ScrapModel> GetAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
