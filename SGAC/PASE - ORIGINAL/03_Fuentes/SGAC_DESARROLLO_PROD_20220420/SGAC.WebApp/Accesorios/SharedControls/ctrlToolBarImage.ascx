<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlToolBarImage.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarImage" %>

<table>

    <tr>
     
        <td id="commandNew" runat="server">
            <asp:ImageButton ID="imgNuevo" runat="server"
                ImageUrl="~/Images/img_16_add.png" ToolTip="Nuevo" Height="16px"
                onclick="imgNuevo_Click" />
        </td>

        <td id="separator1" runat="server"> 
            |
        </td>

        <td id="commandEdit" runat="server">
            <asp:ImageButton ID="imgEditar" runat="server"
                ImageUrl="~/Images/img_16_edit.png" ToolTip="Editar" Height="16px"
                onclick="imgEditar_Click" />
        </td>

        <td id="separator2" runat="server">
            |
        </td>

        <td id="commandDelete" runat="server">
            <asp:ImageButton ID="imgEliminar" runat="server" 
                ImageUrl="~/Images/img_16_delete.png" ToolTip="Eliminar" Height="16px" 
                onclick="imgEliminar_Click" />
        </td>

        <td id="separator3" runat="server">
            |
        </td>

         <td id="commandSave" runat="server">
            <asp:ImageButton ID="imgGrabar" runat="server" 
                 ImageUrl="~/Images/img_16_save.png" ToolTip="Grabar" Height="16px" 
                 onclick="imgGrabar_Click" />
        </td>

        <td id="separator4" runat="server">
            |
        </td>

         <td id="commandUndo" runat="server">
            <asp:ImageButton ID="imgCancelar" runat="server" 
                 ImageUrl="~/Images/img_16_undo.png" ToolTip="Cancelar" Height="16px" 
                 onclick="imgCancelar_Click" />
        </td>

        <td id="separator5" runat="server">
            |
        </td>

        <td id="commandSearch" runat="server">
            <asp:ImageButton ID="imgBuscar" runat="server" 
                ImageUrl="~/Images/img_16_search.png" ToolTip="Buscar" 
                onclick="imgBuscar_Click" />
        </td> 

        <td id="separator6" runat="server">
            |
        </td> 
             
        <td id="commandExportToExcel" runat="server">
            <asp:ImageButton ID="imgExportarExcel" runat="server" 
                ImageUrl="~/Images/img_16_export_excel.png" ToolTip="Exportar a Excel" 
                Height="16px" onclick="imgExportarExcel_Click" />
        </td>

        <td id="separator7" runat="server">
            |
        </td> 

         <td id="commandExportToPDF" runat="server">
            <asp:ImageButton ID="imgExportarPDF" runat="server" 
                 ImageUrl="~/Images/img_16_pdf.png" ToolTip="Exportar a PDF" 
                 Height="16px" onclick="imgExportarPDF_Click" />
        </td>

        <td id="separator8" runat="server">
            |
        </td>

          <td id="commandPrint" runat="server">
            <asp:ImageButton ID="imgPrint" runat="server" 
                 ImageUrl="~/Images/img_16_print.png" ToolTip="Imprimir" Height="16px" 
                  onclick="imgPrint_Click" />
        </td>

        <td id="separator9" runat="server">
            |
        </td>

        <td id="commandExit" runat="server">
            <asp:ImageButton ID="imgSalir" runat="server" 
                ImageUrl="~/Images/img_16_logout.png" ToolTip="Salir" Height="16px" 
                onclick="imgSalir_Click" />
        </td>

    </tr>

</table>