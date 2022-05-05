<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmExpedienteMigratorio.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmExpedienteMigratorio" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
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
                        <asp:Label ID="lblTituloExpedientes" runat="server" Text="Configuración de Carga Inicial - Número de Expedientes"></asp:Label></h2>
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
                                            <td>
                                                <asp:Label ID="lblTipoDocMigratorio" runat="server" Text="Tipo Doc. Migratorio:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoDocMigratorioBusqueda" runat="server" Width="200px">
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
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="gdvExpedientesCorrelativos" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                    AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvExpedientesCorrelativos_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="exp_sExpedienteId" HeaderText="exp_sExpedienteId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Oficina Consular" DataField="ubge_vDistrito">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Periodo" DataField="exp_sPeriodo">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tipo Doc. Migratorio" DataField="para_vDescripcion">
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Número Expediente" DataField="exp_iNumeroExpediente">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <%--<asp:BoundField HeaderText="Estado" DataField="exp_vEstadoExpediente">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>--%>
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
                                                        <%--<asp:BoundField DataField="libr_bCerrado" HeaderText="libr_bCerrado" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>--%>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                                <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsularMant" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td>
                                                <uc1:ctrlOficinaConsular ID="ddlOficinaConsularMant" runat="server" Width="500px" />
                                                <asp:Label ID="lblCO_ddlOficinaConsular" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoDocMigraMant" runat="server" Text="Tipo Doc. Migratorio:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoDocMigraMant" runat="server" Width="200px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlTipoDocMigra" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPeriodoMant" runat="server" Text="Periodo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPeriodoMant" runat="server" Width="80px" Enabled="false">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlPeriodo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNroExpediente" runat="server" Text="Número de Expediente:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNroExpediente" Width="150px" CssClass="campoNumero"
                                                    MaxLength="5" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                                <asp:Label ID="lblEstadoLibro" runat="server" Text="Libro Cerrado:" Enabled="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkEstadoLibro" runat="server" Text="" Checked="false" Enabled="False" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="hdn_sOficinaConsularId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sUsuarioId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sExpedienteId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sAccionId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sIndiceSeleccionado" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_sTipoDocMigra" runat="server" Value="0" />
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

            if (ddlcontrolError(document.getElementById('<%= ddlOficinaConsularMant.FindControl("ddlOficinaConsular").ClientID %>')) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddlTipoDocMigraMant.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%= txtNroExpediente.ClientID %>")) == false) bolValida = false;

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
