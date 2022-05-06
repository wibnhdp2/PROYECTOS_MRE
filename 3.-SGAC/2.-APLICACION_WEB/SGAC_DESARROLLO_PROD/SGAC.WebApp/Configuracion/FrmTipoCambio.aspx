<%@ Page Language="C#" uiCulture="es-PE" culture="es-PE" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmTipoCambio.aspx.cs" Inherits="SGAC.WebApp.Configuracion.TipoCambio" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register src="~/Accesorios/SharedControls/ctrlPageBar.ascx" tagname="PageBar" tagprefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />  
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>   
    <script src="../Scripts/site.js" type="text/javascript"></script>   
    
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">


        var txtTCConsularJS = '<%= txtTCConsular.ClientID %>';

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

            $("#<%=txtTCConsular.ClientID %>").on("blur", redondearNumero);

            $("#<%=txtTCBancario.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtTCConsular.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtLucroCambio.ClientID %>").on("keydown", validarDecimal);

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

        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTitutloTipoCambio" runat="server" Text="Tipo de Cambio Consular"></asp:Label></h2>
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

                        <asp:UpdatePanel runat="server" ID="updConsulta" updatemode="Conditional">

                            <ContentTemplate>                                          

                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFecInicio" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecIniConsulta" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfechaFin" runat="server" Text="Fecha Fin:"></asp:Label>
                                        </td>
                                        <td>
                                            <SGAC_Fecha:ctrlDate ID="dtpFecFinConsulta" runat="server" />
                                        </td>
                                    </tr>
                                </table>

                                <%--Opciones--%>
                                <toolbar:toolbarcontent ID="ctrlToolBarConsulta" runat="server"></toolbar:toolbarcontent>

                                <%-- Grilla --%>
                                <table  width="100%" >

                                    <tr>
                                        <td>
                                            <Label:Validation ID="ctrlValidacion" runat="server"/>
                                        </td>                            
                                    </tr>

                                    <tr>

                                        <td>

                                            <asp:UpdatePanel runat="server" ID="updGrillaConsulta" updatemode="Conditional">

                                                <ContentTemplate>

                                                    <asp:GridView ID="gdvTipoCambio" runat="server"
                                                        CssClass="mGrid"
                                                        AlternatingRowStyle-CssClass="alt" 
                                                        SelectedRowStyle-CssClass="slt"
                                                        AutoGenerateColumns="False"
                                                        GridLines="None"
                                                        onrowcommand="gdvTipoCambio_RowCommand" 
                                                        onrowdatabound="gdvTipoCambio_RowDataBound">

                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>

                                                    <Columns>
                                                        <asp:BoundField DataField="tico_dFecha" HeaderText="Fecha" HeaderStyle-HorizontalAlign="Center"
                                                            DataFormatString="{0:MMM-dd-yyyy}" HtmlEncode="False" 
                                                            HtmlEncodeFormatString="False" >
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="tico_FValorConsular" HeaderText="T.C. Consular" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="tico_FValorBancario" HeaderText="Promedio T.C. Bancario" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="tico_FPromedio" HeaderText="Lubro de Cambio" HeaderStyle-CssClass="ColumnaOculta"
                                                            ItemStyle-CssClass="ColumnaOculta" >
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="tico_FPorcentaje" HeaderText="Porcentaje" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="moneda_oficial" HeaderText="Moneda" >
                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
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

                                                    <SelectedRowStyle CssClass="slt" />

                                                </asp:GridView>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>

                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" OnClick="ctrlPaginador_Click" />

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
                                        <td colspan="4">
                                            <%--Mensaje Por defecto--%>
                                            <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos." CssClass="hideControl" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <Label:Validation ID="ctrlValidacionMant" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFecha" runat="server" Text="Fecha:"></asp:Label>
                                        </td>
                                        <td>
                                             <SGAC_Fecha:ctrlDate ID="dtpFecActual" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFechaFinRegistro" runat="server" Text="Fecha Fin:"></asp:Label>
                                        </td>
                                        <td>
                                             <SGAC_Fecha:ctrlDate ID="dtpFecFin" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTCBancario" runat="server" Text="Promedio T.C. Bancario:" 
                                                CssClass="campoNumero"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTCBancario" 
                                                        runat="server" 
                                                        onkeypress="return isNumero(event)" 
                                                        MaxLength="13" 
                                                        CssClass="campoNumero" 
                                                        Width="120px" Enabled="False" />
                                            <%--<asp:Label ID="lblVal_txtTCBancario" runat="server" Text="*" ForeColor="Red"></asp:Label>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoMonedaB" runat="server" Text="Moneda:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonedaTipo" runat="server" Width="200px" 
                                                Enabled="False"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPorcentaje" runat="server" Text="Porcentaje a Aplicar (%):" CssClass="campoNumero"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPorcentaje" runat="server" Width="50px" 
                                                              CssClass="datePickerCenter" AutoPostBack="True" 
                                                              onselectedindexchanged="ddlPorcentaje_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTCConsular" 
                                                       runat="server" 
                                                       Text="Resultado:" 
                                                       CssClass="campoNumero"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLucroCambio" 
                                                         runat="server" 
                                                         CssClass="campoNumero" 
                                                         MaxLength="13" 
                                                         onkeypress="return isNumero(event)" 
                                                         Width="120px" Enabled="False" />
                                            <%--<asp:Label ID="lblVal_txtTCConsular" runat="server" Text="*" ForeColor="Red"></asp:Label>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoMonedaC" runat="server" Text="Moneda:" Visible="False"></asp:Label>
                                        </td>
                                        <td>    
                                            <asp:DropDownList ID="ddlMonedaLocal" runat="server" Width="200px" 
                                                Enabled="False" Visible="False"></asp:DropDownList>
                                        </td>
                                    </tr>
                                       
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLucroCambio" runat="server" Text="T. C. Consular:" 
                                                CssClass="campoNumero"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTCConsular" 
                                                         runat="server" 
                                                         onkeypress="return isNumero(event)"                                                          
                                                         MaxLength="13" 
                                                         CssClass="campoNumero" 
                                                         Width="120px" Enabled="True" TabIndex="0" />
                                                                             
                                        </td>
                                        <td>
                                            <asp:HiddenField runat ="server" ID="hfTope" Value="0" />
                                        </td>

                                    </tr>
                                                                
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEstadoTipoCambio" runat="server" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkActivoMant" runat="server" Text="Activo" Checked="true" />
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

        function isNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode == 8) {
                letra = true;
            }
            
            if (charCode == 44) {
                letra = true;
            }
            
            if (charCode == 46) {
                letra = true;
            }
            
            if (charCode > 47 && charCode < 58) {
                letra = true;
            }
            
            var letras = "áéíóúñÑ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }
            
            return letra;
        }

        function Validar(e) {
            var bolValida = true;
            var strTCConsular = $.trim($("#<%= txtTCConsular.ClientID %>").val());
            var strTCBancario = $.trim($("#<%= txtTCBancario.ClientID %>").val());           

            var txtTCBancario = document.getElementById('<%= txtTCBancario.ClientID %>');
            if (strTCBancario.length == 0 || strTCBancario == '0') {
                txtTCBancario.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtTCBancario.style.border = "1px solid #888888";
            }

            var txtTCConsular = document.getElementById('<%= txtTCConsular.ClientID %>');
            if (strTCConsular.length == 0) {
                txtTCConsular.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtTCConsular.style.border = "1px solid #888888";
            }           

            return bolValida;
        }

        function validarDecimal(e) {
            if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
                return;
            }
            else {
                if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105 ||
                (e.keyCode == 110) ||
                (e.keyCode == 190)))) {
                    // not 0-9, numpad 0-9                    
                    e.preventDefault();
                    return;
                }
                else {
                    var keyCode = e.keyCode;
                    if (keyCode >= 96 && keyCode <= 105) {
                        keyCode -= 48;
                    }

                    var value = $(this).val();

                    if (keyCode == 110 || keyCode == 190) {
                        if (value.indexOf('.') === -1) {
                            value += String.fromCharCode(keyCode);
                            value = parseInt(value, 10)
                            var maxNumber = $(this).data("maxnumber");
                            if (maxNumber) {
                                maxNumber = parseInt(maxNumber);
                                if (value > maxNumber) {
                                    e.preventDefault();
                                }
                            }
                        }
                        else {
                            e.preventDefault();
                            return;
                        }
                    }
                    else {
                        value += String.fromCharCode(keyCode);
                        value = parseInt(value, 10)
                        var maxNumber = $(this).data("maxnumber");
                        if (maxNumber) {
                            maxNumber = parseInt(maxNumber);
                            if (value > maxNumber) {
                                e.preventDefault();
                            }
                        }
                    }
                }
            }
        }

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

        function Redondear(txt) {
            var flotante = parseFloat(txt.value);
            txt.value = flotante.toFixed(3);
        }

        function redondearNumero() {
            var floNumero = $.trim($("#<%= txtTCConsular.ClientID %>").val());
            var floNumeroSinComas = floNumero.replace(",", "");
            var nummer = (Math.round(floNumeroSinComas * 100) / 100);
            $("#<%= txtTCConsular.ClientID %>").val(nummer.toFixed(2));
            
            return nummer
        }
             
    </script>
    
</asp:Content>
