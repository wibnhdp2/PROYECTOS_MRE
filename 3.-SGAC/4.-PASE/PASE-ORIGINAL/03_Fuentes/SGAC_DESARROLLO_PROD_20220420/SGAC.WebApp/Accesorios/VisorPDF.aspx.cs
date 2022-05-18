using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Accesorios
{
    public partial class VisorPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //-------------------------------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha:30/09/2016
            //Objetivo: Incluir la validación de la sesión: Session["binaryData"]
            //-------------------------------------------------------------------------
            if (HttpContext.Current.Session["binaryData"] != null)
            {
            Response.ContentType = "Application/pdf";
            Response.AddHeader("Content-Disposition", "inline;filename=Documento.pdf");
            Response.BinaryWrite((byte[])HttpContext.Current.Session["binaryData"]);
            Session["binaryData"] = null;
            }
           // Response.End();
        }
    }
}