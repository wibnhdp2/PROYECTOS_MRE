<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="AdministradorMantenimiento.aspx.cs" Inherits="SolCARDIP.Paginas.Administrador.AdministradorMantenimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <base href="AdministradorMantenimiento.aspx" target="_self" />
    <script type="text/javascript">
        function seguridadURLPrevia() {
            if (document.referrer != "") {
                preloader();
            }
            else {
                location.href = '../../mensajes.aspx';
            }
        }

        function mostrarCargando() {
            var master = "ContentPlaceHolder1_";
            document.getElementById(master + "divCargando").style.display = "block";
            document.getElementById(master + "divModal2").style.display = "block";
            document.getElementById(master + "divCargando").focus();
        }

        function preloader() {
            var master = "ContentPlaceHolder1_";
            document.getElementById(master + "divCargando").style.display = "none";
            document.getElementById(master + "divModal2").style.display = "none";
        }

        function verDiv(id) {
            var master = "ContentPlaceHolder1_"
            document.getElementsByName(id).style.display = "block"
        }
        function ocultarDiv(id) {
            var master = "ContentPlaceHolder1_"
            document.getElementById(master + id).style.display = "none"
        }

        function focusControl(controlId) {
            var ctrl = document.getElementById(controlId);
            if (ctrl != null) {
                ctrl.focus();
            }
        }
        window.onload = seguridadURLPrevia;
    </script>
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function validarEditarSolicitante() {
            var txtMantPriApeSol = document.getElementById(master + "txtEditarPriApeSoli");
            var txtMantNombresSol = document.getElementById(master + "txtEditarNombresSoli");
            var ddlMantTipoDodIdent = document.getElementById(master + "ddlEditarTipoDocIdentSoli");
            var txtMantNumDocIdentSol = document.getElementById(master + "txtEditarNumIdentSoli");
            if (txtMantPriApeSol != null & txtMantNombresSol != null & ddlMantTipoDodIdent != null & txtMantNumDocIdentSol != null) {
                if (txtMantPriApeSol.value == "") { alert("DEBE INGRESAR UN APELLIDO PARA EL SOLICITANTE"); txtMantPriApeSol.focus(); return false; }
                if (txtMantNombresSol.value == "") { alert("DEBE INGRESAR UN NOMBRE PARA EL SOLICITANTE"); txtMantNombresSol.focus(); return false; }
                if (ddlMantTipoDodIdent.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE IDENTIFICACIÓN"); ddlMantTipoDodIdent.focus(); return false; }
                if (txtMantNumDocIdentSol.value == "") { alert("DEBE INGRESAR UN NUMERO DE IDENTIFICACIÓN"); txtMantNumDocIdentSol.focus(); return false; }
                return true;
            }
            return false;
        }

        function validarEditarInstitucion() {
            var dropDowncategoria = document.getElementById(master + "ddlEditarCategoriaInst");
            var txtMantNombre = document.getElementById(master + "txtEditarNombreInst");
            var txtMantSiglas = document.getElementById(master + "txtEditarSiglasInst");
            if (dropDowncategoria != null & txtMantNombre != null & txtMantSiglas != null) {
                if (dropDowncategoria.value == "0") { alert("DEBE SELECCIONAR UNA CATEGORIA"); dropDowncategoria.focus(); return false; }
                if (txtMantNombre.value == "") { alert("DEBE INGRESAR UN NOMBRE PARA LA INSTITUCION"); txtMantNombre.focus(); return false; }
                if (txtMantSiglas.value == "") {
                    if (confirm("¿ DESEA AGREGAR LA INSTITUCION CON SIGLAS NO DEFINIDAS ?")) {
                        txtMantSiglas.value = "[ NO DEFINIDO ]";
                    }
                    else {
                        alert("DEBE SELECCIONAR UNAS SIGLAS PARA LA INSTITUCION"); txtMantSiglas.focus(); return false;
                    }
                }
                return true;
            }
            return false;
        }

        function limpiarInstitucion() {
            var dropDowncategoria = document.getElementById(master + "ddlMantCategoriaOfcoEx");
            var txtMantNombre = document.getElementById(master + "txtMantNombre");
            var txtMantSiglas = document.getElementById(master + "txtMantSiglas");
            if (dropDowncategoria != null & txtMantNombre != null & txtMantSiglas != null) {
                dropDowncategoria.value = "0";
                txtMantNombre.value = "";
                txtMantSiglas.value = "";
            }
        }

        function limpiarSolicitante() {
            var txtMantPriApeSol = document.getElementById(master + "txtMantPriApeSol");
            var txtMantSegApeSol = document.getElementById(master + "txtMantSegApeSol");
            var txtMantNombresSol = document.getElementById(master + "txtMantNombresSol");
            var ddlMantTipoDodIdent = document.getElementById(master + "ddlMantTipoDodIdent");
            var txtMantNumDocIdentSol = document.getElementById(master + "txtMantNumDocIdentSol");
            var txtMantTelefonoSol = document.getElementById(master + "txtMantTelefonoSol");
            if (txtMantPriApeSol != null & txtMantSegApeSol != null & txtMantNombresSol != null & ddlMantTipoDodIdent != null & txtMantNumDocIdentSol != null & txtMantTelefonoSol != null) {
                txtMantPriApeSol.value = "";
                txtMantSegApeSol.value = "";
                txtMantNombresSol.value = "";
                ddlMantTipoDodIdent.value = "0";
                txtMantNumDocIdentSol.value = "";
                txtMantTelefonoSol.value = "";
            }
        }


        function validarNuevoSolicitante() {
            var txtMantPriApeSol = document.getElementById(master + "txtMantPriApeSol");
            var txtMantNombresSol = document.getElementById(master + "txtMantNombresSol");
            var ddlMantTipoDodIdent = document.getElementById(master + "ddlMantTipoDodIdent");
            var txtMantNumDocIdentSol = document.getElementById(master + "txtMantNumDocIdentSol");
            if (txtMantPriApeSol != null & txtMantNombresSol != null & ddlMantTipoDodIdent != null & txtMantNumDocIdentSol != null) {
                if (txtMantPriApeSol.value == "") { alert("DEBE INGRESAR UN APELLIDO PARA EL SOLICITANTE"); txtMantPriApeSol.focus(); return false; }
                if (txtMantNombresSol.value == "") { alert("DEBE INGRESAR UN NOMBRE PARA EL SOLICITANTE"); txtMantNombresSol.focus(); return false; }
                if (ddlMantTipoDodIdent.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE IDENTIFICACIÓN"); ddlMantTipoDodIdent.focus(); return false; }
                if (txtMantNumDocIdentSol.value == "") { alert("DEBE INGRESAR UN NUMERO DE IDENTIFICACIÓN"); txtMantNumDocIdentSol.focus(); return false; }
                return true;
            }
            return false;
        }

        function validarNuevaInstitucion() {
            var dropDowncategoria = document.getElementById(master + "ddlMantCategoriaOfcoEx");
            var txtMantNombre = document.getElementById(master + "txtMantNombre");
            var txtMantSiglas = document.getElementById(master + "txtMantSiglas");
            if (dropDowncategoria != null & txtMantNombre != null & txtMantSiglas != null) {
                if (dropDowncategoria.value == "0") { alert("DEBE SELECCIONAR UNA CATEGORIA"); dropDowncategoria.focus(); return false; }
                if (txtMantNombre.value == "") { alert("DEBE INGRESAR UN NOMBRE PARA LA INSTITUCION"); txtMantNombre.focus(); return false; }
                if (txtMantSiglas.value == "") {
                    if (confirm("¿ DESEA AGREGAR LA INSTITUCION CON SIGLAS NO DEFINIDAS ?")) {
                        txtMantSiglas.value = "[ NO DEFINIDO ]";
                    }
                    else {
                        alert("DEBE SELECCIONAR UNAS SIGLAS PARA LA INSTITUCION"); txtMantSiglas.focus(); return false;
                    }
                }
                return true;
            }
            return false;
        }
        function validarNuevaMensaje() {
            var estadoid = document.getElementById(master + "ddlEstadoCarnet");
            var txtMensaje = document.getElementById(master + "txtMensaje");
            if (estadoid.value === "0") {
                alert("DEBE SELECCIONAR ESTADO");
                estadoid.focus();
                return false;
            }
            if (txtMensaje.value == "") {
                alert("DEBE INGRESAR LA DESCRIPCIÓN DEL MENSAJE");
                txtMensaje.focus();
                return false;
            }
            var text=txtMensaje.value;
            txtMensaje.value=text.replace(/(?:\r\n|\r|\n)/g, ' ');
            return true;
        }
        function ancelarEditMensaje() {
            document.getElementById(master + "divEditarMensajeEstado").style.display = "none";
            document.getElementById(master + "divModal").style.display="none";
            return true;
        }

        function validarEdicionDocIdent() {
            var txtNuevoDocIdent = document.getElementById(master + "txtEditarDescCorta");
            var txtNuevoDocIdentDesc = document.getElementById(master + "txtEditarDescLarga");
            if (txtNuevoDocIdent != null & txtNuevoDocIdentDesc != null) {
                if (txtNuevoDocIdent.value == "") { alert("DEBE INGRESAR NOMBRE DE TIPO DE DOCUMENTO"); txtNuevoDocIdent.focus(); return false; }
                if (txtNuevoDocIdentDesc.value == "") { alert("DEBE INGRESAR UNA DESCRIPCION"); txtNuevoDocIdentDesc.focus(); return false; }
                if (confirm("¿ DESEA EDITAR EL DOCUMENTO DE IDENTIDAD ?")) {
                    return true;
                }
            }
            return false;
        }

        function validarEdicionCargo() {
            var ddlSexo = document.getElementById(master + "ddlEditarSexo");
            var ddlTitdep = document.getElementById(master + "ddlEditarTitDep");
            var txtNuevoC = document.getElementById(master + "txteditarCargo");

            if (txtNuevoC != null & ddlSexo != null & ddlTitdep != null) {
                if (ddlSexo.value == "0") { alert("DEBE SELECCIONAR UN SEXO"); ddlSexo.focus(); return false; }
                if (ddlTitdep.value == "0") { alert("DEBE SELECCIONAR UNA OPCION DE TITULAR O DEPENDIENTE"); ddlTitdep.focus(); return false; }
                if (txtNuevoC.value == "") { alert("DEBE AGREGAR UN NOMBRE AL CARGO"); txtNuevoC.focus() ; return false; }
                if (confirm("¿ DESEA EDITAR EL CARGO ?")) {
                    return true;
                }
            }
            return false;
        }

        function validarNuevoCargo() {
            var ddlSexo = document.getElementById(master + "ddlSexo");
            var ddlCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var ddlTitdep = document.getElementById(master + "ddlTitDep");
            var txtNuevoC = document.getElementById(master + "txtNuevoCargo");
            if (txtNuevoC != null & ddlSexo != null & ddlTitdep != null & ddlCalMig != null) {
                if (ddlSexo.value == "0") { alert("DEBE SELECCIONAR UN SEXO"); ddlSexo.focus(); return false; }
                if (ddlCalMig.value == "0") { alert("DEBE SELECCIONAR UNA CALIDAD MIGRATORIA"); ddlCalMig.focus(); return false; }
                if (ddlTitdep.value == "0") { alert("DEBE SELECCIONAR UNA OPCION DE TITULAR O DEPENDIENTE"); ddlTitdep.focus(); return false; }
                if (txtNuevoC.value == "") { alert("DEBE AGREGAR UN NOMBRE AL CARGO"); txtNuevoC.focus(); return false; }
                if (confirm("DESEA AGREGAR EL CARGO: " + txtNuevoC.value)) {
                    return true;
                }
            }
            return false;
        }

        function habilitarBotonEdicion(control, nombreControlEditar) {
            var ibtEditar = document.getElementById(master + nombreControlEditar);
            if (control.value == "0") {
                ibtEditar.disabled = true;
            } 
            else {
                ibtEditar.disabled = false;
            }
        }

        function validarNuevaCalMig() {
            var dropDownTitDep = document.getElementById(master + "ddlNuevoTitDep");
            var txtNuevoCalMigPri = document.getElementById(master + "txtNuevoCalMig");
            var txtNuevoCalMigSec = document.getElementById(master + "txtNuevoCalMigSec");
            var txtNuevoCalMigDefinicion = document.getElementById(master + "txtCalidadMigratoriaDef");
            if (dropDownTitDep != null & txtNuevoCalMigPri != null & txtNuevoCalMigSec != null & txtNuevoCalMigDefinicion != null) {
                if (txtNuevoCalMigPri.value == "") { alert("DEBE INGRESAR NOMBRE DE CALIDAD MIGRATORIA"); txtNuevoCalMigPri.focus(); return false; }
                if (txtNuevoCalMigDefinicion.value == "") { alert("DEBE INGRESAR UNA DEFINICION PARA LA CALIDAD MIGRATORIA"); txtNuevoCalMigDefinicion.focus(); return false; }
                if (dropDownTitDep.value == "0") { alert("DEBE SELECCIONAR LA CATEGORIA DE TITLAR/DEPENDIENTE"); dropDownTitDep.focus(); return false; }
                if (txtNuevoCalMigSec.value == "") { alert("DEBE INGRESAR UNA NOMBRE PARA EL CARGO"); txtNuevoCalMigSec.focus(); return false; }
                return true;
            }
            return false
        }

        function validarNUevoDocIdent() {
            var txtNuevoDocIdent = document.getElementById(master + "txtNuevoDocIdent");
            var txtNuevoDocIdentDesc = document.getElementById(master + "txtNuevoDocIdentDesc");
            if (txtNuevoDocIdent != null & txtNuevoDocIdentDesc != null) {
                if (txtNuevoDocIdent.value == "") { alert("DEBE INGRESAR NOMBRE DE TIPO DE DOCUMENTO"); txtNuevoDocIdent.focus(); return false; }
                if (txtNuevoDocIdentDesc.value == "") { alert("DEBE INGRESAR UNA DESCRIPCION"); txtNuevoDocIdentDesc.focus(); return false; }
                if (confirm("DESEA AGREGAR EL NUEVO DOCUMENTO DE IDENTIDAD: " + txtNuevoDocIdent.value + " (" + txtNuevoDocIdentDesc.value + ")")) {
                    return true;
                }
                
            }
            return false;
        }

        function mostrarTR(idTR) {
            var tr = document.getElementById(idTR);
            if (tr != null) {
                tr.style.display = "table-row";
            }
        }

        function ocultarTR(idTR) {
            var tr = document.getElementById(idTR);
            if (tr != null) {
                tr.style.display = "none";
            }
        }

        function bloquearNuevoDocIdent(accion, valor) {
            var hd1 = document.getElementById("idhdnAccionGuardar");
            //var hd1 = document.getElementById(master + "hdfldValorAccion");

            var dropDownDocIdent = document.getElementById(master + "ddlTipoDocDC");
            var ibtNuevoDI = document.getElementById(master + "itbNuevoDocIdent");
            var ibtEditarDI = document.getElementById(master + "ibtEditarDocIdent");
            var ibtAnularDI = document.getElementById(master + "ibtAnularDocIdent");

            var dropDownCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownTitDep = document.getElementById(master + "ddlTitDep");
            var dropDownCargo = document.getElementById(master + "ddlCalidadMigratoriaSec");
            var ibtNuevoCM = document.getElementById(master + "ibtNuevoCalMig");
            var ibtEditarCM = document.getElementById(master + "ibtEditarCalmig");
            var ibtAnularCM = document.getElementById(master + "ibtAnularCalmig");

            if (accion == "1") {
                if (dropDownDocIdent != null & ibtNuevoDI != null & ibtEditarDI != null & ibtAnularDI != null) {
                    var txtNuevoDocIdent = document.getElementById(master + "txtNuevoDocIdent");
                    var txtNuevoDocIdentDesc = document.getElementById(master + "txtNuevoDocIdentDesc");
                    txtNuevoDocIdent.value = "";
                    txtNuevoDocIdentDesc.value = "";
                    dropDownDocIdent.disabled = valor;
                    ibtNuevoDI.disabled = valor;
                    ibtEditarDI.disabled = valor;
                    ibtAnularDI.disabled = valor;
                    hd1.value = accion;
                }
            }
            if (accion == "2") {
                if (dropDownCalMig != null & dropDownTitDep != null & dropDownCargo != null & ibtNuevoCM != null & ibtEditarCM != null & ibtAnularCM != null) {
                    dropDownCalMig.disabled = valor;
                    dropDownTitDep.disabled = valor;
                    dropDownCargo.disabled = valor;
                    ibtNuevoCM.disabled = valor;
                    ibtEditarCM.disabled = valor;
                    ibtAnularCM.disabled = valor;
                    hd1.value = accion;
                }
            }
            if (accion == "3") {
                var txtNuevoDocIdent = document.getElementById(master + "txtNuevoDocIdent");
                var txtNuevoDocIdentDesc = document.getElementById(master + "txtNuevoDocIdentDesc");
                var lblNuevoDocIdentDesc = document.getElementById(master + "lblTipoDocDL");
                if (dropDownDocIdent != null & ibtNuevoDI != null & ibtEditarDI != null & ibtAnularDI != null & txtNuevoDocIdent != null & txtNuevoDocIdentDesc != null & lblNuevoDocIdentDesc != null) {
                    if (dropDownDocIdent.value != "0") {
                        mostrarTR('trNuevoDocIdent');
                        dropDownDocIdent.disabled = valor;
                        ibtNuevoDI.disabled = valor;
                        ibtEditarDI.disabled = valor;
                        ibtAnularDI.disabled = valor;
                        txtNuevoDocIdent.value = dropDownDocIdent.options[dropDownDocIdent.selectedIndex].text;
                        txtNuevoDocIdentDesc.value = lblNuevoDocIdentDesc.innerHTML;
                        hd1.value = accion;
                    }
                }
            }
        }

        function tabActual(valor) {
            var hd1 = document.getElementById(master + "hdfldTabActual");
            if (hd1 != null) {
                hd1.value = valor;
                cambiarTabs();
            }
        }
        function cambiarTabs() {
            var hd1 = document.getElementById(master + "hdfldTabActual");
            var contenedor = document.getElementById("tdContenedor");
            var tr1 = document.getElementById("trTabs");
            var tabPest = document.getElementById("tabPest" + hd1.value);
            var tab = document.getElementById("tab" + hd1.value);
            var n = 0;
            if (tab != null) {
                if (contenedor != null) {
                    var cantidad = contenedor.children.length
                    for (n = 1; n <= cantidad; n++) {
                        if (contenedor.children[n - 1].id == tab.id) {
                            tab.style.display = "block";
                            //alert(tab.id);
                        }
                        else {
                            contenedor.children[n - 1].style.display = "none";
                            //alert(contenedor.children[n - 1].id);
                        }
                    }
                    n = 0
                    cantidad = tr1.children.length;
                    for (n = 1; n <= cantidad; n++) {
                        if (tr1.children[n - 1].id == tabPest.id) {
                            tabPest.style.backgroundColor = "White";
                            tabPest.className = "Tabs";
                        }
                        else {
                            tr1.children[n - 1].style.backgroundColor = "#E6E6E6";
                            tr1.children[n - 1].className = "Tabs";
                        }
                    }
                }
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
                        <div>
                            <div id="divModal2" runat="server" style="position:absolute;z-index:1;display:none" class="modalBackgroundLoad"></div>
                            <div id="divCargando" runat="server" style="position:absolute;z-index:2;top:50%;left:45%" onblur="ContentPlaceHolder1_divCargando.focus();">
                                <img alt="gif" src="../../Imagenes/Gifs/ajax-loader(1).gif" style="width:100px;height:95px" />
                            </div>
                            <div id="divModal" runat="server" style="display:none" class="modalBackgroundLoad"></div>
                            <div id="divEditarCargo" runat="server" style="position:fixed;top:40%;left:40%;width:30%;height:15%;z-index:2;display:none;border-radius:10px" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EDITAR CARGO</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%">Sexo</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td style="width:40%;color:Red">
                                                            <asp:DropDownList runat="server" ID="ddlEditarSexo" CssClass="dropdownlist" Width="80%" ></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Titular / Dependiente</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlEditarTitDep" runat="server" CssClass="dropdownlist" Width="50%"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Cargo</td>
                                                        <td class="etiqueta">:</td>
                                                        <td class="etiqueta">
                                                            <asp:TextBox runat="server" ID="txteditarCargo" CssClass="textbox" Width="100%" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnGuardarCambioCargo" CssClass="ButtonFiltro" CommandName="guardarEditarCargo" OnClientClick="return validarEdicionCargo();" OnClick="guardarNuevo" Text="Guardar Cambios" Width="150px" />
                                                            <asp:Button runat="server" ID="btnCancelarCargo" CssClass="ButtonFiltro" OnClientClick="ocultarDiv('divModal');ocultarDiv('divEditarCargo');return false" Text="Cancelar" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEditarDocIdent" runat="server" style="position:fixed;top:40%;left:40%;width:30%;height:15%;z-index:2;display:none;border-radius:10px" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EDITAR DOCUMENTO DE IDENTIDAD</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%">Descripcion Corta</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td class="etiqueta">
                                                            <asp:TextBox runat="server" ID="txtEditarDescCorta" CssClass="textbox" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%">Descripcion Larga</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td class="etiqueta">
                                                            <asp:TextBox runat="server" ID="txtEditarDescLarga" CssClass="textbox" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnGuardarCambioDocIdent" CssClass="ButtonFiltro" CommandName="guardarEditarDocIdent" OnClientClick="return validarEdicionDocIdent();" OnClick="guardarNuevo" Text="Guardar Cambios" Width="150px" />
                                                            <asp:Button runat="server" ID="btnCancelarDocIdent" CssClass="ButtonFiltro" OnClientClick="ocultarDiv('divModal');ocultarDiv('divEditarDocIdent');return false" Text="Cancelar" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEditarinstitucion" runat="server" style="position:fixed;top:40%;left:40%;width:40%;height:15%;z-index:2;display:none;border-radius:10px" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EDITAR INSTITUCIÓN</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Categoria Institución</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlEditarCategoriaInst" runat="server" CssClass="dropdownlist" Width="80%"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Nombre Institucion</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarNombreInst" runat="server" CssClass="textbox" Width="65%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Siglas Institución</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarSiglasInst" runat="server" CssClass="textbox" Width="45%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="Button1" CssClass="ImagenBotonGuardar" CommandName="guardarEditarInstitucion" OnClientClick="return validarEditarInstitucion();" OnClick="guardarNuevo" Text="Guardar Cambios" Width="150px" />
                                                            <asp:Button runat="server" ID="Button2" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divModal');ocultarDiv('divEditarinstitucion');return false" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                </fieldset>
                            </div>
                            <div id="divEditarSolicitante" runat="server" style="position:fixed;top:40%;left:40%;width:35%;height:15%;z-index:2;display:none;border-radius:10px" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EDITAR SOLICITANTE</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Primer Apellido</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarPriApeSoli" runat="server" CssClass="textbox" 
                                                                Width="65%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Segundo Apellido</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarSegApeSoli" runat="server" CssClass="textbox" 
                                                                Width="65%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Nombres</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarNombresSoli" runat="server" CssClass="textbox" 
                                                                Width="65%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Tipo Identificación</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlEditarTipoDocIdentSoli" runat="server" 
                                                                CssClass="dropdownlist" Width="80%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Numero Identificacion</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarNumIdentSoli" runat="server" CssClass="textbox" Width="65%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Telefono</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarTelefonoSoli" runat="server" CssClass="textbox" Width="45%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="Button3" CssClass="ImagenBotonGuardar" CommandName="guardarEditarSolicitante" OnClientClick="return validarEditarSolicitante();" OnClick="guardarNuevo" Text="Guardar Cambios" Width="150px" />
                                                            <asp:Button runat="server" ID="Button4" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divModal');ocultarDiv('divEditarSolicitante');return false" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding: 15px 0 10px 0;" align="center">
                                                            <asp:Label runat="server" ID="lblMensajeEditarSoli" CssClass="labelInfo" ForeColor="Red" Text="El solicitante puede tener actas de recepción o conformidad firmadas con los datos anteriores."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEditarMensajeEstado" runat="server" style="position:fixed;top:40%;left:40%;width:40%;height:15%;z-index:2;display:none;border-radius:10px" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EDITAR MENSAJE</legend>
                                                <table class="AnchoTotal">
                                                        <tr>
                                                        <td class="etiqueta" style="width:25%;">
                                                            Estado de Carnet </td>
                                                        <td>
                                                            <asp:HiddenField ID="hOperacion" runat="server" />
                                                            <asp:DropDownList ID="ddlEstadoCarnet" runat="server" 
                                                                CssClass="dropdownlist" Width="80%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">
                                                            Mensaje</td>
                                                        <td>
                                                            <asp:TextBox ID="txtMensaje" runat="server" CssClass="textbox" Width="80%" MaxLength="249" TextMode="MultiLine" Mode="multiline" Wrap="true" height="35px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;"></td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            &nbsp;&nbsp;&nbsp;
                                                            
                                                            <asp:Button ID="Button5" runat="server"    ImageUrl="~/Imagenes/Iconos/add.png"  OnClick="grabarMensajeEstado" OnClientClick="return validarNuevaMensaje();"  Text="Grabar" Width="150px" Height="30"/>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="ImageButtons" runat="server"   ImageUrl="~/Imagenes/Iconos/disconforme.png"  OnClientClick="ancelarEditMensaje();return false;" Text="Cancelar" Width="150px" Height="30"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>
                    </td>
                </tr>
            </table>
            <table class="AnchoTotal">
                <tr>
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td style="width:100%">
                                    <table style="width:100%">
                                        <tr ID="trTabs">
                                        <td ID="tabPest0" class="Tabs" onclick="tabActual(0);" style="display:block;">Tipo Documento Identidad</td>
                                        <td ID="tabPest1" class="Tabs" onclick="tabActual(1);" style="display:block;">Calidad Migratoria</td>
                                        <td ID="tabPest2" class="Tabs" onclick="tabActual(2);" style="display:block;">Insituciones</td>
                                        <td ID="tabPest3" class="Tabs" onclick="tabActual(3);" style="display:block;">Solicitantes</td>
                                        <td ID="tabPest4" class="Tabs" onclick="tabActual(4);" style="display:block;">Mensaje Por Estado</td>
                                        <%--<td id="tabPest2" class="Tabs" style="display:block;" onclick="tabActual(2);">Cargos</td>--%>
                                        
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="AnchoTotal">
                            <tr>
                                <td ID="tdContenedor">
                                    <div id="tab0" style="display:none;">
                                        <fieldset class="Fieldset" style="width:40%">
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td class="etiqueta" style="width:25%">Descripcion Corta</td>
                                                    <td class="etiqueta" style="width:5%">:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNuevoDocIdent" runat="server" CssClass="textbox" Width="50%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%">Descripcion Larga</td>
                                                    <td class="etiqueta" style="width:5%">:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNuevoDocIdentDesc" runat="server" CssClass="textbox" Width="50%"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtNuevoDocIdent" runat="server" CommandName="agregarDocIdent" ImageUrl="~/Imagenes/Iconos/add.png" OnClick="guardarNuevo" OnClientClick="return validarNUevoDocIdent();" ToolTip="Agregar Nuevo Cargo" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div style="width:98%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:40%;height:25px">Descripción Corta</td>
                                                                    <td class="CabeceraGrilla" style="width:40%;height:25px">Descripción Larga</td>
                                                                    <td class="CabeceraGrilla" style="width:20%">Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="div2" runat="server" class="Scroll" style="width:98%;">
                                                            <asp:GridView ID="gvDocIdent" runat="server" 
                                                                AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                ShowHeader="False" style="margin-top: 0px" Width="100%">
                                                                <RowStyle CssClass="FilaDatos" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="DescripcionCorta" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="left" ItemStyle-Width="40%" />
                                                                    <asp:BoundField DataField="DescripcionLarga" ItemStyle-HorizontalAlign="left" ItemStyle-Width="40%" />
                                                                    <asp:TemplateField HeaderText="Envios" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="editarDocIdent" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" OnClick="guardarNuevo" ToolTip="Bloquear Usuario" />
                                                                                        <asp:ImageButton ID="ibtAnular" runat="server" CommandName="blockUser" ImageUrl="~/Imagenes/Iconos/Delete.png" ToolTip="Bloquear Usuario" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div id="tab1" style="display:none;">
                                        <fieldset class="Fieldset" style="width:40%">
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td class="etiqueta">
                                                        Sexo</td>
                                                    <td style="width:40%;color:Red">
                                                        <asp:DropDownList ID="ddlSexo" runat="server" AutoPostBack="true" CssClass="dropdownlist" OnSelectedIndexChanged="seleccionarSexo" Width="50%"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%">Calidad Migratoria</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCalidadMigratoriaPri" runat="server" AutoPostBack="true" CssClass="dropdownlist" OnSelectedIndexChanged="seleccionarCalidadMigratoria" Width="50%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">
                                                        Titular / Dependiente</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTitDep" runat="server" AutoPostBack="true" 
                                                            CssClass="dropdownlist" Enabled="true" 
                                                            OnSelectedIndexChanged="seleccionarCalidadMigratoria" Width="50%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">
                                                        Nuevo Cargo</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNuevoCargo" runat="server" CssClass="textbox" Width="50%"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="agregarCargo" 
                                                            ImageUrl="~/Imagenes/Iconos/add.png" OnClick="guardarNuevo" 
                                                            OnClientClick="return validarNuevoCargo();" ToolTip="Agregar Nuevo Cargo" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div style="width:98%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:80%;height:25px">
                                                                        Cargo</td>
                                                                    <td class="CabeceraGrilla" style="width:20%">
                                                                        Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="div1" runat="server" class="Scroll" style="width:98%;">
                                                            <asp:GridView ID="gvCargos" runat="server" 
                                                                AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                ShowHeader="False" style="margin-top: 0px" Width="100%">
                                                                <RowStyle CssClass="FilaDatos" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Nombre" ItemStyle-Height="25px" 
                                                                        ItemStyle-HorizontalAlign="left" ItemStyle-Width="80%" />
                                                                    <asp:TemplateField HeaderText="Envios" ItemStyle-HorizontalAlign="Center" 
                                                                        ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="editarCargo" 
                                                                                            ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" OnClick="guardarNuevo" 
                                                                                            ToolTip="Bloquear Usuario" />
                                                                                        <asp:ImageButton ID="ibtAnular" runat="server" CommandName="anularCargo" 
                                                                                            ImageUrl="~/Imagenes/Iconos/Delete.png" ToolTip="Bloquear Usuario" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <%--<div id="tab2" style="display:none;">
                                            <fieldset class="Fieldset" style="width:40%">
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Calidad Migratoria</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditarCalMig" runat="server" CssClass="textbox" Width="50%" MaxLength="100"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                                ImageUrl="~/Imagenes/Iconos/save16.png" OnClientClick="return validarNuevaCalMig();" OnClick="guardarNuevo" ToolTip="Guardar" />
                                                            &nbsp;&nbsp;
                                                            <asp:ImageButton ID="ImageButton2" runat="server" 
                                                                ImageUrl="~/Imagenes/Iconos/disconforme.png" OnClientClick="ocultarTR('trNuevaCalMig');bloquearNuevoDocIdent('2',false);return false;" ToolTip="Cancelar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Definición</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEditarCalMigDef" CssClass="textboxMultilineaDef" Width="100%" TextMode="MultiLine" Height="100px" MaxLength="1000" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:25%;">Nuevo Cargo</td>
                                                        <td>
                                                            <asp:TextBox ID="txtNuevoCargoNO" runat="server" CssClass="textbox" Width="50%" MaxLength="100"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                            <asp:ImageButton ID="ImageButton3" runat="server" 
                                                                ImageUrl="~/Imagenes/Iconos/add.png" OnClientClick="return validarNuevaCalMig();" OnClick="guardarNuevo" ToolTip="Guardar" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div style="padding-left:10px;width:97%;">
                                                                <table class="AnchoTotal">
                                                                    <tr>
                                                                        <td class="CabeceraGrilla" style="width:60%;height:25px">Cargo</td>
                                                                        <td class="CabeceraGrilla" style="width:20%;height:25px">Titular / Dependiente</td>
                                                                        <td class="CabeceraGrilla" style="width:20%">Opciones</td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="divGridView" class="Scroll" runat="server" style="padding-left:10px;width:97%;">
                                                                <asp:GridView ID="gvCargosNO" AutoGenerateColumns="false" runat="server" 
                                                                    style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" Width="100%">
                                                                    <RowStyle CssClass="FilaDatos"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Nombre" ItemStyle-Width="60%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="TitularDependiente" ItemStyle-Width="20%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:TemplateField ItemStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <table class="AnchoTotal">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="ibtDevolver" runat="server" ImageUrl="~/Imagenes/Iconos/editarregistro.png" CommandName="devolverRegistro" ToolTip="Devolver Registro" />
                                                                                            <asp:ImageButton ID="ibtAprobar" runat="server" ImageUrl="~/Imagenes/Iconos/Delete.png" CommandName="aprobarRegistro" ToolTip="Aprobar Registro"  />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>--%>
                                    <div id="tab2" style="display:none;">
                                        <fieldset class="Fieldset" style="width:40%">
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td class="etiqueta" style="width:25%;">
                                                        Categoria Institución</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMantCategoriaOfcoEx" runat="server" 
                                                            CssClass="dropdownlist" Width="80%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%;">
                                                        Nombre Institucion</td>
                                                    <td>
                                                        <asp:TextBox ID="txtMantNombre" runat="server" CssClass="textbox" Width="65%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%;">
                                                        Siglas Institución</td>
                                                    <td>
                                                        <asp:TextBox ID="txtMantSiglas" runat="server" CssClass="textbox" Width="45%"></asp:TextBox>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtMantBuscarInst" runat="server" 
                                                            ImageUrl="~/Imagenes/Iconos/buscar1.png" OnClick="buscarInstitucion" 
                                                            ToolTip="Bloquear Usuario" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtMantAgregarInst" runat="server" 
                                                            CommandName="agregarInsitucion" ImageUrl="~/Imagenes/Iconos/add.png" 
                                                            OnClick="seleccionarRegistro" OnClientClick="return validarNuevaInstitucion();" 
                                                            ToolTip="Bloquear Usuario" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtMantLimpiarInst" runat="server" 
                                                            ImageUrl="~/Imagenes/Iconos/clear.png" 
                                                            OnClientClick="limpiarInstitucion();return false;" ToolTip="Bloquear Usuario" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="width:98%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:20%;height:25px">
                                                                        Categoria</td>
                                                                    <td class="CabeceraGrilla" style="width:40%;height:25px">
                                                                        Nombre</td>
                                                                    <td class="CabeceraGrilla" style="width:20%;height:25px">
                                                                        Siglas</td>
                                                                    <td class="CabeceraGrilla" style="width:10%">
                                                                        Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="div4" runat="server" class="Scroll" style="width:99%;">
                                                            <asp:GridView ID="gvMantOfcoEx" runat="server" 
                                                                AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                ShowHeader="False" style="margin-top: 0px" Width="100%">
                                                                <RowStyle CssClass="FilaDatos" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ConCategoria" ItemStyle-Height="25px" 
                                                                        ItemStyle-HorizontalAlign="left" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Nombre" ItemStyle-HorizontalAlign="left" 
                                                                        ItemStyle-Width="40%" />
                                                                    <asp:BoundField DataField="Siglas" ItemStyle-HorizontalAlign="left" 
                                                                        ItemStyle-Width="20%" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="editarInstitucion" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" OnClick="seleccionarRegistro" ToolTip="Editar Institución" />
                                                                                        <%--<asp:ImageButton ID="ibtAnular" runat="server" CommandName="blockUser" ImageUrl="~/Imagenes/Iconos/Delete.png" ToolTip="Bloquear Usuario" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" colspan="2">
                                                        <asp:Label ID="lblMantTotalInst" runat="server" Text="Total Registros: "></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div id="tab3" style="display:block;">
                                        <fieldset class="Fieldset" style="width:45%">
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td>
                                                        <table style="width:80%;">
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Primer Apellido</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMantPriApeSol" runat="server" CssClass="textbox" 
                                                                        Width="65%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Segundo Apellido</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMantSegApeSol" runat="server" CssClass="textbox" 
                                                                        Width="65%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Nombres</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMantNombresSol" runat="server" CssClass="textbox" 
                                                                        Width="65%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Tipo Identificación</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlMantTipoDodIdent" runat="server" 
                                                                        CssClass="dropdownlist" Width="80%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Numero Identificacion</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMantNumDocIdentSol" runat="server" CssClass="textbox" Width="65%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Telefono</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMantTelefonoSol" runat="server" CssClass="textbox" 
                                                                        Width="45%"></asp:TextBox>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ibtMantBuscarSolicitante" runat="server" 
                                                                        ImageUrl="~/Imagenes/Iconos/buscar1.png" OnClick="buscarSolicitante" 
                                                                        ToolTip="Bloquear Usuario" />
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ibtMantAgregarSolicitante" runat="server" 
                                                                        CommandName="agregarSolicitante" ImageUrl="~/Imagenes/Iconos/add.png" 
                                                                        OnClick="seleccionarRegistro" OnClientClick="return validarNuevoSolicitante();" 
                                                                        ToolTip="Bloquear Usuario" />
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ibtMantLimpiarSolicitante" runat="server" 
                                                                        ImageUrl="~/Imagenes/Iconos/clear.png" 
                                                                        OnClientClick="limpiarSolicitante();return false;" ToolTip="Bloquear Usuario" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="width:98%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:45%;height:25px">Solicitante</td>
                                                                    <td class="CabeceraGrilla" style="width:20%;height:25px">Tipo Identificacion</td>
                                                                    <td class="CabeceraGrilla" style="width:15%;height:25px">Numero identificacion</td>
                                                                    <td class="CabeceraGrilla" style="width:10%;height:25px">Telefono</td>
                                                                    <td class="CabeceraGrilla" style="width:10%">Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="div5" runat="server" class="Scroll" style="width:99%;">
                                                            <asp:GridView ID="gvMantSolicitante" runat="server" 
                                                                AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                ShowHeader="False" style="margin-top: 0px" Width="100%">
                                                                <RowStyle CssClass="FilaDatos" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ConNombreCompleto" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="left" ItemStyle-Width="45%" />
                                                                    <asp:BoundField DataField="ConTipoDocIdent" ItemStyle-HorizontalAlign="left" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="NumeroDocumentoIdentidad" ItemStyle-HorizontalAlign="left" ItemStyle-Width="15%" />
                                                                    <asp:BoundField DataField="Telefono" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="editarSolicitante" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" OnClick="seleccionarRegistro" ToolTip="Editar Solicitante" />
                                                                                        <%--<asp:ImageButton ID="ibtAnular" runat="server" CommandName="blockUser" ImageUrl="~/Imagenes/Iconos/Delete.png" ToolTip="Bloquear Usuario" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" colspan="2">
                                                        <asp:Label ID="lblMantTotalSoli" runat="server" Text="Total Registros: "></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div id="tab4" style="display:none;">
                                        <fieldset class="Fieldset" style="width:40%">
                                            <table class="AnchoTotal" >
                                               <tr><td><div align="right" style="padding-right:2px;">
                                                   <asp:ImageButton ID="ImageButton1" runat="server" 
                                                            ImageUrl="~/Imagenes/Iconos/add.png" 
                                                            OnClick="btnNuevoMensaje_click" 
                                                            ToolTip="Nuevo" />
                                                   </div>
                                                   </td>
                                               </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="width:100%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:20%;height:25px">
                                                                        Estado</td>
                                                                    <td class="CabeceraGrilla" style="width:50%;height:25px">
                                                                        Mensaje</td>
                                                                    <td class="CabeceraGrilla" style="width:20%;height:25px">
                                                                        Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div ID="div3" runat="server" class="Scroll" style="width:100%;">
                                                            <asp:GridView ID="gvEstadoMensaje" runat="server" 
                                                                AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                ShowHeader="False" style="margin-top: 0px" Width="100%">
                                                                <RowStyle CssClass="FilaDatos" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Estadoid"   ItemStyle-HorizontalAlign="left" ItemStyle-Width="15%" ItemStyle-Height="25px" Visible="false"/>
                                                                    <asp:BoundField DataField="EstadoDesc" ItemStyle-HorizontalAlign="left"  ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Mensaje"    ItemStyle-HorizontalAlign="left"   ItemStyle-Width="50%" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandName="editarMensajeEstado" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" OnClick="seleccionarRegistro" ToolTip="Editar Mensaje" />
                                                                                        <asp:ImageButton ID="ibtAnular" runat="server" CommandName="eliminarMensajeEstado" ImageUrl="~/Imagenes/Iconos/Delete.png"   OnClick="seleccionarRegistro" ToolTip="Eliminar Mensaje" Visible="false"/>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <input type="hidden" id="idhdnAccionGuardar" name="hdnAccionGuardar" />
            <asp:HiddenField ID="hdfldValorAccion" runat="server"/>
            <asp:HiddenField ID="hdfldTabActual" runat="server"/>
            <asp:HiddenField runat="server" ID="hdrbt" Value="0" />
            <asp:HiddenField ID="TKSEGENC" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
