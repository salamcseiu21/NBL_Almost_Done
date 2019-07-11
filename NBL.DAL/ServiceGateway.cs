using System;
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
            try
            {
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
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
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
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
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
                        CellFiveCondition = reader["Condition5"].ToString(),
                        CellFourCondition = reader["Condition4"].ToString(),
                        CellOneCondition = reader["Condition1"].ToString(),
                        CellSixCondition = reader["Condition6"].ToString(),
                        CellThreeCondition = reader["Condition3"].ToString(),
                        CellTwoCondition = reader["Condition2"].ToString()
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
