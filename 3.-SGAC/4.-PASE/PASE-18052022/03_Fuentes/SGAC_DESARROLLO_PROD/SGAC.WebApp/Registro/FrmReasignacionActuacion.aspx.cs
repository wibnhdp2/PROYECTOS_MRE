using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Globalization;
using System.Threading;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Actuacion.BL;
using SGAC.Registro.Persona.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Controlador;
using SGAC.BE;
using SGAC.Accesorios;
using System.Text;
using System.IO;
using System.Configuration;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActuacionReasignar : MyBasePage
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["GUID"] != null)
                {
                    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                }
                else
                {
                    HFGUID.Value = "";
                }
                CargarDatosPersonaActuacion();
                msjeWarningPersFN.Visible = false;                
            }
        }

        protected void txtPriApellido_TextChanged(object sender, EventArgs e)
        {
            btnBuscarR_Click(sender, e);
        }

        protected void btnBuscarR_Click(object sender, EventArgs e)
        {
            if ((txtNroDocumento.Text.Length > 0) || (txtPriApellido.Text.Length >= 3) || (txtSegApellido.Text.Length >= 3))
            {
                BindGridResignacion(0,
                                    txtNroDocumento.Text.Trim(),
                                    txtPriApellido.Text.Trim(),
                                    txtSegApellido.Text.Trim());

                TextBox txtPage = (TextBox)ctrlPageBar1.FindControl("txtPagina");
            }
            else
            {
                Grd_Solicitante.DataSource = null;
                Grd_Solicitante.DataBind();

                lblMsjeWarnigPersFN.Text = Constantes.CONST_VALIDACION_MIN_3_CARACTERES;
                msjeWarningPersFN.Visible = true;
               txtPriApellido.Focus();
            }
        }

        protected void Grd_Solicitante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[9].Text.Trim() != "&nbsp;")
                    e.Row.Cells[9].Text = (Comun.FormatearFecha(e.Row.Cells[9].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            }
        }

        protected void Grd_Solicitante_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            if (e.CommandName == "Reasignar")
            {
                int intSeleccionado = Convert.ToInt32(e.CommandArgument);
                if (intSeleccionado > -1)
                {
                    Int64 intPersonaId = Convert.ToInt64(Grd_Solicitante.Rows[intSeleccionado].Cells[0].Text);

                    RE_ACTUACION objActuacion = new RE_ACTUACION();
                    objActuacion.actu_iActuacionId = Convert.ToInt64(lblNroActD.Text);
                    objActuacion.actu_sOficinaConsularId = Convert.ToInt16(hOfiConsularID.Value);
                    objActuacion.actu_iPersonaRecurrenteId = intPersonaId;
                    objActuacion.actu_sEstado = (int)Enumerador.enmActuacionEstado.REGISTRADO;
                    objActuacion.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objActuacion.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Int16 sTarifarioID = Convert.ToInt16(Session["acde_sTarifarioId"]);
                    String StrScript = String.Empty;

                    ActuacionMantenimientoBL objBL = new ActuacionMantenimientoBL();
                    string strMensaje = "";

                    Int64 acde_iActuacionDetalleId = 0;
                    
                    if (Request.QueryString["iActuDetalle"] != null)
                    {
                        acde_iActuacionDetalleId = Convert.ToInt64(Sanitizer.GetSafeHtmlFragment(Request.QueryString["iActuDetalle"].ToString()));
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Error. No se pudo grabar la operación.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                    }
                    

                    int intResultado = objBL.Reasignar(objActuacion, sTarifarioID, ref strMensaje, acde_iActuacionDetalleId);

                    if (strMensaje != string.Empty)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Reasignación", strMensaje, false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }

                    if (intResultado > 0)
                    {                       
                        if (Request.QueryString["CodPer"] != null)
                        {
                            string codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

                            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                            {
                                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
                            }
                            else
                            { // PERSONA NATURAL
                                string codTipoDocEncriptada = "";
                                string codNroDocumentoEncriptada = "";

                                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                {
                                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                }
                                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                {
                                    Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                }
                                else
                                {
                                    Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona, false);
                                }
                            }                            
                        }
                        else {
                            Response.Redirect("~/Configuracion/frmBusquedaActuacionAdmin.aspx", false);
                        }                        
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Error. No se pudo grabar la operación.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                    }
                }
            }
        }

        #endregion

        #region Metodos
        private void BindGridResignacion(long LonPersonaId,
                                         string StrNroDoc,
                                         string StrApePat,
                                         string StrApeMat)
        {
            DataTable dtRecurrente = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_PERSONA_REASIGNACION;

            msjeWarningPersFN.Visible = false;

            Proceso MiProc = new Proceso();

            Object[] miArray = new Object[8] { LonPersonaId,
                                               StrNroDoc,
                                               StrApePat,
                                               StrApeMat,
                                               ctrlPageBar1.PaginaActual.ToString(), 
                                               intPaginaCantidad,
                                               IntTotalCount,
                                               IntTotalPages };

            dtRecurrente = (DataTable)MiProc.Invocar(ref miArray,
                                                     "SGAC.BE.RE_ACTUACION",
                                                     Enumerador.enmAccion.BUSCAR);

            if (dtRecurrente.Rows.Count > 0)
            {
                Grd_Solicitante.DataSource = dtRecurrente;
                Grd_Solicitante.DataBind();

                ctrlPageBar1.TotalResgistros = Convert.ToInt32(miArray[6]);
                ctrlPageBar1.TotalPaginas = Convert.ToInt32(miArray[7]);

                ctrlPageBar1.Visible = false;
                if (ctrlPageBar1.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPageBar1.Visible = true;
                }
            }
            else
            {
                lblMsjeWarnigPersFN.Text = Constantes.CONST_VALIDACION_BUSQUEDA;
                msjeWarningPersFN.Visible = true;
                Grd_Solicitante.DataSource = null;
                Grd_Solicitante.DataBind();
            }
        }


        private void CargarDatosPersonaActuacion()
        {
            DataTable dtRecurrente = new DataTable();
            string iActuDetalle = "";
            Proceso objProceso = new Proceso();

            if (Request.QueryString["iActuDetalle"] != null)
            {
                iActuDetalle = Sanitizer.GetSafeHtmlFragment(Request.QueryString["iActuDetalle"].ToString());

                ActuacionConsultaBL obj = new ActuacionConsultaBL();
                dtRecurrente = obj.ObtenerDatosPorActuacionDetalleLeftJoin(Convert.ToInt64(iActuDetalle));

                lblNroDocD.Text = string.Empty;
                lblPrimerApellidoD.Text = string.Empty;
                lblSegundoApellidoD.Text = string.Empty;
                lblNombresD.Text = string.Empty;
                lblNroActD.Text = string.Empty;
                lblRGED.Text = string.Empty;
                lblCorrTariD.Text = string.Empty;
                lblFechaD.Text = string.Empty;
                lblDescripcionD.Text = string.Empty;
                hOfiConsularID.Value = "";
                if (dtRecurrente.Rows.Count > 0)
                {
                    lblNroDocD.Text = dtRecurrente.Rows[0]["peid_vDocumentoNumero"].ToString();
                    lblPrimerApellidoD.Text = dtRecurrente.Rows[0]["pers_vApellidoPaterno"].ToString();
                    lblSegundoApellidoD.Text = dtRecurrente.Rows[0]["pers_vApellidoMaterno"].ToString();
                    lblNombresD.Text = dtRecurrente.Rows[0]["pers_vNombres"].ToString();
                    lblNroActD.Text = dtRecurrente.Rows[0]["acde_iActuacionId"].ToString();
                    lblRGED.Text = dtRecurrente.Rows[0]["acde_ICorrelativoActuacion"].ToString();
                    lblCorrTariD.Text = dtRecurrente.Rows[0]["acde_ICorrelativoTarifario"].ToString();
                    lblFechaD.Text = Comun.FormatearFecha(dtRecurrente.Rows[0]["acde_dFechaRegistro"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    lblDescripcionD.Text = dtRecurrente.Rows[0]["tari_vDescripcion"].ToString();
                    Session["acde_sTarifarioId"] = dtRecurrente.Rows[0]["acde_sTarifarioId"].ToString();
                    if (Request.QueryString["CodOfi"] != null)
                    {
                        hOfiConsularID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodOfi"].ToString());
                    }
                    else
                    {
                        hOfiConsularID.Value = dtRecurrente.Rows[0]["ofco_sOficinaConsularId"].ToString();
                    }
                }
            }
            //------------------------------------------ 
        }
        #endregion

        protected void txtNroDocumento_TextChanged(object sender, EventArgs e)
        {
            btnBuscarR_Click(sender, e);
        }

        protected void txtSegApellido_TextChanged(object sender, EventArgs e)
        {         
            btnBuscarR_Click(sender, e);
        }

    }
}