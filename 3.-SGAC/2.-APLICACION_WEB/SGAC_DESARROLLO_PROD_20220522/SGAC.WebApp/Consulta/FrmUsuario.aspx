<%@ Page Language="C#" UICulture="es-PE" Culture="es-PE" MasterPageFile="~/Site.Master"
    CodeBehind="FrmUsuario.aspx.cs" AutoEventWireup="true" Inherits="SGAC.WebApp.Configuracion.FrmUsuario" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="PageBar" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        var txtelemDniID = '<% =txtDNIMant.ClientID %>';

        var lblNombreMensaje = '<% =lblNombreMensaje.ClientID %>';
        var lblApePatMensaje = '<% =lblApePatMensaje.ClientID %>';
        var lblApeMatMensaje = '<% =lblApeMatMensaje.ClientID %>';

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

            $("#<%=txtDireccionIP.ClientID %>").on("blur", validarDireccionIP);
            $("#<%=txtCorreoMant.ClientID %>").on("blur", validarCorreoElectronico);

            //Previene el postback al hacer enter
            $(function () {
                $(':text').bind('keydown', function (e) {
                    if (e.target.className != "searchtextbox") {
                        if (e.keyCode == 13) { //if this is enter key
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

        function validarDni(txt, lbl) {
            var charpos = txt.value.search("[^A-Za-z]");

            if (txt.value.length > 0 && charpos > 0) {
                lbl.innerText = 'El DNI solo puede contener números.'
                txt.value = '';
            }
            else if (txt.value.length > 0 && txt.value.length != 8) {
                lbl.innerText = 'El DNI debe contener 8 caracteres.'
                txt.value = '';
            }
            else {
                lbl.innerText = '';
            }
        }

        function validarSoloLetras(lbl, txt) {

            var label = document.getElementById(lbl);

            var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ]");

            if (txt.value.length > 0 && charpos >= 0) {
                label.style.display = 'inline';
                txt.value = '';
            }
            else {
                label.style.display = 'none';
            }

        }
        
    </script>
    <div>
        <%-- Consulta --%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloUsuario" runat="server" Text="Usuarios"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>
        <%--Opciones--%>
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">Consulta</a></li>
                            <li><a href="#tab-2">Registro</a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsularConsulta" runat="server" Text="Oficina Consular :"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <uc1:ctrlOficinaConsular ID="ddlOficinaConsularConsulta" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDNIConsulta" runat="server" Text="DNI :" Width="50px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDNIConsulta" runat="server" Width="144px" MaxLength="8" onkeypress="return validatenumber(event)"
                                                    Style="margin-bottom: 0px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNombresConsulta" runat="server" Text="Nombres :"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtNombresConsulta" runat="server" Width="200px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblApePaterno" runat="server" Text="Primer Apellido :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApePaternoConsulta" runat="server" Width="200px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblApeMaterno" runat="server" Text="Segundo Apellido :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApeMaternoConsulta" runat="server" Width="200px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEstadoConsulta" runat="server" Text="Estado :"></asp:Label>
                                                <asp:CheckBox ID="chkEstadoConsulta" Text="Activo" runat="server" Checked="True" />
                                            </td>
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
                                    <%--Mensaje Validación--%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvUsuario" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvUsuario_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="usua_vDocumentoNumero" HeaderText="DNI">
                                                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usua_vApellidoPaterno" HeaderText="Primer Apellido">
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usua_vApellidoMaterno" HeaderText="Segundo Apellido">
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usua_vNombres" HeaderText="Nombres">
                                                            <ItemStyle Width="160px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usua_vAlias" HeaderText="Alias">
                                                            <ItemStyle Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usro_vOficinaConsular" HeaderText="Oficina Consular">
                                                            <ItemStyle Width="130px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usro_vRolConfiguracion" HeaderText="Rol">
                                                            <ItemStyle Width="160px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usua_cEstado" HeaderText="Estado">
                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
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
                                                <PageBar:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
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
                                            <td colspan="6">
                                                <Label:Validation ID="ctrlValidacionMant" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdMsje" colspan="7">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Ingresar campos requeridos (*)"
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEntidadMant" runat="server" Text="Entidad:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:DropDownList ID="ddlEntidadMant" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDNIMantenimiento" runat="server" Text="DNI:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtDNIMant" runat="server" Width="150px" MaxLength="8" onkeypress="return validatenumber(event)"
                                                    onblur="validarDni(this,lblDNIMantMensaje)" TabIndex="0" />
                                                <asp:Label ID="lblVal_txtDNIMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                &nbsp; <a id="lblDNIMantMensaje" style="color: Red; font-size: small;"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombresMant" runat="server" Text="Nombres:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtNombresMant" runat="server" Width="350px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" TabIndex="0" onblur="return validarSoloLetras(lblNombreMensaje , this)" />
                                                <asp:Label ID="lblVal_txtNombresMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblNombreMensaje" runat="server" Text="  Solo se permiten letras."
                                                    ForeColor="Red" Font-Size="Small" Style="display: none"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblApePaternoMant" runat="server" Text="Primer Apellido:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtApePaternoMant" runat="server" Width="350px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" TabIndex="0" onblur="return validarSoloLetras(lblApePatMensaje , this)" />
                                                <asp:Label ID="lblVal_txtApePaternoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblApePatMensaje" runat="server" Text="  Solo se permiten letras."
                                                    ForeColor="Red" Font-Size="Small" Style="display: none"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblApeMaternoMant" runat="server" Text="Segundo Apellido:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtApeMaternoMant" runat="server" Width="350px" CssClass="txtLetra"
                                                    MaxLength="50" onkeypress="return ValidarSujeto(event)" TabIndex="0" onblur="return validarSoloLetras(lblApeMatMensaje , this)" />
                                                <asp:Label ID="lblVal_txtApeMaternoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblApeMatMensaje" runat="server" Text="  Solo se permiten letras."
                                                    ForeColor="Red" Font-Size="Small" Style="display: none"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCorreoMant" runat="server" Text="Correo Electrónico:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtCorreoMant" runat="server" Width="350px" MaxLength="40" CssClass="txtLetra"
                                                    onkeypress="return validarCorreosElectronicos(event)" />
                                                <asp:Label ID="lblVal_txtCorreoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:Label ID="lblMailValidation" CssClass="validationLogin" runat="server" Text="Email no valido"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtTelefono" runat="server" Width="150px" MaxLength="30" onkeypress="return validarTelefonos(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDireccion" runat="server" Text="Dirección:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtDireccion" runat="server" Width="350px" CssClass="txtLetra" MaxLength="80"
                                                    onkeypress="return validarDirecciones(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCuentaRedMant" runat="server" Text="Cuenta de Red:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtCuentaRedMant" runat="server" Width="150px" CssClass="txtLetra"
                                                    MaxLength="15" onkeypress="return ValidarLetras(event)" />
                                                <asp:ImageButton ID="imgValidarUsuario" runat="server" ImageUrl="~/Images/img_16_validate_user.png"
                                                    ToolTip="Validar Usuario" OnClick="imgValidarUsuario_Click" Style="height: 16px;
                                                    width: 14px;" Visible="False" />
                                                <asp:Label ID="lblVal_txtCuentaRedMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblClaveMant" runat="server" Text="Clave:" Visible="false"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtClaveMant" runat="server" Width="350px" Visible="false" CssClass="txtLetra"
                                                    MaxLength="15" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFechaCaducidadMant" runat="server" Text="Fecha Caducidad:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <SGAC_Fecha:ctrlDate ID="dtpFecCaducidad" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDireccionIP" runat="server" Text="Dirección IP:"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtDireccionIP" runat="server" Width="155px" MaxLength="15" onkeypress="return validarDireccionesIP(event)" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIpValidation" CssClass="validationLogin" runat="server" Text="Ip no válida"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblActivoMant" runat="server" Text="Cuenta:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkActivoMant" runat="server" Checked="True" Text="" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblchkActivoMant" runat="server" Text="Activo"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblActivoBloqueo" runat="server" Text="Bloqueo:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkActivoBloqueo" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblchkActivoBloqueo" runat="server" Text="Activo"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblActivoSesion" runat="server" Text="Sesión:" ForeColor="Gray"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkActivoSesion" runat="server" Enabled="False" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblchkActivoSesion" runat="server" Text="Activo" ForeColor="Gray"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNotificaRemesa" runat="server" Text="Notificación Remesa: "></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkNotificaRemesa" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblChekNotficaRemesa" runat="server" Text="Activo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%--Configuracion--%>
                                        <tr>
                                            <td colspan="3">
                                                <h4>
                                                    Configuración
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsularConfig" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <uc1:ctrlOficinaConsular ID="CtrlOficinaConsularConfig" runat="server" Width="350px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblConfiguracion" runat="server" Text="Rol:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlRolConfiguracion" runat="server" Width="350px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlRolConfiguracion" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAcceso" runat="server" Text="Acceso:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlAcceso" runat="server" Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblCamposObligatorios" runat="server" Text="(*) Campos Obligatorios"
                                                    Font-Bold="true"></asp:Label>
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
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    </div>
    <br />
    <div id="MsjeDialog" style="display: none;" title="Seguridad">
        <p>
            Se ha cambiado la oficina consular del usuario. Para que pueda ver los cambios debera
            volver a iniciar session.</p>
    </div>
    <script language="javascript" type="text/javascript">

        $(function () {

            $('#tabs').tabs();

            $("#MsjeDialog").dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                },

                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });

        });

        function OpenWindowJQ() {
            $('#MsjeDialog').dialog('open');
        }

        function validarDireccionIP() {
            var strDireccionIP = $.trim($("#<%= txtDireccionIP.ClientID %>").val());

            if (ValidarIPaddress(strDireccionIP) == false) {
                $("#<%=txtDireccionIP.ClientID %>").css("border", "solid Red 1px");
                $("#<%=txtDireccionIP.ClientID %>").val('');
                $("#<%= lblIpValidation.ClientID %>").show().show().css({ "display": "block" });
            }
            else {
                $("#<%=txtDireccionIP.ClientID %>").css("border", "solid #888888 1px");
                $("#<%= lblIpValidation.ClientID %>").show().show().css({ "display": "none" });
            }
        }

        function validarCorreoElectronico() {
            var strCorreo = $.trim($("#<%= txtCorreoMant.ClientID %>").val());

            if (strCorreo.length == 0) {
                $("#<%=txtCorreoMant.ClientID %>").css("border", "solid #888888 1px");
            }
            else {
                if (ValidarEmail(strCorreo) == false) {
                    $("#<%=txtCorreoMant.ClientID %>").css("border", "solid Red 1px");
                    $("#<%=txtCorreoMant.ClientID %>").val('');
                    $("#<%= lblMailValidation.ClientID %>").show().show().css({ "display": "block" });
                }
                else {
                    $("#<%=txtCorreoMant.ClientID %>").css("border", "solid #888888 1px");
                    $("#<%= lblMailValidation.ClientID %>").show().show().css({ "display": "none" });
                }
            }
        }

        function ValidarCuentaRed() {

            var bolValida = true;

            var strAlias = $.trim($("#<%= txtCuentaRedMant.ClientID %>").val());

            if (strAlias.length == 0) {
                $("#<%=lblVal_txtCuentaRedMant.ClientID %>").show();
                bolValida = false;
            }
            else {
                $("#<%=lblVal_txtCuentaRedMant.ClientID %>").hide();
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
        }

        function Validar() {

            var bolValida = true;

            var strDNI = $.trim($("#<%= txtDNIMant.ClientID %>").val());
            var strNombres = $.trim($("#<%= txtNombresMant.ClientID %>").val());
            var strApePaterno = $.trim($("#<%= txtApePaternoMant.ClientID %>").val());
            var strApeMaterno = $.trim($("#<%= txtApeMaternoMant.ClientID %>").val());
            var strCorreo = $.trim($("#<%= txtCorreoMant.ClientID %>").val());
            var strAlias = $.trim($("#<%= txtCuentaRedMant.ClientID %>").val());
            var strRolConfig = $.trim($("#<%= ddlRolConfiguracion.ClientID %>").val());

            var txtDNIMant = document.getElementById('<%= txtDNIMant.ClientID %>');
            var txtNombresMant = document.getElementById('<%= txtNombresMant.ClientID %>');
            var txtApePaternoMant = document.getElementById('<%= txtApePaternoMant.ClientID %>');
            var txtApeMaternoMant = document.getElementById('<%= txtApeMaternoMant.ClientID %>');
            var txtCorreoMant = document.getElementById('<%= txtCorreoMant.ClientID %>');
            var txtCuentaRedMant = document.getElementById('<%= txtCuentaRedMant.ClientID %>');
            var ddlRolConfiguracion = document.getElementById('<%= ddlRolConfiguracion.ClientID %>');

            var strDireccionIP = $.trim($("#<%= txtDireccionIP.ClientID %>").val());

            if (strDNI.length == 0) {
                txtDNIMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtDNIMant.style.border = "1px solid #888888";
            }

            if (strNombres.length == 0) {
                txtNombresMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtNombresMant.style.border = "1px solid #888888";
            }

            if (strApePaterno.length == 0) {
                txtApePaternoMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtApePaternoMant.style.border = "1px solid #888888";
            }

            if (strApeMaterno.length == 0) {
                txtApeMaternoMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtApeMaternoMant.style.border = "1px solid #888888";
            }

            if (strCorreo.length == 0) {
                $("#<%=txtCorreoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                if (ValidarEmail(strCorreo) == false) {
                    $("#<%=txtCorreoMant.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtCorreoMant.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strAlias.length == 0) {
                txtCuentaRedMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtCuentaRedMant.style.border = "1px solid #888888";
            }

            if (strRolConfig == "0") {
                ddlRolConfiguracion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlRolConfiguracion.style.border = "1px solid #888888";
            }

            if (ValidarIPaddress(strDireccionIP) == false) {
                $("#<%=txtDireccionIP.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDireccionIP.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
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

        function ValidarEmail(email) {

            var ipformat = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (ipformat.test(email)) {
                return true;
            }
            else {
                return false;
            }
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
            if (charCode > 159 && charCode < 164) {
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

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
             
    </script>
</asp:Content>
