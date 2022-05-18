<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="FrmRegistroPersona.aspx.cs"
    Inherits="SGAC.WebApp.Registro.FrmRegistroPersona" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/Validacion/Text.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tMsjeWarnig
        {
            background-color: #F2F1C2;
            border-color: Yellow;
            color: #4B4F5E;
            height: 15px;
            background-image: url('../../Images/img_16_warning.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeWarnig
        {
            margin-left: 25px;
        }
        
        .tMsjeError
        {
            background-color: #FE2E2E;
            border-color: Red;
            color: #FFFFFF;
            height: 15px;
            background-image: url('../../Images/img_16_error.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeError
        {
            margin-left: 25px;
        }
        
        .tMsjeSucess
        {
            background-color: #2E9AFE;
            border-color: Blue;
            color: #FFFFFF;
            height: 15px;
            background-image: url('../../Images/img_16_success.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        
        .lblMsjeSucess
        {
            margin-left: 25px;
        }
        
        .AlineadoDerecha
        {
            text-align: right;
        }
        
        .Oculto
        {
            display: none;
        }
        
        .style1
        {
            width: 301px;
        }
        .style2
        {
            width: 939px;
        }
        .style4
        {
            width: 280px;
        }
        .mTblInterna > tbody > tr > td
        {
            padding:5px
        }
            
        
        .style6
        {
            width: 111px;
        }
                    
.btnBuscar
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	background:-moz-linear-gradient( center top, #ededed 5%, #560e27 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#560e27', endColorstr='#dfdfdf');
	background-color:#560e27;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px solid #dcdcdc;
	border-style:outset;
	height:24px;
	width:80px;	
	background-image: url('../Images/img_16_buscar.png');
	background-repeat: no-repeat;
	background-position:  8px 3px;
	border-style:outset;	
	cursor:pointer;
	color:White;
    margin-left:-5px;	 
}
.btnBuscar:hover {
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #560e27) );
	background:-moz-linear-gradient( center top, #ededed 5%, #560e27 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#560e27;', endColorstr='#ededed');
	background-image: url('../Images/img_16_buscar.png');
	background-repeat: no-repeat;
	background-position: 8px 3px;
	background-color:#8D1640;
	cursor:pointer;
}
.btnBuscar[disabled] {
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
	background:-moz-linear-gradient( center top, #ededed 5%, #560e27 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#560e27', endColorstr='#dfdfdf');
	background-color:#560e27;
	background-image: url('../Images/img_16_buscar_disabled.png');
	background-repeat: no-repeat;
	background-position: 8px 3px;
}        
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    &nbsp;<!-- Script by hscripts.com --><script type="text/javascript">
                                             var r = { 'special': /[\W]/g }
                                             function valid(o, w) {
                                                 o.value = o.value.replace(r[w], '');
                                             }
    </script><!--Script by hscripts.com--><script type="text/javascript">

                                              Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
                                              $(document).ready(function () {
                                                  Load();
                                                  Layout_Filiancion();
                                              });

                                              function Load() {

                                                  var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());
                                                  if (strNacRecurr == '2051') { //Peruana
                                                      $("#<%= chk_PJ.ClientID %>").attr('enabled', 'enabled');
                                                  }
                                                  else if (strNacRecurr == '2052') { //Extranjera
                                                      $("#<%= chk_PJ.ClientID %>").attr('disabled', 'disabled');
                                                      $("#<%= chk_PJ.ClientID %>").prop("checked", "");
                                                  }


                                                  $(function () {
                                                      $(':text').bind('keydown', function (e) {
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

                                                  //--Fecha:03/12/2020; Autor:VPIPA
                                                  //se deshabilita la edicion de las fechas por las constantes errores por parte de usuarios, solo permite selecciona
                                                  $("#MainContent_ctrFecNac_TxtFecha").attr('readOnly', "readOnly");
                                                  $("#MainContent_ctrFecExped_TxtFecha").attr('readOnly', "readOnly");
                                                  $("#MainContent_ctrFecRenov_TxtFecha").attr('readOnly', "readOnly");
                                                  $("#MainContent_ctrFecVcto_TxtFecha").attr('readOnly', "readOnly");
                                                  
                                                  
                                              }



                                              function NoExisteParticipante(tipoFilacion) {
                                                  var bol = true;

                                                  $('#<%=GrdFiliacion.ClientID %> tbody tr').each(function () {

                                                      if (!bol)
                                                          return;

                                                      $(this).children("td").each(function (index) {
                                                          var filiados = ObtenerPermiteFiliados($(this).text()).split(",");

                                                          if (index == $("#<%=HF_TipoFiliacionColumnaIndice.ClientID %>").val()) {
                                                              if (tipoFilacion == filiados[0]) {

                                                                  if (filiados[1] == "1") {
                                                                      bol = false;
                                                                      return;
                                                                  }
                                                              }
                                                          }
                                                      });


                                                  });

                                                  return bol;
                                              }

                                              function ObtenerPermiteFiliados(tipoFilacion) {
                                                  var bol = true;


                                                  var hfDocumentoIdentidad = $("#<%=HF_ValoresFiliacion.ClientID %>").val()

                                                  var documentos = hfDocumentoIdentidad.split("|");

                                                  for (i = 0; i < documentos.length - 1; i++) {

                                                      var valores = documentos[i].split(",");

                                                      if (valores[0] == tipoFilacion) {
                                                          return documentos[i];
                                                      }

                                                  }

                                                  return "";

                                              }

                                              function disable() {

                                                  document.getElementById('<%=txtApepCas.ClientID%>').disabled = 'true';
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

                                              function validFiliacion() {
                                                  var bolValida = true;
                                                  if (ddlcontrolError(document.getElementById("<%=ddl_TipFiliacion.ClientID %>")) == false) bolValida = false;
                                                  if (ddlcontrolError(document.getElementById("<%=ddl_peid_sDocumentoTipoId.ClientID %>")) == false) bolValida = false;
                                                  if (txtcontrolError(document.getElementById("<%=txt_peid_vDocumentoNumero.ClientID %>")) == false) bolValida = false;

                                                  if (txtcontrolError(document.getElementById("<%=txt_pers_vApellidoPaterno.ClientID %>")) == false) bolValida = false;
                                                  if (txtcontrolError(document.getElementById("<%=txt_pers_vNombres.ClientID %>")) == false) bolValida = false;

                                                  var str_peid_sDocumentoTipoId = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
                                                  var str_peid_sDocumentoTipoDesc = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %> option:selected").text();


                                                  var str_peid_vDocumentoNumero = $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val();

                                                  document.getElementById('<%=lblMsjFiliacion.ClientID %>').innerHTML = '';




                                                  var valoresDocumento = ObtenerMaxLenghtDocumentos(str_peid_sDocumentoTipoId).split(",");

                                                  if (valoresDocumento[2] == "True" && isNaN(str_peid_vDocumentoNumero)) {
                                                      $("#<%= lblMsjFiliacion.ClientID %>").html("El tipo de documento '" + str_peid_sDocumentoTipoDesc + "' solo permite números.");
                                                      $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid Red 1px");
                                                      bolValida = false;
                                                  }

                                                  else if (valoresDocumento[0] > str_peid_vDocumentoNumero.length || valoresDocumento[1] < str_peid_vDocumentoNumero.length) {

                                                      $("#<%= lblMsjFiliacion.ClientID %>").html(valoresDocumento[3]);
                                                      $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid Red 1px");
                                                      bolValida = false;
                                                  }

                                                  else if (soloCeros(str_peid_vDocumentoNumero)) {
                                                      $("#<%= lblMsjFiliacion.ClientID %>").html("El número de documento es incorrecto");
                                                      $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid Red 1px");
                                                      bolValida = false;

                                                  }
                                                  else {
                                                      $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid #888888 1px");
                                                      $("#<%= lblMsjFiliacion.ClientID %>").html("");
                                                  }




                                                  if (bolValida == true) {



                                                      var tipoFilacion = $("#<%=ddl_TipFiliacion.ClientID %>").val();
                                                      bolValida = NoExisteParticipante(tipoFilacion);

                                                      if (bolValida == false) {
                                                          showdialog('a', 'AFILACION', 'SOLO SE PERMITE UNA FILIACIÓN DE ESTE TIPO', false, 190, 250);
                                                      }

                                                  }

                                                  return bolValida;
                                              }
                                              function limpiarFiliacion(ctrl) {

                                                  //Limpiando Pantalla
                                                  $("#<%=ddl_TipFiliacion.ClientID %>").val("0");
                                                  $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val("0");
                                                  $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val("");
                                                  $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val("0");
                                                  $("#<%=txt_pers_vNombres.ClientID %>").val("");
                                                  $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
                                                  $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");

                                                  $("#<%=txt_pefi_vLugarNacimiento.ClientID %>").val("");

                                                  $("#pers_iPersonaId").val("0");
                                                  $("#pefi_iPersonaFilacionId").val("0");

                                                  $("#<%=ddl_TipFiliacion.ClientID %>").focus();
                                              }

                                              function Layout_Filiancion() {

                                                  $("#btn_addFiliacion").click(function () {

                                                      if (validFiliacion() == true) {
                                                          var filiacion = {};
                                                          filiacion.pefi_iPersonaFilacionId = $("#pefi_iPersonaFilacionId").val(); // En caso de edición aca debe cambiar
                                                          filiacion.pefi_sTipoFilacionId = $("#<%=ddl_TipFiliacion.ClientID %>").val();

                                                          var persona = {};
                                                          persona.pers_iPersonaId = $("#pers_iPersonaId").val(); // En caso que exista 
                                                          persona.pers_sNacionalidadId = $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val();
                                                          persona.pers_vNombres = $("#<%=txt_pers_vNombres.ClientID %>").val();
                                                          persona.pers_vApellidoPaterno = $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val();
                                                          persona.pers_vApellidoMaterno = $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val();

                                                          var identificacion = {};
                                                          identificacion.peid_sDocumentoTipoId = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
                                                          identificacion.peid_vDocumentoNumero = $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val();

                                                          var otros = {};
                                                          otros.pefi_sTipoFilacionId_desc = $("#<%=ddl_TipFiliacion.ClientID %>").find('option:selected').text();
                                                          otros.pefi_vLugarNacimiento_desc = $("#<%=txt_pefi_vLugarNacimiento.ClientID %>").val();

                                                          if (persona.pers_sNacionalidadId != 0) {
                                                              otros.pefi_sNacionalidad_desc = $("#<%=ddl_pers_sNacionalidadId.ClientID %>").find('option:selected').text();
                                                          } else {
                                                              otros.pefi_sNacionalidad_desc = '';
                                                          }
                                                          var prm = {};
                                                          prm.filiacion = JSON.stringify(filiacion);
                                                          prm.persona = JSON.stringify(persona);
                                                          prm.identificacion = JSON.stringify(identificacion);
                                                          prm.otros = JSON.stringify(otros);

                                                          execute('FrmRegistroPersona.aspx/set_GrdFiliacion', prm);
                                                          document.getElementById("<%=btn_GridFiliacionRefresh.ClientID %>").click();

                                                          //Limpiando Pantalla
                                                          $("#<%=ddl_TipFiliacion.ClientID %>").val("0");
                                                          $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val("0");
                                                          $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val("");
                                                          $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val("0");
                                                          $("#<%=txt_pers_vNombres.ClientID %>").val("");
                                                          $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
                                                          $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");

                                                          $("#<%=txt_pefi_vLugarNacimiento.ClientID %>").val("");

                                                          $("#pers_iPersonaId").val("0");
                                                          $("#pefi_iPersonaFilacionId").val("0");

                                                          $("#<%=ddl_TipFiliacion.ClientID %>").focus();
                                                      }
                                                  });

                                                  //Bloqueo de controles
                                                  //filiacion_disabled(true);
                                              }

                                              function addFiliacion_valid() {

                                                  return true;
                                              }

                                              function filiacion_disabled(estado) {
                                                  $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr("disabled", estado);
                                                  $("#<%=txt_pers_vNombres.ClientID %>").attr("disabled", estado);
                                                  $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr("disabled", estado);
                                                  $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr("disabled", estado);

                                              }

                                              //                                              function filiacion_persona(msj) {
                                              //                                                  var lcancel = false;
                                              //                                                  var peid_sDocumentoTipoId = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>");
                                              //                                                  var peid_vDocumentoNumero = $("#<%=txt_peid_vDocumentoNumero.ClientID %>");

                                              //                                                  if (peid_sDocumentoTipoId.val() == 0) {
                                              //                                                      if (msj == 1) {
                                              //                                                          alert("Seleccione el tipo documento.");
                                              //                                                      }
                                              //                                                      peid_sDocumentoTipoId.focus();
                                              //                                                      lcancel = true;
                                              //                                                  }
                                              //                                                  if ((lcancel == false) && (peid_vDocumentoNumero.val() == "")) {
                                              //                                                      if (msj == 1) {
                                              //                                                          alert("Seleccione el número documento.");
                                              //                                                      }
                                              //                                                      peid_vDocumentoNumero.focus();
                                              //                                                      lcancel = true;
                                              //                                                  }

                                              //                                                  if (lcancel == false) {
                                              //                                                      var prm = {};
                                              //                                                      var url = 'FrmRegistroPersona.aspx/obtener_persona';

                                              //                                                      prm.tipodocumento = peid_sDocumentoTipoId.val();
                                              //                                                      prm.documento = peid_vDocumentoNumero.val();
                                              //                                                      var rspta = execute(url, prm);
                                              //                                                      var persona = $.parseJSON(rspta.d);

                                              //                                                      if (persona.pers_iPersonaId != 0) {
                                              //                                                          $("#pers_iPersonaId").val(persona.pers_iPersonaId);
                                              //                                                          $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val(persona.pers_sNacionalidadId);
                                              //                                                          $("#<%=txt_pers_vNombres.ClientID %>").val(persona.pers_vNombres);
                                              //                                                          $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val(persona.pers_vApellidoPaterno);
                                              //                                                          $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val(persona.pers_vApellidoMaterno);

                                              //                                                          filiacion_disabled(true);

                                              //                                                          if (persona.pers_cNacimientoLugar != null) {
                                              //                                                          } else {
                                              //                                                          }

                                              //                                                      }
                                              //                                                      else {
                                              //                                                          filiacion_disabled(false);
                                              //                                                          $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val("0");
                                              //                                                          $("#<%=txt_pers_vNombres.ClientID %>").val("");
                                              //                                                          $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
                                              //                                                          $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");
                                              //                                                          $("#<%=txt_pefi_vLugarNacimiento.ClientID %>").val("");
                                              //                                                          $("#pers_iPersonaId").val("0");
                                              //                                                          $("#pefi_iPersonaFilacionId").val("0");

                                              //                                                          $("#<%=ddl_pers_sNacionalidadId.ClientID %>").focus();
                                              //                                                      }
                                              //                                                  }
                                              //                                              }

                                              function ConvertDate(jsonDate) {
                                                  var jsMeses = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
                                                  var jsDate = new Date(parseInt(jsonDate.substr(6)));

                                                  return jsMeses[jsDate.getMonth()] + "-" + ("0" + jsDate.getDate()).slice(-2) + "-" + jsDate.getFullYear();
                                              }

                                              function ddl_peid_sDocumentoTipoId_changed() {
                                                  var sDocumentoTipoId = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>");
                                                  var peid_vDocumentoNumero = $("#<%=txt_peid_vDocumentoNumero.ClientID %>");

                                                  var DocumentoNumero_MaxLength = 15;

                                                  switch (sDocumentoTipoId.val()) {
                                                      case "0": DocumentoNumero_MaxLength = 0; break;
                                                      case "1": DocumentoNumero_MaxLength = 8; break;
                                                      case "2": DocumentoNumero_MaxLength = 15; break;
                                                      case "3": DocumentoNumero_MaxLength = 12; break;
                                                      case "4": DocumentoNumero_MaxLength = 12; break;
                                                      case "5": DocumentoNumero_MaxLength = 15; break;
                                                  }

                                                  $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr("maxlength", DocumentoNumero_MaxLength);
                                                  peid_vDocumentoNumero.focus();
                                              }

                                              function filiacion_GrdFiliacion() {
                                                  var url = 'FrmRegistroPersona.aspx/get_Session'
                                                  var prm = {};
                                                  prm.nombre = 'EditFiliacion';

                                                  var rspta = execute(url, prm);
                                                  var filiacion = $.parseJSON(rspta.d);

                                                  $("#pefi_iPersonaFilacionId").val(filiacion.pefi_iPersonaFilacionId);
                                                  $("#<%=ddl_TipFiliacion.ClientID %>").val(filiacion.pefi_sTipoFilacionId);
                                                  $("#pers_iPersonaId").val(filiacion.pers_iPersonaId);
                                                  $("#<%=ddl_pers_sNacionalidadId.ClientID %>").val(filiacion.pers_sNacionalidadId);
                                                  $("#<%=txt_pers_vNombres.ClientID %>").val(filiacion.pers_vNombres);
                                                  $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val(filiacion.pers_vApellidoPaterno);
                                                  $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val(filiacion.pers_vApellidoMaterno);

                                                  $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val(filiacion.peid_sDocumentoTipoId);
                                                  $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val(filiacion.peid_vDocumentoNumero);

                                              }

                                              function ValidarNumeroDocumento(event) {
                                                  var CmbTipoDoc = $("#<%=CmbTipoDoc.ClientID %>").val();
                                                  var bol = false;

                                                  var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

                                                  document.getElementById("<%=txtNroDoc.ClientID %>").maxLength = valoresDocumento[1];

                                                  var documento = document.getElementById("<%=txtNroDoc.ClientID %>").value;
                                                  var documentosinespacios = documento.replace(/ /g, "");
                                                  document.getElementById("<%=txtNroDoc.ClientID %>").value = documentosinespacios;
                                                  
                                                  if (valoresDocumento[2] == "True") {
                                                      bol = OnlyNumeros(event);                                                      
                                                  }
                                                  else {
                                                      bol = isNumeroLetra(event);                                                      
                                                  }
                                                 
                                                  return bol;
                                              }

                                              function ValidarNumeroDocumentoDoc(event) {

                                                  var CmbTipoDoc = $("#<%=ddl_TipoDocumentoM.ClientID %>").val();
                                                  var bol = false;

                                                  var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

                                                  document.getElementById("<%=txtNroDocumentoM.ClientID %>").maxLength = valoresDocumento[1];

                                                  if (valoresDocumento[2] == "True") {
                                                      bol = OnlyNumeros(event);
                                                  }
                                                  else {
                                                      bol = isNumeroLetra(event);
                                                  }

                                                  return bol;
                                              }

                                              function ValidarNroDoc(event) {

                                                  var CmbTipoDoc = $("#<%=CmbTipoDoc.ClientID %>").val();
                                                  var bol = false;

                                                  var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

                                                  document.getElementById("<%=txtNroDoc.ClientID %>").maxLength = valoresDocumento[1];

                                                  if (valoresDocumento[2] == "True") {
                                                      bol = OnlyNumeros(event);
                                                  }
                                                  else {
                                                      bol = isNumeroLetra(event);
                                                  }

                                                  return bol;
                                              }

                                              function ValidarNumeroDocumentoFil(event) {

                                                  var CmbTipoDoc = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
                                                  var bol = false;

                                                  var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

                                                  document.getElementById("<%=txt_peid_vDocumentoNumero.ClientID %>").maxLength = valoresDocumento[1];

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

    </script>
    <script type="text/javascript">

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function validarApellidoCasada()
        {

            var genero = $('#<%= CmbGenero.ClientID %>').find('option:selected').text();
            var estadoCivil = $('#<%= CmbEstCiv.ClientID %>').find('option:selected').text();
 
            if(genero == "FEMENINO" && (estadoCivil == "CASADO" || estadoCivil == "VIUDO"))
            {
                $('#<%= txtApepCas.ClientID %>').removeAttr("disabled"); 
                document.getElementById('<%=txtApepCas.ClientID %>').value = document.getElementById('<%=HF_Persona_ApellidoCasada_Editando.ClientID %>').value;
            }
            else{
                $('#<%= txtApepCas.ClientID %>').attr("disabled",true); 
                document.getElementById('<%=txtApepCas.ClientID %>').value = "";
            }
        }
        
        function RecargarUbigeoNacimiento()
        {
            var ubigeo = document.getElementById('<%=hubigeoNacimiento.ClientID %>').value;
            
            if (ubigeo.length == 6) {
                var provincia = ubigeo.substring(2, 4);
                var distrito = ubigeo.substring(4, 6);;
                cargarProvinciaNacimiento();
                $('#<%= CmbProvPaisNac.ClientID %>').val(provincia).change();
                cargarDistritoNacimiento();
                $('#<%= CmbDistCiudadNac.ClientID %>').val(distrito).change();
            }
        }
        function RecargarUbigeoResidencia() {
            var ubigeo = document.getElementById('<%=hubigeoResidencia.ClientID %>').value;
            
            if (ubigeo.length == 6) {
                var provincia = ubigeo.substring(2, 4);
                var distrito = ubigeo.substring(4, 6);;
                cargarProvinciaResidencia();
                $('#<%= ddlProvPaisResidencia.ClientID %>').val(provincia).change();
                cargarDistritoResidencia();
                $('#<%= ddlDistCiudadResidencia.ClientID %>').val(distrito).change();
            }
        }

        function cargarProvinciaNacimiento() {
            $('#<%= hubigeoNacimiento.ClientID %>').val("");
            $('#<%= CmbProvPaisNac.ClientID %>').empty();
            $('#<%= CmbDistCiudadNac.ClientID %>').empty();
            $('#<%= CmbDistCiudadNac.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');
            var objetoProvincia = localStorage.getItem("objProvincia");

            var ddlDepartamento = document.getElementById('<%=CmbDptoContNac.ClientID %>');

            var listaProvincia = JSON.parse(objetoProvincia);

            $('#<%= CmbProvPaisNac.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaProvincia).each(function () {
                if (this.Ubi01 == ddlDepartamento.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Provincia);
                    option.val(this.Ubi02);
                    $('#<%= CmbProvPaisNac.ClientID %>').append(option);
                    $('#<%= CmbProvPaisNac.ClientID %>').removeAttr("disabled");
                }
            });
        }
        function cargarProvinciaResidencia() {
            $('#<%= hubigeoResidencia.ClientID %>').val("");
            $('#<%= ddlProvPaisResidencia.ClientID %>').empty();
            $('#<%= ddlDistCiudadResidencia.ClientID %>').empty();
            $('#<%= ddlDistCiudadResidencia.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');
            
            var objetoProvincia = localStorage.getItem("objProvincia");

            var ddlDepartamento = document.getElementById('<%=ddlDptContinenteResidencia.ClientID %>');

            var listaProvincia = JSON.parse(objetoProvincia);

            $('#<%= ddlProvPaisResidencia.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaProvincia).each(function () {
                if (this.Ubi01 == ddlDepartamento.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Provincia);
                    option.val(this.Ubi02);
                    $('#<%= ddlProvPaisResidencia.ClientID %>').append(option);
                    $('#<%= ddlProvPaisResidencia.ClientID %>').removeAttr("disabled");
                }
            });
        }

        function cargarDistritoNacimiento() {
            $('#<%= hubigeoNacimiento.ClientID %>').val("");
            $('#<%= CmbDistCiudadNac.ClientID %>').empty()
            var objetoDistrito = localStorage.getItem("objDistrito");

            var ddlDepartamento = document.getElementById('<%=CmbDptoContNac.ClientID %>');
            var ddlProvincia = document.getElementById('<%=CmbProvPaisNac.ClientID %>');

            var listaDistrito = JSON.parse(objetoDistrito);

            $('#<%= CmbDistCiudadNac.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaDistrito).each(function () {
                if (this.Ubi01 == ddlDepartamento.value && this.Ubi02 == ddlProvincia.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Distrito);
                    option.val(this.Ubi03);
                    $('#<%= CmbDistCiudadNac.ClientID %>').append(option);
                    $('#<%= CmbDistCiudadNac.ClientID %>').removeAttr("disabled");
                }
            });
        }
        function cargarDistritoResidencia() {
            $('#<%= hubigeoResidencia.ClientID %>').val("");
            $('#<%= ddlDistCiudadResidencia.ClientID %>').empty()
            var objetoDistrito = localStorage.getItem("objDistrito");

            var ddlDepartamento = document.getElementById('<%=ddlDptContinenteResidencia.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddlProvPaisResidencia.ClientID %>');

            var listaDistrito = JSON.parse(objetoDistrito);

            $('#<%= ddlDistCiudadResidencia.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaDistrito).each(function () {
                if (this.Ubi01 == ddlDepartamento.value && this.Ubi02 == ddlProvincia.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Distrito);
                    option.val(this.Ubi03);
                    $('#<%= ddlDistCiudadResidencia.ClientID %>').append(option);
                    $('#<%= ddlDistCiudadResidencia.ClientID %>').removeAttr("disabled");
                }
            });
        }
        function cargarUbigeoNacimiento() {
            var ddlDepartamento = document.getElementById('<%=CmbDptoContNac.ClientID %>');
            var ddlProvincia = document.getElementById('<%=CmbProvPaisNac.ClientID %>');
            var ddlDistrito = document.getElementById('<%=CmbDistCiudadNac.ClientID %>');

            var ubigeo = ddlDepartamento.value + ddlProvincia.value + ddlDistrito.value;
            $('#<%= hubigeoNacimiento.ClientID %>').val(ubigeo);
        }
        function cargarUbigeoResidencia() {
            var ddlDepartamento = document.getElementById('<%=ddlDptContinenteResidencia.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddlProvPaisResidencia.ClientID %>');
            var ddlDistrito = document.getElementById('<%=ddlDistCiudadResidencia.ClientID %>'); 

            var ubigeo = ddlDepartamento.value + ddlProvincia.value + ddlDistrito.value;
            $('#<%= hubigeoResidencia.ClientID %>').val(ubigeo);
        }
        function Guardarlocalstorage(objProvincia, objDistrito)
        {
            localStorage.setItem("objProvincia", JSON.stringify(objProvincia));
            localStorage.setItem("objDistrito", JSON.stringify(objDistrito));
            console.log(objProvincia);
            console.log(objDistrito);
        }
        function ObtenerElementosGenero() {
            var comboGenero = document.getElementById('<%= CmbGenero.ClientID %>');
            var comboEstadoCivil = document.getElementById('<%= CmbEstCiv.ClientID %>');
            var comboNacionalidad = document.getElementById('<%= ddl_pers_sNacionalidadId.ClientID %>');
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

            for (i = 1; i < comboNacionalidad.length; i++) {
                var largo = comboNacionalidad.options[i].text.length;
                comboNacionalidad.options[i].text = comboNacionalidad.options[i].text.substring(0, largo - 1) + letra;
            }
        }

        function CmbGenero_SelectedIndexChanged() {
            var CmbGenero = $("#<%=CmbGenero.ClientID %>");
            var CmbEstCiv = $("#<%=CmbEstCiv.ClientID %>");

            $("#<%=CmbGenero.ClientID %>").focus();
            if ((CmbGenero.val() == "2002") && ((CmbEstCiv.val() == "2") || (CmbEstCiv.val() == "4"))) {
                $("#<%=txtApepCas.ClientID %>").attr('diabled', false);
            }
            else {
                $("#<%=txtApepCas.ClientID %>").val('');
                $("#<%=txtApepCas.ClientID %>").attr('diabled', true);
            }
        }

        function CmbOcupacion_SelectedIndexChanged() {
            var CmbOcupacion = $("#<%=CmbOcupacion.ClientID %>");
            CmbOcupacion.focus();
        }

        function CmbGradInst_SelectedIndexChanged() {
            $("#<%=CmbGradInst.ClientID %>").focus();
        }

        function OnlyNumeros(e) {
            var key = window.Event ? e.which : e.keyCode
            return (key >= 48 && key <= 57)
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
    <script language="javascript" type="text/javascript">
        function ValidarCaracteres(textareaControl, maxlength) {

            if (textareaControl.value.length > maxlength) {
                textareaControl.value = textareaControl.value.substring(0, maxlength);
                alert("Debe ingresar hasta un maximo de " + maxlength + " caracteres.");
            }
        }
    </script>
    <asp:HiddenField ID="HFGUID" runat="server" />
    <asp:HiddenField ID="HD_FecNac" runat="server" />
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitRegistroUnico" runat="server" Text="Registro Único" /></h2>
                </td>
                <td align="right">
                    <asp:LinkButton ID="lkbTramite" Visible="false" runat="server" 
                        Font-Bold="True" Font-Size="10pt" ForeColor="Blue" Font-Underline="true" 
                        onclick="lkbTramite_Click">Regresar a Trámites</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <table class="mTblPrincipal" align="center">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdPopupBusqueda" UpdateMode="Conditional" runat="server" >
                        <ContentTemplate>
                            <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarButton" runat="server"></ToolBarButton:ToolBarButtonContent>
                            <div style="margin-left:8px;">
                                <asp:Button ID="btnImprimirConstancia" runat="server" 
                                    Text="        Constancia de Inscripción" Width="200px"
                                CssClass="btnPrint" Visible="false" OnClientClick="abrirModalIdioma();return false"/>
                            </div>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdMsje">
                    <asp:Label ID="lblValidacion" runat="server" Text="Debe ingresar los campos requeridos (*)"
                        CssClass="hideControl" ForeColor="Red" Font-Size="14px"> </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdDatosPersona" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table class="mTblSecundaria">
                                <tr>
                                    <td align="center" colspan="3" class="Oculto">
                                        <table>
                                            <tr>
                                                <td align="center" class="style1">
                                                    <asp:RadioButton ID="RdoReniec" runat="server" GroupName="RdoDatosOrigen" Text="RENIEC"
                                                        AutoPostBack="True" OnCheckedChanged="RdoReniec_CheckedChanged" />
                                                </td>
                                                <td style="width: 300px;" align="center">
                                                    <asp:RadioButton ID="RdoDNI" runat="server" GroupName="RdoDatosOrigen" Text="DNI"
                                                        AutoPostBack="True" OnCheckedChanged="RdoDNI_CheckedChanged" />
                                                </td>
                                                <td style="width: 300px;" align="center">
                                                    <asp:RadioButton ID="RdoManual" runat="server" GroupName="RdoDatosOrigen" Text="Manual"
                                                        Checked="True" AutoPostBack="True" OnCheckedChanged="RdoManual_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;" align="center">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgPersona" runat="server" Height="120px" Width="120px" ImageUrl='~/Images/People.png' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="lnk_CapturarImagen" runat="server" Style="float: none;" OnClick="lnk_CapturarImagen_Click"
                                                        Visible="False">Tomar Foto</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="imgFirma" runat="server" Height="50px" Width="120px" ImageUrl="~/Images/Firma.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="HFValidarTextoRune" runat="server" />
                                                    <asp:HiddenField ID="HFValidarTexto" runat="server" />
                                                    <asp:HiddenField ID="HFValidarNumeroDecimal" runat="server" />
                                                    <asp:HiddenField ID="HFValidarNumero" runat="server" />
                                                    <asp:HiddenField ID="HF_Pasaporte_ID" runat="server" Value="1" />
                                                    <asp:HiddenField ID="HF_ValidaDocumento" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblNacionalidad" runat="server" Text="Tipo de Recurrente :" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="HF_ModoEdicion" Value="" runat="server" />
                                                    <asp:DropDownList ID="CmbNacRecurr" runat="server" Width="150px" OnSelectedIndexChanged="CmbNacRecurr_SelectedIndexChanged"
                                                        TabIndex="1" AutoPostBack="True" />
                                                    <asp:Label ID="lblCO_CmbNacRecurr" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblTipoPers" runat="server" Text="Tipo Persona :" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="CmbTipPers" runat="server" Width="150px" TabIndex="2" Enabled="False" />
                                                    <asp:Label ID="lblCO_CmbTipPers" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblTipDoc" runat="server" Text="Tipo de Documento :" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="CmbTipoDoc" runat="server" Width="150px" TabIndex="3" OnSelectedIndexChanged="CmbTipoDoc_SelectedIndexChanged" />
                                                    <asp:Label ID="lblCO_CmbTipoDoc" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                    <asp:CheckBox ID="chkValidarConReniec" runat="server" Text="Validar con RENIEC" Checked="true"  OnCheckedChanged="chkValidarConReniec_change" AutoPostBack="True"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblOtroDocumento" runat="server" Text="Otro documento :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescOtroDocumento" runat="server" CssClass="txtLetra" onkeypress="return isLetra(event)"
                                                        Width="283px"></asp:TextBox>
                                                     <asp:Label ID="lblCO_OtroDoc" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblNroDoc" runat="server" Text="Nro Documento :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValidacionReniec" runat="server"   Text="0" Visible="false" />
                                                    <asp:TextBox ID="txtNroDoc" runat="server" Width="146px" TabIndex="4" MaxLength="20"
                                                        onBlur="return valNumero()" CssClass="txtLetra" onkeypress="return ValidarNroDoc(event)"  />                                                                                                        
                                                    <asp:Button ID="btnValidarDni" runat="server" Text="    Validar" Visible="true" CssClass="btnBuscar" OnClick="btnConsultarPideReniecDNI_Click" 
                                                    ValidationGroup="validGroup1" OnClientClick="disableBtn(this.id, 'Validando...')"    UseSubmitBehavior="false"/>
                                                    <asp:Label ID="lblCO_txtNroDoc" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trRazSocial" runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="LblRazSoc" runat="server" Text="Razón Social :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRazSoc" runat="server" Width="300px" TabIndex="5" CssClass="txtLetra"
                                                        onkeypress="return isLetra(event)" />
                                                    <asp:Label ID="lblCO_txtRazSoc" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblApePat" runat="server" Text="Primer Apellido :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApePat" runat="server" Width="283px" TabIndex="6" CssClass="txtLetra"
                                                        MaxLength="50" onkeypress="return ValidarCamposRUNE(event)" onblur="validarCampos(this)" />
                                                    <asp:Label ID="lblCO_txtApePat" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblApeMat" runat="server" Text="Segundo Apellido :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApeMat" runat="server" Width="283px" TabIndex="7" CssClass="txtLetra"
                                                        MaxLength="50" onkeypress="return ValidarCamposRUNE(event)" onblur="validarCampos(this)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblNombres" runat="server" Text="Nombres :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNombres" runat="server" Width="283px" TabIndex="8" CssClass="txtLetra"
                                                        MaxLength="50" onkeypress="return ValidarCamposRUNE(event)" onblur="validarCampos(this)" />
                                                    <asp:Label ID="lblCO_txtNombres" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblApepCas" runat="server" Text="Apellido de Casada :" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApepCas" runat="server" Width="283px" TabIndex="13" CssClass="txtLetra"
                                                        MaxLength="50" Enabled="False" onkeypress="return ValidarCamposRUNE(event)" onblur="validarCampos(this)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblObservRENIEC" runat="server" Text="Observaciones Reniec :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="txtObsRENIEC" runat="server" Width="280px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-top: 10px;">
                                                    <asp:Label ID="Label8" runat="server" Text="Nacionalidad Vigente:" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td style="padding-top: 10px;">
                                                    <asp:Label ID="lblNacionalidadVigenteCabecera" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdoGrabCrearNewAct" runat="server" Text="Grabar y crear nueva actuación"
                                                GroupName="RuneAct" Checked="True" TabIndex="34" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdoSoloGrab" runat="server" Text="Solo grabar" GroupName="RuneAct"
                                                TabIndex="35" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chk_PJ" runat="server" Text="Pertenece a la Jurisdicción (Crear Actuación 58A)" />
                                        </td>
                                    </tr>
                                    <tr style="height: 23px">
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="¿Persona aún Vive?"></asp:Label>
                                            &nbsp; &nbsp;
                                            <asp:RadioButton ID="RdoViveSi" GroupName="vive" runat="server" Text="Si" Checked="True" />
                                            &nbsp; &nbsp; &nbsp;
                                            <asp:RadioButton ID="RdoViveNo" GroupName="vive" runat="server" Text="No" />
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td>
                                            <asp:CheckBox ID="chkPeruanoMadrePadrePeruno" Text="De Padre/Madre Peruano nacido en el exterior"
                                                runat="server" Visible="False" />
                                            <label id="lblPrimerApeMensaje" style="color: Red; font-size: small; display: none">
                                                Solo puede ingresar letras
                                            </label>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td>
                                            <label id="lblSegundoApeMensaje" style="color: Red; font-size: small; display: none">
                                                Solo puede ingresar letras
                                            </label>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td>
                                            <label id="lblNombreMensaje" style="color: Red; font-size: small; display: none;">
                                                Solo se pueden ingresar letras.</label>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td>
                                            <label id="lblApeCasMensaje" style="color: Red; font-size: small; display: none">
                                                Solo puede ingresar letras
                                            </label>
                                    </tr>
                                </table>
                            </td>
                            </tr> </table>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabDatAdic" runat="server" Text="Datos Principales" TabIndex="10"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabDocumentos" runat="server" Text="Documentos" TabIndex="21"></asp:Label></a></li>
                            <li><a href="#tab-8">
                                <asp:Label ID="lblNacionalidades" runat="server" Text="Nacionalidades" TabIndex="68"></asp:Label></a></li>
                            <li><a href="#tab-3">
                                <asp:Label ID="lblTabDirecciones" runat="server" Text="Direcciones" TabIndex="30"></asp:Label></a></li>
                            <li><a href="#tab-4">
                                <asp:Label ID="lblTabFiliacion" runat="server" Text="Filiación" TabIndex="39"></asp:Label></a></li>
                            <li><a href="#tab-5">
                                <asp:Label ID="lblTabContacto" runat="server" Text="Datos del Contacto" TabIndex="46"></asp:Label></a></li>
                            <li><a href="#tab-6">
                                <asp:Label ID="lblTabDatosMigratorios" runat="server" Text="Datos Migratorios" TabIndex="54"></asp:Label></a></li>
                            <li><a href="#tab-7">
                                <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones" TabIndex="67"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updTabDatAdic" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="CmbEstCiv" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbGenero" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbOcupacion" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddl_Profesion" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbGradInst" EventName="SelectedIndexChanged" />
                                     <%----------Fecha:03/11/2020  Autor: Pipa
                                    se elimina los trigger de los controles (combos de nacimiento) por lo al actualizar los datos del cualquier pestaña se blankeaba
                                    <asp:AsyncPostBackTrigger ControlID="CmbDptoContNac" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbProvPaisNac" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbDistCiudadNac" EventName="SelectedIndexChanged" />
                                    --%>
                                </Triggers>
                                <ContentTemplate>
                                    <asp:HiddenField ID="HF_Persona_TipoDocumento_Editando" Value="-1" runat="server" />
                                    <asp:HiddenField ID="HF_Persona_NumeroDocumento_Editando" Value="" runat="server" />
                                    <asp:HiddenField ID="HF_Persona_Nombre_Editando" Value="" runat="server" />
                                    <asp:HiddenField ID="HF_Persona_ApellidoMaterno_Editando" Value="" runat="server" />
                                    <asp:HiddenField ID="HF_Persona_ApellidoPaterno_Editando" Value="" runat="server" />
                                    <asp:HiddenField ID="HF_Persona_ApellidoCasada_Editando" Value="" runat="server" />
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="LblEstCiv" runat="server" Text="Estado Civil :" />
                                                            </td>
                                                            <td width="200px">
                                                                <%--<asp:DropDownList ID="CmbEstCiv" runat="server" Width="170px" TabIndex="11" OnSelectedIndexChanged="CmbEstCiv_SelectedIndexChanged"
                                                                    AutoPostBack="True" />--%>
                                                                <asp:DropDownList ID="CmbEstCiv" runat="server" Width="170px" TabIndex="11" onChange="validarApellidoCasada();"
                                                                    AutoPostBack="false" />
                                                                <asp:Label ID="lblCO_CmbEstCiv" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="LblGenero" runat="server" Text="Género :" />
                                                            </td>
                                                            <td width="170px">
                                                                <%--<asp:DropDownList ID="CmbGenero" runat="server" Width="150px" TabIndex="12" onChange="javascript:CmbGenero_SelectedIndexChanged();"
                                                                    OnSelectedIndexChanged="CmbGenero_SelectedIndexChanged" AutoPostBack="True" />--%>
                                                                <asp:DropDownList ID="CmbGenero" runat="server" Width="150px" TabIndex="12" onChange="validarApellidoCasada();"
                                                                     AutoPostBack="false" />
                                                                <asp:Label ID="lblCO_CmbGenero" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="160px">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="LblFecNac" runat="server" Text="Fecha de Nac. :" />
                                                            </td>
                                                            <td width="200px">
                                                                <SGAC_Fecha:ctrlDate ID="ctrFecNac" runat="server"  Ejecutar_Scrip="true"/>
                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="LblEdad1" runat="server" Text="Edad :" Visible="false" />
                                                                <asp:Label ID="LblEdad" runat="server" Text="Edad :" CssClass="Oculto" />
                                                            </td>
                                                            <td width="170px">
                                                                <asp:TextBox ID="LblEdad2" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                                                <asp:TextBox ID="txtEdad" runat="server" Width="58px" Enabled="False" CssClass="Oculto" />
                                                            </td>
                                                            <td width="160px">
                                                                <asp:Label ID="lblFecFallece" runat="server" Text="Fecha de Fallecimiento:" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFechaFallecimiento" runat="server" Text="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="LblOcupacion" runat="server" Text="Ocupación :" />
                                                            </td>
                                                            <td width="200px">
                                                                <asp:DropDownList ID="CmbOcupacion" runat="server" Width="170px" TabIndex="14" ClientIDMode="Static"
                                                                    onChange="javascript:CmbOcupacion_SelectedIndexChanged();" />
                                                                <asp:Label ID="lblCO_CmbOcupacion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="lblProfesiones" runat="server" Text="Profesión :" />
                                                            </td>
                                                            <td width="170px">
                                                                <asp:DropDownList ID="ddl_Profesion" runat="server" Width="150px" TabIndex="15" ClientIDMode="Static"
                                                                    OnSelectedIndexChanged="ddl_Profesion_SelectedIndexChanged" />
                                                            </td>
                                                            <td width="160px">
                                                                <asp:Label ID="LblGrdInstr" runat="server" Text="Grado de Instrucción :" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:DropDownList ID="CmbGradInst" runat="server" Width="150px" TabIndex="16" ClientIDMode="Static"
                                                                    onChange="javascript:CmbGradInst_SelectedIndexChanged();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="300px">
                                                                <asp:Label ID="LblDptoContNac" runat="server" Text="Continente/Dpto.de Nacimiento:" />
                                                            </td>
                                                            <td width="230px">
                                                                <%--<asp:DropDownList ID="CmbDptoContNac" runat="server" OnSelectedIndexChanged="CmbDptoContNac_SelectedIndexChanged"
                                                                    TabIndex="17" Width="200px" AutoPostBack="True" />--%>
                                                                <asp:DropDownList ID="CmbDptoContNac" runat="server" onchange="cargarProvinciaNacimiento();"
                                                                    TabIndex="17" Width="200px" AutoPostBack="false" />
                                                                <asp:Label ID="lblCO_CmbDptoContNac" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                            <td align="right" class="style4">
                                                                <asp:Label ID="LblProvPaisNac" runat="server" Text="País/Provincia de Nacimiento:" />
                                                                &nbsp; &nbsp;
                                                            </td>
                                                            <td align="right">
                                                                <%--<asp:DropDownList ID="CmbProvPaisNac" runat="server" Width="150px" Enabled="False"
                                                                    OnSelectedIndexChanged="CmbProvPaisNac_SelectedIndexChanged" TabIndex="18" AutoPostBack="True" />--%>
                                                                <asp:DropDownList ID="CmbProvPaisNac" runat="server" Width="150px" Enabled="False"
                                                                    onchange="cargarDistritoNacimiento();" TabIndex="18" AutoPostBack="false" />
                                                                <asp:Label ID="lblCO_CmbProvPaisNac" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="300px">
                                                                <asp:Label ID="LblDistCiudadNac" runat="server" Text="Distrito/Ciudad de Nacimiento:" />
                                                            </td>
                                                            <td width="230px">
                                                                <asp:DropDownList ID="CmbDistCiudadNac" runat="server" Width="200px" Enabled="False"
                                                                    onchange="cargarUbigeoNacimiento();" TabIndex="19"/>
                                                                    <asp:Label ID="lblCO_CmbDistCiudadNac" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                                                <asp:HiddenField ID="hubigeoNacimiento" runat="server" />
                                                            </td>
                                                            <td align="right" class="style4">
                                                                <asp:Label ID="LblGentilicio" runat="server" Text="Gentilicio :"  style="display:none"/>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="LblDescGentilicio" runat="server" style="display:none"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="130px">
                                                                <asp:Label ID="LblCorrElec" runat="server" Text="Correo Electrónico :" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtEmail" runat="server" TabIndex="20" Width="100%" CssClass="txtLetra"
                                                                    MaxLength="55" onblur="if(this.value!=''){ validarEmail(this); }" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="direccionResidencia" runat="server" visible="true" style="padding: 0 10px 0 10px;">
                                                    <fieldset>
                                                        <legend>Datos de Residencia</legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="130px">
                                                                    <asp:Label ID="lblDireccionResidencia" runat="server" Text="Dirección de Residencia :" />
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtDireccionResidencia" runat="server" TabIndex="20" Width="100%"
                                                                        CssClass="txtLetra" />
                                                                </td>
                                                                <td>
                                                                    <div style="margin-left: 5px;">
                                                                        <asp:Label ID="lblCO_txtDireccionResidencia" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="120px">
                                                                    <asp:Label ID="Label14" runat="server" Text="Continente/Dpto.:" />
                                                                </td>
                                                                <td>
                                                                    <%--<asp:DropDownList ID="ddlDptContinenteResidencia" runat="server" Width="200px" OnSelectedIndexChanged="ddlDptContinenteResidencia_SelectedIndexChanged"
                                                                        AutoPostBack="True" />--%>
                                                                    <asp:DropDownList ID="ddlDptContinenteResidencia" runat="server" Width="200px" onchange="cargarProvinciaResidencia();"
                                                                        AutoPostBack="false" />
                                                                    <asp:Label ID="lblCO_DptContinenteResidencia" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                </td>
                                                                <td align="left" width="120px">
                                                                    <asp:Label ID="Label16" runat="server" Text="País/Provincia:" />
                                                                </td>
                                                                <td align="left" width="230px">
                                                                   <%-- <asp:DropDownList ID="ddlProvPaisResidencia" runat="server" Width="200px" OnSelectedIndexChanged="ddlProvPaisResidencia_SelectedIndexChanged"
                                                                        AutoPostBack="True" />--%>
                                                                    <asp:DropDownList ID="ddlProvPaisResidencia" runat="server" Width="200px" onchange="cargarDistritoResidencia();"
                                                                        AutoPostBack="false" />
                                                                    <asp:Label ID="lblCO_ProvPaisResidencia" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="120px">
                                                                    <asp:Label ID="Label18" runat="server" Text="Distrito/Ciudad:" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlDistCiudadResidencia" runat="server" Width="200px" onchange="cargarUbigeoResidencia();"/>
                                                                    <asp:Label ID="lblCO_DistCiudadResidencia" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                                    <asp:HiddenField ID="hubigeoResidencia" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="120px">
                                                                    <asp:Label ID="Label13" runat="server" Text="Teléfono:" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTelefonoDir" runat="server" TabIndex="20"
                                                                        CssClass="txtLetra" />
                                                                </td>
                                                                <td align="left" width="120px">
                                                                    <asp:Label ID="Label20" runat="server" Text="Código Postal:" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCodPostalDir" runat="server" TabIndex="20"
                                                                        CssClass="txtLetra" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updDocumentos" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="140px">
                                                <asp:Label ID="lblTipoDocumentoM" runat="server" Text="Tipo de Documento:"></asp:Label>
                                            </td>
                                            <td width="160px">
                                                <asp:DropDownList ID="ddl_TipoDocumentoM" runat="server" Width="150px" TabIndex="22" AutoPostBack="True" OnSelectedIndexChanged="TipoDocumentoM_SelectedIndexChanged" />
                                            </td>
                                            <td width="140px">
                                                <asp:Label ID="lblNroDocumentoM" runat="server" Text="Nro de Documento:"></asp:Label>
                                            </td>
                                            <td width="150px">
                                                <asp:TextBox ID="txtNroDocumentoM" CssClass="txtLetra" runat="server" TabIndex="23"
                                                    onkeypress="return ValidarNumeroDocumentoDoc(event)"  Width="120px" />
                                                <asp:Label ID="Label1" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                <asp:Button ID="btnValidarDNI2" runat="server" Text="Validar con RENIEC" Visible="true" OnClick="btnConsultarPideReniecDNI2_Click" 
                                                    ValidationGroup="validGroup1" OnClientClick="disableBtn(this.id, 'Validando...')"    UseSubmitBehavior="false"/>
                                            </td>
                                            <td width="140px">
                                                <asp:Label ID="lblFecVcto" runat="server" Text="Fecha Vencimiento:"></asp:Label>
                                            </td>
                                            <td align="left" width="140px">
                                                <SGAC_Fecha:ctrlDate ID="ctrFecVcto" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="140px">
                                                <asp:Label ID="lblFecExpedicion" runat="server" Text="Fecha Expedición:"></asp:Label>
                                            </td>
                                            <td width="160px">
                                                <SGAC_Fecha:ctrlDate ID="ctrFecExped" runat="server" />
                                            </td>
                                            <td width="140px">
                                                <asp:Label ID="lblLugExp" runat="server" Text="Lugar de Expedición:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtLugExp" runat="server" Width="387px" CssClass="txtLetra" 
                                                    TabIndex="26" onkeypress="return isDireccion(event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="140px">
                                                <asp:Label ID="lblFecRenov" runat="server" Text="Fecha Renovación:"></asp:Label>
                                            </td>
                                            <td width="160px">
                                                <SGAC_Fecha:ctrlDate ID="ctrFecRenov" runat="server" />
                                            </td>
                                            <td width="140px">
                                                <asp:Label ID="lblLugRenov" runat="server" Text="Lugar de Renovación:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtLugRenov" runat="server" Width="387px" CssClass="txtLetra" 
                                                    TabIndex="28" onkeypress="return isDireccion(event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:Label ID="lblActRune" runat="server" Text="Activo en RUNE"></asp:Label>
                                        <asp:CheckBox ID="chk_ActRUNE" runat="server" TabIndex="27" />
                                    </div>
                                    <br />
                                    <div>
                                        <asp:HiddenField ID="hidDocumento" runat="server" EnableViewState="true" Value="" />
                                        <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" Value="" runat="server" />
                                        <asp:HiddenField ID="HF_ValoresFiliacion" Value="" runat="server" />
                                        <asp:HiddenField ID="HF_TipoFiliacionColumnaIndice" Value="0" runat="server" />
                                        <asp:HiddenField ID="HF_TextoValidacion" Value="El Nro. Documento no puede tener menos o más de 8 caracteres."
                                            runat="server" />
                                        <asp:HiddenField ID="HF_TipoDocumento_Editando" Value="-1" runat="server" />
                                        <asp:HiddenField ID="HF_NumeroDocumento_Editando" Value="" runat="server" />
                                        <table>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp; &nbsp; &nbsp;
                                                    <asp:Button ID="btn_GrabarDocumento" runat="server" Width="170px" Text="       Grabar Documento"
                                                        CssClass="btnSaveAnotacion" OnClick="btn_GrabarDocumento_Click" TabIndex="29" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    <asp:Button ID="btn_CancelarMD" runat="server" CssClass="btnLimpiar" OnClick="btn_CancelarMD_Click"
                                                        TabIndex="29" Text="       Limpiar" Width="170px" />
                                                    &nbsp; &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblValidacionDoc" runat="server" Text="Debe ingresar los campos requeridos"
                                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <asp:GridView ID="Grd_Documentos" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="Grd_Documentos_RowCommand"
                                            OnRowCreated="Grd_Documentos_RowCreated" OnRowDataBound="Grd_Documentos_RowDataBound">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="iPersonaIdentificacionId" HeaderText="PersonaIdentificacionId"
                                                    HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sDocumentoTipoId" HeaderText="TipoDocumentoId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vDesTipoDoc" HeaderText="Tipo Documento" />
                                                <asp:BoundField DataField="vDocumentoNumero" HeaderText="Número" />
                                                <asp:BoundField DataField="dFecVcto" HeaderText="Fec. Vcto" />
                                                <asp:BoundField DataField="dFecExpedicion" HeaderText="Fec.Exped." />
                                                <asp:BoundField DataField="vLugarExpedicion" HeaderText="Lug.Exped." />
                                                <asp:BoundField DataField="dFecRenovacion" HeaderText="Fec.Renov." />
                                                <asp:BoundField DataField="vLugarRenovacion" HeaderText="Lug.Renov." />
                                                <asp:BoundField DataField="bActivoEnRune" HeaderText="ActivoEnRune" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Activo" HeaderText="Doc.Activo" />

                                                <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_16_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Indicador" HeaderText="Indicador" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="#FFFF99" Font-Bold="true" ForeColor="Black" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-8">
                            <asp:UpdatePanel ID="updNacionalidad" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <div>
                                                    <table width="100%">
                                                        <caption>
                                                            <br />
                                                            <tr>
                                                                <td align="left" colspan="2" width="180px">
                                                                    <asp:Label ID="Label6" runat="server" Text="Pais :" />
                                                                    <asp:DropDownList ID="ddlPais_Nacion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlContinente_Nacion_SelectedIndexChanged"
                                                                        TabIndex="17" Width="200px" />
                                                                </td>
                                                                <td align="left" colspan="2" width="150px">
                                                                    <asp:Label ID="lblNacionalidadVigente" runat="server" Text="[Sin Nacionalidad]" />
                                                                </td>
                                                                <td align="right" width="200px">
                                                                    <asp:Label ID="Label7" runat="server" Text="Nacionalidad Vigente?:" />
                                                                    <asp:CheckBox ID="chkNacVigente" runat="server" />
                                                                </td>
                                                                <td align="right" width="100px">
                                                                </td>
                                                            </tr>
                                                        </caption>
                                                    </table>
                                                    <br />
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnREGISTRAR_NACIONALIDAD" runat="server" CssClass="btnSaveAnotacion"
                                                                    Text="Grabar" OnClick="btnREGISTRAR_NACIONALIDAD_Click" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="Button3" runat="server" CssClass="btnLimpiar" TabIndex="29" Text="       Limpiar"
                                                                    Width="170px" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="margin-left: 20%;">
                                        <asp:GridView ID="gdvNacionalidad" runat="server" AutoGenerateColumns="False" Width="500px"
                                            CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" ShowHeaderWhenEmpty="True"
                                            OnRowCommand="gdvNacionalidad_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="pena_iPersonaId" HeaderText="pena_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="pena_sPais" HeaderText="pena_sPais" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PAIS_VNOMBRE" HeaderText="País" HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="pena_vNacionalidad" HeaderText="Nacionalidad" HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="pena_bvigente" HeaderText="VIGENCIA" HeaderStyle-Width="150px" />
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/img_grid_delete.png" CommandName="Eliminar"
                                                    ItemStyle-HorizontalAlign="Center" HeaderText="Eliminar" HeaderStyle-Width="50px" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-3">
                            <asp:UpdatePanel ID="UpdDirecciones" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td class="style2">
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" width="150px">
                                                                <asp:Label ID="LblDirDir" runat="server" Text="Dirección :" />
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDirDir" runat="server" CssClass="txtLetra" MaxLength="200" Width="580px" />
                                                                <asp:Label ID="lblCO_CmbTipPers0" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="LblTipRes" runat="server" Text="Tipo de Residencia:" />
                                                            </td>
                                                            <td align="left" width="230px">
                                                                <asp:DropDownList ID="CmbTipRes" runat="server" Width="200px" AutoPostBack="false" />
                                                                <asp:Label ID="lblCO_CmbTipPers1" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="LblDptoContDir" runat="server" Text="Continente/Dpto.:" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="CmbDptoContDir" runat="server" Width="200px" OnSelectedIndexChanged="CmbDptoContDir_SelectedIndexChanged"
                                                                    AutoPostBack="True" />
                                                                <asp:Label ID="lblCO_CmbTipPers2" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="lblProvPaisDir" runat="server" Text="País/Provincia:" />
                                                            </td>
                                                            <td align="left" width="230px">
                                                                <asp:DropDownList ID="CmbProvPaisDir" runat="server" Width="200px" OnSelectedIndexChanged="CmbProvPaisDir_SelectedIndexChanged"
                                                                    AutoPostBack="True" />
                                                                <asp:Label ID="lblCO_CmbTipPers3" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="LblDistCiuDir" runat="server" Text="Distrito/Ciudad:" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="CmbDistCiuDir" runat="server" Width="200px" OnSelectedIndexChanged="CmbDistCiuDir_SelectedIndexChanged"
                                                                    AutoPostBack="True" />
                                                                <asp:Label ID="lblCO_CmbTipPers4" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="lblCodPostal" runat="server" Text="Código Postal:"></asp:Label>
                                                            </td>
                                                            <td align="left" width="230px">
                                                                <asp:TextBox ID="txtCodPost" runat="server" CssClass="txtLetra" MaxLength="20" onkeypress="return isNumeroLetra(event)"
                                                                    Width="196px" />
                                                                <%--<asp:Label ID="lblCO_CmbTipPers5" runat="server" Style="color: #FF0000" Text="*"></asp:Label>--%>
                                                            </td>
                                                            <td align="left" width="120px">
                                                                <asp:Label ID="LblTelfDir" runat="server" Text="Teléfono :" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtTelfDir" runat="server" Width="196px" MaxLength="25" onkeypress="return ValidaTelefono(event)" />
                                                                <%--<asp:Label ID="lblCO_CmbTipPers6" runat="server" Style="color: #FF0000" Text="*"></asp:Label>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Button ID="btn_GrabarDir" runat="server" Text="      Grabar Dirección" CssClass="btnSaveAnotacion"
                                                                OnClientClick="javascript:return IsValidDirecciones();" OnClick="btn_GrabarDir_Click" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                            <asp:Button ID="btnLimpiarDireccion" runat="server" CssClass="btnLimpiar" OnClick="btnLimpiarDireccion_Click"
                                                                TabIndex="29" Text="       Limpiar" Width="170px" />
                                                            &nbsp; &nbsp;
                                                        </td>
                                                        <td id="tdMsjeDir">
                                                            <asp:Label ID="lblValidacionDir" runat="server" Text="Debe ingresar los campos requeridos"
                                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidDirID" runat="server" EnableViewState="true" Value="" />
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField ID="hidPersID" runat="server" EnableViewState="true" Value="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                <asp:GridView ID="GrdDirecciones" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                                                    AlternatingRowStyle-CssClass="alt" OnRowCreated="GrdDirecciones_RowCreated" OnRowCommand="GrdDirecciones_RowCommand"
                                                    TabIndex="37" OnRowDataBound="GrdDirecciones_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="iResidenciaId" HeaderText="ResidenciaId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iPersonaId" HeaderText="PersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vCodigoPostal" HeaderText="CodigoPostal" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vResidenciaDireccion" HeaderText="Dirección">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sResidenciaTipoId" HeaderText="ResidenciaTipoId," HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vResidenciaTipo" HeaderText="Tipo Residencia">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cResidenciaUbigeo" HeaderText="ResidenciaUbigeo" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DptoCont" HeaderText="Dpto./Cont.">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProvPais" HeaderText="Prov./Pais.">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DistCiu" HeaderText="Dist./Ciudad.">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vResidenciaTelefono" HeaderText="Teléfonos">
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-4">
                            <table width="100%">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblMsjFiliacion" runat="server" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:130px">
                                        <asp:Label ID="lblTipFiliacion" runat="server" Text="Tipo Filiación :" />
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddl_TipFiliacion" runat="server" Width="190px" TabIndex="40" />
                                        <asp:Label ID="lblCO_CmbTipPers7" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_peid_sDocumentoTipoId" runat="server" Text="Tipo Documento:"></asp:Label>
                                    </td>
                                    <td style="width:220px">
                                        <asp:DropDownList ID="ddl_peid_sDocumentoTipoId" runat="server" onchange="ddl_peid_sDocumentoTipoId_changed()"
                                            Style="width: 190px;">
                                        </asp:DropDownList>                                        
                                        <asp:Label ID="Label12" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                    </td>
                                    <td align="right" style="width:120px">                                        
                                        <asp:Label ID="lbl_peid_vDocumentoNumero" runat="server" Text="Nro Documento :"></asp:Label>
                                    </td>
                                    <td style="width:220px">
                                        <asp:TextBox ID="txt_peid_vDocumentoNumero" CssClass="txtLetra" runat="server" onkeypress="return ValidarNumeroDocumentoFil(event)"
                                            Style="width: 130px"></asp:TextBox>
                                            <p style="color: Red; display: inline; background: White;">*</p>
                                        <%--<img alt="" src="../Images/img_16_search.png" onclick="javascript:filiacion_persona('1');"
                                            style="cursor: pointer;" />--%>
                                        &nbsp;
                                        <asp:ImageButton ID="imgBuscarFiliado" ImageUrl="../Images/img_16_search.png" runat="server"
                                            OnClick="imgBuscarFiliado_Click" />
                                    </td>
                                    <td align="right" style="width:110px">
                                        
                                        <asp:Label ID="lbl_pers_sNacionalidadId" runat="server" Text="Nacionalidad:"></asp:Label>
                                    </td>
                                    <td style="width:190px">
                                        <asp:DropDownList ID="ddl_pers_sNacionalidadId" runat="server"  Width="150px"
                                            Enabled="False">
                                        </asp:DropDownList>
                                        <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Nombres :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pers_vNombres"  onkeypress="return ValidarSujeto(event)"
                                            runat="server" MaxLength="100" Width="180px" class="txtLetra" Enabled="False"></asp:TextBox>
                                            <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    <td align="right" style="width:120px">
                                       
                                        <asp:Label ID="Label10" runat="server" Text="Primer Apellido :"></asp:Label>
                                    </td>
                                    <td style="width:220px">
                                        <asp:TextBox ID="txt_pers_vApellidoPaterno"  onkeypress="return ValidarSujeto(event)"
                                            runat="server" MaxLength="100" class="txtLetra" Enabled="False" Style="width: 130px"></asp:TextBox>
                                            <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    <td align="right" style="width:110px">
                                        
                                        <asp:Label ID="Label11" runat="server" Text="Segundo Apellido:"></asp:Label>
                                    </td>
                                    <td style="width:190px">
                                        <asp:TextBox ID="txt_pers_vApellidoMaterno"  onkeypress="return ValidarSujeto(event)"
                                            runat="server" MaxLength="100" class="txtLetra" Enabled="False" Style="width: 150px"></asp:TextBox>                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Lugar de Nacimiento:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txt_pefi_vLugarNacimiento" runat="server" onkeypress="return isDireccion(event)"
                                            MaxLength="500" class="txtLetra" Width="100%" CssClass="txtLetra"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <%--<button id="btn_addFiliacion" class="btnSaveAnotacion" type="button">
                                            Agregar Filiación   --%>
                                        <asp:Button ID="btnGrabarFiliacion" runat="server" Text="      Agregar Filiación"
                                        OnClientClick="javascript:return IsValidFiliacion();"
                                            CssClass="btnSaveAnotacion" OnClick="btnGrabarFiliacion_Click" />
                                        <asp:Button ID="btnLimpiarFiliacion" runat="server" Text="      Limpiar"
                                            CssClass="btnLimpiar" OnClientClick="limpiarFiliacion()" OnClick="btnLimpiarFiliacion_Click"/>
                                    </td>
                                    <td>
                                      <asp:Label ID="lblValidacionFiliacion" runat="server" Text="Debe ingresar los campos requeridos"
                                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      </asp:Label>  
                                    </td>
                                </tr>
                            </table>
                            <input type="text" id="pers_iPersonaId" value="0" style="visibility: hidden" />
                            <input type="text" id="pefi_iPersonaFilacionId" value="0" style="visibility: hidden" />
                            <asp:UpdatePanel ID="updFiliacion" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button ID="btn_GridFiliacionRefresh" runat="server" Text="Refresh" OnClick="btn_GridFiliacionRefresh_Click"
                                        Style="visibility: hidden" />
                                    <asp:GridView ID="GrdFiliacion" runat="server" AutoGenerateColumns="False" Width="886px"
                                        CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" ShowHeaderWhenEmpty="True"
                                        OnRowCommand="GrdFiliacion_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="pefi_iPersonaFilacionId" HeaderText="pefi_iPersonaFilacionId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="pefi_iPersonaId" HeaderText="pefi_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="pefi_sTipoFilacionId" HeaderText="pefi_sTipoFilacionId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="pefi_sTipoFilacionId_desc" HeaderText="Tipo" />
                                            <asp:BoundField DataField="pefi_vNombreFiliacion" HeaderText="Apellidos y Nombres" />
                                            <asp:BoundField DataField="pefi_vLugarNacimiento" HeaderText="Lugar Nacimiento" />
                                            <asp:BoundField DataField="pefi_sNacionalidad_desc" HeaderText="Nacionalidad" />
                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/img_16_edit.png" CommandName="Editar"
                                                Visible="false" />
                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/Images/img_grid_delete.png" CommandName="Eliminar" />
                                        </Columns>
                                        <SelectedRowStyle CssClass="slt" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-5">
                            <asp:UpdatePanel ID="UpdContacto" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblRelCto" runat="server" Text="Relación :" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="CmbRelCto" runat="server" Width="182px" TabIndex="47" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblNomComCto" runat="server" Text="Nombre Completo :" />
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="TxtNomCompCont" runat="server" Width="401px" CssClass="txtLetra"
                                                                MaxLength="150" TabIndex="48" onkeypress="return isSujeto(event)" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblDirExtrCto" runat="server" Text="Dirección en el Extranjero :"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtDirExtrCont" runat="server" Width="443px" CssClass="txtLetra"
                                                                MaxLength="255" TabIndex="49" onkeypress="return isDireccion(event)" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblCodPostalCto" runat="server" Text="Código Postal :" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtCodPostCont" runat="server" CssClass="txtLetra" MaxLength="10"
                                                                TabIndex="50" onkeypress="return isSujeto(event)" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblTelfCto" runat="server" Text="Teléfonos :" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtTelfCont" runat="server" Width="195px" MaxLength="25" TabIndex="51"
                                                                onkeypress="return OnlyNumeros(event)" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblDirPerCto" runat="server" Text="Dirección en el Perú :" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtDirPerCont" runat="server" Width="443px" CssClass="txtLetra"
                                                                MaxLength="255" TabIndex="52" onkeypress="return isDireccion(event)" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblMailCto" runat="server" Text="Correo Electrónico :" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtMailCont" runat="server" Width="405px" CssClass="txtLetra" MaxLength="55"
                                                                TabIndex="53" onblur="if(this.value!=''){ validarEmail(this); }" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-6">
                            <asp:UpdatePanel ID="Updmiratorio" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <table class="mTblInterna">
                                                    <tr>
                                                        <td colspan="2" style="background-color: #C0C0C0; font-weight: bold; color: #000000">
                                                            <asp:Label ID="LblInfoMigratoria" runat="server" Text="Información Voluntaria de Carácter Migratorio" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblAñoVivDesdeExtr" runat="server" Text="¿Desde qué año vive en el exterior?" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAñoVivDesde" runat="server" Width="50px" MaxLength="4" TabIndex="55"
                                                                onkeypress="return OnlyNumeros(event)"></asp:TextBox>
                                                            <asp:DropDownList ID="ddl_MesVivDesde" runat="server" TabIndex="56">
                                                            </asp:DropDownList>
                                                            &nbsp;<asp:Label ID="Label2" runat="server" Text="(Año / Mes)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblRetornExt" runat="server" Text="¿Piensa retornar para residir en Perú en algún momento?" />
                                                        </td>
                                                        <td>    
                                                            <label>
                                                                <asp:RadioButton GroupName="GroupRdioRetExt" ID="RdioSiRetornExt" runat="server" TabIndex="57"  onClick="ValidarCheckRetornar(this)" />
                                                                Si
                                                            </label>
                                                            <label>
                                                                <asp:RadioButton GroupName="GroupRdioRetExt" ID="RdioNoRetornExt" runat="server" TabIndex="57"  onClick="ValidarCheckRetornar(this)" />
                                                                No
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblMRetornExt" runat="server" Text="Si la respuesta es afirmativa ¿Cuándo piensa retornar?" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAñoRegreso" runat="server" Width="67px" MaxLength="4" TabIndex="58"
                                                                Enabled="False" onkeypress="return OnlyNumeros(event)"></asp:TextBox>
                                                            <asp:DropDownList ID="ddl_MesRegreso" runat="server" TabIndex="59" Enabled="False">
                                                            </asp:DropDownList>
                                                            &nbsp;<asp:Label ID="Label3" runat="server" Text="(Año / Mes)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblOcupPerú" runat="server" Text="Ocupación en el Perú" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="CbmOcupPeru" runat="server" Width="165px" TabIndex="60" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblOcupExterior" runat="server" Text="Ocupación en el Exterior" Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="CbmOcupExterior" runat="server" Width="165px" TabIndex="61"
                                                                Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #C0C0C0; font-weight: bold; color: #000000">
                                                            <asp:Label ID="lblAfilPeru" runat="server" Text="En el Perú" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            <asp:Label ID="LblAfilSegSoc" runat="server" Text="¿Está afiliado a la Seguridad Social Pública?" />
                                                        </td>
                                                        <td>
                                                            <label >
                                                                <asp:RadioButton GroupName="RdioAfilSegSoc" ID="RdioSiAfilSegSoc" runat="server" TabIndex="62" />
                                                                Si
                                                            </label>
                                                            <label >
                                                                <asp:RadioButton GroupName="RdioAfilSegSoc" ID="RdioNoAfilSegSoc" runat="server" TabIndex="62" />
                                                                No
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblAfilAFP" runat="server" Text="¿Estaba afiliado(a) a una AFP?" />
                                                        </td>
                                                        <td>
                                                             <label>
                                                                <asp:RadioButton GroupName="RdioAfilAFP" ID="RdioSiAfilAFP" runat="server" TabIndex="62" />
                                                                Si
                                                            </label>
                                                            <label >
                                                                <asp:RadioButton GroupName="RdioAfilAFP" ID="RdioNoAfilAFP" runat="server" TabIndex="62"  />
                                                                No
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #C0C0C0; font-weight: bold; color: #000000">
                                                            <asp:Label ID="LblAportExterior" runat="server" Text="En el Exterior" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblAportSegSoc" runat="server" Text="¿Estaba aportando(a) a la Seguridad Social?" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <asp:RadioButton GroupName="RdioAportSegSoc" ID="RdioSiAportSegSoc" runat="server" TabIndex="62" />
                                                                Si
                                                            </label>
                                                            <label >
                                                                <asp:RadioButton GroupName="RdioAportSegSoc" ID="RdioNoAportSegSoc" runat="server" TabIndex="62"  />
                                                                No
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="LblBenficExter1" runat="server" Text="¿Se ha beneficiado Usted, durante su permanencia en el exterior, de algún acuerdo o convenio de naturaleza" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblBenficExter2" runat="server" Text="migratoria, bilateral o multilateral?" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <asp:RadioButton GroupName="RdioBenExter" ID="RdioSiBenExter" onClick="ValidarCheckRetornar(this)" runat="server" TabIndex="62" />
                                                                Si
                                                            </label>
                                                            <label >
                                                                <asp:RadioButton GroupName="RdioBenExter" ID="RdioNoBenExter" onClick="ValidarCheckRetornar(this)" runat="server" TabIndex="62"  />
                                                                No
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="LblAcuerConv" runat="server" Text="Si la respuesta es afirmativa, por favor indique el nombre del acuerdo o convenio" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="TxtAcuerConv" runat="server" Width="588px" CssClass="txtLetra" TabIndex="66"
                                                                Enabled="False" onkeypress="return isNumeroLetra(event)" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-7">
                            <asp:UpdatePanel ID="updobservaciones" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="mTblSecundaria">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TxtObsRune" runat="server" Height="176px" TextMode="MultiLine" Width="840px"
                                                    CssClass="txtLetra" TabIndex="68" onkeypress="return isNumeroLetra(event)" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
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
    <br />
    <div style="color: Black; width: 90%; margin-left: 50px;">
        <b>* Datos Obligatorios de conformidad con el Art. 54 del D.L.N° 1049.</b>
    </div>
    <div id="cancelDialog" style="display: none;" title="Please confirm cancellation">
        <p>
            Are you sure you wish to cancel and abandon your changes?</p>
    </div>
    <br />
    <script language="javascript" type="text/javascript">


        $(function () {

            $('#tabs').tabs();

            var $myDialog = $("#cancelDialog").dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                    'Yes, cancel': function () {
                        $(this).dialog('close');
                        return true;
                    },
                    'No, resume editing': function () {
                        $(this).dialog('close');
                        return false;
                    }
                },

                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
        });

        function validarcaracterrune() {
            var caracterespecial = $("#<%= HFValidarTextoRune.ClientID %>").val();
            var caracter = caracterespecial.split(',');

            var evento = window.event;
            var codigo = evento.charCode || evento.keyCode;
            var char = String.fromCharCode(codigo);

            var bValido = false;

            for (var x = 0; x < caracter.length; x++) {

                if (caracter[x] == char) {
                    bValido = true;
                }
            }

            return bValido;
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

            letra = validarcaracterespecial();
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

            letra = validarcaracterespecial();
            return letra;
        }

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

            letra = validarcaracterespecial();
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

        function isLetra(evt) {
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

            var letras = "áéíóúñÑü";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            letra = validarcaracterespecial();
            return letra;
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

        function isDireccion(evt) {
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

            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ#-_,;:.";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }
        function ValidaTelefono(evt) {
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
            var letras = "#-_";
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

        function isLetraNumeroDoc(evt) {


            var letra = false;
            var charCode = (evt.which) ? evt.which : event.keyCode


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
            //            }

            letra = validarcaracterespecial();
            return letra;
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
                                           "Aceptar": function () {
                                               $(this).dialog("close");
                                           }
                                       }
                                   });
        }

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function EnableTipoBusqueda(rdoTipo) {
            if (rdoTipo == 1 || rdoTipo == 2) {
                $("#<%= CmbNacRecurr.ClientID %>").attr('disabled', 'disabled');
                $("#<%= CmbTipoDoc.ClientID %>").attr('disabled', 'disabled');
                $("#<%= txtApePat.ClientID %>").attr('disabled', 'disabled');
                $("#<%= txtApeMat.ClientID %>").attr('disabled', 'disabled');
                $("#<%= txtApepCas.ClientID %>").attr('disabled', 'disabled');
                $("#<%= txtNombres.ClientID %>").attr('disabled', 'disabled');
            }
            else if (rdoTipo == 3) {
                $("#<%= CmbNacRecurr.ClientID %>").attr('enabled', 'enabled');
                $("#<%= CmbTipoDoc.ClientID %>").attr('enabled', 'enabled');
                $("#<%= txtApePat.ClientID %>").attr('enabled', 'enabled');
                $("#<%= txtApeMat.ClientID %>").attr('enabled', 'enabled');
                $("#<%= txtApepCas.ClientID %>").attr('enabled', 'enabled');
                $("#<%= txtNombres.ClientID %>").attr('enabled', 'enabled');
            }
        }

        function LimpiarCabeceraRUNE() {
            $("#<%= HF_ModoEdicion.ClientID %>").val('new');
            $("#<%= CmbNacRecurr.ClientID %>").val('2051');
            $("#<%= chk_PJ.ClientID %>").prop("checked", "");
            $("#<%= chk_PJ.ClientID %>").attr('enabled', 'enabled');
            $("#<%= CmbTipPers.ClientID %>").val('2101');
            $("#<%= CmbTipoDoc.ClientID %>").val('1');
            $("#<%= txtNroDoc.ClientID %>").val('');
            $("#<%= txtApePat.ClientID %>").val('');
            $("#<%= txtApeMat.ClientID %>").val('');
            $("#<%= txtApepCas.ClientID %>").val('');
            $("#<%= txtNombres.ClientID %>").val('');
            $("#<%= lblValidacion.ClientID %>").hide();
            $("#<%=CmbGenero.ClientID %>").css("border", "solid #888888 1px");
            $('#<%=ctrFecNac.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
            $("#<%=CmbOcupacion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_Profesion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=CmbGradInst.ClientID %>").css("border", "solid #888888 1px");
        }

        function LimpiarPestañaAdicionalesYObservaciones() {
            $("#<%= CmbEstCiv.ClientID %>").val('0');
            $("#<%= CmbGenero.ClientID %>").val('0');
            $("#<%= CmbOcupacion.ClientID %>").val('0');
            $("#<%= ddl_Profesion.ClientID %>").val('0');
            $("#<%= CmbGradInst.ClientID %>").val('0');
            $('#<%= ctrFecNac.FindControl("TxtFecha") %>').val('');
            $("#<%= LblEdad.ClientID %>").show().show().css({ "display": "none" });
            $("#<%= txtEdad.ClientID %>").val('');
            $("#<%= txtEdad.ClientID %>").show().show().css({ "display": "none" });
            $("#<%= CmbDptoContNac.ClientID %>").val('00');
            $("#<%= CmbProvPaisNac.ClientID %>").val('00');
            $("#<%= CmbProvPaisNac.ClientID %>").attr('disabled', 'disabled');
            $("#<%= CmbDistCiudadNac.ClientID %>").val('00');
            $("#<%= CmbDistCiudadNac.ClientID %>").attr('disabled', 'disabled');
            $("#<%= TxtEmail.ClientID %>").val('');
            $("#<%= TxtObsRune.ClientID %>").val('');

            $("#<%= txtTelefonoDir.ClientID %>").val('');
            $("#<%= txtCodPostalDir.ClientID %>").val('');
             

        }

        function LimpiarPestañaDocumentos() {
            $("#<%= ddl_TipoDocumentoM.ClientID %>").val('1');
            $("#<%= txtNroDocumentoM.ClientID %>").val('');
            $('#<%= ctrFecVcto.FindControl("TxtFecha").ClientID %>').val('');
            $('#<%= ctrFecExped.FindControl("TxtFecha").ClientID %>').val('');
            $("#<%= txtLugExp.ClientID %>").val('');
            $('#<%= ctrFecRenov.FindControl("TxtFecha").ClientID %>').val('');
            $("#<%= txtLugRenov.ClientID %>").val('');
            $("#<%= chk_ActRUNE.ClientID %>").prop("checked", "");

            $("#<%= lblValidacionDoc.ClientID %>").hide();
        }

        function LimpiarPestañaDirecciones() {
            $("#<%= hidDirID.ClientID %>").val('');
            $("#<%= hidPersID.ClientID %>").val('');
            $("#<%= TxtDirDir.ClientID %>").val('');
            $("#<%= CmbTipRes.ClientID %>").val('0');
            $("#<%= CmbDptoContDir.ClientID %>").val('00');
            $("#<%= CmbProvPaisDir.ClientID %>").val('00');
            $("#<%= CmbDistCiuDir.ClientID %>").val('00');
            $("#<%= txtCodPost.ClientID %>").val('');
            $("#<%= TxtTelfDir.ClientID %>").val('');

            $("#<%=TxtDirDir.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=CmbTipRes.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=CmbDptoContDir.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=CmbProvPaisDir.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=CmbDistCiuDir.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtCodPost.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=TxtTelfDir.ClientID %>").css("border", "solid #888888 1px");

            $("#<%= lblValidacionDir.ClientID %>").hide();
        }

        function LimpiarPestañaContactosYDatosMigratorios() {
            $("#<%= TxtNomCompCont.ClientID %>").val('');
            $("#<%= CmbRelCto.ClientID %>").val('0');
            $("#<%= TxtDirExtrCont.ClientID %>").val('');
            $("#<%= TxtCodPostCont.ClientID %>").val('');
            $("#<%= TxtTelfCont.ClientID %>").val('');
            $("#<%= TxtDirPerCont.ClientID %>").val('');
            $("#<%= TxtMailCont.ClientID %>").val('');
            $("#<%= txtAñoVivDesde.ClientID %>").val('');
            $("#<%= ddl_MesVivDesde.ClientID %>").val('00');
            $("#<%= RdioSiRetornExt.ClientID %>").prop("checked", "");
            $("#<%= txtAñoRegreso.ClientID %>").val('');
            $("#<%= ddl_MesRegreso.ClientID %>").val('00');
            $("#<%= RdioSiAfilSegSoc.ClientID %>").prop("checked", "");
            $("#<%= RdioSiAfilAFP.ClientID %>").prop("checked", "");
            $("#<%= RdioSiAportSegSoc.ClientID %>").prop("checked", "");
            $("#<%= RdioSiBenExter.ClientID %>").prop("checked", "");
            $("#<%= CbmOcupPeru.ClientID %>").val('0');
            $("#<%= CbmOcupExterior.ClientID %>").val('0');
            $("#<%= TxtAcuerConv.ClientID %>").val('');
        }

        function EditarDirecciones(CodUbigeo) {
            var DptoCont = CodUbigeo.substring(0, 2);
            $("#<%= CmbDptoContDir.ClientID %>").val(DptoCont);
        }

        function IsValidDatosRUNE() {

            var bolValida = true;
            //var strTipoDocumento = "6";
            var strTipoDocumento = $.trim($("#<%= HF_ValidaDocumento.ClientID %>").val());

            var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());
            var strNacRecurrDesc = $.trim($("#<%= CmbNacRecurr.ClientID %> option:selected").text());

            var strTipPers = $.trim($("#<%= CmbTipPers.ClientID %>").val());
            var strTipoDoc = $.trim($("#<%= CmbTipoDoc.ClientID %>").val());
            var strTipoDocDesc = $.trim($("#<%= CmbTipoDoc.ClientID %> option:selected").text());

            var strGenero = $.trim($("#<%= CmbGenero.ClientID %>").val());

            var strEstCivil = $.trim($("#<%= CmbEstCiv.ClientID %>").val());

            var strNroDoc = $.trim($("#<%= txtNroDoc.ClientID %>").val());
            var strApePat = $.trim($("#<%= txtApePat.ClientID %>").val());
            var strApeMat = $.trim($("#<%= txtApeMat.ClientID %>").val());
            var strNombres = $.trim($("#<%= txtNombres.ClientID %>").val());

            var strRazSoc = $.trim($("#<%= txtRazSoc.ClientID %>").val());

            var strFecNacimiento = $.trim($('#<%=ctrFecNac.FindControl("TxtFecha").ClientID %>').val());

            var strRdioSiBenExter = $("#<%=RdioSiBenExter.ClientID %>").prop("checked") ? true : false;
            var strSiNoRetornExt = $("#<%=RdioSiRetornExt.ClientID %>").prop("checked") ? true : false;

            var strAcuerConv = $.trim($('#<%=TxtAcuerConv.ClientID %>').val());

            var strAñoRegresoo = $.trim($("#<%= txtAñoRegreso.ClientID %>").val());
            var strMesRegresoo = $.trim($("#<%= ddl_MesRegreso.ClientID %>").val());



            if (strNroDoc.length == 0) {
                $("#<%=txtNroDoc.ClientID %>").css("border", "solid Red 1px");
                alert('Digite el número de documento.');
                bolValida = false;
                return false;
            } else {
                $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");

                var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");

                if (valoresDocumento[2] == "True" && isNaN(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' solo permite números.");
                    $("#<%= lblValidacion.ClientID %>").show();
                    bolValida = false;
                    return false;
                }

                else if (valoresDocumento[0] > strNroDoc.length || valoresDocumento[1] < strNroDoc.length) {

                    $("#<%= lblValidacion.ClientID %>").html(valoresDocumento[3]);
                    bolValida = false;
                }
                else if (valoresDocumento[4] != '' && valoresDocumento[4] != strNacRecurr) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' no aplica para el tipo de nacionalidad '" + strNacRecurrDesc + "'");
                    bolValida = false;
                    $("#<%= lblValidacion.ClientID %>").show();
                    return false;
                }
                else if (soloCeros(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El número de documento es incorrecto");
                    bolValida = false;
                    $("#<%= lblValidacion.ClientID %>").show();
                    return false;
                }
                else {
                    $("#<%= lblValidacion.ClientID %>").html("");
                }

            }


            if (strApePat.length == 0) {
                $("#<%=txtApePat.ClientID %>").css("border", "solid Red 1px");
                alert('Digite el primer Apellido.');
                bolValida = false;
                return false;
            }
            else {
                $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strTipoDoc == strTipoDocumento) {
                if (strApeMat.length == 0) {
                    $("#<%=txtApeMat.ClientID %>").focus();
                    $("#<%=txtApeMat.ClientID %>").css("border", "solid Red 1px");
                    alert('Digite el segundo Apellido.');
                    bolValida = false;
                    return false;
                } else {
                    $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                }
            }
            if (strNombres.length == 0) {
                $("#<%=txtNombres.ClientID %>").css("border", "solid Red 1px");
                alert('Digite los nombres.');
                bolValida = false;
                return false;
            }
            else {
                $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strFecNacimiento.length > 0) {
                 $("#<%=HD_FecNac.ClientID %>").val(strFecNacimiento);
            }
            else
            {
                $("#<%=HD_FecNac.ClientID %>").val("");
            }

            var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());

            /*VALIDACIÓN SOLO PARA PERUANOS*/
            if (strNacRecurr == '2051') {

                if (strGenero == 0) {
                    $("#<%= CmbGenero.ClientID %>").css("border", "solid Red 1px");
                    alert('Seleccione el género.');
                    bolValida = false;
                    return false;
                }
                else {
                    $("#<%= CmbGenero.ClientID %>").css("border", "solid #888888 1px");

                }


                if (strEstCivil == 0) {
                    $("#<%= CmbEstCiv.ClientID %>").css("border", "solid Red 1px");
                    alert('Seleccione el estado civil.');
                    bolValida = false;
                    return false;
                }
                else {
                    $("#<%= CmbEstCiv.ClientID %>").css("border", "solid #888888 1px");
                }

                var strOcupacion = $.trim($("#<%= CmbOcupacion.ClientID %>").val());



                var strProfesion = $.trim($("#<%= ddl_Profesion.ClientID %>").val());
                var strGradInst = $.trim($("#<%= CmbGradInst.ClientID %>").val());
                if (strFecNacimiento.length > 0) {
                    var validar_edad = calcular_edad(strFecNacimiento);
                    if (validar_edad) {
                        if (strOcupacion == 0) {
                            $("#<%= lblValidacion.ClientID %>").html("Seleccione la ocupación.");
                            $("#<%= CmbOcupacion.ClientID %>").css("border", "solid Red 1px");
                            $("#<%= lblValidacion.ClientID %>").show();
                            alert('Seleccione la ocupación.');
                            return false;
                            bolValida = false;
                        }
                        else {
                            $("#<%= lblValidacion.ClientID %>").html("");
                            $("#<%= CmbOcupacion.ClientID %>").css("border", "solid #888888 1px");

                        }
                        //$("#<%= lblValidacion.ClientID %>").html('');
                        $('#<%= ctrFecNac.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
                        $("#<%=ddl_Profesion.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbGradInst.ClientID %>").css("border", "solid #888888 1px");
                        //$("#<%= CmbOcupacion.ClientID %>").css("border", "solid #888888 1px");
                    }
                    else {
                        $("#<%= CmbOcupacion.ClientID %>").css("border", "solid #888888 1px");
                        //                    var anio = '<%=ConfigurationManager.AppSettings["edadmaximo"].ToString() %>';
                        //                    if (strOcupacion != '0' || strProfesion != '0' || strGradInst != '0') {
                        //                        MoveTabIndex(0);
                        //                        $("#<%=lblValidacion.ClientID %>").html('Fecha de Nacimiento Ingresado es menor que la edad Mínima que es de ' + anio + " años. No debe ingresar datos de profesión, ocupación ni grado de instrucción.");
                        //                        $('#<%=ctrFecNac.FindControl("TxtFecha").ClientID %>').css("border", "solid Red 1px");
                        //                        $("#<%=ddl_Profesion.ClientID %>").css("border", "solid Red 1px");
                        //                        $("#<%=CmbGradInst.ClientID %>").css("border", "solid Red 1px");
                        //                        bolValida = false;
                    }
                    //                }
                }
                else {
                    if (strOcupacion == 0) {
                        $("#<%= lblValidacion.ClientID %>").html("Seleccione la ocupación.");
                        $("#<%= CmbOcupacion.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione la ocupación.');
                        bolValida = false;
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html("");
                        $("#<%= CmbOcupacion.ClientID %>").css("border", "solid #888888 1px");

                    }
                }

                var strDptoContNac = $.trim($("#<%= CmbDptoContNac.ClientID %>").val());
                var strProvPaisNac = $.trim($("#<%= CmbProvPaisNac.ClientID %>").val());
                var strDistCiudadNac = $.trim($("#<%= CmbDistCiudadNac.ClientID %>").val());

                if (strDptoContNac == 0) {
                    $("#<%= lblValidacion.ClientID %>").html("Seleccione la ubicación del nacimiento.");
                    $("#<%= CmbDptoContNac.ClientID %>").css("border", "solid Red 1px");
                    alert('Seleccione el Continente/Dpto.de Nacimiento.');
                    bolValida = false;
                    $("#<%= lblValidacion.ClientID %>").show();
                    return false;
                }
                else {
                    $("#<%= lblValidacion.ClientID %>").html("Debe ingresar los campos requeridos*");
                    $("#<%= CmbDptoContNac.ClientID %>").css("border", "solid #888888 1px");
                }
                if (strProvPaisNac == 0) {
                    $("#<%= lblValidacion.ClientID %>").html("Seleccione la ubicación del nacimiento.");
                    $("#<%= CmbProvPaisNac.ClientID %>").css("border", "solid Red 1px");
                    alert('Seleccione el País/Provincia de Nacimiento.');
                    bolValida = false;
                    $("#<%= lblValidacion.ClientID %>").show();
                    return false;
                }
                else {
                    $("#<%= lblValidacion.ClientID %>").html("");
                    $("#<%= CmbProvPaisNac.ClientID %>").css("border", "solid #888888 1px");
                }
                if (strDistCiudadNac == 0) {
                    $("#<%= lblValidacion.ClientID %>").html("Seleccione la ubicación del nacimiento.");
                    $("#<%= CmbDistCiudadNac.ClientID %>").css("border", "solid Red 1px");
                    alert('Seleccione el Distrito/Ciudad de Nacimiento.');
                    bolValida = false;
                    $("#<%= lblValidacion.ClientID %>").show();
                    return false;
                }
                else {
                    $("#<%= lblValidacion.ClientID %>").html("");
                    $("#<%= CmbDistCiudadNac.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strSiNoRetornExt) {

                    if (strAñoRegresoo.length == 0) {
                        $("#<%= txtAñoRegreso.ClientID %>").css("border", "solid Red 1px");
                        MoveTabIndex(5);
                        bolValida = false;
                    }
                    else {
                        $("#<%= txtAñoRegreso.ClientID %>").css("border", "solid #888888 1px");
                    }


                    if (strMesRegresoo == '00' || strMesRegresoo == '') {
                        $("#<%= ddl_MesRegreso.ClientID %>").css("border", "solid Red 1px");
                        MoveTabIndex(5);
                        bolValida = false;
                    }
                    else {
                        $("#<%= ddl_MesRegreso.ClientID %>").css("border", "solid #888888 1px");
                    }
                }
                else {
                    $("#<%= txtAñoRegreso.ClientID %>").css("border", "solid #888888 1px");
                    $("#<%= ddl_MesRegreso.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strRdioSiBenExter) {
                    if (strAcuerConv.length == 0) {
                        $("#<%= TxtAcuerConv.ClientID %>").css("border", "solid Red 1px");
                        MoveTabIndex(5);
                        bolValida = false;
                    }
                    else {
                        $("#<%= TxtAcuerConv.ClientID %>").css("border", "solid #888888 1px");
                    }
                }
                else {
                    $("#<%= TxtAcuerConv.ClientID %>").css("border", "solid #888888 1px");
                }
                var esVisible = $("#<%= direccionResidencia.ClientID %>").is(":visible");

                if (esVisible == true) {
                    var strDireccionResidencia = $.trim($("#<%= txtDireccionResidencia.ClientID %>").val());
                    var strDptoContResidencia = $.trim($("#<%= ddlDptContinenteResidencia.ClientID %>").val());
                    var strProvPaisResidencia = $.trim($("#<%= ddlProvPaisResidencia.ClientID %>").val());
                    var strDistCiudadResidencia = $.trim($("#<%= ddlDistCiudadResidencia.ClientID %>").val());

                    if (strDireccionResidencia.length == 0) {
                        $("#<%= lblValidacion.ClientID %>").html("Debe ingresar la dirección de residencia.");
                        $("#<%= txtDireccionResidencia.ClientID %>").css("border", "solid Red 1px");
                        alert('Digite la Dirección de residencia.');
                        $("#<%= lblValidacion.ClientID %>").show();
                        bolValida = false;
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html("");
                        $("#<%= txtDireccionResidencia.ClientID %>").css("border", "solid #888888 1px");
                    }
                    if (strDptoContResidencia == 0) {
                        $("#<%= lblValidacion.ClientID %>").html("Seleccione el Continente/Dpto. de residencia.");
                        $("#<%= ddlDptContinenteResidencia.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Continente/Dpto. de residencia');
                        bolValida = false;
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html("");
                        $("#<%= ddlDptContinenteResidencia.ClientID %>").css("border", "solid #888888 1px");
                    }
                    if (strProvPaisResidencia == 0) {
                        $("#<%= lblValidacion.ClientID %>").html("Seleccione el País/Provincia de residencia.");
                        $("#<%= ddlProvPaisResidencia.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el País/Provincia de residencia.');
                        bolValida = false;
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html("");
                        $("#<%= ddlProvPaisResidencia.ClientID %>").css("border", "solid #888888 1px");
                    }
                    if (strDistCiudadResidencia == 0) {
                        $("#<%= lblValidacion.ClientID %>").html("Seleccione el Distrito/Ciudad de residencia.");
                        $("#<%= ddlDistCiudadResidencia.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Distrito/Ciudad de residencia.');
                        bolValida = false;
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html("");
                        $("#<%= ddlDistCiudadResidencia.ClientID %>").css("border", "solid #888888 1px");
                    }
                }

                if (bolValida) {

                    var strFecNac = $('#<%=ctrFecNac.FindControl("TxtFecha").ClientID %>').val();
                    var strDptoContNac = $.trim($("#<%= CmbDptoContNac.ClientID %>").val());
                    var strProvPaisNac = $.trim($("#<%= CmbProvPaisNac.ClientID %>").val());
                    var strDistCiudadNac = $.trim($("#<%= CmbDistCiudadNac.ClientID %>").val());

                    var strCodPostCont = $.trim($("#<%= TxtCodPostCont.ClientID %>").val());
                    var strTelfCont = $.trim($("#<%= TxtTelfCont.ClientID %>").val());
                    var strEmailRune = $.trim($("#<%= TxtEmail.ClientID %>").val());
                    var strMailCont = $.trim($("#<%= TxtMailCont.ClientID %>").val());

                    var strAñoVivDesde = $.trim($("#<%= txtAñoVivDesde.ClientID %>").val());
                    var strMesVivDesde = $.trim($("#<%= ddl_MesVivDesde.ClientID %>").val());

                    var strAñoRegreso = $.trim($("#<%= txtAñoRegreso.ClientID %>").val());
                    var strMesRegreso = $.trim($("#<%= ddl_MesRegreso.ClientID %>").val());
                    var strUbigeoResidencia = $.trim($("#<%= hubigeoResidencia.ClientID %>").val());

                    var strDptoContResidencia = $.trim($("#<%= ddlDptContinenteResidencia.ClientID %>").val());
                    var strProvPaisResidencia = $.trim($("#<%= ddlProvPaisResidencia.ClientID %>").val());
                    var strDistCiudadResidencia = $.trim($("#<%= ddlDistCiudadResidencia.ClientID %>").val());
                    var strDireccionResidencia = $.trim($("#<%= txtDireccionResidencia.ClientID %>").val());



                    var d = new Date();
                    var curr_year = d.getFullYear();
                    var curr_month = d.getMonth() + 1;
                    var personaId = getParameterByName('CodPer');

                    //===============Fecha: 02/10/2020  Autor: Vidal Pipa
                    //  se adiciona en el if  && $("#<%= txtDireccionResidencia.ClientID %>").val() !== undefined
                    //  por lo que despues de ocultar la seccion de datos de residencia los elementos ocultos es undefined
                    if (personaId.length == 0 && $("#<%= txtDireccionResidencia.ClientID %>").val() !== undefined) {
                        if (strDireccionResidencia == "") {
                            alert('Ingrese la dirección de residencia');
                            $("#<%=txtDireccionResidencia.ClientID %>").css("border", "solid Red 1px");
                            bolValida = false;
                            return false;
                        }
                        else {
                            $("#<%=txtDireccionResidencia.ClientID %>").css("border", "solid #888888 1px");
                            if (strUbigeoResidencia == "") {

                                if (strDptoContResidencia == "00") {
                                    $("#<%=ddlDptContinenteResidencia.ClientID %>").css("border", "solid Red 1px");
                                    alert('Seleccione el Continente/Dpto. de residencia.');
                                    return false;
                                }
                                else {
                                    $("#<%=ddlDptContinenteResidencia.ClientID %>").css("border", "solid #888888 1px");
                                }
                                if (strProvPaisResidencia == "0") {
                                    $("#<%=ddlProvPaisResidencia.ClientID %>").css("border", "solid Red 1px");
                                    alert('Seleccione el País/Provincia de residencia.');
                                    return false;
                                }
                                else { $("#<%=ddlProvPaisResidencia.ClientID %>").css("border", "solid #888888 1px"); }
                                if (strDistCiudadResidencia == "0") {
                                    $("#<%=ddlDistCiudadResidencia.ClientID %>").css("border", "solid Red 1px");
                                    alert('Seleccione el Distrito/Ciudad de residencia.');
                                    return false;
                                }
                                else {
                                    $("#<%=ddlDistCiudadResidencia.ClientID %>").css("border", "solid #888888 1px");
                                }
                                bolValida = false;
                            }
                        }
                    }

                    if (strDptoContNac != '00' && strProvPaisNac == '0' && strDistCiudadNac == '0') {
                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo del connacional.');
                        $("#<%=CmbProvPaisNac.ClientID %>").css("border", "solid Red 1px");
                        $("#<%=CmbDistCiudadNac.ClientID %>").css("border", "solid Red 1px");

                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strDptoContNac == '00' && strProvPaisNac != '0' && strDistCiudadNac == '0') {

                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo de nacimiento del connacional.');
                        $("#<%=CmbDptoContNac.ClientID %>").css("border", "solid Red 1px");
                        $("#<%=CmbDistCiudadNac.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Continente/Dpto. y Distrito/Ciudad de Nacimiento.');
                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strDptoContNac == '00' && strProvPaisNac == '0' && strDistCiudadNac != '0') {
                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo de nacimiento del connacional.');
                        $("#<%=CmbDptoContNac.ClientID %>").css("border", "solid Red 1px");
                        $("#<%=CmbProvPaisNac.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Continente/Dpto. y País/Provincia de Nacimiento.');
                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strDptoContNac != '00' && strProvPaisNac != '0' && strDistCiudadNac == '0') {
                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo de nacimiento del connacional.');
                        $("#<%=CmbDistCiudadNac.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Distrito/Ciudad de nacimiento.');
                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strDptoContNac == '00' && strProvPaisNac != '0' && strDistCiudadNac != '0') {
                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo de nacimiento del connacional.');
                        $("#<%=CmbDptoContNac.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el Continente/Dpto. de Nacimiento.');
                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strDptoContNac != '00' && strProvPaisNac == '0' && strDistCiudadNac != '0') {
                        $("#<%=lblValidacion.ClientID %>").html('No ha establecido correctamente el ubigeo de nacimiento del connacional.');
                        $("#<%=CmbProvPaisNac.ClientID %>").css("border", "solid Red 1px");
                        alert('Seleccione el País/Provincia. de Nacimiento.');
                        bolValida = false;

                        MoveTabIndex(0);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%= lblValidacion.ClientID %>").html('');
                    }

                    if (strEmailRune.length != 0) {
                        if (strEmailRune.indexOf('@', 0) == -1 || strEmailRune.indexOf('.', 0) == -1) {
                            $("#<%=TxtEmail.ClientID %>").focus();
                            $("#<%=lblValidacion.ClientID %>").html('El correo electrónico de la persona no es correcto.');
                            $("#<%=TxtEmail.ClientID %>").css("border", "solid Red 1px");
                            alert('Digite el correo electrónico correcto.');

                            $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                            bolValida = false;

                            MoveTabIndex(0);
                            $("#<%= lblValidacion.ClientID %>").show();
                            return false;
                        }
                        else {
                            $("#<%=TxtEmail.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=lblValidacion.ClientID %>").html('');
                        }
                    }

                    if (strCodPostCont.length > 10) {
                        $("#<%=TxtCodPostCont.ClientID %>").focus();
                        $("#<%=lblValidacion.ClientID %>").html('Ha excedido el número permitido de caracteres para el codigo postal del contacto.');
                        $("#<%=TxtCodPostCont.ClientID %>").css("border", "solid Red 1px");

                        $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                        bolValida = false;

                        MoveTabIndex(4);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%=TxtCodPostCont.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=lblValidacion.ClientID %>").html('');
                    }

                    if (strTelfCont.length > 50) {
                        $("#<%=TxtTelfCont.ClientID %>").focus();
                        $("#<%=lblValidacion.ClientID %>").html('Ha excedido el número permitido de caracteres para los teléfonos del contacto.');
                        $("#<%=TxtTelfCont.ClientID %>").css("border", "solid Red 1px");

                        $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                        bolValida = false;

                        MoveTabIndex(4);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%=TxtTelfCont.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=lblValidacion.ClientID %>").html('');
                    }

                    if (strMailCont.length != 0) {
                        if (strMailCont.indexOf('@', 0) == -1 || strMailCont.indexOf('.', 0) == -1) {
                            $("#<%=TxtMailCont.ClientID %>").focus();
                            $("#<%=lblValidacion.ClientID %>").html('El correo electrónico del contacto no es correcto.');
                            $("#<%=TxtMailCont.ClientID %>").css("border", "solid Red 1px");

                            $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                            bolValida = false;

                            MoveTabIndex(4);
                             $("#<%= lblValidacion.ClientID %>").show();
                             return false;
                        }
                        else {
                            $("#<%=TxtMailCont.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=lblValidacion.ClientID %>").html('');
                        }
                    }

                    if (strAñoVivDesde.length > 0) {
                        if (strAñoVivDesde.length < 4) {
                            $("#<%=txtAñoVivDesde.ClientID %>").focus();
                            $("#<%=lblValidacion.ClientID %>").html('El formato del año de residencia en el extranjero es incorrecto.');
                            $("#<%=txtAñoVivDesde.ClientID %>").css("border", "solid Red 1px");

                            $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                            bolValida = false;

                            MoveTabIndex(5);
                            $("#<%= lblValidacion.ClientID %>").show();
                            return false;
                        }
                        else {

                            if (parseInt(strAñoVivDesde) > curr_year) {
                                $("#<%=txtAñoVivDesde.ClientID %>").focus();
                                $("#<%=lblValidacion.ClientID %>").html('El año de residencia en el extranjero no puede ser mayor al año actual.');
                                $("#<%=txtAñoVivDesde.ClientID %>").css("border", "solid Red 1px");

                                $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                                bolValida = false;

                                MoveTabIndex(5);
                                $("#<%= lblValidacion.ClientID %>").show();
                                return false;
                            }
                            else if (parseInt(strAñoVivDesde) == curr_year) {

                                if (parseInt(strMesVivDesde) >= curr_month) {
                                    $("#<%=ddl_MesVivDesde.ClientID %>").focus();
                                    $("#<%=lblValidacion.ClientID %>").html('El mes de residencia en el extranjero no puede ser mayor al mes actual.');
                                    $("#<%=ddl_MesVivDesde.ClientID %>").css("border", "solid Red 1px");

                                    $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                                    bolValida = false;

                                    MoveTabIndex(5);
                                    $("#<%= lblValidacion.ClientID %>").show();
                                    return false;
                                }
                                else {
                                    $("#<%=ddl_MesVivDesde.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=lblValidacion.ClientID %>").html('');

                                }
                            }
                        }
                    }

                    if (strAñoRegreso.length > 0) {
                        if (strAñoRegreso.length < 4) {
                            $("#<%=txtAñoRegreso.ClientID %>").focus();
                            $("#<%=lblValidacion.ClientID %>").html('El formato del año regreso del extranjero es incorrecto.');
                            $("#<%=txtAñoRegreso.ClientID %>").css("border", "solid Red 1px");

                            $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                            $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                            bolValida = false;

                            MoveTabIndex(5);
                            $("#<%= lblValidacion.ClientID %>").show();
                            return false;
                        }
                        else {

                            if (parseInt(strAñoRegreso) < curr_year) {
                                $("#<%=txtAñoRegreso.ClientID %>").focus();
                                $("#<%=lblValidacion.ClientID %>").html('El año de regreso del extranjero puede ser menor al año actual.');
                                $("#<%=txtAñoRegreso.ClientID %>").css("border", "solid Red 1px");

                                $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                                $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                                bolValida = false;

                                MoveTabIndex(5);
                                $("#<%= lblValidacion.ClientID %>").show();
                                return false;
                            }
                            else if (parseInt(strAñoRegreso) == curr_year) {

                                if (parseInt(strMesRegreso) <= curr_month) {
                                    $("#<%=ddl_MesRegreso.ClientID %>").focus();
                                    $("#<%=lblValidacion.ClientID %>").html('El mes de regreso del extranjero no puede ser igual al mes actual.');
                                    $("#<%=ddl_MesRegreso.ClientID %>").css("border", "solid Red 1px");

                                    $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                                    bolValida = false;

                                    MoveTabIndex(5);
                                    $("#<%= lblValidacion.ClientID %>").show();
                                    return false;
                                }
                                else {
                                    $("#<%=ddl_MesRegreso.ClientID %>").css("border", "solid #888888 1px");
                                    $("#<%=lblValidacion.ClientID %>").html('');
                                }
                            }
                        }
                    }

                    if (parseInt(strAñoRegreso) <= parseInt(strAñoVivDesde)) {
                        $("#<%=txtAñoRegreso.ClientID %>").focus();
                        $("#<%=lblValidacion.ClientID %>").html('El año de regreso no puede ser menor o igual al año de residencia en el extranjero.');
                        $("#<%=txtAñoRegreso.ClientID %>").css("border", "solid Red 1px");

                        $("#<%=CmbNacRecurr.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipPers.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=CmbTipoDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNroDoc.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApeMat.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=txtApepCas.ClientID %>").css("border", "solid #888888 1px");
                        bolValida = false;

                        MoveTabIndex(5);
                        $("#<%= lblValidacion.ClientID %>").show();
                        return false;
                    }
                    else {
                        $("#<%=txtAñoRegreso.ClientID %>").css("border", "solid #888888 1px");
                        $("#<%=lblValidacion.ClientID %>").html('');
                    }
                }

            }



            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {

                $("#<%= lblValidacion.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
        }

        function soloCeros(texto) {

            for (i = 0; i < texto.length; i++) {
                if (texto[i] != '0')
                    return false;
            }

            return true;
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

            //retiramos el primer cero de la izquierda
            if (mmCumple.substr(0, 1) == 0) {
                mmCumple = mmCumple.substring(1, 2);
            }
            //retiramos el primer cero de la izquierda
            if (diaCumple.substr(0, 1) == 0) {
                diaCumple = diaCumple.substring(1, 2);
            }
            var edad = yyyyActual - yyyyCumple + 1;

            var key = '<%=ConfigurationManager.AppSettings["MayorEdad"].ToString() %>';

            if (key < edad) {
                return true;
            }
            else {
                return false;
            }
        }

        function IsValidDatosPestañaDocumentos() {

            var bolValida = true;

            var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());
            var strNacRecurrDesc = $.trim($("#<%= CmbNacRecurr.ClientID %> option:selected").text());

            var strTipoDoc = $.trim($("#<%= CmbTipoDoc.ClientID %>").val());
            var strTipoDocDesc = $.trim($("#<%= CmbTipoDoc.ClientID %> option:selected").text());

            var strNroDoc = $.trim($("#<%= txtNroDoc.ClientID %>").val());

            var strApePat = $.trim($("#<%= txtApePat.ClientID %>").val());
            var strApeMat = $.trim($("#<%= txtApeMat.ClientID %>").val());
            var strNombres = $.trim($("#<%= txtNombres.ClientID %>").val());

            var strTipoDocumentoM = $.trim($("#<%= ddl_TipoDocumentoM.ClientID %>").val());
            var strTipoDocumentoMDesc = $.trim($("#<%= ddl_TipoDocumentoM.ClientID %> option:selected").text());
            var strNroDocumentoM = $.trim($("#<%= txtNroDocumentoM.ClientID %>").val());
            var strFechaVcto = $.trim($('#<%= ctrFecVcto.ClientID %>').val());
            var strFechaExped = $.trim($('#<%= ctrFecExped.ClientID %>').val());
            var strFechaRenov = $.trim($('#<%= ctrFecRenov.ClientID %>').val());
            var strLugExp = $.trim($("#<%= txtLugExp.ClientID %>").val());


            var strDocumentoId = $.trim($("#<%= hidDocumento.ClientID %>").val());

            if (strNroDoc.length == 0) {
                $("#<%=txtNroDoc.ClientID %>").focus();
                $("#<%=txtNroDoc.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            } else {

                var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");

                if (valoresDocumento[2] == "True" && isNaN(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' solo permite números.");
                    bolValida = false;
                }

                else if (valoresDocumento[0] > strNroDoc.length || valoresDocumento[1] < strNroDoc.length) {

                    $("#<%= lblValidacion.ClientID %>").html(valoresDocumento[3]);
                    bolValida = false;
                }
                else if (valoresDocumento[4] != '' && valoresDocumento[4] != strNacRecurr) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' no aplica para el tipo de nacionalidad '" + strNacRecurrDesc + "'");
                    bolValida = false;

                }
                else if (soloCeros(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El número de documento es incorrecto");
                    bolValida = false;

                }



            }

            if (strApePat.length == 0) {
                $("#<%=txtApePat.ClientID %>").focus();
                $("#<%=txtApePat.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombres.length == 0) {
                $("#<%=txtNombres.ClientID %>").focus();
                $("#<%=txtNombres.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTipoDocumentoM == '0') {
                $("#<%=ddl_TipoDocumentoM.ClientID %>").focus();
                $("#<%=ddl_TipoDocumentoM.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_TipoDocumentoM.ClientID %>").css("border", "solid #888888 1px");

                if (strTipoDocumentoM.length == 0) {
                    if (ValidarTipoDocumento(strTipoDocumentoM) == false) {
                        $("#<%= lblValidacionDoc.ClientID %>").html('No se puede registrar dos veces un DNI o Libreta militar.');
                        $("#<%=ddl_TipoDocumentoM.ClientID %>").focus();
                        $("#<%=ddl_TipoDocumentoM.ClientID %>").css("border", "solid Red 1px");
                        bolValida = false;
                    }
                    else {
                        $("#<%=ddl_TipoDocumentoM.ClientID %>").css("border", "solid #888888 1px");
                    }
                }
            }

            if (strNroDocumentoM.length == 0) {
                $("#<%=txtNroDocumentoM.ClientID %>").focus();
                $("#<%=txtNroDocumentoM.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            } else {

                var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDocumentoM).split(",");

                if (valoresDocumento[2] == "True" && isNaN(strNroDocumentoM)) {
                    $("#<%= lblValidacionDoc.ClientID %>").html("El tipo de documento '" + strTipoDocumentoMDesc + "' solo permite números.");
                    bolValida = false;
                }

                else if (valoresDocumento[0] > strNroDocumentoM.length || valoresDocumento[1] < strNroDocumentoM.length) {

                    $("#<%= lblValidacionDoc.ClientID %>").html(valoresDocumento[3]);
                    bolValida = false;
                }
                else {
                    bolValida = true;
                }

            }

            if ((strFechaVcto.length != 0) && (strFechaExped.length != 0)) {
                if (ComparaFechas(strFechaVcto, strFechaExped) == false) {
                    $("#<%= lblValidacionDoc.ClientID %>").html('La fecha de vencimiento del documento siempre es superior a la fecha de expedición.');
                    $("#<%=ctrFecVcto.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ctrFecVcto.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if ((strFechaVcto.length != 0) && (strFechaRenov.length != 0)) {
                if (ComparaFechas(strFechaVcto, strFechaRenov) == false) {
                    $("#<%= lblValidacionDoc.ClientID %>").html('La fecha de vencimiento no puede ser menor o igual a la fecha de renovación.');
                    $("#<%=ctrFecVcto.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ctrFecVcto.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (bolValida) {
                $("#<%= lblValidacionDoc.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionDoc.ClientID %>").show();
            }

            return bolValida;
        }

        function IsValidFiliacion() {
            var bolValida = true;

            var strTipFiliacion = $.trim($("#<%= ddl_TipFiliacion.ClientID %>").val());
            var strDocumentoTipo = $.trim($("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val());
            var strNroDoc = $.trim($("#<%= txt_peid_vDocumentoNumero.ClientID %>").val());
            var strNacionalidad = $.trim($("#<%= ddl_pers_sNacionalidadId.ClientID %>").val());
            var strNombres = $.trim($("#<%= txt_pers_vNombres.ClientID %>").val());
            var strApePat = $.trim($("#<%= txt_pers_vApellidoPaterno.ClientID %>").val());
            
            if (strTipFiliacion == '0') {
                $("#<%=ddl_TipFiliacion.ClientID %>").focus();
                $("#<%=ddl_TipFiliacion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_TipFiliacion.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strDocumentoTipo == '0') {
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").focus();
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strNroDoc.length == 0) {
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").focus();
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid #888888 1px");
            }
             if (strNacionalidad == '0') {
                $("#<%=ddl_pers_sNacionalidadId.ClientID %>").focus();
                $("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strNombres.length == 0) {
                $("#<%=txt_pers_vNombres.ClientID %>").focus();
                $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strApePat.length == 0) {
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").focus();
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid #888888 1px");
            }
            if (bolValida) {
                $("#<%= lblValidacionFiliacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionFiliacion.ClientID %>").show();
            }
            return bolValida;
        }

        function IsValidDirecciones() {

            var bolValida = true;

            var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());
            var strNacRecurrDesc = $.trim($("#<%= CmbNacRecurr.ClientID %> option:selected").text());

            var strTipoDoc = $.trim($("#<%= CmbTipoDoc.ClientID %>").val());
            var strTipoDocDesc = $.trim($("#<%= CmbTipoDoc.ClientID %> option:selected").text());

            var strNroDoc = $.trim($("#<%= txtNroDoc.ClientID %>").val());
            var strApePat = $.trim($("#<%= txtApePat.ClientID %>").val());
            var strApeMat = $.trim($("#<%= txtApeMat.ClientID %>").val());
            var strNombres = $.trim($("#<%= txtNombres.ClientID %>").val());

            var strDireccion = $.trim($("#<%= TxtDirDir.ClientID %>").val());
            var strTipRes = $("#<%= CmbTipRes.ClientID %>").val();
            var strDptoCont = $("#<%= CmbDptoContDir.ClientID %>").val();
            var strProvPais = $.trim($("#<%= CmbProvPaisDir.ClientID %>").val());
            var strDistCiudad = $.trim($("#<%= CmbDistCiuDir.ClientID %>").val());
            //var strTelfsDir = $.trim($("#<%= TxtTelfDir.ClientID %>").val());
            var strCodPost = $.trim($("#<%= txtCodPost.ClientID %>").val());

            if (strNroDoc.length == 0) {
                $("#<%=txtNroDoc.ClientID %>").focus();
                $("#<%=txtNroDoc.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            } else {


                var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");

                if (valoresDocumento[2] == "True" && isNaN(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' solo permite números.");
                    bolValida = false;
                }

                else if (valoresDocumento[0] > strNroDoc.length || valoresDocumento[1] < strNroDoc.length) {

                    $("#<%= lblValidacion.ClientID %>").html(valoresDocumento[3]);
                    bolValida = false;
                }
                else if (valoresDocumento[4] != '' && valoresDocumento[4] != strNacRecurr) {
                    $("#<%= lblValidacion.ClientID %>").html("El tipo de documento '" + strTipoDocDesc + "' no aplica para el tipo de nacionalidad '" + strNacRecurrDesc + "'");
                    bolValida = false;

                }
                else if (soloCeros(strNroDoc)) {
                    $("#<%= lblValidacion.ClientID %>").html("El número de documento es incorrecto");
                    bolValida = false;

                }


            }

            if (strApePat.length == 0) {
                $("#<%=txtApePat.ClientID %>").focus();
                $("#<%=txtApePat.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtApePat.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombres.length == 0) {
                $("#<%=txtNombres.ClientID %>").focus();
                $("#<%=txtNombres.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombres.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDireccion.length == 0) {
                $("#<%=TxtDirDir.ClientID %>").focus();
                $("#<%=TxtDirDir.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=TxtDirDir.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTipRes == '0') {
                $("#<%=CmbTipRes.ClientID %>").focus();
                $("#<%=CmbTipRes.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=CmbTipRes.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDptoCont == '0') {
                $("#<%=CmbDptoContDir.ClientID %>").focus();
                $("#<%=CmbDptoContDir.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=CmbDptoContDir.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strProvPais == '0') {
                $("#<%=CmbProvPaisDir.ClientID %>").focus();
                $("#<%=CmbProvPaisDir.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=CmbProvPaisDir.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDistCiudad == '0') {
                $("#<%=CmbDistCiuDir.ClientID %>").focus();
                $("#<%=CmbDistCiuDir.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=CmbDistCiuDir.ClientID %>").css("border", "solid #888888 1px");
            }

            //            if (strTelfsDir.length == 0) {
            //                $("#<%=TxtTelfDir.ClientID %>").focus();
            //                $("#<%=TxtTelfDir.ClientID %>").css("border", "solid Red 1px");
            //                bolValida = false;
            //            }
            //            else {
            //                $("#<%=TxtTelfDir.ClientID %>").css("border", "solid #888888 1px");
            //            }

            //            if (strCodPost.length == 0) {
            //                $("#<%=txtCodPost.ClientID %>").focus();
            //                $("#<%=txtCodPost.ClientID %>").css("border", "solid Red 1px");
            //                bolValida = false;
            //            }
            //            else {
            //                $("#<%=txtCodPost.ClientID %>").css("border", "solid #888888 1px");
            //            }

            if (bolValida) {
                $("#<%= lblValidacionDir.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionDir.ClientID %>").show();
            }

            return bolValida;
        }

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

        function ValidarTipoDocumento(strTipDocumento) {

            var valor;

            var rprta = true;

            if (strTipDocumento == '1' || strTipDocumento == '2') {

                $("#<%= Grd_Documentos.ClientID %> tbody tr").each(function (index) {

                    //1 - DNI
                    //2 - LM/BOL                    

                    $(this).children("td").each(function (index2) {

                        if (index2 == 1) {

                            valor = $(this).text();

                            if (valor == strTipDocumento) {
                                rprta = false;
                            }
                        }
                    })
                })
            }

            return rprta;
        }


        function ComparaFechas(fecha, fecha2) {
            xMonth = parseInt(ObtenerNroMes(fecha.substring(0, 3)));
            xDay = parseInt(fecha.substring(4, 6));
            xYear = parseInt(fecha.substring(7, 11));

            yMonth = parseInt(ObtenerNroMes(fecha2.substring(0, 3)));
            yDay = parseInt(fecha2.substring(4, 6));
            yYear = parseInt(fecha2.substring(7, 11));

            if (xYear > yYear) {
                return true
            }
            else {

                if (xYear == yYear) {

                    if (xMonth > yMonth) {
                        return true
                    }
                    else {

                        if (xMonth == yMonth) {

                            if (xDay > yDay) {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    return false;
                }
            }
        }

        function ObtenerNroMes(strMes) {

            switch (strMes) {
                case 'ene': return 1;
                case 'feb': return 2;
                case 'mar': return 3;
                case 'abr': return 4;
                case 'may': return 5;
                case 'jun': return 6;
                case 'jul': return 7;
                case 'ago': return 8;
                case 'sep': return 9;
                case 'oct': return 10;
                case 'nov': return 11;
                case 'dic': return 12;
            }
        }

        function fechaValida(strDate) {

            if ((strDate.Length == 0) || (strDate == "__/__/____")) {
                return true;
            }

            var d = new Date();
            var curr_year = d.getFullYear();
            var intDateAnio, intDateMes, intDateDia;

            var fecha = new Date(strDate);

            intDateMes = fecha.getMonth() + 1
            intDateAnio = ((fecha.getFullYear()).toString()).substring(1, 5);
            intDateDia = fecha.getDate()

            if (intDateAnio <= 1900) {
                return false;
            }

            if (intDateMes < 1 || intDateMes > 12) {
                return false;
            }

            if (intDateMes == 1 || intDateMes == 3 || intDateMes == 5 || intDateMes == 7 || intDateMes == 8 || intDateMes == 10 || intDateMes == 12) {
                if (intDateDia < 1 || intDateDia > 31) {
                    return false;
                }
            }

            if (intDateMes == 4 || intDateMes == 6 || intDateMes == 9 || intDateMes == 11) {
                if (intDateDia < 1 || intDateDia > 30) {
                    return false;
                }
            }

            if (intDateMes == 2) {
                if (intDateAnio % 4 == 0 && (intDateAnio % 100 != 0 || intDateAnio % 400 == 0) == false) {
                    if (intDateDia < 1 || intDateDia > 28) {
                        return false;
                    }
                }
                else {
                    if (intDateDia < 1 || intDateDia > 29) {
                        return false;
                    }
                }
            }
        }

        function fechaNacimientoValida(strDate) {

            if ((strDate.Length == 0) || (strDate == "__/__/____")) {
                return true;
            }

            var d = new Date();
            var curr_year = d.getFullYear();
            var curr_month = d.getMonth() + 1;
            var curr_day = d.getDate();
            var intDateAnio, intDateMes, intDateDia;

            var fecha = new Date(strDate);

            intDateMes = fecha.getMonth() + 1
            intDateAnio = ((fecha.getFullYear()).toString()).substring(1, 5);
            intDateDia = fecha.getDate()

            if (intDateAnio <= 1900 || intDateAnio > curr_year) {
                return false;
            }

            if (intDateAnio == curr_year && intDateMes > curr_month) {
                return false;
            }

            if (intDateAnio == curr_year && intDateMes == curr_month && intDateDia > curr_day) {
                return false;
            }

            return true;
        }

        function Ejecuar_Script() {
            var edad = calcularEdad();

            $("#<%=LblEdad2.ClientID %>").val(edad);
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

        function calcularEdad() {

            var fecha = document.getElementById('<%= ctrFecNac.FindControl("TxtFecha").ClientID %>').value;

            if (fecha.length == 0) {
                return "";
            }


            var fechaActual = new Date()
            var diaActual = fechaActual.getDate();
            var mmActual = fechaActual.getMonth() + 1;
            var yyyyActual = fechaActual.getFullYear();
            FechaNac = fecha.split("-");
            var diaCumple = FechaNac[1];
            var mmCumple = Search_Number_Mes(FechaNac[0]);
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

        function convertDate(inputFormat) {
            function pad(s) { return (s < 10) ? '0' + s : s; }
            var d = new Date(inputFormat);
            return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
        }

        function formattedDate(date) {
            var d = new Date(date || Date.now()),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [month, day, year].join('/');
        }

        function ValidarCheckRetornar(form) {
            var ValorChecked = document.getElementById('<%= RdioSiRetornExt.ClientID %>').checked;

            if (ValorChecked == true) {
                document.getElementById('<%= txtAñoRegreso.ClientID %>').disabled = false;
                document.getElementById('<%= ddl_MesRegreso.ClientID %>').disabled = false;
                document.getElementById('<%= RdioNoRetornExt.ClientID %>').checked = false;
            }
            else {
                document.getElementById('<%= txtAñoRegreso.ClientID %>').disabled = true;
                document.getElementById('<%= ddl_MesRegreso.ClientID %>').disabled = true;

                document.getElementById('<%= txtAñoRegreso.ClientID %>').value = '';
                document.getElementById('<%= ddl_MesRegreso.ClientID %>').value = '';
            }

            ValorChecked = document.getElementById('<%= RdioSiBenExter.ClientID %>').checked;

            if (ValorChecked == true) {
                document.getElementById('<%= TxtAcuerConv.ClientID %>').disabled = false;
            }
            else {
                document.getElementById('<%= TxtAcuerConv.ClientID %>').disabled = true;
                document.getElementById('<%= TxtAcuerConv.ClientID %>').value = '';
            }
        }

        // Función que valida : Nombres y Apellidos
        function validarSoloLetras(lbl, txt) {

            var valido = true;
            valido = validarcaracterrune();
            if (valido == true) {
                var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ]");

                if (txt.value.length > 0 && charpos >= 0) {
                    lbl.style.display = 'inline';
                    txt.value = '';
                }
                else {
                    lbl.style.display = 'none';
                }
            }

            return valido;
        }



        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }


        function isValidarNroDocumento() {
            var strNacRecurr = $.trim($("#<%= CmbNacRecurr.ClientID %>").val());
            var strTipPers = $.trim($("#<%= CmbTipPers.ClientID %>").val());
            var strTipoDoc = $.trim($("#<%= CmbTipoDoc.ClientID %>").val());
            var strNroDoc = $.trim($("#<%= txtNroDoc.ClientID %>").val());



            if (strTipPers == '2101') {
                if (strNacRecurr != '2052') {
                    if (strTipoDoc == '1') {
                        if ((event.keyCode < 48) || (event.keyCode > 57))
                            event.returnValue = false;
                    }
                    else {

                        if (event.keyCode > 1 && event.keyCode < 4) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 8) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 32) {
                            event.returnValue = true;
                        }
                        if (event.keyCode >= 58 && event.keyCode <= 59) {
                            event.returnValue = true;
                        }
                        if (event.keyCode >= 46 && event.keyCode <= 60) {
                            event.returnValue = true;
                        }
                        if (event.keyCode > 63 && event.keyCode < 91) {
                            event.returnValue = true;
                        }
                        if (event.keyCode > 94 && event.keyCode < 123) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 130) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 144) {
                            event.returnValue = true;
                        }
                        if (event.keyCode > 159 && event.keyCode < 164) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 181) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 214) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 224) {
                            event.returnValue = true;
                        }
                        if (event.keyCode == 233) {
                            event.returnValue = true;
                        }

                        var letras = "aeiouAEIOU";
                        var tecla = String.fromCharCode(event.keyCode);
                        var n = letras.indexOf(tecla);
                        if (n > -1) {
                            event.returnValue = true;
                        }
                    }


                }

            }

        }



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
            if (charcode == 262) {
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
        function abrirModalIdioma() {
            document.getElementById('ModalIdioma').style.display = 'block';
        }
        function CerrarModalIdioma() {
            document.getElementById('ModalIdioma').style.display = 'none';
        }


        function ValidarCamposRUNE(evt) {
            var bolValida = ValidarSujeto(evt);
            if (!bolValida) {
                bolValida = validarcaracterrune();
            }
            return bolValida;
        }
        function esDigito(sChr) {
            var sCod = sChr.charCodeAt(0);
            return ((sCod > 47) && (sCod < 58));
        }

        function valNumero() {
            var bOk = true;

            var CmbTipoDoc = $("#<%=CmbTipoDoc.ClientID %>").val();
            var bol = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(CmbTipoDoc).split(",");

            document.getElementById("<%=txtNroDoc.ClientID %>").maxLength = valoresDocumento[1];
            if (valoresDocumento[2] == "True") {
                if ($("#<%=txtNroDoc.ClientID %>").val() != "") {
                    var nNro = $("#<%=txtNroDoc.ClientID %>").val()
                    
                    for (var i = 0; i < nNro.length; i++) {
                        bOk = bOk && esDigito(nNro.charAt(i));
                    }
                }
                if (!bOk) {                    
                    $("#<%=txtNroDoc.ClientID %>").val("");
                    $("#<%=txtNroDoc.ClientID %>").focus();
                }
            }
        }


        function allValidChars(texto) {
            var campo = texto.value;
            var parsed = true;
            var validchars = "abcdefghijklmnopqrstuvwxyzáéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ ";
            for (var i = 0; i < campo.length; i++) {
                var letter = campo.charAt(i).toLowerCase();
                if (validchars.indexOf(letter) != -1)
                    continue;
                parsed = false;
                break;
            }
            return parsed;
        }

        function validarOnBlurCaracteresRUNE(texto) {
            var campo = texto.value;
            //&#262; es la letra C con acentuación aguda.

            var validrune = $("#<%= HFValidarTextoRune.ClientID %>").val();
            var validchars = "abcdefghijklmnopqrstuvwxyzáéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ " + validrune;            

            var parsed = true;
            for (var i = 0; i < campo.length; i++) {
                var letter = campo.charAt(i).toLowerCase();
                if (validchars.indexOf(letter) != -1 || campo.indexOf(String.fromCharCode(262)) > -1)
                    continue;
                parsed = false;
                break;
            }
            return parsed;
        }

        function validarCampos(texto) {
            var bolValida = validarOnBlurCaracteresRUNE(texto);
                
            if (bolValida == false)
            { texto.value = ""; } 
        }
        function editoDNI() {
            if ($("#<%=CmbTipoDoc.ClientID %>").val() == "1") {
                $("#<%=txtApePat.ClientID %>").val("");
                $("#<%=txtApeMat.ClientID %>").val("");
                $("#<%=txtNombres.ClientID %>").val("");
                $("#<%=txtValidacionReniec.ClientID %>").val("0");
                $("#<%=txtDireccionResidencia.ClientID %>").val("");
            }

        }
        function disableBtn(btnID, newText) {
            Page_IsValid = null;
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            var btn = document.getElementById(btnID);
            var isValidationOk = Page_IsValid;

            if (isValidationOk !== null) {
                if (isValidationOk) {
                    btn.disabled = true;
                    btn.value = newText;
                }
                else {
                    btn.disabled = false;
                    btnFilter.disabled = false;
                }
            }
            else {
                //setTimeout("setImage('" + btnID + "')", 10);
                btn.disabled = true;
                btn.value = newText;
            }
        }

    </script>
</asp:Content>
