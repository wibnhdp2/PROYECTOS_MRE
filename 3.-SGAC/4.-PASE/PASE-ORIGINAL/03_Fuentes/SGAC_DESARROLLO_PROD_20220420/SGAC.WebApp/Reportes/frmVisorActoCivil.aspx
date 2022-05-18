<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVisorActoCivil.aspx.cs"
    Inherits="SGAC.WebApp.Reportes.frmVisorActoCivil" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10; IE=11" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <rsweb:ReportViewer ID="rsReporte" runat="server" Width="100%" Height="100%"
            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt">
            <ServerReport ReportServerUrl="" />
            <LocalReport ReportPath="Reportes\rsActoCivilNacimiento.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="Nacimiento" Name="dsActoCivil" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
