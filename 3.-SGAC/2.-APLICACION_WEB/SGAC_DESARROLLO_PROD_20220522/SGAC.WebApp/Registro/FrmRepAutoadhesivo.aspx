<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRepAutoadhesivo.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRepAutoadesivo" %>

 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10" />
    <title></title>
 
 
    <link href="../Styles/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/style.css" rel="stylesheet" type="text/css" />

  

    <%--<script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>--%>
    <script src="../Scripts/Validacion/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
   

      <style type="text/css">
     
   #DivImprimirAutoAdhesivos
    {
        background-image: url("../Images/Autoadhesivo.jpg");    
        background-repeat: no-repeat;
        background-position: 50% 0%;
        -webkit-background-size: 100% 100%;           /* Safari 3.0 */
        -moz-background-size: 100% 100%;           /* Gecko 1.9.2 (Firefox 3.6) */
        -o-background-size: 100% 100%;           /* Opera 9.5 */
        background-size: 100% 100%;
        width:90%;
        height:500px;
        float:left;
        margin: 0;
        padding: 0;
        
    }
    
    body
    {
        padding: 0;
        margin: 0;
        text-align: left;
        float: left;
       
    }
    
    div
    {
          
         padding:0px;
         margin:0px;    
   }
    
    
.btnPrint
{
	-moz-box-shadow:inset 0px 0px 2px 0px #ffffff;
	-webkit-box-shadow:inset 0px 0px 2px 0px #ffffff;
	box-shadow:inset 0px 0px 2px 0px #ffffff;
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background-color:#ededed;
	-webkit-border-top-left-radius:5px;
	-moz-border-radius-topleft:5px;
	border-top-left-radius:5px;
	-webkit-border-top-right-radius:5px;
	-moz-border-radius-topright:5px;
	border-top-right-radius:5px;
	-webkit-border-bottom-right-radius:5px;
	-moz-border-radius-bottomright:5px;
	border-bottom-right-radius:5px;
	-webkit-border-bottom-left-radius:5px;
	-moz-border-radius-bottomleft:5px;
	border-bottom-left-radius:5px;
	text-indent:0;
	text-decoration:none;
	text-align:center;
	border:1px outset #dcdcdc;
	background-image: url('../Images/img_16_print.png');
	background-repeat: no-repeat;
	background-position: 8px 4px;
	margin-left: 63px;
	 
        }
        
    
 
   
     
    </style>
    <script type="text/javascript">

        $(document).ready(function () {


            funSetSession("vNavegadorActivo", navigator.userAgent.toLowerCase());
            var bIE = false;
            var str_IdActuacionDetalleId = $("#<%= HIdActuacionDetalleId.ClientID %>").val();
            var prm = {};
            prm.IdActuacionDetalleId = str_IdActuacionDetalleId;

            $.ajax({
                type: "POST",
                url: "FrmRepAutoadhesivo.aspx/Imprimir",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(prm),
                dataType: "json",
                success: function (data) {
                    $("#DivImprimirAutoAdhesivos_").html(data.d);
                    return false;

                },
                failure: function (response) {

                    alert(response.d);
                }
            });
            $("#<%= btnImprimirAutoAdhesivo.ClientID %>").live('click', function () {

                //                var ua = window.navigator.userAgent;
                //                var msie = ua.indexOf("MSIE ");

                //                var MarginLeft = $("#<%= hdnMarginLeft.ClientID %>").val();
                //                var MarginTop = $("#<%= hdnMarginTop.ClientID %>").val();
                //                var FontSize = $("#<%= hdnFontSize.ClientID %>").val();

                //                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
                //                {
                //                    $(".container").css("font-size", "");
                //                    $(".container").css("margin-left", "");
                //                    $(".container").css("margin-top", "");

                //                    $(".container").css("font-size", (parseInt(FontSize) + 7) + "px");
                //                    $(".container").css("margin-left", (parseInt(MarginLeft) + 35) + "px");
                //                    $(".container").css("margin-top", (parseInt(MarginTop) + 2) + "px");


                //                    $(".img-cod-bar").css("width", "");
                //                    $(".img-cod-bar").css("height", "");

                //                    $(".img-cod-bar").css("width", "340px");
                //                    $(".img-cod-bar").css("height", "80px");

                //                }


                var headstr = "<html><head></head><body>";
                var footstr = "</body></html>";
                var newstr = $("#DivImprimirAutoAdhesivos_").html();



                var oldstr = document.body.innerHTML;
                document.body.innerHTML = headstr + newstr + footstr;
                window.print();
                document.body.innerHTML = oldstr;
                var str_IdActuacionDetalleId = $("#<%= HIdActuacionDetalleId.ClientID %>").val();
                var str_IdActuacionExtraId = $("#<%= HIdActuacionExtra.ClientID %>").val();
                var str_IdActuacionId = $("#<%= HIdActuacion.ClientID %>").val();
                var prm = {};
                prm.IdActuacionDetalleId = str_IdActuacionDetalleId;
                prm.IdActuacionExtraId = str_IdActuacionExtraId;
                prm.IdActuacionId = str_IdActuacionId;
                $.ajax({
                    type: "POST",
                    url: "FrmRepAutoadhesivo.aspx/ImpresionCorrecta",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(prm),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "OK") {
                            //window.parent.cerrarVentana('MainContent_btnDesabilitarAutoahesivo');
                            cerrarVentana();
                            //window.close();
                        }
                        else {
                            alert(data.d);
                        }
                        return false;

                    },
                    failure: function (response) {

                        alert(response.d);
                    }
                });
                return false;
            });




        });


        /*FUNCIONES GENERALES*/
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
                    alert(msg);
                    rsp = msg;
                },
                error: function (xhr, status, error) {
                    alert(error);
                    rsp = error;
                }
            });

            return rsp;
        }

        function funSetSession(variable, valor) {
            var url = 'FrmRepAutoadhesivo.aspx/SetSession';
            var prm = {};
            prm.variable = variable;
            prm.valor = valor;
            var rspta = execute(url, prm);
        }

    </script>

</head>
<body>


    <form id="form1" runat="server">
        
       
            <asp:HiddenField ID="HF_TIPO_FORMATO" runat="server" Value="0" />
            <asp:HiddenField ID="HF_TIPO_ACTA" runat="server" Value="0" />
            <asp:HiddenField ID="HF_NRO_ACTA" runat="server" Value="0" />

            <asp:HiddenField ID="hdnFontSize" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarginTop" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarginLeft" runat="server" Value="0" />
             
             <asp:HiddenField ID="HFGUID" runat="server" />
             <asp:HiddenField ID="HIdActuacionDetalleId" runat="server" />
            <asp:HiddenField ID="HIdActuacionExtra" runat="server" />
             <asp:HiddenField ID="HIdActuacion" runat="server" />


            <asp:Button ID="btnImprimirAutoAdhesivo" class="btnPrint" runat="server" Text="Imprimir"  Width="112px" Height="24px" />
            <div style="margin: 0; padding:0;" id="DivImprimirAutoAdhesivos" >
                <div style="margin-left:70px; margin-top:70px;" id="DivImprimirAutoAdhesivos_">
                
                </div>
            </div>
           <%--<div style="font-size:30px; display:block; left:-20px; top: 170px; color:GrayText; -webkit-transform: rotate(-45deg); transform: rotate(-45deg); opacity=0.30; 
               position:absolute;">
                SOLO PARA CONSULTAS
           </div>--%>
         
    </form>
</body>
</html>

 