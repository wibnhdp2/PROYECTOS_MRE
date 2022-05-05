<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAlmacenReporte.aspx.cs" 
Inherits="SGAC.WebApp.Almacen.frmAlmacenReporte" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <script type="text/javascript">

        $(function () {
            $('#tabs').tabs();
        });        

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
    </script>

   
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <%--Cuerpo--%>
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloLibroContableRpt" runat="server" Text="Reportes: Almacén"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <br />
    <%--Opciones--%>
    <table style="width: 90%" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="2">
                                         <asp:Label runat="server" Text="Tipo Reporte" 
                                             Font-Size="11pt"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Tipo Insumo" 
                                            Font-Size="11pt"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label  runat="server" Text="Motivo" Font-Size="11pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                     <td colspan="2">
                                        <asp:DropDownList ID="ddlTipoReporte" runat="server" Width="350px" Height="22px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlLibroContableTipo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>

                                    <td colspan="2">
                                        <asp:DropDownList ID="cboInsumo" runat="server" Height="22px" 
                                            Width="220px" 
                                            onselectedindexchanged="cboTipoBovedaO_SelectedIndexChanged" 
                                            AutoPostBack="True" Font-Size="10pt" >
                                        </asp:DropDownList>
                                    </td>

                                    <td colspan="2">
                                        <asp:DropDownList ID="cboMotivo" runat="server" Height="22px" Width="320px" 
                                            Font-Size="10pt" >
                                        </asp:DropDownList>
                                    </td>

                                </tr>

                                 <tr>

                                    <td colspan="2">
                                         <asp:Label ID="Label1" runat="server" Text="Of. Consular" 
                                             Font-Size="11pt"></asp:Label>
                                    </td>

                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" Text="Tipo Bóveda" 
                                            Font-Size="11pt"></asp:Label>
                                    </td>

                                    <td colspan="2">
                                        <asp:Label ID="Label3" runat="server" Text="Bóveda" Font-Size="11pt"></asp:Label>
                                    </td>

                                </tr>
                                <tr>

                                     <td colspan="2">
                                        <uc1:ctrlOficinaConsular ID="cboMisionConsular" runat="server" />
                                    </td>

                                    <td colspan="2">
                                        <asp:DropDownList ID="cboTipoBovedaO" runat="server" Height="22px" 
                                            Width="220px" 
                                            onselectedindexchanged="cboTipoBovedaO_SelectedIndexChanged" 
                                            AutoPostBack="True" Font-Size="10pt" >
                                        </asp:DropDownList>
                                    </td>

                                    <td colspan="2">
                                        <asp:DropDownList ID="cboBovedaO" runat="server" Height="22px" Width="320px" 
                                            Font-Size="10pt" >
                                        </asp:DropDownList>
                                    </td>

                                </tr>

                              
                             
                                
                            <tr>
                                <td>
                                    <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:" 
                                        Font-Size="11pt" Width="100px"></asp:Label>

                                        <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                </td>

                                <td>
                                </td>
                              
                                <td>
                                    <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:" 
                                        CssClass="lblPadding" Font-Size="11pt" Width="100px"></asp:Label>

                                    <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                </td>
                           
                            </tr>
                            </table>
                            <table width="100%">
                                <%--Opciones--%>
                                <tr>
                                    <td>
                                        <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <Label:Validation ID="ctrlValidacion" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gdvReporte" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None">
                                                    <Columns>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
