/*
* General
*
* Author: Isabel Margarita Díaz Mondragón
* Date: 2014/06/20
* Updated: 2014/07/03
* Project: SGAC
*/

// PRUEBAS
function validarDecimal(e) {
    if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
        return;
    }
    else {
        if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105 ||
                (e.keyCode == 110) ||
                (e.keyCode == 190)))) {
            // not 0-9, numpad 0-9                    
            e.preventDefault();
            return;
        }
        else {
            var keyCode = e.keyCode;
            if (keyCode >= 96 && keyCode <= 105) {
                keyCode -= 48;
            }

            var value = $(this).val();

            if (keyCode == 110 || keyCode == 190) {
                if (value.indexOf('.') == -1) {
                    value += String.fromCharCode(keyCode);
                    value = parseInt(value, 10)
                    var maxNumber = $(this).data("maxnumber");
                    if (maxNumber) {
                        maxNumber = parseInt(maxNumber);
                        if (value > maxNumber) {
                            e.preventDefault();
                        }
                    }
                }
                else {
                    e.preventDefault();
                    return;
                }
            }
            else {
                value += String.fromCharCode(keyCode);
                value = parseInt(value, 10)
                var maxNumber = $(this).data("maxnumber");
                if (maxNumber) {
                    maxNumber = parseInt(maxNumber);
                    if (value > maxNumber) {
                        e.preventDefault();
                    }
                }
            }
        }
    }
};
function validarTexto(e) {
    if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
        return;
    }
    else {
        if (e.keyCode >= 65 && e.keyCode <= 90 || e.keyCode == 32 /*ESPACIO*/) {
            var keyCode = e.keyCode;
            var value = $(this).val();
            value += String.fromCharCode(keyCode);
            value = parseInt(value, 10);
        }
        else {
            e.preventDefault();
            return;
        }
    }
};
function validarNumero(e) {
    if (e.keyCode == 8 // backspace
            || e.keyCode == 9 // tab
            || e.keyCode == 13 // enter
            || e.keyCode == 27 // escape
            || e.keyCode == 46 // delete
            || (e.keyCode >= 35 && e.keyCode <= 39) // end, home, left arrow, up arrow, right arrow
            ) {
        return;
    }
    else {
        if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105))) {
            // not 0-9, numpad 0-9
            e.preventDefault();
            return;
        }
        else {
            var keyCode = e.keyCode;
            if (keyCode >= 96 && keyCode <= 105) {
                keyCode -= 48;
            }
            var value = $(this).val();
            value += String.fromCharCode(keyCode);
            value = parseInt(value, 10)
            var maxNumber = $(this).data("maxnumber");
            if (maxNumber) {
                maxNumber = parseInt(maxNumber);
                if (value > maxNumber) {
                    e.preventDefault();
                }
            }
        }
    }
};

// ---


// Validar solo numeros decimales
function validatedecimal(evt) {
    //getting evt code of pressed key
    var keycode = (evt.which) ? evt.which : evt.keyCode; // restrict user to type only one . point in number
    var parts = evt.srcElement.value.split('.');
    if (parts.length > 1 && keycode == 46)
        return false;
    //comparing pressed keycodes
    if (keycode > 31 && (keycode < 48 || keycode > 57) && keycode != 46) {
        //alert(" You can enter only characters 0 to 9 ");
        return false;
    }
    return true;
};

// validar solo numero
function validatenumber(evt) {
 
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
};

// Texto (incluido espacio)
function validarletra(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = [8,37,39,46,53];

    tecla_especial = false;
    for(var i in especiales){
        if(key == especiales[i]){
        tecla_especial = true;
        break;
        } 
    }
 
    if(letras.indexOf(tecla)==-1 && !tecla_especial)
        return false;
};

