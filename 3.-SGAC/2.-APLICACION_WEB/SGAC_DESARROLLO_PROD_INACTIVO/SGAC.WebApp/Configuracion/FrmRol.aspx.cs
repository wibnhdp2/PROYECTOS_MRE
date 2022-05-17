using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Controlador;
using SGAC.Accesorios;
using System.Data;
using SGAC.WebApp.Accesorios.SharedControls;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmRol : MyBasePage
    {
        #region CAMPOS 
        private string strNombreEntidad = "ROL CONFIGURACION";
        private string strVariableAccion = "RolConfig_Accion";
        private string strVariableDt = "RolConfig_Tabla";
        private string strVariableIndice = "RolConfig_Indice";
        private string strVariableDetalleDt = "RolOpcion_Tabla";
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

            ctrlToolBarConsulta.btnBuscarHandler += new ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.btnNuevoHandler += new ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar()";

            

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                    GrabarHandler();
                else
                    CargarGrilla();
            }

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvRolConfiguracion, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            if (!Page.IsPostBack)
            {
                Session["Grabo"] = "NO";
                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvRolConfiguracion };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        protected void ddlFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkGridBuscar.Checked)
            {
                chkGridBuscar.Checked = true;
            }
            else
            {
                chkGridBuscar.Checked = false;
            }

            if (chkGridImprimir.Checked)
            {
                chkGridImprimir.Checked = true;
            }
            else
            {
                chkGridImprimir.Checked = false;
            }

            if (chkGridCancelarC.Checked)
            {
                chkGridCancelarC.Checked = true;
            }
            else
            {
                chkGridCancelarC.Checked = false;
            }

            if (chkGridCerrar.Checked)
            {
                chkGridCerrar.Checked = true;
            }
            else
            {
                chkGridCerrar.Checked = false;
            }

            if (chkGridRegistrar.Checked)
            {
                chkGridRegistrar.Checked = true;
            }
            else
            {
                chkGridRegistrar.Checked = false;
            }

            if (chkGridModificar.Checked)
            {
                chkGridModificar.Checked = true;
            }
            else
            {
                chkGridModificar.Checked = false;
            }

            if (chkGridEliminar.Checked)
            {
                chkGridEliminar.Checked = true;
            }
            else
            {
                chkGridEliminar.Checked = false;
            }

            if (chkGridConfigurar.Checked)
            {
                chkGridConfigurar.Checked = true;
            }
            else
            {
                chkGridConfigurar.Checked = false;
            }

            if (chkGridGrabar.Checked)
            {
                chkGridGrabar.Checked = true;
            }
            else
            {
                chkGridGrabar.Checked = false;
            }

            if (chkGridCancelarM.Checked)
            {
                chkGridCancelarM.Checked = true;
            }
            else
            {
                chkGridCancelarM.Checked = false;
            }
        }

        protected void gdvRolConfiguracion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;

            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                HabilitarMantenimiento(false);
                HabilitarMantenimientoDetalle(false);
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                HabilitarMantenimiento();
                HabilitarMantenimientoDetalle();
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            }

            PintarSeleccionado();
            CargarGrillaDetalle();

            Comun.EjecutarScript(Page, strScript);
        }

        protected void gdvRolOpcion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int intSeleccionado = Convert.ToInt32(e.CommandArgument);
                DataTable dtRolOpcion = ((DataTable)Session["dtRolOpcionAuxiliar"]).Copy();

                if (dtRolOpcion.Rows[intSeleccionado]["roop_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                {
                    dtRolOpcion.Rows[intSeleccionado]["roop_cEstado"] = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                    gdvRolOpcion.Rows[intSeleccionado].Cells[1].Text = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }
                else
                {
                    dtRolOpcion.Rows[intSeleccionado]["roop_cEstado"] = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    gdvRolOpcion.Rows[intSeleccionado].Cells[1].Text = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }

                dtRolOpcion.AcceptChanges();
                Session["dtRolOpcionAuxiliar"] = dtRolOpcion;

                PintarGrillaDetalleFilas();                
            } 
        }

        protected void btnMostrarAgregarOpcion_Click(object sender, EventArgs e)
        {
            if (tRolOpcion.Visible)
            {
                tRolOpcion.Visible = false;
                LimpiarRegistroFormulario();
            }
            else
            {
                tRolOpcion.Visible = true;

                chkGridBuscar.Checked = true;
                chkGridImprimir.Checked = true;
                chkGridCancelarC.Checked = true;
                chkGridCerrar.Checked = true;
                chkGridRegistrar.Checked = true;
                chkGridModificar.Checked = true;
                chkGridEliminar.Checked = true;
                chkGridConfigurar.Checked = true;
                chkGridGrabar.Checked = true;
                chkGridCancelarM.Checked = true;
            }
        }       

        protected void btnAgregarFormulario_Click(object sender, EventArgs e)
        {
            #region Inicializar Tabla Detalle

            // Se verificar si hay opciones registradas, si no hay se carga las cabeceras del DataSource
            bool bolInicializarTabla = true;
            object obj = Session["dtRolOpcionAuxiliar"];

            if (obj != null)
            {
                if (((DataTable)obj).Rows.Count > 0)
                {
                    bolInicializarTabla = false;
                }
            }

            if (bolInicializarTabla)
            {
                DataTable dtRolOpcion = CrearTablaRolOpcion();
                Session[strVariableDetalleDt] = dtRolOpcion;
                Session["dtRolOpcionAuxiliar"] = dtRolOpcion;
            }
            #endregion

            #region Validar que no se registre una plantilla de rol vacia     

            if (chkGridBuscar.Checked == false && 
                chkGridImprimir.Checked == false && 
                chkGridCancelarC.Checked == false && 
                chkGridCerrar.Checked == false && 
                chkGridRegistrar.Checked == false && 
                chkGridModificar.Checked == false && 
                chkGridEliminar.Checked == false &&
                chkGridConfigurar.Checked == false && 
                chkGridGrabar.Checked == false && 
                chkGridCancelarM.Checked == false)
            {
                ctrlValidacionDetalle.MostrarValidacion("No ha marcado ninguna opción.", true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }            
            #endregion

            #region Agregar Detalle Fila

            // Verificar si el formulario ya se encuentra cargado.
            if (!ExisteFormularioRolOpcion())
            {
                ctrlValidacionDetalle.MostrarValidacion("", false, Enumerador.enmTipoMensaje.ERROR);

                DataTable dtRolOpcion = ((DataTable)Session["dtRolOpcionAuxiliar"]).Copy();

                // Actualizar Tabla por si hay cambios en la selección de opciones 
                int i = 0;

                foreach (GridViewRow gvr in gdvRolOpcion.Rows)
                {
                    dtRolOpcion.Rows[i]["op_buscar"] = ((CheckBox)gvr.FindControl("chkGridBuscar")).Checked;
                    dtRolOpcion.Rows[i]["op_imprimir"] = ((CheckBox)gvr.FindControl("chkGridImprimir")).Checked;
                    dtRolOpcion.Rows[i]["op_cancelar_c"] = ((CheckBox)gvr.FindControl("chkGridCancelarC")).Checked;
                    dtRolOpcion.Rows[i]["op_cerrar"] = ((CheckBox)gvr.FindControl("chkGridCerrar")).Checked;
                    dtRolOpcion.Rows[i]["op_registrar"] = ((CheckBox)gvr.FindControl("chkGridRegistrar")).Checked;
                    dtRolOpcion.Rows[i]["op_modificar"] = ((CheckBox)gvr.FindControl("chkGridModificar")).Checked;
                    dtRolOpcion.Rows[i]["op_eliminar"] = ((CheckBox)gvr.FindControl("chkGridEliminar")).Checked;
                    dtRolOpcion.Rows[i]["op_configurar"] = ((CheckBox)gvr.FindControl("chkGridConfigurar")).Checked;
                    dtRolOpcion.Rows[i]["op_grabar"] = ((CheckBox)gvr.FindControl("chkGridGrabar")).Checked;
                    dtRolOpcion.Rows[i]["op_cancelar_m"] = ((CheckBox)gvr.FindControl("chkGridCancelarM")).Checked;
                    i++;
                }

                // Cargar Nueva Fila                
                DataRow drNuevo = dtRolOpcion.NewRow();
                drNuevo["roop_sRolOpcionId"] = 0;
                drNuevo["form_sAplicacionId"] = Comun.ToNullInt32(ddlAplicacionMant.SelectedValue);
                drNuevo["form_sFormularioId"] = Convert.ToInt32(ddlFormulario.SelectedValue);
                drNuevo["form_vFormulario"] = ddlFormulario.SelectedItem.Text;
                drNuevo["op_buscar"] = Convert.ToInt32(chkGridBuscar.Checked);
                drNuevo["op_cancelar_c"] = Convert.ToInt32(chkGridCancelarC.Checked);
                drNuevo["op_cerrar"] = Convert.ToInt32(chkGridCerrar.Checked);
                drNuevo["op_imprimir"] = Convert.ToInt32(chkGridImprimir.Checked);
                drNuevo["op_registrar"] = Convert.ToInt32(chkGridRegistrar.Checked);
                drNuevo["op_modificar"] = Convert.ToInt32(chkGridModificar.Checked);
                drNuevo["op_eliminar"] = Convert.ToInt32(chkGridEliminar.Checked);
                drNuevo["op_configurar"] = Convert.ToInt32(chkGridConfigurar.Checked);
                drNuevo["op_grabar"] = Convert.ToInt32(chkGridGrabar.Checked);
                drNuevo["op_cancelar_m"] = Convert.ToInt32(chkGridCancelarM.Checked);
                drNuevo["roop_cEstado"] = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                dtRolOpcion.Rows.Add(drNuevo);

                gdvRolOpcion.DataSource = dtRolOpcion;
                gdvRolOpcion.DataBind();

                Session["dtRolOpcionAuxiliar"] = dtRolOpcion;

                PintarGrillaDetalleOpciones();
                PintarGrillaDetalleFilas();

                if (chkGridBuscar.Checked)
                {
                    chkGridBuscar.Checked = true;
                }
                else
                {
                    chkGridBuscar.Checked = false;
                }

                if (chkGridImprimir.Checked)
                {
                    chkGridImprimir.Checked = true;
                }
                else
                {
                    chkGridImprimir.Checked = false;
                }

                if (chkGridCancelarC.Checked)
                {
                    chkGridCancelarC.Checked = true;
                }
                else
                {
                    chkGridCancelarC.Checked = false;
                }

                if (chkGridCerrar.Checked)
                {
                    chkGridCerrar.Checked = true;
                }
                else
                {
                    chkGridCerrar.Checked = false;
                }

                if (chkGridRegistrar.Checked)
                {
                    chkGridRegistrar.Checked = true;
                }
                else
                {
                    chkGridRegistrar.Checked = false;
                }

                if (chkGridModificar.Checked)
                {
                    chkGridModificar.Checked = true;
                }
                else
                {
                    chkGridModificar.Checked = false;
                }

                if (chkGridEliminar.Checked)
                {
                    chkGridEliminar.Checked = true;
                }
                else
                {
                    chkGridEliminar.Checked = false;
                }

                if (chkGridConfigurar.Checked)
                {
                    chkGridConfigurar.Checked = true;
                }
                else
                {
                    chkGridConfigurar.Checked = false;
                }

                if (chkGridGrabar.Checked)
                {
                    chkGridGrabar.Checked = true;
                }
                else
                {
                    chkGridGrabar.Checked = false;
                }

                if (chkGridCancelarM.Checked)
                {
                    chkGridCancelarM.Checked = true;
                }
                else
                {
                    chkGridCancelarM.Checked = false;
                }
            }
            else
            {
                ctrlValidacionDetalle.MostrarValidacion("Formulario ya se encuentra agregado.", true, Enumerador.enmTipoMensaje.ERROR);
            }
            #endregion
        }  

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            CargarGrilla();            
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            chkRolActivo.Checked = true;

            gdvRolConfiguracion.DataSource = null;
            gdvRolConfiguracion.DataBind();

            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
            ctrlPaginador.InicializarPaginador();

            ctrlToolBarMantenimiento_btnCancelarHandler();

            updMantenimiento.Update();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            LimpiarDatosMantenimiento();
            LimpiarDatosMantenimientoDetalle();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }
        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            HabilitarMantenimiento();
            HabilitarMantenimientoDetalle();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            string strScript = string.Empty;           

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.ELIMINAR)
            {
                int IntRpta = 0;

                UsuarioRolConsultasBL BL = new UsuarioRolConsultasBL();
                IntRpta = BL.VerificaIntegridadReferencial(Convert.ToInt16(ObtenerFilaSeleccionada()["roco_sRolConfiguracionId"]));

                if (IntRpta > 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No puede eliminar este rol porque tiene usuarios relacionados.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            Session["Grabo"] = "NO";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "var yes = confirm('¿Desea realizar la operación?'); if (yes) __doPostBack('GrabarHandler', 'yes');", true);             
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;            
            LimpiarDatosMantenimiento();
            LimpiarDatosMantenimientoDetalle();
            HabilitarMantenimiento();
            HabilitarMantenimientoDetalle();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            // Consulta
            ddlAplicacionCons.SelectedValue = ((int)Enumerador.enmAplicacion.WEB).ToString();

            // Mantenimiento
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            // Mantenimiento Detalle
            tRolOpcion.Visible = false;
            chk01.Checked = false; chk02.Checked = false;
            chk03.Checked = false; chk04.Enabled = false;
            chk05.Checked = false; chk06.Checked = false;
            chk07.Checked = false; chk08.Checked = true;
            chk09.Checked = true; chk10.Checked = true;
            chk11.Checked = true; chk12.Checked = true;
            chk13.Checked = true; chk14.Checked = true;
            chk15.Checked = true; chk16.Checked = true;
            chk17.Checked = true; chk18.Checked = true;
            chk19.Checked = false; chk20.Checked = false;
            chk21.Checked = false; chk22.Checked = false;
            chk23.Checked = false; chk24.Checked = false;

            LimpiarDatosMantenimiento();
            LimpiarDatosMantenimientoDetalle();
        }

        private void CargarListadosDesplegables()
        {
            // Consulta
            //Util.CargarParametroDropDownList(ddlAplicacionCons, Comun.ObtenerSistemas(Session));

            DataTable dtSistemas = new DataTable();
            dtSistemas = Comun.ObtenerSistemasCargaInicial();

            Util.CargarParametroDropDownList(ddlAplicacionCons, dtSistemas);

            // Detalle
            //Util.CargarParametroDropDownList(ddlAplicacionMant, Comun.ObtenerSistemas(Session));
            Util.CargarParametroDropDownList(ddlAplicacionMant, dtSistemas);

            Util.CargarParametroDropDownList(ddlTipoRol, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_TIPO_ROL), true);

            //Proceso p = new Proceso();
            //object[] arrParametros = { Comun.ToNullInt32(ddlAplicacionMant.SelectedValue) };
            //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_ROLOPCION", "LISTAR");

            RolOpcionConsultasBL objRolOpcionConsultaBL = new RolOpcionConsultasBL();
            DataTable dt = new DataTable();
            dt = objRolOpcionConsultaBL.ObtenerOpcionesFormulario(Comun.ToNullInt32(ddlAplicacionMant.SelectedValue));

            Util.CargarDropDownList(ddlFormulario, dt, "form_vNombre", "form_sFormularioId");
        }

        private void CargarGrilla()
        {            
            //Proceso p = new Proceso();
            DataTable dt = new DataTable();
            //object[] arrParametros = ObtenerFiltro();
            //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_ROLCONFIGURACION", Enumerador.enmAccion.CONSULTAR);

            RolConfigConsultasBL objRolConfigConsultaBL = new RolConfigConsultasBL();
            int intTotalRegistros = 0, intTotalPaginas = 0;
            int intAplicacion = 0;

            string strEstado = string.Empty;
            string strAuditoria = string.Empty;
                        
            if (chkRolActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();            
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();            
            }

            if (ddlAplicacionCons.SelectedValue != string.Empty)
            {
                intAplicacion = Convert.ToInt32(ddlAplicacionCons.SelectedValue);
            }
            dt = objRolConfigConsultaBL.Consultar(ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas,
                                                    intAplicacion, "", strEstado, strAuditoria);


            //if (p.IErrorNumero == 0)
                            
                Session[strVariableDt] = dt;
                gdvRolConfiguracion.SelectedIndex = -1;
                gdvRolConfiguracion.DataSource = dt;
                gdvRolConfiguracion.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);                       
                }

                ctrlPaginador.TotalResgistros = Convert.ToInt32(intTotalRegistros);
                ctrlPaginador.TotalPaginas = Convert.ToInt32(intTotalPaginas);
                ctrlPaginador.Visible = false;

                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;

                updConsulta.Update();
            //}
            //else
            //{
            //    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
            //    Comun.EjecutarScript(Page, strScript);
            //}            
        }

        private void CargarGrillaDetalle()
        {            
            //Proceso p = new Proceso();
            int intTotalRegistros = 0, intTotalPaginas = 0;

            string strEstado = "";

            if (chkRolActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            //object[] arrParametros = {  1,
            //                            100,
            //                            intTotalRegistros,
            //                            intTotalPaginas,
            //                            Convert.ToInt32(ObtenerFilaSeleccionada()["roco_sRolConfiguracionId"]),
            //                            strEstado};

            bool bolHabilitar = false;
            if ((Enumerador.enmAccion)Session[strVariableAccion] == Enumerador.enmAccion.MODIFICAR)
            {
                bolHabilitar = true;
            }

            //DataTable dtOpciones = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_ROLOPCION", Enumerador.enmAccion.CONSULTAR);

            RolOpcionConsultasBL objRolOpcionConsultaBL = new RolOpcionConsultasBL();

            DataTable dtOpciones = new DataTable();

            dtOpciones = objRolOpcionConsultaBL.ObtenerPorRolConfiguracion(1, 100, ref intTotalRegistros, ref intTotalPaginas, Convert.ToInt32(ObtenerFilaSeleccionada()["roco_sRolConfiguracionId"]), strEstado);
            

            //if (p.IErrorNumero == 0)
            if (dtOpciones.Rows.Count > 0)
            {
                Session["dtRolOpcion"] = dtOpciones; 
                Session[strVariableDetalleDt] = dtOpciones;
                Session["dtRolOpcionAuxiliar"] = dtOpciones;
                PintarGrillaDetalleOpciones(bolHabilitar);
                updMantenimiento.Update();
            }
            else
            {
                ctrlValidacionDetalle.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
        }

        private object[] ObtenerFiltro()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;
            string strEstado = string.Empty;
                        
            if (chkRolActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();            
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();            
            }

            string strAuditoria = string.Empty;
            int intAplicacion = 0;

            if (ddlAplicacionCons.SelectedValue != string.Empty)
            {
                intAplicacion = Convert.ToInt32(ddlAplicacionCons.SelectedValue);
            }

            object[] arrParametros = {   
                                ctrlPaginador.PaginaActual,
                                Constantes.CONST_CANT_REGISTRO,
                                intTotalRegistros,
                                intTotalPaginas,
                                intAplicacion,
                                "",
                                strEstado,
                                strAuditoria
                            };

            return arrParametros;
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            if (Session[strVariableIndice] != null)
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
                    ddlAplicacionMant.SelectedValue = drSeleccionado["roco_sAplicacionId"].ToString();

                    BuscarItemCombo(drSeleccionado["roco_sRolTipoId"].ToString(), ref ddlTipoRol);
                    //ddlTipoRol.SelectedValue = drSeleccionado["roco_sRolTipoId"].ToString();
                    txtRolNombreMant.Text = drSeleccionado["roco_vNombre"].ToString();

                    if (drSeleccionado["roco_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkActivoMant.Checked = true;
                    }
                    else
                    {
                        chkActivoMant.Checked = false;
                    }

                    object objHorario = drSeleccionado["roco_cHorario"];

                    if (objHorario != null)
                    {
                        string strHorario = objHorario.ToString();

                        if (strHorario != string.Empty)
                        {
                            // FindControl...  y verificar el tamaño de la cadena strHorario
                            chk01.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(0,1)));
                            chk02.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(1, 1)));
                            chk03.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(2, 1)));
                            chk04.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(3, 1)));
                            chk05.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(4, 1)));
                            chk06.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(5, 1)));
                            chk07.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(6, 1)));
                            chk08.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(7, 1)));
                            chk09.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(8, 1)));
                            chk10.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(9, 1)));
                            chk11.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(10, 1)));
                            chk12.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(11, 1)));
                            chk13.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(12, 1)));
                            chk14.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(13, 1)));
                            chk15.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(14, 1)));
                            chk16.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(15, 1)));
                            chk17.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(16, 1)));
                            chk18.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(17, 1)));
                            chk19.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(18, 1)));
                            chk20.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(19, 1)));
                            chk21.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(20, 1)));
                            chk22.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(21, 1)));
                            chk23.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(22, 1)));
                            chk24.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(23, 1)));
                        }
                    }

                    LimpiarRegistroFormulario();

                    tRolOpcion.Visible = false;

                    updMantenimiento.Update();
                }
            }
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlAplicacionMant.Enabled = bolHabilitar;
            ddlTipoRol.Enabled = bolHabilitar;
            txtRolNombreMant.Enabled = bolHabilitar;

            chk01.Enabled = bolHabilitar; chk02.Enabled = bolHabilitar; 
            chk03.Enabled = bolHabilitar; chk04.Enabled = bolHabilitar;
            chk05.Enabled = bolHabilitar; chk06.Enabled = bolHabilitar;
            chk07.Enabled = bolHabilitar; chk08.Enabled = bolHabilitar;
            chk09.Enabled = bolHabilitar; chk10.Enabled = bolHabilitar;
            chk10.Enabled = bolHabilitar; chk11.Enabled = bolHabilitar;
            chk12.Enabled = bolHabilitar; chk13.Enabled = bolHabilitar;
            chk14.Enabled = bolHabilitar; chk15.Enabled = bolHabilitar;
            chk16.Enabled = bolHabilitar; chk17.Enabled = bolHabilitar;
            chk17.Enabled = bolHabilitar; chk18.Enabled = bolHabilitar;
            chk19.Enabled = bolHabilitar; chk20.Enabled = bolHabilitar;
            chk21.Enabled = bolHabilitar; chk22.Enabled = bolHabilitar;
            chk23.Enabled = bolHabilitar; chk24.Enabled = bolHabilitar;
            chkActivoMant.Enabled = bolHabilitar;
        }

        private void HabilitarMantenimientoDetalle(bool bolHabilitar = true)
        {
            btnMostrarAgregarOpcion.Visible = bolHabilitar;

            gdvRolOpcion.Columns[gdvRolOpcion.Columns.Count - 1].Visible = bolHabilitar;

            foreach (GridViewRow fila in gdvRolOpcion.Rows)
            {
                CheckBox chkBuscar = (CheckBox)fila.FindControl("chkGridBuscar");
                chkBuscar.Enabled = bolHabilitar;

                CheckBox chkImprimir = (CheckBox)fila.FindControl("chkGridImprimir");
                chkImprimir.Enabled = bolHabilitar;

                CheckBox chkCancelarC = (CheckBox)fila.FindControl("chkGridCancelarC");
                chkCancelarC.Enabled = bolHabilitar;

                CheckBox chkCerrar = (CheckBox)fila.FindControl("chkGridCerrar");
                chkCerrar.Enabled = bolHabilitar;

                CheckBox chkRegistrar = (CheckBox)fila.FindControl("chkGridRegistrar");
                chkRegistrar.Enabled = bolHabilitar;

                CheckBox chkModificar = (CheckBox)fila.FindControl("chkGridModificar");
                chkModificar.Enabled = bolHabilitar;

                CheckBox chkEliminar = (CheckBox)fila.FindControl("chkGridEliminar");
                chkEliminar.Enabled = bolHabilitar;

                CheckBox chkConfigurar = (CheckBox)fila.FindControl("chkGridConfigurar");
                chkConfigurar.Enabled = bolHabilitar;

                CheckBox chkGrabar = (CheckBox)fila.FindControl("chkGridGrabar");
                chkGrabar.Enabled = bolHabilitar;

                CheckBox chkCancelarM = (CheckBox)fila.FindControl("chkGridCancelarM");
                chkCancelarM.Enabled = bolHabilitar;
            }
        }

        private void LimpiarDatosMantenimiento()
        {                       
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            ddlTipoRol.SelectedIndex = 0;
            txtRolNombreMant.Text = "";
            tRolOpcion.Visible = false;

            chk01.Checked = false; chk02.Checked = false;
            chk03.Checked = false; chk04.Enabled = false;
            chk05.Checked = false; chk06.Checked = false;
            chk07.Checked = false; chk08.Checked = true;
            chk09.Checked = true; chk10.Checked = true;
            chk11.Checked = true; chk12.Checked = true;
            chk13.Checked = true; chk14.Checked = true;
            chk15.Checked = true; chk16.Checked = true;
            chk17.Checked = true; chk18.Checked = true;
            chk19.Checked = false; chk20.Checked = false;
            chk21.Checked = false; chk22.Checked = false;
            chk23.Checked = false; chk24.Checked = false;
            chkActivoMant.Checked = true;
        }

        private void LimpiarDatosMantenimientoDetalle()
        {
            btnMostrarAgregarOpcion.Visible = true;
            Session[strVariableIndice] = 0;
            Session[strVariableDetalleDt] = null;
            Session["dtRolOpcionAuxiliar"] = null;
            LimpiarRegistroFormulario();
            gdvRolOpcion.DataSource = new DataTable();
            gdvRolOpcion.DataBind();
        }
        
        private SGAC.BE.SE_ROLCONFIGURACION ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();

                SGAC.BE.SE_ROLCONFIGURACION objEntidad = new SGAC.BE.SE_ROLCONFIGURACION();
                objEntidad.roco_sRolConfiguracionId = Convert.ToInt16(drSeleccionado["roco_sRolConfiguracionId"]);
                objEntidad.roco_sAplicacionId = Convert.ToInt16(drSeleccionado["roco_sAplicacionId"]);
                objEntidad.roco_vRolOpcion = drSeleccionado["roco_vRolOpcion"].ToString();
                objEntidad.roco_sRolTipoId = Convert.ToInt16(drSeleccionado["roco_sRolTipoId"]);
                objEntidad.roco_vNombre = drSeleccionado["roco_vNombre"].ToString();
                objEntidad.roco_cHorario = drSeleccionado["roco_cHorario"].ToString();
                objEntidad.roco_cEstado = drSeleccionado["roco_cEstado"].ToString();

                return objEntidad;
            }

            return null;
        }

        private SGAC.BE.SE_ROLCONFIGURACION ObtenerEntidadMantenimiento()
        {
            SGAC.BE.SE_ROLCONFIGURACION objEntidad = new SGAC.BE.SE_ROLCONFIGURACION();

            if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                objEntidad.roco_sRolConfiguracionId = Convert.ToInt16(ObtenerFilaSeleccionada()["roco_sRolConfiguracionId"]);
                objEntidad.roco_vRolOpcion = ObtenerFilaSeleccionada()["roco_vRolOpcion"].ToString();
            }
            else
            {
                objEntidad.roco_vRolOpcion = string.Empty; // En la capa Datos se actualizará el valor
            }

            objEntidad.roco_sAplicacionId = Convert.ToInt16(ddlAplicacionMant.SelectedValue); 
            objEntidad.roco_sRolTipoId = Convert.ToInt16(ddlTipoRol.SelectedValue);            
            objEntidad.roco_vNombre = txtRolNombreMant.Text.Trim().ToUpper().Replace("'","''");
            objEntidad.roco_cHorario = ObtenerHorario();

            if (chkActivoMant.Checked)
            {
                objEntidad.roco_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                objEntidad.roco_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            objEntidad.roco_sUsuarioCreacion= Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.roco_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            objEntidad.roco_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.roco_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

            objEntidad.DiferenciaHoraria = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria"));
            objEntidad.HorarioVerano = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano"));
            objEntidad.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            return objEntidad;
        }

        private List<SGAC.BE.SE_ROLOPCION> ObtenerListadoEntidadDetalle()
        {
            List<SGAC.BE.SE_ROLOPCION> lstEntidadDetalle = new List<BE.SE_ROLOPCION>();

            DataTable dtRolOpcion = (DataTable)Session["dtRolOpcionAuxiliar"];

            foreach (GridViewRow row in gdvRolOpcion.Rows)
            {
                Session["index"] = Convert.ToInt32(row.RowIndex);

                if (gdvRolOpcion.Rows[Convert.ToInt32(Session["index"])].Cells[1].Text.ToString().Equals(((char)Enumerador.enmEstado.ACTIVO).ToString()))
                {
                    SGAC.BE.SE_ROLOPCION objEntidadDetalle = new SGAC.BE.SE_ROLOPCION();

                    objEntidadDetalle.roop_sRolOpcionId = Convert.ToInt16(gdvRolOpcion.Rows[Convert.ToInt32(Session["index"])].Cells[0].Text);
                    objEntidadDetalle.roop_sFormularioId = Convert.ToInt16(gdvRolOpcion.Rows[Convert.ToInt32(Session["index"])].Cells[2].Text);
                    objEntidadDetalle.roop_vAcciones = ObtenerAcciones(row);
                    objEntidadDetalle.roop_cEstado = gdvRolOpcion.Rows[Convert.ToInt32(Session["index"])].Cells[1].Text;
                    objEntidadDetalle.roop_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objEntidadDetalle.roop_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    objEntidadDetalle.roop_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objEntidadDetalle.roop_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                    objEntidadDetalle.DiferenciaHoraria = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria"));
                    objEntidadDetalle.HorarioVerano = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano"));
                    objEntidadDetalle.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    lstEntidadDetalle.Add(objEntidadDetalle);
                }
            }           

            return lstEntidadDetalle;
        }

        private void GrabarHandler()
        {
            string strScript = string.Empty;

            RolConfigMantenimientoBL RolConfBL = new RolConfigMantenimientoBL();
            bool Error = true;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    RolConfBL.Insertar(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);
                    break;

                case Enumerador.enmAccion.MODIFICAR:
                    RolConfBL.Actualizar(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);
                    break;

                case Enumerador.enmAccion.ELIMINAR:
                    RolConfBL.Eliminar(ObtenerEntidadMantenimiento(), ref Error);
                    break;
            }
           
            if (!Error)
            {
                LimpiarDatosMantenimiento();
                HabilitarMantenimiento();

                Session["Grabo"] = "SI";

                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    ctrlPaginador.PaginaActual = 1;
                    ctrlPaginador.InicializarPaginador();

                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    ctrlPaginador.PaginaActual = 1;
                    ctrlPaginador.InicializarPaginador();

                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                CargarGrilla();
            }
            else
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }           

            Comun.EjecutarScript(Page, strScript);
        }

        private void LimpiarRegistroFormulario()
        {
            ddlFormulario.SelectedIndex = -1;
            chkGridBuscar.Checked = false;
            chkGridImprimir.Checked = false;
            chkGridCancelarC.Checked = false;
            chkGridCerrar.Checked = false;

            chkGridRegistrar.Checked = false;
            chkGridModificar.Checked = false;
            chkGridEliminar.Checked = false;
            chkGridConfigurar.Checked = false;
            chkGridGrabar.Checked = false;
            chkGridCancelarM.Checked = false;
        }

        private string ObtenerHorario()
        {
            string strHorario = string.Empty;
            strHorario += Convert.ToInt32(chk01.Checked).ToString();
            strHorario += Convert.ToInt32(chk02.Checked).ToString();
            strHorario += Convert.ToInt32(chk03.Checked).ToString();
            strHorario += Convert.ToInt32(chk04.Checked).ToString();
            strHorario += Convert.ToInt32(chk05.Checked).ToString();
            strHorario += Convert.ToInt32(chk06.Checked).ToString();
            strHorario += Convert.ToInt32(chk07.Checked).ToString();
            strHorario += Convert.ToInt32(chk08.Checked).ToString();
            strHorario += Convert.ToInt32(chk09.Checked).ToString();
            strHorario += Convert.ToInt32(chk10.Checked).ToString();
            strHorario += Convert.ToInt32(chk11.Checked).ToString();
            strHorario += Convert.ToInt32(chk12.Checked).ToString();
            strHorario += Convert.ToInt32(chk13.Checked).ToString();
            strHorario += Convert.ToInt32(chk14.Checked).ToString();
            strHorario += Convert.ToInt32(chk15.Checked).ToString();
            strHorario += Convert.ToInt32(chk16.Checked).ToString();
            strHorario += Convert.ToInt32(chk17.Checked).ToString();
            strHorario += Convert.ToInt32(chk18.Checked).ToString();
            strHorario += Convert.ToInt32(chk19.Checked).ToString();
            strHorario += Convert.ToInt32(chk20.Checked).ToString();
            strHorario += Convert.ToInt32(chk21.Checked).ToString();
            strHorario += Convert.ToInt32(chk22.Checked).ToString();
            strHorario += Convert.ToInt32(chk23.Checked).ToString();
            strHorario += Convert.ToInt32(chk24.Checked).ToString();
            return strHorario;
        }

        private string ObtenerAcciones(GridViewRow fila)
        {
            string strAcciones = string.Empty;

            CheckBox chkBuscar = (CheckBox)fila.FindControl("chkGridBuscar");
            if (chkBuscar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.BUSCAR).ToString() + '|';
            }            

            CheckBox chkImprimir = (CheckBox)fila.FindControl("chkGridImprimir");
            if (chkImprimir.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.IMPRIMIR).ToString() + '|';
            }            

            CheckBox chkCancelarC = (CheckBox)fila.FindControl("chkGridCancelarC");
            if (chkCancelarC.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.CANCELAR_C).ToString() + '|';
            }
           
            CheckBox chkCerrar = (CheckBox)fila.FindControl("chkGridCerrar");
            if (chkCerrar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.CERRAR).ToString() + '|';
            }

            CheckBox chkRegistrar = (CheckBox)fila.FindControl("chkGridRegistrar");
            if (chkRegistrar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.NUEVO).ToString() + '|';
            }

            CheckBox chkModificar = (CheckBox)fila.FindControl("chkGridModificar");
            if (chkModificar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.MODIFICAR).ToString() + '|';
            }

            CheckBox chkEliminar = (CheckBox)fila.FindControl("chkGridEliminar");
            if (chkEliminar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.ELIMINAR).ToString() + '|';
            }

            CheckBox chkConfigurar = (CheckBox)fila.FindControl("chkGridConfigurar");
            if (chkConfigurar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.CONFIGURAR).ToString() + '|';
            }

            CheckBox chkGrabar = (CheckBox)fila.FindControl("chkGridGrabar");
            if (chkGrabar.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.GRABAR).ToString() + '|';
            }

            CheckBox chkCancelarM = (CheckBox)fila.FindControl("chkGridCancelarM");
            if (chkCancelarM.Checked)
            {
                strAcciones += ((char)Enumerador.enmPermisoAccion.CANCELAR_M).ToString() + '|';
            }

            if (strAcciones.Length > 1)
            {
                return strAcciones.Substring(0, strAcciones.Length - 1);
            }
            else
            {
                return strAcciones;
            }
        }

        private string ObtenerAcciones(DataRow drDetalle)
        {
            string strAcciones = string.Empty;

            if (Convert.ToBoolean(drDetalle["op_buscar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.BUSCAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_imprimir"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.IMPRIMIR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_cancelar_c"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.CANCELAR_C).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_cerrar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.CERRAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_registrar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.NUEVO).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_modificar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.MODIFICAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_eliminar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.ELIMINAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_configurar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.CONFIGURAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_grabar"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.GRABAR).ToString() + '|';

            if (Convert.ToBoolean(drDetalle["op_cancelar_m"]))
                strAcciones += ((char)Enumerador.enmPermisoAccion.CANCELAR_M).ToString() + '|';

            if (strAcciones.Length > 1)
            {
                return strAcciones.Substring(0, strAcciones.Length - 1);
            }
            else
            {
                return strAcciones;
            }
        }

        private bool ExisteFormularioRolOpcion()
        {
            bool bolExiste = false;

            DataTable dtOpciones = new DataTable();
            dtOpciones = (DataTable)Session["dtRolOpcionAuxiliar"];

            string strFormularioId = string.Empty;
            string strFormularioSel = ddlFormulario.SelectedValue;

            if (dtOpciones != null)
            {
                foreach (DataRow dr in dtOpciones.Rows)
                {
                    strFormularioId = dr["form_sFormularioId"].ToString();
                    if (strFormularioSel == strFormularioId)
                    {
                        bolExiste = true;
                        break;
                    }
                }
            }

            return bolExiste;
        }

        private void PintarGrillaDetalleOpciones(bool bolHabilitar=true)
        {
            DataTable dtOpciones = ((DataTable)Session["dtRolOpcionAuxiliar"]).Copy();
            gdvRolOpcion.DataSource = dtOpciones;
            gdvRolOpcion.DataBind();

            foreach (GridViewRow fila in gdvRolOpcion.Rows)
            {
                CheckBox chkBuscar = (CheckBox)fila.FindControl("chkGridBuscar");
                chkBuscar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_buscar"]);
                chkBuscar.Enabled = bolHabilitar;

                CheckBox chkImprimir = (CheckBox)fila.FindControl("chkGridImprimir");
                chkImprimir.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_imprimir"]);
                chkImprimir.Enabled = bolHabilitar;

                CheckBox chkCancelarC = (CheckBox)fila.FindControl("chkGridCancelarC");
                chkCancelarC.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_cancelar_c"]);
                chkCancelarC.Enabled = bolHabilitar;

                CheckBox chkCerrar = (CheckBox)fila.FindControl("chkGridCerrar");
                chkCerrar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_cerrar"]);
                chkCerrar.Enabled = bolHabilitar;

                CheckBox chkRegistrar = (CheckBox)fila.FindControl("chkGridRegistrar");
                chkRegistrar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_registrar"]);
                chkRegistrar.Enabled = bolHabilitar;

                CheckBox chkModificar = (CheckBox)fila.FindControl("chkGridModificar");
                chkModificar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_modificar"]);
                chkModificar.Enabled = bolHabilitar;

                CheckBox chkEliminar = (CheckBox)fila.FindControl("chkGridEliminar");
                chkEliminar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_eliminar"]);
                chkEliminar.Enabled = bolHabilitar;

                CheckBox chkConfigurar = (CheckBox)fila.FindControl("chkGridConfigurar");
                chkConfigurar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_configurar"]);
                chkConfigurar.Enabled = bolHabilitar;

                CheckBox chkGrabar = (CheckBox)fila.FindControl("chkGridGrabar");
                chkGrabar.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_grabar"]);
                chkGrabar.Enabled = bolHabilitar;

                CheckBox chkCancelarM = (CheckBox)fila.FindControl("chkGridCancelarM");
                chkCancelarM.Checked = Convert.ToBoolean(dtOpciones.Rows[fila.DataItemIndex]["op_cancelar_m"]);
                chkCancelarM.Enabled = bolHabilitar;
            }
        }

        private void PintarGrillaDetalleFilas()
        {
            DataTable dtRolOpcion = ((DataTable)Session["dtRolOpcionAuxiliar"]).Copy();
            foreach (GridViewRow row in gdvRolOpcion.Rows)
            {
                if (dtRolOpcion.Rows[row.RowIndex]["roop_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                {
                    row.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    row.BackColor = System.Drawing.Color.FromName("#FAD4D9"); 
                }
            }            
        }

        private DataTable CrearTablaRolOpcion()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("roop_sRolOpcionId", typeof(Int16));
            dt.Columns.Add("form_sAplicacionId", typeof(Int16));
            dt.Columns.Add("form_sFormularioId", typeof(Int16));
            dt.Columns.Add("form_vFormulario", typeof(string));
            dt.Columns.Add("form_sReferencia", typeof(int));
            dt.Columns.Add("form_sOrden", typeof(int));
            dt.Columns.Add("form_vRuta", typeof(string));
            dt.Columns.Add("roop_vAcciones", typeof(string));
            dt.Columns.Add("roop_cEstado", typeof(string));
            dt.Columns.Add("iProcesado", typeof(int));
            dt.Columns.Add("op_buscar", typeof(bool));
            dt.Columns.Add("op_imprimir", typeof(bool));
            dt.Columns.Add("op_cancelar_c", typeof(bool));
            dt.Columns.Add("op_cerrar", typeof(bool));
            dt.Columns.Add("op_registrar", typeof(bool));
            dt.Columns.Add("op_modificar", typeof(bool));
            dt.Columns.Add("op_eliminar", typeof(bool));
            dt.Columns.Add("op_configurar", typeof(bool));
            dt.Columns.Add("op_grabar", typeof(bool));
            dt.Columns.Add("op_cancelar_m", typeof(bool));

            return dt;
        }

        #endregion

        protected void ddlAplicacionMant_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Proceso p = new Proceso();
            //object[] arrParametros = { Comun.ToNullInt32(ddlAplicacionMant.SelectedValue) };
            //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_ROLOPCION", "LISTAR");

            RolOpcionConsultasBL objRolOpcionConsultaBL = new RolOpcionConsultasBL();

            DataTable dt = new DataTable();

            dt = objRolOpcionConsultaBL.ObtenerOpcionesFormulario(Comun.ToNullInt32(ddlAplicacionMant.SelectedValue));

            Util.CargarDropDownList(ddlFormulario, dt, "form_vNombre", "form_sFormularioId");

            updMantenimiento.Update();
        }


        private void BuscarItemCombo(string strElementoId, ref DropDownList ddlObjeto)
        {
            ddlObjeto.SelectedIndex = 0;
            for (int i = 0; i < ddlObjeto.Items.Count; i++)
            {
                if (ddlObjeto.Items[i].Value.Equals(strElementoId))
                {
                    ddlObjeto.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}