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
    public partial class UbicacionGeografica : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "UBIGEO";
        private string strVariableAccion = "Ubigeo_Accion";
        private string strVariableDt = "Ubigeo_Tabla";
        private string strVariableIndice = "Ubigeo_Indice";
        #endregion

        #region Eventos
        private void Page_Init(object sender, System.EventArgs e)
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
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvUbicacionGeografica };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void gdvUbicacionGeografica_RowCommand(object sender, GridViewCommandEventArgs e)
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
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();

                txtDptoContNumero2.Enabled = false;
                txtProvPaisNumero2.Enabled = false;
                txtCiudadNumero2.Enabled = false;

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }
            else if (e.CommandName == "Activar")
            {

                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                if (drSeleccionado != null)
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.ACTIVAR;

                    UbigeoMantenimientoBL BL = new UbigeoMantenimientoBL();
                    int intResultado = BL.Eliminar(ObtenerEntidadGrilla("A"), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));


                    if (intResultado == (int)Enumerador.enmResultadoOperacion.OK)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "El registro ha sido activado correctamente.");
                        
                        Session["Grabo"] = "SI";

                        strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                        strScript += Util.HabilitarTab(0);

                        Session[strVariableDt] = new DataTable();
                        BindGrid(txtDptoContDescripcion.Text, txtProvPaisDescripcion.Text, txtCiudadDescripcion.Text);

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
            }

            Comun.EjecutarScript(Page, strScript);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();
            BindGrid(Session["txtDptoContDescripcion"].ToString(), Session["txtProvPaisDescripcion"].ToString(), Session["txtCiudadDescripcion"].ToString());
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();
            BindGrid(txtDptoContDescripcion.Text, txtProvPaisDescripcion.Text, txtCiudadDescripcion.Text);

            Session["txtDptoContDescripcion"] = txtDptoContDescripcion.Text;
            Session["txtProvPaisDescripcion"] = txtProvPaisDescripcion.Text;
            Session["txtCiudadDescripcion"] = txtCiudadDescripcion.Text;
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
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
            txtDptoContNumero2.Enabled = false;
            txtProvPaisNumero2.Enabled = false;
            txtCiudadNumero2.Enabled = false;
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
            Proceso p = new Proceso();
            string strScript = string.Empty;

            UbigeoConsultasBL BL = new UbigeoConsultasBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                int IntRpta = BL.Existe((txtDptoContNumero2.Text.Trim() + txtProvPaisNumero2.Text.Trim() + txtCiudadNumero2.Text.Trim()), 1);

                if (IntRpta == 1)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Ubigeo", "Ya existe el codigo del ubigeo que esta consignando.", false, 190, 250);
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
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        #endregion

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

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            txtDptoContNumero2.Enabled = bolHabilitar;
            txtDptoContDescripcion2.Enabled = bolHabilitar;
            txtProvPaisNumero2.Enabled = bolHabilitar;
            txtProvPaisDescripcion2.Enabled = bolHabilitar;
            txtCiudadNumero2.Enabled = bolHabilitar;
            txtCiudadDescripcion2.Enabled = bolHabilitar;
            txtregISOLetra.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            chkEstadoConsulta.Checked = true;
            txtDptoContDescripcion.Text = "";
            txtProvPaisDescripcion.Text = "";
            txtCiudadDescripcion.Text = "";

            txtDptoContNumero2.Text = "";
            txtDptoContDescripcion2.Text = "";
            txtProvPaisNumero2.Text = "";
            txtProvPaisDescripcion2.Text = "";
            txtCiudadNumero2.Text = "";
            txtCiudadDescripcion2.Text = "";
            txtregISOLetra.Text = "";

            gdvUbicacionGeografica.DataSource = null;
            gdvUbicacionGeografica.DataBind();
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
                    txtDptoContNumero2.Text = drSeleccionado["ubge_cUbi01"].ToString();
                    txtDptoContDescripcion2.Text = drSeleccionado["ubge_vDepartamento"].ToString();
                    txtProvPaisNumero2.Text = drSeleccionado["ubge_cUbi02"].ToString();
                    txtProvPaisDescripcion2.Text = drSeleccionado["ubge_vProvincia"].ToString();
                    txtCiudadNumero2.Text = drSeleccionado["ubge_cUbi03"].ToString();
                    txtCiudadDescripcion2.Text = drSeleccionado["ubge_vDistrito"].ToString();
                    txtregISOLetra.Text = drSeleccionado["ubge_vSiglaPais"].ToString();

                    updMantenimiento.Update();
                }
            }
        }

        private SGAC.BE.MRE.SI_UBICACIONGEOGRAFICA ObtenerEntidadMantenimiento(string estado = "E")
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_UBICACIONGEOGRAFICA objParametro = new BE.MRE.SI_UBICACIONGEOGRAFICA();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objParametro.ubge_cCodigo = Convert.ToString(ObtenerFilaSeleccionada()["ubge_cCodigo"]);
                }
                else
                {
                    objParametro.ubge_cCodigo = txtDptoContNumero2.Text.Trim() + txtProvPaisNumero2.Text.Trim() + txtCiudadNumero2.Text.Trim();
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objParametro.ubge_cUbi01 = txtDptoContNumero2.Text;
                    objParametro.ubge_vDepartamento = txtDptoContDescripcion2.Text.ToUpper();
                    objParametro.ubge_cUbi02 = txtProvPaisNumero2.Text;
                    objParametro.ubge_vProvincia = txtProvPaisDescripcion2.Text.ToUpper();
                    objParametro.ubge_cUbi03 = txtCiudadNumero2.Text;
                    objParametro.ubge_vDistrito = txtCiudadDescripcion2.Text.ToUpper();
                    objParametro.ubge_vSiglaPais = txtregISOLetra.Text.ToUpper();
                }

                objParametro.ubge_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ubge_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.ubge_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ubge_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.ubge_cEstado = estado;
                return objParametro;
            }

            return null;
        }
        private SGAC.BE.MRE.SI_UBICACIONGEOGRAFICA ObtenerEntidadGrilla(string estado = "E")
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_UBICACIONGEOGRAFICA objParametro = new BE.MRE.SI_UBICACIONGEOGRAFICA();

                objParametro.ubge_cCodigo = Convert.ToString(ObtenerFilaSeleccionada()["ubge_cCodigo"]);


                objParametro.ubge_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ubge_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.ubge_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.ubge_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.ubge_cEstado = estado;
                return objParametro;
            }

            return null;
        }

        private void BindGrid(string StrDepartamento, string StrProvincia, string StrDistrito)
        {
            DataTable DtUbigeo = new DataTable();

            Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
            string strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            if (!chkEstadoConsulta.Checked)
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();

            UbigeoConsultasBL BL = new UbigeoConsultasBL();

            DtUbigeo = BL.Consultar(StrDepartamento,
                                    StrProvincia,
                                    StrDistrito,
                                    ctrlPaginador.PaginaActual.ToString(),
                                    intPaginaCantidad,
                                    ref IntTotalCount,
                                    ref IntTotalPages,
                                    strEstado);

            Session[strVariableDt] = DtUbigeo;

            if (DtUbigeo.Rows.Count > 0)
            {
                gdvUbicacionGeografica.SelectedIndex = -1;
                gdvUbicacionGeografica.DataSource = DtUbigeo;
                gdvUbicacionGeografica.DataBind();

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

                gdvUbicacionGeografica.DataSource = null;
                gdvUbicacionGeografica.DataBind();
            }

            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            Int64 intResultado = 0;
            string strScript = string.Empty;

            UbigeoMantenimientoBL BL = new UbigeoMantenimientoBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

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
                case Enumerador.enmAccion.CONSULTAR:
                    break;
                case Enumerador.enmAccion.ACTIVAR:
                    intResultado = BL.Eliminar(ObtenerEntidadMantenimiento("A"), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    break;
            }

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

                Session["Grabo"] = "SI";

                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                Session[strVariableDt] = new DataTable();
                BindGrid(txtDptoContDescripcion.Text, txtProvPaisDescripcion.Text, txtCiudadDescripcion.Text);

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else if (intResultado == (int)Enumerador.enmResultadoOperacion.ERROR)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
            }

            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            Comun.EjecutarScript(Page, strScript);
        }

        #endregion

        protected void gdvUbicacionGeografica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image btnActivar = (System.Web.UI.WebControls.Image)e.Row.FindControl("btnActivar");
                if (!chkEstadoConsulta.Checked)
                {
                    btnActivar.Visible = true;
                }
                else {
                    btnActivar.Visible = false;
                }
            }
        }
    }
}