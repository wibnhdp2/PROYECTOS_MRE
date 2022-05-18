<%@ Page Title="" Language="C#" UICulture="es-PE" Culture="es-PE" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FrmActoGeneral.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActoGeneral" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ctrlTool"
    TagName="ctrlTool" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc2" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlAdjunto.ascx" TagName="ctrlAdjunto"
    TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlCargando.ascx" TagName="ctrlCargando"
    TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlReimprimirbtn.ascx" TagName="ctrlReimprimirbtn"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlBajaAutoadhesivo.ascx" TagName="ctrlBajaAutoadhesivo"
    TagPrefix="uc5" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .style6
        {
            width: 168px;
        }
        .style7
        {
            width: 204px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function pageLoad() {
            $("#<%=txtEstatura.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtAnio.ClientID %>").on("keydown", validarDecimal);
        };

        function Load() {

            //Previene el postback al hacer enter
            $(function () {
                $(':text').bind('keydown', function (e) {
                    //on keydown for all textboxes
                    if (e.target.className != "searchtextbox") {
                        if (e.keyCode == 13) { //if this is enter key
                            e.preventDefault();
                            return false;
                        } else
                            return true;
                    } else
                        return true;
                });
                $('#<%= txtCodAutoadhesivo.ClientID %>').focus();
            });

            //Posibilita desplazamiento con enter entre campos
            $(function () {
                $('input:text:first').focus();
                var $inp = $('input:text');
                $inp.bind('keydown', function (e) {
                    //var key = (e.keyCode ? e.keyCode : e.charCode);
                    var key = e.which;
                    if (key == 13) {
                        e.preventDefault();
                        var nxtIdx = $inp.index(this) + 1;
                        $(":input:text:eq(" + nxtIdx + ")").focus();
                    }
                });
            });
        } 

    </script>

    <script language="javascript" type="text/javascript">
        function ValidarCaracteres(textareaControl, maxlength) {

            if (textareaControl.value.length > maxlength) {
                textareaControl.value = textareaControl.value.substring(0, maxlength);
                alert("Debe ingresar hasta un maximo de " + maxlength + " caracteres.");
            }
        }

        function validarDecimal(e) {
            if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
                return;
            }
            else {
                if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105 ||
                (e.keyCode == 110) ||
                (e.keyCode == 190)))) {
                    // not 0-9, numpad 0-9                    
                    e.preventDefault();
                    return;
                }
                else {
                    var keyCode = e.keyCode;
                    if (keyCode >= 96 && keyCode <= 105) {
                        keyCode -= 48;
                    }

                    var value = $(this).val();

                    if (keyCode == 110 || keyCode == 190) {
                        if (value.indexOf('.') === -1) {
                            value += String.fromCharCode(keyCode);
                            value = parseInt(value, 10)
                            var maxNumber = $(this).data("maxnumber");
                            if (maxNumber) {
                                maxNumber = parseInt(maxNumber);
                                if (value > maxNumber) {
                                    e.preventDefault();
                                }
                            }
                        }
                        else {
                            e.preventDefault();
                            return;
                        }
                    }
                    else {
                        value += String.fromCharCode(keyCode);
                        value = parseInt(value, 10)
                        var maxNumber = $(this).data("maxnumber");
                        if (maxNumber) {
                            maxNumber = parseInt(maxNumber);
                            if (value > maxNumber) {
                                e.preventDefault();
                            }
                        }
                    }
                }
            }
        };
    </script>
    <script language="javascript" type="text/javascript">
        function deleteConfirmation() {
            return confirm("¿Esta seguro de anular el Antecedente Penal?");
        }
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
        function validarEmail(ctrl) {

            var email = ctrl.value;

        if (email.length > 0) {
            expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!expr.test(email)) {
                //alert("xxxx");
                showpopupother("a", "Información", "Error: La dirección de correo " + email + " es incorrecta.", "false", 200, 250);
                ctrl.value = "";
            }
        }
    }
</script>


<script type="text/javascript">
    function ActivarLeySustento() {
        var isCheckedNormativa = $('#<%= RBNormativa.ClientID %>').prop('checked');
        var isCheckedSustento = $('#<%= RBSustentoTipoPago.ClientID %>').prop('checked');
        
        if (isCheckedNormativa) {
             $('#<%= ddlExoneracion.ClientID %>').attr('disabled', false);
             $('#<%= ddlExoneracion.ClientID %>').val('0');
             $('#<%= txtSustentoTipoPago.ClientID %>').val('');
             $('#<%= txtSustentoTipoPago.ClientID %>').attr('disabled', true);
             $('#<%= ddlExoneracion.ClientID %>').focus();
        }

         if (isCheckedSustento) {
            $('#<%= ddlExoneracion.ClientID %>').val('0');
            $('#<%= ddlExoneracion.ClientID %>').attr('disabled', true);
            $('#<%= txtSustentoTipoPago.ClientID %>').attr('disabled', false);
            $('#<%= txtSustentoTipoPago.ClientID %>').val('');
            $('#<%= txtSustentoTipoPago.ClientID %>').focus();
        }
    }

    function CmbGenero_SelectedIndexChanged() {
        var CmbGenero = $("#<%=CmbGenero.ClientID %>");
        var CmbEstCiv = $("#<%=CmbEstCiv.ClientID %>");

        $("#<%=CmbGenero.ClientID %>").focus();
        if ((CmbGenero.val() == "2002") && ((CmbEstCiv.val() == "2") || (CmbEstCiv.val() == "4"))) {
            $("#<%=chkConsignaApeCas.ClientID %>").css("display", "block");
            $("#<%=chkConsignaApeCas.ClientID %>").prop("checked",false);
            $("#<%=lblConsignaApeCas.ClientID %>").css("display", "block");
            $("#<%=lblApellidoCasada.ClientID %>").css("display", "block");
            $("#<%=txtApeCasadaParticipante.ClientID %>").val('');
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', true);
            $("#<%=txtApeCasadaParticipante.ClientID %>").css('display', "block");
        }
        else {
            $("#<%=chkConsignaApeCas.ClientID %>").prop("checked", false);
            $("#<%=chkConsignaApeCas.ClientID %>").css("display", "none");
            $("#<%=lblConsignaApeCas.ClientID %>").css("display", "none");
            $("#<%=lblApellidoCasada.ClientID %>").css("display", "none");
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', true);
            $("#<%=txtApeCasadaParticipante.ClientID %>").css("display", "none");
        }
    }

    function CmbGenero_VisibleApellidoCasada() {
        var CmbGenero = $("#<%=CmbGenero.ClientID %>");
        var CmbEstCiv = $("#<%=CmbEstCiv.ClientID %>");

        $("#<%=CmbGenero.ClientID %>").focus();
        if ((CmbGenero.val() == "2002") && ((CmbEstCiv.val() == "2") || (CmbEstCiv.val() == "4"))) {
            $("#<%=chkConsignaApeCas.ClientID %>").css("display", "block");
            $("#<%=lblConsignaApeCas.ClientID %>").css("display", "block");
            $("#<%=lblApellidoCasada.ClientID %>").css("display", "block");
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', true);
            $("#<%=txtApeCasadaParticipante.ClientID %>").css('display', "block");
        }
        else {
            $("#<%=chkConsignaApeCas.ClientID %>").prop("checked", false);
            $("#<%=chkConsignaApeCas.ClientID %>").css("display", "none");
            $("#<%=lblConsignaApeCas.ClientID %>").css("display", "none");
            $("#<%=lblApellidoCasada.ClientID %>").css("display", "none");
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', true);
            $("#<%=txtApeCasadaParticipante.ClientID %>").css("display", "none");
        }
    }

    function chkConsignaApeCas_VerificarChecked() {
        if ($("#<%=chkConsignaApeCas.ClientID %>").prop("checked")) {
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', false);
            $("#<%=txtApeCasadaParticipante.ClientID %>").focus();
            var ApeCasada = $("#<%=hApeCasadaParticipante.ClientID %>").val();
            $("#<%=txtApeCasadaParticipante.ClientID %>").val(ApeCasada);
        } else {
            $("#<%=txtApeCasadaParticipante.ClientID %>").val('');
            $("#<%=txtApeCasadaParticipante.ClientID %>").attr('disabled', true);
        }

    }
    function ObtenerElementosGenero() {
        var comboGenero = document.getElementById('<%= CmbGenero.ClientID %>');
        var comboEstadoCivil = document.getElementById('<%= CmbEstCiv.ClientID %>');
        var letra = '';

        if (comboGenero.selectedIndex > 0) {
            if (comboGenero.options[comboGenero.selectedIndex].text == 'MASCULINO') {
                letra = 'O';
            }
            else {
                letra = 'A';
            }
        }
        else {
            letra = 'O';
        }
        for (i = 1; i < comboEstadoCivil.length; i++) {
            var largo = comboEstadoCivil.options[i].text.length;
            comboEstadoCivil.options[i].text = comboEstadoCivil.options[i].text.substring(0, largo - 1) + letra;
        }

    }
