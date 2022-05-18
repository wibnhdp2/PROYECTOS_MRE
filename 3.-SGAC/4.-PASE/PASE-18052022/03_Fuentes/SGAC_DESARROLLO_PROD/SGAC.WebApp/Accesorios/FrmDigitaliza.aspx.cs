using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Accesorios
{
    public partial class FrmDigitaliza : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label7.Text = Convert.ToString(Session["SOLICITANTE"]);
            Label9.Text = Convert.ToString(Session["TARIFA_SEL"]);
            Label1.Text = Convert.ToString(Session["ACTDET_RGE"]);
            
            DateTime lAhora = DateTime.Today;
            txtFechaRecepcion.Text = lAhora.ToString("MM-dd-yyyy");
        }

        protected void Button9_Click(object sender, EventArgs e)
        {

        }
    }
}