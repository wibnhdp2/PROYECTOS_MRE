using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.BL;
using System.Data;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Configuracion
{
    public partial class frmBusquedaActuacionAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Cargar(true, true, " - SELECCIONAR - ", "");
                }
                else
                {
                    ctrlOficinaConsular.Cargar(false, false);
                }
                ctrlOficinaConsularDestino.Cargar(true, true, " - SELECCIONAR - ", "");
                Util.CargarComboAnios(ddlAnioBusqueda, 2015, DateTime.Now.Year);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ActuacionConsultaBL obj = new ActuacionConsultaBL();
                DataTable dt = obj.ActuacionesObtenerxRGE(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), Convert.ToInt16(ddlAnioBusqueda.SelectedValue), Convert.ToInt64(txtRGE.Text));
                gdvActuaciones.DataSource = dt;
                gdvActuaciones.DataBind();

                if (dt.Rows.Count > 0)
                {
                    ctrlValidacion.MostrarValidacion("Se encontro la siguiente informacuón", true, Enumerador.enmTipoMensaje.INFORMATION);
                }
                else {
                    ctrlValidacion.MostrarValidacion("No se encontro informacuón", true, Enumerador.enmTipoMensaje.INFORMATION);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void Reasignar(object sender, ImageClickEventArgs e)
        {
            Int64 _iActuacionDetalleId;
            Session["ActuacionDetalleId"] = null;
            _iActuacionDetalleId = Int64.Parse(((ImageButton)sender).Attributes["iActuacionDetalleId"]);

            ActuacionMantenimientoBL _obj = new ActuacionMantenimientoBL();

            //------------------------------------------------------------------------
            // Autor: Miguel Angel Márquez Beltrán
            // Fecha: 18/08/2016
            // Objetivo: Calcular los dias habiles permitidos para la anulación
            //           según sea Jeafatura o Consulado.
            //------------------------------------------------------------------------
            if (!chkObligatorioFecha.Checked)
            {
                DateTime dFechaRegistro = Convert.ToDateTime(((ImageButton)sender).Attributes["dFechaRegistro"]);
                string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                if (dFechaRegistro != null)
                {
                    if (dFechaRegistro.ToString().Trim() != string.Empty)
                    {
                        strFechaRegistro = dFechaRegistro.ToString().Trim();
                    }
                }

                if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                {
                    ctrlValidacion.MostrarValidacion("No puede Reasignar la fecha del trámite no se encuentra dentro del rango permitido.", true, Enumerador.enmTipoMensaje.INFORMATION);
                    return;
                }
            }
            

            if (chkUsarOficinaSeleccionada.Checked)
            {
                Response.Redirect("~/Registro/FrmReasignacionActuacion.aspx?iActuDetalle=" + _iActuacionDetalleId.ToString() + "&CodOfi=" + ctrlOficinaConsularDestino.SelectedValue.ToString(), false);
            }
            else {
                Response.Redirect("~/Registro/FrmReasignacionActuacion.aspx?iActuDetalle=" + _iActuacionDetalleId.ToString(), false);
            }
        }

        protected void chkUsarOficinaSeleccionada_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsarOficinaSeleccionada.Checked)
            {
                ocultar.Visible = true;
            }
            else {
                ocultar.Visible = false;
            }
        }
    }
}