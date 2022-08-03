<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="SupervisorRegistroPrevio.aspx.cs" Inherits="SolCARDIP.Paginas.Supervisor.SupervisorRegistroPrevio" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";
        var privilegios = 0;

        function copiarDatos() {
            var txtApePat = document.getElementById(master + "txtApePat");
            var txtApeMat = document.getElementById(master + "txtApeMat");
            var txtNombres = document.getElementById(master + "txtNombres");

            var txtSolApePat = document.getElementById(master + "txtSolApePat");
            var txtSolApeMat = document.getElementById(master + "txtSolApeMat");
            var txtSolNombres = document.getElementById(master + "txtSolNombres");

            if (txtApePat != null & txtApeMat != null & txtNombres != null & txtSolApePat != null & txtSolApeMat != null & txtSolNombres != null) {
                txtSolApePat.value = txtApePat.value;
                txtSolApeMat.value = txtApeMat.value;
                txtSolNombres.value = txtNombres.value;
            }
        }

        function mostrarFechaPrivilegios() {
            var dropDownTipoEntrada = document.getElementById(master + "ddlTipoEntrada");
            var txtFechaCon = document.getElementById(master + "txtFechaConsulares");
            var txtFechaPri = document.getElementById(master + "txtFechaPrivilegios");
            var trPri = document.getElementById(master + "trPrivilegios");
            if (dropDownTipoEntrada != null & trPri != null) {
                var dato = dropDownTipoEntrada.options[dropDownTipoEntrada.selectedIndex].text
                if (dato == "PRIVILEGIOS") {
                    trPri.style.display = "table-row";
                    privilegios = 1;
                }
                else {
                    txtFechaPri.value = "";
                    trPri.style.display = "none";
                    privilegios = 0;
                }
            }
        }

        function validarRegistrarCarne() {
            var txtApePat = document.getElementById(master + "txtApePat");
            var txtApeMat = document.getElementById(master + "txtApeMat");
            var txtNombres = document.getElementById(master + "txtNombres");
            var dropDownGenero = document.getElementById(master + "ddlSexo");
            var dropDownTipoCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownOficinaConsularEx = document.getElementById(master + "ddlMision");
            var dropDownTipoEntrada = document.getElementById(master + "ddlTipoEntrada");

            var dropDownSolTipoDocIdent = document.getElementById(master + "ddlSolDocumentoIdent");
            var txtSolNumeroDocIdent = document.getElementById(master + "txtSolNumeroDocIdent");
            var txtSolApePat = document.getElementById(master + "txtSolApePat");
            var txtSolApeMat = document.getElementById(master + "txtSolApeMat");
            var txtSolNombres = document.getElementById(master + "txtSolNombres");
            var txtSolTelefono = document.getElementById(master + "txtSolTelefono");

            var txtFechaCon = document.getElementById(master + "txtFechaConsulares");
            var txtFechaPri = document.getElementById(master + "txtFechaPrivilegios");

            if (txtApePat != null & txtApeMat != null & txtNombres != null & dropDownGenero != null & dropDownTipoCalMig != null & dropDownOficinaConsularEx != null & dropDownTipoEntrada != null &
                txtFechaCon != null & txtFechaPri != null &
                dropDownSolTipoDocIdent != null & txtSolNumeroDocIdent != null & txtSolApePat != null & txtSolNombres != null & txtSolTelefono != null) {
                if (txtApePat.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO DE LA PERSONA."); txtApePat.focus(); return false; }
                if (txtNombres.value == "") { alert("DEBE INGRESAR UN NOMBRE DE LA PERSONA."); txtNombres.focus(); return false; }
                //if (dropDownGenero.value == "0") { alert("DEBE SELECCIONAR EL GENERO (SEXO) DE LA PERSONA."); dropDownGenero.focus(); return false; }
                if (dropDownOficinaConsularEx.value == "0") { alert("DEBE SELECCIONAR UNA INSTITUCIÓN."); dropDownOficinaConsularEx.focus(); return false; }
                if (dropDownTipoEntrada.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE ENTRADA."); dropDownTipoEntrada.focus(); return false; }
                //if (dropDownTipoCalMig.value == "0") { alert("DEBE SELECCIONAR UNA CALIDAD MIGRATORIA."); dropDownTipoCalMig.focus(); return false; }
                // VALIDAR FECHAS
                var fechaCon = txtFechaCon.value;
                var fechaPri = txtFechaPri.value;
                if (txtFechaCon.value == "") { alert("DEBE INGRESAR LA FECHA DE RECEPCIÓN EN CONSULARES."); txtFechaCon.focus(); return false; }
                if (!validaFechaDDMMAAAA(fechaCon)) { alert("FECHA NO VALIDA. REVISE."); txtFechaCon.focus(); return false; };
                if (!compararFechaMayorActual(fechaCon)) {alert("FECHA DE RECEPCION NO DEBE SER MAYOR A LA FECHA ACTUAL.");txtFechaCon.focus(); return false;}
                if (privilegios == 1) {
                    if (txtFechaPri.value == "") { alert("DEBE INGRESAR LA FECHA DE RECEPCIÓN EN PRIVILEGIOS."); txtFechaPri.focus(); return false; }
                    if (!validaFechaDDMMAAAA(fechaPri)) { alert("FECHA NO VALIDA. REVISE."); txtFechaPri.focus(); return false; };
                    if (!compararFechaMayorActual(fechaPri)) { alert("FECHA DE RECEPCION NO DEBE SER MAYOR A LA FECHA ACTUAL."); txtFechaPri.focus(); return false; }
                    // COMPARA FECHAS
//                    var fCon = new Date(fechaCon)
//                    var fPri = new Date(fechaPri)
                    var resultadoFechas = compararFechas(fechaPri, fechaCon); //fCon.getTime() < fPri.getTime();
                    if (!resultadoFechas) { alert("FECHA DE RECEPCION DE CONSULARES NO DEBE SER MENOR A LA DE PRIVILEGIOS."); txtFechaPri.focus(); return false; }
                }
                if (dropDownSolTipoDocIdent.value != "0") {
                    //if (dropDownSolTipoDocIdent.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE ENTRADA."); dropDownTipoEntrada.focus(); return false; }
                    if (txtSolNumeroDocIdent.value == "") { alert("DEBE INGRESAR EL NUMERO DE IDENTIFICACION DEL SOLICITANTE."); txtSolNumeroDocIdent.focus(); return false; }
                    if (txtSolApePat.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO DEL SOLICITANTE."); txtSolApePat.focus(); return false; }
                    if (txtSolNombres.value == "") { alert("DEBE INGRESAR UN NOMBRE DEL SOLICITANTE."); txtSolNombres.focus(); return false; }
                }
            }
            else {
                return false;
            }

            if (confirm("¿ DESEA REGISTRAR LA SOLICITUD ?")) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <table class="AnchoTotal">
                <tr>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td style="width:45%;">
                        <fieldset class="Fieldset">
                            <legend class="FieldsetLeyenda">Registro Previo de Solicitudes</legend>
                            <table runat="server" id="tablaRegistro" style="width:100%;">
                                <tr>
                                    <td class="etiqueta" style="width:20%;">N° de Solicitud</td>
                                    <td style="width:40%;">
                                        <asp:Label runat="server" ID="lblIdentiSolicitud" CssClass="labelInfoFile" Font-Size="Large" Text="[ No Generado ]"></asp:Label>
                                    </td>
                                    <td rowspan="9" align="center">
                                        <img src="../../Imagenes/Iconos/user3.png" width="32" height="32" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" style="width:30%;">Primer Apellido</td>
                                    <td style="color:Red;width:60%;">
                                        <asp:TextBox runat="server" ID="txtApePat" CssClass="textbox" Width="70%" MaxLength="100"></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtApePat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Segundo Apellido</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtApeMat" CssClass="textbox" Width="70%" MaxLength="100" Text=""></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtApeMat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Nombres</td>
                                    <td style="color:Red;">
                                        <asp:TextBox runat="server" ID="txtNombres" CssClass="textbox" Width="70%" MaxLength="100" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNombres" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Sexo</td>
                                    <td style="color:Red">
                                        <asp:DropDownList runat="server" ID="ddlSexo" CssClass="dropdownlist" Width="40%" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Calidad Migratoria</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaPri" CssClass="dropdownlist" Width="70%" OnSelectedIndexChanged="calidadHumanitaria" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Categoría Institución</td>
                                    <td style="color:Red;">
                                        <asp:DropDownList runat="server" ID="ddlCategoriaOfcoEx" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="seleccionarCategoriaOficina" Width="50%"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Institución</td>
                                    <td style="color:Red;">
                                        <asp:DropDownList runat="server" ID="ddlMision" CssClass="dropdownlist" AutoPostBack="false" Width="90%" Enabled="false"></asp:DropDownList>&nbsp;&nbsp;&nbsp;*
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" style="width:25%;">Tipo Entrada</td>
                                    <td style="color:Red;">
                                        <asp:DropDownList runat="server" ID="ddlTipoEntrada" onchange="mostrarFechaPrivilegios();" CssClass="dropdownlist" Width="60%"></asp:DropDownList>&nbsp;&nbsp;&nbsp;*
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" style="width:25%;">Fecha de Recep. Consulares</td>
                                    <td style="color:Red;">
                                        <asp:TextBox runat="server" ID="txtFechaConsulares" CssClass="textbox" Width="25%" MaxLength="10" Text=""></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaConsulares" PopupButtonID="ibtFechaConsulares" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                        <asp:ImageButton ID="ibtFechaConsulares" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />&nbsp;&nbsp;&nbsp;*
                                    </td>
                                </tr>
                                <tr runat="server" id="trPrivilegios" style="display:none;">
                                    <td class="etiqueta" style="width:25%;">Fecha de Recep. Privilegios</td>
                                    <td style="color:Red;">
                                        <asp:TextBox runat="server" ID="txtFechaPrivilegios" CssClass="textbox" Width="25%" MaxLength="10" Text=""></asp:TextBox>
                                        <cc1:CalendarExtender ID="calendarEmision" runat="server" TargetControlID="txtFechaPrivilegios" PopupButtonID="ibtFechaPrivilegios" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                        <asp:ImageButton ID="ibtFechaPrivilegios" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />&nbsp;&nbsp;&nbsp;*
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" colspan="4" align="center" style="text-align:center;padding:10px 0px 10px 0px">Los datos con asterisco (*) de color rojo son datos obligatorios</td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" colspan="4" align="center" style="text-align:center;padding:10px 0px 10px 0px">
                                        <asp:Label runat="server" ID="lblNoEditar" Text="" CssClass="labelInfo" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="vertical-align:top;width:45%;">
                        <fieldset class="Fieldset">
                            <legend class="FieldsetLeyenda">Datos del Solicitante</legend>
                            <table class="AnchoTotal" runat="server" id="tablaRegistroSol" >
                                <tr>
                                    <td class="etiqueta" style="width:25%;">Documento de Identidad</td>
                                    <td class="etiqueta">:</td>
                                    <td colspan="2">
                                        <table class="AnchoTotal">
                                            <tr>
                                                <td style="width:40%;color:Red;">
                                                    <asp:DropDownList runat="server" ID="ddlSolDocumentoIdent" CssClass="dropdownlist" AutoPostBack="false" Width="90%"></asp:DropDownList>
                                                </td>
                                                <td style="width:7%;text-align:right;" class="etiqueta">Numero</td>
                                                <td class="etiqueta"style="width:2%;">:</td>
                                                <td style="width:35%;color:Red;">
                                                    <asp:TextBox runat="server" ID="txtSolNumeroDocIdent" CssClass="textbox" MaxLength="20" Width="50%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtSolNumeroDocIdent" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/-.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Imagenes/Iconos/buscarpersona.png" OnClick="obtenerSolicitante" style="height: 15px" ToolTip="Busca un solicitante" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Imagenes/Iconos/copy.png" style="height: 15px" OnClientClick="copiarDatos();return false;" ToolTip="Copiar datos del Titular" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Imagenes/Iconos/clear.png" style="height: 15px" OnClick="limpiarSolicitante" ToolTip="Limpiar Campos" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Primer Apellido</td>
                                    <td class="etiqueta">:</td>
                                    <td style="color:Red;" class="style2">
                                        <asp:TextBox runat="server" ID="txtSolApePat" CssClass="textbox" Width="90%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtSolApePat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Segundo Apellido</td>
                                    <td class="etiqueta">:</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSolApeMat" CssClass="textbox" Width="90%" MaxLength="100" Text=""></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtSolApeMat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Nombres</td>
                                    <td class="etiqueta">:</td>
                                    <td style="color:Red;">
                                        <asp:TextBox runat="server" ID="txtSolNombres" CssClass="textbox" Width="90%" MaxLength="100" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtSolNombres" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Teléfono</td>
                                    <td class="etiqueta">:</td>
                                    <td style="color:Red;">
                                        <asp:TextBox runat="server" ID="txtSolTelefono" CssClass="textbox" Width="60%" Text=""></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtSolTelefono" Enabled="true" ValidChars="1234567890">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" colspan="3" style="padding: 5px 0px 5px 0px;">Observación</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox runat="server" ID="txtSolObservacion" CssClass="textbox" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px" Text=""></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtSolObservacion" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚüÜ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width:25%;"></td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button runat="server" ID="btnGuardar" CssClass="ImagenBotonGuardar" Width="150px" Text="Registrar" OnClientClick="return validarRegistrarCarne();" OnClick="registrarRegPrevio" />
                        <asp:Button runat="server" ID="btnGuardarEdicion" CssClass="ImagenBotonGuardar"  Width="150px" OnClientClick="return validarRegistrarCarne();" OnClick="actualizarRegistroPrevio" Text="Guardar Cambios" Visible="false" />
                        <asp:Button runat="server" ID="btnImprimir" CssClass="ImagenBotonPrint" Width="150px" OnClick="verActaRecepcion" Text="Imprimir Cargo" Visible="false" />
                        <asp:Button runat="server" ID="btnCancelar" CssClass="ImagenBotonCancelar" Width="150px" Text="Cancelar" Visible="false" OnClick="cancelarEdicion" />
                        <asp:Button runat="server" ID="btnVolver" CssClass="ImagenBotonVolver" Width="150px" Text="Volver" Visible="false" OnClick="cancelarEdicion" />
                        <asp:Button runat="server" ID="btnLimpiar" CssClass="ImagenBotonNuevo"  Width="150px" Text="Nuevo Registro" OnClick="nuevoRegistroPrevio"/>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
