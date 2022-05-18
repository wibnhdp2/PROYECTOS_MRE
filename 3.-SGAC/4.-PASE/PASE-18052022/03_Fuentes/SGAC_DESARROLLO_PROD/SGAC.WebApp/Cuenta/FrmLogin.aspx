<%@ Page Title="Autenticaci&oacute;n del SGAC" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FrmLogin.aspx.cs" Inherits="SGAC.WebApp.Cuenta.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="../Styles/modal.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script> 
    <style type="text/css">
        .style1
        {
            height: 25px;
        }
      
    input::-webkit-input-placeholder {
        font-family:Arial;
        font-size:12px;
        text-transform:none;
    }
    input:-moz-placeholder {
      font-family:Arial;
      font-size:12px;
      text-transform:none;
    }
    input:-ms-input-placeholder { 
      font-family:Arial;
      font-size:12px;
      text-transform:none;
    }

    </style>
   
  
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div style="position: fixed; bottom: -2px; right: 0; border: 1px solid rgb(132, 132, 132); background-color: rgb(132, 132, 132);
        padding: 5px 5px 5px 5px; ">
        <asp:CheckBox ID="chkSalto" runat="server" Enabled="false" Checked="false" style="display:none;" />
    </div>
    <div style="text-align: right; display:none;">
        <a href="#" onclick="Popup();">Desbloquear</a>
        <%--<asp:Button ID="btnDesbloque" runat="server" OnClientClick = "Popup(); return false"  Text="Desbloquear" />--%>
    </div>
    <div id="simpleModal_1" class="modal">
        <div class="modal-window">
            <div class="modal-titulo">
                <asp:ImageButton ID="imgCerrarPopup" CssClass="close" ImageUrl="~/Images/close.png"
                    OnClientClick="cerrarPopup(); return false" runat="server" />
                <span>DESBLOQUEAR</span>
            </div>
            <div class="modal-cuerpo">
                <h3>
                    Ingrese usuario y contraseña</h3>
                <asp:TextBox ID="txtUsuario" CssClass="txtLogin" placeholder="Usuario" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txtCodigo" TextMode="Password" CssClass="txtLogin" placeholder="Contraseña" runat="server"></asp:TextBox><br />
            </div>
            <div class="modal-pie">
                <asp:Button ID="btnDesbloquear" runat="server" CssClass="btnLogin" Text="Desbloquear"
                    OnClick="btnDesbloquear_Click" />
            </div>
        </div>
    </div>
    <div>
     <asp:HiddenField ID="hDominio" runat="server" />
        <asp:HiddenField ID="hUserPC" runat="server" />
        <table align="center">
            <tr align="center">
                <td>
                    <h2>
                        <asp:Label ID="lblInicioSesion" runat="server" Text="Inicia Sesión" Font-Names="Arial" ></asp:Label>
                    </h2>
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    <div class="divLogin">
                        <table>
                            <%--Agregado mientras idioma está invisible--%>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Image ID="Image1" runat="server" CssClass="imgLogin" ImageUrl="~/Images/img_48_user_gray.png" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="center" colspan="2">
                                    <asp:ImageButton ID="btnFlagEs" runat="server" ImageUrl="~/Images/img_flag_es.png"
                                        CssClass="btnFlag" OnClick="btnFlagEs_Click" ToolTip="Español" Visible="false" />
                                    <asp:ImageButton ID="btnFlagEn" runat="server" ImageUrl="~/Images/img_flag_en.png"
                                        CssClass="btnFlag" OnClick="btnFlagEn_Click" ToolTip="Ingles" Visible="false" />
                                    <asp:RadioButtonList ID="rblLanguage" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="True" OnSelectedIndexChanged="rblLanguage_SelectedIndexChanged"
                                        TabIndex="5" Visible="false">
                                        <asp:ListItem Value="ES" Selected="True">Español</asp:ListItem>
                                        <asp:ListItem Value="EN">Inglés</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <%--Agregado mientras idioma está invisible--%>
                            <tr>
                                <asp:HiddenField ID="hIP_Cliente" runat="server" />
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblAlias" runat="server">Alias:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAlias" runat="server" CssClass="txtLogin" MaxLength="20" onkeypress="return validarCuentasUsuario(event); "
                                        TabIndex="1" placeholder="Ingrese su cuenta de red"/>
                                </td>
                                <td>
                                    <asp:Label ID="lblAliasValidation" runat="server" Text="*" Visible="False" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    &nbsp;
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lblPassword" runat="server">Contraseña:</asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off"
                                        CssClass="txtLogin" MaxLength="20" onkeypress="return validarCuentasUsuario(event)"
                                        TabIndex="2" placeholder="Ingrese su contraseña"/>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lblPasswordValidation" runat="server" Text="*" Visible="False" ForeColor="Red"></asp:Label>
                                </td>
                                
                            </tr>
                            <div>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkItinerante" runat="server" Text="Itinerante?" 
                                            Checked="false" AutoPostBack="True" 
                                            oncheckedchanged="chkItinerante_CheckedChanged" />
                                    </td>
                                </tr>
                            </div>
                            <div id="ocultarItinerante" runat="server">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCiudad" runat="server" Text="Cuidad: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCiudad" runat="server"  Width="200px" CssClass="txtLogin">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </div>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right" colspan="2">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar sesión" CssClass="btnLogin"  
                                        ValidationGroup="LoginValidationGroup" OnClick="btnLogin_Click" TabIndex="4" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblMensaje1" runat="server" ForeColor="Red" TabIndex="7"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblMensaje"  runat="server" ForeColor="Red" Font-Bold="true" TabIndex="7" Font-Size="14px" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Label ID="lblVersion" runat="server" Text="Label" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Label ID="lblFechaActualizacion" runat="server" Text="Label" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
       
    </div>
    <script language="javascript" type="text/javascript">
        function Popup() {            
            document.getElementById('simpleModal_1').style.display = 'block';
        }
        function cerrarPopup() {

            document.getElementById('simpleModal_1').style.display = 'none';
        }
        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
        function validarCuentasUsuario(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            var letra = false;
            if (charCode > 1 && charCode < 4) {
                letra = true;
            }
            if (charCode == 8) {
                letra = true;
            }
            if (evt.keyCode == 13) evt.keyCode = 9; return true;

            if (charCode == 32) {
                letra = true;
            }
            if (charCode >= 58 && charCode <= 59) {
                letra = true;
            }
            if (charCode >= 46 && charCode <= 60) {
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

            var letras = "áéíóúÁÉÍÓÚaeiouAEIOUñÑ.:,;_-=¿?¡![]{}()*+@/%#$&\"";
            var tecla = String.fromCharCode(charCode);
            var n = letras.indexOf(tecla);
            if (n > -1) {
                letra = true;
            }

            return letra;
        }     

    </script>
</asp:Content>
