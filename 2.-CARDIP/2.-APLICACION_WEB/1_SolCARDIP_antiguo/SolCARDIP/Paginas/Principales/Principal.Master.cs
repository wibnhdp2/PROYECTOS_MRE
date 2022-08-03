using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessLogic;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.ReglasNegocio;

namespace SolCARDIP.Paginas.Principales
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        brGeneral obrGeneral = new brGeneral();
        Utilitarios objUtil = new Utilitarios();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../Login.aspx';", true);
                }
                if (Session["usuario"] != null)
                {
                    objUtil.cargaPaginas(menuPrincipal, this.Page);
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    lblUsuario.Text = objUsuarioBE.NombreCompleto;
                    lblPerfil.Text = objUsuarioBE.NombreRol;
                    lblSede.Text = objUsuarioBE.NombreOficina.ToUpper();
                    //Session["OficinaId"] = objUsuarioBE.codOficina;
                    //Session["UsuarioId"] = objUsuarioBE.UsuarioId;
                    //Session["UsuarioNombre"] = objUsuarioBE.Nombres + " " + objUsuarioBE.ApellidoPaterno + " " + objUsuarioBE.ApellidoMaterno;
                    brGenerales obrGenerales = new brGenerales();
                    beGenerales obeGenerales = obrGenerales.obtenerGenerales();
                    if (obeGenerales != null)
                    {    
                        Session["Generales"] = obeGenerales;
                    }
                }
                //Session["Oficina"] = "1";
                //Session["IP"] = "000.0.0.0";
                SmtpClient oSMTP = new SmtpClient();
                ViewState["host"] = oSMTP.Host;
              
                Td1.InnerText = "";


            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void btncerrarsesion_Click(object sender, EventArgs e)
        {
            string strIsPortal = Session["portal"].ToString();
            Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Expires = -1;
            Response.Cache.SetNoStore();
            Session["usuario"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Contents.RemoveAll();
            if (strIsPortal.Equals("N"))
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "closePage", "window.close();", true);
            }
        }
    }
}