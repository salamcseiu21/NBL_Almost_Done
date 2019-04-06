using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Returns;

namespace NBL.DAL
{
   public class ProductReturnGateway:DbGateway,IProductReturnGateway 
    {
        public int SaveReturnProduct(ReturnModel returnModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveSalesReturnProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnNo", returnModel.ReturnNo);
                CommandObj.Parameters.AddWithValue("@SalesReturnRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@BranchId", returnModel.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", returnModel.CompanyId);
                CommandObj.Parameters.AddWithValue("@Remarks", returnModel.Remarks?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReturnIssueByUserId", returnModel.ReturnIssueByUserId);
                CommandObj.Parameters.AddWithValue("@TotalQuantity", returnModel.Products.Sum(n=>n.Quantity));
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var masterId = Convert.ToInt64(CommandObj.Parameters["@MasterId"].Value);
                var rowAffected = SaveReturnProductDetails(returnModel,masterId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                else
                {
                    sqlTransaction.Rollback();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save return products",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveReturnProductDetails(ReturnModel returnModel, long masterId)
        {
            int i = 0;
            foreach (var item in returnModel.Products)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveSalesReturnDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnId", masterId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@DeliveryId", item.DeliveryId);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", item.DeliveryRef);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public long GetMaxSalesReturnNoByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxSalesReturnNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                long maxSl = 0;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSl = Convert.ToInt64(reader["MaxSl"]);
                }
                reader.Close();
                return maxSl;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max sales return no", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public long GetMaxSalesReturnRefByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxSalesReturnRefByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                long maxRefNo = 0; 
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxRefNo = Convert.ToInt64(reader["MaxRefNo"]);
                }
                reader.Close();
                return maxRefNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max sales return ref no", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

       

        public int Add(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public ReturnModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ReturnModel> GetAll()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllSalesReturn";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ReturnModel> models=new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId=Convert.ToInt64(reader["SalesReturnId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["SalesReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                        Remarks = reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"])
                        
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect sales returns",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
               
            }
        }
        public ICollection<ReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetSalesReturnDetailsBySalesReturnId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnId", salesReturnId);
                List<ReturnDetails> models = new List<ReturnDetails>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnDetails 
                    {
                        SalesReturnId = Convert.ToInt64(reader["SalesReturnId"]),
                        SalesReturnNo=Convert.ToInt64(reader["SalesReturnNo"]),
                        ReturnDateTime = Convert.ToDateTime(reader["ReturnDateTime"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SalsesReturnDetailsId = Convert.ToInt64(reader["SalsesReturnDetailsId"]),
                        SalesAdminUserId = Convert.ToInt32(reader["SalesAdminUserId"]),
                        ApproveBySalesAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        ApproveByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        OrderByUserId = Convert.ToInt32(reader["OrderByUserId"]),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDateTime"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveredDateTime"])

                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect sales returns details", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

    }
}
