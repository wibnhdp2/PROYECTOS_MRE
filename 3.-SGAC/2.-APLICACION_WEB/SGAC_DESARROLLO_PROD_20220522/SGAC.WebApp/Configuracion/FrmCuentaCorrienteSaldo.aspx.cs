using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Contabilidad.CuentaCorriente.BL;
using SGAC.Accesorios;
using System.Data;
using SGAC.BE.MRE;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmCuentaCorrienteSaldo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdn_CuentaCorrienteId.Value = Session["sCuentaCorrienteId"].ToString();
                hdn_TipoMonedaId.Value = Session["sCCTipoMoneda"].ToString();
                
                Session["bCajaChicaSaldoSatisfactorio"] = null;
                ObtenerSaldo();
            }
            
        }

        private void ObtenerSaldo()
        {
            try
            {
                
                CuentaConsultasBL oCuentaConsultasBL = new CuentaConsultasBL();
                double fSaldo = Math.Round(oCuentaConsultasBL.ObtenerSaldoCuenta(Convert.ToInt16(hdn_CuentaCorrienteId.Value)), 2);

                lblSaldoActual.Text = fSaldo.ToString();

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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Double lDiferencia = Convert.ToDouble(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria"));
                Int16 lHorarioVerano = Convert.ToInt16(Accesorios.comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano"));

                DateTime dFechaActualConsulado= Util.ObtenerFechaActual(lDiferencia, lHorarioVerano);

                TransaccionMantenimientoBL oTransaccionMantenimientoBL = new TransaccionMantenimientoBL();
                CO_TRANSACCION oCO_TRANSACCION= new CO_TRANSACCION();
                oCO_TRANSACCION.tran_sCuentaCorrienteId=Convert.ToInt16(hdn_CuentaCorrienteId.Value);
                oCO_TRANSACCION.tran_sOficinaConsularId=Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                oCO_TRANSACCION.tran_sMovimientoTipoId = Convert.ToInt16(Enumerador.enmTipoMovimientoTransaccion.INGRESOS);
                oCO_TRANSACCION.tran_vNumeroOperacion = string.Empty;
                oCO_TRANSACCION.tran_sTipoId = Convert.ToInt16(Enumerador.enmTipoTranIngreso.SALDO_INICIAL); ;
                oCO_TRANSACCION.tran_sMonedaId= Convert.ToInt16(hdn_TipoMonedaId.Value);
                oCO_TRANSACCION.tran_FMonto=Convert.ToDouble(txtMonto.Text);
                oCO_TRANSACCION.tran_FSaldo = Convert.ToDouble(txtMonto.Text);
                oCO_TRANSACCION.tran_dFechaOperacion=dFechaActualConsulado;
                oCO_TRANSACCION.tran_cPeriodo = dFechaActualConsulado.Year.ToString() + dFechaActualConsulado.Month.ToString();
                oCO_TRANSACCION.tran_vObservacion = txtObservacion.Text.Trim().ToUpper();
                oCO_TRANSACCION.tran_sUsuarioCreacion= Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                oCO_TRANSACCION.tran_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                oTransaccionMantenimientoBL.Insert(oCO_TRANSACCION);

                Session["bCuentaCorrienteSaldoSatisfactorio"] = true;

                Comun.EjecutarScript(this, "window.parent.close_ModalPopup();");
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
    }
}