<%@ Page Language="C#"  uiCulture="es-PE" culture="es-PE"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTarifario.aspx.cs"
    Inherits="SGAC.WebApp.Configuracion.Tarifario" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />   
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>   
    <script src="../Scripts/site.js" type="text/javascript"></script>
    

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <br />

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

         function validarSoloLetras(txt) {

             var charpos = txt.value.search("[^A-Za-z]");

             if (txt.value.length > 0 && charpos >= 0) {
                 txt.value = '';
                 alert('Error - No se pueden ingresar caracteres extraños.');
             }
         }


    </script>
   
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTarifario" runat="server" Text="Tarifario"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%--Cuerpo--%>
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%-- Consulta --%>
                                    <table> 
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSeccionConsulta" runat="server" Text="Sección:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlSeccionConsulta" runat="server" Width="500px" 
                                                    onselectedindexchanged="ddlSeccionConsulta_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDescripcionConsulta" runat="server" Text="Descripción:"></asp:Label>
                                            </td>
                                            <td colspan="2" style="margin-left: 40px">
                                                <asp:TextBox ID="txtDescripcionConsulta" runat="server" Width="350px" MaxLength="100" CssClass="txtLetra"></asp:TextBox>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEstadoConsulta" runat="server" Width="200px">
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
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gdvTarifario" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                            OnRowCommand="gdvTarifario_RowCommand">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="tari_sTarifarioId" HeaderText="tari_sTarifarioId" 
                                                                    HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">
                                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_sSeccionId" HeaderText="N° Sección: " HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>                                                                
                                                                <asp:BoundField DataField="tari_sNumero" HeaderText="N°">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_vLetra" HeaderText="Letra">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_vDescripcionCorta" HeaderText="Naturaleza del Acto">
                                                                    <HeaderStyle Width="400px" />
                                                                    <ItemStyle Width="400px" /> 
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vCalculoTipoId" HeaderText="Tipo Cálculo">
                                                                    <HeaderStyle Width="200px" />
                                                                    <ItemStyle Width="200px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_FCosto" HeaderText="Costo">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="vTopeUnidadId" HeaderText="Tope Tipo">
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="tari_ITopeCantidad" HeaderText="Tope Cantidad">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                            runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="slt" />
                                                        </asp:GridView>
                                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>                                                
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                    <table>
                                         <tr>
                                            <td id="tdMsje" colspan="4">
                                                <asp:Label ID="lblValidacion"
                                                           runat="server"
                                                           Text="Debe ingresar los campos requeridos (*)"
                                                           CssClass="hideControl"
                                                           ForeColor="Red"
                                                           Font-Size="14px">
                                                </asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSeccionMant" runat="server" Text="Sección:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlSeccionMant" runat="server" Width="500px" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTarifaNumeroMant" runat="server" Text="Número:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTarifaNumeroMant" runat="server" Width="100px" onKeyPress="return isNumberKey(event)"
                                                    MaxLength="3" CssClass="campoNumero" TabIndex="2" onpaste='return false' ></asp:TextBox>
                                                <asp:Label ID="lblVal_ddlGrupoMant0" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTarifaLetraMant" runat="server" Text="Letra:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTarifaLetraMant" runat="server" Width="100px" MaxLength="4" 
                                                              CssClass="txtLetra" onKeyPress="return isLetra(event)" onblur="return validarSoloLetras(this)"
                                                    TabIndex="3" />
                                            </td>
                                        </tr>
                                        <caption>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNombreCortoMant" runat="server" Text="Nombre Corto:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtNombreCortoMant" runat="server" Width="575px" 
                                                        CssClass="txtLetra" MaxLength="100" TabIndex="4"
                                                        onKeyPress="return isDescripcion(event)" />
                                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDescripcionMant" runat="server" Text="Descripción:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtDescripcionMant" runat="server" Width="580px" 
                                                        CssClass="txtLetra" TabIndex="5" TextMode="MultiLine" 
                                                        onKeyPress="return isDescripcion(event)" />
                                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBasePercepcionMant" runat="server" Text="Base Percepción:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBasePercepcionMant" runat="server" Width="200px" 
                                                        TabIndex="6">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblVal_ddlGrupoMant1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTipoCalculoMant" runat="server" Text="Tipo Cálculo:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTipoCalculoMant" runat="server" Width="200px" 
                                                        TabIndex="7">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblVal_ddlGrupoMant2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>                                           
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTarifaCostoMant" runat="server" Text="Costo en S/C:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTarifaCostoMant" runat="server" Width="200px" 
                                                        CssClass="campoNumero" TabIndex="8" onKeyPress="return isNumeroCosto(event)" />
                                                    <asp:Label ID="lblVal_ddlGrupoMant3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td>
                                                
                                                    <asp:Label ID="lblTopeTipoMant" runat="server" Text="Tope Tipo:"></asp:Label>
                                                
                                                </td>
                                                <td>
                                                
                                                    <asp:DropDownList ID="ddlTopeTipoMant" runat="server" Width="200px" 
                                                        TabIndex="9">
                                                    </asp:DropDownList>
                                                
                                                    <asp:Label ID="lblVal_ddlGrupoMant9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTopeCantidadMant" runat="server" Text="Tope Cantidad:"></asp:Label>
                                                </td>
                                                <td>                                                    
                                                    <asp:TextBox ID="txtTopeCantidadMant" runat="server" 
                                                        onkeypress="return isNumberKey(event)" Width="100px" TabIndex="10"></asp:TextBox>
                                                    <asp:Label ID="lblVal_ddlGrupoMant4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text="Estado:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_estado" runat="server" Width="200px" TabIndex="11">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblVal_ddlGrupoMant5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVigenciaInicio" runat="server" Text="Fecha Vigencia Inicio:"></asp:Label>
                                                </td>
                                                <td>
                                                   <SGAC_Fecha:ctrlDate ID="txtFecInicio" runat="server" />
                                                   <asp:Label ID="Label8" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVigenciaFin" runat="server" Text="Fecha Vigencia Fin:"></asp:Label>
                                                </td>
                                                <td>
                                                    <SGAC_Fecha:ctrlDate ID="txtFecFin" runat="server" />
                                                    <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>                                           
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Fórmula de Cálculo:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtObservaciones" runat="server" Width="635px" TabIndex="14" onKeyPress="return isFormula(event)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Habilitar: </td>
                                                <td>
                                                    <asp:CheckBox ID="chkHabilitaCantidad" runat="server" Text="Cantidad" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" Text="Cantidad Mínima:"></asp:Label>
                                                </td>
                                                <td>                                                    
                                                    <asp:TextBox ID="txtTopeMinimo" runat="server" 
                                                        onkeypress="return isNumberKey(event)" Width="100px" TabIndex="15" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="lblTopeMinimo" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                        </caption>
                                         <tr>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 <asp:CheckBox ID="chkExcepcion" runat="server" Text="Excepción" Checked="False"  />                                                        
                                             </td>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 &nbsp;</td>
                                         </tr>
                                    </table>
                                    <h3>
                                        <asp:Label ID="lblTituloTarifaRequisito" runat="server" Text="Requisitos"></asp:Label></h3>
                                    <table>
                                       <tr>
                                            <td id="tdMsge2" colspan="2">
                                                <asp:Label ID="lblMsjeValFunc" 
                                                           runat="server" 
                                                           Text="Debe ingresar los campos requeridos (*)"
                                                           CssClass="hideControl" 
                                                           ForeColor="Red" 
                                                           Font-Size="14px">
                                                </asp:Label>
                                            </td>
                                        </tr>                                    
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblRequisitoAccion" runat="server" Text="" CssClass="bold"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblRequisito" runat="server" Text="Requisito:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRequisitoMant" runat="server" Width="300px" 
                                                                TabIndex="15" />
                                                            <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTipoActa" runat="server" Text="Tipo Acta: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTipoActaMant" runat="server" Width="200px" 
                                                                TabIndex="16">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblVal_ddlGrupoMant7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCondicion" runat="server" Text="Condición:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCondicionMant" runat="server" Width="200px" 
                                                                TabIndex="17">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblVal_ddlGrupoMant8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:Button ID="btnAceptar" runat="server" Text="Añadir" Width="80px" 
                                                                CssClass="btnGeneral" onclick="btnAceptar_Click" TabIndex="18" 
                                                                OnClientClick="return validarRequisito();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gdvTarifarioRequisito" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" 
                                                                GridLines="None" Width="450px" 
                                                                ShowHeaderWhenEmpty="True" onrowcommand="gdvTarifarioRequisito_RowCommand">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                     <asp:BoundField  DataField="tare_sRequisitoId" HeaderText="Id" ItemStyle-CssClass="ColumnaOculta">                                                                     
                                                                        <HeaderStyle Width="100px" CssClass="ColumnaOculta" />
                                                                        <ItemStyle Width="100px" CssClass="ColumnaOculta" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField  DataField="tare_vRequisito" HeaderText="Requisito">
                                                                        <HeaderStyle Width="150px" />
                                                                        <ItemStyle Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField  DataField="tare_vTipoActa" HeaderText="Tipo Acta">
                                                                        <HeaderStyle Width="150px" />
                                                                        <ItemStyle Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField  DataField="tare_vCondicion" HeaderText="Condición">
                                                                        <HeaderStyle Width="100px" />
                                                                        <ItemStyle Width="100px" />
                                                                    </asp:BoundField>                                                                    
                                                                   <%-- <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEliminar" CommandName="Eliminar" ToolTip="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle Font-Names="Arial" Font-Size="11px" />
                                                                <EmptyDataTemplate>
                                                                    <table ID="tbSinDatosReq">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td width="10%">
                                                                                    <asp:Image ID="imgWarning" runat="server" 
                                                                                        ImageUrl="../Images/img_16_warning.png" />
                                                                                </td>
                                                                                <td width="5%">
                                                                                </td>
                                                                                <td width="85%">
                                                                                    <asp:Label ID="lblSinDatosReq" runat="server" Text="Sin Datos..."></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <SelectedRowStyle CssClass="slt" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                    <asp:Panel ID="pnlHistorico" runat="server">
                                        <h3>
                                            <asp:Label ID="Label3" runat="server" Text="HISTÓRICO TARIFARIO"></asp:Label>
                                        </h3>
                                        <asp:GridView ID="gdvHistorico" runat="server" AlternatingRowStyle-CssClass="alt"
                                            AutoGenerateColumns="False" CssClass="mGrid" Font-Size="10pt" GridLines="None"
                                            ShowHeaderWhenEmpty="True" Width="850px">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="tari_dVigenciaInicio" HeaderText="Fec. Vigencia Inicio" DataFormatString="{0:MMM-dd-yyyy}" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tari_dVigenciaFin" HeaderText="Fec. Vigencia Fin" DataFormatString="{0:MMM-dd-yyyy}" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tari_sBasePercepcionId" HeaderText="Base Percepción" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vCalculoTipoId" HeaderText="Tipo Cálculo" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderStyle Width="200px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tari_FCosto" HeaderText="Costo" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vTopeUnidadId" HeaderText="Tope Tipo" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tari_ITopeCantidad" HeaderText="Tope Cantidad" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle Font-Names="Arial" Font-Size="11px" />
                                            <EmptyDataTemplate>
                                                <table ID="tbSinDatosHis">
                                                    <tbody>
                                                        <tr>
                                                            <td width="10%">
                                                                <asp:Image ID="imgWarning" runat="server" 
                                                                           ImageUrl="../Images/img_16_warning.png" />
                                                            </td>
                                                            <td width="5%">
                                                            </td>
                                                            <td width="85%">
                                                                <asp:Label ID="lblSinDatosHis" runat="server" Text="Sin Datos..."></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>

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

        function Validar() {

            var bolValida = true;

            var strSeccionMant = $.trim($("#<%= ddlSeccionMant.ClientID %>").val());
            var strTarifaNumeroMant = $.trim($("#<%= txtTarifaNumeroMant.ClientID %>").val());
            var strTarifaLetraMant = $.trim($("#<%= txtTarifaLetraMant.ClientID %>").val());
            var strDescripcionMant = $.trim($("#<%= txtDescripcionMant.ClientID %>").val());
            var strNombreCortoMant = $.trim($("#<%= txtNombreCortoMant.ClientID %>").val());
            var strBasePercepcionMant = $.trim($("#<%= ddlBasePercepcionMant.ClientID %>").val());
            var strTipoCalculoMant = $.trim($("#<%= ddlTipoCalculoMant.ClientID %>").val());
            var strTarifaCostoMant = $.trim($("#<%= txtTarifaCostoMant.ClientID %>").val());
            var strTopeTipoMant = $.trim($("#<%= ddlTopeTipoMant.ClientID %>").val());
            var strTopeCantidadMant = $.trim($("#<%= txtTopeCantidadMant.ClientID %>").val());
            var strEstado = $.trim($("#<%= ddl_estado.ClientID %>").val());

            if (txtcontrolError(document.getElementById('<%= txtFecInicio.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;
            if (txtcontrolError(document.getElementById('<%= txtFecFin.FindControl("TxtFecha").ClientID %>')) == false) bolValida = false;

            if (strSeccionMant == '0') {
                $("#<%=ddlSeccionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlSeccionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTarifaNumeroMant.length == 0 && strTarifaLetraMant.length == 0) {
                $("#<%=txtTarifaNumeroMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTarifaNumeroMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDescripcionMant.length == 0) {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombreCortoMant.length == 0) {
                $("#<%=txtNombreCortoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombreCortoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strBasePercepcionMant == '0') {
                $("#<%=ddlBasePercepcionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlBasePercepcionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTipoCalculoMant == '0') {
                $("#<%=ddlTipoCalculoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTipoCalculoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTarifaCostoMant.length == 0) {
                $("#<%=txtTarifaCostoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTarifaCostoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTopeTipoMant == '0') {
                $("#<%=ddlTopeTipoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTopeTipoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTopeCantidadMant.length == 0) {
                $("#<%=txtTopeCantidadMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTopeCantidadMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strEstado == '0') {
                $("#<%=ddl_estado.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_estado.ClientID %>").css("border", "solid #888888 1px");
            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();               
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
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


        function validarRequisito() {

            var bolValida = true;

            var strSeccionMant = $.trim($("#<%= ddlSeccionMant.ClientID %>").val());
            var strTarifaNumeroMant = $.trim($("#<%= txtTarifaNumeroMant.ClientID %>").val());
            var strTarifaLetraMant = $.trim($("#<%= txtTarifaLetraMant.ClientID %>").val());
            var strDescripcionMant = $.trim($("#<%= txtDescripcionMant.ClientID %>").val());
            var strNombreCortoMant = $.trim($("#<%= txtNombreCortoMant.ClientID %>").val());
            var strBasePercepcionMant = $.trim($("#<%= ddlBasePercepcionMant.ClientID %>").val());
            var strTipoCalculoMant = $.trim($("#<%= ddlTipoCalculoMant.ClientID %>").val());
            var strTarifaCostoMant = $.trim($("#<%= txtTarifaCostoMant.ClientID %>").val());
            var strTopeTipoMant = $.trim($("#<%= ddlTopeTipoMant.ClientID %>").val());
            var strTopeCantidadMant = $.trim($("#<%= txtTopeCantidadMant.ClientID %>").val());
            var strEstado = $.trim($("#<%= ddl_estado.ClientID %>").val());

            if (strSeccionMant == '0') {
                $("#<%=ddlSeccionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlSeccionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTarifaNumeroMant.length == 0 && strTarifaLetraMant.length == 0) {
                $("#<%=txtTarifaNumeroMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTarifaNumeroMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDescripcionMant.length == 0) {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strNombreCortoMant.length == 0) {
                $("#<%=txtNombreCortoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNombreCortoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strBasePercepcionMant == '0') {
                $("#<%=ddlBasePercepcionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlBasePercepcionMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTipoCalculoMant == '0') {
                $("#<%=ddlTipoCalculoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTipoCalculoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTarifaCostoMant.length == 0) {
                $("#<%=txtTarifaCostoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTarifaCostoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTopeTipoMant == '0') {
                $("#<%=ddlTopeTipoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTopeTipoMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strTopeCantidadMant.length == 0) {
                $("#<%=txtTopeCantidadMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtTopeCantidadMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strEstado == '0') {
                $("#<%=ddl_estado.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_estado.ClientID %>").css("border", "solid #888888 1px");
            }

            var strRequisitoMant = $.trim($("#<%= ddlRequisitoMant.ClientID %>").val());
            var strTipoActaMant = $.trim($("#<%= ddlTipoActaMant.ClientID %>").val());
            var strCondicionMant = $.trim($("#<%= ddlCondicionMant.ClientID %>").val());

            if (strRequisitoMant == '0') {
                $("#<%=ddlRequisitoMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlRequisitoMant.ClientID %>").css("border", "solid #888888 1px");

                if (ValidarRequisito(strRequisitoMant) == false) {
                    $("#<%= lblMsjeValFunc.ClientID %>").html('No se puede registrar dos veces un requisito.');
                    $("#<%=ddlRequisitoMant.ClientID %>").focus();
                    $("#<%=ddlRequisitoMant.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddlRequisitoMant.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strTipoActaMant == '0') {
                $("#<%=ddlTipoActaMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlTipoActaMant.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strCondicionMant == '0') {
                $("#<%=ddlCondicionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlCondicionMant.ClientID %>").css("border", "solid #888888 1px");
            }
            
            if (bolValida) {
                $("#<%= lblMsjeValFunc.ClientID %>").hide();                
            }
            else {
                $("#<%= lblMsjeValFunc.ClientID %>").show();
            }

            return bolValida;
        }

        function ValidarRequisito(strRequisito) {

            var valor;

            var rprta = true;

            $("#<%= gdvTarifarioRequisito.ClientID %> tbody tr").each(function (index) {

                $(this).children("td").each(function (index2) {

                    if (index2 == 0) {

                        valor = $(this).text();

                        if (valor == strRequisito) {
                            rprta = false;
                        }
                    }
                })
            })

            return rprta;
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

        function isNumeroCosto(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                letra = false;
            }
            if (charCode == 13) {
                letra = false;
            }
            var letras = ".,";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isDescripcion(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
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
            if (charCode == 39) {
                letra = true;
            }
            if (charCode == 231) {
                letra = true;
            }
            if (charCode == 199) {
                letra = true;
            }
            var letras = "0123456789áéíóúÑÁÉÍÓÚ°.():,-/°_";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isFormula(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
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
            if (charCode == 39) {
                letra = true;
            }
            if (charCode == 231) {
                letra = true;
            }
            if (charCode == 199) {
                letra = true;
            }
            var letras = "0123456789áéíóúÑÁÉÍÓÚ°.():,/*-+";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }

        function isLetra(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 32) {
                letra = true;
            }
            if (charCode > 64 && charCode < 91) {
                letra = true;
            }
            if (charCode > 96 && charCode < 123) {
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
            if (charCode == 39) {
                letra = true;
            }
            if (charCode == 231) {
                letra = true;
            }
            if (charCode == 199) {
                letra = true;
            }
            var letras = "aeiouÑAEIOU";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            return letra;
        }     

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
    </script>
</asp:Content>
