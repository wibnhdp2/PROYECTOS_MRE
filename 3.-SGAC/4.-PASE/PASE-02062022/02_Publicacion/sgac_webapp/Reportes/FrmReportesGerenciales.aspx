<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmReportesGerenciales.aspx.cs" Inherits="SGAC.WebApp.Reportes.FrmReportesGerenciales" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation"
    TagPrefix="label" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }

        function validarPreviaImpresion() {

            var esUsuarioValido = $("#<%=ddlusuario.ClientID %>").val() == "0" ? false : true;
            var esAdhesivoValido = $("#<%=ddlEstAutoadhesivo.ClientID %>").val() == "0" ? false : true;
            var esReporteValido = $("#<%=ddlReportesGerenciales.ClientID %>").val() == "5006" ? true : false;


            if (esReporteValido) {
                            
                 if (!esUsuarioValido) {
                    alert("Por favor, elija usuario");
                    return
                }

                if (!esAdhesivoValido) {
                    alert("Por favor, elija estado de autoadhesivo");
                    return
                }           
            }
            
            if (!esUsuarioValido && !esAdhesivoValido) {                
                abrirPopupEspera()
            }
            
        }


    </script>
 
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes Gerenciales:"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <br />
    <table style="width: 90%;" align="center" class="mTblPrincipal">
        <tr>
            <td>
                <div id="tabs">
                    <asp:UpdatePanel runat="server" ID="updReportesGerenciales" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" >
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Reportes Gerenciales:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlReportesGerenciales" runat="server" Width="604px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlReportesGerenciales_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_TramitesSinVincular" runat="server" 
                                            Text="Trámites sin Vincular" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Categoría Oficina:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlCategoriaOficina" runat="server" Width="300px" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Continente:"></asp:Label>
                                    </td>
                                    <td colspan="1">
                                        <asp:DropDownList ID="ddlContinente" runat="server" Width="200" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlContinente_SelectedIndexChanged" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="País:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPais" runat="server" Width="200" TabIndex="2" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="604px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAnioMes" runat="server" Text="Periodo:"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblDel" runat="server" Text="DEL:"></asp:Label>
                                        <asp:DropDownList ID="ddlAnio" runat="server" Style="cursor: pointer;" Width="60px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMes" runat="server" Style="cursor: pointer;">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblAl" runat="server" Text="Al:"></asp:Label>
                                        <asp:DropDownList ID="ddlAnioHasta" runat="server" Style="cursor: pointer;" Width="60px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMesHasta" runat="server" Style="cursor: pointer;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUsiario" runat="server" Text="Usuario:"></asp:Label>
                                    </td>
                                    <td colspan="1">
                                        <asp:DropDownList ID="ddlusuario" runat="server" TabIndex="5" Width="200px">
                                        </asp:DropDownList>
                                        <strong style="color:Red;">(*)</strong>
                                    </td>
                                    
                                    <div id="DivEstadoAutoadhesivo" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblEstadoAutoadh" runat="server" Text="Estado de Autoadhesivo:"></asp:Label>                                                  
                                        </td>
                                        <td>
                                            <strong style="color:Red;">(*)</strong>                                                                                  
                                            <asp:DropDownList ID="ddlEstAutoadhesivo" runat="server" TabIndex="5" Width="180px">
                                            </asp:DropDownList>                                            
                                        </td>
                                    </div>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Tipo Pago:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoPago" runat="server" Width="300px" TabIndex="6">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Tarifa:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTarifa" runat="server" Width="604px" TabIndex="6" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlTarifa_SelectedIndexChanged" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblClasificacion" runat="server" Text="Clasificación:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlClasificacion" runat="server" Width="604px" TabIndex="6" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <div id="ConsultaAnio" runat="server" visible="false">
                                    <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Año:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAnioConsulta" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOrdenado" runat="server" Text="Ordenado Por:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbMonto" GroupName="Ord" runat="server" Checked="true" Text="Saldo" /><br />
                                                <asp:RadioButton ID="rbPaisConsulado" GroupName="Ord" runat="server" Text="Pais-Oficina Consular" />
                                            </td>
                                    
                                    </tr>
                                    <tr><td colspan="4"></td>
                                    </tr>
                                </div>
                                <div id="divPagosEnLima" runat="server" visible="false">
                                <tr>
                                        <td>                                         
                                        </td>
                                        <td>                                          
                                        </td>
                                        <td>Considerar:                                        
                                        </td>
                                        <td >
                                            <asp:RadioButton ID="rdioPagoLima" GroupName="RdioP" runat="server" Checked="true" Text="Con Pagos en Lima" /><br />
                                            <asp:RadioButton ID="rdioSinPagoLima" GroupName="RdioP" runat="server" Text="Sin Pagos en Lima" />
                                        </td>
                                    </tr>
                                </div>
                                <div id="divTipoReporteActuacion" runat="server" visible="false">
                                    <tr>
                                        <td>                                         
                                        </td>
                                         <td>                                       
                                        </td>
                                        <td > Tipo de Reporte:
                                        </td>
                                        <td><asp:RadioButton ID="rdioActuacion" GroupName="RdioAct" runat="server" Checked="true" Text="Por Actuacion" /><br />
                                            <asp:RadioButton ID="rdioRecaudacion" GroupName="RdioAct" runat="server" Text="Por Recaudacion" />                                          
                                        </td>
                                       
                                    </tr>
                                </div>

                                <div runat="server" id="ocultar" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEstadoCivil" runat="server" Text="Estado Civil:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbEstCiv" runat="server" Width="170px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LblGenero" runat="server" Text="Género:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbGenero" runat="server" Width="170px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCodPostal" runat="server" Text="Código Postal:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCodPostal" runat="server" Width="170px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOcupación" runat="server" Text="Ocupación:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbOcupacion" runat="server" Width="170px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProfesion" runat="server" Text="Profesión:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_Profesion" runat="server" Width="170px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGradoInstrucción" runat="server" Text="Grado de Instrucción:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbGradInst" runat="server" Width="170px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblNacionalidad" runat="server" Text="Nacionalidad :" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbNacRecurr" runat="server" Width="170px"/>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkDireccion" runat="server" Text="Buscar Por Ubigeo" AutoPostBack="True"
                                                OnCheckedChanged="chkDireccion_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <div runat="server" id="ocultarDir" visible="false">
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblTipRes" runat="server" Text="Tipo de Residencia:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="CmbTipRes" runat="server" Width="170px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblDptoContDir" runat="server" Text="Dpto./Continente:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="CmbDptoContDir" runat="server" Width="170px" 
                                                    OnSelectedIndexChanged="CmbDptoContDir_SelectedIndexChanged" 
                                                    AutoPostBack="True" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProvPaisDir" runat="server" Text="Prov./País :" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="CmbProvPaisDir" runat="server" Width="170px" 
                                                    OnSelectedIndexChanged="CmbProvPaisDir_SelectedIndexChanged" 
                                                    AutoPostBack="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblDistCiuDir" runat="server" Text="Dist./Ciudad /Estado:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="CmbDistCiuDir" runat="server" Width="170px" />
                                            </td>
                                        </tr>
                                    </div>
                                </div>
                                <tr>
                                    <div id="RangoFechas" runat="server">
                                        <td> 
                                            <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFechaFin" runat="server" CssClass="lblPadding" Text="Fecha Fin:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        </td>
                                    </div>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkFechaActuacion" runat="server" Style="cursor: pointer" Checked="True"
                                            Text="Por fecha de Actuación (Registro General de Entradas)" AutoPostBack="True"
                                            OnCheckedChanged="chkFechaActuacion_CheckedChanged" Visible="False" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkFechaAnulacion" runat="server" Style="cursor: pointer" Text="Por fecha de Anulación"
                                            AutoPostBack="True" OnCheckedChanged="chkFechaAnulacion_CheckedChanged" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">

                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSinFecha" runat="server" Text="Todas las fechas" 
                                            AutoPostBack="true" oncheckedchanged="chkSinFecha_CheckedChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">

                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="BtnAceptar" OnClientClick="return validarPreviaImpresion();" runat="server"
                                            Text="     Imprimir" CssClass="btnPrint" OnClick="BtnAceptar_Click" />
                                        
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnExportar" OnClientClick="return abrirPopupEspera();" runat="server"
                                            Text="     Exportar / Imprimir" CssClass="btnPrint" Width="160px" 
                                            onclick="btnExportar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <label:validation ID="ctrlValidacion" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div id="modalEspera" class="modal">
                                <div class="modal-window">
                                    <div class="modal-titulo">
                                        <span>Procesando...</span>
                                    </div>
                                    <div class="modal-cuerpo">
                                        <br />
                                        <h3 style="font-size: small">
                                            Procesando la información. Por favor espere...</h3>
                                        <asp:Image ID="imgEspera" ImageUrl="../Images/espera.gif" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
  <asp:GridView ID="grdOficinasActivasUniversal" runat="server" Visible="false" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="ofco_sOficinaConsularId" />
        <asp:BoundField DataField="ofco_vCodigo" />
        <asp:BoundField DataField="ofco_vNombre" />
        <asp:BoundField DataField="ofco_iReferenciaPadreId" />
        <asp:BoundField DataField="ofco_iJefaturaFlag" />
        <asp:BoundField DataField="ofco_IRemesaLimaFlag" />
        <asp:BoundField DataField="ofco_cUbigeoCodigo" />
        <asp:BoundField DataField="ofco_cUbigeoCodigoPais" />
        <asp:BoundField DataField="vPaisNombre" />


    </Columns>
    </asp:GridView>
</asp:Content>
