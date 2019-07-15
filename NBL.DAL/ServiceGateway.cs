﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Services;

namespace NBL.DAL
{
   public class ServiceGateway:DbGateway,IServiceGateway
    {

        public int ReceiveServiceProduct(WarrantyBatteryModel product)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_ReceiveServiceProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveDatetime", product.ReceiveDatetime);
                CommandObj.Parameters.AddWithValue("@BarcodeNo", product.Barcode);
                CommandObj.Parameters.AddWithValue("@DelivaryRef", product.DelivaryRef);
                CommandObj.Parameters.AddWithValue("@ReceiveRef", product.ReceiveRef);
                CommandObj.Parameters.AddWithValue("@ReceiveByBranchId", product.ReceiveByBranchId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", product.TransactionRef);
                CommandObj.Parameters.AddWithValue("@SpGrCellOne", product.SpGrCellOne);
                CommandObj.Parameters.AddWithValue("@SpGrCellTwo", product.SpGrCellTwo);
                CommandObj.Parameters.AddWithValue("@SpGrCellThree", product.SpGrCellThree);
                CommandObj.Parameters.AddWithValue("@SpGrCellFour", product.SpGrCellFour);
                CommandObj.Parameters.AddWithValue("@SpGrCellFive", product.SpGrCellFive);
                CommandObj.Parameters.AddWithValue("@SpGrCellSix", product.SpGrCellSix);
                CommandObj.Parameters.AddWithValue("@CellOneConditionId", product.CellOneConditionId);
                CommandObj.Parameters.AddWithValue("@CellTwoConditionId", product.CellTwoConditionId);
                CommandObj.Parameters.AddWithValue("@CellThreeConditionId", product.CellThreeConditionId);
                CommandObj.Parameters.AddWithValue("@CellFourConditionId", product.CellFourConditionId);
                CommandObj.Parameters.AddWithValue("@CellFiveConditionId", product.CellFiveConditionId);
                CommandObj.Parameters.AddWithValue("@CellSixConditionId", product.CellSixConditionId);
                CommandObj.Parameters.AddWithValue("@OpenVoltage", product.OpenVoltage);
                CommandObj.Parameters.AddWithValue("@LoadVoltage", product.LoadVoltage);
                CommandObj.Parameters.AddWithValue("@CellRemarks", product.CellRemarks);
                CommandObj.Parameters.AddWithValue("@CoverStatusId", product.CoverStatusId);
                CommandObj.Parameters.AddWithValue("@ContainerStatusId", product.ContainerStatusId);
                CommandObj.Parameters.AddWithValue("@PostStatusId", product.PostStatusId);
                CommandObj.Parameters.AddWithValue("@ServicingStatusId", product.ServicingStatusId);
                CommandObj.Parameters.AddWithValue("@ChargingSystemId", product.ChargingSystemId);
                CommandObj.Parameters.AddWithValue("@AppUsedFor", product.AppUsedFor?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@AppCapacity", product.AppCapacity ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@OtherInformationRemarks", product.OtherInformationRemarks ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReceiveReport", product.ReceiveReport ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReportByEmployeeId", product.ReportByEmployeeId);
                CommandObj.Parameters.AddWithValue("@EntryByUserId", product.EntryByUserId);
                CommandObj.Parameters.AddWithValue("@Status", product.Status);
                CommandObj.Parameters.AddWithValue("@IsActive", "Y");
                CommandObj.Parameters.AddWithValue("@ServiceBatteryDeliveryDate", product.ServiceBatteryDeliveryDate);
                CommandObj.Parameters.AddWithValue("@ServiceBatteryBarcode", product.ServiceBatteryBarcode?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ServiceBatteryReturnDate", product.ServiceBatteryReturnDate);
                CommandObj.Parameters.AddWithValue("@RbdDate", product.RbdDate);
                CommandObj.Parameters.AddWithValue("@RbdBarcode", product.RbdBarcode?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ForwardToId", product.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardRemarks", product.ForwardRemarks);
                CommandObj.Parameters.AddWithValue("@DistributionPointId", product.DistributionPointId);
                CommandObj.Parameters.AddWithValue("@DistributionPoint", product.DistributionPoint);
                CommandObj.Parameters.AddWithValue("@SpGrCellRemarks", product.SpGrCellRemarks);
                CommandObj.Parameters.Add("@ReceiveId", SqlDbType.Int);
                CommandObj.Parameters["@ReceiveId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var receiveId = Convert.ToInt64(CommandObj.Parameters["@ReceiveId"].Value);
                var rowAffected = SaveForwardDetails(product.ForwardDetails, receiveId);
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
                throw new Exception("Could not Receive service product",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveForwardDetails(ForwardDetails item, long receiveId)
        {
            CommandObj.CommandText = "UDSP_SaveForwardDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@ReceiveId", receiveId);
            CommandObj.Parameters.AddWithValue("@ForwardFromId", item.ForwardFromId);
            CommandObj.Parameters.AddWithValue("@ForwardToId", item.ForwardToId);
            CommandObj.Parameters.AddWithValue("@ForwardDateTime", item.ForwardDateTime);
            CommandObj.Parameters.AddWithValue("@UserId", item.UserId);
            CommandObj.Parameters.AddWithValue("@Remarks", item.ForwardRemarks);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            var i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;
        }

        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProducts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewReceivedServiceProduct> products=new List<ViewReceivedServiceProduct>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReceivedServiceProduct
                    {
                        ReceiveId = Convert.ToInt64(reader["ReceiveId"]),
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        AppCapacity = reader["AppCapacity"].ToString(),
                        AppUsedFor = reader["AppUsedFor"].ToString(),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategoryName=reader["ProductCategoryName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ForwardedTo = reader["ForwardedTo"].ToString(),
                        ReceiveRef =DBNull.Value.Equals(reader["ReceiveRef"])? null:reader["ReceiveRef"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Receive service product", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public long GetMaxWarrantyProductReceiveSlNoByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxWarrantyProductReceiveSlNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                long maxSl = 0;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSl = Convert.ToInt64(reader["MaxSlNo"]);
                }
                reader.Close();
                return maxSl;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get max Receive service product SL no", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProductById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveId", receiveId);
                ConnectionObj.Open();
                ViewReceivedServiceProduct product = new ViewReceivedServiceProduct();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    product = new ViewReceivedServiceProduct
                    {
                        ReceiveId = Convert.ToInt64(reader["ReceiveId"]),
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        AppCapacity = reader["AppCapacity"].ToString(),
                        AppUsedFor = reader["AppUsedFor"].ToString(),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveRef"]) ? null : reader["ReceiveRef"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        CellOneConditionId = Convert.ToInt32(reader["CellOneConditionId"]),
                        CellTwoConditionId = Convert.ToInt32(reader["CellTwoConditionId"]),
                        CellThreeConditionId = Convert.ToInt32(reader["CellThreeConditionId"]),
                        CellFourConditionId = Convert.ToInt32(reader["CellFourConditionId"]),
                        CellFiveConditionId = Convert.ToInt32(reader["CellFiveConditionId"]),
                        CellSixConditionId = Convert.ToInt32(reader["CellSixConditionId"]),
                        SpGrCellOne = Convert.ToDecimal(reader["SpGrCellOne"]),
                        SpGrCellTwo = Convert.ToDecimal(reader["SpGrCellTwo"]),
                        SpGrCellThree = Convert.ToDecimal(reader["SpGrCellThree"]),
                        SpGrCellFour = Convert.ToDecimal(reader["SpGrCellFour"]),
                        SpGrCellFive = Convert.ToDecimal(reader["SpGrCellFive"]),
                        SpGrCellSix = Convert.ToDecimal(reader["SpGrCellSix"]),
                        ForwardedToId = Convert.ToInt32(reader["ForwardToId"]),
                        OpenVoltage = Convert.ToDecimal(reader["OpenVoltage"]),
                        LoadVoltage = Convert.ToDecimal(reader["LoadVoltage"]),
                        CellRemarks = reader["CellRemarks"].ToString(),
                        CoverStatusId = Convert.ToInt32(reader["CoverStatusId"]),
                        ContainerStatusId = Convert.ToInt32(reader["ContainerStatusId"]),
                        PostStatusId = Convert.ToInt32(reader["PostStatusId"]),
                        ReceiveReport = reader["ReceiveReport"].ToString(),
                        OtherInformationRemarks = reader["OtherInformationRemarks"].ToString(),
                        ReportByEmployee = reader["EmployeeName"].ToString(),
                        ChargingSystem = reader["ChargingSystem"].ToString(),
                        ServicingStatus = reader["ServicingStatus"].ToString()

                        
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get Receive service product by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarId(int forwardId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProductsByForwarId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ForwardId", forwardId);
                ConnectionObj.Open();
                List<ViewReceivedServiceProduct> products = new List<ViewReceivedServiceProduct>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReceivedServiceProduct
                    {
                        ReceiveId = Convert.ToInt64(reader["ReceiveId"]),
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        AppCapacity = reader["AppCapacity"].ToString(),
                        AppUsedFor = reader["AppUsedFor"].ToString(),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        ForwardedTo = reader["ForwardedTo"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveRef"]) ? null : reader["ReceiveRef"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Receive service product by forwardid", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int ForwardServiceBattery(ForwardDetails model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_SaveForwardDetailsAfterReceive";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ReceiveId", model.ReceiveId);
                CommandObj.Parameters.AddWithValue("@Remarks", model.ForwardRemarks);
                CommandObj.Parameters.AddWithValue("@ForwardFromId", model.ForwardFromId);
                CommandObj.Parameters.AddWithValue("@ForwardToId", model.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardDateTime", model.ForwardDateTime);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return i;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not save service product forward info", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public WarrantyBatteryModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyBatteryModel> GetAll()
        {
            throw new NotImplementedException();
        }

       
    }
}
