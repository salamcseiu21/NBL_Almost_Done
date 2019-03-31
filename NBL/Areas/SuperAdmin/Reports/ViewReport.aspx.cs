using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;
using NBL.Models;
using NBL.Models.EntityModels.Orders;

namespace NBL.Areas.SuperAdmin.Reports
{
    public partial class ViewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Request.QueryString["name"];
                string reportName = Request.QueryString["id"] + ".rdlc";
                //var p= Request.QueryString["id"];
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath(reportName);
                if(name == "o")
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    var orderDetails = (List<Order>)Session["DS"];
                    ReportDataSource datasource = new ReportDataSource("OrderDataset", (from customer in orderDetails.ToList() select customer));

                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                }
              
            }
        }
    }
}