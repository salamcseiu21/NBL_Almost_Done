using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.ViewModels;

namespace NBL.Models.EntityModels.Services
{
   public class WarrantyBatteryModel
    {
        public long ReceiveId { get; set; }
        public DateTime ReceiveDatetime { get; set; }
        public DateTime DelearReceiveDate { get; set; } 
        public string Barcode { get; set; }
        public string ReceiveRef { get; set; }
        public string ReplaceRef { get; set; } 
        public int ReceiveByBranchId { get; set; }   
        public string DelivaryRef { get; set; }
        public string TransactionRef { get; set; }
        public string VerificationRemarks { get; set; }
        [Required]
        public decimal SpGrCellOne { get; set; }
        [Required]
        public decimal SpGrCellTwo { get; set; }
        [Required]
        public decimal SpGrCellThree { get; set; }
        [Required]
        public decimal SpGrCellFour { get; set; }
        [Required]
        public decimal SpGrCellFive { get; set; }
        [Required]
        public decimal SpGrCellSix { get; set; }
        public decimal SpGrCellValueDifference { get; set; }
        [Required]
        public int CellOneConditionId { get; set; }
        [Required]
        public int CellTwoConditionId { get; set; }
        [Required]
        public int CellThreeConditionId { get; set; }
        [Required]
        public int CellFourConditionId { get; set; }
        [Required]
        public int CellFiveConditionId { get; set; }
        [Required]
        public int CellSixConditionId { get; set; }
        [Required]
        public decimal OpenVoltage { get; set; }
        public decimal LoadVoltage { get; set; }
        public string VoltageRemarks { get; set; } 
        public string SpGrCellRemarks { get; set; }
        public string PrimaryTestResult { get; set; } 
        public int IsPassPrimaryTest { get; set; }
        [Required]
        public int CoverStatusId { get; set; }
        [Required]
        public int ContainerStatusId { get; set; }
        [Required]
        public int PostStatusId { get; set; }
        [Required]
        public int ServicingStatusId { get; set; }
        public string AppUsedFor { get; set; }
        public string AppCapacity{ get; set; }
        [Required]
        public int ChargingSystemId { get; set; }
        public string OtherInformationRemarks { get; set; }
        public string ReceiveReport { get; set; }
     
        public DateTime? ServiceBatteryDeliveryDate { get; set; }
        public string ServiceBatteryBarcode { get; set; }
        public DateTime? ServiceBatteryReturnDate { get; set; }
        public DateTime? RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
        public string ReceiveRemarks { get; set; }

        public int ReportByEmployeeId { get; set; }
        public int EntryByUserId { get; set; }
        public int Status { get; set; }
        public string IsActive { get; set; }
        public int ForwardToId { get; set; }
        [Required]
        public string ForwardRemarks { get; set; }     
        public int DistributionPointId { get; set; }     
        public string DistributionPoint { get; set; }
        public DateTime ForwardDatetime { get; set; } 
        public DateTime SysDatetime { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string IsManualEntry { get; set; }
        public string HasWarranty { get; set; }
        public int IsSoldInGracePeriod { get; set; }
        public int IsInWarrantyPeriod { get; set; }
        public int ServiceDuration { set; get; }
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
