<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="SupervisorConsulta.aspx.cs" Inherits="SolCARDIP.Paginas.Supervisor.SupervisorConsulta" %>
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

        function validarEntregaVarios() {
            var txtApePat = document.getElementById(master + "txtVarApePat");
            var txtNombres = document.getElementById(master + "txtVarNombres");
            var dropDownTipoDoc = document.getElementById(master + "ddlVarDocumentoIdent");
            var txtNumDoc = document.getElementById(master + "txtVarNumeroDocIdent");
            if (txtApePat != null & txtNombres != null & dropDownTipoDoc != null & txtNumDoc != null) {
                if (txtApePat.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO DE LA PERSONA"); txtApePat.focus(); return false; }
                if (txtNombres.value == "") { alert("DEBE INGRESAR EL NOMBRE DE LA PERSONA"); txtNombres.focus(); return false; }
                if (dropDownTipoDoc.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE DOCUMENTO DE IDENTIDAD"); dropDownTipoDoc.focus(); return false; }
                if (txtNumDoc.value == "") { alert("DEBE INGRESAR EL NUMERO DE DOCUMENTO DE IDENTIDAD"); txtNumDoc.focus(); return false; }
            }
            else {
                alert("OCURRIÓ UN ERROR AL OBTENER LOS DATOS");
                return false;
            }

            if (confirm("¿ DESEA GENERAR LA CONFORMIDAD PARA LOS REGISTROS DEL DETALLE ?")) {
                return true;
            }
            else {
                return false;
            }
            return false;
        }

        function validarEntrega() {
            var txtApePat = document.getElementById(master + "txtDEApePat");
            var txtNombres = document.getElementById(master + "txtDENombres");
            var dropDownTipoDoc = document.getElementById(master + "ddlDEDocumentoIdent");
            var txtNumDoc = document.getElementById(master + "txtDENumeroDocIdent");
            if (txtApePat != null & txtNombres != null & dropDownTipoDoc != null & txtNumDoc != null) {
                if (txtApePat.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO DE LA PERSONA"); txtApePat.focus(); return false; }
                if (txtNombres.value == "") { alert("DEBE INGRESAR EL NOMBRE DE LA PERSONA"); txtNombres.focus(); return false; }
                if (dropDownTipoDoc.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE DOCUMENTO DE IDENTIDAD"); dropDownTipoDoc.focus(); return false; }
                if (txtNumDoc.value == "") { alert("DEBE INGRESAR EL NUMERO DE DOCUMENTO DE IDENTIDAD"); txtNumDoc.focus(); return false; }
            }
            else {
                alert("OCURRIÓ UN ERROR AL OBTENER LOS DATOS");
                return false;
            }

            if (confirm("¿ DESEA GENERAR LA CONFORMIDAD PARA EL CARNÉ DE IDENTIDAD ?")) {
                return true;
            }
            else {
                return false;
            }
            return false;
        }

        function validarDerivar() {
            var dropDownRegistrador = document.getElementById(master + "ddlRegistrador");
            if (dropDownRegistrador != null) {
                if (dropDownRegistrador.value == "0") { alert("DEBE SELECCIONAR UN REGISTRADOR PARA DERIVAR"); dropDownRegistrador.focus(); return false; }
                return true;
            }
            return false;
        }

        function limpiarControles() {
            var txtNumeroIdent = document.getElementById(master + "txtNumeroIdent");
            var txtNumeroCarne = document.getElementById(master + "txtNumeroCarne");
            var txtFechaDesde = document.getElementById(master + "txtFechaDesde");
            var txtFechaHasta = document.getElementById(master + "txtFechaHasta");
            var txtApePat = document.getElementById(master + "txtApePat");
            var txtApeMat = document.getElementById(master + "txtApeMat");
            var txtNom = document.getElementById(master + "txtNombres");
            var dropDownPeriodo = document.getElementById(master + "ddlPeriodo");
            var dropDownCalMig = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDownSexo = document.getElementById(master + "ddlSexo");
//            var dropDownCargo = document.getElementById(master + "ddlCalidadMigratoriaSec");
            var dropDownEstado = document.getElementById(master + "ddlEstado");
            //var dropDownPais = document.getElementById(master + "ddlNacionalidad");
            var dropDownCategoria = document.getElementById(master + "ddlCategoriaOfcoEx");
            var dropDownMision = document.getElementById(master + "ddlMision");
            var dropDownTipoEntrada = document.getElementById(master + "ddlTipoEntrada");
            //var lblNacionalidad = document.getElementById(master + "lblNacionalidad");
            if (txtNumeroIdent != null & txtNumeroCarne != null & txtApePat != null & txtApeMat != null & txtNom != null & dropDownSexo != null &
            dropDownPeriodo != null & dropDownCalMig != null & dropDownEstado != null & dropDownCategoria != null & dropDownMision != null & txtFechaDesde != null & txtFechaHasta != null &
            dropDownTipoEntrada != null) {
                txtNumeroIdent.value = "";
                txtNumeroCarne.value = "";
                txtFechaDesde.value = "";
                txtFechaHasta.value = "";
                txtApePat.value = "";
                txtApeMat.value = "";
                txtNom.value = "";
                dropDownPeriodo.value = "0";
                dropDownCalMig.value = "0";
                dropDownSexo.value = "0";
                //dropDownCargo.value = "0";
                dropDownEstado.value = "0";
                //dropDownPais.value = "0";
                dropDownCategoria.value = "0";
                dropDownMision.value = "0";
                dropDownTipoEntrada.value = "0";
                //lblNacionalidad.innerHTML = "";
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
                        </div>
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
                        <div id="divDerivarRegCom" runat="server" style="position:fixed;top:35%;left:35%;width:30%;height:20%;z-index:2;display:none" class="modalBackground1">
                            <table class="AnchoTotal">
                                <tr>
                                    <td>
                                        <fieldset class="Fieldset">
                                            <legend class="FieldsetLeyenda">DERIVAR A REGISTRADOR</legend>
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;"> EL REGISTRO SE DERIVARÁ PARA COMPLETAR LA INFORMACIÓN</td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:30%">Idetnificador</td>
                                                    <td class="etiqueta" style="width:5%">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDRCIdentificador" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Titular del Carné</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDRCFuncionario" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Calidad Migratoria</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDRCCalMig" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Mision</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDRCMision" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Registrador</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlRegistrador" CssClass="dropdownlist" Width="80%"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="right">
                                                        <asp:Button runat="server" ID="btnEmitirAceptar" CssClass="ImagenBotonOk" Text="Aceptar" Width="150px" OnClientClick="return validarDerivar();" OnClick="actualizarEstado" />
                                                        <asp:Button runat="server" ID="btnEmitirCancelar" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divDerivarRegCom');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divDerivarEntrega" runat="server" style="position:fixed;top:25%;left:35%;width:35%;height:20%;z-index:2;display:none" class="modalBackground1">
                            <table class="AnchoTotal">
                                <tr>
                                    <td>
                                        <fieldset class="Fieldset">
                                            <legend class="FieldsetLeyenda">ENTREGA DE CARNÉ</legend>
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td align="right" colspan="3">
                                                        <asp:Label runat="server" ID="lblDETipoEntrada" CssClass="labelInfoFile" Font-Size="18px" Font-Bold="true" Font-Underline="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" colspan="3" style="padding: 12px 0px 12px 0px;text-decoration: underline;">SE ENTREGARÁ DE CARNÉ</td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%">Idetnificador</td>
                                                    <td class="etiqueta" style="width:5%">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDEIdentificador" CssClass="labelInfo" ></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Titular del Carné</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDEFuncionario" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Calidad Migratoria</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDECalMig" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Mision</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDEMision" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="padding: 12px 0px 12px 0px;text-decoration: underline;">DATOS DE RECOJO</td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta" style="width:25%">Documento de Identidad</td>
                                                    <td class="etiqueta" style="width:2%">:</td>
                                                    <td colspan="2">
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td style="width:40%;color:Red;">
                                                                    <asp:DropDownList runat="server" ID="ddlDEDocumentoIdent" CssClass="dropdownlist" AutoPostBack="false" Width="90%"></asp:DropDownList>
                                                                </td>
                                                                <td style="width:7%;text-align:right;" class="etiqueta">Numero</td>
                                                                <td class="etiqueta" style="width:2%">:</td>
                                                                <td style="width:35%;color:Red;">
                                                                    <asp:TextBox runat="server" ID="txtDENumeroDocIdent" CssClass="textbox" MaxLength="20" Width="50%" Text="213"></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtDENumeroDocIdent" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/-.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Imagenes/Iconos/buscarpersona.png" CommandName="solicitanteDE" OnClick="obtenerSolicitante" style="height: 15px" ToolTip="Busca un solicitante" />
                                                                    <%--&nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Imagenes/Iconos/copy.png" style="height: 15px" OnClientClick="copiarDatos();return false;" ToolTip="Copiar datos del Titular" />--%>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Imagenes/Iconos/clear.png" style="height: 15px" CommandName="solicitanteDE" OnClick="limpiarSolicitante" ToolTip="Limpiar Campos" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Primer Apellido</td>
                                                    <td class="etiqueta">:</td>
                                                    <td style="color:Red;" class="style2">
                                                        <asp:TextBox runat="server" ID="txtDEApePat" CssClass="textbox" Width="90%" Text="aaaaaa"></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDEApePat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Segundo Apellido</td>
                                                    <td class="etiqueta">:</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDEApeMat" CssClass="textbox" Width="90%" MaxLength="100" Text="aaaaaaaaa"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtDEApeMat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Nombres</td>
                                                    <td class="etiqueta">:</td>
                                                    <td style="color:Red;">
                                                        <asp:TextBox runat="server" ID="txtDENombres" CssClass="textbox" Width="90%" MaxLength="100" Text="aaaaaaaaa"></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtDENombres" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="etiqueta">Teléfono</td>
                                                    <td class="etiqueta">:</td>
                                                    <td style="color:Red;">
                                                        <asp:TextBox runat="server" ID="txtDETelefono" CssClass="textbox" Width="60%" Text="123123"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtDETelefono" Enabled="true" ValidChars="1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td class="etiqueta">Institucion</td>
                                                    <td class="etiqueta">:</td>
                                                    <td style="color:Red;">
                                                        <asp:Label runat="server" ID="lblDEInstitucion" CssClass="labelInfo"></asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td class="etiqueta" colspan="3" style="padding: 5px 0px 5px 0px;">Observación</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:TextBox runat="server" ID="txtDEObservacion" CssClass="textbox" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px" Text="dfsdf"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtDEObservacion" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚüÜ ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="right">
                                                        <asp:Button runat="server" ID="Button1" CssClass="ImagenBotonActaConformidad" OnClientClick="return validarEntrega();return false;" OnClick="generarActaConformidad" Text="Generar Conformidad" Width="180px" />
                                                        <asp:Button runat="server" ID="Button2" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divDerivarEntrega');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divVarios" runat="server" style="position:fixed;top:25%;left:8%;width:80%;height:45%;z-index:2;display:none" class="modalBackground1">
                            <table class="AnchoTotal">
                                <tr>
                                    <td>
                                        <fieldset class="Fieldset">
                                            <legend class="FieldsetLeyenda">Entrega Multiple</legend>
                                            <table class="AnchoTotal">
                                                <tr>
                                                    <td style="width:60%;vertical-align:top;">
                                                        <div style="padding-left:20px;width:95%;">
                                                            <table class="AnchoTotal">
                                                                <tr>
                                                                    <td class="CabeceraGrilla" style="width:5%;height:25px">N° Solicitud</td>
                                                                    <td class="CabeceraGrilla" style="width:10%;">Tipo Entrada</td>
                                                                    <td class="CabeceraGrilla" style="width:5%">Numero Carné</td>
                                                                    <td class="CabeceraGrilla" style="width:25%">Titular</td>
                                                                    <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="div2" class="Scroll" runat="server" style="padding-left:20px;width:95%;">
                                                            <asp:GridView ID="gvVariosEntrega" AutoGenerateColumns="false" runat="server" 
                                                                style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" Width="100%" EmptyDataText="No se agregaron registros">
                                                                <RowStyle CssClass="FilaDatos"/>
                                                                <Columns>
                                                                    <asp:BoundField DataField="ConIdent" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="ConTipoEntrada" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="ConCarneNumero" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="ConNombreCompleto" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:TemplateField ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <table class="AnchoTotal">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="ibtQuitarLista" runat="server" ImageUrl="~/Imagenes/Iconos/delete.png" OnClick="quitarRegistroLista" OnClientClick="mostrarCargando();" ToolTip="Quitar de la lista" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                    <td style="vertical-align:top;">
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td class="etiqueta" style="padding: 12px 0px 12px 0px;text-decoration: underline;" colspan="3">DATOS DE RECOJO</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%;">Documento de Identidad</td>
                                                                <td class="etiqueta">:</td>
                                                                <td colspan="2">
                                                                    <table class="AnchoTotal">
                                                                        <tr>
                                                                            <td style="width:30%;color:Red;">
                                                                                <asp:DropDownList runat="server" ID="ddlVarDocumentoIdent" CssClass="dropdownlist" AutoPostBack="false" Width="90%"></asp:DropDownList>
                                                                            </td>
                                                                            <td style="width:7%;text-align:right;" class="etiqueta">Numero</td>
                                                                            <td class="etiqueta" style="width:2%">:</td>
                                                                            <td style="width:25%;color:Red;">
                                                                                <asp:TextBox runat="server" ID="txtVarNumeroDocIdent" CssClass="textbox" MaxLength="20" Width="50%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtVarNumeroDocIdent" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ/-.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Imagenes/Iconos/buscarpersona.png" CommandName="solicitanteVar" OnClick="obtenerSolicitante" style="height: 15px" ToolTip="Busca un solicitante" />
                                                                                <%--&nbsp;&nbsp;&nbsp;
                                                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Imagenes/Iconos/copy.png" style="height: 15px" OnClientClick="copiarDatos();return false;" ToolTip="Copiar datos del Titular" />--%>
                                                                                &nbsp;&nbsp;&nbsp;
                                                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Imagenes/Iconos/clear.png" style="height: 15px" CommandName="solicitanteVar" OnClick="limpiarSolicitante" ToolTip="Limpiar Campos" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">Primer Apellido</td>
                                                                <td class="etiqueta">:</td>
                                                                <td style="color:Red;" class="style2">
                                                                    <asp:TextBox runat="server" ID="txtVarApePat" CssClass="textbox" Width="90%" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtVarApePat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">Segundo Apellido</td>
                                                                <td class="etiqueta">:</td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtVarApeMat" CssClass="textbox" Width="90%" MaxLength="100" Text=""></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtVarApeMat" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">Nombres</td>
                                                                <td class="etiqueta">:</td>
                                                                <td style="color:Red;">
                                                                    <asp:TextBox runat="server" ID="txtVarNombres" CssClass="textbox" Width="90%" MaxLength="100" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;*
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtVarNombres" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">Teléfono</td>
                                                                <td class="etiqueta">:</td>
                                                                <td style="color:Red;">
                                                                    <asp:TextBox runat="server" ID="txtVarTelefono" CssClass="textbox" Width="60%" Text=""></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtVarTelefono" Enabled="true" ValidChars="1234567890">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                           <%-- <tr>
                                                                <td class="etiqueta">Institucion</td>
                                                                <td class="etiqueta">:</td>
                                                                <td style="color:Red;">
                                                                    <asp:Label runat="server" ID="lblVarInstitucion" CssClass="labelInfo"></asp:Label>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td class="etiqueta" colspan="3" style="padding: 5px 0px 5px 0px;">Observación</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox runat="server" ID="txtVarObservacion" CssClass="textbox" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px" Text=""></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtDEObservacion" Enabled="true" ValidChars="1234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚüÜ ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Button runat="server" ID="btnAceptarVarios" CssClass="ImagenBotonOk" OnClientClick="return validarEntregaVarios();" OnClick="generarActaConformidad" Text="Aceptar" Width="150px" />
                                                        <asp:Button runat="server" ID="btnCancelarVarios" CssClass="ImagenBotonCancelar" OnClientClick="ocultarDiv('divVarios');ocultarDiv('divModal');return false;" Text="Cancelar" Width="150px" />
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
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Label runat="server" Text="Bandeja  de Registros" ID="lbltitulo" CssClass="titulo"></asp:Label>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:60%;vertical-align:top;">
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Datos del Registro</legend>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="width:20%" class="etiqueta">Periodo</td>
                                                <td style="width:35%">
                                                    <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="dropdownlist" Width="42%"></asp:DropDownList>
                                                </td>
                                                <td style="width:17%" class="etiqueta">Estado</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEstado" CssClass="dropdownlist" AutoPostBack="true" Width="80%" Enabled="true"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Numero Solicitud (Registro)</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNumeroIdent" CssClass="textbox" Width="40%"></asp:TextBox>
                                                </td>
                                                <td style="width:15%" class="etiqueta">Fecha de Registro</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaDesde" CssClass="textbox" Width="45%" MaxLength="10" Text=""></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calendarEmision" runat="server" TargetControlID="txtFechaDesde" PopupButtonID="ibtFechaEmision" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="ibtFechaEmision" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Numero de Carné</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNumeroCarne" CssClass="textbox" Width="40%"></asp:TextBox>
                                                </td>
                                                <td class="etiqueta"></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFechaHasta" CssClass="textbox" Width="45%" MaxLength="10" Text=""></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calendarVen" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="ibtFechaVen" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                    <asp:ImageButton ID="ibtFechaVen" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                </td>
                                            <tr>
                                                <td class="etiqueta">Calidad Migratoria</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaPri" CssClass="dropdownlist" Width="70%"></asp:DropDownList>
                                                </td>
                                                <td class="etiqueta" style="width:25%;">Tipo Entrada</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlTipoEntrada" CssClass="dropdownlist" Width="80%"></asp:DropDownList>
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
                                                    <asp:DropDownList runat="server" ID="ddlMision" CssClass="dropdownlist" AutoPostBack="false" Width="90%" Enabled="false"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="vertical-align:top;" colspan="2">
                                    <fieldset class="Fieldset">
                                        <legend class="FieldsetLeyenda">Datos del Titular del Carné</legend>
                                        <table class="AnchoTotal">
                                            <tr>
                                                <td class="etiqueta" style="width:20%;">Primer Apellido</td>
                                                <td style="color:Red;width:60%;">
                                                    <asp:TextBox runat="server" ID="txtApePat" CssClass="textbox" Width="70%" MaxLength="100"></asp:TextBox>&nbsp;&nbsp;&nbsp;
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
                                                    <asp:TextBox runat="server" ID="txtNombres" CssClass="textbox" Width="70%" MaxLength="100" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNombres" Enabled="true" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="etiqueta">Sexo</td>
                                                <td style="color:Red">
                                                    <asp:DropDownList runat="server" ID="ddlSexo" CssClass="dropdownlist" Width="45%" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <%--<td style="padding-left:50px;">
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td style="width:3%;">
                                                <asp:ImageButton ID="ibtEntregaMultiple" runat="server" ImageUrl="~/Imagenes/Iconos/lista.png" OnClick="mostrarPanelVarios" OnClientClick="mostrarCargando();" ToolTip="Ver lista multiple"/>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblInfoVarios" CssClass="labelInfoFile" Font-Size="13px" Width="100%" Text="Registros Agregados para entregar: "></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>--%>
                                <td align="right" colspan="2">
                                    <asp:Button runat="server" ID="btnRecepVarios" Text="Recepción Multiple" CssClass="ImagenBotonAgregarVariosRecep" OnClientClick="mostrarCargando();" CommandName="recepcionMultiple"  OnClick="mostrarPanelVarios" Width="150px"/>
                                    <asp:Button runat="server" ID="btnAgregarVarios" Text="Entrega Multiple" CssClass="ImagenBotonAgregarVarios" OnClientClick="mostrarCargando();" CommandName="entregaMultiple"  OnClick="mostrarPanelVarios" Width="150px"/>
                                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="ImagenBotonBuscar" Width="150px" OnClientClick="mostrarCargando();" OnClick="buscarRegistros" />
                                    <asp:Button runat="server" ID="btnLimpiar" Text="Limpiar Controles" CssClass="ImagenBotonClean" OnClientClick="limpiarControles();return false;" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <div style="padding-left:20px;width:100%;">
                                        <table class="AnchoTotal">
                                            <tr>
                                                <td class="CabeceraGrilla" style="width:3%;height:25px"></td>
                                                <td class="CabeceraGrilla" style="width:5%;">N° Solicitud</td>
                                                <td class="CabeceraGrilla" style="width:8%;">Tipo Entrada</td>
                                                <td class="CabeceraGrilla" style="width:5%">Numero Carné</td>
                                                <td class="CabeceraGrilla" style="width:8%">Estado</td>
                                                <td class="CabeceraGrilla" style="width:15%">Titular</td>
                                                <%--<td class="CabeceraGrilla" style="width:5%">Sexo</td>--%>
                                                <td class="CabeceraGrilla" style="width:10%">Calidad Migratoria</td>
                                                <td class="CabeceraGrilla" style="width:16%">Mision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Recep. Consulares</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Recep. Privilegios</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Emision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Vencimiento</td>
                                                <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                                <td class="CabeceraGrilla" style="width:5%">Datos de Recepcion</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divGridView" class="Scroll" runat="server" style="padding-left:20px;width:100%;">
                                        <asp:GridView ID="gvRegPrev" AutoGenerateColumns="false" runat="server" 
                                            style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" OnRowDataBound="verTemplate" Width="100%">
                                            <RowStyle CssClass="FilaDatos"/>
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="intAgregarLista" runat="server" ImageUrl="~/Imagenes/Iconos/add.png" CommandName="agregarListaEntrega" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Agregar a la Lista de Entrega" Visible="false" />
                                                                    <asp:ImageButton ID="ibtAgregarListaRecep" runat="server" ImageUrl="~/Imagenes/Iconos/addBlue.png" CommandName="agregarListaRecepcion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Agregar a la Lista de Recepcion" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ConIdent" ItemStyle-Width="5%" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConTipoEntrada" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConCarneNumero" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConEstadoDesc" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConNombreCompleto" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                <%--<asp:BoundField DataField="ConGenero" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />--%>
                                                <asp:BoundField DataField="ConCalidadMigratoriaDesc" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConOficinaConsular" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaConsulares" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaPrivilegios" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaEmision" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaVencimiento" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtEditar" runat="server" ImageUrl="~/Imagenes/Iconos/Edit_16x16.png" CommandName="editarRegistroPrevio" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Editar Registro Previo" Visible="false" />
                                                                    <asp:ImageButton ID="ibtDerivarRegCom" runat="server" ImageUrl="~/Imagenes/Iconos/usuarioDeriva.png" CommandName="derivarRegistroCompleto" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Derivar a Registrador" Visible="false" />
                                                                    <asp:ImageButton ID="ibtEntregar" runat="server" ImageUrl="~/Imagenes/Iconos/caja.png" CommandName="entregarRegistro" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Entrega de Carné" Visible="false" />
                                                                    <asp:ImageButton ID="ibtCargo" runat="server" ImageUrl="~/Imagenes/Iconos/cargo.png" CommandName="verActaConformidad" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Derivar para Entrega" Visible="false" />
                                                                    <asp:ImageButton ID="ibtPDFDatos" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" CommandName="pdfInformacion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Reporte" Visible="true" />
                                                                    <asp:ImageButton ID="ibtMovimientos" runat="server" ImageUrl="~/Imagenes/Iconos/movimientos1.png" CommandName="verMovimientos" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver Movimientos" Visible="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtRecepcion" runat="server" ImageUrl="~/Imagenes/Iconos/recepcion.png" CommandName="verActaRecepcion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver cargo de recepcion" Visible="false" />
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
                                            <td class="EtiquetaCentro" valign="top" style="height:30px;width:78%;">Pagina&nbsp;&nbsp;  <asp:DropDownList ID="ddlPaginas" runat="server" 
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