</script>

    <div>
        <asp:HiddenField ID="HFGUID" runat="server" />
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuación Consular"></asp:Label></h2>
                </td>
                <td style="text-align: right;">
                    <asp:LinkButton ID="Tramite" runat="server" 
                        Font-Bold="True" Font-Size="10pt" ForeColor="Blue" Font-Underline="true" 
                        onclick="Tramite_Click">Regresar a Trámites</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblTituloTarifa" CssClass="titulo_tarifa" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <table style="width: 90%;" align="center" class="mTblSecundaria" bgcolor="#4E102E">
            <tr>
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblDestino" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                                    Font-Underline="false" ForeColor="White" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 90%;" align="center">
            <tr>
                <td>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-5">
                                <asp:Label ID="lblRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblDatos" runat="server" Text="Formato"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblAdjuntos" runat="server" Text="Adjuntos"></asp:Label></a></li>
                            <li><a href="#tab-3">
                                <asp:Label ID="lblAnotacion" runat="server" Text="Anotaciones"></asp:Label></a></li>
                            <li><a href="#tab-6">
                                <asp:Label ID="lblFicha" runat="server" Text="Ficha Registral"></asp:Label></a></li>
                            <li><a href="#tab-7">
                                <asp:Label ID="lblAntecedente" runat="server" Text="Antecedentes Penales"></asp:Label></a></li>
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
                                            <td align="right">
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
                                                                        <asp:Label ID="lblLeyendaTarifa" runat="server" Text=" (Ingresar código actuación y Presionar tecla ENTER)"
                                                                            Visible="false" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top;">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtIdTarifa" runat="server" Width="38px" CssClass="campoNumero"
                                                                                        MaxLength="5" ToolTip="Coloque el número de la seccion del tarifario" OnTextChanged="txtIdTarifa_TextChanged" />
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
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:ListBox ID="LstTarifario" runat="server" Width="659px" 
                                                                                                BackColor="#EAFFFF" />
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
                                                <asp:HiddenField ID="hTipoPago" runat="server" />
                                            </td>
                                            <td align="left">
                                               <table>
                                                <tr>
                                                    <td>
                                                     <asp:DropDownList ID="ddlTipoPago" runat="server" Height="20px" Width="200px" AutoPostBack="True"
                                                    Enabled="True" OnSelectedIndexChanged="ddlTipoPago_SelectedIndexChanged" />
                                                      <asp:Label ID="lblCO_cmb_TipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width:70px">
                                                        <asp:Label ID="lblExoneracion" runat="server" Text="Ley que exonera el pago:"></asp:Label>
                                                    </td>
                                                    <td>
                                                    <asp:DropDownList ID="ddlExoneracion" runat="server" Enabled="True" Height="20px"
                                                    Width="414px" />                                                    
                                                    <asp:Label ID="lblValExoneracion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    <asp:RadioButton ID="RBNormativa" runat="server" Text=""  style="cursor:pointer"
                                                        GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td align="right" style="width:70px">
                                                        <asp:Label ID="lblSustentoTipoPago" runat="server" Text="Sustento:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSustentoTipoPago" runat="server" Width="410px" CssClass="txtLetra" ></asp:TextBox>
                                                        <asp:Label ID="lblValSustentoTipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                        <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text=""  style="cursor:pointer" onclick="ActivarLeySustento()"
                                                        GroupName="GrupoExonera" Width="15px" />
                                                            

                                                    </td>
                                                </tr>
                                               </table>
                                                                                                                                                                                             
                                            </td>
                                            <td>                                                                                        
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td align="right">
                                            <asp:Label ID="lblClasificacion" runat="server" Text="Clasificación:"></asp:Label>    
                                            </td>
                                            <td>
                                            <asp:DropDownList ID="ddlClasificacion" runat="server" Enabled="True" Height="20px"
                                                    Width="420px" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>

                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lblCantidad" runat="server" Text="Cantidad:"></asp:Label>
                                            </td>
                                            <td style="margin-left: 40px">
                                                <asp:TextBox ID="txtCantidad" runat="server" AutoPostBack="True" 
                                                    CssClass="campoNumero" Enabled="True" MaxLength="2" Width="100px" />
                                                &nbsp;
                                                <asp:Label ID="lblCO_txtCantidad" runat="server" Style="color: #FF0000" 
                                                    Text="*"></asp:Label>
                                                <asp:Label ID="lblLeyenda" runat="server" Font-Bold="true" 
                                                    Text=" (Presionar tecla ENTER para calcular total)" Visible="false"></asp:Label>
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
                                                            <asp:Label ID="lblCO_txtNroOperacion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            <asp:HiddenField ID="hModificaTemporal" runat="server" />
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
                                                            <asp:DropDownList ID="ddlNomBanco" runat="server" Width="300px" MaxLength="10">
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
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:HiddenField ID="HF_PAGADO_EN_LIMA" Value="3501" runat="server" />
                                                <asp:HiddenField ID="HF_TRANSFERENCIA" Value="3507" runat="server" />
                                                <asp:HiddenField ID="HF_DEPOSITO_CUENTA" Value="3508" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-1">
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updActuacionAdjuntar" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <uc3:ctrlAdjunto ID="ctrlAdjunto" runat="server"></uc3:ctrlAdjunto>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-3">
                        </div>
                        <div id="tab-6">
                            <asp:UpdatePanel ID="updFicha" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <ctrlTool:ctrlTool ID="ctrlToolBarFicha" runat="server"></ctrlTool:ctrlTool>
                                                <asp:HiddenField ID="hNuevo" runat="server" />
                                                <asp:HiddenField ID="hFichaRegistralId" runat="server" />
                                                <asp:HiddenField ID="hNumeroFicha" runat="server" />
                                                <asp:HiddenField ID="hNuevaDireccion" runat="server" />
                                                <asp:HiddenField ID="hRepetido" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="float: right;">
                                        <asp:RadioButton ID="rbManual" Text="Registro Manual" GroupName="RegistroSIO" runat="server"
                                            BorderStyle="None" Checked="True" Font-Bold="True" AutoPostBack="True" OnCheckedChanged="rbManual_CheckedChanged" /><br />
                                        <asp:RadioButton ID="rbSemi" Text="Semiautomático (SIO)" GroupName="RegistroSIO"
                                            runat="server" Font-Bold="True" AutoPostBack="True" OnCheckedChanged="rbSemi_CheckedChanged" />
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div style="border: 1px solid #dcdcdc;">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblNroFicha" runat="server" Text="Nro Ficha Registral RENIEC:"></asp:Label>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="style7">
                                                    <asp:TextBox ID="txtNroFicha" runat="server" Width="100px" MaxLength="10" onkeypress="return validarTelefonos(event)"
                                                        TabIndex="490"></asp:TextBox>
                                                    <asp:Label ID="Label6" runat="server" Text="*" Style="margin-left: 3px; color: #FF0000"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblEstadoActa" runat="server" Text="Estado del Acta Registral:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEstadoFicha" runat="server" TabIndex="492" Width="130px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlEstadoFicha_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblFechaRegistro" runat="server" Text="Fecha de Registro:"></asp:Label>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="style7">
                                                    <SGAC_Fecha:ctrlDate ID="ctrFechaFicha" runat="server" />
                                                </td>
                                                <td align="right" class="style6">
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblCodigoLocalDestino" runat="server" Text="Código de Local Destino:"></asp:Label>
                                                </td>
                                                <td class="style7">
                                                    <input id="chkCodigoLocal" type="checkbox" onclick="ObtenerCodigoLocal()" />
                                                    Local de Lima
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlLocalDestino" runat="server" TabIndex="493" Width="350px">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label5" runat="server" Text="*" Style="margin-left: 3px; color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <div id="DivSemiAutomatico" runat="server">
                                                    <td align="right">
                                                        <asp:Label ID="lblFecEnvio" runat="server" Text="Fecha de Envío:"></asp:Label>
                                                    </td>
                                                    <td class="style7">
                                                        <SGAC_Fecha:ctrlDate ID="ctrFechaEnvio" runat="server" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblNroHojaRemOfi" runat="server" Text="Nro. Hoja de Remisión/Oficio:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtHojaRemOfi" runat="server"></asp:TextBox>
                                                    </td>
                                                </div>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblObservacion0" runat="server" Text="Observación:"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtObservacionFicha" runat="server" CssClass="txtLetra" Height="50px"
                                                        MaxLength="500" TextMode="MultiLine" Width="98%" TabIndex="495" />
                                                    <asp:HiddenField ID="hObservación" runat="server" />
                                                    <asp:HiddenField ID="hEstadoInicial" runat="server" />
                                                </td>
                                            </tr>
                                            <%--     <tr>
                                                <td class="style5">
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>--%>
                                            <%--<tr>
                                                <td class="style2" colspan="4">
                                                    <asp:Button ID="btnAgregarFicha" runat="server" Text="     Adicionar Ficha" Width="150px"
                                                        CssClass="btnNew" OnClick="btnAgregarFicha_Click" TabIndex="494" />
                                                    <asp:Button ID="btnAceptarFicha" runat="server" Text="     Aceptar" CssClass="btnSave"
                                                        OnClick="btnAceptarFicha_Click" TabIndex="495" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td colspan="4">
                                                    <div id="grillaOcultar" class="ocultar" style="display: none;">
                                                        <asp:GridView ID="GridViewFicha" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="GridViewFicha_RowCommand"
                                                            OnRowDataBound="GridViewFicha_RowDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFichaRegistralId" runat="server" Text='<%# Bind("FIRE_IFICHAREGISTRALID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNumeroFicha" runat="server" Text='<%# Bind("FIRE_VNUMEROFICHA") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstadoFicha" runat="server" Text='<%# Bind("ESTA_VDESCRIPCIONCORTA") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="INUMERO" HeaderText="Nro.">
                                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FIRE_DFECHAESTADO" HeaderText="Fecha del Registro" DataFormatString="{0:dd-MM-yyyy}">
                                                                    <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FIRE_VNUMEROFICHA" HeaderText="Número del Acta">
                                                                    <ItemStyle Width="110px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ESTA_VDESCRIPCIONCORTA" HeaderText="Estado del Acta">
                                                                    <ItemStyle HorizontalAlign="Center" Width="110px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FIRE_VOBSERVACION" HeaderText="Observación">
                                                                    <ItemStyle Width="350px" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnSeleccionar" runat="server" CommandName="Select" ImageUrl="~/Images/img_16_success.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="Editar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" runat="server" CommandName="Editar" ImageUrl="../Images/img_16_edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                                <%--<asp:TemplateField HeaderText="Anular">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnAnular" runat="server" CommandName="Anular" ImageUrl="../Images/img_grid_delete.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <uc2:ctrlPageBar ID="CtrlPageBarFichaRegistral" runat="server" OnClick="ctrlPageBarFichaRegistral_Click"
                                                        Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAgregarParticipante" runat="server" Text="     Agregar Participante"
                                                        Width="170px" CssClass="btnNew" TabIndex="494" OnClientClick="abrirPopupLimpiar(); return false" />
                                                    <asp:HiddenField ID="hNuevoParticipante" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hNuevoParticipanteEDITARID" runat="server" Value="0" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDocAdjuntosFicha" runat="server" Text="     Documentos Adjuntos"
                                                        Width="170px" CssClass="btnNew" TabIndex="494" OnClientClick="abrirPopupDocAdjuntosLimpiar(); return false"/>
                                                    <asp:HiddenField ID="hDocAdjuntoEditar" runat="server" Value = "0" />
                                                    <asp:HiddenField ID="hDocAdjuntoDDL" runat="server" Value = "0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="modalParticipantes" class="modal">
                                        <div class="modal-windowlarge">
                                            <div class="modal-titulo">
                                                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                                                    OnClientClick="cerrarPopup();return false" runat="server" />
                                                <span>PARTICIPANTES</span>
                                            </div>
                                            <div class="modal-cuerpoNormal">
                                                <asp:Panel ID="pnlParticipantes" runat="server">
                                                    <table width="99%">
                                                        <tr>
                                                            <td align="left">
                                                                <h5>
                                                                    <asp:Label ID="Label2" runat="server" Text="Datos del Participante de la Ficha Registral"></asp:Label>
                                                                </h5>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:UpdatePanel ID="updParticipa" runat="server">
                                                        <ContentTemplate>
                                                            <table width="99.5%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                                                border-left: 1px solid gray; border-right: 1px solid gray; margin-left: 2px;">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="lblTipoParticipante" runat="server" Text="Tipo de Participante:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:DropDownList ID="ddl_TipoParticipante" runat="server" Style="cursor: pointer;"
                                                                            Height="21px" Width="200px" TabIndex="500" AutoPostBack="True" OnSelectedIndexChanged="ddl_TipoParticipante_SelectedIndexChanged" />
                                                                        <asp:Label ID="lbl_asterisco_01" runat="server" Text="*" Style="margin-left: 3px;
                                                                            color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <div id="DIV_TIPO_DECLARANTE" runat="server" visible="false">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="Label26" runat="server" Text="Tipo Declarante :" />
                                                                        </td>
                                                                        <td style="width: 220px;">
                                                                            <asp:DropDownList ID="ddlDeclarante" runat="server" Height="21px" Style="cursor: pointer;"
                                                                                TabIndex="501" Width="200px" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label28" runat="server" Text="Tipo Tutor/Guardador:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlTutorG" runat="server" Height="21px" Style="cursor: pointer;"
                                                                                TabIndex="501" Width="200px" />
                                                                        </td>
                                                                        <td align="left" style="width: 10%">
                                                                        </td>
                                                                    </tr>
                                                                </div>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="LblTipDoc" runat="server" Text="Tipo de Documento :" />
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:DropDownList ID="ddl_TipoDocParticipante" runat="server" Height="21px" onchange="TipoDocSelectedChange();"
                                                                            Style="cursor: pointer;" TabIndex="501" Width="200px" />
                                                                        <asp:Label ID="lbl_asterisco_02" runat="server" Style="margin-left: 3px; color: #FF0000"
                                                                            Text="*"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="LblNroDoc" runat="server" Text="Nro Documento:" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNroDocParticipante" runat="server" CssClass="campoNumero" MaxLength="12"
                                                                            onkeypress="return  ValidarNumeroDocumento(event)" TabIndex="502" 
                                                                            Width="220px" AutoPostBack="True" 
                                                                            ontextchanged="txtNroDocParticipante_TextChanged" />
                                                                        <%--<img id="imgBuscarDocumento" alt="Buscar" src="../Images/img_16_search.png" onclick="return imgBuscarDocumento_onclick()" />--%>
                                                                        <asp:ImageButton ID="imbBuscarPersona" alt="Buscar" src="../Images/img_16_search.png"
                                                                            runat="server" OnClick="imbBuscarPersona_Click" />
                                                                        <asp:Label ID="lbl_asterisco_03" runat="server" Style="margin-left: 3px; color: #FF0000"
                                                                            Text="*"></asp:Label>
                                                                        <asp:Button ID="btnBuscarParticipante" runat="server" BackColor="White" CssClass="hideControl"
                                                                            Height="1px" OnClick="btnBuscarParticipante_Click" Width="1px" />
                                                                    </td>
                                                                    <td align="left" style="width: 10%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label32" runat="server" Text="Primer Apellido:" />
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:TextBox ID="txtApePatParticipante" runat="server" CssClass="txtLetra" Width="200px"
                                                                            TabIndex="503" />
                                                                        <asp:Label ID="lbl_asterisco_04" runat="server" Text="*" Style="margin-left: 3px;
                                                                            color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label7" runat="server" Text="Estado Civil :" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="CmbEstCiv" runat="server" Width="150px" onChange="javascript:CmbGenero_SelectedIndexChanged();">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" style="width: 10%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label45" runat="server" Text="Segundo Apellido:" />
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:TextBox ID="txtApeMatParticipante" runat="server" CssClass="txtLetra" Width="200px"
                                                                            TabIndex="504" />
                                                                        <asp:Label ID="lbl_asterisco_05" runat="server" Text="*" Style="margin-left: 3px;
                                                                            color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblGenero" runat="server" Text="Género :" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="CmbGenero" runat="server" Width="150px" onChange="javascript:CmbGenero_SelectedIndexChanged();ObtenerElementosGenero();">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" style="width: 10%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="lblApellidoCasada" runat="server" Text="Apellido de Casada:" />
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:TextBox ID="txtApeCasadaParticipante" runat="server" CssClass="txtLetra" Width="200px"
                                                                            TabIndex="504" />
                                                                        <asp:HiddenField ID="hApeCasadaParticipante" runat="server" />
                                                                    </td>
                                                                    <td align="left"colspan="3">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="right">
                                                                                <asp:CheckBox ID="chkConsignaApeCas"  runat="server" onChange="javascript:chkConsignaApeCas_VerificarChecked();"/>
                                                                                </td>
                                                                                <td align="justify">
                                                                                    <asp:Label ID="lblConsignaApeCas" runat="server" Text="¿Desea que se consigne el apellido de casada? (*)" Font-Bold="True"></asp:Label>  
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label34" runat="server" Text="Nombres:" />
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:TextBox ID="txtNomParticipante" runat="server" CssClass="txtLetra" Width="200px"
                                                                            TabIndex="505" />
                                                                        <asp:Label ID="lbl_asterisco_06" runat="server" Text="*" Style="margin-left: 3px;
                                                                            color: #FF0000"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label10" runat="server" Text="Grado de Instrucción:" />
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:DropDownList ID="CmbGradInst" runat="server" Width="90px">
                                                                        </asp:DropDownList>
                                                                        &nbsp
                                                                        <asp:Label ID="Label12" runat="server" Text="Año Estudio:" />&nbsp;
                                                                        <asp:TextBox ID="txtAnio" Width="50px" MaxLength="4" runat="server"></asp:TextBox>&nbsp;
                                                                        <asp:Label ID="Label13" runat="server" Text="Completo?:" />
                                                                        <asp:DropDownList ID="ddlCompleto" runat="server" Width="50px">
                                                                            <asp:ListItem Value="0" Text="- SELECCIONAR -"></asp:ListItem>
                                                                            <asp:ListItem Value="N" Text="NO"></asp:ListItem>
                                                                            <asp:ListItem Value="S" Text="SI"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" style="width: 10%">
                                                                    </td>
                                                                </tr>
                                                                
                                                                <tr>
                                                                    <td align="right">
                                                                        </td>
                                                                    <td style="width: 220px;">
                                                                        </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label11" runat="server" Text="Estatura :" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEstatura" runat="server" MaxLength="4" CssClass="campoNumero"
                                                                            Width="40px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" style="width: 10%">
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="4">
                                                                        <hr width="96%" align="center" />
                                                                    </td>
                                                                </tr>
                                                                <div id="DIV_LUGAR_DOMICILIO" runat="server">
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <h5>
                                                                                <asp:Label ID="Label3" runat="server" Text="Lugar de Domicilio"></asp:Label>
                                                                            </h5>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblDirDir" runat="server" Text="Dirección :" />
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="TxtDirDir" runat="server" CssClass="txtLetra" MaxLength="200" onkeypress="return isSujeto(event)"
                                                                                Width="580px" TabIndex="506" />
                                                                            <asp:Label ID="lbl_asterisco_07" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                            <asp:HiddenField ID="hd_DirDir" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblDptoContDir" runat="server" Text="Dpto./Continente:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_DeptDomicilio" runat="server" AutoPostBack="True" Style="cursor: pointer;"
                                                                                OnSelectedIndexChanged="ddl_DeptDomicilio_SelectedIndexChanged" Width="200px"
                                                                                TabIndex="507" />
                                                                            <asp:Label ID="lbl_asterisco_08" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblProvPaisDir" runat="server" Text="Prov./País :" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_ProvDomicilio" runat="server" AutoPostBack="True" Style="cursor: pointer;"
                                                                                OnSelectedIndexChanged="ddl_ProvDomicilio_SelectedIndexChanged" Width="200px"
                                                                                TabIndex="508" />
                                                                            <asp:Label ID="lbl_asterisco_09" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td align="left" style="width: 10%">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblDistCiuDir" runat="server" Text="Dist./Ciudad /Estado:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_DistDomicilio" runat="server" Style="cursor: pointer;"
                                                                                Width="200px" TabIndex="509" />
                                                                            <asp:Label ID="lbl_asterisco_10" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label14" runat="server" Text="Codigo Postal :" />
                                                                        </td>
                                                                        <td>
                                                                           <asp:TextBox ID="txtCodPostal" runat="server" Width="100px"  />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblLocalidad" runat="server" Text="Localidad/Ciudad:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_LocalidadDomicilio" runat="server" TabIndex="510" Width="200px" />
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <hr width="96%" align="center" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="lbldFecNacParticipante" runat="server" Text="Fecha de Nacimiento:" />
                                                                        </td>
                                                                        <td>
                                                                            <SGAC_Fecha:ctrlDate ID="txtFechaNacimiento" runat="server" />
                                                                            <asp:Label ID="lbl_asterisco_11" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="LblTelfDir" runat="server" Text="Teléfono :" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TxtTelfDir" runat="server" MaxLength="25" onkeypress="return validarTelefonos(event)"
                                                                                Width="196px" TabIndex="512" />
                                                                        </td>
                                                                        <td align="left" style="width: 10%">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblSenasParticulares" runat="server" Text="Observaciones y/o Señas Particulares:" />
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="TxtSenasParticulares" runat="server" CssClass="txtLetra" MaxLength="250"
                                                                                onkeypress="return isSujeto(event)" onpaste="return false" TabIndex="513" onKeyDown="if(event.keyCode==13) event.keyCode=9;return limitar(event,this.value,250)"
                                                                                onkeyUp="return limitar(event,this.value,250)" Width="580px" TextMode="MultiLine" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblemail" runat="server" Text="Correo Electrónico:" />
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="TxtEmail" runat="server" Width="100%" CssClass="txtLetra" MaxLength="55"
                                                                                onblur="if(this.value!=''){ validarEmail(this); }" />
                                                                        </td>
                                                                    </tr>
                                                                </div>
                                                                <div id="DIV_LUGAR_NACIMIENTO" runat="server">
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <h5>
                                                                                <asp:Label ID="Label4" runat="server" Text="Lugar de Nacimiento"></asp:Label>
                                                                            </h5>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblDptoContDir0" runat="server" Text="Dpto./Continente:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_DeptNacimiento" runat="server" AutoPostBack="True" Style="cursor: pointer;"
                                                                                OnSelectedIndexChanged="ddl_DeptNacimiento_SelectedIndexChanged" Width="200px"
                                                                                TabIndex="514" />
                                                                            <asp:Label ID="lbl_asterisco_12" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblProvPaisDir0" runat="server" Text="Prov./País :" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_ProvNacimiento" runat="server" AutoPostBack="True" Style="cursor: pointer;"
                                                                                OnSelectedIndexChanged="ddl_ProvNacimiento_SelectedIndexChanged" Width="200px"
                                                                                TabIndex="515" />
                                                                            <asp:Label ID="lbl_asterisco_13" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td align="left" style="width: 10%">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblDistCiuDir0" runat="server" Text="Dist./Ciudad /Estado:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_DistNacimiento" runat="server" Width="200px" Style="cursor: pointer;"
                                                                                TabIndex="516" />
                                                                            <asp:Label ID="lbl_asterisco_14" runat="server" Text="*" Style="margin-left: 3px;
                                                                                color: #FF0000" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LblLocalidad0" runat="server" Text="Localidad/Ciudad:" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_LocalidadNacimiento" runat="server" TabIndex="517" Width="200px" />
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </div>
                                                            </table>
                                                            <table width="99.5%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                                                border-left: 1px solid gray; border-right: 1px solid gray; margin-left: 2px;">
                                                                <div id="DIV_OTROS" runat="server">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <h5>
                                                                                <asp:Label ID="Label19" runat="server" Text="Otros Datos"></asp:Label>
                                                                            </h5>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="5">
                                                                            <hr width="96%" align="center" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="3" align="left" style="background-color: #F5F5F5;">
                                                                            <asp:Label ID="LblInfoMigratoria" runat="server" Font-Bold="true" Text="Discapacidad" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="2" align="left">
                                                                            <asp:Label ID="Label20" runat="server" Text="¿Desea que su DNI muestre condición de Discapacidad?" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rbSi_Discapacidad" Text="Si" GroupName="Discapacidad" runat="server" />
                                                                            <asp:RadioButton ID="rbNO_Discapacidad" Text="No" GroupName="Discapacidad" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="3" align="left" style="background-color: #F5F5F5;">
                                                                            <asp:Label ID="Label21" runat="server" Font-Bold="true" Text="Interdicción" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="2" align="left">
                                                                            <asp:Label ID="Label22" runat="server" Text="¿Desea que su DNI muestre condición de Interdicto?" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rbSi_Interdicto" Text="Si" GroupName="Interdicto" runat="server" />
                                                                            <asp:RadioButton ID="rbNO_Interdicto" Text="No" GroupName="Interdicto" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="2" align="left">
                                                                            <asp:Label ID="Label25" runat="server" Text="Nombre Curador: " />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCurador" style="text-transform:uppercase;" runat="server" 
                                                                                Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="3" align="left" style="background-color: #F5F5F5;">
                                                                            <asp:Label ID="Label23" runat="server" Font-Bold="true" Text="Donación de Órganos" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td colspan="2" align="left">
                                                                            <asp:Label ID="Label24" runat="server" Text="¿Acepta donar órganos?" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rbSi_Donador" Text="Si" GroupName="Donador" runat="server" />
                                                                            <asp:RadioButton ID="rbNO_Donador" Text="No" GroupName="Donador" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </div>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" align"center">
                                                                        <asp:Label ID="lblNota" Visible="False" runat="server" 
                                                                            Text="(*) SI DESEA CONSIGNAR EL APELLIDO DE CASADA ES REQUISITO ADJUNTAR EL ACTA DE MATRIMONIO." 
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" align="center">
                                                                        <asp:Button ID="btnGrabarParticipante" runat="server" CssClass="btnSave" Width="180px"
                                                                            Text="       Adicionar participante" OnClick="btnNuevoParticipante_Click" TabIndex="518"
                                                                            Visible="false" />
                                                                        <asp:Button ID="btnEditarParticipante" runat="server" CssClass="btnSave" Width="110px"
                                                                            OnClick="btnEditarParticipante_Click" Text="  Grabar" TabIndex="519" />
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:Button ID="btnCancelarParticipante" runat="server" CssClass="btnUndo" Width="110px"
                                                                            Text="     Limpiar" OnClick="btnCancelarParticipante_Click" TabIndex="520" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:HiddenField ID="HFDep" runat="server" />
                                                                        <asp:HiddenField ID="HFProv" runat="server" />
                                                                        <asp:HiddenField ID="HFdist" runat="server" />
                                                                        <asp:HiddenField ID="HFLNDep" runat="server" />
                                                                        <asp:HiddenField ID="HFLNProv" runat="server" />
                                                                        <asp:HiddenField ID="HFLNdist" runat="server" />
                                                                        <asp:HiddenField ID="hfPersona" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFParticipanteId" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFpere_iResidenciaId" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFFechaNacimiento" runat="server" />
                                                                        <asp:HiddenField ID="HFLugarNacimiento" runat="server" />
                                                                        <asp:HiddenField ID="HF_BusquedaParticipante" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFValidarTexto" runat="server" />
                                                                        <asp:HiddenField ID="HFFichaRegistralId" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFFichaTipoParticipanteId" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="HFNumeroFichaRegistral" runat="server" Value="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="modalPregunta" class="modal">
                                        <div class="modal-window">
                                            <div class="modal-titulo">
                                                <asp:ImageButton ID="ImageButton1" CssClass="close" ImageUrl="~/Images/close.png"
                                                    OnClientClick="cerrarPopupPregunta(); return false" runat="server" />
                                                <span>CONFIRMACIÓN</span>
                                            </div>
                                            <div class="modal-cuerpo">
                                                <br />
                                                <asp:Label ID="lblTitular" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                <asp:Label ID="lbl" runat="server" Text="ES EL TITULAR DEL DOCUMENTO?" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="modal-pie">
                                                <asp:Button ID="btnAceptarTitular" runat="server" CssClass="btnLogin" Text="SI" OnClick="btnAceptarTitular_Click" />
                                                <asp:Button ID="btnCancelarTitular" runat="server" CssClass="btnLogin" Text="NO"
                                                    OnClientClick="cerrarPopupPregunta(); return false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div id="modalDocAdjuntos" class="modal">
                                        <div class="modal-windowlarge">
                                            <div class="modal-titulo">
                                                <asp:ImageButton ID="ImgCerrarPopupAdjuntos" CssClass="close" ImageUrl="~/Images/close.png"
                                                    OnClientClick="cerrarPopupDocAdjuntos();return false;" runat="server" />
                                                <span>DOC. ADJUNTOS</span>
                                            </div>
                                            <div class="modal-cuerpoNormal">
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <table width="99%">
                                                        <tr>
                                                            <td align="left">
                                                                <h5>
                                                                    <asp:Label ID="Label27" runat="server" Text="Documentos Adjuntos"></asp:Label>
                                                                </h5>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <table width="99.5%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                                                border-left: 1px solid gray; border-right: 1px solid gray; margin-left: 2px;">
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label29" runat="server" Text="Documento Adjunto:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 220px;">
                                                                        <asp:DropDownList ID="ddlDocAdjunto" runat="server" Style="cursor: pointer;" Height="21px"
                                                                            Width="200px" TabIndex="500" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label31" runat="server" Text="Número:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNumeroDocAdj" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnAgregarDocAdj" runat="server" CssClass="btnSave" Width="100px"
                                                                            Text="       Agregar" onclick="btnAgregarDocAdj_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <hr />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <h5>
                                                    <asp:Label ID="lblListaParticipantes" runat="server" Text="Lista Participantes"></asp:Label>
                                                </h5>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel ID="updGrilla" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridViewParticipante" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                AutoGenerateColumns="False" GridLines="None" 
                                                OnRowCommand="GridViewParticipante_RowCommand" 
                                                onrowdatabound="GridViewParticipante_RowDataBound">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFichaRegistralParticipanteId" runat="server" Text='<%# Bind("IPARTICIPANTEID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFichaTipoParticipanteId" runat="server" Text='<%# Bind("ITIPO_PARTICIPANTEID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="INUMERO" HeaderText="Nro.">
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VAPELLIDOS_NOMBRES" HeaderText="Apellidos y Nombres">
                                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VTIPO_PARTICIPANTE" HeaderText="Tipo de Participante">
                                                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VNUMERO_DOCUMENTO" HeaderText="Nro. Documento">
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Editar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" runat="server" CommandName="Editar" ImageUrl="../Images/img_16_edit.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Anular">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnAnular" runat="server" CommandName="Anular" ImageUrl="../Images/img_grid_delete.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <h5>
                                                    <asp:Label ID="lblDocAdjuntos" runat="server" Text="Lista de Documentos que se adjuntan al Trámite"></asp:Label>
                                                </h5>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grvDocAdjuntosReniec" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                        AutoGenerateColumns="False" GridLines="None" 
                                        OnRowCommand="grvDocAdjuntosReniec_RowCommand" 
                                        onrowdatabound="grvDocAdjuntosReniec_RowDataBound">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_iFichaRegistralDocumentoID" runat="server" Text='<%# Bind("fido_iFichaRegistralDocumentoID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_iCodDocumento" runat="server" Text='<%# Bind("COD_REAL_DOCUMENTO_FICHA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOCADJUNTO" runat="server" Text='<%# Bind("fido_sDocumentoID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNUMERODOCADJUNTO" runat="server" Text='<%# Bind("NUMERODOCADJUNTO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DOCADJUNTO" HeaderText="Descripción">
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NUMERODOCADJUNTO" HeaderText="Número">
                                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Editar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEditar" runat="server" CommandName="Editar" ImageUrl="../Images/img_16_edit.png" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Anular">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnAnular" runat="server" CommandName="Anular" ImageUrl="../Images/img_grid_delete.png" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-7">
                            <asp:UpdatePanel ID="updAntecedente" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div style="border: 1px solid #dcdcdc;">
                                        <asp:HiddenField ID="hanpe_iAntecedentePenalId" runat="server" />
                                        <table width="100%">
                                            <tr>
                                                
                                                <td style="width:150px">
                                                    <asp:Button ID="btnGrabarAntecedente" runat="server" Text="   Grabar" CssClass="btnSave"
                                                        OnClick="btnGrabarAntecedente_Click" Width="120px" />
                                                </td>
                                                <td id="commandDelete">
                                                    <asp:Button ID="btnAnularAntecedente" runat="server" Text="   Anular" OnClientClick="if ( !deleteConfirmation()) return false;"
                                                        CssClass="btnDelete" OnClick="btnAnularAntecedente_Click" Width="120px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblFechaSolicitudMSIAP" runat="server" Text="Fecha de Solicitud MSIAP:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="ctrFechaSolicitudMSIAP" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblUsuarioAutorizadoMSIAP" runat="server" Text="Usuario autorizado:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlUsuarioAutorizadoMSIAP" runat="server" Width="350px" Style="cursor: pointer"
                                                        TabIndex="500">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblNumeroSolicitudWebMSIAP" runat="server" Text="N&uacute;mero Solicitud WEB:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNumeroSolicutdWebMSIAP" runat="server" MaxLength="30" Width="150px"
                                                        TabIndex="501"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblOficioRespuestaMSIAP" runat="server" Text="Oficio de Respuesta:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOficioRespuestaMSIAP" runat="server" MaxLength="30" CssClass="txtLetra"
                                                        Width="150px" TabIndex="502"></asp:TextBox>
                                                </td>
                                                <td style="width:160px;">
                                                    <asp:Label ID="lblFechaRespuestaMSIAP" runat="server" Text="Fecha de respuesta MSIAP:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="ctrFechaRespuestaMSIAP" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblObservacionMSIAP" runat="server" Text="Observaci&oacute;n:"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="3" align="left">
                                                    <asp:TextBox ID="txtObservacionMSIAP" runat="server" MaxLength="500" CssClass="txtLetra"
                                                        Width="90%" Height="88px" TextMode="MultiLine" TabIndex="503"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblSolicitaParaMSIAP" runat="server" Text="Solicita para:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                     <asp:DropDownList ID="ddlTramiteMSIAP" runat="server" Width="350px" Style="cursor: pointer"
                                                            TabIndex="504">
                                                        </asp:DropDownList>
                                                        <span class="asterisco">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width:190px">
                                                     
                                                </td>
                                                <td align="left" style="width:220px">
                                                    <asp:Label ID="lblRegistraAntecedentePenales" runat="server" 
                                                        Text="¿Registrar Antecedentes Penales?" 
                                                        style="font-weight:bold; color:black; border-color:Red; padding:5px; border-style: solid; border-width:thin;" 
                                                        Width="200px"></asp:Label>
                                                </td>
                                                <td align="left">                                                                                                                                                             
                                                    <asp:Button ID="btnNoAntecedentes" runat="server" Text="NO" Height="30px" Width="50px" 
                                                    CssClass="btnGeneral" OnClientClick="EnableTabIndex(5); MoveTabIndex(6);"/>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnRegistrarAntecedentesPenales" runat="server" 
                                                        Text="SI" CssClass="btnGeneral" Height="30px" 
                                                        OnClientClick="abrirPopupAntecedentesPenales(); return false"
                                                        Width="50px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                           
                                           <table>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <h5>
                                                                <asp:Label ID="lblTituloAntecedentesPenalesDetalle" runat="server" Text="DETALLE DE ANTECEDENTES PENALES"></asp:Label>
                                                            </h5>
                                                        </td>
                                                    </tr>
                                                </table>                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <table width="100%">
                                                <tr>
                                                <td>
                                               
                                                 <asp:GridView ID="gdvAntecedentesPenales" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvAntecedentesPenales_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAntecedentePenalDetalleId" runat="server" Text='<%# Bind("apde_iAntecedentePenalDetalleId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderText="Nro.">
                                                           <HeaderStyle Width="50px"  HorizontalAlign="Center"/>
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="apde_vOrganoJurisdiccional" HeaderText="Organo Jurisdiccional">
                                                            <HeaderStyle Width="250px" HorizontalAlign="Left"/>
                                                            <ItemStyle Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="apde_vNumeroExpediente" HeaderText="Número de Expediente">
                                                            <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                            <ItemStyle Width="100px" HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="apde_dFechaSentencia" HeaderText="Fecha de Sentencia" DataFormatString="{0:dd-MM-yyyy}">
                                                            <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                                                            <ItemStyle Width="90px" HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="apde_vDelito" HeaderText="Delito">
                                                            <HeaderStyle Width="250px" HorizontalAlign="Left"/>
                                                            <ItemStyle Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="apde_dFechaVencimiento" HeaderText="Fecha de Vencimiento" DataFormatString="{0:dd-MM-yyyy}">
                                                            <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                                                            <ItemStyle Width="90px" HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnAnular" CommandName="Anular" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                               
                                                   

                                                <uc2:ctrlPageBar ID="CtrlPaginadorAntecedentesPenales" runat="server" OnClick="CtrlPaginadorAntecedentesPenales_Click" />
                                                </td>
                                            </tr>
                                        </table>                                                
                                                </td>
                                            </tr>
                                           </table>
                                                                                                                    
                                    </div>

  

