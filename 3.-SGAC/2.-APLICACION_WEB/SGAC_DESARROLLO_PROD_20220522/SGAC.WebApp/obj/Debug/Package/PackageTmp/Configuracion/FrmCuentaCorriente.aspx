<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmCuentaCorriente.aspx.cs" Inherits="SGAC.WebApp.Configuracion.CuentaBancaria" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ddlOficina" TagPrefix="ddlOficina" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Load);
        $(document).ready(function () {
            Load();
        });

        function Load() {

            $("#<%=txtNroCuenta.ClientID %>").on("blur", validarNroMinimoCaracteresCuenta);

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
                    <h2>
                        <asp:Label ID="lblTituloCtaBancaria" runat="server" Text="Cuenta Bancaria"></asp:Label></h2>
                </td>
            </tr>
        </table>
        <%--Cuerpo--%>
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
                                            <td>
                                                <asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td>
                                                <ddlOficina:ddlOficina ID="ddlOficinaConsularConsulta" runat="server" AutoPostBack="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBanco" runat="server" Text="Cuenta Bancaria:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBanco" runat="server" Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
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
                                    </table>
                                    <%-- Grilla --%>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="updGrillaConsulta" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gdvCuentaBancaria" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                                            SelectedRowStyle-CssClass="slt" AutoGenerateColumn="False" GridLines="None" AutoGenerateColumns="False"
                                                            OnRowCommand="gdvCuentaBancaria_RowCommand">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="cuco_vNumero" HeaderText="Nro. Cuenta">
                                                                    <ItemStyle HorizontalAlign="Left" Width="140px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="para_vBanco" HeaderText="Banco">
                                                                    <ItemStyle Width="180px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="para_vMoneda" HeaderText="Tipo Moneda">
                                                                    <ItemStyle Width="140px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="para_vTipo" HeaderText="Tipo Cuenta">
                                                                    <ItemStyle Width="180px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="cuco_vRepresentante" HeaderText="Representante">
                                                                    <ItemStyle Width="200px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="para_vSituacion" HeaderText="Situación">
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
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
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hTransaccion" runat="server" 
                                                                            Value='<%# Bind("tran_iTransaccionId") %>' />
                                                                    </ItemTemplate>
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
                                    <ToolBar:ToolBarContent ID="ctrlToolBarMantenimiento" runat="server"></ToolBar:ToolBarContent>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblValidacion" runat="server" Text="Falta validar algunos campos."
                                                    CssClass="hideControl" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficinaConsularMant" runat="server" Text="Oficina Consular:"></asp:Label>
                                            </td>
                                            <td>
                                                <ddlOficina:ddlOficina ID="ddlOficinaConsularMant" runat="server" Enabled="True" />
                                                <asp:Label ID="lblVal_ddlOficinaConsularMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNroCuenta" runat="server" Text="Nro. Cuenta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNroCuenta" runat="server" Width="200px" MaxLength="50" style="text-transform: uppercase;"/>
                                                <asp:Label ID="lblVal_txtNroCuenta" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblVal_ddlCuentaCantidadVal" runat="server" Text="Ingrese mínimo 6 dígitos"
                                                    CssClass="lblVal"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBancoMant" runat="server" Text="Banco:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBancoMant" runat="server" Width="350px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblVal_ddlBancoMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoMonedaMant" runat="server" Text="Tipo Moneda:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoMonedaMant" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoCuentaMant" runat="server" Text="Tipo Cuenta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoCuenta" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSituacionMant" runat="server" Text="Situación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSituacion" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombreTitularMant" runat="server" Text="Titular:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombreTitularMant" runat="server" Width="350px" onBlur="conMayusculas(this)"
                                                    onkeypress="return ValidarSujeto(event)" CssClass="txtLetra" MaxLength="50" />
                                                <asp:Label ID="lblVal_txtNombreTitularMant" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombreSucursal" runat="server" Text="Sucursal:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNombreSucursal" runat="server" MaxLength="50" Width="350px" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" onkeypress="return isLetraNumero(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblObservacion" runat="server" Text="Observación:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtObservacionMant" runat="server" Width="350px" onBlur="conMayusculas(this)"
                                                    CssClass="txtLetra" onkeypress="return isLetraNumero(event)" MaxLength="100" />
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
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSaldoInicial" runat="server" CssClass="btnGeneral" 
                                                        Text="Saldo Inicial" onclick="btnSaldoInicial_Click" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblCamposObligatorios" runat="server" Font-Bold="true" 
                                                        Text="(*) Campos Obligatorios"></asp:Label>
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

        function MoveTabIndex(iTab) {
            $('#tabs').tabs("option", "active", iTab);
        }

        function EnableTabIndex(iTab) {
            $('#tabs').tabs("enable", iTab);
        }

        function DisableTabIndex(iTab) {
            $('#tabs').tabs("disable", iTab);
        }

        function validarNroMinimoCaracteresCuenta() {
            var bolValida = true;
            var strNroCuenta = $.trim($("#<%= txtNroCuenta.ClientID %>").val());

            if (strNroCuenta.length == 0) {
                $("#<%=txtNroCuenta.ClientID %>").css("border", "solid Red 1px");
                bolValida = false;
            }
            else {
                $("#<%=txtNroCuenta.ClientID %>").css("border", "solid #888888 1px");
                if (strNroCuenta.length < 6) {
                    $("#<%=lblVal_ddlCuentaCantidadVal.ClientID %>").show();
                    bolValida = false;
                }
                else {
                    $("#<%=lblVal_ddlCuentaCantidadVal.ClientID %>").hide();
                }
            }
            return bolValida;
        }

        function Validar() {
            var bolValida = true;

            var strOCMant = $('#<%=ddlOficinaConsularMant.FindControl("ddlOficinaConsular").ClientID %>').val();
            var ddlOficinaConsular = document.getElementById('<%= ddlOficinaConsularMant.FindControl("ddlOficinaConsular").ClientID %>');

            var strNroCuenta = $.trim($("#<%= txtNroCuenta.ClientID %>").val());
            var strBancoMant = $.trim($("#<%= ddlBancoMant.ClientID %>").val());
            var strTitular = $.trim($("#<%= txtNombreTitularMant.ClientID %>").val());

            var txtNroCuenta = document.getElementById('<%= txtNroCuenta.ClientID %>');
            var ddlBancoMant = document.getElementById('<%= ddlBancoMant.ClientID %>');
            var txtNombreTitularMant = document.getElementById('<%= txtNombreTitularMant.ClientID %>');

            if (strOCMant == "0" || strOCMant == "- SELECCIONAR -") {
                ddlOficinaConsular.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlOficinaConsular.style.border = "1px solid #888888";
            }

            if (strNroCuenta.length == 0) {
                txtNroCuenta.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtNroCuenta.style.border = "1px solid #888888";
                if (strNroCuenta.length < 6) {
                    $("#<%=lblVal_ddlCuentaCantidadVal.ClientID %>").show();
                    bolValida = false;
                }
                else {
                    $("#<%=lblVal_ddlCuentaCantidadVal.ClientID %>").hide();
                }
            }

            if (strNroCuenta == "") {
                
            }

            if (strBancoMant == "0") {
                ddlBancoMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlBancoMant.style.border = "1px solid #888888";
            }

            if (strTitular.length == 0) {
                txtNombreTitularMant.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtNombreTitularMant.style.border = "1px solid #888888";
            }
           
            if (bolValida) {
                $("#<%= lblValidacion.ClientID %>").hide();                
            }
            else {
                $("#<%= lblValidacion.ClientID %>").show();
            }
            return bolValida;
        }

        function ValidarSujeto(evt) {
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
            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
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

            var letras = "áéíóúüñÑäëïöüÁÉÍÓÚÄËÏÖÜ";
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

        function conMayusculas(field) {
            field.value = field.value.toUpperCase()
        }

        function validarTexto(e) {
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

                if (e.keyCode >= 65 && e.keyCode <= 90 || e.keyCode == 32 /*ESPACIO*/) {
                    var keyCode = e.keyCode;
                    var value = $(this).val();
                    value += String.fromCharCode(keyCode);
                    value = parseInt(value, 10);
                }
                else {
                    e.preventDefault();
                    return;
                }

            }
        }

        function validarNumero(e) {
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

                if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105))) {
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

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }

    </script>
</asp:Content>
