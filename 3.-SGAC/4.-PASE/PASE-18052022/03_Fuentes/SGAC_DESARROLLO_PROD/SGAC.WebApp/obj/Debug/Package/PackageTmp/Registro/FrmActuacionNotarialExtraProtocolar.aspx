<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmActuacionNotarialExtraProtocolar.aspx.cs"
    MaintainScrollPositionOnPostback="true" Inherits="SGAC.WebApp.Registro.FrmActuacionNotarialExtraProtocolar" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlAdjuntoNotarial.ascx" TagName="ctrlAdjunto"
    TagPrefix="uc3" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagName="toolbarcontent"
    TagPrefix="toolbar" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlUploader.ascx" TagName="ctrlUploader"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlUbigeoLineal.ascx" TagName="ctrlUbigeoLineal"
    TagPrefix="uc5" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlUploader.ascx" TagName="ctrlUploader"
    TagPrefix="uc2" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlReimprimirbtn.ascx" TagName="ctrlReimprimirbtn"
    TagPrefix="uc4" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlBajaAutoadhesivo.ascx" TagName="ctrlBajaAutoadhesivo"
    TagPrefix="uc6" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/tinymce/tinymce.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
     <style type="text/css">
        #Background {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: #f9f9f9;
            filter: alpha(opacity=90);
            opacity: 0.9;
            z-index: 100000;
        }

        #Progress {
            position: fixed;
            top: 40%;
            left: 40%;
            height: 20%;
            width: 20%;
            z-index: 100001;
            background-color: #FFFFFF;
            border: 1px solid Gray;
            background-image: url('../Images/cargando.gif');
            background-repeat: no-repeat;
            background-position: center;
        }

        .auto-style1 {
            width: 15%;
        }
    </style>
    <style type="text/css">
        .tMsjeWarnig
        {
            background-color: #F2F1C2;
            border-color: Yellow; /*#6E4E1B;*/
            color: #4B4F5E;
            height: 15px;
            background-image: url('../../Images/img_16_warning.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        .CornerStyle
        {
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
        }
        .ParticipantesLabelStyle
        {
            margin-left: 5px;
            width: 100px;
            display: inline-block;
            background-color: transparent;
        }
        
        .ParticipantesDropDownStyle
        {
            height: 23px;
            width: 180px;
        }
        
        .ParticipantesDropDownStyle > option:hover
        {
            background-color: #1e90ff;
            color: White;
            text-shadow: 1px 1px 1px #3c3c3c;
        }
        
        
        .ParticipantesTextboxDownStyle
        {
            height: 17px;
            width: 177px;
            text-transform: uppercase;
        }
        
        .TituloStyle
        {
            margin-top: 15px;
            background-color: #4E102E;
            border: 1px solid #e0e0e0;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            height: 24px;
            color: White;
            font-weight: bold;
        }
        
        .TituloLabelStyle
        {
            background-color: transparent;
            line-height: 0px;
            margin-left: 12px;
        }
    </style>
    <%--  
Fecha: 06/03/2017
Autor: Miguel Márquez Beltrán
Objetivo: Cambiar el texto de las listas (estado civil y nacionalidad)
           de acuerdo al genero masculino o femenino.
    --%>
    <script type="text/javascript" language="javascript">
        function seguridadURLPrevia() {
            if (document.referrer != "") {
                //Funciones
            }
            else {
                //mensaje de error
                location.href = '../PageError/DeniedPage.aspx';
            }
        }

        window.onload = seguridadURLPrevia;
        </script>
    <script type="text/javascript">

        function ConfirmarEliminar() {
            var Btn_AgregarParticipante =$('#<%= Btn_AgregarParticipante.ClientID %>');

            if (Btn_AgregarParticipante.prop('disabled')) {
                return false;
            }
            else {
                var Ok = confirm('¿Esta seguro de eliminar el Participante?');
                if (Ok)
                    return true;
                else
                    return false;
            }
            
        }
        function cerrarPopupEsperaCarga() {
            document.getElementById('modalEsperaCarga').style.display = 'none';
        }
        function abrirPopupEsperaCarga() {
            document.getElementById('modalEsperaCarga').style.display = 'block';
        }
        function ObtenerElementosGenero() {

            var comboGenero = document.getElementById('<%= ddlRegistroGenero.ClientID %>');
            var comboEstadoCivil = document.getElementById('<%= ddlRegistroEstadoCivil.ClientID %>');
            //          var comboNacionalidad = document.getElementById('<%= ddlRegistroNacionalidad.ClientID %>');
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

            //HabilitarPorParticipante(false);
            HabilitarApellidoCasada();
        }
        function seleccionado() {
            var valorTamano = $("#<%= ddlTamanoLetra.ClientID %>").val();
            if (valorTamano.trim().length > 0) {
                funSetSession("TamanoLetra", valorTamano);
            }
        }

        function HabilitarApellidoCasada() {
            var comboGenero = document.getElementById('<%= ddlRegistroGenero.ClientID %>');
            var comboEstadoCivil = document.getElementById('<%= ddlRegistroEstadoCivil.ClientID %>');

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

            if (comboGenero.selectedIndex > 0 && comboEstadoCivil.selectedIndex > 0) {
                if (comboGenero.options[comboGenero.selectedIndex].text == 'FEMENINO' &&
                  comboEstadoCivil.options[comboEstadoCivil.selectedIndex].text == 'CASADA') {

                    $("#<%= txtApepCas.ClientID %>").attr('disabled', false);
                }
                else {
                    $("#<%= txtApepCas.ClientID %>").attr('disabled', true);
                }
            }
            else {
                $("#<%= txtApepCas.ClientID %>").attr('disabled', true);
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
</script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM2" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuaciones Notariales - EXTRAPROTOCOLARES"></asp:Label></h2>
            </td>
            <td align="right" style="padding-top: 15px">
                <asp:LinkButton ID="Tramite" runat="server" PostBackUrl="~/Registro/FrmTramite.aspx"
                    Font-Bold="True" Font-Size="10pt" ForeColor="Blue" Font-Underline="true">Regresar a Trámites</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="middle">
                <div id="divInformacion" style="height: 28px; background-color: #4E102E; margin: 0px;">
                    <asp:Label ID="lblRecurrente" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                        Font-Size="9pt" Font-Underline="false" ForeColor="White" Text="" Style="position: relative; 
                        top: 7px; left: 5px; float:left;"></asp:Label>
                    <p id='lblTipoActoNotarialEP' style="display: inline-block; position: relative; bottom: 4px;
                        right: 5px; float: right; font-size: 9pt; font-weight: bold; color: White; background-color: transparent">
                    </p>
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 95%;" align="center">
        <tr>
            <td>
                <asp:HiddenField ID="hdn_acno_sOficinaConsularId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_sActoNotarialTipoId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_iActuacionId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_actu_iPersonaRecurrenteId" runat="server" Value="0" />
                <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_iActoNotarialReferenciaId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_ancu_iActoNotarialCuerpoId" runat="server" Value="0" />
                <asp:HiddenField ID="hdm_empr_sTipoEmpresaId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_ando_iActoNotarialDocumentoId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_sUsuarioCreacion" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_vIPCreacion" runat="server" />
                <asp:HiddenField ID="hdn_acno_sUsuarioModificacion" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_vIPModificacion" runat="server" />
                <asp:HiddenField ID="hdn_SelectedTab" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_anpa_iActoNotarialParticipanteId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_btn_Cancel_4" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_bCheckAceptacion" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_bCuerpoGrabado" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PODER_FUERA_REGISTRO" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_AUTORIZACION_VIAJE_MENOR" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_NOTARIA_IDIOMA" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_OTORGANTE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_APODERADO" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_PADRE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_MADRE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_MENOR" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_INTERPRETE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_TITULAR" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_ACOMPANANTE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_TESTIGO_A_RUEGO" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_PARTICIPANTE_RECURRENTE" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_GENERO_MASCULINO" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_GENERO_FEMENINO" runat="server" Value="0" />
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="tabs" style="margin: 0px; padding: 0px;">
                            <ul>
                                <li><a href="#tab-1">
                                    <asp:Label ID="lblDatos" runat="server" Text="Registro"></asp:Label></a></li>
                                <li><a href="#tab-4">
                                    <asp:Label ID="lblParticipantesPFE" runat="server" Text="Participantes"></asp:Label></a></li>
                                <li><a href="#tab-2">
                                    <asp:Label ID="lblCuerpoEscritura" runat="server" Text="Cuerpo"></asp:Label></a></li>
                                <li><a href="#tab-6">
                                    <asp:Label ID="lblAprobar" runat="server" Text="Aprobación y Pago"></asp:Label></a></li>
                                <li><a href="#tab-8">
                                    <asp:Label ID="lblVinculacion" runat="server" Text="Vinculación"></asp:Label></a></li>
                                <li><a href="#tab-5">
                                    <asp:Label ID="lblDocumentos" runat="server" Text="Adjuntos"></asp:Label></a></li>
                            </ul>
                            <div id="tab-1">
                                <!-- REGISTRO - ACTO NOTARIAL -->
                                <asp:UpdatePanel ID="updTipoActo" UpdateMode="Conditional"  runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlTipoActo" runat="server">
                                                        <toolbar:toolbarcontent ID="ctrlToolTipoActo" runat="server"></toolbar:toolbarcontent>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdMsjeRegistro">
                                                    <asp:Label ID="lblValidacionRegistro" runat="server" Text="Debe ingresar los campos requeridos"
                                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lbl_TipoActoNotarialExtra" runat="server" Text="Tipo de Acto Según Tarifario: "></asp:Label>
                                                </td>
                                                <td style="width: 330px;">
                                                    <asp:DropDownList ID="ddlTipoActoNotarialExtra" runat="server" Height="22px" Width="300px"
                                                        Style="cursor: pointer" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoActoNotarialExtra_SelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_subTipoActoNotarialExtra" runat="server" Text="Sub Tipo:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSubTipoNotarialExtra" runat="server" Height="22px" Width="350px"
                                                        Style="cursor: pointer" onchange="nextControlFuncionario()" TabIndex="1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lbl_Funcionario" runat="server" Text="Funcionario Responsable:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFuncionario" runat="server" Height="22px" Width="300px"
                                                        Style="cursor: pointer" TabIndex="2" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCondiciones" runat="server" Text="Condiciones:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCondiciones" runat="server" Height="22px" Width="350px"
                                                        Style="cursor: pointer" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            
                            <div id="tab-4">
                                <!-- PODER FUERA DE REGISTRO (PARTICIPANTES) - ACTO NOTARIAL -->
                                <asp:UpdatePanel ID="UpdatePanelParticipante" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hdn_Tipo_Participante_Selected" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdn_Tipo_Participante_Editando" runat="server" Value="-1" />
                                        <asp:HiddenField ID="hidDocumentoSoloNumero" runat="server" Value="0" />
                                        <asp:HiddenField ID="hParticipanteId" runat="server" />
                                        <div>
                                            <toolbar:toolbarcontent ID="ctrlToolBarParticipante" runat="server"></toolbar:toolbarcontent>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblValidacionParticipante1" runat="server" Text="Debe ingresar los campos requeridos"
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                            </asp:Label>
                                        </div>
                                        <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                            border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTipo" Text="TIPO" runat="server" Style="font-weight: 700; color: #800000;" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 130px">
                                                    <p id="P7" style="color: Black; display: inline; background: White;">
                                                        Tipo Participante:</p>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlRegistroTipoParticipante" runat="server" Width="200px" CssClass="DropDownList"
                                                        TabIndex="1" Style="margin-left: 0px; cursor: pointer" AutoPostBack="True" OnSelectedIndexChanged="ddlRegistroTipoParticipante_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label101" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="dvCamposParticipantes">
                                            <div>
                                            </div>
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDatosPersonales" Text="DATOS PERSONALES" runat="server" Style="font-weight: 700;
                                                            color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 130px">
                                                        <p id="P6" style="color: Black; display: inline; background: White;">
                                                            Tipo Documento:</p>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="ddlRegistroTipoDoc" runat="server" TabIndex="2" Style="cursor: pointer"
                                                            CssClass="DropDownList" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlRegistroTipoDoc_SelectedIndexChanged1" />
                                                        <asp:Label ID="Label102" runat="server" Text="*" Style="color: #FF0000;"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <p id="P5" style="color: Black; display: inline; background: White;">
                                                            Nro. Documento:</p>
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="txtDocumentoNro" runat="server" MaxLength="12" TabIndex="3" Width="150px"
                                                            CssClass="txtLetra" OnTextChanged="txtDocumentoNro_TextChanged" AutoPostBack="True" />
                                                        <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                            Style="width: 16px" ToolTip="Buscar" OnClick="imgBuscar_Click" />
                                                        
                                                        <asp:Label ID="Label2" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LblOtroDocumento" runat="server" Text="Otro documento :" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescOtroDocumento" runat="server" CssClass="txtLetra" onkeypress="return isSujeto(event);"
                                                            Width="283px" TabIndex="4"></asp:TextBox>
                                                        <p id="lblDescOtroDocObl" runat="server" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <p id="P4" style="color: Black; display: inline; background: White;">
                                                            Primer Apellido:</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApePaterno" runat="server" TabIndex="5" MaxLength="80" 
                                                            Width="350px" CssClass="txtLetra" />
                                                        <asp:Label ID="Label103" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td colspan="3">
                                                        <%--<p style="color: Red; display: inline; background: White; ">*</p>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <p id="P3" style="color: Black; display: inline; background: White;">
                                                            Segundo Apellido:</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApeMaterno" runat="server" TabIndex="6" MaxLength="80"
                                                            Width="350px" CssClass="txtLetra" />
                                                    </td>
                                                    <td>
                                                        <p id="P2" style="color: Black; display: inline; background: White;">
                                                            Apellido de Casada:</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtApepCas" runat="server" Width="250px" TabIndex="7" CssClass="txtLetra"
                                                            MaxLength="50" Enabled="False" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <p id="P1" style="color: Black; display: inline; background: White;">
                                                            Nombres:</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNombres" runat="server" TabIndex="8" MaxLength="80" 
                                                            Width="350px" CssClass="txtLetra" />
                                                        <asp:Label ID="Label43" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <p id="lblGeneroTitulo" style="color: Black; display: inline; background: White;">
                                                            Género:</p>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRegistroGenero" runat="server" TabIndex="9" AutoPostBack="true"
                                                            Style="cursor: pointer" CssClass="DropDownList" Width="150px"
                                                            OnSelectedIndexChanged="ddlRegistroGenero_SelectedIndexChanged" />
                                                        <asp:Label ID="Label5" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="DivFechaNacimiento" runat="server">
                                                        <td>
                                                            <p id="lblFechaNacimientoTitulo" style="color: Black; display: inline; background: White;">
                                                                Fecha Nacimiento:</p>
                                                        </td>
                                                        <td>
                                                            <SGAC_Fecha:ctrlDate ID="txtFecNac" runat="server" TabIndex="7" />
                                                            <p id="lblFechaNacObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                            <asp:Label ID="LblEdad" runat="server" Text="Edad :" CssClass="hideControl" Visible="false" />
                                                            <asp:TextBox ID="LblEdad2" runat="server" Enabled="False" Visible="false" Width="30px"
                                                                CssClass="hideControl" TabIndex="12"></asp:TextBox>
                                                            <asp:Button ID="BtnCalcularEdad" CssClass="hideControl" runat="server" Visible="false"
                                                                Width="65px" Text="Calcular" OnClick="BtnCalcularEdad_Click" />
                                                        </td>
                                                    </div>
                                                    <div id="DivEstadoCivil" runat="server">
                                                        <td>
                                                            <p id="lblEstadoCivilTitulo" style="color: Black; display: inline; background: White;">
                                                                Estado Civil:</p>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRegistroEstadoCivil" runat="server" TabIndex="11" onchange="HabilitarApellidoCasada()"
                                                                Style="cursor: pointer" CssClass="DropDownList" Width="145px" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="ddlRegistroEstadoCivil_SelectedIndexChanged" />
                                                            <p id="lblEstadoCivilObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRegistroNacionalidad" runat="server" TabIndex="9" CssClass="hideControl"
                                                                Style="cursor: pointer" Width="150px" />
                                                        </td>
                                                    </div>
                                                </tr>
                                                <tr>
                                                    <div id="DivPaisNacimientoGentilicio" runat="server">
                                                        <td>
                                                            <p id="LblPaisOrigen" style="color: Black; display: inline; background: White;">
                                                                Nacionalidad:</p>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPaisOrigen" runat="server" AutoPostBack="True" Style="cursor: pointer"
                                                                CssClass="DropDownList" OnSelectedIndexChanged="ddlPaisOrigen_SelectedIndexChanged"
                                                                TabIndex="12" Width="350px" />
                                                            <asp:Label ID="Label11" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <p id="LblGentilicio" style="color: Black; display:none; background: White;">
                                                                Gentilicio :</p>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblDescGentilicio" runat="server" Style="text-transform: uppercase; display:none;
                                                                color: Gray" Width="200px" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </div>
                                                </tr>
                                            </table>
                                            <div id="DivDireccion" runat="server">
                                                <table id="dtDireccionTitulo" style="border-bottom: 1px solid #800000; border-top: 1px solid #800000;
                                                    width: 100%; border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDireccion" Text="DIRECCIÓN" runat="server" Style="font-weight: 700;
                                                                color: #800000;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="TableDireccion" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <p id="lblDomicilioTitulo" style="color: Black; display: inline; background: White;">
                                                                Domicilio:</p>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtDireccion" runat="server" TabIndex="13" Width="450px" MaxLength="300"
                                                                CssClass="txtLetra" />
                                                            <p id="lblDireccionObl" style="color: Red; display: inline; background: White; ">
                                                                *</p>
                                                        </td>
                                                        <td>
                                                            <p id="lblCodigoPostalTitulo" style="color: Black; display: inline; background: White;
                                                                visibility: hidden;">
                                                                Código Postal:</p>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="ParticipantesTextboxDownStyle"
                                                                MaxLength="10" TabIndex="14" onkeypress="isNumeroLetra(event);" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <p id="lblUbigeoPaisTitulo" style="color: Black; display: inline; background: White;">
                                                                Cont./Dept.:</p>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_UbigeoPais" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_UbigeoPais_SelectedIndexChanged"
                                                                CssClass="ParticipantesDropDownStyle" TabIndex="15" Style="cursor: pointer">
                                                            </asp:DropDownList>
                                                            <p id="lblUbigeoPaisObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                        <td>
                                                            <p id="lblUbigeoRegionTitulo" style="color: Black; display: inline; background: White;">
                                                                Pais/Prov.:</p>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_UbigeoRegion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_UbigeoRegion_SelectedIndexChanged"
                                                                Style="cursor: pointer" CssClass="ParticipantesDropDownStyle" TabIndex="16">
                                                            </asp:DropDownList>
                                                            <p id="lblUbigeoRegionObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                        <td style="width: 80px">
                                                            <p id="lblUbigeoCiudadTitulo" style="color: Black; display: inline; background: White;">
                                                                Ciu./Dist.:</p>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_UbigeoCiudad" runat="server" AutoPostBack="True" CssClass="ParticipantesDropDownStyle"
                                                                Style="cursor: pointer" TabIndex="17" OnSelectedIndexChanged="ddl_UbigeoCiudad_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <p id="lblUbigeoCiudadObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <table id="dtDireccionOtros" style="border-bottom: 1px solid #800000; border-top: 1px solid #800000;
                                                width: 100%; border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOtros" Text="OTROS" runat="server" Style="font-weight: 700; color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%">
                                                <tr>
                                                    <div id="DivOpcionIncapacitado" runat="server">
                                                        <td style="width: 130px" valign="middle">
                                                            <p id="lblIncapacitadoTitulo" style="color: Black; display: inline-block; background: White;">
                                                                Incapacitado:</p>
                                                        </td>
                                                        <td colspan="3">
                                                            <%--<asp:CheckBox ID="chkIncapacitado" runat="server" TabIndex="18" Width="30px" onclick="funIncapacidadCheckSelected(true)" />--%>
                                                            <asp:CheckBox ID="chkIncapacitado" runat="server" TabIndex="18" Width="30px" 
                                                                AutoPostBack="True" oncheckedchanged="chkIncapacitado_CheckedChanged" />
                                                        </td>
                                                    </div>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <table width="100%" cellpadding="0px" cellspacing="0px">
                                                            <tr>
                                                                <div id="DivIncapacitado" runat="server">
                                                                    <td style="width: 130px">
                                                                        <asp:Label ID="lblIncapacidadFirmar" runat="server" Text="Incapacitado de firmar por ser:"
                                                                            Style="color: Black"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 300px; text-align: left">
                                                                        <asp:TextBox ID="txtRegistroTipoIncapacidad" runat="server" TabIndex="20" MaxLength="300"
                                                                            Width="300px" CssClass="txtLetra" />
                                                                    </td>
                                                                    <td style="width: 35px; text-align: center">
                                                                        <asp:CheckBox ID="chkNoHuella" runat="server" Width="25px" TabIndex="19" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblHuella" runat="server" Text="Incapacitado para realizar la firma y huella"></asp:Label>
                                                                    </td>
                                                                </div>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="DivOcupacion" runat="server">
                                                        <td style="width: 130px">
                                                            <p id="lblProfesionTitulo" style="color: Black; display: inline; background: White;">
                                                                Ocupación:</p>
                                                        </td>
                                                        <td style="width: 500px">
                                                            <asp:DropDownList ID="ddlRegistroProfesion" runat="server" TabIndex="21" Style="cursor: pointer"
                                                                CssClass="DropDownList" Width="480px">
                                                            </asp:DropDownList>
                                                            <p id="lblProfesionObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                    </div>
                                                    <div id="DivIdioma" runat="server">
                                                        <td style="width: 70px">
                                                            <p id="lblIdiomaTitulo" style="color: Black; display: inline; background: White;">
                                                                Idioma:</p>
                                                        </td>
                                                        <td style="width: 210px">
                                                            <asp:DropDownList ID="ddlRegistroIdioma" runat="server" TabIndex="22" Style="cursor: pointer"
                                                                CssClass="DropDownList" Width="180px">
                                                            </asp:DropDownList>
                                                            <p id="lblIdiomaObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </td>
                                                    </div>
                                                </tr>
                                                <tr>
                                                    <td style="width: 605px" colspan="4">
                                                        <div id="OtorganteIncapacitado" runat="server">
                                                            <p id="lblOtorganteIncapacitadoTitulo" style="color: Black; display: inline-block;
                                                                bottom: 5px; background: White;">
                                                                Otorgante con Incapacidad de Firmar:</p>
                                                            &nbsp;
                                                            <asp:DropDownList ID="ddlOtorganteIncapacitado" runat="server" TabIndex="23" Width="380px"
                                                                Style="display: inline; cursor: pointer">
                                                            </asp:DropDownList>
                                                            <p id="lblOtorganteIncapacitadoObl" style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="DivEscrituraPartida" runat="server">
                                                        <td>
                                                            <p id="lblNroEscritura" style="color: Black; display: inline; background: White;">
                                                                N° Escritura:</p>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNumeroEscritura" runat="server" Width="226px" type="text" MaxLength="50"
                                                                CssClass="txtLetra" onkeypress="return isLetraNumeroDoc(event);" TabIndex="24" />
                                                        </td>
                                                        <td>
                                                            <p id="lblPartida" style="color: Black; display: inline; background: White;">
                                                                N° Partida:</p>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNumeroPartida" runat="server" Width="226px" type="text" MaxLength="50"
                                                                CssClass="txtLetra" onkeypress="return isLetraNumeroDoc(event);" TabIndex="25" />
                                                        </td>
                                                    </div>
                                                </tr>
                                            </table>
                                            <div>
                                                <%--<asp:Button ID="Btn_AgregarParticipante" runat="server" Text="Agregar" Width="150px"
                                                    TabIndex="22" CssClass="btnNewActuacion" OnClick="Btn_AgregarParticipante_Click"
                                                    OnClientClick="return ValidarAgregarParticipante();" />--%>
                                                    <asp:Button ID="Btn_AgregarParticipante" runat="server" Text="Agregar" Width="150px"
                                                    TabIndex="22" CssClass="btnNewActuacion" OnClick="Btn_AgregarParticipante_Click" />

                                            </div>
                                        </div>
                                        <div>
                                        <asp:HiddenField ID="hCodPersona" runat="server" Value="0" />
                                        <asp:HiddenField ID="hEditarContestigoRuego" runat="server" Value="0" />
                                        <asp:HiddenField ID="hPerResidenciaID" runat="server" Value="0" />
                                        <asp:HiddenField ID="hParticipanteID_Editar" runat="server" Value="0" />
                                        <asp:HiddenField ID="hTipParticipante_Editar" runat="server" Value="0" />
                                        <asp:HiddenField ID="hTipDoc_Editar" runat="server" Value="0" />
                                        <asp:HiddenField ID="hNumDoc_Editar" runat="server" Value="0" />
                                        <asp:HiddenField ID="hExisteInterprete" runat="server" Value="0" />
                                        <asp:HiddenField ID="hExistePadre" runat="server" Value="0" />
                                        <asp:HiddenField ID="hExisteMadre" runat="server" Value="0" />
                                        <asp:HiddenField ID="hExisteAcompañante" runat="server" Value="0" />
                                        <asp:HiddenField ID="hExisteIdiomaExtranjero" runat="server" Value="0" />
                                            <asp:UpdatePanel runat="server" ID="UpdParticipante" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div style="overflow: auto; height: 190px; width: 900px; border: 1px solid #800000;
                                                        margin: 5px;">
                                                        <asp:GridView ID="grd_Participantes" runat="server" AutoGenerateColumns="False" Width="900px"
                                                            CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" ShowHeaderWhenEmpty="True"
                                                            OnRowCommand="grd_Participantes_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="acpa_sTipoParticipanteId_desc" HeaderText="Tipo Persona" />
                                                                <asp:BoundField HeaderText="Tipo Doc" DataField="peid_sDocumentoTipoId_desc" />
                                                                <asp:BoundField DataField="peid_sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />
                                                                <asp:BoundField HeaderText="Nro Doc" DataField="peid_vDocumentoNumero" />
                                                                <asp:BoundField HeaderText="Participante" DataField="participante" />
                                                                <asp:BoundField HeaderText="Nacionalidad" DataField="pers_sNacionalidadId_desc" />
                                                                <asp:BoundField HeaderText="anpa_iActoNotarialParticipanteId" DataField="anpa_iActoNotarialParticipanteId"
                                                                    HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" />
                                                                <asp:BoundField HeaderText="anpa_sTipoParticipanteId" DataField="anpa_sTipoParticipanteId"
                                                                    HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" />
                                                                <%--<asp:BoundField DataField="anpa_iActoNotarialParticipanteIdAux" HeaderText="anpa_iActoNotarialParticipanteIdAux">
                                                                    <ControlStyle CssClass="hideControl" />
                                                                    <FooterStyle CssClass="hideControl" />
                                                                    <HeaderStyle CssClass="hideControl" />
                                                                    <ItemStyle CssClass="hideControl" />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="anpa_iReferenciaParticipanteId" HeaderText="anpa_iReferenciaParticipanteId">
                                                                    <ControlStyle CssClass="hideControl" />
                                                                    <FooterStyle CssClass="hideControl" />
                                                                    <HeaderStyle CssClass="hideControl" />
                                                                    <ItemStyle CssClass="hideControl" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="pers_sIdiomaNatalId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />
                                                                <asp:BoundField HeaderText="Idioma" DataField="pers_sIdiomaNatalId_desc" />
                                                                <asp:BoundField DataField="anpa_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />
                                                                 <asp:BoundField DataField="anpa_bFlagHuella" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />  
                                                                 <asp:BoundField DataField="pers_bIncapacidadFlag" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />     
                                                                    <asp:BoundField DataField="anpa_vNumeroEscrituraPublica" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />     
                                                                    <asp:BoundField DataField="anpa_vNumeroPartida" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta" />     
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEditarParticipante" CommandName="Editar"  CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEliminarParticipante" CommandName="Eliminar" ToolTip="Eliminar"
                                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_delete.png" OnClientClick="if(!ConfirmarEliminar()) { return false;}" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                            <EmptyDataTemplate>
                                                                <table id="tbSinDatos">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="10%">
                                                                                <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
                                                                            </td>
                                                                            <td width="5%">
                                                                            </td>
                                                                            <td width="85%">
                                                                                <asp:Label ID="lblSinDatos" runat="server" Text="No se encontraron Datos..."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                            <AlternatingRowStyle />
                                                            <PagerStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                    <div id="modalEsperaCarga" class="modal">
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

                                        <div id="OtrosDatos" runat="server" visible = "false">
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" Text="OTROS DATOS" runat="server" Style="font-weight: 700;
                                                            color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            <table width="100%">
                                                
                                                <tr>
                                                    <td style="width: 101px">
                                                        Número de Escritura Pública:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtEscrituraPublica" CssClass="txtLetra" runat="server" Width="200px" ></asp:TextBox>
                                                        <asp:Label ID="Label21" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    
                                                </tr>

                                                <tr>
                                                    <div id="Div2" runat="server">
                                                        <td style="width: 101px">
                                                            Número de Partida Inscrita en Sunarp:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPartidaSunarp" CssClass="txtLetra" runat="server" Width="200px" ></asp:TextBox>
                                                            <asp:Label ID="Label22" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                        </td>
                                                    </div>
                                                    
                                                </tr>
                                            </table>
                                        </div>

                                        <div id="divDestinoMenorId">
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDatosDestino" Text="DATOS DEL DESTINO" runat="server" Style="font-weight: 700;
                                                            color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <p class="ParticipantesLabelStyle">
                                                    Cont./Dept.:</p>
                                                <asp:DropDownList ID="ddl_UbigeoPaisViajeDestino" runat="server"
                                                    Style="cursor: pointer" 
                                                    CssClass="ParticipantesDropDownStyle" TabIndex="23" onchange="cargarProvinciaViaje()">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label9" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                <p class="ParticipantesLabelStyle">
                                                    País/Prov.:</p>
                                                <asp:DropDownList ID="ddl_UbigeoRegionViajeDestino" runat="server" 
                                                    Style="cursor: pointer" 
                                                    CssClass="ParticipantesDropDownStyle" TabIndex="24" onchange="cargarDistritoViaje()">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label8" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                <p class="ParticipantesLabelStyle">
                                                    Ciu./Dist.:</p>
                                                <asp:DropDownList ID="ddl_UbigeoCiudadViajeDestino" runat="server" 
                                                    Style="cursor: pointer" CssClass="ParticipantesDropDownStyle" TabIndex="25" onchange="cargarUbigeoViaje();" >
                                                </asp:DropDownList>
                                                <asp:Label ID="Label7" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                <asp:HiddenField ID="hbigeoViajeDestino" runat="server" />
                                            </div>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td> 
                                                    <td>
                                                        Ejemplo: LIMA-MADRID-LIMA
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 101px">
                                                        Itinerario:
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtItinerario" CssClass="txtLetra" runat="server" Height="50px"
                                                            TextMode="MultiLine" Width="670px" TabIndex="26"></asp:TextBox>
                                                        <asp:Label ID="Label10" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    
                                                </tr>

                                                <tr>
                                                    <div id="observación" runat="server">
                                                        <td style="width: 101px">
                                                            Observación:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtObservacion" CssClass="txtLetra" runat="server" Height="50px"
                                                                TextMode="MultiLine" Width="670px" TabIndex="26"></asp:TextBox>
                                                            <asp:Label ID="Label12" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                        </td>
                                                    </div>
                                                    
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display: none;">
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000; border-top-color: #800000; margin-top: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" Text="OTROS" runat="server" Style="font-weight: 700; color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display: none;">
                                            <asp:Label ID="Label48" runat="server" Text="Precisiones que seran insertadas en el documento definitivo: "
                                                Style="margin-left: 0px" />
                                            <asp:TextBox ID="txtObservacionesCertificador" runat="server" Rows="3" TextMode="MultiLine"
                                                Width="100%" Height="100px" onKeyDown="return limitar(event,this.value,500)"
                                                onkeyUp="return limitar(event,this.value,500)" onkeypress="return isDescripcion(event);"></asp:TextBox>
                                        </div>
                                        <asp:Button ID="btninterprete" runat="server" Text="Button" OnClick="btninterprete_Click"
                                            Style="display: none" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tab-2">
                                <!-- CUERPO - ACTO NOTARIAL -->
                                <!-- Body -->
                                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:Button ID="btnVistaPrevia" runat="server" Text="     Vista Previa" Width="120px"
                                                TabIndex="17" CssClass="btnVisualizar" OnClientClick="return VerificarCuerpo();"
                                                OnClick="btnVistaPrevia_Click" />
                                            &nbsp;
                                            <asp:Button ID="btnGrabarCuerpoTemporal" runat="server" Text="   Grabar" Width="120px"
                                                TabIndex="17" CssClass="btnSaveAnotacion" 
                                                OnClientClick="return ValidarTabCuerpoTemporal();" />
                                            <asp:Button ID="btnGrabarCuerpo" runat="server" Text="   Aprobar" Width="120px" TabIndex="17"
                                                CssClass="btnAprobar" 
                                                OnClientClick="return ValidarTabCuerpo();" 
                                                
                                                 />
                                            <asp:HiddenField ID="hdn_acno_iActoNotarialId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hTipActo" runat="server" Value = "0" />
                                        </div>
                                        <br />
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 200px;">
                                                    <div id="ocultar" runat="server">
                                                        <asp:Label ID="lblLetra" runat="server" Text="Tamaño de Letra"></asp:Label>
                                                        &nbsp;
                                                        <asp:DropDownList ID="ddlTamanoLetra" runat="server" onchange="return seleccionado();"
                                                            Style="cursor: pointer" OnSelectedIndexChanged="ddlTamanoLetra_SelectedIndexChanged">
                                                            <asp:ListItem>7</asp:ListItem>
                                                            <asp:ListItem>8</asp:ListItem>
                                                            <asp:ListItem>9</asp:ListItem>
                                                            <asp:ListItem>10</asp:ListItem>
                                                            <asp:ListItem>11</asp:ListItem>
                                                            <asp:ListItem>12</asp:ListItem>
                                                            <asp:ListItem>13</asp:ListItem>
                                                            <asp:ListItem>14</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkImprimirFirmaTitular1" runat="server" Text="Imprimir Firma del Titular"
                                                        Style="cursor: pointer" Checked="True" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div id="accordion" style="background-color: transparent;">
                                            <h3>
                                                Introducción</h3>
                                            <div style="text-align: justify;">
                                                <asp:Label ID="lblcuerpoIntroduccion" runat="server" Text="" Style="font-size: 12px;
                                                    font-family: Verdana; text-align: justify; background-color: transparent;">
                                                </asp:Label>
                                            </div>
                                            <h3>
                                                Cuerpo</h3>
                                            <div>
                                                <div>
                                                    <asp:TextBox ID="txtCuerpo" runat="server" Height="250px" Class="mceEditor" TextMode="MultiLine"
                                                        Width="100%"></asp:TextBox>
                                                    <asp:HiddenField ID="hCuerpo" runat="server" />
                                                </div>
                                            </div>
                                            <h3>
                                                Conclusión</h3>
                                            <div style="text-align: justify;">
                                                <asp:Label ID="lblcuerpoConclusion" runat="server" Style="font-family: Verdana; text-align: justify;
                                                    background-color: transparent" Text=""> </asp:Label>
                                            </div>
                                            <h3>
                                                Información Adicional</h3>
                                            <div>
                                                <div>
                                                    <asp:TextBox ID="txtInfoAdicional" runat="server" Class="mceEditor" Height="250px"
                                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                    <asp:HiddenField ID="hInfoAdicional" runat="server" />
                                                </div>
                                            </div>
                                            </h3>
                                        </div>
                                        <div style="background-color: transparent; margin: 10px 25px 5px 25px; font-family: Verdana;
                                            font-weight: bold; text-align: center; font-size: 16px;">
                                            <input id="chkxAceptacion" type="checkbox" onchange="return HabilitarGrabarCuerpo();"
                                                style="font-size: 16px;" />
                                            <asp:Label ID="lblchkAceptacion" runat="server" Text=":"></asp:Label>
                                            <asp:CheckBoxList ID="Clst_BaseLegal" runat="server" Width="778px" TabIndex="2" OnSelectedIndexChanged="Clst_BaseLegal_SelectedIndexChanged"
                                                Visible="false">
                                                <asp:ListItem Value="Art167">Artículo 167 del código civil - Poder especial para actos de disposición</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tab-5">
                                <!-- ADJUNTO - ACTO NOTARIAL -->
                                <asp:Button ID="Btn_Refresh" runat="server" Text="Refresh" OnClick="Btn_Refresh_Click"
                                    Style="display: none;" Visible="false" />
                                <asp:UpdatePanel ID="updDigitalizacion" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <input id="btnSave_tab6" type="button" value="   Grabar" class="btnSave" style="width: 90px;"
                                                        onclick="javascript:SaveDocumentoDigitalizado();" />
                                                    <asp:Button ID="btnCancelarTab6" runat="server" Text="    Limpiar" class="btnLimpiar"
                                                        OnClick="btnCancelarTab6_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdMsjeArchivoDigital" colspan="10">
                                                    <asp:Label ID="lblValidacionArchivoDigital" runat="server" Text="Debe ingresar los campos requeridos"
                                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Tipo :" Width="76px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTipoArchivoAdjunto" runat="server" Width="154px" AutoPostBack="True"
                                                        Style="cursor: pointer" TabIndex="26" Height="18px" OnSelectedIndexChanged="ddlTipoArchivoAdjunto_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
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
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDescAdj" runat="server" Text="Descripción :" Width="76px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_AdjuntoDescripcion" runat="server" Width="537px" CssClass="txtLetra"
                                                        MaxLength="300" TabIndex="27" />
                                                </td>
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
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAdjArchiv" runat="server" Text="Adjuntar Archivo :"></asp:Label>
                                                </td>
                                                <td>
                                                    <%-- <div id="divUploadDigitalizacion">
                                                    --%>
                                                    <uc2:ctrlUploader ID="ctrlUploader2" runat="server" FileExtension=".pdf" FileSize="102400"
                                                        Width="2500" OnClick="MyUserControlUploader2Event_Click" />
                                                    <asp:HiddenField ID="hidNomAdjFile2" runat="server" />
                                                    <%--</div>--%>
                                                </td>
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
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnVisualizarDigitalizacion" runat="server" Text="Visualizar Archivo"
                                                        Style="display: inline-block; position: relative; bottom: 8px; left: 0px;" CssClass="btnGeneral"
                                                        Width="147px" Enabled="False" TabIndex="29" OnClick="btnVisualizarDigitalizacion_Click"
                                                        Visible="False" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTamanoMaxAdjnuto" runat="server" Style="color: Gray; font-weight: bold;
                                                        display: inline-block; position: relative; bottom: 8px; right: 260px;"></asp:Label>
                                                </td>
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
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <input id="btnAgregarArchivo" type="button" value="Agregar" class="btnNew" style="width: 110px;"
                                                        onclick="AddDocumentoDigitalizado();" />
                                                </td>
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
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="8">
                                                    <asp:Button ID="btnLoadArchivoDigitalizado" runat="server" Text="Agregar" OnClick="btnLoadArchivoDigitalizado_Click"
                                                        Style="display: none;" />
                                                    <asp:GridView ID="Gdv_Adjunto" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                        AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="True" OnRowCommand="Gdv_Adjunto_RowCommand"
                                                        Width="100%" OnRowDataBound="Gdv_Adjunto_RowDataBound">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ando_iActoNotarialDocumentoId" HeaderText="ActoNotarialDocumentoId"
                                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ando_iActoNotarialId" HeaderText="ActoNotarialId" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ando_vRutaArchivo" HeaderText="Nombre Archivo" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ando_vDescripcion" HeaderText="Descripción">
                                                                <ItemStyle Width="450px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Vista Previa" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnPrint" CommandName="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_16_preview.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle Font-Names="Arial" Font-Size="11px" />
                                                        <EmptyDataTemplate>
                                                            <table id="tbSinDatosDigitalizacion">
                                                                <tbody>
                                                                    <tr>
                                                                        <td width="10%">
                                                                            <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
                                                                        </td>
                                                                        <td width="5%">
                                                                        </td>
                                                                        <td width="85%">
                                                                            <asp:Label ID="lblSinDatosFunc" runat="server" Text="Sin Datos..."></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="ctrlUploader2"></asp:PostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tab-6">
                                <!-- APROBACION Y PAGO - ACTO NOTARIAL -->
                                <!-- Body -->
                                <asp:UpdatePanel ID="updRegPago" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="13">
                                                    <toolbar:toolbarcontent ID="ctrlToolBar5" runat="server"></toolbar:toolbarcontent>
                                                    <div>
                                                    
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                        <tr>
                                            <td colspan="2">
                                                 <asp:Label ID="lblValidacionRegistroPago" runat="server" Text="Falta validar algunos campos."
                                                 CssClass="hideControl" ForeColor="Red"></asp:Label>

                                                <asp:HiddenField ID="HF_NACIONALIDAD_PERUANA" Value="0" runat="server" />
                                                <asp:HiddenField ID="HF_PAGADO_EN_LIMA" Value="3501" runat="server" />
                                                <asp:HiddenField ID="HF_TRANSFERENCIA" Value="3507" runat="server" />
                                                <asp:HiddenField ID="HF_DEPOSITO_CUENTA" Value="3508" runat="server" />                                
                                                <asp:HiddenField ID="HF_TIPO_PAGO" runat="server" />                                
                                                <asp:HiddenField ID="H_CAN_AUTOADHESIVO" runat="server" /> 
                                            </td>
                                        </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFecAct" runat="server" Text="Fecha Registro: "></asp:Label>
                                                </td>
                                                <td colspan=5">
                                                    <asp:Label ID="LblFecha" runat="server" Font-Bold="true"></asp:Label>
                                                </td>
                                                            
                                        </tr>    
                                            
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblTarifaConsular" runat="server" Text="Tarifa Consular:"></asp:Label>
                                                </td>
                                                <td colspan="5" rowspan="2">
                                                    <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                        border-bottom-color: #800000;">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblDatosOficinaRegistral" runat="server" Style="font-weight: 700;
                                                                    color: #800000;" Text="REGISTRO TARIFA" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="border-width: 1px; border-color: #666; border-style: solid">
                                                        
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="lblLeyendaTarifa" runat="server" Font-Bold="true" 
                                                                                Text=" (Ingresar código actuación y Presionar tecla ENTER)" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="Txt_TarifaId" runat="server" CssClass="campoNumero" 
                                                                                            Enabled="False" MaxLength="4" OnTextChanged="Txt_TarifaId_TextChanged" 
                                                                                            TabIndex="1" ToolTip="Coloque el numero de la seccion del tarifario" 
                                                                                            Width="38px" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="updBusTarifa" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="Txt_TarifaDescripcion" runat="server" CssClass="txtLetra" 
                                                                                                    Enabled="False" Width="599px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="ImageButton2" runat="server" Enabled="False" Height="18px" 
                                                                                                    ImageUrl="~/Images/img_16_search.png" OnClick="ImageButton1_Click" 
                                                                                                    ToolTip="Buscar" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:ListBox ID="Lst_Tarifario" runat="server" AutoPostBack="True" 
                                                                                                    BackColor="#EAFFFF" OnSelectedIndexChanged="Lst_Tarifario_SelectedIndexChanged" 
                                                                                                    Width="626px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
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
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                              
                                            </tr>
                                        </table>

                                        <table>
   <tr id="trPago" runat="server">
                            <td>
                            </td>
                            <td align="right" style="vertical-align:top;padding-top: 8px">
                               <asp:Label ID="lblTipPago" runat="server" Text="Tipo de Pago:"></asp:Label>
                            </td>
                            <td>
                                 <table>
                                 <tr>
                                    <td>
                                            <asp:DropDownList ID="ddlTipoPago" runat="server" Height="20px" Width="200px" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmb_TipoPago_SelectedIndexChanged" Enabled="True" />
                                <asp:Label ID="lblCO_cmb_TipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>

                                        </td>
                                        <td align="right" style="width:70px">
                                            <asp:Label ID="lblExoneracion" runat="server" Text="Ley que exonera el pago:"></asp:Label>                                                                                    
                                        </td>
                                        <td>                                      
                                           <asp:DropDownList ID="ddlExoneracion" runat="server"  style="cursor:pointer"
                                                Enabled="True" Height="20px" 
                                                Width="414px" />                                            
                                            <asp:Label ID="lblValExoneracion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            <asp:RadioButton ID="RBNormativa" runat="server" Text="" style="cursor:pointer"
                                                GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()"/>
                                                
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
                                        <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text=""  style="cursor:pointer"
                                            GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()"/>
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
                                <asp:Label ID="lblCantidad" runat="server" Text="Cantidad:"></asp:Label>
                            </td>
                            <td style="margin-left: 40px">
                                <asp:TextBox ID="txtCantidad" runat="server" Width="100px" AutoPostBack="True" 
                                    OnTextChanged="txtCantidad_TextChanged"
                                    Enabled="True" CssClass="campoNumero" MaxLength="2" />
                                &nbsp;
                                <asp:Label ID="lblCO_txtCantidad" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                <asp:Label ID="lblLeyenda" runat="server" Text=" (Presionar tecla ENTER para calcular total)" Font-Bold="true"></asp:Label>
                                <asp:HiddenField runat="server" ID="hdn_tope_min" Value="0" />
                                <asp:TextBox ID="Txt_TarifaProporcional" runat="server" Visible="false"></asp:TextBox>

                            </td>
                            <td>
                            </td>
                        </tr>     
                                        </table>

                                        <table>                                           
                                            <tr>
                                            <td colspan="10" id="pnlPagLima" runat="server" visible="false">
                                               <table class="mTblSecundaria">
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNroOperacion" runat="server" Text="Nro de Operación :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroOperacion" runat="server" Width="200px" MaxLength="50" CssClass="txtLetra" />
                                        <asp:Label ID="lblCO_txtNroOperacion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
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
                                        <asp:Label ID="lblCO_ddlNomBanco" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
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
                                        <asp:Label ID="lblCO_ctrFecPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
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
                                        <asp:TextBox ID="txtMtoCancelado" runat="server" Width="150px" 
                                            CssClass="campoNumero" Enabled="False" />
                                        <asp:Label ID="lblCO_txtMtoCancelado" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>    
                                            </td>
                                            </tr>

                                            <tr>
                                                
                                                <td width="80px">
                                                    <asp:Label ID="lblMtoSC" runat="server" Text="Monto S/C:"></asp:Label>
                                                </td>
                                                <td width="100px">
                                                    <asp:TextBox ID="Txt_MtoSC" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                                </td>
                                                <td width="70px"></td>
                                                <td width="70px">
                                                    <asp:Label ID="lblMtoEn" runat="server" Text="Monto en:"></asp:Label>
                                                </td>
                                                <td width="100px">
                                                    <asp:TextBox ID="Txt_MontML" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                                </td>
                                                <td width="100px">
                                                    <asp:Label ID="LblDescMtoML" runat="server"></asp:Label>
                                                </td>
                                                <td colspan="3" width="100px"></td>
                                            </tr>
                                            <tr>
                                                <td width="80px">
                                                    <asp:Label ID="lblTOTALSC" runat="server" Text="TOTAL S/C: "></asp:Label>
                                                </td>
                                                <td width="100px">
                                                    <asp:TextBox ID="Txt_TotSC" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                                </td>
                                                <td width="70px"></td>
                                                <td width="70px">
                                                    <asp:Label ID="lblTotalEn" runat="server" Text="TOTAL en:" Width="100px"></asp:Label>
                                                </td>
                                                <td width="100px">
                                                    <asp:TextBox ID="Txt_TotML" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                                </td>
                                                <td width="100px">
                                                    <asp:Label ID="LblDescTotML" runat="server"></asp:Label>
                                                </td>
                                                <td colspan="3" width="100px"></td>
                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="7">
                                                    <asp:HiddenField ID="hdn_tari_sTarifarioId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_tari_sNumero" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_tari_vLetra" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdn_tari_sCalculoTipoId" runat="server" Value="0" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                           
                                            
                                            
                                           
                                            
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tab-8">
                                <!-- VINCULACION -->
                                <!-- Body -->
                                <table>
                                </table>
                                <asp:UpdatePanel ID="updFormato" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Button Text="    Grabar" CssClass="btnSave" runat="server" ID="btnGrabarVinculacion"
                                                        OnClick="btnGrabarVinculacion_Click" />
                                                    <asp:HiddenField ID="HIdActuacionDetalleId" runat="server" />
                                                    <uc4:ctrlReimprimirbtn ID="ctrlReimprimirbtn1" runat="server" />
                                                    <uc6:ctrlBajaAutoadhesivo ID="ctrlBajaAutoadhesivo1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblValidacionDetalle" runat="server" CssClass="hideControl" ForeColor="Red"
                                                        Text="Validar Código del Autoadhesivo"></asp:Label>
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
                                                    <asp:TextBox ID="txtAutoadhesivo" runat="server" Width="120px" MaxLength="14" CssClass="txtLetra"
                                                        onkeypress="return isLetraNumeroDoc(event);" TabIndex="10"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hnd_ImpresionCorrecta" Value="0"></asp:HiddenField>
                                                    <asp:Button ID="btnDesabilitarAutoahesivo" runat="server" OnClick="btnDesabilitarAutoahesivo_Click"
                                                        Text="Oculto" Visible="true" CssClass="hideControl" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:Button ID="Btn_ImprimirAutoadhesivo" runat="server" Text="   Autoadhesivo" Width="200px"
                                                        CssClass="btnPrint" TabIndex="9" OnClick="Btn_ImprimirAutoadhesivo_Click" Enabled="False" />
                                                    <asp:HiddenField ID="hCodAutoadhesivo" runat="server" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkImpresion" runat="server" Text="Impresión Correcta"  Enabled="false"
                                                        AutoPostBack="True" OnCheckedChanged="chkImpresion_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                            <div style="margin-left:25%">
                                                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btnLimpiar" 
                                                                    onclick="btnLimpiar_Click"/>
                                                            </div>
                                                        </td>
                                                <td colspan="2" id="trPoderFueraRegistro" runat="server" visible="false">
                                                    <asp:Button ID="Btn_ImprimirPoder" runat="server" Text="       Poder Fuera de Registro"
                                                        Width="200px" CssClass="btnPrint" TabIndex="6" OnClientClick="return VerificarCuerpo();"
                                                        OnClick="Btn_ImprimirPoder_Click" />
                                                </td>
                                                <td id="trAutorizacionViajeMenor" runat="server" colspan="2" visible="false">
                                                    <asp:Button ID="Btn_ImprimirViaje" runat="server" CssClass="btnPrint" OnClientClick="return VerificarCuerpo();" 
                                                        OnClick="Btn_ImprimirViaje_Click" TabIndex="7" Text="      Autorización de viaje"
                                                        Width="200px" />
                                                </td>
                                                <td id="trSupervivencia" runat="server" colspan="2" visible="false">
                                                    <asp:Button ID="Btn_ImprimirSupervivencia" runat="server" Text="Certificado" Width="200px"
                                                        CssClass="btnPrint" TabIndex="8" OnClick="Btn_ImprimirSupervivencia_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td id="trConformidad" runat="server" colspan="2" visible="true">
                                                    <asp:Button ID="btn_ImprimirConformidad" runat="server" Text="   Acta de Conformidad"
                                                        Width="200px" CssClass="btnPrint" TabIndex="9" OnClick="btn_ImprimirConformidad_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelParticipante">
                    <ProgressTemplate>
                        <div id="Background"></div>
                        <div id="Progress">
                            <h6>
                                <p style="text-align: center">
                                </p>
                            </h6>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    <script type="text/javascript" id="campo dinamico">

        

        var iTabTipoActoIndice=0;        
        var iTabParticipanteIndice=1;
        var iTabCuerpoIndice=2;
        var iTabAprobacionPagoIndice=3;
        var iTabVinculacionIndice=4;
        var iTabDigitalizacionIndice=5;
        
       Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);

        function Load() {

            LoadTinyMCE();

            $(function () {
                var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

                $("#accordion").accordion({
                    heightStyle: "content",
                    event: "mousedown",
                    active: activeIndex,

                    activate: function (event, ui) {

                        var index = $(this).children('h3').index(ui.newHeader);
                        $('#<%=hidAccordionIndex.ClientID %>').val(index);
                    }
                });
            });

               
            $("#<%=txtDocumentoNro.ClientID %>").bind("keypress", function (eKd) {
            
                 var valorSeleccionado = $("#<%=ddlRegistroTipoDoc.ClientID %>").val();
                 var hidDocumentoSoloNumero = $("#<%=hidDocumentoSoloNumero.ClientID %>").val();

                 if (hidDocumentoSoloNumero == "1") {
                     return isNumberKey(eKd);
                 }
                 else {
                     return isLetraNumeroDoc(eKd);
                 }
             });

           
            $(function () {
            
                $("#tabs").tabs({
                    activate: function () {
                        var selectedTab = $('#tabs').tabs('option', 'active');
                        $("#MainContent_hdn_SelectedTab").val(selectedTab);

                        funSetSession("anep_iSelectedTabId", selectedTab);


                            if(selectedTab.toString() == iTabCuerpoIndice.toString())
                            {
                                DesactivarAceptacion();
                            }
                            var textoCuerpo = $("#<%=txtCuerpo.ClientID %>").val();
                            var textoAdicional = $("#<%=txtInfoAdicional.ClientID %>").val();
                            var HidentextoAdicional = $("#<%=hInfoAdicional.ClientID %>").val();
                            var HidentextoCuerpo = $("#<%=hCuerpo.ClientID %>").val();

                            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

                            if(tipoActoId=='8031') // Poder Fuera de Registro
                            {   
                                //##########################################################[[[
                                //----adicionado por Pipa, por lo que if($.trim(tinyMCE.editors[0].getContent()) == "") era el culpable de no activar las tab(pestañas)
                                //----31 de actubre 2020 
                                //##########################################################
                                $.trim(tinyMCE.editors[0].setContent(HidentextoCuerpo));
                                $.trim(tinyMCE.editors[1].setContent(HidentextoAdicional));
                                $("#<%=txtCuerpo.ClientID %>").val(HidentextoCuerpo);
                                $("#<%=txtInfoAdicional.ClientID %>").val(HidentextoAdicional);
                                //-------------------se comenta todo los siguiente:
                                /*if($.trim(tinyMCE.editors[0].getContent()) == ""){
                                    $.trim(tinyMCE.editors[0].setContent(HidentextoCuerpo))
                                }
                                if($.trim(tinyMCE.editors[1].getContent()) == ""){
                                    $.trim(tinyMCE.editors[1].setContent(HidentextoAdicional))
                                }
                                if(textoCuerpo == '')
                                {
                                    $("#<%=txtCuerpo.ClientID %>").val(HidentextoCuerpo);
                                }
                                if(textoAdicional == '')
                                {
                                    $("#<%=hInfoAdicional.ClientID %>").val(HidentextoAdicional);
                                }*/
                                //##########################################################]]]
                            }

                            
                        }                   
                });
            });           

            $("#tabs").tabs();


            $("#<%=ddlTipoActoNotarialExtra.ClientID %>").change(function () {
                var tipoActo = $.trim($("#<%= ddlTipoActoNotarialExtra.ClientID %>").val());
            });

        }

        function pageLoad(sender, args) {

            Load();
                     
            var objeto=document.getElementById('divDestinoMenorId');
            if(objeto.style.visibility != "collapse")
                objeto.style.visibility = "collapse";

            var varTipoActoNotarial = $('#<%= ddlTipoActoNotarialExtra.ClientID %>').val();
            if (varTipoActoNotarial != null)
            {
                if(varTipoActoNotarial.toString()=='8033')
                {
                    if(objeto.style.visibility != "visible")
                    objeto.style.visibility = "visible";
                }
            }
            
        }

        function LoadTinyMCE() {
            try {
                tinyMCE.remove();
            }
            catch (e) {
                LoadTinyMCE()
            }

            
         
            tinyMCE.init({
                mode: "textareas",
                editor_selector: "mceEditor",
                content_css : '../Styles/css/tinyMCE.css',
                plugins: [                
                "paste textcolor"
                ],
                menu: {},
                menubar: false,
                toolbar: 'undo redo',
                image_advtab: true,
                language: "es"
            });

        }        
        function disableAllButtons()
        {
            var gridViewID = "<%=grd_Participantes.ClientID %>";
            var gridView = document.getElementById(gridViewID);
            var gridViewControls = gridView.getElementsByTagName("input");
 
            for (i = 0; i < gridViewControls.length; i++)
            {
                // if this input type is button, disable
                if (gridViewControls[i].type == "submit")
                {
                    gridViewControls[i].disabled = true;
                }
            }
        }

        function DesactivarAceptacion()
        {
            var btnGrabarCuerpo = $('#<%= btnGrabarCuerpo.ClientID %>');
            var btnGrabarCuerpoTemporal = $('#<%= btnGrabarCuerpoTemporal.ClientID %>');
            var ddlTamanoLetra = $('#<%= ddlTamanoLetra.ClientID %>');

            var chkAceptacion = $('#chkxAceptacion').attr('disabled', true);
            var btnVistaPrevia = $('#<%= btnVistaPrevia.ClientID %>');
            var Btn_AgregarParticipante =$('#<%= Btn_AgregarParticipante.ClientID %>');
            var ctrlToolBarParticipante =$('#<%= ctrlToolBarParticipante.ClientID %>');
            var bCuerpoGrabado = false; 
            var bCheckAceptado = false; 

            if($('#<%= hdn_bCuerpoGrabado.ClientID %>').val()=="1")
                bCuerpoGrabado=true;

            if($('#<%= hdn_bCheckAceptacion.ClientID %>').val()=="1")
                bCheckAceptado = true;

            if(funGetSession("iOficinaConsularDiferente")=="1")
            {
                 btnGrabarCuerpo.attr('disabled', true);
                chkAceptacion.attr('disabled', true);
                chkAceptacion.attr('checked', bCheckAceptado);
                return;
            }

            if(bCuerpoGrabado && bCheckAceptado)
            {
                btnGrabarCuerpo.attr('disabled', true);
                chkAceptacion.attr('disabled', true);
                chkAceptacion.attr('checked', true);
                btnGrabarCuerpoTemporal.attr('disabled', true);
                btnVistaPrevia.attr('disabled', true);
                Btn_AgregarParticipante.attr('disabled', true);
                $('#MainContent_ctrlToolBarParticipante_btnGrabar').attr('disabled', true);
                $('#MainContent_ctrlToolBarParticipante_btnCancelar').attr('disabled', true);
                $('#MainContent_ctrlToolTipoActo_btnGrabar').attr('disabled', true);

                $("#<%= ddlRegistroTipoParticipante.ClientID %>").attr('disabled', true);
                $("#<%= grd_Participantes.ClientID %>").attr('disabled', true);
                disableAllButtons();
                $("#<%= ddlRegistroTipoDoc.ClientID %>").attr('disabled', true);
                $("#<%= imgBuscar.ClientID %>").attr('disabled', true);
                $("#<%= txtDocumentoNro.ClientID %>").attr('disabled', true);
                $("#<%= BtnCalcularEdad.ClientID %>").attr('disabled', true);
                $("#<%= ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                $("#<%= ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
                $("#<%= ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                $("#<%= ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
                $("#<%= ddlRegistroGenero.ClientID %>").attr('disabled', true);
                $("#<%= chkIncapacitado.ClientID %>").attr('disabled', true);
                $("#<%= txtInfoAdicional.ClientID %>").attr('disabled', true);
                $("#<%= txtCuerpo.ClientID %>").attr('disabled', true);
     
                var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

                if(tipoActoId=='8031')
                {   
                    if(tinyMCE.get('MainContent_txtCuerpo')!=undefined && tinyMCE.get('MainContent_txtCuerpo')!==null){
                        tinyMCE.get('MainContent_txtCuerpo').getBody().setAttribute('contenteditable', false);
                        tinyMCE.get('MainContent_txtInfoAdicional').getBody().setAttribute('contenteditable', false);
                    }
                }
            
            }
            else if(bCuerpoGrabado && !bCheckAceptado)
            {
                btnGrabarCuerpo.attr('disabled', true);
                chkAceptacion.attr('disabled', false);
                chkAceptacion.attr('checked', false);

            }
            else if(!bCuerpoGrabado && bCheckAceptado)
            {
                btnGrabarCuerpo.attr('disabled', false);
                chkAceptacion.attr('disabled', true);
                chkAceptacion.attr('checked', true);

            }
            else if(!bCuerpoGrabado && !bCheckAceptado)        
            {
                btnGrabarCuerpo.attr('disabled', true);
                chkAceptacion.attr('disabled', false);
                chkAceptacion.attr('checked', false);
            }
        }

        function SetTabs(sPaso, tipoActo) {
            $(function () {
                $('#tabs').tabs();
                $('#tabs').justEnableTab(iTabTipoActoIndice);
                $('#tabs').disableTab(iTabParticipanteIndice, true);
                $('#tabs').disableTab(iTabCuerpoIndice, true);
                $('#tabs').disableTab(iTabAprobacionPagoIndice, true);
                $('#tabs').disableTab(iTabVinculacionIndice, true);
                $('#tabs').disableTab(iTabDigitalizacionIndice, true);
            });

            

            $(function () {
                $('#tabs').tabs();
                $('#tabs').justEnableTab(iTabParticipanteIndice);
                $('#tabs').justEnableTab(iTabCuerpoIndice);
                $('#tabs').justEnableTab(iTabAprobacionPagoIndice);
                $('#tabs').justEnableTab(iTabVinculacionIndice);
                $('#tabs').justEnableTab(iTabDigitalizacionIndice);
             
            });

            $(function () {
                $('#tabs').tabs("disable", iTabParticipanteIndice);  
                $('#tabs').tabs("disable", iTabCuerpoIndice);              
                $('#tabs').tabs("disable", iTabAprobacionPagoIndice);
                $('#tabs').tabs("disable", iTabVinculacionIndice);
                $('#tabs').tabs("disable", iTabDigitalizacionIndice);

               
            });

            switch (sPaso.toString()) {

                case "1":
                    $(function () {
                        $('#tabs').tabs();
                        $('#tabs').justEnableTab(iTabParticipanteIndice);

                    });
                    break;

                case "2":
                    $(function () {
                        $('#tabs').tabs();
                        $('#tabs').justEnableTab(iTabParticipanteIndice);
                        $('#tabs').justEnableTab(iTabCuerpoIndice);

                    });
                    break;

                case "3":
                    $(function () {
                        $('#tabs').tabs();
                        $('#tabs').justEnableTab(iTabParticipanteIndice);
                        $('#tabs').justEnableTab(iTabCuerpoIndice);
                        $('#tabs').justEnableTab(iTabAprobacionPagoIndice);

                   });
                    break;

                case "4":
                    $(function () {
                        $('#tabs').tabs();
                        $('#tabs').justEnableTab(iTabParticipanteIndice);
                        $('#tabs').justEnableTab(iTabCuerpoIndice);
                        $('#tabs').justEnableTab(iTabAprobacionPagoIndice);
                        $('#tabs').justEnableTab(iTabVinculacionIndice);

                   });
                    break;

                case "5":
                    $(function () {
                        $('#tabs').tabs();
                        $('#tabs').justEnableTab(iTabParticipanteIndice);
                        $('#tabs').justEnableTab(iTabCuerpoIndice);
                        $('#tabs').justEnableTab(iTabAprobacionPagoIndice);
                        $('#tabs').justEnableTab(iTabVinculacionIndice);
                        $('#tabs').justEnableTab(iTabDigitalizacionIndice);
                  
                    });
                    break;
            }           

            var iTabActual=funGetSession("anep_iSelectedTabId");
            if(iTabActual!='')
            {
                $(function () {
                    $('#tabs').tabs();
                    $('#tabs').enableTab(iTabActual);

                });
            }            

        }

       

        function EnableTabIndex(iTab) {
            $(function () {
                $('#tabs').tabs();
                $('#tabs').enableTab(iTab);
            });
        }

   

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function OcultarTabIndex(iTab) {
            $('#tabs').disableTab(iTab, true);
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function CloseAllAccordionTabs() {
            $('#accordion').accordion({
                heightStyle: "content",
                active: false,
                collapsible: true,
            });
        }

        function ValidarPago() {

            var prm = {};
            var rsp = execute('FrmActuacionNotarialExtraProtocolar.aspx/ExisteActuacion', prm);

            if (rsp.d == "0") {
                showpopupother('a', 'ADJUNTOS', 'No se ha agregado la tarifa consular.', false, 200, 250);
                return;
            }            


            var bolValida = true;            


            bolValida=ucActuacionPagoValidarPago();
          

            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                bolValida = false;
            }
            return bolValida;
        }

        function SetearIntroduccionyConclusionCuerpo(introduccion, conclusion) {

            $("#cuerpoIntroduccion").text(introduccion);
            $("#cuerpoConclusion").text(conclusion);
        }

        function buscarIdiomaGrilla(stridioma) {
            var bolExiste = false;
            
            $("#<%= grd_Participantes.ClientID %> tr:not(:first)").each(function(){                
                    var g_participante = $("td:eq(0)", this).html();
                    if(g_participante == "INTERPRETE"){
                        var g_idioma = $("td:eq(9)",this).html();
                        
                        if(g_idioma == stridioma)
                        {
                            bolExiste = true;
                        }
                        return;
                    }           
                })
            
            return bolExiste; 
        }

        function ValidarActuacionDetalle() {

            var bolValida = true;

            var strTarifa = $.trim($("#<%= Txt_TarifaId.ClientID %>").val());
            var strDescripcion = $.trim($("#<%= Txt_TarifaDescripcion.ClientID %>").val());

            var txtIdTarifa = document.getElementById('<%= Txt_TarifaId.ClientID %>');
            var txtDescripcion = document.getElementById('<%= Txt_TarifaDescripcion.ClientID %>');

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

            return bolValida;
        }


        

        var bIsSelectedChange=false;
        var vCamposAlmacenado = ''; 
        var vPersonaAlmacenada = '';

//        function HabilitarPorParticipante(bSelectedChange)
//        {                       
//            $("#<%= ddlRegistroIdioma.ClientID %>").removeAttr("disabled");
//            var comboGenero = document.getElementById('<%= ddlRegistroGenero.ClientID %>');
//            var tipoParticipanteAnterior= $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val();
//            var tipoParticipanteActual = $("#<%= ddlRegistroTipoParticipante.ClientID %>").val();
//            var tipoActo = $("#<%= ddlTipoActoNotarialExtra.ClientID %>").val();
//            var Acto_PodFueReg = $("#<%= hdn_PODER_FUERA_REGISTRO.ClientID %>").val();
//            var Acto_AutViaMen = $("#<%= hdn_AUTORIZACION_VIAJE_MENOR.ClientID %>").val();

//            var Tipo_otorgante = $("#<%= hdn_PARTICIPANTE_OTORGANTE.ClientID %>").val();
//            var Tipo_apoderado = $("#<%= hdn_PARTICIPANTE_APODERADO.ClientID %>").val();
//            var Tipo_menor = $("#<%= hdn_PARTICIPANTE_MENOR.ClientID %>").val();
//            var Tipo_padre = $("#<%= hdn_PARTICIPANTE_PADRE.ClientID %>").val();
//            var Tipo_madre = $("#<%= hdn_PARTICIPANTE_MADRE.ClientID %>").val();
//            var Tipo_interprete = $("#<%= hdn_PARTICIPANTE_INTERPRETE.ClientID %>").val();
//            var Tipo_acompanante = $("#<%= hdn_PARTICIPANTE_ACOMPANANTE.ClientID %>").val();
//            var Tipo_testigo_ruego = $("#<%= hdn_PARTICIPANTE_TESTIGO_A_RUEGO.ClientID %>").val();
//            var Tipo_recurrente = $("#<%= hdn_PARTICIPANTE_RECURRENTE.ClientID %>").val();
//            var tipo_titular = $("#<%= hdn_PARTICIPANTE_TITULAR.ClientID %>").val();

//            var tipo_genero_masculino  = $("#<%= hdn_GENERO_MASCULINO.ClientID %>").val();
//            var tipo_genero_femenino  = $("#<%= hdn_GENERO_FEMENINO.ClientID %>").val();
//            
//            $("#<%= lblIncapacidadFirmar.ClientID %>").html('');

//            

//            if(tipoParticipanteActual==Tipo_testigo_ruego)
//            {
//                if($("#<%= ddlOtorganteIncapacitado.ClientID %> > option").length==1)
//                {
//                    if(Acto_PodFueReg==tipoActo){
//                    showpopupother('a', 'Participantes', 'No se ha Registrado un Otorgante con incapacidad de firmar.', false, 200, 250);
//                    $("#<%= ddlRegistroTipoParticipante.ClientID %>").val(tipoParticipanteAnterior);
//                    return;
//                    }
//                    else  if(Acto_AutViaMen==tipoActo){
//                         showpopupother('a', 'Participantes', 'No se ha Registrado el Padre con incapacidad de firmar.', false, 200, 250);
//                        $("#<%= ddlRegistroTipoParticipante.ClientID %>").val(tipoParticipanteAnterior);
//                        return;
//                    }
//                }
//            }


//            if($("#<%= hdn_Tipo_Participante_Editando.ClientID %>").val() == Tipo_otorgante &&
//            tipoParticipanteActual==Tipo_testigo_ruego)
//            {
//                    showpopupother('a', 'Participantes', 'No se puede actualizar de Otorgante a Testigo a Ruego.', false, 200, 250);
//                    $("#<%= ddlRegistroTipoParticipante.ClientID %>").val(tipoParticipanteAnterior);
//                    return;
//            }


//            $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val($("#<%= ddlRegistroTipoParticipante.ClientID %>").val())

//            LimpiarFaltantes();

//            bIsSelectedChange =bSelectedChange;

//            vCamposAlmacenado=funGetSession("vCamposPersonaAlmacenada");
//            vPersonaAlmacenada=funGetSession("PersonaAlmacenada");

//            var tipoActoId = $.trim($("#<%= ddlRegistroTipoParticipante.ClientID %>").val());

//            $("#<%= ddlRegistroIdioma.ClientID %> option[value="+ $("#<%= hdn_NOTARIA_IDIOMA.ClientID %>").val() +"]").attr('disabled',false);    

//            if(tipoActoId=="0") //Otorgante
//            {
//                DeshabilitarControles(false,false, false, false, false, false, false, false, false, false, false, false, true);
//                EtiquetasObligatoriasPorParticipante(false,false, false, false, false, false, false, false, false, false, false, false, false);
//            }
//            
//            if(tipoActoId==Tipo_apoderado && tipoActo == Acto_AutViaMen) //Apoderado
//            {
//                var iParticipanteEditando = funGetSession('iParticipanteEditando');

//                $('#lblNroEscritura').toggle(true);
//                
//                if (iParticipanteEditando != "1")
//                {
//                    $("#<%=txtNumeroEscritura.ClientID %>").val("");
//                    $("#<%=txtNumeroPartida.ClientID %>").val("");
//                }
//                $("#<%= txtNumeroEscritura.ClientID %>").css('display','block');                
//                
//                $('#lblPartida').toggle(true);
//                
//                $("#<%= txtNumeroPartida.ClientID %>").css('display','block');         
//            }
//            else
//            {
//                $('#lblNroEscritura').toggle(false);

//                $("#<%=txtNumeroEscritura.ClientID %>").val("");
//                $("#<%= txtNumeroEscritura.ClientID %>").css('display','none');                         

//                $('#lblPartida').toggle(false);
//                $("#<%=txtNumeroPartida.ClientID %>").val("");
//                $("#<%= txtNumeroPartida.ClientID %>").css('display','none');         
//            }

//            if(tipoActoId==Tipo_padre || tipoActoId==Tipo_madre)
//            {
//                $('#lblIncapacitadoTitulo').toggle(true);                
//                $("#<%= chkIncapacitado.ClientID %>").css('display','block');                  

//                $("#<%= lblIncapacidadFirmar.ClientID %>").css('display','block');  
//                $("#<%= txtRegistroTipoIncapacidad.ClientID %>").css('display','block');                                                    
//            }
//            else
//            {
//                $('#lblIncapacitadoTitulo').toggle(false);                
//                $("#<%= chkIncapacitado.ClientID %>").css('display','none');                  
//                $("#<%= lblIncapacidadFirmar.ClientID %>").css('display','none');  
//                $("#<%= txtRegistroTipoIncapacidad.ClientID %>").css('display','none');                                  
//            }
//           

//            if(tipoActoId==Tipo_otorgante) //Otorgante
//            {
//                DeshabilitarControles(true,false, false, false, false, true, false, false, false, false, false, false, true);
//                EtiquetasObligatoriasPorParticipante(false,true, true, true, true, false, true, true, true, true, true, true, false);         
//                $('#lblIncapacitadoTitulo').toggle(true);                
//                $("#<%= chkIncapacitado.ClientID %>").css('display','block');                  

//                $("#<%= lblIncapacidadFirmar.ClientID %>").css('display','block');  
//                $("#<%= txtRegistroTipoIncapacidad.ClientID %>").css('display','block');                                                    

//            }           
//            else if(tipoActoId==Tipo_apoderado) //Apoderado
//            {
//               DeshabilitarControles(true,false, false, false, false, true, false, false, false, false, false, true, true);
//               EtiquetasObligatoriasPorParticipante(false,true, true, true, true, false, true, true, true, true, false, false, false);
//            }
//            else if(tipoActoId==Tipo_padre) //Padre 
//            {
//                DeshabilitarControles(true,false, false, false, false, true, false, false, false, false, false, false, true);
//                EtiquetasObligatoriasPorParticipante(false,false, true, true, true, false, true, true, true, true, true, true, false);

//                if($("#<%= ddlRegistroGenero.ClientID %>").val()=="0")
//                {
//                  $("#<%= ddlRegistroGenero.ClientID %>").val(tipo_genero_masculino);
//                  $("#<%= ddlRegistroGenero.ClientID %>").toggle(true);
//                }
//            }

//             else if (tipoActoId==Tipo_madre) // Madre
//            {
//                DeshabilitarControles(true,true, false, false, false, true, false, false, false, false, false, false, true);
//                EtiquetasObligatoriasPorParticipante(false,false, true, true, true, false, true, true, true, true, true, true, false);

//                if($("#<%= ddlRegistroGenero.ClientID %>").val()=="0")
//                {

//                    $("#<%= ddlRegistroGenero.ClientID %>").val(tipo_genero_femenino);
//                }

//                    $("#<%= ddlRegistroGenero.ClientID %>").toggle(true);

//            }
//            else if(tipoActoId==Tipo_menor) //Menor
//            {
//                DeshabilitarControles(false,false, true, true, true, true, true, true, true, true, true, true, true);
//                EtiquetasObligatoriasPorParticipante(true,true, false, false, false, false, false, false, false, false, false, false, false);
//            }
//            else if(tipoActoId==Tipo_interprete) //Intérprete
//            {
//                DeshabilitarControles(true,false, false, false, false, true, false, false, false, false, false, true, true);
//                EtiquetasObligatoriasPorParticipante(false,true, true, true, true, false, true, true, true, true, true, false, false);  
//                var v_castellano = "10549";
//                var bDistintoCastellano = false;
//                var g_idioma_nuevo;

//                $("#<%= grd_Participantes.ClientID %> tr:not(:first)").each(function(){                
//                    var g_participante = $("td:eq(0)", this).html();
//                    if(g_participante == "OTORGANTE" || g_participante == "PADRE" || g_participante == "MADRE"){
//                        var g_idioma = $("td:eq(9)",this).html();
//                        if(g_idioma != v_castellano)
//                        {
//                            if (buscarIdiomaGrilla(g_idioma) == false)
//                            {
//                                bDistintoCastellano = true;
//                                g_idioma_nuevo = g_idioma;
//                            }
//                        }
//                        return;
//                    }           
//                })

//                if(bDistintoCastellano)
//                {
//                    $("#<%= ddlRegistroIdioma.ClientID %>").val(g_idioma_nuevo);
//                    $("#<%= ddlRegistroIdioma.ClientID %>").attr("disabled", "disabled");                    
//                }
//                else{
//                }
//                

//                $("#<%= ddlRegistroIdioma.ClientID %> option[value="+ $("#<%= hdn_NOTARIA_IDIOMA.ClientID %>").val() +"]").attr('disabled',true);             
//                
//            }
//            else if(tipoActoId==tipo_titular) //Titular
//            {
//                DeshabilitarControles(false,false, false, false, false, true, false, false, false, false, true, true, true);
//                EtiquetasObligatoriasPorParticipante(false,true, true, true, true, false, true, true, true, false, false, false, false);                                                                   
//            }
//            else if(tipoActoId==Tipo_acompanante) //Acompañante
//            {
//                DeshabilitarControles(true,false, true, true, true, true, true, true, true, true, true, true, true);
//                EtiquetasObligatoriasPorParticipante(false,true, false, false, false, false, false, false, false, false, false, false, false);
//            }
//            else if(tipoActoId==Tipo_testigo_ruego) //Testigo a Ruego
//            {
//                DeshabilitarControles(true,false, false, false, false, true, false, false, false, false, false, true, false);
//                EtiquetasObligatoriasPorParticipante(false,true, true, true, true, false, true, true, true, true, true, false, true);
//            }

//            var sNacionalidadId = $("#<%= ddlRegistroNacionalidad.ClientID %>").val();

//            if(sNacionalidadId==0){
//                $("#<%= ddlRegistroNacionalidad.ClientID %>").attr('disabled', false);
//            }


//            if (tipoParticipanteActual == Tipo_otorgante || tipoParticipanteActual == Tipo_padre || tipoParticipanteActual == Tipo_madre)
//            {
//                    if (comboGenero.selectedIndex > 0) {
//                        if (comboGenero.options[comboGenero.selectedIndex].text == 'MASCULINO') {
//                            $("#<%= lblIncapacidadFirmar.ClientID %>").html('Incapacitado de firmar por ser:');                    
//                        }
//                        else {
//                            $("#<%= lblIncapacidadFirmar.ClientID %>").html('Incapacitada de firmar por ser:');
//                        }
//                    }
//                    else {
//                        $("#<%= lblIncapacidadFirmar.ClientID %>").html('Incapacitado de firmar por ser:');
//                    }                       
//                 funIncapacidadCheckSelected(true);    
//            }
//            else
//            {
//                $("#<%= lblIncapacidadFirmar.ClientID %>").html('');
//            }            
//        }


//        function ValidarAgregarParticipante()
//        {
//            var tipoActoId = $.trim($("#<%= ddlRegistroTipoParticipante.ClientID %>").val());
//            var tipoActo = $("#<%= ddlTipoActoNotarialExtra.ClientID %>").val();                        
//            var Tipo_otorgante = $("#<%= hdn_PARTICIPANTE_OTORGANTE.ClientID %>").val();
//            var Tipo_apoderado = $("#<%= hdn_PARTICIPANTE_APODERADO.ClientID %>").val();
//            var Tipo_menor = $("#<%= hdn_PARTICIPANTE_MENOR.ClientID %>").val();
//            var Tipo_padre = $("#<%= hdn_PARTICIPANTE_PADRE.ClientID %>").val();
//            var Tipo_madre = $("#<%= hdn_PARTICIPANTE_MADRE.ClientID %>").val();
//            var Tipo_interprete = $("#<%= hdn_PARTICIPANTE_INTERPRETE.ClientID %>").val();
//            var Tipo_acompanante = $("#<%= hdn_PARTICIPANTE_ACOMPANANTE.ClientID %>").val();
//            var Tipo_testigo_ruego = $("#<%= hdn_PARTICIPANTE_TESTIGO_A_RUEGO.ClientID %>").val();
//            var Tipo_recurrente = $("#<%= hdn_PARTICIPANTE_RECURRENTE.ClientID %>").val();
//            var tipo_titular = $("#<%= hdn_PARTICIPANTE_TITULAR.ClientID %>").val();
//            var Acto_AutViaMen = $("#<%= hdn_AUTORIZACION_VIAJE_MENOR.ClientID %>").val();

//            if (tipoActoId == "0") {
//                $("#<%=ddlRegistroTipoParticipante.ClientID %>").focus();
//                $("#<%=ddlRegistroTipoParticipante.ClientID %>").css("border", "solid Red 1px");
//                return false;
//                }
//                else {
//                    $("#<%=ddlRegistroTipoParticipante.ClientID %>").css("border", "solid #888888 1px");
//                }


//            var bolValida=true;

//            if(tipoActoId==Tipo_otorgante) //Otorgante
//            {
//                bolValida=ValidarPorParticipante(false,true, true, true, true, false, true, true, true, true, true, true, false, false, false);
//            }
//            else if(tipoActoId==Tipo_apoderado) // Apoderado
//            {
//                if (tipoActo == Acto_AutViaMen)
//                {    
//                    bolValida=ValidarPorParticipante(false,true, true, true, true, false, true, true, true, true, false, false, false, true, true);
//                }
//                else
//                {
//                    bolValida=ValidarPorParticipante(false,true, false, false, false, false, false, false, false, false, false, false, false, false, false);
//                }
//            }
//            else if(tipoActoId==Tipo_padre) //Padre
//            {
//                bolValida=ValidarPorParticipante(false,false, true, true, true, false, true, true, true, true, true, true, false, false, false);
//            }
//            else if(tipoActoId==Tipo_madre) //Madre
//            {
//                bolValida=ValidarPorParticipante(false,false, true, true, true, false, true, true, true, true, true, true, false, false, false);
//            }
//            else if(tipoActoId==Tipo_menor) //Menor
//            {
//                bolValida=ValidarPorParticipante(true,true, false, false, false, false, false, false, false, false, false, false, false, false, false);
//                
//            }
//            else if(tipoActoId==Tipo_interprete) //Intérprete
//            {
//                bolValida=ValidarPorParticipante(false,true, true, true, true, false, true, true, true, true, true, false, false, false, false);
//                
//            }
//            else if(tipoActoId==tipo_titular) //Titular
//            {
//                bolValida=ValidarPorParticipante(false,true, true, true, true, false, true, true, true, false, false, false, false, false, false);
//                
//            }
//            else if(tipoActoId==Tipo_acompanante) //Acompañante
//            {
//                bolValida=ValidarPorParticipante(false,true, false, false, false, false, false, false, false, false, false, false, false, false, false);
//                
//            }
//            else if(tipoActoId==Tipo_testigo_ruego) //Testigo a Ruego
//            {
//                bolValida=ValidarPorParticipante(false,true, true, true, true, false, true, true, true, true, true, false, true, false, false);               
//            }

//            return bolValida;
//         
//        }

//        function ValidarPorParticipante(bFecha, bGenero, bEstadoCivil, bNacionalidad, bDomicilio, bCodPostal, bDepartamento, bProvincia, bDistrito, bProfesion, bIdioma, bTipoIncapacidad, bOtorganteIncapacitado, bNumeroEscritura, bNumeroPartida)
//        {
//            var bolValida = true;
//            
//            bolValida= ValidarControl($('#<%= ddlRegistroTipoDoc.ClientID  %>'),'0');
//            bolValida= ValidarControl($('#<%= txtDocumentoNro.ClientID  %>'),'');
//            bolValida= ValidarControl($('#<%= txtApePaterno.ClientID  %>'),'');
//            bolValida= ValidarControl($('#<%= txtApeMaterno.ClientID  %>'),'');
//            bolValida= ValidarControl($('#<%= txtNombres.ClientID  %>'),'');

//            //bolValida= ValidarControl($('#<%= ddlPaisOrigen.ClientID  %>'),'0');

//            if(bFecha)
//            {
//                var strFecNac = $.trim($('#<%= txtFecNac.FindControl("TxtFecha").ClientID  %>').val());
//                var txtFecNac = document.getElementById('<%= txtFecNac.FindControl("TxtFecha").ClientID  %>');
//                if (strFecNac == "") {
//                    bolValida = false;
//                    txtFecNac.style.border = "1px solid Red";
//                }
//                else {
//                    txtFecNac.style.border = "1px solid #888888";
//                }
//            }

//            if(bGenero)
//            {
//                var strGenero = $.trim($("#<%= ddlRegistroGenero.ClientID %>").val());

//                if (strGenero == "0") {
//                    $("#<%=ddlRegistroGenero.ClientID %>").focus();
//                    $("#<%=ddlRegistroGenero.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddlRegistroGenero.ClientID %>").css("border", "solid #888888 1px");
//                }

//            }

//            if(bEstadoCivil)
//            {
//                var strRegistroEstadoCivilExtra = $.trim($("#<%= ddlRegistroEstadoCivil.ClientID %>").val());

//                if (strRegistroEstadoCivilExtra == "0") {
//                    $("#<%=ddlRegistroEstadoCivil.ClientID %>").focus();
//                    $("#<%=ddlRegistroEstadoCivil.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddlRegistroEstadoCivil.ClientID %>").css("border", "solid #888888 1px");
//                }

//            }

//            if(bNacionalidad)
//            {
//                var strRegistroNacionalidad = $.trim($("#<%= ddlPaisOrigen.ClientID %>").val());

//                if (strRegistroNacionalidad == "0") {
//                $("#<%=ddlPaisOrigen.ClientID %>").focus();
//                $("#<%=ddlPaisOrigen.ClientID %>").css("border", "solid Red 1px");
//                bolValida = false;
//                }
//                else {
//                    $("#<%=ddlPaisOrigen.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if(bDomicilio)
//            {
//                var strRegistroDireccion = $.trim($("#<%= txtDireccion.ClientID %>").val());

//                if (strRegistroDireccion == "") {
//                    $("#<%=txtDireccion.ClientID %>").focus();
//                    $("#<%=txtDireccion.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=txtDireccion.ClientID %>").css("border", "solid #888888 1px");
//                }

//            }

//            if(bCodPostal)
//            {
//            }

//            if(bDepartamento)
//            {                
//                var strUbigeoPais = $.trim($("#<%= ddl_UbigeoPais.ClientID %>").val());

//                if (strUbigeoPais == "0") {
//                    $("#<%=ddl_UbigeoPais.ClientID %>").focus();
//                    $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if(bProvincia)
//            {
//                var strUbigeoRegion = $.trim($("#<%= ddl_UbigeoRegion.ClientID %>").val());

//                if (strUbigeoRegion == "0") {
//                    $("#<%=ddl_UbigeoRegion.ClientID %>").focus();
//                    $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if(bDistrito)
//            {
//                var strUbigeoCiudad = $.trim($("#<%= ddl_UbigeoCiudad.ClientID %>").val());

//                if (strUbigeoCiudad == "0") {
//                    $("#<%=ddl_UbigeoCiudad.ClientID %>").focus();
//                    $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if(bProfesion)
//            {
//                var strProfesion = $.trim($("#<%= ddlRegistroProfesion.ClientID %>").val());

//                if (strProfesion == "0") {
//                    $("#<%=ddlRegistroProfesion.ClientID %>").focus();
//                    $("#<%=ddlRegistroProfesion.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddlRegistroProfesion.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if(bIdioma)
//            {
//                var strIdioma = $.trim($("#<%= ddlRegistroIdioma.ClientID %>").val());

//                if (strIdioma == "0") {
//                    $("#<%=ddlRegistroIdioma.ClientID %>").focus();
//                    $("#<%=ddlRegistroIdioma.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddlRegistroIdioma.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }



//            if(bTipoIncapacidad)
//            {
//                if($("#<%=chkIncapacitado.ClientID %>")[0].checked)
//                {
//                    var strTipoIncapacidad = $.trim($("#<%= txtRegistroTipoIncapacidad.ClientID %>").val());

//                    if (strTipoIncapacidad == "") {
//                        $("#<%=txtRegistroTipoIncapacidad.ClientID %>").focus();
//                        $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid Red 1px");
//                        bolValida = false;
//                    }
//                    else {
//                        $("#<%=txtRegistroTipoIncapacidad.ClientID %>").focus();
//                        $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid #888888 1px");
//                    }
//                }
//            }

//            if(bOtorganteIncapacitado)
//            {
//                var strOtorganteIncapacitado = $.trim($("#<%= ddlOtorganteIncapacitado.ClientID %>").val());

//                if (strOtorganteIncapacitado == "0") {
//                    $("#<%=ddlOtorganteIncapacitado.ClientID %>").focus();
//                    $("#<%=ddlOtorganteIncapacitado.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                }
//                else {
//                    $("#<%=ddlOtorganteIncapacitado.ClientID %>").css("border", "solid #888888 1px");
//                }
//            }

//            if (bNumeroEscritura)
//            {
//                 var strNumeroEscritura = $.trim($("#<%= txtNumeroEscritura.ClientID %>").val());
//                
//                 if (strNumeroEscritura == "") {
//                    $("#<%=txtNumeroEscritura.ClientID %>").focus();
//                    $("#<%=txtNumeroEscritura.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                 }
//                 else
//                 {
//                    $("#<%=txtNumeroEscritura.ClientID %>").focus();
//                    $("#<%=txtNumeroEscritura.ClientID %>").css("border", "solid #888888 1px");
//                 }
//            }

//            if (bNumeroPartida)
//            {
//                 var strNumeroPartida = $.trim($("#<%= txtNumeroPartida.ClientID %>").val());

//                 if (strNumeroPartida == "")
//                 {
//                    $("#<%=txtNumeroPartida.ClientID %>").focus();
//                    $("#<%=txtNumeroPartida.ClientID %>").css("border", "solid Red 1px");
//                    bolValida = false;
//                 }
//                 else
//                 {
//                    $("#<%=txtNumeroPartida.ClientID %>").focus();
//                    $("#<%=txtNumeroPartida.ClientID %>").css("border", "solid #888888 1px");
//                 }
//            }


//            return bolValida;
//        }


//        function ValidarControl(control,controlInitialValue)
//        {
//            var strControlValor = $.trim(control.val());

//                if (strControlValor == controlInitialValue) {
//                control.css("border", "solid Red 1px");
//                return false;
//                }
//                else {
//                    control.css("border", "solid #888888 1px");
//                    
//                }

//            return true;
//        }

//       function EtiquetasObligatoriasPorParticipante(bFecha, bGenero, bEstadoCivil, bNacionalidad, bDomicilio, bCodPostal, bDepartamento, bProvincia, bDistrito, bProfesion, bIdioma, bTipoIncapacidad, bOtorganteIncapacitado)
//       {

//            
//            $("#lblFechaNacObl").toggle(bFecha);

//            $("#lblEstadoCivilObl").toggle(bEstadoCivil);
//            $("#lblNacionalidadObl").toggle(bNacionalidad);
//            $("#lblDireccionObl").toggle(bDomicilio);

//            $("#lblUbigeoPaisObl").toggle(bDepartamento);
//            $("#lblUbigeoRegionObl").toggle(bProvincia);
//            $("#lblUbigeoCiudadObl").toggle(bDistrito);
//            $("#lblProfesionObl").toggle(bProfesion);
//            $("#lblIdiomaObl").toggle(bIdioma);
//            $("#lblTipoIncapacidadObl").toggle(bTipoIncapacidad);

//            $("#lblOtorganteIncapacitadoObl").toggle(bOtorganteIncapacitado);

//           
//       }

//       function EsCampoPersonaAlmacenada(campo)
//       {
//            
//            if(vCamposAlmacenado.indexOf(campo)>=0)
//                return true;
//            else
//                return false;
//       }

//        function DeshabilitarControles(bFecha, bGenero, bEstadoCivil, bNacionalidad, bDomicilio, bCodPostal, bDepartamento, bProvincia, bDistrito, bProfesion, bIdioma, bTipoIncapacidad, bOtorganteIncapacitado)
//        {
//            
//            
//            //DeshabilitarControlFecha(bFecha, EsCampoPersonaAlmacenada('nacimiento'));
//            DeshabilitarControlFecha(bFecha, false);

////            DeshabilitarControl(bGenero,$('#<%= ddlRegistroGenero.ClientID  %>'), 0,EsCampoPersonaAlmacenada('genero'));
//            
////            DeshabilitarControl(bEstadoCivil,$('#<%= ddlRegistroEstadoCivil.ClientID  %>'), 0, EsCampoPersonaAlmacenada('estadocivil'));
////            DeshabilitarControl(bEstadoCivil,$('#lblEstadoCivilTitulo'), 1);


////            DeshabilitarControl(bProfesion,$('#<%= ddlRegistroProfesion.ClientID  %>'), 0, EsCampoPersonaAlmacenada('profesion'));
////            DeshabilitarControl(bProfesion,$('#lblProfesionTitulo'), 1);

//            DeshabilitarControl(bOtorganteIncapacitado,$('#<%= ddlOtorganteIncapacitado.ClientID  %>'), 0, EsCampoPersonaAlmacenada('otorganteincapacitado'));
//            DeshabilitarControl(bOtorganteIncapacitado,$('#lblOtorganteIncapacitadoTitulo'), 1);

//                    
//            DeshabilitarControl(bTipoIncapacidad,$('#<%= chkIncapacitado.ClientID  %>'), 2, EsCampoPersonaAlmacenada('incapacidad'));
//            DeshabilitarControl(bTipoIncapacidad,$('#<%= chkNoHuella.ClientID  %>'), 2);

//            DeshabilitarControl(bTipoIncapacidad,$('#lblIncapacitadoTitulo'), 1);

//            DeshabilitarControl(bTipoIncapacidad,$('#<%= txtRegistroTipoIncapacidad.ClientID  %>'), 1, EsCampoPersonaAlmacenada('incapacidad'));
//                       

//            var chkIncapacitado = $('#<%=chkIncapacitado.ClientID %>');


//            funIncapacidadCheckSelected(false);
//                                  

////            DeshabilitarControl(bIdioma,$('#<%= ddlRegistroIdioma.ClientID  %>'), 0, false);
////            DeshabilitarControl(bIdioma,$('#lblIdiomaTitulo'), 1);


//            if($('#<%= ddl_UbigeoPais.ClientID  %>').is(":visible")==false &&
//            $('#<%= ddl_UbigeoRegion.ClientID  %>').is(":visible")==false &&
//            $('#<%= ddl_UbigeoCiudad.ClientID  %>').is(":visible")==false)
//            {
//                $('#dtDireccionTitulo').toggle(false);
//            }
//            else
//            {
//                $('#dtDireccionTitulo').toggle(true);
//            }

//            if($('#<%= ddlRegistroProfesion.ClientID  %>').is(":visible")==false &&
//            $('#<%= ddlRegistroIdioma.ClientID  %>').is(":visible")==false)
//            {
//                $('#dtDireccionOtros').toggle(false);
//            }
//            else
//            {
//                $('#dtDireccionOtros').toggle(true);
//            }
//            
//           
//        }

//        function LimpiarFaltantes()
//        {
//            $('#<%= ddlRegistroTipoDoc.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtDocumentoNro.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtApePaterno.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtApeMaterno.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtNombres.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddlRegistroGenero.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddlRegistroEstadoCivil.ClientID  %>').css("border", "solid #888888 1px");

////            $('#<%= ddlRegistroNacionalidad.ClientID  %>').css("border", "solid #888888 1px");

//             $('#<%= txtRegistroTipoIncapacidad.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtDireccion.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= txtCodigoPostal.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddl_UbigeoPais.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddl_UbigeoRegion.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddl_UbigeoCiudad.ClientID  %>').css("border", "solid #888888 1px");

//            $('#<%= ddlRegistroProfesion.ClientID  %>').css("border", "solid #888888 1px");

//           $('#<%= ddlRegistroIdioma.ClientID  %>').css("border", "solid #888888 1px");

//           $('#<%= ddlOtorganteIncapacitado.ClientID  %>').css("border", "solid #888888 1px");
//        }

    

//        function DeshabilitarControl(bAccion, control, iControlTipo, bCampoAlmacenado)
//        {
//            //iTipoControl == 0 => Combo
//            //iTipoControl == 1 => TextBox


//            var vControlValorBase = "0";

//            if(iControlTipo==1)
//                vControlValorBase="";


//            var vControlValor = control.val();
//            var bPersonaAlmacenada= false;

//            

//            if(vPersonaAlmacenada=="1")
//            {
//                bPersonaAlmacenada=true;
//            }

//            if(bAccion)           
//            {
//                /*control.prop( "disabled", true );*/             

//                control.toggle(false);
//                if(!bPersonaAlmacenada)
//                {
//                    if(iControlTipo!=2)
//                        control.val(vControlValorBase);
//                    else                    
//                        control[0].checked=false;
//                }
//            }
//            else
//            {
//                control.toggle(true);

//                if(bPersonaAlmacenada)
//                {                   
//                    if(iControlTipo!=2)
//                    {
//                        if(vControlValor != vControlValorBase)
//                        {
//                            if(bCampoAlmacenado)
//                            control.prop( "disabled", true );
//                            else
//                            control.prop( "disabled", false );
//                        }
//                    }
//                }
//                else
//                {
//                    if(iControlTipo!=2)
//                    {
//                        if(vControlValor != vControlValorBase)
//                        {
//                            control.prop( "disabled", false );

//                            if(bIsSelectedChange)
//                            control.val(vControlValorBase);
//                        }
//                    }
//                    else
//                    {
//                        control.prop( "disabled", false );

//                            if(bIsSelectedChange)
//                            control[0].checked=false;
//                    }

//                }

//            }


//        }


//        function DeshabilitarControlFecha(bAccion, bCampoAlmacenado)
//        {
//            //iTipoControl == 0 => Combo
//            //iTipoControl == 1 => TextBox

//            var controlTxt  =   $('#<%= txtFecNac.FindControl("TxtFecha").ClientID  %>');
//            var controlBtn  =   $('#<%= txtFecNac.FindControl("btnFecha").ClientID  %>');

//            var vControlValorBase = "";

//            var vControlValor = controlTxt.val();
//            var bPersonaAlmacenada= false;

//            

//            if(vPersonaAlmacenada=="1")
//            {
//                bPersonaAlmacenada=true;
//            }

//            if(bAccion)           
//            {         

//                controlTxt.toggle(false);
//                controlBtn.toggle(false);
//                $('#lblFechaNacimientoTitulo' ).toggle(false);
////                $('#<%= LblEdad.ClientID  %>' ).toggle(false);
////                $('#<%= LblEdad2.ClientID  %>' ).toggle(false);
////                $('#<%= BtnCalcularEdad.ClientID  %>').toggle(false);

//                if(!bPersonaAlmacenada)
//                {
//                    controlTxt.val(vControlValorBase);
//                }
//            }
//            else
//            {
//                controlTxt.toggle(true);
//                controlBtn.toggle(true);
//                $('#lblFechaNacimientoTitulo' ).toggle(true);
////                $('#<%= LblEdad.ClientID  %>' ).toggle(true);
////                $('#<%= LblEdad2.ClientID  %>' ).toggle(true);
////                $('#<%= BtnCalcularEdad.ClientID  %>').toggle(true);

//                if(bPersonaAlmacenada)
//                {                   
//                    if(vControlValor != vControlValorBase)
//                    {
//                        if(bCampoAlmacenado)
//                        {
//                            controlTxt.prop( "disabled", true );
//                            controlBtn.prop( "disabled", true );
//                        }
//                        else
//                        {
//                            controlTxt.prop( "disabled", false );
//                            controlBtn.prop( "disabled", false );
//                        }
//                    }
//                }
//                else
//                {
//                    if(vControlValor != vControlValorBase)
//                    {
//                        controlTxt.prop( "disabled", false );
//                        controlBtn.prop( "disabled", false );

//                        if(bIsSelectedChange)
//                            controlTxt.val(vControlValorBase);
//                    }

//                }

//            }


//        }

       

        
        function GrabarCuerpo(actoNotarial) {

            var iActoNotarialId = actoNotarial;
            if (iActoNotarialId == "0") {
                showpopupother('a', 'CUERPO', 'No se ha ingresado información al Cuerpo', false, 200, 250);
            }
            else {


                var cuerpo = {};
                cuerpo.ancu_iActoNotarialCuerpoId = $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val();
                cuerpo.ancu_iActoNotarialId = iActoNotarialId;


                cuerpo.ancu_vCuerpo = funGetSession("vDocumentoActualPFR");

                cuerpo.ancu_sUsuarioCreacion = $("#<%= hdn_acno_sUsuarioCreacion.ClientID %>").val();
                cuerpo.ancu_vIPCreacion = $("#<%= hdn_acno_vIPCreacion.ClientID %>").val();
                cuerpo.ancu_sUsuarioModificacion = $("#<%= hdn_acno_sUsuarioModificacion.ClientID %>").val();
                cuerpo.ancu_vIPModificacion = $("#<%= hdn_acno_vIPModificacion.ClientID %>").val();
                cuerpo.acno_sTamanoLetra = $("#<%= ddlTamanoLetra.ClientID %>").val();
                cuerpo.TipoActoNotarial = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());
                
                var isChecked = $("#<%= chkImprimirFirmaTitular1.ClientID %>").prop('checked');
                cuerpo.ImprimirFirma = isChecked;

                if (cuerpo.ancu_iActoNotarialId != 0) {
                    var sndprm = {};
                    sndprm.cuerpo = JSON.stringify(cuerpo);
                    var rsp = execute("FrmActuacionNotarialExtraProtocolar.aspx/insert_cuerpo", sndprm);


                    if (rsp.d != null) {
                        var oResponse = JSON.parse(rsp.d);
                        if (oResponse.Identity != 0 && oResponse.Message == null) {
                            $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val(oResponse.Identity);



                            showpopupother('i', 'CUERPO', 'Registro almacenado correctamente', false, 200, 250);

                            $(function () {
                                $('#tabs').tabs();
                                $('#tabs').enableTab(iTabAprobacionPagoIndice);
                            });
                        }
                    }
                    else {
                        if (oResponse.Identity == 0 && oResponse.Message != null) {
                            alert(oResponse.Message.toString());
                        }
                    }



                }
                else {
                    alert("DEBE GRABARSE EL ACTO NOTARIAL PREVIAMENTE.");
                }
            }
        }

        /*FUNCIONES GENERALES*/
//        function execute(urlmetodo, parametros) {
//            var rsp;
//            $.ajax({
//                url: urlmetodo,
//                type: "POST",
//                data: JSON.stringify(parametros),
//                dataType: "json",
//                contentType: "application/json; charset=utf-8",
//                async: false,
//                cancel: false,
//                success: function (response) {
//                    rsp = response;
//                },
//                failure: function (msg) {
//                    alert(msg);
//                    rsp = msg;
//                },
//                error: function (xhr, status, error) {
//                    alert(error);
//                    rsp = error;
//                }
//            });

//            return rsp;
//        }

        function funSetSession(variable, valor) {
            var url = 'FrmActuacionNotarialExtraProtocolar.aspx/SetSession';
            var prm = {};
            prm.variable = variable;
            prm.valor = valor;
            var rspta = execute(url, prm);
        }

        function funGetSession(variable) {
            var url = 'FrmActuacionNotarialExtraProtocolar.aspx/GetSession';
            var prm = {};
            prm.variable = variable;
            var rspta = execute(url, prm);

            return JSON.parse(JSON.stringify(rspta)).d;
        }

        function ValidarTabRegistro() {

                        
                if ($("#<%= ddlTipoActoNotarialExtra.ClientID %>").val() == "8034") {
                    showpopupother('a', 'EXTRAPROTOCOLAR', 'Este Tipo de Acto no se encuentra disponible.', false, 200, 250);                
                    return false;
                      
                }



            var bolValida = true;
            var strTipoActoNotarialExtra = $.trim($("#<%= ddlTipoActoNotarialExtra.ClientID %>").val());

            

            if (strTipoActoNotarialExtra == "0") {
                $("#<%=ddlTipoActoNotarialExtra.ClientID %>").focus();
                $("#<%=ddlTipoActoNotarialExtra.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTipoActoNotarialExtra.ClientID %>").css("border", "solid #888888 1px");
            }


            var strTipoActoNotarialExtraTexto = $("#<%=ddlTipoActoNotarialExtra.ClientID %> option:selected").text();

            if (strTipoActoNotarialExtraTexto == "AUTORIZACIÓN DE VIAJE DE MENOR")
            {
                var strSubTipoActoNotarialExtra = $.trim($("#<%= ddlSubTipoNotarialExtra.ClientID %>").val());

                if (strSubTipoActoNotarialExtra == "0") {
                    $("#<%=ddlSubTipoNotarialExtra.ClientID %>").focus();
                    $("#<%=ddlSubTipoNotarialExtra.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else
                {
                    $("#<%=ddlSubTipoNotarialExtra.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            var strFuncionario = $.trim($("#<%= ddlFuncionario.ClientID %>").val());
            if (strFuncionario == "0") {
                $("#<%=ddlFuncionario.ClientID %>").focus();
                $("#<%=ddlFuncionario.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlFuncionario.ClientID %>").css("border", "solid #888888 1px");
            }

            totalRows = funGetSession("ParticipanteContainerCount");

   
            var mensaje =   "¿Está seguro de grabar los cambios?";

//            if(totalRows!='' && totalRows!='0')
//                mensaje="Se perderá la información de participantes.\n"+mensaje;

            /*Validación*/
            if (bolValida) {
                $("#<%= lblValidacionRegistro.ClientID %>").hide();
                bolValida = confirm(mensaje);
            }
            else {
                $("#<%= lblValidacionRegistro.ClientID %>").show();
                bolValida = false;
            }

            return bolValida;
        }        

        function HabilitarGrabarCuerpo()
        {
            
            if( $('#chkxAceptacion').prop('checked') ) {
                $('#<%= hdn_bCheckAceptacion.ClientID %>').val("1");    
                $('#<%= btnGrabarCuerpo.ClientID %>').attr('disabled', false);
            }
            else{$('#<%= hdn_bCheckAceptacion.ClientID %>').val("0");
            $('#<%= btnGrabarCuerpo.ClientID %>').attr('disabled', true);}
            
            $('#<%= hdn_bCuerpoGrabado.ClientID %>').val("0");


        }
   
        function DeshabilitarConforme()
        {
            $('#chkxAceptacion').attr('disabled', true);            
        }
       

        function DesHabilitarGrabarCuerpo()
        {
            $('#MainContent_btnGrabarCuerpo').attr('disabled', true);
            $('#MainContent_btnGrabarCuerpoTemporal').attr('disabled', true);
            $('#MainContent_btnVistaPrevia').attr('disabled', true);
            $('#MainContent_Btn_AgregarParticipante').attr('disabled', true);
            $('#MainContent_ctrlToolBarParticipante_btnGrabar').attr('disabled', true);
            $('#MainContent_ctrlToolBarParticipante_btnCancelar').attr('disabled', true);
            $('#MainContent_ctrlToolTipoActo_btnGrabar').attr('disabled', true);
            $('#MainContent_ddlTipoActoNotarialExtra').attr('disabled', true);
            $('#MainContent_ddlCondiciones').attr('disabled', true);
            $('#MainContent_ddlFuncionario').attr('disabled', true);
            $('#MainContent_ddlSubTipoNotarialExtra').attr('disabled', true);
            
            $('#chkxAceptacion').attr('disabled', true);
            //$('#ddlTamanoLetra').attr('disabled', true);

            $("#<%= ddlRegistroTipoParticipante.ClientID %>").attr('disabled', true);
            $("#<%= grd_Participantes.ClientID %>").attr('disabled', true);
            disableAllButtons();
            $("#<%= ddlRegistroTipoDoc.ClientID %>").attr('disabled', true);
            $("#<%= imgBuscar.ClientID %>").attr('disabled', true);
            $("#<%= txtDocumentoNro.ClientID %>").attr('disabled', true);
            $("#<%= BtnCalcularEdad.ClientID %>").attr('disabled', true);
            $("#<%= ddl_UbigeoPais.ClientID %>").attr('disabled', true);
            $("#<%= ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
            $("#<%= ddl_UbigeoPais.ClientID %>").attr('disabled', true);
            $("#<%= ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
            $("#<%= ddlRegistroGenero.ClientID %>").attr('disabled', true);
            $("#<%= chkIncapacitado.ClientID %>").attr('disabled', true);
            $("#<%= txtInfoAdicional.ClientID %>").attr('disabled', true);
            $("#<%= txtCuerpo.ClientID %>").attr('disabled', true);
            
//            tinyMCE.init({
//                mode: "textareas",
//                editor_selector: "mceEditor",
//                content_css : '../Styles/css/tinyMCE.css',
//                plugins: [                
//                "paste textcolor"
//                ],
//                menu: {},
//                menubar: false,
//                toolbar: 'undo redo',
//                image_advtab: true,
//		        readonly : true,
//                language: "es"
//            });
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

            if(tipoActoId=='8031')
            {   
                tinyMCE.get('MainContent_txtCuerpo').getBody().setAttribute('contenteditable', false);
                tinyMCE.get('MainContent_txtInfoAdicional').getBody().setAttribute('contenteditable', false);
            }
            
            $('#MainContent_hdn_bCheckAceptacion').val("1");
            $('#MainContent_hdn_bCuerpoGrabado').val("1");


        }
        function ValidarTabCuerpo() {
            var bolValida = true;
            var validacioOk=false;
    
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

            if(tipoActoId=='8031')
            {  
                var strCuerpo = tinyMCE.editors[0].getContent();
                //var strCuerpoLimpio = $.trim(strCuerpo.replace(/<[^>]*>?/g, '').replace('&nbsp;', ''));
                var strCuerpoLimpio = $.trim(strCuerpo);
                var strInfoAdicional = tinyMCE.editors[1].getContent();
                //strInfoAdicional = $.trim(strInfoAdicional.replace(/<[^>]*>?/g, '').replace('&nbsp;', ''));
                strInfoAdicional = $.trim(strInfoAdicional);

                //##########################################################[[[
                //----adicionado por pipa, pasar en Active Tab
                //----31 de octubre 2020
                //-----------------------------------------------------------
                $("#<%= hCuerpo.ClientID %>").val(strCuerpoLimpio);
                $("#<%= hInfoAdicional.ClientID %>").val(strInfoAdicional);                
                //##########################################################]]]
                if (strCuerpoLimpio.length == 0) {
                    $("#<%=txtCuerpo.ClientID %>").focus();
                    $("#<%=txtCuerpo.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtCuerpo.ClientID %>").css("border", "solid #888888 1px");
                }
            }
            /*Validación*/
            if (bolValida) {

                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {


                showpopupother('a', 'CUERPO', 'No se ha ingresado información al Cuerpo', false, 200, 250);
                bolValida = false;
            }

            if (bolValida) {

              //  GuardarCuerpoeInfoAdicional();


                /* JCAYCHO_C */

                 var iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val(); 
                 if (iActoNotarialId == "0") {
                    showpopupother('a', 'CUERPO', 'No se ha ingresado información al Cuerpo', false, 200, 250);
                    validacioOk= false;
                 }
                else {
                                            var cuerpo = {};
                                            cuerpo.ancu_iActoNotarialCuerpoId = $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val();
                                            cuerpo.ancu_iActoNotarialId = iActoNotarialId;
                                            cuerpo.ancu_vCuerpo = ObtenerCuerpo();
                                            cuerpo.ancu_sUsuarioCreacion = $("#<%= hdn_acno_sUsuarioCreacion.ClientID %>").val();
                                            cuerpo.ancu_vIPCreacion = $("#<%= hdn_acno_vIPCreacion.ClientID %>").val();
                                            cuerpo.ancu_sUsuarioModificacion = $("#<%= hdn_acno_sUsuarioModificacion.ClientID %>").val();
                                            cuerpo.ancu_vIPModificacion = $("#<%= hdn_acno_vIPModificacion.ClientID %>").val();
                                            cuerpo.acno_bFlagLeidoAprobado = $('#chkxAceptacion').prop('checked');
                                            cuerpo.acno_sTamanoLetra = $("#<%= ddlTamanoLetra.ClientID %>").val();
                                            cuerpo.TipoActoNotarial = $.trim($("#<%= ddlTipoActoNotarialExtra.ClientID %>").val());
                    
                                            //---adicionar acno_sOficinaConsularId  ==>pipa
                                            cuerpo.acno_sOficinaConsularId =$.trim($("#<%= hdn_acno_sOficinaConsularId.ClientID %>").val());
                                            //---
                                            var isChecked = $("#<%= chkImprimirFirmaTitular1.ClientID %>").prop('checked');
                                            cuerpo.ImprimirFirma = isChecked;

                                            if (cuerpo.ancu_iActoNotarialId != 0) {
                                                var sndprm = {};
                                                sndprm.cuerpo = JSON.stringify(cuerpo);
                                                var rsp = execute("FrmActuacionNotarialExtraProtocolar.aspx/insert_cuerpo", sndprm);
                        
                                                if (rsp.d != null) {
                                                    var oResponse = JSON.parse(rsp.d);
                                                    if (oResponse.Identity != 0 && oResponse.Message == null) {
                                                        $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val(oResponse.Identity);
                                                            //---adicionar acno_sOficinaConsularId  ==>pipa
                                                            //showpopupother('i', 'CUERPO', 'Registro almacenado correctamente', false, 200, 250);
                                                            alert("Registro almacenado correctamente.");
                                                            $('#tabs').tabs();
                                                            $('#tabs').enableTab(iTabAprobacionPagoIndice);
                                                            DesHabilitarGrabarCuerpo();
                                                            validacioOk= true;
                                                            //-----
                                                    }
                                            }
                                            else {
                                                if (oResponse.Identity == 0 && oResponse.Message != null) {
                                                    showpopupother('a', 'CUERPO', oResponse.Message.toString(), false, 200, 250);
                                                    validacioOk= false;
                                                }
                                            }
                                        }
                                        else {
                                            showpopupother('a', 'CUERPO',"DEBE GRABARSE EL ACTO NOTARIAL PREVIAMENTE.", false, 200, 250);
                                        }
             }
          }
          return validacioOk;
        }


        function ValidarTabCuerpoTemporal() {
            var bolValida = true;
            
    
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

            if(tipoActoId=='8031')
            {
                var strCuerpo = tinyMCE.editors[0].getContent();
                //strCuerpo = "<p>" + $("#<%= txtCuerpo.ClientID %>").val() + "</p>";
                var strCuerpoLimpio = $.trim(strCuerpo);

                if (strCuerpoLimpio.length == 0) {
                    $("#<%=txtCuerpo.ClientID %>").focus();
                    $("#<%=txtCuerpo.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtCuerpo.ClientID %>").css("border", "solid #888888 1px");
                }
            }
            /*Validación*/
            if (bolValida) {

                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {


                showpopupother('a', 'CUERPO', 'No se ha ingresado información al Cuerpo', false, 200, 250);
                bolValida = false;
            }

            if (bolValida) {

              //  GuardarCuerpoeInfoAdicional();


                /* JCAYCHO_C */

                 var iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val(); 
                 if (iActoNotarialId == "0") {
                    showpopupother('a', 'CUERPO', 'No se ha ingresado información al Cuerpo', false, 200, 250);
                    return false;
                 }
                else {
                    var cuerpo = {};
                    cuerpo.ancu_iActoNotarialCuerpoId = $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val();
                    cuerpo.ancu_iActoNotarialId = iActoNotarialId;
                    cuerpo.ancu_vCuerpo = ObtenerCuerpo();
                    cuerpo.ancu_sUsuarioCreacion = $("#<%= hdn_acno_sUsuarioCreacion.ClientID %>").val();
                    cuerpo.ancu_vIPCreacion = $("#<%= hdn_acno_vIPCreacion.ClientID %>").val();
                    cuerpo.ancu_sUsuarioModificacion = $("#<%= hdn_acno_sUsuarioModificacion.ClientID %>").val();
                    cuerpo.ancu_vIPModificacion = $("#<%= hdn_acno_vIPModificacion.ClientID %>").val();
                    cuerpo.acno_bFlagLeidoAprobado = false;
                    cuerpo.acno_sTamanoLetra = $("#<%= ddlTamanoLetra.ClientID %>").val();
                    cuerpo.TipoActoNotarial = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());
                    //---adicionar acno_sOficinaConsularId  ==>pipa
                    cuerpo.acno_sOficinaConsularId =$.trim($("#<%= hdn_acno_sOficinaConsularId.ClientID %>").val());

                    var isChecked = $("#<%= chkImprimirFirmaTitular1.ClientID %>").prop('checked');
                    cuerpo.ImprimirFirma = isChecked;

                    if (cuerpo.ancu_iActoNotarialId != 0) {
                        var sndprm = {};
                        sndprm.cuerpo = JSON.stringify(cuerpo);
                        
                        var rsp = execute("FrmActuacionNotarialExtraProtocolar.aspx/insert_cuerpo", sndprm);
                        if (rsp.d != null) {
                            var oResponse = JSON.parse(rsp.d);
                            if (oResponse.Identity != 0 && oResponse.Message == null) {
                                //showpopupother('i', 'CUERPO', 'Registro almacenado correctamente', false, 200, 250);
                                alert("Registro almacenado correctamente.");
                                $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val(oResponse.Identity);
                                $('#chkxAceptacion').attr('disabled', false);                                            
                            }
                        }
                        else {
                            if (oResponse.Identity == 0 && oResponse.Message != null) {
                                showpopupother('a', 'CUERPO', oResponse.Message.toString(), false, 200, 250);
                                return false;
                        }
                    }
                }
                else {
                    showpopupother('a', 'CUERPO',"DEBE GRABARSE EL ACTO NOTARIAL PREVIAMENTE.", false, 200, 250);
                }
             }
          }
          return false;
        }


        function VerificarCuerpo()
        {
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());

            if(tipoActoId=='8031')
            { 
                seleccionado(); 
                if($.trim(tinyMCE.editors[0].getContent())!=""){
                    GuardarCuerpoeInfoAdicional();
                }
                else{
                    showpopupother('a', 'Notarial', 'Debe de Ingresar el Cuerpo del Documento', false, 200, 250);
                    return false;
                }
            }
            else
            {
                if(tipoActoId=='8032' || tipoActoId=='8033')
                   seleccionado(); 
                

                GuardarCuerpoeInfoAdicional();
            }

                    
            return false;
        }


        function ObtenerCuerpo(){
            
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());
            
            var introduccion = $("#<%=lblcuerpoIntroduccion.ClientID %>").html();

            var strCuerpo = '';
            var conclusion = '';
            var extra = '';
            var TextoCompleto = '';
            if(tipoActoId=='8031')
            {          
                strCuerpo = tinyMCE.editors[0].getContent();
                //strCuerpo = "<p>" + $("#<%= txtCuerpo.ClientID %>").val() + "</p>";
                if($.trim(strCuerpo)!="") {
                    var texto = " ";
                     $(strCuerpo + ' > p').each(function(idx, el) {
                        
                        if($.trim($(el).text())!="" && $.trim($(el).text())!=">" ){
                        
                             texto += "<p style='text-align:justify;'>";
                             texto += $(el).text().toUpperCase();
                             texto += "</p>";
                        }
                     });
                    strCuerpo = texto;
                }

                conclusion = $("#<%=lblcuerpoConclusion.ClientID %>").html();
                //extra = "<p>" + $("#<%= txtInfoAdicional.ClientID %>").val() + "</p>";
                extra = tinyMCE.editors[1].getContent();


                if($.trim(extra)!=""){
                     var textoExtra = " ";
                     $(extra + ' > p').each(function(idx, el) {
                        if($.trim($(el).text())!="" && $.trim($(el).text())!=">"){
                             textoExtra += "<p style='text-align:justify;'>";
                             textoExtra += $(el).text().toUpperCase();
                             textoExtra += "</p>";
                        }
                     });
                    extra = textoExtra;
                }
             
             
             TextoCompleto = introduccion + "<tab></tab>" + strCuerpo + "<tab></tab>" + conclusion + "<tab></tab>" + extra;
                
            }
            else{
                 

                TextoCompleto = introduccion;
            }

            return TextoCompleto;
        }
        function GuardarCuerpoeInfoAdicional() {
      
            var tipoActoId = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());
            
            var introduccion = $("#<%=lblcuerpoIntroduccion.ClientID %>").html();

            var strCuerpo = '';
            var conclusion = '';
            var extra = '';

            var TextoCompleto = '';
            if(tipoActoId=='8031') // Poder Fuera de Registro
            {          
             
                //strCuerpo = "<p>" + $("#<%= txtCuerpo.ClientID %>").val() + "</p>";
                strCuerpo = tinyMCE.editors[0].getContent();
                if($.trim(strCuerpo)!="") {
                    var texto = " ";
                     $(strCuerpo + ' > p').each(function(idx, el) {
                        
                        if($.trim($(el).text())!="" && $.trim($(el).text())!=">" ){
                        
                             texto += "<p style='text-align:justify;'>";
                             texto += $(el).text().toUpperCase();
                             texto += "</p>";
                        }
                     });
                    strCuerpo = texto;
                }

                conclusion = $("#<%=lblcuerpoConclusion.ClientID %>").html();
                //extra = "<p>" + $("#<%= txtInfoAdicional.ClientID %>").val() + "</p>";
                extra = tinyMCE.editors[1].getContent();

                if($.trim(extra)!=""){
                     var textoExtra = " ";
                     $(extra + ' > p').each(function(idx, el) {
                        if($.trim($(el).text())!="" && $.trim($(el).text())!=">"){
                             textoExtra += "<p style='text-align:justify;'>";
                             textoExtra += $(el).text().toUpperCase();
                             textoExtra += "</p>";
                        }
                     });
                    extra = textoExtra;
                }
             
             
             TextoCompleto = introduccion + "<tab></tab>" + strCuerpo + "<tab></tab>" + conclusion + "<tab></tab>" + extra;
           
                
            }
            else{

                 TextoCompleto = introduccion;
            }

           
            var prm = {};
            prm.TipoActa = $.trim($("#<%=ddlTipoActoNotarialExtra.ClientID %>").val());
            prm.iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val(); 
            prm.cuerpo = TextoCompleto;
            var isChecked = $("#<%= chkImprimirFirmaTitular1.ClientID %>").prop('checked');
            prm.ImprimirFirma = isChecked;


         $.ajax({
            type: "POST",
            url: "FrmActuacionNotarialExtraProtocolar.aspx/VistaPrevia",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(prm),
            dataType: "json",
            success: function (data) {
             
                if (data.d == "OK") {
                    $("#<%= btnGrabarCuerpoTemporal.ClientID %>").prop('disabled', false);
                    var strUrl = "../Accesorios/VisorPDF.aspx";
                    window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                } else {
                    showdialog('a', 'Registro Notarial : Cuerpo', data.d, false, 160, 300);
                }

            },
            failure: function (response) {
                showdialog('e', 'Registro Notarial : Cuerpo',response.d, false, 160, 300);
            }
        });
     
        }

//        function ExisteCambioEnDocumento()
//        {

//            var vDocumentoActual = funGetSession("vDocumentoActualPFRPrevio");
//            var vDocumentoGrabado= funGetSession("vDocumentoActualPFR");

//            if(vDocumentoActual==vDocumentoGrabado)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }

        function LimpiarTabCuerpo() {
            $("#<%= txtCuerpo.ClientID %>").val('');
            $("#<%=txtCuerpo.ClientID %>").css("border", "solid #888888 1px");

        }

        function SetLabelActoNotarial(texto) {
            document.getElementById('lblTipoActoNotarialEP').innerText = texto;
            var caracteres = document.getElementById("<%=lblRecurrente.ClientID %>").innerText;
            var cantidadMaximo = texto.length + caracteres.length;
            if (cantidadMaximo > 130)
            {
            //document.getElementById('lblTipoActoNotarialEP').style.float = 'left';
                $('#lblTipoActoNotarialEP').css("float", "left");
                $('#lblTipoActoNotarialEP').css("left", "5px");
                $('#divInformacion').css("height", "50px");
                //document.getElementById('divInformacion').style.height = '50px'
            }

        }

        function ValidarTabAprobacionPago() {
            var bolValida = true;
        }

        function LimpiarTabAprobacionPago() {
        }

        function ValidarTabArchivoDigital() {
            var bolValida = true;
        }

        function LimpiarTabArchivoDigital() {
        }

        function isSujeto(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var letra = false;
            if (charCode != null)
            {                
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
                return letra;
            }
            return letra;
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

            var letras = "áéíóúñÑ";
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
            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }
            if (charCode >= 46 && charCode <= 60) {
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

            var letras = "aeiouAEIOU-";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
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

        function isDescripcion(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
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
            var letras = "0123456789áéíóúÑÁÉÍÓÚ°.():,";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isFormula(evt) {
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
            var letras = "0123456789áéíóúÑÁÉÍÓÚ°.():,/*-+";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isNumeroTelefono(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var letra = false;

            if (charCode == 8) {
                letra = true;
            }

            if (charCode == 32) {
                letra = true;
            }

            if (charCode > 47 && charCode < 58) {
                letra = true;
            }

            var letras = "-";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
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
            if (charCode == 32) {
                letra = true;
            }
            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }
            if (charCode >= 46 && charCode <= 60) {
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

        function isLetraNumeroDoc2(evt) {
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
            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }
            if (charCode >= 46 && charCode <= 60) {
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

            var letras = "aeiouAEIOU";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function validarSoloLetras(txt) {
            var charpos = txt.value.search("[^A-Za-z]");

            if (txt.value.length > 0 && charpos >= 0) {
                txt.value = '';
                alert('Error - No se pueden ingresar caracteres extraños.');
            }
        }

        function validarSoloLetras2(txt) {
            var charpos = txt.value.search("[^A-Za-z0-9 áéíóú(),.:°]");

            if (txt.value.length > 0 && charpos >= 0) {
                txt.value = '';
                alert('Error - No se pueden ingresar caracteres extraños.');
            }
        }

        function validarSoloLetras3(lbl, txt) {
            var charpos = txt.value.search("[^A-Za-z áéíóúñÑ.]");

            if (txt.value.length > 0 && charpos >= 0) {
                lbl.style.display = 'inline';
                txt.value = '';
            }
            else {
                lbl.style.display = 'none';
            }
        }

        function validarSoloLetras4(txt) {
            var charpos = txt.value.search("[^A-Za-z áéíóúñÑ. -]");

            if (txt.value.length > 0 && charpos >= 0) {
                txt.value = '';
                alert('Error - No se pueden ingresar caracteres extraños.');
            }
        }

        // FUNCIONES DIGITALIZACION DE ARCHIVOS (TAB 06)  
        //--------------------------------------------------------------------------------------------------------------------------------------
        function tab_06() {
            document.getElementById("<%= btnLoadArchivoDigitalizado.ClientID %>").click();
        }

        function AddDocumentoDigitalizado() {
            if (ValidarTabArchivoDigitalizado()) {
                var archivo = {};
                archivo.ando_iActoNotarialDocumentoId = $("#<%=hdn_ando_iActoNotarialDocumentoId.ClientID %>").val();
                archivo.ando_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
                archivo.ando_sTipoDocumentoId = 4204;
                archivo.ando_sTipoInformacionId = 0;
                archivo.ando_sSubTipoInformacionId = 0;
                archivo.ando_vDescripcion = $("#<%=Txt_AdjuntoDescripcion.ClientID %>").val().toUpperCase();
                archivo.ando_vDetalleDocumento = "";
                archivo.ando_vRutaArchivo = $("#<%=hidNomAdjFile2.ClientID %>").val();

                //archivo.ando_cEstado = "A";

                var prm = {};
                prm.archivo = '[' + JSON.stringify(archivo) + ']';
                var rsp = execute('FrmActuacionNotarialExtraProtocolar.aspx/adicionar_archivo', prm);

                tab_06();
            }
        }

        function OcultarElementoHtml(id,bOcultar)
        {
            if(bOcultar=='0')
                $(id).hide();
            else if(bOcultar=='1')
                $(id).show();
        }

        function SaveDocumentoDigitalizado() {

            var prm = {};
            var rsp = execute('FrmActuacionNotarialExtraProtocolar.aspx/ExisteDocumentosAdjuntos', prm);

            if (rsp.d == "0") {
                showpopupother('a', 'ADJUNTOS', 'No se ha adjuntado documentos', false, 200, 250);
                return;
            }

            if (confirm("¿Está seguro de grabar los cambios?")) {

                var larchivoDigitalizado = {};


                larchivoDigitalizado.ando_iActoNotarialDocumentoId = $("#<%= hdn_ando_iActoNotarialDocumentoId.ClientID %>").val();
                larchivoDigitalizado.ando_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();

                larchivoDigitalizado.ando_sUsuarioCreacion = $("#<%= hdn_acno_sUsuarioCreacion.ClientID %>").val();
                larchivoDigitalizado.ando_vIPCreacion = $("#<%= hdn_acno_vIPCreacion.ClientID %>").val();
                larchivoDigitalizado.ando_sUsuarioModificacion = $("#<%= hdn_acno_sUsuarioModificacion.ClientID %>").val();
                larchivoDigitalizado.ando_vIPModificacion = $("#<%= hdn_acno_vIPModificacion.ClientID %>").val();



                prm = {};
                prm.larchivoDigitalizado = JSON.stringify(larchivoDigitalizado);
                rsp = execute('FrmActuacionNotarialExtraProtocolar.aspx/insertar_archivo', prm);
                showdialog('i', 'Registro Notarial : Archivo Digitalizado(s)', 'El registro ha sido grabado correctamente', false, 160, 300);
            }
        }

        function ValidarTabArchivoDigitalizado() {
         

            var bolValida = true;

            var strDescripcion = $.trim($("#<%= Txt_AdjuntoDescripcion.ClientID %>").val());
            var strNomAdjFile2 = $.trim($("#<%= hidNomAdjFile2.ClientID %>").val());
            var tipoArchivo =$.trim($("#<%= ddlTipoArchivoAdjunto.ClientID %>").val());

            if (strNomAdjFile2.length == 0) {
                bolValida = false;

            }


            if(tipoArchivo==null){
                bolValida = false;
                $("#<%= ddlTipoArchivoAdjunto.ClientID %>").css("border", "solid Red 1px");

            }
            else if(tipoArchivo=="0"){
                bolValida = false;
                $("#<%= ddlTipoArchivoAdjunto.ClientID %>").css("border", "solid Red 1px");
            }






            if (bolValida) {
                $("#<%= lblValidacionArchivoDigital.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionArchivoDigital.ClientID %>").show();
                
                bolValida = false;
            }

            return bolValida;
        }



//        function funIncapacidadCheckSelected(bChecked)
//        {            
//            var iParticipanteEditando = funGetSession('iParticipanteEditando');
//            
//            if(bChecked)
//            {

//                var incapacitado = $("#<%=chkIncapacitado.ClientID %>")[0].checked;
//               // $("#<%=txtRegistroTipoIncapacidad.ClientID %>").val('');
//                
//                if(!incapacitado)
//                {                                                            
//                    var iExisteTestigoRuego = funGetSession('iExisteTestigoRuego');                  
//                }

//            }
//            

//             $("#<%=txtRegistroTipoIncapacidad.ClientID %>").prop("disabled", !incapacitado)
//         
//             if (iParticipanteEditando != "1")
//             {
//                $("#<%= chkNoHuella.ClientID %>").prop("checked", "");                
//             }
//         
//             if (incapacitado)
//             {                
////                    $('#MainContent_lblchkhuella').toggle(true);                                              
//                    $("#<%= chkNoHuella.ClientID %>").css('display','block');                  
//                    $("#<%= lblHuella.ClientID %>").css('display','block');                  
//             }
//             else
//             {
////                    $('#MainContent_lblchkhuella').toggle(false);                                              
//                    $("#<%= chkNoHuella.ClientID %>").css('display','none');                  
//                    $("#<%= lblHuella.ClientID %>").css('display','none');  
//             }
//               

//        }

        function LimpiarTabArchivoDigitalizado() {
            $("#<%=Txt_AdjuntoDescripcion.ClientID %>").val('');
            $("#<%=hidNomAdjFile2.ClientID %>").val('');

            $("#<%=Txt_AdjuntoDescripcion.ClientID %>").css("border", "solid #888888 1px");

            $("#<%= lblValidacionArchivoDigital.ClientID %>").hide();
        }

        function SetearClasebtnAgregarArchivo(accion) {
            //Agregar: 1
            //Actualizar:0

            if (accion == 1) {
                document.getElementById('btnAgregarArchivo').className = "btnNew";
                document.getElementById('btnAgregarArchivo').value = "   Agregar";
            }
            else if (accion == 0) {
                document.getElementById('btnAgregarArchivo').className = "btnSave";
                document.getElementById('btnAgregarArchivo').value = "   Actualizar";
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        function ValidarRegistroActuacion() {
            var bolValida = true;            
            var strTarifa = $.trim($("#<%= Txt_TarifaId.ClientID %>").val());
            var strTipoPago = $.trim($("#<%= ddlTipoPago.ClientID %>").val());
            var strCantidad = $.trim($("#<%= txtCantidad.ClientID %>").val());
            var strTopeMinimo = $.trim($("#<%= hdn_tope_min.ClientID %>").val());

            var strDescripcion = $.trim($("#<%= Txt_TarifaDescripcion.ClientID %>").val());

            var txtIdTarifa = document.getElementById('<%= Txt_TarifaId.ClientID %>');
            var cmb_TipoPago = document.getElementById('<%= ddlTipoPago.ClientID %>');
            var txtCantidad = document.getElementById('<%= txtCantidad.ClientID %>');
            var txtDescripcion = document.getElementById('<%= Txt_TarifaDescripcion.ClientID %>');

            var str_HF_PAGADO_EN_LIMA = $("#<%=HF_PAGADO_EN_LIMA.ClientID %>").val();
            var str_HF_DEPOSITO_CUENTA = $("#<%=HF_DEPOSITO_CUENTA.ClientID %>").val();
            var str_HF_TRANSFERENCIA = $("#<%=HF_TRANSFERENCIA.ClientID %>").val();

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
                if (cmb_TipoPago != null)
                    cmb_TipoPago.style.border = "1px solid Red";
                bolValida = false;
                
            }
            else {
                if (cmb_TipoPago != null)
                    cmb_TipoPago.style.border = "1px solid #888888";

                //PARAMETROS: PAGADO EN LIMA - 3501 
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
            if (strCantidad == "" || strCantidad == "0") {
                txtCantidad.style.border = "1px solid Red";
                bolValida = false;

            }
            else {
                txtCantidad.style.border = "1px solid #888888";
            }


            if (strTopeMinimo != "0") {
                if (parseInt(strCantidad) >= parseInt(strTopeMinimo)) {
                    txtCantidad.style.border = "1px solid #888888";
                }
                else {
                    txtCantidad.style.border = "1px solid Red";
                    bolValida = false;
                    alert("La cantidad debe ser mayor o igual al tope mínimo.");
                }
            }

            if (bolValida) {
                $("#<%= lblValidacionRegistroPago.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacionRegistroPago.ClientID %>").show();
                bolValida = false;
            }
            return bolValida;
        }  

        
        function nextControlFuncionario()
        {
            var valorSeleccionado = $("#<%=ddlSubTipoNotarialExtra.ClientID %>").val();
            if (valorSeleccionado != "0")
            {
                 $("#<%=ddlFuncionario.ClientID %>").focus();
            }

        }

        function nextControlFuncionario()
        {
            var valorSeleccionado = $("#<%=ddlSubTipoNotarialExtra.ClientID %>").val();
            if (valorSeleccionado != "0")
            {
                 $("#<%=ddlFuncionario.ClientID %>").focus();
            }

        }



        function RecargarUbigeoViaje() {
            var ubigeo = document.getElementById('<%=hbigeoViajeDestino.ClientID %>').value;

            if (ubigeo.length == 6) {
                var provincia = ubigeo.substring(2, 4);
                var distrito = ubigeo.substring(4, 6);;
                cargarProvinciaViaje();
                $('#<%= ddl_UbigeoRegionViajeDestino .ClientID %>').val(provincia).change();
                cargarDistritoViaje();
                $('#<%= ddl_UbigeoCiudadViajeDestino .ClientID %>').val(distrito).change();
            }
        }
        function cargarUbigeoViaje() {
            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPaisViajeDestino.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddl_UbigeoRegionViajeDestino.ClientID %>');
            var ddlDistrito = document.getElementById('<%=ddl_UbigeoCiudadViajeDestino.ClientID %>');

            var ubigeo = ddlDepartamento.value + ddlProvincia.value + ddlDistrito.value;
            $('#<%= hbigeoViajeDestino.ClientID %>').val(ubigeo);
        }
        function cargarProvinciaViaje() {
            $('#<%= hbigeoViajeDestino.ClientID %>').val("");
            $('#<%= ddl_UbigeoRegionViajeDestino.ClientID %>').empty();
            $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').empty();
            $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');
            var objetoProvincia = localStorage.getItem("objProvincia");

            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPaisViajeDestino.ClientID %>');

            var listaProvincia = JSON.parse(objetoProvincia);

            $('#<%= ddl_UbigeoRegionViajeDestino .ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaProvincia).each(function () {
                if (this.Ubi01 == ddlDepartamento.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Provincia);
                    option.val(this.Ubi02);
                    $('#<%= ddl_UbigeoRegionViajeDestino.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoRegionViajeDestino.ClientID %>').removeAttr("disabled");
                }
            });
        }

        function cargarDistritoViaje() {
            $('#<%= hbigeoViajeDestino.ClientID %>').val("");
             $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').empty()
             var objetoDistrito = localStorage.getItem("objDistrito");

             var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPaisViajeDestino.ClientID %>');
             var ddlProvincia = document.getElementById('<%=ddl_UbigeoRegionViajeDestino.ClientID %>');

            var listaDistrito = JSON.parse(objetoDistrito);

            $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaDistrito).each(function () {
                if (this.Ubi01 == ddlDepartamento.value && this.Ubi02 == ddlProvincia.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Distrito);
                    option.val(this.Ubi03);
                    $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoCiudadViajeDestino.ClientID %>').removeAttr("disabled");
                }
            });
        }

        function Guardarlocalstorage(objProvincia, objDistrito) {
            localStorage.setItem("objProvincia", JSON.stringify(objProvincia));
            localStorage.setItem("objDistrito", JSON.stringify(objDistrito));
            console.log(objProvincia);
            console.log(objDistrito);
        }
        $(document).ready(function () {
            console.log("recargo");
            RecargarUbigeoViaje();
        });
    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</asp:Content>
