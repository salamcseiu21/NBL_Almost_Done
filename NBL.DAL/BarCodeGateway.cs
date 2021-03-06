﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.EntityModels.Products;
using NBL.Models.Logs;
using NBL.Models.Searchs;

namespace NBL.DAL
{
   public class BarCodeGateway: DbGateway, IBarCodeGateway
    {
        public int GenerateBarCode(BarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public int GetMaxBarCodeSlByInfix(string infix,string lineNo) 
        {
            try
            {
                int maxSl = 0;
                CommandObj.CommandText = "UDSP_GetMaxBarCodeSLByInfixAndLineNo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Infix", infix);
                CommandObj.Parameters.AddWithValue("@lineNo", lineNo);
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSl = Convert.ToInt32(reader["MaxSL"]);
                }
                reader.Close();
                return maxSl;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get barcode by prefix", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(BarCodeModel model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_AddBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@PrintByUserId", model.PrintByUserId);
                CommandObj.Parameters.AddWithValue("@BarCode", model.Barcode);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                var rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not add barcode",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(BarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(BarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public BarCodeModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<BarCodeModel> GetAll()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<BarCodeModel> barcodeList = new List<BarCodeModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    barcodeList.Add(new BarCodeModel
                    {
                        Barcode = reader["BarCode"].ToString()
                    });
                }
                reader.Close();
                return barcodeList;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect barcodes", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();

            }
        }

        public ICollection<BarCodeModel> GetAllByProducitonDateCode(string dateCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllPrintableBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DateCode", dateCode);
                List<BarCodeModel> barcodeList = new List<BarCodeModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    barcodeList.Add(new BarCodeModel
                    {
                        Barcode = reader["BarCode"].ToString()
                    });
                }
                reader.Close();
                return barcodeList;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect barcodes", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();

            }
        }

        public ICollection<BarCodeModel> GetBarCodesBySearchCriteria(PrintBarCodeModel model)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetBarCodesBySearchCriteria";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCodeMasterId", model.BarCodeMasterId);
                CommandObj.Parameters.AddWithValue("@LineNumber", model.ProductionLineNumber);
                CommandObj.Parameters.AddWithValue("@From", model.From);
                CommandObj.Parameters.AddWithValue("@To", model.To);
                List<BarCodeModel> barcodes = new List<BarCodeModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    barcodes.Add(new BarCodeModel
                    {
                        Barcode = reader["BarCode"].ToString(),
                        BarCodeModelId = Convert.ToInt64(reader["BarcodeId"]),
                        ProductName = reader["ProductName"].ToString()
                        
                    });
                }
                
                reader.Close();
                return barcodes;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect barcodes", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();

            }
        }

        public int SaveBarCodes(ViewCreateBarCodeModel model)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_AddBarCodeMaster";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", model.ProductId);
                CommandObj.Parameters.AddWithValue("@ProductionDate", model.ProductionDate);
                CommandObj.Parameters.AddWithValue("@LineNumber", model.ProductionLineId.ToString("D2"));
                CommandObj.Parameters.AddWithValue("@Quantity", model.BarCodes.Count);
                CommandObj.Parameters.AddWithValue("@GenerateByUserId", model.GenerateByUserId);
                CommandObj.Parameters.Add("@BarCodeMasterId", SqlDbType.BigInt);
                CommandObj.Parameters["@BarCodeMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                var barCodeMasterId = Convert.ToInt64(CommandObj.Parameters["@BarCodeMasterId"].Value);
                int result = AddBarCode(model, barCodeMasterId);
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


        private int AddBarCode(ViewCreateBarCodeModel model, long barCodeMasterId)
        { 
            int i = 0;
            foreach (var item in model.BarCodes)
            {
                CommandObj.CommandText = "UDSP_AddBarCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@BarCode", item.Barcode);
              
                CommandObj.Parameters.AddWithValue("@BarCodeMasterId", barCodeMasterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public List<PrintBarCodeModel> GetTodaysProductionProductList(DateTime date)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetBarCodeListByDate";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Date", date);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<PrintBarCodeModel> models = new List<PrintBarCodeModel>();
                while (reader.Read())
                {
                    models.Add(new PrintBarCodeModel 
                    {
                        BarCodeMasterId = Convert.ToInt64(reader["BarCodeMasterId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ProductionLineNumber = reader["LineNumber"].ToString(),
                        
                    });
                }

                reader.Close();
                return models;
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

        public ICollection<BarCodeModel> GetAllBarCodeByInfix(string infix)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllBarCodeByInfix";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Infix", infix);
                List<BarCodeModel> barcodeList = new List<BarCodeModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    barcodeList.Add(new BarCodeModel
                    {
                        BatchCode = reader["BatchCode"].ToString(),
                        LineNumber = reader["LineNumber"].ToString()
                    });
                }
                reader.Close();
                return barcodeList;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect barcodes", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();

            }
        }

        public BarCodeModel GetBarcodeByBatchCode(string batchCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetBarcodeByBatchCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@BatchCode", batchCode);
                ConnectionObj.Open();
                BarCodeModel aModel=new BarCodeModel();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    aModel.Barcode = reader["Barcode"].ToString();
                }
                reader.Close();
                return aModel;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not get barcode by batchcode", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();

            }
        }

        public IEnumerable<BarCodeModel> GetBarcodeReportBySearchCriteria(SearchCriteria searchCriteria)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetBarcodeReportBySearchCriteria";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@FromDate", searchCriteria.StartDate);
                CommandObj.Parameters.AddWithValue("@ToDate", searchCriteria.EndDate);
                CommandObj.Parameters.AddWithValue("@ProductId", searchCriteria.ProductId);
                List<BarCodeModel> barcodeList = new List<BarCodeModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    barcodeList.Add(new BarCodeModel
                    {
                        BatchCode = reader["BatchCode"].ToString(),
                        LineNumber = reader["LineNumber"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        ProductionDateTime = Convert.ToDateTime(reader["ProductionDate"])
                        
                    });
                }
                reader.Close();
                return barcodeList;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect barcodes", exception.InnerException);
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
