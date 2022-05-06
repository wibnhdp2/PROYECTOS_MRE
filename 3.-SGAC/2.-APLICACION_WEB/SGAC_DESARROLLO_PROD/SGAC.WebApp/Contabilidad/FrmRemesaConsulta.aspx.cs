using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Contabilidad.Remesa.BL;
using SGAC.Contabilidad.CuentaCorriente.BL;
using SGAC.Contabilidad.Reportes.BL;
using SGAC.Auditoria.BL;

namespace SGAC.WebApp.Contabilidad
{
    public partial class RemesaConsulta : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "REMESA";
        private string strVariableAccion = "Remesa_Accion";
        private string strVariableDt = "Remesa_Tabla";
        private string strVariableDetalleDt = "RemesaDetalle_Tabla";
        private string strVariableIndice = "Remesa_Indice";
        private string strVariableDetalleIndice = "RemesaDetalle_Indice";
        private int intRemesaOtroMes = 1, intRemesaActual = 0;

        private int intCeldaReme_Estado = 7;
        private int intCeldaReme_Oficina = 11;

        private int intCeldaRede_Total = 6;
        private int intCeldaRede_Dolares = 7;

        private int intColRede_Editar = 8;
        private int intColRede_Anular = 9;
        #endregion

        #region EVENTOS
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;

            ctrlPaginadorDetalle.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorDetalle.Visible = false;
            ctrlPaginadorDetalle.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnPrintHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnConfiguration.Text = "     Enviar";
            ctrlToolBarMantenimiento.btnConfiguration.CssClass = "btnMail";
            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnConfigurationHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonConfigurationClick(ctrlToolBarMantenimiento_btnConfigurationHandler);

            ctrlToolBarMantenimiento.btnConfiguration.OnClientClick = "return ValidarEnvioRemesa()";
            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return ValidarRegistrarRemesa()";

            btnAceptarDetalle.OnClientClick = "return ValidarDetalleActual()";
            btnAceptarOtroMes.OnClientClick = "return ValidarDetalleOtroMes()";

            btnCalcular.OnClientClick = "return ValidarDatosCalculo_OtroMes()";
            btnCalcActual.OnClientClick = "return ValidarDatosCalculo()";
            
            this.dtpFecInicio.StartDate = new DateTime(1900, 1, 1);
            this.dtpFecInicio.EndDate = DateTime.Now;

            this.dtpFecFin.StartDate = new DateTime(1900, 1, 1);            

            this.datFechaDetalle.StartDate = new DateTime(1900, 1, 1);            
            this.datFechaDetalle.EndDate = DateTime.Today;

            this.datFechaOtroMes.StartDate = new DateTime(1900, 1, 1);
            this.datFechaOtroMes.EndDate = DateTime.Now;

