using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL;
using SGAC.BE;
using SGAC.BE.MRE;

namespace SGAC.WebApp.Configuracion
{
    public partial class Tarifario : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "TARIFARIO";
        private string strVariableAccion = "Tarifario_Accion";
        private string strVariableDt = "Tarifario_Tabla";
        private string strVariableIndice = "Tarifario_Indice";
       
        private string strVariableAccionDet = "TarifarioRquisito_Accion";
        private string strVariableDtDet = "TarifarioRquisito_Tabla";
        private string strVariableIndiceDet = "TarifarioRquisito_Indice";
        #endregion

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";  

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            this.txtFecInicio.StartDate = new DateTime(1900, 1, 1);
            this.txtFecInicio.EndDate = DateTime.Now.AddDays(1);
            this.txtFecFin.StartDate = DateTime.Now;
            this.txtFecFin.AllowFutureDate = true;

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                    GrabarHandler();
            }

            if (!Page.IsPostBack)
            {
                Session["Grabo"] = "NO";

                CargarListadosDesplegables();
                CargarDatosIniciales();

                ddlEstadoConsulta.SelectedValue = "33"; //ACTIVO
                ddl_estado.SelectedValue = "33"; //ACTIVO
                ddl_estado.Enabled = false;

                Session["iOperTarifDet"] = true;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvTarifario, gdvTarifarioRequisito};
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void ddlSeccionConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSeccionConsulta.SelectedValue == "0")
            {
                ctrlToolBarConsulta_btnCancelarHandler();
            }
        }

        protected void gdvTarifario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);            

            TarifarioConsultasBL BL = new TarifarioConsultasBL();

            Session["iOperTarifDet"] = false;

            Session[strVariableIndice] = intSeleccionado;

            Int16 intTarifaId = Convert.ToInt16(gdvTarifario.Rows[intSeleccionado].Cells[0].Text);

            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;                

                HabilitarMantenimiento(false);
                PintarSeleccionado();
                
                ddl_estado.Enabled = false;

                /*Llena el historial de la tarifa*/
                pnlHistorico.Visible = true;

                DataTable dt = BL.Obtener(intTarifaId);
                if (dt.Rows.Count > 0)
                {
                    gdvHistorico.DataSource = dt;
                    gdvHistorico.DataBind();
                }
                else
                {
                    dt = FillEmptyDatagdvHistorico();
                    gdvHistorico.DataSource = dt;
                    gdvHistorico.DataBind();
                }

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                
                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();                

                /*Llena el historial de la tarifa*/
                pnlHistorico.Visible = true;

                DataTable dt = BL.Obtener(intTarifaId);
                if (dt.Rows.Count > 0)
                {
                    gdvHistorico.DataSource = dt;
                    gdvHistorico.DataBind();
                }
                else
                {
                    dt = FillEmptyDatagdvHistorico();
                    gdvHistorico.DataSource = dt;
                    gdvHistorico.DataBind(); 
                }

                ddlSeccionMant.Enabled = false;
                txtTarifaNumeroMant.Enabled = false;
                txtTarifaLetraMant.Enabled = false;

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        }           

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();
            BindGrid(ddlSeccionConsulta.SelectedValue, txtDescripcionConsulta.Text, ddlEstadoConsulta.SelectedValue);
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();
            BindGrid(ddlSeccionConsulta.SelectedValue, txtDescripcionConsulta.Text.ToUpper(), ddlEstadoConsulta.SelectedValue);           
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            ddlEstadoConsulta.SelectedValue = ((int)Enumerador.enmTarifarioEstado.ACTIVO).ToString();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;            
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();

            ddlEstadoConsulta.SelectedValue = ((int) Enumerador.enmTarifarioEstado.ACTIVO).ToString();

            Session["iOperTarifDet"] = true;

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;           
            HabilitarMantenimiento();
            ddlSeccionMant.Enabled = false;
            txtTarifaNumeroMant.Enabled = false;
            txtTarifaLetraMant.Enabled = false;

            ddl_estado.Enabled = true;

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));

            updMantenimiento.Update();
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

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (txtFecInicio.Text != string.Empty && txtFecInicio.Text == string.Empty)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No ha establecido correctamente el rango de vigencia del parámetro");
                Comun.EjecutarScript(Page, strScript);                
                return;
            }

            if (txtFecInicio.Text == string.Empty && txtFecFin.Text != string.Empty)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No ha establecido correctamente el rango de vigencia del parámetro");
                Comun.EjecutarScript(Page, strScript);                
                return;
            }

            if (txtFecInicio.Text != string.Empty && txtFecFin.Text != string.Empty)
            {
                if (!DateTime.TryParse(txtFecInicio.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(txtFecInicio.Text);
                }

                if (!DateTime.TryParse(txtFecFin.Text, out datFechaFin))
                {
                    datFechaFin = Comun.FormatearFecha(txtFecFin.Text);
                }

                if (datFechaInicio > datFechaFin)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_VALIDACION_DOS_FECHAS);
                    Comun.EjecutarScript(Page, strScript);                    
                    return;
                }
            }           
            
            TarifarioConsultasBL BLc = new TarifarioConsultasBL();

            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                int IntRpta = BLc.Existe(0, txtTarifaNumeroMant.Text, txtTarifaLetraMant.Text, 1);

                if (IntRpta == 1)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Tarifario", "Ya existe el codigo del tarifario que esta consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }

            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            updMantenimiento.Update();

            Session["Grabo"] = "NO";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "var yes = confirm('¿Desea realizar la operación?'); if (yes) __doPostBack('GrabarHandler', 'yes');", true);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
            ctrlToolBarMantenimiento_btnNuevoHandler();
        }

        #region Requisitos
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[strVariableDtDet];
            DataRow dr;
            if (dt == null)
            {
                dt = CrearDtRegTarifarioRequisito();
            }

            int intTarifarioRequisitoSel = Convert.ToInt32(Session[strVariableIndiceDet]);
            if (intTarifarioRequisitoSel == -1)
            {
                dr = dt.NewRow();
                dr["tare_sTarifarioRequisitoId"] = 0;
                dr["tare_sTarifarioId"] = 0;
                dr["tare_sRequisitoId"] = Convert.ToInt16(ddlRequisitoMant.SelectedValue);
                dr["tare_vRequisito"] = ddlRequisitoMant.SelectedItem.Text;
                dr["tare_sTipoActaId"] = Convert.ToInt16(ddlTipoActaMant.SelectedValue);
                dr["tare_vTipoActa"] = ddlTipoActaMant.SelectedItem.Text;
                dr["tare_sCondicionId"] = Convert.ToInt16(ddlCondicionMant.SelectedValue);
                dr["tare_vCondicion"] = ddlCondicionMant.SelectedItem.Text;               
            }
            else
            {
                dr = dt.NewRow();
                dr["tare_sTarifarioRequisitoId"] = 0;
                dr["tare_sTarifarioId"] = 0;
                dr["tare_sRequisitoId"] = Convert.ToInt16(ddlRequisitoMant.SelectedValue);
                dr["tare_vRequisito"] = ddlRequisitoMant.SelectedItem.Text;
                dr["tare_sTipoActaId"] = Convert.ToInt16(ddlTipoActaMant.SelectedValue);
                dr["tare_vTipoActa"] = ddlTipoActaMant.SelectedItem.Text;
                dr["tare_sCondicionId"] = Convert.ToInt16(ddlCondicionMant.SelectedValue);
                dr["tare_vCondicion"] = ddlCondicionMant.SelectedItem.Text;                
            }

            dt.Rows.Add(dr);

            ddlRequisitoMant.SelectedIndex = -1;
            ddlTipoActaMant.SelectedIndex = -1;
            ddlCondicionMant.SelectedIndex = -1;

            gdvTarifarioRequisito.DataSource = dt;
            gdvTarifarioRequisito.DataBind();
            Session[strVariableDtDet] = dt;
            Session[strVariableIndiceDet] = -1;
        }

        protected void gdvTarifarioRequisito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StrScript = string.Empty;
           
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            Session[strVariableIndiceDet] = intSeleccionadoDet;            

            if (e.CommandName == "Editar")
            {                
                PintarSeleccionadoDet();
            }

            if (e.CommandName == "Eliminar")
            {
                DataTable dtRemesaDetalle = ((DataTable)Session[strVariableDtDet]).Copy();

                dtRemesaDetalle.Rows[intSeleccionadoDet].Delete();
                dtRemesaDetalle.AcceptChanges();
                Session[strVariableDtDet] = dtRemesaDetalle;               

                this.gdvTarifarioRequisito.DataSource = dtRemesaDetalle;
                this.gdvTarifarioRequisito.DataBind();
            }
        }
        #endregion

        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());          

            Session.Add(strVariableAccionDet, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndiceDet, -1);

            DataTable dt = FillEmptyDatagdvHistorico();
            gdvHistorico.DataSource = dt;
            gdvHistorico.DataBind();

            Session.Remove("DtRegTarifarioRequisito");
            Session["DtRegTarifarioRequisito"] = CrearDtRegTarifarioRequisito();
            ((DataTable)Session["DtRegTarifarioRequisito"]).Clear();

            gdvTarifarioRequisito.DataSource = CrearDtRegTarifarioRequisito();
            gdvTarifarioRequisito.DataBind();           

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            ddlSeccionMant.SelectedIndex = -1;
            txtTarifaNumeroMant.Text = "";
            txtTarifaLetraMant.Text = "";
            txtDescripcionMant.Text = "";
            txtNombreCortoMant.Text = "";
            ddlBasePercepcionMant.SelectedIndex = -1;
            ddlTipoCalculoMant.SelectedIndex = -1;
            txtTarifaCostoMant.Text = "0";
            ddlTopeTipoMant.SelectedIndex = -1;
            txtTopeCantidadMant.Text = "";
            txtTopeMinimo.Text = "1";
            chkHabilitaCantidad.Checked = false;
            chkExcepcion.Checked = false;
            txtFecInicio.Text = "";
            txtFecFin.Text = "";
            txtObservaciones.Text = "";

            pnlHistorico.Visible = true;           

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();            
        }      

        private void CargarListadosDesplegables()
        {
            

            //-------------------------------------------------------------
            //Fecha: 05/02/2020
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar las tablas independientemente.
            //-------------------------------------------------------------
            Util.CargarParametroDropDownList(ddlSeccionConsulta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.SECCION), true, "- TODOS -");
            Util.CargarParametroDropDownList(ddlSeccionMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.SECCION), true);

            Util.CargarParametroDropDownList(ddlBasePercepcionMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BASE_PERCEPCION), true);
            Util.CargarParametroDropDownList(ddlRequisitoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.REQUISITO_ACTUACION), true);
            //-------------------------------------------------------------


            Util.CargarParametroDropDownList(ddlTipoCalculoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TARIFARIO_TIPO_CALCULO), true);
            Util.CargarParametroDropDownList(ddlTopeTipoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TARIFARIO_TOPE_UNIDAD), true);

            DataTable DtTarifarioEstados = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.TARIFARIO_ESTADO);
            DataTable DtParametrosFiltrados = new DataTable();
            if (DtTarifarioEstados != null)
            {
                DataView dv = DtTarifarioEstados.DefaultView;
                dv.RowFilter = "id <> 34"; // CARGA LOS ESTADOS DIFERENTE DE INACTIVO
                DtParametrosFiltrados = dv.ToTable();
            }
            Util.CargarParametroDropDownList(ddlEstadoConsulta, DtParametrosFiltrados, true);
            Util.CargarParametroDropDownList(ddl_estado, DtParametrosFiltrados, true);



            Util.CargarParametroDropDownList(ddlTipoActaMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_TIPO_RECONOCIMIENTO), true);
            Util.CargarParametroDropDownList(ddlCondicionMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_REQUISITO_CONDICION), true);
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlSeccionMant.Enabled = bolHabilitar;
            txtTarifaNumeroMant.Enabled = bolHabilitar;
            txtTarifaLetraMant.Enabled = bolHabilitar;
            txtDescripcionMant.Enabled = bolHabilitar;
            txtNombreCortoMant.Enabled = bolHabilitar;
            ddlBasePercepcionMant.Enabled = bolHabilitar;
            ddlTipoCalculoMant.Enabled = bolHabilitar;
            txtTarifaCostoMant.Enabled = bolHabilitar;
            ddlTopeTipoMant.Enabled = bolHabilitar;
            txtTopeCantidadMant.Enabled = bolHabilitar;
            txtTopeMinimo.Enabled = bolHabilitar;
            txtObservaciones.Enabled = bolHabilitar;
            chkHabilitaCantidad.Enabled = bolHabilitar;
            chkExcepcion.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            ddlSeccionMant.SelectedIndex = -1;
            txtTarifaNumeroMant.Text = "";
            txtTarifaLetraMant.Text = "";
            txtDescripcionMant.Text = "";
            txtNombreCortoMant.Text = "";
            ddlBasePercepcionMant.SelectedIndex = -1;
            ddlTipoCalculoMant.SelectedIndex = -1;
            txtTarifaCostoMant.Text = "0";
            ddlTopeTipoMant.SelectedIndex = -1;
            txtTopeCantidadMant.Text = "";
            txtTopeMinimo.Text = "1";
            chkHabilitaCantidad.Checked = false;
            chkExcepcion.Checked = false;
            ddl_estado.SelectedValue = "33"; //ACTIVO
            ddl_estado.Enabled = false;
           
            txtFecInicio.Text = "";
            txtFecFin.Text = "";
            txtObservaciones.Text = "";

            pnlHistorico.Visible = true;

            ddlSeccionConsulta.SelectedValue = "0";
            txtDescripcionConsulta.Text = "";
            gdvTarifario.DataSource = null;
            gdvTarifario.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            Session.Remove("DtRegTarifarioRequisito");
            Session["DtRegTarifarioRequisito"] = CrearDtRegTarifarioRequisito();
            ((DataTable)Session["DtRegTarifarioRequisito"]).Clear();

            gdvTarifarioRequisito.DataSource = null;
            gdvTarifarioRequisito.DataBind();

            gdvTarifarioRequisito.DataSource = CrearDtRegTarifarioRequisito();
            gdvTarifarioRequisito.DataBind();

            DataTable dt = FillEmptyDatagdvHistorico();
            gdvHistorico.DataSource = dt;
            gdvHistorico.DataBind();   

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
                    Session["TarifarioId"] = drSeleccionado["tari_sTarifarioId"].ToString();
                    ddlSeccionMant.SelectedValue = drSeleccionado["tari_sSeccionId"].ToString();
                    txtTarifaNumeroMant.Text = drSeleccionado["tari_sNumero"].ToString();
                    txtTarifaLetraMant.Text = drSeleccionado["tari_vLetra"].ToString();
                    txtNombreCortoMant.Text = drSeleccionado["tari_vDescripcionCorta"].ToString();
                    txtDescripcionMant.Text = drSeleccionado["tari_vDescripcion"].ToString();
                    ddlBasePercepcionMant.SelectedValue = drSeleccionado["tari_sBasePercepcionId"].ToString();

                    ddlTipoCalculoMant.SelectedValue = drSeleccionado["tari_sCalculoTipoId"].ToString();

                    txtObservaciones.Text = drSeleccionado["tari_vCalculoFormula"].ToString();
                    txtTarifaCostoMant.Text = drSeleccionado["tari_FCosto"].ToString();
                    if (drSeleccionado["tari_sTopeUnidadId"].ToString() != string.Empty)
                    {
                        ddlTopeTipoMant.SelectedValue = drSeleccionado["tari_sTopeUnidadId"].ToString();
                    }
                    txtTopeCantidadMant.Text = drSeleccionado["tari_ITopeCantidad"].ToString();
                    txtTopeMinimo.Text = drSeleccionado["tari_ITopeCantidadMinima"].ToString();

                    chkHabilitaCantidad.Checked = Convert.ToBoolean(Convert.ToInt32(drSeleccionado["tari_bHabilitaCantidad"]));

                    chkExcepcion.Checked = Convert.ToBoolean(Convert.ToInt32(drSeleccionado["tari_bFlagExcepcion"]));

                    ddl_estado.SelectedValue = drSeleccionado["tari_sEstadoId"].ToString();
                    //if (ddl_estado.SelectedValue != "35")
                    //{
                    //    ddl_estado.Enabled = false;
                    //}
                    //else
                    //{
                       ddl_estado.Enabled = true;
                    //}

                    if (drSeleccionado["tari_dVigenciaInicio"] != null)
                    {
                        if (drSeleccionado["tari_dVigenciaInicio"].ToString() != string.Empty)
                        {
                            txtFecInicio.Text = Comun.FormatearFecha(drSeleccionado["tari_dVigenciaInicio"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        }
                    }

                    if (drSeleccionado["tari_dVigenciaFin"] != null)
                    {
                        if (drSeleccionado["tari_dVigenciaFin"].ToString() != string.Empty)
                        {
                            txtFecFin.Text = Comun.FormatearFecha(drSeleccionado["tari_dVigenciaFin"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        }
                    }

                    // Requisitos
                    BindGridRequisitos(Convert.ToInt16(drSeleccionado["tari_sTarifarioId"].ToString()));

                    updMantenimiento.Update();
                }
            }          
        }        

        private SGAC.BE.MRE.SI_TARIFARIO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_TARIFARIO objParametro = new SGAC.BE.MRE.SI_TARIFARIO();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objParametro.tari_sTarifarioId = Convert.ToInt16(ObtenerFilaSeleccionada()["tari_sTarifarioId"]);
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objParametro.tari_sSeccionId = Convert.ToInt16(ddlSeccionMant.SelectedValue);
                    if (txtTarifaNumeroMant.Text.Trim() != string.Empty)
                        objParametro.tari_sNumero = Convert.ToInt16(txtTarifaNumeroMant.Text);
                    objParametro.tari_vLetra = txtTarifaLetraMant.Text.ToUpper();
                    objParametro.tari_vDescripcionCorta = txtNombreCortoMant.Text.ToUpper();
                    objParametro.tari_vDescripcion = txtDescripcionMant.Text;
                    objParametro.tari_sBasePercepcionId = Convert.ToInt16(ddlBasePercepcionMant.SelectedValue);
                    objParametro.tari_sCalculoTipoId = Convert.ToInt16(ddlTipoCalculoMant.SelectedValue);
                    objParametro.tari_vCalculoFormula = txtObservaciones.Text;
                    objParametro.tari_FCosto = Convert.ToDouble(txtTarifaCostoMant.Text);
                    objParametro.tari_sTopeUnidadId = Convert.ToInt16(ddlTopeTipoMant.SelectedValue);
                    objParametro.tari_ITopeCantidad = Convert.ToInt32(txtTopeCantidadMant.Text);
                    objParametro.tari_ITopeCantidadMinima = Convert.ToInt32(txtTopeMinimo.Text);
                    objParametro.tari_bHabilitaCantidad = chkHabilitaCantidad.Checked;
                    objParametro.tari_sEstadoId = Convert.ToInt16(ddl_estado.SelectedValue);
                    objParametro.tari_bFlagExcepcion = chkExcepcion.Checked;

                    if (txtFecInicio.Text != string.Empty)
                    {
                        DateTime datFechaInicio = new DateTime();
                        if (!DateTime.TryParse(txtFecInicio.Text, out datFechaInicio))
                        {
                            datFechaInicio = Comun.FormatearFecha(txtFecInicio.Text);
                        }
                        objParametro.tari_dVigenciaInicio = datFechaInicio;
                    }

                    if (txtFecFin.Text != string.Empty)
                    {
                        DateTime datFechaFin = new DateTime();
                        if (!DateTime.TryParse(txtFecFin.Text, out datFechaFin))
                        {
                            datFechaFin = Comun.FormatearFecha(txtFecFin.Text);
                        }
                        objParametro.tari_dVigenciaFin = datFechaFin;
                    }
                }

                objParametro.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objParametro.tari_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.tari_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.tari_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.tari_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                return objParametro;
            }

            return null;
        }

        private SGAC.BE.MRE.SI_TARIFARIO ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                DataTable dt = (DataTable)Session[strVariableDt];
                DataRow drSeleccionado = dt.Rows[intSeleccionado];

                SGAC.BE.MRE.SI_TARIFARIO objParametro = new SGAC.BE.MRE.SI_TARIFARIO();

                objParametro.tari_sSeccionId = Convert.ToInt16(drSeleccionado["tari_sSeccionId"].ToString());
                if (drSeleccionado["tari_sNumero"].ToString() != string.Empty)
                    objParametro.tari_sNumero = Convert.ToInt16(drSeleccionado["tari_sNumero"].ToString());
                objParametro.tari_vLetra = drSeleccionado["tari_vLetra"].ToString();
                objParametro.tari_vDescripcionCorta = drSeleccionado["tari_vDescripcionCorta"].ToString();
                objParametro.tari_vDescripcion = drSeleccionado["tari_vDescripcion"].ToString();
                objParametro.tari_sBasePercepcionId = Convert.ToInt16(drSeleccionado["tari_sBasePercepcionId"].ToString());
                objParametro.tari_sCalculoTipoId = Convert.ToInt16(drSeleccionado["tari_sCalculoTipoId"].ToString());
                objParametro.tari_vCalculoFormula = drSeleccionado["tari_vCalculoFormula"].ToString();
                objParametro.tari_FCosto = Convert.ToDouble(drSeleccionado["tari_FCosto"].ToString());

                if (drSeleccionado["tari_sTopeUnidadId"].ToString() != string.Empty)
                {
                    objParametro.tari_sTopeUnidadId = Convert.ToInt16(drSeleccionado["tari_sTopeUnidadId"].ToString());
                }

                objParametro.tari_ITopeCantidad = Convert.ToInt16(drSeleccionado["tari_ITopeCantidad"].ToString());

                objParametro.tari_sEstadoId = 34; // Se setea en estado "INACTIVO"

                if (drSeleccionado["tari_dVigenciaInicio"] != null)
                {
                    if (drSeleccionado["tari_dVigenciaInicio"].ToString() != string.Empty)
                    {
                        objParametro.tari_dVigenciaInicio = Comun.FormatearFecha(drSeleccionado["tari_dVigenciaInicio"].ToString());
                    }
                }

                if (drSeleccionado["tari_dVigenciaFin"] != null)
                {
                    if (drSeleccionado["tari_dVigenciaFin"].ToString() != string.Empty)
                    {
                        objParametro.tari_dVigenciaFin = Comun.FormatearFecha(drSeleccionado["tari_dVigenciaFin"].ToString());
                    }
                }

                objParametro.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objParametro.tari_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.tari_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.tari_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.tari_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                return objParametro;
            }

            return null;
        }

        private List<SGAC.BE.MRE.SI_TARIFARIOREQUISITO> ObtenerListadoEntidadDetalle()
        {
            DataTable dtTarifarioRequisito = (DataTable)Session[strVariableDtDet];
            if (dtTarifarioRequisito == null)
            {
                return null;
            }

            bool bolExisteDetalle = false;
            List<SGAC.BE.MRE.SI_TARIFARIOREQUISITO> lstEntidadDetalle = new List<SGAC.BE.MRE.SI_TARIFARIOREQUISITO>();
            foreach (DataRow dr in dtTarifarioRequisito.Rows)
            {
                bolExisteDetalle = false;

                if (dr["tare_sTarifarioRequisitoId"].ToString() != string.Empty)
                {
                    if (Convert.ToInt32(dr["tare_sTarifarioRequisitoId"]) != 0)
                    {
                        bolExisteDetalle = true;
                    }
                }

                SGAC.BE.MRE.SI_TARIFARIOREQUISITO objEntidadDetalle = new SGAC.BE.MRE.SI_TARIFARIOREQUISITO();
                objEntidadDetalle.tare_sTarifarioRequisitoId = Convert.ToInt16(dr["tare_sTarifarioRequisitoId"]);
                objEntidadDetalle.tare_sTarifarioId = Convert.ToInt16(dr["tare_sTarifarioId"]);
                objEntidadDetalle.tare_sRequisitoId = Convert.ToInt16(dr["tare_sRequisitoId"]);
                objEntidadDetalle.tare_sTipoActaId = Convert.ToInt16(dr["tare_sTipoActaId"]);
                objEntidadDetalle.tare_sCondicionId = Convert.ToInt16(dr["tare_sCondicionId"]);
                objEntidadDetalle.tare_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidadDetalle.tare_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidadDetalle.tare_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidadDetalle.tare_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidadDetalle.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);                    

                lstEntidadDetalle.Add(objEntidadDetalle);                
            }

            return lstEntidadDetalle;
        }

        private DataRow ObtenerFilaSeleccionadaDet()
        {
            if (Session != null)
            {
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

                int intSeleccionado = (int)Session[strVariableIndiceDet];

                if (enmAccion == Enumerador.enmAccion.INSERTAR)
                {
                    return ((DataTable)Session["DtRegTarifarioRequisito"]).Rows[intSeleccionado];
                }
                else 
                {
                    return ((DataTable)Session[strVariableDtDet]).Rows[intSeleccionado];
                }
            }

            return null;
        }

        private void PintarSeleccionadoDet()
        {
            if (Session != null)
            {
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

                DataRow drSeleccionado = ObtenerFilaSeleccionadaDet();
                if (drSeleccionado != null)
                {
                    if (enmAccion == Enumerador.enmAccion.INSERTAR)
                    {
                        ddlRequisitoMant.SelectedValue = drSeleccionado["tare_sRequisitoId"].ToString();
                        ddlTipoActaMant.SelectedValue = drSeleccionado["tare_sTipoActaId"].ToString();
                        ddlCondicionMant.SelectedValue = drSeleccionado["tare_sCondicionId"].ToString();
                    }
                    else
                    {
                        Session["TarifarioRequisitoId"] = drSeleccionado["tare_sTarifarioRequisitoId"].ToString();
                        ddlRequisitoMant.SelectedValue = drSeleccionado["tare_sRequisitoId"].ToString();
                        ddlTipoActaMant.SelectedValue = drSeleccionado["tare_sTipoActaId"].ToString();
                        ddlCondicionMant.SelectedValue = drSeleccionado["tare_sCondicionId"].ToString();
                    }

                    updMantenimiento.Update();
                }
            }
        }

        private void BindGrid(string StrSeccion, string StrDescripcion, string StrEstado)
        {
            DataTable DtTarifario = new DataTable();

            Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            TarifarioConsultasBL BL = new TarifarioConsultasBL();

            DtTarifario = BL.Consultar(Convert.ToInt16(StrSeccion),
                                       StrDescripcion,
                                       Convert.ToInt16(StrEstado),
                                       ctrlPaginador.PaginaActual.ToString(),
                                       intPaginaCantidad,
                                       ref IntTotalCount,
                                       ref IntTotalPages);

            Session[strVariableDt] = DtTarifario;

            if (DtTarifario.Rows.Count > 0)
            {
                gdvTarifario.SelectedIndex = -1;
                gdvTarifario.DataSource = DtTarifario;
                gdvTarifario.DataBind();

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
                gdvTarifario.DataSource = null;
                gdvTarifario.DataBind();
            }

            updGrillaConsulta.Update();
        }

        private void BindGridRequisitos(int IntTarifarioId)
        {
            DataTable DtTarifarioRequisito = new DataTable();
            TarifarioRequisitoConsultasBL BL = new TarifarioRequisitoConsultasBL();

            DtTarifarioRequisito = BL.Obtener(Convert.ToInt16(IntTarifarioId));

            Session[strVariableDtDet] = DtTarifarioRequisito;

            if (DtTarifarioRequisito.Rows.Count > 0)
            {
                gdvTarifarioRequisito.SelectedIndex = -1;
                gdvTarifarioRequisito.DataSource = DtTarifarioRequisito;
                gdvTarifarioRequisito.DataBind();                         
            }
            else
            {                
                gdvTarifarioRequisito.DataSource = null;
                gdvTarifarioRequisito.DataBind();

                Session.Remove("DtRegTarifarioRequisito");
                Session["DtRegTarifarioRequisito"] = CrearDtRegTarifarioRequisito();
                ((DataTable)Session["DtRegTarifarioRequisito"]).Clear();

                gdvTarifarioRequisito.DataSource = CrearDtRegTarifarioRequisito();
                gdvTarifarioRequisito.DataBind();
            }

            updMantenimiento.Update();
        }

        private void GrabarHandler()
        {
            string strScript = string.Empty;

            TarifarioMantenimientoBL BL = new TarifarioMantenimientoBL();
            bool Error = true;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];            

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    BL.Insert(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);                   

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    BL.Update(ObtenerEntidadMantenimiento(), ObtenerEntidadConsulta(), ObtenerListadoEntidadDetalle(), ref Error);                    

                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    BL.Delete(ObtenerEntidadMantenimiento(), ref Error);

                    break;
            }
           
            if (!Error)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                string StrSeccion = ddlSeccionMant.SelectedValue;

                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                Session["Grabo"] = "SI";

                Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                Session[strVariableDt] = new DataTable();

                if (enmAccion == Enumerador.enmAccion.INSERTAR || enmAccion == Enumerador.enmAccion.MODIFICAR)
                {
                    BindGrid("0", "", "33");
                    ddlEstadoConsulta.SelectedValue = "33";
                }
                else if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    BindGrid("0", "", "35");
                    ddlEstadoConsulta.SelectedValue = "35";
                }

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else if (Error)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error de Sistema");
            }

            Comun.EjecutarScript(Page, strScript);
        }

        private DataTable CrearDtRegTarifarioRequisito()
        {
            DataTable DtRegTarifarioRequisito = new DataTable();
            DtRegTarifarioRequisito.Columns.Clear();

            DataColumn dcTarifarioRequisitoId = DtRegTarifarioRequisito.Columns.Add("tare_sTarifarioRequisitoId", typeof(int));
            dcTarifarioRequisitoId.AllowDBNull = true;
            dcTarifarioRequisitoId.Unique = false;

            DataColumn dcTarifarioId = DtRegTarifarioRequisito.Columns.Add("tare_sTarifarioId", typeof(int));
            dcTarifarioId.AllowDBNull = true;
            dcTarifarioId.Unique = false;

            DataColumn dcRequisitoId = DtRegTarifarioRequisito.Columns.Add("tare_sRequisitoId", typeof(int));
            dcRequisitoId.AllowDBNull = true;
            dcRequisitoId.Unique = false;

            DataColumn dcRequisito = DtRegTarifarioRequisito.Columns.Add("tare_vRequisito", typeof(string));
            dcRequisito.AllowDBNull = true;
            dcRequisito.Unique = false;

            DataColumn dcTipoActaId = DtRegTarifarioRequisito.Columns.Add("tare_sTipoActaId", typeof(int));
            dcTipoActaId.AllowDBNull = true;
            dcTipoActaId.Unique = false;

            DataColumn dcTipoActa = DtRegTarifarioRequisito.Columns.Add("tare_vTipoActa", typeof(string));
            dcTipoActa.AllowDBNull = true;
            dcTipoActa.Unique = false;

            DataColumn dcCondicionId = DtRegTarifarioRequisito.Columns.Add("tare_sCondicionId", typeof(int));
            dcCondicionId.AllowDBNull = true;
            dcCondicionId.Unique = false;

            DataColumn dcCondicion = DtRegTarifarioRequisito.Columns.Add("tare_vCondicion", typeof(string));
            dcCondicion.AllowDBNull = true;
            dcCondicion.Unique = false;

            return DtRegTarifarioRequisito;
        }

        public DataTable FillEmptyDatagdvHistorico()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Fec. Vigencia Inicio", typeof(string));
            dt.Columns.Add("Fec. Vigencia Fin", typeof(string));
            dt.Columns.Add("Base Percepción", typeof(string));
            dt.Columns.Add("Tipo Cálculo", typeof(string));
            dt.Columns.Add("Costo", typeof(string));
            dt.Columns.Add("Tope Tipo", typeof(string));
            dt.Columns.Add("Tope Cantidad", typeof(string));
            return dt;
        }

        #endregion        
    }
}