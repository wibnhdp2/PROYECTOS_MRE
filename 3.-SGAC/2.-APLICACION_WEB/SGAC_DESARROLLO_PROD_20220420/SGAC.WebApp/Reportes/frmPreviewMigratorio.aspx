<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPreviewMigratorio.aspx.cs" Inherits="SGAC.WebApp.Reportes.frmPreviewMigratorio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div style="width:100%;" align="center">
        <rsweb:ReportViewer ID="dsReport" runat="server" 
            AsyncRendering="False" Font-Names="Verdana" Font-Size="8pt" 
            InteractiveDeviceInfos="(Collection)" SizeToReportContent="True" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
            ShowPrintButton="False">
            <LocalReport ReportPath="Reportes\RSMigratorio\rsPasaporteGeneral.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
<script src="<%=Page.ResolveUrl("~/Scripts/jquery-ui-1.10.1.reporte.min.js") %>" type="text/javascript"></script>
