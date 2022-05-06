<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmUbicacionGeografica.aspx.cs"
    Inherits="SGAC.WebApp.Configuracion.UbicacionGeografica" %>

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

         function validarSoloLetras(lbl, txt) {            
             var charpos = txt.value.search("[^A-Za-z ]");

             if (txt.value.length > 0 && charpos >= 0) {
                 lbl.style.display = 'inline';
                 txt.value = '';
             }
             else {
                 lbl.style.display = 'none';
             }
         }
         function isLetras(lbl, txt) {
             var charpos = txt.value.toUpperCase().search("[^A-Z]");

             if (txt.value.length > 0 && charpos >= 0) {
                 lbl.style.display = 'inline';
                 txt.value = '';
             }
             else {
                 lbl.style.display = 'none';
             }
         }
    </script>
   
    <div>
        <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloUbGeografica" runat="server" Text="Ubicación Geográfica"></asp:Label></h2>
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
                                            <td>
                                                <asp:Label ID="lblDptoCont" runat="server" Text="Departamento/Continente:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtDptoContDescripcion" runat="server" Width="300px"
                                                    CssClass="txtLetra"  MaxLength="50" ></asp:TextBox>
                                                    
                                                    <label id="lblDepaContiMensaje" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProvPais" runat="server" Text="Provincia/País:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtProvPaisDescripcion" runat="server" Width="300px" 
                                                    CssClass="txtLetra"   MaxLength="50" TabIndex="1"></asp:TextBox>
                                                    
                                                    <label id="lblProvPaisMensaje" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDistritoCiudad" runat="server" Text="Distrito/Ciudad:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtCiudadDescripcion" runat="server" Width="300px" 
                                                    CssClass="txtLetra"   MaxLength="50" TabIndex="2"></asp:TextBox>
                                                    
                                                    <label id="lblDistCiudadMensaje" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                               <asp:CheckBox ID="chkEstadoConsulta" Text="Activo" runat="server" Checked="True" />
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
                                                <asp:GridView ID="gdvUbicacionGeografica" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                    OnRowCommand="gdvUbicacionGeografica_RowCommand" OnRowDataBound="gdvUbicacionGeografica_RowDataBound">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ubge_cCodigo" HeaderText="Codigo" />
                                                        <asp:BoundField DataField="ubge_vDepartamento" HeaderText="Dpto/Continente">
                                                            <HeaderStyle Width="350px" />
                                                            <ItemStyle Width="350px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ubge_vProvincia" HeaderText="Prov/País">
                                                            <HeaderStyle Width="350px" />
                                                            <ItemStyle Width="350px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ubge_vDistrito" HeaderText="Distrito/Ciudad">
                                                            <HeaderStyle Width="350px" />
                                                            <ItemStyle Width="350px" />
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
                                                        <asp:TemplateField HeaderText="Activar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnActivar" CommandName="Activar" ToolTip="Activar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_16_success.png" />
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
                                            <td>
                                                <asp:Label ID="lblDptoCont2" runat="server" Text="Dpto/Continente:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtDptoContNumero2" runat="server" Width="50px" CssClass="campoNumero"
                                                    onkeypress="return isNumberKey(event)" onpaste="return false" 
                                                    MaxLength="2" TabIndex="3"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                <asp:TextBox ID="txtDptoContDescripcion2" runat="server" Width="300px" 
                                                    CssClass="txtLetra"  MaxLength="50" TabIndex="4"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <label id="lblDepaContiMensaje2" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProvPais2" runat="server" Text="Provincia/País:"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtProvPaisNumero2" runat="server" Width="50px" CssClass="campoNumero"
                                                    onkeypress="return isNumberKey(event)" onpaste="return false" 
                                                    MaxLength="2" TabIndex="5"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                <asp:TextBox ID="txtProvPaisDescripcion2" runat="server" Width="300px" 
                                                    CssClass="txtLetra"  MaxLength="50" TabIndex="6"></asp:TextBox>
                                                    <label style="color:Red; font-size:medium"> *</label>
                                                    <label id="lblProvPaisMensaje2" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDistritoCiudad2" runat="server" Text="Distrito/Ciudad:"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtCiudadNumero2" runat="server" Width="50px" CssClass="campoNumero"
                                                    onkeypress="return isNumberKey(event)" onpaste="return false" 
                                                    MaxLength="2" TabIndex="7"></asp:TextBox>
                                                <label style="color:Red; font-size:medium"> *</label>
                                                <asp:TextBox ID="txtCiudadDescripcion2" runat="server" Width="300px" 
                                                    CssClass="txtLetra"  MaxLength="50"></asp:TextBox>
                                                <label style="color:Red; font-size:medium"> *</label>
                                                <label id="lblDistCiudadMensaje2" style="color:Red; font-size:small; vertical-align: middle; display:none" >  Solo se permiten letras.</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSiglaPais" runat="server" Text="Sigla(ISO 3166):"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                    <asp:TextBox ID="txtregISOLetra" runat="server" Width="50px" CssClass="txtLetra" onblur ="return isLetras(lblMensajeregISOLetra , this)"
                                                            onkeypress="return validarsololetra(event)" onpaste="return false" 
                                                                MaxLength="3" TabIndex="12"></asp:TextBox>  
                                                <label style="color:Red; font-size:medium"> *</label>  
                                                <label id="Label1" style="color:Black; font-size:small; vertical-align: middle;" > (Tres caracteres que identifican al País).</label>                                                                                                            
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

            var strDptoContNumero2 = $.trim($("#<%= txtDptoContNumero2.ClientID %>").val());
            var strDptoContDescripcion2 = $.trim($("#<%= txtDptoContDescripcion2.ClientID %>").val());
            var strProvPaisNumero2 = $.trim($("#<%= txtProvPaisNumero2.ClientID %>").val());
            var strProvPaisDescripcion2 = $.trim($("#<%= txtProvPaisDescripcion2.ClientID %>").val());
            var strCiudadNumero2 = $.trim($("#<%= txtCiudadNumero2.ClientID %>").val());
            var strCiudadDescripcion2 = $.trim($("#<%= txtCiudadDescripcion2.ClientID %>").val());

            if (strDptoContNumero2.length == 0) {
                $("#<%=txtDptoContNumero2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDptoContNumero2.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strDptoContDescripcion2.length == 0) {
                $("#<%=txtDptoContDescripcion2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDptoContDescripcion2.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strProvPaisNumero2.length == 0) {
                $("#<%=txtProvPaisNumero2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtProvPaisNumero2.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strProvPaisDescripcion2.length == 0) {
                $("#<%=txtProvPaisDescripcion2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtProvPaisDescripcion2.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strCiudadNumero2.length == 0) {
                $("#<%=txtCiudadNumero2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtCiudadNumero2.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strCiudadDescripcion2.length == 0) {
                $("#<%=txtCiudadDescripcion2.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtCiudadDescripcion2.ClientID %>").css("border", "solid #888888 1px");
            }
            
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();                
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
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

        function isNombre(evt) {
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
            var letras = "áéíóúüñÑÁÉÍÓÚ";
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