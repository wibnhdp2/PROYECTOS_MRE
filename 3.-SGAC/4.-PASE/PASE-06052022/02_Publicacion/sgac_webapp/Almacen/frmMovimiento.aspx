<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMovimiento.aspx.cs" Inherits="SGAC.WebApp.Almacen.frmMovimientos" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlFecha.ascx" TagName="Fecha" TagPrefix="Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/PopUpEspera.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">

        function pageLoad() {
            $("#<%=txtRanIni.ClientID %>").on("keydown", validarNumero);
            $("#<%=txtRanFin.ClientID %>").on("keydown", validarNumero);
        }



        $(document).ready(function () {
            // grab the script manager
            var manager = Sys.WebForms.PageRequestManager.getInstance();

            // hook in our callbacks for the ASP.NET Ajax Postbacks
            manager.add_beginRequest(modalBeginRequest);
            manager.add_endRequest(modalEndRequest);

            // override the 5px grey border
            $.blockUI.defaults.css['border'] = '0px';
            $.blockUI.defaults.css['height'] = '100px';
            $.blockUI.defaults.css['width'] = '400px';
            $.blockUI.defaults.css['background'] = 'transparent url(Images/background_image_here.png) no-repeat';
        });

        function modalBeginRequest(sender, args) {
            // kill the scrollbars
            //    $('html').css({ "overflow": "hidden" });


            var vGuardandoMovimiento = $('#<%=hfGrabandoMovimiento.ClientID%>').html();

            if (vGuardandoMovimiento != null) {
                if (vGuardandoMovimiento === '1') {
                    // show a modal popup
                    $.blockUI({ message: $('#modalPopup') });
                }
            }


        }

        function modalEndRequest(sender, args) {
            // remove the modal popup
            $.unblockUI();

            $('#<%=hfGrabandoMovimiento.ClientID%>').text('0');
            // restore the scrollbars
            //    $('html').css({ "overflow": "scroll" });

        }        

        $(function () {
            $('#tabs').tabs();            
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
            if (charCode > 31 && charCode < 123) {
                letra = true;
            }
            if (charCode == 130) {
                letra = true;
            }
            if (charCode == 144) {
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
            if (charCode == 193) {
                letra = true;
            }
            if (charCode == 201) {
                letra = true;
            }
            if (charCode == 205) {
                letra = true;
            }
            if (charCode == 211) {
                letra = true;
            }
            if (charCode == 218) {
                letra = true;
            }
            if (charCode == 220) {
                letra = true;
            }
            if (charCode == 252) {
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

        function CalculaRangos() {
            var rangoInicio = document.getElementById('<%= txtRanIni.ClientID %>').value;
            var rangoFin = document.getElementById('<%= txtRanFin.ClientID %>').value;
            document.getElementById('<%= txtCant.ClientID %>').value = "";


            if (parseInt(rangoInicio) <= 0) {
                document.getElementById('<%= txtRanIni.ClientID %>').value = '';
                alert("El rango no puede ser menor o igual a cero.");
                return;
            }

            if (parseInt(rangoFin) <= 0) {
                document.getElementById('<%= txtRanFin.ClientID %>').value
                alert("El rango no puede ser menor o igual a cero.");
                return;
            }

            var RanIni = 0;
            var RanFin = 0;
            var Cant = 0;



            if (rangoInicio == "") {
                RanIni = 0;
            }
            else {
                RanIni = rangoInicio;
            }
            if (rangoFin == "") {
                RanFin = 0;
            }
            else {
                RanFin = rangoFin;
            }


            if (rangoFin != "" && rangoInicio != "") {
                if (parseInt(RanFin) >= parseInt(RanIni)) {
                    Cant = ((RanFin - RanIni) + 1);
                    document.getElementById('<%= txtCant.ClientID %>').value = Cant;
                }
                else {
                    alert("El rango Final debe ser mayor o igual al inicial");
                    Cant = 0;
                    document.getElementById('<%= txtCant.ClientID %>').value = Cant;
                }
            }
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

        function ActivarModal() {
            $('#<%=hfGrabandoMovimiento.ClientID%>').text('1');
        }

        function ValidarRegistro() {

            ActivarModal();

            var bolValida = true;
            var strDdl_01 = $.trim($("#<%= cboTipoBovedaO.ClientID %>").val());
            var strDdl_02 = $.trim($("#<%= cboTipoBovedaD.ClientID %>").val());
            var strDdl_03 = $.trim($("#<%= cboBovedaO.ClientID %>").val());
            var strDdl_04 = $.trim($("#<%= cboBovedaD.ClientID %>").val());
            var strDdl_05 = $.trim($("#<%= cboInsumoR.ClientID %>").val());
            var strDdl_06 = $.trim($("#<%= cboMovimientoTipo.ClientID %>").val());
            var strDdl_07 = $.trim($("#<%= cboMovimientoMotivo.ClientID %>").val());

            var strTxt_01 = $.trim($("#<%= txtPrefijoNum.ClientID %>").val());
            var strTxt_02 = $.trim($("#<%= txtNumeroPedido.ClientID %>").val());
            var strTxt_03 = $.trim($("#<%= txtNroActa.ClientID %>").val());

            var cboTipoBovedaO = document.getElementById('<%= cboTipoBovedaO.ClientID %>');
            if (strDdl_01 == "0") {
                cboTipoBovedaO.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboTipoBovedaO.style.border = "1px solid #888888";
            }

            var cboTipoBovedaD = document.getElementById('<%= cboTipoBovedaD.ClientID %>');
            if (strDdl_02 == "0") {
                cboTipoBovedaD.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboTipoBovedaD.style.border = "1px solid #888888";
            }

            var cboBovedaO = document.getElementById('<%= cboBovedaO.ClientID %>');
            if (strDdl_03 == "0") {
                cboBovedaO.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboBovedaO.style.border = "1px solid #888888";
            }

            var cboBovedaD = document.getElementById('<%= cboBovedaD.ClientID %>');
            if (strDdl_04 == "0") {
                cboBovedaD.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboBovedaD.style.border = "1px solid #888888";
            }

            var cboInsumoR = document.getElementById('<%= cboInsumoR.ClientID %>');
            if (strDdl_05 == "0") {
                cboInsumoR.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboInsumoR.style.border = "1px solid #888888";
            }

            var cboMovimientoTipo = document.getElementById('<%= cboMovimientoTipo.ClientID %>');
            if (strDdl_06 == "0") {
                cboMovimientoTipo.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboMovimientoTipo.style.border = "1px solid #888888";
            }

            var cboMovimientoMotivo = document.getElementById('<%= cboMovimientoMotivo.ClientID %>');
            if (strDdl_07 == "0" || strDdl_07 == "- SELECCIONAR -") {
                cboMovimientoMotivo.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboMovimientoMotivo.style.border = "1px solid #888888";
            }

            /*VALORES DE TXT*/
            var txtPrefijoNum = document.getElementById('<%= txtPrefijoNum.ClientID %>');
            if (strTxt_01.length == 0) {
                txtPrefijoNum.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtPrefijoNum.style.border = "1px solid #888888";
            }

            var txtNumeroPedido = document.getElementById('<%= txtNumeroPedido.ClientID %>');
            if (strTxt_02.length == 0) {
                txtNumeroPedido.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtNumeroPedido.style.border = "1px solid #888888";
            }

//            var txtNroActa = document.getElementById('<%= txtNroActa.ClientID %>');
//            if (strTxt_03.length == 0) {
//                txtNroActa.style.border = "1px solid Red";
//                bolValida = false;
//            }
//            else {
//                txtNroActa.style.border = "1px solid #888888";
//            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            if (!bolValida)
                $('#<%=hfGrabandoMovimiento.ClientID%>').text('0');

            return bolValida;
        }

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function validaHora(ctrl) {
            var x = ctrl.value;
            var hora = x.substring(0, 2);
            var minuto = x.substring(3, 5);
            var valor = true;

            if (x.length > 0 && x.length < 5) {
                valor = false;
            }

            if (hora > 23) {
                valor = false;
            }
            if (minuto > 59) {
                valor = false;
            }
            if (!valor) {
                ctrl.focus();
                alert("Formato de hora Incorrecto");
                ctrl.value = "";
            }
        }


        function isHoraKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = ":"
            var x = ctrl.value;
            var y = x.indexOf(FIND);

            if (charCode == 58) {
                return false;
            }

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


            if (charCode < 48 || charCode > 58)
                return false;
            return true;
        }

        function ConvertirFechaAFormatoUs(textoFecha) {

            textoFecha = textoFecha.replace('ene', 'jan');
            textoFecha = textoFecha.replace('abr', 'apr');
            textoFecha = textoFecha.replace('ago', 'aug');
            textoFecha = textoFecha.replace('dic', 'dec');

            return textoFecha;
        }

        function ValidarFechaInicioFin(sender, args) 
        {

            //            alert('fechaInicio');
            var FechaTexto = document.getElementById('<% =dtpFecha.ClientID %>');
            var FechaValijaTexto = document.getElementById('<% =dtpFechaValija.ClientID %>');


            var Fecha = Date.parse(ConvertirFechaAFormatoUs(FechaTexto.value));
            var FechaValija = Date.parse(ConvertirFechaAFormatoUs(FechaValijaTexto.value));

            if (Fecha > FechaValija) {

                FechaValijaTexto.value = '';
                alert("La fecha de la valija diplomática no puede ser menor a la fecha del movimiento.");
            }
        }


        function fechavalida() {
            document.getElementById('<%= dtpFechaValija.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
        }

        
    </script>
  
    <style type="text/css">
        .style2
        {
            text-align: center;
        }
        .style37
        {
            color: #800000;
            font-size: medium;
        }
        
        .style42
        {
            width: 300px;
            text-align: center;
        }
        .style43
        {
            width: 242px;
            text-align: center;
        }
        .style44
        {
            width: 90px;
            text-align: center;
        }
        .style45
        {
            width: 185px;
            text-align: center;
        }
        .style46
        {
            width: 67%;
        }
        .style48
        {
            width: 686px;
        }
        .style49
        {
            height: 18px;
        }
        .style50
        {
            text-align: right;
            height: 30px;
        }
        .style51
        {
            height: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitMovimientos" runat="server" Text="MOVIMIENTOS DE INSUMOS"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table width="90%" align="center" >
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
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>

                             <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>

                                <table style="width: 844px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFecInicio" runat="server" Text="Fec. Inicio :" Font-Size="10pt"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFecFin" runat="server" Text="Fec. Fin :" Font-Size="10pt" />
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInsumo" runat="server" Text="Insumo :" Font-Size="10pt" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboInsumoC" runat="server" Width="210px" Height="21px" 
                                                Font-Size="10pt" Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                                                        <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <%--<uc3:CalendarExtender ID="CalendarExtender1"
                                                                runat="server" 
                                                                TargetControlID="dtpFecInicio"
                                                                PopupButtonID="imgCal1" />--%>
                                            <%--<Fecha:Fecha ID="dtpFecInicio" runat="server" />--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedidoC" runat="server" Text="Pedido :" Font-Size="10pt" />
                                        </td>
                                        <td>
    <%--                                        <uc3:CalendarExtender ID="CalendarExtender2" 
                                                                runat="server" 
                                                                TargetControlID="dtpFecFin"
                                                                PopupButtonID="imgCal2" />--%>
                                            <%--<Fecha:Fecha ID="dtpFecFin" runat="server" />--%>
                                            <asp:TextBox ID="txtNumeroPedidoC" runat="server" CssClass="txtLetra" 
                                                Font-Size="10pt" MaxLength="12" onkeypress="return isLetraNumeroDoc(event)" 
                                                Width="125px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Font-Size="10pt" Text="Estado :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboMovimientoEstadoC" runat="server" Font-Size="10pt" 
                                                Height="21px" Width="210px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>

            <table style="width: 844px">
            <tr>
                    <td class="style26" colspan="6" bgcolor="#4E102E" align="center">
                        <asp:Label ID="Label8" runat="server" CssClass="style37" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="10pt" Font-Underline="True" 
                            ForeColor="White" Text="UBICACIÓN"></asp:Label>
                    </td>
                </tr>
                <tr>
                    
                    <td class="style57">
                        <asp:Label ID="Label10" runat="server" Font-Size="10pt" Height="16px" 
                            Text="Of. Consular:" Width="85px"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEstado0" runat="server" Font-Size="10pt" 
                            style="text-align: left" Text="Tipo Bóveda: " Width="94px"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEstado1" runat="server" Font-Size="10pt" 
                            style="text-align: left" Text="Bóveda: " Width="99px"></asp:Label>
                    </td>
                    <td class="style68">
                        &nbsp;</td>
                <tr>
                    
                    <td class="style57">
                        <uc1:ctrlOficinaConsular ID="cboMisionConsConsulta" runat="server" 
                            Width="300px" Enabled="False" />
                    </td>
                    <td>
                        <asp:DropDownList ID="cboTipoBovedaConsulta" runat="server" AutoPostBack="True" 
                            Font-Size="10pt" OnSelectedIndexChanged="cboTipoBovedaConsulta_SelectedIndexChanged" 
                            Width="200px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="cboBovedaConsulta" runat="server" AutoPostBack="True" 
                            Font-Size="10pt" Height="24px" Width="270px" 
                            onselectedindexchanged="cboBovedaConsulta_SelectedIndexChanged" 
                            Enabled="False">
                        </asp:DropDownList>
                    </td>
                    
                </tr>
            
             </table>  
             </ContentTemplate>
                            </asp:UpdatePanel> 
                                <%--Opciones--%>
                                <table>
                                    <tr>
                                        <td>
                                            <ToolBar:ToolBarContent ID="ctrlToolBar1" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <%--Opciones--%>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <%--OnRowDataBound="grdMovimiento_RowDataBound" --%>
                                                    <asp:GridView ID="grdMovimiento" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                        runat="server" AutoGenerateColumns="False" GridLines="None" 
                                                        OnRowCommand="grdMovimiento_RowCommand"                                                        
                                                        Width="900px" Font-Size="8pt" onrowdatabound="grdMovimiento_RowDataBound">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="MovimientoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sOficinaConsularIdOrigen" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="OfConsular Origen" HeaderText="Origen"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_sBovedaTipoIdOrigen" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Tipo Origen" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sbodegaOrigenId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Boveda Origen" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sOficinaConsularIdDestino" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OfConsular Destino" HeaderText="Destino"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_sBovedaTipoIdDestino" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Tipo Destino" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sbodegaDestinoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Boveda Destino" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sInsumoTipoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Insumo" HeaderText="Insumo"></asp:BoundField>
                                                            
                                                            <asp:BoundField DataField="pedi_ICantidad" HeaderText="Cant. Solicitada">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InsumoCantidad" HeaderText="Cant. Atendida">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" 
                                                                DataFormatString="{0:MMM-dd-yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_vActaRemision" HeaderText="Acta Rem."></asp:BoundField>
                                                            <asp:BoundField DataField="movi_sMovimientoTipoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_sEstadoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_sMovimientoMotivoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Motivo" HeaderText="Motivo"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_vPrefijoNumeracion" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_sMovimientoMotivoId" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_cPedidoCodigo" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="movi_cMovimientoCodigo" HeaderText="Movimiento"></asp:BoundField>
                                                            <asp:BoundField DataField="movi_dFechaValija" HeaderText="" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnVer" CommandName="Ver" ToolTip="Ver Detalle" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Movimiento"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Recibir" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnAtender" CommandName="Atender" ToolTip="Recibir Movimiento"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/recibir_pedido.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                            </asp:TemplateField>
<%--
                                                            <asp:TemplateField HeaderText="Aceptar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnAceptar" CommandName="Editar" ToolTip="Editar Movimiento"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>

                                                        </Columns>
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

                       

                        <asp:UpdatePanel runat="server" ID="UpdMantenimiento" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>

                                            <ToolBar:ToolBarContent ID="ctrlToolBar2" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblValidacion" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="Falta validar algunos campos." />
                                        </td>
                                    </tr>
                                </table>
                                <table width="934px">
                                    <tr>
                                        <td colspan="6" style="text-align: right">
                                            <asp:Label ID="lblNroMovimientoEtiqueta" runat="server" Font-Bold="True" 
                                                Font-Size="10pt" Text="Nro. Movimiento:"></asp:Label>
                                        </td>
                                        <td style="text-align: left" colspan="2">
                                            <asp:Label ID="lblNroMovimiento" runat="server" Font-Bold="True" Font-Size="10pt"
                                                ForeColor="#4E102E" Style="font-size: 10pt; text-align: right;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTipoInsumo" runat="server" Text="Insumo:" Font-Size="10pt"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="cboInsumoR" runat="server" AutoPostBack="True" Height="20px"
                                                OnSelectedIndexChanged="cboInsumoR_SelectedIndexChanged" Width="160px" Font-Size="10pt">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio5" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPrefijoNum" runat="server" Font-Size="10pt" Text="Prefijo:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrefijoNum" runat="server" Font-Size="10pt" MaxLength="5" Style="text-align: center"
                                                Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFecha" runat="server" Font-Size="10pt" Text="Fecha:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecha" runat="server" Enabled="False" />
                                            <asp:Label ID="lblObligatorio6" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblEstado" runat="server" Font-Size="10pt" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboMovimientoEstado" runat="server" Font-Size="10pt" Height="24px"
                                                Width="125px" Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMovimientoTipo" runat="server" Font-Size="10pt" Style="text-align: right"
                                                Text="Movimiento:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboMovimientoTipo" runat="server" Font-Size="10pt" Height="22px"
                                                Width="160px"  
                                                OnSelectedIndexChanged="cboMovimientoTipo_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio7" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMotivoMovimiento" runat="server" Font-Size="10pt" Style="text-align: right"
                                                Text="Motivo:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboMovimientoMotivo" runat="server" AutoPostBack="True" Font-Size="10pt"
                                                Height="23px" OnSelectedIndexChanged="cboMovimientoMotivo_SelectedIndexChanged"
                                                Width="160px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio8" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblNumeroPedido" runat="server" Font-Size="10pt" Text="Pedido:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumeroPedido" runat="server" Font-Size="10pt" onkeypress="return isLetraNumero(event)"
                                                MaxLength="12" Width="125px" CssClass="txtLetra" Enabled="False">0</asp:TextBox>
                                            <asp:Label ID="lblObligatorio9" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblNroActaRemisionR" runat="server" Font-Size="10pt" Text="N° Acta:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNroActa" runat="server" Font-Size="10pt" MaxLength="12" onkeypress="return isLetraNumeroDoc(event)"
                                                Width="125px" CssClass="txtLetra"></asp:TextBox>
                                            <%--<asp:Label ID="lblObligatorio10" runat="server" Style="color: #FF0000" Text="*"></asp:Label>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                    <div id="ocultarFechaValija" runat="server">
                                        <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td colspan="2" style="text-align: right">
                                                <asp:Label ID="LblFechaValija0" runat="server" Font-Size="10pt" 
                                                    style="font-weight: 700; color: #990033" Text="Datos Valija Diplomática =&gt;"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="LblFechaValija" runat="server" Font-Size="10pt" Text="Fecha:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpFechaValija" runat="server" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="LblHoraValija" runat="server" Font-Size="10pt" Text="Hora:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHoraInicio" runat="server" MaxLength="5" 
                                                    onBlur="validaHora(this)" onkeypress="return isHoraKey(this,event)" 
                                                    Width="57px" onpaste="return false"></asp:TextBox>
                                                <asp:Label ID="LblFechaValija1" runat="server" Font-Size="10pt" 
                                                    style="font-weight: 700; color: #990033" Text="formato Hora (HH:mm)"></asp:Label>
                                            </td>
                                    </div>
                                        
                                    </tr>
                                </table>
                                <table style="border-left: 1px solid #800000; border-right: 1px solid #800000; border-top: 1px solid #800000;
                                    width: 938px; border-bottom-style: none; border-bottom-color: #800000; height: 31px;"
                                    id="DatosPedido" bgcolor="#DBDBDB" runat="server" visible="True">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Pedido" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Motivo" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Insumo" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Fecha" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Acta Pedido" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="Cantidad" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Estado" Font-Size="9pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPedCodigo" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedMotivo" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedInsumo" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedFecha" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedActa" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedCantidad" runat="server" Font-Size="9pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPedEstado" runat="server" Style="text-align: center" Font-Size="9pt"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="border-left: 1px solid #800000; border-right: 1px solid #800000; border-top: 1px solid #800000;
                                    width: 938px; border-bottom-style: none; border-bottom-color: #800000;">
                                    <tr>
                                        <td colspan="2" style="background-color: #C0C0C0">
                                            <asp:Label ID="lblOrigen" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"
                                                Font-Underline="True" ForeColor="#990033" Text="ORIGEN"></asp:Label>
                                        </td>
                                        <td style="background-color: #C0C0C0">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOfConsularO" runat="server" Text="Of. Consular" Font-Size="10pt"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoBovedaO" runat="server" Font-Size="10pt" Text="Tipo Bóveda"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBovedaOrigen" runat="server" Font-Size="10pt" Text="Bóveda"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="cboMisionConsO" runat="server" Enabled="False" 
                                                Visible="True" />
                                            <asp:Label ID="lblObligatorio11" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboTipoBovedaO" runat="server" AutoPostBack="True" Font-Size="10pt"
                                                OnSelectedIndexChanged="cboTipoBovedaO_SelectedIndexChanged" Width="200px" 
                                                Enabled="False">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio12" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboBovedaO" runat="server" Font-Size="10pt" Height="24px" 
                                                Width="300px" onselectedindexchanged="cboBovedaO_SelectedIndexChanged" 
                                                AutoPostBack="True" Enabled="False">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio13" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="border: 1px solid #800000; width: 938px;">
                                    <tr>
                                        <td colspan="2" style="background-color: #C0C0C0">
                                            <asp:Label ID="lblDestino" runat="server" CssClass="style37" Font-Bold="True" Font-Names="Arial"
                                                Font-Size="Smaller" Font-Underline="True" ForeColor="#990033" Text="DESTINO"></asp:Label>
                                        </td>
                                        <td style="background-color: #C0C0C0">
                                            <asp:Label ID="lblSaldoMesAnterior" runat="server" Font-Bold="True" 
                                                Font-Names="Arial" Font-Size="Smaller" Font-Underline="False" 
                                                ForeColor="#990033"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOfConsularO0" runat="server" Font-Size="10pt" Text="Of. Consular"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoBovedaO0" runat="server" Font-Size="10pt" Text="Tipo Bóveda"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBovedaOrigen0" runat="server" Font-Size="10pt" Text="Bóveda"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="cboMisionConsD" runat="server" Enabled="False" 
                                                Visible="True" />
                                            <asp:Label ID="lblObligatorio16" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboTipoBovedaD" runat="server" AutoPostBack="True" Font-Size="10pt"
                                                OnSelectedIndexChanged="cboTipoBovedaD_SelectedIndexChanged" Width="200px" 
                                                Enabled="False">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio15" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboBovedaD" runat="server" Font-Size="10pt" Height="23px" 
                                                Width="300px" AutoPostBack="True" 
                                                onselectedindexchanged="cboBovedaD_SelectedIndexChanged" Enabled="False">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblObligatorio14" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div align="left">
                                </div>
                                
                                <asp:UpdatePanel ID="updRango" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table ID="TodoRango" style="border: 1px solid #800000;" >
                                            <tr style="border: 1px solid #800000">
                                                <td class="style48" valign="top">
                                                    <table width="400px" style="width: 676px">
                                                        <tr>
                                                            <td class="style2">
                                                                <asp:Label ID="lblRanIni" runat="server" Font-Size="10pt" Text="Rango Inicio" 
                                                                    Width="75px"></asp:Label>
                                                            </td>
                                                            <td class="style42">
                                                                <asp:Label ID="blRanFin" runat="server" Font-Size="10pt" Text="Rango Final" 
                                                                    Width="75px"></asp:Label>
                                                            </td>
                                                            <td class="style43" colspan="2">
                                                                <asp:Label ID="lblCant" runat="server" Font-Size="10pt" 
                                                                    Style="text-align: center" Text="Cantidad" Width="75px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: center">
                                                                <asp:Label ID="lblObservacion" runat="server" Font-Size="10pt" 
                                                                    Style="text-align: center" Text="Observación" Width="75px"></asp:Label>
                                                            </td>
                                                            <td class="style45">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style42" valign="top">
                                                                <asp:TextBox ID="txtRanIni" runat="server" CssClass="campoNumero" 
                                                                    Font-Size="10pt" MaxLength="10" onBlur="CalculaRangos()" 
                                                                    onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>
                                                                <asp:Label ID="lblObligatorio20" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td class="style42" valign="top">
                                                                <asp:TextBox ID="txtRanFin" runat="server" CssClass="campoNumero" 
                                                                    Font-Size="10pt" MaxLength="10" onBlur="CalculaRangos()" 
                                                                    onkeypress="return isNumberKey(event)" Width="90px"></asp:TextBox>
                                                                <asp:Label ID="lblObligatorio21" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td class="style43" colspan="2" valign="top">
                                                                <asp:TextBox ID="txtCant" runat="server" AutoPostBack="True" Enabled="False" 
                                                                    Font-Size="10pt" Style="text-align: right" Width="60px"></asp:TextBox>
                                                                <asp:Label ID="lblObligatorio19" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                                                            </td>
                                                            <td class="style44" valign="top">
                                                                <asp:TextBox ID="txtObservacion" runat="server" CssClass="txtLetra" 
                                                                    Font-Size="10pt" Height="48px" MaxLength="150" 
                                                                    onkeypress="return isLetraNumero(event)" TextMode="MultiLine" 
                                                                    Width="265px"></asp:TextBox>
                                                            </td>
                                                            <td class="style45" valign="top">
                                                                <asp:Button ID="btnAdicionar" runat="server" CssClass="btnAlmacenAcepta" 
                                                                    Font-Size="10pt" OnClick="btnAdicionar_Click" Text="Adicionar" 
                                                                    ToolTip="Adicionar" Width="90px" />
                                                                <asp:Button ID="btnCancelar" runat="server" CssClass="btnAlmacenAcepta" Font-Size="10pt" 
                                                                    OnClick="btnCancelar_Click" Text="Cancelar" ToolTip="Cancelar" 
                                                                    Width="90px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" height="50" class="style51">
                                                                <asp:Label ID="LblTotal" runat="server" Font-Size="10pt" 
                                                                    style="font-size: 9pt; color: #800000; font-weight: 700"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                <div style="height:153px; border:1px solid #800000; overflow:auto; margin: 5px 0 5px 0 ;">
                                                                
                                                                    <asp:GridView ID="grdRangos" runat="server" style="margin: 5px;"
                                                                        AlternatingRowStyle-HorizontalAlign="Left" AutoGenerateColumns="False" 
                                                                        CssClass="mGrid" Font-Size="10pt" GridLines="None" 
                                                                        OnRowCommand="grdRangos_RowCommand" onrowdatabound="grdRangos_RowDataBound" 
                                                                        RowStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="Top" 
                                                                        ShowHeaderWhenEmpty="True" Width="650px">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="RangoIni" HeaderText="Rango Ini">
                                                                                <HeaderStyle Width="80px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RangoFin" HeaderText="Rango Fin">
                                                                                <HeaderStyle Width="80px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Cant" HeaderText="Cant">
                                                                                <HeaderStyle Width="80px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                                <HeaderStyle Width="250px" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="mode_sEstadoId" HeaderStyle-CssClass="ColumnaOculta" 
                                                                                HeaderText="" ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                                <ItemStyle CssClass="ColumnaOculta" Width="5px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="movi_IMovimientoId" 
                                                                                HeaderStyle-CssClass="ColumnaOculta" HeaderText="" 
                                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                                <ItemStyle CssClass="ColumnaOculta" Width="5px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="mode_iMovimientoDetalleId" 
                                                                                HeaderStyle-CssClass="ColumnaOculta" HeaderText="" 
                                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                                <ItemStyle CssClass="ColumnaOculta" Width="5px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="mode_iActuacionId" 
                                                                                HeaderStyle-CssClass="ColumnaOculta" HeaderText="" 
                                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                                <ItemStyle CssClass="ColumnaOculta" Width="5px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="mode_iActuaciondetalleId" 
                                                                                HeaderStyle-CssClass="ColumnaOculta" HeaderText="" 
                                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                                <ItemStyle CssClass="ColumnaOculta" Width="5px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Estado" HeaderStyle-CssClass="" HeaderText="Estado" 
                                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                                <HeaderStyle CssClass="ColumnaOculta" Width="5px" />
                                                                                <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Center" Width="5px" />
                                                                            </asp:BoundField>

                                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btnEditar" runat="server" 
                                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" 
                                                                                        ImageUrl="../Images/img_grid_modify.png" ToolTip="Editar Rango" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="50px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btnEliminarDetalle" runat="server" 
                                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                                        CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" 
                                                                                        ToolTip="Eliminar Opción" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="50px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle Font-Names="Arial" Font-Size="11px" />
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
                                                                        <AlternatingRowStyle />
                                                                        <PagerStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>

                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style50" colspan="3">
                                                                <asp:Button ID="btnAceptado" runat="server" CssClass="btnAlmacenAcepta" Font-Size="10pt" 
                                                                    OnClick="btnAceptado_Click" Text="Aceptado" Width="90px" Visible="False" />
                                                                <asp:Button ID="btnRechazado" runat="server" CssClass="btnAlmacenAcepta" 
                                                                    Font-Size="10pt" OnClick="btnRechazado_Click" Text="Rechazado" 
                                                                    Width="90px" Visible="False" />
                                                                <asp:Button ID="btnimprimir" runat="server" Text="Imprimir" onclick="btnimprimir_Click" CssClass="btnAlmacenAcepta" Visible="False"
                                                                    Font-Size="10pt" Width="90px"></asp:Button>
                                                            </td>
                                                            <td class="style50" colspan="3" valign="top">
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" style="width:350px" >
                                                    <table class="style46">
                                                        <tr>
                                                            <td class="style49" valign="top">
                                                                <asp:Label ID="lblInsumosEtiqueta" runat="server" Font-Bold="True" 
                                                                    Font-Names="Arial" Font-Size="10pt" 
                                                                    Font-Underline="False" ForeColor="#990033" Text="Insumos Disponibles :" 
                                                                    Width="150px" Height="19px"></asp:Label>
                                                                <asp:Label ID="LblTotalInsumos" runat="server" Font-Bold="True" 
                                                                    Font-Names="Arial" Font-Size="10pt" Font-Underline="False" ForeColor="#990033" 
                                                                    Height="19px" style="text-align: right" Width="33px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top"  >
                                                                <div style="overflow:auto; height:225px; width:230px; border:1px solid #800000; margin: 5px;">
                                                                    <asp:GridView ID="grdRangosDisponibles" runat="server" style="width:200px; margin: 5px;"
                                                                        AlternatingRowStyle-HorizontalAlign="Left" AutoGenerateColumns="False" 
                                                                        CssClass="mGrid" Font-Size="10pt" GridLines="None" 
                                                                        OnRowCommand="grdRangos_RowCommand" RowStyle-HorizontalAlign="Left" 
                                                                        RowStyle-VerticalAlign="Top" ShowHeaderWhenEmpty="True" 
                                                                        onrowdatabound="grdRangosDisponibles_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Rango Inicio" HeaderText="Rango Inicio">
                                                                                <HeaderStyle Width="75px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Rango Fin" HeaderText="Rango Fin">
                                                                                <HeaderStyle Width="75px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Cant" HeaderText="Cant">
                                                                                <HeaderStyle Width="75px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <RowStyle Font-Names="Arial" Font-Size="11px" />
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
                                                                        <AlternatingRowStyle />
                                                                        <PagerStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="modalPopup">
                                            <b />
                                            <h3 style="font-size:small">Procesando la información. Por favor espere...</h3>
                                            <asp:Image ID="imgEspera" ImageUrl="../Images/espera.gif" runat="server" />
                                        </div>
                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <br />

    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    <asp:Label ID="hfGrabandoMovimiento" runat="server" Text="0" CssClass="hideText"></asp:Label>
   <asp:GridView ID="grdAlmacenUniversal" runat="server" Visible="false" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="OfConsularId" />
        <asp:BoundField DataField="TipoBoveda" />
        <asp:BoundField DataField="IdTablaOrigenRefer" />
        <asp:BoundField DataField="Descripcion" />
        <asp:BoundField DataField="Tabla" />

    </Columns>
    </asp:GridView>

</asp:Content>
