<%@ Page Title="SGAC" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FrmRegistroAsistenciaImpresion.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRegistroAsistenciaImpresion" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagName="ToolBarContent"
    TagPrefix="ToolBar" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrloficinaconsular"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlFecha.ascx" TagName="fecha" TagPrefix="fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="pagebar"
    TagPrefix="pagebarcontent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="validation"
    TagPrefix="label" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function ValidarDatos() {
            var bolValida = true;

            var strOficinaConsular = $('#<%=ctrlOficinaConsular.FindControl("ddlOficinaConsular").ClientID %>').val();
            var ddlOficinaConsular = document.getElementById('<%= ctrlOficinaConsular.FindControl("ddlOficinaConsular").ClientID %>');

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
   
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes: Registro de Asistencia"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>
        <br />
        <%--Opciones--%>
        <table style="width: 90%" align="center" class="mTblPrincipal">
            <tr>
                <td>
                    <div>
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td width="120px">
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButton ID="OptOpcion1" runat="server" OnCheckedChanged="OptOpcion1_CheckedChanged"
                                                Text="Programa de Asistencia Legal Humanitaria - Consulado (PALH)" AutoPostBack="True"
                                                GroupName="pollongo" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButton ID="OptOpcion2" runat="server" OnCheckedChanged="OptOpcion2_CheckedChanged"
                                                Text="Programa de Asistencia Humanitaria - Cancilleria (PAH)" AutoPostBack="True"
                                                GroupName="pollongo" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Fecha Inicio
                                        </td>
                                        <td width="150px">
                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                        </td>
                                        <td width="100px">
                                            Fecha Fin
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            Continente
                                        </td>
                                        <td colspan="3" class="style1">
                                            <asp:DropDownList ID="ddl_Continente" runat="server" Width="400px" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="ddl_Continente_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Oficina Consular&nbsp;
                                        </td>
                                        <td colspan="3">
                                            <uc1:ctrloficinaconsular ID="ctrlOficinaConsular" runat="server" Width="400px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Modalidad
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddl_Modalidad" runat="server" Width="400px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Usuario"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddl_Usuario" runat="server" Width="400px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Titular"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtTitular" runat="server" Width="396px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblValidacion" runat="server" CssClass="hideControl" ForeColor="Red"
                                                Text="Falta ingresar Usuario." />
                                        </td>
                                    </tr>
                                    <%--Opciones--%>
                                    <tr>
                                        <td>
                                            <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 870px;">
                                            <label:validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    </div>
</asp:Content>
