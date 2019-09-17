using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Masters;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;

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
                CommandObj.Parameters.AddWithValue("@TripStatus", dispatchModel.TripModel.Status);

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
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get dispatch deatils Info by dispatchId", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        //-------------Save General Requisition  From Factory--------------------------------
        public int SaveDeliveredGeneralRequisition(List<ScannedProduct> scannedProducts, Delivery aDelivery)
        {

            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "UDSP_SaveDeliveredRequisitionFromFactory";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@IsOwnTransporatoion", aDelivery.IsOwnTransport);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverPhone", aDelivery.DriverPhone ?? "N/A");
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo ?? "N/A");
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count);
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.BigInt);
                CommandObj.Parameters.Add("@InventoryMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters["@InventoryMasterId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;

                CommandObj.ExecuteNonQuery();
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);
                var accountMasterId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                var inventoryMasterId = Convert.ToInt32(CommandObj.Parameters["@InventoryMasterId"].Value);
                int rowAffected = SaveDeliveredRequisitionDetails(scannedProducts, aDelivery, inventoryMasterId, deliveryId);

                int accountAffected = 0;
                if (rowAffected > 0)
                {

                    var financial = aDelivery.FinancialTransactionModel;

                    for (int i = 1; i <= 2; i++)
                    {
                        if (i == 1)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.ExpenceCode, financial.ExpenceAmount, accountMasterId, "Expence code Debit..");
                        }
                        else if (i == 2)
                        {


                            accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.InventoryCode, financial.InventoryAmount * (-1), accountMasterId, "Inventory code Credit..");
                        }


                    }
                }


                if (accountAffected > 0)
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
                throw new Exception("Could not save delivered General Requisition", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetDespatchedBarcodeByDespatchId(long dispatchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDespatchBarcodeByDespatchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                List<ViewProduct> products=new List<ViewProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get despatched barcode", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveDeliveredRequisitionDetails(List<ScannedProduct> scannedProducts, Delivery aDelivery, int inventoryId, int deliveryId)
        {
            int n = 0;
            int i = 0;
            foreach (var item in scannedProducts)
            {

                CommandObj.CommandText = "UDSP_SaveDeliveredRequisitionDetailsFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryId);
                CommandObj.Parameters.AddWithValue("@Status", 2);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                n += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            if (n > 0)
            {
                i = SaveDeliveredItemWithQuantityToFactory(scannedProducts, inventoryId,aDelivery);
            }

            return i;
        }

        private int SaveDeliveredItemWithQuantityToFactory(List<ScannedProduct> deliveredProducts, long inventoryId, Delivery aDelivery)
        {
            int i = 0;
            var groupBy = deliveredProducts.GroupBy(n => n.ProductId);
            foreach (IGrouping<int, ScannedProduct> scannedProducts in groupBy)
            {
                CommandObj.CommandText = "UDSP_SaveDeliveredItemWithQuantityToFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", scannedProducts.Key);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count());
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryId);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }


        private int SaveFinancialTransactionToAccountsDetails(string transactionType, string accountCode, decimal amounts, int accountMasterId, string explanation)
        {

            var i = 0;
            CommandObj.CommandText = "UDSP_SaveFinancialTransactionToAccountsDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", accountCode);
            CommandObj.Parameters.AddWithValue("@TransactionType", transactionType);
            CommandObj.Parameters.AddWithValue("@Amount", amounts);
            CommandObj.Parameters.AddWithValue("@Explanation", explanation);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
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