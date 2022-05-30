<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmCajaChica.aspx.cs" Inherits="SGAC.WebApp.Contabilidad.FrmCajaChica" %>

<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagName="ToolBar" TagPrefix="ToolBar" %> 
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagPrefix="Label" TagName="Validation" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlDate.ascx" TagName="ctrlDate" TagPrefix="SGAC_Fecha" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pageLoad() {
            $("#<%=txtDepositado.ClientID %>").on("keydown", validarDecimal);
            $("#<%=txtDevuelto.ClientID %>").on("keydown", validarDecimal);
        };

        $(function () {
            $("#tabs").tabs();
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    
    <table class="mTblTituloM" align="center">
        <tr>
            <td>
                <h2><asp:Label ID="lblTituloCajaChica" runat="server" Text="Caja Chica"></asp:Label></h2>
            </td>
        </tr>
    </table>
    <table style="width: 90%" align="center">
    <tr><td>

    <div id="tabs">
        <ul>
            <li><a href="#tab-1"><asp:Label ID="lblTabConsulta" runat="server" Text="Registro"></asp:Label></a></li>
        </ul>
        <div id="tab-1">

            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>  

            <toolbar:toolbar ID="ctrlToolBarMantenimiento" runat="server" />
            <asp:HiddenField runat="server" ID="hdn_sOficinaConsularId" Value="0" />
            <asp:HiddenField runat="server" ID="hdn_sUsuarioId" Value="0" />
            <br />

            <b>BÚSQUEDA</b>

        <table class="mTblPrincipal">
            <tr>
                <td><asp:Label ID="lblBanco" runat="server" Text="Banco:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlBanco" runat="server" Width="220px">
                    </asp:DropDownList>
                </td>
                <td style="width:20px"></td>
                <td><asp:Label ID="lblNumOperacion" runat="server" Text="Nro. Operación Bancaria:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtNumOperacion" runat="server" CssClass="txtLetra" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width:20px"></td>
                <td><asp:Button runat="server" ID="btnBuscar" Text="   Buscar" CssClass="btnSearch"
                        onclick="btnBuscar_Click" OnClientClick="return ValidarBusqueda();" /></td>
            </tr>
        </table>
            <br />

            <b>REGISTRO</b>
        <table>
            <tr>
                <td>Saldo: </td>
                <td><asp:TextBox ID="txtSaldoCajaChica" Enabled="false" runat="server" ></asp:TextBox></td>
                <td><asp:Label ID="lblSaldoMoneda" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Fecha: </td>
                <td>
                    <SGAC_Fecha:ctrlDate ID="dtpFecha" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Monto Depositado: </td>
                <td><asp:TextBox ID="txtDepositado" runat="server" MaxLength="10"></asp:TextBox></td>
                <td><asp:Label ID="lblMonedaLocal" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Monto Devuelto: </td>
                <td><asp:TextBox ID="txtDevuelto" runat="server" MaxLength="10"></asp:TextBox></td>
                <td><asp:Label ID="lblDevueltoMoneda" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Nro. Comprobante </td>
                <td><asp:TextBox ID="txtComprobante" runat="server" MaxLength="20" CssClass="txtLetra"></asp:TextBox></td>
            </tr>
        </table>
            <br />
        <table>
            <tr>
                <td>
                    <b>Detalle Movimientos:
                    <asp:Label runat="server" ID="lblTotal"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:GridView ID="gdvMovimientos" runat="server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                    SelectedRowStyle-CssClass="slt" AutoGenerateColumns="False" GridLines="None">
                    <AlternatingRowStyle CssClass="alt" />
                    <SelectedRowStyle CssClass="slt"></SelectedRowStyle>
                    <Columns>
                        <asp:BoundField DataField="MOCA_VTIPOMOVIMIENTO" HeaderText="Tipo">
                            <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="moca_dFecha" HeaderText="Fecha" DataFormatString="{0:MMM-dd-yyyy}">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="acde_ICorrelativoActuacion" HeaderText="RGE">
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="moca_fMontoOperacion" HeaderText="Monto Depositado" DataFormatString="{0:0.00}">
                            <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Right" Width="120px" />
                        </asp:BoundField>                        
                        <asp:BoundField DataField="moca_fMonto" HeaderText="Costo" DataFormatString="{0:0.00}">
                            <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Right" Width="120px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td width="10%">
                                        <asp:Image ID="imgWarning" runat="server" 
                                            ImageUrl="../Images/img_16_warning.png" />
                                    </td>
                                    <td width="5%">
                                    </td>
                                    <td width="85%">
                                        <asp:Label ID="lblSinDatos" runat="server" Text="No se encontraron Datos..."></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
                </td>            
            </tr>
        </table>

            </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>

    </td></tr></table>
    <script type="text/javascript">

        function ValidarBusqueda() {
            var bolValida = true;

            var strNumOperacion = $.trim($("#<%= txtNumOperacion.ClientID %>").val());
            var strBanco = $.trim($("#<%= ddlBanco.ClientID %>").val());

            var txtNumOperacion = document.getElementById('<%= txtNumOperacion.ClientID %>');
            var ddlBanco = document.getElementById('<%= ddlBanco.ClientID %>');

            if (strNumOperacion == "") {
                txtNumOperacion.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                txtNumOperacion.style.border = "1px solid #888888";
            }

            if (strBanco == "0") {
                ddlBanco.style.border = "1px solid Red";
                bolValida = false;
            }
            else {
                ddlBanco.style.border = "1px solid #888888";
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
        };

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
    </script>
</asp:Content>