using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.Logs;

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
        public int Add(WarrantyPolicy model)
        {

            throw new NotImplementedException();
            
        }

        public int Update(WarrantyPolicy model)
        {
            throw new NotImplementedException();
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
