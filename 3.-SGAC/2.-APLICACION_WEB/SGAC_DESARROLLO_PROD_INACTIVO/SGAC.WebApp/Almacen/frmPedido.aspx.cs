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


namespace SGAC.WebApp.Almacen
{
    public partial class frmPedidos : MyBasePage
    {
        #region VARIABLES
        private Int64 iPedidoId;

        PedidoMantenimientoBL oPedidoMantenimientoBL = new PedidoMantenimientoBL();
        PedidoConsultaBL oPedidoConsultaBL = new PedidoConsultaBL();
        PedidoHistoricoConsultaBL oPedidoHistoricoConsultaBL = new PedidoHistoricoConsultaBL();
        PedidoHistoricoMantenimientoBL oPedidoHistoricoMantenimientoBL = new PedidoHistoricoMantenimientoBL();
        #endregion

        #region CAMPOS
        private Enumerador.enmAccion enmAccion;
        private string strNombreEntidad = "PEDIDO";
        private string strVariableAccion = "Pedido_Accion";
        private string strVariablePedido = "Pedido_Id";
        private string strVariableTabla = "DTPedido";
        private string strVariableEstadoInicial = "Pedido_EstadoIdInicio";
        private string strVariableEstadoNuevo = "Pedido_EstadoIdNuevo";
        #endregion

