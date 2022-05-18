<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizarPDF.aspx.cs" Inherits="SGAC.WebApp.Accesorios.VisualizarPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
    if (Session["bytePDF"] != null)
    {
        byte[] byteImagePDF = (byte[])Session["bytePDF"];
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(byteImagePDF);
        Response.End();
        Session.Remove("bytePDF");
    }
 %>
    </div>
    </form>
</body>
</html>
