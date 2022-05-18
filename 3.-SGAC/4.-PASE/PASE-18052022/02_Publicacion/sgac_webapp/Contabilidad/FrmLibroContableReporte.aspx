<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmLibroContableReporte.aspx.cs"
    Inherits="SGAC.WebApp.Contabilidad.LibroContableReporte" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>   
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }
    </script>
   
    <style type="text/css">
.mensajePie
{
    color:Black;
    font-family:@Arial Unicode MS;
    font-size:smaller;
    font-weight:bold; 
}        
        .style1
        {
            width: 132px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <%--Cuerpo--%>
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes: Libros Contables"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <br />

    <%--Opciones--%>
    <table style="width: 90%" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnDownload" />
                        <asp:PostBackTrigger ControlID="btnExportarPDF" />
                    </Triggers>
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="450px" style="cursor:pointer;"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblTipoLibro" runat="server" Text="Tipo Libro:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlLibroContableTipo" runat="server" Width="450px" AutoPostBack="True" style="cursor:pointer;"
                                            OnSelectedIndexChanged="ddlLibroContableTipo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <div runat="server" id="Panel" visible="false" style="padding:-1px;">
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="lblBancoConsulta" runat="server" Text="Cta. Bancaria:"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlBancoConsulta" runat="server" style="cursor:pointer;" 
                                                Width="230px" 
                                                onselectedindexchanged="ddlBancoConsultas_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="lblNroCuenta" runat="server" Text="N° Cuenta:"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlCuentaCorrienteConsulta" runat="server" style="cursor:pointer;" 
                                                Width="230px" AutoPostBack="True" 
                                                onselectedindexchanged="ddlCuentaCorrienteConsulta_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Label ID="lblMoneda" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </div>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblTipoReporteReniec" runat="server" Text="Tipo Reporte RENIEC:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoReporteReniec" runat="server" AutoPostBack="True" 
                                            style="cursor:pointer;" Width="450px" 
                                            onselectedindexchanged="ddlTipoReporteReniec_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblTipoReporteRegistroCivil" runat="server" Text="Tipo Reporte REGISTRO CIVIL:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoReporteRCivil" runat="server" AutoPostBack="True" 
                                            style="cursor:pointer;" Width="450px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <%--<div id="DivDescarga" runat="server" style=" bottom: -2px; right: 0; border: 1px solid #dcdcdc; background-color: #dcdcdc;
                                            padding: 5px 5px 5px 5px;">
                                            <asp:LinkButton ID="lkbDescargarFormato" runat="server" CssClass="btn btn-primary" OnClick="lkbDescargar_Click"
                                            ForeColor="Black">Descargar Formato <i class="glyphicon glyphicon-save"></i></asp:LinkButton>
                                        </div>--%>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblTipoTarifa" runat="server" 
                                            Text="Tipo Tarifa:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoTarifaReniec" runat="server"  
                                            style="cursor:pointer;" Width="450px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblEstadoFichaRegistral" runat="server" Text="Estado Ficha Registral:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlEstadoFichaRegistral" runat="server" 
                                            style="cursor:pointer;" Width="450px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblTipoPago" runat="server" Text="Tipo Pago:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTipoPago" runat="server" AutoPostBack="True" 
                                            style="cursor:pointer;" Width="450px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblAnioMes" runat="server" Text="Periodo:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlAnio" runat="server" Width="60px" AutoPostBack="True" style="cursor:pointer;"
                                            onselectedindexchanged="ddlAnio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="True" style="cursor:pointer;"
                                            onselectedindexchanged="ddlMes_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </td>
                                        
                                    <td>
                                        <asp:RadioButton ID="rbOrderFecha" runat="server" Text="Ordenar Por Fecha" 
                                            Checked="True" Visible="False" GroupName="A" /><br />
                                        <asp:RadioButton ID="rbOrderTarifa" runat="server" Text="Ordenar Por Tarifa" 
                                            Visible="False" GroupName="A"/>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblGuiaDespacho" runat="server" Text="Guía de Despacho:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlGuiaDespacho" runat="server" style="cursor:pointer;" 
                                            Width="130px">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkTodos" runat="server" Text ="Mostrar las Guias Enviadas" 
                                            AutoPostBack="True" oncheckedchanged="chkTodos_CheckedChanged" />
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                    </td>
                                    <td>
                                        <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:" CssClass="lblPadding"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        &nbsp;</td>
                                    <td colspan="3">                                                                                
                                        <asp:CheckBox ID="chkFechaActuacion" runat="server" style="cursor:pointer"
                                            Checked="True" 
                                            Text="Por fecha de Actuación" AutoPostBack="True" 
                                            oncheckedchanged="chkFechaActuacion_CheckedChanged" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkFechaAnulacion" runat="server" style="cursor:pointer"
                                            Text="Por fecha de Anulación" AutoPostBack="True" 
                                            oncheckedchanged="chkFechaAnulacion_CheckedChanged" />
                                        </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <%--Opciones--%>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td align="left" width="130px">
                                                    <asp:Button ID="btnDownload" runat="server" CssClass="btnDownload" Width="100px" Text="      Descargar" OnClick="lkbDescargar_Click"/>
                                                </td>
                                                <td align="left" width="130px">
                                                    <asp:Button ID="btnImprimir" runat="server" CssClass="btnPrint" Width="110px"  
                                                        Text="     Imprimir" onclick="btnImprimir_Click" />                                                                                                                                                            
                                                </td>
                                                <td align="left"  width="130px">
                                                    <asp:Button ID="btnExportarPDF" runat="server" CssClass="btnPDF" Width="110px"  
                                                        Text="      Descargar" onclick="btnExportarPDF_Click" />                                                       
                                                </td>
                                                <td align="left"  width="130px">
                                                <asp:Button ID="btnExportar" OnClientClick="return abrirPopupEspera();" runat="server"
                                                    Text="     Exportar / Imprimir" CssClass="btnPrint" Width="160px" 
                                                    onclick="btnExportar_Click" />
                                                </td>
                                            </tr>
                                        </table>                                                                                                                                                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>                                        
                                        <asp:Label ID="lblMsjDiasHabilesCierreCuenta" runat="server" Text="Label" CssClass="mensajePie" ></asp:Label>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <Label:Validation ID="ctrlValidacion" runat="server" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gdvReporte" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None">
                                                    <Columns>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                                                <h3 style="font-size:small">Procesando la información. Por favor espere...</h3>
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
    
<uc3:modalpopupextender ID="mdlpopup" 
                     BackgroundCssClass="modalbackground" runat="server" TargetControlID="btnShow"     
                     PopupControlID="pnl" >
</uc3:modalpopupextender> 
 <asp:Panel ID="pnl" runat="server"  
   CssClass="modalpopupmsg" BorderStyle="Solid" BorderWidth="1px">             
            <div class='headermsg'><span style="margin-left:5px;font-family:Arial; font-size:14px; font-weight:bold">Confirmar</span></div> 
            <asp:Button ID="btnShow" runat="server" Text="." CssClass="show"  />
            <br />
            <span style="margin-left:5px;font-family:Arial; font-family:Arial; font-size:12px;color:Black">
            ¿desea enviar la Guía de Despacho?</span>
             <br /><br />
             <div>
            <asp:Button ID="btnSIEnviarGuiaDespacho" runat="server" CssClass="btnOk" Width="100px"  
                     Text="      Enviar" onclick="btnSIEnviarGuiaDespacho_Click" />
                 &nbsp; &nbsp;&nbsp;
            <asp:Button ID="btnNO" runat="server" CssClass="btnUndo" Width="100px" Text="      Cancelar" />
            </div>    
   </asp:Panel>   
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
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
</asp:Content>
