using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;

namespace SolCARDIP_REGLINEA.Paginas.Principales
{
    public partial class PrincipalForm : System.Web.UI.MasterPage
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ViewState["IP"] = oCodigoUsuario.obtenerIP();  
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                Response.Redirect(@"..\mensajes.aspx");
            }
        }
        private void MostrarGrillaBotones()
        {
            if (Session["ActivarBotones"] == null)
            {
               // tableGoTo.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "ocultarTabla();", true);
            }
            else
            {
                if ((bool)Session["ActivarBotones"])
                {
                  //  tableGoTo.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "mostrarTabla();", true);
                }
                else
                {
                  //  tableGoTo.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "ocultarTabla();", true);
                }
            }
        }
        protected void botonPrueba(string valor)
        {
            beURL obeURL = new beURL();
            int vV = int.Parse(valor);
            if (vV > 0)
            {
                Response.Redirect(obeURL.goToURL(vV), false);
            }
        }
        //protected void finalizarRegistroLinea(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        beRegistroLinea SessionGeneral = new beRegistroLinea();
        //        string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
        //        string fileName = "error";
        //        string ip = ViewState["IP"].ToString();
        //        brRegistroLinea obrRegistroLinea = new brRegistroLinea();
        //        if (Session["SessionGeneral"] != null)
        //        {
        //            SessionGeneral = (beRegistroLinea)Session["SessionGeneral"];
        //            DateTime DtNow = DateTime.Now;
        //            bool exitoFotografia = false;
        //            string rutaRelativa = "error";
        //            if (SessionGeneral.TipoEmisionObject.Equals("NUEVO"))
        //            {
        //                fileName = oCodigoUsuario.getFileName();
        //                //exitoFotografia = oCodigoUsuario.guardarImagen(rutaAdjuntos, fileName);
        //                exitoFotografia = oCodigoUsuario.guardarImagen(rutaAdjuntos, fileName, (string)Session["tempImagen"], (FileUpload)Session["fileUploadImage"]);
        //                rutaRelativa = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
        //            }
        //            else
        //            {
        //                if (Session["tempImagen"] == null)
        //                {
        //                    exitoFotografia = true;
        //                    rutaRelativa = SessionGeneral.DpRutaAdjunto;
        //                }
        //                else
        //                {
        //                    fileName = oCodigoUsuario.getFileName();
        //                    //exitoFotografia = guardarImagen(rutaAdjuntos, fileName);
        //                    exitoFotografia = oCodigoUsuario.guardarImagen(rutaAdjuntos, fileName, (string)Session["tempImagen"], (FileUpload)Session["fileUploadImage"]);
        //                    rutaRelativa = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
        //                }
        //            }
        //            if (exitoFotografia)
        //            {
        //                SessionGeneral.IpModificacion = ip;
        //                SessionGeneral.DpRutaAdjunto = rutaRelativa;
        //                bool exito = obrRegistroLinea.actualizar(SessionGeneral);
        //                if (exito)
        //                {
        //                    //oCodigoUsuario.DisableControls(tableCargaFoto, false);
        //                    //hdnfldValor.Value = "saved";
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE FINALIZO CON EL REGISTRO CORRECTAMENTE');", true);
        //                }
        //                else
        //                {
        //                    File.Delete(rutaAdjuntos + fileName);
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL FINALIZAR EL REGISTRO');", true);
        //                }
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE SELECCIONO UN ARCHIVO DE FOTOGRAFIA. INTENTELO NUEVAMENTE');", true);
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL CONTINUAR');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obrGeneral.grabarLog(ex);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR')", true);
        //    }
        //}
    }
}