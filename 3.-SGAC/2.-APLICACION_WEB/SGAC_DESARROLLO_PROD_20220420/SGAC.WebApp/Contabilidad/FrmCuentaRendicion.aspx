<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmCuentaRendicion.aspx.cs" Inherits="SGAC.WebApp.Contabilidad.CuentaRendicion" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>

    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.1.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

    <script src="<%= ResolveUrl("~/signalr/hubs") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();

            toastr.options.closeButton = true;

            var proxy = $.connection.myHub;
            proxy.client.receiveNotification = function (message, user) {
                var userName = $('#<%=lblUserName.ClientID%>').html();
                if (user == userName) {
                    toastr.info(message, user);
                }
            };
            $.connection.hub.start();
        });

        function notificar(msg, user_receive) {
            var proxy = $.connection.myHub;
            proxy.server.sendNotifications(msg, user_receive);
            $.connection.hub.start();
        }

    </script>

    <style type="text/css">
        .style2
        {
            text-align: center;
            font-weight: bold;
        } 
        .style3
        {
            text-align: center;
            font-weight: bold;
            width: 273px;
        }
        .style4
        {
            width: 273px;
            background-color: #FFFFFF;
        }
        .style5
        {
            text-align: center;
            font-weight: bold;
            width: 352px;
        }
        .style6
        {
            width: 352px;
            background-color: #FFFFFF;
        }
        .style7
        {
            text-align: center;
            background-color: #FFFFFF;
        }
        .style8
        {
            font-size: large;
            font-family: Tahoma;
            background-color: #999999;
            color: #800000;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%--Titulo--%>
    <table class="mTblTituloM" align="center">
        <tr><td><h2><asp:Label ID="lblTituloRendCuenta" runat="server" Text="Rendición de Cuentas"></asp:Label></h2></td></tr>
    </table>
    <table style="width: 90%" align="center">
    <tr>
    <td>
        <div id="tabs">
            <ul><li><a href="#tab-1"><asp:Label ID="lblTabRendCuenta" runat="server" Text="Rendición de Cuentas"></asp:Label></a></li></ul>
            <div id="tab-1">
                <asp:Label ID="lblFechaActual" runat="server" Text=""></asp:Label>
                <table style="border-collapse:collapse;border:1px solid;">
                    <tr style="background:#4E102E">
                        <td ><font style="color: White">Descripción Proceso</font></td>
                        <td class="style2"><font style="color: White">Fecha</font></td>
                        <td class="style3"><font style="color: White">Alerta en</font></td>
                        <td class="style2"><font style="color: White">Acción</font></td>
                    </tr>
                    <tr>
                        <td class="style6">Cierre Mensual</td>
                        <td class="style7">31/06/2014</td>
                        <td class="style4">&nbsp;</td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="Ejecutar" Width="100px" CssClass="btnGeneral" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">Envío de Remesas</td>
                        <td class="style7">03/07/2014</td>
                        <td class="style4">&nbsp;</td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="Ejecutar" Width="100px" CssClass="btnGeneral" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </td>
    </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
</asp:Content>