using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;

namespace SGAC.WebApp.Contabilidad
{
    public partial class CuentaRendicion : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            if (!Page.IsPostBack)
            {
                DateTime dt = DateTime.Now;
                lblFechaActual.Text = "A la fecha: " + dt.ToString(System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString()) + ".";
            }
        }
    }
}