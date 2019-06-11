using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Masters;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Deliveries;

namespace NBL.DAL
{
    public class FactoryDeliveryGateway : DbGateway,IFactoryDeliveryGateway
    {
        public int SaveDispatchInformation(DispatchModel dispatchModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveDispatchInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchDate", dispatchModel.DispatchDate);
                CommandObj.Parameters.AddWithValue("@TripId", dispatchModel.TripModel.TripId);

                //CommandObj.Parameters.AddWithValue("@RequisitionId", dispatchModel.TripModel.RequisitionId);
                //CommandObj.Parameters.AddWithValue("@ToBranchId", dispatchModel.TripModel.ToBranchId);
                //CommandObj.Parameters.AddWithValue("@ProductId", dispatchModel.TripModel.ProuctId);

                CommandObj.Parameters.AddWithValue("@TransactionRef", dispatchModel.TripModel.TripRef);
                CommandObj.Parameters.AddWithValue("@Quantity", dispatchModel.ScannedProducts.ToList().Count);
                CommandObj.Parameters.AddWithValue("@DispatchRef", dispatchModel.DispatchRef);
                CommandObj.Parameters.AddWithValue("@CompanyId", dispatchModel.CompanyId);
                CommandObj.Parameters.AddWithValue("@DispatchByUserId", dispatchModel.DispatchByUserId);
                CommandObj.Parameters.Add("@DispatchId", SqlDbType.BigInt);
                CommandObj.Parameters["@DispatchId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@InventoryMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@InventoryMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                long dispatchId = Convert.ToInt64(CommandObj.Parameters["@DispatchId"].Value);
                long inventoryMasterId = Convert.ToInt64(CommandObj.Parameters["@InventoryMasterId"].Value);

                int rowAffected = SaveDispatchInformationDetails(dispatchModel, dispatchId,inventoryMasterId);
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
                Log.WriteErrorLog(exception);
                sqlTransaction.Rollback();
                throw new Exception("Could not Save dispatch Info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

       

        private int SaveDispatchInformationDetails(DispatchModel model, long dispatchId,long inventoryMasterId)
        {
            int i=0;
            int n = 0;
            foreach (var item in model.ScannedProducts) 
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveDispatchInformationDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
               
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2,3)));
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                CommandObj.Parameters.AddWithValue("@ProductBarCode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@FactoryInventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.DispatchRef);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i +=Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveDispatchItems(model,dispatchId,inventoryMasterId);
            }
            return n;
        }

        private int SaveDispatchItems(DispatchModel model, long dispatchId,long inventoryMasterId)  
        {
            int i = 0;
            var scannProducts = model.ScannedProducts;
            foreach (var item in model.DispatchModels)
            {
                var qty= scannProducts.Count(n => n.ProductId.Equals(item.ProductId));
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveDispatchItems";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", qty);
                CommandObj.Parameters.AddWithValue("@ToBranchId", item.ToBranchId);
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }


        public DispatchModel GetDispatchByDispatchId(long dispatchId)
        {
            try
            {

                CommandObj.CommandText = "UDSP_GetDispatchByDispatchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                DispatchModel dispatch=null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    dispatch=new DispatchModel
                    {
                        DispatchId = dispatchId,
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        TripId = Convert.ToInt64(reader["TripId"]),
                        DispatchByUserId = Convert.ToInt32(reader["DispatchByUserId"]),
                        DispatchDate = Convert.ToDateTime(reader["DispatchDate"]),
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"]),
                        DispatchRef = reader["DispatchRef"].ToString(),
                        IsCanclled = reader["IsCancelled"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"])?null:reader["Remarks"].ToString()
                    };
                }
                reader.Close();
                return dispatch;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get dispatch Info by dispatchId", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewDispatchModel> GetDispatchDetailsByDispatchId(long dispatchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDispatchDetailsByDispatchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                ConnectionObj.Open();
                List<ViewDispatchModel> dispatchModels=new List<ViewDispatchModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    dispatchModels.Add(new ViewDispatchModel
                    {
                       ProductName = reader["ProductName"].ToString(),
                       ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                       Quantity = Convert.ToInt32(reader["Quantity"]),
                       DispatchItemId = Convert.ToInt64(reader["DispatchItemsId"]),
                       SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                       ProductCategory = new ProductCategory
                       {
                           ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                           ProductCategoryName = reader["ProductCategoryName"].ToString(), 
                       }
                    });
                }
                reader.Close();
                return dispatchModels;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get dispatch deatils Info by dispatchId", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(Delivery model)
        {
            throw new NotImplementedException();
        }

        public int Update(Delivery model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Delivery model)
        {
            throw new NotImplementedException();
        }

        public Delivery GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}