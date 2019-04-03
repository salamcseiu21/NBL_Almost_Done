using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
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
                        
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
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
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
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
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
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

                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewDispatchModel> GetAllReceiveableProductToBranchByTripId(long tripId,int branchId) 
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByTripId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewDispatchModel> list = new List<ViewDispatchModel>();
                while (reader.Read())
                {
                    list.Add(new ViewDispatchModel
                    {
                       
                        ToBranchId = branchId,
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
                        TripId = tripId,
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
                throw new Exception("Could not collect transaction  to barnch by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveScannedProduct(List<ScannedProduct> scannedProducts,int userId)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveScannedProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                long masterId = Convert.ToInt32(CommandObj.Parameters["@MasterId"].Value);
                var rowAffected = SaveScannedProductToFactoryInventory(scannedProducts,masterId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                } 
                return rowAffected;
            }
            catch (Exception exception)
            {
               sqlTransaction.Rollback();
               throw new Exception("Could not saved scanned products",exception);
            }
        }
        private int SaveScannedProductToFactoryInventory(List<ScannedProduct> scannedProducts,long masterId)
        {
            int i = 0;
            foreach (var item in scannedProducts)
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
                       // Age = Convert.ToInt32(reader["Age"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
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
                CommandObj.CommandText = "spSaveInventoryItem";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@DispatchItemId", item.DispatchItemId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@ReceiveByUserId", model.ReceiveByUserId);
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
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
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

                int rowAffected = SaveDeliveredOrderDetails(scannedProducts,aDelivery, inventoryId, deliveryId);
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
                        SystemDateTime = Convert.ToDateTime(reader["SystemDateTime"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect trip info",exception);
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
                CommandObj.Parameters.AddWithValue("@DriverName", model.DriverName);
                CommandObj.Parameters.AddWithValue("@DriverPhone", model.DriverPhone);
                CommandObj.Parameters.AddWithValue("@Transportation", model.Transportation);
                CommandObj.Parameters.AddWithValue("@VehicleNo", model.VehicleNo);
                CommandObj.Parameters.AddWithValue("@TransportationCost", model.TransportationCost);
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

        public ViewDispatchModel GetDispatchByTripId(long tripId)
        {
            try
            {
                ViewDispatchModel model = null;
                CommandObj.CommandText = "UDSP_GetDispatchById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    model=new ViewDispatchModel
                    {
                        TripId = tripId,
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        DispatchRef = reader["DispatchRef"].ToString(),
                        DispatchByUserId = Convert.ToInt32(reader["DispatchByUserId"]),
                        DispatchId = Convert.ToInt64(reader["DispatchId"]),
                        DispatchDate = Convert.ToDateTime(reader["SystemDatetime"]),
                        TransactionRef = reader["TransactionRef"].ToString()
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get dispatch by trip id",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewDispatchModel> GetAllReceiveableItemsByTripAndBranchId(long tripId, int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllReceiveableItemsByTripAndBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
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
                        TripId = tripId,
                        TransactionRef = reader["TransactionRef"].ToString()

                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Coluld not collect receivable product list by trip and product id", exception);
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