using System;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using Microsoft.Reporting.WebForms;
using SGAC.Registro.Actuacion.BL;
using System.Web;

namespace SGAC.WebApp.Consulta
{
    public partial class FrmJudicial : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "ESTADO EXPEDIENTE";

        #endregion

        #region Eventos

        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.VisibleIButtonPrint = true;
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
            //------------------------------------------------
            //Fecha: 04/01/2017
            //Autor: Jonatan Silva Cachay
            //Objetivo: Para la impresión
            //------------------------------------------------
            ctrlToolBarConsulta.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonPrintClick(ctrlToolBarConsulta_btnImprimirHandler);
            
            string strFormatofecha = "";
            strFormatofecha = WebConfigurationManager.AppSettings["FormatoFechas"];
            Session["Formatofecha"] = strFormatofecha;

            txtFechaInicio.EndDate = ObtenerFechaActual(Session);
            txtFechaFin.EndDate = ObtenerFechaActual(Session);

            if (!Page.IsPostBack)
            {
                try
                {
                    HFGUID.Value = PageUniqueId.Replace("-", "");

                    CargarListadosDesplegables();
                    txtFechaInicio.Text = ObtenerFechaActual(Session).ToString(WebConfigurationManager.AppSettings["FormatoFechas"]);
                    txtFechaFin.Text = ObtenerFechaActual(Session).ToString(WebConfigurationManager.AppSettings["FormatoFechas"]);
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

        void ctrlToolBarConsulta_btnPrintHandler()
        {
            DataTable DtDatos = new DataTable();
            ReportParameter[] parameters = new ReportParameter[4];
            string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
            BuscarExpediente();

            parameters[0] = new ReportParameter("TituloReporte", "EXPEDIENTES JUDICIALES");
            parameters[1] = new ReportParameter("SubTituloReporte", "");
            parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());

            DtDatos = (DataTable)Session["Expediente"];

            Session["strNombreArchivo"] = "Reportes/rsActoJudicialConsulta.rdlc";
            Session["DtDatos"] = DtDatos;
            Session["objParametroReportes"] = parameters;

            string strUrl = "../Reportes/FrmVisorRDLC.aspx";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=1000,height=700,left=100,top=100');";
            EjecutarScript(Page, strScript);
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            try
            {
                // LIMPIAR CONSULTA
                txtNroExpediente.Text = string.Empty;
                txtNroHojaRemision.Text = string.Empty;
                ddlExpedienteEstado.SelectedIndex = 0;
                ddlOficinaConsular.SelectedIndex = 0;
                ddlTipoPersona.SelectedIndex = 0;
                txtDemandado.Text = string.Empty;

                txtFechaInicio.Text = ObtenerFechaActual(Session).ToString(WebConfigurationManager.AppSettings["FormatoFechas"]);
                txtFechaFin.Text = ObtenerFechaActual(Session).ToString(WebConfigurationManager.AppSettings["FormatoFechas"]);

                LimpiarNotificados();
                gdvExpediente.DataSource = null;
                gdvExpediente.DataBind();
                ctrlPaginador.Visible = false;
                lblListadoExpediente.Visible = false;

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

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            try
            {
                ctrlPaginador.PaginaActual = 1;
                BuscarExpediente();

                LimpiarNotificados();
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
        //------------------------------------------------
        //Fecha: 04/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Para la impresión
        //------------------------------------------------
        void ctrlToolBarConsulta_btnImprimirHandler()
        {
            if (txtFechaInicio.Text.Trim().Length == 0 || txtFechaFin.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }
            if (Comun.EsFecha(txtFechaInicio.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }
            if (Comun.EsFecha(txtFechaFin.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }
            try
            {
                // LIMPIAR CONSULTA
                
                gdvExpediente.DataSource = null;
                gdvExpediente.DataBind();
                ctrlPaginador.Visible = false;
                lblListadoExpediente.Visible = false;
                
                //IMPRIMIR
                DataTable DtDatos = new DataTable();
                ReportParameter[] parameters = new ReportParameter[4];
                string sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                RetornarListaExpedientes();
                DtDatos = (DataTable)Session["Expediente"];
                if (DtDatos.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion("No se han encontrado registros con los criterios indicados", true, Enumerador.enmTipoMensaje.WARNING);
                    return;
                }


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
                if (ddlOficinaConsular.SelectedValue.ToString().Equals("0"))
                {
                    sNombreOficinaConsular = "TODOS";
                }
                else
                {
                    sNombreOficinaConsular = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(ddlOficinaConsular.SelectedValue.ToString()));
                    sNombreOficinaConsular = sNombreOficinaConsular.Split('-')[1].ToString().Trim();
                }

                parameters = new ReportParameter[7];
                parameters[0] = new ReportParameter("TituloReporte", "EXHORTOS CONSULARES");
                parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
                parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
                parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
                parameters[4] = new ReportParameter("FechaHaber", " Del " + txtFechaInicio.Text + " al " + txtFechaFin.Text);
                parameters[5] = new ReportParameter("FechaActual", strFechaActualConsulado);
                parameters[6] = new ReportParameter("HoraActual", strHoraActualConsulado);
                                
                Session["strNombreArchivo"] = "Reportes/rsActoJudicialExhortos.rdlc";
                Session["DtDatos"] = DtDatos;
                Session["objParametroReportes"] = parameters;
                Session["DataSet"] = "Expediente";
                string strUrl = "../Reportes/frmVisorReporte.aspx";
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=1000,height=700,left=100,top=100');";
                EjecutarScript(Page, strScript);
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
        protected void gdvExpediente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Int64 iActoJudicialId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iActoJudicialId"].ToString());
                Int64 iActoJudicialParticipanteId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iActoJudicialParticipanteId"].ToString());
                Int64 iCodPer = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iCodPer"].ToString());
                int sEstadoId = Convert.ToInt16(gdvExpediente.DataKeys[index].Values["sEstadoId"].ToString());
                
                Session[Constantes.CONST_SESION_ACTUACION_ID] = gdvExpediente.DataKeys[index].Values["iActuacionId"].ToString();
                string strParametro = "";
                LimpiarNotificados();

                if (e.CommandName == "VerNotificado")
                {
                    verNotificados(index);
                    updGrillaNotifica.Update();
                }
                if (e.CommandName == "Historico")
                {
                    verHistorico(index);
                    UpdExpedienteHistorico.Update();
                }
                else if (e.CommandName == "Ver" || e.CommandName == "Actas" )
                {
                    //Cuando se ha seleccionado actas se realiza lo mismo que al 'Ver'
                    //pero adicionalmente se pasa información del acta para que se cargue
                    //en el formulario acto judicial.
                    if (e.CommandName == "Actas")
                    {
                        Session["iCargarTabActas"] = "1";
                        Session["iActoJudicialParticipanteId"] = iActoJudicialParticipanteId;
                        strParametro = iActoJudicialId.ToString() + "-2";
                    }
                    else
                        strParametro = iActoJudicialId.ToString() + "-3";
                    
                    Session["sActoJudicialId"] = strParametro;
                    Session["sDeDondeViene"] = 1;     // LE INDICAMOS AL FORMULARIO FrmActoJudicial QUE ESTA SIENDO LLAMADO DESDE  FrmExpediente
                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                    {
                        Response.Redirect("../Registro/FrmActoJudicial.aspx?CodPer=" + Util.Encriptar(iCodPer.ToString()) + "&Juridica=1", false);
                    }
                    else
                    { // PERSONA NATURAL
                        Response.Redirect("../Registro/FrmActoJudicial.aspx?CodPer=" + Util.Encriptar(iCodPer.ToString()), false);
                    }
                   
                }
                else if ( e.CommandName == "Editar")
                {
                    Int16 intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    Int16 intOficinaLima = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

                    if (intOficinaConsular == intOficinaLima)
                    {
                        if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.ENVIADO))
                        {
                            ctrlValidacion.MostrarValidacion("No se puede modificar un Expediente enviado", true);
                            updConsulta.Update();
                            return;
                        }

                        if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                        {
                            ctrlValidacion.MostrarValidacion("No se puede modificar un Expediente cerrado", true);
                            updConsulta.Update();
                            return;
                        }
                    }
                    else
                    {
                        if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO))
                        {
                            ctrlValidacion.MostrarValidacion("No puede modificar un Expediente que aun no ha sido Enviado", true);
                            updConsulta.Update();
                            return;
                        }

                        if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                        {
                            ctrlValidacion.MostrarValidacion("No puede modificar un Expediente Cerrado", true);
                            updConsulta.Update();
                            return;
                        }
                    }

                    verJudicial(iActoJudicialId, 2, iCodPer);
                }
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

        void verJudicial(Int64 intActoJudicialId, int intTipoAccion, Int64 iCodPer)
        {
            string iActoJudicialId = Convert.ToString(intActoJudicialId);
            int intAccionJudicial = intTipoAccion; // 1 = NUEVO ; 2 = MODIFICAR ; 3 = SOLO CONSULTA
            //-------------------------------------------------------------
            //Fecha: 21/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Deshabilitar variables sin uso.
            //-------------------------------------------------------------
            //int intTipoDemandante = Convert.ToInt32(Session["iTipoId"]); // 2101 = PERSONA // 2102 = PERSONA JURIDICA
            //long lngPersonaId = Convert.ToInt64(Session["iPersonaId"]);
            //long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID]);

            iActoJudicialId += "-" + intAccionJudicial + "-" + "" + "-" + "" + "-" + "";
            Session["sActoJudicialId"] = iActoJudicialId;
            Session["sDeDondeViene"] = 1;     // LE INDICAMOS AL FORMULARIO FrmActoJudicial QUE ESTA SIENDO LLAMADO DESDE  FrmExpediente
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("../Registro/FrmActoJudicial.aspx?CodPer=" + Util.Encriptar(iCodPer.ToString()) + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("../Registro/FrmActoJudicial.aspx?CodPer=" + Util.Encriptar(iCodPer.ToString()), false);
            }
            
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarExpediente();
                LimpiarNotificados();
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

        #endregion

        #region Métodos

        private void CargarListadosDesplegables()
        {
            try
            {
                ddlOficinaConsular.Cargar(true, true, "- TODOS -");
                Util.CargarParametroDropDownList(ddlTipoPersona, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO), true, "- TODOS -");
                Util.CargarParametroDropDownList(ddlExpedienteEstado, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.JUDICIAL_ESTADO_EXPEDIENTE), true, "- TODOS -");
                
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

                // SI LA OFICINA CONSULAR ES DIFERNTE A LIMA
                if (intOficinaConsularId != intOficinaConsularLimaId)
                {
                    ddlOficinaConsular.SelectedValue = Convert.ToString(intOficinaConsularId);
                    ddlOficinaConsular.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region metodos

        private void LimpiarNotificados()
        {
            try
            {
                lblDetalleNotifica.Visible = false;
                lblDatosExpediente.Visible = false;
                lblDatosExpedienteHistorico.Visible = false;
                lblDetalleExpedienteHistorico.Visible = false;

                gdvNotificados.DataSource = null;
                gdvNotificados.DataBind();

                gvdExpedienteHistorico.DataSource = null;
                gvdExpedienteHistorico.DataBind();

                updGrillaNotifica.Update();
                UpdExpedienteHistorico.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void verNotificados(int index)
        {
            try
            {
                Int64 iExpedienteId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iActoJudicialId"].ToString());
                Int64 iActoJudicialParticipanteId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iActoJudicialParticipanteId"].ToString());
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();

                //object[] arrParametros = { iExpedienteId, iActoJudicialParticipanteId };
                //dt = (DataTable)p.Invocar(ref arrParametros, "Notificado", Enumerador.enmAccion.CONSULTAR);

                ActoJudicialConsultaBL objActoJudicialConsultaBL = new ActoJudicialConsultaBL();

                dt = objActoJudicialConsultaBL.consultar_Exp_Participante(iExpedienteId, iActoJudicialParticipanteId);


                gdvNotificados.DataSource = dt;
                gdvNotificados.DataBind();

                lblDetalleExpedienteHistorico.Visible = false;
                lblDatosExpedienteHistorico.Visible = false;
                lblDetalleNotifica.Visible = false;
                lblDatosExpediente.Visible = false;
                gvdExpedienteHistorico.Visible = false;

                if (dt.Rows.Count > 0)
                {
                    gdvNotificados.Visible = true;
                    lblDatosExpediente.Visible = true;
                    lblDetalleNotifica.Visible = true;
                    lblDatosExpediente.Text = gdvExpediente.Rows[index].Cells[2].Text;
                }
                else
                {
                    CtrValidaNotifica.MostrarValidacion("No se ha encontrado notificaciones para este Participante", true, Enumerador.enmTipoMensaje.WARNING);
                    updGrillaExpediente.Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void verHistorico(int index)
        {
            try
            {
                Int64 iExpedienteId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["iActoJudicialId"].ToString());
                ActoJudicialConsultaBL actoJudicialConsultaBL = new ActoJudicialConsultaBL();

                DataTable dt = actoJudicialConsultaBL.Consultar_Expediente_Historico(iExpedienteId);

                gvdExpedienteHistorico.DataSource = dt;
                gvdExpedienteHistorico.DataBind();

                lblDetalleExpedienteHistorico.Visible = false;
                lblDatosExpedienteHistorico.Visible = false;
                lblDetalleNotifica.Visible = false;
                lblDatosExpediente.Visible = false;
                gdvNotificados.Visible = false;

                if (dt.Rows.Count > 0)
                {
                    gvdExpedienteHistorico.Visible = true;
                    lblDatosExpedienteHistorico.Visible = true;
                    lblDetalleExpedienteHistorico.Visible = true;
                    lblDatosExpedienteHistorico.Text = gdvExpediente.Rows[index].Cells[2].Text;
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //------------------------------------------------
        //Fecha: 04/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Para la impresión
        //------------------------------------------------
        private void RetornarListaExpedientes()
        {
            try
            {
                if (txtFechaInicio.Text != "" && txtFechaFin.Text == "")
                {
                    ctrlValidacion.MostrarValidacion("Rango de fecha Invalida", true, Enumerador.enmTipoMensaje.ERROR);
                    updGrillaExpediente.Update();
                    return;
                }

                if (txtFechaInicio.Text == "" && txtFechaFin.Text != "")
                {
                    ctrlValidacion.MostrarValidacion("Rango de fecha Invalida", true, Enumerador.enmTipoMensaje.ERROR);
                    updGrillaExpediente.Update();
                    return;
                }

                if (txtFechaInicio.Text != "" && txtFechaFin.Text != "")
                {
                    DateTime datFechaInicio = new DateTime();
                    DateTime datFechaFin = new DateTime();

                    datFechaInicio = txtFechaInicio.Value();
                    datFechaFin = txtFechaFin.Value();

                    if (datFechaInicio > datFechaFin)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                        updGrillaExpediente.Update();
                    }
                    else
                    {
                        LlenarDatatableExpedientes();
                    }
                }
                else
                {
                    LlenarDatatableExpedientes();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BuscarExpediente()
        {
            try
            {
                if (txtFechaInicio.Text.Trim().Length == 0 || txtFechaFin.Text.Trim().Length == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                    updConsulta.Update();
                    return;
                }
                if (Comun.EsFecha(txtFechaInicio.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                    updConsulta.Update();
                    return;
                }
                if (Comun.EsFecha(txtFechaFin.Text.Trim()) == false)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                    updConsulta.Update();
                    return;
                }

                if (txtFechaInicio.Text != "" && txtFechaFin.Text != "")
                {
                    DateTime datFechaInicio = new DateTime();
                    DateTime datFechaFin = new DateTime();

                    datFechaInicio = txtFechaInicio.Value();
                    datFechaFin = txtFechaFin.Value();

                    if (datFechaInicio > datFechaFin)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                        updGrillaExpediente.Update();
                    }
                    else
                    {
                        CargarGrilla();
                    }
                }
                else
                {
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //------------------------------------------------
        //Fecha: 04/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Para la impresión
        //------------------------------------------------
        private void LlenarDatatableExpedientes()
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dtExpediente = new DataTable();

                RE_ExpedienteJudicial objEn = new RE_ExpedienteJudicial();

                if (ddlOficinaConsular.SelectedIndex > 0)
                    objEn.sOficinaConsularDestinoId = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                if (txtNroExpediente.Text.Trim() != "")
                    objEn.vNumeroExpediente = txtNroExpediente.Text.Trim();
                if (ddlExpedienteEstado.SelectedIndex > 0)
                    objEn.sEstadoExpedienteId = Convert.ToInt32(ddlExpedienteEstado.SelectedValue);
                if (txtNroHojaRemision.Text.Trim() != "")
                    objEn.vNumeroHojaRemision = txtNroHojaRemision.Text.Trim();
                if (ddlTipoPersona.SelectedIndex > 0)
                    objEn.sTipoPersonaId = Convert.ToInt32(ddlTipoPersona.SelectedValue);

                if (txtDemandado.Text.Trim() != "")
                    objEn.vdemandado = txtDemandado.Text.Trim();

                if (!cbo_estado_actas.SelectedItem.Value.Equals("0"))
                    objEn.sEstadoActaId = Convert.ToInt16(cbo_estado_actas.SelectedItem.Value);

                //objEn.iPaginaActual = ctrlPaginador.PaginaActual;
                //objEn.iPaginaCantidad = 1000000;
                //objEn.iTotalRegistros = 0;
                //objEn.iTotalPaginas = 0;

                objEn.dFechaInicio = txtFechaInicio.Value();
                objEn.dFechaFin = txtFechaFin.Value();

                //object[] arrParametros = { objEn };
                //dtExpediente = (DataTable)p.Invocar(ref arrParametros, "RE_ExpedienteJudicial_REP", Enumerador.enmAccion.CONSULTAR);

                ActoJudicialConsultaBL objActoJudicialConsultaBL = new ActoJudicialConsultaBL();

                dtExpediente = objActoJudicialConsultaBL.Reporte_Expediente(objEn);


                Session["Expediente"] = dtExpediente;

                lblListadoExpediente.Visible = false;
                //if (p.IErrorNumero != 0)
                if (dtExpediente.Rows.Count == 0)
                {
                    //string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "No existe ningún registro.");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarGrilla()
        {
            try
            {
                ctrlPaginador.TotalResgistros = 0;
                ctrlPaginador.TotalPaginas = 0;


                //Proceso p = new Proceso();
                DataTable dtExpediente = new DataTable();

                RE_ExpedienteJudicial objEn = new RE_ExpedienteJudicial();

                if (ddlOficinaConsular.SelectedIndex > 0)
                    objEn.sOficinaConsularDestinoId = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                if (txtNroExpediente.Text.Trim() != "")
                    objEn.vNumeroExpediente = txtNroExpediente.Text.Trim();
                if (ddlExpedienteEstado.SelectedIndex > 0)
                    objEn.sEstadoExpedienteId = Convert.ToInt32(ddlExpedienteEstado.SelectedValue);
                if (txtNroHojaRemision.Text.Trim() != "")
                    objEn.vNumeroHojaRemision = txtNroHojaRemision.Text.Trim();
                if (ddlTipoPersona.SelectedIndex > 0)
                    objEn.sTipoPersonaId = Convert.ToInt32(ddlTipoPersona.SelectedValue);

                if (txtDemandado.Text.Trim() != "")
                    objEn.vdemandado = txtDemandado.Text.Trim();

                if (!cbo_estado_actas.SelectedItem.Value.Equals("0"))
                    objEn.sEstadoActaId = Convert.ToInt16(cbo_estado_actas.SelectedItem.Value);

                objEn.iPaginaActual = ctrlPaginador.PaginaActual;
                objEn.iPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
                objEn.iTotalRegistros = 0;
                objEn.iTotalPaginas = 0;
                
                objEn.dFechaInicio = txtFechaInicio.Value();
                objEn.dFechaFin = txtFechaFin.Value();     

                //object[] arrParametros = { objEn };
                //dtExpediente = (DataTable)p.Invocar(ref arrParametros, "RE_ExpedienteJudicial", Enumerador.enmAccion.CONSULTAR);

                ActoJudicialConsultaBL objActoJudicialConsultaBL = new ActoJudicialConsultaBL();

                dtExpediente = objActoJudicialConsultaBL.Consultar_Expediente(objEn);


                Session["Expediente"] = dtExpediente;

                lblListadoExpediente.Visible = false;
                //if (p.IErrorNumero == 0)
                //{
                    gdvExpediente.DataSource = dtExpediente;
                    gdvExpediente.DataBind();

                    if (dtExpediente.Rows.Count == 0)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                        gdvExpediente.Visible = false;
                    }
                    else
                    {
                        lblListadoExpediente.Visible = true;
                        gdvExpediente.Visible = true;
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + objEn.iTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                    }

                    ctrlPaginador.TotalResgistros = objEn.iTotalRegistros;
                    ctrlPaginador.TotalPaginas = objEn.iTotalPaginas;

                    ctrlPaginador.Visible = false;
                    if (ctrlPaginador.TotalPaginas > 1) ctrlPaginador.Visible = true;

                    updGrillaExpediente.Update();
                //}
                //else
                //{
                //    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
                //    Comun.EjecutarScript(Page, strScript);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object[] ObtenerFiltro()
        {
            try
            {
                int intTotalRegistros = 0, intTotalPaginas = 0;

                DateTime datFechaInicio = new DateTime();
                DateTime datFechaFin = new DateTime();

                if (!DateTime.TryParse(txtFechaInicio.Text, out datFechaInicio))
                {
                    datFechaInicio = Comun.FormatearFecha(txtFechaInicio.Text);
                }
                if (!DateTime.TryParse(txtFechaFin.Text, out datFechaFin))
                {
                    datFechaFin = Comun.FormatearFecha(txtFechaFin.Text);
                }

                object[] arrParametros = {ctrlPaginador.PaginaActual,
                                        Constantes.CONST_CANT_REGISTRO,
                                        intTotalRegistros,
                                        intTotalPaginas,
                                        txtNroExpediente.Text,
                                        ddlExpedienteEstado.SelectedValue,
                                        txtNroHojaRemision.Text,
                                        ddlOficinaConsular.SelectedValue,
                                        txtDemandado.Text,
                                        datFechaInicio,
                                        datFechaFin
                                        };
                return arrParametros;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion metodos

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        protected void gdvExpediente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e != null) 
            {
                if (e.Row != null)
                {
                    ImageButton imgButton = e.Row.FindControl("btnActas") as ImageButton;

                    Label lbl_Actas = e.Row.FindControl("lbl_Actas") as Label;

                    if (imgButton != null)
                    {
                        if (e.Row.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpediente, "vExisteActa")].Text.Trim() == "1")
                        {
                            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() != Constantes.CONST_OFICINACONSULAR_LIMA.ToString())
                            {
                                if (Convert.ToInt32(e.Row.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpediente, "sActaJudicialEstadoId")].Text) == (Int32)Enumerador.enmJudicialActaEstado.OBSERVADO)
                                {
                                    imgButton.Visible = true;
                                    lbl_Actas.Visible = false;
                                    lbl_Actas.Text = "SI";
                                    imgButton.ImageUrl = "~/Images/buscar-acta.png";
                                    //------------------------------------------------------------
                                    //Fecha: 08/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Al adicionar el campo vSiglas (Consulado) se incremento el indice en 1.
                                    //------------------------------------------------------------
                                    gdvExpediente.Columns[12].HeaderText = "Actas Observadas";
                                    //------------------------------------------------------------
                                    return;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(e.Row.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpediente, "sActaJudicialEstadoId")].Text) == (Int32)Enumerador.enmJudicialActaEstado.ENVIADO)
                                {
                                    //------------------------------------------------------------
                                    //Fecha: 08/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Al adicionar el campo vSiglas (Consulado) se incremento el indice en 1.
                                    //------------------------------------------------------------
                                    gdvExpediente.Columns[12].HeaderText = "Actas";
                                    //------------------------------------------------------------
                                    imgButton.Visible = true;
                                    lbl_Actas.Visible = false;
                                    lbl_Actas.Text = "NO";
                                    imgButton.ImageUrl = "~/Images/img_16_tramite_aprobar.png";
                                    return;
                                }
                            }

                        }

                        imgButton.Visible = false;
                        
                    }
                }                
            }
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;


            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnEditar };
                Comun.ModoLectura(ref arrImageButtons);
            }
        }

        //------------------------------------------
        //Fecha: 11/07/2019
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