<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlToolBarConfirm.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarConfirm" %>

<%--<link href="../Styles/Site.css" rel="stylesheet" type="text/css" />--%>
<script language="javascript" type="text/javascript">
    $(function () {
        $("#confirm-dialog-delete").html("<table><tr><td><img src='../images/img_msg_question.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" + 
            "¿Está seguro de eliminar?" + 
            "</td></tr></table>");
        $("#confirm-dialog-delete").dialog({
            autoOpen: false,
            title: 'Confirmación',
            modal: true,
            buttons: {
                "Si": function (e) {
                    $(this).dialog("close");
                    <%=Page.ClientScript.GetPostBackEventReference(btnEliminar, "") %>                                        
                },
                "No": function () {
                    $(this).dialog("close");
                    return false;
                }
            }
        });
        $('#<%=btnEliminar.ClientID%>').click(function (e) {
            e.preventDefault();
            $("#confirm-dialog-delete").dialog("open");
        });  
    });
    function jquery_confirm_delete() {
        $("#confirm-dialog-delete").dialog("open");
        return false;
    }
</script>


<table>  
    <tr>
        <td id="commandSearch" runat="server">
            <asp:Button ID="btnBuscar" runat="server" Text="    Buscar" 
                CssClass="btnSearch" onclick="btnBuscar_Click" />
        </td>        

        <td id="commandNew" runat="server">      
            <asp:Button ID="btnNuevo" runat="server" Text="     Nuevo" CssClass="btnNew"
                onclick="btnNuevo_Click" />
        </td> 
        <td id="commandEdit" runat="server">
            <asp:Button ID="btnEditar" runat="server" Text="    Modificar" CssClass="btnEdit" 
                onclick="btnEditar_Click" />
        </td>
        <td id="commandSave" runat="server">
            <%--<asp:Button ID="btnGrabar" runat="server" Text="    Grabar" CssClass="btnSave"     
                OnClientClick="if (jquery_confirm_save() == false) return false;"         
                onclick="btnGrabar_Click"  />--%>
            <asp:Button ID="btnGrabar" runat="server" Text="    Grabar" CssClass="btnSave" 
                onclick="btnGrabar_Click"  />
        </td>
        <td id="commandDelete" runat="server">
            <asp:Button ID="btnEliminar"  runat="server" Text="   Anular" CssClass="btnDelete" 
            OnClientClick="if (jquery_confirm_delete() == false) return false;" 
            onclick="btnEliminar_Click" />
        </td>
        <td id="commandConfiguration" runat="server">
            <asp:Button ID="btnConfiguration" runat="server" Text="    Configurar" 
                CssClass="btnConfiguration" onclick="btnConfiguration_Click" />
        </td>             
        <td id="commandPrint" runat="server">
            <asp:Button ID="btnImprimir" runat="server" Text="     Imprimir" 
                CssClass="btnPrint" onclick="btnImprimir_Click" />
        </td>
        <td id="commandUndo" runat="server">
             <asp:Button ID="btnCancelar" runat="server" Text="    Cancelar" 
                 CssClass="btnUndo" onclick="btnCancelar_Click" />
        </td> 
        <td id="commandExit" runat="server">
            <asp:Button ID="btnSalir" runat="server" Text="   Cerrar" CssClass="btnExit" 
                onclick="btnSalir_Click" />
        </td>   
    </tr>

</table>
