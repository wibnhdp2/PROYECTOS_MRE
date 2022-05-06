<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCambiaEstadoMigratorio.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmCambiaEstadoMigratorio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Validar() {

            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%=ddlFuncionario.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=cbo_Motivo.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=cbo_estado.ClientID %>")) == false) bolValida = false;

            
            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            var acmi_iActoMigratorioId = document.getElementById("<%=hID.ClientID %>").value;
            var amhi_IFuncionarioId = document.getElementById("<%=ddlFuncionario.ClientID %>").value;
            var amhi_sMotivoId = document.getElementById("<%=cbo_Motivo.ClientID %>").value;
            var acmi_sEstadoId = document.getElementById("<%=cbo_estado.ClientID %>").value;

            if (bolValida) {
                $.ajax({
                    type: "POST",
                    url: "FrmCambiaEstadoMigratorio.aspx/Cambiar_Estado",
                    data: '{acmi_iActoMigratorioId: ' + acmi_iActoMigratorioId + ', amhi_IFuncionarioId:' + amhi_IFuncionarioId + ',amhi_sMotivoId:' + amhi_sMotivoId + ', acmi_sEstadoId:' + acmi_sEstadoId + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resultado,
                    error: errores

                });
            }

            return bolValida;
        }
        function resultado(msg) {

            if (msg.d == "1") {
                window.parent.close_ModalPopup('MainContent_btn_Consultar_Evento');
            } else {
                alert('Error: No se realizó el cambio de estado a la acto migratorio');
            }

        }
        function errores(msg) {
            alert('Error: ' + msg.responseText);
        }

        function ddlcontrolError(ctrl) {
            var x = ctrl.selectedIndex;
            var bolValida = true;
            if (x < 1) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table class="mTblSecundaria">
            <asp:HiddenField ID="hID" runat="server" />
            <asp:HiddenField ID="hEstadoId" runat="server" Value="0" />
            <tr>
                <td width="150px" height="22px">
                    <asp:Label ID="Label8" runat="server" Text="Funcionario:" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlFuncionario" runat="server" Width="98%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="150px" height="22px">
                    <asp:Label ID="Label2" runat="server" Text="Estado trámite:" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="cbo_estado" runat="server" Width="98%" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="150px" height="22px">
                    <asp:Label ID="Label1" runat="server" Text="Motivo:" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="cbo_Motivo" runat="server" Width="98%">
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr>
                <td colspan="2" align="center"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                   <asp:Button ID="btn_proceso" runat="server" Text="Aceptar" 
                        OnClientClick="return Validar();"/> 
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
