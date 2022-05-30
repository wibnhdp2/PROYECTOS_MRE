function showdialogBootstrap(type_msg, title, msg, url) {
    var html = "";
    html += "<div class='modal fade' id='modal_Mensajes' tabindex='-1' role='dialog' aria-labelledby='modal_label' aria-hidden='true'>";
    html += "<div class='modal-dialog' style='width: 380px; margin: 13% auto;'>";
    html += "<div class='modal-content'>";
    html += "<div class='modal-body'>";
    html += "<fieldset style='width: 100%; margin: 0px;padding: 0px;'>";

    html += "<table id='TableModal'>";
    html += "<tr id='TableHeaderModal'><td colspan='3' class='text-center'><b>" + title + "</b></td></tr>";



    switch (type_msg) {
        case 'I':
            // Information
            html += "<tr><td><img class='img-responsive' src='" + url + "../Images/img_msg_info.png'> </td><td colspan='2' class='text-left'><b>" + msg + "</b></td></tr>";
            html += "<tr><td colspan='3' class='text-center'><button type='button' id='BtnAceptar' name='BtnAceptar' class='btn btn-primary btn-sm BtnAceptar' data-dismiss='modal'>Aceptar</button></td></tr></table>";
            html += "<br></fieldset></div></div></div></div>";
            break;
        case 'Q':
            // Error
            html += "<tr><td><img class='img-responsive' src='" + url + "../Images/img_msg_question.png'> </td><td colspan='2' class='text-left'><b>" + msg + "</b></td></tr>";
            html += "<tr><td colspan='3' class='text-center'><button type='button' id='BtnSi' name='BtnSi' class='btn btn-info btn-sm'>SI</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button type='button' id='BtnNo' name='BtnNo' class='btn btn-warning btn-sm'>NO</button></td></tr></table>";
            html += "<br></fieldset></div></div></div></div>";
            break;
        case 'W':
            // Warning
            html += "<tr><td><img class='img-responsive' src='" + url + "../Images/img_msg_warning.png'> </td><td colspan='2' class='text-left'><b>" + msg + "</b></td></tr>";
            html += "<tr><td colspan='3' class='text-center'><button type='button' id='BtnAceptar' name='BtnAceptar' class='btn btn-primary btn-sm BtnAceptar' data-dismiss='modal'>Aceptar</button></td></tr></table>";
            html += "<br></fieldset></div></div></div></div>";
            break;
        case 'E':
            // Error
            html += "<tr><td><img class='img-responsive' src='" + url + "../Images/img_msg_error.png'> </td><td colspan='2' class='text-left'><b>" + msg + "</b></td></tr>";
            html += "<tr><td colspan='3' class='text-center'><button type='button' id='BtnAceptar' name='BtnAceptar' class='btn btn-primary btn-sm BtnAceptar' data-dismiss='modal'>Aceptar</button></td></tr></table>";
            html += "<br></fieldset></div></div></div></div>";
            break;
    }
    $("#msg-dialog").html(html);
    $('#modal_Mensajes').modal({ backdrop: 'static', keyboard: true });
    $('#modal_Mensajes').modal('show');

}