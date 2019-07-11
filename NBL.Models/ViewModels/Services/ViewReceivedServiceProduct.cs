using System;
namespace NBL.Models.ViewModels.Services
{
    public class ViewReceivedServiceProduct
    {


        public long ReceiveId { get; set; }
        public DateTime ReceiveDatetime { get; set; }
        public string ProductName { get; set; }
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
        public string CellOneCondition { get; set; }
        public string CellTwoCondition { get; set; }
        public string CellThreeCondition { get; set; }
        public string CellFourCondition { get; set; }
        public string CellFiveCondition { get; set; }
        public string CellSixCondition { get; set; }
        public decimal OpenVoltage { get; set; }
        public decimal LoadVoltage { get; set; }
        public string CellRemarks { get; set; }
        public string CoverStatus { get; set; }
        public string ContainerStatus { get; set; }
        public string PostStatus { get; set; }
        public string ServicingStatus { get; set; }
        public string AppUsedFor { get; set; }
        public string AppCapacity { get; set; }
        public string ChargingSystem { get; set; }
        public string OtherInformationRemarks { get; set; }
        public string ReceiveReport { get; set; }
        public DateTime ServiceBatteryDeliveryDate { get; set; }
        public string ServiceBatteryBarcode { get; set; }
        public DateTime ServiceBatteryReturnDate { get; set; }
        public DateTime RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
        public string ReceiveRemarks { get; set; }
        public string ReportByEmployee { get; set; }
        public string EntryByUser { get; set; }
        public string BranchName { get; set; }  
        public int Status { get; set; }
        public string IsActive { get; set; }
        public DateTime SysDatetime { get; set; }
       

    }
}
