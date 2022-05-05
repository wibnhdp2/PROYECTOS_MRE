using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Configuracion.Maestro.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmPlantillaTraduccion : System.Web.UI.Page
    {
        #region CAMPOS
        private string strNombreEntidad = "TRADUCCION";
        private string strVariableAccion = "Traduccion_Accion";
        private string strVariableDt = "Traduccion_Tabla";
        private string strVariableIndice = "Traduccion_Indice";
        //-------------------------------------------------------
      
        #endregion

        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.Visible = false;
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;               
            ctrlPaginador.PaginaActual = 1;
            //--------------------------------------
           
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarMantenimiento.btnCancelar.Text = "    Limpiar";
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
                NuevaEtiqueta();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnCancelar };
                GridView[] arrGridView = { gdvTraduccion};
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();
            BindGridTraduccion();
        }
       
        private void BindGridTraduccion()
        {
            DataTable dtTraduccion = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            string strEstado = "";

            if (chkActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }
            PlantillaTraduccionConsultasBL BL = new PlantillaTraduccionConsultasBL();

            Int16 intPlanillaId = Convert.ToInt16(ddlConsultaPlantilla.SelectedValue);
            Int16 intIdiomaId = Convert.ToInt16(ddlConsultaIdioma.SelectedValue);

            dtTraduccion = BL.Consultar(0, intPlanillaId, intIdiomaId, 0, strEstado,
                ctrlPaginador.PaginaActual.ToString(), intPaginaCantidad, "S", ref IntTotalCount, ref IntTotalPages);

            Session[strVariableDt] = dtTraduccion;

            if (dtTraduccion.Rows.Count > 0)
            {
                gdvTraduccion.SelectedIndex = -1;
                gdvTraduccion.DataSource = dtTraduccion;
                gdvTraduccion.DataBind();

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

                gdvTraduccion.DataSource = null;
                gdvTraduccion.DataBind();
            }
            updConsulta.Update();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {            
            ddlConsultaPlantilla.SelectedIndex = 0;
            ddlConsultaIdioma.SelectedIndex = 0;
            chkActivo.Checked = true;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            string strScript = string.Empty;

            PlantillaTraduccionConsultasBL BL = new PlantillaTraduccionConsultasBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                DataTable dtTraduccion = new DataTable();
                int IntTotalCount = 0;
                int IntTotalPages = 0;

                Int16 intPlantillaId = Convert.ToInt16(ddlregPlantilla.SelectedValue);
                Int64 intEtiquetaId = Convert.ToInt64(ddlregEtiqueta.SelectedValue);
                Int16 intIdiomaId = Convert.ToInt16(ddlregIdioma.SelectedValue);

                dtTraduccion = BL.Consultar(0, intPlantillaId, intIdiomaId, intEtiquetaId, "A", "1", 1000, "N", ref IntTotalCount, ref IntTotalPages);

                if (IntTotalCount > 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Traducción", "Ya existe la Etiqueta con el idioma traducido.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }

            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            updMantenimiento.Update();

            Session["Grabo"] = "NO";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "var yes = confirm('¿Desea realizar la operación?'); if (yes) __doPostBack('GrabarHandler', 'yes');", true);
        }

        private void GrabarHandler()
        {
            Int64 intResultado = 0;
            string strScript = string.Empty;

            PlantillaTraduccionMantenimientoBL BL = new PlantillaTraduccionMantenimientoBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    intResultado = BL.Insertar(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    intResultado = BL.Actualizar(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    intResultado = BL.Anular(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }
            if (intResultado == (int)Enumerador.enmResultadoOperacion.OK)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }
                Session["Grabo"] = "SI";

                NuevaEtiqueta();

                Session[strVariableDt] = new DataTable();
                BindGridTraduccion();
                
                
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                updConsulta.Update();
                updMantenimiento.Update();
            }
            else if (intResultado == (int)Enumerador.enmResultadoOperacion.ERROR)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
            }            
            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
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
        protected void ddlregPlantilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int16 intPlantilla = Convert.ToInt16(ddlregPlantilla.SelectedValue);

            EtiquetaConsultaBL BL = new EtiquetaConsultaBL();
            DataTable dtEtiqueta = new DataTable();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            if (intPlantilla > 0)
            {
                dtEtiqueta = BL.Consultar(0, intPlantilla, "", "A", 1000, 1, "N", ref IntTotalPages, ref IntTotalCount);                
            }
            if (dtEtiqueta.Rows.Count > 0)
            {
                Util.CargarDropDownList(ddlregEtiqueta, dtEtiqueta, "vValor", "IRgistroId", true);
            }
            else
            {
                ddlregEtiqueta.Items.Clear();
                this.ddlregEtiqueta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            }
            ddlregEtiqueta.Focus();
            updMantenimiento.Update();
        }

        protected void gdvTraduccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strScript = string.Empty;
                int intSeleccionado = Convert.ToInt32(e.CommandArgument);

                Session[strVariableIndice] = intSeleccionado;

                if (e.CommandName == "Consultar")
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = true;

                    HabilitarMantenimiento(false);
                    PintarSeleccionado();

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

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();
            BindGridTraduccion();
        }
            
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlregPlantilla.Enabled = bolHabilitar;
            ddlregEtiqueta.Enabled = bolHabilitar;
            ddlregIdioma.Enabled = bolHabilitar;
            txtregTraduccion.Enabled = bolHabilitar;
            chkActivoMant.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            HF_PlantillaTraduccionId.Value = "0";
            ddlregPlantilla.SelectedIndex = 0;
            ddlregEtiqueta.SelectedIndex = 0;
            ddlregIdioma.SelectedIndex = 0;
            txtregTraduccion.Text = "";
            chkActivoMant.Checked = true;

            gdvTraduccion.DataSource = null;
            gdvTraduccion.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            updMantenimiento.Update();
            updConsulta.Update();
        }

        void NuevaEtiqueta()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_PlantillaTraduccionId.Value = "0";
            txtregTraduccion.Text = "";
            chkActivoMant.Checked = true;

            HabilitarMantenimiento();
            updMantenimiento.Update();
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
     
        private void CargarListadosDesplegables()
        {
            DataTable dtPlantilla = new DataTable();
            dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");
            Util.CargarDropDownList(ddlConsultaPlantilla, dtPlantilla, "descripcion", "id", true, " - TODOS - ");
            Util.CargarDropDownList(ddlregPlantilla, dtPlantilla, "descripcion", "id", true, " - SELECCIONAR - ");
            //-----------------------------------------
            DataTable dtIdioma = new DataTable();

            dtIdioma = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TRADUCCION_IDIOMA);
            dtIdioma.DefaultView.Sort = "descripcion";

            Util.CargarParametroDropDownList(this.ddlConsultaIdioma, dtIdioma, true, " - SELECCIONAR - ");
            Util.CargarParametroDropDownList(this.ddlregIdioma, dtIdioma, true);
            ddlregEtiqueta.Items.Clear();
            this.ddlregEtiqueta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
        }

        private void PintarSeleccionado()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                if (drSeleccionado != null)
                {
                    HF_PlantillaTraduccionId.Value = drSeleccionado["pltr_iPlantillaTraduccionId"].ToString();

                    if (drSeleccionado["etiq_sPlantillaId"].ToString() != "0")
                    {
                        ddlregPlantilla.SelectedValue = drSeleccionado["etiq_sPlantillaId"].ToString();
                        Int16 intPlantilla = Convert.ToInt16(ddlregPlantilla.SelectedValue);
                        EtiquetaConsultaBL BL = new EtiquetaConsultaBL();
                        DataTable dtEtiqueta = new DataTable();
                        int IntTotalCount = 0;
                        int IntTotalPages = 0;
                        dtEtiqueta = BL.Consultar(0, intPlantilla, "", "A", 1000, 1, "N", ref IntTotalPages, ref IntTotalCount);
                        if (dtEtiqueta.Rows.Count > 0)
                        {                            
                            Util.CargarDropDownList(ddlregEtiqueta, dtEtiqueta, "vValor", "IRgistroId", true);
                        }
                        else
                        {
                            ddlregEtiqueta.Items.Clear();
                            this.ddlregEtiqueta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                        }
                        if (drSeleccionado["pltr_iEtiquetaId"].ToString() != "0")
                        {
                            ddlregEtiqueta.SelectedValue = drSeleccionado["pltr_iEtiquetaId"].ToString();
                        }
                        else
                        {
                            ddlregEtiqueta.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddlregPlantilla.SelectedIndex = 0;
                        ddlregEtiqueta.Items.Clear();
                        this.ddlregEtiqueta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                        ddlregEtiqueta.SelectedIndex = 0;
                    }
                    
                    if (drSeleccionado["pltr_sIdiomaId"].ToString() != "0")
                    {
                        ddlregIdioma.SelectedValue = drSeleccionado["pltr_sIdiomaId"].ToString();
                    }
                    else
                    {
                        ddlregIdioma.SelectedIndex = 0;
                    }
                    txtregTraduccion.Text = drSeleccionado["pltr_vTraduccion"].ToString().Trim();
                    if (drSeleccionado["pltr_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkActivoMant.Checked = true;
                    }
                    else
                    {
                        chkActivoMant.Checked = false;
                    }
                    updMantenimiento.Update();
                }
            }
        }    

        private RE_PLANTILLA_TRADUCCION ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                RE_PLANTILLA_TRADUCCION objBE = new RE_PLANTILLA_TRADUCCION();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objBE.pltr_iPlantillaTraduccionId = Convert.ToInt16(ObtenerFilaSeleccionada()["pltr_iPlantillaTraduccionId"]);
                }
                else
                {
                    objBE.pltr_iPlantillaTraduccionId = Convert.ToInt16(HF_PlantillaTraduccionId.Value);
                }
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objBE.pltr_iEtiquetaId = Convert.ToInt64(ddlregEtiqueta.SelectedValue);
                    objBE.pltr_sIdiomaId = Convert.ToInt16(ddlregIdioma.SelectedValue);
                    objBE.pltr_vTraduccion = txtregTraduccion.Text.Trim();
                }
                objBE.pltr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objBE.pltr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objBE.pltr_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objBE.pltr_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                if (chkActivoMant.Checked)
                {
                    objBE.pltr_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    objBE.pltr_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }
                objBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                return objBE;
            }
            return null;
        }

    }
}