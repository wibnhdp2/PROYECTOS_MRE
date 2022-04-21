using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.WebApp.PageError
{
    public partial class GenericErrorPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            //----------------------------------------------------------
            //Fecha: 22/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Ir a la página principal.
            //----------------------------------------------------------
            string strUrlAbsolute = Request.Url.AbsoluteUri;
            int intLocSite = strUrlAbsolute.IndexOf("PageError");
            string strSite = strUrlAbsolute.Substring(0, intLocSite);
            string strURL = strSite + "Cuenta/FrmLogin.aspx";
            idLink.HRef = strURL;
            //----------------------------------------------------------


            base.OnLoad(e);


            Exception exc_Exception = (Exception)Session["_LastException"];

            if (exc_Exception != null)
            {
                #region Auditoría
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = exc_Exception.Message,
                    audi_vMensaje = exc_Exception.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion

                lbl_ErrorMsg.Text = exc_Exception.Message;
                div_StackTrace.InnerHtml = exc_Exception.StackTrace.Replace(Environment.NewLine, "<br />");
            }
            else
            {
                tbl_Exception.Visible = false;
            }
        }    
    }
}