function validarsololetra(evt) {
    /*
    Especiales: otros permitidos
    32: space
    37,39,46
    */
    var key = document.all ? tecla = evt.keyCode : tecla = evt.which;
    var letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var caracter = String.fromCharCode(key).toUpperCase();
    var especiales = [8, 13];/*back, enter */

    for (var i in especiales) {
        if (tecla == especiales[i]) { return true;}
    }
    for (var i in letras) {
        if (caracter == letras[i]) { return true; }
    }
    return false;
};

function validarNombres(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyzñ";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarRangosPaginas(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789 ";
    especiales = [8, 44, 45];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarNombresApellidos(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "áéíóúabcdefghijklmnñopqrstuvwxyzñü ";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarTelefonos(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789-";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarCuentasBancarias(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789-";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarDireccionesIP(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789.";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarCorreosElectronicos(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789áéíóúabcdefghijklmnñopqrstuvwxyzñü.-_@";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarCuentasUsuario(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789abcdefghijklmnñopqrstuvwxyzñ.-_@";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarRutaFormularios(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789abcdefghijklmnñopqrstuvwxyz.-_~/";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarNumerosDecimales(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789.,";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};

function validarZonaHoraria(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "GMTgmt+-0123456789:";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};


function validarDirecciones(evt) {
    var key = (evt.which) ? evt.which : evt.keyCode;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " 0123456789áéíóúabcdefghijklmnñopqrstuvwxyzñü.-_°";
    especiales = [8, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
};


function showdialog(type_msg, title, msg, resize, height, width, aceptar) {
    //$('#img').addClass("img-info");
    //$("#msg-dialog").append(msg);    
    switch (type_msg) {
        case 'i':
            // Information
            $("#msg-dialog").html("<table><tr><td><img src='../images/img_msg_info.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" + msg + "</td></tr></table>");
            $(function () {
                $("#msg-dialog").dialog({
                    title: title,
                    autoOpen: true,
                    resizable: resize,
                    height: height,
                    width: width,
                    modal: true,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog('close');
                        }
                    }                    
                });
            });
            break;
        /*Developer by Carlos Hernández Ledesma*/
        case 'c':
            // Confirmación 
            $("#msg-dialog").html("<table><tr><td><img src='../images/img_msg_question.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" + msg + "</td></tr></table>");
            $(function () {
                $("#msg-dialog").dialog({
                    title: title,
                    autoOpen: true,
                    resizable: resize,
                    height: height,
                    width: width,
                    modal: true,
                    buttons: {
                        "Aceptar": aceptar,
                        "Cancelar": function () {
                            $(this).dialog('close');
                        }
                    }
                });
            });
            break;
        case 'a':
            // Warning
            $("#msg-dialog").html("<table><tr><td><img src='../images/img_msg_warning.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" + msg + "</td></tr></table>");
            $(function () {
                $("#msg-dialog").dialog({
                    title: title,
                    autoOpen: true,
                    resizable: resize,
                    height: height,
                    width: width,
                    modal: true,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog('close');
                        }
                    }
                });
            });
            break;
        case 'e':
            // Error
            $("#msg-dialog").html("<table><tr><td><img src='../images/img_msg_error.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>" + msg + "</td></tr></table>");
            $(function () {
                $("#msg-dialog").dialog({
                    title: title,
                    autoOpen: true,
                    resizable: resize,
                    height: height,
                    width: width,
                    modal: true,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog('close');
                           

                        }
                    }                    
                });
            });
            break;        
        default:
            $(function () {                    
                $("#msg-dialog").dialog({
                    title: title,
                    autoOpen: true,
                    resizable: resize,
                    height: height,
                    width: width,
                    modal: true,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog('close');
                        }
                    }
                });
            });
            break;
    }
};


