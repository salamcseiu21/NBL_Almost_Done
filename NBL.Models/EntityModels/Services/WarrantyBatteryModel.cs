using System;
using System.Collections.Generic;

namespace NBL.Models.EntityModels.Services
{
   public class WarrantyBatteryModel
    {
        public long ReceiveId { get; set; }
        public DateTime ReceiveDatetime { get; set; }
        public string Barcode { get; set; }
        public string DelivaryRef { get; set; }
        public string TransactionRef { get; set; }
        public string SpGrCellOne { get; set; }
        public string SpGrCellTwo { get; set; }
        public string SpGrCellThree { get; set; }
        public string SpGrCellFour { get; set; }
        public string SpGrCellFive { get; set; }
        public string SpGrCellSix { get; set; }
        public int CellOneConditionId { get; set; }
        public int CellTwoConditionId { get; set; }
        public int CellThreeConditionId { get; set; }
        public int CellFourConditionId { get; set; }
        public int CellFiveConditionId { get; set; }
        public int CellSixConditionId { get; set; }
        public string OpenVoltage { get; set; }
        public string LoadVoltage { get; set; }
        public string CellRemarks { get; set; }

        public int CoverStatusId { get; set; }
        public int ContainerStatusId { get; set; }
        public int PostStatusId { get; set; }
        public int ServicingStatusId { get; set; }
        public int AppUsedForId { get; set; }
        public int AppCapacityId { get; set; }
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
        public DateTime SysDatetime { get; set; }
        public List<PhysicalConditionModel> PhysicalConditions { set; get; }
        public List<ServicingModel> ServicingModels { set; get; }   
        public List<ChargingStatusModel> ChargingStatus { set; get; }   
    }
}
