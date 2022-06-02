<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlUbigeoLineal.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlUbigeoLineal" %>
<table style="border-bottom: 1px solid #800000; width: 100%; ">
    <tr>
        <td width="120px">
            <asp:Label ID="Label20" runat="server" Text="Cont./Dept.:" Width="80px" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_ContDepParticipanteAP" runat="server" Height="21px" Width="200px"
                AutoPostBack="True" ClientIDMode="AutoID"
                OnSelectedIndexChanged="ddl_ContDepParticipanteAP_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="Label10" runat="server" Text="País/Prov.:" Width="80px" />
        </td>
        <td align="right">
            <asp:DropDownList ID="ddl_PaisCiudadParticipanteAP" runat="server" Height="21px"
                Width="200px" AutoPostBack="True" ClientIDMode="AutoID"
                OnSelectedIndexChanged="ddl_PaisCiudadParticipanteAP_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="Label21" runat="server" Text="Ciud./Dist.:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_CiudadDistritoParticipanteAP" runat="server" Width="200px"
                Height="21px" AutoPostBack="False" ClientIDMode="AutoID">
            </asp:DropDownList>
        </td>
    </tr>
</table>
