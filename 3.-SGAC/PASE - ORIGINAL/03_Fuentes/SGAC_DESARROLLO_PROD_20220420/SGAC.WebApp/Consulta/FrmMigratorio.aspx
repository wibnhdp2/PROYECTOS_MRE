<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmMigratorio.aspx.cs"
    Inherits="SGAC.WebApp.Consulta.FrmPasaporte" EnableEventValidation="true" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton" TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ddlOficina" TagPrefix="ddlOficina" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

 
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script> 
    

    <script type="text/javascript">
        
        $(function () {
            $('#tabs').tabs();
            $('#tabs').disableTab(1, true);
            $('#tabs').disableTab(2, true); 
            
        });
        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
       
       function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
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
                showdialog('e', 'Consulta Migratorio', 'Número Incorrecto', false, 190, 320);
                control.value = "";
            }
        }
    </script>
      
    <script type="text/javascript">
        function ValidarGrabar() {

            var s_Valiar = true;
            
            var s_Tarifa = document.getElementById('<%= ddlTipoDocConsulta.ClientID %>').value;

            var s_Aprobado = document.getElementById('<%=hId_AprobadoEstado.ClientID %>').value;

            if (s_Aprobado == '0' || s_Aprobado == '') {
                showdialog('a', 'Consulta Migratorio', 'Debe elegir un estado de aprobación', false, 190, 320);
                s_Valiar = false;
            }

            switch (s_Tarifa) {
                case "9303":
                    

                    break;
                case "9302": 
                    if (s_Aprobado == '1') {
                        if (txtcontrolError(document.getElementById('<%=txtOtros.ClientID %>')) == false) s_Valiar = false;
                    }
            
                    break;
                case "9301":
                    if (s_Aprobado == '1') {
                        if (txtcontrolError(document.getElementById('<%=txtNumeroPass.ClientID %>')) == false) s_Valiar = false;
                    }
            
                    break;
            }

            

            if (s_Valiar) {

                var migratorio = {};

                migratorio.acmi_iActoMigratorioId = document.getElementById('<%= hdn_acmi_iActoMigratorioId.ClientID %>').value;
                migratorio.acmi_iActuacionDetalleId = document.getElementById('<%= hdn_acmi_iActuacionDetalleId.ClientID %>').value;
                migratorio.acmi_IFuncionarioId = document.getElementById("<%= ddl_acmi_iFuncionariId.ClientID %>").value;
                migratorio.acmi_sTipoDocumentoMigratorioId = document.getElementById('<%= ddlTipoDocConsulta.ClientID %>').value;
                migratorio.acmi_sTipoActoMigratorio = s_Tarifa;
                migratorio.acmi_sPersonaId = document.getElementById('<%= hdn_pers_iPersonaId.ClientID %>').value;


                switch (s_Tarifa) {
                    case "9303": 
                        migratorio.acmi_sTipoId = document.getElementById('<%= ddlVisaTipo.ClientID %>').value;
                        migratorio.acmi_sSubTipoId = document.getElementById('<%= ddlVisaSubTipo.ClientID %>').value;
                        migratorio.acmi_sEstadoId = "49";
                        
                        break;
                    case "9302": 
                        migratorio.acmi_sTipoId = "0";
                        migratorio.acmi_sSubTipoId = "0";
                        migratorio.acmi_sEstadoId = "46";
                        migratorio.acmi_vNumeroDocumentoAnterior = document.getElementById('<%= txtOtros.ClientID %>').value;

                        break;
                    case "9301":
                        
                        migratorio.acmi_sTipoId = document.getElementById('<%= hdn_acmi_sTipoId.ClientID %>').value;
                        migratorio.acmi_sSubTipoId = "0"
                        migratorio.acmi_sEstadoId = "49";

                        break;
                }

                migratorio.ami_EstadoId = document.getElementById('<%= hId_Estado.ClientID %>').value;
                migratorio.acmi_vNumeroExpediente = document.getElementById('<%= txtNumeroExp.ClientID %>').value;
                migratorio.acmi_vNumeroLamina = document.getElementById('<%= txtNumSal_Lam.ClientID %>').value;
                migratorio.acmi_dFechaExpedicion = document.getElementById('<%= txtFecExpedicion.FindControl("TxtFecha").ClientID %>').value;
                migratorio.acmi_dFechaExpiracion = document.getElementById('<%= txtFecExpiracion.FindControl("TxtFecha").ClientID %>').value;
                migratorio.acmi_vNumeroDocumento = document.getElementById("<%= txtNumeroPass.ClientID %>").value;


                var s_envio = {};
                s_envio.actonomigratorio = JSON.stringify(migratorio);


                $.ajax({
                    type: "POST",
                    url: "FrmMigratorio.aspx/actualizar_registro",
                    data: JSON.stringify(s_envio),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == null) {
                            Cambiar_Estado_Migratorio();


                        } else {
                            showdialog('e', 'Consulta Migratorio', 'Error: ' + msg.d, false, 190, 320);
                        }
                    },
                    error: errores

                });
                
            } else {
            return false;
            }
        }

        function Cambiar_Estado_Migratorio() {
            var Parametros = []
            var acmi_iActoMigratorioId = document.getElementById("<%=hdn_acmi_iActoMigratorioId.ClientID %>").value;
            var s_Fecha = "";
            var acmi_sEstadoId = "0";
            var Observaciones = "";
            var achi_vIFuncionarioId = 0;

            var gridView = document.getElementById("<%=gdvSeguimiento.ClientID %>");

            if (gridView == null) {
                showdialog('a', 'Consulta Migratorio', 'No se econtraron registros para aprobar', false, 190, 320);
                return false;
            }

            
            for (var s_Item = 0; s_Item < gridView.rows.length; s_Item++) {
                if (s_Item > 0) {
                    var inputs = gridView.rows[s_Item].cells[3].textContent;
                    if (inputs.trim() != '') {
                        s_Fecha = gridView.rows[s_Item].cells[0].textContent;
                        acmi_sEstadoId = gridView.rows[s_Item].cells[1].textContent;
                        Observaciones = gridView.rows[s_Item].cells[2].textContent;
                        achi_vIFuncionarioId = gridView.rows[s_Item].cells[7].textContent;
                        break;
                    }
                }
            }
           

            Parametros[0] = acmi_iActoMigratorioId;
            Parametros[1] = s_Fecha;
            Parametros[2] = Observaciones;
            Parametros[3] = acmi_sEstadoId;
            Parametros[4] = document.getElementById("<%=ddlTipoDocConsulta.ClientID %>").value;
            if (acmi_sEstadoId == 2) {
                achi_vIFuncionarioId = 0;
            }
            Parametros[5] = achi_vIFuncionarioId;

            $.ajax({
                type: "POST",
                url: "FrmMigratorio.aspx/Estado_Aprobaciones",
                data: '{parametros: ' + JSON.stringify(Parametros) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d == "1") {

                        var s_Estado = $('#<%=hId_AprobadoEstado.ClientID %>').val();
                        if (s_Estado == '1') {
                            showdialog('i', 'Consulta Migratorio', 'Se realizó la aprobación del trámite', false, 190, 320);
                        } else {
                            showdialog('i', 'Consulta Migratorio', 'Se realizó la observación del trámite', false, 190, 320);
                        }

                        $('#tabs').tabs("option", "active", 0);

                        $("#MainContent_ctrlToolBarConsulta_btnBuscar").click();
                    }
                    else if (msg.d == "9") {
                        showdialog('i', 'Consulta Migratorio', 'Se modifico correctamente el acto', false, 190, 320);
                    }
                    else {
                        showdialog('e', 'Consulta Migratorio', 'Error: No se realizó el cambio de estado a la acto migratorio', false, 190, 320);
                    }
                },
                error: function (msg) {
                    showdialog('e', 'Consulta Migratorio', msg.responseText, false, 190, 320);
                }

            });
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

        function Validar_Aprobacion() {
            var bolValida = true;

            if (txtcontrolError(document.getElementById('<%=txtFechaAprobacion.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlActoMigratorioEstado.ClientID %>")) == false) bolValida = false;

            var estado = document.getElementById("<%=ddlActoMigratorioEstado.ClientID %>").value;

            /*
            if (estado == 1) {
                if (ddlcontrolError(document.getElementById("<%=ddlFuncionario.ClientID %>")) == false) bolValida = false;
            }
            */

            return bolValida;
        }

        function Validar_Cambiar_Estado() {
            var bolValida = true;

            
            if (txtcontrolError(document.getElementById('<%=txtFechaBaja.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%=ddlMotivoAnulacion.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById('<%= ddlDatoFuncionario.ClientID %>')) == false) bolValida = false;
            


            if (bolValida) {
                var Parametros = []
                var acmi_iActoMigratorioId = document.getElementById("<%=hId_Migratorio.ClientID %>").value;
                var amhi_IFuncionarioId = document.getElementById('<%= ddlDatoFuncionario.ClientID %>').value;
                var amhi_sMotivoId = document.getElementById("<%=ddlMotivoAnulacion.ClientID %>").value;
                var s_Fecha = document.getElementById('<%=txtFechaBaja.FindControl("TxtFecha").ClientID %>').value;
                var insu_sInsumoTipoId = document.getElementById("<%=hinsu_sInsumoTipoId.ClientID %>").value;
                var insu_iInsumoId = document.getElementById("<%=hinsu_iInsumoId.ClientID %>").value;
                var Observaciones = document.getElementById("<%=txtObservaciones.ClientID %>").value;
                var estado = document.getElementById("<%=hTipo_Baja.ClientID %>").value;
                Parametros[0] = acmi_iActoMigratorioId;
                Parametros[1] = amhi_IFuncionarioId;
                Parametros[2] = amhi_sMotivoId;
                Parametros[3] = s_Fecha;
                Parametros[4] = insu_sInsumoTipoId;
                Parametros[5] = insu_iInsumoId;
                Parametros[6] = Observaciones;
                Parametros[7] = estado;


                $.ajax({
                    type: "POST",
                    url: "FrmMigratorio.aspx/Actualizar_Estado",
                    data: '{parametros: ' + JSON.stringify(Parametros) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d != "0") {
                            document.getElementById("<%=hId_Baja.ClientID %>").value = msg.d;
                            var s_numeroLamina = document.getElementById("<%=txtNumLamina.ClientID %>").value;
                            var s_numeroLibrillo = document.getElementById("<%=txtNumeroPass.ClientID %>").value;
                            var s_combo = document.getElementById("<%=tipoInsumo.ClientID %>").value;
                            var s_texto = "";
                            if (s_combo == "1") {
                                s_texto = "El Librillo número " + s_numeroLamina;
                            }
                            else {
                                s_texto = "La Lámina número " + s_numeroLamina;
                            }
                            $('#<%=btn_Habilitar.ClientID %>').click();

                            if (document.getElementById("<%=hTipo_Baja.ClientID %>").value == "Anulado") {
                                s_texto = s_texto + " anulado correctamente";
                            }
                            else {
                                s_texto = s_texto + "  dado de BAJA correctamente";
                            }
                            showdialog('i', 'Consulta Migratorio', s_texto, false, 190, 320);
                            

                           
                        } else {
                            showdialog('e', 'Consulta Migratorio', 'Error: No se realizó el cambio de estado a la acto migratorio', false, 190, 320);
                        }
                    },
                    error: function (msg) {
                        showdialog('e', 'Consulta Migratorio', msg.responseText, false, 190, 320);
                    }

                });

                return false;
            }
            return bolValida;
        }
        function Validar_Grabar() {
            var gridView = document.getElementById("<%=gdvVisaMigratorio.ClientID %>");

            if (gridView == null) {
                showdialog('a', 'Consulta Migratorio', 'No se econtraron registros para aprobar', false, 190, 320);
                return false;
            }
           
            var acmi_iActoMigratorioId = []
            var i_Contador = -1;
            var IsValid = false;
            for (var s_Item = 0; s_Item < gridView.rows.length; s_Item++) {
                if (s_Item > 0) {
                    IsValid = false;
                    var inputs = gridView.rows[s_Item].cells[17];
                    if (inputs != null) {
                        for (var i = 0; i < inputs.childNodes.length; i++) {
                            if (inputs.childNodes[i].type == "checkbox" && inputs.childNodes[i].checked) {
                                IsValid = true;
                                i_Contador = i_Contador + 1;
                                break;
                            }
                        }
                    }
                    
                    if (IsValid) {
                        acmi_iActoMigratorioId[i_Contador] = gridView.rows[s_Item].cells[1].textContent;
                    }
                }
            }

            if (acmi_iActoMigratorioId.length > 0) {
                showdialog('c', 'Acto Migratorio', '¿Está seguro que desea aprobar los registros?', false, 190, 320, function () {
                    $.ajax({
                        type: "POST",
                        url: "FrmMigratorio.aspx/Cambiar_Estado",
                        data: '{acmi_iActoMigratorioId: ' + JSON.stringify(acmi_iActoMigratorioId) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: resultado,
                        error: errores

                    });
                    $("#msg-dialog").dialog('close');
                });
                
            } else {
                showdialog('a', 'Consulta Migratorio', 'debe seleccionar al menos un item de la grilla', false, 190, 320);
            }

            return false;
        }

        function resultado(msg) {
            if (msg.d == "1") {
                
                window.parent.close_ModalPopup('MainContent_ctrlToolBarConsulta_btnBuscar');
            } else {
                showdialog('e', 'Consulta Migratorio', 'Error: No se realizó el cambio de estado al acto migratorio', false, 190, 320);
            }

        }
        function errores(msg) {
            showdialog('e', 'Consulta Migratorio', 'Error: ' + msg.responseText, false, 190, 320);
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   
    <table class="mTblTituloM2" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloConsultaMigratoria" runat="server" Text="Consultas - Documentos Migratorios"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table width="95%" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblConsulta" runat="server" Text="Consultar"></asp:Label></a></li>
                        <li><a href="#tab-2">
                            <asp:Label ID="lblModifica" runat="server" Text="Modificar"></asp:Label></a></li>
                        <li><a href="#tab-3">
                            <asp:Label ID="lblAnula" runat="server" Text="Anular/De Baja"></asp:Label></a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel ID="updConsulta" runat="server">
                            <ContentTemplate>
                            
                        
                                <div>
                                    <table width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lblOC" runat="server" Text="Oficina Consular:" />
                                            </td>
                                            <td>
                                                <ddlOficina:ddlOficina ID="ddlOficinaConsular" runat="server" />
                                            </td>
                                        </tr>
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lblTipoDocConsulta" runat="server" Text="Tipo:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:DropDownList ID="ddlTipoDocConsulta" runat="server" Width="164px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlTipoDocConsulta_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="lblSubTipoPasaporte" runat="server" Text="Sub Tipo:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:DropDownList ID="ddlSubTipoPasaporte" runat="server" Width="164px">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%">
                                                <asp:Label ID="Label36" runat="server" Text="Nro. Documento:" />
                                            </td>
                                            <td align="right">
                                                <asp:TextBox ID="txtNumDocumento" runat="server" Width="180px" MaxLength="15" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:Panel ID="pnlEstado" runat="server" Visible="true">
                                        <table width="100%">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlEstadoDoc" runat="server" Width="164px">
                                                        <asp:ListItem Text="- TODOS -"></asp:ListItem>
                                                        <asp:ListItem Text="SOLICITADO"></asp:ListItem>
                                                        <asp:ListItem Text="EXPEDIDO"></asp:ListItem>
                                                        <asp:ListItem Text="REVALIDADO"></asp:ListItem>
                                                        <asp:ListItem Text="ANULADO"></asp:ListItem>
                                                        <asp:ListItem Text="BAJA"></asp:ListItem>
                                                        <asp:ListItem Text="APROBADO"></asp:ListItem>
                                                        <asp:ListItem Text="RECHAZADO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="13%">
                                                    <asp:Label ID="lblNroExpediente" runat="server" Text="Nro. Expediente:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtNumExpediente" runat="server" Width="160px" MaxLength="10" 
                                                        onkeypress="return isNumberKey(event)" onBlur="validanumeroLostFocus(this)" />
                                                </td>
                                                <td width="10%">
                                                    <asp:Label ID="lblVisaPaisResidenciaC" runat="server" Text="País Residencia:"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlVisaPaisResidenciaC" runat="server" Width="184px">
                                                        <asp:ListItem Text="- SELECCIONE -"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div>
                                    <asp:Panel ID="pnlTipoVisa" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="lblVisaTipoC" runat="server" Text="Tipo Visa:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlVisaTipoC" runat="server" Width="164px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlVisaTipoC_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="13%">
                                                    <asp:Label ID="lblSubTipo" runat="server" Text="Sub Tipo:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlSubTipo" runat="server" Width="164px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="10%">
                                                    <asp:Label ID="lblVisaEtiEstado" runat="server" Text="Estado Visa:"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlVisaEtiquetaEstado" runat="server" Width="184px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="Label1" runat="server" Text="Primer Apellido:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:TextBox ID="txtPrimerApellido" runat="server" Width="160px" onkeypress="return NoCaracteresEspeciales(event)" CssClass="txtLetra"/>
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="Label2" runat="server" Text="Segundo Apellido:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:TextBox ID="txtSegundoApellido" runat="server" Width="160px" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)"/>
                                            </td>
                                            <td width="10%">
                                                <asp:Label ID="Label34" runat="server" Text="Nombres:"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox ID="txtNombre" runat="server" Width="180px" CssClass="txtLetra" onkeypress="return NoCaracteresEspeciales(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="Label35" runat="server" Text="Fecha Expedición:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <SGAC_Fecha:ctrlDate ID="txtFechaExpedicion" runat="server" />
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="Label40" runat="server" Text="Fecha Expiración:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <SGAC_Fecha:ctrlDate ID="txtFechaExpiracion" runat="server" />
                                            </td>
                                            <td width="10%">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Registro Inicio:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <SGAC_Fecha:ctrlDate ID="txtFecInicio" runat="server" />
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Registro Fin:"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <SGAC_Fecha:ctrlDate ID="txtFecFin" runat="server" />
                                            </td>
                                            <td width="10%">
                                            </td>
                                            <td align="right">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </div>
                          
                        <div>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div style="float:left;">
                                            <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarConsulta" runat="server" />
                                        </div>
                                        <div style="float:right; text-align:right; padding:5px">
                                            <asp:Button ID="btn_grabar" runat="server" Text="    Aprobar" 
                                                CssClass="btnSave"/>
                                        </div>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <Label:Validation ID="Validation1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Button ID="btn_Consultar_Evento" runat="server" Text="..." 
                                                onclick="btn_Consultar_Evento_Click" style="display:none;" />
                                            <table width="100%">
                                                <tr><td><asp:Label ID="lblTexto_9" runat="server" Style="font-weight: 700; color: #800000;" /></td></tr>
                                                <tr><td><Label:Validation ID="ctrlValidacion" runat="server" /></td></tr>
                                            </table>

                                            <asp:GridView ID="gdvMigratorio" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                AutoGenerateColumns="False" GridLines="None" 
                                                onrowcommand="gdvMigratorio_RowCommand" 
                                                onrowdatabound="gdvMigratorio_RowDataBound">
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="acmi_iActuacionDetalleId" HeaderText="acmi_iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="acmi_iActoMigratorioId" HeaderText="acmi_iActoMigratorioId" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pers_iPersonaId" HeaderText="pers_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Tipo Doc. Migratorio" DataField="acmi_vTipoDocumentoMigratorio">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:BoundField>                            
                                                    <asp:BoundField HeaderText="Nro. Expediente" DataField="acmi_vNumeroExpediente">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nro. Documento" DataField="acmi_vNumeroDocumento">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Fecha Registro" DataField="acmi_dFechaCreacion" 
                                                        DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Oficina Consular" DataField="actu_vOficinaConsular">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Estado Trámite" DataField="acmi_vEstado">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Apellidos y Nombres" DataField="PERS_VDATOS">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="180px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Fecha/Hora Observación" 
                                                        DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" 
                                                        DataField="amhi_dFechaRegistro">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Observaciones" DataField="acmi_vObservaciones">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_edit.png" Visible='<%# IsVisible(Eval("acmi_sEstadoID")) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalle Insumos" 
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_detalle" CommandName="Detalle" ToolTip="Ver Detalle" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/clip_image027.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ACMI_VTIPO" HeaderText="ACMI_VTIPO" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="acmi_sEstadoID" HeaderText="acmi_sEstadoID" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                            <asp:GridView ID="gdvVisaMigratorio" runat="server" CssClass="mGrid" 
                                                SelectedRowStyle-CssClass="slt" GridLines="None" 
                                                onrowcommand="gdvVisaMigratorio_RowCommand" AutoGenerateColumns="False">
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="acmi_iActuacionDetalleId" HeaderText="acmi_iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
			                                            ItemStyle-CssClass="ColumnaOculta">
			                                            <HeaderStyle CssClass="ColumnaOculta" />
			                                            <ItemStyle CssClass="ColumnaOculta" />
		                                            </asp:BoundField>
		                                            <asp:BoundField DataField="acmi_iActoMigratorioId" HeaderText="acmi_iActoMigratorioId" HeaderStyle-CssClass="ColumnaOculta"
			                                            ItemStyle-CssClass="ColumnaOculta">
			                                            <HeaderStyle CssClass="ColumnaOculta" />
			                                            <ItemStyle CssClass="ColumnaOculta" />
		                                            </asp:BoundField>
		                                            <asp:BoundField DataField="pers_iPersonaId" HeaderText="pers_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
			                                            ItemStyle-CssClass="ColumnaOculta">
			                                            <HeaderStyle CssClass="ColumnaOculta" />
			                                            <ItemStyle CssClass="ColumnaOculta" />
		                                            </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Nro. Expediente">
			                                            <ItemTemplate>
                                                            <asp:LinkButton ID="lnk_NumeroExpediente" runat="server" Text='<%# Bind("acmi_vNumeroExpediente") %>' CommandName="Cambiar"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"></asp:LinkButton>
			                                            </ItemTemplate>
			                                            <HeaderStyle HorizontalAlign="Center" />
			                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
		                                            </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Nro. Documento" DataField="acmi_vNumeroDocumento">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
		                                            <asp:BoundField HeaderText="Fecha Registro" DataField="acmi_dFechaCreacion" 
			                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
			                                            <HeaderStyle HorizontalAlign="Center" />
			                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
		                                            </asp:BoundField>
                                                    <asp:BoundField HeaderText="Oficina Consular" DataField="actu_vOficinaConsular">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Estado Trámite"  DataField="acmi_vEstado">
			                                            <HeaderStyle HorizontalAlign="Center" />
			                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
		                                            </asp:BoundField>
                                                    <asp:BoundField HeaderText="Estado Visa">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Tipo Visa" DataField="ACMI_VTIPO">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Sub Tipo Visa" DataField="ACMI_VSUBTIPO">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Apellidos y Nombres" DataField="PERS_VDATOS">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="País Residencia">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Fecha/Hora Observación">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnConsultarVisa" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditarVisa" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_16_edit.png" Visible='<%# IsVisible(Eval("acmi_sEstadoID")) %>'/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalle Insumo" 
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnDarBajaVisa" CommandName="DarBaja" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/clip_image027.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Aprobar">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkGridSel" runat="server" Visible='<%# IsVisible(Eval("acmi_sEstadoID")) %>' Enabled='<%# IsEnabled(Eval("acmi_sEstadoID")) %>' oncheckedchanged="chk_item_CheckedChanged" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="acmi_sEstadoID" HeaderText="acmi_sEstadoID" HeaderStyle-CssClass="ColumnaOculta"
			                                            ItemStyle-CssClass="ColumnaOculta">
			                                            <HeaderStyle CssClass="ColumnaOculta" />
			                                            <ItemStyle CssClass="ColumnaOculta" />
		                                            </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div> 
                        <div id="divDetalle" runat="server" >
                            
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTituloBaja" runat="server" Text="DETALLE DE INSUMOS" Style="font-weight: 700;
                                                    color: #800000;" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvDetalle" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                    AutoGenerateColumns="False" GridLines="None" 
                                                    onrowcommand="gdvDetalle_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="acmi_iActuacionDetalleId" HeaderText="acmi_iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
		                                                <asp:BoundField DataField="acmi_iActoMigratorioId" HeaderText="acmi_iActoMigratorioId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
		                                                <asp:BoundField DataField="pers_iPersonaId" HeaderText="pers_iPersonaId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
                                                        <asp:BoundField DataField="insu_iInsumoId" HeaderText="insu_iInsumoId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
		                                                <asp:BoundField DataField="insu_sInsumoTipoId" HeaderText="insu_sInsumoTipoId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
                                                        <asp:BoundField DataField="amhi_sEstadoId" HeaderText="amhi_sEstadoId" HeaderStyle-CssClass="ColumnaOculta"
			                                                ItemStyle-CssClass="ColumnaOculta">
			                                                <HeaderStyle CssClass="ColumnaOculta" />
			                                                <ItemStyle CssClass="ColumnaOculta" />
		                                                </asp:BoundField>
                                                        <asp:BoundField HeaderText="Nro. Expediente" DataField="acmi_vNumeroExpediente">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Oficina Consular" DataField="actu_vOficinaConsular">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tipo Documento" DataField="para_vDescripcion">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Nro. Documento" DataField="vCodigoInsumo">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fecha Solicitud Baja" 
                                                            DataField="amhi_dFechaRegistro" 
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fecha Baja" DataField="amhi_dFechaRegistro" 
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Estado" DataField="acmi_vEstado">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnAnular" CommandName="Anular" ToolTip="Anular" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_16_tramite_anular.png"  />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dar De Baja" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnDarBaja" CommandName="DarBaja" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_16_tramite_dar_baja.png"/>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hinsu_sInsumoTipoId" runat="server" />
                                    <asp:HiddenField ID="hinsu_iInsumoId" runat="server" />
                                    <asp:HiddenField ID="hId_Migratorio" runat="server" />
                                    <asp:HiddenField ID="hdn_acmi_sTipoId" runat="server" />
                                    <asp:HiddenField ID="hdn_Estado" runat="server" />
                                    <asp:HiddenField ID="hId_Estado" runat="server" />
                             
                        </div> 
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                    </div>

                    <div id="tab-2">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <div>
                                    <ToolBarButton:ToolBarButtonContent ID="toolbarModificar" runat="server" />
                                </div>
                                <div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Nro. Exp: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumeroExp" runat="server" Width="100px" onkeypress="return isNumberKey(event)" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTexto_1" runat="server" Text="Nro. Pasaporte: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hId_AprobadoEstado" runat="server" Value="" />
                                                <asp:TextBox ID="txtNumeroPass" runat="server" Width="100px" onkeypress="return NoCaracteresEspeciales(event)" MaxLength="7" CssClass="txtLetra" onBlur="conMayusculas(this)"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTexto_2" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumSal_Lam" runat="server" Width="100px" onkeypress="return NoCaracteresEspeciales(event)" CssClass="txtLetra" onBlur="conMayusculas(this)" maxLenght="30"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNumeroSalvoconducto" runat="server" Text="Nro. salvoconducto:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOtros" runat="server" Width="100px" onkeypress="return NoCaracteresEspeciales(event)" CssClass="txtLetra" onBlur="conMayusculas(this)" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTexto_3" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecExpedicion" runat="server" EnabledText="false" />                                                
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTexto_4" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecExpiracion" runat="server" EnabledText="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                   <table>
                                        <tr>
                                            <td colspan="2"><asp:HiddenField ID="hdn_acmi_iActoMigratorioId" runat="server" Value="0" /></td>
                                            <td colspan="2"><asp:HiddenField ID="hdn_acmi_iActuacionDetalleId" runat="server" Value="0" /></td>
                                            <td colspan="2"><asp:HiddenField ID="hdn_pers_iPersonaId" runat="server" Value="0" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <asp:HiddenField ID="hiActoMigratorioFormatoId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_resi_iResidenciaId_peru" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdn_resi_iResidenciaId_extranjero" runat="server" Value="0" />
                                            </td>
                                        </tr>
                               
                                <tr>
	                                <td style="width:100px"><asp:Label ID="lblTitularTipoDocumento" runat="server" Text="Tipo de Documento:" /></td>
	                                <td><asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularNumDocumento" runat="server" Text="Número Documento: " /></td>
	                                <td><asp:TextBox ID="txt_numero_doc" runat="server" Width="195px" 
                                            CssClass="campoNumero" Enabled="False" /></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularGenero" runat="server" Text="Género: " /></td>
	                                <td><asp:DropDownList ID="ddlGenero" runat="server" Width="200px" Height="22px" 
                                            Enabled="False"></asp:DropDownList></td>
                                </tr>
                                <tr>
	                                <td style="width:100px"><asp:Label ID="lblTitularFecNacimiento" runat="server" Text="Fecha Nacimiento:" /></td>
	                                <td><SGAC_Fecha:ctrlDate ID="txtFecNacimiento" runat="server" Enabled="false" /></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularEstadoCivil" runat="server" Text="Estado Civil: " /></td>
	                                <td><asp:DropDownList ID="ddlEstadoCivil" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularOcupacion" runat="server" Text="Ocupación: " /></td>
	                                <td><asp:DropDownList ID="ddlOcupacionAct" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblTitularPrimerApe" runat="server" Text="Primer Apellido: " /></td>
	                                <td><asp:TextBox ID="txtApePat" runat="server" Width="195px" CssClass="txtLetra" Enabled="False" 
		                                MaxLength="50" onkeypress="return isNombreApellido(event)" 
                                            onBlur="conMayusculas(this)"/></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularSegundoApe" runat="server" Text="Segundo Apellido: " /></td>
	                                <td><asp:TextBox ID="txtApeMat" runat="server" Width="195px" CssClass="txtLetra" MaxLength="50"
		                                onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" 
                                            Enabled="False" /></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularNombres" runat="server" Text="Nombres: " /></td>
	                                <td><asp:TextBox ID="txtNombres" runat="server" Width="195px" CssClass="txtLetra"  
                                            Enabled="False" MaxLength="50"
		                                onkeypress="return isNombreApellido(event)" onBlur="conMayusculas(this)" /></td>
                                </tr>
                                <tr>
	                                <td colspan="8"><asp:Label ID="lblTituloLugarNacimiento" runat="server" 
                                            Text="Lugar de Nacimiento:" Font-Bold="True" /></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblTitularNacimientoDepa" runat="server" Text="Departamento: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioNacDepa" runat="server" Width="200px" 
			                                Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularNacimientoProv" runat="server" Text="Provincia: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioNacProv" runat="server" Width="200px" 
			                                Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblTitularNacimientoDist" runat="server" Text="Distrito: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioNacDist" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
                                </tr>
                                <tr>
	                                <td colspan="8"><asp:Label ID="lblTituloDomicilioPeru" runat="server" 
                                            Text="Domicilio en el Perú:" Font-Bold="True" /></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblResidenciaDepartamento" runat="server" Text="Departamento: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioPeruDepa" runat="server" Width="200px" 
			                                Height="22px" Enabled="False" ></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblResidenciaProvincia" runat="server" Text="Provincia: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioPeruProv" runat="server" Width="200px" 
			                                Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblResidenciaDistrito" runat="server" Text="Distrito: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioPeruDist" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblResidenciaDireccion" runat="server" Text="Dirección: " /></td>
	                                <td colspan="4"><asp:TextBox ID="txtDomicilioPeru" runat="server" Width="100%" CssClass="txtLetra" 
	                                onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" 
                                            Enabled="False"/></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblResidenciaTelefono" runat="server" Text="Teléfono: " /></td>
	                                <td><asp:TextBox ID="txtTelefonoPeru" runat="server" Width="195px" 
                                            CssClass="txtLetra" onkeypress="return isNumberKey(event)" Enabled="False" /></td>
                                </tr>
                                <tr>
	                                <td colspan="8"><asp:Label ID="Label5" runat="server" 
                                            Text="Domicilio en el Extranjero:" Font-Bold="True" /></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblExtranjeroContinente" runat="server" Text="Continente: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioExtrDepa" runat="server" Width="200px" 
			                                Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblExtranjeroPais" runat="server" Text="País: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioExtrProv" runat="server" Width="200px" 
			                                Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblExtranjeroCiudad" runat="server" Text="Ciudad: " /></td>
	                                <td><asp:DropDownList ID="ddlDomicilioExtrDist" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
                                </tr>
                                <tr>
	                                <td><asp:Label ID="lblExtranjeroDireccion" runat="server" Text="Dirección: " /></td>
	                                <td colspan="4"><asp:TextBox ID="txtDomicilioExtra" runat="server" Width="100%" CssClass="txtLetra" 
		                                onkeypress="return NoCaracteresEspeciales(event)" onBlur="conMayusculas(this)" 
                                            Enabled="False"/></td>
	                                <td style="width:10px"></td>
	                                <td><asp:Label ID="lblExtranjeroTelefono" runat="server" Text="Teléfono: " /></td>
	                                <td><asp:TextBox ID="txtTelefonoExtr" runat="server" Width="195px" 
                                            CssClass="txtLetra" onkeypress="return isNumberKey(event)" Enabled="False" /></td>
                                </tr>
                                <tr id="tr_Caracteristicas" runat="server">
                                    <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                        <asp:Label ID="Label29" Text="CARACTERÍSTICAS FÍSICAS" runat="server" Style="width:100%; font-weight: 700; color: #800000;" />
                                    </td>
                                </tr>
                                <tr id="tr_Carac" runat="server">
	                                <td style="width:100px"><asp:Label ID="lblTitularColorOjos" runat="server" Text="Color Ojos:" /></td>
	                                <td><asp:DropDownList ID="ddlColorOjo" runat="server" Width="200px" Height="22px" 
                                            Enabled="False"></asp:DropDownList></td>                               
	                                <td style="width:10px"></td>
	                                <td style="width:92px"><asp:Label ID="lblTitularColorCabello" runat="server" Text="Color Cabello:" /></td>
	                                <td><asp:DropDownList ID="ddlColorCabello" runat="server" Width="200px" 
                                            Height="22px" Enabled="False"></asp:DropDownList></td>
	                                <td style="width:10px"></td>
	                                <td style="width:60px"><asp:Label ID="lblTitularEstatura" runat="server" Text="Estatura: " /></td>
	                                <td>
		                                <asp:TextBox ID="txtEstatura" runat="server" Width="100px" CssClass="campoNumero" 
                                            onkeypress="return isDecimalKey(this,event)" Enabled="False" />
		                                <asp:Label ID="lblEtiquetaEstatura" runat="server" Text="cms." CssClass="lblEtiquetaLeyenda"></asp:Label>
	                                </td>
                                </tr>
                                <tr id="tr_Filia_" runat="server">
                                    <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                        <asp:Label ID="Label10" Text="FILIACIÓN" runat="server" Style="width:100%; font-weight: 700; color: #800000;" />
                                    </td>
                                </tr>
                                <tr id="tr_Filia_1" runat="server">
	                                <td colspan ="8"><asp:HiddenField ID="hId_Padre" runat="server" /></td>
                                </tr>
                                <tr id="tr_Filia_2" runat="server">
                                    <td style="width:100px"><asp:Label ID="lblTitularFiliacionPadre" runat="server" Text="Vínculo:" /></td>
                                    <td><asp:DropDownList ID="ddlTitularFiliacionPadre" runat="server" Width="200px" Height="22px" Enabled="false"></asp:DropDownList></td>
                                    <td style="width:10px"></td>
                                    <td><asp:Label ID="lblPadreNombres" runat="server" Text="Datos: " /></td>
                                    <td colspan="4"><asp:TextBox ID="txtPadreNombres" runat="server" Width="95%" CssClass="txtLetra" Enabled="false" /></td>
                                </tr>
                                <tr id="tr_Filia_3" runat="server">
	                                <td colspan ="8"><asp:HiddenField ID="hId_Madre" runat="server" /></td>
                                </tr>
                                <tr id="tr_Filia_4" runat="server">
                                    <td style="width:100px"><asp:Label ID="lblTitularFiliacionMadre" runat="server" Text="Vínculo:" /></td>
                                    <td><asp:DropDownList ID="ddlTitularFiliacionMadre" runat="server" Width="200px" Height="22px" Enabled="false"></asp:DropDownList></td>
                                    <td style="width:10px"></td>
                                    <td><asp:Label ID="lblMadreNombres" runat="server" Text="Datos: " /></td>
                                    <td colspan="4"><asp:TextBox ID="txtMadreNombres" runat="server" Width="95%" CssClass="txtLetra" Enabled="false" /></td>
                                </tr>  
                                <tr>
                                    <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; width:"100%">
                                        <asp:Label ID="Label11" Text="Funcionario:" runat="server" Width="100px" />
                                        <asp:DropDownList ID="ddl_acmi_iFuncionariId" runat="server" Enabled="false" Width="400px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                        <asp:Label ID="Label6" Text="IMÁGENES HISTÓRICAS" runat="server" Style="width:100%; font-weight: 700; color: #800000;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Image ID="imgFoto" Height="100px" Width="100px" runat="server" 
                                            ImageUrl="~/Images/imagen-no-disponible.jpg" BorderWidth="1px" />
                                    </td>
                                    <td style="width:10px"></td>
                                    <td colspan="2" align="center">
                                        <asp:Image ID="imgFirma" runat="server" Height="100px" Width="100px" 
                                            ImageUrl="~/Images/imagen-no-disponible.jpg" BorderWidth="1px" />
                                    </td>
                                    <td style="width:10px"></td>
                                    <td colspan="2" align="center">
                                        <asp:Image ID="imgHuella" runat="server" Height="100px" Width="100px" 
                                            ImageUrl="~/Images/imagen-no-disponible.jpg" BorderWidth="1px" />    
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">FOTO</td>
                                    <td style="width:10px"></td>
                                    <td colspan="2" align="center">FIRMA</td>
                                    <td style="width:10px"></td>
                                    <td colspan="2" align="center">HUELLA</td>
                                </tr>                                
                            </table>
                                   <div id="div_apr" runat="server">
                                        <table align="center" width="100%">
                                           <tr>
                                                <td colspan="7" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                    <asp:Label ID="Label8" Text="APROBACIONES" runat="server" Style="width:100%; font-weight: 700; color: #800000;" />
                                                </td>
                                            </tr>
                                           <tr>
                                                <td><asp:Label ID="lblFechaAprobacion" runat="server" Text="Fecha:" Width="100px" /></td>
                                                <td><SGAC_Fecha:ctrlDate ID="txtFechaAprobacion" runat="server" />
                                                    <asp:Label ID="Label30" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                                <td style="width:20px"></td>
                                                <td><asp:Label ID="Label31" runat="server" Text="Estado:" Width="100px" /></td>
                                                <td>
                                                    <asp:DropDownList Width="200px" runat="server" ID="ddlActoMigratorioEstado" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddlActoMigratorioEstado_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">- SELECCIONAR -</asp:ListItem>
                                                    <asp:ListItem Value="1">APROBADO</asp:ListItem>
                                                    <asp:ListItem Value="2">OBSERVADO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label12" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                           
                                           <tr>
                                            <td>
                                                <asp:Label ID="Label13" runat="server" Text="Funcionario:" Visible="False"></asp:Label></td>
                                                <td><asp:DropDownList ID="ddlFuncionario" runat="server" Width="98%" Visible="False">
                                                    </asp:DropDownList></td>
                                           </tr>
                                           <tr>
                                                <td><asp:Label ID="Label33" runat="server" Text="Observaciones:" /></td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtActoMigratorioObservaciones" runat="server" Height="30px" Width="95%" TextMode="MultiLine"
                                                        CssClass="txtLetra" />
                                                </td>
                                                <td style="width:20px"></td>
                                                <td><asp:Button ID="btnAgregarObservacion" runat="server" Text="Aceptar" 
                                                        CssClass="btnGeneral" onclick="btnAgregarObservacion_Click" OnClientClick="return Validar_Aprobacion();" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                   <asp:Panel ID="pnl_Aprobaciones" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="7" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                    <asp:Label ID="Label28" runat="server" Text="DETALLE DE OBSERVACIONES" Style="width:100%; font-weight: 700; color: #800000;" />
                                                </td>
                                            </tr>
                                                   <tr><td colspan="7"><Label:Validation ID="valObservacion" runat="server" /></td></tr>
                                                   <tr>
                                                <td colspan="7">
                                                    <asp:GridView ID="gdvSeguimiento" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False" GridLines="None">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="dFechaRegistro" HeaderText="dFechaRegistro" HeaderStyle-CssClass="ColumnaOculta"
			                                                    ItemStyle-CssClass="ColumnaOculta">
			                                                    <HeaderStyle CssClass="ColumnaOculta" />
			                                                    <ItemStyle CssClass="ColumnaOculta" />
		                                                    </asp:BoundField>
		                                                    <asp:BoundField DataField="sEstadoId" HeaderText="sEstadoId" HeaderStyle-CssClass="ColumnaOculta"
			                                                    ItemStyle-CssClass="ColumnaOculta">
			                                                    <HeaderStyle CssClass="ColumnaOculta" />
			                                                    <ItemStyle CssClass="ColumnaOculta" />
		                                                    </asp:BoundField>
		                                                    <asp:BoundField DataField="vObservacion" HeaderText="vObservacion" HeaderStyle-CssClass="ColumnaOculta"
			                                                    ItemStyle-CssClass="ColumnaOculta">
			                                                    <HeaderStyle CssClass="ColumnaOculta" />
			                                                    <ItemStyle CssClass="ColumnaOculta" />
		                                                    </asp:BoundField>
                                                            <asp:BoundField DataField="amhi_sInsumoId" HeaderText="amhi_sInsumoId" HeaderStyle-CssClass="ColumnaOculta"
			                                                    ItemStyle-CssClass="ColumnaOculta">
			                                                    <HeaderStyle CssClass="ColumnaOculta" />
			                                                    <ItemStyle CssClass="ColumnaOculta" />
		                                                    </asp:BoundField>
                                                            <asp:BoundField HeaderText="Fecha Registro" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" 
                                                                DataField="dFechaRegistro">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Estado" DataField="vEstado">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Observación" DataField="vObservacion">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="450px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="sFuncionarioId" HeaderText="sFuncionarioId" HeaderStyle-CssClass="ColumnaOculta"
			                                                    ItemStyle-CssClass="ColumnaOculta">
			                                                    <HeaderStyle CssClass="ColumnaOculta" />
			                                                    <ItemStyle CssClass="ColumnaOculta" />
		                                                    </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>                                  
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-3">
                        <asp:UpdatePanel ID="UpnlBaja" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hId_Baja" runat="server" />
                                <asp:HiddenField ID="hTipo_Baja" runat="server" />
                                <div align="left">
                                    <ToolBarButton:ToolBarButtonContent ID="toolbarAnular" runat="server" />
                                    <asp:Button ID="btn_Habilitar" runat="server" Text="Habilitar" 
                                        onclick="btn_Habilitar_Click" style="display:none;" />
                                    

                                    <br />
                                    <table id="tbAnular" width="700px" runat="server" 
                                        style="border-bottom: 1px solid #800000; width: 100%; border-bottom-color: #800000;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Nro. Exp: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumExpediernte" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTexto_5" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumPasaporte" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLamina" runat="server" Text="Nro. Lamina:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumLamina" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <div>
                                        <asp:Panel ID="pnlVisaBaja" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label47" runat="server" Text="Tipo: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlVisaTipo" runat="server" Width="150px"
                                                            onselectedindexchanged="ddlVisaTipo_SelectedIndexChanged" 
                                                            AutoPostBack="True">
                                                            <asp:ListItem Text="TEMPORAL"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label48" runat="server" Text="Visa:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlVisaSubTipo" runat="server" Width="150px">
                                                            <asp:ListItem Text="ESTUDIANTE"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label49" runat="server" Text="Titular/Familia"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTitularFamiliar" runat="server" Width="150px" Enabled="false">
                                                            <asp:ListItem Text="TITULAR"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label50" runat="server" Text="T. Permanencia (días): *"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTiempoPermanencia" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <div>
                                        <asp:Panel ID="pnlDatosTitular" runat="server">
                                            <table style="border-top: 1px solid #800000; width: 100%; border-top: #800000;">
                                                <tr>
                                                    
                                                    <td colspan="8" style="border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                        <asp:Label ID="Label14" runat="server" Text="DATOS GENERALES DEL TITULAR" Style="width:100%; font-weight: 700; color: #800000;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDatoTipoDocumento" runat="server" Text="Tipo Documento: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDatoTipoDocumento" runat="server" Width="200px" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDatoNumDocumento" runat="server" Text="Nro. Documento: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDatoNumDocumento" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDatoGenero" runat="server" Text="Género: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDatoGenero" runat="server" Width="120px" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDatoPrimerApe" runat="server" Text="Primer Apellido: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDatoPrimerApe" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDatoSegundoApe" runat="server" Text="Segundo Apellido: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDatoSegundoApe" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDatoNombres" runat="server" Text="Nombres: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDatoNombres" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label15" runat="server" Text="Fecha nacimiento: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_fechaNacimiento" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="lblTexto_7" runat="server" Text="" Style="width:100%; font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="700px">
                                        <tr>
                                            <td width="200px">
                                                <asp:Label ID="lblFechaAnulacion" runat="server" Text="Fecha Baja: "></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFechaBaja" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200px" >
                                                <asp:Label ID="lblTexto_8" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlMotivoAnulacion" runat="server" Width="434px">
                                                    <asp:ListItem Text="- SELECCIONAR -"></asp:ListItem>
                                                    <asp:ListItem Text="AGOTAMIENTO DE PÁGINAS"></asp:ListItem>
                                                    <asp:ListItem Text="DETERIORO"></asp:ListItem>
                                                    <asp:ListItem Text="DETERIORO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200px">
                                                <asp:Label ID="Label4" runat="server" Text="Tipo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="tipoInsumo" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_Estado" runat="server">
                                            <td width="200px">
                                                <asp:Label ID="Label18" runat="server" Text="Estado del Pasaporte:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="cbo_estadoTramite" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="tr_fechas" runat="server">
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Text="Fecha de Expedición :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_expedicion" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text="Fecha de Expiración :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Expiracion" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="Label37" runat="server" Text="OTROS" Style="width:100%; font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="700px">
                                        <tr>
                                            <td width="200px">
                                                <asp:Label ID="Label9" runat="server" Text="Nombre del Funcionario Responsable: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDatoFuncionario" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200px">
                                                <asp:Label ID="lblTexto_6" runat="server" Text="Nro. Correlativo Actuación Consular: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCorreActuacion" runat="server" Width="100px" Enabled="False" style="text-align:right;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200px">
                                                <asp:Label ID="Label32" runat="server" Text="Observaciones:" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtObservaciones" runat="server" Height="30px" Width="430px" TextMode="MultiLine"
                                                    CssClass="txtLetra" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tb_Formulario" runat="server" style="width: 100%">
                                        <tr>
                                           
                                            <td colspan="8" style="border-top: 1px solid #800000; border-top-color: #800000; border-bottom: 1px solid #800000; border-bottom-color: #800000; width: 100%;">
                                                <asp:Label ID="Label39" runat="server" Text="FORMULARIO DGC: " Style="width:100%; font-weight: 700; color: #800000;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table >
                                       
                                        <tr>
                                            <td colspan="2">
                                               
                                                <asp:Button ID="btnFormulario" runat="server" Text="   Formulario" Width="180px"
                                                    CssClass="btnPrint" onclick="btnFormulario_Click" />
                                            </td>
                                        </tr>
                                    </table>
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

    </script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</asp:Content>
