<%@ Page Language="C#" UICulture="es-PE" Culture="es-PE" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FrmOficinaConsular.aspx.cs" Inherits="SGAC.WebApp.Configuracion.OficinaConsular" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
  
    <style type="text/css">
        .style2
        {
            width: 190px;
        }
    </style>
  
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

            if ($("#<%= chkEsJefaturaMant.ClientID %>").attr("checked")) {
                $("#<%= ddlPorcentaje.ClientID %>").attr('enabled', 'enabled');
            }
            else {
                $("#<%= ddlPorcentaje.ClientID %>").attr('disabled', 'disabled');
                $("#<%= ddlPorcentaje.ClientID %>").val('0');
            }

            $("#<%=txtRangoIPInicio.ClientID %>").on("blur", validarDireccionIPInicio);
            $("#<%=txtRangoIPFin.ClientID %>").on("blur", validarDireccionIPFin);

            $(function () {
                $(':text').bind('keydown', function (e) {
                    if (e.target.className != "searchtextbox") {
                        if (e.keyCode == 13) {
                            e.preventDefault();
                            return false;
                        } else
                            return true;
                    } else
                        return true;
                });
            });

            $(function () {
                $('input:text:first').focus();
                var $inp = $('input:text');
                $inp.bind('keydown', function (e) {
                    var key = e.which;
                    if (key == 13) {
                        e.preventDefault();
                        var nxtIdx = $inp.index(this) + 1;
                        $(":input:text:eq(" + nxtIdx + ")").focus();
                    }
                });
            });
        }

        function DisableBtnNuevaMoneda() {
            document.getElementById("btnMonedaNueva").disabled = true;
        }

        function EnabledBtnNuevaMoneda() {
            document.getElementById("btnMonedaNueva").disabled = false;
        }

        function validarSoloLetras(lbl, txt) {

            var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ.]");

            if (txt.value.length > 0 && charpos >= 0) {
                lbl.style.display = 'inline';
                txt.value = '';
            }
            else {
                lbl.style.display = 'none';
            }
        }

        function validarSoloLetras2(txt) {

            var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ. -]");

            if (txt.value.length > 0 && charpos >= 0) {
                txt.value = '';
                alert('Error - No se pueden ingresar caracteres extraños.');
            }
        }

    </script>
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloOfConsular" runat="server" Text="Oficina Consular"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%--Cuerpo--%>
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
                                    <%-- Consulta --%>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Continente/Dpto.:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlContinente" runat="server" Width="300px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlContinente_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                             <td>
                                            </td>
                                            <td>
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text="País/Provincia:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPais" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                             <td>
                                            </td>
                                            <td>
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDescripcion" runat="server" Text="Nombre abreviado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescripcionConsulta" runat="server" Width="300px" MaxLength="150"
                                                    TabIndex="0" CssClass="txtLetra" onKeyPress="return isNombre(event)" onblur="return validarSoloLetras(lblDescripcionConsultaMensaje , this)" />
                                                <label id="lblDescripcionConsultaMensaje" style="color: Red; font-size: small; vertical-align: middle;
                                                    display: none">
                                                    Solo se permiten letras.</label>
                                            </td>
                                             <td>
                                            </td>
                                            <td>
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:150px">
                                                <asp:Label ID="lblCategoria" runat="server" Text="Categoría:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategoria" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                              
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td style="width:350px">
                                                 <asp:CheckBox ID="chkSelEsJefatura" runat="server" Text="Jefatura Consular"  /> 
                                                 &nbsp;&nbsp; &nbsp;&nbsp; 
                                                 <asp:CheckBox ID="chkSelElecciones" runat="server" Text="Elecciones" />                                                                                                   
                                            </td>
                                            <td style="width:100px">
                                                
                                            </td>
                                            <td>
                                            
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:" ></asp:Label>
                                            </td>
                                            <td>
                                            <asp:CheckBox ID="chkRolActivo" runat="server" Text="Activo" Checked="True" />
                                            </td>
                                            <td style="width: 50px">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                           
                                        </tr>
                                    </table>
                                    <%--Opciones--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gdvOficinaConsular" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvOficinaConsular_RowCommand">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ofco_sOficinaConsularId" HeaderText="ID" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vContinente" HeaderText="Continente">
                                                                    <ItemStyle Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ubge_vProvincia" HeaderText="País">
                                                                    <ItemStyle Width="180px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vCategoriaNombre" HeaderText="Categoría">
                                                                    <ItemStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ofco_vSiglas" HeaderText="Nombre abreviado">
                                                                    <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ofco_vJefatura" HeaderText="Jefatura">
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:BoundField>
                                                                 <asp:BoundField DataField="ofco_vElecciones" HeaderText="Elecciones">
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
                                            <td id="tdMsje">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Debe ingresar los campos requeridos (*)"
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                </asp:Label>
                                                <asp:HiddenField ID="hdn_sOficinaConsularId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_vDireccionIP" runat="server" Value="" />
                                                <asp:HiddenField ID="hdn_sUsuarioId" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Text="Continente/Dpto.:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Height="22px"
                                                    OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged" Width="200px" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text="Código de Local:" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCodigoLocal" runat="server" MaxLength="10" onKeyPress="return isNumero(event)"
                                                    TabIndex="36" Width="120px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text="País/Provincia:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlProvincia" runat="server" Height="22px" Width="200px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" TabIndex="2" Enabled="false">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant0" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="Distrito/Ciudad:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDistrito" runat="server" Width="200px" Height="22px"  Enabled="false"
                                                    TabIndex="3">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCategoriaMant" runat="server" Text="Categoría:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategoriaMant" runat="server" Width="300px" TabIndex="4">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMoneda" runat="server" Text="Moneda:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMoneda" runat="server" Width="300px" TabIndex="5">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                <input id="btnMonedaNueva" type="button" value="Nueva Moneda" style="width:110px;" class="btnGeneral" onclick="Popup();"/>
                                            </td>
                                            
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCajaChica" runat="server" Text="Caja Chica" CssClass="btnGeneral"
                                                    OnClick="btnCajaChica_Click" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombreMant" runat="server" Text="Nombre:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombreMant" runat="server" MaxLength="200" Width="350px" CssClass="txtLetra"
                                                    onKeyPress="return isNombre(event)" onblur="return validarSoloLetras2(this)"
                                                    TabIndex="6" />
                                                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Nombre Abreviado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombreAbrev" runat="server" Width="300px" MaxLength="30" CssClass="txtLetra"
                                                    onKeyPress="return isLetra(event)" 
                                                    onblur="return validarSoloLetras2(this)" TabIndex="7" />
                                                    <asp:Label ID="Label24" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDireccionMant" runat="server" Text="Dirección:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDireccionMant" runat="server" Width="350px" MaxLength="200" CssClass="txtLetra"
                                                    TabIndex="8" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label23" runat="server" Text="Latitud y Longitud:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLatitud" runat="server" Width="175px" MaxLength="100" CssClass="txtLetra"
                                                    TabIndex="8" placeholder="Latitud"/>
                                                <asp:TextBox ID="txtLongitud" runat="server" Width="175px" MaxLength="100" CssClass="txtLetra"
                                                    TabIndex="8" placeholder="Longitud"/>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTelefonoMant" runat="server" Text="Teléfono:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelefonoMant" runat="server" Width="350px" MaxLength="100" onKeyPress="return isNumeroTelefono(event)"
                                                    TabIndex="9" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Horario Atención:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk01" Text="01" runat="server" TabIndex="10" />
                                                <asp:CheckBox ID="chk02" Text="02" runat="server" TabIndex="11" />
                                                <asp:CheckBox ID="chk03" Text="03" runat="server" TabIndex="12" />
                                                <asp:CheckBox ID="chk04" Text="04" runat="server" TabIndex="13" />
                                                <asp:CheckBox ID="chk05" Text="05" runat="server" TabIndex="14" />
                                                <asp:CheckBox ID="chk06" Text="06" runat="server" TabIndex="15" />
                                                <asp:CheckBox ID="chk07" Text="07" runat="server" TabIndex="16" />
                                                <asp:CheckBox ID="chk08" Text="08" runat="server" TabIndex="17" />
                                                <asp:CheckBox ID="chk09" Text="09" runat="server" TabIndex="18" />
                                                <asp:CheckBox ID="chk10" Text="10" runat="server" TabIndex="19" />
                                                <asp:CheckBox ID="chk11" Text="11" runat="server" TabIndex="20" />
                                                <asp:CheckBox ID="chk12" Text="12" runat="server" TabIndex="21" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk13" Text="13" runat="server" TabIndex="22" />
                                                <asp:CheckBox ID="chk14" Text="14" runat="server" TabIndex="23" />
                                                <asp:CheckBox ID="chk15" Text="15" runat="server" TabIndex="24" />
                                                <asp:CheckBox ID="chk16" Text="16" runat="server" TabIndex="25" />
                                                <asp:CheckBox ID="chk17" Text="17" runat="server" TabIndex="26" />
                                                <asp:CheckBox ID="chk18" Text="18" runat="server" TabIndex="27" />
                                                <asp:CheckBox ID="chk19" Text="19" runat="server" TabIndex="28" />
                                                <asp:CheckBox ID="chk20" Text="20" runat="server" TabIndex="29" />
                                                <asp:CheckBox ID="chk21" Text="21" runat="server" TabIndex="30" />
                                                <asp:CheckBox ID="chk22" Text="22" runat="server" TabIndex="31" />
                                                <asp:CheckBox ID="chk23" Text="23" runat="server" TabIndex="32" />
                                                <asp:CheckBox ID="chk24" Text="24" runat="server" TabIndex="33" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDiferenciaHorarioMant" runat="server" Text="Diferencia Horaria:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlZonaHoraria" runat="server" Width="450px" TabIndex="34">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlDiferenciaHoraria" runat="server" Visible="False">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDiferenciaHorarioMant0" runat="server" Text="Sitio Web:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSitioWeb" runat="server" MaxLength="200" Width="350px" CssClass="txtLetra"
                                                    onKeyPress="return isURL(event)" TabIndex="35" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDiferenciaHorarioMant1" runat="server" Text="Rango IP Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRangoIPInicio" runat="server" MaxLength="15" onKeyPress="return isNumeroDecimal(event)"
                                                    Width="120px" TabIndex="36" />
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblIpValidationIni" CssClass="validationLogin" runat="server" Text="Ip no válida"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDiferenciaHorarioMant2" runat="server" Text="Rango IP Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRangoIPFin" runat="server" MaxLength="15" onKeyPress="return isNumeroDecimal(event)"
                                                    Width="120px" TabIndex="37" />
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblIpValidationFin" CssClass="validationLogin" runat="server" Text="Ip no válida"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkTieneASN" runat="server" Text="Realiza Asistencia al Nacional"
                                                    TabIndex="38"  />
                                            </td>
                                            <td colspan="2">
                                             <asp:CheckBox ID="chkRemesaLimaMant" runat="server" Text="Remesa Directa a Lima"
                                                    TabIndex="40" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkEsJefaturaMant" runat="server" Text="Jefatura Consular" TabIndex="39"
                                                    AutoPostBack="True" OnCheckedChanged="chkEsJefaturaMant_CheckedChanged" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkElecciones" runat="server" Text="Elecciones" TabIndex="40"  />                                                        
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td>
                                               <asp:Label ID="lblEstadoMant" runat="server" Text="Estado:" ></asp:Label> 
                                            </td>
                                            <td>
                                            <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Porcentaje Aplica TCC:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPorcentaje" runat="server" Width="100px" CssClass="campoNumero"
                                                    TabIndex="41">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Solicita Pedidos a:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOficinaConsularPedido" runat="server" TabIndex="42"
                                                    Width="450px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label19" runat="server" Text="Dependiente de:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOficinaDependiente" runat="server" TabIndex="42"
                                                    Width="450px">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label21" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <h3>
                                                    <asp:Label ID="Label5" runat="server" Text="Oficina Consular Dependientes"></asp:Label></h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvDependientes" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                    GridLines="None" OnRowCommand="gdvDependientes_RowCommand" ShowHeaderWhenEmpty="True"
                                                    SelectedRowStyle-CssClass="slt" Width="100%">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ofco_sOficinaConsularId" HeaderText="Id">
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ofco_vNombre" HeaderText="Nombre">
                                                            <HeaderStyle Width="600px" />
                                                            <ItemStyle Width="600px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Names="Arial" Font-Size="11px" />
                                                    <EmptyDataTemplate>
                                                        <table id="tbSinDatosOFCD">
                                                            <tbody>
                                                                <tr>
                                                                    <td width="10%">
                                                                        <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
                                                                    </td>
                                                                    <td width="5%">
                                                                    </td>
                                                                    <td width="85%">
                                                                        <asp:Label ID="lblSinDatosOFCD" runat="server" Text="Sin Datos..."></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                                <div>
                                                    <PageBarContent:PageBar ID="ctrlPaginadorDep" runat="server" OnClick="ctrlPaginadorDep_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="80%">
                                        <tr>
                                            <td colspan="4">
                                                <h3>
                                                    <asp:Label ID="Funcionarios" runat="server" Text="Funcionarios"></asp:Label></h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdMsge2" colspan="4">
                                                <asp:Label ID="lblMsjeValFunc" runat="server" Text="Debe ingresar los campos requeridos (*)"
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="Funcionario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBuscarFunc" runat="server" Text="Buscar" CssClass="btnGeneral"
                                                    OnClick="btnBuscarFunc_Click" />
                                            </td>
                                            <td class="style2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                            </td>
                                            <asp:Button ID="btnEjecutarFuncionario" runat="server" Text="Aceptar" Style="display: none;"
                                                OnClick="btnEjecutarFuncionario_Click" />
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:GridView ID="gdvFuncionario" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                    GridLines="None" OnRowCommand="gdvFuncionario_RowCommand" ShowHeaderWhenEmpty="True"
                                                    SelectedRowStyle-CssClass="slt" Width="100%">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ocfu_sOfiConFuncionarioId" 
                                                            HeaderText="ocfu_sOfiConFuncionarioId" Visible="False" />
                                                        <asp:BoundField DataField="IFuncionarioId" HeaderText="Id">
                                                            <HeaderStyle Width="50px" />
                                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ocfu_vApellidoPaternoFuncionario" HeaderText="Primer Apellido">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ocfu_vApellidoMaternoFuncionario" HeaderText="Segundo Apellido">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ocfu_vNombreFuncionario" HeaderText="Nombres">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ocfu_cEstado" HeaderText="Estado" HeaderStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CARGO" HeaderText="Cargo">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sGenero" HeaderText="Género" HeaderStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle HorizontalAlign="Center" CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditarCargo" CommandName="EditarCargo" ToolTip="Editar Cargo"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Names="Arial" Font-Size="11px" />
                                                    <EmptyDataTemplate>
                                                        <table id="tbSinDatosFunc">
                                                            <tbody>
                                                                <tr>
                                                                    <td width="10%">
                                                                        <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
                                                                    </td>
                                                                    <td width="5%">
                                                                    </td>
                                                                    <td width="85%">
                                                                        <asp:Label ID="lblSinDatosFunc" runat="server" Text="Sin Datos..."></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="ocultarFuncionario" runat="server" visible="false" style="border: 1px solid #cdcdcd;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFuncionario" runat="server" Font-Bold="True"></asp:Label>
                                                    <asp:HiddenField ID="hCodID" runat="server" />
                                                </td>
                                                <td>
                                                    <div style="margin: 20px; margin-right:20px;">
                                                        <asp:Label ID="Label12" runat="server" Text=" CARGO: " Font-Bold="True"></asp:Label>
                                                        <asp:DropDownList ID="ddlCargos" runat="server" Width="250px">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnActualizarCargo" runat="server" Text="    Actualizar" 
                                                            CssClass="btnSave" Width="100px" onclick="btnActualizarCargo_Click"/>    
                                                    </div>
                                                    
                                                    <div style="margin: 20px; margin-right:20px;">
                                                        <asp:Label ID="Label13" runat="server" Text="  SEXO : " Font-Bold="True"></asp:Label>
                                                        &nbsp;&nbsp;<asp:DropDownList ID="ddlSexo" runat="server" Width="250px">
                                                        <asp:ListItem Value="0">- SELECCIONAR -</asp:ListItem>
                                                        <asp:ListItem Value="1">MASCULINO</asp:ListItem>
                                                        <asp:ListItem Value="2">FEMENINO</asp:ListItem>
                                                        </asp:DropDownList>    
                                                    </div>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>


                                <div id="simpleModal_1" class="modal">
                                <div class="modal-window" style="min-height: 30%; max-height: 100%;">
                                    <div class="modal-titulo" style="margin: 0px 0px 15px 0px;">
                                        <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                                            OnClientClick="cerrarPopup(); return false" runat="server" />
                                        <span>MONEDA</span>
                                    </div>
                                    <div class="modal-cuerpoNormal" style="border: 1px #dcdcdc solid; padding: 5px 5px 50px 5px;
                                        margin-left: 10px; margin-right: 10px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label14" runat="server" Text="Moneda"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMonedaPopup" runat="server" Width="220px">
                                                                        </asp:DropDownList>
                                                    <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label20" runat="server" Text="Fecha Vigencia Inicio:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="ctrFechaInicio" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label22" runat="server" Text="Fecha Vigencia Fin:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="ctrFechaFin" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <hr />
                                        <table style="height:20%">
                                            <tr>
                                                <td style="width: 45%">
                           
                                                </td>
                                                <td align="center" valign="top">
                                                    <asp:Button ID="btnGrabar" CssClass="btnSave" Width="140px" runat="server" 
                                                        Text="  Grabar Moneda" OnClientClick="return ValidarMoneda();" 
                                                        onclick="btnGrabar_Click"/>
                                                    <asp:Button ID="btnSalir" CssClass="btnExit" Width="100px" runat="server" Text="   Salir" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <Label:Validation ID="ctrlValidacionRegistro" runat="server" />
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div style="width: 10px; height: 10px; background: #6EC8D0; margin-right:5px; float:left;" ></div>
                                                    <span> Moneda Vigente </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="modal-pie" style="margin-bottom: 30px; min-height: 30%; max-height: 100%;
                                        display: inline-block;">
                                        <div style="padding: 0px 10px 10px 10px; background: #ffffff;">
                                            <asp:GridView ID="gdvMonedaPop" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                OnRowCommand="gdvMoneda_RowCommand" OnRowDataBound="gdvMoneda_RowDataBound"
                                                GridLines="None" PageSize="9000" SelectedRowStyle-CssClass="slt">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="OFConsular" HeaderText="Oficina Consular">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio de Vigencia">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin de Vigencia">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Vigente" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" /> 
                                                    <asp:TemplateField HeaderText="Editar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditarMoneda" CommandName="EditarMoneda" ToolTip="Editar Moneda" 
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                CommandName="EliminarMoneda" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar" OnClientClick="if(!ConfirmarEliminar()) return false;" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDOficinaMoneda" runat="server" Text='<%# Bind("ofmo_sOficinaMonedaId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMoneda" runat="server" Text='<%# Bind("Moneda") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Bind("FechaInicio") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaFin" runat="server" Text='<%# Bind("FechaFin") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                        </div>
                                            <asp:HiddenField ID="hidEditar" Value="0" runat="server" />
                                    </div>
                                    </div>
                                </div>
                            </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    </div>



    


    <br />
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });
        function Popup() {
            document.getElementById('simpleModal_1').style.display = 'block';
        }
        function cerrarPopup() {
            document.getElementById('simpleModal_1').style.display = 'none';
        }
        function ConfirmarEliminar() {
            var r = confirm("Esta seguro de eliminar el registro");
            if (r == true) {
                return true;
            } else {
                return false;
            }
        }
        function HabilitaComboPorcentaje(iEnabled) {
            if (iEnabled == 1) {
                $("#<%= ddlPorcentaje.ClientID %>").attr('enabled', 'enabled');
            }
            else if (iEnabled == 0) {
                $("#<%= ddlPorcentaje.ClientID %>").attr('disabled', 'disabled');
                $("#<%= ddlPorcentaje.ClientID %>").val('0');
            }
        }
        function ValidarMoneda() {
            var bolValida = true;
            var strMoneda = $.trim($("#<%= ddlMonedaPopup.ClientID %>").val());
            var strFechaFin = document.getElementById('<%= ctrFechaFin.FindControl("TxtFecha").ClientID %>').value;
            var strFichaIni = document.getElementById('<%= ctrFechaInicio.FindControl("TxtFecha").ClientID %>').value;

            if (strMoneda == '0') {
                $("#<%=ddlMonedaPopup.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlMonedaPopup.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strFechaFin.length == 0) {
                $("#<%=ctrFechaFin.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ctrFechaFin.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strFichaIni.length == 0) {
                $("#<%=ctrFechaInicio.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ctrFechaInicio.ClientID %>").css("border", "solid #888888 1px");
            }

            if (!bolValida) {
                alert('Ingrese todo los campos para poder realizar el registro de la moneda');
            }
            return bolValida;
        }


        function Validar() {

            var bolValida = true;

            var strDepartamento = $.trim($("#<%= ddlDepartamento.ClientID %>").val());
            var strProvincia = $.trim($("#<%= ddlProvincia.ClientID %>").val());
            var strDistrito = $.trim($("#<%= ddlDistrito.ClientID %>").val());
            var strCategoriaMant = $.trim($("#<%= ddlCategoriaMant.ClientID %>").val());
            var strMoneda = $.trim($("#<%= ddlMoneda.ClientID %>").val());
            var strNombreMant = $.trim($("#<%= txtNombreMant.ClientID %>").val());
            var strPorcentaje = $.trim($("#<%= ddlPorcentaje.ClientID %>").val());

            var strZonaHoraria = $.trim($("#<%= ddlZonaHoraria.ClientID %>").val());

            var strDireccionIPIni = $.trim($("#<%= txtRangoIPInicio.ClientID %>").val());
            var strDireccionIPFin = $.trim($("#<%= txtRangoIPFin.ClientID %>").val());
            var strOficCD = $.trim($("#<%= ddlOficinaConsularPedido.ClientID %>").val());

            var strNombreAbreviado = $.trim($("#<%= txtNombreAbrev.ClientID %>").val()); 

            if (strDepartamento == '0') {
                $("#<%=ddlDepartamento.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlDepartamento.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strProvincia == '0') {
                $("#<%=ddlProvincia.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlProvincia.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDistrito == '0') {
                $("#<%=ddlDistrito.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlDistrito.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strCategoriaMant == '0') {
                $("#<%=ddlCategoriaMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlCategoriaMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strMoneda == '0') {
                $("#<%=ddlMoneda.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlMoneda.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombreMant.length == 0) {
                $("#<%=txtNombreMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombreMant.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strNombreAbreviado.length == 0) {
                $("#<%=txtNombreAbrev.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombreAbrev.ClientID %>").css("border", "solid #888888 1px");
            }

            

            if (strZonaHoraria == '0') {
                $("#<%=ddlZonaHoraria.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlZonaHoraria.ClientID %>").css("border", "solid #888888 1px");
            }

            if (ValidarIPaddress(strDireccionIPIni) == false) {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid #888888 1px");
            }

            if (ValidarIPaddress(strDireccionIPFin) == false) {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strOficCD == '0') {
                $("#<%=ddlOficinaConsularPedido.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlOficinaConsularPedido.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
        }

        function validarFuncionario() {

            var bolValida = true;

            var strDepartamento = $.trim($("#<%= ddlDepartamento.ClientID %>").val());
            var strProvincia = $.trim($("#<%= ddlProvincia.ClientID %>").val());
            var strDistrito = $.trim($("#<%= ddlDistrito.ClientID %>").val());
            var strCategoriaMant = $.trim($("#<%= ddlCategoriaMant.ClientID %>").val());
            var strMoneda = $.trim($("#<%= ddlMoneda.ClientID %>").val());
            var strNombreMant = $.trim($("#<%= txtNombreMant.ClientID %>").val());
            var strPorcentaje = $.trim($("#<%= ddlPorcentaje.ClientID %>").val());
            var strDireccionIPIni = $.trim($("#<%= txtRangoIPInicio.ClientID %>").val());
            var strDireccionIPFin = $.trim($("#<%= txtRangoIPFin.ClientID %>").val());
            var strOficCD = $.trim($("#<%= ddlOficinaConsularPedido.ClientID %>").val());

            if (strDepartamento == '0') {
                $("#<%=ddlDepartamento.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlDepartamento.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strProvincia == '0') {
                $("#<%=ddlProvincia.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlProvincia.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDistrito == '0') {
                $("#<%=ddlDistrito.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlDistrito.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strCategoriaMant == '0') {
                $("#<%=ddlCategoriaMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlCategoriaMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strMoneda == '0') {
                $("#<%=ddlMoneda.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlMoneda.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombreMant.length == 0) {
                $("#<%=txtNombreMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombreMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (ValidarIPaddress(strDireccionIPIni) == false) {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid #888888 1px");
            }

            if (ValidarIPaddress(strDireccionIPFin) == false) {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strOficCD == '0') {
                $("#<%=ddlOficinaConsularPedido.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlOficinaConsularPedido.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                $("#<%= lblMsjeValFunc.ClientID %>").hide();
            }
            else {
                $("#<%= lblMsjeValFunc.ClientID %>").show();
            }

            return bolValida;
        }

        function validarDireccionIPInicio() {
            var strDireccionIP = $.trim($("#<%= txtRangoIPInicio.ClientID %>").val());

            if (ValidarIPaddress(strDireccionIP) == false) {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid Red 1px");
                $("#<%=txtRangoIPInicio.ClientID %>").val('');
                $("#<%=lblIpValidationIni.ClientID %>").show().show().css({ "display": "block" });
            }
            else {
                $("#<%=txtRangoIPInicio.ClientID %>").css("border", "solid #888888 1px");
                $("#<%=lblIpValidationIni.ClientID %>").show().show().css({ "display": "none" });
            }
        }

        function validarDireccionIPFin() {
            var strDireccionIP = $.trim($("#<%= txtRangoIPFin.ClientID %>").val());

            if (ValidarIPaddress(strDireccionIP) == false) {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid Red 1px");
                $("#<%=txtRangoIPFin.ClientID %>").val('');
                $("#<%=lblIpValidationFin.ClientID %>").show().show().css({ "display": "block" });
            }
            else {
                $("#<%=txtRangoIPFin.ClientID %>").css("border", "solid #888888 1px");
                $("#<%=lblIpValidationFin.ClientID %>").show().show().css({ "display": "none" });
            }
        }

        function ValidarFuncionario(strFuncionario) {

            var valor;

            var rprta = true;

            $("#<%= gdvFuncionario.ClientID %> tbody tr").each(function (index) {

                $(this).children("td").each(function (index2) {

                    if (index2 == 0) {

                        valor = $(this).text();

                        if (valor == strFuncionario) {
                            rprta = false;
                        }
                    }
                })
            })

            return rprta;
        }

        function isNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }

            if (charCode > 47 && charCode < 58) {
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

        function isNombre(evt) {
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
            var letras = "áéíóúÑÁÉÍÓÚ.";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isLetra(evt) {
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
            var letras = "aeiouÑAEIOU-";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isURL(evt) {
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
            var letras = "0123456789aeiouÑAEIOU_-/:.";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isNumeroTelefono(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 32) {
                letra = true;
            }

            if (charCode > 47 && charCode < 58) {
                letra = true;
            }

            var letras = "-";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function isNumeroDecimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 32) {
                letra = true;
            }

            if (charCode > 47 && charCode < 58) {
                letra = true;
            }

            var letras = "-.";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function ValidarIPaddress(ipaddress) {

            if (ipaddress.length == 0) {
                return true;
            }

            if (ipaddress == "0.0.0.0") {
                return false;
            }

            if (ipaddress == "255.255.255.255") {
                return false;
            }

            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;

            if (ipformat.test(ipaddress)) {
                return true;
            }
            else {
                return false;
            }
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

       
    </script>
</asp:Content>
