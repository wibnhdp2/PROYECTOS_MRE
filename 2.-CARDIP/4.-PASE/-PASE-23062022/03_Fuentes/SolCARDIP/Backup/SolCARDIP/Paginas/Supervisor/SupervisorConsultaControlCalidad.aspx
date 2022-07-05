<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="SupervisorConsultaControlCalidad.aspx.cs" Inherits="SolCARDIP.Paginas.Supervisor.SupervisorConsultaControlCalidad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        function validarConforme() {
            for (i = 1; i <= 15; i++) {
                var chk = document.getElementById("chkConCal" + i);
                if (chk != null) {
                    if (chk.checked == false) {
                        alert("HAY DATOS OBSERVADOS. REVISE.")
                        return false;
                    }
                }
                else {
                    alert("OCURRIO UN ERROR MIENTRAS SE OBTENIAN LOS VALORES.");
                    return false;
                }
            }
            var txtObsImpreso = document.getElementById(master + "txtObs16");
            if (txtObsImpreso.value.trim() != "") { alert("SE ESTAN OBSERVANDO DATOS. REVISE"); return false; }
            if (confirm("¿DESEA DAR CONFORMIDAD DEL CARNÉ?")) {
                return true;
            }
            return false;
        }

        function recuperarValores(controlCalidad) {
            var contador = 1;
            for (i = 0; i <= controlCalidad.length; i++) {
                var TR = document.getElementById("TR" + (contador));
                var chk = document.getElementById("chkConCal" + contador);
                var valor = controlCalidad.charAt(i);
                if (TR != null) {
                    if (valor != "|" & valor != "") {
                        if (valor == "1") {
                            TR.style.backgroundColor = "#9FF781"
                            chk.checked = true;
                        }
                        else {
                            TR.style.backgroundColor = "#F78181"
                            chk.checked = false;
                        }
                        contador++;
                    }
                }
            }
        }

        function validarControlCalidad() {
            var hd1 = document.getElementById(master + "hdfldControlCalidad");
            var valor = "";
            var contador = 0;
            if (hd1 != null) {
                for (i = 1; i <= 15; i++) {
                    var chk = document.getElementById("chkConCal" + i);
                    if (chk != null) {
                        if (chk.checked == true) { valor = valor + "|1"; }
                        else { valor = valor + "|0"; contador++ }
                    }
                    else {
                        alert("OCURRIO UN ERROR MIENTRAS SE OBTENIAN LOS VALORES.");
                        return false;
                    }
                }
            }
            else {
                alert("OCURRIO UN ERROR MIENTRAS SE OBTENIAN LOS VALORES.");
                return false;
            }
            var txtObsImpreso = document.getElementById(master + "txtObs16");
            if (contador == 0 & txtObsImpreso.value.trim() == "") {alert("NO SE ESTA OBSERVANDO NINGUN DATO IMPRESO.");return false;}
            var valorCompleto = valor.substring(1, valor.length)
            hd1.value = valorCompleto;
            if (validarTextoControlCalidad()) {
                if (confirm("¿DESEA OBSERVAR EL CARNÉ?")) {
                    return true;
                }
                return false;
            }
            else {
                return false;
            }
        }

        function validarTextoControlCalidad() {
            var hd1 = document.getElementById(master + "hdfldTextoConCal");
            var valor = "";
            if (hd1 != null) {
                for (i = 1; i <= 16; i++) {
                    var txt = document.getElementById(master + "txtObs" + i);
                    if (txt != null) {
                        if (txt.value == "") { valor = valor + "|"; }
                        else { valor = valor + "|" + txt.value; }
                    }
                    else {
                        alert("OCURRIO UN ERROR MIENTRAS SE OBTENIAN LOS VALORES.");
                        return false;
                    }
                }
            }
            else {
                alert("OCURRIO UN ERROR MIENTRAS SE OBTENIAN LOS VALORES.");
                return false;
            }
            var valorCompleto = valor.substring(1, valor.length)
            hd1.value = valorCompleto;
            return true;
        }

        function cambiarColorTR(control, id) {
            var TR = document.getElementById("TR" + id);
            if (TR != null) {
                if (control.checked) {
                    TR.style.backgroundColor = "#9FF781"
                }
                else {
                    TR.style.backgroundColor = "#F78181"
                }
            }
        }
        
        function limpiarControles() {
            var txtNumeroIdent = document.getElementById(master + "txtNumeroIdent");
            var txtNumeroCarne = document.getElementById(master + "txtNumeroCarne");
            var txtFechaEmision = document.getElementById(master + "txtFechaEmision");
            var txtFechaVenc = document.getElementById(master + "txtFechaVen");
            var txtApePat = document.getElementById(master + "txtApePat");
            var txtApeMat = document.getElementById(master + "txtApeMat");
            var txtNom = document.getElementById(master + "txtNomrbes");
            var dropDownPeriodo = document.getElementById(master + "ddlPeriodo");
            var dropDownCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownTitDep = document.getElementById(master + "ddlTitDep");
            var dropDownCargo = document.getElementById(master + "ddlCalidadMigratoriaSec");
            var dropDownEstado = document.getElementById(master + "ddlEstado");
            var dropDownPais = document.getElementById(master + "ddlNacionalidad");
            var dropDownCategoria = document.getElementById(master + "ddlCategoriaOfcoEx");
            var dropDownMision = document.getElementById(master + "ddlMision");
            var lblNacionalidad = document.getElementById(master + "lblNacionalidad");
            if (txtNumeroIdent != null & txtNumeroCarne != null & txtFechaEmision != null & txtFechaVenc != null & txtApePat != null & txtApeMat != null & txtNom != null &
            dropDownPeriodo != null & dropDownCalMig != null & dropDownTitDep != null & dropDownCargo != null & dropDownEstado != null & dropDownPais != null & dropDownCategoria != null & dropDownMision != null) {
                txtNumeroIdent.value = "";
                txtNumeroCarne.value = "";
                txtFechaEmision.value = "";
                txtFechaVenc.value = "";
                txtApePat.value = "";
                txtApeMat.value = "";
                txtNom.value = "";
                dropDownPeriodo.value = "0";
                dropDownCalMig.value = "0";
                dropDownTitDep.value = "0";
                dropDownCargo.value = "0";
                dropDownEstado.value = "0";
                dropDownPais.value = "0";
                dropDownCategoria.value = "0";
                dropDownMision.value = "0";
                lblNacionalidad.innerHTML = "";
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
                            <div id="divMovimientos" runat="server" style="position:fixed;top:35%;left:15%;width:65%;height:10%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset" >
                                                <legend class="FieldsetLeyenda">Movimientos de Registro</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <table class="AnchoTotal">
                                                                    <tr>
                                                                        <td class="CabeceraGrilla" style="width:7%;height:25px">Fecha Mov</td>
                                                                        <td class="CabeceraGrilla" style="width:7%">Hora Mov</td>
                                                                        <td class="CabeceraGrilla" style="width:15%">Oficina Mov</td>
                                                                        <td class="CabeceraGrilla" style="width:20%">Usuario Mov</td>
                                                                        <td class="CabeceraGrilla" style="width:13%">Estado</td>
                                                                        <td class="CabeceraGrilla" style="width:18%">Tipo Observacion</td>
                                                                        <td class="CabeceraGrilla" style="width:20%">Detalle Observacion</td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="div1" class="ScrollMovimientos" runat="server">
                                                                <asp:GridView ID="gvMovimientos" AutoGenerateColumns="false" runat="server" 
                                                                    style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" Width="100%">
                                                                    <RowStyle CssClass="FilaDatos" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="MovFecha" ItemStyle-Width="7%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovHora" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovOficina" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovUsuario" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovEstado" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovTipoObs" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="MovObsDesc" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdMovimientos" class="ButtonFiltro" align="right">
                                                            <input id="btnCerrarMovimientos" type="button" class="ButtonFiltro" style="width:150px" value="Cerrar Detalle" onblur="this.focus()" onclick="ocultarDiv('divMovimientos');ocultarDiv('divModal')" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divControlCalidad" runat="server" style="position:fixed;top:20%;left:20%;width:53%;height:45%;z-index:2;display:none;" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">CONTROL DE CALIDAD</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;">REVISAR LOS DATOS IMPRESOS Y CONTRASTAR CONTRA LO SOLICTADO</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:15%">Identificador</td>
                                                        <td class="etiqueta" style="width:2%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalIdentificador" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">N° Carné</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalNroCarne" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td style="width:50%;vertical-align:top;">
                                                                        <fieldset class="Fieldset">
                                                                            <legend class="FieldsetLeyenda">FRONTAL</legend>
                                                                            <table class="AnchoTotal">
                                                                                <tr id="TR1" class="trCalidad">
                                                                                    <td class="etiqueta" style="width:25%;">Calidad Migratoria</td>
                                                                                    <td class="etiqueta" style="width:5%;">:</td>
                                                                                    <td style="width:5%;">
                                                                                        <input class="checkbox" id="chkConCal1" type="checkbox" checked="checked" onchange="cambiarColorTR(this, 1);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs1" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR2" class="trCalidad">
                                                                                    <td class="etiqueta">Primer Apellido</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal2" type="checkbox" checked="checked" onchange="cambiarColorTR(this, 2);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs2" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR3" class="trCalidad">
                                                                                    <td class="etiqueta">Segundo Apellido</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal3" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 3);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs3" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR4" class="trCalidad">
                                                                                    <td class="etiqueta">Nombres</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal4" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 4);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs4" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR5" class="trCalidad">
                                                                                    <td class="etiqueta">Nacionalidad</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal5" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 5);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs5" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR6" class="trCalidad">
                                                                                    <td class="etiqueta">Fecha de Nacimiento</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal6" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 6);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs6" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR7" class="trCalidad">
                                                                                    <td class="etiqueta">Estado Civil</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal7" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 7);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs7" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR8" class="trCalidad">
                                                                                    <td class="etiqueta">Sexo</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal8" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 8);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs8" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR9" class="trCalidad">
                                                                                    <td class="etiqueta">Fecha Vencimiento</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal9" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 9);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs9" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR15" class="trCalidad">
                                                                                    <td class="etiqueta">Foto Titular</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal15" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 15);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs15" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR10" class="trCalidad">
                                                                                    <td class="etiqueta">Firma Titular</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal10" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 10);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs10" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                    <td style="vertical-align:top;">
                                                                        <fieldset class="Fieldset">
                                                                            <legend class="FieldsetLeyenda">REVERSO</legend>
                                                                            <table class="AnchoTotal">
                                                                                <tr id="TR11" class="trCalidad">
                                                                                    <td class="etiqueta" style="width:25%;">Institución</td>
                                                                                    <td class="etiqueta" style="width:5%;">:</td>
                                                                                    <td style="width:5%;">
                                                                                        <input class="checkbox" id="chkConCal11" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 11);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs11" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR12" class="trCalidad">
                                                                                    <td class="etiqueta">Cargo</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal12" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 12);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs12" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR13" class="trCalidad">
                                                                                    <td class="etiqueta">Domicilio</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal13" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 13);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs13" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TR14" class="trCalidad">
                                                                                    <td class="etiqueta">Fecha Emision</td>
                                                                                    <td class="etiqueta">:</td>
                                                                                    <td>
                                                                                        <input class="checkbox" id="chkConCal14" type="checkbox"checked="checked" onchange="cambiarColorTR(this, 14);" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtObs14" CssClass="textbox" Width="100%" MaxLength="32"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table class="AnchoTotal">
                                                                            <tr id="TR16">
                                                                                <td class="etiqueta" style="width:10%;">Impresion</td>
                                                                                <td class="etiqueta" style="width:2%;">:</td>
                                                                                <td colspan="2">
                                                                                    <asp:TextBox runat="server" ID="txtObs16" CssClass="textbox" Width="55%" MaxLength="32"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnContCalAceptar" CssClass="ImagenBotonOk" CommandName="emitido"  OnClientClick="return validarConforme();" OnClick="actualizarEstado" Text="Conforme" Width="120px" />
                                                            <asp:Button runat="server" ID="btnContCalRechazar" CssClass="ImagenBotonObservar" OnClientClick="return validarControlCalidad();" OnClick="actualizarEstado" Text="Observar" Width="120px" />
                                                            <asp:Button runat="server" ID="btnContCalCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divControlCalidad');ocultarDiv('divModal');return false;" Text="Cancelar" Width="160px" />
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
                <tr>
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Label runat="server" Text="Bandeja  de Registros" ID="lbltitulo" CssClass="titulo"></asp:Label>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:45%;vertical-align:top;">
                                    <fieldset class="Fieldset" style="height:200px;">
                                        <legend class="FieldsetLeyenda">Datos del Carne</legend>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="width:25%" class="etiqueta">Periodo</td>
                                                <td style="width:35%">
                                                    <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="dropdownlist" Width="42%"></asp:DropDownList>
                                                </td>
                                                <td style="width:17%" class="etiqueta">Estado</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEstado" CssClass="dropdownlist" AutoPostBack="true" Width="100%" Enabled="true"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Numero Solicitud (Registro)</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNumeroIdent" CssClass="textbox" Width="40%"></asp:TextBox>
                                                </td>
                                                <td class="etiqueta">Nro Mesa de Partes</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMesaPartes" CssClass="textbox" Width="70%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Numero de Carné</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNumeroCarne" CssClass="textbox" Width="40%"></asp:TextBox>
                                                </td>
                                                <td class="etiqueta">Fecha de Emision</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaEmision" CssClass="textbox" Width="70%" MaxLength="10" Text=""></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calendarEmision" runat="server" TargetControlID="txtFechaEmision" PopupButtonID="ibtFechaEmision" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="ibtFechaEmision" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Sexo</td>
                                                <td style="width:40%;">
                                                    <asp:DropDownList runat="server" ID="ddlSexo" CssClass="dropdownlist" AutoPostBack="true" Width="80%" OnSelectedIndexChanged="seleccionarSexo"></asp:DropDownList>
                                                </td>
                                                <td class="etiqueta">Fecha de Vencimiento</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaVen" CssClass="textbox" Width="70%" MaxLength="10" Text=""></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calendarVen" runat="server" TargetControlID="txtFechaVen" PopupButtonID="ibtFechaVen" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="ibtFechaVen" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Calidad Migratoria</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaPri" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="seleccionarCalidadMigratoria" Width="70%"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Titular / Dependiente</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlTitDep" CssClass="dropdownlist" AutoPostBack="true" Width="70%" Enabled="true" OnSelectedIndexChanged="seleccionarCalidadMigratoria"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Cargo</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaSec" CssClass="dropdownlist" AutoPostBack="false" Width="100%" Enabled="false"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="vertical-align:top;width:25%;">
                                    <fieldset class="Fieldset" style="height:200px;">
                                        <legend class="FieldsetLeyenda">Datos del Funcionario</legend>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="width:30%" class="etiqueta">Apellido Paterno</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtApePat" CssClass="textbox" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Apellido Materno</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtApeMat" CssClass="textbox" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Nombres</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNomrbes" CssClass="textbox" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Pais (Nacionalidad)</td>
                                                <td style="color:Red;">
                                                    <asp:DropDownList runat="server" ID="ddlNacionalidad" CssClass="dropdownlist" AutoPostBack="true" Width="60%" OnSelectedIndexChanged="obtenerNacionalidadPais"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:Label runat="server" ID="lblNacionalidad" CssClass="labelInfo" ForeColor="Green"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="vertical-align:top;">
                                    <fieldset class="Fieldset" style="height:200px;">
                                        <legend class="FieldsetLeyenda">Datos de la Mision</legend>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="width:10%;" class="etiqueta">Categoria</td>
                                                <td style="color:Red;width:70%">
                                                    <asp:DropDownList runat="server" ID="ddlCategoriaOfcoEx" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="seleccionarCategoriaOficina" Width="60%"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Misión</td>
                                                <td style="color:Red;">
                                                    <asp:DropDownList runat="server" ID="ddlMision" CssClass="dropdownlist" AutoPostBack="false" Width="60%" Enabled="false"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <%--<asp:Button runat="server" ID="prueba" Text="prueba" OnClientClick="mostrarCargando();return false;" />--%>
                                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="ImagenBotonBuscar" Width="150px" OnClientClick="mostrarCargando();" OnClick="buscarRegistros" />
                                    <asp:Button runat="server" ID="btnLimpiar" Text="Limpiar Controles" CssClass="ImagenBotonClean" OnClientClick="limpiarControles();return false;" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div style="padding-left:20px;width:98%;">
                                        <table class="AnchoTotal">
                                            <tr>
                                                <td class="CabeceraGrilla" style="width:5%;height:25px">Solicitud</td>
                                                <td class="CabeceraGrilla" style="width:5%">Nro Mesa Partes</td>
                                                <td class="CabeceraGrilla" style="width:4%">Numero Carné</td>
                                                <td class="CabeceraGrilla" style="width:8%">Estado</td>
                                                <td class="CabeceraGrilla" style="width:12%">Titular</td>
                                                <td class="CabeceraGrilla" style="width:8%">Pais Nacionalidad</td>
                                                <td class="CabeceraGrilla" style="width:10%">Calidad Migratoria</td>
                                                <td class="CabeceraGrilla" style="width:10%">Cargo</td>
                                                <td class="CabeceraGrilla" style="width:16%">Mision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Inscripción (Registro)</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Emision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Vencimiento</td>
                                                <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divGridView" class="Scroll" runat="server" style="padding-left:20px;width:98%;">
                                        <asp:GridView ID="gvCarne" AutoGenerateColumns="false" runat="server" 
                                            style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" Width="100%" OnRowDataBound="verTemplate">
                                            <RowStyle CssClass="FilaDatos"/>
                                            <Columns>
                                                <asp:BoundField DataField="ConIdent" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="IdentMesaPartes" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="CarneNumero" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConEstado" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFuncionario" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConPaisNacionalidad" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConCalidadMigratoria" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConCargo" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConOficinaConsularEx" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaInscripcion" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaEmision" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaVen" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtControlCalidad" runat="server" ImageUrl="~/Imagenes/Iconos/calidad1.png" CommandName="controlCalidad" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Abrir Control de Calidad" Visible="false" />
                                                                    <asp:ImageButton ID="ibtPDFDatos" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" CommandName="pdfInformacion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Reporte" Visible="true" />
                                                                    <asp:ImageButton ID="ibtMovimientos" runat="server" ImageUrl="~/Imagenes/Iconos/movimientos1.png" CommandName="verMovimientos" OnClientClick="mostrarCargando();" OnClick="seleccionarRegistro" ToolTip="Ver Movimientos" Visible="true" />
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
                                <td colspan="3">
                                    <div style="position:absolute; width:20%">
                                        <table>
                                            <tr>
                                                <td class="etiqueta" style="width:100%">
                                                    <asp:Label ID="lblTotalRegistros" runat="server" Text="Resultado de la busqueda: "></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td class="EtiquetaCentro" valign="top" style="height:30px">Pagina&nbsp;&nbsp;  <asp:DropDownList ID="ddlPaginas" runat="server" 
                                                AutoPostBack="true" Width="50" OnSelectedIndexChanged="cambiarPagina"></asp:DropDownList>&nbsp;&nbsp; <asp:Label ID="lblTotalPaginas" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdrbt" Value="0" />
            <asp:HiddenField ID="TKSEGENC" runat="server" Visible="false" />
            <asp:HiddenField ID="hdfldControlCalidad" runat="server" />
            <asp:HiddenField ID="hdfldTextoConCal" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
