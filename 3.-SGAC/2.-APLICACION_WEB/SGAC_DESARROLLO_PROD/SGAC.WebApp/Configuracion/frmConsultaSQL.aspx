<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmConsultaSQL.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmConsultaSQL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <link href="../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>Escriba su consulta</h2>
        <asp:TextBox ID="txtConsultaSQL" TextMode="MultiLine" runat="server" Height="168px" 
            Width="435px"></asp:TextBox>
            <br />
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" 
            onclick="btnConsultar_Click" />
    
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
