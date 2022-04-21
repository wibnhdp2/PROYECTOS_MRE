<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mensajes.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas.mensajes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Estilos/StyleButton.css" rel="stylesheet" type="text/css" />
    <link href="../Estilos/MRE.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table style="width:100%;height:500px;" align="center">
            <tr>
                <td align="center">
                    <table style="border-style:solid;border-width:1px;border-color:Black;height:150px;">
                        <tr>
                            <td align="center" rowspan="4" style="border-style:solid;border-width:1px;border-color:Black;width:20%;">
                                <img alt="logo" src="../Imagenes/Iconos/alert48.png" width="48" height="48" />
                            </td>
                            <td style="border-style:solid;border-width:1px;border-color:Black;">
                                <table>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td align="center"><asp:Label ID="Label2"  runat="server" Font-Names="Trebuchet MS" Font-Bold="true" Font-Size="18px" Text="Sistema de Registro en Linea para Carnés de Identidad"></asp:Label>  </td>
                                    </tr>
                                    <tr><td style="font-family:Trebuchet MS;font-size:11px;font-weight:normal;">Se produjo un error al recuperar la informacion del servidor. Intentelo nuevamente</td></tr>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td align="center">
                                            <%--<button id="btnGoTo3" runat="server" class="buttonAnimated" style="vertical-align:middle;width:150px" onclick="goPage();"><span class="spanHover">Homepage</span></button>--%>
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
