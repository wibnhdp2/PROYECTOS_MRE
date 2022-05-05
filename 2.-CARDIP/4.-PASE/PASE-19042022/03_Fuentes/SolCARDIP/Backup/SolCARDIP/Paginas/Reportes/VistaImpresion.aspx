<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VistaImpresion.aspx.cs" Inherits="SolCARDIP.Paginas.Reportes.VistaImpresion" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vista de Impresión</title>
    <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8; IE=9; IE=10;" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link rel="Shortcut Icon" href="../../Imagenes/Logos/CARDIP.jpg" />
    <script language="JavaScript" type="text/javascript" src="../../JavaScript/jquery/1.3.2/jquery.min.js"></script>
    <%--<script type="text/javascript">
        function seguridadURLPrevia() {
            if (document.referrer == "") {
                location.href = '../../mensajes.aspx';
            }
        }
        window.onload = seguridadURLPrevia;
    </script>--%>
    <script type="text/javascript">
        //BLOQUEA F5 Y BACKSPACE
        function checkKeyCode(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if (event.keyCode == 116) {
                evt.keyCode = 0;
                return false
            }
            if (event.keyCode == 8 && document.activeElement.type != "text") {
                if (event.keyCode == 8 && document.activeElement.type == "textarea") {
                    return true;
                }
                evt.keyCode = 0;
                return false;
            }
            //            if (event.keyCode == 13 && document.activeElement.type == "text") {
            //                evt.keyCode = 0;
            //                return false;
            //            }
            //            if (event.keyCode == 13 && document.activeElement.type == "checkbox") {
            //                evt.keyCode = 0;
            //                return false;
            //            }
        }
        document.onkeydown = checkKeyCode;
    </script>
     <script language="javascript" type="text/javascript">
         // Guardando la URL de la imagen del botón imprimir. Esta imagen es una copia de la imagen mostrada en el ReportViewer, guardada en la carpeta de imágenes de la aplicación web.
         var urlImg = '<%=Page.ResolveUrl("../../Imagenes/Iconos/img_16_print.png") %>';

         // Función que se ejecuta una vez se ha terminado de cargar el DOM de la página web en el navegador
         $(document).ready(function () {
             colocarBtnImprimir();    // Colocar el botón de imprimir en la barra de herramientas del ReportViewer
             $("#BtnImprimir").click(imprimirDiv);  // Asignando la función "imprimirDiv" al evento click del botón de impresión
         });

         // Esta función coloca el botón de imprimir en la barra de herramientas del ReportViewer
         function colocarBtnImprimir() {
             var jqoBarraRpt = $('div#ReportViewer1_ctl05>div:first-child');    // Buscando el div que contiene la barra de herramientas del RportViewer

             if (jqoBarraRpt && jqoBarraRpt.length > 0    // Verificando que el DIV barra de herramientas fue encontrado,
        && jqoBarraRpt.find('#BtnImprimir').length <= 0) {    // y verificando que el botón de imprimir no existe ya

                 // Colocando el botón de impresión, con una estructura similar a la que tiene el botón original en el ReportViewer
                 jqoBarraRpt.append('<table cellpadding="0" cellspacing="0" ToolbarSpacer="true" style="display:inline;width:10px;"><tr><td></td></tr></table><div style="display:inline;font-family:Verdana;font-size:8pt;vertical-align:top;"><table cellpadding="0" cellspacing="0" style="display:inline;"><tr><td height="28px"><div"><div id="BtnImprimir"><table title="Print"><tr><td><img title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;" /></td></tr></table></div><div disabled="disabled" style="display:none;border:1px transparent Solid;"><table title="Print"><tr><td><input type="image" disabled="disabled" title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;cursor:default;" /></td></tr></table></div></div></td></tr></table></div>');
             }
         }
         // Función que se encarga de imprimir el reporte
         function imprimirDiv() {
             var divImprimir = $("div[id$='ReportDiv']").parent();    // Obteniendo el DIV que contiene el reporte a imprimir
             var newWin = window.open();    // Abriendo una nueva ventana del navegador
             newWin.document.open();    // Abriendo el documento de la nueva ventana, para escribir su contenido
             newWin.document.write('<html><head><style type="text/css">' + getAllStyleSheetsAsText() + '</style></head><body>' + divImprimir.html() + '</body>');
             newWin.document.close();
             newWin.print();
             newWin.close();
         }
         function getAllStyleSheetsAsText() {
             var cssText = '';
             var sheets = document.styleSheets;
             for (var c = 0; c < sheets.length; c++) {
                 var sheet = sheets[c];
                 if ((sheet.ownerNode || sheet.owningElement).id.endsWith('_ReportControl_styles')) {
                     var rules = sheet.rules || sheet.cssRules;
                     for (var r = 0; r < rules.length; r++) {
                         var cssRule = rules[r];
                         if ($.browser.msie) {
                             var cssText = cssText + cssRule.selectorText + '{' + cssRule.style.cssText.toLowerCase() + '}';
                         } else {
                             var cssText = cssText + cssRule.cssText;
                         }
                     }
                 }
             }
             return cssText;
         }
    </script>
    <style type="text/css">
       #BtnImprimir {
            border: 1px solid transparent;
        }
        #BtnImprimir:hover {
            border: 1px solid rgb(51,102,153); 
            background-color: rgb(221,238,247);
            cursor:pointer;
        } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div1">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"  SizeToReportContent="True" 
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="987px" ShowPrintButton="false" >
                <LocalReport>
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet1" />
                    </DataSources>                                   
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server">       
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
