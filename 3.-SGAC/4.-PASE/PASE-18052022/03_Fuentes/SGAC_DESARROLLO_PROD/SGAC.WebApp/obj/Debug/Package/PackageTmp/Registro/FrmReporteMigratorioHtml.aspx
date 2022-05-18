<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmReporteMigratorioHtml.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmReporteMigratorioHtml" %>

<html>
<head runat="server">

    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>

    

    <% if (HttpContext.Current.Session["iTipo_Reporte"].ToString() == "DGC_005_VISA_LAMINA") %>
    <% { %>
    <link href="../Styles/spsam.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/svisas.css" rel="stylesheet" type="text/css" />
    <% } %>
    <script type="text/javascript">
        
        var top_margin2 = '<%=ConfigurationManager.AppSettings["Pasaporte_Top"].ToString() %>'; 
        var left_margin2 = '<%=ConfigurationManager.AppSettings["Pasaporte_Left"].ToString() %>';
        

        var top_margin = parseFloat(top_margin2);
        var left_margin = parseFloat(left_margin2);

        
//        var ua = window.navigator.userAgent;
//        var msie = ua.indexOf("MSIE ");


//        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
//        {
//            
//            $(".container").css("font-size", (parseInt(FontSize) + 7) + "px");
//           
//            left_margin  = left_margin + 35;
//            top_margin = top_margin + 2;

