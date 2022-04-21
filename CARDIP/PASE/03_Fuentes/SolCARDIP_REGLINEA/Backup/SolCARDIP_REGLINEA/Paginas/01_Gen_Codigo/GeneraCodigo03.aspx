<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/PrincipalForm.Master"
    AutoEventWireup="true" CodeBehind="GeneraCodigo03.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas._01_Gen_Codigo.GeneraCodigo03" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var master = "ContentPlaceHolder2_";

        function visibleRow(valor) {
            var tr1 = document.getElementById("trMensaje");
            if (tr1 != null) {
                tr1.style.display = valor;
            }
        }

        function validarRecuperar() {
            var txt1 = document.getElementById(master + "txtCodSolicitud");
            if (txt1 != null) {
                if (txt1.value.length != 9) {
                    alert("NO ES UN CODIGO DE SOLICITUD VÁLIDO.");
                    txt1.focus();
                    return false;
                }
                return true;
            }
            return false;
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
                    <td style="padding: 10px 0px 10px 0px">
                        <table style="text-align: center;">
                            <tr>
                                <td align="center">
                                    <h5>
                                        Ingrese el codigo de Solicitud a Editar:</h5>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="vertical-align: top; padding: 15px 0px 15px 0px">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtCodSolicitud" CssClass="form-control form-control-sm"
                                            Width="30%" Height="30px" Style="text-align: center; font-family: Trebuchet MS;"
                                            Font-Size="18px" Font-Bold="true" ForeColor="#7b012a"></asp:TextBox>
                                        <div class="input-group-append">
                                            <asp:LinkButton ID="btnBuscar" CssClass="btn btn-primary" OnClientClick="return validarRecuperar();"
                                                runat="server" OnClick="buscarSolicitud"> <i class="fas fa-search fa-sm"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <div class="table-responsive">
                                        <div id="divGridView" class="dataTables_wrapper dt-bootstrap4">
                                            <asp:GridView ID="gvRegLinea" AutoGenerateColumns="false" runat="server" CssClass="table table-bordered dataTable table-hover"
                                                Width="100%" EmptyDataText="[No se encontro la solicitud requerida]" OnRowDataBound="verTemplate"
                                                OnPreRender="gvRegLinea_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="NumeroRegLinea" HeaderText="Número Solicitud">
                                                        <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                        <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConEstadoDesc" HeaderText="Estado">
                                                        <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                        <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConNombreCompleto" HeaderText="Solicitante">
                                                        <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="250px" />
                                                        <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="250px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConFechaCreacion" HeaderText="Fecha Creación">
                                                        <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                        <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ConHoraCreacion" HeaderText="Hora Creación">
                                                        <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                        <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtEditar" runat="server" ImageUrl="~/Imagenes/editar.png"
                                                                ToolTip="Editar" OnClick="nextPage" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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
                                Al recuperar la solicitud podrá actualizar los datos registrados anteriormente.
                                Tome en cuenta que solo podrá realizár esta acción hasta que se haya iniciado el
                                trámite de emisión de carne de identidad.</h6>
                        </blockquote>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <hr />
                        <div class="form-group">
                            <div class="col">
                                <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-primary" Width="150px"
                                    Text="Cancelar" OnClick="prevPage" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
