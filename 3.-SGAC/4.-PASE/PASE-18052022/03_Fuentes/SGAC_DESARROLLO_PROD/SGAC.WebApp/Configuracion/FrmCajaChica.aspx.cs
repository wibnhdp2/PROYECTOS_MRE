using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Contabilidad.CuentaCorriente.BL;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmCajaChica : System.Web.UI.Page
    {
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["bCajaChicaSaldoSatisfactorio"] = null;
                ObtenerSaldoCajaChica();
            }
        }

        private void ObtenerSaldoCajaChica()
        {
            try
            {
                CajaChicaConsultasBL oCajaChicaConsultasBL = new CajaChicaConsultasBL();
                double fSaldo = oCajaChicaConsultasBL.ObtenerSaldo(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
                lblSaldoActual.Text = Math.Round(fSaldo, 2).ToString();
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

            

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                Double lDiferencia = Convert.ToDouble(Session["HorarioDiferencia"]);
                Int16 lHorarioVerano = Convert.ToInt16(Session["HorarioVerano"]);

                CajaChicaMantenimientoBL oCajaChicaMantenimientoBL = new CajaChicaMantenimientoBL();
                CO_MOVIMIENTOCAJACHICA oCO_MOVIMIENTOCAJACHICA = new CO_MOVIMIENTOCAJACHICA();
                oCO_MOVIMIENTOCAJACHICA.moca_sTipoMovimientoId = Convert.ToInt16(Enumerador.enmTipoMovimientoTransaccion.INGRESOS);
                oCO_MOVIMIENTOCAJACHICA.moca_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                oCO_MOVIMIENTOCAJACHICA.moca_fMonto = Convert.ToDouble(txtMonto.Text);
                oCO_MOVIMIENTOCAJACHICA.moca_dFechaRegistro = Util.ObtenerFechaActual(lDiferencia, lHorarioVerano);
                oCO_MOVIMIENTOCAJACHICA.moca_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                oCO_MOVIMIENTOCAJACHICA.moca_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                oCajaChicaMantenimientoBL.Insert(oCO_MOVIMIENTOCAJACHICA);

                Session["bCajaChicaSaldoSatisfactorio"] = true;

                Comun.EjecutarScript(this, "window.parent.close_ModalPopup('MainContent_btnEjecutarFuncionario');");
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

            
        }
    }
}