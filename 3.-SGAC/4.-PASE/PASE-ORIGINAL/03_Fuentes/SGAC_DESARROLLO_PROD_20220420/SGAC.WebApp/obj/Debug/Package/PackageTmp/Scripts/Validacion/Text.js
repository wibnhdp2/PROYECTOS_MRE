/*Valida Solo Numeros*/
function ValidaSoloNumeros() {
    if ((event.keyCode < 48) || (event.keyCode > 57))
        event.returnValue = false;
}

/*Valida Solo Numero Decimales*/
function ValidaDecimal() {

        
    if ((event.keyCode < 48 || event.keyCode > 57) || event.keyCode == 46) {
            
        if (event.keyCode != 46) {
                    
            event.returnValue = false;
        }
        
    }

}

/*Validar Numero y Letras*/
function ValidaNumeroLetras() {
    if ((event.keyCode < 48) || (event.keyCode > 90)) {
        event.returnValue = false;
    }
   
}
var validos = " abcdefghijklmnopqrstuvwxyz0123456789";
function soloLetrasYNum(campo) {
    var letra;
    var bien = true;
    for (var i = 0; i < campo.value.length; i++) {
        letra = campo.value.charAt(i).toLowerCase()
        if (validos.indexOf(letra) == -1) { bien = false; };
    }
    if (!bien) {
        campo.focus();
    }
}

