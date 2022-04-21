<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmDigitaliza.aspx.cs" Inherits="SGAC.WebApp.Accesorios.FrmDigitaliza" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc3" %>

<%@ Register src="SharedControls/ctrlScanner.ascx" tagname="ctrlScanner" tagprefix="uc1" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">     
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <style type="text/css">
        .style3
        {
            width: 659px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    
    <div>
        <br />

        <table class="mTblTituloM" align="center">
            <tr><td><h2><asp:Label ID="lblTituloDigitalizar" runat="server" Text="Digitalizar Documento"></asp:Label></h2></td></tr>
        </table>    
        <table style="width: 54%;" align="center" >
            <tr>
                <td><asp:Label runat="server" Text="Solicitante:" ID="Label4" Width="80px"></asp:Label></td>
                <td colspan="2"><asp:Label runat="server" Text="Rafael Torres Mejía" ID="Label7" Width="400px"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Tarifa:" ID="Label8"></asp:Label></td>
                <td colspan="2">
                    <asp:Label runat="server" Text="1 - Acta de Nacimiento" ID="Label9" Width="400px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Fecha:" ID="Label3"></asp:Label></td>
                <td>
                            <asp:TextBox ID="txtFechaRecepcion" runat="server" Width="100px" ReadOnly="true" Enabled="false" />
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/img_16_calendar.png"
                                ImageAlign="AbsMiddle" />
                </td>
                <td>
                    <asp:Label runat="server" Text="RGE: 11" ID="Label1" Width="246px"></asp:Label>
                </td>
            </tr>
            </table>
        <br />
        <table align="center" style="height: 431px; width: 521px">
            <tr>
                <td align="center">
                    <uc1:ctrlScanner ID="ctrlScanner1" runat="server" />
                </td>
            </tr>
            </table>
        
    </div>
</asp:Content>

