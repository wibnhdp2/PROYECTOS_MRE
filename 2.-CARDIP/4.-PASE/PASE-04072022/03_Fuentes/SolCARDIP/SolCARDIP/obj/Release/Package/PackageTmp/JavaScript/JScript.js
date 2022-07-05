// VALIDACION DE FECHAS ------------------------------------------------------------------------------------------------------------
function validaFechaDDMMAAAA(fecha) {
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
    if (isDate(fecha)) {
        return true;
    } else {
        return false;
    }
}
// FECHA MAYOR MENOR ------------------------------------------------------------------------------------------------------------
function compararFechaMayorActual(fecha) {
    var now = new Date();
    var todayAtMidn = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    var mes = (parseInt(fecha.substring(3, 5)) - 1);
    var year = fecha.substring(6, 10);
    var dia = fecha.substring(0, 2);

    var fechaInicial1 = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    valorFecha1 = fechaInicial1.valueOf();
    FechaActual = valorFecha1 + (60 * 24 * 60 * 60 * 1000);

    var fechaInicial2 = new Date(year, mes, dia);
    valorFecha2 = fechaInicial2.valueOf();
    fechaComparar = valorFecha2 + (60 * 24 * 60 * 60 * 1000);

    if (fechaComparar > FechaActual) {
        return false;
    }
    return true;
}
// COMPARAR FECHAS MAYOR MENOR ------------------------------------------------------------------------------------------------------------
//function compararFechas(fechaMenor, fechaMayor) {
//    var mesMenor = (parseInt(fechaMenor.substring(3, 5)) - 1);
//    var yearMenor = fechaMenor.substring(6, 10);
//    var diaMenor = fechaMenor.substring(0, 2);

//    var mesMayor = (parseInt(fechaMayor.substring(3, 5)) - 1);
//    var yearMayor = fechaMayor.substring(6, 10);
//    var diaMayor = fechaMayor.substring(0, 2);

//    var fechaInicialMenor = new Date(yearMenor, mesMenor, diaMenor);
//    valorFechaMenor = fechaInicialMenor.valueOf();
//    fechaCompararMenor = valorFecha2 + (60 * 24 * 60 * 60 * 1000);

//    var fechaInicialMayor = new Date(yearMayor, mesMayor, diaMayor);
//    valorFechaMayor = fechaInicialMayor.valueOf();
//    fechaCompararMayor = valorFechaMayor + (60 * 24 * 60 * 60 * 1000);

//    if (fechaCompararMenor > fechaCompararMayor) {
//        return false;
//    }
//    return true;
//}

function compararFechas(fechaMenor, fechaMayor) {
        var mesMenor = (parseInt(fechaMenor.substring(3, 5)));
        var yearMenor = fechaMenor.substring(6, 10);
        var diaMenor = fechaMenor.substring(0, 2);
        var fechaMenorFor = new Date(mesMenor + "/" + diaMenor + "/" + yearMenor);

        var mesMayor = (parseInt(fechaMayor.substring(3, 5)));
        var yearMayor = fechaMayor.substring(6, 10);
        var diaMayor = fechaMayor.substring(0, 2);
        var fechaMayorFor = new Date(mesMayor + "/" + diaMayor + "/" + yearMayor);

        if (fechaMayorFor.getTime() === fechaMenorFor.getTime()) {
            return true;
        }
        if (fechaMayorFor > fechaMenorFor) {
            return true;
        }
        if (fechaMayorFor < fechaMenorFor) {
            return false;
        }
}

// FECHA HOY ------------------------------------------------------------------------------------------------------------
function getToday() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
    return(today);
}
// CONVERTIR FECHAS ---------------------------------------------------------------------------------------
function convertirFechas(Fecha) {
    var now = new Date();
    var todayAtMidn = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    var mes = (parseInt(Fecha.substring(3, 5)) - 1);
    var year = Fecha.substring(6, 10);
    var dia = Fecha.substring(0, 2);

    var fechaInicial = new Date(year, mes, dia);
    valorFecha = fechaInicial.valueOf();
    FechaFinal = valorFecha + (60 * 24 * 60 * 60 * 1000);

    return FechaFinal;
}
// SUMAR FECHAS ------------------------------------------------------------------------------------------------
function sumarFechas(fechaInicial, tipo, cantidad) {

    var cantidadFinal = 0;
    if (tipo == "day") { cantidadFinal = parseInt(cantidad); }
    if (tipo == "month") { cantidadFinal = parseInt(cantidad * 30); }
    if (tipo == "year") { cantidadFinal = parseInt(cantidad * 365); }

    var mes = (parseInt(fechaInicial.substring(3, 5)));
    var year = fechaInicial.substring(6, 10);
    var dia = fechaInicial.substring(0, 2);
    fecha = mes + "/" + dia + "/" + year;
    var dat = new Date(fecha.valueOf());
    dat.setDate(dat.getDate() + cantidadFinal);
    var dd = dat.getDate();
    var mm = dat.getMonth() + 1; //January is 0!
    var yyyy = dat.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var FechaFinal = dd + '/' + mm + '/' + yyyy;
    return FechaFinal;
}

function obtenerFechaEscrita(fechaInicial) {
//    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
//    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    var days = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
    var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

    var mes = (parseInt(fechaInicial.substring(3, 5)));
    var year = fechaInicial.substring(6, 10);
    var dia = fechaInicial.substring(0, 2);
    fecha = mes + "/" + dia + "/" + year;
    var dat = new Date(fecha.valueOf());

    var dayWeek = days[dat.getDay()];
    var montName = monthNames[dat.getMonth()];

    var dd = dat.getDate();
    var mm = dat.getMonth() + 1; //January is 0!
    var yyyy = dat.getFullYear();

    var cadena = "El Carné vencerá el " + dayWeek + " " + dd + " de " + montName + " de " + yyyy

    return cadena;
}

// COOKIES ------------------------------------------------------------------------------------------------

function utf8_to_b64(str) {
    return window.btoa(unescape(encodeURIComponent(str)));
}

function b64_to_utf8(str) {
    return decodeURIComponent(escape(window.atob(str)));
}

function str2ByteArr(str) {
    var bytes = [];

    for (var i = 0; i < str.length; ++i) {
        bytes.push(str.charCodeAt(i));
        bytes.push(0);
    }
    return bytes;
}

function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}