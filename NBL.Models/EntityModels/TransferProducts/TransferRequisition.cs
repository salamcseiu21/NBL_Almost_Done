using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.TransferProducts
{
    public  class TransferRequisition
    {

        public long TransferRequisitionId { get; set; }
        public string TransferRequisitionRef { get; set; }
        public DateTime TransferRequisitionDate { get; set; }
        public int RequisitionByUserId { get; set; }
        public int RequisitionByBranchId { get; set; }
        public int RequisitionToBranchId { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public char Cancel { get; set; }
        public char EntryStatus { get; set; }
        public int ApproveByUserId { get; set; }
        public DateTime ApproveDateTime { get; set; }
        public DateTime SysDateTime { get; set; }
        public List<Product> Products { get; set; }
    }
}
