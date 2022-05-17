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
    public partial class FrmNorma : System.Web.UI.Page
    {
       #region CAMPOS
            private string strNombreEntidad = "NORMA";
            private string strVariableAccion = "Norma_Accion";
            private string strVariableDt = "Norma_Tabla";
            private string strVariableIndice = "Norma_Indice";
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

            dtpConsultaFechaFin.AllowFutureDate = true;
            dtpregFechaFin.AllowFutureDate = true;

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
                GridView[] arrGridView = { gdvNormas };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void gdvNormas_RowCommand(object sender, GridViewCommandEventArgs e)
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
            ddlTipoNorma.SelectedIndex = 0;
            txtConsultaNorma.Text = "";
            dtpConsultaFechaInicio.Text = "";
            dtpConsultaFechaFin.Text = "";
            ddlConsultaEstadoNorma.SelectedIndex = 0;
            ddlConsultaGrupo.SelectedIndex = 0;

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

            NormaTarifarioDL objNormaBL = new NormaTarifarioDL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                DataTable dtNorma = new DataTable();

                int IntTotalCount = 0;
                int IntTotalPages = 0;

                string strDescripcionCortaNorma = txtregDescripcionCorta.Text.Trim();
                short intEstadoNormaId = Convert.ToInt16(ddlregEstadoNorma.SelectedValue);
                short intGrupoNormaId = Convert.ToInt16(ddlregGrupoNorma.SelectedValue);

                dtNorma = objNormaBL.ConsultarNorma(0, 1, strDescripcionCortaNorma, "", "", intEstadoNormaId,intGrupoNormaId, 1, 1, "N", ref IntTotalCount, ref IntTotalPages);

                if (IntTotalCount > 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Ya existe la Norma.", false, 190, 250);
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
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        protected void ddlregTipoNorma_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarObjetoNorma();
        }

        #region Métodos

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
            DataTable dtTipoNorma = new DataTable();
            DataTable dtObjetoNorma = new DataTable();
            DataTable dtEstadoNorma = new DataTable();
            DataTable dtGrupoNorma = new DataTable();

            dtTipoNorma = comun_Part1.ObtenerParametrosPorGrupo(Session, "NORMA-DOCUMENTOS");
            dtObjetoNorma = comun_Part1.ObtenerParametrosPorGrupo(Session, "NORMA-OBJETO");
            dtGrupoNorma = comun_Part1.ObtenerParametrosPorGrupo(Session, "NORMA-GRUPO");

            dtEstadoNorma = comun_Part1.ObtenerParametrosPorGrupoMRE("NORMA-ESTADO");

            Util.CargarParametroDropDownList(ddlTipoNorma, dtTipoNorma, true, "- TODOS -");
            Util.CargarParametroDropDownList(ddlregTipoNorma, dtTipoNorma);

            Util.CargarParametroDropDownList(ddlConsultaEstadoNorma, dtEstadoNorma, true, "- TODOS -");
            Util.CargarParametroDropDownList(ddlConsultaGrupo, dtGrupoNorma, true, "- TODOS -");

            Util.CargarParametroDropDownList(ddlregEstadoNorma, dtEstadoNorma, true);
            Util.CargarParametroDropDownList(ddlregGrupoNorma, dtGrupoNorma, true);

            CargarObjetoNorma();
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlregTipoNorma.Enabled = bolHabilitar;
            ddlregObjetoNorma.Enabled = bolHabilitar;
            txtregArticulo.Enabled = bolHabilitar;
            txtregInciso.Enabled = bolHabilitar;
            txtregNombreArticulo.Enabled = bolHabilitar;
            txtregDescripcionCorta.Enabled = bolHabilitar;
            txtregDescripcion.Enabled = bolHabilitar;
            dtpregFechaInicio.Enabled = bolHabilitar;
            dtpregFechaFin.Enabled = bolHabilitar;
            ddlregEstadoNorma.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_sNormaId.Value = "0";
            ddlregTipoNorma.SelectedIndex = 0;
            ddlregObjetoNorma.SelectedIndex = 0;
            ddlregGrupoNorma.SelectedIndex = 0;
            txtregArticulo.Text = "";
            txtregInciso.Text = "";
            txtregNombreArticulo.Text = "";
            txtregDescripcionCorta.Text = "";
            txtregDescripcion.Text = "";

            string strFechaTexto = Comun.ObtenerFechaActualTexto(Session);
            dtpregFechaInicio.Text = strFechaTexto;
            dtpregFechaFin.Text = strFechaTexto;
            //dtpregFechaFin.Text = Comun.ObtenerFechaActualTexto(Session);
            
            ddlregEstadoNorma.SelectedIndex = 0;
            //----------------------------------

            DataTable dtEstadoNorma = new DataTable();
//            dtEstadoNorma = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], "NORMA-ESTADO");
            dtEstadoNorma = comun_Part1.ObtenerParametrosPorGrupoMRE("NORMA-ESTADO");

            for (int i = 0; i < dtEstadoNorma.Rows.Count; i++)
            {
                if (dtEstadoNorma.Rows[i]["descripcion"].ToString().Equals("VIGENTE"))
                {
                    ddlregEstadoNorma.SelectedValue = dtEstadoNorma.Rows[i]["id"].ToString();
                    break;
                }
            }
            //----------------------------------

            gdvNormas.DataSource = null;
            gdvNormas.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

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
                    HF_sNormaId.Value = drSeleccionado["norm_sNormaId"].ToString();
                                       
                    ddlregTipoNorma.SelectedValue = drSeleccionado["norm_sTipoNormaId"].ToString();

                    CargarObjetoNorma();

                    ddlregObjetoNorma.SelectedValue = drSeleccionado["norm_sObjetoNormaId"].ToString();
                    txtregArticulo.Text = drSeleccionado["norm_vNumeroArticulo"].ToString();
                    txtregInciso.Text = drSeleccionado["norm_vInciso"].ToString();
                    txtregNombreArticulo.Text = drSeleccionado["norm_vNombreArticulo"].ToString();
                    txtregDescripcionCorta.Text = drSeleccionado["norm_vDescripcionCorta"].ToString();
                    txtregDescripcion.Text = drSeleccionado["norm_vDescripcion"].ToString();

                    if (drSeleccionado["norm_dVigenciaInicio"].ToString().Length > 0)
                    {
                        dtpregFechaInicio.Text = Comun.FormatearFecha(drSeleccionado["norm_dVigenciaInicio"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    }
                    else
                    {
                        dtpregFechaInicio.Text = "";
                    }
                    if (drSeleccionado["norm_dVigenciaFin"].ToString().Length > 0)
                    {
                        dtpregFechaFin.Text = Comun.FormatearFecha(drSeleccionado["norm_dVigenciaFin"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    }
                    else
                    {
                        dtpregFechaFin.Text = "";
                    }
                    
                    ddlregEstadoNorma.SelectedValue = drSeleccionado["norm_sEstadoId"].ToString();

                    if (drSeleccionado["norm_sGrupoNormaId"] != null)
                    {
                        ddlregGrupoNorma.SelectedValue = drSeleccionado["norm_sGrupoNormaId"].ToString();
                    }
                    else
                    {
                        ddlregGrupoNorma.SelectedIndex = 0;
                    }
                    updMantenimiento.Update();
                }
            }
        }

        private SGAC.BE.MRE.SI_NORMA ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_NORMA objParametro = new BE.MRE.SI_NORMA();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objParametro.norm_sNormaId = Convert.ToInt16(ObtenerFilaSeleccionada()["norm_sNormaId"]);
                }
                else
                {
                    objParametro.norm_sNormaId = Convert.ToInt16(HF_sNormaId.Value);
                }
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objParametro.norm_sTipoNormaId = Convert.ToInt16(ddlregTipoNorma.SelectedValue);
                    objParametro.norm_sObjetoNormaId = Convert.ToInt16(ddlregObjetoNorma.SelectedValue);
                    objParametro.norm_vNumeroArticulo = txtregArticulo.Text.ToUpper();
                    objParametro.norm_vInciso = txtregInciso.Text.ToUpper();
                    objParametro.norm_vNombreArticulo = txtregNombreArticulo.Text.ToUpper();
                    objParametro.norm_vDescripcionCorta = txtregDescripcionCorta.Text.ToUpper();

                    string strDescripcion = hdn_descripcion.Value.ToUpper();
                    objParametro.norm_vDescripcion = strDescripcion;
                    //objParametro.norm_vDescripcion = txtregDescripcion.Text.ToUpper();

                    DateTime datFechaInicio = Comun.FormatearFecha(dtpregFechaInicio.Text);
                    objParametro.norm_dVigenciaInicio = datFechaInicio;

                    DateTime datFechaFin = Comun.FormatearFecha(dtpregFechaFin.Text);
                    objParametro.norm_dVigenciaFin = datFechaFin;

                    objParametro.norm_sEstadoId = Convert.ToInt16(ddlregEstadoNorma.SelectedValue);

                    objParametro.norm_sGrupoNormaId = Convert.ToInt16(ddlregGrupoNorma.SelectedValue);  
                }
                objParametro.norm_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.norm_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.norm_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.norm_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                return objParametro;
            }
            return null;
        }

        private void BindGrid()
        {
            DataTable dtNorma = new DataTable();
            NormaTarifarioDL objNormaBL = new NormaTarifarioDL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;                        

            short intTipoNormaId = 0;
            short intEstadoNormaId = 0;
            short intGrupoNormaId = 0;

            //-----------------------------------------------------
            if (ddlTipoNorma.SelectedIndex > 0)
            {
                intTipoNormaId = Convert.ToInt16(ddlTipoNorma.SelectedValue);
            }

            string strNorma = txtConsultaNorma.Text.ToUpper();
            string strYYYYMMDD_Ini = "";


            if (dtpConsultaFechaInicio.Text.Trim() != "")
            {
                if (Comun.EsFecha(dtpConsultaFechaInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strYYYYMMDD_Ini = Comun.syyyymmdd(dtpConsultaFechaInicio.Value().ToShortDateString());
            }

            string strYYYYMMDD_Fin = "";

            if (dtpConsultaFechaFin.Text.Trim() != "")
            {
                if (Comun.EsFecha(dtpConsultaFechaFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strYYYYMMDD_Fin = Comun.syyyymmdd(dtpConsultaFechaFin.Value().ToShortDateString());
            }
            if (ddlConsultaEstadoNorma.SelectedIndex > 0)
            {
                intEstadoNormaId = Convert.ToInt16(ddlConsultaEstadoNorma.SelectedValue);
            }
            if (ddlConsultaGrupo.SelectedIndex > 0)
            {
                intGrupoNormaId = Convert.ToInt16(ddlConsultaGrupo.SelectedValue);
            }

            dtNorma = objNormaBL.ConsultarNorma(intTipoNormaId, 0, strNorma, strYYYYMMDD_Ini, strYYYYMMDD_Fin, intEstadoNormaId, intGrupoNormaId,
                        intPaginaCantidad, ctrlPaginador.PaginaActual, "S", ref IntTotalCount, ref IntTotalPages);

            Session[strVariableDt] = dtNorma;

            if (dtNorma.Rows.Count > 0)
            {
                gdvNormas.SelectedIndex = -1;
                gdvNormas.DataSource = dtNorma;
                gdvNormas.DataBind();

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

                gdvNormas.DataSource = null;
                gdvNormas.DataBind();
            }
            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            BE.MRE.SI_NORMA objNormaBE = new BE.MRE.SI_NORMA();

            string strScript = string.Empty;

            NormaTarifarioDL objNormaBL = new NormaTarifarioDL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            

            if (enmAccion == Enumerador.enmAccion.INSERTAR || enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                if (dtpregFechaInicio.Text.Trim() != "")
                {
                    if (Comun.EsFecha(dtpregFechaInicio.Text.Trim()) == false)
                    {
                        Session["Grabo"] = string.Empty;
                        Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_VALIDACION_FECHA_INICIAL);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }                    
                }
                if (dtpregFechaFin.Text.Trim() != "")
                {
                    if (Comun.EsFecha(dtpregFechaFin.Text.Trim()) == false)
                    {
                        Session["Grabo"] = string.Empty;
                        Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_VALIDACION_FECHA_FINAL);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }                    
                }
                if (dtpregFechaInicio.Value() > dtpregFechaFin.Value())
                {
                    Session["Grabo"] = string.Empty;
                    Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "La Fecha final no debe ser menor a la fecha de inicio.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }


            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    objNormaBE = objNormaBL.InsertarNorma(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    objNormaBE = objNormaBL.ActualizarNorma(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    objNormaBE = objNormaBL.AnularNorma(ObtenerEntidadMantenimiento());
                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }

            if (objNormaBE.Error == false)
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

        private void CargarObjetoNorma()
        {
            Int16 intTipoNorma = Convert.ToInt16(ddlregTipoNorma.SelectedValue);

            DataTable dtObjetoNorma = new DataTable();

            dtObjetoNorma = comun_Part1.ObtenerParametrosPorGrupo(Session, "NORMA-OBJETO");

            
            DataView dv = dtObjetoNorma.DefaultView;
            dv.RowFilter = "para_vReferencia = '" + intTipoNorma.ToString() + "'";
            DataTable dtObjetoTipoNorma = dv.ToTable();


            Util.CargarParametroDropDownList(ddlregObjetoNorma, dtObjetoTipoNorma, true);

        }

        #endregion

       

    }
}