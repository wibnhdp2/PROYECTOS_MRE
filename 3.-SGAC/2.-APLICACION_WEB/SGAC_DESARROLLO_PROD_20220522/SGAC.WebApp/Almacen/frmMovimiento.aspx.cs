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

using System.Drawing;
using SGAC.Almacen.BL;

namespace SGAC.WebApp.Almacen
{
    public partial class frmMovimientos : MyBasePage
    {

        #region VARIABLES
        private int intMovimientoEstado;
        private int intPasaPedidoMovimiento;
        private Int64 iMovimientoId = 0;
        private Int64 iActuacionId = 0;
        private Int64 iActuaciondetalleId = 0;

        MovimientoConsultaBL oMovimientoConsultaBL = new MovimientoConsultaBL();
        MovimientoDetalleConsultaBL oMovimientoDetalleConsultaBL = new MovimientoDetalleConsultaBL();
        PedidoConsultaBL oPedidoConsultaBL = new PedidoConsultaBL();
        MovimientoMantenimientoBL oMovimientoMantenimientoBL = new MovimientoMantenimientoBL();
        InsumoMantenimientoBL oInsumoMantenimientoBL = new InsumoMantenimientoBL();

        #endregion

        #region CAMPOS
        private string strNombreEntidad = "MOVIMIENTO";
        private string strVariableAccion = "Movimiento_Accion";
        private string strVariableMovimiento = "Movimiento_Id";
        private Enumerador.enmAccion enmAccion;

        private bool bEsLima;
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
            ocultarFechaValija.Visible = false;
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();
            Session["Formatofecha"] = strFormatoFechas;


            if (Session["PasaPedidoMovimiento"] != null)
            {
                if (Session["PasaPedidoMovimiento"].ToString() == "1")
                {
                    //Se ocultan los botones de aceptar y rechazar cuando se atiende un pedido.
                    btnAceptado.Visible = false;
                    btnRechazado.Visible = false;
                }

                intPasaPedidoMovimiento = Convert.ToInt32(Session["PasaPedidoMovimiento"]);
            }
            else
            {
                Session["PasaPedidoMovimiento"] = 0;
            }


            grdMovimiento.Columns[ObtenerIndiceColumnaGrilla(grdMovimiento, "Editar")].Visible = false;

