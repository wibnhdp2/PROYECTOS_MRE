using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Data;

using SGAC.Registro.Actuacion.BL;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp.Reportes
{
    public partial class frmReporteAntecedentesPenales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnImprimir.OnClientClick = "return abrirPopupEspera();";
            ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();

                this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
                this.dtpFecInicio.EndDate = DateTime.Now;

                this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);


            }
        }
        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sOficinaConsularId = 0;

                sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

                UsuarioConsultasBL obj = new UsuarioConsultasBL();
                DataTable dt = obj.ObtenerLista(sOficinaConsularId);
                Util.CargarDropDownList(ddlUsuarios, dt, "usua_vAlias", "usua_sUsuarioId", true, " - TODOS - ");
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
        private void CargarListadosDesplegables()
        {
            // Lima - Carga todas las Misiones
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Cargar(true, false);
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, false);
            }
            ddlOficinaConsular_SelectedIndexChanged(null,null);
        }
        private void CargarDatosIniciales()
        {
            dtpFecInicio.Text = DateTime.Today.ToString("MMM-01-yyyy");
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

            ctrlOficinaConsular.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            //--------------------------------------------
            ddlTipoReporte.AppendDataBoundItems = true;
            ddlTipoReporte.Items.Insert(0, new ListItem(SGAC.Accesorios.Constantes.CONST_CERTIFICADO_ANTECEDENTES_PENALES, "0"));
            ddlTipoReporte.Items.Insert(1, new ListItem(SGAC.Accesorios.Constantes.CONST_CERTIFICADO_ANTECEDENTES_PENALES_USUARIO, "1")); 
            ddlTipoReporte.DataBind();
        }

        protected void chkFechaRegistroMSIAP_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaRegistroRGE.Checked = !chkFechaRegistroMSIAP.Checked;
        }

        protected void chkFechaRegistroRGE_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaRegistroMSIAP.Checked = !chkFechaRegistroRGE.Checked;
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            btnImprimir.Enabled = false;

            try
            {
                if (ddlTipoReporte.SelectedIndex == -1)
                {
                    ctrlValidacion.MostrarValidacion("Seleccion el tipo de Reporte", true, Enumerador.enmTipoMensaje.ERROR);
                    btnImprimir.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "div", "verDIV();", true); 
                    return;
                }

                if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                    btnImprimir.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "div", "verDIV();", true);
                    return;
                }

                if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                    btnImprimir.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "div", "verDIV();", true);
                    return;
                }
                if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                    btnImprimir.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "div", "verDIV();", true);
                    return;
                }

                DateTime datFechaInicio = new DateTime();
                DateTime datFechaFin = new DateTime();

                if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                }
                if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
                {
                    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                }

                if (datFechaInicio > datFechaFin)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                }
                else
                {
                    Session["FechaIntervalo"] = "PERIODO DEL " + datFechaInicio.ToShortDateString() + " AL " + datFechaFin.ToShortDateString();
                    Session["IdOficinaConsular_contabilidad"] = ctrlOficinaConsular.SelectedValue;
                    VerVistaPrevia();

                }
                btnImprimir.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "div", "verDIV();", true);
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
        private void VerVistaPrevia()
        {
            bool bolVistaPrevia = false;
            ctrlValidacion.MostrarValidacion("", false);
            DataSet ds = new DataSet();
            
            ds = ObtenerDataTableReporte();

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                    if (intCantidadRegistros > 0)
                    {
                        Session["dtDatos"] = ds.Tables[0];
                        if (ddlTipoReporte.SelectedItem.Text == "CERTIFICADO DE ANTECEDENTES PENALES")
                        {
                            Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES);
                        }
                        else {
                            Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES_USUARIO);
                        }
                        bolVistaPrevia = true;
                    }
                }
            }
            if (!bolVistaPrevia)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
        }

        private DataSet ObtenerDataTableReporte()
        {
            DataSet ds = new DataSet();

            short intOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular.SelectedValue);

            string strFechaInicio = "";
            string strFechaFinal = "";

            if (dtpFecInicio.Text.Trim().Length > 10)
                {
                    strFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text).ToString("yyyyMMdd");
                }
            if (dtpFecFin.Text.Trim().Length > 10)
                {
                    strFechaFinal = Comun.FormatearFecha(dtpFecFin.Text).ToString("yyyyMMdd");
                }
                                    
            
            int sUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            int intOficinaConsularLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            string strClaseFecha = "";
            if (chkFechaRegistroMSIAP.Checked == true)
            {
                strClaseFecha = "M";
            }
            else
            {
                strClaseFecha = "R";
            }

            Antecedente_PenalBL objAntecedentePenalBL = new Antecedente_PenalBL();

            if (ddlTipoReporte.SelectedItem.Text == "CERTIFICADO DE ANTECEDENTES PENALES")
            {
                ds = objAntecedentePenalBL.ObtenerDataSet(0, intOficinaConsularId, strFechaInicio, strFechaFinal, strClaseFecha,0);
            }
            else {
                ds = objAntecedentePenalBL.ObtenerDataSet(0, intOficinaConsularId, strFechaInicio, strFechaFinal, strClaseFecha,Convert.ToInt16( ddlUsuarios.SelectedValue));
            }

                                    
            return ds;
        }
        private int ObtenerTotalRegistroDataSet(DataSet ds)
        {
            int intTotalRegistros = 0;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i] != null)
                {
                    intTotalRegistros += ds.Tables[i].Rows.Count;
                }
            }
            return intTotalRegistros;
        }
    }
}