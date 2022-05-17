using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SGAC.Accesorios;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Registro
{
    public partial class FrmPreviewEP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strRutaArchivo = Convert.ToString(Sanitizer.GetSafeHtmlFragment(Request.QueryString["filename"].ToString()));
            

            if (Session["strTipoArchivo"].Equals(".pdf"))
            {
                if (strRutaArchivo.Length > 0)
                {
                    if (Session["strpathAnioMesDia"] != null)
                    {
                        string strpathAnioMesDia = Session["strpathAnioMesDia"].ToString();
                        string strRutaArchivoDestino = Path.Combine(strpathAnioMesDia, strRutaArchivo);

                        if (File.Exists(strRutaArchivoDestino))
                        {
                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(strRutaArchivoDestino);
                            Response.End();
                        }
                    }
                    //-------------------------------------------------------------------------                    
                }
            }            
        }
    }
}