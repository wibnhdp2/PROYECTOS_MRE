<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTelevisorRegistro.aspx.cs"
    Inherits="SGAC.WebApp.Colas.FrmTelevisorRegistro" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();

            $("#datepicker1").datepicker();
            $('#datepicker1').datepicker('option', { dateFormat: 'dd/mm/yy' });

            $("#datepicker2").datepicker();
            $('#datepicker2').datepicker('option', { dateFormat: 'dd/mm/yy' });

            $("#datepicker3").datepicker();
            $('#datepicker3').datepicker('option', { dateFormat: 'dd/mm/yy' });
        });

    </script>
    
    <script type="text/javascript">
       
        function showpopupother(typeMsg, title, msg, resize, height, width) {
            showdialog(typeMsg, title, msg, resize, height, width);
        }

        function conMayusculas(field) {
            field.value = field.value.toUpperCase();
        }

        function Validar() {
            var bolValida = true;

            var strDescripcion = $.trim($("#<%= txtDescripcion.ClientID %>").val());
            var strMarca = $.trim($("#<%= txtMarca.ClientID %>").val());
            var strModelo = $.trim($("#<%= txtModelo.ClientID %>").val());
            var strSerie = $.trim($("#<%= txtSerie.ClientID %>").val());
            var strObservacion = $.trim($("#<%= TxtObservacion.ClientID %>").val());

            if (strDescripcion.length == 0) {
                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtDescripcion.ClientID %>').style.border = "1px solid #888888";
            }

            if (strMarca.length == 0) {
                document.getElementById('<%= txtMarca.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtMarca.ClientID %>').style.border = "1px solid #888888";
            }

            if (strModelo.length == 0) {
                document.getElementById('<%= txtModelo.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtModelo.ClientID %>').style.border = "1px solid #888888";
            }

            if (strSerie.length == 0) {
                document.getElementById('<%= txtSerie.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtSerie.ClientID %>').style.border = "1px solid #888888";
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

        function ValidarTextBox() {
            var strDesc = $.trim($("#<%= txtDescripcion.ClientID %>").val());
            var strMar = $.trim($("#<%= txtMarca.ClientID %>").val());
            var strMod = $.trim($("#<%= txtModelo.ClientID %>").val());
            var strSer = $.trim($("#<%= txtSerie.ClientID %>").val());
            var strObs = $.trim($("#<%= TxtObservacion.ClientID %>").val());

            if (strDesc.length == 0) {
                alert('Ingrese la descripcion del televisor', 'Sistema de Colas');
                return false;
            }

            if (strMar.length == 0) {
                alert('Ingrese la marca del televisor', 'Sistema de Colas');
                return false;
            }
            if (strMod.length == 0) {
                alert('Ingrese el modelo del televisor', 'Sistema de Colas');
                return false;
            }
            if (strSer.length == 0) {
                alert('Ingrese el numero de serie del televisor', 'Sistema de Colas');
                return false;
            }
            if (strObs.length == 0) {
                alert('Ingrese las observaciones del televisor', 'Sistema de Colas');
                return false;
            }
            return confirm('¿Desea guardar el registro?');
        }

    </script>
 

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

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
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    MANTENIMIENTO DE TELEVISORES</h2>
            </td>
        </tr>
    </table>
    <%--Opciones--%>
    <table style="width: 90%;" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">Consulta</a></li>
                        <li><a href="#tab-2">Registro</a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            Oficina Consular :
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <ToolBar:ToolBarContent ID="ctrlToolBar1" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <div align="left">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <Label:Validation ID="ctrlValidacionTelevisor" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdTelevisor" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                            Width="870px" OnSelectedIndexChanged="grdTelevisor_SelectedIndexChanged" OnRowCommand="grdTelevisor_RowCommand"
                                                            DataKeyNames="telv_sOficinaConsularId">
                                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Codigo" DataField="telv_sTelevisorId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Descripción" DataField="telv_vDescripcion">
                                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Marca" DataField="telv_vMarca">
                                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Modelo" DataField="telv_vModelo">
                                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="N° Serie" DataField="telv_vSerie" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Observación" DataField="telv_vCaracteristicas" ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
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
                                                <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click"
                                                    Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-2">
                        <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <ToolBar:ToolBarContent ID="ctrlToolBar2" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                CssClass="hideControl" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Oficina Consular:
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Width="450px" Enabled="True" />
                                            <asp:TextBox ID="TxtId" runat="server" Visible="False">TxtId</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Descripción :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" Width="446px" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="50"></asp:TextBox>
                                            <asp:Label ID="ValidaDesc" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Marca:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMarca" runat="server" Width="446px" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="20"></asp:TextBox>
                                            <asp:Label ID="ValMarca" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Modelo:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtModelo" runat="server" Width="446px" MaxLength="20" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)"></asp:TextBox>
                                            <asp:Label ID="ValModelo" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Nº Serie:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSerie" runat="server" Width="446px" MaxLength="20" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)"></asp:TextBox>
                                            <asp:Label ID="ValSerie" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Observación:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtObservacion" TextMode="multiline" Columns="50" Rows="7" runat="server"
                                                onBlur="conMayusculas(this)" CssClass="txtLetra" onkeypress="return isLetraNumero(event)"
                                                Width="444px" />
                                            <asp:Label ID="ValObservacion" runat="server" CssClass="lblVal">*</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="lblHide"></asp:Label>
</asp:Content>
