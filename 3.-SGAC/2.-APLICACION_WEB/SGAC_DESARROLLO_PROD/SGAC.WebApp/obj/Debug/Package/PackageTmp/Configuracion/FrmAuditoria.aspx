<%@ Page Language="C#" UICulture="es-PE" Culture="es-PE" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FrmAuditoria.aspx.cs" Inherits="SGAC.WebApp.Configuracion.Auditoria" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ddlOficina" TagPrefix="ddlOficina" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

    <style type="text/css">
        .lblPaddingLeft
        {
            padding-left: 40px;
            padding-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

            //Previene el postback al hacer enter
            $(function () {
                $(':text').bind('keydown', function (e) {
                    //on keydown for all textboxes
                    if (e.target.className != "searchtextbox") {
                        if (e.keyCode == 13) { //if this is enter key
                            e.preventDefault();
                            return false;
                        } else
                            return true;
                    } else
                        return true;
                });
            });

            //Posibilita desplazamiento con enter entre campos
            $(function () {
                $('input:text:first').focus();
                var $inp = $('input:text');
                $inp.bind('keydown', function (e) {
                    //var key = (e.keyCode ? e.keyCode : e.charCode);
                    var key = e.which;
                    if (key == 13) {
                        e.preventDefault();
                        var nxtIdx = $inp.index(this) + 1;
                        $(":input:text:eq(" + nxtIdx + ")").focus();
                    }
                });
            });
        }
       
    </script>
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloAuditoria" runat="server" Text="Auditoría"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <table width="90%" align="center">
            <tr>
                <td>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">Consulta</a></li>
                            <li><a href="#tab-2">Detalle</a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="120px">
                                                <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td width="190px">
                                                <SGAC_Fecha:ctrlDate ID="txtFecInicio" runat="server" />                                                
                                            </td>
                                            <td width="100px">
                                                <asp:Label ID="lblFecFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecFin" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <ddlOficina:ddlOficina ID="ctrlOficinaConsular" runat="server" Width="548px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUsuario" runat="server" Text="Usuario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUsuario" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFormulario" runat="server" Text="Formulario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFormulario" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Tipo Resultado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoResultado" runat="server" Width="150px" 
                                                    AutoPostBack="True" 
                                                    onselectedindexchanged="ddlTipoResultado_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOperacion" runat="server" Text="Tipo Operación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOperacionCon" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Opciones--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Mensaje Validación--%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Grilla--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvAuditoria" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvAuditoria_RowCommand" OnRowDataBound="gdvAuditoria_RowDataBound">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="audi_dCreaFecha" HeaderText="Fecha">
                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="audi_vOficinaConsular" HeaderText="O.C.">
                                                            <ItemStyle Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="audi_vUsuario" HeaderText="Usuario">
                                                            <ItemStyle Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="form_vNombre" HeaderText="Formulario">
                                                            <ItemStyle Width="140px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="para_vOperacionTipo" HeaderText="Operación">
                                                            <ItemStyle Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="audi_vTablaNombre" HeaderText="Tabla">
                                                            <ItemStyle Width="130px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="audi_vHostName" HeaderText="Hostname">
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>                                                        
                                                        <%--<asp:BoundField DataField="audi_vDireccionIP" HeaderText="IP">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
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
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUsuarioDetalle" runat="server" Text="Usuario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUsuarioDetalle" runat="server" Width="160px" CssClass="txtLetra"
                                                    MaxLength="50" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td style="width:50px"></td>
                                            <td>
                                                <asp:Label ID="lblFechaRegistro" runat="server" Text="Fecha:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaRegistro" runat="server" Width="160px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblResultado" runat="server" Text="Resultado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlResultado" runat="server" Width="250px" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width:50px"></td>
                                            <td>
                                                <asp:Label ID="lblOperacionTipo" runat="server" Text="Operación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOperacionTipo" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFormularioDetalle" runat="server" Text="Formulario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFormularioDetalle" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTabla" runat="server" Text="Tabla:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTabla" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHostname" runat="server" Text="HostName:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHostName" runat="server" Width="160px" CssClass="txtLetra" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width:50px"></td>
                                            <td>
                                                <asp:Label ID="lblIPDetalle" runat="server" Text="IP:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIP" runat="server" Width="160px" CssClass="datePickerCenter"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblComentario" runat="server" Text="Comentario:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtComentario" runat="server" Width="780px" Enabled="false"></asp:TextBox>
                                            </td>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCampos" runat="server" Text="Campos:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtCampos" runat="server" Width="780px" CssClass="txtLetra" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblValores" runat="server" Text="Valores:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtValores" runat="server" Width="780px" Height="100px" CssClass="txtLetra" TextMode="MultiLine"></asp:TextBox>
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
    </div>
    <script language="javascript" type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

    </script>
</asp:Content>
