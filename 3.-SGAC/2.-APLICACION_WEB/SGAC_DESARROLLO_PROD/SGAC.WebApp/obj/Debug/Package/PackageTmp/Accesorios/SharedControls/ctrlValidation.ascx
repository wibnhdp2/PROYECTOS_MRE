<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlValidation.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlValidation" %>
<link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .imgIcono
    {
        margin-left: 10px;
    }
    .trAdvertencia
    {
        background-color: #F2F1C2; 
        border-color: #f89406;
        color: #4B4F5E;    
        height: 16px;        
        width:100%;
    }
    .trError
    {
        background-color: #FAD4D9; /*#FA5858*//*#FE2E2E*/ 
        border-color: #bd362f;
        color: #4B4F5E;    
        height: 15px;
        width:100%;
    }
    .trInformativo
    {
        background-color: #A9F5BC/*#2E9AFE*/; 
        border-color: #51a351;
        color: #4B4F5E;    
        height: 15px;
        width:100%;
    }

     
</style>

<table align="left" id="trMensaje" runat="server" width="100%">
    <tr>
        <td width="40px">
            <asp:Image ID="imgIcono" 
                       runat="server" 
                       ImageUrl="~/Images/img_16_warning.png" 
                       CssClass="imgIcono" />
        </td>
        <td>
            <asp:Label ID="lblEtiqueta" 
                       runat="server" 
                       Text="Mensaje Validación" 
                       CssClass="lblEtiqueta"></asp:Label>
       </td>
    </tr>
</table>