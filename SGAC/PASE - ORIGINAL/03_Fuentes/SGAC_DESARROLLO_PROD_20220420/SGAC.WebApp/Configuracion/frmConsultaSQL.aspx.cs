using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Configuracion.Maestro.BL;
using System.Data;
namespace SGAC.WebApp.Configuracion
{
    public partial class frmConsultaSQL : System.Web.UI.Page
    {
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            DataTable _dt = new DataTable();
            TablaMaestraConsultaBL obj = new TablaMaestraConsultaBL();
            _dt = obj.ConsultarSQL(txtConsultaSQL.Text);

            GridView1.DataSource = _dt;
            GridView1.DataBind();
        }
    }
}