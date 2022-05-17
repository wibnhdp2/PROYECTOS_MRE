<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmFichaRegistral.aspx.cs" Inherits="SGAC.WebApp.Consulta.FrmFichaRegistral" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.1.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/signalr/hubs") %>" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        $(function () {
            $('#tabs').tabs();

            /*NOTIFICACION*/
            toastr.options.closeButton = true;

            var proxy = $.connection.myHub;
            proxy.client.receiveNotification = function (message, user) {
                var userName = $('#<%=lblUserName.ClientID%>').html();
                if (user == userName) {
                    toastr.info(message, user);
                }
            };
            $.connection.hub.start();
        });

        function notificar(msg, user_receive) {
            var proxy = $.connection.myHub;
            proxy.server.sendNotifications(msg, user_receive);
            $.connection.hub.start();
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function Limpiar() {
            $("#<%= txtNroFichaRegistral.ClientID %>").val('');
            $("#<%= ddlEstadoFicha.ClientID %>").val('0');
            $("#<%= txtRGE.ClientID %>").val('');
            $("#<%= txtGuia.ClientID %>").val('');
            $("#<%= txtNroDocParticipante.ClientID %>").val('');
            $("#<%= txtApePatParticipante.ClientID %>").val('');
            $("#<%= txtApeMatParticipante.ClientID %>").val('');
            $("#<%= txtNomParticipante.ClientID %>").val('');
            $("#<%= txtNroHojaSelGuia.ClientID %>").val('');
        };

        function seleccionar_todo() {
            if (document.getElementById('<%=gdvFicha.ClientID%>') != null) {

                var x = document.getElementById('<%=gdvFicha.ClientID%>').querySelectorAll("input");
                var i;
                var cnt = 0;
                for (i = 0; i < x.length; i++) {
                    if (x[i].type == "checkbox") {
                        x[i].checked = document.getElementById('<%=chkSeleccionarTodo.ClientID%>').checked;
                        if (x[i].checked) {
                            cnt++;
                        }
                    }
                }
            }
        } 
    </script>
    <style type="text/css">
    .campoNumero2
    {
	    text-align:left;
	    text-transform:uppercase;
    }
        
    </style>    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="Consulta de Ficha Registral RENIEC"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table width="90%" align="center">
        <tr>
            <td>
                <div id="tabs">
                    <ul>
                        <li><a href="#tab-1">
                            <asp:Label ID="lblConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                    </ul>
                    <div id="tab-1">
                        <asp:UpdatePanel runat="server" ID="updConsulta" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="95%">
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:CheckBox ID="chkSIO" runat="server" Text="Asignación de Envio (SIO)" AutoPostBack="True"
                                                    OnCheckedChanged="chkSIO_CheckedChanged" />
                                            </h4>
                                        </td>
                                    </tr>
                                </table>
                                <table width="95%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                    border-left: 1px solid gray; border-right: 1px solid gray">
                                    <tr>
                                        <td width="150px" align="right">
                                            <asp:Label ID="lblOficinaConsularConsulta" runat="server" Text="Oficina Consular :"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <uc1:ctrlOficinaConsular ID="ctrlOficinaConsular" runat="server" Width="100%" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="150px" align="right">
                                            <asp:Label ID="lblFichaRegistral" runat="server" Text="N° Ficha Registral:"></asp:Label>
                                        </td>
                                        <td width="220px">
                                            <asp:TextBox ID="txtNroFichaRegistral" runat="server" Width="150px" onkeypress="return validarTelefonos(event)"
                                                MaxLength="10" />
                                        </td>
                                        <td width="150px" align="right">
                                            <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoFicha" runat="server" Width="200px" TabIndex="1" Style="cursor: pointer;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaInicio" runat="server" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Final:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaFin" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblRGE" runat="server" Text="R.G.E.:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRGE" runat="server" onkeypress="return validarTelefonos(event)"
                                                Width="150px" TabIndex="4" MaxLength="9" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblGuia" runat="server" Text="Guía de despacho:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuia" runat="server" onkeypress="return validarTelefonos(event)"
                                                Width="150px" TabIndex="5" MaxLength="10" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblNroHojaSelGuia" runat="server" Text="Nro.Hoja de Remisión u Oficio:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNroHojaSelGuia" runat="server" MaxLength="10" onkeypress="return validarTelefonos(event)"
                                                TabIndex="6" Width="150px" />
                                        </td>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div id="ocultarDiv" runat="server">
                                    <table width="95%">
                                        <tr>
                                            <td>
                                                <h5>
                                                    <asp:Label ID="lblTitular" runat="server" Text="Titular del Documento"></asp:Label>
                                                </h5>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="95%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                        border-left: 1px solid gray; border-right: 1px solid gray">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblNroDocumento" runat="server" Text="N° Documento:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNroDocParticipante" runat="server" CssClass="campoNumero2" MaxLength="20"
                                                    TabIndex="7" Width="220px" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblPrimerApellido" runat="server" Text="Primer Apellido:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApePatParticipante" runat="server" CssClass="txtLetra" TabIndex="8"
                                                    Width="400px" MaxLength="100" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblSegundoApellido" runat="server" Text="Segundo Apellido:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApeMatParticipante" runat="server" CssClass="txtLetra" TabIndex="9"
                                                    Width="400px" MaxLength="100" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblNombres" runat="server" Text="Nombres:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNomParticipante" runat="server" CssClass="txtLetra" TabIndex="10"
                                                    Width="400px" MaxLength="100" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnImprimir" runat="server" CssClass="btnPrint" 
                                                TabIndex="11" Text="     Imprimir" Width="110px" 
                                                onclick="btnImprimir_Click" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnBuscar" runat="server" CssClass="btnSearch" OnClick="btnBuscar_Click"
                                                TabIndex="11" Text="     Buscar" Width="110px" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnLimpiar" runat="server" CssClass="btnLimpiar" Text="   Limpiar"
                                                OnClientClick="Limpiar()" OnClick="btnLimpiar_Click" TabIndex="12" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnSeleccionarFichas" runat="server" Text="     Agregar Datos de Envío de SIO"
                                                    CssClass="btnNew" CausesValidation="false" TabIndex="12" Width="230px" OnClientClick="AbrirPopup(); return false;" />
                                            </div>
                                            <br />
                                            <asp:HiddenField ID="HFFichaRegistralId" runat="server" Value="0" />
                                            <asp:CheckBox ID="chkSeleccionarTodo" runat="server" onclick="seleccionar_todo()"
                                                Text="Seleccionar Todo" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gdvFicha" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                    AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvFicha_RowCommand"
                                    OnRowDataBound="gdvFicha_RowDataBound">
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField Visible="True">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSeleccionar" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFichaRegistralId" runat="server" Text='<%# Bind("FIRE_IFICHAREGISTRALID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumeroFicha" runat="server" Text='<%# Bind("VNUMEROFICHA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTarifarioId" runat="server" Text='<%# Bind("STARIFARIOID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActuacionId" runat="server" Text='<%# Bind("IACTUACIONID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActuacionDetalleId" runat="server" Text='<%# Bind("IACTUACIONDETALLEID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPersonaId" runat="server" Text='<%# Bind("IPERSONARECURRENTEID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="N° Ficha Registral" DataField="VNUMEROFICHA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha" DataField="DFECHA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="R.G.E." DataField="ICORRELATIVOACTUACION">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="N° Hoja" DataField="VNUMEROHOJA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="N° Documento" DataField="VDOCUMENTONUMERO">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VNUMEROGUIADESPACHO" HeaderText="N° Guia Despacho" />
                                        <asp:BoundField HeaderText="Titular" DataField="VNOMBRES_APELLIDOS">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tarifa" DataField="VNRO_TARIFA_DESC">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Estado" DataField="ESTA_VDESCRIPCIONCORTA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Oficina Consular" DataField="OFCO_VNOMBRE">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Oficio Envío SIO" DataField="fire_vNumeroOficio">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha Envío SIO" DataField="fire_dFechaEnvio">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Consultar" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnConsultarFicha" runat="server" CommandName="Consultar" ImageUrl="../Images/img_16_search.png" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Editar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditarFicha" runat="server" CommandName="Editar" ImageUrl="../Images/img_16_edit.png" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="slt" />
                                </asp:GridView>
                                <div>
                                    <PageBarContent:PageBar ID="ctrlPaginadorFicha" runat="server" Visible="False" OnClick="ctrlPageBarFichaRegistral_Click" />
                                </div>
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <h5>
                                                <asp:Label ID="lblDetalle" runat="server" Text="Detalle de la Ficha Registral :"></asp:Label>
                                                <asp:Label ID="lblNroFicha" runat="server" Text="Label"></asp:Label>
                                            </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvHistorico" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                AutoGenerateColumns="False" GridLines="None">
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                <Columns>
                                                    <asp:BoundField HeaderText="Fecha" DataField="FIHI_DFECHAESTADO" DataFormatString="{0:dd-MM-yyyy}">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Estado" DataField="ESTA_VDESCRIPCIONCORTA">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nro.Guía" DataField="FIHI_VNUMEROGUIA">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Observación" DataField="FIHI_VOBSERVACION">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="400px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="slt" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <PageBarContent:PageBar ID="ctrlPaginadorFichaHistorica" runat="server" Visible="False"
                                        OnClick="ctrlPageBarFichaHistorica_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div id="modalEnvio" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopup();return false" runat="server" />
                <span>Ingrese los datos de Envío</span>
            </div>
            <div class="modal-cuerpo" style="height:230px;">
                <table width="90%" style="border-bottom: 1px solid gray; border-top: 1px solid gray;
                                    border-left: 1px solid gray; border-right: 1px solid gray; margin-top:10px; margin-left:10px; margin-right:5px;" >
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Nro.Hoja de Remisión u Oficio:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHojaRemi" CssClass="txtLetra" runat="server"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label2" runat="server" Text="Fecha de Envío:"></asp:Label>
                        </td>
                        <td>
                            <SGAC_Fecha:ctrlDate ID="ctrFechaEnvio" runat="server" />
                        </td>
                    </tr>
                </table>
                 
            </div>
            <div class="modal-pie">
               <asp:Button ID="BtnActualizar" CssClass="btnLogin" runat="server" OnClientClick="if(!Validar()) return false;"
                    Text="Actualizar datos de envío" OnClick="btnSeleccionarFichas_Click" />
            </div>
        </div>
    </div>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    <script type="text/javascript">
        function cerrarPopup() {
            document.getElementById('modalEnvio').style.display = 'none';
        }

        function AbrirPopup() {
            document.getElementById('modalEnvio').style.display = 'block';
        }

        function Validar() {
            var bolValida = true;
            var txtMotivo = $('#<%= txtHojaRemi.ClientID %>').val();
            if (txtMotivo == "") {
                document.getElementById('<%=txtHojaRemi.ClientID %>').style.border = "1px solid Red";
                document.getElementById('<%=txtHojaRemi.ClientID %>').focus();
                bolValida = false;
            }
            else { document.getElementById('<%=txtHojaRemi.ClientID %>').style.border = ""; }
            var strFecha = $.trim($('#<%= ctrFechaEnvio.FindControl("TxtFecha").ClientID %>').val());
            var txtFecha = document.getElementById('<%= ctrFechaEnvio.FindControl("TxtFecha").ClientID %>');

            if (strFecha == "") {
                txtFecha.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtFecha.style.border = "1px solid #888888";
            }

            return bolValida;
        }
        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

        function isNombreApellido(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
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
            if (charCode > 159 && charCode < 166) {
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

            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;

        }

        function NoCaracteresEspeciales(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = true;


            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 38) {
                letra = false;
            }
            if (charCode == 60) {
                letra = false;
            }
            if (charCode == 62) {
                letra = false;
            }
            return letra;

        }

        function isLetraNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (charCode == 13) {
                letra = false;
            }
            if (charCode == 32) {
                letra = true;
            }

            if (charCode > 39 && charCode < 60) {
                letra = true;
            }
            if (charCode > 63 && charCode < 91) {
                letra = true;
            }
            if (charCode > 94 && charCode < 123) {
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


            var letras = "áéíóúÁÉÍÓÚñÑäëïöüÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;

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

        function isDecimalKey(ctrl, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var FIND = "."
            var x = ctrl.value
            var y = x.indexOf(FIND)

            if (charCode == 46) {
                if (y != -1 || x.length == 0)
                    return false;
            }
            if (charCode != 46 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>
</asp:Content>