            hdn_ofco_sOficinaConsularId.Value = Convert.ToString(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            hdn_ofco_sReferenciaId.Value = Convert.ToString(Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID]);

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, gdvRemesa, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();
                HabilitarMantenimiento(true);
                InicializarControlesRemesaDetalle();
                //-------------------------------------------------------
                // Fecha: 26/01/2017
                // Autor: Miguel Márquez Beltrán
                // Objetivo: Mostrar mensaje de Pendiente y/o Enviados
                //-------------------------------------------------------
                lblMsjPendienteEnvio.Text = "";
                lblMsjEnviados.Text = "";

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.ADMINISTRATIVO ||
                    Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.SUPERADMIN)
                {
                    ConsultarAlerta();
                }

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ddlOficinaOrigenCons.Enabled = false;
                    ddlOficinaDestinoCons.Enabled = false;
                }
                //-------------------------------------------------------
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar, ctrlToolBarMantenimiento.btnConfiguration};                
                Comun.ModoLectura(ref arrButtons);
            }
        }

        protected void gdvRemesa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;

            int intSeleccionado = Convert.ToInt32(e.CommandArgument);
            Session[strVariableIndice] = intSeleccionado;
            txtObservacion.Enabled = false;
            lblObserv.Visible = false;
            ddlOficinaDestinoMant.Enabled = false;

            int intEstadoRemesa = 0; int intOCOrigen = 0; int intOCDestino = 0; int intOCLogeo = 0;

            intEstadoRemesa = Convert.ToInt32(gdvRemesa.DataKeys[intSeleccionado].Values["reme_sEstadoId"].ToString());
            intOCOrigen = Convert.ToInt32(gdvRemesa.DataKeys[intSeleccionado].Values["reme_sOficinaConsularOrigenId"].ToString());
            intOCDestino = Convert.ToInt32(gdvRemesa.DataKeys[intSeleccionado].Values["reme_sOficinaConsularDestinoId"].ToString());
            intOCLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaDestinoMant.Cargar(true);
            }
            else
            {
                ddlOficinaDestinoMant.CargarPorOficinasConsulares(
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] + "," +
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());
            }

            Util.CargarParametroDropDownList(ddl_rede_actual_tipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_TRANSACCION), true);
            
            if (e.CommandName == "Consultar")
            {
                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

                if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.ENVIADA && intOCLogeo == intOCOrigen) ctrlToolBarMantenimiento.btnEditar.Enabled = false;

                if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.APROBADA) ctrlToolBarMantenimiento.btnEditar.Enabled = false;

                if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.ANULADA) ctrlToolBarMantenimiento.btnEditar.Enabled = false;

                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;
                HabilitarMantenimiento(false);
                if (intOCLogeo == intOCDestino)
                {
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                }
                else if (intOCLogeo == intOCDestino)
                {
                    // nada
                }
                else
                {
                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                }
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                //if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.CERRADA) return;
                if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.ANULADA) return;
                if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.APROBADA) return;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
                hEstadoRemesa.Value = "0";
                if (intOCLogeo == intOCOrigen)
                {
                    #region OC Login es ORIGEN
                    txtObservacion.Enabled = false;
                    lblObserv.Visible = false;
                    ddlRemesaEstadoMant.Enabled = false;
                    ddlOficinaDestinoMant.Enabled = false;
                    if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.PENDIENTE)
                    {
                        #region PENDIENTE
                        ctrlToolBarMantenimiento.btnConfiguration.Enabled = true;
                        HabilitarMantenimiento(true);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                        ddlOficinaDestinoMant.Enabled = true;
                        #endregion
                    }
                    else if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.ENVIADA)
                    {
                        #region ENVIADA
                        ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                        ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                        ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;
                        HabilitarMantenimiento(false);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                        #endregion
                    }
                    else if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.OBSERVADA)
                    {
                        hEstadoRemesa.Value = intEstadoRemesa.ToString();
                        ctrlToolBarMantenimiento.btnConfiguration.Enabled = true;
                        HabilitarMantenimiento(true);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    }
                    else {
                        ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;
                        HabilitarMantenimiento(true);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    }
                    #endregion
                }
                else if (intOCLogeo == intOCDestino)
                {
                    #region OC Login es DESTINO
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = true;

                    // Cargar estados remesa
                    if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.APROBADA && intOCDestino == Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.PENDIENTE).ToString()).Enabled = false;
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.ENVIADA).ToString()).Enabled = false;
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.OBSERVADA).ToString()).Enabled = false;
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.ANULADA).ToString()).Enabled = false;
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.APROBADA).ToString()).Enabled = false;
                    }
                    else
                    {
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.PENDIENTE).ToString()).Enabled = false;
                        ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.ENVIADA).ToString()).Enabled = false;
                        //ddlRemesaEstadoMant.Items.FindByValue(((int)Enumerador.enmRemesaEstado.CERRADA).ToString()).Enabled = false;
                    }

                    if (intEstadoRemesa != (int)Enumerador.enmRemesaEstado.ENVIADA &&
                        intEstadoRemesa != (int)Enumerador.enmRemesaEstado.OBSERVADA &&
                        intEstadoRemesa != (int)Enumerador.enmRemesaEstado.APROBADA)
                    {
                        ctrlValidacion.MostrarValidacion("No tiene Permisos para Editar", true, Enumerador.enmTipoMensaje.ERROR);
                        ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                        ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                        ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                        HabilitarMantenimiento(false);
                        strScript = Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);
                    }

                    if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.ENVIADA ||
                        intEstadoRemesa == (int)Enumerador.enmRemesaEstado.OBSERVADA)
                    {
                        ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                        ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                        ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

                        HabilitarMantenimiento(false);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                        txtObservacion.Enabled = true;
                        lblObserv.Visible = true;
                        ddlOficinaDestinoMant.Enabled = false;
                        ddlRemesaEstadoMant.Enabled = true;
                        ddlRemesaEstadoMant.SelectedValue = ((int)Enumerador.enmRemesaEstado.APROBADA).ToString();
                    }

                    if (intEstadoRemesa == (int)Enumerador.enmRemesaEstado.APROBADA && intOCDestino == Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                        ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                        ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                        HabilitarMantenimiento(false);
                        strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                        txtObservacion.Enabled = false;
                        //lblObserv.Visible = false;
                        ddlOficinaDestinoMant.Enabled = false;
                        ddlRemesaEstadoMant.Enabled = true;
                        //ddlRemesaEstadoMant.SelectedValue = ((int)Enumerador.enmRemesaEstado.CERRADA).ToString();
                    }
                    #endregion
                }
                else
                {
                    ctrlValidacion.MostrarValidacion("No tiene Permisos para Editar", true, Enumerador.enmTipoMensaje.ERROR);
                    updConsulta.Update();
                    return;
                }                
            }

            PintarSeleccionado();
            CargarDetalle();

            Comun.EjecutarScript(Page, strScript);

            if (intOCLogeo == intOCDestino)
            {
                if (intEstadoRemesa != (int)Enumerador.enmRemesaEstado.ENVIADA &&
                      intEstadoRemesa != (int)Enumerador.enmRemesaEstado.OBSERVADA &&
                        intEstadoRemesa != (int)Enumerador.enmRemesaEstado.APROBADA)
                {
                    updConsulta.Update();
                }
            }
        }
        protected void gdvRemesa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.ANULADA ||
                    //Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.CERRADA ||
                    Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.APROBADA )
                {
                    e.Row.FindControl("btnEditar").Visible = false;
                }
                if (Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.APROBADA 
                            && Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    // oficina logueada diferente a Lima y estado APROBADA
                    e.Row.FindControl("btnEditar").Visible = false;
                }
                else if (Convert.ToInt32(e.Row.Cells[intCeldaReme_Oficina].Text.Trim()) != Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) 
                        && Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.PENDIENTE)
                {
                    // Oficina consular logueada diferente a ORIGEN y estado pendiente
                    e.Row.FindControl("btnEditar").Visible = false;
                }
                else if (Convert.ToInt32(e.Row.Cells[intCeldaReme_Oficina].Text.Trim()) == Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
                        && Convert.ToInt32(e.Row.Cells[intCeldaReme_Estado].Text.Trim()) == (int)Enumerador.enmRemesaEstado.ENVIADA)
                {
                    // oficina consular ORIGEN con estado enviado no editar
                    e.Row.FindControl("btnEditar").Visible = false;
                }
            }
        }

        protected void gdvRemesaDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!ctrlToolBarMantenimiento.btnGrabar.Enabled) return;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            if ((Enumerador.enmAccion)Session[strVariableAccion] == Enumerador.enmAccion.INSERTAR)
            {
                if (e.CommandName == "Eliminar")
                {
                    DataTable dtRemesaDetalle = ((DataTable)Session[strVariableDetalleDt]).Copy();

                    txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"],
                        (
                        Convert.ToDouble(txtRemesaTotal.Text.Trim()) -
                        Convert.ToDouble(gdvRemesaDetalle.Rows[intSeleccionado].Cells[intCeldaRede_Total].Text.Trim())
                        ));

                    txtRemesaTotalDolares.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"],
                        (
                        Convert.ToDouble(txtRemesaTotal.Text.Trim()) -
                        Convert.ToDouble(gdvRemesaDetalle.Rows[intSeleccionado].Cells[intCeldaRede_Dolares].Text.Trim())
                        ));

                    dtRemesaDetalle.Rows[intSeleccionado].Delete();
                    dtRemesaDetalle.AcceptChanges();
                    Session[strVariableDetalleDt] = dtRemesaDetalle;

                    gdvRemesaDetalle.DataSource = dtRemesaDetalle;
                    gdvRemesaDetalle.DataBind();

                    //tDetalle.Visible = false;
                    //tRemesaOtroMes.Visible = false;

                    updMantenimiento.Update();
                    updRemesaDetalle.Update();

                }
                else if (e.CommandName == "Editar")
                {
                    Session[strVariableDetalleIndice] = intSeleccionado;
                    PintarDetalleSelEnPantalla(intSeleccionado);
                }
            }
            else
            {
                int intEstadoRemesa = 0;
                intEstadoRemesa = Convert.ToInt32(ObtenerFilaSeleccionada()["reme_sEstadoId"]);
                if (intEstadoRemesa != (int)Enumerador.enmRemesaEstado.APROBADA || intEstadoRemesa != (int)Enumerador.enmRemesaEstado.ANULADA)
                {
                    int intOCOrigen = 0; int intOCDestino = 0; int intOCLogeo = 0;
                    intOCOrigen = Convert.ToInt32(ObtenerFilaSeleccionada()["reme_sOficinaConsularOrigenId"]);
                    intOCDestino = Convert.ToInt32(ObtenerFilaSeleccionada()["reme_sOficinaConsularDestinoId"]);
                    intOCLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    if (intOCLogeo == intOCOrigen)
                    {
                        if (intEstadoRemesa != (int)Enumerador.enmRemesaEstado.ENVIADA)
                        {
                            if (e.CommandName == "Eliminar")
                            {
                                DataTable dtRemesaDetalle = ((DataTable)Session[strVariableDetalleDt]).Copy();

                                txtRemesaTotal.Text = 
                                    (Convert.ToDouble(txtRemesaTotal.Text.Trim()) -
                                    Convert.ToDouble(gdvRemesaDetalle.Rows[intSeleccionado].Cells[intCeldaRede_Total].Text.Trim())).ToString("#0.000");

                                dtRemesaDetalle.Rows[intSeleccionado].Delete();
                                dtRemesaDetalle.AcceptChanges();
                                Session[strVariableDetalleDt] = dtRemesaDetalle;

                                gdvRemesaDetalle.DataSource = dtRemesaDetalle;
                                gdvRemesaDetalle.DataBind();

                                tDetalle.Visible = false;
                                tRemesaOtroMes.Visible = false;
                                updMantenimiento.Update();
                                updRemesaDetalle.Update();

                            }
                            else if (e.CommandName == "Editar")
                            {
                                Session[strVariableDetalleIndice] = intSeleccionado;
                                PintarDetalleSelEnPantalla(intSeleccionado);
                            }
                        }
                    }
                }
            }
        }        

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }
        protected void ctrlPaginadorDetalle_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updRemesaDetalle.Update();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            DateTime? datFechaInicio = new DateTime();
            DateTime? datFechaFin = new DateTime();

            ctrlPaginador.InicializarPaginador();

            if (ddlOficinaOrigenCons.SelectedItem.ToString().Contains("SELECC"))
            {
                ctrlValidacion.MostrarValidacion("Seleccione Oficina Consular Origen.", true, Enumerador.enmTipoMensaje.ERROR);
                return;
            }

            //if (dtpFecInicio.Text == string.Empty || dtpFecFin.Text == string.Empty)
            //{
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

            if (dtpFecInicio.Text != string.Empty)
                datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            else
                datFechaInicio = null;

            if (dtpFecFin.Text != string.Empty)
                datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            else
                datFechaFin = null;

            if (datFechaInicio > datFechaFin)
            {
                Session[strVariableDt] = new DataTable();
                gdvRemesa.DataSource = new DataTable();
                gdvRemesa.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                CargarGrilla();
            }
        }

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            #region Filtros Reporte
            int intOficinaConsularOrigen = 0;
            int intOficinaConsularDestino = 0;
            int intTipo = 0;
            int intEstado = 0;

            DateTime datFechaInicio = DateTime.Today;
            DateTime datFechaFin = DateTime.Today;

            //---------------------------------------------------------------------------------------
            //Fecha: 07/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Lima en la consulta tiene la opción todos o seleccionar un consulado.
            //          El consulado selecciona su consulado o dependientes por defecto.
            //---------------------------------------------------------------------------------------
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlOficinaOrigenCons.SelectedIndex > 0)
                {
                    intOficinaConsularOrigen = Convert.ToInt32(ddlOficinaOrigenCons.SelectedValue);
                }
            }
            else
            {
                intOficinaConsularOrigen = Convert.ToInt32(ddlOficinaOrigenCons.SelectedValue);
            }
            //---------------------------------------------------------------------------------------
            if (ddlOficinaDestinoCons.SelectedValue != string.Empty)
            {
                intOficinaConsularDestino = Convert.ToInt32(ddlOficinaDestinoCons.SelectedValue);
            }

            if (ddlRemesaTipoConsulta.SelectedValue != null)
            {
                intTipo = Convert.ToInt32(ddlRemesaTipoConsulta.SelectedValue);
            }

            if (ddlRemesaEstadoConsulta.SelectedValue != null)
            {
                intEstado = Convert.ToInt32(ddlRemesaEstadoConsulta.SelectedValue);
            }

            if (dtpFecInicio.Text != string.Empty)
            {
                datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);                
            }

            if (dtpFecFin.Text != string.Empty)
            {
                datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);                
            }
                       
            #endregion

            // Variable Sesión: Paramétros 
            object[] arrParametros = {  intOficinaConsularOrigen, intOficinaConsularDestino,
                                        intEstado, intTipo,
                                        datFechaInicio, datFechaFin,
                                        Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()), 
                                        Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]) };

            ReporteConsultasBL objBL = new ReporteConsultasBL();
            DataTable dt = new DataTable();

            dt = objBL.ObtenerRemesasConsulares(intOficinaConsularOrigen,
                                        intOficinaConsularDestino,
                                        intTipo,
                                        intEstado,                                        
                                        datFechaInicio,
                                        datFechaFin,
                                        Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()),
                                        Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]));

            Proceso p = new Proceso(); 
            if (p.IErrorNumero != 0)
            {   // Hay error
                ctrlValidacion.MostrarValidacion(p.vErrorMensaje, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                Session["FechaIntervalo"] = " Del " + dtpFecInicio.Text + " al " + dtpFecFin.Text;
                Session["SP_PARAMETROS"] = arrParametros;
                Session["dtDatos"] = dt;
                if (dt.Rows.Count > 0)
                {
                    Session["IdOficinaConsular_contabilidad"] = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt.Copy());
                    Comun.VerVistaPrevia(Session, Page, ds, Enumerador.enmReporteContable.REMESA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
            }
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            LimpiarDatosConsulta();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            txtObservacion.Enabled = false;
            lblObserv.Visible = false;
            //lblRemesaTipoMant.Visible = false;
            //lblMensaje.Visible = false;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaDestinoMant.Cargar(true);
            }
            else
            {
                ddlOficinaDestinoMant.CargarPorOficinasConsulares(
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] + "," +
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());
            }

            DropDownList ddlOficinaDestinoMan = (DropDownList)ddlOficinaDestinoMant.FindControl("ddlOficinaConsular");
            ddlOficinaDestinoMan.Items.Remove(ddlOficinaDestinoMan.Items.FindByValue(ddlOficinaOrigenMant.SelectedValue));

            InicializarControlesRemesaDetalle();

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

            if (Convert.ToInt32(ddlRemesaEstadoMant.SelectedValue) == (int)Enumerador.enmRemesaEstado.PENDIENTE)
            {
                ctrlToolBarMantenimiento.btnConfiguration.Enabled = true;
            }
            else
            {
                ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;
            }

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
            if (ddlRemesaMes.SelectedValue.ToString() == "0")
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Debe Ingresar el periodo al que pertenece la remesa consular."));
                ddlRemesaMes.Focus();
                return;
            }
            RemesaMantenimientoBL BL = new RemesaMantenimientoBL();
            bool Error = true;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            bool bobserv = false;
            switch (Convert.ToInt32(ddlRemesaEstadoMant.SelectedValue))
            {
                case (int)Enumerador.enmRemesaEstado.ANULADA:
                    if (txtObservacion.Text.Trim() == "") bobserv = true;
                    break;
                case (int)Enumerador.enmRemesaEstado.OBSERVADA:
                    if (txtObservacion.Text.Trim() == "") bobserv = true;
                    break;
            }

            if (bobserv)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "REMESA CONSULAR", "Ingrese una Observación."); ;
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (ddlOficinaOrigenMant.SelectedValue == ddlOficinaDestinoMant.SelectedValue)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "REMESA CONSULAR", "La oficina consular de origen debe ser diferente a la destino."); ;
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            if (ddlRemesaEstadoMant.SelectedValue == "0")
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "REMESA CONSULAR", "Seleccione un estado"); ;
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:

                    if (gdvRemesaDetalle.Rows.Count < 1)
                    {
                        string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "REMESA CONSULAR", "Ingrese un detalle."); ;
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }

                    BL.Insert(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    BL.Update(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);
                    break;
                case Enumerador.enmAccion.ELIMINAR:;
                    BL.Eliminar(ObtenerEntidadMantenimiento(), ref Error);
                    break;
            }

            if (Error == false)
            {
                LimpiarDatosMantenimiento();
                HabilitarMantenimiento();

                string strScript = string.Empty;
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                CargarGrilla();
                Comun.EjecutarScript(Page, strScript);
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del sistema. Contactese con soporte técnico."));
            }
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            LimpiarDatosMantenimiento();
            LimpiarDatosDetalle();
            HabilitarMantenimiento();
            InicializarControlesRemesaDetalle();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }

        /// <summary>
        /// Evento para enviar remesa
        /// </summary>
        void ctrlToolBarMantenimiento_btnConfigurationHandler()
        {
            #region Envío Correo
            string strScript = string.Empty;

            string strSMTPServer = string.Empty;
            string strSMTPPuerto = string.Empty;
            string strEmailFrom = string.Empty;
            string strEmailPassword = string.Empty;
            string strEmailTo = string.Empty;
            string strEmailCC = string.Empty;
            string strEmailCCO = string.Empty;

            strSMTPServer = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.SERVIDOR, "descripcion");
            strSMTPPuerto = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.PUERTO, "descripcion");

            strEmailFrom = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CORREO_DE, "descripcion");
            strEmailPassword = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CONTRASENIA, "descripcion");

            /*Jonatan: Se agrego el parametro de Correo_Para*/
            strEmailTo = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CORREO_PARA, "descripcion");

            //if (ConfigurationManager.AppSettings["EmailTo"] != null)
            //    strEmailTo = ConfigurationManager.AppSettings["EmailTo"].ToString();
            if (ConfigurationManager.AppSettings["EmailCC"] != null)
                strEmailCC = ConfigurationManager.AppSettings["EmailCC"].ToString();
            if (ConfigurationManager.AppSettings["EmailCCO"] != null)
                strEmailCCO = ConfigurationManager.AppSettings["EmailCCO"].ToString();
            
            string strTitulo = "Envío de Remesa - " + Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE];
            string strCuerpo = "";
            bool bEnabledSSL = true;
            bool bIsBodyHtml = false;

            strCuerpo += "\n" + "-----------------------------------------------------";
            strCuerpo += "\n";
            strCuerpo += "\n" + "Fecha: " + Comun.ObtenerFechaActualTexto(Session);
            strCuerpo += System.Environment.NewLine;
            strCuerpo += "OC Origen: " + ddlOficinaOrigenMant.SelectedItem.Text;
            strCuerpo += System.Environment.NewLine;
            strCuerpo += "OC Destino: " + ddlOficinaDestinoMant.SelectedItem.Text;
            strCuerpo += System.Environment.NewLine;
            //strCuerpo += "Tipo: " + ddlRemesaTipoMant.SelectedItem.Text;
            //strCuerpo += System.Environment.NewLine;
            strCuerpo += "Monto: " + string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txtRemesaTotal.Text));
            strCuerpo += "\n";
            strCuerpo += "\n" + "-----------------------------------------------------";
            strCuerpo += "\n" + "Usuario: " + Session[Constantes.CONST_SESION_USUARIO];
            strCuerpo += "\n" + "Fecha Envío: " + DateTime.Now.ToString(WebConfigurationManager.AppSettings["FormatoFechas"] + " HH:mm:ss");

            // ENVIAR CORREO
            Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
            bool bEnviado = false;
            string strMensaje = string.Empty;
            string strCorreo = string.Empty;
            try
            {
                bEnviado = Correo.EnviarCorreo(strSMTPServer, strSMTPPuerto,
                                               strEmailFrom, strEmailPassword,
                                               strEmailTo, strEmailCC, strEmailCCO,
                                               strTitulo, strCuerpo, bEnabledSSL, bIsBodyHtml, System.Net.Mail.MailPriority.High, null);
            }
            catch (Exception ex)
            {
                strCorreo += " (Error al enviar el correo: " + ex.Message + ")";

                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ENVIO CORREO",
                    audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = ex.Message,
                    audi_vMensaje = ex.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion
            }
            //--

            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Correo enviado exitosamente.", false, 160, 300);
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;
            #endregion

            #region Actualización Estado
            
            RemesaMantenimientoBL BL = new RemesaMantenimientoBL();
            bool Error = true;

            DataRow dr = ObtenerFilaSeleccionada();
            if (dr != null)
            {
                int intRemesaId = Convert.ToInt32(dr["reme_iRemesaId"]);
                int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                BL.ActualizarEstado(intRemesaId,
                                    0,
                                    (int)Enumerador.enmRemesaEstado.ENVIADA,
                                    Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                    Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(),
                                    intOficinaConsularId,
                                    ref Error);

                if (Error == false)
                {
                    ddlRemesaEstadoMant.SelectedValue = ((int)Enumerador.enmRemesaEstado.ENVIADA).ToString();
                    datFechaEnvio.Text = Comun.ObtenerFechaActualTexto(Session);
                }
                else
                {
                    enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                    strMensaje = "Error en la actualización de la Remesa. Verificar datos.";
                }
            }
            
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();

            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.HabilitarTab(0);

            CargarGrilla();

            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            #endregion

            string strScriptCorreo = string.Empty;
            if (!bEnviado)
            {
                enmTipoMensaje = Enumerador.enmTipoMensaje.ERROR;
                strCorreo = "Correo no enviado." + strCorreo;
                strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strCorreo);

                Comun.EjecutarScript(Page, strScriptCorreo);
            }
            else
            {
                if (strMensaje != string.Empty)
                {
                    strScript += Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strMensaje);
                }

                Comun.EjecutarScript(Page, strScript);
            }
        }

        protected void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            InicializarControlesRemesaDetalle();
        }
        protected void btnRemesaOtroMes_Click(object sender, EventArgs e)
        {
            ActivarBotonRemesaDetalle(tRemesaOtroMes, btnRemesaOtroMes, tDetalle, btnAgregarDetalle);
            LimpiarDatosRemesaDetalle();
        }

        /// <summary>
        /// Agregar Remesa Detalle a la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarDetalle_Click(object sender, EventArgs e)
        {
            if (ddlMesActual.SelectedValue.ToString() == "0")
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Debe Ingresar el periodo al que pertenece el detalle de la remesa consular."));
                ddlMesActual.Focus();
                return;
            }

            if (datFechaDetalle.Text.Trim() != "")
            {
                if (Comun.EsFecha(datFechaDetalle.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "La fecha de depósito no es válida."));                    
                    return;
                }
            }

            DataTable dt = (DataTable)Session[strVariableDetalleDt];
            DataRow dr;
            if (dt == null)
            {
                dt = CrearTablaRemesaDetalle();
            }

            int intRemesaDetalleSel = Convert.ToInt32(Session[strVariableDetalleIndice]);

            //datFechaDetalle.Text = hd_fecha.Value;
            if (intRemesaDetalleSel == -1)
            {
                // INSERTAR
                #region Remesa Detalle Insertar
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    dr = dt.NewRow();
                    dr["rede_iRemesaDetalleId"] = 0;
                    dr["rede_iRemesaId"] = ObtenerFilaSeleccionada()["reme_iRemesaId"];
                    dr["rede_sTipoId"] = ddl_rede_actual_tipo.SelectedValue;
                    dr["rede_vTipo"] = ddl_rede_actual_tipo.SelectedItem.Text;
                    dr["rede_cPeriodo"] = ObtenerFilaSeleccionada()["reme_cPeriodo"];
                    dr["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaDetalle.Text);
                    dr["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_actual_total.Text));

                    if (txt_rede_FTipoCambioBancario.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], 0);
                    else
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_FTipoCambioBancario.Text));

                    if (txt_rede_FTipoCambioConsular.Text.Trim() != string.Empty)
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], 0);                        
                    else
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_FTipoCambioConsular.Text));

                    dr["rede_FTotalDolares"] = CalcularCompraDeDivisas(dr["rede_FMonto"].ToString(), dr["rede_FTipoCambioBancario"].ToString());
                    dr["rede_sBancoId"] = ddl_rede_actual_banco.SelectedValue;
                    dr["rede_sCuentaCorrienteId"] = ddl_rede_actual_cuenta.SelectedValue;
                    dr["rede_sMonedaId"] = ddl_rede_actual_Moneda.SelectedValue;
                    dr["rede_vNroVoucher"] = txt_rede_actual_voucher.Text.Trim();
                    dr["rede_vObservacion"] = txtObsDetalle.Text.Trim().ToUpper();


                    dr["rede_vResponsableEnvio"] = txtResponsableEnvio.Text.Trim().ToUpper();
                    dr["rede_bMesFlag"] = intRemesaActual;
                    dr["rede_sEstadoId"] = ObtenerFilaSeleccionada()["reme_sEstadoId"];
                    dr["rede_vEstado"] = ObtenerFilaSeleccionada()["reme_vEstado"];
                    dr["rede_sClasificacionId"] = ddlClasificacion.SelectedValue;
                    dr["rede_vClasificacion"] = ddlClasificacion.SelectedItem.Text;
                }
                else
                {
                    dr = dt.NewRow();
                    dr["rede_iRemesaDetalleId"] = 0;
                    dr["rede_iRemesaId"] = 0;
                    dr["rede_sTipoId"] = ddl_rede_actual_tipo.SelectedValue;
                    dr["rede_vTipo"] = ddl_rede_actual_tipo.SelectedItem.Text;
                    dr["rede_cPeriodo"] = ddlAnioActual.SelectedValue + "-" +(ddlMesActual.SelectedIndex).ToString().PadLeft(2, '0');
                    dr["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaDetalle.Text);
                    dr["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_actual_total.Text));
                    if (txt_rede_FTipoCambioBancario.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], 0);
                    else
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_FTipoCambioBancario.Text.Trim()));

                    if (txt_rede_FTipoCambioConsular.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], 0);
                    else
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_FTipoCambioConsular.Text.Trim()));

                    dr["rede_FTotalDolares"] = CalcularCompraDeDivisas(dr["rede_FMonto"].ToString(), dr["rede_FTipoCambioBancario"].ToString());

                    dr["rede_sBancoId"] = ddl_rede_actual_banco.SelectedValue;
                    dr["rede_sCuentaCorrienteId"] = ddl_rede_actual_cuenta.SelectedValue;
                    dr["rede_sMonedaId"] = ddl_rede_actual_Moneda.SelectedValue;
                    dr["rede_vNroVoucher"] = txt_rede_actual_voucher.Text.Trim();
                    dr["rede_vObservacion"] = txtObsDetalle.Text.Trim().ToUpper();
                    dr["rede_vResponsableEnvio"] = txtResponsableEnvio.Text.Trim().ToUpper();
                    dr["rede_bMesFlag"] = intRemesaActual;
                    dr["rede_sEstadoId"] = (int)Enumerador.enmRemesaEstado.PENDIENTE;
                    dr["rede_vEstado"] = Enumerador.enmRemesaEstado.PENDIENTE.ToString();
                    dr["rede_sClasificacionId"] = ddlClasificacion.SelectedValue;
                    dr["rede_vClasificacion"] = ddlClasificacion.SelectedItem.Text;
                }
                dt.Rows.Add(dr);
                #endregion
            }
            else
            {   // MODIFICAR
                #region Remesa Detalle Modificar
                dt.Rows[intRemesaDetalleSel]["rede_sTipoId"] = ddl_rede_actual_tipo.SelectedValue;
                dt.Rows[intRemesaDetalleSel]["rede_vTipo"] = ddl_rede_actual_tipo.SelectedItem.Text;
                dt.Rows[intRemesaDetalleSel]["rede_cPeriodo"] = ddlAnioActual.SelectedValue + "-" +  (ddlMesActual.SelectedIndex).ToString().PadLeft(2, '0');
                dt.Rows[intRemesaDetalleSel]["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaDetalle.Text);
                dt.Rows[intRemesaDetalleSel]["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_actual_total.Text));
                dt.Rows[intRemesaDetalleSel]["rede_vObservacion"] = txtObsDetalle.Text.Trim().ToUpper();
                if (txt_rede_FTipoCambioBancario.Text.Trim() == string.Empty)
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], "0");
                else
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_FTipoCambioBancario.Text.Trim()));

                if (txt_rede_FTipoCambioConsular.Text.Trim() == string.Empty)
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], "0");
                else
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_FTipoCambioConsular.Text.Trim()));

                dt.Rows[intRemesaDetalleSel]["rede_FTotalDolares"] = CalcularCompraDeDivisas(dt.Rows[intRemesaDetalleSel]["rede_FMonto"].ToString(), dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioBancario"].ToString());

                if (ddl_rede_actual_banco.SelectedIndex > 0)
                    dt.Rows[intRemesaDetalleSel]["rede_sBancoId"] = ddl_rede_actual_banco.SelectedValue;
                if (ddl_rede_actual_cuenta.SelectedIndex > 0)
                    dt.Rows[intRemesaDetalleSel]["rede_sCuentaCorrienteId"] = ddl_rede_actual_cuenta.SelectedValue;
                if (ddl_rede_actual_Moneda.SelectedIndex > 0)
                    dt.Rows[intRemesaDetalleSel]["rede_sMonedaId"] = ddl_rede_actual_Moneda.SelectedValue;
                if (txt_rede_actual_voucher.Text.Trim() != string.Empty)
                    dt.Rows[intRemesaDetalleSel]["rede_vNroVoucher"] = txt_rede_actual_voucher.Text.Trim();

                dt.Rows[intRemesaDetalleSel]["rede_vResponsableEnvio"] = txtResponsableEnvio.Text.Trim().ToUpper();
                dt.Rows[intRemesaDetalleSel]["rede_sClasificacionId"] = ddlClasificacion.SelectedValue;
                dt.Rows[intRemesaDetalleSel]["rede_vClasificacion"] = ddlClasificacion.SelectedItem.Text;
                
                dt.AcceptChanges();
                #endregion
            }

            gdvRemesaDetalle.DataSource = dt;
            gdvRemesaDetalle.DataBind();
            Session[strVariableDetalleDt] = dt;
            LimpiarDatosRemesaDetalle();

            double dblTotalRemesaDetalle = 0;
            dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FMonto");
            txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);
            dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FTotalDolares");
            txtRemesaTotalDolares.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);
            updMantenimiento.Update();
        }

        /// <summary>
        /// Agregar Remesa Detalle de Otro Mes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOtroMes_Click(object sender, EventArgs e)
        {
            if (datFechaOtroMes.Text.Trim() != "")
            {
                if (Comun.EsFecha(datFechaOtroMes.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "La fecha de depósito no es válida."));
                    return;
                }
            }

            DataTable dt = (DataTable)Session[strVariableDetalleDt];
            DataRow dr;
            if (dt == null)
            {
                dt = CrearTablaRemesaDetalle();
            }

            int intRemesaDetalleSel = Convert.ToInt32(Session[strVariableDetalleIndice]);
            if (intRemesaDetalleSel == -1)
            {   // INSERTAR
                #region Remesa Detalle Insertar
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    dr = dt.NewRow();
                    dr["rede_iRemesaDetalleId"] = 0;
                    dr["rede_iRemesaId"] = ObtenerFilaSeleccionada()["reme_iRemesaId"];
                    dr["rede_sTipoId"] = ddlTipoRemesaOtroMes.SelectedValue;
                    dr["rede_vTipo"] = ddlTipoRemesaOtroMes.SelectedItem.Text;
                    dr["rede_sEstadoId"] = ObtenerFilaSeleccionada()["reme_sEstadoId"];
                    dr["rede_vEstado"] = ObtenerFilaSeleccionada()["reme_vEstado"];
                    dr["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaOtroMes.Text);

                    dr["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_otro_total.Text));

                    if (txt_rede_otro_FTipoCambioBancario.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], 0);                        
                    else
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_otro_FTipoCambioBancario.Text));

                    if (txt_rede_otro_FTipoCambioConsular.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], 0);
                    else
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_otro_FTipoCambioConsular.Text));

                    dr["rede_FTotalDolares"] = CalcularCompraDeDivisas(dr["rede_FMonto"].ToString(), dr["rede_FTipoCambioBancario"].ToString());

                    dr["rede_sBancoId"] = ddl_rede_otro_banco.SelectedValue;
                    dr["rede_sCuentaCorrienteId"] = ddl_rede_otro_cuenta.SelectedValue;
                    dr["rede_sMonedaId"] = ddl_rede_otro_moneda.SelectedValue;
                    dr["rede_vNroVoucher"] = txt_rede_otro_voucher.Text.Trim();
                    dr["rede_cPeriodo"] = ddlAnioOtro.SelectedValue + (ddlMesOtro.SelectedIndex + 1).ToString().PadLeft(2, '0');
                    dr["rede_vResponsableEnvio"] = txtTitularDatos.Text.Trim().ToUpper();
                    dr["rede_vNroVoucher"] = txt_rede_otro_voucher.Text;
                    dr["rede_bMesFlag"] = intRemesaOtroMes;
                }
                else
                {
                    dr = dt.NewRow();
                    dr["rede_iRemesaDetalleId"] = 0;
                    dr["rede_iRemesaId"] = 0;
                    dr["rede_sTipoId"] = ddlTipoRemesaOtroMes.SelectedValue;
                    dr["rede_vTipo"] = ddlTipoRemesaOtroMes.SelectedItem.Text;
                    dr["rede_sEstadoId"] = (int)Enumerador.enmRemesaEstado.PENDIENTE;
                    dr["rede_vEstado"] = "";
                    dr["rede_cPeriodo"] = ddlAnioOtro.SelectedValue + (ddlMesOtro.SelectedIndex + 1).ToString().PadLeft(2, '0');
                    dr["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaOtroMes.Text);
                    dr["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_otro_total.Text));
                    if (txt_rede_otro_FTipoCambioBancario.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], "0");                        
                    else
                        dr["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_otro_FTipoCambioBancario.Text));
                    if (txt_rede_otro_FTipoCambioConsular.Text.Trim() == string.Empty)
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], "0");
                    else
                        dr["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_otro_FTipoCambioConsular.Text));

                    dr["rede_FTotalDolares"] = CalcularCompraDeDivisas(dr["rede_FMonto"].ToString(), dr["rede_FTipoCambioBancario"].ToString());

                    dr["rede_sBancoId"] = ddl_rede_otro_banco.SelectedValue;
                    dr["rede_sCuentaCorrienteId"] = ddl_rede_otro_cuenta.SelectedValue;
                    dr["rede_sMonedaId"] = ddl_rede_otro_moneda.SelectedValue;
                    dr["rede_vResponsableEnvio"] = txtTitularDatos.Text.Trim().ToUpper();
                    if (ddlTarifaDescripcion.SelectedIndex > 0)
                    {
                        dr["rede_sTarifarioId"] = ddlTarifaDescripcion.SelectedValue;
                        dr["rede_vTarifario"] = ddlTarifaDescripcion.SelectedItem.Text;
                    }
                    dr["rede_vNroVoucher"] = txt_rede_otro_voucher.Text;
                    dr["rede_bMesFlag"] = intRemesaOtroMes;
                }
                dt.Rows.Add(dr);
                #endregion
            }
            else
            {   // MODIFICAR
                #region Remesa Detalle Modificar
                dt.Rows[intRemesaDetalleSel]["rede_sTipoId"] = ddlTipoRemesaOtroMes.SelectedValue;
                dt.Rows[intRemesaDetalleSel]["rede_vTipo"] = ddlTipoRemesaOtroMes.SelectedItem.Text;
                dt.Rows[intRemesaDetalleSel]["rede_cPeriodo"] = ddlAnioOtro.SelectedValue + (ddlMesOtro.SelectedIndex + 1).ToString().PadLeft(2, '0');
                dt.Rows[intRemesaDetalleSel]["rede_dFechaEnvio"] = Comun.FormatearFecha(datFechaOtroMes.Text);
                dt.Rows[intRemesaDetalleSel]["rede_FMonto"] = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(txt_rede_otro_total.Text));

                if (txt_rede_otro_FTipoCambioBancario.Text.Trim() == string.Empty)
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], "0");                    
                else
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioBancario"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCB"], Convert.ToDouble(txt_rede_otro_FTipoCambioBancario.Text));

                if (txt_rede_otro_FTipoCambioConsular.Text.Trim() == string.Empty)
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], "0");
                else
                    dt.Rows[intRemesaDetalleSel]["rede_FTipoCambioConsular"] = string.Format(ConfigurationManager.AppSettings["sessionFormatoDecimalTCC"], Convert.ToDouble(txt_rede_otro_FTipoCambioConsular.Text));                    

                dt.Rows[intRemesaDetalleSel]["rede_sBancoId"] = ddl_rede_otro_banco.SelectedValue;
                dt.Rows[intRemesaDetalleSel]["rede_sCuentaCorrienteId"] = ddl_rede_otro_cuenta.SelectedValue;
                dt.Rows[intRemesaDetalleSel]["rede_sMonedaId"] = ddl_rede_otro_moneda.SelectedValue;

                dt.Rows[intRemesaDetalleSel]["rede_vResponsableEnvio"] = txtTitularDatos.Text.Trim().ToUpper();
                if (ddlTarifaDescripcion.SelectedIndex > 0)
                {
                    dt.Rows[intRemesaDetalleSel]["rede_sTarifarioId"] = ddlTarifaDescripcion.SelectedValue;
                    dt.Rows[intRemesaDetalleSel]["rede_vTarifario"] = ddlTarifaDescripcion.SelectedItem.Text;
                }
                else
                {
                    dt.Rows[intRemesaDetalleSel]["rede_sTarifarioId"] = 0;
                    dt.Rows[intRemesaDetalleSel]["rede_vTarifario"] = string.Empty;
                }
                dt.Rows[intRemesaDetalleSel]["rede_vNroVoucher"] = txt_rede_otro_voucher.Text;
                dt.AcceptChanges();
                #endregion
            }

            gdvRemesaDetalle.DataSource = dt;
            gdvRemesaDetalle.DataBind();
            Session[strVariableDetalleDt] = dt;
            LimpiarDatosRemesaOtroMes();

            // Actualizar Total
            double dblTotalRemesaDetalle = 0;
            dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FMonto");
            txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);
            dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FTotalDolares");
            txtRemesaTotalDolares.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);
            updMantenimiento.Update();
            //--- 
        }
        protected void btnCancelarDetalle_Click(object sender, EventArgs e)
        {
            LimpiarDatosRemesaDetalle();
        }
        protected void btnCancelarOtroMes_Click(object sender, EventArgs e)
        {
            LimpiarDatosRemesaOtroMes();
        }

        protected void ddl_rede_actual_banco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_rede_actual_banco.SelectedIndex > 0)
                CargarCuentasPorBanco(ddl_rede_actual_banco, ddl_rede_actual_cuenta);
            else
                Util.CargarParametroDropDownList(ddl_rede_actual_cuenta, new DataTable(), true);
        }
        protected void ddl_rede_otro_banco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_rede_otro_banco.SelectedIndex > 0)
                CargarCuentasPorBanco(ddl_rede_otro_banco, ddl_rede_otro_cuenta);
            else
                Util.CargarParametroDropDownList(ddl_rede_otro_cuenta, new DataTable(), true);
        }
        protected void ddlAnioOtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPeriodoOtroMes();
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            decimal dblTipoCambio = 0;
            decimal dblTotalMonedaLocal = 0;

            if (txt_rede_otro_FTipoCambioBancario.Text.Trim() != string.Empty)
                dblTipoCambio = Convert.ToDecimal(txt_rede_otro_FTipoCambioBancario.Text);
            if (txt_rede_otro_total.Text.Trim() != string.Empty)
                dblTotalMonedaLocal = Convert.ToDecimal(txt_rede_otro_total.Text);

            if (dblTipoCambio == 0)
            {
                txt_rede_otro_dolares.Text = "";
            }
            else
            {
                if (dblTipoCambio>1)
                { txt_rede_otro_dolares.Text = Convert.ToDecimal((dblTotalMonedaLocal * dblTipoCambio)).ToString(); }
                else { txt_rede_otro_dolares.Text = Convert.ToDecimal((dblTotalMonedaLocal / dblTipoCambio)).ToString(); }
            }
        }

        protected void btnCalcActual_Click(object sender, EventArgs e)
        {
            decimal dblTipoCambio = 0;
            decimal dblTotalMonedaLocal = 0;
            string vContinente = Session[Constantes.CONST_SESION_CONTINENTE].ToString();

            if (txt_rede_FTipoCambioBancario.Text.Trim() != string.Empty)
                dblTipoCambio = Convert.ToDecimal(txt_rede_FTipoCambioBancario.Text);
            if (txt_rede_actual_total.Text.Trim() != string.Empty)
                dblTotalMonedaLocal = Convert.ToDecimal(txt_rede_actual_total.Text);

            if (dblTipoCambio == 0)
            {
                txt_rede_actual_total_dolares.Text = "";
            }
            else
            {
                if (vContinente == "EUROPA")
                {
                    if (dblTipoCambio > 1)
                    {
                        txt_rede_actual_total_dolares.Text = Convert.ToDecimal(dblTotalMonedaLocal * dblTipoCambio).ToString();
                    }
                    else
                    {
                        txt_rede_actual_total_dolares.Text = Convert.ToDecimal(dblTotalMonedaLocal / dblTipoCambio).ToString();
                    }
                }
                else {
                    txt_rede_actual_total_dolares.Text = Convert.ToDecimal(dblTotalMonedaLocal / dblTipoCambio).ToString();
                }
            }
        }
        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Session[strVariableDetalleIndice] = -1;

            LimpiarDatosConsulta();
            LimpiarDatosMantenimiento();

            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            // Consulta

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaOrigenCons.Cargar(true, true, "- TODOS -");
            }
            else
            {
                ddlOficinaOrigenCons.Cargar();
            }
           
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                ddlOficinaDestinoCons.Cargar(true, true, "- SELECCIONAR -");
            else
                ddlOficinaDestinoCons.CargarPorOficinasConsulares(
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] + "," +
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());

            Util.CargarParametroDropDownList(ddlRemesaEstadoConsulta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.REMESA), true, " - TODOS - ");
            Util.CargarParametroDropDownList(ddlRemesaTipoConsulta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_TRANSACCION), true, " - TODOS - ");

            // Mantenimiento
            ddlOficinaOrigenMant.Cargar(false, false);
            
            
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                ddlOficinaDestinoMant.Cargar(true);
            else
                ddlOficinaDestinoMant.CargarPorOficinasConsulares(
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] + "," +
                    Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString());

            DropDownList ddlOficinaDestinoMan = (DropDownList)ddlOficinaDestinoMant.FindControl("ddlOficinaConsular");
            ddlOficinaDestinoMan.Items.Remove(ddlOficinaDestinoMan.Items.FindByValue(ddlOficinaOrigenMant.SelectedValue));
            
            Util.CargarComboAnios(ddlRemesaAnio, DateTime.Now.Year - 1, DateTime.Now.Year);
            DataTable dtMes = new DataTable();

            dtMes = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES);


            Util.CargarDropDownList(ddlRemesaMes, dtMes, "valor", "id", true);

            // Remesa Periodo Actual
            Util.CargarComboAnios(ddlAnioActual, DateTime.Now.Year - 1, DateTime.Now.Year);

            Util.CargarDropDownList(ddlMesActual, dtMes, "valor", "id", true);



            Util.CargarParametroDropDownList(ddl_rede_actual_tipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_TRANSACCION), true);
            Util.CargarParametroDropDownList(ddlRemesaEstadoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.REMESA), true);

            //----------------------------------------------------------------------
            // Fecha: 20/11/2019
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Optimizar la carga de monedas en las listas desplegables 
            //----------------------------------------------------------------------
            DataTable dtMonedas = new DataTable();
            dtMonedas = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            
            Util.CargarParametroDropDownList(ddl_rede_actual_Moneda, dtMonedas, true);
            Util.CargarParametroDropDownList(ddl_rede_actual_moneda_local, dtMonedas, true);
            Util.CargarParametroDropDownList(ddl_rede_otro_moneda, dtMonedas, true);
            Util.CargarParametroDropDownList(ddl_rede_otro_moneda_local, dtMonedas, true);
            //----------------------------------------------------------------------

            CargarBancos(ddl_rede_actual_banco, ddl_rede_actual_cuenta, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            Util.CargarParametroDropDownList(ddlClasificacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Constantes.CONST_CLASIFICACION_REMESA), true);

            // Remesa Detalle Otro Periodo
            Util.CargarComboAnios(ddlAnioOtro, DateTime.Now.Year - 1, DateTime.Now.Year);
            Util.CargarDropDownList(ddlMesOtro, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_MES), "valor", "id");
            Util.CargarParametroDropDownList(ddlTipoRemesaOtroMes, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONTA_TIPO_TRANSACCION), true);
                        

            CargarBancos(ddl_rede_otro_banco, ddl_rede_otro_cuenta, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            //------------------------------------------------
            //Fecha: 24/10/2016
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Mostrar todas las tarifas consulares
            //------------------------------------------------
            DataTable dtTarifa = new DataTable();

            object[] arrParametros = { 0, "", 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 500, 0, 0 };

            dtTarifa = comun_Part2.ObtenerTarifario(Session, ref arrParametros);
            //---------------------------------------------
                        
            
            Util.CargarDropDownList(ddlTarifaDescripcion, dtTarifa.Copy(), "tari_vDescripcionCorta", "tari_sTarifarioId", true);
        }

        private void CargarGrilla()
        {
            ctrlToolBarConsulta.btnImprimir.Enabled = false;
            DataTable dtRemesa = new DataTable();

            int intTotalRegistros = 0, intTotalPaginas = 0;
            int intOficinaConsularOrigen = 0;
            int intOficinaConsularDestino = 0;
            int intTipo = 0;
            int intEstado = 0;            
            // Deberia poner el día de hoy o MinValue ---------------------
            DateTime? datFechaInicio = DateTime.Today;
            DateTime? datFechaFin = DateTime.Today;

            //---------------------------------------------------------------------------------------
            //Fecha: 07/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Lima en la consulta tiene la opción todos o seleccionar un consulado.
            //          El consulado selecciona su consulado o dependientes por defecto.
            //---------------------------------------------------------------------------------------

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlOficinaOrigenCons.SelectedIndex > 0)
                {
                    intOficinaConsularOrigen = Convert.ToInt32(ddlOficinaOrigenCons.SelectedValue);
                }
            }
            else
            {
                intOficinaConsularOrigen = Convert.ToInt32(ddlOficinaOrigenCons.SelectedValue);
            }
            //---------------------------------------------------------------------------------------

            if (ddlOficinaDestinoCons.SelectedValue != string.Empty)
                intOficinaConsularDestino = Convert.ToInt32(ddlOficinaDestinoCons.SelectedValue);

            if (ddlRemesaTipoConsulta.SelectedValue != null)
                intTipo = Convert.ToInt32(ddlRemesaTipoConsulta.SelectedValue);

            if (ddlRemesaEstadoConsulta.SelectedValue != null)
                intEstado = Convert.ToInt32(ddlRemesaEstadoConsulta.SelectedValue);

            if (dtpFecInicio.Text != string.Empty)
            {
                datFechaInicio = Comun.FormatearFecha(dtpFecInicio.Text);
            }
            else {
                datFechaInicio = null;
            }

            if (dtpFecFin.Text != string.Empty)
            {
                datFechaFin = Comun.FormatearFecha(dtpFecFin.Text);
            }
            else {
                datFechaFin = null;
            }

            try
            {
                RemesaConsultasBL objBL = new RemesaConsultasBL();
                dtRemesa = objBL.Consultar(ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO,
                                            ref intTotalRegistros, ref intTotalPaginas,
                                            intOficinaConsularOrigen, intOficinaConsularDestino,
                                            intEstado, datFechaInicio, datFechaFin);

                Session[strVariableDt] = dtRemesa;
                gdvRemesa.SelectedIndex = -1;
                gdvRemesa.DataSource = dtRemesa;
                gdvRemesa.DataBind();

                if (dtRemesa.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                    ctrlToolBarConsulta.btnImprimir.Enabled = true;
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
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "",
                    audi_sOperacionTipoId = (int) Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sOperacionResultadoId = (int) Enumerador.enmResultadoAuditoria.ERR,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = ex.Message,
                    audi_vMensaje = ex.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion

                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REMESA CONSULTA", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                Comun.EjecutarScript(Page, strScript);
            }
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
                DataRow drRemesaSel = ObtenerFilaSeleccionada();
                if (drRemesaSel != null)
                {
                    ddlOficinaOrigenMant.SelectedValue = drRemesaSel["reme_sOficinaConsularOrigenId"].ToString();

                    ddlRemesaAnio.SelectedValue = drRemesaSel["reme_cPeriodo"].ToString().Substring(0, 4);
                    ddlRemesaMes.SelectedIndex = Convert.ToInt32(drRemesaSel["reme_cPeriodo"].ToString().Substring(5, 2));
                    if (drRemesaSel["reme_dFechaEnvio"] != null)
                    {
                        if (drRemesaSel["reme_dFechaEnvio"].ToString() != string.Empty)
                        {
                            datFechaEnvio.Text = Comun.FormatearFecha(drRemesaSel["reme_dFechaEnvio"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                            ddlOficinaDestinoMant.Cargar(true);
                        }
                    }
                    ddlOficinaDestinoMant.SelectedValue = drRemesaSel["reme_sOficinaConsularDestinoId"].ToString();

                    ddlRemesaEstadoMant.SelectedValue = drRemesaSel["reme_sEstadoId"].ToString();

                    #region Validación Grilla Columnas
                    if (ddlRemesaEstadoMant.SelectedIndex > 0)
                    {
                        int intOCOrigen = Convert.ToInt32(ddlOficinaOrigenMant.SelectedValue);
                        int intOCLogeo = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        if (intOCOrigen != intOCLogeo)
                        {
                            gdvRemesaDetalle.Columns[intColRede_Editar].Visible = false;
                            gdvRemesaDetalle.Columns[intColRede_Anular].Visible = false;
                        }
                        else
                        {
                            if (Convert.ToInt32(ddlRemesaEstadoMant.SelectedValue) == (int)Enumerador.enmRemesaEstado.ENVIADA ||
                                Convert.ToInt32(ddlRemesaEstadoMant.SelectedValue) == (int)Enumerador.enmRemesaEstado.APROBADA ||
                                Convert.ToInt32(ddlRemesaEstadoMant.SelectedValue) == (int)Enumerador.enmRemesaEstado.ANULADA)
                            {
                                gdvRemesaDetalle.Columns[intColRede_Editar].Visible = false;
                                gdvRemesaDetalle.Columns[intColRede_Anular].Visible = false;
                            }
                            else
                            {
                                gdvRemesaDetalle.Columns[intColRede_Editar].Visible = true;
                                gdvRemesaDetalle.Columns[intColRede_Anular].Visible = true;
                            }
                        }
                    }
                    #endregion

                    if (drRemesaSel["reme_FMonto"].ToString() == string.Empty)
                        txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], 0);
                    else
                        txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(drRemesaSel["reme_FMonto"]));

                    txtObservacion.Text = drRemesaSel["reme_vObservacion"].ToString();
                    updMantenimiento.Update();
                }
            }
        }

        private void CargarDetalle()
        {
            DataTable dt = new DataTable();

            int intRemesaId = Convert.ToInt32(ObtenerFilaSeleccionada()["reme_iRemesaId"]);
            int intTotalRegistros = 0, intTotalPaginas = 0;            

            try
            {                
                RemesaDetalleConsultasBL objBL = new RemesaDetalleConsultasBL();
                dt = objBL.ObtenerPorRemesa(ctrlPaginadorDetalle.PaginaActual,
                                             Constantes.CONST_CANT_REGISTRO,
                                             ref intTotalRegistros, ref intTotalPaginas,
                                             intRemesaId);

                LimpiarDatosDetalle();

                gdvRemesaDetalle.SelectedIndex = -1;
                gdvRemesaDetalle.DataSource = dt;
                gdvRemesaDetalle.DataBind();

                Session[strVariableDetalleDt] = dt;

                // Actualizar Total Dólares - A1
                double strTotalDolares = CalcularTotalRemesa("rede_FTotalDolares");
                txtRemesaTotalDolares.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], strTotalDolares);
            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "REMESA CONSULAR",
                    audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = ex.Message,
                    audi_vMensaje = ex.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion

                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REMESA CONSULAR", Constantes.CONST_MENSAJE_OPERACION_FALLIDA + " (Error: " +ex.Message + ")");
                Comun.EjecutarScript(Page, strScript);
            }
            finally
            {       
                ctrlPaginadorDetalle.TotalResgistros = intTotalRegistros;
                ctrlPaginadorDetalle.TotalPaginas = intTotalPaginas;

                ctrlPaginadorDetalle.Visible = false;
                if (ctrlPaginadorDetalle.TotalPaginas > 1)
                {
                    ctrlPaginadorDetalle.Visible = true;
                }
            }                                  
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlRemesaAnio.Enabled = bolHabilitar;
            ddlRemesaMes.Enabled = bolHabilitar;

            ddl_rede_actual_tipo.Enabled = bolHabilitar;
            ddlClasificacion.Enabled = bolHabilitar;

            //btnAgregarDetalle.Visible = bolHabilitar;
            //btnRemesaOtroMes.Visible = bolHabilitar;
        }

        private void LimpiarDatosConsulta()
        {
            Session[strVariableDt] = null;
            Session[strVariableIndice] = -1;

            ctrlToolBarConsulta.btnImprimir.Enabled = false;
            
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaOrigenCons.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
                ddlOficinaDestinoCons.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString();
            }
            else
            {   // Lima
                ddlOficinaOrigenCons.SelectedIndex = 0;
                ddlOficinaDestinoCons.SelectedValue = Constantes.CONST_OFICINACONSULAR_LIMA.ToString();
            }

            dtpFecInicio.Text = DateTime.Today.ToString("MMM-01-yyyy");
            dtpFecFin.Text = Comun.ObtenerFechaActualTexto(Session);

            gdvRemesa.DataSource = null;
            gdvRemesa.DataBind();
            ctrlPaginador.InicializarPaginador();
        }

        private void LimpiarDatosMantenimiento()
        {
            gdvRemesaDetalle.DataSource = new DataTable();
            gdvRemesaDetalle.DataBind();

            ddlOficinaOrigenMant.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                ddlOficinaDestinoMant.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID].ToString();
            else
                ddlOficinaDestinoMant.SelectedIndex = 0;

            ddlOficinaDestinoMant.Enabled = true;

            ddlRemesaAnio.SelectedValue = DateTime.Today.Year.ToString();
            ddlAnioActual.SelectedValue = DateTime.Today.Year.ToString();
            ddlRemesaMes.SelectedIndex = -1;           
            datFechaEnvio.Enabled = false;
            datFechaEnvio.Text = string.Empty;
            txtRemesaTotal.Text = string.Empty;
            txtRemesaTotalDolares.Text = string.Empty;
            txtObservacion.Text = string.Empty;

            ddlRemesaEstadoMant.Items.Clear();
            Util.CargarParametroDropDownList(ddlRemesaEstadoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.REMESA), true);
            ddlRemesaEstadoMant.SelectedValue = ((int)Enumerador.enmRemesaEstado.PENDIENTE).ToString();
            ddlRemesaEstadoMant.Enabled = false;            

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            LimpiarDatosDetalle();
        }

        private void LimpiarDatosDetalle()
        {
            Session[strVariableDetalleDt] = null;

            tDetalle.Visible = false;
            btnAgregarDetalle.BackColor = ColorTranslator.FromHtml("#ededed");
            btnAgregarDetalle.Font.Bold = false;
            tRemesaOtroMes.Visible = false;
            btnRemesaOtroMes.BackColor = ColorTranslator.FromHtml("#ededed");
            btnRemesaOtroMes.Font.Bold = false;            

            LimpiarDatosRemesaDetalle();
            LimpiarDatosRemesaOtroMes();            
        }

        private void LimpiarDatosRemesaDetalle()
        {
            Session[strVariableDetalleIndice] = -1;
            ddlMesActual.SelectedIndex = - 1;

            datFechaDetalle.Text = Comun.ObtenerFechaActualTexto(Session);
            txt_rede_FTipoCambioConsular.Text = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
            txt_rede_FTipoCambioBancario.Text = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
            
            ddl_rede_actual_Moneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();
            ddl_rede_actual_moneda_local.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();

            ddl_rede_actual_tipo.SelectedIndex = -1;
            ddlClasificacion.SelectedIndex = -1;
            ddl_rede_actual_banco.SelectedIndex = -1;
            ddl_rede_actual_cuenta.SelectedIndex = -1;
            txt_rede_actual_total.Text = string.Empty;
            txt_rede_actual_total_dolares.Text = string.Empty;
            txt_rede_actual_voucher.Text = string.Empty;
            txtResponsableEnvio.Text = string.Empty;
            txtObsDetalle.Text = string.Empty;
        }

        private void LimpiarDatosRemesaOtroMes()
        {
            Session[strVariableDetalleIndice] = -1;

            datFechaOtroMes.Text = Comun.ObtenerFechaActualTexto(Session);
            ddlAnioOtro.SelectedValue = DateTime.Today.Year.ToString();
            CargarPeriodoOtroMes(); // MDIAZ - 15/03/2015
            ddlMesOtro.SelectedIndex = DateTime.Today.Month - 2;
            txt_rede_otro_FTipoCambioConsular.Text = Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString();
            txt_rede_otro_FTipoCambioBancario.Text = Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO].ToString();
            ddlTipoRemesaOtroMes.SelectedIndex = -1;

            txt_rede_otro_total.Text = string.Empty;
            txt_rede_otro_dolares.Text = string.Empty;

            ddl_rede_otro_moneda_local.SelectedValue = Session[Constantes.CONST_SESION_TIPO_MONEDA_ID].ToString();
            ddl_rede_otro_moneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();

            ddl_rede_otro_banco.SelectedIndex = 0;
            ddl_rede_otro_cuenta.SelectedIndex = 0;
            txt_rede_otro_voucher.Text = string.Empty;
            txtTitularDatos.Text = string.Empty;
            ddlTarifaDescripcion.SelectedIndex = 0;
        }

        private SGAC.BE.MRE.CO_REMESA ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                DataTable dt = (DataTable)Session[strVariableDt];
                DataRow drSeleccionado = dt.Rows[intSeleccionado];

                SGAC.BE.MRE.CO_REMESA obj = new SGAC.BE.MRE.CO_REMESA();
                obj.reme_iRemesaId = Convert.ToInt32(drSeleccionado["reme_iRemesaId"]);
                obj.reme_sOficinaConsularOrigenId = Convert.ToInt16(drSeleccionado["reme_sOficinaConsularOrigenId"]);
                obj.reme_sOficinaConsularDestinoId = Convert.ToInt16(drSeleccionado["reme_sOficinaConsularDestinoId"]);
                obj.reme_dFechaEnvio = Comun.FormatearFecha(drSeleccionado["reme_dFechaEnvio"].ToString());
                obj.reme_sTipoId = Convert.ToInt16(drSeleccionado["reme_sTipoId"]);
                obj.reme_vNumeroEnvio = drSeleccionado["reme_vNumeroEnvio"].ToString();
                obj.reme_sEstadoId = Convert.ToInt16(drSeleccionado["reme_sEstadoId"]);
                obj.reme_sCuentaCorrienteId = Convert.ToInt16(drSeleccionado["reme_sCuentaCorrienteId"]);
                obj.reme_FMonto = Convert.ToDouble(drSeleccionado["reme_FMonto"]);
                obj.reme_vObservacion = drSeleccionado["reme_vObservacion"].ToString();
                obj.reme_sUsuarioCreacion = Convert.ToInt16(drSeleccionado["reme_sUsuarioCreacion"]);

                return obj;
            }
            return null;
        }

        private SGAC.BE.MRE.CO_REMESA ObtenerEntidadMantenimiento()
        {
            SGAC.BE.MRE.CO_REMESA obj = new SGAC.BE.MRE.CO_REMESA();
            if (Session != null)
            {
                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    obj.reme_iRemesaId = Convert.ToInt64(ObtenerFilaSeleccionada()["reme_iRemesaId"]);
                }
                obj.reme_sOficinaConsularOrigenId = Convert.ToInt16(ddlOficinaOrigenMant.SelectedValue);
                obj.reme_sOficinaConsularDestinoId = Convert.ToInt16(ddlOficinaDestinoMant.SelectedValue);
                obj.reme_cPeriodo = ddlRemesaAnio.SelectedValue + (Convert.ToInt32(ddlRemesaMes.SelectedIndex)).ToString().PadLeft(2, '0');
                
                double dblTotalRemesaDetalle = 0;
                dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FMonto");
                txtRemesaTotal.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);
                dblTotalRemesaDetalle = CalcularTotalRemesa("rede_FTotalDolares");
                txtRemesaTotalDolares.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], dblTotalRemesaDetalle);

                obj.reme_FMonto = Convert.ToDouble(txtRemesaTotal.Text);
                if (datFechaEnvio.Text.Trim() != string.Empty)
                {
                    obj.reme_dFechaEnvio = Comun.FormatearFecha(datFechaEnvio.Text);
                }
                obj.reme_vObservacion = Util.ReemplazarCaracter(txtObservacion.Text.Trim());

                obj.reme_sEstadoId = Convert.ToInt16(ddlRemesaEstadoMant.SelectedValue);

                obj.reme_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.reme_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                obj.reme_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.reme_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                obj.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                //obj.DiferenciaHoraria = Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sDiferenciaHoraria"));
                //obj.HorarioVerano = Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sHorarioVerano"));
            }

            return obj;
        }

        private List<SGAC.BE.MRE.CO_REMESADETALLE> ObtenerListadoEntidadDetalle()
        {
            DataTable dtRemesaDetalle = (DataTable)Session[strVariableDetalleDt];
            if (dtRemesaDetalle == null)
            {
                return null;
            }

            bool bolExisteDetalle = false;
            List<SGAC.BE.MRE.CO_REMESADETALLE> lstEntidadDetalle = new List<SGAC.BE.MRE.CO_REMESADETALLE>();
            foreach (DataRow dr in dtRemesaDetalle.Rows)
            {
                bolExisteDetalle = false;
                if (dr["rede_iRemesaDetalleId"].ToString() != string.Empty)
                {
                    if (Convert.ToInt32(dr["rede_iRemesaDetalleId"]) != 0)
                        bolExisteDetalle = true;
                }
                if (Convert.ToInt32(dr["rede_sEstadoId"]) == (int)Enumerador.enmRemesaEstado.ANULADA &&
                    !bolExisteDetalle)
                {
                    // Si es Anulado y no tiene ID NO CONSIDERAR
                }
                else
                {
                    SGAC.BE.MRE.CO_REMESADETALLE objEntidadDetalle = new SGAC.BE.MRE.CO_REMESADETALLE();
                    objEntidadDetalle.rede_iRemesaDetalleId = Convert.ToInt32(dr["rede_iRemesaDetalleId"]);
                    objEntidadDetalle.rede_iRemesaId = Convert.ToInt32(dr["rede_iRemesaId"]);
                    objEntidadDetalle.rede_sTipoId = Convert.ToInt16(dr["rede_sTipoId"]);

                    //if (dr["rede_sEstadoId"].ToString() != string.Empty)
                    //    objEntidadDetalle.rede_sEstadoId = Convert.ToInt16(dr["rede_sEstadoId"]);
                    //else
                    //    objEntidadDetalle.rede_sEstadoId = (int)Enumerador.enmRemesaEstado.PENDIENTE;

                    objEntidadDetalle.rede_cPeriodo = dr["rede_cPeriodo"].ToString();

                    if (dr["rede_dFechaEnvio"] != null)
                    {
                        if (dr["rede_dFechaEnvio"].ToString() != string.Empty)
                            objEntidadDetalle.rede_dFechaEnvio = Comun.FormatearFecha(dr["rede_dFechaEnvio"].ToString()); 
                    }

                    if (dr["rede_FTipoCambioBancario"] != null)
                    {
                        if (dr["rede_FTipoCambioBancario"].ToString() != string.Empty)
                        {
                            objEntidadDetalle.rede_FTipoCambioBancario = Convert.ToDouble(dr["rede_FTipoCambioBancario"]);
                        }
                    }

                    if (dr["rede_FTipoCambioConsular"] != null)
                    {
                        if (dr["rede_FTipoCambioConsular"].ToString() != string.Empty)
                        {
                            objEntidadDetalle.rede_FTipoCambioConsular = Convert.ToDouble(dr["rede_FTipoCambioConsular"]);
                        }
                    }
                    objEntidadDetalle.rede_sCuentaCorrienteId = Convert.ToInt16(dr["rede_sCuentaCorrienteId"]);
                    objEntidadDetalle.rede_sMonedaId = Convert.ToInt16(dr["rede_sMonedaId"]);
                    objEntidadDetalle.rede_FMonto = Convert.ToDouble(dr["rede_FMonto"]);

                    if (dr["rede_vNroVoucher"] != null)
                    {
                        if (dr["rede_vNroVoucher"].ToString().Trim() != string.Empty)
                        {
                            objEntidadDetalle.rede_vNroVoucher = dr["rede_vNroVoucher"].ToString().Trim();
                        }
                    }

                    objEntidadDetalle.rede_vResponsableEnvio = dr["rede_vResponsableEnvio"].ToString();
                    objEntidadDetalle.rede_vRecurrente = "";

                    if (dr["rede_sTarifarioId"] != null)
                    {
                        if (dr["rede_sTarifarioId"].ToString().Trim() != string.Empty)
                        {
                            objEntidadDetalle.rede_sTarifarioId = Convert.ToInt16(dr["rede_sTarifarioId"]);
                        }
                    }

                    objEntidadDetalle.rede_vObservacion = dr["rede_vObservacion"].ToString();
                    objEntidadDetalle.rede_bMesFlag = Convert.ToBoolean(Convert.ToInt32(dr["rede_bMesFlag"]));

                    objEntidadDetalle.rede_sEstadoId = Convert.ToInt16(ddlRemesaEstadoMant.SelectedValue);

                    objEntidadDetalle.rede_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objEntidadDetalle.rede_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    objEntidadDetalle.rede_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objEntidadDetalle.rede_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    objEntidadDetalle.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    //objEntidadDetalle.DiferenciaHoraria = Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sDiferenciaHoraria"));
                    //objEntidadDetalle.HorarioVerano = Convert.ToInt16(Comun.ObtenerDatoOficinaConsular(Session, "ofco_sHorarioVerano"));

                    objEntidadDetalle.ClasificacionID = Convert.ToInt16(dr["rede_sClasificacionId"]);

                    lstEntidadDetalle.Add(objEntidadDetalle);
                }
            }
            return lstEntidadDetalle;
        }

        private double CalcularTotalRemesa(string strNombreCampo)
        {
            double dTotalRemesa = 0.0;
       
                if ((DataTable)Session[strVariableDetalleDt] != null)
                {
                    DataTable dtRemesaDetalle = ((DataTable)Session[strVariableDetalleDt]).Copy();
                    foreach (GridViewRow row in gdvRemesaDetalle.Rows)
                    {
                        if (Convert.ToInt32(dtRemesaDetalle.Rows[row.RowIndex]["rede_sEstadoId"]) != ((int)Enumerador.enmRemesaEstado.ANULADA))
                        {
                            dTotalRemesa += Convert.ToDouble(dtRemesaDetalle.Rows[row.RowIndex][strNombreCampo]);
                        }
                    }
                }

            return dTotalRemesa;
        }

        private void PintarDetalleSelEnPantalla(int intSeleccionado)
        {
            DataTable dt = (DataTable)Session[strVariableDetalleDt];

            if (dt != null)
            {
                DataTable dtRemesaDetalle = dt.Copy();
                DataRow drDetalle = dtRemesaDetalle.Rows[intSeleccionado];

                string strPeriodo = drDetalle["rede_cPeriodo"].ToString();
                int intOtroMes = Convert.ToInt32(drDetalle["rede_bMesFlag"]);

                if (intOtroMes == intRemesaOtroMes)
                {   // REMESA DETALLE - OTRO MES
                    ddlAnioOtro.SelectedValue = strPeriodo.Substring(0, 4);
                    CargarPeriodoOtroMes(); // MDIAZ - 15/03/2015
                    ddlMesOtro.SelectedIndex = Convert.ToInt32(strPeriodo.Substring(5, 2)) - 1;
                    if (drDetalle["rede_sTarifarioId"].ToString().Trim() != string.Empty)
                        ddlTarifaDescripcion.SelectedValue = drDetalle["rede_sTarifarioId"].ToString();

                    txt_rede_otro_FTipoCambioBancario.Text = "0";
                    if (drDetalle["rede_FTipoCambioBancario"].ToString().Trim() != string.Empty)
                        txt_rede_otro_FTipoCambioBancario.Text = drDetalle["rede_FTipoCambioBancario"].ToString();

                    if (drDetalle["rede_FTipoCambioConsular"].ToString().Trim() != string.Empty)
                        txt_rede_otro_FTipoCambioConsular.Text = drDetalle["rede_FTipoCambioConsular"].ToString();

                    if (drDetalle["rede_dFechaEnvio"] != null)
                    {
                        if (drDetalle["rede_dFechaEnvio"].ToString().Trim() != string.Empty)
                            datFechaOtroMes.Text = string.Format(ConfigurationManager.AppSettings["FormatoFechaContable"], drDetalle["rede_dFechaEnvio"]); 
                    }

                    if (drDetalle["rede_sTipoId"].ToString().Trim() != string.Empty)
                        ddlTipoRemesaOtroMes.SelectedValue = drDetalle["rede_sTipoId"].ToString();

                    txt_rede_otro_total.Text = "0";
                    if (drDetalle["rede_FMonto"].ToString().Trim() != string.Empty)
                        txt_rede_otro_total.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(drDetalle["rede_FMonto"]));

                    // 2015-05-15 : Calcular total remesado en la moneda general
                    txt_rede_otro_dolares.Text = CalcularCompraDeDivisas(txt_rede_otro_total.Text, txt_rede_otro_FTipoCambioBancario.Text);

                    if (drDetalle["rede_vResponsableEnvio"].ToString().Trim() != string.Empty)
                        txtTitularDatos.Text = drDetalle["rede_vResponsableEnvio"].ToString();

                    if (drDetalle["rede_vNroVoucher"].ToString().Trim() != string.Empty)
                        txt_rede_otro_voucher.Text = drDetalle["rede_vNroVoucher"].ToString();

                    if (drDetalle["rede_sBancoId"].ToString() != string.Empty)
                        ddl_rede_otro_banco.SelectedValue = drDetalle["rede_sBancoId"].ToString();

                    if (ddl_rede_otro_banco.SelectedIndex > 0)
                    {
                        CargarCuentasPorBanco(ddl_rede_otro_banco, ddl_rede_otro_cuenta);

                        if (drDetalle["rede_sCuentaCorrienteId"].ToString() != string.Empty)
                            ddl_rede_otro_cuenta.SelectedValue = drDetalle["rede_sCuentaCorrienteId"].ToString();
                    }

                    if (drDetalle["rede_vNroVoucher"].ToString() != string.Empty)
                        txt_rede_otro_voucher.Text = drDetalle["rede_vNroVoucher"].ToString();

                    btnRemesaOtroMes.Font.Bold = true;
                    btnRemesaOtroMes.BackColor = ColorTranslator.FromHtml("#dfdfdf");
                    btnAgregarDetalle.Font.Bold = false;
                    btnAgregarDetalle.BackColor = ColorTranslator.FromHtml("#ededed");
                    tDetalle.Visible = false;
                    tRemesaOtroMes.Visible = true;
                }
                else
                {   // REMESA DETALLE
                    ddlAnioActual.SelectedValue = strPeriodo.Substring(0, 4);
                    ddlMesActual.SelectedIndex = Convert.ToInt32(strPeriodo.Substring(5, 2));

                    txt_rede_FTipoCambioBancario.Text = "0";
                    if (drDetalle["rede_FTipoCambioBancario"].ToString().Trim() != string.Empty)
                        txt_rede_FTipoCambioBancario.Text = drDetalle["rede_FTipoCambioBancario"].ToString();

                    txt_rede_FTipoCambioConsular.Text = "0";
                    if (drDetalle["rede_FTipoCambioConsular"].ToString().Trim() != string.Empty)
                        txt_rede_FTipoCambioConsular.Text = drDetalle["rede_FTipoCambioConsular"].ToString();

                    if (drDetalle["rede_vObservacion"].ToString().Trim() != string.Empty)
                        txtObsDetalle.Text = drDetalle["rede_vObservacion"].ToString();

                    if (drDetalle["rede_dFechaEnvio"] != null)
                    {
                        if (drDetalle["rede_dFechaEnvio"].ToString().Trim() != string.Empty)
                            datFechaDetalle.Text = string.Format(ConfigurationManager.AppSettings["FormatoFechaContable"], Comun.FormatearFecha(drDetalle["rede_dFechaEnvio"].ToString()));
                    }

                    if (drDetalle["rede_sTipoId"].ToString().Trim() != string.Empty)
                        ddl_rede_actual_tipo.SelectedValue = drDetalle["rede_sTipoId"].ToString();

                    txt_rede_actual_total.Text = "0";
                    if (drDetalle["rede_FMonto"].ToString().Trim() != string.Empty)
                        txt_rede_actual_total.Text = string.Format(ConfigurationManager.AppSettings["FormatoDecimalContable"], Convert.ToDouble(drDetalle["rede_FMonto"]));

                    // 2015-05-15 : Calcular total remesado en la moneda general
                    txt_rede_actual_total_dolares.Text = CalcularCompraDeDivisas(txt_rede_actual_total.Text, txt_rede_FTipoCambioBancario.Text);

                    if (drDetalle["rede_vResponsableEnvio"].ToString().Trim() != string.Empty)
                        txtResponsableEnvio.Text = drDetalle["rede_vResponsableEnvio"].ToString();

                    if (drDetalle["rede_sBancoId"].ToString() != string.Empty)
                        ddl_rede_actual_banco.SelectedValue = drDetalle["rede_sBancoId"].ToString();

                    if (ddl_rede_actual_banco.SelectedIndex > 0)
                    {
                        CargarCuentasPorBanco(ddl_rede_actual_banco, ddl_rede_actual_cuenta);
                        if (drDetalle["rede_sCuentaCorrienteId"].ToString() != string.Empty)
                            ddl_rede_actual_cuenta.SelectedValue = drDetalle["rede_sCuentaCorrienteId"].ToString();
                    }
                                        
                    if (drDetalle["rede_vNroVoucher"].ToString() != string.Empty)
                        txt_rede_actual_voucher.Text = drDetalle["rede_vNroVoucher"].ToString();
                    if (drDetalle["rede_sMonedaId"].ToString() != string.Empty)
                        ddl_rede_actual_Moneda.Text = drDetalle["rede_sMonedaId"].ToString();

                    if (drDetalle["rede_sClasificacionId"].ToString().Trim() != string.Empty)
                        ddlClasificacion.SelectedValue = drDetalle["rede_sClasificacionId"].ToString();

                    btnAgregarDetalle.Font.Bold = true;
                    btnAgregarDetalle.BackColor = ColorTranslator.FromHtml("#dfdfdf");
                    btnRemesaOtroMes.Font.Bold = false;
                    btnRemesaOtroMes.BackColor = ColorTranslator.FromHtml("#ededed");
                    tRemesaOtroMes.Visible = false;
                    tDetalle.Visible = true;
                }
            }
            else
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REMESA CONSULAR", "No hay remesa detalle para seleccionar.");
                Comun.EjecutarScript(Page, strScript);
            }

            updRemesaDetalle.Update();
        }

        private void PintarGrillaDetalleFilas()
        {
            DataTable dtRemesaDetalle = ((DataTable)Session[strVariableDetalleDt]).Copy();
            foreach (GridViewRow row in gdvRemesaDetalle.Rows)
            {
                if (Convert.ToInt32(dtRemesaDetalle.Rows[row.RowIndex]["rede_sEstadoId"]) == ((int)Enumerador.enmRemesaEstado.ANULADA))
                {
                    row.BackColor = System.Drawing.Color.FromName("#FAD4D9");
                }
                else
                {
                    row.BackColor = System.Drawing.Color.White;
                }
            }
        }        

        /// <summary>
        /// Activa Boton Remesa Detalle - Limpiar Datos Remesa Detalle
        /// </summary>
        private void InicializarControlesRemesaDetalle()
        {
            ActivarBotonRemesaDetalle(tDetalle, btnAgregarDetalle, tRemesaOtroMes, btnRemesaOtroMes);
            LimpiarDatosRemesaDetalle();
        }

        private void CargarPeriodoOtroMes()
        {
            int intMesActual = DateTime.Now.Month - 1;
            if (Convert.ToInt32(ddlAnioOtro.SelectedValue) == DateTime.Now.Year)
            {
                for (int i = intMesActual; i < ddlMesOtro.Items.Count; i++)
                {
                    ddlMesOtro.Items[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < ddlMesOtro.Items.Count; i++)
                {
                    ddlMesOtro.Items[i].Enabled = true;
                }
            }
        }

        private void CargarBancos(DropDownList ddlBanco, DropDownList ddlCuenta, int intOficinaConsularId)
        {
            TransaccionConsultasBL objBL = new TransaccionConsultasBL();
            DataTable dt = objBL.ObtenerBancoCuenta(intOficinaConsularId);

            Util.CargarDropDownList(ddlBanco, dt, "descripcion", "id", true);            

            ddlCuenta.Items.Clear();
            ddlCuenta.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
        }
        private void CargarCuentasPorBanco(DropDownList ddlBanco, DropDownList ddlCuenta)
        {
            CuentaConsultasBL objBL = new CuentaConsultasBL();
            int intBancoId = Convert.ToInt32(ddlBanco.SelectedValue);
            int intOficinaConsularId = Convert.ToInt32(ddlOficinaOrigenMant.SelectedValue);
            int intTotalRegistros = 0, intTotalPaginas = 0;

            DataTable dt = objBL.Consultar(intOficinaConsularId, intBancoId, 1, 1000, ref intTotalRegistros, ref intTotalPaginas);
            Util.CargarDropDownList(ddlCuenta, dt, "cuco_vNumero", "cuco_sCuentaCorrienteId", true);
        }

        public DataTable CrearTablaRemesaDetalle()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("rede_iRemesaDetalleId", typeof(string));
            dt.Columns.Add("rede_iRemesaId", typeof(string));
            dt.Columns.Add("rede_sTipoId", typeof(string));
            dt.Columns.Add("rede_vTipo", typeof(string));
            dt.Columns.Add("rede_sEstadoId", typeof(string));
            dt.Columns.Add("rede_vEstado", typeof(string));
            dt.Columns.Add("rede_cPeriodo", typeof(string));
            dt.Columns.Add("rede_dFechaEnvio", typeof(DateTime));
            dt.Columns.Add("rede_FTipoCambioBancario", typeof(string));
            dt.Columns.Add("rede_FTipoCambioConsular", typeof(string));
            dt.Columns.Add("rede_FMonto", typeof(string));
            dt.Columns.Add("rede_FTotalDolares", typeof(string));
            dt.Columns.Add("rede_sBancoId", typeof(string));
            dt.Columns.Add("rede_sCuentaCorrienteId", typeof(string));
            dt.Columns.Add("rede_sMonedaId", typeof(string));
            dt.Columns.Add("rede_vNroVoucher", typeof(string));
            dt.Columns.Add("rede_vResponsableEnvio", typeof(string));
            dt.Columns.Add("rede_vRecurrente", typeof(string));
            dt.Columns.Add("rede_sTarifarioId", typeof(string));
            dt.Columns.Add("rede_vTarifario", typeof(string));
            dt.Columns.Add("rede_vObservacion", typeof(string));
            dt.Columns.Add("rede_bMesFlag", typeof(string));
            dt.Columns.Add("rede_sClasificacionId", typeof(string));
            dt.Columns.Add("rede_vClasificacion", typeof(string));
            return dt;
        }        

        private void ActivarBotonRemesaDetalle(
            System.Web.UI.HtmlControls.HtmlTable tblActivo, Button btnActivo,
            System.Web.UI.HtmlControls.HtmlTable tblInactivo, Button btnInactivo)
        {
            if (tblActivo.Visible)
            {
                btnActivo.Font.Bold = false;
                btnActivo.BackColor = ColorTranslator.FromHtml("#ededed");
                tblActivo.Visible = false;
            }
            else
            {
                btnActivo.Font.Bold = true;
                btnActivo.BackColor = ColorTranslator.FromHtml("#dfdfdf");
                btnInactivo.Font.Bold = false;
                btnInactivo.BackColor = ColorTranslator.FromHtml("#ededed");

                tblInactivo.Visible = false;
                tblActivo.Visible = true;
            }
        }

        private string CalcularCompraDeDivisas(string strTotalLocal, string strTipoCambioBancario)
        {
            string strTotalDolares = "0";
            double dblTotalLocal = 0;
            double dblTipoCambioBancario = 0;
            string vContinente = Session[Constantes.CONST_SESION_CONTINENTE].ToString();

            if (strTotalLocal != string.Empty)
                dblTotalLocal = Convert.ToDouble(strTotalLocal);
            if (strTipoCambioBancario != string.Empty)
                dblTipoCambioBancario = Convert.ToDouble(strTipoCambioBancario);

            if (dblTipoCambioBancario != 0)
            {
                if (vContinente == "EUROPA")
                {
                    if (dblTipoCambioBancario > 1)
                    {
                        strTotalDolares = (dblTotalLocal * dblTipoCambioBancario).ToString();
                    }
                    else
                    {
                        strTotalDolares = (dblTotalLocal / dblTipoCambioBancario).ToString();
                    }
                }
                else {
                    strTotalDolares = (dblTotalLocal / dblTipoCambioBancario).ToString();
                }
                //strTotalDolares = (dblTotalLocal / dblTipoCambioBancario).ToString();
            }
            
            return strTotalDolares;
        }
        #endregion


        //----------------------------------------------------------------------
        // Fecha: 27/01/2017
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Consultar las alertas pendientes de envio y/o enviadas
        //----------------------------------------------------------------------

        private void ConsultarAlerta()
        {
            bool bAlertaPendienteFlag = false;
            bool bAlertaEnviadosFlag = false;

            RemesaConsultasBL objBL = new RemesaConsultasBL();

            int iOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            objBL.ConsultarAlerta(iOficinaConsularId, ref bAlertaPendienteFlag, ref bAlertaEnviadosFlag);

            lblMsjPendienteEnvio.Text = "";
            lblMsjEnviados.Text = "";

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (bAlertaPendienteFlag)
                {
                    lblMsjPendienteEnvio.Text = "Consulados tienen Remesas PENDIENTE de envío.";
                }
                if (bAlertaEnviadosFlag)
                {
                    lblMsjEnviados.Text = "Remesas ENVIADAS por Consulados.";
                }
            }
            else
            {
                if (bAlertaPendienteFlag)
                {
                    lblMsjPendienteEnvio.Text = "Tiene Remesa PENDIENTE de envío.";
                }
            }
        }
    }
}
