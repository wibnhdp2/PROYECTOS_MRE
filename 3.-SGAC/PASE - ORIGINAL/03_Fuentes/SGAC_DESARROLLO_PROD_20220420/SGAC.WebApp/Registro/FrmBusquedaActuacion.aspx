<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmBusquedaActuacion.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActuacionSearch" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%--<%@ Register Src="~/Accesorios/SharedControls/ctrlBusqueda.ascx" TagPrefix="uc" TagName="ctrlBusqueda" %>--%>
<%@ Register Src="~/Accesorios/SharedControls/ctrlBusquedaAvanzada.ascx" TagPrefix="uc" TagName="ctrlBusqueda" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    
    <%--<link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />    
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.numeric.js" type="text/javascript"></script>--%>
   
   <script type="text/javascript" language="javascript">
       function seguridadURLPrevia() {
           if (document.referrer != "") {
               //Funciones
           }
           else {
               //mensaje de error
               location.href = '../PageError/DeniedPage.aspx';
           }
       }

       window.onload = seguridadURLPrevia;
        </script>
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
