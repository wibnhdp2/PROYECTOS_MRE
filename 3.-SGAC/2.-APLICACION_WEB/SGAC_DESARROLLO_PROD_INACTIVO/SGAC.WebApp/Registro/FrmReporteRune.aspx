<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmReporteRune.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmReporteRune" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

  
   <script src="../Scripts/Validacion/jquery.min.js" type="text/javascript"></script>
   <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
 <%--   <script src="../Scripts/jquery-ui-1.10.1.reporte.min.js" type="text/javascript"></script>--%>


<style type="text/css">
.btnPrint
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	height:26px;
	width:91px;
	background-image: url('../Images/img_16_print.png');
	background-repeat: no-repeat;
	background-position: 8px 4px;
	cursor:pointer;
}
</style>


</head>
<body>
    <form id="form1" runat="server">
    <input type="button" id="btnPrint" class="btnPrint" value="Imprimir" onclick="Print()" style="display:none" />
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div id="ReportDiv" align="center">
                <rsweb:ReportViewer ID="dsReport"  style="margin 0 auto;"
                    runat="server"  DocumentMapWidth="100%" BorderStyle="None"  PromptAreaCollapsed="True"
                    Height="600px" Width="100%" ShowPrintButton="true" 
                    PageCountMode="Actual">
                 </rsweb:ReportViewer>
        </div>


        
        <script type="text/javascript">
        function Print() {
            var report = document.getElementById("<%=dsReport.ClientID %>");
            var div = report.getElementsByTagName("DIV");
            var reportContents;
            for (var i = 0; i < div.length; i++) {
                if (div[i].id.indexOf("VisibleReportContent") != -1) {
                    reportContents = div[i].innerHTML;
                    break;
                }
            }
            var frame1 = document.createElement('iframe');
            frame1.name = "frame1";
            frame1.style.position = "absolute";
            frame1.style.top = "-1000000px";
            document.body.appendChild(frame1);
            var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
            frameDoc.document.open();
            frameDoc.document.write('<html><head><title>RDLC Report</title>');
            frameDoc.document.write('</head><body style = "font-family:arial;font-size:10pt;">');
            frameDoc.document.write(reportContents);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();
            setTimeout(function () {
                window.frames["frame1"].focus();
                window.frames["frame1"].print();
                document.body.removeChild(frame1);
            }, 500);
        }
    </script>
    </form>
</body>

</html>
