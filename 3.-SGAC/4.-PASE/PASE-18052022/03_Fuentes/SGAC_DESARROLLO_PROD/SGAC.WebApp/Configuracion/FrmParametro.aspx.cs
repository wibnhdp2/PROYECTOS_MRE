using System;
using System.Configuration;
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
    public partial class FrmParametro : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "PARAMETRO";
        private string strVariableAccion = "Parametro_Accion";
        private string strVariableDt = "Parametro_Tabla";
        private string strVariableIndice = "Parametro_Indice";
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

            /* Setea el rango ingreso permitido de fecha de Inicio del Parametro del control calendario */
            this.calExtFechaInicio.StartDate = new DateTime(2015, 1, 1);

            /* Setea el rango ingreso permitido de fecha de Fin del Parametro del control calendario */
            this.calExtFechaFin.StartDate = new DateTime(2015, 1, 1);

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
                txtFindGrupo.Attributes.Add("style", "visibility:hidden");
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvParametro };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue == "0")
            {
                ctrlToolBarConsulta_btnCancelarHandler();
            }
        }
        
        protected void gdvParametro_RowCommand(object sender, GridViewCommandEventArgs e)
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

                hidOper.Value = Convert.ToString(Enumerador.enmAccion.MODIFICAR);

                HabilitarMantenimiento(false);
                PintarSeleccionado();

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                hidOper.Value = Convert.ToString(Enumerador.enmAccion.MODIFICAR);
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
            string strEstado = string.Empty;

            if (chkEstado.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();   
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();   
            }

            Session[strVariableDt] = new DataTable();
            BindGrid(ddlGrupo.SelectedItem.Text.Trim(), strEstado);            
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            string strEstado = string.Empty;
                 
            if (ddlGrupo.SelectedValue == "0")
            {
                ctrlValidation.MostrarValidacion(Constantes.CONST_MENSAJE_NO_SELECCION_FILTROS, true, Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {
                if (chkEstado.Checked)
                {
                    strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();   
                }
                else
                {
                    strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();   
                }

                Session[strVariableDt] = new DataTable();
                ctrlPaginador.InicializarPaginador();
                BindGrid(ddlGrupo.SelectedItem.Text.Trim(), strEstado);                
            }

            updArbol.Update();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            hidOper.Value = Convert.ToString(Enumerador.enmAccion.INSERTAR);
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            hidOper.Value = Convert.ToString(Enumerador.enmAccion.MODIFICAR);
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
            updMantenimiento.Update();
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            HabilitarMantenimiento(false);
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
            hidOper.Value = Convert.ToString(Enumerador.enmAccion.MODIFICAR);
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, "Elimina"));
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {                     
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (txtFechaInicio.Text != string.Empty && txtFechaFin.Text == string.Empty)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No ha establecido correctamente el rango de vigencia del parámetro");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (txtFechaInicio.Text == string.Empty && txtFechaFin.Text != string.Empty)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No ha establecido correctamente el rango de vigencia del parámetro");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (txtFechaInicio.Text != string.Empty && txtFechaFin.Text != string.Empty)
            {
                if (!DateTime.TryParse(txtFechaInicio.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(txtFechaInicio.Text);
                }

                if (!DateTime.TryParse(txtFechaFin.Text, out datFechaFin))
                {
                    datFechaFin = Comun.FormatearFecha(txtFechaFin.Text);
                }

                if (datFechaInicio > datFechaFin)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_VALIDACION_DOS_FECHAS);
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
            Comun.EjecutarScript(Page, Util.ActivarTab(0) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            hidOper.Value = Convert.ToString(Enumerador.enmAccion.INSERTAR);
            
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            txtOrdenMant.Text = "";

            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";          

            chkEstadoMant.Checked = true;
            chkExcepcion.Checked = false;

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));

            updMantenimiento.Update();
        }     

        private void CargarListadosDesplegables()
        {
            DataTable dtGrupo = new DataTable();

            dtGrupo = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.GRUPO);

            DataView dv = dtGrupo.DefaultView;
            dv.Sort = "descripcion";

            DataTable dtGrupoOrdenado = new DataTable();

            dtGrupoOrdenado = dv.ToTable();


            Util.CargarParametroDropDownList(ddlGrupo, dtGrupoOrdenado, true);
            Util.CargarParametroDropDownList(ddlGrupoMant, dtGrupoOrdenado, true);
        }        

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlGrupoMant.Enabled = bolHabilitar;
            txtDescripcionMant.Enabled = bolHabilitar;
            txtValorMant.Enabled = bolHabilitar;
            txtReferenciaMant.Enabled = bolHabilitar;
            txtOrdenMant.Enabled = bolHabilitar;
            imgCal1.Enabled = bolHabilitar;
            imgCal2.Enabled = bolHabilitar;
            chkVisibleMant.Enabled = bolHabilitar;
            chkEstadoMant.Enabled = bolHabilitar;
            chkExcepcion.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;            

            ddlGrupoMant.Items.Clear();
            DataTable dtGrupo = new DataTable();
            dtGrupo = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.GRUPO);
            DataView dv = dtGrupo.DefaultView;
            dv.Sort = "descripcion";
            DataTable dtGrupoOrdenado = new DataTable();

            dtGrupoOrdenado = dv.ToTable();

            Util.CargarParametroDropDownList(ddlGrupoMant, dtGrupoOrdenado, true);

            txtDescripcionMant.Text = "";
            txtValorMant.Text = "";
            txtReferenciaMant.Text = "";
            txtOrdenMant.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";

            chkVisibleMant.Checked = true;
            chkEstado.Checked = true;
            chkExcepcion.Checked = false;


            ddlGrupo.SelectedValue = "0";           
            gdvParametro.DataSource = null;
            gdvParametro.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            updMantenimiento.Update();
            updArbol.Update();
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
                    ddlGrupoMant.SelectedItem.Text = drSeleccionado["para_vGrupo"].ToString();
                    txtDescripcionMant.Text = drSeleccionado["para_vDescripcion"].ToString();
                    txtValorMant.Text = drSeleccionado["para_vValor"].ToString();
                    txtReferenciaMant.Text = drSeleccionado["para_vReferencia"].ToString();
                    txtOrdenMant.Text = drSeleccionado["para_tOrden"].ToString();

                    if (drSeleccionado["para_dVigenciaInicio"] != null)
                    {
                        if (drSeleccionado["para_dVigenciaInicio"].ToString() != string.Empty)
                        {
                            txtFechaInicio.Text = Comun.FormatearFecha(drSeleccionado["para_dVigenciaInicio"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        }
                    }

                    if (drSeleccionado["para_dVigenciaFin"] != null)
                    {
                        if (drSeleccionado["para_dVigenciaFin"].ToString() != string.Empty)
                        {
                            txtFechaFin.Text = Comun.FormatearFecha(drSeleccionado["para_dVigenciaFin"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        }
                    }

                    if (drSeleccionado["para_bFlagExcepcion"].ToString() != string.Empty)
                    {
                        chkExcepcion.Checked = Convert.ToBoolean(drSeleccionado["para_bFlagExcepcion"]);
                    }


                    if (drSeleccionado["para_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkEstadoMant.Checked = true;
                    }
                    else
                    {
                        chkEstadoMant.Checked = false;
                    }

                    if (drSeleccionado["para_bVisible"].ToString() != string.Empty)
                    {
                        chkVisibleMant.Checked = Convert.ToBoolean(drSeleccionado["para_bVisible"]);
                    }

                    updMantenimiento.Update();
                }
            }
        }

        private SGAC.BE.MRE.SI_PARAMETRO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_PARAMETRO objParametro = new BE.MRE.SI_PARAMETRO();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objParametro.para_sParametroId = Convert.ToInt16(ObtenerFilaSeleccionada()["para_sParametroId"]);
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objParametro.para_vGrupo = ddlGrupoMant.SelectedItem.Text;
                    objParametro.para_vDescripcion = txtDescripcionMant.Text;
                    objParametro.para_vValor = txtValorMant.Text.ToUpper();

                    if (txtReferenciaMant.Text != string.Empty)
                    {
                        objParametro.para_vReferencia = txtReferenciaMant.Text;
                    }

                    if (txtOrdenMant.Text != string.Empty)
                    {
                        objParametro.para_tOrden = Convert.ToByte(txtOrdenMant.Text);
                    }                   

                    if (txtFechaInicio.Text != string.Empty)
                    {
                        DateTime datFechaInicio = new DateTime();
                        if (!DateTime.TryParse(txtFechaInicio.Text, out datFechaInicio))
                        {
                            datFechaInicio = Comun.FormatearFecha(txtFechaInicio.Text);
                        }
                        objParametro.para_dVigenciaInicio = datFechaInicio;
                    }

                    if (txtFechaFin.Text != string.Empty)
                    {
                        DateTime datFechaFin = new DateTime();
                        if (!DateTime.TryParse(txtFechaFin.Text, out datFechaFin))
                        {
                            datFechaFin = Comun.FormatearFecha(txtFechaFin.Text);
                        }
                        objParametro.para_dVigenciaFin = datFechaFin;
                    }

                    if (chkExcepcion.Checked)
                    {
                        objParametro.para_bFlagExcepcion = true;
                    }
                    else
                    {
                        objParametro.para_bFlagExcepcion = false;
                    }

                    if (chkVisibleMant.Checked)
                        objParametro.para_bVisible = true;
                    else
                        objParametro.para_bVisible = false;

                    if (chkEstadoMant.Checked)
                    {
                        objParametro.para_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    }
                    else
                    {
                        objParametro.para_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                    }

                    if (objParametro.para_cEstado == "A")
                    {
                        objParametro.para_bPrecarga = true;
                    }
                    else
                    {
                        objParametro.para_bPrecarga = false;
                    }
                }

                objParametro.para_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.para_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.para_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.para_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();                

                return objParametro;
            }

            return null;
        }

        private void BindGrid(string StrGrupo, string StrEstado)
        {
            DataTable DtParametros = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            ParametroConsultasBL BL = new ParametroConsultasBL();

            DtParametros = BL.Consultar(StrGrupo,
                                        StrEstado,
                                        ctrlPaginador.PaginaActual.ToString(),
                                        intPaginaCantidad,
                                        ref IntTotalCount,
                                        ref IntTotalPages);

            Session[strVariableDt] = DtParametros;
             
            if (DtParametros.Rows.Count > 0)
            {
                gdvParametro.SelectedIndex = -1;
                gdvParametro.DataSource = DtParametros;
                gdvParametro.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidation.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(IntTotalCount), true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidation.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                ctrlPaginador.Visible = false;
                ctrlPaginador.PaginaActual = 1;
                ctrlPaginador.InicializarPaginador();              
                
                gdvParametro.DataSource = null;
                gdvParametro.DataBind();                
            }

            updGrillaConsulta.Update();            
        }

        private void GrabarHandler()
        {
            int intResultado = 0;
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];            

            string strGrupoConsulta = ddlGrupoMant.SelectedItem.Text.Trim();
            string idGrupoConsulta = ddlGrupoMant.SelectedValue.ToString().Trim();
            ParametroMantenimientoBL BL = new ParametroMantenimientoBL();
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:                    
                    intResultado = BL.Insertar(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));                    
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    intResultado = BL.Actualizar(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));                    
                    break;
                case Enumerador.enmAccion.ELIMINAR:                    
                    intResultado = BL.Eliminar(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));                    
                    break;
            }

            //DataTable DtParametrosConsulta = new DataTable();

            //int IntTotalCount = 0;
            //int IntTotalPages = 0;

            //ParametroConsultasBL BLc = new ParametroConsultasBL();
            
            //DtParametrosConsulta =BLc.ConsultarParametro(1,
            //                                            "1",
            //                                            10000,
            //                                            ref IntTotalCount,
            //                                            ref IntTotalPages);

            //Session[Constantes.CONST_SESION_DT_PARAMETRO] = DtParametrosConsulta;

            if (intResultado == (int)Enumerador.enmResultadoOperacion.OK)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }


                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                Session["Grabo"] = "SI";

                Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                Session[strVariableDt] = new DataTable();

                if (enmAccion == Enumerador.enmAccion.INSERTAR || enmAccion == Enumerador.enmAccion.MODIFICAR)
                {
                    chkEstado.Checked = true;
                    BindGrid(strGrupoConsulta, "A");
                }
                else if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    chkEstado.Checked = false;
                    BindGrid(strGrupoConsulta, "E");
                }

                ddlGrupo.SelectedValue = idGrupoConsulta;

                updArbol.Update();
                updMantenimiento.Update();
            }
            else if (intResultado == (int)Enumerador.enmResultadoOperacion.ERROR)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error de sistema. Contactese con soporte técnico.");
            }

            Comun.EjecutarScript(Page, strScript);
        }

        #endregion        
    }
}