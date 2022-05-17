<%@ Page Language="C#"  AutoEventWireup="true"  CodeFile="FrmCapturaImagen.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmCapturaImagen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
 
    <head id="Head1" runat="server">
     
        <title>Capture Image</title>
   
        <script src='<%=ResolveUrl("~/Scripts/jquery.min.js") %>' type="text/javascript"></script>
        <script src='<%=ResolveUrl("~/Scripts/jquery.webcam.js") %>' type="text/javascript"></script>

        <script type="text/javascript">

            var pageUrl = '<%=ResolveUrl("../Registro/FrmCapturaImagen.aspx") %>';
            $(function () {
                jQuery("#webcam").webcam({
                    width: 320,
                    height: 240,
                    mode: "save",
                    swffile: '<%=ResolveUrl("~/WebcamResources/jscam.swf") %>',
                    debug: function (type, status) {
                        $('#camStatus').append(type + ": " + status + '<br /><br />');
                    },
                    onSave: function (data) {
                        $.ajax({
                            type: "POST",
                            url: pageUrl + "/GetCapturedImage",
                            data: '',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $("[id*=imgCapture]").css("visibility", "visible");
                                $("[id*=imgCapture]").attr("src", data.d);
                                window.opener.$("[id*='imgPersona']").attr("src", data.d);

                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        });
                    },
                    onCapture: function () {
                        webcam.save(pageUrl);
                    }
                });
            });

            function Capture() {
                webcam.capture();
                return false;
            }

            function tomarfoto() {
                alert('hola');
            }

            function ImagenSelected(campo, campo2) {

                window.close();
            } 

        </script>

    </head>

    <body>

        <form id="form1" runat="server">

            <table border="0" cellpadding="0" cellspacing="0">

                <tr>
                    <td align="center">
                        <u>&nbsp;Camara web</u>
                    </td>
                    <td>
                    </td>
                    <td align="center">
                        Foto Capturada</td>
                </tr>

                <tr>
                    <td>
                        <div id="webcam">
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                       <asp:Image ID="imgCapture" runat="server" Style="visibility: hidden; width: 320px;
                            height: 240px" />
                    </td>
                </tr>

            </table>

            <br />

            <asp:Button ID="btnCapture" Text="Capturar" runat="server" 
                OnClientClick="return Capture();" onclick="btnCapture_Click" />

            <asp:Button ID="BtnAceptar" runat="server" onclick="BtnAceptar_Click" 
                 Text="Aceptar" />

        </form>

    </body>


</html>
