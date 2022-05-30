<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeguimientoSolicitudesSunarp.aspx.cs" Inherits="SGAC.WebApp.Sunarp.SeguimientoSolicitudesSunarp" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>    
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTitutloTipoCambio" runat="server" Text="Seguimiento de Solicitudes - SUNARP"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <div id="tabs">
            <ul>
                <li><a href="#tab-1">
                    <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
            </ul>
        </div>
        <div id="tab-1">
            <fieldset class="Fieldset">
                <legend>Datos de la Escritura:</legend>
                <table>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="lblConsulado" runat="server" Text="Oficina Consular:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" />
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="lblPeriodo" runat="server" Text="Nro. de Escritura:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtEscritura" runat="server" Width="345px" Style="text-transform: uppercase;" onkeypress="return soloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha de Extensíón:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtFechaInicio" runat="server" type="date" Width="165px"></asp:TextBox>
                            - 
                            <asp:TextBox ID="txtFechaFin" runat="server" type="date" Width="165px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="Fieldset">
                <legend>Datos de la Solicitud:</legend>
                <table>
                     <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="Label2" runat="server" Text="Nro. de Título:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="ddlAnio" runat="server" Width="85px"></asp:DropDownList>
                            <asp:TextBox ID="txtNroTitulo" runat="server" Width="250px" Style="text-transform: uppercase;" onkeypress="return soloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="Label3" runat="server" Text="Número CUO:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtNumCuo" runat="server" Style="text-transform: uppercase;" Width="340px" onkeypress="return AlfaNumerico(event)"></asp:TextBox>
                       
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="Label1" runat="server" Text="Estado:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="ddlEstado" runat="server" Width="340px"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1" style="width: 100px;">
                            <asp:Label ID="Label4" runat="server" Text="Fecha de Solicitud:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtFecSolIni" runat="server" type="date" Width="165px"></asp:TextBox>
                            - 
                            <asp:TextBox ID="txtFecSolFin" runat="server" type="date" Width="165px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:Button ID="btnBuscar" runat="server" Text="    Buscar" OnClientClick="return ValidarDatosBusqueda();"
                        CssClass="btnSearch" OnClick="btnBuscar_Click" />
            <asp:Button runat="server" ID="btnReporte" Text="     Reporte" Width="120px"
                        CssClass="btnExcel" OnClientClick="Popup();" OnClick="btnReporte_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="    Limpiar"
                        CssClass="btnLimpiar" OnClick="btnLimpiar_Click" />
                
                <hr />
                <table>
                    <tr>
                        <td colspan="2">
                            <div style="width: 12px; height: 12px; background: #A4F7EB; float:left;  border: 1px solid #555; margin-right:5px;"></div>
                            <div style="width:350px;">
                                <span style="font-weight:bold; font-size:12px;"> Solicitudes con Parte Firmado Digitalmente</span>
                            </div>
                        </td>
                    </tr>
                </table>
            <asp:GridView ID="grvSolicitudes"
                    runat="server"
                    CssClass="mGrid"
                    AlternatingRowStyle-CssClass="alt"
                    SelectedRowStyle-CssClass="slt"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    OnRowDataBound="grvSolicitudes_RowDataBound">

                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                    <Columns>
                        <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SolicitudID"
                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                            <HeaderStyle CssClass="ColumnaOculta" />
                            <ItemStyle CssClass="ColumnaOculta" />
                        </asp:BoundField>
                        <asp:BoundField DataField="oficina" HeaderText="Oficina">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CUO" HeaderText="C.U.O">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TIPO_ACTO" HeaderText="Tipo Acto">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="FECHA" HeaderText="Fecha">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="ESTADO" HeaderText="Estado">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HOJA_PRESENTACION" HeaderText="Hoja Presentación">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TITULO" HeaderText="Titulo">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHA_INSCRIPCION" HeaderText="Fecha de Inscripción">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHA_PRESENTACION" HeaderText="Fecha de Presentación">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="Fecha de Vencimiento">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NOMBRE_PARTE" HeaderText="SOIN_vPARTEFIRMADO"
                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                            <HeaderStyle CssClass="ColumnaOculta" />
                            <ItemStyle CssClass="ColumnaOculta" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnVer" ToolTip="Ver Historial"  runat="server" ImageUrl="../Images/img_16_estado_tramite.png" OnClick="VerHistorial" iSolicitud='<%# Eval("SOLICITUD_ID")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnParte" ToolTip="Parte"  runat="server" ImageUrl="../Images/img_16_preview.png"  OnClientClick="return VerParte(this);" Parte='<%# Eval("NOMBRE_PARTE")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                    </Columns>

                    <SelectedRowStyle CssClass="slt" />

                </asp:GridView>

            <div id="modalPDF" class="modal">
                <div class="modal-windowlarge">
                    <div class="modal-titulo">
                        <asp:ImageButton ID="imgCerrarPopupPDF" CssClass="close" ImageUrl="~/Images/img_cerrar.gif"
                            OnClientClick="cerrarPopupPDF(); return false" runat="server" />
                        <span>Parte</span>
                    </div>
                    <div class="modal-cuerpoNormal" style="height: 750px;">
                        <iframe id="pdfiframe" style="width: 100%; height: 750px;"></iframe>
                    </div>
                </div>
            </div>

            <div id="modalHistorico" class="modal">
                <div class="modal-window">
                    <div class="modal-titulo">
                        <asp:ImageButton ID="ImageButton1" CssClass="close" ImageUrl="~/Images/img_cerrar.gif"
                            OnClientClick="cerrarPopup(); return false" runat="server" />
                        <span>CUO:</span> <asp:Label ID="lblCUOHistorico" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="modal-cuerpoNormal" style="height: 350px;">
                        <asp:GridView ID="grvDetalle"
                            runat="server"
                            CssClass="mGrid"
                            AlternatingRowStyle-CssClass="alt"
                            SelectedRowStyle-CssClass="slt"
                            AutoGenerateColumns="False"
                            GridLines="None">
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="FECHA" HeaderText="FECHA">
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO">
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OBSERVACION" HeaderText="OBSERVACIÓN">
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle CssClass="slt" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
<script language="javascript" type="text/javascript">
    $(function () {
        $('#tabs').tabs();
    });

