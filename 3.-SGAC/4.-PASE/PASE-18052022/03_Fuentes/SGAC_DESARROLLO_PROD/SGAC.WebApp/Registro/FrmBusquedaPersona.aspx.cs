using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Registro
{
    public partial class FrmRegistroUnicoSearch : MyBasePage
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //clrlBusqueda.Direcciona = Enumerador.enmBusquedaDirecciona.RUNE;
                    //clrlBusqueda.TipoPersona = Enumerador.enmTipoPersona.NATURAL;
                    //if (Session["BUSQUEDA_TIPO_PERSONA"] != null)
                    //{
                    //    if (Convert.ToInt32(Session["BUSQUEDA_TIPO_PERSONA"]) == (int)Enumerador.enmTipoPersona.JURIDICA)
                    //        clrlBusqueda.TipoPersona = Enumerador.enmTipoPersona.JURIDICA;                        
                    //}
                    //Session.Remove("BUSQUEDA_TIPO_PERSONA");
                    ctrlBusqueda.Direcciona = Enumerador.enmBusquedaDirecciona.RUNE;
                    ctrlBusqueda.TipoPersona = Enumerador.enmTipoPersona.NATURAL;
                    if (Session["BUSQUEDA_TIPO_PERSONA"] != null)
                    {
                        if (Convert.ToInt32(Session["BUSQUEDA_TIPO_PERSONA"]) == (int)Enumerador.enmTipoPersona.JURIDICA)
                            ctrlBusqueda.TipoPersona = Enumerador.enmTipoPersona.JURIDICA;
                    }
                    Session.Remove("BUSQUEDA_TIPO_PERSONA");
                }

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }        
        #endregion
    }
}
