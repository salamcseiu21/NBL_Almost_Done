using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
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
                n = SaveDispatchItems(model.DispatchModels,dispatchId,inventoryMasterId);
            }
            return n;
        }

        private int SaveDispatchItems(IEnumerable<ViewDispatchModel> products, long dispatchId,long inventoryMasterId)  
        {
            int i = 0;
            foreach (var item in products)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveDispatchItems";
                CommandObj.CommandType = CommandType.StoredProcedure;
             
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
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