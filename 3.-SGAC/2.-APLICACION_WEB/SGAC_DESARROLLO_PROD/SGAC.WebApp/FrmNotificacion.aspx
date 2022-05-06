<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNotificacion.aspx.cs" Inherits="SGAC.WebApp.FrmNotificacion" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table style="width: 85%; border-spacing: 0px;" align="center">
            <tr>
                <td>
                    <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblAlertaDescripcion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblOrigen" Text="BANDEJA" runat="server" Style="font-weight: 700; color: #800000;" />
                </td>
                <td style="text-align: right">
                    <asp:LinkButton ID="lbIrRemesa" runat="server" 
                        Font-Bold="True" Font-Size="10pt" ForeColor="Gray"
                        onclick="lbIrRemesa_Click" 
                        PostBackUrl="~/Contabilidad/FrmRemesaConsulta.aspx"
                        OnClientClick="window.document.forms[0].target='_parent';">Ir a Remesas Consulares</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">                    
                    <asp:Label ID="lblAviso" runat="server" Visible="false"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>     
                <td colspan="3">                    
                    <table>
                        <tr>
                            <td><asp:Label ID="Label3" runat="server" Text="Leyenda: " Font-Size="10pt" ForeColor="Black" Width="100px"></asp:Label></td>
                            <td style="border-width: 1px; border-color: #666; border-style: solid; font-weight: bold; font-size:10pt; width:120px; background-color: #FFAAAA" align="center">
                                Requiere Atención
                            </td>
                            <td style="border-width: 1px; border-color: #666; border-style: solid; font-weight: bold; font-size:10pt; width:120px; background-color:White" align="center">
                                Informativo
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="overflow:auto; height:200px; width:100%; border:1px solid #800000; margin: 5px;">
                    <asp:GridView ID="gdvAlerta" runat="server" AutoGenerateColumns="False" 
                        Width="920px" CssClass="mGrid" 
                        GridLines="None" SelectedRowStyle-CssClass="slt" 
                        ShowHeaderWhenEmpty="True" onrowdatabound="gdvAlerta_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="reno_sOficinaConsularOrigenId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                <HeaderStyle CssClass="ColumnaOculta" />
                                <ItemStyle CssClass="ColumnaOculta" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ORIGEN" HeaderText="OC Origen" >
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="reno_sOficinaConsularDestinoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                <HeaderStyle CssClass="ColumnaOculta" />
                                <ItemStyle CssClass="ColumnaOculta" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DESTINO" HeaderText="OC Destino" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>                            
                            <asp:BoundField DataField="RENO_CPERIODO" HeaderText="Periodo" >
                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <%--Columna: 5--%>
                            <asp:BoundField DataField="PENDIENTE" HeaderText="Cant. Remesas Pendientes" >
                                <ItemStyle Width="100px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OBSERVADO" HeaderText="Cant. Remesas Observadas" >
                                <ItemStyle Width="100px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENVIADO" HeaderText="Cant. Remesas Enviadas" >
                            <ItemStyle Width="100px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="APROBADO" HeaderText="Cant. Remesas Aprobadas" >
                            <ItemStyle Width="100px" HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                    </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3"><br /></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblTituloDetalle" Text="DETALLE" runat="server" Style="font-weight: 700; color: #800000;" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox ID="txtDetalle" runat="server" TextMode="MultiLine" Visible="false" Height="120px" Width="100%" Enabled="false" BackColor="White" 
                    style="font-size: 10pt; font-family: Arial;"></asp:TextBox>
                </td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>
