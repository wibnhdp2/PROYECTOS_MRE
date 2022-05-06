<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmTablasMaestras.aspx.cs" Inherits="SGAC.WebApp.Configuracion.FrmTablasMaestras" %>
    
<%@ Register Src="../Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar"
    TagName="ToolBarContent" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label"
    TagName="Validation" %>
<%@ Register Src="../Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar"
    TagPrefix="PageBarContent" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server"> 
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style1
        {
            width: 91px;
        }
        .style2
        {
            width: 90px;
        }
        .style4
        {
            width: 80px;
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

        function LimpiarCampos() {
           
            var txtCodigo = document.getElementById('<% =txtCodigo.ClientID %>');
            var txtNombre = document.getElementById('<% =txtNombre.ClientID %>');
            var txtDescripcion = document.getElementById('<% =txtDescripcion.ClientID %>');
            var txtSimbolo = document.getElementById('<% =txtSimbolo.ClientID %>');

            var ddl_Grupo = document.getElementById('<% =ddl_Grupo.ClientID %>');


            if(txtCodigo!=null)
                txtCodigo.value = "";

            if (txtNombre != null)
                txtNombre.value = "";

            if (txtDescripcion != null)
                txtDescripcion.value = "";

            if (txtSimbolo != null)
                txtSimbolo.value = "";

            if (ddl_Grupo != null)
                ddl_Grupo.selectedIndex = 0;

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
    </script>
   
    <div>
        <%-- Consulta --%>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2>
                        <asp:Label ID="lblTituloManTabMaestras" runat="server" Text="Tablas Maestras"></asp:Label>
                    </h2>
                </td>
            </tr>
        </table>
        <%--Opciones--%>
        <table style="width: 90%" align="center">
            <tr>
                <td>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%-- Consulta --%>
                                    <table>
                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblTabla" runat="server" Text="Tabla:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updComboFiltro" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddl_TablaBusqueda" runat="server" Width="400px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddl_TablaBusqueda_SelectedIndexChanged"/>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="BusquedaPlantilla" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td class="style2">
                                                            <asp:Label ID="Label2" runat="server" Text="Plantilla: "></asp:Label>
                                                        </td>
                                                        <td>
                                                        <asp:DropDownList ID="ddl_ConsultaPlantilla" runat="server" Width="400px" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddl_ConsultaPlantilla_SelectedIndexChanged"/>                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="style2">
                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Checked="True" />
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
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:GridView ID="Grd_Tablas" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                        SelectedRowStyle-CssClass="slt" AutoGenerateColumn="False" OnRowCommand="Grd_Tablas_RowCommand"
                                                        AutoGenerateColumns="False">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ITabla" HeaderText="Tabla">
                                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vGrupo" HeaderText="Grupo" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta" />
                                                            <asp:BoundField DataField="IRgistroId" HeaderText="RegistroId" >
                                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vDescripcion" HeaderText="Descripción">
                                                                <HeaderStyle Width="350px" />
                                                                <ItemStyle Width="350px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vValor" HeaderText="Valor">
                                                                <HeaderStyle Width="350px" />
                                                                <ItemStyle Width="350px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="vSimbolo" HeaderText="Simbolo" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta" />
                                                            <asp:BoundField DataField="cCodigo" HeaderText="Codigo" HeaderStyle-CssClass="ColumnaOculta"
                                                                ItemStyle-CssClass="ColumnaOculta" />
                                                            <asp:BoundField DataField="cEstado" HeaderText="Estado">
                                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="slt" />
                                                    </asp:GridView>
                                                </div>
                                                <div>
                                                    <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />
                                                </div>
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
                                    <table width="100%">
                                        <tr>
                                            <td id="tdMsje" colspan="2">
                                                <asp:Label ID="lblValidacion"
                                                           runat="server"
                                                           Text="Debe ingresar los campos requeridos (*)"
                                                           CssClass="hideControl"
                                                           ForeColor="Red"
                                                           Font-Size="14px">
                                                </asp:Label>
                                                <asp:HiddenField ID="hidTabla" runat="server" />
                                                <asp:HiddenField ID="HF_Tabla" runat="server" />     
                                            </td>
                                        </tr>
                                        <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td class="style4">
                                                        <asp:Label ID="lblTablaRegistro" runat="server" Text="Tabla :"></asp:Label>                                                
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_TablaRegistro" runat="server" Width="400px" AutoPostBack="True"
                                                             OnSelectedIndexChanged="ddl_TablaRegistro_SelectedIndexChanged"  onchange="LimpiarCampos()" TabIndex="1" />
                                                        <asp:Label ID="lblVal_ddl_Tabla" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        </tr>

                                        <tr>
                                            <td runat="server" id="CampoPlantilla" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblPlantilla" runat="server" Text="Plantilla: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_Plantilla" runat="server" Width="400px" TabIndex="1" />
                                                            <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblNro" runat="server" Text="Orden:"></asp:Label>
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="txtNro" runat="server" Width="100px" 
                                                                onkeypress="return isNumberKey(event)" TabIndex="2" MaxLength="2" />
                                                           <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblEtiqueta" runat="server" Text="Etiqueta:"></asp:Label>                                                           
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEtiqueta" runat="server" Width="700px" 
                                                                MaxLength="200" 
                                                                onkeypress="return isNumeroLetra(event)" TabIndex="5" />
                                                            <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td runat="server" id="CampoCodigo" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblCodigo" runat="server" Text="Codigo :"></asp:Label>                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="txtLetra" MaxLength="10" 
                                                                onkeypress="return isNumeroLetra(event)" TabIndex="2" Enabled="false"/>
                                                            <asp:Label ID="lblVal_txtCodigo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="CampoGrupo" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblGrupo" runat="server" Text="Grupo :"></asp:Label>                                                           
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_Grupo" runat="server" Width="400px" TabIndex="3" />
                                                            <asp:Label ID="lblVal_ddl_Grupo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="CampoNombre" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblNombre" runat="server" Text="Nombre :"></asp:Label>                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNombre" runat="server" Width="400px" CssClass="txtLetra" onkeypress="return isNumeroLetra(event)"
                                                                MaxLength="50" TabIndex="4" />
                                                            <asp:Label ID="lblVal_txtNombre" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="CampoDescripcion" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción :"></asp:Label>                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescripcion" runat="server" Width="400px" Height="57px" TextMode="MultiLine"
                                                                CssClass="txtLetra" MaxLength="100" 
                                                                onkeypress="return isNumeroLetra(event)" TabIndex="5" />
                                                            <asp:Label ID="lblVal_txtDescripcion" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                  
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="CampoSimbolo" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblSimbolo" runat="server" Text="Simbolo :"></asp:Label>                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSimbolo" runat="server" CssClass="txtLetra" MaxLength="10" 
                                                                onkeypress="return isNumeroLetra(event)" TabIndex="6" />
                                                            <asp:Label ID="lblVal_txtSimbolo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            <asp:Label ID="lblEstadoMant" runat="server" Text="Estado:" ></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
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

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function Validar() {

            var bolValida = true;

            var strCbmTabla = $.trim($("#<%= ddl_TablaRegistro.ClientID %>").val());
            var strCodigo = $.trim($("#<%= txtCodigo.ClientID %>").val());
            var strGrupo = $.trim($("#<%= ddl_Grupo.ClientID %>").val());
            var strNombre = $.trim($("#<%= txtNombre.ClientID %>").val());
            var strDescripcion = $.trim($("#<%= txtDescripcion.ClientID %>").val());
            var strSimbolo = $.trim($("#<%= txtSimbolo.ClientID %>").val());
            var strHidTabla = $.trim($("#<%= hidTabla.ClientID %>").val());
            var strNombreTabla = $.trim($("#<%= HF_Tabla.ClientID %>").val());
            var strPlantilla = $.trim($("#<%= ddl_Plantilla.ClientID %>").val());
            var strOrden = $.trim($("#<%= txtNro.ClientID %>").val());
            var strEtiqueta = $.trim($("#<%= txtEtiqueta.ClientID %>").val());


            if (strCbmTabla == '0') {
                $("#<%=ddl_TablaRegistro.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=ddl_TablaRegistro.ClientID %>").css("border", "solid #888888 1px");
            }

            if (strHidTabla == 356 || strHidTabla == 357 || strHidTabla == 358 || strHidTabla == 359 || strHidTabla == 361 || strHidTabla == 363 || strHidTabla == 366 || strHidTabla == 367 || strHidTabla == 370) {
                if (strNombre.length == 0) {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strDescripcion.length == 0) {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strHidTabla == 365 || strHidTabla == 368) {
//                if (strHidTabla == 368) {
//                    if (strCodigo.length == 0) {
//                        $("#<%=txtCodigo.ClientID %>").css("border", "solid Red 1px");
//                        bolValida = false;
//                    }
//                    else {
//                        $("#<%=txtCodigo.ClientID %>").css("border", "solid #888888 1px");
//                    }
//                }

                if (strNombre.length == 0) {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strDescripcion.length == 0) {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid #888888 1px");
                }                
            }

            if (strHidTabla == 362 || strHidTabla == 371) {
                if (strGrupo == '00') {
                    $("#<%=ddl_Grupo.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddl_Grupo.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strNombre.length == 0) {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strDescripcion.length == 0) {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strHidTabla == 364) {
                if (strNombre.length == 0) {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strDescripcion.length == 0) {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid #888888 1px");
                }

                if (strSimbolo.length == 0) {
                    $("#<%=txtSimbolo.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtSimbolo.ClientID %>").css("border", "solid #888888 1px");
                } 
            }

            if (strHidTabla == 369) {
                if (strDescripcion.length == 0) {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtDescripcion.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strHidTabla == 360) {
                if (strNombre.length == 0) {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNombre.ClientID %>").css("border", "solid #888888 1px");
                }
            }

            if (strNombreTabla == "MA_ETIQUETA") {
                if (strPlantilla == '0') {
                    $("#<%=ddl_Plantilla.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=ddl_Plantilla.ClientID %>").css("border", "solid #888888 1px");
                }
                if (strOrden.length == 0) {
                    $("#<%=txtNro.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtNro.ClientID %>").css("border", "solid #888888 1px");
                }
                if (strEtiqueta.length == 0) {
                    $("#<%=txtEtiqueta.ClientID %>").css("border", "solid Red 1px");
                    bolValida = false;
                }
                else {
                    $("#<%=txtEtiqueta.ClientID %>").css("border", "solid #888888 1px");
                }
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

            var letras = "áéíóúÁÉÍÓÚñÑ()[]-_$";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }

        // Llamada a popup de mensaje de informacion
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
             
    </script>
</asp:Content>
