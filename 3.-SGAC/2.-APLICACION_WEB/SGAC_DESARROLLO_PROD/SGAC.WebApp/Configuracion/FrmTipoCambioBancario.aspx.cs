using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL; 

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmTipoCambioBancario : MyBasePage
    {
        private string formatoTCB;

        #region CAMPOS
        private string strNombreEntidad = "T.C. BANCARIO";
        private string strVariableAccion = "TipoCambioBan_Accion";
        private string strVariableDt = "TipoCambioBanc_Tabla";
        private string strVariableIndice = "TipoCambioBanc_Indice";
        #endregion
               
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            /*CARGA EL FORMATO NUMERRICO PARA ESTE MODULO*/
            formatoTCB = WebConfigurationManager.AppSettings["FormatoDecimalTCB"];

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

            this.dtpFecIniConsulta.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecIniConsulta.EndDate = new DateTime(3000,1,1);

            this.dtpFecFinConsulta.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecFinConsulta.EndDate = new DateTime(3000,1,1);

            this.dtpFecActual.StartDate = DateTime.Now;
            this.dtpFecActual.EndDate = new DateTime();

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
                if (e.Row.Cells[0].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[0].Text = (Comun.FormatearFecha(e.Row.Cells[0].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    e.Row.Cells[1].Text = (Convert.ToDouble(e.Row.Cells[1].Text)).ToString(formatoTCB);
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

            HabilitarMantenimiento();

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;            
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

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
            TipoCambioBancarioMantenimientoBL BL = new TipoCambioBancarioMantenimientoBL();
            object[] arrParametros = new object[1];
            SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO obj = new SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO();
            //Proceso p = new Proceso();
            int intResultado = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            double TCB = 0;
            try { TCB = Convert.ToDouble(txtTCBancario.Text); }
            catch (FormatException ex)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "TIPO DE CAMBIO BANCARIO", "El formato de tipo de cambio no es correcto.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }            

            if (TCB == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TIPO DE CAMBIO BANCARIO", "No puede grabar tipos de cambio iguales a cero.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            try {
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
                        obj = ObtenerEntidadMantenimiento();
                        arrParametros[0] = obj;
                        intResultado = BL.Delete(obj);
                        break;
                }
                strScript = string.Empty;

                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
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

                    CargarGrilla();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }

                Comun.EjecutarScript(Page, strScript);

                ctrlToolBarMantenimiento_btnCancelarHandler();
            }
            catch (Exception ex)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, ex.Message);
            }
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }
        
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());           

            LimpiarDatosConsulta();
            LimpiarDatosMantenimiento();

            string strScript = string.Empty;
            // IDM (30/11/2014)
            Comun.EnviarAlertaContable(Session, Page);            
            // FIN

            strScript += Util.NombrarTab(0, "Consulta");
            Comun.EjecutarScript(Page, strScript);

            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlMonedaTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA));
        }

        private void CargarGrilla()
        {
            try
            {
                object[] arrParametros = ObtenerFiltro();
                DataTable dt = new DataTable();
                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SI_TIPOCAMBIO_BANCARIO", Enumerador.enmAccion.CONSULTAR);

                int intTotalRegistros = 0, intTotalPaginas = 0;
                TipoCambioBancarioConsultasBL obj = new TipoCambioBancarioConsultasBL();
                dt = obj.Obtener(Convert.ToInt32(arrParametros[0].ToString()), Comun.FormatearFecha(arrParametros[1].ToString()), Comun.FormatearFecha(arrParametros[2].ToString()), Convert.ToInt32(arrParametros[3].ToString()),
                    Convert.ToInt32(arrParametros[4].ToString()), ref intTotalRegistros, ref intTotalPaginas);
                if (dt.Rows.Count >= 0)
                {
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
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(arrParametros[5]), true, Enumerador.enmTipoMensaje.INFORMATION);
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
            }
            catch (Exception ex)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, ex.Message);
                Comun.EjecutarScript(Page, strScript);
            }
            //Proceso p = new Proceso();
            
        }

        private object[] ObtenerFiltro()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;
            object[] arrParametros = new object[7];

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecIniConsulta.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFinConsulta.Text);

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
                    dtpFecActual.Text = Comun.FormatearFecha(drSeleccionado["tiba_dFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    dtpFecFin.Text = Comun.FormatearFecha(drSeleccionado["tiba_dFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    dtpFecFin.Enabled = false;
                    ddlMonedaTipo.SelectedValue = drSeleccionado["tiba_sMonedaId"].ToString();                                  

                    txtTCBancario.Text = double.Parse(drSeleccionado["tiba_FValorBancario"].ToString()).ToString(formatoTCB);

                    if (drSeleccionado["tiba_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
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
            txtTCBancario.Enabled = bolHabilitar;
            dtpFecFin.Enabled = bolHabilitar;
        }

        private void LimpiarDatosConsulta()
        {
            DateTime fFchInicio = Comun.FormatearFecha("01/" + DateTime.Now.ToString("MM/yyyy"));

            string strFormatofecha = "";
            strFormatofecha = Convert.ToString(Session["Formatofecha"]);

            dtpFecIniConsulta.Text = fFchInicio.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            string strFechaTexto = Comun.ObtenerFechaActualTexto(Session);
            dtpFecFinConsulta.Text = strFechaTexto;
            dtpFecActual.Text = strFechaTexto;
            dtpFecFin.Text = strFechaTexto;
            gdvTipoCambio.DataSource = null;
            gdvTipoCambio.DataBind();            
            
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;
        }

        private void LimpiarDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            dtpFecActual.Text = Comun.ObtenerFechaActualTexto(Session);
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);
            txtTCBancario.Text = "0";

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID]) != 0)
            {
                ddlMonedaTipo.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID].ToString();
            }
            else
            {
                ddlMonedaTipo.SelectedIndex = 0;
            }

            chkActivoMant.Checked = true;
        }

        private SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO ObtenerEntidadConsulta()
        {
            int intSeleccionado = (int)Session[strVariableIndice];
            DataTable dt = (DataTable)Session[strVariableDt];
            DataRow drSeleccionado = dt.Rows[intSeleccionado];

            SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO obj = new SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO();

            obj.tiba_iTipoCambioBancarioId = Convert.ToInt64(drSeleccionado["tiba_iTipoCambioId"]);
            obj.tiba_sOficinaConsularId = Convert.ToInt16(drSeleccionado["tiba_sOficinaConsularId"]);
            obj.tiba_dFecha = Comun.FormatearFecha(drSeleccionado["tiba_dFecha"].ToString());
            obj.tiba_sMonedaId = Convert.ToInt16(drSeleccionado["tiba_sMonedaId"]);
            obj.tiba_FValorBancario = Convert.ToDouble(drSeleccionado["tiba_FValorBancario"]);
            obj.tiba_cEstado = drSeleccionado["tiba_cEstado"].ToString();

            return obj;
        }
         
        private SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO ObtenerEntidadMantenimiento()
        {
            SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO obj = new SGAC.BE.MRE.SI_TIPOCAMBIO_BANCARIO();

            obj.tiba_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                obj.tiba_iTipoCambioBancarioId = Convert.ToInt64(ObtenerFilaSeleccionada()["tiba_ITipoCambioId"]);
                obj.tiba_sOficinaConsularId = Convert.ToInt16(ObtenerFilaSeleccionada()["tiba_sOficinaConsularId"]);
            }

            DateTime datFechaActual = Comun.FormatearFecha(dtpFecActual.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);

            obj.tiba_dFecha = datFechaActual;
            obj.tiba_dFechaFin = datFechaFin;

            obj.tiba_sMonedaId = Convert.ToInt16(ddlMonedaTipo.SelectedValue.ToString());

            if (txtTCBancario.Text.Trim() != string.Empty)
            {
                obj.tiba_FValorBancario = Convert.ToDouble(txtTCBancario.Text);
            }

            if (chkActivoMant.Checked)
            {
                obj.tiba_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                obj.tiba_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            obj.tiba_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj.tiba_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            obj.tiba_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj.tiba_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

            return obj;
        }
    }
}