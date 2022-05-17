<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmConfiguracionVentanilla.aspx.cs"
    Inherits="SGAC.WebApp.Colas.FrmConfiguracionVentanilla" %>

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
    
    <script type='text/javascript'>
        function Forzar() {
            __doPostBack('', '');
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function Validar() {
            var bolValida = true;
            var strDescripcion = $.trim($("#<%= txtDescripcion.ClientID %>").val());
            var strNumOrden = $.trim($("#<%= txtNumOrden.ClientID %>").val());

            if (strDescripcion.length == 0) {

                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid #888888";
            }

            if (strNumOrden.length == 0) {
                document.getElementById('<%= txtNumOrden.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtNumOrden.ClientID %>').style.border = "1px solid #888888";
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

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

    </script>
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
        .style1
        {
            width: 672px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function EliminarRegistro(control) {
            var valor = true;

            var row = control.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var customerId = row.cells[0].innerHTML;

            document.getElementById('<%= hd_rowIndex.ClientID %>').value = rowIndex;


            var btnsubserv = document.getElementById('<%= btnAdicionarServicios.ClientID %>');
            if (btnsubserv.disabled) {
                valor = false;
            }

            if (valor) {
                valor = confirm("Desea eliminar un registro");
            }

            return valor;
        }

        function checkEnable(control) {
            var valor = true;
            var btnsubserv = control;
            var btnsubserv = document.getElementById('<%= btnAdicionarServicios.ClientID %>');

            if (btnsubserv.disabled) {

                valor = false;
                if (chk.checked) {
                    chk.checked = true;
                }
                if (!chk.checked) {
                    chk.checked = false;
                }
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

    </script>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    MANTENIMIENTO DE VENTANILLA</h2>
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
                                        <td width="130px">
                                            Oficina Consular :
                                        </td>
                                        <td align="left">
                                            <uc1:ctrlOficinaConsular ID="ddlOficinaConsularConsulta" runat="server" Width="400px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <Label:Validation ID="ctrlValidacionVentanilla" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdVentanilla" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                        SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                        Width="870px" OnRowCommand="grdVentanilla_RowCommand">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="vent_sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                                                HeaderText="sOficinaConsularId" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vent_sVentanillaId" HeaderStyle-CssClass="ColumnaOculta"
                                                                HeaderText="Código" ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vent_vDescripcion" HeaderStyle-Width="1200" HeaderText="Descripción">
                                                                <HeaderStyle Width="1200px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vent_INumeroOrden" HeaderStyle-Width="180" HeaderText="N° Orden">
                                                                <HeaderStyle Width="180px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vent_sSectorId" HeaderStyle-CssClass="ColumnaOculta" HeaderText="vent_sSectorId"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="updMantenimiento" runat="server">
                            <ContentTemplate>
                                <div>
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                </div>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                CssClass="hideControl" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="110px">
                                            Oficina Consular :
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ddlOficinaConsularConsulta2" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="110px">
                                            Descripción
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" Width="346px" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="50"></asp:TextBox>
                                            <asp:Label ID="ValidaDescVent" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="110px">
                                            Sector :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSector" runat="server" Width="350px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="110px">
                                            N° Orden :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumOrden" runat="server" Width="70px" MaxLength="2" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            <asp:Label ID="ValidaNroOrdVent" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <Label:Validation ID="ctrlValidacionVentanilla1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:Button ID="btnAdicionarServicios" runat="server" Text="Mostrar Servicios" CssClass="btnGeneral"
                                        OnClick="btnAdicionarServicios_Click" Width="120px" />
                                    <asp:HiddenField ID="hd_Detalle" runat="server" />
                                    <asp:HiddenField ID="hd_rowIndex" runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalPanel_Servicio" runat="server" TargetControlID="hd_Detalle"
                                        PopupControlID="Panel_Cab" Drag="true" BackgroundCssClass="modalBackground">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="Panel_Cab" runat="server" CssClass="st_PanelPopupAjax01" HorizontalAlign="Left"
                                        BackColor="#FFFFFF" BorderColor="#000000" BorderStyle="Groove" Width="615px"
                                        Style="display: none">
                                        <%--Style="display: none"--%>
                                        <table cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td class="encebezadotabla-2" width="97%">
                                                    SELECCIONAR SERVICIOS
                                                </td>
                                                <td align="right" width="3%" valign="top">
                                                    <asp:ImageButton ID="imgCerrar" runat="server" Height="17" ImageAlign="Top" ImageUrl="../Images/img_cerrar.gif"
                                                        ToolTip="Cerrar Ventana" Visible="true" Width="17" OnClick="imgCerrar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="overflow: auto; left: 0px; width: 615px; top: 0px; height: 500px">
                                            <asp:GridView ID="grvServicios" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                DataKeyNames="serv_sOficinaConsularId,serv_sServicioId,SERVICIO,SUBSERVICIO,serv_IOrden,serv_sTipo"
                                                GridLines="None" OnRowDataBound="grvServicios_RowDataBound" ShowHeaderWhenEmpty="True"
                                                Width="600px">
                                                <Columns>
                                                    <asp:BoundField DataField="serv_sTipo" HeaderStyle-CssClass="ColumnaOculta" HeaderText="sTipo"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Elegir">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" Checked='<%# Convert.ToBoolean(Eval("activo"))%>'
                                                                DataField="activo" Enabled='<%# !Convert.ToBoolean(Eval("activo"))%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Servicio">
                                                        <ItemTemplate>
                                                            <%#Eval("SERVICIO")%>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="500px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Servicio">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblserv_SUBSERVICIO" runat="server" Text='<%# Bind("SUBSERVICIO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="500px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Orden">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblserv_IOrden" runat="server" Text='<%# Bind("serv_IOrden") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <div align="right">
                                            <asp:Button ID="btnAdicionarServicio" runat="server" Text="Adicionar" CssClass="btnGeneral"
                                                OnClick="btnAdicionarServicio_Click" />
                                        </div>
                                    </asp:Panel>
                                </div>
                                <table width="600px">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grvVentanillaServicio" runat="server" AutoGenerateColumns="False"
                                                CssClass="mGrid" GridLines="None" SelectedRowStyle-CssClass="slt" Width="600px"
                                                ShowHeaderWhenEmpty="True" OnRowCommand="grvVentanillaServicio_RowCommand" OnRowDataBound="grvVentanillaServicio_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="vede_sServicioId" HeaderText="Codigo" HeaderStyle-CssClass="ColumnaOculta"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Obligatorio">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkObligatorio" runat="server" Checked='<%# Eval("vede_IObligatorio").ToString() == "1" ? true : false  %>'
                                                                OnClientClick="return EliminarRegistro();" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Servicio">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblserv_vDescripcion" runat="server" Text='<%# Eval("serv_vDescripcion").ToString()  %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Servicio">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblserv_SubSevicio" runat="server" Text='<%# Eval("subServicio").ToString() %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Orden">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblserv_IOrden" runat="server" Text='<%# Bind("serv_IOrden") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnQuitarServicio" runat="server" CommandArgument='<%# Bind("vede_sServicioId") %>'
                                                                CommandName="Eliminar" ImageUrl="../Images/img_grid_delete.png" ToolTip="Eliminar"
                                                                OnClientClick="return EliminarRegistro(this);" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="serv_sTipo" HeaderStyle-CssClass="ColumnaOculta" HeaderText="Tipo"
                                                        ItemStyle-CssClass="ColumnaOculta">
                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                    </asp:BoundField>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="lblHide"></asp:Label>
    <div id="DetServicios">
        <asp:UpdatePanel ID="UpdDetServicios" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="style7">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
