<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlPageBar.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlPageBar" %>
 

<asp:UpdatePanel runat="server" ID="updPaginator"  updatemode="Conditional">

    <Triggers>
        <asp:AsyncPostBackTrigger controlid="btnPrimero" eventname="Click" />
        <asp:AsyncPostBackTrigger controlid="btnAnterior" eventname="Click" />
        <asp:AsyncPostBackTrigger controlid="btnSiguiente" eventname="Click" />
        <asp:AsyncPostBackTrigger controlid="btnUltimo" eventname="Click" />        
       
       
    </Triggers>

    <ContentTemplate>        

        <table width="100%" style="background-color: #CCCCCC;">

            <tr>

                <td>(Página
                    <asp:label id="lblPaginaActual" Runat="server" Text="1"></asp:label>&nbsp;de
                    <asp:label id="lblTotalPaginas" Runat="server" Text="0"></asp:label>)
                </td>

                <td>
                    (Total Registros: <asp:Label id="lblInicio" runat="server" Text="0"></asp:Label>&nbsp;-&nbsp;<asp:Label id="lblFin" runat="server" Text="0"></asp:Label> 
                    &nbsp;de&nbsp;<asp:Label id="lblNroRegistros" runat="server" Text="0"></asp:Label>)
                </td>

                <td valign="top" align="right">
                    Página
                    <asp:TextBox ID="txtPagina" runat="server" Width="53px" Text="1" MaxLength="4"    onpaste="return false"></asp:TextBox> 
                    <asp:Button ID="btnIrPagina" runat="server" Text="Ir" onclick="btnIrPagina_Click" />
                </td>

                <td align="right">
                    <asp:imagebutton id="btnPrimero" Runat="server" Enabled="false"
                        ImageUrl="../../Images/NavFirstPageDisabled.gif" 
                        onclick="btnPrimero_Click" Width="22px" />
                    <asp:imagebutton id="btnAnterior" Runat="server" Enabled="false"
                        ImageUrl="../../Images/NavPreviousPageDisabled.gif" onclick="btnAnterior_Click" />
                    <asp:imagebutton id="btnSiguiente" Runat="server" Enabled="false"
                        ImageUrl="../../Images/NavNextPageDisabled.gif" onclick="btnSiguiente_Click" />
                    <asp:imagebutton id="btnUltimo" Runat="server" Enabled="false"
                        ImageUrl="../../Images/NavLastPageDisabled.gif" onclick="btnUltimo_Click" />
                </td>

            </tr>

        </table>

    </ContentTemplate>

</asp:UpdatePanel>
 
 