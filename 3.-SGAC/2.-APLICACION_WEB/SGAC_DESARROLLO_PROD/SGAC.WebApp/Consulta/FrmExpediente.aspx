<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmExpediente.aspx.cs"
    Inherits="SGAC.WebApp.Consulta.FrmJudicial" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlFecha.ascx" TagName="ctrlFecha" TagPrefix="uc3" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/toastr.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.1.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/signalr/hubs") %>" type="text/javascript"></script>
    
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

        function clearCalendar() {
            document.getElementById("<%=txtFechaInicio.ClientID %>").value = "";
            document.getElementById("<%=txtFechaFin.ClientID %>").value = "";
        };

    </script>
      
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HFGUID" runat="server" />
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloExpedientes" runat="server" Text="Consulta - Exhortos Consulares"></asp:Label></h2>
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
                                <table width="100%">
                                    <tr>
                                        <td width="150px">
                                            <asp:Label ID="lblNroExpediente" runat="server" Text="Nro. Expediente:"></asp:Label>
                                        </td>
                                        <td width="220px">
                                            <asp:TextBox ID="txtNroExpediente" runat="server" Width="150px" onkeypress="return NoCaracteresEspeciales(event)"
                                                onBlur="conMayusculas(this)" />
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblExpedienteEstado" runat="server" Text="Estado Expediente:"></asp:Label>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlExpedienteEstado" runat="server" Width="170px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width:95px;">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Label3" runat="server" Text="Estado Actas:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cbo_estado_actas" runat="server" Width="100px">
                                                            <asp:ListItem Value="0">- TODOS -</asp:ListItem>
                                                            <asp:ListItem Value="71">OBSERVADO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNroHojaRemi" runat="server" Text="Nro. Hoja Remisión:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNroHojaRemision" runat="server" Width="150px" onkeypress="return NoCaracteresEspeciales(event)"
                                                onBlur="conMayusculas(this)" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                        </td>
                                        <td>
                                            <uc1:ctrlOficinaConsular ID="ddlOficinaConsular" runat="server" Width="370px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Tipo Persona:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoPersona" runat="server" Width="153px">
                                            </asp:DropDownList> 
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Demandado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDemandado" runat="server" Width="368px" CssClass="txtLetra" onkeypress="return isNombreApellido(event)"
                                                onBlur="conMayusculas(this)" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Registro Inicio:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaInicio" runat="server" />
                                            <%--<asp:TextBox ID="txtFechaInicio" runat="server" Width="90px" Enabled="False" />
                                            <uc3:CalendarExtender ID="cFechaInicio" runat="server" TargetControlID="txtFechaInicio"
                                                PopupButtonID="imgCal3" Format="MMM-dd-yyyy" />
                                            <asp:ImageButton ID="imgCal3" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                                ImageAlign="AbsMiddle" />--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Registro Fin:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="txtFechaFin" runat="server" />
                                            <%--<asp:TextBox ID="txtFechaFin" runat="server" Width="90px" Enabled="False" />
                                            <uc3:CalendarExtender ID="cFechaFin" runat="server" TargetControlID="txtFechaFin"
                                                PopupButtonID="imgCal1" Format="MMM-dd-yyyy" />
                                            <asp:ImageButton ID="imgCal1" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                                ImageAlign="AbsMiddle" />&nbsp;&nbsp; <a onclick="clearCalendar()" href="#">Limpiar
                                                    fechas</a>--%>
                                        </td>
                                    </tr>
                                </table>
                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                <asp:UpdatePanel runat="server" ID="updGrillaExpediente" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <Label:Validation ID="ctrlValidacion" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblListadoExpediente" runat="server" Style="font-weight: 700; color: #990033"
                                                        Text="Listado de Expedientes" Visible="False" Font-Size="10pt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Leyenda" runat="server" 
                                                        Text="El estado del expediente OBSERVADO, indica que tiene un demandado con acta observada" 
                                                        Font-Bold="True" Font-Size="Small" ForeColor="Maroon"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gdvExpediente" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gdvExpediente_RowCommand"
                                            
                                            
                                            DataKeyNames="iActoJudicialId,IdParticipante,sEstadoId,iActoJudicialParticipanteId, sActaJudicialEstadoId, iActuacionId,iCodPer" 
                                            onrowdatabound="gdvExpediente_RowDataBound">
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            <Columns>                                          
                                                <asp:BoundField HeaderText="Nro." DataField="Id">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Fecha Registro" DataField="dFechaRegistro" DataFormatString="{0:MMM-dd-yyyy HH:mm}">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Consulado" DataField="vSiglas">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Nro. Expediente" DataField="vNumeroExpediente">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Estado Expediente" DataField="vEstadoExped">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Fecha Observación" DataField="dFechaObservacion" DataFormatString="{0:MMM-dd-yyyy}">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Demandado" DataField="vnombreParticipante">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tarifa" DataField="starifa">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tipo Pago" DataField="sPagoTipo">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Ver Notificado" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnVerNotificado" CommandName="VerNotificado" ToolTip="Ver Listado de Notificados"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_notificados.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ver Expediente" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnVer" CommandName="Ver" ToolTip="Ver Expediente Judicial"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar Expediente Judicial"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_edit.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Acta" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Actas" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="btnActas" CommandName="Actas" ToolTip="Actas"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_grid_delete.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>

                                                 <asp:BoundField HeaderText="vExisteActa" DataField="vExisteActa" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Historico" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnHistotico" CommandName="Historico" ToolTip="Historico de Expediente"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" ImageUrl="../Images/img_16_preview.png" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="sActaJudicialEstadoId" DataField="sActaJudicialEstadoId" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="iCodPer" DataField="iCodPer" HeaderStyle-CssClass="ColumnaOculta"
                                                    ItemStyle-CssClass="ColumnaOculta">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>
                                        <div>
                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click"
                                                Visible="False" />
                                        </div>
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel runat="server" ID="updGrillaNotifica" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                             <tr>
                                                <td>
                                                    <Label:Validation ID="CtrValidaNotifica" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDetalleNotifica" runat="server" Style="font-weight: 700; color: #990033"
                                                        Text="Detalle de Notificados para Expediente: " Visible="False"></asp:Label>
                                                    <asp:Label ID="lblDatosExpediente" runat="server" Style="font-weight: 700; color: #990033"
                                                        Text="000001" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gdvNotificados" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False" GridLines="None">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Nro." DataField="Id">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Fecha Notificación" 
                                                                DataField="ajno_dFechaHoraNotificacion" DataFormatString="{0:MMM-dd-yyyy}">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Nro. Expediente" DataField="acju_vNumeroExpediente">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Estado Notificación" 
                                                                DataField="esta_vDescripcionCorta">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Notificado" DataField="acju_vApeNombre">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Dirección" Visible="False">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Consulado" DataField="ofco_vNombre">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel runat="server" ID="UpdExpedienteHistorico" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                             <tr>
                                                <td>
                                                    <Label:Validation ID="lblExpedienteHistorico" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDetalleExpedienteHistorico" runat="server" Style="font-weight: 700; color: #990033"
                                                        Text="Histórico de Expediente: " Visible="False"></asp:Label>
                                                    <asp:Label ID="lblDatosExpedienteHistorico" runat="server" Style="font-weight: 700; color: #990033"
                                                        Text="000001" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvdExpedienteHistorico" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False" GridLines="None">
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField HeaderText="achi_iActoJudicialId" DataField="achi_iActoJudicialId" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Nro. Expediente" DataField="acju_vNumeroExpediente">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                           
                                                            <asp:BoundField HeaderText="achi_sEstadoId" DataField="achi_sEstadoId" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>
                                                           
                                                            <asp:BoundField HeaderText="Estado de Expediente" DataField="vEstadoExpediente">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                            </asp:BoundField>

                                                            <asp:BoundField HeaderText="Fecha de Registro" 
                                                                DataField="achi_dFechaRegistro" DataFormatString="{0:MMM-dd-yyyy HH:mm}">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            
                                                            <asp:BoundField HeaderText="achi_sUsuarioCreacion" DataField="achi_sUsuarioCreacion" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta">
                                                                <HeaderStyle CssClass="ColumnaOculta" />
                                                                <ItemStyle CssClass="ColumnaOculta" />
                                                            </asp:BoundField>

                                                         
                                                            <asp:BoundField HeaderText="Usuario" DataField="vUsuario">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                            </asp:BoundField>
                                                        
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                                        
                </div>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserName" runat="server" Text="" CssClass="hideText"></asp:Label>
    <script type="text/javascript">

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
