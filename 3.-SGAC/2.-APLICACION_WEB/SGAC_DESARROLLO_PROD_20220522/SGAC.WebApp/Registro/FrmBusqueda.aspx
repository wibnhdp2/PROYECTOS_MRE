<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmBusqueda.aspx.cs" Inherits="SGAC.WebApp.Registro.FrmBusqueda" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Src="~/Accesorios/SharedControls/ctrlBusquedaPopup.ascx" TagPrefix="uc" TagName="ctrlBusqueda" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background:#FFFFFF;">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr align="center">
                <td><uc:ctrlBusqueda ID="clrlAdjunto" runat="server"></uc:ctrlBusqueda></td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>
