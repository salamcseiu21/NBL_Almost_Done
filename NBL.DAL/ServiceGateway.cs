using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Orders;
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
                CommandObj.Parameters.AddWithValue("@IsInWarrantyPeriod", product.IsInWarrantyPeriod);
                CommandObj.Parameters.AddWithValue("@IsSoldInGracePeriod", product.IsSoldInGracePeriod);
                CommandObj.Parameters.AddWithValue("@TransactionRef", product.TransactionRef);
                CommandObj.Parameters.AddWithValue("@SpGrCellOne", product.SpGrCellOne);
                CommandObj.Parameters.AddWithValue("@SpGrCellTwo", product.SpGrCellTwo);
                CommandObj.Parameters.AddWithValue("@SpGrCellThree", product.SpGrCellThree);
                CommandObj.Parameters.AddWithValue("@SpGrCellFour", product.SpGrCellFour);
                CommandObj.Parameters.AddWithValue("@SpGrCellFive", product.SpGrCellFive);
                CommandObj.Parameters.AddWithValue("@SpGrCellSix", product.SpGrCellSix);
                CommandObj.Parameters.AddWithValue("@SpGrCellDiff", product.SpGrCellValueDifference);
                CommandObj.Parameters.AddWithValue("@CellOneConditionId", product.CellOneConditionId);
                CommandObj.Parameters.AddWithValue("@CellTwoConditionId", product.CellTwoConditionId);
                CommandObj.Parameters.AddWithValue("@CellThreeConditionId", product.CellThreeConditionId);
                CommandObj.Parameters.AddWithValue("@CellFourConditionId", product.CellFourConditionId);
                CommandObj.Parameters.AddWithValue("@CellFiveConditionId", product.CellFiveConditionId);
                CommandObj.Parameters.AddWithValue("@CellSixConditionId", product.CellSixConditionId);
                CommandObj.Parameters.AddWithValue("@OpenVoltage", product.OpenVoltage);
                CommandObj.Parameters.AddWithValue("@LoadVoltage", product.LoadVoltage);
                CommandObj.Parameters.AddWithValue("@VoltageRemarks", product.VoltageRemarks?? (object)DBNull.Value);
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
                CommandObj.Parameters.AddWithValue("@HasWarranty",product.HasWarranty);
                CommandObj.Parameters.AddWithValue("@ServiceBatteryDeliveryDate", product.ServiceBatteryDeliveryDate ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ServiceBatteryBarcode", product.ServiceBatteryBarcode?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ServiceBatteryReturnDate", product.ServiceBatteryReturnDate ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@RbdDate", product.RbdDate ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@SaleDate", product.SaleDate);
                CommandObj.Parameters.AddWithValue("@RbdBarcode", product.RbdBarcode?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ForwardToId", product.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardRemarks", product.ForwardRemarks?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@SpGrCellRemarks", product.SpGrCellRemarks ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@PrimaryTestResult", product.PrimaryTestResult ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@IsPassPrimaryTest", product.IsPassPrimaryTest);
                CommandObj.Parameters.AddWithValue("@DistributionPointId", product.DistributionPointId);
                CommandObj.Parameters.AddWithValue("@DistributionPoint", product.DistributionPoint ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@ClientId", product.ClientId);
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
            CommandObj.Parameters.AddWithValue("@Remarks", item.ForwardRemarks ?? (object)DBNull.Value);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            var i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;
        }
        public int ReceiveServiceProductTemp(WarrantyBatteryModel product)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ReceiveServiceProductTemp";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveDatetime", product.ReceiveDatetime);
                CommandObj.Parameters.AddWithValue("@BarcodeNo", product.Barcode);
                CommandObj.Parameters.AddWithValue("@ClientId", product.ClientId);
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@DelivaryRef", product.DelivaryRef);
                CommandObj.Parameters.AddWithValue("@ReceiveRef", product.ReceiveRef);
                CommandObj.Parameters.AddWithValue("@ReceiveByBranchId", product.ReceiveByBranchId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", product.TransactionRef);
                CommandObj.Parameters.AddWithValue("@ReplaceRef", product.ReplaceRef);
                CommandObj.Parameters.AddWithValue("@ReportByEmployeeId", product.ReportByEmployeeId);
                CommandObj.Parameters.AddWithValue("@EntryByUserId", product.EntryByUserId);
                CommandObj.Parameters.AddWithValue("@Status", product.Status);
                CommandObj.Parameters.AddWithValue("@IsActive", "Y");
                CommandObj.Parameters.AddWithValue("@Remarks", product.ReceiveRemarks);
                CommandObj.Parameters.AddWithValue("@IsManualEntry", product.IsManualEntry);
                CommandObj.Parameters.AddWithValue("@SaleDate", product.SaleDate);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

                return rowAffected;
            }
            catch (SqlException sqlException)
            {
               //var result= sqlException.Source;
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not Receive service product due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Receive service product", exception);
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
                        ProductCategoryName=reader["ProductCategoryName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ForwardedTo = reader["ForwardedTo"].ToString(),
                        ReceiveRef =DBNull.Value.Equals(reader["ReceiveRef"])? null:reader["ReceiveRef"].ToString(),
                        ReceiveByBranch = reader["ReceiveByBranch"].ToString(),
                        PrimaryTestResult = reader["PrimaryTestResult"].ToString()
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
                        VoltageRemarks = reader["VoltageRemarks"].ToString(),
                        CoverStatusId = Convert.ToInt32(reader["CoverStatusId"]),
                        ContainerStatusId = Convert.ToInt32(reader["ContainerStatusId"]),
                        PostStatusId = Convert.ToInt32(reader["PostStatusId"]),
                        ReceiveReport = reader["ReceiveReport"].ToString(),
                        OtherInformationRemarks = reader["OtherInformationRemarks"].ToString(),
                        ReportByEmployee = reader["EmployeeName"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ChargingSystem = reader["ChargingSystem"].ToString(),
                        ServicingStatus = reader["ServicingStatus"].ToString(),
                        EntryByUser = reader["EntryByUser"].ToString(),
                        ReceiveByBranch = reader["ReceiveByBranch"].ToString(),
                        SpGrRemarks =DBNull.Value.Equals(reader["SpGrCellRemarks"])? null:reader["SpGrCellRemarks"].ToString(),
                        SpGrCellValueDifference=Convert.ToDecimal(reader["SpGrCellDiff"]),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        PrimaryTestResult = reader["PrimaryTestResult"].ToString(),
                        HasWarranty = reader["HasWarranty"].ToString(),
                        RbdDate = DBNull.Value.Equals(reader["RbdDate"]) ? (DateTime ?)null : Convert.ToDateTime(reader["RbdDate"]),
                        RbdBarcode = reader["RbdBarcode"].ToString(),
                        ServiceBatteryBarcode = reader["ServiceBatteryBarcode"].ToString(),
                        ServiceBatteryDeliveryDate = DBNull.Value.Equals(reader["ServiceBatteryDeliveryDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ServiceBatteryDeliveryDate"]),
                        ServiceBatteryReturnDate = DBNull.Value.Equals(reader["ServiceBatteryReturnDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ServiceBatteryReturnDate"])

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
        public ViewReceivedServiceProduct GetDeliverableServiceProductById(long receivedId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliverableServiceProductById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveId", receivedId);
                ConnectionObj.Open();
                ViewReceivedServiceProduct product = new ViewReceivedServiceProduct();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ViewReceivedServiceProduct
                    {
                        ReceiveId = receivedId,
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ReceiveRef = reader["ReceiveRef"].ToString(),
                        ReceiveReport = reader["ReceiveReport"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ExpiryDate = DBNull.Value.Equals(reader["ExpiryDate"]) ? default(DateTime?) : Convert.ToDateTime(reader["ExpiryDate"]),
                        SaleDate = DBNull.Value.Equals(reader["SaleDate"]) ? default(DateTime?) : Convert.ToDateTime(reader["SaleDate"]),
                        BranchId = Convert.ToInt32(reader["ReceiveByBranchId"])
                        
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get deliverable Receive service product by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewTestPolicy GetTestPolicyByCategoryAndProductId(int testCategory, int productId)
        {
            try
            {


                CommandObj.CommandText = "UDSP_GetTestPolicyByCategoryAndProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CategoryId", testCategory);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                ViewTestPolicy policy = new ViewTestPolicy();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    policy.Ocv = Convert.ToDecimal(reader["OCV"]);
                    policy.LoadVoltage = Convert.ToDecimal(reader["LoadVoltage"]);
                    policy.SgDifference = Convert.ToDecimal(reader["SgDifference"]);
                }
                reader.Close();
                return policy;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get test policy by product id and category Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarIdAndBranchId(int forwardId, int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProductsByForwarIdAndBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ForwardId", forwardId);
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
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
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveRef"]) ? null : reader["ReceiveRef"].ToString(),
                        ReceiveByBranch = reader["ReceiveByBranch"].ToString(),
                        ForwardDatetime = Convert.ToDateTime(reader["ForwardDatetime"]),
                        ReceiveRemarks = reader["ReceiveReport"].ToString(),
                        DischargeReport = DBNull.Value.Equals(reader["DischargeReport"]) ? null : reader["DischargeReport"].ToString(),
                        ChargerReport = DBNull.Value.Equals(reader["ChargeReport"]) ? null : reader["ChargeReport"].ToString(),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ReceiveByBranchId = branchId,
                        PrimaryTestResult = reader["PrimaryTestResult"].ToString(),
                        HasWarranty = reader["HasWarranty"].ToString()

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Receive service product by forwardid and branchId", exception);
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
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveRef"]) ? null : reader["ReceiveRef"].ToString(),
                        ReceiveByBranch = reader["ReceiveByBranch"].ToString(),
                        ForwardDatetime = Convert.ToDateTime(reader["ForwardDatetime"]),
                        ReceiveRemarks = reader["ReceiveReport"].ToString(),
                        DischargeReport = DBNull.Value.Equals(reader["DischargeReport"]) ? null : reader["DischargeReport"].ToString(),
                        ChargerReport = DBNull.Value.Equals(reader["ChargeReport"]) ? null : reader["ChargeReport"].ToString(),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ReceiveByBranchId = Convert.ToInt32(reader["ReceiveByBranchId"]),
                        HasWarranty = reader["HasWarranty"].ToString()
                        
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
        public ICollection<Client> GetClientListByServiceForwardId(int forwardId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetClientListByServiceForwardId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ForwardToId", forwardId);
                ConnectionObj.Open();
                List<Client> clients = new List<Client>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        
                        ClientName = reader["ClientName"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"])

                    });
                }
                reader.Close();
                return clients;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect ClientList By Service  forwardid", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int ForwardServiceBatteryToDeistributionPoint(long receiveId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ForwardServiceBatteryToDeistributionPoint";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ReceiveId", receiveId);
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
                throw new Exception("Could not forward service product to distribution point", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByStatusAndBranchId(int status, int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProductsByStatusAndBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Status", status);
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                List<ViewReceivedServiceProduct> products = new List<ViewReceivedServiceProduct>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReceivedServiceProduct
                    {
                        ReceiveId = Convert.ToInt64(reader["ReceiveId"]),
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveRef"]) ? null : reader["ReceiveRef"].ToString(),
                        ReceiveRemarks = reader["ReceiveReport"].ToString(),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ExpiryDate = DBNull.Value.Equals(reader["ExpiryDate"]) ? default(DateTime?) :Convert.ToDateTime(reader["ExpiryDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"])
                        
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Receive service product by status and BranchId", exception);
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

        public int SaveCharegeReport(ChargeReportModel product)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveCharegeReport";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ParentId", product.ParentId);
                CommandObj.Parameters.AddWithValue("@BatteryReceiveId", product.BatteryReceiveId);
                CommandObj.Parameters.AddWithValue("@SpGrCellOne", product.SpGrCellOne);
                CommandObj.Parameters.AddWithValue("@SpGrCellTwo", product.SpGrCellTwo);
                CommandObj.Parameters.AddWithValue("@SpGrCellThree", product.SpGrCellThree);
                CommandObj.Parameters.AddWithValue("@SpGrCellFour", product.SpGrCellFour);
                CommandObj.Parameters.AddWithValue("@SpGrCellFive", product.SpGrCellFive);
                CommandObj.Parameters.AddWithValue("@SpGrCellSix", product.SpGrCellSix);
                CommandObj.Parameters.AddWithValue("@SpGrCellValueDiff", product.SpGrCellValueDifference);
                CommandObj.Parameters.AddWithValue("@CellOneConditionId", product.CellOneConditionId);
                CommandObj.Parameters.AddWithValue("@CellTwoConditionId", product.CellTwoConditionId);
                CommandObj.Parameters.AddWithValue("@CellThreeConditionId", product.CellThreeConditionId);
                CommandObj.Parameters.AddWithValue("@CellFourConditionId", product.CellFourConditionId);
                CommandObj.Parameters.AddWithValue("@CellFiveConditionId", product.CellFiveConditionId);
                CommandObj.Parameters.AddWithValue("@CellSixConditionId", product.CellSixConditionId);
                CommandObj.Parameters.AddWithValue("@OpenVoltage", product.OpenVoltage);
                CommandObj.Parameters.AddWithValue("@LoadVoltage", product.LoadVoltage);
                CommandObj.Parameters.AddWithValue("@VoltageRemarks", product.VoltageRemarks);
                CommandObj.Parameters.AddWithValue("@ReportByEmployeeId", product.ReportByEmployeeId);
                CommandObj.Parameters.AddWithValue("@EntryByUserId", product.EntryByUserId);
                CommandObj.Parameters.AddWithValue("@ForwardToId", product.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardRemarks", product.ForwardRemarks);
                CommandObj.Parameters.AddWithValue("@SpGrCellRemarks", product.SpGrCellRemarks);
                CommandObj.Parameters.AddWithValue("@IsPassChargeTest", product.IsPassChargeTest);
                CommandObj.Parameters.AddWithValue("@Report", product.Report);
                CommandObj.Parameters.Add("@ReportId", SqlDbType.Int);
                CommandObj.Parameters["@ReportId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var reportId = Convert.ToInt64(CommandObj.Parameters["@ReportId"].Value); 
                var rowAffected = SaveForwardDetails(product.ForwardDetails, product.BatteryReceiveId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }

                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not save charge report", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveDischargeReport(DischargeReportModel product)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveDischargeReport";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ParentId", product.ParentId);
                CommandObj.Parameters.AddWithValue("@BatteryReceiveId", product.BatteryReceiveId);
                CommandObj.Parameters.AddWithValue("@ReportByEmployeeId", product.ReportByEmployeeId);
                CommandObj.Parameters.AddWithValue("@EntryByUserId", product.EntryByUserId);
                CommandObj.Parameters.AddWithValue("@ForwardToId", product.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardRemarks", product.ForwardRemarks);
                CommandObj.Parameters.AddWithValue("@DischargeReport", product.DischargeReport);
                CommandObj.Parameters.AddWithValue("@DischargeRemarks", product.DischargeRemarks);
                CommandObj.Parameters.AddWithValue("@DischargeAmp", product.DischargeAmp);
                CommandObj.Parameters.AddWithValue("@BarckUpTime", product.BackUpTime);
                CommandObj.Parameters.AddWithValue("@RecommendedBarckUpTime", product.RecommendedBackUpTime);
                CommandObj.Parameters.AddWithValue("@Tv", product.Tv);
                CommandObj.Parameters.AddWithValue("@Lv", product.Lv);
                CommandObj.Parameters.Add("@ReportId", SqlDbType.Int);
                CommandObj.Parameters["@ReportId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var reportId = Convert.ToInt64(CommandObj.Parameters["@ReportId"].Value);
                var rowAffected = SaveForwardDetails(product.ForwardDetails, product.BatteryReceiveId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }

                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not save charge report", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ChargeReportModel GetChargeReprortByReceiveId(long id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetChargeReprortByReceiveId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveId", id);
                ChargeReportModel model=null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model=new ChargeReportModel
                    {
                        BatteryReceiveId = id,
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
                        OpenVoltage = Convert.ToDecimal(reader["OpenVoltage"]),
                        LoadVoltage = Convert.ToDecimal(reader["LoadVoltage"]),
                        VoltageRemarks = DBNull.Value.Equals(reader["VoltageRemarks"])? null: reader["VoltageRemarks"].ToString(),
                        ForwardRemarks = DBNull.Value.Equals(reader["ForwardRemarks"]) ? null : reader["ForwardRemarks"].ToString(),
                        SpGrCellRemarks = DBNull.Value.Equals(reader["SpGrCellRemarks"]) ? null : reader["SpGrCellRemarks"].ToString(),
                        ReportByEmp=reader["ReportBy"].ToString(),
                        SpGrCellValueDifference = Convert.ToDecimal(reader["SpGrCellDiff"])

                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get charge report by receive Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public DischargeReportModel GetDisChargeReprortByReceiveId(long id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDisChargeReprortByReceiveId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveId", id);
                DischargeReportModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model = new DischargeReportModel
                    {
                        BatteryReceiveId = id,
                        Lv = Convert.ToDecimal(reader["Lv"]),
                        Tv = Convert.ToDecimal(reader["Tv"]),
                        BackUpTime = Convert.ToDecimal(reader["BackUpTime"]),
                        RecommendedBackUpTime = Convert.ToDecimal(reader["RecommendedBackUpTime"]),
                        DischargeAmp =DBNull.Value.Equals(reader["DischargeAmp"])? default(decimal): Convert.ToDecimal(reader["DischargeAmp"]),
                        DischargeReport = DBNull.Value.Equals(reader["DischargeReport"]) ? null : reader["DischargeReport"].ToString(),
                        DischargeRemarks = DBNull.Value.Equals(reader["DischargeRemarks"]) ? null : reader["DischargeRemarks"].ToString(),
                        ForwardRemarks = DBNull.Value.Equals(reader["ForwardRemarks"]) ? null : reader["ForwardRemarks"].ToString(),
                        ReportByEmp = reader["ReportBy"].ToString()

                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get discharge report by receive Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewSoldProduct> GetAllSoldProducts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetSoldProductInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewSoldProduct> products = new List<ViewSoldProduct>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewSoldProduct
                    {
                       
                        BarCode = reader["Barcode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                       DeliveryRef = reader["DeliveryRef"].ToString(),
                       OrderRef = reader["OrderRef"].ToString(),
                       ClientName = reader["ClientInfo"].ToString(),
                       SaleDate = Convert.ToDateTime(reader["SaleDate"]),
                        BranchName=reader["BranchName"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        FolioEntryDate = Convert.ToDateTime(reader["UpdateDate"]),
                        FolioEntryBy = reader["SaleDateUpdaeBY"].ToString()

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get sold folio entried products", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveApprovalInformation(int userId, ForwardDetails item)
        {
            try
            {
                CommandObj.CommandText = "UDSP_SaveForwardDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ReceiveId", item.ReceiveId);
                CommandObj.Parameters.AddWithValue("@ForwardFromId", item.ForwardFromId);
                CommandObj.Parameters.AddWithValue("@ForwardToId", item.ForwardToId);
                CommandObj.Parameters.AddWithValue("@ForwardDateTime", item.ForwardDateTime);
                CommandObj.Parameters.AddWithValue("@UserId", item.UserId);
                CommandObj.Parameters.AddWithValue("@Remarks", item.ForwardRemarks);
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
                throw new Exception("Could not save approval information", exception);
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
