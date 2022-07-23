<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master" AutoEventWireup="true" CodeBehind="AdministradorReportes.aspx.cs" Inherits="SolCARDIP.Paginas.Administrador.AdministradorReportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html, body
        {
            height:100%;
            margin:0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updPrincipal">
        <ContentTemplate>
            <table class="AnchoTotal">
                <tr>
                    <td>
                        <div>
                            <div id="divModal2" runat="server" style="position:absolute;z-index:1;display:none" class="modalBackgroundLoad"></div>
                            <div id="divCargando" runat="server" style="position:absolute;z-index:2;top:50%;left:45%" onblur="ContentPlaceHolder1_divCargando.focus();">
                                <img alt="gif" src="../../Imagenes/Gifs/ajax-loader(1).gif" style="width:100px;height:95px" />
                            </div>
                            <div id="divModal" runat="server" style="display:none" class="modalBackgroundLoad"></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="AnchoTotal">
                            <tr>
                                <td>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td style="width:100%">
                                                <table style="width:100%">
                                                    <tr id="trTabs">
                                                        <td id="tabPest0" class="Tabs" style="display:block;" onclick="tabActual(0);">Reportes</td>
                                                        <%--<td id="tabPest1" class="Tabs" style="display:block;" onclick="tabActual(1);">Calidad Migratoria</td>
                                                        <td id="tabPest2" class="Tabs" style="display:block;" onclick="tabActual(2);">Cargos</td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="AnchoTotal">
                                        <tr>
                                            <td id="tdContenedor">
                                                <div id="tab0" style="display:block;">
                                                    
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
