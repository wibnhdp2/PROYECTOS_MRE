<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master"
    AutoEventWireup="true" CodeBehind="RegistradorCarneRegistro.aspx.cs" Inherits="SolCARDIP.Paginas.Registrador.RegistradorCarneRegistro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../../Estilos/modal.css" rel="stylesheet" type="text/css" />
    <base href="RegistradorCarneRegistro.aspx" target="_self" />
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function vertrRelDep(valor) {
            var tr1 = document.getElementById("trRelDep")
            if (tr1 != null) {
                tr1.style.display = valor;
            }
        }

        function cerrarDiv(divNombre) {
            var div1 = document.getElementById(master + divNombre);
            if (div1 != null) {
                div1.style.display = "none";
            }
        }

        function disIntablaFuncionario(valor) {
            var dropDownTipoCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownCatMision = document.getElementById(master + "ddlCategoriaOfcoEx");
            var dropDownOficinaConsularEx = document.getElementById(master + "ddlMision");
            for (i = 0; i < document.forms[0].length; i++) {
                doc = document.forms[0].elements[i];
                switch (doc.type) {
                    case "text":
                        doc.disabled = valor;
                        break;
                    //                case "submit": 
                    //                    doc.disabled = valor; 
                    //                    break; 
                    case "image":
                        doc.disabled = valor;
                        break;
                    case "select-one":
                        doc.disabled = valor;
                        break;
                    case "select-multiple":
                        doc.disabled = valor;
                        break;
                    default:
                        break;
                }
            }
            if (dropDownTipoCalMig.options[dropDownTipoCalMig.selectedIndex].text == "HUMANITARIA") {
                dropDownCatMision.disabled = true;
                dropDownOficinaConsularEx.disabled = true;
            }
        }

        function validarRegistrarCarne() {
            var lblNacionalidad = document.getElementById(master + "lblNacionalidad");

            var txtApePat = document.getElementById(master + "txtApePat");
            var txtApeMat = document.getElementById(master + "txtApeMat");
            var txtNombres = document.getElementById(master + "txtNombres");
            var txtFechaNac = document.getElementById(master + "txtFechaNac");
            var txtNumeroDocIdent = document.getElementById(master + "txtNumeroDocIdent");
            var txtDireccion = document.getElementById(master + "txtDomicilio");
            var txtMesaPartes = document.getElementById(master + "txtMesaPartes");

            var dropDownEstCivil = document.getElementById(master + "ddlEstadoCivil");
            var dropDownGenero = document.getElementById(master + "ddlSexo");
            var dropDownDocIdent = document.getElementById(master + "ddlDocumentoIdent");
            var dropDownPais = document.getElementById(master + "ddlNacionalidad");
            var dropDownDepartamento = document.getElementById(master + "ddlDepartamento");
            var dropDownProvincia = document.getElementById(master + "ddlProvincia");
            var dropDownDistrito = document.getElementById(master + "ddlDistrito");
            var dropDownTipoCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownCargo = document.getElementById(master + "ddlCalidadMigratoriaSec");
            var dropDownOficinaConsularEx = document.getElementById(master + "ddlMision");

            if (txtApePat != null & txtApeMat != null & txtNombres != null & txtFechaNac != null & txtNumeroDocIdent != null & txtDireccion != null & txtMesaPartes != null &
            dropDownEstCivil != null & dropDownGenero != null & dropDownDocIdent != null & dropDownPais != null & dropDownDepartamento != null & dropDownProvincia != null & dropDownDistrito != null &
            dropDownTipoCalMig != null & dropDownCargo != null & dropDownOficinaConsularEx != null) {

                if (txtMesaPartes.value == "") {
                    if (!confirm("¿ DESEA REGISTRAR SIN NUMERO DE MESA DE PARTES ?")) {
                        txtMesaPartes.focus();
                        return false;
                    }
                }

                if (txtApePat.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO DE LA PERSONA."); txtApePat.focus(); return false; }
                if (txtNombres.value == "") { alert("DEBE INGRESAR UN NOMBRE DE LA PERSONA."); txtNombres.focus(); return false; }
                if (txtFechaNac.value == "") { alert("DEBE INGRESAR LA FECHA DE NACIMIENTO DE LA PERSONA."); txtFechaNac.focus(); return false; }

                // VALIDAR FECHA DE NACIMIENTO ------------------------------------------------------
                var fechaNac = txtFechaNac.value;

                if (!validaFechaDDMMAAAA(fechaNac)) { alert("FECHA NO VALIDA. REVISE."); txtFechaNac.focus(); return false; };

                var now = new Date();
                var todayAtMidn = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                var mes = (parseInt(fechaNac.substring(3, 5)) - 1);
                var year = fechaNac.substring(6, 10);
                var dia = fechaNac.substring(0, 2);

                var fechaInicial1 = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                valorFecha1 = fechaInicial1.valueOf();
                FechaActual = valorFecha1 + (60 * 24 * 60 * 60 * 1000);

                var fechaInicial2 = new Date(year, mes, dia);
                valorFecha2 = fechaInicial2.valueOf();
                FechaNacimiento = valorFecha2 + (60 * 24 * 60 * 60 * 1000);

                if (FechaNacimiento > FechaActual) {
                    alert("FECHA DE NACIMIENTO NO DEBE SER MAYOR A LA FECHA ACTUAL.");
                    return false;
                }

                // ----------------------------------------------------------------------------------

                if (dropDownGenero.value == "0") { alert("DEBE SELECCIONAR EL GENERO (SEXO) DE LA PERSONA."); dropDownGenero.focus(); return false; }
                if (dropDownEstCivil.value == "0") { alert("DEBE SELECCIONAR EL ESTADO CIVIL DE LA PERSONA."); dropDownEstCivil.focus(); return false; }

                if (dropDownDocIdent.value == "0") { alert("DEBE SELECCIONAR EL TIPO DE DOCUMENTO DE IDENTIDAD DE LA PERSONA."); dropDownDocIdent.focus(); return false; }
                if (txtNumeroDocIdent.value == "") { alert("DEBE INGRESAR EL NUMERO DE DOCUMENTO DE IDENTIFICACION DE LA PERSONA."); txtNumeroDocIdent.focus(); return false; }

                if (dropDownPais.value == "0") { alert("DEBE SELECCIONAR EL PAIS DE ORIGEN DE LA PERSONA"); dropDownPais.focus(); return false; }
                if (lblNacionalidad.innerHTML == "[ NO DEFINIDO ]") { alert("LA NACIONALIDAD DE LA PERSONA ESTA NO DEFINIDA. COMUNIQUESE CON SOPORTE TECNICO."); dropDownPais.focus(); return false; }

                if (txtDireccion.value == "") { alert("DEBE INGRESAR LA DIRECCION O DOMICILIO DE LA PERSONA."); txtDireccion.focus(); return false; }
                if (dropDownDepartamento.value == "00") { alert("DEBE SELECCIONAR EL DEPARTAMENTO DEL DOMICILIO DE LA PERSONA."); dropDownDepartamento.focus(); return false; }
                if (dropDownProvincia.value == "00") { alert("DEBE SELECCIONAR LA PROVINCIA DEL DOMICILIO DE LA PERSONA."); dropDownProvincia.focus(); return false; }
                if (dropDownDistrito.value == "00") { alert("DEBE SELECCIONAR EL DISTRITO DEL DOMICILIO DE LA PERSONA."); dropDownDistrito.focus(); return false; }

                if (dropDownTipoCalMig.value == "0") { alert("DEBE SELECCIONAR UNA CALIDAD MIGRATORIA."); dropDownTipoCalMig.focus(); return false; }
                if (dropDownCargo.value == "0") { alert("DEBE SELECCIONAR UN CARGO"); dropDownCargo.focus(); return false; }

                if (dropDownTipoCalMig.options[dropDownTipoCalMig.selectedIndex].text != "HUMANITARIA") {
                    if (dropDownOficinaConsularEx.value == "0") { alert("DEBE SELECCIONAR UNA OFICINA CONSULAR."); dropDownOficinaConsularEx.focus(); return false; }
                    if (dropDownOficinaConsularEx.options[dropDownOficinaConsularEx.selectedIndex].text == "-----") { alert("NO ES UNA INSTITUCION VALIDA."); dropDownOficinaConsularEx.focus(); return false; }
                }
            }
            else {
                return false;
            }
            if (confirm("¿ DESEA REGISTRAR EL CARNÉ DE IDENTIDAD ?")) {
                disIntablaFuncionario(true);
                return true;
            }
            else {
                return false;
            }

        }
        
        function AbrirPopup() {
            document.getElementById('simpleModal_1').style.display = 'block';
        }
        function cerrarPopup() {
            document.getElementById('simpleModal_1').style.display = 'none';
        }
        function validarCalidadMigratoria() {
            var dropDown1 = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDown2 = document.getElementById(master + "ddlTitDep");
            if (dropDown1 != null & dropDown2 != null) {
                if (dropDown1.value == "0" | dropDown2.value == "0") {
                    return false;
                }
                else {
                    return true;
                }
            }
            return false
        }

        function changeRadioButton() {
            var hd1 = document.getElementById(master + "hdrbt");
            var rbt1 = document.getElementById(master + "rbtTitular");
            var rbt2 = document.getElementById(master + "rbtDependiente");
            if (hd1 != null & rbt1 != null & rbt2 != null) {
                if (hd1.value == "0") {
                    rbt1.checked = true;
                    rbt2.checked = false;
                }
                else {
                    rbt1.checked = false;
                    rbt2.checked = true;
                }
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            font-family: Trebuchet MS;
            font-style: normal;
            font-size: 12px;
            font-weight: bold;
            text-align: left;
            margin-left: 80px;
            width: 30%;
            height: 25px;
        }
        .style2
        {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <div id="simpleModal_1" class="modal">
                <div class="modal-window">
                    <div class="modal-titulo">
                        <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Imagenes/Iconos/cerrar.png"
                            OnClientClick="cerrarPopup(); return false" runat="server" />
                        <span>VALIDACIÓN</span>
                    </div>
                    <div class="modal-cuerpo">
                        <h3>
                            Existe una persona que cuenta con carné y tiene el mismo apellido y nombre. ¿Desea continuar?</h3>
                    </div>
                    <div class="modal-pie">
                        <asp:Button ID="btnRegistrarCarnetPopup" runat="server" CssClass="ButtonFiltro"
                            Text="Continuar" onclick="btnRegistrarCarnetPopup_Click" />
                        <asp:Button ID="btnCerrarPopup" runat="server" CssClass="ButtonFiltro" Text="Cancelar"
                            TOnClientClick="cerrarPopup(); return false" />
                    </div>
                </div>
            </div>
            <table align="center" style="width: 80%;">
                <tr>
                    <td>
                        <div id="divModal" runat="server" style="display: none" class="modalBackgroundLoad">
                        </div>
                        <div id="divRelDep" runat="server" style="position: fixed; top: 40%; left: 40%; width: 30%;
                            height: 10%; z-index: 2; display: none" class="modalBackground1">
                            <table class="AnchoTotal">
                                <tr>
                                    <td>
                                        <fieldset class="Fieldset">
                                            <legend class="FieldsetLeyenda">Relacion de Dependencia</legend>
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td class="etiqueta">
                                                        Numero de Carné
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblRelDepNumCarne" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">
                                                        Nombre del Titular
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblRelDepTitular" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <input type="button" value="Cerrar" onclick="cerrarDiv('divRelDep');cerrarDiv('divModal');" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="etiqueta" colspan="2" style="font-size: 16px;">
                        Solicitud:
                        <asp:Label runat="server" ID="lblIdentificador" CssClass="labelInfoFile" Font-Size="16px"
                            Text="[ No Registrado ]"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <table style="width: 100%" id="tablaMesaPartes" runat="server">
                            <tr>
                                <td class="etiqueta" style="width: 10%">
                                    NRO MESA DE PARTES
                                </td>
                                <td class="etiqueta" style="width: 3%">
                                    :
                                </td>
                                <td style="width: 50%">
                                    <asp:TextBox runat="server" ID="txtMesaPartes" CssClass="textbox" Width="15%" MaxLength="20"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblInfoRenovar"
                                        CssClass="labelInfoFile" ForeColor="Red"></asp:Label>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtMesaPartes"
                                        Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890-/">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <td rowspan="2">
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td class="etiqueta" style="width: 25%;">
                                                Solicitud en Linea
                                            </td>
                                            <td style="width: 55%;">
                                                <asp:TextBox runat="server" ID="txtRegLinea" CssClass="textboxRegLinea" Font-Names="Trebuchet MS"
                                                    Style="text-align: center; background-color: #0055aa; color: White" Width="100%"
                                                    MaxLength="9"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtRegLinea"
                                                    Enabled="true" ValidChars="0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtBuscarRegLinea" runat="server" ImageUrl="~/Imagenes/Iconos/quicksearch.gif"
                                                    ToolTip="Buscar Solicitud en Linea" BorderWidth="0" OnClick="buscarRegLinea" />
                                                <asp:ImageButton ID="ImgDescargarResumenLinea" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" Visible="false"
                                                    ToolTip="Descargar Resumen de Solicitud" BorderWidth="0" OnClick="descargarResumen" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lblRegLineaMensaje" CssClass="labelInfo" Width="100%"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="etiqueta">
                                    ES MENOR DE EDAD
                                </td>
                                <td class="etiqueta">
                                    :
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkEsMenor" CssClass="checkbox" Text=" " />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; vertical-align: top;">
                        <table class="AnchoTotal">
                            <tr>
                                <td>
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Datos del Titular</legend>
                                        <table id="tablaFuncionario" runat="server" class="AnchoTotal">
                                            <tr>
                                                <td class="style1">
                                                    Primer Apellido
                                                </td>
                                                <td style="color: Red;" class="style2">
                                                    <asp:TextBox runat="server" ID="txtApePat" CssClass="textbox" Width="90%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtApePat"
                                                        Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                </td>
                                                <td rowspan="3" align="right">
                                                    <img src="../../Imagenes/Iconos/user3.png" width="32" height="32" alt="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Segundo Apellido
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtApeMat" CssClass="textbox" Width="90%" MaxLength="100"
                                                        Text=""></asp:TextBox>
                                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtApeMat"
                                                        Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Nombres
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:TextBox runat="server" ID="txtNombres" CssClass="textbox" Width="90%" MaxLength="100"
                                                        Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                    <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNombres"
                                                        Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Fecha de Nacimiento
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:TextBox runat="server" ID="txtFechaNac" CssClass="textbox" Width="20%" MaxLength="10"
                                                        Text=""></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFechaNac"
                                                        Enabled="true" ValidChars="1234567890/">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <cc1:CalendarExtender ID="CalendarOcurrencia" runat="server" TargetControlID="txtFechaNac"
                                                        PopupButtonID="ibtFechaNac" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <asp:ImageButton ID="ibtFechaNac" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif"
                                                        BorderWidth="0" />&nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Sexo
                                                </td>
                                                <td colspan="2">
                                                    <table class="AnchoTotal">
                                                        <tr>
                                                            <td style="width: 40%; color: Red">
                                                                <asp:DropDownList runat="server" ID="ddlSexo" CssClass="dropdownlist" AutoPostBack="true"
                                                                    Width="80%" OnSelectedIndexChanged="seleccionarSexo">
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp;&nbsp;*
                                                            </td>
                                                            <td style="width: 20%; text-align: right;" class="etiqueta">
                                                                Estado Civil
                                                            </td>
                                                            <td style="width: 40%; color: Red;">
                                                                <asp:DropDownList runat="server" ID="ddlEstadoCivil" CssClass="dropdownlist" AutoPostBack="false"
                                                                    Width="80%" Enabled="false">
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp;&nbsp;*
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Documento de Identidad
                                                </td>
                                                <td colspan="2">
                                                    <table class="AnchoTotal">
                                                        <tr>
                                                            <td style="width: 40%; color: Red;">
                                                                <asp:DropDownList runat="server" ID="ddlDocumentoIdent" CssClass="dropdownlist" AutoPostBack="false"
                                                                    Width="90%">
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp;&nbsp;*
                                                            </td>
                                                            <td style="width: 20%; text-align: right;" class="etiqueta">
                                                                Numero
                                                            </td>
                                                            <td style="width: 40%; color: Red;">
                                                                <asp:TextBox runat="server" ID="txtNumeroDocIdent" CssClass="textbox" MaxLength="20"
                                                                    Width="80%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtNumeroDocIdent"
                                                                    Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/-.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    País (Nacionalidad)
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlNacionalidad" CssClass="dropdownlist" AutoPostBack="true"
                                                        Width="40%" OnSelectedIndexChanged="obtenerNacionalidadPais">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;* &nbsp;&nbsp;
                                                    <asp:Label runat="server" ID="lblNacionalidad" CssClass="labelInfo" ForeColor="Green"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Teléfono
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:TextBox runat="server" ID="txtTelefono" CssClass="textbox" Width="60%" Text=""></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtTelefono"
                                                        Enabled="true" ValidChars="1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Domicilio
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:TextBox runat="server" ID="txtDomicilio" CssClass="textbox" MaxLength="200"
                                                        Width="90%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtDomicilio"
                                                        Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Departamento
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlDepartamento" CssClass="dropdownlist" AutoPostBack="true"
                                                        Width="50%" OnSelectedIndexChanged="seleccionarDepartamento">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Provincia
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlProvincia" CssClass="dropdownlist" AutoPostBack="true"
                                                        Width="50%" Enabled="false" OnSelectedIndexChanged="seleccionarProvincia">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Distrito
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlDistrito" CssClass="dropdownlist" AutoPostBack="false"
                                                        Width="50%" Enabled="false">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Datos del Documento (Carné)</legend>
                                        <table class="AnchoTotal">
                                            <tr>
                                                <td style="width: 50%;" class="etiqueta">
                                                    Fecha de Emisión (Se genera con la aprobación)
                                                </td>
                                                <td style="width: 30%;">
                                                    <asp:TextBox runat="server" ID="txtFechaEmision" CssClass="textbox" Width="50%" MaxLength="10"
                                                        Enabled="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtFechaEmision"
                                                        Enabled="true" ValidChars="1234567890/">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td rowspan="2" align="right">
                                                    <img src="../../Imagenes/Iconos/carne.png" width="32" height="32" alt="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Fecha de Vencimiento (se genera desde la administracion)
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaVencimiento" CssClass="textbox" Width="50%"
                                                        MaxLength="10" Enabled="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtFechaVencimiento"
                                                        Enabled="true" ValidChars="1234567890/">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align: top;">
                        <table class="AnchoTotal">
                            <tr>
                                <td colspan="2">
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Calidad Migratoria</legend>
                                        <table id="tablaCalMig" runat="server" class="AnchoTotal">
                                            <tr>
                                                <td style="width: 15%;" class="etiqueta">
                                                    Tipo
                                                </td>
                                                <td style="width: 45%;">
                                                    <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaPri" CssClass="dropdownlist"
                                                        AutoPostBack="true" OnSelectedIndexChanged="seleccionarCalidadMigratoria" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="etiqueta" style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlTitDep" CssClass="dropdownlist" AutoPostBack="true"
                                                        Width="60%" Enabled="true" OnSelectedIndexChanged="seleccionarCalidadMigratoria">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                    <%--<label><input runat="server" type="radio" id="rbtTitular" onchange="document.getElementById(master + 'rbtDependiente').checked = false;document.getElementById(master + 'hdrbt').value = 0;return validarCalidadMigratoria();" checked="true" />Titular</label>--%>
                                                    <%--<label><input runat="server" type="radio" id="rbtDependiente" onchange="document.getElementById(master + 'rbtTitular').checked = false;document.getElementById(master + 'hdrbt').value = 1;return validarCalidadMigratoria();" />Dependiente</label>--%>
                                                </td>
                                                <td rowspan="4" align="right">
                                                    <img src="../../Imagenes/Iconos/calidad.png" width="32" height="32" alt="" />
                                                </td>
                                            </tr>
                                            <tr id="trRelDep" runat="server" style="display: none;">
                                                <td class="etiqueta">
                                                    Rel. Dep.
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:TextBox runat="server" ID="txtRelDep" CssClass="textboxRegLinea" Font-Names="Trebuchet MS"
                                                        Style="text-align: center; background-color: White; color: Black" Width="95%"
                                                        MaxLength="9"></asp:TextBox>
                                                </td>
                                                <td class="etiqueta" style="vertical-align: middle;">
                                                    <asp:ImageButton ID="ibtBuscarRelDep" runat="server" ImageUrl="~/Imagenes/Iconos/buscar1.png"
                                                        ToolTip="Buscar Realcion de Dependencia" OnClick="buscarRelacionDependencia"
                                                        BorderWidth="0" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="ibtVerRelDep" runat="server" ImageUrl="~/Imagenes/Iconos/verFoto.png"
                                                        ToolTip="Buscar Realcion de Dependencia" OnClick="mostrarRelDep" BorderWidth="0"
                                                        Visible="false" />
                                                    <%--<asp:Label runat="server" ID="lblMensaje" CssClass="labelInfo textbox" ForeColor="#0055aa" Text="[NO SELECCIONADO]"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Cargo
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaSec" CssClass="dropdownlist"
                                                        AutoPostBack="false" Width="100%" Enabled="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:Label runat="server" ID="lblMensajeCalidadMigratoria" CssClass="labelInfo" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox runat="server" ID="txtCalidadMigratoria" CssClass="textboxMultilineaDef"
                                                        Width="100%" TextMode="MultiLine" Height="100px" onkeydown="return false;"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Datos de la Mision</legend>
                                        <table id="tablaMision" runat="server" class="AnchoTotal">
                                            <tr>
                                                <td style="width: 20%;" class="etiqueta">
                                                    Categoría
                                                </td>
                                                <td style="color: Red; width: 70%">
                                                    <asp:DropDownList runat="server" ID="ddlCategoriaOfcoEx" CssClass="dropdownlist"
                                                        AutoPostBack="true" OnSelectedIndexChanged="seleccionarCategoriaOficina" Width="70%">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                </td>
                                                <td rowspan="2" align="right">
                                                    <img src="../../Imagenes/Iconos/consulado1.png" width="32" height="32" alt="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Institución
                                                </td>
                                                <td style="color: Red;">
                                                    <asp:DropDownList runat="server" ID="ddlMision" CssClass="dropdownlist" AutoPostBack="false"
                                                        Width="90%" Enabled="false">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;*
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Fotografía (Obligatorio)</legend>
                                        <table id="tablaFotografia" runat="server" class="AnchoTotal">
                                            <tr>
                                                <td style="width: 30%;" class="etiqueta">
                                                    Seleccionar Foto
                                                </td>
                                                <td>
                                                    <input id="hdnFile" type="hidden" runat="server" value="0" />
                                                    <asp:FileUpload ID="fulImagen" accept="application/png" runat="server" onchange="ContentPlaceHolder1_hdnFile.value = 1;javascript:try{submit();}catch(err){}"
                                                        Width="100%" />
                                                </td>
                                                <td align="right" rowspan="5" style="vertical-align: middle;">
                                                    <asp:ImageButton ID="ibtVerImagen" runat="server" ImageUrl="~/Imagenes/Iconos/jpeg1.png"
                                                        OnClientClick="window.open('../VerImagenNueva.aspx','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0','_blank')"
                                                        ToolTip="Ver Fotografia Nueva" BorderWidth="0" Width="40" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Nombre Archivo
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoNombre" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Peso Archivo
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoPeso" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Tipo Imagen
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoExtension" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Fotografía almacenada
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ibtVerImagenAlmacenada" runat="server" ImageUrl="~/Imagenes/Iconos/openLog.png"
                                                        OnClientClick="window.open('../VerImagenSave.aspx?imagen=foto','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"
                                                        Enabled="false" ToolTip="Ver Fotografia Almacenada" BorderWidth="0" Width="15" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblObsFoto" CssClass="labelInfo" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Firma (Obligatorio)</legend>
                                        <table id="tablaFirma" runat="server" class="AnchoTotal">
                                            <tr>
                                                <td style="width: 30%;" class="etiqueta">
                                                    Seleccionar Firma
                                                </td>
                                                <td>
                                                    <input id="hdnFileFirma" type="hidden" runat="server" value="0" />
                                                    <asp:FileUpload ID="fulImagenFirma" accept="application/png" runat="server" onchange="ContentPlaceHolder1_hdnFileFirma.value = 1;javascript:try{submit();}catch(err){}"
                                                        Width="100%" />
                                                </td>
                                                <td align="right" rowspan="5" style="vertical-align: middle;">
                                                    <asp:ImageButton ID="ibtVerFirma" runat="server" ImageUrl="~/Imagenes/Iconos/jpeg1.png"
                                                        OnClientClick="window.open('../VerFirmaNueva.aspx','_blank','width=450,height=400,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0','_blank')"
                                                        ToolTip="Ver Fotografia Nueva" BorderWidth="0" Width="40" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Nombre Archivo
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoNombreFirma" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Peso Archivo
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoPesoFirma" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Tipo Imagen
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" ID="lblArchivoExtensionFirma" CssClass="labelInfoFile"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">
                                                    Firma almacenada
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ibtVerFirmaAlmacenada" runat="server" ImageUrl="~/Imagenes/Iconos/openLog.png"
                                                        OnClientClick="window.open('../VerFirmaSave.aspx?imagen=firma','_blank','width=450,height=400,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"
                                                        Enabled="false" ToolTip="Ver Fotografia Almacenada" BorderWidth="0" Width="15" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblObsFirma" CssClass="labelInfo" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="etiqueta" style="color: Green;">
                                    * Solo Archivos en formato JPEG
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblObsImpresion" CssClass="labelInfo" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" class="EtiquetaCentro">
                        Los datos que tienen el simbolo asterisco (*) de color rojo al lado derecho son
                        datos obligatorios
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="2" align="center" class="EtiquetaCentro"><asp:Label runat="server" ID="lblPrueba"></asp:Label></td>
                </tr>--%>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Label runat="server" ID="lblInfoObserbacion"  CssClass="labelInfoFile" ForeColor="Red"></asp:Label>                        
                        <asp:Button runat="server" ID="btnGuardar" CssClass="ImagenBotonGuardar" Width="150px" Text="Registrar Carné" OnClientClick="return validarRegistrarCarne();" OnClick="registrarCarne" />
                        <asp:Button runat="server" ID="btnGuardarEdicion" CssClass="ImagenBotonGuardar" Width="150px" OnClientClick="return validarRegistrarCarne();" OnClick="guardarCambios" Text="Guardar Cambios" Visible="false" />
                        <asp:Button runat="server" ID="btnCancelar" CssClass="ImagenBotonCancelar" OnClick="cancelarEdicion" Width="150px" Text="Cancelar" Visible="false" />
                        <%--<asp:Button runat="server" ID="btnLimpiar" CssClass="ImagenBotonNuevo"  Width="150px" Text="Nuevo Carné" OnClick="limpiarControles" />--%>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hRutaResumenLinea"  />
            <asp:HiddenField ID="hErrorRegistro" runat="server" />
            <asp:HiddenField runat="server" ID="hdrbt" Value="0" />
            <asp:HiddenField ID="TKSEGENC" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
