using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Contabilidad.CuentaCorriente.BL;
using SGAC.Contabilidad.Reportes.BL;
using SGAC.Configuracion.Sistema.BL;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;
using Excel;
using ClosedXML.Excel;
using System.Globalization;
using System.Web.Configuration;
namespace SGAC.WebApp.Contabilidad
{
    public partial class EstadoBancarioConsulta : MyBasePage
    {
        private string formatoDecimal;

        //-------------------------------------------------------
        //Fecha: 11/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Las columnas del Excel seran:
        //        Fecha, Texto, Monto
        //        De acuerdo a la plantilla: 
        //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
        //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //---------------------------------------------------------
        enum ColumnaExcel
        {
            ColumnFecha = 0,
            ColumnTexto = 1,
            ColumnMonto = 2
        }

        #region CAMPOS
        private string strNombreEntidad = "ESTADO BANCARIO";
        private string strVariableAccion = "EstadoBancario_Accion";
        private string strVariableDt = "EstadoBancario_Tabla";
        private string strVariableIndice = "EstadoBancario_Indice";
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
            // PARA QUITAR EN ALGUN MOMENTO
            EstadoOcultar.Visible = false;
            grvConciliacionPendientes.Visible = false;
            //FIN
            formatoDecimal = WebConfigurationManager.AppSettings["FormatoDecimalTCC"];
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);
            ctrlOficinaConsular.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnConfigurationHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonConfigurationClick(ctrlToolBarMantenimiento_btnConfigurationHandler);

            ctrlSubirExcel1.btnUploadButtonHandler += new Accesorios.SharedControls.ctrlSubirExcel.OnUploadButtonClick(ctrlSubirExcel_UploadButton);
            ctrlSubirExcel1.btnUploadButtonInicioHandler += new Accesorios.SharedControls.ctrlSubirExcel.OnUploadButtonInicioClick(ctrlSubirExcel_UploadButtonInicio);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return ValidarRegistro()";
            //this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            //this.dtpFecInicio.EndDate = DateTime.Now;
            //this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);

            this.dtpFechaTransaccion.StartDate = new DateTime(1900, 1, 1);
            this.dtpFechaTransaccion.EndDate = DateTime.Now;

            ctrFechaConciliacion.StartDate = new DateTime(1900, 1, 1);
            ctrFechaConciliacion.EndDate = DateTime.Now;

            CtrFechaConciliacionPop.StartDate = new DateTime(1900, 1, 1);
            CtrFechaConciliacionPop.EndDate = DateTime.Now;

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvEstadoBancario, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            if (!Page.IsPostBack)
            {
                Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                CargarListadosDesplegables();
                CargarDatosIniciales();               
                btnMasivo.Enabled = false;
                btnResumenBancos.Enabled = false;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                Comun.ModoLectura(ref arrButtons);
            }
        }
        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ctrlToolBarConsulta_btnCancelarHandler();
                ddlCuentaCorrienteConsulta.Items.Clear();
                //updConsulta.Update();
                TransaccionConsultasBL objTransaccionBL = new TransaccionConsultasBL();
                DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue));
                //DataTable dt1 = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                if (dt != null)
                {
                    if (dt.Rows.Count > 0) ctrlValidacion.MostrarValidacion("", false);
                    else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);
                }
                else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);

                Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
                //Util.CargarDropDownList(ddlBancoMant, dt1, "descripcion", "id", true);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }

        }
        protected void gdvEstadoBancario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;
            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR; 
            }
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != ObtenerConsuladoFilaSeleccionada())
            {
                CargarListadosDesplegablesVisualizarMant();
            }
            //-----------------------------------------------------------
            //Fecha: 07/01/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Para que no se vuelva a cargar la oficina consular
            //          y asi no obtener el valor cero.
            //------------------------------------------------------------
            //else { CargarListadosDesplegables(); }

            PintarSeleccionado();
            ListarConciliacionesPendientes();
            
            
            if (e.CommandName == "Consultar")
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != ObtenerConsuladoFilaSeleccionada())
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                    ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                    HabilitarMantenimiento(false);
                }
                else {
                    Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                    ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                    HabilitarMantenimiento(false);
                }
                ListarConciliacionesPendientes();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup", "cerrarPopup();", true);
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                Comun.EjecutarScript(Page, strScript);
            }
            else if (e.CommandName == "Editar")
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != ObtenerConsuladoFilaSeleccionada())
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                    ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                    HabilitarMantenimiento(false);
                }
                else
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                    ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                    HabilitarMantenimiento(true);
                    //--------------------------------------------
                    //Fecha: 26/02/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Deshabilitar el Banco y la cuenta
                    //          al editar debido a que no se  
                    //          debe trasladar una transacción
                    //          a otro banco y cuenta que corresponde
                    //          a la tabla: CO_CUENTACORRIENTE
                    //--------------------------------------------
                    ddlBancoMant.Enabled = false;
                    ddlCuentaMant.Enabled = false;
                    //--------------------------------------------
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup", "cerrarPopup();", true);

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                
                Comun.EjecutarScript(Page, strScript);
            }

        }
        protected void gdvEstadoBancario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[0].Text = (Comun.FormatearFecha(e.Row.Cells[0].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
                //if (Convert.ToInt32(e.Row.Cells[7].Text.Trim()) == (int)Enumerador.enmTipoTranIngreso.SALDO_INICIAL)
                //{
                //    e.Row.FindControl("btnEditar").Visible = false;
                //    e.Row.FindControl("btnFind").Visible = false;
                //}
            }
        }
        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        protected void ddlBancoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBancoConsulta.SelectedIndex > 0)
            {
                int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
                int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue);
                object[] arrParametros = { intOficinaConsularId, intBancoId, 1, 1000, 0, 0 };

                //Proceso p = new Proceso();

                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_CUENTACORRIENTE", Enumerador.enmAccion.CONSULTAR);
                CuentaConsultasBL _obj = new CuentaConsultasBL();
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;
                
                DataTable dt = _obj.Consultar(intOficinaConsularId, intBancoId, 1, 1000, ref intTotalRegistros, ref intTotalPaginas);
                if (dt.Rows.Count > 0)
                {
                    Util.CargarDropDownList(ddlCuentaCorrienteConsulta, dt, "cuco_vNumero", "cuco_sCuentaCorrienteId", true);
                }
                else
                {
                    ddlCuentaCorrienteConsulta.Items.Clear();
                    ddlCuentaCorrienteConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));                
                }
            }
            else
            {
                ddlCuentaCorrienteConsulta.Items.Clear();
                ddlCuentaCorrienteConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));                
            }
        }

        protected void ddlCuentaCorrienteConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            //------------------------------------------------------------------
            //Fecha: 16/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Comentar la activación o desactivación del botón buscar,
            //         el periodo y el mensaje.
            //------------------------------------------------------------------

            Button btnBuscar = (Button)ctrlToolBarConsulta.FindControl("btnBuscar");
