<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmGuiaDespacho.aspx.cs" maintainScrollPositionOnPostBack="true"
Inherits="SGAC.WebApp.Registro.frmGuiaDespacho" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation"
    TagPrefix="Label" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

 <script type="text/javascript">

     $(function () {
         $('#tabs').tabs();
        
     });

    </script>
 <script type="text/javascript">
     function estadoBotonGrabar() {
         var x = document.getElementById('<%=gdvFichas.ClientID%>').querySelectorAll("input");
         var i;
         var cnt = 0;
         for (i = 0; i < x.length; i++) {
             if (x[i].type == "checkbox" && x[i].checked) {
                 cnt++;
             }
         }
         if (cnt > 0) {
             document.getElementById('<%=btnAdicionarFichas.ClientID%>').disabled = false;
             if (x.length == cnt) {
                 document.getElementById('chkMarcarTodos').checked = 1;
             }
             else {
                 document.getElementById('chkMarcarTodos').checked = 0;
             }
             document.getElementById('<%=lblCantidad.ClientID%>').innerHTML = cnt;
         }
         else {
             document.getElementById('<%=btnAdicionarFichas.ClientID%>').disabled = true;
             document.getElementById('chkMarcarTodos').checked = 0;
             document.getElementById('<%=lblCantidad.ClientID%>').innerHTML = "0";
         }
     }
     function showpopupother(type_msg, title, msg, resize, height, width) {
         showdialog(type_msg, title, msg, resize, height, width);
     }

     function seleccionar_todo() {
         if (document.getElementById('<%=gdvFichas.ClientID%>') != null) {

             var x = document.getElementById('<%=gdvFichas.ClientID%>').querySelectorAll("input");
             var i;
             var cnt = 0;
             for (i = 0; i < x.length; i++) {
                 if (x[i].type == "checkbox") {
                     x[i].checked = document.getElementById('chkMarcarTodos').checked;
                     if (x[i].checked) {
                         cnt++;
                     }
                 }
             }
             if (cnt > 0) {
                 document.getElementById('<%=btnAdicionarFichas.ClientID%>').disabled = false;
                 document.getElementById('<%=lblCantidad.ClientID%>').innerHTML = cnt;
             }
             else {
                 document.getElementById('<%=btnAdicionarFichas.ClientID%>').disabled = true;
                 document.getElementById('<%=lblCantidad.ClientID%>').innerHTML = "0";
             }
         }
     } 
 </script>

 

    <style type="text/css">
        .style4
        {
            width: 158px;
        }
        .style5
        {
            width: 196px;
        }
        .style6
        {
            width: 186px;
        }
        .style8
        {
            width: 261px;
        }
        .style9
        {
            width: 197px;
        }
        .style10
        {
            width: 287px;
        }
        .style11
        {
            width: 195px;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    

 <table style="width: 90%; border-spacing: 0px;" align="center">
        <tr>
            <td>
                <h2>
                    GUÍA DE DESPACHO - RENIEC</h2>
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
                                <table width="100%" style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray">
                                    <tr>
                                        <td align="right" class="style9" >
                                            <asp:Label ID="lblGuiaDespachoSelGuia" runat="server" Text="Guía Despacho:"></asp:Label>
                                        </td>
                                        <td>
                                                <asp:TextBox ID="txtGuiaDespachoSelGuia" runat="server" onkeypress="return validarTelefonos(event)"
                                                    Width="150px" MaxLength="10" />
                                        </td>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td>                                                                                                
                                            <asp:HiddenField ID="HFGuiaDespachoId" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="style9" >                                        
                                            <asp:Label ID="lblFechaInicioSelGuia" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaInicioSelGuia" runat="server" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblFechaFinSelGuia" runat="server" Text="Fecha Final:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaFinSelGuia" runat="server" />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td align="right" class="style9">
                                            <asp:Label ID="lblTipoEnvioSelGuia" runat="server" Text="Tipo de Envio:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="RBListTipoEnvioSelGuia" runat="server" 
                                                style="cursor:pointer;">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblNombreEmpresaSelGuia" runat="server" 
                                                Text="Nombre de la Empresa de Envio:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombreEmpresaSelGuia" runat="server" CssClass="txtLetra" 
                                                TabIndex="4" Width="200px" MaxLength="100" />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td align="right" class="style9">
                                            <asp:Label ID="lblNroHojaSelGuia" runat="server" 
                                                Text="Nro.Hoja de Remisión u Oficio:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNrotHojaSelGuia" runat="server" TabIndex="5" onkeypress="return validarTelefonos(event)"
                                                Width="150px" MaxLength="10" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblGuiaAereaSelGuia" runat="server" Text="Guía Aérea:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuiaAereaSelGuia" runat="server" TabIndex="6" Width="150px" onkeypress="return validarTelefonos(event)"
                                                MaxLength="10" />
                                        </td>
                                    </tr>
                                     <tr>
                                         <td align="right" class="style9">
                                             <asp:Label ID="lblEstadoGuiaSel" runat="server" Text="Estado de la Guía:"></asp:Label>
                                         </td>
                                         <td>
                                             <asp:DropDownList ID="ddl_EstadoGuiaSel" runat="server" Height="21px" style="cursor:pointer;"
                                                 TabIndex="100" Width="250px" />
                                         </td>
                                         <td align="right">
                                             &nbsp;</td>
                                         <td>
                                             &nbsp;</td>
                                    </tr>
                                     <tr>
                                        <td colspan="4">
                                            <ToolBar:ToolBarContent ID="ctrlToolBarGuiaDespacho" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <Label:Validation ID="ctrlValidacionAtencion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div align="left">
                            <asp:Panel ID="pnlGuiaDespacho" runat="server"> 
                            
                                <h5>
                                    <asp:Label ID="lblListaGuiaDespacho" runat="server" Text="Lista Guías de Despacho"> </asp:Label> 
                                </h5>
                              <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdGuiaDespacho" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False" GridLines="None" Width="870px" 
                                                        OnRowCommand="grdGuiaDespacho_RowCommand" PageSize="5" 
                                                onrowdatabound="grdGuiaDespacho_RowDataBound">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                           <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGuiaDespachoId" runat="server" Text='<%# Bind("GUDE_IGUIADESPACHOID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNumeroGuiaDespacho" runat="server" Text='<%# Bind("VNUMEROGUIADESPACHO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEstado" runat="server" Text='<%# Bind("ESTADOGUIA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Nro." DataField="INUMERO" HeaderStyle-Width="80">
                                                                <HeaderStyle Width="80px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Fecha de Envio" DataField="DFECHAENVIO" DataFormatString="{0:dd-MM-yyyy}">
                                                                <HeaderStyle Width="110px" />
                                                                <ItemStyle HorizontalAlign="Center" Width="130px"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Oficina Consular" DataField="VSIGLAS" HeaderStyle-Width="80">
                                                                <HeaderStyle Width="80px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>

                                                            <asp:BoundField HeaderText="Tipo de Envio" DataField="VTIPOENVIO">
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Nombre de la Empresa" DataField="VNOMBREEMPRESAENVIO">
                                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="N° Guía Aérea" DataField="VGUIAAEREA">
                                                                <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="N° Hoja" DataField="VNUMEROHOJA">
                                                                <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="N° Guía Despacho" DataField="VNUMEROGUIADESPACHO">
                                                                <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Estado Guía" DataField="ESTADOGUIA">
                                                                <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                            </asp:BoundField>
                                                            
                                                            <asp:TemplateField HeaderText="Selec. Fichas" 
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" CommandName="Seleccion" ToolTip="Consultar" runat="server" ImageUrl="../Images/img_16_success.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEditarGuiaDespacho" runat="server" CommandName="Editar" ImageUrl="../Images/img_16_edit.png" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Anular">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnAnularGuiaDespacho" runat="server" CommandName="Anular" 
                                                                            ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');"/>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                                                    </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                 <div>
                                       <PageBarContent:PageBar ID="ctrlPaginadorGuia" runat="server" Visible="False" OnClick="ctrlPageBarGuiaDespacho_Click"/>
                                 </div>
                                 
                                 <table width="100%">
                                    <tr>
                                        <td align="left" class="style8">
                                             <h5>
                                                <asp:Label ID="lblEtiquetaGuiaDespacho" runat="server" Text="Guía de Despacho N°" ></asp:Label>
                                                    <asp:Label ID="lblNroGuiaDespacho" runat="server" Text="Label"></asp:Label>
                                            </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSeleccionarFichas" runat="server" Text="       Seleccionar Fichas"  
                                                    CssClass="btnNew" CausesValidation="false" 
                                                    TabIndex="12" Width="180px" 
                                                    onclick="btnSeleccionarFichas_Click"  />

                                        </td>
                                    </tr>
                                 </table>
                                 
                                                    
                            </asp:Panel>

                          <asp:Panel ID="pnlFichasEnviadas" runat="server"> 
                          
                                  <h5>
                                        <asp:Label ID="lblDetalle" runat="server" Text="Lista Fichas Registrales"> </asp:Label> 
                                  </h5>
                                    <asp:GridView ID="gdvFichasEnviadas" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                            AutoGenerateColumns="False" GridLines="None" 
                                            onrowcommand="gdvFichasEnviadas_RowCommand" PageSize="5" 
                                      onrowdatabound="gdvFichasEnviadas_RowDataBound">
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            <Columns>            
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFichaEnviadaId" runat="server" Text='<%# Bind("FIEN_IFICHAENVIADAID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                          
                                                <asp:BoundField HeaderText="N° Ficha Registral" DataField="VNUMEROFICHA">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Fecha" DataField="DFECHA" DataFormatString="{0:dd-MM-yyyy}">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="R.G.E." DataField="ICORRELATIVOACTUACION">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Titular" DataField="VNOMBRES_APELLIDOS">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tarifa" DataField="VNRO_TARIFA_DESC">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Estado" DataField="ESTA_VDESCRIPCIONCORTA">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Anular">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnAnular" runat="server" CommandName="Anular" 
                                                            ImageUrl="../Images/img_grid_delete.png" OnClientClick="return confirm('Desea Eliminar el registro');"/>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                               </asp:TemplateField>

                                            </Columns>
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>

                                 <div>
                                       <PageBarContent:PageBar ID="ctrlPaginadorListaFichasEnviadas" runat="server" Visible="False" OnClick="ctrlPaginadorListaFichasEnviadas_Click"/>
                                 </div>
                        </asp:Panel>                                                       
                            </div>



<cc1:modalpopupextender ID="mdlpopupFichas" 
                     BackgroundCssClass="modalbackground" runat="server" TargetControlID="btnShow"     
                     PopupControlID="pnl" >
  </cc1:modalpopupextender>         
   <asp:Panel ID="pnl" runat="server"  CssClass="modalpopup" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Auto">             
   <div style="overflow: scroll; max-height: 500px;height:300px;" >
            <div class='headermsg'><span style="margin-left:5px;font-family:Arial; font-size:14px; font-weight:bold">Consulta de Fichas Registrales</span></div>
            <asp:Button ID="btnShow" runat="server" Text="." CssClass="show" />    
            <asp:UpdatePanel ID="updFicha" runat="server">
                <ContentTemplate>
                
                 
                    <table width="97%" style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray" align="center">
                       
                        <tr>
                            <td align="right" class="style4">
                                <asp:Label ID="lblTipoTramiteSelFicha" runat="server" Text="Tipo de Trámite:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_TipoTramiteSelFicha" runat="server" Height="21px" style="cursor:pointer;"
                                    TabIndex="100" Width="250px" />
                            </td>
                            <td class="style5">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="style4">
                                <asp:Label ID="lblFichaRegistralSelFicha" runat="server" Text="N° Ficha Registral:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNroFichaRegistralSelFicha" runat="server"   onkeypress="return validarTelefonos(event)"                                  
                                    Width="150px" MaxLength="10" TabIndex="101" />
                            </td>
                            <td align="right" class="style5">
                                <asp:Label ID="lblEstadoFichaRegistralSelFicha" runat="server" 
                                    Text="Estado de la Ficha Registral:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEstadoFichaSelFicha" runat="server" TabIndex="102" style="cursor:pointer;"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style4">
                                <asp:Label ID="lblFechaInicioSelFicha" runat="server" Text="Fecha Inicio:"></asp:Label>
                            </td>
                            <td>
                                <SGAC_Fecha:ctrlDate ID="txtFechaInicioSelFicha" runat="server" />
                            </td>
                            <td align="right" class="style5">
                                <asp:Label ID="lblFechaFinSelFicha" runat="server" Text="Fecha Final:"></asp:Label>
                            </td>
                            <td>
                                <SGAC_Fecha:ctrlDate ID="txtFechaFinSelFicha" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                    <ToolBar:ToolBarContent ID="ctrlToolBarConsultarFichas" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table width="97%" align="center">
                        <tr>
                                   
                        <td>

                        <asp:Button ID="btnAdicionarFichas" runat="server" Text="       Adicionar Fichas"  
                            CssClass="btnEnviar" CausesValidation="false" 
                            TabIndex="103" Width="160px" onclick="btnAdicionarFichas_Click"  />

                        </td>
                        <tr>
                        <td>
                        
                        </td>
                        <tr>
                            <td colspan="2">
                                    <Label:Validation ID="ctrlValidacionFichas" runat="server" />       
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input id="chkMarcarTodos" type="checkbox"  style="cursor: pointer" onclick="seleccionar_todo()"/>
                                 Marcar todos
                                </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h5>
                                    <asp:Label ID="lblListaFichasPorEnviar" runat="server" 
                                        Text="Lista Fichas por Enviar"> </asp:Label>
                                    <asp:Label ID="lblText" runat="server" Text=" - Seleccionados: "></asp:Label>
                                    <asp:Label ID="lblCantidad" runat="server" Text="0"></asp:Label>
                                </h5>
                                <div style="width:100%; height:300; overflow: auto;">
                                    <asp:GridView ID="gdvFichas" runat="server" AutoGenerateColumns="False" 
                                        CssClass="mGrid" GridLines="None" PageSize="9000" 
                                        SelectedRowStyle-CssClass="slt">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelItem" runat="server" onclick="estadoBotonGrabar()" 
                                                        style="cursor: pointer" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VNUMEROFICHA" HeaderText="N° Ficha Registral">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DFECHA" DataFormatString="{0:dd-MM-yyyy}" 
                                                HeaderText="Fecha">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ICORRELATIVOACTUACION" HeaderText="R.G.E.">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VNOMBRES_APELLIDOS" HeaderText="Titular">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VNRO_TARIFA_DESC" HeaderText="Tarifa">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTA_VDESCRIPCIONCORTA" HeaderText="Estado">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="slt" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        </table> 
                   

                </ContentTemplate>
            </asp:UpdatePanel>
           </div>                
 </asp:Panel> 


                        </ContentTemplate>
                    </asp:UpdatePanel>
                 </div>

                 <div id="tab-2">
                    <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" style="border-bottom: 1px solid gray; border-top: 1px solid gray; border-left: 1px solid gray; border-right: 1px solid gray"> 
                                <tr>
                                    <td colspan="4">
                                         <ToolBar:ToolBarContent ID="ctrlToolBarMantenimientoGuiaDespacho" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                         <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                CssClass="hideControl" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style11">
                                        <asp:Label ID="lblFechaEnvio" runat="server" Text="Fecha de Envio:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <SGAC_Fecha:ctrlDate ID="txtFechaEnvioGuia" runat="server" />
                                    </td>
                                    <td align="right" class="style6">
                                        <asp:Label ID="lblGuiaDespacho" runat="server" Text="Guía Despacho:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGuiaDespacho" runat="server" TabIndex="200" Width="150px" 
                                            onkeypress="return validarTelefonos(event)" MaxLength="10"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style11">
                                        <asp:Label ID="lblTipoEnvio" runat="server" Text="Tipo de Envio:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <asp:RadioButtonList ID="RBListTipoEnvio" runat="server" 
                                            style="cursor:pointer;" AutoPostBack="True" 
                                            onselectedindexchanged="RBListTipoEnvio_SelectedIndexChanged">
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="right" class="style6">
                                        <asp:Label ID="lblNombreEmpresa" runat="server" 
                                            Text="Nombre de la Empresa de Envio:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombreEmpresa" runat="server" CssClass="txtLetra" 
                                            TabIndex="201" Width="200px" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style11">
                                        <asp:Label ID="lblNroHoja" runat="server" Text="Nro.Hoja de Remisión u Oficio:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <asp:TextBox ID="txtNroHoja" runat="server" Width="150px" TabIndex="202" 
                                            onkeypress="return validarTelefonos(event)" MaxLength="10"/>
                                    </td>
                                    <td align="right" class="style6">
                                        <asp:Label ID="lblGuiaAerea" runat="server" Text="Guía Aérea:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGuiaAerea" runat="server" TabIndex="203" Width="150px" 
                                            onkeypress="return validarTelefonos(event)" MaxLength="10"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style11">
                                        <asp:Label ID="lblEstadoGuia" runat="server" Text="Estado de la Guía:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <asp:DropDownList ID="ddl_EstadoGuia" runat="server" Height="21px" style="cursor:pointer;"
                                            TabIndex="100" Width="250px" />
                                    </td>
                                    <td align="right" class="style6">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    <Label:Validation ID="ctrlValidacionEdicionGuia" runat="server" />   
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                 </div>
            </div>
        </td>
    </tr>
    <tr>
        <td align="left">
            &nbsp;</td>
    </tr>
 </table>
</asp:Content>
