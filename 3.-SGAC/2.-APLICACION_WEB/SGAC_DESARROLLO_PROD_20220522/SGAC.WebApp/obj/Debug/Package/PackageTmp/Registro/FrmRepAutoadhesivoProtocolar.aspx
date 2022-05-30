<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRepAutoadhesivoProtocolar.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRepAutoadhesivoProtocolar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title></title>

    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10" />

    <link href="../Styles/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/showLoading.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/Validacion/jquery.showLoading.js" type="text/javascript"></script>
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
        
    
    table tr 
    {
        padding:0px;
        margin:0px;    
    }
    </style>
 

<script type="text/javascript">

    $(document).ready(function () {
    
        var str_titulo = $("#HDN_TITULO").val();
        $("#titulo").html(str_titulo);

        
        var html = '';
        var Paginastotal = 1;

       
                CargarAutoAdhesivo("1");


          $("#<%= btnImprimirAutoAdhesivo.ClientID %>").on('click', function () {

//                    var ua = window.navigator.userAgent;
//                    var msie = ua.indexOf("MSIE ");

//                    var MarginLeft = $("#<%= hdnMarginLeft.ClientID %>").val();
//                    var MarginTop = $("#<%= hdnMarginTop.ClientID %>").val();
//                    var FontSize = $("#<%= hdnFontSize.ClientID %>").val();

//                    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
//                    {
//                        $(".container").css("font-size", "");
//                        $(".container").css("margin-left", "");
//                        $(".container").css("margin-top", "");

//                        $(".container").css("font-size", (parseInt(FontSize) + 7) + "px");
//                        $(".container").css("margin-left", (parseInt(MarginLeft) + 35) + "px");
//                        $(".container").css("margin-top", (parseInt(MarginTop) + 2) + "px");


//                        $(".img-cod-bar").css("width", "");
//                        $(".img-cod-bar").css("height", "");

//                        $(".img-cod-bar").css("width", "340px");
//                        $(".img-cod-bar").css("height", "80px");

//                    }

            var headstr = "<html><head></head><body>";
            var footstr = "</body></html>";
            var newstr = $("#DivImprimirAutoAdhesivos_").html();
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            var prm = {};
            prm.strGUID = str_GUID;

            $.ajax({
                    type: "POST",
                    url: "FrmRepAutoadhesivoProtocolar.aspx/ImpresionCorrecta",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(prm),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "OK") {
                            cerrarVentana();
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
             
            location.reload();


            return false;
        });

        function CargarAutoAdhesivo(parametro) {
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            prm = {};
            prm.para_sParametro = parametro;
            prm.strGUID = str_GUID;
            $.ajax({
                type: "POST",
                url: "FrmRepAutoadhesivoProtocolar.aspx/Imprimir",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(prm),
                dataType: "json",
                async: false,
                cancel: false,
                success: function (data) {
                    $("#DivImprimirAutoAdhesivos_").html(data.d);
                    return false;

                },
                failure: function (response) {

                    alert(response.d);
                }
            });
        }
    });
    </script>

</head>
<body>
  
    <form id="form1" runat="server">
    
        <asp:HiddenField ID="HDN_TITULO" runat="server" Value="0" />            
        <asp:HiddenField ID="hdnFontSize" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarginTop" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarginLeft" runat="server" Value="0" />
             <asp:HiddenField ID="HFGUID" runat="server" />
        <table>
            <tr>
                <td>
                <asp:Button ID="btnImprimirAutoAdhesivo" class="btnPrint" runat="server" Text="Imprimir"  Width="112px" Height="24px" />
                </td>
            </tr>
            <tr>
                <td>
                <label id="Label1" class="titulo">AUTOADHESIVO</label> 
                </td>
            </tr>
        </table>
                      
             


         <div style="margin: 0; padding:0;" id="DivImprimirAutoAdhesivos" >
                <div style="margin-left:70px; margin-top:70px;" id="DivImprimirAutoAdhesivos_">
                
                </div>
         </div>

    </form>
</body>
</html>
