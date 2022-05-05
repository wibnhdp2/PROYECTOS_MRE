using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using Microsoft.Security.Application;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Persona.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActaConformidadActoNotarial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["GUID"] != null)
                //{
                //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                //}
                //else
                //{
                //    HFGUID.Value = "";
                //}
                string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
                if (Convert.ToInt64(codPersona) > 0)
                {
                    GetDataPersona(Convert.ToInt64(codPersona));
                }
                Imprimir_Acta_ConformidadServer();
            }
        }

        [System.Web.Services.WebMethod]
        public static string Imprimir_Acta_Conformidad(string strGUID)
        {
            string strEtiquetaSolicitante = string.Empty;
           

            if (HttpContext.Current.Session["ApePat" + strGUID] != null)
            {
                strEtiquetaSolicitante += HttpContext.Current.Session["ApePat" + strGUID].ToString() + " ";
            }

            if (HttpContext.Current.Session["ApeMat" + strGUID] != null)
            {
                strEtiquetaSolicitante += HttpContext.Current.Session["ApeMat" + strGUID].ToString() + " ";
            }


            if (HttpContext.Current.Session["Nombres" + strGUID] != null)
            {
                if (HttpContext.Current.Session["Nombres" + strGUID].ToString().Trim() != string.Empty)
                {
                    strEtiquetaSolicitante += ", " + HttpContext.Current.Session["Nombres" + strGUID].ToString() + " ";
                }
            }

            string documento = string.Empty;
            string numero = string.Empty;

            if (HttpContext.Current.Session["DescTipDoc" + strGUID] != null)
            {
                documento = HttpContext.Current.Session["DescTipDoc" + strGUID].ToString();
            }

            if (HttpContext.Current.Session["NroDoc" + strGUID] != null)
            {
                numero = HttpContext.Current.Session["NroDoc" + strGUID].ToString();
            }


            DateTime dt_Fecha = Comun.FormatearFecha(Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));

            string script = string.Empty;

            string str_Fecha = dt_Fecha.ToString("dd") + " de " + dt_Fecha.ToString("MMMM") + " de " + dt_Fecha.ToString("yyyy");

            script = "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\"><input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\">DECLARACIÓN DE CONFORMIDAD DEL USUARIO</span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"justify\" style=\"line-height: 150%; \"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">Yo, " + strEtiquetaSolicitante +
                ", identificado con el " + documento + " N° " + numero + ", declaro que he leído y revisado el formato, que he tenido a la vista y me ha sido entregado en la fecha, manifestando mi conformidad con su contenido </span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\" style=\"margin-bottom:5px;\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"80%\"><tr><td width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">" + Convert.ToString(HttpContext.Current.Session["CiudadOficinaConsular"]) + ", " +str_Fecha + "</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"163px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"border-color: groove; border-width: groove; border-style: groove; height:145px;\"></td></tr><tr><td align=\"center\" width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\" font-size:10pt\">Huella Digital</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"35%\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + strEtiquetaSolicitante + "</span></font></td></tr><tr><td align=\"center\" width=\"50%\" style=\"border-top-style: dashed; border-width: 2px\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + documento + " N° " + numero + "</span></font></td></tr></table></div>";

            return script;

        }
        public string Imprimir_Acta_ConformidadServer()
        {
            string strEtiquetaSolicitante = string.Empty;


            if (ViewState["ApePat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApePat"].ToString() + " ";
            }

            if (ViewState["ApeMat"] != null)
            {
                strEtiquetaSolicitante += ViewState["ApeMat"].ToString() + " ";
            }


            if (ViewState["Nombre"] != null)
            {
                if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                {
                    strEtiquetaSolicitante += ", " + ViewState["Nombre"].ToString() + " ";
                }
            }

            string documento = string.Empty;
            string numero = string.Empty;

            if (ViewState["DescTipDoc"] != null)
            {
                documento = ViewState["DescTipDoc"].ToString();
            }

            if (ViewState["NroDoc"] != null)
            {
                numero = ViewState["NroDoc"].ToString();
            }


            DateTime dt_Fecha = Comun.FormatearFecha(Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));

            string script = string.Empty;

            string str_Fecha = dt_Fecha.ToString("dd") + " de " + dt_Fecha.ToString("MMMM") + " de " + dt_Fecha.ToString("yyyy");

            script = "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\"><input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\">DECLARACIÓN DE CONFORMIDAD DEL USUARIO</span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"justify\" style=\"line-height: 150%; \"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">Yo, " + strEtiquetaSolicitante +
                ", identificado con el " + documento + " N° " + numero + ", declaro que he leído y revisado el formato, que he tenido a la vista y me ha sido entregado en la fecha, manifestando mi conformidad con su contenido </span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\" style=\"margin-bottom:5px;\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"80%\"><tr><td width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">" + Convert.ToString(HttpContext.Current.Session["CiudadOficinaConsular"]) + ", " + str_Fecha + "</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"163px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"border-color: groove; border-width: groove; border-style: groove; height:145px;\"></td></tr><tr><td align=\"center\" width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\" font-size:10pt\">Huella Digital</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"35%\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + strEtiquetaSolicitante + "</span></font></td></tr><tr><td align=\"center\" width=\"50%\" style=\"border-top-style: dashed; border-width: 2px\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + documento + " N° " + numero + "</span></font></td></tr></table></div>";

            return script;

        }
        private void GetDataPersona(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            try
            {
                DataTable dt = new DataTable();
                SGAC.Registro.Persona.BL.PersonaConsultaBL objPersonaBL = new SGAC.Registro.Persona.BL.PersonaConsultaBL();
                EmpresaConsultaBL objEmpresa = new EmpresaConsultaBL();

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    DataSet ds = objEmpresa.ConsultarId(LonPersonaId);
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = objPersonaBL.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
                }

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    ViewState["Nombre"] = string.Empty;
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vRazonSocial"].ToString();
                    ViewState["ApeMat"] = string.Empty;
                    ViewState["ApeCasada"] = string.Empty;
                    ViewState["Nombres"] = string.Empty;

                    ViewState["DescTipDoc"] = dt.Rows[0]["empr_vTipoDocumento"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNumeroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = string.Empty;
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = "2102";
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sTipoDocumentoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sTipoEmpresaId"].ToString();
                    ViewState["FecNac"] = string.Empty;
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = string.Empty;
                }
                else
                { // Persona natural
                    ViewState["Nombre"] = dt.Rows[0]["vNombres"].ToString();
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vApellidoPaterno"].ToString();
                    ViewState["ApeMat"] = dt.Rows[0]["vApellidoMaterno"].ToString();
                    ViewState["ApeCasada"] = dt.Rows[0]["vApellidoCasada"].ToString();
                    ViewState["Nombres"] = ViewState["ApePat"] + " " + ViewState["ApeMat"] + ViewState["ApeCasada"] + " , " + ViewState["Nombre"];

                    ViewState["DescTipDoc"] = dt.Rows[0]["vDescTipDoc"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = dt.Rows[0]["sNacionalidadId"].ToString();
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sDocumentoTipoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["FecNac"] = dt.Rows[0]["dNacimientoFecha"].ToString();
                    ViewState["PER_GENERO"] = dt.Rows[0]["sGeneroId"].ToString();
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = dt.Rows[0]["vTipoDocumento"].ToString();

                    ViewState["DtPersonaAct"] = null;
                }

                dt = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}