<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmNorma.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmNorma" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="ctrlPageBar"
    TagPrefix="PageBar" %> 
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

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
        .style13
        {
            width: 168px;
        }
        .style16
        {
            width: 130px;
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

         
                  
         function limitar(e, contenido, caracteres) {
             // obtenemos la tecla pulsada
             var unicode = e.keyCode ? e.keyCode : e.charCode;

             // Permitimos las siguientes teclas:
             // 8 backspace
             // 46 suprimir
             // 13 enter
             // 9 tabulador
             // 37 izquierda
             // 39 derecha
             // 38 subir
             // 40 bajar


             if (unicode == 8 || unicode == 46 || unicode == 13 || unicode == 9 || unicode == 37 || unicode == 39 || unicode == 38 || unicode == 40) {                 
                 return true;
             }
             
             // Si ha superado el limite de caracteres devolvemos false
             if (contenido.length >= caracteres) {
                 return false;
             }
             
             return true;
         }
         function contar(e, contenido, caracteres) {
             var valor = limitar(e, contenido, caracteres);
             if (valor == true) {
                 document.getElementById("spanLimiteTexto").innerHTML = "Límite de texto: " + (caracteres - contenido.length).toString() + " de " + caracteres.toString();
             }
             else {
                 document.getElementById("spanLimiteTexto").innerHTML = "Límite de texto: 0 de " + caracteres.toString();
             }
             return valor;
         }
    </script>
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="NORMAS"></asp:Label></h2>
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
                                                <asp:Label ID="lblConsultaTipoNorma" runat="server" Text="Tipo de Norma:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlTipoNorma" runat="server" Width="220px">
                                                </asp:DropDownList>
                                                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">                                               
                                                    <asp:Label ID="lblConsultaNorma" runat="server" Text="Norma:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                             <asp:TextBox ID="txtConsultaNorma" runat="server" Width="600px"
                                                    CssClass="txtLetra" MaxLength="400" ></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblConsultaFechaInicial" runat="server" Text="Fecha inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpConsultaFechaInicio" runat="server" />
                                            </td>
                                            <td class="style10">
                                                <asp:Label ID="lblConsultaFechaFin" runat="server" Text="Fecha fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpConsultaFechaFin" runat="server" />
                                            </td>
                                        </tr>
                                       <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblConsultaEstado" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                             <asp:DropDownList ID="ddlConsultaEstadoNorma" runat="server" Width="220px">
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
                                                <asp:GridView ID="gdvNormas" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvNormas_RowCommand">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>                           
                                                                                 
                                                        <asp:BoundField DataField="TIPO_NORMA" HeaderText="Tipo">
                                                            <HeaderStyle Width="80px" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="norm_vDescripcionCorta" HeaderText="Norma">
                                                            <HeaderStyle Width="300px" />
                                                            <ItemStyle Width="300px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="norm_vDescripcion" HeaderText="Descripción">
                                                            <HeaderStyle Width="400px" />
                                                            <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="estado" HeaderText="Estado">
                                                            <HeaderStyle Width="70px"  HorizontalAlign="Center" />
                                                            <ItemStyle Width="70px"  HorizontalAlign="Center"/>
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
                                            <td class="style16">
                                                <asp:Label ID="lblregTipoNorma" runat="server" Text="Tipo Norma:"></asp:Label>

                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlregTipoNorma" runat="server" Width="220px" 
                                                    AutoPostBack="True" 
                                                    onselectedindexchanged="ddlregTipoNorma_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lblregObjetoNorma" runat="server" Text="Objeto Norma:"></asp:Label>
                                            </td>
                                             <td colspan="3">
                                                  <asp:DropDownList ID="ddlregObjetoNorma" runat="server" Width="605px">
                                                    </asp:DropDownList>
                                                    <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">                                                
                                                <asp:Label ID="lblregArticulo" runat="server" Text="Artículo:"></asp:Label>                                                
                                            </td>
                                            <td class="style13">
                                               <asp:TextBox ID="txtregArticulo" runat="server" Width="150px" CssClass="txtLetra" 
                                                                onpaste="return false" 
                                                                MaxLength="10" ></asp:TextBox>                                                
                                            </td>
                                            <td style="width:120px;text-align:right;">
                                                <asp:Label ID="lblregInciso" runat="server" Text="Inciso:"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                               <asp:TextBox ID="txtregInciso" runat="server" Width="100px" CssClass="txtLetra" 
                                                                onpaste="return false" 
                                                                MaxLength="5" ></asp:TextBox>
                                            
                                            </td>
                                            
                                            
                                        </tr>
                                        <tr>
                                            <td class="style16">   
                                                <asp:Label ID="lblregNombreArticulo" runat="server" Text="Nombre Artículo:"></asp:Label>                                              
                                            </td>
                                            <td colspan="3">
                                               <asp:TextBox ID="txtregNombreArticulo" runat="server" Width="600px" CssClass="txtLetra" 
                                                                onpaste="return false" 
                                                                MaxLength="250" ></asp:TextBox>
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lblregNorma" runat="server" Text="Norma:"></asp:Label>                                              
                                            </td>
                                            <td colspan="3">
                                               <asp:TextBox ID="txtregDescripcionCorta" runat="server" Width="600px" CssClass="txtLetra" 
                                                                onpaste="return false" 
                                                                MaxLength="200" ></asp:TextBox>
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lblregDescripcion" runat="server" Text="Descripción:"></asp:Label>                                              
                                            </td>
                                            <td colspan="3">
                                               <asp:TextBox ID="txtregDescripcion" runat="server" Width="600px" CssClass="txtLetra"  
                                                                onKeyUp="return contar(event,this.value,1000)" onKeyDown="return contar(event,this.value,1000)"
                                                                MaxLength="1000" Height="100px" TextMode="MultiLine" ></asp:TextBox>
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                &nbsp;</td>
                                            <td colspan="3">                                                
                                                <span id="spanLimiteTexto"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lblregFechaInicial" runat="server" Text="Fecha Vigencia Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpregFechaInicio" runat="server" />
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                            <td style="text-align:right">
                                                <asp:Label ID="lblregFechaFin" runat="server" Text="Fecha Vigencia Fin:"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <SGAC_Fecha:ctrlDate ID="dtpregFechaFin" runat="server" />
                                                <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lblregEstado" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlregEstadoNorma" runat="server" Width="220px">
                                                    </asp:DropDownList>
                                                    <label style="color:Red; font-size:medium"> &nbsp;*</label>
                                                <asp:HiddenField ID="HF_sNormaId" runat="server" />
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

            var strregTipoNorma = $.trim($("#<%= ddlregTipoNorma.ClientID %>").val());
            var strregObjetoNorma = $.trim($("#<%= ddlregObjetoNorma.ClientID %>").val());
            var strregNombreArticulo = $.trim($("#<%= txtregNombreArticulo.ClientID %>").val());
            var strregDescripcionCorta = $.trim($("#<%= txtregDescripcionCorta.ClientID %>").val());
            

            var strregFechaInicio = $.trim($('#<%=dtpregFechaInicio.FindControl("TxtFecha").ClientID %>').val());
            var txtregFechaInicio = document.getElementById('<%= dtpregFechaInicio.FindControl("TxtFecha").ClientID %>');

            var strregFechaFin = $.trim($('#<%=dtpregFechaFin.FindControl("TxtFecha").ClientID %>').val());
            var txtregFechaFin = document.getElementById('<%= dtpregFechaFin.FindControl("TxtFecha").ClientID %>');

            var strregEstadoNorma = $.trim($("#<%= ddlregEstadoNorma.ClientID %>").val());

            if (strregTipoNorma == 0) {
                $("#<%=ddlregTipoNorma.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregTipoNorma.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strregObjetoNorma == 0) {
                $("#<%=ddlregObjetoNorma.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregObjetoNorma.ClientID %>").css("border", "solid #888888 1px");
            }
            if (strregDescripcionCorta.length == 0) {
                $("#<%=txtregDescripcionCorta.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtregDescripcionCorta.ClientID %>").css("border", "solid #888888 1px");
            }

           
            if (strregFechaInicio == "") {
                txtregFechaInicio.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtregFechaInicio.style.border = "1px solid #888888";
            }

            if (strregFechaFin == "") {
                txtregFechaFin.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtregFechaFin.style.border = "1px solid #888888";
            }

            if (strregEstadoNorma == 0) {
                $("#<%=ddlregEstadoNorma.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddlregEstadoNorma.ClientID %>").css("border", "solid #888888 1px");
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