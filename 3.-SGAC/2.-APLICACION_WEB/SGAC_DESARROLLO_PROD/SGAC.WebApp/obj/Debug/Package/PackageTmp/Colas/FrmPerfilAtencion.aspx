<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FrmPerfilAtencion.aspx.cs"
    Inherits="SGAC.WebApp.Colas.FrmPerfilesAtencion" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc2" %>

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
        
        function validatenumber(event) {
            return validatedecimal(event);
        }

        function showpopupother(typeMsg, title, msg, resize, height, width) {
            showdialog(typeMsg, title, msg, resize, height, width);
        }

        function ValidarTextBox() {
            var bolValida = true;
            var strDesc = $.trim($("#<%= txtDesc.ClientID %>").val());
            var strCantTick = $.trim($("#<%= txtCanTick.ClientID %>").val());
            var strTiempo = $.trim($("#<%= txtTiemAtencion.ClientID %>").val());

            if (strDesc.length == 0) {
                document.getElementById('<%= txtDesc.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtDesc.ClientID %>').style.border = "1px solid #888888";
            }

            if (strCantTick.length == 0) {
                document.getElementById('<%= txtCanTick.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtCanTick.ClientID %>').style.border = "1px solid #888888";
            }

            if (strTiempo.length == 0) {
                document.getElementById('<%= txtTiemAtencion.ClientID %>').style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                document.getElementById('<%= txtTiemAtencion.ClientID %>').style.border = "1px solid #888888";
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
        
        function validaHora(ctrl) {
            var x = ctrl.value;
            var hora = x.substring(0, 2);
            var minuto = x.substring(3, 5);
            var valor = true;

            if (x.length<5 && x.length>0) {
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

        function isHoraKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = ":"
            var x = ctrl.value;
            var y = x.indexOf(FIND);

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

            if (charCode == 58) {
                if (y != -1 || x.length < 2)
                    return false;
            }
            if (charCode < 48 || charCode > 58)
                return false;

            return true;
        }

    </script>

    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    PERFILES DE ATENCIÓN</h2>
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
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div align="left">
                                    <table width="100%">
                                        <tr>
                                            <td width="140px">
                                                Oficina Consular :
                                            </td>
                                            <td>
                                                <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular1" runat="server" Width="450px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server" OnClick="ctrlToolBar1_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <Label:Validation ID="ctrlValidacionAtencion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdPerfilesAtencion" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                            AutoGenerateColumns="False" GridLines="None" Width="870px" OnSelectedIndexChanged="grdPerfilesAtencion_SelectedIndexChanged"
                                                            OnRowCommand="grdPerfilesAtencion_RowCommand" DataKeyNames="peat_sOficinaConsularId">
                                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Codigo" DataField="peat_IPerfilId" HeaderStyle-CssClass="ColumnaOculta"
                                                                    ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Descripción" DataField="peat_vDescripcion" HeaderStyle-Width="350">
                                                                    <HeaderStyle Width="350px" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Cantidad Ticket" DataField="peat_INumeroTicket">
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Tiempo Atención" DataField="peat_ITiempoAtencion">
                                                                    <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
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
                                            Oficina Consular :
                                        </td>
                                        <td>
                                            <uc2:ctrlOficinaConsular ID="ctrlOficinaConsular2" runat="server" Width="350px" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Descripción:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDesc" runat="server" Width="346px" onBlur="conMayusculas(this)"
                                                CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="50"></asp:TextBox>
                                            <asp:Label ID="lbldescripcionVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cantidad de Tickets:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCanTick" runat="server" Width="80px" MaxLength="4" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            <asp:Label ID="lblCantidadVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tiempo de Atención
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTiemAtencion" runat="server" Width="80px" MaxLength="5" onkeypress="return isHoraKey(this,event)"
                                                onBlur="validaHora(this)"></asp:TextBox>
                                            <asp:Label ID="Label1" runat="server" Text="(hh:mm)"></asp:Label>
                                            <asp:Label ID="lblHoraVal" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>