using System;
using SGAC.Accesorios;
using SGAC.BE.MRE;
namespace SGAC.WebApp.PageError
{
    public partial class NotFoundPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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


            #region Auditoría
            if (Session["PAGINA_ACCEDER"] != null)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = "No tiene acceso al formulario: " + Session["PAGINA_ACCEDER"].ToString(),
                    audi_vMensaje = "No se encontró el recurso solicitado",
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                Session["PAGINA_ACCEDER"] = null;
            }            
            #endregion


        }
    }
}