<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmServicioConsular.aspx.cs"
    Inherits="SGAC.WebApp.Colas.FrmServiciosConsulares" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

    <style type="text/css">
        .tValida
        {
            background-color: #F2F1C2;
            border-color: Yellow; /*#6E4E1B;*/
            color: #4B4F5E;
            height: 15px;
            background-image: url('../../Images/img_16_warning.png');
            background-repeat: no-repeat;
            background-position: 8px 2px;
            width: 100%;
        }
        .lblValida
        {
            margin-left: 25px;
        }
        
        #lblConfirm
        {
            background-color: #d1efc2;
            width: 39%;
            height: 20px;
            border: 1px solid #ffffff;
            padding: 2px 2px 2px 2px;
            display: none;
        }
        
        #lblWarning
        {
            width: 39%;
            padding: 2px 2px 2px 2px;
            height: 20px;
            border: 1px solid #ffffff;
            background-color: #F2F1C2;
            display: none;
        }
    </style>
 
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function EditarRegistro() {
            var valor = true;
            var btnsubserv = document.getElementById('<%= btnsubservicios.ClientID %>');
            if (btnsubserv.disabled == true) {
                valor = false;
            }

            return valor;
        }

        function EliminarRegistro() {
            var valor = true;
            var btnsubserv = document.getElementById('<%= btnsubservicios.ClientID %>');
            if (btnsubserv.disabled == true) {
                valor = false;
            }

            if (valor) {
                valor = confirm("Desea eliminar un registro");
            }
            return valor;
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


        $(function () {

            $('#tabs').tabs();
           
        });

        function Validar() {
            var bolValida = true;
            var strDescripcion = $.trim($("#<%= txtDesc.ClientID %>").val());
            var strNumOrden = $.trim($("#<%= txtNumOrd.ClientID %>").val());

            if (strDescripcion.length == 0) {
                document.getElementById('<%= txtDesc.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtDesc.ClientID %>').style.border = "1px solid #888888";
            }

            if (strNumOrden.length == 0) {
                document.getElementById('<%= txtNumOrd.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtNumOrd.ClientID %>').style.border = "1px solid #888888";
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

        function validarDetalle() {
            var bolValida = true;
            var strDescripcion = $.trim($("#<%= txtDescSub.ClientID %>").val());
            var strNumOrden = $.trim($("#<%= txtNumOrdSub.ClientID %>").val());

            if (strDescripcion.length == 0) {
                $("#<%=lblDescripcionDetalle.ClientID %>").show();
                bolValida = false;
            }
            else {
                $("#<%=lblDescripcionDetalle.ClientID %>").hide();
            }
            if (strNumOrden.length == 0) {
                $("#<%=lblNumeroOrden.ClientID %>").show();
                bolValida = false;
            }
            else {
                $("#<%=lblNumeroOrden.ClientID %>").hide();
            }

            return bolValida;
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

    </script>
    <div>
        <table style="width: 90%; border-spacing: 0px;" align="center">
            <tr>
                <td>
                    <h2>
                        ADMINISTRACIÓN DE SERVICIO CONSULAR</h2>
                </td>
            </tr>
        </table>
        <table style="width: 90%; border-spacing: 0px;" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">Consulta</a></li>
                            <li><a href="#tab-2">Registro</a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="12%">
                                                Oficina Consular:
                                            </td>
                                            <td width="88%">
                                                <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="400px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server" OnClick="ctrlToolBarConfirm_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdVentanilla" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                            Width="870px" OnRowCommand="grdVentanilla_RowCommand" OnSelectedIndexChanged="grdVentanilla_SelectedIndexChanged"
                                                            DataKeyNames="serv_sOficinaConsularId">
                                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Codigo" DataField="serv_sServicioId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Descripción" DataField="serv_vDescripcion">
                                                                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="N° Orden" DataField="serv_IOrden">
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="serv_sTipo" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Tipo"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="serv_vServicioDireccion" HeaderStyle-CssClass="ColumnaOculta" HeaderText="direccion"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Left" />
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
                                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                    CssClass="hideControl" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Oficina Consular:
                                            </td>
                                            <td>
                                                <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Width="410px" />
                                                <asp:TextBox ID="TxtId" runat="server" Visible="False" Width="86px"></asp:TextBox>
                                                <asp:TextBox ID="txtcod" runat="server" MaxLength="2" onkeypress="return validatenumber(event)"
                                                    Visible="False" Width="70px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Descripción:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDesc" runat="server" Width="600px" MaxLength="100" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" onkeypress="return isLetraNumero(event)"></asp:TextBox>
                                                <asp:Label ID="lblDescripcion" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                N° Orden:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumOrd" runat="server" Width="50px" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                <asp:Label ID="lblNumeroOrden" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Lugar de atención:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDireccionServicio" runat="server" Width="600px" MaxLength="100" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellspacing="0">
                                        <tr>
                                            <td colspan="2">
                                                <table width="505px">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="GridDetalle" runat="server" AlternatingRowStyle-CssClass="alt"
                                                                AutoGenerateColumns="False" CssClass="mGrid" GridLines="None" OnRowCommand="GridDetalle_RowCommand"
                                                                SelectedRowStyle-CssClass="slt" Width="870px" ShowHeaderWhenEmpty="True">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="serv_sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                                                        HeaderText="OficinaC" ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_sServicioId" HeaderText="Codigo" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_sServicioIdCab" HeaderStyle-CssClass="ColumnaOculta"
                                                                        HeaderText="IdPadre" ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_vDescripcion" HeaderText="Descripción SubServicio">
                                                                        <ItemStyle Width="500px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_IOrden" HeaderText="N° Orden">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_cEstado" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Activo"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_sTipo" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Tipo"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="serv_vServicioDireccion" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Direccion"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEditarSub" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                CommandName="Editar" ImageUrl="../Images/img_grid_modify.png" ToolTip="Editar"
                                                                                OnClientClick="return EditarRegistro();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnElinarSub" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar"
                                                                                OnClientClick="return EliminarRegistro();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="slt" />
                                                                <RowStyle Font-Names="Arial" Font-Size="11px"></RowStyle>
                                                                <EmptyDataTemplate>
                                                                    <table id="tbSinDatos">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td width="10%">
                                                                                    <asp:Image runat="server" ID="imgWarning" ImageUrl="../Images/img_16_warning.png" />
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
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <PageBarContent:PageBar ID="ctrlPaginadorDetalle" runat="server" OnClick="ctrlPaginadorDetalle_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:Button ID="btnsubservicios" runat="server" OnClick="btnsubservicios_Click" Text="SubServicios"
                                            CssClass="btnGeneral" Enabled="False" />
                                    </div>
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hd_Detalle" runat="server" />
                                                <cc1:ModalPopupExtender ID="ModalPanel_Cab" runat="server" TargetControlID="hd_Detalle"
                                                    PopupControlID="Panel_Cab" Drag="true" BackgroundCssClass="modalBackground">
                                                </cc1:ModalPopupExtender>
                                                <div align="left">
                                                    <asp:Panel ID="Panel_Cab" runat="server" CssClass="st_PanelPopupAjax01" HorizontalAlign="Left"
                                                        BackColor="#FFFFFF" BorderColor="#000000" BorderStyle="Groove" Width="500px"
                                                        Style="display: none">
                                                        <table cellspacing="0" width="100%">
                                                            <tr>
                                                                <td class="encebezadotabla-2" width="97%">
                                                                    INGRESO DE SUB SERVICIO
                                                                </td>
                                                                <td align="right" width="3%" valign="top">
                                                                    <asp:ImageButton ID="imgCerrar" runat="server" Height="17" ImageAlign="Top" ImageUrl="../Images/img_cerrar.gif"
                                                                        ToolTip="Cerrar Ventana" Visible="true" Width="17" OnClick="imgCerrar_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td width="100px" colspan="2">
                                                                    <asp:Label ID="lblValidarDetalle" runat="server" Text="Falta validar algunos campos."
                                                                        CssClass="hideControl" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100px">
                                                                    <asp:Label ID="lblsubservicio" runat="server" ForeColor="Black" Text="Descripción:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescSub" runat="server" MaxLength="100" onBlur="conMayusculas(this)"
                                                                        CssClass="txtLetra" onkeypress="return isLetraNumero(event)" Width="95%"></asp:TextBox>
                                                                    <asp:Label ID="lblDescripcionDetalle" runat="server" Text="*" CssClass="lblVal"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100px">
                                                                    <asp:Label ID="lblordensub" runat="server" ForeColor="Black" Text="Nro Orden:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNumOrdSub" runat="server" MaxLength="3" Width="50px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                    <asp:Label ID="lblOrdenDetalle" runat="server" Text="*" CssClass="lblVal"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100px">
                                                                    <asp:Label ID="lbldireccionsubservicio" runat="server" ForeColor="Black" Text="Lugar de atención:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                     <asp:TextBox ID="txtDireccionSubServicio" runat="server" Width="95%" MaxLength="100" onBlur="conMayusculas(this)"
                                                                            CssClass="txtLetra" ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td width="160px">
                                                                    <asp:TextBox ID="TxtIdDesc" runat="server" Visible="False" Width="86px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btngrabarSubServicio" runat="server" OnClick="btngrabarSubServicio_Click"
                                                                        CssClass="btnGeneral" Text="Adicionar" Width="97px" OnClientClick="validarDetalle()" />
                                                                    <asp:Button ID="btnModificarSub" runat="server" CssClass="btnEdit" OnClick="btnModificarSub_Click"
                                                                        Text="    Modificar" />
                                                                    <asp:Button ID="btnCancelarSub" runat="server" CssClass="btnUndo" OnClick="btnCancelarSub_Click"
                                                                        Text="    Cancelar" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="lblHide"></asp:Label>
    </div>
    <script type="text/javascript">

        function validatenumber(event) {
            return validatedecimal(event);
        }

        function showpopupother(typeMsg, title, msg, resize, height, width) {
            showdialog(typeMsg, title, msg, resize, height, width);
        }

        function cambiarPestaña(estado) {
            if (estado == 1) {
                $('#tabs li a')[1].click();
            } else {
                $('#tabs li a')[0].click();
            }
        }

        function mostrarMensajeConfirmacion(tipo) {
            $("#lblConfirm").css("display", "block");
            $("#lblWarning").css("display", "none");
            $('#tabs li a')[0].click();

            if (tipo == '1') {
                /*[NUEVO]*/
                $("#lblConfirm").html('<img class="imgIcono" src="../Images/img_16_success.png" />El registro del servicio se ha registrado correctamente.');
            } else if (tipo == '2') {
                /*[ACTUALIZAR]*/
                $("#lblConfirm").html('<img class="imgIcono" src="../Images/img_16_success.png" />El registro del servicio se ha actualizado correctamente.');
            } else {
                /*[ELIMINAR]*/
                $("#lblConfirm").html('<img class="imgIcono" src="../Images/img_16_success.png" />El registro del servicio se ha eliminado correctamente.');
            }
        }

        function mostrarMensajeError(tipo) {
            $("#lblConfirm").css("display", "none");
            $("#lblWarning").css("display", "block");
            $('#tabs li a')[0].click();

            if (tipo == '1') {
                /*[NUEVO]*/
                $("#lblWarning").html('<img class="imgIcono" src="../Images/img_16_warning.png" />Advertencia, no se logro registrar el servicio intentarlo nuevamente.');
            } else if (tipo == '2') {
                /*[ACTUALIZAR]*/
                $("#lblWarning").html('<img class="imgIcono" src="../Images/img_16_warning.png" />Advertencia, no se logro actualizar el servicio intentarlo nuevamente.');
            } else {
                /*[ELIMINAR]*/
                $("#lblWarning").html('<img class="imgIcono" src="../Images/img_16_warning.png" />Advertencia, no se logro eliminar el servicio intentarlo nuevamente.');
            }
        }
    </script>
</asp:Content>
