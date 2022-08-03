using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolCARDIP_REGLINEA.Paginas
{
    public partial class mensajes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Expires = -1;
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.RemoveAll();
            Session.Contents.RemoveAll();
            Session["SessionGeneral"] = null;
            Session["Generales"] = null;
        }
    }
}