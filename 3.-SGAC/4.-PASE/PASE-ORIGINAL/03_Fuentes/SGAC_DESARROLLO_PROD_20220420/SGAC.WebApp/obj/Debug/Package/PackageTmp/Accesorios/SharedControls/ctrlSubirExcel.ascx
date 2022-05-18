<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlSubirExcel.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlSubirExcel" %>
<asp:FileUpload id="FileUploadControl" runat="server"  accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"/>
    <asp:Button runat="server" id="UploadButton" CssClass="btnUpload" text="      Subir" onclick="UploadButton_Click"/>
    <br /><br />
    <asp:Label runat="server" id="StatusLabel" text=" " />
    <asp:HiddenField ID="hRutaArchivo" runat="server" />
