﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Services
{
  public  class DischargeReportModel
    {

        public long DischargeReportId { get; set; }
        public long ParentId { get; set; }
        public long BatteryReceiveId { get; set; }
        public decimal Tv { get; set; }
        public decimal Lv { get; set; }
        public decimal DischargeAmp { get; set; }
        public decimal BackUpTime { get; set; }
        public decimal RecommendedBackUpTime { get; set; }
        public string DischargeReport { get; set; }
        public string DischargeRemarks { get; set; } 
        public int ReportByEmployeeId { get; set; }
        public int EntryByUserId { get; set; }
        public int ForwardToId { get; set; }
        public string ForwardRemarks { get; set; }
        public ForwardDetails ForwardDetails { get; set; }
        public string ReportByEmp { get; set; }
        public DateTime ForwardDatetime { get; set; } 
        public DateTime ReportDatetime { get; set; } 
        public DischargeReportModel()
        {
            ForwardDetails = new ForwardDetails();
        }
    }
}
