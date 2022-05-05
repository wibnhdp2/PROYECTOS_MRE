using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmUsuario : MyBasePage
    { 
        #region CAMPOS
        private string strNombreEntidad = "USUARIO";
        private string strVariableAccion = "Usuario_Accion";
        private string strVariableDt = "Usuario_Tabla";
        private string strVariableIndice = "Usuario_Indice";
        #endregion

        #region EVENTOS
        private void Page_Init(object sender, System.EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);            
            
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnConfigurationHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonConfigurationClick(ctrlToolBarMantenimiento_btnConfigurationHandler);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";
            imgValidarUsuario.OnClientClick = "return ValidarCuentaRed();";
            
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";          

            /* Setea el rango ingreso de fechas permitido del control calendario */
            this.dtpFecCaducidad.AllowFutureDate = true;
            this.dtpFecCaducidad.StartDate = ObtenerFechaActual(HttpContext.Current.Session);
            this.dtpFecCaducidad.EndDate = ObtenerFechaActual(HttpContext.Current.Session).AddYears(30);

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvUsuario, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                    GrabarHandler();
                else
                    CargarGrilla();
            }

            if (!Page.IsPostBack)
            {
                Session["Grabo"] = "NO";
                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updConsulta.Update();
        }

        protected void gdvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
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
                PintarSeleccionado(intSeleccionado);

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;              

                HabilitarMantenimiento(true);
                PintarSeleccionado(intSeleccionado);

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        } 
       
        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            Session[strVariableDt] = new DataTable();            
            CargarGrilla();
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            /* No tiene implementación */
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlOficinaConsularConsulta.SelectedIndex = 0;
            txtDNIConsulta.Text = "";
            txtNombresConsulta.Text = "";
            txtApeMaternoConsulta.Text = "";
            txtApePaternoConsulta.Text = "";
            txtDireccionIP.Text = "";
            chkEstadoConsulta.Checked = true;

            gdvUsuario.DataSource = null;
            gdvUsuario.DataBind();

            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
            ctrlPaginador.InicializarPaginador();

            ctrlToolBarMantenimiento_btnCancelarHandler();

            updMantenimiento.Update();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;            
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
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            txtCuentaRedMant.Style.Add("border", "solid #888888 1px");

            string strScript = string.Empty;
            string strResultadoValidacion = string.Empty;
            DataTable DtUsro = new DataTable();
            UsuarioConsultasBL UsBL = new UsuarioConsultasBL();
            UsuarioRolConsultasBL UsroBL = new UsuarioRolConsultasBL(); 

            /* ESTA VALIDACION SE ESTA REALIZANDO TEMPORALMENTE HAS QUE SE IMPLENTE EL BLACK LIST CORRESPONDIENTE */
            if (txtCuentaRedMant.Text.ToUpper() == "GUEST" || txtCuentaRedMant.Text.ToUpper() == "ADMINISTRATOR")
            {              
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Las cuentas de red GUEST O ADMINISTRATOR no pueden ser procesados en la BD de usuarios de este sistema porque son de carácter reservado.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            bool bolValidado = DirectorioActivo.ExisteUsuario(txtCuentaRedMant.Text);
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                if (!bolValidado)
                {
                    txtCuentaRedMant.Style.Add("border", "solid Red 1px");
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "La cuenta de red no está registrada en el dominio.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                strResultadoValidacion = UsBL.ValidarUsuarioDocumento(txtDNIMant.Text, txtCuentaRedMant.Text.ToUpper(), (int)Enumerador.enmAccion.INSERTAR);
                if (strResultadoValidacion != string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, strResultadoValidacion, false, 200, 300);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                DtUsro = UsroBL.Validar(txtCuentaRedMant.Text.ToUpper(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                if (DtUsro.Rows.Count != 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "El Alias ya se encuentra asignado a un rol.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            else if (enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                if (!bolValidado)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "La cuenta de red no está registrada en el dominio.");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                strResultadoValidacion = UsBL.ValidarUsuarioDocumento(txtDNIMant.Text, txtCuentaRedMant.Text, (int)Enumerador.enmAccion.MODIFICAR);
                if (strResultadoValidacion != string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, strResultadoValidacion, false, 200, 300);
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
            Comun.EjecutarScript(Page, Util.ActivarTab(0) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnConfigurationHandler()
        {
            /* No tiene implementación */
        }

        protected void imgValidarUsuario_Click(object sender, ImageClickEventArgs e)
        {
            bool bolValidado = DirectorioActivo.ExisteUsuario(txtCuentaRedMant.Text);

            if (bolValidado)
            {                    
                ctrlValidacionMant.MostrarValidacion("Usuario Válido.", true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacionMant.MostrarValidacion("Usuario No Válido.", true, Enumerador.enmTipoMensaje.WARNING);
            }

            updMantenimiento.Update();
        }
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            /* Variables de Sesión */
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            /* Habilitar */
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            /* Datos - Consulta, Mantenimiento */
            ddlOficinaConsularConsulta.SelectedValue = ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();
            chkEstadoConsulta.Checked = true;

            dtpFecCaducidad.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.ToString("dd") + "-" + DateTime.Today.ToString("yyyy");
                        
            CtrlOficinaConsularConfig.SelectedValue = ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();
            ddlRolConfiguracion.SelectedIndex = 0;
            ddlAcceso.SelectedValue = ((int)Enumerador.enmAccesoUsuario.INDIVIDUAL).ToString();
            ddlEntidadMant.SelectedValue = ((int)Enumerador.enmEntidad.MRE).ToString();
            chkActivoMant.Checked = true;

            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            /* Filtro por atributos del usuario */
            if ((int)Session[Constantes.CONST_SESION_ACCESO_ID] == (int)Enumerador.enmAccesoUsuario.INDIVIDUAL || 
                (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                /* Consulta */
                ddlOficinaConsularConsulta.Cargar(false, false);
            }
            else
            {
                ddlOficinaConsularConsulta.Cargar(false, true, "- TODAS -");
            }

            /* Detalle */
            CtrlOficinaConsularConfig.Cargar(false, false);

            Util.CargarParametroDropDownList(ddlEntidadMant, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_ENTIDAD)); 

            RolConfigConsultasBL objBL = new RolConfigConsultasBL();
            DataTable dtConfiguracion = objBL.Listar((int)Enumerador.enmAplicacion.WEB);

            Util.CargarDropDownList(ddlRolConfiguracion, dtConfiguracion, "roco_vNombre", "roco_sRolConfiguracionId", true);
            Util.CargarParametroDropDownList(ddlAcceso, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SISTEMA_ACCESOS));
        }

        private void CargarGrilla()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;
            DropDownList ComboConsulta = (DropDownList)ddlOficinaConsularConsulta.FindControl("ddlOficinaConsular");
            int IntCuentaOFc = Convert.ToInt32(ComboConsulta.Items.Count.ToString());
            int intOficinaSeleccionada = 0;
            if (IntCuentaOFc > 0)
            {
                intOficinaSeleccionada = Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue);
            }
            string strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            if (!chkEstadoConsulta.Checked)
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();

            UsuarioConsultasBL objBL = new UsuarioConsultasBL();
            DataTable dt = new DataTable();
            dt = objBL.ObtenerPorFiltros(ctrlPaginador.PaginaActual,Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas,
                intOficinaSeleccionada, 
                txtDNIConsulta.Text.Trim(), txtNombresConsulta.Text.Trim(),txtApePaternoConsulta.Text.Trim(), txtApeMaternoConsulta.Text.Trim(),
                strEstado);

            Session[strVariableDt] = dt;
            gdvUsuario.SelectedIndex = -1;
            gdvUsuario.DataSource = dt;
            gdvUsuario.DataBind();

            if (dt.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
            }

            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;

            ctrlPaginador.Visible = false;
            if (ctrlPaginador.TotalPaginas > 1)
            {
                ctrlPaginador.Visible = true;
            }                

            updConsulta.Update();
        }

        private object[] ObtenerFiltro()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;

            object[] arrParametros = new object[10];
            arrParametros[0] = ctrlPaginador.PaginaActual;
            arrParametros[1] = Constantes.CONST_CANT_REGISTRO;
            arrParametros[2] = intTotalRegistros;
            arrParametros[3] = intTotalPaginas;

             //Evaluar el numero de registros que tiene el combo
            DropDownList ComboConsulta = (DropDownList)ddlOficinaConsularConsulta.FindControl("ddlOficinaConsular");
            int IntCuentaOFc = Convert.ToInt32(ComboConsulta.Items.Count.ToString());

            int intOficinaSeleccionada = 0;

            if (IntCuentaOFc > 0)
            {
                intOficinaSeleccionada = Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue);
            }
            else {
                arrParametros[4] = 0;            
            }

            arrParametros[4] = intOficinaSeleccionada;
            arrParametros[5] = txtDNIConsulta.Text.Trim();
            arrParametros[6] = txtNombresConsulta.Text.Trim();
            arrParametros[7] = txtApePaternoConsulta.Text.Trim();
            arrParametros[8] = txtApeMaternoConsulta.Text.Trim();

            if (chkEstadoConsulta.Checked)
            {
                arrParametros[9] = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                arrParametros[9] = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }
         
            return arrParametros;
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            if (Session != null)
            {
                DataTable dt = (DataTable)Session[strVariableDt];
                int intSeleccionado = (int)Session[strVariableIndice];                
                DataRow dr = dt.Rows[intSeleccionado];
                return dr;
            }

            return null;
        }

        private void PintarSeleccionado(int intSeleccionado)
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();

                if (drSeleccionado != null)
                {
                    txtDNIMant.Text = drSeleccionado["usua_vDocumentoNumero"].ToString();
                    txtNombresMant.Text = drSeleccionado["usua_vNombres"].ToString();                    
                    txtApePaternoMant.Text = drSeleccionado["usua_vApellidoPaterno"].ToString();
                    txtApeMaternoMant.Text = drSeleccionado["usua_vApellidoMaterno"].ToString();
                    txtCorreoMant.Text = drSeleccionado["usua_vCorreoElectronico"].ToString();
                    txtTelefono.Text = drSeleccionado["usua_vTelefono"].ToString();
                    txtDireccion.Text = drSeleccionado["usua_vDireccion"].ToString();
                    txtCuentaRedMant.Text = drSeleccionado["usua_vAlias"].ToString();
                    txtDireccionIP.Text = drSeleccionado["usua_vDireccionIP"].ToString();                    

                    if (drSeleccionado["usua_sEmpresaId"] != null)
                    {
                        ddlEntidadMant.SelectedValue = drSeleccionado["usua_sEmpresaId"].ToString();
                    }

                    if (drSeleccionado["usua_dFechaCaducidad"] != null)
                    {
                        if (drSeleccionado["usua_dFechaCaducidad"].ToString() != string.Empty)
                        {
                            dtpFecCaducidad.Text = Comun.FormatearFecha(drSeleccionado["usua_dFechaCaducidad"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                        }
                    }

                    if (drSeleccionado["usua_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkActivoMant.Checked = true;
                    }
                    else
                    {
                        chkActivoMant.Checked = false;
                    }

                    if (drSeleccionado["usua_bSesionActiva"].ToString() == "True")
                    {
                        chkActivoSesion.Checked = true;
                    }
                    else
                    {
                        chkActivoSesion.Checked = false;
                    }
                    if (drSeleccionado["usua_bBloqueoActiva"].ToString() == "True")
                    {
                        chkActivoBloqueo.Checked = true;
                    }
                    else
                    {
                        chkActivoBloqueo.Checked = false;
                    }

                    if (drSeleccionado["usua_bNotificaRemesa"].ToString() == "True")
                    {
                        chkNotificaRemesa.Checked = true;
                    }
                    else
                    {
                        chkNotificaRemesa.Checked = false;
                    }
                    

                    CtrlOficinaConsularConfig.SelectedValue = drSeleccionado["usro_sOficinaConsularId"].ToString();
                    
                    ddlRolConfiguracion.SelectedValue = drSeleccionado["usro_sRolConfiguracionId"].ToString();

                    if (drSeleccionado["usro_sAcceso"].ToString() != "0")
                    {
                        ddlAcceso.SelectedValue = drSeleccionado["usro_sAcceso"].ToString();
                    }

                    updMantenimiento.Update();
                }
            }
        }
        
        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            txtDNIMant.Enabled = bolHabilitar;
            txtNombresMant.Enabled = bolHabilitar;
            txtApePaternoMant.Enabled = bolHabilitar;
            txtApeMaternoMant.Enabled = bolHabilitar;
            txtCorreoMant.Enabled = bolHabilitar;
            txtCuentaRedMant.Enabled = bolHabilitar;
            ddlEntidadMant.Enabled = bolHabilitar;            

            txtDireccionIP.Enabled = bolHabilitar;

            chkActivoMant.Enabled = bolHabilitar;
            chkActivoBloqueo.Enabled = bolHabilitar;
            chkNotificaRemesa.Enabled = bolHabilitar;
            txtTelefono.Enabled = bolHabilitar;
            txtDireccion.Enabled = bolHabilitar;
             
            CtrlOficinaConsularConfig.Enabled = bolHabilitar;
            ddlRolConfiguracion.Enabled = bolHabilitar;
            ddlAcceso.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;           

            txtDNIMant.Text = "";
            txtNombresMant.Text = "";
            txtApePaternoMant.Text = "";
            txtApeMaternoMant.Text = "";
            txtCorreoMant.Text = "";
            txtCuentaRedMant.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            ddlEntidadMant.SelectedValue = ((int)Enumerador.enmEntidad.MRE).ToString();
            ddlAcceso.SelectedValue = ((int) Enumerador.enmAccesoUsuario.INDIVIDUAL).ToString();

            dtpFecCaducidad.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.ToString("dd") + "-" + DateTime.Today.ToString("yyyy");

            txtDireccionIP.Text = "";

            chkActivoMant.Checked = true;
            chkActivoBloqueo.Checked = false;
            chkNotificaRemesa.Checked = false;

            CtrlOficinaConsularConfig.SelectedIndex = -1;
            ddlRolConfiguracion.SelectedIndex = -1;          
        }

        private SGAC.BE.MRE.SE_USUARIO ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                DataTable dt = (DataTable)Session[strVariableDt];
                DataRow drSeleccionado = dt.Rows[intSeleccionado];

                SGAC.BE.MRE.SE_USUARIO objEntidad = new SGAC.BE.MRE.SE_USUARIO();
                objEntidad.usua_sUsuarioId = Convert.ToInt16(drSeleccionado["usua_sUsuarioId"]);
                objEntidad.usua_vAlias = drSeleccionado["usua_vAlias"].ToString();                
                objEntidad.usua_vNombres = drSeleccionado["usua_vNombres"].ToString();
                objEntidad.usua_vApellidoPaterno = drSeleccionado["usua_vApellidos"].ToString();
                objEntidad.usua_vApellidoMaterno = drSeleccionado["usua_vApellidos"].ToString();
                objEntidad.usua_vCorreoElectronico = drSeleccionado["usua_vCorreoElectronico"].ToString();
                objEntidad.usua_vDireccion = drSeleccionado["usua_vDireccion"].ToString();
                objEntidad.usua_vTelefono = drSeleccionado["usua_vTelefono"].ToString();
                objEntidad.usua_sDocumentoTipoId = (int) Enumerador.enmTipoDocumento.DNI; 
                objEntidad.usua_vDocumentoNumero = drSeleccionado["usua_vDocumentoNumero"].ToString();
                objEntidad.usua_sEmpresaId = Convert.ToInt16(drSeleccionado["usua_iEmpresaId"]);
                objEntidad.usua_sReferenciaId = Convert.ToInt16(drSeleccionado["usua_sReferenciaId"]);
                objEntidad.usua_dFechaCaducidad = Comun.FormatearFecha(dtpFecCaducidad.Text);
                objEntidad.usua_vDireccionIP = txtDireccionIP.Text;
                objEntidad.usua_cEstado = drSeleccionado["usua_cEstado"].ToString();
                objEntidad.usua_bSesionActiva = Convert.ToBoolean(drSeleccionado["usua_bSesionActiva"].ToString());
                objEntidad.usua_bBloqueoActiva = Convert.ToBoolean(drSeleccionado["usua_bBloqueoActiva"].ToString());
                objEntidad.usua_bNotificaRemesa = Convert.ToBoolean(drSeleccionado["usua_bNotificaRemesa"].ToString());

                return objEntidad;
            }

            return null;
        }

        private SGAC.BE.MRE.SE_USUARIO ObtenerEntidadMantenimiento()
        {
            SGAC.BE.MRE.SE_USUARIO objEntidad = new SGAC.BE.MRE.SE_USUARIO();

            if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                objEntidad.usua_sUsuarioId = Convert.ToInt16(ObtenerFilaSeleccionada()["usua_sUsuarioId"]);
            }          

            objEntidad.usua_sDocumentoTipoId = (int)Enumerador.enmTipoDocumento.DNI; 
            objEntidad.usua_vDocumentoNumero = txtDNIMant.Text.Trim().ToUpper();
            objEntidad.usua_vNombres = txtNombresMant.Text.Trim().ToUpper();
            objEntidad.usua_vApellidoPaterno = txtApePaternoMant.Text.Trim().ToUpper();
            objEntidad.usua_vApellidoMaterno = txtApeMaternoMant.Text.Trim().ToUpper();
            objEntidad.usua_vCorreoElectronico = txtCorreoMant.Text.Trim().ToUpper();
            objEntidad.usua_vAlias = txtCuentaRedMant.Text.ToUpper();
            objEntidad.usua_sEmpresaId = Convert.ToInt16(ddlEntidadMant.SelectedValue);

            DateTime datFechaCadud = Comun.FormatearFecha(dtpFecCaducidad.Text);
            objEntidad.usua_dFechaCaducidad = datFechaCadud;

            if (chkActivoMant.Checked)
            {
                objEntidad.usua_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                objEntidad.usua_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            if (chkActivoBloqueo.Checked)
            {
                objEntidad.usua_bBloqueoActiva = true;
            }
            else
            {
                objEntidad.usua_bBloqueoActiva = false;
            }

            if (chkNotificaRemesa.Checked)
            {
                objEntidad.usua_bNotificaRemesa = true;
            }
            else
            {
                objEntidad.usua_bNotificaRemesa = false;
            }

            objEntidad.usua_vDireccionIP = txtDireccionIP.Text;

            objEntidad.usua_vDireccion = txtDireccion.Text.Trim().ToUpper().Replace("'","''");
            objEntidad.usua_vTelefono = txtTelefono.Text.Trim();
            objEntidad.usua_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.usua_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            objEntidad.usua_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.usua_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();           
            objEntidad.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            return objEntidad;          
        }

        private SGAC.BE.MRE.SE_USUARIOROL ObtenerEntidadDetalleMantenimiento()
        {
            SGAC.BE.MRE.SE_USUARIOROL objEntidad = new SGAC.BE.MRE.SE_USUARIOROL();

            if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                objEntidad.usro_sUsuarioRolId = Convert.ToInt16(ObtenerFilaSeleccionada()["usro_sUsuarioRolId"]);
                objEntidad.usro_sUsuarioId = Convert.ToInt16(ObtenerFilaSeleccionada()["usua_sUsuarioId"]);
            }

            if (CtrlOficinaConsularConfig.SelectedIndex > -1)
            {
                objEntidad.usro_sOficinaConsularId = Convert.ToInt16(CtrlOficinaConsularConfig.SelectedValue);
            }

            if (ddlRolConfiguracion.SelectedIndex > 0)
            {
                objEntidad.usro_sRolConfiguracionId = Convert.ToInt16(ddlRolConfiguracion.SelectedValue);
            }

            objEntidad.usro_sAcceso = Convert.ToInt16(ddlAcceso.SelectedValue);
            objEntidad.usro_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.usro_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            objEntidad.usro_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objEntidad.usro_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            return objEntidad;
        }

        private void GrabarHandler()
        {
            string strScript = string.Empty;

            UsuarioMantenimientoBL UsBL = new UsuarioMantenimientoBL();
            bool Error = true;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];            

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:                   
                    UsBL.Insert(ObtenerEntidadMantenimiento(), ObtenerEntidadDetalleMantenimiento(), ref Error);

                    break;

                case Enumerador.enmAccion.MODIFICAR:
                    UsBL.Update(ObtenerEntidadMantenimiento(), ObtenerEntidadDetalleMantenimiento(), ref Error);

                    break;

                case Enumerador.enmAccion.ELIMINAR:
                    UsBL.Delete(ObtenerEntidadMantenimiento(), ref Error);

                    break;
            }
          
            if (!Error)
            {
                /* Validación del cambio de oficina consular de un usuario */
                if (enmAccion == Enumerador.enmAccion.MODIFICAR)
                {
                    string StrSessionUsuario = Convert.ToString(Session[Constantes.CONST_SESION_USUARIO]);
                    if (txtCuentaRedMant.Text.Trim() == StrSessionUsuario)
                    {
                        if (Convert.ToInt32(CtrlOficinaConsularConfig.SelectedValue) != (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])))
                        {
                            Label mpLabel = (Label)Master.FindControl("lblUserWelcome");
                            mpLabel.Visible = false;
                            Session.RemoveAll();
                            Response.Redirect("~/Cuenta/FrmLogin.aspx");
                        }
                    }
                }

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
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error de Sistema");
            }          

            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            Comun.EjecutarScript(Page, strScript);
        }
        #endregion
    }
}