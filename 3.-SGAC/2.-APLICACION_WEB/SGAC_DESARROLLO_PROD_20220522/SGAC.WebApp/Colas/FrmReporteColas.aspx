<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmReporteColas.aspx.cs" Inherits="SGAC.WebApp.Colas.FrmReporteColas" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });
    </script>


</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="REPORTE DE COLAS"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table style="width: 90%" align="center">
    <tr>
    <td>
    <div id="tabs">
        <asp:UpdatePanel ID="UpdPrincipal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>                            
                            <div align="left">
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                        </td>
                                        <td colspan="3" align="left">
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="450px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblTipoReporte" runat="server" Text="Tipo Reporte:"></asp:Label>
                                        </td>
                                        <td colspan="3" align="left">
                                            <asp:DropDownList ID="ddlTipoReporte" runat="server" Width="450px" OnSelectedIndexChanged="ddlTipoReporte_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />                                            
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:" CssClass="lblPadding"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="4">
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                <br />
                                <br />
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </td>
    </tr>   
    </table> 
</asp:Content>