            //Configuración ctrlToolBar1
            ctrlToolBar1.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar1_btnBuscarHandler);
            ctrlToolBar1.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar1_btnCancelarHandler);

            ctrlToolBar1.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBar1.btnCancelar.Text = "    Limpiar";
            ctrlToolBar1.btnImprimir.Visible = false;
            //Configuración ctrlToolBar2
            ctrlToolBar2.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBar2_btnNuevoHandler);
            ctrlToolBar2.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar2_btnCancelarHandler);
            ctrlToolBar2.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBar2_btnEditarHandler);
            ctrlToolBar2.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBar2_btnGrabarHandler);
            ctrlToolBar2.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBar2_btnEliminarHandler);
        

            btnAceptado.OnClientClick = "return ActivarModal()";
            btnRechazado.OnClientClick = "return ActivarModal()";

            Comun.CargarPermisos(Session, ctrlToolBar1, ctrlToolBar2, grdMovimiento, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            ctrlToolBar2.btnGrabar.OnClientClick = "return ValidarRegistro()";
            ctrlToolBar2.btnEliminar.OnClientClick = "return ActivarModal()";

            cboMisionConsConsulta.AutoPostBack = true;
            cboMisionConsConsulta.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
            
            cboMisionConsO.AutoPostBack = true;
            cboMisionConsO.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsularO_SelectedIndexChanged);

            cboMisionConsD.AutoPostBack = true;
            cboMisionConsD.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsularD_SelectedIndexChanged);

            dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            dtpFecInicio.EndDate = new DateTime(3000, 1, 1);

            dtpFecFin.StartDate = new DateTime(1900, 1, 1);
            dtpFecFin.EndDate = new DateTime(3000, 1, 1);


            //this.dtpFechaValija.StartDate = new DateTime(1900, 1, 1);
            //this.dtpFechaValija.EndDate = new DateTime(3000, 1, 1);


            dtpFechaValija.AllowFutureDate = true;

            this.dtpFechaValija.StartDate = Comun.FormatearFecha((Comun.ObtenerFechaActualTexto(Session)));




            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() ==
                        ((int)Constantes.CONST_OFICINACONSULAR_LIMA).ToString())
            {
                bEsLima = true;
            }

            #region Inicializar Estados de Controles

            // ADMINISTRADOR y SUPERADMIN
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                btnAdicionar.Enabled = false;
                btnCancelar.Enabled = false;

                txtRanIni.Enabled = false;
                txtRanFin.Enabled = false;
                txtCant.Enabled = false;
                txtObservacion.Enabled = false;

                ctrlToolBar2.btnNuevo.Enabled = false;
            }
            else
            {
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString() ==
                    Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
                {
                    cboMisionConsConsulta.Enabled = true;
                }

                cboTipoBovedaConsulta.Enabled = true;
                cboBovedaConsulta.Enabled = true;

                ctrlToolBar2.btnNuevo.Enabled = true;
            }

            #endregion


            //string strFormatofecha = "";
            //strFormatofecha = WebConfigurationManager.AppSettings["FormatoFechas"];
            //Session["Formatofecha"] = strFormatofecha;




            if (!IsPostBack)
            {
                llenarBovedas();
                //Se obtiene si el usuario es de jefatura
                object oJefaturaFlag = cboMisionConsO.EsOficinaJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                Session["oJefaturaFlag"] = bool.Parse(oJefaturaFlag.ToString());

                DatosPedido.Visible = false;
                lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);


                CargarListados();
                HabilitaCampos();
                LimpiaCampos();//si hay error deshabilitar


                cboMovimientoEstado.SelectedValue = Convert.ToInt16(Enumerador.enmMovimientoEstado.REGISTRADO).ToString();

                Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
                Session[strVariableMovimiento] = 0;
                Session["dt"] = null;


                if (intPasaPedidoMovimiento == 1)
                {
                    #region Carga Datos Pedido

                    lblPedActa.Text = Session["Pedido_Acta"].ToString();
                    lblPedCantidad.Text = Session["Pedido_Cantidad"].ToString();
                    lblPedEstado.Text = Session["Pedido_Estado"].ToString();
                    lblPedFecha.Text = Session["Pedido_Fecha"].ToString();
                    lblPedInsumo.Text = Session["Pedido_Insumo"].ToString();
                    lblPedMotivo.Text = Session["Pedido_Motivo"].ToString();
                    lblPedCodigo.Text = Session["Pedido_Codigo"].ToString();
                    txtNumeroPedido.Text = Session["Pedido_Codigo"].ToString();

                    cboMovimientoTipo.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoTipo.SALIDA).ToString();
                    cboMovimientoTipo.Enabled = false;
                    cboMovimientoMotivo.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString();
                    cboMovimientoMotivo.Enabled = false;
                    cboMovimientoEstado.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoEstado.REGISTRADO).ToString();
                    cboInsumoR.SelectedValue = Session["Pedido_InsumoTipo"].ToString();
                    cboInsumoR.Enabled = false;
                    //dtpFecha.Text = DateTime.Parse(Session["Pedido_Fecha"].ToString()).ToString(strFormatoFechas);
                    dtpFecha.Text = DateTime.Today.ToString(strFormatoFechas);
                    cboMisionConsO.SelectedValue = Session["OC_PEDI_ORIGEN"].ToString();
                    CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Session["OC_PEDI_ORIGEN"].ToString());

                    cboMisionConsD.SelectedValue = Session["OC_PEDI_DESTINO"].ToString();
                    CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Session["OC_PEDI_DESTINO"].ToString());

                    string scriptMover = @"$(function(){{
                                        MoveTabIndex(1);
                                   }});";
                    scriptMover = string.Format(scriptMover);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

                    DatosPedido.Visible = true;

                    TraePefijoNumeracion();
                    TraeRangosDisponibles();

                    #endregion
                }

                // Inicializar
                Session["rango_seleccionado"] = -1;
                Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    ctrlToolBar2.btnGrabar.Enabled = true;
                }



                ctrlToolBar2.btnEditar.Enabled = false;
                ctrlToolBar2.btnEliminar.Enabled = false;
                btnAceptado.Visible = false;
                btnRechazado.Visible = false;
                ctrlToolBar2.btnConfiguration.Enabled = false;
                updConsulta.Update();
                updGrillaConsulta.Update();                
            }

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]).ToString() == ((int)Enumerador.enmTipoRol.OPERATIVO).ToString())
            {
                cboMovimientoTipo.Enabled = false;
                cboMovimientoMotivo.Enabled = false;
            }

            //***********************************************
            // Fecha: 28/11/2019
            // Autor: Miguel Márquez Beltrán
            // Motivo: Asignar por defecto: Autoadhesivo
            //          y deshabilitar el control.
            //***********************************************
            Comun.SeleccionarItem(cboInsumoR, "AUTOADHESIVO");

            cboInsumoR_SelectedIndexChanged(this, null);
            cboInsumoR.Enabled = false;
            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;
            //***********************************************

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBar2.btnNuevo, ctrlToolBar2.btnGrabar, ctrlToolBar2.btnEliminar, btnAceptado, btnAdicionar, btnRechazado };
                Comun.ModoLectura(ref arrButtons);                
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                CargarOficinaConsular(cboMisionConsConsulta, cboTipoBovedaConsulta, cboBovedaConsulta, cboMisionConsConsulta.SelectedValue, string.Empty);
            else
                CargarOficinaConsular(cboMisionConsConsulta, cboTipoBovedaConsulta, cboBovedaConsulta, cboMisionConsConsulta.SelectedValue, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString(), true);
        }

        void ddlOficinaConsularO_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue, string.Empty);
            TraeRangosDisponibles();

            grdRangos.DataSource = new DataTable();
            grdRangos.DataBind();

            Session["dt"] = null;

        }

        void ddlOficinaConsularD_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, cboMisionConsD.SelectedValue, string.Empty);
        }


        protected void grdMovimiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sEstadoId")].Text.Trim() !=
                        Convert.ToInt32(Enumerador.enmMovimientoEstado.REGISTRADO).ToString())
                    {
                        e.Row.FindControl("btnEditar").Visible = false;
                        e.Row.FindControl("btnAtender").Visible = false;
                    }

                    if (e.Row.Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Fecha")].Text.Trim() != "&nbsp;")
                        e.Row.Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Fecha")].Text = (Comun.FormatearFecha(e.Row.Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Fecha")].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

                    ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;

                    ImageButton btnAtender = e.Row.FindControl("btnAtender") as ImageButton;


                    if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
                    {
                        ImageButton[] arrImageButtons = { btnEditar, btnAtender };
                        Comun.ModoLectura(ref arrImageButtons);
                    }
                }
            }
        }

        protected void grdMovimiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intIndex = Convert.ToInt32(e.CommandArgument);
            int intSeleccionado = Convert.ToInt32(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "MovimientoId")].Text);

            Session[strVariableMovimiento] = intSeleccionado;
            Session["dt"] = null;
            Session["dtDetalleMovimientoModificar"] = null;

            if (intSeleccionado > -1)
            {

                Util.CargarParametroDropDownList(cboMovimientoTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_MOVIMIENTO), true);

                cboMisionConsD.Enabled = false;
                cboTipoBovedaD.Enabled = false;
                cboBovedaD.Enabled = false;

                Session["PasaPedidoMovimiento"] = "0";
                DatosPedido.Visible = false;

                string strDescripcionEstado = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Estado")].Text;


                if (e.CommandName == "Atender" || e.CommandName == "Editar" || e.CommandName == "Ver")
                {
                    SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = ValoresSeleccionCabecera(intIndex);

                    DesHabilitaCampos();

                    Session["rango_seleccionado"] = -1;




                    if (e.CommandName == "Ver")
                    {
                        TraeDetalleMovimiento(intIndex);


                        #region Consultar
                        Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                        PintarCabecera(objMOVIMIENTO);

                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);

                        #endregion
                    }
                    else
                    {

                        #region Validacion



                        if (objMOVIMIENTO.movi_sEstadoId.ToString() == Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString() ||
                                objMOVIMIENTO.movi_sEstadoId.ToString() == Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString() ||
                                objMOVIMIENTO.movi_sEstadoId.ToString() == Convert.ToInt32(Enumerador.enmMovimientoEstado.RECHAZADO).ToString())
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Movimiento (" + objMOVIMIENTO.movi_cMovimientoCodigo + ") porque se encuentra " + strDescripcionEstado));
                            return;
                        }


                        #endregion

                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                        if (e.CommandName == "Atender")
                        {
                            #region Atender

                            if (objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() != ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString() &&
                                objMOVIMIENTO.movi_sOficinaConsularIdDestino.ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
                            {

                            }

                            TraeDetalleMovimiento(intIndex);

                            Session[strVariableAccion] = Enumerador.enmAccion.ATENDER;
                            PintarCabecera(objMOVIMIENTO);

                            btnAceptado.Visible = true;
                            btnRechazado.Visible = true;
                            ctrlToolBar2.btnEliminar.Enabled = true;

                            if (objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() != ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString() &&
                                objMOVIMIENTO.movi_sOficinaConsularIdDestino.ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
                            {

                                btnAceptado.Visible = false;
                                btnRechazado.Visible = false;
                            }

                            if (objMOVIMIENTO.movi_sOficinaConsularIdOrigen.ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
                            {
                                ctrlToolBar2.btnEliminar.Enabled = false;
                            }

                            #endregion
                        }

                        else if (e.CommandName == "Editar")
                        {

                            #region Editar

                            TraeDetalleMovimiento(intIndex);
                            Session["dtDetalleMovimientoModificar"] = ((DataTable)Session["dt"]).Copy();

                            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                            HabilitaCampos();

                            #region Movimientos por Pedidos Atendidos

                            if (objMOVIMIENTO.movi_sMovimientoTipoId.ToString() == ((int)Enumerador.enmMovimientoTipo.SALIDA).ToString() &&
                                objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString())
                            {
                                Session["PasaPedidoMovimiento"] = "1";
                                ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);

                                cboInsumoR.Enabled = false;
                                cboMovimientoTipo.Enabled = false;
                                cboMovimientoMotivo.Enabled = false;
                            }

                            #endregion

                            PintarCabecera(objMOVIMIENTO);

                            btnAceptado.Visible = false;
                            btnRechazado.Visible = false;

                            #endregion
                        }


                    }


                    LblTotal.Text = "Total de " + cboInsumoR.SelectedItem.Text + " : " + CalculaTotal() + "   ";

                    Comun.EjecutarScript(Page, strScript);

                    grdRangos.Visible = true;

                    LblTotal.Visible = true;
                    lblNroMovimiento.Visible = true;
                    lblNroMovimientoEtiqueta.Visible = true;

                    UpdMantenimiento.Update();
                    updRango.Update();

                }
            }
        }

        private void CargarComboMovimientoMotivo()
        {
            // Todos los movimientos
            Util.CargarParametroDropDownList(cboMovimientoTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_MOVIMIENTO), true);

            // Segun seleccionado se carga el motivo
            Util.CargarParametroDropDownList(cboMovimientoMotivo, new DataTable(), true);
        }

        protected void grdRangos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intIndexD = Convert.ToInt32(e.CommandArgument);
            int intMovimientoId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "movi_IMovimientoId")].Text);
            int intMovimientoDetId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iMovimientoDetalleId")].Text);

            if (intMovimientoId > -1)
            {
                if (e.CommandName == "Editar")
                {   //Muestra (intSeleccionado)
                    Session["rango_seleccionado"] = intIndexD;
                    SGAC.BE.AL_MOVIMIENTODETALLE objMOVIMIENTOD = ValoresSeleccionDetalle(intIndexD, intMovimientoId, intMovimientoDetId);
                    PintarDetalle(objMOVIMIENTOD);
                    grdRangos.Enabled = true;
                    updRango.Update();
                }
                else if (e.CommandName == "Eliminar")
                {
                    DataTable dt = null;
                    Session["rango_seleccionado"] = -1;
                    if (intMovimientoDetId > 0)
                    {
                        dt = ((DataTable)Session["dt"]);

                        dt.Rows[intIndexD]["mode_sEstadoId"] = (int)Enumerador.enmMovimientoEstado.ANULADO;
                        grdRangos.Rows[intIndexD].Visible = false;
                    }
                    else
                    {
                        dt = ((DataTable)Session["dt"]).Copy();

                        dt.Rows[intIndexD].Delete();
                        dt.AcceptChanges();

                        if (grdRangos.Rows.Count == 1)
                        {
                            dt = Cabecera_detalle();
                        }

                        Session["dt"] = dt;
                        grdRangos.DataSource = dt;
                        grdRangos.DataBind();
                    }

                    LimpiaCamposRangos();
                }

                UpdMantenimiento.Update();
            }
        }

        private DataTable Cabecera_detalle()
        {
            DataTable dt = new DataTable("detalle");
            dt.Columns.Add("RangoIni", typeof(string));
            dt.Columns.Add("RangoFin", typeof(string));
            dt.Columns.Add("Cant", typeof(string));
            dt.Columns.Add("Observacion", typeof(string));
            dt.Columns.Add("mode_sEstadoId", typeof(string));
            dt.Columns.Add("movi_IMovimientoId", typeof(string));
            dt.Columns.Add("mode_iMovimientoDetalleId", typeof(string));
            dt.Columns.Add("mode_iActuacionId", typeof(string));
            dt.Columns.Add("mode_iActuaciondetalleId", typeof(string));
            dt.Columns.Add("mode_cEstado", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            return dt;
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            // Obtener indice Seleccionado de la grilla de Rangos
            if (txtRanIni.Text == "" || txtRanFin.Text == "" || txtRanIni.Text == "0" || txtRanFin.Text == "0" /*|| txtObservacion.Text.Trim().ToUpper() == ""*/
                /*|| txtNroActa.Text == ""*/ || cboInsumoR.SelectedItem.Text == "- SELECCIONAR -"
                || cboMovimientoTipo.SelectedItem.Text == "- SELECCIONAR -" || cboMovimientoMotivo.SelectedItem.Text == "- SELECCIONAR -"
                || cboBovedaO.SelectedItem.Text == "- SELECCIONAR -" || cboBovedaD.SelectedItem.Text == "- SELECCIONAR -")
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Ingresar datos principales y rango."));
            }
            else
            {
                int intRangoSel = Convert.ToInt32(Session["rango_seleccionado"]);
                int intRanIni = Convert.ToInt32(txtRanIni.Text);
                int intRanFin = Convert.ToInt32(txtRanFin.Text);

                if (grdRangos.Rows.Count == 1)// Se validara que solo se pueda ingresar un insumo a la vez 25/04/2016
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo se puede realizar un movimiento de insumo a la vez."));
                    return;
                }

                #region VALIDA RangoFinal>RangoInicial
                if (intRanFin < intRanIni)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Rango no válido."));
                    return;
                }

                CalculaRangos();

                int intOficinaConsularId = 0, intMotivoId = 0, intRangoInicial = 0, intRangoFinal = 0, intTipoInsumo = 0, intResultado = 0;
                int intTipoBovedaId = 0, intBovedaId = 0;
                string strMensaje = "";

                if (cboMisionConsO.SelectedValue != null)
                    intOficinaConsularId = Convert.ToInt32(cboMisionConsO.SelectedValue);

                if (cboMovimientoMotivo.SelectedValue != null)
                {
                    if (cboMovimientoMotivo.SelectedIndex > 0)
                        intMotivoId = Convert.ToInt32(cboMovimientoMotivo.SelectedValue);
                }

                if (cboInsumoR.SelectedValue != null)
                    intTipoInsumo = Convert.ToInt32(cboInsumoR.SelectedValue);

                intRangoInicial = Convert.ToInt32(txtRanIni.Text);
                intRangoFinal = Convert.ToInt32(txtRanFin.Text);
                intResultado = 0;
                strMensaje = "Mensaje";
                if (cboTipoBovedaO.SelectedValue != null)

                    intTipoBovedaId = Convert.ToInt32(cboTipoBovedaO.SelectedValue);

                if (cboBovedaO.SelectedValue != null)
                    intBovedaId = Convert.ToInt32(cboBovedaO.SelectedValue);

                if (intTipoBovedaId == Convert.ToInt32(Enumerador.enmBovedaTipo.MISION))
                {
                    intBovedaId = intOficinaConsularId;
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] == Enumerador.enmAccion.MODIFICAR)
                {

                    List<int[]> listRangos = new List<int[]>();
                    bool bVerificarRangos = true;

                    if (Session["dtDetalleMovimientoModificar"] != null)
                    {
                        int RangoInicioAux = intRangoInicial;
                        int RangoFinalAux = intRangoFinal;

                        int[] Rangos = new int[2];

                        DataView dv = ((DataTable)Session["dtDetalleMovimientoModificar"]).DefaultView;
                        dv.Sort = "Rango Ini desc";

                        foreach (DataRow dr in dv.ToTable().Rows)
                        {


                            if (listRangos.Count > 0)
                            {
                                RangoInicioAux = listRangos[listRangos.Count - 1][0];
                                RangoFinalAux = listRangos[listRangos.Count - 1][1];
                            }


                            if (RangoInicioAux > int.Parse(dr["Rango Fin"].ToString()) ||
                                RangoFinalAux < int.Parse(dr["Rango Ini"].ToString()))
                            {
                                listRangos.Add(new int[] 
                            { 
                                RangoInicioAux, 
                                RangoFinalAux
                            });
                            }
                            else if (RangoInicioAux >= int.Parse(dr["Rango Ini"].ToString()) &&
                                RangoFinalAux <= int.Parse(dr["Rango Fin"].ToString()))
                            {
                                bVerificarRangos = false;
                                break;
                            }
                            else if (RangoInicioAux < int.Parse(dr["Rango Ini"].ToString()))
                            {
                                listRangos.Add(new int[] 
                            { 
                                RangoInicioAux, 
                                int.Parse(dr["Rango Ini"].ToString())-1 
                            });

                                if (RangoFinalAux < int.Parse(dr["Rango Fin"].ToString()))
                                {
                                    listRangos.Add(new int[] 
                                { 
                                    int.Parse(dr["Rango Ini"].ToString())+1, 
                                    RangoFinalAux 
                                });
                                }
                                else
                                {
                                    listRangos.Add(new int[] 
                                { 
                                    RangoInicioAux+1, 
                                    int.Parse(dr["Rango Fin"].ToString())-1
                                });

                                    if (RangoFinalAux > int.Parse(dr["Rango Fin"].ToString()))
                                    {
                                        listRangos.Add(new int[] 
                                        { 
                                            int.Parse(dr["Rango Fin"].ToString())+1, 
                                            RangoFinalAux
                                        });
                                    }
                                }
                            }
                            else
                            {
                                int RanIni = 0;

                                if (RangoInicioAux == int.Parse(dr["Rango Ini"].ToString()))
                                    RanIni = int.Parse(dr["Rango Ini"].ToString()) + 1;
                                else
                                    RanIni = RangoInicioAux;

                                if (RangoFinalAux < int.Parse(dr["Rango Fin"].ToString()))
                                {
                                    listRangos.Add(new int[] 
                                { 
                                    RanIni, 
                                    RangoFinalAux 
                                });
                                }
                                else
                                {
                                    listRangos.Add(new int[] 
                                { 
                                    RanIni, 
                                    int.Parse(dr["Rango Fin"].ToString())-1
                                });

                                    if (RangoFinalAux > int.Parse(dr["Rango Fin"].ToString()))
                                    {
                                        listRangos.Add(new int[] 
                                    { 
                                        int.Parse(dr["Rango Fin"].ToString())+1, 
                                        RangoFinalAux
                                    });
                                    }
                                }

                            }

                        }
                    }

                    if (bVerificarRangos)
                    {

                        foreach (int[] intRango in listRangos)
                        {

                            DataTable dtR = oMovimientoConsultaBL.ValidaRangosDetalle(intOficinaConsularId, intMotivoId, intTipoInsumo, intRango[0], intRango[1], ref intResultado, ref strMensaje, intTipoBovedaId, intBovedaId);

                            if (intResultado < 0 || intResultado > 0)
                            {
                                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, strMensaje));
                                TraeRangosDisponibles();
                                return;
                            }
                        }

                    }

                }
                else
                {

                    DataTable dtR = oMovimientoConsultaBL.ValidaRangosDetalle(intOficinaConsularId, intMotivoId, intTipoInsumo, intRangoInicial, intRangoFinal, ref intResultado, ref strMensaje, intTipoBovedaId, intBovedaId);

                    if (intResultado < 0 || intResultado > 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, strMensaje));
                        TraeRangosDisponibles();
                        return;
                    }
                }


                #endregion

                bool bolModifca = true;
                int intRanIniComparacion = 0;
                int intRanFinComparacion = 0;

                // INSERTAR                
                if (intRangoSel == -1)
                {
                    int intTotalPermitido = Convert.ToInt32(ConfigurationManager.AppSettings["CantidadRegistrosMax"]);
                    int Total = CalculaTotal();
                    int TotalRango = Convert.ToInt32(txtCant.Text);

                    if (Total >= intTotalPermitido || TotalRango > intTotalPermitido)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Máximo permitido: " + intTotalPermitido + " registros "));
                    }
                    else
                    {
                        #region Insertar RAngo
                        int intInserta = 0;

                        if (Session["dt"] == null)
                        {
                            DataTable dt = filldata();
                            DataRow Row1;
                            Row1 = dt.NewRow();
                            Row1["RangoIni"] = this.txtRanIni.Text;
                            Row1["RangoFin"] = this.txtRanFin.Text;
                            Row1["Cant"] = this.txtCant.Text;
                            Row1["Observacion"] = this.txtObservacion.Text.Trim().ToUpper();
                            Row1["mode_sEstadoId"] = Convert.ToInt32(Enumerador.enmMovimientoEstado.REGISTRADO);
                            Row1["movi_IMovimientoId"] = iMovimientoId;
                            Row1["mode_iMovimientoDetalleId"] = 0;
                            Row1["mode_iActuacionId"] = iActuacionId;
                            Row1["mode_iActuaciondetalleId"] = iActuaciondetalleId;
                            Row1["Estado"] = Convert.ToString(Enumerador.enmRangoEstado.NUEVO);
                            dt.Rows.Add(Row1);
                            dt.AcceptChanges();
                            grdRangos.DataSource = dt;
                            grdRangos.DataBind();
                            Session["dt"] = dt;
                            UpdMantenimiento.Update();
                        }
                        else
                        {
                            DataTable dt = (Session["dt"]) as DataTable;
                            DataRow Row1;
                            Row1 = dt.NewRow();
                            int reg = dt.Rows.Count;

                            for (int r = 0; r < reg; r++)
                            {
                                intInserta = VerificaRangos(intRanIni.ToString(), intRanFin.ToString());
                                if (intInserta != 0)
                                {
                                    break;
                                }
                            }

                            if (intInserta == 0)
                            {
                                Row1["RangoIni"] = this.txtRanIni.Text;
                                Row1["RangoFin"] = this.txtRanFin.Text;
                                Row1["Cant"] = this.txtCant.Text;
                                Row1["Observacion"] = this.txtObservacion.Text.Trim().ToUpper();
                                Row1["mode_sEstadoId"] = this.cboMovimientoEstado.SelectedValue;
                                Row1["movi_IMovimientoId"] = Convert.ToInt32(Session[strVariableMovimiento]);
                                Row1["mode_iMovimientoDetalleId"] = 0;
                                Row1["mode_iActuacionId"] = iActuacionId;
                                Row1["mode_iActuaciondetalleId"] = iActuaciondetalleId;
                                Row1["Estado"] = Convert.ToString(Enumerador.enmRangoEstado.NUEVO);
                                dt.Rows.Add(Row1);
                                dt.AcceptChanges();
                                grdRangos.DataSource = dt;
                                grdRangos.DataBind();
                                Session["dt"] = dt;

                                UpdMantenimiento.Update();
                            }
                            else
                            {
                                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Rango ya registrado"));
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    //MODIFICA
                    #region Modifica Rango
                    // VALIDO
                    DataTable dt = (Session["dt"]) as DataTable;
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        // No debe verificar con el RANGO a ser modificado
                        if (r != intRangoSel)
                        {
                            intRanIniComparacion = Convert.ToInt32(dt.Rows[r]["Rango Ini"]);
                            intRanFinComparacion = Convert.ToInt32(dt.Rows[r]["Rango Fin"]);

                            if (intRanIni >= intRanIniComparacion && intRanIni <= intRanFinComparacion ||
                                intRanFin >= intRanIniComparacion && intRanFin <= intRanFinComparacion)
                            {
                                bolModifca = false;
                                break;
                            }
                        }
                    }
                    #endregion

                    if (bolModifca)
                    {
                        dt.Rows[intRangoSel]["RangoIni"] = intRanIni;
                        dt.Rows[intRangoSel]["RangoFin"] = intRanFin;
                        dt.Rows[intRangoSel]["Cant"] = intRanFin - intRanIni + 1;
                        dt.Rows[intRangoSel]["Observacion"] = txtObservacion.Text.Trim().ToUpper();
                        dt.AcceptChanges();
                        grdRangos.DataSource = dt;
                        grdRangos.DataBind();
                        Session["dt"] = dt;
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Rango ya registrado"));
                    }
                }

                // Asignar sin seleccón
                Session["rango_seleccionado"] = -1;
                LimpiaCamposRangos();

                UpdMantenimiento.Update();
            }

            grdRangos.Visible = true;
            grdRangos.Enabled = true;
            updRango.Update();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiaCamposRangos();
        }

        protected void btnAceptado_Click(object sender, EventArgs e)
        {
            string strMovimientoCodigo = lblNroMovimiento.Text;
            string strMovimientoEstado = cboMovimientoEstado.SelectedValue;
            string strDescripcionEstado = cboMovimientoEstado.SelectedItem.Text;


            if (cboTipoBovedaD.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.USUARIO).ToString())
            {
                if (!(cboBovedaD.SelectedValue.ToString() == Session[Constantes.CONST_SESION_USUARIO_ID].ToString()
                    || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_ADMINISTRADOR_CONSULADO_SCI"
                    || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_ADMINISTRADOR"))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "El movimiento solo puede ser Aceptado o Rechazado por el usuario asignado o con el rol de Administrador."));
                    return;
                }
            }



            if (strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString() || strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString() || strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.RECHAZADO).ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Movimiento (" + strMovimientoCodigo + ") porque se encuentra " + strDescripcionEstado));
            }
            else
            {
                if (grdRangos.Rows.Count > 0)
                {
                    intMovimientoEstado = Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO);

                    if (grdRangos.Rows.Count > 0)
                    {
                        GrabarDatosAceptar();
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No existen rangos en detalle de movimiento"));
                    }
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No existen rangos en detalle de movimiento"));
                }
            }

            ctrlToolBar1_btnBuscarHandler();
        }

        protected void btnRechazado_Click(object sender, EventArgs e)
        {

            if (cboTipoBovedaD.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.USUARIO).ToString())
            {
                if (!(cboBovedaD.SelectedValue.ToString() == Session[Constantes.CONST_SESION_USUARIO_ID].ToString()
                    || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_ADMINISTRADOR_CONSULADO_SCI"
                    || Session[Constantes.CONST_SESION_USUARIO_ROL].ToString() == "SGAC_ADMINISTRADOR"))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "El movimiento solo puede ser Aceptado o Rechazado por el usuario asignado o con el rol de Administrador."));
                    return;
                }
            }

            string strMovimientoCodigo = lblNroMovimiento.Text;
            string strMovimientoEstado = cboMovimientoEstado.SelectedValue;
            string strDescripcionEstado = cboMovimientoEstado.SelectedItem.Text;

            if (strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString() || strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString() || strMovimientoEstado == Convert.ToInt32(Enumerador.enmMovimientoEstado.RECHAZADO).ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Movimiento (" + strMovimientoCodigo + ") porque se encuentra " + strDescripcionEstado));
            }
            else
            {
                if (grdRangos.Rows.Count > 0)
                {
                    intMovimientoEstado = Convert.ToInt32(Enumerador.enmMovimientoEstado.RECHAZADO);

                    if (grdRangos.Rows.Count > 0)
                    {
                        GrabarDatosRechazar();
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No existen rangos en detalle de movimiento"));
                    }
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No existen rangos en detalle de movimiento"));
                }
            }

            ctrlToolBar1_btnBuscarHandler();
        }

        protected void txtRanIni_TextChanged(object sender, EventArgs e)
        {
            CalculaRangos();
        }

        protected void txtRanFin_TextChanged(object sender, EventArgs e)
        {
            CalculaRangos();
        }

        protected void cboTipoBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarBoveda(cboTipoBovedaO, cboBovedaO, cboMisionConsO.SelectedValue.ToString());
            cboBovedaO_SelectedIndexChanged(null, null);

            if (!bEsLima)
            {
                if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                {
                    if (cboTipoBovedaO.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmBovedaTipo.USUARIO).ToString())
                    {
                        ProcesarEstadosCombosOrigenDestino(false, true, true, false, false, false);
                        CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
                    }
                    else
                    {
                        CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID]).ToString());
                        ProcesarEstadosCombosOrigenDestino(false, true, false, false, false, false);
                    }
                }
            }


            grdRangos.DataSource = new DataTable();
            grdRangos.DataBind();

            Session["dt"] = null;
        }

        protected void cboTipoBovedaD_SelectedIndexChanged(object sender, EventArgs e)
        {

            CargarBoveda(cboTipoBovedaD, cboBovedaD, cboMisionConsD.SelectedValue.ToString());

        }

        protected void cboMovimientoMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {

            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
            CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());



            if (cboMovimientoMotivo.SelectedValue.ToString() != "- SELECCIONAR -")
            {


                if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.CARGA_INICIAL).ToString())
                {
                    ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);
                }
                else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString())
                {
                    ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, true);
                    cboTipoBovedaD.SelectedValue = ((int)Enumerador.enmBovedaTipo.USUARIO).ToString();
                    cboTipoBovedaD_SelectedIndexChanged(null, null);
                }
                else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                {
                    if (bEsLima)
                    {
                        ProcesarEstadosCombosOrigenDestino(false, false, true, false, false, false);
                        cboTipoBovedaO.SelectedValue = ((int)Enumerador.enmBovedaTipo.USUARIO).ToString();

                    }
                    else
                    {
                        ProcesarEstadosCombosOrigenDestino(false, true, true, false, false, false);
                    }
                    cboTipoBovedaO_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);
            }

            TraeRangosDisponibles();
            grdRangos.DataSource = new DataTable();
            grdRangos.DataBind();

            Session["dt"] = null;

        }

        protected void cboInsumoR_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblTotal.Visible = false;
            TraePefijoNumeracion();
            TraeRangosDisponibles();
        }

        protected void cboMovimientoTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMovimientoTipo.SelectedValue.ToString() != "0")
                {
                    CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
                    CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());

                    llenar_cboMovimientoMotivo();

                    grdRangos.DataSource = new DataTable();
                    grdRangos.DataBind();

                    Session["dt"] = null;
                    ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);

                }
                else
                {
                    LimpiaCampos();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cboBovedaO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBovedaO.SelectedItem.Text != "- SELECCIONAR -")
            {
                TraeRangosDisponibles();
            }
            else
            {
                grdRangosDisponibles.DataSource = new DataTable();
                grdRangosDisponibles.DataBind();

                LblTotalInsumos.Text = "0";
            }

            grdRangos.DataSource = new DataTable();
            grdRangos.DataBind();

            Session["dt"] = null;
        }

        protected void grdRangos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LblTotal.Text = "Total de " + cboInsumoR.SelectedItem.Text + " : " + CalculaTotal() + "   ";

            if (grdRangos.Rows.Count > 0)
            {
                if (e.Row.Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_sEstadoId")].Text == Convert.ToInt16(Enumerador.enmMovimientoEstado.ANULADO).ToString())
                    e.Row.Visible = false;
            }
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;

            ImageButton btnEliminarDetalle = e.Row.FindControl("btnEliminarDetalle") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar, btnEliminarDetalle };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }


        protected void grdRangosDisponibles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int sumI = 0;
            for (int i = 0; i < grdRangosDisponibles.Rows.Count; i++)
            {
                sumI += int.Parse(grdRangosDisponibles.Rows[i].Cells[ObtenerIndiceColumnaGrilla(grdRangosDisponibles, "Cant")].Text);
            }
            LblTotalInsumos.Text = Convert.ToInt32(sumI).ToString();
        }

        protected void cboBovedaD_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        protected void cboTipoBovedaConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarBoveda(cboTipoBovedaConsulta, cboBovedaConsulta, cboMisionConsConsulta.SelectedValue.ToString());
        }

        protected void cboBovedaConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region MANTENIMIENTO
        void ctrlToolBar1_btnCancelarHandler()
        {
            HabilitaCampos();
            LimpiaCampos();
            grdMovimiento.DataSource = null;
            grdMovimiento.DataBind();
            ctrlPaginador.Visible = false;
            btnAceptado.Visible = false;
            btnRechazado.Visible = false;
            //***********************************************
            // Fecha: 28/11/2019
            // Autor: Miguel Márquez Beltrán
            // Motivo: Asignar por defecto: Autoadhesivo
            //          y deshabilitar el control.
            //***********************************************
            Comun.SeleccionarItem(cboInsumoR, "AUTOADHESIVO");

            cboInsumoR_SelectedIndexChanged(this, null);
            cboInsumoR.Enabled = false;

            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;
            //***********************************************

            UpdMantenimiento.Update();
        }

        void ctrlToolBar2_btnNuevoHandler()
        {
            String strFormatoFechas = String.Empty;


            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();



            Session["PasaPedidoMovimiento"] = 0;
            Session["rango_seleccionado"] = -1;
            Session["dt"] = null;
            intMovimientoEstado = Convert.ToInt32(Enumerador.enmMovimientoEstado.REGISTRADO);
            enmAccion = Enumerador.enmAccion.INSERTAR;

            Session["dtDetalleMovimientoModificar"] = null;

            TraePefijoNumeracion();
            HabilitaCampos();
            LimpiaCampos();
            cboMovimientoEstado.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoEstado.REGISTRADO).ToString();
            dtpFechaValija.Text = DateTime.Today.ToString(strFormatoFechas);
            txtHoraInicio.Text = "";

            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() !=
                ((int)Constantes.CONST_OFICINACONSULAR_LIMA).ToString())
            {
                ELiminarRegistroCombo(cboMovimientoTipo, ((int)Enumerador.enmMovimientoTipo.ENTRADA).ToString());// Se elimina el movimiento de entrada cuando es un usuario que no es de lima
            }

            // Validar botones...
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                ctrlToolBar2.btnGrabar.Enabled = true;
                btnAdicionar.Enabled = true;
                btnCancelar.Enabled = true;
            }
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            btnAceptado.Visible = false;
            btnRechazado.Visible = false;

            TraeRangosDisponibles();

            cboMovimientoMotivo.Items.Clear();
            cboMovimientoMotivo.Items.Insert(0, new ListItem("- SELECCIONAR -", "TODO"));

            grdRangos.DataSource = Cabecera_detalle();
            grdRangos.DataBind();
            DatosPedido.Visible = false;

            //***********************************************
            // Fecha: 28/11/2019
            // Autor: Miguel Márquez Beltrán
            // Motivo: Asignar por defecto: Autoadhesivo
            //          y deshabilitar el control.
            //***********************************************
            Comun.SeleccionarItem(cboInsumoR, "AUTOADHESIVO");

            cboInsumoR_SelectedIndexChanged(this, null);
            cboInsumoR.Enabled = false;
            //***********************************************
        }

        void ctrlToolBar2_btnCancelarHandler()
        {
            LimpiaCampos();
            LimpiaDatosPedido();
            LimpiaCamposRangos();
            HabilitaCampos();
            TraePefijoNumeracion();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                ctrlToolBar2.btnGrabar.Enabled = true;
            }
            else
            {
                ctrlToolBar2.btnNuevo.Enabled = true;
            }

            btnAdicionar.Enabled = false;
            btnCancelar.Enabled = false;
            ctrlToolBar2.btnConfiguration.Enabled = false;
            ctrlToolBar2_btnNuevoHandler();

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
            DatosPedido.Visible = false;

            //***********************************************
            // Fecha: 28/11/2019
            // Autor: Miguel Márquez Beltrán
            // Motivo: Asignar por defecto: Autoadhesivo
            //          y deshabilitar el control.
            //***********************************************
            Comun.SeleccionarItem(cboInsumoR, "AUTOADHESIVO");

            cboInsumoR_SelectedIndexChanged(this, null);
            cboInsumoR.Enabled = false;
            Comun.SeleccionarItem(cboInsumoC, "AUTOADHESIVO");
            cboInsumoC.Enabled = false;
            //***********************************************

            UpdMantenimiento.Update();
            updRango.Update();
        }

        void ctrlToolBar2_btnEditarHandler()
        {
            if (cboMovimientoEstado.SelectedValue == Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString() || cboMovimientoEstado.SelectedValue == Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Movimiento (" + lblNroMovimiento.Text + ") porque se encuentra " + cboMovimientoEstado.SelectedItem.Text));
            }
            else
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                ctrlToolBar2.btnEditar.Enabled = false;
                ctrlToolBar2.btnEliminar.Enabled = true;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    ctrlToolBar2.btnGrabar.Enabled = true;
                    btnAdicionar.Enabled = true;
                    btnCancelar.Enabled = true;
                }
                btnAceptado.Visible = true;
                btnRechazado.Visible = true;

                ctrlToolBar2.btnConfiguration.Enabled = false;
                HabilitaCampos();
                grdRangos.Enabled = true;
                updRango.Update();
            }
        }

        void ctrlToolBar2_btnGrabarHandler()
        {
            if (grdRangos.Rows.Count <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe ingresar los rangos"));
                return;
            }
            TextBox txt = (TextBox)dtpFechaValija.FindControl("TxtFecha");
            txt.BorderColor = Color.Gray;

            if (!dtpFechaValija.ToDateTime())
            {
                txt.BorderWidth = 1;
                txt.BorderStyle = BorderStyle.Solid;
                txt.BorderColor = Color.Red;

                string StrScript = string.Empty;
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Fecha de Valija no valida"));
                return;
            }


            if (cboTipoBovedaO.SelectedValue.ToString() == cboTipoBovedaD.SelectedValue.ToString() &&
                cboBovedaO.SelectedValue.ToString() == cboBovedaD.SelectedValue.ToString() &&
                cboMovimientoMotivo.SelectedValue.ToString() != ((int)Enumerador.enmMovimientoMotivo.CARGA_INICIAL).ToString())
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se puede realizar un movimiento con el mismo origen y destino."));
                return;
            }


            //====================modulo de verificacion de almacen
            #region verificacion_almacen

            int intOficinaConsularId = 0, intMotivoId = 0, intRangoInicial = 0, intRangoFinal = 0, intTipoInsumo = 0, intResultado = 0;
            int intTipoBovedaId = 0, intBovedaId = 0;
            string strMensaje = "";

            if (cboMisionConsO.SelectedValue != null)
                intOficinaConsularId = Convert.ToInt32(cboMisionConsO.SelectedValue);

            if (cboMovimientoMotivo.SelectedValue != null)
            {
                if (cboMovimientoMotivo.SelectedIndex > 0)
                    intMotivoId = Convert.ToInt32(cboMovimientoMotivo.SelectedValue);
            }

            if (cboInsumoR.SelectedValue != null)
                intTipoInsumo = Convert.ToInt32(cboInsumoR.SelectedValue);

            strMensaje = "Mensaje";
            if (cboTipoBovedaO.SelectedValue != null)

                intTipoBovedaId = Convert.ToInt32(cboTipoBovedaO.SelectedValue);

            if (cboBovedaO.SelectedValue != null)
                intBovedaId = Convert.ToInt32(cboBovedaO.SelectedValue);

            if (intTipoBovedaId == Convert.ToInt32(Enumerador.enmBovedaTipo.MISION))
            {
                intBovedaId = intOficinaConsularId;
            }

            intRangoInicial = Convert.ToInt32(grdRangos.Rows[0].Cells[0].Text);
            intRangoFinal = Convert.ToInt32(grdRangos.Rows[0].Cells[1].Text);

            DataTable dtR = oMovimientoConsultaBL.ValidaRangosDetalle(intOficinaConsularId, intMotivoId, intTipoInsumo, intRangoInicial, intRangoFinal, ref intResultado, ref strMensaje, intTipoBovedaId, intBovedaId);

            if (intResultado < 0 || intResultado > 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, strMensaje));
                TraeRangosDisponibles();
                return;
            }

            #endregion

            



            /*if (grdRangosDisponibles.Rows.Count >= 0)
            {
                int Rg_inicio = Convert.ToInt32(grdRangosDisponibles.Rows[0].Cells[0].Text);
                int RG_inicio_elegido = Convert.ToInt32(grdRangos.Rows[0].Cells[0].Text);
                int contador = 0;
                foreach (DataRow item in grdRangosDisponibles.Rows)
                {
                    if (Convert.ToInt32(item["Rango Inicio"].ToString()) == RG_inicio_elegido)
                    {
                        contador++;
                        break;
                    }
                }
                if (contador==0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "El rango inicial del insumo no coincida con el almacen."));
                    return;    
                }               
            }*/
            //======================================================


            if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString() ||
                cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
            {
                if (cboMisionConsO.SelectedValue.ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo puede realizar devolución de insumos de su oficina consular."));
                    return;
                }


                if (cboTipoBovedaO.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.USUARIO).ToString())
                {
                    if (!(cboMisionConsO.SelectedValue.ToString() == cboMisionConsD.SelectedValue.ToString() &&
                        cboTipoBovedaD.SelectedValue.ToString() == ((int)Enumerador.enmBovedaTipo.MISION).ToString()))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo puede realizar devolución a su oficina consular."));
                        return;
                    }


                }
                else
                {
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()) == 1)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo puede realizar devolución de usuario a oficina consular."));
                        return;
                    }

                    if (cboMisionConsO.EsPermitidoDestinoLima())
                    {
                        if (cboBovedaD.SelectedValue.ToString() != "1")
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo puede realizar devolución desde su oficina consular a Lima."));
                            return;
                        }
                    }
                    else if (!cboMisionConsO.EsDestinoMiJefatura(cboMisionConsD.SelectedValue.ToString()))
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Solo puede realizar devolución desde su oficina consular a su Jefatura."));
                        return;
                    }
                }
            }

            if ((intPasaPedidoMovimiento != 1 &&
                (Convert.ToInt32(cboMovimientoMotivo.SelectedValue) == (int)Enumerador.enmMovimientoMotivo.POR_PEDIDO &&
                    cboMisionConsO.SelectedValue.ToString() != Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString())) ||
                    (Convert.ToInt32(cboMovimientoMotivo.SelectedValue) == (int)Enumerador.enmMovimientoMotivo.POR_ACTUACION_CONSULAR))
            {
                if (Convert.ToInt32(cboMovimientoMotivo.SelectedValue) == (int)Enumerador.enmMovimientoMotivo.POR_PEDIDO)
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se permite este tipo de operación, debe atender PEDIDO"));
                if (Convert.ToInt32(cboMovimientoMotivo.SelectedValue) == (int)Enumerador.enmMovimientoMotivo.POR_ACTUACION_CONSULAR)
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No se permite este tipo de operación, debe realizar ACTUACIÓN CONSULAR"));
            }
            else
            {
                if (cboMovimientoEstado.SelectedValue == Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString() || cboMovimientoEstado.SelectedValue == Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString())
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, " No se puede editar Movimiento (" + lblNroMovimiento.Text + ") porque se encuentra " + cboMovimientoEstado.SelectedItem.Text));
                }
                else
                {
                    if (grdRangos.Rows.Count > 0)
                    {
                        GrabarDatosCabecera();
                        DatosPedido.Visible = false;
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "No existen rangos en detalle de movimiento"));
                    }
                }
            }

        }

        void ctrlToolBar2_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBar2_btnGrabarHandler();
            DesHabilitaCampos();

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                txtObservacion.Enabled = true;
            }
            btnAdicionar.Enabled = false;
            btnCancelar.Enabled = false;
            DatosPedido.Visible = false;
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
                grdMovimiento.DataSource = new DataTable();
                grdMovimiento.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {

                ctrlPaginador.InicializarPaginador();

                CargarGrilla();
            }
        }

        void ctrlToolBar1_btnExportarPDFHandler()
        {
        }
        #endregion

        #region METODOS

        private void LimpiarGrilla()
        {
            Session["dt"] = null;
            grdMovimiento.DataSource = null;
            grdMovimiento.DataBind();
            ctrlPaginador.Visible = false;
        }

        private void LimpiaDatosPedido()
        {
            Session["Pedido_Acta"] = "";
            Session["Pedido_Cantidad"] = "";
            Session["Pedido_Estado"] = "";
            Session["Pedido_Fecha"] = "";
            Session["Pedido_Insumo"] = "";
            Session["Pedido_Motivo"] = "";
            Session["Pedido_Codigo"] = "";
            lblPedActa.Text = Session["Pedido_Acta"].ToString();
            lblPedCantidad.Text = Session["Pedido_Cantidad"].ToString();
            lblPedEstado.Text = Session["Pedido_Estado"].ToString();
            lblPedFecha.Text = Session["Pedido_Fecha"].ToString();
            lblPedInsumo.Text = Session["Pedido_Insumo"].ToString();
            lblPedMotivo.Text = Session["Pedido_Motivo"].ToString();
            lblPedCodigo.Text = Session["Pedido_Codigo"].ToString();
        }

        private void CargarGrilla()
        {
            int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            int intTipoInsumo = 0;
            int intEstado = 0;

            int sOficinaConsularIdConsulta = 0;
            int intBovedaTipoIdConsulta = 0;
            int intBodegaConsultaId = 0;

            string strCodPedidoC = "";

            if (cboInsumoC.SelectedValue != null)
                intTipoInsumo = Convert.ToInt32(cboInsumoC.SelectedValue);
            if (cboMovimientoEstadoC.SelectedValue != null)
                intEstado = Convert.ToInt32(cboMovimientoEstadoC.SelectedValue);
            if (txtNumeroPedidoC.Text != null)
                strCodPedidoC = txtNumeroPedidoC.Text.Trim();


            if (cboMisionConsConsulta.SelectedValue != null)
                sOficinaConsularIdConsulta = Convert.ToInt32(cboMisionConsConsulta.SelectedValue);


            if (cboTipoBovedaConsulta.SelectedValue != null)
                intBovedaTipoIdConsulta = Convert.ToInt32(cboTipoBovedaConsulta.SelectedValue);


            if (cboBovedaConsulta.SelectedValue != null)
                intBodegaConsultaId = Convert.ToInt32(cboBovedaConsulta.SelectedValue);


            if (intOficinaConsularId > 0 && intBovedaTipoIdConsulta > 0 && intBodegaConsultaId > 0)
            {
                DateTime datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
                DateTime datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);


                int intTotalRegistros = 0, intTotalPaginas = 0;

                DataTable dt = oMovimientoConsultaBL.Consultar(intOficinaConsularId,
                                        datFechaInicio,
                                        datFechaFin,
                                        intTipoInsumo,
                                        intEstado,
                                        strCodPedidoC,
                                        ctrlPaginador.PaginaActual,
                                        Constantes.CONST_CANT_REGISTRO,
                                        ref intTotalRegistros,
                                        ref intTotalPaginas,
                                        sOficinaConsularIdConsulta,
                                        intBovedaTipoIdConsulta,
                                        intBodegaConsultaId);

                grdMovimiento.DataSource = dt;
                grdMovimiento.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    updConsulta.Update();
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros.ToString(), true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;
                ctrlPaginador.Visible = false;

                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;
            }

            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FALTAFILTRO, true, Enumerador.enmTipoMensaje.WARNING);
            }


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

        private void CargarGrillaRangosDisponibles()
        {

            int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            int intBovedaTipoIdOrigen = 0, intBodegaOrigenId = 0, intTipoInsumo = 0;

            if (cboInsumoR.SelectedValue != null)
                intTipoInsumo = Convert.ToInt32(cboInsumoR.SelectedValue);
            else
                return;

            if (cboTipoBovedaO.SelectedValue != null)
            {
                intBovedaTipoIdOrigen = Convert.ToInt32(cboTipoBovedaO.SelectedValue);
            }
            if (cboBovedaO.SelectedValue != null)
            {

                if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() != cboBovedaO.SelectedValue.ToString())
                {
                    if ((cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString() ||
                    cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString()) &&
                        enmAccion != Enumerador.enmAccion.MODIFICAR)
                    {
                        intOficinaConsularId = int.Parse(cboMisionConsO.SelectedValue.ToString());
                        intBodegaOrigenId = int.Parse(cboBovedaO.SelectedValue.ToString());
                    }
                    else
                    {
                        intBodegaOrigenId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                    }
                }
                else
                    intBodegaOrigenId = Convert.ToInt32(cboBovedaO.SelectedValue.ToString());
            }

            DataTable dt = oMovimientoConsultaBL.ConsultaRangosDisponibles(intOficinaConsularId,
                                         intBovedaTipoIdOrigen,
                                         intBodegaOrigenId,
                                         intTipoInsumo);
            grdRangosDisponibles.DataSource = dt;
            grdRangosDisponibles.DataBind();

            if (dt.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
            else
            {
                updConsulta.Update();

            }
        }

        private void TraePefijoNumeracion()
        {
            try
            {
                //Trae prefijo de numeración según TIPO DE INSUMO
                if (cboInsumoR.SelectedIndex > 0)
                {
                    DataTable dtParametros = new DataTable();

                    string strDato = string.Empty;
                    int intParametroId = Convert.ToInt32(cboInsumoR.SelectedValue);

                    strDato = comun_Part1.ObtenerParametroDatoPorCampo(Session, "", intParametroId, "Valor");

                    txtPrefijoNum.Text = strDato;
                }
                else
                {
                    ctrlValidacion.MostrarValidacion("Seleccionar un " + strNombreEntidad + " para " + enmAccion + ".");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
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

            cboInsumoC.SelectedIndex = 0;
            cboMovimientoEstadoC.SelectedIndex = 0;
            txtNumeroPedidoC.Text = "";

            //Campos de Registro
            cboMovimientoTipo.SelectedIndex = 0;
            lblNroMovimiento.Visible = false;
            lblNroMovimientoEtiqueta.Visible = false;
            txtCant.Text = Convert.ToInt32(0).ToString();
            txtNroActa.Text = "";
            txtNumeroPedido.Text = "0";
            txtObservacion.Text = "";
            txtPrefijoNum.Text = "";
            txtRanIni.Text = "";
            txtRanFin.Text = "";
            dtpFecha.Text = DateTime.Today.ToString(strFormatoFechas);
            dtpFechaValija.Text = DateTime.Today.ToString(strFormatoFechas);
            cboInsumoR.SelectedIndex = 0;
            TraePefijoNumeracion();


            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                TraeRangosDisponibles();

            ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString() !=
                    Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
            {
                Util.CargarParametroDropDownList(cboMovimientoTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_MOVIMIENTO), true);
                ELiminarRegistroCombo(cboMovimientoTipo, ((int)Enumerador.enmMovimientoTipo.ENTRADA).ToString());


            }

            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());

            CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());

            if (Session["PasaPedidoMovimiento"].ToString() == "0")
            {
                cboMovimientoMotivo.Items.Clear();
                cboMovimientoMotivo.Items.Insert(0, new ListItem("- SELECCIONAR -"));
            }

            // Rangos
            Session["rango_seleccionado"] = -1;
            Session["dt"] = null;
            grdRangos.DataSource = null;
            grdRangos.DataBind();

            // Control de Opciones
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                ctrlToolBar2.btnGrabar.Enabled = true;
            }

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

            cboInsumoC.SelectedIndex = 0;
            cboInsumoC.Enabled = true;
            //Campos de Registro
            txtNroActa.Enabled = false;

            txtObservacion.Enabled = false;
            txtPrefijoNum.Enabled = false;
            txtRanFin.Enabled = false;
            txtRanIni.Enabled = false;
            dtpFecha.Enabled = false;
            dtpFechaValija.Enabled = false;

            dtpFechaValija.Enabled = false;

            txtHoraInicio.Enabled = false;
            cboInsumoR.SelectedIndex = 0;
            cboInsumoR.Enabled = false;
            TraePefijoNumeracion();
            cboMovimientoMotivo.Enabled = false;
            cboMovimientoTipo.Enabled = false;

            cboMisionConsO.Enabled = false;
            cboTipoBovedaO.Enabled = false;
            cboBovedaO.Enabled = false;

            cboMisionConsD.Enabled = false;
            cboTipoBovedaD.Enabled = false;
            cboBovedaD.Enabled = false;

            grdRangos.Enabled = false;

            ctrlToolBar2.btnGrabar.Enabled = false;
            ctrlToolBar2.btnEditar.Enabled = false;
            ctrlToolBar2.btnEliminar.Enabled = false;
            btnAceptado.Visible = false;
            btnRechazado.Visible = false;
            btnAdicionar.Enabled = false;
            btnCancelar.Enabled = false;

            txtRanIni.Enabled = false;
            txtRanFin.Enabled = false;
            txtObservacion.Enabled = false;
        }

        private void HabilitaCampos()
        {
            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            //Campos de Consulta
            dtpFecInicio.Text = DateTime.Today.ToString(strFormatoFechasInicio);
            dtpFecFin.Text = DateTime.Today.ToString(strFormatoFechas);


            cboInsumoC.SelectedIndex = 0;
            cboInsumoC.Enabled = true;
            //Campos de Registro
            txtNroActa.Enabled = true;

            txtPrefijoNum.Enabled = false;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                txtRanFin.Enabled = true;
                txtRanIni.Enabled = true;
                txtObservacion.Enabled = true;
            }


            dtpFechaValija.Enabled = true;

            txtHoraInicio.Enabled = true;
            cboInsumoR.SelectedIndex = 0;
            cboInsumoR.Enabled = true;
            TraePefijoNumeracion();
            cboMovimientoMotivo.Enabled = true;
            cboMovimientoTipo.Enabled = true;

            cboMisionConsO.Enabled = true;
            cboTipoBovedaO.Enabled = true;
            cboBovedaO.Enabled = true;

            cboMisionConsD.Enabled = true;
            cboTipoBovedaD.Enabled = true;
            cboBovedaD.Enabled = true;

            grdRangos.Enabled = true;

            ctrlToolBar2.btnGrabar.Enabled = true;
            ctrlToolBar2.btnEditar.Enabled = true;
            ctrlToolBar2.btnEliminar.Enabled = true;

            btnAceptado.Visible = true;
            btnRechazado.Visible = true;
            btnAdicionar.Enabled = true;
            btnCancelar.Enabled = true;

            txtRanIni.Enabled = true;
            txtRanFin.Enabled = true;
            txtObservacion.Enabled = true;
        }

        private void CargarListados()
        {
            //---------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 19/11/2019
            //Motivo: Se unifico en un solo datatable
            //---------------------------------------------------
            DataTable dtAlmacenTipoInsumo = new DataTable();

            dtAlmacenTipoInsumo = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO);

            Util.CargarParametroDropDownList(cboInsumoC, dtAlmacenTipoInsumo, true, " - TODOS - ");
            Util.CargarParametroDropDownList(cboInsumoR, dtAlmacenTipoInsumo, true);
            //---------------------------------------------------

            Util.CargarParametroDropDownList(cboMovimientoTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_MOVIMIENTO), true);
            Util.CargarParametroDropDownList(cboMovimientoMotivo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO), true);

            //---------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 19/11/2019
            //Motivo: Se unifico en un solo datatable
            //---------------------------------------------------
            DataTable dtMovimiento = new DataTable();

            dtMovimiento = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.MOVIMIENTO);

            Util.CargarParametroDropDownList(cboMovimientoEstadoC, dtMovimiento, true, " - TODOS - ");
            Util.CargarParametroDropDownList(cboMovimientoEstado, dtMovimiento);
            //---------------------------------------------------


            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]).ToString() == ((int)Enumerador.enmTipoRol.OPERATIVO).ToString())
            {
                cboMovimientoTipo.Enabled = false;
                cboMovimientoMotivo.Enabled = false;
            }

            //Se cargan combos de Consulta
            cboMisionConsConsulta.CargarPorJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                CargarOficinaConsular(cboMisionConsConsulta, cboTipoBovedaConsulta, cboBovedaConsulta, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
            else
                CargarOficinaConsular(cboMisionConsConsulta, cboTipoBovedaConsulta, cboBovedaConsulta, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString(), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]).ToString(), true);

            //Se cargan combos de Origen
            cboMisionConsO.CargarPorJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());


            //Se cargan combos de Destino
            cboMisionConsD.CargarPorJefatura(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString());
            
        }

        private void TraeDetalleMovimiento(int intCodigoMov)
        {
            int mode_iMovimientoId = Convert.ToInt32(grdMovimiento.Rows[intCodigoMov].Cells[0].Text);

            DataTable dt = oMovimientoDetalleConsultaBL.MovimientoDetalleConsultar(mode_iMovimientoId);


            Session["dt"] = dt;

            grdRangos.DataSource = dt;
            grdRangos.DataBind();
        }

        private void TraeDetalleRangos(int intCodigoMov, int intCodigoMovDet)
        {
            int mode_iMovimientoId = Convert.ToInt32(Session[strVariableMovimiento]);
            int mode_iMovimientoDetalleId = intCodigoMovDet;


            DataTable dt = oMovimientoDetalleConsultaBL.MovimientoDetalleConsultar(mode_iMovimientoId);

            grdRangos.DataSource = dt;
            grdRangos.DataBind();
        }

        private object[] ObtenerParametrosBusqueda()
        {

            int pedi_IOficinaConsularIdOrigen = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            string strActaRemision = "";
            int intEstado = 0;
            int intInsumo = 0;

            int intTotalRegistros = 0, intTotalPaginas = 0;

            string strCodPedidoC = txtNumeroPedido.Text;

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

        private void PintarCabecera(SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO)
        {
            cboMovimientoTipo.SelectedValue = objMOVIMIENTO.movi_sMovimientoTipoId.ToString();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.CONSULTAR)
            {
                Util.CargarParametroDropDownList(cboMovimientoMotivo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO));
                cboMovimientoMotivo.SelectedValue = objMOVIMIENTO.movi_sMovimientoMotivoId.ToString();

                if (cboMovimientoMotivo.SelectedValue == "6152")
                {
                    btnimprimir.Visible = true;
                }
                else
                {
                    btnimprimir.Visible = false;
                }
            }
            else if (enmAccion == Enumerador.enmAccion.ATENDER || enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                llenar_cboMovimientoMotivo();
                cboMovimientoMotivo.SelectedValue = objMOVIMIENTO.movi_sMovimientoMotivoId.ToString();
            }

            cboInsumoR.SelectedValue = objMOVIMIENTO.movi_sInsumoTipoId.ToString();
            txtPrefijoNum.Text = objMOVIMIENTO.movi_vPrefijoNumeracion;
            dtpFecha.Text = objMOVIMIENTO.movi_dFechaRegistro.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            if (objMOVIMIENTO.movi_vActaRemision == "&nbsp;")
            {
                txtNroActa.Text = "";
            }
            else {
                txtNroActa.Text = objMOVIMIENTO.movi_vActaRemision;
            }
            
            txtNumeroPedido.Text = objMOVIMIENTO.movi_cPedidoCodigo;
            cboMovimientoEstado.SelectedValue = objMOVIMIENTO.movi_sEstadoId.ToString();
            lblNroMovimiento.Text = objMOVIMIENTO.movi_cMovimientoCodigo;

            // origen
            if (objMOVIMIENTO.movi_sBovedaTipoIdOrigen.ToString() == ((int)Enumerador.enmBovedaTipo.MISION).ToString())
            {
                CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO,
                    objMOVIMIENTO.movi_sOficinaConsularIdOrigen.ToString());
            }
            else
            {
                CargarOficinaConsular(cboMisionConsO, cboTipoBovedaO, cboBovedaO,
                    objMOVIMIENTO.movi_sOficinaConsularIdOrigen.ToString(),
                    objMOVIMIENTO.movi_sBodegaOrigenId.ToString(), true);
            }


            // destino
            if (objMOVIMIENTO.movi_sBovedaTipoIdDestino.ToString() == ((int)Enumerador.enmBovedaTipo.MISION).ToString())
            {
                CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD,
                    objMOVIMIENTO.movi_sOficinaConsularIdDestino.ToString());
            }
            else
            {
                CargarOficinaConsular(cboMisionConsD, cboTipoBovedaD, cboBovedaD,
                    objMOVIMIENTO.movi_sOficinaConsularIdDestino.ToString(),
                    objMOVIMIENTO.movi_sBodegaDestinoId.ToString(), true);
            }

            
            dtpFechaValija.Text = Convert.ToDateTime(objMOVIMIENTO.movi_dFechaValija).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            txtHoraInicio.Text = Convert.ToDateTime(objMOVIMIENTO.movi_dFechaValija).ToString("HH:mm");
            UpdMantenimiento.Update();
        }

        private void PintarDetalle(SGAC.BE.AL_MOVIMIENTODETALLE objMOVIMIENTOD)
        {
            txtRanIni.Text = objMOVIMIENTOD.mode_vRangoInicial.ToString();
            txtRanFin.Text = objMOVIMIENTOD.mode_vRangoFinal.ToString();
            txtCant.Text = objMOVIMIENTOD.mode_ICantidad.ToString();
            txtObservacion.Text = Page.Server.HtmlDecode(objMOVIMIENTOD.mode_vObservacion);

            UpdMantenimiento.Update();
        }

        private void GrabarDatosCabecera()
        {

            String strFormatoFechas = String.Empty;
            String strFormatoFechasInicio = String.Empty;

            strFormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            strFormatoFechasInicio = ConfigurationManager.AppSettings["FormatoFechasInicio"].ToString();

            // Validacion
            Session["AccionMovimiento"] = "GRABAR";

            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = new BE.AL_MOVIMIENTO();

            object[] arrParametros = null;

            //Validar que el registro no haya sido modificado
            #region Validacion Pedido

            if (Session["PasaPedidoMovimiento"].ToString() == "1" && ((Enumerador.enmAccion)Session[strVariableAccion]) != Enumerador.enmAccion.MODIFICAR)
            {
                arrParametros = ObtenerParametrosBusqueda();

                int iTotalRegistros = (int)arrParametros[9];
                int intTotalPaginas = (int)arrParametros[10];


                DataTable dt = oPedidoConsultaBL.Consultar((int)arrParametros[0], (DateTime)arrParametros[1], (DateTime)arrParametros[2], arrParametros[3].ToString(), (int)arrParametros[4], (int)arrParametros[5], arrParametros[6].ToString(), (int)arrParametros[7], (int)arrParametros[8], ref iTotalRegistros, ref intTotalPaginas);

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
            }

            #endregion

            int intResultado = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    objMOVIMIENTO = ValoresNuevosCabecera();
                    arrParametros = new object[2];

                    List<SGAC.BE.AL_MOVIMIENTODETALLE> listaDetalle = ObtenerMoviemientoDetalle();

                    //listaDetalle.

                    foreach (SGAC.BE.AL_MOVIMIENTODETALLE objDetalle in listaDetalle)
                    {
                        if (cboMovimientoTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoTipo.SALIDA).ToString())
                        {
                            if (cboMovimientoMotivo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoMotivo.DETERIORADO).ToString())
                            {
                                objDetalle.mode_vObservacion += " (BAJA)";
                            }
                            else if (cboMovimientoMotivo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString())
                            {
                                objDetalle.mode_vObservacion += " (PERDIDA)";
                            }
                            else if (cboMovimientoMotivo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString())
                            {
                                objDetalle.mode_vObservacion += " (BAJA)";
                            }
                            else if (cboMovimientoMotivo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                            {
                                objDetalle.mode_vObservacion += " (DEVUELVE)";
                            }
                        }
                    }

                    intResultado = oMovimientoMantenimientoBL.MovimientoAdicionar(objMOVIMIENTO, listaDetalle);

                    if (intResultado == (int)Enumerador.enmResultadoQuery.ERR)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO));

                        ctrlToolBar2_btnNuevoHandler();

                        Session["AccionMovimiento"] = null;
                        return;
                    }

                    iMovimientoId = objMOVIMIENTO.movi_iMovimientoId;
                    Session["Movimiento_Id"] = iMovimientoId;

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Movimiento Registrado");
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    objMOVIMIENTO = ValoresNuevosCabecera();
                    intResultado = oMovimientoMantenimientoBL.MovimientoActualizar(objMOVIMIENTO, ObtenerMoviemientoDetalle());

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Movimiento Modificado: (" + lblNroMovimiento.Text + " )");
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    cboMovimientoEstado.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoEstado.ANULADO).ToString();
                    objMOVIMIENTO = ValoresNuevosCabecera();

                    objMOVIMIENTO.movi_iMovimientoId = Convert.ToInt32(Session["Movimiento_Id"]);

                    intResultado = oMovimientoMantenimientoBL.MovimientoEliminar(objMOVIMIENTO);

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_EXITO + "\n Movimiento Anulado: (" + lblNroMovimiento.Text + " )");
                    break;
            }

            string strScript = string.Empty;

            if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                else
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);

                HabilitaCampos();
                LimpiaDatosPedido();
                LimpiaCamposRangos();
                LimpiaCampos();

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    btnAdicionar.Enabled = true;
                    btnCancelar.Enabled = true;
                }

                grdRangos.Visible = false;
                btnAceptado.Visible = false;
                btnRechazado.Visible = false;

                ctrlToolBar2.btnConfiguration.Enabled = false;
                updRango.Update();
                UpdMantenimiento.Update();

                CargarGrilla();

                Session["OBJ_MOVIMIENTO"] = objMOVIMIENTO;
                if (intPasaPedidoMovimiento == 1)
                {
                    btnimprimir.Visible = true;
                    strScript += "window.open('" + "../Almacen/FrmReporte.aspx" + "', 'popup_window', 'scrollbars=1,resizable=1,fullscreen=yes');";
                }
                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.HabilitarTab(0);
                
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            TraeRangosDisponibles();
            Session["AccionMovimiento"] = null;
            Comun.EjecutarScript(Page, strScript);

                ctrlToolBar2_btnNuevoHandler();
                ctrlToolBar1_btnBuscarHandler();
            
            
        }

        private void GrabarDatosAceptar()
        {
            Session["AccionMovimiento"] = "ACEPTAR";
            object[] arrParametros = new object[2];
            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = new SGAC.BE.AL_MOVIMIENTO();
            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO2 = new SGAC.BE.AL_MOVIMIENTO(); 
            int intResultado = 0;
            objMOVIMIENTO = ValoresNuevosCabecera();
            objMOVIMIENTO.movi_sEstadoId = Convert.ToInt16(Enumerador.enmMovimientoEstado.ACEPTADO);
            objMOVIMIENTO2 = ValoresNuevosCabecera();
            List<SGAC.BE.AL_MOVIMIENTODETALLE> listaDetalle = ObtenerMoviemientoDetalle();
            List<SGAC.BE.AL_MOVIMIENTODETALLE> listaDetalle2 = ObtenerMoviemientoDetalle();

            foreach (SGAC.BE.AL_MOVIMIENTODETALLE objDetalle in listaDetalle)
            {
                objDetalle.mode_sEstadoId = Convert.ToInt16(Enumerador.enmMovimientoEstado.ACEPTADO);


                if (objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() !=
                    Convert.ToInt32(Enumerador.enmMovimientoMotivo.DETERIORADO).ToString() &&
                    objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() !=
                    Convert.ToInt32(Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString() &&
                    objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() !=
                    Convert.ToInt32(Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString() &&
                    objMOVIMIENTO.movi_sMovimientoMotivoId.ToString() !=
                    Convert.ToInt32(Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                {
                    objDetalle.mode_vObservacion += " (ENVIA)";
                }
            }


            //intResultado = oMovimientoMantenimientoBL.MovimientoActualizar(objMOVIMIENTO, listaDetalle);

            string strScript = string.Empty;

            //if (intResultado == (int)Enumerador.enmResultadoQuery.ERR)
            //{

            //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);

            //    Comun.EjecutarScript(Page, strScript);

            //    Session["AccionMovimiento"] = null;
            //    return;
            //}

            Int16 intMovimientoTipo = Convert.ToInt16(cboMovimientoTipo.SelectedValue);

            Int16 intMovimientoMotivo = Convert.ToInt16(cboMovimientoMotivo.SelectedValue);

            //Los movimientos de salida que son aceptados, generan automaticamente un movimiento de entrada.

            if (intMovimientoTipo == Convert.ToInt16(Enumerador.enmMovimientoTipo.SALIDA))
            {
                intMovimientoTipo = Convert.ToInt16(Enumerador.enmMovimientoTipo.ENTRADA);
            }

            foreach (SGAC.BE.AL_MOVIMIENTODETALLE objDetalle in listaDetalle2)
            {
                if (cboMovimientoTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmMovimientoTipo.SALIDA).ToString())
                {

                    objDetalle.mode_vObservacion = objDetalle.mode_vObservacion.Replace(" (ENVIA)", "") + " (RECIBE)";
                }
            }

            objMOVIMIENTO2.movi_sMovimientoTipoId = intMovimientoTipo;
            objMOVIMIENTO2.movi_sEstadoId = Convert.ToInt16(Enumerador.enmMovimientoEstado.ACEPTADO);

            intResultado = oMovimientoMantenimientoBL.MovimientoAdicionarYActualizarEstado(objMOVIMIENTO2, listaDetalle2,objMOVIMIENTO, listaDetalle);

            iMovimientoId = objMOVIMIENTO2.movi_iMovimientoId;
            Session["Movimiento_Id"] = iMovimientoId;

            if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    LimpiaCampos();
                    HabilitaCampos();
                }

                cboMovimientoEstado.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoEstado.ACEPTADO).ToString();
                DesHabilitaCampos();
                LimpiaDatosPedido();
                LimpiaCampos();
                LimpiaCamposRangos();
                grdRangos.Visible = false;
                ctrlToolBar2.btnEditar.Enabled = false;
                ctrlToolBar2.btnEliminar.Enabled = false;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    ctrlToolBar2.btnGrabar.Enabled = true;
                }
                ctrlToolBar2.btnConfiguration.Enabled = false;

                UpdMantenimiento.Update();
                updRango.Update();

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                CargarGrilla();
                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }


            Session["AccionMovimiento"] = null;
            Session["rango_seleccionado"] = -1;
            Session["dt"] = null;


            Comun.EjecutarScript(Page, strScript);
            ctrlToolBar2_btnNuevoHandler();
        }

        private void GrabarDatosRechazar()
        {
            Session["AccionMovimiento"] = "RECHAZAR";
            object[] arrParametros = new object[2];
            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO;
            int intResultado = 0;

            objMOVIMIENTO = ValoresNuevosCabecera();
            objMOVIMIENTO.movi_sEstadoId = Convert.ToInt16(Enumerador.enmMovimientoEstado.RECHAZADO);
            List<SGAC.BE.AL_MOVIMIENTODETALLE> listaDetalle = ObtenerMoviemientoDetalle();
            foreach (SGAC.BE.AL_MOVIMIENTODETALLE objDetalle in listaDetalle)
            {
                objDetalle.mode_sEstadoId = Convert.ToInt16(Enumerador.enmMovimientoEstado.RECHAZADO);

            }

            arrParametros[0] = objMOVIMIENTO;
            arrParametros[1] = listaDetalle;



            // ACTUALIZAR
            Int16 intOCOrigen = objMOVIMIENTO.movi_sOficinaConsularIdDestino;
            Int16 intBovedaTipoOrigen = objMOVIMIENTO.movi_sBovedaTipoIdDestino;
            Int16 intBovedaOrigen = objMOVIMIENTO.movi_sBodegaDestinoId;
            Int16 intOCDestino = objMOVIMIENTO.movi_sOficinaConsularIdOrigen;
            Int16 intBovedaTipoDestino = objMOVIMIENTO.movi_sBovedaTipoIdOrigen;
            Int16 intBovedaDestino = objMOVIMIENTO.movi_sBodegaOrigenId;

            objMOVIMIENTO.movi_sOficinaConsularIdOrigen = intOCOrigen;
            objMOVIMIENTO.movi_sBovedaTipoIdOrigen = intBovedaTipoOrigen;
            objMOVIMIENTO.movi_sBodegaOrigenId = intBovedaOrigen;
            objMOVIMIENTO.movi_sOficinaConsularIdDestino = intOCDestino;
            objMOVIMIENTO.movi_sBovedaTipoIdDestino = intBovedaTipoDestino;
            objMOVIMIENTO.movi_sBodegaDestinoId = intBovedaDestino;

            intResultado = oMovimientoMantenimientoBL.MovimientoActualizar(objMOVIMIENTO, listaDetalle);

            string strScript = string.Empty;

            if (intResultado == (int)Enumerador.enmResultadoQuery.ERR)
            {

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);

                Comun.EjecutarScript(Page, strScript);

                Session["AccionMovimiento"] = null;
                return;
            }

            if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    LimpiaCampos();
                    HabilitaCampos();
                }
                cboMovimientoEstado.SelectedValue = Convert.ToInt32(Enumerador.enmMovimientoEstado.RECHAZADO).ToString();
                DesHabilitaCampos();
                LimpiaDatosPedido();
                LimpiaCampos();
                LimpiaCamposRangos();
                grdRangos.Visible = false;
                ctrlToolBar2.btnEditar.Enabled = false;
                ctrlToolBar2.btnEliminar.Enabled = false;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
                {
                    ctrlToolBar2.btnGrabar.Enabled = true;
                }
                ctrlToolBar2.btnConfiguration.Enabled = false;
                UpdMantenimiento.Update();
                updRango.Update();

                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                CargarGrilla();
                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }


            Session["AccionMovimiento"] = null;
            Session["rango_seleccionado"] = -1;
            Session["dt"] = null;
            Comun.EjecutarScript(Page, strScript);
            ctrlToolBar2_btnNuevoHandler();
        }

        private void GrabarDatosInsumo()
        {
            // Validacion
            object[] arrParametros = new object[1];
            SGAC.BE.AL_INSUMO objINSUMO;
            int intResultado = 0;
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    arrParametros = new object[4];
                    objINSUMO = ValoresNuevosInsumo();
                    intResultado = oInsumoMantenimientoBL.InsumoAdicionar(objINSUMO, Convert.ToInt32(Session["Movimiento_Id"]), 1, 15);
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    arrParametros = new object[4];
                    objINSUMO = ValoresNuevosInsumo();
                    intResultado = oInsumoMantenimientoBL.InsumoActualizar(objINSUMO);
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    arrParametros = new object[4];
                    objINSUMO = ValoresNuevosInsumo();
                    intResultado = oInsumoMantenimientoBL.InsumoActualizar(objINSUMO);
                    break;
            }
        }

        private void LimpiaCamposRangos()
        {
            txtRanIni.Text = "";
            txtRanFin.Text = "";
            txtCant.Text = Convert.ToInt32(0).ToString();
            txtObservacion.Text = "";
        }

        private void PintarGrillaDetalleFilas()
        {
            DataTable dt = ((DataTable)Session["dt"]).Copy();
            foreach (GridViewRow row in grdRangos.Rows)
            {
                if (Convert.ToInt32(dt.Rows[row.RowIndex]["mode_sEstadoId"]) == (int)Enumerador.enmMovimientoEstado.ANULADO)
                { row.BackColor = System.Drawing.Color.FromName("#FAD4D9"); }
                else
                { row.BackColor = System.Drawing.Color.White; }
            }
        }

        private void CalculaRangos()
        {
            int RanIni;
            int RanFin;
            if (txtRanFin.Text == "")
            { RanIni = 0; }
            else
            { RanIni = Convert.ToInt32(txtRanIni.Text); }

            if (txtRanFin.Text == "")
            {
                txtRanFin.Text = txtRanIni.Text;
                RanFin = RanIni;
            }
            else
            { RanFin = Convert.ToInt32(txtRanFin.Text); }

            txtCant.Text = ((RanFin - RanIni) + 1).ToString();

        }

        private SGAC.BE.AL_MOVIMIENTO ValoresSeleccionCabecera(int intIndex)
        {
            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = new BE.AL_MOVIMIENTO();
            objMOVIMIENTO.movi_iMovimientoId = Convert.ToInt32(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "MovimientoId")].Text);
            objMOVIMIENTO.movi_sOficinaConsularIdOrigen = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sOficinaConsularIdOrigen")].Text);
            Session["OfConsular Origen"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "OfConsular Origen")].Text;
            objMOVIMIENTO.movi_sBovedaTipoIdOrigen = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sBovedaTipoIdOrigen")].Text);
            Session["Tipo Origen"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Tipo Origen")].Text;
            objMOVIMIENTO.movi_sBodegaOrigenId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sBodegaOrigenId")].Text);
            Session["Boveda Origen"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Boveda Origen")].Text;
            objMOVIMIENTO.movi_sOficinaConsularIdDestino = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sOficinaConsularIdDestino")].Text);
            Session["OfConsular Destino"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "OfConsular Destino")].Text;
            objMOVIMIENTO.movi_sBovedaTipoIdDestino = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sBovedaTipoIdDestino")].Text);
            Session["Tipo Destino"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Tipo Destino")].Text;
            objMOVIMIENTO.movi_sBodegaDestinoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sBodegaDestinoId")].Text);
            Session["Boveda Destino"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Boveda Destino")].Text;
            objMOVIMIENTO.movi_sInsumoTipoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sInsumoTipoId")].Text);
            Session["Insumo"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Insumo")].Text;


            #region MDIAZ - 20150301 : Error al convertir MAR-01-2015 a DATETIME
            objMOVIMIENTO.movi_dFechaRegistro = Comun.FormatearFecha(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Fecha")].Text);
            #endregion

            objMOVIMIENTO.movi_vActaRemision = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_vActaRemision")].Text;
            objMOVIMIENTO.movi_sMovimientoTipoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sMovimientoTipoId")].Text);
            Session["TipoMovimiento"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "TipoMovimiento")].Text;
            objMOVIMIENTO.movi_sEstadoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sEstadoId")].Text);
            Session["Estado"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Estado")].Text;
            objMOVIMIENTO.movi_sMovimientoMotivoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sMovimientoMotivoId")].Text);
            Session["Motivo"] = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "Motivo")].Text;
            objMOVIMIENTO.movi_vPrefijoNumeracion = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_vPrefijoNumeracion")].Text;
            objMOVIMIENTO.movi_sMovimientoMotivoId = Convert.ToInt16(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_sMovimientoMotivoId")].Text);
            objMOVIMIENTO.movi_cPedidoCodigo = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_cPedidoCodigo")].Text;
            objMOVIMIENTO.movi_cMovimientoCodigo = grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_cMovimientoCodigo")].Text;
            objMOVIMIENTO.movi_dFechaValija = Comun.FormatearFecha(grdMovimiento.Rows[intIndex].Cells[ObtenerIndiceColumnaGrilla(grdMovimiento, "movi_dFechaValija")].Text);
            return objMOVIMIENTO;
        }

        private SGAC.BE.AL_MOVIMIENTODETALLE ValoresSeleccionDetalle(int intIndexD, int intSeleccionado, int intSeleccionadoD)
        {
            SGAC.BE.AL_MOVIMIENTODETALLE objMOVIMIENTOD = new BE.AL_MOVIMIENTODETALLE();
            objMOVIMIENTOD.mode_vRangoInicial = grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "RangoIni")].Text;
            objMOVIMIENTOD.mode_vRangoFinal = grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "RangoFin")].Text;
            objMOVIMIENTOD.mode_ICantidad = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "Cant")].Text);
            objMOVIMIENTOD.mode_vObservacion = grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "Observacion")].Text;
            objMOVIMIENTOD.mode_sEstadoId = Convert.ToInt16(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_sEstadoId")].Text);
            objMOVIMIENTOD.mode_iMovimientoId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "movi_IMovimientoId")].Text);
            objMOVIMIENTOD.mode_iMovimientoDetalleId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iMovimientoDetalleId")].Text);

            if (grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuacionId")].Text == null ||
                grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuacionId")].Text == "&nbsp;" ||
                grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuacionId")].Text == string.Empty)
            {
                objMOVIMIENTOD.mode_iActuacionId = 0;
            }
            else
            {
                objMOVIMIENTOD.mode_iActuacionId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuacionId")].Text);
            }

            if (grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuaciondetalleId")].Text == null ||
                grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuaciondetalleId")].Text == "&nbsp;" ||
                grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuaciondetalleId")].Text == string.Empty)
            {
                objMOVIMIENTOD.mode_iActuacionDetalleId = 0;
            }
            else
            {
                objMOVIMIENTOD.mode_iActuacionDetalleId = Convert.ToInt32(grdRangos.Rows[intIndexD].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "mode_iActuaciondetalleId")].Text);
            }

            return objMOVIMIENTOD;
        }

        private SGAC.BE.AL_MOVIMIENTO ValoresNuevosCabecera()
        {
            if (Session != null)
            {
                SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = new BE.AL_MOVIMIENTO();

                objMOVIMIENTO.movi_sMovimientoTipoId = Convert.ToInt16(cboMovimientoTipo.SelectedValue);
                objMOVIMIENTO.movi_sMovimientoMotivoId = Convert.ToInt16(cboMovimientoMotivo.SelectedValue);
                objMOVIMIENTO.movi_sInsumoTipoId = Convert.ToInt16(cboInsumoR.SelectedValue);
                objMOVIMIENTO.movi_vPrefijoNumeracion = txtPrefijoNum.Text;

                DateTime datFechaRegistro = Comun.FormatearFecha(dtpFecha.Text);

                objMOVIMIENTO.movi_dFechaRegistro = datFechaRegistro;


                objMOVIMIENTO.movi_vActaRemision = txtNroActa.Text;

                if (lblNroMovimiento.Text == "")
                {
                    objMOVIMIENTO.movi_cMovimientoCodigo = "0";
                }
                else
                {
                    objMOVIMIENTO.movi_cMovimientoCodigo = lblNroMovimiento.Text;
                }

                objMOVIMIENTO.movi_cPedidoCodigo = txtNumeroPedido.Text;
                objMOVIMIENTO.movi_sOficinaConsularIdOrigen = Convert.ToInt16(cboMisionConsO.SelectedValue);
                objMOVIMIENTO.movi_sBovedaTipoIdOrigen = Convert.ToInt16(cboTipoBovedaO.SelectedValue);

                if (Convert.ToInt32(cboTipoBovedaO.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
                    objMOVIMIENTO.movi_sBodegaOrigenId = Convert.ToInt16(cboMisionConsO.SelectedValue);
                else
                    objMOVIMIENTO.movi_sBodegaOrigenId = Convert.ToInt16(cboBovedaO.SelectedValue);

                objMOVIMIENTO.movi_sOficinaConsularIdDestino = Convert.ToInt16(cboMisionConsD.SelectedValue);
                objMOVIMIENTO.movi_sBovedaTipoIdDestino = Convert.ToInt16(cboTipoBovedaD.SelectedValue);

                if (Convert.ToInt32(cboTipoBovedaD.SelectedValue) == (int)Enumerador.enmBovedaTipo.MISION)
                    objMOVIMIENTO.movi_sBodegaDestinoId = Convert.ToInt16(cboMisionConsD.SelectedValue);
                else
                    objMOVIMIENTO.movi_sBodegaDestinoId = Convert.ToInt16(cboBovedaD.SelectedValue);

                objMOVIMIENTO.movi_sEstadoId = Convert.ToInt16(cboMovimientoEstado.SelectedValue);
                objMOVIMIENTO.movi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objMOVIMIENTO.movi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objMOVIMIENTO.movi_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objMOVIMIENTO.movi_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objMOVIMIENTO.movi_iMovimientoId = Convert.ToInt32(Session["Movimiento_Id"]);


                DateTime datFechaValija = Comun.FormatearFecha(dtpFechaValija.Text);

                string fecha = datFechaValija.ToString();


                objMOVIMIENTO.movi_dFechaValija = Comun.FormatearFecha(fecha.Substring(0, 10) + " " + txtHoraInicio.Text);

                return objMOVIMIENTO;
            }

            return null;
        }

        private SGAC.BE.AL_MOVIMIENTODETALLE ValoresNuevosDetalle(int totalRows, int r)
        {
            if (Session != null)
            {

                string sDetalleAccion = string.Empty;
                string sDetalleTextoReemplazar = string.Empty;

                if (Session["AccionMovimiento"].ToString() == "GRABAR")
                {

                    if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString())
                    {
                        sDetalleAccion = "(ATIENDE PEDIDO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString())
                    {
                        sDetalleAccion = "(EMITE TRASLADO INTERNO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.CARGA_INICIAL).ToString())
                    {
                        sDetalleAccion = "(CARGA INICIAL)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_ANULACION).ToString())
                    {
                        sDetalleAccion = "(DEVUELVE POR ANULACIÓN)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_ERROR_FABRICA).ToString())
                    {
                        sDetalleAccion = "(DEVUELVE POR ERROR DE FÁBRICA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_DETERIORADO).ToString())
                    {
                        sDetalleAccion = "(DEVUELVE POR DETERIORO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_MALA_IMPRESION).ToString())
                    {
                        sDetalleAccion = "(DEVUELVE POR MALA IMPRESIÓN)";
                    }

                    if ((Enumerador.enmAccion)Session[strVariableAccion] == Enumerador.enmAccion.MODIFICAR &&
                        cboMovimientoMotivo.SelectedValue.ToString() != ((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString())
                    {
                        sDetalleAccion += " (EDICIÓN)";
                    }

                }
                else if (Session["AccionMovimiento"].ToString() == "ACEPTAR")
                {
                    if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString())
                    {
                        sDetalleAccion = "(ACEPTA PEDIDO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString())
                    {
                        sDetalleAccion = "(ACEPTA TRASLADO INTERNO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString())
                    {
                        sDetalleAccion = "(ACEPTA BAJA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString())
                    {
                        sDetalleAccion = "(ACEPTA PÉRDIDA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString())
                    {
                        sDetalleAccion = "(ACEPTA BAJA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                    {
                        sDetalleAccion = "(ACEPTA DEVOLUCIÓN)";
                    }
                }
                else if (Session["AccionMovimiento"].ToString() == "RECHAZAR")
                {
                    if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString())
                    {
                        sDetalleAccion = "(RECHAZA PEDIDO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString())
                    {
                        sDetalleAccion = "(RECHAZA TRASLADO INTERNO)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString())
                    {
                        sDetalleAccion = "(RECHAZA BAJA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString())
                    {
                        sDetalleAccion = "(RECHAZA PÉRDIDA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString())
                    {
                        sDetalleAccion = "(RECHAZA BAJA)";
                    }
                    else if (cboMovimientoMotivo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString())
                    {
                        sDetalleAccion = "(RECHAZA DEVOLUCIÓN)";
                    }
                }



                for (int D = r; D < totalRows; r++)
                {
                    DataTable dt = (Session["dt"]) as DataTable;
                    if (dt != null)
                    {
                        SGAC.BE.AL_MOVIMIENTODETALLE objMOVIMIENTOD = new BE.AL_MOVIMIENTODETALLE();
                        objMOVIMIENTOD.mode_vRangoInicial = dt.Rows[r]["RangoIni"].ToString();
                        objMOVIMIENTOD.mode_vRangoFinal = dt.Rows[r]["RangoFin"].ToString();
                        objMOVIMIENTOD.mode_ICantidad = Convert.ToInt32(dt.Rows[r]["Cant"].ToString());

                        if (Session["AccionMovimiento"].ToString() == "ACEPTAR" ||
                            Session["AccionMovimiento"].ToString() == "RECHAZAR" ||
                            (Enumerador.enmAccion)Session[strVariableAccion] == Enumerador.enmAccion.MODIFICAR)
                        {
                            objMOVIMIENTOD.mode_vObservacion = sDetalleAccion;
                        }
                        else
                        {
                            objMOVIMIENTOD.mode_vObservacion = dt.Rows[r]["Observacion"].ToString().Trim() + " " + sDetalleAccion;
                        }

                        objMOVIMIENTOD.mode_iActuacionId = iActuacionId;
                        objMOVIMIENTOD.mode_iActuacionDetalleId = iActuaciondetalleId;

                        if (dt.Rows[r]["mode_sEstadoId"].ToString() == Convert.ToInt16(Enumerador.enmMovimientoEstado.ANULADO).ToString())
                            objMOVIMIENTOD.mode_sEstadoId = Convert.ToInt16(dt.Rows[r]["mode_sEstadoId"]);
                        else
                            objMOVIMIENTOD.mode_sEstadoId = Convert.ToInt16(cboMovimientoEstado.SelectedValue); ;

                        objMOVIMIENTOD.mode_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objMOVIMIENTOD.mode_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objMOVIMIENTOD.mode_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objMOVIMIENTOD.mode_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objMOVIMIENTOD.mode_iMovimientoDetalleId = Convert.ToInt32(dt.Rows[r]["mode_iMovimientoDetalleId"].ToString());
                        return objMOVIMIENTOD;
                    }
                }
            }
            return null;
        }


        private List<BE.AL_MOVIMIENTODETALLE> ObtenerMoviemientoDetalle()
        {
            List<BE.AL_MOVIMIENTODETALLE> Lista = new List<BE.AL_MOVIMIENTODETALLE>();
            int totalRows = grdRangos.Rows.Count;
            SGAC.BE.AL_MOVIMIENTODETALLE objMOVIMIENTOD;
            for (int r = 0; r < totalRows; r++)
            {
                objMOVIMIENTOD = ValoresNuevosDetalle(totalRows, r);
                Lista.Add(objMOVIMIENTOD);
            }
            return Lista;
        }

        private SGAC.BE.AL_INSUMO ValoresNuevosInsumo()
        {
            if (Session != null)
            {
                SGAC.BE.AL_INSUMO objINSUMO = new BE.AL_INSUMO();
                objINSUMO.insu_sInsumoTipoId = Convert.ToInt16(cboInsumoR.SelectedValue);
                objINSUMO.insu_vPrefijoNumeracion = txtPrefijoNum.Text;
                objINSUMO.insu_vCodigoUnicoFabrica = "0";
                objINSUMO.insu_dFechaRegistro = Comun.FormatearFecha(dtpFecha.Text);
                objINSUMO.insu_sEstadoId = Convert.ToInt16(cboMovimientoEstado.SelectedValue);
                objINSUMO.insu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objINSUMO.insu_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objINSUMO.AL_MOVIMIENTO = new BE.AL_MOVIMIENTO();
                objINSUMO.AL_MOVIMIENTO.movi_sOficinaConsularIdOrigen = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objINSUMO.AL_MOVIMIENTO.movi_iMovimientoId = Convert.ToInt32(Session["Movimiento_Id"].ToString());
                return objINSUMO;
            }
            return null;
        }

        public static void Disable(Page container)
        {
            for (int i = 0; i < container.Controls.Count; i++)
            {
                container.Form.Controls[i].EnableViewState = false;
            }
        }

        private int VerificaRangos(string ValRanIni, string ValRanFin)
        {
            DataTable dt = (Session["dt"]) as DataTable;
            DataRow Row1;
            Row1 = dt.NewRow();

            int intInserta;
            int VRI = Convert.ToInt32(ValRanIni);
            int VRF = Convert.ToInt32(ValRanFin);
            int ver1 = Convert.ToInt32(dt.Rows[0]["RangoIni"]);
            int ver2 = Convert.ToInt32(dt.Rows[0]["RangoFin"]);

            if ((VRI >= ver1 && VRI <= ver2) || (VRF >= ver1 && VRF <= ver2))
            { intInserta = 1; }
            else { intInserta = 0; }

            return intInserta;
        }

        private void MesAnterior()
        {
            int intOficinaConsularId = Convert.ToInt32(cboMisionConsD.SelectedValue);
            int intTipoInsumo = 0;

            if (cboInsumoR.SelectedValue != null)
                intTipoInsumo = Convert.ToInt32(cboInsumoR.SelectedValue);


            string strSaldoInicial = "0";
            DataTable dt = oMovimientoConsultaBL.ConsultarMesAnterior(intOficinaConsularId, intTipoInsumo);

            if (dt.Rows.Count > 0)
                strSaldoInicial = dt.Rows[0][0].ToString();

            lblSaldoMesAnterior.Text = "SALDO MES ANTERIOR : " + strSaldoInicial;
        }

        public DataTable filldata()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RangoIni", typeof(string));
            dt.Columns.Add("RangoFin", typeof(string));
            dt.Columns.Add("Cant", typeof(string));
            dt.Columns.Add("Observacion", typeof(string));
            dt.Columns.Add("mode_sEstadoId", typeof(string));
            dt.Columns.Add("movi_IMovimientoId", typeof(string));
            dt.Columns.Add("mode_iMovimientoDetalleId", typeof(string));
            dt.Columns.Add("mode_iActuacionId", typeof(string));
            dt.Columns.Add("mode_iActuaciondetalleId", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            return dt;
        }

        private void TraeRangosDisponibles()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) != (int)Enumerador.enmTipoRol.OPERATIVO)
            {
                try
                {
                    CargarGrillaRangosDisponibles();
                    grdRangosDisponibles.Visible = true;
                    LblTotal.Visible = true;
                    LblTotalInsumos.Visible = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        //Se limpia el combo dejando solo los registros con los códigos especificados en la variables lstRegistrosConservar
        private void borrarRegistrosCombo(List<string> lstRegistrosConservar, DataTable dtRegistrosCombo)
        {
            for (int i = dtRegistrosCombo.Rows.Count - 1; i >= 0; i--)
            {
                if (!lstRegistrosConservar.Contains(dtRegistrosCombo.Rows[i]["para_sParametroId"].ToString()))
                    dtRegistrosCombo.Rows[i].Delete();
            }
        }

        private void ProcesarEstadosCombosOrigenDestino(bool bOficinaO, bool bTipoBovedaO, bool bBovedaO, bool bOficinaD, bool bTipoBovedaD, bool bBovedaD)
        {
            cboMisionConsO.Enabled = bOficinaO;
            cboTipoBovedaO.Enabled = bTipoBovedaO;
            cboBovedaO.Enabled = bBovedaO;

            cboMisionConsD.Enabled = bOficinaD;
            cboTipoBovedaD.Enabled = bTipoBovedaD;
            cboBovedaD.Enabled = bBovedaD;
        }

        private void llenar_cboMovimientoMotivo()
        {

            DataTable dt = oMovimientoConsultaBL.MovimientoMotivo(Convert.ToInt32(cboMovimientoTipo.SelectedValue), Convert.ToInt32(cboMisionConsD.SelectedValue));

            cboMovimientoMotivo.Items.Clear();
            cboMovimientoMotivo.DataTextField = "para_vDescripcion";
            cboMovimientoMotivo.DataValueField = "para_sParametroId";

            if (Session["PasaPedidoMovimiento"].ToString() == "0")
            {
                if (dt.Rows.Count > 0)
                {

                    List<string> list = new List<string>();

                    txtNumeroPedidoC.Text = "0";
                    Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];


                    if (cboMovimientoTipo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoTipo.ENTRADA).ToString())
                    {
                        list.Add(((int)Enumerador.enmMovimientoMotivo.CARGA_INICIAL).ToString());
                        borrarRegistrosCombo(list, dt);

                        if (enmAccion != Enumerador.enmAccion.ATENDER)
                        {
                            ProcesarEstadosCombosOrigenDestino(false, false, false, false, false, false);

                        }



                    }
                    else if (cboMovimientoTipo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoTipo.SALIDA).ToString())
                    {
                        list.Add(((int)Enumerador.enmMovimientoMotivo.POR_TRASLADO_INTERNO).ToString());
                        list.Add(((int)Enumerador.enmMovimientoMotivo.DETERIORADO).ToString());
                        list.Add(((int)Enumerador.enmMovimientoMotivo.INUTILIZADO).ToString());
                        list.Add(((int)Enumerador.enmMovimientoMotivo.POR_PERDIDA_ROBO).ToString());
                        list.Add(((int)Enumerador.enmMovimientoMotivo.DEVOLUCION).ToString());

                        if (enmAccion == Enumerador.enmAccion.CONSULTAR ||
                            enmAccion == Enumerador.enmAccion.ATENDER)
                        {
                            list.Add(((int)Enumerador.enmMovimientoMotivo.POR_PEDIDO).ToString());
                        }

                        borrarRegistrosCombo(list, dt);

                    }
                }
            }

            cboMovimientoMotivo.DataSource = dt;
            cboMovimientoMotivo.DataBind();
            cboMovimientoMotivo.Items.Insert(0, new ListItem("- SELECCIONAR -"));


            if (cboMovimientoTipo.SelectedValue.ToString() == ((int)Enumerador.enmMovimientoTipo.ENTRADA).ToString())
            {
                cboMovimientoMotivo.SelectedIndex = 1;
            }
            else
            {
                cboMovimientoMotivo.SelectedIndex = 0;
            }
        }

        private int CalculaTotal()
        {
            int sum = 0;
            for (int i = 0; i < grdRangos.Rows.Count; i++)
            {
                sum += int.Parse(grdRangos.Rows[i].Cells[ObtenerIndiceColumnaGrilla(grdRangos, "Cant")].Text);
            }
            return sum;
        }


        private void CargarOficinaConsular(ctrlOficinaConsular cboOficConsular, DropDownList cboTipoBoveda, DropDownList cboBoveda, string strOficConsularId, string strUsuarioId = "", bool bEsUsuario = false)
        {

            if (cboTipoBoveda.Items.Count == 0)
                Util.CargarParametroDropDownList(cboTipoBoveda, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA));

            cboOficConsular.SelectedValue = strOficConsularId;

            cboTipoBoveda.SelectedValue = bEsUsuario ? ((int)Enumerador.enmBovedaTipo.USUARIO).ToString() : ((int)Enumerador.enmBovedaTipo.MISION).ToString();
            
            //-----------------------------------------
            DataTable dtBovedas = new DataTable();

            dtBovedas = obtenerBovedas();

            DataView dvO = dtBovedas.Copy().DefaultView;
            //-----------------------------------------
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
                //-------------------------------------------
                DataTable dtBovedas = new DataTable();

                dtBovedas = obtenerBovedas();

                DataView dvO = dtBovedas.Copy().DefaultView;
                //-------------------------------------------

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

        #endregion

        protected void btnimprimir_Click(object sender, EventArgs e)
        {
            SGAC.BE.AL_MOVIMIENTO objMOVIMIENTO = new BE.AL_MOVIMIENTO();
            objMOVIMIENTO = ValoresNuevosCabecera();
            Session["OBJ_MOVIMIENTO"] = objMOVIMIENTO;
            string strScript = string.Empty;
            strScript += "window.open('" + "../Almacen/FrmReporte.aspx" + "', 'popup_window', 'scrollbars=1,resizable=1,fullscreen=yes');";


            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
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
      
        //************************
    }
}
