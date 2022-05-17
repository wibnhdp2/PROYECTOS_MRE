<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmProtocolar.aspx.cs" Inherits="SGAC.WebApp.Consulta.FrmProtocolar" %>

<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate"
    TagPrefix="uc2" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="uc3" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagName="ctrlValidation"
    TagPrefix="uc4" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .asterisco
        {
            margin-left: 3px;
            color: #ad0f14;
            font-size: medium;
        }
    </style>
   
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <asp:HiddenField ID="HFGUID" runat="server" />
        <div>
            <%--Titulo--%>
            <table class="mTblTituloM2" align="center">
                <tr>
                    <td>
                        <h2>
                            <asp:Label ID="lblTituloConsultaProtocolar" runat="server" Text="Consultas - Actos Notariales"></asp:Label>
                        </h2>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <%--Cuerpo--%>
            <table style="width: 100%" align="center">
                <tr>
                    <td align="left">
                        <div id="tabs">
                            <ul>
                                <li><a href="#tab-1">
                                    <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a>
                                </li>
                            </ul>
                            <div id="tab-1">
                                <asp:UpdatePanel ID="updConsultaProtocolar" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:Label runat="server" ID="lblDatosEP" Font-Bold="true" Font-Size="Medium">Datos del Acto Notarial</asp:Label>
                                                        <span class="asterisco">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 140px">
                                                        <asp:Label ID="lblOficinaConsularConsulta" runat="server" Text="Oficina Consular :"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="100%" AutoPostBack="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEstEP" runat="server" Text="Estado del Trámite: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEstadoEP" runat="server" Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNroEP0" runat="server" Height="16px" Text="Actos Notariales: "
                                                            Width="140px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlActoNotarial" runat="server" Width="160px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlActoNotarial_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 10px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTipoActo" runat="server" Text="Tipo Acto Notarial: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTipoActo" runat="server" Width="250px" OnSelectedIndexChanged="ddlTipoActo_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNroEP" runat="server" Text="N° Escritura Pública: " Width="140px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNroEP" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 10px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 120px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Medium"> 
                                                        Fecha de la Extensión</asp:Label>
                                                    </td>
                                                    <td style="width: 10px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 120px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Fec. Extensión (Inicio): "
                                                            Width="130px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc2:ctrlDate ID="ctrFecPagoIni" runat="server" />
                                                    </td>
                                                    <td style="width: 10px">
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="Label5" runat="server" Text="Fec. Extensión (Fin): "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc2:ctrlDate ID="ctrFecPagoFin" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>                                                   
                                                </tr>
                                               
                                                <tr>
                                                <td colspan="7">
                                                    <table id="tablaDestinoAutorizacionViaje" runat="server" width="100%">
                                                     <tr>
                                                        <td colspan="7">
                                                        &nbsp;
                                                        </td>
                                                    </tr>
                                                <tr>
                                                    <td colspan="2">
                                                     <asp:Label ID="Label6" runat="server" Text="Datos del Destino "
                                                           Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td style="width:140px">
                                                    <asp:Label ID="Label7" runat="server" Text="Cont./Dept.: "
                                                            Width="130px"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width:220px">
                                                        <asp:DropDownList ID="ddl_UbigeoPaisViajeDestino" runat="server" AutoPostBack="True" Width="190px"
                                                        Style="cursor: pointer" OnSelectedIndexChanged="ddl_UbigeoPaisViajeDestino_SelectedIndexChanged"
                                                        CssClass="ParticipantesDropDownStyle" TabIndex="23">
                                                        </asp:DropDownList>
                                                        
                                                    </td>
                                                    <td style="width:90px">
                                                    <asp:Label ID="Label8" runat="server" Text="País/Prov.: "
                                                            Width="80px"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width:210px">
                                                    <asp:DropDownList ID="ddl_UbigeoRegionViajeDestino" runat="server" AutoPostBack="True" Width="190px"
                                                    Style="cursor: pointer" OnSelectedIndexChanged="ddl_UbigeoRegionViajeDestino_SelectedIndexChanged"
                                                    CssClass="ParticipantesDropDownStyle" TabIndex="24">
                                                     </asp:DropDownList>
                                               
                                                    </td>
                                                    <td style="width:90px">
                                                     <asp:Label ID="Label12" runat="server" Text="Ciu./Dist.: "
                                                            Width="80px"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width:210px">
                                                        <asp:DropDownList ID="ddl_UbigeoCiudadViajeDestino" runat="server" AutoPostBack="True"  Width="190px"
                                                        Style="cursor: pointer" CssClass="ParticipantesDropDownStyle" TabIndex="25" OnSelectedIndexChanged="ddl_UbigeoCiudadViajeDestino_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                       
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="7">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label11" runat="server" Font-Bold="true" Font-Size="Medium"> 
                                                        Fecha de Creación</asp:Label>
                                                    </td>
                                                    <td style="width: 10px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 120px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFecExtencionIni" runat="server" Text="Fec. Inicio: "
                                                            Width="130px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc2:ctrlDate ID="ctrlDateFecExtIni" runat="server" />
                                                    </td>
                                                    <td style="width: 10px">
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="lblFecExtencionFin" runat="server" Text="Fec. Fin: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc2:ctrlDate ID="ctrlDateFecExtFin" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRGE" runat="server" Text="R.G.E: "
                                                            Width="130px"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRGE" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                    </td>
                                                     <td>
                                                    </td>
                                                     <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="4">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Medium"> Datos 
                                                            de Participante</asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTipoParticipante" runat="server" Text="Tipo Participante: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlParticipanteTipo" runat="server" Width="140px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDocumentoTipo" runat="server" Text="Tipo Documento: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDocumentoTipo" runat="server" Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDocumentoNumero" runat="server" Text="Número Documento: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDocumentoNumero" runat="server" CssClass="txtLetra" MaxLength="30"
                                                            Width="140px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl1erApellido" runat="server" Text="Primer Apellido: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPrimerApellido" runat="server" CssClass="txtLetra" MaxLength="50"
                                                            Width="140px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10px">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl2doApellido" runat="server" Text="Segundo Apellido: " Width="120px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSegundoApellido" runat="server" CssClass="txtLetra" MaxLength="50"
                                                            Width="140px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="lblNombres" runat="server" Text="Nombres: "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNombres" runat="server" CssClass="txtLetra" MaxLength="50" Width="140px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="4">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 120px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFucionarioAutorizador" runat="server" Text="Funcionario Autorizador: "
                                                            Width="140px"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="ddlFuncionarioAutorizador" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 120px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    
                                                </tr>
                                                
                                            </table>
                                            <br />
                                            <div>
                                                <span style="color: #ad0f14; font-size: medium">(*)</span> <span>campos obligatorios</span>
                                                <br />
                                                <asp:Button ID="btnBuscar" runat="server" Text="     Buscar" CssClass="btnSearch"
                                                    OnClick="btnBuscar_Click" />
                                                <asp:Button ID="btnImprimir" runat="server" Text="    Imprimir" CssClass="btnPrint"
                                                    OnClick="btnImprimir_Click" />
                                                <asp:Button ID="btnLimpiar" runat="server" Text="     Limpiar" CssClass="btnLimpiar"
                                                    OnClick="btnLimpiar_Click" />
                                            </div>
                                            <br />
                                            <div>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <uc4:ctrlValidation ID="ctrlValProtocolar" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gdvProtocolar" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                                OnRowCommand="gdvProtocolar_RowCommand" DataKeyNames="sTipoActoNotarialId,iPersonaRecurrente,sSubTipoActoNotarialId,acno_sEstadoId,vEstado, sTipDocumento, vDocumentoNumero, vRutaArchivo, vNombreConsulado"
                                                                OnRowDataBound="gdvProtocolar_RowDataBound">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <SelectedRowStyle CssClass="slt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="acno_iActuacionId" HeaderText="acno_iActuacionId" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="acno_iActoNotarialId" HeaderText="acno_iActoNotarialId"
                                                                        HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vNumeroEscrituraPublica" HeaderText="N° E.P.">
                                                                        <ItemStyle HorizontalAlign="Center" Width="55px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dFecAprobacion" DataFormatString="{0:MMM-dd-yyyy HH:mm:ss}"
                                                                        HeaderText="Fec. Extensión">
                                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vPersonaRecurrente" HeaderText="Recurrente">
                                                                        <ItemStyle Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vTipoActoNotarial" HeaderText="Tipo Acto">
                                                                        <ItemStyle Width="120px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="acno_vOficinaConsularCiudad" HeaderText="Oficina Consular">
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vSubTipoActoNotarial" HeaderText="Sub Tipo Acto">
                                                                        <ItemStyle Width="120px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vNumProyecto" HeaderText="Nro. Proyecto">
                                                                        <ItemStyle Width="70px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vEstado" HeaderText="Estado">
                                                                        <HeaderStyle Width="50px" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDescDocumento" HeaderText="TipoDocumentoId">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDocumentoNumero" HeaderText="NroDocumento">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="sTipDocumento" HeaderText="sTipDocumento">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Ver Participantes" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgParticipantes" CommandName="VerParticipantes" ToolTip="Ver Participantes"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" Height="22px"
                                                                                ImageUrl="~/Images/group.png" Width="18px" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Ver Escritura Pública" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnVerFormato" CommandName="VerBorrador" ToolTip="Consultar"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_preview.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
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
                                                                    <asp:TemplateField HeaderText="Solicitar Parte/Testimonio" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ColumnaOculta"
                                                                        HeaderStyle-Font-Size="Smaller" ItemStyle-CssClass="ColumnaOculta">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnSolicitarFormato" CommandName="SolicitarFormato" ToolTip="Solicitar Parte y/o Testimonio"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_add.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta"/>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" CssClass="ColumnaOculta"/>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Rectificación" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ColumnaOculta"
                                                                        HeaderStyle-Font-Size="Smaller" ItemStyle-CssClass="ColumnaOculta">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnRectificacion" CommandName="Rectificacion" ToolTip="Rectificación Acto Notarial"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_add.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" CssClass="ColumnaOculta"/>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" CssClass="ColumnaOculta" />
                                                                    </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>                                                                            
                                                                            <asp:LinkButton ID="lnkVerProyectoEP" runat="server" CommandName="VerProyectoEP"  ToolTip="Ver Proyecto de Escritura Pública" 
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" ForeColor="Red" Font-Underline="true">Ver Proyecto de E.P.</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    </asp:TemplateField>                                                                     

                                                                    <asp:TemplateField HeaderText="Seguimiento" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnSeguimiento" CommandName="Seguimiento" ToolTip="Seguimiento Acto Notarial"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_add.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="vNombreFuncionario" HeaderText="vNombreFuncionario" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vNombreConsulado" HeaderText="vNombreConsulado" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                     <asp:BoundField DataField="pers_sGeneroId" HeaderText="Genero" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                     <asp:BoundField DataField="pago_dFechaCreacion" HeaderText="FechaPago" HeaderStyle-CssClass="ColumnaOculta"
                                                                        ItemStyle-CssClass="ColumnaOculta">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                     <asp:BoundField DataField="vRutaArchivo" HeaderText="vRutaArchivo">
                                                                        <HeaderStyle CssClass="ColumnaOculta" />
                                                                        <ItemStyle CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <uc3:ctrlPageBar ID="ctrlPaginadorProtocolar" runat="server" OnClick="ctrlPaginadorProtocolar_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label Text="LISTA DE PARTES" runat="server" ID="lblListaParte" Visible="false"
                                                                Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gdvParte" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                                Visible="false" ShowHeaderWhenEmpty="true" DataKeyNames="insu_vCodigoUnicoFabrica, tari_vTarifa"
                                                                onrowcommand="gdvParte_RowCommand" onrowdatabound="gdvParte_RowDataBound">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <SelectedRowStyle CssClass="slt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ande_sCorrelativo" HeaderText="Nro. Parte">
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ande_dFechaExtension" DataFormatString="{0:MMM-dd-yyyy hh:mm:ss}"
                                                                        HeaderText="Fec.Extensión">
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="tari_vTarifa" HeaderText="Tarifa">
                                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ande_sNumeroFoja" HeaderText="Folios">
                                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ubge_vDistrito" HeaderText="Oficina Consular">
                                                                        <ItemStyle Width="180px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ande_vPresentanteNombre" HeaderText="Solicitante">
                                                                        <ItemStyle Width="300px" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                     <asp:BoundField HeaderText="Código de Insumo" DataField="insu_vCodigoUnicoFabrica" >
                                                                        <ItemStyle HorizontalAlign="Center" Width="150px"  />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Registrar Parte" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>                                                                                                                                                        
                                                                            <asp:ImageButton ID="btnGenerarParte" CommandName="GenerarParte" ToolTip="Registrar Parte"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/clip_image027.gif" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <table id="tbSinDatos">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td width="10%">
                                                                                    <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
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
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label Text="LISTA DE TESTIMONIOS" runat="server" ID="lblListaTestimonio" Visible="false"
                                                                Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <asp:GridView ID="gdvTestimonio" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                            Visible="false" ShowHeaderWhenEmpty="true" DataKeyNames="insu_vCodigoUnicoFabrica, tari_vTarifa"
                                                            onrowcommand="gdvTestimonio_RowCommand" onrowdatabound="gdvTestimonio_RowDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <SelectedRowStyle CssClass="slt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ande_sCorrelativo" HeaderText="Nro. Testimonio">
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ande_dFechaExtension" DataFormatString="{0:MMM-dd-yyyy hh:mm:ss}"
                                                                    HeaderText="Fec.Extensión">
                                                                    <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_vTarifa" HeaderText="Tarifa">
                                                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ande_sNumeroFoja" HeaderText="Folios">
                                                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ubge_vDistrito" HeaderText="Oficina Consular">
                                                                    <ItemStyle Width="180px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ande_vPresentanteNombre" HeaderText="Solicitante">
                                                                    <ItemStyle Width="300px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Código de Insumo" DataField="insu_vCodigoUnicoFabrica" >
                                                                        <ItemStyle HorizontalAlign="Center" Width="150px"  />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Registrar Testimonio" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller">
                                                                        <ItemTemplate>                                                                                                                                                        
                                                                            <asp:ImageButton ID="btnGenerarTestimonio" CommandName="GenerarTestimonio" ToolTip="Registrar Testimonio"
                                                                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/clip_image027.gif" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Size="Smaller" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <table id="tbSinDatos">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="10%">
                                                                                <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
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
                                                        </asp:GridView>
                                                    </tr>
                                                    
                                            </table>
                                        </div>
                                        <table>
                                        <div id="ocultar"  runat="server" visible="false">
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="LISTA DE PARTICIPANTES" runat="server" ID="Label2" 
                                                                    Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <asp:GridView ID="grvParticipantes" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                                ShowHeaderWhenEmpty="true">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <SelectedRowStyle CssClass="slt" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="vTipoParticipanteId" HeaderText="Tipo Participante">
                                                                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vDescTipDoc" HeaderText="Tipo Documento">
                                                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vNroDocumento" HeaderText="Nro.Documento">
                                                                        <ItemStyle Width="70px" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vPersona" HeaderText="Participante">
                                                                        <ItemStyle Width="100px" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vNacionalidad" HeaderText="Nacionalidad">
                                                                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <table id="tbSinDatos">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td width="10%">
                                                                                    <asp:Image ID="imgWarning" runat="server" ImageUrl="../Images/img_16_warning.png" />
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
                                                            </asp:GridView>
                                                        </tr>
                                                    </div>

                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
      
    </script>
</asp:Content>
