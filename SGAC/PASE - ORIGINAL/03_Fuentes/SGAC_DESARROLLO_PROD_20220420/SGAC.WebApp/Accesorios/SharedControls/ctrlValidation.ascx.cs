using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlValidation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            trMensaje.Visible = false;
        }

        public void MostrarValidacion(string strMensajeValidacion, bool bolMostrar = true, Enumerador.enmTipoMensaje enmMensaje = Enumerador.enmTipoMensaje.WARNING)
        {
            trMensaje.Visible = false;

            switch (enmMensaje)
            {
                case Enumerador.enmTipoMensaje.NONE:
                    imgIcono.ImageUrl = "~/Images/img_16_other.png";

                    break;

                case Enumerador.enmTipoMensaje.INFORMATION:
                    trMensaje.Attributes.Add("class", "trInformativo");
                    imgIcono.ImageUrl = "~/Images/img_16_success.png";

                    break; 
               
                case Enumerador.enmTipoMensaje.WARNING:
                    trMensaje.Attributes.Add("class", "trAdvertencia");
                    imgIcono.ImageUrl = "~/Images/img_16_warning.png";

                    break;

                case Enumerador.enmTipoMensaje.ERROR:
                    trMensaje.Attributes.Add("class", "trError");
                    imgIcono.ImageUrl = "~/Images/img_16_error.png";

                    break;

                default:

                    break;
            }

            lblEtiqueta.Text = strMensajeValidacion;
            trMensaje.Visible = bolMostrar;
        }
    }
}