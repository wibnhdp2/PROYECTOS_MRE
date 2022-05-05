<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="AdminsitradorHerramientas.aspx.cs" Inherits="SolCARDIP.Paginas.Administrador.AdminsitradorHerramientas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";
        
        function seguridadURLPrevia() {
            if (document.referrer != "") {
                preloader();
            }
            else {
                location.href = '../../mensajes.aspx';
            }
        }

        function mostrarCargando() {
            document.getElementById(master + "divCargando").style.display = "block";
            document.getElementById(master + "divModal2").style.display = "block";
            document.getElementById(master + "divCargando").focus();
        }

        function preloader() {
            document.getElementById(master + "divCargando").style.display = "none";
            document.getElementById(master + "divModal2").style.display = "none";
        }

        function verDiv(id) {
            document.getElementsByName(id).style.display = "block"
        }
        function ocultarDiv(id) {
            document.getElementById(master + id).style.display = "none"
        }

        function focusControl(controlId) {
            var ctrl = document.getElementById(controlId);
            if (ctrl != null) {
                ctrl.focus();
            }
        }
        window.onload = seguridadURLPrevia;
    </script>
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function tabActual(valor) {
            var hd1 = document.getElementById(master + "hdfldTabActual");
            if (hd1 != null) {
                hd1.value = valor;
                cambiarTabs();
            }
        }
        function cambiarTabs() {
            var hd1 = document.getElementById(master + "hdfldTabActual");
            var contenedor = document.getElementById("tdContenedor");
            var tr1 = document.getElementById("trTabs");
            var tabPest = document.getElementById("tabPest" + hd1.value);
            var tab = document.getElementById("tab" + hd1.value);
            var n = 0;
            if (tab != null) {
                if (contenedor != null) {
                    var cantidad = contenedor.children.length
                    for (n = 1; n <= cantidad; n++) {
                        if (contenedor.children[n - 1].id == tab.id) {
                            tab.style.display = "block";
                            //alert(tab.id);
                        }
                        else {
                            contenedor.children[n - 1].style.display = "none";
                            //alert(contenedor.children[n - 1].id);
                        }
                    }
                    n = 0
                    cantidad = tr1.children.length;
                    for (n = 1; n <= cantidad; n++) {
                        if (tr1.children[n - 1].id == tabPest.id) {
                            tabPest.style.backgroundColor = "White";
                            tabPest.className = "Tabs";
                        }
                        else {
                            tr1.children[n - 1].style.backgroundColor = "#E6E6E6";
                            tr1.children[n - 1].className = "Tabs";
                        }
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        var master = "ContentPlaceHolder1_";

        function validarGuardar() {
            var dropDownUsuario = document.getElementById(master + "ddlUsuario");
            var dropDownTipoDoc = document.getElementById(master + "ddlTipoDoc");
            var dropDownRol = document.getElementById(master + "ddlRol");
            var txtNumDoc = document.getElementById(master + "txtNumeroDoc");
            var txtApeP = document.getElementById(master + "txtApePat");
            var txtApeM = document.getElementById(master + "txtApeMat");
            var txtNom = document.getElementById(master + "txtNombres");
            var txtCorreo = document.getElementById(master + "txtCorreo");
            var txtAlias = document.getElementById(master + "txtAlias");
            if (dropDownTipoDoc.value == "0") { alert("DEBE SELECCIONAR UN TIPO DE DOCUMENTO DE IDENTIFICACION"); dropDownTipoDoc.focus(); return false; }
            if (txtNumDoc.value == "") { alert("DEBE INGRESAR UN NUMERO DE DOCUEMNTO DE IDENTIFICACION"); txtNumDoc.focus(); return false; }
            if (txtApeP.value == "") { alert("DEBE INGRESAR EL APELLIDO PATERNO"); txtApeP.focus(); return false; }
            if (txtApeM.value == "") { alert("DEBE INGRESAR EL APELLIDO MATERNO"); txtApeM.focus(); return false; }
            if (txtNom.value == "") { alert("DEBE INGRESAR EL NOMBRE"); txtNom.focus(); return false; }
            if (txtCorreo.value == "") { alert("DEBE INGRESAR EL CORREO ELECTRONICO"); txtCorreo.focus(); return false; }
            if (txtAlias.value == "") { alert("DEBE INGRESAR EL ALIAS (CTA DE RED)"); txtAlias.focus(); return false; }
            if (dropDownRol.value == "0") { alert("DEBE SELECCIONAR UN PERFIL (ROL)"); dropDownRol.focus(); return false; }
            if (confirm("¿DESEA GUARDAR LOS CAMBIOS?")) {
                return true;
            }
            return false;
        }

        function validarMostrarInfo() {
            var dropDownUsuario = document.getElementById(master + "ddlUsuario");
            if (dropDownUsuario != null) {
                if (dropDownUsuario.value != "0") {
                    return true;
                }
                return false;
            }
            return false;
        }

        function validarGrabarBloqueo(valor) {
            if (valor == "0") {
                if (confirm("¿BLOQUEAR USUARIO?")) {
                    return true;
                }
            }
            if (valor == "1") {
                if (confirm("¿DESBLOQUEAR USUARIO?")) {
                    return true;
                }
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updPrincipal" runat="server">
        <ContentTemplate>
            <table class="AnchoTotal">
                <tr>
                    <td>
                        <div>
                            <div id="divModal2" runat="server" style="position:absolute;z-index:1;display:none" class="modalBackgroundLoad"></div>
                            <div id="divCargando" runat="server" style="position:absolute;z-index:2;top:50%;left:45%" onblur="ContentPlaceHolder1_divCargando.focus();">
                                <img alt="gif" src="../../Imagenes/Gifs/ajax-loader(1).gif" style="width:100px;height:95px" />
                            </div>
                            <div id="divModal" runat="server" style="display:none" class="modalBackgroundLoad"></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td style="width:100%">
                                                <table style="width:100%">
                                                    <tr id="trTabs">
                                                        <td id="tabPest0" class="Tabs" style="display:block;" onclick="tabActual(0);">Bloq / Desbloq Usuarios</td>
                                                        <td id="tabPest1" class="Tabs" style="display:block;" onclick="tabActual(1);">Mantenimiento Usuario</td>
                                                        <td id="tabPest2" class="Tabs" style="display:block;" onclick="tabActual(2);">Otros</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td id="tdContenedor">
                                                <div id="tab0" style="display:block;">
                                                    <fieldset class="Fieldset">
                                                        <table style="width:80%;">
                                                            <tr>
                                                                <td style="width:100%;">
                                                                    <table style="width:80%;">
                                                                        <tr>
                                                                            <td class="etiqueta" style="width:10%;">Perfil</td>
                                                                            <td class="etiqueta" style="width:3%;">:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="ddlFiltroRol" CssClass="dropdownlist" 
                                                                                    Width="30%"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="etiqueta">Bloq / Desbloq</td>
                                                                            <td class="etiqueta">:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="ddlFiltroBloqDes" CssClass="dropdownlist" 
                                                                                    Width="30%"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" align="right">
                                                                                <asp:Button runat="server" ID="btnBuscarUsuarios" OnClick="buscarUsuarios" Text="Buscar" Width="150" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <div style="width:98%;">
                                                                                    <table class="AnchoTotal">
                                                                                        <tr>
                                                                                            <td class="CabeceraGrilla" style="width:5%;height:25px">Alias</td>
                                                                                            <td class="CabeceraGrilla" style="width:10%">Apellido Paterno</td>
                                                                                            <td class="CabeceraGrilla" style="width:10%">Apellido Materno</td>
                                                                                            <td class="CabeceraGrilla" style="width:10%">Nombres</td>
                                                                                            <td class="CabeceraGrilla" style="width:12%">Perfil</td>
                                                                                            <td class="CabeceraGrilla" style="width:5%">Bloqueo</td>
                                                                                            <td class="CabeceraGrilla" style="width:5%">Opciones</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div ID="divGridView" runat="server" class="Scroll" style="width:98%;">
                                                                                    <asp:GridView ID="gvUsuarios" runat="server" 
                                                                                        AlternatingRowStyle-BackColor="Control" AutoGenerateColumns="false" 
                                                                                        OnRowDataBound="verTemplate" ShowHeader="False" style="margin-top: 0px" 
                                                                                        Width="100%">
                                                                                        <RowStyle CssClass="FilaDatos" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="Alias" ItemStyle-Height="25px" 
                                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                                                                            <asp:BoundField DataField="Apellidopaterno" ItemStyle-Height="25px" 
                                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                                            <asp:BoundField DataField="Apellidomaterno" ItemStyle-HorizontalAlign="Center" 
                                                                                                ItemStyle-Width="10%" />
                                                                                            <asp:BoundField DataField="Nombres" ItemStyle-HorizontalAlign="Center" 
                                                                                                ItemStyle-Width="10%" />
                                                                                            <asp:BoundField DataField="Rol" ItemStyle-HorizontalAlign="Center" 
                                                                                                ItemStyle-Width="12%" />
                                                                                            <asp:TemplateField HeaderText="Envios" ItemStyle-HorizontalAlign="Center" 
                                                                                                ItemStyle-Width="5%">
                                                                                                <ItemTemplate>
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="ibtBloq0" runat="server" CommandName="blockUser" 
                                                                                                                    ImageUrl="~/Imagenes/Iconos/bloqueo.png" OnClick="bloquearDesbloquear" 
                                                                                                                    OnClientClick="return validarGrabarBloqueo(0);" ToolTip="Bloquear Usuario" />
                                                                                                                <asp:ImageButton ID="ibtBloq1" runat="server" CommandName="unlockUser" 
                                                                                                                    ImageUrl="~/Imagenes/Iconos/desbloqueo.png" OnClick="bloquearDesbloquear" 
                                                                                                                    OnClientClick="return validarGrabarBloqueo(1);" ToolTip="Desbloquear Usuario" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField ItemStyle-Width="5%">
                                                                                                <ItemTemplate>
                                                                                                    <table class="AnchoTotal">
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:ImageButton ID="ibtDevolver" runat="server" CommandName="verInfoUsuario" 
                                                                                                                    ImageUrl="~/Imagenes/Iconos/buscar1.png" OnClick="seleccionarAccion" 
                                                                                                                    ToolTip="Ver Registro" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="BloqueoActiva" ItemStyle-HorizontalAlign="Center" 
                                                                                                ItemStyle-Width="12%" Visible="true" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                                <div ID="tab1" style="display:none;">
                                                    <fieldset class="Fieldset">
                                                        <table style="width:25%;">
                                                            <tr>
                                                                <td class="etiqueta" style="width:25%">
                                                                    Tipo Documento</td>
                                                                <td class="etiqueta" style="width:5%">
                                                                    :</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTipoDoc" runat="server" CssClass="dropdownlist" 
                                                                        Enabled="false" Width="90%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Numero Documento</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNumeroDoc" runat="server" CssClass="textbox" 
                                                                        Enabled="false" Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Apellido Paterno</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtApePat" runat="server" CssClass="textbox" Enabled="false" 
                                                                        Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Apellido Materno</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtApeMat" runat="server" CssClass="textbox" Enabled="false" 
                                                                        Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Nombres</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNombres" runat="server" CssClass="textbox" Enabled="false" 
                                                                        Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Correo</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="textbox" Enabled="false" 
                                                                        Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Alias</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAlias" runat="server" CssClass="textbox" Enabled="false" 
                                                                        Width="90%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta">
                                                                    Perfil Actual</td>
                                                                <td class="etiqueta">
                                                                    :</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="dropdownlist" 
                                                                        Enabled="false" Width="90%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="3">
                                                                    <asp:Button ID="btnNuevo" runat="server" CssClass="ButtonFiltro" 
                                                                        onclick="btnNuevo_Click" Text="Nuevo" />
                                                                    <asp:Button ID="btnEditar" runat="server" CssClass="ButtonFiltro" 
                                                                        Enabled="false" onclick="btnEditar_Click" Text="Editar" />
                                                                    <asp:Button ID="btnGuardar" runat="server" CommandName="guardar" 
                                                                        CssClass="ButtonFiltro" OnClick="seleccionarAccion" 
                                                                        OnClientClick="return validarGuardar();" Text="Guardar" Visible="false" />
                                                                    <asp:Button ID="btnCancelar" runat="server" CssClass="ButtonFiltro" 
                                                                        onclick="btnCancelar_Click" Text="Cancelar" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                                <div ID="tab2" style="display:none;">
                                                    <fieldset class="Fieldset">
                                                        <table style="width:70%;">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Button ID="btnCargarOtros" runat="server" CssClass="ButtonFiltro" 
                                                                        OnClick="cargarOtros" Text="Cargar Valores" Width="150" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" style="width:10%">
                                                                    Conexion</td>
                                                                <td class="etiqueta" style="width:5%">
                                                                    :</td>
                                                                <td>
                                                                    <asp:Label ID="lblConexion" runat="server" CssClass="labelInfo" Width="100%"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="etiqueta" colspan="3">
                                                                    Log de Errores</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtLog" runat="server" Font-Names="Consolas" Font-Size="11" 
                                                                        Height="300px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>                                                           
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
            <asp:HiddenField ID="hdfldTabActual" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
