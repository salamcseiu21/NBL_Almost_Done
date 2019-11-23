using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Products;

namespace NBL.Models.ViewModels.Services
{
    public class ViewReceivedServiceProduct
    {


        public long ReceiveId { get; set; }
        public DateTime ReceiveDatetime { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string Barcode { get; set; }
        public string ReceiveRef { get; set; }
        public string DelivaryRef { get; set; }
        public string TransactionRef { get; set; }
        public string VerificationRemarks { get; set; }
        public decimal SpGrCellOne { get; set; }
        public decimal SpGrCellTwo { get; set; }
        public decimal SpGrCellThree { get; set; }
        public decimal SpGrCellFour { get; set; }
        public decimal SpGrCellFive { get; set; }
        public decimal SpGrCellSix { get; set; }
        public decimal SpGrCellValueDifference { get; set; }    
        public string SpGrRemarks { get; set; } 
        public string PrimaryTestResult { get; set; }  
        public int CellOneConditionId { get; set; }
        public int CellTwoConditionId { get; set; }
        public int CellThreeConditionId { get; set; }
        public int CellFourConditionId { get; set; }
        public int CellFiveConditionId { get; set; }
        public int CellSixConditionId { get; set; }

        public decimal OpenVoltage { get; set; }
        public decimal LoadVoltage { get; set; }
        public string VoltageRemarks { get; set; }
        public string CoverStatus { get; set; }
        public string ContainerStatus { get; set; }
        public string PostStatus { get; set; }
        public string ServicingStatus { get; set; }
        public string AppUsedFor { get; set; }
        public string AppCapacity { get; set; }
        public string ChargingSystem { get; set; }
        public string OtherInformationRemarks { get; set; }
        public string ReceiveReport { get; set; }
        public DateTime? ServiceBatteryDeliveryDate { get; set; }
        public string ServiceBatteryBarcode { get; set; }
        public DateTime? ServiceBatteryReturnDate { get; set; }
        public decimal RecBackupTime { get; set; } 


        public int CoverStatusId { get; set; }
        public int ContainerStatusId { get; set; }
        public int PostStatusId { get; set; }
        public int ServicingStatusId { get; set; }
        public int ChargingSystemId { get; set; }
      
        public DateTime? RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
        public string ReceiveRemarks { get; set; }
        public string DischargeReport { get; set; }
        public string ChargerReport { get; set; }
        public string HasWarranty { get; set; } 
        public string ClientInfo { get; set; }
        public int ClientId { get; set; }   
        public ViewClient Client { get; set; }   
        public string ReportByEmployee { get; set; }
        public string EntryByUser { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
        public string IsActive { get; set; }
        public DateTime SysDatetime { get; set; }
        public string ForwardedTo { get; set; }
        public int ForwardedToId { get; set; }
        public int ReportByEmployeeId { get; set; }
        public int EntryByUserId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int ProductId { get; set; }

        public ViewProductHistory ProductHistory { get; set; }
        public ChargeReportModel ChargeReportModel { get; set; }
        public DischargeReportModel DischargeReportModel { get; set; }
        public List<ForwardToModel> ForwardToModels { set; get; }
        public DateTime ForwardDatetime { get; set; }
        public string ReceiveByBranch { get; set; } 
        public int ReceiveByBranchId { get; set; } 
        public ViewReceivedServiceProduct()
        {
            ForwardToModels=new List<ForwardToModel>();
            ChargeReportModel=new ChargeReportModel();
            DischargeReportModel=new DischargeReportModel();
            Client=new ViewClient();
        }
    }
}
