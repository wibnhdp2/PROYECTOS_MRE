<%@ Page Title="" Language="C#" uiCulture="es-PE" culture="es-PE"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTipoActoProtocolarTarifario.aspx.cs" 
Inherits="SGAC.WebApp.Configuracion.FrmTipoActoProtocolarTarifario" %>

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
        .style10
        {
            width: 395px;
        }
        #contadorTarifasMarcadas
        {
            width: 251px;
            margin-left: 0px;
        }
        #contadorOficinasMarcadas
        {
            width: 464px;
        }
        .style20
        {
            width: 270px;
        }
        .style23
        {
            width: 218px;
        }
        .style26
        {
            width: 156px;
        }
        .style27
        {
            width: 162px;
        }
        .style28
        {
            width: 113px;
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
        <%--Cuerpo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="TARIFAS POR TIPO DE ACTO PROTOCOLAR"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%-- Consulta --%>
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
                                            <td class="style26">
                                                <asp:Label ID="lblConsultaTipoPago" runat="server" Text="Tipo de Acto Protocolar:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConsultaTipoActoprotocolar" runat="server" Width="490px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td class="style26">
                                                <asp:Label ID="lblConsultaTarifa" runat="server" Text="Tarifa Consular:"></asp:Label>
                                        </td>
                                        <td>
                                                <asp:DropDownList ID="ddlConsultaTarifa" runat="server" Width="490px">
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
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdvTipoActoProtocolar" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvTipoActoProtocolar_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>                           
                                                                                 
                                                        <asp:BoundField DataField="TIPO_ACTO" HeaderText="Tipo de Acto Protocolar" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle Width="300px" />
                                                            <ItemStyle Width="300px" HorizontalAlign="Left"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="tari_vDescripcionCorta" HeaderText="Tarifa" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle Width="500px" />
                                                            <ItemStyle Width="500px" HorizontalAlign="Left" />
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
                                            <td class="style27">
                                                    <asp:Label ID="lblregTipoActoProtocolar" runat="server" Text="Tipo de Acto Protocolar:"></asp:Label>
                                               </td>
                                            <td>
                                               <asp:DropDownList ID="ddlregTipoActoProtocolar" runat="server" Width="490px">
                                               </asp:DropDownList> 
                                               <label style="color:Red; font-size:medium">
                                                &nbsp;*</label>
                                                </td>
                                    </tr>
                                   </table>
                                    <asp:HiddenField ID="HF_TipoActoProtocolarId" runat="server" />
                                   <table>

                                        <tr>
                                            <td colspan="4">
                                                <table>
                                                    <tr>
                                                    <td class="style20">
                                                        <asp:Label ID="lblregTarifa" runat="server" Text="Tarifario Consular:"></asp:Label>         
                                                    </td>
                                                    <td class="style28">
                                                    
                                                    </td>
                                                    <td class="style23">
                                                            
                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                    
                                                    </tr>
                                                    <tr>
                                                        <td class="style20">
                                                        <asp:CheckBox ID="chkSoloTarifasSeleccionadas" runat="server" AutoPostBack="True" 
                                                                        oncheckedchanged="chkSoloTarifasSeleccionadas_CheckedChanged" 
                                                                        Text="Mostrar solo las seleccionadas" />
                                                        </td>
                                                        <td class="style28">
                                                        </td>
                                                        <td class="style23">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </td>
                                            
                                        </tr>

                                       
                                        <tr>
                                            <td style="vertical-align:top" colspan="5">
                                           
                                                <table style="width: 360px">
                                                    <tr>
                                                        <td>
                                                            <div style="height:350px; width: 600px; border: 1px solid #ddd; overflow-y: scroll;">
                                                                <asp:GridView ID="gdvTarifario" runat="server" 
                                                                    AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
                                                                    DataKeyNames="tari_sTarifarioId" GridLines="None" 
                                                                    onrowdatabound="gdvTarifario_RowDataBound" SelectedRowStyle-CssClass="slt" 
                                                                    Width="550px">
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="tari_vdescripcioncorta" HeaderText="Tarifa" 
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle Width="500px" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="500px" />
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

            var strregTipoActoProtocolar = $.trim($("#<%= ddlregTipoActoProtocolar.ClientID %>").val());


            if (strregTipoActoProtocolar == 0) {
                $("#<%=ddlregTipoActoProtocolar.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregTipoActoProtocolar.ClientID %>").css("border", "solid #888888 1px");
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