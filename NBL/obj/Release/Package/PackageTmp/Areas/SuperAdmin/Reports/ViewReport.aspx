<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="NBL.Areas.SuperAdmin.Reports.ViewReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width"/>
    <title>Report</title>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
  
</head>
<body>
   
            <form id="form1" runat="server">
       <div class="panel panel-primary">
       <div class="panel panel-body">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
           <div style="text-align:center">

  
               <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" Width="100%" Height="900" CssClass="text-center" SizeToReportContent="false">
    </rsweb:ReportViewer>
    </div>
       
               </div>
   </div>
    </form>
   
    <script src="../Scripts/bootstrap.js"></script>
</body>
</html>

