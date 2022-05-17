using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;

using SGAC.Registro.Actuacion.BL;
using SGAC.Controlador;
using System.Collections;
using Microsoft.Reporting.WebForms;
//--------------------------------------------//
// Fecha: 04/01/2017
// Autor: Miguel Márquez Beltrán
// Objetivo: Consultar las Fichas Registrales
//--------------------------------------------//

namespace SGAC.WebApp.Consulta
{
    public partial class FrmFichaRegistral : System.Web.UI.Page
    {
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginadorFicha.PageSize = 30;
            ctrlPaginadorFicha.Visible = false;
            ctrlPaginadorFicha.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EstablecerEstadoFichaSeleccionada();
                lblDetalle.Visible = false;
                lblNroFicha.Visible = false;
                btnSeleccionarFichas.Visible = false;
                chkSeleccionarTodo.Visible = false;
                HFGUID.Value = PageUniqueId.Replace("-", "");

                /* Filtra por atributos del usuario */
                if ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ctrlOficinaConsular.Cargar(false, false);
                    ctrlOficinaConsular.ddlOficinaConsular.Enabled = false;
                }
                else
                {
                    ctrlOficinaConsular.Cargar(true, false);
                    ctrlOficinaConsular.ddlOficinaConsular.Enabled = true;
                    ctrlOficinaConsular.ddlOficinaConsular.SelectedValue = "1";

                }
            }
            ctrFechaEnvio.AllowFutureDate = true;

            
        }
        private void EstablecerEstadoFichaSeleccionada()
        {
            ddlEstadoFicha.Items.Clear();

            DataTable dtEstado = new DataTable();
//            dtEstado = Comun.ObtenerParametrosPorGrupo((DataTable)Session[Constantes.CONST_SESION_DT_ESTADO], SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);
            dtEstado = comun_Part1.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_FICHA_ESTADO);
            string strtexto = "";
            string strvalue = "";

            ListItem listaItems = new ListItem();
            for (int i = 0; i < dtEstado.Rows.Count; i++)
            {
                strtexto = dtEstado.Rows[i]["VALOR"].ToString().Trim();
                strvalue = dtEstado.Rows[i]["ID"].ToString().Trim();
                if (dtEstado.Rows[i]["VALOR"].ToString().Trim() != "RECUPERADO CON FICHA"
                    && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "RECUPERADO - ENVIADO"
                    && dtEstado.Rows[i]["VALOR"].ToString().Trim() != "INCOMPLETO")
                {
                    listaItems = new ListItem(strtexto, strvalue);
                    ddlEstadoFicha.Items.Add(listaItems);
                }
            }
            ddlEstadoFicha.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
        }
        protected void gdvFicha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long intFichaRegistralId = 0;


            if (e.CommandName == "Consultar")
            {
                #region ConsultarFichaRegistral

                    GridViewRow gvrModificar;
                    gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                    Label lblFichaRegistralId = (Label)gvrModificar.FindControl("lblFichaRegistralId");

                    intFichaRegistralId = Convert.ToInt64(lblFichaRegistralId.Text.Trim());

                    HFFichaRegistralId.Value = lblFichaRegistralId.Text.Trim();

                    Label lblNumeroFicha = (Label)gvrModificar.FindControl("lblNumeroFicha");

                    lblNroFicha.Text = lblNumeroFicha.Text;
                    lblDetalle.Visible = true;
                    lblNroFicha.Visible = true;

                    ConsultarFichaRegistralHistorico(intFichaRegistralId);
                #endregion
            }
            if (e.CommandName == "Editar")
            {
                #region EditarFichaRegistral
                    GridViewRow gvrModificar;
                    gvrModificar = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                    Label lblTarifarioId = (Label)gvrModificar.FindControl("lblTarifarioId");

                    Int16 intTarifaId = Convert.ToInt16(lblTarifarioId.Text.Trim());

                    Label lblActuacionDetalleId = (Label)gvrModificar.FindControl("lblActuacionDetalleId");

                    long intActuacionDetalleId = Convert.ToInt64(lblActuacionDetalleId.Text.Trim());

                
                    Label lblActuacionId = (Label)gvrModificar.FindControl("lblActuacionId");

                    long intActuacionId = Convert.ToInt64(lblActuacionId.Text.Trim());

                    Label lblPersonaId = (Label)gvrModificar.FindControl("lblPersonaId");
                    Label lblNumeroFicha = (Label)gvrModificar.FindControl("lblNumeroFicha");

                    //Session["iPersonaId" + HFGUID.Value] = lblPersonaId.Text.Trim();


                    InvocarActoGeneral(intTarifaId, intActuacionId, intActuacionDetalleId, lblPersonaId.Text.Trim(),lblNumeroFicha.Text);
                #endregion
            }
        }
        protected void gdvFicha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (chkSIO.Checked)
                {
                    CheckBox chkbox = (CheckBox)e.Row.FindControl("chkSeleccionar");
                    chkbox.Enabled = true;
                }
                else {
                    CheckBox chkbox = (CheckBox)e.Row.FindControl("chkSeleccionar");
                    chkbox.Enabled = false;
                }
            }
            
        }

        protected void ctrlPageBarFichaRegistral_Click(object sender, EventArgs e)
        {
            BusquedaFichaRegistral();
        }
       
        protected void ctrlPageBarFichaHistorica_Click(object sender, EventArgs e)
        {
            long intFichaRegistralId = Convert.ToInt64(HFFichaRegistralId.Value);
            
            ConsultarFichaRegistralHistorico(intFichaRegistralId);

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            BusquedaFichaRegistral();

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
        }

        private void BusquedaFichaRegistral(long intFichaRegistralId = 0)
        {
            string strNroFichaRegistral = txtNroFichaRegistral.Text.Trim();
            int intEstadoFichaId =  Convert.ToInt32(ddlEstadoFicha.SelectedValue);
            string strFechaInicio = "";
            string strFechaFin = "";
            int intRGE = 0;                                
            string strNroGuia = txtGuia.Text.Trim();
            string strNroDocumento = txtNroDocParticipante.Text.Trim();
            string strPrimerApellido = txtApePatParticipante.Text.Trim();
            string strSegundoApellido = txtApeMatParticipante.Text.Trim();
            string strNombres = txtNomParticipante.Text.Trim();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = 30;
            int PaginaActual = ctrlPaginadorFicha.PaginaActual;
            int intOficinaConsularId = 0;
            string strNroHoja = txtNroHojaSelGuia.Text.Trim();
            bool bSIO = false;

            if (chkSIO.Checked)
            {
                bSIO = true;   
            }
            else { 
                bSIO = false; 
            }
            if (txtFechaInicio.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strFechaInicio = Comun.FormatearFecha(txtFechaInicio.Text).ToString("yyyyMMdd");
            }
            if (txtFechaFin.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strFechaFin = Comun.FormatearFecha(txtFechaFin.Text).ToString("yyyyMMdd");
            }

            if (txtRGE.Text.Trim().Length > 0)
            {
                intRGE = Convert.ToInt32(txtRGE.Text.Trim());
            }

            intOficinaConsularId = Convert.ToUInt16(ctrlOficinaConsular.SelectedValue);
           

            DataTable dtFichaRegistral = new DataTable();
            FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();

            dtFichaRegistral = objFichaRegistralBL.ConsultarTitular(intOficinaConsularId, intFichaRegistralId, strNroFichaRegistral, intEstadoFichaId, strFechaInicio, strFechaFin,
                                                                    intRGE, strNroGuia, strNroDocumento, strPrimerApellido, strSegundoApellido, strNombres, strNroHoja, bSIO,
                                                                    PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dtFichaRegistral.Rows.Count > 0)
            {
                ctrlPaginadorFicha.TotalResgistros = IntTotalCount;
                ctrlPaginadorFicha.TotalPaginas = IntTotalPages;

                ctrlPaginadorFicha.Visible = false;
                if (chkSIO.Checked)
                {
                    btnSeleccionarFichas.Visible = true;
                    chkSeleccionarTodo.Visible = true;
                }
                if (ctrlPaginadorFicha.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginadorFicha.Visible = true;
                }
                chkSeleccionarTodo.Checked = false;
            }
            else
            {
                btnSeleccionarFichas.Visible = false;
                chkSeleccionarTodo.Visible = false;
                ctrlPaginadorFicha.Visible = false;
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            lblDetalle.Visible = false;
            lblNroFicha.Visible = false;
            gdvHistorico.DataSource = null;
            gdvHistorico.DataBind();

            gdvFicha.DataSource = dtFichaRegistral;
            gdvFicha.DataBind();
            updConsulta.Update();
        }

        private void ConsultarFichaRegistralHistorico(long intFichaRegistralId = 0)
        {
            DataTable dtFichaRegistralHistorico = new DataTable();
            FichaRegistralHistoricoBL objFichaRegistralHistoricoBL = new FichaRegistralHistoricoBL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = ctrlPaginadorFichaHistorica.PaginaActual;


            dtFichaRegistralHistorico = objFichaRegistralHistoricoBL.Consultar(intFichaRegistralId,
                                        PaginaActual, intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dtFichaRegistralHistorico.Rows.Count > 0)
            {
                ctrlPaginadorFichaHistorica.TotalResgistros = IntTotalCount;
                ctrlPaginadorFichaHistorica.TotalPaginas = IntTotalPages;

                ctrlPaginadorFichaHistorica.Visible = false;

                if (ctrlPaginadorFichaHistorica.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginadorFichaHistorica.Visible = true;
                }
            }
            else
            {
                ctrlPaginadorFichaHistorica.Visible = false;
            }
            gdvHistorico.DataSource = dtFichaRegistralHistorico;
            gdvHistorico.DataBind();
        }

        private void InvocarActoGeneral(Int16 intTarifaId, long intActuacionId, long intActuacionDetalleId,string codPersona,string fichaRegistrar ="")
        {
            //Proceso p = new Proceso();
            Enumerador.enmTipoOperacion enmTipoOperacion = Enumerador.enmTipoOperacion.ACTUALIZACION;

            //object[] arrParametros = { intTarifaId, (int)Enumerador.enmTipoOperacion.ACTUALIZACION };

            //string strFormulario = (string)p.Invocar(ref arrParametros, "ACTUACIONDET_FORMATO", Enumerador.enmAccion.CONSULTAR);


            ActuacionMantenimientoBL objActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            string strFormulario = objActuacionMantenimientoBL.ObtenerFormularioPorTarifa(intTarifaId, (int)Enumerador.enmTipoOperacion.ACTUALIZACION).ToString();
 

            if (strFormulario != null)
            {
                //string strFormulario = "FrmActoGeneral.aspx-1|2|5";

                string[] arrDatos = strFormulario.Split('-');

                Session["Actuacion_Accion"] = (int)enmTipoOperacion;

                BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(intActuacionDetalleId);

                //dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]
                //75206

                Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);

                Session.Add(Constantes.CONST_SESION_ACTUACIONDET_TABS, arrDatos[1]);
                Session["ACTO_GENERAL_MRE"] = null;

                Session[Constantes.CONST_SESION_ACTUACIONDET_ID +  HFGUID.Value] = intActuacionDetalleId;
                Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = intActuacionDetalleId;
                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = intActuacionId;
                fichaRegistrar = Util.Encriptar(fichaRegistrar);
                string codPerEncriptado = Util.Encriptar(codPersona);
                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmActoGeneral.aspx?CodPer=" + codPerEncriptado + "&fregi=" + fichaRegistrar + "&Juridica=1", false);
                }
                else
                { // PERSONA NATURAL
                    Response.Redirect("~/Registro/FrmActoGeneral.aspx?CodPer=" + codPerEncriptado + "&fregi=" + fichaRegistrar,false);
                }
                

            }
        }

        private BE.RE_TARIFA_PAGO ObtenerDatosTarifaPago(Int64 lngActuacionDetalleId)
        {
            ActuacionPagoConsultaBL objBL = new ActuacionPagoConsultaBL();
            DataTable dtPago = objBL.ActuacionPagoObtener(lngActuacionDetalleId);

            BE.RE_TARIFA_PAGO objTarifaPago = new BE.RE_TARIFA_PAGO();
            if (dtPago.Rows.Count > 0)
            {
                DataRow dr = dtPago.Rows[0];

                objTarifaPago.sTarifarioId = Convert.ToInt16(dr["sTarifarioId"]);
                objTarifaPago.vTarifa = dr["vTarifa"].ToString();
                objTarifaPago.vTarifaDescripcion = dr["vTarifa"].ToString() + " - " + dr["vTarifaDescripcion"].ToString();
                objTarifaPago.vTarifaDescripcionLarga = dr["descripcion"].ToString();
                objTarifaPago.datFechaRegistro = Comun.FormatearFecha(dr["Fecha"].ToString());
                objTarifaPago.tari_sSeccionId = Convert.ToInt32(dr["tari_sSeccionId"]);
                objTarifaPago.sTipoActuacion = Convert.ToInt16(dr["sTipoActuacion"]);

                objTarifaPago.sTipoPagoId = Convert.ToInt16(dr["pago_sPagoTipoId"]);
                objTarifaPago.dblCantidad = Convert.ToDouble(dr["Cantidad"]);
                objTarifaPago.dblMontoSolesConsulares = Convert.ToDouble(dr["FSolesConsular"]);
                objTarifaPago.dblMontoMonedaLocal = Convert.ToDouble(dr["FMonedaExtranjera"]);
                objTarifaPago.dblTotalSolesConsulares = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                objTarifaPago.dblTotalMonedaLocal = Convert.ToDouble(dr["FTOTALMONEDALocal"]);
                objTarifaPago.vObservaciones = dr["acde_vNotas"].ToString();
                objTarifaPago.vMonedaLocal = dr["vMonedaLocal"].ToString();

                if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.DEPOSITO_CUENTA ||
                    objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.TRANSFERENCIA_BANCARIA)
                {
                    if (Convert.ToInt16(dr["pago_sBancoId"]) != 0)
                    {
                        objTarifaPago.vNumeroOperacion = Convert.ToString(dr["pago_vBancoNumeroOperacion"]);
                        objTarifaPago.sBancoId = Convert.ToInt16(dr["pago_sBancoId"]);
                        objTarifaPago.datFechaPago = Comun.FormatearFecha(dr["pago_dFechaOperacion"].ToString());
                        objTarifaPago.dblMontoCancelado = Convert.ToDouble(dr["FTOTALSOLESCONSULARES"]);
                    }
                }
                //--------------------------------------------
                // Creador por: Miguel Angel Márquez Beltrán
                // Fecha: 15-08-2016
                // Objetivo: Adicionar la columna Clasificacion
                // Referencia: Requerimiento No.001_2.doc
                //--------------------------------------------
                objTarifaPago.dblClasificacion = Convert.ToDouble(dr["acde_sClasificacionTarifaId"]);
                //--------------------------------------------
                // Creador por: Miguel Angel Márquez Beltrán
                // Fecha: 27-10-2016
                // Objetivo: Adicionar la columna Norma del Tarifario
                //--------------------------------------------
                objTarifaPago.dblNormaTarifario = Convert.ToDouble(dr["pago_iNormaTarifarioId"]);
                //--------------------------------------------
            }
            return objTarifaPago;
        }

        protected void chkSIO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSIO.Checked)
            {
                ocultarDiv.Visible = false;
                gdvFicha.DataSource = null;
                gdvFicha.DataBind();
                ctrlPaginadorFicha.Visible = false;
                txtGuia.Enabled = false;
            }
            else {
                ocultarDiv.Visible = true; 
                btnSeleccionarFichas.Visible = false;
                chkSeleccionarTodo.Visible = false;
                gdvFicha.DataSource = null;
                gdvFicha.DataBind();
                ctrlPaginadorFicha.Visible = false;
                txtGuia.Enabled = true;
            }
        }

        protected void btnSeleccionarFichas_Click(object sender, EventArgs e)
        {

            ArrayList listaDocumentos = new ArrayList();
            SGAC.BE.MRE.RE_FICHAREGISTRAL objFichaBE;
            bool resultado = false;
            foreach (GridViewRow row in gdvFicha.Rows)
            {
                CheckBox chkSelItem;
                chkSelItem = (CheckBox)row.FindControl("chkSeleccionar");
                if (chkSelItem.Checked == true)
                {
                    resultado = true;
                    break;
                }
            }
            if (resultado == false)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Fichas SIO - Actualizar", "Debe seleccionar al menos un registro."));
                return;
            }

            foreach (GridViewRow row in gdvFicha.Rows)
            {
                CheckBox chkSelItem;
                chkSelItem = (CheckBox)row.FindControl("chkSeleccionar");

                Label lblFICHAREGISTRARLID;
                lblFICHAREGISTRARLID = (Label)row.FindControl("lblFichaRegistralId");
                if (chkSelItem.Checked == true)
                {
                    objFichaBE = new BE.MRE.RE_FICHAREGISTRAL();
                    objFichaBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    objFichaBE.fire_vNumeroOficio = txtHojaRemi.Text;
                    objFichaBE.fire_iFichaRegistralId = Convert.ToInt64(lblFICHAREGISTRARLID.Text);

                    objFichaBE.fire_dFechaEnvio = Comun.FormatearFecha(ctrFechaEnvio.Text);
                    objFichaBE.fire_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objFichaBE.fire_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    listaDocumentos.Add(objFichaBE);
                }
            }

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    CheckBox chkSelItem;
                //    chkSelItem = (CheckBox)gdvFicha.Rows[i].FindControl("chkSeleccionar");
                //    if (chkSelItem.Checked == true)
                //    {
                //        objFichaBE = new BE.MRE.RE_FICHAREGISTRAL();
                //        objFichaBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                //        objFichaBE.fire_vNumeroOficio = txtHojaRemi.Text;
                //        objFichaBE.fire_iFichaRegistralId = Convert.ToInt64(dt.Rows[i]["FIRE_IFICHAREGISTRALID"].ToString());

                //        objFichaBE.fire_dFechaEnvio = Comun.FormatearFecha(ctrFechaEnvio.Text);
                //        objFichaBE.fire_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                //        objFichaBE.fire_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                //        listaDocumentos.Add(objFichaBE);
                //    }
                //}
            FichaRegistralBL objFicha = new FichaRegistralBL();

            objFicha.ActualizarEnvioSIO(listaDocumentos);

            if (!(objFicha.isError))
            {
                BusquedaFichaRegistral();
                updConsulta.Update();
                chkSeleccionarTodo.Checked = false;
                txtHojaRemi.Text = "";
                ctrFechaEnvio.Text = "";
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Fichas SIO - Actualizar", Constantes.CONST_MENSAJE_EXITO));
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Fichas SIO - Actualizar", Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            long intFichaRegistralId = 0;
            string strNroFichaRegistral = txtNroFichaRegistral.Text.Trim();
            int intEstadoFichaId = Convert.ToInt32(ddlEstadoFicha.SelectedValue);
            string strFechaInicio = "";
            string strFechaFin = "";
            int intRGE = 0;
            string strNroGuia = txtGuia.Text.Trim();
            string strNroDocumento = txtNroDocParticipante.Text.Trim();
            string strPrimerApellido = txtApePatParticipante.Text.Trim();
            string strSegundoApellido = txtApeMatParticipante.Text.Trim();
            string strNombres = txtNomParticipante.Text.Trim();
            bool bSIO = false;
            int intOficinaConsularId = 0;
            string strNroHoja = txtNroHojaSelGuia.Text.Trim();
            
            if (txtFechaInicio.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strFechaInicio = Comun.FormatearFecha(txtFechaInicio.Text).ToString("yyyyMMdd");
            }
            if (txtFechaFin.Text.Trim().Length > 10)
            {
                if (Comun.EsFecha(txtFechaFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);                    
                    return;
                }
                strFechaFin = Comun.FormatearFecha(txtFechaFin.Text).ToString("yyyyMMdd");
            }

            if (txtRGE.Text.Trim().Length > 0)
            {
                intRGE = Convert.ToInt32(txtRGE.Text.Trim());
            }

            
            intOficinaConsularId = Convert.ToUInt16(ctrlOficinaConsular.SelectedValue); 
          
       
            

            if (chkSIO.Checked)
            {
                bSIO = true;
            }
            else{bSIO = false;}

            DataTable dtFichaRegistral = new DataTable();
            FichaRegistralBL objFichaRegistralBL = new FichaRegistralBL();

            dtFichaRegistral = objFichaRegistralBL.ConsultarTitularReporte(intOficinaConsularId, intFichaRegistralId, strNroFichaRegistral, intEstadoFichaId, strFechaInicio, strFechaFin,
                                                                    intRGE, strNroGuia, strNroDocumento, strPrimerApellido, strSegundoApellido, strNombres, strNroHoja, bSIO);

            if (dtFichaRegistral.Rows.Count == 0)
            {
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "REPORTE", "No existe información para mostrar");
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            else {
                string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
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
                //----------------------------

                //string strFechaActualConsulado = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session))).ToString("MMM-dd-yyyy");
                //string strHoraActualConsulado = Accesorios.Comun.ObtenerHoraActualTexto(HttpContext.Current.Session);

                ReportParameter[] parameters = new ReportParameter[6];
                parameters[0] = new ReportParameter("TituloReporte", "FICHAS REGISTRALES");
                parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
                parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
                parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
                parameters[4] = new ReportParameter("FechaActual", strFechaActualConsulado);
                parameters[5] = new ReportParameter("HoraActual", strHoraActualConsulado);

                Session["strNombreArchivo"] = "Reportes/rsFichasRegistrales.rdlc";
                Session["DtDatos"] = dtFichaRegistral;
                Session["objParametroReportes"] = parameters;
                Session["DataSet"] = "dtFichaRegistrales";
                string strUrl = "../Reportes/frmVisorReporte.aspx";
                string Script = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=700,height=700,left=100,top=100');";
                Comun.EjecutarScript(Page, Script);
            }
        }

        //------------------------------------------
        //Fecha: 02/04/2019
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear GUID 
        //------------------------------------------        
        private string _pageUniqueId = Guid.NewGuid().ToString();

        public string PageUniqueId
        {
            get { return _pageUniqueId; }
        }
    }
}
