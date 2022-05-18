<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlMessageConfirm.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlMessageConfirm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>

<cc1:ModalPopupExtender 
    ID="ModalPopupExtenderMessage" 
    runat="server" 
    BackgroundCssClass="modalBackground" 
    PopupControlID="MessageBox" 
    TargetControlID="LinkButton1" >
</cc1:ModalPopupExtender>

<table id="MessageBox" runat="server" style="border:1px solid Black;width:350px;height:160px;" >

    <tr >
        <td class="messageheader" colspan="2">
            
            <asp:Label ID="LabelPopupHeader" runat="server" CssClass="messageheadertext" Text="SGAC"></asp:Label>
            
            <asp:HyperLink ID="CloseButton" runat="server">

                <asp:Image
                    ID="Image1"
                    runat="server" 
                    ToolTip="Click aquí para cerrar." 
                    ImageUrl="~/Imagenes/close.png" 
                    onclick="ocultarmsj();" />

            </asp:HyperLink>

        </td>
    </tr>

    <tr>
        <td colspan="2" style="height:10px;">
        </td>
    </tr>  
      
    <tr>
        <td style="width:15%;">
        </td>

        <td style="width:85%;">

            <div id="DivMensaje" >
                <p style="text-align: justify;">                    
                    <asp:Label ID="litMessage" runat="server" Text=""></asp:Label>
                </p>
            </div>

            <div id="valsumGroup" >
                <asp:ValidationSummary ID="summary1" ValidationGroup="" runat="server" />
                <asp:ValidationSummary ID="summary2" ValidationGroup="" runat="server" />
                <asp:ValidationSummary ID="summary3" ValidationGroup="" runat="server" />
            </div>

        </td>

    </tr>

    <tr>
        <td class="messagefooter" colspan="2">            
            <input id="ButtonOK" type="button" value="Aceptar" class="buttonmessage" onclick="ocultarmsj();" />
        </td>
    </tr>

    <tr>
        <td colspan="2" style="height:20px;">
        </td>
    </tr>

</table>

<script type="text/javascript" language="javascript">

    ocultarmsj = function () {
        $find('<%=ModalPopupExtenderMessage.ClientID %>').hide();
        if (ctrlfocus != null) {
            ctrlfocus.focus();
            ctrlfocus = null;
        }
    }

    var ctrlfocus = null;
    function LoadMessage(grpName) {
        var valsum = $get("valsumGroup");
        var spanmen = $get("DivMensaje");
        var tblmensaje = $get("<%= MessageBox.ClientID %>");
        if (valsum != null) { valsum.style.display = 'inline'; spanmen.style.display = 'none'; tblmensaje.className = 'info'; }
        $find('<%=ModalPopupExtenderMessage.ClientID %>').show();
        ctrlfocus = document.activeElement;
    }

    function ValidateJs(validationGroupName) {
        if (typeof (Page_ClientValidate) == 'function') {
            var validationResult = Page_ClientValidate(validationGroupName);
            if (validationResult == false) {

                if (!Page_IsValid) {
                    LoadMessage(validationGroupName);
                }
            } else {
                return true;
            }
        }
    }

    ShowMessage = function (mensaje, tipo) {
        var valsum = $get("valsumGroup");
        var spanmen = $get("DivMensaje");
        var tblmensaje = $get("<%= MessageBox.ClientID %>");
        var litmensaje = $get("<%= litMessage.ClientID %>");
        var classdes = "";
        valsum.style.display = 'none';
        if (mensaje != null) {
            spanmen.style.display = 'inline';
            litmensaje.innerHTML = mensaje;
            //[error] = 1
            //info = 2
            //success = 3
            //warning = 4
            switch (tipo) {
                case 1: classdes = 'error'; break;
                case 2: classdes = 'info'; break;
                case 3: classdes = 'success'; break;
                case 4: classdes = 'warning'; break;
                default: classdes = 'info'; break;
            }
            tblmensaje.className = classdes;
        }

        $find('<%=ModalPopupExtenderMessage.ClientID %>').show();
    }

</script>