function onlyNumbers(e) {
    var keynum;
    var keychar;

    if (window.event) {  //IE
        keynum = e.keyCode;
    }
    if (e.which) { //Netscape/Firefox/Opera
        keynum = e.which;
    }
    if ((keynum == 8 || keynum == 9 || keynum == 46 || (keynum >= 35 && keynum <= 40) ||
       (event.keyCode >= 96 && event.keyCode <= 105))) return true;

    if (keynum == 110 || keynum == 190) {
        var checkdot = document.getElementById('price').value;
        var i = 0;
        for (i = 0; i < checkdot.length; i++) {
            if (checkdot[i] == '.') return false;
        }
        if (checkdot.length == 0) document.getElementById('price').value = '0';
        return true;
    }
    keychar = String.fromCharCode(keynum);

    return !isNaN(keychar);
}

function execute(urlmetodo, parametros) {
     
    var rsp;
    $.ajax({
        url: urlmetodo,
        type: "POST",
        data: JSON.stringify(parametros),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        cancel: false,
        success: function (response) {
            rsp = response;
        },
        failure: function (msg) {
            showdialog('a', 'Aviso', 'Se ha encontrado un problema en el sistema refresque la página por favor', false, 160, 300);
            rsp = msg;

        },
        error: function (xhr, status, error) {
            showdialog('a', 'Aviso', error, false, 160, 300);
            //alert(error);
            rsp = error;

        }
    });

    return rsp;
}

function set_dropdownlist(url, prm, obj) {
    var rspta = execute(url, prm);
    var ubigeo = $.parseJSON(rspta.d);

    obj.empty();
    obj.append($('<option></option>').val('00').html('- SELECCIONAR -'));
    for (var i = 0; i < ubigeo.length; i++) {
        var Ubigeo = ubigeo[i];
        obj.append($('<option></option>').val(Ubigeo.ValueField).html(Ubigeo.TextField));
    }
}

var div1_top = parseFloat("290");
var div1_left = parseFloat("27");

var divOCR1_top = parseFloat("266");
var divOCR1_left = parseFloat("27");

var divCodigoPdf_top = parseFloat("203");
var divCodigoPdf_left = parseFloat("172");

var divFirma_top = parseFloat("210");
var divFirma_left = parseFloat("50");

var divExpirationDate_top = parseFloat("170");
var divExpirationDate_left = parseFloat("313");

var divIssueDate_top = parseFloat("170");
var divIssueDate_left = parseFloat("145");

var divBirthdate_top = parseFloat("144");
var divBirthdate_left = parseFloat("313");

var divPlaceofBirth_top = parseFloat("144");
var divPlaceofBirth_left = parseFloat("145");

var divNumdoc1_top = parseFloat("117");
var divNumdoc1_left = parseFloat("313");

var divNacionalidad_top = parseFloat("117");
var divNacionalidad_left = parseFloat("145");

var div2_top = parseFloat("92");
var div2_left = parseFloat("413");

var divFirstName_top = parseFloat("92");
var divFirstName_left = parseFloat("145");

var divLastName_top = parseFloat("68");
var divLastName_left = parseFloat("145");

var divFoto2_top = parseFloat("40");
var divFoto2_left = parseFloat("365");

var capaImagen_top = parseFloat("0");
var capaImagen_left = parseFloat("0");

var divFoto_top = parseFloat("50");
var divFoto_left = parseFloat("31");

var divDocumentNo_top = parseFloat("42");
var divDocumentNo_left = parseFloat("145");


function limitar(e, contenido, caracteres) {
    // obtenemos la tecla pulsada
    var unicode = e.keyCode ? e.keyCode : e.charCode;

    // Permitimos las siguientes teclas:
    // 8 backspace
    // 46 suprimir
    // 13 enter
    // 9 tabulador
    // 37 izquierda
    // 39 derecha
    // 38 subir
    // 40 bajar
    if (unicode == 8 || unicode == 46 || unicode == 13 || unicode == 9 || unicode == 37 || unicode == 39 || unicode == 38 || unicode == 40)
        return true;

    // Si ha superado el limite de caracteres devolvemos false
    if (contenido.length >= caracteres)
        return false;

    return true;
}
