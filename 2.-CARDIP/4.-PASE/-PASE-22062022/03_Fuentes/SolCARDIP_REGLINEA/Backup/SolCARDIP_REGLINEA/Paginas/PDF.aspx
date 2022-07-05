<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas.PDF" %>
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
        
    }
 %>
 <html>
    <head>
        <base href="PDF.aspx" target="_self" />
    </head>
 </html>
