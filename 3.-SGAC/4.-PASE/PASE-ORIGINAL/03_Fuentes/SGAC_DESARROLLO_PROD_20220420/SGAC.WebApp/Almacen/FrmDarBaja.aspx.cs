using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Almacen
{
    public partial class FrmDarBaja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Session["baja_accion"] = 1;

            Comun.EjecutarScript(this, "window.parent.close_ModalPopup('MainContent_btnEjecutar');");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session["baja_accion"] = 0;

            Comun.EjecutarScript(this, "window.parent.close_ModalPopup('MainContent_btnEjecutar');");
        }
    }
}