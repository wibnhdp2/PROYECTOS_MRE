using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using System.Data;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmFormulario : MyBasePage
    {
        #region CAMPOS 
        private string strNombreEntidad = "FORMULARIO";
        private string strVariableAccion = "Formulario_Accion";
        private string strVariableDt = "Formulario_Tabla";
        private string strVariableIndice = "Formulario_Indice";
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

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);            
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvFormulario, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvFormulario };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        protected void gdvFormulario_RowCommand(object sender, GridViewCommandEventArgs e)
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

                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            CargarGrilla();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            txtDescripcionConsulta.Text = "";

            gdvFormulario.DataSource = null;
            gdvFormulario.DataBind();

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
            updMantenimiento.Update(); // IDM-PENDIENTE (temporal necesario)
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            object[] arrParametros = new object[1];
            FormularioMantenimientoBL BL = new FormularioMantenimientoBL();
            SGAC.BE.MRE.SE_FORMULARIO obj;
            Proceso p = new Proceso();

            int intResultado = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;
                    intResultado = BL.Insert(ref obj);                    
                    break;

                case Enumerador.enmAccion.MODIFICAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;
                    intResultado = BL.Update(obj);                    
                    break;

                case Enumerador.enmAccion.ELIMINAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;
                    intResultado = BL.Delete(obj);                   
                    break;

                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }

            string strScript = string.Empty;
            if (p.IErrorNumero == 0)
            {
                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    LimpiarDatosMantenimiento();
                    HabilitarMantenimiento();

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
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        } 
      
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            LimpiarDatosMantenimiento();            

            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            DataTable dtSistemas = new DataTable();

            dtSistemas = Comun.ObtenerSistemasCargaInicial();

            //Util.CargarParametroDropDownList(ddlAplicacionConsulta, Comun.ObtenerSistemas(Session));
            Util.CargarParametroDropDownList(ddlAplicacionConsulta, dtSistemas);

            //Util.CargarParametroDropDownList(ddlAplicacionMant, Comun.ObtenerSistemas(Session));
            Util.CargarParametroDropDownList(ddlAplicacionMant, dtSistemas);

            //Proceso p = new Proceso();
            //object[] arrParametros = { Convert.ToInt32(ddlAplicacionMant.SelectedValue), (int)Enumerador.enmVisibilidad.INVISIBLE };
            //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_FORMULARIO", "LISTAR");

            FormularioConsultasBL objFormularioConsultaBL = new FormularioConsultasBL();
            DataTable dt = new DataTable();
            dt = objFormularioConsultaBL.ListarPorAplicacion(Convert.ToInt32(ddlAplicacionMant.SelectedValue), (int)Enumerador.enmVisibilidad.INVISIBLE);

            Util.CargarDropDownList(ddlReferenciaMant, dt, "form_vDescripcion", "form_sFormularioId", true);
        }

        private void CargarGrilla()
        {
            if (Page.IsValid)
            {
                Proceso p = new Proceso();
                FormularioConsultasBL BL = new FormularioConsultasBL();
                DataTable dt = new DataTable();

                #region Obtener Filtros
                // Obtenemos los Filtros
                int intTotalRegistros = 0, intTotalPaginas = 0;
                string strEstado = string.Empty;
                if (chkEstado.Checked)
                {
                    strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }               
                #endregion

                dt=BL.ObtenerPorAplicacion(ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO,
                                        ref intTotalRegistros, ref intTotalPaginas,
                                        Convert.ToInt32(ddlAplicacionConsulta.SelectedValue),
                                        txtDescripcionConsulta.Text.Trim().ToUpper(),
                                        strEstado);

                if (p.IErrorNumero == 0)
                {
                    Session.Add(strVariableDt, dt);
                    gdvFormulario.SelectedIndex = -1;
                    gdvFormulario.DataSource = dt;
                    gdvFormulario.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                    }
                    else
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(intTotalRegistros), true, Enumerador.enmTipoMensaje.INFORMATION);
                    }

                    ctrlPaginador.TotalResgistros = Convert.ToInt32(intTotalRegistros);
                    ctrlPaginador.TotalPaginas = Convert.ToInt32(intTotalPaginas);

                    ctrlPaginador.Visible = false;
                    if (ctrlPaginador.TotalPaginas > 1)
                    {
                        ctrlPaginador.Visible = true;
                    }

                    updConsulta.Update();
                }
                else
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
                    Comun.EjecutarScript(Page, strScript);
                }
            } 
        }       

        private DataRow ObtenerFilaSeleccionada()
        {
            int intSeleccionado = (int)Session[strVariableIndice];
            return ((DataTable)Session[strVariableDt]).Rows[intSeleccionado];
        }

        private void PintarSeleccionado()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                if (drSeleccionado != null)
                {
                    ddlAplicacionMant.SelectedValue = drSeleccionado["form_sAplicacionId"].ToString();
                    if (drSeleccionado["form_sReferenciaId"].ToString().Equals("0"))
                    {
                        ddlReferenciaMant.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlReferenciaMant.SelectedValue = drSeleccionado["form_sReferenciaId"].ToString();
                    }

                    txtFormularioNombre.Text = drSeleccionado["form_vNombre"].ToString();
                    txtFormularioRutaMant.Text = drSeleccionado["form_vRuta"].ToString();
                    txtFormularioOrden.Text = drSeleccionado["form_sOrden"].ToString();
                    chkVisible.Checked = Convert.ToBoolean(drSeleccionado["form_bVisible"]);
                    if (drSeleccionado["form_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                        chkHabilita.Checked = true;
                    else
                        chkHabilita.Checked = false;

                    updMantenimiento.Update();
                }
            } 
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlAplicacionMant.Enabled = bolHabilitar;
            ddlReferenciaMant.Enabled = bolHabilitar;
            txtFormularioNombre.Enabled = bolHabilitar;
            txtFormularioRutaMant.Enabled = bolHabilitar;
            txtFormularioOrden.Enabled = bolHabilitar;
            chkVisible.Enabled = bolHabilitar;
            chkHabilita.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            ddlReferenciaMant.SelectedIndex = 0;
            ddlAplicacionMant.SelectedValue = ((int)Enumerador.enmAplicacion.WEB).ToString();

            txtFormularioNombre.Text = "";
            txtFormularioRutaMant.Text = "";
            txtFormularioOrden.Text = "";

            chkVisible.Checked = true;
            chkHabilita.Checked = true;

            updMantenimiento.Update();

        }

        private SGAC.BE.MRE.SE_FORMULARIO ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();

                SGAC.BE.MRE.SE_FORMULARIO objEntidad = new SGAC.BE.MRE.SE_FORMULARIO();
                objEntidad.form_sFormularioId = Convert.ToInt16(drSeleccionado["form_sFormularioId"]);                
                objEntidad.form_sAplicacionId = Convert.ToInt16(drSeleccionado["form_sAplicacionId"]);
                objEntidad.form_vNombre = drSeleccionado["form_vNombre"].ToString();
                objEntidad.form_sReferenciaId = Convert.ToInt16(drSeleccionado["form_sReferenciaId"]);
                objEntidad.form_vRuta = drSeleccionado["form_vRuta"].ToString();
                objEntidad.form_vImagen = drSeleccionado["form_vImagen"].ToString();
                objEntidad.form_sOrden = Convert.ToInt16(drSeleccionado["form_sOrden"]);
                objEntidad.form_bVisible = Convert.ToBoolean(drSeleccionado["form_bVisible"]);
                objEntidad.form_cEstado = drSeleccionado["form_cEstado"].ToString();

                return objEntidad;
            }

            return null;
        }

        private SGAC.BE.MRE.SE_FORMULARIO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SE_FORMULARIO objEntidad = new SGAC.BE.MRE.SE_FORMULARIO();
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objEntidad.form_sFormularioId = Convert.ToInt16(ObtenerFilaSeleccionada()["form_sFormularioId"]);
                }
                objEntidad.form_sAplicacionId = Convert.ToInt16(ddlAplicacionMant.SelectedValue);
                objEntidad.form_vNombre = txtFormularioNombre.Text.Trim();
                if (ddlReferenciaMant.SelectedIndex > 0)
                {
                    objEntidad.form_sReferenciaId = Convert.ToInt16(ddlReferenciaMant.SelectedValue);
                }
                objEntidad.form_vRuta = txtFormularioRutaMant.Text.Trim();

                //objEntidad.form_vImagen = 

                objEntidad.form_sOrden = Convert.ToInt16(txtFormularioOrden.Text);
                if (chkVisible.Checked)
                {
                    objEntidad.form_bVisible = Convert.ToBoolean((int)Enumerador.enmVisibilidad.VISIBLE);
                }
                else
                {
                    objEntidad.form_bVisible = Convert.ToBoolean((int)Enumerador.enmVisibilidad.INVISIBLE);
                }
                if (chkHabilita.Checked)
                {
                    objEntidad.form_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    objEntidad.form_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }
                objEntidad.form_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidad.form_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidad.form_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidad.form_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidad.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                return objEntidad;
            }

            return null;
        }
        #endregion

        protected void ddlAplicacionMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Proceso p = new Proceso();
            //object[] arrParametros = { Convert.ToInt32(ddlAplicacionMant.SelectedValue), (int)Enumerador.enmVisibilidad.INVISIBLE };
            //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_FORMULARIO", "LISTAR");
            //Util.CargarDropDownList(ddlReferenciaMant, dt, "form_vDescripcion", "form_sFormularioId", true);


            FormularioConsultasBL objFormularioConsultaBL = new FormularioConsultasBL();
            DataTable dt = new DataTable();
            dt = objFormularioConsultaBL.ListarPorAplicacion(Convert.ToInt32(ddlAplicacionMant.SelectedValue), (int)Enumerador.enmVisibilidad.INVISIBLE);

            Util.CargarDropDownList(ddlReferenciaMant, dt, "form_vDescripcion", "form_sFormularioId", true);

            updMantenimiento.Update();
        }
    }
}