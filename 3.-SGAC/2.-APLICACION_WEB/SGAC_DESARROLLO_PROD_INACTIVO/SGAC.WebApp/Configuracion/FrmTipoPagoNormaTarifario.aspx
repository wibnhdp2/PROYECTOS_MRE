<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTipoPagoNormaTarifario.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmTipoPagoNormaTarifario" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="PageBar" %> 

<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style2
        {
            width: 119px;
        }
        .style10
        {
            width: 395px;
        }
        #contadorTarifasMarcadas
        {
            width: 251px;
            margin-left: 0px;
        }
        .style19
        {
            width: 251px;
        }
        #contadorOficinasMarcadas
        {
            width: 464px;
        }
        .style20
        {
            width: 270px;
        }
        .style22
        {
            width: 24px;
        }
        .style23
        {
            width: 218px;
        }
        .style24
        {
            width: 287px;
        }
        .style25
        {
            width: 451px;
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

             $(function () {
                 ContarCheckboxMarcados('gdvNorma', '#contadorNormasMarcadas');
                 ContarCheckboxMarcados('gdvTarifario', '#contadorTarifasMarcadas');
             });
         }

        

         function ContarCheckboxMarcados(gdv, etiqueta, obj) {
             var intContadorHead = 0;
             var intContador = 0;

             $('table[id$=' + gdv + '] tr td').each(function () {

                 if ($(this).find('input[type=checkbox]').is(':checked')) {
                     intContador++;
                 }
             });

             if (intContador > 1) {
                 $(etiqueta).html(intContador.toString() + ' seleccionadas');
             } else {
                 $(etiqueta).html(intContador.toString() + ' seleccionada');
             }
             
         }
    </script>
   
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="TIPO DE PAGO - NORMA - TARIFA"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%--Cuerpo--%>
        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">Consulta</a></li>
                            <li><a href="#tab-2">Registro</a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%-- Consulta --%>
                                    <table>
                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblConsultaTipoPago" runat="server" Text="Tipo de Pago:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConsultaTipoPago" runat="server" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>
                                                <asp:Label ID="lblConsultaTarifa" runat="server" Text="Tarifa:"></asp:Label>
                                        </td>
                                        <td>
                                                <asp:DropDownList ID="ddlConsultaTarifa" runat="server" Width="490px">
                                                </asp:DropDownList>                                                    
                                        </td>
                                       </tr>                                      
                                      <tr>
                                            <td>                                               
                                                <asp:Label ID="lblConsultaNorma" runat="server" Text="Norma:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConsultaNorma" runat="server" Width="490px">
                                                </asp:DropDownList>                                                    
                                            </td>
                                        </tr>                                                                              
                                        
                                       
                                        <tr>
                                            <td>
                                               <asp:Label ID="lblconsultaExcepcion" runat="server" Text="Excepción:"></asp:Label> 
                                             </td>
                                            <td>
                                                <asp:CheckBox ID="chkconsultaExcepcion" runat="server" Text="Activo" 
                                                    TextAlign="Right" />                                                
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
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvTipoPagoNorma" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvTipoPagoNorma_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>                           
                                                                                 
                                                        <asp:BoundField DataField="TARIFA" HeaderText="Tarifa" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle Width="60px" />
                                                            <ItemStyle Width="60px" HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TIPO_PAGO" HeaderText="Tipo de Pago" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="norm_vDescripcionCorta" HeaderText="Norma">
                                                            <HeaderStyle Width="500px" />
                                                            <ItemStyle Width="500px" />
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
                                                <PageBar:ctrlPageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
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
                                        <td id="tdMsje" colspan="2">
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
                                            <td class="style2">
                                                    <asp:Label ID="lblregTipoPago" runat="server" Text="Tipo de Pago:"></asp:Label>
                                               </td>
                                            <td>
                                               <asp:DropDownList ID="ddlregTipoPago" runat="server" Width="490px">
                                               </asp:DropDownList> 
                                               <label style="color:Red; font-size:medium">
                                                &nbsp;*</label>
                                                </td>
                                    </tr>
                                   </table>
                                    <asp:HiddenField ID="HF_TipoPagoId" runat="server" />
                                    <asp:HiddenField ID="HF_NormaId" runat="server" />
                                   <table>

                                        <tr>
                                            <td colspan="5">
                                                <table>
                                                    <tr>
                                                    <td class="style20">
                                                        <asp:Label ID="lblregTarifa" runat="server" Text="Tarifa:"></asp:Label>         
                                                    </td>
                                                    <td class="style22">
                                                    
                                                    </td>
                                                    <td class="style23">
                                                            <asp:Label ID="lblregNorma" runat="server" Text="Norma:"></asp:Label>    
                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                    <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style20">
                                                        <asp:CheckBox ID="chkSoloTarifasSeleccionadas" runat="server" AutoPostBack="True" 
                                                                        oncheckedchanged="chkSoloTarifasSeleccionadas_CheckedChanged" 
                                                                        Text="Mostrar solo las seleccionadas" />
                                                        </td>
                                                        <td class="style22">
                                                        </td>
                                                        <td class="style23">
                                                        <asp:CheckBox ID="chkSoloNormasSeleccionadas" runat="server" AutoPostBack="True" 
                                                                                oncheckedchanged="chkSoloNormasSeleccionadas_CheckedChanged" 
                                                                                Text="Mostrar solo las seleccionadas" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </td>
                                        </tr>

                                       
                                        <tr>
                                            <td style="vertical-align:top" colspan="2">
                                                <table style="width: 291px">
                                                    <tr>
                                                        <td>
                                                            <div style="height:350px; width: 257px; border: 1px solid #ddd; overflow-y: scroll;">
                                                                <asp:GridView ID="gdvTarifario" runat="server" 
                                                                    AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
                                                                    DataKeyNames="tari_sTarifarioId" GridLines="None" 
                                                                    onrowdatabound="gdvTarifario_RowDataBound" SelectedRowStyle-CssClass="slt" 
                                                                    Width="238px">
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="tarifa" HeaderText="Tarifa" 
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="tari_FCosto" HeaderText="Costo" 
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkSeleccionarTodasTarifas" runat="server" 
                                                                                    AutoPostBack="True" 
                                                                                    oncheckedchanged="chkSeleccionarTodasTarifas_CheckedChanged" Text="Seleccionar" 
                                                                                    TextAlign="Left" ToolTip="Seleccionar todas las tarifas" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle VerticalAlign="Middle" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSeleccionarTarifa" runat="server" ToolTip="Seleccionar la Tarifa" onclick="ContarCheckboxMarcados('gdvTarifario','#contadorTarifasMarcadas')"/>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("tari_sTarifarioId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <SelectedRowStyle CssClass="slt" />
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                        <td style="vertical-align:top; " class="style19">
                                                            <label style="color:Red; font-size:medium">
                                                            &nbsp;*</label>                                                                                                                        
                                                        </td>
                                                    </tr>
                                                </table>                                    
                                               
                                            </td>
                                            <td style="vertical-align:top" colspan="3">
                                              <table style="width: 500px">
                                                    <tr>
                                                        <td class="style10">
                                                            <div style="height:350px; width: 473px; border: 1px solid #ddd; overflow-y: scroll;">
                                                                <asp:GridView ID="gdvNorma" runat="server" 
                                                                    AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
                                                                    DataKeyNames="norm_sNormaId" GridLines="None" 
                                                                    onrowdatabound="gdvNorma_RowDataBound" SelectedRowStyle-CssClass="slt">
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="norm_vDescripcionCorta" HeaderText="Norma" 
                                                                            ItemStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle Width="350px" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkSeleccionarTodasNormas" runat="server" 
                                                                                    AutoPostBack="True" 
                                                                                    oncheckedchanged="chkSeleccionarTodasNormas_CheckedChanged" Text="Seleccionar" 
                                                                                    TextAlign="Left" ToolTip="Seleccionar todas las Normas" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle VerticalAlign="Middle" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSeleccionarNorma" runat="server" ToolTip="Seleccionar la Norma" onclick="ContarCheckboxMarcados('gdvNorma','#contadorNormasMarcadas')" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("norm_sNormaId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <SelectedRowStyle CssClass="slt" />
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                        <td style="vertical-align:top; width:250px">
                                                            
                                                            
                                                        </td>
                                                    </tr>
                                                   
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="5">
                                                <table>
                                                    <tr>
                                                        <td align="right" style="padding-right:40px">
                                                                <div id="contadorTarifasMarcadas"></div>
                                                        </td>
                                                        <td align="right" style="padding-right:0px">
                                                              <div id="contadorNormasMarcadas"></div>
                                                        </td>
                                                    </tr>
                                                    <tr>                                                        
                                                        <td class="style24">
                                                            <asp:CheckBox ID="chkExcepcionTarifa" runat="server" Text ="Excepciones" 
                                                                AutoPostBack="True" oncheckedchanged="chkExcepcionTarifa_CheckedChanged" /> 
                                                        </td>
                                                        <td class="style25">
                                            
                                                        </td>                                                        
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkTarifaConCosto" runat="server" Text="Con costo" 
                                                                AutoPostBack="True" oncheckedchanged="chkTarifaConCosto_CheckedChanged" /> 
                                                        </td>
                                                        <td class="style25"></td>
                                                    </tr>                                                        
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkTarifaSinCosto" runat="server" Text="Sin costo" 
                                                                AutoPostBack="True" oncheckedchanged="chkTarifaSinCosto_CheckedChanged" /> 
                                                        </td>
                                                        <td class="style25"></td>
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
    </div>
    <br />
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#tabs').tabs();
        });

        function Validar() {

            var bolValida = true;

            var strregTipoPago = $.trim($("#<%= ddlregTipoPago.ClientID %>").val());


            if (strregTipoPago == 0) {
                $("#<%=ddlregTipoPago.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregTipoPago.ClientID %>").css("border", "solid #888888 1px");
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
        }


        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
             
    </script>
</asp:Content>