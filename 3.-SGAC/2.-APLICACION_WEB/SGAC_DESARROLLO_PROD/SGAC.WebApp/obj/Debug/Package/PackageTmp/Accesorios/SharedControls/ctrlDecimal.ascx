<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlDecimal.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlDecimal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:TextBox ID="txtDecimal" runat="server" MaxLength="12" 
    CssClass="campoNumero" ontextchanged="txtDecimal_TextChanged"></asp:TextBox>
<cc1:MaskedEditExtender ID="txtDecimal_MaskedEditExtender" runat="server" Enabled="True"
    Mask="9,999,999.99" MaskType="Number" TargetControlID="txtDecimal" MessageValidatorTip="true"
    ErrorTooltipEnabled="True">
</cc1:MaskedEditExtender>