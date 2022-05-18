using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmConsuladoTipoPagoTarifario : System.Web.UI.Page
    {
        #region CAMPOS
            private string strNombreEntidad = "OFICINA_TIPOPAGO";
            private string strVariableAccion = "OficinaTipoPago_Accion";
            private string strVariableDt = "OficinaTipoPago_Tabla";
            private string strVariableIndice = "OficinaTipoPago_Indice";
        #endregion

        private void Page_Init(object sender, System.EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;

            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                    GrabarHandler();
            }


            if (!Page.IsPostBack)
            {
                Session["Grabo"] = "NO";
                CargarDatosIniciales();
                CargarListadosDesplegables();
                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvOficinaTipoPagoTarifa };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void chkSeleccionarTodosTiposPago_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSeleccionarTodosTiposPago = (CheckBox)gdvTipoPago.HeaderRow.FindControl("chkSeleccionarTodosTiposPago");
            foreach (GridViewRow row in gdvTipoPago.Rows)
            {
                CheckBox chkSeleccionarTipoPago = (CheckBox)row.FindControl("chkSeleccionarTipoPago");
                if (chkSeleccionarTodosTiposPago.Checked == true)
                {
                    chkSeleccionarTipoPago.Checked = true;
                }
                else
                {
                    chkSeleccionarTipoPago.Checked = false;
                }
            }     
        }

        protected void chkSeleccionarTodasTarifas_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSeleccionarTodasTarifas = (CheckBox)gdvTarifario.HeaderRow.FindControl("chkSeleccionarTodasTarifas");
            foreach (GridViewRow row in gdvTarifario.Rows)
            {
                CheckBox chkSeleccionarTarifa = (CheckBox)row.FindControl("chkSeleccionarTarifa");
                if (chkSeleccionarTodasTarifas.Checked == true)
                {
                    chkSeleccionarTarifa.Checked = true;
                }
                else
                {
                    chkSeleccionarTarifa.Checked = false;
                }
            } 
        }

        protected void gdvOficinaTipoPagoTarifa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;

            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

                HabilitarMantenimiento(false);
                PintarSeleccionado();

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();
                ddlregOficinaConsular.Enabled = false;


                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();

            BindGrid();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();

            BindGrid();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlConsultaTipoPago.SelectedIndex = 0;
            ddlConsultaOficinaConsular.SelectedIndex = 0;
            ddlConsultaTarifa.SelectedIndex = 0;

            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            HabilitarMantenimiento(false);
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, "Elimina"));
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            string strScript = string.Empty;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlregOficinaConsular.SelectedIndex == 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar una Oficina Consular.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            //-----------------------------------------------------------
            ActualizarSesionTipoPago();
            DataTable dtListaTipoPago = new DataTable();
            dtListaTipoPago = (DataTable)Session["listaTiposPago"];
            DataView dvTiposPago = dtListaTipoPago.DefaultView;
            dvTiposPago.RowFilter = "seleccion = 1";
            DataTable dtTiposPagoSel = dvTiposPago.ToTable();

            if (dtTiposPagoSel.Rows.Count == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar por lo menos un Tipo de Pago.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            //-------------------------------------
            ActualizarSesionTarifario();
            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();

            if (dtTarifasSel.Rows.Count == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar por lo menos una Tarifa Consular.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
                       
            //-------------------------------------
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                DataTable dtOficinaTarifaTipoPago = new DataTable();
                OficinaConsularTarifarioTipoPagoDL objOficinaTarifaTipoPagoBL;

                Int16 intOficinaConsularId = 0;
                string strOficinaConsular = "";

                if (ddlregOficinaConsular.SelectedIndex > 0)
                {
                    intOficinaConsularId = Convert.ToInt16(ddlregOficinaConsular.SelectedValue);
                    strOficinaConsular = ddlregOficinaConsular.SelectedItem.Text;
                }
                Int16 intTipoPagoId = 0;
                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
                bool bExiste = false;
                string strTipoPago = "";

                for (int i = 0; i < dtTiposPagoSel.Rows.Count; i++)
                {
                    if (dtTiposPagoSel.Rows[i]["seleccion"].ToString().Equals("1"))
                    {
                        intTipoPagoId = Convert.ToInt16(dtTiposPagoSel.Rows[i]["id"].ToString());
                        strTipoPago = dtTiposPagoSel.Rows[i]["descripcion"].ToString();
                        objOficinaTarifaTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

                        dtOficinaTarifaTipoPago = objOficinaTarifaTipoPagoBL.Consultar(intOficinaConsularId, intTipoPagoId, "", false, intPaginaCantidad, 1, "S", ref IntTotalCount, ref IntTotalPages);

                        if (dtOficinaTarifaTipoPago.Rows.Count > 0)
                        {
                            bExiste = true;
                            break;
                        }
                    }
                }

                if (bExiste)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Ya existe registrado la Oficina Consular: " + strOficinaConsular + " y el Tipo de Pago: " + strTipoPago, false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            //-------------------------------------
            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            updMantenimiento.Update();

            Session["Grabo"] = "NO";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "var yes = confirm('¿Desea realizar la operación?'); if (yes) __doPostBack('GrabarHandler', 'yes');", true);

        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        protected void gdvTipoPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = (Label)e.Row.FindControl("lblID");
                CheckBox chkSeleccionarTipoPago = (CheckBox)e.Row.FindControl("chkSeleccionarTipoPago");

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaTiposPago"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["id"].ToString().Equals(lblID.Text.Trim()))
                        {
                            if (dt.Rows[i]["seleccion"].ToString().Equals("1"))
                            {
                                chkSeleccionarTipoPago.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        protected void gdvTarifario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = (Label)e.Row.FindControl("lblID");
                CheckBox chkSeleccionarTarifa = (CheckBox)e.Row.FindControl("chkSeleccionarTarifa");

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaTarifas"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tari_sTarifarioId"].ToString().Equals(lblID.Text.Trim()))
                        {
                            if (dt.Rows[i]["seleccion"].ToString().Equals("1"))
                            {
                                chkSeleccionarTarifa.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        protected void chkSoloTiposPagoSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTipopago();
        }

        protected void chkSoloTarifasSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTarifaCosto();
        }

        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            DataTable dtTipoPago = new DataTable();
            DataTable dtNorma = new DataTable();
            DataTable dtTarifario = new DataTable();


            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                //ctrlOficinaConsular.Cargar(true, false);
                DataTable _dt = new DataTable();

                _dt = Comun.ObtenerOficinasActivas(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));

                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ddlConsultaOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- TODOS -");
                Util.CargarDropDownList(ddlregOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- SELECCIONAR -");
            }
            else
            {
                ddlConsultaOficinaConsular.Cargar(false, false);
                ddlregOficinaConsular.Cargar(false, false);
            }

            //-------------------------------------------------------
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

            DataView dvTipoPago = dtTipoPago.DefaultView;
            dvTipoPago.Sort = "descripcion";
            DataTable dtTipoPagoOrdenada = dvTipoPago.ToTable();

            Util.CargarParametroDropDownList(ddlConsultaTipoPago, dtTipoPagoOrdenada, true, "- TODOS -");
            DataTable dtListaTiposPago = new DataTable();
            dtListaTiposPago = CrearlistaTiposPago(dtTipoPagoOrdenada);


            Session["listaTiposPago"] = dtListaTiposPago;
            FiltroExcepcionTipopago(true);
            //----------------------------------------------
            //------------------------------------------------
            //Fecha: 03/04/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Mostrar todas las tarifas consulares
            //------------------------------------------------
            object[] arrParametros = { 0, "", 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 500, 0, 0 };


            dtTarifario = comun_Part2.ObtenerTarifarioConsulta(Session, ref arrParametros);

            Util.CargarDropDownList(ddlConsultaTarifa, dtTarifario, "tari_vdescripcioncorta", "tari_starifarioId", true, "- TODOS -");

            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);

            Session["listaTarifas"] = dtListaTarifas;
            FiltroExcepcionTarifaCosto(true);
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlregOficinaConsular.Enabled = bolHabilitar;
        }


        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_TipoPagoId.Value = "";
            HF_OficinaConsularId.Value = "";

            ddlregOficinaConsular.SelectedIndex = 0;


            gdvTipoPago.DataSource = null;
            gdvTipoPago.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            chkSoloTarifasSeleccionadas.Checked = false;
            chkSoloTarifasSeleccionadas.Text = "Mostrar solo las seleccionadas";
            chkSoloTiposPagoSeleccionadas.Checked = false;
            chkSoloTiposPagoSeleccionadas.Text = "Mostrar solo las seleccionadas";

            chkExcepcionTarifa.Checked = false;
            chkExcepcionTipoPago.Checked = false;
            chkTarifaConCosto.Checked = false;
            chkTarifaSinCosto.Checked = false;
            //------------------------------------------------
            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);
            DataView dvTipoPago = dtTipoPago.DefaultView;
            dvTipoPago.Sort = "descripcion";
            DataTable dtTipoPagoOrdenada = dvTipoPago.ToTable();
            DataTable dtListaTiposPago = new DataTable();
            dtListaTiposPago = CrearlistaTiposPago(dtTipoPagoOrdenada);

            Session["listaTiposPago"] = dtListaTiposPago;
            FiltroExcepcionTipopago(true);
            
            //------------------------------------------------
            DataTable dtTarifario = new DataTable();
            //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];
            dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);

            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);

            Session["listaTarifas"] = dtListaTarifas;
            FiltroExcepcionTarifaCosto(true);
            //------------------------------------------------

            updMantenimiento.Update();
            updConsulta.Update();
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            if (Session != null)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                return ((DataTable)Session[strVariableDt]).Rows[intSeleccionado];
            }

            return null;
        }

        private void PintarSeleccionado()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                if (drSeleccionado != null)
                {
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                    chkSoloTiposPagoSeleccionadas.Checked = false;
                    chkSoloTarifasSeleccionadas.Checked = false;

                    //---------------------------------------------------------------
                    HF_OficinaConsularId.Value = drSeleccionado["ofpa_sOficinaConsularId"].ToString();
                    Int16 intOficinaConsularId = Convert.ToInt16(HF_OficinaConsularId.Value);

                    ddlregOficinaConsular.SelectedValue = HF_OficinaConsularId.Value;

                    HF_TipoPagoId.Value = drSeleccionado["ofpa_sPagoTipoId"].ToString();
                    Int16 intTipoPagoId = Convert.ToInt16(HF_TipoPagoId.Value);
                    
                    bool bExcepcion = chkconsultaExcepcion.Checked;
                    //---------------------------------------------------------------
                    DataTable dtTipoPago = new DataTable();

                    dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

                    DataView dvTipoPago = dtTipoPago.DefaultView;
                    dvTipoPago.Sort = "descripcion";
                    DataTable dtTipoPagoOrdenada = dvTipoPago.ToTable();
                    
                    DataTable dtListaTiposPago = new DataTable();
                    dtListaTiposPago = CrearlistaTiposPago(dtTipoPagoOrdenada);
                    //---------------------------------------------------------------
                    DataTable dtTarifario = new DataTable();
                    //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];
                    dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);

                    DataTable dtListaTarifas = new DataTable();

                    dtListaTarifas = CrearListaTarifas(dtTarifario);
                    //---------------------------------------------------------------
                    DataTable dtOficinaTarifarioTipoPago = new DataTable();

                    OficinaConsularTarifarioTipoPagoDL objOficinaTarifarioTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    dtOficinaTarifarioTipoPago = objOficinaTarifarioTipoPagoBL.Consultar(intOficinaConsularId, intTipoPagoId, "", bExcepcion, 20000, 1, "S", ref IntTotalCount, ref IntTotalPages);

                    DataTable dtTiposPagoSel = dtOficinaTarifarioTipoPago.DefaultView.ToTable(true, "ofpa_sPagoTipoId");
                    DataTable dtTarifarioSel = dtOficinaTarifarioTipoPago.DefaultView.ToTable(true, "ofpa_sTarifarioId");

                    if (dtOficinaTarifarioTipoPago.Rows.Count > 0)
                    {
                        #region ExisteNormaTarifario

                        Int16 intTarifaId = 0;
                        intTipoPagoId = 0;

                        //-----------------------------------------------------------------
                        for (int i = 0; i < dtTiposPagoSel.Rows.Count; i++)
                        {
                            intTipoPagoId = Convert.ToInt16(dtTiposPagoSel.Rows[i]["ofpa_sPagoTipoId"].ToString());

                            for (int x = 0; x < dtListaTiposPago.Rows.Count; x++)
                            {
                                if (intTipoPagoId == Convert.ToInt16(dtListaTiposPago.Rows[x]["id"].ToString()))
                                {
                                    dtListaTiposPago.Rows[x]["seleccion"] = "1";
                                    break;
                                }
                            }
                        }
                        //-----------------------------------------------------------------
                        for (int i = 0; i < dtTarifarioSel.Rows.Count; i++)
                        {
                            intTarifaId = Convert.ToInt16(dtTarifarioSel.Rows[i]["ofpa_sTarifarioId"].ToString());

                            for (int x = 0; x < dtListaTarifas.Rows.Count; x++)
                            {
                                if (intTarifaId == Convert.ToInt16(dtListaTarifas.Rows[x]["tari_sTarifarioId"].ToString()))
                                {
                                    dtListaTarifas.Rows[x]["seleccion"] = "1";
                                    break;
                                }
                            }
                        }


                        //-----------------------------------------------------------------
                        #endregion
                    }
                    //-----------------------------------------------------

                    chkSoloTiposPagoSeleccionadas.Checked = true;
                    //-----------------------------------------------------
                    Session["listaTiposPago"] = dtListaTiposPago;
                    //-----------------------------------------------------
                    chkSoloTiposPagoSeleccionadas.Text = "Todas los Tipos de Pago";
                    DataTable dtTiposPago = new DataTable();
                    dtTiposPago = (DataTable)Session["listaTiposPago"];
                    DataView dvTiposPago = dtTiposPago.DefaultView;

                    dvTiposPago.RowFilter = "seleccion = 1";
                    DataTable dtListaTiposPagoFiltradas = dvTiposPago.ToTable();

                    //-----------------------------------------------------
                    gdvTipoPago.DataSource = dtListaTiposPagoFiltradas;
                    gdvTipoPago.DataBind();
                    //===========================================================

                    Session["listaTarifas"] = dtListaTarifas;
                    //-----------------------------------------------------
                    chkSoloTarifasSeleccionadas.Checked = true;
                    chkSoloTarifasSeleccionadas.Text = "Todas las tarifas";
                    DataTable dtTarifas = new DataTable();
                    dtTarifas = (DataTable)Session["listaTarifas"];
                    DataView dvTarifas = dtTarifas.DefaultView;

                    dvTarifas.RowFilter = "seleccion = 1";

                    DataTable dtListaTarifasFiltrada = dvTarifas.ToTable();
                    //-----------------------------------------------------

                    gdvTarifario.DataSource = dtListaTarifasFiltrada;
                    gdvTarifario.DataBind();


                    if (Convert.ToBoolean(drSeleccionado["FlagExcepcionTipoPago"].ToString()) == true)
                    {
                        chkExcepcionTipoPago.Checked = true;
                        FiltroExcepcionTipopago();
                    }
                    if (Convert.ToBoolean(drSeleccionado["FlagExcepcionTarifa"].ToString()) == true)
                    {
                        chkExcepcionTarifa.Checked = true;
                        FiltroExcepcionTarifaCosto();
                    }

                    //-----------------------------------------------------
                    updMantenimiento.Update();
                }
            }
        }

        private SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO objParametro = new BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    //Editar o Anular
                    objParametro.ofpa_iOficinaTipoPagoId = 0;
                    objParametro.ofpa_sPagoTipoId = Convert.ToInt16(HF_TipoPagoId.Value);
                    objParametro.ofpa_sTarifarioId = 0;
                    objParametro.ofpa_sOficinaConsularId = Convert.ToInt16(ddlregOficinaConsular.SelectedValue);
                }
                objParametro.ofpa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ofpa_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.ofpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ofpa_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                return objParametro;
            }

            return null;
        }

        private void BindGrid()
        {
            DataTable dtOficinaTarifaTipoPago = new DataTable();
            OficinaConsularTarifarioTipoPagoDL objOficinaTarifaTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            Int16 intOficinaConsularId = 0;
            Int16 intTipoPagoId = 0;
            string strTarifaLetra = "";
            bool bExcepcion = false;
            //-----------------------------------------------------
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlConsultaOficinaConsular.SelectedIndex > 0)
                {
                    intOficinaConsularId = Convert.ToInt16(ddlConsultaOficinaConsular.SelectedValue);
                }
            }
            else
            {
                intOficinaConsularId = Convert.ToInt16(ddlConsultaOficinaConsular.SelectedValue);
            }
            if (ddlConsultaTipoPago.SelectedIndex > 0)
            {
                intTipoPagoId = Convert.ToInt16(ddlConsultaTipoPago.SelectedValue);
            }

            if (ddlConsultaTarifa.SelectedIndex > 0)
            {
                string strDescripcion = ddlConsultaTarifa.SelectedItem.Text;

                strTarifaLetra = strDescripcion.Substring(0, strDescripcion.IndexOf(" ")).Trim();
            }

            bExcepcion = chkconsultaExcepcion.Checked;

            dtOficinaTarifaTipoPago = objOficinaTarifaTipoPagoBL.Consultar(intOficinaConsularId, intTipoPagoId, strTarifaLetra, bExcepcion, intPaginaCantidad, ctrlPaginador.PaginaActual, "S", ref IntTotalCount, ref IntTotalPages);

            Session[strVariableDt] = dtOficinaTarifaTipoPago;

            if (dtOficinaTarifaTipoPago.Rows.Count > 0)
            {
                gdvOficinaTipoPagoTarifa.SelectedIndex = -1;
                gdvOficinaTipoPagoTarifa.DataSource = dtOficinaTarifaTipoPago;
                gdvOficinaTipoPagoTarifa.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(IntTotalCount), true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                ctrlPaginador.Visible = false;
                ctrlPaginador.PaginaActual = 1;
                ctrlPaginador.InicializarPaginador();

                gdvOficinaTipoPagoTarifa.DataSource = null;
                gdvOficinaTipoPagoTarifa.DataBind();
            }
            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPagoBE = new BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO();

            string strScript = string.Empty;

            OficinaConsularTarifarioTipoPagoDL objOficinaTarifarioTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

            List<SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO> listaOficinaTarifaTipoPago = new List<BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO>();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:

                    ActualizarSesionTipoPago();
                    ActualizarSesionTarifario();
                    listaOficinaTarifaTipoPago = ObtenerListaOficinaTarifaTipoPago();

                    objOficinaTarifaTipoPagoBE = objOficinaTarifarioTipoPagoBL.Insertar(listaOficinaTarifaTipoPago);

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    objOficinaTarifaTipoPagoBE = objOficinaTarifarioTipoPagoBL.Anular(ObtenerEntidadMantenimiento());
                    if (objOficinaTarifaTipoPagoBE.Error == false)
                    {
                        ActualizarSesionTipoPago();
                        ActualizarSesionTarifario();
                        listaOficinaTarifaTipoPago = ObtenerListaOficinaTarifaTipoPago();
                        objOficinaTarifaTipoPagoBE = objOficinaTarifarioTipoPagoBL.Insertar(listaOficinaTarifaTipoPago);
                    }

                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    objOficinaTarifaTipoPagoBE = objOficinaTarifarioTipoPagoBL.Anular(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }
            if (objOficinaTarifaTipoPagoBE.Error == false)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                Session["Grabo"] = "SI";

                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                Session[strVariableDt] = new DataTable();
                BindGrid();

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
            }
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, strScript);
        }

        private DataTable CrearTablaTipoPago()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id", typeof(Int16));
            dt.Columns.Add("descripcion", typeof(String));
            dt.Columns.Add("FlagExcepcion", typeof(Int16));
            dt.Columns.Add("seleccion", typeof(Int16));
            return dt;
        }
        private DataTable CrearTablaTarifario()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("tari_sTarifarioId", typeof(Int16));
            dt.Columns.Add("tarifa", typeof(String));
            dt.Columns.Add("tari_FCosto", typeof(float));
            dt.Columns.Add("FlagExcepcion", typeof(Int16));
            dt.Columns.Add("seleccion", typeof(Int16));

            return dt;
        }
        private DataTable CrearListaTarifas(DataTable dtTarifario)
        {
            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearTablaTarifario();

            DataRow dr;
            string strTarifa = "";
            string strDescripcion = "";
            float intCosto = 0;
            bool bExcepcion = false;
            string strLetra = "";

            for (int i = 0; i < dtTarifario.Rows.Count; i++)
            {
                dr = dtListaTarifas.NewRow();

                dr["tari_sTarifarioId"] = dtTarifario.Rows[i]["tari_sTarifarioId"].ToString();

                strDescripcion = dtTarifario.Rows[i]["tari_vdescripcioncorta"].ToString();
                intCosto = Convert.ToInt64(dtTarifario.Rows[i]["tari_FCosto"].ToString());

                strLetra = dtTarifario.Rows[i]["tari_vLetra"].ToString().Trim();

                if (strLetra.Length == 0)
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim();
                }
                else
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim() + "-" + strLetra;
                }
                
                bExcepcion = Convert.ToBoolean(dtTarifario.Rows[i]["tari_bFlagExcepcion"].ToString());

                dr["tarifa"] = strTarifa;
                dr["tari_FCosto"] = intCosto;
                if (bExcepcion)
                {
                    dr["FlagExcepcion"] = "1";
                }
                else
                {
                    dr["FlagExcepcion"] = "0";
                }
                dr["seleccion"] = "0";

                dtListaTarifas.Rows.Add(dr);
            }
            return dtListaTarifas;
        }

        private DataTable CrearlistaTiposPago(DataTable dtTipoPago)
        {
            DataTable dtListaTiposPago = new DataTable();

            dtListaTiposPago = CrearTablaTipoPago();

            DataRow dr;
            bool bExcepcion = false;

            for (int i = 0; i < dtTipoPago.Rows.Count; i++)
            {
                dr = dtListaTiposPago.NewRow();

                dr["id"] = dtTipoPago.Rows[i]["id"].ToString();
                dr["descripcion"] = dtTipoPago.Rows[i]["descripcion"].ToString();
                bExcepcion = Convert.ToBoolean(dtTipoPago.Rows[i]["bFlagExcepcion"].ToString());

                if (bExcepcion)
                {
                    dr["FlagExcepcion"] = "1";
                }
                else
                {
                    dr["FlagExcepcion"] = "0";
                }
                dr["seleccion"] = "0";
                dtListaTiposPago.Rows.Add(dr);
            }

            return dtListaTiposPago;
        }

        private void ActualizarSesionTipoPago()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTiposPago"];

            for (int x = 0; x < gdvTipoPago.Rows.Count; x++)
            {
                GridViewRow row = gdvTipoPago.Rows[x];
                CheckBox chkSeleccionarTipoPago = (CheckBox)row.FindControl("chkSeleccionarTipoPago");
                Int16 intTipoPagoId = Convert.ToInt16(gdvTipoPago.DataKeys[x].Value);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (intTipoPagoId == Convert.ToInt16(dt.Rows[i]["id"].ToString()))
                    {
                        if (chkSeleccionarTipoPago.Checked)
                        {
                            dt.Rows[i]["seleccion"] = "1";
                        }
                        else
                        {
                            dt.Rows[i]["seleccion"] = "0";
                        }
                        break;
                    }
                }
            }
            Session["listaTiposPago"] = dt;
        }
        private void ActualizarSesionTarifario()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTarifas"];

            for (int x = 0; x < gdvTarifario.Rows.Count; x++)
            {
                GridViewRow row = gdvTarifario.Rows[x];
                CheckBox chkSeleccionarTarifa = (CheckBox)row.FindControl("chkSeleccionarTarifa");
                Int16 intTarifarioId = Convert.ToInt16(gdvTarifario.DataKeys[x].Value);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (intTarifarioId == Convert.ToInt16(dt.Rows[i]["tari_sTarifarioId"].ToString()))
                    {
                        if (chkSeleccionarTarifa.Checked)
                        {
                            dt.Rows[i]["seleccion"] = "1";
                        }
                        else
                        {
                            dt.Rows[i]["seleccion"] = "0";
                        }
                        break;
                    }
                }
            }
            Session["listaTarifas"] = dt;
        }
        private List<SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO> ObtenerListaOficinaTarifaTipoPago()
        {
            SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO BE;
            List<SGAC.BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO> listaNormaTarifario = new List<BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO>();

            //--------------------------------------------------------
            DataTable dtListaTiposPago = new DataTable();
            dtListaTiposPago = (DataTable)Session["listaTiposPago"];
            DataView dvTiposPago = dtListaTiposPago.DefaultView;
            dvTiposPago.RowFilter = "seleccion = 1";
            DataTable dtTiposPagoSel = dvTiposPago.ToTable();
            //--------------------------------------------------------
            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();
            //--------------------------------------------------------

            Int16 intTarifaId = 0;
            Int16 intOficinaConsularId = 0;
            Int16 intTipoPagoId = 0;

            if (ddlregOficinaConsular.SelectedIndex > 0)
            {
                intOficinaConsularId = Convert.ToInt16(ddlregOficinaConsular.SelectedValue);
            }


            for (int i = 0; i < dtTiposPagoSel.Rows.Count; i++)
            {
                intTipoPagoId = Convert.ToInt16(dtTiposPagoSel.Rows[i]["id"].ToString());

                for (int x = 0; x < dtTarifasSel.Rows.Count; x++)
                {
                    intTarifaId = Convert.ToInt16(dtTarifasSel.Rows[x]["tari_sTarifarioId"].ToString());

                    BE = new BE.MRE.SI_OFICINA_TARIFA_TIPO_PAGO();

                    BE.ofpa_sOficinaConsularId = intOficinaConsularId;
                    BE.ofpa_sTarifarioId = intTarifaId;
                    BE.ofpa_sPagoTipoId = intTipoPagoId;
                    BE.ofpa_cEstado = "A";                                
                    BE.ofpa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE.ofpa_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    BE.ofpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE.ofpa_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    listaNormaTarifario.Add(BE);
                }               
            }
                     
            return listaNormaTarifario;
        }

        protected void chkExcepcionTipoPago_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTipopago();
        }

        protected void chkExcepcionTarifa_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTarifaCosto();
        }

        protected void chkTarifaConCosto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTarifaConCosto.Checked)
            {
                chkTarifaSinCosto.Checked = !(chkTarifaConCosto.Checked);
            }
            FiltroExcepcionTarifaCosto();
        }

        protected void chkTarifaSinCosto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTarifaSinCosto.Checked)
            {
                chkTarifaConCosto.Checked = !(chkTarifaSinCosto.Checked);
            }
            FiltroExcepcionTarifaCosto();
        }

        private void FiltroExcepcionTarifaCosto(bool bEsNuevo = false)
        {
            bool bExcepcion = chkExcepcionTarifa.Checked;
            bool bTarifaConCosto = chkTarifaConCosto.Checked;
            bool bTarifaSinCosto = chkTarifaSinCosto.Checked;
            bool bTarifasSeleccionadas = chkSoloTarifasSeleccionadas.Checked;
            string strTarifasSeleccionadas = "";

            if (bTarifasSeleccionadas)
            {
                strTarifasSeleccionadas = " and seleccion = 1";
                chkSoloTarifasSeleccionadas.Text = "Todas las tarifas";
            }
            else
            {
                chkSoloTarifasSeleccionadas.Text = "Mostrar solo las seleccionadas";
            }

            if (bEsNuevo == false)
            {
                ActualizarSesionTarifario();
            }

            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTarifas"];
            DataView dv = dt.DefaultView;

            if (bExcepcion)
            {
                dv.RowFilter = "FlagExcepcion = 1" + strTarifasSeleccionadas;

                if (bTarifaConCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 1 and tari_FCosto > 0" + strTarifasSeleccionadas;
                }
                if (bTarifaSinCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 1 and tari_FCosto = 0" + strTarifasSeleccionadas;
                }
            }
            else
            {
                dv.RowFilter = "FlagExcepcion = 0" + strTarifasSeleccionadas;
                if (bTarifaConCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 0 and tari_FCosto > 0" + strTarifasSeleccionadas;
                }
                if (bTarifaSinCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 0 and tari_FCosto = 0" + strTarifasSeleccionadas;
                }
            }
            DataTable dtListaTarifasFiltradas = dv.ToTable();
            gdvTarifario.DataSource = dtListaTarifasFiltradas;
            gdvTarifario.DataBind();
        }

        private void FiltroExcepcionTipopago(bool bEsNuevo= false)
        {
            bool bExcepcion = chkExcepcionTipoPago.Checked;
            bool bSoloTiposPagosSeleccionadas = chkSoloTiposPagoSeleccionadas.Checked;
            string strTiposPagosSeleccionadas = "";

            if (bSoloTiposPagosSeleccionadas)
            {
                chkSoloTiposPagoSeleccionadas.Text = "Todos los Tipos de Pago";
                strTiposPagosSeleccionadas = " and seleccion = 1";
            }
            else
            {
                chkSoloTiposPagoSeleccionadas.Text = "Mostrar solo las seleccionadas";
            }
            
            if (bEsNuevo == false)
            {
                ActualizarSesionTipoPago();
            }


            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTiposPago"];
            DataView dv = dt.DefaultView;

            if (bExcepcion)
            {
                dv.RowFilter = "FlagExcepcion = 1" + strTiposPagosSeleccionadas;
            }
            else
            {
                dv.RowFilter = "FlagExcepcion = 0" + strTiposPagosSeleccionadas;
            }
            DataTable dtListaTiposPagoFiltradas = dv.ToTable();
            gdvTipoPago.DataSource = dtListaTiposPagoFiltradas;
            gdvTipoPago.DataBind();
        }
    }
}