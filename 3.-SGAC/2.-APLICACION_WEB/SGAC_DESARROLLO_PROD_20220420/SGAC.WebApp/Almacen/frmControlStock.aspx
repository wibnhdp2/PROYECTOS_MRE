<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmControlStock.aspx.cs" Inherits="SGAC.WebApp.Almacen.ControlStock" %>

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
        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
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
        }
                        
        .style9
        {
            width: 162px;
            text-align: left;
        }
                                
        #dtpFecIni
        {
            width: 95px;
        }
        #dtpFecFin
        {
            width: 95px;
        }
                        
        .style13
        {
            width: 134px;
        }
                        
        .style14
        {
            width: 100%;
            height: 29px;
        }
                        
    </style>
   
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%--Opciones--%>
    <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr><td><h2>
            <asp:Label ID="lblControlStock" runat="server" Text="CONTROL DE STOCK"></asp:Label>
            </h2></td></tr>
    </table>

    <%--Cuerpo--%>
    <table style="width: 90%" align="center">

        <tr>

        <td align ="left">

        <div id="tabs">
            <ul>
            <li><a href="#tab-1">Consulta</a></li>
            </ul>

            <div id="tab-1">

            <asp:UpdatePanel runat="server" ID="updConsulta" updatemode="Conditional">
            <ContentTemplate>

            <table style="width: 100%; ">

                <tr>

                    <td class="style3">

                        <table>
                                    
                            <tr>
                                <td valign="middle" style="text-align: right">
                                    <asp:Label ID="LblInsumo" runat="server" Text="Insumo :" Font-Size="10pt" 
                                        Width="80px"></asp:Label>
                                </td>
                                <td valign="middle" class="style9">
                                    <asp:DropDownList ID="cboInsumo" runat="server" Width="215px" Font-Size="10pt" 
                                        Height="22px"></asp:DropDownList>
                                </td>

                                <td valign="middle" style="text-align: right">
                                    <asp:Label ID="LblFecIni" runat="server" Text="Fec. Inicio :" Font-Size="10pt" 
                                        Width="80px"></asp:Label>
                                </td>
                                <td class="style13">
                                    <SGAC_Fecha:ctrlDate ID="dtpFecInicio" runat="server" />
                                    <%--<fecha:fecha ID="dtpFecInicio" runat="server" />--%>
                                </td>

                                <td valign="middle" style="text-align: right">
                                    <asp:Label ID="LblFecFin" runat="server" Text="Fec. Fin :" Font-Size="10pt" 
                                        Width="80px"></asp:Label>
                                </td>
                                <td>
                                     <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                    <%--<fecha:fecha ID="dtpFecFin" runat="server" />--%>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="style26" colspan="6" bgcolor="#4E102E" align="center">
                                    <asp:Label ID="lblOrigen" runat="server" CssClass="style37" Font-Bold="True" 
                                        Font-Names="Arial" Font-Size="10pt" Font-Underline="True" 
                                        ForeColor="White" Text="UBICACIÓN"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>

                                <td colspan="2">
                                     <asp:Label ID="lblOfConsularO" runat="server" Text="Of. Consular" 
                                         Font-Size="10pt"></asp:Label>
                                </td>

                                <td colspan="2">
                                    <asp:Label ID="lblTipoBovedaO" runat="server" Text="Tipo Bóveda" 
                                        Font-Size="10pt"></asp:Label>
                                </td>

                                <td colspan="2">
                                    <asp:Label ID="lblBovedaOrigen" runat="server" Text="Bóveda" Font-Size="10pt"></asp:Label>
                                </td>

                            </tr>

                            <tr>

                                <td colspan="2">
                                    <uc1:ctrlOficinaConsular ID="cboMisionConsO" runat="server" />
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

                        </table>

                        <div>
                            <br />
                        </div>

                        <table>
                            <tr>
                                <td><toolbar:toolbarcontent ID="ctrlToolBar1" runat="server"></toolbar:toolbarcontent></td>
                            </tr>
                        </table>

                        <table style="width: 100%; height: 28px; margin-top: 3px;">
                                <tr>
                                    <td align="left" class="style14">
                                        <Label:Validation ID="ctrlValidacion" runat="server" />
                                    </td>
                                </tr>
                        
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="LblSaldo" runat="server" Font-Bold="True" Font-Size="Small" 
                                        style="color: #990033" Width="554px"></asp:Label>
                                </td>
                            </tr>

                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="updGrillaConsulta" runat="server" updatemode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="grReporteGestion" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" CssClass="mGrid" Font-Size="10pt" GridLines="None" 
                                                    OnRowDataBound="grReporteGestion_RowDataBound" Width="900px">
                                                    <Columns>
                                                        <asp:BoundField DataField="OfConsular_Origen" 
                                                            HeaderStyle-CssClass="ColumnaOculta" HeaderText="OfConsular_Origen" 
                                                            ItemStyle-BackColor="#EDEDED" ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle BackColor="#EDEDED" CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TipoBoveda" HeaderStyle-CssClass="ColumnaOculta" 
                                                            HeaderText="TipoBoveda" ItemStyle-BackColor="#EDEDED" 
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle BackColor="#EDEDED" CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" 
                                                            ItemStyle-BackColor="#EDEDED" ItemStyle-Width="90" 
                                                            DataFormatString="{0:MMM-dd-yyyy}">
                                                            <ItemStyle BackColor="#EDEDED" Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TipoInsumo" HeaderText="Tipo Insumo" 
                                                            ItemStyle-BackColor="#EDEDED" ItemStyle-Width="200">
                                                            <ItemStyle BackColor="#EDEDED" Width="200px" />
                                                        </asp:BoundField>
                       <%--                                 <asp:BoundField DataField="Entrada" HeaderText="Entrada" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75"></asp:BoundField>--%>
