<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmReporte.aspx.cs" Inherits="SGAC.WebApp.Almacen.FrmReporte" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function cerrar() {
            window.close();
        }


        $(document).ready(function () {
            $('#dsReport_ctl05:last-child').children().append('<div class=" " style="display:inline-block;font-family:Verdana;font-size:8pt;vertical-align:top;"><table cellpadding="0" cellspacing="0" style="display:inline;"><tbody><tr><td height="28px"></td><td width="4px"></td><td height="28px"><div><div id="dsReport_Custom_Print_Button" style="border: 1px solid transparent; background-color: transparent; cursor: default;"><table title="Print"><tbody><tr><td><input type="image" title="Print" src="Reserved.ReportViewerWebControl.axd?OpType=Resource&Name=Microsoft.Reporting.WebForms.Icons.Print.gif" alt="Refresh" style="border-style:None;height:16px;width:16px;border-width:0px;"></td></tr></tbody></table></div></div></td></tr></tbody></table></div>');

            $('#dsReport_Custom_Print_Button').hover(function () {

                $(this).css({ 'border': '1px solid #336699', 'background-color': '#DDEEF7', 'cursor': 'pointer' });

            }, function () {

                $(this).css({ 'border': '1px solid transparent', 'background-color': 'transparent', 'cursor': 'default' })

            });

            $('#dsReport_Custom_Print_Button').click(function () {
                $('#dsReport_ctl05').hide();
                $('.Cerrar').hide();
                window.print();
                $('#dsReport_ctl05').show();
                $('.Cerrar').show();
            });
        });

    </script>





    <style type="text/css">
        #ReportDiv
        {
            width: 693px;
            height: 817px;
        }
    </style>
</head>
<body >
    <form id="form1" runat="server" submitdisabledcontrols="False" visible="True" 
    
    style="background-color: #FFFFFF; width: 700px; text-align: right; height: 887px;">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

                        <asp:Button ID="btnCerrar"  runat="server" Text="   Cerrar" CssClass="btnExit  Cerrar" OnClientClick="return cerrar()" />

    <div id="ReportDiv" align="center">
        <rsweb:ReportViewer ID="dsReport" runat="server" style="margin 0 auto;" Height="600px" 
        Width="100%"
         DocumentMapWidth="100%" BorderStyle="None"  PromptAreaCollapsed="True"
                    ShowPrintButton="false" PageCountMode="Actual" 
            SizeToReportContent="True">
    </rsweb:ReportViewer>
    </div>
     
    </form>
</body>
</html>
