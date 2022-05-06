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
    public partial class frmReactivarActuacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Cargar(true, true);
                    ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                }
                else
                {
                    ctrlOficinaConsular.Cargar(false, false);
                    ctrlOficinaConsular.Enabled = false;
                    ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                }
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                btnReactivar.Visible = false;
            }
        }
        private void LimpiarGrilla()
        {
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);

                if (hFechaTramite.Value.Trim() != string.Empty)
                {
                    strFechaRegistro = hFechaTramite.Value.Trim();
                }

                try
                {
                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == true)
                    {
                        #region Reactivar Actuación
                        Int16 CodUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        ActuacionMantenimientoBL objBL = new ActuacionMantenimientoBL();
                        objBL.ReactivarActuacion(Convert.ToInt64(hActuacionDetalleID.Value), Convert.ToInt64(hActuacionID.Value), CodUsuario, Convert.ToInt16(hOficinaConsularID.Value),txtMotivo.Text);
                        ctrlValidacion.MostrarValidacion("Se reactivo el trámite correctamente", true, Enumerador.enmTipoMensaje.INFORMATION);
                        btnLimpiar_Click(null, null);
                        #endregion
                    }
                    else
                    {
                        ctrlValidacion.MostrarValidacion("No se puede reactivar, tiene fecha de cierre.", true, Enumerador.enmTipoMensaje.ERROR);
                    }


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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            hFechaTramite.Value = "";
            hOficinaConsularID.Value = "";
            hActuacionDetalleID.Value = "";
            hActuacionID.Value = "";

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ctrlOficinaConsular.ddlOficinaConsular.SelectedIndex <= 1)
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(), "myScript", "alert('Seleccione una oficina consular');", true);
                    return;
                }
            }

            if (txtCorActuacion.Text == "")
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(), "myScript", "alert('el valor del correlativo debe ser mayor a 0');", true);
                return;
            }

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DataTable dtActuaciones = objBL.ActuacionesObtenerAnuladasReactivar(Convert.ToInt16(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), Convert.ToInt32(txtCorActuacion.Text), Convert.ToInt16(ddlAnio.SelectedValue));

            if (dtActuaciones.Rows.Count > 0)
            {
                hFechaTramite.Value = Convert.ToDateTime(dtActuaciones.Rows[0]["dFechaRegistro"]).ToShortDateString();
                hOficinaConsularID.Value = dtActuaciones.Rows[0]["sOficinaConsularId"].ToString();
                hActuacionDetalleID.Value = dtActuaciones.Rows[0]["iActuacionDetalleId"].ToString();
                hActuacionID.Value = dtActuaciones.Rows[0]["iActuacionId"].ToString();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO, true, Enumerador.enmTipoMensaje.INFORMATION);

                grReporteGestion.DataSource = dtActuaciones;
                grReporteGestion.DataBind();
                btnReactivar.Enabled = true;
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

                grReporteGestion.DataSource = null;
                grReporteGestion.DataBind();
                btnReactivar.Enabled = false;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCorActuacion.Text = "";
            txtMotivo.Text = "";
            ctrlOficinaConsular.SelectedIndex = 0;
            LimpiarGrilla();
            hFechaTramite.Value = "";
            hOficinaConsularID.Value = "";
            hActuacionDetalleID.Value = "";
            hActuacionID.Value = "";
        }
    }
}