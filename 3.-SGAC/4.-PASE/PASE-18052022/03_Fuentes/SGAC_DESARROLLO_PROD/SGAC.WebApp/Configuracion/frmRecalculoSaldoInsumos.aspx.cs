using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using SGAC.Almacen.BL;
namespace SGAC.WebApp.Configuracion
{
    public partial class frmRecalculoSaldoInsumos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Util.CargarComboAnios(ddlAnio, 2016, DateTime.Now.Year);
                DataTable dtMes = new DataTable();

                dtMes = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES);

                Util.CargarDropDownList(ddlMes, dtMes, "valor", "id");

                Util.CargarComboAnios(ddlAnioFinal, 2016, DateTime.Now.Year);

                Util.CargarDropDownList(ddlMesFinal, dtMes, "valor", "id");
                
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnioFinal.SelectedValue = DateTime.Now.Year.ToString();

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Cargar(true, true);
                }
                else
                {
                    ctrlOficinaConsular.Cargar(false, false);
                    ctrlOficinaConsular.Enabled = false;
                }
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                BtnRecalcular.Visible = false;
            }
        }

        protected void BtnRecalcular_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                Int16 CodUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                Int16 MesInicial = Convert.ToInt16(ddlMes.SelectedIndex + 1);
                Int16 MesFin = Convert.ToInt16(ddlMesFinal.SelectedIndex + 1);
                try
                {
                    InsumoMantenimientoBL obj = new InsumoMantenimientoBL();
                    obj.RecalcularSaldosInsumo(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), Convert.ToInt16(ddlAnio.SelectedValue), MesInicial, Convert.ToInt16(ddlAnioFinal.SelectedValue), MesFin, CodUsuario,chkReiniciar.Checked);

                    if (chkReiniciar.Checked)
                    {
                        ctrlValidacion.MostrarValidacion("Se ha reniciado los valores de los saldos correctamente de " + ddlAnio.SelectedValue.ToString() + "-" + ddlMes.SelectedItem.Text + " hasta " + ddlAnioFinal.SelectedValue.ToString() + "-" + ddlMesFinal.SelectedItem.Text, true, Enumerador.enmTipoMensaje.INFORMATION);
                    }
                    else {
                        ctrlValidacion.MostrarValidacion("Se ha recalculado los saldos correctamente de " + ddlAnio.SelectedValue.ToString() + "-" + ddlMes.SelectedItem.Text + " hasta " + ddlAnioFinal.SelectedValue.ToString() + "-" + ddlMesFinal.SelectedItem.Text, true, Enumerador.enmTipoMensaje.INFORMATION);
                    }
                    
                    //ctrlValidacion.MostrarValidacion("No tiene Permisos para Editar", true, Enumerador.enmTipoMensaje.ERROR);
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

        private bool Validar() {
            
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ctrlOficinaConsular.SelectedIndex == 0)
                {
                    ctrlValidacion.MostrarValidacion("Seleccione la Oficina Consular.", true, Enumerador.enmTipoMensaje.ERROR);
                    return false;
                }
            }

            if (Convert.ToInt16(ddlAnio.SelectedValue) > Convert.ToInt16(ddlAnioFinal.SelectedValue))
            {
                ctrlValidacion.MostrarValidacion("El año inicial no puede ser menor al año final.", true, Enumerador.enmTipoMensaje.ERROR);
                return false;
            }
            if (Convert.ToInt16(ddlAnio.SelectedValue) <= Convert.ToInt16(ddlAnioFinal.SelectedValue))
            {
                if (Convert.ToInt16(ddlMes.SelectedValue) > Convert.ToInt16(ddlMesFinal.SelectedValue))
                {
                    ctrlValidacion.MostrarValidacion("El mes inicial no puede ser menor al mes final.", true, Enumerador.enmTipoMensaje.ERROR);
                    return false;
                }
            }
            return true;                
        }
    }
}