using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.Areas.Sales.DAL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Logs;
using NBL.Models.ViewModels;

namespace NBL.Areas.Sales.DAL
{
    public class InvoiceGateway:DbGateway,IInvoiceGateway
    {

        public int GetMaxInvoiceNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxInvoiceNoOfCurrentYear";
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
                throw new Exception("Could not collect max invoice no of  current year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int GetMaxInvoiceNo()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxInvoiceNo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int maxInvoiceNo = 0; 
                if (reader.Read())
                {
                    maxInvoiceNo = Convert.ToInt32(reader["MaxInvoiceNo"]);
                }
                reader.Close();
                return maxInvoiceNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect max invoice no", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedOrdersByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat=Convert.ToDecimal(reader["Vat"]),
                        NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CompanyId = companyId,
                        BranchId=Convert.ToInt32(reader["BranchId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not get Invoiced orders by company id due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxVoucherNoByTransactionInfix(string infix) 
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
                throw new Exception("Could not collect max voucher no", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int accountAffected = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveInvoicedOrder";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceDateTime", anInvoice.InvoiceDateTime);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", anInvoice.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", anInvoice.TransactionRef);
                CommandObj.Parameters.AddWithValue("@InvoiceNo", anInvoice.InvoiceNo);
                CommandObj.Parameters.AddWithValue("@TotalQuantity", orderItems.Sum(n => n.Quantity));
                CommandObj.Parameters.AddWithValue("@ClientId", anInvoice.ClientId);
                CommandObj.Parameters.AddWithValue("@InvoiceByUserId", anInvoice.InvoiceByUserId);
                CommandObj.Parameters.AddWithValue("@BranchId", anInvoice.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", anInvoice.CompanyId);
                CommandObj.Parameters.AddWithValue("@VoucherNo", anInvoice.VoucherNo);
                CommandObj.Parameters.Add("@InvoiceId", SqlDbType.Int);
                CommandObj.Parameters["@InvoiceId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int invoiceId = Convert.ToInt32(CommandObj.Parameters["@InvoiceId"].Value);
                //------------------
                //CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.Int);
                //CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;

                // int accountMasterId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                int rowAffected = SaveInvoiceDetails(orderItems, invoiceId);

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
                //        accountAffected += SaveInvoiceDetailsToAccountsDetails(anInvoice, accountMasterId);
                //    }

                    //if (anInvoice.SpecialDiscount != 0)
                    //{
                    //    for (int i = 1; i <= 2; i++)
                    //    {
                    //        if (i == 1)
                    //        {
                    //            anInvoice.TransactionType = "Dr";
                    //            anInvoice.Amounts = anInvoice.SpecialDiscount;
                    //            anInvoice.SubSubSubAccountCode = anInvoice.DiscountAccountCode;
                    //        }
                    //        else
                    //        {
                    //            anInvoice.TransactionType = "Cr";
                    //            anInvoice.Amounts = anInvoice.SpecialDiscount * (-1);
                    //            anInvoice.SubSubSubAccountCode = anInvoice.ClientAccountCode;
                    //        }
                    //        accountAffected += SaveInvoiceDetailsToAccountsDetails(anInvoice, accountMasterId);
                    //    }
                    //}
                    //---------------------------------END----------
                    //if (anInvoice.Discount != 0)
                    //{
                    //    for (int i = 1; i <= 2; i++)
                    //    {
                    //        if (i == 1)
                    //        {
                    //            anInvoice.TransactionType = "Dr";
                    //            anInvoice.Amounts = anInvoice.Discount;
                    //            anInvoice.SubSubSubAccountCode = "2601041";
                    //        }
                    //        else
                    //        {
                    //            anInvoice.TransactionType = "Cr";
                    //            anInvoice.Amounts = anInvoice.Discount * (-1);
                    //            anInvoice.SubSubSubAccountCode = anInvoice.ClientAccountCode;
                    //        }
                    //        accountAffected += SaveInvoiceDetailsToAccountsDetails(anInvoice, accountMasterId);
                    //    }
                    //}

                //}
                //if (accountAffected > 0)
                //{
                //    sqlTransaction.Commit();
                //}
                //return accountAffected;

                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (SqlException sqlException)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Invoiced order info due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Invoiced order info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveInvoiceDetailsToAccountsDetails(Invoice anInvoice,int accountMasterId)
        {

            var i=0;
            CommandObj.CommandText = "UDSP_SaveInvoiceDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", anInvoice.SubSubSubAccountCode);
            CommandObj.Parameters.AddWithValue("@TransactionType", anInvoice.TransactionType);
            CommandObj.Parameters.AddWithValue("@Amount", anInvoice.Amounts);
            CommandObj.Parameters.AddWithValue("@Explanation", anInvoice.Explanation);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;
           

        }

        private int SaveInvoiceDetails(IEnumerable<OrderItem> orderItems, int invoiceId)
        {
            int i = 0;
            foreach (var order in orderItems)
            {
                CommandObj.CommandText = "spSaveInvoiceDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InvoiceId", invoiceId);
                CommandObj.Parameters.AddWithValue("@ProductId", order.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", order.Quantity);
                CommandObj.Parameters.AddWithValue("@VatId", order.VatId);
                CommandObj.Parameters.AddWithValue("@DiscountId", order.DiscountId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }


        public ICollection<Invoice> GetLatestInvoicedOrdersByDistributionPoint(int distributionPointId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetLatestInvoicedOrdersByDistributionPoint";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DistributionPointId", distributionPointId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientName = reader["Name"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get latest Invoiced orders by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<Invoice> GetAllInvoicedOrdersByDistributionPoint(int distributionPointId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllInvoicedOrdersByDistributionPoint";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DstributionPointId", distributionPointId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientName = reader["Name"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by distribution point id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<Invoice> GetAllInvoicedOrdersByCompanyIdAndStatus(int companyId, int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllInvoicedOrderByCompanyIdAndStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Status", status);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CompanyId = companyId,
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },
                       
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not get Invoiced orders by company id and Status due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by company id and Status", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetInvoicedOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        BranchId = branchId,
                        CompanyId=companyId,
                        TransactionRef=reader["TransactionRef"].ToString(),
                        SysDateTime=Convert.ToDateTime(reader["SysDateTime"]),
                        ClientId=Convert.ToInt32(reader["ClientId"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientName = reader["Name"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType=new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            } 
                        },
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch(Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchCompanyAndUserId(int branchId, int companyId, int invoiceByUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetInvoicedOrdersByBranchCompanyAndUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@InvoiceByUserId", invoiceByUserId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = invoiceByUserId,
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        BranchId = branchId,
                        CompanyId = companyId,
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by branch,company and user Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetInvoicedOrdersByUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceByUserId", invoiceByUserId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ClientId = Convert.ToInt32(reader["ClientId"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by user id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedRefferencesByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        Amounts = Convert.ToDecimal(reader["Amount"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        ClientId = clientId
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoice reference by client id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedOrderDetailsByInvoiceId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceId", invoiceId);
                List<InvoiceDetails> invoiceDetails = new List<InvoiceDetails>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    InvoiceDetails invoice = new InvoiceDetails
                    {
                        InvoiceDetailsId=Convert.ToInt32(reader["InvoiceDetailsId"]),
                        InvoiceId=Convert.ToInt32(reader["InvoiceId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                        DiscountId = Convert.ToInt64(reader["DiscountId"]),
                        Discount = Convert.ToDecimal(reader["DiscountAmount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        VatId = Convert.ToInt64(reader["VatId"])
                    };
                    invoiceDetails.Add(invoice);
                }
                reader.Close();
                return invoiceDetails;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedOrderDetailsByInvoiceRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceRef", invoiceRef);
                List<InvoiceDetails> invoiceDetails = new List<InvoiceDetails>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    InvoiceDetails invoice = new InvoiceDetails
                    {
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"])

                    };
                    invoiceDetails.Add(invoice);
                }
                reader.Close();
                return invoiceDetails;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by invoice ref", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Invoice GetInvoicedOrderByInvoiceId(int invoiceId)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedOrderByInvoiceId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceId", invoiceId);
                Invoice invoice = new Invoice();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                      invoice = new Invoice
                      {
                          InvoiceId = invoiceId,
                          InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                          Amounts = Convert.ToDecimal(reader["Amounts"]),
                          Discount= Convert.ToDecimal(reader["Discount"]),
                          SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                          Vat = Convert.ToDecimal(reader["Vat"]),
                          InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                          InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                          InvoiceRef = reader["InvoiceRef"].ToString(),
                          Quantity = Convert.ToInt32(reader["TotalQuantity"]),
                          InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                          Cancel = Convert.ToChar(reader["Cancel"]),
                          BranchId = Convert.ToInt16(reader["BranchId"]),
                          CompanyId = Convert.ToInt16(reader["CompanyId"]),
                          TransactionRef = reader["TransactionRef"].ToString(),
                          SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                          ClientId = Convert.ToInt32(reader["ClientId"]),
                          OrderId = Convert.ToInt64(reader["OrderId"]),
                          Client = new Client
                          {
                              ClientId = Convert.ToInt32(reader["ClientId"]),
                              ClientName = reader["Name"].ToString(),
                              CommercialName = reader["CommercialName"].ToString(),
                              SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                              ClientType = new ClientType
                              {
                                  ClientTypeId=Convert.ToInt32(reader["ClientTypeId"]),
                                  ClientTypeName = reader["ClientTypeName"].ToString()
                              }
                          }
                      };

                }
                reader.Close();
                return invoice;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Invoiced orders by Invoice id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewProduct> GetDeliveredProductsByInvoiceRef(string invoiceRef)
        {
            try
            {
                List<ViewProduct> products=new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_GetDeliveredProductsByInvoiceRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceRef", invoiceRef);
                ConnectionObj.Open();
               
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
                
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get Delivered Qty by  invoice ref", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<Invoice> GetInvoicedOrdersByCompanyIdAndDate(int companyId, DateTime date)
        {
            try
            {
                CommandObj.CommandText = "spGetInvoicedOrdersByCompanyIdAndDate";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Date", date);
                List<Invoice> invoiceList = new List<Invoice>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef = reader["InvoiceRef"].ToString(),
                        InvoiceStatus = Convert.ToInt16(reader["InvoiceStatus"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CompanyId = companyId,
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    };
                    invoiceList.Add(invoice);
                }
                reader.Close();
                return invoiceList;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not get Invoiced orders by company id and date due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get Invoiced orders by company id and date", exception);
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