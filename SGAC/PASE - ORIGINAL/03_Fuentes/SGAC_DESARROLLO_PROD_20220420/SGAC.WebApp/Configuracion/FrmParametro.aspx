<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmParametro.aspx.cs"
    Inherits="SGAC.WebApp.Configuracion.FrmParametro" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarButton.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

        function ConvertirFechaAFormatoUs(textoFecha) {

            textoFecha = textoFecha.replace('ene', 'jan');
            textoFecha = textoFecha.replace('abr', 'apr');
            textoFecha = textoFecha.replace('ago', 'aug');
            textoFecha = textoFecha.replace('dic', 'dec');

            return textoFecha;
        }

        function ValidarFechaInicioFin(sender, args) {
            var fechaInicioTexto = document.getElementById('<% =txtFechaInicio.ClientID %>');
            var fechaFinTexto = document.getElementById('<% =txtFechaFin.ClientID %>');

            var fechaIincio = Date.parse(ConvertirFechaAFormatoUs(fechaInicioTexto.value));
            var fechaFin = Date.parse(ConvertirFechaAFormatoUs(fechaFinTexto.value));

            if (fechaIincio > fechaFin) {                

                var senderId = sender._id.toLowerCase(); 

                if (senderId.indexOf("inicio")>-1) {
                    fechaInicioTexto.value = ''
                }
                else {
                    fechaFinTexto.value = '';
                }

                alert("El campo 'Fecha Inicio' no puede ser mayor al campo la 'Fecha Fin'.");

            }
        }
        function busqueda() {
            var input = document.getElementById('<% =txtFindGrupo.ClientID %>').value.toUpperCase();
            var output = document.getElementById('<% =ddlGrupo.ClientID %>').options;

            for (var i = 0; i < output.length; i++) {

                if (output[i].text.toUpperCase().indexOf(input) > -1) {
                    output[i].selected = true;
                }
                if (input == '') {
                    output[0].selected = true;
                }
            }
        }
        function ActivaFindGrupo(controlID) {
            var input = document.getElementById(controlID);

            if (input.style.visibility == 'visible') {
                input.style.visibility = 'hidden';
            }
            else {
                input.style.visibility = 'visible';
                input.focus();
            }
        }
    </script>
   
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloParametro" runat="server" Text="Parámetros del Sistema"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>
        <%-- Consulta --%>
        <table width="90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updArbol" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr style="vertical-align: top">
                                            <td>
                                            <asp:UpdatePanel ID="updddlGrupo" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblGrupo" runat="server" Text="Grupo:"></asp:Label>
                                                        </td>
                                                        <td>                                                            
                                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="300px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <img alt="Busca Grupo" src="../Images/img_16_search.png" style="cursor:pointer" onclick="ActivaFindGrupo('<%= txtFindGrupo.ClientID %>')" title="Buscar Grupo"/>
                                                               
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFindGrupo" runat="server"  
                                                             MaxLength="100"                                                              
                                                             onKeyDown="if(event.keyCode==13) event.keyCode=9;" 
                                                             onkeypress="return isNombre(event)" 
                                                             onkeyup="busqueda()"                                                               
                                                             Width="300px"></asp:TextBox>   
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkEstado" Text="Estado" runat="server" Checked="True" />
                                                        </td>                                                        
                                                    </tr>
                                                </table>
                                                 </ContentTemplate>
                                            </asp:UpdatePanel>
                                                <%--Opciones--%>
                                                <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                                <%-- Grilla --%>
                                                <table>
                                                    <tr>
                                                        <td style="width: 870px;">
                                                            <Label:Validation ID="ctrlValidation" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gdvParametro" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                                        SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None"
                                                                        OnRowCommand="gdvParametro_RowCommand">
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="para_sParametroId" HeaderText="ID">
                                                                                <HeaderStyle Width="30px" />
                                                                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="para_vDescripcion" HeaderText="Parámetro">
                                                                                <HeaderStyle Width="350px" />
                                                                                <ItemStyle Width="350px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="para_vValor" HeaderText="Valor">
                                                                                <HeaderStyle Width="350px" />
                                                                                <ItemStyle Width="350px" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="30px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="30px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="slt" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
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
                                                <asp:HiddenField ID="hidOper" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblGrupoMant" runat="server" Text="Grupo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGrupoMant" runat="server" Width="300px" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlGrupoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDescripcionMant" runat="server" Text="Descripción:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescripcionMant" runat="server" Width="300px" MaxLength="100"
                                                    onkeypress="return isNombre(event)" TabIndex="2" />
                                                <asp:Label ID="lblVal_txtDescripcionMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblValorMant" runat="server" Text="Valor:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtValorMant" runat="server" Width="300px" MaxLength="50" 
                                                  CssClass="txtLetra" onkeypress="return isNumeroLetra(event)" TabIndex="3" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReferenciaMant" runat="server" Text="Referencia:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferenciaMant" runat="server" Width="300px" MaxLength="100"
                                                    CssClass="txtLetra" onkeypress="return isNumeroLetra(event)" 
                                                    TabIndex="4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOrdenMant" runat="server" Text="Orden:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrdenMant" runat="server" Width="100px" 
                                                    onkeypress="return isNumberKey(event)" TabIndex="5" MaxLength="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaInicio" runat="server" Width="100px" Enabled="False" 
                                                    TabIndex="6"  />
                                                <cc1:CalendarExtender ID="calExtFechaInicio" runat="server" Format="MMM-dd-yyyy"
                                                    PopupButtonID="imgCal1" TargetControlID="txtFechaInicio" OnClientDateSelectionChanged="ValidarFechaInicioFin"/>
                                                <asp:ImageButton ID="imgCal1" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                                    ImageAlign="AbsMiddle" TabIndex="6" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaFin" runat="server" Width="100px" Enabled="False" 
                                                    TabIndex="7" />
                                                <cc1:CalendarExtender ID="calExtFechaFin" runat="server" Format="MMM-dd-yyyy" PopupButtonID="imgCal2"
                                                    TargetControlID="txtFechaFin" OnClientDateSelectionChanged="ValidarFechaInicioFin"/>
                                                <asp:ImageButton ID="imgCal2" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                                    ImageAlign="AbsMiddle" TabIndex="7" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <asp:CheckBox ID="chkExcepcion" runat="server" Text="Excepción" Checked="False" 
                                                    TabIndex="9" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkVisibleMant" runat="server" Text="Visible" 
                                                    Checked="True" TabIndex="8" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkEstadoMant" runat="server" Text="Activo" Checked="True" 
                                                    TabIndex="9" />
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

        function ValidarGrabar() {

            var bolValida = true;

            var strGrupoMant = $.trim($("#<%= ddlGrupoMant.ClientID %>").val());
            var strDescripcionMant = $.trim($("#<%= txtDescripcionMant.ClientID %>").val());            
            var strOper = $.trim($("#<%= hidOper.ClientID %>").val());

            if (strOper == 'INSERTAR') {
                if (strGrupoMant == '0') {
                    $("#<%=ddlGrupoMant.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddlGrupoMant.ClientID %>").css("border", "solid #888888 1px");
                }
            }
            else {
                if (strGrupoMant.length == 0) {
                    $("#<%=ddlGrupoMant.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddlGrupoMant.ClientID %>").css("border", "solid #888888 1px");
                }
            }          

            if (strDescripcionMant.length == 0) {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtDescripcionMant.ClientID %>").css("border", "solid #888888 1px");
            }
           
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }

        function isNumeroLetra(evt) {
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
            if (charCode > 47 && charCode < 58) {
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

            var letras = "áéíóúñÑ()[]-_$:.\/%*,";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
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
            if (charCode > 47 && charCode < 58) {
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

            var letras = "áéíóúñÑ()[]-+,";
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

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }       
    </script>
</asp:Content>
