using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Configuration;
using System.Data;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;
using SGAC.BE.MRE;

//--------------------------------------------//
// Fecha: 04/01/2017
// Autor: Miguel Márquez Beltrán 
// Objetivo: Registrar la Guía de Despacho
//--------------------------------------------//
namespace SGAC.WebApp.Registro
{
    public partial class frmGuiaDespacho : MyBasePage
    {
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginadorGuia.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorGuia.Visible = false;
            ctrlPaginadorGuia.PaginaActual = 1;


            ctrlPaginadorListaFichasEnviadas.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorListaFichasEnviadas.Visible = false;
            ctrlPaginadorListaFichasEnviadas.PaginaActual = 1;
        }
               
        protected void Page_Load(object sender, EventArgs e)
        {
            
           // Comun.CargarPermisos(Session, ctrlToolBarGuiaDespacho, ctrlToolBarMantenimientoGuiaDespacho, grdGuiaDespacho, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            ctrlToolBarGuiaDespacho.VisibleIButtonBuscar = true;
            ctrlToolBarGuiaDespacho.VisibleIButtonCancelar = true;
            ctrlToolBarGuiaDespacho.VisibleIButtonSalir = true;

            ctrlToolBarGuiaDespacho.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarGuiaDespacho_btnBuscarHandler);
            ctrlToolBarGuiaDespacho.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarGuiaDespacho_btnCancelarHandler);
            ctrlToolBarGuiaDespacho.btnSalirHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonSalirClick(ctrlToolBarGuiaDespacho_btnSalirHandler);

            ctrlToolBarConsultarFichas.VisibleIButtonBuscar = true;
            ctrlToolBarConsultarFichas.VisibleIButtonCancelar = true;
            ctrlToolBarConsultarFichas.VisibleIButtonSalir = true;

            ctrlToolBarConsultarFichas.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsultarFichas_btnBuscarHandler);
            ctrlToolBarConsultarFichas.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsultarFichas_btnCancelarHandler);
            ctrlToolBarConsultarFichas.btnSalirHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonSalirClick(ctrlToolBarConsultarFichas_btnSalirHandler);

            ctrlToolBarMantenimientoGuiaDespacho.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimientoGuiaDespacho.VisibleIButtonEditar = true;
            ctrlToolBarMantenimientoGuiaDespacho.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimientoGuiaDespacho.VisibleIButtonCancelar = true;

            ctrlToolBarMantenimientoGuiaDespacho.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimientoGuiaDespacho_btnNuevoHandler);
            ctrlToolBarMantenimientoGuiaDespacho.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimientoGuiaDespacho_btnEditarHandler);
            ctrlToolBarMantenimientoGuiaDespacho.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimientoGuiaDespacho_btnGrabarHandler);
            ctrlToolBarMantenimientoGuiaDespacho.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimientoGuiaDespacho_btnCancelarHandler);

            if (!Page.IsPostBack)
            {
                //Inicializar opciones 

                ctrlToolBarMantenimientoGuiaDespacho.btnNuevo.Enabled = true;
                ctrlToolBarMantenimientoGuiaDespacho.btnEditar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnGrabar.Enabled = true;
                ctrlToolBarMantenimientoGuiaDespacho.btnCancelar.Enabled = true;


                ctrlToolBarConsultarFichas.btnBuscar.Enabled = true;
                ctrlToolBarConsultarFichas.btnCancelar.Enabled = true;             
   

                ctrlToolBarGuiaDespacho.btnBuscar.Enabled = true;
                ctrlToolBarGuiaDespacho.btnCancelar.Enabled = true;
                ddl_EstadoGuia.Enabled = false;

                EstablecerEstadoFichaSeleccionada();

                DataTable dtEstadoGuia = new DataTable();

//                dtEstadoGuia = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_GUIA_DESPACHO_ESTADO);
                dtEstadoGuia = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_GUIA_DESPACHO_ESTADO);

                DataView dvEstadoGuia = dtEstadoGuia.DefaultView;
                DataTable dtEstadoguiaOrdenado = dvEstadoGuia.ToTable();
                dtEstadoguiaOrdenado.DefaultView.Sort = "Id ASC";
                Util.CargarParametroDropDownList(ddl_EstadoGuiaSel, dtEstadoguiaOrdenado, true);

                Util.CargarParametroDropDownList(ddl_EstadoGuia, dtEstadoguiaOrdenado);

                dtEstadoGuia.Dispose();

                DataTable dtTipoEnvio = new DataTable();

                dtTipoEnvio = comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Constantes.CONST_RENIEC_TIPO_ENVIO);

                Util.CargarParametroRadioButtonList(RBListTipoEnvioSelGuia, dtTipoEnvio);
                if (RBListTipoEnvioSelGuia.Items.Count > 0)
                { RBListTipoEnvioSelGuia.Items[0].Selected = true; }
                Util.CargarParametroRadioButtonList(RBListTipoEnvio, dtTipoEnvio);
                if (RBListTipoEnvio.Items.Count > 0)
                { RBListTipoEnvio.Items[0].Selected = true; }

                dtTipoEnvio.Dispose(); 

                DataTable dtTarifa = new DataTable();
                //TarifarioConsultasBL objTarifarioBL = new TarifarioConsultasBL();

                dtTarifa = Comun.ObtenerTarifarioCargaInicial(Session);

                //if (Session[Constantes.CONST_SESION_DT_TARIFARIO] != null)
                if (dtTarifa != null)
                {
                    //dtTarifa = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];

                    DataView dv = dtTarifa.DefaultView;
                    dv.RowFilter = " tari_sSeccionId = 8";
                    DataTable dtTarifaSeccion8 = dv.ToTable();

                    Util.CargarDropDownList(ddl_TipoTramiteSelFicha, dtTarifaSeccion8, "tari_vDescripcionCorta", "tari_sTarifarioId", true);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddl_TipoTramiteSelFicha, new DataTable(), true);
                }
                pnlGuiaDespacho.Visible = false;
                pnlFichasEnviadas.Visible = false;
                btnSeleccionarFichas.Visible = false;
                lblEtiquetaGuiaDespacho.Visible = false;
                lblNroGuiaDespacho.Visible = false;
                txtGuiaDespachoSelGuia.Focus();
            }

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimientoGuiaDespacho.btnNuevo, ctrlToolBarMantenimientoGuiaDespacho.btnGrabar, ctrlToolBarMantenimientoGuiaDespacho.btnEliminar };
                Comun.ModoLectura(ref arrButtons);
            }
        }
                                    
        #region FichasEnviadas

        protected void gdvFichasEnviadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long intFichaEnviadaId = 0;

            if (e.CommandName == "Anular")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblFichaEnviadaId = (Label)gvrModificar.FindControl("lblFichaEnviadaId");

                intFichaEnviadaId = Convert.ToInt64(lblFichaEnviadaId.Text);

                AnularFichaEnviada(intFichaEnviadaId);
            }
        }

        private void consultarFichasEnviadas(long intGuiaDespachoId)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
            int PaginaActual = ctrlPaginadorListaFichasEnviadas.PaginaActual;
            long intFichaEnviadaId = 0;
            long intFichaRegistralId = 0;

            DataTable dt = new DataTable();

            FichaEnviadaBL objFichaEnviadaBL = new FichaEnviadaBL();

            dt = objFichaEnviadaBL.Consultar(intFichaEnviadaId, intGuiaDespachoId, intFichaRegistralId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dt.Rows.Count > 0)
            {
                ctrlPaginadorListaFichasEnviadas.TotalResgistros = IntTotalCount;
                ctrlPaginadorListaFichasEnviadas.TotalPaginas = IntTotalPages;

                ctrlPaginadorListaFichasEnviadas.Visible = false;

                if (ctrlPaginadorListaFichasEnviadas.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginadorListaFichasEnviadas.Visible = true;
                }
                pnlFichasEnviadas.Visible = true;
            }
            else
            {
                ctrlPaginadorListaFichasEnviadas.Visible = false;
                pnlFichasEnviadas.Visible = false;
            }            

            gdvFichasEnviadas.DataSource = dt;
            gdvFichasEnviadas.DataBind();
            updConsulta.Update();
        }

        protected void ctrlPaginadorListaFichasEnviadas_Click(object sender, EventArgs e)
        {
            long intGuiaDespachoId = Convert.ToInt64(HFGuiaDespachoId.Value);

            consultarFichasEnviadas(intGuiaDespachoId);
        }
      
        #endregion

        #region BusquedaGuiaDespacho
        private void EstablecerEstadoFichaSeleccionada()
        {
            ddlEstadoFichaSelFicha.Items.Clear();

            DataTable dtEstado = new DataTable();
            //dtEstado = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);
            dtEstado = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);

            string strtexto = "";
            string strvalue = "";
            
            ListItem listaItems = new ListItem();
            for (int i = 0; i < dtEstado.Rows.Count; i++)
            {
                strtexto = dtEstado.Rows[i]["VALOR"].ToString().Trim();
                strvalue = dtEstado.Rows[i]["ID"].ToString().Trim();
                if (dtEstado.Rows[i]["VALOR"].ToString().Trim() != "INCOMPLETO" && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "ANULADO" && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "ENVIADO" && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "OBSERVADO"
                    && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "RECUPERADO CON FICHA"
                    && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "RECUPERADO - ENVIADO")
                {
                    listaItems = new ListItem(strtexto, strvalue);
                    ddlEstadoFichaSelFicha.Items.Add(listaItems);
                }
            }
            ddlEstadoFichaSelFicha.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
        }
        private bool validarBusquedaGuiaDespacho()
        {
            bool bolEscorrecto = true;

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            
            if (txtFechaInicioSelGuia.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaInicioSelGuia.Text.Trim()) == false)
                {
                    ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                    return false;
                }
                datFechaInicio = Comun.FormatearFecha(txtFechaInicioSelGuia.Text);
            }
            
            if (txtFechaFinSelGuia.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaFinSelGuia.Text.Trim()) == false)
                {
                    ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                    return false;
                }
                datFechaFin = Comun.FormatearFecha(txtFechaFinSelGuia.Text);
            }
            if (txtFechaInicioSelGuia.Text.Trim().Length > 10 && txtFechaFinSelGuia.Text.Trim().Length > 10)
            {
                if (datFechaInicio > datFechaFin)
                {
                    ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                }
            }
            return bolEscorrecto;
        }

        void ctrlToolBarGuiaDespacho_btnBuscarHandler()
        {
            if (validarBusquedaGuiaDespacho())
            {
                long intGuiaDespachoId = 0;
                string strGuiaDespacho = txtGuiaDespachoSelGuia.Text.Trim();
                string strFechaInicio = "";
                string strFechaFinal = "";
                Int16 intTipoEnvioId = Convert.ToInt16(RBListTipoEnvioSelGuia.SelectedValue);
                string strNroHoja = txtNrotHojaSelGuia.Text.Trim();
                string strNombreEmpresa = txtNombreEmpresaSelGuia.Text.Trim().ToUpper();
                string strGuiaAerea = txtGuiaAereaSelGuia.Text.Trim();
                Int16 intEstadoGuia = 0;
                int intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
                int PaginaActual = ctrlPaginadorGuia.PaginaActual;


                if (txtFechaInicioSelGuia.Text.Trim().Length > 10)
                {
                    strFechaInicio = Comun.FormatearFecha(txtFechaInicioSelGuia.Text).ToString("yyyyMMdd");
                }
                if (txtFechaFinSelGuia.Text.Trim().Length > 10)
                {
                    strFechaFinal = Comun.FormatearFecha(txtFechaFinSelGuia.Text).ToString("yyyyMMdd");
                }

                if (ddl_EstadoGuiaSel.SelectedIndex > 0)
                {
                    intEstadoGuia = Convert.ToInt16(ddl_EstadoGuiaSel.SelectedValue);
                }
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    intOficinaConsularId = 0;
                }

                DataTable dt = new DataTable();
                GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();

                dt = objGuiaDespachoBL.Consultar(intOficinaConsularId, intGuiaDespachoId, strFechaInicio, strFechaFinal, intTipoEnvioId, strNombreEmpresa,
                                                strGuiaAerea, strNroHoja, strGuiaDespacho, intEstadoGuia, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

                if (dt.Rows.Count > 0)
                {
                    ctrlPaginadorGuia.TotalResgistros = IntTotalCount;
                    ctrlPaginadorGuia.TotalPaginas = IntTotalPages;

                    ctrlPaginadorGuia.Visible = false;

                     if (ctrlPaginadorGuia.TotalResgistros > intPaginaCantidad)
                        {
                            ctrlPaginadorGuia.Visible = true;
                        }
                     pnlGuiaDespacho.Visible = true;
                     lblListaGuiaDespacho.Visible = true;
                }
                else
                {
                    ctrlPaginadorGuia.Visible = false;
                    pnlGuiaDespacho.Visible = false;
                    lblListaGuiaDespacho.Visible = false;
                }
                btnSeleccionarFichas.Visible = false;
                grdGuiaDespacho.DataSource = dt;
                grdGuiaDespacho.DataBind();
                updConsulta.Update();
            }
        }
        
        void ctrlToolBarGuiaDespacho_btnCancelarHandler()
        {
            txtGuiaDespachoSelGuia.Text = "";
            txtFechaInicioSelGuia.Text = "";
            txtFechaFinSelGuia.Text = "";
            txtNrotHojaSelGuia.Text = "";
            txtNombreEmpresaSelGuia.Text = "";
            txtGuiaAereaSelGuia.Text = "";
            ddl_EstadoGuiaSel.SelectedIndex = 0;
            grdGuiaDespacho.DataSource = null;
            grdGuiaDespacho.DataBind();
            lblListaGuiaDespacho.Visible = false;
            ctrlPaginadorGuia.InicializarPaginador();
        }

        private void consultarGuiaDespacho(long intGuiaDespachoId)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = 1;
            int intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                intOficinaConsularId = 0;
            }

            DataTable dt = new DataTable();
            GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();

            dt = objGuiaDespachoBL.Consultar(intOficinaConsularId, intGuiaDespachoId, "", "", 0, "", "", "", "", 0, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["DFECHAENVIO"] != null)
                {
                    string strFechaEnvioGuia = dt.Rows[0]["DFECHAENVIO"].ToString().Trim();

                    txtFechaEnvioGuia.Text = Comun.FormatearFecha(strFechaEnvioGuia).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
                else
                {
                    txtFechaEnvioGuia.Text = "";
                }
                txtGuiaDespacho.Text = dt.Rows[0]["VNUMEROGUIADESPACHO"].ToString().Trim();
                txtNombreEmpresa.Text = dt.Rows[0]["VNOMBREEMPRESAENVIO"].ToString().Trim();
                txtNroHoja.Text = dt.Rows[0]["VNUMEROHOJA"].ToString().Trim();
                txtGuiaAerea.Text = dt.Rows[0]["VGUIAAEREA"].ToString().Trim();
                string strEstadoGuiaId = dt.Rows[0]["SESTADOGUIA"].ToString().Trim();

                ddl_EstadoGuia.SelectedValue = strEstadoGuiaId;

                string strTipoEnvioId = dt.Rows[0]["STIPOENVIOID"].ToString().Trim();

                for (int i = 0; i < RBListTipoEnvio.Items.Count; i++)
                {
                    RBListTipoEnvio.Items[i].Selected = false;
                }

                for (int i = 0; i < RBListTipoEnvio.Items.Count; i++)
                {
                    if (RBListTipoEnvio.Items[i].Value.Equals(strTipoEnvioId))
                    {
                        RBListTipoEnvio.Items[i].Selected = true;
                        break;
                    }
                }
                updMantenimiento.Update();
            }
        }

        protected void grdGuiaDespacho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long intGuiaDespachoId = 0;

            if (e.CommandName == "Seleccion")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblGuiaDespachoId = (Label)gvrModificar.FindControl("lblGuiaDespachoId");

                intGuiaDespachoId = Convert.ToInt64(lblGuiaDespachoId.Text.Trim());
                HFGuiaDespachoId.Value = lblGuiaDespachoId.Text.Trim();

                Label lblNumeroGuiaDespacho = (Label)gvrModificar.FindControl("lblNumeroGuiaDespacho");

                Label lblEstadoGuia = (Label)gvrModificar.FindControl("lblEstado");

                lblNroGuiaDespacho.Text = lblNumeroGuiaDespacho.Text.Trim();

                string strEstado = lblEstadoGuia.Text;

                if (strEstado == "ENVIADO")
                {
                    btnSeleccionarFichas.Enabled = false;
                    gdvFichasEnviadas.Enabled = false;
                }
                else { btnSeleccionarFichas.Enabled = true;
                        gdvFichasEnviadas.Enabled = true;
                }

                consultarFichasEnviadas(intGuiaDespachoId);
                ctrlToolBarMantenimientoGuiaDespacho.btnNuevo.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnEditar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnEliminar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnGrabar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnCancelar.Enabled = true;

                btnSeleccionarFichas.Visible = true;
                lblEtiquetaGuiaDespacho.Visible = true;
                lblNroGuiaDespacho.Visible = true;
            }
            if (e.CommandName == "Editar")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblGuiaDespachoId = (Label)gvrModificar.FindControl("lblGuiaDespachoId");

                intGuiaDespachoId = Convert.ToInt64(lblGuiaDespachoId.Text.Trim());
                HFGuiaDespachoId.Value = lblGuiaDespachoId.Text.Trim();

                consultarGuiaDespacho(intGuiaDespachoId);
                ctrlToolBarMantenimientoGuiaDespacho.btnNuevo.Enabled = true;
                ctrlToolBarMantenimientoGuiaDespacho.btnEditar.Enabled = true;
                ctrlToolBarMantenimientoGuiaDespacho.btnEliminar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnGrabar.Enabled = false;
                ctrlToolBarMantenimientoGuiaDespacho.btnCancelar.Enabled = true;
                pnlFichasEnviadas.Visible = false;
                btnSeleccionarFichas.Visible = false;
                lblEtiquetaGuiaDespacho.Visible = false;
                lblNroGuiaDespacho.Visible = false;
                ddl_EstadoGuia.Enabled = true;
                string strScript = string.Empty;
                strScript += Util.HabilitarTab(1);
                Comun.EjecutarScript(Page, strScript);
                
            }
            if (e.CommandName == "Anular")
            {
                GridViewRow gvrModificar;
                gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                Label lblGuiaDespachoId = (Label)gvrModificar.FindControl("lblGuiaDespachoId");

                intGuiaDespachoId = Convert.ToInt64(lblGuiaDespachoId.Text.Trim());
                pnlFichasEnviadas.Visible = false;
                btnSeleccionarFichas.Visible = false;
                lblEtiquetaGuiaDespacho.Visible = false;
                lblNroGuiaDespacho.Visible = false;

                AnularGuiaDespacho(intGuiaDespachoId);
                
            }

        }

        protected void ctrlPageBarGuiaDespacho_Click(object sender, EventArgs e)
        {
            ctrlToolBarGuiaDespacho_btnBuscarHandler();
        }

        #endregion

        #region FichasPorEnviar

        private bool validarBusquedaFichas()
        {
            bool bolEsCorrecto = true;

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (txtFechaInicioSelFicha.Text.Trim().Length > 10)
            {
                datFechaInicio = Comun.FormatearFecha(txtFechaInicioSelFicha.Text);
            }
            if (txtFechaFinSelFicha.Text.Trim().Length > 10)
            {
                datFechaFin = Comun.FormatearFecha(txtFechaFinSelFicha.Text);
            }
            if (txtFechaInicioSelFicha.Text.Trim().Length > 10 && txtFechaFinSelFicha.Text.Trim().Length > 10)
            {
                if (datFechaInicio > datFechaFin)
                {
                    ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                }
            }
            return bolEsCorrecto;
        }

        void ctrlToolBarConsultarFichas_btnBuscarHandler()
        {
            if (!validarBusquedaFichas())
            { return; }

            long intFichaRegistralId = 0;
            Int16 intTipoTramite = 0;
            string strNroFichaRegistralSelFicha = txtNroFichaRegistralSelFicha.Text.Trim();
            string strFechaInicio = "";
            string strFechaFinal = "";
            Int16 intEstadoFichaSelFicha = 0;
            int intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = 9000;
            int PaginaActual = 1;

            if (ddl_TipoTramiteSelFicha.SelectedIndex > 0)
            {
                intTipoTramite = Convert.ToInt16(ddl_TipoTramiteSelFicha.SelectedValue);
            }

            if (txtFechaInicioSelFicha.Text.Trim().Length > 10)
            {
                strFechaInicio = Comun.FormatearFecha(txtFechaInicioSelFicha.Text).ToString("yyyyMMdd");
            }
            if (txtFechaFinSelFicha.Text.Trim().Length > 10)
            {
                strFechaFinal = Comun.FormatearFecha(txtFechaFinSelFicha.Text).ToString("yyyyMMdd");
            }

            if (ddlEstadoFichaSelFicha.SelectedIndex > 0)
            {
                intEstadoFichaSelFicha = Convert.ToInt16(ddlEstadoFichaSelFicha.SelectedValue);
            }

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                intOficinaConsularId = 0;
            }

            if ((strFechaFinal != "" && strFechaInicio == "") || strFechaInicio != "" && strFechaFinal == "")
            {
                ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                return;
            }
            DataTable dt = new DataTable();
            FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();

            dt = objFichaRegistralBL.ConsultarFichasPorEnviar(intOficinaConsularId, intFichaRegistralId, intTipoTramite, strNroFichaRegistralSelFicha, intEstadoFichaSelFicha,
                                                             strFechaInicio, strFechaFinal, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dt.Rows.Count > 0)
            {                
                lblListaFichasPorEnviar.Visible = true;
                lblText.Visible = true;
                lblCantidad.Visible = true;
                btnAdicionarFichas.Visible = true;
                btnAdicionarFichas.Enabled = false;
            }
            else
            {                
                lblListaFichasPorEnviar.Visible = false;
                lblText.Visible = false;
                lblCantidad.Visible = false;
                btnAdicionarFichas.Visible = false;
            }
            Session["FichasPorEnviar"] = dt;
            gdvFichas.DataSource = dt;
            gdvFichas.DataBind();
        }
        
        void ctrlToolBarConsultarFichas_btnCancelarHandler()
        {
            ddl_TipoTramiteSelFicha.SelectedIndex = 0;
            txtNroFichaRegistralSelFicha.Text = "";
            txtFechaInicioSelFicha.Text = "";
            txtFechaFinSelFicha.Text = "";
            ddlEstadoFichaSelFicha.SelectedIndex = 0;
            Session["FichasPorEnviar"] = null;
            gdvFichas.DataSource = null;
            gdvFichas.DataBind();
            btnAdicionarFichas.Visible = false;
            lblListaFichasPorEnviar.Visible = false;
            lblText.Visible = false;
            lblCantidad.Visible = false;
        }

        

        private bool validarAdicionarFichas()
        {
            bool bolEsCorrecto = true;

            if (HFGuiaDespachoId.Value.Equals("0"))
            {
                ctrlValidacionFichas.MostrarValidacion("Seleccione una Guía de Despacho", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }

            if (Session["FichasPorEnviar"] == null)
            {
                ctrlValidacionFichas.MostrarValidacion("No existen fichas por enviar", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            DataTable dt = new DataTable();
            dt = (DataTable)Session["FichasPorEnviar"];

            //-----------------------------------------------------------------------------
            // Valida si existe marcado por lo menos un item antes de guardar los cambios
            //-----------------------------------------------------------------------------
            bool bSelecciono = false;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CheckBox chkSelItem;

                chkSelItem = (CheckBox)gdvFichas.Rows[i].FindControl("chkSelItem");
                if (chkSelItem.Checked == true)
                {
                    bSelecciono = true;
                    break;
                }
            }
            if (bSelecciono == false)
            {
                ctrlValidacionFichas.MostrarValidacion("No se ha seleccionado ninguna ficha", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            return bolEsCorrecto;
        }

        protected void btnAdicionarFichas_Click(object sender, EventArgs e)
        {
            if (!(validarAdicionarFichas()))
            { return; }

            long intGuiaDespachoId = Convert.ToInt64(HFGuiaDespachoId.Value);

            DataTable dt = new DataTable();
            dt = (DataTable)Session["FichasPorEnviar"];

            ArrayList listaDocumentos = new ArrayList();
            RE_FICHAENVIADA objFichaEnviadaBE;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CheckBox chkSelItem;
                chkSelItem = (CheckBox)gdvFichas.Rows[i].FindControl("chkSelItem");
                if (chkSelItem.Checked == true)
                {
                    objFichaEnviadaBE = new RE_FICHAENVIADA();

                    objFichaEnviadaBE.fien_iGuiaDespachoId = intGuiaDespachoId;
                    objFichaEnviadaBE.fien_iFichaRegistralId = Convert.ToInt64(dt.Rows[i]["FIRE_IFICHAREGISTRALID"].ToString());
                    objFichaEnviadaBE.fien_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objFichaEnviadaBE.fien_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    objFichaEnviadaBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    listaDocumentos.Add(objFichaEnviadaBE);
                }
            }
            FichaEnviadaBL objFichaEnviadaBL = new FichaEnviadaBL();

            objFichaEnviadaBL.insertar(listaDocumentos);

            if (!(objFichaEnviadaBL.isError))
            {
                mdlpopupFichas.Hide();
                consultarFichasEnviadas(intGuiaDespachoId);
                updConsulta.Update();

                ctrlToolBarConsultarFichas_btnCancelarHandler();
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Fichas para Enviar - Nuevo", Constantes.CONST_MENSAJE_EXITO));

            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Fichas para Enviar - Nuevo", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
            }
        }

        #endregion

        #region MantenimientoGuiaDespacho

        private bool validarEdicionGuiaDespacho()
        {
            bool bolEscorrecto = true;

            if (txtFechaEnvioGuia.Text.Trim() == "")
            {
                ctrlValidacionEdicionGuia.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            if (Comun.EsFecha(txtFechaEnvioGuia.Text.Trim()) == false)
            {
                ctrlValidacionEdicionGuia.MostrarValidacion("La fecha de envio de la Guía no es válida.", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            if (txtGuiaDespacho.Text.Trim() == "")
            {
                ctrlValidacionEdicionGuia.MostrarValidacion("Digite la Guía de Despacho", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            if (txtNombreEmpresa.Text.Trim() == "" && txtNombreEmpresa.Enabled)
            {
                ctrlValidacionEdicionGuia.MostrarValidacion("Digite el Nombre de la Empresa de Envio", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            if (txtNroHoja.Text.Trim() == "")
            {
                ctrlValidacionEdicionGuia.MostrarValidacion("Digite el Nro. de Hoja de Remisión y Oficio", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            if (txtGuiaAerea.Text.Trim() == "")
            {
                ctrlValidacionEdicionGuia.MostrarValidacion("Digite el Nro. de Guía Aérea", true, Enumerador.enmTipoMensaje.INFORMATION);
                return false;
            }
            return bolEscorrecto;
        }

        void ctrlToolBarMantenimientoGuiaDespacho_btnNuevoHandler()
        {
            ctrlToolBarMantenimientoGuiaDespacho.btnNuevo.Enabled = false;
            ctrlToolBarMantenimientoGuiaDespacho.btnEditar.Enabled = false;
            ctrlToolBarMantenimientoGuiaDespacho.btnEliminar.Enabled = false;
            ctrlToolBarMantenimientoGuiaDespacho.btnGrabar.Enabled = true;
            ctrlToolBarMantenimientoGuiaDespacho.btnCancelar.Enabled = true;
            ddl_EstadoGuia.SelectedIndex = 0;
            ddl_EstadoGuia.Enabled = false;
            txtFechaEnvioGuia.Text = "";
            txtGuiaDespacho.Text = "";
            txtNroHoja.Text = "";
            txtNombreEmpresa.Text = "";
            txtGuiaAerea.Text = "";
        }
        
        void ctrlToolBarMantenimientoGuiaDespacho_btnEditarHandler()
        {
            if (!validarEdicionGuiaDespacho())
            { return; }

            long intGuiaDespachoId = Convert.ToInt64(HFGuiaDespachoId.Value);
            string strGuiaDespacho = txtGuiaDespacho.Text.Trim();
            Int16 intTipoEnvioId = Convert.ToInt16(RBListTipoEnvio.SelectedValue);
            string strNroHoja = txtNroHoja.Text.Trim();
            string strNombreEmpresa = txtNombreEmpresa.Text.Trim().ToUpper();
            string strGuiaAerea = txtGuiaAerea.Text.Trim();
            Int16 intEstadoGuia = Convert.ToInt16(ddl_EstadoGuia.SelectedValue);

            DateTime dFechaEnvio = txtFechaEnvioGuia.Value();

            RE_GUIADESPACHO objGuiaDespachoBE = new RE_GUIADESPACHO();

            objGuiaDespachoBE.gude_iGuiaDespachoId = intGuiaDespachoId;
            objGuiaDespachoBE.gude_dFechaEnvio = dFechaEnvio;
            objGuiaDespachoBE.gude_vNumeroGuiaDespacho = strGuiaDespacho;
            objGuiaDespachoBE.gude_sTipoEnvioId = intTipoEnvioId;
            objGuiaDespachoBE.gude_vNombreEmpresaEnvio = strNombreEmpresa;
            objGuiaDespachoBE.gude_vNumeroHoja = strNroHoja;
            objGuiaDespachoBE.gude_vGuiaAerea = strGuiaAerea;
            objGuiaDespachoBE.gude_sEstadoGuia = intEstadoGuia;
            objGuiaDespachoBE.gude_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objGuiaDespachoBE.gude_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            objGuiaDespachoBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();

            objGuiaDespachoBL.Actualizar(objGuiaDespachoBE);
          

            string strScript = string.Empty;

            if (!(objGuiaDespachoBL.isError))
            {
                ctrlToolBarGuiaDespacho_btnBuscarHandler();
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Guía de Despacho - Actualizar", Constantes.CONST_MENSAJE_MANT_EXITO);                
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Guía de Despacho - Actualizar", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);                
            }
            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
            mdlpopupFichas.Hide();
        }

        private void AnularGuiaDespacho(long intGuiaDespachoId)
        {
            GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();
            RE_GUIADESPACHO objGuiaDespachoBE = new RE_GUIADESPACHO();

            objGuiaDespachoBE.gude_iGuiaDespachoId = intGuiaDespachoId;
            objGuiaDespachoBE.gude_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objGuiaDespachoBE.gude_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            objGuiaDespachoBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            objGuiaDespachoBL.Anular(objGuiaDespachoBE);

            if (!(objGuiaDespachoBL.isError))
            {
                ctrlToolBarGuiaDespacho_btnBuscarHandler();

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Guía de Despacho - Anulación", Constantes.CONST_MENSAJE_EXITO_ANULAR));
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Guía de Despacho - Anulación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
            }
        }

        void ctrlToolBarMantenimientoGuiaDespacho_btnGrabarHandler()
        {
            if (!validarEdicionGuiaDespacho())
            { return; }

            string strGuiaDespacho = txtGuiaDespacho.Text.Trim();
            Int16 intTipoEnvioId = Convert.ToInt16(RBListTipoEnvio.SelectedValue);
            string strNroHoja = txtNroHoja.Text.Trim();
            string strNombreEmpresa = txtNombreEmpresa.Text.Trim().ToUpper();
            string strGuiaAerea = txtGuiaAerea.Text.Trim();
            Int16 intEstadoGuia = Convert.ToInt16(ddl_EstadoGuia.SelectedValue);

            DateTime dFechaEnvio = txtFechaEnvioGuia.Value();

            RE_GUIADESPACHO objGuiaDespachoBE = new RE_GUIADESPACHO();

            objGuiaDespachoBE.gude_dFechaEnvio = dFechaEnvio;
            objGuiaDespachoBE.gude_vNumeroGuiaDespacho = strGuiaDespacho;
            objGuiaDespachoBE.gude_sTipoEnvioId = intTipoEnvioId;
            objGuiaDespachoBE.gude_vNombreEmpresaEnvio = strNombreEmpresa;
            objGuiaDespachoBE.gude_vNumeroHoja = strNroHoja;
            objGuiaDespachoBE.gude_vGuiaAerea = strGuiaAerea;
            objGuiaDespachoBE.gude_sEstadoGuia = intEstadoGuia;
            objGuiaDespachoBE.gude_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objGuiaDespachoBE.gude_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            objGuiaDespachoBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            GuiaDespachoBL objGuiaDespachoBL = new GuiaDespachoBL();

            objGuiaDespachoBL.Insertar(objGuiaDespachoBE);

            string strScript = string.Empty;

            if (!(objGuiaDespachoBL.isError))
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Guía de Despacho - Nuevo", Constantes.CONST_MENSAJE_EXITO);
                ctrlToolBarMantenimientoGuiaDespacho_btnCancelarHandler();
                ctrlToolBarGuiaDespacho_btnBuscarHandler();
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Guía de Despacho - Nuevo", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
            }
            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.HabilitarTab(0);
            txtNombreEmpresa.Enabled = false;
            Comun.EjecutarScript(Page, strScript);
        }
        
        void ctrlToolBarMantenimientoGuiaDespacho_btnCancelarHandler()
        {
            txtFechaEnvioGuia.Text = "";
            txtGuiaDespacho.Text = "";
            for (int i = 0; i < RBListTipoEnvio.Items.Count; i++)
            {
                RBListTipoEnvio.Items[i].Selected = false;
            }
            RBListTipoEnvio.Items[0].Selected = true;
            txtNroHoja.Text = "";
            txtNombreEmpresa.Text = "";
            txtGuiaAerea.Text = "";
            ddl_EstadoGuia.SelectedIndex = 0;
            ctrlToolBarMantenimientoGuiaDespacho.btnNuevo.Enabled = true;
            ctrlToolBarMantenimientoGuiaDespacho.btnGrabar.Enabled = true;
            ctrlToolBarMantenimientoGuiaDespacho.btnEditar.Enabled = false;            

            string strScript = string.Empty;            
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
   
        }

        #endregion

        private void AnularFichaEnviada(long intFichaEnviadaId)
        {
            FichaEnviadaBL objFichaEnviadaBL = new FichaEnviadaBL();
            RE_FICHAENVIADA objFichaEnviadaBE = new RE_FICHAENVIADA();

            objFichaEnviadaBE.fien_iFichaEnviadaId = intFichaEnviadaId;
            objFichaEnviadaBE.fien_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objFichaEnviadaBE.fien_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            objFichaEnviadaBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            objFichaEnviadaBL.anular(objFichaEnviadaBE);

            if (!(objFichaEnviadaBL.isError))
            {
                long intGuiaDespachoId = Convert.ToInt64(HFGuiaDespachoId.Value);

                consultarFichasEnviadas(intGuiaDespachoId);

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ficha Enviada - Anulación", Constantes.CONST_MENSAJE_EXITO_ANULAR));
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ficha Enviada - Anulación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
            }
        }

        protected void btnSeleccionarFichas_Click(object sender, EventArgs e)
        {
            lblListaFichasPorEnviar.Visible = false;
            lblText.Visible = false;
            lblCantidad.Visible = false;
            btnAdicionarFichas.Visible = false;            
            mdlpopupFichas.Show();
        }

        void ctrlToolBarConsultarFichas_btnSalirHandler()
        {
            ddl_TipoTramiteSelFicha.SelectedIndex = 0;
            txtNroFichaRegistralSelFicha.Text = "";
            txtFechaInicioSelFicha.Text = "";
            txtFechaFinSelFicha.Text = "";
            ddlEstadoFichaSelFicha.SelectedIndex = 0;
            Session["FichasPorEnviar"] = null;
            gdvFichas.DataSource = null;
            gdvFichas.DataBind();
            btnAdicionarFichas.Visible = false;
            lblListaFichasPorEnviar.Visible = false;
            lblText.Visible = false;
            lblCantidad.Visible = false;
            mdlpopupFichas.Hide();
        }

        void ctrlToolBarGuiaDespacho_btnSalirHandler()
        {
            Session["FichasPorEnviar"] = null;
            Response.Redirect("~/Default.aspx");
        }

        protected void RBListTipoEnvio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBListTipoEnvio.SelectedValue == "11073")
            {
                txtNombreEmpresa.Enabled = false;
            }
            else { txtNombreEmpresa.Enabled = true; }
        }

        protected void grdGuiaDespacho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnEditarGuiaDespacho = e.Row.FindControl("btnEditarGuiaDespacho") as ImageButton;

            ImageButton btnAnularGuiaDespacho = e.Row.FindControl("btnAnularGuiaDespacho") as ImageButton;
            

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditarGuiaDespacho, btnAnularGuiaDespacho};
                Comun.ModoLectura(ref arrImageButtons);
            }
        }

        protected void gdvFichasEnviadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnAnular = e.Row.FindControl("btnAnular") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnAnular };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }
    }
}
