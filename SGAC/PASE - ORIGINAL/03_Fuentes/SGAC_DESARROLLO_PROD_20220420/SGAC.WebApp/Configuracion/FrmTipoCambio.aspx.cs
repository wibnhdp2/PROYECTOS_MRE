using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL; 
 
namespace SGAC.WebApp.Configuracion
{
    public partial class TipoCambio : MyBasePage
    {
        private string formatoTCC;
        private string formatoTCB;

        #region CAMPOS
        private string strNombreEntidad = "TIPO CAMBIO";
        private string strVariableAccion = "TipoCambio_Accion";
        private string strVariableDt = "TipoCambio_Tabla";
        private string strVariableIndice = "TipoCambio_Indice";       
        #endregion
                
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            formatoTCC = WebConfigurationManager.AppSettings["FormatoDecimalTCC"];
            formatoTCB = WebConfigurationManager.AppSettings["FormatoDecimalTCB"];

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

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";


            this.dtpFecIniConsulta.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecIniConsulta.EndDate = new DateTime(3000,1,1);

            this.dtpFecFinConsulta.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecFinConsulta.EndDate = new DateTime(3000,1,1);

            this.dtpFecActual.StartDate = DateTime.Now;
            this.dtpFecActual.EndDate = new DateTime(3000,1,1);

            this.dtpFecFin.StartDate = DateTime.Now;
            this.dtpFecFin.EndDate = new DateTime(3000, 1, 1);
            this.dtpFecFin.AllowFutureDate = true;

