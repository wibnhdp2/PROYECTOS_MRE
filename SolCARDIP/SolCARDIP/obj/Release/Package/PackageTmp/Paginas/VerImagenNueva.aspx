<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerImagenNueva.aspx.cs" Inherits="SolCARDIP.Paginas.VerImagen" %>
<%@ Import Namespace="System.IO" %>
<%       
    if (Session["tempImagenFotografiaNueva"] != null) // CARGA EL ARCHIVO PDF ALMACENADO EN MEMORIA EN LA SESSION INDICADA
    {
        string base64String = (string)Session["tempImagenFotografiaNueva"];
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
        <base href="VerImagenNueva.aspx" target="_self" />
    </head>
 </html>