<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="RecepcionEntradaRegistro.aspx.cs" Inherits="SolCARDIP.Paginas.Recepcion.RecepcionEntradaRegistro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var master = "ContentPlaceHolder1_";
    var controlFoco;
    function validarControles() {
        var controlCategoria = document.getElementById(master + "ddlCategoriaOfcoEx");
        var controlMision = document.getElementById(master + "ddlMision");
        var controlTipoEntrada = document.getElementById(master + "ddlTipoEntrada");
        var controlCantidad = document.getElementById(master + "txtCantReg");
        var controlDocReferencia = document.getElementById(master + "txtDocReferencia");

        if (controlCategoria == null) { return false; }
        if (controlMision == null) { return false; }
        if (controlTipoEntrada == null) { return false; }
        if (controlCantidad == null) { return false; }
        if (controlDocReferencia == null) { return false; }

        if (controlCategoria.value == "0") {mensajeControlVacio(1); controlCategoria.focus(); return false; }
        if (controlMision.value == "0") { mensajeControlVacio(2); controlMision.focus(); return false; }
        if (controlTipoEntrada.value == "0") { mensajeControlVacio(3); controlTipoEntrada.focus(); return false; }
        if (controlCantidad.value == "" | parseInt(controlCantidad.value) <= 0) { mensajeControlVacio(4); controlCantidad.focus(); return false; }
        if (controlDocReferencia.value == "") { mensajeControlVacio(5); controlDocReferencia.focus(); return false; }
        return true;
    }

    function mensajeControlVacio(control) {
        var td;
        var mensaje = "";
        for (i = 1; i <= 5; i++) {
            td = document.getElementById("Mensaje" + i);
            if (td != null) {
                td.innerHTML = "";
            }
        }
        if (control == 1) { td = document.getElementById("Mensaje" + control); mensaje = "DEBE SELECCIONAR UNA CATEGORIA"; }
        if (control == 2) { td = document.getElementById("Mensaje" + control); mensaje = "DEBE SELECCIONAR UNA INSTITUCIÓN"; }
        if (control == 3) { td = document.getElementById("Mensaje" + control); mensaje = "DEBE SELECCIONAR LA OFICINA QUE RECEPCIONA LA DOCUMENTACIÓN"; }
        if (control == 4) { td = document.getElementById("Mensaje" + control); mensaje = "DEBE INDICAR LA CANTIDAD DE SOLICITUDES"; }
        if (control == 5) { td = document.getElementById("Mensaje" + control); mensaje = "DEBE INDICAR UN DOCUMENTO DE REFERENCIA"; }
        if (td != null) {
            td.innerHTML = mensaje;
        }
    }

    function mensajeControlOK(control) {
        var td;
        var mensaje = "";
        if (control == 1) { td = document.getElementById("Mensaje" + control);}
        if (control == 2) { td = document.getElementById("Mensaje" + control);}
        if (control == 3) { td = document.getElementById("Mensaje" + control);}
        if (control == 4) { td = document.getElementById("Mensaje" + control);}
        if (control == 5) { td = document.getElementById("Mensaje" + control); }
        if (td != null) {
            td.innerHTML = mensaje;
        }
    }

    function validarGuardar() {
        var controles = validarControles();
        if (controles) {
            if (confirm("¿DESEA GUARDAR EL REGISTRO DE ENTRADA?")) {
                return true;
            }
        }
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <table class="AnchoTotal">
                <tr>
                    <td>
                        <fieldset class="Fieldset">
                            <legend class="FieldsetLeyenda">Registro de Entrada</legend>
                            <table style="width:60%;">
                                <tr>
                                    <td class="etiqueta" style="width:20%;">N° de Cargo</td>
                                    <td style="width:40%;">
                                        <asp:Label runat="server" ID="lblNroCargo" CssClass="labelInfoFile" Font-Size="Large" Text="[ No Generado ]"></asp:Label>
                                    </td>
                                    <td style="width:40%;"></td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Categoría</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCategoriaOfcoEx" CssClass="dropdownlist" AutoPostBack="true" onchange="mensajeControlOK(1);"  OnSelectedIndexChanged="seleccionarCategoriaOficina" Width="60%"></asp:DropDownList>
                                    </td>
                                    <td id="Mensaje1" class="labelInfoControl"></td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" style="width:25%;">Intitución</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMision" onchange="mensajeControlOK(2);"  CssClass="dropdownlist" Width="100%"></asp:DropDownList>
                                    </td>
                                    <td id="Mensaje2" class="labelInfoControl"></td>
                                </tr>
                                <tr>
                                    <td class="etiqueta" style="width:25%;">Recepcionado por</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTipoEntrada" onchange="mensajeControlOK(3);" CssClass="dropdownlist" Width="60%"></asp:DropDownList>
                                    </td>
                                    <td id="Mensaje3" class="labelInfoControl"></td>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Cantidad de solicitudes</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCantReg" onkeyup="mensajeControlOK(4);"  CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td id="Mensaje4" class="labelInfoControl"></td>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtCantReg" Enabled="true" ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </tr>
                                <tr>
                                    <td class="etiqueta">Documento de Referencia</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDocReferencia" onkeyup="mensajeControlOK(5);"  CssClass="textbox" Width="60%"></asp:TextBox>
                                    </td>
                                    <td id="Mensaje5" class="labelInfoControl"></td>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtDocReferencia" Enabled="true" ValidChars="0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz">
                                    </cc1:FilteredTextBoxExtender>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Button runat="server" ID="btnGuardar" CssClass="ImagenBotonGuardar" Width="150px" Text="Guardar Entrada" OnClientClick="return validarGuardar();" />
                                        <asp:Button runat="server" ID="btnImprimir" CssClass="ImagenBotonPrint" Width="150px" Text="Imprimir Cargo" Visible="false" />
                                        <asp:Button runat="server" ID="btnLimpiar" CssClass="ImagenBotonNuevo" Width="150px" Text="Nueva Entrada"/>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
