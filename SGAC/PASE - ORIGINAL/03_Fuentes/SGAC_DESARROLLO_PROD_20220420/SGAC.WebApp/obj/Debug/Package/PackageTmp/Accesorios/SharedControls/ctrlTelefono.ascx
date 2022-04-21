<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlTelefono.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlTelefono" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
 


<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
<br />
<asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
<asp:MaskedEditExtender ID="txtTelefono_MaskedEditExtender" runat="server" 
    TargetControlID="txtTelefono" Mask="99.99" MaskType="Number">
</asp:MaskedEditExtender>
<p>
    &nbsp;</p>

 


