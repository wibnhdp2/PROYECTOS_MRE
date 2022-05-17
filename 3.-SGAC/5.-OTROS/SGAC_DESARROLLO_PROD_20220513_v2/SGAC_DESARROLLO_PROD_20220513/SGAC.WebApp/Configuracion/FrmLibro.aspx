<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmLibro.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmLibro" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation" TagPrefix="Label" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

   
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloLibros" runat="server" Text="Configuración de Carga Inicial - Libros - Almacén"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td>
                                                <uc1:ctrlOficinaConsular ID="ddlOficinaConsular" runat="server" Width="400px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblTipoConfiguracion" runat="server" Text="Tipo Configuración:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoConfiguracion" runat="server" Width="200px" OnSelectedIndexChanged="ddlTipoConfiguracion_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoInsumoConsulta" runat="server" Text="Tipo Insumo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoInsumoConsulta" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPeriodo" runat="server" Text="Periodo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPeriodo" runat="server" Width="60px" Enabled="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%;" colspan="2">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlLibroCorrelativos" runat="server" Width="100%" >
                                        <table style = "border-spacing: 0" width = "100%">
                                            <tr>
                                                <td colspan="2" style="vertical-align: top">
                                                    <asp:GridView ID="gdvLibroCorrelativos" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False" GridLines="None" 
                                                        OnRowCommand="gdvLibroCorrelativos_RowCommand" 
                                                        onrowdatabound="gdvLibroCorrelativos_RowDataBound">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="libr_sLibroId" HeaderText="libr_sLibroId" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Oficina Consular" DataField="ubge_vDistrito">
                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Periodo" DataField="libr_sPeriodo">
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Tipo Libro" DataField="para_vDescripcion">
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Número Libro" DataField="libr_iNumeroLibro">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Número Ecritura" DataField="libr_iNumeroEscritura">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Número Folio Actual" DataField="libr_iNumeroFolioActual">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Número Folio Total" DataField="libr_iNumeroFolioTotal">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Estado" DataField="libr_vEstadoLibro">
                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="libr_bCerrado" HeaderText="libr_bCerrado" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="ESTADO" ItemStyle-CssClass="ColumnaOculta">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblestado" runat="server" Text='<%#Eval("libr_vEstadoLibro")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                           </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                    <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                                </td>
                                            </tr>
                                            <%--</table>--%>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlStockAlmacen" runat="server" Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gdvStockAlmacen" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                        GridLines="None" OnRowCommand="gdvStockAlmacen_RowCommand" SelectedRowStyle-CssClass="slt">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="stck_sStockId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="stck_sStockId"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ubge_vDistrito" HeaderText="Oficina Consular">
                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="para_vDescripcion" HeaderText="Tipo Insumo">
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="stck_INumeroStockMinimo" HeaderText="Cantidad Stock Mínimo">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="UltimaFechaOrden" HeaderText="Fecha">
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        CommandName="Consultar" ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Consultar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        CommandName="Editar" ImageUrl="../Images/img_grid_modify.png" ToolTip="Editar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                    <PageBarContent:PageBar ID="ctrlPaginadorStock" runat="server" OnClick="ctrlPaginadorStock_Click" />
                                                </td>
                                            </tr>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblOficinaConsularMant" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlOficinaConsularMant" runat="server" Width="400px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlOficinaConsular" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblLibroTipoMant" runat="server" Text="Tipo Configuracion:"></asp:Label>
                                            </td>
                                            <td colspan="3" >
                                                <asp:DropDownList ID="ddlLibroTipoMant" runat="server" Width="400px" OnSelectedIndexChanged="ddlLibroTipoMant_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlLibroTipo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblPeriodoMant" runat="server" Text="Periodo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlPeriodoMant" runat="server" Width="150px" >
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlPeriodo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblLibroNumero" runat="server" Text="Número de Libro:"></asp:Label>
                                            </td>
                                            <td style="width:250px">
                                                <asp:TextBox runat="server" ID="txtNumeroLibro" Width="150px" CssClass="campoNumero"
                                                    MaxLength="5" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td class="style1">
                                                <asp:Label ID="lblNumeroEscritura" runat="server" Text="Número de Escritura Pública:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNumeroEscritura" Width="150px" onkeypress="return validatenumber(event)"
                                                    CssClass="campoNumero" MaxLength="5"></asp:TextBox>
                                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblNumeroFolioInicial" runat="server" Text="Número de Folio Inicial:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNumFolioInicial" Width="150px" CssClass="campoNumero"
                                                    MaxLength="5" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                                <td class="style1">
                                                    <asp:Label ID="lblNumeroFolioActual" runat="server" Text="Número de Folio Actual:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNumFolioActual" Width="150px" CssClass="campoNumero"
                                                    MaxLength="5" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblNumeroFoliosTotales" runat="server" Text="Número de Folios Totales:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNumeroFoliosTotales" Width="150px" CssClass="campoNumero"
                                                    MaxLength="5" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                                                
                                            </td>
                                            <td>
                                            <asp:Label ID="lblEtiquetaFoliosDisponibles" runat="server"  
                                                        Text="Número de Folios disponibles:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFoliosDisponibles" runat="server"  
                                                        Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblIntervaloFolios" runat="server"  
                                                        Text="Intervalo de folios:"></asp:Label>
                                            </td>
                                            <td align="left" colspan="2">
                                            <asp:TextBox runat="server" ID="txtIntervaloFolios" Width="400px" 
                                                    MaxLength="200" onkeypress="return validarRangosPaginas(event)"></asp:TextBox>                                            
                                            <br />                                            
                                            <asp:Label ID="lblTextoIntervalo" runat="server" Text="Introduzca rangos de números y/o folios separados por comas (ej.2, 5-8)" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgFolioNC" runat="server" ImageUrl="~/Images/NoCorre.jpg" alt="No corre"/>
                                            </td>
                                        </tr>
                                      
                                        <tr>
                                            <td class="style1">
                                                <asp:Label ID="lblEstadoLibro" runat="server"  
                                                    Text="Libro Cerrado:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkEstadoLibro" runat="server" Checked="false" 
                                                    Text="" />
                                            </td>
                                            <td valign="top">
                                                <asp:Button ID="btnImprimirFojasNC" runat="server" Text="    Imprimir" 
                                                    CssClass="btnPrint" onclick="btnImprimirFojasNC_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td>
                                                <asp:Label ID="lblTipoInsumo" runat="server" Text="Tipo Insumo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlTipoInsumo" runat="server" Width="400px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCantidadStockMinimo" runat="server" Text="Cantidad stock mínimo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtStockMinimo" runat="server" CssClass="campoNumero" onkeypress="return validatenumber(event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="hdn_sOficinaConsularId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sUsuarioId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sLibroId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sStockId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sAccionId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sIndiceSeleccionado" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sTipoInsumo" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function txtcontrolError(ctrl) {
            var x = ctrl.value;
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function ddlcontrolError(ctrl) {
            var x = ctrl.selectedIndex;
            var bolValida = true;
            if (x < 1) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function ValidarRegistro() {
            var bolValida = true;


            if (ddlcontrolError(document.getElementById("<%= ddlLibroTipoMant.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%= txtNumeroLibro.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNumeroEscritura.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNumFolioInicial.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNumFolioActual.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNumeroFoliosTotales.ClientID %>")) == false) bolValida = false;

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }
    </script>

</asp:Content>