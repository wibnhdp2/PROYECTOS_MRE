using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmExpedienteMigratorio : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return ValidarRegistro()";

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            if (!Page.IsPostBack)
            {
                hdn_sOficinaConsularId.Value = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
                hdn_sUsuarioId.Value = Session[Constantes.CONST_SESION_USUARIO_ID].ToString();
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();

                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvExpedientesCorrelativos };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            gdvExpedientesCorrelativos.DataSource = null;
            gdvExpedientesCorrelativos.DataBind();

            CargarDatosGrilla();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlOficinaConsular.SelectedIndex = 0;
            ddlTipoDocMigratorioBusqueda.SelectedIndex = 0;
            Session["DT_EXPEDIENTES"] = new DataTable();
            gdvExpedientesCorrelativos.DataSource = new DataTable();
            gdvExpedientesCorrelativos.DataBind();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();
            HabilitarOpcionesPorAccion();
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.MODIFICAR).ToString();

            HabilitarOpcionesPorAccion();
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.ELIMINAR).ToString();
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            ExpedienteMantenimientoBL objBL = new ExpedienteMantenimientoBL();
            BE.MRE.SI_EXPEDIENTE objExpediente = new BE.MRE.SI_EXPEDIENTE();
            
            ActoMigratorioConsultaBL oActoMigratorioConsultaBL = new ActoMigratorioConsultaBL();
            objExpediente = ObtenerDatosExpediente();

            int intAccionId = Convert.ToInt32(hdn_sAccionId.Value);
            String StrScript = String.Empty;
            string strMensaje = string.Empty;

            if (intAccionId != (int)Enumerador.enmAccion.ELIMINAR)
            {
                strMensaje = oActoMigratorioConsultaBL.ExisteNumeroDocumento(objExpediente);
                if (strMensaje != string.Empty)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Expediente", strMensaje, true, 200, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            }

            switch (intAccionId)
            {
                case (int)Enumerador.enmAccion.INSERTAR:
                    objExpediente = objBL.insertar(objExpediente);
                    break;
                case (int)Enumerador.enmAccion.MODIFICAR:
                    objExpediente = objBL.actualizar(objExpediente);
                    break;
                case (int)Enumerador.enmAccion.ELIMINAR:
                    objExpediente = objBL.eliminar(objExpediente);
                    break;
                default:
                    break;
            }

            string strScript = string.Empty;
            if (!objExpediente.Error)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - EXPEDIENTES", Constantes.CONST_MENSAJE_EXITO);
                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);

                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();
                HabilitarCamposMantenimiento();
                LimpiarDatosMantenimiento();

                gdvExpedientesCorrelativos.DataSource = null;
                gdvExpedientesCorrelativos.DataBind();
                ctrlPaginador.InicializarPaginador();
                updConsulta.Update();
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - EXPEDIENTES", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            Comun.EjecutarScript(Page, Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        #region Métodos

        public void CargarListadosDesplegables()
        {
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaConsular.Cargar(false, true, "- TODAS -");
            }
            else
            {
                ddlOficinaConsular.Cargar(false, false);
            }
            Util.CargarComboAnios(ddlPeriodo, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;

            Util.CargarComboAnios(ddlPeriodoMant, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodoMant.SelectedIndex = ddlPeriodo.Items.Count - 1;

            ddlOficinaConsularMant.Cargar(false, true);

            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_DOCUMENTOS).Copy();

            var doc_Migratorio = (from dr in dt.AsEnumerable()
                                  where Convert.ToInt64(dr["id"]) != (Int64)Enumerador.enmDocumentoMigratorio.PASAPORTE
                                  select dr).CopyToDataTable();

            Util.CargarParametroDropDownList(ddlTipoDocMigraMant, doc_Migratorio, true);
            Util.CargarParametroDropDownList(ddlTipoDocMigratorioBusqueda, doc_Migratorio, true);
            
        }

        public void CargarDatosIniciales()
        {
            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_DOCUMENTOS).Copy();

            var doc_Migratorio = (from dr in dt.AsEnumerable()
                                  where Convert.ToInt64(dr["id"]) != (Int64)Enumerador.enmDocumentoMigratorio.PASAPORTE 
                                  select dr).CopyToDataTable();

            Util.CargarParametroDropDownList(ddlTipoDocMigraMant, doc_Migratorio, true);
            //HabilitarOpcionesPorAccion();
            Util.CargarParametroDropDownList(ddlTipoDocMigratorioBusqueda, doc_Migratorio, true);
            //HabilitarOpcionesPorAccion();
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
        }

        public void HabilitarOpcionesPorAccion()
        {
            string strScript = string.Empty;
            int intAccion = Convert.ToInt32(hdn_sAccionId.Value);

            switch (intAccion)
            {
                case (int)Enumerador.enmAccion.INSERTAR:
                    HabilitarCamposMantenimiento();
                    LimpiarDatosMantenimiento();

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    break;
                case (int)Enumerador.enmAccion.MODIFICAR:
                    HabilitarCamposMantenimiento();

                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = true;

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    break;
                case (int)Enumerador.enmAccion.ELIMINAR:
                    HabilitarCamposMantenimiento();

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    break;
                default:
                    HabilitarCamposMantenimiento(false);

                    ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    break;
            }
            Comun.EjecutarScript(Page, strScript);
        }

        public void HabilitarCamposMantenimiento(bool bolHabilitar = true)
        {
            ddlOficinaConsularMant.Enabled = bolHabilitar;
            ddlTipoDocMigraMant.Enabled = bolHabilitar;
            txtNroExpediente.Enabled = bolHabilitar;
        }

        public void LimpiarDatosMantenimiento()
        {
            hdn_sExpedienteId.Value = "0";
            hdn_sIndiceSeleccionado.Value = "0";

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            ddlOficinaConsularMant.SelectedIndex = 0;
            ddlTipoDocMigraMant.SelectedIndex = 0;
            txtNroExpediente.Text = string.Empty;

            HabilitarCamposMantenimiento();
        }

        public void CargarDatosGrilla()
        {
            bool bolTipoDocSeleccionado = true;

            if (ddlTipoDocMigratorioBusqueda.SelectedIndex > 0)
            {
                int intOficinaConsularSel = 0;
                if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    if (ddlOficinaConsular.SelectedIndex > 0)
                        intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                }
                else
                {
                    intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                }

                int intPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                int intTipoDocMigratorio = 0;
                if ((ddlTipoDocMigratorioBusqueda.SelectedValue) == SGAC.Accesorios.Enumerador.enmGrupo.ACTO_MIGRATORIO_DOCUMENTOS.ToString())
                {
                    if (ddlTipoDocMigratorioBusqueda.SelectedIndex > 0)
                        intTipoDocMigratorio = Convert.ToInt32(ddlTipoDocMigratorioBusqueda.SelectedValue);
                }
                else
                {
                    intTipoDocMigratorio = Convert.ToInt32(ddlTipoDocMigratorioBusqueda.SelectedValue);
                }

                ExpedienteConsultasBL objBL = new ExpedienteConsultasBL();

                int intPaginaActual = ctrlPaginador.PaginaActual;
                int intPaginaCantidad = ctrlPaginador.PageSize;
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dtExpedientes = objBL.obtener(intOficinaConsularSel, intPeriodo, intPaginaActual, intPaginaCantidad, intTipoDocMigratorio,
                    ref intTotalRegistros, ref intTotalPaginas); 

                if (dtExpedientes.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                Session["DT_EXPEDIENTES"] = dtExpedientes;
                gdvExpedientesCorrelativos.DataSource = dtExpedientes;
                gdvExpedientesCorrelativos.DataBind();

                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;
                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }
            }
            else
            {
                bolTipoDocSeleccionado = false;
            }

            if (!bolTipoDocSeleccionado)
            {
                ctrlValidacion.MostrarValidacion("Seleccione un Tipo de Documento Migratorio", true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        public BE.MRE.SI_EXPEDIENTE ObtenerDatosExpediente()
        {
            BE.MRE.SI_EXPEDIENTE objExpediente = new BE.MRE.SI_EXPEDIENTE();
            objExpediente.exp_sExpedienteId = Convert.ToInt16(hdn_sExpedienteId.Value);
            objExpediente.exp_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularMant.SelectedValue);
            objExpediente.exp_sPeriodo = Convert.ToInt16(ddlPeriodo.SelectedItem.Value.ToString());
            objExpediente.exp_sTipoDocMigId = Convert.ToInt16(ddlTipoDocMigraMant.SelectedValue);
            objExpediente.exp_INumeroExpediente = Convert.ToInt32(txtNroExpediente.Text);
            objExpediente.exp_sUsuarioCreacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objExpediente.exp_vIPCreacion = Util.ObtenerDireccionIP();
            objExpediente.exp_sUsuarioModificacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objExpediente.exp_vIPModificacion = Util.ObtenerDireccionIP();
            objExpediente.OficinaConsultar = Convert.ToInt16(hdn_sOficinaConsularId.Value);
            objExpediente.HostName = Util.ObtenerHostName();

            return objExpediente;
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarDatosGrilla();
            updConsulta.Update();
        }

        protected void gdvExpedientesCorrelativos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            hdn_sIndiceSeleccionado.Value = intSeleccionadoDet.ToString();
            string vExpediente = gdvExpedientesCorrelativos.Rows[intSeleccionadoDet].Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpedientesCorrelativos, "exp_sExpedienteId")].Text;

            if (e.CommandName == "Consultar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.CONSULTAR).ToString();

            }
            else if (e.CommandName == "Editar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.MODIFICAR).ToString();

            }
            HabilitarOpcionesPorAccion();
            PintarSeleccionado();
        }

        public void PintarSeleccionado()
        {
            DataTable dt = (DataTable)Session["DT_EXPEDIENTES"];

            if (dt == null)
                return;
            if (dt.Rows.Count == 0)
                return;

            ExpedienteConsultasBL objBL = new ExpedienteConsultasBL();
            BE.MRE.SI_EXPEDIENTE objExpediente = new BE.MRE.SI_EXPEDIENTE();

            objExpediente.exp_sExpedienteId = Convert.ToInt16(dt.Rows[Convert.ToInt32(hdn_sIndiceSeleccionado.Value)][0]);
            objExpediente.exp_sTipoDocMigId = Convert.ToInt16(dt.Rows[Convert.ToInt32(hdn_sIndiceSeleccionado.Value)][4]); 
            objExpediente = objBL.Consultar(objExpediente);
            hdn_sExpedienteId.Value = objExpediente.exp_sExpedienteId.ToString();
            hdn_sTipoDocMigra.Value = objExpediente.exp_sTipoDocMigId.ToString();
            ddlOficinaConsularMant.SelectedValue = Convert.ToString(objExpediente.exp_sOficinaConsularId);
            ddlTipoDocMigraMant.SelectedValue = Convert.ToString(objExpediente.exp_sTipoDocMigId);
            ddlPeriodoMant.SelectedValue = Convert.ToString(objExpediente.exp_sPeriodo);
            txtNroExpediente.Text = Convert.ToString(objExpediente.exp_INumeroExpediente);
            updMantenimiento.Update();
        }
        #endregion
    }
}