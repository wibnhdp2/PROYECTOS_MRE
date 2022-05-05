<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRepAnotacion.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmRepAnotacion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

     <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
 
 <style type="text/css">
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
 
    <!--[if IE]> <link rel="stylesheet" type="text/css" href="ie.css"> <![endif]-->
    <link href="../Styles/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/style.css" rel="stylesheet" type="text/css" />
    <%--<script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>--%>
    <script src="../Scripts/Validacion/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    

    <script type="text/javascript">

        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "FrmRepAnotacion.aspx/Imprimir",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#DivImprimirAnotacion").html(data.d);
                    return false;

                },
                failure: function (response) {

                    alert(response.d);
                }
            });
            $("#<%= btnImprimirAnotacion.ClientID %>").live('click', function () {
                var headstr = "<html><head>  <meta http-equiv=Content-type content=text/html charset=UTF-8> </head><body>";

                var footstr = "</body></html>";
                var newstr = $("#DivImprimirAnotacion").html();
                var oldstr = document.body.innerHTML;
                document.body.innerHTML = headstr + newstr + footstr;
                window.print();
                document.body.innerHTML = oldstr;
                return false;
            });




        });
    </script>
</head>
<body>


    <form id="form1" runat="server">
        
       
            <asp:Button ID="btnImprimirAnotacion" class="btnPrint" runat="server" Text="Imprimir"  Width="112px" Height="24px" />
            <div id="DivImprimirAnotacion" style="width:710px"></div>
         
    </form>

</body>
</html>
