<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmBusquedaPersona.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRegistroUnicoSearch" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%--<%@ Register Src="~/Accesorios/SharedControls/ctrlBusqueda.ascx" TagPrefix="uc" TagName="ctrlBusqueda" %>--%>
<%@ Register Src="~/Accesorios/SharedControls/ctrlBusquedaAvanzada.ascx" TagPrefix="uc" TagName="ctrlBusqueda" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">    
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    
   
    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
        <table width="100%">
            <tr align="center">
                <td>
                     <asp:UpdatePanel ID="updBusqueda" runat="server">
                        <ContentTemplate>
                            <%--<uc:ctrlBusqueda ID="clrlBusqueda" runat="server"></uc:ctrlBusqueda>--%>
                            <uc:ctrlBusqueda  ID="ctrlBusqueda" runat="server" />
                        </ContentTemplate>
                     </asp:UpdatePanel>
                </td>
            </tr>
        </table>
  

  
</asp:Content>
