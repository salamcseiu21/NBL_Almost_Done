using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels.ProductWarranty;
using NBL.Models.ViewModels.Services;

namespace NBL.DAL
{
   public class PolicyGateway:DbGateway,IPolicyGateway
    {


        public int AddProductWarrentPolicy(WarrantyPolicy model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddProductWarrentPolicy";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", model.ProductId);
                CommandObj.Parameters.AddWithValue("@WarrantyFrom", model.WarrantyFrom);
                CommandObj.Parameters.AddWithValue("@WarrantyPeriodInDays", model.WarrantyPeriodInDays);
                CommandObj.Parameters.AddWithValue("@AgeLimitInDealerStock", model.AgeLimitInDealerStock);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@FromBatch", model.FromBatch ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ToBatch", model.ToBatch ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ClientId", model.ClientId?? (object)DBNull.Value);
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
                throw new Exception("Coluld not add product warrenty policy");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewWarrantyPolicy> GetAllWarrantyPolicy()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllWarrantyPolicy";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ICollection<ViewWarrantyPolicy> policies=new List<ViewWarrantyPolicy>();
                while (reader.Read())
                {
                    policies.Add(new ViewWarrantyPolicy
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        AgeLimitInDealerStock = Convert.ToInt32(reader["AgeLimitInDealerStock"]),
                        WarrantyPeriodInDays = Convert.ToInt32(reader["LifeTime"]),
                        Id = Convert.ToInt64(reader["Id"])
                        
                    });
                }
                reader.Close();
                return policies;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Coluld not Collect product warrenty policy");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveTestPolicy(TestPolicyModel model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_SaveTestPolicy";
                CommandObj.CommandType = CommandType.StoredProcedure; 
                CommandObj.Parameters.AddWithValue("@TestCategoryId", model.TestCategoryId);
                CommandObj.Parameters.AddWithValue("@ProductId", model.ProductId);
                CommandObj.Parameters.AddWithValue("@Parameter", model.Parameter);
                CommandObj.Parameters.AddWithValue("@AcceptableValue", model.AcceptableValue);
                CommandObj.Parameters.AddWithValue("@Remarks", model.Remarks);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
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
                throw new Exception("Coluld not add product test policy");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewTestPolicy> GetAllTestPolicy()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllTestPolicy";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ICollection<ViewTestPolicy> policies = new List<ViewTestPolicy>();
                while (reader.Read())
                {
                    policies.Add(new ViewTestPolicy 
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                      ParameterName = reader["Parameters"].ToString(),
                      AcceptableValue = Convert.ToDecimal(reader["AcceptableValue"]),
                      Remarks = reader["Remarks"].ToString(),
                      TestCategory = reader["TestName"].ToString()

                    });
                }
                reader.Close();
                return policies;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Coluld not Collect product test policy");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ViewTestPolicy DischargeTestPolicyByProductIdCategoryIdAndMonth(int productId, int categoryId, int month)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDischargeTestPolicyByProductIdCategoryIdAndMonth";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Month", month);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                CommandObj.Parameters.AddWithValue("@CategoryId", categoryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                ViewTestPolicy policy = new ViewTestPolicy();
                if(reader.Read())
                {
                    policy.AcceptableValue = Convert.ToDecimal(reader["AcceptableValue"]);
                    policy.ProductId = productId;
                }
                reader.Close();
                return policy;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Coluld not Collect product test policy by productid,categoryid,month");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(WarrantyPolicy model)
        {

            throw new NotImplementedException();
            
        }

        public int Update(WarrantyPolicy model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateWarrantyPolicy";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@Id", model.Id);
                CommandObj.Parameters.AddWithValue("@WarrantyPeriod", model.WarrantyPeriodInDays);
                CommandObj.Parameters.AddWithValue("@AgeLimitInDealerStock", model.AgeLimitInDealerStock);
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
                throw new Exception("Coluld not update product warrenty policy");
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Delete(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public WarrantyPolicy GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyPolicy> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