//             
//        }

       
        $(document).ready(function () {
            var str_GUID = $("#<%= HFGUID.ClientID %>").val();
            var prm = {};
            prm.strGUID = str_GUID;

            $.ajax({
                type: "POST",
                url: "FrmReporteMigratorioHtml.aspx/Imprimir_Lamina",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(prm),
                dataType: "json",
                success: function (msg) {
                    document.body.innerHTML = msg.d;
                },
                error: function (s_Error) {
                }

            });
        });
        function btn_Imprimir_onclick() {
            $("#btn_Imprimir").hide();
            
            <% if (HttpContext.Current.Session["Formato"].ToString() == "10") %>
            <% { %>
                
                $.ajax({
                    type: "POST",
                    url: "FrmReporteMigratorioHtml.aspx/Actualizar_Estado_Entregado",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if(msg.d=='1'){                            
                        }

                       


                    },
                    error: function (s_Error) {
                    }
                });
            <% }%>


            <% if (HttpContext.Current.Session["Formato"].ToString() == "1") %>
            <% { %>

            if(isNaN(top_margin)){
                top_margin = parseFloat("0");
            }

            if(isNaN(left_margin)){
                top_margin = parseFloat("0");
            }

            document.getElementById('divFoto').style.top = (divFoto_top + top_margin) + "px";
            document.getElementById('divFoto').style.left = (divFoto_left + left_margin) + "px";

            document.getElementById('divDocumentNo').style.top = (divDocumentNo_top + top_margin) + "px";
            document.getElementById('divDocumentNo').style.left = (divDocumentNo_left + left_margin) + "px";

            document.getElementById('divFoto2').style.top = (divFoto2_top + top_margin) + "px";
            document.getElementById('divFoto2').style.left = (divFoto2_left + left_margin) + "px";

            document.getElementById('divLastName').style.top = (divLastName_top + top_margin) + "px";
            document.getElementById('divLastName').style.left = (divLastName_left + left_margin) + "px";

            document.getElementById('divFirstName').style.top = (divFirstName_top + top_margin) + "px";
            document.getElementById('divFirstName').style.left = (divFirstName_left + left_margin) + "px";

            document.getElementById('div2').style.top = (div2_top + top_margin) + "px";
            document.getElementById('div2').style.left = (div2_left + left_margin) + "px";

            document.getElementById('divNacionalidad').style.top = (divNacionalidad_top + top_margin) + "px";
            document.getElementById('divNacionalidad').style.left = (divNacionalidad_left + left_margin) + "px";

            document.getElementById('divNumdoc1').style.top = (divNumdoc1_top + top_margin) + "px";
            document.getElementById('divNumdoc1').style.left = (divNumdoc1_left + left_margin) + "px";

            document.getElementById('divPlaceofBirth').style.top = (divPlaceofBirth_top + top_margin) + "px";
            document.getElementById('divPlaceofBirth').style.left = (divPlaceofBirth_left + left_margin) + "px";

            document.getElementById('divBirthdate').style.top = (divBirthdate_top + top_margin) + "px";
            document.getElementById('divBirthdate').style.left = (divBirthdate_left + left_margin) + "px";

            document.getElementById('divIssueDate').style.top = (divIssueDate_top + top_margin) + "px";
            document.getElementById('divIssueDate').style.left = (divIssueDate_left + left_margin) + "px";

            document.getElementById('divExpirationDate').style.top = (divExpirationDate_top + top_margin) + "px";
            document.getElementById('divExpirationDate').style.left = (divExpirationDate_left + left_margin) + "px";

            document.getElementById('divFirma').style.top = (divFirma_top + top_margin) + "px";
            document.getElementById('divFirma').style.left = (divFirma_left + left_margin) + "px";

            document.getElementById('divCodigoPdf').style.top = (divCodigoPdf_top + top_margin) + "px";
            document.getElementById('divCodigoPdf').style.left = (divCodigoPdf_left + left_margin) + "px";

            document.getElementById('divOCR1').style.top = (divOCR1_top + top_margin) + "px";
            document.getElementById('divOCR1').style.left = (divOCR1_left + left_margin) + "px";

            document.getElementById('div1').style.top = (div1_top + top_margin) + "px";
            document.getElementById('div1').style.left = (div1_left + left_margin) + "px";

            document.getElementById('capaImagen').style.top = (capaImagen_top + top_margin) + "px";
            document.getElementById('capaImagen').style.left = (capaImagen_left + left_margin) + "px";
//            document.getElementById('capaImagen').style.top = "15cm";
//            document.getElementById('capaImagen').style.left = "42mm";

            var str_GUID = $("#<%= HFGUID.ClientID %>").val();

            <% } %>

           
            window.print();
              

            $("#btn_Imprimir").show();
            var prm = {};
            prm.strGUID = str_GUID;

            $.ajax({
                                type: "POST",
                                url: "FrmReporteMigratorioHtml.aspx/ImpresionCorrecta",
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

            <% if (HttpContext.Current.Session["Formato"].ToString() == "10") %>
            <% { %>
                window.opener.location.reload();
            <% }%>

            

            <% if (HttpContext.Current.Session["Formato"].ToString() == "1") %>
            <% { %>
            document.getElementById('div1').style.top = div1_top +  "px";
            document.getElementById('div1').style.left = div1_left + "px";

            document.getElementById('divOCR1').style.top = divOCR1_top + "px";
            document.getElementById('divOCR1').style.left = divOCR1_left + "px";

            document.getElementById('divCodigoPdf').style.top = divCodigoPdf_top + "px";
            document.getElementById('divCodigoPdf').style.left = divCodigoPdf_left + "px";

            document.getElementById('divFirma').style.top = divFirma_top + "px";
            document.getElementById('divFirma').style.left = divFirma_left + "px";

            document.getElementById('divExpirationDate').style.top = divExpirationDate_top + "px";
            document.getElementById('divExpirationDate').style.left = divExpirationDate_left + "px";

            document.getElementById('divIssueDate').style.top = divIssueDate_top + "px";
            document.getElementById('divIssueDate').style.left = divIssueDate_left + "px";

            document.getElementById('divBirthdate').style.top = divBirthdate_top + "px";
            document.getElementById('divBirthdate').style.left = divBirthdate_left + "px";

            document.getElementById('divPlaceofBirth').style.top = divPlaceofBirth_top + "px";
            document.getElementById('divPlaceofBirth').style.left = divPlaceofBirth_left + "px";

            document.getElementById('divNumdoc1').style.top = divNumdoc1_top + "px";
            document.getElementById('divNumdoc1').style.left = divNumdoc1_left + "px";

            document.getElementById('divNacionalidad').style.top = divNacionalidad_top + "px";
            document.getElementById('divNacionalidad').style.left = divNacionalidad_left + "px";

            document.getElementById('div2').style.top = div2_top + "px";
            document.getElementById('div2').style.left = div2_left + "px";

            document.getElementById('divFirstName').style.top = divFirstName_top + "px";
            document.getElementById('divFirstName').style.left = divFirstName_left + "px";


            document.getElementById('divLastName').style.top = divLastName_top + "px";
            document.getElementById('divLastName').style.left = divLastName_left + "px";

            document.getElementById('divFoto2').style.top = divFoto2_top + "px";
            document.getElementById('divFoto2').style.left = divFoto2_left + "px";

            document.getElementById('capaImagen').style.top = capaImagen_top + "px";
            document.getElementById('capaImagen').style.left = capaImagen_left + "px";

            document.getElementById('divFoto').style.top = divFoto_top + "px";
            document.getElementById('divFoto').style.left = divFoto_left + "px";

            document.getElementById('divDocumentNo').style.top = divDocumentNo_top + "px";
            document.getElementById('divDocumentNo').style.left = divDocumentNo_left + "px";
            <% } %>


           
                          

            
        }

    </script>
     <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1 {
	        WIDTH: 18%
        }
        .style3 {
	        WIDTH: 15%
        }
    </style>
</head>
<body>
    <asp:HiddenField ID="HFGUID" runat="server" />

</body>
</html>