<%--                                                        <asp:BoundField DataField="Retorno" HeaderText="Retorno" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75"></asp:BoundField>--%>
                                                        <asp:BoundField DataField="para_MotivoMovimiento" 
                                                            HeaderText="Motivo de Movimiento" 
                                                            ItemStyle-BackColor="#EDEDED" ItemStyle-Width="200">
                                                            <ItemStyle BackColor="#EDEDED" Width="200px" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="INGRESOS" HeaderStyle-Font-Size="X-Small" 
                                                            HeaderText="ENTRADAS" ItemStyle-BackColor="#EDEDED" ItemStyle-Font-Bold="true" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80">
                                                            <HeaderStyle Font-Size="X-Small" />
                                                            <ItemStyle BackColor="#EDEDED" Font-Bold="True" HorizontalAlign="Right" 
                                                                Width="80px" />
                                                        </asp:BoundField>
<%--                                                        <asp:BoundField DataField="Salida" HeaderText="Salida" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75"></asp:BoundField>--%>
<%--                                                        <asp:BoundField DataField="Devolucion" HeaderText="Devolución" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75"></asp:BoundField>--%>
                                                  <%--      <asp:BoundField DataField="Bajas" HeaderText="Bajas" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75"></asp:BoundField>--%>
                                                        <asp:BoundField DataField="SALIDAS" HeaderStyle-Font-Size="X-Small" 
                                                            HeaderText="SALIDAS" ItemStyle-BackColor="#EDEDED" ItemStyle-Font-Bold="true" 
                                                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80">
                                                            <HeaderStyle Font-Size="X-Small" />
                                                            <ItemStyle BackColor="#EDEDED" Font-Bold="True" HorizontalAlign="Right" 
                                                                Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Font-Size="X-Small" HeaderText="SALDO" 
                                                            ItemStyle-BackColor="#D6D6D6" ItemStyle-Font-Bold="true" 
                                                            ItemStyle-ForeColor="#4E102E" ItemStyle-HorizontalAlign="Right" 
                                                            ItemStyle-Width="80" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="SALDO" runat="server" Text='<%# Eval("SALDO") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="X-Small" />
                                                            <ItemStyle BackColor="#D6D6D6" Font-Bold="True" ForeColor="#4E102E" 
                                                                HorizontalAlign="Right" Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" 
                                            OnClick="ctrlPaginador_Click" />
                                    </td>
                                </tr>
                                <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="lblSaldoFinal" runat="server" Font-Bold="True" Font-Size="Small" 
                                        style="color: #990033" Width="554px"></asp:Label>
                                </td>
                            </tr>

                        </table>

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