using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Management;
using System.Security.Principal;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;

namespace SolCARDIP_REGLINEA.Paginas._01_Gen_Codigo
{
    public partial class GeneraCodigo01 : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        public static beRegistroLinea SessionGeneral = new beRegistroLinea();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ViewState["IP"] = oCodigoUsuario.obtenerIP();
                    //crearGrafico();
                    refreshCaptcha();
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }            
        }
        #region Genera Captcha

        protected void refreshCaptcha()
        {
            beGrafico obeGrafico = oCodigoUsuario.obtenerGrafico("",250,15);
            if (obeGrafico != null)
            {
                ViewState["CodCaptcha"] = obeGrafico.codCaptcha;
                imgCaptcha.Src = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(obeGrafico.buffer));
                //txtCaptcha.Text = obeGrafico.sb.ToString();
                txtCaptcha.Text = "";
            }
        }

        protected void lbnActualizarCaptcha_Click(object sender, EventArgs e)
        {
            try
            {
                refreshCaptcha();
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected bool compararCaptcha(string strIngresado)
        {
            string codCaptcha = ViewState["CodCaptcha"].ToString();
            return String.Equals(strIngresado, codCaptcha);
        }
        #endregion

        protected void nextPage(object sender, EventArgs e)
        {
            try
            {
                string valorCaptchaUsuario = hdfldCaptchaUsuario.Value.Trim();
                bool exitoCaptcha = compararCaptcha(valorCaptchaUsuario);
                if (exitoCaptcha)
                {
                    int cmn = int.Parse(((Button)sender).CommandName);
                    beTipoEmision obeTipoEmision = new beTipoEmision();
                    string tipoEmision = obeTipoEmision.TIPO_EMSION(cmn);
                    SessionGeneral.TipoEmisionObject = tipoEmision;
                    if (tipoEmision.Equals("NUEVO"))
                    {
                        string codSol = codigoSolicitud();
                        if (!codSol.Equals("error"))
                        {
                            Response.Redirect(@"..\01_Gen_Codigo\GeneraCodigo02.aspx",false);
                        }
                        else
                        {
                            refreshCaptcha();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL GENERAR EL NUMERO DE SOLICITUD. INTENTELO NUEVAMENTE');", true);
                        }
                    }
                    else
                    {
                        Response.Redirect(@"..\01_Gen_Codigo\GeneraCodigo03.aspx");
                    }
                }
                else
                {
                    refreshCaptcha();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('CODIGO CAPTCHA INCORRECTO');", true);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected string codigoSolicitud()
        {
            string codSol = "error";
            int idRegistroLinea = -1;
            beRegistroLinea parametros = new beRegistroLinea();
            parametros.IpCreacion = ViewState["IP"].ToString();
            brRegistroLinea obrRegistroLinea = new brRegistroLinea();
            idRegistroLinea = obrRegistroLinea.adicionar(parametros);
            if (idRegistroLinea != -1)
            {
                SessionGeneral.RegistroLineaId = idRegistroLinea;
                SessionGeneral = obrRegistroLinea.obtenerNumero(SessionGeneral);
                if (SessionGeneral.NumeroRegLinea != null)
                {
                    if (!SessionGeneral.NumeroRegLinea.Equals(""))
                    {
                        SessionGeneral.RegistroLineaId = idRegistroLinea;
                        Session["SessionGeneral"] = SessionGeneral;
                        codSol = SessionGeneral.NumeroRegLinea;
                    }
                }
            }
            return (codSol);
        }
    }
}