using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using System.Web.Configuration;
using SGAC.Almacen.BL;


namespace SGAC.WebApp.Almacen
{
    public partial class ControlStock : MyBasePage
    {
        #region VARIABLES
        public string strMesN;

        #endregion
         
        #region CAMPOS
        private string strVariableAccion = "Listado_Accion";
        private string strNombreEntidad = "CONTROL DE STOCK";

        ReportParameter[] parameters;
        String sNombreDsReporteServices = String.Empty;
        String strRutaBase = String.Empty;

        DataTable dtReporte = null;
        #endregion

        #region EVENTOS
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PasaPedidoMovimiento"] = 0;

            ctrlToolBar1.VisibleIButtonBuscar = true;
            ctrlToolBar1.VisibleIButtonCancelar = true;
            ctrlToolBar1.VisibleIButtonPrint = true;

            ctrlToolBar1.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBar1.btnCancelar.Text = "      Limpiar";

            ctrlToolBar1.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar1_btnBuscarHandler);
            ctrlToolBar1.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar1_btnCancelarHandler);
            ctrlToolBar1.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBar1_btnPrintHandler);

            cboMisionConsO.AutoPostBack = true;
            cboMisionConsO.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsularO_SelectedIndexChanged);

            this.dtpFecInicio.StartDate = new DateTime(1900,1,1);
            this.dtpFecInicio.EndDate = new DateTime(3000,1,1);

            this.dtpFecFin.StartDate = new DateTime(1900,1,1);
            this.dtpFecFin.EndDate = new DateTime(3000,1,1);

            //string strFormatofecha = "";
            //strFormatofecha = WebConfigurationManager.AppSettings["FormatoFechas"];
            //Session["Formatofecha"] = strFormatofecha;

            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();
            Session["Formatofecha"] = strFormatoFechas;

            if (!IsPostBack)
            {
                llenarBovedas();
                lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);
                dtpFecInicio.Text = DateTime.Today.ToString(strFormatoFechasInicio);
                dtpFecFin.Text = DateTime.Today.ToString(strFormatoFechas);
                CargarListados();
                cboInsumo.SelectedValue = Convert.ToInt32(Enumerador.enmInsumoTipo.AUTOADHESIVO).ToString();
                cboInsumo.Enabled = true;
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
                cboInsumo.Enabled = false;

                updConsulta.Update();
            }
        }

        void ctrlToolBar1_btnPrintHandler()
        {
            if (cboBovedaO.SelectedItem.Text == "- SELECCIONAR -")
            {

                Session["dt"] = new DataTable();
                grReporteGestion.DataSource = new DataTable();
                grReporteGestion.DataBind();

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Se debe seleccionar la Bóveda"));
                return;
            }

            if (dtpFecInicio.Text.Trim().Length == 0 || dtpFecFin.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);

                return;
            }
            if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);

                return;
            }

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);


            if (datFechaInicio > datFechaFin)
            {
                Session["dt"] = new DataTable();
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {
                CargarReporte();
            }

        }

        void ddlOficinaConsularO_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarGrilla();

            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.DefaultView;
            //-----------------------------------------------

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue.ToString() + " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;
            Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
        }

        void ctrlToolBar1_btnBuscarHandler()
        {
            if (dtpFecInicio.Text.Trim().Length == 0 || dtpFecFin.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(dtpFecInicio.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                
                return;
            }
            if (Comun.EsFecha(dtpFecFin.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                
                return;
            }
            if (cboBovedaO.SelectedItem.Text == "- SELECCIONAR -")
            {
                
                Session["dt"] = new DataTable();
                grReporteGestion.DataSource = new DataTable();
                grReporteGestion.DataBind();

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Se debe seleccionar la Bóveda"));
                return;
            }

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            

            if (datFechaInicio > datFechaFin)
            {
                Session["dt"] = new DataTable();
                grReporteGestion.DataSource = new DataTable();
                grReporteGestion.DataBind();
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {
                CargarGrilla();
            }
        }


        void ctrlToolBar1_btnCancelarHandler()
        {
            LimpiaCampos();
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
            ctrlPaginador.Visible = false;

            Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
            cboInsumo.Enabled = false;

        }


        protected void grReporteGestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal tot = (Literal)e.Row.FindControl("SALDO");
                int total = int.Parse(tot.Text);

                if (total < 0)
                {
                    e.Row.BackColor = Color.FromName("#ffc7ce");
                    e.Row.Cells[ObtenerIndiceColumnaGrilla(grReporteGestion, "INGRESOS")].BackColor = Color.FromName("#ffc7ce");
                    e.Row.Cells[ObtenerIndiceColumnaGrilla(grReporteGestion, "SALIDAS")].BackColor = Color.FromName("#ffc7ce");
                    e.Row.Cells[ObtenerIndiceColumnaGrilla(grReporteGestion, "SALDO")].BackColor = Color.FromName("#ffc7ce");
                    e.Row.Cells[ObtenerIndiceColumnaGrilla(grReporteGestion, "SALDO")].ForeColor = Color.FromName("#FF0000");
                    
                }
                
                int iFechaCol=ObtenerIndiceColumnaGrilla(grReporteGestion,"Fecha");

                if (e.Row.Cells[iFechaCol].Text.Trim() != "&nbsp;")
                    e.Row.Cells[iFechaCol].Text = (Comun.FormatearFecha(e.Row.Cells[iFechaCol].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

            }
        }
        #endregion


        #region METODOS
        private void CargarListados()
        {
            Util.CargarParametroDropDownList(cboInsumo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true, " - TODOS - ");
            Util.CargarParametroDropDownList(cboTipoBovedaO, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));

            //Trae Boveda Origen
            cboMisionConsO.Cargar();
            cboMisionConsO.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

            Util.CargarParametroDropDownList(cboTipoBovedaO, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));

            DataTable dtOficinasConsularesO = new DataTable();
            dtOficinasConsularesO = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

            //dtOficinasConsularesO = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();


            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvD = dtBovedas.DefaultView;
            //-----------------------------------------------
            
            //DataView dvD = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;
            dvD.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue + " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;
            Util.CargarDropDownList(cboBovedaO, dvD.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);

            if (((int)Constantes.CONST_OFICINACONSULAR_LIMA).ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
            {
                cboMisionConsO.Enabled = false;
            }

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                cboTipoBovedaO.SelectedValue = ((int)Enumerador.enmBovedaTipo.USUARIO).ToString();
                cboTipoBovedaO_SelectedIndexChanged(cboTipoBovedaO, null); 
            }
        }

        private void LimpiarGrilla()
        {
            Session["dt"] = null;
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
            LblSaldo.Text = "";
            lblSaldoFinal.Text = "";
        }

        private void LimpiaCampos()
        {
            //Campos de Consulta

            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            dtpFecInicio.Text = DateTime.Today.ToString(strFormatoFechasInicio);
            dtpFecFin.Text = DateTime.Today.ToString(strFormatoFechas);
            cboInsumo.SelectedIndex = 0;
            cboInsumo.Enabled = true;
            cboMisionConsO.SelectedIndex = 0;
            cboTipoBovedaO.SelectedIndex = 0;
            cboBovedaO.SelectedIndex = 0;
            cboMisionConsO.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.DefaultView;
            //-----------------------------------------------

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue.ToString() + " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;
            Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
        }

        private int ObtenerIndiceColumnaGrilla(GridView grid, string col)
        {


            string field = string.Empty;
            for (int i = 0; i < grid.Columns.Count; i++)
            {

                if (grid.Columns[i].GetType() == typeof(BoundField))
                {
                    field = ((BoundField)grid.Columns[i]).DataField.ToLower();
                }
                else if (grid.Columns[i].GetType() == typeof(TemplateField))
                {
                    field = ((TemplateField)grid.Columns[i]).HeaderText.ToLower();
                }

                if (field == col.ToLower())
                {
                    return i;
                }

                field = string.Empty;
            }

            return -1;
        }
        private void CargarReporte()
        {
            /*
                     *Jonatan Silva Cachay 11/09/2017
                     *reporte de control de stock
            */
            if (cboBovedaO.SelectedItem.Text != "- SELECCIONAR -")
            {
                string strMensaje = "";
                int intTipoInsumo = 0, intOficinaConsularIdOrigen = 0, intBovedaTipoIdOrigen = 0, intBodegaOrigenId = 0;
                int intOficinaConsularId = Convert.ToInt32(cboMisionConsO.SelectedValue);

                if (cboInsumo.SelectedValue != null)
                    intTipoInsumo = Convert.ToInt32(cboInsumo.SelectedValue);

                if (cboMisionConsO.SelectedValue != null)
                    intOficinaConsularIdOrigen = Convert.ToInt32(cboMisionConsO.SelectedValue);

                if (cboTipoBovedaO.SelectedValue != null)
                    intBovedaTipoIdOrigen = Convert.ToInt32(cboTipoBovedaO.SelectedValue);

                if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
                {
                    if (cboBovedaO.SelectedValue != null)
                        intBodegaOrigenId = intOficinaConsularIdOrigen;
                }
                else
                {
                    if (cboBovedaO.SelectedValue != null)
                        intBodegaOrigenId = Convert.ToInt32(cboBovedaO.SelectedValue);
                }

                strMensaje = "Mensaje";

                if (intOficinaConsularIdOrigen > 0 && intBovedaTipoIdOrigen > 0 && intBodegaOrigenId > 0)
                {

                    MovimientoConsultaBL oMovimientoConsultaBL = new MovimientoConsultaBL();

                    int intTotalRegistros = 0, intTotalPaginas = 0;

                    DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                    DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);



                    DataTable dt = oMovimientoConsultaBL.ConsultarStock(intTipoInsumo,
                                               datFechaInicio,
                                               datFechaFin,
                                               intOficinaConsularIdOrigen,
                                               intBovedaTipoIdOrigen,
                                               intBodegaOrigenId,
                                               ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO_REPORTE,
                                               ref intTotalRegistros,
                                               ref intTotalPaginas,
                                               ref strMensaje);


                    // Mensaje total de registros 0
                    if (dt.Rows.Count == 0)
                    { ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA); }
                    else
                    {
                        string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
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
                        
                        //strFechaActualConsulado = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
                        //strHoraActualConsulado = Accesorios.Comun.ObtenerHoraActualTexto(HttpContext.Current.Session);

                        string strBoveda = string.Empty;
                        string strFechaDe = string.Empty;
                        strBoveda = cboTipoBovedaO.SelectedItem.Text + " : " + cboBovedaO.SelectedItem.Text;
                        strFechaDe = "Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
                        ReportParameter[] parameters = new ReportParameter[9];
                        parameters[0] = new ReportParameter("TituloReporte", "CONTROL DE STOCK");
                        parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
                        parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
                        parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
                        parameters[4] = new ReportParameter("FechaHaber", " Saldo al " + dtpFecInicio.Text);
                        parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
                        parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
                        parameters[7] = new ReportParameter("Boveda", strBoveda);
                        parameters[8] = new ReportParameter("FechaDe", strFechaDe);

                        Session["strNombreArchivo"] = "Almacen/rsControlStock.rdlc";
                        Session["DtDatos"] = dt;
                        Session["objParametroReportes"] = parameters;
                        Session["DataSet"] = "dtControlStock";
                        string strUrl = "../Reportes/frmVisorReporte.aspx";
                        string Script = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=700,left=100,top=100');";
                        Comun.EjecutarScript(Page, Script);
                    }


                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
                }
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        private void CargarGrilla()
        {
            if (cboBovedaO.SelectedItem.Text!="- SELECCIONAR -")
            {
            string strMensaje = "";
            int intTipoInsumo = 0, intOficinaConsularIdOrigen = 0, intBovedaTipoIdOrigen = 0, intBodegaOrigenId = 0;
            int intOficinaConsularId = Convert.ToInt32(cboMisionConsO.SelectedValue);

            if (cboInsumo.SelectedValue != null)
                intTipoInsumo = Convert.ToInt32(cboInsumo.SelectedValue);

            if (cboMisionConsO.SelectedValue != null)
                intOficinaConsularIdOrigen = Convert.ToInt32(cboMisionConsO.SelectedValue);

            if (cboTipoBovedaO.SelectedValue != null)
                intBovedaTipoIdOrigen = Convert.ToInt32(cboTipoBovedaO.SelectedValue);

            if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
            {
                if (cboBovedaO.SelectedValue != null)
                    intBodegaOrigenId = intOficinaConsularIdOrigen;
            }
            else
            {
                if (cboBovedaO.SelectedValue != null)
                    intBodegaOrigenId = Convert.ToInt32(cboBovedaO.SelectedValue);
            }

            strMensaje = "Mensaje";  

            if (intOficinaConsularIdOrigen > 0 && intBovedaTipoIdOrigen > 0 && intBodegaOrigenId > 0)
            {

                MovimientoConsultaBL oMovimientoConsultaBL = new MovimientoConsultaBL();

                int intTotalRegistros = 0, intTotalPaginas = 0;

                DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
                

              
                DataTable dt=oMovimientoConsultaBL.ConsultarStock(intTipoInsumo,
                                           datFechaInicio,
                                           datFechaFin,
                                           intOficinaConsularIdOrigen,
                                           intBovedaTipoIdOrigen,
                                           intBodegaOrigenId,
                                           ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO,
                                           ref intTotalRegistros,
                                           ref intTotalPaginas,
                                           ref strMensaje);



                string cadena = strMensaje;

                string[] separadas;

                separadas = cadena.Split('|');

                LblSaldo.Text = separadas[0];
                lblSaldoFinal.Text = separadas[1];

                grReporteGestion.DataSource = dt;
                grReporteGestion.DataBind();

                // Mensaje total de registros 0
                if (dt.Rows.Count == 0)
                { ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA); }
                else
                { ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + dt.Rows.Count.ToString(), true, Enumerador.enmTipoMensaje.INFORMATION); }

                // Paginador
                ctrlPaginador.TotalResgistros = Convert.ToInt32(intTotalRegistros);
                ctrlPaginador.TotalPaginas = Convert.ToInt32(intTotalPaginas);

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;

                updConsulta.Update();
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
            }
        }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
            }

        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }
        #endregion

        protected void cboTipoBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarGrilla();
            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.DefaultView;
            //-----------------------------------------------

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue.ToString() + " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;
           
            if (Convert.ToInt32(cboBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
            {
                Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "OfConsularId", true);
            }
            else
            {
                Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
            }

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                cboBovedaO.SelectedValue = Session[Constantes.CONST_SESION_USUARIO_ID].ToString();
                cboTipoBovedaO.Enabled = false;
                cboBovedaO.Enabled = false;

            }
        }

        private void llenarBovedas()
        {
            DataTable dtBovedas = new DataTable();
            dtBovedas = Comun.ObtenerBovedas();
            grdAlmacenUniversal.DataSource = dtBovedas;
            grdAlmacenUniversal.DataBind();
        }

        private DataTable obtenerBovedas()
        {
            DataTable dt = new DataTable();

            dt = CrearDataTable();
            string strcelda = "";

            for (int i = 0; i < grdAlmacenUniversal.Rows.Count; i++)
            {
                DataRow dr;
                GridViewRow row = grdAlmacenUniversal.Rows[i];
                dr = dt.NewRow();
                for (int x = 0; x < row.Cells.Count; x++)
                {
                    strcelda = HttpUtility.HtmlDecode(row.Cells[x].Text);

                    dr[x] = strcelda;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        private DataTable CrearDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("OfConsularId", typeof(String));
            dt.Columns.Add("TipoBoveda", typeof(String));
            dt.Columns.Add("IdTablaOrigenRefer", typeof(String));
            dt.Columns.Add("Descripcion", typeof(String));
            dt.Columns.Add("Tabla", typeof(String));
            return dt;
        }

    }
}