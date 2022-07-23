<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerImagenSave.aspx.cs" Inherits="SolCARDIP.Paginas.VerImagenSave" %>
<%@ Import Namespace="System.IO" %>
<%       
    if (Session["tempImagenFotografiaSave"] != null) // CARGA EL ARCHIVO PDF ALMACENADO EN MEMORIA EN LA SESSION INDICADA
    {
        string base64String = (string)Session["tempImagenFotografiaSave"];
        byte[] imgByte = Convert.FromBase64String(base64String);
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = true;
        Response.ContentType = "image/jpeg;base64";
        Response.BinaryWrite(imgByte);
        Response.End();
    }
    else
    {
        Byte[] imageByte = null;
        imageByte = File.ReadAllBytes(Server.MapPath("../Imagenes/notFound.jpg"));
        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
        byte[] imgByte = Convert.FromBase64String(base64String);
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = true;
        Response.ContentType = "image/jpeg;base64";
        Response.BinaryWrite(imgByte);
        Response.End();
    }
 %>
 <html>
    <head>
        <base href="VerImagenSave.aspx" target="_self" />
    </head>
 </html>