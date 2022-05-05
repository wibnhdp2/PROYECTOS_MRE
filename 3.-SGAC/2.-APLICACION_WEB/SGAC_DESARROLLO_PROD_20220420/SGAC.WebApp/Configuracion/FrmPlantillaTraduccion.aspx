<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmPlantillaTraduccion.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmPlantillaTraduccion" %>

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
            width: 73px;
        }
        .style11
        {
            width: 120px;
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
         }

       
      
         
       
    </script>
   
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Plantilla de Traducción de Formatos"></asp:Label></h2>
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
                                                <asp:Label ID="lblConsultaPlantilla" runat="server" Text="Plantilla:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlConsultaPlantilla" runat="server" TabIndex="2" 
                                                        Width="400px">
                                                </asp:DropDownList>                                                                                                        
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td> 
                                            <asp:Label ID="lblConsultaIdioma" runat="server" Text="Idioma:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConsultaIdioma" runat="server" TabIndex="2" 
                                                        Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="style10">                                               
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                       
                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivo" runat="server" Checked="True" Text="Activo" />
                                            </td>
                                            <td class="style10">
                                            </td>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                
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
                                                <asp:GridView ID="gdvTraduccion" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvTraduccion_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="Nro.">
                                                           <HeaderStyle Width="50px"  HorizontalAlign="Center"/>
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="etiq_vEtiqueta" HeaderText="Etiqueta">
                                                            <HeaderStyle Width="400px" HorizontalAlign="Left"/>
                                                            <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pltr_vTraduccion" HeaderText="Traducción">
                                                            <HeaderStyle Width="400px" HorizontalAlign="Left"/>
                                                            <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IDIOMA" HeaderText="Idioma">
                                                            <HeaderStyle Width="150px" HorizontalAlign="Left"/>
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="pltr_cEstado" HeaderText="Estado">
                                                            <HeaderStyle Width="50px"  HorizontalAlign="Center"/>
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
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
                                    <asp:HiddenField ID="HF_PlantillaTraduccionId" runat="server" />
                                    <table>
                                        <tr>
                                            <td id="tdMsje" colspan="5">
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
                                            <td class="style11">
                                                <asp:Label ID="lblregPlantilla" runat="server" Text="Plantilla:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="ddlregPlantilla" runat="server" TabIndex="2" 
                                                        Width="400px" 
                                                    onselectedindexchanged="ddlregPlantilla_SelectedIndexChanged" 
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                    <label style="color:Red; font-size:medium"> &nbsp;*</label>                                                   
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                <asp:Label ID="lblregEtiqueta" runat="server" Text="Etiqueta a Traducir:"></asp:Label>
                                            </td>
                                             <td colspan="4">
                                                <asp:DropDownList ID="ddlregEtiqueta" runat="server" TabIndex="2" 
                                                        Width="700px">
                                                </asp:DropDownList>
                                                    <label style="color:Red; font-size:medium"> &nbsp;*</label>                                                    
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                <asp:Label ID="lblregIdioma" runat="server" Text="Traducir al Idioma:" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlregIdioma" runat="server" TabIndex="2" 
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>   
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                <asp:Label ID="lblregTraduccion" runat="server" Text="Traducción:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                            <asp:TextBox ID="txtregTraduccion" runat="server" Width="650px"                                                     
                                                    MaxLength="200" 
                                                    TabIndex="2"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> &nbsp;*</label>   
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                <asp:Label ID="lblEstadoMant" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivoMant" runat="server" Checked="true" Text="Activo" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                     <table>
                                        <tr>
                                            <td>
                                                <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                            </td>
                                        </tr>
                                    </table>

                                    <table>
                                        <tr>
                                            <td style="width: 870px;">
                                                <Label:Validation ID="ctrlValidationEtiqueta" runat="server" />
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

            var strregPlantilla = $.trim($("#<%= ddlregPlantilla.ClientID %>").val());
            var strregEtiqueta = $.trim($("#<%= ddlregEtiqueta.ClientID %>").val());
            var strregIdioma = $.trim($("#<%= ddlregIdioma.ClientID %>").val());
            var strregTraduccion = $.trim($("#<%= txtregTraduccion.ClientID %>").val());
           
            if (strregPlantilla == 0) {
                $("#<%=ddlregPlantilla.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregPlantilla.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregEtiqueta == 0) {
                $("#<%=ddlregEtiqueta.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregEtiqueta.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregIdioma == 0) {
                $("#<%=ddlregIdioma.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregIdioma.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strregTraduccion.length == 0) {
                $("#<%=txtregTraduccion.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregTraduccion.ClientID %>").css("border", "solid #888888 1px");
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