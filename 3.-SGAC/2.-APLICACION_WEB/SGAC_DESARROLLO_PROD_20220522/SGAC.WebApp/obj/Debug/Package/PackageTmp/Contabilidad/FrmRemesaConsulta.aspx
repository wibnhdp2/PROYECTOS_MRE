<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmRemesaConsulta.aspx.cs"
    Inherits="SGAC.WebApp.Contabilidad.RemesaConsulta" %> 

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ddlOficina" TagPrefix="ddlOficina" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function pageLoad() {
            $("#<%=txt_rede_actual_total.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txt_rede_otro_total.ClientID %>").on("keydown", validarDecimal);            
        }

        $(function () {
            $('#tabs').tabs();            
        });
        
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function ValidarRegistrarRemesa() {
            var bolValida = true;

            var intCantidad = $('#<%= ddlOficinaDestinoMant.FindControl("ddlOficinaConsular").ClientID %>')[0].childElementCount;
            if (intCantidad > 1) {
                var strOficinaConsularDestino = $('#<%= ddlOficinaDestinoMant.FindControl("ddlOficinaConsular").ClientID %>').val();
                var ddlOficinaConsularDestino = document.getElementById('<%= ddlOficinaDestinoMant.FindControl("ddlOficinaConsular").ClientID %>');
                if (strOficinaConsularDestino == "0") {
                    ddlOficinaConsularDestino.style.border = "1px solid Red";
                    bolValida = false;
                }
                else {
                    ddlOficinaConsularDestino.style.border = "1px solid #888888";
                }
            }
            var strValorMes = $.trim($("#<%= ddlRemesaMes.ClientID %>").val());
            var ddlMesRemesa = document.getElementById('<%= ddlRemesaMes.ClientID %>');
            if (strValorMes == "" || strValorMes == "0") {
                ddlMesRemesa.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlMesRemesa.style.border = "1px solid #888888";
            }
            var strTotal = $.trim($("#<%= txtRemesaTotal.ClientID %>").val());
            var txtRemesaTotal = document.getElementById('<%= txtRemesaTotal.ClientID %>');
            if (strTotal == "" || strTotal == "0") {
                txtRemesaTotal.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtRemesaTotal.style.border = "1px solid #888888";
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

        function ValidarDatosCalculo() {
            var bolValida = true;

            if (txtcontrolError(document.getElementById("<%= txt_rede_actual_total.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txt_rede_FTipoCambioBancario.ClientID %>")) == false) bolValida = false;
            return bolValida;
        }
        function ValidarDatosCalculo_OtroMes() {
            var bolValida = true;

            if (txtcontrolError(document.getElementById("<%= txt_rede_otro_total.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txt_rede_otro_FTipoCambioBancario.ClientID %>")) == false) bolValida = false;
            return bolValida;
        }

        function ValidarDetalleActual() {
            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%= ddl_rede_actual_tipo.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txt_rede_actual_total.ClientID %>")) == false) bolValida = false;            
            if (txtcontrolError(document.getElementById("<%= txt_rede_FTipoCambioBancario.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_rede_actual_banco.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_rede_actual_cuenta.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txt_rede_actual_voucher.ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txtResponsableEnvio.ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= datFechaDetalle.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txt_rede_actual_total_dolares.ClientID %>')) == false) bolValida = false;

            var strValorMes = $.trim($("#<%= ddlMesActual.ClientID %>").val());
            var ddlMesActual = document.getElementById('<%= ddlMesActual.ClientID %>');
            if (strValorMes == "" || strValorMes == "0") {
                ddlMesActual.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlMesActual.style.border = "1px solid #888888";
            }

            var strValorClasificacion = $.trim($("#<%= ddlClasificacion.ClientID %>").val());
            var ddlClasificacion = document.getElementById('<%= ddlClasificacion.ClientID %>');
            if (strValorClasificacion == "" || strValorClasificacion == "0") {
                ddlClasificacion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlClasificacion.style.border = "1px solid #888888";
            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacionOtroMes.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionOtroMes.ClientID %>").show();
            }
            return bolValida;
        }

        function ValidarDetalleOtroMes() {
            var bolValida = true;

            if (ddlcontrolError(document.getElementById("<%= ddlTipoRemesaOtroMes.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txt_rede_otro_total.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById("<%= txt_rede_otro_FTipoCambioBancario.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_rede_otro_banco.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_rede_otro_cuenta.ClientID %>")) == false) bolValida = false;
            if (ddlcontrolError(document.getElementById("<%= ddl_rede_otro_moneda.ClientID %>")) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txt_rede_otro_voucher.ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= datFechaOtroMes.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txt_rede_otro_total.ClientID %>')) == false) bolValida = false; 
            /*if (txtcontrolError(document.getElementById('<%= txtTitularDatos.ClientID %>')) == false) bolValida = false;*/

            if (bolValida) {
                $("#<%= lblValidacionOtroMes.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacionOtroMes.ClientID %>").show();
            }
            return bolValida;
        }

        function ValidarEnvioRemesa(e) {
            var bolValida = true;
                        

            if (bolValida) {
                $("#<%= lblValidacionEnvio.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de ENVIAR Remesa?");
            }
            else {
                $("#<%= lblValidacionEnvio.ClientID %>").show();
            }
            return bolValida;
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
                alert("Numero Incorrecto");
                control.value = "";
            }
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

        function ValidarLetras(evt) {
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
            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        };

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
    </script>
   
    <style type="text/css">
    .mensajePie
    {
        color:Black;
        font-family:@Arial Unicode MS;
        font-size:smaller;
        font-weight:bold; 
    }        
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }
    </script>
    <%--Consulta --%>
    <table class="mTblTituloM2" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloRemesaConsular" runat="server" Text="Remesas Consulares"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <%--Opciones--%>
    <table style="width: 95%;" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                        <li><a href="#tab-2">
                            <asp:Label ID="lblRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <%-- Grilla --%>
                                <table width="100%">
                                    <tr>
                                        <td width="130px">
                                            <asp:Label ID="lblOCOrigen" runat="server" Text="Of. Consular Origen:"></asp:Label>
                                        </td>
                                        <td>
                                            <ddlOficina:ddlOficina ID="ddlOficinaOrigenCons" runat="server" Width="560px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="130px">
                                            <asp:Label ID="lblOCDestino" runat="server" Text="Of. Consular Destino:"></asp:Label>
                                        </td>
                                        <td>
                                            <ddlOficina:ddlOficina ID="ddlOficinaDestinoCons" runat="server" Width="560px" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td width="130px">
                                            <asp:Label ID="lblRemesaEstado1" runat="server" Text="Estado Remesa:"></asp:Label>
                                        </td>
                                        <td width="220px">
                                            <asp:DropDownList ID="ddlRemesaEstadoConsulta" runat="server" Width="202px">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="130px">
                                            <asp:Label ID="lblRemesaTipo1" runat="server" Text="Tipo Remesa:" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRemesaTipoConsulta" runat="server" Width="200px" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="130px">
                                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td width="220px">
                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                        </td>
                                        <td width="130px">
                                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblMsjPendienteEnvio" runat="server" Text="Label" CssClass="mensajePie" ></asp:Label>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblMsjEnviados" runat="server" Text="Label" CssClass="mensajePie" ></asp:Label>       
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gdvRemesa" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt" Width="100%"
                                                        AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvRemesa_RowCommand"
                                                        
                                                        DataKeyNames="reme_sEstadoId,reme_sOficinaConsularOrigenId,reme_sOficinaConsularDestinoId" 
                                                        onrowdatabound="gdvRemesa_RowDataBound">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <%--Posición: 0 --%>
                                                            <asp:BoundField DataField="reme_cPeriodo" HeaderText="Periodo" >
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="60px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="reme_dFechaCreacion" HeaderText="Fecha Registro" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:BoundField>                                                                
                                                            <asp:BoundField DataField="reme_vSiglasOficinaConsularOrigen" HeaderText="Consulado">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:BoundField>                                                                                                                                                                                
                                                            <asp:BoundField DataField="reme_vCuentaCorriente" HeaderText="Nro. Cuenta" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="reme_fMonto" HeaderText="Monto Remesa" 
                                                                DataFormatString="{0:#.00000}">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="reme_dFechaEnvio" HeaderText="Fecha Envío" DataFormatString="{0:MMM-dd-yyyy}"
                                                                HtmlEncode="False" HtmlEncodeFormatString="False">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:BoundField>
                                                            <%--Posición: 5 --%>
                                                            <asp:BoundField DataField="reme_vTipo" HeaderText="Tipo" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>                                                            
                                                            <%--Posición: 6 - Validación de permiso --%>
                                                            <asp:BoundField DataField="reme_sEstadoId" HeaderText="reme_sEstadoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="reme_vEstado" HeaderText="Estado">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnFind" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <%--POsición: 10 - gdvRemesa_RowDataBound --%>
                                                            <asp:BoundField DataField="reme_sOficinaConsularOrigenId" HeaderText="reme_sOficinaConsularOrigenId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdn_ofco_sOficinaConsularId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_ofco_sReferenciaId" runat="server" Value="0" />

                                <asp:HiddenField ID="hEstadoRemesa" runat="server" />

                                <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                <div>
                                    <asp:Label ID="lblValidacionRegistro" runat="server" Text="Falta validar algunos campos o ingresar detalle."
                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblValidacionEnvio" runat="server" Text="Falta ingresar algunos campos"
                                        CssClass="hideControl" ForeColor="Red" Font-Size="14px"></asp:Label>
                                </div>
                                <table width="100%">
                                    <tr>
                                        <td><asp:Label ID="lblOCOrigenMant" runat="server" Text="O. C. Origen:"></asp:Label></td>
                                        <td colspan="3">
                                            <ddlOficina:ddlOficina ID="ddlOficinaOrigenMant" runat="server" Enabled="false" Width="315px" />
                                        </td>
                                        <td colspan="4">
                                            <table>
                                                <tr>
                                                    <td align="center"><asp:Label ID="lblOCDestinoMant" runat="server" Text="O. C. Destino:" Width="100px"></asp:Label></td>
                                                    <td>
                                                        <ddlOficina:ddlOficina ID="ddlOficinaDestinoMant" runat="server" Width="320px" />
                                                        <asp:Label ID="lblCO_ddlOficinaDestinoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="90px">
                                            <asp:Label ID="lblAnioMesRemesa" runat="server" Text="Periodo:"></asp:Label>
                                        </td>
                                        <td colspan="6">
                                            <table>
                                                <tr>
                                                    <td style="width: 135px">
                                                        <asp:DropDownList ID="ddlRemesaAnio" runat="server" Width="60px"></asp:DropDownList>
                                                        <asp:DropDownList ID="ddlRemesaMes" runat="server" Width="60px"></asp:DropDownList>
                                                        <asp:Label ID="lblMesRemesa" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td><asp:Label ID="lblFechaEnvio" runat="server" Text="Fecha Envío:"></asp:Label></td>
                                                    <td><SGAC_Fecha:ctrlDate ID="datFechaEnvio" runat="server" /></td>
                                                    <td style="width:10px"></td>
                                                    <td><asp:Label ID="Label17" runat="server" Text="Total (Moneda Local):"  Width="90px"></asp:Label></td>                                                    
                                                    <td>
                                                        <asp:TextBox ID="txtRemesaTotal" runat="server" Width="120px" MaxLength="20" CssClass="campoNumero"                                            
                                                            Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td><asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                    <td><asp:Label ID="lblRemesaTotal" runat="server" Text="Total (Dólares):" Width="70px"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemesaTotalDolares" runat="server" Width="120px" MaxLength="20" CssClass="campoNumero"
                                                            Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>                                        
                                        <td><asp:Label ID="lblRemesaEstadoMant" runat="server" Text="Estado:"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="ddlRemesaEstadoMant" runat="server" Width="180px" 
                                                Enabled="False" Height="16px"></asp:DropDownList>
                                        </td>
                                        
                                    </tr> 
                                    <tr>
                                    
                                        <td>
                                            <asp:Label ID="lblObservacion" runat="server" Text="Observación:"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtObservacion" runat="server" Width="92%" MaxLength="200" TextMode="MultiLine"
                                                onBlur="conMayusculas(this)" CssClass="txtLetra" Enabled="False" 
                                                Height="50px"></asp:TextBox>
                                            <asp:Label ID="lblObserv" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                        </td>
                                    </tr>                                   
                                </table>
                                <asp:UpdatePanel ID="updRemesaDetalle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAgregarDetalle" runat="server" Text="Registrar Detalle" Width="200px"
                                                        Height="20px" BorderColor="#993333" BackColor="#ededed" BorderStyle="Groove"
                                                        BorderWidth="1px" Font-Bold="false" Font-Italic="False" Font-Names="Arial" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" ForeColor="#990033" 
                                                        OnClick="btnAgregarDetalle_Click" Visible="False" />
                                                    <asp:Button ID="btnRemesaOtroMes" runat="server" Text="Registrar Detalle Otro Mes"
                                                        Width="200px" Height="20px" BorderColor="#993333" BackColor="#ededed" BorderStyle="Groove"
                                                        BorderWidth="1px" Font-Bold="false" Font-Italic="False" Font-Names="Arial" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" ForeColor="#990033" 
                                                        OnClick="btnRemesaOtroMes_Click" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField runat="server" Value="0" ID="hdn_indice_sel" />
                                                    <asp:HiddenField runat="server" Value="0" ID="hdn_accion_sel" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tDetalle" runat="server" border="0" style="border-style: solid; border-width: 1px;
                                            border-color: #800000; width: 100%">
                                            <tr>
                                                <td bgcolor="#4E102E" colspan="6">
                                                    <asp:Label ID="lblOrigen" runat="server" Font-Bold="True" Font-Names="Arial"
                                                        Font-Size="10pt" ForeColor="White" Text="REMESA PERIODO ACTUAL"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblValidacionDetalle" runat="server" Text="Falta validar algunos campos."
                                                        CssClass="hideControl" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblMesAnio" runat="server" Text="Periodo:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAnioActual" runat="server" Width="60px"></asp:DropDownList>
                                                    <asp:DropDownList ID="ddlMesActual" runat="server" Width="60px" ></asp:DropDownList>
                                                    <asp:Label ID="lblMesActual" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td><asp:Label ID="lblDetTipoMant" runat="server" Text="Tipo Remesa:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_rede_actual_tipo" runat="server" Width="180px"></asp:DropDownList>
                                                    <asp:Label ID="lblVal_ddlRemesaTipoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td><asp:Label ID="Label19" runat="server" Text="Clasificación Remesa:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlClasificacion" runat="server" Width="180px"></asp:DropDownList>
                                                    <asp:Label ID="Label20" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblTCConsular" runat="server" Text="T.C. Consular:"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_FTipoCambioConsular" runat="server" Enabled="False" CssClass="campoNumero" Width="150px"></asp:TextBox>
                                                </td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:Label ID="Label15" runat="server" Text="Moneda:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:DropDownList ID="ddl_rede_actual_moneda_local" runat="server" Width="183px" Enabled="false"></asp:DropDownList>
                                                </td>      
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:Label ID="Label4" runat="server" Text="Moneda:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:DropDownList ID="ddl_rede_actual_Moneda" runat="server" Width="180px" 
                                                        Enabled="False"></asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddl_rede_actual_Moneda" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>      
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblTCBancario" runat="server" Text="T.C. Bancario:"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_FTipoCambioBancario" runat="server" Enabled="true" CssClass="campoNumero"
                                                        onkeypress="return isDecimalKey(this,event)" onBlur="validanumeroLostFocus(this)"
                                                        Width="150px"></asp:TextBox>
                                                    <asp:Label ID="lblCO_rede_FTipoCambioBancario" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:Label ID="lblMontoMant" runat="server" Text="Total:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:TextBox ID="txt_rede_actual_total" runat="server" Width="180px" MaxLength="10" CssClass="campoNumero"
                                                        onkeypress="return isDecimalKey(this,event)" onBlur="validanumeroLostFocus(this)"></asp:TextBox>
                                                    <asp:Label ID="lblVal_txtMontoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:Label ID="Label16" runat="server" Text="Total:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:TextBox ID="txt_rede_actual_total_dolares" runat="server" Width="170px" 
                                                        MaxLength="20" CssClass="campoNumero" Enabled="false"></asp:TextBox>
                                                    <asp:Button ID="btnCalcActual" runat="server" CssClass="btnCalcular"
                                                        onclick="btnCalcActual_Click" />
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFechaDeposito" runat="server" Text="Fecha Depósito:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="datFechaDetalle" runat="server" />
                                                </td>
                                                <td><asp:Label ID="Label3" runat="server" Text="Banco:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_rede_actual_banco" runat="server" Width="180px" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddl_rede_actual_banco_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddl_banco" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td><asp:Label ID="lbl_RemesaDetalle_Cuenta" runat="server" Text="N° Cuenta:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_rede_actual_cuenta" runat="server" Width="180px"></asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddl_RemesaDetalle_Cuenta" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="Label18" runat="server" Text="Observación:"></asp:Label></td>
                                                <td colspan="5">
                                                    <asp:TextBox ID="txtObsDetalle"  runat="server" CssClass="txtLetra" TextMode="MultiLine" Width="92%" MaxLength="200" Height="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="Label6" runat="server" Text="N° Voucher/Cheque:"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_actual_voucher" runat="server" CssClass="campoNumero" Width="150px" MaxLength="20"></asp:TextBox>
                                                    <asp:Label ID="lblCO_txt_rede_actual_voucher" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 120px">
                                                    <asp:Label ID="lblResponsableEnvio" runat="server" Text="Responsable Envío:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtResponsableEnvio" runat="server" Width="400px" MaxLength="100"
                                                        onkeypress="return ValidarLetras(event)" CssClass="txtLetra" ></asp:TextBox>
                                                    <asp:Label ID="lblCO_txtResponsableEnvio" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Button ID="btnAceptarDetalle" runat="server" Text="Aceptar" CssClass="btnGeneral"
                                                        OnClick="btnAceptarDetalle_Click" OnClientClick="return ValidarDetalleActual()" />
                                                    <asp:Button ID="btnCancelarDetalle" runat="server" Text="Cancelar" CssClass="btnGeneral"
                                                        OnClick="btnCancelarDetalle_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tRemesaOtroMes" runat="server" border="0" style="border-style: solid;
                                            border-width: 1px; border-color: #800000; width: 100%">
                                            <tr>
                                                <td bgcolor="#4E102E" colspan="6">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                                                        ForeColor="White" Text="REMESA PERIODO OTRO"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="lblValidacionOtroMes" runat="server" Text="Falta validar algunos campos."
                                                        CssClass="hideControl" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMesAnioOtroMes" runat="server" Text="Periodo:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAnioOtro" runat="server" Width="60px" 
                                                        AutoPostBack="True" onselectedindexchanged="ddlAnioOtro_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlMesOtro" runat="server" Width="60px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTipoRemesaOtroMes" runat="server" Text="Tipo Remesa:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlTipoRemesaOtroMes" runat="server" Width="250px">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblVal_ddlTipoRemesaOtroMes" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="Label8" runat="server" Text="T.C. Consular:"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_otro_FTipoCambioConsular" runat="server" Enabled="False" CssClass="campoNumero"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:Label ID="Label5" runat="server" Text="Moneda:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:DropDownList ID="ddl_rede_otro_moneda_local" runat="server" Width="183px" Enabled="false"></asp:DropDownList>
                                                </td>             
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:Label ID="Label14" runat="server" Text="Moneda:"></asp:Label></td>
                                                <td style="border-top-style: solid; border-top-width: 1px; border-top-color: #800000">
                                                    <asp:DropDownList ID="ddl_rede_otro_moneda" runat="server" Width="183px"></asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddl_rede_otro_moneda" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="Label9" runat="server" Text="T.C. Bancario:"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_otro_FTipoCambioBancario" runat="server" Enabled="true" CssClass="campoNumero"
                                                        onkeypress="return isDecimalKey(this,event)" onBlur="validanumeroLostFocus(this)"
                                                        Width="150px"></asp:TextBox>
                                                    <asp:Label ID="lblCO_otro_FTipoCambioBancario" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:Label ID="lblMontoOtroMes" runat="server" Text="Total:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:TextBox ID="txt_rede_otro_total" runat="server" Width="180px" 
                                                        MaxLength="10" CssClass="campoNumero"
                                                        onkeypress="return isDecimalKey(this,event)" 
                                                        onBlur="validanumeroLostFocus(this)" ></asp:TextBox>
                                                    <asp:Label ID="lblVal_txtMontoOtroMes" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:Label ID="Label7" runat="server" Text="Total:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #800000;">
                                                    <asp:TextBox ID="txt_rede_otro_dolares" runat="server" Width="180px" 
                                                        MaxLength="20" CssClass="campoNumero" Enabled="false"></asp:TextBox>
                                                    <asp:Button ID="btnCalcular" runat="server"
                                                        onclick="btnCalcular_Click" CssClass="btnCalcular" />
                                                </td>                                                    
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFechaOtroMes" runat="server" Text="Fecha Depósito:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="datFechaOtroMes" runat="server" />
                                                </td>
                                                <td><asp:Label ID="Label10" runat="server" Text="Banco:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_rede_otro_banco" runat="server" Width="180px" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddl_rede_otro_banco_SelectedIndexChanged"  >
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblCO_ddl_otro_banco" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td><asp:Label ID="Label11" runat="server" Text="N° Cuenta:"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_rede_otro_cuenta" runat="server" Width="183px"></asp:DropDownList>
                                                    <asp:Label ID="Label12" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVoucherOtroMes" runat="server" Text="N° Voucher/Cheque:"></asp:Label>                                                    
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_rede_otro_voucher" runat="server" Width="150px" CssClass="campoNumero" MaxLength="20" ></asp:TextBox>
                                                    <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTarifa" runat="server" Text="Tarifa:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlTarifaDescripcion" runat="server" Width="462px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 120px">
                                                    <asp:Label ID="lblTitularDatos" runat="server" Text="Datos Titular:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtTitularDatos" runat="server" Width="400px" MaxLength="100" onkeypress="return ValidarLetras(event)" CssClass="txtLetra"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Button ID="btnAceptarOtroMes" runat="server" Text="Aceptar" CssClass="btnGeneral"
                                                        OnClick="btnAceptarOtroMes_Click" OnClientClick="return ValidarDetalleOtroMes()" />
                                                    <asp:Button ID="btnCancelarOtroMes" runat="server" Text="Cancelar" CssClass="btnGeneral"
                                                        OnClick="btnCancelarOtroMes_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gdvRemesaDetalle" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvRemesaDetalle_RowCommand">
                                            <Columns>
                                                <%--Posicion : 0--%>
                                                <asp:BoundField DataField="rede_bMesFlag" HeaderText="Flag" HeaderStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <%--Posicion : 1--%>
                                                <asp:BoundField DataField="rede_cPeriodo" HeaderText="Periodo">
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:BoundField>
                                                <%--Posicion : 2--%>
                                                <asp:BoundField DataField="rede_dFechaEnvio" DataFormatString="{0:MMM-dd-yyyy}" HeaderText="Fecha Envío">
                                                    <ItemStyle Font-Bold="False" HorizontalAlign="Center" Width="70px" />
                                                </asp:BoundField>
                                                <%--Posicion : 3--%>
                                                <asp:BoundField DataField="rede_vTipo" HeaderText="Tipo Remesa">
                                                    <ItemStyle HorizontalAlign="Center" Width="140px" />
                                                </asp:BoundField>
                                                <%--Posicion : 4--%>
                                                <asp:BoundField DataField="rede_vClasificacion" HeaderText="Clasificación">
                                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                </asp:BoundField>
                                                <%--Posicion : 5--%>
                                                <asp:BoundField DataField="rede_FTipoCambioBancario" HeaderText="T.C.Bancario" DataFormatString="{0:#0.00000}">
                                                    <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <%-- Posición 6 : Celda usada para calcular total remesa (Buscar: Cells[6]) --%>
                                                <asp:BoundField DataField="rede_FMonto" HeaderText="Total Moneda Local" 
                                                    DataFormatString="{0:#.00000}">
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:BoundField>
                                                <%--Posicion : 7--%>
                                                <asp:BoundField DataField="rede_FTotalDolares" HeaderText="Total (Dólares)" 
                                                    DataFormatString="{0:#.00000}">
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:BoundField>                                                
                                                <%--Posicion : 8--%>
                                                <asp:BoundField DataField="rede_vObservacion" HeaderText="Observación">
                                                    <ItemStyle HorizontalAlign="Justify" Width="120px" />
                                                </asp:BoundField>  
                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <%--Posicion : 9--%>
                                                <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnAnular" CommandName="Eliminar" ToolTip="Anular Rango" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                            </Columns> 
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>
                                        <PageBarContent:PageBar ID="ctrlPaginadorDetalle" runat="server" OnClick="ctrlPaginadorDetalle_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
</asp:Content>