<div id="modalAntecedentesPenales" class="modal">
    <div class="modal-window">
        <div class="modal-titulo">
            <asp:ImageButton ID="imgCerrarPopupAntecedentesPenales" CssClass="close" ImageUrl="~/Images/close.png"
                     OnClientClick="cerrarPopupAntecedentesPenales();return false" runat="server" />
            <span>Registro de Antecedentes Penales</span>
        </div>
        <div class="modal-cuerpoNormalAntecedentes">
            <asp:Panel ID="pnlAntecedentesPenales" runat="server">
                      <asp:UpdatePanel ID="updAntecedentesPenales" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="HF_NroExpediente" runat="server" />
                    <table width="99%">                                            
                                            <tr>
                                                <td colspan="4">
                                                    <table width="100%" style="border-color:Black;border-width:1px; border-style:solid">
                                                <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblOrganoJurisdiccionalMSIAP" runat="server" 
                                                        Text="Organo Jurisdiccional:" Width="120px"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                     <asp:TextBox ID="txtOrganoJurisdiccionalMSIAP" runat="server" MaxLength="200"  CssClass="txtLetra"
                                                                                Width="90%" TabIndex="505" />
                                                    <span class="asterisco">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblNumeroExpediente" runat="server" Text="Número de Expediente:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                     <asp:TextBox ID="txtNumeroExpedienteMSIAP" runat="server" MaxLength="20"  CssClass="txtLetra"
                                                                                TabIndex="506" Width="50%" />
                                                    <span class="asterisco">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblFechaSentenciaMSIAP" runat="server" Text="Fecha de Sentencia:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <SGAC_Fecha:ctrlDate ID="txtFechaSentenciaMSIAP" runat="server" />
                                                    <span class="asterisco">*</span>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDelitoMSIAP" runat="server" Text="Delito:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                     <asp:TextBox ID="txtDelitoMSIAP" runat="server" MaxLength="200"  CssClass="txtLetra"
                                                                                Width="90%" TabIndex="507" />
                                                    <span class="asterisco">*</span>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblFechaVencimientoMSIAP" runat="server" Text="Fecha de Vencimiento:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <SGAC_Fecha:ctrlDate ID="txtFechaVencimientoMSIAP" runat="server" />                                                    
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDuracionPenaMSIAP" runat="server" Text="Duración de la Pena:"></asp:Label>
                                                </td>
                                                <td colspan="3" style="width:100%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width:140px">
                                                                    <asp:TextBox ID="txtDuracionAniosMSIAP" runat="server" MaxLength="4" onClick="this.select()"
                                                                     CssClass="campoNumero"   Width="30px" TabIndex="508" 
                                                                     onkeypress="return isNumberKey(event)"/>
                                                                    <asp:TextBox ID="txtDuracionMesesMSIAP" runat="server" MaxLength="2" onClick="this.select()"
                                                                     CssClass="campoNumero"   Width="30px" TabIndex="509" 
                                                                     onkeypress="return isNumberKey(event)"/>
                                                                    <asp:TextBox ID="txtDuracionDiasMSIAP" runat="server" MaxLength="2" onClick="this.select()"
                                                                     CssClass="campoNumero"   Width="30px" TabIndex="510" 
                                                                     onkeypress="return isNumberKey(event)"/>
                                                                    
                                                            </td>
                                                            <td>
                                                                    <asp:Label ID="Label8" runat="server" Text="(Ingrese el número de años, meses, días)"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                    <asp:Label ID="Label9" runat="server" Text="años / meses / días "></asp:Label>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblTipoPenaMSIAP" runat="server" Text="Tipo de Pena:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtTipoPena" runat="server" MaxLength="200" Width="90%" CssClass="txtLetra"
                                                        TabIndex="512" />
                                                    <span class="asterisco">*</span>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td colspan="4">
                                                      <asp:Button ID="btnAgregarAntecedentePenal" runat="server" Text="   Agregar"
                                                        Width="120px" CssClass="btnSave" TabIndex="494" 
                                                          onclick="btnAgregarAntecedentePenal_Click" />

                                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <input id="btnLimpiarAntecedentePenal" type="button" value="     Limpiar"  class="btnLimpiar" onclick="LimpiarAntecedentePenalDetalle()"/>

                                                </td>
                                            </tr>
                                                     <tr>
                                                        <td colspan="4">
                                                        </td>
                                                     </tr>   
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                        </table>   
                        </ContentTemplate>                                              
                    </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
