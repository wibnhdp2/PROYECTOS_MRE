<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVisorRDCL.aspx.cs" Inherits="SGAC.WebApp.Colas.WebForm1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10; IE=11" />

    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>

    <script src="<%=Page.ResolveUrl("~/Scripts/jquery-ui-1.10.1.reporte.min.js") %>" type="text/javascript"></script>
</head>

<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div>
          
            <rsweb:ReportViewer ID="dsReport" 
                                runat="server" 
                              Height="431px" 
                            Width="700px" 
                            DocumentMapWidth="100%" BorderStyle="None" SizeToReportContent="True" 
                            ZoomMode="PageWidth" DocumentMapCollapsed="True" 
                            PromptAreaCollapsed="True" ViewStateMode="Enabled">
            </rsweb:ReportViewer>
    

     
    
        </div> 

        </form>

    </body>

</html>