</script>
        <script>
            function ValidarDatosBusqueda() {
                var txtOficina = $('#<%= ctrlOficinaConsular1.ClientID %>').prop('selectedIndex');
                var txtEstado = $('#<%= ddlEstado.ClientID %>').prop('selectedIndex');

                if (txtOficina == 0) {
                    alert('Ingrese la oficina consular');
                    return false;
                }

                var txtNumeroEscritura = $('#<%= txtEscritura.ClientID %>').val();
                var txtNumeroCUO = $('#<%= txtNumCuo.ClientID %>').val();
                var estado = $('#<%= ddlEstado.ClientID %>').prop('selectedIndex');

                var FechaInicio = $('#<%= txtFechaInicio.ClientID %>').val();
                var FechaFin = $('#<%= txtFechaFin.ClientID %>').val();


                if (FechaInicio.length > 0 || FechaFin.length > 0) {
                    if (FechaInicio == "" || FechaFin == "") {
                        alert('Ingrese la fecha extención de inicio y fin');
                        return false;
                    }
                    else {
                        if (txtNumeroEscritura.length == 0) {

                            alert('Ingrese número de escritura');
                            return false;
                        }
                    }
                }

                var FechaInicioSol = $('#<%= txtFecSolIni.ClientID %>').val();
                var FechaFinSol = $('#<%= txtFecSolFin.ClientID %>').val();


                if (FechaInicioSol.length > 0 || FechaFinSol.length > 0) {
                    if (FechaInicioSol == "" || FechaFinSol == "") {
                        alert('Ingrese la fecha de solicitud de inicio y fin');
                        return false;
                    }
                }


                var ddlAnio = $('#<%= ddlAnio.ClientID %>').prop('selectedIndex');
                var txtTitulo = $('#<%= txtNroTitulo.ClientID %>').val();

                if (ddlAnio > 0) {

                    if (txtTitulo.length == 0) {
                        alert('Ingrese el número del título');
                        return false;
                    }
                }
                if (txtTitulo.length > 0) {
                    if (ddlAnio == 0) {
                        alert('Ingrese el año del título');
                    }
                }

                var aFecha1 = FechaInicio.split('-');
                var aFecha2 = FechaFin.split('-');
                var fFecha1 = Date.UTC(aFecha1[0], aFecha1[1] - 1, aFecha1[2]);
                var fFecha2 = Date.UTC(aFecha2[0], aFecha2[1] - 1, aFecha2[2]);
                var dif = fFecha2 - fFecha1;
                var dias = Math.floor(dif / (1000 * 60 * 60 * 24));

                if (dias >= 365) {
                    alert('El rango de fechas de extención no debe ser mayor a 1 año');
                    return false;
                }

                var Fecha1 = FechaInicioSol.split('-');
                var Fecha2 = FechaFinSol.split('-');
                var Fecha1 = Date.UTC(Fecha1[0], Fecha1[1] - 1, Fecha1[2]);
                var Fecha2 = Date.UTC(Fecha2[0], Fecha2[1] - 1, Fecha2[2]);
                var dif2 = Fecha2 - Fecha1;
                var dias2 = Math.floor(dif2 / (1000 * 60 * 60 * 24));

                if (dias2 >= 365) {
                    alert('El rango de fechas de la solicitud no debe ser mayor a 1 año');
                    return false;
                }


                if (txtNumeroEscritura.length == 0 && txtNumeroCUO.length == 0 && estado == 0 && txtEstado == 0 && FechaInicio.length == 0 && FechaFin.length == 0 && FechaInicioSol.length == 0 && FechaFinSol.length == 0 && ddlAnio == 0 && txtTitulo.length == 0) {
                    alert('Ingrese algun filtro más de búsqueda');
                    return false;
                }
                return true;
            }


            function VerParte(element) {


                var cadena = element.getAttribute("Parte");

                var ruta = cadena.substr(1, 4) + "\\" + cadena;

                var str_archivoFirma = "?archivo=" + ruta;

                var argsValores = str_archivoFirma;
                var prmsValores;
                var returnValue;
                prmsValores = "dialogWidth:1300px;dialogHeight:1000px;center:yes;scrollbars=yes;help=no;status=no;toolbar=no";

                document.getElementById('pdfiframe').src = "../Accesorios/VistaPreviaPDF.aspx" + argsValores, "NewWin", prmsValores;
                PopupPDF();
                return false;
            }
            function Popup() {
                document.getElementById('modalHistorico').style.display = 'block';
            }
            function cerrarPopup()
            {
                document.getElementById('modalHistorico').style.display = 'none';
            }
            function PopupPDF() {
                document.getElementById('modalPDF').style.display = 'block';
            }
            function cerrarPopupPDF() {

                document.getElementById('modalPDF').style.display = 'none';
            }

            function soloNumeros(e) {
                tecla = (document.all) ? e.keyCode : e.which;

                //Tecla de retroceso para borrar, siempre la permite
                if (tecla == 8) {
                    return true;
                }
                // Patron de entrada, en este caso solo acepta numeros
                patron = /[0-9]/;
                tecla_final = String.fromCharCode(tecla);
                return patron.test(tecla_final);
            }
            function AlfaNumerico(e) {
                tecla = (document.all) ? e.keyCode : e.which;

                //Tecla de retroceso para borrar, siempre la permite
                if (tecla == 8) {
                    return true;
                }
                // tecla ñ
                if (tecla == 241) {
                    return true;
                }
                // tecla Ñ
                if (tecla == 209) {
                    return true;
                }
                // tecla .
                if (tecla == 46) {
                    return true;
                }
                // tecla -
                if (tecla == 45) {
                    return true;
                }
                //Tecla de espacio, siempre la permite
                if (tecla == 32) {
                    return true;
                }
                // Patron de entrada, en este caso solo acepta numeros y letras
                patron = /[A-Za-z0-9]/;
                tecla_final = String.fromCharCode(tecla);
                return patron.test(tecla_final);
            }
            function letras(e) {
                tecla = (document.all) ? e.keyCode : e.which;

                //Tecla de retroceso para borrar, siempre la permite
                if (tecla == 8) {
                    return true;
                }
                // tecla ñ
                if (tecla == 241) {
                    return true;
                }
                // tecla Ñ
                if (tecla == 209) {
                    return true;
                }
                // tecla .
                if (tecla == 46) {
                    return true;
                }
                // tecla -
                if (tecla == 45) {
                    return true;
                }
                // tecla apostrofe
                if (tecla == 39) {
                    return true;
                }
                //Tecla de espacio, siempre la permite
                if (tecla == 32) {
                    return true;
                }
                // Patron de entrada, en este caso solo acepta letras
                patron = /[A-Za-z áéíóú ÁÉÍÓÚ Üü]/;
                tecla_final = String.fromCharCode(tecla);
                return patron.test(tecla_final);
            }
        </script>
</asp:Content>

