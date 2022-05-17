using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Almacen.BL;
using System.Data;
namespace SGAC.WebApp.Configuracion
{
    public partial class frmReactivarInsumos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                btnReactivar.Visible = false;
            }
        }
        private void LimpiarGrilla()
        {
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
            ctrlPaginador.Visible = false;
        }
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        private void CargarGrilla()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;
            InsumoConsultaBL _obj = new InsumoConsultaBL();
            DataTable _dt = new DataTable();

            _dt = _obj.ConsultarPorRangos(Convert.ToInt16(ctrlOficinaConsular.SelectedValue),
                                          txtRanIni.Text.ToString(),
                                          txtRanFin.Text.ToString(),
                                          ctrlPaginador.PaginaActual,
                                           Constantes.CONST_CANT_REGISTRO,
                                           ref intTotalRegistros,
                                           ref intTotalPaginas);
            if (_dt.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                btnReactivar.Enabled = false;
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                grReporteGestion.DataSource = _dt;
                grReporteGestion.DataBind();

                ctrlPaginador.TotalResgistros = Convert.ToInt32(intTotalRegistros);
                ctrlPaginador.TotalPaginas = Convert.ToInt32(intTotalPaginas);
                btnReactivar.Enabled = true;
                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;
            }
        }
        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }
        private void CalcularCantidad()
        {
            Int32 RangoIni = Convert.ToInt32(txtRanIni.Text);
            Int32 RangoFin = Convert.ToInt32(txtRanFin.Text);

            txtCant.Text = Convert.ToString(RangoFin - RangoIni + 1);
        
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtRanIni.Text.Length > 0 && txtRanFin.Text.Length > 0)
            {
                CargarGrilla();
                CalcularCantidad();
                updGrillaConsulta.Update();
            }
            else {
                ctrlValidacion.MostrarValidacion("Complete los datos de busqueda");
            }
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            try
            {
                Int16 CodUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                string RangoIni = txtRanIni.Text;
                string RangoFin = txtRanFin.Text;
                
                InsumoMantenimientoBL _obj = new InsumoMantenimientoBL();
                if (txtMotivo.Text.Trim().Length == 0)
                {
                    _obj.ReactivarInsumos(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), RangoIni, RangoFin, CodUsuario, null);
                }
                else {
                    _obj.ReactivarInsumos(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), RangoIni, RangoFin, CodUsuario, txtMotivo.Text);
                }

                LimpiarGrilla();

                ctrlValidacion.MostrarValidacion("Se Reactivo Correctamente", true, Enumerador.enmTipoMensaje.INFORMATION);
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarGrilla();
            ctrlValidacion.MostrarValidacion("");
        }
    }
}