<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmRegistroAsistencia.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRegistroAsistencia" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlUploader.ascx" TagName="ctrlUploader"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc4" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td colspan="7">
                    <h2>
                        <asp:Label ID="lblTituloOfConsular" runat="server" Text="PAH - PALH"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label>
                            </a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="120px">
                                                <asp:Label ID="Label4" runat="server" Text="Nro Documento:" Width="100px"></asp:Label>
                                            </td>
                                            <td width="100px" align="left">
                                                <asp:TextBox ID="txtNroDocumento" runat="server" Width="80px" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" OnTextChanged="txtNroDocumento_TextChanged" AutoPostBack="True" />
                                            </td>
                                            <td width="150px" style="text-align: right" colspan="2">
                                                <asp:Label ID="Label5" runat="server" Text="Primer Apellido: " Width="100px"></asp:Label>
                                            </td>
                                            <td width="150px" align="left">
                                                <asp:TextBox ID="txtPriApellido" runat="server" Width="90px" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" onkeypress="return ValidarSujeto(event)"  />
                                            </td>
                                            <td width="150px" style="text-align: right" colspan="2">
                                                <asp:Label ID="Label6" runat="server" Text="Segundo Apellido: " Width="140px"></asp:Label>
                                            </td>
                                            <td width="150px" align="left" colspan="2">
                                                <asp:TextBox ID="txtSegApellido" runat="server" Width="90px" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" onkeypress="return ValidarSujeto(event)"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server" OnClick="ctrlToolBar1_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <Label:Validation ID="ctrlValidacionVentanilla" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <asp:GridView ID="GrdAnotaciones" runat="server" AlternatingRowStyle-CssClass="alt" Width="100%"
                                                    AutoGenerateColumns="False" CssClass="mGrid" GridLines="None" OnRowCommand="GrdAnotaciones_RowCommand"
                                                    OnRowCreated="GrdAnotaciones_RowCreated">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="iPersonaId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="PersonaId"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vNroDocumento" HeaderText="Nº Documento">
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vApellidoPaterno" HeaderText="Primer Apellido">
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vApellidoMaterno" HeaderText="Segundo Apellido">
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vNombres" HeaderText="Nombres">
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnPrint" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Seleccionar" ImageUrl="../Images/img_sel_check.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <uc3:ctrlPageBar ID="ctrlPageBar1" runat="server" OnClick="ctrlPageBar1_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblDetalleAsistencia" runat="server" Font-Bold="True" ForeColor="#990033"
                                                    Text="Detalle de Asistencias"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                            <td colspan="3" style="text-align: right">
                                                <asp:Button ID="btnNuevaAsistencia" runat="server" OnClick="btnNuevaAsistencia_Click"
                                                    Text="Nueva Asistencia" Width="127px" CssClass="btnGeneral" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <asp:GridView ID="GrdAnatacion2" runat="server" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="vNombre,sOficinaConsularOrigenId,sOficinaConsularId,asis_iFuncionarioId,iPersonaId"
                                                    GridLines="None" OnRowCommand="GrdAnatacion2_RowCommand" OnRowDataBound="GrdAnatacion2_RowDataBound">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="iAsistenciaId" HeaderText="ID">
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTipAsistencia" HeaderText="Tipo Asistencia">
                                                            <ItemStyle Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dFecServicio" HeaderText="Fecha" HtmlEncode="False" HtmlEncodeFormatString="False">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vNroCaso" HeaderText="Nº Informe">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vHoraInicio" HeaderText="Hora Inicio">
                                                            <ItemStyle HorizontalAlign="Center" Width="65px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vHoraFin" HeaderText="Hora Fin">
                                                            <ItemStyle HorizontalAlign="Center" Width="65px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Oficina Consular">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOficinaConsularId" runat="server" Text='<%# Eval("vOficinaConsularId").ToString() == "" ? "-":Eval("vOficinaConsularId").ToString() %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DescripcionTipoServId" HeaderText="Tipo Servicio">
                                                            <ItemStyle Width="180px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Consultar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnPrint1" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Consultar" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEdit1" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Editar" ImageUrl="../Images/img_grid_modify.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="asis_sCircunscripcionId" HeaderText="asis_sCircunscripcionId"
                                                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <uc3:ctrlPageBar ID="ctrlPageBar2" runat="server" OnClick="ctrlPageBar2_Click" />
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
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:HiddenField ID="hidNomAnotFile" runat="server" />
                                                    <asp:Label ID="lblValidacion" runat="server" Text="Debe ingresar los campos requeridos (*)"
                                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                    </asp:Label>
                                                    <div align="left">
                                                        &nbsp;<asp:Label ID="lblNombre" runat="server" Visible="False"></asp:Label>
                                                    </div>
                                                    <asp:Panel ID="pnlRtnOpcion" runat="server">
                                                        <div align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="130px">
                                                                        <asp:Label ID="lblTipoAyuda" runat="server" Text="Tipo Ayuda PALH:"></asp:Label>
                                                                        <asp:Label ID="lblTipPAH" runat="server" Text="Tipo Ayuda PAH:"></asp:Label>
                                                                    </td>
                                                                    <td width="300px">
                                                                        <asp:DropDownList ID="ddl_TipAyPAHL" runat="server" AutoPostBack="True" Enabled="False"
                                                                            OnSelectedIndexChanged="ddl_TipAyPAHL_SelectedIndexChanged" Width="265px" TabIndex="5"
                                                                            Height="20px" />
                                                                        <asp:Label ID="lblCO_ddl_TipAyPAHL" runat="server" Style="color: #FF0000;" Text="*"></asp:Label>
                                                                        <asp:DropDownList ID="ddl_TipAyPAH" runat="server" AutoPostBack="True" Enabled="False"
                                                                            Height="20px" OnSelectedIndexChanged="ddl_TipAyPAH_SelectedIndexChanged" TabIndex="6"
                                                                            Width="265px" />
                                                                        <asp:Label ID="Label7" runat="server" Style="color: #FF0000;" Text="*"></asp:Label>
                                                                    </td>
                                                                    <td width="120px">
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="130px">
                                                                        <asp:Label ID="LblTipoAnot" runat="server" Text="Solicitud Informes:"></asp:Label>
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:DropDownList ID="ddl_Otros" runat="server" Enabled="False" Height="20px" Width="428px"
                                                                            TabIndex="7" />
                                                                        <asp:Label ID="Label9" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="130px">
                                                                        <asp:Label ID="Label2" runat="server" Text="Fecha Servicio:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <SGAC_Fecha:ctrlDate ID="txtFecServcio" runat="server" />
                                                                        <asp:Label ID="Label12" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblNroCaso" runat="server" Text="Nº Informe Técnico:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNroCaso" runat="server" CssClass="txtLetra" MaxLength="10" onkeypress="return validarDirecciones(event)"
                                                                            TabIndex="9" Width="270px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="130px">
                                                                        <asp:Label ID="lblHoraInicio" runat="server" Text="Hora Inicio:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtHoraInicio" runat="server" Enabled="False" MaxLength="5" onBlur="validaHora(this)"
                                                                            onkeypress="return isHoraKey(this,event)" TabIndex="10" Width="42px" />
                                                                        <asp:Label ID="lblFHoraInicio" runat="server" Font-Bold="True" Text="(hh:mm)"></asp:Label>
                                                                        <asp:Label ID="lbl_txtHoraInicio" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblHoraFin" runat="server" Text="Hora Fin:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtHoraFin" runat="server" onkeypress="return isHoraKey(this,event)"
                                                                            onBlur="validaHora(this)" MaxLength="5" Enabled="False" TabIndex="11" Width="42px" />
                                                                        <asp:Label ID="lblFHoraFin" runat="server" Font-Bold="True" Text="(hh:mm)"></asp:Label>
                                                                        <asp:Label ID="lbl_txtHoraFin" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="lblOficinaConsular" runat="server" Text="Of. Consular Destino:"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <uc4:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Width="428px" Enabled="False"
                                                                    TabIndex="12" />
                                                                <asp:Label ID="lbl_ctrlOficinaConsular1" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="Label1" runat="server" Text="Departamento:"></asp:Label>
                                                            </td>
                                                            <td width="200px">
                                                                <asp:DropDownList ID="ddl_Departamento" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Pais_SelectedIndexChanged"
                                                                    TabIndex="13" Width="170px" />
                                                                <asp:Label ID="lbl_ddl_Pais" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="70px">
                                                                <asp:Label ID="lblCiudad" runat="server" Text="Provincia :"></asp:Label>
                                                            </td>
                                                            <td width="200px">
                                                                <asp:DropDownList ID="ddl_Provincia" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Ciudad_SelectedIndexChanged"
                                                                    TabIndex="14" Width="170px" />
                                                                <asp:Label ID="lbl_ddl_Ciudad" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="70px">
                                                                <asp:Label ID="lblDistrito" runat="server" Text="Distrito :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_Distrito" runat="server" Enabled="False" TabIndex="15"
                                                                    Width="140px" AutoPostBack="True" 
                                                                    onselectedindexchanged="ddl_Distrito_SelectedIndexChanged" />
                                                                <asp:Label ID="lbl_ddl_Distrito" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TR_Cir">
                                                            <td>
                                                                <asp:Label ID="Label10" runat="server" Text="Circunscripción :"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="ddlCircunscripcion" runat="server" Height="21px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbl_obligatorio_Circunscripcion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="lblMonto" runat="server" Text="Moneda"></asp:Label>
                                                                :
                                                            </td>
                                                            <td width="200px">
                                                                <asp:DropDownList ID="ddl_Moneda" runat="server" Enabled="False" Width="170px" TabIndex="16">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbl_ddl_Moneda" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="50px">
                                                                <asp:Label ID="lblMonto0" runat="server" Text="Monto:"></asp:Label>
                                                            </td>
                                                            <td width="200px">
                                                                <asp:TextBox ID="txtMonto" runat="server" CssClass="campoNumero" onkeypress="return isDecimalKey(this,event)"
                                                                    onBlur="validanumeroLostFocus(this)" Enabled="False" TabIndex="17" Width="80px"
                                                                    MaxLength="8" />
                                                                <asp:Label ID="lbl_txtMonto" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="150px">
                                                                &nbsp;
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionario :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlFuncionario" runat="server" Width="430px" Enabled="False"
                                                                    TabIndex="19">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblFunc" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            
                                                                <td width="130px">
                                                                    <asp:Label ID="lblDirURL" Visible="false" runat="server" Text="Dirección URL:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDirURL" Visible="false" runat="server" Width="698px" Enabled="False" onBlur="conMayusculas(this)"
                                                                        CssClass="txtLetra" TabIndex="20" MaxLength="200" />
                                                                </td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="lblTipoEstado" runat="server" Text="Estado:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_Estado" runat="server" Enabled="False" Width="170px" TabIndex="21">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbl_ddl_Estado" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="TR_Oficina" runat="server">
                                                            <td>
                                                                <asp:Label ID="Label11" runat="server" Text="Oficina Consular :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <uc4:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="604px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div align="center">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="150px">
                                                                <asp:Button ID="btnAgregarBene" runat="server" OnClick="btnAgregarBene_Click" Text="Agregar Beneficiario"
                                                                    CssClass="btnGeneral" Width="140px" />
                                                            </td>
                                                            <td width="230px" align="right">
                                                                <asp:Label ID="Label3" runat="server" Text="Número Beneficiarios:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtNumBene" runat="server" TabIndex="18" onkeypress="return isNumberKey(event)"
                                                                    onBlur="validanumeroLostFocus(this)" Width="50px" MaxLength="2" CssClass="campoNumero"
                                                                    Enabled="False" AutoPostBack="True"></asp:TextBox>
                                                                <asp:Label ID="lbl_TxtNumBene" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hd_Beneficiario" runat="server" />
                                                    <asp:HiddenField ID="hdn_actu_iPersonaId" runat="server" Value="0" />
                                                    <asp:Button ID="btnRealizarBusqueda" runat="server" CssClass="hideControl" OnClick="btnRealizarBusqueda_Click" />
                                                    <div style="border-style: double; overflow: auto; left: 0px; width: 99%; top: 0px;
                                                        height: 200px">
                                                        <asp:GridView ID="gvBeneficiario" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                            GridLines="None" ShowHeaderWhenEmpty="True" DataKeyNames="asbe_iAsistenciaBeneficiarioId,asbe_iAsistenciaId,asbe_sDocumentoTipoId,asbe_ISolicitante,asbe_iPersonaId,asbe_sGeneroId"
                                                            OnRowCommand="gvBeneficiario_RowCommand" Width="98%">
                                                            <Columns>
                                                                <asp:BoundField DataField="asbe_vNombreDocumento" HeaderText="Documento">
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="asbe_vDocumentoNumero" HeaderText="N° Documento">
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="asbe_vApellidoPaterno" HeaderText="Primer Apellido">
                                                                    <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="asbe_vApellidoMaterno" HeaderText="Segundo Apellido">
                                                                    <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="asbe_vNombres" HeaderText="Nombres">
                                                                    <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Genero" HeaderText="Género">
                                                                    <ItemStyle Width="70px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit2" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            CommandName="Editar" ImageUrl="../Images/img_grid_modify.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                            <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                                                            <EmptyDataTemplate>
                                                                <table id="tbSinDatos">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="10%">
                                                                                <asp:Image runat="server" ID="imgWarning" ImageUrl="../Images/img_16_warning.png" />
                                                                            </td>
                                                                            <td width="5%">
                                                                            </td>
                                                                            <td width="85%">
                                                                                <asp:Label ID="lblSinDatos" runat="server" Text="No se encontraron Datos..."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                            <AlternatingRowStyle />
                                                            <PagerStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <table>
                                                    <tr>
                                                        <td width="130px">
                                                            <asp:Label ID="LblDescAnot" runat="server" Text="Comentarios:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescAnot" runat="server" Width="698px" Height="68px" TextMode="MultiLine"
                                                                onBlur="conMayusculas(this)" CssClass="txtLetra" Enabled="False" TabIndex="22" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    <div>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="130px">
                                                                    <asp:Label ID="LblAdjAnot" runat="server" Text="Nombre Archivo :"></asp:Label>
                                                                </td>
                                                                <td width="500px">
                                                                    <uc1:ctrlUploader ID="ctrlUploader1" runat="server" FileExtension=".pdf" FileSize="102400"
                                                                        OnClick="MyUserControlUploader1Event_Click" />
                                                                    <br />
                                                                    <asp:Label ID="lblLeyenda" runat="server" Text="Solo se permiten guardar archivos de Tipo PDF de un tamaño Max. de 1024 kb"
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="CmdVisualizar" runat="server" OnClick="Button1_Click" Text="Visualizar Archivo"
                                                                        CssClass="btnGeneral" Width="147px" Enabled="False" TabIndex="18" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="6">
                                                                <asp:HiddenField ID="hidNomAdjFile" runat="server" />
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" Text="(*)Selecionar uno de los tipos de asistencia"
                                                                    Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ctrlUploader1" />
                                    <asp:PostBackTrigger ControlID="CmdVisualizar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    </div>
    <script language="javascript" type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        var patron = new Array(2, 2)

        function validatenumber(event) {
            return validatedecimal(event);
        }


        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function nuevaventana(ancho, alto) {
            var URL;
            var posicion_x;
            var posicion_y;
            URL = "../Registro/frmUpload.aspx";
            posicion_x = (screen.width / 2) - (ancho / 2);
            posicion_y = (screen.height / 2) - (alto / 2);
            open(URL, "Menu_AMB", "width=" + ancho + ",height=" + alto + ",menubar=0 ,toolbar=0,scrollbars=NO, left=" + posicion_x + ",top=" + posicion_y);
        }

        function conMayusculas(control) {
            control.value = control.value.toUpperCase();
        }

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function Cambiar_Nombre(strNombre) {
            $('#<%= lblTabRegistro.ClientID %>').val(strNombre);
        }
        function ValidarLetras(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 164) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function ValidarSujeto(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 164) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }
            if (charCode == 39) {
                letra = true;
            }
            if (charCode == 231) {
                letra = true;
            }
            if (charCode == 199) {
                letra = true;
            }
            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isNombreApellido(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 166) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;

        }

        function isLetraNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 32) {
                letra = true;
            }

            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }

            if (charCode >= 46 && charCode <= 60) {
                letra = true;
            }

            if (charCode > 63 && charCode < 91) {
                letra = true;
            }

            if (charCode > 94 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }

            if (charCode == 144) {
                letra = true;
            }

            if (charCode > 159 && charCode < 164) {
                letra = true;
            }

            if (charCode == 181) {
                letra = true;
            }

            if (charCode == 214) {
                letra = true;
            }

            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            return letra;
        }

        function validaHora(ctrl) {
            var x = ctrl.value;
            var valor = true;

            if (x.length == 4) {
                x = x + "0";
                ctrl.value = x;
            }

            var hora = x.substring(0, 2);
            var minuto = x.substring(3, 5);

            if (hora > 23) {
                valor = false;
            }
            if (minuto > 59) {
                valor = false;
            }
            if (x.length < 5) {
                valor = false;
            }
            if (x.length == 0) {
                valor = true;
            }
            if (!valor) {

                ctrl.focus();
                alert("Formato de hora Incorrecto");
                ctrl.value = "";

            }
        }

        function ComparaHoras(horaIni, horaFin) {
            var valor = false;
            xHora = horaIni.substring(0, 2);
            xMinuto = horaIni.substring(3, 5);

            yHora = horaFin.substring(0, 2);
            yMinuto = horaFin.substring(3, 5);

            if (xHora > yHora) {
                valor = true
            }
            else {
                if (xHora == yHora) {

                    if (xMinuto > yMinuto) {
                        valor = true
                    }
                }
            }

            return valor;
        }

        function validanumeroLostFocus(control) {
            var valor = true;
            var texto = control.value.trim();
            control.value = texto;

            var letras = "0123456789.";
            var n = 0;
            while (n < texto.length) {
                var x = texto.substring(n, n + 1)
                if (letras.indexOf(x) < 0) {
                    valor = false;
                }
                n++;
            }

            if (texto.substring(0, 1) == ".") {
                valor = false;
            }

            if (texto.substring(n - 1, n) == ".") {
                valor = false;
            }

//            if (texto == "0") {
//                valor = false;
//            }

            if (!valor) {
                control.focus();
                alert("Número Incorrecto");
                control.value = "";
            }

        }

        function isDecimalKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = "."
            var x = ctrl.value
            var y = x.indexOf(FIND)

            if (charCode == 46) {
                if (y != -1 || x.length == 0)
                    return false;
            }
            if (charCode != 46 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }

        function isHoraKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = ":";
            var x = ctrl.value;
            var y = x.indexOf(FIND);

            var minuto = x.split(':');
            if (x.length == 2) {
                x = x + ":";
                ctrl.value = x;
            }

            if (x.split(':').length = 1) {
                if (x.length > 1 && x.length < 3) {
                    if (minuto[0].length > 1 && charCode != 58) {
                        return false;
                    }
                }
            }

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 58) {
                return false;
            }

            if (charCode < 48 || charCode > 58)
                return false;
            return true;
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }


        function txtcontrolError(ctrl) {
            var x = ctrl.value.trim();
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

        function RecuperaDatos() {

            return true;
        }

        function fechavalida() {
            document.getElementById('<%= txtFecServcio.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
        }

        function numDigitosDocumento(field) {
            field.value = field.value.toUpperCase();

            var codDocumento = ddl.options[ddl.selectedIndex].value;

            field.style.border = "1px solid #888888";
            if (field.value.length > 0) {
                if (codDocumento == 1) {
                    if (field.value.length != 8) {
                        field.style.border = "1px solid Red";
                        alert("NÚMERO DE DOCUMENTO INCORRECTO");

                        field.value = "";
                    }
                }
                else {
                    if (field.value.length < 5) {
                        field.style.border = "1px solid Red";
                        alert("NÚMERO DE DOCUMENTO INCORRECTO");
                        field.value = "";
                    }
                }
            }
        }

        function Validar() {
            var ddlTipAyPAH;
            try {
                ddlTipAyPAH = document.getElementById("<%=ddl_TipAyPAH.ClientID %>");
            }
            catch (err) {
                ddlTipAyPAH = document.getElementById("<%=ddl_TipAyPAHL.ClientID %>");
            }

            if (ddlTipAyPAH == null) {
                return ValidarPAHL();
            } else {
                return ValidarPAH();
            }
        }



        function ValidarPAH() {

            var bolValida = true;
            var bolValidaHora = false;

            var ddlTipAyPAH = document.getElementById("<%=ddl_TipAyPAH.ClientID %>");

            if (ddlcontrolError(document.getElementById("<%=ddl_TipAyPAH.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_Estado.ClientID %>")) == false) bolValida = false;

            var strMoneda = $('#<%= ddl_Moneda.ClientID %>').val();
            if (strMoneda == "3" || strMoneda == "42" || strMoneda == "102") {

                var ctrl = document.getElementById('<%= ddl_Moneda.ClientID %>');
                ctrl.style.border = "1px solid #888888";

            }
            else {
                bolValida = false;
                var ctrl = document.getElementById('<%= ddl_Moneda.ClientID %>');
                ctrl.style.border = "1px solid Red";
            }

            if (ddlcontrolError(document.getElementById('<%= ctrlOficinaConsular.FindControl("ddlOficinaConsular").ClientID %>')) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_Distrito.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById('<%= txtFecServcio.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;

            var monto = document.getElementById("<%=txtMonto.ClientID %>").value;
            if (monto.length < 1) {
                document.getElementById("<%=txtMonto.ClientID %>").value = "0";
            }

            if (txtcontrolError(document.getElementById("<%=txtMonto.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=TxtNumBene.ClientID %>")) == false) bolValida = false;

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;

        }

        function ValidarPAHL() {

            var bolValida = true;
            var bolValidaHora = false;

            var ddlTipAyPAHL = document.getElementById("<%=ddl_TipAyPAHL.ClientID %>");

            var strHorIni = document.getElementById("<%=txtHoraInicio.ClientID %>").value;
            var strHorFin = document.getElementById("<%=txtHoraFin.ClientID %>").value;

            if (ddlcontrolError(document.getElementById("<%=ddlCircunscripcion.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_TipAyPAHL.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_Estado.ClientID %>")) == false) bolValida = false;

            var strMoneda = $('#<%= ddl_Moneda.ClientID %>').val();
//            if (strMoneda == "3" || strMoneda == "42" || strMoneda == "102") {

//                var ctrl = document.getElementById('<%= ddl_Moneda.ClientID %>');
//                ctrl.style.border = "1px solid #888888";

//            }
//            else {
//                bolValida = false;
//                var ctrl = document.getElementById('<%= ddl_Moneda.ClientID %>');
//                ctrl.style.border = "1px solid Red";
//            }

            if (ddlcontrolError(document.getElementById("<%=ddl_Distrito.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txtFecServcio.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtHoraInicio.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtHoraFin.ClientID %>")) == false) bolValida = false;

            var monto = document.getElementById("<%=txtMonto.ClientID %>").value;
            if (monto.length < 1) {
                document.getElementById("<%=txtMonto.ClientID %>").value = "0";
            }

            document.getElementById("<%=txtMonto.ClientID %>")

            if (txtcontrolError(document.getElementById("<%=txtMonto.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=TxtNumBene.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlFuncionario.ClientID %>")) == false) bolValida = false;

            if (strHorIni.length > 0 && strHorFin.length > 0) {

                bolValidaHora = bolValidaHora = ComparaHoras(strHorIni, strHorFin);
            }
            if (bolValidaHora) {
                bolValida = false;
            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            /*MENSAJE DE HORA*/
            if (bolValidaHora) {
                alert("Rango de horas Incorrecto");
            }

            return bolValida;
        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

        }

    </script>
</asp:Content>
