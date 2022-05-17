<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmFormulario.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmFormulario" %>
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
                        <asp:Label ID="lblTituloFormulario" runat="server" Text="Formularios"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>

        <%--Cuerpo--%>
        <table style="width: 90%" align="center">

            <tr>

                <td align ="left">

                    <div id="tabs">

                        <ul>
                            <li><a href="#tab-1"><asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2"><asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>

                        <div id="tab-1">

                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">

                                <ContentTemplate>

                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAplicacionConsulta" runat="server" Text="Aplicación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAplicacionConsulta" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFormularioDescripcion" runat="server" Text="Formulario:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtDescripcionConsulta" runat="server" Width="300px" MaxLength="30" 
                                                             CssClass="txtLetra" onkeypress="return ValidarLetras(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkEstado" Text="Estado" runat="server" Checked="True" />
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

                                    <table>
                                      
                                        <tr>
                                            <td>

                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" ChildrenAsTriggers="true" updatemode="Conditional">
                                        
                                                    <ContentTemplate>
                                        
                                                        <asp:GridView ID="gdvFormulario" runat="server"
                                                            CssClass="mGrid"
                                                            AlternatingRowStyle-CssClass="alt" 
                                                            SelectedRowStyle-CssClass="slt"
                                                            AutoGenerateColumns="False"
                                                            GridLines="None"
                                                            onrowcommand="gdvFormulario_RowCommand">

                                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                                                            <Columns>                                   
                                                                <asp:BoundField DataField="form_vAplicacion" HeaderText="Aplicación" >   
                                                                <ItemStyle Width="80px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="form_vReferencia" HeaderText="Referencia" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="form_vNombre" HeaderText="Formulario" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="form_vRuta" HeaderText="Ruta" > 
                                                                <ItemStyle Width="300px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="form_sOrden" HeaderText="Orden" > 

                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
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
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">

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
                                                <asp:DropDownList ID="ddlAplicacionMant" runat="server" Width="300px" 
                                                    AutoPostBack="True" 
                                                    onselectedindexchanged="ddlAplicacionMant_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReferenciaMant" runat="server" Text="Referencia:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlReferenciaMant" runat="server" Width="391px"></asp:DropDownList>
                                            </td>
                                        </tr>  
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFormularioNombreMant" runat="server" Text="Nombre:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFormularioNombre" runat="server" Width="391px" 
                                                             MaxLength="100" onkeypress="return ValidarLetras(event)"></asp:TextBox>
                                                <asp:Label ID="lblVal_txtFormularioNombre" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFormularioOrden" runat="server" Text="Orden:"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtFormularioOrden" runat="server" Width="61px" MaxLength="1" 
                                                             CssClass="campoNumero" onkeypress="return validatenumber(event)"></asp:TextBox>
                                                <asp:Label ID="lblVal_txtFormularioOrden" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFormularioRutaMant" runat="server" Text="Ruta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFormularioRutaMant" runat="server" Width="391px" 
                                                             MaxLength="100" onkeypress="return validarRutaFormularios(event)"></asp:TextBox>
                                                <asp:Label ID="lblVal_txtFormularioRutaMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>                                                           
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVisible" runat="server" Text="Visible:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkVisible" Text="Visible" runat="server" Checked="true"/>
                                            </td>                                    
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                                            </td>
                                            <td>                                    
                                                <asp:CheckBox ID="chkHabilita" Text="Activo" runat="server" Checked ="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblCamposObligatorios" runat="server" Text="(*) Campos Obligatorios" Font-Bold="true"></asp:Label>
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
            var strFormNombre = $.trim($("#<%= txtFormularioNombre.ClientID %>").val());
            var strFormOrden = $.trim($("#<%= txtFormularioOrden.ClientID %>").val());
            var strFormRuta = $.trim($("#<%= txtFormularioRutaMant.ClientID %>").val());

            var txtFormularioNombre = document.getElementById('<%= txtFormularioNombre.ClientID %>');
            var txtFormularioOrden = document.getElementById('<%= txtFormularioOrden.ClientID %>');
            var txtFormularioRutaMant = document.getElementById('<%= txtFormularioRutaMant.ClientID %>');

            if (strFormNombre.length == 0) {
                txtFormularioNombre.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtFormularioNombre.style.border = "1px solid #888888";
            }
            if (strFormOrden.length == 0) {
                txtFormularioOrden.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtFormularioOrden.style.border = "1px solid #888888";
            }
            if (strFormRuta.length == 0) {
                txtFormularioRutaMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtFormularioRutaMant.style.border = "1px solid #888888";
            }

            /*MENSAJE DE CONFIRMACIÓN*/
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
                bolValida = confirm("¿Está seguro de grabar los cambios?");
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