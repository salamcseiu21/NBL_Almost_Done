using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Returns;

namespace NBL.DAL
{
   public class ProductReturnGateway:DbGateway,IProductReturnGateway 
    {
        public int SaveReturnProduct(ReturnModel returnModel)
        {
            int rowAffected = 0;
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveSalesReturnProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", returnModel.ClientId?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@SalesReturnNo", returnModel.ReturnNo);
                CommandObj.Parameters.AddWithValue("@SalesReturnRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@BranchId", returnModel.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", returnModel.CompanyId);
                CommandObj.Parameters.AddWithValue("@Remarks", returnModel.Remarks?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReturnIssueByUserId", returnModel.ReturnIssueByUserId);
                CommandObj.Parameters.AddWithValue("@TotalQuantity", returnModel.Products.Sum(n=>n.Quantity));
                CommandObj.Parameters.AddWithValue("@CurrentApproverRoleId", returnModel.CurrentApproverRoleId);
                CommandObj.Parameters.AddWithValue("@CurrentApprovalLevel", returnModel.CurrentApprovalLevel);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var masterId = Convert.ToInt64(CommandObj.Parameters["@MasterId"].Value);
                var list=returnModel.Products.GroupBy(n => n.DeliveryRef).ToList();
                foreach (IGrouping<string, ReturnProduct> products in list)
                {
                    var deliveryref = products.Key;
                    long refId = SaveReturnDeliveryRef(deliveryref,masterId);
                    if (refId > 0)
                    {
                        var newListByDeleveryRef = returnModel.Products.FindAll(n => n.DeliveryRef == deliveryref);
                        rowAffected += SaveReturnProductDetails(newListByDeleveryRef, masterId, refId);
                    }
                    else
                    {
                        rowAffected = 0;
                    }
                    

                }
              
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
                throw new Exception("Could not save return products",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private long SaveReturnDeliveryRef(string deliveryref, long masterId)
        {
            CommandObj.Parameters.Clear();
            CommandObj.CommandText = "UDSP_SaveReturnDeliveryRef";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.AddWithValue("@SalesReturnId", masterId);
            CommandObj.Parameters.AddWithValue("@Deliveryref", deliveryref);
            CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
            CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            var i = Convert.ToInt64(CommandObj.Parameters["@MasterId"].Value);
            return i;
        }


        private int SaveReturnProductDetails(List<ReturnProduct> products, long masterId,long refId)
        {
            int i = 0;
            foreach (var item in products)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveSalesReturnDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnId", masterId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@DeliveryId", item.DeliveryId);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", item.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@RefDetailsId", refId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public long GetMaxSalesReturnNoByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxSalesReturnNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                long maxSl = 0;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSl = Convert.ToInt64(reader["MaxSl"]);
                }
                reader.Close();
                return maxSl;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max sales return no", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public long GetMaxSalesReturnRefByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxSalesReturnRefByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                long maxRefNo = 0; 
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxRefNo = Convert.ToInt64(reader["MaxRefNo"]);
                }
                reader.Close();
                return maxRefNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max sales return ref no", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public long GetMaxSalesReturnReceiveRefByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxSalesReturnReceiveRefByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                long maxRefNo = 0;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxRefNo = Convert.ToInt64(reader["MaxRefNo"]);
                }
                reader.Close();
                return maxRefNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max sales return receive ref no", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReturnProductModel> GetSalesReturnProductListToTest()
        {
            try
            {
                CommandObj.CommandText = "UDSP_SalesReturnProductListToTest";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewReturnProductModel> products=new List<ViewReturnProductModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReturnProductModel
                    {
                        ReturnRef = reader["ReturnRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ReceiveSalesReturnDetailsId = Convert.ToInt64(reader["ReceiveSalesReturnDetailsId"]),
                        SalesReturnId = Convert.ToInt64(reader["SalesReturnId"]),
                        ReceiveSalesReturnId = Convert.ToInt64(reader["ReceiveSalesReturnId"])
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Sales return product list",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int AddVerificationNoteToReturnsProduct(long returnRcvDetailsId, string notes, int userUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddVerificationNoteToReturnsProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Notes", notes);
                CommandObj.Parameters.AddWithValue("@UserId", userUserId);
                CommandObj.Parameters.AddWithValue("@ReturnDetailsId", returnRcvDetailsId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not add verification staus to returns product", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReturnProductModel> GetAllVerifiedSalesReturnProducts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllVerifiedSalesReturnProducts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewReturnProductModel> products = new List<ViewReturnProductModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReturnProductModel
                    {
                        ReturnRef = reader["ReturnRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ReceiveSalesReturnDetailsId = Convert.ToInt64(reader["ReceiveSalesReturnDetailsId"]),
                        SalesReturnId = Convert.ToInt64(reader["SalesReturnId"]),
                        ReceiveSalesReturnId = Convert.ToInt64(reader["ReceiveSalesReturnId"]),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        ReturnDate = Convert.ToDateTime(reader["ReturnDateTime"]),
                        VerifiedNotes = reader["Notes"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect verified Sales return product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int ApproveReturnBySalesManager(ReturnModel returnModel)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveSalesReturnBySalesManager";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@NotesByManager", returnModel.NotesByManager);
                CommandObj.Parameters.AddWithValue("@SalesReturnId", returnModel.SalesReturnId);
                CommandObj.Parameters.AddWithValue("@ApproveByManagerUserId", returnModel.ApproveByManagerUserId);
                CommandObj.Parameters.AddWithValue("@ReturnRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@ApproverActionId", returnModel.AproveActionId);
                CommandObj.Parameters.AddWithValue("@LastApproverDatetime", returnModel.LastApproverDatetime);
                CommandObj.Parameters.AddWithValue("@LastApproverRoleId", returnModel.LastApproverRoleId);
                CommandObj.Parameters.AddWithValue("@CurrentApproverRoleId", returnModel.CurrentApproverRoleId);
                CommandObj.Parameters.AddWithValue("@CurrentApprovalLevel", returnModel.CurrentApprovalLevel);
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
                throw new Exception("Could not approve sales returns by sales Manager", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }
        public int ApproveReturnBySalesAdmin(ReturnModel returnModel, decimal lessAmount)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveReturnBySalesAdmin";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@NotesByAdmin", returnModel.NotesByAdmin);
                CommandObj.Parameters.AddWithValue("@SalesReturnId", returnModel.SalesReturnId);
                CommandObj.Parameters.AddWithValue("@ReturnRef", returnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@ApproverActionId", returnModel.AproveActionId);
                CommandObj.Parameters.AddWithValue("@ApproveByAdminUserId", returnModel.ApproveByAdminUserId);
                CommandObj.Parameters.AddWithValue("@LastApproverDatetime", returnModel.LastApproverDatetime);
                CommandObj.Parameters.AddWithValue("@LastApproverRoleId", returnModel.LastApproverRoleId);
                CommandObj.Parameters.AddWithValue("@CurrentApproverRoleId", returnModel.CurrentApproverRoleId);
                CommandObj.Parameters.AddWithValue("@CurrentApprovalLevel", returnModel.CurrentApprovalLevel);
                CommandObj.Parameters.AddWithValue("@IsFinalApproved", returnModel.IsFinalApproved);
                CommandObj.Parameters.AddWithValue("@LessAmount", lessAmount);
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
                throw new Exception("Could not approve sales returns by Sales Admin", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ICollection<ReturnModel> GetAllReturnsByApprovarRoleId(int approverRoleId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllReturnsByApprovarRoleId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ApproverRoleId", approverRoleId);
                List<ReturnModel> models = new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"]) ? null : reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        ClientInfo =DBNull.Value.Equals(reader["ClientInfo"])? null: reader["ClientInfo"].ToString(),

                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal):Convert.ToDecimal(reader["LessAmount"]),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null:Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null:Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null:Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null:Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null:Convert.ToInt32(reader["IsFinalApproved"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns by approver role Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

       

        public ReturnModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetSalesReturnBySalesReturnId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnId", salesReturnId);
                ReturnModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model = new ReturnModel
                    {
                        ClientId =DBNull.Value.Equals(reader["ClientId"])? (int?)null: Convert.ToInt32(reader["ClientId"]),
                        ClientInfo = DBNull.Value.Equals(reader["ClientInfo"]) ? null : reader["ClientInfo"].ToString(),
                        EmployeeInfo = DBNull.Value.Equals(reader["EmployeeInfo"]) ? null : reader["EmployeeInfo"].ToString(),
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"])? null: reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        //ReturnApproveByUserId = Convert.ToInt32(reader["ReturnApproveByUserId"]),
                        //ReturnApproveDateTime = DBNull.Value.Equals(reader["ReturnApproveDate"])
                        //    ? default(DateTime)
                        //    : Convert.ToDateTime(reader["ReturnApproveDate"]),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal) : Convert.ToDecimal(reader["LessAmount"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?) null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null:  Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null : Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null : Convert.ToInt32(reader["IsFinalApproved"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect sales returns by id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public int ReceiveSalesReturnProduct(ViewReturnReceiveModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_ReceiveSalesReturnProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TotalQuantity", model.Products.Count);
                CommandObj.Parameters.AddWithValue("@ReturnRef", model.ReturnModel.ReturnRef);
                CommandObj.Parameters.AddWithValue("@ReturnNo", model.ReturnModel.ReturnNo);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.TransactionRef); 
                CommandObj.Parameters.AddWithValue("@ReceiveByUserId", model.ReceiveByUserId);
                CommandObj.Parameters.AddWithValue("@Notes", model.Notes??(object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@SalesReturnId", model.ReturnModel.SalesReturnId);
                CommandObj.Parameters.Add("@MasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@MasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var masterId = Convert.ToInt64(CommandObj.Parameters["@MasterId"].Value);
                rowAffected = SaveSalesReturnDetails(model.Products,masterId);
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
                throw new Exception("Could not receive sales return products", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        

        private int SaveSalesReturnDetails(List<ScannedProduct> modelProducts, long masterId)
        {
            int i = 0;
            foreach (ScannedProduct product in modelProducts)
            {
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_SaveReceiveSalesReturnDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@MasterId",masterId);
                CommandObj.Parameters.AddWithValue("@ProductId",product.ProductId);
                CommandObj.Parameters.AddWithValue("@BarCode",product.ProductCode);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public ICollection<ReturnModel> GetAll()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllSalesReturn";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ReturnModel> models=new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId=Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks =DBNull.Value.Equals(reader["Remarks"])?null: reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        ClientInfo = reader["ClientInfo"].ToString()
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
               
            }
        }

        public ICollection<ReturnModel> GetAllReturnsByStatus(int status)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetAllReturnsByStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Status", status);
                List<ReturnModel> models = new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"]) ? null : reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?) null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?) null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal) : Convert.ToDecimal(reader["LessAmount"]),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null : Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null : Convert.ToInt32(reader["IsFinalApproved"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns by status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ICollection<ReturnModel> GetAllFinalApprovedReturnsList()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllFinalApprovedReturnsList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ReturnModel> models = new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"]) ? null : reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        ClientInfo = reader["ClientInfo"].ToString(),
                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal) : Convert.ToDecimal(reader["LessAmount"]),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null : Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null : Convert.ToInt32(reader["IsFinalApproved"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns by status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ICollection<ReturnModel> GetAllFinalApprovedGeneralReturnsList()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllFinalApprovedGeneralReturnsList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ReturnModel> models = new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"]) ? null : reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        EmployeeInfo = reader["EmployeeInfo"].ToString(),
                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal) : Convert.ToDecimal(reader["LessAmount"]),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null : Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null : Convert.ToInt32(reader["IsFinalApproved"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns by status", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ICollection<ReturnModel> GetAllGeneralReqReturnsByApprovarRoleId(int approverRoleId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllGeneralReqReturnsByApprovarRoleId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ApproverRoleId", approverRoleId);
                List<ReturnModel> models = new List<ReturnModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ReturnModel
                    {
                        SalesReturnId = Convert.ToInt64(reader["Id"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ReturnRef = reader["ReturnRef"].ToString(),
                        ReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        TotalQuantity = Convert.ToInt32(reader["Quantity"]),
                        Remarks = DBNull.Value.Equals(reader["Remarks"]) ? null : reader["Remarks"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByManagerUserId = DBNull.Value.Equals(reader["ApproveByManagerUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByManagerUserId"]),
                        ApproveByManagerDate = DBNull.Value.Equals(reader["ApproveByManagerDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByManagerDate"]),
                        ApproveByAdminDate = DBNull.Value.Equals(reader["ApproveByAdminDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["ApproveByAdminDate"]),
                        ApproveByAdminUserId = DBNull.Value.Equals(reader["ApproveByAdminUserId"]) ? (int?)null : Convert.ToInt32(reader["ApproveByAdminUserId"]),
                        NotesByManager = DBNull.Value.Equals(reader["NotesByManager"]) ? null : reader["NotesByManager"].ToString(),
                        NotesByAdmin = DBNull.Value.Equals(reader["NotesByAdmin"]) ? null : reader["NotesByAdmin"].ToString(),
                        ReturnStatus = Convert.ToInt32(reader["Status"]),
                        ReturnIssueByUserId = Convert.ToInt32(reader["ReturnIssueByUserId"]),
                        EmployeeInfo = DBNull.Value.Equals(reader["EmployeeInfo"]) ? null : reader["EmployeeInfo"].ToString(),

                        LessAmount = DBNull.Value.Equals(reader["LessAmount"]) ? default(decimal) : Convert.ToDecimal(reader["LessAmount"]),
                        LastApproverDatetime = DBNull.Value.Equals(reader["LastApproverDatetime"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastApproverDatetime"]),
                        LastApproverRoleId = DBNull.Value.Equals(reader["LastApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["LastApproverRoleId"]),
                        CurrentApproverRoleId = DBNull.Value.Equals(reader["CurrentApproverRoleId"]) ? (int?)null : Convert.ToInt32(reader["CurrentApproverRoleId"]),
                        CurrentApprovalLevel = DBNull.Value.Equals(reader["CurrentApprovalLevel"]) ? (int?)null : Convert.ToInt32(reader["CurrentApprovalLevel"]),
                        IsFinalApproved = DBNull.Value.Equals(reader["IsFinalApproved"]) ? (int?)null : Convert.ToInt32(reader["IsFinalApproved"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns by approver role Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ICollection<ViewReturnDetails> GetGeneralReqReturnDetailsById(long returnId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetGeneralReqReturnDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReturnId", returnId);
                List<ViewReturnDetails> models = new List<ViewReturnDetails>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewReturnDetails
                    {
                        SalesReturnId = Convert.ToInt64(reader["ReturnId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        DeliveredDateTime =DBNull.Value.Equals(reader["DeliveryDate"])? (DateTime?)null: Convert.ToDateTime(reader["DeliveryDate"]),
                        ReturnDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect sales returns details", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }


        public ICollection<ViewReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetSalesReturnDetailsBySalesReturnId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnId", salesReturnId);
                List<ViewReturnDetails> models = new List<ViewReturnDetails>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    models.Add(new ViewReturnDetails
                    {
                        SalesReturnId = Convert.ToInt64(reader["ReturnId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveryDate"]),
                        ReturnDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDate"]),
                        VatAmount = Convert.ToDecimal(reader["VatAmount"]),
                        DiscountAmount = Convert.ToDecimal(reader["UnitDiscount"]),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        OrderRef = reader["OrderRef"].ToString(),
                        SepecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"])

                    });
                }
                reader.Close();
                return models;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect sales returns details", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

        public ViewReturnDetails GetReturnDetailsById(long salsesReturnDetailsId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetSalesReturnDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SalesReturnDetailsId", salsesReturnDetailsId);
                ViewReturnDetails model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    model = new ViewReturnDetails
                    {
                        SalesReturnId = Convert.ToInt64(reader["SalesReturnId"]),
                        SalesReturnNo = Convert.ToInt64(reader["SalesReturnNo"]),
                        ReturnDateTime = Convert.ToDateTime(reader["ReturnDateTime"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryId = Convert.ToInt64(reader["DeliveryId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SalsesReturnDetailsId = Convert.ToInt64(reader["SalsesReturnDetailsId"]),
                        SalesAdminUserId = Convert.ToInt32(reader["SalesAdminUserId"]),
                        ApproveBySalesAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        ApproveByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        OrderByUserId = Convert.ToInt32(reader["OrderByUserId"]),
                        OrderDateTime = Convert.ToDateTime(reader["OrderDateTime"]),
                        OrderId = Convert.ToInt64(reader["OrderId"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveredDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        SalesReturnRef=reader["SalesReturnRef"].ToString(),
                       
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect sales returns details", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();

            }
        }

       

        public int Add(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(ReturnModel model)
        {
            throw new NotImplementedException();
        }
    }
}
