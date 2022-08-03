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
    public partial class GeneraCodigo03 : System.Web.UI.Page
    {
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        public static beRegistroLinea SessionGeneral = new beRegistroLinea();
        brGeneral obrGeneral = new brGeneral();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["SessionGeneral"] != null)
                    {
                        SessionGeneral = (beRegistroLinea)Session["SessionGeneral"];
                    }
                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void buscarSolicitud(object sender, EventArgs e)
        {
            try
            {
                string numSol = txtCodSolicitud.Text.Trim().ToUpper(); ;
                SessionGeneral = new beRegistroLinea();
                SessionGeneral.NumeroRegLinea = numSol;
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                SessionGeneral = obrRegistroLinea.consultarRegistro(SessionGeneral);
                if (SessionGeneral != null)
                {
                    if (SessionGeneral.RegistroLineaId > 0)
                    {
                        SessionGeneral.NumeroRegLinea = numSol;
                        beTipoEmision obeTipoEmision = new beTipoEmision();
                        string tipoEmision = obeTipoEmision.TIPO_EMSION(1);
                        SessionGeneral.TipoEmisionObject = tipoEmision;
                        Session["SessionGeneral"] = SessionGeneral;
                        List<beRegistroLinea> lista = new List<beRegistroLinea>();
                        lista.Add(SessionGeneral);
                        gvRegLinea.DataSource = lista;
                        gvRegLinea.DataBind();
                        //lblMensajeRecuperar.ForeColor = (Color)System.Drawing.ColorTranslator.FromHtml("#0055aa");
                        //ibtEditarSol.Visible = true;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "visibleRow", "visibleRow('table-row');", true);
                        //lblMensajeRecuperar.Text = "SOLICITUD ENCONTRADA";//+ SessionGeneral.NumeroRegLinea + " DE FECHA " + SessionGeneral.ConFechaCreacion;

                    }
                    else
                    {
                        gvRegLinea.DataSource = null;
                        gvRegLinea.DataBind();
                        //ibtEditarSol.Visible = false;
                        //lblMensajeRecuperar.ForeColor = Color.Red;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "visibleRow", "visibleRow('table-row');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "visibleRow", "visibleRow('none');", true);
                        //lblMensajeRecuperar.Text = "NO SE UBICO LA SOLICITUD";
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL BUSCAR LA SOLICITUD');", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void verTemplate(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItemIndex > -1)
                {
                    if (e.Row.Cells[1].Text == "REGISTRADO")
                    {
                        e.Row.Cells[5].Controls[1].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void nextPage(object sender, EventArgs e)
        {
            try
            {
                if (SessionGeneral != null)
                {
                    if (SessionGeneral.RegistroLineaId > 0)
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
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        
        protected void prevPage(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(@"..\01_Gen_Codigo\GeneraCodigo01.aspx");
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void gvRegLinea_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.HeaderRow.CssClass = "thead-light";
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
                gv.HeaderRow.CssClass = "thead-light";
            }
        }
    }
}