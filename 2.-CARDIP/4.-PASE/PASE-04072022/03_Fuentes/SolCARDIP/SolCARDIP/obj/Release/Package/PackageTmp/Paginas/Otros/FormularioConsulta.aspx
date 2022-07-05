<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="FormularioConsulta.aspx.cs" Inherits="SolCARDIP.Paginas.Otros.FormularioConsulta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var master = "ContentPlaceHolder1_";

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
        var dropDownSexo = document.getElementById(master + "ddlSexo");
        var lblNacionalidad = document.getElementById(master + "lblNacionalidad");
        if (txtNumeroIdent != null & txtNumeroCarne != null & txtFechaEmision != null & txtFechaVenc != null & txtApePat != null & txtApeMat != null & txtNom != null &
            dropDownPeriodo != null & dropDownCalMig != null & dropDownTitDep != null & dropDownCargo != null & dropDownEstado != null & dropDownPais != null & dropDownCategoria != null & dropDownMision != null & dropDownSexo != null) {
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
            dropDownSexo.value = "0";
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
                                                <td style="width:40%;color:Red">
                                                    <asp:DropDownList runat="server" ID="ddlSexo" CssClass="dropdownlist" AutoPostBack="true" Width="80%" OnSelectedIndexChanged="seleccionarSexo"></asp:DropDownList>&nbsp;&nbsp;&nbsp;*
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
                                                <%--<td class="CabeceraGrilla" style="width:5%">Fecha Inscripción (Registro)</td>--%>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Emision</td>
                                                <td class="CabeceraGrilla" style="width:5%">Fecha Vencimiento</td>
                                                <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                                <%--<td class="CabeceraGrilla" style="width:3%">Otros</td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divGridView" class="Scroll" runat="server" style="padding-left:20px;width:98%;">
                                        <asp:GridView ID="gvCarne" AutoGenerateColumns="false" runat="server" 
                                            style="margin-top: 0px" ShowHeader="False" AlternatingRowStyle-BackColor="Control" Width="100%">
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
                                                <%--<asp:BoundField DataField="ConFechaInscripcion" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />--%>
                                                <asp:BoundField DataField="ConFechaEmision" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConFechaVen" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <table class="AnchoTotal">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ibtPDFDatos" runat="server" ImageUrl="~/Imagenes/Iconos/pdf.png" CommandName="pdfInformacion" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Reporte" Visible="true" />
                                                                    <%--<asp:ImageButton ID="ibtMovimientos" runat="server" ImageUrl="~/Imagenes/Iconos/movimientos1.png" CommandName="verMovimientos" OnClick="seleccionarRegistro" OnClientClick="mostrarCargando();" ToolTip="Ver Movimientos" Visible="true" />--%>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
