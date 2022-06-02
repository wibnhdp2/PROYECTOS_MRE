<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlReimprimirbtn.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlReimprimirbtn" %>
<asp:Button ID="btnReimprimir" runat="server" Text="   Reimpresión de Autoadhesivo" Width="250px"
CssClass="btnPrint" TabIndex="50" Enabled="False" 
    onclick="btnReimprimir_Click" />

<asp:HiddenField ID="hSeImprime" runat="server" />
<asp:HiddenField ID="HFGUID" runat="server" />
