using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.DAL
{
    public class ProductionQcGateway:DbGateway,IProductionQcGateway
    {
        public int Add(Product model)
        {
            throw new NotImplementedException();
        }

        public int Update(Product model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Product model)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public int SaveRejectedProduct(RejectedProduct rejectedProduct)
        {
            try
            {
                CommandObj.CommandText = "UDSP_SaveRejectedProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Barcode", rejectedProduct.Barcode);
                CommandObj.Parameters.AddWithValue("@RejectionReasonId", rejectedProduct.RejectionReasonId);
                CommandObj.Parameters.AddWithValue("@Notes", rejectedProduct.Notes);
                CommandObj.Parameters.AddWithValue("@UserId", rejectedProduct.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not save rejected product",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public ICollection<ViewRejectedProduct> GetRejectedProductListBystatus(int status)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetRejectedProductListBystatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Status", status);
                List<ViewRejectedProduct> products=new List<ViewRejectedProduct>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewRejectedProduct
                    {
                        RejectionId = Convert.ToInt64(reader["RejectionId"]),
                        Barcode = reader["Barcode"].ToString(),
                        Notes = reader["Notes"].ToString(),
                        NotesByQc = DBNull.Value.Equals(reader["QcNotes"])?null: reader["QcNotes"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        RejectionReasonId = Convert.ToInt32(reader["RejectionReasonId"]),
                        VerificationStatus =DBNull.Value.Equals(reader["QcPassedOrFailedStatus"])?default(int): Convert.ToInt32(reader["QcPassedOrFailedStatus"]),
                        RejectionReason = new RejectionReason
                        {
                            RejectionReasonId = Convert.ToInt32(reader["RejectionReasonId"]),
                            Reason = reader["Reason"].ToString()
                        }
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect rejected product", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public int AddVerificationNotesToRejectedProduct(ProductVerificationModel verificationModel)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddVerificationNotesToRejectedProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Notes", verificationModel.Notes);
                CommandObj.Parameters.AddWithValue("@RejectionId", verificationModel.RejectionId);
                CommandObj.Parameters.AddWithValue("@VerifiedByUserId", verificationModel.VerifiedByUserId);
                CommandObj.Parameters.AddWithValue("@PassOrFailedStatus", verificationModel.QcPassorFailedStatus);
                CommandObj.Parameters.Add("@RowAffected",SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not add verification notes to rejected product", exception);
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
