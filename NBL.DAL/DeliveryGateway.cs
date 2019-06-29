using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Transports;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Replaces;

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
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Orders", exception);
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
                throw new Exception("Could not Collect Invoiced Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                        Quantity = reader["DeliveredQuantity"] is DBNull ? 0 : Convert.ToInt32(reader["DeliveredQuantity"]),
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
                throw new Exception("Could not Collect Delivered Orders details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                        VehicleNo = reader["VehicleNo"].ToString()
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered products due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivery ref due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivery details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered Orders details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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
                throw new Exception("Could not Collect Delivered products due to Db Exception", exception);
            }
            catch (Exception exception)
            {
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