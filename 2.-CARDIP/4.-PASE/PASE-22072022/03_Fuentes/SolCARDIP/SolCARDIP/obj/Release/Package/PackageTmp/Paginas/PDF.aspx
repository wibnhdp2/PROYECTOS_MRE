<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="SolCARDIP.Paginas.PDF" %>

<%
    if (Session["bytePDF"] != null) // CARGA EL ARCHIVO PDF ALMACENADO EN MEMORIA EN LA SESSION INDICADA
    {
        byte[] byteImagePDF = (byte[])Session["bytePDF"];
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(byteImagePDF);
        Response.End();
    }
 %>
 <html>
    <head>
        <base href="PDF.aspx" target="_self" />
    </head>
 </html>
