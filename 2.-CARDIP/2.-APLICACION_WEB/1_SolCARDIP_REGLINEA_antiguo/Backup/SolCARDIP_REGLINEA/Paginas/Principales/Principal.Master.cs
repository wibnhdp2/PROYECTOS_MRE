using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;
using System.Reflection;

namespace SolCARDIP_REGLINEA.Paginas.Principales
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        brGeneral obrGeneral = new brGeneral();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                hlActualizacion.Text = hlActualizacion.Text + obrGeneral.FechaUpdate + "    - Versión: " + ver.ToString();
            }
        }
    }
}