<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" CodeBehind="AdministradorCarneConsulta.aspx.cs" Inherits="SolCARDIP.Paginas.Administrador.AdministradorCarneConsulta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <base href="AdministradorCarneConsulta.aspx" target="_self" />
    <style type="text/css">
        html, body
        {
            height:100%;
            margin:0;
        }
    </style>
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

    </script>
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function traerFechaEmision() {
            var txtFechaEmision = document.getElementById(master + "txtAprobarFechaEmision");
            var hd1 = document.getElementById(master + "hdfldFechaEmision");
            if (txtFechaEmision != null & hd1 != null) {
                var valorEmision = hd1.value;
                if (valorEmision == "-") {
                    txtFechaEmision.value = getToday();
                }
                else {
                    txtFechaEmision.value = hd1.value;
                }
            }            
        }

        function cambioFecha() {
            var dropDownTiempos = document.getElementById(master + "ddlTiempos");
            var txtFechaVencimiento = document.getElementById(master + "txtAprobarFechaVencimiento");
            if (!validaFechaDDMMAAAA(txtFechaVencimiento.value)) {
                tdFechaEscrita.innerHTML = "";
            }
            else {
                tdFechaEscrita.innerHTML = obtenerFechaEscrita(txtFechaVencimiento.value);
            }
            dropDownTiempos.value = "0";
        }

        function obtenerFechaVencimiento() {
            var txtFechaEmision = document.getElementById(master + "txtAprobarFechaEmision");
            var txtFechaVencimiento = document.getElementById(master + "txtAprobarFechaVencimiento");
            var dropDownTiempos = document.getElementById(master + "ddlTiempos");
            if (txtFechaEmision != null & txtFechaVencimiento != null & dropDownTiempos != null) {
                var ubicacion = dropDownTiempos.value.indexOf(" ");
                var tipo = dropDownTiempos.value.substring(0, parseInt(ubicacion));
                var numero = dropDownTiempos.value.substring(parseInt(ubicacion + 1), parseInt(dropDownTiempos.value.length));
                txtFechaVencimiento.value = sumarFechas(txtFechaEmision.value, tipo, numero);
                tdFechaEscrita.innerHTML = obtenerFechaEscrita(sumarFechas(txtFechaEmision.value, tipo, numero));
            }
        }

        function validarAprobar() {
            var txtFechaEmision = document.getElementById(master + "txtAprobarFechaEmision");
            var txtFechaVencimiento = document.getElementById(master + "txtAprobarFechaVencimiento");
            if (txtFechaEmision != null & txtFechaVencimiento != null) {

                // VALIDAR FECHA DE EMISION ------------------------------------------------------
                var fechaEmi = txtFechaEmision.value;
                var fechaVen = txtFechaVencimiento.value;

                if (!validaFechaDDMMAAAA(fechaEmi)) { alert("FECHA DE EMISION NO VALIDA. REVISE."); txtFechaEmision.focus(); return false; };
                if (!validaFechaDDMMAAAA(fechaVen)) { alert("FECHA DE VENCIMIENTO NO VALIDA. REVISE."); txtFechaVencimiento.focus(); return false; };

                var FechaActual = convertirFechas(getToday());
                var FechaEmision = convertirFechas(fechaEmi);
                var FechaVencimiento = convertirFechas(fechaVen);

//                if (FechaEmision != FechaActual) {
//                    alert("FECHA DE EMISION DEBE SER IGUAL A LA FECHA ACTUAL.");
//                    txtFechaEmision.value = getToday();
//                    //txtFechaVencimiento.focus();
//                    return false;
//                }

                //if (FechaVencimiento < FechaActual) {
                if (FechaVencimiento < FechaEmision) {
                    alert("FECHA DE VENCIMIENTO DEBE SER MAYOR O IGUAL A LA FECHA DE EMISION.");
                    txtFechaVencimiento.focus();
                    return false;
                }
                // ----------------------------------------------------------------------------------
                
                if (txtFechaEmision.value == "") { alert("DEBE INGRESAR UNA FECHA DE EMISION"); txtFechaEmision.focus(); return false; }
                if (txtFechaVencimiento.value == "") { alert("DEBE INGRESAR UNA FECHA DE VENCIMIENTO"); txtFechaVencimiento.focus(); return false; }
                return true;
            }
            return false  
        }

        function validarDevolver() {
            var dropDownTipoObs = document.getElementById(master + "ddlTipoObs");
            var txtDetalleObs = document.getElementById(master + "txtDetalleObs");
            if (dropDownTipoObs != null & txtDetalleObs != null) {
                if (dropDownTipoObs.value == "0") { alert("DEBE SELECCIONAR UN MOTIVO PARA LA DEVOLUCION");dropDownTipoObs.focus();  return false; }
                if (txtDetalleObs.value == "") { alert("DEBE INGRESAR UN DETALLE PARA LA DEVOLUCION"); txtDetalleObs.focus(); return false; }
                return true;
            }
            return false;
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

        window.onload = seguridadURLPrevia;
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
                            <div id="divAprobar" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">APROBAR REGISTRO</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;"> SE APROBARÁ EL REGISTRO PARA SU EMISIÓN (IMPRESIÓN)</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Identificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAprobarIdentificador" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAprobarFuncionario" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">País (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAprobarPais" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Misión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAprobarMision" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblAprobarCalMig" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Fecha de Emisión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAprobarFechaEmision" CssClass="textbox" Width="35%" MaxLength="10" Text="" Enabled="false" ></asp:TextBox>
                                                            
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAprobarFechaEmision" PopupButtonID="ibtAprobarFechaEmision" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                            <asp:ImageButton ID="ibtAprobarFechaEmision" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />
                                                            
                                                            <asp:ImageButton ID="ibtHoy" runat="server" ImageUrl="~/Imagenes/Iconos/hoy.png" OnClientClick="ContentPlaceHolder1_txtAprobarFechaEmision.value = getToday();return false;" ToolTip="Hoy" BorderWidth="0" />
                                                            <asp:ImageButton ID="ibtFechaEmisionAnt" runat="server" ImageUrl="~/Imagenes/Iconos/back.png" OnClientClick="traerFechaEmision();return false;" ToolTip="Ultima Fecha de Emision" BorderWidth="0" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAprobarFechaEmision" Enabled="true" ValidChars="1234567890/">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Fecha de Vencimiento</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAprobarFechaVencimiento" onkeyup="cambioFecha();" CssClass="textbox" Width="35%" MaxLength="10" Text=""></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtAprobarFechaVencimiento" PopupButtonID="ibtAprobarFechaVencimiento" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                            <asp:ImageButton ID="ibtAprobarFechaVencimiento" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />
                                                            <asp:DropDownList runat="server" ID="ddlTiempos" CssClass="dropdownlist" onchange="obtenerFechaVencimiento();" AutoPostBack="false" Width="25%"></asp:DropDownList>
                                                            <%--<asp:DropDownList runat="server" ID="ddlNumeros" CssClass="dropdownlist" Width="15%"></asp:DropDownList>--%>
                                                            <asp:ImageButton ID="ibtYear" runat="server" ImageUrl="~/Imagenes/Iconos/hoy.png" OnClientClick="obtenerFechaVencimiento();return false;" ToolTip="Hoy" BorderWidth="0" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAprobarFechaVencimiento" Enabled="true" ValidChars="1234567890/">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td colspan="2" class="etiqueta" style="color:Green;padding-top:10px;padding-bottom:10px" id="tdFechaEscrita"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnAprobarAceptar" CssClass="ImagenBotonOk" OnClientClick="return validarAprobar();" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="btnAprobarCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divAprobar');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divDevolver" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">DEVOLVER REGISTRO</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;">SE DEVOLVERÁ EL REGISTRO PARA MODIFICACIÓN</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Identificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDevolverIdetn" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDevolverFuncionario" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">País (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDevolverPais" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Misión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDevolverMision" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDevolverCalMig" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Motivo Observación</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlTipoObs" CssClass="dropdownlist" Width="100%"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" colspan="3">Detalle de Observación</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="txtDetalleObs" CssClass="textbox" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtDetalleObs" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnDevolverAceptar" CssClass="ImagenBotonOk" OnClientClick="return validarDevolver();" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="btnDevolverCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divDevolver');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEmitidoImpr" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">EMITIR REGISTRO</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;"> EL REGISTRO QUEDARÁ HABILITADO PARA SU CONSULTA EXTERNA</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Idetnificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmitirIdentificador" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmitirFuncionario" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Pais (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmitirPais" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Mision</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmitirMision" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEmitirCalMig" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnEmitirAceptar" CssClass="ImagenBotonOk" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="btnEmitirCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divEmitidoImpr');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divReimpresion" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">DUPLICADO CARNÉ</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;">SE HABILITARA PARA SU REIMPRESIÓN</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Identificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblIdentDupli" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFuncionarioDupli" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">País (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblPaisDupli" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Misión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblMisionDupli" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCalMigDupli" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Motivo</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlTipoDuplicado" CssClass="dropdownlist" Width="100%"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" colspan="3">Detalle</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="txtDetalleDuplicado" CssClass="textbox" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtDetalleObs" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="Button1" CssClass="ImagenBotonOk" OnClientClick="return validarDevolver();" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="Button2" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divReimpresion');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
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
                                                    <asp:ImageButton ID="ibtFechaEmision" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />
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
                                                    <asp:ImageButton ID="ibtFechaVen" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" BorderWidth="0" />
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
                                                <td class="CabeceraGrilla" style="width:9%">Calidad Migratoria</td>
                                                <td class="CabeceraGrilla" style="width:9%">Cargo</td>
                                                <td class="CabeceraGrilla" style="width:15%">Mision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Inscripción (Registro)</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Emision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Vencimiento</td>
                                                <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                                <%--<td class="CabeceraGrilla" style="width:3%">Otros</td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divGridView" class="Scroll" runat="server" style="padding-left:20px;width:98%;">
                                        <asp:GridView ID="gvCarne" AutoGenerateColumns="false" runat="server" 
                                            style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" OnRowDataBound="verTemplate" Width="100%">
                                            <RowStyle CssClass="FilaDatos"/>
                                            <Columns>
                                                <asp:BoundField DataField="ConIdent" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="IdentMesaPartes" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="CarneNumero" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConEstado" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFuncionario" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConPaisNacionalidad" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConCalidadMigratoria" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConCargo" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConOficinaConsularEx" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaInscripcion" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaEmision" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaVen" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtDevolver" runat="server" ImageUrl="~/Imagenes/Iconos/devolver.png" CommandName="devolverRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Devolver Registro" Visible="false" />
                                                                    <asp:ImageButton ID="ibtAprobar" runat="server" ImageUrl="~/Imagenes/Iconos/conforme.png" CommandName="aprobarRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Aprobar Registro" Visible="false" />
                                                                    <asp:ImageButton ID="ibtEmitidoImpr" runat="server" ImageUrl="~/Imagenes/Iconos/emitidoImpr.png" CommandName="emitidoRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Emitido (Impreso)" Visible="false" />
                                                                    <asp:ImageButton ID="ibtVerFoto" runat="server" ImageUrl="~/Imagenes/Iconos/verFoto.png" CommandName="verFotografia" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver Fotografia" Visible="false" />
                                                                    <asp:ImageButton ID="ibInhabilitar" runat="server" ImageUrl="~/Imagenes/Iconos/pausa.png" CommandName="inhabilitar" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Inhabilitar" Visible="false" />
                                                                    <asp:ImageButton ID="ibHabilitar" runat="server" ImageUrl="~/Imagenes/Iconos/play.png" CommandName="habilitar" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Habilitar" Visible="false" />
                                                                    <asp:ImageButton ID="ibtHabilitarEdicion" runat="server" ImageUrl="~/Imagenes/Iconos/editCorrect.png" CommandName="devolverRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Habilitar" Visible="false" />
                                                                    <asp:ImageButton ID="ibtPDFDatos" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" CommandName="pdfInformacion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Reporte" Visible="true" />
                                                                    <asp:ImageButton ID="ibtMovimientos" runat="server" ImageUrl="~/Imagenes/Iconos/movimientos1.png" CommandName="verMovimientos" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver Movimientos" Visible="true" />

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField ItemStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtReimpresion" runat="server" ImageUrl="~/Imagenes/Iconos/reimpresion.jpg" CommandName="reimpresion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Duplicado" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
            <asp:HiddenField runat="server" ID="hdfldFechaEmision" />
            <asp:HiddenField ID="TKSEGENC" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
