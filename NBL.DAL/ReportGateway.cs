using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Reports;
using NBL.Models.ViewModels.Summaries;

namespace NBL.DAL
{
    public class ReportGateway:DbGateway,IReportGateway
    {
        public IEnumerable<ViewClient> GetTopClients()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClients";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect top clients due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewClient> GetTopClientsByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClientsByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect top clients by year due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients by year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClientsByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect top clients by branch Id due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients by branch Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClientsByBranchIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients by branch id and Year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewProduct> GetPopularBatteries()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteries";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>(); 
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect popular batteries", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewProduct> GetPopularBatteriesByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteriesByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>();
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect popular batteries by year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteriesByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>();
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect popular batteries by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchIdCompanyIdAndYear(int branchId, int companyId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteriesByBranchIdCompanyIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>();
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect popular batteries by branch and Company Id and year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromFactory()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptDistributedProductFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewDisributedProduct> products = new List<ViewDisributedProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewDisributedProduct
                    {
                        InventoryDetailsId = Convert.ToInt64(reader["Id"]),
                        InventoryMasterId = Convert.ToInt64(reader["FactoryInventorymasterId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        BarCode = reader["ProductBarCode"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["TransactionDate"]),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = DBNull.Value.Equals(reader["DeliveryRef"]) ? null : reader["DeliveryRef"].ToString(),
                        DeliveredByUserId = DBNull.Value.Equals(reader["DeliveredByUserId"]) ? default(int) : Convert.ToInt32(reader["DeliveredByUserId"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionCenterId"]) ? default(int) : Convert.ToInt32(reader["DistributionCenterId"]),
                        OrderId = DBNull.Value.Equals(reader["OrderId"]) ? default(long) : Convert.ToInt64(reader["OrderId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        OrderRef = DBNull.Value.Equals(reader["OrderRef"]) ? null : reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = DBNull.Value.Equals(reader["InvoiceRef"]) ? null: reader["InvoiceRef"].ToString(),
                        InvoiceId = DBNull.Value.Equals(reader["InvoiceId"]) ? default(long) : Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = DBNull.Value.Equals(reader["ClientId"]) ? default(int): Convert.ToInt32(reader["ClientId"]),
                        ClientCommercialName = DBNull.Value.Equals(reader["CommercialName"])? null:reader["CommercialName"].ToString(),
                        ClientName = DBNull.Value.Equals(reader["ClientName"]) ? null : reader["ClientName"].ToString(),
                        ClientAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        IsSaleDateUpdated =!DBNull.Value.Equals(reader["SaleDateUpdateByUserId"])

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect distributed product from Factory", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromBranch()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptDistributedProductFromBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewDisributedProduct> products = new List<ViewDisributedProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewDisributedProduct
                    {
                        InventoryDetailsId = Convert.ToInt64(reader["InventoryDetailsId"]),
                        InventoryMasterId = Convert.ToInt64(reader["InventoryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        BarCode = reader["ProductBarCode"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["TransactionDate"]),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        DeliveredByUserId = DBNull.Value.Equals(reader["DeliveredByUserId"]) ? default(int) : Convert.ToInt32(reader["DeliveredByUserId"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionCenterId"]) ? default(int) : Convert.ToInt32(reader["DistributionCenterId"]),
                        OrderId = DBNull.Value.Equals(reader["OrderId"]) ? default(long): Convert.ToInt64(reader["OrderId"]),
                        OrderRef = DBNull.Value.Equals(reader["OrderRef"]) ? null : reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = DBNull.Value.Equals(reader["InvoiceRef"])? null: reader["InvoiceRef"].ToString(),
                        InvoiceId = DBNull.Value.Equals(reader["InvoiceId"]) ? default(int) : Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = DBNull.Value.Equals(reader["ClientId"])? default(int): Convert.ToInt32(reader["ClientId"]),
                        ClientCommercialName = DBNull.Value.Equals(reader["CommercialName"]) ? null : reader["CommercialName"].ToString(),
                        ClientName = DBNull.Value.Equals(reader["ClientName"]) ? null : reader["ClientName"].ToString(),
                        ClientAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        IsSaleDateUpdated = !DBNull.Value.Equals(reader["SaleDateUpdateByUserId"])

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect distributed product from branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ViewDisributedProduct GetDistributedProductFromFactory(string barcode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetDistributedProductFromFactoryByBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", barcode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewDisributedProduct product = null;
                if(reader.Read())
                {
                   product=new ViewDisributedProduct
                    {
                        InventoryDetailsId = Convert.ToInt64(reader["Id"]),
                        InventoryMasterId = Convert.ToInt64(reader["FactoryInventorymasterId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        BarCode = reader["ProductBarCode"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["TransactionDate"]),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        DistributionPointId = Convert.ToInt32(reader["DistributionCenterId"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        OrderRef = reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientCommercialName = DBNull.Value.Equals(reader["CommercialName"]) ? null : reader["CommercialName"].ToString(),
                        ClientName = DBNull.Value.Equals(reader["ClientName"]) ? null : reader["ClientName"].ToString(),
                        ClientAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        SaleDate = DBNull.Value.Equals(reader["SaleDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["SaleDate"])


                   };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect distributed product from factory", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ViewDisributedProduct GetDistributedProductFromBranch(string barcode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetDistributedProductFromBranchByBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", barcode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewDisributedProduct product = null;
                if (reader.Read())
                {
                    product = new ViewDisributedProduct
                    {
                        InventoryDetailsId = Convert.ToInt64(reader["InventoryDetailsId"]),
                        InventoryMasterId = Convert.ToInt64(reader["InventoryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        BarCode = reader["ProductBarCode"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["TransactionDate"]),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        DistributionPointId = Convert.ToInt32(reader["DistributionCenterId"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        OrderRef = reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientCommercialName = DBNull.Value.Equals(reader["CommercialName"]) ? null : reader["CommercialName"].ToString(),
                        ClientName = DBNull.Value.Equals(reader["ClientName"]) ? null : reader["ClientName"].ToString(),
                        ClientAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        SaleDate =DBNull.Value.Equals(reader["SaleDate"])? (DateTime?)null: Convert.ToDateTime(reader["SaleDate"])

                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect distributed product from branch", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewProduct> GetTotalStock()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTotalStock";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewProduct> products=new List<ViewProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQty"]),
                        Quantity = Convert.ToInt32(reader["StockQty"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"])
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect total Stock  from branch & factory", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewLoginInfo> GetLoginHistoryByDate(DateTime date)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptLoginHistoryByDate";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@LoginDate", date);
                List<ViewLoginInfo> loginHistory = new List<ViewLoginInfo>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    loginHistory.Add(new ViewLoginInfo
                    {
                       LoginDateTime = Convert.ToDateTime(reader["LoginDatetime"]),
                       UserId = Convert.ToInt32(reader["UserId"]),
                       UserName = reader["UserName"].ToString(),
                       CityName =DBNull.Value.Equals(reader["CityName"])? null: reader["CityName"].ToString(),
                       CountryName = DBNull.Value.Equals(reader["CountryName"])? null: reader["CountryName"].ToString(),
                       CountryCode = DBNull.Value.Equals(reader["CountryCode"])? null: reader["CountryCode"].ToString(),
                       ZipCode = DBNull.Value.Equals(reader["ZipCode"])? null: reader["ZipCode"].ToString(),
                       IpAddress = DBNull.Value.Equals(reader["IpAddress"])? null: reader["IpAddress"].ToString(),
                        MacAddress = DBNull.Value.Equals(reader["MacAddress"])? null: reader["MacAddress"].ToString(),
                        DesignationName = DBNull.Value.Equals(reader["DesignationName"])? null: reader["DesignationName"].ToString(),
                        Latitude = DBNull.Value.Equals(reader["Latitude"])? null: reader["Latitude"].ToString(),
                        Longitude = DBNull.Value.Equals(reader["Longitude"])? null: reader["Longitude"].ToString(),
                        RegionName = DBNull.Value.Equals(reader["RegionName"])? null: reader["RegionName"].ToString(),
                        TimeZone = DBNull.Value.Equals(reader["TimeZone"])? null: reader["TimeZone"].ToString(),
                        EmployeeName = DBNull.Value.Equals(reader["EmployeeName"])? null: reader["EmployeeName"].ToString()
                        
                    });
                }
                reader.Close();
                return loginHistory;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Login History by Date", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<OrderHistory> GetDistributionSetOrders()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetDistributionSetOrders";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<OrderHistory> orders=new List<OrderHistory>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderHistory
                    {
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName =reader["BranchName"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        DistributionPointId = Convert.ToInt32(reader["DistributionCenterId"]),
                        DistributionCenter = reader["DistributionCenter"].ToString(),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        OrderRef = reader["OrderRef"].ToString(),
                        DistributionPointSetByUserId = Convert.ToInt32(reader["DistributionPointSetByUserId"]),
                        DistributionPointSetDateTime = Convert.ToDateTime(reader["DistributionPointSetDateTime"]),
                        DistributionPointSetBy = reader["DistributionPointSetBy"].ToString(),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
                reader.Close();
                return orders;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Distribution Set Orders  History", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<UserWiseOrder> UserWiseOrders()
        {
            try
            {

                CommandObj.CommandText = "UDSP_RptGetUserWiseTotalOrder";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<UserWiseOrder> orders=new List<UserWiseOrder>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new UserWiseOrder
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                        EmployeeName = DBNull.Value.Equals(reader["EmployeeName"]) ? null : reader["EmployeeName"].ToString(),
                        Email = DBNull.Value.Equals(reader["EmailAddress"])?null: reader["EmailAddress"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AlternatePhone"]) ? null : reader["AlternatePhone"].ToString(),
                        Phone = DBNull.Value.Equals(reader["Phone"]) ? null : reader["Phone"].ToString(),
                        EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"]) ? null : reader["EmployeeImage"].ToString(),
                        EmployeeSignature = DBNull.Value.Equals(reader["EmployeeSignature"]) ? null : reader["EmployeeSignature"].ToString(),
                        SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                        Designation = DBNull.Value.Equals(reader["DesignationName"]) ? null : reader["DesignationName"].ToString(),
                        Department = DBNull.Value.Equals(reader["DepartmentName"]) ? null : reader["DepartmentName"].ToString(),
                        Branch = DBNull.Value.Equals(reader["BranchName"]) ? null : reader["BranchName"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        TotalOrder = Convert.ToInt32(reader["TotalOrder"])
                    });
                }
                reader.Close();
                return orders;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect user wise Orders ", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ViewProductHistory GetProductHistoryByBarCode(string barcode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetProductHistoryByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@Bracode", barcode);
                ViewProductHistory product =null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    product = new ViewProductHistory
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["Barcode"].ToString(),
                        ClientName=reader["Client"].ToString(),
                        DeliveryDate = DBNull.Value.Equals(reader["DeliveryDate"])? default(DateTime):Convert.ToDateTime(reader["DeliveryDate"]),
                        DeliveryRef = DBNull.Value.Equals(reader["DeliveryRef"]) ? null: reader["DeliveryRef"].ToString(),
                        ReceiveDate = DBNull.Value.Equals(reader["ReceiveDate"]) ? default(DateTime) : Convert.ToDateTime(reader["ReceiveDate"]),
                        ReceiveRef = DBNull.Value.Equals(reader["ReceiveDate"]) ? null : reader["ReceiveRef"].ToString(),
                        ProductCategoryName = reader["Segment"].ToString(),
                        DeliveredBy = DBNull.Value.Equals(reader["DeliveredBy"])? null: reader["DeliveredBy"].ToString(),
                        DeliveryFromBranch = DBNull.Value.Equals(reader["BranchName"]) ? null : reader["BranchName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get product history ", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewOrderHistory> GetOrderHistoriesByYear(int year)
        {
            try
            {

                CommandObj.CommandText = "UDSP_RptGetOrderHistoryByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                List<ViewOrderHistory> orders = new List<ViewOrderHistory>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new ViewOrderHistory
                    {
                       OrderId = Convert.ToInt64(reader["OrderId"]),
                       OrderRef = reader["OrderRef"].ToString(),
                       OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                       OrderStatus = Convert.ToInt32(reader["OrderStatus"]),
                       InvoiceId = DBNull.Value.Equals(reader["InvoiceId"])? (long?)null:Convert.ToInt64(reader["InvoiceId"]),
                       InvoiceRef = DBNull.Value.Equals(reader["InvoiceRef"])?null:reader["InvoiceRef"].ToString(),
                       DeliveryId = DBNull.Value.Equals(reader["DeliveryId"]) ? (long?)null : Convert.ToInt64(reader["DeliveryId"]),
                       DeliveryRef = DBNull.Value.Equals(reader["DeliveryRef"]) ? null : reader["DeliveryRef"].ToString(),
                       BranchName = reader["OrderByBranch"].ToString()
                    });
                }
                reader.Close();
                return orders;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect  Orders history ref by year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<TerritoryWiseDeliveredQty> GetTerritoryWishTotalSaleQtyByBranchId(int branchId)
        {
            try
            {

                CommandObj.CommandText = "UDSP_RptGetTerritoryWishTotalSaleQtyByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                List<TerritoryWiseDeliveredQty> quantitis = new List<TerritoryWiseDeliveredQty>(); 
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    quantitis.Add(new TerritoryWiseDeliveredQty
                    {
                       TerritoryName = reader["TerritoryName"].ToString(),
                       TerritoryId = Convert.ToInt64(reader["TerritoryId"]),
                       Quantity = Convert.ToInt32(reader["TotalQuantity"])
                 
                    });
                }
                reader.Close();
                return quantitis;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect  territory wish delivered qty by branchID", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewClient> GetClientList()
        {
            try
            {

                CommandObj.CommandText = "UDSP_RptGetClientList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewClient> clients = new List<ViewClient>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = DBNull.Value.Equals(reader["CommercialName"]) ? null : reader["CommercialName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Fax = DBNull.Value.Equals(reader["Fax"]) ? null : reader["Fax"].ToString(),
                        Website = DBNull.Value.Equals(reader["Website"]) ? null : reader["Website"].ToString(),
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                        ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                        ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                        NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                        Active = reader["Active"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        BranchName = DBNull.Value.Equals(reader["BranchName"]) ? null : reader["BranchName"].ToString()
                    });
                }
                reader.Close();
                return clients;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect  client list", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ProductDetails> GetAllProductDetails()
        {
            try
            {

                CommandObj.CommandText = "UDSP_RptGetProductDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ProductDetails> detailses = new List<ProductDetails>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    detailses.Add(new ProductDetails
                    {
                       ProductId = Convert.ToInt32(reader["ProductId"]),
                       ProductName = reader["ProductName"].ToString(),
                       CategoryName = reader["CategoryName"].ToString(),
                       UnitPrice = DBNull.Value.Equals(reader["UnitPrice"])? 0:Convert.ToDecimal(reader["UnitPrice"]),
                       DealerDiscount = DBNull.Value.Equals(reader["DealerDiscount"])? (decimal?)null:Convert.ToDecimal(reader["DealerDiscount"]),
                       CorporateDiscount = DBNull.Value.Equals(reader["CorporateDiscount"])? (decimal?)null:Convert.ToDecimal(reader["CorporateDiscount"]),
                       IndividualDiscount = DBNull.Value.Equals(reader["IndividualDiscount"])? (decimal?)null:Convert.ToDecimal(reader["IndividualDiscount"]),
                       VatAmount = DBNull.Value.Equals(reader["VatAmount"])? (decimal?)null:Convert.ToDecimal(reader["VatAmount"])

                    });
                }
                reader.Close();
                return detailses;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect product Details", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ViewEntityCount GetTotalEntityCount()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTotalEntityCount";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
               ViewEntityCount entity=new ViewEntityCount();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    entity = new ViewEntityCount
                    {
                        TotalBranch = Convert.ToInt32(reader["TotalBranch"]),
                        TotalClient = Convert.ToInt32(reader["TotalClient"]),
                        TotalDept = Convert.ToInt32(reader["TotalDept"]),
                        TotalEmployee = Convert.ToInt32(reader["TotalEmp"]),
                        TotalRegion = Convert.ToInt32(reader["TotalRegion"]),
                        TotalTerritory = Convert.ToInt32(reader["TotalTerritory"])
                    };
                }
                reader.Close();
                return entity;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Entity Count", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
    }
}