<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDarBaja.aspx.cs" Inherits="SGAC.WebApp.Almacen.FrmDarBaja" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2-modal.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <table width="100%">
            <tr align="center">
            <td colspan="4">¿Está seguro de dar de Baja al Insumo?</td>
            </tr>
            <tr>
            <td colspan="4"><br /></td>
            </tr>            
            <tr>
            <td></td>
            <td align="center">
                <asp:Button ID="btnAceptar" runat="server" Text="Sí" cssClass="btnGeneral" 
                    width="50px" onclick="btnAceptar_Click" />
            </td>
            <td align="center">
                <asp:Button ID="btnCancelar" runat="server" Text="No" cssClass="btnGeneral" 
                    width="50px" onclick="btnCancelar_Click" />
            </td>
            <td></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
