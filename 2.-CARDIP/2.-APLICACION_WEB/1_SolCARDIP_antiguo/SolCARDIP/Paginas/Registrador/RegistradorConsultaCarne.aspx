<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="RegistradorConsultaCarne.aspx.cs" Inherits="SolCARDIP.Paginas.Registrador.RegistradorConsultaCarne"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Estilos/modal.css" rel="stylesheet" type="text/css" />
    <base href="RegistradorConsultaCarne.aspx" target="_self" />
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
        function validarCampoObservacion() {
            if (document.getElementById("ContentPlaceHolder1_txtMotivoObs").value === "") {
                 alert("Ingrese el Motivo de la Observación.")
                return false;
            } else {
                return true;
            }
        }

        window.onload = seguridadURLPrevia;
    </script>
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";
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

            var txtMesaPartes = document.getElementById(master + "txtMesaPartes");
            var txtFechaIni = document.getElementById(master + "txtFechaIni");
            var txtFechaFin = document.getElementById(master + "txtFechaFin");
            var ddlSexo = document.getElementById(master + "ddlSexo");
            var chkSolOnline = document.getElementById(master + "chkSolOnline");
            var DatosCarnet = document.getElementById(master + "DatosCarnet");
            var DatosFuncionario = document.getElementById(master + "DatosFuncionario");
            var DatosMision = document.getElementById(master + "DatosMision");
            
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


                chkSolOnline.checked = false;

                DatosCarnet.disabled = false;
                DatosFuncionario.disabled = false;
                DatosMision.disabled = false;                

                txtMesaPartes.value = "";
                txtFechaIni.value = "";
                txtFechaFin.value = "";
                ddlSexo.value = "0";
                
            }
        }
        function AbrirPopup() {
            document.getElementById('simpleModal_1').style.display = 'block';
        }
        function cerrarPopup() {
            document.getElementById('simpleModal_1').style.display = 'none';
        }

        function AbrirPopupRenovacion() {
            document.getElementById('modalRenovacion').style.display = 'block';
        }
        function cerrarPopupRenovacion() {
            document.getElementById('modalRenovacion').style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
            <div id="modalRenovacion" class="modal">
                <div class="modal-window">
                    <div class="modal-titulo">
                        <asp:ImageButton ID="ImageButton1" CssClass="close" ImageUrl="~/Imagenes/Iconos/cerrar.png"
                            OnClientClick="cerrarPopupRenovacion(); return false" runat="server" />
                        <span>DUPLICADO DE CARNET</span>
                    </div>
                    <div class="modal-cuerpo">
                        <h3>Datos de Carnet Asociado</h3>
                        <asp:Label ID="lblNroCarne" runat="server" Text="" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblPersona" runat="server" Text="" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblEstado" runat="server" Text="" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblFechaRegistro" runat="server" Text="" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblFechaEmision" runat="server" Text="" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblFechaVencimiento" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hEstadoIdPopup" runat="server" />
                    <asp:HiddenField ID="hCanetIdPupop" runat="server" />
                    <asp:HiddenField ID="hNumSolLinea" runat="server" />
                    <div class="modal-pie">
                        <asp:Button ID="btnGenerarDuplicadorPopup" runat="server"
                            CssClass="ImagenBotonGuardar" Text="       Generar Duplicado"
                            OnClick="btnGenerarDuplicadorPopup_Click" />
                    </div>
                </div>
            </div>

    
   <%-- <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>--%>

        <div id="simpleModal_1" class="modal">
            <div class="modal-window">
                <div class="modal-titulo">
                    <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Imagenes/Iconos/cerrar.png"
                        OnClientClick="cerrarPopup(); return false" runat="server" />
                    <span>VINCULAR A REGISTRO EN LINEA</span>
                </div>
                <div class="modal-cuerpo">
                    <h3>
                        Ingrese Número de registro en Linea</h3>
                    <asp:TextBox ID="txtNroRegistroLinea" placeholder="Registro en Linea" runat="server"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hEstadoId" runat="server" />
                <asp:HiddenField ID="hCanetId" runat="server" />
                <div class="modal-pie">
                    <asp:Button ID="btnGenerarDuplicador" runat="server" 
                        CssClass="ImagenBotonGuardar" Text="       Generar Duplicado" 
                        onclick="btnGenerarDuplicador_Click"/>
                </div>
            </div>
        </div>
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
                            <div id="divDerivar" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">DERIVAR REGISTRO - APROBACIÓN</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;"> SE DERIVARÁ EL REGISTRO AL ADMINISTRADOR</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Identificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDerivarIdentificador" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDerivarFuncionario" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">País (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDerivarPais" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Misión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDerivarMision" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDerivarCalMig" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnDerivarAceptar" CssClass="ImagenBotonOk" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="btnDerivarCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divDerivar');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divDerivarControlCalidad" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                                <table class="AnchoTotal">
                                    <tr>
                                        <td>
                                            <fieldset class="Fieldset">
                                                <legend class="FieldsetLeyenda">DERIVAR REGISTRO - CONTROL DE CALIDAD</legend>
                                                <table class="AnchoTotal">
                                                    <tr>
                                                        <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;"> SE DERIVARÁ EL REGISTRO PARA CONTROL DE CALIDAD</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta" style="width:30%">Identificador</td>
                                                        <td class="etiqueta" style="width:5%">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalIdentificador" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Funcionario</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalFuncionario" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">País (Nacionalidad)</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalPais" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Misión</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalMision" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                        <td class="etiqueta">:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblContCalCalMig" CssClass="labelInfo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:Button runat="server" ID="btnContCalAceptar" CssClass="ImagenBotonOk" OnClick="actualizarEstado" Text="Aceptar" Width="150px" />
                                                            <asp:Button runat="server" ID="btnContCalCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divDerivarControlCalidad');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
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
                                    <fieldset runat="server" id="DatosCarnet" class="Fieldset" style="height:200px;">
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
                                    <fieldset>
                                        <table>
                                            <tr>
                                                <td class="etiqueta">
                                                    <asp:CheckBox ID="chkSolOnline" AutoPostBack="true" runat="server" OnCheckedChanged="chkSolOnline_CheckedChanged" />
                                                    Buscar Solicitudes Online
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td class="etiqueta">Fecha Inicio</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaIni" CssClass="textbox" Width="70%" MaxLength="10" Text="" Enabled="False"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaIni" PopupButtonID="btnFechaIni" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="btnFechaIni" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" Enabled="False" ToolTip="Seleccione Fecha inicio" BorderWidth="0" />
                                                </td>
                                                <td class="etiqueta">Fecha Fin</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox" Width="70%" MaxLength="10" Text="" Enabled="False"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaFin" PopupButtonID="btnFechaFin" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="btnFechaFin" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" Enabled="False" ToolTip="Seleccione Fecha Fin" BorderWidth="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="vertical-align:top;width:25%;">
                                    <fieldset runat="server" id="DatosFuncionario" class="Fieldset" style="height:200px;">
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
                                    <fieldset runat="server" id="DatosMision" class="Fieldset" style="height:200px;">
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
                                    <div style="padding-left:20px;width:98%;" runat="server" id="cabeceraGrilla">
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
                                            style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" OnRowDataBound="verTemplate" Width="100%">
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
                                                                    <asp:ImageButton ID="ibtEditar" runat="server" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" CommandName="editarRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Editar Registro" Visible="false" />
                                                                    <asp:ImageButton ID="ibtDerivar" runat="server" ImageUrl="~/Imagenes/Iconos/derivar.png" CommandName="derivarRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Derivar Registro" Visible="false" />
                                                                    <asp:ImageButton ID="ibtRenovar" runat="server" ImageUrl="~/Imagenes/Iconos/renovar1.png" CommandName="renovarRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Editar para Renovar" Visible="false" />
                                                                    <asp:ImageButton ID="ibtControlCalidad" runat="server" ImageUrl="~/Imagenes/Iconos/calidad1.png" CommandName="controlCalidad" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Derivar a Control de Calidad" Visible="false" />
                                                                    <asp:ImageButton ID="ibtPDFDatos" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" CommandName="pdfInformacion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Reporte" Visible="true" />
                                                                    <asp:ImageButton ID="ibtMovimientos" runat="server" ImageUrl="~/Imagenes/Iconos/movimientos1.png" CommandName="verMovimientos" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver Movimientos" Visible="true" />
                                                                    <asp:ImageButton ID="ibtDuplicado" runat="server" ImageUrl="~/Imagenes/Iconos/genorpago.png" OnClick="AbrirPopup" ToolTip="Generar Duplicado" Visible="false" estadoId='<%# Eval("Estadoid")%>' CarnetCodigoID='<%# Eval("CarneIdentidadid")%>'/>
                                                                    <asp:ImageButton ID="ibtDerivarRegCom" runat="server" ImageUrl="~/Imagenes/Iconos/usuarioDeriva.png" CommandName="derivarRegistroCompleto" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Auto Derivar para Renovación" Visible="false" />
                                                                    
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:GridView ID="gdvSolicitudesLinea" runat="server"  CssClass="CabeceraGrillaNormal" Width="100%" Font-Names="Arial" Font-Size="10px"   
                                        EmptyDataText="No hay registros"  AutoGenerateColumns="False" OnRowDataBound="gdvSolicitudesLinea_rowDataBound" >
                                            <RowStyle CssClass="FilaDatos"/>
                                            <Columns>
                                                <asp:BoundField DataField="RELI_NUMERO" HeaderText="NUM.SOLICITUD">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_NOMBRE_COMPLETO" HeaderText="NOMBRE">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_ESTADO_DESC" HeaderText="ESTADO">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_TITDEPDESC" HeaderText="RELACION DEPENDENCIA">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_TIPOEMISION" HeaderText="TIPO DE EMISIÓN">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_VDP_MOTIVO_DUPLICADO" HeaderText="MOTIVO DUPLICADO">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NUMERO_CARNE" HeaderText="CARNET ASOCIADO">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RELI_FECHACREA" HeaderText="FECHA REGISTRO">    <%--3--%>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Resumen">   <%--18--%>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDescargar" runat="server" CommandName="Descargar" ImageUrl="~/Imagenes/Iconos/pdf.png" OnClick="descargarResumen" resumen='<%# Eval("RELI_VDP_RUTA_RESUMEN_ANEXOS")%>'/>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Atender">   <%--18--%>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgAtender" runat="server" ImageUrl="~/Imagenes/Iconos/certificar.png" OnClick="Atender" idRegLinea='<%# Eval("REGLINEA_ID")%>' numRegLinea='<%# Eval("RELI_NUMERO")%>' numeroCarne='<%# Eval("NUMERO_CARNE")%>' tipEmision ='<%# Eval("RELI_TIPOEMISION")%>' estado='<%# Eval("RELI_ESTADO_DESC")%>' Visible ="false"/>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Observar">   <%--18--%>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgObservar" runat="server" ImageUrl="~/Imagenes/Iconos/view.png" OnClick="ObservarRegistro" ToolTip="Observar" Visible="false"  idRegLinea='<%# Eval("REGLINEA_ID")%>' numRegLinea='<%# Eval("RELI_NUMERO")%>' numeroCarne='<%# Eval("NUMERO_CARNE")%>' tipEmision ='<%# Eval("RELI_TIPOEMISION")%>'/>
                                                        <!--<asp:ImageButton ID="imgSubsanar" runat="server" ImageUrl="~/Imagenes/Iconos/view2.png"  OnClick="SubsanarRegistro" OnClientClick="mostrarCargando();" ToolTip="Subsanar" Visible="false" idRegLinea='<%# Eval("REGLINEA_ID")%>' numRegLinea='<%# Eval("RELI_NUMERO")%>' numeroCarne='<%# Eval("NUMERO_CARNE")%>' tipEmision ='<%# Eval("RELI_TIPOEMISION")%>'/>-->
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" CssClass="hidden" />
                                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                
                                                
                                            </Columns>
                                            </asp:GridView>

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
            <div id="divObservar" runat="server" style="position:fixed;top:40%;left:40%;width:33%;height:20%;z-index:2;display:none; background-color:#ffffff;" class="modalBackground1">
                <fieldset class="Fieldset"  style="height:95%" >
                    <legend class="FieldsetLeyenda">Observar</legend>
                    <table style="width:100%" border="0">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Motivo de la Observación:
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="lblRegIdentificador" runat="server" />
                                <asp:TextBox ID="txtMotivoObs" runat="server" style="width:95%; text-transform: uppercase;" MaxLength="249"></asp:TextBox> 

                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <div style="position: absolute; bottom: 5px;  text-align: center;width: 100%;" align="center">
                        <asp:Button runat="server" ID="Button1" CssClass="ImagenBotonOk" CommandName="observarDocumento"  OnClientClick="return validarCampoObservacion();" OnClick="actualizarEstadoObservado" Text="Observar" Width="120px" />
                        <asp:Button runat="server" ID="Button2" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divObservar');ocultarDiv('divModal');return false;" Text="Cancelar" Width="160px" />
                    </div>
                 </fieldset>
            </div>
            <asp:HiddenField runat="server" ID="hdrbt" Value="0" />
            <asp:HiddenField ID="TKSEGENC" runat="server" Visible="false" />
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
