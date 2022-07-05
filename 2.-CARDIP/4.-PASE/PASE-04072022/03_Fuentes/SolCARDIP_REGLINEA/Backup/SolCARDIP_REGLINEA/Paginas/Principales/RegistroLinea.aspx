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
                        <iframe id="PDFdocument" runat="server" height="490px" width="100%" src="../01_Gen_Codigo/GeneraCodigo01.aspx"  ></iframe>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdfldAF" Value="1" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
