using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Transports;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Reports;

namespace NBL.DAL
{
    public class DeliveryGateway:DbGateway,IDeliveryGateway
    {
        public int ChangeOrderStatusByManager(Order aModel)
        {
            try
            {

                CommandObj.CommandText = "spChangeOrderStatusByManager";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderAndTransctionId", aModel.OrderId);
                CommandObj.Parameters.AddWithValue("@Status ", aModel.Status);
                CommandObj.Parameters.AddWithValue("@DeliveryDate", aModel.DeliveryDateTime);
                CommandObj.Parameters.AddWithValue("@DeliveryOrRcvUserId", aModel.DeliveredOrReceiveUserId);
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
                throw new Exception("Could not Change the Order status", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Delivery GetOrderByDeliveryId(long deliveryId)
        {
            try
            {
                CommandObj.CommandText = "spGetOrderByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Delivery aModel = new Delivery();
                if (reader.Read())
                {
                    aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["SysDateTime"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"])
                        }
                    };
                }
                reader.Close();
                return aModel;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Order by delivery Id due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Order by delivery Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllInvoiceListByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<DeliveryModel> orders = new List<DeliveryModel>();
                while (reader.Read())
                {
                    DeliveryModel aModel = new DeliveryModel
                    {
                        InventoryId = Convert.ToInt32(reader["BranchInventoryId"]),
                        BranchId = Convert.ToInt32(reader["ToBranchId"]),
                        Invoice = reader["InvoiceNo"].ToString(),
                        TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                        Transactionid = Convert.ToInt32(reader["Transactionid"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Status = Convert.ToInt32(reader["Status"])
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Invoiced Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Invoiced Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(long deliveryId)
        {
            try
            {

                CommandObj.CommandText = "spGetDeliveredOrderDetailsByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<DeliveryDetails> orders = new List<DeliveryDetails>();
                while (reader.Read())
                {
                    var aModel = new DeliveryDetails
                    {
                        DeliveryId = deliveryId,
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Quantity = reader["ProductWishDeliveredQty"] is DBNull ? 0 : Convert.ToInt32(reader["ProductWishDeliveredQty"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        //ProductBarCode = reader["ProductBarCode"].ToString(),
                        CategoryId=Convert.ToInt32(reader["CategoryId"]),
                        CategoryName=reader["ProductCategoryName"].ToString()
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered details Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrders()
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrders";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        InvoiceId=Convert.ToInt32(reader["InvoiceId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime=Convert.ToDateTime(reader["SysDateTime"]),
                        DriverName = reader["DriverName"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        ClientInfo = reader["ClientInfo"].ToString()
                        
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId",branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = branchId,
                        FromBranchId = branchId,
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        Quantity = Convert.ToInt32(reader["DeliveredQuantity"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        },
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                    Address = reader["Address"].ToString(),
                Phone = reader["Phone"].ToString(),
                    AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                    Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                   ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                    ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                    NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                    Active = reader["Active"].ToString(),
                   CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                    MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                    TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                    RegionId = Convert.ToInt32(reader["RegionId"]),
                   SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                    BranchId = Convert.ToInt32(reader["BranchId"]),
                    ClientType = new ClientType
                    {
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                    }
                }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,int deliveredByUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByBranchCompanyAndUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", deliveredByUserId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = branchId,
                        FromBranchId = branchId,
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by branch,Company and User Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef)
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrderByInvoiceRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceRef",invoiceRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductId=Convert.ToInt32(reader["ProductId"]),
                        Quantity=Convert.ToInt32(reader["Quantity"])
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Order by Invoice ref", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductId(long deliveryId, int productId)
        {
            try
            {

                CommandObj.CommandText = "spGetDeliveredProductsByDeliveryIdAndProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.AddWithValue("@productId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    ViewProduct aModel = new ViewProduct
                    {
                        
                        ProductId = productId,
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    };
                    products.Add(aModel);
                }
                reader.Close();
                return products;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered products due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered products Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDeliveredOrderModel> GetDeliveredOrderByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliveryInfoByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                List<ViewDeliveredOrderModel> refModels=new List<ViewDeliveredOrderModel>();
                ConnectionObj.Open();

                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    refModels.Add(new ViewDeliveredOrderModel
                    {
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        DistributorName = reader["DistributorName"].ToString(),
                        DeliveredQty = Convert.ToInt32(reader["DeliveredQuantity"]),
                        DeliveryStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DriverName = reader["DriverName"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        ApproveByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        NsmName = reader["NsmName"].ToString(),
                        SalesAdminName = reader["SalesAdminName"].ToString(),
                        ApproveBySalesAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        SalesAdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        OrderByUserId = Convert.ToInt32(reader["OrderByUserId"]),
                        SalesPersonName = reader["SalesPersonName"].ToString(),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDateTime"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientCode = DBNull.Value.Equals(reader["ClientCode"])?null:reader["ClientCode"].ToString(),
                        ClientId = clientId,
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"])
                    });
                }
                reader.Close();
                return refModels;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivery ref due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivery ref", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDeliveredOrderModel> GetDeliveryDetailsInfoByDeliveryId(long deliveryId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliveryDetailsInfoByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                List<ViewDeliveredOrderModel> models = new List<ViewDeliveredOrderModel>();
                ConnectionObj.Open();

                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewDeliveredOrderModel
                    {
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        DistributorName = reader["DistributorName"].ToString(),
                        DeliveredQty = Convert.ToInt32(reader["DeliveredQuantity"]),
                        DeliveryStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DriverName = reader["DriverName"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        ApproveByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        NsmName = reader["NsmName"].ToString(),
                        SalesAdminName = reader["SalesAdminName"].ToString(),
                        ApproveBySalesAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        SalesAdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        OrderByUserId = Convert.ToInt32(reader["OrderByUserId"]),
                        SalesPersonName = reader["SalesPersonName"].ToString(),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDateTime"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientCode = DBNull.Value.Equals(reader["ClientCode"]) ? null : reader["ClientCode"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategoryName =DBNull.Value.Equals(reader["ProductCategoryName"])?null: reader["ProductCategoryName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        VatAmount = Convert.ToDecimal(reader["VatAmount"]),
                        UnitDiscount = Convert.ToDecimal(reader["UnitDiscount"]),
                        SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                    });
                }
                reader.Close();
                return models;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivery details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivery details", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyAndUserId(int distributionPointId, int companyId,
            int userId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByDistributionPointCompanyAndUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DistributionPointId", distributionPointId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", userId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        }
                        ,
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                            Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                            ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                            ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                            NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                            Active = reader["Active"].ToString(),
                            CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                            MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                            TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            BranchId = Convert.ToInt32(reader["BranchId"]),
                            Outstanding = DBNull.Value.Equals(reader["Outstanding"]) ? default(decimal): Convert.ToDecimal(reader["Outstanding"]),
                    ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by branch,Company and User Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }


      
        public IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryIdFromFactory(int deliveryId)
        {
            try
            {

                CommandObj.CommandText = "spGetDeliveredOrderDetailsByDeliveryIdFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<DeliveryDetails> orders = new List<DeliveryDetails>();
                while (reader.Read())
                {
                    var aModel = new DeliveryDetails
                    {
                        DeliveryId = deliveryId,
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Quantity = reader["DeliveredQuantity"] is DBNull ? 0 : Convert.ToInt32(reader["DeliveredQuantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        //ProductBarCode = reader["ProductBarCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["ProductCategoryName"].ToString()
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered details Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewProduct> GetDeliveredProductsByDeliveryIdAndProductIdFromFactory(int deliveryId, int productId)
        {

            try
            {

                CommandObj.CommandText = "spGetDeliveredProductsByDeliveryIdAndProductIdFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.AddWithValue("@productId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    ViewProduct aModel = new ViewProduct
                    {

                        ProductId = productId,
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    };
                    products.Add(aModel);
                }
                reader.Close();
                return products;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered products due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered products Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewReplaceDetailsModel> GetDeliveredReplaceDetailsByDeliveryId(int deliveryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointCompanyDateAndUserId(int distributionPointId, int companyId, DateTime date,
            int userId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByDistributionPointCompanyDateAndUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DistributionPointId", distributionPointId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@DeliveryDate", date);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", userId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        },
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                            Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                            ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                            ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                            NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                            Active = reader["Active"].ToString(),
                            CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                            MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                            TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            BranchId = Convert.ToInt32(reader["BranchId"]),
                            Outstanding = DBNull.Value.Equals(reader["Outstanding"]) ? default(decimal) : Convert.ToDecimal(reader["Outstanding"]),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by branch,Company and User Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByDistributionPointAndCompanyId(int branchId, int companyId)
        {
            try
            {
               // CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByBranchCompanyAndUserId";
                CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByDistributionPointAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DistributionPointId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = branchId,
                        FromBranchId = branchId,
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        },
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by deistribution point and Company", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewProduct> GetDeliveredProductListByTransactionRef(string deliveryRef)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetDeliveredProductsByDelivereRef";
                CommandObj.Parameters.AddWithValue("@TransactionRef", deliveryRef);
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered product List due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Delivered lsit by deistribution point and Company", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewDeliveredOrderModel> GetDeliveredGeneralReqById(long deliveryId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliveredGeneralReqById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                List<ViewDeliveredOrderModel> models = new List<ViewDeliveredOrderModel>();
                ConnectionObj.Open();

                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewDeliveredOrderModel
                    {
                        DeliveryRef = reader["TransactionRef"].ToString(),
                        DeliveryId = deliveryId,
                        DeliveredQty = Convert.ToInt32(reader["DeliveredQty"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString()
                    });
                }
                reader.Close();
                return models;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect general requisition Delivery  details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect general requisition Delivery details", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<Delivery> GetAllDeliveredOrdersByBranchAndCompany(int branchId, int companyId, int orderByUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetAllDeliveredOrdersByBranchAndCompany";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@OrderByUserId", orderByUserId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        }
                        ,
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                            Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                            ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                            ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                            NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                            Active = reader["Active"].ToString(),
                            CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                            MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                            TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                            RegionId = Convert.ToInt32(reader["RegionId"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            BranchId = Convert.ToInt32(reader["BranchId"]),
                            Outstanding = DBNull.Value.Equals(reader["Outstanding"]) ? default(decimal) : Convert.ToDecimal(reader["Outstanding"]),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Orders by branch,Company and order by UserId", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewClientStockProduct> GetClientStockProductAgeByDeliveryId(long deliveryId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetClientStockProductAgeByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                List<ViewClientStockProduct> models = new List<ViewClientStockProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewClientStockProduct
                    {
   
                        Quantity = Convert.ToInt32(reader["TotalQuantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategory = reader["ProductCategoryName"].ToString(),
                        AgeInDealerStock = Convert.ToInt32(reader["AgeInDealerStock"]),
                        AgeLimitInDealerStock = Convert.ToInt32(reader["AgeLimitInDealerStock"]),
                        LifeTime = Convert.ToInt32(reader["LifeTime"]),
                        ReceiveQuantity = Convert.ToInt32(reader["ReceiveQuantity"])
                    });
                }
                reader.Close();
                return models;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Client stock product age by delivery id due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Client stock product age by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewClientStockReport> GetAllClientsByClientTypeId(int clientTypeId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetAllClientsByClientTypeId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientTypeId", clientTypeId);
                List<ViewClientStockReport> models = new List<ViewClientStockReport>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewClientStockReport 
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientCode = reader["SubSubSubAccountCode"].ToString()
                    });
                }
                reader.Close();
                return models;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Client by client type id due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Client by client type id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDeliveredOrderModel> GetDeliveredOrderBySearchCriteria(SearchCriteria aCriteria)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliveredOrderBySearchCriteria";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", aCriteria.ClientId);
                CommandObj.Parameters.AddWithValue("@MonthNo", aCriteria.MonthNo);
                List<ViewDeliveredOrderModel> refModels = new List<ViewDeliveredOrderModel>();
                ConnectionObj.Open();

                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    refModels.Add(new ViewDeliveredOrderModel
                    {
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        DistributorName = reader["DistributorName"].ToString(),
                        DeliveredQty = Convert.ToInt32(reader["DeliveredQuantity"]),
                        DeliveryStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DriverName = reader["DriverName"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        ApproveByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        NsmName = reader["NsmName"].ToString(),
                        SalesAdminName = reader["SalesAdminName"].ToString(),
                        ApproveBySalesAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        SalesAdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        OrderByUserId = Convert.ToInt32(reader["OrderByUserId"]),
                        SalesPersonName = reader["SalesPersonName"].ToString(),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDateTime"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientCode = DBNull.Value.Equals(reader["ClientCode"]) ? null : reader["ClientCode"].ToString(),
                        ClientId = aCriteria.ClientId,
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"])
                    });
                }
                reader.Close();
                return refModels;

            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect Delivered Order by Search Criteria due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Delivered Collect Order by Search Criteria", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
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