<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas.PDF" %>
<%
    string parametro = Request.QueryString["imagen"];

    if (parametro == "denuncia")
    {
        if (Session["tempDenunciaSave"] != null)
        {
            string base64String = (string)Session["tempDenunciaSave"];
            byte[] byteImagePDF = Convert.FromBase64String(base64String);
            Response.Clear();
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteImagePDF);
            Response.End();

        }
    }
    else {
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
    }
    
 %>
 <html>
    <head>
        <base href="PDF.aspx" target="_self" />
    </head>
 </html>
