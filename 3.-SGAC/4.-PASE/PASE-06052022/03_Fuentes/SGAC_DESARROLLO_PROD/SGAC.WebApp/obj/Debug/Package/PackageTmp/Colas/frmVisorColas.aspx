<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVisorColas.aspx.cs"
    Inherits="SGAC.WebApp.Colas.frmVisorColas" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script> 
    <link href="../Styles/print.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmReporte" runat="server">
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="dsReport" runat="server" Width="100%" Font-Names="Verdana"
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Height="700px" ShowPrintButton="False" 
            AsyncRendering="False" SizeToReportContent="True" style="margin: 0,auto" >
            <ServerReport ReportServerUrl="" />
            <LocalReport ReportPath="Reportes\Rs\Report2.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
<script src="<%=Page.ResolveUrl("~/Scripts/jquery-ui-1.10.1.reporte.min.js") %>" type="text/javascript"></script>
