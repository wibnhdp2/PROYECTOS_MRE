<%@ Page Title="Sistema de Gesti&oacute;n de Autoadhesivos Consulares" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SGAC.WebApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent" >
    <link rel="stylesheet" type="text/css" href="Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/site.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    
    
 
     
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
    </script>
    
    <%--<asp:Button ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />--%>
    <asp:Button ID="btnEjecutarFuncionario" runat="server" Text="Aceptar"  style="display:none;"
                                                    onclick="btnEjecutarFuncionario_Click"  />

</asp:Content>
