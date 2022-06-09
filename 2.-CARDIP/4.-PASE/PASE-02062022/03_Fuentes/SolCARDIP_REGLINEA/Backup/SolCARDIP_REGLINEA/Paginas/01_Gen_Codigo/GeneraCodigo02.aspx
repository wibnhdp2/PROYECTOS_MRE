<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/PrincipalForm.Master"
    AutoEventWireup="true" CodeBehind="GeneraCodigo02.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas._01_Gen_Codigo.GeneraCodigo02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder2_"

        function RefreshParent(url) {
            window.parent.location.href = url;
        }

        function copiarCaptcha() {
            var aux = document.createElement("input");
            aux.setAttribute("value", document.getElementById(master + "hdnCaptcha").value);
            document.body.appendChild(aux);
            aux.select();
            document.execCommand("copy");
            document.body.removeChild(aux);
        }

        function mouseDown(control) {
            control.src = "../../Imagenes/Iconos/copy48Black.png";
        }

        function mouseUp(control) {
            control.src = "../../Imagenes/Iconos/copy48White.png";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <table class="TablaConformidad" style="width: 100%;">
                <tr>
                    <td align="center">
                        <div class="alert alert-danger" role="alert">
                            <h4 class="alert-heading">
                                Información Importante!</h4>
                            <p>
                                Esta plataforma tiene como finalidad el poder realizar una solicitud en linea de
                                emisión de carné de identidad para extranjeros en el Perú.</p>
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
                                            Su código de solicitud es:</h5>
                                    </blockquote>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="vertical-align: middle;">
                                    <div class="input-group">
                                        <input type="hidden" runat="server" id="hdnCaptcha" value="" />
                                        <img id="imgCodSol" width="300" height="60" enableviewstate="false" alt="" runat="server"
                                            src="" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton runat="server" ID="ibtCopiarCodigo" ImageUrl="../../Imagenes/Iconos/copy48White.png"
                                            ToolTip="Copiar Solicitud" onmousedown="mouseDown(this);" onmouseup="mouseUp(this);"
                                            OnClientClick="copiarCaptcha();return false;" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <blockquote>
                            <h6>
                                Este código le permitira recuperar y actualizar la informacion de la presente solicitud
                                hasta que esa misma se apruebe / inice el tramite de emision de carné de identidad</h6>
                        </blockquote>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <hr />
                        <div class="form-group">
                            <div class="col">
                                <asp:Button runat="server" ID="btnContinuar" Text="Continuar" OnClick="nextPage"
                                    CssClass="btn btn-primary" Width="150px" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
