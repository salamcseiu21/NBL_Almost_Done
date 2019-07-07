﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Services;

namespace NBL.DAL
{
   public class ServiceGateway:DbGateway,IServiceGateway
    {

        public int ReceiveServiceProduct(WarrantyBatteryModel product)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ReceiveServiceProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ReceiveDatetime", product.ReceiveDatetime);
                CommandObj.Parameters.AddWithValue("@BarcodeNo", product.Barcode);
                CommandObj.Parameters.AddWithValue("@DelivaryRef", product.DelivaryRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", product.TransactionRef);
                CommandObj.Parameters.AddWithValue("@SpGrCellOne", product.SpGrCellOne);
                CommandObj.Parameters.AddWithValue("@SpGrCellTwo", product.SpGrCellTwo);
                CommandObj.Parameters.AddWithValue("@SpGrCellThree", product.SpGrCellThree);
                CommandObj.Parameters.AddWithValue("@SpGrCellFour", product.SpGrCellFour);
                CommandObj.Parameters.AddWithValue("@SpGrCellFive", product.SpGrCellFive);
                CommandObj.Parameters.AddWithValue("@SpGrCellSix", product.SpGrCellSix);
                CommandObj.Parameters.AddWithValue("@CellOneConditionId", product.CellOneConditionId);
                CommandObj.Parameters.AddWithValue("@CellTwoConditionId", product.CellTwoConditionId);
                CommandObj.Parameters.AddWithValue("@CellThreeConditionId", product.CellThreeConditionId);
                CommandObj.Parameters.AddWithValue("@CellFourConditionId", product.CellFourConditionId);
                CommandObj.Parameters.AddWithValue("@CellFiveConditionId", product.CellFiveConditionId);
                CommandObj.Parameters.AddWithValue("@CellSixConditionId", product.CellSixConditionId);
                CommandObj.Parameters.AddWithValue("@OpenVoltage", product.OpenVoltage);
                CommandObj.Parameters.AddWithValue("@LoadVoltage", product.LoadVoltage);
                CommandObj.Parameters.AddWithValue("@CellRemarks", product.CellRemarks);
                CommandObj.Parameters.AddWithValue("@CoverStatusId", product.CoverStatusId);
                CommandObj.Parameters.AddWithValue("@ContainerStatusId", product.ContainerStatusId);
                CommandObj.Parameters.AddWithValue("@PostStatusId", product.PostStatusId);
                CommandObj.Parameters.AddWithValue("@ServicingStatusId", product.ServicingStatusId);
                CommandObj.Parameters.AddWithValue("@ChargingSystemId", product.ChargingSystemId);
                CommandObj.Parameters.AddWithValue("@AppUsedFor", product.AppUsedFor?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@AppCapacity", product.AppCapacity ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@OtherInformationRemarks", product.OtherInformationRemarks ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReceiveReport", product.ReceiveReport ?? (object)DBNull.Value);
                CommandObj.Parameters.AddWithValue("@ReportByEmployeeId", product.ReportByEmployeeId);
                CommandObj.Parameters.AddWithValue("@EntryByUserId", product.EntryByUserId);
                CommandObj.Parameters.AddWithValue("@Status", product.Status);
                CommandObj.Parameters.AddWithValue("@IsActive", "Y");
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
                throw new Exception("Could not Receive service product",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetReceivedServiceProducts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                List<ViewReceivedServiceProduct> products=new List<ViewReceivedServiceProduct>();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewReceivedServiceProduct
                    {
                        ReceiveId = Convert.ToInt64(reader["ReceiveId"]),
                        ReceiveDatetime = Convert.ToDateTime(reader["ReceiveDatetime"]),
                        AppCapacity = reader["AppCapacity"].ToString(),
                        AppUsedFor = reader["AppUsedFor"].ToString(),
                        Barcode = reader["BarcodeNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        DelivaryRef = reader["DelivaryRef"].ToString(),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        ReceiveRef =DBNull.Value.Equals(reader["ReceiveRef"])? null:reader["ReceiveRef"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw new Exception("Could not collect Receive service product", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int Add(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public WarrantyBatteryModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyBatteryModel> GetAll()
        {
            throw new NotImplementedException();
        }

       
    }
}
