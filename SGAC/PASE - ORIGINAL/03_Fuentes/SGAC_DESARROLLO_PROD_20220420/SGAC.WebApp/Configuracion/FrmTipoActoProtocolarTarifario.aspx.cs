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
    public partial class FrmTipoActoProtocolarTarifario : System.Web.UI.Page
    {
        #region CAMPOS
        private string strNombreEntidad = "TipoActoProtocolar_Tarifario";
        private string strVariableAccion = "TipoActoProtocolarTarifario_Accion";
        private string strVariableDt = "TipoActoProtocolarTarifario_Tabla";
        private string strVariableIndice = "TipoActoProtocolarTarifario_Indice";
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
                GridView[] arrGridView = { gdvTarifario };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }

        protected void gdvTipoActoProtocolar_RowCommand(object sender, GridViewCommandEventArgs e)
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
                ddlregTipoActoProtocolar.Enabled = false;


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
            ddlConsultaTipoActoprotocolar.SelectedIndex = 0;
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
            
            if (ddlregTipoActoProtocolar.SelectedIndex == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar un Tipo de Acto Protocolar.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            //-----------------------------------------------------------
            ActualizarSesionTarifario();
            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();

            //-----------------------------------------------------------

            if (dtTarifasSel.Rows.Count == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar por lo menos una Tarifa Consular.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
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

        protected void chkSoloTarifasSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarTarifasSeleccionadas();
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
            DataTable dtTipoActoProtocolar = new DataTable();
            DataTable dtTarifario = new DataTable();

            //-------------------------------------------------------
            

            DataTable dtOrdenadoTipActNotarial = new DataTable();

            dtOrdenadoTipActNotarial = ObtenerTipoActoNotarial();

            Util.CargarDropDownList(ddlConsultaTipoActoprotocolar, dtOrdenadoTipActNotarial, "descripcion", "Id", true, "- TODOS -");
            Util.CargarDropDownList(ddlregTipoActoProtocolar, dtOrdenadoTipActNotarial, "descripcion", "Id", true);


            object[] arrParametros = { 2, "", 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 500, 0, 0 };


            dtTarifario = comun_Part2.ObtenerTarifarioConsulta(Session, ref arrParametros);

            Util.CargarDropDownList(ddlConsultaTarifa, dtTarifario, "tari_vdescripcioncorta", "tari_starifarioId", true, "- TODOS -");

            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);

            Session["listaTarifas"] = dtListaTarifas;
            gdvTarifario.DataSource = dtListaTarifas;
            gdvTarifario.DataBind();
        }
    
        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_TipoActoProtocolarId.Value = "";

            ddlregTipoActoProtocolar.SelectedIndex = 0;


            gdvTipoActoProtocolar.DataSource = null;
            gdvTipoActoProtocolar.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            chkSoloTarifasSeleccionadas.Checked = false;
            chkSoloTarifasSeleccionadas.Text = "Mostrar solo las seleccionadas";


            //------------------------------------------------
            DataTable dtTarifario = new DataTable();
            //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];
            dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);


            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);

            Session["listaTarifas"] = dtListaTarifas;

            gdvTarifario.DataSource = dtListaTarifas;
            gdvTarifario.DataBind();
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
                    chkSoloTarifasSeleccionadas.Checked = false;

                    //---------------------------------------------------------------
                    HF_TipoActoProtocolarId.Value = drSeleccionado["acta_sTipoActoProtocolarId"].ToString();

                    short intTipoActoProtocolarId = Convert.ToInt16(HF_TipoActoProtocolarId.Value);

                    ddlregTipoActoProtocolar.SelectedValue = HF_TipoActoProtocolarId.Value;

                    //---------------------------------------------------------------

                    DataTable dtTarifario = new DataTable();
                    dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);

                    DataTable dtListaTarifas = new DataTable();

                    dtListaTarifas = CrearListaTarifas(dtTarifario);
                    //---------------------------------------------------------------
                    DataTable dtTipoActoProtocolarTarifario = new DataTable();

                    TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;
                    short intTipoActoProtocolarTarifarioId = 0;

                    dtTipoActoProtocolarTarifario = objTipoActoProtocolarConsultaBL.Consultar_TipoActoProtocolarTarifario(intTipoActoProtocolarTarifarioId,intTipoActoProtocolarId, 0, 20000, "1", "S", ref IntTotalCount, ref IntTotalPages);

                    DataTable dtTarifarioSel = dtTipoActoProtocolarTarifario.DefaultView.ToTable(true, "acta_sTarifarioId");

                    if (dtTipoActoProtocolarTarifario.Rows.Count > 0)
                    {

                        Int16 intTarifaId = 0;
                        //-----------------------------------------------------------------
                        for (int i = 0; i < dtTarifarioSel.Rows.Count; i++)
                        {
                            intTarifaId = Convert.ToInt16(dtTarifarioSel.Rows[i]["acta_sTarifarioId"].ToString());

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
                    }
                    //-----------------------------------------------------

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

                    //-----------------------------------------------------

                  
                    updMantenimiento.Update();
                }
            }
        }


        private SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objParametro = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    //Editar o Anular
                    objParametro.acta_iTipoActoProtocolarTarifarioId = 0;
                    objParametro.acta_sTipoActoProtocolarId = Convert.ToInt16(HF_TipoActoProtocolarId.Value);
                    objParametro.acta_sTarifarioId = 0;
                    objParametro.OficinaConsultar = 0;
                }
                objParametro.acta_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.acta_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.acta_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.acta_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                return objParametro;
            }

            return null;
        }
        private void BindGrid()
        {
            DataTable dtTipoActoProtocolarTarifario = new DataTable();
            TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            short intTipoActoProtocolarId = 0;
            short intTarifaId = 0;
            //-----------------------------------------------------

            if (ddlConsultaTipoActoprotocolar.SelectedIndex > 0)
            {
                intTipoActoProtocolarId = Convert.ToInt16(ddlConsultaTipoActoprotocolar.SelectedValue);
            }
            if (ddlConsultaTarifa.SelectedIndex > 0)
            {
                intTarifaId = Convert.ToInt16(ddlConsultaTarifa.SelectedValue);
            }

            short intTipoActoProtocolarTarifarioId = 0;
            dtTipoActoProtocolarTarifario = objTipoActoProtocolarConsultaBL.Consultar_TipoActoProtocolarTarifario(intTipoActoProtocolarTarifarioId,intTipoActoProtocolarId, intTarifaId, intPaginaCantidad, ctrlPaginador.PaginaActual.ToString(), "S", ref IntTotalCount, ref IntTotalPages);

            Session[strVariableDt] = dtTipoActoProtocolarTarifario;

            if (dtTipoActoProtocolarTarifario.Rows.Count > 0)
            {
                gdvTipoActoProtocolar.SelectedIndex = -1;
                gdvTipoActoProtocolar.DataSource = dtTipoActoProtocolarTarifario;
                gdvTipoActoProtocolar.DataBind();

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

                gdvTipoActoProtocolar.DataSource = null;
                gdvTipoActoProtocolar.DataBind();
            }
            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarBE = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

            string strScript = string.Empty;

            TipoActoProtocolarTarifarioMantenimientoBL objTipoActoProtocolarTarifarioBL = new TipoActoProtocolarTarifarioMantenimientoBL();
            TipoActoProtocolarTarifarioConsultasBL objTipoActoProtocolarTarifarioConsultaBL = new TipoActoProtocolarTarifarioConsultasBL();

            List<SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> listaActoProtocolarTarifario = new List<BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO>();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:

                    ActualizarSesionTarifario();
                    listaActoProtocolarTarifario = ObtenerListaTipoActoNotarialTarifario();

                    objTipoActoProtocolarBE = objTipoActoProtocolarTarifarioBL.InsertarTipoActoProtocolarTarifario(listaActoProtocolarTarifario);

                    break;
                case Enumerador.enmAccion.MODIFICAR:

                    ActualizarSesionTarifario();
                    listaActoProtocolarTarifario = ObtenerListaTipoActoNotarialTarifario();
                   
                    //------------------------------------------------------------------------------------
                    //Fecha: 06/06/2019
                    //Autor: Miguel Márquez
                    //Objetivo: Verificar si los registros de la Tabla: SI_NORMA_TARIFARIO deben anularse.
                    //------------------------------------------------------------------------------------
                    BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolar = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();
                    BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO BE;

                    objTipoActoProtocolar = ObtenerEntidadMantenimiento();

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;
                    short intTipoActoProtocolarId = Convert.ToInt16(objTipoActoProtocolar.acta_sTipoActoProtocolarId);
                    Int16 intTarifaId = 0;
                    short intTipoActoProtocolarTarifarioId = 0;
                    bool bExiste = false;
                    bool bExisteError = false;

                    DataTable dtTipoActoProtocolarTarifario = new DataTable();
                    dtTipoActoProtocolarTarifario = objTipoActoProtocolarTarifarioConsultaBL.Consultar_TipoActoProtocolarTarifario(intTipoActoProtocolarTarifarioId,intTipoActoProtocolarId, intTarifaId, 10000, "1", "N", ref IntTotalCount, ref IntTotalPages);
                    List<SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> listaActoProtocolarTarifarioAnular = new List<BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO>();
                    
                    for (int x = 0; x < dtTipoActoProtocolarTarifario.Rows.Count; x++)
                    {
                        intTipoActoProtocolarTarifarioId = Convert.ToInt16(dtTipoActoProtocolarTarifario.Rows[x]["acta_iTipoActoProtocolarTarifarioId"].ToString());
                        intTarifaId = Convert.ToInt16(dtTipoActoProtocolarTarifario.Rows[x]["acta_sTarifarioId"].ToString());

                        bExiste = false;

                        for (int i = 0; i < listaActoProtocolarTarifario.Count; i++)
                        {
                            if (intTarifaId == listaActoProtocolarTarifario[i].acta_sTarifarioId)
                            {
                                bExiste = true;
                                break;
                            }
                        }
                        if (bExiste == false)
                        { 
                                BE = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();
                                BE.acta_iTipoActoProtocolarTarifarioId = intTipoActoProtocolarTarifarioId;
                                BE.acta_sTipoActoProtocolarId = intTipoActoProtocolarId;
                                BE.acta_sTarifarioId = intTarifaId;
                                BE.acta_cEstado = "A";
                                BE.acta_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                BE.acta_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                BE.acta_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                BE.acta_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                listaActoProtocolarTarifarioAnular.Add(BE);
                        }
                    }
                   
                    if (listaActoProtocolarTarifarioAnular.Count > 0)
                    {
                        for (int i = 0; i < listaActoProtocolarTarifarioAnular.Count; i++)
                        {
                            bool bExistenRegistro = false;
                            intTarifaId = listaActoProtocolarTarifarioAnular[i].acta_sTarifarioId;
                            bExistenRegistro = objTipoActoProtocolarTarifarioConsultaBL.ConsultarExisteTipoActoProtocolarTarifario(intTipoActoProtocolarId, intTarifaId);
                            if (bExistenRegistro == false)
                            {
                                objTipoActoProtocolar.acta_iTipoActoProtocolarTarifarioId = listaActoProtocolarTarifarioAnular[i].acta_iTipoActoProtocolarTarifarioId;
                                objTipoActoProtocolarBE = objTipoActoProtocolarTarifarioBL.AnularTipoActoProtocolarTarifario(objTipoActoProtocolar);
                            }
                            else
                            {
                                bExisteError = true;
                                objTipoActoProtocolarBE.Error = true;
                                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Existen registros vinculados al tipo de Acto Protocolar y la tarifa consular.");
                                break;
                            }
                        }
                    }
                    //------------------------------------------------------------------------

                    if (bExisteError == false)
                    {
                        //-------------------------------------------------------------------------------------------
                        //Fecha: 06/06/2019
                        //Autor: Miguel Márquez
                        //Objetivo: Verificar si los registros de la lista: listaNormaTarifario deben adicionarse
                        //-------------------------------------------------------------------------------------------
                       
                        List<BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> listaTipoActosNotarialesTarifarioNuevos = new List<BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO>();
                        //------------------------------------------------------
                        for (int x = 0; x < listaActoProtocolarTarifario.Count; x++)
                        {
                            intTarifaId = listaActoProtocolarTarifario[x].acta_sTarifarioId;

                            bool bExistenRegistro = false;
                            //---------------------------------------------------
                            for (int i = 0; i < dtTipoActoProtocolarTarifario.Rows.Count; i++)
                            {
                                if (Convert.ToInt16(dtTipoActoProtocolarTarifario.Rows[i]["acta_sTarifarioId"].ToString()) == intTarifaId)
                                {
                                    bExistenRegistro = true;
                                    break;
                                }
                            }
                            //---------------------------------------------------
                            if (bExistenRegistro == false)
                            {
                                BE = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();
                                BE.acta_sTipoActoProtocolarId = intTipoActoProtocolarId;
                                BE.acta_sTarifarioId = intTarifaId;
                                BE.acta_cEstado = "A";
                                BE.acta_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                BE.acta_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                BE.acta_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                BE.acta_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                listaTipoActosNotarialesTarifarioNuevos.Add(BE);
                            }
                        }
                        //------------------------------------------------------
                        if (listaTipoActosNotarialesTarifarioNuevos.Count > 0)
                        {
                            objTipoActoProtocolarBE = objTipoActoProtocolarTarifarioBL.InsertarTipoActoProtocolarTarifario(listaTipoActosNotarialesTarifarioNuevos);
                        }
                    }
                    //************************************
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    BE = new BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();
                    objTipoActoProtocolar = ObtenerEntidadMantenimiento();

                    intTipoActoProtocolarId = Convert.ToInt16(objTipoActoProtocolar.acta_sTipoActoProtocolarId);
                    bool bExistenRegistrox = false;
                    bExistenRegistrox = objTipoActoProtocolarTarifarioConsultaBL.ConsultarExisteTipoActoProtocolarTarifario(intTipoActoProtocolarId, 0);
                    if (bExistenRegistrox == false)
                    {                        
                        objTipoActoProtocolarBE = objTipoActoProtocolarTarifarioBL.AnularTipoActoProtocolarTarifarioTodos(objTipoActoProtocolar);
                    }
                    else
                    {
                        objTipoActoProtocolarBE.Error = true;
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Existen registros vinculados al tipo de Acto Protocolar.");
                    }

                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }
            if (objTipoActoProtocolarBE.Error == false)
            {
              
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
              

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
                if (strScript == string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
                }
            }
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, strScript);
        }

        private DataTable CrearTablaTarifario()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("tari_sTarifarioId", typeof(Int16));
            dt.Columns.Add("tarifa", typeof(String));
            dt.Columns.Add("tari_vdescripcioncorta", typeof(String));
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
            string strLetra = "";

            for (int i = 0; i < dtTarifario.Rows.Count; i++)
            {
                dr = dtListaTarifas.NewRow();

                dr["tari_sTarifarioId"] = dtTarifario.Rows[i]["tari_sTarifarioId"].ToString();

                strDescripcion = dtTarifario.Rows[i]["tari_vdescripcioncorta"].ToString();
                strLetra = dtTarifario.Rows[i]["tari_vLetra"].ToString().Trim();

                if (strLetra.Length == 0)
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim();
                }
                else
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim() + "-" + strLetra;
                }


                dr["tarifa"] = strTarifa;
                dr["tari_vdescripcioncorta"] = strDescripcion;
                dr["seleccion"] = "0";

                dtListaTarifas.Rows.Add(dr);
            }
            return dtListaTarifas;
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
        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlregTipoActoProtocolar.Enabled = bolHabilitar;
        }

        private void FiltrarTarifasSeleccionadas(bool bEsNuevo = false)
        {
            bool bTarifasSeleccionadas = chkSoloTarifasSeleccionadas.Checked;
            string strTarifasSeleccionadas = "";

            if (bTarifasSeleccionadas)
            {
                strTarifasSeleccionadas = "seleccion = 1";
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

            dv.RowFilter = strTarifasSeleccionadas;
            DataTable dtListaTarifasFiltradas = dv.ToTable();
            gdvTarifario.DataSource = dtListaTarifasFiltradas;
            gdvTarifario.DataBind();

        }

        private DataTable ObtenerTipoActoNotarial()
        {
            DataTable dtTipoActoNotarial = new DataTable();

            dtTipoActoNotarial = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR);

            //string FormatosProtocolar = ConfigurationManager.AppSettings["FormatoProtocolar"].ToString();

            //DataTable new_Data = null;

            //new_Data = (from dtsi in dtTipoActoNotarial.AsEnumerable()
            //            where FormatosProtocolar.Contains(dtsi["id"].ToString())
            //            select dtsi).CopyToDataTable();
            //new_Data.DefaultView.Sort = "torden asc";

            //return new_Data;
            return dtTipoActoNotarial;
        }

        private List<SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> ObtenerListaTipoActoNotarialTarifario()
        {
            SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO BE;
            List<SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> listaTipoActoTarifario = new List<BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO>();

            //----------------------------------------------
            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();

            short intTarifaId = 0;
            short intTipoActoProtocolar = 0;

            if (ddlregTipoActoProtocolar.SelectedIndex > 0)
            {
                intTipoActoProtocolar = Convert.ToInt16(ddlregTipoActoProtocolar.SelectedValue);
            }

            for (int x = 0; x < dtTarifasSel.Rows.Count; x++)
            {
                intTarifaId = Convert.ToInt16(dtTarifasSel.Rows[x]["tari_sTarifarioId"].ToString());

                BE = new SGAC.BE.MRE.SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

                BE.acta_iTipoActoProtocolarTarifarioId = 0;
                BE.acta_sTipoActoProtocolarId = intTipoActoProtocolar;
                BE.acta_sTarifarioId = intTarifaId;
                BE.acta_cEstado = "A";
                BE.acta_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                BE.acta_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                BE.acta_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                BE.acta_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                listaTipoActoTarifario.Add(BE);

            }

            return listaTipoActoTarifario;
        }
        //-----------------------
    }
}