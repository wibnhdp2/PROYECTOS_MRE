 <%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTramite.aspx.cs" maintainScrollPositionOnPostBack="true"
    Inherits="SGAC.WebApp.Registro.FrmTramite" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc4" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBarButton"
    TagName="ToolBarButtonContent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

 
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

    <style type="text/css">
        .tMsjeWarnig 
        {
            background-color: #F2F1C2;
            border-color: #f89406;
            color: #4B4F5E;
            height: 15px;
            width: 100%;
        }
        
        .modalBg
        {
            background-color: #cccccc;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
        
        .modalPanelAnular
        {
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 320px;
        }
        
        .AlineadoDerecha
        {
            text-align: right;
        }
        .style1
        {
            height: 23px;
        }
    </style>
   <script type="text/javascript" language="javascript">
       function seguridadURLPrevia() {
           if (document.referrer != "") {
               //Funciones
           }
           else {
               //mensaje de error
               location.href = '../PageError/DeniedPage.aspx';
           }
       }
       

       window.onload = seguridadURLPrevia;
        </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    
    <asp:HiddenField ID="HFGUID" runat="server" />
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloTramite" runat="server" Text="Trámites"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <table width="95%" align="center">
            <tr>
                <td runat="server" id="msjeWarningStock" visible="false" colspan="8">
                    <table id="tMsjeWarnigStock" class="tMsjeWarnig" align="center">
                        <tr>
                            <td width="40px">
                                <asp:Image ID="imgIcono" runat="server" ImageUrl="~/Images/img_16_warning.png" CssClass="imgIcono" />
                            </td>
                            <td>
                                <asp:Label ID="lblMsjeWarnigStock" runat="server" Text="Mensaje Validación" CssClass="lblEtiqueta"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4" style="font-weight: bold;">
                </td>
                <td align="right" colspan="4" style="font-weight: bold;">
                    <asp:UpdatePanel ID="updSaldoAutoadhesivo" UpdateMode="Conditional" runat="server"
                        ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:Label ID="lblSaldoEtiqueta" runat="server" Text="Saldo de Autoadhesivos :"></asp:Label>
                            <asp:Label ID="lblSaldoInsumo" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table style="width: 95%;" align="center" class="mTblSecundaria">
            <tr>
                <td style="width: 100px;">
                </td>
                <td style="vertical-align: top;" align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="ImgPersona" runat="server" Height="101px" Width="109px" ImageUrl="~/Images/People.png" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="left">
                    <table>
                        <tr>
                            <td style="width:120px">
                                <asp:Label ID="lblTipDoc" runat="server" Text="Tipo de Documento :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipDocVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNroDoc" runat="server" Text="Nro Documento :"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:Label ID="lblNroDocVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPrimerApe" runat="server" Text="Primer Apellido:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrimerApeVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSegundoApe" runat="server" Text="Segundo Apellido :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSegundoApeVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <div runat="server" id="DivApellidoCasada">
                            <tr>
                                <td>
                                    <asp:Label ID="lblApeCasada" runat="server" Text="Apellido de Casada:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblApeCasadaVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </div>
                        
                        <tr>
                            <td>
                                <asp:Label ID="lblNombresAct" runat="server" Text="Nombres :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNombresVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDireccion" runat="server" Text="Dirección :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDireccionVal" runat="server" Text="" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnActualizarDatos" runat="server" Text=" Actualizar Datos" Width="180px"
                                    CssClass="btnEdit" Visible="true" OnClick="btnActualizarDatos_Click" />
                            </td>
                        </tr>
                        <tr>
                            <asp:HiddenField ID="hid_iTipoId" runat="server" Value="0" />
                        </tr>
                        <tr>
                            <asp:HiddenField ID="hid_iPersonaId" runat="server" Value="0" />
                            <asp:HiddenField ID="hiDocumentoTipoId" runat="server" Value="0" />
                            <asp:HiddenField ID="hNroDoc" runat="server" Value="0" />
                            <asp:HiddenField ID="hid_bFallecidoFlag" runat="server" Value="0" />
                            <asp:HiddenField ID="hEdad" runat="server" Value="0" />
                        </tr>
                    </table>
                </td>
                <td style="width: 100px;">
                </td>
            </tr>
        </table>
        <%--DataField="dFechaRegistro"--%>
        <table style="width: 95%;" align="center" class="mTblSecundaria">
            <tr>
                <td>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblActuacion" runat="server" Text="Actuaciones Consulares"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblExpediente" runat="server" Text="Exhortos Consulares"></asp:Label></a></li>
                            <li><a href="#tab-3">
                                <asp:Label ID="lblProyecto" runat="server" Text="Actos Notariales"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsultaActuacion" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSeccion" runat="server" Text="Sección: " />
                                            </td>
                                            <td colspan="5">
                                                <asp:DropDownList ID="ddlSeccion" runat="server" Width="350px">
                                                    <asp:ListItem Text=" - TODAS -"></asp:ListItem>
                                                </asp:DropDownList>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td width="170px">
                                                <SGAC_Fecha:ctrlDate ID="txtFecIniAct" runat="server" />

                                            </td>
                                            <td style="width: 100px" align="right">
                                                <asp:Label ID="lblFecFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td width="170px">
                                                <SGAC_Fecha:ctrlDate ID="txtFecFinAct" runat="server"  />
                                            </td>
                                            <td style="width: 50px">
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBuscarActuacion" runat="server" Text="  Buscar" CssClass="btnSearch"
                                                    OnClick="btnBuscarActuacion_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Button ID="btnNuevaAct" runat="server" Text="   Nueva Actuación" Width="160px"
                                                    CssClass="btnNewDirFil" OnClick="btnNuevaAct_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <Label:Validation ID="ctrlValActuacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnEjecutarAnulacion" runat="server" Text="Aceptar"  style="display:none;"
                                                    onclick="btnEjecutarAnulacion_Click"  />

                                                <asp:GridView ID="gdvActuaciones" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    runat="server" AutoGenerateColumns="False" Width="100%" GridLines="None" OnRowCommand="gdvActuaciones_RowCommand"
                                                    OnRowDataBound="gdvActuaciones_RowDataBound" 
                                                    onrowcreated="gdvActuaciones_RowCreated">
                                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="iActuacionId" HeaderText="iActuacionId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iActuacionDetalleId" HeaderText="ActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sSeccionId" HeaderText="sSeccionId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sTarifarioId" HeaderText="sTarifarioId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="R.G.E." DataField="vCorrelativoActuacion" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Corr. Tarifa" DataField="vCorrelativoTarifario" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fecha" DataField="dFechaRegistro" HeaderStyle-HorizontalAlign="Center"
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tarifa" DataField="vTarifa" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Descripción" DataField="vDescripcion" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle  Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sOficinaConsularId" HeaderText="sOficinaConsularId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Oficina Consular" DataField="vOficinaCiudad" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fec. Hora Digitaliza" DataField="dFechaDigitaliza" HeaderStyle-HorizontalAlign="Center"
                                                            DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                            <ItemStyle HorizontalAlign="Center" Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Usuario Digitaliza" DataField="vUsuarioDigitaliza" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>                                                        
                                                        <asp:BoundField DataField="sUsuarioDigitaliza" HeaderText="sUsuarioDigitaliza" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Código Autoadhesivo" DataField="vCodigoInsumo" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <%--Opciones Grilla--%>
                                                        <asp:TemplateField HeaderText="Seg." ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnSeguimiento" CommandName="Seguimiento" ToolTip="Seguimiento Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_estado_tramite.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditarAct" CommandName="EditarAct" ToolTip="Editar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultarAct" CommandName="ConsultarAct" ToolTip="Consultar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reasignar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnReasignar" CommandName="Reasignar" ToolTip="Reasignar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_reasignar.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <a>
                                                                    <asp:ImageButton ID="btnAnularAct" CommandName="Anular" ToolTip="Anular Actuación"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <div>
                                                    <PageBarContent:PageBar ID="ctrlPaginadorActuacion" runat="server" OnClick="ctrlPaginadorActuacion_Click" />
                                                </div>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaSeguimiento" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td class="style1">
                                                                    <asp:Label ID="lblTtlSeguimiento" runat="server" Text="SEGUIMIENTO" Style="font-weight: 700;
                                                                        color: #800000;" Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gdvSeguimiento" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                        SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                                        ShowHeaderWhenEmpty="True" >
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Fecha Registro" DataField="dFechaRegistro" 
                                                                                DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}">
                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField HeaderText="Estado" DataField="vEstado">
                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField HeaderText="Funcionario" DataField="vFuncionario">
                                                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField HeaderText="Observación" DataField="vObservacion">
                                                                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField HeaderText="Usuario" DataField="usua_vAlias">
                                                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="slt" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updConsultaExpediente" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFecIniExp" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecIniExp"  runat="server" />
                                            </td>
                                            <td style="width: 50px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFecFinExp" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecFinExp"  runat="server" />
                                            </td>
                                            <td style="width: 50px">
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBuscarExpediente" runat="server" Text="  Buscar" CssClass="btnSearch"
                                                    UseSubmitBehavior="False" OnClick="btnBuscarExpediente_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <asp:Button ID="btnNuevoExpediente" runat="server" Text="Nuevo Expediente" Width="180px"
                                                    CssClass="btnNewActuacion" OnClick="btnNuevoExpediente_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <Label:Validation ID="ctrlValExpediente" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvExpediente" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    Width="100%" DataKeyNames="actu_iPersonaRecurrenteId,acju_iActoJudicialId,acju_sEstadoId" 
                                                    onrowcommand="gdvExpediente_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Fecha" DataFormatString="{0:MMM-dd-yyyy HH:mm}" 
                                                            DataField="acju_dFechaRecepcion">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Nro. Expediente" DataField="acju_vNumeroExpediente">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Estado Expediente" DataField="acju_vEstado">
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Recurrente" DataField="para_Recurrente">
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tarifa" DataField="tari_vDescripcionCorta">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Width="160px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tipo Pago" DataField="para_TipoPagoDescripcion">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Actuación" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnFind" CommandName="Consultar" ToolTip="Consultar Actuación"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="slt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <PageBarContent:PageBar ID="ctrlPaginadorExpediente" runat="server" OnClick="ctrlPaginadorExpediente_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-3">
                            <asp:UpdatePanel ID="updConsultaProyecto" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecIniProy" runat="server" />
                                            </td>
                                            <td style="width: 50px">
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="txtFecFinProy" runat="server" />
                                            </td>
                                            <td style="width: 50px">
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBuscarProyecto" runat="server" Text="  Buscar" CssClass="btnSearch"
                                                    UseSubmitBehavior="False" OnClick="btnBuscarProyecto_Click" />
                                            </td>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                <asp:Button ID="btnConsultarProyecto" runat="server" Text="  Consular" 
                                                    CssClass="btnEdit" onclick="btnConsultarProyecto_Click" Visible="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Button ID="btnNuevoExtraPro" runat="server" Text="  Nuevo Extraprotocolar" Width="200px"
                                                    CssClass="btnNewActuacion" OnClick="btnNuevoExtraPro_Click" />
                                                <asp:Button ID="btnNuevoProtocolar" runat="server" Text="  Nuevo Protocolar" Width="200px"
                                                    CssClass="btnNewActuacion" OnClick="btnNuevoProtocolar_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <Label:Validation ID="ctrlValProyecto" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvProyecto" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" 
                                                    GridLines="None" onrowcommand="gdvProyecto_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <SelectedRowStyle CssClass="slt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="acno_iActoNotarialId" HeaderText="acno_iActoNotarialId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="actu_iActuacionId" HeaderText="actu_iActuacionId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="acno_sTipoActoNotarialId" HeaderText="acno_sTipoActoNotarialId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="acde_iActuacionDetalleId" HeaderText="acde_iActuacionDetalleId" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta">
                                                            <HeaderStyle CssClass="ColumnaOculta" />
                                                            <ItemStyle CssClass="ColumnaOculta" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tipo Acto" DataField="acno_vTipoActoNotarial" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Denominación" DataField="acno_vSubTipoActoNotarial" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Oficina Consular" DataField="acno_vOficinaConsularCiudad" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Fecha" DataFormatString="{0:MMM-dd-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Center"
                                                            DataField="acno_dFechaCreacion">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Nro. Proyecto" DataField="vNumProyecto" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Recurrente" DataField="actu_vPersonaRecurrente" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Tipo Pago" DataField="vTipoPago" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Total" DataField="FTotal" HeaderStyle-HorizontalAlign="Center"
                                                            DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:BoundField>                                                        
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditarProy" CommandName="EditarProy" ToolTip="Editar Proyecto"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultarProy" CommandName="ConsultarAct" ToolTip="Consultar Proyecto"
                                                                    CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <a>
                                                                    <asp:ImageButton ID="btnAnularActNotarial" CommandName="Anular" ToolTip="Anular Acto Notarial"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Size="Smaller" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <PageBarContent:PageBar ID="ctrlPaginadorProyecto" runat="server" OnClick="ctrlPaginadorProyecto_Click" />
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
    <br />
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function ddlcontrolError(ctrl) {
            if (ctrl != null) {
                var x = ctrl.selectedIndex;
                if (x < 1) {
                    ctrl.style.border = "1px solid Red";
                    bolValida = false;
                }
                else {
                    ctrl.style.border = "1px solid #888888";
                }
            }
            return bolValida;
        }

        function txtcontrolError(ctrl) {
            var x = ctrl.value.trim();
            var bolValida = true;

            if (x.length == 0) {
                ctrl.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ctrl.style.border = "1px solid #888888";
            }
            return bolValida;
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
        function noPaste() {
            return false;
        }
        $(document).ready(function () {
            $("#MainContent_txtFecIniExp_TxtFecha").attr('onpaste', "return noPaste()");
            $("#MainContent_txtFecFinExp_TxtFecha").attr('onpaste', "return noPaste()");
            $("#MainContent_txtFecIniAct_TxtFecha").attr('onpaste', "return noPaste()");
            $("#MainContent_txtFecFinAct_TxtFecha").attr('onpaste', "return noPaste()");
            $("#MainContent_txtFecIniProy_TxtFecha").attr('onpaste', "return noPaste()");
            $("#MainContent_txtFecFinProy_TxtFecha").attr('onpaste', "return noPaste()");
        });
         
    </script>

    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>

</asp:Content>
