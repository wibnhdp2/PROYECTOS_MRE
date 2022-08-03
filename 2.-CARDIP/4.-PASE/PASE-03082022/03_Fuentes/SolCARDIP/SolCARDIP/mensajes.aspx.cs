using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolCARDIP.Librerias.ReglasNegocio;

namespace SolCARDIP
{
    public partial class mensajes : System.Web.UI.Page
    {
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        brGeneral obrGeneral = new brGeneral();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string strMensaje = "";
                    if (Session["mensaje"] != null)
                    {
                        strMensaje = Session["mensaje"].ToString();
                    }
                    else
                    {
                        strMensaje = "Acceso restringido";
                    }
                    lblmensaje.Text = strMensaje;
                    urlLogin.Value = obrGeneral.urlSistema;
                    Session.Abandon();
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Expires = -1;
                    Response.Cache.SetNoStore();
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Contents.RemoveAll();
                    Session["usuario"] = null;
                }
            }
            catch (ArgumentNullException ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
    }
}