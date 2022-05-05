<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAnularTramite.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmAnularTramite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#<%=txtMotivo.ClientID%>").focus(function () {
                 $("#tdMsje").attr('hidden', true);
             });
             $("#<%=txtAutorizacion.ClientID %>").focus(function () {

                 $("#tdMsje").attr('hidden', true);
             });
             $("#<%=ddlFuncionarioAnulacion.ClientID %>").focus(function () {

                 $("#tdMsje").attr('hidden', true);
             });

         });
         

        function Validar() {
         
            var bolValida = true;   

            var strMotivo = $.trim($("#<%= txtMotivo.ClientID %>").val());
            var strAutorizacion = $.trim($("#<%= txtAutorizacion.ClientID %>").val());
            var strFuncionario = $.trim($("#<%= ddlFuncionarioAnulacion.ClientID %>").val());

            if (strMotivo.length == 0) {
                $("#<%=txtMotivo.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtMotivo.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strAutorizacion.length == 0) {
                $("#<%=txtAutorizacion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtAutorizacion.ClientID %>").css("border", "solid #888888 1px");
            }

            
            if (strFuncionario == 0) {
                $("#<%=ddlFuncionarioAnulacion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlFuncionarioAnulacion.ClientID %>").css("border", "solid #888888 1px");
            }      

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");

                if (bolValida == false) {
                    $("#lblValidacion").hide();
                   
                }
                else {
                    $("#lblValidacion").show();
                }


            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
                $("#<%= lblValidacion.ClientID %>").html("Debe ingresar los campos requeridos (*)");
                $("#tdMsje").attr('hidden', false);
            }

           
            

            return bolValida;
        }        

    </script>
</head>
<body style="background:#FFF;">


    <form id="form1" runat="server">

        
       
          <table class="" style="margin:0 auto; margin-top:10px;">
            <tr>
                <td id="tdMsje" colspan="2" align="center">
                    <asp:Label ID="lblValidacion" runat="server" 
                    ForeColor="Red"
                    Font-Size="14px"
                    CssClass="">
                    </asp:Label>
                </td>
             
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFuncionarioAnulacion" runat="server" Text="Funcionario"></asp:Label>                    
                </td>
                <td>
                    <asp:DropDownList ID="ddlFuncionarioAnulacion" runat="server" Height="20px" 
                        Width="218px" 
                        onselectedindexchanged="ddlFuncionarioAnulacion_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblVal1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlFuncionarioDoc" runat="server" Visible="False" >                       
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><span id="lblMotivo">Motivo</span></td>
                <td>
                    <asp:TextBox ID="txtMotivo" runat="server" Height="62px" TextMode="MultiLine" 
                        Width="213px" Text="" CssClass="txtLetra" MaxLength="10" 
                        ontextchanged="txtMotivo_TextChanged" />
                    <asp:Label ID="lblVal2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td><span id="lblAutorizacion">Clave Autorización</span></td>
                <td>
                    <asp:TextBox ID="txtAutorizacion" runat="server" Width="159px" 
                        TextMode="Password" autocomplete="off" MaxLength="10" CssClass="txtLetra" 
                        ontextchanged="txtAutorizacion_TextChanged" />
                    <asp:Label ID="lblVal3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">&nbsp;</td>                 
            </tr>            
            <tr>
                <td colspan="2" align="center">
                   <asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="btnGeneral"
                        onclick="Button1_Click" OnClientClick="return Validar();"  />
                </td>
            </tr>
        </table>

     
    </form>
</body>
</html>


