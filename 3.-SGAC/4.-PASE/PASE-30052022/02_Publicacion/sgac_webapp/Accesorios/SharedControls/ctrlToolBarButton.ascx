<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlToolBarButton.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton" %>

<%--<link href="../Styles/Site.css" rel="stylesheet" type="text/css" />--%>
<asp:UpdatePanel ID="UpdPopupBusqueda" UpdateMode="Conditional"  runat="server">
        <Triggers>
              <asp:AsyncPostBackTrigger controlid="btnNuevo" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnEditar" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnGrabar" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnImprimir" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnEliminar" eventname="Click" />

              <asp:AsyncPostBackTrigger controlid="btnCancelar" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnConfiguration" eventname="Click" />
              <asp:AsyncPostBackTrigger controlid="btnSalir" eventname="Click" />
         </Triggers> 

        <ContentTemplate>
                    
       <table>  
        <tr>
        <td id="commandNew" runat="server">
            <asp:Button ID="btnNuevo" runat="server" Text="    Nuevo" CssClass="btnNew"
                onclick="btnNuevo_Click" />
        </td> 
        <td id="commandEdit" runat="server">
            <asp:Button ID="btnEditar" runat="server" Text="    Modificar" CssClass="btnEdit" 
                onclick="btnEditar_Click" />
        </td>         
        <td id="commandSave" runat="server">
            <asp:Button ID="btnGrabar" runat="server" Text="    Grabar" CssClass="btnSave"          
                onclick="btnGrabar_Click" onclientclick="return ValidarGrabar()" />
        </td> 
        <td id="commandSearch" runat="server">
            <asp:Button ID="btnBuscar" runat="server" Text="    Buscar" 
                CssClass="btnSearch" onclick="btnBuscar_Click" />
        </td>        
        <%--<td id="commandExportToExcel" runat="server">
            <asp:Button ID="btnExportarExcel" runat="server" Text="    Excel" 
                CssClass="btnExcel" onclick="btnExportarExcel_Click" />
        </td>
        <td id="commandExportToPDF" runat="server">
            <asp:Button ID="btnExportarPDF" runat="server" Text=" PDF" CssClass="btnPDF" 
                onclick="btnExportarPDF_Click" />
        </td> --%>
        <td id="commandPrint" runat="server">
            <asp:Button ID="btnImprimir" runat="server" Text="     Imprimir" 
                CssClass="btnPrint" onclick="btnImprimir_Click" />
        </td>
        <td id="commandDelete" runat="server">
            <asp:Button ID="btnEliminar" runat="server" Text="     Anular" CssClass="btnDelete" 
            onclick="btnEliminar_Click" />
        </td>
         <td id="commandUndo" runat="server">
             <asp:Button ID="btnCancelar" runat="server" Text="    Cancelar" 
                 CssClass="btnUndo" onclick="btnCancelar_Click" />
        </td>
        <td id="commandConfiguration" runat="server">
            <asp:Button ID="btnConfiguration" runat="server" Text="   Configurar" 
                CssClass="btnConfiguration" onclick="btnConfiguration_Click" />
        </td> 
        <td id="commandExit" runat="server">
            <asp:Button ID="btnSalir" runat="server" Text="     Cerrar" CssClass="btnExit" 
                onclick="btnSalir_Click" />
        </td>
    </tr>

</table>

        </ContentTemplate>
</asp:UpdatePanel>
  