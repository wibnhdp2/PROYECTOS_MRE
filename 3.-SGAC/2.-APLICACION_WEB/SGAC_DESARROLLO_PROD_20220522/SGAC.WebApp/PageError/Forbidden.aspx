<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forbidden.aspx.cs" Inherits="SGAC.WebApp.PageError.Forbidden" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SGAC</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" width="100%" cellpadding="0" id="Table2">
            <tr>
                <td style="background-image: url(Images/header.png); width: 333px; padding-left: 8px">
                    <img src="../Images/header.png" title="Header" alt="Fondo" />
                </td>
            </tr>
        </table>
        <div style="padding-left: 40px; margin-top: 10px">
            <table id="Table5" cellspacing="0" cellpadding="5" border="0">
                <tr>
                    <td valign="top" rowspan="3">
                        <img src="../Images/img_msg_error.png" title="Error" alt="Logo" />
                    </td>
                    <td style="font-weight: bold; font-size: 18px; border-bottom: gainsboro 1px solid">
                        403.-Error prohibido. Se produjo un error al recuperar la informacion del servidor. Intentelo nuevamente.
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <a href="javascript:window.opener.document.location.reload();self.close()">Ir a la pagina principal </a>
                        <%--<a href="../Default.aspx">Ir a la pagina principal </a>--%>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>