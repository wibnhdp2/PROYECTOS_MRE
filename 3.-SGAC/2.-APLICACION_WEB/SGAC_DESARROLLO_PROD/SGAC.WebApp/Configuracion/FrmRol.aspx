<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmRol.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmRol" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlPageBar.ascx" tagname="PageBar" tagprefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
 
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

    </script>
   
    <div>
        
         <%--Titulo--%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblRol" runat="server" Text="Roles"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>

        <%--Cuerpo--%>
        <table style="width: 90%" align="center">

            <tr>

                <td>

                    <div id="tabs">

                        <ul>
                            <li><a href="#tab-1"><asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2"><asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>

                        <div id="tab-1">

                        <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">

                            <ContentTemplate>

                                <%-- Consulta --%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAplicacionCons" runat="server" Text="Aplicación:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAplicacionCons" runat="server" Width="255px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEstado" runat="server" Text="Estado:" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkRolActivo" runat="server" Text="Activo" Checked="True" />
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
                                    
                                <%--Mensaje Validación--%>  
                                <table>  
                                    <tr>
                                        <td style="width: 870px;">
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                                    
                                <%-- Grilla --%>
                                <table>                                    

                                    <tr>

                                        <td>

                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" ChildrenAsTriggers="true" updatemode="Conditional">

                                                <ContentTemplate>

                                                    <asp:GridView ID="gdvRolConfiguracion" runat="server"
                                                                    CssClass="mGrid"
                                                                    AlternatingRowStyle-CssClass="alt" 
                                                                    SelectedRowStyle-CssClass="slt"
                                                                    AutoGenerateColumns="False"
                                                                    GridLines="None"
                                                                    onrowcommand="gdvRolConfiguracion_RowCommand">

                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                                                        <Columns>                                       
                                                            <asp:BoundField DataField="roco_vAplicacion" HeaderText="Aplicación" >
                                                            <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="roco_vRolTipo" HeaderText="Tipo Rol" > 
                                                            <ItemStyle Width="250px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="roco_vNombre" HeaderText="Nombre" > 
                                                            <ItemStyle Width="400px" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate> 
                                                                    <asp:ImageButton ID="btnConsultar" 
                                                                        CommandName="Consultar" tooltip="Consultar"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" tooltip="Editar"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>                                    
                                                        <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
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

                                    <toolbar:toolbarcontent ID="ctrlToolBarMantenimiento" runat="server"></toolbar:toolbarcontent>

                                    <table> 

                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos." CssClass="hideControl" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAplicacionMant" runat="server" Text="Aplicación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAplicacionMant" runat="server" Width="200px" 
                                                    AutoPostBack="True" 
                                                    onselectedindexchanged="ddlAplicacionMant_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoRol" runat="server" Text="Tipo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoRol" runat="server" Width="200px"></asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlTipoRol" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombreMant" runat="server" Text="Nombre:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRolNombreMant" runat="server" Width="400px" CssClass="txtLetra" onkeypress="return ValidarLetras(event)"></asp:TextBox>
                                                <asp:Label ID="lblVal_txtRolNombreMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHorarioMant" runat="server" Text="Horario:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk01" Text="01" runat="server" />
                                                <asp:CheckBox ID="chk02" Text="02" runat="server" />
                                                <asp:CheckBox ID="chk03" Text="03" runat="server" />
                                                <asp:CheckBox ID="chk04" Text="04" runat="server" />
                                                <asp:CheckBox ID="chk05" Text="05" runat="server" />
                                                <asp:CheckBox ID="chk06" Text="06" runat="server" />
                                                <asp:CheckBox ID="chk07" Text="07" runat="server" />
                                                <asp:CheckBox ID="chk08" Text="08" runat="server" />
                                                <asp:CheckBox ID="chk09" Text="09" runat="server" />
                                                <asp:CheckBox ID="chk10" Text="10" runat="server" />
                                                <asp:CheckBox ID="chk11" Text="11" runat="server" />
                                                <asp:CheckBox ID="chk12" Text="12" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="chk13" Text="13" runat="server" />
                                                <asp:CheckBox ID="chk14" Text="14" runat="server" />
                                                <asp:CheckBox ID="chk15" Text="15" runat="server" />
                                                <asp:CheckBox ID="chk16" Text="16" runat="server" />
                                                <asp:CheckBox ID="chk17" Text="17" runat="server" />
                                                <asp:CheckBox ID="chk18" Text="18" runat="server" />
                                                <asp:CheckBox ID="chk19" Text="19" runat="server" />
                                                <asp:CheckBox ID="chk20" Text="20" runat="server" />
                                                <asp:CheckBox ID="chk21" Text="21" runat="server" />
                                                <asp:CheckBox ID="chk22" Text="22" runat="server" />
                                                <asp:CheckBox ID="chk23" Text="23" runat="server" />
                                                <asp:CheckBox ID="chk24" Text="24" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEstadoMant" runat="server" Text="Estado:" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                            
                                            </td>
                                        </tr>

                                        <tr>                                                    
                                            <td colspan="2">
                                                <asp:Label ID="lblCamposObligatorios" runat="server" Text="(*) Campos Obligatorios" Font-Bold="true"></asp:Label>
                                            </td>                                            
                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                            
                                            </td>
                                        </tr>
                                    </table>

                                    <table>

                                        <tr>

                                            <td>

                                                <table>

                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMostrarAgregarOpcion" runat="server" Text="Registrar" 
                                                                    Width="163px" BorderColor="#993333" 
                                                                    BorderStyle="Groove" BorderWidth="1px" Font-Bold="True" Font-Italic="False" 
                                                                    Font-Names="Arial" Font-Overline="False" Font-Strikeout="False" 
                                                                    Font-Underline="False" ForeColor="#990033" 
                                                                    onclick="btnMostrarAgregarOpcion_Click" />        
                                                        </td>

                                                        <td style="width:50px">
                                                        </td>

                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td><asp:Label ID="Label4" runat="server" Text="(" Font-Bold="True"></asp:Label></td>
                                                                    <td><asp:Label ID="Label3" runat="server" Text="Leyenda: " Font-Bold="True"></asp:Label></td>
                                                                    <td style="width:10px"></td>
                                                                    <td style="border-width: 1px; border-color: #666; border-style: solid; font-weight: bold;">
                                                                        <asp:Label ID="Label1" runat="server" Text="Registro a Eliminar" 
                                                                            BackColor="#FAD4D9"></asp:Label>
                                                                    </td>
                                                                    <td style="width:10px"></td>
                                                                    <td style="border-width: 1px; border-color: #666; border-style: solid; font-weight: bold;">
                                                                        <asp:Label ID="Label2" runat="server" Text="Registro Activo" BackColor="White"></asp:Label>
                                                                    </td>
                                                                    <td><asp:Label ID="Label5" runat="server" Text=")" Font-Bold="True"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                    </tr>

                                                </table>

                                                <table ID="tRolOpcion" runat="server" border="0" style="border-style: solid; border-width: 1px; border-color: #800000; width:720px">
                                                    
                                                    <tr>
                                                        <td colspan="12">
                                                            <Label:Validation ID="ctrlValidacionDetalle" runat="server"/>
                                                        </td>
                                                    </tr>                                                   

                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFormulario" runat="server" Width="200px" 
                                                                AutoPostBack="True" onselectedindexchanged="ddlFormulario_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td align="center"><asp:CheckBox ID="chkGridBuscar" runat="server" Text="Buscar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridImprimir" runat="server" Text="Imprimir" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridCancelarC" runat="server" Text="Limpiar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridCerrar" runat="server" Text="Cerrar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridRegistrar" runat="server" Text="Registrar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridModificar" runat="server" Text="Modificar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridEliminar" runat="server" Text="Anular" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridConfigurar" runat="server" Text="Configurar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridGrabar" runat="server" Text="Grabar" Width="50px" /></td>
                                                        <td align="center"><asp:CheckBox ID="chkGridCancelarM" runat="server" Text="Cancelar" Width="50px" /></td>
                                                        <td>
                                                            <asp:Button ID="btnAgregarFormulario"
                                                                        runat="server" 
                                                                        Text="Agregar" 
                                                                        CssClass="btnGeneral" 
                                                                        onclick="btnAgregarFormulario_Click" />
                                                        </td>
                                                    </tr>

                                                </table>

                                            </td>

                                        </tr>                                        

                                        <tr>
                                            <td>   

                                                <asp:GridView ID="gdvRolOpcion" runat="server"
                                                              CssClass="mGrid"
                                                              SelectedRowStyle-CssClass="slt"
                                                              AutoGenerateColumns="False"
                                                              GridLines="None"
                                                              onrowcommand="gdvRolOpcion_RowCommand" 
                                                              ViewStateMode="Enabled" >

                                                        <Columns>
                                                            
                                                            <asp:BoundField DataField="roop_sRolOpcionId" HeaderText="RolOpcionId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">                                                
                                                                <HeaderStyle Width="200px" />
                                                                <ItemStyle Width="200px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="roop_cEstado" HeaderText="Estado" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">                                                
                                                                <HeaderStyle Width="200px" />
                                                                <ItemStyle Width="200px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="form_sFormularioId" HeaderText="FormularioId" HeaderStyle-CssClass="ColumnaOculta" ItemStyle-CssClass="ColumnaOculta">                                                
                                                                <HeaderStyle Width="200px" />
                                                                <ItemStyle Width="200px" />
                                                            </asp:BoundField>
                                                                                      
                                                            <asp:BoundField DataField="form_vFormulario" HeaderText="Formulario" >                                                
                                                                <HeaderStyle Width="200px" />
                                                                <ItemStyle Width="200px" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField HeaderText="Buscar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridBuscar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Imprimir">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridImprimir" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Limpiar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridCancelarC" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Cerrar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridCerrar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Registrar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridRegistrar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Modificar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridModificar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Eliminar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridEliminar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Configurar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridConfigurar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Grabar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridGrabar" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Cancelar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGridCancelarM" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                
                                                            <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton
                                                                        ID="btnEliminarDetalle"
                                                                        CommandName="Eliminar"
                                                                        tooltip="Anular Opción"
                                                                        CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" 
                                                                        runat="server" 
                                                                        ImageUrl="../Images/img_grid_delete.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>

                                                        </Columns>

                                                        <SelectedRowStyle CssClass="slt" />

                                                </asp:GridView>

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

    <script language="javascript" type="text/javascript">

        $(function () {
            $('#tabs').tabs();           
        });       

        function Validar() {
            var bolValida = true;
            var strRolTipo = $.trim($("#<%= ddlTipoRol.ClientID %>").val());
            var strRolNombre = $.trim($("#<%= txtRolNombreMant.ClientID %>").val());

            var ddlTipoRol = document.getElementById('<%= ddlTipoRol.ClientID %>');
            var txtRolNombreMant = document.getElementById('<%= txtRolNombreMant.ClientID %>');

            if (strRolTipo == "0") {
                ddlTipoRol.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlTipoRol.style.border = "1px solid #888888";
            }

            if (strRolNombre.length == 0) {
                txtRolNombreMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtRolNombreMant.style.border = "1px solid #888888";
            }

            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }

            return bolValida;
        }

        function ValidarLetras(evt) {
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

            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
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