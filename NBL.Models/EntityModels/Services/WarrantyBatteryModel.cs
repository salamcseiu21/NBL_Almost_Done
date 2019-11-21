using System;
using System.Collections.Generic;
using NBL.Models.ViewModels;

namespace NBL.Models.EntityModels.Services
{
   public class WarrantyBatteryModel
    {
        public long ReceiveId { get; set; }
        public DateTime ReceiveDatetime { get; set; }
        public string Barcode { get; set; }
        public string ReceiveRef { get; set; }
        public string ReplaceRef { get; set; } 
        public int ReceiveByBranchId { get; set; }   
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
        public int CellOneConditionId { get; set; }
        public int CellTwoConditionId { get; set; }
        public int CellThreeConditionId { get; set; }
        public int CellFourConditionId { get; set; }
        public int CellFiveConditionId { get; set; }
        public int CellSixConditionId { get; set; }
        public decimal OpenVoltage { get; set; }
        public decimal LoadVoltage { get; set; }
        public string VoltageRemarks { get; set; } 
        public string SpGrCellRemarks { get; set; }
        public string PrimaryTestResult { get; set; } 
        public int IsPassPrimaryTest { get; set; } 

        public int CoverStatusId { get; set; }
        public int ContainerStatusId { get; set; }
        public int PostStatusId { get; set; }
        public int ServicingStatusId { get; set; }
        public string AppUsedFor { get; set; }
        public string AppCapacity{ get; set; }
        public int ChargingSystemId { get; set; }
        public string OtherInformationRemarks { get; set; }
        public string ReceiveReport { get; set; }
     
        public DateTime ServiceBatteryDeliveryDate { get; set; }
        public string ServiceBatteryBarcode { get; set; }
        public DateTime ServiceBatteryReturnDate { get; set; }
        public DateTime RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
        public string ReceiveRemarks { get; set; }
        public int ReportByEmployeeId { get; set; }
        public int EntryByUserId { get; set; }
        public int Status { get; set; }
        public string IsActive { get; set; }
        public int ForwardToId { get; set; }    
        public string ForwardRemarks { get; set; }     
        public int DistributionPointId { get; set; }     
        public string DistributionPoint { get; set; }
        public DateTime ForwardDatetime { get; set; } 
        public DateTime SysDatetime { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string IsManualEntry { get; set; }
        public string HasWarranty { get; set; } 
        public List<PhysicalConditionModel> PhysicalConditions { set; get; }
        public List<ServicingModel> ServicingModels { set; get; }   
        public List<ChargingStatusModel> ChargingStatus { set; get; }
        public List<ForwardToModel> ForwardToModels { set; get; }
        public List<ViewBranch> DistributionPoints { get; set; }
        public ForwardDetails ForwardDetails { get; set; }

        public WarrantyBatteryModel()
        {
            PhysicalConditions=new List<PhysicalConditionModel>();
            ServicingModels=new List<ServicingModel>();
            ChargingStatus=new List<ChargingStatusModel>();
            ForwardToModels=new List<ForwardToModel>();
            DistributionPoints = new List<ViewBranch>();
            ForwardToModels=new List<ForwardToModel>();
        }
    }
}
