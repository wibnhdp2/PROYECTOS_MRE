<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="RegistroLinea.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas.Principales.RegistroLinea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Estilos/all.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Estilos/sb-admin-2.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1 class="h3 mb-4 text-gray-800">
        Solicitud de Emisión de Carné de Identidad</h1>
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <table style="width:100%">
                <tr>
                    <td align="center">
                        <iframe id="PDFdocument" runat="server" height="900" width="100%" src="../01_Gen_Codigo/GeneraCodigo01.aspx"  ></iframe>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdfldAF" Value="1" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="mensajePrincipal" tabindex="-1" role="dialog" data-backdrop="static"
                aria-hidden="true">
                <div id="imprimir" class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">NOTA!
                            </h5>
                            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="card-body">
                                <h5 class="card-title" style="color:red; font-weight:bold;"><u> Información para iniciar su solicitud de Carné:</u></h5>
                                <h5 style="color:red; font-weight:bold;">Usted podrá realizar la solicitud de carné de identidad en línea, <u>si cuenta con alguna de las calidades migratorias consignadas en el siguiente cuadro :</u></h5>
                                <hr />
                                <ul class="list-group">
                                  <li class="list-group-item" style="font-weight:bold;">DIPLOMÁTICO</li>
                                  <li class="list-group-item"style="font-weight:bold;">CONSULAR</li>
                                  <li class="list-group-item"style="font-weight:bold;">OFICIAL</li>
                                  <li class="list-group-item"style="font-weight:bold;">FAMILIAR DE OFICIAL</li>
                                  <li class="list-group-item"style="font-weight:bold;">COOPERANTE</li>
                                  <li class="list-group-item"style="font-weight:bold;">INTERCAMBIO</li>
                                  <li class="list-group-item"style="font-weight:bold;">PERIODISTA</li>
                                   <li class="list-group-item"style="font-weight:bold;">PRODUCCIÓN ARTÍSTICA</li>
                                </ul>
                            </div>
                            <span style="font-weight:bold;">* Si usted tiene dudas respecto a su calidad migratoria, puede comunicarse al 2043678 y al 2043303.</span>
                        </div>
                        <div class="modal-footer">
                            <button id="btnAceptar" class="btn btn-primary" type="button" data-dismiss="modal">
                                Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>

    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/jquery.min.js")%>' type="text/javascript"></script>
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/bootstrap.bundle.min.js")%>'
        type="text/javascript"></script>
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/sb-admin-2.min.js")%>'
        type="text/javascript"></script>
</asp:Content>