//            btnBuscar.Enabled = false;
            if (ddlCuentaCorrienteConsulta.SelectedIndex > 0)
            {
                int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
                string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text;
                Int16 CodNumeroCuenta = Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue);

                object[] arrParametros = { Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), 
                                           intBancoId, 
                                           strNumeroCuenta };

                CuentaConsultasBL _obj = new CuentaConsultasBL();

                
                string strPeriodo = "";
                //strPeriodo = ddlAnioBusqueda.SelectedValue + (ddlMesBusqueda.SelectedIndex + 1).ToString().PadLeft(2, '0');

                DataTable dt = _obj.ObtenerPorNroCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), intBancoId, strNumeroCuenta, CodNumeroCuenta, strPeriodo);
                    if (dt.Rows.Count > 0)
                    {
                        txtRepresentanteConsulta.Text = dt.Rows[0]["cuco_vRepresentante"].ToString();
                        ddlMonedaTipoConsulta.SelectedValue = dt.Rows[0]["cuco_sMonedaId"].ToString();

  //                      btnBuscar.Enabled = true;
                        btnMasivo.Enabled = true;
                        btnResumenBancos.Enabled = true;
                    }
                    else
                    {
    //                    btnBuscar.Enabled = false;
                        btnMasivo.Enabled = false;
                        btnResumenBancos.Enabled = false;
                        txtRepresentanteMant.Text = "";
                        ddlMonedaTipoMantenimiento.SelectedIndex = 0;
                        txtSaldo.Text = "0.000";
                        updConsulta.Update();
                       // Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se encuentra Cuenta Corriente con los datos ingresados."));
                    }
            }
            else
            {
                txtRepresentanteConsulta.Text = "";
                ddlMonedaTipoConsulta.SelectedIndex = 0;
            }

            updConsulta.Update();

        }

        protected void ddlBancoMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCuentaPorBanco();
        }

        private void mostrarRepresentante()
        {
            //------------------------------------------------------------
            //Fecha: 16/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Comentar el mensaje y consultar el representante.
            //------------------------------------------------------------

            int intBancoId = Convert.ToInt32(ddlBancoMant.SelectedValue);
            string strNumeroCuenta = ddlCuentaMant.SelectedItem.Text.Trim();
            int oficinaConsularID;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != ObtenerConsuladoFilaSeleccionada())
            {
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
                {
                    oficinaConsularID = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                }
                else { oficinaConsularID = ObtenerConsuladoFilaSeleccionada(); }
                
            }
            else {
                oficinaConsularID = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            }

            object[] arrParametros = { oficinaConsularID, 
                                        intBancoId, 
                                        strNumeroCuenta };

            Int16 CodNumeroCuenta = Convert.ToInt16(ddlCuentaMant.SelectedValue);
            CuentaConsultasBL _obj = new CuentaConsultasBL();

            string strPeriodo = "";
            long iTransaccionId = 0;
            strPeriodo = ddlAnio.SelectedValue + (ddlMes.SelectedIndex + 1).ToString().PadLeft(2, '0');

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            if (enmAccion == Enumerador.enmAccion.MODIFICAR || enmAccion == Enumerador.enmAccion.ELIMINAR)
            {
                iTransaccionId = Convert.ToInt64(ObtenerFilaSeleccionada()["tran_iTransaccionId"]);
            }

            DataTable dt = new DataTable();
            txtSaldo.Text = "0.000";
            hSaldo.Value = "0";

            dt = _obj.ObtenerPorNroCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), intBancoId, strNumeroCuenta, CodNumeroCuenta);
            if (dt.Rows.Count > 0)
            {
                ddlMonedaTipoMantenimiento.SelectedValue = dt.Rows[0]["cuco_sMonedaId"].ToString();
                txtRepresentanteMant.Text = dt.Rows[0]["cuco_vRepresentante"].ToString();

                dt = _obj.ObtenerPorNroCuenta(Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue), intBancoId, strNumeroCuenta, CodNumeroCuenta, strPeriodo, iTransaccionId);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["tran_FSaldo"].ToString().Equals("0"))
                    {
                        txtSaldo.Text = "0.000";
                    }
                    else
                    {
                        txtSaldo.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["tran_FSaldo"]).ToString(formatoDecimal));
                        hSaldo.Value = Convert.ToDouble(dt.Rows[0]["tran_FSaldo"]).ToString();
                    }
                }                
            }
            updConsultaFiltro.Update();
        }

        private void CargarDatosCuetna()
        {
            if (ddlCuentaMant.SelectedIndex > 0)
            {
                mostrarRepresentante();
                ListarConciliacionesPendientes();
            }
        }

        protected void ddlCuentaMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDatosCuetna();
        }

        protected void ddlOperacionTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel.Visible = false;
            if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
            {
                EstadoOcultar.Visible = false;
                if (chkOperacionesPend.Checked)
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_INGRESO_CONCILIACION));
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO));
                }
            }
            else
            {
                if (chkOperacionesPend.Checked)
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_EGRESO_CONCILIACION));
                    Util.CargarParametroDropDownList(ddlEstadoDep, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_ESTADO_DEPOSITO_CONCILIACION));
                    ddlEstadoDep.SelectedValue = ddlEstadoDep.Items.FindByText("PENDIENTE").Value;
                    EstadoOcultar.Visible = true;
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO));
                EstadoOcultar.Visible = false;
                }
            }
            // PARA QUITAR EN ALGUN MOMENTO
            EstadoOcultar.Visible = false;
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Nuevo();
            updMantenimiento.Update();
            if (ddlBancoConsulta.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Seleccione una Cuenta Bancaria" + "');", true);
                ddlBancoConsulta.Focus();
                return;
            }
            //------------------------------------------
            //Fecha: 09/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Solo presentar el reporte resumen
            //------------------------------------------
            //DateTime datFechaInicio = new DateTime();
            //DateTime datFechaFin = new DateTime();

            ctrlPaginador.InicializarPaginador();

            //if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
            //{
            //    gdvEstadoBancario.DataSource = new DataTable();
            //    gdvEstadoBancario.DataBind();

            //    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
            //    return;
            //}
            //if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
            //{
            //    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                
            //    return;
            //}
            //if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
            //{
            //    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                
            //    return;
            //}
            //if (dtpFecInicio.Text != string.Empty)
            //{
            //    if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
            //        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            //}
            //else
            //    datFechaInicio = DateTime.Now;

            //if (dtpFecFin.Text != string.Empty)
            //{
            //    if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
            //        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            //}
            //else
            //    datFechaFin = DateTime.Now;

            //if (datFechaInicio > datFechaFin)
            //{
            //    Session[strVariableDt] = new DataTable();
            //    gdvEstadoBancario.DataSource = new DataTable();
            //    gdvEstadoBancario.DataBind();

            //    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            //}
            //else
            //{
                CargarGrilla();
            //}
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            Nuevo();
            updMantenimiento.Update();
            if (ddlBancoConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == 0)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Seleccione datos de la cuenta.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            #region Filtro Reporte
            int intOficinaConsularId;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            }
            else {
                intOficinaConsularId = ObtenerConsuladoFilaSeleccionada();
            }
            
            int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
            string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text.Trim();

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            datFechaInicio = DateTime.Now;
            datFechaFin = DateTime.Now;
            //if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
            //{
            //    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            //}
            //if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
            //{
            //    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            //}

            object[] arrParametros = {intOficinaConsularId, 
                                        intBancoId,
                                        strNumeroCuenta,
                                        datFechaInicio,
                                        datFechaFin};

            string tBusqueda = "";
            //if (rbFechaTransaccion.Checked)
            //{
            //    tBusqueda = "F";
            //}
            //else
            //{
                tBusqueda = "P";
            //}


            #endregion
            //------------------------------------------
            //Fecha: 09/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Solo presentar el reporte resumen
            //------------------------------------------
            //if (tBusqueda == "F")
            //{
            //    if (datFechaInicio.Month != datFechaFin.Month || datFechaInicio.Year != datFechaFin.Year)
            //    {
            //        ctrlValidacion.MostrarValidacion("Para visualizar el Reporte de Resumen de Bancos, el rango de fechas debe pertenecer al mismo año y mes.");
            //        return;
            //        //string strScript = string.Empty;
            //        //strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Para visualizar el Reporte, el rango de fechas debe pertenecer al mismo año y mes.");
            //        //Comun.EjecutarScript(Page, strScript);
            //        //return;
            //    }
            //}

            ReporteConsultasBL _obj = new ReporteConsultasBL();
            DataTable dt = new DataTable();
            dt = _obj.ObtenerEstadosBancarios(intOficinaConsularId, intBancoId, strNumeroCuenta, datFechaInicio, datFechaFin, Convert.ToInt16(ddlAnioBusqueda.SelectedValue), Convert.ToInt16(ddlMesBusqueda.SelectedIndex + 1), tBusqueda, Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue));
            //Proceso p = new Proceso();
            
            //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_REPORTE", "ESTADO");

            if (dt.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion("No se encontraron registros", true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                Session["Reporte_vCuenta"] = ddlCuentaCorrienteConsulta.SelectedItem.Text;
                Session["SP_PARAMETROS"] = arrParametros;
                Session[Constantes.CONST_SESION_REPORTE_DT] = dt;
                Session["dtDatos"] = dt;
                if (dt.Rows.Count > 0)
                {
                    Session["IdOficinaConsular_contabilidad"] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt.Copy());

                    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReporteContable.ESTADO_BANCARIO;
                    Session[Constantes.CONST_SESION_REPORTE_DT] = ds;

                    string strUrl = "../Contabilidad/FrmReporteContables.aspx?Cs=" + ctrlOficinaConsular.ddlOficinaConsular.SelectedItem.Text;
                    string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                    //EjecutarScript(Page, strScript);
                    strScript = string.Format(strScript);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup" + DateTime.Now.Ticks.ToString(), strScript, true);

                    //Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.ESTADO_BANCARIO,ctrlOficinaConsular.ddlOficinaConsular.SelectedItem.Text);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
            }
        }
        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            Nuevo();
            updMantenimiento.Update();
            // Implementar...
            ddlBancoConsulta.SelectedIndex = -1;
            ddlCuentaCorrienteConsulta.SelectedIndex = -1;
            ddlMonedaTipoConsulta.SelectedIndex = -1;
            txtRepresentanteConsulta.Text = string.Empty;

            gdvEstadoBancario.DataSource = Cabecera_detalle();
            gdvEstadoBancario.DataBind();

            ctrlPaginador.Visible = false;
        }

        private DataTable Cabecera_detalle()
        {
            DataTable dt = new DataTable("detalle");

            dt.Columns.Add("tran_dFechaOperacion", typeof(string));
            dt.Columns.Add("tran_vNumeroOperacion", typeof(string));
            dt.Columns.Add("tran_vTipo", typeof(string));
            dt.Columns.Add("tran_fIngreso", typeof(string));
            dt.Columns.Add("tran_fSalida", typeof(string));
            dt.Columns.Add("tran_FSaldo", typeof(string));

            return dt;
        }

        private void Nuevo()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }
        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            //CargarListadosDesplegables();
            Nuevo();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup", "cerrarPopup();", true);
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
        }

        private bool validarSaldoPositivo()
        {
            if (ddlOperacionTipo.SelectedItem.Text == "EGRESOS")
            {
                double montoFinal = 0;
                if (Convert.ToString(Session[strVariableAccion]) == Enumerador.enmAccion.MODIFICAR.ToString())
                {
                    if (Convert.ToString(ddlOperacionTipo.SelectedValue) != hOperacionTipo.Value)
                    {
                        // Se ha modificado el tipo de operación, era ingreso y se cambio a egreso
                        montoFinal = Convert.ToDouble(hSaldo.Value) - Convert.ToDouble(txtMontoMant.Text);
                    }
                    else
                    {
                        montoFinal = Convert.ToDouble(hSaldo.Value) + Convert.ToDouble(hMontoMant.Value) - Convert.ToDouble(txtMontoMant.Text);
                    }

                }
                else if (Convert.ToString(Session[strVariableAccion]) == Enumerador.enmAccion.ELIMINAR.ToString())
                {
                    return true;
                }
                else
                {
                    montoFinal = Convert.ToDouble(txtSaldo.Text) - Convert.ToDouble(txtMontoMant.Text);
                }


                if (Convert.ToDouble(txtSaldo.Text) == 0)
                {
                    string strScriptMsg = string.Empty;
                    strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se puede registrar un egreso cuando el saldo es igual a 0");
                    Comun.EjecutarScript(Page, strScriptMsg);
                    return false;
                }

                if (montoFinal < 0)
                {
                    if (!chkOperacionesPend.Checked)
                    {
                        string strScriptMsg = string.Empty;
                        strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se puede registrar como egreso un monto mayor al saldo");
                        Comun.EjecutarScript(Page, strScriptMsg);
                        return false;
                    }
                }
            }
            else { 
                if (Convert.ToString(Session[strVariableAccion]) == Enumerador.enmAccion.ELIMINAR.ToString())
                {
                    double montoFinal = 0;
                    montoFinal = Convert.ToDouble(hSaldo.Value) - Convert.ToDouble(hMontoMant.Value);

                    if (montoFinal < 0)
                    {
                        if (!chkOperacionesPend.Checked)
                        {
                            string strScriptMsg = string.Empty;
                            strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se puede eliminar el ingreso porque el saldo seria negativo");
                            Comun.EjecutarScript(Page, strScriptMsg);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
             Proceso p = new Proceso();
            //----------------------------------
            //Fecha: 10/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se consulta el saldo.
            //----------------------------------

            mostrarRepresentante();

            if (validarSaldoPositivo() == false)
            {
                return;
            }

            if (Comun.EsFecha(dtpFechaTransaccion.Text.Trim()) == false)
                {
                    string strScriptMsg = string.Empty;
                    strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "La fecha de transacción no es válida.");
                    Comun.EjecutarScript(Page, strScriptMsg);
                    return;
                }
            


            #region Validar Nro de Cuenta
            string strCuentaId = string.Empty;
            //int intBancoId = Convert.ToInt32(ddlBancoMant.SelectedValue);
            //string strNumeroCuenta = ddlCuentaMant.SelectedItem.Text.Trim();

            //object[] arrParametrosCuenta = { Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 
            //                        intBancoId, 
            //                        strNumeroCuenta };

            //DataTable dtCuenta = (DataTable)p.Invocar(ref arrParametrosCuenta, "SGAC.BE.CO_CUENTACORRIENTE", Enumerador.enmAccion.LEER);
            //if (p.IErrorNumero == 0)
            //{
            //    if (dtCuenta.Rows.Count > 0)
            //    {
            //        strCuentaId = dtCuenta.Rows[0]["cuco_sCuentaCorrienteId"].ToString();
            //    }
            //}
            strCuentaId = ddlCuentaMant.SelectedValue.ToString();
            if (strCuentaId == string.Empty)
            {
                lblValidacion.Visible = true;
                return;
            }
            else
            {
                lblValidacion.Visible = false;
            }
            #endregion

            object[] arrParametros = new object[1];
            SGAC.BE.MRE.CO_TRANSACCION obj;
            long intResultado = 0;

            TransaccionMantenimientoBL BL = new TransaccionMantenimientoBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;

                    intResultado = BL.InsertNew(obj);
                    if (intResultado == -99)
                    {
                        string strScriptMsg = string.Empty;
                        strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se puede registrar, porque ya se ingresó el saldo inicial a esa cuenta.");
                        Comun.EjecutarScript(Page, strScriptMsg);
                        return;
                    }
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    obj = ObtenerEntidadMantenimiento();
                    //arrParametros[0] = obj;
                    //intResultado = (int)p.Invocar(ref arrParametros, "SGAC.BE.CO_TRANSACCION", Enumerador.enmAccion.MODIFICAR);

                    TransaccionMantenimientoBL objTransaccionMantenimientoBL = new TransaccionMantenimientoBL();

                    //----------------------------------------------------------------
                    //Fecha: 10/02/2021
                    //Motivo: Se hace uso de un solo mantenimiento 
                    //intResultado = objTransaccionMantenimientoBL.Update(obj);
                    //Autor: Miguel Márquez Beltrán
                    //----------------------------------------------------------------


                    intResultado = BL.UpdateNew(ObtenerEntidadMantenimiento());
                    if (intResultado == -99)
                    {
                        string strScriptMsg = string.Empty;
                        strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se registro, porque ya se ingresó el saldo inicial a esa cuenta.");
                        Comun.EjecutarScript(Page, strScriptMsg);
                        return;
                    }
                    
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;

                    intResultado = BL.Delete(ObtenerEntidadMantenimiento());

                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }

            string strScript = string.Empty;
            if (p.IErrorNumero == 0)
            {
                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    if (ddlBancoConsulta.SelectedIndex != 0 && ddlCuentaCorrienteConsulta.SelectedIndex != 0)
                    {
                        CargarGrilla();
                    }

                    LimpiarDatosMantenimiento(true);
                    HabilitarMantenimiento();

                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.HabilitarTab(1);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup", "cerrarPopup();", true);
                    Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad + " - " + Session[strVariableAccion],
                                                        Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad + " - " + Session[strVariableAccion],
                                p.vErrorMensaje);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            //CargarListadosDesplegables();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup", "cerrarPopup();", true);
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }

        void ctrlToolBarMantenimiento_btnConfigurationHandler()
        {
            // Implementar...
        }

        protected void ddlBancoConsulta_DataBound(object sender, EventArgs e)
        {
            if (((DropDownList)sender).Items.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myScript", "MensajeBancario(0);", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myScript", "MensajeBancario(1);", true);
            }
        }
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Button btnBuscar = (Button)ctrlToolBarConsulta.FindControl("btnBuscar");
            //------------------------------------------------------------------
            //Fecha: 16/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Comentar la activación o desactivación del botón buscar.
            //------------------------------------------------------------------
            //btnBuscar.Enabled = false;

            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            LimpiarDatosConsulta();
            LimpiarDatosMantenimiento();

            gdvEstadoBancario.DataSource = Cabecera_detalle();
            gdvEstadoBancario.DataBind();

            ctrlOficinaConsular.ddlOficinaConsular.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Enabled = false;
                btnMasivo.Enabled = true;
            }
            else { btnMasivo.Enabled = false; }
            Comun.EjecutarScript(Page, Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL));
            updMantenimiento.Update();
        }
        //------------------------------------------------
        //Fecha: 05/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Cargar listas cuando se quiera visualizar de lima a otro consulado
        //------------------------------------------------
        private void CargarListadosDesplegablesVisualizarMant()
        {
            TransaccionConsultasBL objTransaccionBL = new TransaccionConsultasBL();
            DataTable dt = objTransaccionBL.ObtenerBancoCuenta(ObtenerConsuladoFilaSeleccionada());
           
            if (dt != null)
            {
                if (dt.Rows.Count > 0) ctrlValidacion.MostrarValidacion("", false);
                else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);
            }
            else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);

            Util.CargarDropDownList(ddlBancoMant, dt, "descripcion", "id", true);
        }

        private void CargarListadosDesplegables()
        {
            TransaccionConsultasBL objTransaccionBL = new TransaccionConsultasBL();
            DataTable dt = objTransaccionBL.ObtenerBancoCuenta(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Cargar(true, true, " - SELECCIONAR - ", ""); 
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, false);
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0) ctrlValidacion.MostrarValidacion("", false);
                else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);
            }
            else ctrlValidacion.MostrarValidacion("No hay Cuenta Bancaria registrada.", false);

            DataTable dtMonedas = new DataTable();
            dtMonedas = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
            {
                Util.CargarDropDownList(ddlBancoMant, dt, "descripcion", "id", true);
                Util.CargarDropDownList(ddlBancoMantPopup, dt, "descripcion", "id", true);
                Util.CargarParametroDropDownList(ddlMonedaTipoMantenimiento, dtMonedas, true, " ----- ");
                Util.CargarDropDownList(ddlBancoConsulta, dt, "descripcion", "id", true);
                Util.CargarParametroDropDownList(ddlMonedaTipoConsulta, dtMonedas, true, " ----- ");
            }
            else {
                Util.CargarDropDownList(ddlBancoMant, dt, "descripcion", "id", true);
                Util.CargarDropDownList(ddlBancoMantPopup, dt, "descripcion", "id", true);
                Util.CargarParametroDropDownList(ddlMonedaTipoMantenimiento, dtMonedas, true, " ----- ");
            }
            DataTable dtTipoIngreso = new DataTable();
            dtTipoIngreso = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO);

            DataTable dtTipoAbono = new DataTable();
            dtTipoAbono = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_ABONO);

            Util.CargarParametroDropDownList(ddlTransaccionTipo, dtTipoIngreso);
            //Util.CargarParametroDropDownList(ddlTransaccionTipoPopup, dtTipoIngreso);

        //ListItem itemSALDOINICIAL = ddlTransaccionTipoPopup.Items.FindByText("SALDO INICIAL");
        //    if (itemSALDOINICIAL != null)
        //    {
        //        ddlTransaccionTipoPopup.Items.Remove(itemSALDOINICIAL);
        //    }
        //    ListItem itemToTDD = ddlTransaccionTipoPopup.Items.FindByText("TRANSFERENCIA DE DEPENDIENTE");
        //    if (itemToTDD != null)
        //    {
        //        ddlTransaccionTipoPopup.Items.Remove(itemToTDD);
        //    }

            DataView dv = dtTipoAbono.DefaultView;
            DataTable dtTipoAbonoOrdenado = dv.ToTable();
            dtTipoAbonoOrdenado.DefaultView.Sort = "PARA_SPARAMETROID ASC";

            Util.CargarParametroDropDownList(ddlOperacionTipo, dtTipoAbonoOrdenado);
            //Util.CargarParametroDropDownList(ddlOperacionTipoPopup, dtTipoAbonoOrdenado);
            DataTable dtMes = new DataTable();
            dtMes = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES);
            
            Util.CargarDropDownList(ddlMes, dtMes, "valor", "id");
            Util.CargarComboAnios(ddlAnio, 2015, DateTime.Now.Year);

            Util.CargarDropDownList(ddlMesPopup, dtMes, "valor", "id");
            Util.CargarComboAnios(ddlAnioPopup, 2015, DateTime.Now.Year);

            Util.CargarDropDownList(ddlMesTran, dtMes, "valor", "id");
            Util.CargarComboAnios(ddlAnioTran, 2015, DateTime.Now.Year);

            if (Convert.ToInt32(Session[strVariableAccion]) != (int)Enumerador.enmAccion.MODIFICAR)
            {
                Util.CargarDropDownList(ddlMesBusqueda, dtMes, "valor", "id");
                Util.CargarComboAnios(ddlAnioBusqueda, 2015, DateTime.Now.Year);
            }
            

            ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
            ddlAnioPopup.SelectedValue = DateTime.Now.Year.ToString();
            ddlAnioTran.SelectedValue = DateTime.Now.Year.ToString();
            
            if (Convert.ToInt32(Session[strVariableAccion]) != (int)Enumerador.enmAccion.MODIFICAR)
            {
                if (Convert.ToInt32(Session[strVariableAccion]) != (int)Enumerador.enmAccion.CONSULTAR)
                {
                    ddlAnioBusqueda.SelectedValue = DateTime.Now.Year.ToString();
                    ddlCuentaCorrienteConsulta.Items.Clear();
                    ddlCuentaCorrienteConsulta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));  
                }
            }
                          

        }

        private void CargarGrilla()
        {
            ctrlToolBarConsulta.btnImprimir.Enabled = false;
            #region Reportes Estado Bancario
            //btnConciliacionBancaria.Enabled = false;
            btnResumenBancos.Enabled = false;
            #endregion
            if (Page.IsValid)
            {
                #region Validación de Cuenta
                if (ddlBancoConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == 0)
                {
                    //Limpiar Grilla
                    gdvEstadoBancario.DataSource = null;
                    gdvEstadoBancario.DataBind();
                    //Limpiar Consulta Cuenta
                    txtRepresentanteConsulta.Text = "";
                    ddlMonedaTipoConsulta.SelectedIndex = 0;
                    return;
                }
                #endregion

//                Proceso p = new Proceso();
                DataTable dtEstadoBancario = new DataTable();
                int intTotalRegistros = 0, intTotalPaginas = 0;
                string tBusqueda = "";
                object[] arrParametros = ObtenerFiltro();
  //              dtEstadoBancario = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_TRANSACCION", Enumerador.enmAccion.CONSULTAR);

                TransaccionConsultasBL _obj = new TransaccionConsultasBL();
                //if (rbFechaTransaccion.Checked)
                //{
                //    tBusqueda = "F";
                //}
                //else{
                    tBusqueda = "P";
                //}


                dtEstadoBancario = _obj.Consultar(Convert.ToInt16(arrParametros[0]), Convert.ToInt16(arrParametros[1]), arrParametros[2].ToString(), Convert.ToDateTime(arrParametros[3]), Convert.ToDateTime(arrParametros[4]),
                    Convert.ToInt16(ddlAnioBusqueda.SelectedValue),Convert.ToInt16(ddlMesBusqueda.SelectedIndex + 1),tBusqueda,txtNroOperacionBusq.Text,Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue),
                    ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas);

                //if (p.IErrorNumero == 0)
                //{
                    Session.Add(strVariableDt, dtEstadoBancario);
                    gdvEstadoBancario.SelectedIndex = -1;
                    gdvEstadoBancario.DataSource = dtEstadoBancario;
                    gdvEstadoBancario.DataBind();

                    if (dtEstadoBancario.Rows.Count == 0)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                        gdvEstadoBancario.Visible = false;
                        ctrlToolBarConsulta.btnImprimir.Enabled = false;
                    }
                    else
                    {
                        ctrlToolBarConsulta.btnImprimir.Enabled = true;
                        gdvEstadoBancario.Visible = true;
                        btnResumenBancos.Enabled = true;
                        //if (rbFechaTransaccion.Checked)
                        //{
                        //    //ctrlToolBarConsulta.btnImprimir.Enabled = true;

                        //    #region Reportes Estado Bancario
                        //    //btnConciliacionBancaria.Enabled = true;
                        //    //btnResumenBancos.Enabled = true;
                        //    #endregion
                        //}
                        //else {
                        //    //ctrlToolBarConsulta.btnImprimir.Enabled = false;

                        //    #region Reportes Estado Bancario
                        //    //btnConciliacionBancaria.Enabled = true;
                        //    //btnResumenBancos.Enabled = false;
                        //    #endregion
                        //}



                        ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(intTotalRegistros), true, Enumerador.enmTipoMensaje.INFORMATION);
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
        }

        private object[] ObtenerFiltro()
        {
            int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue) ;
            int intBancoId = Convert.ToInt32(ddlBancoConsulta.SelectedValue);
            string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text.Trim();

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();
            //------------------------------------------
            //Fecha: 09/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Solo presentar el reporte resumen
            //------------------------------------------
            datFechaInicio = DateTime.Now;
            datFechaFin = DateTime.Now;
            //if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
            //{
            //    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            //}
            //if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
            //{
            //    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            //}


            int intTotalRegistros = 0, intTotalPaginas = 0;

            object[] arrParametros = {intOficinaConsularId, 
                                        intBancoId,
                                        strNumeroCuenta,
                                        datFechaInicio,
                                        datFechaFin,
                                        ctrlPaginador.PaginaActual,
                                        Constantes.CONST_CANT_REGISTRO,
                                        intTotalRegistros,
                                        intTotalPaginas};
            return arrParametros;
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            int intSeleccionado = (int)Session[strVariableIndice];
            return ((DataTable)Session[strVariableDt]).Rows[intSeleccionado];
        }
        private int ObtenerConsuladoFilaSeleccionada()
        {
            
            int intOficinaConsular = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion != Enumerador.enmAccion.ELIMINAR)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                DataTable _dt = new DataTable();
                //int intOficinaConsular = 0;
                _dt = (DataTable)Session[strVariableDt];
                if (_dt.Rows.Count == 0)
                {
                    if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
                    {
                        intOficinaConsular = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    }
                }
                else
                {
                    if (intSeleccionado == -1)
                    {
                        intOficinaConsular = Convert.ToInt32(_dt.Rows[0]["tran_sOficinaConsularId"]);
                    }
                    else
                    {
                        intOficinaConsular = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
                    }
                }
                
            }
            else {
                intOficinaConsular = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            }
            return Convert.ToInt32(intOficinaConsular);   
        }

        private void PintarSeleccionado()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();

                if (drSeleccionado != null)
                {
                    hTransaccionEditar.Value = drSeleccionado["tran_iTransaccionId"].ToString();
                    ddlBancoMant.SelectedValue = drSeleccionado["cuco_sBancoId"].ToString();

                    // Cargar Cuenta sún Banco Seleccionado
                    CargarCuentaPorBanco();
                    ddlCuentaMant.SelectedValue = drSeleccionado["tran_sCuentaCorrienteId"].ToString();
                    //---
                    mostrarRepresentante();


                    if (drSeleccionado["tran_cPeriodo"].ToString() != string.Empty)
                    {
                        ddlAnio.SelectedValue = drSeleccionado["tran_cPeriodo"].ToString().Substring(0, 4);
                        ddlMes.SelectedIndex = Convert.ToInt32(drSeleccionado["tran_cPeriodo"].ToString().Substring(4, 2)) - 1;
                    }

                    ddlMonedaTipoMantenimiento.SelectedValue = drSeleccionado["cuco_sMonedaId"].ToString();
                    dtpFechaTransaccion.Text = Comun.FormatearFecha(drSeleccionado["tran_dFechaOperacion"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                    ddlOperacionTipo.SelectedValue = drSeleccionado["tran_sMovimientoTipoId"].ToString();

                    if (drSeleccionado["tran_cPeriodoAnterior"].ToString().Trim() == string.Empty)
                    { 
                        chkOtroMes.Checked = false;
                    }
                    else { 
                        
                        chkOtroMes.Checked = true;
                        ddlAnioTran.Visible = true;
                        ddlMesTran.Visible = true;
                        ddlAnioTran.SelectedValue = drSeleccionado["tran_cPeriodoAnterior"].ToString().Substring(0, 4);
                        ddlMesTran.SelectedIndex = Convert.ToInt32(drSeleccionado["tran_cPeriodoAnterior"].ToString().Substring(4, 2)) - 1;
                    }

                    chkOtroMesCheckedChanged();

                    //if (drSeleccionado["esConciliacion"].ToString() == string.Empty)
                    //{ chkOperacionesPend.Checked = false; }
                    //else { 
                    //    chkOperacionesPend.Checked = true;
                    //}
                    
                    chkOperacionesPend.Checked = Convert.ToBoolean(drSeleccionado["esConciliacion"]);
                    if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                    {
                        EstadoOcultar.Visible = false;
                        if (chkOperacionesPend.Checked)
                        {
                            Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_INGRESO_CONCILIACION));
                        }
                        else {
                            Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO));
                        }
                        hOperacionTipo.Value = Convert.ToInt32(ddlOperacionTipo.SelectedValue).ToString();
                    }
                    else
                    {
                        if (chkOperacionesPend.Checked)
                        {
                            Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_EGRESO_CONCILIACION));
                            Util.CargarParametroDropDownList(ddlEstadoDep, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_ESTADO_DEPOSITO_CONCILIACION));
                            ddlEstadoDep.SelectedValue = ddlEstadoDep.Items.FindByText("PENDIENTE").Value;
                            EstadoOcultar.Visible = true;
                            if (ddlEstadoDep.SelectedItem.Text == Constantes.CONST_PARAMETRO_ESTADO_CONCILIACION_CONCILIADO)
                            {
                                OcultarFechaConciliacion.Visible = true;
                                ctrFechaConciliacion.Text = Comun.FormatearFecha(drSeleccionado["tran_dFechaConciliacion"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            }
                            else { OcultarFechaConciliacion.Visible = false; }
                        }
                        else{
                            Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO));
                            EstadoOcultar.Visible = false;
                        }
                        hOperacionTipo.Value = Convert.ToInt32(ddlOperacionTipo.SelectedValue).ToString();
                    }
                    if (drSeleccionado["tran_sEstadoDepositoConciliacion"].ToString() != "0")
                    { ddlEstadoDep.SelectedValue = drSeleccionado["tran_sEstadoDepositoConciliacion"].ToString(); }
                    ddlTransaccionTipo.SelectedValue = drSeleccionado["tran_sTipoId"].ToString();
                    

                    txtOperacionNro.Text = drSeleccionado["tran_vNumeroOperacion"].ToString();

                    txtSaldo.Text = Convert.ToDouble(drSeleccionado["tran_FSaldo"]).ToString(formatoDecimal);
                    txtObservacion.Text = drSeleccionado["cuco_vObservacion"].ToString();
                    txtSaldoTotal.Text = Convert.ToDouble(drSeleccionado["saldoTotal"]).ToString(formatoDecimal);
                    txtSaldoTotal.Text = String.Format("  {0:F2}", Convert.ToDouble(txtSaldoTotal.Text));

                    //------------------------------------------------
                    //Fecha: 05/01/2017
                    //Autor: Jonatan Silva Cachay
                    //Objetivo: Visualizar nueva lista cuando es transferencia de Dependiente
                    //------------------------------------------------
                    if (Convert.ToInt32(ddlTransaccionTipo.SelectedValue) == (int)Enumerador.enmTipoTranIngreso.TRANSFERENCIA_DEPENDIENTE) // TRANSFERENCIA DE DEPENDIENTE 
                    {
                        CargarListaDependientes(); 
                    }
                    else
                    {
                        panel.Visible = false;
                    }
                    txtMontoMant.Text = Convert.ToDouble(drSeleccionado["tran_FMonto"]).ToString(formatoDecimal);
                    hMontoMant.Value = drSeleccionado["tran_FMonto"].ToString();
                    txtFondoMontoRREE.Text = drSeleccionado["tran_FMontoRREE"].ToString();
                    txtFondoMontoElec.Text = drSeleccionado["tran_FMontoElectoral"].ToString();
                    txtMontoLima.Text = drSeleccionado["tran_FMontoPagadoLima"].ToString();
                    txtMontoMilitar.Text = drSeleccionado["tran_FMontoMilitar"].ToString();
                    txtMontoRetencion.Text = drSeleccionado["tran_FRetencion"].ToString();
                    ddlOfcDependiente.SelectedValue = drSeleccionado["tran_sOficinaDepenConsularId"].ToString();

                    updMantenimiento.Update();
                }
            }
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlBancoMant.Enabled = bolHabilitar;
            ddlCuentaMant.Enabled = bolHabilitar;
            txtMontoMant.Enabled = bolHabilitar;
            ddlMes.Enabled = bolHabilitar;
            ddlAnio.Enabled = bolHabilitar;
            //ddlMesTran.Enabled = bolHabilitar;
            //ddlAnioTran.Enabled = bolHabilitar;
            ddlTransaccionTipo.Enabled = bolHabilitar;
            txtOperacionNro.Enabled = bolHabilitar;
            ddlOperacionTipo.Enabled = bolHabilitar;
            if (ddlOfcDependiente.SelectedIndex > 0)
            { txtMontoMant.Enabled = false; }
            else { txtMontoMant.Enabled = bolHabilitar; }
            txtObservacion.Enabled = bolHabilitar;
            this.ddlOfcDependiente.Enabled = bolHabilitar;
            txtFondoMontoElec.Enabled = bolHabilitar;
            txtFondoMontoRREE.Enabled = bolHabilitar;
            txtMontoLima.Enabled = bolHabilitar;
            txtMontoMilitar.Enabled = bolHabilitar;
            txtMontoRetencion.Enabled = bolHabilitar;
            txtSaldoTotal.Enabled = false;
            grvConciliacionPendientes.Enabled = bolHabilitar;
        }

        private void LimpiarDatosConsulta()
        {
            ctrlToolBarConsulta.btnImprimir.Enabled = false;
            ddlCuentaCorrienteConsulta.SelectedIndex = 0;
            ddlBancoConsulta.SelectedIndex = 0;
            txtRepresentanteConsulta.Text = "";
            ddlMonedaTipoConsulta.SelectedIndex = 0;
            //------------------------------------------------------------------
            //Fecha: 16/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Inicializar el mes de consulta.
            //------------------------------------------------------------------
            ddlMesBusqueda.SelectedIndex = DateTime.Now.Month - 1;
            //------------------------------------------------------------------
        }

        private void LimpiarDatosMantenimiento(bool cabezara = false)
        {
            if (cabezara == false)
            {
                ddlBancoMant.SelectedIndex = 0;
                ddlCuentaMant.Items.Clear();
                ddlCuentaMant.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                txtRepresentanteMant.Text = "";
                ddlMonedaTipoMantenimiento.SelectedIndex = -1;
                ddlMes.SelectedIndex = DateTime.Now.Month - 1;
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
            }
            chkOperacionesPend.Checked = false;
            EstadoOcultar.Visible = false;
            if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
            {
                Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO));
            }
            else
            {
                Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO));
            }

            //if (chkOperacionesPend.Checked)
            //{
            //    if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
            //    { ddlEstadoDep.SelectedValue = ddlEstadoDep.Items.FindByText("PENDIENTE").Value; }
            //}
            
            ddlTransaccionTipo.SelectedIndex = -1;
            
            ddlMesPopup.SelectedIndex = DateTime.Now.Month - 1;
            ddlAnioPopup.SelectedValue = DateTime.Now.Year.ToString();
            ddlMesTran.SelectedIndex = DateTime.Now.Month - 1;
            ddlAnioTran.SelectedValue = DateTime.Now.Year.ToString();
            dtpFechaTransaccion.Text = DateTime.Now.ToString("MMM-dd-yyyy");
            ctrFechaConciliacion.Text = DateTime.Now.ToString("MMM-dd-yyyy");
            CtrFechaConciliacionPop.Text = DateTime.Now.ToString("MMM-dd-yyyy");
            txtOperacionNro.Text = "";
            txtMontoMant.Text = "";
            txtSaldo.Text = "0.00";
            txtObservacion.Text = "";
            txtSaldoTotal.Text = "";

            chkOtroMes.Checked = false;
            ddlMesTran.Visible = false;
            ddlAnioTran.Visible = false;
            OcultarFechaConciliacion.Visible = false;

            txtFondoMontoRREE.Text = "";
            txtFondoMontoElec.Text = "";
            txtMontoLima.Text = "";
            txtMontoMilitar.Text = "";
            txtMontoRetencion.Text = "";
            ddlOfcDependiente.SelectedIndex = -1;
            panel.Visible = false;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            if (cabezara)
            {
                Session[strVariableAccion] = (int)Enumerador.enmAccion.INSERTAR;
                CargarDatosCuetna();
            }
        }

        private SGAC.BE.MRE.CO_TRANSACCION ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                SGAC.BE.MRE.CO_TRANSACCION obj = new SGAC.BE.MRE.CO_TRANSACCION();
                obj.tran_iTransaccionId = Convert.ToInt64(drSeleccionado["tran_iTransaccionId"]);
                obj.tran_sCuentaCorrienteId = Convert.ToInt16(drSeleccionado["tran_sCuentaCorrienteId"]);
                obj.tran_sOficinaConsularId = Convert.ToInt16(drSeleccionado["tran_sOficinaConsularId"]);
                obj.tran_sTipoId = Convert.ToInt16(drSeleccionado["tran_sTipoId"]);
                obj.tran_sMovimientoTipoId = Convert.ToInt16(drSeleccionado["tran_sMovimientoTipoId"]);
                obj.tran_FMonto = Convert.ToDouble(drSeleccionado["tran_FMonto"]);
                obj.tran_FSaldo = Convert.ToDouble(drSeleccionado["tran_FSaldo"]);
                obj.tran_vNumeroOperacion = drSeleccionado["tran_vNumeroOperacion"].ToString();
                obj.tran_dFechaOperacion = Comun.FormatearFecha(drSeleccionado["tran_dFechaOperacion"].ToString());
                obj.tran_cEstado = drSeleccionado["tran_cEstado"].ToString();
                return obj;
            }

            return null;
        }

        private SGAC.BE.MRE.CO_TRANSACCION ObtenerEntidadMantenimiento()
        {
           
            // Cuando se puede o no modificar el monto de un transacción
            if (Session != null)
            {
                
                SGAC.BE.MRE.CO_TRANSACCION obj = new SGAC.BE.MRE.CO_TRANSACCION();

                //------------------------------------------------
                //Fecha: 05/01/2017
                //Autor: Jonatan Silva Cachay
                //Objetivo: Se llena los nuevos campos
                //------------------------------------------------
                double MontoRREE = 0, MontoElec = 0, MontoLima = 0, MontoMilitar = 0,MontoRetencion = 0;
                if (ddlOfcDependiente.SelectedIndex > 0)
                {
                    obj.tran_sOficinaDepenConsularId = Convert.ToInt16(ddlOfcDependiente.SelectedValue);
                }
                else
                {
                    obj.tran_sOficinaDepenConsularId = 0;
                }

                MontoRREE = txtFondoMontoRREE.Text == "" ? 0 : Convert.ToDouble(txtFondoMontoRREE.Text);
                MontoElec = txtFondoMontoElec.Text == "" ? 0 : Convert.ToDouble(txtFondoMontoElec.Text);
                MontoLima = txtMontoLima.Text == "" ? 0 : Convert.ToDouble(txtMontoLima.Text);
                MontoMilitar = txtMontoMilitar.Text == "" ? 0 : Convert.ToDouble(txtMontoMilitar.Text);
                MontoRetencion = txtMontoRetencion.Text == ""? 0 : Convert.ToDouble(txtMontoRetencion.Text);

                obj.tran_FMontoRREE = MontoRREE;
                obj.tran_FMontoElectoral = MontoElec;
                obj.tran_FMontoPagadoLima = MontoLima;
                obj.tran_FMontoMilitar = MontoMilitar;
                obj.tran_FRetencion = MontoRetencion;

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    obj.tran_iTransaccionId = Convert.ToInt64(ObtenerFilaSeleccionada()["tran_iTransaccionId"]);
                }

                obj.tran_sCuentaCorrienteId = Convert.ToInt16(ddlCuentaMant.SelectedValue);
                obj.tran_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                {
                    DataTable dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO);
                    foreach (DataRow row in dt.Rows)
                    {
                         if(row["id"].ToString() == ddlTransaccionTipo.SelectedValue.ToString()){
                             obj.tran_sTipoId = Convert.ToInt16(ddlTransaccionTipo.SelectedValue);
                         }                         
                    }
                }
                else if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
                {
                    DataTable dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["id"].ToString() == ddlTransaccionTipo.SelectedValue.ToString())
                        {
                            obj.tran_sTipoId = Convert.ToInt16(ddlTransaccionTipo.SelectedValue);
                        }
                    }
                }
                else {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Por seguridad vuelva a grabar la información.");
                    Comun.EjecutarScript(Page, strScript);
                    return null;
                }

                obj.tran_sTipoId = Convert.ToInt16(ddlTransaccionTipo.SelectedValue);
                obj.tran_sMovimientoTipoId = Convert.ToInt16(ddlOperacionTipo.SelectedValue);
                obj.tran_sMonedaId = Convert.ToInt16(ddlMonedaTipoMantenimiento.SelectedValue);
                if (Convert.ToInt32(ddlTransaccionTipo.SelectedValue) == (int)Enumerador.enmTipoTranIngreso.TRANSFERENCIA_DEPENDIENTE) // TRANSFERENCIA DE DEPENDIENTE
                {
                    if (ddlOfcDependiente.SelectedIndex > 0)
                    {
                        obj.tran_FMonto = MontoRREE + MontoElec + MontoMilitar - MontoRetencion;
                        txtMontoMant.Text = Convert.ToDouble(obj.tran_FMonto).ToString(formatoDecimal);
                    }
                }
                else {
                    obj.tran_FMonto = Convert.ToDouble(txtMontoMant.Text);
                }

                if (chkOperacionesPend.Checked)
                {
                    obj.tran_FSaldo = Convert.ToDouble(txtSaldo.Text);
                }
                else { obj.tran_FSaldo = ObtenerSaldo(); }
                
                
                obj.tran_vNumeroOperacion = txtOperacionNro.Text.Trim().ToUpper();

                DateTime datFechaInicio = new DateTime();
                if (!DateTime.TryParse(dtpFechaTransaccion.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(dtpFechaTransaccion.Text);
                }

                obj.tran_dFechaOperacion = datFechaInicio;
                obj.tran_cPeriodo = ddlAnio.SelectedValue + (ddlMes.SelectedIndex + 1).ToString().PadLeft(2, '0');

                if (chkOtroMes.Checked)
                {
                    obj.tran_cPeriodoAnterior = ddlAnioTran.SelectedValue + (ddlMesTran.SelectedIndex + 1).ToString().PadLeft(2, '0');
                }
                else { obj.tran_cPeriodoAnterior = null; }

                if (chkOperacionesPend.Checked && Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
                {
                    if (ddlEstadoDep.SelectedItem.Text == Constantes.CONST_PARAMETRO_ESTADO_CONCILIACION_CONCILIADO)
                    { 
                        DateTime datFechaConciliacion = new DateTime();
                        if (!DateTime.TryParse(ctrFechaConciliacion.Text, out datFechaConciliacion))
                        {
                            datFechaConciliacion = Comun.FormatearFecha(ctrFechaConciliacion.Text);
                        }
                        obj.tran_dFechaConciliacion = datFechaConciliacion;
                    }
                    else { obj.tran_dFechaConciliacion = null; }

                    obj.tran_sEstadoDepositoConciliacion = Convert.ToInt16(ddlEstadoDep.SelectedValue);
                }
                else { 
                        obj.tran_sEstadoDepositoConciliacion = 0;
                        obj.tran_dFechaConciliacion = null;
                }

                obj.tran_vObservacion = txtObservacion.Text.Trim().Replace("'", "''").ToUpper();
                obj.tran_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.tran_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                obj.tran_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.tran_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                obj.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                return obj;
            }
            return null;
        }

        private void CargarCuentaPorBanco()
        {
            
            if (ddlBancoMant.SelectedIndex > 0)
            {
                int intBancoId = Convert.ToInt32(ddlBancoMant.SelectedValue);
                int intOficinaConsularId;
                
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != ObtenerConsuladoFilaSeleccionada())
                {
                    if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
                    {
                        intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    }
                    else {
                        intOficinaConsularId = ObtenerConsuladoFilaSeleccionada();
                    }
                }
                else {
                     intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                }
                //int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                object[] arrParametros = { intOficinaConsularId, intBancoId, 1, 1000, 0, 0 };

                //Proceso p = new Proceso();
                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CO_CUENTACORRIENTE", Enumerador.enmAccion.CONSULTAR);
                CuentaConsultasBL _obj = new CuentaConsultasBL();
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dt = _obj.Consultar(intOficinaConsularId, intBancoId, 1, 1000, ref intTotalRegistros, ref intTotalPaginas);
                if (dt.Rows.Count > 0)
                {
                    Util.CargarDropDownList(ddlCuentaMant, dt, "cuco_vNumero", "cuco_sCuentaCorrienteId", true);
                }
                else
                {
                    ddlCuentaMant.Items.Clear();
                    ddlCuentaMant.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));                
                }
            }
            else
            {
                ddlCuentaMant.Items.Clear();
                ddlCuentaMant.Items.Insert(0, new ListItem("- SELECCIONAR -", "0")); 
                ddlMonedaTipoMantenimiento.SelectedIndex = 0;
                txtSaldo.Text = "0.000";
            }
        }

        private Double ObtenerSaldo()
        {
            int intTipoOperacionActual = Convert.ToInt32(ddlOperacionTipo.SelectedValue);
            double dNuevoSaldo = 0.0;
            double dSaldoActual = Convert.ToDouble(txtSaldo.Text);
            double dMontoActual = Convert.ToDouble(txtMontoMant.Text);

            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
            {
                if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                {
                    dNuevoSaldo = dSaldoActual + dMontoActual;
                }
                else
                {
                    dNuevoSaldo = dSaldoActual - dMontoActual;
                }
            }
            else if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.MODIFICAR)
            {
                int intTipoOperacionInicial = Convert.ToInt32(ObtenerFilaSeleccionada()["tran_sMovimientoTipoId"]);
                double dMontoInicial = 0;

                // Obtener Monto Inicial
                dMontoInicial = Convert.ToDouble(ObtenerFilaSeleccionada()["tran_FMonto"]);
                if (intTipoOperacionInicial == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
                {
                    dMontoInicial = (-1) * Convert.ToDouble(ObtenerFilaSeleccionada()["tran_FMonto"]);
                }

                // Obtener Monto Nuevo
                if (intTipoOperacionActual == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
                {
                    dMontoActual = (-1) * dMontoActual;
                }

                dNuevoSaldo = dSaldoActual - dMontoInicial + dMontoActual;
            }
            else if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.ELIMINAR)
            {
                int intTipoOperacionInicial = Convert.ToInt32(ObtenerFilaSeleccionada()["tran_sMovimientoTipoId"]);
                double dMontoInicial = 0;

                dMontoInicial = Convert.ToDouble(ObtenerFilaSeleccionada()["tran_FMonto"]);
                if (intTipoOperacionInicial == (int)Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
                {
                    dMontoInicial = (-1) * Convert.ToDouble(ObtenerFilaSeleccionada()["tran_FMonto"]);
                }

                if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                {
                    dNuevoSaldo = dSaldoActual - dMontoInicial + dMontoActual;
                }
                else
                {
                    dNuevoSaldo = dSaldoActual - dMontoInicial - dMontoActual;
                }
            }

            return dNuevoSaldo;
        }
        #endregion

        //protected void btnConciliacionBancaria_Click(object sender, EventArgs e)
        //{
        //    string strScript = string.Empty;

        //    if (ddlBancoConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == 0)
        //    {
        //        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Seleccione datos de la cuenta.");
        //        Comun.EjecutarScript(Page, strScript);
        //        return;
        //    }


        //    ReporteConsultasBL objBL = new ReporteConsultasBL();

        //    #region Filtros
        //    //Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
        //    Int16 intOficinaConsularId;
        //    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
        //    {
        //        intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
        //    }
        //    else
        //    {
        //        intOficinaConsularId = Convert.ToInt16(ObtenerConsuladoFilaSeleccionada());
        //    }
        //    Int16 intBancoId = Convert.ToInt16(ddlBancoConsulta.SelectedValue);
        //    Int16 intCuentaId = Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue);
        //    string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text.Trim();

        //    DateTime datFechaInicio = new DateTime();
        //    DateTime datFechaFin = new DateTime();

        //    if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
        //    {
        //        datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
        //    }
        //    if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
        //    {
        //        datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
        //    }
        //    #endregion

        //    DataSet ds = objBL.ObtenerReporteConciliacion(intOficinaConsularId, intBancoId, intCuentaId, datFechaInicio, datFechaFin);

        //    double dblSaldo = 0;
        //    if (ds.Tables[0] != null)
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            dblSaldo = Convert.ToDouble(ds.Tables[0].Rows[0]["fSaldo"]);
        //        }
        //    }

        //    #region Vista Previa
        //    Session[Constantes.CONST_SESION_REPORTE_TIPO] = Enumerador.enmReporteContable.CONCILIACION;
        //    Session[Constantes.CONST_SESION_REPORTE_DT] = ds;
        //    Session["dtDatos"] = ds.Tables[0];
        //    Session["dtDatos1"] = ds.Tables[1];
        //    Session["dtDatos2"] = ds.Tables[2];
        //    Session["IdOficinaConsular_contabilidad"] = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
        //    Session["saldo_contabilidad"] = dblSaldo;

        //    Session["nro_cuenta"] = strNumeroCuenta;
        //    Session["conciliacion_ini"] = dtpFecInicio.Text;
        //    Session["conciliacion_fin"] = dtpFecFin.Text;

        //    string strUrl = "../Contabilidad/FrmReporteContables.aspx";
        //    strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
        //    Comun.EjecutarScript(Page, strScript);
        //    #endregion

        //}
        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        protected void btnResumenBancos_Click(object sender, EventArgs e)
        {
            Nuevo();
            updMantenimiento.Update();
            if (ddlBancoConsulta.SelectedIndex == 0 || ddlCuentaCorrienteConsulta.SelectedIndex == 0)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Seleccione datos de la cuenta.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            string strNumeroCuenta = ddlCuentaCorrienteConsulta.SelectedItem.Text.Trim();
            Int16 intCuentaId = Convert.ToInt16(ddlCuentaCorrienteConsulta.SelectedValue);

            DataTable dtBancos = new DataTable();
            DataTable dtConciliacion = new DataTable();
            DataSet ds = new DataSet();

            #region Filtros
            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            datFechaInicio = DateTime.Now;
            datFechaFin = DateTime.Now;
            //------------------------------------------
            //Fecha: 09/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Solo presentar el reporte resumen
            //------------------------------------------

            //if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
            //{
            //    datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            //}
            //if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
            //{
            //    datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            //}
            #endregion
            

            string tBusqueda = "";
            //if (rbFechaTransaccion.Checked)
            //{
            //    tBusqueda = "F";
            //}
            //else
            //{
                tBusqueda = "P";
            //}

            //if (tBusqueda == "F")
            //{
            //    if (datFechaInicio.Month != datFechaFin.Month || datFechaInicio.Year != datFechaFin.Year)
            //    {
            //        ctrlValidacion.MostrarValidacion("Para visualizar el Reporte de Resumen de Bancos, el rango de fechas debe pertenecer al mismo año y mes.");
            //        return;
            //        //string strScript = string.Empty;
            //        //strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Para visualizar el Reporte de Resumen de Bancos, el rango de fechas debe pertenecer al mismo año y mes.");
            //        //strScript = string.Format(strScript);
            //        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup" + DateTime.Now.Ticks.ToString(), strScript, true);
            //        ////Comun.EjecutarScript(Page, strScript);
            //        //return;
            //    }
            //}

            ReporteConsultasBL objConsultaBL = new ReporteConsultasBL();
            ds = objConsultaBL.ObtenerTransaccionResumen(intCuentaId, datFechaInicio, datFechaFin, Convert.ToInt16(ddlAnioBusqueda.SelectedValue), Convert.ToInt16(ddlMesBusqueda.SelectedIndex + 1), tBusqueda);

            string fechaInicio;
            //if (tBusqueda == "F")
            //{
            //    fechaInicio = datFechaInicio.ToShortDateString();
            //}
            //else {
                fechaInicio = "01/" + string.Format("{0:00}", Convert.ToInt16(ddlMesBusqueda.SelectedIndex + 1)) + "/" + Convert.ToInt16(ddlAnioBusqueda.SelectedValue);
            //}
            


            dtBancos = ds.Tables[0];
            dtConciliacion = ds.Tables[1];
            //------------------------------------------------
            //Fecha: 05/01/2017
            //Autor: Jonatan Silva Cachay
            //Objetivo: Imprime en un report
            //------------------------------------------------
            if (dtBancos.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                return;
            }
            string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(ctrlOficinaConsular.ddlOficinaConsular.SelectedValue.ToString()));
            sNombreOficinaConsular = sNombreOficinaConsular.Split('-')[1].ToString().Trim();

            //-----------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 19/11/2019
            // Objetivo: Consulta de fecha y hora unificada.
            //-----------------------------------------------------

            string strFechaActualConsulado = "";
            string strHoraActualConsulado = "";

            Comun.ObtenerFechaHoraActualTexto(HttpContext.Current.Session, ref strFechaActualConsulado, ref strHoraActualConsulado);

            strFechaActualConsulado = Comun.FormatearFecha(strFechaActualConsulado).ToString("MMM-dd-yyyy");
            //----------------------------

            //string strFechaActualConsulado = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
            //string strHoraActualConsulado = Accesorios.Comun.ObtenerHoraActualTexto(HttpContext.Current.Session);

            
            string fechaF; // = "01/" + DateTime.Now.AddMonths(1).ToString("MM") + "/" + DateTime.Now.Year;
            fechaF = Convert.ToDateTime(fechaInicio).AddMonths(1).ToString();
            System.DateTime NuevaFechaFin = Convert.ToDateTime(fechaF);
            NuevaFechaFin = NuevaFechaFin.AddDays(-1);
            
            ReportParameter[] parameters = new ReportParameter[7];
            parameters[0] = new ReportParameter("TituloReporte", "RESUMEN DE BANCOS");
            parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            parameters[4] = new ReportParameter("FechaHaber", " CONCILIACIÓN CUENTA BANCARIA CON CORTE DE " + "01 AL " + NuevaFechaFin.Day.ToString() + " DE " + MonthName(NuevaFechaFin.Month).ToUpper());
            parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
            parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);

            Session["strNombreArchivo"] = "Contabilidad/rsResumenBancos.rdlc";
            Session["DtDatos"] = dtBancos;
            Session["DtDatos1"] = dtConciliacion;
            Session["objParametroReportes"] = parameters;
            Session["DataSet"] = "dsResumenBancos";
            Session["DataSet2"] = "dtResumenBancosConciliacion";
            string strUrl = "../Reportes/frmVisorReporte.aspx?REP=RESUMEN DE BANCOS";
            string Script = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=700,left=100,top=100');";
            //Comun.EjecutarScript(Page, Script);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenPopup" + DateTime.Now.Ticks.ToString(), Script, true);

        }
        //------------------------------------------------
        //Fecha: 05/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Visualizar nueva lista cuando es transferencia de Dependiente
        //------------------------------------------------
        protected void ddlTransaccionTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOfcDependiente.SelectedIndex = -1;
            txtMontoLima.Text = "";
            txtFondoMontoRREE.Text = "";
            txtFondoMontoElec.Text = "";
            txtMontoMilitar.Text = "";
            txtMontoRetencion.Text = "";
            if (Convert.ToInt32(ddlTransaccionTipo.SelectedValue) == (int)Enumerador.enmTipoTranIngreso.TRANSFERENCIA_DEPENDIENTE) // TRANSFERENCIA DE DEPENDIENTE
            {
                CargarListaDependientes();   
            }
            else {
                panel.Visible = false;
                txtMontoMant.Enabled = true;
                //txtMontoMant.Text = dMonto.ToString();
            }
        }
        //------------------------------------------------
        //Fecha: 10/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Cargar Lista de Dependientes
        //------------------------------------------------
        private void CargarListaDependientes()
        {
            DataTable DtOficCH = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = 1000;
            int intOficinaConsularId;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt32(ObtenerConsuladoFilaSeleccionada()))
            {
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmAccion.INSERTAR)
                {
                    intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                }
                else { intOficinaConsularId = Convert.ToInt32(ObtenerConsuladoFilaSeleccionada()); }
                 
            }
            else {
                 intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            }
            
            OficinaConsularConsultasBL BL = new OficinaConsularConsultasBL();

            DtOficCH = BL.ObtenerDependientes(intOficinaConsularId,
                                             "1",
                                             intPaginaCantidad,
                                             ref IntTotalCount,
                                             ref IntTotalPages);

            Util.CargarDropDownList(ddlOfcDependiente, DtOficCH, "ofco_vNombre", "ofco_sOficinaConsularId", true);

            if (DtOficCH.Rows.Count > 0)
            {
                panel.Visible = true;
                txtMontoMant.Enabled = false;
                txtMontoMant.Text = "0";
            }
            else {
                panel.Visible = false;
                txtMontoMant.Enabled = true;
                txtMontoMant.Text = "";
            }
        }

        protected void ddlBancoMantPopup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBancoMantPopup.SelectedIndex > 0)
            {
                int intBancoId = Convert.ToInt32(ddlBancoMantPopup.SelectedValue);
                int intOficinaConsularId;
                
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;
                intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                CuentaConsultasBL _obj = new CuentaConsultasBL();
                DataTable dt = _obj.Consultar(intOficinaConsularId, intBancoId, 1, 1000, ref intTotalRegistros, ref intTotalPaginas);
                                
                if (dt.Rows.Count > 0)
                {
                    Util.CargarDropDownList(ddlCuentaMantPopup, dt, "cuco_vNumero", "cuco_sCuentaCorrienteId", true);
                }
            }
            else
            {
                ddlCuentaMantPopup.SelectedIndex = 0;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
        }

        //protected void ddlOperacionTipoPopup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(ddlOperacionTipoPopup.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
        //    {
        //        Util.CargarParametroDropDownList(ddlTransaccionTipoPopup, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO));
        //    }
        //    else
        //    {
        //        Util.CargarParametroDropDownList(ddlTransaccionTipoPopup, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO));
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
        //}

        private void ctrlSubirExcel_UploadButtonInicio()
        {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
        }
        private void ctrlSubirExcel_UploadButton()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
            try{
                ImportarExcelGrilla(ctrlSubirExcel1.RutaArchivo, ctrlSubirExcel1.Extension);
                if (gdvExcel.Rows.Count > 0)
                {
                    btnGrabarExcel.Enabled = true;
                }
                else { btnGrabarExcel.Enabled = false; }
            }
            catch{
                ctrlSubirExcel1.EliminarExcel(ctrlSubirExcel1.RutaArchivo);
            }
            ctrlSubirExcel1.EliminarExcel(ctrlSubirExcel1.RutaArchivo);
        }

        private bool ImportarExcelGrilla(string FilePath, string Extension)
        {
            if (ddlBancoMantPopup.SelectedIndex <= 0)
            {
                ctrlSubirExcel1.TextoResultado = "Complete los campos obligatorios";
                return false;
            }
            else if (ddlCuentaMantPopup.SelectedIndex <= 0)
            {
                ctrlSubirExcel1.TextoResultado = "Complete los campos obligatorios";
                return false;
            }

            DataSet ds;
            DataTable dt = new DataTable();
            try
            {
                var file = new FileInfo(FilePath);

                using (var stream = new FileStream(FilePath, FileMode.Open))
                {
                    if (Extension != ".xls" && Extension != ".xlsx")
                    {
                        return false;
                    }
                    IExcelDataReader reader = null;
                    if (file.Extension == ".xls")
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);

                    }
                    else if (file.Extension == ".xlsx")
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    if (reader == null)
                        return false;

                    reader.IsFirstRowAsColumnNames = true;
                    ds = reader.AsDataSet();
                    dt = ds.Tables[0];


                    bool respuesta = false;
                    //-------------------------------------------------------
                    //Fecha: 11/05/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Las columnas del Excel seran:
                    //        Fecha, Texto, Monto
                    //        De acuerdo a la plantilla: 
                    //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
                    //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
                    //---------------------------------------------------------

                    if (dt.Columns.Count >= 3)
                    {
                        EliminarFilasVacias(dt);
                        respuesta = ValidarFechaDatatable(dt);
                        if (respuesta == false)
                        {
                            return false;
                        }
                    }
                    else {
                        ctrlValidacionRegistro.MostrarValidacion("El excel no cumple con la cantidad de columnas establecidas!.", true, Enumerador.enmTipoMensaje.WARNING);
                        return false; 
                    }
                    respuesta = ValidarCamposObligatorios(dt);
                    if (respuesta == false)
                    {
                        ctrlValidacionRegistro.MostrarValidacion("El excel no cumple con los campos mínimos establecidos!.", true, Enumerador.enmTipoMensaje.WARNING);
                        return false;
                    }

                    TransaccionConsultasBL _obj = new TransaccionConsultasBL();
                    DataTable _dtResultado = new DataTable();

                    //--------------------------------------------------------------
                    //Fecha: 10/05/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Quitar los parametros de Tipo de Operación y 
                    //          Tipo de Transacción. 
                    //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
                    //--------------------------------------------------------------

                    _dtResultado = _obj.VerificarRegistroMasivo(LlenarXML(dt), Convert.ToInt16(ddlCuentaMantPopup.SelectedValue));

                    lblCantidad.Text = "Cantidad de Registros: " + _dtResultado.Rows.Count.ToString();
                    gdvExcel.DataSource = _dtResultado;
                    gdvExcel.DataBind();
                    controlesCargaMasiva(false);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------------
        //Fecha: 12/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Las columnas del Excel seran:
        //        Fecha, Texto, Monto
        //        De acuerdo a la plantilla: 
        //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
        //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //---------------------------------------------------------

        private string LlenarXML(DataTable dt)
        {
            StringBuilder strXmlDatos = new StringBuilder();
            try
            {
                strXmlDatos.Append("<R>");
                foreach (DataRow row in dt.Rows)
                {
                    strXmlDatos.AppendFormat("<P pFechaTransaccion = '{0}' pTexto = '{1}' pMonto = '{2}'/> ",
                    row[(int)ColumnaExcel.ColumnFecha].ToString().Replace("'", ""),
                    row[(int)ColumnaExcel.ColumnTexto].ToString().Replace("'", ""),
                    row[(int)ColumnaExcel.ColumnMonto].ToString().Replace("'", "")
                    );
                }
                strXmlDatos.Append("</R>");
            }
            catch
            {
                throw;
            }
            return strXmlDatos.ToString();
        }
        protected void gdvExcel_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _error = DataBinder.Eval(e.Row.DataItem, "tran_ERROR").ToString();

                if (_error != String.Empty)
                {
                    e.Row.BackColor = System.Drawing.Color.Pink;
                    e.Row.Enabled = false;
                }
            }

        }

        protected void btnGrabarExcel_Click(object sender, EventArgs e)
        {
            if (!(validarAdicionarTransacciones()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
                return; 
            }
            TransaccionMantenimientoBL _obj = new TransaccionMantenimientoBL();
            string _XML,cPeriodo;
            _XML = LlenarXMLGrilla(gdvExcel);
            cPeriodo = ddlAnioPopup.SelectedValue + (ddlMesPopup.SelectedIndex + 1).ToString().PadLeft(2, '0');

            //--------------------------------------------------------------
            //Fecha: 10/05/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Quitar los parametros de Tipo de Operación y 
            //          Tipo de Transacción. 
            //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
            //--------------------------------------------------------------

            _obj.RegistroMasivoTransacciones(_XML, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(ddlCuentaMantPopup.SelectedValue), cPeriodo, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), Session[Constantes.CONST_SESION_DIRECCION_IP].ToString());

            string strScriptMsg = string.Empty;
            strScriptMsg = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Se realizó el registro correctamente!.");
            Comun.EjecutarScript(Page, strScriptMsg);
            limpiarMasivo();
            controlesCargaMasiva(true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
        }
        //-------------------------------------------------------
        //Fecha: 12/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Las columnas del Excel seran:
        //        Fecha, Texto, Monto
        //        De acuerdo a la plantilla: 
        //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
        //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //---------------------------------------------------------

        private string LlenarXMLGrilla(GridView Grilla)
        {
            StringBuilder strXmlDatos = new StringBuilder();
            try
            {
                CheckBox chkSelItem;
                strXmlDatos.Append("<R>");
                foreach (GridViewRow fila in Grilla.Rows)
                {
                    chkSelItem = (CheckBox)fila.FindControl("chkSelItem");
                    if (chkSelItem.Checked == true)
                    {
                        strXmlDatos.AppendFormat("<P pFechaTransaccion = '{0}' pTipoOperacionId = '{1}' pTipoTransaccionId = '{2}' pMonto = '{3}'/> ",
                           fila.Cells[1].Text,
                           fila.Cells[2].Text,
                           fila.Cells[4].Text,
                           fila.Cells[6].Text
                       );
                    }
                   
                }
                strXmlDatos.Append("</R>");
            }
            catch
            {
                throw;
            }
            return strXmlDatos.ToString();
        }

        private void limpiarMasivo()
        {
            this.gdvExcel.DataSource = null;
            gdvExcel.DataBind();
            lblCantidad.Text = "";
            lblSeleccionados.Text = "";
            btnGrabarExcel.Enabled = false;
        }
        private bool validarAdicionarTransacciones()
        {
            bool bolEsCorrecto = true;

            //-----------------------------------------------------------------------------
            // Valida si existe marcado por lo menos un item antes de guardar los cambios
            //-----------------------------------------------------------------------------
            bool bSelecciono = false;

            for (int i = 0; i < gdvExcel.Rows.Count; i++)
            {
                CheckBox chkSelItem;

                chkSelItem = (CheckBox)gdvExcel.Rows[i].FindControl("chkSelItem");
                if (chkSelItem.Checked == true)
                {
                    bSelecciono = true;
                    break;
                }
            }
            if (bSelecciono == false)
            {
                ctrlValidacionRegistro.MostrarValidacion("No se ha seleccionado ninguna registro", true, Enumerador.enmTipoMensaje.WARNING);
                return false;
            }
            return bolEsCorrecto;
        }

        private void controlesCargaMasiva(bool activar)
        {
            ddlAnioPopup.Enabled = activar;
            ddlMesPopup.Enabled = activar;
            ddlCuentaMantPopup.Enabled = activar;
            ddlBancoMantPopup.Enabled = activar;
            //ddlOperacionTipoPopup.Enabled = activar;
            //ddlTransaccionTipoPopup.Enabled = activar;
        }

        protected void btnLimpiarExcel_Click(object sender, EventArgs e)
        {
            limpiarMasivo();
            controlesCargaMasiva(true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "Popup();", true);
        }
        //-------------------------------------------------------
        //Fecha: 11/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Las columnas del Excel seran:
        //        Fecha, Texto, Monto
        //        De acuerdo a la plantilla: 
        //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
        //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //---------------------------------------------------------

        private void EliminarFilasVacias(DataTable dt)
        {
            for (int x = dt.Rows.Count - 1; x >= 0; x--)
            {
                if (dt.Rows[x][(int)ColumnaExcel.ColumnFecha].ToString() == ""
                    || dt.Rows[x][(int)ColumnaExcel.ColumnTexto].ToString() == ""
                    || dt.Rows[x][(int)ColumnaExcel.ColumnMonto].ToString() == "")
                {
                    dt.Rows[x].Delete();
                }
            }
            dt.AcceptChanges();

        }

        
        private bool ValidarFechaDatatable(DataTable dt)
        {
            //Convertir la fecha en el documento excel: 
            //=TEXTO(A12;"dd/mm/yyyy")
            bool respuesta = false;
            foreach (DataRow row in dt.Rows)
            {
                if (ValidarFecha(row[(int)ColumnaExcel.ColumnFecha].ToString()))
                {
                    respuesta = true;
                }
                else
                {
                    ctrlValidacionRegistro.MostrarValidacion("Verificar la Fila " + (dt.Rows.IndexOf(row) + 1).ToString() +" del excel, el campo de fecha no tiene un formato correcto!.", true, Enumerador.enmTipoMensaje.WARNING);
                    respuesta = false;
                    break;
                }
            }
            return respuesta;
        }


        //-------------------------------------------------------
        //Fecha: 12/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Las columnas del Excel seran:
        //        Fecha, Texto, Monto
        //        De acuerdo a la plantilla: 
        //        PLANTILLA DE CONCILIACION BANCARIA  OFICINAS  CONSULARES
        //        DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //---------------------------------------------------------

        private bool ValidarCamposObligatorios(DataTable dt)
        {
            bool respuesta = false;

            foreach (DataRow row in dt.Rows)
            {
                if (row[(int)ColumnaExcel.ColumnFecha].ToString() == ""
                    || row[(int)ColumnaExcel.ColumnTexto].ToString() == ""
                    || row[(int)ColumnaExcel.ColumnMonto].ToString() == "")
                {
                    respuesta = false;
                    break;
                }
                else
                {
                    respuesta = true;
                }
            }
            return respuesta;
        }
        private bool ValidarFecha(string fecha)
        {
            try
            {
                if (fecha == "")
                {
                    return true;
                }
                else
                {
                    DateTime.Parse(fecha);
                    return true; 
                }
            }
            catch
            {
                return false;
            }
        }
        protected void lkbDescargar_Click(object sender, EventArgs e)
        {
            string FolderPath = ConfigurationManager.AppSettings["UploadPath"];
            string FilePath;
            FilePath = FolderPath + @"\"+ "PLANTILLA.xlsx";
            // Limpiamos la salida
            Response.Clear();
            // Con esto le decimos al browser que la salida sera descargable
            Response.ContentType = "application/octet-stream";
            // esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
            Response.AddHeader("Content-Disposition", "attachment; filename=Plantilla.xlsx");
            // Escribimos el fichero a enviar 
            Response.WriteFile(FilePath);
            // volcamos el stream 
            Response.Flush();
            // Enviamos todo el encabezado ahora
            Response.End();
        }

        private void chkOtroMesCheckedChanged()
        {
            if (chkOtroMes.Checked)
            {
                ddlAnioTran.Visible = true;
                ddlMesTran.Visible = true;
            }
            else
            {
                ddlAnioTran.Visible = false;
                ddlMesTran.Visible = false;
            }
        }
        protected void chkOtroMes_CheckedChanged(object sender, EventArgs e)
        {
            chkOtroMesCheckedChanged();
        }

        protected void chkOperacionesPend_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOperacionesPend.Checked)
            {
                if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_INGRESO_CONCILIACION));
                    EstadoOcultar.Visible = false;
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlEstadoDep, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_ESTADO_DEPOSITO_CONCILIACION));
                    ddlEstadoDep.SelectedValue = ddlEstadoDep.Items.FindByText("PENDIENTE").Value;
                    EstadoOcultar.Visible = true;
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_PARAMETRO_EGRESO_CONCILIACION));
                }
                panel.Visible = false;
            }
            else {
                EstadoOcultar.Visible = false;
                if (Convert.ToInt32(ddlOperacionTipo.SelectedValue) == (int)Enumerador.enmTipoMovimientoTransaccion.INGRESOS)
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_INGRESO)); 
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlTransaccionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_EGRESO)); 
                }
            }
            // PARA QUITAR EN ALGUN MOMENTO
            EstadoOcultar.Visible = false;
        }

        private void ListarConciliacionesPendientes()
        {
            
            TransaccionConsultasBL _obj = new TransaccionConsultasBL();
            DataTable _dt = new DataTable();
            if (hTransaccionEditar.Value == "")
            {
                hTransaccionEditar.Value = "0";
            }
            _dt = _obj.ListarConciliacionesPendientes(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt16(ddlCuentaMant.SelectedValue),Convert.ToInt64(hTransaccionEditar.Value));

            if (_dt.Rows.Count > 0)
            {
                lblConciliaciones.Text = "Conciliaciones Pendientes - Nro.Cuenta: " + ddlCuentaMant.SelectedItem.Text + " :";
            }
            else { lblConciliaciones.Text = ""; }
            grvConciliacionPendientes.DataSource = _dt;
            grvConciliacionPendientes.DataBind();
        }

        protected void ddlEstadoDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstadoDep.SelectedItem.Text == Constantes.CONST_PARAMETRO_ESTADO_CONCILIACION_CONCILIADO)
            { OcultarFechaConciliacion.Visible = true; }
            else { OcultarFechaConciliacion.Visible = false; }
            
        }

        protected void Conciliar(object sender, ImageClickEventArgs e)
        {
            double MontoConciliar = 0;
            MontoConciliar = Convert.ToDouble(((ImageButton)sender).Attributes["MontoConciliar"]);
            long _idTransaccion = 0;
            _idTransaccion = Convert.ToInt64(((ImageButton)sender).Attributes["Transaccion"]);
            Comun.EjecutarScript(this, "PopupConciliacion(" + _idTransaccion.ToString() + "," + MontoConciliar.ToString() + ");");
        }

        protected void ConciliarParte(object sender, ImageClickEventArgs e)
        {
            double MontoConciliar = 0;
            MontoConciliar = Convert.ToDouble(((ImageButton)sender).Attributes["MontoConciliar"]);


            long _idTransaccion = 0;
            _idTransaccion = Convert.ToInt64(((ImageButton)sender).Attributes["Transaccion"]);
            Comun.EjecutarScript(this, "PopupConciliacionParte(" + _idTransaccion.ToString() + "," + MontoConciliar.ToString() + ");");
        }

        protected void BtnConciliar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime datFechaConciliacion = new DateTime();
                double NuevoMonto;
                if (!DateTime.TryParse(CtrFechaConciliacionPop.Text, out datFechaConciliacion))
                {
                    datFechaConciliacion = Comun.FormatearFecha(CtrFechaConciliacionPop.Text);
                }
                if (txtMontoNuevo.Text == "")
                {
                    NuevoMonto = 0;
                }
                else {
                    NuevoMonto = Convert.ToDouble(txtMontoNuevo.Text);
                }
                TransaccionMantenimientoBL _obj = new TransaccionMantenimientoBL();
                _obj.ActualizarDatosConciliacion(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), Convert.ToInt64(hTransaccion.Value), datFechaConciliacion, NuevoMonto);
                ListarConciliacionesPendientes();
                Comun.EjecutarScript(Page, Util.HabilitarTab(1));
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
            
        }

    }
}
