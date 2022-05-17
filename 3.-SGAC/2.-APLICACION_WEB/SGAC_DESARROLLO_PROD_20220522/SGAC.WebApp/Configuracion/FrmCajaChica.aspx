<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCajaChica.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmCajaChica" %>

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

                    <div id='divValidacion' style="visibility:collapse">
                        <asp:Label ID="lblValidacion" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="Debe ingresar el monto." />
                    </div>

                    <div style="float: left; bottom:10px; margin:10px 0px 0px 0px">
                        <asp:Label ID="Label5" runat="server" Style="text-align: center" Text="Ingreso:"
                            Width="90px" Font-Bold="False"></asp:Label>
                        <asp:TextBox ID="txtMonto" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"
                                     Width="100px"  />

                        <asp:Button ID="btnGuardar" runat="server" CssClass="btnSave" Height="24"
                                    Text="     Guardar" style="margin:0px 0px 0px 10px"
                            onclick="btnGuardar_Click" OnClientClick="return ValidarMonto();"/>
                    </div>

                    <div style="float: right; vertical-align:text-bottom; margin:15px 0px 0px 0px">
                        <asp:Label ID="Label3" runat="server" Style="text-align: center" Text="Saldo Actual:"
                            Width="100px" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblSaldoActual" runat="server" Style="text-align: center" Text="2,000 $"
                            Width="100px" Font-Bold="true"></asp:Label>

                         
                    </div>


                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
 <script type="text/javascript">


     function ValidarMonto() {

         var vMonto = $.trim($('#<%= txtMonto.ClientID %>').val());

         if (vMonto != '') {
            
             return true;
         }
         else {
             $("#<%=txtMonto.ClientID %>").css("border", "solid Red 1px");
             $('#divValidacion').prop('visibility', 'visible');
             
             return false;
         }
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


 
    
    </script>
</html>
