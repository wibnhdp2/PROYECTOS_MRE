using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;
using SGAC.Contabilidad.CuentaCorriente.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class CuentaBancaria : MyBasePage
    {
        private string strNombreEntidad = "CUENTA CORRIENTE";
        private string strVariableAccion = "CuentaCorriente_Accion";
        private string strVariableDt = "CuentaCorriente_Tabla";
        private string strVariableIndice = "CuentaCorriente_Indice";
         
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

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            ddlOficinaConsularConsulta.AutoPostBack = true;
            ddlOficinaConsularConsulta.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            ddlOficinaConsularMant.AutoPostBack = true;
            ddlOficinaConsularMant.ddlOficinaConsular.SelectedIndexChanged +=new EventHandler(ddlOficinaConsularMant_SelectedIndexChanged);
 
            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvCuentaBancaria, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();
                Session["sCCTipoMoneda"] = null;
                Session["sCuentaCorrienteId"] = null;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvCuentaBancaria };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }        

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cargar Bancos
            CargarCuentasPorBanco();

            // Cargar Oficina Consular
            ddlOficinaConsularMant.SelectedValue = ddlOficinaConsularConsulta.SelectedValue;
            updMantenimiento.Update();

            gdvCuentaBancaria.DataSource = new DataTable();
            gdvCuentaBancaria.DataBind();
        }

        void ddlOficinaConsularMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            EstablecerMonedaPorOficinaSeleccionada();
            updMantenimiento.Update();
        }

        protected void gdvCuentaBancaria_RowCommand(object sender, GridViewCommandEventArgs e)
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
                txtNroCuenta.ReadOnly = false;                

                PintarSeleccionado();
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }
            Comun.EjecutarScript(Page, strScript);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            CargarGrilla();
            LimpiarDatosMantenimiento();
            updMantenimiento.Update();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            chkActivo.Checked = true;
            LimpiarDatosConsulta();
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
            CuentaMantenimientoBL objBL = new CuentaMantenimientoBL();
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                BE.MRE.CO_CUENTACORRIENTE objCuentaCorriente = ObtenerEntidadMantenimiento();
                objCuentaCorriente = objBL.Insert(objCuentaCorriente);
                if (objCuentaCorriente.cuco_sCuentaCorrienteId == 0)
                {
                    if (objCuentaCorriente.Error)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CUENTA BANCARIA", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CUENTA BANCARIA", objCuentaCorriente.Message);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }
            }
            else if (enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                BE.MRE.CO_CUENTACORRIENTE objCuentaCorriente = ObtenerEntidadMantenimiento();
                objCuentaCorriente = objBL.Update(objCuentaCorriente);

                if (objCuentaCorriente.Error)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CUENTA BANCARIA", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
                else if (objCuentaCorriente.Message != string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CUENTA BANCARIA", objCuentaCorriente.Message);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            else if (enmAccion == Enumerador.enmAccion.ELIMINAR)
            {
                int intResultado = objBL.Delete(ObtenerEntidadMantenimiento());
                if (intResultado < 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "CUENTA BANCARIA", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }               
            }

            // CARGA
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            updMantenimiento.Update();

            LimpiarDatosConsulta();
            CargarGrilla("GRABAR");
            updConsulta.Update();

            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CUENTA BANCARIA", Constantes.CONST_MENSAJE_MANT_EXITO);
            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }
                
        private void CargarDatosIniciales()
        {
            // Inicializar variables de sesión
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            // Inicializar opciones - Acción: INSERTAR
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            // Asignar Valores iniciales
            if (Session != null)
            {
                if (Session[Constantes.CONST_SESION_TIPO_MONEDA_ID] != null)
                {
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]) != 0)
                    {
                        ddlTipoMonedaMant.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID].ToString();
                    }
                }
            }

            ddlTipoCuenta.SelectedValue = ((int)Enumerador.enmTipoCuenta.TRANSACCION).ToString();
            ddlSituacion.SelectedValue = ((int)Enumerador.enmSituacion.OPERATIVA).ToString();

            ddlOficinaConsularConsulta.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            ddlOficinaConsularMant.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

            updMantenimiento.Update();
        }

        private void EstablecerMonedaPorOficinaSeleccionada()
        {
            int intMonedaid = 0;
            ddlTipoMonedaMant.SelectedIndex = 0;
            if (ddlOficinaConsularMant.SelectedIndex > 0)
            {
                int intOficinaConsularId = Convert.ToInt32(ddlOficinaConsularMant.SelectedValue);
                
                //object[] arrParametros = { intOficinaConsularId };
                //Proceso p = new Proceso();
                //intMonedaid = (Int32)p.Invocar(ref arrParametros,"SGAC.BE.MA_MONEDA",Enumerador.enmAccion.LEER);

                SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL objTablaMaestraConsultaBL = new SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL();

                intMonedaid = Convert.ToInt32(objTablaMaestraConsultaBL.ObtenerMonedaId(intOficinaConsularId));

                if (intMonedaid > 0)
                {
                    ddlTipoMonedaMant.SelectedValue = intMonedaid.ToString();
                }
            }
        }

        private void CargarListadosDesplegables()
        {
            ddlOficinaConsularConsulta.Cargar();
            ddlOficinaConsularConsulta.SelectedIndex = -1;

            TransaccionConsultasBL objBL = new TransaccionConsultasBL();
            DataTable dt = objBL.ObtenerBancoCuenta(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
            string strItemAdicional = " - SELECCIONAR - ";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strItemAdicional = " - TODOS - ";
                }
            }
            Util.CargarDropDownList(ddlBanco, dt, "descripcion", "id", true, strItemAdicional);

            // Mantenimiento
            ddlOficinaConsularMant.Cargar();
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaConsularMant.Enabled = false;
            }

            Util.CargarParametroDropDownList(ddlBancoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
            Util.CargarParametroDropDownList(ddlTipoMonedaMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA));
            Util.CargarParametroDropDownList(ddlTipoCuenta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_CUENTA));
            Util.CargarParametroDropDownList(ddlSituacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_SITUACION_CUENTA));
        }

        private void CargarGrilla(object nAccion = null)
        {
            Proceso p = new Proceso();
            DataTable dt = new DataTable();

            int intTotalRegistros = 0, intTotalPaginas = 0;
            int intOficinaConsualrId = 0;
            int intBancoId = 0;

            if (nAccion == null)
                intOficinaConsualrId = Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue);
            else
                intOficinaConsualrId = Convert.ToInt32(ddlOficinaConsularMant.SelectedValue);

            intBancoId = Convert.ToInt32(ddlBanco.SelectedValue);

            string strEstado = "";

            if (chkActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }


            CuentaConsultasBL objBL = new CuentaConsultasBL();
            dt = objBL.Consultar(intOficinaConsualrId, intBancoId,
                ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas, strEstado);

            if (p.IErrorNumero == 0)
            {
                Session[strVariableDt] = dt;
                gdvCuentaBancaria.SelectedIndex = -1;
                gdvCuentaBancaria.DataSource = dt;
                gdvCuentaBancaria.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA + " o no ha registrado una cuenta bancaria");
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

                #region Carga de datos Oficina Consular
                if (nAccion != null)
                {
                    if (ddlOficinaConsularMant.SelectedIndex > 0)
                    {
                        ddlOficinaConsularConsulta.SelectedValue = ddlOficinaConsularMant.SelectedValue;
                        if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                        {
                            ddlOficinaConsularMant.Enabled = false;
                        }
                        CargarCuentasPorBanco();
                    }
                }
                #endregion

                updConsulta.Update();
            }
            else
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
                Comun.EjecutarScript(Page, strScript);
            }
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
                //int intSeleccionado = (int)Session[strVariableIndice];
                //HiddenField transaccion = gdvCuentaBancaria.SelectedRow.Cells[intSeleccionado].FindControl("hTransaccion") as HiddenField;
                if (drSeleccionado.ItemArray[15].ToString().Length == 0)
                {
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                }
                else { ctrlToolBarMantenimiento.btnEliminar.Enabled = true; }
                if (drSeleccionado != null)
                {
                    Session["sCuentaCorrienteId"] = drSeleccionado["cuco_sCuentaCorrienteId"].ToString();
                    ddlOficinaConsularMant.SelectedValue = drSeleccionado["cuco_sOficinaConsularId"].ToString();
                    txtNroCuenta.Text = drSeleccionado["cuco_vNumero"].ToString();
                    ddlBancoMant.SelectedValue = drSeleccionado["cuco_sBancoId"].ToString();
                    ddlTipoMonedaMant.SelectedValue = drSeleccionado["cuco_sMonedaId"].ToString();
                    Session["sCCTipoMoneda"] = drSeleccionado["cuco_sMonedaId"].ToString();
                    ddlTipoCuenta.SelectedValue = drSeleccionado["cuco_sTipoId"].ToString();
                    txtNombreTitularMant.Text = drSeleccionado["cuco_vRepresentante"].ToString();
                    txtNombreSucursal.Text = drSeleccionado["cuco_vSucursal"].ToString();
                    txtObservacionMant.Text = drSeleccionado["cuco_vObservacion"].ToString();
                    ddlSituacion.SelectedValue = drSeleccionado["cuco_sSituacionId"].ToString();

                    if (drSeleccionado["cuco_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
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

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            //ddlOficinaConsularMant.Enabled = bolHabilitar;
            txtNroCuenta.Enabled = bolHabilitar;
            ddlBancoMant.Enabled = bolHabilitar;
            ddlTipoMonedaMant.Enabled = bolHabilitar;
            ddlTipoCuenta.Enabled = bolHabilitar;
            txtNombreTitularMant.Enabled = bolHabilitar;
            txtNombreSucursal.Enabled = bolHabilitar;
            txtObservacionMant.Enabled = bolHabilitar;
            ddlSituacion.Enabled = bolHabilitar;
            chkActivoMant.Enabled = bolHabilitar;
        }

        private void LimpiarDatosConsulta()
        {
            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != null)
                ddlOficinaConsularConsulta.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            else
                ddlOficinaConsularConsulta.SelectedIndex = 0;

            TransaccionConsultasBL objBL = new TransaccionConsultasBL();
            DataTable dt = objBL.ObtenerBancoCuenta(Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue));

            string strItemAdicional = " - SELECCIONAR - ";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strItemAdicional = " - TODOS - ";
                }
            }

            Util.CargarDropDownList(ddlBanco, dt, "descripcion", "id", true, strItemAdicional);

            gdvCuentaBancaria.DataSource = null;
            gdvCuentaBancaria.DataBind();

            ctrlPaginador.InicializarPaginador();            

            objBL = null;
            dt = null;
        }

        private void LimpiarDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            txtNombreSucursal.Text = "";
            txtNombreTitularMant.Text = "";
            txtNroCuenta.Text = "";
            txtObservacionMant.Text = "";
            chkActivoMant.Checked = true;

            ddlOficinaConsularMant.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

            ddlBancoMant.SelectedIndex = 0;
            ddlTipoMonedaMant.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID].ToString();
            ddlTipoCuenta.SelectedIndex = 0;
            ddlSituacion.SelectedIndex = 0;           
        }

        private SGAC.BE.MRE.CO_CUENTACORRIENTE ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                DataTable dt = (DataTable)Session[strVariableDt];
                DataRow drSeleccionado = dt.Rows[gdvCuentaBancaria.SelectedIndex];

                SGAC.BE.MRE.CO_CUENTACORRIENTE obj = new SGAC.BE.MRE.CO_CUENTACORRIENTE();
                obj.cuco_sCuentaCorrienteId = Convert.ToInt16(drSeleccionado["cuco_sCuentaCorrienteId"]);
                obj.cuco_sOficinaConsularId = Convert.ToInt16(drSeleccionado["cuco_sOficinaConsularId"]);
                obj.cuco_sBancoId = Convert.ToInt16(drSeleccionado["cuco_sBancoId"]);
                obj.cuco_vNumero = drSeleccionado["cuco_vNumero"].ToString();
                obj.cuco_sMonedaId = Convert.ToInt16(drSeleccionado["cuco_sMonedaId"]);
                obj.cuco_sTipoId = Convert.ToInt16(drSeleccionado["cuco_sTipoId"]);
                obj.cuco_vRepresentante = drSeleccionado["cuco_vRepresentante"].ToString();
                obj.cuco_vSucursal = drSeleccionado["cuco_vSucursal"].ToString();
                obj.cuco_sSituacionId = Convert.ToInt16(drSeleccionado["cuco_sSituacionId"]);
                obj.cuco_vObservacion = drSeleccionado["cuco_vObservacion"].ToString();

                if (chkActivoMant.Checked)
                {
                    obj.cuco_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    obj.cuco_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }

                return obj;
            }

            return null;
        }

        private SGAC.BE.MRE.CO_CUENTACORRIENTE ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.CO_CUENTACORRIENTE obj = new SGAC.BE.MRE.CO_CUENTACORRIENTE();
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    obj.cuco_sCuentaCorrienteId = Convert.ToInt16(ObtenerFilaSeleccionada()["cuco_sCuentaCorrienteId"]);
                }
                obj.cuco_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularMant.SelectedValue);
                obj.cuco_sBancoId = Convert.ToInt16(ddlBancoMant.SelectedValue);
                obj.cuco_vNumero = txtNroCuenta.Text.ToUpper().Trim();
                obj.cuco_sMonedaId = Convert.ToInt16(ddlTipoMonedaMant.SelectedValue);
                obj.cuco_sTipoId = Convert.ToInt16(ddlTipoCuenta.SelectedValue);
                obj.cuco_vRepresentante = txtNombreTitularMant.Text.Trim().ToUpper();
                obj.cuco_vSucursal = txtNombreSucursal.Text.Trim().ToUpper();
                obj.cuco_sSituacionId = Convert.ToInt16(ddlSituacion.SelectedValue);
                obj.cuco_vObservacion = txtObservacionMant.Text.Trim().ToUpper();
               
                if (chkActivoMant.Checked)
                {
                    obj.cuco_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    obj.cuco_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }
                obj.cuco_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.cuco_vIPCreacion = Util.ObtenerDireccionIP();
                obj.cuco_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.cuco_vIPModificacion = Util.ObtenerDireccionIP();

                return obj;
            }

            return null;
        }

        private void CargarCuentasPorBanco()
        {
            //Proceso p = new Proceso();
            //object[] arrParametros = { Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue) };

            //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_TRANSACCION", "LISTA_BANCO");

            TransaccionConsultasBL objTransaccionConsultaBL = new TransaccionConsultasBL();
            DataTable dt = new DataTable();
            dt = objTransaccionConsultaBL.ObtenerBancoCuenta(Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue));

            string strItemAdicional = " - SELECCIONAR - ";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    strItemAdicional = " - TODOS - ";
            }

            Util.CargarDropDownList(ddlBanco, dt, "descripcion", "id", true, strItemAdicional);
        }   

        public bool ValidaDatos()
        {
            bool bolValido = true;

            if (txtNroCuenta.Text.Trim() == string.Empty)
                bolValido = false;

            if (txtNombreTitularMant.Text.Trim() == string.Empty)
                bolValido = false;

            return bolValido;
        }

        protected void btnSaldoInicial_Click(object sender, EventArgs e)
        {
            

            if (Convert.ToInt16(Session[strVariableAccion]) == Convert.ToInt16(Enumerador.enmAccion.MODIFICAR))
                Comun.EjecutarScript(this, "showModalPopup('../Configuracion/FrmCuentaCorrienteSaldo.aspx','SALDO INICIAL',130, 530, '');");
            else

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,"Saldo Inicial", "Solo puede acceder en modo edición."));
        }
    }
}