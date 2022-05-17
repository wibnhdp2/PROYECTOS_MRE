<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlBusquedaAutoadhesivo.ascx.cs"
    Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlBusquedaAutoadhesivo" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<div>
    <div>        
    <asp:HiddenField ID="HFGUID" runat="server" />
        <table width="90%">
            <tr>
                <td>
                    <h2><asp:Label ID="lblBusquedaAutoadhesivo" runat="server" Text="Consulta del Autoadhesivo Consular"></asp:Label></h2>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <asp:UpdatePanel runat="server" ID="UpdGrvPaginada" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hEnter" runat="server" />
                <asp:HiddenField ID="hid_iPersonaId" runat="server" Value="0" />                
                <asp:HiddenField ID="hdn_tipo_persona" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_tipo_direccion" runat="server" Value="0" />

                <table class="mTblPrincipal">
                    <tr>
                        <td>
                            <asp:Label ID="lblNroAutoadhesivo" runat="server" Text="Número de Autoadhesivo Consular: "></asp:Label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtNroAutoadhesivo" runat="server" Width="200px" MaxLength="20" CssClass="txtLetra"
                                               
                                              ToolTip="Para poder realizar la búsqueda debe ingresar o leer el Nro. del Autoadhesivo Consular." />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Buscar" runat="server" Text="   Buscar" CssClass="btnSearch" OnClick="btn_Buscar_Click"
                                            UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table> 

                <table width="90%">
                    <tr>
                        <td>
                            <Label:Validation ID="ctrlValActuacion" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:GridView ID="gdvActuaciones" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    runat="server" AutoGenerateColumns="False" Width="100%" 
                                 GridLines="None" OnRowCommand="gdvActuaciones_RowCommand"
                                                    onrowcreated="gdvActuaciones_RowCreated" 
                                 onrowdatabound="gdvActuaciones_RowDataBound">
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
                                                        <asp:BoundField DataField="sSeccionId" HeaderText="sSeccionId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTarifarioId" HeaderText="sTarifarioId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="R.G.E." DataField="vCorrelativoActuacion">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Corr. Tarifa" DataField="vCorrelativoTarifario">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fecha" DataField="dFechaRegistro" 
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tarifa" DataField="vTarifa">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Descripción" DataField="vDescripcion">
                                                            <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sOficinaConsularId" HeaderText="sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Oficina Consular" DataField="vOficinaCiudad">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fec. Hora Digitaliza" DataField="dFechaDigitaliza" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta"
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                             <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Usuario Digitaliza" DataField="vUsuarioDigitaliza" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle HorizontalAlign="Center" Width="80px"  CssClass="ColumnaOculta" />
                                                        </asp:BoundField>                                                        
                                                        <asp:BoundField DataField="sUsuarioDigitaliza" HeaderText="sUsuarioDigitaliza" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Código Autoadhesivo" DataField="vCodigoInsumo">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <%--Opciones Grilla--%>
                                                        <asp:TemplateField HeaderText="Seg." ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnSeguimiento" CommandName="Seguimiento" ToolTip="Seguimiento Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="~/Images/img_16_estado_tramite.png" Enabled="False" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="ColumnaOculta"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditarAct" CommandName="EditarAct" ToolTip="Editar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="~/Images/img_16_edit.png" Enabled="False" EnableViewState="True" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta"/>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="ColumnaOculta"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultarAct" CommandName="ConsultarAct" ToolTip="Consultar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="~/Images/img_gridbuscar.gif" EnableTheming="True" EnableViewState="True" Enabled="True" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reasignar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnReasignar" CommandName="Reasignar" ToolTip="Consultar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="~/Images/img_16_reasignar.png" Enabled="False" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta"/>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="ColumnaOculta"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <ItemTemplate>
                                                                <a>
                                                                    <asp:ImageButton ID="btnAnularAct" CommandName="Anular" ToolTip="Anular Actuación"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="~/Images/img_grid_delete.png" Enabled="False" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta"/>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="ColumnaOculta"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                        </td>
                    </tr>
                </table>

                <table width="90%">
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

