using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Products;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

namespace NBL.DAL
{
    public class OrderGateway:DbGateway,IOrderGateway
    {
        public IEnumerable<Order> GetAll()
        {
            try
                {
                CommandObj.CommandText = "UDSP_GetAllOrders";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Order> orders = new List<Order>();
                while (reader.Read())
                {
                    var anOrder = new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrederRef = reader["OrderRef"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CommercialName = reader["CommercialName"].ToString(),
                            ClientName = reader["Name"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString()
                        },
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = Convert.ToInt32(reader["Branchid"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])

                    };

                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
                catch (SqlException exception)
                {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
                catch (Exception exception)
                {
                throw new Exception("Could not Collect Orders", exception);
            }
                finally
                {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Order> GetOrdersByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrdersByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Order> orders = new List<Order>();
                while (reader.Read())
                {
                    var anOrder = new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrederRef = reader["OrderRef"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = branchId,
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Collect Orders due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetLastestOrdersByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    orders.Add(new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect lastest order by company id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetOrdersByCompanyId(int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrdersByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                var orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    orders.Add(new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = Convert.ToInt32(reader["Branchid"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        CompanyId = companyId,
                        OrederRef = reader["OrderRef"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            ClientName = Convert.ToString(reader["Name"]),
                            CommercialName = reader["CommercialName"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },
                        
                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchAndCompnayId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                var orders = new List<ViewOrder>();
                while (reader.Read())
                {

                    orders.Add(new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrederRef = reader["OrderRef"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = Convert.ToInt32(reader["Branchid"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])

                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrderListByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewInvoicedOrder> orders = new List<ViewInvoicedOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewInvoicedOrder 
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = clientId,
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        InvoiceByUserId = Convert.ToInt32(reader["InvoiceByUserId"]),
                        InvoiceStatus = Convert.ToInt32(reader["InvoiceStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        InvoiceDateTime = Convert.ToDateTime(reader["InvoiceDateTime"]),
                        Amounts=Convert.ToDecimal(reader["Amounts"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount=Convert.ToDecimal(reader["SpecialDiscount"]),
                        Vat=Convert.ToDecimal(reader["Vat"]),
                        NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                        //Cancel=Convert.toChar("Cancel"),
                        InvoiceNo=Convert.ToInt32(reader["InvoiceNo"]),
                        InvoiceRef=reader["InvoiceRef"].ToString(),
                        TransactionRef=reader["TransactionRef"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        OrderStatus=Convert.ToInt32(reader["OrderStatus"])

                    };

                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchIdCompanyIdAndStatus(int branchId, int companyId,int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrdersByBranchIdCompanyIdAndStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Status",status);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {

                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        User = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            EmployeeName = reader["EmployeeName"].ToString()
                        },
                        BranchName = reader["BranchName"].ToString(),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },
                       
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount= Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        VerificationStatus = Convert.ToInt32(reader["VerificationStatus"])
                    };

                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect pending Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewOrder> GetPendingOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetPendingOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {

                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        User = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            EmployeeName = reader["EmployeeName"].ToString()
                        },
                        BranchName = reader["BranchName"].ToString(),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },

                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        VerificationStatus = Convert.ToInt32(reader["VerificationStatus"])
                    };

                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect pending Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId) 
        {
            try
                {
                CommandObj.CommandText = "UDSP_GetOrdersWithClientInformaitonByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    orders.Add(new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]) 

                    });
                }

                reader.Close();
                return orders;
            }
                catch (SqlException exception)
                {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
                catch (Exception exception)
                {
                throw new Exception("Could not Collect Orders", exception);
            }
                finally
                {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrderItemsByOrderId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                var orderItems = new List<OrderItem>();
                while (reader.Read())
                {
              
                    orderItems.Add(new OrderItem
                    {

                        OrderItemId = Convert.ToInt32(reader["OrderItemId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                        ProductName = reader["ProductName"].ToString(),
                        DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        SlNo = Convert.ToInt32(reader["SlNo"]),
                        VatId = Convert.ToInt32(reader["VatId"]),
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        Vat = Convert.ToDecimal(reader["Vat"])
                    });
                }
                reader.Close();
                return orderItems;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Order items due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Order items", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId,int companyId) 
        {
             
                 try
                {
                 
                  CommandObj.CommandText = "UDSP_GetOrdersByBranchAndCompanyIdWithClientInformaiton";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                    CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<ViewOrder> orders = new List<ViewOrder>();
                    while (reader.Read())
                    {
                        var anOrder = new ViewOrder
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            OrderSlipNo = reader["OrderSlipNo"].ToString(),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            BranchName = reader["BranchName"].ToString(),
                            ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                            NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                            NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                            CompanyId = companyId,
                            BranchId = branchId,
                            Vat = Convert.ToDecimal(reader["Vat"]),
                            Amounts = Convert.ToDecimal(reader["Amounts"]),
                            CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                            Status = Convert.ToInt32(reader["OrderStatus"]),
                            ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                            CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                            SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            StatusDescription = reader["StatusDescription"].ToString(),
                            Client =new Client
                            {
                                ClientName = reader["Name"].ToString(),
                                CommercialName = reader["CommercialName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Address = reader["Address"].ToString(),
                                AlternatePhone = reader["AltPhone"].ToString(),
                                SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                                ClientType = new ClientType
                                {
                                    ClientTypeName = reader["ClientTypeName"].ToString(),
                                    ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                                }
                            },
                           
                        };
                        orders.Add(anOrder);
                    }

                    reader.Close();
                    return orders;
                }
                catch (SqlException sqlException)
                {
                    throw new Exception("Could not Collect Orders due to sql Exception", sqlException);
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not Collect Orders", exception);
                }
                finally
                {
                    CommandObj.Parameters.Clear();
                    CommandObj.Dispose();
                    ConnectionObj.Close();
                }
            
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchCompanyAndNsmUserId(int branchId, int companyId,int nsmUserId)
        {

            try
            {

                CommandObj.CommandText = "UDSP_GetOrdersByBranchCompanyAndNsmUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@NsmUserId", nsmUserId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Collect Orders by NSM user Id due to Db Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders by NSM user Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }

        }
        public IEnumerable<ViewOrder> GetOrdersByNsmUserId(int nsmUserId)
        {

            try
            {

                CommandObj.CommandText = "UDSP_GetOrdersByNsmUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@NsmUserId", nsmUserId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["TypeId"])
                            }
                        }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders by NSM user Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }

        }
        public IEnumerable<ViewOrder> GetLatestOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetLastestOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Discount= Convert.ToDecimal(reader["Discount"]),
                        NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId=Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel=reader["ReasonOfCancel"].ToString(),
                        CancelDateTime=Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate=Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ClientType = new ClientType
                        {
                            ClientTypeName = reader["ClientTypeName"].ToString(),
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                        }
                    }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Collect lastest order by branch and company id due to Db Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect lastest order by branch and company id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
        try
        {
            CommandObj.CommandText = "spGetAllOrderDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            ConnectionObj.Open();
            SqlDataReader reader = CommandObj.ExecuteReader();
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            while (reader.Read())
            {
                OrderDetails aModel = new OrderDetails
                {
                    OrderDetailsId = Convert.ToInt32(reader["OrderDetailsId"]),
                    ProductId = Convert.ToInt32(reader["ProductId"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    OrderId = Convert.ToInt32(reader["OrderId"]),
                    ProductCategoryName = reader["ProductCategoryName"].ToString(),
                    SlNo = Convert.ToInt32(reader["SlNo"])

                };
                orderDetails.Add(aModel);
            }
            reader.Close();
            return orderDetails;

        }
    catch (SqlException exception)
    {
    throw new Exception("Could not Collect Order details due to Db Exception", exception);
}
catch (Exception exception)
{
throw new Exception("Could not Collect Orders details", exception);
}
finally
{
CommandObj.Parameters.Clear();
CommandObj.Dispose();
ConnectionObj.Close();
}
        }
        public IEnumerable<OrderDetails> GetOrderDetailsByOrderId(long orderId) 
        {
           
                try
                {
                    CommandObj.CommandText = "spGetOrderDetailsByOrderId";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<OrderDetails> orderDetails = new List<OrderDetails>();
                   

                    while (reader.Read())
                    {
                        OrderDetails aModel = new OrderDetails
                        {

                            OrderDetailsId = Convert.ToInt32(reader["OrderDetailsId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            UnitPrice=Convert.ToDecimal(reader["UnitPrice"]),
                            SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                            ProductName = reader["ProductName"].ToString(),
                            DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString(),
                            SlNo = Convert.ToInt32(reader["SlNo"]),
                            VatId = Convert.ToInt32(reader["VatId"]),
                            DiscountId = Convert.ToInt32(reader["DiscountId"]),
                            Vat = Convert.ToDecimal(reader["Vat"])
                        };

                    orderDetails.Add(aModel);
                    }
                    reader.Close();
                    return orderDetails;

                }
                catch (SqlException exception)
                {
                    throw new Exception("Could not Collect Order details due to Db Exception", exception);
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not Collect Orders details", exception);
                }
                finally
                {
                    CommandObj.Parameters.Clear();
                    CommandObj.Dispose();
                    ConnectionObj.Close();
                }
            
        } 
        public ViewOrder GetOrderByOrderId(int orderId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetOrderByOrderId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewOrder order=null;
                while (reader.Read())
                {
                     order = new ViewOrder
                    {
                         OrderId = orderId,
                         OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                         ClientId = Convert.ToInt32(reader["ClientId"]),
                         OrderSlipNo = reader["OrderSlipNo"].ToString(),
                         OrederRef=reader["OrderRef"].ToString(),
                         UserId = Convert.ToInt32(reader["UserId"]),
                         User = new User
                         {
                             UserId = Convert.ToInt32(reader["UserId"]),
                             UserName = reader["UserName"].ToString(),
                             EmployeeName = reader["EmployeeName"].ToString()
                         },
                         BranchId = Convert.ToInt32(reader["Branchid"]),
                         Amounts = Convert.ToDecimal(reader["Amounts"]),
                         Vat = Convert.ToDecimal(reader["Vat"]),
                         Discount= Convert.ToDecimal(reader["Discount"]),
                         SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                         NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                         Status = Convert.ToInt32(reader["OrderStatus"]),
                         SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                         ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                         ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                         AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                         NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                         Cancel = Convert.ToChar(reader["Cancel"]),
                         CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                         CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                         ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                         StatusDescription = reader["StatusDescription"].ToString(),
                         CompanyId = Convert.ToInt32(reader["CompanyId"]),
                         DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                         DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        
                     };
                }

                reader.Close();
                return order;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Collect Order due to Db Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int GetOrderMaxSerialNoByYear(int year)  
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxOrderSlNoByYear"; 
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
            catch (Exception exception)
            {
                throw new Exception("Could not collect max serial no of order of current Year",exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public Order GetOrderInfoByTransactionRef(string transactionRef)
        {
            try
            {
                CommandObj.CommandText = "spGetOrderByTransactionReference";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionRef", transactionRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Order order = null;
                if(reader.Read())
                {
                    order = new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrederRef = transactionRef,
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = Convert.ToInt32(reader["Branchid"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        NetAmounts=Convert.ToDecimal(reader["NetAmounts"]),
                        Discount= Convert.ToDecimal(reader["Discount"]),
                        Vat =Convert.ToDecimal(reader["Vat"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"])
                    };
                }

                reader.Close();
                return order;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Order due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Order by transaction reference", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ChartModel> GetTotalOrdersOfCurrentYearByCompanyId(int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTotalOrdersOfCurrentYearByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                List<ChartModel> models = new List<ChartModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ChartModel
                    {
                        Total = Convert.ToInt32(reader["TotalOrder"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect total Orders by Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ChartModel> GetTotalOrdersByBranchIdCompanyIdAndYear(int branchId,int companyId,int year) 
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTotalOrdersByBranchIdCompanyIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                List<ChartModel> models = new List<ChartModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ChartModel
                    {
                        Total = Convert.ToInt32(reader["TotalOrder"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect total Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ChartModel> GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTotalOrdersByCompanyIdAndYear";
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
                        Total = Convert.ToInt32(reader["TotalOrder"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect total Orders by company id and year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ChartModel> GetTotalOrdersByYear(int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetTotalOrdersByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                List<ChartModel> models = new List<ChartModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ChartModel
                    {
                        Total = Convert.ToInt32(reader["TotalOrder"]),
                        MonthName = reader["MonthName"].ToString()
                    };
                    models.Add(model);
                }
                reader.Close();
                return models;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect total Orders by year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int Save(Order order)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {

                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveNewOrder";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", order.ClientId);
                CommandObj.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                CommandObj.Parameters.AddWithValue("@UserId", order.UserId);
                CommandObj.Parameters.AddWithValue("@OrderSlipNo", order.OrderSlipNo);
                CommandObj.Parameters.AddWithValue("@OrderRefNo", order.OrederRef);
                CommandObj.Parameters.AddWithValue("@BranchId", order.BranchId);
                CommandObj.Parameters.AddWithValue("@Amounts", order.Amounts);
                CommandObj.Parameters.AddWithValue("@Vat", order.Vat);
                CommandObj.Parameters.AddWithValue("@Discount", order.Discount);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", order.SpecialDiscount);
                CommandObj.Parameters.AddWithValue("@CompanyId", order.CompanyId);
                CommandObj.Parameters.Add("@OrderId", SqlDbType.Int);
                CommandObj.Parameters["@OrderId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int orderId = Convert.ToInt32(CommandObj.Parameters["@OrderId"].Value);
                int result = SaveOrderDetails(order.Products, orderId);
                if (result > 0)
                {
                    sqlTransaction.Commit();
                }
                return result;

            }
            catch (SqlException sqlException)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Order due to sql exception", sqlException);

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
        private int SaveOrderDetails(IEnumerable<Product> products, int orderId)
        {
            int i = 0;
            foreach (var item in products)
            {
                CommandObj.CommandText = "UDSP_SaveOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@VatId", item.VatId);
                CommandObj.Parameters.AddWithValue("@ProductDetailsId", item.ProductDetailsId);
                CommandObj.Parameters.AddWithValue("@DiscountId", item.DiscountId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public int UpdateOrder(ViewOrder order)
        {
            try
            {
                CommandObj.CommandText = "spUpdateOrderBySalesPerson";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", order.OrderId);
                CommandObj.Parameters.AddWithValue("@Amount", order.Amounts);
                CommandObj.Parameters.AddWithValue("@Vat", order.Vat);
                CommandObj.Parameters.AddWithValue("@Discount", order.Discount);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", order.SpecialDiscount);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not update discount amount due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not update discount amount", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int AddNewItemToExistingOrder(Product aProduct,int orderId) 
        {
            try
            {

                CommandObj.CommandText = "UDSP_SaveOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                CommandObj.Parameters.AddWithValue("@ProductId", aProduct.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", aProduct.Quantity);
                CommandObj.Parameters.AddWithValue("@VatId", aProduct.VatId);
                CommandObj.Parameters.AddWithValue("@ProductDetailsId", aProduct.ProductDetailsId);
                CommandObj.Parameters.AddWithValue("@DiscountId", aProduct.DiscountId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not add new item to exiting order due to Sql Exception",sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Add New item to existing Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int DeleteProductFromOrderDetails(long orderItemId)
        {
            try
            {

                CommandObj.CommandText = "UDSP_DeleteProductFromOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderItemId", orderItemId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not delete product due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not delete product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int CancelOrder(ViewOrder order)
        {
            try
            {
                CommandObj.CommandText = "spCancelOrder";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", order.OrderId);
                CommandObj.Parameters.AddWithValue("@Reason", order.ResonOfCancel);
                CommandObj.Parameters.AddWithValue("@CancelByUserId", order.CancelByUserId);
                CommandObj.Parameters.AddWithValue("@OrderStatus", order.Status);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not cancel order due to Sql Exception",sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not cancel  Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int ApproveOrderByNsm(ViewOrder aModel)
        {
            try
            {
                CommandObj.CommandText = "spApproveOrderByNsm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", aModel.OrderId);
                CommandObj.Parameters.AddWithValue("@Amount", aModel.Amounts);
                CommandObj.Parameters.AddWithValue("@Vat", aModel.Vat);
                CommandObj.Parameters.AddWithValue("@Discount", aModel.Discount);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", aModel.SpecialDiscount);
                CommandObj.Parameters.AddWithValue("@Status ", aModel.Status);
                CommandObj.Parameters.AddWithValue("@ApprovedByNsmDateTime", aModel.ApprovedByNsmDateTime);
                CommandObj.Parameters.AddWithValue("@NsmUserId", aModel.NsmUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Update Order Status due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Update Order Status", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ApproveOrderByAdmin(ViewOrder order)
        {
            try
            {
                CommandObj.CommandText = "spApproveOrderByAdmin";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", order.OrderId);
                CommandObj.Parameters.AddWithValue("@SpecialDiscount", order.SpecialDiscount);
                CommandObj.Parameters.AddWithValue("@Discount", order.Discount);
                CommandObj.Parameters.AddWithValue("@Status ", order.Status);
                CommandObj.Parameters.AddWithValue("@Amounts", order.Amounts);
                CommandObj.Parameters.AddWithValue("@AdminUserId", order.AdminUserId);
                CommandObj.Parameters.AddWithValue("@Vat", order.Vat);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not approve order by Admin due to Sql Exception",sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve order by Admin", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int UpdateOrderDetails(IEnumerable<OrderItem> orderItems)
        {

            try
            {
                int i = 0;
                foreach (var item in orderItems)
                {
                    CommandObj.Parameters.Clear();
                    CommandObj.CommandText = "UDSP_UpdateOrderDetails";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.AddWithValue("@OrderItemId", item.OrderItemId);
                    CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                    CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    ConnectionObj.Open();
                    CommandObj.ExecuteNonQuery();
                    i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                    ConnectionObj.Close();
                }
                return i;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not update Order items due to sql exception", sqlException);
            }
            catch (Exception e)
            {
                throw new Exception("Could not update Order items", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<Order> GetOrdersByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "spGetOrdersByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Order> orders = new List<Order>();
                while (reader.Read())
                {

                    orders.Add(new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchId = Convert.ToInt32(reader["Branchid"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        ApprovedByAdminDateTime = Convert.ToDateTime(reader["ApprovedByAdminDateTime"]),
                        AdminUserId = Convert.ToInt32(reader["AdminUserId"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        DeliveryDateTime = Convert.ToDateTime(reader["DeliveredDateTime"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        OrederRef = reader["OrderRef"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                        
                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public List<Product> GetProductListByOrderId(int orderId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetProductListByOrderId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Product> products = new List<Product>(); 
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                        ProductName = reader["ProductName"].ToString(),
                        DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]),
                        VatId = Convert.ToInt32(reader["VatId"]),
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        Vat = Convert.ToDecimal(reader["Vat"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Order details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect product lsit by order Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewOrder> GetDelayedOrdersToSalesPersonByBranchAndCompanyId(int branchId, int companyId) 
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDelayedOrderListToSalesPersonByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect delayed Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect delayed Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewOrder> GetDelayedOrdersToNsmByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDelayedOrderListToNsmByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect delayed Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect delayed Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewOrder> GetDelayedOrdersToAdminByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDelayedOrderListToAdminByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {
                    var anOrder = new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BranchName = reader["BranchName"].ToString(),
                        ClientName = reader["Name"].ToString(),
                        Status = Convert.ToInt32(reader["OrderStatus"]),
                        ClientEmail = reader["Email"].ToString(),
                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeName = reader["ClientTypeName"].ToString(),
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                            }
                        }

                    };
                    orders.Add(anOrder);
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect delayed Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect delayed Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int UpdateVerificationStatus(int orderId, string verificationNote, int userUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateVerificationStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderId", orderId);
                CommandObj.Parameters.AddWithValue("@Notes", verificationNote);
                CommandObj.Parameters.AddWithValue("@UserId", userUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not update verification  status due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not update verification status", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewVerifiedOrderModel> GetVerifiedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetVerifiedOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewVerifiedOrderModel> orders = new List<ViewVerifiedOrderModel>();
                while (reader.Read())
                {
                    
                    orders.Add(new ViewVerifiedOrderModel
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        OrderSlipNo = reader["OrderSlipNo"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        User = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            EmployeeName = reader["EmployeeName"].ToString()
                        },
                        BranchName = reader["BranchName"].ToString(),
                        Client = new Client
                        {
                            ClientName = reader["Name"].ToString(),
                            CommercialName = reader["CommercialName"].ToString(),
                            Email = reader["Email"].ToString(),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            Address = reader["Address"].ToString(),
                            AlternatePhone = reader["AltPhone"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ClientType = new ClientType
                            {
                                ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                                ClientTypeName = reader["ClientTypeName"].ToString()
                            }
                        },

                        ApprovedByNsmDateTime = Convert.ToDateTime(reader["ApprovedByNsmDateTime"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        SpecialDiscount = Convert.ToDecimal(reader["SpecialDiscount"]),
                        NetAmounts = Convert.ToDecimal(reader["NetAmounts"]),
                        NsmUserId = Convert.ToInt32(reader["NsmUserId"]),
                        CompanyId = companyId,
                        BranchId = branchId,
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        CancelByUserId = Convert.ToInt32(reader["CancelByUserId"]),
                        ResonOfCancel = reader["ReasonOfCancel"].ToString(),
                        CancelDateTime = Convert.ToDateTime(reader["CancelDateTime"]),
                        SysDate = Convert.ToDateTime(reader["SysDateTime"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        VerificationStatus = Convert.ToInt32(reader["VerificationStatus"]),
                        Notes = reader["VerificationNote"].ToString(),
                        VerifiedByUserId = Convert.ToInt32(reader["VerifyedByUserId"]),
                        VerifiedDateTime = Convert.ToDateTime(reader["VerificationDateTime"])
                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect verified Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect verified Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ViewOrder> GetOrder(SearchCriteriaModel searchCriteria)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetAllOrders";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DisplayLength", searchCriteria.DisplayLength);
                CommandObj.Parameters.AddWithValue("@DisplayStart", searchCriteria.DisplayStart);
                CommandObj.Parameters.AddWithValue("@SortCol", searchCriteria.SortColomnIndex);
                CommandObj.Parameters.AddWithValue("@SortDir", searchCriteria.SortDirection);
                CommandObj.Parameters.AddWithValue("@Search", searchCriteria.Search);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewOrder> orders = new List<ViewOrder>();
                while (reader.Read())
                {

                    orders.Add(new ViewOrder
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        OrederRef = reader["OrderRef"].ToString(),
                        StatusDescription = reader["StatusDescription"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Client = new Client
                        {
                            CommercialName = reader["CommercialName"].ToString()
                        }

                    });
                }

                reader.Close();
                return orders;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Orders by Search Creteria", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int SaveSoldProductBarCode(RetailSale retail)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
               
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveSoldProductBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", retail.BarCode);
                CommandObj.Parameters.AddWithValue("@UserId", retail.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", retail.BranchId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
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
            catch (SqlException sqlException)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save status", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

     
    }
}