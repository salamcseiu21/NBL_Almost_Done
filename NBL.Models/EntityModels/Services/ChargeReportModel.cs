using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Services
{
   public class ChargeReportModel
    {
        public long ChargeReportId { get; set; }
        public long ParentId { get; set; }
        public long BatteryReceiveId { get; set; } 
        public decimal SpGrCellOne { get; set; }
        public decimal SpGrCellTwo { get; set; }
        public decimal SpGrCellThree { get; set; }
        public decimal SpGrCellFour { get; set; }
        public decimal SpGrCellFive { get; set; }
        public decimal SpGrCellSix { get; set; }
        public int CellOneConditionId { get; set; }
        public int CellTwoConditionId { get; set; }
        public int CellThreeConditionId { get; set; }
        public int CellFourConditionId { get; set; }
        public int CellFiveConditionId { get; set; }
        public int CellSixConditionId { get; set; }
        public decimal OpenVoltage { get; set; }
        public decimal LoadVoltage { get; set; }
        public string CellRemarks { get; set; }
        public string SpGrCellRemarks { get; set; }
        public int ReportByEmployeeId { get; set; }
        public int EntryByUserId { get; set; }
        public int ForwardToId { get; set; }
        public string ForwardRemarks { get; set; }
        public string Report { get; set; }  
        public ForwardDetails ForwardDetails { get; set; }

        public ChargeReportModel()
        {
            ForwardDetails=new ForwardDetails();
        }
    }
}
