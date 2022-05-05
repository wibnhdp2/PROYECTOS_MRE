<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmActoMilitar.aspx.cs" maintainScrollPositionOnPostBack="true" Inherits="SGAC.WebApp.Registro.FrmActuacionMilitar" %>
     
<%@ Register Src="~/Accesorios/SharedControls/ctrlAdjunto.ascx" TagName="ctrlAdjunto" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlFecha.ascx" TagName="ctrlFecha" TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton" TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ctrlTool" TagName="ctrlTool" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc2" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<%@ Register src="../Accesorios/SharedControls/ctrlGridParticipante.ascx" tagname="ctrlGridParticipante" tagprefix="uc2" %>
<%@ Register src="../Accesorios/SharedControls/ctrlUbigeo.ascx" tagname="ctrlUbigeo" tagprefix="uc4" %>
<%@ Register src="../Accesorios/SharedControls/ctrlUbigeoLineal.ascx" tagname="ctrlUbigeoLineal" tagprefix="uc5" %>

  

<%@ Register src="../Accesorios/SharedControls/ctrlReimprimirbtn.ascx" tagname="ctrlReimprimirbtn" tagprefix="uc6" %>
<%@ Register src="../Accesorios/SharedControls/ctrlBajaAutoadhesivo.ascx" tagname="ctrlBajaAutoadhesivo" tagprefix="uc7" %>

  

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.1.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/signalr/hubs") %>" type="text/javascript"></script>  
    <style type="text/css">
        .style2
        {
            width: 376px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <%--<asp:Label ID="Label38" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>--%>
 <asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM2" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuación Consular"></asp:Label></h2>
            </td>
            <td style="text-align: right">
                    <asp:LinkButton ID="Tramite" runat="server" 
                        PostBackUrl="~/Registro/FrmTramite.aspx" Font-Bold="True" Font-Size="10pt" ForeColor="Blue" Font-Underline="true">
                        Regresar a Trámites</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTarifa" CssClass="titulo_tarifa" runat="server" Text="79a - Por la inscripción en el Registro Militar dentro del plazo legal."></asp:Label>
            </td>
        </tr>  
    </table>
    <table style="width: 95%;" align="center" class="mTblSecundaria" bgcolor="#4E102E">
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDestino" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                                Font-Size="10pt" Font-Underline="false" ForeColor="White" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="95%" align="center">
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
                                            <ctrlTool:ctrlTool ID="ctrlToolBarRegistro" runat="server"></ctrlTool:ctrlTool>                                            
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                        </td> 
                                        <td colspan="3">
                                            <asp:HiddenField ID="hidEventNuevo" runat="server" />
                                            <asp:HiddenField ID="hidDocumentoSoloNumero" Value="0" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td> 
                                        <td colspan="3">
                                            <asp:Label ID="lblValidacionRegistro" runat="server" Text="Falta validar algunos campos."
                                                    CssClass="hideControl" ForeColor="Red"></asp:Label>
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
                                                    <td colspan="2">
                                                        <asp:Label ID="lblLeyendaTarifa" runat="server" Text=" (Ingresar código actuación y Presionar tecla ENTER)" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtIdTarifa" runat="server" Width="38px" CssClass="campoNumero" 
                                                                        MaxLength="5"
                                                                        ToolTip="Coloque el número de la seccion del tarifario" />
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
                                                                                ToolTip="Buscar" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:ListBox ID="LstTarifario" runat="server" Width="659px" AutoPostBack="True" BackColor="#EAFFFF" />
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
                            <td>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblTipPago" runat="server" Text="Tipo de Pago:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoPago" runat="server" Height="20px" Width="200px" AutoPostBack="True"
                                    Enabled="True" />
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
                                <asp:TextBox ID="txtCantidad" runat="server" Width="100px" AutoPostBack="True" 
                                    Enabled="True" CssClass="campoNumero" MaxLength="2" />
                                &nbsp;
                                <asp:Label ID="lblCO_txtCantidad" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                <asp:Label ID="lblLeyenda" runat="server" Text=" (Presionar tecla ENTER para calcular total)" Font-Bold="true"></asp:Label>
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
                                        <asp:TextBox ID="txtNroOperacion" runat="server" Width="200px" MaxLength="50" />
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
                                        <asp:DropDownList ID="ddlNomBanco" runat="server" Width="300px" MaxLength="10"></asp:DropDownList>
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
                                <asp:TextBox ID="txtTotalSC" runat="server" Width="150px" Enabled="False" CssClass="campoNumero"/>
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
                                    <tr> 
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div id="tab-1">
                        <asp:UpdatePanel ID="updToolFormato" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnImpresionConstanciaVP" runat="server" CssClass="btnPrint" 
                                        onclick="btnImpresionConstanciaVP_Click" TabIndex="50" 
                                        Text="     Vista Previa Constancia Inscripción" Width="250px" />
                                    <asp:Button ID="btnImpresionRegistroVP" runat="server" CssClass="btnPrint" 
                                        onclick="btnImpresionRegistroVP_Click" TabIndex="50" 
                                        Text="     Vista Previa Hoja de Registro" Width="250px" />
                                    <ToolBarButton:ToolBarButtonContent ID="ctrlToolbarFormato" runat="server"></ToolBarButton:ToolBarButtonContent>
                                    <asp:HiddenField ID="hifRegistroMilitar" runat="server" Value="0" />
                                    <asp:HiddenField ID="hi_iPersonaId" runat="server" Value="0" />
                                </td>
                                <td align="right">
                                    &nbsp;</td>
                            </tr>
                        </table>
                        <div>
                            <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                <tr>
                                    <td width="130px">
                                        <asp:Label ID="lblClase" runat="server" Text="Clase: " ></asp:Label>
                                    </td>
                                    <td width="170px">
                                        <asp:TextBox ID="txtClase" runat="server" Width="146px" 
                                        MaxLength="50" TabIndex="1" CssClass="txtLetra"></asp:TextBox>
                                        <asp:Label ID="lblCO_ContDepOffiReg6" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                    <td width="100px">
                                        <asp:Label ID="lblLibro" runat="server" Text="Libro: "></asp:Label>
                                    </td>
                                    <td width="170px">
                                        <asp:TextBox ID="txtLibro" runat="server" Width="146px" 
                                        MaxLength="50" TabIndex="2" CssClass="txtLetra"></asp:TextBox>
                                        <asp:Label ID="lblCO_ContDepOffiReg7" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                    <td width="130px">
                                        <asp:Label ID="Label7" runat="server" Text="Folio: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFolio" runat="server" Width="156px" onkeypress="return isNumberKey(event)"
                                        MaxLength="3" TabIndex="3" CssClass="txtLetra"></asp:TextBox>
                                        <asp:Label ID="lblCO_ContDepOffiReg8" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Calificación: "></asp:Label>
                                    </td> 
                                    <td>
                                        <asp:DropDownList ID="ddlCalificacion" runat="server" Width="150px" 
                                            Height="21px" Enabled="false" AutoPostBack="True" TabIndex="4" />
                                        <asp:Label ID="lblCalificacion_val" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="Institución: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInstitucion" runat="server" Width="150px" 
                                            Height="21px" Enabled="false" TabIndex="5" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label35" runat="server" Text="Servicio en la Reserva: " Width="140px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlServicioReserva" runat="server" Width="160px" 
                                            Height="21px" Enabled="false" TabIndex="6" />
                                        <asp:Label ID="lblCO_ContDepOffiReg11" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table id="TitDatosTitular" runat="server" style="border-bottom: 1px solid #800000;
                                width: 100%; border-bottom-color: #800000;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDatosTitular" Text="FILIACIÓN DEL INSCRITO" runat="server" Style="font-weight: 700; color: #800000;" />
                                </td>
                            </tr>
                        </table>
                        <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                            <tr>
                                <td width="130px">
                                    <asp:Label ID="lblFechaTitular" runat="server" Text="Fecha Nac.: "></asp:Label>
                                </td>
                                <td width="170px">
                                    <SGAC_Fecha:ctrlDate ID="txtFecNac" runat="server" />
                                    <%--<asp:TextBox ID="txtFecNac" runat="server" Width="90px" Enabled="False" 
                                        TabIndex="7" />
                                    <cc1:CalendarExtender ID="calExtFecNac" runat="server" Format="MMM-dd-yyyy" PopupButtonID="imgCal2"
                                        TargetControlID="txtFecNac" />
                                    <asp:ImageButton ID="imgCal2" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                        ImageAlign="AbsMiddle" />--%>
                                    <asp:Label ID="Label47" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHoraTitular" runat="server" Text="Hora Nac.: "></asp:Label>
                                </td>
                                <td width="170px">
                                    <asp:TextBox ID="txtHora" runat="server" Width="40px" onkeypress="return isHoraKey(this,event)"
                                                    onBlur="validaHora(this)" MaxLength="5" TabIndex="8"></asp:TextBox>
                                    <asp:Label ID="Label46" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                    <asp:Label ID="LblFechaValija1" runat="server" Font-Size="10pt" 
                                                    style="font-weight: 700; color: #990033; font-size: x-small;" 
                                                    Text="(HH:mm)" Width="114px"></asp:Label>
                                </td>
                                <td width="130px">
                                    <asp:Label ID="Label12" runat="server" Text="Género:" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGenero" runat="server" Width="160px" Height="21px" Enabled="false"
                                        TabIndex="9" />
                                    <asp:Label ID="Label44" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                            </tr> 
                            <tr>
                                <td>
                                    <asp:Label ID="Label69" runat="server" Text="Nombres" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombresTitular" runat="server" Width="146px" onkeypress="return isNombreApellido(event)"
                                        MaxLength="100" Enabled="false" CssClass="txtLetra" TabIndex="10" />
                                </td>
                                <td>
                                    <asp:Label ID="Label67" runat="server" Text="Primer Apellido" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApePatTitular" runat="server" Width="146px" onkeypress="return isNombreApellido(event)"
                                        MaxLength="100"  Enabled="false" CssClass="txtLetra" TabIndex="11"/>
                                </td>
                                <td width="120px">
                                    <asp:Label ID="Label68" runat="server" Text="Segundo Apellido" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApeMatTitular" runat="server" Width="156px" onkeypress="return isNombreApellido(event)"
                                        MaxLength="100"  Enabled="false" CssClass="txtLetra" TabIndex="12"/>
                                </td>
                            </tr>
                            <%--DETALLE DE PARTICIPANTES--%>
                            <tr id="FilaDatAdicRerMil1" runat="server">
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Estado Civil" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEstadoCivil" runat="server" Width="150px" 
                                        Height="21px" TabIndex="13" />
                                    <asp:Label ID="Label40" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label33" runat="server" Text="Nro. Hijos: " />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroHijos" runat="server" Width="93px" CssClass="campoNumero"
                                        onkeypress="return isNumberKey(event)" onBlur="validanumeroLostFocus(this)"
                                        MaxLength="2" TabIndex="14" />
                                    <asp:Label ID="Label39" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Tipo de Inscripción: " />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInscripcion" runat="server" Width="160px" 
                                        Height="21px" TabIndex="15" />
                                    <%--<asp:TextBox ID="txtTipInscripcion" runat="server" Width="156px" onBlur="conMayusculas(this)"/>--%>
                                    <%--<asp:Label ID="Label38" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr id="FilaDatAdicRerMil2" runat="server">
                                <td>
                                    <asp:Label ID="lblEstatura" runat="server" Text="Estatura:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEstatura" runat="server" Width="80px" onkeypress="return isNumberKey(event)"
                                        onBlur="validanumeroLostFocus(this)" MaxLength="4" 
                                        CssClass="campoNumero" TabIndex="16" />
                                    <asp:Label ID="Label6" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                    <asp:Label ID="lblEstaturaMed" runat="server" Text="cm." Width="30px" 
                                        style="font-weight: 700; color: #800000; font-size: x-small"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblPeso" runat="server" Text="Peso:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPeso" runat="server" Width="80px" CssClass="campoNumero" onkeypress="return isNumberKey(event)"
                                        onBlur="validanumeroLostFocus(this)" MaxLength="5" TabIndex="17" />
                                    <asp:Label ID="Label24" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                    <asp:Label ID="lblMedida" runat="server" Text="Kg." 
                                        style="font-weight: 700; color: #800000; font-size: x-small" Width="30px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblColorTez" runat="server" Text="Color de tez:" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlColorTez" runat="server" Width="160px" Height="21px" 
                                        TabIndex="18" />
                                    <asp:Label ID="Label31" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr id="FilaDatAdicRerMil3" runat="server">
                                <td>
                                    <asp:Label ID="lblColorOjos" runat="server" Text="Color/ojos:" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlColorOjos" runat="server" Width="150px" Height="21px" 
                                        TabIndex="19"/>
                                    <asp:Label ID="lblCO_ContDepOffiReg30" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblGrupoSang" runat="server" Text="Grupo/Sang.:" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGrupoSanguineo" runat="server" Width="150px" 
                                        Height="21px" TabIndex="20" />
                                    <asp:Label ID="lblCO_ContDepOffiReg31" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSeniasPart" runat="server" Text="Señas Particulares:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSeniasPart" runat="server" Width="156px" 
                                        onBlur="conMayusculas(this)" CssClass="txtLetra"
                                    MaxLength="250" TabIndex="21"/>
                                    <asp:Label ID="lblCO_ContDepOffiReg32" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDatosOficinaRegistral" runat="server" Text="LUGAR DE NACIMIENTO"
                                        Style="font-weight: 700; color: #800000;" />
                                </td>
                            </tr>
                        </table>
                        <table style="border-bottom: 1px solid #800000; width: 96%; ">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNacLugarTipo" runat="server" Text="Tipo Ocurrencia:" 
                                        Width="115px"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlNacLugarTipo" runat="server" Width="290px" 
                                        Height="21px" TabIndex="22">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label48" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lblNacLugar" runat="server" Text="Lugar:" Width="60px"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNacLugar" runat="server" Width="286px" onkeypress="return NoCaracteresEspeciales(event)"
                                        onBlur="conMayusculas(this)" CssClass="txtLetra" TabIndex="23"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan = "4">
                                    <uc4:ctrlUbigeo ID="ctrlUbigeo_Nacimiento" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <%--lUGAR DE DOMICILIO--%>
                        <%--style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;"--%>
                        <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="DOMICILIO" Width="120px"
                                        Style="font-weight: 700; color: #800000;" />
                                </td>
                                <td>
                                    <asp:HiddenField ID="hifResidenciaId" runat="server" Value="0" />
                                </td>
                                <td>
                                    <asp:HiddenField ID="hifResidenciaCodigoPostal" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>

                        <table width="100%">
                            <tr>
                                <td width="120px">
                                    <asp:Label ID="Label26" runat="server" Text="Dirección:"></asp:Label>
                                </td>
                                <td width="478px">
                                    <asp:TextBox ID="txtDireccion" runat="server" Width="453px" onkeypress="return NoCaracteresEspeciales(event)"
                                        onBlur="conMayusculas(this)" MaxLength="500" CssClass="txtLetra" 
                                        TabIndex="24"></asp:TextBox>
                                    <asp:Label ID="Label11" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                                <td width="90px" align="right">
                                    <asp:Label ID="Label13" runat="server" Text="Teléfono: "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDomicilioTelef" runat="server" Width="120px" CssClass="txtLetra"
                                        onkeypress="return isNumberKey(event)" MaxLength="50" TabIndex="25"></asp:TextBox>
                                    <asp:Label ID="Label56" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <uc5:ctrlUbigeoLineal ID="ctrlUbigeoLineal_Domicilio" runat="server" />
                            <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label23" runat="server" Text="DATOS DE PARTICIPANTE(S)"
                                        Style="font-weight: 700; color: #800000;" />
                                </td>
                            </tr>
                        </table>
                        
                       
                    <div>
<table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
    <tr>
        <td width="170px">
            <asp:Label ID="lblPartTipoPart" runat="server" Text="Tipo Participante:" />
        </td>
        <td width="35%">
            <asp:DropDownList ID="ddl_TipoParticipante" runat="server" Width="250px" 
                Height="21px" TabIndex="26">
            </asp:DropDownList>
            <asp:Label ID="Label60" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
         <td>
            <asp:TextBox ID="txtPersonaId" runat="server" Text="0" CssClass="hideControl"></asp:TextBox>
        </td>
        <%--<td width="15%">
            <asp:Label ID="lblPartTipoVinc" runat="server" Text="Tipo Dato:" />
        </td>--%>
       <%-- <td width="33%">
            <asp:DropDownList ID="ddl_TipoDatoParticipante" runat="server" Width="265px" Style="margin-bottom: 0px"
                Height="21px" TabIndex="27">
            </asp:DropDownList> 
        </td>--%>
    </tr>
   <%-- <tr>
        <td>
            <asp:Label ID="Label36" runat="server" Text="Tipo Vínculo:" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_TipoVinculoParticipante" runat="server" Width="250px" 
                Height="21px" TabIndex="28">
            </asp:DropDownList>
        </td>
       
        <td>
        </td>
    </tr>--%>
    <tr>

        <td>
            <asp:Label ID="Label49" runat="server" Text="Nacionalidad" />
        </td>
        <td>
            <asp:DropDownList ID="ddl_NacParticipante" runat="server" Width="250px" 
                Height="21px" TabIndex="27" AutoPostBack="True">
            </asp:DropDownList>
            <asp:Label ID="Label65" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
        </td>
       

        
        
        
    </tr>
    <caption>
        <tr>
            <td>
                <asp:Label ID="lblPartTipoDoc" runat="server" Text="Tipo Documento:" />
            </td>
            <td>
                <asp:DropDownList ID="ddl_TipoDocParticipante" runat="server" 
                    AutoPostBack="True" Height="21px" 
                    onselectedindexchanged="ddl_TipoDocParticipante_SelectedIndexChanged" 
                    TabIndex="28" Width="250px">
                </asp:DropDownList>
                <asp:Label ID="Label63" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblPartNroDoc" runat="server" Text="Nro. Documento:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNroDocParticipante" runat="server" CssClass="campoNumero" 
                    MaxLength="20" TabIndex="29" Width="250px" />
                <asp:ImageButton ID="imgBuscar" runat="server" 
                    ImageUrl="~/Images/img_16_search.png" OnClick="imgBuscar_Click" 
                    Style="width: 16px" />
                <asp:Label ID="Label64" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label32" runat="server" Text="Primer Apellido:" />
            </td>
            <td>
                <asp:TextBox ID="txtApePatParticipante" runat="server" CssClass="txtLetra" 
                    onBlur="conMayusculas(this)" TabIndex="30" Width="250px" />
                <asp:Label ID="Label70" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label45" runat="server" Text="Segundo Apellido:" />
            </td>
            <td>
                <asp:TextBox ID="txtApeMatParticipante" runat="server" CssClass="txtLetra" 
                    MaxLength="100" onBlur="conMayusculas(this)" TabIndex="31" Width="250px" />
                <asp:Label ID="lblApeMater" runat="server" ForeColor="Red" Text="*" 
                    Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label34" runat="server" Text="Nombres:" />
            </td>
            <td>
                <asp:TextBox ID="txtNomParticipante" runat="server" CssClass="txtLetra" 
                    onBlur="conMayusculas(this)" TabIndex="32" Width="250px" />
                <asp:Label ID="Label66" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label73" runat="server" Text="Dirección:" />
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtDireccionParticipante" runat="server" CssClass="txtLetra" 
                    MaxLength="500" onBlur="conMayusculas(this)" TabIndex="35" Width="430px" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <uc4:ctrlUbigeo ID="ctrlUbigeo1" runat="server" />
            </td>
            <td align="right">
            </td>
        </tr>
        <tr>
            <td align="right" colspan="4" style="width:400px">
                <asp:Button ID="btnAceptar" runat="server" CssClass="btnNewDirFil" 
                    OnClick="btnAceptar_Click" TabIndex="36" Text="    Adicionar" Width="120px" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="btnLimpiar" 
                    OnClick="btnCancelar_Click" Text="    Limpiar" Width="120px" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
    </caption>

    
</table>
<br />
<table width="100%" class="mTblSecundaria" bgcolor="#4E102E">
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="DETALLE DE PARTICIPANTE(S)" Style="font-weight: 700;"
                ForeColor="White" />
        </td>
    </tr>
</table>
<table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
    <tr>
        <td>
            <asp:GridView ID="Grd_Participantes" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                GridLines="None" SelectedRowStyle-CssClass="slt" DataKeyNames="iActuacionParticipanteId,iPersonaId,sTipoParticipanteId,sTipoDatoId,sTipoVinculoId"
                ShowHeaderWhenEmpty="True" OnRowCommand="Grd_Participantes_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="Nro." DataField="iItemRow" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="iActuacionParticipanteId" DataField="iActuacionParticipanteId"
                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Apellidos y Nombres" DataField="vNombreCompleto" />


                    <asp:BoundField DataField="sTipoParticipanteId" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="sTipoParticipanteId" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Tipo Participante" DataField="vTipoParticipante" />
                    <asp:BoundField DataField="sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="sDocumentoTipoId" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>

                    <asp:BoundField DataField="sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="sDocumentoTipoId" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vDocumentoNumero" HeaderStyle-CssClass="ColumnaOculta"
                        HeaderText="vDocumentoNumero" ItemStyle-CssClass="ColumnaOculta">
                        <HeaderStyle CssClass="ColumnaOculta" />
                        <ItemStyle CssClass="ColumnaOculta" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nro. Documento" DataField="vDocumentoCompleto" />
                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEditar2" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                CommandName="Editar" ImageUrl="~/Images/img_grid_modify.png" ToolTip="Editar Notificación" 
                                visible='<%# IsVisible(Eval("vTipoParticipante")) %>'
                            />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAnular4" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                CommandName="Eliminar" ImageUrl="~/Images/img_grid_delete.png" ToolTip="Eliminar Notificación" 
                                visible='<%# IsVisible(Eval("vTipoParticipante")) %>'
                            />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="ttt">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Toipo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
                <SelectedRowStyle CssClass="slt" />
                <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                <EmptyDataTemplate>
                    <table id="tbSinDatos0">
                        <tbody>
                            <tr>
                                <td width="10%">
                                    <asp:Image runat="server" ID="imgWarning0" ImageUrl="~/Images/img_16_warning.png" />
                                </td>
                                <td width="5%">
                                </td>
                                <td width="85%">
                                    <asp:Label ID="lblSinDatos0" runat="server" Text="No se encontraron Datos..."></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <AlternatingRowStyle />
                <PagerStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>
</table>

            <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                <tr>
                        <td>
                          
                        </td>
                    </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="OBSERVACIONES" Width="120px"
                            Style="font-weight: 700; color: #800000;" />
                    </td>
                </tr>
            </table>

            <table style="border-bottom: 1px solid #800000; width: 100%;">
           
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtObservacionesAP" runat="server" Height="49px" Width="100%" TextMode="MultiLine"
                            CssClass="txtLetra" onBlur="conMayusculas(this)"/>
                    </td>
                </tr>
                

                <tr>
                    <td style="text-align: center; font-family:Verdana; font-weight:bold; text-align:center; font-size:16;" colspan="2">
                        <asp:CheckBox ID="chk_verificar" runat="server" 
                            oncheckedchanged="chk_verificar_CheckedChanged" AutoPostBack="True" 
                            Font-Size="12pt" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        &nbsp;</td>
                    <td>
                        <asp:HiddenField ID="hifVistaPrevia" runat="server" />
                    </td>
                </tr>
            </table>
                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="updActuacionAdjuntar" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <uc1:ctrlAdjunto runat="server" ID="ctrlAdjunto" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-3">
                    </div>
                    <div id="tab-4">
                      <asp:UpdatePanel ID="updVinculacion" UpdateMode="Conditional" runat="server">
                        <Triggers>
                                    <asp:PostBackTrigger ControlID="btnImpresionConstancia" />
                                    <asp:PostBackTrigger ControlID="btnImpresionRegistro" />
                        </Triggers>
                            <ContentTemplate>
                        <table>
                            <tr>
                                    <td colspan="4">
                                     
                                       <asp:Button Text="    Grabar" CssClass="btnSave" runat="server"  
                                            ID="btnGrabarVinculacion" onclick="btnGrabarVinculacion_Click" />

                                        <uc6:ctrlReimprimirbtn ID="ctrlReimprimirbtn1" runat="server" />
                                        <uc7:ctrlBajaAutoadhesivo ID="ctrlBajaAutoadhesivo1" runat="server" />

                                    </td>
                                </tr>

                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblValidacionDetalle" runat="server" CssClass="hideControl" 
                                        ForeColor="Red" Text="Validar Código del Autoadhesivo"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 300px" colspan="2">
                                    <h3>
                                        <asp:Label ID="Label17" runat="server" Text="PASO 1: Vinculación Autoadhesivo"></asp:Label></h3>
                                </td>

                                <td style="width: 300px" colspan="2">
                                    <h3>
                                        <asp:Label ID="Label15" runat="server" Text="PASO 2: Vista Previa e Impresión"></asp:Label></h3>
                                </td>
                                <td style="width: 300px" colspan="2">
                                    <h3>
                                        <asp:Label ID="Label16" runat="server" Text="PASO 3: Aprobación Impresión"></asp:Label></h3>
                                </td>
                               
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Text="Código:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCodAutoadhesivo" runat="server" Width="120px" 
                                        MaxLength="20" ontextchanged="txtCodAutoadhesivo_TextChanged" 
                                        Enabled="True"></asp:TextBox>
                                </td>

                                <td colspan="2">
                                    <asp:Button ID="btnVistaPrev" runat="server" Text="   Autoadhesivo" Width="180px"
                                        CssClass="btnPrint" TabIndex="50" onclick="btnVistaPrev_Click" 
                                        Enabled="False" />
                                     <asp:HiddenField  runat="server" ID="hdn_ImpresionCorrecta" Value="0"></asp:HiddenField>
                                     <asp:HiddenField  runat="server" ID="hCodAutoadhesivo"></asp:HiddenField>
                                     
                                     <asp:Button ID="btnDesabilitarAutoahesivo" runat="server"  
                                                    onclick="btnDesabilitarAutoahesivo_Click" Text="Oculto" Visible="True" 
                                                    CssClass="hideControl" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkImpresion" runat="server" Text="Impresión Correcta" 
                                        oncheckedchanged="chkImpresion_CheckedChanged" AutoPostBack="True" 
                                        Enabled="False" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td colspan="2">
                                    <asp:Button ID="btnImpresionConstancia" runat="server" 
                                        Text="   Constancia Inscripción" Width="180px"
                                        CssClass="btnPrint" TabIndex="50" onclick="btn_Print_Constancia_Click" />
                                </td>
                            </tr>
                            <tr>
                            <td colspan="2"></td>
                                <td colspan="2">
                                    <asp:Button ID="btnImpresionRegistro" runat="server" Text="   Hoja de Registro" Width="180px"
                                        CssClass="btnPrint" TabIndex="50" onclick="btn_Print_Registro_Click" />
                                </td>
                            </tr>
                            <tr>
                            <td colspan="2"></td>
                                <td>

                                    <asp:Button ID="btn_confirmar" runat="server" CssClass="btnPrint" 
                                         Text="   Acta de Conformidad" Width="180px" 
                                        onclick="btn_confirmar_Click" />

                                </td>
                            
                            </tr>

                        </table>
                       
                           
                             <table>
                                    <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grd_ActInsDet" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                        AutoGenerateColumns="False" GridLines="None" Width="680px">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="idActuacionInsumoDetalle" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" 
                                                        Text='<%# Bind("aide_iActuacionInsumoDetalleId") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" 
                                                        Text='<%# Bind("aide_iActuacionInsumoDetalleId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="aide_iActuacionDetalleId" HeaderText="idActuacionDetalle"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" 
                                                Visible="False" >
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="insu_iInsumoId" HeaderText="idInsumo"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" 
                                                Visible="False" >
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="aide_dFechaVinculacion" HeaderText="Fecha y Hora" 
                                                DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" >
                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="USUARIO" HeaderText="Usuario" >
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="insu_sInsumoTipoId" HeaderText="Tipo Insumo" >
                                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="aide_bFlagImpresion"  
                                                HeaderText="Impresión Correcta" >
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="insu_vCodigoUnicoFabrica" 
                                                HeaderText="Código de Insumo" >
                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CantidadImpresiones" HeaderText="Cantidad Impresiones">
                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                                        
                                </td>
                            </tr>
                                    <tr>
                                        <td colspan="4" class="style2">
                                            <uc2:ctrlpagebar ID="CtrlPageBarActuacionInsumoDetalle" runat="server" OnClick="ctrlPagActuacionInsumoDetalle_Click"
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
    <script language="javascript" type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
            //$("#<%=txtNroDocParticipante.ClientID %>").numeric();

        });
        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }
        function Load() {

            var isPostBackObject = document.getElementById('isPostBack');

            if (isPostBackObject != null) {
                if (isPostBackObject.value == "0") {

//                    txtNroHijos = $("#<%=txtNroHijos.ClientID %>");
                    txtPeso = $("#<%=txtPeso.ClientID %>");

//                    if (txtNroHijos.val() != '') {
//                        if (txtNroHijos.val()==0) {
//                            txtNroHijos.val('');
//                        }
//                    }

                    if (txtPeso.val() != '') {
                        if (txtPeso.val() == 0) {
                            txtPeso.val('');
                        }
                    }
                    isPostBackObject.value = "1";
                }
                $('#<%= txtCodAutoadhesivo.ClientID %>').focus();
            }


            $("#<%=txtNroDocParticipante.ClientID %>").bind("keypress", function (e) {

                if (e.keyCode == 13) {
                    document.getElementById("<%=imgBuscar.ClientID %>").click();
                    e.preventDefault();
                }
                else {

                    var vSoloNumero = $("#<%=hidDocumentoSoloNumero.ClientID %>").val();

                    if(vSoloNumero=="1")
                        return isNumero(e);
                     else
                        return isLetraNumeroDoc(e);

                
                }

            });



            $("#<%=txtNroDocParticipante.ClientID %>").bind("blur", function (e) {                
                    document.getElementById("<%=imgBuscar.ClientID %>").click();
                    e.preventDefault();              

            });
            }

