using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActuacionSearch : MyBasePage
    {
        #region Eventos     
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Session["Nombre"] = null;
                    Session["flgModoBusquedaAct"] = null;
                    Session["ApePat"] = null;
                    Session["ApeMat"] = null;
                    Session["ApeCasada"] = null;
                    Session["Nombres"] = null;

                    Session["DescTipDoc"] = null;
                    Session["NroDoc"] = null;
                    Session["PER_NACIONALIDAD"] = null;
                    Session["iPersonaId"] = null;
                    Session["iTipoId"] = null;
                    Session["iDocumentoTipoId"] = null;
                    Session["iPersonaTipoId"] = null;
                    Session["FecNac"] = null;
                    Session["PER_GENERO"] = null;
                    Session["iCodPersonaId"] = null;
                    Session["DescTipDoc_OTRO"] = null;
                    Session["DtPersonaAct"] = null;

                    //clrlBusqueda.Direcciona = Enumerador.enmBusquedaDirecciona.TRAMITE;
                    ctrlBusqueda.Direcciona = Enumerador.enmBusquedaDirecciona.TRAMITE;
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