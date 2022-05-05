using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Security; 
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Persona.BL;
using SGAC.Registro.Actuacion.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.Accesorios;
using System.Threading;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging; 
using System.Configuration;
using System.IO;
using SGAC.WebApp;
using SGAC.WebApp.Registro;
using System.Web.SessionState;
using Microsoft.Security.Application;

namespace SGAC.WebApp
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler, IRequiresSessionState
    {      
        public void ProcessRequest (HttpContext context) {  

            DataTable tbRegistro = new DataTable();
            PersonaFotoConsultaBL PersonaFotoConsultaBL = new PersonaFotoConsultaBL();

            if (context.Request.QueryString["GUID"] != null)
            {
                string strGUID = Sanitizer.GetSafeHtmlFragment(context.Request.QueryString["GUID"].ToString());
                tbRegistro = PersonaFotoConsultaBL.PersonaFotoGetFotoFirma(Convert.ToInt64(context.Session["iPersonaId" + strGUID]), 2551);
            }
            else
            {
                tbRegistro = PersonaFotoConsultaBL.PersonaFotoGetFotoFirma(Convert.ToInt64(context.Session["iPersonaId"]), 2551);
            }
            byte[] imagen = (byte[])(tbRegistro.Rows[0][0]);
            context.Response.ContentType = "image/jpg";
            context.Response.OutputStream.Write(imagen, 0, imagen.Length);
        }
     
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}