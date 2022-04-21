<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="FrmActoNotarialProtocolares.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActoNotarialProtocolares" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar" TagPrefix="uc2" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlUploader.ascx" TagName="ctrlUploader" TagPrefix="uc2" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagName="toolbarcontent" TagPrefix="toolbar" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlReimprimirbtn.ascx" TagName="ctrlReimprimirbtn" TagPrefix="uc4" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlBajaAutoadhesivo.ascx" TagName="ctrlBajaAutoadhesivo" TagPrefix="uc6" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Validacion/Text.js" type="text/javascript"></script>
    <script src="../Scripts/tinymce/tinymce.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
             
    <style type="text/css">
        .hideControl
        {
	        display: none;
        }
        .ddlRepresentantes
        {
            width: 404px;
        } 
        .style5
        {
            width: 130px;
            height: 30px;
        }
        .style6
        {
            height: 30px;
        }
        .style7
        {
            width: 25px;
            height: 30px;
        }
        .style12{
            width: 136px;
        }
        .disableElementsOfDiv {
            pointer-events: none;
            opacity: 0.4;
        }
        .tableHoverable tr:hover {background-color: #f8f8f8;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Script by hscripts.com -->
    <script type="text/javascript">
        var r = { 'special': /[\W]/g }
        function valid(o, w) {
            o.value = o.value.replace(r[w], '');
        }

    </script>
    
    <script type="text/javascript">    
        //------------------------------------------------------------------------
        // No remover esta linea sino no realiza la busqueda cuando 
        // se ingresa un nuevo participante y luego uno existente.
        //------------------------------------------------------------------------          
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        //------------------------------------------------------------------------
                 

        $(document).ready(function () {
            Load();

            $("#<%=txt_acno_Numero_Minuta.ClientID %>").numeric();
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").numeric();
            $("#<%=txtNroOficio.ClientID %>").numeric();
            $("#<%=txtNroFojaIni.ClientID %>").numeric();
            $("#<%=txtNroFojaFinal.ClientID %>").numeric();
            $("#<%=txtNroEscritura.ClientID %>").numeric();           
            $("#<%=txtNumeroOficioAdi.ClientID %>").numeric();

            $('#<%=chkIncapacitado.ClientID %>').hide();
            $('#<%=txtRegistroTipoIncapacidad.ClientID %>').hide();
            
            $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
            $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');

            $('#<%=lblIncapacitadoTitulo.ClientID %>').hide();
            $('#<%=lblIncapacidadTitulo.ClientID %>').hide();
            
            $('#<%=lblParticipanteDiscapacidad.ClientID %>').hide();
            $('#<%=ddl_Participante_Discapacidad.ClientID %>').hide();

             OcularListaInterpretes();
            //$("#btnSave_tab3").prop("disabled", "disabled");
            //$("#<%=Btn_VistaPreviaAprobar.ClientID %>").attr('disabled', true);
            $("#<%= Btn_VistaPreviaAprobar.ClientID %>").prop('disabled', true);
            MostrarOcultarTablaJuridica();

        });
        function setearTextoNormativo(){
                var insertos="";
                var insertosAux="";
                var ids="";
                $('#<%= divListNormativo.ClientID %>').children('input').each(function (e,c) {
                     if (this.checked) 
                     {
                        var id=this.id;
                        id=id.replace("check","");
                        ids=ids+id+"-";
                     }
                });
                var cuerpo={};
                cuerpo.ids=ids;
                var sndprm = {};
                sndprm.cuerpo = JSON.stringify(cuerpo);
                $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/obtenerTextoNormativo",
                data: JSON.stringify(sndprm),
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (rsp) {
                    if (rsp != null) {
                        
                        var data = JSON.parse(rsp);
                        var json = JSON.parse(data.d);
                        for(var i=0;i<json.length;i++){
                            var texto=json[i].arle_vDescripcionLarga.toUpperCase();
                            insertos=insertos +"<P>INSERTO: "+texto.substring(3,texto.length) ;
                            insertosAux=insertosAux +"INSERTO: "+json[i].arle_vDescripcionLarga_aux.toUpperCase()+"\n";
                        }
                        $('#<%=txtTextoNormativo.ClientID %>').val(insertos);
                        $('#<%=txtTextoNormativo_.ClientID %>').val(insertosAux);
                    }
                    else {
                        if (oResponse.Identity == 0 && oResponse.Message != null) {
                            showdialog('e', 'Escritura Pública', oResponse.Message.toString(), false, 160, 300);
                        }
                    }
                },
                error: errores
            });
                
                
                

            
        }

        function validarAnioEscritura()
        {
            var anioActual = new Date().getFullYear();
            var strAnioEscrituraPri = $.trim($("#<%= txtAnioEscrituraPri.ClientID %>").val());
            if (strAnioEscrituraPri > anioActual) {
                alert("El año de la escritura no puede ser mayor al año actual."); 
                $("#<%= txtAnioEscrituraPri.ClientID %>").focus();
                $("#<%= txtAnioEscrituraPri.ClientID %>").val('');
                return false;
            }
            if (strAnioEscrituraPri < 1900) {
                alert("El año de la escritura no puede ser menor al año 1900."); 
                $("#<%= txtAnioEscrituraPri.ClientID %>").focus();
                $("#<%= txtAnioEscrituraPri.ClientID %>").val('');
                return false;
            }

            return true;
        }

        function ValidarDatosVinculacion() {
            var bolValida = true; 
            var valCodigoAutoadhesivo = $.trim($("#<%= txtCodigoInsumo.ClientID %>").val());
            var valTipoFormato = $("#<%= hdn_tipo_formato_proto.ClientID %>").val();

            $("#<%=txtCodigoInsumo.ClientID %>").css("border", "solid #888888 1px");


            if (valCodigoAutoadhesivo.length == 0) {
                $("#<%= txtCodigoInsumo.ClientID %>").focus();
                $("#<%= txtCodigoInsumo.ClientID %>").css("border", "solid Red 1px");
                bolValida = false; 
            }
            // 8142 -> PARTE    8143 -> TESTIMONIO
            if (valTipoFormato == "8142" || valTipoFormato == "8143") {
                if (valTipoFormato == "8142") 
                {
                    var valNumOficio = $.trim($("#<%= txtNumeroOficioAdi.ClientID %>").val());
                    if (valNumOficio.length == 0) {
                        $("#<%= txtNumeroOficioAdi.ClientID %>").focus();
                        $("#<%= txtNumeroOficioAdi.ClientID %>").css("border", "solid Red 1px");
                        bolValida = false; 
                    }
                    else {
                        $("#<%=txtNumeroOficioAdi.ClientID %>").css("border", "solid #888888 1px");        
                    }
                }
                var validaFuncionarioResponsable = $("#<%= HF_ValidaTablaFuncionarioResponsable.ClientID %>").val();
                if (validaFuncionarioResponsable == "S")
                {
                    var funcionarioResponsable = $("#<%=ddlFuncionarioResponsable.ClientID %> ").val();
                    if (funcionarioResponsable == "0")
                    {
                        $("#<%=ddlFuncionarioResponsable.ClientID %> ").focus();
                        $("#<%=ddlFuncionarioResponsable.ClientID %> ").css("border", "solid Red 1px");
                        bolValida = false;
                    }
                    else
                    {
                      $("#<%=ddlFuncionarioResponsable.ClientID %> ").css("border", "solid #888888 1px");  
                    }
                }
            }
            return bolValida;
        }

        function Load() {           
         
            $("#MainContent_ctrFechaExpedicionPri_TxtFecha").attr('readOnly', "readOnly");

            $("#tabs").tabs({

                activate: function () {
                    var selectedTab = $('#tabs').tabs('option', 'active');

                    var CONST_SESION_ACTUACION_ID = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();

                    $("#<%= hdn_SelectedTab.ClientID %>").val(selectedTab);

                    switch (selectedTab) {

                        case 0: // TAB REGISTRO - (FRANK SIMON)
                            tab_01();
                         
                            break;

                        case 1: // TAB PARTICIPANTES - (FRANK SIMON) [MODIFICADO - SANDRO GUINET]
                            Layout_TipoPersona();

                            $('#divRepresentantesLegales').hide();

                            if ($("#<%=hdn_actu_iPersonaRecurrenteId.ClientID %>").val() == "0") {

                                var tipopersona = $("#<%=ddl_pers_sPersonaTipoId.ClientID %> ").val();

                                switch (tipopersona) {

                                    case "2101": //Natural
                                        var TipoDocumento_N = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
                                        var NumeroDocumento_N = $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val();

                                        if (TipoDocumento_N != '0' && NumeroDocumento_N.trim() != '') {
                                           
                                            tab_02_PersonaNaturalBuscar("N");
                                        }

                                        break;

                                    case "2102": //Juridica
                                        var TipoDocumento = $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val();
                                        var NumeroDocumento = $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val();

                                        if (TipoDocumento != '0' && NumeroDocumento.trim() != '') {
                                            tab_02_PersonaJuridicaBuscar();
                                        }

                                        break;
                                }
                            }

                            break;

                        case 2: // TAB CUERPO (SANDRO GUINET)      
                            var chk_Activo = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');

                            if (!chk_Activo) {
                                $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', true);
                                $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', true);

                                $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', true);
                                $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', true);

                                $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', true);
                                $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', true);

                            } else {
                                $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', false);
                                $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', false);
                                $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', false);
                                $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', false);

                                $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', false);
                                $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', false);
                            }

                            break;

                        case 3: // TAB PAGOS                             
                          
                            break;

                        case 4: // TAB VINCULACIÓN                             

                            //disableElements($('#tabs-6').children());
                            break;

                        case 5: // TAB DIGITALIZACIÓN DE ARCHIVOS                         

                            tab_06();

                            break;
                    }
                },

                active: document.getElementById('<%= hdn_SelectedTab.ClientID%>').value

            });


            $("#accordion").accordion({
                heightStyle: "content",
            });
            loadtynyec();

            $("#mostrar").click(function () {
                tinyMCE.activeEditor.setContent('<span>some</span> html');
            });
            //--------------------------------------------------------------


            $("#<%=Cmb_TipoActoNotarial.ClientID %>").change(function () {
                var tipoActoProtocolar = $('#<%=Cmb_TipoActoNotarial.ClientID %>').val();   
               
                     
                $("#<%= HF_ACTONOTARIAL_POR_TARIFA.ClientID%>").val(tipoActoProtocolar);     
                var texto = $('#<%=Cmb_TipoActoNotarial.ClientID %> :selected').text();             
                var strTipoActoProtocolarCompraVenta = $("#<%= HF_TIPOACTO_COMPRA_VENTA.ClientID %>").val();
                
                if(tipoActoProtocolar!=0){
                    if(strTipoActoProtocolarCompraVenta==tipoActoProtocolar){
                        $('#<%=chk_acno_bFlagMinuta.ClientID %>').prop("checked", "checked");  
                        $('#<%=chk_acno_bFlagMinuta.ClientID %>').attr('disabled', true);  
                    }
                    else{
                       $('#<%=chk_acno_bFlagMinuta.ClientID %>').prop("checked", "");      
                       $('#<%=chk_acno_bFlagMinuta.ClientID %>').attr('disabled', false);          
                    }
                    }
                    else{
                    texto = "";
                    }
                
                SetLabelActoNotarial(texto);
              
//                if(tipoActoProtocolar==strTipoActoProtocolarCompraVenta){
//                    $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %> option[value='8417']").remove();
//                }
                
            });

            $("#<%=ddlFuncionario.ClientID %>").change(function () {   
             
                var iFuncionario = $('#<%=ddlFuncionario.ClientID %>').val();            
                $("#<%=hdn_FuncionarioId.ClientID %>").val(iFuncionario);
                
            });



               $("#<%=ddl_Participante_Discapacidad.ClientID %>").change(function () {
                    $("#<%=HF_IREFERENCIAPARTICIPANTE.ClientID %>").val($("#<%=ddl_Participante_Discapacidad.ClientID %>").val());
               });

               $("#<%=ddl_Participante_Interprete.ClientID %>").change(function () {
                    $("#<%=HF_IREFERENCIAINTERPRETE.ClientID %>").val($("#<%=ddl_Participante_Interprete.ClientID %>").val());
               });
            
               $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").change(function () {
                    if ($("#<%=ddl_pers_sEstadoCivilId.ClientID %>").val() != "0")
                    {
                         $("#<%=ddlPaisOrigen.ClientID %>").focus();
                    }
               });

            $("#<%=ddl_pers_sPersonaTipoId.ClientID %>").change(function () {
                
                Layout_TipoPersona();
               
            });

            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").bind("keypress", function (e) {
                
                if (e.keyCode == 13) {
                        tab_02_PersonaNaturalBuscar("N");
                        e.preventDefault();
                        
                }
            });
            
            

            $("#<%=txt_empr_vNumeroDocumento.ClientID %>").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                     tab_02_PersonaJuridicaBuscar();
                    e.preventDefault();
                }
            });

   
     $("#<%=btn_confirmar.ClientID %>").on('click', function (e) {
         prm = {};
         prm.intTipoActa =  $("#<%=HF_FORMATO_ACTIVO.ClientID %>").val();
         prm.strNOMBRE_RECURRENTE = $("#<%=HF_NOMBRE_RECURRENTE.ClientID %>").val();
         prm.strDOCUMENTO= $("#<%=HF_DOCUMENTO.ClientID %>").val();
         prm.strTipoFormato= $("#<%=hdn_tipo_formato_proto.ClientID %>").val();
         
         var totalRow = $("#<%=gdvVinculacion.ClientID %> tr").length - 1;
         
        var myControlId = '<%=gdvVinculacion.ClientID%>';

        var valores = new Array();
        i=0;
        var textoEP = "ESCRITURA";
        var textoParte = "PARTE";
        var textoTestimonio = "TESTIMONIO";
        var textoActaConformidad = "";

        $("#<%=gdvVinculacion.ClientID %> tr").each(function() {
            //Skip first(header) row
            if (!this.rowIndex) return;
                valores[i] =$(this).find("td:eq(4)").html();                              
                i++;
        });

        var i = 0;
        var bEP = false;
        var bParte = false;
        var bTestimonio = false;

        for (i=0;i<totalRow;i++) { 
            if (valores[i] == textoEP)
            {
               bEP = true;
            }
            if (valores[i] == textoParte)
            {
               bParte = true;
            }
            if (valores[i] == textoTestimonio)
            {
               bTestimonio = true;
            }
        }

        if (bEP == true && bParte == false && bTestimonio == false)
        {
            textoActaConformidad = "ESCRITURA PÚBLICA";
        }
        if (bEP == true && bParte == true && bTestimonio == false)
        {
            textoActaConformidad = "ESCRITURA PÚBLICA Y PARTE";
        }
        if (bEP == true && bParte == true && bTestimonio == true)
        {
            textoActaConformidad = "ESCRITURA PÚBLICA, PARTE y TESTIMONIO";
        }

        prm.strtituloActaConformidad = textoActaConformidad;     

        $.ajax({
            type: "POST",
            url: "FrmActoNotarialProtocolares.aspx/ImprimirActaConformidad",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(prm),
            dataType: "json",
            success: function (data) {

                if (data.d == true) {
                    var strUrl = "../Accesorios/VisorPDF.aspx";
                    window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                } else {
                    showdialog('a', 'Registro Notarial : Cuerpo', 'EL FORMATO ESTA EN DESARROLLO.', false, 160, 300);
                }

            },
            failure: function (response) {
                showdialog('e', 'Registro Notarial : Cuerpo',response.d, false, 160, 300);
            }
        });
        e.preventDefault();

    });
    
 

            $("#<%=btnSave_tab6.ClientID %>").on('click', function (e) {
             
                SaveDocumentoDigitalizado();
                e.preventDefault();
             
            });
            MostrarOcultarTablaJuridica();
        }

        function loadtynyec() {
            tinyMCE.remove();
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
                language: "es"
            });

        }            
        
 function ObtenerElementosGenero() {
          var comboGenero = document.getElementById('<%= ddl_pers_sGeneroId.ClientID %>');
          var comboEstadoCivil = document.getElementById('<%= ddl_pers_sEstadoCivilId.ClientID %>');
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
          var largo = 0;
          for (i = 1; i < comboEstadoCivil.length; i++) {
              largo = comboEstadoCivil.options[i].text.length;
              comboEstadoCivil.options[i].text = comboEstadoCivil.options[i].text.substring(0, largo - 1) + letra;
          }

          for (i = 1; i < comboNacionalidad.length; i++) {
              largo = comboNacionalidad.options[i].text.length;
              comboNacionalidad.options[i].text = comboNacionalidad.options[i].text.substring(0, largo - 1) + letra;
          }
      }

      function limpiarFechaOtorgantes()
      {
          $("#<%= HFOtorganteId.ClientID %>").val("");
          $("#<%= lblNombresOtorgante.ClientID %>").html("Nombres del Otorgante");
          $("#<%=btnActualizarFechaOtorgante.ClientID %>").attr('disabled', true); 
          $('#<%= ctrFechaSuscripcion.FindControl("TxtFecha").ClientID %>').val('');      
      }
        function ActivarOtrodocumento()
        {
            var strDocumentoTipoId = $.trim($("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val());

            if (strDocumentoTipoId == "4") {
                $("#<%= tablaOtroDocumento.ClientID %>").css("display","block");
	            $("#<%=txtDescOtroDocumento.ClientID %>").attr('disabled', false);
            }
            else
            {	            
                $("#<%= tablaOtroDocumento.ClientID %>").css("display", "none");
	            $("#<%=txtDescOtroDocumento.ClientID %>").attr('disabled', true);
            }
             $("#<%= txt_peid_vDocumentoNumero.ClientID %>").focus();
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
<script type="text/javascript">
    function MostrarOcultarTablaJuridica(){        
        var tipopersona = $("#<%=ddl_pers_sPersonaTipoId.ClientID %> ").val();
        if (tipopersona == "2101") { // Natural
            $('#tablaJuridica').hide();
        } else {
            if (tipopersona == "2102") { // Jurídica
                $('#tablaJuridica').show();
            } else {
                $('#tablaJuridica').hide();
            }
        }
    }
    function ActivaFindOficina(controlID) {
        var input = document.getElementById(controlID);

        if (input.style.visibility == 'visible') {
            input.style.visibility = 'hidden';
        }
        else {
            input.style.visibility = 'visible';
            input.focus();
        }
    }
    function BusquedaOficinaRegistral() {

        var input = $.trim($("#<%= txtFindOficinaRegistral.ClientID %>").val()).toUpperCase();

        var output = document.getElementById("<%= ddl_OficinaRegistralRegistrador.ClientID %>").options;

        for (var i = 0; i < output.length; i++) {

            if (output[i].text.toUpperCase().indexOf(input) > -1) {
                output[i].selected = true;
            }
            if (input == '') {
                output[0].selected = true;
            }
        }
    }

    function BusquedaOficinaConsularPri() {

        var input = $.trim($("#<%= txtFindOficinaConsularPri.ClientID %>").val()).toUpperCase();

        var output = document.getElementById("<%= ddlOficinaConsularPri.ClientID %>").options;

        for (var i = 0; i < output.length; i++) {

            if (output[i].text.toUpperCase().indexOf(input) > -1) {
                output[i].selected = true;
            }
            if (input == '') {
                output[0].selected = true;
            }
        }
    }

    function ValidarTabBusquedaPrimigenia() {
        var bolValida = true;

        var strAnioEscrituraPri = $.trim($("#<%= txtAnioEscrituraPri.ClientID %>").val());
        var strNumeroEscrituraPublicaPri = $.trim($("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val());
        var strOficinaConsularPri = $.trim($("#<%= ddlOficinaConsularPri.ClientID %>").val());

        if (strAnioEscrituraPri.length == 0) {
            $("#<%=txtAnioEscrituraPri.ClientID %>").focus();
            $("#<%=txtAnioEscrituraPri.ClientID %>").css("border", "solid Red 1px");
            bolValida = false;
        }
        else {
            $("#<%=txtAnioEscrituraPri.ClientID %>").css("border", "solid #888888 1px");
        }
        if (strNumeroEscrituraPublicaPri.length == 0) {
            $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").focus();
            $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid Red 1px");
            bolValida = false;
        }
        else {
            $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid #888888 1px");
        }
        if (strOficinaConsularPri == "0") {
            $("#<%=ddlOficinaConsularPri.ClientID %>").focus();
            $("#<%=ddlOficinaConsularPri.ClientID %>").css("border", "solid Red 1px");
            bolValida = false;
        }
        else {
            $("#<%=ddlOficinaConsularPri.ClientID %>").css("border", "solid #888888 1px");
        }
        return bolValida;
    }
    function limpiarbusquedaEscrituraPri() {
        $("#<%=txtAnioEscrituraPri.ClientID %>").val('');
        $("#<%=txtAnioEscrituraPri.ClientID %>").css("border", "solid #888888 1px");
        $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").val('');
        $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid #888888 1px");
        $("#<%=ddlOficinaConsularPri.ClientID %>").val('0');
        $("#<%=ddlOficinaConsularPri.ClientID %>").css("border", "solid #888888 1px");
        $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val('');
        $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').attr('disabled', false);
        $("#<%= txtTipoActoNotarialPri.ClientID %>").val('');
        $("#<%=txtTipoActoNotarialPri.ClientID %>").attr('disabled', false);
        $("#<%=txtNotariaPri.ClientID %>").val('');
        $("#<%=txtFindOficinaConsularPri.ClientID %>").val('');
        $("#<%=txtFindOficinaConsularPri.ClientID %>").css('visibility', 'hidden');        
    }

    function busquedaEscritura() {
        var rpta = isSessionTimeOut();
        if (rpta != 'ok') return;

        if (ValidarTabBusquedaPrimigenia()) {
            var prm = {};
            prm.anpr_cAnioEscritura = $("#<%= txtAnioEscrituraPri.ClientID %>").val();
            prm.anpr_vNumeroEscrituraPublica = $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val();
            prm.anpr_sOficinaConsularId = $("#<%= ddlOficinaConsularPri.ClientID %>").val();
            prm.PG_PE = $("#<%= hf_PG_PE.ClientID %>").val();
            var sndprm = {};
            sndprm.actonotarialPrimigenia = JSON.stringify(prm);

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/busqueda_escritura",
                data: JSON.stringify(sndprm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rsp) {
                    if (rsp.d != null) {
                        var oResponse = JSON.parse(rsp.d);
                        if (oResponse.acno_iActoNotarialId != 0 && oResponse.Message == null) {
                            $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val(oResponse.acno_iActoNotarialId);
                            $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val(oResponse.FechaExtension);
                            $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').attr('disabled', true);
                            $("#<%= txtTipoActoNotarialPri.ClientID %>").val(oResponse.Sub_Tipo);
                            $("#<%=txtTipoActoNotarialPri.ClientID %>").attr('disabled', true);
                            $("#<%=txtNotariaPri.ClientID %>").focus();
                            
                        }
                        else {
                            $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val('0');
                            $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val('');
                            $('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').attr('disabled', false);
                            $("#<%= txtTipoActoNotarialPri.ClientID %>").val('');
                            $("#<%=txtTipoActoNotarialPri.ClientID %>").attr('disabled', false);
                            $("#<%= txtNotariaPri.ClientID %>").val('');
                            $("#<%=txtNotariaPri.ClientID %>").attr('disabled', false);
                            showdialog('i', 'Registro Notarial : Búsqueda', "No se encuentra registrado la Escritura Pública", false, 160, 300);
                            
                        }
                    }
                    else {
                        if (oResponse.Identity == 0 && oResponse.Message != null) {
                            showdialog('e', 'Registro Notarial : Búsqueda', oResponse.Message.toString(), false, 160, 300);
                        }
                    }
                },
                error: errores
            });
        }
    }

</script>

    <table class="mTblTituloM2" align="center">
        <tr>
            <td align="left" valign="top">
                <h2>
                    <asp:Label ID="lblTitRegistroUnico" runat="server" Text="ESCRITURAS PÚBLICAS" /></h2>
            </td>
            <td align="right">
                <asp:LinkButton ID="Tramite" runat="server" PostBackUrl="~/Registro/FrmTramite.aspx"
                    Font-Bold="True" Font-Size="10pt" ForeColor="Blue" 
                    OnClick="Tramite_Click" Font-Underline="True">Regresar a Trámites</asp:LinkButton>
            </td>
        </tr>
    </table>
    <table style="width: 95%;" align="center"  bgcolor="#4E102E">
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top" style="width: 500px;">
                            <asp:Label ID="lblRecurrente" runat="server" Font-Bold="True" BackColor="" Font-Names="Arial"
                                Style="position: relative; top: 1px; left: 5px;" Font-Size="10pt" Font-Underline="false"
                                ForeColor="White" Text=""></asp:Label>
                        </td>
                        <td align="right" style="width: 500px;">
                            <asp:Label ID="lblTipoActoNotarialEP" runat="server"  Font-Bold="True" BackColor="" Font-Names="Arial"
                                Style="position: relative; top: 1px; left: 5px;" Font-Size="10pt" Font-Underline="false"
                                ForeColor="White" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 95%;" align="center">
        <tr>
            <td>
                <asp:HiddenField ID="hdn_AccionOperacion" runat="server"  Value="0" />
                <asp:HiddenField ID="hdn_AccionActualizar" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_AccionConsultar" runat="server" Value="0" />
             
                <asp:HiddenField ID="hdn_acno_iActoNotarialId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_actu_iPersonaRecurrenteId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_iActoNotarialReferenciaId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_ancu_iActoNotarialCuerpoId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_SelectedTab" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_ando_iActoNotarialDocumentoId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_tipoParticipante" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_acno_estadoActoProtocolar" runat="server" Value="0" />
                
                <asp:HiddenField ID="hdn_TxtIntro" runat="server" Value="" />
                <asp:HiddenField ID="hdn_TxtConclu" runat="server" Value="" />
                <asp:HiddenField ID="Hdn_TxtFinal" runat="server" Value="" />
                <asp:HiddenField ID="hdn_IndiceGrillaParticipantesEdicion" runat="server" Value="" />
                <asp:HiddenField ID="hdn_FuncionarioId" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_Rectificacion" runat="server" Value="0" />
                <asp:HiddenField ID="hdn_NoExistePersonaBuscada" runat="server" />
                <asp:HiddenField ID="hdn_CONFORMIDAD_DE_TEXTO" runat="server" Value="0" />

                <asp:HiddenField ID="HF_INTERPRETE" runat="server" Value="0" />
                <asp:HiddenField ID="HF_TESTIGO_A_RUEGO" runat="server" Value="0" />
                <asp:HiddenField ID="HF_OTORGANTE" runat="server" Value="0" />
                <asp:HiddenField ID="HF_APODERADO" runat="server" Value="0" />
                <asp:HiddenField ID="HF_VENDEDOR" runat="server" Value="0" />
                <asp:HiddenField ID="HF_COMPRADOR" runat="server" Value="0" />
                <asp:HiddenField ID="HF_ANTICIPANTE" runat="server" Value="0" />
                <asp:HiddenField ID="HF_ANTICIPADO" runat="server" Value="0" />
                <asp:HiddenField ID="HF_DONANTE" runat="server" Value="0" />
                <asp:HiddenField ID="HF_DONATARIO" runat="server" Value="0" />


                <asp:HiddenField ID="HF_NACIONALIDAD_EXTRANJERA" runat="server" Value="0" />
                <asp:HiddenField ID="HF_NACIONALIDAD_PERUANA" Value="0" runat="server" />
                <asp:HiddenField ID="HF_PAGADO_EN_LIMA" Value="3501" runat="server" />
                <asp:HiddenField ID="HF_GRATIS" Value="0" runat="server" />
                <asp:HiddenField ID="HF_NOCOBRADO" Value="0" runat="server" />
                <asp:HiddenField ID="HF_EFECTIVO" Value="0" runat="server" />
                <asp:HiddenField ID="HF_FORMATO_ACTIVO" Value="0" runat="server" />
                
                <asp:HiddenField ID="HF_TIPOACTO_COMPRA_VENTA" Value="0" runat="server" />

                <asp:HiddenField ID="HF_ACTONOTARIAL_POR_TARIFA" Value="0" runat="server" />
                <asp:HiddenField ID="HF_TEXTOVALIDACION" Value="" runat="server" />
                <asp:HiddenField ID="HF_ValoresDocumentoIdentidad" Value="" runat="server" />        
                
                <asp:HiddenField ID="HF_CIUDAD_FECHA" Value="" runat="server" />       
                <asp:HiddenField ID="HF_TIPO_PAGO_ID" runat="server" />   
                <asp:HiddenField ID="HF_NOMBRE_RECURRENTE" runat="server" />
                <asp:HiddenField ID="HF_DOCUMENTO" runat="server" />
                <asp:HiddenField ID="HFValidarTextoRune" runat="server" />
                
                
                <asp:HiddenField ID="HF_vDescTipDoc" runat="server" />
                <asp:HiddenField ID="HF_vNroDocumento" runat="server" />

                <br />
                <div id="tabs" style="margin: 0px; padding: 0px;">
                    <ul>
                        <li><a href="#tabs-1">
                            <asp:Label ID="lblTitTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        <li><a href="#tabs-2">
                            <asp:Label ID="lblTitTabParticip" runat="server" Text="Participantes"></asp:Label></a></li>
                        <li><a href="#tabs-3">
                            <asp:Label ID="lblTipTabCuerpo" runat="server" Text="Cuerpo"></asp:Label></a></li>
                        <li><a href="#tabs-4">Aprobación y Pago</a></li>
                        <li><a href="#tabs-5">Vinculación</a></li>
                        <li><a href="#tabs-6">Adjuntos</a></li>
                    </ul>
                    <div id="tabs-1">
                        <div align="left">
                            <table style="width:100%">
                                <tbody>
                                    <tr>
                                        <td style="width:140px">
                                            <input id="btnSave_tab1" type="button" value="  Grabar" class="btnSave" onclick="grabar_tab_01();"
                                                style="width: 100px;" />
                                        </td>
                                        <td>
                                            <input id="btnCncl_tab1" type="button" value="  Limpiar" class="btnLimpiar" style="width: 100px;"
                                                onclick="LimpiarTabRegistro();" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:UpdatePanel runat="server" ID="updTipoActoNotarial" UpdateMode="Conditional">
                            <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td id="tdMsjeRegistro" colspan="7">
                                            <asp:Label ID="lblValidacionRegistro" runat="server" Text="Debe ingresar los campos requeridos"
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTipo" runat="server" Text="Tipo de Acto: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Cmb_TipoActoNotarial" runat="server" Width="343px"  AutoPostBack="true" style="cursor:pointer"
                                                onselectedindexchanged="Cmb_TipoActoNotarial_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <p style="color: Red; display: inline; background: White;">
                                                (*)</p>
                                            <asp:CheckBox ID="chk_acno_bFlagMinuta" runat="server" onChange="javascript:checkMinuta();"/>
                                            <asp:Label ID="lblMinuta" runat="server" Text="Minuta"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <p style="background: White;">"...QUE CONSTE (EL)(LA)..."</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDenominacion" runat="server" Text="Denominación del Acto: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Denominacion" runat="server" Width="573px" data-required="true"
                                                 type="text"
                                                MaxLength="200" CssClass="txtLetra" Height="74px" TextMode="MultiLine"  />
                                            <asp:Label ID="Label10" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lbl_Funcionario" runat="server" Text="Funcionario Responsable:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFuncionario" runat="server"   Width="382px" style="cursor:pointer"
                                                TabIndex="2" />
                                            <p style="color: Red; display: inline; background: White;">
                                                (*)</p>
                                        </td>
                                    </tr>
                                    <tr id="trNroEscrituraAnterior" style="display: none;">
                                        <td>
                                            <asp:Label ID="lblNroEscrituraAnt" runat="server" Text="Nro.Escritura Pública Anterior: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_EscrituraAnterior" runat="server" Width="226px" data-required="true"
                                                type="text" MaxLength="50" CssClass="txtLetra" onkeypress="return isLetraNumeroDoc(event);" />
                                            <asp:Label ID="Label2" runat="server" Text="(*)" Style="color: #FF0000;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trAccionRectificacion" style="display: none;">
                                        <td>
                                            <asp:Label ID="lblAccion" runat="server" Text="Acción: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAccionR" runat="server" Width="343px" style="cursor:pointer">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label4" runat="server" Text="(*)" Style="color: #FF0000;"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        
                        <!-- Tabla Primigenia -->
                            <asp:HiddenField ID="hdf_ActoNotarialReferencialPriId" runat="server" />
                            <asp:HiddenField ID="hdf_ActoNotarialPrimigeniaId" runat="server" />
                            <asp:HiddenField ID="hf_ValidaTablaPrimigenia" runat="server" />
                            <asp:HiddenField ID="hf_PG_PE" runat="server" />
                            <br />
                            <table id="tablaPrimigenia" style="display: none;" >
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAnioEscrituraPri" runat="server" Text="Año:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAnioEscrituraPri" runat="server" Width="50px" data-required="true"
                                            type="text" MaxLength="4" onkeypress='validaSiEsNumero(event)' onchange="return validarAnioEscritura()" />
                                            <asp:Label ID="Label6" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblNumeroEscrituraPri" runat="server" Text="Número de Escritura:"></asp:Label>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtNumeroEscrituraPublicaPri" runat="server" Width="100px" data-required="true" 
                                                        type="text" MaxLength="50" CssClass="txtLetra" onkeypress='validaSiEsNumero(event)' />
                                            <asp:Label ID="Label8" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOficinaConsularPri" runat="server" Text="Oficina Consular:"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:DropDownList ID="ddlOficinaConsularPri" runat="server"   Width="300px" style="cursor:pointer"/>
                                    
                                    &nbsp;<img alt="Buscar Oficina Consular" src="../Images/i.p.buscar.gif" style="cursor:pointer" onclick="ActivaFindOficina('<%= txtFindOficinaConsularPri.ClientID %>')" title="Buscar Oficina Consular"/>
                                    </td>
                                    <td align="right">
                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5"></td>
                                    <td>
                                             <asp:TextBox ID="txtFindOficinaConsularPri" runat="server" Width="300px"  onBlur="conMayusculas(this)"
                                                      onkeyup="BusquedaOficinaConsularPri()"  CssClass="txtLetra"  />
                                    </td>
                                    <td>
                                     <input id="btnBuscarEscrituraPrimigenia" type="button" value="  Buscar" class="btnBusqueda" onclick="busquedaEscritura()"
                                                style="width: 100px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="border-bottom-style:solid; border-bottom-color:Gray; border-bottom-width:thin" >
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Label ID="lblFechaExpedicionPri" runat="server" Text="Fecha de expedición de la 1ra. primigenia:"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                             <SGAC_Fecha:ctrlDate ID="ctrFechaExpedicionPri" runat="server"  /> 
                                             <asp:Label ID="Label11" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"  align="right">
                                        <asp:Label ID="lblTipoActoPri" runat="server" Text="Tipo de acto:"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                         <asp:TextBox ID="txtTipoActoNotarialPri" runat="server" Width="300px"  onBlur="conMayusculas(this)"
                                                      MaxLength="100"  CssClass="txtLetra" data-required="true" />
                                        <asp:Label ID="Label14" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"  align="right">
                                        <asp:Label ID="lblNotariaPri" runat="server" Text="Notaria:"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtNotariaPri" runat="server" Width="300px"  onBlur="conMayusculas(this)"
                                                      MaxLength="200"  CssClass="txtLetra"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="Label15" runat="server" Text="(*)" Style="color: #FF0000"></asp:Label>
                                         <asp:Label ID="lblDatosObligatoriosPri" runat="server" Text="Datos Obligatorios"></asp:Label>
                                         
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="tabs-2">
                        <div align="left">

                         <asp:UpdatePanel runat="server" ID="updParticipantesOtogarntes" UpdateMode="Conditional">
                            <ContentTemplate>
                            <table style="width:100%">
                                <tbody>
                                    <tr>
                                        <td style="width:105px">
                                                                                                                                 
                                            <asp:Button ID="btnGrabarParticipantesNew" runat="server" CssClass="btnSave" 
                                                Text="   Grabar" style="width: 100px;" OnClientClick="tab_02_Limpiar();"
                                                onclick="btnGrabarParticipantesNew_Click" />
                                                 
                                        </td>
                                        <td>
                                            <input id="Button7" type="button" value="  Limpiar" class="btnLimpiar" style="width: 100px;"
                                                onclick="tab_02_Limpiar();" />
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td id="tdMsjeParticipantes" colspan="7">
                                            <asp:Label ID="lblValidacionParticipantes" runat="server" Text="Debe ingresar los campos requeridos"
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="border-top: 1px solid #800000; border-top-color: #800000;
                                            border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                            <asp:Label ID="Label5" runat="server" Text="Tipo" Style="font-weight: 700; color: #800000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo Persona:
                                        </td>
                                        <td colspan="2" class="style12">
                                            <asp:DropDownList ID="ddl_pers_sPersonaTipoId" runat="server" Width="200px" style="cursor:pointer" onclick="MostrarOcultarTablaJuridica();" />
                                          
                                        </td>
                                        <td>
                                            Tipo Participante:
                                        </td>
                                        <td colspan="2">
                                            <asp:HiddenField ID="HF_sTipoParticipanteId" runat="server" />
                                            <asp:DropDownList ID="ddl_anpa_sTipoParticipanteId" runat="server" style="cursor:pointer" onChange = "javascript:ValidarParticipanteTestigoRuego();ValidarParticipanteInterprete();"
                                                Width="200px" >
                                            </asp:DropDownList>
                                            <p style="color: Red; display: inline; background: White;">
                                                *</p>
                                        </td>
                                    </tr>
                                    
                                    </tbody>
                            </table>
                            <div id="tablaJuridica">
                            
                             <table style="width:100%" >
                                <tbody>
                                    <tr>
                                        <td colspan="7" style="border-top: 1px solid #800000; border-top-color: #800000;
                                            border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                            <asp:Label ID="Label23" runat="server" Text="Persona Jurídica" Style="font-weight: 700;
                                                color: #800000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo Documento:
                                        </td>
                                        <td colspan="2" class="style12">
                                            <asp:DropDownList ID="ddl_empr_sTipoDocumentoId" runat="server" Width="200px" style="cursor:pointer">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Nro Documento:
                                        </td>
                                        <td colspan="2">
                                            <div class="control-group">
                                                <div class="controls">
                                                    <asp:TextBox ID="txt_empr_vNumeroDocumento" runat="server" Width="197px" minlength="11"
                                                        onkeypress="isNumberKey(event);"  MaxLength="20" CssClass="txtLetra" />
                                                    <img alt="" src="../Images/img_16_search.png" onclick="javascript:tab_02_PersonaJuridicaBuscar();"
                                                        style="cursor: pointer;" />
                                                    <asp:Button ID="btn_ejecutar_Busqueda" runat="server" Text="Button" OnClientClick="return tab_02_PersonaJuridicaBuscar();"
                                                        Style="display: none;" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Razón Social:
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txt_empr_vRazonSocial" runat="server" Width="650px" CssClass="txtLetra"
                                                MaxLength="255" onkeypress="return isLetraNumeroDoc(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <div id="divRepresentantesLegales">
                                                <label>
                                                    Representante(s) Legal(es)</label>
                                                    <asp:DropDownList runat="server" ID="ddlRepresentantes" Enabled="false" CssClass="ddlRepresentantes" style="cursor:pointer"></asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                         <asp:UpdatePanel runat="server" ID="updDatosParticipantes" UpdateMode="Conditional">
                            
                                <ContentTemplate>
                                    <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%; border-bottom-color: #800000;border-top-color: #800000;margin-top: 5px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" Text="Persona Natural ó Representante Legal (Empresa)" runat="server" Style="font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width:100%">
                                        <caption>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <asp:HiddenField ID="hdn_acpa_distrito" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_acpa_provincia" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_acpa_pais" runat="server" Value="0" />
                                                    <asp:HiddenField ID="HF_REGISTRO_NUEVO" runat="server" Value="0" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:HiddenField ID="hdn_Tipo_Participante_Editando" runat="server" 
                                                        Value="-1" />
                                                    <asp:HiddenField ID="hdn_Tipo_Participante_Selected" runat="server" Value="0" />
                                                    <asp:HiddenField ID="HF_IREFERENCIAPARTICIPANTE" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdn_acpa_residenciaId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="HF_IREFERENCIAINTERPRETE" runat="server" Value="0" />
                                                </td>
                                            </tr>                                           
                                            
                                            <tr>
                                                <td style="width:120px">
                                                    Tipo Documento:
                                                    </td>
                                                <td style="width:300px">                                                    
                                                    <asp:DropDownList ID="ddl_peid_sDocumentoTipoId" runat="server" Width="180px" 
                                                    style="cursor:pointer" CssClass="DropDownList"  onChange = "javascript:ActivarOtrodocumento();" >
                                                    </asp:DropDownList>
                                                    <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                                <td align="right">
                                                    Nro Documento:
                                                </td>
                                                <td>
                                                    <div class="control-group">
                                                        <div class="controls">
                                                            <asp:TextBox ID="txt_peid_vDocumentoNumero" runat="server" AutoPostBack="false"  
                                                                CssClass="txtLetra" MaxLength="20" onkeypress="return isValidarNroDocumento(); "                                                                 
                                                                Width="197px" />
                                                            <img id="imgBuscarPN"  src="../Images/img_16_search.png"                                                                  
                                                                   style="cursor: pointer; width: 16px" alt="Buscar documento" 
                                                                   onclick="imgBuscarPersonaN(event)"  
                                                                    />
                                                                                                                     
                                                            <p style="color: Red; display: inline; background: White;">
                                                                *</p>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <div id="tablaOtroDocumento" runat="server" style="display: none;">
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblOtroDocumento" runat="server" Text="Otro documento :" />
                                                </td>
                                                <td>
                                                    
                                                    <asp:TextBox ID="txtDescOtroDocumento" runat="server" CssClass="txtLetra" 
                                                        onkeypress="return isSujeto(event);" Width="250px" MaxLength="100"></asp:TextBox>
                                                    <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            </div>
                                            
                                            <tr>
                                                <td>
                                                  Primer Apellido:  </td>
                                                <td>
                                                                                                        
                                                    <asp:TextBox ID="txt_pers_vApellidoPaterno" runat="server" CssClass="txtLetra" 
                                                        MaxLength="100" onkeypress="return isSujeto(event);" onblur="validarCampos(this)" Width="280px" />
                                                    <p style="color: Red; display: inline; background: White;">*</p>                                                                                                        
                                                </td>
                                                <td align="right">
                                                    Apellido de Casada:
                                                </td>
                                                <td>
                                                     <asp:TextBox ID="txt_pers_vApellidoCasada" runat="server" Width="200px" 
                                                         TabIndex="7" CssClass="txtLetra"
                                                                onkeypress="return isSujeto(event);" onblur="validarCampos(this)"
                                                            MaxLength="100" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Segundo Apellido:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_pers_vApellidoMaterno" runat="server" CssClass="txtLetra" 
                                                        MaxLength="100" onkeypress="return isSujeto(event);" onblur="validarCampos(this)" Width="280px" />
                                                </td>
                                                <td>
                                                    
                                                </td>
                                                <td>
                                                   
                                                </td>
                                                                                               
                                                
                                            </tr>  
                                        </caption>
                                        <tr>
                                            <td>
                                                Nombres:
                                            </td>
                                            <td>
                                               
                                               <asp:TextBox ID="txt_pers_vNombres" runat="server" CssClass="txtLetra" 
                                                        MaxLength="100" onkeypress="return isSujeto(event);"  onblur="validarCampos(this)"
                                                        Width="280px" />
                                                     <p style="color: Red; display: inline; background: White;">*</p>
                                               </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                           
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Género:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_pers_sGeneroId" runat="server" 
                                                    CssClass="DropDownList" onchange="ObtenerElementosGenero();validarApellidoCasada();" 
                                                    style="cursor:pointer" Width="250px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblAstGenero" runat="server" 
                                                    Style="color: #FF0000; " Text="*"></asp:Label>
                                            </td>
                                            <td align="right">
                                                Estado Civil:</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_pers_sEstadoCivilId" runat="server"  onchange="validarApellidoCasada();"
                                                    CssClass="DropDownList" style="cursor:pointer" Width="200px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblAstEstCivil" runat="server" 
                                                    Style="color: #FF0000; " Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblPaisOrigen" runat="server" Text="Nacionalidad :" />
                                            </td>
                                            <td colspan="3">
                                            <%--<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                                                    style="cursor:pointer" CssClass="DropDownList"                                                     
                                                    Width="250px" 
                                                    onselectedindexchanged="ddlPaisOrigen_SelectedIndexChanged" />--%>

                                                <asp:DropDownList ID="ddlPaisOrigen" runat="server"  
                                                    style="cursor:pointer" CssClass="DropDownList"                                                                                                         
                                                    Width="250px" />
                                                <p style="color: Red; display: inline; background: White;">*</p>

                                                &nbsp;

                                                <asp:Label ID="LblDescNacionalidadCopia" runat="server" 
                                                    style="text-transform:uppercase; color:Gray" Width="300px" />
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_pers_sNacionalidadId" runat="server" Width="180px" CssClass="hideControl">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            
                                        </tr>
                                    </table>
                                    <table style="width:100%">
                                        <tr>
                                            <td colspan="6" style="border-top: 1px solid #800000; border-top-color: #800000;
                                                border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="lbl_direccion" runat="server" Text="Dirección" Style="font-weight: 700;
                                                    color: #800000;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Domicilio:</td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txt_resi_vResidenciaDireccion" runat="server" CssClass="txtLetra"
                                                                MaxLength="255" Width="95%" 
                                                    onBlur="conMayusculas(this)" />
                                                <asp:Label ID="lblAstDomicilio" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                            </td>                                                        
                    
                                        </tr>
                                       
                                        <tr>
                                            <td>Continente/Dpto.:</td>
                                            <td>

                                                <asp:DropDownList ID="ddl_UbigeoPais" runat="server" Width="200px" onchange = "cargarProvincia();" style="cursor:pointer" CssClass="DropDownList" AutoPostBack="false">
                                                </asp:DropDownList>

                                                <asp:Label ID="lblAstDpto" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                            
                                            </td>
                                            <td>País/Provincia:</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_UbigeoRegion" runat="server" onchange = "cargarDistrito();" style="cursor:pointer" CssClass="DropDownList" AutoPostBack="false"
                                                    Width="200px">
                                                </asp:DropDownList>

                                                <asp:Label ID="lblAstProv" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                            </td>
                                            <td>Distrito/Ciudad:</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_UbigeoCiudad" runat="server" onchange = "cargarUbigeo();" style="cursor:pointer" CssClass="DropDownList" AutoPostBack="false"
                                                    Width="200px">
                                                </asp:DropDownList>

                                                <asp:Label ID="lblAstDist" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                                <asp:HiddenField ID="hubigeo" runat="server" />
                                                <asp:HiddenField ID="hubigeoLoad" runat="server" />
                                            </td>
                                            
                                            
                                        </tr>
                                        <tr id="trCodigoPostal" style="display: none;">
                                            <td> Código Postal:                                              
                                            </td>
                                            <td>               
                                                 <asp:TextBox ID="txt_resi_vCodigoPostal" runat="server" CssClass="txtLetra" 
                                                        MaxLength="10" onkeypress="isNumeroLetra(event);"  
                                                        />                              
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
                                    </table>

                                    <table id="dtDireccionOtros" style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%; border-bottom-color: #800000;border-top-color: #800000;margin-top: 5px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOtros" Text="OTROS" runat="server" Style="font-weight: 700;
                                                    color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                            
                            <table style="width:100%">
                                 <tr>
                                    <div id="DivOpcionIncapacitado" runat="server">
                                        <td style="width: 130px" valign="middle">
                                            <asp:Label ID="lblIncapacitadoTitulo" runat="server" Text="Incapacitado" style="color: Black;display: inline-block;"></asp:Label>
                                        </td>
                                        <td colspan="3" >
                                            <asp:CheckBox ID="chkIncapacitado" runat="server" Width="100px" 
                                                style="vertical-align:middle;" AutoPostBack="false"
                                            onChange="javascript:check_HabilitarIncapacidad();" />                                       
                                        </td>                                   
                                    </div>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table width="100%" cellpadding="0px" cellspacing="0px">
                                            <tr>
                                                <div id="DivIncapacitado" runat="server">
                                                    <td style="width: 130px">
                                                         <asp:Label ID="lblIncapacidadTitulo" runat="server" Text="Incapacitado de firmar por ser:"
                                                              Style="color: Black" Width="155px"></asp:Label>
                                                    </td>
                                                    <td style="width: 300px; text-align: left">
                                                         <asp:TextBox ID="txtRegistroTipoIncapacidad" runat="server" 
                                                             Width="300px" MaxLength="300" CssClass="txtLetra" /> 
                                                    </td>
                                                    <td style="width: 35px; text-align: center">
                                                            <asp:CheckBox ID="chkNoHuella" runat="server" Width="25px" TabIndex="11" />
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
                                            <p id="lblProfesionTitulo" style="color: Black; display: inline; background: White; ">Ocupación:</p>
                                         </td>
                                         <td style="width: 450px">
                                             <asp:DropDownList ID="ddl_pers_sProfesionId" runat="server" 
                                                 style="cursor:pointer" CssClass="DropDownList"
                                                Width="400px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblAstProf" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                         </td>
                                    </div>
                                    <div id="DivIdioma" runat="server">
                                        <td style="width:70px">
                                            <p id="lblIdiomaTitulo" style="color: Black; display: inline; background: White; ">Idioma:</p>
                                        </td>
                                        <td style="width:210px">
                                            <asp:DropDownList ID="ddl_pers_sIdiomaNatalId" runat="server" style="cursor:pointer" 
                                            CssClass="DropDownList" Width="180px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblAstIdioma" runat="server" Text="*" Style="color: #FF0000; "></asp:Label>
                                         </td>
                                    </div>                                   
                                </tr>
                                <tr>
                                    <td style="width: 605px" colspan="4">
                                        <div id="OtorganteIncapacitado" runat="server">
                                             <asp:Label ID="lblParticipanteDiscapacidad" runat="server" style="color: Black;"
                                                        Text="Otorgante con Incapacidad de Firmar: " ></asp:Label>
                                            &nbsp;
                                            <asp:DropDownList ID="ddl_Participante_Discapacidad" runat="server" 
                                                    style="display: inline; cursor:pointer" Width="380px">
                                             </asp:DropDownList>                                             
                                        </div>
                                    </td>                                   
                                </tr>
                                <tr>
                                        <td style="width: 605px" colspan="4">
                                            <div id="OtorganteConInterprete" runat="server">
                                                <asp:Label ID="lblParticipanteConInterprete" runat="server" style="color: Black;"
                                                        Text="Otorgante con Interprete: " ></asp:Label>
                                                    &nbsp;
                                                    <asp:DropDownList ID="ddl_Participante_Interprete" runat="server" 
                                                            style="display: inline; cursor:pointer" Width="380px">
                                                    </asp:DropDownList> 
                                            </div>
                                        </td>
                                </tr>
                            </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div>
                                <asp:UpdatePanel runat="server" ID="updGrillaParticipantes" UpdateMode="Conditional">
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAgregarParticipanteNew" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Button ID="btnAgregarParticipanteNew" runat="server" 
                                            Text="     Agregar" CssClass="btnNew"
                                            Style="width: 100px;" onclick="btnAgregarParticipanteNew_Click"  />

                                        <br />
                                        <div style="overflow:auto; height:190px; width:900px; border:1px solid #800000; margin: 5px;">
                                        <asp:GridView ID="grd_Participantes" runat="server" AutoGenerateColumns="False" Width="900px"
                                            CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" ShowHeaderWhenEmpty="True"
                                            OnRowCommand="grd_Participantes_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="acpa_sTipoParticipanteId_desc" HeaderText="Tipo Participante">
                                                    <HeaderStyle Width="150px" />
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tipo Doc." DataField="peid_sDocumentoTipoId_desc" />
                                                <asp:BoundField DataField="peid_sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta" >
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Nro Doc." DataField="peid_vDocumentoNumero" />
                                                <asp:BoundField HeaderText="Participante" DataField="participante" HtmlEncode="false">
                                                    <HeaderStyle Width="300px" />
                                                    <ItemStyle Width="300px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="anpa_sTipoParticipanteId">                                                   
                                                    <ControlStyle CssClass="hideControl" Width="0px" />
                                                    <FooterStyle CssClass="hideControl" Width="0px" />
                                                    <HeaderStyle CssClass="hideControl" Width="0px" />
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>                                         
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Nacionalidad" DataField="vNacionalidad">
                                                    <HeaderStyle Width="130px" />
                                                    <ItemStyle Width="130px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Idioma" DataField="pers_sIdiomaNatalId_desc">
                                                    <HeaderStyle Width="130px" />
                                                    <ItemStyle Width="130px" />
                                                </asp:BoundField>                                                

                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEditarParticipante" CommandName="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEliminarParticipante" CommandName="Eliminar" ToolTip="Eliminar"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_delete.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="anpa_iActoNotarialParticipanteId">
                                                     <ControlStyle CssClass="hideControl" />
                                                    <FooterStyle CssClass="hideControl" />
                                                    <HeaderStyle CssClass="hideControl" />
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="pers_bIncapacidadFlag"  
                                                    HeaderText="pers_bIncapacidadFlag"   > 
                                                    <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="anpa_iActoNotarialParticipanteIdAux"  ControlStyle-Width="0px"
                                                    HeaderText="anpa_iActoNotarialParticipanteIdAux">
                                                     <ControlStyle CssClass="hideControl"  Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl"  Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="anpa_sReferenciaParticipanteId" ControlStyle-Width="0px"
                                                    HeaderText="anpa_sReferenciaParticipanteId">
                                                     <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="pers_sNacionalidadId_desc" ControlStyle-Width="0px"
                                                    HeaderText="pers_sNacionalidadId_desc">
                                                     <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>

                                                 <asp:BoundField DataField="anpa_cEstado" ControlStyle-Width="0px"
                                                    HeaderText="anpa_cEstado">
                                                     <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="resi_vResidenciaDireccion" ControlStyle-Width="0px"
                                                    HeaderText="resi_vResidenciaDireccion">
                                                     <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="resi_cResidenciaUbigeo" ControlStyle-Width="0px"
                                                    HeaderText="resi_cResidenciaUbigeo">
                                                     <ControlStyle CssClass="hideControl" Width="0px"/>
                                                    <FooterStyle CssClass="hideControl" Width="0px"/>
                                                    <HeaderStyle CssClass="hideControl" Width="0px"/>
                                                    <ItemStyle CssClass="hideControl" Width="0px"/>
                                                </asp:BoundField>

                                            </Columns>
                                            <SelectedRowStyle CssClass="slt " />
                                        </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div id="tabs-3" align="left">
                     
                        <table>
                            <tbody>
                                <tr>       
                                        <td>
                                            <input id="btnTraerCuerpo" type="button" 
                                            value="Generar Cuerpo" class="btnAlmacenAcepta"
                                            onclick="generar_cuerpo_escritura();" style="width: 130px;" />
                                        </td>   
                                    <td>                                    
                                                                                        
                                        <asp:Button ID="Btn_VistaPreviaAprobar" runat="server" Text="    Vista Previa" 
                                            Width="120px" CssClass="btnSearch" Visible="true"  Enabled="true"  
                                            OnClientClick="return tab_03_VistaPrevia('1');" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;<input id="btnCncl_tab3" type="button" value="    Limpiar" class="btnLimpiar" onclick="LimpiarTabCuerpo();" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <div id="tdMsjeCuerpo">
                            <asp:Label ID="lblValidacionCuerpo" runat="server" Text="Debe ingresar los campos requeridos para la generación del cuerpo"
                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"> </asp:Label>
 
                        </div>
                        <div>
                         <asp:UpdatePanel ID="updCuerpoCabecera" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <table>
                            <tr>
                               <td colspan="10">
                                 <asp:Label ID="lblEtiquetaFoliosDisponibles" runat="server"  
                                                        Text="Número de Folios disponibles:"></asp:Label>
                                   &nbsp;
                                 <asp:Label ID="lblFoliosDisponibles" runat="server"  
                                                        Text=""></asp:Label>
                                </td>

                            </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="N°Escritura: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroEscritura" runat="server" MaxLength="5" 
                                             Width="91px" Enabled="False">MRE000</asp:TextBox>
                                        <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    
                                      <td>
                                          <asp:Label ID="lblNroOficio" runat="server" Text="N° de Oficio: "></asp:Label>
                                    </td>
                                    <td>
                                            <asp:TextBox ID="txtNroOficio" runat="server" MaxLength="5" onkeypress="isNumberKey(event);"
                                             Width="86px"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label28" runat="server" Text="N°Libro: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroLibro" runat="server" MaxLength="5" onkeypress="isNumberKey(event);"
                                             Width="80px" Enabled="False">LIBR000</asp:TextBox>
                                        <p style="color: Red; display: inline; background: White;">
                                            *</p>
                                    </td>
                                    
                                    <td>
                                        <asp:Label ID="Label29" runat="server" Text="N°Foja Inicial: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="HF_NroHojas" runat="server" />
                                        <asp:TextBox ID="txtNroFojaIni" runat="server" Width="40px" MaxLength="4" onkeypress="isNumberKey(event);"
                                             Enabled="False">0</asp:TextBox>
                                        <p style="color: Red; display: inline; background: White;">
                                            *</p>
                                    </td>
                                   
                                    <td>
                                        <asp:Label ID="Label30" runat="server" Text="N°Foja Final: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroFojaFinal" runat="server" Width="40px" MaxLength="4" onkeypress="isNumberKey(event);"
                                            Enabled="False">0</asp:TextBox>
                                        <p style="color: Red; display: inline; background: White;">
                                            *</p>
                                    </td>
                                </tr>                                                                                                                                                                                            
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Costo de la Escritura Pública: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCostoEP" runat="server" Width="60px" Enabled="False">0</asp:TextBox>
                                    </td>
                                    <td style="width:100px" align="right">
                                        <asp:Label ID="Label16" runat="server" Text="2do. Parte: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCostoParte2" runat="server" Width="60px" Enabled="False">0</asp:TextBox>
                                    </td>
                                    <td  style="width:100px" align="right">
                                        <asp:Label ID="Label17" runat="server" Text="Testimonio: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCostoTestimonio" runat="server" Width="60px" Enabled="False">0</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                        <br />
                        <div id="accordion" style="width: 905px;">
                            <h3 style="display:none">Introducción <span style="color: Red; display: none; background-color: transparent;">*</span></h3>
                            <div style="display:none">
                                <asp:Label ID="lblIntro" runat="server" Text=""  style="display:none"></asp:Label>
                            </div>
                                                                                    
                            <h3>Texto Central <span style="color: Red; display: inline; background-color: transparent;">*</span></h3>
                            <div>                                
                                <asp:TextBox ID="RichTextBox" runat="server" Class="mceEditor txtLetra" 
                                    Height="180px" TextMode="MultiLine" Width="870px"></asp:TextBox> 
                            </div>
                            <h3 style="display:none">Cierre del Texto Central <span style="color: Red; display: none; background-color: transparent;">*</span></h3>
                            <div style="display:none">
                                 <asp:Label ID="lblCierreTextoCentral" runat="server" Text="" ></asp:Label>
                            </div> 
                                <h3>Imágenes</h3>
                                <div>

                                    <asp:UpdatePanel ID="updImagenes" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>Título:</td>
                                                    <td><asp:TextBox runat="server" ID="txtImagenTitulo" Width="400px" onBlur="conMayusculas(this)" CssClass="txtLetra" MaxLength="150"></asp:TextBox> 
                                                    <p style="color: Red; display: inline; background-color: transparent;">*</p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Archivo:</td>
                                                    <td>
                                                         <asp:FileUpload ID="FileUploadImagen" runat="server" size="65" onChange = "javascript:UploadImg();" accept=".gif,.png,.jpg,.jpeg" />
                                                         <br />
                                                            <p style="display: inline; background-color:White;">Archivos validos: gif, png, jpg, jpge.</p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblValidacionImagen" runat="server" Text="Debe ingresar los campos requeridos."
                                                            CssClass="hideControl" ForeColor="Red" Font-Size="14px">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAgregarImagen" runat="server" Text="  Agregar" CssClass="btnNew" Style="width: 110px;"  OnClientClick="return AgregarImagenes();" />                                                        
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hdn_imagen_nombre" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Button ID="btnCargarImagenes" runat="server" Text="Agregar" OnClick="btnCargarImagenes_Click" Style="display: none;" />
                                                        <asp:GridView ID="gdvImagenes" runat="server" CssClass="mGrid" 
                                                            AlternatingRowStyle-CssClass="alt" GridLines="None" 
                                                            AutoGenerateColumns="False" onrowcommand="gdvImagenes_RowCommand">
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
                                                                <asp:BoundField DataField="ando_vRutaArchivo" HeaderText="Nombre Archivo">
                                                                    <ItemStyle Width="300px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ando_vDescripcion" HeaderText="Título">
                                                                    <ItemStyle Width="400px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Vista Previa" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnVistaPrevia" CommandName="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_16_preview.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>                                                                
                                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEliminarImagen" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnCargarImagenes" EventName="Click" />
                                            
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <h3 style="display: none">Conclusiones <span style="color: Red; background-color: transparent;">*</span></h3>                               
                                <div style="display:none">
                                    <asp:Label ID="lblConcluciones" runat="server" Text=""></asp:Label>
                                </div>
                                
                                
                                
                                <h3>Descripción del inciso C del artículo 55 del Decreto Legislativo 1049.</h3>
                                <div>
                                     <asp:UpdatePanel ID="updIncisoCArticulo55" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tablaIncisoCArticulo55" runat="server" style="width: 870px;">
                                                <tr>
                                                    <td style="width: 905px;">
                                                        <asp:TextBox ID="RichTextBoxDL1049Art55IncisoC" runat="server" Class="mceEditor" 
                                                            Height="180px" TextMode="MultiLine" Width="870px"></asp:TextBox>                                                
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                     </asp:UpdatePanel>
                                </div>
                                

                                <h3>Texto Normativos</h3>
                                <div>
                                    <asp:UpdatePanel ID="updTxtNormativo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td valign="top">       
                                                        <asp:HiddenField ID="hf_idsNormativos" runat="server" />
                                                        <div runat="server" ID="divListNormativo"></div>
                                                        <br />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTextoNormativo" runat="server" Enabled="False" 
                                                            Height="180px" Style="text-align: justify;" TextMode="MultiLine" Width="550px"></asp:TextBox>
                                                        <asp:TextBox ID="txtTextoNormativo_" runat="server" Enabled="False" 
                                                        Height="180px" Style="text-align: justify;" TextMode="MultiLine" Width="550px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                
                                <h3 style="display:none">Final <span style="color: Red; display: none; background-color: transparent;">*</span></h3>
                                <div style="display:none">
                                    <asp:Label ID="lblFinal" runat="server" Text=""></asp:Label>
                                </div>

                                <h3>Texto Adicional</h3>
                                <div>
                                    <asp:TextBox ID="RichTextBoxAdicional" runat="server" Class="mceEditor" 
                                        Height="180px" TextMode="MultiLine" Width="870px"></asp:TextBox>
                                </div>
                                <h3>Fecha de Suscripción de Firmas<span style="color: Red; display: inline; background-color: transparent;">*</span></h3>
                                <div>
                                    <asp:UpdatePanel ID="updFirmaOtorgantes" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td style="width:100px" align="left">
                                                        <asp:Button ID="btnActualizarFechaOtorgante" runat="server" Text="      Actualizar" CssClass="btnEdit" Width="130px"
                                                            onclick="btnActualizarFechaOtorgante_Click" />
                                                    </td>
                                                    <td style="width:380px" align="left">
                                                        <asp:Label ID="lblNombresOtorgante" runat="server" Text="Nombres del Otorgante" Width="380px"></asp:Label>
                                                    </td>
                                                    <td style="width:200px" align="left">
                                                        Fecha de Suscripción de Firmas:
                                                    </td>
                                                    <td>
                                                        <SGAC_Fecha:ctrlDate ID="ctrFechaSuscripcion" runat="server" />
                                                        <asp:HiddenField ID="HFOtorganteId" runat="server" />
                                                        <asp:HiddenField ID="hdn_acno_dFechaConclusionFirma" runat="server" />
                                                        <asp:HiddenField ID="hdn_acno_dFechaExtension" runat="server" />                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input id="btnCancelarFechaOtorgante" type="button" value="      Limpiar"  class="btnLimpiar" onclick="limpiarFechaOtorgantes()"/>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>                                            
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnActualizarFechaOtorgante" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div>
                                        <asp:UpdatePanel runat="server" ID="updGrillaOtorgantes" UpdateMode="Conditional">
                                             <ContentTemplate>
                                                <div style="overflow:auto; height:190px; width:850px; border:1px solid #800000; margin: 5px 0px 0px 0px;">
                                                    <asp:GridView ID="grd_Otorgantes" runat="server" AutoGenerateColumns="False" Width="850px"
                                                    CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" 
                                                        ShowHeaderWhenEmpty="True" onrowcommand="grd_Otorgantes_RowCommand">
                                                    <Columns>
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActoNotarialParticipanteId" runat="server" Text='<%# Bind("anpa_iActoNotarialParticipanteId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                          </asp:TemplateField>                                                            
                                                            <asp:BoundField DataField="acpa_sTipoParticipanteId_desc" HeaderText="Tipo Participante">
                                                                <HeaderStyle Width="150px" />
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Tipo Doc." DataField="peid_sDocumentoTipoId_desc" />
                                                            <asp:BoundField DataField="peid_sDocumentoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta" >
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Nro Doc." DataField="peid_vDocumentoNumero" />
                                                            <asp:BoundField HeaderText="Participante" DataField="participante" HtmlEncode="false">
                                                                <HeaderStyle Width="300px" />
                                                                <ItemStyle Width="300px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Fecha de Suscripción" DataField="anpa_dFechaSuscripcion" HtmlEncode="false"
                                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm}">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditarFechaParticipante" CommandName="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                    </Columns>
                                                <SelectedRowStyle CssClass="slt " />
                                             </asp:GridView>
                                                </div>
                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                        </div>
                        <div style="background-color: transparent;">
                            <p style="font-size: xx-small; color: Gray; background-color: inherit; font-style: italic;">
                                
                            </p>
                        </div>
                        <br />
                        <div>
                            <asp:UpdatePanel ID="updDatosAbogado" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="90%">
                                        <tbody>

                                            <tr>
                                                <td>
                                                    Nro. Minuta :
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txt_acno_Numero_Minuta" runat="server" Width="226px" CssClass="txtLetra"
                                                        MaxLength="5"  />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="7">
                                                <table>
                                                    <tr>
                                                    <td>
                                                        Autorizado por:
                                                    </td>
                                                       
                                                        <td style="width:310px" align="center" valign="middle">
                                                        D.N.I.:
                                                         <asp:TextBox ID="txtAutorizacionNroDocumento" runat="server" AutoPostBack="false"  
                                                                CssClass="txtLetra" MaxLength="20" onkeypress="return isValidarAutorizacionNroDocumento(); "                                                                 
                                                                Width="197px" />
                                                            <img id="imgBuscarAutorizacionDocumento"  src="../Images/img_16_search.png"                                                                   
                                                                style="cursor: pointer; width: 16px" alt="Buscar documento" 
                                                                onclick="imgBuscarPersonaAutorizacion(event)"  
                                                            />

                                                        </td>
                                                        <td>
                                                             <asp:TextBox ID="txt_acno_sAutorizacionTipoId" runat="server" Width="226px" CssClass="txtLetra"
                                                                    MaxLength="300" onkeypress="return isSujeto(event);" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                                                                   
                                                </td>
                                            </tr>

                                             <tr>
                                                <td>
                                                    Nombre del Colegio de Abogado:
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txt_acno_vNombreColegiatura" runat="server" Width="226px" CssClass="txtLetra"
                                                        MaxLength="100"  onkeypress="return isSujeto(event);" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    Nro.Colegiatura:
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txt_acno_vNumeroColegiatura" runat="server" Width="226px" CssClass="txtLetra"
                                                        MaxLength="50"  onkeypress="isNumeroLetra(event);" />
                                                </td>
                                            </tr>
                                            <tr style="display:none">
                                                <td valign="top">
                                                    Número de firmas:
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txt_ancu_vFirmaIlegible" runat="server" Width="226px"  
                                                        onkeypress="isDescripcion(event);"  
                                                        CssClass="txtLetra"  />                                                    
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="7">
                                <table width="110%">
                                        <tr>
                                            <td>
                                         <h3>
                                            Parte
                                         </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                         <table width="100%" style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray;">
                           
                            <tr>
                                <td colspan="4">
                                     <h3 id="hRegistradorId" runat="server">
                                            Registrador                                   
                                     </h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                     <asp:UpdatePanel ID="UpdUbigeoRegistrador" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                    <asp:HiddenField ID="HF_NUMERO_PAGINA_DOCUMENTO" Value="1" runat="server" />
                                                  <table width="100%" id="tablaRegistradorId" runat="server">
                                                    <tr>
                                                        <td style="width:130px">                                                            
                                                             <asp:Label ID="lblRegistrador" runat="server" Text="Registrador:"  />
                                                        </td>
                                                        <td>
                                                             <asp:TextBox ID="txtRegistradorNombres" runat="server" Width="300px"  onBlur="conMayusculas(this)"
                                                                CssClass="txtLetra"  />
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                
                                                    <tr>
                                                        <td style="width:130px">
                                                            <asp:Label ID="Label20" runat="server" Text="Oficina Registral:" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_OficinaRegistralRegistrador" runat="server" Height="21px" Width="300px" style="cursor:pointer" >
                                                            </asp:DropDownList>
                                                            &nbsp;<img alt="Buscar Oficina Registral" src="../Images/i.p.buscar.gif" style="cursor:pointer" onclick="ActivaFindOficina('<%= txtFindOficinaRegistral.ClientID %>')" title="Buscar Oficina Registral"/>
                                                            
                                                        </td>
                                                        <td>
                                                        <asp:TextBox ID="txtFindOficinaRegistral" runat="server" Width="300px"  onBlur="conMayusculas(this)"
                                                            onkeyup="BusquedaOficinaRegistral()"
                                                                CssClass="txtLetra"  />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                <asp:UpdatePanel ID="UpdPresentante" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                 <h3 id="hPresentanteId" runat="server">
                                        Presentante
                                 </h3>
                                 <hr />
                                 </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                 <asp:UpdatePanel ID="updParte" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <table id="tablaPresentanteId" runat="server">
                                            <tr>
                                                <td><asp:RadioButton  runat="server" Text="Apoderado" GroupName="GPresentante" 
                                                        ID="rbApoderado" ></asp:RadioButton></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_Apoderado" runat="server" Height="21px" Width="300px" AutoPostBack="false" style="cursor:pointer">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td><asp:RadioButton runat="server" Text="Otros" Checked="True" 
                                                        GroupName="GPresentante" ID="rbOtros"></asp:RadioButton></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="width:130px">
                                                    <asp:Label ID="Label12" runat="server" Text="Tipo Documento:"   />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_TipoDocrepresentante" runat="server" Height="21px" Width="300px" style="cursor:pointer"
                                                        AutoPostBack="false"  >
                                                    </asp:DropDownList>
                                                    <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                                <td style="width:25px"></td>
                                                <td style="width:145px">
                                                    <asp:Label ID="Label13" runat="server" Text="Número Documento:"  />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRepresentanteNroDoc" runat="server" Width="196px"  onBlur="conMayusculas(this)" onkeypress="return isValidarNroDocumentoPresentante()"
                                                        CssClass="txtLetra" Visible="true" />
                                                        <img id="imgBuscarPresentante"  src="../Images/img_16_search.png"                                                                   
                                                            style="cursor: pointer; width: 16px" alt="Buscar documento" 
                                                            onclick="imgBuscarPersonaPresentante(event)"  
                                                            />
                                                    <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:HiddenField ID="HF_presentanteId" Value="0" runat="server" />
                                                        <asp:Label ID="Label7" runat="server" Text="Nombres y Apellidos:"  />
                                                </td>
                                                <td class="style6" >
                                                        <asp:TextBox ID="txtRepresentanteNombres" runat="server" Width="296px"  onBlur="conMayusculas(this)"
                                                        CssClass="txtLetra" Visible="true" />
                                                    <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                                <td class="style7"></td>
                                                <td class="style6">
                                                <asp:Label ID="Label19" runat="server" Text="Género:"   />
                                                </td>
                                                <td class="style6">
                                                        <asp:DropDownList ID="ddl_GerenoPresentante" runat="server" Width="200px" style="cursor:pointer">
                                                        </asp:DropDownList>
                                                        <p style="color: Red; display: inline; background: White;">*</p>
                                                </td>
                                            </tr>
                                            <!-------------->
                                            <tr>
                                            <td colspan="5">
                                <asp:UpdatePanel runat="server" ID="UpdateGridPresentantes" UpdateMode="Conditional">
                                    
                                    <ContentTemplate>
                                        <asp:Button ID="btnPresentanteAgregar" runat="server" 
                                            Text="     Agregar" CssClass="btnNew"
                                            Style="width: 100px;" onclick="btnAgregarPresentantNew_Click"  />

                                        <br />
                                        <div style="overflow:auto; height:190px; width:900px; border:1px solid #800000; margin: 5px;">
                                        <asp:GridView ID="GridViewPresentante" runat="server" AutoGenerateColumns="False" Width="900px"
                                            CssClass="mGrid tableHoverable" GridLines="None" SelectedRowStyle-CssClass="slt" ShowHeaderWhenEmpty="True"
                                            OnRowCommand="grd_Presentantes_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="anpr_iActoNotarialPresentanteId" HeaderText="Nro" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="anpr_sTipoPresentante" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="anpr_sTipoPresentante_desc" HeaderText="Tipo Presentante"></asp:BoundField>
                                                
                                                <asp:BoundField HeaderText="Nombres y Apellidos" DataField="anpr_vPresentanteNombre" ><HeaderStyle Width="300px" /><ItemStyle Width="150px" /></asp:BoundField>
                                                
                                                <asp:BoundField DataField="anpr_sPresentanteTipoDocumento" >
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tipo documento" DataField="anpr_vPresentanteTipoDocumento_desc"  >
                                                    
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Documento" DataField="anpr_vPresentanteNumeroDocumento" />
                                                <asp:BoundField HeaderText="Genero" DataField="anpr_sPresentanteGenero" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                </asp:BoundField>                                           
                                                <asp:BoundField HeaderText="Genero" DataField="anpr_vPresentanteGenero_desc" HtmlEncode="false">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField> 

                                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEliminarParticipante" CommandName="Eliminar" ToolTip="Eliminar" 
                                                            OnClientClick="return confirm('Desea Eliminar el registro');"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_delete.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>

                                            </Columns>
                                            <SelectedRowStyle CssClass="slt " />
                                        </asp:GridView>
                                        </ContentTemplate>
					</asp:UpdatePanel>
                                        </td>
                                        </tr>
                                            <!-------->
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
                                            </tr>
                                            <tr>
                                                <td colspan="7" align="center">
                                                    <br />
                                                 
                                                    <asp:CheckBox ID="cbxAfirmarTexto"  runat="server" Text="" Font-Size="12" 
                                                        Font-Bold="True" Font-Names="Verdana" 
                                                         onChange = "javascript:check_AprobarVistaPrevia();" 
                                                        OnCheckedChanged="cbxAfirmarTexto_CheckedChanged" 
                                                       style="display:none"
                                                        EnableViewState="true" 
                                                        ViewStateMode="Enabled"  
                                                        AutoPostBack="True" />

                                                        <asp:Button ID="Btn_AfirmarTextoLeido" 
                                                        style="width:120px;"
                                                        OnClick="btnAfirmarTextoLeido_click" 
                                                        Text="Leído y Conforme" runat="server" AutoPostBack="True" CssClass="btnAlmacenAcepta"  />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                                                


                    </div>
                    <div id="tabs-4">
                        <asp:UpdatePanel ID="updRegPago" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="13">
                                            <toolbar:toolbarcontent ID="ctrlToolBar5" runat="server"></toolbar:toolbarcontent>                                            
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCancelarAprobacion" runat="server" 
                                                Text="     Cancelar Aprobación" class="btnUndo"
                                            style="width: 165px;" onclick="btnCancelarAprobacion_Click"/>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="7" rowspan="2">
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000;">
                                                <tr>
                                                    <td>
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
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top;">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="Txt_TarifaId" runat="server" Width="38px" ToolTip="Coloque el numero de la seccion del tarifario"
                                                                                    MaxLength="4" TabIndex="1" CssClass="campoNumero" OnTextChanged="Txt_TarifaId_TextChanged"
                                                                                    AutoPostBack="True" />
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
                                                                                        <asp:TextBox ID="Txt_TarifaDescripcion" runat="server" Width="599px" Enabled="False"
                                                                                            CssClass="txtLetra" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/img_16_search.png"
                                                                                            ToolTip="Buscar" Height="18px" OnClick="ImageButton1_Click" Visible="false" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ListBox ID="Lst_Tarifario" runat="server" Width="626px" AutoPostBack="True"
                                                                                            BackColor="#EAFFFF" OnSelectedIndexChanged="Lst_Tarifario_SelectedIndexChanged" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ibRecargar" runat="server" ImageUrl="~/Images/img_32_refresh.png"
                                                                                            ToolTip="Recargar" Height="18px" onclick="ibRecargar_Click"  />
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
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label53" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTarifaConsular1" runat="server" Text="Derecho Proporcional:" 
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_TarifaProporcional" runat="server" Width="95px" ToolTip="Coloque el número de la sección del tarifario"
                                                MaxLength="6" CssClass="campoNumero" Enabled="False" Visible="False" />
                                        </td>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblTarifaConsular2" runat="server" Text="Cantidad:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_TarifaCantidad" runat="server" Width="95px" ToolTip="Coloque el numero de la sección del tarifario"
                                                Text="1" MaxLength="6" CssClass="campoNumero" OnTextChanged="Txt_TarifaCantidad_TextChanged"
                                                onkeypress="javascript:validarNumero();" 
                                                AutoPostBack="True" />
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblLeyenda" runat="server" Text=" (Presionar tecla ENTER para calcular total)"
                                                Font-Bold="true" Font-Size="X-Small" Width="160px"></asp:Label>
                                            <asp:Button ID="Btn_AgregarTarifa" runat="server" Text="   Agregar" Width="100px"
                                                TabIndex="2" CssClass="btnNewActuacion" OnClick="Btn_AgregarTarifa_Click" />
                                            <asp:Button ID="btnLimpiarTarifa" runat="server" Text="   Limpiar" CssClass="btnLimpiar"
                                                OnClick="btnLimpiarTarifa_Click" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMtoSC" runat="server" Text="Monto S/C:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_MtoSC" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                        </td>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblMtoEn" runat="server" Text="Monto en:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_MontML" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="LblDescMtoML" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTOTALSC" runat="server" Text="TOTAL S/C: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_TotSC" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                        </td>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblTotalEn" runat="server" Text="TOTAL en:" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_TotML" runat="server" Width="95px" Enabled="False" CssClass="campoNumero" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="LblDescTotML" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="3">
                                            <asp:HiddenField ID="hdn_tari_sTarifarioId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_tari_sNumero" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_tari_vLetra" runat="server" Value="" />
                                            <asp:HiddenField ID="hdn_tari_sCalculoTipoId" runat="server" Value="0" />
                                        </td>
                                        <td colspan="3">
                                            <asp:HiddenField ID="hdn_cant_fojas_escritura" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdn_cant_fojas_parte" runat="server" Value="0" />                                            
                                            <asp:HiddenField ID="hdn_cant_fojas_testimonio" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td colspan="8">
                                            <asp:GridView ID="Gdv_Tarifa" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="True" Width="100%"
                                                OnRowCommand="Gdv_Tarifa_RowCommand" OnRowDataBound="Gdv_Tarifa_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField HeaderText="acde_iActuacionDetalleId" DataField="acde_iActuacionDetalleId"
                                                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="acde_sTarifarioId" DataField="acde_sTarifarioId" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="R.G.E." DataField="acde_ICorrelativoActuacion">
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Correlativo Tarifa" DataField="acde_ICorrelativoTarifario">
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Fecha" DataField="acde_dFechaRegistro" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Tarifa" DataField="tari_vNumero">
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="P. S/C" DataField="pago_FMontoSolesConsulares">
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="P. Moneda Local" DataField="pago_FMontoMonedaLocal">
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEliminar3" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="javascript:CheckPago(this);" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle />
                                                <PagerStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" colspan="6" align="right">
                                            <asp:Label ID="Label52" runat="server" Text="Total General en S/C:" Style="font-weight: 700"></asp:Label>
                                        </td>
                                        <td valign="top" colspan="3" align="center" style="border-top-color: #800000; width: 100%;">
                                            <table>
                                                <tr>
                                                    <td style="width: 180px">
                                                    </td>
                                                    <td valign="top" align="center">
                                                        <asp:Label ID="Lbl_TotalGeneral" runat="server" Text="0.00" Font-Bold="true" Width="100px"></asp:Label>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <asp:Label ID="Lbl_TotalExtranjera" runat="server" Text="0.00" Font-Bold="true" Width="100px"></asp:Label>
                                                    </td>
                                                    <td style="width: 60px">
                                                    </td>
                                                    <td style="width: 60px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                                border-bottom-color: #800000;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="REGISTRAR PAGO" Style="font-weight: 700;
                                                            color: #800000;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipPago1" runat="server" Text="Tipo Pago:"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlTipoPago" runat="server" Height="22px" Width="180px"  AutoPostBack="true" style="cursor:pointer"
                                                TabIndex="3" onselectedindexchanged="ddlTipoPago_SelectedIndexChanged" />
                                            <p style="color: Red; display: inline; background: White;">
                                                *</p>
                                        </td>
                                        <td align="right" style="width:70px">
                                            <asp:Label ID="lblExoneracion" runat="server" Text="Ley que exonera el pago:"></asp:Label> 
                                        </td>
                                        <td>
                                        <asp:DropDownList ID="ddlExoneracion" runat="server"  style="cursor:pointer; margin-left: 0px;"
                                                Enabled="True" Height="20px" 
                                                Width="414px" />                                            
                                            <asp:Label ID="lblValExoneracion" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            <asp:RadioButton ID="RBNormativa" runat="server" Text=""  style="cursor:pointer"
                                                GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()"/>
                                                
                                        </td>
                                        <td style="width: 20px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblTipPago2" runat="server" Text="Nro Voucher:" Visible="False"></asp:Label>
                                        </td>
                                           <td colspan="3">
                                          <asp:TextBox ID="Txt_VoucherNro" runat="server" Width="120px" MaxLength="20" TabIndex="4"
                                                onkeyup="valid(this,'special')" onblur="valid(this,'special')" 
                                                CssClass="campoNumero" Visible="False"></asp:TextBox>
                                              <asp:Label ID="Label24" runat="server" 
                                                   style="color:Red;display:inline;background:white;" Text="*" Visible="False"></asp:Label>

                                        </td>
                                        <td align="right" style="width:70px">
                                            <asp:Label ID="lblSustentoTipoPago" runat="server" Text="Sustento:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSustentoTipoPago" runat="server" Width="410px" CssClass="txtLetra" ></asp:TextBox>
                                            <asp:Label ID="lblValSustentoTipoPago" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text=""  style="cursor:pointer"
                                                GroupName="GrupoExonera" Width="15px" onclick="ActivarLeySustento()"/>
                                                
                                        </td>
                                        <td style="width: 20px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" id="pnlPagLima" runat="server" visible="false">
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
                                                        <asp:DropDownList ID="ddlNomBanco" runat="server" Width="300px" MaxLength="10" style="cursor:pointer"></asp:DropDownList>
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
                                                        <asp:Label ID="lblMtoCancelado" runat="server"  Visible="false" Text="Monto Cancelado :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMtoCancelado" runat="server" Width="150px"     Visible="false" 
                                                            CssClass="campoNumero" Enabled="False" />
                                                        <asp:Label ID="lblCO_txtMtoCancelado" runat="server" Text="*"   Visible="false"  Style="color: #FF0000"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>                                                    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                        </td>
                                        <td colspan="4" style="text-align: center">
                                            &nbsp;
                                        </td>
                                        <td style="text-align: center">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlNuevoFormato" runat="server" Visible="false">
                                <table style="width:100%">
                                <tr>
                                    <td colspan="5">
                                        <table style="border-bottom: 1px solid #800000; border-top: 1px solid #800000; width: 100%;
                                            border-bottom-color: #800000;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label36" runat="server" Text="DATOS FORMATO ADICIONAL" Style="font-weight: 700;
                                                        color: #800000;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label41" runat="server" Text="Funcionario Responsable:"   />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAdicionalFuncionario" runat="server" Height="21px" Width="300px" style="cursor:pointer"
                                            AutoPostBack="false"  >
                                        </asp:DropDownList>
                                        <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    <td style="width:25px"></td>
                                    <td style="width:145px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5"><h4>Presentante</h4></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAdicionalTipoDoc" runat="server" Text="Tipo Documento:"   />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAdicionalTipoDoc" runat="server" Height="21px" Width="300px" style="cursor:pointer"
                                            AutoPostBack="false"  >
                                        </asp:DropDownList>
                                        <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    <td style="width:25px"></td>
                                    <td style="width:145px">
                                        <asp:Label ID="lblAdicionalNumDoc" runat="server" Text="Número Documento:"  />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAdicionalNumDoc" runat="server" Width="192px"  onBlur="conMayusculas(this)" onkeypress="return isValidarNroDocumentoPresentanteAdi()"
                                            CssClass="txtLetra" Visible="true" />
                                        <p style="color: Red; display: inline; background: White;">*</p>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAdicionalNombres" runat="server" Text="Nombres y Apellidos:"  />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdicionalNombres" runat="server" Width="292px" onBlur="conMayusculas(this)" CssClass="txtLetra" Visible="true" />
                                            <p style="color: Red; display: inline; background: White;">*</p>
                                        </td>
                                        <td style="width:25px"></td>
                                        <td>
                                        <asp:Label ID="lblAdicionalGenero" runat="server" Text="Género:"   />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAdicionalGenero" runat="server" Width="200px" style="cursor:pointer">
                                            </asp:DropDownList>
                                            <p style="color: Red; display: inline; background: White;">*</p>
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-5">                        
                        <asp:UpdatePanel ID="updFormato" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button Text="    Grabar" CssClass="btnSave" runat="server" ID="btnGrabarVinculacion" TabIndex="1"
                                                OnClick="btnGrabarVinculacion_Click" OnClientClick="return ValidarDatosVinculacion();" />
                                               <asp:HiddenField ID="HFGUID" runat="server" />

                                                <uc4:ctrlReimprimirbtn ID="ctrlReimprimirbtn1" runat="server" />
                                                    <uc6:ctrlBajaAutoadhesivo ID="ctrlBajaAutoadhesivo1" runat="server" />
                                        </td>                                                                                                                     
                                    </tr>
                                    
                                </table>

                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <h4>
                                        <asp:Label ID="Label33" runat="server" Text="PASO 1: Vinculación Autoadhesivo"></asp:Label></h4>
                                </td>
                                
                                <td>
                                    <h4><asp:Label ID="Label31" runat="server" Text="PASO 2: Vista Previa e Impresión"></asp:Label></h4>
                                </td>
                                <td>
                                     <h4><asp:Label ID="Label32" runat="server" Text="PASO 3: Aprobación Impresión"></asp:Label></h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label ID="lblNumeroOficioPaso" runat="server" Text="PASO 4: Nro. de Oficio" Visible="false"></asp:Label></h4>
                                </td>
                                <td></td>
                            </tr>                            
                            <tr>
                                 <td>
                                    <asp:Label ID="Label34" runat="server" Text="Código:"></asp:Label>                                    
                                </td>
                                <td>
                                   <asp:TextBox ID="txtCodigoInsumo" runat="server" Width="120px" MaxLength="14" CssClass="txtLetra"
                                        onkeypress="return isLetraNumeroDoc(event);" TabIndex="6"></asp:TextBox>
                                        
                                    <asp:Label ID="Label35" runat="server" Text="*" Style="color: #FF0000"></asp:Label> 
                                    <asp:HiddenField ID="hnd_ImpresionCorrecta" runat="server" Value="0" />
                                </td>

                                <td>
                                    <asp:Button ID="btnAutoadhesivo" runat="server" Text="   Autoadhesivo" 
                                        Width="200px" CssClass="btnPrint" onclick="btnAutoadhesivo_Click"  TabIndex="4" />
                                        <asp:HiddenField ID="hCodAutoadhesivo" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkImpresionCorrecta" runat="server" Text="Impresión Correcta" 
                                        Enabled="False"  AutoPostBack="true" TabIndex="5" />
                                </td>
                                        
                                <td>
                                        <asp:TextBox ID="txtNumeroOficioAdi" runat="server" Width="100px" Visible="false"
                                        MaxLength="5" onkeypress="isNumberKey(event);" TabIndex="7"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                               <td align="center" colspan="2">
                               
                                 <asp:Button ID="btnLimpiarVinc" runat="server" Text=" Limpiar" TabIndex="2"
                                                CssClass="btnLimpiar"  onclick="btnLimpiarVinc_Click" /> 
                               </td>
                                
                                <td>                                        
                                    <asp:Button ID="btnFormato" runat="server" Text="   Parte" Width="200px" 
                                        CssClass="btnPrint" Visible="false" 
                                        OnClientClick="return tabImprimirVistaPrevia('0');" TabIndex="8" 
                                        />
                                </td>
                                <td>                                        
                                    
                                </td>
                                <td>
                                     <asp:Button ID="btnDesabilitarAutoahesivo" runat="server" onclick="btnDesabilitarAutoahesivo_Click" Text="Oculto" Visible="true" CssClass="hideControl" />                                                                                                         
                                <asp:HiddenField ID="HF_ValidaTablaFuncionarioResponsable" runat="server" />
                                <table id="tabFuncionarioResponsable" style="display: none;">
                                    <tr>
                                        <td>
                                             <asp:Label ID="Label18" runat="server" Text="Funcionario Responsable:"></asp:Label>   
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlFuncionarioResponsable" runat="server"   style="cursor:pointer"/>
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
                                <td>                                                                        
                                </td>
                                <td>
                                <asp:Button ID="btn_confirmar" runat="server" Text="   Acta de Conformidad" Width="200px" TabIndex="3"
                                                CssClass="btnPrint" onclick="btn_confirmar_Click" />
                                </td>
                                <td></td>
                                <td>
                                
                                </td>
                                <td>
                                
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:HiddenField ID="hdn_seleccionado" runat="server" Value="-1" />
                                    <asp:HiddenField ID="hdn_tipo_formato_proto" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdn_correlativo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdn_actonotarialdetalle_id" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdn_actuacion_id" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdn_actuaciondetalle_id" runat="server" Value="0" />
                                    
                                    <asp:HiddenField ID="hdn_acno_iActuacionId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdn_vActuacionDetalleIds" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="gdvVinculacion" runat="server" CssClass="mGrid" 
                                        AlternatingRowStyle-CssClass="alt" GridLines="None" DataKeyNames="acde_vNotas, insu_vCodigoUnicoFabrica"
                                        AutoGenerateColumns="False" onrowcommand="gdvVinculacion_RowCommand" 
                                        TabIndex="10" >
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <%--Posición 0--%>
                                            <asp:BoundField DataField="ande_iActoNotarialDetalleId" HeaderText="ande_iActoNotarialDetalleId" 
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta"/>
                                            </asp:BoundField>
                                            <%--Posición 1--%>
                                            <asp:BoundField DataField="acde_iActuacionId" HeaderText="acde_iActuacionId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <%--Posición 2--%>
                                            <asp:BoundField DataField="ande_iActuacionDetalleId" HeaderText="ande_iActuacionDetalleId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>
                                            <%--Posición 3--%>
                                            <asp:BoundField DataField="ande_sTipoFormatoId" HeaderText="ande_sTipoFormatoId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>                         
                                            <%--Posición 4--%>                   
                                            <asp:BoundField HeaderText="Tipo de Formato" DataField="ande_vTipoFormato">
                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%--Posición 5--%>
                                            <asp:BoundField HeaderText="Ítem" DataField="ande_sCorrelativo">
                                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Fecha y Hora" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" DataField="aide_dFechaVinculacion">
                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>                                            
                                            <asp:BoundField HeaderText="Usuario" DataField="usua_vAlias">
                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%--Posición 8--%>
                                            <asp:BoundField HeaderText="Nro. Oficio" DataField="ande_vNumeroOficio">
                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%--Posición 9--%>
                                            <asp:BoundField  HeaderText="Impresión Correcta" DataField="aide_bFlagImpresion">
                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%--Posición 10--%>
                                            <asp:BoundField HeaderText="Código de Insumo" DataField="insu_vCodigoUnicoFabrica">
                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%--Posición 11--%>
                                             <asp:BoundField DataField="insu_iInsumoId" HeaderText="insu_iInsumoId"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
                                            </asp:BoundField>  
                                            <%--Posición 12--%>
                                             <asp:BoundField DataField="acde_vNotas" HeaderText="acde_vNotas"
                                                HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                <ItemStyle CssClass="ColumnaOculta" />
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
                                <td colspan="6">
                                    <uc2:ctrlPageBar ID="CtrlPageBarActuacionInsumoDetalle" runat="server" OnClick="ctrlPagActuacionInsumoDetalle_Click"
                                                Visible="false" />
                                </td>
                            </tr>
                        </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </div>
                    <div id="tabs-6">
                        <asp:UpdatePanel ID="updComandosTab6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="10">
                                            <asp:Button ID="btnSave_tab6" runat="server" Text="    Grabar" CssClass="btnSave" Style="width: 91px;" />
                                            
                                            <asp:Button ID="btnCancelarTab6" runat="server" Text="    Limpiar" CssClass="btnLimpiar"
                                                OnClick="btnCancelarTab6_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdMsjeArchivoDigital" colspan="10">
                                            <asp:Label ID="lblValidacionArchivoDigital" runat="server" Text="Debe ingresar los campos requeridos"
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:Label>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>                                            
                                            <asp:Label ID="lblDescAdj" runat="server" Text="Descripción :" Width="76px"></asp:Label></td>
                                        <td>
                                            <table width="100%">
                                            <tr align="left"><td>                                            
                                                <asp:TextBox ID="Txt_AdjuntoDescripcion" runat="server" Width="537px" CssClass="txtLetra"
                                                    onkeypress="isDescripcion(event);" MaxLength="300" TabIndex="2" />
                                                <asp:Label ID="lblAstD" runat="server" Text="*" Style="color: #FF0000"></asp:Label>
                                            </td></tr>
                                            </table>    
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
                                        <td align="left">
                                            <uc2:ctrlUploader ID="ctrlUploader2" runat="server" FileExtension=".pdf" FileSize="102400"
                                                Width="2500" OnClick="MyUserControlUploader2Event_Click" />
                                            <asp:HiddenField ID="hidNomAdjFile2" runat="server" />
                                            
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
                                            <asp:Label ID="Label22" runat="server" Text="Solo se permiten guardar archivos de Tipo PDF de un tamaño Max. de 1024 kb"
                                                Font-Bold="True"></asp:Label>
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
                                                Width="95%">
                                                <AlternatingRowStyle CssClass="alt " />
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
                                                    <asp:BoundField DataField="ando_vRutaArchivo" HeaderText="Nombre Archivo">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ando_vDescripcion" HeaderText="Descripción">
                                                        <ItemStyle Width="450px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Vista Previa" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnPrint" CommandName="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_preview.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>
                                                </Columns>
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
                </div>
            </td>
        </tr>
    </table>
    <br />


    <div id="msg-dialog">
    
    </div>
    <script type="text/javascript" language="javascript">

        function validarNumero() {
            $("#<%=Txt_TarifaCantidad.ClientID %>").numeric();
        }


        function isSessionTimeOut() {
            var rpt = 'ok';
            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/isSessionTimeOut",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cancel: false,
                success: function (data) {
                    var bol = data.d

                    if (bol == false) {
                        location.href = "../Cuenta/FrmLogin.aspx";
                        rpt = 'error';
                    } else {
                        return true;
                    }

                },
                failure: function (response) {
                    showdialog('e', 'ACTO PROTOCOLAR :', response.d, false, 160, 300);
                }
            });


            return rpt;

        }

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
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

        function validaSiEsNumero(evt) {
            var theEvent = evt || window.event;

            // Handle paste
            if (theEvent.type === 'paste') {
                key = event.clipboardData.getData('text/plain');
            } else {
                // Handle key press
                var key = theEvent.keyCode || theEvent.which;
                key = String.fromCharCode(key);
            }
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
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

        function isDescripcion(evt) {
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
            var letras = "0123456789áéíóúÑÁÉÍÓÚ°.():,";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

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

            return letra;
        }
         function ValidarCamposRUNE(evt) {
            var bolValida = isSujeto(evt);
            if (!bolValida) {
                bolValida = validarcaracterrune();
            }
            return bolValida;
        }
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

        function isValidarAutorizacionNroDocumento() {
            var strTipoDoc = "1";

            if (strTipoDoc == 0)
                event.returnValue = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");
            document.getElementById("<%=txtAutorizacionNroDocumento.ClientID %>").maxLength = valoresDocumento[1];

            if (valoresDocumento[2] == "True") { //Acepta Solo Numeros

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
        
        function isValidarNroDocumento() {


            var strTipoDoc = $.trim($("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val());

            if (strTipoDoc == 0)
                event.returnValue = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");
            document.getElementById("<%=txt_peid_vDocumentoNumero.ClientID %>").maxLength = valoresDocumento[1];

            if (valoresDocumento[2] == "True") { //Acepta Solo Numeros

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

        function isValidarNroDocumentoPresentante() {
            var strTipoDoc = $.trim($("#<%= ddl_TipoDocrepresentante.ClientID %>").val());

            if (strTipoDoc == 0)
                event.returnValue = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");
            document.getElementById("<%=txtRepresentanteNroDoc.ClientID %>").maxLength = valoresDocumento[1];

            if (valoresDocumento[2] == "True") {

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

        function isValidarNroDocumentoPresentanteAdi() {
            var strTipoDoc = $.trim($("#<%= ddlAdicionalTipoDoc.ClientID %>").val());

            if (strTipoDoc == 0)
                event.returnValue = false;

            var valoresDocumento = ObtenerMaxLenghtDocumentos(strTipoDoc).split(",");
            document.getElementById("<%=txtAdicionalNumDoc.ClientID %>").maxLength = valoresDocumento[1];

            if (valoresDocumento[2] == "True") { //Acepta Solo Numeros



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


        function ObtenerMaxLenghtDocumentos(id) {

            var hfDocumentoIdentidad = $("#MainContent_HF_ValoresDocumentoIdentidad").val()

            var documentos = hfDocumentoIdentidad.split("|");

            for (i = 0; i < documentos.length - 1; i++) {

                var valores = documentos[i].split(",");

                if (valores[0] == id) {
                    return valores[1] + "," + valores[2] + "," + valores[3] + "," + valores[5];
                }

            }

            return "";



        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }



        function ObtenerEstado() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;


            var hdn_acno_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
            var hdn_acno_iActuacionId = $("#<%=hdn_acno_iActuacionId.ClientID %>").val();
            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/Consultar_Estado",
                data: '{hdn_acno_iActoNotarialId: ' + hdn_acno_iActoNotarialId + ', hdn_acno_iActuacionId: ' + hdn_acno_iActuacionId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (resp) {

                    var oEstado = JSON.parse(resp.d);
                    $("#<%= hdn_acno_estadoActoProtocolar.ClientID %>").val(oEstado);
                },

                error: errores

            });
        }


        function pageLoadedHandler() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var hdn_acno_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
            var hdn_acno_iActuacionId = $("#<%=hdn_acno_iActuacionId.ClientID %>").val();
            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
            

            for (var i = 0; i < 6; i++) {
                Desabilitar_Tab(i);
            }

            if (hdn_acno_iActoNotarialId.trim() == "") {
                hdn_acno_iActoNotarialId = "0";
            }

            if (parseInt(hdn_acno_iActoNotarialId) == 0) {
                Habilitar_Tab(0);
                var i = 0;
                for (i = 1; i < 6; i++) {
                    Desabilitar_Tab(i);
                }
                
            }
            else {
            

                var hdn_Accion = $("#<%=hdn_AccionOperacion.ClientID %>").val();
                if (hdn_Accion == "1058") {

                    Habilitar_Tab(0);
                    Habilitar_Tab(1);
                    Habilitar_Tab(2);
                    Habilitar_Tab(3);

                    disableElements($('#tabs-1').children());
                    disableElements($('#tabs-2').children());
                    disableElements($('#tabs-3').children());

                    $("#accordion").prop("disabled", false);

                    for (var i = 4; i < 6; i++) {
                        Desabilitar_Tab(i);
                    }

                    Mover_TabIndex(3);

                    for (var i = 0; i < 6; i++) {
                        Desabilitar_Tab(i);
                    }
                }
                else {



                    $.ajax({
                        type: "POST",
                        url: "FrmActoNotarialProtocolares.aspx/Consultar_Estado",
                        data: '{hdn_acno_iActoNotarialId: ' + hdn_acno_iActoNotarialId + ', hdn_acno_iActuacionId: ' + hdn_acno_iActuacionId + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (resp) {
                            
                            switch (resp.d) {
                                case "98":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);

                                    for (var i = 2; i < 6; i++) {
                                        Desabilitar_Tab(i);
                                    }

                                    $("#btnCncl_tab1").prop("disabled", "disabled");

                                    Mover_TabIndex(1);

                                    break;
                                case "99":

                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);

                                    for (var i = 3; i < 6; i++) {
                                        Desabilitar_Tab(i);
                                    }

                                    $("#btnCncl_tab1").prop("disabled", "disabled");

                                    Mover_TabIndex(2);

                                    break;
                                case "100":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);

                                    for (var i = 4; i < 6; i++) {
                                        Desabilitar_Tab(i);
                                    }

                                    $("#btnCncl_tab1").prop("disabled", "disabled");

                                    Mover_TabIndex(2);

                                    break;
                                case "101":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);
                                    Habilitar_Tab(3);

                                    for (var i = 5; i < 6; i++) {
                                        Desabilitar_Tab(i);
                                    }

                                    Mover_TabIndex(3);

                                    disableElements($('#tabs-1').children());
                                    disableElements($('#tabs-2').children());

                                    //$("#btnSave_tab3").prop("disabled", "disabled");
                                    //$("#btnTraerCuerpo").prop("disabled", "disabled");
                                    //$("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");

                                    $("#btnCncl_tab3").prop("disabled", "disabled");
                                    $("#<%=txtNroEscritura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroLibro.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaIni.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaFinal.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtImagenTitulo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=FileUploadImagen.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnAgregarImagen.ClientID %>").prop("disabled", "disabled");
                                    
                                    
                                    $("#<%=txtTextoNormativo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=divListNormativo.ClientID %>").addClass("disableElementsOfDiv");

                                    $("#<%=txtNroOficio.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRegistradorNombres.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtRepresentanteNombres.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_TipoDocrepresentante.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRepresentanteNroDoc.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_GerenoPresentante.ClientID %>").prop("disabled", "disabled");


                                    $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");
                                    break;
                                case "102":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);
                                    Habilitar_Tab(3);
                                    Habilitar_Tab(4);

                                    for (var i = 6; i < 6; i++) {
                                        Desabilitar_Tab(i);
                                    }

                                    Mover_TabIndex(4);

                                    disableElements($('#tabs-1').children());
                                    disableElements($('#tabs-2').children());

                                    //$("#btnSave_tab3").prop("disabled", "disabled");
                                    $("#btnTraerCuerpo").prop("disabled", "disabled");
                                    //$("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");

                                    $("#btnCncl_tab3").prop("disabled", "disabled");
                                    $("#<%=txtNroEscritura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroLibro.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaIni.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaFinal.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtImagenTitulo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=FileUploadImagen.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnAgregarImagen.ClientID %>").prop("disabled", "disabled");
                                   
                                    $("#<%=txtTextoNormativo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=divListNormativo.ClientID %>").addClass("disableElementsOfDiv");

                                    $("#<%=txtNroOficio.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRegistradorNombres.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtRepresentanteNombres.ClientID %>").prop("disabled", "disabled");


                                    $("#<%=ddl_TipoDocrepresentante.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRepresentanteNroDoc.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_GerenoPresentante.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");

                                    disableElements($('#tabs-4').children());
                                    break;
                                case "103":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);
                                    Habilitar_Tab(3);
                                    Habilitar_Tab(4);
                                    Habilitar_Tab(5);

                                    Mover_TabIndex(5);

                                    disableElements($('#tabs-1').children());
                                    disableElements($('#tabs-2').children());

                                    //$("#btnSave_tab3").prop("disabled", "disabled");
                                    $("#btnTraerCuerpo").prop("disabled", "disabled");
                                    //$("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");

                                    $("#btnCncl_tab3").prop("disabled", "disabled");
                                    $("#<%=txtNroEscritura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroLibro.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaIni.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaFinal.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtImagenTitulo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=FileUploadImagen.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnAgregarImagen.ClientID %>").prop("disabled", "disabled");
                                    
                                    $("#<%=txtTextoNormativo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=divListNormativo.ClientID %>").addClass("disableElementsOfDiv");

                                    disableElements($('#tabs-4').children());

                                    $("#<%=btnGrabarVinculacion.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnLimpiarVinc.ClientID %>").prop("disabled", "disabled");//vpipa

                                    $("#<%=txtNroOficio.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRegistradorNombres.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtRepresentanteNombres.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_TipoDocrepresentante.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRepresentanteNroDoc.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_GerenoPresentante.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");

                                    break;
                                case "104":
                                    Habilitar_Tab(0);
                                    Habilitar_Tab(1);
                                    Habilitar_Tab(2);
                                    Habilitar_Tab(3);
                                    Habilitar_Tab(4);
                                    Habilitar_Tab(5);

                                    Mover_TabIndex(5);

                                    disableElements($('#tabs-1').children());
                                    disableElements($('#tabs-2').children());

                                    //$("#btnSave_tab3").prop("disabled", "disabled");
                                    $("#btnTraerCuerpo").prop("disabled", "disabled");
                                    //$("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");

                                    $("#btnCncl_tab3").prop("disabled", "disabled");
                                    $("#<%=txtNroEscritura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroLibro.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaIni.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtNroFojaFinal.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtImagenTitulo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=FileUploadImagen.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnAgregarImagen.ClientID %>").prop("disabled", "disabled");
                                    
                                    $("#<%=txtTextoNormativo.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=divListNormativo.ClientID %>").addClass("disableElementsOfDiv");

                                    $("#<%=txtNroOficio.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRegistradorNombres.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txtRepresentanteNombres.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_TipoDocrepresentante.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtRepresentanteNroDoc.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=ddl_GerenoPresentante.ClientID %>").prop("disabled", "disabled");

                                    $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");

                                    disableElements($('#tabs-4').children());

                                    $("#<%=btnGrabarVinculacion.ClientID %>").prop("disabled", "disabled");
                                    $("#<%=btnLimpiarVinc.ClientID %>").prop("disabled", "disabled"); //vpipa

                                    $("#btnSave_tab6").prop("disabled", "disabled");

                                    $("#<%=btnCancelarTab6.ClientID %>").attr('disabled', true);
                                    

                                    

                                    break;
                            }
                        },

                        error: errores

                    });

                }
            }
        }

        function HabilitarTabPagos() {
            Habilitar_Tab(3);
            Mover_TabIndex(3);
            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");

            $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
        }

        function HabilitarTabVinculacion() {
            Habilitar_Tab(4);
            Mover_TabIndex(4);
        }

        function HabilitarTabDigitalizac() {
            Habilitar_Tab(5);
            Mover_TabIndex(5);
        }

        function Habilitar_Tab(i_Index) {
            $('#tabs').enableTab(i_Index);
        }
        function Desabilitar_Tab(i_Index) {
            $('#tabs').disableTab(i_Index);
        }

        function Mover_TabIndex(i_Index) {
            $('#tabs').tabs("option", "active", i_Index);
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function enableElements(el) {
            for (var i = 0; i < el.length; i++) {
                el[i].disabled = false; enableElements(el[i].children);
            }
        }

        function disableElements(el) {
            for (var i = 0; i < el.length; i++) {
                el[i].disabled = true; disableElements(el[i].children);
            }
        }

        function DeshabilitaElementosTabs() {
            disableElements($('#tabs-1').children());
            disableElements($('#tabs-2').children());

            // vpipa central
            //tinyMCE.editors[0].getBody().setAttribute('contenteditable', false);
            //tinyMCE.editors[0].getDoc().designMode = 'Off';

            //$("#btnSave_tab3").prop("disabled", "disabled");
            //$("#btnTraerCuerpo").prop("disabled", "disabled");
            $("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");
            
            $("#btnCncl_tab3").prop("disabled", "disabled");
            $("#<%=txtNroEscritura.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtNroLibro.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtNroFojaIni.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtNroFojaFinal.ClientID %>").prop("disabled", "disabled");

            $("#<%=txtImagenTitulo.ClientID %>").prop("disabled", "disabled");
            $("#<%=FileUploadImagen.ClientID %>").prop("disabled", "disabled");
            $("#<%=btnAgregarImagen.ClientID %>").prop("disabled", "disabled");
            
            $("#<%=txtTextoNormativo.ClientID %>").prop("disabled", "disabled");
            $("#<%=divListNormativo.ClientID %>").addClass("disableElementsOfDiv");

            $("#<%=txtNroOficio.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtRegistradorNombres.ClientID %>").prop("disabled", "disabled");

            $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").prop("disabled", "disabled");

            $("#<%=txtRepresentanteNombres.ClientID %>").prop("disabled", "disabled");
            $("#<%=ddl_TipoDocrepresentante.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtRepresentanteNroDoc.ClientID %>").prop("disabled", "disabled");
            $("#<%=ddl_GerenoPresentante.ClientID %>").prop("disabled", "disabled");

            /*----Datos MINUTA------*/
            $("#<%=txt_acno_Numero_Minuta.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").prop("disabled", "disabled");
            $("#<%=txtAutorizacionNroDocumento.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_acno_vNombreColegiatura.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").prop("disabled", "disabled");
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").prop("disabled", "disabled");
            /*---------------------*/

            disableElements($('#tabs-4').children());
        }

        var isConsultar = $("#<%= hdn_AccionConsultar.ClientID%>").val();
        var isOperacion = $("#<%= hdn_AccionOperacion.ClientID%>").val();

        if (isConsultar == isOperacion) {
            isConsulta();
        }
        function isConsulta() {
            disableElements($('#tabs-1').children());
            disableElements($('#tabs-2').children());
            disableElements($('#tabs-3').children());
            disableElements($('#tabs-4').children());
            disableElements($('#tabs-5').children());
            disableElements($('#tabs-6').children());
        }

        function DeshabilitaElementosTabDigitalizacion() {

            $("#btnSave_tab6").removeAttr('enabled');
            $("#btnSave_tab6").prop("disabled", "disabled");
            $("#<%=btnCancelarTab6.ClientID %>").prop("disabled", "disabled");
            $("#<%=Txt_AdjuntoDescripcion.ClientID %>").prop("disabled", "disabled");

            
            $("#btnSave_tab6").prop("disabled", "disabled");
            $("#<%=Gdv_Adjunto.ClientID %>").prop("disabled", "disabled");
        }


        function funSetSession(variable, valor) {
            var url = 'FrmActoNotarialProtocolares.aspx/SetSession';
            var prm = {};
            prm.variable = variable;
            prm.valor = valor;
            var rspta = execute(url, prm);
        }

        function funGetSession(variable) {
            var url = 'FrmActoNotarialProtocolares.aspx/GetSession';
            var prm = {};
            prm.variable = variable;
            var rspta = execute(url, prm);
            return JSON.parse(JSON.stringify(rspta)).d;
        }

        function SetLabelActoNotarial(texto) {
            document.getElementById('<%= lblTipoActoNotarialEP.ClientID %>').innerText = texto;

            //            var tipoActoProtocolar = $('#<%=Cmb_TipoActoNotarial.ClientID %>').val();
            //            var strTipoActoProtocolarCompraVenta = $("#<%= HF_TIPOACTO_COMPRA_VENTA.ClientID %>").val();

            //            if (tipoActoProtocolar == strTipoActoProtocolarCompraVenta) {
            //                $('#<%=ddl_anpa_sTipoParticipanteId.ClientID %> option[value="8417"]').remove();

            //            }


        }
        function tab_01() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var iActuacionId = $("#<%=hdn_acno_iActuacionId.ClientID %>").val();
            var iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
            if (iActoNotarialId != 0) {
                var prm = {};
                prm.iActuacionId = iActuacionId;
                prm.iActoNotarialId = iActoNotarialId;

                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/tab_registro",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rspt) {
                        var lACTONOTARIAL = $.parseJSON(rspt.d);
                        $("#<%=Cmb_TipoActoNotarial.ClientID %>").val(lACTONOTARIAL.acno_sSubTipoActoNotarialId);
                        $("#<%=Txt_EscrituraAnterior.ClientID %>").val(lACTONOTARIAL.acno_vNumeroEscrituraPublica);
                        $("#<%=Txt_Denominacion.ClientID %>").val(lACTONOTARIAL.acno_vDenominacion);
                        $("#<%=chk_acno_bFlagMinuta.ClientID %>").val(lACTONOTARIAL.acno_bFlagMinuta);
                    },
                    error: errores
                });
            }

        }

        function grabar_tab_01() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            if ($("#<%= hdn_actu_iPersonaRecurrenteId.ClientID %>") == "0") {
                alert('Error desde actuación');
                return;
            }

            //  vpipa en caso que no hay nigun participantes, adicionar por defecto, esto ocurre cuando eliminan 
            //  a los participantes en la pestaña de participantes luego regresa a la pestaño registro y vuelven a grabar
            var ExiteElParticipante = ExisteParticipanteXDefecto($("#<%= HF_vNroDocumento.ClientID %>").val());
            if (VacioGrdParticipantes() || ExiteElParticipante==false) {
                var DocumentoTipo = $("#<%= HF_vDescTipDoc.ClientID %>").val();
                var DocumentoNumero = $("#<%= HF_vNroDocumento.ClientID %>").val();
                var sTipoParticipanteId = $("#<%= HF_sTipoParticipanteId.ClientID %>").val();
                $("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val(DocumentoTipo);
                $("#<%= txt_peid_vDocumentoNumero.ClientID %>").val(DocumentoNumero);
                $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val(sTipoParticipanteId);
            }
            //---------------

            $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", true);
            $("#<%=cbxAfirmarTexto.ClientID %>").prop("disabled", true);
            document.getElementById('MainContent_cbxAfirmarTexto').parentElement.disabled = true;
            
            
            if (ValidarTabRegistro()) {
                var prm = {};
                
                var strFlujoRectificacion = $.trim($("#<%= hdn_Rectificacion.ClientID %>").val());
                var strValidarTablaPrimigenia = $.trim($("#<%= hf_ValidaTablaPrimigenia.ClientID %>").val());

                prm.acno_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
                prm.acno_iActuacionId = $("#<%= hdn_acno_iActuacionId.ClientID %>").val();
                
                var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
                prm.acno_sSubTipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
                prm.acno_bFlagMinuta = $("#<%= chk_acno_bFlagMinuta.ClientID %>").is(':checked');
                prm.acno_vDenominacion = $("#<%= Txt_Denominacion.ClientID %>").val().toUpperCase();
                prm.acno_IFuncionarioAutorizadorId = $("#<%= ddlFuncionario.ClientID %>").val();



                if (strFlujoRectificacion != "0") {                    

                    prm.acno_vNumeroEscrituraPublica = $("#<%= Txt_EscrituraAnterior.ClientID %>").val().toUpperCase();
                    prm.acno_sAccionSubTipoActoNotarialId = $("#<%= ddlAccionR.ClientID %>").val();
                    prm.acno_iActoNotarialReferenciaId = $("#<%= hdn_acno_iActoNotarialReferenciaId.ClientID %>").val();
                }
                else {
                    prm.acno_vNumeroEscrituraPublica = null;
                    prm.acno_sAccionSubTipoActoNotarialId = 8421; //ACCION DE CREACION
                    prm.acno_iActoNotarialReferenciaId = null;
                }
                
                prm.actu_iPersonaRecurrenteId = $("#<%= hdn_actu_iPersonaRecurrenteId.ClientID %>").val();

                if (strValidarTablaPrimigenia == "S") {
                    prm.ValidarTablaPrimigenia = "S";

                    if ($("#<%= hdf_ActoNotarialPrimigeniaId.ClientID %>").val() == "")
                    {
                        prm.ActoNotarialPrimigeniaId = 0;
                    }
                    else
                    {
                        prm.ActoNotarialPrimigeniaId = $("#<%= hdf_ActoNotarialPrimigeniaId.ClientID %>").val();
                    }

                    prm.AnioEscrituraPri = $("#<%= txtAnioEscrituraPri.ClientID %>").val();
                    prm.NumeroEscrituraPublicaPri = $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val();
                    prm.OficinaConsularPri = $("#<%= ddlOficinaConsularPri.ClientID %>").val();
                    prm.FechaExpedicionPri = $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val();
                    prm.TipoActoNotarialPri = $("#<%= txtTipoActoNotarialPri.ClientID %>").val();
                    prm.Notaria = $("#<%= txtNotariaPri.ClientID %>").val();
                    prm.acno_iActoNotarialReferenciaPriId = $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val();

                }
                else {
                    prm.ValidarTablaPrimigenia = "N";
                    prm.ActoNotarialPrimigeniaId = 0;
                }

                var sndprm = {};
                sndprm.actonotarial = JSON.stringify(prm);

                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/insert_registro",
                    data: JSON.stringify(sndprm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rsp) {
                        if (rsp.d != null) {
                            var oResponse = JSON.parse(rsp.d);
                            if (oResponse.acno_iActoNotarialId != 0 && oResponse.Message == null) {
                                $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val(oResponse.acno_iActoNotarialId);
                                $("#<%= hdf_ActoNotarialPrimigeniaId.ClientID %>").val(oResponse.iActoNotarialPrimigeniaId);
                                $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val(oResponse.acno_iActoNotarialReferenciaPriId);
                                $("#<%= hdn_acno_iActuacionId.ClientID %>").val(oResponse.acno_iActuacionId);
                                funSetSession("acno_iActoNotarialId", oResponse.acno_iActoNotarialId.toString());
                                funSetSession("ACT_ID", oResponse.acno_iActuacionId.toString());                                

                                $("#<%= HF_FORMATO_ACTIVO.ClientID %>").val($("#<%=Cmb_TipoActoNotarial.ClientID %>").val());
                                $("#<%=Cmb_TipoActoNotarial.ClientID %>").prop("disabled", "disabled");
                                $("#btnCncl_tab1").prop("disabled", "disabled");

                                showpopupother('i', 'Registro Notarial', 'El registro ha sido grabado correctamente', false, 160, 300);
                                //pageLoadedHandler();
                                
                                tab_02_PersonaNaturalBuscar("S");
                                if (strValidarTablaPrimigenia == "S") {
                                    $("#tablaPrimigenia").show();
                                    $("#<%= hf_ValidaTablaPrimigenia.ClientID%>").val("S");
                                }
                                //-----------------------------------------------------------
                                Habilitar_Tab(0);
                                Habilitar_Tab(1);
                                for (var i = 2; i < 6; i++) {
                                    Desabilitar_Tab(i);
                                }

                                $("#btnCncl_tab1").prop("disabled", "disabled");
                                Mover_TabIndex(1);
                                //-----------------------------------------------------------                           
                            }
                        }
                        else {
                            if (oResponse.Identity == 0 && oResponse.Message != null) {
                                showdialog('e', 'Registro Notarial : Cuerpo', oResponse.Message.toString(), false, 160, 300);
                            }
                        }
                    },
                    error: errores

                });
            }

        }

        function funSetParticipantes(tipoActo) {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var prm = {};
            prm.tipoActo = tipoActo;
            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/cargar_cboparticipantes",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspta) {
                    var participantes = $.parseJSON(rspta.d);

                    var ddl = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>");
                    ddl.empty();

                    var strSeleccionar = "- SELECCIONAR -";
                    ddl.append($('<option></option>').val('0').html(strSeleccionar));
                    for (var i = 0; i < participantes.length; i++) {
                        var Participantes = participantes[i];
                        ddl.append($('<option></option>').val(Participantes.ValueField).html(Participantes.TextField));
                    }
                },

                error: errores
            });
        }

        function ValidarTabRegistro() {

            var bolValida = true;

            var strFlujoRectificacion = $.trim($("#<%= hdn_Rectificacion.ClientID %>").val());

            var strTipoActoNotarial = $.trim($("#<%= Cmb_TipoActoNotarial.ClientID %>").val());
            var strDenominacion = $.trim($("#<%= Txt_Denominacion.ClientID %>").val());
            var strFuncionario = $.trim($("#<%= ddlFuncionario.ClientID %>").val());
            var strEscrituraAnterior = $.trim($("#<%= Txt_EscrituraAnterior.ClientID %>").val());
            var strAccionTipoActoNotarial = $.trim($("#<%= ddlAccionR.ClientID %>").val());

            var strValidarTablaPrimigenia = $.trim($("#<%= hf_ValidaTablaPrimigenia.ClientID %>").val());

            if (strFlujoRectificacion == "0") {

                if (strTipoActoNotarial == "0") {
                    $("#<%=Cmb_TipoActoNotarial.ClientID %>").focus();
                    $("#<%=Cmb_TipoActoNotarial.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=Cmb_TipoActoNotarial.ClientID %>").css("border", "solid #888888 1px");
                }
            }
            else {

                if (strAccionTipoActoNotarial == "0") {
                    $("#<%=ddlAccionR.ClientID %>").focus();
                    $("#<%=ddlAccionR.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddlAccionR.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strDenominacion.length == 0) {
                $("#<%=Txt_Denominacion.ClientID %>").focus();
                $("#<%=Txt_Denominacion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=Txt_Denominacion.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strFuncionario == "0") {
                $("#<%=ddlFuncionario.ClientID %>").focus();
                $("#<%=ddlFuncionario.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlFuncionario.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                if (strValidarTablaPrimigenia == "S") {
                   bolValida = ValidarTabPrimigenia();
                }
            }
            /*Validación*/
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


        function ValidarTabPrimigenia() {
            var bolValida = true;

            var strAnioEscrituraPri = $.trim($("#<%= txtAnioEscrituraPri.ClientID %>").val());
            var strNumeroEscrituraPublicaPri = $.trim($("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val());
            var strFechaExpedicionPri = $.trim($('#<%=ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val());
            var strTipoActoNotarialPri = $.trim($("#<%= txtTipoActoNotarialPri.ClientID %>").val());

            if (strAnioEscrituraPri.length == 0) {
                $("#<%=txtAnioEscrituraPri.ClientID %>").focus();
                $("#<%=txtAnioEscrituraPri.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtAnioEscrituraPri.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strNumeroEscrituraPublicaPri.length == 0) {
                $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").focus();
                $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strFechaExpedicionPri.length == 0) {

                $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').focus();
                $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').css("border", "solid Red 1px");                
                
                bolValida = false;
            }
            else {
                $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
            }
            if (strTipoActoNotarialPri.length == 0) {
                $("#<%=txtTipoActoNotarialPri.ClientID %>").focus();
                $("#<%=txtTipoActoNotarialPri.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTipoActoNotarialPri.ClientID %>").css("border", "solid #888888 1px");
            }
            return bolValida;
        }


        function disabled_tab_01(estado) {
            $("#<%=Cmb_TipoActoNotarial.ClientID %>").attr('disabled', estado);
            $("#<%=ddlFuncionario.ClientID %>").attr('disabled', estado);
            $("#<%=Txt_EscrituraAnterior.ClientID %>").attr('disabled', estado);
            $("#<%=Txt_Denominacion.ClientID %>").attr('disabled', estado);
            $("#btnSave_tab1").attr('disabled', estado);
            $("#btnCncl_tab1").attr('disabled', estado);
        }

        function LimpiarTabRegistro() {

            var strFlujoRectificacion = $.trim($("#<%= hdn_Rectificacion.ClientID %>").val());

            $("#<%= Txt_Denominacion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%= ddlFuncionario.ClientID %>").css("border", "solid #888888 1px");
            $("#<%= ddlAccionR.ClientID %>").css("border", "solid #888888 1px");
            
            if (strFlujoRectificacion != "0") {
                $("#<%= Txt_Denominacion.ClientID %>").val('');
                $("#<%= ddlFuncionario.ClientID %>").val('0');
                $("#<%= ddlAccionR.ClientID %>").val('0');
            }
            else {
                $("#<%= Cmb_TipoActoNotarial.ClientID %>").val('0');
                $("#<%= Cmb_TipoActoNotarial.ClientID %>").css("border", "solid #888888 1px");
                $("#<%= Txt_Denominacion.ClientID %>").val('');
                $("#<%= ddlFuncionario.ClientID %>").val('0');
                $("#<%= Cmb_TipoActoNotarial.ClientID %>").change();
                
            }

            var strValidarTablaPrimigenia = $.trim($("#<%= hf_ValidaTablaPrimigenia.ClientID %>").val());
            if (strValidarTablaPrimigenia == "S") {
                $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val('0');
                $("#<%= hdf_ActoNotarialPrimigeniaId.ClientID %>").val('0');
                $("#<%= txtAnioEscrituraPri.ClientID %>").val('');
                $("#<%= txtAnioEscrituraPri.ClientID %>").css("border", "solid #888888 1px");
                $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val('');
                $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid #888888 1px");
                $("#<%= ddlOficinaConsularPri.ClientID %>").val('0');
                $("#<%= ddlOficinaConsularPri.ClientID %>").css("border", "solid #888888 1px");
                $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val('');
                $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
                $("#<%=txtTipoActoNotarialPri.ClientID %>").val('');
                $("#<%=txtTipoActoNotarialPri.ClientID %>").css("border", "solid #888888 1px");
                $("#<%=txtNotariaPri.ClientID %>").val('');
            }


            $("#<%= lblValidacionRegistro.ClientID %>").hide();
        }

        function limpiarTablaPrimigenia() {
            $("#<%= hdf_ActoNotarialReferencialPriId.ClientID %>").val('0');
            $("#<%= hdf_ActoNotarialPrimigeniaId.ClientID %>").val('0');
            $("#<%= txtAnioEscrituraPri.ClientID %>").val('');
            $("#<%= txtAnioEscrituraPri.ClientID %>").css("border", "solid #888888 1px");
            $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").val('');
            $("#<%= txtNumeroEscrituraPublicaPri.ClientID %>").css("border", "solid #888888 1px");
            $("#<%= ddlOficinaConsularPri.ClientID %>").val('0');
            $("#<%= ddlOficinaConsularPri.ClientID %>").css("border", "solid #888888 1px");
            $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').val('');
            $('#<%= ctrFechaExpedicionPri.FindControl("TxtFecha").ClientID %>').css("border", "solid #888888 1px");
            $("#<%=txtTipoActoNotarialPri.ClientID %>").val('');
            $("#<%=txtTipoActoNotarialPri.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtNotariaPri.ClientID %>").val('');

        }

        function HabilitarCodigoPostal() {
            $("#trCodigoPostal").show();
        }
        function DeshabilitarCodigoPostal() {
            $("#trCodigoPostal").hide();
        }

        function HabilitaCamposRectificacion() {
            $("#trNroEscrituraAnterior").show();
            $("#trAccionRectificacion").show();
        }

        function SoloHabilitarTablaPrimigenia() {
            $("#tablaPrimigenia").show();
            $("#<%= hf_ValidaTablaPrimigenia.ClientID%>").val("S");
        }
        function HabilitarTablaPrimigenia() {
            $("#tablaPrimigenia").show();
            $("#<%= hf_ValidaTablaPrimigenia.ClientID%>").val("S");
            limpiarTablaPrimigenia();
        }

        function DeshablitarTablaPrimigenia() {
            $("#tablaPrimigenia").hide();
            $("#<%= hf_ValidaTablaPrimigenia.ClientID%>").val("N");
            limpiarTablaPrimigenia();
        }

        function HabilitarTablaFuncionarioResponsable() {
            $("#tabFuncionarioResponsable").show();
            $("#<%= HF_ValidaTablaFuncionarioResponsable.ClientID%>").val("S");
            $("#<%= ddlFuncionarioResponsable.ClientID %>").val('0');
        }
        function DeshabilitarTablaFuncionarioResponsable() {
            $("#tabFuncionarioResponsable").hide();
            $("#<%= HF_ValidaTablaFuncionarioResponsable.ClientID%>").val("N");
            $("#<%= ddlFuncionarioResponsable.ClientID %>").val('0');
        }
        

        function Buscar_Representante_Legal() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var seleccion = document.getElementById('ddlRepresentantes');
            var str_codigo_Persona = seleccion.options[seleccion.selectedIndex].value;
            if (str_codigo_Persona.trim() == "") { return; }
            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/Retornar_persona",
                data: '{iPersona: ' + JSON.stringify(str_codigo_Persona) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var objPersona = rspt.d;

                    $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val(objPersona[1]);
                    $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val(objPersona[0]);

                    tab_02_PersonaNaturalBuscar("N");

                },

                error: errores
            });
        }


        function errores(msg) {
            if (msg.responseText != "") {
                showdialog('e', 'Registro Notarial : Cuerpo', 'ERROR: ' + msg.responseText, false, 160, 300);
            }
        }


        function Layout_TipoPersona() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var tipopersona = $("#<%=ddl_pers_sPersonaTipoId.ClientID %> ").val();
            switch (tipopersona) {
                case "2101": //Natural
                    $("#<%=lbl_direccion.ClientID %>").text("Dirección");

                    $("#<%=ddl_pers_sPersonaTipoId.ClientID %>").css("border", "solid #888888 1px");
                    $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val('0');
                    $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val('');
                    $("#<%=txt_empr_vRazonSocial.ClientID %>").val('');

                    $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").attr('disabled', true);
                    $("#<%=txt_empr_vNumeroDocumento.ClientID %>").attr('disabled', true);
                    $("#<%=txt_empr_vRazonSocial.ClientID %>").attr('disabled', true);

                    //                    $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', true);
                    //                    $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', true);
                    //                    $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', false);
                    $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr('disabled', false);
                    //                    $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', true);

                    //                    $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
                    //                    $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
                    //                    $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', true);

                    break;

                case "2102": //Juridica

                    //$("#<%=ddl_pers_sPersonaTipoId.ClientID %>").val('2101');
                    //                    showpopupother('i', 'Registro Notarial', 'ESTE FLUJO SE ENCUENTRA EN DESARROLLO.', false, 160, 300, '');

                    break;
            }
        }
        function imgBuscarPersonaN(e) {
            if ($("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de participante', false, 160, 300);
                return;
            }
            if ($("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de documento', false, 160, 300);
                return;
            }
            var tipopersona = $("#<%=ddl_pers_sPersonaTipoId.ClientID %> ").val();
            switch (tipopersona) {
                case "2101": //Natural
                    var TipoDocumento_N = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
                    var NumeroDocumento_N = $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val();

                    if (TipoDocumento_N != '0' && NumeroDocumento_N.trim() != '') {
                        tab_02_PersonaNaturalBuscar("N");
                        e.preventDefault();
                    }
                    break;

                case "2102": //Juridica
                    var TipoDocumento = $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val();
                    var NumeroDocumento = $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val();

                    if (TipoDocumento != '0' && NumeroDocumento.trim() != '') {
                        tab_02_PersonaJuridicaBuscar();
                        e.preventDefault();
                    }
                    break;
            }
        }

        function imgBuscarPersonaAutorizacion(e) {
            var NroDocumento = $("#<%=txtAutorizacionNroDocumento.ClientID %>").val();

            if (NroDocumento.trim() == '') {
                showdialog('a', 'Actos Protocolares : Autorización', 'Digite el número de documento', false, 160, 300);
                return;
            }
            tab_02_AutorizacionBuscar();
            e.preventDefault();

        }
        function tab_02_AutorizacionBuscar() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            var DocumentoTipo = "1";
            var NroDocumento = $("#<%=txtAutorizacionNroDocumento.ClientID %>").val();
            if ((DocumentoTipo != "0") && (NroDocumento != "")) {
                var prm = {};
                prm.idpersona = null;
                prm.tipodocumento = DocumentoTipo;
                prm.documento = NroDocumento;
                prm.ActoNotarialId = "";
                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtener_persona",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var Persona = $.parseJSON(msg.d);
                        if (Persona.pers_iPersonaId != "0") {

                            $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val(Persona.pers_vNombres + ' ' + Persona.pers_vApellidoPaterno + ' ' + Persona.pers_vApellidoMaterno);
                        }
                        else {
                            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").focus();
                            $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val("");
                            showdialog('a', 'Actos Protocolares : Autorización', 'La persona no existe', false, 160, 300);
                        }
                    },
                    error: function (err) {
                        alert('Error: ' + err.responseText);
                    }
                });
            }
        }

        function imgBuscarPersonaPresentante(e) {
            if ($("#<%=ddl_TipoDocrepresentante.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Presentante(s)', 'Falta seleccionar el tipo de documento', false, 160, 300);
                return;
            }
            var NroDocRepresentante = $("#<%=txtRepresentanteNroDoc.ClientID %>").val();

            if (NroDocRepresentante.trim() == '') {
                showdialog('a', 'Actos Protocolares : Presentante(s)', 'Digite el número de documento', false, 160, 300);
                return;
            }
            tab_02_PresentanteBuscar();
            e.preventDefault();
        }

        function tab_02_PresentanteBuscar() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            var DocumentoTipo = $("#<%=ddl_TipoDocrepresentante.ClientID %>").val();
            var NroDocRepresentante = $("#<%=txtRepresentanteNroDoc.ClientID %>").val();
            if ((DocumentoTipo != "0") && (NroDocRepresentante != "")) {
                var prm = {};
                prm.idpersona = null;
                prm.tipodocumento = DocumentoTipo;
                prm.documento = NroDocRepresentante;
                prm.ActoNotarialId = "";
                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtener_persona",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var Persona = $.parseJSON(msg.d);
                        if (Persona.pers_iPersonaId != "0") {

                            $("#<%= txtRepresentanteNombres.ClientID %>").val(Persona.pers_vNombres + ' ' + Persona.pers_vApellidoPaterno + ' ' + Persona.pers_vApellidoMaterno);
                            $("#<%= ddl_GerenoPresentante.ClientID %>").val(Persona.pers_sGeneroId);
                        }
                        else {
                            $("#<%=txtRepresentanteNombres.ClientID %>").focus();
                            $("#<%= txtRepresentanteNombres.ClientID %>").val("");
                            $("#<%= ddl_GerenoPresentante.ClientID %>").val("0");
                            showdialog('a', 'Actos Protocolares : Presentante(s)', 'La persona no existe', false, 160, 300);
                        }
                    },
                    error: function (err) {
                        alert('Error: ' + err.responseText);
                    }
                });
             }
        }

        function tab_02_PersonaJuridicaBuscar() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            if ($("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de participante', false, 160, 300);
                return;
            }
            if ($("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de documento', false, 160, 300);
                return;
            }

            if ($("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val() != "0") {

                var rpta = isSessionTimeOut();
                if (rpta != 'ok') return;

                var EmpresaTipo = $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val();
                var EmpresaNumero = $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val();

                if ((EmpresaTipo != "") && (EmpresaNumero != "")) {
                    var prm = {};
                    prm.tipodocumento = EmpresaTipo;
                    prm.documento = EmpresaNumero;

                    $.ajax({
                        type: "POST",
                        url: "FrmActoNotarialProtocolares.aspx/obtener_empresa",
                        data: JSON.stringify(prm),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (rspta) {
                            var Empresa = $.parseJSON(rspta.d);

                            if (Empresa.empr_iEmpresaId != 0) {
                                tab_02_PersonaJuridicaExiste(Empresa);
                            }
                            else {
                                showdialog('a', 'Actos Protocolares : Participante(s)', 'No existe la empresa. Deberá registrarla en el Modulo de Empresas.', false, 160, 300);
                            }
                        },

                        error: errores

                    });
                }
            }
            else {
            }

            return false;
        }

        function tab_02_PersonaJuridicaExiste(empresa) {
            if ($("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de participante', false, 160, 300);
                return;
            }
            if ($("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de documento', false, 160, 300);
                return;
            }
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            if (empresa.empr_iEmpresaId != "0") {
                $("#<%=txt_empr_vRazonSocial.ClientID %>").val(empresa.empr_vRazonSocial);
                $("#<%=txt_empr_vRazonSocial.ClientID %>").attr('disabled', true);

                var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();

                var prm = {};
                prm.idempresa = empresa.empr_iEmpresaId;

                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtener_representeslegales",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rspta) {
                        var RepresentantesLegales = $.parseJSON(rspta.d);
                        var output = [];

                        if (RepresentantesLegales.length != 0) {

                            if (RepresentantesLegales.length == 1) {
                                //CUANDO ES UN SOLO REPRESENTANTE
                                var prm = {};
                                prm.idpersona = RepresentantesLegales[0].rele_iPersonaId;
                                prm.tipodocumento = null;
                                prm.documento = null;

                                $.ajax({
                                    type: "POST",
                                    url: "FrmActoNotarialProtocolares.aspx/obtener_persona",
                                    data: JSON.stringify(prm),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (rspta) {
                                        var Persona = $.parseJSON(rspta.d);
                                        funInfoPersona(Persona);

                                        //                                        $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', true);
                                        //                                        $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', true);
                                        //                                        $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', true);

                                        if (TipoActoNotarialId == "8050" || TipoActoNotarialId == "8051") { //Si son poderes
                                            //                                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', true);

                                            //                                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
                                            //                                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', true);
                                        }
                                        else {
                                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', false);
                                            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', false);
                                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', false);

                                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', false);
                                            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
                                            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
                                            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
                                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', false);
                                        }

                                        $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").focus();
                                    },

                                    error: errores

                                });
                            }
                            else {

                                var strSeleccionar = "- SELECCIONAR -";
                                output.push('<option value="0">' + strSeleccionar + '</option>')

                                for (var i = 0; i < RepresentantesLegales.length; i++) {
                                    var RepresentanteLegal = RepresentantesLegales[i];
                                    var prm = {};
                                    prm.idpersona = RepresentanteLegal.rele_iPersonaId;
                                    prm.tipodocumento = null;
                                    prm.documento = null;

                                    $.ajax({
                                        type: "POST",
                                        url: "FrmActoNotarialProtocolares.aspx/obtener_persona",
                                        data: JSON.stringify(prm),
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (rspta) {
                                            var Representante = $.parseJSON(rspta.d);
                                            var value = Representante.pers_vApellidoPaterno + ' ' + Representante.pers_vApellidoMaterno + ',' + Representante.pers_vNombres;
                                            output.push('<option value="' + Representante.pers_iPersonaId.toString() + '">' + value + '</option>')
                                            $('#ddlRepresentantes').html(output.join(''));

                                            if ($("#<%=hdn_actu_iPersonaRecurrenteId.ClientID %>").val() == "0") {
                                                Buscar_Representante_Legal();
                                            }
                                        },

                                        error: errores

                                    });
                                }

                                $('#divRepresentantesLegales').show();
                            }
                        }
                        else {
                            showdialog('a', 'Actos Protocolares : Participante(s)', 'La empresa que ha consultado no tiene Representantes Legales. Deberá ingresarlo en el Modulo de Empresas.', false, 160, 300);

                        }
                    },

                    error: errores

                });
            }
            else {

                localStorage.setItem("ExistEmpresa", "0");
            }
        }

        function tab_02_PersonaNaturalBuscar(validar) {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            if ($("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de participante', false, 160, 300);
                return;
            }
            if ($("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val() == "0") {
                showdialog('a', 'Actos Protocolares : Participante(s)', 'Falta seleccionar el tipo de documento', false, 160, 300);
                return;
            }
            

            var DocumentoTipo = $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val();
            var DocumentoNumero = $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val();
            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
            var ActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
            var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();
            var InterpreteId = $("#<%= HF_INTERPRETE.ClientID %>").val();

            var ApoderadoId = $("#<%= HF_APODERADO.ClientID %>").val();
            var CompradorId = $("#<%= HF_COMPRADOR.ClientID %>").val();
            var AnticipadoId = $("#<%= HF_ANTICIPADO.ClientID %>").val();
            var DonatarioId = $("#<%= HF_DONATARIO.ClientID %>").val();
            var esGrupoApoderado = "NO";
            

            if (TipoParticipanteId != InterpreteId)
            {
                ActoNotarialId = "";
            }

            if (TipoParticipanteId == ApoderadoId || TipoParticipanteId == CompradorId
                || TipoParticipanteId == AnticipadoId || TipoParticipanteId == DonatarioId) {
                esGrupoApoderado = "SI";
            }
            

            if ((DocumentoTipo != "0") && (DocumentoNumero != "")) {
                var prm = {};
                prm.idpersona = null;
                prm.tipodocumento = DocumentoTipo;
                prm.documento = DocumentoNumero;
                prm.ActoNotarialId = ActoNotarialId;
                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtener_persona",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var Persona = $.parseJSON(msg.d);
                        var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();
                        var DocTipoParticipanteId = $("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val();
                        var DocNumero = $("#<%= txt_peid_vDocumentoNumero.ClientID %>").val();

                        tab_02_PersonaClear();

                        $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val(TipoParticipanteId);
                        $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val(DocTipoParticipanteId);
                        $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val(DocNumero);

                        if (Persona.pers_iPersonaId != "0") {

                            $("#<%=hdn_NoExistePersonaBuscada.ClientID %>").val("0");
                            funInfoPersona(Persona);
                            if (validar == "S") {
                                mostrarUbigeo();
                                if (ValidarIngresoParticipante() == true) {
                                    darclick();
                                }
                            }

                            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").change();

                            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', true);
                            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr('disabled', true);

                            //                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', true);
                            //                            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', true);
                            //                            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', true);
                            //                            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', true);

                            //                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', true);
                            //                            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', true);
                            //                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', true);

                            //                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', true);


                            //                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', true);
                            //                            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                            //                            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
                            //                            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
                            //                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', true);


                            //                            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', false);
                            //                            if (Persona.pers_sGeneroId != 0) {
                            //                                $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', true);
                            //                            }

                            //                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', false);
                            //                            if (Persona.pers_sEstadoCivilId != 0) {
                            //                                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', true);
                            //                            }


                            //                            $("#<%=chkIncapacitado.ClientID %>").attr('disabled', false);
                            //                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").attr('disabled', true);



                            //                            if (Persona.bIncapacidadFlag != null) {
                            //                                if (Persona.bIncapacidadFlag) {
                            //                                    $("#<%=chkIncapacitado.ClientID %>").prop("checked", "checked");
                            //                                }
                            //                                else {
                            //                                    $("#<%=chkIncapacitado.ClientID %>").attr('disabled', false);

                            //                                    $("#<%=chkIncapacitado.ClientID %>").prop("checked", "");
                            //                                }
                            //                            }

                            //                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', false);
                            //                            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
                            //                            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
                            //                            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
                            //                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', false);


                            //                            var residenciaTop = Persona._ResidenciaTop;

                            //                            if (residenciaTop != null) {

                            //                                $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', true);
                            //                                $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
                            //                                $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
                            //                                $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', true);
                            //                                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', true);
                            //                            }

                            $("#<%=lblAstGenero.ClientID %>").show();
                            $("#<%=lblAstEstCivil.ClientID %>").show();
                            $("#<%=lblAstProf.ClientID %>").show();
                            $("#<%=lblAstIdioma.ClientID %>").show();
                            $("#<%=lblAstDomicilio.ClientID %>").show();
                            $("#<%=lblAstDpto.ClientID %>").show();
                            $("#<%=lblAstProv.ClientID %>").show();
                            $("#<%=lblAstDist.ClientID %>").show();

                            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").focus();

                            var str_nacionalidad = $("#<%=ddlPaisOrigen.ClientID %>").val();

                            if (str_nacionalidad == '0') {

                                $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', false);
                            }

                        }
                        else {

                            $("#<%=hdn_NoExistePersonaBuscada.ClientID %>").val("1");
                            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").focus();
                            showdialog('a', 'Actos Protocolares : Participante(s)', 'La persona no existe', false, 160, 300);

                            $("#<%= txtDescOtroDocumento.ClientID %>").val("");

                            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
                            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");
                            $("#<%=txt_pers_vApellidoCasada.ClientID %>").val("");

                            $("#<%=txt_pers_vNombres.ClientID %>").val("");
                            $("#<%=ddl_pers_sGeneroId.ClientID %>").val("0");
                            $("#<%=ddlPaisOrigen.ClientID %>").val("0");

                            $("#<%=LblDescNacionalidadCopia.ClientID %>").text("");

                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").val("0");

                            $("#<%=ddl_pers_sProfesionId.ClientID %>").val("0");

                            var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();
                            var InterpreteId = $("#<%= HF_INTERPRETE.ClientID %>").val();

                            if (TipoParticipanteId == InterpreteId) {
                                $("#<%= ddl_pers_sIdiomaNatalId.ClientID %>").val(persona.pers_sIdiomaNatalId);
                                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', true);
                            }
                            else {
                                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").val("0");
                                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', false);
                            }
                            $("#<%=chkIncapacitado.ClientID %>").prop("checked", "");
                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").val("");


                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").val("");
                            $("#<%=ddl_UbigeoPais.ClientID %>").val("0");
                            $("#<%=ddl_UbigeoRegion.ClientID %>").val("0");
                            $("#<%=ddl_UbigeoCiudad.ClientID %>").val("0");

                            if (esGrupoApoderado == "SI") {
                                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").val("REPÚBLICA DEL PERÚ");
                                $("#<%=ddl_UbigeoPais.ClientID %>").val("14");
                                $("#<%=hdn_acpa_pais.ClientID %>").val("14");
                                cargarProvincia();
                                $("#<%=ddl_UbigeoRegion.ClientID %>").val("01");
                                $("#<%=hdn_acpa_provincia.ClientID %>").val("01");                                
                                cargarDistrito();
                                $("#<%=ddl_UbigeoCiudad.ClientID %>").val("01");
                                $("#<%=hdn_acpa_distrito.ClientID %>").val("01");
                                
                            }

                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").val("");

                            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', false);
                            //$("#<%= txtDescOtroDocumento.ClientID %>").attr('disabled', false);

                            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', false);
                            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', false);
                            $("#<%=txt_pers_vApellidoCasada.ClientID %>").attr('disabled', false);

                            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', false);

                            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', false);
                            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', false);

                            $("#<%=chkIncapacitado.ClientID %>").attr('disabled', false);
                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").attr('disabled', false);
                            $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
                            $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
                            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', false);
                            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
                            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
                            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
                            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', false);
                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").attr('disabled', true);
                            $("#<%=lblAstGenero.ClientID %>").show();
                            $("#<%=lblAstEstCivil.ClientID %>").show();
                            $("#<%=lblAstProf.ClientID %>").show();
                            $("#<%=lblAstIdioma.ClientID %>").show();
                            $("#<%=lblAstDomicilio.ClientID %>").show();
                            $("#<%=lblAstDpto.ClientID %>").show();
                            $("#<%=lblAstProv.ClientID %>").show();
                            $("#<%=lblAstDist.ClientID %>").show();

                        }
                    },
                    error: function (err) {
                        alert('Error: ' + err.responseText);
                    }
                });
            }
            else {
                if (validar == "N") {
                    showdialog('a', 'Actos Protocolares : Participante(s)', 'No ha colocado los datos', false, 160, 300);
                }
                tab_02_PersonaClear();
            }
        }

        function tab_02_PersonaNaturalExiste(estado) {
            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', false);
            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', false);
            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', false);
            $("#<%=txt_pers_vApellidoCasada.ClientID %>").attr('disabled', false);

            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', false);
            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', false);
            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', false);

            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', false);
            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', false);

        }

        function funInfoPersona(persona) {
            $("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val(persona.Identificacion.peid_sDocumentoTipoId);
            $("#<%= txt_peid_vDocumentoNumero.ClientID %>").val(persona.Identificacion.peid_vDocumentoNumero);

            var descripcionOtroDocumento = persona.Identificacion.peid_vTipodocumento;

            $("#<%= txtDescOtroDocumento.ClientID %>").val(persona.Identificacion.peid_vTipodocumento);

            if (descripcionOtroDocumento != null) {
                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css('visibility', 'visible');
                $("#<%= tablaOtroDocumento.ClientID %>").css("display", "block");                

            } else {
                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css('visibility', 'hidden');
                $("#<%= tablaOtroDocumento.ClientID %>").css("display", "none");
            }
            
            $("#<%= ddl_pers_sNacionalidadId.ClientID %>").val(persona.pers_sNacionalidadId);
            $("#<%= txt_pers_vApellidoPaterno.ClientID %>").val(persona.pers_vApellidoPaterno);
            $("#<%= txt_pers_vApellidoMaterno.ClientID %>").val(persona.pers_vApellidoMaterno);
            $("#<%= txt_pers_vApellidoCasada.ClientID %>").val(persona.pers_vApellidoCasada);
            $("#<%= txt_pers_vNombres.ClientID %>").val(persona.pers_vNombres);
            $("#<%= ddl_pers_sGeneroId.ClientID %>").val(persona.pers_sGeneroId);
            
            if ($("#<%=ddl_peid_sDocumentoTipoId.ClientID %> option:selected").text() == "DNI" && persona.pers_sPaisId == "0") {
                $("#<%=ddlPaisOrigen.ClientID %>" + " option").each(function () {
                    if ($(this).text() == "PERÚ") {
                        $("#<%=ddlPaisOrigen.ClientID %>").val($(this).val());
                        $("#<%=ddlPaisOrigen.ClientID %>").attr('selected', 'selected');
                        $("#<%= LblDescNacionalidadCopia.ClientID %>").text("PERUANA");
                    }
                });
            } else {
                $("#<%= ddlPaisOrigen.ClientID %>").val(persona.pers_sPaisId);
                $("#<%= LblDescNacionalidadCopia.ClientID %>").text(persona.vNacionalidad);
            }
            $("#<%= ddl_pers_sEstadoCivilId.ClientID %>").val(persona.pers_sEstadoCivilId);
            $("#<%= ddl_pers_sProfesionId.ClientID %>").val(persona.pers_sOcupacionId);
            $("#<%= ddl_pers_sIdiomaNatalId.ClientID %>").val(persona.pers_sIdiomaNatalId);
            
            var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();
            var InterpreteId = $("#<%= HF_INTERPRETE.ClientID %>").val();
            if (TipoParticipanteId == InterpreteId) {
                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', true);
            }
            
            if (persona.pers_bIncapacidadFlag != null) {
                if (persona.pers_bIncapacidadFlag) {
                    //                    $("#<%= chkIncapacitado.ClientID %>").prop("checked", "checked");
                    //                    $("#<%= txtRegistroTipoIncapacidad.ClientID %>").val(persona.pers_vDescripcionIncapacidad);
                    //------------------------------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Fecha: 19/09/2017
                    //Objetivo: Poner en blanco la incapacidad cuando se busca la persona.
                    //------------------------------------------------------------------------
                    $("#<%= chkIncapacitado.ClientID %>").prop("checked", "");
                    $("#<%= txtRegistroTipoIncapacidad.ClientID %>").val("");
                    //------------------------------------------------------------------------

                } else {
                    $("#<%= chkIncapacitado.ClientID %>").prop("checked", "");
                    $("#<%= txtRegistroTipoIncapacidad.ClientID %>").val("");
                }
            }

            $("#<%= hdn_IndiceGrillaParticipantesEdicion.ClientID %>").val("0");

            ObtenerElementosGenero();
            validarApellidoCasada();
            var residenciaTop = persona._ResidenciaTop;

            if (residenciaTop != null) {
                var ubigeo = residenciaTop.resi_cResidenciaUbigeo;

                if (ubigeo != null) {
                    var pais = ubigeo.substring(0, 2);
                    var region = ubigeo.substring(2, 4);
                    var ciudad = ubigeo.substring(4, 6);
                    //9213 -> Estado Unidos de América
                    if (pais + region == "9213") {
                        $("#trCodigoPostal").show();
                     }
                    else {
                        $("#trCodigoPostal").hide();
                    }

                    $("#<%=hdn_acpa_pais.ClientID %>").val(pais.toString());
                    $("#<%=hdn_acpa_provincia.ClientID %>").val(region.toString());
                    $("#<%=hdn_acpa_distrito.ClientID %>").val(ciudad.toString());

                    $("#<%=ddl_UbigeoPais.ClientID %>").val(pais.toString());
                    var ddl1 = $("#<%= ddl_UbigeoRegion.ClientID %>");
                    var ddl2 = $("#<%= ddl_UbigeoCiudad.ClientID %>");

                    funSetUbigeoProvincia(ddl1, pais.toString());
                    $("#<%= ddl_UbigeoRegion.ClientID %>").val(region.toString());
                    funSetUbigeoDistrito(ddl2, pais.toString(), region.toString());
                    $("#<%= ddl_UbigeoCiudad.ClientID %>").val(ciudad.toString());
                }
                $("#<%= hdn_acpa_residenciaId.ClientID %>").val(residenciaTop.resi_iResidenciaId)
                
                
                $("#<%= txt_resi_vResidenciaDireccion.ClientID %>").val(residenciaTop.resi_vResidenciaDireccion);

                $("#<%= txt_resi_vCodigoPostal.ClientID %>").val(residenciaTop.resi_vCodigoPostal);

            } else {
                var ddl1 = $("#<%= ddl_UbigeoRegion.ClientID %>");
                var ddl2 = $("#<%= ddl_UbigeoCiudad.ClientID %>");
                funSetUbigeoProvincia(ddl1, 0);
                funSetUbigeoDistrito(ddl2, 0, 0);
            }
                        
        }


        function funSetUbigeoProvincia(combo, provincia) {
            var prm = {};
            prm.ubigeo = provincia;

            var url = "FrmActoNotarialProtocolares.aspx/obtener_provincias";

            set_dropdownlist(url, prm, combo);
        }

        function funSetUbigeoDistrito(combo, pais, region) {

            var prm = {};
            prm.departamento = pais;
            prm.provincia = region;

            var url = "FrmActoNotarialProtocolares.aspx/obtener_distritos";
            var rspta = execute(url, prm);

            set_dropdownlist(url, prm, combo);

        }

        function ValidarIngresoParticipante() {
            var bolValida = true;

            var GrillaParticipantesEdicion = $("#<%= hdn_IndiceGrillaParticipantesEdicion.ClientID %>").val();

            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
            var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();
            var strDocumentoTipoId = $.trim($("#<%= ddl_peid_sDocumentoTipoId.ClientID %>").val());
            var strDocumentoNumero = $.trim($("#<%= txt_peid_vDocumentoNumero.ClientID %>").val());

            var strsNacionalidadId = $.trim($("#<%= ddl_pers_sNacionalidadId.ClientID %>").val());

            var strApellidoPaterno = $.trim($("#<%= txt_pers_vApellidoPaterno.ClientID %>").val());
            var strApellidoMaterno = $.trim($("#<%= txt_pers_vApellidoMaterno.ClientID %>").val());
            var strApellidoCasada = $.trim($("#<%= txt_pers_vApellidoCasada.ClientID %>").val());

            var strNombres = $.trim($("#<%= txt_pers_vNombres.ClientID %>").val());
            var strGeneroId = $.trim($("#<%= ddl_pers_sGeneroId.ClientID %>").val());

            var strPaisOrigenId = $.trim($("#<%= ddlPaisOrigen.ClientID %>").val());

            var strEstadoCivilId = $.trim($("#<%= ddl_pers_sEstadoCivilId.ClientID %>").val());
            var strvProfesion = $.trim($("#<%= ddl_pers_sProfesionId.ClientID %>").val());
            var strsIdioma = $.trim($("#<%= ddl_pers_sIdiomaNatalId.ClientID %>").val());

            var str_RegistroTipoIncapacidad = $.trim($("#<%= txtRegistroTipoIncapacidad.ClientID %>").val());

            var strvResidenciaDireccion = $.trim($("#<%= txt_resi_vResidenciaDireccion.ClientID %>").val());
            var strUbigeoPais = $.trim($("#<%= ddl_UbigeoPais.ClientID %>").val());
            var strUbigeoRegion = $.trim($("#<%= ddl_UbigeoRegion.ClientID %>").val());
            var strUbigeoCiudad = $.trim($("#<%= ddl_UbigeoCiudad.ClientID %>").val());
            var strvCodigoPostal = $.trim($("#<%= txt_resi_vCodigoPostal.ClientID %>").val());


            var str_NacionalidadExtranjera = $.trim($("#<%= HF_NACIONALIDAD_EXTRANJERA.ClientID %>").val());
            var str_NacionalidadPeruana = $.trim($("#<%= HF_NACIONALIDAD_PERUANA.ClientID %>").val());

            var str_Otorgante = $.trim($("#<%= HF_OTORGANTE.ClientID %>").val());
            var str_Apoderado = $.trim($("#<%= HF_APODERADO.ClientID %>").val());
            var str_testigo = $.trim($("#<%= HF_TESTIGO_A_RUEGO.ClientID %>").val());
            var str_Vendedor = $.trim($("#<%= HF_VENDEDOR.ClientID %>").val());
            var str_Anticipante = $.trim($("#<%= HF_ANTICIPANTE.ClientID %>").val());
            var str_Interprete = $.trim($("#<%= HF_INTERPRETE.ClientID %>").val());

            var ddl_Participante_Discapacidad = $.trim($("#<%= ddl_Participante_Discapacidad.ClientID %>").val());
            var ddl_Participante_Interprete = $.trim($("#<%=ddl_Participante_Interprete.ClientID %>").val());

            if (TipoParticipanteId == "0") {
                $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").focus();
                $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").css("border", "solid Red 1px");
                $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").css("border", "solid #888888 1px");
            }

            

            if (TipoParticipanteId != "0") {
                if (TipoParticipanteId == str_Interprete) {
                    if ($("#<%=ddl_Participante_Interprete.ClientID %>").length > 1) {
                        $("#<%=ddl_Participante_Interprete.ClientID %>").focus();
                        $("#<%=ddl_Participante_Interprete.ClientID %>").css("border", "solid Red 1px");
                        $("#<%=ddl_Participante_Interprete.ClientID %>").attr('disabled', false);
                    }
                    else {
                        $("#<%=ddl_Participante_Interprete.ClientID %>").css("border", "solid #888888 1px");
                    }
                }
            }


            if (strDocumentoTipoId == "0") {
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").focus();
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid Red 1px");
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDocumentoNumero.length == 0) {
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").focus();
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDocumentoTipoId == "4") {
                var strTipoDocumentoOtros = $.trim($("#<%= txtDescOtroDocumento.ClientID %>").val());
                if (strTipoDocumentoOtros.length == 0) {
                    $("#<%=txtDescOtroDocumento.ClientID %>").focus();
                    $("#<%=txtDescOtroDocumento.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescOtroDocumento.ClientID %>").css("border", "solid #888888 1px");
                } 
            }

            /*
            if (strsNacionalidadId == "0") {
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").focus();
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid Red 1px");
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', false);
            bolValida = false;
            }
            else {
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid #888888 1px");
            }
            */
                        

            if (strApellidoPaterno.length == 0) {
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").focus();
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid #888888 1px");
            }
            
            if (strNombres.length == 0) {
                $("#<%=txt_pers_vNombres.ClientID %>").focus();
                $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid #888888 1px");
            }
            
            if (strGeneroId == "0") {
                $("#<%=ddl_pers_sGeneroId.ClientID %>").focus();
                $("#<%=ddl_pers_sGeneroId.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_pers_sGeneroId.ClientID %>").css("border", "solid #888888 1px");
            }
            
            if (strPaisOrigenId == "0") {
                $("#<%=ddlPaisOrigen.ClientID %>").focus();
                $("#<%=ddlPaisOrigen.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlPaisOrigen.ClientID %>").css("border", "solid #888888 1px");
            }

            

            if (strEstadoCivilId == "0") {
                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").focus();
                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").css("border", "solid #888888 1px");
            }

            
            if (strvResidenciaDireccion.length == 0) {
                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").focus();
                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").css("border", "solid Red 1px");
                $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strUbigeoPais == "0") {
                $("#<%=ddl_UbigeoPais.ClientID %>").focus();
                $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid Red 1px");
                $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strUbigeoRegion == "0") {
                $("#<%=ddl_UbigeoRegion.ClientID %>").focus();
                $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid Red 1px");
                $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strUbigeoCiudad == "0") {
                $("#<%=ddl_UbigeoCiudad.ClientID %>").focus();
                $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid Red 1px");
                $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
                bolValida = false;
            }
            else {
                $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid #888888 1px");
            }


            if (str_Apoderado != TipoParticipanteId) {

                if (strvProfesion == "0") {
                    $("#<%=ddl_pers_sProfesionId.ClientID %>").focus();
                    $("#<%=ddl_pers_sProfesionId.ClientID %>").css("border", "solid Red 1px");
                    $("#<%=ddl_pers_sProfesionId.ClientID %>").attr('disabled', false);
                    bolValida = false;
                }
                else {
                    $("#<%=ddl_pers_sProfesionId.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strsIdioma == "0") {
                    $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").focus();
                    $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css("border", "solid Red 1px");
                    $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', false);
                    bolValida = false;
                }
                else {
                    $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css("border", "solid #888888 1px");
                }
            } else {

                if (str_Otorgante == TipoParticipanteId || str_Vendedor == TipoParticipanteId || str_Anticipante == TipoParticipanteId) {
                    if ($("#<%= chkIncapacitado.ClientID %>").is(':checked')) {

                        if (str_RegistroTipoIncapacidad.length == 0) {
                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid Red 1px");
                            bolValida = false;
                        } else {
                            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid #888888 1px");
                        }
                    }
                }
            }

            if (str_testigo == TipoParticipanteId) {

                if (ddl_Participante_Discapacidad == 0) {
                    $("#<%=ddl_Participante_Discapacidad.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                } else {
                    $("#<%=ddl_Participante_Discapacidad.ClientID %>").css("border", "solid #888888 1px");
                }
            }


            if ($("#<%= chkIncapacitado.ClientID %>").is(':checked')) {

                if (str_RegistroTipoIncapacidad.length == 0) {
                    $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                } else {
                    $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid #888888 1px");
                }
            }


            if (GrillaParticipantesEdicion == "0") {
                if (ExisteParticipanteIngresado(strDocumentoNumero) == false) {
                    bolValida = false;
                    showdialog('a', 'Registro Notarial : Participante(s)', 'El participante ya se encuentra registrado', false, 160, 300);
                }
            }

            

            return bolValida;
        }
        function VacioGrdParticipantes() {
            var valor;
            var rprta = true;
            $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {
                rprta = false;
            });
            return rprta;
        }
        function ExisteParticipanteXDefecto(strNroDocumento) {
            var valor;
            var rprta = false;
            $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {
                $(this).children("td").each(function (index2) {
                    valor = $(this).text();
                    if (valor == strNroDocumento) {
                        rprta = true;
                    }
                    
                })
            })

            return rprta;
        }

        function ExisteParticipanteIngresado(strNroDocumento) {
            var valor;
            var rprta = true;
            $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {
                $(this).children("td").each(function (index2) {
                    if (index2 == 2) {
                        valor = $(this).text();
                        if (valor == strNroDocumento) {
                            rprta = false;
                        }
                    }
                })
            })

            return rprta;
        }

        function ExisteParticipantePeruano() {
            var valor;
            var rpta = false;
            $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {
                $(this).children("td").each(function (index15) {
                    if (index15 == 15) {
                        valor = $(this).text();
                        if (valor.substring(0, 4) == "PERU") {
                            rpta = true;
                        }
                    }
                })
            })

            return rpta;
        }


        function ValidarParticipanteTestigoRuego() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var stipoParticipanteAnterior = $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val();
            var sTipoParticipanteId = $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val();
            var sTipoParticipanteEditando = $("#<%= hdn_Tipo_Participante_Editando.ClientID %>").val();


            if (sTipoParticipanteEditando == "8415" && sTipoParticipanteId == "8414") {
                showpopupother('a', 'PaRegistro Notarial : Participantes', 'No se puede actualizar de Otorgante a Testigo a Ruego.', false, 200, 250);
                $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val(stipoParticipanteAnterior);
                return;
            }
            //            8414: TESTIGO A RUEGO
            if (sTipoParticipanteId == "8414") {
                MostrarlistaIncapacidad();
            }
            else {
                OcularListaIncapacidad();
            }

            $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val(sTipoParticipanteId);
            var sTipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();

            prm = {}
            prm.sTipoParticipanteId = sTipoParticipanteId;
            prm.sTipoActoNotarialId = sTipoActoNotarialId;
            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/ValidarParticipanteTestigoRuego",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var data = rspt.d;
                    if (data != 'ok') {
                        showdialog('a', 'Registro Notarial : Cuerpo', data, false, 160, 300);
                        $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val('0');
                    }

                    return false;
                }
            });
        }

        function ValidarParticipanteInterprete() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var stipoParticipanteAnterior = $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val();
            var sTipoParticipanteId = $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val();
            var sTipoParticipanteEditando = $("#<%= hdn_Tipo_Participante_Editando.ClientID %>").val();


            if (sTipoParticipanteEditando == "8415" && sTipoParticipanteId == "8413") {
                showpopupother('a', 'PaRegistro Notarial : Participantes', 'No se puede actualizar de Otorgante a Interprete.', false, 200, 250);
                $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val(stipoParticipanteAnterior);
                return;
            }
            //8413: INTERPRETE
            if (sTipoParticipanteId == "8413") {
                if ($("#<%=ddl_Participante_Interprete.ClientID %>").length > 1) {
                    MostrarlistaInterpretes();
                }
                else {
                    OcularListaInterpretes();
                }
            }
            else {
                OcularListaInterpretes();
            }

            $("#<%= hdn_Tipo_Participante_Selected.ClientID %>").val(sTipoParticipanteId);
            var sTipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();

            prm = {}
            prm.sTipoParticipanteId = sTipoParticipanteId;
            prm.sTipoActoNotarialId = sTipoActoNotarialId;
            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/ValidarParticipanteInterprete",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var data = rspt.d;
                    if (data != 'ok') {
                        showdialog('a', 'Registro Notarial : Cuerpo', data, false, 160, 300);
                        $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val('0');
                    }

                    return false;
                }
            });

        }


        function tab_02_PersonaClear() {
            
            $("#<%=hdn_acpa_residenciaId.ClientID %>").val("0");

            $("#imgBuscarPN").attr('disabled', false);

            $("#<%=ddl_pers_sPersonaTipoId.ClientID %>").val("2101");
            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val("0");

            var EmprDocTipodocId = $("#<%= ddl_empr_sTipoDocumentoId.ClientID %>").val();
            var EmprDocNumero = $("#<%= txt_empr_vNumeroDocumento.ClientID %>").val();
            var EmprRazSocial = $("#<%= txt_empr_vRazonSocial.ClientID %>").val();

            if (EmprDocTipodocId != 0 && EmprDocNumero.length != 0 && EmprRazSocial.length != 0) {
                $("#<%=ddl_pers_sPersonaTipoId.ClientID %>").val("2102");
                $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val(EmprDocTipodocId);
                $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val(EmprDocNumero);
                $("#<%=txt_empr_vRazonSocial.ClientID %>").val(EmprRazSocial);
            }
            else {
                $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val("0");
                $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val("");
                $("#<%=txt_empr_vRazonSocial.ClientID %>").val("");
                $('#divRepresentantesLegales').hide();

                $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").attr('disabled', 'disabled');
                $("#<%=txt_empr_vNumeroDocumento.ClientID %>").attr('disabled', 'disabled');
                $("#<%=txt_empr_vRazonSocial.ClientID %>").attr('disabled', 'disabled');
            }

            //$("#<%=ddl_pers_sNacionalidadId.ClientID %>").val("0");
            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val("0");
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val("");
            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', false);
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr('disabled', false);
          //  $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").focus();
            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");
            $("#<%=txt_pers_vApellidoCasada.ClientID %>").val("");

            $("#<%=txt_pers_vNombres.ClientID %>").val("");
            $("#<%=ddl_pers_sGeneroId.ClientID %>").val("0");
            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").val("0");
            $("#<%=ddl_pers_sProfesionId.ClientID %>").val("0");
            $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").val("0");

            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").val("");
            $("#<%=ddl_UbigeoPais.ClientID %>").val("0");
            $("#<%=ddl_UbigeoRegion.ClientID %>").val("0");
            $("#<%=ddl_UbigeoCiudad.ClientID %>").val("0");
            $("#<%=txt_resi_vCodigoPostal.ClientID %>").val("");

            $("#<%=hdn_acpa_residenciaId.ClientID %>").val("0");
            $("#<%=hdn_acpa_provincia.ClientID %>").val("0");
            $("#<%=hdn_acpa_distrito.ClientID %>").val("0");

            //$("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', 'disabled');
            //            
            //            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', 'disabled');

            //$("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoCasada.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sGeneroId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sProfesionId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=chkIncapacitado.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_resi_vCodigoPostal.ClientID %>").css("border", "solid #888888 1px");
        }

        function tab_02_Limpiar() {
            $("#imgBuscarPN").attr('disabled', false);
            $("#<%=ddlPaisOrigen.ClientID %>").attr('disabled', false);
            $("#<%=ddl_pers_sPersonaTipoId.ClientID %>").val("2101");
            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").val("0");
         //   $("#<%=ddlPaisOrigen.ClientID %>").val("173");

            $("#<%=ddl_empr_sTipoDocumentoId.ClientID %>").val("0");
            

            $("#<%=txt_empr_vNumeroDocumento.ClientID %>").val("");
            $("#<%= txtDescOtroDocumento.ClientID %>").val("");
            $("#<%=txt_empr_vRazonSocial.ClientID %>").val("");
            $('#divRepresentantesLegales').hide();

            //$("#<%=ddl_pers_sNacionalidadId.ClientID %>").val("0");
            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val("0");
            /*$("#<%=ddl_peid_sDocumentoTipoId.ClientID %>" + " option").each(function () {
                if ($(this).text() === "DNI") {
                    console.log($(this).val() + "___" + $(this).text());
                    $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").val($(this).val());
                    $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('selected', 'selected');
                }
            });*/

            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").val("");

            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', false);
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr('disabled', false);
            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").focus();
            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").val("");
            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").val("");
            $("#<%=txt_pers_vApellidoCasada.ClientID %>").val("");

            $("#<%=txt_pers_vNombres.ClientID %>").val("");
            $("#<%=ddl_pers_sGeneroId.ClientID %>").val("0");
            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").val("0");
            $("#<%=ddl_pers_sProfesionId.ClientID %>").val("0");
            $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").val("0");
            $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', false);
            $("#<%=chkIncapacitado.ClientID %>").prop("checked", "");
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").val("");

            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").val("");
            $("#<%=ddl_UbigeoPais.ClientID %>").val("0");
            $("#<%=ddl_UbigeoRegion.ClientID %>").val("0");
            $("#<%=ddl_UbigeoCiudad.ClientID %>").val("0");
            $("#<%=txt_resi_vCodigoPostal.ClientID %>").val("");

            $("#<%=hdn_acpa_residenciaId.ClientID %>").val("0");
            $("#<%=hdn_acpa_provincia.ClientID %>").val("0");
            $("#<%=hdn_acpa_distrito.ClientID %>").val("0");            

            // $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', 'disabled');

            //            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', 'disabled');
            //            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', 'disabled');

            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").css("border", "solid #888888 1px");
            // $("#<%=ddl_pers_sNacionalidadId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vApellidoCasada.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_pers_vNombres.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sGeneroId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sProfesionId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=chkIncapacitado.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoPais.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoRegion.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_UbigeoCiudad.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_resi_vCodigoPostal.ClientID %>").css("border", "solid #888888 1px");

//            $("#<%=lblAstGenero.ClientID %>").hide();
//            $("#<%=lblAstEstCivil.ClientID %>").hide();
//            $("#<%=lblAstProf.ClientID %>").hide();
//            $("#<%=lblAstIdioma.ClientID %>").hide();
//            $("#<%=lblAstDomicilio.ClientID %>").hide();
//            $("#<%=lblAstDpto.ClientID %>").hide();
//            $("#<%=lblAstProv.ClientID %>").hide();
//            $("#<%=lblAstDist.ClientID %>").hide();

            $("#<%=hdn_IndiceGrillaParticipantesEdicion.ClientID %>").val("0");
            $("#<%=HF_REGISTRO_NUEVO.ClientID %>").val("-1");

            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").focus();

            $("#<%=chkIncapacitado.ClientID %>").hide();
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").hide();
            $("#<%=lblIncapacitadoTitulo.ClientID %>").hide();
            $("#<%=lblIncapacidadTitulo.ClientID %>").hide();
            $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
            $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
            $("#<%=ddl_Participante_Discapacidad.ClientID %>").val("0");
            $("#<%=ddl_Participante_Interprete.ClientID %>").val("0");

            $("#<%= hdn_Tipo_Participante_Editando.ClientID %>").val("-1");
            $("#<%= btnAgregarParticipanteNew.ClientID %>").attr('value', '  Agregar');

            OcularListaInterpretes();
            $("#<%=ddlPaisOrigen.ClientID %>").val("0");
            $("#<%=LblDescNacionalidadCopia.ClientID %>").text("");
            ActivarOtrodocumento();
            $("#trCodigoPostal").hide();
        }
        function limpiarCamposAprobacionPagos() {
           
            $("#<%=ddlTipoPago.ClientID %>").attr('disabled', false);
            $("#<%=Txt_TarifaCantidad.ClientID %>").val("1");
            //$("#<%=Txt_TarifaId.ClientID %>").val("");

            $("#<%= Txt_MtoSC.ClientID %>").val("");
            $("#<%=Txt_MontML.ClientID %>").val("");
            $("#<%=Txt_TotSC.ClientID %>").val("");
            $("#<%=Txt_TotML.ClientID %>").val("");

            $("#<%= txtNroOperacion.ClientID %>").val("");
            $("#<%=ddlNomBanco.ClientID %>").val("0");

            //$('#Gdv_Tarifa').remove();


        }
        function deshabilitarAux() {
            $("#<%=ddl_peid_sDocumentoTipoId.ClientID %>").attr('disabled', false);
            $("#<%=txt_peid_vDocumentoNumero.ClientID %>").attr('disabled', false);
            $("#imgBuscarPN").attr('disabled', false);
            // $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', true);
            //            $("#<%=txt_pers_vApellidoPaterno.ClientID %>").attr('disabled', true);
            //            $("#<%=txt_pers_vApellidoMaterno.ClientID %>").attr('disabled', true);
            //            $("#<%=txt_pers_vNombres.ClientID %>").attr('disabled', true);

            var strGeneroId = $.trim($("#<%= ddl_pers_sGeneroId.ClientID %>").val());
            var strEstadoCivilId = $.trim($("#<%= ddl_pers_sEstadoCivilId.ClientID %>").val());
            // var str_nacionalidad = $.trim($("#<%= ddl_pers_sNacionalidadId.ClientID %>").val());

            var strsProfesionId = $.trim($("#<%= ddl_pers_sProfesionId.ClientID %>").val());
            var strsIdiomaNatalId = $.trim($("#<%= ddl_pers_sIdiomaNatalId.ClientID %>").val());

            var vResidenciaDireccion = $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").val();
            var ddl_UbigeoPais = $("#<%=ddl_UbigeoPais.ClientID %>").val();
            var ddl_UbigeoRegion = $("#<%=ddl_UbigeoRegion.ClientID %>").val();
            var ddl_UbigeoCiudad = $("#<%=ddl_UbigeoCiudad.ClientID %>").val();


            /*
            if (str_nacionalidad == "0") {
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', false);
            }
            else {
            $("#<%=ddl_pers_sNacionalidadId.ClientID %>").attr('disabled', true);
            }
            */
            //            if (strGeneroId == "0") {
            //                $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=ddl_pers_sGeneroId.ClientID %>").attr('disabled', true);
            //            }

            //            if (strEstadoCivilId == "0") {
            //                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=ddl_pers_sEstadoCivilId.ClientID %>").attr('disabled', true);
            //            }

            //            if (strsProfesionId == "0") {
            //                $("#<%=ddl_pers_sProfesionId.ClientID %>").attr('disabled', false);
            //            }

            //            if (strsIdiomaNatalId == "0") {
            //                $("#<%=ddl_pers_sIdiomaNatalId.ClientID %>").attr('disabled', false);
            //            }

            //            if (ddl_UbigeoPais == "0" || ddl_UbigeoPais == "00") {
            //                $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=ddl_UbigeoPais.ClientID %>").attr('disabled', true);
            //            }

            //            if (ddl_UbigeoRegion == "0" || ddl_UbigeoRegion == "00") {
            //                $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=ddl_UbigeoRegion.ClientID %>").attr('disabled', true);
            //            }

            //            if (ddl_UbigeoCiudad == "0" || ddl_UbigeoCiudad == "00") {
            //                $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=ddl_UbigeoCiudad.ClientID %>").attr('disabled', true);
            //            }

            //            if (vResidenciaDireccion.length == 0) {
            //                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', false);
            //            }
            //            else {
            //                $("#<%=txt_resi_vResidenciaDireccion.ClientID %>").attr('disabled', true);
            //            }

            //            $("#<%=txt_resi_vCodigoPostal.ClientID %>").attr('disabled', false);
        }


        function MostrasIncapacidad() {
            $("#<%=chkIncapacitado.ClientID %>").show();
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").show();
            $("#<%=lblIncapacitadoTitulo.ClientID %>").show();
            $("#<%=lblIncapacidadTitulo.ClientID %>").show();

        }
        function MostrarHuella() {
            $("#<%=lblHuella.ClientID %>").css('visibility', 'visible');
            $("#<%=chkNoHuella.ClientID %>").css('visibility', 'visible');
        }
        function OcultarHuella() {
            $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
            $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
        }
        function OcultarIncapacidad() {
            $("#<%=chkIncapacitado.ClientID %>").hide();
            $("#<%=txtRegistroTipoIncapacidad.ClientID %>").hide();
            $("#<%=lblIncapacitadoTitulo.ClientID %>").hide();
            $("#<%=lblIncapacidadTitulo.ClientID %>").hide();
        }

        function MostrarlistaIncapacidad() {
            $("#<%=lblParticipanteDiscapacidad.ClientID %>").show();
            $("#<%=ddl_Participante_Discapacidad.ClientID %>").show();
        }
        function OcularListaIncapacidad() {
            $("#<%=lblParticipanteDiscapacidad.ClientID %>").hide();
            $("#<%=ddl_Participante_Discapacidad.ClientID %>").hide();
        }
        function MostrarlistaInterpretes() {
            $("#<%=lblParticipanteConInterprete.ClientID %>").show();
            $("#<%=ddl_Participante_Interprete.ClientID %>").show();
        }
        function OcularListaInterpretes() {
            $("#<%=lblParticipanteConInterprete.ClientID %>").hide();
            $("#<%=ddl_Participante_Interprete.ClientID %>").hide();
        }


        function ddl_UbigeoPais_change() {
            var ddl_DeptOcurrencia = $("#<%=ddl_UbigeoPais.ClientID %>").val();
            var ddl1 = $("#<%= ddl_UbigeoRegion.ClientID %>");
            var ddl2 = $("#<%= ddl_UbigeoCiudad.ClientID %>");
            $("#<%=hdn_acpa_pais.ClientID %>").val(ddl_DeptOcurrencia)
            funSetUbigeoProvincia(ddl1, ddl_DeptOcurrencia);
            funSetUbigeoDistrito(ddl2, 0, 0);
            if ($("#<%=ddl_UbigeoPais.ClientID %>").val() != "0") {
                $("#<%=ddl_UbigeoRegion.ClientID %>").focus();
            }
        }
        function ddl_UbigeoRegion_change() {
            var ddl = $("#<%= ddl_UbigeoCiudad.ClientID %>");
            var ddl_DeptOcurrencia = $("#<%=ddl_UbigeoPais.ClientID %>").val();
            var ddl_ProvOcurrencia = $("#<%=ddl_UbigeoRegion.ClientID %>").val();
            $("#<%=hdn_acpa_provincia.ClientID %>").val(ddl_ProvOcurrencia)
            funSetUbigeoDistrito(ddl, ddl_DeptOcurrencia, ddl_ProvOcurrencia);
            if ($("#<%=ddl_UbigeoRegion.ClientID %>").val() != "00") {                
                $("#<%=ddl_UbigeoCiudad.ClientID %>").focus();
            }
        }
        function ddl_UbigeoCiudad_change() {
            var ddl_distrito = $("#<%=ddl_UbigeoCiudad.ClientID %>").val();
            if (ddl_distrito > 0) {
                $("#<%=hdn_acpa_distrito.ClientID %>").val(ddl_distrito);
            }
            else {
                $("#<%=hdn_acpa_distrito.ClientID %>").val(0);
            }
        }

        function mostrarUbigeo() {
            var ubigeo = $("#<%=hubigeoLoad.ClientID %>").val();
            var pais = ubigeo.substring(0, 2);
            var region = ubigeo.substring(2, 4);
            var ciudad = ubigeo.substring(4, 6); 
            
            $('#<%= ddl_UbigeoPais.ClientID %>').val(pais);
            $('#<%= hdn_acpa_pais.ClientID %>').val(pais);            

            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPais.ClientID %>');

            $('#<%= ddl_UbigeoRegion.ClientID %>').empty();
            $('#<%= ddl_UbigeoCiudad.ClientID %>').empty();

            var objetoProvincia = localStorage.getItem("objProvincia");
            var listaProvincia = JSON.parse(objetoProvincia);
            
            $('#<%= ddl_UbigeoRegion.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaProvincia).each(function () {
                if (this.Ubi01 == ddlDepartamento.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Provincia);
                    option.val(this.Ubi02);
                    $('#<%= ddl_UbigeoRegion.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoRegion.ClientID %>').removeAttr("disabled");
                }
            });

            $('#<%= ddl_UbigeoRegion.ClientID %>').val(region);
            $('#<%= hdn_acpa_provincia.ClientID %>').val(region);

            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPais.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddl_UbigeoRegion.ClientID %>');

            var objetoDistrito = localStorage.getItem("objDistrito");
            var listaDistrito = JSON.parse(objetoDistrito);

            $('#<%= ddl_UbigeoCiudad.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaDistrito).each(function () {
                if (this.Ubi01 == ddlDepartamento.value && this.Ubi02 == ddlProvincia.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Distrito);
                    option.val(this.Ubi03);
                    $('#<%= ddl_UbigeoCiudad.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoCiudad.ClientID %>').removeAttr("disabled");
                }
            });

            $('#<%= ddl_UbigeoCiudad.ClientID %>').val(ciudad);
            $('#<%= hdn_acpa_distrito.ClientID %>').val(ciudad);

            $('#<%= hubigeo.ClientID %>').val(ubigeo);
            var ddlCiudad = document.getElementById('<%=ddl_UbigeoCiudad.ClientID %>');            
        }

        function cargarProvincia() {
            $('#<%= hubigeo.ClientID %>').val("");
            $('#<%= ddl_UbigeoRegion.ClientID %>').empty();
            $('#<%= ddl_UbigeoCiudad.ClientID %>').empty();
            $('#<%= ddl_UbigeoCiudad.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');
            var objetoProvincia = localStorage.getItem("objProvincia");

            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPais.ClientID %>');

            $('#<%= hdn_acpa_pais.ClientID %>').val(ddlDepartamento.value);

            var listaProvincia = JSON.parse(objetoProvincia);

            $('#<%= ddl_UbigeoRegion.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaProvincia).each(function () {
                if (this.Ubi01 == ddlDepartamento.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Provincia);
                    option.val(this.Ubi02);
                    $('#<%= ddl_UbigeoRegion.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoRegion.ClientID %>').removeAttr("disabled");
                }
            });
        }

        function cargarDistrito() {
            $('#<%= hubigeo.ClientID %>').val("");
            $('#<%= ddl_UbigeoCiudad.ClientID %>').empty()
            var objetoDistrito = localStorage.getItem("objDistrito");

            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPais.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddl_UbigeoRegion.ClientID %>');

            $('#<%= hdn_acpa_provincia.ClientID %>').val(ddlProvincia.value);
            

            var ubiUSA = ddlDepartamento.value + ddlProvincia.value;
            //9213 -> Estado Unidos de América
            if (ubiUSA == "9213") {
                $("#trCodigoPostal").show();
            }
            else {
                $("#trCodigoPostal").hide();
            }

            var listaDistrito = JSON.parse(objetoDistrito);

            $('#<%= ddl_UbigeoCiudad.ClientID %>').append('<option value="0">- SELECCIONAR -</option>');

            $(listaDistrito).each(function () {
                if (this.Ubi01 == ddlDepartamento.value && this.Ubi02 == ddlProvincia.value) {
                    var option = $(document.createElement('option'));
                    option.text(this.Distrito);
                    option.val(this.Ubi03);
                    $('#<%= ddl_UbigeoCiudad.ClientID %>').append(option);
                    $('#<%= ddl_UbigeoCiudad.ClientID %>').removeAttr("disabled");
                }
            });
        }
        function cargarUbigeo() {
            var ddlDepartamento = document.getElementById('<%=ddl_UbigeoPais.ClientID %>');
            var ddlProvincia = document.getElementById('<%=ddl_UbigeoRegion.ClientID %>');
            var ddlDistrito = document.getElementById('<%=ddl_UbigeoCiudad.ClientID %>');

            $('#<%= hdn_acpa_distrito.ClientID %>').val(ddlDistrito.value);

            var ubigeo = ddlDepartamento.value + ddlProvincia.value + ddlDistrito.value;
            $('#<%= hubigeo.ClientID %>').val(ubigeo);
        }
        function Guardarlocalstorage(objProvincia, objDistrito) {
            localStorage.setItem("objProvincia", JSON.stringify(objProvincia));
            localStorage.setItem("objDistrito", JSON.stringify(objDistrito));
            console.log(objProvincia);
            console.log(objDistrito);
        }
        function tab_03_CuerpoSave() {
            var valid;
            //return new Promise(function (resolve, reject) {

                                        var rpta = isSessionTimeOut();
            
                                        if (rpta != 'ok') return;
            
                                        if (ValidarTabCuerpo()) {


                                            $("#<%= HF_NUMERO_PAGINA_DOCUMENTO.ClientID %>").val("1");
                                            $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", true);
                                            $("#<%=cbxAfirmarTexto.ClientID %>").prop("disabled", true);//--
                                            document.getElementById('MainContent_cbxAfirmarTexto').parentElement.disabled = true;
                                            $("#<%=cbxAfirmarTexto.ClientID %>").prop('checked', false);
                                            IdentityName = "ancu_iActoNotarialCuerpoId";
                                            var cuerpo = {};

                                            cuerpo.ancu_iActoNotarialCuerpoId = $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val();
                                            cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();

                                            var strTextoAbogado = $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val().toUpperCase();
                                            var strTextoNroColegiatura = $("#<%= txt_acno_vNumeroColegiatura.ClientID %>").val().toUpperCase();
                                            var strNroFirmasIlegibles = $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val();
                                            var chkMinuta = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');

                                            var strTextoIntroduccion = $("#<%= lblIntro.ClientID %>").html();



                                            var strTextoNormativo = $("#<%= txtTextoNormativo.ClientID %>").val();

                                            var strTextNormativoFormateado = "";

                                            var strTextoMinuta = "";

                                            var str_acno_vNombreColegiatura = $("#<%=txt_acno_vNombreColegiatura.ClientID %>").val().toUpperCase();

                                            if (strTextoNormativo.length > 0) {
                                                strTextNormativoFormateado = "<p style='text-align:justify;'>" + strTextoNormativo + "</p>";
                                            }

                                            //-------------------------------------
                                            //Fecha: 19/09/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Quitar el siguiente texto cuando es minuta.
                                            //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
                                            //-------------------------------------
                                            //                if (chkMinuta) {

                                            //                    strTextoMinuta += "<p style='text-align:justify;'>" + strNroFirmasIlegibles + " FIRMAS ILEGIBLES.</p>";
                                            //                    strTextoMinuta += "<p style='text-align:justify;'> AUTORIZA LA MINUTA: " + strTextoAbogado + " - ABOGADO CON REGISTRO DEL " + str_acno_vNombreColegiatura + " NÚMERO: " + strTextoNroColegiatura + ".</p>";
                                            //                }

                                            //var strTextoCentral = "<p style='text-align:justify;'>" + tinyMCE.editors[0].getContent() + "</p>";
                                            var strTextoCentral = tinyMCE.editors[0].getContent();

                                            var textoCentral = "";
                                            $(strTextoCentral + ' > p').each(function (idx, el) {
                                                if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                                                    //                        textoCentral += "<p style='text-align:justify;'>";
                                                    //                        textoCentral += "PRIMERO: " + $(el).text().replace("PRIMERO:", "").trim().toUpperCase();
                                                    //                        textoCentral += "</p>";
                                                    if ($(el).text().trim().length > 0) {
                                                        textoCentral += "<p style='text-align:justify;'>";
                                                        textoCentral += $(el).text().trim().toUpperCase();
                                                        textoCentral += "</p>";
                                                    }
                                                }
                                            });


                                            strTextoCentral = textoCentral;

                                            //-----------------------------------------

                                            var strDL1049Articulo55C = tinyMCE.editors[1].getContent();

                                            var DL1049Articulo55C = "";

                                            $(strDL1049Articulo55C + ' > p').each(function (idx, el) {
                                                if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                                                    //                        textoCentral += "<p style='text-align:justify;'>";
                                                    //                        textoCentral += "PRIMERO: " + $(el).text().replace("PRIMERO:", "").trim().toUpperCase();
                                                    //                        textoCentral += "</p>";
                                                    if ($(el).text().trim().length > 0) {
                                                        DL1049Articulo55C += $(el).text().trim().toUpperCase();
                                                    }
                                                }
                                            });
                                            //-----------------------------------------

                                            var strTextoConclusion = "<p style='text-align:justify;'>" + $("#<%= lblConcluciones.ClientID %>").html() + "</p>";

                                            var strTextoFinal = "<p style='text-align:justify;'>" + $("#<%= lblFinal.ClientID %>").html() + "</p>";

                                            var strTextoAdicional = "<p style='text-align:justify;'>" + tinyMCE.editors[2].getContent() + "</p>";

                                            var textoAdicional = "";
                                            $(strTextoAdicional + ' > p').each(function (idx, el) {
                                                if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                                                    if ($(el).text().trim().length > 0) {
                                                        textoAdicional += "<p style='text-align:justify;'>";
                                                        textoAdicional += $(el).text().toUpperCase();
                                                        textoAdicional += "</p>";
                                                    }
                                                }
                                            });

                                            strTextoAdicional = textoAdicional;
                                            /*Imagenes*/
                                            var totalRows = 0;
                                            $('#<%=gdvImagenes.ClientID%> tr:not(:last)').each(function () {
                                                totalRows = totalRows + 1;
                                            });

                                            var vImagenes = "";
                                            if (totalRows > 0) {
                                                vImagenes = "#IMAGEN#";
                                            }

                                            var strCiudadFecha = $("#<%= HF_CIUDAD_FECHA.ClientID %>").val()

                                            var strFechaPosterior = strCiudadFecha.replace(/['"]+/g, '');
                                            var strTextoPosterior = "";

                                            //-------------------------------------
                                            //Fecha: 19/09/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Quitar el siguiente texto cuando es minuta.
                                            //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
                                            //-------------------------------------

                                            if (strCiudadFecha.length != 0 && !chkMinuta) {

                                                strTextoPosterior = "<p style='text-align:justify;'>USTED SEÑOR(A) CÓNSUL, SE SERVIRÁ AGREGAR LO QUE SEA DE LEY, ELEVAR A ESCRITURA PÚBLICA LA PRESENTE EXPRESIÓN DE VOLUNTAD Y PASAR LOS PARTES A LOS REGISTROS PÚBLICOS RESPECTIVOS PARA SU DEBIDA INSCRIPCIÓN.</p>";
                                                strFechaPosterior = "<p>" + strFechaPosterior + "</p>";

                                                $("#<%= lblCierreTextoCentral.ClientID %>").html(strTextoPosterior + strFechaPosterior);
                                            }

                                            cuerpo.ancu_vCuerpo = strTextoIntroduccion + "<tagx></tagx>" + strTextoCentral + "<tagx></tagx>" + strTextoPosterior + strFechaPosterior + "<tagx></tagx>" + strTextoMinuta + "<tagx></tagx>" + vImagenes + "<tagx></tagx>" + strTextoConclusion + "<tagx></tagx>" + strTextNormativoFormateado + "<tagx></tagx>" + strTextoAdicional + "<tagx></tagx>" + strTextoFinal;

                                            var strTextoNormativoSel = "";
                                            $('#<%= divListNormativo.ClientID %>').children('input').each(function (e, c) {
                                                if (this.checked) {
                                                    strTextoNormativoSel += $(this).val() + "|";
                                                }
                                            });
                                            cuerpo.ancu_vTextoCentral = strTextoCentral;
                                            cuerpo.ancu_vTextoAdicional = strTextoAdicional;
                                            cuerpo.ancu_vTextoNormativo = strTextoNormativoSel;
                                            cuerpo.ancu_vDL1049Articulo55C = DL1049Articulo55C;
                                            //------------------------------------------------------------------------------------------------------------------------

                                            cuerpo.ancu_vFirmaIlegible = $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val().toUpperCase();
                                            cuerpo.acno_sAutorizacionTipoId = $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").val().toUpperCase();
                                            cuerpo.acno_vAutorizacionDocumentoNumero = $("#<%=txtAutorizacionNroDocumento.ClientID %>").val().toUpperCase();

                                            cuerpo.acno_vNombreColegiatura = str_acno_vNombreColegiatura;
                                            cuerpo.acno_vNumeroColegiatura = $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").val().toUpperCase();

                                            cuerpo.acno_vNumeroLibro = $("#<%=txtNroLibro.ClientID %>").val().toUpperCase();
                                            cuerpo.acno_vNumeroFojaInicial = $("#<%= txtNroFojaIni.ClientID %>").val();
                                            cuerpo.acno_vNumeroFojaFinal = $("#<%= txtNroFojaFinal.ClientID %>").val();
                                            cuerpo.acno_sNumeroHojas = $("#<%= HF_NroHojas.ClientID %>").val();
                                            //--se agrega los campos de montos
                                            cuerpo.acno_nCostoEP = $("#<%= txtCostoEP.ClientID %>").val();
                                            cuerpo.acno_nCostoParte2 = $("#<%= txtCostoParte2.ClientID %>").val();
                                            cuerpo.acno_nCostoTestimonio = $("#<%= txtCostoTestimonio.ClientID %>").val();

                                            cuerpo.acno_vNumeroEscrituraPublica = $("#<%=txtNroEscritura.ClientID %>").val().toUpperCase();
                                            cuerpo.acno_iNumeroActoNotarial = $.trim($("#<%= txt_acno_Numero_Minuta.ClientID %>").val());

                                            cuerpo.acno_vNroOficio = $("#<%= txtNroOficio.ClientID %>").val();
                                            cuerpo.acno_vRegistrador = $("#<%= txtRegistradorNombres.ClientID %>").val();
                                            cuerpo.acno_iOficinaRegistralId = $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").val().toString();
                                            cuerpo.acno_vPresentante = $("#<%= txtRepresentanteNombres.ClientID %>").val();
                                            cuerpo.acno_iTipoDocRepresentante = $.trim($("#<%= ddl_TipoDocrepresentante.ClientID %>").val());
                                            cuerpo.acno_vNumeroDocumentoRepresentante = $.trim($("#<%= txtRepresentanteNroDoc.ClientID %>").val());
                                            cuerpo.acno_sPresentanteGenero = $.trim($("#<%= ddl_GerenoPresentante.ClientID %>").val());
                                            cuerpo.strTipoActoNotarial = $.trim($("#<%= Cmb_TipoActoNotarial.ClientID %> option:selected").text());

                                            cuerpo.acno_dFechaConclusionFirma = $("#<%= hdn_acno_dFechaConclusionFirma.ClientID %>").val();



                                            if (cuerpo.ancu_iActoNotarialId != 0) {
                                                var sndprm = {};
                                                sndprm.cuerpo = JSON.stringify(cuerpo);

                                                $.ajax({
                                                    type: "POST",
                                                    url: "FrmActoNotarialProtocolares.aspx/insert_cuerpo",
                                                    data: JSON.stringify(sndprm),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (rsp) {
                                                        if (rsp.d != null) {
                                                            var oResponse = JSON.parse(rsp.d);
                                                            if (oResponse.Identity != 0 && oResponse.Message == null) {
                                                                $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val(oResponse.Identity);
                                                                $("#<%=Btn_VistaPreviaAprobar.ClientID %>").removeAttr('disabled');

                                                                //esto se comenta por que se mostraba al mismo tiemo con el vista previa de pdf y luego desaparece el mensaje,
                                                                showdialog('i', 'Registro Notarial : Cuerpo', 'El registro ha sido grabado correctamente', false, 160, 300);
                                                                return true;
                                                            }
                                                        }
                                                        else {
                                                            if (oResponse.Identity == 0 && oResponse.Message != null) {

                                                                showdialog('a', 'Registro Notarial : Cuerpo', oResponse.Message.toString(), false, 160, 300);
                                                            }
                                                            return false;
                                                        }
                                                    },
                                                    error: errores

                                                });
                                            }
                                            else {
                                                showdialog('a', 'Registro Notarial : Cuerpo', 'Debe Grabarse el Acto Notarial Previamente.', false, 160, 300);
                                                return false;
                                            }

                                        }
                                        else {
                                            showdialog('a', 'Registro Notarial : Cuerpo', 'Falta completar los datos.', false, 160, 300);
                                            return false;
                                        }

            //});
        }

        function check_AprobarVistaPrevia() {

            /*if ($("#<%= cbxAfirmarTexto.ClientID %>").is(':checked')) {
                HabilitarTabPagos();
                //$("#btnSave_tab3").prop('disabled', true);
                
                $("#btnCncl_tab3").prop('disabled', true);
                $("#btnTraerCuerpo").prop('disabled', true);
                $("#<%= Btn_VistaPreviaAprobar.ClientID %>").prop('disabled', true);
                
            } else {
                //$("#btnSave_tab3").prop('disabled', false);
                $("#btnCncl_tab3").prop('disabled', false);
                //$("#btnTraerCuerpo").prop('disabled', false);
            }*/

            HabilitarTabPagos();
            //$("#btnSave_tab3").prop('disabled', true);
            $("#btnCncl_tab3").prop('disabled', true);
            $("#btnTraerCuerpo").prop('disabled', true);
            $("#<%= Btn_VistaPreviaAprobar.ClientID %>").prop('disabled', true);
            //$("#<%= Btn_AfirmarTextoLeido.ClientID %>").prop('disabled', true);
            


        }

        function check_HabilitarIncapacidad() {
            if ($("#<%= chkIncapacitado.ClientID %>").is(':checked')) {
                $("#<%=txtRegistroTipoIncapacidad.ClientID %>").prop('disabled', false);
                $("#<%=lblHuella.ClientID %>").css('visibility', 'visible');
                $("#<%=chkNoHuella.ClientID %>").css('visibility', 'visible');
                $("#<%=chkNoHuella.ClientID %>").prop('checked', false);
                $("#<%= txtRegistroTipoIncapacidad.ClientID %>").focus();

            } else {
                $("#<%=txtRegistroTipoIncapacidad.ClientID %>").prop('disabled', true);
                $("#<%=txtRegistroTipoIncapacidad.ClientID %>").val('');
                $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
                $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
            }
        }

        function tab_03_VistaPrevia(vVistaPrevia) {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var bCorrecto = true;
            var textoCentral = "";
            var strTextoCentral = tinyMCE.editors[0].getContent();
            var strTextoCentral2 = strTextoCentral.trim();

            strTextoCentral2 = strTextoCentral2.replace(/\n/g, " "); //replace break lines
            strTextoCentral2 = strTextoCentral2.split('&nbsp;').join(' '); //replace &nbsp; multiples
            strTextoCentral2 = strTextoCentral2.replace(/\s+/g, ' ');
            strTextoCentral2 = strTextoCentral2.replace(/\s{2,}/g, ' '); //tabas, multiple spaces
            if (strTextoCentral2.length > 0) {
                var arr = strTextoCentral2.split(" ");
                arr.forEach(function (palabra) {
                    if (palabra.length > 67 && !palabra.includes('----------') && !palabra.includes('======')) {
                        showdialog('a', 'Registro Notarial : Cuerpo', 'Hay una palabra demasiado larga en el texto central.', false, 160, 600);
                        bCorrecto = false;
                    }
                }, this);

            }
            if (bCorrecto == false) {
                return false;
            }
           
           /*
            if (vVistaPrevia == '1') {
                var isOkSave=await  tab_03_CuerpoSave();
                if (isOkSave == false) return false;
            }
            */

            /*var ejecutoOk = false;            
            if (vVistaPrevia == '1') {
                if (tab_03_CuerpoSave() == false){ 
                    return false;
                }
                ejecutoOk=true;
            }
            if (vVistaPrevia == '1' && ejecutoOk==false) {
                return false;
            }*/

            //--------------------tab_03_CuerpoSave()
            if (ValidarTabCuerpo()) {


                $("#<%= HF_NUMERO_PAGINA_DOCUMENTO.ClientID %>").val("1");
                $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", true);
                $("#<%=cbxAfirmarTexto.ClientID %>").prop("disabled", true); //--
                document.getElementById('MainContent_cbxAfirmarTexto').parentElement.disabled = true;
                $("#<%=cbxAfirmarTexto.ClientID %>").prop('checked', false);
                IdentityName = "ancu_iActoNotarialCuerpoId";
                var cuerpo = {};

                cuerpo.ancu_iActoNotarialCuerpoId = $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val();
                cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();

                var strTextoAbogado = $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val().toUpperCase();
                var strTextoNroColegiatura = $("#<%= txt_acno_vNumeroColegiatura.ClientID %>").val().toUpperCase();
                var strNroFirmasIlegibles = $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val();
                var chkMinuta = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');

                var strTextoIntroduccion = $("#<%= lblIntro.ClientID %>").html();



                var strTextoNormativo = $("#<%= txtTextoNormativo.ClientID %>").val();

                var strTextNormativoFormateado = "";

                var strTextoMinuta = "";

                var str_acno_vNombreColegiatura = $("#<%=txt_acno_vNombreColegiatura.ClientID %>").val().toUpperCase();

                if (strTextoNormativo.length > 0) {
                    strTextNormativoFormateado = "<p style='text-align:justify;'>" + strTextoNormativo + "</p>";
                }

                //-------------------------------------
                //Fecha: 19/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Quitar el siguiente texto cuando es minuta.
                //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
                //-------------------------------------
                //                if (chkMinuta) {

                //                    strTextoMinuta += "<p style='text-align:justify;'>" + strNroFirmasIlegibles + " FIRMAS ILEGIBLES.</p>";
                //                    strTextoMinuta += "<p style='text-align:justify;'> AUTORIZA LA MINUTA: " + strTextoAbogado + " - ABOGADO CON REGISTRO DEL " + str_acno_vNombreColegiatura + " NÚMERO: " + strTextoNroColegiatura + ".</p>";
                //                }

                //var strTextoCentral = "<p style='text-align:justify;'>" + tinyMCE.editors[0].getContent() + "</p>";
                var strTextoCentral = tinyMCE.editors[0].getContent();

                var textoCentral = "";
                $(strTextoCentral + ' > p').each(function (idx, el) {
                    if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                        //                        textoCentral += "<p style='text-align:justify;'>";
                        //                        textoCentral += "PRIMERO: " + $(el).text().replace("PRIMERO:", "").trim().toUpperCase();
                        //                        textoCentral += "</p>";
                        if ($(el).text().trim().length > 0) {
                            textoCentral += "<p style='text-align:justify;'>";
                            textoCentral += $(el).text().trim().toUpperCase();
                            textoCentral += "</p>";
                        }
                    }
                });


                strTextoCentral = textoCentral;

                //-----------------------------------------

                var strDL1049Articulo55C = tinyMCE.editors[1].getContent();

                var DL1049Articulo55C = "";

                $(strDL1049Articulo55C + ' > p').each(function (idx, el) {
                    if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                        //                        textoCentral += "<p style='text-align:justify;'>";
                        //                        textoCentral += "PRIMERO: " + $(el).text().replace("PRIMERO:", "").trim().toUpperCase();
                        //                        textoCentral += "</p>";
                        if ($(el).text().trim().length > 0) {
                            DL1049Articulo55C += $(el).text().trim().toUpperCase();
                        }
                    }
                });
                //-----------------------------------------

                var strTextoConclusion = "<p style='text-align:justify;'>" + $("#<%= lblConcluciones.ClientID %>").html() + "</p>";

                var strTextoFinal = "<p style='text-align:justify;'>" + $("#<%= lblFinal.ClientID %>").html() + "</p>";

                var strTextoAdicional = "<p style='text-align:justify;'>" + tinyMCE.editors[2].getContent() + "</p>";

                var textoAdicional = "";
                $(strTextoAdicional + ' > p').each(function (idx, el) {
                    if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                        if ($(el).text().trim().length > 0) {
                            textoAdicional += "<p style='text-align:justify;'>";
                            textoAdicional += $(el).text().toUpperCase();
                            textoAdicional += "</p>";
                        }
                    }
                });

                strTextoAdicional = textoAdicional;
                /*Imagenes*/
                var totalRows = 0;
                $('#<%=gdvImagenes.ClientID%> tr:not(:last)').each(function () {
                    totalRows = totalRows + 1;
                });

                var vImagenes = "";
                if (totalRows > 0) {
                    vImagenes = "#IMAGEN#";
                }

                var strCiudadFecha = $("#<%= HF_CIUDAD_FECHA.ClientID %>").val()

                var strFechaPosterior = strCiudadFecha.replace(/['"]+/g, '');
                var strTextoPosterior = "";

                //-------------------------------------
                //Fecha: 19/09/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Quitar el siguiente texto cuando es minuta.
                //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
                //-------------------------------------

                if (strCiudadFecha.length != 0 && !chkMinuta) {

                    strTextoPosterior = "<p style='text-align:justify;'>USTED SEÑOR(A) CÓNSUL, SE SERVIRÁ AGREGAR LO QUE SEA DE LEY, ELEVAR A ESCRITURA PÚBLICA LA PRESENTE EXPRESIÓN DE VOLUNTAD Y PASAR LOS PARTES A LOS REGISTROS PÚBLICOS RESPECTIVOS PARA SU DEBIDA INSCRIPCIÓN.</p>";
                    strFechaPosterior = "<p>" + strFechaPosterior + "</p>";

                    $("#<%= lblCierreTextoCentral.ClientID %>").html(strTextoPosterior + strFechaPosterior);
                }

                cuerpo.ancu_vCuerpo = strTextoIntroduccion + "<tagx></tagx>" + strTextoCentral + "<tagx></tagx>" + strTextoPosterior + strFechaPosterior + "<tagx></tagx>" + strTextoMinuta + "<tagx></tagx>" + vImagenes + "<tagx></tagx>" + strTextoConclusion + "<tagx></tagx>" + strTextNormativoFormateado + "<tagx></tagx>" + strTextoAdicional + "<tagx></tagx>" + strTextoFinal;

                var strTextoNormativoSel = "";
                $('#<%= divListNormativo.ClientID %>').children('input').each(function (e, c) {
                    if (this.checked) {
                        var ides = this.id.replace(/check/g,"");
                        strTextoNormativoSel += ides + "|";
                    }
                });
                $("#<%= hf_idsNormativos.ClientID %>").val(strTextoNormativoSel);
                cuerpo.ancu_vTextoCentral = strTextoCentral;
                cuerpo.ancu_vTextoAdicional = strTextoAdicional;
                cuerpo.ancu_vTextoNormativo = strTextoNormativoSel;
                cuerpo.ancu_vDL1049Articulo55C = DL1049Articulo55C;
                //------------------------------------------------------------------------------------------------------------------------

                cuerpo.ancu_vFirmaIlegible = $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val().toUpperCase();
                cuerpo.acno_sAutorizacionTipoId = $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").val().toUpperCase();
                cuerpo.acno_vAutorizacionDocumentoNumero = $("#<%=txtAutorizacionNroDocumento.ClientID %>").val().toUpperCase();
                cuerpo.acno_vNombreColegiatura = str_acno_vNombreColegiatura;
                cuerpo.acno_vNumeroColegiatura = $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").val().toUpperCase();

                cuerpo.acno_vNumeroLibro = $("#<%=txtNroLibro.ClientID %>").val().toUpperCase();
                cuerpo.acno_vNumeroFojaInicial = $("#<%= txtNroFojaIni.ClientID %>").val();
                cuerpo.acno_vNumeroFojaFinal = $("#<%= txtNroFojaFinal.ClientID %>").val();
                cuerpo.acno_sNumeroHojas = $("#<%= HF_NroHojas.ClientID %>").val();
                //--se agrega los campos de montos
                cuerpo.acno_nCostoEP = $("#<%= txtCostoEP.ClientID %>").val();
                cuerpo.acno_nCostoParte2 = $("#<%= txtCostoParte2.ClientID %>").val();
                cuerpo.acno_nCostoTestimonio = $("#<%= txtCostoTestimonio.ClientID %>").val();

                cuerpo.acno_vNumeroEscrituraPublica = $("#<%=txtNroEscritura.ClientID %>").val().toUpperCase();
                cuerpo.acno_iNumeroActoNotarial = $.trim($("#<%= txt_acno_Numero_Minuta.ClientID %>").val());

                cuerpo.acno_vNroOficio = $("#<%= txtNroOficio.ClientID %>").val();
                cuerpo.acno_vRegistrador = $("#<%= txtRegistradorNombres.ClientID %>").val();
                cuerpo.acno_iOficinaRegistralId = $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").val().toString();
                cuerpo.acno_vPresentante = $("#<%= txtRepresentanteNombres.ClientID %>").val();
                cuerpo.acno_iTipoDocRepresentante = $.trim($("#<%= ddl_TipoDocrepresentante.ClientID %>").val());
                cuerpo.acno_vNumeroDocumentoRepresentante = $.trim($("#<%= txtRepresentanteNroDoc.ClientID %>").val());
                cuerpo.acno_sPresentanteGenero = $.trim($("#<%= ddl_GerenoPresentante.ClientID %>").val());
                cuerpo.strTipoActoNotarial = $.trim($("#<%= Cmb_TipoActoNotarial.ClientID %> option:selected").text());

                cuerpo.acno_dFechaConclusionFirma = $("#<%= hdn_acno_dFechaConclusionFirma.ClientID %>").val();


                if (cuerpo.ancu_iActoNotarialId != 0) {
                    var sndprm = {};
                    sndprm.cuerpo = JSON.stringify(cuerpo);

                    $.ajax({
                        type: "POST",
                        url: "FrmActoNotarialProtocolares.aspx/insert_cuerpo",
                        data: JSON.stringify(sndprm),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (rsp) {
                            if (rsp.d != null) {
                                var oResponse = JSON.parse(rsp.d);
                                if (oResponse.Identity != 0 && oResponse.Message == null) {
                                    $("#<%= hdn_ancu_iActoNotarialCuerpoId.ClientID %>").val(oResponse.Identity);
                                    $("#<%=Btn_VistaPreviaAprobar.ClientID %>").removeAttr('disabled');

                                    //esto se comenta por que se mostraba al mismo tiemo con el vista previa de pdf y luego desaparece el mensaje,
                                    //showdialog('i', 'Registro Notarial : Cuerpo', 'El registro ha sido grabado correctamente', false, 160, 300);
                                    ///----------vista pdf
                                    var str_hdn_acno_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
                                    var str_tipo_formato = $("#<%=hdn_tipo_formato_proto.ClientID %>").val();
                                    var str_correlativo = $("#<%=hdn_correlativo.ClientID %>").val();
                                    var str_num_oficio = "";
                                    if ($("#<%=txtNumeroOficioAdi.ClientID %>").val() != null) {
                                        str_num_oficio = $("#<%=txtNumeroOficioAdi.ClientID %>").val();
                                        if (str_num_oficio == "") {
                                            showdialog('a', 'Registro Notarial : Formato', 'Falta ingresar Nro. de Oficio.', false, 160, 300);
                                            return false;
                                        }
                                    }

                                    var str_hdn_actonotarialdetalle = 0;
                                    //if ($("#<%=txtNumeroOficioAdi.ClientID %>").is(':enabled')) {
                                    str_hdn_actonotarialdetalle = $("#<%=hdn_actonotarialdetalle_id.ClientID %>").val();
                                    

                                    prm = {};
                                    prm.AcnoNotarialId = str_hdn_acno_iActoNotarialId;
                                    prm.TipoFormato = str_tipo_formato;
                                    prm.Correlativo = str_correlativo;
                                    prm.NumOficio = str_num_oficio;
                                    prm.ActoNotarialDetalleId = str_hdn_actonotarialdetalle;
                                    prm.vVistaPrevia = vVistaPrevia;
                                    $.ajax({
                                        type: "POST",
                                        url: "FrmActoNotarialProtocolares.aspx/ImprimirVistaPrevia",
                                        contentType: "application/json; charset=utf-8",
                                        data: JSON.stringify(prm),
                                        dataType: "json",
                                        success: function (data) {

                                            var objVistaPrevia = $.parseJSON(data.d);
                                            if (objVistaPrevia.bResultado == true) {
                                                $("#<%= HF_NUMERO_PAGINA_DOCUMENTO.ClientID %>").val(objVistaPrevia.NumeroPagina);
                                                $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", false);
                                                $("#<%= cbxAfirmarTexto.ClientID %>").prop('disabled', false);
                                                $("#<%= cbxAfirmarTexto.ClientID %>").prop("checked", false);
                                                $("#<%= cbxAfirmarTexto.ClientID %>").removeAttr("checked")
                                                document.getElementById('MainContent_cbxAfirmarTexto').parentNode.removeAttribute('disabled');
                                                document.getElementById('MainContent_cbxAfirmarTexto').parentNode.classList.remove("aspNetDisabled");

                                                calculoVisual();
                                                var strUrl = "../Accesorios/VisorPDF.aspx";
                                                window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                                                return false;

                                            } else {
                                                if (objVistaPrevia.vMensaje == "") {
                                                    showdialog('a', 'Registro Notarial : Cuerpo', 'PROBLEMAS PARA REALIZAR LA VISTA PREVIA COORDINE CON SU ADMINISTRADOR.', false, 160, 300);
                                                    return false;
                                                }
                                                else {
                                                    showdialog('a', 'Registro Notarial : Cuerpo.', objVistaPrevia.vMensaje, false, 160, 300);
                                                    return false;
                                                }
                                            }

                                        },
                                        failure: function (response) {
                                            showdialog('e', 'Registro Notarial : Cuerpo', response.d, false, 160, 300);
                                            return false;
                                        }
                                    });
                                    //-------------fin vista pdf
                                    return true;
                                }
                            }
                            else {
                                if (oResponse.Identity == 0 && oResponse.Message != null) {

                                    showdialog('a', 'Registro Notarial : Cuerpo', oResponse.Message.toString(), false, 160, 300);
                                }
                                return false;
                            }
                        },
                        error: errores

                    });
                }
                else {
                    showdialog('a', 'Registro Notarial : Cuerpo', 'Debe Grabarse el Acto Notarial Previamente.', false, 160, 300);
                    return false;
                }

            }
            else {
                showdialog('a', 'Registro Notarial : Cuerpo', 'Falta completar los datos.', false, 160, 300);
                return false;
            }


           


            return false;
        }


        function tabImprimirVistaPrevia(vVistaPrevia) {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            
            var str_hdn_acno_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
            var str_tipo_formato = $("#<%=hdn_tipo_formato_proto.ClientID %>").val();
            var str_correlativo = $("#<%=hdn_correlativo.ClientID %>").val();
            var str_num_oficio = "";
            if ($("#<%=txtNumeroOficioAdi.ClientID %>").val() != null) {
                str_num_oficio = $("#<%=txtNumeroOficioAdi.ClientID %>").val();
                if (str_num_oficio == "") {
                    showdialog('a', 'Registro Notarial : Formato', 'Falta ingresar Nro. de Oficio.', false, 160, 300);
                    return false;
                }
            }
            var str_hdn_actonotarialdetalle = 0;
        //    if ($("#<%=txtNumeroOficioAdi.ClientID %>").is(':enabled')) {
                str_hdn_actonotarialdetalle = $("#<%=hdn_actonotarialdetalle_id.ClientID %>").val();
          

            prm = {};
            prm.AcnoNotarialId = str_hdn_acno_iActoNotarialId;
            prm.TipoFormato = str_tipo_formato;
            prm.Correlativo = str_correlativo;
            prm.NumOficio = str_num_oficio;
            prm.ActoNotarialDetalleId = str_hdn_actonotarialdetalle;
            prm.vVistaPrevia = vVistaPrevia;
            $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/ImprimirVistaPrevia",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(prm),
                    dataType: "json",
                    success: function (data) {
                    var objVistaPrevia = $.parseJSON(data.d);
                        if (objVistaPrevia.bResultado == true) {
                            $("#<%= HF_NUMERO_PAGINA_DOCUMENTO.ClientID %>").val(objVistaPrevia.NumeroPagina);
                            $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", false);
                            $("#<%= cbxAfirmarTexto.ClientID %>").prop('disabled', false);
                            $("#<%= cbxAfirmarTexto.ClientID %>").prop("checked", false);
                            $("#<%= cbxAfirmarTexto.ClientID %>").removeAttr("checked")
                            document.getElementById('MainContent_cbxAfirmarTexto').parentNode.removeAttribute('disabled');
                            document.getElementById('MainContent_cbxAfirmarTexto').parentNode.classList.remove("aspNetDisabled");

                            calculoVisual();
                            var strUrl = "../Accesorios/VisorPDF.aspx";
                            window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                            return false;
                        } else {
                            if (objVistaPrevia.vMensaje == "") {
                                showdialog('a', 'Registro Notarial : Cuerpo', 'PROBLEMAS PARA REALIZAR LA VISTA PREVIA COORDINE CON SU ADMINISTRADOR.', false, 160, 300);
                                return false;
                            }
                            else {
                                showdialog('a', 'Registro Notarial : Cuerpo.', objVistaPrevia.vMensaje, false, 160, 300);
                                return false;
                            }
                        }

                },
            failure: function (response) {
                showdialog('e', 'Registro Notarial : Cuerpo', response.d, false, 160, 300);
                return false;
            }
        });
        return true;

   }

        function tab_03_AprobarDocumento() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var str_hdn_acno_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
            prm = {};
            prm.AcnoNotariaId = str_hdn_acno_iActoNotarialId;

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/AprobarDocumento",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(prm),
                dataType: "json",
                success: function (data) {
                }
            });
        }

        function ActivarSeccionCuerpo(seccion) {
            $("#accordion").accordion({
                active: seccion
            });
        }

        function ValidarTabCuerpo() {
            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();
            
            var strNroEscritura = $.trim($("#<%= txtNroEscritura.ClientID %>").val());
            var strNroLibro = $.trim($("#<%= txtNroLibro.ClientID %>").val());
            var strNroFojaIni = $.trim($("#<%= txtNroFojaIni.ClientID %>").val());
            var strNroFojaFinal = $.trim($("#<%= txtNroFojaFinal.ClientID %>").val());

            $("#<%=ddl_TipoDocrepresentante.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").css("border", "solid #888888 1px");

            
            //----------------------------------------------------------------------
            //Fecha: 14/10/2021
            //No validar cuanto el tipo de acto es Renuncia a la Nacionalidad            
            //----------------------------------------------------------------------
            if (TipoActoNotarialId != "8069") {
                
                var strOficinaRegistral = $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").val();
                if (strOficinaRegistral == 0) {

                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").focus();
                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").css("border", "solid Red 1px");
                    showdialog('a', 'Registro Notarial : Cuerpo', 'No se ha seleccionado la oficina registral.', false, 160, 300);
                    return;
                }
                else {
                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").css("border", "solid #888888 1px");
                }

//                var strDocTipoRepresen = $("#<%= ddl_TipoDocrepresentante.ClientID %>").val();
//                if (strDocTipoRepresen == 0) {
//                    $("#<%= ddl_TipoDocrepresentante.ClientID %>").focus();
//                    $("#<%= ddl_TipoDocrepresentante.ClientID %>").css("border", "solid Red 1px");
//                    showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado Tipo de Documento del Presentante', false, 160, 300);
//                    return;
//                }
//                else {
//                    $("#<%=ddl_TipoDocrepresentante.ClientID %>").css("border", "solid #888888 1px");
//                }
//                

//                var strDocNumeroRepresen = $.trim($("#<%= txtRepresentanteNroDoc.ClientID %>").val());
//                if (strDocNumeroRepresen.length == 0) {
//                    $("#<%= txtRepresentanteNroDoc.ClientID %>").focus();
//                    $("#<%= txtRepresentanteNroDoc.ClientID %>").css("border", "solid Red 1px");
//                    showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado Número de Documento del Presentante', false, 160, 300);
//                    return;
//                }
//                else {
//                    if (strDocNumeroRepresen.length != 8 && strDocTipoRepresen == 1) {
//                        $("#<%= txtRepresentanteNroDoc.ClientID %>").focus();
//                        $("#<%= txtRepresentanteNroDoc.ClientID %>").css("border", "solid Red 1px");
//                        showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado Número de Documento válido.', false, 160, 300);
//                        return;
//                    }
//                    else {
//                        $("#<%=txtRepresentanteNroDoc.ClientID %>").css("border", "solid #888888 1px");
//                    }
//                }

//                var strNombresRepresen = $.trim($("#<%= txtRepresentanteNombres.ClientID %>").val());
//                if (strNombresRepresen.length == 0) {
//                    $("#<%= txtRepresentanteNombres.ClientID %>").focus();
//                    $("#<%= txtRepresentanteNombres.ClientID %>").css("border", "solid Red 1px");
//                    showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado Nombres y Apellidos del Presentante', false, 160, 300);
//                    return;
//                }
//                else {
//                    $("#<%=txtRepresentanteNombres.ClientID %>").css("border", "solid #888888 1px");
//                }

//                var strGeneroRepresen = $("#<%= ddl_GerenoPresentante.ClientID %>").val();
//                if (strGeneroRepresen == 0) {
//                    $("#<%= ddl_GerenoPresentante.ClientID %>").focus();
//                    $("#<%= ddl_GerenoPresentante.ClientID %>").css("border", "solid Red 1px");
//                    showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado Género del Presentante', false, 160, 300);
//                    return;
//                }
//                else {
//                    $("#<%=ddl_GerenoPresentante.ClientID %>").css("border", "solid #888888 1px");
//                }

            }
           // var strTextoIntroduccion = $("#<%= lblIntro.ClientID %>").html();

            var strTextNormativo = $("#<%= txtTextoNormativo.ClientID %>").val();

            var strTextoCentral = tinyMCE.editors[0].getContent();

          //  var strTextoConclusion = $("#<%= lblConcluciones.ClientID %>").html();

            var chk_Activo = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');

            var str_acno_Numero_Minuta = $.trim($("#<%= txt_acno_Numero_Minuta.ClientID %>").val());
            var str_acno_vNombreColegiatura = $.trim($("#<%= txt_acno_vNombreColegiatura.ClientID %>").val());

            var strAutorizacionTipoId = $.trim($("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val());
            var strNumeroColegiatura = $.trim($("#<%= txt_acno_vNumeroColegiatura.ClientID %>").val());
            var strFirmaIlegible = $.trim($("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val());


            if (strNroEscritura.length == 0) {
                $("#<%= txtNroEscritura.ClientID %>").focus();
                $("#<%= txtNroEscritura.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de escritura', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroEscritura.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNroLibro.length == 0) {
                $("#<%= txtNroLibro.ClientID %>").focus();
                $("#<%= txtNroLibro.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de libro', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroLibro.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNroFojaIni.length == 0) {
                $("#<%= txtNroFojaIni.ClientID %>").focus();
                $("#<%= txtNroFojaIni.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de foja inicial', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroFojaIni.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNroFojaFinal.length == 0) {
                $("#<%= txtNroFojaFinal.ClientID %>").focus();
                $("#<%= txtNroFojaFinal.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de foja final', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroFojaFinal.ClientID %>").css("border", "solid #888888 1px");
            }

//            if (strTextoIntroduccion.length == 0) {
//                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha generado la introducción de la escritura', false, 160, 300);
//                $("#accordion").accordion({
//                    active: 0
//                });
//                return;
//            }

           

            if (strTextoCentral.length == 0) {
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el texto central de la escritura', false, 160, 300);
                $("#accordion").accordion({
                    active: 1
                });
                return;
            }

            var bExistePresentante = false;
            bExistePresentante = CountRowsGridViewPresentante();
            if (bExistePresentante == false)
            {
                showdialog('a', 'Registro Notarial : Cuerpo', 'Debe adicionar un presentante.', false, 160, 300);
                return;
            }

            //            return confirm("¿Está seguro de grabar los cambios?");
            return true;
        }

        function funHabilitar_Tab03(estado) {
            $("#<%=RichTextBox.ClientID %>").attr('disabled', estado);

            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', estado);
            $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', estado);
            $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', estado);
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', estado);

            $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', estado);
            $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', estado);

            //$("#btnSave_tab3").attr('disabled', estado);
        }

        function LimpiarTabCuerpo() {
            $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop('disabled', true);
            $("#<%=txtNroEscritura.ClientID %>").val('MRE000');
            $("#<%=txtNroLibro.ClientID %>").val('LIBR000');
            $("#<%=txtNroFojaIni.ClientID %>").val('0');
            $("#<%=txtNroFojaFinal.ClientID %>").val('0');

            //$("#<%=ddl_OficinaRegistralRegistrador.ClientID %>").val('0');

            $("#<%=txtRegistradorNombres.ClientID %>").val('');
            $("#<%=txtRepresentanteNombres.ClientID %>").val('');
            $("#<%=txtRepresentanteNroDoc.ClientID %>").val('');

            $("#<%=txtCostoEP.ClientID %>").val('0');
            $("#<%=txtCostoParte2.ClientID %>").val('0');
            $("#<%=txtCostoTestimonio.ClientID %>").val('0');

            $("#<%=txt_acno_Numero_Minuta.ClientID %>").val('');
            $("#<%=txt_acno_vNombreColegiatura.ClientID %>").val('');
            $("#<%=txtNroOficio.ClientID %>").val('');
            $("#<%=ddl_GerenoPresentante.ClientID %>").val('0');
            $("#<%=ddl_Apoderado.ClientID %>").val('0');            
            $("#<%=ddl_TipoDocrepresentante.ClientID %>").val('0');

            $("#<%= lblIntro.ClientID %>").html('');
            $("#<%=txtTextoNormativo.ClientID %>").val('');
            $("#<%=txtTextoNormativo_.ClientID %>").val('');
            tinyMCE.editors[0].setContent('');
            $("#<%= lblConcluciones.ClientID %>").html('');

            tinyMCE.editors[1].setContent('');
            tinyMCE.editors[2].setContent('');

            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").val('');
            $("#<%=txtAutorizacionNroDocumento.ClientID %>").val('');
            $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").val('');
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").val('');

            $("#<%=txtNroEscritura.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtNroLibro.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtNroFojaIni.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtNroFojaFinal.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txtAutorizacionNroDocumento.ClientID %>").css("border", "solid #888888 1px");

            $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").css("border", "solid #888888 1px");
            $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").css("border", "solid #888888 1px");

            $("#<%= lblValidacionCuerpo.ClientID %>").hide();

            //$("#btnSave_tab3").prop("disabled", "disabled");
            $("#<%= Btn_VistaPreviaAprobar.ClientID %>").prop('disabled', true);
            $("#<%= cbxAfirmarTexto.ClientID %>").prop('checked', false);


            $('#<%= divListNormativo.ClientID %>').children('input').each(function (e, c) {
                $(this).attr("checked", false);
            });

            $("#accordion").accordion({
                active: 0
            });

            $("#<%= txtNroEscritura.ClientID %>").focus();
        }

        function generar_cuerpo_escritura() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;
            //$("#btnSave_tab3").prop('disabled', true);
            if (ValidarGeneracionCuerpo()) {
                //  $("#<%=Btn_VistaPreviaAprobar.ClientID %>").prop("disabled", "disabled");
                $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", true);
                $("#<%=cbxAfirmarTexto.ClientID %>").prop("disabled", true); //--
                document.getElementById('MainContent_cbxAfirmarTexto').parentElement.disabled = true;
                $("#<%=cbxAfirmarTexto.ClientID %>").prop('checked', false);

                llenar_Ciudad_Fecha();
                llenar_introduccion_escritura();
                llenar_conclusion_escritura();
                llenar_final_escritura();
                
                $("#accordion").accordion({
                    active: 0
                });
                //$("#btnSave_tab3").prop('disabled', false);
                 showdialog('i', 'Registro Notarial : Cuerpo', 'El Documento ha sido generado correctamente.', false, 160, 300);
                $("#<%=Btn_VistaPreviaAprobar.ClientID %>").removeAttr('disabled');
            }
        }

        function llenar_introduccion_escritura() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var cuerpo = {};

            cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
            cuerpo.acno_IFuncionarioAutorizadorId = $("#<%= ddlFuncionario.ClientID %> option:selected").text().toUpperCase();
            cuerpo.acno_vNumeroEscrituraPublica = $("#<%= txtNroEscritura.ClientID %>").val().toUpperCase();

            cuerpo.acno_vAutorizacionTipo = $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val().toUpperCase();
            cuerpo.acno_vAutorizacionDocumentoNumero = $("#<%= txtAutorizacionNroDocumento.ClientID %>").val().toUpperCase();
            cuerpo.acno_vNumeroColegiatura = $("#<%= txt_acno_vNumeroColegiatura.ClientID %>").val().toUpperCase();
            cuerpo.ancu_vFirmaIlegible = $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val().toUpperCase();
            cuerpo.acno_iNumeroActoNotarial = $.trim($("#<%= txt_acno_Numero_Minuta.ClientID %>").val());
            cuerpo.acno_dFechaConclusionFirma = $("#<%= hdn_acno_dFechaConclusionFirma.ClientID %>").val();
            cuerpo.textoActoNotarial = $("#<%= Cmb_TipoActoNotarial.ClientID %> option:selected").text().toUpperCase();
            cuerpo.acno_vNombreColegiatura = $("#<%= txt_acno_vNombreColegiatura.ClientID %>").val().toUpperCase(); 

            var prm = {};
            prm.cuerpo = JSON.stringify(cuerpo);

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/llenar_introduccion_escritura",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var lACTONOTARIAL = rspt.d;
                    $("#<%= lblIntro.ClientID %>").html(lACTONOTARIAL);
                    $("#<%= hdn_TxtIntro.ClientID %>").val(lACTONOTARIAL);
                },
                error: errores
            });
            
            
        }

        function llenar_Ciudad_Fecha() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var cuerpo = {};

            cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
            var prm = {};
            prm.cuerpo = JSON.stringify(cuerpo);

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/ObtenerCiudadFecha",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var strCiudadFecha = rspt.d;
                    $("#<%= HF_CIUDAD_FECHA.ClientID %>").val(strCiudadFecha);

                },
                error: errores
            });
        }

        function llenar_conclusion_escritura() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var cuerpo = {};

            cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
            cuerpo.acno_IFuncionarioAutorizadorId = $("#<%= ddlFuncionario.ClientID %> option:selected").text().toUpperCase();
            cuerpo.acno_vNumeroEscrituraPublica = $("#<%= txtNroEscritura.ClientID %>").val().toUpperCase();
            cuerpo.acno_vNumeroFojaInicial = $("#<%= txtNroFojaIni.ClientID %>").val().toUpperCase();
            cuerpo.acno_vNumeroFojaFinal = $("#<%= txtNroFojaFinal.ClientID %>").val().toUpperCase();
            cuerpo.textoActoNotarial = $("#<%= Cmb_TipoActoNotarial.ClientID %> option:selected").text().toUpperCase();

            var strDL1049Articulo55C = tinyMCE.editors[1].getContent();

            var DL1049Articulo55C = "";

            $(strDL1049Articulo55C + ' > p').each(function (idx, el) {
                if ($(el).text() != "" && $.trim($(el).text()) != ">") {
                    //                        textoCentral += "<p style='text-align:justify;'>";
                    //                        textoCentral += "PRIMERO: " + $(el).text().replace("PRIMERO:", "").trim().toUpperCase();
                    //                        textoCentral += "</p>";
                    if ($(el).text().trim().length > 0) {
                        DL1049Articulo55C += $(el).text().trim().toUpperCase();
                    }
                }
            });

            cuerpo.ancu_vDL1049Articulo55C = DL1049Articulo55C;

            var prm = {};
            prm.cuerpo = JSON.stringify(cuerpo);

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/llenar_conclusion_escritura",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var lACTONOTARIAL = rspt.d;
                    $("#<%= lblConcluciones.ClientID %>").html(lACTONOTARIAL);
                    $("#<%= hdn_TxtConclu.ClientID %>").val(lACTONOTARIAL);
                },
                error: errores
            });
        }


        function llenar_final_escritura() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var cuerpo = {};

            cuerpo.ancu_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();
            cuerpo.textoActoNotarial = $("#<%= Cmb_TipoActoNotarial.ClientID %> option:selected").text().toUpperCase();

            var prm = {};
            prm.cuerpo = JSON.stringify(cuerpo);

            $.ajax({
                type: "POST",
                url: "FrmActoNotarialProtocolares.aspx/llenar_final_escritura",
                data: JSON.stringify(prm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rspt) {
                    var lACTONOTARIAL = rspt.d;
                    $("#<%= lblFinal.ClientID %>").html(lACTONOTARIAL);
                    $("#<%= Hdn_TxtFinal.ClientID %>").val(lACTONOTARIAL);
                },
                error: errores
            });
        }


        function ValidarGeneracionCuerpo() {
            var bolValida = false;
            var strNroEscritura = $.trim($("#<%= txtNroEscritura.ClientID %>").val());
            var strNroLibro = $.trim($("#<%= txtNroLibro.ClientID %>").val());
            var strNroFojaIni = $.trim($("#<%= txtNroFojaIni.ClientID %>").val());
            var strNroFojaFinal = $.trim($("#<%= txtNroFojaFinal.ClientID %>").val());

            var strAutorizacionTipo = $.trim($("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").val());
            var strNumeroColegiatura = $.trim($("#<%= txt_acno_vNumeroColegiatura.ClientID %>").val());
            var strFirmaIlegible = $.trim($("#<%= txt_ancu_vFirmaIlegible.ClientID %>").val());

            var strNumero_Minuta = $.trim($("#<%= txt_acno_Numero_Minuta.ClientID %>").val());
            var bMinuta = $("#<%= chk_acno_bFlagMinuta.ClientID %>");
            var domCheckbox1 = bMinuta[0];
            var isChecked = domCheckbox1.checked;
            var TipoActoNotarialId = $("#<%= Cmb_TipoActoNotarial.ClientID %>").val();

            if (strNroEscritura.length == 0) {
                $("#<%= txtNroEscritura.ClientID %>").focus();
                $("#<%= txtNroEscritura.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de escritura', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroEscritura.ClientID %>").css("border", "solid #888888 1px");
                bolValida = true;
            }

            if (strNroLibro.length == 0) {
                $("#<%= txtNroLibro.ClientID %>").focus();
                $("#<%= txtNroLibro.ClientID %>").css("border", "solid Red 1px");
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el número de libro', false, 160, 300);
                return;
            }
            else {
                $("#<%=txtNroLibro.ClientID %>").css("border", "solid #888888 1px");
                bolValida = true;
            }
            //---------------------------------------------------

            //----------------------------------------------------------------------
            //Fecha: 14/10/2021
            //No validar cuanto el tipo de acto es Renuncia a la Nacionalidad            
            //----------------------------------------------------------------------
            if (TipoActoNotarialId != "8069") {

                var strOficinaRegistral = $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").val();
                if (strOficinaRegistral == 0) {

                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").focus();
                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").css("border", "solid Red 1px");
                    showdialog('a', 'Registro Notarial : Cuerpo', 'No se ha seleccionado la oficina registral.', false, 160, 300);
                    return;
                }
                else {
                    $("#<%= ddl_OficinaRegistralRegistrador.ClientID %>").css("border", "solid #888888 1px");
                    bolValida = true;
                }
              
            }
            var bExistePresentante = false;
            bExistePresentante = CountRowsGridViewPresentante();
            if (bExistePresentante == false)
            {
                showdialog('a', 'Registro Notarial : Cuerpo', 'Debe adicionar un presentante.', false, 160, 300);
                return;
            }
            //-------------------------------------
            //Fecha: 19/09/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Los siguientes campos son informativos:
            //          Nro.Minuta, Autorizado, Nombre del Colegio de de Abogado, Nro.Colegiatura y Número de firmas.
            //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
            //-------------------------------------

            //            if (isChecked) {

            //                if (strNumero_Minuta.length == 0) {
            //                    $("#<%= txt_acno_Numero_Minuta.ClientID %>").css("border", "solid Red 1px");
            //                    bolValida = false;
            //                }
            //                else {
            //                    $("#<%=txt_acno_Numero_Minuta.ClientID %>").css("border", "solid #888888 1px");
            //                }

            //                if (strAutorizacionTipo.length == 0) {
            //                    $("#<%= txt_acno_sAutorizacionTipoId.ClientID %>").css("border", "solid Red 1px");
            //                    bolValida = false;
            //                }
            //                else {
            //                    $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").css("border", "solid #888888 1px");
            //                }

            //                if (strNumeroColegiatura.length == 0) {
            //                    $("#<%= txt_acno_vNumeroColegiatura.ClientID %>").css("border", "solid Red 1px");
            //                    bolValida = false;
            //                }
            //                else {
            //                    $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").css("border", "solid #888888 1px");
            //                }

            //                if (strFirmaIlegible.length == 0) {
            //                    $("#<%= txt_ancu_vFirmaIlegible.ClientID %>").css("border", "solid Red 1px");
            //                    bolValida = false;
            //                }
            //                else {
            //                    $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").css("border", "solid #888888 1px");
            //                }
            //            }

            var strTextoCentral = tinyMCE.editors[0].getContent();
            if (strTextoCentral.length == 0) {
                showdialog('a', 'Registro Notarial : Cuerpo', 'No ha ingresado el texto central de la escritura', false, 160, 300);
                $("#accordion").accordion({
                    active: 1
                });
                return;
            }

            if (bolValida) {
                $("#<%= lblValidacionCuerpo.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionCuerpo.ClientID %>").show();
                return;
            }

            return true;
        }

        function CountRowsGridViewPresentante() {
            var bolValida = false;
            var totalRowCount = 0;
            var rowCount = 0;
            var gridView = document.getElementById("<%=GridViewPresentante.ClientID %>");
            var rows = gridView.getElementsByTagName("tr")
            for (var i = 0; i < rows.length; i++) {
                totalRowCount++;
                if (rows[i].getElementsByTagName("td").length > 0) {
                    rowCount++;
                }
            }
           // var message = "Total Row Count: " + totalRowCount;
           // message += "\nRow Count: " + rowCount;
           // alert(message);
            if (rowCount > 0)
            {
                bolValida = true;
            }
            return bolValida;
        }


        function Rellenar_introduccion_conclusion_escritura() {
            var strIntroduccion = $("#<%= hdn_TxtIntro.ClientID %>").val();
            $("#<%= lblIntro.ClientID %>").html(strIntroduccion);
            var strConclusion = $("#<%= hdn_TxtConclu.ClientID %>").val();
            $("#<%= lblConcluciones.ClientID %>").html(strConclusion);
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

        function CheckPago(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var ACTDET_ID = row.cells[0].innerHTML;

            if (ACTDET_ID != "0") {
                showdialog('a', 'Registro Notarial: Aprobación y Pago', 'No se puede eliminar cuando ya fue registrado el pago.', false, 160, 300);
                return false;
            }
            else {
                return true;
            }
        }

        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var ACTDET_ID = row.cells[0].innerHTML;

            if ($("#<%=ddlTipoPago.ClientID %>").val() == "0") {
                showdialog('a', 'Registro Notarial: Aprobación y Pago', 'Previamente el registro el pago.', false, 160, 300);
            }
            else {

                funSetSession("intActuacionDetalleIndice", rowIndex.toString());
                funSetSession("ACTDET_ID", ACTDET_ID);
                $("#tabs").tabs("option", "active", 5);
            }

            return false;
        }

        function ValidarPago() {
            var bolValida = true;


            var strVoucherNro = $.trim($("#<%= Txt_VoucherNro.ClientID %>").val());
            var isDisabledVoucher = $("#<%= Txt_VoucherNro.ClientID %>").prop('disabled');

            var strTipoPago = $.trim($("#<%= ddlTipoPago.ClientID %>").val());

            var txtVoucherNro = document.getElementById('<%= Txt_VoucherNro.ClientID %>');
            var ddlTipoPago = document.getElementById('<%= ddlTipoPago.ClientID %>');

            if (!isDisabledVoucher && strVoucherNro == "") {
                txtVoucherNro.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtVoucherNro.style.border = "1px solid #888888";
            }

            if (strTipoPago == "0") {
                ddlTipoPago.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlTipoPago.style.border = "1px solid #888888";
            }

            var totalRows = 0;
            $('#<%=Gdv_Tarifa.ClientID%> tr:not(:last)').each(function () {
                totalRows = totalRows + 1;
            });

            if (totalRows > 0) {

            }
            else {
                bolValida = false;
                alert('No ha ingresado datos');
            }


            if (bolValida) {
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                bolValida = false;
            }
            return bolValida;
        }

        function tab_06() {
            document.getElementById("<%= btnLoadArchivoDigitalizado.ClientID %>").click();
        }

        function AddDocumentoDigitalizado() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

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

                var prm = {};
                prm.archivo = '[' + JSON.stringify(archivo) + ']';
                var rsp = execute('FrmActoNotarialProtocolares.aspx/adicionar_archivo', prm);

                tab_06();
            }
        }

        function SaveDocumentoDigitalizado() {

            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return;

            var totalRows = 0;
            $('#<%=Gdv_Adjunto.ClientID%> tr:not(:last)').each(function () {
                totalRows = totalRows + 1;
            });

            if (totalRows > 0) {

                if (confirm("¿Está seguro de grabar los cambios?")) {

                    var larchivoDigitalizado = {};

                    larchivoDigitalizado.ando_iActoNotarialDocumentoId = $("#<%= hdn_ando_iActoNotarialDocumentoId.ClientID %>").val();
                    larchivoDigitalizado.ando_iActoNotarialId = $("#<%= hdn_acno_iActoNotarialId.ClientID %>").val();

                    var prm = {};
                    prm.larchivoDigitalizado = JSON.stringify(larchivoDigitalizado);
                    var rsp = execute('FrmActoNotarialProtocolares.aspx/insertar_archivo', prm);
                    showdialog('i', 'Registro Notarial : Archivo Digitalizado(s)', 'El registro ha sido grabado correctamente', false, 160, 300);
                }
            }
            else {
                alert('No ha ingresado datos');
            }
        }

        function ValidarTabArchivoDigitalizado() {

            var bolValida = true;

            var strDescripcion = $.trim($("#<%= Txt_AdjuntoDescripcion.ClientID %>").val());
            var strNomAdjFile2 = $.trim($("#<%= hidNomAdjFile2.ClientID %>").val());

            if (strDescripcion.length == 0) {
                $("#<%=Txt_AdjuntoDescripcion.ClientID %>").focus();
                $("#<%=Txt_AdjuntoDescripcion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=Txt_AdjuntoDescripcion.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNomAdjFile2.length == 0) {
                bolValida = false;
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

        function LimpiarTabArchivoDigitalizado() {
            $("#<%=Txt_AdjuntoDescripcion.ClientID %>").val('');
            $("#<%=hidNomAdjFile2.ClientID %>").val('');

            $("#<%=Txt_AdjuntoDescripcion.ClientID %>").css("border", "solid #888888 1px");

            $("#<%= lblValidacionArchivoDigital.ClientID %>").hide();
        }
             

    </script>

    <script type="text/javascript" language="javascript">
        function AgregarImagenes() {
            var rpta = isSessionTimeOut();
            if (rpta != 'ok') return false;

            if (ValidarImagenAdjunta()) {
                var imagen = {};

                imagen.ando_iActoNotarialDocumentoId = $("#<%=hdn_ando_iActoNotarialDocumentoId.ClientID %>").val();
                imagen.ando_iActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
                imagen.ando_sTipoDocumentoId = 4202;
                imagen.ando_sTipoInformacionId = 0;
                imagen.ando_sSubTipoInformacionId = 0;
                imagen.ando_vDescripcion = $("#<%=txtImagenTitulo.ClientID %>").val().toUpperCase();
                imagen.ando_vDetalleDocumento = "";
                imagen.ando_vRutaArchivo = $("#<%=hdn_imagen_nombre.ClientID %>").val();

                var prm = {};
                prm.imagen = '[' + JSON.stringify(imagen) + ']';
                var rsp = execute('FrmActoNotarialProtocolares.aspx/argregar_imagen', prm);

                CargarImagenes();
                return false;
            } else {
                return false;
            }
        }

        function CargarImagenes() {
            document.getElementById("<%= btnCargarImagenes.ClientID %>").click();
        }

        function ValidarImagenAdjunta() {
            var bolValida = true;

            var strImagenTitulo = $.trim($("#<%= txtImagenTitulo.ClientID %>").val());
            var strImagenNombre = $.trim($("#<%= hdn_imagen_nombre.ClientID %>").val());

            if (strImagenTitulo.length == 0) {
                $("#<%=txtImagenTitulo.ClientID %>").focus();
                $("#<%=txtImagenTitulo.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtImagenTitulo.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strImagenNombre.length == 0) {
                bolValida = false;
            }

            if (bolValida) {
                $("#<%= lblValidacionImagen.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionImagen.ClientID %>").show();
                bolValida = false;
            }

            return bolValida;
        }
    </script>


     
    <script typ="text/javascript">

        function UploadImg() {
            var ext = $('#<%=FileUploadImagen.ClientID %>').val().split('.').pop().toLowerCase();
            var input = document.getElementById('<%= FileUploadImagen.ClientID %>');
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                alert('El archivo es inválido');
                $('#<%=FileUploadImagen.ClientID %>').val('');
                $("#<%= hdn_imagen_nombre.ClientID %>").val('');
            } else {
                readURL(input, '', '', ext);
            }
        }



        function readURL(input, srt_Carpeta, str_Name, str_ext) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {

                    $.ajax({
                        type: "POST",
                        url: "Services/Services.asmx/GetCapturedImage",
                        data: '{str_Base64: ' + JSON.stringify(e.target.result) + ', str_Carpeta: ' + JSON.stringify(srt_Carpeta) + ', str_Name: ' + JSON.stringify(str_Name) + ', str_ext: ' + JSON.stringify(str_ext) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            $("#<%= hdn_imagen_nombre.ClientID %>").val(data.d);
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                }
                reader.readAsDataURL(input.files[0]);
            }
        }


        function pageLoad() {
            var ajaxInProgress = false; //add this to stop recursive calls when errors occur
            $("#<%=ddl_anpa_sTipoParticipanteId.ClientID %>").change(function () {


                var tipoParticId = $('#<%=ddl_anpa_sTipoParticipanteId.ClientID %>').val();
                var str_Participante_Otorgante = $("#<%= HF_OTORGANTE.ClientID %>").val();
                var str_Participante_Vendedor = $("#<%= HF_VENDEDOR.ClientID %>").val();
                var str_Participante_Anticipante = $("#<%= HF_ANTICIPANTE.ClientID %>").val();


                var str_Participante_Testigo_a_Ruego = $("#<%= HF_TESTIGO_A_RUEGO.ClientID %>").val();
                var str_Participante_Interprete = $("#<%= HF_INTERPRETE.ClientID %>").val();

                if ($('#<%=txtRegistroTipoIncapacidad.ClientID %>').val() == "") {
                    $('#<%=chkIncapacitado.ClientID %>').prop("checked", "");
                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').val("");
                }


                $('#<%=lblParticipanteDiscapacidad.ClientID %>').hide();
                $('#<%=ddl_Participante_Discapacidad.ClientID %>').hide();

                OcularListaInterpretes();

                var TipoParticipanteId = $("#<%= ddl_anpa_sTipoParticipanteId.ClientID %>").val();

                if (tipoParticId == str_Participante_Otorgante || tipoParticId == str_Participante_Vendedor || tipoParticId == str_Participante_Anticipante) {

                    $('#<%=chkIncapacitado.ClientID %>').show();
                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').show();
                    $('#<%=lblIncapacitadoTitulo.ClientID %>').show();
                    $('#<%=lblIncapacidadTitulo.ClientID %>').show();

                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').prop('disabled', true);
                }
                else {
                    $('#<%=chkIncapacitado.ClientID %>').hide();
                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').hide();
                    $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
                    $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
                    $('#<%=lblIncapacitadoTitulo.ClientID %>').hide();
                    $('#<%=lblIncapacidadTitulo.ClientID %>').hide();


                }
                if (TipoParticipanteId == "8414") {
                    $("#<%=lblParticipanteDiscapacidad.ClientID %>").show();
                    $("#<%=ddl_Participante_Discapacidad.ClientID %>").show();

                    if (ajaxInProgress) return;
                }
                else {
                    $("#<%=lblParticipanteDiscapacidad.ClientID %>").hide();
                    $("#<%=ddl_Participante_Discapacidad.ClientID %>").hide();
                }


                var obj = $("#<%= ddl_Participante_Discapacidad.ClientID %>");
                obj.empty();
                obj.append($('<option></option>').val('0').html('- SELECCIONAR -'));
                //-------------------------------------------------------------------------
                if (TipoParticipanteId == "8413") {
                    if ($("#<%=ddl_Participante_Interprete.ClientID %>").length > 1) {
                        MostrarlistaInterpretes();
                        if (ajaxInProgress) return;
                    }
                    else {
                        OcularListaInterpretes();
                    }
                }
                else {
                    OcularListaInterpretes();
                }


                var objparticipanteinterprete = $("#<%= ddl_Participante_Interprete.ClientID %>");
                objparticipanteinterprete.empty();
                objparticipanteinterprete.append($('<option></option>').val('0').html('- SELECCIONAR -'));


                if (tipoParticId == str_Participante_Otorgante || tipoParticId == str_Participante_Vendedor || tipoParticId == str_Participante_Anticipante) {

                    $('#<%=chkIncapacitado.ClientID %>').show();
                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').show();
                    $('#<%=lblIncapacitadoTitulo.ClientID %>').show();
                    $('#<%=lblIncapacidadTitulo.ClientID %>').show();

                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').prop('disabled', true);
                    $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
                    $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');
                    OcularListaInterpretes();
                }
                else {

                    $('#<%=chkIncapacitado.ClientID %>').hide();
                    $('#<%=txtRegistroTipoIncapacidad.ClientID %>').hide();

                    $('#<%=lblIncapacitadoTitulo.ClientID %>').hide();
                    $('#<%=lblIncapacidadTitulo.ClientID %>').hide();

                    $("#<%=lblHuella.ClientID %>").css('visibility', 'hidden');
                    $("#<%=chkNoHuella.ClientID %>").css('visibility', 'hidden');

                    if (tipoParticId == str_Participante_Testigo_a_Ruego) {
                        $('#<%=lblParticipanteDiscapacidad.ClientID %>').show();
                        $('#<%=ddl_Participante_Discapacidad.ClientID %>').show();

                        $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {

                            var campo1, campo2, codigoparticipante, aux, aux1;
                            var codigoparticipanteaux = false;
                            $(this).children("td").each(function (index2) {


                                if (index2 == 0) { aux1 = $(this).text(); }
                                if (index2 == 4) { campo2 = $(this).text(); }
                                if (index2 == 6) { codigoparticipante = $(this).text(); }
                                if (index2 == 9) { campo1 = $(this).text(); }
                                if (index2 == 11) { codigoparticipanteaux = $(this).text().toLowerCase(); }
                                if (index2 == 12) { aux = $(this).text(); }


                                if (codigoparticipanteaux == 'true') {
                                    ajaxInProgress = true;

                                    if (aux1 == 'OTORGANTE') {
                                        if (aux != null) {
                                            obj.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                    if (aux1 == 'VENDEDOR') {
                                        if (aux != null) {
                                            obj.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                    if (aux1 == 'ANTICIPANTE') {
                                        if (aux != null) {
                                            obj.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                }

                            });

                        });
                    }
                    //-------------------------------------------------
                    var nro_interprete = 0;
                    if (tipoParticId == str_Participante_Interprete) {

                        $("#<%= grd_Participantes.ClientID %> tbody tr").each(function (index) {

                            var campo1, campo2, codigoparticipante, aux, aux1, idioma;
                            var codigoparticipanteaux = false;
                            $(this).children("td").each(function (index2) {


                                if (index2 == 0) { aux1 = $(this).text(); }
                                if (index2 == 4) { campo2 = $(this).text(); }
                                if (index2 == 6) { codigoparticipante = $(this).text(); }
                                if (index2 == 7) { idioma = $(this).text().toUpperCase(); }
                                if (index2 == 9) { campo1 = $(this).text(); }
                                if (index2 == 11) { codigoparticipanteaux = $(this).text().toLowerCase(); }
                                if (index2 == 12) { aux = $(this).text(); }


                                if (idioma != 'CASTELLANO') {
                                    ajaxInProgress = true;

                                    if (aux1 == 'OTORGANTE') {
                                        if (aux != null) {
                                            nro_interprete++;
                                            objparticipanteinterprete.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                    if (aux1 == 'VENDEDOR') {
                                        if (aux != null) {
                                            nro_interprete++;
                                            objparticipanteinterprete.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                    if (aux1 == 'ANTICIPANTE') {
                                        if (aux != null) {
                                            nro_interprete++;
                                            objparticipanteinterprete.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }
                                    if (aux1 == 'DONANTE') {
                                        if (aux != null) {
                                            nro_interprete++;
                                            objparticipanteinterprete.append($('<option></option>').val(aux).html(campo2));
                                            return false;
                                        }
                                    }

                                }

                            });

                        });

                        if (nro_interprete > 1) {
                            MostrarlistaInterpretes();
                        }
                        else {
                            OcularListaInterpretes();
                        }
                    }
                    //-------------------------------------------------
                }

                $("#<%=hdn_acno_tipoParticipante.ClientID %>").val(tipoParticId);

            });
            ajaxInProgress = false;


            $("#<%= rbApoderado.ClientID %>").click(function () {

                $("#<%= ddl_TipoDocrepresentante.ClientID %>").attr('disabled', true);
                $("#imgBuscarPresentante").attr('disabled', true);
                $("#<%= txtRepresentanteNombres.ClientID %>").attr('disabled', true);
                $("#<%= txtRepresentanteNroDoc.ClientID %>").attr('disabled', true);
                $("#<%= ddl_GerenoPresentante.ClientID %>").attr('disabled', true);

                $("#<%= ddl_Apoderado.ClientID %>").attr('disabled', false);

                $("#<%= ddl_TipoDocrepresentante.ClientID %>").val('0');
                $("#<%= txtRepresentanteNombres.ClientID %>").val('')
                $("#<%= txtRepresentanteNroDoc.ClientID %>").val('')
                $("#<%= ddl_GerenoPresentante.ClientID %>").val('0')

            });


            $("#<%= rbOtros.ClientID %>").click(function () {

                $("#<%= ddl_TipoDocrepresentante.ClientID %>").attr('disabled', false);
                $("#imgBuscarPresentante").attr('disabled', false);
                $("#<%= txtRepresentanteNombres.ClientID %>").attr('disabled', false);
                $("#<%= txtRepresentanteNroDoc.ClientID %>").attr('disabled', false);
                $("#<%= ddl_GerenoPresentante.ClientID %>").attr('disabled', false);

                $("#<%= ddl_Apoderado.ClientID %>").attr('disabled', true);

                $("#<%= ddl_TipoDocrepresentante.ClientID %>").val('0');
                $("#<%= txtRepresentanteNombres.ClientID %>").val('')
                $("#<%= txtRepresentanteNroDoc.ClientID %>").val('')
                $("#<%= ddl_GerenoPresentante.ClientID %>").val('0')

                $("#<%= ddl_Apoderado.ClientID %>").val('0');


            });

            $("#<%= ddl_Apoderado.ClientID %>").change(function () {

                var prm = {};
                prm.parametros = $(this).val();

                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtenerDatosParticipante",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rspt) {

                        var objPerson = $.parseJSON(rspt.d);

                        $("#<%= ddl_TipoDocrepresentante.ClientID %>").val(objPerson.Identificacion.peid_sDocumentoTipoId);
                        $("#<%= txtRepresentanteNroDoc.ClientID %>").val(objPerson.Identificacion.peid_vDocumentoNumero);
                        $("#<%= txtRepresentanteNombres.ClientID %>").val(objPerson.pers_vNombres + " " + objPerson.pers_vApellidoPaterno + " " + objPerson.pers_vApellidoMaterno);
                        $("#<%= ddl_GerenoPresentante.ClientID %>").val(objPerson.pers_sGeneroId);

                    }

                });

            });

            $("#<%=ddlPaisOrigen.ClientID %>").change(function () {
                var prm = {};
                prm.strPaisId = $(this).val();
                $.ajax({
                    type: "POST",
                    url: "FrmActoNotarialProtocolares.aspx/obtenerNacionalidad",
                    data: JSON.stringify(prm),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (rspt) {
                        var objNacionalidad = $.parseJSON(rspt.d);
                        $("#<%= LblDescNacionalidadCopia.ClientID %>").text(objNacionalidad);

                    }
                });
            });
			//------------------------
			
      }
      function validarApellidoCasada() {

          var genero = $('#<%= ddl_pers_sGeneroId.ClientID %>').find('option:selected').text();
          var estadoCivil = $('#<%= ddl_pers_sEstadoCivilId.ClientID %>').find('option:selected').text();

          if (genero == "FEMENINO" && (estadoCivil == "CASADO" || estadoCivil == "CASADA" || estadoCivil == "VIUDO" || estadoCivil == "VIUDA")) {
              $('#<%= txt_pers_vApellidoCasada.ClientID %>').removeAttr("disabled");              
          }
          else {
              $('#<%= txt_pers_vApellidoCasada.ClientID %>').attr("disabled", true);
              document.getElementById('<%=txt_pers_vApellidoCasada.ClientID %>').value = "";
          }
      }
      function darclick() {

          var obj = document.getElementById('<%= btnAgregarParticipanteNew.ClientID %>');

          obj.click();

      }
      function checkMinuta() {
          var chk_Activo = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');

          if (!chk_Activo) {
              $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").val('');

              $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', true);
              $("#<%=txtAutorizacionNroDocumento.ClientID %>").val('');

              $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").val('');

              $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', true);
              $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").val('');

              $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_Numero_Minuta.ClientID %>").val('');

              $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_vNombreColegiatura.ClientID %>").val('');

          } else {
              $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', false);

              $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', false);

              $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', false);
              $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', false);

              $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', false);
              $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', false);
          }
      }
      function validarOnBlurCaracteresRUNE(texto) {
          var campo = texto.value;

          var validrune = $("#<%= HFValidarTextoRune.ClientID %>").val();
          var validchars = "abcdefghijklmnopqrstuvwxyzáéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ " + validrune;

          var parsed = true;
          for (var i = 0; i < campo.length; i++) {
              var letter = campo.charAt(i).toLowerCase();
              if (validchars.indexOf(letter) != -1)
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

      function habilitarTabCuerpo() {
          enableElements($('#tabs-3').children());  
        //  $("#btnTraerCuerpo").prop('disabled', false);
        //  $("#btnCncl_tab3").prop('disabled', false);

          $("#<%=Btn_VistaPreviaAprobar.ClientID %>").attr('disabled', true);

          $("#<%=txtNroEscritura.ClientID %>").attr('disabled', true);
          $("#<%=txtNroLibro.ClientID %>").attr('disabled', true);
          $("#<%=txtNroFojaIni.ClientID %>").attr('disabled', true);
          $("#<%=txtNroFojaFinal.ClientID %>").attr('disabled', true);
          $("#<%=txtCostoEP.ClientID %>").attr('disabled', true);
          $("#<%=txtCostoParte2.ClientID %>").attr('disabled', true);
          $("#<%=txtCostoTestimonio.ClientID %>").attr('disabled', true);
          

          var chk_Apoderado = $('#<%= rbApoderado.ClientID %>').prop('checked');
          if (chk_Apoderado) {
              $("#<%= ddl_TipoDocrepresentante.ClientID %>").attr('disabled', true);
              $("#<%= txtRepresentanteNombres.ClientID %>").attr('disabled', true);
              $("#<%= txtRepresentanteNroDoc.ClientID %>").attr('disabled', true);
              $("#<%= ddl_GerenoPresentante.ClientID %>").attr('disabled', true);
              $("#<%= ddl_Apoderado.ClientID %>").attr('disabled', false);
          }

          var chk_Otros = $('#<%= rbOtros.ClientID %>').prop('checked');
          if (chk_Otros) {
              $("#<%= ddl_TipoDocrepresentante.ClientID %>").attr('disabled', false);
              $("#<%= txtRepresentanteNombres.ClientID %>").attr('disabled', false);
              $("#<%= txtRepresentanteNroDoc.ClientID %>").attr('disabled', false);
              $("#<%= ddl_GerenoPresentante.ClientID %>").attr('disabled', false);
              $("#<%= ddl_Apoderado.ClientID %>").attr('disabled', true);
          }

          var chk_Activo = $("#<%=chk_acno_bFlagMinuta.ClientID %>").is(':checked');
          if (chk_Activo) {
              $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', false);
              $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', false);
              $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', false);
              $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', false);
              $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', false);
              $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', false);
          }
          else {
              $("#<%=txt_acno_sAutorizacionTipoId.ClientID %>").attr('disabled', true);
              $("#<%=txtAutorizacionNroDocumento.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_vNumeroColegiatura.ClientID %>").attr('disabled', true);
              $("#<%=txt_ancu_vFirmaIlegible.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_Numero_Minuta.ClientID %>").attr('disabled', true);
              $("#<%=txt_acno_vNombreColegiatura.ClientID %>").attr('disabled', true);
          }
          $("#<%=Btn_AfirmarTextoLeido.ClientID %>").prop("disabled", true);
          $('#<%=cbxAfirmarTexto.ClientID %>').prop("checked", false);
          $("#<%=cbxAfirmarTexto.ClientID %>").attr('disabled', true);
          document.getElementById('MainContent_cbxAfirmarTexto').parentElement.disabled = true;
          //$('#<%=rbApoderado.ClientID %>').attr('disabled', false);
          //$('#<%=rbOtros.ClientID %>').attr('disabled', false);
             
      }

      function calculoVisual() {
          var str_ActoNotarialId = $("#<%=hdn_acno_iActoNotarialId.ClientID %>").val();
          var str_ActuacionId = $("#<%=hdn_acno_iActuacionId.ClientID %>").val();
          var str_ActoNotarialTipo = $("#<%=Cmb_TipoActoNotarial.ClientID%>").val();
          var str_TipoActoNotarial = $("#<%=Cmb_TipoActoNotarial.ClientID%> option:selected").text();
          var str_NumeroPagina = $("#<%=HF_NUMERO_PAGINA_DOCUMENTO.ClientID %>").val();

          prm = {};
          prm.strActoNotarialId = str_ActoNotarialId;
          prm.strActuacionId = str_ActuacionId;
          prm.strActoNotarialTipo = str_ActoNotarialTipo;
          prm.strTipoActoNotarial = str_TipoActoNotarial;
          prm.strNumeroPagina = str_NumeroPagina;
          $.ajax({
              type: "POST",
              url: "FrmActoNotarialProtocolares.aspx/CalculoVisual_EP_Testimonio_Parte",
              contentType: "application/json; charset=utf-8",
              data: JSON.stringify(prm),
              dataType: "json",
              success: function (data) {
                  var obj = $.parseJSON(data.d);
                  $("#<%= txtCostoEP.ClientID %>").val(obj.CostoEP);
                  $("#<%= txtCostoParte2.ClientID %>").val(obj.CostoParte2);
                  $("#<%= txtCostoTestimonio.ClientID %>").val(obj.CostoTestimonio);
              },
              failure: function (response) {
                  showdialog('e', 'Registro Notarial : Cuerpo', response.d, false, 160, 300);
              }
          });

      }

    </script>
    <script src="../Scripts/Validacion/bootstrap.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>

</asp:Content>



