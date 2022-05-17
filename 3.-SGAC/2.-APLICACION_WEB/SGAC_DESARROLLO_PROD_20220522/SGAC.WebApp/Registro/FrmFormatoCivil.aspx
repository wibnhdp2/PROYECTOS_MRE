<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmFormatoCivil.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmFormatoCivil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
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
        
       
      #cuerpo
       {
        width:770px;
        height:1096px;   
        }
    </style>
        
   
    <link href="../Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Validacion/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>

   <script type="text/javascript">

       $(document).ready(function () {

           var str_HF_REFISTROCIVIL = $("#<%= HF_REFISTROCIVIL.ClientID %>").val();
           var str_HF_ACTUDETALLE = $("#<%= HF_ACTUDETALLE.ClientID %>").val();
           var str_HF_TIPOACTA = $("#<%= HF_TIPOACTA.ClientID %>").val();

           prm = {};

           prm.HF_REFISTROCIVIL = str_HF_REFISTROCIVIL;
           prm.HF_ACTUDETALLE = str_HF_ACTUDETALLE;
           prm.intTipoActa = str_HF_TIPOACTA;

           $.ajax({
               type: "POST",
               url: "FrmFormatoCivil.aspx/Imprimir",
               contentType: "application/json; charset=utf-8",
               data: JSON.stringify(prm),
               dataType: "json",
               success: function (data) {
                   if (data.d == true) {
                       var strUrl = "../Accesorios/VisorPDF.aspx";
                       window.open('../Accesorios/VisorPDF.aspx', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');
                   }

               },
               failure: function (response) {

                   alert(response.d);
               }
           });


       });
   </script>
</head>
<body>
     
     

     <form id="form1" runat="server">
     
     

 
                <asp:HiddenField ID="HF_REFISTROCIVIL" runat="server" Value="0" />
                <asp:HiddenField ID="HF_ACTUDETALLE" runat="server" Value="0" />
                <asp:HiddenField ID="HF_TIPOACTA" runat="server" Value="0" />
                <button type="button" id="BtnImprimir" class="btn btn-xs btnPrint" class="hide" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Imprimir</button> 
     <%--           <asp:Button ID="BtnPrint" runat="server" CssClass="btnPrint" 
                    Text="         Imprimir" onclick="BtnPrint_Click"  />--%>
                <div id="cuerpo" style="text-align:justify;margin:5px;padding:5px; ">
 
                  
                </div>   
                

                

 
     </form>
</body>
</html>
