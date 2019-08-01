using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Approval;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Employees;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.DAL
{
    public class ProductGateway:DbGateway,IProductGateway
    {
        public IEnumerable<Product> GetAll()
        {
            try
            {
                CommandObj.CommandText = "spGetAllProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Product> products = new List<Product>();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        Vat = Convert.ToDecimal(reader["VatAmount"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        LastPriceUpdateDate = Convert.ToDateTime(reader["PriceUpdateDate"]),
                        LastVatUpdateDate = Convert.ToDateTime(reader["VatUpdateDate"])
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

        public IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllProductByBranchAndCompanyId";
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
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Vat = Convert.ToDecimal(reader["Vat"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        
        public Product GetProductByProductAndClientTypeId(int productId,int clientTypeId)
        {
            try
            {
                CommandObj.CommandText = "spGetProductByProductAndClientTypeId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientTypeId", clientTypeId);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Product product = new Product();
                if (reader.Read())
                {
                    product = new Product 
                    {
                        ProductId = productId,
                        ProductName = reader["ProductName"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Vat = Convert.ToDecimal(reader["VatAmount"]),
                        VatId=Convert.ToInt32(reader["VatId"]),
                        DiscountAmount= Convert.ToDecimal(reader["DiscountAmount"]),
                        DiscountId=Convert.ToInt32(reader["DiscountId"]),
                        SalePrice=Convert.ToDecimal(reader["SalePrice"]),
                        ProductDetailsId=Convert.ToInt32(reader["ProductDetailsId"])

                    };
                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxTransferRequisitionNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxTransferRequisitionNoOfCurrentYear";
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
                throw new Exception("Could not collect max transfer issue no of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        //-----------------Load deliverable Issue list----------------
        public IEnumerable<TransferIssue> GetDeliverableTransferIssueList()
        {
            try
            {
                CommandObj.CommandText = "spGetDeliverableTransferIssueList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssue> issueList = new List<TransferIssue>(); 
                while (reader.Read())
                {
                    issueList.Add(new TransferIssue
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    });
                }

                reader.Close();
                return issueList;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int ApproveTransferIssue(TransferIssue transferIssue)
        {
            try
            {
                CommandObj.CommandText = "spApproveTransferIssue";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transferIssue.TransferIssueId);
                CommandObj.Parameters.AddWithValue("@ApproveByUserId", transferIssue.ApproveByUserId);
                CommandObj.Parameters.AddWithValue("@ApproveDateTime", transferIssue.ApproveDateTime);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int IssueProductToTransfer(TransferIssue aTransferIssue)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveTransferIssue";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueDate", aTransferIssue.TransferIssueDate);
                CommandObj.Parameters.AddWithValue("@FromBranchId", aTransferIssue.FromBranchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aTransferIssue.ToBranchId);
                CommandObj.Parameters.AddWithValue("@IssueByUserId", aTransferIssue.IssueByUserId);
                CommandObj.Parameters.AddWithValue("@TransferIssueRef", aTransferIssue.TransferIssueRef);
                CommandObj.Parameters.AddWithValue("@QuantityIssued", aTransferIssue.Products.Sum(n => n.Quantity));
                CommandObj.Parameters.Add("@TransferIssueId", SqlDbType.Int);
                CommandObj.Parameters["@TransferIssueId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int transferIssueId = Convert.ToInt32(CommandObj.Parameters["@TransferIssueId"].Value);
                int rowAffected = SaveTransferIssueDetails(aTransferIssue.Products, transferIssueId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Transfer issue Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id)
        {
            try
            {
                CommandObj.CommandText = "spGetTransferIssueDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssueDetails> products = new List<TransferIssueDetails>();
                while (reader.Read())
                {
                    products.Add(new TransferIssueDetails
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        TransferIssueDetailsId = Convert.ToInt32(reader["TransferIssueDetailsId"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued product list by id ", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<TransferIssue> GetTransferIssueList()  
        {
            try
            {
                CommandObj.CommandText = "spGetTransferIssueList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssue> issueList = new List<TransferIssue>();
                while (reader.Read())
                {
                    issueList.Add(new TransferIssue
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    });
                }

                reader.Close();
                return issueList;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public TransferIssue GetTransferIssueById(int transferIssueId) 
        {
            try
            {
                CommandObj.CommandText = "spGetTransferIssueById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transferIssueId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                TransferIssue issue=new TransferIssue();
                if (reader.Read())
                {
                    issue = new TransferIssue
                    {
                        TransferIssueId =transferIssueId,
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    };
                }
                reader.Close();
                return issue;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issue by id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }

        }

        public TransferIssue GetDeliverableTransferIssueById(int transerIssueId)
        {

            try
            {
                CommandObj.CommandText = "spGetDeliverableTransferIssueById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transerIssueId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                TransferIssue issue=new TransferIssue();
                if (reader.Read())
                {
                    issue = new TransferIssue
                    {
                        TransferIssueId = transerIssueId,
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    };
                }

                reader.Close();
                return issue;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issue by id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveTransferIssueDetails(List<Product> products, int transferIssueId)
        {
            int i = 0;
            foreach (Product product in products)
            {
                CommandObj.CommandText = "spSaveTransferIssueDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transferIssueId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

      
        public int GetProductMaxSerialNo()
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "spGetProductMaxSerialNo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                   maxSlno=Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return maxSlno;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not get  product max serial", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId)
        {
          
                try
                {
                    CommandObj.CommandText = "spGetPoructByProductCategoryId";
                    CommandObj.Parameters.AddWithValue("@ProductCategoryId", productCategoryId);
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
                            ProductAddedDate = Convert.ToDateTime(reader["ProductAddedDate"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ProductCategoryName = reader["ProductCategoryName"].ToString(),
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                            ProductTypeName = reader["ProductTypeName"].ToString(),
                            Unit = reader["Unit"].ToString(),
                            UnitInStock = Convert.ToInt32(reader["UnitInStock"]),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            DealerComision = Convert.ToDecimal(reader["DealerComision"]),
                            DealerPrice = Convert.ToDecimal(reader["DealerPrice"]),
                            Vat = Convert.ToDecimal(reader["Vat"]),
                            DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"])

                        });
                    }
                    reader.Close();
                    return products;
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            
        }
        public int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model)
        {

            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spTransferProductToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                CommandObj.Parameters.AddWithValue("@FromBranchId", model.FromBranchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@TransctionId", model.TransactionId);
                CommandObj.Parameters.AddWithValue("@Transportation", model.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", model.DriverName);
                CommandObj.Parameters.AddWithValue("@TransportationCost", model.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", model.VehicleNo);
                //CommandObj.Parameters.Add("@InventoryMasterIdRE", SqlDbType.Int);
                //CommandObj.Parameters["@InventoryMasterIdRE"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@InventoryMasterId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
               // int inventoryMasterIdRe = Convert.ToInt32(CommandObj.Parameters["@InventoryMasterIdRE"].Value);  
                int inventoryMasterId = Convert.ToInt32(CommandObj.Parameters["@InventoryMasterId"].Value);
                int od = SaveTransferDetails(transactionModels,inventoryMasterId);
                if (od > 0)
                {
                    sqlTransaction.Commit();
                }
                return od;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Transfer Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveTransferDetails(List<TransactionModel> transactionModels, int inventoryMasterId)
        {
            int i = 0;
            foreach (var order in transactionModels)
            {
                CommandObj.CommandText = "spSaveTransferDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", order.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", order.Quantity);
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public int Save(Product aProduct)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddNewProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", aProduct.CompanyId);
                CommandObj.Parameters.AddWithValue("@ProductTypeId", aProduct.ProductTypeId);
                CommandObj.Parameters.AddWithValue("@CategoryId", aProduct.CategoryId);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", aProduct.SubSubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@ProductName", aProduct.ProductName);
                CommandObj.Parameters.AddWithValue("@Unit", aProduct.Unit);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected= Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw  new Exception("Could not Save product info",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ProductDetails GetProductDetailsByProductId(int productId)
        {
            try
            {
                CommandObj.CommandText = "spGetProductDetailsByProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                ProductDetails product = new ProductDetails();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    product.DealerPrice =DBNull.Value.Equals(reader["DealerPrice"])? default(decimal): Convert.ToDecimal(reader["DealerPrice"]);
                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product details", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Product GetProductByProductId(int productId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductByProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                Product product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = productId,
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString()
                        },
                        ProductType = new ProductType
                        {
                          ProductTypeId  = Convert.ToInt32(reader["ProductTypeId"]),
                          ProductTypeName = reader["ProductTypeName"].ToString()
                        }
                      
                    };

                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product details", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxProductionNoteNoByYear(int year)
        {
            try
            {
                int maxProductionNoteNo = 0;
                CommandObj.CommandText = "UDSP_GetMaxProductionNoteNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxProductionNoteNo = Convert.ToInt32(reader["MaxProductionNoteNo"]);
                }
                reader.Close();
                return maxProductionNoteNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max production note no by year", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveProductionNote(ProductionNote productionNote)
        {
            try
            {
               
                CommandObj.CommandText = "UDSP_SaveProductionNote";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductionNoteNo", productionNote.ProductionNoteNo);
                CommandObj.Parameters.AddWithValue("@ProductionNoteRef", productionNote.ProductionNoteRef);
                CommandObj.Parameters.AddWithValue("@ProductionNoteDate", productionNote.ProductionNoteDate);
                CommandObj.Parameters.AddWithValue("@ProductionNoteByUserId", productionNote.ProductionNoteByUserId);
                CommandObj.Parameters.AddWithValue("@ProductId", productionNote.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", productionNote.Quantity);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not save production note", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewProductionNoteModel> PendingProductionNote()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllPendingProducitonNote";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewProductionNoteModel> production=new List<ViewProductionNoteModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    production.Add(new ViewProductionNoteModel
                    {
                        Id = Convert.ToInt32(reader["ProductionNoteId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductionNoteNo = reader["ProductionNoteNo"].ToString(),
                        ProductionNoteRef = reader["ProductionNoteRef"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductionNoteDate = Convert.ToDateTime(reader["ProductionNoteDate"]),
                        Product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"])
                        }
                    });
                }
                reader.Close();
                return production;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending production notes", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

       
        public ICollection<ScannedProduct> GetScannedProductListFromTextFile(string filePath) 
        {

            try
            {
                List<ScannedProduct> barCodes = new List<ScannedProduct>(); 
                // Read a text file using StreamReader
                using (StreamReader sr = new StreamReader(filePath))
                {

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                      var product= GetProductByProductId(Convert.ToInt32(line.Substring(2, 3)));
                        
                        barCodes.Add(
                            new ScannedProduct
                            {
                                ProductCode = line,
                                ProductName = product.ProductName,
                                CategoryId = product.CategoryId,
                                CategoryName = product.ProductCategory.ProductCategoryName,
                                ProductId = Convert.ToInt32(line.Substring(2, 3))

                            });
                    }
                    sr.Close();
                }

                return barCodes;
            }
            catch(Exception exception)
            {
                throw new Exception("Unable to read product text file",exception);
            }
        }

        public bool AddProductToTextFile(string productCode,string filePath)
        {
            try
            {
                using (StreamWriter w = File.AppendText(filePath))
                {
                    w.WriteLine(productCode);
                    w.Flush();
                    return true;
                }
            }
            catch (Exception exception)
            {

                throw new Exception("Could not add product to text file",exception);
            }

        }

        public bool AddProductToInventory(List<Product> products)
        {
            try
            {
                long rowAffected = 0;
                foreach (Product product in products)
                {
                    CommandObj.Parameters.Clear();
                    CommandObj.CommandText = "UDSP_SaveProductToFactoryInventory";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.BigInt);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    ConnectionObj.Open();
                    CommandObj.ExecuteNonQuery();
                    rowAffected += Convert.ToInt64(CommandObj.Parameters["@RowAffected"].Value);
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                }


                return rowAffected > 0;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not save product to inventory", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public List<Product> GetIssuedProductListById(int id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetIssuedProductListById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Product> products = new List<Product>();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"])

                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect issued  product list by id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ScannedProduct GetProductByBarCode(string barCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductByBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", barCode);
                ConnectionObj.Open();
                ScannedProduct scannedProduct = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    scannedProduct=new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString() 
                    };
                }
                reader.Close();
                return scannedProduct;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product by barcode",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
            }
        }

        public int SaveRequisitionInfo(ViewRequisitionModel aRequisitionModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveRequisitionInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionDate", aRequisitionModel.RequisitionDate);
                CommandObj.Parameters.AddWithValue("@RequisitionByUserId", aRequisitionModel.RequisitionByUserId);
                CommandObj.Parameters.AddWithValue("@RequisitionRef", aRequisitionModel.RequisitionRef);
                CommandObj.Parameters.AddWithValue("@Quantity", aRequisitionModel.Products.Sum(n => n.Quantity));
                CommandObj.Parameters.Add("@RequisitionId", SqlDbType.Int);
                CommandObj.Parameters["@RequisitionId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int requisitionId = Convert.ToInt32(CommandObj.Parameters["@RequisitionId"].Value);
                int rowAffected = SaveRequisitionDetails(aRequisitionModel.Products, requisitionId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save requisition Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveRequisitionDetails(List<RequisitionModel> products, int requisitionId)
        {
            int i = 0;
            foreach (var product in products)
            {
                CommandObj.CommandText = "UDSP_SaveRequisitionDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ToBranchId", product.ToBranchId);
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitionId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public int SaveGeneralRequisitionInfo(GeneralRequisitionModel aRequisitionModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveGeneralRequisitionInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionDate", aRequisitionModel.RequisitionDate);
                CommandObj.Parameters.AddWithValue("@RequisitionByUserId", aRequisitionModel.RequisitionByUserId);
                CommandObj.Parameters.AddWithValue("@RequisitionRef", aRequisitionModel.RequisitionRef);
                CommandObj.Parameters.AddWithValue("@RequisitionRemarks", aRequisitionModel.RequisitionRemarks);
                CommandObj.Parameters.AddWithValue("@CurrentApprovalLevel", aRequisitionModel.CurrentApprovalLevel);
                CommandObj.Parameters.AddWithValue("@CurrentApproverUserId", aRequisitionModel.CurrentApproverUserId);
                CommandObj.Parameters.Add("@RequisitionId", SqlDbType.Int);
                CommandObj.Parameters["@RequisitionId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int requisitionId = Convert.ToInt32(CommandObj.Parameters["@RequisitionId"].Value);
                int rowAffected = SaveGeneralRequisitionDetails(aRequisitionModel.RequisitionModels.ToList(), requisitionId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save general requisition Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveGeneralRequisitionDetails(List<RequisitionModel> products, int requisitionId)
        {
            int i = 0;
            foreach (var product in products)
            {
                CommandObj.CommandText = "UDSP_SaveGeneralRequisitionDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@RequisitionForId", product.RequisitionForId);
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitionId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public int GetMaxGeneralRequisitionNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxGeneralRequisitionNoOfCurrentYear";
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
                throw new Exception("Could not collect max requisition no of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewGeneralRequisitionModel> GetAllGeneralRequisitions()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllGeneralRequsitions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewGeneralRequisitionModel> requisitions=new List<ViewGeneralRequisitionModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewGeneralRequisitionModel
                    {
                        
                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        ApproverEmp = reader["ApproverName"].ToString(),
                        CurrentApproverUserId = Convert.ToInt32(reader["CurrentApproverUserId"]),
                        CurrentApprovalLevel = Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionPointId"])?default(int):Convert.ToInt32(reader["DistributionPointId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        IsCancelled = reader["IsCancelled"].ToString(),
                        IsFinalApproved = reader["IsFinalApproved"].ToString(),
                        RequisitionByEmp = reader["RequsitionBy"].ToString(),
                        RequisitionRemarks = reader["RequisitionRemarks"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        LastApproverUserId = DBNull.Value.Equals(reader["LastApproverUserId"])?default(int):Convert.ToInt32(reader["LastApproverUserId"]),
                        LastApproveDateTime = DBNull.Value.Equals(reader["LastApproveDateTime"])?default(DateTime):Convert.ToDateTime(reader["LastApproveDateTime"]),
                        LastApproverEmp = DBNull.Value.Equals(reader["LastApproverEmpName"])?null:reader["LastApproverEmpName"].ToString(),
                        DeliveryStatus = Convert.ToInt32(reader["DeliveryStatus"])

                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect general requisition list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewGeneralRequistionDetailsModel> GetGeneralRequisitionDetailsById(long requisitiionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetGeneralRequisitionDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitiionId);
                List<ViewGeneralRequistionDetailsModel> details=new List<ViewGeneralRequistionDetailsModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    details.Add(new ViewGeneralRequistionDetailsModel
                    {
                        AccountCode = DBNull.Value.Equals(reader["AccountCode"])?null:reader["AccountCode"].ToString(),
                        RequisitionForId = Convert.ToInt32(reader["RequisitionForId"]),
                        Description = reader["Description"].ToString(),
                        GeneralRequisitionDetailsId = Convert.ToInt64(reader["GeneralRequisitionDetailsId"]),
                        GeneralRequisitionId = Convert.ToInt64(reader["GeneralRequisitionId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString()
                    });
                }
                reader.Close();
                return details;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect general requisition details by Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewGeneralRequisitionModel> GetGeneralRequisitionByUserId(int userId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetGeneralRequisitionByUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionByUserId", userId);
                List<ViewGeneralRequisitionModel> requisitions = new List<ViewGeneralRequisitionModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewGeneralRequisitionModel
                    {

                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        CurrentApproverUserId = Convert.ToInt32(reader["CurrentApproverUserId"]),
                        CurrentApprovalLevel = Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionPointId"]) ? default(int) : Convert.ToInt32(reader["DistributionPointId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        IsCancelled = reader["IsCancelled"].ToString(),
                        IsFinalApproved = reader["IsFinalApproved"].ToString(),
                        RequisitionRemarks = reader["RequisitionRemarks"].ToString(),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        LastApproverUserId = DBNull.Value.Equals(reader["LastApproverUserId"]) ? default(int) : Convert.ToInt32(reader["LastApproverUserId"]),
                        LastApproveDateTime = DBNull.Value.Equals(reader["LastApproveDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["LastApproveDateTime"]),
                        DeliveryStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DeliveryId=Convert.ToInt64(reader["DeliveryId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString()

                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect general requisition list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int UpdateGeneralRequisitionQuantity(string id, int quantity)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateGeneralRequisitionQuantity";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@GeneralRequisitionDetailsId", Convert.ToInt64(id));
                CommandObj.Parameters.AddWithValue("@Quantity", quantity);
                ConnectionObj.Open();
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not update general requisition qty", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int RemoveProductByIdDuringApproval(string id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RemoveProductByIdDuringApproval";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@GeneralRequisitionDetailsId", Convert.ToInt64(id));
                ConnectionObj.Open();
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not remove product from general requisition", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public GeneralRequisitionModel GetGeneralRequisitionById(long requisitiionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetGeneralRequisitionById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitiionId);
                GeneralRequisitionModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model=new GeneralRequisitionModel
                    {
                        RequisitionId = requisitiionId,
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionByEmployee = new Employee
                        {
                          Email  = reader["REEmail"].ToString(),
                          EmployeeName = reader["RequisitionBy"].ToString(),
                          EmployeeId = Convert.ToInt32(reader["EmployeeId"])
                        },
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        CurrentApproverUserId = Convert.ToInt32(reader["CurrentApproverUserId"]),
                        CurrentApprovalLevel = Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        DistributionPointId = DBNull.Value.Equals(reader["DistributionPointId"]) ? default(int) : Convert.ToInt32(reader["DistributionPointId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        IsCancelled = reader["IsCancelled"].ToString(),
                        IsFinalApproved = reader["IsFinalApproved"].ToString(),
                        RequisitionRemarks = reader["RequisitionRemarks"].ToString(),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        LastApproverUserId = DBNull.Value.Equals(reader["LastApproverUserId"]) ? default(int) : Convert.ToInt32(reader["LastApproverUserId"]),
                        LastApproveDateTime = DBNull.Value.Equals(reader["LastApproveDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["LastApproveDateTime"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get general requisition by id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ApproveGeneralRequisition(GeneralRequisitionModel model, int nextApproverUser, int nextApprovalLevel,ApprovalDetails approval)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveGeneralRequisition";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@LastApproverUserId", model.CurrentApproverUserId);
                CommandObj.Parameters.AddWithValue("@CurrentApprovalLevel", nextApprovalLevel);
                CommandObj.Parameters.AddWithValue("@CurrentApproverUserId", nextApproverUser);
                CommandObj.Parameters.AddWithValue("@IsFinalApproved", model.IsFinalApproved);
                CommandObj.Parameters.AddWithValue("@ApproverId", approval.ApproverUserId);
                CommandObj.Parameters.AddWithValue("@ApproverActionId", approval.ApprovalActionId);
                CommandObj.Parameters.AddWithValue("@Remarks", approval.Remarks);
                CommandObj.Parameters.AddWithValue("@RequisitionId", model.RequisitionId);
                CommandObj.Parameters.AddWithValue("@IsCancelled", model.IsCancelled);
                CommandObj.Parameters.AddWithValue("@EntryStatus", "E");
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve general requisition", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ApproveGeneralRequisitionByScm(int userId, int distributionPoint,long requisitionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveGeneralRequisitionByScm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.AddWithValue("@DIstributionPointId", distributionPoint);
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitionId);
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
                throw new Exception("Could not approve general requisition", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ApproveGeneralRequisitionBySalesAdmin(int userId, long requisitiionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveGeneralRequisitionBySalesAdmin";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitiionId);
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
                throw new Exception("Could not approve general requisition by Sales Admin", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<object> GetAllProductBySearchTerm(string searchTerm)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllProductBySearchTerm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SearchTerm", searchTerm);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<object> products = new List<object>();
                while (reader.Read())
                {
                    products.Add(new 
                    {

                        val = Convert.ToInt32(reader["ProductId"]),
                        label = reader["ProductName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect product list by search term", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewSoldProduct> GetTempSoldBarcodesFromXmlFile(string filePath)
        {
            try
            {
                List<ViewSoldProduct> products = new List<ViewSoldProduct>();
                var xmlData = XDocument.Load(filePath).Element("BarCodes")?.Elements();
                foreach (XElement element in xmlData)
                {
                    var product = new ViewSoldProduct();
                    var elementValue = element.Elements();
                    var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                    product.BarCode = xElements[0].Value;
                    product.ProductId = Convert.ToInt32(xElements[1].Value);
                    product.ProductName = xElements[2].Value;
                    product.CategoryName = xElements[3].Value;
                    product.SaleDate = Convert.ToDateTime(xElements[4].Value);
                    product.ClientName = xElements[5].Value;
                    product.ClientAccountCode = xElements[6].Value;
                    product.ClientCommercialName = xElements[7].Value;
                    product.DeliveryDate = Convert.ToDateTime(xElements[8].Value);
                    products.Add(product);
                }
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not collect sold product list", exception);
            }
        }

        public int AddBarCodeToTempSoldProductXmlFile(ViewDisributedProduct product, string barcode, string filePath)
        {
            try
            {
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("BarCodes")?.Add(
                    new XElement("BarCode", new XAttribute("Id", barcode),
                        new XElement("Code", barcode),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("ProductCategoryName", product.ProductCategoryName),
                        new XElement("SaleDate", product.SaleDate),
                        new XElement("ClientName", product.ClientName),
                        new XElement("ClientCode", product.ClientAccountCode),
                        new XElement("ClientCommercialName", product.ClientCommercialName),
                        new XElement("DeliveryDate", product.DeliveryDate)


                    ));
                xmlDocument.Save(filePath);
                return 1;
            }
        catch (Exception exception)
        {
            Log.WriteErrorLog(exception);
            throw new Exception("Colud not collect sold product list", exception);
        }
}

        public int GetMaxRequisitionNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxRequisitionNoOfCurrentYear";
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
                throw new Exception("Could not collect max requisition no of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewRequisitionModel> GetRequsitionsByStatus(int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRequisitionsByStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Status", status);
                ConnectionObj.Open();
                List<ViewRequisitionModel> requisitions=new List<ViewRequisitionModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewRequisitionModel
                    {
                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionBy = reader["RequisitionBy"].ToString(),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        Status = status
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get requisition by Status",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public List<RequisitionModel> GetRequsitionDetailsById(long requisitionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRequsitionDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitionId);
                List<RequisitionModel> requisitions=new List<RequisitionModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new RequisitionModel
                    {
                        RequisitionId=requisitionId,
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        RequisitionQty = Convert.ToInt32(reader["RequisitionQty"]),
                        DeliveryQty = Convert.ToInt32(reader["DeliveryQty"]),
                        PendingQty = Convert.ToInt32(reader["PendingQty"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        ToBranch = new Branch
                        {
                            
                            BranchName = reader["BranchName"].ToString(),
                            BranchAddress = reader["BranchAddress"].ToString()
                        }
                    });
                }
                reader.Close();
                return requisitions;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get requisition details by id",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewRequisitionModel> GetRequsitions()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRequisitions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewRequisitionModel> requisitions = new List<ViewRequisitionModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewRequisitionModel
                    {
                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionBy = reader["RequisitionBy"].ToString(),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        RequisitionQty = Convert.ToInt32(reader["RequisitionQty"]),
                        DeliveryQty = Convert.ToInt32(reader["DeliveryQty"]),
                        PendingQty = Convert.ToInt32(reader["PendingQty"]),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        SystemDateTime = DBNull.Value.Equals(reader["SystemDateTime"])?default(DateTime):Convert.ToDateTime(reader["SystemDateTime"])
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get requisition by Status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewDispatchModel> GetDeliverableProductListByTripId(long tripId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeliverableProductListByTripId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TripId", tripId);
                List<ViewDispatchModel> products=new List<ViewDispatchModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewDispatchModel
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString()
                        },
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"])
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get deliverable products by trip id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveMonthlyRequisitionInfo(MonthlyRequisitionModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int i = 0;
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveMonthlyRequisitionInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@MonthlyRequisitionRef", model.RequisitionRef);
                CommandObj.Parameters.AddWithValue("@Quantity", model.Quantity);
                CommandObj.Parameters.AddWithValue("@RequisitionByUserId", model.RequisitionByUserId);
                CommandObj.Parameters.Add("@MonthlyRequisitionId", SqlDbType.BigInt);
                CommandObj.Parameters["@MonthlyRequisitionId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var requisionId = Convert.ToInt64(CommandObj.Parameters["@MonthlyRequisitionId"].Value);
                i = SaveMonthlyRequisitionItems(model, requisionId);
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
                throw new Exception("Could not save monthly Requisiton", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }


        private int SaveMonthlyRequisitionItems(MonthlyRequisitionModel model, long requisionId)
        {
            int i = 0;
            foreach (var product in model.Products)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveMonthlyRequisitionItems";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@MonthlyRequisitionId", requisionId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }
        public ICollection<ViewMonthlyRequisitionModel> GetMonthlyRequsitions()
        {
            try
            {
                List<ViewMonthlyRequisitionModel> requisitions=new List<ViewMonthlyRequisitionModel>();
                CommandObj.CommandText = "UDSP_GetMonthlyRequsitions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewMonthlyRequisitionModel
                    {
                        RequisitionRef = reader["MonthlyRequisitionRef"].ToString(),
                        RequisitionId = Convert.ToInt64(reader["MonthlyRequisitionId"]),
                        RequisitionDate = Convert.ToDateTime(reader["SystemDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionBy = reader["RequisitionBy"].ToString()
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
               
                throw new Exception("Could not collect monthly Requisiton", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxMonnthlyRequisitionNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxMonnthlyRequisitionNoOfCurrentYear";
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
                throw new Exception("Could not collect max Monthly requisition no of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public ICollection<RequisitionItem> GetMonthlyRequsitionItemsById(long requisitionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMonthlyRequsitionItemsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionId", requisitionId);
                List<RequisitionItem> requisitions=new List<RequisitionItem>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new RequisitionItem
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString()
                        
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect monthly requisition items by id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                CommandObj.CommandText = "spGetAllProductList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Product> products = new List<Product>();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
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

        public int SaveProductDetails(ViewCreateProductDetailsModel model)
        {
            try
            {
                CommandObj.CommandText = "spSaveProductDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                CommandObj.Parameters.AddWithValue("@ProductId", model.ProductId);
                CommandObj.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
                CommandObj.Parameters.AddWithValue("@UserId", model.UpdatedByUserId);
                CommandObj.Parameters.AddWithValue("@UpdatedAt", model.UpdatedDate);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return i;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not save product details", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public List<Product> GetAllProductionAbleProductByDateCode(string productionDateCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllProductionAbleProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DateCode", productionDateCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Product> products = new List<Product>();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
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

        public List<Product> GetTempReplaceProducts(string filePath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewRequisitionModel> GetPendingRequsitions()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetPendingRequsitions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewRequisitionModel> requisitions = new List<ViewRequisitionModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewRequisitionModel
                    {
                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionBy = reader["RequisitionBy"].ToString(),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        RequisitionQty = Convert.ToInt32(reader["RequisitionQty"]),
                        DeliveryQty = Convert.ToInt32(reader["DeliveryQty"]),
                        PendingQty = Convert.ToInt32(reader["PendingQty"]),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        SystemDateTime = DBNull.Value.Equals(reader["SystemDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["SystemDateTime"])
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get requisition by Status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewDispatchModel> GetAllDispatchList()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDispatchList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewDispatchModel> models=new List<ViewDispatchModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewDispatchModel
                    {
                        DispatchId = Convert.ToInt64(reader["DispatchId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        DispatchRef = reader["DispatchRef"].ToString(),
                        DispatchDate = Convert.ToDateTime(reader["SystemDateTime"]),
                        DispatchByUserId = Convert.ToInt32(reader["DispatchByUserId"]),
                        DispatchBy = reader["EmployeeName"].ToString()
                    });
                }
                reader.Close();
                return models;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect dispatch List", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewRequisitionModel> GetLatestRequisitions()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetLatestRequisitions";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewRequisitionModel> requisitions = new List<ViewRequisitionModel>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    requisitions.Add(new ViewRequisitionModel
                    {
                        RequisitionId = Convert.ToInt64(reader["RequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionBy = reader["RequisitionBy"].ToString(),
                        RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]),
                        RequisitionQty = Convert.ToInt32(reader["RequisitionQty"]),
                        DeliveryQty = Convert.ToInt32(reader["DeliveryQty"]),
                        PendingQty = Convert.ToInt32(reader["PendingQty"]),
                        RequisitionRef = reader["RequisitionRef"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        SystemDateTime = DBNull.Value.Equals(reader["SystemDateTime"]) ? default(DateTime) : Convert.ToDateTime(reader["SystemDateTime"])
                    });
                }
                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get requisition by Status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveTransferRequisitionInfo(TransferRequisition aRequisitionModel)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveTransferRequisitionInfo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RequisitionDate", aRequisitionModel.TransferRequisitionDate);
                CommandObj.Parameters.AddWithValue("@RequisitionByUserId", aRequisitionModel.RequisitionByUserId);
                CommandObj.Parameters.AddWithValue("@RequisitionByBranchId", aRequisitionModel.RequisitionByBranchId);
                CommandObj.Parameters.AddWithValue("@RequisitionToBranchId", aRequisitionModel.RequisitionToBranchId);
                CommandObj.Parameters.AddWithValue("@RequisitionRef", aRequisitionModel.TransferRequisitionRef);
                CommandObj.Parameters.AddWithValue("@Quantity", aRequisitionModel.Products.Sum(n => n.Quantity));
                CommandObj.Parameters.Add("@RequisitionId", SqlDbType.Int);
                CommandObj.Parameters["@RequisitionId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var requisitionId = Convert.ToInt64(CommandObj.Parameters["@RequisitionId"].Value);
                int rowAffected = SaveTransferRequisitionDetails(aRequisitionModel.Products, requisitionId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not save transfer requisition info", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<TransferRequisition> GetTransferRequsitionByStatus(int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTransferRequsitionByStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Status", status);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferRequisition> requisitions = new List<TransferRequisition>();
                while (reader.Read())
                {
                    requisitions.Add(new TransferRequisition
                    {
                        RequisitionByBranchId = Convert.ToInt32(reader["RequisitionByBranchId"]),
                        RequisitionToBranchId = Convert.ToInt32(reader["RequisitionToBranchId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransferRequisitionRef = reader["TransferRequisitionRef"].ToString(),
                        TransferRequisitionDate = Convert.ToDateTime(reader["TransferRequisitionDate"]),
                        TransferRequisitionId = Convert.ToInt64(reader["TransferRequisitionId"]),
                        RequisitionByUserId = Convert.ToInt32(reader["RequisitionByUserId"]),
                        RequisitionByBranch = new ViewBranch
                        {
                            BranchName = reader["RequisitionByBranchName"].ToString(),
                            Title = reader["RequisitionByBranchTitle"].ToString(),
                            BranchId = Convert.ToInt32(reader["RequisitionByBranchId"])
                        }
                    });
                }

                reader.Close();
                return requisitions;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get transfer requisition by status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<TransferRequisitionDetails> GetTransferRequsitionDetailsById(long transferRequisitionId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTransferRequsitionDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TarnsferRequsisitonId", transferRequisitionId);
                ConnectionObj.Open();
                List<TransferRequisitionDetails> details=new List<TransferRequisitionDetails>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    details.Add(new TransferRequisitionDetails
                    {
                        TransferRequisitionId = transferRequisitionId,
                        TransferRequisitionDetailsId = Convert.ToInt64(reader["TransferRequisitionDetailsId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["ProductWiseQty"]),
                        RequisitionByBranchId = Convert.ToInt32(reader["RequisitionByBranchId"])
                    });
                }
                reader.Close();
                return details;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get transfer requisition details by requisition Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int RemoveProductRequisitionProductById(long id)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RemoveProductRequisitionProductById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RdId", id);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get remove product form  requisition  by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int UpdateRequisitionQuantity(long id, int quantity)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateRequisitionQuantity";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@RdId", id);
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
                throw new Exception("Could not  update product  requisition qty  by Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int ApproveRequisition(long id, ViewUser user)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveRequisition";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TrId", id);
                CommandObj.Parameters.AddWithValue("@UserId", user.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve requisition", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public List<ViewTransferProductDetails> TransferReceiveableDetails(long transferId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTransferReceiveableDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferId", transferId);
                ConnectionObj.Open();
                List<ViewTransferProductDetails> details = new List<ViewTransferProductDetails>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    details.Add(new ViewTransferProductDetails
                    {
                        TransferId = transferId,
                        TransferItemId = Convert.ToInt64(reader["TransferItemId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        
                        Quantity = Convert.ToInt32(reader["ProductWiseQty"]),
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["ProductCategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString()
                        }
                    });
                }
                reader.Close();
                return details;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get transfer receiveable details by transfer Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveTransferRequisitionDetails(List<Product> products, long requisitionId)
        {

            int i = 0;
            foreach (var product in products)
            {
                CommandObj.CommandText = "UDSP_SaveTransferRequisitionDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@TransferRequisitionId", requisitionId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }
    }
}