//            function TipoValidacionDocumento() {

//                var strTipoDocParticipante = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val()).toString();


//                if (strTipoDocParticipante == '3' || strTipoDocParticipante == '4' || strTipoDocParticipante == '5') {
//                    $("#<%=txtNroDocParticipante.ClientID %>").alphanumeric();
//                                       
//                }
//                else {
//                    $("#<%=txtNroDocParticipante.ClientID %>").numeric();                    
//                }
            //            }


            function isNumero(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode

                var letra = false;
                if (charCode > 1 && charCode < 4) {
                    letra = true;
                }
                if (charCode > 47 && charCode < 58) {
                    letra = true;
                }

                var letras = "áéíóúñ";
                var tecla = String.fromCharCode(charCode);
                var n = letras.indexOf(tecla);
                if (n > -1) {
                    letra = true;
                }

                return letra;
            }


            function isLetraNumeroDoc(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode

                var letra = false;
                if (charCode > 1 && charCode < 4) {
                    letra = true;
                }
                if (charCode == 8) {
                    letra = true;
                }
                if (charCode == 32) {
                    letra = true;
                }
                if (charCode == 46) {
                    letra = true;
                }
                if (charCode >= 58 && charCode <= 59) {
                    letra = true;
                }
                if (charCode >= 48 && charCode <= 60) {
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
                if (charCode > 159 && charCode < 161) {
                    letra = true;
                }

                if (charCode > 161 && charCode < 164) {
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

                var letras = "aeiouAEIOU";
                var tecla = String.fromCharCode(charCode);
                var n = letras.indexOf(tecla);
                if (n > -1) {
                    letra = true;
                }

                return letra;
            }

           
        function ActoMilitar_Participantes() {
            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%= ddl_TipoParticipante.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_TipoDocParticipante.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNroDocParticipante.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_NacParticipante.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtNomParticipante.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txtApePatParticipante.ClientID %>")) == false) bolValida = false;

            if(document.getElementById("<%= lblApeMater.ClientID %>")!=null) {
                if (txtcontrolError(document.getElementById("<%= txtApeMatParticipante.ClientID %>")) == false) bolValida = false;
            }


            return bolValida;
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

        function ValidarPersona() {
            var bolValida = true;

            var strTipoDocParticipante = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());
            var ddlTipoDocParticipante = document.getElementById('<%= ddl_TipoDocParticipante.ClientID %>');
            if (strTipoDocParticipante == "0") {
                bolValida = false;
                ddlTipoDocParticipante.style.border = "1px solid Red";
            }
            else {
                ddlTipoDocParticipante.style.border = "1px solid #888888";
            }

            var strNroDocumento = $.trim($("#<%= txtNroDocParticipante.ClientID %>").val());
            var txtNroDocumento = document.getElementById('<%= txtNroDocParticipante.ClientID %>');
            if (strNroDocumento == "") {
                bolValida = false;
                txtNroDocumento.style.border = "1px solid Red";
            }
            else {
                txtNroDocumento.style.border = "1px solid #888888";
            }
            return bolValida;
        }
    </script>
    <script language="javascript" type="text/javascript">
        var nav4 = window.Event ? true : false; 

        $(function () {
            $('#tabs').tabs();
            $('#tabs').enableTab(1);
            $('#tabs').disableTab(3, false);
        });

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $(function () {
                $('#tabs').tabs();
                $('#tabs').enableTab(iTab);
            });
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        // Llamada a popup de mensaje de informacion
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

       
        //-- FUNCIONES-------------------

        function ValidarGrabar() {
            var bolValor = true;
            bolValor = ActoMilitar_formato();   //ACTO MILITAR           

            /*MENSAJE DE CONFIRMACIÓN*/
            var VistaPrevia=$("#<%= hifVistaPrevia.ClientID %>").val();
            if (VistaPrevia = 0) {
                if (bolValor) {
                    bolValor = confirm("¿Está seguro de grabar los cambios?");
                }
            }
            return bolValor;

        }

        //Para llamar a la vinculación
        function Vincular_Adhesivo() {
            document.getElementById("<%= btnGrabarVinculacion.ClientID %>").click();
//            $("#btnGrabarVinculacion").click();
        }

        function ValidarAutoadhesivo() {
            var bolValida = true;
            var strCodigo = $.trim($("#<%= txtCodAutoadhesivo.ClientID %>").val());
//            if (strCodigo.length < 12) {
//                bolValida = false;
//            }
            if (bolValida) {
                $("#<%= lblValidacionDetalle.ClientID %>").hide();
                abrirPopupEspera();
            }
            else {
                $("#<%= lblValidacionDetalle.ClientID %>").show();
            }
            return bolValida;
        }

        //-- VALIDACION GENERAL---------
        function txtcontrolError(ctrl) {
            var bolValida = true;
            if (ctrl != null) {
                var x = ctrl.value.trim();
                

                if (x.length == 0) {
                    ctrl.style.border = "1px solid Red";
                    bolValida = false;
                }
                else {
                    ctrl.style.border = "1px solid #888888";
                }
            }
            return bolValida;
        }


        function ddlcontrolError(ctrl) {
             if (ctrl != null) { 
                var x = ctrl.selectedIndex;
                var bolValida = true;
                if (x < 1) {
                    ctrl.style.border = "1px solid Red";
                    bolValida = false;
                }
                else {
                    ctrl.style.border = "1px solid #888888";
                }
            }
                return bolValida;
        }

        //-- FIN VALIDACION GENERAL---------
        function ActoMilitar_formato() {
            var bolValor = true;
            var i_estadoFoco = 0;

            var strClase = $.trim($("#<%= txtClase.ClientID %>").val());
            var txtClase = document.getElementById('<%= txtClase.ClientID %>');
            if (strClase == "") {
                bolValor = false;
                txtClase.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtClase.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtClase.style.border = "1px solid #888888";
            }

            var strLibro = $.trim($("#<%= txtLibro.ClientID %>").val());
            var txtLibro = document.getElementById('<%= txtLibro.ClientID %>');
            if (strLibro == "") {
                bolValor = false;
                txtLibro.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtLibro.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtLibro.style.border = "1px solid #888888";
            }

            var strFolio = $.trim($("#<%= txtFolio.ClientID %>").val());
            var txtFolio = document.getElementById('<%= txtFolio.ClientID %>');
            if (strFolio == "") {
                bolValor = false;
                txtFolio.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtFolio.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtFolio.style.border = "1px solid #888888";
            }

            var strFecNac = $.trim($('#<%= txtFecNac.FindControl("TxtFecha").ClientID  %>').val());
            var txtFecNac = document.getElementById('<%= txtFecNac.FindControl("TxtFecha").ClientID  %>');
            if (strFecNac == "") {
                bolValor = false;
                txtFecNac.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    i_estadoFoco = 1;
                }
            }
            else {
                txtFecNac.style.border = "1px solid #888888";
            }


            var strEstadoCivil = $.trim($("#<%= ddlEstadoCivil.ClientID %>").val());
            var ddlEstadoCivil = document.getElementById('<%= ddlEstadoCivil.ClientID %>');
            if (strEstadoCivil == "0") {
                bolValor = false;
                ddlEstadoCivil.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlEstadoCivil.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlEstadoCivil.style.border = "1px solid #888888";
            }


            var strEstatura = $.trim($("#<%= txtEstatura.ClientID %>").val());
            var txtEstatura = document.getElementById('<%= txtEstatura.ClientID %>');
            if (strEstatura == "" || parseFloat(strEstatura) == 0) {
                bolValor = false;
                txtEstatura.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtEstatura.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtEstatura.style.border = "1px solid #888888";
            }


            var strColorOjos = $.trim($("#<%= ddlColorOjos.ClientID %>").val());
            var ddlColorOjos = document.getElementById('<%= ddlColorOjos.ClientID %>');
            if (strColorOjos == "0") {
                bolValor = false;
                ddlColorOjos.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlColorOjos.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlColorOjos.style.border = "1px solid #888888";
            }


            var strHora = $.trim($("#<%= txtHora.ClientID %>").val());
            var txtHora = document.getElementById('<%= txtHora.ClientID %>');
            if (strHora == "") {
                bolValor = false;
                txtHora.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtHora.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtHora.style.border = "1px solid #888888";
            }


            var strNroHijos = $.trim($("#<%= txtNroHijos.ClientID %>").val());
            var txtNroHijos = document.getElementById('<%= txtNroHijos.ClientID %>');
            if (strNroHijos == "") {
                bolValor = false;
                txtNroHijos.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtNroHijos.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtNroHijos.style.border = "1px solid #888888";
            }


            var strPeso = $.trim($("#<%= txtPeso.ClientID %>").val());
            var txtPeso = document.getElementById('<%= txtPeso.ClientID %>');
            if (strPeso == "" || parseFloat(strPeso) == 0) {
                bolValor = false;
                txtPeso.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtPeso.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtPeso.style.border = "1px solid #888888";
            }


            var strGrupoSanguineo = $.trim($("#<%= ddlGrupoSanguineo.ClientID %>").val());
            var ddlGrupoSanguineo = document.getElementById('<%= ddlGrupoSanguineo.ClientID %>');
            if (strGrupoSanguineo == "0") {
                bolValor = false;
                ddlGrupoSanguineo.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlGrupoSanguineo.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlGrupoSanguineo.style.border = "1px solid #888888";
            }


            var strGenero = $.trim($("#<%= ddlGenero.ClientID %>").val());
            var ddlGenero = document.getElementById('<%= ddlGenero.ClientID %>');
            if (strGenero == "0") {
                bolValor = false;
                ddlGenero.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlGenero.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlGenero.style.border = "1px solid #888888";
            }

            var strColorTez = $.trim($("#<%= ddlColorTez.ClientID %>").val());
            var ddlColorTez = document.getElementById('<%= ddlColorTez.ClientID %>');
            if (strColorTez == "0") {
                bolValor = false;
                ddlColorTez.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlColorTez.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlColorTez.style.border = "1px solid #888888";
            }

            var strSeniasPart = $.trim($("#<%= txtSeniasPart.ClientID %>").val());
            var txtSeniasPart = document.getElementById('<%= txtSeniasPart.ClientID %>');
            if (strSeniasPart == "") {
                bolValor = false;
                txtSeniasPart.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtSeniasPart.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtSeniasPart.style.border = "1px solid #888888";
            }

            var strNacLugarTipo = $.trim($("#<%= ddlNacLugarTipo.ClientID %>").val());
            var ddlNacLugarTipo = document.getElementById('<%= ddlNacLugarTipo.ClientID %>');
            if (strNacLugarTipo == "0") {
                bolValor = false;
                ddlNacLugarTipo.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    ddlNacLugarTipo.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                ddlNacLugarTipo.style.border = "1px solid #888888";
            }

            var strNacLugar = $.trim($("#<%= txtNacLugar.ClientID %>").val());
            var txtNacLugar = document.getElementById('<%= txtNacLugar.ClientID %>');
            if (strNacLugar == "") {
                bolValor = false;
                txtNacLugar.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtNacLugar.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtNacLugar.style.border = "1px solid #888888";
            }

            var strDireccion = $.trim($("#<%= txtDireccion.ClientID %>").val());
            var txtDireccion = document.getElementById('<%= txtDireccion.ClientID %>');
            if (strDireccion == "") {
                bolValor = false;
                txtDireccion.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtDireccion.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtDireccion.style.border = "1px solid #888888";
            }

            var strDomicilioTelef = $.trim($("#<%= txtDomicilioTelef.ClientID %>").val());
            var txtDomicilioTelef = document.getElementById('<%= txtDomicilioTelef.ClientID %>');
            if (strDomicilioTelef == "") {
                bolValor = false;
                txtDomicilioTelef.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtDomicilioTelef.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtDomicilioTelef.style.border = "1px solid #888888";
            }

            return bolValor;
        }

        
        function ValidarPersona() {
            var bolValida = true;

            var strTipoDocParticipante = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());
            var ddlTipoDocParticipante = document.getElementById('<%= ddl_TipoDocParticipante.ClientID %>');
            if (strTipoDocParticipante == "0") {
                bolValida = false;
                ddlTipoDocParticipante.style.border = "1px solid Red";
            }
            else {
                ddlTipoDocParticipante.style.border = "1px solid #888888";
            }

            var strNroDocumento = $.trim($("#<%= txtNroDocParticipante.ClientID %>").val());
            var txtNroDocumento = document.getElementById('<%= txtNroDocParticipante.ClientID %>');
            if (strNroDocumento == "") {
                bolValida = false;
                txtNroDocumento.style.border = "1px solid Red";
            }
            else {
                txtNroDocumento.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        //--VALIDACION HORAS--
        function validHoraII(evt) {
            // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57  
            var key = nav4 ? evt.which : evt.keyCode;
            return (key <= 13 || (key >= 48 && key <= 57) || key == 58);
        }

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
                alert("Formato de hora Incorrecto");
                ctrl.value = "";
            }
        }

        function isHoraKeyII(ctrl, evt) {
            var tIngresado = ctrl.value;
            var charCode = (evt.which) ? evt.which : event.keyCode

            if ((tIngresado.Len == 2) && (charCode == 58)) {
                tIngresado = tIngresado + ":" ;
            }
            else {
                ctrl.value = tIngresado;
            }
            return true;
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
//        function validaHora(ctrl) {
//            var x = ctrl.value;
//            var valor = true;

//            if (x.length == 4) {
//                x = x + "0";
//                ctrl.value = x;
//            }

//            var hora = x.substring(0, 2);
//            var minuto = x.substring(3, 5);

//            if (hora > 23) {
//                valor = false;
//            }
//            if (minuto > 59) {
//                valor = false;
//            }
//            if (x.length < 5) {
//                valor = false;
//            }
//            if (x.length == 0) {
//                valor = true;
//            }
//            if (!valor) {

//                ctrl.focus();
//                alert("Formato de hora Incorrecto");
//                ctrl.value = "";

//            }
//        }

//        function isHoraKey(ctrl, evt) {
//            var charCode = (evt.which) ? evt.which : event.keyCode
//            var FIND = ":";
//            var x = ctrl.value;
//            var y = x.indexOf(FIND);

//            var minuto = x.split(':');
//            if (x.length == 2) {
//                x = x + ":";
//                ctrl.value = x;
//            }

//            if (x.split(':').length = 1) {
//                if (x.length > 1 && x.length < 3) {
//                    if (minuto[0].length > 1 && charCode != 58) {
//                        return false;
//                    }
//                }
//            }

////            if (charCode == 8) {
////                letra = true;
////            }

//            if (charCode == 58) {
//                return false;
//            }

//            if (charCode < 48 || charCode > 58)
//                return false;
//            return true;
//        }
        //--FIN VALIDACION HORAS--


        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

//        function conLimit(element, limit) {
//            var max_chars = limit;

//            if (element.value.length > max_chars) {
//                element.value = element.value.substr(0, max_chars);
//            }
//        }

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


            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 37) {
                letra = false;
            }
            if (charCode == 38) {
                letra = false;
            }
            if (charCode == 60) {
                letra = false;
            }
            if (charCode == 62) {
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

        //--VALIDACION NUMEROS--

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
                alert("Número Incorrecto");
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

        //--FIN VALIDACION NUMEROS--
    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</asp:Content>
