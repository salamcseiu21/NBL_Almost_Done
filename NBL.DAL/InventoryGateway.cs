﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Returns;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Replaces;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.DAL
{
    public class InventoryGateway:DbGateway,IInventoryGateway
    {
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductInByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        
                        Quantity = Convert.ToInt32(reader["StockQuantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetTotalReceiveProductByBranchAndCompanyId(int branchId, int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetTotalReceiveProductByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {

                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public ICollection<ViewProduct> GetTotalReceiveProductByCompanyId(int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetTotalReceiveProductByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {

                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

       
        public ICollection<ViewProduct> GetDeliveredProductByBranchAndCompanyId(int branchId, int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetTotalDeliveredProductByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {

                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public List<ChartModel> GetTotalProductionCompanyIdAndYear(int companyId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTotalProductionByCompanyIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                List<ChartModel> models = new List<ChartModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ChartModel
                    {
                        Total = Convert.ToInt32(reader["Total"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect production due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect total production by company id and year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ChartModel> GetTotalDispatchCompanyIdAndYear(int companyId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTotalDispatchCompanyIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                List<ChartModel> models = new List<ChartModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ChartModel
                    {
                        Total = Convert.ToInt32(reader["Total"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect dispatch due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect total dispatch by company id and year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int GetProductStatusInFactoryByBarCode(string barcode)
        {
            try
            {
                int s = -1;
                CommandObj.CommandText = "UDSP_RptGetProductStatusInFactoryByBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", barcode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    s = Convert.ToInt32(reader["Status"]);
                }
                reader.Close();
                return s;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get barcode status due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get total barcode status by company id and year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int GetProductStatusInBranchInventoryByBarCode(string barcode)
        {
            try
            {
                int s = -1;
                CommandObj.CommandText = "UDSP_RptGetProductStatusInBranchInventoryByBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", barcode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    s = Convert.ToInt32(reader["Status"]);
                }
                reader.Close();
                return s;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get barcode status due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get total barcode status by company id and year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

     

        public long GetmaxProductionRefByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetmaxProductionRefByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;
            }
            catch (SqlException exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get max Producton ref due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get max Producton ref by year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

       
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int GetMaxDeliveryRefNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxDeliveryRefOfCurrentYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect max delivery ref of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int GetMaxDispatchRefNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxDispatchRefOfCurrentYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect max dispatch ref of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByDeliveryRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryRef", deliveryRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByDispatchId(long dispatchId, int branchId) 
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByDispatchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewDispatchModel> list = new List<ViewDispatchModel>();
                while (reader.Read())
                {
                    list.Add(new ViewDispatchModel
                    {
                       
                        ToBranchId = branchId,
                        DispatchId = dispatchId,
                        DispatchByUserId = Convert.ToInt32(reader["DispatchByUserId"]),
                        DispatchItemId = Convert.ToInt64(reader["DispatchItemsId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        DispatchDate = Convert.ToDateTime(reader["DispatchDate"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString()
                        },
                        DispatchRef = reader["DispatchRef"].ToString(),
                        Remarks = reader["Remarks"].ToString(),
                        TripId = Convert.ToInt64(reader["TripId"]),
                        SentQuantity = Convert.ToInt32(reader["SentQty"]),
                        ReceiveQuantity = Convert.ToInt32(reader["ReceiveQuantity"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                       // ProductBarcode = reader["ProductBarCode"].ToString()
                    });
                }

                reader.Close();
                return list;


            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect receivable product to barnch by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public TransactionModel GetTransactionModelById(long id)
        {
            try
            {
                CommandObj.CommandText = "spGetTransactionModelById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                TransactionModel model =null;
                if(reader.Read())
                {
                   model=new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        FromBranch = new Branch
                        {
                            BranchName = reader["FromBranchName"].ToString(),
                            BranchAddress = reader["FromBranchAddress"].ToString(),
                        },
                        ToBranch = new Branch
                        {
                            BranchName = reader["ToBranchName"].ToString(),
                            BranchAddress = reader["ToBranchAddress"].ToString(),
                        }
                   };
                }

                reader.Close();
                return model;


            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect transaction  to barnch by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveScannedProduct(ProductionModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveScannedProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@Quantity", model.TotalQuantity);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.TransactionRef);
                CommandObj.Parameters.AddWithValue("@TransactionType", model.TransactionType);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                long masterId = Convert.ToInt32(CommandObj.Parameters["@MasterId"].Value);
                var rowAffected = SaveScannedProductToFactoryInventory(model,masterId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                } 
                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                sqlTransaction.Rollback();
               throw new Exception("Could not saved scanned products",exception);
            }
        }
        private int SaveScannedProductToFactoryInventory(ProductionModel model,long masterId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in model.ScannedProducts)
            {

                CommandObj.CommandText = "UDSP_SaveProductToFactoryInventoryDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@MasterId", masterId);
                CommandObj.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveScannedItemWithQuantity(model, masterId);
            }
            return n;
        }
        private int SaveScannedItemWithQuantity(ProductionModel model, long masterId)
        {
            int i = 0;

            foreach (var item in model.ScannedProducts.GroupBy(n => n.ProductId))
            {

                CommandObj.CommandText = "UDSP_SaveScannedItemWithQuantity";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", item.Key);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Count());
                CommandObj.Parameters.AddWithValue("@FactoryInventoryId", masterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

           
            return i;
        }

        public ScannedProduct IsThisProductSold(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductSold";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode)
        {
            try
            {
                List<ViewProduct> products=new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_OldestProductByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", scannedBarCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while(reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductionDate =Convert.ToDateTime(reader["Production_Date"])
                        
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get  Oldest product qty by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ScannedProduct IsThisProductDispachedFromFactory(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductDispachedFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);

                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ScannedProduct IsThisProductAlreadyInFactoryInventory(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductAlreadyInFactoryInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewFactoryStockModel> GetStockProductInFactory()
        {
            try
            {
                List<ViewFactoryStockModel> products=new List<ViewFactoryStockModel>();
                CommandObj.CommandText = "UDSP_GetStockProductInFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewFactoryStockModel
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductImage = reader["ProductImagePath"].ToString(),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        ProductTypeName = reader["ProductTypeName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                       // ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ComeIntoStore = Convert.ToDateTime(reader["ComeIntoStore"]),
                        CategoryName = reader["ProductCategoryName"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                       // Age =DBNull.Value.Equals(reader["Age"])? default(int): Convert.ToInt32(reader["Age"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect stock product in facotry store", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                List<ViewBranchStockModel> products = new List<ViewBranchStockModel>();
                CommandObj.CommandText = "UDSP_GetStockProductInBranchByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewBranchStockModel
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductImage = reader["ProductImagePath"].ToString(),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        ProductTypeName = reader["ProductTypeName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        //ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ComeIntoBranch = Convert.ToDateTime(reader["ComeIntoBranch"]),
                        CategoryName = reader["ProductCategoryName"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        //Age = Convert.ToInt32(reader["Age"]),
                       // AgeAtBranch = Convert.ToInt32(reader["AgeAtBranch"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect stock product in branch store", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory()
        {
            try
            {
                List<ViewProductTransactionModel> transactions=new List<ViewProductTransactionModel>();
                CommandObj.CommandText = "UDSP_GetAllProductTransactionFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    transactions.Add(new ViewProductTransactionModel
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransactionRef = reader["TransferIssueRef"].ToString(),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        IssueStatus = Convert.ToInt32(reader["IssueStatus"]),
                        QuantityIssued = Convert.ToInt32(reader["QuantityIssued"]),
                        DeliveredQuantity = Convert.ToInt32(reader["DeliveredQuantity"]),
                        ReceivedQuantity = Convert.ToInt32(reader["ReceivedQuantity"]),
                        DeliveredStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DeliveredAt = Convert.ToDateTime(reader["DeliveredAt"]),
                        ApprovedDateTime = Convert.ToDateTime(reader["ApproveDateTime"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        ToBranch = new Branch
                        {
                            BranchName = reader["TBName"].ToString(),
                            Title = reader["TBTitle"].ToString(),
                            BranchAddress = reader["TBAddress"].ToString(),
                            BranchEmail = reader["TBEmail"].ToString(),
                            BranchPhone = reader["TBPhone"].ToString()
                        }
                    });
                }
                reader.Close();
                return transactions;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Production transaction info",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public ICollection<ReceiveProductViewModel> GetAllReceiveableListByBranchAndCompanyId(int branchId,int companyId) 
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivalbeProductByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId",companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ReceiveProductViewModel> list = new List<ReceiveProductViewModel>();
                while (reader.Read())
                {
                    list.Add(new ReceiveProductViewModel
                    {
                       
                        ToBranchId = branchId,
                        //CreatedByUserId = Convert.ToInt32(reader["CreatedByUserId"]),
                        DispatchByUserId=Convert.ToInt32(reader["DispatchByUserId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ReceiveQty = Convert.ToInt32(reader["ReceiveQty"]),
                        DispatchId=Convert.ToInt64(reader["DispatchId"]),
                        DispatchItemsId=Convert.ToInt64(reader["DispatchItemsId"]),
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"]),
                        DispatchRef = reader["DispatchRef"].ToString(),
                        TripRef = reader["TripRef"].ToString(),
                        TripId = Convert.ToInt32(reader["TripId"]),
                        //DriverPhone = reader["DriverPhone"].ToString(),
                       // DriverName = reader["DriverName"].ToString(),
                        //TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        //Transportation = reader["Transportation"].ToString(),
                       // Remarks = reader["Remarks"].ToString(),
                        //VehicleNo = reader["VehicleNo"].ToString()
                       
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);

                throw new Exception("Could not Get receivable product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int ReceiveProduct(ViewDispatchModel model)
        {
            
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveReceiveProuctToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.DispatchDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.DispatchRef);
                CommandObj.Parameters.AddWithValue("@Quantity", model.Quantity);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", model.ReceiveByUserId);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int rowAffected = SaveReceiveProductDetails(model, inventoryId);
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
                throw new Exception("Could not receive product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

      

        public int SaveReceiveProductDetails(ViewDispatchModel model, int inventoryId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in model.ScannedProducts) 
            {
                CommandObj.CommandText = "spSaveReceiveProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2,3)));
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n= SaveReceivedItemWithQuantity(model, inventoryId);
            }
            return n;
        }


        private int SaveReceivedItemWithQuantity(ViewDispatchModel model, int inventoryId)
        {
            int i = 0;
            foreach (var item in model.DispatchModels)
            {
                var qty1 = model.DispatchModels.ToList().FindAll(n => n.ProductId == item.ProductId)
                    .Sum(n => n.Quantity);
                var qty2 = model.ScannedProducts.ToList().FindAll(n => n.ProductId == item.ProductId).Count;
                int status = 2;
                if (qty1 != qty2)
                {
                    status = 0;
                }
                var totalReceiveQty = qty2 + item.ReceiveQuantity;
                CommandObj.CommandText = "spSaveInventoryItem";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@DispatchItemId", item.DispatchItemId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", qty2);
                CommandObj.Parameters.AddWithValue("@ReceiveQuantity", totalReceiveQty);
                CommandObj.Parameters.AddWithValue("@ReceiveByUserId", model.ReceiveByUserId);
                CommandObj.Parameters.AddWithValue("@Status", status);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            try
            {
                int stockQty = 0;
                CommandObj.CommandText = "spGetStockQtyByBranchIdAndProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    stockQty = Convert.ToInt32(reader["StockQuantity"]);
                }
                reader.Close();
                return stockQty;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect Stock Qty", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int SaveDeliveredOrder(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus,int orderStatus)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "spSaveDeliveredOrderInformation";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", aDelivery.SpecialDiscount);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aDelivery.VoucherNo);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@InvoiceId",aDelivery.InvoiceId);
                CommandObj.Parameters.AddWithValue("@InvoiceStatus", invoiceStatus);
                CommandObj.Parameters.AddWithValue("@OrderStatus", orderStatus);
                CommandObj.Parameters.AddWithValue("@IsOwnTransporatoion", aDelivery.IsOwnTransport);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverPhone", aDelivery.DriverPhone ?? "N/A");
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo ?? "N/A");
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@DistributionPointId", aDelivery.DistributionPointId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;

                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);
                var accountMasterId= Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                int rowAffected = SaveDeliveredOrderDetails(scannedProducts,aDelivery, inventoryId, deliveryId);

                int accountAffected = 0;
                if (rowAffected > 0)
                {

                   var financial= aDelivery.FinancialTransactionModel;
                  
                    for (int i = 1; i <= 4; i++)
                    {
                        if (i == 1)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Dr",financial.ClientCode,financial.ClientDrAmount, accountMasterId,"Client Debit..");
                        }
                        else if(i==2)
                        {

                          
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.SalesRevenueCode, financial.SalesRevenueAmount*(-1), accountMasterId,"Salses Credit..");
                        }
                        else if(i==3)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.GrossDiscountCode, financial.GrossDiscountAmount, accountMasterId,"Gross Discount Debit..");
                        }
                        else if (i == 4)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.VatCode, financial.VatAmount*(-1), accountMasterId,"Vat Credit..");
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
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveDeliveredOrderDetails(List<ScannedProduct> scannedProducts, Delivery aDelivery, int inventoryId,int deliveryId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in scannedProducts)
            {
               
                CommandObj.CommandText = "spSaveDeliveredOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2,3)));
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Status", 1);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            if (i > 0)
            {
                n = SaveDeliveredItemWithQuantity(scannedProducts, inventoryId,aDelivery);
            }
            return n;
        }

        private int SaveDeliveredItemWithQuantity(List<ScannedProduct> deliveredProducts, int inventoryId,Delivery aDelivery)
        {
            int i = 0;
            var groupBy = deliveredProducts.GroupBy(n => n.ProductId);
            foreach (IGrouping<int, ScannedProduct> scannedProducts in groupBy)
            {
                CommandObj.CommandText = "spSaveDeliveredItemToInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", scannedProducts.Key);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count());
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }


        private int SaveFinancialTransactionToAccountsDetails(string transactionType,string accountCode,decimal amounts, int accountMasterId,string explanation)
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



        public int ReceiveProduct(List<ScannedProduct> barcodeList, int branchId, int userId, FinancialTransactionModel financialModel,ReturnModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveReceiveReturnProuctToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.ApproveByAdminDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.ReturnRef);
                CommandObj.Parameters.AddWithValue("@Quantity", model.TotalQuantity);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.AddWithValue("@ReturnNo", model.ReturnNo);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                var accountMasterId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                int rowAffected = SaveReceiveReturnProductDetails(barcodeList, inventoryId);
              


                int accountAffected = 0;
                if (rowAffected > 0)
                {
                    var financial = financialModel;

                    if (!model.IsGeneralReturn)
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (i == 1)
                            {
                                accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.ClientCode, financial.ClientCrAmount * (-1), accountMasterId, "Client Credit..");
                            }
                            else if (i == 2)
                            {


                                accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.SalesRevenueCode, financial.SalesRevenueAmount, accountMasterId, "Salses Debit..");
                            }
                            else if (i == 3)
                            {
                                accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.GrossDiscountCode, financial.GrossDiscountAmount * (-1), accountMasterId, "Gross Discount Credit..");
                            }
                            else if (i == 4)
                            {
                                accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.VatCode, financial.VatAmount, accountMasterId, "Vat Debit..");
                            }
                            else if (i == 5 && financial.SalesReturnAmount > 0)
                            {

                                accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.ClientCode, financial.SalesReturnAmount, accountMasterId, "Client Debit..");
                            }
                            else if (i == 6 && financial.SalesReturnAmount > 0)
                            {
                                accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.SalesReturnCode, financial.SalesReturnAmount * (-1), accountMasterId, "Sales return  Credit..");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            if (i == 1)
                            {
                                accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.ExpenceCode, financial.ExpenceAmount * (-1), accountMasterId, "Expence Credit..");
                            }
                            else if (i == 2)
                            {

                                accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.InventoryCode, financial.InventoryAmount, accountMasterId, "Inventory Debit..");
                            }
                            
                        }
                    }

                   

                    
                }
                if (accountAffected > 0)
                {
                    sqlTransaction.Commit();
                    return accountAffected;
                }
                return accountAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                sqlTransaction.Rollback();
                throw new Exception("Could not receive product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int SaveReceiveReturnProductDetails(List<ScannedProduct> products, int inventoryId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in products)
            {
                CommandObj.CommandText = "spSaveReceiveProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveReceivedReturnItemWithQuantity(products, inventoryId);
            }
            return n;
        }

        private int SaveReceivedReturnItemWithQuantity(List<ScannedProduct> products, int inventoryId)
        {
            int i = 0;
            foreach (var scannedProducts in products.GroupBy(n => n.ProductId))
            {
                CommandObj.CommandText = "spSaveReturnItemToInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", scannedProducts.Key);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.ToList().FindAll(n => n.ProductId == scannedProducts.Key).Count);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }


        public ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetProductLifeCycleByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", productBarCode);
                ViewProductLifeCycleModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model=new ViewProductLifeCycleModel
                    {
                        Age = Convert.ToInt32(reader["Age"]),
                        ComeIntoInventory = Convert.ToDateTime(reader["ComeIntoInventory"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        LifeTime = Convert.ToInt32(reader["LifeTime"])
                       // Status = Convert.ToInt32(reader["Status"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get product life cycle by barcode",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
       
        public IEnumerable<ViewProduct> GetAllProductsBarcode()
        {
            try
            {
                List<ViewProduct> products=new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_GetAllProductsBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect products barcode",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }
        //----------------Save data to Trip,TripItem And Tripdetails ..-------------

        public long GetMaxTripRefNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxTripRefNoOfCurrentYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                long slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt64(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect max trip ref", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewTripModel> GetAllTrip()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllTrip";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                List<ViewTripModel> models=new List<ViewTripModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewTripModel
                    {
                        TripId = Convert.ToInt64(reader["TripId"]),
                        TripRef = reader["TripRef"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        DriverName = reader["DriverName"].ToString(),
                        CreatedByUserId = Convert.ToInt32(reader["CreatedByUserId"]),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DeliveryQuantity = Convert.ToInt32(reader["Quantity"]),
                        Transportation = reader["Transportation"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        Remarks = reader["Remarks"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TripCreatedBy = new ViewEmployee
                        {
                            SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"])? null: reader["SubSubSubAccountCode"].ToString(),
                            EmployeeName = DBNull.Value.Equals(reader["EmployeeName"]) ? null : reader["EmployeeName"].ToString(),
                            DepartmentName = DBNull.Value.Equals(reader["DepartmentName"]) ? null : reader["DepartmentName"].ToString(),
                            DesignationName = DBNull.Value.Equals(reader["DesignationName"]) ? null : reader["DesignationName"].ToString(),
                            EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"]) ? null : reader["EmployeeImage"].ToString()
                        }
                       
                     
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect trip info",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewTripModel> GetAllDeliverableTripList()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDeliverableTripList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                List<ViewTripModel> models = new List<ViewTripModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewTripModel
                    {
                        TripId = Convert.ToInt64(reader["TripId"]),
                        TripRef = reader["TripRef"].ToString(),
                        DriverPhone = reader["DriverPhone"].ToString(),
                        DriverName = reader["DriverName"].ToString(),
                        CreatedByUserId = Convert.ToInt32(reader["CreatedByUserId"]),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DeliveryQuantity = Convert.ToInt32(reader["Quantity"]),
                        Transportation = reader["Transportation"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        Remarks = reader["Remarks"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TripCreatedBy = new ViewEmployee
                        {
                            SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                            EmployeeName = DBNull.Value.Equals(reader["EmployeeName"]) ? null : reader["EmployeeName"].ToString(),
                            DepartmentName = DBNull.Value.Equals(reader["DepartmentName"]) ? null : reader["DepartmentName"].ToString(),
                            DesignationName = DBNull.Value.Equals(reader["DesignationName"]) ? null : reader["DesignationName"].ToString(),
                            EmployeeImage = DBNull.Value.Equals(reader["EmployeeImage"]) ? null : reader["EmployeeImage"].ToString()
                        }


                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect deliverable trip info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<Inventory> GetReceivedProductByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceiveProductByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                List<Inventory> models = new List<Inventory>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new Inventory
                    {
                        InventoryId = Convert.ToInt64(reader["InventoryId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect received product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetReceivedProductBarcodeById(long inventoryId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedProductBarcodeByInventoryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                List<ViewProduct> models = new List<ViewProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewProduct
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString()
                       
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect received barcdoe list by id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetReceivedProductById(long inventoryId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedProductByInventoryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                List<ViewProduct> models = new List<ViewProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewProduct
                    {
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])

                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Collect received product list by id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }


        public int CreateTrip(ViewCreateTripModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                int i =0;
                CommandObj.CommandText = "UDSP_SaveTrip";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripRef", model.TripRef);
                CommandObj.Parameters.AddWithValue("@Remarks", model.Remarks);
                CommandObj.Parameters.AddWithValue("@DriverName", model.DriverName?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverPhone", model.DriverPhone ?? "N/A");
                CommandObj.Parameters.AddWithValue("@Transportation", model.Transportation ?? "N/A");
                CommandObj.Parameters.AddWithValue("@VehicleNo", model.VehicleNo ?? "N/A");
                CommandObj.Parameters.AddWithValue("@TransportationCost", model.TransportationCost);
                CommandObj.Parameters.AddWithValue("@IsOwnTransporatoion", model.IsOwnTransport);
                CommandObj.Parameters.AddWithValue("@UserId", model.CreatedByUserId);
                CommandObj.Parameters.Add("@TripId", SqlDbType.BigInt);
                CommandObj.Parameters["@TripId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var tripId = Convert.ToInt64(CommandObj.Parameters["@TripId"].Value);
                i = SaveTripItems(model.TripModels, tripId);
                if (i > 0)
                {
                    sqlTransaction.Commit(); 
                }
                else
                {
                    sqlTransaction.Rollback();
                }
                return i;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                sqlTransaction.Rollback();
                throw new Exception("Unable to create trip", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        private static bool IsRequisitionDeliveredQtyEqual(ICollection<ViewTripModel> modelTripModels, long requisitonId)
        {

            var dq = modelTripModels.ToList().FindAll(n => n.RequisitionId == requisitonId)
                .Sum(n => n.DeliveryQuantity);
            var rq = modelTripModels.ToList()
                .FindAll(n => n.RequisitionId == requisitonId).Sum(n => n.RequisitionQty);

            return modelTripModels.ToList().FindAll(n => n.RequisitionId == requisitonId)
                       .Sum(n => n.DeliveryQuantity) == modelTripModels.ToList()
                       .FindAll(n => n.RequisitionId == requisitonId).Sum(n => n.RequisitionQty);
        }

        private int SaveTripItems(ICollection<ViewTripModel> modelTripModels, long tripId) 
        {
            int i = 0;
            int n = 0;
            foreach (ViewTripModel model in modelTripModels)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveTripItems";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                CommandObj.Parameters.AddWithValue("@RequisitionId", model.RequisitionId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@ProductId", model.ProuctId);
                CommandObj.Parameters.AddWithValue("@Quantity", model.DeliveryQuantity);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n += SaveTripDetails(modelTripModels, tripId);
            }
            return n;
        }

        private int SaveTripDetails(ICollection<ViewTripModel> modelTripModels, long tripId)
        {
            int i = 0;
            foreach (long requisitonId in modelTripModels.Select(n => n.RequisitionId).Distinct())
            {
                
                //int requisitionStaus = Convert.ToInt32(RequisitionStatus.PartialDelivery);
                //if (IsRequisitionDeliveredQtyEqual(modelTripModels, requisitonId))
                //{
                //    requisitionStaus = Convert.ToInt32(RequisitionStatus.FullDelivery);
                //}
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveTripDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitonId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public ViewDispatchModel GetDispatchByDispatchId(long dispatchId)
        {
            try
            {
                ViewDispatchModel model = null;
                CommandObj.CommandText = "UDSP_GetDispatchByDispatchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    model=new ViewDispatchModel
                    {
                        TripId = Convert.ToInt64(reader["TripId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        DispatchRef = reader["DispatchRef"].ToString(),
                        DispatchByUserId = Convert.ToInt32(reader["DispatchByUserId"]),
                        DispatchId = dispatchId,
                        DispatchDate = Convert.ToDateTime(reader["SystemDatetime"]),
                        TransactionRef = reader["TransactionRef"].ToString()
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get dispatch by trip id",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDispatchModel> GetAllReceiveableItemsByDispatchAndBranchId(long dispatchId, int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllReceiveableItemsByDispatchAndBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DispatchId", dispatchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                List<ViewDispatchModel> products=new List<ViewDispatchModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewDispatchModel
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductBarcode = reader["ProductBarcode"].ToString(),
                        ToBranchId = branchId,
                        TripId = Convert.ToInt64(reader["TripId"]),
                        DispatchId = dispatchId,
                        TransactionRef = reader["TransactionRef"].ToString()

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Coluld not collect receivable product list by trip and product id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                
            }
        }

        public long GetMaxVoucherNoByTransactionInfix(string infix)
        {
            try
            {
                CommandObj.CommandText = "spGetMaxVoucherNoOfByTransactionInfix";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionInfix", infix);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int voucherNo = 0;
                if (reader.Read())
                {
                    voucherNo = Convert.ToInt32(reader["MaxVoucherNo"]);
                }
                reader.Close();
                return voucherNo;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect max voucher no", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ProductionSummary> GetProductionSummaries()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductionSummary";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ProductionSummary> summaries=new List<ProductionSummary>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    summaries.Add(new ProductionSummary
                    {
                        ProductCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CameToStoreDate = Convert.ToDateTime(reader["CameToStoreDate"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return summaries;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect prduction summary", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ProductionSummary> GetProductionSummaryByMonth(DateTime dateTime)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductionSummaryByMonth";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Date", dateTime);
                ConnectionObj.Open();
                List<ProductionSummary> summaries = new List<ProductionSummary>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    summaries.Add(new ProductionSummary
                    {
                        ProductCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                     
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["ProductCategoryName"].ToString()

                    });
                }
                reader.Close();
                return summaries;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect prduction summary by month", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        //--------------------------Replace----------------------
        public int SaveReplaceDeliveryInfo(List<ScannedProduct> scannedProducts, Delivery aDelivery, int replaceStatus,ViewReplaceModel replaceModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "UDSP_SaveReplaceDeliveryInfo";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aDelivery.VoucherNo);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@InvoiceId", aDelivery.InvoiceId);
                CommandObj.Parameters.AddWithValue("@ReplaceStatus", replaceStatus);
                CommandObj.Parameters.AddWithValue("@IsOwnTransporatoion", aDelivery.IsOwnTransport);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverPhone", aDelivery.DriverPhone ?? "N/A");
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo ?? "N/A");
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@DistributionPointId", aDelivery.DistributionPointId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;

                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);
                //int rowAffected = SaveDeliveredOrderDetails(scannedProducts, aDelivery, inventoryId, deliveryId);
                int rowAffected = SaveDeliveredReplaceDetails(scannedProducts, aDelivery, inventoryId, deliveryId,replaceModel);
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
                throw new Exception("Could not Save replace products", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveDeliveredReplaceDetails(List<ScannedProduct> scannedProducts, Delivery aDelivery, int inventoryId, int deliveryId,ViewReplaceModel replaceModel)
        {
            int i = 0;
            int n = 0;
            foreach (var item in scannedProducts)
            {

                CommandObj.CommandText = "spSaveDeliveredReplaceDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Status", 1);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.AddWithValue("@SaleDate", replaceModel.SaleDate?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@RbdBarCode", replaceModel.RbdBarcode ?? (object)DBNull.Value);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            if (i > 0)
            {
                n = SaveDeliveredItemWithQuantity(scannedProducts, inventoryId, aDelivery);
            }
            return n;
        }

        public int SaveTransferDeliveredProduct(TransferModel aModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveTransferDeliveredProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;

                CommandObj.Parameters.AddWithValue("@TransactionDate", aModel.Delivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aModel.Delivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@IsOwnTransporatoion", aModel.Delivery.IsOwnTransport);
                CommandObj.Parameters.AddWithValue("@Transportation", aModel.Delivery.Transportation ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverName", aModel.Delivery.DriverName ?? "N/A");
                CommandObj.Parameters.AddWithValue("@DriverPhone", aModel.Delivery.DriverPhone ?? "N/A");
                CommandObj.Parameters.AddWithValue("@TransportationCost", aModel.Delivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aModel.Delivery.VehicleNo ?? "N/A");
                CommandObj.Parameters.AddWithValue("@CompanyId", aModel.Delivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aModel.TransferRequisition.TransferRequisitionRef);
                CommandObj.Parameters.AddWithValue("@Quantity", aModel.ScannedBarCodes.ToList().Count);
                CommandObj.Parameters.AddWithValue("@TransferToBranchId", aModel.TransferRequisition.RequisitionByBranchId);
                CommandObj.Parameters.AddWithValue("@TransferFromBranchId", aModel.TransferRequisition.RequisitionToBranchId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", aModel.Delivery.DeliveredByUserId);
                CommandObj.Parameters.Add("@TransferId", SqlDbType.BigInt);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.BigInt);
                CommandObj.Parameters["@TransferId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                long transferId = Convert.ToInt64(CommandObj.Parameters["@TransferId"].Value);
                long inventoryId= Convert.ToInt64(CommandObj.Parameters["@InventoryId"].Value); 
                int rowAffected = SaveTransferProductDetails(aModel, transferId,inventoryId);
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

       

        private int SaveTransferProductDetails(TransferModel aModel, long transferId,long inventoryId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in aModel.ScannedBarCodes)
            {
                CommandObj.CommandText = "UDSP_SaveTransferProductDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@TransferId", transferId);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@ProductBarCode", item.ProductCode);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveTransferedItems(aModel.Detailses, transferId,inventoryId);
            }
            return n;
        }

        private int SaveTransferedItems(IEnumerable<TransferRequisitionDetails> products, long transferId,long inventoryId)
        {
            int i = 0;
            foreach (var item in products)
            {
                CommandObj.CommandText = "UDSP_SaveTransferedItems";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@TransferId", transferId);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }
        public ICollection<ViewTransferProductModel> GetAllTransferedListByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllTransferedListByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewTransferProductModel> list = new List<ViewTransferProductModel>();
                while (reader.Read())
                {
                    list.Add(new ViewTransferProductModel
                    {

                        ToBranchId = branchId,
                        //CreatedByUserId = Convert.ToInt32(reader["CreatedByUserId"]),
                        TransferByUserId = Convert.ToInt32(reader["TransferByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ReceivedQuantity = Convert.ToInt32(reader["ReceiveQuantity"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredAt = Convert.ToDateTime(reader["SystemDateTime"]),
                        TransferId = Convert.ToInt64(reader["TransferId"])

                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get receivable product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public List<string> GetTransferReceiveableBarcodeList(long transferId)
        {
            try
            {

                CommandObj.CommandText = "UDSP_GetTransferReceiveableBarcodeList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferId", transferId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<string> list = new List<string>();
                while (reader.Read())
                {
                    list.Add(reader["ProductBarCode"].ToString());
                }

                reader.Close();
                return list;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not Get transfer receivable product code by transferid", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ReceiveTransferedProduct(TransferModel aModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveReceiveProuctToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aModel.ViewTransferProductModel.TransactionRef);
                CommandObj.Parameters.AddWithValue("@Quantity", aModel.ScannedBarCodes.Count);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aModel.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aModel.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aModel.User.UserId);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int rowAffected = SaveTransferReceiveProductDetails(aModel, inventoryId);
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
                throw new Exception("Could not receive product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int SaveTransferReceiveProductDetails(TransferModel model, int inventoryId)
        {
            int i = 0;
            int n = 0;
            foreach (var item in model.ScannedBarCodes)
            {
                CommandObj.CommandText = "spSaveTransferReceiveProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            if (i > 0)
            {
                n = SaveTransferReceivedItemWithQuantity(model, inventoryId);
            }
            return n;
        }
        private int SaveTransferReceivedItemWithQuantity(TransferModel model, int inventoryId)
        {
            int i = 0;
            foreach (var item in model.Products)
            {
                var qty1 = model.Products.ToList().FindAll(n => n.ProductId == item.ProductId)
                    .Sum(n => n.Quantity);
                var qty2 = model.ScannedBarCodes.ToList().FindAll(n => n.ProductId == item.ProductId).Count;
                int status = 2;
                if (qty1 != qty2)
                {
                    status = 0;
                }
                CommandObj.CommandText = "spSaveTransferItemInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@TransferItemId", item.TransferItemId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", qty2);
                CommandObj.Parameters.AddWithValue("@ReceiveByUserId", model.User.UserId);
                CommandObj.Parameters.AddWithValue("@Status", status);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }

        //---------------------delivery order form factory-----------------

        public int SaveDeliveredOrderFromFactory(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "UDSP_SaveDeliveredOrderInformationFromFactory";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", aDelivery.SpecialDiscount);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aDelivery.VoucherNo);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@InvoiceId", aDelivery.InvoiceId);
                CommandObj.Parameters.AddWithValue("@InvoiceStatus", invoiceStatus);
                CommandObj.Parameters.AddWithValue("@OrderStatus", orderStatus);
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
                int rowAffected = SaveDeliveredOrderDetailsFromFactory(scannedProducts, aDelivery, deliveryId,inventoryMasterId);

                int accountAffected = 0;
                if (rowAffected > 0)
                {

                    var financial = aDelivery.FinancialTransactionModel;

                    for (int i = 1; i <= 4; i++)
                    {
                        if (i == 1)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.ClientCode, financial.ClientDrAmount, accountMasterId, "Client Debit..");
                        }
                        else if (i == 2)
                        {


                            accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.SalesRevenueCode, financial.SalesRevenueAmount * (-1), accountMasterId, "Salses Credit..");
                        }
                        else if (i == 3)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Dr", financial.GrossDiscountCode, financial.GrossDiscountAmount, accountMasterId, "Gross Discount Debit..");
                        }
                        else if (i == 4)
                        {
                            accountAffected += SaveFinancialTransactionToAccountsDetails("Cr", financial.VatCode, financial.VatAmount * (-1), accountMasterId, "Vat Credit..");
                        }

                    }
                }


                //if (rowAffected > 0)
                //{

                //    for (int i = 1; i <= 2; i++)
                //    {
                //        if (i == 1)
                //        {
                //            anInvoice.TransactionType = "Dr";
                //            anInvoice.SubSubSubAccountCode = anInvoice.ClientAccountCode;
                //        }
                //        else
                //        {
                //            anInvoice.TransactionType = "Cr";
                //            anInvoice.Amounts = anInvoice.Amounts * (-1);
                //            anInvoice.SubSubSubAccountCode = "1001021";
                //        }
                //        accountAffected += SaveFinancialTransactionToAccountsDetails(anInvoice, accountMasterId);
                //    }

                //    if (anInvoice.SpecialDiscount != 0)
                //    {
                //        for (int i = 1; i <= 2; i++)
                //        {
                //            if (i == 1)
                //            {
                //                anInvoice.TransactionType = "Dr";
                //                anInvoice.Amounts = anInvoice.SpecialDiscount;
                //                anInvoice.SubSubSubAccountCode = anInvoice.DiscountAccountCode;
                //            }
                //            else
                //            {
                //                anInvoice.TransactionType = "Cr";
                //                anInvoice.Amounts = anInvoice.SpecialDiscount * (-1);
                //                anInvoice.SubSubSubAccountCode = anInvoice.ClientAccountCode;
                //            }
                //            accountAffected += SaveFinancialTransactionToAccountsDetails(anInvoice, accountMasterId);
                //        }
                //    }


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
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveDeliveredOrderDetailsFromFactory(List<ScannedProduct> scannedProducts, Delivery aDelivery, int deliveryId,long inventoryMasterId)
        {
           
            int n = 0;
            int i = 0;
            foreach (var item in scannedProducts)
            {

                CommandObj.CommandText = "UDSP_SaveDeliveredOrderDetailsFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(2, 3)));
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.AddWithValue("@Status", 2);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                n += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            if (n > 0)
            {
                i = SaveDeliveredItemWithQuantityToFactory(scannedProducts, aDelivery, inventoryMasterId);
            }
           
            return i;
        }


        private int SaveDeliveredItemWithQuantityToFactory(List<ScannedProduct> deliveredProducts,Delivery aDelivery,long inventoryMasterId)
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
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);

            }
            return i;
        }
        public int SaveDeliveredGeneralRequisition(List<ScannedProduct> scannedProducts, Delivery aDelivery)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "UDSP_SaveDeliveredRequisitionInformation";
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
                int rowAffected = SaveDeliveredOrderDetails(scannedProducts,aDelivery,inventoryMasterId,deliveryId);

                int accountAffected = 0;
                if (rowAffected > 0)
                {

                    var financial = aDelivery.FinancialTransactionModel;

                    for (int i = 1; i <= 2; i++)
                    {
                        if(i == 1)
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
                sqlTransaction.Rollback();
                throw new Exception("Could not Save general Requisition", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<object> GetStockProductByBranchCompanyIdAndSerachTerm(int branchId, int companyId, string searchTerm)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductInByBranchCompanyIdAndSearchTerm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@SearchTerm", searchTerm);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<object> products = new List<object>();
                while (reader.Read())
                {
                    products.Add(new 
                    {

                        label = reader["ProductName"].ToString(),
                        val = Convert.ToInt32(reader["ProductId"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<object> GetFactoryStockProductBySearchTerm(string searchTerm)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetFactoryStockProductBySearchTerm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SearchTerm", searchTerm);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<object> products = new List<object>();
                while (reader.Read())
                {
                    products.Add(new
                    {

                        label = reader["ProductName"].ToString(),
                        val = Convert.ToInt32(reader["ProductId"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect factory stock  product list by Search Term", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetStockProductQuantityInFactoryById(int productId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductQuantityInFactoryById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int quantity = 0;
                if(reader.Read())
                {

                    quantity = Convert.ToInt32(reader["Quantity"]);

                }

                reader.Close();
                return quantity;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect factory stock qty by   product id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetRequsitionVeStockProductQtyByDistributionCenter(int distributionCenterId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptRequsitionVeStockProductQtyByDistributionCenter";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DistributionCenterId", distributionCenterId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {

                        StockQuantity = Convert.ToInt32(reader["Stock"]),
                        RequisitionQty = Convert.ToInt32(reader["RequisitionQty"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewTripDetailsModel> GetTripItemsByTripId(long tripId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTripItemsByTripId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                ConnectionObj.Open();
                List<ViewTripDetailsModel> models=new List<ViewTripDetailsModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewTripDetailsModel
                    {
                        TripItemId = Convert.ToInt64(reader["TripItemsId"]),
                        TripId = tripId,
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"]),
                        IsCancelled = reader["IsCancelled"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TripStatus = Convert.ToInt32(reader["Status"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ToBranch = new ViewBranch
                        {
                            BranchName = reader["BranchName"].ToString(),
                            BranchAddress = reader["BranchAddress"].ToString(),
                            BranchId = Convert.ToInt32(reader["ToBranchId"])
                           
                        }
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect trip item list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UpdateTripItemQuantity(long tripItemId, int quantity)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateTripItemQuantity";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@TripItemId", tripItemId);
                CommandObj.Parameters.AddWithValue("@Quantity", quantity);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not update trip item quantity", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        //----------------------------Product Life Cicle/1-July-2019-------
        public ViewProductLifeCycleModel GetProductLifeCycle(string code)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetProductLifeCycle";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Code", code);
                ViewProductLifeCycleModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model = new ViewProductLifeCycleModel
                    {
                        Age = Convert.ToInt32(reader["Age"]),
                        ComeIntoInventory = DBNull.Value.Equals(reader["ScanDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ScanDate"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        LifeTime = Convert.ToInt32(reader["LifeTime"]),
                        ProductCategoryName=reader["ProductCategoryName"].ToString(),
                        DispatchDate = DBNull.Value.Equals(reader["DespatchDate"])? (DateTime?)null: Convert.ToDateTime(reader["DespatchDate"]),
                        LineNo = Convert.ToInt32(reader["LineNumber"]),
                        BatchCode = reader["BatchCode"].ToString(),
                        ShiftNo = reader["ShiftNo"].ToString(),
                        Barcode = reader["ProductBarcode"].ToString()

                        // Status = Convert.ToInt32(reader["Status"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get product life cycle by barcode", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewProductHistory GetProductHistoryByBarcode(string barcode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductHistoryByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SearchCode", barcode);
                ViewProductHistory model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model = new ViewProductHistory
                    {
                        ProductAge = Convert.ToInt32(reader["ProductAge"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"]+" ("+ reader["Segment"]+")",
                        ProductCategoryName = reader["Segment"].ToString(),
                        ProductBarCode = reader["ProductBarcode"].ToString(),
                        LifeTime = Convert.ToInt32(reader["LifeTime"]),
                        AgeLimitInDealerStock = Convert.ToInt32(reader["AgeLimitInDealerStock"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        ClientType = reader["ClientTypeName"].ToString(),
                        ClientCode = reader["SubSubSubAccountCode"].ToString(),
                        ClientPhone = reader["ClientPhone"].ToString(),
                        ClientAddress = reader["ClientAddress"].ToString(),
                        TerritoryName = reader["TerritoryName"].ToString(),
                        RegionName = reader["RegionName"].ToString(),
                        AssignedEmpName = reader["AssignedEmpName"].ToString(),
                        AssignedEmpPhone = reader["AssignedEmpPhone"].ToString(),
                        OrderRef = reader["OrderRef"].ToString(),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        DeliveryFromBranch = reader["DistributionCenter"].ToString(),
                        OrderFromBranch = reader["OrderByBranch"].ToString(),
                        SaleDate = DBNull.Value.Equals(reader["SaleDate"])? DateTime.Now: Convert.ToDateTime(reader["SaleDate"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get product history by barcode", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
    }
}