            string strFormatofecha = "";
            strFormatofecha = WebConfigurationManager.AppSettings["FormatoFechas"];
            Session["Formatofecha"] = strFormatofecha;

            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvTipoCambio };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }
        
        protected void gdvTipoCambio_RowCommand(object sender, GridViewCommandEventArgs e)
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

        protected void gdvTipoCambio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text.Trim() != "&nbsp;")
                    {
                        e.Row.Cells[0].Text = (Comun.FormatearFecha(e.Row.Cells[0].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                        e.Row.Cells[1].Text = (Convert.ToDouble(e.Row.Cells[1].Text)).ToString(formatoTCC);
                    }
                }               
            }
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (dtpFecIniConsulta.Text.Trim().Length == 0 || dtpFecFinConsulta.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(dtpFecIniConsulta.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);                
                return;
            }
            if (Comun.EsFecha(dtpFecFinConsulta.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);                
                return;
            }

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecIniConsulta.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFinConsulta.Text);
                       
            if (datFechaInicio > datFechaFin)
            {
                Session[strVariableDt] = new DataTable();
                gdvTipoCambio.DataSource = new DataTable();
                gdvTipoCambio.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                ctrlPaginador.InicializarPaginador();
                CargarGrilla();
            }
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            gdvTipoCambio.DataSource = null;
            gdvTipoCambio.DataBind();
            
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            ctrlToolBarMantenimiento_btnCancelarHandler();
            updMantenimiento.Update();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;           

            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();

            HallarTCConsular();

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
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            //Validar al grabar el maximo valor del tipo de Cambio Consular
            if (Convert.ToDouble(txtTCConsular.Text) > Convert.ToDouble(hfTope.Value) && enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "TIPO CAMBIO CONSULAR", "El tipo de cambio consular no debe exceder de: " + Convert.ToDouble(hfTope.Value));
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            object[] arrParametros = new object[1];
            TipoCambioMantenimientoBL BL = new TipoCambioMantenimientoBL();
            SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR obj = new BE.MRE.SI_TIPOCAMBIO_CONSULAR();
            Proceso p = new Proceso();
            int intResultado = 0;           

            double PTCC = Convert.ToDouble(txtTCBancario.Text);
            double LTCC = Convert.ToDouble(txtLucroCambio.Text);
            double TCC = Convert.ToDouble(txtTCConsular.Text);

            if (PTCC == 0)
            {
                strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TIPO DE CAMBIO CONSULAR", "No puede grabar tipos de cambio con promedios iguales a cero.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (LTCC == 0)
            {
                strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TIPO DE CAMBIO CONSULAR", "No puede grabar tipos de cambio lucros iguales a cero.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (TCC == 0)
            {
                strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TIPO DE CAMBIO CONSULAR", "No puede grabar tipos de cambio iguales a cero.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;
                    intResultado = BL.Insert(obj);
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    obj = ObtenerEntidadMantenimiento();
                    arrParametros[0] = obj;
                    intResultado = BL.Update(obj);
                    break;

                case Enumerador.enmAccion.ELIMINAR:
                    // Si es diferente a la fecha actual
                    if (!dtpFecActual.Text.Equals(DateTime.Today.ToString("MM-dd-yyyy")))
                    {
                        obj = ObtenerEntidadMantenimiento();
                        arrParametros[0] = obj;
                        intResultado = BL.Delete(obj);
                    }
                    else
                    {
                        intResultado = (int)Enumerador.enmResultadoQuery.OTRO;
                    }
                    break;
            }

            strScript = string.Empty;
            if (p.IErrorNumero == Constantes.CONST_CONTROLADOR_ERROR_NO)
            {
                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    // Si es el día actual y se ha modificado el tipo de cambio consular
                    double dblTCCActual = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

                    // Obtener la fecha de la OC (PENDIENTE)                    
                    if (dtpFecActual.Text.Trim().Equals(
                                                        Util.ObtenerFechaActual(obj.DiferenciaHoraria, obj.HorarioVerano).ToString(ConfigurationManager.AppSettings["FormatoFechas"])))
                    {
                        Session[Constantes.CONST_SESION_TIPO_CAMBIO] = obj.tico_FValorConsular.ToString(formatoTCC);
                        Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO] = obj.tico_FValorBancario;
                        Session[Constantes.CONST_SESION_TIPO_MONEDA_ID] = obj.tico_sMonedaId;
                        Session[Constantes.CONST_SESION_TIPO_MONEDA] = ddlMonedaTipo.SelectedItem.Text;

                        // Actualiza el TC del MasterPage cuando Varía
                        if (enmAccion != Enumerador.enmAccion.ELIMINAR)
                        {
                            if (Convert.ToDouble(obj.tico_FValorConsular) != dblTCCActual)
                            {                                
                                Label mpLabel = new Label();                                
                                mpLabel = (Label)Master.FindControl("lblTipoCambio");
                                if (mpLabel != null)
                                {
                                    mpLabel.Text = "T.C. Consular: " +
                                                   Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString() + " " +
                                                   Session[Constantes.CONST_SESION_TIPO_MONEDA];
                                }
                            }
                        }
                    }

                    if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                    {
                        ctrlPaginador.PaginaActual = 1;
                        ctrlPaginador.InicializarPaginador();

                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                    }
                    else
                    {
                        ctrlPaginador.PaginaActual = 1;
                        ctrlPaginador.InicializarPaginador();

                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                    }

                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.HabilitarTab(0);

                    LimpiarDatosMantenimiento();
                    HabilitarMantenimiento();

                    CargarGrilla();
                }
                else if (intResultado == (int)Enumerador.enmResultadoQuery.OTRO)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No se puede eliminar el Tipo de Cambio de Hoy.");
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

            ctrlToolBarMantenimiento_btnCancelarHandler();

            Session["MODIFICO"] = "MODIFICADO";
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();          

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }      

        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            LimpiarDatosConsulta();
            LimpiarDatosMantenimiento();

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarComboAnios(ddlPorcentaje, 1, Constantes.CONST_PORCENTAJE_MAX_TC);

            DataTable dtMonedas = new DataTable();
            dtMonedas = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            Util.CargarParametroDropDownList(ddlMonedaTipo, dtMonedas);
            Util.CargarParametroDropDownList(ddlMonedaLocal, dtMonedas);
        }

        private void CargarGrilla()
        {
            TipoCambioConsultasBL objBL = new TipoCambioConsultasBL();
            
            object[] arrParametros = ObtenerFiltro();
            int intTotalRegistros = 0, intTotalPaginas = 0;

            try
            {
                DataTable dt = new DataTable();
                dt = objBL.Obtener(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                Comun.FormatearFecha(dtpFecIniConsulta.Text), Comun.FormatearFecha(dtpFecFinConsulta.Text),
                ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas);
            
                Session[strVariableDt] = dt;
                gdvTipoCambio.SelectedIndex = -1;
                gdvTipoCambio.DataSource = dt;
                gdvTipoCambio.DataBind();

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
            catch (Exception ex)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, ex.Message);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        private object[] ObtenerFiltro()
        {
            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecIniConsulta.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFinConsulta.Text);
                      
            int intTotalRegistros = 0, intTotalPaginas = 0;
            object[] arrParametros = new object[7];
            arrParametros[0] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            arrParametros[1] = datFechaInicio;
            arrParametros[2] = datFechaFin;
            arrParametros[3] = ctrlPaginador.PaginaActual;
            arrParametros[4] = Constantes.CONST_CANT_REGISTRO;
            arrParametros[5] = intTotalRegistros;
            arrParametros[6] = intTotalPaginas;
            return arrParametros;
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
                    dtpFecActual.Text = Comun.FormatearFecha(drSeleccionado["tico_dFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    dtpFecFin.Text = Comun.FormatearFecha(drSeleccionado["tico_dFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    dtpFecFin.Enabled = false;

                    ddlMonedaTipo.SelectedValue = drSeleccionado["tico_sMonedaId"].ToString();                   
                    ddlPorcentaje.SelectedValue = drSeleccionado["tico_FPorcentaje"].ToString().Trim();                          

                    if (drSeleccionado["tico_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkActivoMant.Checked = true;
                    }
                    else
                    {
                        chkActivoMant.Checked = false;
                    }

                    txtTCBancario.Text = double.Parse(drSeleccionado["tico_FValorBancario"].ToString()).ToString();
                    txtLucroCambio.Text = double.Parse(drSeleccionado["tico_FPromedio"].ToString()).ToString(formatoTCB);
                    txtTCConsular.Text = double.Parse(drSeleccionado["tico_FValorConsular"].ToString()).ToString(formatoTCC);

                    updMantenimiento.Update();
                }
            }
        }

        private void ObtenerPromedioTCB()
        {
            //Proceso p = new Proceso();
            //object[] arrParametros = { 
            //                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 
            //                            DateTime.Today,
            //                            DateTime.Today.AddDays(-15)
            //                         };

            //DataTable dtDatos = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SI_TIPOCAMBIO_BANCARIO", "CONSULTA_PROMEDIO");

            TipoCambioBancarioConsultasBL objTipoCambioBancarioConsultaBL = new TipoCambioBancarioConsultasBL();
            DataTable dtDatos = new DataTable();

            dtDatos = objTipoCambioBancarioConsultaBL.ObtenerPromedioTipoCambio(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), DateTime.Today, DateTime.Today.AddDays(-15));


            //if (p.IErrorNumero == Constantes.CONST_CONTROLADOR_ERROR_NO)
            //{
                if (dtDatos.Rows.Count > 0)
                {
                    double TCBancario = 0;

                    if (Convert.ToDouble(dtDatos.Rows[0]["FValorBancario"]) > 0)
                    {
                        // Obtener el promedio Actual
                        TCBancario = Convert.ToDouble(dtDatos.Rows[0]["FPromedio"].ToString());
                        txtTCBancario.Text = TCBancario.ToString(formatoTCB);
                    }                    
                }
           // }

            updMantenimiento.Update();
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {            
            ddlPorcentaje.Enabled = bolHabilitar;
            txtTCConsular.Enabled = bolHabilitar;
            dtpFecFin.Enabled = bolHabilitar;
        }

        private void LimpiarDatosConsulta()
        {
            DateTime fFchInicio = Comun.FormatearFecha("01" + "/" + DateTime.Today.ToString("MM") + "/" + DateTime.Today.ToString("yyyy"));

            string strFormatofecha = "";
            strFormatofecha = Convert.ToString(Session["Formatofecha"]);

            dtpFecIniConsulta.Text = fFchInicio.ToString(strFormatofecha);

            string strFechaTexto = Comun.ObtenerFechaActualTexto(Session);

            dtpFecFinConsulta.Text = strFechaTexto;
            dtpFecActual.Text = strFechaTexto;
            dtpFecFin.Text = strFechaTexto;
        }

        private void LimpiarDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            dtpFecActual.Text = Comun.ObtenerFechaActualTexto(Session);
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

            txtTCConsular.Text = "";
            txtTCBancario.Text = "";

            double lucroCambio = 0;
            txtLucroCambio.Text = lucroCambio.ToString(formatoTCB);
            ddlPorcentaje.SelectedIndex = 0;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID]) != 0)
            {
                ddlMonedaTipo.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID].ToString();
                ddlMonedaLocal.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID].ToString();
            }
            else
            {                
                ddlMonedaTipo.SelectedIndex = 0;
                ddlMonedaLocal.SelectedIndex = 0;
            }

            //Proceso p = new Proceso();
            //object[] arrParametros = { 
            //                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 
            //                            DateTime.Today, 
            //                            DateTime.Today.AddDays(-15)
            //                         };

            //DataTable dtDatos = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SI_TIPOCAMBIO_BANCARIO", "CONSULTA_PROMEDIO");


            try
            {
                TipoCambioBancarioConsultasBL obj = new TipoCambioBancarioConsultasBL();
                DataTable dtDatos = obj.ObtenerPromedioTipoCambio(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                            DateTime.Today,
                                            DateTime.Today.AddDays(-15));
                if (dtDatos.Rows.Count > 0)
                {
                    double TCBancario = 0;
                    double dTCBancario = 0;
                    double dLucroCambio = 0;

                    if (Convert.ToDouble(dtDatos.Rows[0]["FValorBancario"]) > 0)
                    {
                        TCBancario = Convert.ToDouble(dtDatos.Rows[0]["FValorBancario"].ToString());
                        txtTCBancario.Text = TCBancario.ToString(formatoTCB);
                    }

                    dTCBancario = Convert.ToDouble(dtDatos.Rows[0]["FPromedio"].ToString());
                    txtTCBancario.Text = dTCBancario.ToString(formatoTCB).ToString();
                    dLucroCambio = Convert.ToDouble(dtDatos.Rows[0]["FPromedio"].ToString());
                    txtLucroCambio.Text = dLucroCambio.ToString(formatoTCB);
                }

                HallarTCConsular();
                chkActivoMant.Checked = true;
            }
            catch
            { }
        }        

        private SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR ObtenerEntidadConsulta()
        {
            int intSeleccionado = (int)Session[strVariableIndice];
            DataTable dt = (DataTable)Session[strVariableDt];
            DataRow drSeleccionado = dt.Rows[intSeleccionado];

            SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR obj = new SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR();

            obj.tico_iTipoCambioConsularId = Convert.ToInt64(drSeleccionado["tico_iTipoCambioId"]);
            obj.tico_sOficinaConsularId = Convert.ToInt16(drSeleccionado["tico_sOficinaConsularId"]);
            obj.tico_dFecha = Comun.FormatearFecha(drSeleccionado["tico_dFecha"].ToString());
            obj.tico_sMonedaId = Convert.ToInt16(drSeleccionado["tico_sMonedaId"]);
            obj.tico_FPorcentaje = Convert.ToDouble(drSeleccionado["tico_FPorcentaje"]);
            obj.tico_FPromedio = Convert.ToDouble(drSeleccionado["tico_FPromedio"]);
            obj.tico_FValorConsular = Convert.ToDouble(drSeleccionado["tico_FValorConsular"]);
            obj.tico_FValorBancario = Convert.ToDouble(drSeleccionado["tico_FValorBancario"]);
            obj.tico_cEstado = drSeleccionado["tico_cEstado"].ToString();

            return obj;
        }

        private SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR ObtenerEntidadMantenimiento()
        {
            SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR obj = new SGAC.BE.MRE.SI_TIPOCAMBIO_CONSULAR();

            obj.tico_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                obj.tico_iTipoCambioConsularId = Convert.ToInt64(ObtenerFilaSeleccionada()["tico_ITipoCambioId"]);
                obj.tico_sOficinaConsularId = Convert.ToInt16(ObtenerFilaSeleccionada()["tico_sOficinaConsularId"]);
            }

            DateTime datFecha = Comun.FormatearFecha(dtpFecActual.Text);
            obj.tico_dFecha = datFecha;

            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            obj.tico_dFechaFin = datFechaFin;

            DateTime datFechaCreacion = Comun.FormatearFecha(dtpFecActual.Text);
            obj.tico_dFechaCreacion = datFechaCreacion;

            obj.tico_sMonedaId = Convert.ToInt16(ddlMonedaTipo.SelectedValue.ToString());

            if (ddlPorcentaje.SelectedIndex > 0)
            {
                obj.tico_FPorcentaje = Convert.ToDouble(ddlPorcentaje.SelectedValue);
            }
            else
            {
                obj.tico_FPorcentaje = 1.0;
            }

            obj.tico_FValorConsular = Convert.ToDouble(txtTCConsular.Text.Trim());

            if (txtTCBancario.Text.Trim() != string.Empty)
            {
                obj.tico_FValorBancario = Convert.ToDouble(txtTCBancario.Text.Trim());
            }

            if (txtLucroCambio.Text.Trim() != string.Empty)
            {
                obj.tico_FPromedio = Convert.ToDouble(txtLucroCambio.Text.Trim());
            }
            else
            {
                obj.tico_FPromedio = 0;
            }

            if (chkActivoMant.Checked)
            {
                obj.tico_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                obj.tico_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            obj.tico_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj.tico_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            obj.tico_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj.tico_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

            obj.DiferenciaHoraria = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(obj.tico_sOficinaConsularId, "ofco_sDiferenciaHoraria"));
            obj.HorarioVerano = Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(obj.tico_sOficinaConsularId, "ofco_sHorarioVerano"));

            return obj;
        }

        protected void ddlPorcentaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                ObtenerPromedioTCB();             
                HallarTCConsular();
            }
            else if (enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                HallarTCConsular();
            }
        }       

        void HallarTCConsular()
        {
            double dblTC = 0;
            double dblTcPromedio = 0;
            double dblPorcentaje = 0;
            double dblValorPorcentaje = 0;
            string strScript = string.Empty;            

            dblPorcentaje = Convert.ToDouble(ddlPorcentaje.SelectedValue);
            dblTcPromedio = Convert.ToDouble(txtTCBancario.Text);

            dblValorPorcentaje = (dblTcPromedio * (dblPorcentaje/100));
            dblTC = (dblTcPromedio + dblValorPorcentaje);

            // Varía según cambio del porcentaje
            double dLucroCambio = dblValorPorcentaje;
            txtLucroCambio.Text = dLucroCambio.ToString(formatoTCB);
            
            double dblTCCMaximo = Math.Round(dblTcPromedio * Convert.ToDouble(1.00 + (Constantes.CONST_PORCENTAJE_MAX_TC / 100.00)),2); 
            
            hfTope.Value = dblTCCMaximo.ToString();

            if (dblTC > dblTCCMaximo)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "TIPO CAMBIO CONSULAR", "El tipo de cambio consular no debe exceder de: " + dblTCCMaximo);
                Comun.EjecutarScript(Page, strScript);
            }           

            txtTCConsular.Text = dblTC.ToString(formatoTCC);            

            updMantenimiento.Update();            
        }

    }
}