<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="FormularioReportes.aspx.cs" Inherits="SolCARDIP.Paginas.Reportes.FormularioReportes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function validarReporte(FechaInicio, FechaFin) {
            var txt1 = document.getElementById(master + FechaInicio);
            var txt2 = document.getElementById(master + FechaFin);
            var dropDown1 = document.getElementById(master + "ddlCalidadMigratoriaPri");
            var dropDown2 = document.getElementById(master + "ddlEstado");
            var dropDown3 = document.getElementById(master + "ddlMision");

            if (txt1 != null & txt2 != null) {
                if (txt1.value == "") { alert("DEBE INGRESAR UNA FECHA INICIO"); txt1.focus(); return false; }
                if (txt2.value == "") { alert("DEBE INGRESAR UNA FECHA FIN"); txt2.focus(); return false; }
                // VALIDA FECHAS
                var fechaIni = txt1.value;
                var fechaFin = txt2.value;
                if (!validaFechaDDMMAAAA(fechaIni)) { alert("FECHA INICIO NO VALIDA. REVISE."); txt1.focus(); return false; };
                if (!validaFechaDDMMAAAA(fechaFin)) { alert("FECHA FIN NO VALIDA. REVISE."); txt2.focus(); return false; };

                return true;
            }
        }

        function existeFiltros() {
            
            var ddlStipoEmisionRep = document.getElementById(master + "ddlStipoEmisionRep").value;
            var txtFechaInicioRep = document.getElementById(master + "txtFechaInicioRep").value;
            var txtFechaFinRep = document.getElementById(master + "txtFechaFinRep").value;
            var ddlEstadoRep = document.getElementById(master + "ddlEstadoRep").value;
            //var ddlTipoDocRep = document.getElementById(master + " ddlTipoDocRep").value;
            var txtNum_documentoRep = document.getElementById(master + "txtNum_documentoRep").value;
            var txtNum_solicitudRep = document.getElementById(master + "txtNum_solicitudRep").value;
            var txtApellidosRep = document.getElementById(master + "txtApellidosRep").value;
            var txtFechaNacimientoRep = document.getElementById(master + "txtFechaNacimientoRep").value;
            var ddlPaisRep = document.getElementById(master + "ddlPaisRep").value;
            var ddlTitulaFamiliarRep = document.getElementById(master + "ddlTitulaFamiliarRep").value;
            var ddlOficinaRep = document.getElementById(master + "ddlOficinaRep").value;
            var ddlUsuarioRep = document.getElementById(master + "ddlUsuarioRep").value;

            if (ddlStipoEmisionRep == 0 && txtFechaInicioRep == "" && txtFechaFinRep == "" && ddlEstadoRep == 0 && 
                txtNum_documentoRep == "" && txtNum_solicitudRep == "" && txtApellidosRep == "" && txtFechaNacimientoRep == "" &&
                ddlPaisRep == 0 && ddlTitulaFamiliarRep == 0 && ddlOficinaRep == 0 && ddlUsuarioRep == 0
            ) {
                alert("DEBE INGRESAR AL MENOS UN FILTRO");
                return false;
            }
            return true;

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
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td style="width:100%">
                                                <table style="width:100%">
                                                    <tr id="trTabs">
                                                        <td id="tabPest0" class="Tabs" style="display:block;" onclick="tabActual(0);">Resumen x Calidad Migratoria</td>
                                                        <td id="tabPest1" class="Tabs" style="display:block;" onclick="tabActual(1);">Detalle de Carné de Identidad</td>
                                                        <td id="tabPest2" class="Tabs" style="display:block;" onclick="tabActual(2);">Consulta de carnet</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td id="tdContenedor">
                                                <div id="tab0" style="display:block;">
                                                    <table class="AnchoTotal">
                                                        <tr>
                                                            <td>
                                                                <table style="width:30%;">
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Desde</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox" Width="50%" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="calendarEmision" runat="server" TargetControlID="txtFechaInicio" PopupButtonID="ibtFechaInicio" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaInicio" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Hasta</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox" Width="50%" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaFin" PopupButtonID="ibtFechaFin" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaFin" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:50%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:50%;">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button runat="server" ID="btnResumenxCalidad" Text="Traer Datos" CssClass="ImagenBotonData" OnClientClick="return validarReporte('txtFechaInicio','txtFechaFin');" OnClick="traerDatos" Width="150px" />
                                                                            <asp:Button runat="server" ID="btnVerReporteResumenxCalidad" Text="Ver Reporte" CssClass="ImagenBotonReporte" OnClick="imprimirReporteResumen" Width="150px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:50%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:50%;">
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:12%;">Calidades</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td><asp:Label runat="server" ID="lblCantCalMig" CssClass="labelInfo"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta">Registrados</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td><asp:Label runat="server" ID="lblCantReg" CssClass="labelInfo"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta">Emitidos</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td><asp:Label runat="server" ID="lblCantEmi" CssClass="labelInfo"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta">Vigentes</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td><asp:Label runat="server" ID="lblCantVig" CssClass="labelInfo"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta">Vencidos</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td><asp:Label runat="server" ID="lblCantVen" CssClass="labelInfo"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div style="width:50%;">
                                                                    <table class="AnchoTotal">
                                                                        <tr>
                                                                            <td class="CabeceraGrilla" style="width:25%;height:25px">Calidad Migratoria</td>
                                                                            <td class="CabeceraGrilla" style="width:10%">Registrados</td>
                                                                            <td class="CabeceraGrilla" style="width:10%">Emitidos</td>
                                                                            <td class="CabeceraGrilla" style="width:10%">Vigentes</td>
                                                                            <td class="CabeceraGrilla" style="width:10%">Vencidos</td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div ID="divGridView" runat="server" class="Scroll" style="width:51%;">
                                                                    <asp:GridView ID="gvReportexCalidad" runat="server" 
                                                                        AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" ShowHeader="False" style="margin-top: 0px" EmptyDataText="No hay datos para mostrar"
                                                                        Width="100%">
                                                                        <RowStyle CssClass="FilaDatos" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="CalidadMigratoria" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%" />
                                                                            <asp:BoundField DataField="Registrados" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                            <asp:BoundField DataField="Emitidos" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                            <asp:BoundField DataField="Vigentes" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                            <asp:BoundField DataField="Vencidos" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="tab1" style="display:none;">
                                                    <table class="AnchoTotal">
                                                        <tr>
                                                            <td>
                                                                <table style="width:30%;">
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Buscar Por</td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rbFechaRegistro" runat="server" Text="Fecha de Registro" GroupName="FECHA" Checked/>
                                                                            <asp:RadioButton ID="rbFechaEmision" runat="server" Text="Fecha de Emisión" GroupName="FECHA"/>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Desde</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFechaInicioDet" CssClass="textbox" Width="50%" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaInicioDet" PopupButtonID="ibtFechaInicioDet" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaInicioDet" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Hasta</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFechaFinDet" CssClass="textbox" Width="50%" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaFinDet" PopupButtonID="ibtFechaFinDet" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaFinDet" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta">Calidad Migratoria</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlCalidadMigratoriaPri" CssClass="dropdownlist" AutoPostBack="false" Width="70%"></asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Estado</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlEstado" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList>
                                                                        </td>
                                                                    </tr>
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
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button runat="server" ID="btnTraerDatosDetalle" Text="Traer Datos" CssClass="ImagenBotonData" OnClientClick="return validarReporte('txtFechaInicioDet','txtFechaFinDet')" OnClick="traerDatosDetalle" Width="150px" />
                                                                            <asp:Button runat="server" ID="Button2" Text="Ver Reporte" CssClass="ImagenBotonReporte" OnClick="imprimirReporteDetalle" Width="150px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">

                                                                <div ID="div1" runat="server" class="Scroll" style="width:100%;">
                                                                    <asp:GridView ID="gvReporteDetalle" runat="server" 
                                                                        AlternatingRowStyle-BackColor="Control" 
                                                                        AutoGenerateColumns="false" 
                                                                        ShowHeader="true" style="margin-top: 2px" 
                                                                        EmptyDataText="No hay datos para mostrar"
                                                                        Width="100%"                                                                        
                                                                        
                                                                        >
                                                                        <RowStyle CssClass="FilaDatos" />
                                                                        <HeaderStyle 
                                                                            CssClass="CabeceraGrilla"
                                                                            BorderColor="White" 
                                                                            BorderWidth="1px"                                                                                                                                                       
                                                                         />
                                                             
                                                                        <Columns>
                                                                            <asp:BoundField DataField="NumeroCarne" HeaderText="Numero Carné" ItemStyle-Height="35px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="CalidadMigratoria" HeaderText="Calidad Migratoria" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="FechaReg" HeaderText="Fecha Registro" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="FechaEmi" HeaderText="Fecha Emision" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="FechaVen" HeaderText="Fecha Vencimiento" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Titular" HeaderText="Titular" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="TitDep" HeaderText="Estatus Migratorio" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="PaisNac" HeaderText="País (Nacionalidad)" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="OficinaConsularEx" HeaderText="Institucion" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Ecivil" HeaderText="Estado Civil" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Departamento" HeaderText="Departamento" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Provincia" HeaderText="Provincia" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Distrito" HeaderText="Distrito" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:15%;">Cantidad de Registros Encontrados</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblCantRegDetalle" CssClass="labelInfo"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="tab2" style="display:none;">
                                                    <table class="AnchoTotal">
                                                        <tr>
                                                            <td>
                                                                <table style="width:30%;">
                                                                    
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Tipo Emisión</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlStipoEmisionRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Fecha Desde</td>
                                                                        <td><div style="display:flex">
                                                                            <div>
                                                                            <asp:TextBox runat="server" ID="txtFechaInicioRep" CssClass="textbox" Width="100px" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFechaInicioRep" PopupButtonID="ibtFechaInicioRep" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaInicioRep" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Inicio" BorderWidth="0" />
                                                                            </div>
                                                                            <div>&nbsp;&nbsp;
                                                                            Hasta:
                                                                            <asp:TextBox runat="server" ID="txtFechaFinRep" CssClass="textbox" Width="100px" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtFechaFinRep" PopupButtonID="ibtFechaFinRep" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaFinRep" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Estado</td>
                                                                        <td> <asp:DropDownList runat="server" ID="ddlEstadoRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Tipo Documento</td>
                                                                        <td> <div style="display:flex">
                                                                            <div>
                                                                            <asp:DropDownList runat="server" ID="ddlTipoDocRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList>
                                                                                </div>
                                                                            <div>&nbsp;&nbsp; Nro Documento
                                                                            <asp:TextBox runat="server" ID="txtNum_documentoRep" CssClass="dropdownlist" Width="50%" Enabled="true"/>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Nro Solicitud</td>
                                                                        <td> <asp:TextBox runat="server" ID="txtNum_solicitudRep" CssClass="dropdownlist" Width="50%" Enabled="true"/></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Nro Carnet</td>
                                                                        <td> <asp:TextBox runat="server" ID="txtNumero_carneRep" CssClass="dropdownlist" Width="50%" Enabled="true"/></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Apellidos y Nombres</td>
                                                                        <td> <asp:TextBox style="text-transform: uppercase;" runat="server" ID="txtApellidosRep" CssClass="dropdownlist" Width="100%" Enabled="true"/></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:30%;">Fecha de Nacimiento</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtFechaNacimientoRep" CssClass="textbox" Width="100px" MaxLength="10" Text=""></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtFechaNacimientoRep" PopupButtonID="ibtFechaNacimientoRep" Format="dd/MM/yyyy"></cc1:CalendarExtender >
                                                                            <asp:ImageButton ID="ibtFechaNacimientoRep" runat="server" ImageUrl="~/Imagenes/Iconos/ico_calendar.gif" ToolTip="Seleccione Fecha de Ocurrencia" BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Nacionalidad</td>
                                                                        <td> <asp:DropDownList runat="server" ID="ddlPaisRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Titular/Familiar</td>
                                                                        <td> <asp:DropDownList runat="server" ID="ddlTitulaFamiliarRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Institución</td>
                                                                        <td> <asp:DropDownList runat="server" ID="ddlOficinaRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:17%" class="etiqueta">Funcionario Responsable</td>
                                                                        <td> <asp:DropDownList runat="server" ID="ddlUsuarioRep" CssClass="dropdownlist" Width="100%" Enabled="true"></asp:DropDownList></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button runat="server" ID="Button1" Text="Traer Datos" CssClass="ImagenBotonData" OnClientClick="return existeFiltros()" OnClick="consultarCarnet" Width="150px" />
                                                                            <asp:Button runat="server" ID="Button3" Text="Ver Reporte" CssClass="ImagenBotonReporte" OnClick="imprimirReporteConsultaCarnet" Width="150px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr><td><hr /></td></tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div style="width:90%;">
                                                                    <table class="AnchoTotal">
                                                                        <tr>
                                                                            <td class="CabeceraGrilla" style="width:5%">Num Solicitud</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Reli_stipo_emision</td>ww-->
                                                                            <td class="CabeceraGrilla" style="width:9%">Apellidos Nombres</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Fecha Nacimiento</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Tipo doc.</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Numero Documento</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Para_sparametroid</td>ww-->
                                                                            <td class="CabeceraGrilla" style="width:5%">Titula/Familiar</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Documento del titular</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Pers_spaisid</td>ww-->
                                                                            <td class="CabeceraGrilla" style="width:8%">Nacionalidad</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Calidad Migratoria</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Caid_sestadoid</td>ww-->
                                                                            <td class="CabeceraGrilla" style="width:5%">Estado</td>
                                                                            <td class="CabeceraGrilla" style="width:5%">Numero Carnet</td>
                                                                            <td class="CabeceraGrilla" style="width:6%">Fecha de Emision</td>
                                                                            <td class="CabeceraGrilla" style="width:6%">Fecha de Vencimiento</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Oficina_consularid</td>ww-->
                                                                            <td class="CabeceraGrilla" style="width:8%">Institucion	</td>
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Caid_dfechacreacion</td>ww-->
                                                                            <!--<td class="CabeceraGrilla" style="width:8%">Caid_susuariocreacion</td>ww-->
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div ID="div2" runat="server" class="Scroll" style="width:90%;">
                                                                    <asp:GridView ID="gvConsultaCarnet" runat="server"  AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" ShowHeader="False" style="margin-top: 0px" EmptyDataText="No hay datos para mostrar"
                                                                        Width="100%">
                                                                        <RowStyle CssClass="FilaDatos" />
                                                                        <Columns>
                                                                        <asp:BoundField DataField="Num_solicitud" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Apellidos_nombres" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%" />
                                                                        <asp:BoundField DataField="Fecha_nacimientofecha" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="TipoDoc" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Documentonumero" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Status_mig" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Documento_titular" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Pais_nacionalidad" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" />
                                                                        <asp:BoundField DataField="Calidad_migratoria" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Estado" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Numero_carne" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                        <asp:BoundField DataField="Fecha_emision" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" />
                                                                        <asp:BoundField DataField="Fecha_vencimiento" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" />
                                                                        <asp:BoundField DataField="Oficina_consular_extranjera" ItemStyle-Height="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:90%;">
                                                                    <tr>
                                                                        <td class="etiqueta" style="width:15%;">Cantidad de Registros Encontrados</td>
                                                                        <td class="etiqueta" style="width:5%;">:</td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="cantRegistroCarnet" CssClass="labelInfo"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdfldTabActual" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
