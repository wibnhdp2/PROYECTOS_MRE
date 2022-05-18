<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="frmListadoInsumos.aspx.cs" Inherits="SGAC.WebApp.Almacen.frmListadoInsumos" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlFecha.ascx" TagName="Fecha" TagPrefix="Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    

    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
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

        function validarSoloLetras(txt) {

            var valido = true;

            if (valido == true) {
                var charpos = txt.value.search("[^A-Za-z áéíóúÁÉÍÓÚñÑ1234567890-]");

                if (txt.value.length > 0 && charpos >= 0) {
                    txt.value = '';
                    alert('Solo se permiten letras, números y(o) guiones.');
                }
            }

            return valido;
        }

        function ValidarBuscar() {

            var txtfechaini = $('#<%= txtFecIniAct.FindControl("TxtFecha").ClientID %>').val();
            var txtfechafin = $('#<%= txtFecFinAct.FindControl("TxtFecha").ClientID %>').val();

            if (txtfechaini == "") {
                document.getElementById('<%=txtFecIniAct.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                document.getElementById('<%=txtFecIniAct.FindControl("TxtFecha").ClientID %>').focus();
                alert("¡Debe ingresar una fecha Inicio!");
                return false;
            } else {
                document.getElementById('<%=txtFecIniAct.FindControl("TxtFecha").ClientID %>').style.border = "";
            }
            if (txtfechafin == "") {
                document.getElementById('<%=txtFecFinAct.FindControl("TxtFecha").ClientID %>').style.border = "1px solid Red";
                document.getElementById('<%=txtFecFinAct.FindControl("TxtFecha").ClientID %>').focus();
                alert("¡Debe ingresar una fecha Fin!");
                return false;
            } else {
                document.getElementById('<%=txtFecFinAct.FindControl("TxtFecha").ClientID %>').style.border = "";
            }

            return true;
        }

        function ValidarDarBaja() {
            var txtMotivo = $('#<%= txtMotivo.ClientID %>').val();
            if (txtMotivo == "") {
                document.getElementById('<%=txtMotivo.ClientID %>').style.border = "1px solid Red";
                document.getElementById('<%=txtMotivo.ClientID %>').focus();
                return false;
            }
            else { document.getElementById('<%=txtMotivo.ClientID %>').style.border = ""; }
            return true;
        }
        function OcultarMostrarFechas() {
            if ($('#<%= chkOcultar.ClientID %>').prop('checked')) {
                $('.ocultar').show();
            }
            else {
                $('.ocultar').hide();
            }
        }

    </script>

      

    <style type="text/css">
        #dtpFI
        {
            width: 111px;
        }
        
        #dtpFF
        {
            width: 111px;
        }
        
        #datepicker1
        {
            width: 91px;
            text-align: center;
        }
        #datepicker2
        {
            width: 91px;
            text-align: center;
        }
        
        #datepicker3
        {
            width: 91px;
            text-align: center;
        }
        
        .style3
        {
            text-align: center;
            width: 942px;
        }
        
        #dtpFecIni
        {
            width: 87px;
        }
        #dtpFecFin
        {
            width: 87px;
        }
        
        .style26
        {
            text-align: center;
            width: 921px;
        }
        
        .style28
        {
            color: #800000;
        }
        
        .style37
        {
        }
        
        .style55
        {
        }
        
        .style57
        {
            width: 378px;
        }
        
        .style61
        {
            width: 140px;
            text-align: left;
        }
        .style64
        {
            width: 134px;
            text-align: right;
        }
        .style65
        {
            width: 123px;
            text-align: right;
        }
        
        .style68
        {
            width: 129px;
        }
        
        .style69
        {
            height: 26px;
        }
        .style70
        {
            width: 378px;
            height: 26px;
        }
        .style71
        {
            width: 129px;
            height: 26px;
        }
        
        .style73
        {
            text-align: center;
            background-color: #660033;
        }
        .style75
        {
            width: 882px;
        }
        .style76
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="modalBaja" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopup();return false" runat="server" />
                <span>Motivo de Baja del Insumo</span>
            </div>
            <div class="modal-cuerpo">
                <h3>
                    INGRESE EL MOTIVO</h3>
                <asp:TextBox ID="txtMotivo" CssClass="txtLetra" TextMode="MultiLine" runat="server"
                    Height="80px" Width="350px"></asp:TextBox>
                <asp:HiddenField ID="hInsumoID" runat="server" />
            </div>
            <div class="modal-pie">
                <asp:Button ID="BtnAceptarBaja" CssClass="btnLogin" runat="server" OnClientClick="if(!ValidarDarBaja()) return false;"
                    Text="Aceptar" OnClick="BtnAceptarBaja_Click" />
                <button class="btnLogin" onclick="cerrarPopup(); return false">
                    Cancelar</button>
            </div>
        </div>
    </div>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitListadoInsumos" runat="server" Text="LISTADOS DE INSUMOS" Font-Size="12pt"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table style="width: 87%" align="center">
        <tr>
            <td align="left" class="style75">
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">Consulta</a></li>
                        <li><a href="#tab-2">Detalle</a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table style="width: 844px">
                                    <tr>
                                        <td class="style69">
                                            <asp:Label ID="lblTipoInsumo" runat="server" Font-Size="10pt" Style="text-align: right"
                                                Text="Insumo: " Width="80px"></asp:Label>
                                        </td>
                                        <td class="style70">
                                            <asp:DropDownList ID="cboTipoInsumo" runat="server" Height="22px" Style="margin-left: 0px"
                                                Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style69" align="right">
                                            <asp:Label ID="lblEstado" runat="server" Font-Size="10pt" Style="text-align: left"
                                                Text="Estado: " Width="52px"></asp:Label>
                                        </td>
                                        <td class="style71">
                                            <asp:DropDownList ID="cboEstado" runat="server" Height="22px" Style="margin-left: 0px"
                                                Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="lblCodMov" runat="server" Font-Size="10pt" Style="text-align: right"
                                                Text="Movimiento:" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNroMov" runat="server" CssClass="txtLetra" Font-Size="10pt" MaxLength="12"
                                                onkeypress="return isLetraNumeroDoc(event)" onblur="return validarSoloLetras(this)"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblCodMov0" runat="server" Font-Size="10pt" Style="text-align: right"
                                                Text="Cod. Insumo:" Width="82px"></asp:Label>
                                        </td>
                                        <td class="style57" colspan="2">
                                            <asp:TextBox ID="txtCodInsumoC" runat="server" CssClass="txtLetra" Font-Size="10pt"
                                                MaxLength="13" onkeypress="return isLetraNumeroDoc(event)" onblur="return validarSoloLetras(this)"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                           <%-- <asp:CheckBox ID="chkOcultar" runat="server" 
                                                Text="Buscar Por Fecha" oncheckedchanged="chkOcultar_CheckedChanged" 
                                                AutoPostBack="True" />--%>
                                            <asp:CheckBox ID="chkOcultar" runat="server" Checked="true" Enabled="false"
                                                Text="Buscar Por Fecha" onclick="OcultarMostrarFechas();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                            <td align="right">
                                                <div class="ocultar" style="display:none;">
                                                    <asp:Label ID="lblFecIni" runat="server" Font-Size="10pt" Text="Fec. Inicio:"></asp:Label>
                                                </div>
                                            </td>
                                            <td class="style57">
                                                <div class="ocultar" style="display:none;">
                                                    <SGAC_Fecha:ctrlDate ID="txtFecIniAct" runat="server" />
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div class="ocultar" style="display:none;">
                                                    <asp:Label ID="lblFecFin" runat="server" Font-Size="10pt" Height="20px" Style="text-align: right"
                                                    Text="Fec. Fin:" Width="52px"></asp:Label>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <div class="ocultar" style="display:none;">
                                                    <SGAC_Fecha:ctrlDate ID="txtFecFinAct" runat="server" />
                                                </div>
                                            </td>
                                        
                                    </tr>
                                </table>
                                <table style="width: 844px">
                                    <tr>
                                        <td class="style3" colspan="6" bgcolor="#4E102E">
                                            <asp:Label ID="lblOrigen" runat="server" CssClass="style37" Font-Bold="True" Font-Names="Arial"
                                                Font-Size="10pt" Font-Underline="True" ForeColor="White" Text="UBICACIÓN"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style57">
                                            <asp:Label ID="Label2" runat="server" Font-Size="10pt" Height="16px" Text="Of. Consular:"
                                                Width="85px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEstado0" runat="server" Font-Size="10pt" Style="text-align: left"
                                                Text="Tipo Bóveda: " Width="94px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEstado1" runat="server" Font-Size="10pt" Style="text-align: left"
                                                Text="Bóveda: " Width="99px"></asp:Label>
                                        </td>
                                        <td class="style68">
                                            &nbsp;
                                        </td>
                                        <tr>
                                            <td class="style57">
                                                <uc1:ctrlOficinaConsular ID="cboMisionConsO" runat="server" Width="300px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboTipoBovedaO" runat="server" AutoPostBack="True" Font-Size="10pt"
                                                    OnSelectedIndexChanged="cboTipoBovedaO_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboBovedaO" runat="server" AutoPostBack="True" Font-Size="10pt"
                                                    Width="270px" OnSelectedIndexChanged="cboBovedaO_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="style3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <ToolBar:ToolBarContent ID="ctrlToolBar1" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="btnEjecutar" runat="server" Text="Dar Baja" Style="display: none;"
                                                OnClick="btnEjecutar_Click" />
                                            <table align="left" style="width: 594px; height: 28px; margin-top: 3px;">
                                                <tr>
                                                    <td style="width: 594px" align="left">
                                                        <Label:Validation ID="ctrlValidacion" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="updGrillaConsulta" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grReporteGestion" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alt"
                                                                    AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="Estado" GridLines="None"
                                                                    OnRowDataBound="grReporteGestion_RowDataBound" OnRowCommand="grReporteGestion_RowCommand"
                                                                    Font-Size="8pt" Width="850px">
                                                                    <AlternatingRowStyle CssClass="alt" Width="1000" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="insu_iInsumoId" HeaderText="insu_iInsumoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_iMovimientoId" HeaderText="insu_iMovimientoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_sInsumoTipoId" HeaderText="insu_sInsumoTipoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Insumo" HeaderText="Insumo" ItemStyle-Width="200">
                                                                            <ItemStyle Width="180px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_vPrefijoNumeracion" HeaderText="Prefijo" ItemStyle-Width="80">
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_vCodigoUnicoFabrica" HeaderText="Código" ItemStyle-Width="120">
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="90" DataFormatString="{0:MMM-dd-yyyy}">
                                                                            <ItemStyle Width="160px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" ItemStyle-Width="850">
                                                                            <ItemStyle Width="280px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="insu_sEstadoId" HeaderText="insu_sEstadoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                            ItemStyle-CssClass="ColumnaOculta">
                                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-Width="250">
                                                                            <ItemStyle Width="350px" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderText="">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="imgestado" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BorderStyle="None" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnVer" CommandName="Ver" ToolTip="Ver Detalle" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                    runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Baja" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDesvincular" CommandName="Desvincular" ToolTip="Desvincular"
                                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/unlink.png" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2" style="width: 886px; height: 439px; margin-right: 0px;">
                        <asp:UpdatePanel ID="UpdMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table style="border: 1px solid #800000; width: 882px;">
                                    <tr>
                                        <td class="style73" bgcolor="#4E102E" colspan="5">
                                            <asp:Label ID="lblDestino0" runat="server" CssClass="style37" Font-Bold="True" Font-Names="Arial"
                                                Font-Size="10pt" Font-Underline="True" ForeColor="White" Text="DATOS DE INSUMO"
                                                Width="150px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="style65">
                                            <b>
                                                <asp:Label ID="Label1" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Tipo Insumo :"
                                                    Width="110px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style61">
                                            <b>
                                                <asp:Label ID="lblTipoInsumoH" runat="server" Font-Size="10pt" ForeColor="Black"
                                                    Width="180px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style64">
                                            <b>
                                                <asp:Label ID="Label5" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Estado Actual :"
                                                    Width="110px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style76" itemstyle-width="500">
                                            <b>
                                                <asp:Label ID="lblEstadoInsumoH" runat="server" Font-Size="10pt" ForeColor="Black"
                                                    Width="350px"></asp:Label>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr class="style28">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="style65">
                                            <b>
                                                <asp:Label ID="Label4" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Código Insumo :"
                                                    Width="110px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style61">
                                            <b>
                                                <asp:Label ID="lblCodUnicoH" runat="server" Font-Size="10pt" ForeColor="Black" Width="180px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style64">
                                            <b>
                                                <asp:Label ID="Label7" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Ubicación :"
                                                    Width="110px"></asp:Label>
                                            </b>
                                        </td>
                                        <td class="style76" itemstyle-width="500">
                                            <b>
                                                <asp:Label ID="lblUbicacion" runat="server" ForeColor="Black" Font-Size="10pt" Width="350px"></asp:Label>
                                            </b>
                                        </td>
                                    </tr>
                                    <caption>
                                        <tr class="style28">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="style65">
                                                <b>
                                                    <asp:Label ID="Label3" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Movimiento :"
                                                        Width="110px"></asp:Label>
                                                </b>
                                            </td>
                                            <td class="style61">
                                                <b>
                                                    <asp:Label ID="lblMovimientoH" runat="server" Font-Size="10pt" ForeColor="Black"
                                                        Width="180px"></asp:Label>
                                                </b>
                                            </td>
                                            <td class="style64">
                                                <b>
                                                    <asp:Label ID="Label6" runat="server" Font-Size="10pt" ForeColor="Maroon" Text="Fecha - Hora :"
                                                        Width="110px"></asp:Label>
                                                </b>
                                            </td>
                                            <td class="style76" itemstyle-width="500">
                                                <b>
                                                    <asp:Label ID="LblFechaH" runat="server" Font-Size="10pt" ForeColor="Black" Width="180px"></asp:Label>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr class="style28">
                                            <td class="style55" colspan="5" style="text-align: center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="style4" colspan="5">
                                                <b>
                                                    <asp:Label ID="Lbl_TituloHistorico" runat="server" Font-Size="10pt" ForeColor="Black"
                                                        Style="color: #990033">HISTÓRICO DE MOVIMIENTO DEL INSUMO</asp:Label>
                                                </b>
                                                <div id="Div1" style="width: 818px; height: 323px; overflow: scroll; text-align: left;">
                                                    <asp:GridView ID="grdEstadoInsumo" runat="server" AlternatingRowStyle-CssClass="alt"
                                                        AutoGenerateColumns="False" CssClass="mGrid" OnRowDataBound="grdEstadoInsumo_RowDataBound"
                                                        Font-Size="10pt" GridLines="None" Width="783px">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="inhi_iInsumoHistoricoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                HeaderText="inhi_iInsumoHistoricoId" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="inhi_iInsumoId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="inhi_iInsumoId"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                DataFormatString="{0:MMM-dd-yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" Width="70px"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle HorizontalAlign="Center"  Width="60px"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="inhi_sEstadoId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="inhi_sEstadoId"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="usua_vAlias" HeaderText="Usuario" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="inhi_vObservacion" HeaderText="Observación" ItemStyle-Width="300">
                                                                <ItemStyle Width="450px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Fecha1" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Fecha1"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </caption>
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

    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function Popup(valorInsumo) {
            var myHidden = document.getElementById('<%= hInsumoID.ClientID %>');

            if (myHidden)//checking whether it is found on DOM, but not necessary
            {
                myHidden.value = valorInsumo;
            }
            document.getElementById('modalBaja').style.display = 'block';
        }
        function cerrarPopup() {
            document.getElementById('modalBaja').style.display = 'none';
        }
    </script>

</asp:Content>
