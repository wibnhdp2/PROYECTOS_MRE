using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SGAC.WebApp
{
    /// <summary>
    /// Summary description for LoadImagen
    /// </summary>
    public class LoadImagen : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            if (context.Request.QueryString["vClass"].ToString().Equals("imagen-no-disponible.jpg"))
                uploadPath = context.Server.MapPath("~/Images/");

            if (context.Request.QueryString["img"] != null)
                uploadPath = context.Server.MapPath("~/Images/");

            var s_RutaImagen = uploadPath +  context.Request.QueryString["vClass"].ToString();

            context.Response.ContentType = "image/jpg";
            if (System.IO.Path.GetExtension(s_RutaImagen).ToUpper() != ".PDF")
            {
                context.Response.TransmitFile(s_RutaImagen);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}