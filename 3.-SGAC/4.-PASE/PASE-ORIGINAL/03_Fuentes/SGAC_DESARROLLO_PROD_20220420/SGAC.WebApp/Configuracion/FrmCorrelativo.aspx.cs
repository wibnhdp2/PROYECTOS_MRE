using System;
using System.Data;
using System.Web.UI;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace SGAC.WebApp.Configuracion
{
    public partial class FrmCorrelativo : MyBasePage
    {
        #region Variables
        private SI_TARIFARIO objTarifarioBE;
        private string strVariableTarifario = "objTarifarioBE";

        
        #endregion        

        #region Eventos
        protected void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            if (!Page.IsPostBack)
            {
                gvdTarifas.DataSource = new DataTable();
                gvdTarifas.DataBind();
                PageBarRegistro.InicializarPaginador();

                Session["CorrelativosxTarifario"] = null;

                hdn_sOficinaConsularId.Value = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

                CargarListadosDesplegables();

                ddlOficinaConsularConsulta.SelectedValue = hdn_sOficinaConsularId.Value;
                ddlOficinaConsularRegistro.SelectedValue = hdn_sOficinaConsularId.Value;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                btnGrabarCorrelativa.Visible = false;
            }
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarDatosGrilla();
            updConsulta.Update();
        }

        protected void ctrlPageBarRegistro_Click(object sender, EventArgs e)
        {
            CargarDatosGrillaRegistro();
            UpdatePanel1.Update();
        }

        private void CargarDatosGrillaRegistro()
        {
            ActualizarTarifaMemoria();

            int intOficinaConsularSel = 0;
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {                
                    intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsularRegistro.SelectedValue);
            }
            else
            {
                intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsularRegistro.SelectedValue);
            }

            int intPeriodo = Convert.ToInt32(ddlPeriodoRegistro.SelectedItem.Value);

            int intSeccion = Convert.ToInt32(ddlSeccion.SelectedValue);


            int intPaginaActual = PageBarRegistro.PaginaActual;
            int intPaginaCantidad = 1000;
            int intTotalRegistros = 0;
            int intTotalPaginas = 0;
            int intCorrelativo = 0;

            RegistroActuacionConsultasBL objBL = new RegistroActuacionConsultasBL();
            DataTable dt = objBL.obtenerCorrelativoTarifa(intOficinaConsularSel, intPeriodo, intSeccion, intPaginaActual, intPaginaCantidad,
                ref intTotalRegistros, ref intTotalPaginas, ref intCorrelativo);

            txtRGE.Text = intCorrelativo.ToString();
            if (dt.Rows.Count > 0)
                ActualizarTarifaGrilla(ref dt);

            //Session["DT_CORRELATIVOSREGISTRO"] = dt;
            gvdTarifas.DataSource = dt;
            gvdTarifas.DataBind();

            PageBarRegistro.TotalResgistros = intTotalRegistros;
            PageBarRegistro.TotalPaginas = intTotalPaginas;

            if (PageBarRegistro.TotalPaginas > 1)
            {
                PageBarRegistro.Visible = true;
            }
            else
            {
                PageBarRegistro.InicializarPaginador();
            }

            ActualizarTarifaMemoria();
            txtTOTAL.Text = Convert.ToString(SumaCorrelativos());
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();
            gdvCorrelativos.DataSource = null;
            gdvCorrelativos.DataBind();

            CargarDatosGrilla();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            Session["DT_CORRELATIVOS"] = new DataTable();
            gdvCorrelativos.DataSource = new DataTable();
            gdvCorrelativos.DataBind();
        }

       

        #endregion

        #region Métodos
        private void CargarListadosDesplegables()
        {
            ddlOficinaConsularConsulta.Cargar(false, false);
            Util.CargarComboAnios(ddlPeriodo, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;

            ddlOficinaConsularRegistro.Cargar(false, false);

            DataTable dtSecciones = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.SECCION);
            DataView dv = dtSecciones.DefaultView;
            dv.Sort = "id asc";
            DataTable dtSeccionesOrdenadas = dv.ToTable();

            Util.CargarParametroDropDownList(ddlSeccion, dtSeccionesOrdenadas, true, " - TODOS - ");
            Util.CargarComboAnios(ddlPeriodoRegistro, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodoRegistro.SelectedIndex = ddlPeriodoRegistro.Items.Count - 1;
        }

        public void CargarDatosGrilla()
        {
            int intOficinaConsularSel = 0;
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {                
                    intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue);
            }
            else
            {
                intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue);
            }

            int intPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            int intSeccion = 0;
            int intPaginaActual = ctrlPaginador.PaginaActual;
            int intPaginaCantidad = ctrlPaginador.PageSize;
            int intTotalRegistros = 0;
            int intTotalPaginas = 0;

            RegistroActuacionConsultasBL objBL = new RegistroActuacionConsultasBL();
            DataTable dtRegistroActuacion = objBL.obtener(intOficinaConsularSel, intPeriodo, intSeccion, intPaginaActual, intPaginaCantidad,
                ref intTotalRegistros, ref intTotalPaginas);

            if (dtRegistroActuacion.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
            }

            Session["DT_CORRELATIVOS"] = dtRegistroActuacion;
            gdvCorrelativos.DataSource = dtRegistroActuacion;
            gdvCorrelativos.DataBind();

            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;
            if (ctrlPaginador.TotalPaginas > 1)
            {
                ctrlPaginador.Visible = true;
            }
            else
            {
                ctrlPaginador.InicializarPaginador();
            }
        }



        private void CargarObjetoTarifario(DataTable dtTarifarioFiltrado, int intIndiceSeleccionado)
        {
            objTarifarioBE = new SI_TARIFARIO();

            objTarifarioBE.tari_sTarifarioId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTarifarioId"]);
            objTarifarioBE.tari_sSeccionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sSeccionId"]);
            objTarifarioBE.tari_sNumero = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sNumero"]);
            objTarifarioBE.tari_vLetra = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vLetra"].ToString();
            objTarifarioBE.tari_FCosto = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FCosto"]);
            objTarifarioBE.tari_vDescripcion = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcion"].ToString();
            objTarifarioBE.tari_vDescripcionCorta = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcionCorta"].ToString();

            objTarifarioBE.tari_sBasePercepcionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sBasePercepcionId"]);
            objTarifarioBE.tari_sCalculoTipoId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sCalculoTipoId"]);
            objTarifarioBE.tari_vCalculoFormula = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vCalculoFormula"].ToString();

            objTarifarioBE.tari_sTopeUnidadId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTopeUnidadId"]);
            objTarifarioBE.tari_ITopeCantidad = Comun.ToNullInt32(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_ITopeCantidad"]);

            if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"] != System.DBNull.Value)
            {
                objTarifarioBE.tari_FMontoExceso = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"]);
            }
            else
            {
                objTarifarioBE.tari_FMontoExceso = 0;
            }

            objTarifarioBE.tari_bTarifarioDependienteFlag = false;
            if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"] != null)
                if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"].ToString() != string.Empty)
                    objTarifarioBE.tari_bTarifarioDependienteFlag = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"]);

            objTarifarioBE.tari_bHabilitaCantidad = false;
            if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"] != null)
                if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"].ToString() != string.Empty)
                    objTarifarioBE.tari_bHabilitaCantidad = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"]);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Session["CorrelativosxTarifario"] = null;
            gvdTarifas.DataSource = new DataTable();
            gvdTarifas.DataBind();
            PageBarRegistro.InicializarPaginador();


            CargarDatosGrillaRegistro();
        }

        protected void gvdTarifas_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if(e.Row.RowIndex>=0)
            {

                string corr = e.Row.Cells[Util.ObtenerIndiceColumnaGrilla(gvdTarifas, "corr_ICorrelativo")].Text;

                System.Web.UI.WebControls.TextBox txt = e.Row.FindControl("txtCorrelativo") as System.Web.UI.WebControls.TextBox;
                txt.Text = corr;
            }
            
        }

        protected void btnGrabarCorrelativa_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt64(txtRGE.Text) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Participante", "alert('El RGE debe ser mayor a 0');", true); 
                return;
            }
            ActualizarTarifaMemoria();
            List<RE_CORRELATIVO> listCorrelativo = Session["CorrelativosxTarifario"] as List<RE_CORRELATIVO>;

            bool bSinDatos = false;
            if (listCorrelativo != null)
            {
                if (listCorrelativo.Count == 0)
                    bSinDatos = true;
            }
            else
            {
                bSinDatos = true;
            }

            if (bSinDatos)
            {
                Comun.EjecutarScriptUniqueIdDinamico(this.Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Correlativos de Actuaciones", "No existe información para almacenar."), "MensajeGrabacion");
                return;
            }

            RE_CORRELATIVO correlativo = new RE_CORRELATIVO();

            correlativo.corr_sOficinaConsularId = listCorrelativo[0].corr_sOficinaConsularId;
            correlativo.corr_sPeriodo = listCorrelativo[0].corr_sPeriodo;
            correlativo.corr_ICorrelativo = listCorrelativo[0].corr_ICorrelativo;
            correlativo.corr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
            correlativo.corr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

            string sRge = txtRGE.Text;
            if (sRge == string.Empty)
                correlativo.corr_ICorrelativo = 0;
            else
                correlativo.corr_ICorrelativo = Convert.ToInt32(sRge);

            listCorrelativo.Add(correlativo);

            ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            string result = oActuacionMantenimientoBL.ActualizarTarifarioCorrelativo(listCorrelativo);


            if (result == string.Empty)
            {
                Comun.EjecutarScriptUniqueIdDinamico(this.Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Correlativos de Actuaciones", Constantes.CONST_MENSAJE_EXITO), "MensajeGrabacion");
            }
            else
            {
                Comun.EjecutarScriptUniqueIdDinamico(this.Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Correlativos de Actuaciones", oActuacionMantenimientoBL.strMensajeError), "MensajeGrabacion");
            }

            
        }

        private int SumaCorrelativos()
        {
            int suma = 0;
            foreach (GridViewRow row in gvdTarifas.Rows)
            {
                TextBox txt = row.FindControl("txtCorrelativo") as TextBox;
                string sCorrelativo = txt.Text;

                if (txt.Text.Trim() == string.Empty)
                {sCorrelativo = "0";}

                suma = suma + Convert.ToInt32(sCorrelativo);
            }

            return suma;
        }
        private void ActualizarTarifaMemoria()
        {

            List<RE_CORRELATIVO> listCorrelativo = new List<RE_CORRELATIVO>();

            if (Session["CorrelativosxTarifario"] != null)
                listCorrelativo = Session["CorrelativosxTarifario"] as List<RE_CORRELATIVO>;


            foreach (GridViewRow row in gvdTarifas.Rows)
            {

                
                TextBox txt = row.FindControl("txtCorrelativo") as TextBox;
                string sCorrelativo = txt.Text;

                if (txt.Text.Trim() == string.Empty)
                    sCorrelativo = "0";


                RE_CORRELATIVO correlativo = new RE_CORRELATIVO();
                correlativo.corr_sTarifarioId = Convert.ToInt16(row.Cells[Util.ObtenerIndiceColumnaGrilla(gvdTarifas, "tari_sTarifarioId")].Text);
                correlativo.corr_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularRegistro.SelectedValue);
                correlativo.corr_sPeriodo = Convert.ToInt16(ddlPeriodo.SelectedItem.Value);
                correlativo.corr_ICorrelativo = Convert.ToInt16(sCorrelativo);
                correlativo.corr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                correlativo.corr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                bool bExiste = false;
                foreach (RE_CORRELATIVO corr in listCorrelativo)
                {
                    if (corr.corr_sTarifarioId == correlativo.corr_sTarifarioId)
                    {
                        corr.corr_ICorrelativo = correlativo.corr_ICorrelativo;
                        bExiste = true;
                        break;
                    }
                }

                if (!bExiste)
                {
                    listCorrelativo.Add(correlativo);

                }
            }

            Session["CorrelativosxTarifario"] = listCorrelativo;

        }



        private void ActualizarTarifaGrilla(ref DataTable dt)
        {
            List<RE_CORRELATIVO> listCorrelativo = new List<RE_CORRELATIVO>();

            if (Session["CorrelativosxTarifario"] != null)
                listCorrelativo = Session["CorrelativosxTarifario"] as List<RE_CORRELATIVO>;

            int iTarifarioId = 0;
            foreach (DataRow row in dt.Rows)
            {
                iTarifarioId = Convert.ToInt16(row["tari_sTarifarioId"].ToString());
                foreach (RE_CORRELATIVO corr in listCorrelativo)
                {
                    if (corr.corr_sTarifarioId == iTarifarioId)
                    {
                        row["corr_ICorrelativo"] = corr.corr_ICorrelativo;
                        break;
                    }
                }
            }

            var dtAux = dt.AsEnumerable().Where(x => x["tari_sTarifarioId"] != System.DBNull.Value);

            if (dtAux.AsDataView().Count > 0)
                dt = dt.AsEnumerable().Where(x => x["tari_sTarifarioId"] != System.DBNull.Value).CopyToDataTable();
            else
                dt = new DataTable();
        }


        #endregion

        #region WebMethod

        protected void btnSumarizar_Click(object sender, EventArgs e)
        {
            ActualizarTarifaMemoria();
            List<RE_CORRELATIVO> listCorrelativo = new List<RE_CORRELATIVO>();
            int iTotalCorrelativo = 0;

            if (System.Web.HttpContext.Current.Session["CorrelativosxTarifario"] != null)
                listCorrelativo = System.Web.HttpContext.Current.Session["CorrelativosxTarifario"] as List<RE_CORRELATIVO>;

            foreach (RE_CORRELATIVO corr in listCorrelativo)
            {
                iTotalCorrelativo += corr.corr_ICorrelativo;
            }

            txtRGE.Text=iTotalCorrelativo.ToString();

        }

        #endregion
    }
}