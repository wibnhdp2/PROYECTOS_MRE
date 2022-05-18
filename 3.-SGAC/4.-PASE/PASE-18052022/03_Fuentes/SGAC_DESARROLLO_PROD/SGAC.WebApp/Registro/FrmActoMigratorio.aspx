<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="FrmActoMigratorio.aspx.cs"
    Inherits="SGAC.WebApp.Registro.FrmActoMigratorio" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlActuacionPago.ascx" TagName="ctrlActuacionPago"
    TagPrefix="SGAC_ActuacionPago" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlActuacionPago.ascx" TagName="CtrlPago_Visa"
    TagPrefix="SGAC_CtrlPago_Visa" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc2" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlAdjunto.ascx" TagName="ctrlAdjunto"
    TagPrefix="SGAC_Adjunto" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();

            var ddlAutorizado = $("#<%=ddlAutorizado.ClientID %>").val();
            if (ddlAutorizado == '0') {
                Desabilitar_Controles();
            }
        });

        function Load() {

            $(function () {
                $('input:text:first').focus();
                var $inp = $('input:text');
                $inp.bind('keydown', function (e) {

                    var key = e.which;
                    if (key == 13) {
                        e.preventDefault();
                        var nxtIdx = $inp.index(this) + 1;
                        $(":input:text:eq(" + nxtIdx + ")").focus();
                    }
                });
            });

            $("#<%=ddlFuncionario.ClientID %>").change(function () {
                var hId_TipoActuacion = $('#<%=hId_TipoActuacion.ClientID %>').val();
                var ddlFuncionario = $("#<%=ddlFuncionario.ClientID %>").val();
                $.ajax({
                    type: "POST",
                    url: "FrmActoMigratorio.aspx/ddlFuncionario_SelectedIndexChanged",
                    data: '{hId_TipoActuacion: ' + JSON.stringify(hId_TipoActuacion) + ', ddlFuncionario: ' + JSON.stringify(ddlFuncionario) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $("#<%=txt_Funcionario_Cargo.ClientID %>").val(msg.d);
                    },
                    error: errores

                });
            });

        }
    </script>
    <asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM2" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuación Consular"></asp:Label></h2>
            </td>
            <td style="text-align: right">
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Registro/FrmTramite.aspx"
                    Font-Bold="True" Font-Size="10pt" ForeColor="Blue" Font-Underline="true">Regresar a Trámites</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTituloTarifa" CssClass="titulo_tarifa" runat="server" Text="63a - Por expedir un salvoconducto, válido por 30 días, para un viaje de retorno al país."></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 95%;" align="center" class="mTblSecundaria" bgcolor="#4E102E">
        <tr>
            <td align="left">
                <table style="width: 100%;">
                    <tr>
                        <td class="style26">
                            <asp:Label ID="lblDestino" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                                Font-Size="10pt" Font-Underline="false" ForeColor="White" Text=""></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Button ID="btn_actualza_estado" runat="server" Text="Button" OnClick="btn_actualza_estado_Click"
                                        Style="display: none;" />
                                    <asp:Label ID="lblEstado" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                                        Font-Size="10pt" Font-Underline="false" ForeColor="White" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 95%;" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-5">
                            <asp:Label ID="Label2" runat="server" Text="Registro"></asp:Label></a></li>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblDatos" runat="server" Text="Formato"></asp:Label></a></li>
                        <li><a href="#tab-2">
                            <asp:Label ID="lblAdjuntos" runat="server" Text="Adjuntos"></asp:Label></a></li>
                        <li><a href="#tab-3">
                            <asp:Label ID="lblAnotacion" runat="server" Text="Anotaciones"></asp:Label></a></li>
                        <li><a href="#tab-4">
                            <asp:Label ID="lblAutoadhesivo" runat="server" Text="Vinculación"></asp:Label></a></li>
                    </ul>
                    <div id="tab-5">
                        <asp:UpdatePanel ID="updRegPago" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarRegistro" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <asp:HiddenField ID="hidEventNuevo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblValidacionRegistro" runat="server" Text="" ForeColor="Red" CssClass="hideControl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblFecAct" runat="server" Text="Fecha Registro: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblFecha" runat="server" Font-Bold="true"></asp:Label>
                                            <asp:HiddenField ID="hidPagoId" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblTarifaConsular" runat="server" Text="Tarifa Consular:"></asp:Label>
                                        </td>
                                        <td>
                                            <table style="border-width: 1px; border-color: #666; border-style: solid">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td style="vertical-align: top;">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtIdTarifa" runat="server" Width="38px" CssClass="campoNumero"
                                                                                    MaxLength="5" ToolTip="Coloque el número de la seccion del tarifario" Enabled="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel runat="server" ID="updBusTarifa" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtDescTarifa" runat="server" Width="653px" Enabled="False" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgBuscarTarifarioM" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                                                            ToolTip="Buscar" Visible="false" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:ListBox ID="LstTarifario" runat="server" Width="659px" AutoPostBack="True" BackColor="#EAFFFF"
                                                                                            Visible="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="txtIdTarifa" EventName="TextChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr style="display: none; visibility: hidden;">
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <SGAC_ActuacionPago:ctrlActuacionPago ID="ucCtrlActuacionPago" runat="server" />
                                    </tr>
                                    <tr runat="server">
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblTipPago" runat="server" Text="Tipo de Pago:"></asp:Label>
                                        </td>
                                        <td>
                                          <asp:HiddenField ID="hTipoPago" runat="server" />
                                            <asp:DropDownList ID="ddlTipoPago" runat="server" Height="20px" Width="200px" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="ddlTipoPago_SelectedIndexChanged"  />
                                            <asp:Label ID="lblCO_cmb_TipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblCantidad" runat="server" Text="Cantidad:"></asp:Label>
                                        </td>
                                        <td style="margin-left: 40px">
                                            <asp:TextBox ID="txtCantidad" runat="server" Width="100px" Enabled="false" CssClass="campoNumero"
                                                MaxLength="2" />
                                            &nbsp;
                                            <asp:Label ID="lblCO_txtCantidad" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" id="pnlPagLima" runat="server" visible="false">
                                            <table class="mTblSecundaria">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblNroOperacion" runat="server" Text="Nro de Operación :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNroOperacion" runat="server" Width="200px" MaxLength="20" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblNombBanco" runat="server" Text="Nombre del Banco :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlNomBanco" runat="server" Width="300px" MaxLength="10" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFecPago" runat="server" Text="Fecha de Pago :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <SGAC_Fecha:ctrlDate ID="ctrFecPago" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblMtoCancelado" runat="server" Text="Monto Cancelado :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMtoCancelado" runat="server" Width="150px" CssClass="campoNumero" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblMtoSC" runat="server" Text="Monto S/C:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMontoSC" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />
                                            &nbsp;<asp:Label ID="lblMtoEn" runat="server" Text="Monto en:" Width="60px"></asp:Label>&nbsp;
                                            <asp:TextBox ID="txtMontoML" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />&nbsp;
                                            <asp:Label ID="LblDescMtoML" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr noshade="noshade" size="2" style="width: 90%;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblTOTALSC" runat="server" Text="TOTAL S/C: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTotalSC" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />
                                            &nbsp;<asp:Label ID="lblTotalEn" runat="server" Text="TOTAL en:" Width="60px"></asp:Label>&nbsp;
                                            <asp:TextBox ID="txtTotalML" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />&nbsp;
                                            <asp:Label ID="LblDescTotML" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblObservacionesAct" runat="server" Text="Observaciones: "></asp:Label><br />
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtObservaciones" runat="server" Height="30px" Width="700px" TextMode="MultiLine"
                                                CssClass="txtLetra" MaxLength="500" />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <hr noshade="noshade" size="2" style="width: 90%;" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-1">
                        <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="display: inline-block; width: 70%;">
                                    <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarActuacion" runat="server" />
                                    <asp:HiddenField ID="hId_Estado" runat="server" Value="0" />
                                    <asp:HiddenField ID="hd_pantalla" runat="server" Value="0" />
                                    <asp:HiddenField ID="hacmi_iActoMigratorioId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hacmi_iActuacionDetalleId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hiActoMigratorioFormatoId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hId_Persona" runat="server" />
                                    <asp:HiddenField ID="hdn_resi_iResidenciaId_peru" runat="server" />
                                    <asp:HiddenField ID="hdn_resi_iResidenciaId_extranjero" runat="server" />
                                    <asp:HiddenField ID="hTab_Index" runat="server" />
                                    <asp:HiddenField ID="hdn_acmi_sTipoId" runat="server" />
                                    <asp:HiddenField ID="hValida_Lamina" runat="server" />
                                    <asp:Button ID="btnPersona" runat="server" OnClick="btnPersona_Click" Text="Clase Persona"
                                        Style="display: none;" />
                                    <asp:HiddenField ID="hdn_CONFORMIDAD_DE_TEXTO" runat="server" />
                                    <asp:HiddenField ID="hDocumento_Pasaporte" runat="server" />
                                    <asp:HiddenField ID="hDocumento_Salvoconducto" runat="server" />
                                    <asp:HiddenField ID="hDocumento_Visa" runat="server" />
                                    <asp:HiddenField ID="hPasaporte_Expedido" runat="server" />
                                    <asp:HiddenField ID="hPasaporte_Revalidado" runat="server" />
                                    <asp:HiddenField ID="hVisa_Estado_Registrado" runat="server" />
                                    <asp:HiddenField ID="hSalvoconducto_Estado_Registrado" runat="server" />
                                    <asp:HiddenField ID="hVisaTipo_Temporal" runat="server" />
                                    <asp:HiddenField ID="hPago_Lima" runat="server" />
                                </div>
                                <div style="display: inline-block; width: 25%; text-align: right;">
                                </div>
                                <br />
                                <div>
                                    <asp:UpdatePanel ID="updCaberaFormato" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td colspan="9">
                                                                    <asp:Label ID="lbl_Texto12" runat="server" Text="Ingrese este dato si la Visa ha sido Aprobada."
                                                                        ForeColor="Maroon"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Text="Nro. Exp: " Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>
                                                                  <%--  <asp:TextBox ID="txtNumeroExp" runat="server" Width="135px" CssClass="campoNumero"
                                                                        MaxLength="30" onkeypress="return isNumberKey(event)" onBlur="conMayusculas(this)" />--%>
                                                                        <asp:TextBox ID="txtNumeroExp" runat="server" Width="135px" CssClass="campoNumero"
                                                                        MaxLength="5" onpaste="return false" Enabled="False">000</asp:TextBox>
                                                                    <asp:Label ID="lblCO_txtNumeroExp" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTexto_1" runat="server" Font-Bold="true"></asp:Label><br />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNumeroPass" runat="server" Width="100px" onkeypress="return NoCaracteresEspeciales(event)"
                                                                        Style="text-transform: uppercase;" onBlur="conMayusculas(this)" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTexto_2" runat="server" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNumSal_Lam" runat="server" Width="135px" onkeypress="return NoCaracteresEspeciales(event)"
                                                                        Style="text-transform: uppercase;" onBlur="conMayusculas(this)" maxLenght="30" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTexto_5" runat="server" Font-Bold="true" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOtros" runat="server" Width="100px" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)"
                                                                        Style="text-transform: uppercase;" onBlur="conMayusculas(this)" />
                                                                    <asp:Label ID="lblCO_txtOtros" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTexto_3" runat="server" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <SGAC_Fecha:ctrlDate ID="txtFecExpedicion" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTexto_4" runat="server" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <SGAC_Fecha:ctrlDate ID="txtFecExpiracion" runat="server" />
                                                                    <asp:Label ID="Label19" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="tr_Pais" runat="server" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label32" runat="server" Text="País:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPais" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="IdVisa_1">
                                    <asp:Panel ID="pnlVisa_1" runat="server" Visible="false">
                                        <div>
                                            <asp:UpdatePanel ID="UpnlTipoVisa" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="tblVisa_1" runat="server" style="width: 100%;">
                                                        <tr>
                                                            <td colspan="9" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                                <asp:Label ID="lblDatosVisa" Text="1. PROCEDIMIENTO A REALIZAR" runat="server" Style="width: 100%;
                                                                    font-weight: 700; color: #800000;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="45px" height="25px">
                                                                <asp:Label ID="Label1" runat="server" Text="Tipo: " />
                                                            </td>
                                                            <td width="100px">
                                                                <asp:DropDownList ID="ddlVisaTipo" runat="server" Width="90px" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlVisaTipo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label15" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td width="40px">
                                                                <asp:Label ID="lblVIsa" runat="server" Text="Visa:"></asp:Label>
                                                            </td>
                                                            <td width="215px">
                                                                <asp:DropDownList ID="ddlVisaSubTipo" runat="server" Width="210px" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlVisaSubTipo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label16" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td width="50px">
                                                                <asp:Label ID="Label6" runat="server" Text="Titular/Familia: " />
                                                            </td>
                                                            <td width="110px">
                                                                <asp:DropDownList ID="ddlTitularFamiliar" runat="server" Width="100px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label17" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblSigla" runat="server" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td width="125px">
                                                                <asp:Label ID="Label466" runat="server" Text="T. Permanencia (días):"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTiempoPermanencia" runat="server" Width="25px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:Label ID="Label18" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="check_3" runat="server">
                                                            <td colspan="3" id="check_1">
                                                            </td>
                                                            <td colspan="5" id="check_2">
                                                                <asp:CheckBox runat="server" ID="chkAcuerdoAP" Text="Marcar Casilla si corresponde al Acuerdo AP-Programa de Vacaciones y Trabajo"
                                                                    OnCheckedChanged="chkAcuerdoAP_CheckedChanged" AutoPostBack="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="lblDatosSolicitante" Text="DATOS GENERALES DEL TITULAR" runat="server"
                                                    Style="width: 100%; font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Debe ingresar los campos requeridos (*)"
                                                    CssClass="hideControl" ForeColor="Red" Font-Size="Small">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblTitularTipoDocumento" runat="server" Text="Tipo Documento:" />
                                            </td>
                                            <td style="width: 210px">
                                                <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="190px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlTipoDocumento" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularNumDocumento" runat="server" Text="Número Documento: " />
                                            </td>
                                            <td style="width: 220px">
                                                <asp:TextBox ID="txtNumDocumento" runat="server" Width="185px" CssClass="campoNumero"
                                                    onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" />
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularGenero" runat="server" Text="Género: " />
                                            </td>
                                            <td style="width: 220px">
                                                <asp:DropDownList ID="ddlGenero" runat="server" Width="185px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblTitularFecNacimiento" runat="server" Text="Fecha Nacimiento:" />
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecNacimiento" runat="server" />
                                                <asp:Label ID="Label20" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularEstadoCivil" runat="server" Text="Estado Civil: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEstadoCivil" runat="server" Width="190px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularOcupacion" runat="server" Text="Ocupación: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOcupacionAct" runat="server" Width="185px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitularPrimerApe" runat="server" Text="Primer Apellido: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApePat" runat="server" Width="195px" CssClass="txtLetra" Enabled="false"
                                                    MaxLength="50" onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" />
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularSegundoApe" runat="server" Text="Segundo Apellido: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApeMat" runat="server" Width="190px" CssClass="txtLetra" MaxLength="50"
                                                    onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" Enabled="False" />
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularNombres" runat="server" Text="Nombres: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombres" runat="server" Width="185px" CssClass="txtLetra" Enabled="false"
                                                    MaxLength="50" onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="lblTituloLugarNacimiento" runat="server" Text="Lugar de Nacimiento:"
                                                    Font-Bold="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitularNacimientoDepa" runat="server" Text="Departamento / Continente: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioNacDepa" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioNacDepa_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDepNac" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularNacimientoProv" runat="server" Text="Povincia / País: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioNacProv" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioNacProv_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioNacProv" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitularNacimientoDist" runat="server" Text="Distrito / Ciudad: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioNacDist" runat="server" Width="180px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioNacDist" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="lblTituloDomicilioPeru" runat="server" Text="Domicilio en el Perú:"
                                                    Font-Bold="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblResidenciaDepartamento" runat="server" Text="Departamento: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioPeruDepa" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioPeruDepa_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label27" runat="server" Style="color: #FF0000" Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblResidenciaProvincia" runat="server" Text="Provincia: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioPeruProv" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioPeruProv_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioPeruProv" runat="server" Style="color: #FF0000"
                                                    Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblResidenciaDistrito" runat="server" Text="Distrito: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioPeruDist" runat="server" Width="180px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioPeruDist" runat="server" Style="color: #FF0000"
                                                    Text="*" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblResidenciaDireccion" runat="server" Text="Dirección: " />
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtDomicilioPeru" runat="server" Width="507px" CssClass="txtLetra"
                                                    onBlur="conMayusculas(this)" MaxLength="200" />
                                                <asp:Label ID="Label30" runat="server" Style="color: #FF0000" Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblResidenciaTelefono" runat="server" Text="Teléfono: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelefonoPeru" runat="server" Width="185px" CssClass="txtLetra"
                                                    onkeypress="return isNumberKey(event)" MaxLength="25" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="Label7" runat="server" Text="Domicilio en el Extranjero:" Font-Bold="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExtranjeroContinente" runat="server" Text="Continente: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioExtrDepa" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioExtrDepa_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label28" runat="server" Style="color: #FF0000" Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExtranjeroPais" runat="server" Text="País: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioExtrProv" runat="server" Width="190px" Height="22px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDomicilioExtrProv_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioExtrProv" runat="server" Style="color: #FF0000"
                                                    Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExtranjeroCiudad" runat="server" Text="Ciudad: " />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDomicilioExtrDist" runat="server" Width="185px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlDomicilioExtrDist" runat="server" Style="color: #FF0000"
                                                    Text="*" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExtranjeroDireccion" runat="server" Text="Dirección: " />
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtDomicilioExtra" runat="server" Width="507px" CssClass="txtLetra"
                                                    onBlur="conMayusculas(this)" />
                                                <asp:Label ID="Label31" runat="server" Style="color: #FF0000" Text="*" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExtranjeroTelefono" runat="server" Text="Teléfono: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelefonoExtr" runat="server" Width="185px" CssClass="txtLetra"
                                                    onkeypress="return isNumberKey(event)" MaxLength="25" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divFiliacion" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="Label29" Text="CARACTERÍSTICAS FÍSICAS" runat="server" Style="width: 100%;
                                                    font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80px">
                                                <asp:Label ID="lblTitularColorOjos" runat="server" Text="Color Ojos:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlColorOjo" runat="server" Width="200px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlColorOjo" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td style="width: 92px">
                                                <asp:Label ID="lblTitularColorCabello" runat="server" Text="Color Cabello:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlColorCabello" runat="server" Width="200px" Height="22px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCO_ddlColorCabello" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td style="width: 60px">
                                                <asp:Label ID="lblTitularEstatura" runat="server" Text="Estatura: " />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEstatura" runat="server" Width="100px" CssClass="campoNumero"
                                                    onkeypress="return isDecimalKey(this,event)" MaxLength="6" />
                                                <asp:Label ID="lblCO_txtEstatura" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                <asp:Label ID="lblEtiquetaEstatura" runat="server" Text="Mts." CssClass="lblEtiquetaLeyenda"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="Label10" Text="FILIACIÓN" runat="server" Style="width: 100%; font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:HiddenField ID="hId_Padre" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblTitularFiliacionPadre" runat="server" Text="Vínculo:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTitularFiliacionPadre" runat="server" Width="200px" Height="22px"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPadreNombres" runat="server" Text="Datos: " />
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtPadreNombres" runat="server" Width="95%" CssClass="txtLetra"
                                                    onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" />
                                                <asp:Label ID="Label33" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:HiddenField ID="hId_Madre" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblTitularFiliacionMadre" runat="server" Text="Vínculo:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTitularFiliacionMadre" runat="server" Width="200px" Height="22px"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 10px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMadreNombres" runat="server" Text="Datos: " />
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtMadreNombres" runat="server" Width="95%" CssClass="txtLetra"
                                                    onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" />
                                                <asp:Label ID="Label34" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="IdVisa_2">
                                    <asp:Panel ID="pnlVisa_2" runat="server" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                    border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                    <asp:Label ID="Label11" runat="server" Text="3. RESERVADO PARA USO INTERNO (OFFICIAL USE ONLY)"
                                                        Style="font-weight: 700; color: #800000;" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td height="22px">
                                                        <asp:DropDownList ID="ddlAutorizado" runat="server" Width="600px" Height="22px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlAutorizado_SelectedIndexChanged">
                                                            <asp:ListItem Text="AUTORIZADO POR LA MISIÓN O SECCIÓN (PARA EL CASO DE VISAS RESIDENTES CONSIDERAR ANTECEDENTES"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label22" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblRREEDoc" runat="server" Text="Tipo Doc (RREE):" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="txtRREEDoc" runat="server" Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblRREENum" runat="server" Text="Número (RREE):" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRREENum" runat="server" Width="100px" onkeypress="return isNumberKey(event)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRREEFecha" runat="server" Text="Fecha (RREE):" Width="100px" />
                                                </td>
                                                <td style="width: 170px;">
                                                    <SGAC_Fecha:ctrlDate ID="txtRREEFecha" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDIGEMINDoc" runat="server" Text="Tipo Doc. (DIGEMIN):" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="txtDIGEMINDoc" runat="server" Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDIGEMINNum" runat="server" Text="Número (DIGEMIN):" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDIGEMINNum" runat="server" Width="100px" onkeypress="return isNumberKey(event)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDIGEMINFecha" runat="server" Text="Fecha (DIGEMIN):" Width="100px" />
                                                </td>
                                                <td style="width: 170px;">
                                                    <SGAC_Fecha:ctrlDate ID="txtDIGEMINFecha" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <div id="IdOtros">
                                        <table width="100%" id="tbOtros" runat="server">
                                            <tr>
                                                <td colspan="5" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                    border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                    <asp:Label ID="Label4" runat="server" Text="OTROS" Style="font-weight: 700; color: #800000;" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%; border-bottom-color: #800000;">
                                            <tr>
                                                <td width="150px">
                                                    <asp:Label ID="lbl_Pasaporte_Anterior" runat="server" Text="Nro. Pasaporte Anterior:" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_nroPasaporte_Anterior" runat="server" onkeypress="return NoCaracteresEspeciales(event)"
                                                        Style="text-transform: uppercase;" onBlur="conMayusculas(this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150px" height="22px">
                                                    <asp:Label ID="Label8" runat="server" Text="Funcionario:" />
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlFuncionario" runat="server" Width="98%">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddlFuncionario" runat="server" Text="*" ForeColor="Red" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="150px">
                                                    <asp:Label ID="lbl_Cargo_funcionario" runat="server" Text="Cargo" Width="22px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txt_Funcionario_Cargo" runat="server" Width="98%" Enabled="false"
                                                        onBlur="conMayusculas(this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divRevalidado" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="6" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                    border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                    <asp:Label ID="Label5" runat="server" Text="DATOS DE PASAPORTE A REVALIDAR" Style="font-weight: 700;
                                                        color: #800000;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOficinaPasaporte" runat="server" Text="Oficina Consular donde obtuve el pasaporte:" />
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddl_amfr_sOficinaConsularId" runat="server" Width="300px" Height="22px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_amfr_sOficinaConsularId_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label38" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOficinaMigraciones" runat="server" Text="Oficina de Migraciones donde obtuvo el pasaporte:" />
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddl_amfr_sOficinaMigracionId" runat="server" Width="300px"
                                                        Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddl_amfr_sOficinaMigracionId_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label39" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblFecPasaporteExpedido" runat="server" Text="Fecha Expedición:" />
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="txt_amfr_dFechaExpedicion" runat="server" />
                                                    <asp:Label ID="Label40" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                                <td style="width: 10px">
                                                </td>
                                                <td style="width: 70px">
                                                    <asp:Label ID="lblFecPasaporteExpirado" runat="server" Text="Fecha Expiración:" />
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="txt_amfr_dFechaExpiracion" runat="server" />
                                                    <asp:Label ID="Label41" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:Panel ID="pnlVisa_3" runat="server" Visible="false">
                                        <div id="VDiplomatica" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="5" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                        border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                        <asp:Label ID="Label12" runat="server" Text="4. SOLO PARA VISAS DIPLÓMATICAS, OFICIALES Y ESPECIALES"
                                                            Style="font-weight: 700; color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td width="16%">
                                                        <asp:Label ID="Label45" runat="server" Text="Cargo:" />
                                                    </td>
                                                    <td width="34%">
                                                        <asp:DropDownList ID="cbo_cargo_diplomatico" runat="server" Width="97%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="16%">
                                                        <asp:Label ID="Label46" runat="server" Text="Motivo:" />
                                                    </td>
                                                    <td width="34%" align="right">
                                                        <asp:TextBox ID="txtMotivo" runat="server" Width="97%" CssClass="txtLetra" onBlur="conMayusculas(this)"
                                                            onkeypress="return NoCaracteresEspeciales(event)" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label47" runat="server" Text="Institución que inicialmente solicitó la visa:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInstSolicito" runat="server" Width="97%" CssClass="txtLetra"
                                                            onBlur="conMayusculas(this)" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label48" runat="server" Text="Institución a través del cual se realiza el trámite:" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtInstTramito" runat="server" Width="97%" CssClass="txtLetra" onBlur="conMayusculas(this)"
                                                            onkeypress="return NoCaracteresEspeciales(event)" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label49" runat="server" Text="Oficina de Cancillería que solicita autorización:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="dllOficinaConcilleria" runat="server" Width="98%">
                                                            <asp:ListItem Text=" - SELECCIONE - " />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label50" runat="server" Text="Documento que autoriza la visa:" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtDocAutoriza" runat="server" Width="97%" CssClass="txtLetra" onBlur="conMayusculas(this)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="vPrensa" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="5" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                        border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                        <asp:Label ID="lblVisaPrensaGlosa" runat="server" Text="5. SOLO PARA VISAS DE PRENSA"
                                                            Style="font-weight: 700; color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td width="16%">
                                                        <asp:Label ID="Label51" runat="server" Text="Medio de Comunicación:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVisaMedioComu" runat="server" Width="310px" CssClass="txtLetra"
                                                            onBlur="conMayusculas(this)" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="16%">
                                                        <asp:Label ID="lblVisaPrensaCargo" runat="server" Text="Cargo:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cbo_cargo_prensa" runat="server" Width="310px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVisaPrensaMotivo" runat="server" Text="Motivo:" Width="120px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVisaPrensaMotivo" runat="server" Width="310px" CssClass="txtLetra"
                                                            onBlur="conMayusculas(this)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div id="IdObservaciones">
                                    <table style="border-bottom: 1px solid #800000; width: 100%; border-top-color: #800000;">
                                        <tr>
                                            <td colspan="5" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="Label13" runat="server" Text="OBSERVACIONES" Style="font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtObservacion" runat="server" Height="40px" Width="100%" TextMode="MultiLine"
                                                    CssClass="txtLetra" onBlur="conMayusculas(this)" />
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="7">
                                                <div class="checkbox text-center">
                                                    <asp:CheckBox ID="chk_verificar" runat="server" Text="" Font-Names="Verdana" Font-Bold="True"
                                                        Font-Size="12pt" AutoPostBack="True" OnCheckedChanged="chk_verificar_CheckedChanged" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:HiddenField ID="hId_TipoActuacion" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="UpdAdjuntos" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_solicitar" runat="server" Text="     Solicitar" OnClientClick="return confirma_solicita();"
                                    CssClass="btnSave" Style="display: none;" />
                                <asp:Button ID="btn_Enviar_Solicitar" runat="server" Text="     Solicitar" CssClass="btnSave"
                                    OnClick="btn_solicitar_Click" Style="display: none;" />
                                <br />
                                <SGAC_Adjunto:ctrlAdjunto runat="server" ID="ctrlAdjunto" />
                                <br />
                                <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;
                                    border-top-width: 1px; border-bottom-width: 1px; border-top-color: #800000; border-top-style: solid;
                                    border-bottom-style: solid;" align="center">
                                    <tr>
                                        <td colspan="5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%" align="center">
                                            <asp:Image ID="imgFoto" runat="server" Height="100px" Width="100px" ImageUrl="~/Images/imagen-no-disponible.jpg" />
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 25%" align="center">
                                            <asp:Image ID="imgHuella" runat="server" Height="100px" Width="100px" ImageUrl="~/Images/imagen-no-disponible.jpg" />
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 25%" align="center">
                                            <asp:Image ID="imgFirma" runat="server" Height="100px" Width="100px" ImageUrl="~/Images/imagen-no-disponible.jpg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 25%">
                                            <strong>Foto</strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td align="center" style="width: 25%">
                                            <strong>Huella</strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td align="center" style="width: 25%">
                                            <strong>Firma</strong>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-3">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <ToolBarButton:ToolBarButtonContent ID="ToolBarButtonContent_Pagos" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <SGAC_CtrlPago_Visa:CtrlPago_Visa ID="CtrlPago_Visa" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label57" runat="server" Text="Monto S/C:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_montosc" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />
                                            &nbsp;<asp:Label ID="Label58" runat="server" Text="Monto en:" Width="60px"></asp:Label>&nbsp;
                                            <asp:TextBox ID="txt_montoml" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />&nbsp;
                                            <asp:Label ID="lbl_monto" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr noshade="noshade" size="2" style="width: 90%;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label9" runat="server" Text="TOTAL S/C: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_total_sc" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />
                                            &nbsp;<asp:Label ID="Label25" runat="server" Text="TOTAL en:" Width="60px"></asp:Label>&nbsp;
                                            <asp:TextBox ID="txt_total" runat="server" Width="150px" Enabled="False" CssClass="campoNumero" />&nbsp;
                                            <asp:Label ID="lbl_monto_total" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-4">
                        <asp:UpdatePanel ID="updVinculacion" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button Text="    Grabar" CssClass="btnSave" runat="server" ID="btnGrabarVinculacion"
                                                OnClick="btnGrabarVinculacion_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblValidacionDetalle" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="Validar Código del Autoadhesivo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 350px" colspan="2">
                                            <h3>
                                                <asp:Label ID="Label36" runat="server" Text="PASO 1: Vinculación Insumos"></asp:Label></h3>
                                        </td>
                                        <td style="width: 300px" colspan="2">
                                            <h3>
                                                <asp:Label ID="Label14" runat="server" Text="PASO 2: Vista Previa e Impresión"></asp:Label></h3>
                                        </td>
                                        <td style="width: 300px" colspan="2">
                                            <h3>
                                                <asp:Label ID="Label35" runat="server" Text="PASO 3: Aprobación Impresión"></asp:Label></h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label37" runat="server" Text="Código:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCodAutoadhesivo" runat="server" Width="132px" MaxLength="15"
                                                CssClass="txtLetra"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnVistaPrev" runat="server" Text="   Autoadhesivo" Width="180px"
                                                CssClass="btnPrint" TabIndex="50" OnClick="btnVistaPrev_Click" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_PrintAutoadhesivo" runat="server" Text="" OnCheckedChanged="chk_PrintAutoadhesivo_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label26" runat="server" Text="Impresión Correcta"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label44" runat="server" Text="Código:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCodLamina" runat="server" Width="132px" MaxLength="15" CssClass="txtLetra"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btn_Lamina" runat="server" Text="   Lámina" Width="180px" CssClass="btnPrint"
                                                OnClick="btn_Lamina_Click" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_PintLamina" runat="server" Text="" OnCheckedChanged="chk_PintLamina_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label24" runat="server" Text="Impresión Correcta"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text="Código:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCodPassaporte" runat="server" Width="132px" MaxLength="15" CssClass="txtLetra"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btn_formato" runat="server" Text="   Formato DGC-006" Width="180px"
                                                CssClass="btnPrint" OnClick="btn_formato_Click" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_passaporte" runat="server" Text="" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="Impresión Correcta"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td colspan="6">
                                            <asp:Button ID="btn_confirmar" runat="server" Text="   Acta de Conformidad" Width="180px"
                                                CssClass="btnPrint" OnClick="btn_confirmar_Click" />
                                            <br />
                                            <asp:Button ID="btnDesabilitarAutoahesivo" runat="server" OnClick="btnDesabilitarAutoahesivo_Click"
                                                Text="Oculto" Visible="True" CssClass="hideControl" />
                                            <asp:Button ID="btnDesabilitarLamina" runat="server" OnClick="btnDesabilitarLamina_Click"
                                                Text="Oculto" Visible="True" CssClass="hideControl" />
                                            <asp:HiddenField ID="hdn_ImpresionCorrecta" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="Grd_ActInsDet" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                AutoGenerateColumns="False" GridLines="None">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="aide_iActuacionInsumoDetalleId" HeaderText="idActuacionInsumoDetalle"
                                                        Visible="False" />
                                                    <asp:BoundField DataField="aide_iActuacionDetalleId" HeaderText="idActuacionDetalle"
                                                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" Visible="False">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="insu_iInsumoId" HeaderText="idInsumo" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta" Visible="False">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="aide_dFechaVinculacion" HeaderText="Fecha y Hora" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="USUARIO" HeaderText="Usuario">
                                                        <ItemStyle Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="insu_sInsumoTipoId" HeaderText="Tipo Insumo">
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="aide_bFlagImpresion" HeaderText="Impresión Correcta">
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="insu_vCodigoUnicoFabrica" HeaderText="Código de Insumo">
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CantidadImpresiones" HeaderText="Cantidad Impresiones">
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc2:ctrlPageBar ID="CtrlPageBarActuacionInsumoDetalle" runat="server" OnClick="ctrlPagActuacionInsumoDetalle_Click"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="modalEspera" class="modal">
                                        <div class="modal-window">
                                            <div class="modal-titulo">
                                                <span>GRABANDO...</span>
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
                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    <script type="text/javascript">


        function validaHora(ctrl) {
            var x = ctrl.value;
            var valor = true;

            if (x.length == 4) {
                x = x + "0";
                ctrl.value = x;
            }

            var hora = x.substring(0, 2);
            var minuto = x.substring(3, 5);

            if (hora > 23) {
                valor = false;
            }
            if (minuto > 59) {
                valor = false;
            }
            if (x.length < 5) {
                valor = false;
            }
            if (x.length == 0) {
                valor = true;
            }
            if (!valor) {

                ctrl.focus();
                showdialog('a', 'Acto Migratorio', 'Formato de hora Incorrecto', false, 190, 320);
                ctrl.value = "";

            }
        }
        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }
        function isHoraKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = ":";
            var x = ctrl.value;
            var y = x.indexOf(FIND);

            var minuto = x.split(':');
            if (x.length == 2) {
                x = x + ":";
                ctrl.value = x;
            }

            if (x.split(':').length = 1) {
                if (x.length > 1 && x.length < 3) {
                    if (minuto[0].length > 1 && charCode != 58) {
                        return false;
                    }
                }
            }

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 58) {
                return false;
            }

            if (charCode < 48 || charCode > 58)
                return false;
            return true;
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

        function isNombreApellido(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 166) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;

        }

        function NoCaracteresEspeciales(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            var letras = "-*?´(),;.:_|°=¿#%&/{}[]+!<>~@'$¡";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = false;
            }

            return letra;

        }



        function isLetraNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
                letra = true;
            }

            if (charCode > 39 && charCode < 60) {
                letra = true;
            }
            if (charCode > 63 && charCode < 91) {
                letra = true;
            }
            if (charCode > 94 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
                letra = true;
            }
            if (charCode > 159 && charCode < 164) {
                letra = true;
            }
            if (charCode == 181) {
                letra = true;
            }
            if (charCode == 214) {
                letra = true;
            }
            if (charCode == 224) {
                letra = true;
            }
            if (charCode == 233) {
                letra = true;
            }


            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;

        }


        function validanumeroLostFocus(control) {
            var valor = true;
            var texto = control.value.trim();
            control.value = texto;

            var letras = "0123456789.";
            var n = 0;
            while (n < texto.length) {
                var x = texto.substring(n, n + 1)
                if (letras.indexOf(x) < 0) {
                    valor = false;
                }
                n++;
            }

            if (texto.substring(0, 1) == ".") {
                valor = false;
            }

            if (texto.substring(n - 1, n) == ".") {
                valor = false;
            }

            if (!valor) {
                control.focus();
                showdialog('a', 'Acto Migratorio', 'Formato de hora Incorrecto', false, 190, 320);
                control.value = "";
            }

        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            return letra;
        }

        function isDecimalKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = "."
            var x = ctrl.value
            var y = x.indexOf(FIND)

            if (charCode == 46) {
                if (y != -1 || x.length == 0)
                    return false;
            }
            if (charCode != 46 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function confirma_solicita() {
            showdialog('c', 'Acto Migratorio', '¿Está seguro que desea solicitar este Trámite?', false, 190, 320, function () {
                $('#<%=btn_Enviar_Solicitar.ClientID %>').click();
                $("#msg-dialog").dialog('close');
            });
            return false;
        }
        
    </script>
    <script type="text/javascript">

        function ValidarGrabar() {

            var s_Valiar = false;


            var Index_TabControl = document.getElementById('<%= hTab_Index.ClientID %>').value;

            if (Index_TabControl == '0') {

                return ValidarRegistroActuacion();
            }
            var s_Tarifa = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;

            switch (s_Tarifa) {
                case document.getElementById('<%= hDocumento_Salvoconducto.ClientID %>').value:
                    s_Valiar = Validar_Salvoconducto();
                    break;
                case document.getElementById('<%= hDocumento_Visa.ClientID %>').value:
                    s_Valiar = Validarformato_visa();
                    break;
                case document.getElementById('<%= hDocumento_Pasaporte.ClientID %>').value:
                    var s_Tipo_Pasaporte = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;
                    switch (s_Tipo_Pasaporte) {
                        case document.getElementById('<%= hPasaporte_Expedido.ClientID %>').value:
                            s_Valiar = Validar_ExpePasaporte();
                            break;
                        case document.getElementById('<%= hPasaporte_Revalidado.ClientID %>').value:
                            s_Valiar = Validar_RenovPasaporte();
                            break;
                    }
                    break;
            }

            if (s_Valiar) {
                if (bol_Imprimir == false) {
                    showdialog('c', 'Acto Migratorio', '¿Está seguro que desea grabar la información?', false, 190, 320, Valiar_Grabar_Migratorio);
                }
                else {
                    Valiar_Grabar_Migratorio();
                }

            }
        }


        function Valiar_Grabar_Migratorio() {




            var s_Tarifa = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;

            var migratorio = {};

            migratorio.acmi_iActoMigratorioId = document.getElementById('<%= hacmi_iActoMigratorioId.ClientID %>').value;
            migratorio.acmi_iActuacionDetalleId = document.getElementById('<%= hacmi_iActuacionDetalleId.ClientID %>').value;
            migratorio.acmi_IFuncionarioId = document.getElementById('<%= ddlFuncionario.ClientID %>').value;
            migratorio.acmi_sTipoDocumentoMigratorioId = document.getElementById('<%= ddlTipoDocumento.ClientID %>').value;
            migratorio.acmi_sTipoActoMigratorio = s_Tarifa;
            migratorio.acmi_bImprimir = bol_Imprimir;

            switch (s_Tarifa) {
                case document.getElementById('<%= hDocumento_Visa.ClientID %>').value:
                    migratorio.acmi_sTipoId = document.getElementById('<%= ddlVisaTipo.ClientID %>').value;
                    migratorio.acmi_sSubTipoId = document.getElementById('<%= ddlVisaSubTipo.ClientID %>').value;
                    migratorio.acmi_sEstadoId = document.getElementById('<%= hVisa_Estado_Registrado.ClientID %>').value;

                    migratorio.amfr_iActoMigratorioFormatoId = document.getElementById('<%= hiActoMigratorioFormatoId.ClientID %>').value;
                    migratorio.amfr_iActoMigratorioId = document.getElementById('<%= hacmi_iActoMigratorioId.ClientID %>').value;
                    migratorio.amfr_sTitularFamiliaId = document.getElementById('<%= ddlTitularFamiliar.ClientID %>').value;
                    if (migratorio.acmi_sSubTipoId == '9061') {
                        migratorio.amfr_bAcuerdoProgramaFlag = document.getElementById('<%= chkAcuerdoAP.ClientID %>').checked;
                    }
                    else {
                        migratorio.amfr_bAcuerdoProgramaFlag = "false";
                    }
                    migratorio.amfr_vVisaCodificacion = document.getElementById('<%= txtNumeroPass.ClientID %>').value;
                    migratorio.amfr_sDiasPermanencia = document.getElementById('<%= txtTiempoPermanencia.ClientID %>').value;
                    migratorio.amfr_sTipoAutorizacionId = document.getElementById('<%= ddlAutorizado.ClientID %>').value;
                    migratorio.amfr_sTipoDocumentoRREEId = document.getElementById('<%= txtRREEDoc.ClientID %>').value;
                    migratorio.amfr_vNumDocumentoRREE = document.getElementById('<%= txtRREENum.ClientID %>').value;
                    migratorio.amfr_dFechaRREE = document.getElementById('<%= txtRREEFecha.FindControl("TxtFecha").ClientID %>').value;
                    migratorio.amfr_sTipoDocumentoDIGEMINId = document.getElementById('<%= txtDIGEMINDoc.ClientID %>').value;
                    migratorio.amfr_vNumDocumentoDIGEMIN = document.getElementById('<%= txtDIGEMINNum.ClientID %>').value;
                    migratorio.amfr_dFechaDIGEMIN = document.getElementById('<%= txtDIGEMINFecha.FindControl("TxtFecha").ClientID %>').value;

                    migratorio.amfr_vCargoFuncionario = document.getElementById('<%= txt_Funcionario_Cargo.ClientID %>').value;

                    if (document.getElementById('<%= cbo_cargo_diplomatico.ClientID %>') != null)
                        migratorio.amfr_sCargoDiplomaticoId = document.getElementById('<%= cbo_cargo_diplomatico.ClientID %>').value;
                    else
                        migratorio.amfr_sCargoDiplomaticoId = 0;
                    if (document.getElementById('<%= txtMotivo.ClientID %>') != null)
                        migratorio.amfr_vMotivoVisaDiplomatica = document.getElementById('<%= txtMotivo.ClientID %>').value;
                    else
                        migratorio.amfr_vMotivoVisaDiplomatica = "";
                    if (document.getElementById('<%= txtInstSolicito.ClientID %>') != null)
                        migratorio.amfr_vInstitucionSolicitaVisaDiplomatica = document.getElementById('<%= txtInstSolicito.ClientID %>').value;
                    else
                        migratorio.amfr_vInstitucionSolicitaVisaDiplomatica = "";
                    if (document.getElementById('<%= txtInstTramito.ClientID %>') != null)
                        migratorio.amfr_vInstitucionRealizaVisaDiplomatica = document.getElementById('<%= txtInstTramito.ClientID %>').value;
                    else
                        migratorio.amfr_vInstitucionRealizaVisaDiplomatica = "";
                    if (document.getElementById('<%= dllOficinaConcilleria.ClientID %>') != null)
                        migratorio.amfr_sCancilleriaSolicitaAutorizacionId = document.getElementById('<%= dllOficinaConcilleria.ClientID %>').value;
                    else
                        migratorio.amfr_sCancilleriaSolicitaAutorizacionId = 0;
                    if (document.getElementById('<%= txtDocAutoriza.ClientID %>') != null)
                        migratorio.amfr_vDocumentoAutoriza = document.getElementById('<%= txtDocAutoriza.ClientID %>').value;
                    else
                        migratorio.amfr_vDocumentoAutoriza = "";
                    migratorio.amfr_bFlagVisaPrensa
                    if (document.getElementById('<%= txtVisaMedioComu.ClientID %>') != null)
                        migratorio.amfr_vMedioComunicacionPrensa = document.getElementById('<%= txtVisaMedioComu.ClientID %>').value;
                    else
                        migratorio.amfr_vMedioComunicacionPrensa = "";
                    if (document.getElementById('<%= cbo_cargo_prensa.ClientID %>') != null)
                        migratorio.amfr_sCargoPrensaId = document.getElementById('<%= cbo_cargo_prensa.ClientID %>').value;
                    else
                        migratorio.amfr_sCargoPrensaId = 0;
                    if (document.getElementById('<%= txtVisaPrensaMotivo.ClientID %>') != null)
                        migratorio.amfr_vMotivoPrensa = document.getElementById('<%= txtVisaPrensaMotivo.ClientID %>').value;
                    else
                        migratorio.amfr_vMotivoPrensa = "";
                    migratorio.amfr_vObservaciones = document.getElementById('<%= txtObservacion.ClientID %>').value;
                    migratorio.amfr_cEstado
                    migratorio.amfr_sTipoNumeroPasaporteId = document.getElementById('<%= ddlTipoDocumento.ClientID %>').value;

                    migratorio.amfr_vNumeroPasaporte = document.getElementById('<%= txtNumDocumento.ClientID %>').value;

                    break;
                case document.getElementById('<%= hDocumento_Salvoconducto.ClientID %>').value:
                    migratorio.acmi_sTipoId = "0";
                    migratorio.acmi_sSubTipoId = "0";
                    migratorio.acmi_sEstadoId = document.getElementById('<%= hSalvoconducto_Estado_Registrado.ClientID %>').value;
                    migratorio.acmi_vNumeroDocumentoAnterior = document.getElementById('<%= txtOtros.ClientID %>').value;
                    migratorio.acmi_sPaisId = document.getElementById('<%= ddlPais.ClientID %>').value;
                    break;
                case document.getElementById('<%= hDocumento_Pasaporte.ClientID %>').value:
                    migratorio.acmi_sTipoId = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;
                    migratorio.acmi_sSubTipoId = "0"
                    migratorio.acmi_sEstadoId = document.getElementById('<%= hVisa_Estado_Registrado.ClientID %>').value;

                    migratorio.amfr_iActoMigratorioFormatoId = document.getElementById('<%= hiActoMigratorioFormatoId.ClientID %>').value;
                    migratorio.amfr_iActoMigratorioId = document.getElementById('<%= hacmi_iActoMigratorioId.ClientID %>').value;

                    migratorio.amfr_sTipoNumeroPasaporteId = document.getElementById('<%= ddlTipoDocumento.ClientID %>').value;
                    migratorio.amfr_vNumeroPasaporte = document.getElementById('<%= txtNumeroPass.ClientID %>').value;

                    var s_Tipo_Pasaporte = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;
                    switch (s_Tipo_Pasaporte) {
                        case document.getElementById('<%= hPasaporte_Expedido.ClientID %>').value:
                            migratorio.acmi_vNumeroDocumentoAnterior = document.getElementById('<%= txt_nroPasaporte_Anterior.ClientID %>').value;
                            break;
                        case document.getElementById('<%= hPasaporte_Revalidado.ClientID %>').value:
                            migratorio.amfr_sPasaporteRevalidarOficinaConsularId = $("#<%= ddl_amfr_sOficinaConsularId.ClientID %>").val();
                            migratorio.amfr_sPasaporteRevalidarOficinaMigracionId = $("#<%= ddl_amfr_sOficinaMigracionId.ClientID %>").val();
                            migratorio.amfr_dPasaporteRevalidarFechaExpedicion = $('#<%= txt_amfr_dFechaExpedicion.FindControl("TxtFecha").ClientID %>').val();
                            migratorio.amfr_dPasaporteRevalidarFechaExpiracion = $('#<%= txt_amfr_dFechaExpiracion.FindControl("TxtFecha").ClientID %>').val();
                            break;
                    }
                    break;
            }

            migratorio.acmi_vNumeroExpediente = document.getElementById('<%= txtNumeroExp.ClientID %>').value;
            if (document.getElementById('<%= txtNumSal_Lam.ClientID %>') != null)
                migratorio.acmi_vNumeroLamina = document.getElementById('<%= txtNumSal_Lam.ClientID %>').value;
            else
                migratorio.acmi_vNumeroLamina = "";
            migratorio.acmi_dFechaExpedicion = document.getElementById('<%= txtFecExpedicion.FindControl("TxtFecha").ClientID %>').value;
            migratorio.acmi_dFechaExpiracion = document.getElementById('<%= txtFecExpiracion.FindControl("TxtFecha").ClientID %>').value;
            migratorio.acmi_vNumeroDocumento = document.getElementById("<%= txtNumeroPass.ClientID %>").value;

            migratorio.acmi_vObservaciones = document.getElementById('<%= txtObservacion.ClientID %>').value;


            var titular = {}

            titular.pers_iPersonaId = document.getElementById('<%= hId_Persona.ClientID %>').value;
            titular.pers_sEstadoCivilId = document.getElementById('<%= ddlEstadoCivil.ClientID %>').value;
            titular.pers_sOcupacionId = document.getElementById('<%= ddlOcupacionAct.ClientID %>').value;
            titular.pers_sGeneroId = document.getElementById('<%= ddlGenero.ClientID %>').value;
            titular.pers_vApellidoPaterno = document.getElementById('<%= txtApePat.ClientID %>').value;
            titular.pers_vApellidoMaterno = document.getElementById('<%= txtApeMat.ClientID %>').value;
            titular.pers_vNombres = document.getElementById('<%= txtNombres.ClientID %>').value;
            titular.pers_dNacimientoFecha = document.getElementById('<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>').value;

            titular.peid_sDocumentoTipoId = document.getElementById('<%= ddlTipoDocumento.ClientID %>').value;
            titular.peid_vDocumentoNumero = document.getElementById('<%= txtNumDocumento.ClientID %>').value;
            titular.pers_cNacimientoLugar = document.getElementById('<%= ddlDomicilioNacDepa.ClientID %>').value +
                    document.getElementById('<%= ddlDomicilioNacProv.ClientID %>').value + document.getElementById('<%= ddlDomicilioNacDist.ClientID %>').value;

            titular.resi_iResidenciaId_peru = document.getElementById('<%= hdn_resi_iResidenciaId_peru.ClientID %>').value;
            titular.resi_cResidenciaUbigeo_peru = document.getElementById('<%= ddlDomicilioPeruDepa.ClientID %>').value +
                    document.getElementById('<%= ddlDomicilioPeruProv.ClientID %>').value + document.getElementById('<%= ddlDomicilioPeruDist.ClientID %>').value;
            titular.resi_vResidenciaDireccion_peru = $("#<%= txtDomicilioPeru.ClientID %>").val();
            titular.resi_vResidenciaTelefono_peru = $("#<%= txtTelefonoPeru.ClientID %>").val();

            titular.resi_iResidenciaId_extranjero = $("#<%= hdn_resi_iResidenciaId_extranjero.ClientID %>").val();
            titular.resi_cResidenciaUbigeo_extranjero = document.getElementById('<%= ddlDomicilioExtrDepa.ClientID %>').value +
                    document.getElementById('<%= ddlDomicilioExtrProv.ClientID %>').value + document.getElementById('<%= ddlDomicilioExtrDist.ClientID %>').value;


            titular.resi_vResidenciaDireccion_extranjero = $("#<%= txtDomicilioExtra.ClientID %>").val();
            titular.resi_vResidenciaTelefono_extranjero = $("#<%= txtTelefonoExtr.ClientID %>").val();

            titular.pers_vEstatura = document.getElementById('<%= txtEstatura.ClientID %>').value;
            titular.pers_sColorOjosId = document.getElementById('<%= ddlColorOjo.ClientID %>').value;
            titular.pers_sColorCabelloId = document.getElementById('<%= ddlColorCabello.ClientID %>').value;

            var padre = {}

            padre.pefi_iPersonaFilacionId = document.getElementById('<%= hId_Padre.ClientID %>').value;
            padre.pefi_sTipoFilacionId = document.getElementById('<%= ddlTitularFiliacionPadre.ClientID %>').value;
            padre.pefi_vNombreFiliacion = document.getElementById('<%= txtPadreNombres.ClientID %>').value;

            var madre = {}

            madre.pefi_iPersonaFilacionId = document.getElementById('<%= hId_Madre.ClientID %>').value;
            madre.pefi_sTipoFilacionId = document.getElementById('<%= ddlTitularFiliacionMadre.ClientID %>').value;
            madre.pefi_vNombreFiliacion = document.getElementById('<%= txtMadreNombres.ClientID %>').value;

            migratorio.Titular = JSON.stringify(titular);
            migratorio.Padre = JSON.stringify(padre);
            migratorio.Madre = JSON.stringify(madre);

            var str_GUID = $("#<%= HFGUID.ClientID %>").val();

            var s_envio = {};
            s_envio.actonomigratorio = JSON.stringify(migratorio);
            s_envio.strGUID = str_GUID;


            if (bol_Imprimir == true) {
                $.ajax({
                    type: "POST",
                    url: "FrmActoMigratorio.aspx/insert_registro",
                    data: JSON.stringify(s_envio),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resultado_impresion,
                    error: errores

                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "FrmActoMigratorio.aspx/insert_registro",
                    data: JSON.stringify(s_envio),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resultado,
                    error: errores

                });
            }



            bol_Estado_Grabar = true;

            if (bol_Imprimir == false) {
                $("#msg-dialog").dialog('close');
            }
        }
        function ValidarRegistroActuacion() {
            var bolValida = true;

            var strTarifa = $.trim($("#<%= txtIdTarifa.ClientID %>").val());
            var strTipoPago = $.trim($("#<%= ddlTipoPago.ClientID %>").val());
            var strCantidad = $.trim($("#<%= txtCantidad.ClientID %>").val());
            var strDescripcion = $.trim($("#<%= txtDescTarifa.ClientID %>").val());

            var txtIdTarifa = document.getElementById('<%= txtIdTarifa.ClientID %>');
            var cmb_TipoPago = document.getElementById('<%= ddlTipoPago.ClientID %>');
            var txtCantidad = document.getElementById('<%= txtCantidad.ClientID %>');
            var txtDescripcion = document.getElementById('<%= txtDescTarifa.ClientID %>');

            if (strTarifa == "") {
                txtIdTarifa.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtIdTarifa.style.border = "1px solid #888888";
            }

            if (strDescripcion == "") {
                txtDescripcion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtDescripcion.style.border = "1px solid #888888";
            }

            if (strTipoPago == "0") {
                cmb_TipoPago.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cmb_TipoPago.style.border = "1px solid #888888";

                if (strTipoPago == document.getElementById('<%= hPago_Lima.ClientID %>').value) {
                    var strNroOperacion = $.trim($("#<%= txtNroOperacion.ClientID %>").val());
                    var strNombreBanco = $.trim($("#<%= ddlNomBanco.ClientID %>").val());
                    var strMontoCancelado = $.trim($("#<%= txtMtoCancelado.ClientID %>").val());
                    var strFecha = $.trim($("#<%= ctrFecPago.ClientID %>").val());

                    var txtNroOperacion = document.getElementById('<%= txtNroOperacion.ClientID %>');
                    var ddlNomBanco = document.getElementById('<%= ddlNomBanco.ClientID %>');
                    var txtMtoCancelado = document.getElementById('<%= txtMtoCancelado.ClientID %>');
                    var txtFecha = document.getElementById('<%= ctrFecPago.ClientID %>');

                    if (strNroOperacion == "") {
                        txtNroOperacion.style.border = "1px solid Red";
                        bolValida = false;
                    }
                    else {
                        txtNroOperacion.style.border = "1px solid #888888";
                    }

                    if (strNombreBanco == "0") {
                        ddlNomBanco.style.border = "1px solid Red";
                        bolValida = false;
                    }
                    else {
                        ddlNomBanco.style.border = "1px solid #888888";
                    }

                    if (strMontoCancelado == "") {
                        txtMtoCancelado.style.border = "1px solid Red";
                        bolValida = false;
                    }
                    else {
                        txtMtoCancelado.style.border = "1px solid #888888";
                    }

                    if (strFecha == "") {
                        txtFecha.style.border = "1px solid Red";
                        bolValida = false;
                    }
                    else {
                        txtFecha.style.border = "1px solid #888888";
                    }
                }
            }

            if (strCantidad == "") {
                txtCantidad.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtCantidad.style.border = "1px solid #888888";
            }

            if (bolValida) {
                $("#<%= lblValidacionRegistro.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacionRegistro.ClientID %>").show();
                bolValida = false;
            }
            return bolValida;
        }

        var iGrabar = 0;
        function Validar_Registro_Pago() {


            if (iGrabar == 1) return true;
            var bolValida = false;

            var vValor = $('#<%= CtrlPago_Visa.FindControl("txtControlValidado").ClientID %>').val();

            if (vValor == "1") {
                bolValida = true;
            }


            if (!bolValida) {

                var btnValidar = $('#<%= CtrlPago_Visa.FindControl("btnValidar").ClientID %>');
                btnValidar.click();
                return false;
            }

            showdialog('c', 'Acto Migratorio', '¿Está seguro de grabar los cambios?', false, 190, 320, function () {
                iGrabar = 1;
                $('#<%=ToolBarButtonContent_Pagos.FindControl("btnGrabar").ClientID %>').click();
                iGrabar = 0;
                $("#msg-dialog").dialog('close');
            });

            return false;
        }

        var bol_Imprimir = false;
        var bol_Estado_Grabar = false;

        function Imprimir_Formatos() {
            bol_Imprimir = true;
            ValidarGrabar();


            bol_Imprimir = false;
            return false;
        }

        function resultado(msg) {

            if (msg.d == null) {

                var hacmi_iActuacionDetalleId = document.getElementById('<%= hacmi_iActuacionDetalleId.ClientID %>').value;
                var iPersonaId = document.getElementById('<%= hId_Persona.ClientID %>').value;
                var iActuacion = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;

                showdialog('i', 'Acto Migratorio', 'Registro almacenado correctamente.', false, 190, 320);

                Pintar_datos(iPersonaId, hacmi_iActuacionDetalleId, iActuacion);

            } else {
                showdialog('e', 'Acto Migratorio', msg.d.toString(), false, 190, 320);
            }

        }

        function resultado_impresion(msg) {

            if (msg.d == null) {

                var hacmi_iActuacionDetalleId = document.getElementById('<%= hacmi_iActuacionDetalleId.ClientID %>').value;
                var iPersonaId = document.getElementById('<%= hId_Persona.ClientID %>').value;
                var iActuacion = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;

                Pintar_datos_impresion(iPersonaId, hacmi_iActuacionDetalleId, iActuacion);

            } else {
                showdialog('e', 'Acto Migratorio', msg.d.toString(), false, 190, 320);
            }

        }
        function errores(msg) {
            showdialog('e', 'Acto Migratorio', 'Error: ' + msg.responseText, false, 190, 320);
        }
        function Pintar_datos(iPersonaId, hacmi_iActuacionDetalleId, iActuacion) {
            $.ajax({
                type: "POST",
                url: "FrmActoMigratorio.aspx/Cargar_Datos_Iniciales",
                data: '{iPersonaId: ' + iPersonaId + ', hacmi_iActuacionDetalleId: ' + hacmi_iActuacionDetalleId + ', iActuacion: ' + iActuacion + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var hacmi_iActoMigratorioId = msg.d[0];
                    var hiActoMigratorioFormatoId = msg.d[1];
                    var hdn_resi_iResidenciaId_peru = msg.d[2];
                    var hdn_resi_iResidenciaId_extranjero = msg.d[3];
                    var hId_Padre = msg.d[4];
                    var hId_Madre = msg.d[5];
                    var acmi_sEstadoId = msg.d[6];
                    var hId_Estado = msg.d[7];

                    if (acmi_sEstadoId == '0') {
                        acmi_sEstadoId = "";
                    }

                    document.getElementById('<%= hId_Madre.ClientID %>').value = hId_Madre;
                    document.getElementById('<%= hId_Padre.ClientID %>').value = hId_Padre;
                    $("#<%= hdn_resi_iResidenciaId_extranjero.ClientID %>").val(hdn_resi_iResidenciaId_extranjero);
                    document.getElementById('<%= hdn_resi_iResidenciaId_peru.ClientID %>').value = hdn_resi_iResidenciaId_peru;

                    document.getElementById('<%= hiActoMigratorioFormatoId.ClientID %>').value = hiActoMigratorioFormatoId;
                    document.getElementById('<%= hacmi_iActoMigratorioId.ClientID %>').value = hacmi_iActoMigratorioId;

                    document.getElementById('<%= lblEstado.ClientID %>').value = acmi_sEstadoId;
                    document.getElementById('<%= hId_Estado.ClientID %>').value = hId_Estado;

                    document.getElementById('<%= lblEstado.ClientID %>').html = "REGISTRADO";

                    document.getElementById('<%= btn_actualza_estado.ClientID %>').click();

                    Habilitar_Tab(2);

                    var s_Tarifa = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;
                    var s_Valiar = false;
                    switch (s_Tarifa) {
                        case document.getElementById('<%= hDocumento_Salvoconducto.ClientID %>').value:
                            break;
                        case document.getElementById('<%= hDocumento_Visa.ClientID %>').value:
                            var control = document.getElementById('<%= txtNumeroPass.ClientID %>').value;
                            if (control == '') {

                            } else {

                                var s_TipoPago = $('#<%= CtrlPago_Visa.FindControl("ddlTipoPago").ClientID %>').val();

                                if (s_TipoPago == '0') {
                                    Habilitar_Tab(3);
                                } else {
                                    Habilitar_Tab(4);
                                }


                                document.getElementById('<%= txtCodLamina.ClientID %>').value = document.getElementById('<%= txtNumSal_Lam.ClientID %>').value;

                            }
                            break;
                        case document.getElementById('<%= hDocumento_Pasaporte.ClientID %>').value:
                            var s_Tipo_Pasaporte = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;
                            switch (s_Tipo_Pasaporte) {
                                case document.getElementById('<%= hPasaporte_Expedido.ClientID %>').value:
                                    break;
                                case document.getElementById('<%= hPasaporte_Revalidado.ClientID %>').value:
                                    break;
                            }
                            break;
                    }

                },
                error: function (err) {
                    showdialog('e', 'Acto Migratorio', 'Error: ' + err.responseText, false, 190, 320);
                }
            });
        }

        function Pintar_datos_impresion(iPersonaId, hacmi_iActuacionDetalleId, iActuacion) {
            $.ajax({
                type: "POST",
                url: "FrmActoMigratorio.aspx/Cargar_Datos_Iniciales",
                data: '{iPersonaId: ' + iPersonaId + ', hacmi_iActuacionDetalleId: ' + hacmi_iActuacionDetalleId + ', iActuacion: ' + iActuacion + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var hacmi_iActoMigratorioId = msg.d[0];
                    var hiActoMigratorioFormatoId = msg.d[1];
                    var hdn_resi_iResidenciaId_peru = msg.d[2];
                    var hdn_resi_iResidenciaId_extranjero = msg.d[3];
                    var hId_Padre = msg.d[4];
                    var hId_Madre = msg.d[5];
                    var acmi_sEstadoId = msg.d[6];
                    var hId_Estado = msg.d[7];

                    if (acmi_sEstadoId == '0') {
                        acmi_sEstadoId = "";
                    }

                    document.getElementById('<%= hId_Madre.ClientID %>').value = hId_Madre;
                    document.getElementById('<%= hId_Padre.ClientID %>').value = hId_Padre;
                    $("#<%= hdn_resi_iResidenciaId_extranjero.ClientID %>").val(hdn_resi_iResidenciaId_extranjero);
                    document.getElementById('<%= hdn_resi_iResidenciaId_peru.ClientID %>').value = hdn_resi_iResidenciaId_peru;

                    document.getElementById('<%= hiActoMigratorioFormatoId.ClientID %>').value = hiActoMigratorioFormatoId;
                    document.getElementById('<%= hacmi_iActoMigratorioId.ClientID %>').value = hacmi_iActoMigratorioId;

                    document.getElementById('<%= lblEstado.ClientID %>').value = acmi_sEstadoId;
                    document.getElementById('<%= hId_Estado.ClientID %>').value = hId_Estado;

                    document.getElementById('<%= lblEstado.ClientID %>').html = "REGISTRADO";

                    var hId_TipoActuacion = document.getElementById('<%= hId_TipoActuacion.ClientID %>').value;
                    var ddlVisaSubTipo = '0';
                    var hdn_acmi_sTipoId = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;

                    switch (hId_TipoActuacion) {
                        case document.getElementById('<%= hDocumento_Visa.ClientID %>').value:
                            ddlVisaSubTipo = document.getElementById('<%= ddlVisaSubTipo.ClientID %>').value;
                            break;
                    }


                    $.ajax({
                        type: "POST",
                        url: "FrmActoMigratorio.aspx/Habilitar_Formato_DGC",
                        data: '{hId_TipoActuacion: ' + JSON.stringify(hId_TipoActuacion) + ', ddlVisaSubTipo: ' + JSON.stringify(ddlVisaSubTipo) + ', hdn_acmi_sTipoId: ' + JSON.stringify(hdn_acmi_sTipoId) + ', hacmi_iActoMigratorioId: ' + JSON.stringify(hacmi_iActoMigratorioId) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (msg.d != null) {
                                var strUrl = "../Registro/frmReporteMigratorio.aspx?vClass=1" + msg.d;
                                window.open(strUrl, 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');
                            }
                        },
                        error: errores

                    });



                },
                error: function (err) {
                    showdialog('e', 'Acto Migratorio', 'Error: ' + err.responseText, false, 190, 320);
                }
            });
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
            $('#tabs').enableTab(1);
            $('#tabs').disableTab(3);
            $('#tabs').disableTab(4);


            $('#tabs').click('tabsselect', function (event, ui) {
                var selectedTab = $("#tabs").tabs('option', 'active');
                document.getElementById('<%= hTab_Index.ClientID %>').value = selectedTab;

            });

        });

        function Desabilitar_Controles() {
            $("#<%=txtRREEDoc.ClientID %>").val('0');
            $("#<%=txtRREENum.ClientID %>").val('');
            $("#<%=txtDIGEMINDoc.ClientID %>").val('0');
            $("#<%=txtDIGEMINNum.ClientID %>").val('');

            $('#<%=txtRREEFecha.FindControl("txtFecha").ClientID %>').val('');
            $('#<%=txtDIGEMINFecha.FindControl("txtFecha").ClientID %>').val('');

            $("#<%=txtRREEDoc.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtRREENum.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtDIGEMINDoc.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtDIGEMINNum.ClientID %>").prop("disabled", "disabled");

            $('#<%=txtRREEFecha.FindControl("txtFecha").ClientID %>').prop("disabled", "disabled");
            $('#<%=txtDIGEMINFecha.FindControl("txtFecha").ClientID %>').prop("disabled", "disabled");

            $('#<%=txtRREEFecha.FindControl("btnFecha").ClientID %>').prop("disabled", "disabled");
            $('#<%=txtDIGEMINFecha.FindControl("btnFecha").ClientID %>').prop("disabled", "disabled");
        }
        function Habilitar_Tab(i_Index) {
            $('#tabs').enableTab(i_Index);
        }
        function Desabilitar_Tab(i_Index) {
            $('#tabs').disableTab(i_Index);
        }
        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function txtcontrolError(ctrl) {
            var x = ctrl.value;
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function ddlcontrolError(ctrl) {
            var x = ctrl.selectedIndex;
            var bolValida = true;
            if (x < 1) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }


        function ValidarControlesComun() {
            var bolValida = true;


            if (txtcontrolError(document.getElementById('<%=txtFecExpedicion.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;

            if (document.getElementById('<%=txtFecExpedicion.FindControl("TxtFecha").ClientID %>').value != '') {
                bolValida = is_Date(document.getElementById('<%=txtFecExpedicion.FindControl("TxtFecha").ClientID %>'));
                if (!bolValida) {
                    document.getElementById('<%=txtFecExpedicion.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                }
            }

            if (txtcontrolError(document.getElementById('<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;


            if (txtcontrolError(document.getElementById("<%=txtNumDocumento.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlGenero.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlEstadoCivil.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtApePat.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtNombres.ClientID %>")) == false) bolValida = false;




            var s_Existe_1 = document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>").value;
            if (s_Existe_1 != '0') {
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

                $("#<%=lblCO_ddlDomicilioPeruProv.ClientID %>").show();
                $("#<%=lblCO_ddlDomicilioPeruDist.ClientID %>").show();
            }
            else {
                $("#<%=lblCO_ddlDomicilioPeruProv.ClientID %>").hide();
                $("#<%=lblCO_ddlDomicilioPeruDist.ClientID %>").hide();
            }
            var _Extranjero = document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>").value;
            if (_Extranjero != '0') {

                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;

                $("#<%=lblCO_ddlDomicilioExtrProv.ClientID %>").show();
                $("#<%=lblCO_ddlDomicilioExtrDist.ClientID %>").show();

            } else {
                $("#<%=lblCO_ddlDomicilioExtrProv.ClientID %>").hide();
                $("#<%=lblCO_ddlDomicilioExtrDist.ClientID %>").hide();
            }

            var strOcupacion = $('#<%=ddlOcupacionAct.ClientID %>').val();
            var strFecNacimiento = $('#<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>').val();
            if (strFecNacimiento.length > 0) {
                var validar_edad = calcular_edad(strFecNacimiento);
                if (validar_edad) {
                    $("#<%= lblValidacion.ClientID %>").html('');
                    $("#<%= lblValidacion.ClientID %>").hide();
                    $('#<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
                    $("#<%=ddlOcupacionAct.ClientID %>").css("border", "solid #888888 1px");
                }
                else {
                    var anio = '<%=ConfigurationManager.AppSettings["edadmaximo"].ToString() %>';
                    if (strOcupacion != '0') {
                        $("#<%=lblValidacion.ClientID %>").html('Fecha de Nacimiento Ingresado es menor que la edad Mínima que es de ' + anio + " años. No debe ingresar datos de ocupación.");
                        $("#<%=lblValidacion.ClientID %>").show();
                        $('#<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>').css("border", "solid Red 1px");
                        $("#<%=ddlOcupacionAct.ClientID %>").css("border", "solid Red 1px");
                        bolValida = false;
                    } else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                        $("#<%= lblValidacion.ClientID %>").hide();
                        $('#<%=txtFecNacimiento.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
                        $("#<%=ddlOcupacionAct.ClientID %>").css("border", "solid #888888 1px");
                    }
                }
            }
            if (ddlcontrolError(document.getElementById("<%=ddlTipoDocumento.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtNumeroExp.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioNacDepa.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioNacProv.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioNacDist.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById('<%=ddlFuncionario.ClientID %>')) == false) bolValida = false;

            return bolValida;
        }

        function calcular_edad(fecha) {
            var fechaActual = new Date()
            var diaActual = fechaActual.getDate();
            var mmActual = fechaActual.getMonth() + 1;
            var yyyyActual = fechaActual.getFullYear();

            FechaNac = fecha.split("-");
            var diaCumple = FechaNac[0];
            var mmCumple = FechaNac[1];
            var yyyyCumple = FechaNac[2];


            if (mmCumple.substr(0, 1) == 0) {
                mmCumple = mmCumple.substring(1, 2);
            }

            if (diaCumple.substr(0, 1) == 0) {
                diaCumple = diaCumple.substring(1, 2);
            }
            var edad = yyyyActual - yyyyCumple;

            var key = '<%=ConfigurationManager.AppSettings["edadmaximo"].ToString() %>';

            if (key < edad) {
                return true;
            }
            else {
                return false;
            }
        }

        function Validar_RenovPasaporte() {
            var bolValida = true;

            bolValida = ValidarControlesComun();

            if (txtcontrolError(document.getElementById("<%=txtNumeroExp.ClientID %>")) == false) bolValida = false;


            if (txtcontrolError(document.getElementById('<%=txt_amfr_dFechaExpedicion.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%=txt_amfr_dFechaExpiracion.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddlColorOjo.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlColorCabello.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtEstatura.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtPadreNombres.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtMadreNombres.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddl_amfr_sOficinaConsularId.ClientID %>")) == false) {
                if (ddlcontrolError(document.getElementById("<%=ddl_amfr_sOficinaMigracionId.ClientID %>")) == false) {
                    bolValida = false;
                }
            } else {
                document.getElementById("<%=ddl_amfr_sOficinaMigracionId.ClientID %>").style.border = "1px solid #888888";
            }

            if (bolValida) {
                if (bol_Imprimir == false) {

                }
            }

            return bolValida;

        }

        function Validar_ExpePasaporte() {
            var bolValida = true;

            bolValida = ValidarControlesComun();

            if (ddlcontrolError(document.getElementById("<%=ddlColorOjo.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlColorCabello.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtEstatura.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtPadreNombres.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtMadreNombres.ClientID %>")) == false) bolValida = false;


            var resi_vResidenciaDireccion_peru = $("#<%=ddlDomicilioPeruDepa.ClientID %>").val();
            var resi_vResidenciaDireccion_extranjero = $("#<%=ddlDomicilioExtrDepa.ClientID %>").val();


            if (resi_vResidenciaDireccion_peru == '0' && resi_vResidenciaDireccion_extranjero == '0') {

                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;


                if (txtcontrolError(document.getElementById("<%=txtDomicilioPeru.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtDomicilioExtra.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoPeru.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtTelefonoExtr.ClientID %>")) == false) bolValida = false;

            } else {
                document.getElementById("<%=txtDomicilioPeru.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=txtDomicilioExtra.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=txtTelefonoPeru.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=txtTelefonoExtr.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>").style.border = "1px solid #888888";
            }


            if (resi_vResidenciaDireccion_peru != '0') {
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtDomicilioPeru.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoPeru.ClientID %>")) == false) bolValida = false;
            }

            if (resi_vResidenciaDireccion_extranjero != '0') {
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtDomicilioExtra.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoExtr.ClientID %>")) == false) bolValida = false;

            }



            if (bolValida) {

                if (bol_Imprimir == false) {
                }
            }

            return bolValida;

        }

        function Validar_Salvoconducto() {
            var bolValida = true;

            bolValida = ValidarControlesComun();

            if (ddlcontrolError(document.getElementById("<%=ddlPais.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtNumeroPass.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddlColorOjo.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlColorCabello.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtEstatura.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtPadreNombres.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtMadreNombres.ClientID %>")) == false) bolValida = false;

            var resi_vResidenciaDireccion_peru = $("#<%=ddlDomicilioPeruDepa.ClientID %>").val();
            var resi_vResidenciaDireccion_extranjero = $("#<%=ddlDomicilioExtrDepa.ClientID %>").val();


            if (resi_vResidenciaDireccion_peru == '0' && resi_vResidenciaDireccion_extranjero == '0') {

                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;


                if (txtcontrolError(document.getElementById("<%=txtDomicilioPeru.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtDomicilioExtra.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoPeru.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtTelefonoExtr.ClientID %>")) == false) bolValida = false;

            } else {
                document.getElementById("<%=txtDomicilioPeru.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=txtDomicilioExtra.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=txtTelefonoPeru.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=txtTelefonoExtr.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>").style.border = "1px solid #888888";

                document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>").style.border = "1px solid #888888";
                document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>").style.border = "1px solid #888888";
            }


            if (resi_vResidenciaDireccion_peru != '0') {
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtDomicilioPeru.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoPeru.ClientID %>")) == false) bolValida = false;
            }

            if (resi_vResidenciaDireccion_extranjero != '0') {
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
                if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtDomicilioExtra.ClientID %>")) == false) bolValida = false;

                if (txtcontrolError(document.getElementById("<%=txtTelefonoExtr.ClientID %>")) == false) bolValida = false;

            }

            if (bolValida) {

                if (bol_Imprimir == false) {
                }
            }
            return bolValida;
        }



        function Validarformato_visa() {
            var bolValida = true;

            if (document.getElementById('<%=txtFecExpiracion.FindControl("TxtFecha").ClientID %>').value != '') {
                bolValida = is_Date(document.getElementById('<%=txtFecExpiracion.FindControl("TxtFecha").ClientID %>'));
                if (!bolValida) {
                    document.getElementById('<%=txtFecExpiracion.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                }
            }

            var s_Tipo = document.getElementById('<%= ddlAutorizado.ClientID %>').value;
            if (s_Tipo == 9221) {

                if (document.getElementById('<%=txtRREEFecha.FindControl("TxtFecha").ClientID %>').value != '') {
                    bolValida = is_Date(document.getElementById('<%=txtRREEFecha.FindControl("TxtFecha").ClientID %>'));
                    if (!bolValida) {
                        document.getElementById('<%=txtRREEFecha.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                    }
                }


                if (document.getElementById('<%=txtDIGEMINFecha.FindControl("TxtFecha").ClientID %>').value != '') {
                    bolValida = is_Date(document.getElementById('<%=txtDIGEMINFecha.FindControl("TxtFecha").ClientID %>'));
                    if (!bolValida) {
                        document.getElementById('<%=txtDIGEMINFecha.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                    }
                }
            }

            if (ValidarControlesComun() == false) {
                bolValida = false;
            }



            if (ddlcontrolError(document.getElementById("<%=ddlVisaTipo.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlVisaSubTipo.ClientID %>")) == false) bolValida = false;
            if (document.getElementById("<%=ddlVisaSubTipo.ClientID %>").value != document.getElementById('<%= hVisaTipo_Temporal.ClientID %>').value) {
                if (ddlcontrolError(document.getElementById("<%=ddlTitularFamiliar.ClientID %>")) == false) bolValida = false;
            }
            if (txtcontrolError(document.getElementById("<%=txtTiempoPermanencia.ClientID %>")) == false) bolValida = false;



            if (txtcontrolError(document.getElementById('<%=txtFecExpiracion.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;


            if (ddlcontrolError(document.getElementById("<%=ddlAutorizado.ClientID %>")) == false) bolValida = false;


            var estado = document.getElementById("<%=hId_Estado.ClientID %>").value;


            if (estado == 51 || estado == 48 || estado == 88) {
                if (txtcontrolError(document.getElementById("<%=txtNumeroPass.ClientID %>")) == false) bolValida = false;
            }


            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDepa.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruProv.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioPeruDist.ClientID %>")) == false) bolValida = false;

            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDepa.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrProv.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlDomicilioExtrDist.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtDomicilioPeru.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtDomicilioExtra.ClientID %>")) == false) bolValida = false;

            if (txtcontrolError(document.getElementById("<%=txtTelefonoPeru.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtTelefonoExtr.ClientID %>")) == false) bolValida = false;



            if (bolValida) {

                if (bol_Imprimir == false) {
                }
            }

            return bolValida;
        }
    </script>
    <script type="text/javascript">
        function is_Date(crl) {
            if (crl.value == '' || crl.value == 'MMM-dd-yyyy') {
                return;
            }
            var fecha = get_Date(crl.value);


            var dtCh = "/";
            var minYear = 1900;
            var maxYear = 2100;
            function isInteger(s) {
                var i;
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (((c < "0") || (c > "9"))) return false;
                }
                return true;
            }
            function stripCharsInBag(s, bag) {
                var i;
                var returnString = "";
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (bag.indexOf(c) == -1) returnString += c;
                }
                return returnString;
            }
            function daysInFebruary(year) {
                return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
            }
            function DaysArray(n) {
                for (var i = 1; i <= n; i++) {
                    this[i] = 31
                    if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                    if (i == 2) { this[i] = 29 }
                }
                return this
            }
            function isDate(dtStr) {
                var daysInMonth = DaysArray(12)
                var pos1 = dtStr.indexOf(dtCh)
                var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
                var strDay = dtStr.substring(0, pos1)
                var strMonth = dtStr.substring(pos1 + 1, pos2)
                var strYear = dtStr.substring(pos2 + 1)
                strYr = strYear
                if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
                if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
                for (var i = 1; i <= 3; i++) {
                    if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
                }
                month = parseInt(strMonth)
                day = parseInt(strDay)
                year = parseInt(strYr)
                if (pos1 == -1 || pos2 == -1) {
                    return false
                }
                if (strMonth.length < 1 || month < 1 || month > 12) {
                    return false
                }
                if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                    return false
                }
                if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                    return false
                }
                if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                    return false
                }
                return true
            }
            if (!isDate(fecha)) {
                return false;
            } else {
                return true;
            }
        }

        function get_Date(strfecha) {
            var s_NameMes = strfecha.substring(0, 3);
            var s_dia = strfecha.substring(4, 6);
            var s_year = strfecha.substring(7, 11);

            return s_dia + '/' + Search_Number_Mes(s_NameMes) + '/' + s_year;
        }

        function Search_Number_Mes(strNameMes) {
            if (strNameMes.toLowerCase() == 'ene') {
                return '01';
            }
            if (strNameMes.toLowerCase() == 'feb') {
                return '02';
            }
            if (strNameMes.toLowerCase() == 'mar') {
                return '03';
            }
            if (strNameMes.toLowerCase() == 'abr') {
                return '04';
            }
            if (strNameMes.toLowerCase() == 'may') {
                return '05';
            }
            if (strNameMes.toLowerCase() == 'jun') {
                return '06';
            }
            if (strNameMes.toLowerCase() == 'jul') {
                return '07';
            }
            if (strNameMes.toLowerCase() == 'ago') {
                return '08';
            }
            if (strNameMes.toLowerCase() == 'sep') {
                return '09';
            }
            if (strNameMes.toLowerCase() == 'oct') {
                return '10';
            }
            if (strNameMes.toLowerCase() == 'nov') {
                return '11';
            }
            if (strNameMes.toLowerCase() == 'dic') {
                return '12';
            }
            return '__';
        }

        function Button1_onclick() {
            showdialog('c', 'Consulta Migratorio', 'No se econtraron registros para aprobar', false, 190, 320, function () {
                alert("Hola");
                $("#msg-dialog").dialog('close');
            });
        }
        
    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</asp:Content>
