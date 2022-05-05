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
using SGAC.WebApp.Accesorios.SharedControls;
using System.Web.Configuration;
using SGAC.Almacen.BL;
using SGAC.BE;

namespace SGAC.WebApp.Almacen
{
    public partial class frmListadoInsumos : MyBasePage
    {
        InsumoConsultaBL oInsumoConsultaBL = new InsumoConsultaBL();

        #region CAMPOS

        private string strNombreEntidad = "LISTADO DE INSUMOS";

        #endregion

        #region VARIABLES
        public string strMesN;
        #endregion

        #region CAMPOS
        private string strVariableAccion = "Listado_Accion";
        private string strVariableTabla = "DTInsumo";
        private string strVariableInsumo = "Insumo_Id";
        #endregion

        private void LimpiarGrilla()
        {
            Session["dt"] = null;
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
            ctrlPaginador.Visible = false;
        }

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
            ctrlToolBar1.VisibleIButtonPrint = false;

            ctrlToolBar1.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar1_btnBuscarHandler);
            ctrlToolBar1.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar1_btnCancelarHandler);
            ctrlToolBar1.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBar1_btnPrintHandler);

            ctrlToolBar1.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBar1.btnCancelar.Text = "      Limpiar";

            ctrlToolBar1.btnBuscar.OnClientClick = "return ValidarBuscar();";
                

            //ceFecInicio.Format = ConfigurationManager.AppSettings["FormatoFechas"];
            //ceFecFin.Format = ConfigurationManager.AppSettings["FormatoFechas"];

            cboMisionConsO.AutoPostBack = true;
            cboMisionConsO.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

            // ADMINISTRADOR y SUPERADMIN
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.ADMINISTRATIVO &&
                Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.SUPERADMIN)
            {

            }

            
        
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();
            Session["Formatofecha"] = strFormatoFechas;

            if (!IsPostBack)
            {
                llenarBovedas();

                LimpiaCampos();

                lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);
                Util.CargarParametroDropDownList(cboTipoInsumo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true, " - TODOS - ");
                Util.CargarParametroDropDownList(cboEstado, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.INSUMO), true, " - TODOS - ");
         

                // Se carga la misión consular
                cboMisionConsO.Cargar(true);
                cboMisionConsO.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

                //Si el usuario es operativo, se setea los valores de mision tipo de boveda y boveda y se deshabilita para no ser modificado.
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue.ToString(), Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]).ToString(), true);


                    cboBovedaO.Enabled = false;
                    cboTipoBovedaO.Enabled = false;
                    cboMisionConsO.Enabled = false;
                }
                else
                {
                    CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue.ToString()); 
                }

                txtFecIniAct.Text = DateTime.Today.ToString(strFormatoFechasInicio);
                txtFecFinAct.Text = DateTime.Today.ToString(strFormatoFechas);

                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                Comun.SeleccionarItem(cboTipoInsumo, "AUTOADHESIVO");
                cboTipoInsumo.Enabled = false;

            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBar1.btnEditar, ctrlToolBar1.btnEliminar, ctrlToolBar1.btnGrabar, ctrlToolBar1.btnNuevo };
                Comun.ModoLectura(ref arrButtons);
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue.ToString()); 
        }

        void ctrlToolBar1_btnBuscarHandler()
        {
            if (chkOcultar.Checked)
            {
                DateTime datFechaInicio = Comun.FormatearFecha(txtFecIniAct.Text);
                DateTime datFechaFin = Comun.FormatearFecha(txtFecFinAct.Text);

                if (Comun.EsFecha(txtFecIniAct.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                    Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
                    return;
                }
                if (Comun.EsFecha(txtFecFinAct.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                    Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
                    return;
                }

                if (cboBovedaO.SelectedItem.Text == "- SELECCIONAR -" && cboTipoBovedaO.SelectedIndex == 1)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Se debe seleccionar la Bóveda"));
                    return;
                }

                if (datFechaInicio > datFechaFin)
                {
                    Session["dt"] = new DataTable();
                    grReporteGestion.DataSource = new DataTable();
                    grReporteGestion.DataBind();
                    LimpiaCampos();
                    grReporteGestion.DataSource = null;
                    grReporteGestion.DataBind();
                    ctrlPaginador.Visible = false;
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.WARNING);
                }
                else
                {
                    ctrlPaginador.InicializarPaginador();
                    CargarGrilla();
                }
            }
            else{
                if (cboBovedaO.SelectedItem.Text == "- SELECCIONAR -" && cboTipoBovedaO.SelectedIndex == 1)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Se debe seleccionar la Bóveda"));
                    return;
                }
                    ctrlPaginador.InicializarPaginador();
                    CargarGrilla();
            }
            Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
        }

        void ctrlToolBar1_btnCancelarHandler()
        {
            LimpiaCampos();
            grReporteGestion.DataSource = null;
            grReporteGestion.DataBind();
            ctrlPaginador.Visible = false;
            Comun.SeleccionarItem(cboTipoInsumo, "AUTOADHESIVO");
            cboTipoInsumo.Enabled = false;

            Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
        }

        void ctrlToolBar1_btnPrintHandler()
        {

        }

        private void LimpiaCampos()
        {
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            txtFecIniAct.Text = DateTime.Today.ToString(strFormatoFechasInicio);
            txtFecFinAct.Text = DateTime.Today.ToString(strFormatoFechas);
            //dtpFecInicio.Enabled = true;
            //dtpFecFin.Enabled = true;

            txtCodInsumoC.Text = "";
            txtNroMov.Text = "";
            lblTipoInsumoH.Text = "";
            lblCodUnicoH.Text = "";
            lblMovimientoH.Text = "";
            lblEstadoInsumoH.Text = "";
            LblFechaH.Text = "";
            lblUbicacion.Text = "";
            cboTipoInsumo.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                cboMisionConsO.Enabled = true;
            }
            else{
                cboMisionConsO.Enabled = false;
            }

            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            cboTipoBovedaO.SelectedIndex = 0;
            cboTipoBovedaO_SelectedIndexChanged(null, null);


            grdEstadoInsumo.DataSource = null;
            grdEstadoInsumo.DataBind();
            UpdMantenimiento.Update();
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        private void CargarGrilla()
        {
            Proceso p = new Proceso();
            int intOficinaConsularIdOrigen = Convert.ToInt32(cboMisionConsO.SelectedValue);
            int intEstado = 0;

            string insu_vMovimientoCod = "0";
            if (txtNroMov.Text == "")
                insu_vMovimientoCod = "0";
            else
                insu_vMovimientoCod = txtNroMov.Text;

            int intTipoInsumo = 0, intBovedaTipoIdOrigen = 0, intBodegaOrigenId = 0;
            intTipoInsumo = Convert.ToInt32(cboTipoInsumo.SelectedValue);


            
            if (cboEstado.SelectedValue != null)
                intEstado = Convert.ToInt32(cboEstado.SelectedValue);

            if (cboTipoBovedaO.SelectedValue != null)
                intBovedaTipoIdOrigen = Convert.ToInt32(cboTipoBovedaO.SelectedValue);
             
            if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
            {
                if (cboBovedaO.SelectedValue != null)
                    intBodegaOrigenId = intOficinaConsularIdOrigen;
            }
            else
            {
                if (cboBovedaO.SelectedValue != null) {
                    if (cboTipoBovedaO.SelectedIndex == 0)
                    { 
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "¡Seleccione tipo de Bóbeda! "));
                        return;
                    }
                    intBodegaOrigenId = Convert.ToInt32(cboBovedaO.SelectedValue);
                }
                    
            }


            if (intOficinaConsularIdOrigen > 0 && intBovedaTipoIdOrigen > 0 )//&& intBodegaOrigenId > 0)
            {
                int intTotalRegistros = 0, intTotalPaginas = 0;

                DateTime datFechaInicio = Comun.FormatearFecha(txtFecIniAct.Text);
                DateTime datFechaFin = Comun.FormatearFecha(txtFecFinAct.Text);

                DataTable dt = new DataTable();
                if (chkOcultar.Checked)
                {
                    dt = oInsumoConsultaBL.Consultar(intOficinaConsularIdOrigen,
                                           intBovedaTipoIdOrigen,
                                           intBodegaOrigenId,
                                           insu_vMovimientoCod,
                                           datFechaInicio,
                                           datFechaFin,

                                           ctrlPaginador.PaginaActual,
                                           Constantes.CONST_CANT_REGISTRO, 
                                           ref intTotalRegistros,
                                           ref intTotalPaginas,
                                           intTipoInsumo,
                                           txtCodInsumoC.Text.Trim(),
                                           intEstado);
                }
                else {
                    dt = oInsumoConsultaBL.ConsultarSinFecha(intOficinaConsularIdOrigen,
                                           intBovedaTipoIdOrigen,
                                           intBodegaOrigenId,
                                           insu_vMovimientoCod,
                                           ctrlPaginador.PaginaActual,
                                           Constantes.CONST_CANT_REGISTRO, 
                                           ref intTotalRegistros,
                                           ref intTotalPaginas,
                                           intTipoInsumo,
                                           txtCodInsumoC.Text.Trim(),
                                           intEstado);
                
                }

                //---------------------------------------------------------------------
                // Fecha: 02/12/2019
                // Autor: Miguel Márquez Beltrán
                // Motivo: Sesion no se utiliza y demanda mucho recurso de memoria.
                //---------------------------------------------------------------------
                //Session[strVariableTabla] = dt;

                grReporteGestion.DataSource = dt;
                grReporteGestion.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                ctrlPaginador.TotalResgistros = Convert.ToInt32(intTotalRegistros);
                ctrlPaginador.TotalPaginas = Convert.ToInt32(intTotalPaginas);

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        protected void grReporteGestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string KeyID = grReporteGestion.DataKeys[e.Row.RowIndex].Value.ToString();     
              
                System.Web.UI.WebControls.Image imagen = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgestado");
             
                switch (KeyID)
                {
                    case "DISPONIBLE":
                        imagen.ImageUrl = "../Images/img_16_status1.png";
                        break;
                    case "ASIGNADO":
                        imagen.ImageUrl = "../Images/img_16_status2.png";
                        break;
                    case "BAJA":
                        imagen.ImageUrl = "../Images/img_16_status3.png";
                        break;
                    case "ANULADO":
                        imagen.ImageUrl = "../Images/img_16_bajar.png"; 
                        break;
                    default:
                        imagen.ImageUrl = "../Images/img_16_statusN.png";
                        break;
                }

                string strFormatofecha = "";
                strFormatofecha = Convert.ToString(Session["Formatofecha"]);

                string Date = e.Row.Cells[6].Text;
                e.Row.Cells[6].Text = Comun.FormatearFecha(Date).ToString(strFormatofecha);



                ImageButton btnDesvincular = e.Row.FindControl("btnDesvincular") as ImageButton;
                if (btnDesvincular != null)
                    btnDesvincular.Visible = false;

               Int16 sEstado= Convert.ToInt16(e.Row.Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "insu_sEstadoId")].Text);

               if (sEstado == Convert.ToInt16(Enumerador.enmInsumoEstado.VINCULADO_ACTUACION))
               {
                   btnDesvincular.Visible = true;
                   
               }


               if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
               {
                   ImageButton[] arrImageButtons = { btnDesvincular };
                   Comun.ModoLectura(ref arrImageButtons);
               }
            }
        }

        protected void grReporteGestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intIndex = Convert.ToInt32(e.CommandArgument);


            int intSeleccionado = Convert.ToInt32(grReporteGestion.Rows[intIndex].Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "insu_iInsumoId")].Text);
            Session[strVariableInsumo] = intSeleccionado;

            if (intSeleccionado > -1)
            {
                if (e.CommandName == "Ver")
                {
                    int intCodigoPed = intIndex;
                    lblTipoInsumoH.Text = grReporteGestion.Rows[intCodigoPed].Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "Insumo")].Text;
                    lblCodUnicoH.Text = grReporteGestion.Rows[intCodigoPed].Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "insu_vCodigoUnicoFabrica")].Text;
                    lblUbicacion.Text = grReporteGestion.Rows[intCodigoPed].Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "Ubicacion")].Text;
                    TraeHistoricoInsumo(intSeleccionado,intCodigoPed);
                    UpdMantenimiento.Update();
                }
                else if (e.CommandName == "Desvincular")
                {
                    if (validarDesvincularFechaRegistro(intSeleccionado) == true)
                    {
                        //------------------------------------------------------------------------
                        // Autor: Jonatan Silva Cachay
                        // Fecha: 16/02/2017
                        // Objetivo: Se cambia el Popup Formulario por un modal html 
                        // y se reemplaza la session por un valor que ejecuta el javascript
                        //------------------------------------------------------------------------
                        
                        //Session["obj_insumo_id"] = intSeleccionado;
                        Comun.EjecutarScript(this, "Popup(" + intSeleccionado.ToString() + ");");
                    }
                }

            }
        }

        private void TraeHistoricoInsumo(int intSeleccionado, int intCodigoPed)
        {
            Proceso p = new Proceso();
            int insu_iInsumoId = Convert.ToInt32(grReporteGestion.Rows[intCodigoPed].Cells[Util.ObtenerIndiceColumnaGrilla(grReporteGestion, "insu_iInsumoId")].Text);

            string strMovimientoCodigo="";
           
            
            DataTable dt = oInsumoConsultaBL.ConsultarHistorico(insu_iInsumoId, ref strMovimientoCodigo);

            grdEstadoInsumo.DataSource = dt;
            grdEstadoInsumo.DataBind();

            lblMovimientoH.Text = Convert.ToString(strMovimientoCodigo);

            if (dt.Rows.Count > 0)
            {
                
                DateTime datFechaInicio = Comun.FormatearFecha(grdEstadoInsumo.Rows[0].Cells[2].Text);
                
                LblFechaH.Text = (Convert.ToDateTime(datFechaInicio)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]) + " " + grdEstadoInsumo.Rows[0].Cells[3].Text;

                lblEstadoInsumoH.Text = grdEstadoInsumo.Rows[0].Cells[5].Text;
            }
            string script = Util.HabilitarTab(1);
            Comun.EjecutarScript(Page, script);
        }

        protected void grdEstadoInsumo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[2].Text = (Comun.FormatearFecha(e.Row.Cells[2].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
            }
        }

        protected void cboTipoBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarGrilla();

            CargarBoveda(cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue.ToString());
           

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;

            Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
        }

        protected void cboBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarGrilla();
            Comun.EjecutarScript(Page, "OcultarMostrarFechas();");
        }

        private void CargarOficinaConsular(ctrlOficinaConsular cboOficConsular, DropDownList cboTipoBoveda, DropDownList cboBoveda, string strOficConsularId, string strUsuarioId = "", bool bEsUsuario = false)
        {

            if (cboTipoBoveda.Items.Count == 0)
            {
                Util.CargarParametroDropDownList(cboTipoBoveda, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));
                cboTipoBovedaO.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            }

            cboOficConsular.SelectedValue = strOficConsularId;

            cboTipoBoveda.SelectedValue = bEsUsuario ? ((int)Enumerador.enmBovedaTipo.USUARIO).ToString() : ((int)Enumerador.enmBovedaTipo.MISION).ToString();

            //-----------------------------------------------
            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.Copy().DefaultView;
            //-----------------------------------------------
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
                //-----------------------------------------------
                DataTable dtBovedas = new DataTable();
                dtBovedas = obtenerBovedas();

                DataView dvO = dtBovedas.Copy().DefaultView;
                //-----------------------------------------------

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


                    Util.CargarDropDownList(cboBoveda, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true, " - TODOS - ");
                    cboBoveda.SelectedIndex = 0;
                }
            }
            else
            {
                cboBoveda.Items.Clear();
                cboBoveda.Items.Insert(0, new ListItem("- SELECCIONAR -"));
            }
        }

        protected void btnEjecutar_Click(object sender, EventArgs e)
        {
            Int64 intInsumoId = 0;
            int intBaja = 0;
            if (Session["baja_accion"] != null)
            {
                intBaja = Convert.ToInt32(Session["baja_accion"]);
                Session.Remove("baja_accion");
            }

            if (Session["obj_insumo_id"] != null)
            {
                intInsumoId = Convert.ToInt64(Session["obj_insumo_id"]);
                Session.Remove("obj_insumo_id");
            }

            if (intBaja == 1)
            {
                try
                {
                    AL_INSUMO oAL_INSUMO = new AL_INSUMO();
                    oAL_INSUMO.insu_iInsumoId = intInsumoId;
                    oAL_INSUMO.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    oAL_INSUMO.insu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    
                    InsumoMantenimientoBL oInsumoMantenimientoBL = new InsumoMantenimientoBL();
                    oInsumoMantenimientoBL.InsumoDarDeBaja(oAL_INSUMO);

                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "El insumo ha sido dado de baja satisfactoriamente."));

                    ctrlToolBar1_btnBuscarHandler();
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

        bool validarDesvincularFechaRegistro(int intInsumoId)
        {
            bool bConforme = true;
            string strFechaRegistro = "";
            
            DataTable dt = oInsumoConsultaBL.ConsultarFechaRegistro_por_IdInsumo(intInsumoId);
            
            if (dt.Rows.Count > 0)
            {
                strFechaRegistro = dt.Rows[0]["ACTU_DFECHAREGISTRO"].ToString();

                bConforme = CalcularDiasHabilesModificacion(strFechaRegistro);
            }

            return bConforme;
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 18/08/2016
        // Objetivo: Calcular los dias habiles permitidos para la anulación
        //           según sea Jeafatura o Consulado.
        // Referencia: Requerimiento No.001_3.doc
        //------------------------------------------------------------------------
        bool CalcularDiasHabilesModificacion(string strFechaRegistro)
        {
            bool bpermitir = true;

            DateTime dFecRegistro = Comun.FormatearFecha(strFechaRegistro);
            DateTime dFecActual = Comun.FormatearFecha(Comun.ObtenerFechaActualTexto(Session));

            int intEsJefatura = Convert.ToInt32(Session[Constantes.CONST_SESION_JEFATURA_FLAG]);
            string strDiasHabiles = "";
            string strMsjHabiles = "";

            if (intEsJefatura == 1)
            {
                strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesJefatura"].ToString();
            }
            else
            {
                strDiasHabiles = ConfigurationManager.AppSettings["sDiasActuacionesHabilesConsulado"].ToString();
            }
            strMsjHabiles = ConfigurationManager.AppSettings["vMsjDiasActuacionHabiles"].ToString();

            int intDiasHabiles = Convert.ToInt32(strDiasHabiles);

            int intMesUltimaFecha = dFecRegistro.Month;
            int intAnioUltimaFecha = dFecRegistro.Year;

            if (intMesUltimaFecha == 12)
            {
                intMesUltimaFecha = 1;
                intAnioUltimaFecha = intAnioUltimaFecha + 1;
            }
            else
            {
                intMesUltimaFecha = intMesUltimaFecha + 1;
            }

            DateTime dFecUltima = new DateTime(intAnioUltimaFecha, intMesUltimaFecha, intDiasHabiles);

            if (dFecUltima < dFecActual)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", strMsjHabiles));
                bpermitir = false;
            }
            return bpermitir;
            //------------------------------------------------------------------------

        }
        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 16/02/2017
        // Objetivo: Ejecuta el Boton del Popup para anular el insumo
        //------------------------------------------------------------------------
        protected void BtnAceptarBaja_Click(object sender, EventArgs e)
        {
            Int64 intInsumoId = 0;

            if (hInsumoID.Value != "")
            {
                intInsumoId = Convert.ToInt32(hInsumoID.Value);
            }

            try
            {
                AL_INSUMO oAL_INSUMO = new AL_INSUMO();
                oAL_INSUMO.insu_iInsumoId = intInsumoId;
                oAL_INSUMO.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                oAL_INSUMO.insu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                oAL_INSUMO.insu_vMotivoBaja = txtMotivo.Text;
                InsumoMantenimientoBL oInsumoMantenimientoBL = new InsumoMantenimientoBL();
                oInsumoMantenimientoBL.InsumoDarDeBaja(oAL_INSUMO);

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "El insumo ha sido dado de baja satisfactoriamente."));

                ctrlToolBar1_btnBuscarHandler();
                txtMotivo.Text = "";
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

        //protected void chkOcultar_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkOcultar.Checked)
        //    {
        //        Fechas.Visible = true;
        //    }
        //    else {
        //        Fechas.Visible = false;
        //    }
            
        //}


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