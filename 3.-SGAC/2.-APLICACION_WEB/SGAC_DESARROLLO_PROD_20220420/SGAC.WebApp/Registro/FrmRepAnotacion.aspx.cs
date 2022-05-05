using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Registro
{
    public partial class FrmRepAnotacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [System.Web.Services.WebMethod]
        public static String Imprimir()
        {
            try{

                String html = String.Empty;
                html = HttpContext.Current.Session["ANOTACION_DESC"].ToString();
                HttpContext.Current.Session.Remove("ANOTACION_DESC");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}