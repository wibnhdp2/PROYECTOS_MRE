<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mensajes.aspx.cs" Inherits="SolCARDIP.mensajes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="PRAGMA" content="NO-CACHE"/>
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate"/>
        <meta http-equiv="Expires" content="0"/>
        <link rel="SHORTCUT ICON" href="Images/Iconos/escudo.ico" />
        <title>Carne para Diplomaticos</title>
        <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
        <link rel="Shortcut Icon" href="../../Imagenes/Logos/EVD.jpg" />
        <style type="text/css">
        .btnGenerico
        {
            -moz-box-shadow: inset 0px 0px 2px 0px #ffffff;
            -webkit-box-shadow: inset 0px 0px 2px 0px #ffffff;
            box-shadow: inset 0px 0px 2px 0px #ffffff;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e82728', endColorstr='#aa0c0d');
            -webkit-border-top-left-radius: 5px;
            -moz-border-radius-topleft: 5px;
            border-top-left-radius: 5px;
            -webkit-border-top-right-radius: 5px;
            -moz-border-radius-topright: 5px;
            border-top-right-radius: 5px;
            -webkit-border-bottom-right-radius: 5px;
            -moz-border-radius-bottomright: 5px;
            border-bottom-right-radius: 5px;
            -webkit-border-bottom-left-radius: 5px;
            -moz-border-radius-bottomleft: 5px;
            border-bottom-left-radius: 5px;
            text-indent: 0;
            width of image>px;
            color: white;
            border: 1px outset #db1e1c;
            background-color: #cc1819;
            height: 26px;
        }        
        </style>
</head>
<body>
    <form id="form1" runat="server">
            <table style="width:100%;height:100%;" align="center">
                <tr>
                    <td align="center">
                        <div style="position:absolute;top:30%;left:35%">
                            <table style="border-style:solid;border-width:2px;border-color:Black;">
                                <tr>
                                    <td rowspan="4" style="border-style:solid;border-width:1px;border-color:Black;">
                                        <img alt="logo" src="Imagenes/Logos/CARDIP.jpg" width="120" height="110" />
                                    </td>
                                    <td style="border-style:solid;border-width:1px;border-color:Black;">
                                        <table>
                                            <tr><td></td></tr>
                                            <tr>
                                                <td align="center"><asp:Label ID="Label2"  runat="server" Font-Names="Verdana" Font-Bold="true" Font-Size="18px" Text="Sistema de Emisión de Carnés para Extranjeros en el Perú"></asp:Label>  </td>
                                            </tr>
                                            <tr>
                                                <td align="center"><asp:Label ID="lblmensaje" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="13px" Font-Names="Verdana" Text="Label" CssClass="etiquetaTitulo"></asp:Label></td>
                                            </tr>
                                            <tr><td style="font-family:Verdana;font-size:11px;font-weight:bold;">Se produjo un error al recuperar la INFORMACIÓN del servidor, comuniquese con el administrador.</td></tr>
                                            <tr><td style="font-family:Verdana;font-size:11px;font-weight:bold;">Salga de esta ventana e inicie sesion nuevamente.</td></tr>
                                            <tr><td></td></tr>
                                            <tr>
                                                <td align="center">
                                                    <input type="hidden" id="urlLogin" runat="server" />
                                                    <asp:Button ID="btnSalir" runat="server" Text="Iniciar Sesion" 
                                                    CssClass="btnGenerico" OnClientClick="goPage(); return false;"  
                                                    Width="105px" />
                                                </td>
                                            </tr>
                                            <tr><td></td></tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        <script type="text/javascript">
            function goPage() {
                var hd1 = document.getElementById("urlLogin");
                if (hd1 != null) {
                    var url = hd1.value;
                    window.location = url;
                }
            }
        </script>
    </form>
</body>
</html>
