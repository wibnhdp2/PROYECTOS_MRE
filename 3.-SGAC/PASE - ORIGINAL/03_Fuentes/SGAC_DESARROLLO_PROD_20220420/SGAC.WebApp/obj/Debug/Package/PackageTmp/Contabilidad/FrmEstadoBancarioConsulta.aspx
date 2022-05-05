<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmEstadoBancarioConsulta.aspx.cs"
    Inherits="SGAC.WebApp.Contabilidad.EstadoBancarioConsulta" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagName="ToolBar"
    TagPrefix="ToolBar" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlSubirExcel.ascx" TagName="ctrlSubirExcel"
    TagPrefix="uc2" %>
<%--<%@ Register src="../Accesorios/SharedControls/ctrlDecimal.ascx" tagname="ctrlDecimal" tagprefix="uc3" %>--%>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    
    
    <%--<script src="../Scripts/jquery.mask.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script src="<%= ResolveUrl("~/signalr/hubs") %>" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        function pageLoad() {
            $("#<%=txtMontoMant.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtFondoMontoRREE.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtMontoLima.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtFondoMontoElec.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtMontoMilitar.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtMontoNuevo.ClientID %>").on("keydown", validarDecimal);
            //$("#<%=txtMontoMant.ClientID %>").mask('000,000,000.00', { reverse: true });

        };
     
        

        function notificar(msg, user_receive) {
            var proxy = $.connection.myHub;
            proxy.server.sendNotifications(msg, user_receive);
            $.connection.hub.start();
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function ValidarRegistro() {
            var bolValida = true;

            var strBancoMant = $.trim($("#<%= ddlBancoMant.ClientID %>").val());
            var strCuenta = $.trim($("#<%= ddlCuentaMant.ClientID %>").val());
            var strOperacion = $.trim($("#<%= txtOperacionNro.ClientID %>").val());
            var strMonto = $.trim($("#<%= txtMontoMant.ClientID %>").val());
            var strRepresentante = $.trim($("#<%= txtRepresentanteMant.ClientID %>").val());

            document.getElementById("<%=ddlMes.ClientID %>").style.border = "1px solid #888888";
            document.getElementById("<%=ddlAnio.ClientID %>").style.border = "1px solid #888888";

            var ddlanio = document.getElementById("<%=ddlAnio.ClientID %>");
            var anio = ddlanio.options[ddlanio.selectedIndex].text.toUpperCase();

            var ddlperiodo = document.getElementById("<%=ddlMes.ClientID %>");
            var periodoUsuario = ddlperiodo.options[ddlperiodo.selectedIndex].text.toUpperCase();



            var meses = new Array("ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SET", "OCT", "NOV", "DIC");
            var f = new Date();
            var fechaSistema = meses[f.getMonth()] + "-01-" + f.getFullYear();
            var fecha = periodoUsuario + '-01-' + anio;

            if (anio > f.getFullYear()) {
                document.getElementById("<%=ddlMes.ClientID %>").style.border = "1px solid Red";
                document.getElementById("<%=ddlAnio.ClientID %>").style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                if (anio < f.getFullYear()) {
                    document.getElementById("<%=ddlMes.ClientID %>").style.border = "1px solid #888888";
                    document.getElementById("<%=ddlAnio.ClientID %>").style.border = "1px solid #888888";
                }
                else {
                    if (f.getMonth() < ddlperiodo.selectedIndex) {
                        document.getElementById("<%=ddlMes.ClientID %>").style.border = "1px solid Red";
                        document.getElementById("<%=ddlAnio.ClientID %>").style.border = "1px solid Red";
                        bolValida = false;
                    }

                }

            }



            var ddlBancoMant = document.getElementById('<%= ddlBancoMant.ClientID %>');
            var ddlCuentaMant = document.getElementById('<%= ddlCuentaMant.ClientID %>');
            var txtOperacionNro = document.getElementById('<%= txtOperacionNro.ClientID %>');
            var txtMontoMant = document.getElementById('<%= txtMontoMant.ClientID %>');
            var txtRepresentanteMant = document.getElementById('<%= txtRepresentanteMant.ClientID %>');
            var ddlOfcDependiente = document.getElementById('<%=ddlOfcDependiente.ClientID %>');
            var ddlTransaccionTipo = document.getElementById('<%=ddlTransaccionTipo.ClientID %>');


            if (ddlTransaccionTipo.value == "5304") {
                if (ddlOfcDependiente.value == "0") {
                    ddlOfcDependiente.style.border = "1px solid Red";
                    bolValida = false;
                }
                else { ddlOfcDependiente.style.border = "1px solid #888888"; }
            }

            if (strBancoMant == "0") {
                ddlBancoMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlBancoMant.style.border = "1px solid #888888";
            }

            if (strCuenta == "0") {
                ddlCuentaMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlCuentaMant.style.border = "1px solid #888888";
            }


            if (strMonto.length == 0) {
                txtMontoMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtMontoMant.style.border = "1px solid #888888";
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                //bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }

        function soloNumeros(e) {
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
                if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105))) {
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
        };

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
   
    <style type="text/css">
        .style1
        {
            height: 59px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function seleccionar_todo() {
            if (document.getElementById('<%=gdvExcel.ClientID%>') != null) {

                var x = document.getElementById('<%=gdvExcel.ClientID%>').querySelectorAll("input");
                var i;
                var cnt = 0;
                for (i = 0; i < x.length; i++) {
                    if (x[i].type == "checkbox") {
                        if (x[i].disabled == false) {
                            x[i].checked = document.getElementById('chkMarcarTodos').checked;
                            if (x[i].checked) {
                                cnt++;
                            }
                        }
                    }
                }
                if (cnt == 0) {
                    document.getElementById('chkMarcarTodos').checked = 0;
                    document.getElementById('<%=btnGrabarExcel.ClientID%>').disabled = true;
                }
                else {
                    document.getElementById('<%=btnGrabarExcel.ClientID%>').disabled = false;
                }
                document.getElementById('<%=lblSeleccionados.ClientID%>').innerHTML = "Seleccionados: " + cnt;
            }
        }
        function estadoBotonGrabar() {
            var x = document.getElementById('<%=gdvExcel.ClientID%>').querySelectorAll("input");
            var i;
            var cnt = 0;
            for (i = 0; i < x.length; i++) {
                if (x[i].type == "checkbox" && x[i].checked) {
                    cnt++;
                }
            }
            if (cnt > 0) {
                document.getElementById('<%=btnGrabarExcel.ClientID%>').disabled = false;
                if (x.length == cnt) {
                    document.getElementById('chkMarcarTodos').checked = 1;
                }
                else {
                    document.getElementById('chkMarcarTodos').checked = 0;
                }
                document.getElementById('<%=lblSeleccionados.ClientID%>').innerHTML = "Seleccionados: " + cnt;
            }
            else {
                document.getElementById('<%=btnGrabarExcel.ClientID%>').disabled = true;
                document.getElementById('chkMarcarTodos').checked = 0;
                document.getElementById('<%=lblSeleccionados.ClientID%>').innerHTML = "Seleccionados: " + "0";
            }
        }

        function Popup() {
            document.getElementById('simpleModal_1').style.display = 'block';
            document.getElementById('<%=btnGrabarExcel.ClientID%>').disabled = true;
        }
        function cerrarPopup() {
            document.getElementById('simpleModal_1').style.display = 'none';
        }
        function cerrarPopupEspera() {
            document.getElementById('modalEspera').style.display = 'none';
        }
        function abrirPopupEspera() {
            document.getElementById('modalEspera').style.display = 'block';
        }
        function PopupConciliacionParte(valorInsumo, montoConciliado) {
            var myHidden = document.getElementById('<%= hTransaccion.ClientID %>');
            var montoNuevo = document.getElementById('<%= txtMontoNuevo.ClientID %>');
            var MontoActual = document.getElementById('<%= txtMontoActual.ClientID %>'); 
            if (myHidden)//checking whether it is found on DOM, but not necessary
            {
                myHidden.value = valorInsumo;
                MontoActual.value = parseFloat(montoConciliado).toFixed(2);
                montoNuevo.value = "";
                $("#<%= txtMontoNuevo.ClientID %>").prop("disabled", false); 
            }
            document.getElementById('modalTransaccion').style.display = 'block';
        }

        function PopupConciliacion(valorInsumo, montoConciliado) {
            var myHidden = document.getElementById('<%= hTransaccion.ClientID %>');
            var montoNuevo = document.getElementById('<%= txtMontoNuevo.ClientID %>');
            var MontoActual = document.getElementById('<%= txtMontoActual.ClientID %>'); 
            if (myHidden)//checking whether it is found on DOM, but not necessary
            {
                myHidden.value = valorInsumo;
                MontoActual.value = parseFloat(montoConciliado).toFixed(2);
                $("#<%= txtMontoNuevo.ClientID %>").prop("disabled", true);
            } 
            document.getElementById('modalTransaccion').style.display = 'block';
        }
        function cerrarPopupConciliacion() {
            document.getElementById('modalTransaccion').style.display = 'none';
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

            var letras = "áéíóúñÑ";
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

        function MensajeBancario(mostrarMensaje) {
            if (mostrarMensaje == 1) {
                $("#<%= lblMensajeBanco.ClientID %>").hide();
                $("#<%= linkCuentaBancaria.ClientID %>").hide();
            }
            else {
                $("#<%= lblMensajeBanco.ClientID %>").show();
                $("#<%= linkCuentaBancaria.ClientID %>").show();
            }
        }

    </script>
    <div style="position: fixed; bottom: -2px; right: 0; border: 1px solid #550e27; background-color: #550e27;
        padding: 5px 5px 5px 5px;">
        <asp:LinkButton ID="lkbDescargar" runat="server" CssClass="btn btn-primary" OnClick="lkbDescargar_Click"
            ForeColor="White">Descargar Plantilla <i class="glyphicon glyphicon-save"></i></asp:LinkButton>
    </div>
    <div id="simpleModal_1" class="modal">
        <div class="modal-windowlarge" style="min-height: 30%; max-height: 100%;">
            <div class="modal-titulo" style="margin: 0px 0px 15px 0px;">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopup(); return false" runat="server" />
                <span>CARGAR ARCHIVO EXCEL</span>
            </div>
            <div class="modal-cuerpoNormal" style="border: 1px #dcdcdc solid; padding: 5px 5px 0px 5px;
                margin-left: 10px; margin-right: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Cta. Bancaria:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBancoMantPopup" runat="server" Width="220px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlBancoMantPopup_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Periodo:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMesPopup" runat="server" Width="60px" />
                            <asp:DropDownList ID="ddlAnioPopup" runat="server" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Nro. Cuenta:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCuentaMantPopup" runat="server" Width="220px">
                            </asp:DropDownList>
                            <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                         
                        </td>
                        <td>
                           
                        </td>
                    </tr>
                   
                </table>
                <br />
                <hr />
                <table>
                    <tr>
                        <td style="width: 63%">
                            <uc2:ctrlSubirExcel ID="ctrlSubirExcel1" runat="server" />
                        </td>
                        <td align="right" valign="top">
                            <asp:Button ID="btnGrabarExcel" CssClass="btnSave" Width="170px" runat="server" Text="     Grabar Transacciones"
                                OnClick="btnGrabarExcel_Click" Enabled="false" />
                            <asp:Button ID="btnLimpiarExcel" CssClass="btnLimpiar" Width="100px" runat="server"
                                Text="     Limpiar" OnClick="btnLimpiarExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <Label:Validation ID="ctrlValidacionRegistro" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal-pie" style="margin-bottom: 30px; min-height: 30%; max-height: 100%;
                display: inline-block;">
                <div style="padding: 0px 10px 10px 10px; background: #ffffff;">
                    <div style="text-align: left; margin-top: 10px;">
                        <asp:Label ID="lblCantidad" runat="server" ForeColor="Black" Text=""></asp:Label><br />
                        <asp:Label ID="lblSeleccionados" runat="server" ForeColor="Black" Text=""></asp:Label><br />
                        <input id="chkMarcarTodos" type="checkbox" style="cursor: pointer" onclick="seleccionar_todo()" />
                        <asp:Label ID="Label9" runat="server" ForeColor="Black" Text="Marcar todos"></asp:Label>
                    </div>
                    <asp:GridView ID="gdvExcel" runat="server" AutoGenerateColumns="False" CssClass="mGrid" Width="100%"
                        GridLines="None" PageSize="9000" SelectedRowStyle-CssClass="slt" OnRowDataBound="gdvExcel_RowDataBound">
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelItem" runat="server" onclick="estadoBotonGrabar()" Style="cursor: pointer" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="tran_dFechaOperacion" HeaderText="Fecha Transacción">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>                            
                            <asp:BoundField DataField="tran_TipoOperacionId" HeaderText="TipoOperacionId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" >
                                <HeaderStyle CssClass="ColumnaOculta" />
                                <ItemStyle CssClass="ColumnaOculta" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tran_vTipoOperacion" HeaderText="Tipo Operación">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tran_TipoTransaccionId" HeaderText="TipoTransaccionId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta" >
                                <HeaderStyle CssClass="ColumnaOculta" />
                                <ItemStyle CssClass="ColumnaOculta" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tran_vTipoTransaccion" HeaderText="Tipo Transacción">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tran_FMonto" HeaderText="Monto">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tran_ERROR" HeaderText="OBSERVACIÓN">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="slt" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div id="modalTransaccion" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCloseTra" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopupConciliacion();return false" runat="server" />
                <span>CONCILIACIÓN</span>
            </div>
            <div class="modal-cuerpo" style="height:230px; text-align:left;">
                    <table width="90%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                    border-left: 1px solid gray; border-right: 1px solid gray; margin-top:10px; margin-left:10px; margin-right:5px;" >
                    <tr>
                        <td>
                            INGRESE FECHA DE CONCILIACIÓN:
                        </td>
                        <td>
                            <SGAC_Fecha:ctrlDate ID="CtrFechaConciliacionPop" runat="server" />
                        </td>
                    </tr>
                     <tr>
                    <td>
                            MONTO ACTUAL:
                        </td>
                        <td>
                            <asp:TextBox ID="txtMontoActual" Width="150px" Enabled = "false" CssClass="campoNumero" Text="0.000" MaxLength="15" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td>
                            INGRESE NUEVO MONTO:
                        </td>
                        <td>
                            <asp:TextBox ID="txtMontoNuevo" Width="150px" CssClass="campoNumero" Text="0.000" MaxLength="15" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hTransaccion" runat="server" />
            </div>
            <div class="modal-pie">
                <asp:Button ID="BtnConciliar" CssClass="btnLogin" runat="server" Text="Conciliar" OnClick="BtnConciliar_Click" />
                <button class="btnLogin" onclick="cerrarPopupConciliacion(); return false">
                    Cancelar</button>
            </div>
        </div>
    </div>
    <%--Opciones--%>
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloEstadoBancario" runat="server" Text="Estado Bancario"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <%--Opciones--%>
    <table width="90%" align="center">
        <tr>
            <td align="left">
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                        <li><a href="#tab-2">
                            <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div>
                                    <asp:UpdatePanel ID="updConsultaFiltro" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMensajeBanco" runat="server" CssClass="hideControl" ForeColor="Red"
                                                            Text="Para visualizar los estados debe registrar una " />
                                                        <asp:LinkButton ID="linkCuentaBancaria" runat="server" PostBackUrl="~/Configuracion/FrmCuentaCorriente.aspx"
                                                            CssClass="hideControl" Font-Bold="True" Font-Size="9pt" ForeColor="Red">Cuenta Bancaria</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td width="150px">
                                                        <asp:Label ID="lblOfcConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" 
                                                            AutoPostBack="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <asp:Label ID="lblBancoConsulta" runat="server" Text="Cta. Bancaria:"></asp:Label>
                                                    </td>
                                                    <td width="300px">
                                                        <asp:DropDownList ID="ddlBancoConsulta" runat="server" Width="250px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlBancoConsulta_SelectedIndexChanged" OnDataBound="ddlBancoConsulta_DataBound">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="120px">
                                                        <asp:Label ID="lblNroCuenta" runat="server" Text="N° Cuenta:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCuentaCorrienteConsulta" runat="server" Width="200px" OnSelectedIndexChanged="ddlCuentaCorrienteConsulta_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <asp:Label ID="lblRepresentante" runat="server" Text="Representante:"></asp:Label>
                                                    </td>
                                                    <td width="300px">
                                                        <asp:TextBox ID="txtRepresentanteConsulta" runat="server" Width="246px" Enabled="False"></asp:TextBox>
                                                    </td>
                                                    <td width="120px">
                                                        <asp:Label ID="lblTipoMoneda" runat="server" Text="Tipo Moneda:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMonedaTipoConsulta" runat="server" Width="200px" Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td width="150px">
                                               <%-- <asp:Label ID="txtTipoBusqueda" runat="server" Text="Buscar Por:"></asp:Label>--%>
                                               <asp:Label ID="Label10" runat="server" Text="Periodo:"></asp:Label>
                                                </td>
                                                <td>
                                                   <%-- <asp:RadioButton ID="rbFechaTransaccion" Text = "Fecha de Transacción" 
                                                        runat="server" GroupName="Fecha" Checked="True" AutoPostBack="True" 
                                                        oncheckedchanged="rbFechaTransaccion_CheckedChanged" /><br />
                                                    <asp:RadioButton ID="rbPeriodo" Text="Periodo" runat="server" GroupName="Fecha" 
                                                        AutoPostBack="True" oncheckedchanged="rbPeriodo_CheckedChanged" />--%>
                                                         <asp:DropDownList ID="ddlAnioBusqueda" runat="server" Width="60px" />
                                                            <asp:DropDownList ID="ddlMesBusqueda" runat="server" Width="60px" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNroOperacionBusq" runat="server" Text="Nro. Operación:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNroOperacionBusq" runat="server" Width="192px"></asp:TextBox>
                                                </td>
                                                </tr>
                                               <%-- <div runat="server" id="DivFecha">
                                                    <tr>
                                                        <td width="150px">
                                                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                                        </td>
                                                        <td width="300px">
                                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                                        </td>
                                                        <td width="120px">
                                                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                                        </td>
                                                    </tr>
                                                </div>--%>
                                                <%--<div runat="server" id="divPeriodo" visible="true">
                                                    <tr>
                                                        <td width="150px">
                                                            
                                                        </td>
                                                        <td>
                                                           
                                                        </td>
                                                    </tr>
                                                </div>--%>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div>
                                    <table>
                                        <tr>
                                            <td align="left" style="width: 72%">
                                                <ToolBar:ToolBar ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBar>
                                            </td>
                                            <td align="right">
                                                <asp:Button runat="server" ID="btnMasivo" Text="     Carga Masiva" Width="120px"
                                                    CssClass="btnExcel" OnClientClick="Popup();" />
                                            </td>
                                            <td align="right">
                                              <%--  <asp:Button runat="server" ID="btnConciliacionBancaria" Text="Conciliación" Width="100px"
                                                    CssClass="btnGeneral" Visible="false" OnClick="btnConciliacionBancaria_Click" />--%>
                                            </td>
                                            <td align="right">
                                                <asp:Button runat="server" ID="btnResumenBancos" Text="Resumen" Width="100px" CssClass="btnGeneral"
                                                    Enabled="false" OnClick="btnResumenBancos_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <Label:Validation ID="ctrlValidacion" runat="server" />
                                </div>
                                <br />
                                <div align="left">
                                    <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gdvEstadoBancario" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                OnRowCommand="gdvEstadoBancario_RowCommand" Width="100%" OnRowDataBound="gdvEstadoBancario_RowDataBound">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="tran_dFechaOperacion" HeaderText="Fecha">
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_vNumeroOperacion" HeaderText="Nro. Operación">
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="periodo" HeaderText="Periodo">
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="periodoAnteriorCompleto" HeaderText="Periodo Pendiente">
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="tran_vTipo" HeaderText="Tipo Transacción">
                                                        <HeaderStyle Width="200px" />
                                                        <ItemStyle Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OficinaDependiente" HeaderText="Oficina Dependiente" />
                                                    <asp:BoundField DataField="tran_fIngreso" HeaderText="Ingresos" DataFormatString="{0:N}">
                                                        <HeaderStyle Width="80px" />
                                                        
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_fSalida" HeaderText="Egresos" DataFormatString="{0:N}">
                                                        
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_FSaldo" HeaderText="Saldo" DataFormatString="{0:N}">
                                                        
                                                        <HeaderStyle Width="100px"   CssClass="ColumnaOculta"/>
                                                        <ItemStyle HorizontalAlign="Right" Width="100px"  CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <%-- Posición: 6 --%>
                                                    <asp:BoundField DataField="tran_sTipoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnFind" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="tran_cPeriodoAnterior" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="esConciliacion" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_sEstadoDepositoConciliacion" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_dFechaConciliacion" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField> 
                                                    <asp:BoundField DataField="tran_iTransaccionId" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>             
                                                                                             
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div>
                                    <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:HiddenField ID="hTransaccionEditar" runat="server" />
                        <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <ToolBar:ToolBar ID="ctrlToolBarMantenimiento" runat="server" />
                                <table border="0" style="width: 90%; border-style: solid; border-width: 1px; border-color: #000000;
                                    margin-top: 10px;">
                                    <tr>
                                        <td align="left" colspan="1" style="width: 100px;">
                                            <asp:Label ID="lblPeriodo" runat="server" Text="Mes de Recaudación:"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:DropDownList ID="ddlMes" runat="server" Width="60px" />
                                            <asp:DropDownList ID="ddlAnio" runat="server" Width="60px" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 90%; border-style: solid; border-width: 1px; border-color: #000000;
                                    margin-top: 5px;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                CssClass="hideControl" ForeColor="Red" Font-Size="14px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBancoMant" runat="server" Text="Cta. Bancaria:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBancoMant" runat="server" Width="180px" OnSelectedIndexChanged="ddlBancoMant_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblVal_ddlBancoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNroCuentaMant" runat="server" Text="Nro. Cuenta:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCuentaMant" runat="server" Width="220px" OnSelectedIndexChanged="ddlCuentaMant_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblVal_ddlCuentaMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRepresentanteMant" runat="server" Text="Representante:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRepresentanteMant" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMonedaTipoMant" runat="server" Text="Tipo Moneda:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonedaTipoMantenimiento" runat="server" Width="220px" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 90%; border-style: solid; border-width: 1px; border-color: #000000;
                                    margin-top: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFechaTransaccion" runat="server" Text="Fecha Transacción:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFechaTransaccion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTipoOperacion" runat="server" Text="Tipo Operación:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOperacionTipo" runat="server" Width="150px" OnSelectedIndexChanged="ddlOperacionTipo_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkOperacionesPend" runat="server" Text="Operaciones Pendientes (Concilación)"
                                                Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkOperacionesPend_CheckedChanged" />
                                            <asp:HiddenField ID="hOperacionTipo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTipoTransaccion" runat="server" Text="Tipo Transacción:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTransaccionTipo" runat="server" Width="380px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlTransaccionTipo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="panel" runat="server" visible="false" style="border-width: 1px; border-style: groove;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblOfcDependiente" runat="server" Text="Oficina Dependiente:"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="ddlOfcDependiente" runat="server" Width="320px">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblValidaOfcDependiente" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFondoMontoRREE" runat="server" Text="Monto Fondo RREE:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFondoMontoRREE" runat="server" Width="90px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblFondoMontoElec" runat="server" Text="Monto Fondo Electoral:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFondoMontoElec" runat="server" Width="90px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMontoLima" runat="server" Text="Monto de trámites Pagado en Lima:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMontoLima" runat="server" Width="90px"></asp:TextBox>
                                                            <asp:Label ID="dolar" runat="server" Text="USD" Font-Bold="True"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMontoMilitar" runat="server" Text="Monto Militar:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMontoMilitar" runat="server" Width="90px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMontoRetencion" runat="server" Text="Retención Art.10:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMontoRetencion" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPeriodoorigen" runat="server" Text="Corresponde a otro mes?"></asp:Label>
                                            <asp:CheckBox ID="chkOtroMes" runat="server" OnCheckedChanged="chkOtroMes_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMesTran" runat="server" Width="60px" Visible="False" />
                                            <asp:DropDownList ID="ddlAnioTran" runat="server" Width="60px" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNroOperacion" runat="server" Text="N° Operación:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOperacionNro" CssClass="txtLetra" runat="server" Width="150px"
                                                MaxLength="20"></asp:TextBox>
                                            <asp:Label ID="lblVal_txtOperacionNro" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMonto" runat="server" Text="Monto:"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="width: 100%">
                                                <div style="float: left;">
                                                    <asp:TextBox ID="txtMontoMant" runat="server" Width="150px" CssClass="campoNumero"
                                                        MaxLength="15"></asp:TextBox>
                                                   <%-- <uc3:ctrlDecimal ID="txtMontoMant"  Width="150px" runat="server" />--%>
                                                    
                                                    <asp:HiddenField ID="hMontoMant" runat="server" />
                                                    <asp:Label ID="lblVal_txtMontoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </div>
                                                <div style="float: right; margin-right: 30%;" id="EstadoOcultar" runat="server" visible="false">
                                                    <asp:Label ID="lblEstadoDep" runat="server" Text="Estado Deposito:"></asp:Label>
                                                    <asp:DropDownList ID="ddlEstadoDep" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEstadoDep_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <div id="OcultarFechaConciliacion" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblFechaConciliacion" runat="server" Text="Fecha Conciliación:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="ctrFechaConciliacion" runat="server" />
                                            </td>
                                        </div>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lblSaldo" runat="server" Text="Saldo:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSaldo" runat="server" CssClass="campoNumero" Width="150px" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hSaldo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Saldo Total:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSaldoTotal" runat="server" CssClass="campoNumero" Width="150px"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblObservacion" runat="server" Text="Observación:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObservacion" runat="server" Width="450px" CssClass="txtLetra"
                                                onkeypress="return isLetraNumero(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblCamposObligatorios" runat="server" Text="(*) Campos Obligatorios"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="width: 90%; border-style: solid; border-width: 1px; border-color: #000000;
                                    margin-top: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblConciliaciones" runat="server" Font-Bold="true" Text="Conciliaciones Pendientes:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grvConciliacionPendientes" runat="server" AutoGenerateColumns="False"
                                                CssClass="mGrid" GridLines="None" PageSize="9000" SelectedRowStyle-CssClass="slt">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="NRO.">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_MesRecaudacion" HeaderText="Mes de Recaudación">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_dFechaOperacion" HeaderText="Fecha Transacción">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="para_vDescripcion" HeaderText="Tipo Operación">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_fIngreso" HeaderText="Ingreso" DataFormatString="{0:N}">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tran_fSalida" HeaderText="Egreso" DataFormatString="{0:N}">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="estadoConciliacion" HeaderText="Estado Deposito">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Conciliar Total" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnConciliar" runat="server" OnClick="Conciliar" Transaccion='<%# Eval("tran_iTransaccionId")%>' MontoConciliar='<%# Eval("tran_fSalida")%>'
                                                                ImageUrl="../Images/img_16_tramite_aprobar.png" ToolTip="Conciliar Total" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Conciliar en parte" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnConciliarParte" runat="server" OnClick="ConciliarParte" Transaccion='<%# Eval("tran_iTransaccionId")%>' MontoConciliar='<%# Eval("tran_fSalida")%>'
                                                                ImageUrl="../Images/img_16_accounting.png" ToolTip="Conciliar en parte" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div id="modalEspera" class="modal">
                                        <div class="modal-window">
                                            <div class="modal-titulo">
                                                <span>CARGANDO...</span>
                                            </div>
                                            <div class="modal-cuerpo">
                                                <br />
                                                <h3 style="font-size: small">
                                                    Cargando la información. Por favor espere...</h3>
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
</asp:Content>
