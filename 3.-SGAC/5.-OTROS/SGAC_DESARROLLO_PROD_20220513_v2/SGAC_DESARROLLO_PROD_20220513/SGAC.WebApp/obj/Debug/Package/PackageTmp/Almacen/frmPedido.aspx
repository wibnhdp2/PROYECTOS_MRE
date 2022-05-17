<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPedido.aspx.cs" Inherits="SGAC.WebApp.Almacen.frmPedidos" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlFecha.ascx" tagname="Fecha" tagprefix="Fecha" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlPageBar.ascx" tagname="PageBar" tagprefix="PageBarContent" %>
<%@ Register src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" tagname="ctrlOficinaConsular" tagprefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
        
    <script type="text/javascript">
        function pageLoad() {
            $("#<%=txtCant.ClientID %>").on("keydown", validarNumero);
        }

        $(function () {
            $('#tabs').tabs();            
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function ValidarRegistro() {
            var bolValida = true;
            var strDdl_01 = $.trim($("#<%= cboTipoBovedaO.ClientID %>").val());
            var strDdl_02 = $.trim($("#<%= cboTipoBovedaD.ClientID %>").val());
            var strDdl_03 = $.trim($("#<%= cboBovedaO.ClientID %>").val());
            var strDdl_04 = $.trim($("#<%= cboBovedaD.ClientID %>").val());
            var strDdl_05 = $.trim($("#<%= cboInsumo.ClientID %>").val());
            var strDdl_06 = $.trim($("#<%= cboTipoPed.ClientID %>").val());
            var strDdl_07 = $.trim($("#<%= cboPedidoMotivo.ClientID %>").val());

            var strTxt_01 = $.trim($("#<%= txtCant.ClientID %>").val());
            var strTxt_02 = $.trim($("#<%= txtActRemR.ClientID %>").val());
            var strTxt_03 = $.trim($("#<%= txtDescripcion.ClientID %>").val());

            /*VALORES DE CBO*/

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

            var cboInsumo = document.getElementById('<%= cboInsumo.ClientID %>');
            if (strDdl_05 == "0") {
                cboInsumo.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboInsumo.style.border = "1px solid #888888";
            }

            var cboTipoPed = document.getElementById('<%= cboTipoPed.ClientID %>');
            if (strDdl_06 == "0") {
                cboTipoPed.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboTipoPed.style.border = "1px solid #888888";
            }

            var cboPedidoMotivo = document.getElementById('<%= cboPedidoMotivo.ClientID %>');
            if (strDdl_07 == "0") {
                cboPedidoMotivo.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                cboPedidoMotivo.style.border = "1px solid #888888";
            }

            /*VALORES DE TXT*/
            var txtCant = document.getElementById('<%= txtCant.ClientID %>');
            if (strTxt_01.length == 0) {
                txtCant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtCant.style.border = "1px solid #888888";
            }

            var txtActRemR = document.getElementById('<%= txtActRemR.ClientID %>');
//            if (strTxt_02.length == 0) {
//                txtActRemR.style.border = "1px solid Red";
//                bolValida = false;
//            }
//            else {
//                txtActRemR.style.border = "1px solid #888888";
//            }

            //            var txtDescripcion = document.getElementById('<%= txtDescripcion.ClientID %>');
            //            if (strTxt_03.length == 0) {
            //                txtDescripcion.style.border = "1px solid Red";
            //                bolValida = false;
            //            }
            //            else {
            //                txtDescripcion.style.border = "1px solid #888888";
            //            }

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
            var letras = "áéíóúÁÉÍÓÚñÑ";
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

        function validarCantidad(txt) {

            if (txt.value.trim() != '') {
                var valor = parseInt(txt.value)
                if (valor <= 0) {
                    txt.value = '';
                    alert('La cantidad del pedido debe ser mayor a cero.');
                    return;
                }

                txt.value = valor;
            }

        }

    </script>
   
    <style type="text/css">
        .style4
        {
            width: 472px;
            text-align: center;
        }
        .style26
        {
            text-align: center;
        }
        #dtpFecha
        {
            width: 85px;
        }
        .style37
        {
            width: 139px;
            color: #FFFFFF;
        }
        .style40
        {
            width: 193px;
        }
        .style46
        {
            text-align: center;
        }
        .style51
        {
            text-align: center;
            width: 377px;
            height: 19px;
        }
        .style52
        {
            text-align: center;
            height: 19px;
        }
        #TablaEstadoPedido
        {
            width: 842px;
        }
        .style53
        {
            width: 4px;
            }
        .style54
        {
            text-align: right;
            width: 248px;
        }
        .style55
        {
            width: 546px;
        }
        .style56
        {
            width: 499px;
            text-align: right;
        }
        .style57
        {
            width: 454px;
        }
        .style59
        {
            width: 856px;
            text-align: right;
        }
        .style64
        {
            text-align: center;
            width: 130px;
        }
        .style65
        {
            width: 377px;
        }
        .style67
        {
            width: 125px;
        }
        .style70
        {
            width: 147px;
        }
        .style71
        {
            text-align: center;
            width: 210px;
        }
        .style72
        {
            width: 210px;
        }
        .style73
        {
            text-align: center;
            width: 377px;
        }
        .style74
        {
            text-align: center;
            width: 378px;
        }
        </style>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<Fecha:Fecha ID="dtpFecFin" runat="server" />--%>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr><td><h2>
            <asp:Label ID="lblTitPedidos" runat="server" Text="PEDIDOS DE INSUMOS"></asp:Label>
            </h2></td></tr>
    </table>
    <%--Opciones--%>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td align ="left">
                <div id="tabs">
                    <ul>
                    <li><a href="#tab-1">Consulta</a></li>
                    <li><a href="#tab-2">Registro</a></li>
                    </ul>
                    <div id="tab-1">
                    <asp:UpdatePanel runat="server" ID="updConsulta" updatemode="Conditional">
                    <ContentTemplate>

                        <table style="width: 889px">                                    
                            <tr>
                                    <td class="style64"><asp:Label ID="lblFecInicio" runat="server" Text="Fec. Inicio" 
                                            Font-Size="10pt"></asp:Label></td>

                                    <td class="style64">
                                        <asp:Label ID="lblFecFin" runat="server" Font-Size="10pt" Text="Fec. Fin"></asp:Label>
                                    </td>

                                    <td class="style26">
                                        <asp:Label ID="lblNroActaRem" runat="server" Font-Size="10pt" 
                                            Text="N° Acta Remisión"></asp:Label>
                                    </td>
                                    <td class="style26">
                                        <asp:Label ID="lblCodPedidoC" runat="server" Font-Size="10pt" 
                                            Text="Código Pedido"></asp:Label>
                                    </td>

                                    <td class="style26">
                                        <asp:Label ID="lblEstadoC" runat="server" Font-Size="10pt" Text="Estado"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <asp:Label ID="lblInsumo0" runat="server" Font-Size="10pt" 
                                            style="text-align: center" Text="Insumo"></asp:Label>
                                    </td>

                                    <td class="style26">
                                        &nbsp;</td>

                            </tr>

                            <tr>
                                    <td class="style64">
                                        <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                        <%--<Fecha:Fecha ID="dtpFecInicio" runat="server" />--%>
                                    </td>

                                    <td class="style64">
                                        <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        <%--<Fecha:Fecha ID="dtpFecFin" runat="server" />--%>
                                    </td>

                                    <td class="style26">
                                        <asp:TextBox ID="txtActRemC" runat="server" Font-Size="10pt" MaxLength="12"  onkeypress="return isLetraNumeroDoc(event)"
                                            style="text-align: center" Width="140px" CssClass="txtLetra"></asp:TextBox>
                                    </td>
                                    <td class="style26">
                                        <asp:TextBox ID="txtCodPedidoC" runat="server" Font-Size="10pt" MaxLength="12" 
                                            onkeypress="return isLetraNumeroDoc(event)" style="text-align: center" 
                                            Width="140px" CssClass="txtLetra"></asp:TextBox>
                                    </td>

                                    <td class="style26">
                                        <asp:DropDownList ID="cboPedidoEstadoC" runat="server" Font-Size="10pt" 
                                            Height="22px" Width="144px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style53">
                                        <asp:DropDownList ID="cboInsumoC" runat="server" Font-Size="10pt" Height="22px" 
                                            Width="170px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style26">
                                        &nbsp;</td>
                            </tr>
                         </table>

                        <%--Opciones--%>
                        <table>
                            <tr>
                                <td>
                                    <toolbar:toolbarcontent ID="ctrlToolBar1" runat="server"></toolbar:toolbarcontent>
                                </td>
                            </tr>
                        </table>



                        <%--Opciones--%>
                        <table>
<%--                        <tr><td><Label:Validation ID="ctrlValidacion" runat="server"/></td></tr>
                        <tr>--%>
                        <tr>
                            <td class="style40"  style="width: 594px">
                                <Label:Validation ID="ctrlValidacion" runat="server" />
                            </td>
                        </tr>
                        </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="updGrillaConsulta" runat="server" updatemode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdPedido" runat="server" AlternatingRowStyle-CssClass="alt" 
                                                AutoGenerateColumns="False" CssClass="mGrid" Font-Size="10pt" GridLines="None" 
                                                onrowcommand="grdPedido_RowCommand" 
                                                OnRowDataBound="grdPedido_RowDataBound" 
                                                Width="900px">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="pedi_iPedidoId" HeaderStyle-CssClass="ColumnaOculta" 
                                                        HeaderText="pedi_iPedidoId" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sOficinaConsularIdOrigen" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sOficinaConsularIdOrigen" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OC Origen" HeaderText="Origen" ItemStyle-Width="60">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sBovedaTipoIdOrigen" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sBovedaTipoIdOrigen" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sBodegaOrigenId" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sBodegaOrigenId" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sOficinaConsularIdDestino" 
                                                        HeaderStyle-CssClass="ColumnaOculta" 
                                                        HeaderText="pedi_sOficinaConsularIdDestino" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OC Destino" HeaderText="Destino" 
                                                        ItemStyle-Width="60">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sBovedaTipoIdDestino" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sBovedaTipoIdDestino" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sBodegaDestinoId" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sBodegaDestinoId" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sPedidoTipoId" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sPedidoTipoId" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Tipo" HeaderStyle-CssClass="ColumnaOculta" 
                                                        HeaderText="Tipo" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sPedidoMotivoId" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sPedidoMotivoId" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" ItemStyle-Width="150">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_cPedidoCodigo" HeaderText="Pedido" 
                                                        ItemStyle-Width="75">
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sInsumoTipoId" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_sInsumoTipoId" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Insumo" HeaderText="Insumo" ItemStyle-Width="120">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha Pedido" 
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60" 
                                                        DataFormatString="{0:MMM-dd-yyyy}">
                                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="Fecha_Atendido" HeaderText="Fecha ATendido" 
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60" 
                                                        DataFormatString="{0:MMM-dd-yyyy}">
                                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="pedi_vActaRemision" HeaderText="N° Acta" 
                                                        ItemStyle-Width="75">
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_vDescripcion" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_vDescripcion" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_ICantidad" HeaderText="Cant" 
                                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40">
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_sEstadoId" HeaderStyle-CssClass="ColumnaOculta" 
                                                        HeaderText="pedi_sEstadoId" ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-Width="70">
                                                        <ItemStyle Width="70px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pedi_vObservacion" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_vObservacion" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-Width="35">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnVer" runat="server" 
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" 
                                                                ImageUrl="../Images/img_gridbuscar.gif" tooltip="Ver Detalle" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="35px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-Width="35">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnEditar" runat="server" 
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" 
                                                                ImageUrl="../Images/img_grid_modify.png" tooltip="Editar Pedido" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="35px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Atender" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-Width="35">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnAtender" runat="server" 
                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                CommandName="Atender" ImageUrl="../Images/img_16_order_attend.png" 
                                                                tooltip="Atender Pedido" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="35px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="pedi_dFechaModificacion" 
                                                        HeaderStyle-CssClass="ColumnaOculta" HeaderText="pedi_vObservacion" 
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>

                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <PageBarContent:PageBar ID="ctrlPaginador" runat="server" 
                                        OnClick="ctrlPaginador_Click" />
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>

                    <div id="tab-2" style="width: 934px; height: 439px;">
                        <asp:UpdatePanel ID="UpdMantenimiento" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                        <%--Opciones--%>
                        <table align="left" style="width: 1053px">
                            <tr>
                            <td><toolbar:toolbarcontent ID="ctrlToolBar2" runat="server"></toolbar:toolbarcontent></td>
                            </tr>
                            <tr>
                            <td>
                                <asp:Label ID="lblValidacion" runat="server" CssClass="hideControl" 
                                    ForeColor="Red" Text="Falta validar algunos campos."></asp:Label>
                            </td>
                        </tr>
                        </table>
                        <table>
                        
                        </table>

                        <br />

                        <table style="border-left: 1px solid #800000; border-right: 1px solid #800000; border-top: 1px solid #800000; 
                            width: 929px; border-bottom-style: none; border-bottom-color: #800000;">

                        <tr>
                            <td style="text-align: right" class="style65">
                                &nbsp;</td>
                            <td style="text-align: right">
                                &nbsp;</td>
                            <td class="style70">
                                &nbsp;</td>
                            <td style="text-align: right">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="10pt" 
                                    Text="Nro. Pedido:"></asp:Label>
                            </td>
                            <td class="style67">
                                <asp:Label ID="lblNroPedido" runat="server" Font-Bold="True" Font-Size="10pt" 
                                    ForeColor="#4E102E" style="font-size: 10pt; text-align: right;"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td bgcolor="#4E102E" class="style26" colspan="5">
                                <asp:Label ID="lblOrigen" runat="server" CssClass="style37" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="8pt" Font-Underline="True" ForeColor="White" 
                                    Text="ORIGEN"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="style51">
                                <asp:Label ID="lblOfConsularO" runat="server" Text="Of. Consular" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td class="style52" colspan="2">
                                <asp:Label ID="lblTipoBovedaO" runat="server" Text="Tipo Bóveda" 
                                    Font-Size="10pt" Width="205px"></asp:Label>
                            </td>
                            <td class="style52" colspan="2">
                                <asp:Label ID="lblBovedaOrigen" runat="server" Text="Bóveda" Font-Size="10pt" 
                                    Width="310px"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="style73">
                                <uc1:ctrlOficinaConsular ID="cboMisionConsO" runat="server" Enabled="False" />
                                <asp:Label ID="lblObligatorio1" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                            </td>

                            <td  class="style26" colspan="2">
                                <asp:DropDownList ID="cboTipoBovedaO" runat="server" Width="195px" 
                                    onselectedindexchanged="cboTipoBovedaO_SelectedIndexChanged" 
                                    AutoPostBack="True" Font-Size="10pt" Height="22px" Enabled="False"></asp:DropDownList>
                                <asp:Label ID="lblObligatorio0" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                            </td>

                            <td colspan="2" style="text-align: center">
                                <asp:DropDownList ID="cboBovedaO" runat="server" Width="300px" Font-Size="10pt" 
                                    Height="22px" Enabled="False"></asp:DropDownList>
                                <asp:Label ID="lblObligatorio" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                            </td>
                        </tr>

                        </table>

                        <table style="border: 1px solid #800000; width: 929px;">

                        <tr>
                            <td class="style26" colspan="3" bgcolor="#4E102E">
                                <asp:Label ID="lblDestino" runat="server" CssClass="style37" Font-Bold="True" 
                                    Font-Names="Arial" Font-Size="8pt" Font-Underline="True" 
                                    ForeColor="White" Text="DESTINO"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="style74">
                                <asp:Label ID="lblOfConsularD" runat="server" Text="Of. Consular" 
                                    Font-Size="10pt"></asp:Label>
                            </td>
                            <td class="style71">
                                <asp:Label ID="lblTipoBovedaD" runat="server" Text="Tipo Bóveda" 
                                    Font-Size="10pt" Width="205px"></asp:Label>
                            </td>
                            <td class="style46">
                                <asp:Label ID="lblBovedaDestino" runat="server" Text="Bóveda" Font-Size="10pt" 
                                    Width="310px"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="style74">
                                <uc1:ctrlOficinaConsular ID="cboMisionConsD" runat="server" 
                                    EnableTheming="True" />
                                <asp:Label ID="lblObligatorio2" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                <br />
                            </td>

                            <td class="style72">
                                    <asp:DropDownList ID="cboTipoBovedaD" runat="server" Width="195px" 
                                    onselectedindexchanged="cboTipoBovedaD_SelectedIndexChanged" 
                                    AutoPostBack="True" Font-Size="10pt" Height="22px" Enabled="False"></asp:DropDownList>
                                    <asp:Label ID="lblObligatorio3" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                            </td>

                            <td style="text-align: center">
                                <asp:DropDownList ID="cboBovedaD" runat="server" Width="300px" Font-Size="10pt" 
                                    Height="22px" Enabled="False"></asp:DropDownList>
                                <asp:Label ID="lblObligatorio4" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                            </td>                                                            
                        </tr>

                        </table>

                        <table style="border-left: 1px solid #800000; border-right: 1px solid #800000; border-top: 1px solid #800000;  
                            border-bottom: 1px solid #800000; width: 929px; border-bottom-color: #800000;">                                        
                            
                            <tr>
                                <td colspan="6" style="text-align: center">
                                <asp:Label ID="Label3" runat="server" ForeColor="#4E102E" style="font-weight: 700" 
                                        Text="DATOS DE PEDIDO" Font-Size="10pt"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td class="style59">
                                    <asp:Label ID="lblInsumo" runat="server" Text="Insumo :" Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style57">
                                    <asp:DropDownList ID="cboInsumo" runat="server" Width="215px" Height="22px" 
                                        Font-Size="10pt"></asp:DropDownList>
                                    <asp:Label ID="lblObligatorio5" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>

                                <td class="style56">
                                    <asp:Label ID="lblTipoPedido" runat="server" Text="Tipo de Pedido :" 
                                        Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style55">
                                    <asp:DropDownList ID="cboTipoPed" runat="server" Width="239px" Height="22px" 
                                        Font-Size="10pt" onselectedindexchanged="cboTipoPed_SelectedIndexChanged" 
                                        AutoPostBack="True" Enabled="False"></asp:DropDownList>
                                    <asp:Label ID="lblObligatorio6" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>

                                <td class="style54">
                                    <asp:Label ID="lblCantidad" runat="server" Text="Cantidad :" 
                                        Font-Size="10pt"></asp:Label></td>
                                <td class="style40">
                                    <asp:TextBox ID="txtCant" runat="server" Height="20px" MaxLength="7"  onkeypress="return isNumberKey(event)"
                                        CssClass="campoNumero" Width="57px" Font-Size="10pt" TabIndex="0" onblur="return validarCantidad(this)" ></asp:TextBox>
                                    <asp:Label ID="lblObligatorio7" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>
                            </tr>

                            <tr>

                                <td class="style59">
                                    <asp:Label ID="lblNroActaRemisionR" runat="server" Text="N° Acta Remisión :" 
                                        Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style57">
                                    <asp:TextBox ID="txtActRemR" runat="server" Width="150px" MaxLength="12"  onkeypress="return isLetraNumeroDoc(event)"
                                        Font-Size="10pt" CssClass="txtLetra"></asp:TextBox>                                        
                                    <asp:Label ID="lblObligatorio8" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>   
                                     
                                <td class="style56">
                                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripción :" 
                                        Font-Size="10pt" Visible="False"></asp:Label>
                                </td>

                                <td class="style55">
                                    <asp:TextBox ID="txtDescripcion" runat="server" Width="233px" MaxLength="150"  onkeypress="return isLetraNumero(event)"
                                        Font-Size="10pt" Visible="False"></asp:TextBox>
                                </td>
                                        
                                <td class="style54">
                                    &nbsp;<asp:Label ID="lblFecha" runat="server" Text="Fecha :" Visible="False" 
                                        Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style40">
                                    <asp:TextBox ID="dtpFecha"
                                        runat="server"
                                        Width="100px" Enabled="False" Visible="False" />
<%--                                    <uc3:CalendarExtender ID="ceFecha"
                                                        runat="server" 
                                                        TargetControlID="dtpFecha"
                                                        PopupButtonID="imgCal3" />--%>
                                    <%--<asp:ImageButton ID="imgCal3" 
                                                    runat="server" 
                                                    ImageUrl="../Images/img_16_calendar.png"
                                                    ImageAlign="AbsMiddle" Visible="False" />--%>
                                    <%--<Fecha:Fecha ID="dtpFecha" runat="server" Visible="False" />--%>
                                </td>
                                        
                            </tr>

                            <tr>
                                            
                                <td class="style59">
                                    &nbsp;<asp:Label ID="lblPedidoEstado" runat="server" Text="Estado :" 
                                        Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style57">
                                    <asp:DropDownList ID="cboPedidoEstadoR" runat="server" Width="150px" 
                                        Height="22px" Font-Size="10pt"></asp:DropDownList>
                                    <asp:Label ID="lblObligatorio11" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>

                                <td rowspan="2" valign="top" class="style56">
                                    <asp:Label ID="lblObservación" runat="server" Text="Observación :" 
                                        Font-Size="10pt"></asp:Label>
                                </td>
                                <td colspan="3" rowspan="2" valign="top">
                                    <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine"  onkeypress="return isLetraNumero(event)"
                                        Width="389px" Height="37px" MaxLength="150" Font-Size="10pt" 
                                        CssClass="txtLetra"></asp:TextBox>
                                </td>

                            </tr>

                            <tr>
                                        
                                <td class="style59">
                                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Motivo :" Font-Size="10pt"></asp:Label>
                                </td>

                                <td class="style57">
                                    <asp:DropDownList ID="cboPedidoMotivo" runat="server" Height="22px" 
                                        Width="215px" Font-Size="10pt"></asp:DropDownList>
                                    <asp:Label ID="lblObligatorio12" runat="server" style="color: #FF0000" Text="*"></asp:Label>
                                </td>

                            </tr>

                        </table>   
                       
                        <br />

                        <table id="TablaEstadoPedido" runat="server" align="center">
                            <tr>                                       
                                <td class="style4">
                                    <%--<asp:UpdatePanel ID="UpdHistorico" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                    
                                    <asp:GridView ID="grdEstadoPedido" 
                                            CssClass="mGrid"
                                            AlternatingRowStyle-CssClass="alt"
                                            runat="server"
                                            AutoGenerateColumns="False"
                                            OnRowDataBound="grdEstadoPedido_RowDataBound"
                                            GridLines="None" Width="800px" Font-Size="10pt">

                                    <AlternatingRowStyle CssClass="alt">
                                    </AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="pehi_iPedidoHistoricoId"  HeaderText="pehi_iPedidoHistoricoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle CssClass="ColumnaOculta" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="pehi_iPedidoId"  HeaderText="pehi_iPedidoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle CssClass="ColumnaOculta" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width=200 
                                            DataFormatString="{0:MMM-dd-yyyy}">
                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="pehi_sEstadoId" HeaderText="pehi_sEstadoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle CssClass="ColumnaOculta" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ></asp:BoundField>
                                        <asp:BoundField DataField="pehi_sMotivoId" HeaderText="pehi_sMotivoId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                            <HeaderStyle CssClass="ColumnaOculta" />
                                            <ItemStyle CssClass="ColumnaOculta" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Motivo" HeaderText="Motivo"></asp:BoundField>
                                        <asp:BoundField DataField="pehi_vObservacion" HeaderText="Observación"></asp:BoundField>

                                    </Columns>                                    
                                </asp:GridView>
                                    
                                    <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
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

    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
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
