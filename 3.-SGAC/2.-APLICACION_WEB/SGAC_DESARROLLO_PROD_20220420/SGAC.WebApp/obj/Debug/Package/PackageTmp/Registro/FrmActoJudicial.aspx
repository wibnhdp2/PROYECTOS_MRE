<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
 CodeBehind="FrmActoJudicial.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmJudicial" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc4" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.1.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

    <style type="text/css">
    
    .BordesColores
    {
        border: 1px solid #993333; 
        background-color:#ededed; 
        border-style:groove;
        background-color:transparent;        
        position:relative;
        margin:0px 0px 5px 0px;
    }
    
    .NotificacionDivs
    {
        padding: 0px 5px 5px 0px;

    }
    
    .SinCss
    {
        padding:0px;
    }
    

    
    .NotificacionTituloPestana
    {
        width:200px; 
        height:20px; 
        text-align:center; 
        border: 1px solid #993333; 
        border-bottom:0px; 
        background-color:#ededed;
        font-weight:normal; 
        font-style:normal; 
        line-height:20px; 
        padding:0px 5px; 
        font-family:Arial; 
        color:#990033;         
        border-radius:5px 5px 0px 0px; 
        
    }
    
    .NotificacionLabel
    {
        display:inline-block;
        position:relative;
        
    }
    
    .NotificacionControl
    {
        display:inline-block;
        position:relative;
        text-transform:uppercase;
    }
    
    </style>

    <script type="text/javascript">

        
        $(function () {
            $('#tabs').tabs();
        });

        function EnableTabIndex(iTab) {
            $(function () {
                $('#tabs').tabs();
                $('#tabs').enableTab(iTab);
            });
        }

        

        function LimpiarNotificacionEnvio() {

            $("#ddlViaEnvio").val('0');
            $("#txtEmpPostal").val('');
            $("#txtPersNotifica").val('');
            $('#<%= txtFechaNotifica.FindControl("TxtFecha").ClientID  %>').val();
            $("#txtHoraNotifica").val('');
            $("#txtNotificacionCuerpo").val('');
        }


        function LimpiarNotificacionRecepcion() {

            $("#ddlTipoRecepcion").val('0');
            $("#txtNroCedula").val('');
            $("#txtPerRecep").val('');
            $('#<%= txtFchRecep.FindControl("TxtFecha").ClientID  %>').val();
            $("#txtHoraRecep").val('');
            $("#txtNotiObservacion").val('');
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

            if (x.split(':').length == 1) {
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

        function EventosControles() {

            $("#<%=txtNumDoc3.ClientID %>").bind("blur", function (e) {

                    document.getElementById("<%=imgBuscar.ClientID %>").click();
                    e.preventDefault();
              

            });
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
            if (charCode == 46) {
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

        function isNumberMRE(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;

            if (charCode == 92) {
                letra = false;
            }

            if (charCode == 47) {
                letra = false;
            }
            if (charCode == 45) {
                letra = false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
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

            var letras = "áéíóúñÑ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        function validarCorreosDatosMRE(evt) {
            var key = (evt.which) ? evt.which : evt.keyCode;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "0123456789abcdefghijklmnñopqrstuvwxyz-/";
            especiales = [8, 39, 46];

            tecla_especial = false;
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }

            if (letras.indexOf(tecla) == -1 && !tecla_especial)
                return false;
        };

        function validarCorreosElectronicos(evt) {
            var key = (evt.which) ? evt.which : evt.keyCode;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "0123456789áéíóúabcdefghijklmnñopqrstuvwxyzñü.-_@";
            especiales = [8, 39, 46];

            tecla_especial = false;
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }

            if (letras.indexOf(tecla) == -1 && !tecla_especial)
                return false;
        };

        function txtcontrolError(ctrl) {
            var x = ctrl.value.trim();
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

        function ValidarGrabar() {
            return ValidarDatos();
        }

        function ValidarActasGrabar() {
            var bolValor = true;

            if (bolValor) {
                bolValor = confirm("¿Está seguro de grabar los cambios?");
            }
            return bolValor;
        }

        function ValidarFinalizarActas() {
            var bolValor = true;

            if (bolValor) {
                bolValor = confirm("¿Está seguro de finalizar el ingreso de Actas?");
            }
            return bolValor;
        }

        function ValidarEnvioExpediente() {
            var bolValor = true;

            var actojudicialEstado = funGetSession('ActoJudicialEstadoId');

            if (actojudicialEstado != null) {
                if (actojudicialEstado == 62) {
                    bolValor = confirm("El expediente aún no ha sido enviado. ¿Está seguro que desea salir?");
                }
            }

            
            return bolValor;
        }

        function ValidarActualizacionNotificacion() {
            var bolValor = true;

            var NotificacionId = funGetSession('NotificacionIdEditando');

            if (NotificacionId != null) {
                if (NotificacionId != '') {
                    bolValor = confirm("Aún no se guardado la actualización. ¿Está seguro que desea continuar?");
                }
            }


            return bolValor;
        }

        function ValidarEnviar() {
            var bolValor = true;

            if (bolValor) {
                bolValor = confirm("¿Está seguro de Enviar el Acta?");
            }
            return bolValor;
        }


        function ValidarCerrar() {
            var bolValor = true;

            if (bolValor) {
                bolValor = confirm("¿Está seguro de cerrar el Expediente?");
            }
            return bolValor;
        }

        function ValidarFinalizar() {
            var bolValor = true;

            var strOrigen = $('#<%=ctrlToolBarNoti.FindControl("btnConfiguration").ClientID %>').val();
            strOrigen = strOrigen.trim();

            if (strOrigen == "Finalizar") {
                if (bolValor) {
                    bolValor = confirm("¿Está seguro de Finalizar las Notificaciones?");
                }
            }
            else {
                bolValor = true;
            }
            return bolValor;
        }

        function ValidarObservacion() {
            var bolValida = true;


            var strDescripcion = $.trim($("#<%= txtDescSub.ClientID %>").val());

            if (strDescripcion.length == 0) {
                bolValida = false;
            }
            else {
                bolValida = true;
            }

            if (bolValida) {
                bolValida = confirm("¿Está seguro de Observar el Acta?");
            }
            return bolValida;
        }
              

        function ValidarPagos() {
            var bolValida = true;


            var check = $("#<%=chkGratuito.ClientID %>").is(':checked');

            if (!check) {

                if (txtcontrolError(document.getElementById("<%=txtNumOrdPag.ClientID %>")) == false) bolValida = false;

                var strOrigen = $('#<%=txtFchPago.FindControl("TxtFecha").ClientID %>').val();

                if (strOrigen == "") {
                    var ddlOCOrigen = document.getElementById('<%= txtFchPago.FindControl("TxtFecha").ClientID %>');

                    ddlOCOrigen.style.border = "1px solid Red";
                    bolValida = false;
                }


                if (txtcontrolError(document.getElementById("<%=txtCosto.ClientID %>")) == false) bolValida = false;
                /*if (txtcontrolError(document.getElementById("<%=txtCosto2.ClientID %>")) == false) bolValida = false;*/

                if ((document.getElementById("<%=ddlTipoPago.ClientID %>").value != 3503) && (document.getElementById("<%=ddlTipoPago.ClientID %>").value != 3504) && (document.getElementById("<%=ddlTipoPago.ClientID %>").value != 3502)) {

                    if (ddlcontrolError(document.getElementById("<%=ddlBanco.ClientID %>")) == false) bolValida = false;
                    if (txtcontrolError(document.getElementById("<%=txtNumCheque.ClientID %>")) == false) bolValida = false;
                }
            }


            if (ddlcontrolError(document.getElementById("<%=ddlTarifaConsul.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlMoneda.ClientID %>")) == false) bolValida = false;

                
            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;
        }

        function ValidarActas() {
            var bolValida = true;
            

            var strOrigen = $('#<%=txtActaFecha.FindControl("TxtFecha").ClientID %>').val();
                       

            if (strOrigen == "") {
                var ddlOCOrigen = document.getElementById('<%= txtActaFecha.FindControl("TxtFecha").ClientID %>');

                ddlOCOrigen.style.border = "1px solid Red";
                bolValida = false;
            }

            if (txtcontrolError(document.getElementById("<%=txtActaHora.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtActaResponsable.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtActaCuerpo.ClientID %>")) == false) bolValida = false;
            /*if (txtcontrolError(document.getElementById("<%=txtResultado.ClientID %>")) == false) bolValida = false;*/
            /*if (txtcontrolError(document.getElementById("<%=txtActaObservacion.ClientID %>")) == false) bolValida = false;*/

            if (ddlcontrolError(document.getElementById("<%=ddlActaTipo.ClientID %>")) == false) bolValida = false;
            
            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;
        }

        function ValidarNotificaciones() {
            var bolValida = true;

            var TextBox1 = '<%=btnNotiAceptar.ClientID%>';
            var text1 = document.getElementById(TextBox1);

            if (document.getElementById("<%=ddlViaEnvio.ClientID %>").value == 8551) {
                if (txtcontrolError(document.getElementById("<%=txtPersNotifica.ClientID %>")) == false) bolValida = false;
            }
            else {
                if (txtcontrolError(document.getElementById("<%=txtEmpPostal.ClientID %>")) == false) bolValida = false;
            }

            if (text1.value == "Adicionar") {
                if (ddlcontrolError(document.getElementById("<%=ddlViaEnvio.ClientID %>")) == false) bolValida = false;

                var strOrigen = $('#<%=txtFechaNotifica.FindControl("TxtFecha").ClientID %>').val();

                if (strOrigen == "") {
                    var ddlOCOrigen = document.getElementById('<%= txtFechaNotifica.FindControl("TxtFecha").ClientID %>');

                    ddlOCOrigen.style.border = "1px solid Red";
                    bolValida = false;
                }


                if (txtcontrolError(document.getElementById("<%=txtHoraNotifica.ClientID %>")) == false) bolValida = false;
                
            }
            if (text1.value == "Actualizar") {
                if (ddlcontrolError(document.getElementById("<%=ddlViaEnvio.ClientID %>")) == false) bolValida = false;
                                
                var strOrigen = $('#<%=txtFechaNotifica.FindControl("TxtFecha").ClientID %>').val();

                if (strOrigen == "") {
                    var ddlOCOrigen = document.getElementById('<%= txtFechaNotifica.FindControl("TxtFecha").ClientID %>');

                    ddlOCOrigen.style.border = "1px solid Red";
                    bolValida = false;
                }

                if (txtcontrolError(document.getElementById("<%=txtHoraNotifica.ClientID %>")) == false) bolValida = false;
                
               

                var tipoRecepcion = $("#<%=ddlTipoRecepcion.ClientID %>").val();
                var nrocedula = $("#<%=txtNroCedula.ClientID %>").val();

                if (tipoRecepcion != '0' || nrocedula!='') {

                    if (ddlcontrolError(document.getElementById("<%=ddlTipoRecepcion.ClientID %>")) == false) bolValida = false;

                    if (document.getElementById("<%=ddlViaEnvio.ClientID %>").value == 8552) {
                        if (txtcontrolError(document.getElementById("<%=txtNroCedula.ClientID %>")) == false) bolValida = false;
                    }

                    if (document.getElementById("<%=ddlTipoRecepcion.ClientID %>").value == 8521 ||
                    document.getElementById("<%=ddlTipoRecepcion.ClientID %>").value == 8522 ||
                    document.getElementById("<%=ddlTipoRecepcion.ClientID %>").value == 8523) {
                        var strOrigen = $('#<%=txtFchRecep.FindControl("TxtFecha").ClientID %>').val();

                        if (strOrigen == "") {
                            var ddlOCOrigen = document.getElementById('<%= txtFchRecep.FindControl("TxtFecha").ClientID %>');

                            ddlOCOrigen.style.border = "1px solid Red";
                            bolValida = false;
                        }

                        if (txtcontrolError(document.getElementById("<%=txtHoraRecep.ClientID %>")) == false) bolValida = false;
                        if (txtcontrolError(document.getElementById("<%=txtPerRecep.ClientID %>")) == false) bolValida = false;
                    }

                    if (document.getElementById("<%=ddlTipoRecepcion.ClientID %>").value == 8524 ||
                    document.getElementById("<%=ddlTipoRecepcion.ClientID %>").value == 8525) {
                        var strOrigen = $('#<%=txtFchRecep.FindControl("TxtFecha").ClientID %>').val();

                        if (strOrigen == "") {
                            var ddlOCOrigen = document.getElementById('<%= txtFchRecep.FindControl("TxtFecha").ClientID %>');

                            ddlOCOrigen.style.border = "1px solid Red";
                            bolValida = false;
                        }

                        if (txtcontrolError(document.getElementById("<%=txtHoraRecep.ClientID %>")) == false) bolValida = false;

                        if (txtcontrolError(document.getElementById("<%=txtNotiObservacion.ClientID %>")) == false) bolValida = false;
                    }

                }
                                
            }

            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;
        }

        function ValidarDatos() {
            var bolValida = true;

            if (txtcontrolError(document.getElementById("<%=txtNroDoc1.ClientID %>")) == false) bolValida = false;
                        
            //------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
            //           y la fecha de salida de valija
            // Fecha: 13/01/2017
            //------------------------------------------------------------------
//            var strOrigen = $('#<%=txtFchRecepcion.FindControl("TxtFecha").ClientID %>').val();

//            if (strOrigen == "") {
//                var ddlOCOrigen = document.getElementById('<%= txtFchRecepcion.FindControl("TxtFecha").ClientID %>');

//                ddlOCOrigen.style.border = "1px solid Red";
//                bolValida = false;
//            }
                        
            if (txtcontrolError(document.getElementById("<%=txtManteria.ClientID %>")) == false) bolValida = false;
                    

            if (ddlcontrolError(document.getElementById("<%=ddlTipoNotifica.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtNumOficio.ClientID %>")) == false) bolValida = false;
            

            if (document.getElementById("<%=ddlTipoNotifica.ClientID %>").value == 8501) {
                if (txtcontrolError(document.getElementById("<%=txtNumExp.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtOrgano.ClientID %>")) == false) bolValida = false;
            }

            if (document.getElementById("<%=ddlTipoNotifica.ClientID %>").value == 8502) {
                if (ddlcontrolError(document.getElementById("<%=ddlEntSoli.ClientID %>")) == false) bolValida = false;
            }

            if (bolValida) {
                $("#<%= lblValidacionExpediente.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacionExpediente.ClientID %>").show();
            }

            return bolValida;
        }

        function ValidarNunDocParticipante() {
            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%=ddlTipPersona3.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlTipDoc3.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%=txtNumDoc3.ClientID %>")) == false) bolValida = false;

            var strNumDocDemandate = document.getElementById("<%=txtNroDoc1.ClientID %>").value;
            var strTipoDocDemandate = document.getElementById("<%=ddlTipDocumento.ClientID %>").value;


            var strNumDocDemandado = document.getElementById("<%=txtNumDoc3.ClientID %>").value;
            var strTipoDocDemandado = document.getElementById("<%=ddlTipDoc3.ClientID %>").value;


            if ((strTipoDocDemandate == strTipoDocDemandado) && (strNumDocDemandate == strNumDocDemandado)) {
                bolValida = false;
                document.getElementById("<%=txtNom3.ClientID %>").value = '';
                document.getElementById("<%=txtApePat3.ClientID %>").value = '';
                document.getElementById("<%=txtApeMat3.ClientID %>").value = '';
                showpopupother('a', 'EXPEDIENTE JUDICIAL', "El Numero de Documento del Demandado no Puede ser igual al del Demandante", false, 200, 300);

            }

            return bolValida;
        }

        function ValidarNotificados() {
            var bolValida = true;
            var strNumDocDemandate = document.getElementById("<%=txtNroDoc1.ClientID %>").value;
            var strTipoDocDemandate = document.getElementById("<%=ddlTipDocumento.ClientID %>").value;


            var strNumDocDemandado = document.getElementById("<%=txtNumDoc3.ClientID %>").value;
            var strTipoDocDemandado = document.getElementById("<%=ddlTipDoc3.ClientID %>").value;


            if (txtcontrolError(document.getElementById("<%=txtNroDoc1.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlTipPersona3.ClientID %>")) == false) bolValida = false;

            if (document.getElementById("<%=ddlTipPersona3.ClientID %>").value == 2101) {
                if (txtcontrolError(document.getElementById("<%=txtNom3.ClientID %>")) == false) bolValida = false;
                if (txtcontrolError(document.getElementById("<%=txtApePat3.ClientID %>")) == false) bolValida = false;

                if (strTipoDocDemandado==1) {
                    if (txtcontrolError(document.getElementById("<%=txtApeMat3.ClientID %>")) == false) bolValida = false;
                }
            }
            else {
                if (txtcontrolError(document.getElementById("<%=txtNom3.ClientID %>")) == false) bolValida = false;
            }

            //------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
            //           y la fecha de salida de valija
            // Fecha: 13/01/2017
            //------------------------------------------------------------------

//            var strOrigen = $('#<%=txtFchRecepcion.FindControl("TxtFecha").ClientID %>').val();

//            if (strOrigen == "") {
//                var ddlOCOrigen = document.getElementById('<%= txtFchRecepcion.FindControl("TxtFecha").ClientID %>');

//                ddlOCOrigen.style.border = "1px solid Red";
//                bolValida = false;
//            }


            if (txtcontrolError(document.getElementById("<%=txtNumHojRem.ClientID %>")) == false) bolValida = false;

            if ((strTipoDocDemandate == strTipoDocDemandado) && (strNumDocDemandate == strNumDocDemandado)) {
                bolValida = false;
                showpopupother('a', 'EXPEDIENTE JUDICIAL', 'El Numero de Documento del Demandado no Puede ser igual al del Demandante', false, 200, 300);
            }

            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }

            return bolValida;
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
                alert("Número Incorrecto");
                control.value = "";
            }
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }


        function funGetSession(variable) {
            var url = 'FrmActoJudicial.aspx/GetSession';
            var prm = {};
            prm.variable = variable;
            var rspta = execute(url, prm);

            return JSON.parse(JSON.stringify(rspta)).d;
        }

        function execute(urlmetodo, parametros) {
            var rsp;
            $.ajax({
                url: urlmetodo,
                type: "POST",
                data: JSON.stringify(parametros),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                cancel: false,
                success: function (response) {
                    rsp = response;
                },
                failure: function (msg) {
                    alert(msg);
                    rsp = msg;
                },
                error: function (xhr, status, error) {
                    alert(error);
                    rsp = error;
                }
            });

            return rsp;
        }

    </script>
    <style type="text/css">
        .style3
        {
            width: 149px;
        }
        .style4
        {
            height: 29px;
        }
        .style5
        {
            width: 260px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    |&nbsp;<%--Consulta --%><table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloJudicial" runat="server" Text="Exhortos Consulares"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HFGUID" runat="server" />
    <table style="width: 90%;" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblExpediente" runat="server" Text="Exhorto Consular"></asp:Label></a></li>
                        <li><a href="#tab-2">
                            <asp:Label ID="lblNotificacion" runat="server" Text="Notificaciones"></asp:Label></a></li>
                        <li><a href="#tab-3">
                            <asp:Label ID="lblActa" runat="server" Text="Acta Diligenciamiento-Complementaria"></asp:Label></a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel ID="updNotificados" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <ToolBarButton:ToolBarButtonContent ID="ctrlToolBar" runat="server"></ToolBarButton:ToolBarButtonContent>

                                <div style=" margin: 10px 0px 5px 0px"> <%--Label y validación del Demandante --%>
                                    <div>
                                        <Label:Validation ID="Validation1" runat="server" />
                                    </div>
                                    <div>
                                        <asp:Label ID="lblValidacionExpediente" runat="server" Text="Falta ingresar algunos campos"
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"></asp:Label>
                                    </div>
                                    
                                </div>

                                <div class="NotificacionTituloPestana" style=" clear:both;">I. Datos del Demandante</div>


                                <div class="BordesColores" style="clear: both; padding: 5px 5px 5px 10px;" > <%--Formulario del Demandante --%>

                                    <div> <%--Fila 1 --%>

                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="lblDemandanteTipo" runat="server" Text="Tipo Persona:" Width="100px" />
                                            <asp:DropDownList ID="ddlTipPersona" runat="server" Width="170px" Enabled="false" TabIndex="0">
                                                <asp:ListItem Text="NATURAL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                  

                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label68" runat="server" Text="Tipo Documento:" Width="100px"/>
                                            <asp:DropDownList ID="ddlTipDocumento" runat="server" Width="173px" Enabled="false"
                                                                TabIndex="1">
                                                <asp:ListItem Text="DNI"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="lblNumDoc" runat="server" Text="Nro. Documento:" Width="100px" />
                                            <asp:TextBox ID="txtNroDoc1" runat="server" Width="160px" onkeypress="return isNumberKey(event)"
                                                                CssClass="txtLetra" Enabled="False" TabIndex="2" ReadOnly="True" />
                                        </div>

                                        

                                    </div>                                   

                                    <div style="display: inline-block; width:100%; padding:4px 0px 0px 0px;"> <%--Fila 2 --%>
                                        <div >
                                            <asp:Label ID="lblDemandanteNombre" runat="server" Text="Nombre: " Width="100px"/>
                                            <asp:TextBox ID="txtNom1" runat="server" Width="450px" onkeypress="return isSujeto(event)"
                                                    CssClass="txtLetra" TabIndex="3" ReadOnly="True" Enabled="False" />
                                        </div>
                                    </div>

                                    <div style="display: inline-block; width:100%; padding:5px 0px 0px 0px;"> <%--Fila 3 --%>
                                        <div >
                                            <asp:Label ID="Label81" runat="server" Text="Dirección:" Width="100px" />
                                            <asp:TextBox ID="txtDir1" runat="server" Width="450px" CssClass="txtLetra" TabIndex="4"
                                                ReadOnly="True" Enabled="False" />
                                        </div>
                                    </div>

                                    <div style="display: inline-block; width:100%; padding:5px 0px 0px 0px;"> <%--Fila 4 --%>
                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="lblDemandanteTelefono" runat="server" Text="Teléfono:" Width="100px" />
                                            <asp:TextBox ID="txtTel1" runat="server" onkeypress="return isNumeroTelefono(event)"
                                                Width="165px" CssClass="txtLetra" TabIndex="5" ReadOnly="True" 
                                                Enabled="False" />
                                        </div>

                                  

                                        <div style="display: inline-block; width:35%;">
                                            <asp:Label ID="lblDemandanteCorreo" runat="server" Text="Correo:" Width="100px"/>
                                            <asp:TextBox ID="txtCorreo1" runat="server" Width="170px" onkeypress="return validarCorreosElectronicos(event)"
                                                CssClass="txtLetra" Enabled="False" TabIndex="6" ReadOnly="True" />     
                                        </div>
                                    </div>                                    

                                </div>
                               
                                
                                <div class="NotificacionTituloPestana" style=" margin: 30px 0px 0px 0px">II. Datos del Expediente</div>


                                <div class="BordesColores" style="clear: both; padding: 5px 5px 5px 10px;" > <%--Datos del Expediente --%>
                                    <div style="display: inline-block; width:45%;">
                                        <asp:Label ID="Label8" runat="server" Text="Tipo Notificación:" Width="100px"/>
                                        <asp:DropDownList ID="ddlTipoNotifica" runat="server" Width="240px" Enabled="false"
                                                TabIndex="7" 
                                                onselectedindexchanged="ddlTipoNotifica_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>    
                                            <asp:Label ID="lblCO_txtOrgano" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                    </div>


                                    <div>
                                        <div style="display: inline-block; width:45%; padding: 5px 0px 0px 0px;">
                                            <asp:Label ID="Label90" runat="server" Text="Entidad Solic.:" Width="100px"/>
                                            <asp:DropDownList ID="ddlEntSoli" runat="server" Width="240px" Enabled="false" TabIndex="8">
                                                </asp:DropDownList>                                            
                                                <asp:Label ID="lblCO_ddlEntSoli" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                        </div>
                                    </div>

                                    <div>                            
                                    
                                        <div style="display: inline-block; width:45%; padding:5px 0px 0px 0px;">
                                            <asp:Label ID="Label35" runat="server" Text="Nro. Expediente:" Width="100px" />
                                            <asp:TextBox ID="txtNumExp" runat="server" Width="240px" onkeypress="return validarCorreosDatosMRE(event)"
                                                    CssClass="txtLetra" TabIndex="9" MaxLength="20" Enabled="False" 
                                                    AutoPostBack="True" ontextchanged="txtNumExp_TextChanged" />                                            
                                            <asp:Label ID="lblCO_txtNumExp" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                        </div>

                                        <div style="display: inline-block; float: right;">
                                            <asp:Label ID="Label50" runat="server" Text="Fecha Recepción:" Width="100px"/>
                                            <SGAC_Fecha:ctrlDate ID="txtFchRecepcion" runat="server" TabIndex="10"/>
                                                
                                        </div>

                                    </div>

                                    <div>                            
                                    
                                        <div style="display: inline-block; width:45%; padding:5px 0px 0px 0px;">
                                            <asp:Label ID="Label86" runat="server" Text="Número de Oficio:" Width="100px"/>
                                            <asp:TextBox ID="txtNumOficio" runat="server" CssClass="txtLetra" Width="240px" onkeypress="return validarCorreosDatosMRE(event)"
                                            TabIndex="11" MaxLength="30" Enabled="False" /> 
                                            <asp:Label ID="lblCO_txtNumOficio" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                        </div>

                                        <div style="display: inline-block; float: right;">
                                            <asp:Label ID="Label70" runat="server" Text="Fecha Audiencia:" Enabled="false" Width="100px"/>
                                            <SGAC_Fecha:ctrlDate ID="txtFchAudiencia" runat="server" TabIndex="12" />
                                            <asp:Label ID="lblCO_txtFchAudiencia" runat="server" Text=" " Style="color: #FF0000" ></asp:Label>
                                        </div>

                                    </div>

                                    <div>                            
                                    
                                        <div style="display: inline-block; width:100%; padding:5px 0px 0px 0px;">
                                            <asp:Label ID="Label49" runat="server" Text="Materia Demanda:" Width="100px" style="position: relative; bottom:20px;"/>
                                            <asp:TextBox ID="txtManteria" runat="server" CssClass="txtLetra" Width="705px" Height="43px" 
                                                TabIndex="13" MaxLength="200" Enabled="False" TextMode="MultiLine" />   
                                            <asp:Label ID="lblCO_txtManteria" runat="server" Text="*" Style="color: #FF0000; position: relative; bottom:30px;"></asp:Label>
                                        </div>

                                        

                                    </div>

                                    <div>                            
                                    
                                        <div style="display: inline-block; width:100%; padding:5px 0px 0px 0px;">
                                           <asp:Label ID="Label353" runat="server" Text="Organo Judicial:" Width="100px" style="position: relative; bottom:20px;"/>
                                           <asp:TextBox ID="txtOrgano" runat="server" CssClass="txtLetra" Width="705px" Height="43px"
                                                TabIndex="14" MaxLength="200" Enabled="False" TextMode="MultiLine"/>       
                                            <asp:Label ID="lblCO_ddlTipoNotifica" runat="server" Text="*" Style="color: #FF0000; position: relative; bottom:30px;"></asp:Label>
                                        </div>

                                        

                                    </div>

                                    <div>                            
                                    
                                        <div style="display: inline-block; width:100%; padding:5px 0px 0px 0px;">
                                           <asp:Label ID="Label96" runat="server" Text="Observaciones:" Width="100px" style=" position: relative; bottom:20px;"/>
                                           <asp:TextBox ID="txtObs" runat="server" Width="705px" Height="43px" TextMode="MultiLine"
                                                CssClass="txtLetra" TabIndex="15" Enabled="False" />   
                                        </div>

                                        

                                    </div>

                                    

                                </div>

                                

                              

                                <div>
                                    <Label:Validation ID="ValParticipante" runat="server" />
                                </div>

                                <div class="NotificacionTituloPestana" style=" margin: 30px 0px 0px 0px">III. Personas/Empresas a Notificar</div>


                                    <div class="BordesColores" style="clear: both; padding: 5px 5px 5px 10px;" > <%--Datos del Expediente --%>
                                       
                                        

                                        <div> <%--Fila 1 --%>

                                            <div style="display: inline-block; width:33%; ">
                                                <asp:Label ID="Label3" runat="server" Text="Tipo Persona:" Width="100px"/>
                                                <asp:DropDownList ID="ddlTipPersona3" runat="server" Width="160px" Enabled="false"
                                                TabIndex="16" onselectedindexchanged="ddlTipPersona3_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                                <asp:ListItem Text="NATURAL"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label21" runat="server" Text="*" Style="color: #FF0000"/>
                                            </div>

                                  

                                            <div style="display: inline-block; width:33%;">
                                                <asp:Label ID="Label6" runat="server" Text="Tipo Documento:" Width="100px"/>
                                                <asp:DropDownList ID="ddlTipDoc3" runat="server" Width="160px" Enabled="false" 
                                                TabIndex="17" AutoPostBack="True" 
                                                onselectedindexchanged="ddlTipDoc3_SelectedIndexChanged">
                                                <asp:ListItem Text="DNI"></asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:Label ID="Label23" runat="server" Text="*" Style="color: #FF0000" 
                                                    Visible="False" />
                                            </div>

                                            <div style="display: inline-block; width:33%;">
                                                <asp:Label ID="Label20" runat="server" Text="Nro. Documento:" />
                                                <asp:TextBox ID="txtNumDoc3" runat="server" Width="145px" CssClass="txtLetra"
                                                TabIndex="18" Enabled="False" />
                                                <asp:Label ID="Label24" runat="server" Text="*" Style="color: #FF0000" 
                                                    Visible="False" />
                                                <asp:ImageButton ID="imgBuscar" ImageUrl="~/Images/img_16_search.png" runat="server" OnClientClick="return ValidarNunDocParticipante()"
                                                OnClick="imgBuscar_Click" Enabled="False" TabIndex="19"/>
                                            </div>                                       

                                        </div>   
                                        
                                        <div  style="padding:5px 0px 0px 0px;">
                                        
                                            <div> 
                 
                                                <asp:Label ID="lblNombreDemandado" runat="server" Text="Nombres" Width="100px"/>
                                                <asp:TextBox ID="txtNom3" runat="server" Width="440px" onkeypress="return isSujeto(event)"
                                                CssClass="txtLetra" TabIndex="20" Enabled="False" />

                                                <asp:Button ID="btnBuscarParticipante" runat="server" Text="Buscar Persona" Width="110px"
                                                OnClick="btnBuscarParticipante_Click" OnClientClick="return ValidarNunDocParticipante()"
                                                Enabled="False" Visible="False" />
                                            </div>  

                                            
                                        </div>
                                        

                                        <div style="padding:5px 0px 0px 0px;">                                  
                                            <asp:Label ID="Label5" runat="server" Text="Primer Apellido:" Width="100px"/>
                                            <asp:TextBox ID="txtApePat3" runat="server" Width="240px" onkeypress="return isSujeto(event)"
                                            CssClass="txtLetra" TabIndex="21" Enabled="False" />
                                            <asp:Label ID="lblObligatorioApePadre" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
 
                                        </div>  

                                        <div style="padding:5px 0px 0px 0px;"> 
                                            <asp:Label ID="Label25" runat="server" Text="Segundo Apellido:" Width="100px"/>
                                            <asp:TextBox ID="txtApeMat3" runat="server" Width="240px" onkeypress="return isSujeto(event)"
                                            CssClass="txtLetra" TabIndex="22" Enabled="False" />
                                            <asp:Label ID="lblObligatorioApeMadre" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            
   
                                        </div>  

                                        <div style="padding:5px 0px 0px 0px;">

                                            <div style="display: inline-block; width:45%;">
                                                <asp:Label ID="Label29" runat="server" Text="N° Hoja Remisión:" Width="100px"/>
                                                <asp:TextBox ID="txtNumHojRem" runat="server" Width="240px" onkeypress="return validarCorreosDatosMRE(event)"
                                                CssClass="txtLetra" TabIndex="23" MaxLength="50" Enabled="False" />                                            
                                                <asp:Label ID="lblCO_txtNumHojRem" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            </div>

                                            <div style="display: inline-block; ">
                                                <asp:Label ID="Label28" runat="server" 
                                                Text="Fec. Salida Valija:" ToolTip="Fecha de Salida de Valija Diplomática" Width="100px"/>

                                                <SGAC_Fecha:ctrlDate ID="txtFchValDip" runat="server" Width="160px" TabIndex="24"/>
                                                
                                            </div>

                                        </div>   

                                        <div style="padding:5px 0px 0px 0px;"> 

                                            <div style="display: inline-block; width:70%;">
                                                <asp:Label ID="Label38" runat="server" Text="Consulado Dest." Width="100px" ToolTip="Consulado Destino:"/>

                                                <uc4:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Enabled="False"
                                                Width="440px" TabIndex="25"/>
                                                <asp:Label ID="Label82" runat="server" Text="*" Style="color: #FF0000" />
                                            </div>
                                  
                                            <div style="display: inline-block; width:29%; text-align:right;">
                                                <asp:Button ID="btnAceptarNotifica" runat="server" Text="Adicionar" CssClass="btnNew" TabIndex="26"
                                                OnClick="btnAceptarNotifica_Click" Enabled="False" OnClientClick="return ValidarNotificados()" style="width:100px; text-align:right;" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Limpiar" CssClass="btnLimpiar" TabIndex="27"
                                                OnClick="btnCancelar_Click" Enabled="False" style="width:100px; text-align:right;"/>
                                            </div>
                                     
                                        </div>   

                                        <div style="height:150px; border:1px solid #800000; overflow:auto; margin: 5px 0 5px 0;">

                                            <asp:GridView ID="gdvExpNotificados" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvExpNotificados_RowCommand"
                                            ShowHeaderWhenEmpty="True" DataKeyNames="ajpa_iPersonaId,ajpa_iEmpresaId,ajpa_vNotificado,ajpa_iActoJudicialParticipanteId,ajpa_iActuacionDetalleId,acde_sTarifarioId,tari_vDescripcionCorta,actu_dFechaRegistro,ajpa_sOficinaConsularDestinoId,ajpa_sEstadoId,ajpa_sDocumentoTipoId,ajpa_vDocumentoNumero">
                                            <Columns>
                                                <asp:BoundField HeaderText="Nro." DataField="ajpa_iNumero" >
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Fecha Notificación" 
                                                    DataField="ajpa_dFechaNotificacion" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" />
                                                <asp:BoundField HeaderText="Nro. Expediente" DataField="ajpa_vNroExpediente" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Estado Demandado" 
                                                    DataField="ajno_vEstadoPartipante" />
                                                <asp:BoundField HeaderText="Notificado" DataField="ajpa_vNotificado" />
                                                <asp:BoundField HeaderText="Consulado Destino" DataField="ajpa_vConsulado" />
                                                <asp:TemplateField HeaderText="Notificación">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnNotificar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            CommandName="Notificar" ImageUrl="../Images/img_16_notification.png" ToolTip="Notificar" TabIndex="28"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ver">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnFind" runat="server" 
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" 
                                                            ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Ver Notificación" TabIndex="29"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" TabIndex="30"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <%--
                                <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Anular Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                            runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Editar Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_16_delete.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="iActoJudicialParticipante" DataField="ajpa_iActoJudicialParticipanteId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="ajpa_iPersonaId" DataField="ajpa_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="ajpa_vDocumentoNumero" DataField="ajpa_vDocumentoNumero" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                            <EmptyDataTemplate>
                                                <table ID="tbSinDatos">
                                                    <tbody>
                                                        <tr>
                                                            <td width="10%">
                                                                <asp:Image ID="imgWarning" runat="server" 
                                                                    ImageUrl="../Images/img_16_warning.png" />
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
                                        </asp:GridView>
                                    </div>
                                    </div>
                                         
                                <div>
                                    <Label:Validation ID="valExpedientePagos" runat="server" />
                                </div>
                                <div class="NotificacionTituloPestana" style=" margin: 30px 0px 0px 0px">IV. Datos de la Actuación Consular</div>


                                <div class="BordesColores" style="clear: both; padding: 5px 5px 5px 10px;" >
                                    <div>
                                        <div style="display: inline-block; width:70%;  ">
                                                <asp:Label ID="label876" runat="server" Text="Gratuito:" Width="100px" />
                                                
                
                                                <asp:CheckBox ID="chkGratuito" runat="server" 
                                                    oncheckedchanged="chkGratuito_CheckedChanged" AutoPostBack="True" />
                                                
                
                                        </div>
                                    </div>
                                    
                                    <div>
                                        <div style="display: inline-block; width:70%; margin-bottom:5px; ">
                                                <asp:Label ID="lblTituloMotivoGratuito" runat="server" Text="Motivo Gratuidad:" Width="100px"/>
                                                
                                                <asp:TextBox ID="txtMotivoGratuidad" runat="server" Width="430px"></asp:TextBox>
                                                
                                        </div>
                                    </div>           
                                               
                                                                                                                   
                                    <div>
                                        <div style="display: inline-block; width:70%; ">
                                                <asp:Label ID="Label1" runat="server" Text="Tarifa Consular:" Width="100px"/>
                                                <asp:DropDownList ID="ddlTarifaConsul" runat="server" Width="440px" Enabled="false"
                                            TabIndex="31" AutoPostBack="True"
                                            onselectedindexchanged="ddlTarifaConsul_SelectedIndexChanged">
                                            <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                             </asp:DropDownList>
                                            <asp:Label ID="Label2" runat="server" Text="*" Style="color: #FF0000"/>
                                        </div>
                                    </div>

                                    <div style="padding:5px 0px 0px 0px;">
                                        <div style="display: inline-block; width:33%; ">
                                            <asp:Label ID="Label54" runat="server" Text="Nro. Orden Pago:" Width="100px"/>
                                            <asp:TextBox ID="txtNumOrdPag" runat="server" Width="160px" CssClass="txtLetra" onkeypress="return validarCorreosDatosMRE(event)"
                                                TabIndex="32" MaxLength="20" Enabled="False" />
                                            <asp:Label ID="Label66" runat="server" Text="*" Style="color: #FF0000"/>
                                        </div>
                                        
                                        
                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label10" runat="server" Text="Costo (S/C):" Width="100px"/>
                                            <asp:TextBox ID="txtCosto" runat="server" Width="160px" CssClass="campoNumero" onkeypress="return isDecimalKey(this,event)"
                                            TabIndex="33" Enabled="false" />&nbsp;
                                        </div>      
                                        
                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label4" runat="server" Text="Fecha Pago:" Width="100px"/>
                                            <SGAC_Fecha:ctrlDate ID="txtFchPago" runat="server" TabIndex="34"/>
                                            <asp:Label ID="Label7" runat="server" Text="*" Style="color: #FF0000" />
                                        </div>
     
                                    </div>

                                    <div style="padding:5px 0px 0px 0px;">
                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label11" runat="server" Text="Tipo Pago:" Width="100px"/>
                                            <asp:DropDownList ID="ddlTipoPago" runat="server" Width="160px" Enabled="false" 
                                                TabIndex="35" onselectedindexchanged="ddlTipoPago_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label12" runat="server" Text="*" Style="color: #FF0000" />
                                        </div>                                  

                                        <div style="display: inline-block; width:40%;">
                                            <asp:Label ID="Label13" runat="server" Text="Moneda:" Width="100px"/>
                                            <asp:DropDownList ID="ddlMoneda" runat="server" Width="190px" Enabled="false" 
                                                TabIndex="36">
                                                <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label22" runat="server" Text="*" Style="color: #FF0000"/>

                                            <asp:TextBox ID="txtCosto2" runat="server" Width="90px" CssClass="campoNumero" onkeypress="return isDecimalKey(this,event)"
                                            TabIndex="37" ReadOnly="True" Enabled="False" Visible="False" />&nbsp;
                                        </div>
                                              
                                    </div>

                                    <div style="padding:5px 0px 0px 0px;">
                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label27" runat="server" Text="Banco:" Width="100px"/>
                                           <asp:DropDownList ID="ddlBanco" runat="server" Width="150px" Enabled="false" 
                                            TabIndex="38">
                                            <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label41" runat="server" Text="*" Style="color: #FF0000"/>
                                        </div>

                                  

                                        <div style="display: inline-block; width:33%;">
                                            <asp:Label ID="Label53" runat="server" Text="Nro. Voucher:" 
                                            Width="100px"></asp:Label>
                                            <asp:TextBox ID="txtNumCheque" runat="server" Width="100px" CssClass="txtLetra" onkeypress="return validarCorreosDatosMRE(event)"
                                            TabIndex="39" MaxLength="20" Enabled="False" />
                                            <asp:Label ID="Label61" runat="server" Text="*" Style="color: #FF0000"/>
                                        </div>

                                        <div style="display: inline-block; width:29%; text-align:right;">
                                            <asp:Button ID="btnAceptarPago" runat="server" Text="     Adicionar" 
                                            CssClass="btnNew" Enabled="False"  
                                            OnClientClick="return ValidarPagos()" onclick="btnAceptarPago_Click" 
                                            TabIndex="40" />

                                            <asp:Button ID="btn_CancelarPago" runat="server" Text="     Limpiar" CssClass="btnLimpiar"
                                            Enabled="False" onclick="btn_CancelarPago_Click" TabIndex="41" />
                                        </div>
      
                                    </div>

                                    <div style="height:150px; border:1px solid #800000; overflow:auto; margin: 5px 0 5px 0;">
                            	<asp:GridView ID="gdvPagos" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                    AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvPagos_RowCommand"
                                    ShowHeaderWhenEmpty="True" DataKeyNames="pago_iPagoId">
                                    <Columns>
                                        <asp:BoundField HeaderText="Código" DataField="pago_sId" >
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Descripción" 
                                            DataField="pago_sTarifarioDescripcion" />
                                        <asp:BoundField HeaderText="Monto S.C." DataField="pago_FMontoSolesConsulares" 
                                            DataFormatString="{0:#,##0.00}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Ver">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnFind" runat="server" 
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" 
                                                    ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Ver Notificación" TabIndex="42"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                    runat="server" ImageUrl="../Images/img_grid_modify.png" TabIndex="43"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <%--
                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Anular Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                    runat="server" ImageUrl="../Images/img_grid_delete.png" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Editar Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                    runat="server" ImageUrl="../Images/img_16_delete.png" TabIndex="44"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                    <EmptyDataTemplate>
                                        <table ID="tbSinDatos">
                                            <tbody>
                                                <tr>
                                                    <td width="10%">
                                                        <asp:Image ID="imgWarning" runat="server" 
                                                            ImageUrl="../Images/img_16_warning.png" />
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
                                </asp:GridView>
                            </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
               </div>

                                                     
                    <div id="tab-2">
                        <%--AGREGAR NOTIFICACION--%>
                        <asp:Button ID="btn_imprimir_expediente" runat="server" Text="Imprimir" 
                            onclick="btn_imprimir_expediente_Click" style="display:none;" />
                        <asp:UpdatePanel runat="server" ID="updNotificaciones" UpdateMode="Conditional">
                            <%--<Triggers>
                                <asp:PostBackTrigger ControlID="gdvNotificaciones" />
                            </Triggers>--%>
                            <ContentTemplate>
                                <asp:HiddenField ID="hIndex" runat="server" />
                                <asp:HiddenField ID="hId_Actuacion_Select" runat="server" />
                                <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarNoti" runat="server"></ToolBarButton:ToolBarButtonContent>
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <Label:Validation ID="Validation2" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="PERSONA/EMPRESA A NOTIFICAR :  " Style="font-weight: 700;
                                                color: #800000;" />
                                            <asp:Label ID="lblNombreNotificado" runat="server" Text="" Style="font-weight: 700;
                                                color: #800000;" />
                                            <asp:Button ID="Button8" runat="server" Text="Button" Visible="False" />
                                            <asp:Button ID="Button9" runat="server" Text="Actas" Style="height: 26px"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                                <br />

                                <div class="NotificacionTituloPestana" style=" clear:both;">Envío de Notificación</div>

                                  <div class="BordesColores" >
                                    <div  style="clear: both; padding: 10px 5px 5px 10px;">
                                    
                                        <div class="NotificacionDivs">
                                            <asp:Label ID="Label42" CssClass="NotificacionLabel" runat="server" 
                                                Text="Vía Envio:" Width="150px"/>
                                            <asp:DropDownList ID="ddlViaEnvio" runat="server" TabIndex="55" Enabled="False" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlViaEnvio_SelectedIndexChanged" Width="250px">
                                                    <asp:ListItem Text="NOTIFICACIÓN PERSONAL"></asp:ListItem>
                                                    <asp:ListItem Text="CORREO POSTAL CERTIFICADO"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label44" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                        </div>

                                        <div>

                                            <div class="NotificacionDivs"">
                                                <asp:Label ID="Label74" CssClass="NotificacionLabel" runat="server" 
                                                    Text="Empresa Servicio Postal:" Width="150px" />
                                                <asp:TextBox ID="txtEmpPostal" runat="server" CssClass="txtLetra" onkeypress="return isSujeto(event)"
                                                        TabIndex="56" MaxLength="100" Enabled="False" Width="250px" />
                                                <asp:Label ID="Label76" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 

                                                <asp:Label ID="Label77" CssClass="NotificacionLabel" runat="server" Text="Persona que notifica:" Width="140px" style="display:inline-block; position: relative; left: 15px;" />

                                                <asp:TextBox ID="txtPersNotifica" runat="server" CssClass="txtLetra"
                                                    onkeypress="return isSujeto(event)" TabIndex="57" 
                                                    MaxLength="100" Enabled="False" Width="250px" />
                                            
                                                <asp:Label ID="Label78" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                            </div>
                                        


                                        </div>

                                        <div class="NotificacionDivs">
                                       
                                            <asp:Label ID="Label69" runat="server" Text="Fecha/Hora Notificación:" Width="150px" style="display:inline-block"/>

                                            <SGAC_Fecha:ctrlDate ID="txtFechaNotifica" runat="server" style="display:inline-block" TabIndex="58"/>
                                            <asp:Label ID="Label71" runat="server" Text="*" Style="color: #FF0000; display:inline-block"></asp:Label> 

                                            <asp:TextBox ID="txtHoraNotifica" runat="server" Width="60px" CssClass="txtLetra"
                                                onBlur="validaHora(this)" onkeypress="return isHoraKey(this,event)" 
                                                TabIndex="59" MaxLength="5" Enabled="False" style="display:inline-block"/>
                                            <asp:Label ID="Label75" runat="server" Text="*" Style="color: #FF0000; display:inline-block"></asp:Label>
                                         
                                            <asp:Label ID="Label79" runat="server" Text=" (HH:MM)" style="color: #FF0000; display:inline-block"></asp:Label> 

                                    
                                        </div>       
                                    
                                        <div>
                                            <div class="NotificacionDivs" style="display: inline-block">
                                                <asp:Label ID="Label83" runat="server" Text="Cuerpo Notificación:" Width="150px" 
                                                style="position: relative; bottom:10px; display:inline-block;"/>
                                                <asp:TextBox ID="txtNotificacionCuerpo" runat="server" Height="43px" Width="660px"
                                                        TextMode="MultiLine" CssClass="txtLetra" MaxLength="1200" 
                                                        Enabled="False" TabIndex="60"/>
                                                <asp:Label ID="Label84" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                            </div>                                                           
                                        </div>                                       
                                        

                                    </div>

                                    
                                  </div>

                                  <br />

                                  <div class="NotificacionTituloPestana">Recepción de Notificación</div>

                                  <div id="Div1" class="BordesColores" >
                                    <div style="clear: both; padding: 10px 5px 5px 10px;">
                                    <div class="NotificacionDivs">
                                        <asp:Label ID="Label85" CssClass="NotificacionLabel" runat="server" 
                                            Text="Tipo Recepción:" Width="150px" TabIndex="61"/>

                                       <asp:DropDownList ID="ddlTipoRecepcion" runat="server" Width="250px" TabIndex="62"
                                                Enabled="False" AutoPostBack="True" 
                                                onselectedindexchanged="ddlTipoRecepcion_SelectedIndexChanged">
                                                <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                                <asp:ListItem Text="RECIBIDO"></asp:ListItem>
                                                <asp:ListItem Text="NO RECIBIDO"></asp:ListItem>
                                                <asp:ListItem Text="RECIBIDO, NO IDENTIFICADO"></asp:ListItem>
                                                <asp:ListItem Text="NO EXISTE DIRECCIÓN"></asp:ListItem>
                                                <asp:ListItem Text="DESTINATARIO NO CONOCIDO"></asp:ListItem>
                                                <asp:ListItem Text="OTROS"></asp:ListItem>
                                            </asp:DropDownList>

                                        <asp:Label ID="Label32" runat="server" Text="*" Style="color: #FF0000"></asp:Label>       

                                        <asp:Label ID="Label93" CssClass="NotificacionLabel" runat="server" Text="Número de Cédula:" Width="130px" style="display:inline-block; padding: 0px 0px 0px 20px;" />

                                        <asp:TextBox ID="txtNroCedula" runat="server" CssClass="txtLetra"
                                                onkeypress="return isNumberKey(event)" TabIndex="63" 
                                                MaxLength="20" Enabled="False" Width="140px" />
                                            
                                        <asp:Label ID="Label94" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 

                                    </div>
                                                                                             
                                    <div>

                                        <div class="NotificacionDivs"">
                                            <asp:Label ID="Label91" CssClass="NotificacionLabel" runat="server" 
                                                Text="Persona que Recibe:" Width="150px" />
                                            <asp:TextBox ID="txtPerRecep" runat="server" CssClass="txtLetra" onkeypress="return isSujeto(event)"
                                                    TabIndex="64" MaxLength="100" Enabled="False" Width="250px" />
                                            <asp:Label ID="Label92" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 

                                        </div>
                                        


                                    </div>

                                    <div class="NotificacionDivs">
                                       
                                        <asp:Label ID="Label31" runat="server" Text="Fecha/Hora Recepción:" Width="150px" style="display:inline-block"/>

                                        <SGAC_Fecha:ctrlDate ID="txtFchRecep" runat="server" style="display:inline-block" TabIndex="65"/>
                                        <asp:Label ID="Label36" runat="server" Text="*" Style="color: #FF0000; display:inline-block"></asp:Label> 

                                        <asp:TextBox ID="txtHoraRecep" runat="server" Width="60px" CssClass="txtLetra"
                                            onBlur="validaHora(this)" onkeypress="return isHoraKey(this,event)" 
                                            TabIndex="66" MaxLength="5" Enabled="False" style="display:inline-block"/>
                                        <asp:Label ID="Label58" runat="server" Text="*" Style="color: #FF0000; display:inline-block"></asp:Label>
                                         
                                        <asp:Label ID="Label64" runat="server" Text=" (HH:MM)" style="color: #FF0000; display:inline-block"></asp:Label> 

                                    
                                    </div>
                                    
                                    <div>
                                        <div class="NotificacionDivs" style="display: inline-block">
                                           <asp:Label ID="lblNotiObservacion" runat="server" Text="Comentario:" Width="150px" 
                                           style="position: relative; bottom:15px; display:inline-block;" />
                                           <asp:TextBox ID="txtNotiObservacion" runat="server" Height="35px" Width="423px"
                                                    TextMode="MultiLine" CssClass="txtLetra" TabIndex="67" MaxLength="1200" 
                                                    Enabled="False"/>
                                            
                                        </div>

                                     </div> 
                                    </div> 
                                                               
                                    
                                  </div>

                                    <div style=" background:#D3D3D3; height:30px;">
                                        <div style=" float:right; position:relative; right:15px; top:3px;">

                                            <asp:Button ID="btnNotiAceptar" runat="server" Text="Adicionar" CssClass="btnSave"
                                            OnClick="btnNotiAceptar_Click" OnClientClick="return ValidarNotificaciones()"
                                            Enabled="True" TabIndex="68" Width="100px" style="text-align:right;"/>

                                            <asp:Button ID="btnNotiCancelar" runat="server" Text="  Limpiar" CssClass="btnLimpiar"
                                            OnClick="btnNotiCancelar_Click" TabIndex="69" Width="100px" style="text-align:center;" OnClientClick="return LimpiarNotificacionEnvio()" />
        
                                        </div>                                                 
                                    </div> 
                                        

                                
                                <br />
                                
                                <div style="height:220px; margin: 5px 0 5px 0;">
                                    <asp:GridView ID="gdvNotificaciones" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                    AutoGenerateColumns="False" GridLines="None" DataKeyNames="ajno_vPersonaRecibeNotificacion,ajno_iActoJudicialNotificacionId,ajno_sViaEnvioId,ajno_sEstadoId,ajno_sGuardado,ajno_dFechaHoraNotificacion"
                                    OnRowCommand="gdvNotificaciones_RowCommand" 
                                    onrowdatabound="gdvNotificaciones_RowDataBound" 
                                    onrowcreated="gdvNotificaciones_RowCreated">
                                    <Columns>
                                        <asp:BoundField HeaderText="Nro." 
                                            DataField="ajno_sCorrelativo" >
                                            <ItemStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Vía Envío" DataField="ajno_vViaEnvio" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Estado" 
                                            DataField="ajno_vEstadoInicial" >
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha Hora Notificacion Enviada" 
                                            DataField="ajno_dFechaHoraNotificacion" 
                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" >
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Estado" DataField="esta_vDescripcionCorta" >
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ajno_dFechaHoraRecepcion" 
                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" 
                                            HeaderText="Fecha Hora Notificacion Recibida">
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ajno_vTipRecepcion" HeaderText="Tipo Recepcion">
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Imprimir" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnImprimir" CommandName="Imprimir" ToolTip="Imprimir Notificación" TabIndex="70"
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_print.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actas">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnNotificar" runat="server" 
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" TabIndex="71"
                                                    CommandName="Notificar" ImageUrl="../Images/img_16_notification.png" 
                                                    ToolTip="Notificar" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ver">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnFind" runat="server" TabIndex="72"
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" 
                                                    ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Ver Notificación" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Notificación" TabIndex="73"
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_modify.png"/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Eliminar Notificación" TabIndex="74"
                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="slt" />
                                </asp:GridView>
                                </div>
                                <div>
                                    <PageBarContent:PageBar ID="ctrlPaginadorActuacion" runat="server" OnClick="ctrlPaginadorActuacion_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-3">
                        <asp:UpdatePanel runat="server" ID="updActas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarActa" runat="server"></ToolBarButton:ToolBarButtonContent>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <Label:Validation ID="Validation3" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label26" runat="server" Text="PERSONA/EMPRESA NOTIFICADA :    " Style="font-weight: 700;
                                                color: #800000;" />
                                            &nbsp;
                                            <asp:Label ID="lblNombreActa" runat="server" Text="" Style="font-weight: 700;
                                                color: #800000;" />
                                            <asp:Button ID="Button10" runat="server" Text="Button" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div class="NotificacionTituloPestana" style=" clear:both;">Agregar Acta</div>
                                <table id="Table2" runat="server" border="0" style="border-style: solid; border-width: 1px;
                                    border-color: #800000; width: 860px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblActaTipo" runat="server" Text="Tipo Acta:" />
                                        </td>
                                        <td width="170px">
                                            <asp:DropDownList ID="ddlActaTipo" runat="server" Width="150px" TabIndex="53" 
                                                Enabled="False">
                                                <asp:ListItem Text="DILIGENCIAMIENTO"></asp:ListItem>
                                                <asp:ListItem Text="COMPLEMENTARIA"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label46" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                        </td>
                                        <td width="80px">
                                            <asp:Label ID="lblActaFechaHora" runat="server" Text="Fecha/Hora:" />
                                        </td>
                                        <td class="style5">
                                            <SGAC_Fecha:ctrlDate ID="txtActaFecha" runat="server"/>
                                    
                                            <asp:Label ID="Label47" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 

                                            <asp:TextBox ID="txtActaHora" runat="server" Width="50px" CssClass="txtLetra" onBlur="validaHora(this)"
                                                onkeypress="return isHoraKey(this,event)" TabIndex="55" 
                                                MaxLength="5" Enabled="False" />
                                            <asp:Label ID="Label57" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                            <asp:Label ID="Label40" runat="server" Text=" (HH:MM)" Style="color: #FF0000; display:inline;"></asp:Label> 
                                        </td>
                                        <td width="80px">
                                            <asp:Label ID="lblActaEtado" runat="server" Text="Estado:" />
                                        </td>
                                        <td width="150px">
                                            <asp:DropDownList ID="ddlActaEstado" runat="server" Width="130px" TabIndex="56" 
                                                Enabled="False">
                                                <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:Label ID="Label59" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblActaResponsable" runat="server" Text="Responsable:" />
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtActaResponsable" runat="server" Width="450px" 
                                                CssClass="txtLetra" onkeypress="return isSujeto(event)"
                                                TabIndex="57" Enabled="False" />

                                            <asp:Label ID="Label60" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label37" runat="server" Text="Cuerpo Acta:" />
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtActaCuerpo" Height="90px" runat="server" Width="730px" CssClass="txtLetra"
                                                TextMode="MultiLine" TabIndex="58" Enabled="False" />
                                            <asp:Label ID="Label62" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label34" runat="server" Text="Resultado:"  Visible="false"/>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtResultado" Height="90px" runat="server" Width="730px" CssClass="txtLetra"
                                                TextMode="MultiLine" TabIndex="59" Enabled="False" Visible="false" />
                                            <asp:Label ID="Label63" runat="server" Text="*" Style="color: #FF0000" Visible="false" ></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblActaObservacion" runat="server" Text="Observación:" 
                                                Visible="False" />
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtActaObservacion" runat="server" Width="450px" CssClass="txtLetra"
                                                TextMode="MultiLine" TabIndex="60" Visible="False" Enabled="False" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnActaAceptar" runat="server" Text="Adicionar" CssClass="btnGeneral"
                                                OnClick="btnActaAceptar_Click" OnClientClick="return ValidarActas()" Enabled="False" />
                                            <asp:Button ID="btnActaCancelar" runat="server" Text="  Limpiar" CssClass="btnLimpiar"
                                                OnClick="btnActaCancelar_Click" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%">
                                   
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvActaDiligenciamiento" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                AutoGenerateColumns="False" GridLines="None" 
                                                DataKeyNames="acjd_iActaJudicialId,acjd_sEstadoId,acjd_sGuardado,acde_sEstadoId,acjd_vTipoActa" 
                                                onrowcommand="gdvActaDiligenciamiento_RowCommand" 
                                                onrowdatabound="gdvActaDiligenciamiento_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Nro. Acta" DataField="acjd_iActaJudicialId" />
                                                    <asp:BoundField HeaderText="Tipo de Acta" DataField="acjd_vTipoActa" />
                                                    <asp:BoundField HeaderText="Fecha/Hora Envío" DataField="acjd_dFechaHoraActa" 
                                                        DataFormatString="{0:MMM-dd-yyyy HH:mm}" />
                                                    <asp:BoundField HeaderText="Responsable" DataField="acjd_vResponsable" />
                                                    <asp:BoundField HeaderText="Estado" DataField="acjd_vEstado" />
                                                    
                                                    <asp:BoundField DataField="acjd_vObservaciones" HeaderText="Observaciones" />
                                                    
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnFind" CommandName="Ver" ToolTip="Ver Notificación" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEdit" CommandName="Editar" ToolTip="Editar Acta" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Imprimir">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEnviar" CommandName="Enviar" ToolTip="Imprimir Acta" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_print.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Observar">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnObserva" CommandName="Observar" ToolTip="Observar Acta" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_tramite_rechazar.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Eliminar Notificación"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label52" runat="server" Text="DETALLE DE ACTA COMPLEMENTARIA" Style="font-weight: 700;
                                                color: #800000;" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvActaComplementaria" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="True">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Nro. Acta" />
                                                    <asp:BoundField HeaderText="Fecha/Hora Envío" />
                                                    <asp:BoundField HeaderText="Responsable" />
                                                    <asp:BoundField HeaderText="Estado" />
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnFind" CommandName="Ver" ToolTip="Ver Notificación" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Notificación"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Eliminar Notificación"
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>

                                <div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hd_Detalle" runat="server" />
                                                <asp:HiddenField ID="hidDocumentoSoloNumero" Value ="0" runat="server" />
                                                <cc1:ModalPopupExtender ID="ModalPanel_Cab" runat="server" TargetControlID="hd_Detalle"
                                                    PopupControlID="Panel_Cab" Drag="true" BackgroundCssClass="modalBackground">
                                                </cc1:ModalPopupExtender>
                                                <div align="left">
                                                    <asp:Panel ID="Panel_Cab" runat="server" CssClass="st_PanelPopupAjax01" HorizontalAlign="Left"
                                                        BackColor="#FFFFFF" BorderColor="#000000" BorderStyle="Groove" Width="500px"
                                                        Style="display: none">
                                                        <table cellspacing="0" width="100%">
                                                            <tr>
                                                                <td class="encebezadotabla-2" width="97%">
                                                                    Observacion del Acta
                                                                </td>
                                                                <td align="right" width="3%" valign="top">
                                                                    <asp:ImageButton ID="imgCerrar" runat="server" Height="17" ImageAlign="Top" ImageUrl="../Images/img_cerrar.gif"
                                                                        ToolTip="Cerrar Ventana" Visible="true" Width="17" OnClick="imgCerrar_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td width="100px" colspan="2">
                                                                    <asp:Label ID="lblValidarDetalle" runat="server" Text="Falta validar algunos campos."
                                                                        CssClass="hideControl" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100px">
                                                                    <asp:Label ID="lblsubservicio" runat="server" ForeColor="Black" Text="Observacion:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescSub" runat="server" MaxLength="50" TextMode="MultiLine"
                                                                        CssClass="txtLetra" Width="95%"></asp:TextBox>
                                                                    <asp:Label ID="lblDescripcionDetalle" runat="server" Text="*" CssClass="lblVal"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btngrabarSubServicio" runat="server" CssClass="btnGeneral" 
                                                                        Text="Grabar" Width="97px" OnClientClick="ValidarObservacion()" OnClick="btngrabarSubServicio_Click" />
                                                                 
                                                                    <asp:Button ID="btnModificarSub" runat="server" Text="    Cancelar" CssClass="btnGeneral"  OnClick="btncancelarSub_Click"/> 

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
         $(document).ready(function () {
             Load();
         });


         function Load() {

            $("#<%=txtManteria.ClientID %>").attr('maxlength','200');
            $("#<%=txtOrgano.ClientID %>").attr('maxlength', '200');
            $("#<%=txtObs.ClientID %>").attr('maxlength', '500');
            $("#<%=txtManteria.ClientID %>").attr('maxlength', '200');
            $("#<%=txtNotificacionCuerpo.ClientID %>").attr('maxlength', '1200');



            $("#<%=txtNumDoc3.ClientID %>").bind("keypress", function (eKd) {

                var vSoloNumero = $("#<%=hidDocumentoSoloNumero.ClientID %>").val();           

                if (vSoloNumero == "1") {
                    return isNumberKey(eKd);
                }
                else {
                    return isLetraNumeroDoc(eKd);
                }
            });

    


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

             var letras = "aeiouAEIOU-";
             var tecla = String.fromCharCode(charCode);
             var n = letras.indexOf(tecla);
             if (n > -1) {
                 letra = true;
             }

             return letra;
         }

        function Imprimir_expediente(row, ID) {
            document.getElementById('<%= hIndex.ClientID %>').value = row;
            document.getElementById('<%= hId_Actuacion_Select.ClientID %>').value = ID;
            document.getElementById('<%= btn_imprimir_expediente.ClientID %>').click();
        }
</script>
</asp:Content>

