<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCuentaCorrienteSaldo.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmCuentaCorrienteSaldo" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

    
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
    
        <style type="text/css">
            .style1
            {
                width: 768px;
            }
        </style>
    
</head>
<body>       
    <form id="form1" runat="server">
    
    <div>
        
        <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdGrvPaginada" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--Cuerpo--%>
                    <asp:HiddenField ID="hdn_CuentaCorrienteId" runat="server" />
                    <asp:HiddenField ID="hdn_TipoMonedaId" runat="server" />
                    <div id='divValidacion' style="visibility:collapse">
                        <asp:Label ID="lblValidacion" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="Debe ingresar el monto." />
                    </div>

                    <div style="float: left; bottom:10px; margin:10px 0px 5px 0px">
                        <asp:Label ID="Label5" runat="server" Style="text-align:left; margin-left:10px;" Text="Ingreso:"
                            Width="90px" Font-Bold="False"></asp:Label>
                        <asp:TextBox ID="txtMonto" runat="server" MaxLength="20" style="margin-left:-10px;" onkeypress="return isNumberKey(event)"
                                     Width="150px"  />

                        
                    </div>

                    

                    <div style="float: left; vertical-align:text-bottom; margin:15px 0px 0px 15px">
                        <asp:Label ID="Label3" runat="server" Style="text-align: left; margin-bottom:10px;" Text="Saldo Actual:"
                            Width="80px" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblSaldoActual" runat="server" Style="text-align: left;" Text="0"
                            Width="50px" Font-Bold="true"></asp:Label>

                         
                    </div>

                    <div style="clear:both; bottom:10px; margin:15px 0px 0px 0px">
                        <asp:Label ID="Label1" runat="server" Style="text-align: center" Text="Observación:"
                            Width="90px" Font-Bold="False"></asp:Label>
                        <asp:TextBox ID="txtObservacion" runat="server" MaxLength="20" onkeypress="return isLetraNumeroDoc(event)"
                                     Width="260px" CssClass="txtLetra"  />

                        <asp:Button ID="btnGuardar" runat="server" CssClass="btnSave" Height="24"
                                    Text="     Guardar" style="margin:0px 0px 0px 10px" 
                            onclick="btnGuardar_Click" OnClientClick="return ValidarMonto();"/>
                    </div>
                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
 <script type="text/javascript">


     function ValidarMonto() {

         var bValida;
         var vMonto = $.trim($('#<%= txtMonto.ClientID %>').val());
         var vObservacion = $.trim($('#<%= txtObservacion.ClientID %>').val());

         if (vMonto != '') {

             bValida = true;
             $("#<%=txtMonto.ClientID %>").css("border", "solid #888888 1px");
         }
         else {
             $("#<%=txtMonto.ClientID %>").css("border", "solid Red 1px");

             bValida = false;
         }

         if (vObservacion != '') {

             bValida = true;
             $("#<%=txtObservacion.ClientID %>").css("border", "solid #888888 1px");
         }
         else {
             $("#<%=txtObservacion.ClientID %>").css("border", "solid Red 1px");

             bValida = false;
         }


         return bValida;

     }

     function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode

         var letra = true;
         if (charCode > 31 && (charCode < 48 || charCode > 57)) {
             letra = false;
         }
         if (charCode == 13) {
             letra = false;
         }
         return letra;
     }

     function isLetraNumeroDoc(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode

         var letra = false;
         if (charCode > 1 && charCode < 4) {
             letra = true;
         }
         if (charCode == 8) {
             letra = true;
         }
         if (charCode == 32) {
             letra = true;
         }
         if (charCode >= 58 && charCode <= 59) {
             letra = true;
         }
         if (charCode >= 46 && charCode <= 60) {
             letra = true;
         }
         if (charCode > 63 && charCode < 91) {
             letra = true;
         }
         if (charCode > 94 && charCode < 123) {
             letra = true;
         }
         if (charCode == 130) {
             letra = true;
         }
         if (charCode == 144) {
             letra = true;
         }
         if (charCode > 159 && charCode < 164) {
             letra = true;
         }
         if (charCode == 181) {
             letra = true;
         }
         if (charCode == 214) {
             letra = true;
         }
         if (charCode == 224) {
             letra = true;
         }
         if (charCode == 233) {
             letra = true;
         }

         var letras = "aeiouAEIOU-";
         var tecla = String.fromCharCode(charCode);
         var n = letras.indexOf(tecla);
         if (n > -1) {
             letra = true;
         }

         return letra;
     }
 
    
    </script>
</html>