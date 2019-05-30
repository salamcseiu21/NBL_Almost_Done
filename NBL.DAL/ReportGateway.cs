using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

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
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveredByUserId = DBNull.Value.Equals(reader["DeliveredByUserId"]) ? default(int) : Convert.ToInt32(reader["DeliveredByUserId"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionCenterId"]) ? default(int) : Convert.ToInt32(reader["DistributionCenterId"]),
                        OrderId = DBNull.Value.Equals(reader["OrderId"]) ? default(long) : Convert.ToInt64(reader["OrderId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        OrderRef = reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientCommercialName = DBNull.Value.Equals(reader["CommercialName"])?null:reader["CommercialName"].ToString(),
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
                        OrderRef = reader["OrderRef"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceId = Convert.ToInt64(reader["InvoiceId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
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
                        ClientAccountCode = reader["SubSubSubAccountCode"].ToString()
                        

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
    }
}