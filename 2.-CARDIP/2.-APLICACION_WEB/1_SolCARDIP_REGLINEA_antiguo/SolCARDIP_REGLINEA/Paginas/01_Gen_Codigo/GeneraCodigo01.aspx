<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/PrincipalForm.Master"
    AutoEventWireup="true" CodeBehind="GeneraCodigo01.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas._01_Gen_Codigo.GeneraCodigo01" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder2_";
        function validarCaptcha() {
            var txtCaptcha = document.getElementById(master + "txtCaptcha");
            var hdn = document.getElementById(master + "hdfldCaptchaUsuario");
            if (txtCaptcha != null) {
                valor = txtCaptcha.value;
                valor = valor.toUpperCase();
                hdn.value = valor;
                if (valor != "") {
                    if (valor.length == 5) {
                        return true;
                    }
                    else {
                        txtCaptcha.value = "";
                        alert("NO ES UN VALOR CAPTCHA VALIDO");
                        return false;
                    }
                }
                else {
                    txtCaptcha.value = "";
                    alert("NO ES UN VALOR CAPTCHA VALIDO");
                    return false;
                }
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            
            <table class="TablaConformidad" style="width: 100%; text-align: center;">
                <tr>
                    <td align="center">
                        <div class="alert alert-danger" role="alert">
                            <h4 class="alert-heading">
                                ¡Información Importante!</h4>
                            <p>
                                Esta plataforma tiene como finalidad el poder realizar una solicitud en linea de
                                emisión de carné de identidad para extranjeros en el Perú.</p>
                            <p>
                                Para realizar una nueva solicitud, presionar F5.</p>
                            <hr>
                        </div>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <table style="text-align: center;">
                            <tr>
                                <td align="center">
                                    <blockquote>
                                        <h5>
                                            Ingrese el código captcha</h5>
                                    </blockquote>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <img id="imgCaptcha" width="250" height="80" enableviewstate="false" alt="" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" class="EtiquetaCentro">
                                    <asp:LinkButton ID="lbnActualizarCaptcha" Text="Actualizar Código Captcha" ForeColor="#7b012a"
                                        runat="server" OnClick="lbnActualizarCaptcha_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:TextBox runat="server" ID="txtCaptcha" CssClass="form-control form-control-sm  text-uppercase"
                                        Font-Size="16px" Style="text-align: center;" Font-Bold="true" ForeColor="#7b012a"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <div class="form-group">
                            <div class="col">
                                <asp:Button runat="server" ID="btnIniciarSol" CssClass="btn btn-primary" Text="Nueva Solicitud"
                                    CommandName="0" OnClientClick="return validarCaptcha();" OnClick="nextPage" Width="190px" />
                                <asp:Button runat="server" ID="Button1" CssClass="btn btn-primary" CommandName="1"
                                    OnClick="nextPage" OnClientClick="return validarCaptcha();" Text="Buscar Solicitud"
                                    Width="190px" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdfldCaptchaUsuario" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>
