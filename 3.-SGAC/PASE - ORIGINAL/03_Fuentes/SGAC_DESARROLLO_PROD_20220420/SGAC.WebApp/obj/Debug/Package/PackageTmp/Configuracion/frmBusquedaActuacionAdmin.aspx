<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="frmBusquedaActuacionAdmin.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmBusquedaActuacionAdmin" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation"
    TagPrefix="label" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="Consulta - Actuación:"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table style="width: 90%;" align="center" class="mTblPrincipal">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="604px" />
                            <asp:CheckBox ID="chkUsarOficinaSeleccionada" runat="server" 
                                Text="¿Usar Oficina Seleccionada?" 
                                oncheckedchanged="chkUsarOficinaSeleccionada_CheckedChanged" 
                                AutoPostBack="True" />
                        </td>
                    </tr>
                    <div id="ocultar" runat="server" visible="false">
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Oficina Destino:"></asp:Label>
                            </td>
                            <td colspan="4">
                                
                                
                                <uc1:ctrlOficinaConsular ID="ctrlOficinaConsularDestino" runat="server" />
                                
                                
                            </td>
                        </tr>
                    </div>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Año:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAnioBusqueda" runat="server" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="RGE:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRGE" type="number" runat="server" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnBuscar" runat="server" Text="    Buscar" CssClass="btnSearch"
                                OnClick="btnBuscar_Click" />
                                
                        </td>
                        <td>
                        <asp:CheckBox ID="chkObligatorioFecha" runat="server" Text="No Validar Fecha Cierre de Mes" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <label:validation ID="ctrlValidacion" runat="server" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gdvActuaciones" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                runat="server" AutoGenerateColumns="False" Width="100%" GridLines="None">
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="iActuacionId" HeaderText="iActuacionId" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="iActuacionDetalleId" HeaderText="ActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="R.G.E." DataField="vCorrelativoActuacion" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Corr. Tarifa" DataField="vCorrelativoTarifario" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Fecha" DataField="dFechaRegistro" HeaderStyle-HorizontalAlign="Center"
                                        DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tarifa" DataField="vTarifa" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Descripción" DataField="vDescripcion">
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sOficinaConsularId" HeaderText="sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                        ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Oficina Consular" DataField="vOficinaCiudad" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Código Autoadhesivo" DataField="vCodigoInsumo" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>
                                    <%--Opciones Grilla--%>
                                    <asp:TemplateField HeaderText="Reasignar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnReasignar" ToolTip="Reasignar Actuación" runat="server" ImageUrl="../Images/img_16_reasignar.png"
                                                OnClick="Reasignar" iActuacionDetalleId='<%# Eval("iActuacionDetalleId")%>' dFechaRegistro='<%# Eval("dFechaRegistro")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Size="Smaller" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a>
                                    <asp:ImageButton ID="btnAnularAct" ToolTip="Anular Actuación" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                </a>
                            </ItemTemplate>
                            <HeaderStyle Font-Size="Smaller" />
                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                        </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
