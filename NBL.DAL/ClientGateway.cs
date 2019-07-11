using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Locations;
using NBL.Models.EntityModels.Masters;
using NBL.Models.Logs;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class ClientGateway:DbGateway,IClientGateway
    {
       // readonly OrderManager _orderManager = new OrderManager();
        private readonly ICommonGateway _iCommonGateway;

        public ClientGateway(ICommonGateway iCommonGateway)
        {
            _iCommonGateway = iCommonGateway;
        }
        public int Add(Client client)
        {
            try
            {
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_AddNewClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Name", client.ClientName);
                CommandObj.Parameters.AddWithValue("@CommercialName", client.CommercialName);
                CommandObj.Parameters.AddWithValue("@Address", client.Address);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", client.SubSubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@SubSubAccountCode",client.SubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@PostOfficeId", client.PostOfficeId ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", client.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@Phone", client.Phone);
                CommandObj.Parameters.AddWithValue("@AltPhone", client.AlternatePhone ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Gender", client.Gender);
                CommandObj.Parameters.AddWithValue("@ClientImage", client.ClientImage ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ClientSignature", client.ClientSignature ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Email", client.Email ?? (object)DBNull.Value);
                //CommandObj.Parameters.AddWithValue("@Fax", client.Fax);
                //CommandObj.Parameters.AddWithValue("@Website", client.Website);
                CommandObj.Parameters.AddWithValue("@CreditLimit", client.CreditLimit);
                CommandObj.Parameters.AddWithValue("@UserId", client.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", client.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", client.CompanyId);
                CommandObj.Parameters.AddWithValue("@NationalIdNo", client.NationalIdNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@TinNo", client.TinNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@TerritoryId", client.TerritoryId);
                CommandObj.Parameters.AddWithValue("@RegionId", client.RegionId);
                CommandObj.Parameters.AddWithValue("@ContactPersonName", client.ContactPerson ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ContactPersonPhone", client.ContactPersonPhone ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@AssignedEmpId", client.AssignedEmpId ?? (object)DBNull.Value);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not Save", sqlException);
            }
            catch(Exception e)
            {
                Log.WriteErrorLog(e);
                throw new Exception("Clould not Save Client",e);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public List<Client> GetPendingClients()
        {
            try
            {

                List<Client> clients = new List<Client>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAllPendingClients";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        PostOfficeId =DBNull.Value.Equals(reader["PostOfficeId"]) ?(int?)null : Convert.ToInt32(reader["PostOfficeId"]),
                        //PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"])? null:reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        SerialNo = Convert.ToInt32(reader["SlNo"]),
                        AddedBy = reader["AddedBy"].ToString(),
                        SystemDateTime = Convert.ToDateTime(reader["EntryDate"])

                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect pending Clients due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect pending Clients", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        public IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetClientAttachmentsByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                List<ClientAttachment> attachments = new List<ClientAttachment>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    attachments.Add(new ClientAttachment
                    {
                        AttachmentName = reader["AttachmentName"].ToString(),
                        ClientId = clientId,
                        FileExtension = reader["FileExtension"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedByUserId = Convert.ToInt32(reader["UploadedByUserId"]),
                        Id = Convert.ToInt32(reader["AttachmentId"])
                    });
                }
                reader.Close();
                return attachments;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Clients attachment by Client Id due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect Client attachment by Client Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
    
        public IEnumerable<ClientAttachment> GetClientAttachments()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetClientAttachments";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ClientAttachment> attachments=new List<ClientAttachment>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    attachments.Add(new ClientAttachment
                    {
                        AttachmentName = reader["AttachmentName"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        FileExtension = reader["FileExtension"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedByUserId = Convert.ToInt32(reader["UploadedByUserId"]),
                        Id = Convert.ToInt32(reader["AttachmentId"])
                    });
                }
                reader.Close();
                return attachments;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Clients attachment due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect Client attachment", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public Client GetClientByEmailAddress(string email)
        {
            try
            {
                Client client = new Client();
                CommandObj.CommandText = "spGetClientByEmailAddress";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Email",email);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                if(reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone = reader["AltPhone"].ToString();
                    client.Email = reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = reader["Fax"].ToString();
                    client.Website = reader["Website"].ToString();
                    client.PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]);
                    client.ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.ClientImage = reader["ClientImage"].ToString();
                    client.ClientSignature = reader["ClientSignature"].ToString();
                    client.NationalIdNo = reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.TerritoryId = Convert.ToInt32(reader["TerritoryId"]);
                }
                reader.Close();
                return client;
            }
            catch(SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to Get Client By Email", sqlException);
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                throw new Exception("Unable to Get Client By Email",e);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
       
        public int Update(Client client)
        {
            try
            {
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_UpdateClientInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", client.ClientId);
                CommandObj.Parameters.AddWithValue("@Name", client.ClientName);
                CommandObj.Parameters.AddWithValue("@CommercialName", client.CommercialName);
                CommandObj.Parameters.AddWithValue("@Address", client.Address);
                CommandObj.Parameters.AddWithValue("@PostOfficeId", client.PostOfficeId?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", client.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@Phone", client.Phone);
                CommandObj.Parameters.AddWithValue("@AltPhone", client.AlternatePhone ??(object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Gender", client.Gender);
                CommandObj.Parameters.AddWithValue("@ClientImage", client.ClientImage ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ClientSignature", client.ClientSignature ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@Email", client.Email ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@CreditLimit", client.CreditLimit);
                //CommandObj.Parameters.AddWithValue("@Fax", client.Fax);
                //CommandObj.Parameters.AddWithValue("@Website", client.Website);
                CommandObj.Parameters.AddWithValue("@UserId", client.UserId);
                CommandObj.Parameters.AddWithValue("@NationalIdNo", client.NationalIdNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@TinNo", client.TinNo ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@TerritoryId", client.TerritoryId);
                CommandObj.Parameters.AddWithValue("@RegionId", client.RegionId);
                CommandObj.Parameters.AddWithValue("@Active", client.Active);
                CommandObj.Parameters.AddWithValue("@BranchId", client.BranchId);
                CommandObj.Parameters.AddWithValue("@ContactPersonName", client.ContactPerson ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ContactPersonPhone", client.ContactPersonPhone ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@AssignedEmpId", client.AssignedEmpId ?? (object)DBNull.Value);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;

                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to update  client info due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to update  client info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Client GetById(int clientId)
        {

            try
            {
                Client client = new Client();
                CommandObj.CommandText = "UDSP_GetClientById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId",clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.CommercialName = reader["CommercialName"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone = DBNull.Value.Equals(reader["AltPhone"])?null:reader["AltPhone"].ToString();
                    client.Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = DBNull.Value.Equals(reader["Fax"]) ? null : reader["Fax"].ToString();
                    client.Website = DBNull.Value.Equals(reader["Website"]) ? null : reader["Website"].ToString();
                    client.PostOfficeId = DBNull.Value.Equals(reader["Website"]) ?(int?) null : Convert.ToInt32(reader["PostOfficeId"]);
                    client.ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]);
                    client.UpazillaId = DBNull.Value.Equals(reader["UpazillaId"]) ? (int?)null : Convert.ToInt32(reader["UpazillaId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.DistrictId = DBNull.Value.Equals(reader["DistrictId"]) ? (int?)null : Convert.ToInt32(reader["DistrictId"]);
                    client.DivisionId = DBNull.Value.Equals(reader["DivisionId"]) ? (int?)null : Convert.ToInt32(reader["DivisionId"]);
                    client.ClientImage =DBNull.Value.Equals(reader["ClientImage"])? null: reader["ClientImage"].ToString();
                    client.ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString();
                    client.NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.TerritoryId = Convert.ToInt32(reader["TerritoryId"]);
                    client.RegionId = Convert.ToInt32(reader["RegionId"]);
                    client.SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString();
                    client.BranchId = Convert.ToInt32(reader["BranchId"]);
                    client.ClientType = new ClientType
                    {
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                    };

                }
                reader.Close();
                return client;

            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Client Information by client Id due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);

                throw new Exception("Unable to collect Client Information by client Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }

        }
        public ViewClient GetClientDeailsById(int clientId) 
        {

            try
            {
                ViewClient client = new ViewClient();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_GetClientDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.CommercialName = reader["CommercialName"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone =DBNull.Value.Equals(reader["AltPhone"])?null: reader["AltPhone"].ToString();
                    client.Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = DBNull.Value.Equals(reader["Fax"]) ? null : reader["Fax"].ToString();
                    client.Website = DBNull.Value.Equals(reader["Website"]) ? null : reader["Website"].ToString();
                    client.PostOfficeId = DBNull.Value.Equals(reader["PostOfficeId"]) ?(int?) null: Convert.ToInt32(reader["PostOfficeId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.BranchId = Convert.ToInt32(reader["BranchId"]);
                   
                    client.Division = new Division
                    {
                        DivisionId =DBNull.Value.Equals(reader["DivisionId"])? default(int): Convert.ToInt32(reader["DivisionId"]),
                        DivisionName = reader["DivisionName"].ToString()
                    };
                    client.District = new District
                    {
                        DistrictId = DBNull.Value.Equals(reader["DistrictId"]) ? default(int) : Convert.ToInt32(reader["DistrictId"]),
                        DistrictName =DBNull.Value.Equals(reader["DistrictName"])?null: reader["DistrictName"].ToString(),
                        DivisionId = DBNull.Value.Equals(reader["DivisionId"]) ? default(int) : Convert.ToInt32(reader["DivisionId"])
                    };
                    client.Upazilla = new Upazilla
                    {
                        UpazillaId = DBNull.Value.Equals(reader["UpazillaId"]) ? default(int) : Convert.ToInt32(reader["UpazillaId"]),
                        UpazillaName = DBNull.Value.Equals(reader["UpazillaName"]) ? null : reader["UpazillaName"].ToString(),
                        DistrictId = DBNull.Value.Equals(reader["DistrictId"]) ? default(int) : Convert.ToInt32(reader["DistrictId"])
                    };
                    client.PostOffice = new PostOffice
                    {
                        PostOfficeId =DBNull.Value.Equals(reader["PostOfficeId"])? default(int): Convert.ToInt32(reader["PostOfficeId"]),
                        PostOfficeName = DBNull.Value.Equals(reader["PostOfficeName"])?null: reader["PostOfficeName"].ToString(),
                        Code =DBNull.Value.Equals(reader["PostCode"])?null: reader["PostCode"].ToString()
                    };
                    client.ClientType = new ClientType
                    {
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        ClientTypeName = reader["ClientTypeName"].ToString()
                    };
                    client.ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]);
                    client.ClientTypeName = reader["ClientTypeName"].ToString();
                    client.SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString();
                    client.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                    client.ClientImage =DBNull.Value.Equals(reader["ClientImage"])? null: reader["ClientImage"].ToString();
                    client.ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString();
                    client.NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.BranchName = reader["CBranchName"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.ClientType = _iCommonGateway.GetAllClientType().ToList()
                        .Find(n => n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"]));
                    client.Outstanding = DBNull.Value.Equals(reader["Outstanding"])? (decimal?) null: Convert.ToDecimal(reader["Outstanding"]);
                   

                }
                reader.Close();
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                client.ClientAttachments = GetClientAttachmentsByClientId(clientId).ToList();
                return client;

            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Client Information due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect Client Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }

        }

        public IEnumerable<ViewClient> GetAllClientDetails()
        {
            try
            {
                List<ViewClient> clients = new List<ViewClient>();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "spGetAllClientDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName =DBNull.Value.Equals(reader["CommercialName"])?null: reader["CommercialName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Fax = DBNull.Value.Equals(reader["Fax"]) ? null : reader["Fax"].ToString(),
                        Website = DBNull.Value.Equals(reader["Website"]) ? null : reader["Website"].ToString(),
                        PostOfficeId = DBNull.Value.Equals(reader["Website"]) ? default(int) : Convert.ToInt32(reader["PostOfficeId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        UpazillaId = DBNull.Value.Equals(reader["UpazillaId"])?default(int): Convert.ToInt32(reader["UpazillaId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        DistrictId = DBNull.Value.Equals(reader["DistrictId"]) ? default(int) : Convert.ToInt32(reader["DistrictId"]),
                        DivisionId = DBNull.Value.Equals(reader["DivisionId"]) ? default(int) : Convert.ToInt32(reader["DivisionId"]),
                        ClientType = new ClientType
                        {
                            ClientTypeName = reader["ClientTypeName"].ToString(),
                        },
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        PostCode = DBNull.Value.Equals(reader["PostCode"])?null: reader["PostCode"].ToString(),
                        PostOfficeName = DBNull.Value.Equals(reader["PostOfficeName"]) ? null : reader["PostOfficeName"].ToString(),
                        UpazillaName = DBNull.Value.Equals(reader["UpazillaName"]) ? null : reader["UpazillaName"].ToString(),
                        DistrictName = DBNull.Value.Equals(reader["DistrictName"]) ? null : reader["DistrictName"].ToString(),
                        DivisionName = DBNull.Value.Equals(reader["DivisionName"]) ? null : reader["DivisionName"].ToString(),
                        SubSubSubAccountCode =DBNull.Value.Equals(reader["SubSubSubAccountCode"])?null: reader["SubSubSubAccountCode"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                        ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                        ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                        NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null : reader["NationalIdNo"].ToString(),
                        Active = reader["Active"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        BranchName = DBNull.Value.Equals(reader["NBranchName"])?null:reader["NBranchName"].ToString(),
                        TotalOrder = Convert.ToInt32(reader["TotalOrder"])
                        
                    });

                }
                reader.Close();
                return clients;

            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Client Information due to Sql Exception", sqlException);
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                throw new Exception("Unable to collect Client Information", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxSerialNoOfClientByAccountPrefix(string acountPrefix)
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "spGetMaxSerialNoOfClientByAccountPrefix";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@Prefix", acountPrefix);
               
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSlno = Convert.ToInt32(reader["MaxClientNo"]);
                }
                reader.Close();
                return maxSlno;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Colud not get  Client max serial", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<object> GetClientByBranchIdAndSearchTerm(int branchId, string searchTerm)
        {
            try
            {
                List<object> clients = new List<object>();
                CommandObj.Parameters.Clear();
                //CommandObj.CommandText = "spGetAllClientDetailsByBranchId";
                CommandObj.CommandText = "UDSP_GetClientByBranchIdAndSearchTerm";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@SearchTerm", searchTerm);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new 
                    {
                        label = reader["Name"].ToString(),
                        val= Convert.ToInt32(reader["ClientId"])
                    });

                }
                reader.Close();
                return clients;

            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                throw new Exception("Unable to collect Client Information for auto complete by Branch and Serach term", e);

            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId)
        {
            try
            {
                List<ViewClient> clients = new List<ViewClient>();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "spGetAllClientDetailsByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {


                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = DBNull.Value.Equals(reader["CommercialName"])?null: reader["CommercialName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null :reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Fax = DBNull.Value.Equals(reader["Fax"]) ? null : reader["Fax"].ToString(),
                        Website = DBNull.Value.Equals(reader["Website"]) ? null : reader["Website"].ToString(),
                        PostOfficeId = DBNull.Value.Equals(reader["PostOfficeId"]) ? default(int) : Convert.ToInt32(reader["PostOfficeId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        UpazillaId = DBNull.Value.Equals(reader["UpazillaId"]) ? default(int) : Convert.ToInt32(reader["UpazillaId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        DistrictId = DBNull.Value.Equals(reader["DistrictId"]) ? default(int) : Convert.ToInt32(reader["DistrictId"]),
                        DivisionId = DBNull.Value.Equals(reader["DivisionId"]) ? default(int) : Convert.ToInt32(reader["DivisionId"]),
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        PostCode = DBNull.Value.Equals(reader["PostCode"]) ? null: reader["PostCode"].ToString(),
                        PostOfficeName = DBNull.Value.Equals(reader["PostOfficeName"]) ? null : reader["PostOfficeName"].ToString(),
                        UpazillaName = DBNull.Value.Equals(reader["UpazillaName"]) ? null : reader["UpazillaName"].ToString(),
                        DistrictName = DBNull.Value.Equals(reader["DistrictName"]) ? null : reader["DistrictName"].ToString(),
                        DivisionName = DBNull.Value.Equals(reader["DivisionName"]) ? null : reader["DivisionName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                        ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                        ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                        NationalIdNo = DBNull.Value.Equals(reader["NationalIdNo"]) ? null: reader["NationalIdNo"].ToString(),
                        Active = reader["Active"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"])

                    });

                }
                reader.Close();
                return clients;

            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                throw new Exception("Unable to collect Client Information", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewClient> GetClientByBranchId(int branchId)
        {
            try
            {

                List<ViewClient> clients = new List<ViewClient>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetClientByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {

                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        ClientImage =DBNull.Value.Equals(reader["ClientImage"])?null: reader["ClientImage"].ToString(),
                        ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                        PostOfficeId = DBNull.Value.Equals(reader["PostOfficeId"])? default(int): Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"])?null: reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        Active = reader["Active"].ToString(),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        TotalOrder = Convert.ToInt32(reader["TotalOrder"]),
                        ClientType = _iCommonGateway.GetAllClientType().ToList().Find(n =>
                            n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"])),
                        AssignedEmpId = DBNull.Value.Equals(reader["AssignedEmpId"])?default(int):Convert.ToInt32(reader["AssignedEmpId"]),
                        AssignedEmpName = DBNull.Value.Equals(reader["EmployeeName"]) ? null : reader["EmployeeName"].ToString()
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Clients by Branch Id due to sql exception", sqlException);
            }
            catch (Exception exception)

            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect Clients by Branch Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        public int ApproveClient(Client aClient, ViewUser anUser)
        {
            try
            {
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                CommandObj.CommandText = "spApproveNewClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", aClient.ClientId);
                CommandObj.Parameters.AddWithValue("@ApprovedByUserId", anUser.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not approve new client due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not approve new client", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode)
        {
            try
            {
                CommandObj.Dispose();
                decimal outstangingBalance=0;
                CommandObj.CommandText = "UDSP_ClientOustandingBalanceBySubSubSubAccountCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SubSubSubCode", subsubsubAccountCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    outstangingBalance =DBNull.Value.Equals(reader["Outstanding"])?default(decimal): Convert.ToDecimal(reader["Outstanding"]);
                }
                reader.Close();
                return outstangingBalance;
            }
            
            catch (SqlException sqlException)
            {
                 Log.WriteErrorLog(sqlException);
                throw new Exception("Could not collect outstanding balance of client by Account code due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect outstanding balance of client by Account code", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UploadClientDocument(ClientAttachment clientAttachment)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UploadClientDocument";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientAttachment.ClientId);
                CommandObj.Parameters.AddWithValue("@AttachmentName", clientAttachment.AttachmentName);
                CommandObj.Parameters.AddWithValue("@UploadedByUserId", clientAttachment.UploadedByUserId);
                CommandObj.Parameters.AddWithValue("@FileExtension", clientAttachment.FileExtension);
                CommandObj.Parameters.AddWithValue("@FilePath", clientAttachment.FilePath);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not upload client document due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not upload client document", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewClientSummaryModel> GetClientSummary()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetClientSummary";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                List<ViewClientSummaryModel> summary=new List<ViewClientSummaryModel>();
                while (reader.Read())
                {
                    summary.Add(new ViewClientSummaryModel
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        Debit = Convert.ToDecimal(reader["DebitAmount"]),
                        Credit = Convert.ToDecimal(reader["CreditAmount"]),
                        Outstanding = Convert.ToDecimal(reader["OutStanding"]),
                        TotalOrder = Convert.ToInt32(reader["TotalOrder"])
                    });
                }

                return summary;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Could not upload client summary due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not upload client summary", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Delete(Client model)
        {
            throw new NotImplementedException();
        }
        public ICollection<Client> GetAll()
        {
            try
            {

                List<Client> clients = new List<Client>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAllClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = DBNull.Value.Equals(reader["CommercialName"]) ? null : reader["CommercialName"].ToString(),
                        ClientImage = DBNull.Value.Equals(reader["ClientImage"]) ? null : reader["ClientImage"].ToString(),
                        ClientSignature = DBNull.Value.Equals(reader["ClientSignature"]) ? null : reader["ClientSignature"].ToString(),
                        PostOfficeId = DBNull.Value.Equals(reader["PostOfficeId"])? (int?) null: Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = DBNull.Value.Equals(reader["Phone"]) ? null : reader["Phone"].ToString(),
                        AlternatePhone = DBNull.Value.Equals(reader["AltPhone"]) ? null : reader["AltPhone"].ToString(),
                        Email = DBNull.Value.Equals(reader["Email"]) ? null : reader["Email"].ToString(),
                        Address = DBNull.Value.Equals(reader["Address"]) ? null : reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = DBNull.Value.Equals(reader["SubSubSubAccountCode"]) ? null : reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        Active = reader["Active"].ToString(),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        ClientType = _iCommonGateway.GetAllClientType().ToList()
                            .Find(n => n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"])),
                        Branch = new Branch
                        {
                            BranchName =DBNull.Value.Equals(reader["NBranchName"])?null: reader["NBranchName"].ToString(),
                            BranchAddress = DBNull.Value.Equals(reader["NBranchAddress"]) ? null : reader["NBranchAddress"].ToString(),
                            Title = DBNull.Value.Equals(reader["Title"]) ? null : reader["Title"].ToString(),
                            BranchId = Convert.ToInt32(reader["BranchId"])
                        }
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                Log.WriteErrorLog(sqlException);
                throw new Exception("Unable to collect Clients due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Unable to collect Clients", exception);
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