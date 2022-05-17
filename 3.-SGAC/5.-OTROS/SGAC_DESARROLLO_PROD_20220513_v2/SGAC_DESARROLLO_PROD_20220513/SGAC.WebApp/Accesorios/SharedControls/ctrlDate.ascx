<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlDate.ascx.cs" Inherits="SGAC.WebApp.Accesorios.SharedControls.ctrlDate"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1"  %>

<style type="text/css"> 
    input::-ms-clear{
    display: none;
    }
</style>

<%--onkeypress="return Validar_Fecha(event, this);"--%>
<%--onblur="validaFechaMMMddyyyy(this);"--%>

<asp:TextBox ID="TxtFecha" 
             runat="server" 
             MaxLength="11" Columns="11"  
             placeholder="MMM-dd-yyyy" data-mask="LLL-99-9999"/>
<asp:Label ID="lblErrorDate" runat="server" ForeColor="Maroon" Text=""></asp:Label>
<asp:ImageButton ID="btnFecha" runat="server" ImageUrl="~/Images/calendar.png"
               ToolTip="Calendario" ImageAlign="AbsMiddle" />


<cc1:CalendarExtender ID="ceFecha" 
                      runat="server" 
                      Format="MMM-dd-yyyy" 
                      TargetControlID="TxtFecha"
                      PopupButtonID="btnFecha" 
                      EnableViewState="true"
                      ViewStateMode="Enabled" />
  
<script type="text/javascript">

    function Validar_Fecha_<%=uniqueKey %>(evt, ctrl) {        
        var charCode = (evt.which) ? evt.which : event.keyCode
        var FIND = "-";
        var x = ctrl.value;
        var y = x.indexOf(FIND);

        if (event.keyCode == 45 || event.keyCode == 47 || event.keyCode == 32) {
            return false;
        }
        var valor = true;
        if (x.length < 3 || y == 0 || (y>0 && y<3 )) {

            if ((event.keyCode != 32) && (event.keyCode < 65) || (event.keyCode > 90) && (event.keyCode < 97) || (event.keyCode > 122)) {
                event.returnValue = false;
            }
        }
        else {
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (charCode == 13) {
                return false;
            }
        }

        var mes = x.split('-');
        if (x.length == 3) {
            x = x + "-";
            ctrl.value = x;
        }
        var dia = x.split('-');
        if (x.length == 6) {
            x = x + "-";
            ctrl.value = x;
        }
        
        return true;
    }

    function Validando_Fecha(fecha, ctrl) {
        fecha = get_Date(fecha);
        var dtCh = "/";
        var minYear = 1900;
        var maxYear = 2100;

        function isInteger(s) {
            var i;
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (((c < "0") || (c > "9"))) return false;
            }
            return true;
        }

        function stripCharsInBag(s, bag) {
            var i;
            var returnString = "";
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (bag.indexOf(c) == -1) returnString += c;
            }
            return returnString;
        }

        function daysInFebruary(year) {
            return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
        }

        function DaysArray(n) {
            for (var i = 1; i <= n; i++) {
                this[i] = 31
                if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                if (i == 2) { this[i] = 29 }
            }
            return this
        }

        function isDate(dtStr) {
            var daysInMonth = DaysArray(12)
            var pos1 = dtStr.indexOf(dtCh)
            var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
            var strDay = dtStr.substring(0, pos1)
            var strMonth = dtStr.substring(pos1 + 1, pos2)
            var strYear = dtStr.substring(pos2 + 1)
            strYr = strYear
            if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
            if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            month = parseInt(strMonth)
            day = parseInt(strDay)
            year = parseInt(strYr)
            if (pos1 == -1 || pos2 == -1) {
                return false
            }
            if (strMonth.length < 1 || month < 1 || month > 12) {
                return false
            }
            if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                return false
            }
            if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                return false
            }
            if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                return false
            }
            return true
        }
        var nameControl = ctrl.id.substring(0, ctrl.id.length - 8) + 'lblErrorDate';
        if (!isDate(fecha)) {

            document.getElementById(nameControl).innerHTML = "Error.!";
            

        } else {
            document.getElementById(nameControl).innerHTML = "";
        }
    }

    function validaFechaMMMddyyyy_<%=uniqueKey %>(crl) {
        if (crl.value == '' || crl.value == 'MMM-dd-yyyy') {
            return;
        }
        var fecha = get_Date(crl.value);

        var dtCh = "/";
        var minYear = 1900;
        var maxYear = 2100;
        function isInteger(s) {
            var i;
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (((c < "0") || (c > "9"))) return false;
            }
            return true;
        }

        function stripCharsInBag(s, bag) {
            var i;
            var returnString = "";
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (bag.indexOf(c) == -1) returnString += c;
            }
            return returnString;
        }

        function daysInFebruary(year) {
            return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
        }
        function DaysArray(n) {
            for (var i = 1; i <= n; i++) {
                this[i] = 31
                if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                if (i == 2) { this[i] = 29 }
            }
            return this
        }

        function isDate(dtStr) {
            var daysInMonth = DaysArray(12)
            var pos1 = dtStr.indexOf(dtCh)
            var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
            var strDay = dtStr.substring(0, pos1)
            var strMonth = dtStr.substring(pos1 + 1, pos2)
            var strYear = dtStr.substring(pos2 + 1)
            strYr = strYear
            if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
            if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            month = parseInt(strMonth)
            day = parseInt(strDay)
            year = parseInt(strYr)
            if (pos1 == -1 || pos2 == -1) {
                return false
            }
            if (strMonth.length < 1 || month < 1 || month > 12) {
                return false
            }
            if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                return false
            }
            if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                return false
            }
            if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                return false
            }
            
            return true
        }
        var nameControl = crl.id.substring(0, crl.id.length - 8) + 'lblErrorDate';
        if (!isDate(fecha)) {

            document.getElementById(nameControl).innerHTML = "Error.!";

        } else {
            document.getElementById(nameControl).innerHTML = "";
        }

    }

    function get_Date(strfecha) {
        var s_NameMes = strfecha.substring(0, 3);
        var s_dia = strfecha.substring(4, 6);
        var s_year = strfecha.substring(7, 11);

        return s_dia + '/' + Search_Number_Mes(s_NameMes) + '/' + s_year;
    }

    function Search_Number_Mes(strNameMes) {
        if (strNameMes.toLowerCase() == 'ene') {
            return '01';
        }
        if (strNameMes.toLowerCase() == 'feb') {
            return '02';
        }
        if (strNameMes.toLowerCase() == 'mar') {
            return '03';
        }
        if (strNameMes.toLowerCase() == 'abr') {
            return '04';
        }
        if (strNameMes.toLowerCase() == 'may') {
            return '05';
        }
        if (strNameMes.toLowerCase() == 'jun') {
            return '06';
        }
        if (strNameMes.toLowerCase() == 'jul') {
            return '07';
        }
        if (strNameMes.toLowerCase() == 'ago') {
            return '08';
        }
        if (strNameMes.toLowerCase() == 'sep') {
            return '09';
        }
        if (strNameMes.toLowerCase() == 'oct') {
            return '10';
        }
        if (strNameMes.toLowerCase() == 'nov') {
            return '11';
        }
        if (strNameMes.toLowerCase() == 'dic') {
            return '12';
        }
        return '__';
    }
</script>