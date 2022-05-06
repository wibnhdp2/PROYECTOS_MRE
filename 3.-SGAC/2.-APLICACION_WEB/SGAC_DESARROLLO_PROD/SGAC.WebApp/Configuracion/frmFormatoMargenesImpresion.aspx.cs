using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using System.Data;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Maestro.BL;
namespace SGAC.WebApp.Configuracion
{
    public partial class frmFormatoMargenesImpresion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
            if (!Page.IsPostBack)
            {
                hdn_sOficinaConsularId.Value = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
                ctrlPaginador.InicializarPaginador();
                CargarListadosDesplegables();
                Limpiar();
            }
            
        }
        private void CargarListadosDesplegables()
        {
            ddlOficinaConsularConsulta.Cargar(false, false);
            ddlOficinaConsularRegistro.Cargar(false, false);
            DataTable dtTipDocumentos = comun_Part1.ObtenerParametrosPorGrupo(Session, "DOCUMENTOS-IMPRESION");
            Util.CargarParametroDropDownList(ddlDocumento, dtTipDocumentos, true, " - SELECCIONE - ");
            Util.CargarParametroDropDownList(ddlDocumentoConsulta, dtTipDocumentos, true, " - TODOS - ");
        }
        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            gdvDocumentos.DataSource = null;
            gdvDocumentos.DataBind();
            CargarGrilla();
        }
        private void Limpiar()
        {
            hCodigo.Value = "0";
            ddlDocumento.SelectedIndex = 0;
            ddlMargenIzquierdo.SelectedValue = "0";
            ddlMargenSuperior.SelectedValue = "0";
            ddlSeccion.SelectedValue = "1";
            gdvDocumentos.DataSource = null;
            gdvDocumentos.DataBind();
            txtDescripcion.Text = "";
            updConsulta.Update();
        }
        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            Limpiar();
        }
        private void CargarGrilla()
        {
            try
            {
                ctrlValidacion.Visible = false;
                Int16 sOficinaConsular = 0;
                Int16 sTipDoc = 0;

                sOficinaConsular = Convert.ToInt16(ddlOficinaConsularConsulta.SelectedValue);
                if (ddlDocumentoConsulta.SelectedIndex > 0)
                {
                    sTipDoc = Convert.ToInt16(ddlDocumentoConsulta.SelectedValue);
                }
                TablaMaestraConsultaBL Obj = new TablaMaestraConsultaBL();
                DataTable dt= Obj.ConsultarMargenesDocumento(sOficinaConsular, sTipDoc);

                if (dt.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO, true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                gdvDocumentos.DataSource = dt;
                gdvDocumentos.DataBind();
            }
            catch
            { 
            
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
            BloquearCampos();
        }

        protected void btnGrabarCorrelativa_Click(object sender, EventArgs e)
        {
            try
            {
                long iCorrelativo = 0;
                Int16 sTipoDocumento = 0;
                Int16 sOficinaConsular = 0;
                byte sSeccion = 0;

                int resultado = 0;
                iCorrelativo = Convert.ToInt64(hCodigo.Value);
                sTipoDocumento = Convert.ToInt16(ddlDocumento.SelectedValue);
                sOficinaConsular = Convert.ToInt16(ddlOficinaConsularRegistro.SelectedValue);
                sSeccion = Convert.ToByte(ddlSeccion.SelectedValue);

                TablaMaestraMantenimientoBL obj = new TablaMaestraMantenimientoBL();

                resultado = obj.RegistrarMargenImpresion(iCorrelativo, sTipoDocumento, sOficinaConsular,
                                            sSeccion,txtDescripcion.Text,Convert.ToInt16(ddlMargenSuperior.SelectedValue),
                                            Convert.ToInt16(ddlMargenIzquierdo.SelectedValue), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()));
                if (resultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    Limpiar();
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "GRABAR", "Se registro Correctamente", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
                else {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "GRABAR", "Ocurrio un error", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }

            }
            catch
            {

            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int resultado = 0;
                long iCorrelativo = 0;
                iCorrelativo = Convert.ToInt64(hCodigo.Value);
                TablaMaestraMantenimientoBL obj = new TablaMaestraMantenimientoBL();
                resultado = obj.EliminarMargenImpresion(iCorrelativo, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()));

                if (resultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    Limpiar();
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "GRABAR", "Se registro Correctamente", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
                else
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "GRABAR", "Ocurrio un error", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            catch
            {

            }
        }

        private void BloquearCampos()
        { 
            long iCorrelativo = 0;
            iCorrelativo = Convert.ToInt64(hCodigo.Value);

            string accion;
            accion = hEditarVer.Value;

            if (iCorrelativo == 0) // SI ES NUEVO
            {
                ddlDocumento.Enabled = true;
                ddlOficinaConsularRegistro.Enabled = true;
                ddlSeccion.Enabled = true;
                txtDescripcion.Enabled = true;
                ddlMargenSuperior.Enabled = true;
                ddlMargenIzquierdo.Enabled = true;
            }
            else { // EDITAR O VER
                if (accion == "E")
                {
                    ddlDocumento.Enabled = false;
                    ddlOficinaConsularRegistro.Enabled = false;
                    ddlSeccion.Enabled = false;
                    txtDescripcion.Enabled = true;
                    ddlMargenSuperior.Enabled = true;
                    ddlMargenIzquierdo.Enabled = true;
                    btnGrabarCorrelativa.Enabled = true;
                    btnEliminar.Enabled = true;
                }
                else {
                    ddlDocumento.Enabled = false;
                    ddlOficinaConsularRegistro.Enabled = false;
                    ddlSeccion.Enabled = false;
                    txtDescripcion.Enabled = false;
                    ddlMargenSuperior.Enabled = false;
                    ddlMargenIzquierdo.Enabled = false;
                    btnGrabarCorrelativa.Enabled = false;
                    btnEliminar.Enabled = false;
                }
            }
        }
        protected void gdvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gdvDocumentos.Rows[intSeleccionado];

            long iCorrelativo = Convert.ToInt64(row.Cells[0].Text);
            hCodigo.Value = iCorrelativo.ToString();
            if (e.CommandName == "Consultar")
            {
                hEditarVer.Value = "C";
                BloquearCampos();

                PintarSeleccionado(iCorrelativo);

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                hEditarVer.Value = "E";
                BloquearCampos();
                PintarSeleccionado(iCorrelativo);

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);   
            }
            Comun.EjecutarScript(Page, strScript);
        }

        private void PintarSeleccionado(long icorrelativo)
        {

            TablaMaestraConsultaBL obj = new TablaMaestraConsultaBL();
            DataTable dt = obj.ObtenerMargenesDocumento(icorrelativo);

            ddlDocumento.SelectedValue = dt.Rows[0]["mado_sTipDocImpresion"].ToString();
            ddlOficinaConsularRegistro.SelectedValue = dt.Rows[0]["mado_sOficinaConsular"].ToString();
            ddlSeccion.SelectedValue = dt.Rows[0]["mado_sSeccion"].ToString();
            txtDescripcion.Text = dt.Rows[0]["mado_vDescripcion"].ToString();
            ddlMargenSuperior.SelectedValue = dt.Rows[0]["mado_sMargenSuperior"].ToString();
            ddlMargenIzquierdo.SelectedValue = dt.Rows[0]["mado_sMargenIzquierdo"].ToString();
            updMantenimiento.Update();
        }

    }
}