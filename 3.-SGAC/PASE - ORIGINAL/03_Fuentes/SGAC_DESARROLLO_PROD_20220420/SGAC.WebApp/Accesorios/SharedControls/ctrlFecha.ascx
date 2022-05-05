<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlFecha.ascx.cs" Inherits="SGAC.WebApp.Fecha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css"> 
    .centrado
    {
        text-align:center;
        width: 100px;
    }
</style>

<asp:TextBox ID="TxtFecha" 
             runat="server" 
             Columns="11" 
             MaxLength="10"
              />

<asp:Image ID="btnFecha" 
           runat="server" 
           ImageUrl="~/Images/img_16_calendar.png" 
           ImageAlign="AbsMiddle" 
           ToolTip="Calendario"  />

<cc1:CalendarExtender ID="ceFecha" 
                      runat="server" 
                      Format="dd/MM/yyyy" 
                      TargetControlID="TxtFecha"
                      PopupButtonID="btnFecha" 
                      EnableViewState="true"
                      ViewStateMode="Enabled" />

<cc1:MaskedEditExtender ID="meFecha" 
                        runat="server" 
                        MaskType="Date" 
                        Mask="99/99/9999" 
                        UserDateFormat="DayMonthYear" 
                        EnableViewState="true" 
                        ViewStateMode="Enabled"
                        ClearMaskOnLostFocus="true" 
                        ErrorTooltipEnabled="true" 
                        MessageValidatorTip="true" 
                        ClearTextOnInvalid="true" 
                        TargetControlID="txtFecha" />

<script language="javascript" type="text/javascript">

    function valFecha(oTxt) {
        var bOk = true;
        if (oTxt.value != "__/__/____") {
            bOk = bOk && (valDia(oTxt));
            bOk = bOk && (valMes(oTxt));
            bOk = bOk && (valAno(oTxt));

            bOk = bOk && (valSep(oTxt));

            if (!bOk) {
                alert("Fecha inválida");
                oTxt.value = "";
                oTxt.focus();
            }

            if (document.getElementById("ctl00_SesPlaceHolder_txtRegDia") != undefined || document.getElementById("ctl00_SesPlaceHolder_txtRegDia") != null) {
                llenarDia(oTxt);
            }
        }
    }

    function valDia(oTxt) {
        var bOk = false;
        var nDia = parseInt(oTxt.value.substr(0, 2), 10);
        bOk = bOk || ((nDia >= 1) && (nDia <= finMes(oTxt)));
        return bOk;
    }

    function valMes(oTxt) {
        var bOk = false;
        var nMes = parseInt(oTxt.value.substr(3, 2), 10);
        bOk = bOk || ((nMes >= 1) && (nMes <= 12));
        return bOk;
    }

    function valAno(oTxt) {
        var bOk = true;
        var nAno = oTxt.value.substr(6);
        bOk = bOk && (nAno.length == 4);
        if (bOk) {
            for (var i = 0; i < nAno.length; i++) {
                bOk = bOk && esDigito(nAno.charAt(i));
                if (nAno.charAt(i) != 2 && nAno.charAt(i) != 1 && i == 0) {
                    bOk = false;
                }
            }
        }
        if (nAno.charAt(0) == 1 && nAno.charAt(1) != 9)
            bOk = false;
        return bOk;
    }
    function valSep(oTxt) {
        var bOk = false;
        bOk = bOk || ((oTxt.value.charAt(2) == "/") && (oTxt.value.charAt(5) == "/"));
        return bOk;
    }
    function esDigito(sChr) {
        var sCod = sChr.charCodeAt(0);
        return ((sCod > 47) && (sCod < 58));
    }
    function finMes(oTxt) {
        var nMes = parseInt(oTxt.value.substr(3, 2), 10);
        var nRes = 0;
        switch (nMes) {
            case 1: nRes = 31; break;
            case 2:
                if ((parseInt(oTxt.value.substr(6)) % 4 == 0) && ((parseInt(oTxt.value.substr(6)) % 100 != 0) || (parseInt(oTxt.value.substr(6)) % 400 == 0))) {
                    nRes = 29;
                }
                else {
                    nRes = 28;
                }

                break;
            case 3: nRes = 31; break;
            case 4: nRes = 30; break;
            case 5: nRes = 31; break;
            case 6: nRes = 30; break;
            case 7: nRes = 31; break;
            case 8: nRes = 31; break;
            case 9: nRes = 30; break;
            case 10: nRes = 31; break;
            case 11: nRes = 30; break;
            case 12: nRes = 31; break;
        }
        return nRes;
    }

    function OnClientShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 10005;
    }

    function llenarDia(oTxt) {

        if (document.getElementById("ctl00_SesPlaceHolder_txtRegMes1_CalendarExtender1").value != "") {
            var nDia = parseInt(oTxt.value.substr(0, 2), 10);
            var txtRegDia = document.getElementById("ctl00_SesPlaceHolder_txtRegDia");
            txtRegDia.value = nDia;
        }
    }

    function ShowModalPopup1(ModalBehaviour) {
        parent.ShowModalPopup(ModalBehaviour);
    }

    function HideModalPopup1(ModalBehaviour) {
        parent.HideModalPopup(ModalBehaviour);
    }
</script>
