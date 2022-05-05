<%@ Page Language="C#"  MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="FrmActuacion.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmActuacion" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlUploader.ascx" tagname="ctrlUploader" tagprefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton" TagName="ToolBarButtonContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">     
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script> 
    <script src="../Scripts/site.js" type="text/javascript"></script>    
    <script src="../Scripts/Validacion/Text.js" type="text/javascript"></script>
    
   <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tMsjeWarnig
        {
            background-color: #F2F1C2;
            border-color: #f89406;
            color: #4B4F5E;
            height: 15px;
            width: 100%;
        }
        .style1
        {
            height: 37px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
            $('#tabs').disableTab(1, false);
            $('#tabs').disableTab(2, false);
            $('#tabs').disableTab(3, false);
            $('#tabs').disableTab(4, false);
            $('#tabs').disableTab(5, false);
            $('#tabs').disableTab(6, false);
        });

        function validarObligario() {
            if ($('#<%= chkObligatorio.ClientID %>').prop('checked')) {
                $('#<%= lblddlClasificacion.ClientID %>').show();
            }
            else {
                $('#<%= lblddlClasificacion.ClientID %>').hide();
                var cmbClas = document.getElementById('<%= ddlClasificacion.ClientID %>');
                cmbClas.style.border = "1px solid #888888";
            }
        }

        function ValidarRegistroActuacion() {
            var bolValida = true;

            var strTarifa = $.trim($("#<%= txtIdTarifa.ClientID %>").val());
            var strTipoPago = $.trim($("#<%= ddlTipoPago.ClientID %>").val());
            var strCantidad = $.trim($("#<%= txtCantidad.ClientID %>").val());
            var strTopeMinimo = $.trim($("#<%= hdn_tope_min.ClientID %>").val());

            var strDescripcion = $.trim($("#<%= txtDescTarifa.ClientID %>").val());

            var txtIdTarifa = document.getElementById('<%= txtIdTarifa.ClientID %>');
            var cmb_TipoPago = document.getElementById('<%= ddlTipoPago.ClientID %>');
            var txtCantidad = document.getElementById('<%= txtCantidad.ClientID %>');
            var txtDescripcion = document.getElementById('<%= txtDescTarifa.ClientID %>');

            var str_HF_PAGADO_EN_LIMA = $("#<%=HF_PAGADO_EN_LIMA.ClientID %>").val();
            var str_HF_DEPOSITO_CUENTA = $("#<%=HF_DEPOSITO_CUENTA.ClientID %>").val();
            var str_HF_TRANSFERENCIA = $("#<%=HF_TRANSFERENCIA.ClientID %>").val();

            var strClasificacion = $("#<%=ddlClasificacion.ClientID %>").val();
            var cmbClasificacion = document.getElementById('<%= ddlClasificacion.ClientID %>');

//            if ($('#<%= chkObligatorio.ClientID %>').prop('checked')) {
//                 if (strClasificacion == "0") {
//                     if (cmbClasificacion != null) {
//                         cmbClasificacion.style.border = "1px solid Red";
//                     }
//                     bolValida = false;
//                }
//             }
//             else{
//                 if (cmbClasificacion != null)
//                     cmbClasificacion.style.border = "1px solid #888888";
//             }

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


        function Validaciones_Migratorio() {
            var acde_sTarifarioId = document.getElementById('<%= hacde_sTarifarioId.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "FrmActuacion.aspx/Validaciones_Migratorio",
                data: '{acde_sTarifarioId: ' + JSON.stringify(acde_sTarifarioId) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                     
                    if (msg.d != '') {
                        document.getElementById('<%= h_Mensaje.ClientID %>').value = msg.d;
                    }
                },
                error: function (err) {
                    alert('Error: ' + err.responseText);
                }
            });

 
            if (document.getElementById('<%= h_Mensaje.ClientID %>').value != '') {
                alert(document.getElementById('<%= h_Mensaje.ClientID %>').value);
                return false;
            } else {
                return true;
            }
        }

        function validatePass(campo) 
        {
            var RegExPattern = /[^0-9A-Za-z]/g;
            var errorMessage = 'Password Incorrecta.';
            if ((campo.value.match(RegExPattern)) && (campo.value != '')) {
                alert('Password Correcta');
            } else {
                alert(errorMessage);
                campo.focus();
            } 
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
            

            letras = "¡";
            tecla = String.fromCharCode(charCode);
            n = letras.indexOf(tecla);
            if (n > -1) {
                letra = false;
            }
            

           
            return letra;
        }

        function Popup() {
            document.getElementById('simpleModal_1').style.display = 'block';
        }
        function cerrarPopup() {

            document.getElementById('simpleModal_1').style.display = 'none';
        }
    </script>   
    
    
    <!-- Script by hscripts.com -->
    <script type="text/javascript">
        var r = { 'special': /[\W]/g }
        function valid(o, w) {
            o.value = o.value.replace(r[w], '');
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
    <%--Consulta --%>
    <table class="mTblTituloM" align="center">
        <tr><td><h2><asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Actuación Consular"></asp:Label></h2></td></tr>
    </table>
    <table width="90%" align="center">
            <tr>
                <td runat="server" id="msjeWarningStock" visible="false" colspan="8">
                    <table id="tMsjeWarnigStock" class="tMsjeWarnig" align="center">
                        <tr>
                            <td width="40px">
                                <asp:Image ID="imgIcono" runat="server" ImageUrl="~/Images/img_16_warning.png" CssClass="imgIcono" />
                            </td>
                            <td>
                                <asp:Label ID="lblMsjeWarnigStock" runat="server" Text="Mensaje Validación" CssClass="lblEtiqueta"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4" style="font-weight: bold;">
                </td>
                <td align="right" colspan="4" style="font-weight: bold;">
                    <asp:UpdatePanel ID="updSaldoAutoadhesivo" UpdateMode="Conditional" runat="server"
                        ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:Label ID="lblSaldoEtiqueta" runat="server" Text="Saldo de Autoadhesivos :"></asp:Label>
                            <asp:Label ID="lblSaldoInsumo" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    <table style="width: 90%;" align="center" class="mTblSecundaria" bgcolor="#4E102E">
        <tr>
            <td align="left">
                <table>                                        
                    <tr>                                            
                        <td>
                            <asp:Label ID="lblDestino" runat="server" Font-Bold="True" BackColor=""
                                Font-Names="Arial" Font-Size="10pt" Font-Underline="false" 
                                ForeColor="White" Text=""></asp:Label>     
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div id="simpleModal_1" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopup(); return false" runat="server" />
                <span>REGISTRO ACTUACIÓN</span>
            </div>
            <div class="modal-cuerpo">
                <h3>
                    <asp:Label ID="lblTexto" runat="server" Text=""></asp:Label>
                </h3>
            </div>
            <div class="modal-pie">
                <asp:Button ID="btnSIDeclarante" runat="server" CssClass="btnLogin" Text="SI" 
                    onclick="btnSIDeclarante_Click"/>
                <asp:Button ID="btnNODeclarante" runat="server" CssClass="btnLogin" Text="NO" 
                    onclick="btnNODeclarante_Click"/>
            </div>

        </div>
    </div>
    <table style="width: 90%;" align="center" >
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-0"><asp:Label ID="lblRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        <li><a href="#tab-1"><asp:Label ID="lblDatos" runat="server" Text="Formato"></asp:Label></a></li>
                        <li><a href="#tab-2"><asp:Label ID="lblAdjuntos" runat="server" Text="Adjuntos"></asp:Label></a></li>
                        <li><a href="#tab-3"><asp:Label ID="lblAnotacion" runat="server" Text="Anotaciones"></asp:Label></a></li>
                        <li><a href="#tab-4"><asp:Label ID="lblFicha" runat="server" Text="Ficha Registral"></asp:Label></a></li>
                        <li><a href="#tab-5"><asp:Label ID="lblAntecedente" runat="server" Text="Antecedentes Penales"></asp:Label></a></li>
                        <li><a href="#tab-6"><asp:Label ID="lblVinculacion" runat="server" Text="Vinculación"></asp:Label></a></li>
                    </ul>
                    
                    <div id="tab-0">
                        <table>                                
                            <tr>
                                <td>
                                    <ToolBarButton:ToolBarButtonContent ID="ctrlToolBarRegistroActuacion" runat="server"></ToolBarButton:ToolBarButtonContent>
                                </td>
                                <td colspan="4" style="float:right;">
                                    <asp:Label ID="lblFecAct" runat="server" Text="Fecha Registro: "></asp:Label>                                                
                                    <asp:Label ID="LblFecha" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="updRegPago" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>                    
                    <table>
                        <tr>
                            <td>
                            </td> 
                            <td colspan="3">
                                <asp:HiddenField ID="hidEventNuevo" runat="server" />
                                <asp:HiddenField ID="hid_iRecurrenteId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td> 
                            <td colspan="3">
                                <asp:Label ID="lblValidacionRegistro" runat="server" Text="Falta validar algunos campos."
                                        CssClass="hideControl" ForeColor="Red"></asp:Label>
                                <asp:HiddenField ID="HFValidarTexto" runat="server" />
                                <asp:HiddenField ID="HFValidarNumeroDecimal" runat="server" />
                                <asp:HiddenField ID="hSeccionId" runat="server" />
                                <asp:HiddenField ID="HFValidarNumero" runat="server" />
                                <asp:HiddenField ID="hacde_sTarifarioId" runat="server" />
                                <asp:HiddenField ID="h_Mensaje" runat="server" />
                                <asp:HiddenField ID="HF_ACT_DET_AUX" runat="server" />


                                <asp:HiddenField ID="HF_NACIONALIDAD_PERUANA" Value="0" runat="server" />
                                <asp:HiddenField ID="HF_PAGADO_EN_LIMA" Value="3501" runat="server" />
                                <asp:HiddenField ID="HF_TRANSFERENCIA" Value="3507" runat="server" />
                                <asp:HiddenField ID="HF_DEPOSITO_CUENTA" Value="3508" runat="server" />                                

                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>                                                
                            <td align="left">
                                
                            </td>                                                                       
                            <td>  
                                
                                <asp:HiddenField ID="hidPagoId" runat="server" /> 
                            </td>
                            <td><asp:HiddenField ID="hdn_tari_vDescripcionLarga" runat="server" Value="" /></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right" style="margin-top:0px;padding-top:0px;">
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
                                                                    <asp:TextBox ID="txtIdTarifa" runat="server" Width="38px" CssClass="campoNumero txtLetra" 
                                                                        MaxLength="5" onpaste="return false"
                                                                        ToolTip="Digite el número de la tarifa consular"  onkeypress="return isNumeroLetra(event)" 
                                                                        OnTextChanged="txtIdTarifa_TextChanged"    AutoPostBack="True" />
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
                                                                                ToolTip="Buscar" OnClick="imgBuscarTarifarioM_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:ListBox ID="LstTarifario" runat="server" Width="659px" AutoPostBack="True" OnSelectedIndexChanged="LstTarifario_SelectedIndexChanged"
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
                                            <asp:RadioButton ID="RBNormativa" runat="server" Text=""  style="cursor:pointer"
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
                                        <asp:RadioButton ID="RBSustentoTipoPago" runat="server" Text=""   style="cursor:pointer"
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
                                <asp:Label ID="lblClasificacion" runat="server" Text="Clasificación:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlClasificacion" runat="server" Enabled="True" 
                                    Height="20px" Width="420px" />
                                <asp:Label ID="lblddlClasificacion" runat="server" Style="color: #FF0000" 
                                    Text="*" Visible="False"></asp:Label>
                                <asp:CheckBox ID="chkObligatorio" runat="server" Checked="True" 
                                    onclick="validarObligario();" Text="¿Es Obligatorio?" Visible="False" />
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
                        <tr align="left">                                        
                            <td colspan="4" style="text-align: center">
                                <asp:Label ID="lblTituloActGeneradas" runat="server" Text="Actuaciones Generadas" 
                                    style="font-weight: 700; color: #800000; text-decoration: underline"></asp:Label><br/>
                            </td>                                                
                        </tr>
                        <tr> 
                            <td colspan="4">
                            <asp:GridView ID="gdvActuacionesGeneradas"
                                CssClass="mGrid" 
                                AlternatingRowStyle-CssClass="alt" 
                                runat="server" 
                                AutoGenerateColumns="False"
                                Width="100%"
                                GridLines="None" onrowcommand="gdvActuacionesGeneradas_RowCommand" 
                                    onrowdatabound="gdvActuacionesGeneradas_RowDataBound">
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="iActuacionDetalleId" HeaderText="ActuacionDetalleId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="iActuacionId" HeaderText="ActuacionId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="sTarifarioId" HeaderText="TarifarioId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="vCorrelativoActuacion" HeaderText="R.G.E." >
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vCorrelativoTarifario" 
                                        HeaderText="Corr. Tarifa" >
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dFechaRegistro" HeaderText="Fecha" 
                                        DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vTarifa" HeaderText="Tarifa" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vDescripcion" HeaderText="Descripción" />
                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton
                                                ID="btnEditar0"
                                                CommandName="Editar"
                                                tooltip="Editar Actuación"
                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                runat="server" 
                                                ImageUrl="../Images/img_16_edit.png" />
                                        </ItemTemplate>                                                   
                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Consultar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton
                                                ID="btnFind"
                                                CommandName="Consultar"
                                                tooltip="Consultar Actuación"
                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                runat="server"
                                                ImageUrl="../Images/img_gridbuscar.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:TemplateField>                                    
                                    <asp:BoundField DataField="ofco_sOficinaConsularId" HeaderText="ofco_sOficinaConsularId" ItemStyle-CssClass="ColumnaOculta">
                                        <HeaderStyle CssClass="ColumnaOculta" />
                                        <ItemStyle CssClass="ColumnaOculta" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Flujo General" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton
                                                ID="btnIrGeneral"
                                                CommandName="IrGeneral"
                                                tooltip="Editar Acto general"
                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                runat="server"
                                                ImageUrl="../Images/img_title.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </td>
                        </tr>
                    </table>
                                            <asp:HiddenField ID="hCorrelativo" runat="server" />

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="tab-1">
                    </div>
                    <div id="tab-2">
                    </div>
                    <div id="tab-3">
                    </div>
                    <div id="tab-4">
                    </div>
                    <div id="tab-5">
                    </div>
                    <div id="tab-6">
                    </div>

                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
</asp:Content>
