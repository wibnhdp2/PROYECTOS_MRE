<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlUbigeo.ascx.cs"
    Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlUbigeo" %>
<%--<style type="text/css">
    .style1
    {
        width: 337px;
    }
</style>--%>
<table>
    <tr>
        <td style="width:115px">
            <asp:Label ID="Label20" runat="server" Text="Cont./Dept.:" width="90px" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_ContDepParticipanteAP" runat="server" Height="21px" Width="300px" style="cursor:pointer;"
                AutoPostBack="True" ClientIDMode="AutoID"
                OnSelectedIndexChanged="ddl_ContDepParticipanteAP_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td style="width:25px"></td>
        <td>
            <asp:Label ID="Label10" runat="server" Text="País/Prov.:" Width="98px" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_PaisCiudadParticipanteAP" runat="server" Height="21px" style="cursor:pointer;"
                Width="300px" AutoPostBack="True" ClientIDMode="AutoID"
                OnSelectedIndexChanged="ddl_PaisCiudadParticipanteAP_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label21" runat="server" Text="Ciud./Dist.:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_CiudadDistritoParticipanteAP" runat="server" Width="300px" style="cursor:pointer;"
                Height="21px" AutoPostBack="False" ClientIDMode="AutoID">
            </asp:DropDownList>
        </td>
        <td style="width:20px"></td>
        <td>
            <asp:Label ID="Label22" runat="server" Text="Centro Poblado:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_CentroPobladoParticipanteAP" runat="server" Width="300px" style="cursor:pointer;"
                Height="21px" AutoPostBack="True" ClientIDMode="AutoID">
            </asp:DropDownList>
        </td>
    </tr>
</table>
