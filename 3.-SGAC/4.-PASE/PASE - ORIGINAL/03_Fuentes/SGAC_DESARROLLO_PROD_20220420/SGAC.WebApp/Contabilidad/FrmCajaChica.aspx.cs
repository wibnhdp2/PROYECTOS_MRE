using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Contabilidad.CuentaCorriente.BL;

namespace SGAC.WebApp.Contabilidad
{
    public partial class FrmCajaChica : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;

            ctrlToolBarMantenimiento.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarMantenimiento.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            if (!Page.IsPostBack)
            {
                gdvMovimientos.DataSource = new DataTable();
                gdvMovimientos.DataBind();

                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatos();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            string strScript = string.Empty;
            string strMensaje = string.Empty;

            #region Validación
            double dblDepositado = 0;
            double dblDevuelto = 0;
            if (txtDepositado.Text.Trim() == string.Empty)
            {
                strMensaje += " Monto Depositado,";
            }
            else
            {
                dblDepositado = Convert.ToDouble(txtDepositado.Text);
                if (dblDepositado == 0)
                    strMensaje += "  Monto Depositado Mayor a 0,";
            }
            if (txtDevuelto.Text.Trim() == string.Empty)
            {
                strMensaje += "  Monto Devuelto,";
            }
            else
            {
                dblDevuelto = Convert.ToDouble(txtDevuelto.Text);
                if (dblDevuelto == 0)
                    strMensaje += " Monto Devuelto Mayor a 0,";
            }
            if (dblDevuelto > dblDepositado)
            {
                strMensaje += " Monto Devuelto debe ser menor igual al Monto Depositado,";
            }
            if (dtpFecha.Text.Length < 11)
            {
                strMensaje += " Fecha de Devolución,";
            }

            if (strMensaje != string.Empty)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CAJA CHICA - MOVIMIENTOS", "Validar:" + strMensaje.Substring(0, strMensaje.Length-1));
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            #endregion

            BE.MRE.CO_MOVIMIENTOCAJACHICA CajaChica = new BE.MRE.CO_MOVIMIENTOCAJACHICA();
            CajaChica.moca_sOficinaConsularId = Convert.ToInt16(hdn_sOficinaConsularId.Value);
            CajaChica.moca_sTipoMovimientoId = (Int16)Enumerador.enmTipoMovimientoTransaccion.SALIDAS;
            CajaChica.moca_fMonto = Convert.ToDouble(txtDevuelto.Text);
            CajaChica.moca_fMontoOperacion = Convert.ToDouble(txtDepositado.Text);
            CajaChica.moca_sBancoId = Convert.ToInt16(ddlBanco.SelectedValue);
            CajaChica.moca_dFechaRegistro = Comun.FormatearFecha(dtpFecha.Text);
            CajaChica.moca_vNumeroComprobante = txtComprobante.Text.Trim().ToUpper();
            CajaChica.moca_vNumeroOperacion = txtNumOperacion.Text.Trim().ToUpper();
            CajaChica.moca_sUsuarioCreacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            CajaChica.moca_vIPCreacion = Util.ObtenerDireccionIP();
            CajaChica.HostName = Util.ObtenerHostName();

            CajaChicaMantenimientoBL objBL = new CajaChicaMantenimientoBL();
            CajaChica = objBL.Insert(CajaChica);

            LimpiarDatos();
            updMantenimiento.Update();

            if (CajaChica.Error)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CAJA CHICA - MOVIMIENTOS", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CAJA CHICA - MOVIMIENTOS", Constantes.CONST_MENSAJE_EXITO);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Limpiar 
            dtpFecha.Text = Comun.ObtenerFechaActualTexto(Session);
            txtDepositado.Text = "";
            txtDevuelto.Text = "";
            txtComprobante.Text = "";
            lblTotal.Text = "";

            gdvMovimientos.DataSource = new DataTable();
            gdvMovimientos.DataBind();

            // Realizar Búsqueda
            Int16 intBancoId = Convert.ToInt16(ddlBanco.SelectedValue);
            Int16 intOficinaConsularId = Convert.ToInt16(hdn_sOficinaConsularId.Value);

            string strNumeroOperacion = txtNumOperacion.Text.Trim();

            CajaChicaConsultasBL objBL = new CajaChicaConsultasBL();
            DataTable dtMovimientos = objBL.ObtenerMovimientosPorNumOperacion(intOficinaConsularId, intBancoId, strNumeroOperacion);

            Double dblTotal = 0;
            foreach (DataRow dr in dtMovimientos.Rows)
            {
                dblTotal += Convert.ToDouble(dr["moca_fMonto"]);
            }
            if (dblTotal > 0)
                lblTotal.Text = "(Total: " + Convert.ToDecimal(string.Format("{0:0.00}", dblTotal)) + ")"; 

            gdvMovimientos.DataSource = dtMovimientos;
            gdvMovimientos.DataBind();
        }

        private void CargarDatosIniciales()
        {
            hdn_sOficinaConsularId.Value = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            hdn_sUsuarioId.Value = Session[Constantes.CONST_SESION_USUARIO_ID].ToString();

            dtpFecha.Text = Comun.ObtenerFechaActualTexto(Session);

            CajaChicaConsultasBL oCajaChicaConsultasBL = new CajaChicaConsultasBL();
            double dblSaldo = oCajaChicaConsultasBL.ObtenerSaldo(Convert.ToInt16(hdn_sOficinaConsularId.Value));
            txtSaldoCajaChica.Text = Convert.ToDecimal(string.Format("{0:0.00}", dblSaldo)).ToString();

            lblSaldoMoneda.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            lblMonedaLocal.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            lblDevueltoMoneda.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
        }

        private void LimpiarDatos()
        {
            // Consulta
            ddlBanco.SelectedIndex = 0;
            txtNumOperacion.Text = "";

            // Registro Devolución
            CajaChicaConsultasBL oCajaChicaConsultasBL = new CajaChicaConsultasBL();
            double dblSaldo = oCajaChicaConsultasBL.ObtenerSaldo(Convert.ToInt16(hdn_sOficinaConsularId.Value));
            txtSaldoCajaChica.Text = Convert.ToDecimal(string.Format("{0:0.00}", dblSaldo)).ToString();

            dtpFecha.Text = Comun.ObtenerFechaActualTexto(Session);
            txtDepositado.Text = "";
            txtDevuelto.Text = "";
            txtComprobante.Text = "";
            lblTotal.Text = "";

            gdvMovimientos.DataSource = new DataTable();
            gdvMovimientos.DataBind();
        }
    }
}