        #region EVENTOS
        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

     
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBar1.btnImprimir.Visible = false;
            ctrlToolBar1.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar1_btnBuscarHandler);
            ctrlToolBar1.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar1_btnCancelarHandler);

            ctrlToolBar2.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBar2_btnNuevoHandler);
            ctrlToolBar2.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar2_btnCancelarHandler);
            ctrlToolBar2.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBar2_btnEditarHandler);
            ctrlToolBar2.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBar2_btnGrabarHandler);
            ctrlToolBar2.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBar2_btnEliminarHandler);

            // Configuración botones
            ctrlToolBar2.btnGrabar.OnClientClick = "return ValidarRegistro()";

            // Oficinas Consulares
            cboMisionConsO.ddlOficinaConsular.AutoPostBack = true;
            cboMisionConsO.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsularO_SelectedIndexChanged);

            cboMisionConsD.ddlOficinaConsular.AutoPostBack = true;
            cboMisionConsD.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsularD_SelectedIndexChanged);

            dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            dtpFecInicio.EndDate = new DateTime(3000, 1, 1);

            dtpFecFin.StartDate = new DateTime(1900, 1, 1);
            dtpFecFin.EndDate = new DateTime(3000, 1, 1);

            Comun.CargarPermisos(Session, ctrlToolBar1, ctrlToolBar2, grdPedido, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);


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
                object oJefaturaFlag = cboMisionConsO.EsOficinaJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                Session["oJefaturaFlag"] = bool.Parse(oJefaturaFlag.ToString());

                lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

                llenarBovedas();
                
                CargarListados();
                CargarDatosIniciales();
            }


            if (!cboMisionConsO.OficinaPoseeHijos(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()))
            {
                grdPedido.Columns[ObtenerIndiceColumnaGrilla(grdPedido, "Atender")].Visible = false;
                cboMisionConsO.Enabled = false;
                cboBovedaO.Enabled = false;

                if(cboMisionConsO.EsPermitidoDestinoLima())
                cboTipoPed.SelectedValue = ((int)Enumerador.enmPedidoTipo.PEDIDO_LIMA).ToString();
                else
                    cboTipoPed.SelectedValue = ((int)Enumerador.enmPedidoTipo.PEDIDO_JEFATURA).ToString();
            }


            if ((bool)Session["oJefaturaFlag"] &&
                Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() != Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
                cboTipoPed.Enabled = true;
            else
                cboTipoPed.Enabled = false;


            Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
            cboInsumo.Enabled = false;

            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBar2.btnNuevo, ctrlToolBar2.btnGrabar, ctrlToolBar2.btnEliminar};
                GridView[] arrGridView = { grdPedido };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        void ddlOficinaConsularO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvO = new DataView();

            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();
            
            dvO = dtBovedas.DefaultView;

            dvO = dtBovedas.Copy().DefaultView;

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue.ToString() +
                            " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;

            DataTable dtBovedaOrigen = dvO.ToTable();
            Util.CargarDropDownList(cboBovedaO, dtBovedaOrigen, "Descripcion", "OfConsularId", true);
        }

        void ddlOficinaConsularD_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvD = new DataView();

            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            dvD = dtBovedas.Copy().DefaultView;


            //DataView dvD = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;

            dvD.RowFilter = "OfConsularId=" + cboMisionConsD.SelectedValue.ToString() +
                            " and TipoBoveda=" + cboTipoBovedaD.SelectedValue;

            DataTable dtBovedaDestino = dvD.ToTable();
            Util.CargarDropDownList(cboBovedaD, dtBovedaDestino, "Descripcion", "OfConsularId", true);
        }

        protected void cboMisionConsD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdEstadoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Trim() != "&nbsp;")
                    e.Row.Cells[2].Text = (Comun.FormatearFecha(e.Row.Cells[2].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            }
        }

        protected void grdPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[ObtenerIndiceColumnaGrilla(grdPedido,"pedi_sEstadoId")].Text.Trim()!=
                    Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE).ToString())
                {
                    e.Row.FindControl("btnEditar").Visible = false;
                    e.Row.FindControl("btnAtender").Visible = false;
                }

                string strFormatofecha = "";
                strFormatofecha = Convert.ToString(Session["Formatofecha"]);

                string Date = e.Row.Cells[16].Text;  //Campo donde tienes tu fecha
                e.Row.Cells[16].Text = Convert.ToDateTime(Date).ToString(strFormatofecha);

                if (e.Row.Cells[17].Text != "&nbsp;")
                {
                    Date = e.Row.Cells[17].Text;  //Campo donde tienes tu fecha
                    e.Row.Cells[17].Text = Convert.ToDateTime(Date).ToString(strFormatofecha);
                }
                
            }
        }

        protected void grdPedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intIndex = Convert.ToInt32(e.CommandArgument);
            Session["PasaPedidoMovimiento"] = 0;

            grdPedido.DataSource = (DataTable)Session[strVariableTabla];
            grdPedido.DataBind();

            int intSeleccionado = Convert.ToInt32(grdPedido.Rows[intIndex].Cells[0].Text);
            Session[strVariablePedido] = intSeleccionado;
            Session[strVariableEstadoInicial] = Convert.ToInt32(grdPedido.Rows[intIndex].Cells[21].Text);
            

            if (intSeleccionado > -1)
            {
                 
                //La fecha de modificación permitirá verificar posteriormente que el pedido no se encuentre modificado.
                Session["pedi_dFechaModificacion"] = grdPedido.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdPedido, "pedi_dFechaModificacion")].Text.ToLower().Replace("&nbsp;", "").Replace("&amp;", "").Trim();

                if (e.CommandName == "Ver")
                {
                    Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                    SGAC.BE.AL_PEDIDO objPEDIDO = ValoresSeleccionCabecera(intIndex);
                    PintarCabecera(objPEDIDO);
                    int intCodigoPed = intIndex;
                    TraeHistoricoPedido(intCodigoPed);

                    ctrlToolBar2.btnGrabar.Enabled = false;
                    ctrlToolBar2.btnEditar.Enabled = true;
                    ctrlToolBar2.btnEliminar.Enabled = false;

                    cboTipoPed.Enabled = false;
                    TablaEstadoPedido.Visible = true;
                    DesHabilitaCampos();

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    Comun.EjecutarScript(Page, strScript);
                }
                else if (e.CommandName == "Editar")
                {
                    int intResultado = 0;
                    SGAC.BE.AL_PEDIDO objPEDIDO = ValoresSeleccionCabecera(intIndex);
                    string strPedidoCodigo = grdPedido.Rows[intIndex].Cells[13].Text;
                    string strPedidoEstado = grdPedido.Rows[intIndex].Cells[21].Text;
                    string strDescripcionEstado = grdPedido.Rows[intIndex].Cells[22].Text;
                    DataTable dt = oPedidoConsultaBL.ExistePedidoAtendido(strPedidoCodigo);

                    intResultado = Convert.ToInt32(dt.Rows[0]["CANT"]);

                    ctrlToolBar2.btnEditar.Enabled = false;
                    ctrlToolBar2.btnEliminar.Enabled = true;
                    ctrlToolBar2.btnGrabar.Enabled = true;

                    if (intResultado == 0 && strPedidoEstado == Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE).ToString())
                    {
                        Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                        PintarCabecera(objPEDIDO);

                        int intCodigoPed = intIndex;
                        TraeHistoricoPedido(intCodigoPed);
                        TablaEstadoPedido.Visible = true;
                        HabilitaCampos();
                        TablaEstadoPedido.Visible = true;
                        
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                        Comun.EjecutarScript(Page, strScript);
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Pedido (" + strPedidoCodigo + ") porque se encuentra " + strDescripcionEstado));
                    }

                }
                else if (e.CommandName == "Atender")
                {
                    int intResultado = 0;
                    SGAC.BE.AL_PEDIDO objPEDIDO = ValoresSeleccionCabecera(intIndex);
                    string strPedidoCodigo = grdPedido.Rows[intIndex].Cells[13].Text;
                    string strPedidoEstado = grdPedido.Rows[intIndex].Cells[21].Text;
                    string strDescripcionEstado = grdPedido.Rows[intIndex].Cells[22].Text;

                    // Verificación: 
                    if (objPEDIDO.pedi_sOficinaConsularIdDestino != Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede atender pedido (" + strPedidoCodigo + "). Se encuentra asignado a otra Oficina Consular."));
                        return;
                    }

                    DataTable dt = oPedidoConsultaBL.ExistePedidoAtendido(strPedidoCodigo);

                    intResultado = Convert.ToInt32(dt.Rows[0]["CANT"]);

                    if (intResultado == 0 && strPedidoEstado == Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE).ToString())
                    {
                        Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                        Session["Pedido_Id"] = objPEDIDO.pedi_iPedidoId;
                        Session["Pedido_Acta"] = objPEDIDO.pedi_vActaRemision;
                        Session["Pedido_Cantidad"] = objPEDIDO.pedi_ICantidad;
                        Session["Pedido_Estado"] = Session["Estado"];
                        Session["Pedido_Fecha"] = objPEDIDO.pedi_dFechaRegistro.ToShortDateString();
                        Session["Pedido_Insumo"] = Session["Insumo"];
                        Session["Pedido_InsumoTipo"] = objPEDIDO.pedi_sInsumoTipoId;
                        Session["Pedido_Motivo"] = Session["Motivo"];
                        Session["Pedido_Codigo"] = objPEDIDO.pedi_cPedidoCodigo;
                        Session["OC_PEDI_ORIGEN"] = objPEDIDO.pedi_sOficinaConsularIdDestino;
                        Session["OC_PEDI_DESTINO"] = objPEDIDO.pedi_sOficinaConsularIdOrigen;
                        Session["BO_PEDI_ORIGEN"] = objPEDIDO.pedi_sBodegaDestinoId;
                        Session["BO_PEDI_DESTINO"] = objPEDIDO.pedi_sBodegaOrigenId;

                        Session["PasaPedidoMovimiento"] = 1;
                        Response.Redirect("frmMovimiento.aspx");

                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                        Comun.EjecutarScript(Page, strScript);
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede atender pedido (" + strPedidoCodigo + "). Se encuentra " + strDescripcionEstado));
                    }
                }
            }
        }

        protected void cboTipoBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtBovedasO = new DataTable();

            DataTable dtBovedas = new DataTable();

            dtBovedas = obtenerBovedas();

            dtBovedasO = dtBovedas.Copy();

            
            //DataTable dtBovedasO = new DataTable();

            //dtBovedasO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy();

            DataView dvO = (dtBovedasO).DefaultView;
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue +
                            " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;

            DataTable dtBovedaOrigen = dvO.ToTable();
            Util.CargarDropDownList(cboBovedaO, dtBovedaOrigen, "Descripcion", "OfConsularId", true);
            UpdMantenimiento.Update();
        }

        protected void cboTipoBovedaD_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtBovedasD = new DataTable();

            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            dtBovedasD = dtBovedas.Copy();

            //dtBovedasD = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy();

            DataView dvD = (dtBovedasD).DefaultView;
            dvD.RowFilter = "OfConsularId=" + cboMisionConsD.SelectedValue + " and TipoBoveda=" + cboTipoBovedaD.SelectedValue;

            Util.CargarDropDownList(cboBovedaD, dvD.ToTable(), "Descripcion", "OfConsularId", true);
            UpdMantenimiento.Update();
        }
        #endregion

        #region MANTENIMIENTO
        void ctrlToolBar2_btnNuevoHandler()
        {
            Session[strVariableEstadoInicial] = Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE);
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Session["pedi_dFechaModificacion"] = null;
            ctrlToolBar2.btnGrabar.Enabled = true;
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            HabilitaCampos();
            LimpiaCampos();

            TablaEstadoPedido.Visible = false;
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

            txtActRemR.Enabled = true;
            txtObservacion.Enabled = true;
            txtCant.Enabled = true;

            Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
            cboInsumo.Enabled = false;

            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;
        }

        void ctrlToolBar2_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
            HabilitaCampos();
            LimpiaCampos();
            TablaEstadoPedido.Visible = false;

            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;
            Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
            cboInsumo.Enabled = false;


            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }

        void ctrlToolBar2_btnEditarHandler()
        {
            if (cboPedidoEstadoR.SelectedValue != Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE).ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Pedido (" + lblNroPedido.Text + ") porque se encuentra " + cboPedidoEstadoR.SelectedItem.Text));
            }
            else
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                ctrlToolBar2.btnEditar.Enabled = false;
                ctrlToolBar2.btnEliminar.Enabled = false;
                ctrlToolBar2.btnGrabar.Enabled = true;
                ctrlToolBar2.btnConfiguration.Enabled = false;

                HabilitaCampos();
                TablaEstadoPedido.Visible = true;
            }
        }

        void ctrlToolBar2_btnGrabarHandler()
        {
            if (cboTipoBovedaO.SelectedValue.ToString() == cboTipoBovedaD.SelectedValue.ToString() &&
                cboBovedaO.SelectedValue.ToString() == cboBovedaD.SelectedValue.ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se puede realizar un pedido con el mismo origen y destino."));
                return;
            }

            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() == Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
            {
                if (!cboMisionConsO.EsPermitidoDestinoLima())
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se puede realizar un pedido porque el origen es una dependencia."));
                    return;
                }
            }
            else
            {
                if (!cboMisionConsO.EsDestinoMiJefatura(cboMisionConsD.SelectedValue.ToString()))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se puede realizar un pedido porque el origen no es dependencia del destino."));
                    return;
                }
            }

            GrabarDatosCabecera();
            DesHabilitaCampos();
            TablaEstadoPedido.Visible = true;
            ctrlToolBar2_btnNuevoHandler();
            UpdMantenimiento.Update();
        }

        void ctrlToolBar2_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBar2_btnGrabarHandler();
            DesHabilitaCampos();
            cboPedidoMotivo.Enabled = true;
            txtObservacion.Enabled = true;
        }

        #endregion

        #region CONSULTA
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

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            
            if (datFechaInicio > datFechaFin)
            {
                Session["dt"] = new DataTable();
                grdPedido.DataSource = new DataTable();
                grdPedido.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.WARNING);                
            }
            else
            {
                ctrlPaginador.InicializarPaginador();
                CargarGrilla();
            }
        }
        void ctrlToolBar1_btnCancelarHandler()
        {
            HabilitaCampos();
            LimpiaCampos();
            grdPedido.DataSource = null;
            grdPedido.DataBind();
            ctrlPaginador.Visible = false;

            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;

            Comun.SeleccionarItem(cboInsumo, "AUTOADHESIVO");
            cboInsumo.Enabled = false;

            UpdMantenimiento.Update();
        }
        #endregion

        #region METODOS

        private void CargarDatosIniciales()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Session["PasaPedidoMovimiento"] = 0;
            cboInsumo.Enabled = true;
            cboPedidoEstadoR.Enabled = false;
            cboMisionConsO.AutoPostBack = true;
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            ctrlToolBar2.btnConfiguration.Enabled = false;
            HabilitaCampos();
            LimpiaCampos();
            UpdMantenimiento.Update();
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

        private object[] ObtenerParametrosBusqueda(string strCodPedidoC)
        {
            
            int pedi_IOficinaConsularIdOrigen = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            string strActaRemision = "";
            int intEstado = 0;
            int intInsumo = 0;

            if (txtActRemR.Text != null)
                strActaRemision = txtActRemC.Text.Trim();
            if (cboPedidoEstadoC.SelectedValue != null)
                intEstado = Convert.ToInt32(cboPedidoEstadoC.SelectedValue);
            if (cboInsumoC.SelectedValue != null)
                intInsumo = Convert.ToInt32(cboInsumoC.SelectedValue);

            int intTotalRegistros = 0, intTotalPaginas = 0;

            DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
           
            object[] arrParametros = { pedi_IOficinaConsularIdOrigen, 
                                       datFechaInicio, 
                                       datFechaFin, 
                                       strActaRemision, 
                                       intEstado,
                                       intInsumo,
                                       strCodPedidoC,
                                       ctrlPaginador.PaginaActual,
                                       Constantes.CONST_CANT_REGISTRO,
                                       intTotalRegistros,
                                       intTotalPaginas
                                     };

            return arrParametros;
        }

        private void GrabarDatosHistorico()
        {
            SGAC.BE.AL_PEDIDOHISTORICO objPEDIDOD;
            int intOficinaConsularId = Convert.ToInt32(cboMisionConsO.SelectedValue);

            int intResultado;

            objPEDIDOD = ValoresNuevosHistorico();

            Session[strVariableEstadoNuevo] = Convert.ToInt32(cboPedidoEstadoR.SelectedValue);
            Session[strVariableEstadoInicial] = Convert.ToInt32(Session[strVariableEstadoInicial]);

            if (Convert.ToInt32(Session[strVariableEstadoNuevo]) == Convert.ToInt32(Session[strVariableEstadoInicial])
                && (Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
            {
                intResultado = oPedidoHistoricoMantenimientoBL.PedidoHistoricoActualizar(objPEDIDOD, intOficinaConsularId);
            }
            else
            {
                intResultado = oPedidoHistoricoMantenimientoBL.PedidoHistoricoAdicionar(objPEDIDOD, intOficinaConsularId);
            }
        }

        private void CargarGrilla()
        {
            object[] arrParametros = ObtenerParametrosBusqueda(string.Empty);

            int intTotalRegistros =(int)arrParametros[9];
            int intTotalPaginas = (int)arrParametros[10];

            DataTable dt = oPedidoConsultaBL.Consultar((int)arrParametros[0], (DateTime)arrParametros[1], (DateTime)arrParametros[2],
                arrParametros[3].ToString(), (int)arrParametros[4], (int)arrParametros[5], arrParametros[6].ToString(),
                (int)arrParametros[7], (int)arrParametros[8], ref intTotalRegistros, ref intTotalPaginas);

            Session[strVariableTabla] = dt;
            grdPedido.DataSource = dt;
            grdPedido.DataBind();

            // Mensaje total de registros 0
            if (dt.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA,true,Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);

            }

            // Paginador
            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;

            ctrlPaginador.Visible = false;
            if (ctrlPaginador.TotalPaginas > 1)
                ctrlPaginador.Visible = true;

            updGrillaConsulta.Update();
            updConsulta.Update();
        }

        private void GrabarDatosCabecera()
        {
            SGAC.BE.AL_PEDIDO objPEDIDO = new BE.AL_PEDIDO();
            objPEDIDO = ValoresNuevosCabecera();
            object[] arrParametros = ObtenerParametrosBusqueda(objPEDIDO.pedi_cPedidoCodigo);


            int intTotalRegistros = (int)arrParametros[9];
            int intTotalPaginas = (int)arrParametros[10];

            DataTable dt = oPedidoConsultaBL.Consultar((int)arrParametros[0], (DateTime)arrParametros[1], (DateTime)arrParametros[2],
                arrParametros[3].ToString(), (int)arrParametros[4], (int)arrParametros[5], arrParametros[6].ToString(),
                (int)arrParametros[7], (int)arrParametros[8], ref intTotalRegistros, ref intTotalPaginas);

            //Validar que el registro no haya sido modificado
            if (dt.Rows.Count > 0)
            {
                if (Session["pedi_dFechaModificacion"].ToString() !=
                        dt.Rows[0]["pedi_dFechaModificacion"].ToString())
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "El pedido ha sido modificado, no se puede guardar la información."));
                    Comun.EjecutarScript(Page, Util.MoverTab(0));
                    ctrlToolBar2_btnNuevoHandler();
                    return;
                }
            }

            arrParametros = new object[1];


            int intResultado = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    objPEDIDO = ValoresNuevosCabecera();
                    intResultado = oPedidoMantenimientoBL.PedidoAdicionar(objPEDIDO);

                    iPedidoId = objPEDIDO.pedi_iPedidoId;
                    GrabarDatosHistorico();
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Pedido Generado: (" + iPedidoId + " )", true, Enumerador.enmTipoMensaje.INFORMATION);
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    objPEDIDO = ValoresNuevosCabecera();
                    intResultado = oPedidoMantenimientoBL.PedidoActualizar(objPEDIDO);
                    GrabarDatosHistorico();
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Pedido Modificado: (" + lblNroPedido.Text + " )");
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    objPEDIDO = ValoresNuevosCabecera();

                    objPEDIDO.pedi_sEstadoId = Convert.ToInt16(Enumerador.enmPedidoEstado.ANULADO);
                    cboPedidoEstadoR.SelectedValue = Convert.ToInt32(Enumerador.enmPedidoEstado.ANULADO).ToString();

                    intResultado = oPedidoMantenimientoBL.PedidoEliminar(objPEDIDO);
                    GrabarDatosHistorico();
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Pedido Anulado: (" + lblNroPedido.Text + " )");
                    break;
            }

            string strScript = string.Empty;

            if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
            {
                HabilitaCampos();
                LimpiaCampos();

                ActualizaHistoricoPedido(Convert.ToInt32(objPEDIDO.pedi_iPedidoId));
                CargarGrilla();

                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                else
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);

        }

       
        private void HabilitaCampos()
        {
            txtActRemC.Enabled = true;
            cboPedidoEstadoC.SelectedIndex = 0;

            cboTipoBovedaO.Enabled = false;
            cboMisionConsD.Enabled = false;
            cboTipoBovedaD.Enabled = false;
            cboBovedaD.Enabled = false;
            dtpFecha.Enabled = false;
            txtActRemR.Enabled = true;
            txtDescripcion.Enabled = true;
            txtCant.Enabled = true;
            txtObservacion.Enabled = true;
            cboInsumo.Enabled = true;
            cboPedidoMotivo.Enabled = false;
            cboPedidoEstadoR.Enabled = false;
            grdEstadoPedido.Enabled = false;
        }

        private void DesHabilitaCampos()
        {
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            //Campos de Consulta
            dtpFecInicio.Text = DateTime.Today.ToString(strFormatoFechasInicio);
            dtpFecFin.Text = DateTime.Today.ToString(strFormatoFechas);

            txtActRemC.Enabled = true;
            cboPedidoEstadoC.SelectedIndex = 0;
            //Campos de Registro
            cboMisionConsO.Enabled = false;
            cboTipoBovedaO.Enabled = false;
            cboBovedaO.Enabled = false;
            cboMisionConsD.Enabled = false;
            cboTipoBovedaD.Enabled = false;
            cboBovedaD.Enabled = false;
            dtpFecha.Text = DateTime.Today.ToString(strFormatoFechas);
            dtpFecha.Enabled = false;
            txtActRemR.Enabled = false;
            txtDescripcion.Enabled = false;
            txtCant.Enabled = false;
            txtObservacion.Enabled = false;
            cboInsumo.Enabled = false;
            cboPedidoMotivo.Enabled = false;
            cboPedidoEstadoR.Enabled = false;
            grdEstadoPedido.Enabled = false;
        }

        private void CargarListados()
        {
            

            // Consulta
            Util.CargarParametroDropDownList(cboPedidoEstadoC, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PEDIDO), true, " - TODOS - ");

            //*****************************************
            
            Enumerador.enmGrupo[] arrGrupos = { Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO,Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO, 
                                                Enumerador.enmGrupo.ALMACEN_MOTIVO_PEDIDO, Enumerador.enmGrupo.ALMACEN_TIPO_PEDIDO };
            Enumerador.enmGrupo[] arrGrupos1 = { Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO };

            Enumerador.enmGrupo[] arrGrupos2 = { Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO, Enumerador.enmGrupo.ALMACEN_MOTIVO_PEDIDO, Enumerador.enmGrupo.ALMACEN_TIPO_PEDIDO };
                        
            DropDownList[] arrDDL1 = { cboInsumoC};
            DropDownList[] arrDDL2 = { cboInsumo, cboPedidoMotivo, cboTipoPed };

            DataTable dtGrupoParametros = new DataTable();

            dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL1, arrGrupos1, dtGrupoParametros, true, " - TODOS - ");
            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL2, arrGrupos2, dtGrupoParametros, true);
            //***********************************************

            //Util.CargarParametroDropDownList(cboInsumoC, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true, " - TODOS - ");

            // Mantenimiento            
            //Util.CargarParametroDropDownList(cboInsumo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true);
            //Util.CargarParametroDropDownList(cboPedidoMotivo, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_MOTIVO_PEDIDO), true);
            //Util.CargarParametroDropDownList(cboTipoPed, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_PEDIDO), true);


            Util.CargarParametroDropDownList(cboPedidoEstadoR, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PEDIDO), true);
            cboInsumo.SelectedValue = Convert.ToInt32(Enumerador.enmInsumoTipo.AUTOADHESIVO).ToString();

            //Trae Boveda Origen
            cboMisionConsO.CargarPorJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO,  Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());

            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() != Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
                ELiminarRegistroCombo(cboMisionConsO.ddlOficinaConsular, Constantes.CONST_OFICINACONSULAR_LIMA.ToString());

            //Trae Boveda Destino
            cboMisionConsD.CargarPorJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());
        }

        private void LimpiaCampos()
        {
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            dtpFecInicio.Text = DateTime.Today.ToString(strFormatoFechasInicio);
            dtpFecFin.Text = DateTime.Today.ToString(strFormatoFechas);

            cboInsumoC.SelectedIndex = 0;
            txtActRemC.Text = "";
            txtCodPedidoC.Text = "";
            cboPedidoEstadoC.SelectedIndex = 0;

            //Campos de Registro
            lblNroPedido.Text = "";
            txtCant.Text = "";
            txtActRemR.Text = "";
            txtDescripcion.Text = "";
            txtObservacion.Text = "";

            dtpFecha.Text = DateTime.Today.ToString(strFormatoFechas);
            dtpFecha.Enabled = false;

            cboPedidoEstadoR.SelectedValue = Convert.ToInt32(Enumerador.enmPedidoEstado.PENDIENTE).ToString();
            cboInsumo.SelectedValue = Convert.ToInt32(Enumerador.enmInsumoTipo.AUTOADHESIVO).ToString();
            cboPedidoMotivo.SelectedValue = Convert.ToInt32(Enumerador.enmPedidoMotivo.REPOSICION_STOCK).ToString();

            cboInsumo.Enabled = true;

            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() == Constantes.CONST_OFICINACONSULAR_LIMA.ToString() ||
                (bool)Session["oJefaturaFlag"]) //Si la oficina es Lima o es Jefatura se inicalizan los combos de origen
            {
                cboTipoPed.SelectedValue = Convert.ToInt32(Enumerador.enmPedidoTipo.PEDIDO_LIMA).ToString();
                cboTipoPed_SelectedIndexChanged(null, null);
            }
            else
            {
                cboTipoPed.SelectedValue = Convert.ToInt32(Enumerador.enmPedidoTipo.PEDIDO_JEFATURA).ToString();
                cboTipoPed_SelectedIndexChanged(null, null);          
            }

            cboBovedaD.SelectedValue = cboMisionConsD.SelectedValue.ToString();

            // Pedido Histórico
            grdEstadoPedido.DataSource = null;
            grdEstadoPedido.DataBind();

            // Control de Opciones
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            ctrlToolBar2.btnGrabar.Enabled = true;
        }

        private void TraeHistoricoPedido(int intCodigoPed)
        {
            int pedi_iPedidoId = Convert.ToInt32(grdPedido.Rows[intCodigoPed].Cells[0].Text);

            DataTable dt = oPedidoHistoricoConsultaBL.Consultar(pedi_iPedidoId);
            grdEstadoPedido.DataSource = dt;
            grdEstadoPedido.DataBind();
        }

        private void ActualizaHistoricoPedido(int pedi_iPedidoId)
        {
            DataTable dt = oPedidoHistoricoConsultaBL.Consultar(pedi_iPedidoId);

            grdEstadoPedido.DataSource = dt;
            grdEstadoPedido.DataBind();
        }

        private void ELiminarRegistroCombo(DropDownList ddl, string Value)
        {
            for (int i = ddl.Items.Count - 1; i >= 0; i--)
            {
                if (ddl.Items[i].Value == Value)
                {
                    ddl.Items.RemoveAt(i);
                }
            }
        }


        private void CargarOficinaConsular(ctrlOficinaConsular cboOficConsular, DropDownList cboTipoBoveda, DropDownList cboBoveda, string strOficConsularId, string strUsuarioId = "", bool bEsUsuario = false)
        {

            if (cboTipoBoveda.Items.Count == 0)
                Util.CargarParametroDropDownList(cboTipoBoveda, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));

            cboOficConsular.SelectedValue = strOficConsularId;

            cboTipoBoveda.SelectedValue = bEsUsuario ? ((int)Enumerador.enmBovedaTipo.USUARIO).ToString() : ((int)Enumerador.enmBovedaTipo.MISION).ToString();

            //-------------------------------------------------------
            DataView dvO = new DataView();

            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            dvO = dtBovedas.Copy().DefaultView;

            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;
            //-------------------------------------------------------
            
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

        private void PintarCabecera(SGAC.BE.AL_PEDIDO objPEDIDO)
        {
            cboTipoPed.SelectedValue = objPEDIDO.pedi_sPedidoTipoId.ToString();
            cboPedidoMotivo.SelectedValue = objPEDIDO.pedi_sPedidoMotivoId.ToString();
            lblNroPedido.Text = objPEDIDO.pedi_cPedidoCodigo.ToString();
            cboInsumo.SelectedValue = objPEDIDO.pedi_sInsumoTipoId.ToString();
            dtpFecha.Text = objPEDIDO.pedi_dFechaRegistro.ToShortDateString();
            txtActRemR.Text = objPEDIDO.pedi_vActaRemision.ToString();

            if (txtActRemR.Text.ToUpper() == "&NBSP;")
            {
                txtActRemR.Text = "";
            }
            txtDescripcion.Text = objPEDIDO.pedi_vDescripcion.ToString();
            txtCant.Text = objPEDIDO.pedi_ICantidad.ToString();
            cboPedidoEstadoR.SelectedValue = objPEDIDO.pedi_sEstadoId.ToString();
            txtObservacion.Text = Page.Server.HtmlDecode(objPEDIDO.pedi_vObservacion.ToString());

            cboTipoBovedaO.SelectedValue = objPEDIDO.pedi_sBovedaTipoIdOrigen.ToString();
            cboMisionConsO.SelectedValue = objPEDIDO.pedi_sOficinaConsularIdOrigen.ToString();

            //-------------------------------------------------------
            DataView dvO = new DataView();

            DataTable dtBovedas = new DataTable();
            dtBovedas = obtenerBovedas();

            dvO = dtBovedas.Copy().DefaultView;


            //DataView dvO = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).Copy().DefaultView;
            //-------------------------------------------------------
            
            dvO.RowFilter = "OfConsularId=" + cboMisionConsO.SelectedValue +
                            " and TipoBoveda=" + cboTipoBovedaO.SelectedValue;

            if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
            {
                Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "OfConsularId", true);
            }
            else
            {
                Util.CargarDropDownList(cboBovedaO, dvO.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
            }
            cboBovedaO.SelectedValue = objPEDIDO.pedi_sBodegaOrigenId.ToString();

            cboMisionConsD.SelectedValue = objPEDIDO.pedi_sOficinaConsularIdDestino.ToString();
            cboTipoBovedaD.SelectedValue = objPEDIDO.pedi_sBovedaTipoIdDestino.ToString();
            //-------------------------------------------------
            DataView dvD = new DataView();

            dvD = dtBovedas.Copy().DefaultView;
            
            //DataView dvD = ((DataTable)Session[Constantes.CONST_SESION_DT_BOVEDA]).DefaultView;
            //-------------------------------------------------
            dvD.RowFilter = "OfConsularId=" + objPEDIDO.pedi_sOficinaConsularIdDestino;

            if (Convert.ToInt32(cboTipoBovedaD.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
            {
                dvD.RowFilter = "TipoBoveda= " + objPEDIDO.pedi_sBovedaTipoIdDestino.ToString();
                Util.CargarDropDownList(cboBovedaD, dvD.ToTable(), "Descripcion", "OfConsularId", true);
            }
            else
            {
                Util.CargarDropDownList(cboBovedaD, dvD.ToTable(), "Descripcion", "IdTablaOrigenRefer", true);
            }
            cboBovedaD.SelectedValue = objPEDIDO.pedi_sBodegaDestinoId.ToString();

            UpdMantenimiento.Update();
        }

        private SGAC.BE.AL_PEDIDO ValoresNuevosCabecera()
        {
            if (Session != null)
            {
                SGAC.BE.AL_PEDIDO objPEDIDO = new BE.AL_PEDIDO();
                objPEDIDO.pedi_iPedidoId = Convert.ToInt64(Session[strVariablePedido]);
                objPEDIDO.pedi_sPedidoTipoId = Convert.ToInt16(cboTipoPed.SelectedValue);
                objPEDIDO.pedi_sPedidoMotivoId = Convert.ToInt16(cboPedidoMotivo.SelectedValue);

                if (lblNroPedido.Text.Trim() == string.Empty)
                    objPEDIDO.pedi_cPedidoCodigo = "0";
                else
                    objPEDIDO.pedi_cPedidoCodigo = lblNroPedido.Text;

                objPEDIDO.pedi_sOficinaConsularIdOrigen = Convert.ToInt16(cboMisionConsO.SelectedValue);
                objPEDIDO.pedi_sBovedaTipoIdOrigen = Convert.ToInt16(cboTipoBovedaO.SelectedValue);

                if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
                    objPEDIDO.pedi_sBodegaOrigenId = Convert.ToInt16(cboMisionConsO.SelectedValue);
                else
                    objPEDIDO.pedi_sBodegaOrigenId = Convert.ToInt16(cboBovedaO.SelectedValue);

                objPEDIDO.pedi_sOficinaConsularIdDestino = Convert.ToInt16(cboMisionConsD.SelectedValue);
                objPEDIDO.pedi_sBovedaTipoIdDestino = Convert.ToInt16(cboTipoBovedaD.SelectedValue);

                if (Convert.ToInt32(cboTipoBovedaD.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
                    objPEDIDO.pedi_sBodegaDestinoId = Convert.ToInt16(cboMisionConsD.SelectedValue);
                else
                    objPEDIDO.pedi_sBodegaDestinoId = Convert.ToInt16(cboBovedaD.SelectedValue);

                objPEDIDO.pedi_sInsumoTipoId = Convert.ToInt16(cboInsumo.SelectedValue);
                objPEDIDO.pedi_dFechaRegistro = Comun.FormatearFecha(dtpFecha.Text);

                objPEDIDO.pedi_vActaRemision = txtActRemR.Text;
                objPEDIDO.pedi_vDescripcion = txtDescripcion.Text;

                if (txtCant.Text.Trim() == string.Empty)
                    objPEDIDO.pedi_ICantidad = Convert.ToInt32(0);
                else
                    objPEDIDO.pedi_ICantidad = Convert.ToInt32(txtCant.Text);

                objPEDIDO.pedi_vObservacion = txtObservacion.Text;

                if (Convert.ToInt32(Session[strVariableAccion]) == Convert.ToInt32(Enumerador.enmAccion.ELIMINAR))
                    objPEDIDO.pedi_sEstadoId = Convert.ToInt16(Enumerador.enmPedidoEstado.ANULADO);
                else
                    objPEDIDO.pedi_sEstadoId = Convert.ToInt16(cboPedidoEstadoR.SelectedValue);

                objPEDIDO.pedi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                
                objPEDIDO.pedi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPEDIDO.pedi_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objPEDIDO.pedi_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                return objPEDIDO;
            }

            return null;
        }

        private SGAC.BE.AL_PEDIDO ValoresSeleccionCabecera(int intIndex)
        {
            SGAC.BE.AL_PEDIDO objPEDIDO = new BE.AL_PEDIDO();

            objPEDIDO.pedi_iPedidoId = Convert.ToInt32(grdPedido.Rows[intIndex].Cells[0].Text);
            objPEDIDO.pedi_sOficinaConsularIdOrigen = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[1].Text);
            Session["OC Origen"] = grdPedido.Rows[intIndex].Cells[2].Text;
            objPEDIDO.pedi_sBovedaTipoIdOrigen = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[3].Text);
            objPEDIDO.pedi_sBodegaOrigenId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[4].Text);
            objPEDIDO.pedi_sOficinaConsularIdDestino = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[5].Text);
            Session["OC Destino"] = grdPedido.Rows[intIndex].Cells[6].Text;
            objPEDIDO.pedi_sBovedaTipoIdDestino = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[7].Text);
            objPEDIDO.pedi_sBodegaDestinoId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[8].Text);
            objPEDIDO.pedi_sPedidoTipoId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[9].Text);
            Session["Tipo"] = grdPedido.Rows[intIndex].Cells[10].Text;
            objPEDIDO.pedi_sPedidoMotivoId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[11].Text);
            Session["Motivo"] = grdPedido.Rows[intIndex].Cells[12].Text;
            objPEDIDO.pedi_cPedidoCodigo = grdPedido.Rows[intIndex].Cells[13].Text;
            objPEDIDO.pedi_sInsumoTipoId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[14].Text);
            Session["Insumo"] = grdPedido.Rows[intIndex].Cells[15].Text;
            objPEDIDO.pedi_dFechaRegistro = Comun.FormatearFecha(grdPedido.Rows[intIndex].Cells[16].Text);
            objPEDIDO.pedi_vActaRemision = grdPedido.Rows[intIndex].Cells[18].Text;
            objPEDIDO.pedi_vDescripcion = grdPedido.Rows[intIndex].Cells[19].Text;
            objPEDIDO.pedi_ICantidad = Convert.ToInt32(grdPedido.Rows[intIndex].Cells[20].Text);
            objPEDIDO.pedi_sEstadoId = Convert.ToInt16(grdPedido.Rows[intIndex].Cells[21].Text);
            Session["Estado"] = grdPedido.Rows[intIndex].Cells[22].Text;
            objPEDIDO.pedi_vObservacion = grdPedido.Rows[intIndex].Cells[23].Text;

            return objPEDIDO;
        }

        private SGAC.BE.AL_PEDIDOHISTORICO ValoresNuevosHistorico()
        {
            if (Session != null)
            {
                SGAC.BE.AL_PEDIDOHISTORICO objPEDIDOD = new BE.AL_PEDIDOHISTORICO();

                objPEDIDOD.pehi_sEstadoId = Convert.ToInt16(cboPedidoEstadoR.SelectedValue);
                objPEDIDOD.pehi_dFechaRegistro = Comun.FormatearFecha(dtpFecha.Text);
                objPEDIDOD.pehi_sMotivoId = Convert.ToInt16(cboPedidoMotivo.SelectedValue);
                objPEDIDOD.pehi_vObservacion = Page.Server.HtmlDecode(txtObservacion.Text);
                objPEDIDOD.pehi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPEDIDOD.pehi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPEDIDOD.pehi_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
                switch (enmAccion)
                {
                    case Enumerador.enmAccion.INSERTAR:
                        objPEDIDOD.pehi_iPedidoId = iPedidoId;
                        break;
                    case Enumerador.enmAccion.MODIFICAR:
                        objPEDIDOD.pehi_iPedidoId = Convert.ToInt32(Session[strVariablePedido]);
                        break;
                    case Enumerador.enmAccion.ELIMINAR:
                        objPEDIDOD.pehi_iPedidoId = Convert.ToInt32(Session[strVariablePedido]);
                        break;
                }
                return objPEDIDOD;
            }
            return null;
        }

        #endregion

        protected void cboTipoPed_SelectedIndexChanged(object sender, EventArgs e)
        { String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();
            
            if (cboTipoPed.SelectedValue.ToString() == ((int)Enumerador.enmPedidoTipo.PEDIDO_JEFATURA).ToString())
            {
                CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());                

                if ((bool)Session["oJefaturaFlag"]) //Cuando es una jefatura, el destino es la misma oficina, caso contrario, el destino es la oficina de referencia.
                {
                    CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                    cboMisionConsO.Enabled = true;
                    cboBovedaO.Enabled = true;
                }
                else
                {
                    CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());
                }
            }
            else if (cboTipoPed.SelectedValue.ToString() == ((int)Enumerador.enmPedidoTipo.PEDIDO_LIMA).ToString())
            {
                CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Constantes.CONST_OFICINACONSULAR_LIMA.ToString());                

                if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() == Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
                {
                    cboMisionConsO.Enabled = true;
                    cboBovedaO.Enabled = true;
                }
                else
                {
                    cboMisionConsO.Enabled = false;
                    cboBovedaO.Enabled = false;
                }
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

            for(int i=0;i<grdAlmacenUniversal.Rows.Count;i++)
                {
                DataRow dr;
                GridViewRow row = grdAlmacenUniversal.Rows[i];
                dr = dt.NewRow();
                    for (int x = 0; x < row.Cells.Count; x++)
                    {
                        strcelda =  HttpUtility.HtmlDecode(row.Cells[x].Text);

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
