using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;

namespace SolCARDIP_REGLINEA.Paginas._01_Gen_Codigo
{
    public partial class GeneraCodigo02 : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        static beRegistroLinea SessionGeneral = new beRegistroLinea();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["SessionGeneral"] != null)
                {
                    SessionGeneral = (beRegistroLinea)Session["SessionGeneral"];
                    SessionGeneral.TipoEmisionObject = "NUEVO";
                    hdnCaptcha.Value = SessionGeneral.NumeroRegLinea;
                    beGrafico obeGrafico = oCodigoUsuario.obtenerGrafico(SessionGeneral.NumeroRegLinea,450,20);
                    imgCodSol.Src = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(obeGrafico.buffer));
                }
            }
        }
        protected void nextPage(object sender, EventArgs e)
        {
            try
            {
                //Capturamos el contexto actual.
                System.Web.UI.Page formulario = (System.Web.UI.Page)HttpContext.Current.Handler;
                //Creamos script que redireccione desde el padre a otra ventana.
                string script = "<script type=\"text/javascript\">";
                script += "window.parent.location.href=\"../Principales/frmRegistroLinea.aspx\";";
                script += "</script>";
                //Registramos el Script.
                ScriptManager.RegisterStartupScript(formulario.Page, typeof(string), "AbrirVentana", script, false);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
    }
}