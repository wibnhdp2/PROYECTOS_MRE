<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="FrmActoCivil.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActoCivil" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc2" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlAdjunto.ascx" TagName="ctrlAdjunto"
    TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ctrlTool"
    TagName="ctrlTool" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlGridParticipante.ascx" TagName="ctrlGridParticipante"
    TagPrefix="uc2" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlUbigeo.ascx" TagName="ctrlUbigeo"
    TagPrefix="uc4" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlReimprimirbtn.ascx" TagName="ctrlReimprimirbtn"
    TagPrefix="uc3" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlBajaAutoadhesivo.ascx" TagName="ctrlBajaAutoadhesivo"
    TagPrefix="uc5" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="content-Type" content="text/html; charset=UTF-8" />
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/Validacion/Text.js" type="text/javascript"></script>
    <script src="../Scripts/tinymce/tinymce.min.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            height: 19px;
        }
        .Oculto
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);

        $(document).ready(function () {
            Load();
            funSetSession("vNavegadorActivo", navigator.userAgent.toLowerCase());

        });
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
        function OnlyNumeros(e) {
            var key = window.Event ? e.which : e.keyCode
            return (key >= 48 && key <= 57)
        }
        function ValidarNumeroDocumento(event) {
            var CmbTipoDoc = $("#<%=ddl_TipoDocParticipante.ClientID %>").val();
            var bol = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

            document.getElementById("<%=txtNroDocParticipante.ClientID %>").maxLength = valoresDocumento[1];

                                                  var documento = document.getElementById("<%=txtNroDocParticipante.ClientID %>").value;
            var documentosinespacios = documento.replace(/ /g, "");
            document.getElementById("<%=txtNroDocParticipante.ClientID %>").value = documentosinespacios;
                                                  if (valoresDocumento[2] == "True") {
                                                      bol = OnlyNumeros(event);
                                                  }
                                                  else {
                                                      bol = isNumeroLetra(event);
                                                  }



                                                  return bol;
                                              }
        function Ejecuar_Script() {


            var fechaInicial = get_Date(document.getElementById('<%=ctrFecRegistro.FindControl("TxtFecha").ClientID %>').value);
            var fechaFinal = get_Date(document.getElementById('<%=txtFecNac.FindControl("TxtFecha").ClientID %>').value);

            var d_Validar_Fecha_1 = is_Date(document.getElementById('<%=ctrFecRegistro.FindControl("TxtFecha").ClientID %>'));
            var d_Validar_Fecha_2 = is_Date(document.getElementById('<%=txtFecNac.FindControl("TxtFecha").ClientID %>'));

            var d_Validar_Fecha_3 = is_Date(document.getElementById('<%=CtrldFecNacimientoParticipante.FindControl("TxtFecha").ClientID %>'));



            if (d_Validar_Fecha_1) {

                var nameControl = document.getElementById('<%= ctrFecRegistro.FindControl("lblErrorDate").ClientID %>')
                document.getElementById('<%= ctrFecRegistro.FindControl("lblErrorDate").ClientID %>').innerHTML = "";

            }

            if (d_Validar_Fecha_2) {

                var nameControl = document.getElementById('<%= txtFecNac.FindControl("lblErrorDate").ClientID %>')
                document.getElementById('<%= txtFecNac.FindControl("lblErrorDate").ClientID %>').innerHTML = "";

            }

            if (d_Validar_Fecha_3) {

                var nameControl = document.getElementById('<%= CtrldFecNacimientoParticipante.FindControl("lblErrorDate").ClientID %>')
                document.getElementById('<%= CtrldFecNacimientoParticipante.FindControl("lblErrorDate").ClientID %>').innerHTML = "";

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


        function calcular_edad(fecha) {
            var fechaActual = new Date()
            var diaActual = fechaActual.getDate();
            var mmActual = fechaActual.getMonth() + 1;
            var yyyyActual = fechaActual.getFullYear();
            FechaNac = fecha.split("/");
            var diaCumple = FechaNac[0];
            var mmCumple = FechaNac[1];
            var yyyyCumple = FechaNac[2];
            //retiramos el primer cero de la izquierda
            if (mmCumple.substr(0, 1) == 0) {
                mmCumple = mmCumple.substring(1, 2);
            }
            //retiramos el primer cero de la izquierda
            if (diaCumple.substr(0, 1) == 0) {
                diaCumple = diaCumple.substring(1, 2);
            }
            var edad = yyyyActual - yyyyCumple;

            //validamos si el mes de cumpleaños es menor al actual
            //o si el mes de cumpleaños es igual al actual
            //y el dia actual es menor al del nacimiento
            //De ser asi, se resta un año
            if ((mmActual < mmCumple) || (mmActual == mmCumple && diaActual < diaCumple)) {
                edad--;
            }
            return edad;
        };


        function Load() {
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
                $('#<%= txtCodAutoadhesivo.ClientID %>').focus();
            });

            $(function () {
                $('input:CheckBox:first').focus();
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
            });
            $(function () {
                $(':CheckBox').bind('keydown', function (e) {
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
            });


            $(function () {



                $("#<%=txtNomParticipante.ClientID %>").on('keypress', function (e) {
                    var key = (e.which) ? e.which : e.keyCode;   /*e.keyCode || e.which;*/
                    var tecla = String.fromCharCode(key).toLowerCase();


                    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyzñ.";
                    var especiales = ['8', '37', '39', '46'];

                    tecla_especial = false
                    for (var i in especiales) {
                        if (key == especiales[i]) {
                            tecla_especial = true;
                            break;
                        }
                    }

                    if ((letras.indexOf(tecla) == -1 && !tecla_especial)) {
                        return false;
                    }
                });


                $("#<%=txtApeMatParticipante.ClientID %>").on('keypress', function (e) {
                    var key = (e.which) ? e.which : e.keyCode;   /*e.keyCode || e.which;*/
                    var tecla = String.fromCharCode(key).toLowerCase();


                    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyzñ.";
                    var especiales = ['8', '37', '39', '46'];

                    tecla_especial = false
                    for (var i in especiales) {
                        if (key == especiales[i]) {
                            tecla_especial = true;
                            break;
                        }
                    }

                    if ((letras.indexOf(tecla) == -1 && !tecla_especial)) {
                        return false;
                    }
                });

                $("#<%=txtApePatParticipante.ClientID %>").on('keypress', function (e) {
                    var key = (e.which) ? e.which : e.keyCode;   /*e.keyCode || e.which;*/
                    var tecla = String.fromCharCode(key).toLowerCase();


                    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyzñ.";
                    var especiales = ['8', '37', '39', '46'];

                    tecla_especial = false
                    for (var i in especiales) {
                        if (key == especiales[i]) {
                            tecla_especial = true;
                            break;
                        }
                    }

                    if ((letras.indexOf(tecla) == -1 && !tecla_especial)) {
                        return false;
                    }
                });

                $("#<%=txtDireccionParticipante.ClientID %>").on('keypress', function (e) {
                    var key = (e.which) ? e.which : e.keyCode;   /*e.keyCode || e.which;*/
                    var tecla = String.fromCharCode(key).toLowerCase();


                    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyzäëïöüABCDEFGHIJKLMNÑOPQRSTUVWXYZÄËÏÖÜ.1234567890-:,°/.#()&*ª";
                    var especiales = ['8', '13', '37', '39'];

                    tecla_especial = false
                    for (var i in especiales) {
                        if (key == especiales[i]) {
                            tecla_especial = true;
                            break;
                        }
                    }

                    if ((letras.indexOf(tecla) == -1 && !tecla_especial)) {
                        return false;
                    }
                });

                $("#<%= rbSi.ClientID %>").click(function () {

                    $("#<%= HF_TIENE_DOCUMENTO.ClientID %>").val('1');
                    $("#<%= ddl_TipoDocParticipante.ClientID %>").attr('disabled', false);
                    $("#<%= txtNroDocParticipante.ClientID %>").attr('disabled', false);




                    $("#<%= ddl_TipoDocParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtNroDocParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= ddl_NacParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtNomParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtApeMatParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtApePatParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtDireccionParticipante.ClientID %>").css("border", "1px solid #888888");



                    $("#<%= ddl_TipoDocParticipante.ClientID %>").val('0');
                    $("#<%= txtNroDocParticipante.ClientID %>").val('');
                    $("#<%= ddl_NacParticipante.ClientID %>").val('0');
                    var str_intTipoActa = $("#<%=ddlTipoActa.ClientID %>").val();
                    
                    var str_HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO = $("#<%= HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO.ClientID %>").val();
                    var str_TipoParticipante = $("#<%= ddl_TipoParticipante.ClientID %>").val();
                    
                    



                    $("#<%= txtNomParticipante.ClientID %>").val('');
                    $("#<%= txtApeMatParticipante.ClientID %>").val('');
                    $("#<%= txtApePatParticipante.ClientID %>").val('');
                    $("#<%= txtDireccionParticipante.ClientID %>").val('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_ContDepParticipanteAP").ClientID %>').val('0');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_PaisCiudadParticipanteAP").ClientID %>').html('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_CiudadDistritoParticipanteAP").ClientID %>').html('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_CentroPobladoParticipanteAP").ClientID %>').html('');


                    habilitarControlRune(true);
                    desabilitar_declarante_registrador_civil();



                });

                $("#<%= rbNo.ClientID %>").click(function () {

                    $("#<%= HF_TIENE_DOCUMENTO.ClientID %>").val('0');
                    $("#<%= ddl_TipoDocParticipante.ClientID %>").attr('disabled', true);
                    $("#<%= txtNroDocParticipante.ClientID %>").attr('disabled', true);

                    $("#<%= ddl_TipoDocParticipante.ClientID %>").val('0');
                    $("#<%= txtNroDocParticipante.ClientID %>").val('');

                    $("#<%= ddl_NacParticipante.ClientID %>").val('0');
                    $("#<%= txtNomParticipante.ClientID %>").val('');
                    $("#<%= txtApeMatParticipante.ClientID %>").val('');
                    $("#<%= txtApePatParticipante.ClientID %>").val('');
                    $("#<%= txtDireccionParticipante.ClientID %>").val('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_ContDepParticipanteAP").ClientID %>').val('0');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_PaisCiudadParticipanteAP").ClientID %>').html('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_CiudadDistritoParticipanteAP").ClientID %>').html('');
                    $('#<%= ctrlUbigeo1.FindControl("ddl_CentroPobladoParticipanteAP").ClientID %>').html('');


                    $("#<%= ddl_TipoDocParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtNroDocParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= ddl_NacParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtNomParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtApeMatParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtApePatParticipante.ClientID %>").css("border", "1px solid #888888");
                    $("#<%= txtDireccionParticipante.ClientID %>").css("border", "1px solid #888888");

                    habilitarControlRune(false);


                    var str_intTipoActa = $("#<%=ddlTipoActa.ClientID %>").val();
                    
                    var str_HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO = $("#<%= HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO.ClientID %>").val();
                    var str_TipoParticipante = $("#<%= ddl_TipoParticipante.ClientID %>").val();
                    
                    
                    //if (str_intTipoActa == str_Acta_Matrimonio) {

                    //    if (str_HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO == str_TipoParticipante) {

                            
                            //$("#<%= ddl_NacParticipante.ClientID %>").attr('disabled', true);
                        //}
                        //else {
                            //$("#<%= ddl_NacParticipante.ClientID %>").val('0');
                            //$("#<%= ddl_NacParticipante.ClientID %>").attr('disabled', false);
                        //}
                    //}
                    //if (str_intTipoActa == str_Acta_Nacimiento) {
                        //$("#<%= ddl_NacParticipante.ClientID %>").val(str_HF_NACIONALIDAD_PERUANA);
                        //$("#<%= ddl_NacParticipante.ClientID %>").attr('disabled', false);
                    //}


                    desabilitar_declarante_registrador_civil();



                });

                $("#<%=ddl_NacParticipante.ClientID %>").change(function () {


                });


                $("#imgBuscarDocumento").on('click', function () {

                    Documento_OnClick();

                });

                //                $("#<%=BtnCalcularEded.ClientID %>").on('click', function (e) {

                //                    var fechaInicial = get_Date(document.getElementById('<%=CtrldFecNacimientoParticipante.FindControl("TxtFecha").ClientID %>').value);
                //                    var d_Validar_Fecha_1 = is_Date(document.getElementById('<%=CtrldFecNacimientoParticipante.FindControl("TxtFecha").ClientID %>'));


                //                    $("#<%=LblEdad2.ClientID %>").hide();
                //                    if (d_Validar_Fecha_1) {

                ////                        var edad = calcular_edad(fechaInicial);

                ////                        $("#<%=LblEdad2.ClientID %>").val(edad);
                ////                        $("#<%=LblEdad2.ClientID %>").show();

                //                    }

                //                    e.preventDefault();

                //                });

                $("#<%=btnAceptar.ClientID %>").on('click', function (e) {
                    var bol = ActoCivil_Participantes();
                    if (bol) {

                        //  $("#<%=LblEdad2.ClientID %>").hide();
                    }

                });


                try {
                    tinyMCE.remove();
                }
                catch (e) {
                    LoadTinyMCE()
                }


                tinyMCE.init({
                    mode: "textareas",

                    editor_selector: "mceEditor",
                    plugins: ["textcolor"],
                    toolbar1: "bold italic |alignleft aligncenter alignright alignjustify | forecolor",
                    toolbar2: "fontselect | fontsizeselect",
                    //            readonly: iEstado,
                    language: "es"
                });


            });


            function habilitarControlRune(bol) {

                $("#<%= ddl_NacParticipante.ClientID %>").attr('disabled', bol);
                $("#<%= txtNomParticipante.ClientID %>").attr('disabled', bol);
                $("#<%= txtApeMatParticipante.ClientID %>").attr('disabled', bol);
                $("#<%= txtApePatParticipante.ClientID %>").attr('disabled', bol);
                //                $("#<%= txtDireccionParticipante.ClientID %>").attr('disabled', bol);
            }


            function desabilitar_declarante_registrador_civil() {

               
            }



            function OnlyNumeros(e) {
                var key = window.Event ? e.which : e.keyCode
                return (key >= 48 && key <= 57)
            }


            function validarSoloLetras(txt) {

                var valido = true;

                var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ]");

                if (txt.value.length > 0 && charpos >= 0) {

                    txt.value = '';
                }

                return valido;
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



            /* Solo Nombres*/
            function ValidarSujeto(evt) {
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
                return letra;
            }



        }


        /*FUNCIONES GENERALES*/
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

        function funSetSession(variable, valor) {
            var url = 'FrmActoCivil.aspx/SetSession';
            var prm = {};
            prm.variable = variable;
            prm.valor = valor;
            var rspta = execute(url, prm);
        }

        function funGetSession(variable) {
            var url = 'FrmActoCivil.aspx/GetSession';
            var prm = {};
            prm.variable = variable;
            var rspta = execute(url, prm);

            return JSON.parse(JSON.stringify(rspta)).d;
        }

    </script><!-- Script by hscripts.com --><script type="text/javascript">
        var r = { 'special': /[\W]/g }
        function valid(o, w) {
            o.value = o.value.replace(r[w], '');
        }


        function abrirPopupBusquedaActas() {
            document.getElementById('modalBusquedaTitular').style.display = 'block';
        }

        function cerrarPopupBusquedaActas() {
            document.getElementById('modalBusquedaTitular').style.display = 'none';
        }
        function limpiarCamposBusquedaActas() {
            $("#<%=txtBuscarApPaterno.ClientID %>").val("");
            $("#<%=txtBuscarApMaterno.ClientID %>").val("");
            $("#<%=txtBuscarNombre.ClientID %>").val("");
            $("#<%=txtBuscarApPaterno.ClientID %>").focus();
        }
        function limpiarAnotacion() {
            $("#<%=txtNumeroActa.ClientID %>").val("");
            $("#<%=txtTitular.ClientID %>").val("");
            $("#<%=ddlTipoActaAnotacion.ClientID %>").val('0');
            $("#<%=cmb_TipoAnotacion.ClientID %>").val('0');
            tinyMCE.activeEditor.setContent("");
            $("#<%=txtNumeroActa.ClientID %>").focus();
        }

    </script><script type="text/javascript">
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
    </script><!--Script by hscripts.com--><div>
        <asp:HiddenField ID="HFGUID" runat="server" />
        <asp:UpdatePanel ID="updbotones" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAnotacion" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCopiaCert" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <table class="mTblTituloM" align="center">
                    <tr>
                        <td>
                            <h2>
                                <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuación Consular"></asp:Label></h2>
                        </td>
                        <td style="text-align: right">
                            <asp:LinkButton ID="Tramite" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Blue"
                                Font-Underline="True" OnClick="Tramite_Click">Regresar a Trámites</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblTituloTarifa" CssClass="titulo_tarifa" runat="server" Text="1 - Por Inscribir un nacimiento, un matrimonio o una defunción."></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 90%;" align="center" class="mTblSecundaria" bgcolor="#4E102E">
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDestino" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                                            Font-Size="10pt" Font-Underline="false" ForeColor="White" Text="SALAZAR VEGA, FIORELLA KARINA - DNI: 09623456"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%--ENCABEZADO DEL REGISTRO CIVIL--%>
                <table id="Table2" style="width: 90%;" align="center" runat="server">
                    <tr>
                        <td align="left">
                        </td>
                        <td style="margin-left: 80px" align="right">
                            <asp:Button ID="btnAnotacion" runat="server" Text="       Nueva Anotación" CssClass="btnAnotacion"
                                OnClick="btnAnotacion_Click" Visible="true" />
                            &nbsp;
                            <asp:Button ID="btnCopiaCert" runat="server" Text="       Copia Certificada" CssClass="btnCopiaCertificada"
                                OnClick="btnCopiaCertificada_Click" Visible="true" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--DATOS DEL LUGAR DE OCURRENCIA--%>
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
                                                <asp:HiddenField ID="HF_ACTUACIONDET_ID" runat="server" />
                                                <asp:HiddenField ID="HF_ESRune" runat="server" Value="0" />
                                                <asp:HiddenField ID="HF_Validar_Caracteres" runat="server" />
                                                <asp:HiddenField ID="HF_iReferenciaId" runat="server" Value="0" />
                                                <asp:HiddenField ID="HF_ACT_ID" runat="server" Value="0" />
                                                <asp:HiddenField ID="HF_iPersonaID" runat="server" Value="0" />
                                                <br />
                                                <asp:HiddenField ID="HF_RECI_AUX" runat="server" Value="0" />
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
                                                <asp:HiddenField ID="HFAutodhesivo" runat="server" />
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
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top;">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtIdTarifa" runat="server" Width="38px" CssClass="campoNumero"
                                                                                        MaxLength="5" ToolTip="Coloque el número de la seccion del tarifario" onkeypress="return isNumeroLetra(event)"
                                                                                        onpaste="return false" AutoPostBack="True" OnTextChanged="txtIdTarifa_TextChanged" />
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
                                        <tr class="Oculto">
                                            <td>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Label3" runat="server" Text="Tipo:" class="Oculto"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlActuacionTipo" runat="server" Height="20px" Width="200px"
                                                    class="Oculto" AutoPostBack="True" Enabled="false">
                                                </asp:DropDownList>
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
                                                                OnSelectedIndexChanged="cmb_TipoPago_SelectedIndexChanged" Enabled="True" />
                                                            <asp:Label ID="lblCO_cmb_TipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                        </td>
                                                        <td align="right" style="width: 70px">
                                                            <asp:Label ID="lblExoneracion" runat="server" Text="Ley que exonera el pago:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlExoneracion" runat="server" Enabled="True" Height="20px"
                                                                Width="414px" />
                                                            <asp:Label ID="lblValExoneracion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            <asp:RadioButton ID="RBNormativa" runat="server" Text="" Style="cursor: pointer"
                                                                GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="right" style="width: 70px">
                                                            <asp:Label ID="lblSustentoTipoPago" runat="server" Text="Sustento:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSustentoTipoPago" runat="server" Width="410px" CssClass="txtLetra"></asp:TextBox>
                                                            <asp:Label ID="lblValSustentoTipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text="" Style="cursor: pointer"
                                                                GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()" />
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
                                                <asp:TextBox ID="txtCantidad" runat="server" Width="100px" AutoPostBack="True" Enabled="True"
                                                    CssClass="campoNumero" MaxLength="2" OnTextChanged="txtCantidad_TextChanged" />
                                                &nbsp;
                                                <asp:Label ID="lblCO_txtCantidad" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                <asp:Label ID="lblLeyenda" runat="server" Text=" (Presionar tecla ENTER para calcular total)"
                                                    Font-Bold="true"></asp:Label>
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
                                                            <asp:TextBox ID="txtMtoCancelado" runat="server" Width="150px" CssClass="campoNumero"
                                                                Enabled="False" />
                                                            <asp:Label ID="lblCO_txtMtoCancelado" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
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
                                                    CssClass="txtLetra" MaxLength="500" Enabled="False" />
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
                                                <asp:GridView ID="gdvActuaciones" runat="server" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="False" CssClass="mGrid" GridLines="None" Width="100%" OnRowCommand="gdvActuaciones_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="iActuacionId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="iActuacionId"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="ActuacionDetalleId" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sSeccionId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="sSeccionId"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTarifarioId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="sTarifarioId"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCorrelativoActuacion" HeaderText="R.G.E.">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCorrelativoTarifario" HeaderText="Corr. Tarifa">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dFechaRegistro" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}"
                                                            HeaderText="Fecha">
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTarifa" HeaderText="Tarifa">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDescripcion" HeaderText="Descripción">
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="sOficinaConsularId" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vOficinaCiudad" HeaderText="Oficina Consular">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dFechaDigitaliza" HeaderStyle-CssClass="ColumnaOculta"
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" HeaderText="Fec. Hora Digitaliza">
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" />
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vUsuarioDigitaliza" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="Usuario Digitaliza">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sUsuarioDigitaliza" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="sUsuarioDigitaliza" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCodigoInsumo" HeaderText="Código Autoadhesivo" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <%--Opciones Grilla--%>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderText="Editar" ItemStyle-HorizontalAlign="Center"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditarAct" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="EditarAct" ImageUrl="../Images/img_16_edit.png" ToolTip="Editar Actuación" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updFormato" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgBuscarActaAnterior" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscarTitularActa" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>

                                   

                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hInicioTramite" runat="server" />
                                                            <asp:Button ID="BtnVistaPrevia" runat="server" CssClass="btnVisualizar" Text="  Vista Previa"
                                                                Width="125px" OnClick="BtnVistaPrevia_Click" OnClientClick="return abrirPopupEspera();" />
                                                        </td>
                                                        <td>
                                                            <ctrlTool:ctrlTool ID="ctrlToolbarFormato" runat="server"></ctrlTool:ctrlTool>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <asp:HiddenField ID="hifRegistroCivil" runat="server" Value="0" />
                                            </td>
                                            <td align="right" style="width: 130px;">
                                                <table id="tablaCUI" runat="server" style="width: 100px; height: 50px; border-style: solid;
                                                    border-color: #a9a9a9; border-width: thin; padding-bottom: 5px; padding-top: 5px;
                                                    padding-left: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkconCUI" runat="server" Text="CON CUI" AutoPostBack="true" Style="cursor: pointer"
                                                                OnCheckedChanged="chkconCUI_CheckedChanged" />
                                                            <br />
                                                            <asp:CheckBox ID="chksinCUI" runat="server" Text="SIN CUI" AutoPostBack="true" Style="cursor: pointer"
                                                                OnCheckedChanged="chksinCUI_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                <asp:Label ID="Label445" runat="server" Font-Bold="true" Text="Tipo Acta:"></asp:Label>
                                                &nbsp;
                                                <asp:DropDownList ID="ddlTipoActa" runat="server" AutoPostBack="true" Font-Bold="true"
                                                    OnSelectedIndexChanged="ddlTipoActa_SelectedIndexChanged" Style="cursor: pointer"
                                                    Width="120px" />
                                                <asp:Label ID="Label4" runat="server" Style="color: #FF0000" Text="*"></asp:Label>

                                                    </td>
                                                </tr>
                                                <tr>
                                                <td>
                                                <table id="tablaInscripcionOficio" runat="server" style="padding-bottom: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkInscripcionOficio" Text="Inscripción de Oficio" 
                                                                 runat="server" AutoPostBack="true" Style="cursor: pointer" OnCheckedChanged="chkInscripcionOficio_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                                </tr>
                                            </table>

                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <table id="tablaReconocimientoAdopcion" runat="server" style="padding-bottom: 5px;">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReconocimientoAdopcion" Text="¿Reconocimiento o Adopción?" runat="server"
                                                    AutoPostBack="true" Style="cursor: pointer" OnCheckedChanged="chkReconocimientoAdopcion_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tablaReconstitucionReposicion" runat="server" style="padding-bottom: 5px;">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReconstitucionReposicion" Text="¿Reconstitución o Reposición?"
                                                    runat="server" AutoPostBack="true" Style="cursor: pointer" OnCheckedChanged="chkReconstitucionReposicion_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="pnlActaAnterior" runat="server" style="width: 100%; border-style: solid;
                                        border-color: #800000; border-width: thin; padding-left: 10px; padding-right: 10px;
                                        padding-bottom: 10px; padding-top: 10px;">
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label29" runat="server" Text="Nro.del Acta anterior:"></asp:Label>
                                            </td>
                                            <td style="width: 150px;">
                                                <asp:TextBox ID="txtNumeroActaAnterior" runat="server" Width="100px" MaxLength="10"
                                                    onkeypress="return validatenumber(event)"></asp:TextBox>
                                                &nbsp;<asp:ImageButton ID="imgBuscarActaAnterior" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                    ToolTip="Buscar Acta Anterior" OnClick="imgBuscarActaAnterior_Click" />
                                            </td>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label30" runat="server" Text="Titular:"></asp:Label>
                                            </td>
                                            <td style="width: 530px;">
                                                <asp:TextBox ID="txtTitularActa" runat="server" Width="350px" MaxLength="200" class="txtLetra"
                                                    onkeypress="return ValidarSujeto(event)"></asp:TextBox>
                                                &nbsp;<asp:Label ID="Label31" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                &nbsp;
                                                <asp:Button ID="btnBuscarTitularActa" runat="server" Text="     Buscar Titular" CssClass="btnBusqueda"
                                                    OnClick="btnBuscarTitularActa_Click" Width="140px" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="border-bottom: 1px solid #800000; margin-top: 5px; margin-bottom: 5px;">
                                    </div>
                                    <%--ENCABEZADO DEL REGISTRO CIVIL--%>
                                    <table id="pnlRegCivEncabezado" runat="server" style="border-bottom: 1px solid #800000;
                                        width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="Label26" runat="server" Text="Libro: "></asp:Label>
                                            </td>
                                            <td style="width: 120px">
                                                <asp:TextBox ID="txtLibroRegCiv" runat="server" Width="100px" MaxLength="50" CssClass="txtLetra" />
                                                <asp:Label ID="lblCO_ContDepOffiReg12" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 100px">
                                                <asp:Label ID="Label27" runat="server" Text="Nro. Acta: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNroActa" runat="server" Width="100px" onkeypress="return validatenumber(event)"
                                                    MaxLength="10" />
                                                <asp:Label ID="lblCO_ContDepOffiReg13" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCUI" runat="server" Text="CUI: " CssClass="" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNroCUI" runat="server" Width="100px" MaxLength="8" CssClass=""
                                                    onkeypress="return validatenumber(event)" Visible="False" />
                                                <asp:ImageButton ID="imgBuscarCUI" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                            Style="width: 16px" ToolTip="Buscar" OnClick="imgBuscarCUI_Click" />
                                                <asp:Label ID="lblCO_txtNroCUI" runat="server" Style="color: #990033; font-size: x-small;"
                                                    Text="(8 dígitos)"></asp:Label>
                                                <asp:HiddenField ID="hCUI" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label44" runat="server" Text="Fecha Registro: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaRegistro" runat="server" Width="90px" Enabled="False" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--SECCION PARA EL ACTA DE MATRIMONIO--%>
                                    <table id="pnlDatosAdicioActaMatrim" runat="server" style="border-bottom: 1px solid #800000;
                                        width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="Label333" runat="server" Text="Expediente:"></asp:Label>
                                            </td>
                                            <td style="width: 120px">
                                                <asp:TextBox ID="txtNroExpediente" runat="server" Width="100px" MaxLength="50" CssClass="txtLetra" />
                                                <%-- <asp:Label ID="lblCO_ContDepOffiReg16" runat="server" Style="color: #FF0000" Text="*"></asp:Label>--%>
                                            </td>
                                            <td style="width: 120px">
                                                <asp:Label ID="Label444" runat="server" Text="Cargo Celebrante:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCargoCelebrante" runat="server" Width="95%" onkeypress="return NoCaracteresEspeciales(event)"
                                                    MaxLength="100" CssClass="txtLetra" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--DATOS DEL TITULAR--%>
                                    <table id="TitDatosTitular" runat="server" style="border-bottom: 1px solid #800000;
                                        width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDatosTitular" runat="server" Text="" Style="font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="pnlDatosTitular" runat="server" class="mTblTituloM" style="border-bottom: 1px solid #800000;
                                        width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblFechaTitular" runat="server" Text="Fecha Nac.: "></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecNac" runat="server" />
                                                <asp:Label ID="lblCO_ContDepOffiReg18" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHoraTitular" runat="server" Text="Hora Nac.: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHora" runat="server" Width="40px" onkeypress="return isHoraKey(this,event)"
                                                    onBlur="validaHora(this)" MaxLength="5"></asp:TextBox>
                                                <asp:Label ID="lblCO_ContDepOffiReg19" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                <asp:Label ID="LblFechaValija1" runat="server" Font-Size="10pt" Style="font-weight: 700;
                                                    color: #990033; font-size: x-small;" Text="(HH:mm)" Width="114px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="Género:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Genero" runat="server" Width="130px" OnSelectedIndexChanged="ddl_Genero_SelectedIndexChanged"
                                                    Style="cursor: pointer" />
                                                <asp:Label ID="lblCO_ddl_Genero" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label69" runat="server" Text="Nombres" Width="100px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombresTitular" runat="server" Width="180px" MaxLength="100" style="text-transform:uppercase;" />
                                                <asp:Label ID="Label33" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label67" runat="server" Text="Primer Apellido" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApePatTitular" runat="server" Width="180px" 
                                                     MaxLength="100" style="text-transform:uppercase;"/>
                                                <asp:Label ID="Label35" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label68" runat="server" Text="Segundo Apellido" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApeMatTitular" runat="server" Width="180px"
                                                    MaxLength="100" style="text-transform:uppercase;"/>
                                                <asp:Label ID="Label37" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--DATOS DEL LUGAR DE OCURRENCIA--%>
                                    <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDatosOcurrencia" Text="LUGAR DE OCURRENCIA" runat="server" Style="font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblNacLugarTipo" runat="server" Text="Tipo:" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_TipoOcurrencia" runat="server" Width="200px" Style="cursor: pointer" />
                                                <asp:Label ID="lblCO_ContDepOffiReg0" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 20px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNacLugar" runat="server" Text="Lugar:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtLugarOcurrencia" runat="server" Width="300px" onkeypress="return NoCaracteresEspeciales(event)"
                                                    onBlur="" CssClass="txtLetra" MaxLength="50" />
                                                <asp:Label ID="lblCO_ContDepOffiReg3" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text="Cont./Dept.:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_DeptOcurrencia" runat="server" Width="300px" AutoPostBack="true"
                                                    Style="cursor: pointer" OnSelectedIndexChanged="ddl_DeptOcurrencia_SelectedIndexChanged" />
                                                <asp:Label ID="lblCO_ContDepOffiReg1" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 20px">
                                            </td>
                                            <td>
                                                <asp:Label ID="Label24" runat="server" Text="País/Prov.:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_ProvOcurrencia" runat="server" Width="300px" Style="cursor: pointer"
                                                    OnSelectedIndexChanged="ddl_ProvOcurrencia_SelectedIndexChanged" AutoPostBack="True" />
                                                <asp:Label ID="lblCO_ContDepOffiReg4" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="Ciud./Dist.:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_DistOcurrencia" runat="server" Width="300px" Style="cursor: pointer" />
                                                <asp:Label ID="lblCO_ContDepOffiReg2" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                            </td>
                                            <td style="width: 20px">
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Centro Poblado:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddl_CentroPobladoOcurrencia" runat="server" Width="300px" Style="cursor: pointer" />
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    <br />
                                            <asp:Button ID="btnAgregarParticipante" runat="server" Text="     Agregar Participante" Enabled="false"
                                                        Width="170px" CssClass="btnNew" TabIndex="494"  OnClick="btnAgregarParticipante_Click" />
                                    <hr />
                                    <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" Width="100%" runat="server" Text="DETALLE DE PARTICIPANTE(S)"
                                                    Style="font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hTitularId" runat="server" />
                                                <asp:GridView ID="Grd_Participantes" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                    Width="100%" GridLines="None" SelectedRowStyle-CssClass="slt" DataKeyNames="iActuacionParticipanteId,iPersonaId,sTipoParticipanteId,sTipoDatoId,sTipoVinculoId"
                                                    ShowHeaderWhenEmpty="True" OnRowCommand="Grd_Participantes_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="sTipoParticipanteId" HeaderText="TipoParticipanteId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HeaderText="iActuacionParticipanteId" DataField="iActuacionParticipanteId"
                                                            HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Apellidos y Nombres" DataField="vNombreCompleto" />
                                                        <asp:BoundField HeaderText="Tipo Participante" DataField="vTipoParticipante" />
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
                                                                    CommandName="Editar" ImageUrl="~/Images/img_grid_modify.png" ToolTip="Editar Notificación" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnAnular4" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Eliminar" ImageUrl="~/Images/img_grid_delete.png" ToolTip="Eliminar Notificación" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="iPersonaId" HeaderText="iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pers_sNacionalidadId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="vDocumentoNumero" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vNombres" HeaderText="vNombres" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vApellidoPaterno" HeaderText="vApellidoPaterno" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vApellidoMaterno" HeaderText="vApellidoMaterno" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="resi_cResidenciaUbigeo" HeaderText="resi_cResidenciaUbigeo" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="resi_vResidenciaDireccion" HeaderText="resi_vResidenciaDireccion" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iActuacionParticipanteId" HeaderText="iActuacionParticipanteId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTipoDatoId" HeaderText="sTipoDatoId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTipoVinculoId" HeaderText="sTipoVinculoId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pers_sGeneroId" HeaderText="pers_sGeneroId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pers_sEstadoCivilId" HeaderText="pers_sEstadoCivilId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pers_dNacimientoFecha" HeaderText="pers_dNacimientoFecha" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pers_cNacimientoLugar" HeaderText="pers_cNacimientoLugar" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID ="hDireccion" runat="server" Value='<%# Bind("resi_vResidenciaDireccion") %>'/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
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
                                                <asp:Label ID="Label9" runat="server" Text="OBSERVACIONES" Style="font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCivilObservaciones" runat="server" Height="32px" Width="98%"
                                                    CssClass="txtLetra" onBlur="" MaxLength="300" Rows="3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <br />
                                                <asp:CheckBox ID="cbxAfirmarTexto" runat="server" Text="" Font-Size="12pt" AutoPostBack="true"
                                                    OnCheckedChanged="cbxAfirmarTexto_CheckedChanged" Font-Bold="True" Font-Names="Verdana" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updActuacionAdjuntar" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <uc1:ctrlAdjunto runat="server" ID="ctrlAdjunto" />
                                </ContentTemplate>
                                <%--<Triggers>
                                    <asp:PostBackTrigger ControlID="ctrlUploader1"></asp:PostBackTrigger>
                                </Triggers>--%>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-3">
                            <asp:UpdatePanel ID="updAnotaciones" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="Grd_Anotaciones" />
                                    <asp:AsyncPostBackTrigger ControlID="imgBuscarActa" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscarTitular" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblValidacionAnotacion" runat="server" CssClass="hideControl" Font-Size="14px"
                                                    ForeColor="Red" Text="Falta validar algunos campos."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label2" runat="server" Text="Fecha:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="ctrFecRegistro" runat="server" />
                                                <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label20" runat="server" Text="Tipo de Acta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoActaAnotacion" runat="server" Width="250px" Style="cursor: pointer"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTipoActaAnotacion_SelectedIndexChanged" />
                                                &nbsp;<asp:Label ID="Label21" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label16" runat="server" Text="Número de Acta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumeroActa" runat="server" Width="100px" MaxLength="10" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                &nbsp;<asp:ImageButton ID="imgBuscarActa" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                    ToolTip="Buscar Acta" OnClick="imgBuscarActa_Click" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="Label17" runat="server" Text="Titular:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTitular" runat="server" Width="400px" MaxLength="200" class="txtLetra"
                                                    onkeypress="return ValidarSujeto(event)"></asp:TextBox>
                                                &nbsp;<asp:Label ID="Label19" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                &nbsp;
                                                <asp:Button ID="btnBuscarTitular" runat="server" Text="     Buscar Titular" CssClass="btnBusqueda"
                                                    OnClick="btnBuscarTitular_Click" Width="140px" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px">
                                                <asp:Label ID="lblTipAnot" runat="server" Text="Tipo de Anotación:"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmb_TipoAnotacion" runat="server" Height="20px" Width="250px" />
                                                &nbsp;<asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="HFEsConsulta" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px; vertical-align: top">
                                                <asp:Label ID="lblDescAnotacion" runat="server" Text="Descripción :" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescAnotacion" runat="server" CssClass="mceEditor" MaxLength="999"
                                                    TextMode="MultiLine" Width="700px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnGrabAnotacion" runat="server" CssClass="btnSaveAnotacion" OnClick="BtnGrabAnotacion_Click"
                                                    Text="       Guardar Anotación" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <input id="btnLimpiarAnotacion" type="button" value="  Limpiar" class="btnLimpiar"
                                                    onclick="limpiarAnotacion()" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:GridView ID="Grd_Anotaciones" runat="server" AlternatingRowStyle-CssClass="alt"
                                                    AutoGenerateColumns="False" CssClass="mGrid" GridLines="None" OnRowCommand="Grd_Anotaciones_RowCommand"
                                                    OnRowCreated="Grd_Anotaciones_RowCreated" OnRowDataBound="Grd_Anotaciones_RowDataBound"
                                                    ShowHeaderWhenEmpty="True">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="iAnotacionId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="iAnotacionId"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="iActuacionDetalleId" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTipAnotacionId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="sTipAnotacionId" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dFecRegistro" DataFormatString="{0:MMM-dd-yyyy}" HeaderText="Fec. Registro"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTipAnotacion" HeaderText="Tipo Anotación">
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vComentarios" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Descripción"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDescripcionCorta" HeaderText="Descripción" ItemStyle-Width="500px" />
                                                        <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnPrint" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="ImprimirAnotacion" OnClientClick="postBack();" ImageUrl="../Images/img_16_print.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnFind" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Consultar" ImageUrl="../Images/img_gridbuscar.gif" ToolTip="Consultar Actuación Anotaciones" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Editar" ImageUrl="../Images/img_16_edit.png" ToolTip="Editar" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="anot_sTipoActaId" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="sTipoActaId" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="anot_vTitular" HeaderStyle-CssClass="ColumnaOculta" HeaderText="vTitular"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="anot_iNumeroActaAnterior" HeaderStyle-CssClass="ColumnaOculta"
                                                            HeaderText="iNumeroActaAnterior" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <uc2:ctrlPageBar ID="CtrlPageBarActAnotacion" runat="server" OnClick="CtrlPageBarActAnotacion_Click"
                                                    Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-4">
                            <asp:UpdatePanel ID="updVinculacion" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnActa" />
                                    <asp:PostBackTrigger ControlID="btnVistaPrev" />
                                </Triggers>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Button Text="    Grabar" CssClass="btnSave" runat="server" ID="btnGrabarVinculacion"
                                                    OnClick="btnGrabarVinculacion_Click" />
                                                <uc3:ctrlReimprimirbtn ID="ctrlReimprimirbtn1" runat="server" />
                                                <uc5:ctrlBajaAutoadhesivo ID="ctrlBajaAutoadhesivo1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTipoActaTarifa" runat="server" Text="Tipo Acta:" Font-Bold="true"
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlActaTarifa" runat="server" Width="150px" Visible="false"
                                                    Style="cursor: pointer" AutoPostBack="True" />
                                                <asp:Label ID="lblCO_ddlActaTarifa" runat="server" Text="*" Style="color: #FF0000"
                                                    Visible="false"></asp:Label>
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
                                                    <asp:Label ID="Label10" runat="server" Text="PASO 1: Vinculación Autoadhesivo"></asp:Label></h3>
                                            </td>
                                            <td style="width: 300px" colspan="2">
                                                <h3>
                                                    <asp:Label ID="Label15" runat="server" Text="PASO 2: Vista Previa e Impresión"></asp:Label></h3>
                                            </td>
                                            <td style="width: 300px" colspan="2">
                                                <h3>
                                                    <asp:Label ID="Label13" runat="server" Text="PASO 3: Aprobación Impresión"></asp:Label></h3>
                                            </td>
                                        </tr>
                                        <tr style="padding: 0; margin: 0;">
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text="Código:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCodAutoadhesivo" runat="server" Width="120px" MaxLength="20"
                                                    CssClass="txtLetra"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                                <asp:Button ID="btnVistaPrev" runat="server" Text="   Autoadhesivo" Width="180px"
                                                    OnClientClick="return VistaPreviaAutoAdhesivos();" CssClass="btnPrint" TabIndex="50"
                                                    Enabled="False" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkImpresion" runat="server" Text="Impresión Correcta" OnCheckedChanged="chkImpresion_CheckedChanged"
                                                    AutoPostBack="false" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr style="padding: 0; margin: 0;">
                                            <td colspan="2">
                                                <div style="margin-left: 25%">
                                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btnLimpiar" OnClick="btnLimpiar_Click" />
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <%--<asp:Button ID="BtnActaConformidad" runat="server" Width="180px" CssClass="btnPrint" TabIndex="50"
                                                    Text="    Acta de Conformidad"   />--%>
                                                <asp:Button ID="btnActa" runat="server" Width="180px" CssClass="btnPrint" TabIndex="50"
                                                    OnClientClick="return ImprimirActa();" Text="Acta Civil" />
                                                <br />
                                                <%--<asp:Button ID="BtnActaConformidad" runat="server" Width="180px" CssClass="btnPrintVistaPrevia" TabIndex="50"
                                                    OnClientClick="return ImprimirActaConformidad();" Text="    Acta de Conformidad" 
                                                     />--%>
                                                <asp:Button ID="BtnActaConformidad" runat="server" Width="180px" CssClass="btnPrintVistaPrevia"
                                                    TabIndex="50" Text="    Acta de Conformidad" OnClick="BtnActaConformidad_Click" />
                                                <%-- <input type="button" id="BtnActaConformidad" class="btnPrintVistaPrevia" value="   Acta de Conformidad"
                                                    onclick="ImprimirActaConformidad();" />--%>
                                                <asp:Button ID="btnDesabilitarAutoahesivo" runat="server" OnClick="btnDesabilitarAutoahesivo_Click"
                                                    Text="Oculto" Visible="true" CssClass="hideControl" />
                                                <br />
                                                <asp:Button ID="btnSolicitudInscr" runat="server" Width="180px" CssClass="btnPrint"
                                                    TabIndex="50" Text="    Solicitud de Inscripción" Visible="False" OnClick="btnSolicitudInscr_Click"
                                                    Enabled="False" />
                                                <asp:HiddenField ID="hdn_ImpresionCorrecta" runat="server" Value="0" />
                                                <asp:HiddenField ID="hCodAutoadhesivo" runat="server" />
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
                                                <h3 style="font-size: small">
                                                    Procesando la información. Por favor espere...</h3>
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
        <div id="modalBusquedaTitular" class="modal">
            <div class="modal-window" style="width: 650px;">
                <div class="modal-titulo">
                    <asp:ImageButton ID="ImageButton1" CssClass="close" ImageUrl="~/Images/close.png"
                        OnClientClick="cerrarPopupBusquedaActas(); return false" runat="server" />
                    <span>BUSCAR</span>
                </div>
                <div class="modal-cuerpo">
                    <asp:UpdatePanel ID="updBusquedaTitular" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <br />
                            <table align="center">
                                <tr>
                                    <td align="right" style="padding-right: 5px">
                                        <asp:Label ID="Label22" runat="server" Text="Apellido Paterno:"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBuscarApPaterno" runat="server" CssClass="txtLetra" Width="300px"
                                            onkeypress="return isNombreApellido(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 5px">
                                        <asp:Label ID="Label23" runat="server" Text="Apellido Materno:"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBuscarApMaterno" runat="server" CssClass="txtLetra" Width="300px"
                                            onkeypress="return isNombreApellido(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 5px">
                                        <asp:Label ID="Label28" runat="server" Text="Nombres:"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="txtLetra" Width="300px"
                                            onkeypress="return isNombreApellido(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnFiltrarTitulares" runat="server" Text="      Buscar" Width="100px"
                                            CssClass="btnBusqueda" OnClick="btnFiltrarTitulares_Click" />
                                        &nbsp;&nbsp;&nbsp;
                                        <input id="btnLimpiar" type="button" value="Limpiar" class="btnLogin" onclick="limpiarCamposBusquedaActas()"
                                            style="width: 100px" />
                                    </td>
                                </tr>
                            </table>
                            <table align="center" style="padding-left: 5px; padding-right: 5px;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="Grd_BusquedaActas" runat="server" AlternatingRowStyle-CssClass="alt"
                                            AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="reci_iRegistroCivilId"
                                            GridLines="None" Width="100%" OnRowCommand="Grd_BusquedaActas_RowCommand" OnRowCreated="Grd_BusquedaActas_RowCreated">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="reci_iRegistroCivilId" HeaderStyle-CssClass="ColumnaOculta"
                                                    HeaderText="iRegistroCivilId" ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="reci_sTipoActaId" HeaderStyle-CssClass="ColumnaOculta"
                                                    HeaderText="sTipoActaId" ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="reci_vLibro" HeaderText="Libro">
                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="reci_vNumeroActa" HeaderText="Nro.Acta">
                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="peid_vDocumentoNumero" HeaderText="Nro.Documento">
                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TITULAR" HeaderText="Titular" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO_ACTA" HeaderText="Tipo de Acta" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnSeleccionar" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            CommandName="Select" ImageUrl="~/Images/img_sel_check.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc2:ctrlPageBar ID="CtrlPageBarActas" runat="server" OnClick="CtrlPageBarActas_Click"
                                            Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div id="modalParticipantes" class="modal">
            <div class="modal-windowlarge">
                <div class="modal-titulo">
                    <asp:ImageButton ID="imgCerrarPopupAntecedentesPenales" CssClass="close" ImageUrl="~/Images/close.png"
                        OnClientClick="cerrarPopupParticipantes();return false" runat="server" />
                    <span>Registro de Participantes</span>
                </div>
                <div class="modal-cuerpoNormal">
                    <asp:Panel ID="pnlParticipantes" runat="server">
                        <asp:UpdatePanel ID="updParticipa" runat="server">
                            <ContentTemplate>
                                <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDatosParticipantes" runat="server" Text="DATOS DE PARTICIPANTE(S)"
                                                Style="font-weight: 700; color: #800000;" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; border-bottom-color: #800000;">
                                    <tr>
                                        <td style="width: 90%" colspan="4">
                                            <asp:Label ID="lblValidacionParticipante" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="El Nro. Documento no puede tener menos o más de 8 caracteres."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px">
                                            <asp:Label ID="lblPartTipoPart" runat="server" Text="Tipo Participante:" />
                                        </td>
                                        <td style="width: 300px">
                                            <asp:DropDownList ID="ddl_TipoParticipante" runat="server" Width="250px" Height="21px"
                                                Style="cursor: pointer" OnSelectedIndexChanged="ddl_TipoParticipante_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label60" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="width: 120px">
                                            <asp:Label ID="lblPartTipoVinc" runat="server" Text="Tipo Dato:" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_TipoDatoParticipante" runat="server" Width="265px" Style="margin-bottom: 0px;
                                                cursor: pointer" Height="21px" Enabled="False" AutoPostBack="True" OnSelectedIndexChanged="ddl_TipoDatoParticipante_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label36" runat="server" Text="Tipo Vínculo:" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_TipoVinculoParticipante" runat="server" Width="250px" Height="21px"
                                                Style="cursor: pointer" onchange="TipoVinculoParticipanteChange();">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label14" runat="server" ForeColor="#FF0000" Text="*" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPersonaId" runat="server" Text="0" CssClass="hideControl"></asp:TextBox>
                                            <asp:TextBox ID="txtParticipanteID" runat="server" Text="0" CssClass="hideControl"></asp:TextBox>
                                            <asp:TextBox ID="txtPersonaId2" runat="server" Text="0" CssClass="hideControl"></asp:TextBox>
                                            <asp:Label ID="lbltienendocumento" runat="server" Text="¿Tiene Documentos?:" Visible="False" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbSi" runat="server" Text="Si" GroupName="ExisteDocumento" CssClass="rbExisteDocumento"
                                                OnCheckedChanged="rbSi_CheckedChanged" Checked="True" Visible="False" AutoPostBack="true" />
                                            <asp:RadioButton ID="rbNo" runat="server" Text="No" GroupName="ExisteDocumento" CssClass="rbExisteDocumento"
                                                Visible="False" OnCheckedChanged="rbNo_CheckedChanged" AutoPostBack="true" />
                                            <asp:HiddenField ID="HF_TIENE_DOCUMENTO" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPartTipoDoc" runat="server" Text="Tipo Documento:" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_TipoDocParticipante" runat="server" Width="250px" Height="21px"
                                                Style="cursor: pointer" onchange="TipoDocSelectedChange();">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label63" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPartNroDoc" runat="server" Text="Nro. Documento:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNroDocParticipante" runat="server" Width="220px" CssClass="campoNumero" AutoPostBack="True"
                                                MaxLength="12" onBlur="return ValidarNumeroDocumento(event)" OnTextChanged="txtNroDocParticipante_TextChanged" onkeypress="return ValidarNumeroDocumento(event)"/>
                                            <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                            Style="width: 16px" ToolTip="Buscar" OnClick="imgBuscar_Click" />
                                            <asp:Label ID="Label64" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label49" runat="server" Text="Nacionalidad" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_NacParticipante" runat="server" Width="250px" Height="21px"
                                                Style="cursor: pointer" Enabled="False">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label65" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label34" runat="server" Text="Nombres:" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNomParticipante" runat="server" Width="220px" CssClass="txtLetra"
                                                Enabled="False" />
                                            <asp:Label ID="Label66" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label32" runat="server" Text="Primer Apellido:" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtApePatParticipante" runat="server" Width="246px" CssClass="txtLetra" />
                                            <asp:Label ID="Label70" runat="server" ForeColor="#FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label45" runat="server" Text="Segundo Apellido:" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtApeMatParticipante" runat="server" Width="220px" CssClass="txtLetra" />
                                            <asp:Label ID="Label446" runat="server" ForeColor="#FF0000" Text="*" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGenero_Titular" runat="server" Text="Género:" Visible ="false" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGenero_Titular" runat="server" Width="250px" Visible ="false" 
                                                    Style="cursor: pointer" />
                                            <asp:Label ID="lblObligatorioGenero" runat="server" ForeColor="#FF0000" Text="*" Visible ="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                            <asp:Label ID="lblEstadoCivil" runat="server" Text="Estado Civil:" EnableTheming="False"
                                                Visible="False" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CmbEstCiv" runat="server" Width="250px" Height="21px" Enabled="False"
                                                Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldFecNacParticipante" runat="server" Text="Fecha de Nacimiento:"
                                                Visible="False" />
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="CtrldFecNacimientoParticipante" runat="server" Visible="false"
                                                Enabled="True" />
                                            <asp:Label ID="LblEdad" runat="server" Text="Edad :" CssClass="" Visible="false" />
                                            <asp:TextBox ID="LblEdad2" runat="server" Enabled="False" Visible="false" Width="50px"
                                                CssClass="campoNumero"></asp:TextBox>
                                            <asp:Button ID="BtnCalcularEded" CssClass="btnGeneral" runat="server" Visible="false"
                                                Width="65px" Text="Calcular" OnClick="BtnCalcularEded_Click" />
                                            <asp:Label ID="lblObligaFecNacimientoParticipante" runat="server" ForeColor="#FF0000" Text="*" Visible ="false"></asp:Label>
                                            <br>
                                            <span id="SP_Mensaje" style="color: red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblUbigeoParticipantes" runat="server" Text="LUGAR DE NACIMIENTO"
                                                Style="font-weight: 700; color: #800000;" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                    <tr>
                                        <td align="left" class="" style="width: 115px">
                                            <asp:Label ID="lblDireccionParticipante" runat="server" Text=" Dirección:" Visible="False" />
                                        </td>
                                        <td colspan="3" align="left">
                                            <asp:TextBox ID="txtDireccionParticipante" runat="server" Width="98%" CssClass="txtLetra"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <uc4:ctrlUbigeo ID="ctrlUbigeo1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="4">
                                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClientClick="abrirPopupEspera();" CssClass="btnGeneral" OnClick="btnAceptar_Click" />
                                            <asp:Button ID="btnCancelar" runat="server" Text="Limpiar" CssClass="btnGeneral"
                                                OnClick="btnCancelar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <span id="SpanMensaje" style="color: red;"></span>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
        <div>
            <asp:HiddenField ID="HF_PARTICIPANTE_CELEBRANTE_MATRIMONIO" runat="server" Value="0" />
            <asp:HiddenField ID="HF_PARTICIPANTE_RECURRENTE_NACIMIENTO" runat="server" Value="0" />
            <asp:HiddenField ID="HF_PARTICIPANTE_RECURRENTE_MATRIMONIO" runat="server" Value="0" />
            <asp:HiddenField ID="HF_PARTICIPANTE_RECURRENTE_DEFUNCION" runat="server" Value="0" />
            <asp:HiddenField ID="HF_PARTICIPANTE_PADRE_DEFUNCION" Value="0" runat="server" />
            <asp:HiddenField ID="HF_PARTICIPANTE_MADRE_DEFUNCION" Value="0" runat="server" />
            <asp:HiddenField ID="HF_PARTICIPANTE_DECLARANTE_DEFUNCION" Value="0" runat="server" />
            <asp:HiddenField ID="HF_PARTICIPANTE_REGISTRADORCIVIL_DEFUNCION" Value="0" runat="server" />
            <asp:HiddenField ID="HF_NACIONALIDAD_PERUANA" Value="0" runat="server" />
            <asp:HiddenField ID="HF_PAGADO_EN_LIMA" Value="3501" runat="server" />
            <asp:HiddenField ID="HF_TRANSFERENCIA" Value="3507" runat="server" />
            <asp:HiddenField ID="HF_DEPOSITO_CUENTA" Value="3508" runat="server" />
            <asp:HiddenField ID="hdn_CONFORMIDAD_DE_TEXTO" Value="0" runat="server" />
            <asp:HiddenField ID="HF_HABILITAR_CHECK_IMPRESION" Value="false" runat="server" />
            <asp:HiddenField ID="HF_TARIFA_1" Value="0" runat="server" />
            <asp:HiddenField ID="HF_TARIFA_2" Value="0" runat="server" />
            <asp:HiddenField ID="HF_TARIFA_3" Value="0" runat="server" />
            <asp:HiddenField ID="HF_TARIFA_4" Value="0" runat="server" />
            <asp:HiddenField ID="ESTADOVISTAPREVIA" Value="0" runat="server" />
            <asp:HiddenField ID="HFModoVistaAutoadhesivo" Value="0" runat="server" />
            <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" Value="" runat="server" />
            <asp:HiddenField ID="HF_Genero_Participante" Value="0" runat="server" />
            <asp:HiddenField ID="HF_TextoValidacion" Value="El Nro. Documento no puede tener menos o más de 8 caracteres."
                runat="server" />
            <asp:HiddenField ID="hValorChkSinApeMat" Value="0" runat="server" />
            <br />
            <br />
        </div>
    </div>
    <script language="javascript" type="text/javascript">



        function ImprimirActaConformidad() {

            var str_hifRegistroCivil = $("#<%=hifRegistroCivil.ClientID %>").val();
            var str_HF_ACTUACIONDET_ID = $("#<%=HF_ACTUACIONDET_ID.ClientID %>").val();
            var str_intTipoActa;
            str_intTipoActa = $("#<%=ddlTipoActa.ClientID %>").val();

            

            var str_hf_tarifa_1 = $("#<%= HF_TARIFA_1.ClientID %>").val();
            var str_hf_tarifa_2 = $("#<%= HF_TARIFA_2.ClientID %>").val();
            var str_hf_tarifa_3 = $("#<%= HF_TARIFA_3.ClientID %>").val();
            var str_hf_tarifa_4 = $("#<%= HF_TARIFA_4.ClientID %>").val();

            var str_txtIdTarifa = $("#<%= txtIdTarifa.ClientID %>").val();
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            var prm = {};

            if (str_txtIdTarifa == str_hf_tarifa_1) {

                if (str_hifRegistroCivil > 0) {
                    try {

                        if (str_HF_ACTUACIONDET_ID > 0) {

                            prm.intTipoActa = str_intTipoActa;
                            prm.strGUID = str_GUID;
                            $.ajax({
                                type: "POST",
                                url: "FrmActoCivil.aspx/ImprimirActaConformidad",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(prm),
                                dataType: "json",
                                success: function (data) {
                                    if (data.d == true) {
                                        //                                        var strUrl = "../Accesorios/VisorPDF.aspx";
                                        //                                        window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                                        var strUrl = "../Accesorios/VistaPreviaPDF.aspx";
                                        window.open('../Accesorios/VistaPreviaPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                                    } else {
                                        alert("EL FORMATO ESTA EN DESARROLLO");
                                    }

                                },
                                failure: function (response) {

                                    alert(response.d);
                                }
                            });

                        }
                        else {
                            showpopupother('e', 'VINCULACIÓN', "Debe Grabar el Registro Civil antes de Realizar la Vinculacion.", false, 190, 290);

                        }

                    } catch (err) {
                        showpopupother('e', 'VINCULACIÓN', 'ERROR. ' + err, false, 190, 290);

                    }
                }
                else {

                    showpopupother('e', 'VINCULACIÓN', "Debe Grabar el Registro Civil antes de realizar la vinculación.", false, 190, 290);

                }
            }
            else {
                ddlTipoActaAnotacion
                if (str_txtIdTarifa == str_hf_tarifa_2) {
                    str_intTipoActa = $("#<%=ddlTipoActaAnotacion.ClientID %>").val();
                }
                else {
                    str_intTipoActa = $("#<%=ddlActaTarifa.ClientID %>").val();
                }

                prm.intTipoActa = str_intTipoActa;

                alert(str_intTipoActa);

                prm.intTipoActa = str_intTipoActa;
                $.ajax({
                    type: "POST",
                    url: "FrmActoCivil.aspx/ImprimirActaConformidad",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(prm),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == true) {
                            //                            var strUrl = "../Accesorios/VisorPDF.aspx";
                            //                            window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                            var strUrl = "../Accesorios/VisPDF.aspx";
                            window.open('../Accesorios/VisPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                        } else {
                            alert("EL FORMATO ESTA EN DESARROLLO");
                        }

                    },
                    failure: function (response) {

                        alert(response.d);
                    }
                });




            }

            return false;
        }



        function VistaPreviaAutoAdhesivos() {
            funSetSession('ACTUALIZA', "");

            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            var str_hifRegistroCivil = $("#<%=hifRegistroCivil.ClientID %>").val();
            var str_HF_ACTUACIONDET_ID = $("#<%=HF_ACTUACIONDET_ID.ClientID %>").val();
            var str_intTipoActa = $("#<%=ddlTipoActa.ClientID %>").val();



            var str_hf_tarifa_1 = $("#<%= HF_TARIFA_1.ClientID %>").val();
            var str_hf_tarifa_2 = $("#<%= HF_TARIFA_2.ClientID %>").val();
            var str_hf_tarifa_3 = $("#<%= HF_TARIFA_3.ClientID %>").val();
            var str_hf_tarifa_4 = $("#<%= HF_TARIFA_4.ClientID %>").val();

            var str_txtIdTarifa = $("#<%= txtIdTarifa.ClientID %>").val();

            var strHFModoVistaAutoadhesivo = $("#<%= HFModoVistaAutoadhesivo.ClientID %>").val();

            if (strHFModoVistaAutoadhesivo == 1) {
                //                   window.open('../Registro/FrmRepAutoadhesivo.aspx', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=650,left=100,top=100');

                //s  abrirVentana();
                abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?GUID=' + str_GUID, 'AUTOADHESIVOS', 610, 450, '');
                $("#<%= chkImpresion.ClientID %>").attr('disabled', false);
                $("#<%= HF_HABILITAR_CHECK_IMPRESION.ClientID %>").val("true");
            } else {

                if (strHFModoVistaAutoadhesivo == 2) {

                    var prm = {};
                    prm.strGUID = str_GUID;
                    $.ajax({
                        type: "POST",
                        url: "FrmActoCivil.aspx/GetGenerarAutoAdhesivosPDF",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(prm),
                        dataType: "json",
                        success: function (data) {
                            if (data.d == true) {
                                var strUrl = "../Accesorios/VisorPDF.aspx";
                                //                                    window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');

                                abrirVentana('../Accesorios/VisorPDF.aspx', 'AUTOADHESIVOS', 650, 500, '');

                                $("#<%= chkImpresion.ClientID %>").attr('disabled', false);
                                $("#<%= HF_HABILITAR_CHECK_IMPRESION.ClientID %>").val("true");

                            } else {
                                showdialog('a', 'Registro Civil', 'Hemos tenido un Problema para generar el AutoAdhesivo.', 100, 160, false);
                            }

                        },
                        failure: function (response) {

                            showdialog('e', 'Registro Civil', response.d, 100, 160, false);
                        }
                    });

                }
                else {
                    showdialog('a', 'Registro Civil', 'Hemos tenido un Problema para generar el AutoAdhesivo.', 100, 160, false);
                }
            }

            return false;
        }


        function ImprimirActa() {
            var str_hifRegistroCivil = $("#<%=hifRegistroCivil.ClientID %>").val();
            var str_HF_ACTUACIONDET_ID = $("#<%=HF_ACTUACIONDET_ID.ClientID %>").val();
            var str_intTipoActa = $("#<%=ddlTipoActa.ClientID %>").val();

     

            if (str_hifRegistroCivil > 0) {
                try {

                    if (str_HF_ACTUACIONDET_ID > 0) {

                        var intTipoActa = $("#<%= ddlTipoActa.ClientID %>").val();

                        prm = {};

                        prm.HF_REFISTROCIVIL = str_hifRegistroCivil;
                        prm.HF_ACTUDETALLE = str_HF_ACTUACIONDET_ID;
                        prm.intTipoActa = intTipoActa;

                        $.ajax({
                            type: "POST",
                            url: "FrmActoCivil.aspx/Imprimir",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(prm),
                            dataType: "json",
                            success: function (data) {
                                if (data.d == true) {
                                    //                                    myWindow = window.open('../Accesorios/VisPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');

                                    myWindow = window.open('../Accesorios/VistaPreviaPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');

                                } else {
                                    alert("EL FORMATO ESTA EN DESARROLLO");
                                }

                            },
                            failure: function (response) {

                                alert(response.d);
                            }
                        });



                    }
                    else {
                        showpopupother('e', 'VINCULACIÓN', "Debe Grabar el Registro Civil antes de Realizar la Vinculacion.", false, 190, 290);

                    }

                } catch (err) {
                    showpopupother('e', 'VINCULACIÓN', 'ERROR. ' + err, false, 190, 290);

                }
            }
            else {

                showpopupother('e', 'VINCULACIÓN', "Debe Grabar el Registro Civil antes de realizar la vinculación.", false, 190, 290);

            }

            return false;

        }


        function TipoVinculoParticipanteChange() {

       

        }

        function TipoDocSelectedChange() {
            var txtNroDocumento = document.getElementById('<%= txtNroDocParticipante.ClientID %>');
            txtNroDocumento.value = "";
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


        function calcular_edad(fecha) {

            
        };


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

            var letras = "áéíóúñÑ";
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


        function ActoCivil_Participantes() {


        }


        function ValidarPersona() {
            var bolValida = true;

            var strTipoDocParticipante = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());

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

        function abrirPopupParticipantes() {
            document.getElementById('modalParticipantes').style.display = 'block';
        }
        function CerrarPopupParticipantes() {
            document.getElementById('modalParticipantes').style.display = 'none';
        }


        $(function () {
            $('#tabs').tabs();
            $('#tabs').enableTab(1);
            $('#tabs').disableTab(3);

        });

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }



        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }
        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
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
                cmb_TipoPago.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
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

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function ValidarGrabar() {
            var bolValor = true;
            var i_estadoFoco = 0;
            var strActoCivil = $.trim($("#<%= ddlTipoActa.ClientID %>").val());


            var strLibro = $.trim($("#<%= txtLibroRegCiv.ClientID %>").val());
            var txtLibro = document.getElementById('<%= txtLibroRegCiv.ClientID %>');
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

            /* ACTA */
            var strNroActa = $.trim($("#<%= txtNroActa.ClientID %>").val());


            var txtNroActa = document.getElementById('<%= txtNroActa.ClientID %>');
            if (strNroActa == "") {
                bolValor = false;
                txtNroActa.style.border = "1px solid Red";
                if (i_estadoFoco == 0) {
                    txtNroActa.focus();
                    i_estadoFoco = 1;
                }
            }
            else {
                txtNroActa.style.border = "1px solid #888888";
            }


            

            /*4501 : NACIMIENTO*/
            if (strActoCivil == str_Acta_Nacimiento) {
                var strNroCUI = $.trim($("#<%= txtNroCUI.ClientID %>").val());
                var txtNroCUI = document.getElementById('<%= txtNroCUI.ClientID %>');
                var strConCUI = $.trim($("#<%= chkconCUI.ClientID %>").val());
                if ($.trim(strNroCUI) != "") {
                    if ($.trim(strNroCUI).length > 0 && $.trim(strNroCUI).length < 8) {
                        bolValor = false;
                        txtNroCUI.style.border = "1px solid Red";
                        if (i_estadoFoco == 0) {
                            txtNroCUI.focus();
                            i_estadoFoco = 1;
                        }
                    }
                    else {
                        txtNroCUI.style.border = "1px solid #888888";
                    }
                }
                else {

                    if (strConCUI == "1") {
                        txtNroCUI.style.border = "1px solid Red";
                        bolValor = false;
                    }
                    else {
                        txtNroCUI.style.border = "1px solid #888888";
                    }
                }


            }

            /*4502 : MATRIMONIO*/
            if (strActoCivil == str_Acta_Matrimonio) {

                var strCargo = $.trim($("#<%= txtCargoCelebrante.ClientID %>").val());
                var txtCargo = document.getElementById('<%= txtCargoCelebrante.ClientID %>');

            }

            /*4501 : NACIMIENTO, 4503 : DEFUNCION*/
            if (strActoCivil == str_Acta_Nacimiento || strActoCivil == str_Acta_Defuncion) {

                if (txtcontrolError(document.getElementById('<%= txtFecNac.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;



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
                /*GÉNERO*/

                var strGenero = $.trim($("#<%= ddl_Genero.ClientID %>").val());
                var txtGenero = document.getElementById('<%= ddl_Genero.ClientID %>');
                if (strGenero == "0") {
                    bolValor = false;
                    txtGenero.style.border = "1px solid Red";
                    if (i_estadoFoco == 0) {
                        txtGenero.focus();
                        i_estadoFoco = 1;
                    }
                }
                else {
                    txtGenero.style.border = "1px solid #888888";
                }

                /*ddl_TipoOcurrencia*/
                var strLugarOcurrencia = $.trim($("#<%= ddl_TipoOcurrencia.ClientID %>").val());
                var ddlLugarOcurrencia = document.getElementById('<%= ddl_TipoOcurrencia.ClientID %>');
                if (strLugarOcurrencia == "0") {
                    bolValor = false;
                    ddlLugarOcurrencia.style.border = "1px solid Red";
                    if (i_estadoFoco == 0) {
                        ddlLugarOcurrencia.focus();
                        i_estadoFoco = 1;
                    }
                }
                else {
                    ddlLugarOcurrencia.style.border = "1px solid #888888";
                }

                /*txtLugarOcurrencia*/
                var strDireccion = $.trim($("#<%= txtLugarOcurrencia.ClientID %>").val());
                var txtDireccion = document.getElementById('<%= txtLugarOcurrencia.ClientID %>');
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
            }

            if (ddlcontrolError(document.getElementById('<%= ddl_DeptOcurrencia.ClientID %>')) == false) bolValor = false;
            if (ddlcontrolError(document.getElementById('<%= ddl_ProvOcurrencia.ClientID %>')) == false) bolValor = false;
            if (ddlcontrolError(document.getElementById('<%= ddl_DistOcurrencia.ClientID %>')) == false) bolValor = false;





            var str_ESTADOVISTAPREVIA = $.trim($("#<%= ESTADOVISTAPREVIA.ClientID %>").val());

            if (str_ESTADOVISTAPREVIA == 1) {
                if (bolValor) {
                    bolValor = confirm("¿Está seguro de grabar los cambios?");
                }
            }
            return bolValor;
        }

        function ValidarVinculacion() {
            var bolValida = true;
            var strImpresion = $.trim($("#<%= chkImpresion.ClientID %>").val());
            var strCodigo = $.trim($("#<%= txtCodAutoadhesivo.ClientID %>").val());

            var chkImpresion = document.getElementById('<%= chkImpresion.ClientID %>');
            var txtCodAutoadhesivo = document.getElementById('<%= txtCodAutoadhesivo.ClientID %>');

            if (strImpresion == "" || strImpresion == "0") {
                chkImpresion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                chkImpresion.style.border = "1px solid #888888";
            }

            if (strCodigo == "") {
                txtCodAutoadhesivo.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtCodAutoadhesivo.style.border = "1px solid #888888";
            }
            var str_txtIdTarifa = $("#<%= txtIdTarifa.ClientID %>").val();
            var str_hf_tarifa_2 = $("#<%= HF_TARIFA_2.ClientID %>").val();
            var str_hifRegistroCivil = $.trim($("#<%= hifRegistroCivil.ClientID %>").val());
            if (str_hifRegistroCivil < 1 && str_txtIdTarifa != str_hf_tarifa_2) {

                showpopupother('e', 'VINCULACIÓN', "Debe Grabar el Registro Civil antes de realizar la vinculación.", false, 190, 290);
                bolValida = false;
                return bolValida;
            }
            if (bolValida) {
                $("#<%= lblValidacionDetalle.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
                if (bolValida) {
                    abrirPopupEspera();
                }
            }

            else {
                $("#<%= lblValidacionDetalle.ClientID %>").show();
                bolValida = false;
            }
            return bolValida;
        }

        //-- VALIDACION GENERAL---------
        function txtcontrolError(ctrl) {
            var x = ctrl.value.trim();
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = ""; //1px solid #888888
            }
            return bolValida;
        }

        function txtcontrolError_disable(ctrl) {
            var x = ctrl.value.trim();
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "";
                bolValida = true;
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
                ctrl.style.border = ""; //1px solid #888888
            }
            return bolValida;
        }

        function ddlcontrolError_disable(ctrl) {
            var x = ctrl.selectedIndex;
            var bolValida = true;
            if (x < 1) {
                ctrl.style.border = "";
                bolValida = true;
            }
            return bolValida;
        }

        //-- FIN VALIDACION GENERAL---------
        //--VALIDACION HORAS--
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

            if (charCode == 8) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 37) {
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
    <script language="javascript" type="text/javascript">

        function GridViewOcultar() {

            var rows = $("#MainContent_Grd_Participantes tr:gt(0)");
            var rowToShow = rows.find("td:eq(0)").filter(":contains('E')").closest("tr");
            rows.show().not(rowToShow).hide();
        }

        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }

        function ValidarRegistroAnotacion() {
            var bolValida = true;

            tinyMCE.triggerSave();

            var strSeleccionado = $.trim($("#<%= cmb_TipoAnotacion.ClientID %>").val());
            var strDescripcion = $.trim($(".mceEditor").val());

            var ddlTipoAnotacion = document.getElementById('<%= cmb_TipoAnotacion.ClientID %>');
            var txtDescAnotacion = document.getElementById('<%= txtDescAnotacion.ClientID %>');
            var txtTitular = document.getElementById('<%= txtTitular.ClientID %>');
            var strTitular = $.trim($("#<%= txtTitular.ClientID %>").val());

            var ddlTipoActaAnotacion = document.getElementById('<%= ddlTipoActaAnotacion.ClientID %>');
            var strSelTipoActaAnotacion = $.trim($("#<%= ddlTipoActaAnotacion.ClientID %>").val());

            if (txtcontrolError(document.getElementById('<%= ctrFecRegistro.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;

            if (strSelTipoActaAnotacion == "0") {
                ddlTipoActaAnotacion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlTipoActaAnotacion.style.border = "1px solid #888888";
            }

            if (strTitular.length == 0) {
                txtTitular.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtTitular.style.border = "1px solid #888888";
            }

            if (strSeleccionado == "0") {
                ddlTipoAnotacion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlTipoAnotacion.style.border = "1px solid #888888";
            }

            if (strDescripcion.length == 0) {
                txtDescAnotacion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtDescAnotacion.style.border = "1px solid #888888";
            }

            if (bolValida) {

                $("#<%= lblValidacionAnotacion.ClientID %>").hide();
                var isConsultad = $("#<%= HFEsConsulta.ClientID %>").val();

                if (isConsultad == 0) {
                    bolValida = confirm("¿Está seguro de grabar los cambios?");
                }
            }
            else {
                $("#<%= lblValidacionAnotacion.ClientID %>").show();
                bolValida = false;
            }
            return bolValida;
        }


        function Documento_OnClick() {

            var ddl_TipoDocParticipante = document.getElementById('<%= ddl_TipoDocParticipante.ClientID %>');
            var ddl_TipoParticipante = document.getElementById('<%= ddl_TipoParticipante.ClientID %>');
            
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();

            var bolValida = true;

            var prm = {};
            prm.tipo = $("#<%= ddl_TipoDocParticipante.ClientID %>").val();
            prm.documento = $("#<%= txtNroDocParticipante.ClientID %>").val();
            prm.TipoParticipante = $("#<%= ddl_TipoParticipante.ClientID %>").val();
            prm.strGUID = str_GUID;

            var strActoCivil = $.trim($("#<%= ddlTipoActa.ClientID %>").val());
            
            
            
            var str_TipoParticipante = $("#<%= ddl_TipoParticipante.ClientID %>").val();


            if (strActoCivil == str_Acta_Matrimonio) {

            }


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
                        url: "FrmActoCivil.aspx/GetPersonExist",
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
        }


        ////////////////////
        // Calcular la edad
        ////////////////////
        function CalculoFecha() {

        }

        function OnSuccess(response) {
            

        }



        function IsValidDatosParticipantes() {

            var bolValida = true;

            var strTipoDoc = $.trim($("#<%= ddl_TipoDocParticipante.ClientID %>").val());
            var strNacParticipante = $.trim($("#<%= ddl_NacParticipante.ClientID %>").val());

            var strNroDoc = $.trim($("#<%= txtNroDocParticipante.ClientID %>").val());



            var strApePat = $.trim($("#<%= txtApePatParticipante.ClientID %>").val());
            var strApeMat = $.trim($("#<%= txtApeMatParticipante.ClientID %>").val());
            var strNombres = $.trim($("#<%= txtNomParticipante.ClientID %>").val());




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


        function OnlyNumeros(e) {
            var key = window.Event ? e.which : e.keyCode
            return (key >= 48 && key <= 57)
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

        function Oculdad_ddlnacionalidad() {
            $("#<%= ddl_NacParticipante.ClientID %>").hide();
        }
        function mostrar_ddlnacionalidad() {
            $("#<%= ddl_NacParticipante.ClientID %>").show();
        }
        function validafechasDefuncion(dat_fechaNacPar, dat_fechaDefPar) {
            var f1 = dat_fechaNacPar.split("/");
            var f2 = dat_fechaDefPar.split("/");
            //var fecha = get_Date(crl.value);
            if (f2 == ",__,") {
                $('#SP_Mensaje').html('Debe ingresar la fecha de defunción');

                return false;
            }
            var cont = 0;
            if (f1[2] >= f2[2]) {
                if (f1[2] == f2[2]) {
                    if (f1[1] >= f2[1]) {
                        if (f1[1] == f2[1]) {
                            if (f1[0] > f2[0]) {
                                cont = 1;
                            }
                        } else {
                            cont = 1;
                        }
                    }
                } else {
                    cont = 1;
                }


            }
            if (cont == 1) {
                $('#SP_Mensaje').html('La fecha de nacimiento no puede ser </br> mayor a la fecha de fallecimiento');

                //str_fechaNacPar.style.border = "1px solid Red";

                return false;
            } else {
                $('#SP_Mensaje').html('');
                return true;
                //str_fechaNacPar.style.border = "";
            }
        }
        function postBack() {
            funSetSession('ACTUALIZA', 'ACTUALIZA');
        }
    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</asp:Content>