</div>



          

                                </ContentTemplate>
                                                                
                                
                            </asp:UpdatePanel>


                        </div>
                        <div id="tab-4">
                            <asp:UpdatePanel ID="updVinculacion" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btn_acta_diligenciamiento" />
                                </Triggers>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Button Text="    Grabar" CssClass="btnSave" runat="server" ID="btnGrabarVinculacion"
                                                    OnClick="btnGrabarVinculacion_Click" />
                                                <uc1:ctrlReimprimirbtn ID="ctrlReimprimirbtn1" runat="server" />
                                                <uc5:ctrlBajaAutoadhesivo ID="ctrlBajaAutoadhesivo1" runat="server" />
                                                <asp:Button ID="btnRegistroSUNARP" runat="server" Text="      SUNARP" 
                                                    CssClass="btnGeneralConImagen" onclick="btnRegistroSUNARP_Click" 
                                                    Visible="False"/>                                                

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblValidacionDetalle" runat="server" Text="Validar Código del Autoadhesivo"
                                                    CssClass="hideControl" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center; width: 300px;">
                                                            <h3>
                                                                <asp:Label ID="Label17" runat="server" Text="PASO 1: Vinculación Autoadhesivo"></asp:Label></h3>
                                                        </td>
                                                        <td colspan="2" style="text-align: center; width: 300px;">
                                                            <h3>
                                                                <asp:Label ID="Label15" runat="server" Text="PASO 2: Vista Previa e Impresión"></asp:Label></h3>
                                                        </td>
                                                        <td colspan="2" style="text-align: center; width: 300px;">
                                                            <h3>
                                                                <asp:Label ID="Label16" runat="server" Text="PASO 3: Aprobación Impresión"></asp:Label></h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text="Código:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCodAutoadhesivo" runat="server" Width="120px" MaxLength="20"
                                                                CssClass="txtLetra"
                                                                TabIndex="1"></asp:TextBox>
                                                        </td>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Button ID="btnVistaPrev" runat="server" Text="   Autoadhesivo" Width="270px"
                                                                CssClass="btnPrint" TabIndex="50" OnClick="btnVistaPrev_Click" 
                                                                Enabled="False" />
                                                             <asp:Button ID="btnConstanciaInscripcion" runat="server" 
                                                                Text="   Constancia de Inscripción" Width="270px"
                                                                CssClass="btnPrint" TabIndex="50"
                                                                Visible="False" OnClientClick="abrirModalIdioma();return false" />
                                                        </td>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:CheckBox ID="chkImpresion" runat="server" Text="Impresión Correcta" Enabled="False"
                                                                OnCheckedChanged="chkImpresion_CheckedChanged" AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div style="margin-left: 25%">
                                                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btnLimpiar" OnClick="btnLimpiar_Click" />
                                                            </div>
                                                        </td>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Button ID="btn_acta_diligenciamiento" runat="server" Text=" Acta de Diligenciamiento"
                                                                TabIndex="50" Visible="false" CssClass="btnPrintVistaPrevia" 
                                                                OnClick="btn_acta_diligenciamiento_Click" Width="270px" />
                                                            <br />                                                                                                                        
                                                            <asp:Button ID="btnDesabilitarAutoahesivo" runat="server" OnClick="btnDesabilitarAutoahesivo_Click"
                                                                Text="Oculto" Visible="True" CssClass="hideControl" />
                                                            <asp:HiddenField ID="hnd_ImpresionCorrecta" Value="0" runat="server" />
                                                            <asp:HiddenField ID="hDetActuacion" runat="server" />
                                                            <asp:HiddenField ID="hCodAutoadhesivo" runat="server" />
                                                            <asp:HiddenField ID="HF_RutaFoto" runat="server" />
                                                            <table id="BotonesAntecedentesPenalesId" runat="server" visible="false">
                                                                <tr>
                                                                    <td>
                                                                            <asp:Button ID="btnImpresionCertificadoAntecedentesPenales" runat="server" Text="   Certificado de Antecedentes Penales" 
                                                                                CssClass="btnPrint" 
                                                                                onclick="btnImpresionCertificadoAntecedentesPenales_Click" Width="270px"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btn_ImprimirConformidad" runat="server" Text="   Acta de Conformidad" 
                                                                         Width="270px" CssClass="btnPrint" TabIndex="9" OnClick="btn_ImprimirConformidad_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td colspan="2" valign="top" align="center">
                                                        <table id="tablaFileUpLoadFoto" runat="server" width="100%" style="text-align:center" visible="false">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAdjuntarFoto" runat="server" Text="¿Adjuntar archivo de foto?" style="font-weight:bold; color:Black"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%--<asp:FileUpload ID="FileUploadFoto" runat="server" onchange="javascript:try{submit();}catch(err){}" Width="230px"></asp:FileUpload>  --%>
                                                                    <asp:FileUpload ID="FileUploadFoto" runat="server" onchange="postBack();" Width="230px"></asp:FileUpload>  
                                                                    <%--<asp:FileUpload ID="FileUploadFoto" runat="server" Width="230px"></asp:FileUpload>  --%>
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblMensajeFileUploadFoto" runat="server" style="color:Black"
                                                                    Text="elija imagenes (*.jpg) hasta 18kb"></asp:Label>        
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblMensajeErrorUpLoadFoto" runat="server" style="color:Red" Text=""></asp:Label>        
                                                                </td>
                                                            </tr>
                                                        </table>
                                                                                                                                                                                                                               
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <asp:GridView ID="Grd_ActInsDet" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                AutoGenerateColumns="False" GridLines="None">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="idActuacionInsumoDetalle" Visible="False">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("aide_iActuacionInsumoDetalleId") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("aide_iActuacionInsumoDetalleId") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
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
                                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="USUARIO" HeaderText="Usuario">
                                                                        <ItemStyle Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="insu_sInsumoTipoId" HeaderText="Tipo Insumo">
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="aide_bFlagImpresion" HeaderText="Impresión Correcta">
                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="insu_vCodigoUnicoFabrica" HeaderText="Código de Insumo">
                                                                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CantidadImpresiones" HeaderText="Cantidad Impresiones">
                                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td colspan="2">
                                                            <uc4:ctrlCargando ID="ctrlCargando1" Visible="false" runat="server"></uc4:ctrlCargando>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <uc2:ctrlPageBar ID="CtrlPageBarActuacionInsumoDetalle" runat="server" OnClick="ctrlPagActuacionInsumoDetalle_Click"
                                                                Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
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
                                                <h3 style="font-size: small">
                                                    Procesando la información. Por favor espere...</h3>
                                                <asp:Image ID="imgEspera" ImageUrl="../Images/espera.gif" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="ModalIdioma" class="modal">
                            <div class="modal-windowsmall">
                                <div class="modal-titulo">
                                    <asp:ImageButton ID="ImageButton3" CssClass="close" ImageUrl="~/Images/close.png"
                                                    OnClientClick="CerrarModalIdioma();return false" runat="server" />
                                    <span>Seleccione el Idioma de Impresión</span>
                                </div>
                                <div class="modal-cuerpo">
                                    <br />
                                    <asp:DropDownList ID="ddlIdiomas" runat="server">
                                    <asp:ListItem Value="1" Text="Español"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Ingles"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnImprimir" CssClass="btnPrint" runat="server" Text="  Imprimir" OnClick="ImprimirConstancia" />
                                </div>
                            </div>
                        </div>
                        <div id="modalPreviImpresión" class="modal">
                            <div class="modal-windowsmall">
                                <div class="modal-titulo">
                                    <asp:ImageButton ID="ImageButton2" CssClass="close" ImageUrl="~/Images/close.png"
                                                    OnClientClick="CerrarPopupImpresionPrevia();return false" runat="server" />
                                    <span>Seleccione los campos a imprimir</span>
                                </div>
                                <div class="modal-cuerpo">
                                    <br />
                                    <div style="text-align:left; padding:15px;">
                                        <asp:CheckBox ID="chkSeleccionarTodo" onclick="marcar(this);" runat="server" Checked="true" Text="Seleccionar Todos"/> 
                                        <hr />
                                    </div>
                                    <div id="checkbox" style="text-align:left; padding-left:15px;">
                                        <asp:CheckBox ID="chkModLugDomicilio"  runat="server" Checked="true" Text="Lugar de Domicilio"/> <br />
                                        <asp:CheckBox ID="chkModDireccion"  runat="server" Checked="true" Text="Dirección"/> <br />
                                        <asp:CheckBox ID="chkModEstCivil"  runat="server" Checked="true" Text="Estado Civil"/> <br />
                                        <asp:CheckBox ID="chkModGradoInstr"  runat="server" Checked="true" Text="Grado de Instrucción"/> <br />
                                        <asp:CheckBox ID="chkModEstatura"  runat="server" Checked="true" Text="Estatura" /> <br />
                                        <asp:CheckBox ID="chkModGenero" runat="server" Checked="true" Text="Género" /> <br />
                                        <asp:CheckBox ID="chkModDiscapacidad"  runat="server" Checked="true" Text="Discapacidad" /> <br />
                                        <asp:CheckBox ID="chkModInterdiccion"  runat="server" Checked="true" Text="Interdicción" /> <br />
                                        <asp:CheckBox ID="chkModObservacion"  runat="server" Checked="true" Text="Observaciones y/o señas particulares" /> <br />
                                        <asp:CheckBox ID="chkDonaOrganos"  runat="server" Checked="true" Text="Dona Organos" /><br />
                                        <asp:CheckBox ID="chkModFecNac"  runat="server" Checked="true" Text="Fecha de Nacimiento" /> <br />
                                        <asp:CheckBox ID="chkModLugNac"  runat="server" Checked="true" Text="Lugar de Nacimiento" /><br />
                                        <asp:CheckBox ID="chkModTelefono"  runat="server" Checked="true" Text="Telefono" /><br />
                                        <asp:CheckBox ID="chkModEmail"  runat="server" Checked="true" Text="Email" />
                                        
                                    </div>
                                    
                                    <hr />
                                    <asp:Button ID="BtnAceptar" runat="server"
                                                Text="     Imprimir" CssClass="btnPrint" 
                                        onclick="BtnAceptar_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" Value="" runat="server" />
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
            $('#tabs').enableTab(6);
            $('#tabs').disableTab(1);
            $('#tabs').disableTab(2);
            $('#tabs').disableTab(3);
            $('#tabs').disableTab(4);
            $('#tabs').disableTab(5);
        });

        $("#imgBuscarDocumento").on('click', function () {
            Documento_OnClick();
        });

        function marcar(source) {
            
            var checkboxes = document.getElementById('checkbox').getElementsByTagName('input');
            
            for (i = 0; i < checkboxes.length; i++) //recoremos todos los controles
            {
                if (checkboxes[i].type == "checkbox") //solo si es un checkbox entramos
                {
                    checkboxes[i].checked = source.checked; //si es un checkbox le damos el valor del checkbox que lo llamó (Marcar/Desmarcar Todos)
                }
            }
        }
                   

        function custom_alert(output_msg, title_msg) {
            if (!title_msg)
                title_msg = 'Alert';

            if (!output_msg)
                output_msg = 'No Message to Display.';

            $("<div></div>").html("<table><tr><td><img src='../images/img_msg_warning.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" +
            output_msg + "</td></tr></table>").dialog({
                title: title_msg,
                resizable: false,
                modal: true,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function ValidarRegistroActuacion() {
            var bolValida = true;

            var strTipoPago = $.trim($("#<%= ddlTipoPago.ClientID %>").val());
            var cmb_TipoPago = document.getElementById('<%= ddlTipoPago.ClientID %>');

            var str_HF_PAGADO_EN_LIMA = $("#<%=HF_PAGADO_EN_LIMA.ClientID %>").val();
            var str_HF_DEPOSITO_CUENTA = $("#<%=HF_DEPOSITO_CUENTA.ClientID %>").val();
            var str_HF_TRANSFERENCIA = $("#<%=HF_TRANSFERENCIA.ClientID %>").val();


            if (strTipoPago == "0") {
                if (cmb_TipoPago != null)
                    cmb_TipoPago.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                if (cmb_TipoPago != null)
                    cmb_TipoPago.style.border = "1px solid #888888";

                /* PARAMETROS: PAGADO EN LIMA - 3501 */
                if (strTipoPago == str_HF_PAGADO_EN_LIMA ||
                    strTipoPago == str_HF_DEPOSITO_CUENTA ||
                    strTipoPago == str_HF_TRANSFERENCIA) {
                    var strNroOperacion = $.trim($("#<%= txtNroOperacion.ClientID %>").val());
                    var strNombreBanco = $.trim($("#<%= ddlNomBanco.ClientID %>").val());
                    var strMontoCancelado = $.trim($("#<%= txtMtoCancelado.ClientID %>").val());
                    var strFecha = $.trim($('#<%= ctrFecPago.FindControl("TxtFecha").ClientID %>').val());

                    var txtNroOperacion = document.getElementById('<%= txtNroOperacion.ClientID %>');
                    var ddlNomBanco = document.getElementById('<%= ddlNomBanco.ClientID %>');
                    var txtMtoCancelado = document.getElementById('<%= txtMtoCancelado.ClientID %>');
                    var txtFecha = document.getElementById('<%= ctrFecPago.FindControl("TxtFecha").ClientID %>');

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


        function ValidarAutoadhesivo() {
            var bolValida = true;
            // debugger;
            var strCodigo = $.trim($("#<%= txtCodAutoadhesivo.ClientID %>").val());

            //         var chkImpresion = $("#<%= chkImpresion.ClientID %>").prop("checked") ? true : false;

            if (strCodigo.length < 12) {
                bolValida = false;
            }

            //         if (chkImpresion==false) {
            //             bolValida = false;
            //         }

            if (bolValida) {
                $("#<%= lblValidacionDetalle.ClientID %>").hide();
                abrirPopupEspera();
            }
            else {
                $("#<%= lblValidacionDetalle.ClientID %>").show();
            }
            return bolValida;
        }




        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            //            $('#tabs').tabs("disable", iTab);
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }



        function Documento_OnClick() {

            var ddl_TipoDocParticipante = document.getElementById('<%= ddl_TipoDocParticipante.ClientID %>');
            var ddl_TipoParticipante = document.getElementById('<%= ddl_TipoParticipante.ClientID %>');
            var HF_BusquedaParticipante = $("#<%=HF_BusquedaParticipante.ClientID %>").val();

            var bolValida = true;

            var prm = {};
            prm.tipo = $("#<%= ddl_TipoDocParticipante.ClientID %>").val();
            prm.documento = $("#<%= txtNroDocParticipante.ClientID %>").val();
            prm.TipoParticipante = $("#<%= ddl_TipoParticipante.ClientID %>").val();


            if (prm.TipoParticipante == 0) {
                ddl_TipoParticipante.style.border = "1px solid Red";
                ddl_TipoParticipante.focus();
                bolValida = false;
            }
            else {
                ddl_TipoDocParticipante.style.border = "1px solid #888888";
            }

            if (prm.tipo == 0) {
                ddl_TipoDocParticipante.style.border = "1px solid Red";
                if (prm.TipoParticipante == 0) {
                    ddl_TipoParticipante.focus();
                }
                else {
                    ddl_TipoDocParticipante.focus();
                }
                bolValida = false;
            }
            else {
                ddl_TipoDocParticipante.style.border = "1px solid #888888";
            }
            if (bolValida) {

                if (prm.documento != "") {
                    $.ajax({
                        type: "POST",
                        url: "FrmActoGeneral.aspx/GetPersonExist",
                        data: JSON.stringify(prm),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnSuccess,
                        failure: function (response) {

                            alert(response.d);
                        }
                    });

                }
            }
            desabilitarBotonesEditar();
        }

        function ValidarNumeroDocumento(event) {


            var CmbTipoDoc = $("#MainContent_ddl_TipoDocParticipante").val();

            if (CmbTipoDoc == 0)
                return false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

            document.getElementById("MainContent_txtNroDocParticipante").maxLength = valoresDocumento[1];

            if (valoresDocumento[2] == "True") {
                bol = OnlyNumeros(event);
            }
            else {
                bol = isNumeroLetra(event);
            }

            desabilitarBotonesEditar();
            return bol;
        }

        function ObtenerMaxLenghtDocumentos(id) {

            var bEsCui = false;

            if (id == 6) {
                id = 1;
                bEsCui = true;

            }


            var hfDocumentoIdentidad = $("#<%=HF_ValoresDocumentoIdentidad.ClientID %>").val();

            var documentos = hfDocumentoIdentidad.split("|");

            for (i = 0; i < documentos.length - 1; i++) {

                var valores = documentos[i].split(",");

                if (bEsCui)
                    valores[5] = valores[5].replace('DNI', 'CUI');

                if (valores[0] == id) {
                    return valores[1] + "," + valores[2] + "," + valores[3] + "," + valores[5] + "," + valores[4];
                }

            }

            return "";

        }


        function OnlyNumeros(evt) {

            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                status = "This field accepts numbers only."
                return false
            }
            status = ""
            return true

        }

        function isNumeroLetra(evt) {
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
            if (charCode == 45) {
                letra = true;
            }

            if (charCode > 47 && charCode < 58) {
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

            var letras = "áéíóúñÑ'";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            letras = "¡";
            tecla = String.fromCharCode(charCode);
            n = letras.indexOf(tecla);
            if (n > -1) {
                letra = false;
            }

            return letra;
        }
        function imgBuscarDocumento_onclick() {

        }
        function TipoDocSelectedChange() {

            var strTipoDoc = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());
            if (strTipoDoc > 0) {
                $("#<%= txtNroDocParticipante.ClientID %>").focus();
            }
            desabilitarBotonesEditar();
        }

        function OnSuccess(response) {
            var objPerson = $.parseJSON(response.d);

            if (objPerson.iPersonaId != 0) {

                $("#<%= txtNomParticipante.ClientID %>").val(objPerson.vNombres);
                $("#<%= txtApePatParticipante.ClientID %>").val(objPerson.vApellidoPaterno);
                $("#<%= txtApeMatParticipante.ClientID %>").val(objPerson.vApellidoMaterno);
                $('#<%= txtApeCasadaParticipante.ClientID %>').val(objPerson.vApellidoCasada);

                //-----------------------------------------------------------
                // Datos de Nacimiento
                //-----------------------------------------------------------
                var dFecNac = objPerson.dFecNacimiento;
                var LugarNacimiento = objPerson.cNacimientoLugar;
                if (dFecNac.length != 0 && dFecNac != "0") {
                    dFecNac = dFecNac.substring(0, 11);
                    $('#<%= txtFechaNacimiento.FindControl("TxtFecha").ClientID %>').val(dFecNac);
                }
                $("#<%= TxtSenasParticulares.ClientID %>").val(objPerson.vSenasculares);
                //-----------------------------------------------------------
                // Datos de Residencia
                //-----------------------------------------------------------
                $("#<%= TxtDirDir.ClientID %>").val(objPerson.vDireccion);
                $("#<%= TxtTelfDir.ClientID %>").val(objPerson.vTelefono);


                var iDptoContId = objPerson.iDptoContId.toString();
                var iProvPaisId = objPerson.iProvPaisId.toString();
                var iDistCiuId = objPerson.iDistCiuId.toString();
                var pere_iResidenciaId = objPerson.acpa_iResidenciaId;


                $("#<%= HFDep.ClientID %>").val(iDptoContId);
                $("#<%= HFProv.ClientID %>").val(iProvPaisId);
                $("#<%= HFdist.ClientID %>").val(iDistCiuId);

                if (pere_iResidenciaId > 0) {
                    $("#<%= HFpere_iResidenciaId.ClientID %>").val(pere_iResidenciaId);
                }
                else {
                    $("#<%= HFpere_iResidenciaId.ClientID %>").val('0');
                }

                var cNacimientoLugar = objPerson.cNacimientoLugar;
                if (cNacimientoLugar == "0") {
                    $("#<%= HFLNDep.ClientID %>").val(0);
                    $("#<%= HFLNProv.ClientID %>").val(0);
                    $("#<%= HFLNdist.ClientID %>").val(0);
                }
                else {
                    var LNDptoContId = cNacimientoLugar.substring(0, 2);
                    var LNProvId = cNacimientoLugar.substring(2, 4);
                    var LNDistId = cNacimientoLugar.substring(4);

                    $("#<%= HFLNDep.ClientID %>").val(LNDptoContId);
                    $("#<%= HFLNProv.ClientID %>").val(LNProvId);
                    $("#<%= HFLNdist.ClientID %>").val(LNDistId);
                }


                //-----------------------------------------------------------

                $("#<%= hfPersona.ClientID %>").val(objPerson.iPersonaId);


                var btn = document.getElementById('<%= btnBuscarParticipante.ClientID %>');
                btn.click();

            }
            else {
                //             $("#<%= txtNomParticipante.ClientID %>").attr('disabled', false);
                //             $("#<%= txtApePatParticipante.ClientID %>").attr('disabled', false);
                //             $("#<%= txtApeMatParticipante.ClientID %>").attr('disabled', false);
                //             $("#<%= TxtDirDir.ClientID %>").attr('disabled', false);
                //             $("#<%= TxtSenasParticulares.ClientID %>").attr('disabled', false);
                //             $("#<%= TxtTelfDir.ClientID %>").attr('disabled', false);

                $("#<%= txtNomParticipante.ClientID %>").val('');
                $("#<%= txtApePatParticipante.ClientID %>").val('');
                $("#<%= txtApeMatParticipante.ClientID %>").val('');

                $("#<%= TxtDirDir.ClientID %>").val('');
                $("#<%= TxtTelfDir.ClientID %>").val('');

                $("#<%= hfPersona.ClientID %>").val('0');
                $("#<%= HFDep.ClientID %>").val('0');
                $("#<%= HFProv.ClientID %>").val('0');
                $("#<%= HFdist.ClientID %>").val('0');

                $("#<%= HFLNDep.ClientID %>").val('0');
                $("#<%= HFLNProv.ClientID %>").val('0');
                $("#<%= HFLNdist.ClientID %>").val('0');

                $("#<%= HFpere_iResidenciaId.ClientID %>").val('0');
                $("#<%= HFLugarNacimiento.ClientID %>").val('0');

                $('#<%= txtFechaNacimiento.FindControl("TxtFecha").ClientID %>').attr('disabled', false);

                $("#<%= TxtSenasParticulares.ClientID %>").val('');

                showpopupother('i', 'Acto Civil', "El número de documento no esta registrado en el sistema.", false, 190, 290);
            }
        }

        function validarcaracterespecial() {
            var caracterespecial = $("#<%= HFValidarTexto.ClientID %>").val();
            var caracter = caracterespecial.split(',');

            var evento = window.event;
            var codigo = evento.charCode || evento.keyCode;
            var char = String.fromCharCode(codigo);

            var bValido = true;

            for (var x = 0; x < caracter.length; x++) {

                if (caracter[x] == char) {
                    bValido = false;
                }
            }

            return bValido;
        }

        function isSujeto(evt) {
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
            if (charCode == 39) {
                letra = true;
            }
            if (charCode == 231) {
                letra = true;
            }
            if (charCode == 199) {
                letra = true;
            }
            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            letra = validarcaracterespecial();
            return letra;
        }

        function Ejecuar_Script() {
            var valida_fechanac = is_Date(document.getElementById('<%=txtFechaNacimiento.FindControl("TxtFecha").ClientID %>'));

            if (valida_fechanac) {

                var nameControl = document.getElementById('<%= txtFechaNacimiento.FindControl("lblErrorDate").ClientID %>')
                document.getElementById('<%= txtFechaNacimiento.FindControl("lblErrorDate").ClientID %>').innerHTML = "";
            }
        }

        function is_Date(crl) {
            //toastr.options.positionClass = "toast-top-right";
            if (crl != null) {

                if (crl.value == '' || crl.value == 'MMM-dd-yyyy' || crl.value == null) {
                    return;
                }
            } else {
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
 

    </script>
    <script type="text/javascript">
        function ObtenerCodigoLocal() {

            if (document.getElementById("chkCodigoLocal").checked == true) {
                $("#<%= ddlLocalDestino.ClientID %>").val('LIMA');
            } else {
                $("#<%= ddlLocalDestino.ClientID %>").val('0');
            }
        }
    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }
        function ActivarGrilla() {
            $('.ocultar').show();
        }
        function OcultarGrilla() {
            $('.ocultar').hide();
        }
        function cerrarPopup() {
            document.getElementById('modalParticipantes').style.display = 'none';
        }
        function cerrarPopupAntecedentesPenales() {
            document.getElementById('modalAntecedentesPenales').style.display = 'none';
        }
        function abrirPopupPregunta() {
            document.getElementById('modalPregunta').style.display = 'block';
        }
        function cerrarPopupPregunta() {
            document.getElementById('modalPregunta').style.display = 'none';
            document.getElementById('modalParticipantes').style.display = 'block';
        }
        function abrirPopup() {
            document.getElementById('modalParticipantes').style.display = 'block';
            desabilitarBotonesEditar();
        }
        function abrirModalIdioma() {
            document.getElementById('ModalIdioma').style.display = 'block';
        }
        function CerrarModalIdioma() {
            document.getElementById('ModalIdioma').style.display = 'none';
        }
        function desabilitarBotonesEditar() {
            $('#<%= btnGrabarParticipante.ClientID %>').attr('disabled', true);
            $('#<%= btnEditarParticipante.ClientID %>').attr('disabled', false);
        }
        function desabilitarBotonesGrabar() {
            $('#<%= btnEditarParticipante.ClientID %>').attr('disabled', false);
        }
        function ActivarNuevoParticipante() {
            $("#<%= hNuevoParticipante.ClientID %>").val('1');
        }
        function abrirPopupLimpiar() {
            document.getElementById('modalParticipantes').style.display = 'block';
            $("#<%= txtNomParticipante.ClientID %>").val('');
            $("#<%= txtApePatParticipante.ClientID %>").val('');
            $("#<%= txtApeMatParticipante.ClientID %>").val('');
            $("#<%= hNuevoParticipante.ClientID %>").val('1');

            $('#<%= txtNomParticipante.ClientID %>').attr('disabled', false);
            $('#<%= txtApePatParticipante.ClientID %>').attr('disabled', false);
            $('#<%= txtApeMatParticipante.ClientID %>').attr('disabled', false);
            $('#<%= txtApeCasadaParticipante.ClientID %>').attr('disabled', false);
            $('#<%= btnGrabarParticipante.ClientID %>').attr('disabled', false);
            $('#<%= btnEditarParticipante.ClientID %>').attr('disabled', false);
            $("#<%= ddl_TipoParticipante.ClientID %>").val('0');
            $("#<%= ddl_TipoDocParticipante.ClientID %>").val('0');

            $("#<%= ddl_DistDomicilio.ClientID %>").val('0');
            $("#<%= ddl_ProvDomicilio.ClientID %>").val('0');
            $("#<%= ddl_DeptDomicilio.ClientID %>").val('0');

            $("#<%= ddl_DeptNacimiento.ClientID %>").val('0');
            $("#<%= ddl_ProvNacimiento.ClientID %>").val('0');
            $("#<%= ddl_DistNacimiento.ClientID %>").val('0');

            $("#<%= TxtDirDir.ClientID %>").val('');
            $("#<%= TxtTelfDir.ClientID %>").val('');
            $("#<%= txtNroDocParticipante.ClientID %>").val('');

            $('#<%= txtFechaNacimiento.FindControl("TxtFecha").ClientID %>').attr('disabled', false);
            $('#<%= txtFechaNacimiento.FindControl("TxtFecha").ClientID %>').val('');
            $("#<%= TxtEmail.ClientID %>").val('');

            $("#<%= TxtSenasParticulares.ClientID %>").val('');

            $("#<%= CmbEstCiv.ClientID %>").val('0');
            $("#<%= CmbGenero.ClientID %>").val('0');
            $("#<%= CmbGradInst.ClientID %>").val('0');
            $("#<%= txtAnio.ClientID %>").val('');
            $("#<%= ddlCompleto.ClientID %>").val('0');
            $("#<%= txtEstatura.ClientID %>").val('');

            $("#<%= rbSi_Discapacidad.ClientID %>").attr('checked', false);
            $("#<%= rbNO_Discapacidad.ClientID %>").attr('checked', false);
            $("#<%= rbSi_Interdicto.ClientID %>").attr('checked', false);
            $("#<%= rbNO_Interdicto.ClientID %>").attr('checked', false);
            $("#<%= rbSi_Donador.ClientID %>").attr('checked', false);
            $("#<%= rbNO_Donador.ClientID %>").attr('checked', false);
            $("#<%= txtCurador.ClientID %>").val('');
            $("#<%= ddlDeclarante.ClientID %>").val('0');
            $("#<%= ddlTutorG.ClientID %>").val('0');
            $("#<%= chkConsignaApeCas.ClientID %>").attr('checked', false);
        }
        function abrirPopupAntecedentesPenales() {
            LimpiarAntecedentePenalDetalle();            
            document.getElementById('modalAntecedentesPenales').style.display = 'block';
        }
        function abrirPopupDocAdjuntosLimpiar() {
            document.getElementById('modalDocAdjuntos').style.display = 'block';
            $("#<%= hDocAdjuntoEditar.ClientID %>").val('0');
            $("#<%= txtNumeroDocAdj.ClientID %>").val('');
            $("#<%= ddlDocAdjunto.ClientID %>").val('0');
        }
        function cerrarPopupDocAdjuntos() {
            document.getElementById('modalDocAdjuntos').style.display = 'none';
        }
        function abrirPopupDocAdjuntos() {
            document.getElementById('modalDocAdjuntos').style.display = 'block';
        }

        function LimpiarAntecedentePenalDetalle() {
            $("#<%= txtOrganoJurisdiccionalMSIAP.ClientID %>").val('');
            $("#<%= txtNumeroExpedienteMSIAP.ClientID %>").val('');

            $('#<%= txtFechaSentenciaMSIAP.FindControl("TxtFecha").ClientID %>').val('');
            
            $("#<%= txtDelitoMSIAP.ClientID %>").val('');
            $("#<%= txtDuracionAniosMSIAP.ClientID %>").val('0');
            $("#<%= txtDuracionMesesMSIAP.ClientID %>").val('0');
            $("#<%= txtDuracionDiasMSIAP.ClientID %>").val('0');

            $('#<%= txtFechaVencimientoMSIAP.FindControl("TxtFecha").ClientID %>').val('');
            $("#<%= txtTipoPena.ClientID %>").val('');
            $("#<%= btnAgregarAntecedentePenal.ClientID %>").attr('value', '   Agregar');
            funSetSession("AntecedentePenalDetalleId", "0");
        }
        function CambiarTexto_btnAgregarAntecedentePenal() {
            $("#<%= btnAgregarAntecedentePenal.ClientID %>").attr('value', '   Actualizar');
        }
        function abrirEditarPopupAntecedentesPenales() {
            document.getElementById('modalAntecedentesPenales').style.display = 'block';
        }
        function abrirPopupImpresionPrevia() {
            document.getElementById('modalPreviImpresión').style.display = 'block';
        }
        function CerrarPopupImpresionPrevia() {
            document.getElementById('modalPreviImpresión').style.display = 'none';
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
        function funSetSession(variable, valor) {
            var url = 'FrmActoGeneral.aspx/SetSession';
            var prm = {};
            prm.variable = variable;
            prm.valor = valor;
            var rspta = execute(url, prm);
        }
        function postBack() {
            __doPostBack('ACTUALIZA', '1');
        }

    </script>
</asp:Content>
