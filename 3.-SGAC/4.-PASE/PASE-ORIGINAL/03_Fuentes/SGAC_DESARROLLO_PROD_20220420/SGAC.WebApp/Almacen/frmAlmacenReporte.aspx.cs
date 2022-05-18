using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Configuration;
using SGAC.WebApp.Accesorios;
using SGAC.WebApp.Accesorios.SharedControls;

namespace SGAC.WebApp.Almacen
{
    public partial class frmAlmacenReporte : System.Web.UI.Page
    {
        #region CAMPOS
        private string strVariableDt = "ReporteContable_Tabla";
        private string strVariableIndice = "ReporteContable_Indice";
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);


            this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecInicio.EndDate = DateTime.Now;

            this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecFin.EndDate = DateTime.Now;

            cboMisionConsular.AutoPostBack = true;
            cboMisionConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);


            Comun.CargarPermisos(Session, ctrlToolBarConsulta, null, gdvReporte, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
        
            if (!Page.IsPostBack)
            {
                llenarBovedas();
                CargarDatosIniciales();
                CargarListados();
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMisionConsular.SelectedValue.ToString() != "0")
            {
                CargarOficinaConsular(cboMisionConsular, cboTipoBovedaO, cboBovedaO, cboMisionConsular.SelectedValue, string.Empty);
            }
            else
            {

            }
        }

        protected void ddlLibroContableTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void ctrlToolBarConsulta_btnBuscarHandler()
        {

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (!DateTime.TryParse(dtpFecInicio.Text, out datFechaInicio))
            {
                datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            }
            if (!DateTime.TryParse(dtpFecFin.Text, out datFechaFin))
            {
                datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            }


            if (datFechaInicio > datFechaFin)
            {
                Session[strVariableDt] = new DataTable();
                gdvReporte.DataSource = new DataTable();
                gdvReporte.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                CargarGrilla();
            }
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
           
            

            if (datFechaInicio > datFechaFin)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                Session["FechaIntervalo"] = dtpFecInicio.Text + " - " + dtpFecFin.Text;
                Session["IdOficinaConsular"] = cboMisionConsular.SelectedValue.ToString();
                Session["IdTipoBoveda"] = cboMisionConsular.SelectedValue.ToString();
                Session["IdBoveda"] = cboMisionConsular.SelectedValue.ToString();

                VerVistaPrevia();
            }
        }
        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            //
        }
        #endregion

        #region Métodos

        private void CargarListados()
        {
            //-------------------------------------
            Enumerador.enmGrupo[] arrGrupos = {Enumerador.enmGrupo.ALMACEN_TIPO_REPORTE, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO, 
                                               Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA};
            DataTable dtGrupoParametros = new DataTable();

            dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

            
            Enumerador.enmGrupo[] arrGrupos1 = { Enumerador.enmGrupo.ALMACEN_TIPO_REPORTE };
            Enumerador.enmGrupo[] arrGrupos2 = { Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO, Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA };
            DropDownList[] arrDDL1 = { ddlTipoReporte };
            DropDownList[] arrDDL2 = { cboInsumo, cboMotivo, cboTipoBovedaO };


            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL1, arrGrupos1, dtGrupoParametros, true);
            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL2, arrGrupos2, dtGrupoParametros, true, " - TODOS - ");
            //-------------------------------------      

            //Util.CargarParametroDropDownList(ddlTipoReporte, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_REPORTE), true);
            //Util.CargarParametroDropDownList(cboInsumo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true, " - TODOS - ");
            //Util.CargarParametroDropDownList(cboMotivo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO), true, "  - TODOS - ");
            //Util.CargarParametroDropDownList(cboTipoBovedaO, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA), true, " - TODOS - ");
           

            //Se cargan combos de Consulta
            cboMisionConsular.Cargar(true);

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                CargarOficinaConsular(cboMisionConsular, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
            else
                CargarOficinaConsular(cboMisionConsular, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString(), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]).ToString(), true);

        }

        private void CargarDatosIniciales()
        {
            String FormatoFechas = String.Empty;
            String FormatoFechasInicio = String.Empty;

            FormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            FormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();
            
            dtpFecInicio.Text = DateTime.Today.ToString(FormatoFechasInicio);
            dtpFecFin.Text = DateTime.Today.ToString(FormatoFechas);
        }
       
        private void CargarGrilla()
        {
        }

        private void CargarOficinaConsular(ctrlOficinaConsular cboOficConsular, DropDownList cboTipoBoveda, DropDownList cboBoveda, string strOficConsularId, string strUsuarioId = "", bool bEsUsuario = false)
        {

            if (cboTipoBoveda.Items.Count == 0)
                Util.CargarParametroDropDownList(cboTipoBoveda, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));

            cboOficConsular.SelectedValue = strOficConsularId;

            cboTipoBoveda.SelectedValue = bEsUsuario ? ((int)Enumerador.enmBovedaTipo.USUARIO).ToString() : ((int)Enumerador.enmBovedaTipo.MISION).ToString();

            //-----------------------------------------
            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.Copy().DefaultView;
            //----------------------------------------------
            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;
            dvO.RowFilter = "OfConsularId=" + strOficConsularId + " and TipoBoveda=" + cboTipoBoveda.SelectedValue;

            cboBoveda.Items.Clear();
            cboBoveda.DataSource = dvO.ToTable();

            if (!bEsUsuario)
            {
                if (dvO.ToTable().Rows.Count > 0)
                    cboBoveda.SelectedValue = dvO.ToTable().Rows[0]["OfConsularId"].ToString();

                Util.CargarDropDownList(cboBoveda, dvO.ToTable(), "Descripcion", "OfConsularId", true);
                cboBoveda.SelectedValue = strOficConsularId;
            }
            else
            {
                if (dvO.ToTable().Rows.Count > 0)
                    cboBoveda.SelectedValue = dvO.ToTable().Rows[0]["IdTablaOrigenRefer"].ToString();

                Util.CargarDropDownList(cboBoveda, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
                cboBoveda.SelectedValue = strUsuarioId;
            }


        }

        private void CargarBoveda(DropDownList cboTipoBoveda, DropDownList cboBoveda, string strOficConsularId)
        {

            if (cboTipoBoveda.SelectedValue.ToString() != "0")
            {
                //-----------------------------------------
                DataTable dtBovedas = new DataTable();
                dtBovedas = obtenerBovedas();

                DataView dvO = dtBovedas.Copy().DefaultView;
                //----------------------------------------------
                
                //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;

                dvO.RowFilter = "OfConsularId=" + strOficConsularId + " and TipoBoveda=" + cboTipoBoveda.SelectedValue;


                if (cboTipoBoveda.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.MISION).ToString())
                {
                    Util.CargarDropDownList(cboBoveda, dvO.ToTable(), "Descripcion", "OfConsularId", true);
                    cboBoveda.SelectedIndex = 1;
                }
                else if (cboTipoBoveda.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.USUARIO).ToString())
                {
                    cboBoveda.Items.Clear();
                    cboBoveda.DataSource = dvO.ToTable();


                    if (dvO.ToTable().Rows.Count > 0)
                        cboBoveda.SelectedValue = dvO.ToTable().Rows[0]["IdTablaOrigenRefer"].ToString();


                    Util.CargarDropDownList(cboBoveda, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
                    cboBoveda.SelectedIndex = 0;
                }
            }
            else
            {
                cboBoveda.Items.Clear();
                cboBoveda.Items.Insert(0, new ListItem("- SELECCIONAR -"));
            }



        }

        private object[] ObtenerFiltro(bool bolVerEnGrilla = true)
        {
            object[] arrParametros = new object[7];

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);            

            arrParametros[0] = Int32.Parse(cboMisionConsular.SelectedValue.ToString());
            arrParametros[1] = Int32.Parse(cboTipoBovedaO.SelectedValue.ToString());

            // auditoría
            arrParametros[2] = Int32.Parse(cboBovedaO.SelectedValue.ToString());
            arrParametros[3] = Int32.Parse(cboInsumo.SelectedValue.ToString());
            arrParametros[4] = Int32.Parse(cboMotivo.SelectedValue.ToString());

            arrParametros[5] = Comun.FormatearFecha(dtpFecFin.Text);
            arrParametros[6] = Comun.FormatearFecha(dtpFecFin.Text);

            return arrParametros;
        }

        private void VerVistaPrevia()
        {
            // Limpiar Mensaje
            ctrlValidacion.MostrarValidacion("", false);

            DataSet ds = new DataSet();
            bool bolVistaPrevia = false;
            bool bolReporteSeleccionado = true;

            if (ddlTipoReporte.SelectedIndex > 0)
            {
                // Información del reporte
                object[] arrParametros = ObtenerFiltro(false);
                Session["SP_PARAMETROS"] = arrParametros;

                Session["AlmacenReporteId"] = Convert.ToInt32(ddlTipoReporte.SelectedValue);

                if (Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS ||
                    Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS_DETALLADO ||
                    Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.INSUMOS_REMITIDOS)
                {

                    ds = ObtenerDataTableReporte(arrParametros);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int intCantidadRegistros = ObtenerTotalRegistroDataSet(ds);
                            if (intCantidadRegistros > 0)
                            {
                                Session["dtDatos"] = ds.Tables[0];
                                bolVistaPrevia = true;
                                CargarReporte();
                            }

                            if (!bolVistaPrevia)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                            }
                        }
                    }

                }
            }
            else
            {
                bolReporteSeleccionado = false;
            }

            if (!bolReporteSeleccionado)
            {
                ctrlValidacion.MostrarValidacion("Seleccione Tipo de Reporte", true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        private void CargarReporte()
        {

            string strUrl = "../Almacen/frmReporteRS.aspx";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,fullscreen=yes');";

            Comun.EjecutarScript(Page, strScript);
        }

        private DataSet ObtenerDataTableReporte(object[] arrParametros)
        {
            //Proceso p = new Proceso();
            DataSet ds = new DataSet();
            SGAC.Almacen.Reportes.BL.ReporteAlmacenConsultasBL objReporteAlmacenConsultaBL = new SGAC.Almacen.Reportes.BL.ReporteAlmacenConsultasBL();

            int intTipoReporte = Convert.ToInt32(ddlTipoReporte.SelectedValue);
            Session["SP_PARAMETROS"] = arrParametros;
            switch (intTipoReporte)
            {
                case (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS:
                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.AL_INSUMO", "REPORTEINSUMO");

                    ds = objReporteAlmacenConsultaBL.ObtenerReporteInsumos(Int32.Parse(cboMisionConsular.SelectedValue.ToString()), Int32.Parse(cboTipoBovedaO.SelectedValue.ToString()),
                                                                            Int32.Parse(cboBovedaO.SelectedValue.ToString()), Int32.Parse(cboInsumo.SelectedValue.ToString()), Int32.Parse(cboMotivo.SelectedValue.ToString()),
                                                                            Comun.FormatearFecha(dtpFecInicio.Text), Comun.FormatearFecha(dtpFecFin.Text));

                    break;
                case (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS_DETALLADO:
                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.AL_INSUMO", "REPORTEINSUMOSDETALLE");

                    ds = objReporteAlmacenConsultaBL.ObtenerReporteInsumosDetallado(Int32.Parse(cboMisionConsular.SelectedValue.ToString()), Int32.Parse(cboTipoBovedaO.SelectedValue.ToString()),
                                                                            Int32.Parse(cboBovedaO.SelectedValue.ToString()), Int32.Parse(cboInsumo.SelectedValue.ToString()), Int32.Parse(cboMotivo.SelectedValue.ToString()),
                                                                            Comun.FormatearFecha(dtpFecInicio.Text), Comun.FormatearFecha(dtpFecFin.Text));

                    break;
                case (int)Enumerador.enmAlmacenReporte.INSUMOS_REMITIDOS:
                    //ds = (DataSet)p.Invocar(ref arrParametros, "SGAC.BE.AL_INSUMO", "REPORTEINSUMOSREMITIDOS");

                    ds = objReporteAlmacenConsultaBL.ObtenerReporteInsumosRemitidos(Int32.Parse(cboMisionConsular.SelectedValue.ToString()), Int32.Parse(cboTipoBovedaO.SelectedValue.ToString()),
                                                                            Int32.Parse(cboBovedaO.SelectedValue.ToString()), Int32.Parse(cboInsumo.SelectedValue.ToString()), Int32.Parse(cboMotivo.SelectedValue.ToString()),
                                                                            Comun.FormatearFecha(dtpFecInicio.Text), Comun.FormatearFecha(dtpFecFin.Text));

                    break; 
            }
            return ds;
        }
        private int ObtenerTotalRegistroDataSet(DataSet ds)
        {
            int intTotalRegistros = 0;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i] != null)
                {
                    intTotalRegistros += ds.Tables[i].Rows.Count;
                }
            }
            return intTotalRegistros;
        }
        #endregion

        protected void cboTipoBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarBoveda(cboTipoBovedaO, cboBovedaO, cboMisionConsular.SelectedValue.ToString());
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