<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmAutoadhesivoConsular.aspx.cs" Inherits="SGAC.WebApp.Consulta.FrmAutoadhesivoConsular" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlBusquedaAutoadhesivo.ascx" TagPrefix="uc"
    TagName="ctrlBusquedaAutoadhesivo" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style1
        {
            height: 19px;
        }
    </style>
    <script type="text/javascript">
        function abrirVentana(url, title, height, width, evento) {
            VentanaModal = window.open(url, title, 'scrollbars=1,resizable=1,width=' + width + ',height=' + height + ',left=100,top=100');

        }
    </script>
   
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div>
            <asp:HiddenField ID="HFGUID" runat="server" />
            <table width="100%">
                <tr align="center">
                    <td class="style1">
                        <uc:ctrlBusquedaAutoadhesivo ID="ctrlBusquedaAutoadhesivo" runat="server"></uc:ctrlBusquedaAutoadhesivo>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
