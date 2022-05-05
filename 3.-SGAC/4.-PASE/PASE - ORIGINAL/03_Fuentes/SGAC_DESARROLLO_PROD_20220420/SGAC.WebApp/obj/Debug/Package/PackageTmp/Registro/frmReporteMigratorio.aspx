<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReporteMigratorio.aspx.cs"
    Inherits="SGAC.WebApp.Registro.frmReporteMigratorio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   
    <meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">	
	<meta name="description" content="">
	<meta name="author" content="">

    
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $("#btn_Imprimir").click(btn_Imprimir_onclick);    //Asociando la función "imprimirDiv" al clic del botón para Imprimir Reporte
    });

    function btn_Imprimir_onclick() {
        $("#btn_Imprimir").hide();
        window.print();


        $.ajax({
            type: "POST",
            url: "FrmReporteMigratorioHtml.aspx/ImpresionCorrecta",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d == "OK") {

                    cerrarVentana();
                }
                else {
                    alert(data.d);
                }
                return false;

            },
            failure: function (response) {

                alert(response.d);
            }
        });

        $("#btn_Imprimir").show();
    }

</script>

</head> 
<body >
    <form id="frmReporte" runat="server">

    <asp:HiddenField ID="hdn_Tipo" runat="server" />
    <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir" 
        CausesValidation="false" OnClientClick="return false;"  
        UseSubmitBehavior="false"/>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
    <rsweb:ReportViewer ID="rsReporte" runat="server" Height="870px" 
        Width="100%" ShowToolBar="False" 
        SizeToReportContent="True" DocumentMapCollapsed="True" 
        EnableTheming="False" InteractivityPostBackMode="AlwaysSynchronous" 
        PageCountMode="Actual" PromptAreaCollapsed="True">
    </rsweb:ReportViewer>
    </form>
</body>
</html>
