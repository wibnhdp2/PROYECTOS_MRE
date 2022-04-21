using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Drawing;
using System.Web;
using System.Globalization;
using System.Net;
using System.IO;
using iTextSharp.text.pdf;
using Microsoft.Security.Application;
using SGAC.Registro.Persona.BL;


namespace SGAC.WebApp.Registro
{
    public partial class FrmJudicial : MyBasePage
    {
        private string strMensajeActaNoEnviada = "No se pudo enviar el Acta";
        private string strMensajeActaEnviada = "El acta se envió con éxito";
        private string strMensajeExpedienteCierre = "El expediente se cerró con éxito";

        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginadorActuacion.PageSize = Constantes.CONST_PAGE_SIZE_NORIFICACIONES;
            ctrlPaginadorActuacion.Visible = false;
            ctrlPaginadorActuacion.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ctrlToolBar.VisibleIButtonGrabar = true;
                ctrlToolBar.VisibleIButtonCancelar = true;
                ctrlToolBar.VisibleIButtonConfiguration = true;
                ctrlToolBar.VisibleIButtonSalir = true;
                ctrlToolBarNoti.VisibleIButtonNuevo = true;

                ctrlToolBar.btnConfiguration.CssClass = "btnNew";
                ctrlToolBar.btnConfiguration.Text = "     Enviar";
                ctrlToolBar.btnSalir.Width = 150;
                ctrlToolBar.btnSalir.Text = "     Cerrar Expediente";

                ctrlToolBar.btnConfiguration.OnClientClick = "return ValidarEnviar();";
                ctrlToolBar.btnSalir.OnClientClick = "return ValidarCerrar();";
                ctrlToolBar.btnCancelar.OnClientClick = "return ValidarEnvioExpediente();";

                ctrlToolBarNoti.VisibleIButtonGrabar = true;
                ctrlToolBarNoti.VisibleIButtonCancelar = true;
                ctrlToolBarNoti.VisibleIButtonConfiguration = true;
                ctrlToolBarNoti.btnConfiguration.OnClientClick = "return ValidarFinalizar();";
                ctrlToolBarNoti.btnGrabar.OnClientClick = "return ValidarActualizacionNotificacion();";
                ctrlToolBarNoti.btnNuevo.OnClientClick = "return LimpiarNotificacionEnvio();";

                ctrlToolBarActa.VisibleIButtonPrint = true;

                ctrlToolBarActa.VisibleIButtonGrabar = true;
                ctrlToolBarActa.VisibleIButtonCancelar = true;
                ctrlToolBarActa.VisibleIButtonConfiguration = true;

                ctrlToolBarActa.btnImprimir.Text = "  Enviar";
                ctrlToolBarActa.btnImprimir.CssClass = "btnEnviar";

                ctrlToolBarActa.btnConfiguration.CssClass = "btnNew";

                ctrlToolBarActa.btnConfiguration.Text = "     Actuación";

                ctrlToolBarActa.VisibleIButtonSalir = true;
                ctrlToolBarActa.btnSalir.CssClass = "btnNew";
                ctrlToolBarActa.btnSalir.Text = "     Finalizar";

                ctrlToolBarActa.btnGrabar.OnClientClick = "return ValidarActasGrabar();";
                ctrlToolBarActa.btnSalir.OnClientClick = "return ValidarFinalizarActas();";


                ctrlToolBar.btnEditarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonEditarClick(MyControl_btnEditar);
                ctrlToolBar.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(MyControl_btnGrabar);
                ctrlToolBar.btnCancelarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(MyControl_btnCancelar);
                ctrlToolBar.btnConfigurationHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonConfigurationClick(MyControl_btnConfiguration);
                ctrlToolBar.btnSalirHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonSalirClick(MyControl_btnSalirHandler);

                ctrlToolBarNoti.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolBarNoti_btnGrabarHandler);
                ctrlToolBarNoti.btnCancelarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarNoti_btnCancelar);
                ctrlToolBarNoti.btnConfigurationHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonConfigurationClick(ctrlToolBarNoti_btnConfiguration);
                ctrlToolBarNoti.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonNuevoClick(ctrlToolBarNoti_btnNuevoHandler);


                ctrlToolBarActa.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolBarActa_btnGrabarHandler);
                ctrlToolBarActa.btnCancelarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarActa_btnCancelarHandler);
                ctrlToolBarActa.btnConfigurationHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonConfigurationClick(ctrlToolBarActa_btnConfigurationHandler);
                ctrlToolBarActa.btnPrintHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonPrintClick(ctrlToolBarActa_btnPrintHandler);


                ctrlToolBarActa.btnSalirHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonSalirClick(ctrlToolBarActa_btnSalirHandler);

                this.txtFchRecepcion.StartDate = new DateTime(1900, 1, 1);
                this.txtFchRecepcion.EndDate = ObtenerFechaActual(HttpContext.Current.Session);

                this.txtFchAudiencia.StartDate = new DateTime(1900, 1, 1);
                this.txtFchAudiencia.AllowFutureDate = true;

                this.txtActaFecha.StartDate = ObtenerFechaActual(HttpContext.Current.Session);
                this.txtActaFecha.AllowFutureDate = true;

                this.txtFchValDip.StartDate = new DateTime(1900, 1, 1);
                //------------------------------------------------------------
                //Fecha: 18/01/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Habilitar fecha limite final a la fecha actual.
                //------------------------------------------------------------                                
                this.txtFchValDip.AllowFutureDate = true;

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.WARNING);

                if (!Page.IsPostBack)
                {
                    string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
                    if (Convert.ToInt64(codPersona) > 0)
                    {
                        //------------------------------------------------------
                        //Fecha: 19/10/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Obtener el tipo y numero de documento
                        //------------------------------------------------------
                        string codTipoDoc = "";
                        string codNroDocumento = "";
                        if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                        {
                            codTipoDoc = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString()));
                            codNroDocumento = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString()));
                        }
                        if (codTipoDoc.Length > 0 && codNroDocumento.Length > 0)
                        {
                            GetDataPersona(Convert.ToInt64(codPersona), Convert.ToInt16(codTipoDoc), codNroDocumento);
                        }
                        else
                        {
                            GetDataPersona(Convert.ToInt64(codPersona));
                        }
                    }
                   
                    Session["intParticipaPersonaId"] = 0;
                    if (Session["strBusqueda"] != null)
                    {
                        Session.Remove("strBusqueda");
                    }

                    gdvExpNotificados.DataSource = new DataTable();
                    gdvExpNotificados.DataBind();

                    gdvPagos.DataSource = new DataTable();
                    gdvPagos.DataBind();

                    ctrlToolBarNoti.btnConfiguration.Text = "     Finalizar";
                    ctrlToolBarActa.VisibleIButtonSalir = true;

                    Comun.EjecutarScript(this, Util.DeshabilitarTab(2) + Util.DeshabilitarTab(1) + Util.MoverTab(0));

                    int iActoJudicialId = 0;
                    int iAccionBoton = 0;

                    Session["ActoJudicialEstadoId"] = null;
                    Session["dtTmpPartipante"] = "";                  // DATA TABLE PARA ALMACENAR LA LISTA DE PARTICIPANTES
                    Session["dtTmpPartipantesEliminados"] = null;     // DATA TABLE PARA ALMACENAR LA LISTA DE PARTICIPANTES ELIMINADOS
                    Session["dtTmpNotificacionesEliminadas"] = null;  // DATA TABLE PARA ALMACENAR LA LISTA DE NOTIFICACIONES ELIMINADOS
                    Session["dtTmpNotificacion"] = "";                // DATA TABLE PARA ALMACENAR LA LISTA DE NOTIFICACIONES
                    Session["dtTmpActa"] = "";                        // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS
                    ViewState["intTipoPersona"] = "";                   // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS
                    ViewState["intPersonaId"] = "";                     // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS
                    Session["intActoJudicialId"] = "";                // ALMACENAMOS EL ID 
                    Session["intActuacionId"] = "";                   // ALMACENAMOS EL ID 

                    string strValor = Convert.ToString(Session["sActoJudicialId"]);
                    string[] strDato = strValor.Split('-');


                    if (strValor != "")
                    {
                        iActoJudicialId = Convert.ToInt32(strDato[0]);
                        iAccionBoton = Convert.ToInt32(strDato[1]);
                        Session["IQueHace"] = iAccionBoton;
                        Session["intActoJudicialId"] = iActoJudicialId;
                    }
                    else
                    {
                        iActoJudicialId = 5;
                        iAccionBoton = 3;
                        Session["IQueHace"] = iAccionBoton;
                    }

                    ctrlOficinaConsular1.Cargar();
                    ctrlOficinaConsular1.ddlOficinaConsular.AutoPostBack = true;
                    ctrlOficinaConsular1.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

                    CargarCombos();

                    Session["iActoJudicialId"] = iActoJudicialId;

                    if ((iAccionBoton == 2) || (iAccionBoton == 3))
                    {
                        MostrarActoJudicial();
                        if (iAccionBoton == 2)
                        {
                            Bloquea(true);
                            if (ddlTipoNotifica.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoNotificacion.NOTICACION_JUDICIAL).ToString())
                            {
                                ddlEntSoli.Enabled = false;
                            }
                            Session["intQueHace_Participante"] = 1;
                        }
                        activarToolBar();
                    }
                    else
                    {

                        ViewState["intTipoPersona"] = strDato[2];                   // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS
                        ViewState["intPersonaId"] = strDato[3];                     // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS

                        Nuevo();
                    }


                    if (Session["ActoJudicialEstadoId"] != null)
                    {
                        if (Session["ActoJudicialEstadoId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialExpedienteEstado.ENVIADO).ToString())
                        {
                            gdvExpNotificados.Columns[8].Visible = false;
                            gdvExpNotificados.Columns[9].Visible = false;
                        }

                    }

                }

                Comun.EjecutarScript(this, "EventosControles();");


                if (Session["iCargarTabActas"] != null && Session["iActoJudicialParticipanteId"] != null)
                {
                    if (Session["iCargarTabActas"].ToString() == "1")
                    {
                        foreach (GridViewRow gvr in gdvExpNotificados.Rows)
                        {
                            if (gvr.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpNotificados, "ajpa_iActoJudicialParticipanteId")].Text ==
                                Session["iActoJudicialParticipanteId"].ToString())
                            {
                                ImageButton imgButton = gvr.FindControl("btnNotificar") as ImageButton;
                                if (imgButton != null)
                                {
                                    GridViewCommandEventArgs gvcea = new GridViewCommandEventArgs(imgButton, new CommandEventArgs("Notificar", gvr.RowIndex));
                                    gdvExpNotificados_RowCommand(imgButton, gvcea);
                                }

                            }
                        }

                    }

                }


                if (chkGratuito.Checked)
                {
                    txtMotivoGratuidad.Visible = true;
                    lblTituloMotivoGratuito.Visible = true;
                }
                else
                {
                    txtMotivoGratuidad.Visible = false;
                    lblTituloMotivoGratuito.Visible = false;
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

        void ctrlToolBarActa_btnPrintHandler()
        {
            int index = -1;
            DataTable dt = (DataTable)Session["dtTmpActa"];
            Int64 iActaJudicialId = -1;
            Int16 sEstadoId = -1;
            Int16 sGuardado = -1;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["acjd_sEstadoId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialActaEstado.REGISTRADO).ToString())
                {
                    iActaJudicialId = Convert.ToInt64(dr["acjd_iActaJudicialId"]);
                    sGuardado = Convert.ToInt16(dr["acjd_sGuardado"]);
                    sEstadoId = Convert.ToInt16(Enumerador.enmJudicialActaEstado.REGISTRADO);
                    break;
                }
            }



            if (iActaJudicialId == -1)
            {
                return;
            }

            if (sGuardado == 0)
            {
                Validation3.MostrarValidacion("Se requiere grabar el acta antes de ejecutar esta acción", true, Enumerador.enmTipoMensaje.WARNING);
                updActas.Update();
                return;
            }


            EnviarActas(iActaJudicialId);
            BlanqueaActas();
            Int64 iActoJudicialNotificacionId = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);
            CargarActas(iActoJudicialNotificacionId);
            BloquearControlesActa(false);
        }

        void ctrlToolBarNoti_btnNuevoHandler()
        {

            DataTable dtNotificacion = new DataTable();
            bool bError = false;
            string strMensaje = string.Empty;


            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            if (dtNotificacion.Rows.Count > 0)                                     // SI EL NUMERO DE NOTIFICACIONES ES IGUAL A 0 ACTIVAMOS LOS BOTONES DE ADICIONAR Y LOS CAMPOS DE INGRESO DE NOTIFICACION
            {
                Int16 intParticipanteEstadoId = Convert.ToInt16(Session["ParticipanteEstadoId"]);

                if (dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO).ToString()).Length > 0 ||
                    dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR).ToString()).Length > 0 ||
                    dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO).ToString()).Length > 0 ||
                    dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA).ToString()).Length > 0 ||
                    dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON).ToString()).Length > 0)
                {
                    bError = true;
                    strMensaje = "No puede crear notificaciones. Existe una notificación recibida.";
                }
                else if (dtNotificacion.Select("ajno_sTipoRecepcionId is null").Length > 0 ||
                    dtNotificacion.Select("ajno_sTipoRecepcionId = 0").Length > 0)
                {
                    bError = true;
                    strMensaje = "No puede crear notificaciones. Existe una notificación por respuesta.";

                }
            }


            if (bError)
            {
                EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "NOTIFICACIÓN", strMensaje));
            }
            else
            {
                btnNotiCancelar_Click(null, null);
            }
        }



        void MyControl_btnSalirHandler()
        {
            if (ParticipanteActasCerradas() == true)
            {
                Int16 intEstadoExpedienteId = Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO);
                Int64 intActoJudicialId = Convert.ToInt64(Session["iActoJudicialId"]);

                if (ActualizarEstadoExpediente(intActoJudicialId, intEstadoExpedienteId) == true)
                {
                    ctrlToolBar.btnSalir.Enabled = false;
                    Validation1.MostrarValidacion(strMensajeExpedienteCierre, true, Enumerador.enmTipoMensaje.INFORMATION);
                    updNotificados.Update();
                }
            }
        }

        void MyControl_btnConfiguration()
        {
            int intResultado = 0;
            ActoJudicialMantenimientoBL funJudicial = new ActoJudicialMantenimientoBL();

            Int64 intActoJudicialId = Convert.ToInt64(Session["iActoJudicialId"]);
            Int16 intEstadoId = Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.ENVIADO);
            Int16 intUsuarioid = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            Int16 intOficinaId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string strHostname = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

            if (intOficinaConsularId == intOficinaConsularLimaId)
            {
                intResultado = funJudicial.ActualizarEstado(intActoJudicialId, intEstadoId, intUsuarioid, strDireccionIP, intOficinaId, strHostname);

                if (intResultado == 1)
                {
                    ctrlToolBar.btnConfiguration.Enabled = false;
                    Validation1.MostrarValidacion("El Expediente se envió con éxito, ya no podrá editar este Expediente", true, Enumerador.enmTipoMensaje.INFORMATION);
                    ctrlToolBar.btnGrabar.Enabled = false;

                    btnAceptarNotifica.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnAceptarPago.Enabled = false;
                    btn_CancelarPago.Enabled = false;

                    Session["IQueHace"] = 3;
                    MostrarActoJudicial();
                    Bloquea(false);

                    txtOrgano.Enabled = false;
                    gdvExpNotificados.Columns[8].Visible = false;
                    gdvExpNotificados.Columns[9].Visible = false;

                    gdvPagos.Columns[4].Visible = false;
                    gdvPagos.Columns[5].Visible = false;
                }
            }
            else
            {
                Validation1.MostrarValidacion("No puede ejecutar esta opción desde una Oficina Consular", true, Enumerador.enmTipoMensaje.WARNING);
            }

            updNotificados.Update();
        }

        void MyControl_btnEditar()
        {
            Session["IQueHace"] = 2;

            Blanquea();
            BlanqueaPersonasNotifica();
            BlanqueNotificaciones();
            BlanqueaActas();
            MostrarActoJudicial();
            Bloquea(true);
            updNotificados.Update();
        }

        void MyControl_btnGrabar()
        {
            string StrScript = string.Empty;
            try
            {
                if (GrabarPestana1())
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Exhortos Consulares", Constantes.CONST_MENSAJE_EXITO);

                    // SOLO VER
                    Session["IQueHace"] = 2;
                    Session["intQueHace_Participante"] = 3;

                    Comun.EjecutarScriptUpdatePanel(updNotificados, StrScript);
                    MostrarActoJudicial();
                    Bloquea(false);
                    BloquearDatosExpediente(true);
                    activarToolBar();
                    return;
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

        private void BloquearDatosExpediente(bool bAction)
        {
            ddlTipoNotifica.Enabled = bAction;

            txtNumExp.Enabled = bAction;
            txtNumOficio.Enabled = bAction;
            txtFchRecepcion.Enabled = bAction;
            txtFchAudiencia.Enabled = bAction;
            txtManteria.Enabled = bAction;

            txtObs.Enabled = bAction;

            if (ddlTipoNotifica.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoNotificacion.NOTICACION_ADMINISTRATIVA).ToString())
            {
                ddlEntSoli.Enabled = true;
                txtOrgano.Enabled = false;
            }
            else
            {
                ddlEntSoli.Enabled = false;
                txtOrgano.Enabled = true;
            }
        }

        void MyControl_btnCancelar()
        {
            string codPersona = Request.QueryString["CodPer"].ToString();

            if (Convert.ToInt16(Session["sDeDondeViene"]) == 1) { Response.Redirect("~/Consulta/FrmExpediente.aspx", false); }
            if (Convert.ToInt16(Session["sDeDondeViene"]) == 2)
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value, false);
                //}
                //else
                //{
                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
                }
                else
                { // PERSONA NATURAL
                    string codTipoDocEncriptada = "";
                    string codNroDocumentoEncriptada = "";
                    if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                    {
                        codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                        codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                    }
                    if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                    {
                        Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                    }
                    else
                    {
                        Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona, false);
                    }
                }
                
                //}
            }
            Session["sDeDondeViene"] = "";
        }

        bool VerificarGraboNotificacion()
        {
            bool booOk = false;

            DataTable dtNotificacion = new DataTable();
            int intFila = 0;


            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            booOk = true;
            if (dtNotificacion.Rows.Count != 0)
            {
                for (intFila = 0; intFila <= dtNotificacion.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt16(dtNotificacion.Rows[intFila]["ajno_sGuardado"].ToString()) == 0)
                    {
                        Validation2.MostrarValidacion("Se requiere grabar las notificación antes de ejecutar esta acción", true, Enumerador.enmTipoMensaje.WARNING);
                        updNotificaciones.Update();
                        booOk = false;
                        break;
                    }
                    booOk = true;
                }
            }
            return booOk;
        }

        bool VerificarGraboActa()
        {
            bool booOk = false;

            DataTable dtActa = new DataTable();
            int intFila = 0;

            dtActa = (DataTable)Session["dtTmpActa"];

            booOk = true;
            if (dtActa.Rows.Count != 0)
            {
                for (intFila = 0; intFila <= dtActa.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt16(dtActa.Rows[intFila]["acjd_sGuardado"].ToString()) == 0)
                    {
                        Validation2.MostrarValidacion("Se requiere grabar el Acta antes de ejecutar esta acción", true, Enumerador.enmTipoMensaje.WARNING);
                        updNotificaciones.Update();
                        booOk = false;
                        break;
                    }
                    booOk = true;
                }
            }
            return booOk;
        }

        void ctrlToolBarNoti_btnGrabarHandler()
        {
            if (GrabarNotificaciones() == true)
            {
                ctrlPaginadorActuacion.InicializarPaginador();
                cargarNotificaciones();

                modificar_Notificaciones();

                Session["NotificacionIdEditando"] = null;
            }
        }

        void ctrlToolBarNoti_btnCancelar()
        {
            ActivarNotificaControl();
            BlanqueNotificaciones();
            CrearTmpNotificaciones();

            DataTable dtTmpNotificaciones = new DataTable();

            if (Session["dtTmpNotificacion"] != null)
                dtTmpNotificaciones = (DataTable)Session["dtTmpNotificacion"];

            gdvNotificaciones.DataSource = dtTmpNotificaciones;

            gdvNotificaciones.DataBind();
            BlanqueNotificaciones();

            updNotificados.Update();

            Session["NotificacionIdEditando"] = null;

            Comun.EjecutarScriptUpdatePanel(updNotificaciones, Util.DeshabilitarTab(1) + Util.ActivarTab(0, "Expediente"));
            MostrarActoJudicial();
        }

        void ctrlToolBarNoti_btnConfiguration()
        {
            Int64 intParticipanteId = Convert.ToInt64(Session["iActoJudicialParticipanteId"]);
            Int16 intEstadoParticipante = Convert.ToInt16(Session["ParticipanteEstadoId"]);
            int intNotificacionGuardada = Convert.ToInt16(Session["NotificacionGuardada"]);
            DataTable dtPartipante = new DataTable();
            DataTable dtNotificaciones = new DataTable();

            // ACTUALIZAMOS EN EL DATATABLE DE PARTICIPANTES EL ESTADO DE PARTICIPNATE ACTUAL
            dtPartipante = (DataTable)Session["dtTmpPartipante"];

            if (Session["dtTmpNotificacion"] != null)
                dtNotificaciones = (DataTable)Session["dtTmpNotificacion"];

            if (intNotificacionGuardada == 0)
            {
                Validation2.MostrarValidacion("Hay Notificaciones pendientes de guardar, No puede cerrar las Notificaciones", true, Enumerador.enmTipoMensaje.WARNING);
                updNotificaciones.Update();
                return;
            }

            DateTime FechaInicio = ObtenerFechaActual(HttpContext.Current.Session);

            if (dtNotificaciones.Rows.Count != 0)
            {
                FechaInicio = Comun.FormatearFecha(dtNotificaciones.Rows[0]["ajno_dFechaHoraNotificacion"].ToString());
            }

            if (intEstadoParticipante != Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
            {
                bool booresultado = false;

                Int16 intEstadiParticipante = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO);
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                string strHostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                Int16 intUsuarioModifica = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                string strIpModifica = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                // VALIDAMOS QUE HAYA NOTIFICACIONES QUE GUARDAR
                if (dtNotificaciones.Rows.Count == 0)
                {
                    Validation2.MostrarValidacion("No hay Notificaciones para este participante, debe agregar al menos una notificación", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificaciones.Update();
                    return;
                }

                ActoJudicialParticipanteConsultaBL funParticipa = new ActoJudicialParticipanteConsultaBL();
                booresultado = funParticipa.ActualizarEstadoParticipante(intParticipanteId, intEstadiParticipante, intUsuarioModifica, strIpModifica, intOficinaConsularId, strHostName);

                if (booresultado == true)
                {

                    int intFila = 0;
                    Validation2.MostrarValidacion("Se cerró el envío de notificaciones con éxito, no podrá volver a notificar a este participante", true, Enumerador.enmTipoMensaje.INFORMATION);
                    updNotificaciones.Update();



                    Session["ParticipanteEstadoId"] = intEstadiParticipante;                 // ACTUALIZAMOS EL ESTADO DEL PARTICIPANTE

                    for (intFila = 0; intFila <= dtPartipante.Rows.Count - 1; intFila++)
                    {
                        if (Convert.ToInt64(dtPartipante.Rows[intFila]["ajpa_iActoJudicialParticipanteId"].ToString()) == intParticipanteId)
                        {
                            dtPartipante.Rows[intFila]["ajpa_sEstadoId"] = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO);
                        }
                    }

                    Session["dtTmpPartipante"] = dtPartipante;

                    gdvExpNotificados.DataSource = dtPartipante;
                    gdvExpNotificados.DataBind();

                    DataTable dtTmpNotificacion = new DataTable();

                    if (Session["dtTmpNotificacion"] != null)
                        dtTmpNotificacion = (DataTable)Session["dtTmpNotificacion"];

                    gdvNotificaciones.DataSource = dtTmpNotificacion;
                    gdvNotificaciones.DataBind();

                    ctrlToolBarNoti.btnConfiguration.Text = "     Actas";

                    Notificar(Convert.ToInt32(intParticipanteId), lblNombreNotificado.Text, updNotificaciones);
                    return;
                }
                else
                {
                    Validation2.MostrarValidacion("No se pudo cerrar el envio de notificaciones", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificaciones.Update();
                    return;
                }
            }
            else
            {
                Int16 intEstadiParticipante = Convert.ToInt16(Session["ParticipanteEstadoId"]);

                if (dtNotificaciones.Rows.Count == 0)
                {
                    Validation2.MostrarValidacion("Debe de ingresar al menos una Notificación para generar un acta", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificaciones.Update();
                    return;
                }

                //  SI EL ESTADO DEL PARTICIPANTE ES CERRADO, QUIERE DECIR QUE AL PARTICIPANTE SE LE CERRARON LAS NOTIFICACIONES
                if (intEstadiParticipante == Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                {
                    // OBTENEMOS EL ULTIMO ID DE LA NOTIFICACION Y CON ESE GENERAMO EL ACTA,
                    // ESTO ES POR MIENTRAS HASTA QUE SE DEFINA LA NUEVA ESTRUCTURA DE LA TABLA
                    Session["iActoJudicialNotificacionId"] = dtNotificaciones.Rows[0]["ajno_iActoJudicialNotificacionId"].ToString();

                    Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

                    GenerarActas();


                    // SI LA OFICINA CONSULAR ES LIMA
                    if (intOficinaConsularId == intOficinaConsularLimaId)
                    {
                        // DESABILITAMOS EL BOTON ADICIONAR 
                        btnActaAceptar.Enabled = false;
                        ddlActaTipo.Enabled = false;
                        // OCULTAMOS LAS COLUMAS DEL GRID DE ACTAS
                        gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = false;
                        gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = false;
                        gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = false;
                        // SI EL ESTADO DE ACTAS DEL PARTICIPANTE ES =  0  INDICA QUE LAS ACTAS ESTAN CERRADAS, Y OCULTAMOS LA COLUMNA DE OBSERVACIONES
                        if (HallarParticipanteEstadoActa(intParticipanteId) == 0)
                        {
                            ctrlToolBarActa.btnSalir.Enabled = false;
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                        }
                        else
                        {
                            ctrlToolBarActa.btnSalir.Enabled = true;
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = true;
                        }
                    }
                    else
                    {
                        // DESABILITAMOS EL BOTON FINALIZAR
                        ctrlToolBarActa.btnSalir.Enabled = false;

                        /*Verificando si las actas estan obervadas*/
                        DataTable dt_acta = (DataTable)Session["dtTmpActa"];
                        DataTable dt_acta_observada = null;
                        #region - Validar si esta observado -

                        try
                        {
                            dt_acta_observada = (from dt in dt_acta.AsEnumerable()
                                                 where Convert.ToInt32(dt["acjd_sEstadoId"]) != (Int32)Enumerador.enmJudicialActaEstado.OBSERVADO
                                                 select dt).CopyToDataTable();
                        }
                        catch
                        {
                            dt_acta_observada = new DataTable();
                        }

                        if (dt_acta_observada.Rows.Count > 0)
                        {
                            ddlActaTipo.Enabled = false;
                            txtActaFecha.Enabled = false;
                            //txtActaHora.ReadOnly = true;
                            txtActaHora.Enabled = false;
                            //ddlActaEstado.Enabled = true;
                            //txtActaResponsable.ReadOnly = true;
                            //txtActaCuerpo.ReadOnly = true;
                            //txtResultado.ReadOnly = true;
                            //txtActaObservacion.ReadOnly = true;
                            txtActaResponsable.Enabled = false;
                            txtActaCuerpo.Enabled = false;
                            
                            //txtResultado.Enabled = false;
                            
                            txtActaObservacion.Enabled = false;

                            btnActaAceptar.Enabled = false;
                            btnActaCancelar.Enabled = false;
                            ctrlToolBarActa.btnGrabar.Enabled = false;

                            // Validation3.MostrarValidacion("No se puede registrar Actas mientras esté en Lima", true, Enumerador.enmTipoMensaje.WARNING);

                        }
                        else
                        {
                            ctrlToolBarActa.btnGrabar.Enabled = true;
                        }
                        #endregion


                        // SI LA OFICINA ES UN CONSULADO
                        // SI EL ESTADO DE ACTAS DEL PARTICIPANTE ES =  0  INDICA QUE LAS ACTAS ESTAN CERRADAS, Y OCULTAMOS LA COLUMNA DE OBSERVACIONES
                        if (HallarParticipanteEstadoActa(intParticipanteId) == 0)
                        {
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = false;
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = false;
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = false;
                            gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;

                            // DESACTIVAMOS EL BOTON ADICIONAR
                            btnActaAceptar.Enabled = false;

                        }
                        else
                        {
                            // SI NO ESTA CERRADO PREGUNTAMOS SI ESTA EN MODO DE CONSULTA
                            if (Convert.ToInt16(Session["IQueHace"]) == 3)
                            {
                                // SI ES MODO CONULTA OCULTAMOS LAS COLUMNAS DEL GRID Y DESABILITAMOS EL BOTON AGREGAR
                                // MOSTRAMOS LAS COLUMNAS
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = false;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = true;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = false;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                                // ACTIVAMOS EL BOTON ADICIONAR
                                btnActaAceptar.Enabled = false;
                            }
                            else
                            {
                                // SI ES DIFERENTE A CONSULTA MOSTRAMOS LAS COLUMAS Y ACTIVAMOS EL BOTON AGREGAR
                                // MOSTRAMOS LAS COLUMNAS
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = true;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = true;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = true;
                                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                            }
                        }
                    }

                    updActas.Update();
                }
            }
        }

        void ctrlToolBarActa_btnGrabarHandler()
        {

            if (GrabarActas() == true)
            {
                Int64 iActoJudicialNotificacionId;
                BlanqueaActas();
                iActoJudicialNotificacionId = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);
                CargarActas(iActoJudicialNotificacionId);

                Session["iActaJudicialId"] = null;

            }
        }

        void ctrlToolBarActa_btnCancelarHandler()
        {
            ActivarActasControl();
            BlanqueaActas();
            crearTmpActas();

            DataTable dtTmpActas = (DataTable)Session["dtTmpActas"];

            gdvActaDiligenciamiento.DataSource = dtTmpActas;
            gdvActaDiligenciamiento.DataBind();

            updActas.Update();

            Comun.EjecutarScriptUpdatePanel(updActas, Util.DeshabilitarTab(2) + Util.ActivarTab(1, "Notificaciones"));
        }

        void ctrlToolBarActa_btnConfigurationHandler()
        {
            DataTable dtActa = new DataTable();

            dtActa = (DataTable)Session["dtTmpActa"];

            if (dtActa.Rows.Count == 0)
            {
                Validation3.MostrarValidacion("No se agregó ninguna Acta de diligenciamiento, Debe ingresar al menos un Acta", true, Enumerador.enmTipoMensaje.WARNING);
                updActas.Update();
                return;
            }

            if (VerificarGraboActa() == false) { return; }

            #region  IDM - 20150308 - Actualizar correlativos y estado
            ActoJudicialMantenimientoBL objBL = new ActoJudicialMantenimientoBL();
            int intResultado = objBL.ActualizarCorrelativoActoJudicial(
                Convert.ToInt64(Session["iActuacionDetalleId"]),
                Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                Convert.ToInt16(Session["iTarifarioId"]),
                Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]));

            if (intResultado == -1)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTO JUDICIAL", "No se pudo generar la actuación.");
                Comun.EjecutarScriptUpdatePanel(updNotificaciones, strScript);
                return;
            }
            #endregion


            string strDescripcionTarifa = Convert.ToString(Session["DescTarifa"]);
            string strFechaActuacion = Convert.ToString(Session["LblFecha"]);

            Session["IntTarifarioId"] = Session["iTarifarioId"];
            Session["DescTarifa"] = strDescripcionTarifa;
            Session["LblFecha"] = strFechaActuacion;

            Session["strActo"] = "Judicial";
            Session["iOficinaConsular"] = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID];
            Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = Session["iActuacionDetalleId"];
            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = Convert.ToInt64(Session["intActuacionId"]);

            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmActoGeneral.aspx?vClass=1&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";
                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/FrmActoGeneral.aspx?vClass=1&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmActoGeneral.aspx?vClass=1&CodPer=" + codPersona, false);
                }
            }
            
        }

        void ctrlToolBarActa_btnSalirHandler()
        {
            DataTable dtActas = new DataTable();
            int intNumeroRegistros = 0;
            int intFila = 0;
            Int64 intParticipanteId = Convert.ToInt64(Session["iActoJudicialParticipanteId"]);

            dtActas = (DataTable)Session["dtTmpActa"];
            intNumeroRegistros = dtActas.Rows.Count;


            // AVEDRIGUAMOS SI LA ULTIMA ACTA INGRESA ESTA OBSERVADA,  SI ESTA OBSERVADA CANCELAMOS EL PROCESO DE CIERRE
            if (dtActas.Rows.Count != 0)
            {
                if (Convert.ToInt16(dtActas.Rows[dtActas.Rows.Count - 1]["acjd_sEstadoId"].ToString()) == Convert.ToInt16(Enumerador.enmJudicialActaEstado.OBSERVADO))
                {
                    Validation3.MostrarValidacion("Se ha detectado que la última acta registrada ha sido observada, no se puede cerrar las Actas", true, Enumerador.enmTipoMensaje.INFORMATION);
                    updActas.Update();
                    return;
                }
            }


            if (intNumeroRegistros == 0)
            {
                Validation3.MostrarValidacion("debe de registrar al menos un acta de Diligenciamiento para poder cerrar el ingreso de actas", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();
                return;
            }

            //VERIFICAMOS QUE LAS ACTAS HAYAN SIDO ENVIADAS, SI ALGUNA DE LAS ACTAS NO ESTA ENVIADA, SE CANCELA EL PROCESO DE CIERRE DE ACTAS
            for (intFila = 0; intFila <= dtActas.Rows.Count - 1; intFila++)
            {
                if (Convert.ToInt16(dtActas.Rows[intFila]["acjd_sEstadoId"].ToString()) == Convert.ToInt16(Enumerador.enmJudicialActaEstado.REGISTRADO))
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('No se han enviado todas las actas, Debe de enviar todas las actas para poder cerrar ');", true);
                    return;
                }
            }

            // CERRAMOS LAS ACTAS PARA EL PARTICIPANTE ACTUAL
            if (CerrarActas(intParticipanteId) == false)
            {
                Validation3.MostrarValidacion("No se pudo cerrar las Actas para este participante", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();
                return;
            }
            else
            {
                Validation3.MostrarValidacion("Se cerraron las Actas con éxito, No podrá agregar ni editar actas para este Participante", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();

                if (ParticipanteActasCerradas() == true)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Se han cerrado las actas de todos los participantes, Ya puede cerrar el Expediente');", true);
                    ctrlToolBar.btnSalir.Enabled = true;
                    updNotificados.Update();
                }


                ActoJudicialConsultaBL CL_ActoJudicial = new ActoJudicialConsultaBL();
                BE.RE_ACTOJUDICIAL BE_ActoJudicial = new BE.RE_ACTOJUDICIAL();
                Int64 iActoJudicialId = Convert.ToInt64(Session["intActoJudicialId"]);


                BE_ActoJudicial = CL_ActoJudicial.Obtener(iActoJudicialId);

                CargarParticipantes(BE_ActoJudicial);

                if (HallarParticipanteEstadoActa(intParticipanteId) == 0)
                {
                    ctrlToolBarActa.btnSalir.Enabled = false;
                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                }
                else
                {
                    ctrlToolBarActa.btnSalir.Enabled = true;
                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = true;
                }
                return;
            }


        }

        bool CerrarExpediente(Int64 intExpedienteId)
        {
            bool booResult = false;


            return booResult;
        }

        bool ParticipanteActasCerradas()
        {
            bool booResult = false;

            Int64 intActoJudicialid = Convert.ToInt64(Session["intActoJudicialId"]);
            SGAC.Registro.Actuacion.BL.ActoJudicialParticipanteConsultaBL miFun = new SGAC.Registro.Actuacion.BL.ActoJudicialParticipanteConsultaBL();

            booResult = miFun.ParticipanteActasCerradas(intActoJudicialid);
            return booResult;
        }

        bool CerrarActas(Int64 intParticipanteId)
        {
            bool booResult = false;
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);
            string strHostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
            Int16 intUsuarioModifica = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string strIpModifica = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);


            ActoJudicialParticipanteConsultaBL funParticipante = new ActoJudicialParticipanteConsultaBL();
            // false = se cierra el ingreso de actas para el participante
            booResult = funParticipante.ActualizarEstadoActa(intParticipanteId, false, intUsuarioModifica, strIpModifica, intOficinaConsularId, strHostName);
            return booResult;
        }

        protected void btnNotiAceptar_Click(object sender, EventArgs e)
        {

            Int16 intNotificacionIdEstado = 0;
            DataTable dtTmpNotificacion = new DataTable();

            //DateTime datFchRecepcion = txtFchRecepcion.Value();                                                                 // FECHA DE RECEPCION DEL EXPEDIENTE JUDICIAL
            DateTime datFchNotificacionNoti = Comun.FormatearFecha(txtFechaNotifica.Value().ToString().Substring(0, 10) + " " + txtHoraNotifica.Text + ":00");      // FECHA DE LA NOTIFICACION

            DateTime? datFchRecepcionNoti = null;                                                                               // FECHA DE RECEPCION DE LA NOTIFICACION
            string strEstadoDescripcion = "";

            Session["NotificacionGuardada"] = 0;

            if (txtFchRecep.Text != "")
            {
                datFchRecepcionNoti = Comun.FormatearFecha(txtFchRecep.Value().ToString().Substring(0, 10) + " " + txtHoraRecep.Text);
            }

            int sCorrelativo = 0;
            int intFila = 0;
            //------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
            //           y la fecha de salida de valija
            // Fecha: 13/01/2017
            //------------------------------------------------------------------
            //if (datFchNotificacionNoti <= datFchRecepcion)                                                                      // VALIDAMOS QUE LA FECHA DE RECEPCION DE LA NOTIFICACION NO SEA MAYOR A LA FECHA DE RECEPCION DEL EXPEDIENTE JUDICIAL
            //{
            //    Validation2.MostrarValidacion("La fecha de Notificación no puede ser menor o igual a la fecha de Recepción del Expediente", true, Enumerador.enmTipoMensaje.WARNING);
            //    updNotificaciones.Update();
            //    return;
            //}
            //------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
            //           y la fecha de salida de valija
            // Fecha: 13/01/2017
            //------------------------------------------------------------------
            //if (datFchRecepcionNoti != null)
            //{
            //    if (datFchRecepcionNoti <= datFchRecepcion)                                                                     // VALIDAMOS QUE LA FECHA DE RECEPCION DE LA NOTIFICACION NO SEA MENOR A LA FECHA DE RECEPCION DEL EXPEDIENTE JUDICIAL
            //    {
            //        Validation2.MostrarValidacion("La fecha de Recepción de la notificación no puede ser menor o igual a la fecha de Recepción del Expediente", true, Enumerador.enmTipoMensaje.WARNING);
            //        updNotificaciones.Update();
            //        return;
            //    }
            //}

            dtTmpNotificacion = (DataTable)Session["dtTmpNotificacion"];


            if (btnNotiAceptar.Text == "Adicionar")
            {
                int intNewIdNotificacion = 0;


                intNotificacionIdEstado = Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.REGISTRADO);


                int intNumRegistro = dtTmpNotificacion.Rows.Count - 1;



                if (dtTmpNotificacion.Rows.Count != 0)
                {
                    bool bNotificacionCompletada = false;
                    if (dtTmpNotificacion.Rows[intNumRegistro]["ajno_sTipoRecepcionId"] != null)
                    {
                        if (dtTmpNotificacion.Rows[intNumRegistro]["ajno_sTipoRecepcionId"].ToString().Trim() != string.Empty)
                        {
                            if (Convert.ToInt32(dtTmpNotificacion.Rows[intNumRegistro]["ajno_sTipoRecepcionId"].ToString()) != 0)
                            {
                                bNotificacionCompletada = true;
                            }
                        }
                    }

                    if (!bNotificacionCompletada)
                    {
                        Validation2.MostrarValidacion("No puede generar una nueva Notificación hasta que la anterior no haya sido completada", true, Enumerador.enmTipoMensaje.WARNING);
                        updNotificaciones.Update();
                        return;
                    }

                    for (intFila = 0; intFila <= dtTmpNotificacion.Rows.Count - 1; intFila++)
                    {
                        sCorrelativo = sCorrelativo + 1;
                    }
                    sCorrelativo = sCorrelativo + 1;

                    DataView dvResult = dtTmpNotificacion.DefaultView;
                    dvResult.Sort = "ajno_iActoJudicialNotificacionId ASC";
                    dtTmpNotificacion = dvResult.Table;

                    intNewIdNotificacion = (Convert.ToInt32(dvResult[0]["ajno_iActoJudicialNotificacionId"].ToString()) - 1);
                    if (intNewIdNotificacion >= 0)
                    {
                        intNewIdNotificacion = -1;
                    }
                }
                else
                {
                    sCorrelativo = 1;
                    intNewIdNotificacion = -1;
                }

                DataRow row;
                row = dtTmpNotificacion.NewRow();

                strEstadoDescripcion = "REGISTRADO";
                int intParticipanteId = Convert.ToInt32(Session["intParticipanteid"]);                  // ID DEL PARTICIPANTE

                row["ajno_iActoJudicialNotificacionId"] = intNewIdNotificacion;
                row["ajno_iActoJudicialParticipanteId"] = intParticipanteId;
                row["ajno_sTipoRecepcionId"] = ddlTipoRecepcion.SelectedValue;
                row["ajno_sViaEnvioId"] = ddlViaEnvio.SelectedValue;
                row["ajno_vEmpresaServicioPostal"] = txtEmpPostal.Text;
                row["ajno_vPersonaNotificacion"] = txtPersNotifica.Text;
                row["ajno_dFechaHoraNotificacion"] = datFchNotificacionNoti;
                row["ajno_vNumeroCedula"] = txtNroCedula.Text;
                row["ajno_vPersonaRecibeNotificacion"] = txtPerRecep.Text;

                if (txtFchRecep.Text != "") { row["ajno_dFechaHoraRecepcion"] = datFchRecepcionNoti; }

                row["ajno_vCuerpoNotificacion"] = txtNotificacionCuerpo.Text;
                row["ajno_vObservaciones"] = txtNotiObservacion.Text;
                row["ajno_sEstadoId"] = intNotificacionIdEstado;                                              // INDICAMOS EL ESTADO DEL REGISTRO
                row["ajno_vViaEnvio"] = ddlViaEnvio.SelectedItem.Text;
                row["ajno_sCorrelativo"] = sCorrelativo;

                row["ajno_vEstadoDescripcion"] = strEstadoDescripcion;
                row["ajno_sGuardado"] = 0;                                                                    // INDICAMOS QUE ESTE REGISTRO REQUIERE DE SER GUARDADO                        
                row["ajno_vEstadoInicial"] = "ENVIADO";

                dtTmpNotificacion.Rows.Add(row);
            }
            else
            {
                if (ddlTipoRecepcion.SelectedValue != "")
                {
                    if ((ddlTipoRecepcion.SelectedValue == ((int)Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO).ToString()) ||
                        (ddlTipoRecepcion.SelectedValue == ((int)Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR).ToString()) ||
                        (ddlTipoRecepcion.SelectedValue == ((int)Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO).ToString()) ||
                        (ddlTipoRecepcion.SelectedValue == ((int)Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA).ToString()) ||
                        (ddlTipoRecepcion.SelectedValue == ((int)Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON).ToString()))
                    {


                        DataTable dtNotificacion = new DataTable();

                        if (Session["dtTmpNotificacion"] != null)
                            dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

                        //if (dtNotificacion.Rows.Count > 0)                                     // SI EL NUMERO DE NOTIFICACIONES ES IGUAL A 0 ACTIVAMOS LOS BOTONES DE ADICIONAR Y LOS CAMPOS DE INGRESO DE NOTIFICACION
                        //{

                        //    if (dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO).ToString()).Length > 0 ||
                        //        dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR).ToString()).Length > 0 ||
                        //        dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO).ToString()).Length > 0 ||
                        //        dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA).ToString()).Length > 0 ||
                        //        dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON).ToString()).Length > 0)
                        //    {
                        //        EjecutarScript(Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "NOTIFICACIÓN", "No puede existir más de una notificación recibida."));
                        //        return;
                        //    }

                        //}

                        intNotificacionIdEstado = Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.NOTIFICACION_RECIBIDA);
                        strEstadoDescripcion = "NOTIFICACION RECIBIDA";
                        SetearEstadoNotificado(false);
                    }
                    else
                    {
                        intNotificacionIdEstado = Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.NOTIFICACION_NO_RECIBIDA);
                        strEstadoDescripcion = "NOTIFICACION NO RECIBIDA";
                        SetearEstadoNotificado(false);
                    }

                    ctrlToolBarNoti.btnConfiguration.Visible = true;
                }
                else
                {
                    intNotificacionIdEstado = Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.REGISTRADO);
                    strEstadoDescripcion = "REGISTRADO";
                }

                for (intFila = 0; intFila <= dtTmpNotificacion.Rows.Count - 1; intFila++)
                {
                    Int64 iActoJudicialNotificacionId = Convert.ToInt64(dtTmpNotificacion.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString());
                    Int64 iActoJudicialNotificacionIdActual = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);

                    if (iActoJudicialNotificacionId == iActoJudicialNotificacionIdActual)
                    {
                        if (ddlTipoRecepcion.SelectedValue.ToString() != "0")
                        {
                            dtTmpNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"] = ddlTipoRecepcion.SelectedValue;
                        }
                        else
                        {
                            dtTmpNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"] = DBNull.Value;
                        }

                        dtTmpNotificacion.Rows[intFila]["ajno_sViaEnvioId"] = ddlViaEnvio.SelectedValue;
                        dtTmpNotificacion.Rows[intFila]["ajno_vEmpresaServicioPostal"] = txtEmpPostal.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_vPersonaNotificacion"] = txtPersNotifica.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"] = datFchNotificacionNoti;
                        dtTmpNotificacion.Rows[intFila]["ajno_vNumeroCedula"] = txtNroCedula.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_vPersonaRecibeNotificacion"] = txtPerRecep.Text;

                        if (txtFchRecep.Text != string.Empty)
                            dtTmpNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"] = Comun.FormatearFecha(txtFchRecep.Value().ToString().Substring(0, 10) + " " + txtHoraRecep.Text + ":00");
                        else
                            dtTmpNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"] = DBNull.Value;

                        dtTmpNotificacion.Rows[intFila]["ajno_vCuerpoNotificacion"] = txtNotificacionCuerpo.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_vObservaciones"] = txtNotiObservacion.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_sEstadoId"] = intNotificacionIdEstado;                               // INDICAMOS EL ESTADO DEL REGISTRO
                        dtTmpNotificacion.Rows[intFila]["ajno_vViaEnvio"] = ddlViaEnvio.SelectedItem.Text;
                        dtTmpNotificacion.Rows[intFila]["ajno_vEstadoDescripcion"] = strEstadoDescripcion;
                        dtTmpNotificacion.Rows[intFila]["ajno_sGuardado"] = 0;                                                     // INDICAMOS QUE ESTE REGISTRO REQUIERE DE SER GUARDADO  
                        if (ddlTipoRecepcion.SelectedValue.ToString() != "0")
                            dtTmpNotificacion.Rows[intFila]["ajno_vTipRecepcion"] = ddlTipoRecepcion.SelectedItem.Text;
                        else
                            dtTmpNotificacion.Rows[intFila]["ajno_vTipRecepcion"] = DBNull.Value;
                    }
                }
            }

            Session["dtTmpNotificacion"] = dtTmpNotificacion;            // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvNotificaciones.DataSource = dtTmpNotificacion;

            gdvNotificaciones.DataBind();

            BlanqueNotificaciones();
            ActivarNotificaControl();


            botonAdicionar();
            Session["NotificacionIdEditando"] = null;

            btnNotiCancelar_Click(null, null);


            ddlViaEnvio.Enabled = false;
            txtEmpPostal.Enabled = false;
            txtPersNotifica.Enabled = false;
            txtFechaNotifica.Enabled = false;
            txtHoraNotifica.Enabled = false;
            txtNotificacionCuerpo.Enabled = false;

            updNotificaciones.Update();
        }

        private void SetearEstadoNotificado(bool bRecibido)
        {
            if (Session["iActoJudicialParticipanteId"] != null)
            {
                DataTable dtTmpPartcipa = (DataTable)Session["dtTmpPartipante"];
                foreach (DataRow dr in dtTmpPartcipa.Rows)
                {
                    if (dr["ajpa_iActoJudicialParticipanteId"].ToString() == Session["iActoJudicialParticipanteId"].ToString())
                    {
                        if (bRecibido)
                        {
                            dr["ajno_vEstadoPartipante"] = "NOTIFICADO";
                        }
                        else
                        {
                            dr["ajno_vEstadoPartipante"] = "SIN NOTIFICAR";
                        }

                        break;
                    }
                }

                gdvExpNotificados.DataSource = dtTmpPartcipa;
                gdvExpNotificados.DataBind();

                Session["dtTmpPartipante"] = dtTmpPartcipa;

                updNotificados.Update();

            }
        }

        protected void btnActaCancelar_Click(object sender, EventArgs e)
        {

            BlanqueaActas();
            BlanqueaPersonasNotifica();
            Session["iActaJudicialId"] = null;
            BloquearControlesActa(true);
            ddlActaTipo.Enabled = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            Session["intQueHace_Participante"] = 1;
            btnAceptarNotifica.Text = "Adicionar";

            ddlTipPersona3.Enabled = true;
            ddlTipPersona3.SelectedValue = "0";

            ddlTipDoc3.Enabled = true;
            ddlTipDoc3.SelectedValue = "0";

            txtNumDoc3.Enabled = true;
            txtNumDoc3.Text = string.Empty;


            txtFchValDip.Enabled = true;
            txtFchValDip.Text = string.Empty;

            txtNumHojRem.Enabled = true;
            txtNumHojRem.Text = string.Empty;

            ctrlOficinaConsular1.Enabled = true;
            ctrlOficinaConsular1.SelectedValue = "0";

            txtNom3.Text = string.Empty;
            txtApePat3.Text = string.Empty;
            txtApeMat3.Text = string.Empty;

            btnAceptarNotifica.Enabled = true;
        }

        protected void btnNotiCancelar_Click(object sender, EventArgs e)
        {
            BlanqueNotificaciones();
            ActivarNotificaControl();
            Session["NotificacionIdEditando"] = null;
        }

        protected void btnBuscarParticipante_Click(object sender, EventArgs e)
        {

        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        int HallarParticipanteEstadoActa(Int64 intParticipanteId)
        {
            int intResult = 0;
            DataTable dtParticipante = new DataTable();
            int intFila = 0;

            dtParticipante = (DataTable)Session["dtTmpPartipante"];

            if (dtParticipante.Rows.Count != 0)
            {
                for (intFila = 0; intFila <= dtParticipante.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt64(dtParticipante.Rows[intFila]["ajpa_iActoJudicialParticipanteId"].ToString()) == intParticipanteId)
                    {
                        intResult = Convert.ToInt32(dtParticipante.Rows[intFila]["ajpa_bActaFlag"]);
                        break;
                    }
                }
            }

            return intResult;
        }

        protected void gdvExpNotificados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);                             // VARIABLE QUE NOS INDICA EN QUE MODO SE ENCUENTRA EL FORMULARIO
            Int64 intPersonaId = 0;
            Int64 intEmpresaId = 0;

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

            int index = Convert.ToInt32(e.CommandArgument);
            int iActuacionDetalleId = 0;
            int iTarifariod = 0;
            string strDescripcionCorta = "";
            string strFechaActuacion = "";

            string strNombreParticipante = gdvExpNotificados.DataKeys[index].Values["ajpa_vNotificado"].ToString();

            Int16 sTipoDocumentoId = 0;

            Int32 intindiceparticipante = 0;
            intindiceparticipante = Convert.ToInt16(gdvExpNotificados.Rows[index].Cells[0].Text);


            if (!string.IsNullOrEmpty(gdvExpNotificados.DataKeys[index].Values["ajpa_sDocumentoTipoId"].ToString()))
                sTipoDocumentoId = Convert.ToInt16(gdvExpNotificados.DataKeys[index].Values["ajpa_sDocumentoTipoId"]);



            string vNumeroDocumento = string.Empty;

            if (!string.IsNullOrEmpty(gdvExpNotificados.DataKeys[index].Values["ajpa_vDocumentoNumero"].ToString()))
                vNumeroDocumento = gdvExpNotificados.DataKeys[index].Values["ajpa_vDocumentoNumero"].ToString();

            int intParticipanteId = Convert.ToInt32(gdvExpNotificados.DataKeys[index].Values["ajpa_iActoJudicialParticipanteId"].ToString());
            Int16 intParticipanteEstadoId = Convert.ToInt16(gdvExpNotificados.DataKeys[index].Values["ajpa_sEstadoId"].ToString());

            if (gdvExpNotificados.DataKeys[index].Values["ajpa_iActuacionDetalleId"].ToString() != "" && gdvExpNotificados.DataKeys[index].Values["ajpa_iActuacionDetalleId"].ToString() != "0")
            {
                iActuacionDetalleId = Convert.ToInt32(gdvExpNotificados.DataKeys[index].Values["ajpa_iActuacionDetalleId"].ToString());
                if (gdvExpNotificados.DataKeys[index].Values["acde_sTarifarioId"].ToString().Length > 0)
                {
                    iTarifariod = Convert.ToInt32(gdvExpNotificados.DataKeys[index].Values["acde_sTarifarioId"].ToString());
                }
                                
                strDescripcionCorta = gdvExpNotificados.DataKeys[index].Values["tari_vDescripcionCorta"].ToString();
                strFechaActuacion = gdvExpNotificados.DataKeys[index].Values["actu_dFechaRegistro"].ToString();
            }

            int intOficinaConsularParticipanteId = Convert.ToInt32(gdvExpNotificados.DataKeys[index].Values["ajpa_sOficinaConsularDestinoId"].ToString());

            Session["iActuacionDetalleId"] = iActuacionDetalleId;
            Session["iTarifarioId"] = iTarifariod;
            Session["DescTarifa"] = strDescripcionCorta;
            Session["LblFecha"] = strFechaActuacion;
            Session["OficinaConsularParticipanteId"] = intOficinaConsularParticipanteId;
            Session["ParticipanteEstadoId"] = intParticipanteEstadoId;

            if (gdvExpNotificados.DataKeys[index].Values["ajpa_iPersonaId"].ToString() != "")
            {
                intPersonaId = Convert.ToInt64(gdvExpNotificados.DataKeys[index].Values["ajpa_iPersonaId"].ToString());                         // ID DEL DEMANDADO
            }

            if (gdvExpNotificados.DataKeys[index].Values["ajpa_iEmpresaId"].ToString() != "")
            {
                intEmpresaId = Convert.ToInt64(gdvExpNotificados.DataKeys[index].Values["ajpa_iEmpresaId"].ToString());                         // ID DE LA DEMANDADA
            }

            if (gdvExpNotificados.DataKeys[index].Values["ajpa_iEmpresaId"].ToString() != "")
            {
                intEmpresaId = Convert.ToInt64(gdvExpNotificados.DataKeys[index].Values["ajpa_iEmpresaId"].ToString());                         // ID DEL DEMANDADO
            }
            else
            {
                intEmpresaId = 0;
            }

            Session["iActoJudicialParticipanteId"] = intParticipanteId;

            if (e.CommandName == "Ver")
            {
                Session["intQueHace_Participante"] = 3;
                Session["intParticipaPersonaId"] = intPersonaId;
                Session["intParticipaEmpresaId"] = intEmpresaId;

                MostrarParticipante(sTipoDocumentoId, vNumeroDocumento, 3);
                updNotificados.Update();
            }

            if (e.CommandName == "Notificar")
            {
                ctrlPaginadorActuacion.InicializarPaginador();
                cargarNotificaciones();
                Notificar(intParticipanteId, strNombreParticipante, updNotificados);

            }
            else if (e.CommandName == "Editar")
            {
                Session["intQueHace_Participante"] = 2;
                Session["intParticipaPersonaId"] = intPersonaId;
                Session["intParticipaEmpresaId"] = intEmpresaId;



                MostrarParticipante(sTipoDocumentoId, vNumeroDocumento, 2, intindiceparticipante);

                if (intParticipanteId >= 0 || intPersonaId > 0 || intEmpresaId > 0)
                {
                    txtNom3.Enabled = false;
                    txtApePat3.Enabled = false;
                    txtApeMat3.Enabled = false;
                }

                updNotificados.Update();
            }
            else if (e.CommandName == "Eliminar")
            {
                EliminarParticipante(intParticipanteId);
            }
        }

        protected void btnAceptarNotifica_Click(object sender, EventArgs e)
        {
            DataTable dtTmpPartcipa = new DataTable();
            int intQueHace_Participante = Convert.ToInt32(Session["intQueHace_Participante"]);
            int intFila = 0;
            DateTime? datFchLlegada;
           // DateTime datFchRecepcion = txtFchRecepcion.Value();

            if (txtFchValDip.Text != "")
            {
                if (Comun.EsFecha(txtFchValDip.Text.Trim()) == false)
                {
                    ValParticipante.MostrarValidacion("La fecha de salida de valija no es válida.", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return;
                }
                datFchLlegada = Comun.FormatearFecha(txtFchValDip.Value().ToString());
            }
            else
            {
                datFchLlegada = null;
            }

            dtTmpPartcipa = (DataTable)Session["dtTmpPartipante"];

            Int64 intParticipaPersonaId = Convert.ToInt64(Session["intParticipaPersonaId"]);
            Int64 intParticipaEmpresaId = Convert.ToInt64(Session["intParticipaEmpresaId"]);
            string strNumeroDocumento = txtNumDoc3.Text;

            //------------------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
            //           y la fecha de salida de valija
            // Fecha: 13/01/2017
            //------------------------------------------------------------------

            //if (txtFchValDip.Text != "")
            //{
            //    if (datFchLlegada < datFchRecepcion)
            //    {
            //        ValParticipante.MostrarValidacion("La fecha de llegada de la Valija no puede ser menor a la fecha de recepcion del Expediente", true, Enumerador.enmTipoMensaje.WARNING);
            //        updNotificados.Update();
            //        return;
            //    }
            //}

            if (intQueHace_Participante == 1)                                   // SI SE ESTA AGREGANDO UN PARTICIPANTE, NOS FIJAMOS SI NO FUE INGRESADO YA
            {
                if (BuscarNotificadoEnTemporal(dtTmpPartcipa, intParticipaPersonaId, Convert.ToInt16(ddlTipPersona3.SelectedValue), txtNumDoc3.Text,
                    Comun.ToNullInt32(ddlTipDoc3.SelectedItem.Value)) == true)
                {
                    ValParticipante.MostrarValidacion("El Nº de documento  que intenta agregar ya fue seleccionado, ingrese otro", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return;
                }
            }

            if (ctrlOficinaConsular1.SelectedValue == "0")
            {
                ValParticipante.MostrarValidacion("No ha ingresado la Oficina Consular de destino", true, Enumerador.enmTipoMensaje.WARNING);
                updNotificados.Update();
                return;
            }

            if (intQueHace_Participante == 1)
            {
                DataRow row;
                row = dtTmpPartcipa.NewRow();                                   // ((DataTable)Session["DtRegDirecciones"]).NewRow();
                int intCorrelativoId = 0;
                int intTipParticipante = Convert.ToInt32(Enumerador.enmJudicialTipoParticipante.DEMANDADO);                                  // LE INDICAMOS AL SISTEMA QUE EL TIPO DE PARTICIPANTE SERA (8542 = DEMANDADO)
                Int64 intiActoJudicialParticipanteId = 0;                       // ALMACENAMOS EL ID TEMPORAL DEL PARTICIPANTE

                Int16 intEstadoParticpanteid = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.REGISTRADO);

                if (dtTmpPartcipa.Rows.Count != 0)
                {
                    for (intFila = 0; intFila <= dtTmpPartcipa.Rows.Count - 1; intFila++)
                    {
                        intCorrelativoId = intCorrelativoId + 1;
                    }
                    intCorrelativoId = intCorrelativoId + 1;
                }
                else
                {
                    intCorrelativoId = 1;
                }

                // HALLAMOS EL ID TEMPORAL DEL PARTICIPANTE
                if (dtTmpPartcipa.Rows.Count != 0)
                {
                    DataTable dtTMP = new DataTable();
                    DataView dvResult = dtTmpPartcipa.DefaultView;
                    dvResult.Sort = "ajpa_iActoJudicialParticipanteId ASC";

                    if (Convert.ToInt32(dvResult[0]["ajpa_iActoJudicialParticipanteId"]) > 0)
                        intiActoJudicialParticipanteId = -1;
                    else
                        intiActoJudicialParticipanteId = (Convert.ToInt32(dvResult[0]["ajpa_iActoJudicialParticipanteId"].ToString()) - 1);
                }
                else
                {
                    intiActoJudicialParticipanteId = -1;
                }

                row["ajpa_iActoJudicialParticipanteId"] = intiActoJudicialParticipanteId;
                row["ajpa_iActoJudicialId"] = 0;

                #region validación
                if (Session["iActoJudicialId"] != null)
                    if (Session["iActoJudicialId"].ToString() != string.Empty)
                        row["ajpa_iActoJudicialId"] = Convert.ToInt64(Session["iActoJudicialId"]);
                #endregion

                row["ajpa_sTipoParticipanteId"] = intTipParticipante;
                row["ajpa_sOficinaConsularDestinoId"] = Convert.ToInt16(ctrlOficinaConsular1.SelectedValue);
                row["ajpa_sDocumentoTipoId"] = Convert.ToInt16(ddlTipDoc3.SelectedValue);
                row["ajpa_sTipoPersonaId"] = Convert.ToInt16(ddlTipPersona3.SelectedValue);
                row["ajpa_iPersonaId"] = intParticipaPersonaId;
                row["ajpa_iEmpresaId"] = intParticipaEmpresaId;
                row["ajpa_dFechaAceptacionExpediente"] = ObtenerFechaActual(HttpContext.Current.Session);
                row["ajpa_sEstadoId"] = intEstadoParticpanteid;
                row["ajpa_iNumero"] = intCorrelativoId;                                     // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                row["ajpa_vNroExpediente"] = txtNumExp.Text.ToUpper();                                // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                row["ajpa_vEstadoExpediente"] = "";                                         // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                row["ajpa_vNombre"] = txtNom3.Text.ToUpper();
                row["ajpa_vApePaterno"] = txtApePat3.Text.ToUpper();
                row["ajpa_vApeMaterno"] = txtApeMat3.Text.ToUpper();



                if (Convert.ToInt16(ddlTipPersona3.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL))
                {
                    row["ajpa_vNotificado"] = txtApePat3.Text.ToUpper() + " " + txtApeMat3.Text.ToUpper() + ", " + txtNom3.Text.ToUpper();
                }
                else
                {
                    row["ajpa_vNotificado"] = txtNom3.Text.ToUpper();
                }
                row["ajpa_vConsulado"] = ctrlOficinaConsular1.SelectedItem.Text.ToUpper();
                if (txtFchValDip.Text != "")
                {
                    row["ajpa_dFechaLlegadaValija"] = datFchLlegada;
                }
                row["ajpa_vDocumentoNumero"] = txtNumDoc3.Text.ToUpper();
                row["ajpa_vNumeroHojaRemision"] = txtNumHojRem.Text.ToUpper();
                row["ajno_vEstadoPartipante"] = "SIN NOTIFICAR";

                dtTmpPartcipa.Rows.Add(row);
            }
            else
            {
                if (Session["iActoJudicialParticipanteId"] != null)
                {
                    for (intFila = 0; intFila <= dtTmpPartcipa.Rows.Count - 1; intFila++)
                    {
                        if (Session["iActoJudicialParticipanteId"].ToString() == dtTmpPartcipa.Rows[intFila]["ajpa_iActoJudicialParticipanteId"].ToString())
                        {
                            //CAdena para verificar que el id de persona no esté presente en otro registro.
                            string strSelectQueryDT = string.Empty;
                            string msjErrorTipo = string.Empty;
                            if (intParticipaPersonaId != 0)
                            {
                                msjErrorTipo = "persona";
                                strSelectQueryDT = "ajpa_iPersonaId =" + intParticipaPersonaId + " and ajpa_iActoJudicialParticipanteId <> " + Session["iActoJudicialParticipanteId"].ToString();
                            }
                            if (intParticipaEmpresaId != 0)
                            {
                                msjErrorTipo = "empresa";
                                strSelectQueryDT = "ajpa_iEmpresaId =" + intParticipaEmpresaId + " and ajpa_iActoJudicialParticipanteId <> " + Session["iActoJudicialParticipanteId"].ToString();
                            }

                            if (dtTmpPartcipa.Select(strSelectQueryDT).Length > 0 && msjErrorTipo != "")
                            {
                                ValParticipante.MostrarValidacion("Ya existe la " + msjErrorTipo + " que intenta ingresar.", true, Enumerador.enmTipoMensaje.WARNING);
                                updNotificados.Update();
                                return;
                            }

                            dtTmpPartcipa.Rows[intFila]["ajpa_iEmpresaId"] = intParticipaEmpresaId;
                            dtTmpPartcipa.Rows[intFila]["ajpa_iPersonaId"] = intParticipaPersonaId;
                            dtTmpPartcipa.Rows[intFila]["ajpa_sTipoPersonaId"] = Convert.ToInt16(ddlTipPersona3.SelectedValue);
                            dtTmpPartcipa.Rows[intFila]["ajpa_sDocumentoTipoId"] = Convert.ToInt16(ddlTipDoc3.SelectedValue);
                            dtTmpPartcipa.Rows[intFila]["ajpa_vDocumentoNumero"] = txtNumDoc3.Text.ToUpper();
                            dtTmpPartcipa.Rows[intFila]["ajpa_vNombre"] = txtNom3.Text.ToUpper();
                            dtTmpPartcipa.Rows[intFila]["ajpa_vApePaterno"] = txtApePat3.Text.ToUpper();
                            dtTmpPartcipa.Rows[intFila]["ajpa_vApeMaterno"] = txtApeMat3.Text.ToUpper();
                            dtTmpPartcipa.Rows[intFila]["ajpa_vNotificado"] = string.Format("{0} {1},{2}", txtApePat3.Text.ToUpper(), txtApeMat3.Text.ToUpper(), txtNom3.Text.ToUpper());

                            dtTmpPartcipa.Rows[intFila]["ajpa_sOficinaConsularDestinoId"] = Convert.ToInt16(ctrlOficinaConsular1.SelectedValue);
                            dtTmpPartcipa.Rows[intFila]["ajpa_vConsulado"] = ctrlOficinaConsular1.SelectedItem.Text;
                            dtTmpPartcipa.Rows[intFila]["ajpa_dFechaLlegadaValija"] = datFchLlegada;
                            dtTmpPartcipa.Rows[intFila]["ajpa_vNumeroHojaRemision"] = txtNumHojRem.Text.ToUpper();

                            break;
                        }
                    }
                }
            }

            Session["dtTmpPartipante"] = dtTmpPartcipa;            // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA
            Session["intQueHace_Participante"] = 1;                // LO PONEMOS EN MODO DE NUEVO
            gdvExpNotificados.DataSource = dtTmpPartcipa;
            gdvExpNotificados.DataBind();

            BlanqueaPersonasNotifica();

            string strMensaje = string.Empty;
            if (intQueHace_Participante == 1)
                strMensaje = "El Participante se agregó con éxito";
            else
                strMensaje = "El Participante se actualizó con éxito";

            btnCancelar_Click(null, null);
            ValParticipante.MostrarValidacion(strMensaje, true, Enumerador.enmTipoMensaje.INFORMATION);
            Session["intParticipaPersonaId"] = 0;


            updNotificados.Update();
        }

        protected void btnActaAceptar_Click(object sender, EventArgs e)
        {
            DataTable dtTmpActa = new DataTable();
            DateTime datFechaHoraRecepcion;
            int intUsuarioId = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            Int64 iActoJudicialNotificacionId = Convert.ToInt32(Session["iActoJudicialNotificacionId"]);
            Int64 iActaJudicialId = 0;
            int intFila = 0;
            bool bExisteActacomplementaria = false;
            bool bExisteActaDiligenciamiento = false;

            if (Session["iActaJudicialId"] != null)
                iActaJudicialId = Convert.ToInt64(Session["iActaJudicialId"]);

            dtTmpActa = (DataTable)Session["dtTmpActa"];
            datFechaHoraRecepcion = Comun.FormatearFecha(txtActaFecha.Value().ToString().Substring(0, 10) + " " + txtActaHora.Text);

            for (intFila = 0; intFila <= dtTmpActa.Rows.Count - 1; intFila++)
            {
                if (dtTmpActa.Rows[intFila]["acjd_sTipoActaId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString())
                {
                    bExisteActacomplementaria = true;
                }
                else
                {
                    bExisteActaDiligenciamiento = true;
                }
            }


            if (btnActaAceptar.Text == "Adicionar")
            {
                if (!bExisteActaDiligenciamiento)                                    // SI EL NUMERO DE ACTAS DE DILIGENCIAMIENTO ES IGUAL A 0
                {
                    if (ddlActaTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString())                          // SI EL TIPO DE ACTA ES DIFERENTE A 8531 EMITIMO ALERTA QUE DEBE DE AGREGAR UN ACTA DE DILIGENCIAMIENTO PRIMERO
                    {
                        Validation3.MostrarValidacion("Debe de agregar un Acta de Diligenciamiento, para agregar este tipo de Acta", true, Enumerador.enmTipoMensaje.ERROR);
                        updActas.Update();
                        return;
                    }
                }
                else if (bExisteActacomplementaria)
                {
                    if (ddlActaTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_DILIGENCIAMIENTO).ToString())                          // SI EL TIPO DE ACTA ES DIFERENTE A 8531 EMITIMO ALERTA QUE DEBE DE AGREGAR UN ACTA DE DILIGENCIAMIENTO PRIMERO
                    {
                        Validation3.MostrarValidacion("Ya existe un Acta Complementaria, ya no puede agregar actas de diligenciamiento", true, Enumerador.enmTipoMensaje.ERROR);
                        updActas.Update();
                        return;
                    }
                }

                // NOS ASEGURAMOS QUE EXISTA UN ACTA OBSERVADA
                bool bTodaActaObservada = true;

                for (intFila = 0; intFila <= dtTmpActa.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt16(dtTmpActa.Rows[intFila]["acjd_sEstadoId"].ToString()) != Convert.ToInt16(Enumerador.enmJudicialActaEstado.OBSERVADO))
                    {
                        bTodaActaObservada = false;
                        break;
                    }
                }

                if (!bTodaActaObservada)
                {
                    Validation3.MostrarValidacion("No puede crear un Acta porque la última no ha sido observada.", true, Enumerador.enmTipoMensaje.ERROR);
                    updActas.Update();
                    return;
                }

                DataRow row;
                row = dtTmpActa.NewRow();

                row["acjd_iActaJudicialId"] = 0;
                row["acjd_iActoJudicialNotificacionId"] = iActoJudicialNotificacionId;
                row["acjd_sTipoActaId"] = ddlActaTipo.SelectedValue;
                row["acjd_IFuncionarioFirmanteId"] = intUsuarioId;
                row["acjd_dFechaHoraActa"] = datFechaHoraRecepcion;
                row["acjd_vCuerpoActa"] = txtActaCuerpo.Text.ToUpper();
                //---------------------------------------------------------------
                //Fecha: 17/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Eliminar el campo: Resultado.
                //---------------------------------------------------------------
                row["acjd_vResultado"] = "";

                row["acjd_vObservaciones"] = txtActaObservacion.Text.ToUpper();
                row["acjd_sEstadoId"] = ddlActaEstado.SelectedValue;
                row["acjd_vEstado"] = ddlActaEstado.SelectedItem.Text;
                row["acjd_vResponsable"] = txtActaResponsable.Text.ToUpper();
                row["acjd_sGuardado"] = 0;
                row["acde_sEstadoId"] = 0;


                if (ddlActaTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString())
                {
                    row["acjd_vTipoActa"] = "COMPLEMENTARIA";
                }
                else
                {
                    row["acjd_vTipoActa"] = "DILIGENCIAMIENTO";
                }

                dtTmpActa.Rows.Add(row);
            }
            else
            {
                for (intFila = 0; intFila <= dtTmpActa.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt64(dtTmpActa.Rows[intFila]["acjd_iActaJudicialId"].ToString()) == iActaJudicialId)
                    {
                        dtTmpActa.Rows[intFila]["acjd_sTipoActaId"] = ddlActaTipo.SelectedValue;
                        dtTmpActa.Rows[intFila]["acjd_dFechaHoraActa"] = datFechaHoraRecepcion;
                        dtTmpActa.Rows[intFila]["acjd_vCuerpoActa"] = txtActaCuerpo.Text.ToUpper();
                        //---------------------------------------------------------------
                        //Fecha: 17/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Eliminar el campo: Resultado.
                        //---------------------------------------------------------------
                        //dtTmpActa.Rows[intFila]["acjd_vResultado"] = txtResultado.Text.ToUpper();
                        dtTmpActa.Rows[intFila]["acjd_vResultado"] = "";

                        dtTmpActa.Rows[intFila]["acjd_vObservaciones"] = txtActaObservacion.Text.ToUpper();
                        dtTmpActa.Rows[intFila]["acjd_sEstadoId"] = ddlActaEstado.Text;
                        dtTmpActa.Rows[intFila]["acjd_vEstado"] = ddlActaEstado.SelectedItem.Text.ToUpper();
                        dtTmpActa.Rows[intFila]["acjd_vResponsable"] = txtActaResponsable.Text.ToUpper();
                        dtTmpActa.Rows[intFila]["acjd_sGuardado"] = 0;
                        dtTmpActa.Rows[intFila]["acde_sEstadoId"] = 0;

                        if (ddlActaTipo.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString())
                        {
                            dtTmpActa.Rows[intFila]["acjd_vTipoActa"] = "COMPLEMENTARIA";
                        }
                        else
                        {
                            dtTmpActa.Rows[intFila]["acjd_vTipoActa"] = "DILIGENCIAMIENTO";
                        }


                    }
                }
            }
            Session["dtTmpActa"] = dtTmpActa;                                                  // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvActaDiligenciamiento.DataSource = dtTmpActa;
            gdvActaDiligenciamiento.DataBind();
            BlanqueaActas();
            Session["iActaJudicialId"] = null;
            btnActaAceptar.Text = "Adicionar";
            updActas.Update();
        }

        protected void gdvNotificaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);                             // VARIABLE QUE NOS INDICA EN QUE MODO SE ENCUENTRA EL FORMULARIO

            int index = Convert.ToInt32(e.CommandArgument);
            string strNombreRecepciona = gdvNotificaciones.DataKeys[index].Values["ajno_vPersonaRecibeNotificacion"].ToString();
            int iActoJudicialNotificacionId = Convert.ToInt32(gdvNotificaciones.DataKeys[index].Values["ajno_iActoJudicialNotificacionId"].ToString());
            Int16 sViaEnvioId = Convert.ToInt16(gdvNotificaciones.DataKeys[index].Values["ajno_sViaEnvioId"].ToString());
            Int16 sEstadoId = Convert.ToInt16(gdvNotificaciones.DataKeys[index].Values["ajno_sEstadoId"].ToString());
            Int16 sGuardado = Convert.ToInt16(Session["NotificacionGuardada"]);                                               // AVERIGUAMOS SI HAY NOTIFICACIONES PENDIENTES DE GUARDAR

            Session["iActoJudicialNotificacionId"] = iActoJudicialNotificacionId;
            Session["sViaEnvioId"] = sViaEnvioId;

            if (e.CommandName == "Imprimir")
            {
                if (sGuardado == 0)
                {
                    Validation2.MostrarValidacion("Se requiere grabar la notificacion antes de ejecutar esta accion", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificaciones.Update();
                    return;
                }

                Comun.EjecutarScriptUpdatePanel(updNotificaciones, "Imprimir_expediente(" + index + "," + iActoJudicialNotificacionId + ");");
            }

            if (e.CommandName == "Ver")
            {
                MostrarNotificaciones(iActoJudicialNotificacionId, 3);
                Session["NotificacionIdEditando"] = null;
            }

            if (e.CommandName == "Editar")
            {

                MostrarNotificaciones(iActoJudicialNotificacionId, 2);
                Session["NotificacionIdEditando"] = iActoJudicialNotificacionId.ToString();
            }

            if (e.CommandName == "Eliminar")
            {
                //----------------------------------------------------------------------
                //Fecha: 07/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Permitir la anulación de la notificación
                //----------------------------------------------------------------------

                //bool bReturn = false; ;
                //string strMensaje = string.Empty;
                //if (sEstadoId != Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.ENVIADO) &&
                //    sEstadoId != Convert.ToInt16(Enumerador.enmJudicialNotificacionEstado.REGISTRADO))
                //{
                //    strMensaje = "Solo puede eliminar notificaciones con estado ENVIADO o REGISTRADO";
                //    bReturn = true;
                //}

                //if (bReturn)
                //{
                //    Comun.EjecutarScriptUpdatePanel(updNotificaciones, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "NOTIFICACIONES", strMensaje));
                //    return;
                //}

                EliminarNotificaciones(iActoJudicialNotificacionId);
            }
        }

        protected void ddlTipPersona3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTipoDocumento();
            if (ddlTipPersona3.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                lblObligatorioApeMadre.Visible = false;
                lblObligatorioApePadre.Visible = false;
                lblNombreDemandado.Text = "Empresa:";
            }
            else
            {
                lblObligatorioApeMadre.Visible = true;
                lblObligatorioApePadre.Visible = true;
                lblNombreDemandado.Text = "Nombres:";
            }
        }


        protected void ddlTipDoc3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDocumentoIdentidad = new DataTable();

            dtDocumentoIdentidad = Comun.ObtenerListaDocumentoIdentidad();

            //DataTable dtDocumentoIdentidad = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

            if (ddlTipDoc3.SelectedValue != Convert.ToInt32(Enumerador.enmEmpresaTipoDocumento.RUC).ToString())
            {
                foreach (DataRow fila in dtDocumentoIdentidad.Rows)
                {
                    if (fila["doid_sTipoDocumentoIdentidadId"].ToString() == ddlTipDoc3.SelectedValue.ToString())
                    {
                        int iMaxLenght = 0;
                        short sNacionalidad = 0;


                        if (!String.IsNullOrEmpty(fila["doid_bNumero"].ToString()))
                        {
                            bool bNumero = Convert.ToBoolean(fila["doid_bNumero"]);
                            hidDocumentoSoloNumero.Value = bNumero ? "1" : "0";
                        }

                        if (!String.IsNullOrEmpty(fila["doid_sDigitos"].ToString()))
                        {
                            iMaxLenght = Convert.ToInt16(fila["doid_sDigitos"]);
                        }

                        if (!String.IsNullOrEmpty(fila["doid_sTipoNacionalidad"].ToString()))
                        {
                            sNacionalidad = Convert.ToInt16(fila["doid_sTipoNacionalidad"]);
                        }


                        txtNumDoc3.MaxLength = iMaxLenght;

                        break;
                    }

                }

            }
            else
            {
                txtNumDoc3.MaxLength = 11;

            }

            if (ddlTipDoc3.SelectedValue == "1")  //DNI
            {
                lblObligatorioApeMadre.Visible = true;
            }
            else
            {
                lblObligatorioApeMadre.Visible = false;
            }

            txtNumDoc3.Text = "";




        }

        protected void gdvNotificaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gdvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);                             // VARIABLE QUE NOS INDICA EN QUE MODO SE ENCUENTRA EL FORMULARIO
            int index = Convert.ToInt32(e.CommandArgument);
            Int64 intPagoId = Convert.ToInt64(gdvPagos.DataKeys[index].Values["pago_iPagoId"].ToString());

            Session["pago_iPagoId"] = intPagoId;
            chkGratuito_CheckedChanged(null, null);
            if (e.CommandName == "Ver")
            {
                MostrarPago(intPagoId, 3);
                updNotificados.Update();
            }
            else if (e.CommandName == "Editar")
            {
                MostrarPago(intPagoId, 2);
                updNotificados.Update();
            }
            else if (e.CommandName == "Eliminar")
            {
                EliminarPagos(intPagoId);
            }


        }

        protected void btn_CancelarPago_Click(object sender, EventArgs e)
        {
            BlanqueaPagos();
            ActivarControlesTipoPago();
            ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString();
            ddlMoneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();
            btnAceptarPago.Text = "     Adicionar";
            btnAceptarPago.Enabled = true;
            updNotificados.Update();
        }

        protected void btnAceptarPago_Click(object sender, EventArgs e)
        {
            DataTable TmpREPagos = new DataTable();
            DataRow row;
            DateTime? datFchOperacion = null;
            int intNewIdNotificacion = 0;
            int intFila = 0;

            TmpREPagos = (DataTable)Session["dtTmpRePagos"];

            if (txtFchPago.Text != string.Empty)
            {
                if (Comun.EsFecha(txtFchPago.Text.Trim()) == false)
                {
                    valExpedientePagos.MostrarValidacion("La fecha de pago no es válida.", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return;
                }
                datFchOperacion = txtFchPago.Value();
            }
            else
                datFchOperacion = ObtenerFechaActual(HttpContext.Current.Session);

            if (btnAceptarPago.Text.Trim() == "Adicionar")
            {
                if (TmpREPagos.Rows.Count != 0)
                {
                    DataTable dtTMP = new DataTable();
                    DataView dvResult = TmpREPagos.DefaultView;
                    dvResult.Sort = "pago_iPagoId ASC";

                    intNewIdNotificacion = (Convert.ToInt32(dvResult[0]["pago_iPagoId"].ToString()) - 1);
                }
                else
                {
                    intNewIdNotificacion = -1;
                }

                // REVISAMOS SI EL TIPO CAMBIO ES IGUAL 0 
                if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
                {
                    valExpedientePagos.MostrarValidacion("No ha ingresado el Tipo de Cambio para el día actual", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return;
                }

                // BUSCAMOS SI  YA SE AGREGO UNA TARIFA 32A O 33A
                bool booSeEncontro = false;
                string strTarifaIdEncontrada = string.Empty;
                for (intFila = 0; intFila <= TmpREPagos.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt16(TmpREPagos.Rows[intFila]["pago_sTarifarioId"].ToString()) == 78 ||
                        Convert.ToInt16(TmpREPagos.Rows[intFila]["pago_sTarifarioId"].ToString()) == 81)
                    {

                        booSeEncontro = true;
                        strTarifaIdEncontrada = TmpREPagos.Rows[intFila]["pago_sTarifarioId"].ToString();
                        break;
                    }
                }

                string strMensaje = "";
                bool booRetornar = false;

                if (booSeEncontro == true)
                {
                    if (ddlTarifaConsul.SelectedValue == "78") { strMensaje = "Ya se ha agregado una tarifa 32A o 33A"; booRetornar = true; }
                    if (ddlTarifaConsul.SelectedValue == "81") { strMensaje = "Ya se ha agregado una tarifa 33A o 32A"; booRetornar = true; }
                }
                else
                {
                    if (ddlTarifaConsul.SelectedValue == "79") { strMensaje = "Debe agregar primero una tarifa 32A"; booRetornar = true; }
                    if (ddlTarifaConsul.SelectedValue == "82") { strMensaje = "Debe agregar primero una tarifa o 33A"; booRetornar = true; }
                }

                if (booRetornar == true)
                {
                    valExpedientePagos.MostrarValidacion(strMensaje, true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return;
                }
            }

            if (btnAceptarPago.Text.Trim() == "Adicionar")
            {
                Int32 intCorrelativo = 0;

                if (TmpREPagos.Rows.Count != 0)
                {
                    for (intFila = 0; intFila <= TmpREPagos.Rows.Count - 1; intFila++)
                    {
                        intCorrelativo = intCorrelativo + 1;
                    }
                    intCorrelativo = intCorrelativo + 1;
                }
                else
                {
                    intCorrelativo = 1;
                }

                row = TmpREPagos.NewRow();
                row["pago_sId"] = intCorrelativo;
                row["pago_iPagoId"] = intNewIdNotificacion;
                row["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue);
                row["pago_iActuacionDetalleId"] = 0;
                row["pago_dFechaOperacion"] = datFchOperacion;
                row["pago_sMonedaLocalId"] = Convert.ToInt16(ddlMoneda.SelectedValue);

                if (chkGratuito.Checked)
                {
                    row["pago_FMontoMonedaLocal"] = float.Parse("0.00");
                    row["pago_FMontoSolesConsulares"] = float.Parse("0.00");
                    row["pago_bPagadoFlag"] = 0;
                    row["pago_sBancoId"] = Convert.ToInt16("0");
                    row["pago_vBancoNumeroOperacion"] = string.Empty;
                    row["pago_vNumeroVoucher"] = string.Empty;
                }
                else
                {
                    row["pago_FMontoMonedaLocal"] = float.Parse(txtCosto2.Text);
                    row["pago_FMontoSolesConsulares"] = float.Parse(txtCosto.Text);
                    row["pago_bPagadoFlag"] = 1;
                    row["pago_sBancoId"] = Convert.ToInt16(ddlBanco.SelectedValue);
                    row["pago_vBancoNumeroOperacion"] = txtNumCheque.Text;
                    row["pago_vNumeroVoucher"] = txtNumOrdPag.Text;
                }

                row["pago_FTipCambioBancario"] = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                row["pago_FTipCambioConsular"] = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);




                row["pago_vComentario"] = txtMotivoGratuidad.Text;
                row["pago_sOficinaConsularId"] = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                row["pago_sUsuarioCreacion"] = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                row["pago_vIPCreacion"] = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                row["pago_vHostName"] = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                row["pago_sTarifarioId"] = ddlTarifaConsul.SelectedValue;
                row["pago_sTarifarioDescripcion"] = ddlTarifaConsul.SelectedItem.Text;

                TmpREPagos.Rows.Add(row);
            }
            else
            {
                Int64 iPagoIdActual = Convert.ToInt64(Session["pago_iPagoId"]);

                for (intFila = 0; intFila <= TmpREPagos.Rows.Count - 1; intFila++)
                {
                    if (Convert.ToInt64(TmpREPagos.Rows[intFila]["pago_iPagoId"].ToString()) == iPagoIdActual)
                    {
                        TmpREPagos.Rows[intFila]["pago_sPagoTipoId"] = Convert.ToInt16(ddlTipoPago.SelectedValue); ;
                        TmpREPagos.Rows[intFila]["pago_iActuacionDetalleId"] = 0;
                        TmpREPagos.Rows[intFila]["pago_dFechaOperacion"] = datFchOperacion; ;
                        TmpREPagos.Rows[intFila]["pago_sMonedaLocalId"] = Convert.ToInt16(ddlMoneda.SelectedValue);



                        if (chkGratuito.Checked)
                        {
                            TmpREPagos.Rows[intFila]["pago_FMontoMonedaLocal"] = float.Parse("0.00");
                            TmpREPagos.Rows[intFila]["pago_FMontoSolesConsulares"] = float.Parse("0.00");
                            TmpREPagos.Rows[intFila]["pago_bPagadoFlag"] = 0;
                            TmpREPagos.Rows[intFila]["pago_sBancoId"] = Convert.ToInt16("0");
                            TmpREPagos.Rows[intFila]["pago_vBancoNumeroOperacion"] = string.Empty;
                            TmpREPagos.Rows[intFila]["pago_vNumeroVoucher"] = string.Empty;
                        }
                        else
                        {
                            TmpREPagos.Rows[intFila]["pago_FMontoMonedaLocal"] = float.Parse(txtCosto2.Text);
                            TmpREPagos.Rows[intFila]["pago_FMontoSolesConsulares"] = float.Parse(txtCosto.Text);
                            TmpREPagos.Rows[intFila]["pago_bPagadoFlag"] = 1;
                            TmpREPagos.Rows[intFila]["pago_sBancoId"] = Convert.ToInt16(ddlBanco.SelectedValue);
                            TmpREPagos.Rows[intFila]["pago_vBancoNumeroOperacion"] = txtNumCheque.Text;
                            TmpREPagos.Rows[intFila]["pago_vNumeroVoucher"] = txtNumOrdPag.Text;
                        }




                        TmpREPagos.Rows[intFila]["pago_FTipCambioBancario"] = Convert.ToDecimal(Session[Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO]);
                        TmpREPagos.Rows[intFila]["pago_FTipCambioConsular"] = Convert.ToDecimal(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);
                        TmpREPagos.Rows[intFila]["pago_vComentario"] = txtMotivoGratuidad.Text;
                        TmpREPagos.Rows[intFila]["pago_sOficinaConsularId"] = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        TmpREPagos.Rows[intFila]["pago_sUsuarioCreacion"] = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        TmpREPagos.Rows[intFila]["pago_vIPCreacion"] = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                        TmpREPagos.Rows[intFila]["pago_vHostName"] = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                        TmpREPagos.Rows[intFila]["pago_sTarifarioId"] = ddlTarifaConsul.SelectedValue;
                        TmpREPagos.Rows[intFila]["pago_sTarifarioDescripcion"] = ddlTarifaConsul.SelectedItem.Text;
                    }
                }
            }

            Session["dtTmpRePagos"] = TmpREPagos;            // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA
            gdvPagos.DataSource = TmpREPagos;
            gdvPagos.DataBind();

            BlanqueaPagos();

            ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString();
            ddlMoneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();
            btn_CancelarPago_Click(sender, e);


            chkGratuito_CheckedChanged(null, null);



            updNotificados.Update();
        }

        protected void ddlTarifaConsul_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intTarifaConsularid = Convert.ToInt32(ddlTarifaConsul.SelectedValue);
            int intFila = 0;
            DataTable dtTarifa = (DataTable)Session["dtTarifa"];
            double dobTarifaCosto = 0;
            double douTCConsular = Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]);

            ddlBanco.SelectedValue = "0";
            txtNumCheque.Text = "";
            txtNumOrdPag.Text = string.Empty;
            txtFchPago.Text = string.Empty;

            for (intFila = 0; intFila <= dtTarifa.Rows.Count - 1; intFila++)
            {
                if (intTarifaConsularid == Convert.ToInt32(dtTarifa.Rows[intFila]["tari_sTarifarioId"].ToString()))
                {
                    dobTarifaCosto = Convert.ToDouble(dtTarifa.Rows[intFila]["tari_FCosto"].ToString());
                    decimal decValor = Convert.ToDecimal(dtTarifa.Rows[intFila]["tari_FCosto"].ToString());
                    txtCosto.Text = decValor.ToString("0.00");

                    txtCosto2.Text = Convert.ToString(dobTarifaCosto * douTCConsular);
                }
            }

            if (ddlTarifaConsul.SelectedValue == "0")
            {
                txtCosto.Text = "";
                txtCosto2.Text = "";
            }

            if (chkGratuito.Checked)
            {
                chkGratuito_CheckedChanged(null, null);
            }
        }

        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivarControlesTipoPago();
        }

        protected void ddlViaEnvio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(ddlViaEnvio.SelectedValue) == Convert.ToInt16(Enumerador.enmJudicialViaEnvio.NOTIFICACION_PERSONAL))
            {
                txtEmpPostal.Enabled = false;
                txtEmpPostal.Text = "";
                txtPersNotifica.Enabled = true;
                txtNroCedula.Enabled = false;
                txtNroCedula.Text = string.Empty;
            }
            else
            {
                txtEmpPostal.Enabled = true;
                txtPersNotifica.Enabled = false;
                txtPersNotifica.Text = "";
                txtNroCedula.Enabled = true;
            }
        }

        void GenerarNotificacion(Int64 iActoJudicialParticipanteId, Int64 iActoJudicialNotificacionId)
        {
            DataTable dtResult = new DataTable();
            Proceso MiProc = new Proceso();
            DataTable DtUbigeo = new DataTable();

            DataTable dt = new DataTable();
            DataTable dtTMPReemplazar = new DataTable();
            DataTable dtNotificacion = new DataTable();
            ActoJudicialNotificacionConsultaBL funNotifica = new ActoJudicialNotificacionConsultaBL();
            DataTable dtIdNotificado = new DataTable();
            Int64 intIdPersona = 0;
            Int64 intIdEmpresa = 0;
            int intFila = 0;
            string strFechaCarta = "";
            string strDireccion1 = "";
            string strDireccion2 = "";
            int intIdTipoActa = 0;

            dtTMPReemplazar = CrearTmpTabla();
            dtIdNotificado = funNotifica.Obtener_Id_Notificado(iActoJudicialParticipanteId, iActoJudicialNotificacionId);


            if (dtIdNotificado.Rows.Count != 0)
            {
                EnPersona objEn = new EnPersona();

                intIdTipoActa = Convert.ToInt32(dtIdNotificado.Rows[0]["Id"].ToString());

                if (dtIdNotificado.Rows[0]["ajpa_iPersonaId"].ToString() != "")
                {
                    intIdPersona = Convert.ToInt64(dtIdNotificado.Rows[0]["ajpa_iPersonaId"].ToString());
                    objEn.iPersonaId = intIdPersona;
                }
                else
                {
                    intIdEmpresa = Convert.ToInt64(dtIdNotificado.Rows[0]["ajpa_iEmpresaId"].ToString());
                    objEn.iEmpresaId = Convert.ToInt32(intIdEmpresa);
                }

                object[] arrParametros = { objEn };
                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
                strDireccion1 = objEn.vDireccion;

                strDireccion2 = objEn.vDptoCont + ", " + objEn.vDistCiu;

                if (strDireccion2.ToString().Trim() == ",")
                {
                    strDireccion2 = "";
                }
            }

            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            // OBTENEMOS EL NOMBRE LOS DATOS DE LA OFICINA CONSULAR
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL xFuncion = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();
            dtResult = xFuncion.ObtenerPorId(intOficinaConsularId);

            int inTFila = 0;
            string strUbigeo = "";

            strUbigeo = Session[Constantes.CONST_SESION_UBIGEO].ToString();

            for (inTFila = 0; inTFila <= dtResult.Rows.Count - 1; inTFila++)
            {
                if (strUbigeo == dtResult.Rows[intFila]["ofco_cUbigeoCodigo"].ToString())
                {
                    strUbigeo = dtResult.Rows[intFila]["ubge_vDistrito"].ToString();
                }
            }

            for (intFila = 0; intFila <= dtNotificacion.Rows.Count - 1; intFila++)
            {
                if (iActoJudicialNotificacionId == Convert.ToInt64(dtNotificacion.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString()))
                {
                    DateTime datFechaNotificacion = Comun.FormatearFecha(dtNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString());
                    strFechaCarta = strUbigeo + ", " + datFechaNotificacion.ToString("dd") + " de " + datFechaNotificacion.ToString("MMMM") + " del " + datFechaNotificacion.ToString("yyyy");
                }
            }

            string strNombreNotificado = txtPersNotifica.Text;
            string strDemandante = txtNom1.Text;
            string strDemandado = lblNombreNotificado.Text;
            string strNroExpediente = txtNumExp.Text;
            string strFechaRemitida = string.Empty;

            if (hIndex.Value != null)
            {
                if (Convert.ToInt32(hIndex.Value) < gdvNotificaciones.Rows.Count - 1)
                {
                    DateTime dFechaNotificacion = Comun.FormatearFecha(gdvNotificaciones.DataKeys[Convert.ToInt32(hIndex.Value) + 1].Values["ajno_dFechaHoraNotificacion"].ToString());
                    strFechaRemitida = Util.ObtenerFechaParaDocumentoLegalProtocolar(dFechaNotificacion);
                }
            }


            DataRow row = dtTMPReemplazar.NewRow(); row["strCadenaBuscar"] = "[FechaCarta]"; row["strCadenaReemplazar"] = strFechaCarta; dtTMPReemplazar.Rows.Add(row);
            DataRow row1 = dtTMPReemplazar.NewRow(); row1["strCadenaBuscar"] = "[NombreNotificado]"; row1["strCadenaReemplazar"] = strNombreNotificado; dtTMPReemplazar.Rows.Add(row1);
            DataRow row2 = dtTMPReemplazar.NewRow(); row2["strCadenaBuscar"] = "[Direccion1]"; row2["strCadenaReemplazar"] = strDireccion1; dtTMPReemplazar.Rows.Add(row2);
            DataRow row3 = dtTMPReemplazar.NewRow(); row3["strCadenaBuscar"] = "[Direccion2]"; row3["strCadenaReemplazar"] = strDireccion2; dtTMPReemplazar.Rows.Add(row3);
            DataRow row4 = dtTMPReemplazar.NewRow(); row4["strCadenaBuscar"] = "[Demandante]"; row4["strCadenaReemplazar"] = strDemandante; dtTMPReemplazar.Rows.Add(row4);
            DataRow row5 = dtTMPReemplazar.NewRow(); row5["strCadenaBuscar"] = "[Demandado]"; row5["strCadenaReemplazar"] = strDemandado; dtTMPReemplazar.Rows.Add(row5);
            DataRow row6 = dtTMPReemplazar.NewRow(); row6["strCadenaBuscar"] = "[NroExpediente]"; row6["strCadenaReemplazar"] = strNroExpediente; dtTMPReemplazar.Rows.Add(row6);
            DataRow row7 = dtTMPReemplazar.NewRow(); row7["strCadenaBuscar"] = "[FechaRemitida]"; row7["strCadenaReemplazar"] = strFechaRemitida; dtTMPReemplazar.Rows.Add(row7);
            DataRow row8 = dtTMPReemplazar.NewRow(); row8["strCadenaBuscar"] = "[Juzgado]";



            if (txtOrgano.Text.ToString() != string.Empty)
                row8["strCadenaReemplazar"] = txtOrgano.Text;
            else
                row8["strCadenaReemplazar"] = ddlEntSoli.SelectedItem.Text;

            dtTMPReemplazar.Rows.Add(row8);
            DataRow row9 = dtTMPReemplazar.NewRow(); row9["strCadenaBuscar"] = "[Consulado]"; row9["strCadenaReemplazar"] = Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE]; dtTMPReemplazar.Rows.Add(row9);
            DataRow row10 = dtTMPReemplazar.NewRow(); row10["strCadenaBuscar"] = "[Logo]"; dtTMPReemplazar.Rows.Add(row10);

            DataRow row11 = dtTMPReemplazar.NewRow(); row11["strCadenaBuscar"] = "[ORDINAL]"; row11["strCadenaReemplazar"] = NumeroOrdinal(intIdTipoActa); dtTMPReemplazar.Rows.Add(row11);

            Util.DataTableVarcharMayusculas(dtTMPReemplazar);

            string strRutaHtml = string.Empty;
            string strArchivoPDF = string.Empty;

            #region SELECCIONAMOS PLANTILLA
            if (intIdTipoActa == 1)
            {
                strRutaHtml = Server.MapPath("~/Registro/Plantillas/primera-notificacion-exhorto.html");
                strArchivoPDF = "primera-notificacion-exhorto.pdf";
            }
            if (intIdTipoActa > 1)
            {
                strRutaHtml = Server.MapPath("~/Registro/Plantillas/segunda-notificacion-exhorto.html");
                strArchivoPDF = NumeroOrdinal(intIdTipoActa).ToLower().Trim() + "-notificacion-exhorto.pdf";
            }

            #endregion

            String localfilepath = String.Empty;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            string strRutaPDF = uploadPath + @"\" + strArchivoPDF;

            //UIConvert.GenerarPDF(dtTMPReemplazar, strRutaHtml, strRutaPDF);

            //DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, strRutaHtml, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
            //DocumentoiTextSharp odoc = new DocumentoiTextSharp(

            // DocumentoiTextSharp.CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, "", null);

            CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"));

            if (System.IO.File.Exists(strRutaPDF))
            {
                this.hIndex.Value = string.Empty;
                this.hId_Actuacion_Select.Value = string.Empty;

                new Descarga().Download(strRutaPDF, strArchivoPDF, false);
            }
        }


        private static void CreateFilePDFConformidad(DataTable TablaText, string HtmlPath, string PdfPath, string imgServerPAth, List<object[]> listFirmas = null, bool bAplicarCierreTexto = false)
        {
            try
            {
                if (!File.Exists(HtmlPath))
                    return;

                if (File.Exists(PdfPath))
                    File.Delete(PdfPath);


                float fMargenIzquierdaDoc = 80;
                float fMargenDerechaDoc = 80;
                float fImageMargin = 40f;
                float fImageWidth = 57.77f;
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, 100, 100);

                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;
                iTextSharp.text.pdf.PdfPTable oPdfPTable;
                iTextSharp.text.pdf.PdfPRow oPdfPRow;
                iTextSharp.text.pdf.PdfPCell oPdfPCell = null;
                iTextSharp.text.Chunk oChunk;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;
                iTextSharp.text.FontFactory.RegisterDirectories();

                document.Open();

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));

                document.Open();
                document.NewPage();

                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);

                float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                #region Imagen

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgServerPAth);
                img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 130);
                img.ScaleAbsoluteHeight(85f);
                img.ScaleAbsoluteWidth(fImageWidth);
                document.Add(img);

                #endregion

                #region Consulado Imagen

                // PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);


                cb.BeginText();


                cb.SetFontAndSize(bfTimes, 6);

                string texto = string.Empty;

                float pos = 0;
                float tamPalabra = 0;
                float ancho = 80f;
                String NombreConsulado = String.Empty;
                NombreConsulado = HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString();

                if (NombreConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                {
                    ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                    NombreConsulado = NombreConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                }




                int iPosicionComa = NombreConsulado.IndexOf(",");

                if (iPosicionComa >= 0)
                    NombreConsulado = NombreConsulado.Substring(0, iPosicionComa);



                float posxAcumulado = tamPalabra;

                foreach (string palabra in NombreConsulado.Split(' '))
                {
                    tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                    if (posxAcumulado + tamPalabra > ancho)
                    {

                        cb.SetTextMatrix(40f + (57.77f / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 140 + pos);
                        cb.ShowText(texto.Trim());
                        texto = string.Empty;

                        pos -= 10;
                        posxAcumulado = 0;
                    }

                    posxAcumulado += tamPalabra;
                    posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                    texto += " " + palabra;
                }

                if (texto.Trim() != string.Empty)
                {
                    cb.SetTextMatrix(40f + (57.77f / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 140 + pos);
                    cb.ShowText(texto.Trim());
                }

                cb.EndText();

                #endregion

                for (int k = 0; k < objects.Count; k++)
                {
                    oIElement = (iTextSharp.text.IElement)objects[k];
                    if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {
                        oParagraph = new iTextSharp.text.Paragraph();
                        oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;

                        //iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                        iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                        for (int z = 0; z < oIElement.Chunks.Count; z++)
                        {
                            strContent = ReplaceTexto(oIElement.Chunks[z].Content.ToString(), TablaText);

                            if (strContent != "\n")
                            {
                                strContent = strContent.Trim();
                                oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                            }
                            else
                            {
                                oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                continue;
                            }

                            if (z == oIElement.Chunks.Count - 1)
                            {
                                List<string> listTextos = new List<string>();
                                List<iTextSharp.text.Font> listFonts = new List<iTextSharp.text.Font>();

                                string textoNotarialCierre = string.Empty;
                                foreach (iTextSharp.text.Chunk ch in oIElement.Chunks)
                                {
                                    listTextos.Add(ch.Content.Trim());
                                    listFonts.Add(ch.Font);
                                }

                                if (strContent.Trim() != string.Empty &&
                                     strContent.Trim() != "PODER FUERA DE REGISTRO" &&
                                     strContent.Trim() != "CONCLUSIÓN:" &&
                                    bAplicarCierreTexto)
                                {
                                    textoNotarialCierre = Comun.ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                }

                                if (textoNotarialCierre != string.Empty)
                                {
                                    iTextSharp.text.Font font = new iTextSharp.text.Font(oIElement.Chunks[z].Font);
                                    font.SetStyle(0);
                                    oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                }
                            }
                            else
                            {
                                oParagraph.Add(new iTextSharp.text.Chunk(" ", oIElement.Chunks[z].Font));
                            }
                        }

                        oParagraph.SetLeading(0.0f, 1.5f);
                        document.Add(oParagraph);
                    }

                    else if (objects[k].GetType().FullName == "iTextSharp.text.pdf.PdfPTable")
                    {

                        oPdfPTable = (iTextSharp.text.pdf.PdfPTable)objects[k];

                        iTextSharp.text.pdf.PdfPTable oNewPdfPTable = new iTextSharp.text.pdf.PdfPTable(oPdfPTable.NumberOfColumns);

                        int[] DimensionColumna = new int[oPdfPTable.NumberOfColumns];
                        int aux;

                        oNewPdfPTable.WidthPercentage = 100;

                        string imgFirma1 = string.Empty;
                        string imgFirma2 = string.Empty;

                        iTextSharp.text.Image jpg = null;
                        for (int row = 0; row < oPdfPTable.Rows.Count; row++)
                        {
                            for (int cell = 0; cell < oPdfPTable.Rows[row].GetCells().Length; cell++)
                            {
                                oPdfPCell = oPdfPTable.Rows[row].GetCells()[cell];
                                oParagraph = new iTextSharp.text.Paragraph();

                                for (int paragraph = 0; paragraph < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements.Count; paragraph++)
                                {
                                    for (int chunk = 0; chunk < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements[paragraph].Chunks.Count; chunk++)
                                    {
                                        if (!oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma1]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma2]"))
                                        {
                                            strContent = ReplaceTexto(oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content, TablaText);
                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Font));
                                            aux = strContent.Length;
                                            if (aux > DimensionColumna[cell])
                                                DimensionColumna[cell] = aux;

                                            oParagraph.Leading = 12;
                                        }
                                        else
                                        {
                                            //otro texto para las imagenes en caso de poner el sello, si no
                                        }
                                    }
                                }
                                aux = 0;
                            }

                            oPdfPCell.CompositeElements.Clear();
                            oPdfPCell.AddElement(oParagraph);

                            if (jpg != null)
                            {
                                oPdfPCell.AddElement(jpg);
                                jpg = null;
                            }

                            oNewPdfPTable.AddCell(oPdfPCell);
                        }
                    }
                }

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                //foreach (object[] firma in listFirmas)
                //{
                //    parrafo = new iTextSharp.text.Paragraph();
                //    frase = new iTextSharp.text.Phrase();

                //    if (writer.GetVerticalPosition(false) >= 220)
                //    {
                //        frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n\n"));
                //        parrafo.Add(frase);
                //        document.Add(parrafo);
                //    }
                //    else
                //    {
                //        while (writer.GetVerticalPosition(false) < 220)
                //        {
                //            document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                //        }
                //    }

                //    if ((bool)firma[2])
                //    {
                //        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                //        cb.Rectangle(document.PageSize.Width - 160, writer.GetVerticalPosition(false) - 10, 70f, 80f);
                //        cb.Stroke();
                //    }

                //    //parrafo = new iTextSharp.text.Paragraph();
                //    //parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                //    //parrafo.Font = iTextSharp.text.FontFactory.GetFont("Arial");

                //    //frase = new iTextSharp.text.Phrase();
                //    //frase.Add(new iTextSharp.text.Chunk("\n" + "                                                                                                        Huella Digital"));
                //    //frase.Add(new iTextSharp.text.Chunk("\n\n" + "---------------------------------------------------------------"));
                //    //frase.Add(new iTextSharp.text.Chunk("\n" + firma[0].ToString().ToUpper()));
                //    //frase.Add(new iTextSharp.text.Chunk("\n" + firma[1].ToString()));

                //    //parrafo.Add(frase);
                //    //document.Add(parrafo);
                //}

                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ReplaceTexto(string oTexto, System.Data.DataTable dt)
        {
            string s_NewTexto = oTexto;
            int intFila = 0;
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        for (intFila = 0; intFila <= dt.Rows.Count - 1; intFila++)
                        {
                            string strcadBuscar = dt.Rows[intFila]["strCadenaBuscar"].ToString();
                            string strcadReemplaza = dt.Rows[intFila]["strCadenaReemplazar"].ToString();

                            s_NewTexto = s_NewTexto.Replace(strcadBuscar, strcadReemplaza);
                        }
                    }
                }
            }
            catch
            {
                s_NewTexto = oTexto;
            }

            return s_NewTexto;
        }
        void GenerarActas()
        {
            Int64 iActoJudicialNotificacionId = 0;

            EjecutarScript("EnableTabIndex(2);");

            Session["iActaJudicialId"] = null;

            iActoJudicialNotificacionId = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);
            lblNombreActa.Text = lblNombreNotificado.Text;
            BlanqueaActas();
            ActivarActasControl();
            CargarActas(iActoJudicialNotificacionId);
            btnActaAceptar.Text = "Adicionar";

            ddlActaEstado.SelectedValue = Convert.ToString(Convert.ToUInt16(Enumerador.enmJudicialActaEstado.REGISTRADO));

            updActas.Update();
        }

        DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }

        void cargarDatosDemandante()
        {
            int intPersonaId = 0;

            int iActoJudicialId = Convert.ToInt32(Session["iActoJudicialId"]);
            DataTable dtPartipanteDemandante = new DataTable();
            ActoJudicialParticipanteConsultaBL CLActoJudicialParticipa = new ActoJudicialParticipanteConsultaBL();

            Int16? intOficinaConsularId = null;                          // LE ENVIAMOS NULL PARA QUE FILTRE TODOS LOS PARTICIPANTES DEL EXPEDIENTE, AQUI NO SE REQUIERE EL FILTRO

            dtPartipanteDemandante = CLActoJudicialParticipa.Obtener(iActoJudicialId, 8541, intOficinaConsularId);               // OBTENEMOS LA LISTA DE PARTICIPANTES DE LA BD

            if (dtPartipanteDemandante.Rows.Count != 0)
            {
                if (dtPartipanteDemandante.Rows[0]["ajpa_iPersonaId"].ToString() != "")
                {
                    EnPersona objEn = new EnPersona();
                    intPersonaId = Convert.ToInt32(dtPartipanteDemandante.Rows[0]["ajpa_iPersonaId"].ToString());
                    objEn.iPersonaId = intPersonaId;

                    object[] arrParametros = { objEn };
                    objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

                    txtNom3.Text = objEn.vNombres;

                    ddlTipPersona.SelectedValue = objEn.sPersonaTipoId.ToString();
                    ddlTipDocumento.SelectedValue = objEn.sDocumentoTipoId.ToString();

                    ViewState["intPersonaId"] = objEn.iPersonaId;



                    txtNroDoc1.Text = objEn.vDocumentoNumero;
                    txtNom1.Text = objEn.vApellidoPaterno + " " + objEn.vApellidoMaterno + ", " + objEn.vNombres;
                    txtDir1.Text = objEn.vDireccion;
                    txtTel1.Text = objEn.vTelefono;
                    txtCorreo1.Text = objEn.vCorreoElectronico;
                }

                if (dtPartipanteDemandante.Rows[0]["ajpa_iEmpresaId"].ToString() != "")
                {
                    Util.CargarParametroDropDownList(ddlTipDocumento, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO), true);

                    SGAC.Registro.Persona.BL.PersonaConsultaBL funEmpresa = new SGAC.Registro.Persona.BL.PersonaConsultaBL();

                    DataTable dtEmpresa = new DataTable();

                    dtEmpresa = funEmpresa.Obtener_Empresa_Por_Id(Convert.ToInt32(dtPartipanteDemandante.Rows[0]["ajpa_iEmpresaId"].ToString()));

                    if (dtEmpresa.Rows.Count != 0)
                    {
                        ddlTipPersona.SelectedValue = dtEmpresa.Rows[0]["empr_cTipoPersona"].ToString();
                        ddlTipDocumento.SelectedValue = dtEmpresa.Rows[0]["empr_sTipoDocumentoId"].ToString();
                        txtNroDoc1.Text = dtEmpresa.Rows[0]["empr_vNumeroDocumento"].ToString();
                        txtNom1.Text = dtEmpresa.Rows[0]["empr_vRazonSocial"].ToString();
                        txtDir1.Text = dtEmpresa.Rows[0]["Direccion"].ToString();
                        txtTel1.Text = dtEmpresa.Rows[0]["empr_vTelefono"].ToString();
                        txtCorreo1.Text = dtEmpresa.Rows[0]["empr_vCorreo"].ToString();
                    }

                    ViewState["intPersonaId"] = Convert.ToInt32(dtPartipanteDemandante.Rows[0]["ajpa_iEmpresaId"]);
                }
            }
        }

        void CargarPagos(Int64 intActuacionID = 0)
        {
            ActuacionPagoConsultaBL funPagos = new ActuacionPagoConsultaBL();
            DataTable dtPagos = new DataTable();
            DataTable dtTMPPagos = new DataTable();
            int intFila = 0;
            int intCorrelativo = 0;

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);


            string sCodParticipante = string.Empty;
            if (Session["dtTmpPartipante"] != null)
            {
                foreach (DataRow item in ((DataTable)Session["dtTmpPartipante"]).Rows)
                {
                    sCodParticipante += item["ajpa_iActoJudicialParticipanteId"].ToString() + ",";
                }
                if (sCodParticipante.Length > 0)
                {
                    sCodParticipante = sCodParticipante.Substring(0, sCodParticipante.Length - 1);
                }

            }

            if (intOficinaConsularId == intOficinaConsularLimaId)
            {
                dtPagos = funPagos.ObtenerPago(sCodParticipante, null);//funPagos.ObtenerPago(intActuacionID, null);
            }
            else
            {
                dtPagos = funPagos.ObtenerPago(sCodParticipante, intOficinaConsularId); //funPagos.ObtenerPago(intActuacionID, intOficinaConsularId);
            }

            dtTMPPagos = (DataTable)Session["dtTmpRePagos"];

            for (intFila = 0; intFila <= dtPagos.Rows.Count - 1; intFila++)
            {
                DataRow row;

                row = dtTMPPagos.NewRow();
                intCorrelativo = intCorrelativo + 1;
                row["pago_sId"] = intCorrelativo;
                row["pago_iPagoId"] = dtPagos.Rows[intFila]["pago_iPagoId"].ToString();
                row["pago_sPagoTipoId"] = dtPagos.Rows[intFila]["pago_sPagoTipoId"].ToString();                                         // LE ASIGNAMOS EL TIPO DE PAGO NO COBRADO
                row["pago_iActuacionDetalleId"] = dtPagos.Rows[intFila]["pago_iActuacionDetalleId"].ToString();
                row["pago_dFechaOperacion"] = dtPagos.Rows[intFila]["pago_dFechaOperacion"].ToString();
                row["pago_sMonedaLocalId"] = dtPagos.Rows[intFila]["pago_sMonedaLocalId"].ToString();
                row["pago_FMontoMonedaLocal"] = dtPagos.Rows[intFila]["pago_FMontoMonedaLocal"].ToString();
                row["pago_FMontoSolesConsulares"] = dtPagos.Rows[intFila]["pago_FMontoSolesConsulares"].ToString();
                row["pago_FTipCambioBancario"] = dtPagos.Rows[intFila]["pago_FTipCambioBancario"].ToString();
                row["pago_FTipCambioConsular"] = dtPagos.Rows[intFila]["pago_FTipCambioConsular"].ToString();

                if (dtPagos.Rows[intFila]["pago_sBancoId"].ToString() != "")
                {
                    row["pago_sBancoId"] = dtPagos.Rows[intFila]["pago_sBancoId"].ToString();
                }

                row["pago_vBancoNumeroOperacion"] = dtPagos.Rows[intFila]["pago_vBancoNumeroOperacion"].ToString();
                row["pago_bPagadoFlag"] = dtPagos.Rows[intFila]["pago_bPagadoFlag"].ToString();
                row["pago_vComentario"] = dtPagos.Rows[intFila]["pago_vComentario"].ToString();
                row["pago_sOficinaConsularId"] = dtPagos.Rows[intFila]["actu_sOficinaConsularId"].ToString();
                row["pago_sUsuarioCreacion"] = dtPagos.Rows[intFila]["pago_sUsuarioCreacion"].ToString();
                row["pago_vIPCreacion"] = dtPagos.Rows[intFila]["pago_vIPCreacion"].ToString();
                row["pago_vHostName"] = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                row["pago_sTarifarioId"] = dtPagos.Rows[intFila]["acde_sTarifarioId"].ToString();
                string strDesctarifa = dtPagos.Rows[intFila]["tari_sNumero"].ToString() + " " + dtPagos.Rows[intFila]["tari_vLetra"].ToString() + " " + dtPagos.Rows[intFila]["tari_vDescripcionCorta"].ToString();
                row["pago_sTarifarioDescripcion"] = strDesctarifa;

                row["pago_vNumeroVoucher"] = dtPagos.Rows[intFila]["pago_vNumeroVoucher"].ToString();
                row["pago_iActuacionId"] = dtPagos.Rows[intFila]["acde_iActuacionId"].ToString();

                dtTMPPagos.Rows.Add(row);
            }

            Session["dtTmpRePagos"] = dtTMPPagos;                                                                    // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvPagos.DataSource = dtTMPPagos;
            gdvPagos.DataBind();

            updNotificados.Update();
        }

        void CargarParticipantes(BE.RE_ACTOJUDICIAL BE_ActoJudicial)
        {
            ActoJudicialParticipanteConsultaBL CLActoJudicialParticipa = new ActoJudicialParticipanteConsultaBL();
            DataTable dtPartipante = new DataTable();
            DataTable dtTmpPartipante = new DataTable();
            int iActoJudicialId = Convert.ToInt32(Session["iActoJudicialId"]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

            CrearTemporales();
            dtTmpPartipante = (DataTable)Session["dtTmpPartipante"];

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (intOficinaConsularLimaId == intOficinaConsularId)                                                 // PREGUNTAMOS SI LA OFICINA CIONSULAR ES IGUAL A LIMA
            {
                dtPartipante = CLActoJudicialParticipa.Obtener(iActoJudicialId, 8542, null);                      // SI ES LIMA MOSTRAMOS TODOS LOS PARTICIPATES
            }
            else
            {
                // SI EL CONSULADO ES DIFERENTE A LIMA
                dtPartipante = CLActoJudicialParticipa.Obtener(iActoJudicialId, 8542, intOficinaConsularId);      // MOSTRAMOS LOS PARTICIPANTES DEL CONSULADO ACTUAL
            }

            int intCorrelativo = 0;

            //CARGAMOS LOS PARTICIPANTES EN EL TEMPORAL
            foreach (DataRow fila in dtPartipante.Rows)
            {
                DataRow row;

                row = dtTmpPartipante.NewRow();
                string strEstadoExpediente = EstadoExpediente(BE_ActoJudicial.acju_sEstadoId);
                intCorrelativo = intCorrelativo + 1;

                row["ajpa_iActoJudicialParticipanteId"] = fila["ajpa_iActoJudicialParticipanteId"].ToString();
                row["ajpa_iActoJudicialId"] = fila["ajpa_iActoJudicialId"].ToString();
                row["ajpa_sTipoParticipanteId"] = fila["ajpa_sTipoParticipanteId"].ToString();
                row["ajpa_sOficinaConsularDestinoId"] = Convert.ToInt16(fila["ajpa_sOficinaConsularDestinoId"].ToString());
                row["ajpa_sTipoPersonaId"] = Convert.ToInt16(fila["ajpa_sTipoPersonaId"].ToString());

                if (fila["ajpa_iPersonaId"].ToString() != "")
                {
                    row["ajpa_iPersonaId"] = fila["ajpa_iPersonaId"].ToString();
                }

                if (fila["ajpa_iEmpresaId"].ToString() != "")
                {
                    row["ajpa_iEmpresaId"] = fila["ajpa_iEmpresaId"].ToString();
                }
                row["ajpa_dFechaAceptacionExpediente"] = fila["ajpa_dFechaAceptacionExpediente"].ToString();
                row["ajpa_sEstadoId"] = fila["ajpa_sEstadoId"].ToString();
                row["ajpa_iNumero"] = intCorrelativo;                                                               // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                if (fila["ajpa_dFechaNotificacion"].ToString() != "")
                {
                    row["ajpa_dFechaNotificacion"] = fila["ajpa_dFechaNotificacion"].ToString();                    // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                }
                row["ajpa_vNroExpediente"] = BE_ActoJudicial.acju_vNumeroExpediente.ToUpper();                                // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                row["ajpa_vEstadoExpediente"] = strEstadoExpediente;                                                // CAMPO AGREGADO PARA MOSTRAR EN LA GRILLA
                row["ajpa_vNombre"] = fila["pers_vNombres"].ToString();
                row["ajpa_vApePaterno"] = fila["pers_vApellidoPaterno"].ToString();
                row["ajpa_vApeMaterno"] = fila["pers_vApellidoMaterno"].ToString();
                row["ajpa_vNotificado"] = row["ajpa_vNombre"] + " " + row["ajpa_vApePaterno"] + " " + row["ajpa_vApeMaterno"];

                ctrlOficinaConsular1.SelectedValue = fila["ajpa_sOficinaConsularDestinoId"].ToString();
                row["ajpa_vConsulado"] = fila["ofco_vNombre"].ToString();

                if (fila["peid_sDocumentoTipoId"].ToString() != "")
                {
                    row["ajpa_sDocumentoTipoId"] = fila["peid_sDocumentoTipoId"].ToString();
                }
                row["ajpa_vDocumentoNumero"] = fila["peid_vDocumentoNumero"].ToString();

                if (fila["ajpa_iActuacionDetalleId"].ToString() != "")
                {
                    row["ajpa_iActuacionDetalleId"] = Convert.ToInt64(fila["ajpa_iActuacionDetalleId"].ToString());
                }
                if (fila["ajpa_dFechaLlegadaValija"].ToString() != "")
                {
                    row["ajpa_dFechaLlegadaValija"] = fila["ajpa_dFechaLlegadaValija"].ToString();
                }

                if (fila["acde_sTarifarioId"] != null)
                    if (fila["acde_sTarifarioId"].ToString() != string.Empty)
                        row["acde_sTarifarioId"] = Convert.ToInt16(fila["acde_sTarifarioId"].ToString());

                row["tari_vDescripcionCorta"] = fila["tari_vDescripcionCorta"].ToString();
                row["actu_dFechaRegistro"] = fila["actu_dFechaRegistro"].ToString();
                row["ajno_vEstadoPartipante"] = fila["ajno_vEstadoPartipante"].ToString();
                row["ajpa_vNumeroHojaRemision"] = fila["ajpa_vNumeroHojaRemision"].ToString();

                row["ajpa_bActaFlag"] = fila["ajpa_bActaFlag"].ToString();
                row["ajpa_bNotificacionFlag"] = fila["ajpa_bNotificacionFlag"].ToString();

                dtTmpPartipante.Rows.Add(row);
            }

            Session["dtTmpPartipante"] = dtTmpPartipante;
            gdvExpNotificados.DataSource = dtTmpPartipante;
            gdvExpNotificados.DataBind();


            if (BE_ActoJudicial.acju_sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO))
            {
                gdvExpNotificados.Columns[6].Visible = false;
            }


            BlanqueaPersonasNotifica();
            updNotificados.Update();
        }

        void cargarNotificaciones()
        {

            int intFila = 0;
            Int64 iActoJudicialId = Convert.ToInt64(Session["iActoJudicialId"]);
            DataTable dtNotificaciones = new DataTable();
            DataTable dtTmpNotificaciones = new DataTable();
            int intCorrelativo = 0;
            Int64 iActoJudicialParticipanteId = Convert.ToInt64(Session["iActoJudicialParticipanteId"]);

            CrearTmpNotificaciones();
            dtTmpNotificaciones = (DataTable)Session["dtTmpNotificacion"];

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            ActoJudicialNotificacionConsultaBL CLActoJudicialNotifica = new ActoJudicialNotificacionConsultaBL();
            dtNotificaciones = CLActoJudicialNotifica.Obtener(iActoJudicialId, iActoJudicialParticipanteId, ctrlPaginadorActuacion.PaginaActual.ToString(), ctrlPaginadorActuacion.PageSize,
                ref IntTotalCount, ref IntTotalPages);

            if (dtNotificaciones.Rows.Count != 0)
            {
                intCorrelativo = dtNotificaciones.Rows.Count + 1;

                for (intFila = 0; intFila <= dtNotificaciones.Rows.Count - 1; intFila++)
                {
                    DataRow row;
                    row = dtTmpNotificaciones.NewRow();
                    intCorrelativo = intCorrelativo - 1;

                    row["ajno_iActoJudicialNotificacionId"] = dtNotificaciones.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString();
                    row["ajno_iActoJudicialParticipanteId"] = dtNotificaciones.Rows[intFila]["ajno_iActoJudicialParticipanteId"].ToString();
                    if (dtNotificaciones.Rows[intFila]["ajno_sTipoRecepcionId"].ToString() != "")
                    {
                        row["ajno_sTipoRecepcionId"] = dtNotificaciones.Rows[intFila]["ajno_sTipoRecepcionId"].ToString();
                    }
                    row["ajno_sViaEnvioId"] = dtNotificaciones.Rows[intFila]["ajno_sViaEnvioId"].ToString();
                    row["ajno_vEmpresaServicioPostal"] = dtNotificaciones.Rows[intFila]["ajno_vEmpresaServicioPostal"].ToString();
                    row["ajno_vPersonaNotificacion"] = dtNotificaciones.Rows[intFila]["ajno_vPersonaNotificacion"].ToString();
                    row["ajno_dFechaHoraNotificacion"] = dtNotificaciones.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString();
                    row["ajno_vNumeroCedula"] = dtNotificaciones.Rows[intFila]["ajno_vNumeroCedula"].ToString();
                    row["ajno_vPersonaRecibeNotificacion"] = dtNotificaciones.Rows[intFila]["ajno_vPersonaRecibeNotificacion"].ToString();

                    if (dtNotificaciones.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString() != "")
                    {
                        row["ajno_dFechaHoraRecepcion"] = dtNotificaciones.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString();
                    }

                    row["ajno_vCuerpoNotificacion"] = dtNotificaciones.Rows[intFila]["ajno_vCuerpoNotificacion"].ToString();
                    row["ajno_vObservaciones"] = dtNotificaciones.Rows[intFila]["ajno_vObservaciones"].ToString();
                    row["ajno_sEstadoId"] = dtNotificaciones.Rows[intFila]["ajno_sEstadoId"].ToString();                                      // INDICAMOS EL ESTADO DEL REGISTRO
                    row["ajno_vViaEnvio"] = dtNotificaciones.Rows[intFila]["ajno_sViaEnvioDescripcion"].ToString();
                    row["ajno_sCorrelativo"] = dtNotificaciones.Rows[intFila]["id"].ToString();
                    row["ajno_vEstadoDescripcion"] = dtNotificaciones.Rows[intFila]["esta_vDescripcionCorta"].ToString();
                    row["ajno_sGuardado"] = 1;

                    row["ajno_vEstadoInicial"] = dtNotificaciones.Rows[intFila]["ajno_vEstadoInicial"].ToString();
                    row["ajno_vTipRecepcion"] = dtNotificaciones.Rows[intFila]["ajno_vTipRecepcion"].ToString();
                    row["esta_vDescripcionCorta"] = dtNotificaciones.Rows[intFila]["esta_vDescripcionCorta"].ToString();

                    dtTmpNotificaciones.Rows.Add(row);
                }

                ctrlPaginadorActuacion.TotalResgistros = Convert.ToInt32(IntTotalCount);
                ctrlPaginadorActuacion.TotalPaginas = Convert.ToInt32(IntTotalPages);

                ctrlPaginadorActuacion.Visible = false;
                if (ctrlPaginadorActuacion.TotalResgistros > Constantes.CONST_PAGE_SIZE_NORIFICACIONES)
                {
                    ctrlPaginadorActuacion.Visible = true;
                }
            }

            Session["dtTmpNotificacion"] = dtTmpNotificaciones;                                                                               // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvNotificaciones.DataSource = dtTmpNotificaciones;
            gdvNotificaciones.DataBind();

            
            
            BlanqueNotificaciones();

            updNotificados.Update();
        }

        protected void ctrlPaginadorActuacion_Click(object sender, EventArgs e)
        {
            cargarNotificaciones();
            updNotificaciones.Update();
        }

        void CargarActas(Int64 iActoJudicialNotificacionId)
        {
            int intFila = 0;
            int iActoJudicialId = Convert.ToInt32(Session["iActoJudicialId"]);
            DataTable dtActas = new DataTable();
            DataTable dtTmpActas = new DataTable();

            crearTmpActas();
            dtTmpActas = (DataTable)Session["dtTmpActa"];
            ActaJudicialConsultaBL CLActas = new ActaJudicialConsultaBL();
            dtActas = CLActas.Obtener(iActoJudicialNotificacionId);




            if (dtActas.Rows.Count != 0)
            {
                for (intFila = 0; intFila <= dtActas.Rows.Count - 1; intFila++)
                {
                    DataRow row;
                    row = dtTmpActas.NewRow();

                    row["acjd_iActaJudicialId"] = dtActas.Rows[intFila]["acjd_iActaJudicialId"].ToString();
                    row["acjd_iActoJudicialNotificacionId"] = dtActas.Rows[intFila]["acjd_iActoJudicialNotificacionId"].ToString();
                    row["acjd_sTipoActaId"] = dtActas.Rows[intFila]["acjd_sTipoActaId"].ToString();
                    row["acjd_IFuncionarioFirmanteId"] = dtActas.Rows[intFila]["acjd_IFuncionarioFirmanteId"].ToString();
                    row["acjd_dFechaHoraActa"] = dtActas.Rows[intFila]["acjd_dFechaHoraActa"].ToString();
                    row["acjd_vCuerpoActa"] = dtActas.Rows[intFila]["acjd_vCuerpoActa"].ToString();
                    row["acjd_vResultado"] = dtActas.Rows[intFila]["acjd_vResultado"].ToString();
                    row["acjd_vObservaciones"] = dtActas.Rows[intFila]["acjd_vObservaciones"].ToString();
                    row["acjd_sEstadoId"] = dtActas.Rows[intFila]["acjd_sEstadoId"].ToString();
                    row["acjd_vEstado"] = dtActas.Rows[intFila]["acjd_vEstadoDescripcion"].ToString();
                    row["acjd_vResponsable"] = dtActas.Rows[intFila]["acjd_vResponsable"].ToString();
                    row["acjd_sGuardado"] = 1;
                    row["acde_sEstadoId"] = dtActas.Rows[intFila]["acde_sEstadoId"].ToString();

                    if (dtActas.Rows[intFila]["acjd_sTipoActaId"].ToString() == Convert.ToString((Int16)Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA))
                    {
                        row["acjd_vTipoActa"] = "COMPLEMENTARIA";
                    }
                    else
                    {
                        row["acjd_vTipoActa"] = "DILIGENCIAMIENTO";
                    }

                    dtTmpActas.Rows.Add(row);
                }
            }

            if (dtActas.Select("acjd_sEstadoId=" + Convert.ToInt32(Enumerador.enmJudicialActaEstado.REGISTRADO).ToString()).Length > 0)
            {
                ctrlToolBarActa.btnImprimir.Enabled = true;
            }
            else
            {
                ctrlToolBarActa.btnImprimir.Enabled = false;
            }

            Session["dtTmpActa"] = dtTmpActas;            // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            if (dtTmpActas.Rows.Count == 0)
            {
                ddlActaTipo.SelectedValue = Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_DILIGENCIAMIENTO).ToString();
                ddlActaTipo.Enabled = false;
            }
            else
            {
                ddlActaTipo.SelectedValue = Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString();
                ddlActaTipo.Enabled = false;
            }

            gdvActaDiligenciamiento.DataSource = dtTmpActas;
            gdvActaDiligenciamiento.DataBind();

            BlanqueaActas();

            updActas.Update();
        }

        void MostrarActoJudicial()
        {
            BE.RE_ACTOJUDICIAL BE_ActoJudicial = new BE.RE_ACTOJUDICIAL();
            ActoJudicialConsultaBL CL_ActoJudicial = new ActoJudicialConsultaBL();

            String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            Session["intActuacionId"] = 0;
            Int64 intActuacionId = 0;

            DateTime datFecha;
            int iActoJudicialId = Convert.ToInt32(Session["iActoJudicialId"]);

            BE_ActoJudicial = CL_ActoJudicial.Obtener(iActoJudicialId);

            cargarDatosDemandante();

            Session["ActoJudicialEstadoId"] = BE_ActoJudicial.acju_sEstadoId.ToString();

            txtNumExp.Text = BE_ActoJudicial.acju_vNumeroExpediente;
            txtNumHojRem.Text = BE_ActoJudicial.acju_vNumeroHojaRemision;

            if (BE_ActoJudicial.acju_dFechaRecepcion.ToString() != "")
            {
                datFecha = Comun.FormatearFecha(BE_ActoJudicial.acju_dFechaRecepcion.ToString());
                txtFchRecepcion.Text = datFecha.ToString(FormatoFechas);
            }

            txtManteria.Text = BE_ActoJudicial.acju_vMateriaDemanda;

            if (BE_ActoJudicial.acju_dFechaAudiencia.ToString() != "")
            {
                datFecha = Comun.FormatearFecha(BE_ActoJudicial.acju_dFechaAudiencia.ToString());
                txtFchAudiencia.Text = datFecha.ToString(FormatoFechas);
            }

            if (BE_ActoJudicial.acju_dFechaCitacion.ToString() != "")
            {
                datFecha = Comun.FormatearFecha(BE_ActoJudicial.acju_dFechaCitacion.ToString());
            }

            txtOrgano.Text = BE_ActoJudicial.acju_vOrganoJudicial;
            ddlTipoNotifica.SelectedValue = BE_ActoJudicial.acju_sTipoNotificacion.ToString();

            txtNumOficio.Text = BE_ActoJudicial.acju_vNumeroOficio;
            ddlEntSoli.SelectedValue = BE_ActoJudicial.acju_sEntidadSolicitanteId.ToString();
            txtObs.Text = BE_ActoJudicial.acju_vObservaciones;
            intActuacionId = BE_ActoJudicial.acju_iActuacionId;

            Session["intActuacionId"] = intActuacionId;
            Session["intActuacionEstadoId"] = BE_ActoJudicial.acju_sEstadoId;

            CargarParticipantes(BE_ActoJudicial);

            CargarPagos(intActuacionId);

            ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString();
            ddlMoneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();

            if (Convert.ToInt16(Session["IQueHace"]) != 3)
            {
                ValidarTipoNotificacion();
            }

            ctrlToolBar.btnConfiguration.Enabled = false;

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

            if (intOficinaConsularId == intOficinaConsularLimaId)                                                                // PREGUNTAMOS SI LA OFICINA ACTUAL ES LIMA
            {
                if (BE_ActoJudicial.acju_sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO))           // SI NO HA SIDO ENVIADO EL EXPEDIENTE ACTIVAMOS EL BOTON
                {
                    if (Convert.ToInt16(Session["IQueHace"]) == 2)
                    {
                        ctrlToolBar.btnConfiguration.Enabled = true;
                    }
                    else
                    {
                        ctrlToolBar.btnConfiguration.Enabled = false;
                    }
                }
            }

            // AVERIGUAMOS SI LAS ACTAS ETAN CERRADAS PARA ACTIVAR EL BOTON CERRAR EXPEDIENTE

            if (ParticipanteActasCerradas() == true)
            {
                if (intOficinaConsularId == intOficinaConsularLimaId)
                {
                    if (BE_ActoJudicial.acju_sEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                    {
                        ctrlToolBar.btnSalir.Enabled = false;
                    }
                    else
                    {
                        ctrlToolBar.btnSalir.Enabled = true;
                    }
                }
                else
                {
                    ctrlToolBar.btnSalir.Enabled = false;
                }
            }
            else
            {
                ctrlToolBar.btnSalir.Enabled = false;
            }
            updNotificados.Update();
        }

        string EstadoExpediente(int intEstadoId)
        {
            int intFila = 0;
            string strEstadoDescripcion = "";
            DataTable dtEstadoExpediente = new DataTable();
            dtEstadoExpediente = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.JUDICIAL_ESTADO_EXPEDIENTE);

            for (intFila = 0; intFila <= dtEstadoExpediente.Rows.Count - 1; intFila++)
            {
                if (dtEstadoExpediente.Rows[intFila]["id"].ToString() == intEstadoId.ToString())
                {
                    strEstadoDescripcion = dtEstadoExpediente.Rows[intFila]["descripcion"].ToString();
                    break;
                }
            }

            return strEstadoDescripcion;
        }

        void CargarCombos()
        {
            DataTable dtTarifa = new DataTable();
            DataTable dtTipDoc = new DataTable();
            DataTable dtEstadoExpediente = new DataTable();
            ActoJudicialConsultaBL funActoJudicial = new ActoJudicialConsultaBL();

            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);

            dtEstadoExpediente = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.JUDICIAL_ESTADO_ACTA);

            DataView dv = dtTipDoc.DefaultView;
            DataTable dtOrdenado = dv.ToTable();

            dtOrdenado.DefaultView.Sort = "Id ASC";

            EstadoConsultaBL Estado = new EstadoConsultaBL();
            DataTable dtResult = new DataTable();
            SGAC.Accesorios.Util FunUtil = new SGAC.Accesorios.Util();

            dtTarifa = funActoJudicial.Obtenertarifas();
            Session["dtTarifa"] = dtTarifa;

            Util.CargarParametroDropDownList(ddlTipPersona, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO), true);
            Util.CargarParametroDropDownList(ddlTipPersona3, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO), true);

            Util.CargarDropDownList(ddlTipDocumento, dtOrdenado, "Valor", "Id", true);
            Util.CargarDropDownList(ddlTipDoc3, dtOrdenado, "Valor", "Id", true);

            Util.CargarParametroDropDownList(ddlTipoNotifica, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_JUDICIAL_EXPEDIENTE_TIPO_NOTIFICACIÓN), true);
            Util.CargarParametroDropDownList(ddlEntSoli, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_JUDICIAL_EXPEDIENTE_ENTIDAD_SOLICITANTE), true);

            Util.CargarParametroDropDownList(ddlTipoRecepcion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_JUDICIAL_NOTIFICACION_TIPO_RECEPCIÓN), true);
            Util.CargarParametroDropDownList(ddlViaEnvio, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_JUDICIAL_NOTIFICACION_VIA_ENVÍO), true);

            Util.CargarParametroDropDownList(ddlActaTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_JUDICIAL_ACTA_TIPO), true);

            Util.CargarParametroDropDownList(ddlActaEstado, dtEstadoExpediente, true);

            Util.CargarDropDownList(ddlTarifaConsul, dtTarifa, "tari_vDescripcion", "tari_sTarifarioId", true);
            Util.CargarParametroDropDownList(ddlTipoPago, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);
            Util.CargarParametroDropDownList(ddlBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);

            DataTable dtMoneda = new DataTable();
            dtMoneda = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            Util.CargarParametroDropDownList(ddlMoneda, dtMoneda, true);
            
            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
            }

            ddlTarifaConsul.SelectedIndex = 0;
            ddlTipoPago.SelectedIndex = 0;
            ddlMoneda.SelectedIndex = 0;
            ddlBanco.SelectedIndex = 0;
            ddlActaEstado.SelectedIndex = 0;
        }

        void CrearTmpNotificaciones()
        {
            DataTable dtNotificacion = new DataTable();

            dtNotificacion.Columns.Add("ajno_iActoJudicialNotificacionId", typeof(int));
            dtNotificacion.Columns.Add("ajno_iActoJudicialParticipanteId", typeof(int));
            dtNotificacion.Columns.Add("ajno_sTipoRecepcionId", typeof(Int16));
            dtNotificacion.Columns.Add("ajno_sViaEnvioId", typeof(Int16));
            dtNotificacion.Columns.Add("ajno_vEmpresaServicioPostal", typeof(string));
            dtNotificacion.Columns.Add("ajno_vPersonaNotificacion", typeof(string));
            dtNotificacion.Columns.Add("ajno_dFechaHoraNotificacion", typeof(DateTime));
            dtNotificacion.Columns.Add("ajno_vNumeroCedula", typeof(string));
            dtNotificacion.Columns.Add("ajno_vPersonaRecibeNotificacion", typeof(string));
            dtNotificacion.Columns.Add("ajno_dFechaHoraRecepcion", typeof(DateTime));
            dtNotificacion.Columns.Add("ajno_vCuerpoNotificacion", typeof(string));
            dtNotificacion.Columns.Add("ajno_vObservaciones", typeof(string));
            dtNotificacion.Columns.Add("ajno_sEstadoId", typeof(Int16));
            dtNotificacion.Columns.Add("ajno_vViaEnvio", typeof(string));
            dtNotificacion.Columns.Add("ajno_sCorrelativo", typeof(Int16));
            dtNotificacion.Columns.Add("ajno_vEstadoDescripcion", typeof(string));
            dtNotificacion.Columns.Add("ajno_sGuardado", typeof(Int16));                                 // ESTE CAMPO INDICA QUE EL REGISTRO HA SIDO GUARDADO 1 = GUARDADO  0 = NO GUARDADO, POR DEFECTO CUANDO SE MUESTRA UNA NOTIFICACION ESTA SE MOSTRARA COMO GUARDADA
            dtNotificacion.Columns.Add("ajno_vEstadoInicial", typeof(string));
            dtNotificacion.Columns.Add("ajno_vTipRecepcion", typeof(string));
            dtNotificacion.Columns.Add("esta_vDescripcionCorta", typeof(string));
            Session["dtTmpNotificacion"] = dtNotificacion;
        }

        void crearTmpActas()
        {
            DataTable dtActas = new DataTable();

            dtActas.Columns.Add("acjd_iActaJudicialId", typeof(int));
            dtActas.Columns.Add("acjd_iActoJudicialNotificacionId", typeof(int));
            dtActas.Columns.Add("acjd_sTipoActaId", typeof(Int16));
            dtActas.Columns.Add("acjd_IFuncionarioFirmanteId", typeof(Int64));
            dtActas.Columns.Add("acjd_dFechaHoraActa", typeof(DateTime));
            dtActas.Columns.Add("acjd_vCuerpoActa", typeof(string));
            dtActas.Columns.Add("acjd_vTipoActa", typeof(string));

            dtActas.Columns.Add("acjd_vResultado", typeof(string));
            dtActas.Columns.Add("acjd_vObservaciones", typeof(string));
            dtActas.Columns.Add("acjd_sEstadoId", typeof(Int16));
            dtActas.Columns.Add("acjd_vEstado", typeof(string));
            dtActas.Columns.Add("acjd_vResponsable", typeof(string));
            dtActas.Columns.Add("acjd_sGuardado", typeof(Int16));                                 // ESTE CAMPO INDICA QUE EL REGISTRO HA SIDO GUARDADO 1 = GUARDADO  0 = NO GUARDADO, POR DEFECTO CUANDO SE MUESTRA UNA NOTIFICACION ESTA SE MOSTRARA COMO GUARDADA
            dtActas.Columns.Add("acde_sEstadoId", typeof(Int16));

            Session["dtTmpActa"] = dtActas;
        }

        void CrearTmpRePagos()
        {
            DataTable dtPagos = new DataTable();

            dtPagos.Columns.Add("pago_sId", typeof(Int32));
            dtPagos.Columns.Add("pago_iPagoId", typeof(Int64));
            dtPagos.Columns.Add("pago_sPagoTipoId", typeof(Int16));
            dtPagos.Columns.Add("pago_iActuacionDetalleId", typeof(Int64));
            dtPagos.Columns.Add("pago_dFechaOperacion", typeof(DateTime));
            dtPagos.Columns.Add("pago_sMonedaLocalId", typeof(Int16));
            dtPagos.Columns.Add("pago_FMontoMonedaLocal", typeof(float));
            dtPagos.Columns.Add("pago_FMontoSolesConsulares", typeof(float));
            dtPagos.Columns.Add("pago_FTipCambioBancario", typeof(float));
            dtPagos.Columns.Add("pago_FTipCambioConsular", typeof(float));
            dtPagos.Columns.Add("pago_sBancoId", typeof(Int16));
            dtPagos.Columns.Add("pago_vBancoNumeroOperacion", typeof(string));
            dtPagos.Columns.Add("pago_bPagadoFlag", typeof(bool));
            dtPagos.Columns.Add("pago_vComentario", typeof(string));
            dtPagos.Columns.Add("pago_sOficinaConsularId", typeof(Int16));
            dtPagos.Columns.Add("pago_sUsuarioCreacion", typeof(Int16));
            dtPagos.Columns.Add("pago_vIPCreacion", typeof(string));
            dtPagos.Columns.Add("pago_vHostName", typeof(string));
            dtPagos.Columns.Add("pago_sTarifarioId", typeof(Int16));
            dtPagos.Columns.Add("pago_sTarifarioDescripcion", typeof(string));
            dtPagos.Columns.Add("pago_vNumeroVoucher", typeof(string));
            dtPagos.Columns.Add("pago_iActuacionId", typeof(Int64));

            Session["dtTmpRePagos"] = dtPagos;
        }

        void CrearTemporales()
        {
            DataTable dtParticipante = new DataTable();

            dtParticipante.Columns.Add("ajpa_iActoJudicialParticipanteId", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_iActoJudicialId", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_sTipoParticipanteId", typeof(Int16));
            dtParticipante.Columns.Add("ajpa_sOficinaConsularDestinoId", typeof(Int16));
            dtParticipante.Columns.Add("ajpa_sTipoPersonaId", typeof(Int16));
            dtParticipante.Columns.Add("ajpa_iPersonaId", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_iEmpresaId", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_dFechaAceptacionExpediente", typeof(DateTime));
            dtParticipante.Columns.Add("ajpa_cEstado", typeof(string));
            dtParticipante.Columns.Add("ajpa_iNumero", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_dFechaNotificacion", typeof(DateTime));
            dtParticipante.Columns.Add("ajpa_vNroExpediente", typeof(string));
            dtParticipante.Columns.Add("ajpa_vEstadoExpediente", typeof(string));
            dtParticipante.Columns.Add("ajpa_vNombre", typeof(string));
            dtParticipante.Columns.Add("ajpa_vApePaterno", typeof(string));
            dtParticipante.Columns.Add("ajpa_vApeMaterno", typeof(string));
            dtParticipante.Columns.Add("ajpa_vNotificado", typeof(string));
            dtParticipante.Columns.Add("ajpa_vConsulado", typeof(string));
            dtParticipante.Columns.Add("ajpa_sDocumentoTipoId", typeof(Int16));
            dtParticipante.Columns.Add("ajpa_vDocumentoNumero", typeof(string));
            dtParticipante.Columns.Add("ajpa_iActuacionDetalleId", typeof(Int64));
            dtParticipante.Columns.Add("ajpa_dFechaLlegadaValija", typeof(DateTime));
            dtParticipante.Columns.Add("acde_sTarifarioId", typeof(Int64));
            dtParticipante.Columns.Add("tari_vDescripcionCorta", typeof(string));
            dtParticipante.Columns.Add("actu_dFechaRegistro", typeof(string));
            dtParticipante.Columns.Add("ajno_vEstadoPartipante", typeof(string));
            dtParticipante.Columns.Add("ajpa_sEstadoId", typeof(Int16));
            dtParticipante.Columns.Add("ajpa_vNumeroHojaRemision", typeof(string));
            dtParticipante.Columns.Add("ajpa_bActaFlag", typeof(bool));
            dtParticipante.Columns.Add("ajpa_bNotificacionFlag", typeof(bool));

            Session["dtTmpPartipante"] = dtParticipante;

            CrearTmpNotificaciones();
            crearTmpActas();
            CrearTmpRePagos();
        }

        void Blanquea()
        {
            // ************************
            // 1 = DATOS DEL DEMANDANTE
            ddlTipPersona.SelectedIndex = 0;
            ddlTipDocumento.SelectedIndex = 0;
            txtNroDoc1.Text = "";
            txtNom1.Text = "";
            txtDir1.Text = "";
            txtTel1.Text = "";
            txtCorreo1.Text = "";

            // ************************
            // 2 = DATOS DEL EXPEDIENTE
            txtNumExp.Text = "";
            txtNumHojRem.Text = "";
            txtFchRecepcion.Text = "";
            txtManteria.Text = "";
            txtFchAudiencia.Text = "";
            txtOrgano.Text = "";
            ddlTipoNotifica.SelectedIndex = 0;
            txtNumOficio.Text = "";
            ddlEntSoli.SelectedIndex = 0;
            txtObs.Text = "";

            BlanqueaPersonasNotifica();

            // **********************************
            // 4 = DATOS DE LA ACTUACION CONSULAR
            ddlTarifaConsul.SelectedIndex = 0;
            txtNumOrdPag.Text = "";
            txtFchPago.Text = "";
            txtCosto.Text = "";
            ddlTipoPago.SelectedIndex = 0;
            ddlMoneda.SelectedIndex = 0;
            txtCosto2.Text = "";
            ddlBanco.SelectedIndex = 0;
            txtNumCheque.Text = "";

            // ***************************
            // PERSONA/EMPRESA A NOTIFICAR
            ddlViaEnvio.SelectedIndex = 0;
            txtEmpPostal.Text = "";
            txtPersNotifica.Text = "";
            txtFechaNotifica.Text = "";
            txtHoraNotifica.Text = "";
            txtNroCedula.Text = "";
            ddlTipoRecepcion.SelectedIndex = 0;
            txtPerRecep.Text = "";
            txtFchRecep.Text = "";
            txtHoraRecep.Text = "";
            txtNotificacionCuerpo.Text = "";
            txtNotiObservacion.Text = "";

            // ***************************
            // PERSONA/EMPRESA A NOTIFICADA
            ddlActaTipo.SelectedIndex = 0;
            txtActaFecha.Text = "";
            txtActaHora.Text = "";
            ddlActaEstado.SelectedIndex = 0;
            txtActaResponsable.Text = "";
            txtActaCuerpo.Text = "";
            //---------------------------------------------------------------
            //Fecha: 17/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Eliminar el campo: Resultado.
            //---------------------------------------------------------------
            //txtResultado.Text = "";
            txtActaObservacion.Text = "";

            lblNombreNotificado.Text = "";
            lblNombreActa.Text = "";

            lblNombreActa.Text = "";
            lblNombreNotificado.Text = "";
        }

        void BlanqueaPersonasNotifica()
        {
            // *********************************
            // 3 = PERSONAS/EMPRESAS A NOTIFICAR
            ddlTipPersona3.SelectedIndex = 0;
            ddlTipDoc3.SelectedIndex = 0;

            txtNom3.Enabled = false;
            txtApePat3.Enabled = false;
            txtApeMat3.Enabled = false;

            txtNumDoc3.Text = "";
            txtNom3.Text = "";
            txtApePat3.Text = "";
            txtApeMat3.Text = "";
            txtFchValDip.Text = "";
            txtNumHojRem.Text = "";
            ctrlOficinaConsular1.SelectedValue = "0";
        }

        void BlanqueaPagos()
        {
            ddlTarifaConsul.SelectedIndex = 0;
            ddlTipoPago.SelectedIndex = 0;
            ddlMoneda.SelectedIndex = 0;
            ddlBanco.SelectedIndex = 0;

            txtNumOrdPag.Text = "";
            txtFchPago.Text = "";
            txtCosto.Text = "";
            txtCosto2.Text = "";
            txtNumCheque.Text = "";
            txtMotivoGratuidad.Text = "";
        }

        void BlanqueNotificaciones()
        {
            // ***************************
            // PERSONA/EMPRESA A NOTIFICAR
            ddlViaEnvio.SelectedIndex = 0;
            txtEmpPostal.Text = "";
            txtPersNotifica.Text = "";
            txtFechaNotifica.Text = "";
            txtHoraNotifica.Text = "";
            txtNroCedula.Text = "";
            ddlTipoRecepcion.SelectedIndex = 0;
            txtPerRecep.Text = "";
            txtFchRecep.Text = "";
            txtHoraRecep.Text = "";
            txtNotificacionCuerpo.Text = "";
            txtNotiObservacion.Text = "";
        }

        void BlanqueaActas()
        {
            //// *****
            //// ACTAS 
            txtActaFecha.Text = "";
            txtActaHora.Text = "";
            txtActaResponsable.Text = "";
            txtActaCuerpo.Text = "";
            //---------------------------------------------------------------
            //Fecha: 17/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Eliminar el campo: Resultado.
            //---------------------------------------------------------------
            //txtResultado.Text = "";
            txtActaObservacion.Text = "";

            btnActaAceptar.Text = "Adicionar";

            ddlActaEstado.SelectedValue = Convert.ToString(Convert.ToUInt16(Enumerador.enmJudicialActaEstado.REGISTRADO));
            updActas.Update();
        }

        void activarToolBar()
        {
            int intOficinaIdLima = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficinaIdActual = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intEstadoExpedienteId = Convert.ToInt16(Session["intActuacionEstadoId"]);

            gdvNotificaciones.Columns[8].Visible = false;                                   // ESTA COLUMNA YA NO DEBE DE MOSTRARSE SE CAMBIA POR EL BOTON FINALIZAR 

            if (intOficinaIdLima == intOficinaIdActual)                                     // SI LA OFICINA ACTUAL ES IGUAL A LIMA ACTIVAMOS LOS CONTROLES DE LA PESTAÑA 1 PARA EDICION  E INSERCCION
            {
                gdvNotificaciones.Columns[10].Visible = false;
                gdvNotificaciones.Columns[11].Visible = false;

                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = false;
                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = false;

                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = true;
                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = false;

                if (Convert.ToInt16(Session["IQueHace"]) == 3)
                {
                    ctrlToolBar.btnEditar.Enabled = true;
                    ctrlToolBar.btnGrabar.Enabled = false;

                    ctrlToolBar.btnConfiguration.Enabled = false;
                    ctrlToolBar.btnCancelar.Enabled = true;
                    if (intEstadoExpedienteId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                    {
                        gdvExpNotificados.Columns[8].Visible = true;
                    }
                    else
                    {
                        gdvExpNotificados.Columns[8].Visible = false;
                    }

                    gdvExpNotificados.Columns[9].Visible = false;

                    gdvPagos.Columns[4].Visible = false;
                    gdvPagos.Columns[5].Visible = false;
                }
                else
                {
                    ctrlToolBar.btnEditar.Enabled = false;
                    ctrlToolBar.btnGrabar.Enabled = true;
                    ctrlToolBar.btnCancelar.Enabled = true;
                }

                updNotificados.Update();

                ctrlToolBarNoti.btnEditar.Enabled = false;
                ctrlToolBarNoti.btnGrabar.Enabled = false;
                ctrlToolBarNoti.btnConfiguration.Enabled = false;
                updNotificaciones.Update();

                ctrlToolBarActa.btnGrabar.Enabled = false;
                ctrlToolBarActa.btnConfiguration.Enabled = false;
            }
            else
            {
                ctrlToolBar.btnEditar.Enabled = false;
                ctrlToolBar.btnGrabar.Enabled = false;
                ctrlToolBar.btnCancelar.Enabled = true;

                // SI ES CUALQUIER CONSULADO NO DEBE DE VISUALIZARCE LOS BOTONES 
                gdvExpNotificados.Columns[8].Visible = false;
                gdvExpNotificados.Columns[9].Visible = false;

                gdvPagos.Columns[4].Visible = false;
                gdvPagos.Columns[5].Visible = false;

                gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                updNotificados.Update();

                if (Convert.ToInt16(Session["IQueHace"]) == 3)
                {
                    ctrlToolBarNoti.btnEditar.Enabled = false;
                    ctrlToolBarNoti.btnGrabar.Enabled = false;
                    ctrlToolBarNoti.btnCancelar.Enabled = true;
                    ctrlToolBarNoti.btnConfiguration.Enabled = false;

                    gdvNotificaciones.Columns[10].Visible = false;
                    gdvNotificaciones.Columns[11].Visible = false;

                    updNotificaciones.Update();

                    ctrlToolBarActa.btnCancelar.Enabled = true;
                    ctrlToolBarActa.btnGrabar.Enabled = false;
                    ctrlToolBarActa.btnConfiguration.Enabled = false;

                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Editar")].Visible = false;
                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Imprimir")].Visible = false;
                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Observar")].Visible = false;
                    gdvActaDiligenciamiento.Columns[Util.ObtenerIndiceColumnaGrilla(gdvActaDiligenciamiento, "Anular")].Visible = false;
                    updActas.Update();
                }
                else
                {
                    ctrlToolBarNoti.btnEditar.Enabled = false;
                    ctrlToolBarNoti.btnGrabar.Enabled = true;
                    ctrlToolBarNoti.btnCancelar.Enabled = true;
                    ctrlToolBarNoti.btnConfiguration.Enabled = true;
                    updNotificaciones.Update();

                    ctrlToolBarActa.btnCancelar.Enabled = true;
                    ctrlToolBarActa.btnGrabar.Enabled = true;
                    ctrlToolBarActa.btnConfiguration.Enabled = true;
                    updActas.Update();
                }
            }
        }

        void Bloquea(bool bAction)
        {
            int intOficinaIdLima = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficinaIdActual = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (intOficinaIdLima == intOficinaIdActual)                                             // SI LA OFICINA ACTUAL ES IGUAL A LIMA ACTIVAMOS LOS CONTROLES DE LA PESTAÑA 1 PARA EDICION  E INSERCCION
            {
                txtNumExp.Enabled = bAction;
                txtNumHojRem.Enabled = bAction;
                txtManteria.Enabled = bAction;

                ddlTipoNotifica.Enabled = bAction;
                ddlEntSoli.Enabled = bAction;
                txtNumOficio.Enabled = bAction;
                txtObs.Enabled = bAction;

                txtFchRecepcion.Enabled = bAction;
                txtFchAudiencia.Enabled = bAction;

                //// *********************************
                //// 3 = PERSONAS/EMPRESAS A NOTIFICAR
                ddlTipPersona3.Enabled = bAction;
                ddlTipDoc3.Enabled = bAction;
                txtNumDoc3.Enabled = bAction;
                ctrlOficinaConsular1.Enabled = bAction;
                txtFchValDip.Enabled = bAction;

                imgBuscar.Enabled = bAction;
                btnAceptarNotifica.Enabled = bAction;

                btnCancelar.Enabled = bAction;


                txtNumOrdPag.Enabled = bAction;
                txtNumCheque.Enabled = bAction;

                txtFchPago.Enabled = bAction;
                ddlTarifaConsul.Enabled = bAction;
                ddlBanco.Enabled = bAction;

                btnAceptarPago.Enabled = bAction;
                btn_CancelarPago.Enabled = bAction;


                if (Session["ActoJudicialEstadoId"] != null)
                {
                    if (Session["ActoJudicialEstadoId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialExpedienteEstado.REGISTRADO).ToString())
                    {
                        BotonesEdicion(true);
                    }
                    else
                    {
                        BotonesEdicion(false);
                    }
                }

            }

            activarToolBar();
        }

        void BotonesEdicion(bool bAction)
        {
            btnCancelar.Enabled = bAction;
            btnAceptarNotifica.Enabled = bAction;
            ddlTipPersona3.Enabled = bAction;
            ddlTipDoc3.Enabled = bAction;
            txtNumDoc3.Enabled = bAction;
            imgBuscar.Enabled = bAction;

            txtFchValDip.Enabled = bAction;
            txtNumHojRem.Enabled = bAction;
            ctrlOficinaConsular1.Enabled = bAction;


            ddlTarifaConsul.Enabled = bAction;
            txtNumOrdPag.Enabled = bAction;
            txtFchPago.Enabled = bAction;

            ddlBanco.Enabled = bAction;
            txtNumCheque.Enabled = bAction;


            btnAceptarPago.Enabled = bAction;
            btn_CancelarPago.Enabled = bAction;
        }

        void Nuevo()
        {
            Bloquea(true);
            Blanquea();

            ddlTipPersona.Focus();

            CrearTemporales();
            DataTable dtParticipa = new DataTable();
            DataTable dtNotificacion = new DataTable();
            DataTable dtActa = new DataTable();

            int intPersonaTipoId = Convert.ToInt32(ViewState["intTipoPersona"]);                   // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS
            int intDemandanteId = Convert.ToInt32(Session["intDemandanteId"]);                     // DATA TABLE PARA ALMACENA LA LISTA DE ACTAS

            dtParticipa = (DataTable)Session["dtTmpPartipante"];

            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            dtActa = (DataTable)Session["dtTmpActa"];

            gdvExpNotificados.DataSource = dtParticipa;
            gdvExpNotificados.DataBind();

            gdvNotificaciones.DataSource = dtNotificacion;
            gdvNotificaciones.DataBind();

            gdvActaDiligenciamiento.DataSource = dtActa;
            gdvActaDiligenciamiento.DataBind();

            MostrarDatosDemandante(intPersonaTipoId, Convert.ToInt64(ViewState["intPersonaId"]));

            Session["intQueHace_Participante"] = 1;                                            // LE INDICAMOS A LA GRILLA DE PARTICIPANTES QUE SE AGREGARA UN NUEVO PARTICPANTE

            ddlTipoPago.SelectedValue = ((int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA).ToString();
            ddlMoneda.SelectedValue = Constantes.CONST_DOLAR_ID.ToString();

            ctrlToolBar.btnEditar.Enabled = false;
            ctrlToolBar.btnGrabar.Enabled = true;
            ctrlToolBar.btnCancelar.Enabled = true;
            ctrlToolBar.btnConfiguration.Enabled = false;
            updNotificados.Update();

            ctrlToolBarNoti.btnGrabar.Enabled = false;
            ctrlToolBarNoti.btnCancelar.Enabled = false;
            ctrlToolBarNoti.btnConfiguration.Enabled = false;
            updNotificaciones.Update();

            ctrlToolBarActa.btnCancelar.Enabled = false;
            ctrlToolBarActa.btnGrabar.Enabled = false;
            ctrlToolBarActa.btnConfiguration.Enabled = false;

            gdvExpNotificados.Columns[6].Visible = false;

            ctrlToolBar.btnSalir.Enabled = false;                        // DESACTIVAMOS EL BOTON CERRAR EXPEDIENTE

            updActas.Update();
        }

        void MostrarDatosDemandante(int intPersonaTipo, Int64 intDemandanteId)
        {
            EnPersona objEn = new EnPersona();

            if (intPersonaTipo == (int)Enumerador.enmTipoPersona.NATURAL)
            {
                objEn.iPersonaId = intDemandanteId;
                object[] arrParametros = { objEn };
                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

                ddlTipPersona.SelectedValue = objEn.sPersonaTipoId.ToString();
                ddlTipDocumento.SelectedValue = objEn.sDocumentoTipoId.ToString();
                txtNroDoc1.Text = objEn.vDocumentoNumero;
                txtNom1.Text = objEn.vApellidoPaterno + " " + objEn.vApellidoMaterno + ", " + objEn.vNombres;
                txtDir1.Text = objEn.vDireccion;
                txtTel1.Text = objEn.vTelefono;
                txtCorreo1.Text = objEn.vCorreoElectronico;
            }

            if (intPersonaTipo == (int)Enumerador.enmTipoPersona.JURIDICA)
            {
                Util.CargarParametroDropDownList(ddlTipDocumento, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO), true);

                SGAC.Registro.Persona.BL.PersonaConsultaBL funEmpresa = new SGAC.Registro.Persona.BL.PersonaConsultaBL();

                DataTable dtEmpresa = new DataTable();

                dtEmpresa = funEmpresa.Obtener_Empresa_Por_Id(Convert.ToInt32(intDemandanteId));

                if (dtEmpresa.Rows.Count != 0)
                {
                    ddlTipPersona.SelectedValue = dtEmpresa.Rows[0]["empr_cTipoPersona"].ToString();
                    ddlTipDocumento.SelectedValue = dtEmpresa.Rows[0]["empr_sTipoDocumentoId"].ToString();
                    txtNroDoc1.Text = dtEmpresa.Rows[0]["empr_vNumeroDocumento"].ToString();
                    txtNom1.Text = dtEmpresa.Rows[0]["empr_vRazonSocial"].ToString();
                    txtDir1.Text = dtEmpresa.Rows[0]["Direccion"].ToString();
                    txtTel1.Text = dtEmpresa.Rows[0]["empr_vTelefono"].ToString();
                    txtCorreo1.Text = dtEmpresa.Rows[0]["empr_vCorreo"].ToString();
                }
            }
        }

        bool HayPartipante(DataTable dtTmpParticipante)
        {
            bool booOk = false;
            int intNumeroregistros = dtTmpParticipante.Rows.Count;

            if (intNumeroregistros == 0)
            {
                booOk = false;
            }
            else
            {
                booOk = true;
            }

            return booOk;
        }

        bool GenerarActuacion(ref SGAC.BE.MRE.RE_ACTUACION Actuacion, ref List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE2, ref List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO2)
        {
            ActuacionMantenimientoBL FunActuacion = new ActuacionMantenimientoBL();
            SGAC.BE.MRE.RE_ACTUACION RE_AACTUACION = new SGAC.BE.MRE.RE_ACTUACION();
            List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE = new List<SGAC.BE.MRE.RE_ACTUACIONDETALLE>();
            List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO = new List<SGAC.BE.MRE.RE_PAGO>();

            DataTable dtDetActuacion = new DataTable();
            DataTable dtParticipantes = new DataTable();

            int intNumeroParticipante = 0;
            Int64 intPersonaId = Convert.ToInt64(ViewState["intPersonaId"]); ;
            int intFuncionarioid = 0;     //pasarle el id del funcionario
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string strHostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
            int intFila = 0;
            Int64 iActuacionId = 0;

            dtDetActuacion = CrearTmpActuacionDetalle();                                                     // CREAMOS EL DATATABLE TEMPORAL PARA EL DETALLE DE LAS ACTUACIONES
            dtParticipantes = (DataTable)Session["dtTmpPartipante"];

            DataTable dtTmpRePagos = new DataTable();
            dtTmpRePagos = (DataTable)Session["dtTmpRePagos"];

            if (dtParticipantes.Rows.Count != 0)
            {
                if (dtParticipantes.Rows.Count == 1)
                { intNumeroParticipante = 1; }
                else
                { intNumeroParticipante = dtParticipantes.Rows.Count; }
            }

            if (Convert.ToInt16(Session["IQueHace"]) == 1)
            {
                iActuacionId = 0;
            }
            else
            {
                iActuacionId = (dtTmpRePagos.Rows[0]["pago_iActuacionId"].ToString() == "" ? 0 : Convert.ToInt64(dtTmpRePagos.Rows[0]["pago_iActuacionId"].ToString()));
            }

            #region CARGAMOS LA TABLA RE_ACTUACION
            // CARGAMOS LA TABLA RE_ACTUACION
            RE_AACTUACION.actu_iActuacionId = iActuacionId;
            RE_AACTUACION.actu_sOficinaConsularId = intOficinaConsularId;
            RE_AACTUACION.actu_FCantidad = intNumeroParticipante;

            if (ddlTipPersona.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.NATURAL).ToString())
            {
                RE_AACTUACION.actu_iPersonaRecurrenteId = intPersonaId;
                RE_AACTUACION.actu_iEmpresaRecurrenteId = 0;
            }
            else
            {
                RE_AACTUACION.actu_iPersonaRecurrenteId = 0;
                RE_AACTUACION.actu_iEmpresaRecurrenteId = intPersonaId;
            }
            //RE_AACTUACION.actu_iEmpresaRecurrenteId = 0;
            RE_AACTUACION.actu_IFuncionarioId = intFuncionarioid;
            RE_AACTUACION.actu_dFechaRegistro = ObtenerFechaActual(HttpContext.Current.Session);
            RE_AACTUACION.actu_sEstado = Convert.ToInt16(Enumerador.enmActuacionEstado.CANCELADO);
            RE_AACTUACION.actu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            RE_AACTUACION.actu_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            RE_AACTUACION.actu_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);

            RE_AACTUACION.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            RE_AACTUACION.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            RE_AACTUACION.actu_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);

            RE_AACTUACION.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
            RE_AACTUACION.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            #endregion

            #region CARGAMOS LA TABLA RE_AACTUACIONDETALLE
            //CARGAMOS LA TABLA RE_AACTUACIONDETALLE
            Int16 sTarifarioId = 0;
            int sCorrelativo = 0;

            if (dtTmpRePagos.Rows.Count != 0)
            {
                for (intFila = 0; intFila <= dtTmpRePagos.Rows.Count - 1; intFila++)
                {
                    SGAC.BE.MRE.RE_ACTUACIONDETALLE RE_AACTUACIONDETALLE = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();

                    sCorrelativo = sCorrelativo + 1;

                    //if (intFila == 0) { sTarifarioId = 78; }   // TARIFA 32A
                    //if (intFila == 1) { sTarifarioId = 79; }   // TARIFA 32B

                    sTarifarioId = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sTarifarioId"].ToString());

                    if ((dtTmpRePagos.Rows[intFila]["pago_iActuacionDetalleId"].Equals(System.DBNull.Value)) == true)
                    {
                        RE_AACTUACIONDETALLE.acde_iActuacionDetalleId = 0;
                    }
                    else
                    {
                        RE_AACTUACIONDETALLE.acde_iActuacionDetalleId = Convert.ToInt32(dtTmpRePagos.Rows[intFila]["pago_iActuacionDetalleId"].ToString());
                    }

                    RE_AACTUACIONDETALLE.acde_IFuncionarioAnulaId = 0; //Convert.ToInt32(dtParticipantes.Rows[intFila]["ajpa_iPersonaId"].ToString());
                    RE_AACTUACIONDETALLE.acde_iActuacionId = iActuacionId;                              // ESTE DATO SE ACTUALIZARA EN EL STORE CUANDO SE ESTE AGREGANDO UN REGISTRO
                    RE_AACTUACIONDETALLE.acde_sTarifarioId = sTarifarioId;
                    RE_AACTUACIONDETALLE.acde_sItem = Convert.ToInt16(sCorrelativo);
                    RE_AACTUACIONDETALLE.acde_dFechaRegistro = ObtenerFechaActual(HttpContext.Current.Session);
                    RE_AACTUACIONDETALLE.acde_bRequisitosFlag = false;                       // INDICAMOS A LA ACTUACION QUE NO TIENE TRAMITES
                    RE_AACTUACIONDETALLE.acde_IFuncionarioFirmanteId = 0;                    // ESTE DATO SE TRAERA DE LA BD DE MRE
                    RE_AACTUACIONDETALLE.acde_IFuncionarioContactoId = 0;                    // ESTE DATO SE TRAERA DE LA BD DE MRE
                    RE_AACTUACIONDETALLE.acde_vNotas = "ACTUACION GENERADA AUTOMATICAMENTE POR EL PROCESO DE ACTAS JUDICIALES";
                    RE_AACTUACIONDETALLE.acde_sEstadoId = Convert.ToInt16(Enumerador.enmActuacionEstado.REGISTRADO);                           // LE INDICAMOS EL ESTADO DE LA ACTUACION
                    RE_AACTUACIONDETALLE.acde_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_AACTUACIONDETALLE.acde_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_AACTUACIONDETALLE.acde_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);
                    RE_AACTUACIONDETALLE.acde_iReferenciaId = null;

                    RE_AACTUACIONDETALLE.acde_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_AACTUACIONDETALLE.acde_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_AACTUACIONDETALLE.acde_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);

                    RE_AACTUACIONDETALLE.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                    RE_AACTUACIONDETALLE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    LIS_RE_ACTUACIONDETALLE.Add(RE_AACTUACIONDETALLE);
                }
            }
            #endregion

            #region LLENAMOS EL OBJETO RE_PAGOS
            // LLENAMOS EL OBJETO RE_PAGOS
            dtTmpRePagos = (DataTable)Session["dtTmpRePagos"];

            int intNumReg = dtTmpRePagos.Rows.Count - 1;

            for (intFila = 0; intFila <= dtParticipantes.Rows.Count - 1; intFila++)
            {
                SGAC.BE.MRE.RE_PAGO Pagos = new SGAC.BE.MRE.RE_PAGO();

                Pagos.pago_iPagoId = Convert.ToInt64(dtTmpRePagos.Rows[intFila]["pago_iPagoId"].ToString());
                Pagos.pago_sPagoTipoId = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sPagoTipoId"].ToString());
                Pagos.pago_iActuacionDetalleId = 0;
                Pagos.pago_dFechaOperacion = Comun.FormatearFecha(dtTmpRePagos.Rows[intFila]["pago_dFechaOperacion"].ToString());
                Pagos.pago_sMonedaLocalId = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sMonedaLocalId"].ToString());
                Pagos.pago_FMontoMonedaLocal = Convert.ToDouble(dtTmpRePagos.Rows[intFila]["pago_FMontoMonedaLocal"].ToString());
                Pagos.pago_FMontoSolesConsulares = Convert.ToDouble(dtTmpRePagos.Rows[intFila]["pago_FMontoSolesConsulares"].ToString());
                Pagos.pago_FTipCambioBancario = Convert.ToDouble(dtTmpRePagos.Rows[intFila]["pago_FTipCambioBancario"].ToString());
                Pagos.pago_FTipCambioConsular = Convert.ToDouble(dtTmpRePagos.Rows[intFila]["pago_FTipCambioConsular"].ToString());
                Pagos.pago_vBancoNumeroOperacion = dtTmpRePagos.Rows[intFila]["pago_vBancoNumeroOperacion"].ToString();
                Pagos.pago_bPagadoFlag = Convert.ToBoolean(dtTmpRePagos.Rows[intFila]["pago_bPagadoFlag"].ToString());
                Pagos.pago_vComentario = dtTmpRePagos.Rows[intFila]["pago_vComentario"].ToString();
                Pagos.pago_sUsuarioCreacion = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sUsuarioCreacion"].ToString());
                Pagos.pago_vIPCreacion = dtTmpRePagos.Rows[intFila]["pago_vIPCreacion"].ToString();
                Pagos.pago_vNumeroVoucher = dtTmpRePagos.Rows[intFila]["pago_vNumeroVoucher"].ToString();

                if (dtTmpRePagos.Rows[intFila]["pago_sBancoId"].ToString().Trim() == string.Empty)
                {
                    Pagos.pago_sBancoId = null;
                }
                else if (Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sBancoId"].ToString()) == 0)
                {
                    Pagos.pago_sBancoId = null;
                }
                else
                {
                    Pagos.pago_sBancoId = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sBancoId"].ToString());
                }

                Pagos.pago_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);

                if (Convert.ToInt16(Session["IQueHace"]) == 1)
                {
                    Pagos.pago_sUsuarioModificacion = null;
                    Pagos.pago_vIPModificacion = null;
                    Pagos.pago_dFechaModificacion = null;
                }
                else
                {
                    Pagos.pago_sUsuarioModificacion = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sUsuarioCreacion"].ToString());
                    Pagos.pago_vIPModificacion = dtTmpRePagos.Rows[intFila]["pago_vIPCreacion"].ToString();
                    Pagos.pago_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                }

                Pagos.pago_cEstado = "A";
                Pagos.OficinaConsultar = Convert.ToInt16(dtTmpRePagos.Rows[intFila]["pago_sOficinaConsularId"].ToString());
                Pagos.HostName = dtTmpRePagos.Rows[intFila]["pago_vHostName"].ToString();
                LIS_RE_PAGO.Add(Pagos);
            }
            #endregion

            LIS_RE_ACTUACIONDETALLE2 = LIS_RE_ACTUACIONDETALLE;
            LIS_RE_PAGO2 = LIS_RE_PAGO;
            Actuacion = RE_AACTUACION;                // DEVOLVEMOS EL OBJETO ACTUACION

            return true;
        }

        DataTable CrearTmpActuacionDetalle()
        {
            DataTable dtActuacionDetalle = new DataTable();
            dtActuacionDetalle.Columns.Add("acde_iActuacionDetalleId", typeof(Int64));
            dtActuacionDetalle.Columns.Add("acde_iActuacionId", typeof(Int64));
            dtActuacionDetalle.Columns.Add("acde_sTarifarioId", typeof(Int16));
            dtActuacionDetalle.Columns.Add("acde_sItem", typeof(Int16));
            dtActuacionDetalle.Columns.Add("acde_dFechaRegistro", typeof(DateTime));
            dtActuacionDetalle.Columns.Add("acde_bRequisitosFlag", typeof(bool));
            dtActuacionDetalle.Columns.Add("acde_ICorrelativoActuacion", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_ICorrelativoTarifario", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_IFuncionarioFirmanteId", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_IFuncionarioContactoId", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_IImpresionFuncionarioId", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_vNotas", typeof(string));
            dtActuacionDetalle.Columns.Add("acde_IFuncionarioAnulaId", typeof(Int32));
            dtActuacionDetalle.Columns.Add("acde_vMotivoAnulacion", typeof(string));
            dtActuacionDetalle.Columns.Add("acde_iReferenciaId", typeof(Int64));
            dtActuacionDetalle.Columns.Add("acde_sEstadoId", typeof(Int16));
            dtActuacionDetalle.Columns.Add("acde_sUsuarioCreacion", typeof(Int16));
            dtActuacionDetalle.Columns.Add("acde_vIPCreacion", typeof(string));
            dtActuacionDetalle.Columns.Add("acde_dFechaCreacion", typeof(DateTime));
            dtActuacionDetalle.Columns.Add("acde_sUsuarioModificacion", typeof(Int16));
            dtActuacionDetalle.Columns.Add("acde_vIPModificacion", typeof(string));
            dtActuacionDetalle.Columns.Add("acde_dFechaModificacion", typeof(DateTime));

            return dtActuacionDetalle;
        }

        bool Existe32A(DataTable dtTMPPagos)
        {
            bool booOk = false;
            int intFila = 0;
            Int64 intTarida32A = 78;
            Int64 intTarida33A = 81;

            for (intFila = 0; intFila <= dtTMPPagos.Rows.Count - 1; intFila++)
            {
                Int64 intTarifarioId = Convert.ToInt64(dtTMPPagos.Rows[intFila]["pago_sTarifarioId"].ToString());

                if (intTarida32A == intTarifarioId) { booOk = true; break; }
                if (intTarida33A == intTarifarioId) { booOk = true; break; }
            }

            return booOk;
        }

        bool GenerarRuneRapido(ref DataTable dtParticipante, Int16 intOficinaId, string strHostName, Int16 intUsuarioCreacionId, string strIPCreacion)
        {
            SGAC.Registro.Persona.BL.PersonaMantenimientoBL funRune = new SGAC.Registro.Persona.BL.PersonaMantenimientoBL();
            bool booOk = false;
            DataTable dtParticipantes = new DataTable();
            dtParticipantes = (DataTable)Session["dtTmpPartipante"];

            booOk = funRune.InsertarRuneRapido(ref dtParticipantes, intOficinaId, strHostName, intUsuarioCreacionId, strIPCreacion);
            return booOk;
        }

        bool GrabarPestana1()
        {
            try
            {
                bool bolOk = false;
                SGAC.BE.MRE.RE_ACTOJUDICIAL RE_ACTOJUDICIAL = new SGAC.BE.MRE.RE_ACTOJUDICIAL();
                List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE> PARTICIPANTE_LISTA = new List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE>();
                SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION = new SGAC.BE.MRE.RE_ACTUACION();
                List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO = new List<SGAC.BE.MRE.RE_PAGO>();
                List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE = new List<SGAC.BE.MRE.RE_ACTUACIONDETALLE>();

                Proceso MiProc = new Proceso();
                DataTable dtParticipante = new DataTable();
                DataTable dtTmpRePagos = new DataTable();

               // DateTime datFchRecep = txtFchRecepcion.Value();
               // DateTime datFchAudi = txtFchAudiencia.Value();

                int intQueHace = Convert.ToInt32(Session["IQueHace"]);                   // VARIABLE QUE NOS INDICA EN QUE MODO SE ENCUENTRA EL FORMULARIO
                int intPersonaId = Convert.ToInt32(ViewState["intPersonaId"]);             // VARIABLE QUE ALMACENA EL ID DEL DEMANDANTE
                Int64 intActuacionId = 0;
                Int64 intActoJudicialId = Convert.ToInt64(Session["intActoJudicialId"]);

                #region Validar si existen demandados

                dtParticipante = (DataTable)Session["dtTmpPartipante"];
                if (HayPartipante(dtParticipante) == false)
                {
                    ValParticipante.MostrarValidacion("No ha ingresado Demandados para este expediente, Debe ingresar 1 Demandado como mínimo", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return false;
                }
                if (txtFchAudiencia.Text.Trim().Length > 0)
                {
                    if (Comun.EsFecha(txtFchAudiencia.Text.Trim()) == false)
                    {
                        ValParticipante.MostrarValidacion("La fecha de la audiencia no es válida.", true, Enumerador.enmTipoMensaje.WARNING);
                        return false;
                    }
                }
                if (txtFchRecepcion.Text.Trim().Length > 0)
                {
                    if (Comun.EsFecha(txtFchRecepcion.Text.Trim()) == false)
                    {
                        ValParticipante.MostrarValidacion("La fecha de recepción no es válida.", true, Enumerador.enmTipoMensaje.WARNING);
                        return false;
                    }
                }
                #endregion

                #region Validamos si el numero de demandado coincide con el mumero de pago

                dtTmpRePagos = (DataTable)Session["dtTmpRePagos"];

                if (CantidadParticipanteYPagosSoncorrectos(dtParticipante, dtTmpRePagos) == false)
                {
                    ValParticipante.MostrarValidacion("El importe Abonado no concuerda con el número de Participantes, verifique el Abono realizado", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return false;
                }
                #endregion

                #region validamos la fecha de audiencia no sea menor a la fecha de recepcion
                //------------------------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                //           y la fecha de salida de valija
                // Fecha: 13/01/2017
                //------------------------------------------------------------------

                //if (datFchRecep > datFchAudi)
                //{
                //    Validation1.MostrarValidacion("La fecha de audiencia no puede ser menor a la fecha de recepción", true, Enumerador.enmTipoMensaje.WARNING);
                //    updNotificados.Update();
                //    return false;
                //}

                //if (datFchRecep > datFchCita)
                //{
                //    Validation1.MostrarValidacion("La fecha de citación no puede ser menor a la fecha de recepción", true, Enumerador.enmTipoMensaje.WARNING);
                //    updNotificados.Update();
                //    return false;
                //}
                #endregion

                #region VERIFICAMOS QUE EN LA LISTA DE PAGOS HAYA UNA (32 A) COMO MINIMO
                if (Existe32A(dtTmpRePagos) == false)
                {
                    Validation1.MostrarValidacion("Debe agregar al menos una tarifa 32A para poder seguir con el expediente", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return false;
                }

                #endregion

                int intFila = 0;

                Int16 intTipDemandante = Convert.ToInt16(Enumerador.enmJudicialTipoParticipante.DEMANDANTE);


                #region OBTENEMOS LOS OBJETOS RE_ACTUACION Y RE_ACTUACIONDETALLE
                // SI ESTA EN MODO NUEVO, AGREGAMOS LA ACTUACION
                //if (GenerarActuacion(ref intActuacionId, ref Actuacion, ref LIS_RE_ACTUACIONDETALLE) == false)

                if (GenerarActuacion(ref RE_ACTUACION, ref LIS_RE_ACTUACIONDETALLE, ref LIS_RE_PAGO) == false)
                {
                    Validation1.MostrarValidacion("No se pudo generar la actuación para este acto judicial, se cancela el proceso de grabación", true, Enumerador.enmTipoMensaje.WARNING);
                    updNotificados.Update();
                    return false;
                }

                if (intQueHace != 1)
                {
                    intActuacionId = Convert.ToInt64(Session["intActuacionId"]);
                }
                #endregion

                #region LLENAMOS LOS OBJETOS DE ACTO JUDICIAL (RE_ACTOJUDICIAL, RE_ACTOJUDICIALPARTICIPANTE )
                // **********************************
                // LLENAMOS EL OBJETO RE_ACTOJUDICIAL
                RE_ACTOJUDICIAL.acju_iActoJudicialId = intActoJudicialId;                                       // ESTE DATO ES 0 SE AUTOGENERA AL MOMENTO DE GRABAR EL REGISTRO
                RE_ACTOJUDICIAL.acju_iActuacionId = intActuacionId;                                             // ESTE DATO VIENE DE ACTUACION  
                RE_ACTOJUDICIAL.acju_sTipoNotificacion = Convert.ToInt16(ddlTipoNotifica.SelectedValue);
                RE_ACTOJUDICIAL.acju_sEntidadSolicitanteId = Convert.ToInt16(ddlEntSoli.SelectedValue);

                //------------------------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                //           y la fecha de salida de valija
                // Fecha: 13/01/2017
                //------------------------------------------------------------------

                if (txtFchRecepcion.Text.Trim().Length == 0)
                {
                    RE_ACTOJUDICIAL.acju_dFechaRecepcion = null;
                }
                else
                {
                    RE_ACTOJUDICIAL.acju_dFechaRecepcion = txtFchRecepcion.Value();
                }

                if (txtFchAudiencia.Text.Trim().Length == 0)
                {
                    RE_ACTOJUDICIAL.acju_dFechaAudiencia = null;
                }
                else
                {
                    RE_ACTOJUDICIAL.acju_dFechaAudiencia = txtFchAudiencia.Value();
                }
                //------------------------------------------------------------------

                RE_ACTOJUDICIAL.acju_dFechaRegistro = ObtenerFechaActual(HttpContext.Current.Session);
                RE_ACTOJUDICIAL.acju_vJuzgado = txtOrgano.Text;

                RE_ACTOJUDICIAL.acju_vNumeroExpediente = txtNumExp.Text;
                RE_ACTOJUDICIAL.acju_vMateriaDemanda = txtManteria.Text;
                RE_ACTOJUDICIAL.acju_vNumeroOficio = txtNumOficio.Text;
                RE_ACTOJUDICIAL.acju_vObservaciones = txtObs.Text;
                RE_ACTOJUDICIAL.acju_sEstadoId = Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO);                //ESTADO DEL ACTA 55 = SIN NOTIFICAR (VER TABLA MA_ESTADO)
                RE_ACTOJUDICIAL.acju_vOrganoJudicial = txtOrgano.Text;
                if (intQueHace == 1)
                {
                    RE_ACTOJUDICIAL.acju_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIAL.acju_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIAL.acju_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);
                }
                if (intQueHace == 2)
                {
                    RE_ACTOJUDICIAL.acju_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIAL.acju_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIAL.acju_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                }

                RE_ACTOJUDICIAL.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                RE_ACTOJUDICIAL.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];


                // **********************************************
                // LLENAMOS EL OBJETO RE_ACTOJUDICIALPARTICIPANTE

                if (Session["dtTmpPartipantesEliminados"] != null)
                {
                    DataTable dtParticipantesEliminados = (DataTable)Session["dtTmpPartipantesEliminados"];

                    if (dtParticipantesEliminados.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtParticipantesEliminados.Rows)
                        {
                            dtParticipante.ImportRow(dr);
                        }
                    }
                }

                for (intFila = 0; intFila <= dtParticipante.Rows.Count - 1; intFila++)
                {
                    SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE RE_ACTOJUDICIALPARTICIPANTE = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();

                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_iActoJudicialParticipanteId = Convert.ToInt32(dtParticipante.Rows[intFila]["ajpa_iActoJudicialParticipanteId"].ToString());
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_iActoJudicialId = Convert.ToInt32(dtParticipante.Rows[intFila]["ajpa_iActoJudicialId"].ToString());
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sTipoParticipanteId = Convert.ToInt16(dtParticipante.Rows[intFila]["ajpa_sTipoParticipanteId"].ToString());
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sOficinaConsularDestinoId = Convert.ToInt16(dtParticipante.Rows[intFila]["ajpa_sOficinaConsularDestinoId"].ToString());
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sTipoPersonaId = Convert.ToInt16(dtParticipante.Rows[intFila]["ajpa_sTipoPersonaId"].ToString());

                    if (RE_ACTOJUDICIALPARTICIPANTE.ajpa_sTipoPersonaId == (short)Enumerador.enmTipoPersona.NATURAL)
                        RE_ACTOJUDICIALPARTICIPANTE.ajpa_iPersonaId = Convert.ToInt32(dtParticipante.Rows[intFila]["ajpa_iPersonaId"].ToString());
                    else
                        RE_ACTOJUDICIALPARTICIPANTE.ajpa_iEmpresaId = Convert.ToInt32(dtParticipante.Rows[intFila]["ajpa_iEmpresaId"].ToString());


                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaAceptacionExpediente = Comun.FormatearFecha(dtParticipante.Rows[intFila]["ajpa_dFechaAceptacionExpediente"].ToString());

                    //RE_ACTOJUDICIALPARTICIPANTE.ajpa_cEstado = Convert.ToString(dtParticipante.Rows[intFila]["ajpa_cEstado"].ToString());
                    //// *****************************************************************************************************************************************************************
                    // IDM-AGREGADO 08/03/2015
                    //RE_ACTOJUDICIALPARTICIPANTE.ajpa_iActuacionDetalleId = 1;     //// esto se debe de cambiar por id de la actuacion cuando se genere las actuacion por los participantes
                    //// *****************************************************************************************************************************************************************

                    if (dtParticipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString() != "")
                    {
                        RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaLlegadaValija = Comun.FormatearFecha(dtParticipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString());
                    }
                    else
                    {
                        RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaLlegadaValija = null;
                    }

                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);

                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);

                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_sEstadoId = Convert.ToInt16(dtParticipante.Rows[intFila]["ajpa_sEstadoId"].ToString());
                    RE_ACTOJUDICIALPARTICIPANTE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                    RE_ACTOJUDICIALPARTICIPANTE.ajpa_vNumeroHojaRemision = dtParticipante.Rows[intFila]["ajpa_vNumeroHojaRemision"].ToString();

                    PARTICIPANTE_LISTA.Add(RE_ACTOJUDICIALPARTICIPANTE);
                }
                #endregion

                #region AGREGAMOS AL DEMANDANTE COMO PARTICIPANTE, EL DEMANDANTE TENDRA EL CAMPO ajpa_sTipoParticipanteId = 8541
                // ********************************************************************************************************
                // AGREGAMOS AL DEMANDANTE COMO PARTICIPANTE, EL DEMANDANTE TENDRA EL CAMPO ajpa_sTipoParticipanteId = 8541

                if (intQueHace == 1)                                 // PREGUNTAMOS SI SE ESTA AGREGANDO UN NUEVO REGISTRO JUDICIAL PARA AGREGAR EL DEMANDANTE
                {
                    SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE RE_ACTOJUDICIALPARTICIPANTE2 = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();

                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iActoJudicialParticipanteId = -99;
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iActoJudicialId = 0;
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sTipoParticipanteId = intTipDemandante;
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sOficinaConsularDestinoId = null;
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sTipoPersonaId = Convert.ToInt16(ddlTipPersona.SelectedValue);

                    // MDIAZ - 20150301 - Convert.toInt32(ddlTipPersona.SelectedValue) == (int)Enumerador.enmTipoPersona.NATURAL;
                    if (ddlTipPersona.SelectedValue == "2101")
                    {
                        RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iPersonaId = intPersonaId;
                        RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iEmpresaId = null;
                    }
                    else
                    {
                        RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iPersonaId = null;
                        RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iEmpresaId = intPersonaId;
                    }

                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_dFechaAceptacionExpediente = null;
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sEstadoId = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.REGISTRADO);
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_dFechaLlegadaValija = null;

                    //// *****************************************************************************************************
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_iActuacionDetalleId = null;       //// esto se debe de cambiar por id de la actuacion cuando se genere las actuacion por los participantes
                    //// *****************************************************************************************************

                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);

                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);

                    RE_ACTOJUDICIALPARTICIPANTE2.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    RE_ACTOJUDICIALPARTICIPANTE2.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
                    RE_ACTOJUDICIALPARTICIPANTE2.ajpa_vNumeroHojaRemision = null;
                    PARTICIPANTE_LISTA.Add(RE_ACTOJUDICIALPARTICIPANTE2);
                }
                #endregion

                // INICIAMOS EL PROCESO DE ESCRITURA DE LOS DATOS
                SGAC.Registro.Actuacion.BL.ActoJudicialMantenimientoBL miFun = new SGAC.Registro.Actuacion.BL.ActoJudicialMantenimientoBL();
                int intResult = 0;

                if (intQueHace == 1)
                {

                    AgregarParticipanteRecurrente(PARTICIPANTE_LISTA);

                    intResult = miFun.Insertar(ref RE_ACTOJUDICIAL, PARTICIPANTE_LISTA, RE_ACTUACION, LIS_RE_ACTUACIONDETALLE, LIS_RE_PAGO, dtParticipante);
                }
                else
                {
                    intResult = miFun.Actualizar(ref RE_ACTOJUDICIAL, PARTICIPANTE_LISTA, RE_ACTUACION, LIS_RE_ACTUACIONDETALLE, LIS_RE_PAGO, dtParticipante);
                }


                if (intResult == 1)
                {

                    Session["intActoJudicialId"] = RE_ACTOJUDICIAL.acju_iActoJudicialId;
                    Session["iActoJudicialId"] = RE_ACTOJUDICIAL.acju_iActoJudicialId;
                    Session["dtTmpPartipantesEliminados"] = null;

                    string strValor = Convert.ToString(Session["sActoJudicialId"]);
                    string[] strDato = strValor.Split('-');

                    Session["sActoJudicialId"] = RE_ACTOJUDICIAL.acju_iActoJudicialId + "-" + "2" + "-" + strDato[2] + "-" + strDato[3] + "-" + strDato[4];

                    bolOk = true;
                }
                return bolOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void AgregarParticipanteRecurrente(List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE> listaParticipantes)
        {

            Int64 intiActoJudicialParticipanteId = 0;

            DataTable dtParticipante = (DataTable)Session["dtTmpPartipante"];

            DateTime? datFchLlegada;
            DateTime? datFchRecepcion = txtFchRecepcion.Value();

            if (txtFchValDip.Text != "")
            {
                datFchLlegada = Comun.FormatearFecha(txtFchValDip.Value().ToString());
            }
            else
            {
                datFchLlegada = null;
            }

            if (dtParticipante.Rows.Count != 0)
            {
                DataTable dtTMP = new DataTable();
                DataView dvResult = dtParticipante.DefaultView;
                dvResult.Sort = "ajpa_iActoJudicialParticipanteId ASC";

                if (Convert.ToInt32(dvResult[0]["ajpa_iActoJudicialParticipanteId"]) > 0)
                    intiActoJudicialParticipanteId = -1;
                else
                    intiActoJudicialParticipanteId = (Convert.ToInt32(dvResult[0]["ajpa_iActoJudicialParticipanteId"].ToString()) - 1);

            }
            else
            {
                intiActoJudicialParticipanteId = -1;
            }



            SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE RE_ACTOJUDICIALPARTICIPANTE = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_iActoJudicialParticipanteId = intiActoJudicialParticipanteId;
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_iActoJudicialId = 0;
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_sTipoParticipanteId = Convert.ToInt16(Enumerador.enmJudicialTipoParticipante.RECURRENTE);



            RE_ACTOJUDICIALPARTICIPANTE.ajpa_sTipoPersonaId = Convert.ToInt16(ViewState["iTipoId"].ToString());

            RE_ACTOJUDICIALPARTICIPANTE.ajpa_iPersonaId = Convert.ToInt32(ViewState["iPersonaId"].ToString());

            RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaAceptacionExpediente = ObtenerFechaActual(HttpContext.Current.Session);

            RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaLlegadaValija = datFchLlegada;


            RE_ACTOJUDICIALPARTICIPANTE.ajpa_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);

            RE_ACTOJUDICIALPARTICIPANTE.ajpa_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);

            RE_ACTOJUDICIALPARTICIPANTE.ajpa_sEstadoId = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.REGISTRADO);
            RE_ACTOJUDICIALPARTICIPANTE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            RE_ACTOJUDICIALPARTICIPANTE.HostName = (string)Session[Constantes.CONST_SESION_HOSTNAME];
            RE_ACTOJUDICIALPARTICIPANTE.ajpa_vNumeroHojaRemision = txtNumHojRem.Text;

            listaParticipantes.Add(RE_ACTOJUDICIALPARTICIPANTE);

        }

        void Notificar(int iActoJudicialParticipanteId, string strParticipanteNombres, UpdatePanel upd = null)
        {
            lblNombreNotificado.Text = strParticipanteNombres;
            Session["intParticipanteid"] = iActoJudicialParticipanteId;
            Int16 intParticipanteEstadoId = Convert.ToInt16(Session["ParticipanteEstadoId"]);

            if (upd == null)
            {
                Comun.EjecutarScript(this, "EnableTabIndex(1);");
            }
            else
            {
                Comun.EjecutarScriptUpdatePanel(upd, "EnableTabIndex(1);");
            }

            BlanqueNotificaciones();

            int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            int intOficinaConsularParticipanteId = Convert.ToInt32(Session["OficinaConsularParticipanteId"]);

            Int16 intEstadiParticipante = Convert.ToInt16(Session["ParticipanteEstadoId"]);

            if (intOficinaConsularId == intOficinaConsularParticipanteId)                   // SI ES CONSULADO
            {
                Int16 intAJEstadoId = Convert.ToInt16(Session["intActuacionEstadoId"]);

                if (intAJEstadoId == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO))
                {
                    Validation2.MostrarValidacion("No se puede Notificar este expediente, Aun no ha sido enviado", true, Enumerador.enmTipoMensaje.WARNING);
                }
                else
                {
                    ctrlToolBarNoti.btnConfiguration.Enabled = true;

                    // PREGUNTAMOS POR EL ESTADO DEL PARTICIPANTE PARA MOSTRAR LAS OPCIONES
                    if (intEstadiParticipante == Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                    {
                        gdvNotificaciones.Columns[10].Visible = false;
                        gdvNotificaciones.Columns[11].Visible = false;
                        ctrlToolBarNoti.btnNuevo.Enabled = false;
                        ctrlToolBarNoti.btnConfiguration.Text = "     Actas";
                        updNotificaciones.Update();
                    }
                    else
                    {
                        gdvNotificaciones.Columns[10].Visible = true;
                        gdvNotificaciones.Columns[11].Visible = true;
                        ctrlToolBarNoti.btnNuevo.Enabled = true;
                        ctrlToolBarNoti.btnConfiguration.Text = "     Finalizar";

                        updNotificaciones.Update();
                    }

                    ActivarNotificaControl();
                }


            }
            else
            {
                // SI ES LIMA
                gdvNotificaciones.Columns[10].Visible = false;
                gdvNotificaciones.Columns[11].Visible = false;

                ctrlToolBarNoti.btnConfiguration.Enabled = true;
                btnNotiAceptar.Enabled = false;
                btnNotiCancelar.Enabled = false;
                ctrlToolBarNoti.btnNuevo.Enabled = false;
                ddlViaEnvio.Enabled = false;
                txtEmpPostal.Enabled = false;
                txtPersNotifica.Enabled = false;
                txtFechaNotifica.Enabled = false;
                txtHoraNotifica.Enabled = false;
                txtNroCedula.Enabled = false;
                ddlTipoRecepcion.Enabled = false;
                txtPerRecep.Enabled = false;
                txtFchRecep.Enabled = false;
                txtHoraRecep.Enabled = false;
                txtNotificacionCuerpo.Enabled = false;
                txtNotiObservacion.Enabled = false;

                if (intEstadiParticipante != Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                {
                    ctrlToolBarNoti.btnConfiguration.Enabled = false;
                }

                Validation2.MostrarValidacion("No se puede registrar Notificaciones mientras no esté en el Consulado destino", true, Enumerador.enmTipoMensaje.WARNING);
            }

            Session["NotificacionGuardada"] = 1;

            #region ObtenerEstadosActayNotificaciones
            int intFila = 0;
            DataTable dtPartipantes = new DataTable();
            dtPartipantes = (DataTable)Session["dtTmpPartipante"];


            for (intFila = 0; intFila <= dtPartipantes.Rows.Count - 1; intFila++)
            {
                if (Convert.ToInt64(dtPartipantes.Rows[intFila]["ajpa_iActoJudicialParticipanteId"].ToString()) == Convert.ToInt64(iActoJudicialParticipanteId))
                {
                    Session["EstadoActas"] = dtPartipantes.Rows[intFila]["ajpa_bActaFlag"].ToString();
                    Session["EstadoNotificacion"] = dtPartipantes.Rows[intFila]["ajpa_bNotificacionFlag"].ToString();
                }
            }


            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() == Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA).ToString())
            {
                if (intEstadiParticipante == Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                {
                    gdvNotificaciones.Columns[10].Visible = false;
                    gdvNotificaciones.Columns[11].Visible = false;
                    ctrlToolBarNoti.btnConfiguration.Text = "     Actas";
                    updNotificaciones.Update();
                }
                else
                {
                    gdvNotificaciones.Columns[10].Visible = true;
                    gdvNotificaciones.Columns[11].Visible = true;
                    ctrlToolBarNoti.btnConfiguration.Text = "     Finalizar";

                    updNotificaciones.Update();
                }

                if (ctrlToolBarNoti.btnConfiguration.Text.Trim() != "Actas")
                {
                    ctrlToolBarNoti.btnConfiguration.Visible = false;
                }
                else
                {
                    ctrlToolBarNoti.btnConfiguration.Visible = Visible;
                }
            }

            #endregion

            botonAdicionar();
            updNotificaciones.Update();

            modificar_Notificaciones();

            if (Session["iCargarTabActas"] != null)
            {
                if (Session["iCargarTabActas"].ToString() == "1")
                {
                    ctrlToolBarNoti_btnConfiguration();
                    Session.Remove("iCargarTabActas");
                }
            }
        }

        bool GrabarNotificaciones()
        {
            bool bolOk = false;
            int intFila = 0;
            int IntRpta = 0;
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);
            DataTable dtNotificaciones = new DataTable();
            int intPartipantePersonaId = Convert.ToInt32(Session["intParticipanteid"]);
            dtNotificaciones = (DataTable)Session["dtTmpNotificacion"];


           

            ActoJudicialNotificacionMantenimientoBL miFun = new ActoJudicialNotificacionMantenimientoBL();
            List<SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION> NOTIFICACIONES_LISTA = new List<SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION>();

            Controlador.Proceso MiProc = new Proceso();
            Object[] Parametros = new Object[1] { NOTIFICACIONES_LISTA };

            // **********************************************
            // INICIAMOS EL PROCESO DE ESCRITURA DE LOS DATOS

            if (Session["dtTmpNotificacionesEliminadas"] != null)
            {
                DataTable dtNotificacionesEliminadas = (DataTable)Session["dtTmpNotificacionesEliminadas"];

                if (dtNotificacionesEliminadas.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNotificacionesEliminadas.Rows)
                    {
                        dtNotificaciones.ImportRow(dr);
                    }
                }
            }
            //---------------------------------------------------------------------------------
            //Fecha:08/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Se incluyo la validación al final de la importación de los anulados
            //---------------------------------------------------------------------------------
            if (dtNotificaciones.Rows.Count == 0)
            {
                Validation2.MostrarValidacion("No se ha agregado ninguna Notificación, Debe ingresar al menos una Notificación", true, Enumerador.enmTipoMensaje.INFORMATION);
                updNotificaciones.Update();
                return false;
            }
            //---------------------------------------------------------------------------------
            for (intFila = 0; intFila <= dtNotificaciones.Rows.Count - 1; intFila++)
            {
                SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION RE_ACTOJUDICIALNOTIFICACION = new SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION();

                RE_ACTOJUDICIALNOTIFICACION.ajno_iActoJudicialNotificacionId = Convert.ToInt64(dtNotificaciones.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString());                                     // ESTE DATO ES 0 SE AUTOGENERA AL MOMENTO DE GRABAR EL REGISTRO
                RE_ACTOJUDICIALNOTIFICACION.ajno_iActoJudicialParticipanteId = Convert.ToInt64(dtNotificaciones.Rows[intFila]["ajno_iActoJudicialParticipanteId"].ToString());


                RE_ACTOJUDICIALNOTIFICACION.ajno_sTipoRecepcionId = null;

                if (dtNotificaciones.Rows[intFila]["ajno_sTipoRecepcionId"].ToString() != string.Empty)
                {
                    if (Convert.ToInt16(dtNotificaciones.Rows[intFila]["ajno_sTipoRecepcionId"].ToString()) != 0)
                    {
                        RE_ACTOJUDICIALNOTIFICACION.ajno_sTipoRecepcionId = Convert.ToInt16(dtNotificaciones.Rows[intFila]["ajno_sTipoRecepcionId"].ToString());
                    }
                }

                RE_ACTOJUDICIALNOTIFICACION.ajno_sViaEnvioId = Convert.ToInt16(dtNotificaciones.Rows[intFila]["ajno_sViaEnvioId"].ToString());
                RE_ACTOJUDICIALNOTIFICACION.ajno_vEmpresaServicioPostal = dtNotificaciones.Rows[intFila]["ajno_vEmpresaServicioPostal"].ToString();
                RE_ACTOJUDICIALNOTIFICACION.ajno_vPersonaNotificacion = dtNotificaciones.Rows[intFila]["ajno_vPersonaNotificacion"].ToString();
                RE_ACTOJUDICIALNOTIFICACION.ajno_dFechaHoraNotificacion = Comun.FormatearFecha(dtNotificaciones.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString());
                RE_ACTOJUDICIALNOTIFICACION.ajno_vNumeroCedula = dtNotificaciones.Rows[intFila]["ajno_vNumeroCedula"].ToString();
                RE_ACTOJUDICIALNOTIFICACION.ajno_vPersonaRecibeNotificacion = dtNotificaciones.Rows[intFila]["ajno_vPersonaRecibeNotificacion"].ToString();

                if (dtNotificaciones.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString() != "")
                {
                    RE_ACTOJUDICIALNOTIFICACION.ajno_dFechaHoraRecepcion = Comun.FormatearFecha(dtNotificaciones.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString());
                }
                else
                {
                    RE_ACTOJUDICIALNOTIFICACION.ajno_dFechaHoraRecepcion = null;
                }

                RE_ACTOJUDICIALNOTIFICACION.ajno_vCuerpoNotificacion = dtNotificaciones.Rows[intFila]["ajno_vCuerpoNotificacion"].ToString();
                RE_ACTOJUDICIALNOTIFICACION.ajno_vObservaciones = dtNotificaciones.Rows[intFila]["ajno_vObservaciones"].ToString();
                RE_ACTOJUDICIALNOTIFICACION.ajno_sEstadoId = Convert.ToInt16(dtNotificaciones.Rows[intFila]["ajno_sEstadoId"].ToString());

                if (Convert.ToInt64(dtNotificaciones.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString()) < 0)
                {
                    RE_ACTOJUDICIALNOTIFICACION.ajno_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALNOTIFICACION.ajno_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALNOTIFICACION.ajno_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);
                }
                else
                {
                    RE_ACTOJUDICIALNOTIFICACION.ajno_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    RE_ACTOJUDICIALNOTIFICACION.ajno_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    RE_ACTOJUDICIALNOTIFICACION.ajno_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                }

                RE_ACTOJUDICIALNOTIFICACION.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                RE_ACTOJUDICIALNOTIFICACION.HostName = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);

                NOTIFICACIONES_LISTA.Add(RE_ACTOJUDICIALNOTIFICACION);
            }

            IntRpta = miFun.Insertar(NOTIFICACIONES_LISTA);
            if (IntRpta == 1)
            {
                Session["NotificacionGuardada"] = 1;
                Validation2.MostrarValidacion("Las Notificaciones se guardaron con éxito", true, Enumerador.enmTipoMensaje.INFORMATION);
                updNotificaciones.Update();
                bolOk = true;
            }
            else
            {
                Validation2.MostrarValidacion("No se pudieron guardar las Notificaciones, revise la información ingresada", true, Enumerador.enmTipoMensaje.INFORMATION);
                updNotificaciones.Update();
                bolOk = false;
            }

            MostrarActoJudicial();

            return bolOk;
        }

        bool GrabarActas()
        {
            bool bolOk = false;
            int intFila = 0;
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);
            DataTable dtActas = new DataTable();
            ActaJudicialMantenimientoBL miFun = new ActaJudicialMantenimientoBL();
            List<SGAC.BE.MRE.RE_ACTAJUDICIAL> ACTA_LISTA = new List<SGAC.BE.MRE.RE_ACTAJUDICIAL>();
            Controlador.Proceso MiProc = new Proceso();
            Object[] Parametros = new Object[1] { ACTA_LISTA };

            SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION = new SGAC.BE.MRE.RE_ACTUACION();
            List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO = new List<SGAC.BE.MRE.RE_PAGO>();
            List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE = new List<SGAC.BE.MRE.RE_ACTUACIONDETALLE>();

            if (GenerarActuacion(ref RE_ACTUACION, ref LIS_RE_ACTUACIONDETALLE, ref LIS_RE_PAGO) == false)
            {
                Validation1.MostrarValidacion("No se pudo generar la actuación para este acto judicial, se cancela el proceso de grabación", true, Enumerador.enmTipoMensaje.WARNING);
                updNotificados.Update();
                return false;
            }

            string iActoJudicialParticipanteId = Session["iActoJudicialParticipanteId"].ToString();
            String _dFechaRegistro = "";
            int intResult = miFun.Insertar_Actu_Det(RE_ACTUACION, LIS_RE_ACTUACIONDETALLE, LIS_RE_PAGO, (DataTable)Session["dtTmpPartipante"], ref iActoJudicialParticipanteId, ref _dFechaRegistro);
            if (_dFechaRegistro != "")
            {
                Session["LblFecha"] = _dFechaRegistro;
                Session["iActuacionDetalleId"] = iActoJudicialParticipanteId;
            }
            //if (intResult == 0)
            //{
            //    return false;
            //}

            //return false;

            dtActas = (DataTable)Session["dtTmpActa"];

            if (dtActas.Rows.Count == 0)
            {
                Validation3.MostrarValidacion("No se agregó ninguna Acta de diligenciamiento, Debe ingresar al menos un Acta", true, Enumerador.enmTipoMensaje.WARNING);
                updActas.Update();
                return false;
            }

            // **********************************************
            // LLENAMOS EL OBJETO RE_ACTOJUDICIALNOTIFICACION
            for (intFila = 0; intFila <= dtActas.Rows.Count - 1; intFila++)
            {
                SGAC.BE.MRE.RE_ACTAJUDICIAL RE_ACTAJUDICIAL = new SGAC.BE.MRE.RE_ACTAJUDICIAL();

                RE_ACTAJUDICIAL.acjd_iActaJudicialId = Convert.ToInt64(dtActas.Rows[intFila]["acjd_iActaJudicialId"].ToString());
                RE_ACTAJUDICIAL.acjd_iActoJudicialNotificacionId = Convert.ToInt64(dtActas.Rows[intFila]["acjd_iActoJudicialNotificacionId"].ToString());
                RE_ACTAJUDICIAL.acjd_sTipoActaId = Convert.ToInt16(dtActas.Rows[intFila]["acjd_sTipoActaId"].ToString());
                RE_ACTAJUDICIAL.acjd_IFuncionarioFirmanteId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
                RE_ACTAJUDICIAL.acjd_dFechaHoraActa = Comun.FormatearFecha(dtActas.Rows[intFila]["acjd_dFechaHoraActa"].ToString());
                RE_ACTAJUDICIAL.acjd_vCuerpoActa = Convert.ToString(dtActas.Rows[intFila]["acjd_vCuerpoActa"].ToString());
                RE_ACTAJUDICIAL.acjd_vResultado = Convert.ToString(dtActas.Rows[intFila]["acjd_vResultado"].ToString());
                RE_ACTAJUDICIAL.acjd_vObservaciones = Convert.ToString(dtActas.Rows[intFila]["acjd_vObservaciones"].ToString());
                RE_ACTAJUDICIAL.acjd_sEstadoId = Convert.ToInt16(dtActas.Rows[intFila]["acjd_sEstadoId"].ToString());
                RE_ACTAJUDICIAL.acjd_vResponsable = dtActas.Rows[intFila]["acjd_vResponsable"].ToString();
                RE_ACTAJUDICIAL.acjd_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                RE_ACTAJUDICIAL.acjd_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                RE_ACTAJUDICIAL.acjd_dFechaCreacion = ObtenerFechaActual(HttpContext.Current.Session);
                RE_ACTAJUDICIAL.acjd_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                RE_ACTAJUDICIAL.acjd_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                RE_ACTAJUDICIAL.acjd_dFechaModificacion = ObtenerFechaActual(HttpContext.Current.Session);
                RE_ACTAJUDICIAL.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                RE_ACTAJUDICIAL.HostName = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);

                ACTA_LISTA.Add(RE_ACTAJUDICIAL);
            }

            int intValor = 0;

            intValor = miFun.Insertar(ACTA_LISTA);



            if (intValor == 1)
            {
                Validation3.MostrarValidacion("La Acta de Diligenciamiento se grabó correctamente", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();
                bolOk = true;
            }
            else
            {
                Validation3.MostrarValidacion("No se pudo guardar las Actas de Diligenciamiento, revise la información ingresada", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();
            }

            return bolOk;
        }

        void ActivarActasControl()
        {
            int intOficinaIdLima = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficinaIdActual = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);

            if (intOficinaIdLima != intOficinaIdActual)                                             // SI LA OFICINA ACTUAL ES IGUAL A LIMA ACTIVAMOS LOS CONTROLES DE LA PESTAÑA 1 PARA EDICION  E INSERCCION
            {
                if (intQueHace != 3)
                {
                    ddlActaTipo.Enabled = true;
                    txtActaHora.Enabled = true;
                    txtActaFecha.Enabled = true;
                    txtActaResponsable.Enabled = true;
                    txtActaCuerpo.Enabled = true;
                    //---------------------------------------------------------------
                    //Fecha: 17/02/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Eliminar el campo: Resultado.
                    //---------------------------------------------------------------
                    //txtResultado.Enabled = true;
                    txtActaObservacion.Enabled = true;

                    btnActaAceptar.Enabled = true;
                    btnActaCancelar.Enabled = true;
                }
                else
                {
                    ddlActaTipo.Enabled = false;
                    txtActaFecha.Enabled = false;
                    txtActaHora.Enabled = false;
                    txtActaResponsable.Enabled = false;
                    txtActaCuerpo.Enabled = false;
                    //---------------------------------------------------------------
                    //Fecha: 17/02/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Eliminar el campo: Resultado.
                    //---------------------------------------------------------------
                    //txtResultado.Enabled = false;
                    txtActaObservacion.Enabled = false;

                    btnActaAceptar.Enabled = false;
                    btnActaCancelar.Enabled = false;

                    Validation3.MostrarValidacion("No se puede registrar Actas mientras esté en Lima", true, Enumerador.enmTipoMensaje.WARNING);
                }
            }
            else
            {
                ddlActaTipo.Enabled = false;
                txtActaFecha.Enabled = false;
                txtActaHora.Enabled = false;
                ddlActaEstado.Enabled = false;
                txtActaResponsable.Enabled = false;
                txtActaCuerpo.Enabled = false;
                //---------------------------------------------------------------
                //Fecha: 17/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Eliminar el campo: Resultado.
                //---------------------------------------------------------------
                //txtResultado.Enabled = false;
                txtActaObservacion.Enabled = false;
                ddlActaTipo.Enabled = false;
                btnActaAceptar.Enabled = false;
                btnActaCancelar.Enabled = false;
            }
            updActas.Update();
        }

        void ActivarNotificacionControlNotifica()
        {
            btnNotiAceptar.Enabled = true;
            btnNotiCancelar.Enabled = true;
            ctrlToolBarNoti.btnConfiguration.Enabled = true;

            // ACTIVAMOS LOS CONTROLES DE LA EMISION DE LA NOTIFICACION

            ddlViaEnvio.Enabled = true;
            txtEmpPostal.Enabled = true;
            txtPersNotifica.Enabled = true;
            txtFechaNotifica.Enabled = true;
            txtHoraNotifica.Enabled = true;
            txtNotificacionCuerpo.Enabled = true;


            // DESACTIVAMOS LOS CONTROLES DE LA RECEPCION DE NOTIFICACION
            ddlTipoRecepcion.Enabled = false;
            txtNroCedula.Enabled = false;
            txtPerRecep.Enabled = false;
            txtFchRecep.Enabled = false;
            txtHoraRecep.Enabled = false;
            txtNotiObservacion.Enabled = false;
        }

        void ActivarNotificacionControlRegistraVisita()
        {
            btnNotiAceptar.Enabled = true;
            btnNotiCancelar.Enabled = true;
            ctrlToolBarNoti.btnConfiguration.Enabled = true;
            // DESACTIVAMOS LOS CONTROLES DE LA EMISION DE LA NOTIFICACION
            ddlViaEnvio.Enabled = false;
            txtEmpPostal.Enabled = false;
            txtPersNotifica.Enabled = false;
            txtFechaNotifica.Enabled = false;
            txtHoraNotifica.Enabled = false;
            txtNotificacionCuerpo.Enabled = false;

            // ACTIVAMOS LOS CONTROLES DE LA RECEPCION DE NOTIFICACION
            if (Convert.ToInt16(ddlViaEnvio.SelectedValue) == Convert.ToUInt16(Enumerador.enmJudicialViaEnvio.NOTIFICACION_PERSONAL))
            {
                txtNroCedula.Enabled = false;
            }
            else
            {
                txtNroCedula.Enabled = true;
            }

            ddlTipoRecepcion.Enabled = true;
            txtPerRecep.Enabled = true;
            txtFchRecep.Enabled = true;
            txtHoraRecep.Enabled = true;
            txtNotiObservacion.Enabled = true;

            txtPerRecep.Enabled = true;
            txtHoraRecep.Enabled = true;
            txtNotiObservacion.Enabled = true;

        }

        void BloquearNotificacion()
        {
            btnNotiAceptar.Enabled = false;
            btnNotiCancelar.Enabled = true;
            ctrlToolBarNoti.btnConfiguration.Enabled = true;
            // ACTIVAMOS LOS CONTROLES DE LA EMISION DE LA NOTIFICACION
            ddlViaEnvio.Enabled = false;
            txtEmpPostal.Enabled = false;
            txtPersNotifica.Enabled = false;
            txtFechaNotifica.Enabled = false;
            txtHoraNotifica.Enabled = false;
            txtNotificacionCuerpo.Enabled = false;
            txtFechaNotifica.Enabled = false;

            // DESACTIVAMOS LOS CONTROLES DE LA RECEPCION DE NOTIFICACION
            ddlTipoRecepcion.Enabled = false;
            txtNroCedula.Enabled = false;
            txtPerRecep.Enabled = false;
            txtFchRecep.Enabled = false;
            txtHoraRecep.Enabled = false;
            txtNotiObservacion.Enabled = false;
            txtFchRecepcion.Enabled = false;
        }

        void ActivarNotificaControl()
        {
            int intOficinaIdLima = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficinaIdActual = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);

            if (intOficinaIdLima != intOficinaIdActual)                       // SI LA OFICINA ACTUAL ES DIFERENTE A LIMA ACTIVAMOS LOS CONTROLES DE LA PESTAÑA 1 PARA EDICION  E INSERCCION
            {
                if (intQueHace != 3)   // PREGUTAMOS SI EL FORMULARIO ESTA EN EDICION O ADICION
                {
                    DataTable dtNotificacion = new DataTable();

                    if (Session["dtTmpNotificacion"] != null)
                        dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

                    if (dtNotificacion.Rows.Count == 0)                                     // SI EL NUMERO DE NOTIFICACIONES ES IGUAL A 0 ACTIVAMOS LOS BOTONES DE ADICIONAR Y LOS CAMPOS DE INGRESO DE NOTIFICACION
                    {
                        ActivarNotificacionControlNotifica();                               // ACTIVAMOS PARA LA EMISION DE LA NOTIFICACION
                        ctrlToolBarNoti.btnConfiguration.Visible = false;
                    }
                    else
                    {
                        Int16 intParticipanteEstadoId = Convert.ToInt16(Session["ParticipanteEstadoId"]);
                        ctrlToolBarNoti.btnConfiguration.Visible = true;

                        //// SI LA ULTIMA NOTIFICACION INGRESADA ES IGUAL A RECIBIDO
                        if (intParticipanteEstadoId == Convert.ToInt32(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                        {
                            BloquearNotificacion(); // BLOQUEAMOS TODA LA NOTIFICACION
                            ctrlToolBarNoti.btnNuevo.Enabled = false;
                        }
                        else if (dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO).ToString()).Length > 0 ||
                            dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR).ToString()).Length > 0 ||
                            dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO).ToString()).Length > 0 ||
                            dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA).ToString()).Length > 0 ||
                            dtNotificacion.Select("ajno_sTipoRecepcionId =" + Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON).ToString()).Length > 0)
                        {
                            BloquearNotificacion(); // BLOQUEAMOS TODA LA NOTIFICACION
                        }
                        else if (dtNotificacion.Select("ajno_sTipoRecepcionId is null").Length > 0 ||
                            dtNotificacion.Select("ajno_sTipoRecepcionId = 0").Length > 0)
                        {
                            BloquearNotificacion(); // BLOQUEAMOS TODA LA NOTIFICACION
                            ctrlToolBarNoti.btnConfiguration.Visible = false;

                        }
                        else
                        {
                            ctrlToolBarNoti.btnNuevo.Enabled = true;
                            ActivarNotificacionControlNotifica();                          // ACTIVAMOS LOS CONTROLS PARA EMITIR LA NOTIFICACION 
                        }
                    }
                }
                else
                {
                    BloquearNotificacion();                                                // BLOQUEAMOS TODA LA NOTIFICACION
                }
            }
            else
            {
                // SI ES LIMA DESABILITA LOS CONTROLES
                BloquearNotificacion();
            }



            botonAdicionar();
            updNotificaciones.Update();
        }

        void ActivarNotificaControlEditable(int iActoJudicialNotificacionId)
        {
            int intOficinaIdLima = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficinaIdActual = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);

            if (intOficinaIdLima != intOficinaIdActual)
            {
                btnNotiAceptar.Enabled = true;
                btnNotiCancelar.Enabled = true;
                ctrlToolBarNoti.btnConfiguration.Enabled = true;
                // ACTIVAMOS LOS CONTROLES DE LA EMISION DE LA NOTIFICACION
                ddlViaEnvio.Enabled = true;
                txtEmpPostal.Enabled = true;
                txtPersNotifica.Enabled = true;
                txtFechaNotifica.Enabled = true;
                txtHoraNotifica.Enabled = true;
                txtNotificacionCuerpo.Enabled = true;

                // DESACTIVAMOS LOS CONTROLES DE LA RECEPCION DE NOTIFICACION
                ddlTipoRecepcion.Enabled = false;

                if (ddlTipoRecepcion.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO).ToString() ||
                    ddlTipoRecepcion.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR).ToString() ||
                    ddlTipoRecepcion.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO).ToString())
                {
                    txtPerRecep.Enabled = true;
                    txtFchRecep.Enabled = true;
                    txtHoraRecep.Enabled = true;
                }
                else if (ddlTipoRecepcion.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA).ToString() ||
                    ddlTipoRecepcion.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON).ToString())
                {
                    txtFchRecep.Enabled = true;
                    txtHoraRecep.Enabled = true;
                }
                else if (ddlTipoRecepcion.SelectedValue.ToString() == "0")
                {
                    ddlTipoRecepcion.Enabled = true;
                    txtPerRecep.Enabled = true;
                    txtFchRecep.Enabled = true;
                    txtHoraRecep.Enabled = true;
                }
                else
                {
                    txtPerRecep.Enabled = false;
                    txtFchRecep.Enabled = false;
                    txtHoraRecep.Enabled = false;
                }

                txtNroCedula.Enabled = true;
                txtNotiObservacion.Enabled = true;
            }
            else
            {
                BloquearNotificacion();
            }

            botonActualizar();
            updNotificaciones.Update();
        }

        void LlenarControlesNotificacion(int iActoJudicialNotificacionId)
        {
            int intFila = 0;
            DataTable dtNotificacion = new DataTable();
            DateTime datFecha;
            String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();

            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            for (intFila = 0; intFila <= dtNotificacion.Rows.Count - 1; intFila++)
            {
                if (iActoJudicialNotificacionId == Convert.ToInt32(dtNotificacion.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString()))
                {
                    #region Controles de Envío

                    ddlViaEnvio.SelectedValue = dtNotificacion.Rows[intFila]["ajno_sViaEnvioId"].ToString();
                    txtEmpPostal.Text = dtNotificacion.Rows[intFila]["ajno_vEmpresaServicioPostal"].ToString();
                    txtPersNotifica.Text = dtNotificacion.Rows[intFila]["ajno_vPersonaNotificacion"].ToString();

                    if (dtNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString() != "")
                    {
                        datFecha = Comun.FormatearFecha(dtNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString());
                        txtFechaNotifica.Text = datFecha.ToString(FormatoFechas);
                        txtHoraNotifica.Text = datFecha.ToString("hh:mm");
                    }
                    txtNotificacionCuerpo.Text = dtNotificacion.Rows[intFila]["ajno_vCuerpoNotificacion"].ToString();

                    #endregion

                    #region Controles de Recepción

                    if (dtNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"].ToString() != "")
                    {
                        ddlTipoRecepcion.SelectedValue = dtNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"].ToString();
                    }

                    txtNroCedula.Text = dtNotificacion.Rows[intFila]["ajno_vNumeroCedula"].ToString();
                    txtPerRecep.Text = dtNotificacion.Rows[intFila]["ajno_vPersonaRecibeNotificacion"].ToString();

                    if (dtNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString() != "")
                    {
                        datFecha = Comun.FormatearFecha(dtNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString());
                        txtFchRecep.Text = datFecha.ToString(FormatoFechas);
                        txtHoraRecep.Text = datFecha.ToString("hh:mm");
                    }

                    txtNotiObservacion.Text = dtNotificacion.Rows[intFila]["ajno_vObservaciones"].ToString();


                    #endregion
                }
            }


            updNotificaciones.Update();
        }

        void MostrarPago(Int64 iPagoId, int Tipo)
        {
            // Tipo = 2 Edicion de registro;  Tipo = 3 consulta registro
            int intFila = 0;
            DataTable dtTMPPago = new DataTable();

            dtTMPPago = (DataTable)Session["dtTmpRePagos"];
            String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            for (intFila = 0; intFila <= dtTMPPago.Rows.Count - 1; intFila++)
            {
                if (iPagoId == Convert.ToInt64(dtTMPPago.Rows[intFila]["pago_iPagoId"].ToString()))
                {
                    ddlTarifaConsul.SelectedValue = dtTMPPago.Rows[intFila]["pago_sTarifarioId"].ToString();
                    txtNumOrdPag.Text = dtTMPPago.Rows[intFila]["pago_vNumeroVoucher"].ToString();

                    DateTime datFecha = Comun.FormatearFecha(dtTMPPago.Rows[intFila]["pago_dFechaOperacion"].ToString());
                    txtFchPago.Text = datFecha.ToString(FormatoFechas);

                    chkGratuito.Checked = !Convert.ToBoolean(dtTMPPago.Rows[intFila]["pago_bPagadoFlag"]);
                    txtMotivoGratuidad.Text = dtTMPPago.Rows[intFila]["pago_vComentario"].ToString();
                    txtCosto.Text = dtTMPPago.Rows[intFila]["pago_FMontoSolesConsulares"].ToString();
                    ddlTipoPago.SelectedValue = dtTMPPago.Rows[intFila]["pago_sPagoTipoId"].ToString();
                    ddlMoneda.SelectedValue = dtTMPPago.Rows[intFila]["pago_sMonedaLocalId"].ToString();
                    txtCosto2.Text = dtTMPPago.Rows[intFila]["pago_FMontoMonedaLocal"].ToString();
                    if (dtTMPPago.Rows[intFila]["pago_sBancoId"].ToString() != "")
                    {
                        ddlBanco.SelectedValue = dtTMPPago.Rows[intFila]["pago_sBancoId"].ToString();
                    }
                    txtNumCheque.Text = dtTMPPago.Rows[intFila]["pago_vBancoNumeroOperacion"].ToString();
                    txtNumOrdPag.Text = dtTMPPago.Rows[intFila]["pago_vNumeroVoucher"].ToString();

                    chkGratuito_CheckedChanged(null, null);
                }
            }

            if (Tipo == 3)
            {
                btnAceptarPago.Enabled = false;

                ddlTarifaConsul.Enabled = false;
                ddlBanco.Enabled = false;
                txtNumCheque.Enabled = false;
                txtNumOrdPag.Enabled = false;
                txtFchPago.Enabled = false;

                txtMotivoGratuidad.Enabled = false;
                txtCosto.Enabled = false;
            }

            if (Tipo == 2)
            {
                btnAceptarPago.Enabled = true;
                btnAceptarPago.Text = "    Actualizar";

                ddlTarifaConsul.Enabled = true;
                ddlBanco.Enabled = true;
                txtNumCheque.Enabled = true;
                txtNumOrdPag.Enabled = true;
                txtFchPago.Enabled = true;
            }
        }

        void MostrarParticipante(Int16 sTipoDocumentoId, string vNumeroDocumento, int Tipo, Int32 intindiceparticipante = 0)
        {
            // Tipo = 2 EDITAR;   Tipo = 3 Consulta   
            int intFila = 0;
            DataTable dtPartipante = new DataTable();
            DateTime datFecha;
            dtPartipante = (DataTable)Session["dtTmpPartipante"];
            String FormatoFecha = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            for (intFila = 0; intFila <= dtPartipante.Rows.Count - 1; intFila++)
            {
                if (sTipoDocumentoId == 0 && vNumeroDocumento == "" && intindiceparticipante > 0)
                {
                    if (intindiceparticipante == Convert.ToInt16(dtPartipante.Rows[intFila]["ajpa_inumero"]))
                    {
                        ddlTipPersona3.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sTipoPersonaId"].ToString();
                        ActualizarTipoDocumento();
                        ddlTipDoc3.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString();

                        txtNumDoc3.Text = dtPartipante.Rows[intFila]["ajpa_vDocumentoNumero"].ToString();
                        txtNom3.Text = dtPartipante.Rows[intFila]["ajpa_vNombre"].ToString();
                        txtApePat3.Text = dtPartipante.Rows[intFila]["ajpa_vApePaterno"].ToString();
                        txtApeMat3.Text = dtPartipante.Rows[intFila]["ajpa_vApeMaterno"].ToString();

                        if (dtPartipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString() != "")
                        {
                            datFecha = Comun.FormatearFecha(dtPartipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString());
                            txtFchValDip.Text = datFecha.ToString("MMM-dd-yyyy");
                        }


                        txtNumHojRem.Text = dtPartipante.Rows[intFila]["ajpa_vNumeroHojaRemision"].ToString();
                        ctrlOficinaConsular1.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sOficinaConsularDestinoId"].ToString();
                    }
                }else if (sTipoDocumentoId == Convert.ToInt16(dtPartipante.Rows[intFila]["ajpa_sDocumentoTipoId"]) &&
                   vNumeroDocumento == dtPartipante.Rows[intFila]["ajpa_vDocumentoNumero"].ToString())
                {
                    ddlTipPersona3.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sTipoPersonaId"].ToString();
                    ActualizarTipoDocumento();
                    ddlTipDoc3.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString();

                    txtNumDoc3.Text = dtPartipante.Rows[intFila]["ajpa_vDocumentoNumero"].ToString();
                    txtNom3.Text = dtPartipante.Rows[intFila]["ajpa_vNombre"].ToString();
                    txtApePat3.Text = dtPartipante.Rows[intFila]["ajpa_vApePaterno"].ToString();
                    txtApeMat3.Text = dtPartipante.Rows[intFila]["ajpa_vApeMaterno"].ToString();

                    if (dtPartipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString() != "")
                    {
                        datFecha = Comun.FormatearFecha(dtPartipante.Rows[intFila]["ajpa_dFechaLlegadaValija"].ToString());
                        txtFchValDip.Text = datFecha.ToString("MMM-dd-yyyy");
                    }


                    txtNumHojRem.Text = dtPartipante.Rows[intFila]["ajpa_vNumeroHojaRemision"].ToString();
                    ctrlOficinaConsular1.SelectedValue = dtPartipante.Rows[intFila]["ajpa_sOficinaConsularDestinoId"].ToString();
                }
            }

            if (Tipo == 3)
            {
                btnAceptarNotifica.Enabled = false;

                ddlTipPersona3.Enabled = false;
                ddlTipDoc3.Enabled = false;
                txtNumDoc3.Enabled = false;

                txtFchValDip.Enabled = false;
                txtNumHojRem.Enabled = false;
                ctrlOficinaConsular1.Enabled = false;

                txtNom3.Enabled = false;
                txtApePat3.Enabled = false;
                txtApeMat3.Enabled = false;


            }
            if (Tipo == 2)
            {
                btnAceptarNotifica.Enabled = true;
                btnAceptarNotifica.Text = "Actualizar";

                ddlTipPersona3.Enabled = true;
                ddlTipDoc3.Enabled = true;
                txtNumDoc3.Enabled = true;


                txtFchValDip.Enabled = true;
                txtNumHojRem.Enabled = true;
                ctrlOficinaConsular1.Enabled = true;
            }
        }

        void MostrarNotificaciones(int iActoJudicialNotificacionId, int intAccion)
        {
            int intFila = 0;
            DataTable dtNotificacion = new DataTable();
            DateTime datFecha;

            txtPerRecep.Enabled = false;
            String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            for (intFila = 0; intFila <= dtNotificacion.Rows.Count - 1; intFila++)
            {
                if (iActoJudicialNotificacionId == Convert.ToInt32(dtNotificacion.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString()))
                {
                    ddlViaEnvio.SelectedValue = dtNotificacion.Rows[intFila]["ajno_sViaEnvioId"].ToString();
                    txtEmpPostal.Text = dtNotificacion.Rows[intFila]["ajno_vEmpresaServicioPostal"].ToString();
                    txtPersNotifica.Text = dtNotificacion.Rows[intFila]["ajno_vPersonaNotificacion"].ToString();

                    if (dtNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString() != "")
                    {
                        datFecha = Comun.FormatearFecha(dtNotificacion.Rows[intFila]["ajno_dFechaHoraNotificacion"].ToString());
                        txtFechaNotifica.Text = datFecha.ToString(FormatoFechas);
                        txtHoraNotifica.Text = datFecha.ToString("hh:mm");
                    }
                    txtNroCedula.Text = dtNotificacion.Rows[intFila]["ajno_vNumeroCedula"].ToString();

                    if (dtNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"].ToString() != "")
                    {
                        ddlTipoRecepcion.SelectedValue = dtNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"].ToString();

                    }

                    txtPerRecep.Text = dtNotificacion.Rows[intFila]["ajno_vPersonaRecibeNotificacion"].ToString();

                    if (dtNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString() != "")
                    {
                        datFecha = Comun.FormatearFecha(dtNotificacion.Rows[intFila]["ajno_dFechaHoraRecepcion"].ToString());
                        txtFchRecep.Text = datFecha.ToString(FormatoFechas);
                        txtHoraRecep.Text = datFecha.ToString("hh:mm");
                    }

                    txtNotificacionCuerpo.Text = dtNotificacion.Rows[intFila]["ajno_vCuerpoNotificacion"].ToString();
                    txtNotiObservacion.Text = dtNotificacion.Rows[intFila]["ajno_vObservaciones"].ToString();

                    // SI EL ESTADO DE LA NOTIFICACION ES IGUAL RECIBIDO

                    BloquearNotificacion();                             // BLOQUEAMOS LOS CONTROLES DE NOTIFICACION
                    break;
                }
            }



            if (intAccion == 3)
            {

                ddlTipoRecepcion.Enabled = false;
                txtNroCedula.Enabled = false;
                txtPerRecep.Enabled = false;
                txtFchRecep.Enabled = false;
                txtHoraRecep.Enabled = false;
                txtNotiObservacion.Enabled = false;

                botonAdicionar();
                btnNotiAceptar.Enabled = false;
            }

            if (intAccion == 2)
            {
                if (dtNotificacion.Rows[intFila]["ajno_sEstadoId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialNotificacionEstado.ENVIADO).ToString() ||
                    dtNotificacion.Rows[intFila]["ajno_sEstadoId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialNotificacionEstado.REGISTRADO).ToString() ||
                    dtNotificacion.Rows[intFila]["ajno_sTipoRecepcionId"].ToString() == string.Empty)
                {
                    ddlTipoRecepcion.SelectedValue = "0";
                }

                if (Convert.ToInt16(ddlViaEnvio.SelectedValue) == Convert.ToInt16(Enumerador.enmJudicialViaEnvio.NOTIFICACION_PERSONAL))
                {
                    txtNroCedula.Enabled = false;
                }
                else
                {
                    txtNroCedula.Enabled = true;
                }

                ddlTipoRecepcion.Enabled = true;
                txtPerRecep.Enabled = true;
                txtFchRecep.Enabled = true;
                txtHoraRecep.Enabled = true;
                txtNotiObservacion.Enabled = true;

                ddlViaEnvio.Enabled = true;


                txtFechaNotifica.Enabled = true;
                txtHoraNotifica.Enabled = true;
                txtNotificacionCuerpo.Enabled = true;


                if (ddlViaEnvio.SelectedValue.ToString() == Convert.ToInt32(Enumerador.enmJudicialViaEnvio.NOTIFICACION_CORREO).ToString())
                {
                    txtEmpPostal.Enabled = true;
                    txtPersNotifica.Enabled = false;
                }
                else
                {
                    txtEmpPostal.Enabled = false;
                    txtPersNotifica.Enabled = true;
                }

                botonActualizar();
                btnNotiAceptar.Enabled = true;
                ddlTipoRecepcion_SelectedIndexChanged(null, null);
            }



            updNotificaciones.Update();
        }

        void EliminarNotificaciones(int iActoJudicialNotificacionId)
        {
            int intFila = 0;
            DataTable dtNotificacion = new DataTable();
            DataTable dtNotificacionesEliminadas = new DataTable();

            if (Session["dtTmpNotificacion"] != null)
                dtNotificacion = (DataTable)Session["dtTmpNotificacion"];

            if (Session["dtTmpNotificacionesEliminadas"] != null)
            {
                dtNotificacionesEliminadas = (DataTable)Session["dtTmpNotificacionesEliminadas"];
            }
            else
            {
                dtNotificacionesEliminadas = dtNotificacion.Clone();
            }

            for (intFila = 0; intFila <= dtNotificacion.Rows.Count - 1; intFila++)
            {
                if (iActoJudicialNotificacionId == Convert.ToInt32(dtNotificacion.Rows[intFila]["ajno_iActoJudicialNotificacionId"].ToString()) &&
                    dtNotificacion.Rows[intFila]["ajno_sEstadoId"].ToString() != Convert.ToInt32(Enumerador.enmJudicialNotificacionEstado.ANULADO).ToString())
                {
                    if (iActoJudicialNotificacionId != -1)
                    {
                        dtNotificacion.Rows[intFila]["ajno_sEstadoId"] = Convert.ToInt32(Enumerador.enmJudicialNotificacionEstado.ANULADO).ToString();
                        dtNotificacionesEliminadas.ImportRow(dtNotificacion.Rows[intFila]);

                    }

                    dtNotificacion.Rows.RemoveAt(intFila);

                }
            }

            if (Session["NotificacionIdEditando"] != null)
            {
                if (iActoJudicialNotificacionId.ToString() == Session["NotificacionIdEditando"].ToString())
                {
                    LimpiarPantallaNotificacion();
                    Session.Remove("NotificacionIdEditando");
                }
            }

            if (dtNotificacion.Rows.Count == 0)
                Session.Remove("NotificacionIdEditando");


            Session["dtTmpNotificacion"] = dtNotificacion;
            Session["dtTmpNotificacionesEliminadas"] = dtNotificacionesEliminadas;


            gdvNotificaciones.DataSource = dtNotificacion;
            gdvNotificaciones.DataBind();

            btnNotiCancelar_Click(null, null);

            updNotificaciones.Update();


        }

        void EliminarActas(Int64 iActaJudicialId)
        {
            int intFila = 0;
            DataTable dtActas = new DataTable();
            dtActas = (DataTable)Session["dtTmpActa"];

            for (intFila = 0; intFila <= dtActas.Rows.Count - 1; intFila++)
            {
                if (Convert.ToInt64(dtActas.Rows[intFila]["acjd_iActaJudicialId"].ToString()) == iActaJudicialId)
                {
                    dtActas.Rows.RemoveAt(intFila);
                }
            }

            Session["dtTmpActa"] = dtActas;                                         // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvActaDiligenciamiento.DataSource = dtActas;
            gdvActaDiligenciamiento.DataBind();

            updActas.Update();
        }

        void EliminarParticipante(Int64 intParticipanteId)
        {

            DataTable dtPartipanteEliminados = new DataTable();
            Boolean intResult = false;
            DataTable dtPartipante = new DataTable();
            DataView dv = new DataView((DataTable)Session["dtTmpPartipante"]);
            dv.Sort = "ajpa_iNumero DESC";


            dtPartipante = dv.ToTable();


            if (Session["dtTmpPartipantesEliminados"] == null)
            {
                dtPartipanteEliminados = dtPartipante.Clone();
            }
            else
            {
                dtPartipanteEliminados = (DataTable)Session["dtTmpPartipantesEliminados"];
            }

            for (int i = 0; i <= dtPartipante.Rows.Count - 1; i++)
            {
                DataRow dr = dtPartipante.Rows[i];

                if (Convert.ToInt64(dr["ajpa_iActoJudicialParticipanteId"].ToString()) == intParticipanteId)
                {
                    dr["ajpa_sEstadoId"] = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.ANULADO);
                    if (intParticipanteId > 0)
                    {
                        EliminarPagos_paticipante((Int16)intParticipanteId);
                        dtPartipanteEliminados.ImportRow(dr);
                    }
                    dtPartipante.Rows.RemoveAt(i);
                    break;
                }
                else
                {
                    dr["ajpa_iNumero"] = Convert.ToInt64(dr["ajpa_iNumero"]) - 1;
                }

            }

            Session["dtTmpPartipantesEliminados"] = dtPartipanteEliminados;
            Session["dtTmpPartipante"] = dtPartipante;                             // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvExpNotificados.DataSource = dtPartipante;
            gdvExpNotificados.DataBind();


            BlanqueaPersonasNotifica();
            Session["intQueHace_Participante"] = 1;
            btnAceptarNotifica.Text = "Adicionar";
            updNotificados.Update();
        }

        void EliminarParticipante_pagos(Int64 intParticipanteId)
        {

            DataTable dtPartipanteEliminados = new DataTable();
            Boolean intResult = false;
            DataTable dtPartipante = new DataTable();
            DataView dv = new DataView((DataTable)Session["dtTmpPartipante"]);
            dv.Sort = "ajpa_iNumero DESC";


            dtPartipante = dv.ToTable();


            if (Session["dtTmpPartipantesEliminados"] == null)
            {
                dtPartipanteEliminados = dtPartipante.Clone();
            }
            else
            {
                dtPartipanteEliminados = (DataTable)Session["dtTmpPartipantesEliminados"];
            }

            for (int i = 0; i <= dtPartipante.Rows.Count - 1; i++)
            {
                DataRow dr = dtPartipante.Rows[i];

                if (Convert.ToInt64(dr["ajpa_iActoJudicialParticipanteId"].ToString()) == intParticipanteId)
                {
                    dr["ajpa_sEstadoId"] = Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.ANULADO);
                    dtPartipanteEliminados.ImportRow(dr);
                    dtPartipante.Rows.RemoveAt(i);
                    break;
                }
                else
                {
                    dr["ajpa_iNumero"] = Convert.ToInt64(dr["ajpa_iNumero"]) - 1;
                }

            }

            Session["dtTmpPartipantesEliminados"] = dtPartipanteEliminados;
            Session["dtTmpPartipante"] = dtPartipante;                             // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvExpNotificados.DataSource = dtPartipante;
            gdvExpNotificados.DataBind();


            BlanqueaPersonasNotifica();
            Session["intQueHace_Participante"] = 1;
            btnAceptarNotifica.Text = "Adicionar";
            updNotificados.Update();
        }

        bool BuscarNotificadoEnTemporal(DataTable dtTmpparticipa, Int64 intPersonaId, Int16 intTipoPersonaId, string strNumDocPersona, int strTipoDocPersona)
        {
            bool booOk = false;
            int intFila = 0;
            string strNumeroDocumento = "";
            string strTipoDocumento = "";

            for (intFila = 0; intFila <= dtTmpparticipa.Rows.Count - 1; intFila++)
            {
                strNumeroDocumento = dtTmpparticipa.Rows[intFila]["ajpa_vDocumentoNumero"].ToString();
                strTipoDocumento = dtTmpparticipa.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString();
                if ((strTipoDocumento == strTipoDocPersona.ToString()) && (strNumDocPersona == strNumeroDocumento) && (strNumDocPersona != ""))
                {
                    booOk = true;
                    break;
                }
            }

            return booOk;
        }

        void ActualizarTipoDocumento()
        {
            if (ddlTipPersona3.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.NATURAL).ToString())
            {
                DataTable dtTipDoc = new DataTable();
                dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                DataView dv = dtTipDoc.DefaultView;
                DataTable dtOrdenado = dv.ToTable();

                dtOrdenado.DefaultView.Sort = "Id ASC";
                Util.CargarDropDownList(ddlTipDoc3, dtOrdenado, "Valor", "Id", true);
                txtNom3.Enabled = true;
                txtApePat3.Enabled = true;
                txtApeMat3.Enabled = true;
            }
            else
            {
                Util.CargarParametroDropDownList(ddlTipDoc3, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO), true);
                txtNom3.Enabled = false;
                txtApePat3.Enabled = false;
                txtApeMat3.Enabled = false;
            }
            txtNumDoc3.Text = "";

            txtNom3.Text = "";
            txtApePat3.Text = "";
            txtApeMat3.Text = "";


        }

        void LlamarActuacion()
        {
            string codPersona = Request.QueryString["CodPer"].ToString();
            Session["IntTarifarioId"] = 0;
            Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = 0;
            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/frmActosgeneral.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";
                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/frmActosgeneral.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/frmActosgeneral.aspx?CodPer=" + codPersona, false);
                }
            }
            
        }

        void EnviarActas(Int64 iActaJudicialId)
        {
            ActaJudicialMantenimientoBL miFun = new ActaJudicialMantenimientoBL();
            Int16 sEstadoId = Convert.ToInt16(Enumerador.enmJudicialActaEstado.ENVIADO);
            Int16 sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            Int16 sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string vHostName = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);
            int intResultado = 0;

            intResultado = miFun.Actualizar_Estado(iActaJudicialId, sEstadoId, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName, "");

            if (intResultado == 1)
            {
                Validation3.MostrarValidacion(strMensajeActaEnviada, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                Validation3.MostrarValidacion(strMensajeActaNoEnviada, true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        bool ActualizarEstadoExpediente(Int64 iActoJudicialId, Int16 intEstadoId)
        {
            int intResultado = 0;
            bool bOk = false;
            ActoJudicialMantenimientoBL funJudicial = new ActoJudicialMantenimientoBL();

            Int16 intUsuarioid = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            Int16 intOficinaId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string strHostname = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

            intResultado = funJudicial.ActualizarEstado(iActoJudicialId, intEstadoId, intUsuarioid, strDireccionIP, intOficinaId, strHostname);

            if (intResultado == 1) { bOk = true; }

            return bOk;
        }

        bool SaberSiCerramos(Int64 iActuacionId)
        {
            bool booOk = false;
            DataTable dtresult = new DataTable();
            Int64 iActuId = Convert.ToInt64(Session["intActoJudicialId"]);

            ActoJudicialConsultaBL FunActuacion = new ActoJudicialConsultaBL();

            dtresult = FunActuacion.SaberSiCerramos(iActuId);

            if (dtresult.Rows.Count != 0)
            {
                int intNumParticipante = Convert.ToInt16(dtresult.Rows[0]["NumParticipante"].ToString());
                int intNumActasEnviadas = Convert.ToInt16(dtresult.Rows[0]["NumActasEnviadas"].ToString());

                if (intNumParticipante != intNumActasEnviadas)
                {
                    booOk = false;
                }
                else
                {
                    booOk = true;
                }
            }
            return booOk;
        }

        protected void gdvActaDiligenciamiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intQueHace = Convert.ToInt32(Session["IQueHace"]);                             // VARIABLE QUE NOS INDICA EN QUE MODO SE ENCUENTRA EL FORMULARIO

            int index = Convert.ToInt32(e.CommandArgument);
            Int64 iActaJudicialId = Convert.ToInt32(gdvActaDiligenciamiento.DataKeys[index].Values["acjd_iActaJudicialId"].ToString());
            Int16 sEstadoId = Convert.ToInt16(gdvActaDiligenciamiento.DataKeys[index].Values["acjd_sEstadoId"].ToString());
            Int16 sGuardado = Convert.ToInt16(gdvActaDiligenciamiento.DataKeys[index].Values["acjd_sGuardado"].ToString());

            Int16 sActuacionDetalleEdtadoId = Convert.ToInt16(gdvActaDiligenciamiento.DataKeys[index].Values["acde_sEstadoId"].ToString());

            Session["iActaJudicialId"] = iActaJudicialId;

            if (e.CommandName == "Ver")
            {
                MostrarActas(iActaJudicialId, 1);
                BloquearControlesActa(false);
            }

            if (e.CommandName == "Editar")
            {

                if (sEstadoId != Convert.ToInt16(Enumerador.enmJudicialActaEstado.REGISTRADO))
                {
                    Validation3.MostrarValidacion("Solo se puede editar actas con estado REGISTRADO", true, Enumerador.enmTipoMensaje.WARNING);
                    updActas.Update();
                }
                else
                {
                    MostrarActas(iActaJudicialId, 2);
                    BloquearControlesActa(true);
                }
            }

            if (e.CommandName == "Enviar")
            {
                Int64 iActoJudicialNotificacionId = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);
                ImprimirActaDiligenciamiento();
            }

            if (e.CommandName == "Observar")
            {
                if (sGuardado == 0)
                {
                    Validation3.MostrarValidacion("Se requiere grabar el acta antes de ejecutar esta acción", true, Enumerador.enmTipoMensaje.WARNING);
                    updActas.Update();
                    return;
                }

                if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialActaEstado.ENVIADO))
                {
                    txtDescSub.Text = string.Empty;
                    ModalPanel_Cab.Show();
                }
                else
                {
                    if (sEstadoId == Convert.ToInt16(Enumerador.enmJudicialActaEstado.OBSERVADO))
                    {
                        Validation3.MostrarValidacion("El acta ya fue Observada", true, Enumerador.enmTipoMensaje.WARNING);
                    }
                    else
                    {
                        Validation3.MostrarValidacion("No puede observar un acta que no ha sido enviada", true, Enumerador.enmTipoMensaje.WARNING);
                    }
                    updActas.Update();
                }
            }

            if (e.CommandName == "Eliminar")
            {
                if (sEstadoId != Convert.ToInt16(Enumerador.enmJudicialActaEstado.REGISTRADO))
                {
                    Validation3.MostrarValidacion("Solo se puede eliminar actas con estado REGISTRADO", true, Enumerador.enmTipoMensaje.WARNING);
                    updActas.Update();
                }
                else
                {
                    EliminarActas(iActaJudicialId);
                }
            }
        }

        void BloquearControlesActa(bool bAction)
        {
            ddlActaTipo.Enabled = bAction;
            txtActaFecha.Enabled = bAction;
            txtActaHora.Enabled = bAction;
            txtActaResponsable.Enabled = bAction;
            txtActaCuerpo.Enabled = bAction;
            //---------------------------------------------------------------
            //Fecha: 17/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Eliminar el campo: Resultado.
            //---------------------------------------------------------------
            //txtResultado.Enabled = bAction;
            txtActaObservacion.Enabled = bAction;
            btnActaAceptar.Enabled = bAction;

        }

        void MostrarActas(Int64 iActaJudicialId, int intTipoVizualizacion)
        {
            int intFila = 0;
            DataTable dtActas = new DataTable();
            DateTime datFecha;

            dtActas = (DataTable)Session["dtTmpActa"];
            String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            for (intFila = 0; intFila <= dtActas.Rows.Count - 1; intFila++)
            {
                if (iActaJudicialId == Convert.ToInt64(dtActas.Rows[intFila]["acjd_iActaJudicialId"].ToString()))
                {
                    datFecha = Comun.FormatearFecha(dtActas.Rows[intFila]["acjd_dFechaHoraActa"].ToString());

                    ddlActaTipo.SelectedValue = dtActas.Rows[intFila]["acjd_sTipoActaId"].ToString();
                    txtActaFecha.Text = datFecha.ToString(FormatoFechas);
                    txtActaHora.Text = datFecha.ToString("hh:mm");
                    ddlActaEstado.SelectedValue = dtActas.Rows[intFila]["acjd_sEstadoId"].ToString();

                    txtActaResponsable.Text = dtActas.Rows[intFila]["acjd_vResponsable"].ToString();
                    txtActaCuerpo.Text = dtActas.Rows[intFila]["acjd_vCuerpoActa"].ToString();
                    //---------------------------------------------------------------
                    //Fecha: 17/02/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Eliminar el campo: Resultado.
                    //---------------------------------------------------------------
                    //txtResultado.Text = dtActas.Rows[intFila]["acjd_vResultado"].ToString();
                    txtActaObservacion.Text = dtActas.Rows[intFila]["acjd_vObservaciones"].ToString();

                    break;
                }
            }
            if (intTipoVizualizacion == 2) { btnActaAceptar.Text = "Actualizar"; }

            updActas.Update();
        }

        void EliminarPagos(Int64 intPagoId)
        {
            DataTable dtTMPPagos = new DataTable();
            dtTMPPagos = (DataTable)Session["dtTmpRePagos"];

            for (int i = dtTMPPagos.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtTMPPagos.Rows[i];
                if (Convert.ToInt64(dr["pago_iPagoId"].ToString()) == intPagoId)
                    dr.Delete();
                if (intPagoId > 0)
                {
                    EliminarParticipante_pagos(intPagoId);
                }
            }

            Session["dtTmpRePagos"] = dtTMPPagos;                                                // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvPagos.DataSource = dtTMPPagos;
            gdvPagos.DataBind();

            updNotificados.Update();
        }

        void EliminarPagos_paticipante(Int64 intPagoId)
        {
            DataTable dtTMPPagos = new DataTable();
            dtTMPPagos = (DataTable)Session["dtTmpRePagos"];

            for (int i = dtTMPPagos.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtTMPPagos.Rows[i];
                if (Convert.ToInt64(dr["pago_iPagoId"].ToString()) == intPagoId)
                    dr.Delete();
            }

            Session["dtTmpRePagos"] = dtTMPPagos;                                                // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

            gdvPagos.DataSource = dtTMPPagos;
            gdvPagos.DataBind();

            updNotificados.Update();
        }


        bool CantidadParticipanteYPagosSoncorrectos(DataTable dtParticipantes, DataTable dtRePagos)
        {
            bool booOk = false;
            int intFila = 0;
            int intFila2 = 0;
            int intNumeroPagados = 0;
            int intNumParticipante = dtParticipantes.Rows.Count;
            DataTable dtTarifa = (DataTable)Session["dtTarifa"];

            for (intFila = 0; intFila <= dtRePagos.Rows.Count - 1; intFila++)
            {
                for (intFila2 = 0; intFila2 <= dtTarifa.Rows.Count - 1; intFila2++)
                {
                    Int16 intTarifaPagadaId = Convert.ToInt16(dtRePagos.Rows[intFila]["pago_sTarifarioId"].ToString());
                    Int16 intTarifaId = Convert.ToInt16(dtTarifa.Rows[intFila2]["tari_sTarifarioId"].ToString());

                    if (intTarifaId == intTarifaPagadaId)
                    {
                        double douValorTarifaPagada = Convert.ToDouble(dtRePagos.Rows[intFila]["pago_FMontoSolesConsulares"].ToString());
                        double douValorTarifa = Convert.ToInt16(dtTarifa.Rows[intFila2]["tari_FCosto"].ToString());

                        int intCantiPagados = (int)(douValorTarifaPagada / douValorTarifa);

                        if (Convert.ToBoolean(dtRePagos.Rows[intFila]["pago_bPagadoFlag"]))
                        {
                            intNumeroPagados = (intNumeroPagados + intCantiPagados);
                        }
                        else
                            intNumeroPagados++;
                        break;
                    }
                }
            }

            if (intNumParticipante == intNumeroPagados)
            {
                booOk = true;
            }
            return booOk;
        }

        void ActivarControlesTipoPago()
        {
            // SI EL TIPO DE PAGO ES "EFECTIVO" O "NO PAGADO" DESACTIVAMOS BANCO Y NUMERO DE CHEQUE
            if ((Convert.ToString(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.POR_CORREO).ToString()) ||
                (Convert.ToString(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.EFECTIVO).ToString()) ||
                (Convert.ToString(ddlTipoPago.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoCobroActuacion.NO_COBRADO).ToString()))
            {
                ddlBanco.Enabled = false;
                txtNumCheque.Enabled = false;

                ddlBanco.SelectedValue = "0";
                txtNumCheque.Text = "";
            }
            else
            {
                ddlBanco.Enabled = true;
                txtNumCheque.Enabled = true;
            }
        }

        protected void btn_imprimir_expediente_Click(object sender, EventArgs e)
        {
            var index = Convert.ToInt32(hIndex.Value);
            Int64 iActoJudicialNotificacionId = Convert.ToInt64(hId_Actuacion_Select.Value);
            Int64 iActoJudicialParticipanteId = Convert.ToInt64(Session["iActoJudicialParticipanteId"]);

            GenerarNotificacion(iActoJudicialParticipanteId, iActoJudicialNotificacionId);
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            int intQueHace_Participante = Convert.ToInt32(Session["intQueHace_Participante"]);
            int iParticipantePersonaId = Convert.ToInt32(Session["intParticipaPersonaId"]);

            if (gdvExpNotificados.Rows.Count > 0)
            {
                foreach (GridViewRow row in gdvExpNotificados.Rows)
                {
                    if (row.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpNotificados, "ajpa_iPersonaId")].Text == iParticipantePersonaId.ToString())
                    {
                        if (row.Cells[Util.ObtenerIndiceColumnaGrilla(gdvExpNotificados, "ajpa_vDocumentoNumero")].Text == txtNumDoc3.Text)
                        {
                            return;
                        }
                    }
                }

            }


            string strNombre = "";
            string strNumeroDigitos = "";
            bool booEsCorto = false;

            Session["intParticipaPersonaId"] = 0;
            Session["intParticipaEmpresaId"] = 0;


            if (ddlTipPersona3.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.NATURAL).ToString())                      // SI ES PERSONA JURIDICA
            {
                if (ddlTipDoc3.SelectedValue == "1")                         // SI ES DNI
                {
                    if (txtNumDoc3.Text.Length < 8)
                    {
                        strNumeroDigitos = "8"; booEsCorto = true;
                    }
                }
            }
            else
            {
                if (txtNumDoc3.Text.Length < 11)
                {
                    strNumeroDigitos = "11"; booEsCorto = true;
                }
            }

            if (booEsCorto == true)
            {
                ValParticipante.MostrarValidacion("El Número de dígitos para este tipo de documento debe de ser igual a " + strNumeroDigitos, true, Enumerador.enmTipoMensaje.WARNING);
                updNotificados.Update();
                return;
            }

            if (ddlTipPersona3.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.NATURAL).ToString())
            {
                EnPersona objEn = new EnPersona();
                objEn.sPersonaTipoId = Convert.ToInt16(ddlTipPersona3.Text);
                objEn.sDocumentoTipoId = Convert.ToInt16(ddlTipDoc3.SelectedValue);
                objEn.vDocumentoNumero = txtNumDoc3.Text;

                object[] arrParametros = { objEn };
                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

                txtNom3.Text = objEn.vNombres;
                txtApePat3.Text = objEn.vApellidoPaterno;
                txtApeMat3.Text = objEn.vApellidoMaterno;

                strNombre = txtNom3.Text + txtApePat3.Text + txtApeMat3.Text;
                Session["intParticipaPersonaId"] = objEn.iPersonaId;
            }
            else
            {
                DataTable DtEmpresa = new DataTable();

                Proceso MiProc = new Proceso();
                string strNomrazonSocial = "";
                int intPaginaActual = 1;
                int intPaginaSize = 10;
                Object[] miArray = new Object[6] { txtNumDoc3.Text, strNomrazonSocial, intPaginaActual, intPaginaSize, 0, 0 };

                DtEmpresa = (DataTable)MiProc.Invocar(ref miArray, "SGAC.BE.RE_ACTUACION", "CONSULTAREMPRESA");

                if (DtEmpresa.Rows.Count > 0)
                {
                    txtNom3.Text = DtEmpresa.Rows[0]["empr_vRazonSocial"].ToString();
                    strNombre = DtEmpresa.Rows[0]["empr_vRazonSocial"].ToString();
                    txtApePat3.Text = "";
                    txtApeMat3.Text = "";
                    Session["intParticipaEmpresaId"] = Convert.ToInt32(DtEmpresa.Rows[0]["empr_iEmpresaId"].ToString());
                }
                else
                {
                    txtNom3.Text = "";
                    txtApePat3.Text = "";
                    txtApeMat3.Text = "";
                    Session["intParticipaEmpresaId"] = 0;
                }
            }

            if (strNombre == "")
            {
                txtNom3.Enabled = true;
                txtApePat3.Enabled = true;
                txtApeMat3.Enabled = true;

                txtNom3.Enabled = true;

                if (ddlTipPersona3.SelectedValue == ((int)Enumerador.enmTipoPersona.NATURAL).ToString())
                {
                    txtApePat3.Enabled = true;
                    txtApeMat3.Enabled = true;
                }
                else
                {
                    txtApePat3.Enabled = false;
                    txtApeMat3.Enabled = false;
                }
                return;
            }
            else
            {
                txtNom3.Enabled = false;
                txtApePat3.Enabled = false;
                txtApeMat3.Enabled = false;
            }
        }

        protected void ddlTipoNotifica_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarTipoNotificacion();
        }

        void ValidarTipoNotificacion()
        {
            if (Convert.ToInt16(ddlTipoNotifica.SelectedValue) == Convert.ToInt16(Enumerador.enmJudicialTipoNotificacion.NOTICACION_ADMINISTRATIVA))
            {
                txtOrgano.Text = "";

                txtOrgano.Enabled = false;
                ddlEntSoli.Enabled = true;
                lblCO_ddlTipoNotifica.Visible = false;
                lblCO_ddlEntSoli.Visible = true;

                lblCO_txtNumExp.Visible = false;
            }
            else
            {
                ddlEntSoli.SelectedValue = "0";

                txtOrgano.Enabled = true;
                ddlEntSoli.Enabled = false;
                lblCO_ddlTipoNotifica.Visible = true;
                lblCO_ddlEntSoli.Visible = false;

                lblCO_txtNumExp.Visible = true;
            }
            updNotificados.Update();
        }

        protected void imgCerrar_Click(object sender, ImageClickEventArgs e)
        {
            ModalPanel_Cab.Hide();
            updActas.Update();
        }

        protected void btngrabarSubServicio_Click(object sender, EventArgs e)
        {
            if (txtDescSub.Text.Trim() == string.Empty)
            {
                Validation3.MostrarValidacion("No ha indicado la Observación del Acta", true, Enumerador.enmTipoMensaje.INFORMATION);
                updActas.Update();

                ModalPanel_Cab.Show();
                return;
            }

            Int64 intActoJudicialId = Convert.ToInt64(Session["iActoJudicialId"]);
            Int16 intEstadoExpedienteId = Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.OBSERVADO);

            ActaJudicialMantenimientoBL miFun = new ActaJudicialMantenimientoBL();
            Int64 iActaJudicialId = Convert.ToInt64(Session["iActaJudicialId"]);
            Int16 sEstadoActaId = Convert.ToInt16(Enumerador.enmJudicialActaEstado.OBSERVADO);
            Int16 sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            Int16 sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            string vHostName = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);
            string strObservaciones = txtDescSub.Text.ToUpper();
            int intResultado = 0;

            intResultado = miFun.Actualizar_Estado(iActaJudicialId, sEstadoActaId, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName, strObservaciones);

            if (intResultado == 1)
            {
                Validation3.MostrarValidacion("El acta se Observó con éxito", true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                Validation3.MostrarValidacion("No se pudo Observar el Acta", true, Enumerador.enmTipoMensaje.WARNING);
            }

            // MOSTRAMOS LOS DATOS ACTUALIZADOS DEL ACTA
            BlanqueaActas();
            Int64 iActoJudicialNotificacionId = Convert.ToInt64(Session["iActoJudicialNotificacionId"]);
            CargarActas(iActoJudicialNotificacionId);


            updActas.Update();
        }


        protected void btncancelarSub_Click(object sender, EventArgs e)
        {
            ModalPanel_Cab.Hide();
            updActas.Update();
        }

        protected void btnModificarSub_Click(object sender, EventArgs e)
        {
        }

        protected void gdvNotificaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell2 = new TableCell();
                HeaderRow.ForeColor = Color.White;

                HeaderCell2.Text = "";
                HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell2.VerticalAlign = VerticalAlign.Middle;
                HeaderCell2.RowSpan = 1;
                HeaderRow.Cells.Add(HeaderCell2);


                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "";
                HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell2.VerticalAlign = VerticalAlign.Middle;
                HeaderCell2.RowSpan = 1;
                HeaderRow.Cells.Add(HeaderCell2);

                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "E N V I O ";
                HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell2.ColumnSpan = 2;
                //--------------------------------------------------------------------
                //Fecha: 08/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar el color blanco a las letras
                //--------------------------------------------------------------------
                HeaderCell2.ForeColor = ColorTranslator.FromHtml("#FFFFFF");  
                HeaderRow.Cells.Add(HeaderCell2);

                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "R E S P U E S T A";
                HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell2.ColumnSpan = 3;
                //--------------------------------------------------------------------
                //Fecha: 08/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar el color blanco a las letras
                //--------------------------------------------------------------------
                HeaderCell2.ForeColor = ColorTranslator.FromHtml("#FFFFFF");  
                HeaderRow.Cells.Add(HeaderCell2);

                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "";
                HeaderCell2.ColumnSpan = 1;
                HeaderRow.Cells.Add(HeaderCell2);


                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "";
                HeaderCell2.ColumnSpan = 1;
                HeaderRow.Cells.Add(HeaderCell2);


                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

                if (intOficinaConsularId != intOficinaConsularLimaId)
                {
                    Int16 intEstadiParticipante = Convert.ToInt16(Session["ParticipanteEstadoId"]);
                    if (Convert.ToInt16(Session["IQueHace"]) == 2)
                    {
                        if (intEstadiParticipante != Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.CERRADO))
                        {
                            HeaderCell2 = new TableCell();
                            HeaderCell2.Text = "";
                            HeaderCell2.ColumnSpan = 1;
                            HeaderRow.Cells.Add(HeaderCell2);

                            HeaderCell2 = new TableCell();
                            HeaderCell2.Text = "";
                            HeaderCell2.ColumnSpan = 1;
                            HeaderRow.Cells.Add(HeaderCell2);
                        }
                    }
                }

                HeaderRow.BackColor = ColorTranslator.FromHtml("#4E102E");
                HeaderRow.Font.Name = "Arial";
                HeaderRow.Font.Size = 10;
                HeaderRow.Font.Bold = true;

                gdvNotificaciones.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void ddlTipoRecepcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(ddlTipoRecepcion.Text) == Convert.ToInt16(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO) ||
                Convert.ToInt16(ddlTipoRecepcion.Text) == Convert.ToInt16(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR) ||
                Convert.ToInt16(ddlTipoRecepcion.Text) == Convert.ToInt16(Enumerador.enmJudicialTipoRecepcion.RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO))
            {
                txtPerRecep.Enabled = true;
                txtFchRecep.Enabled = true;
                txtHoraRecep.Enabled = true;
            }
            else if (Convert.ToInt16(ddlTipoRecepcion.Text) == Convert.ToInt16(Enumerador.enmJudicialTipoRecepcion.DEJADO_BAJO_LA_PUERTA) ||
            Convert.ToInt16(ddlTipoRecepcion.Text) == Convert.ToInt16(Enumerador.enmJudicialTipoRecepcion.DEJADO_EN_EL_BUZON))
            {
                txtFchRecep.Enabled = true;
                txtHoraRecep.Enabled = true;
            }
            else
            {
                txtFchRecep.Text = string.Empty;
                txtHoraRecep.Text = string.Empty;
                txtPerRecep.Text = string.Empty;

                txtPerRecep.Enabled = false;
                txtFchRecep.Enabled = false;
                txtHoraRecep.Enabled = false;
            }
        }

        protected void txtNumExp_TextChanged(object sender, EventArgs e)
        {
            if (txtNumExp.Text != "")
            {
                int intFila = 0;
                DataTable DtParticipante = new DataTable();

                DtParticipante = (DataTable)Session["dtTmpPartipante"];

                if (DtParticipante.Rows.Count != 0)
                {
                    for (intFila = 0; intFila <= DtParticipante.Rows.Count - 1; intFila++)
                    {
                        DtParticipante.Rows[intFila]["ajpa_vNroExpediente"] = txtNumExp.Text;
                    }

                    Session["dtTmpPartipante"] = DtParticipante;            // SUBIMOS EL TEMPORAL PARA ALMACENARLO EN MEMORIA

                    gdvExpNotificados.DataSource = DtParticipante;
                    gdvExpNotificados.DataBind();

                    updNotificados.Update();
                }
                string str_val = txtNumOficio.Text;

                txtNumOficio.Focus();
            }
        }

        void LimpiarPantallaNotificacion()
        {
            ddlViaEnvio.SelectedValue = "0";
            txtEmpPostal.Text = string.Empty;
            txtPersNotifica.Text = string.Empty;
            txtFechaNotifica.Text = string.Empty;
            txtHoraNotifica.Text = string.Empty;
            txtNotificacionCuerpo.Text = string.Empty;

            ddlTipoRecepcion.SelectedValue = "0";
            txtNroCedula.Text = string.Empty;
            txtPerRecep.Text = string.Empty;
            txtFchRecep.Text = string.Empty;
            txtHoraRecep.Text = string.Empty;
            txtNotiObservacion.Text = string.Empty;
        }

        void botonAdicionar()
        {
            btnNotiAceptar.Text = "Adicionar";
            btnNotiAceptar.CssClass = "btnNew";
        }

        void botonActualizar()
        {
            btnNotiAceptar.Text = "Actualizar";
            btnNotiAceptar.CssClass = "btnSave";
        }


        void EjecutarScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopUp" + ObtenerFechaActual(HttpContext.Current.Session).Ticks.ToString(), script, true);
        }

        #region WebMethods

        [System.Web.Services.WebMethod]
        public static string GetSession(string variable)
        {
            string strReturn = string.Empty;
            if (HttpContext.Current.Session[variable] != null)
                strReturn = HttpContext.Current.Session[variable].ToString();
            return strReturn.Trim();
        }

        #endregion

        void ImprimirActaDiligenciamiento()
        {
            DataTable dtActas = new ActaJudicialConsultaBL().Obtener(Convert.ToInt64(Session["iActoJudicialNotificacionId"]));

            if (dtActas.Rows.Count == 0)
            {
                Validation3.MostrarValidacion("Debe grabar el acta para imprimir", true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }

            string s_Cuerpo = string.Empty;
            string s_Resultado = string.Empty;
            string s_Observacion = string.Empty;
            string strRutaHtml = string.Empty;
            string strArchivoPDF = string.Empty;

            foreach (DataRow dr in dtActas.Rows)
            {
                if (dr["acjd_iActaJudicialId"].ToString() == Session["iActaJudicialId"].ToString())
                {
                    s_Cuerpo = dr["acjd_vCuerpoActa"].ToString();
                    s_Resultado = dr["acjd_vResultado"].ToString();
                    s_Observacion = dr["acjd_vObservaciones"].ToString();

                    if (dr["acjd_sTipoActaId"].ToString() == Convert.ToInt32(Enumerador.enmJudicialTipoActa.ACTA_COMPLEMENTARIA).ToString())
                    {
                        strRutaHtml = Server.MapPath("~/Registro/Plantillas/acta-complementaria.html");
                        strArchivoPDF = "acta-complementaria.pdf";
                    }
                    else
                    {
                        strRutaHtml = Server.MapPath("~/Registro/Plantillas/acta-diligenciamiento.html");
                        strArchivoPDF = "acta-diligenciamiento.pdf";
                    }

                    break;
                }
            }

            var dtTMPReemplazar = CrearTmpTabla();

            DataRow row = dtTMPReemplazar.NewRow(); row["strCadenaBuscar"] = "[strCuerpo]"; row["strCadenaReemplazar"] = s_Cuerpo; dtTMPReemplazar.Rows.Add(row);
            DataRow row1 = dtTMPReemplazar.NewRow(); row1["strCadenaBuscar"] = "[strResultado]"; row1["strCadenaReemplazar"] = s_Resultado; dtTMPReemplazar.Rows.Add(row1);
            //DataRow row2 = dtTMPReemplazar.NewRow(); row2["strCadenaBuscar"] = "[strObservacion]"; row2["strCadenaReemplazar"] = s_Observacion; dtTMPReemplazar.Rows.Add(row2);
            DataRow row2 = dtTMPReemplazar.NewRow(); row2["strCadenaBuscar"] = "[strObservacion]"; row2["strCadenaReemplazar"] = (s_Observacion != "" ? "" : s_Observacion) /* El tiket 361  nos indica que no deberia mostrase la observacion */; dtTMPReemplazar.Rows.Add(row2);
            DataRow row4 = dtTMPReemplazar.NewRow(); row4["strCadenaBuscar"] = "[Consulado]"; row4["strCadenaReemplazar"] = Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE]; dtTMPReemplazar.Rows.Add(row4);
            DataRow row3 = dtTMPReemplazar.NewRow(); row3["strCadenaBuscar"] = "[Logo]"; dtTMPReemplazar.Rows.Add(row3);

            Util.DataTableVarcharMayusculas(dtTMPReemplazar);


            String localfilepath = String.Empty;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            string strRutaPDF = uploadPath + @"\" + strArchivoPDF;

            //  UIConvert.GenerarPDF(dtTMPReemplazar, strRutaHtml, strRutaPDF);
            CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, HttpContext.Current.Server.MapPath("~/Images/Escudo.PNG"));

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {


                    Session["binaryData"] = FileBuffer;
                    string strUrl = "../Accesorios/VisorPDF.aspx";
                    string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                    EjecutarScript(strScript);
                }
            }
        }

        protected void gdvActaDiligenciamiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void chkGratuito_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGratuito != null)
            {
                if (chkGratuito.Checked)
                {
                    txtNumOrdPag.Enabled = false;
                    txtNumOrdPag.Text = string.Empty;

                    txtCosto.Enabled = false;
                    txtCosto.Text = "0";

                    txtCosto2.Enabled = false;
                    txtCosto2.Text = "0";

                    ddlBanco.Enabled = false;
                    ddlBanco.SelectedValue = "0";

                    txtNumCheque.Enabled = false;
                    txtNumCheque.Text = "";

                    txtFchPago.Enabled = false;
                    txtFchPago.Text = string.Empty;

                    txtMotivoGratuidad.Visible = true;
                    lblTituloMotivoGratuito.Visible = true;

                    ddlTipoPago.SelectedValue = Convert.ToString(((Int16)Enumerador.enmTipoCobroActuacion.GRATIS));

                }
                else
                {
                    txtNumOrdPag.Enabled = true;


                    ddlBanco.Enabled = true;

                    txtNumCheque.Enabled = true;

                    txtFchPago.Enabled = true;

                    txtMotivoGratuidad.Visible = false;
                    lblTituloMotivoGratuito.Visible = false;

                    ddlTipoPago.SelectedValue = Convert.ToString(((Int16)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA));

                }

                updNotificados.Update();
            }

        }


        #region General
        internal string NumeroOrdinal(int s_numero)
        {
            string v_Numero = string.Empty;
            switch (s_numero)
            {
                case 1:
                    v_Numero = "PRIMERA";
                    break;
                case 2:
                    v_Numero = "SEGUNDA";
                    break;
                case 3:
                    v_Numero = "TERCERA";
                    break;
                case 4:
                    v_Numero = "CUARTA";
                    break;
                case 5:
                    v_Numero = "QUINTA";
                    break;
                case 6:
                    v_Numero = "SEXTA";
                    break;
                case 7:
                    v_Numero = "SÉPTIMA";
                    break;
                case 8:
                    v_Numero = "OCTAVA";
                    break;
                case 9:
                    v_Numero = "NOVENA";
                    break;
                case 10:
                    v_Numero = "DÉCIMA";
                    break;
                case 11:
                    v_Numero = "UNDÉCIMA";
                    break;
                case 12:
                    v_Numero = "DUODÉCIMA";
                    break;
                case 13:
                    v_Numero = "DECIMOTERCERA";
                    break;
                case 14:
                    v_Numero = "DECIMOCUARTA";
                    break;
                case 15:
                    v_Numero = "DECIMOQUINTA";
                    break;
                case 16:
                    v_Numero = "DECIMOSEXTA";
                    break;
                case 17:
                    v_Numero = "DECIMOSÉPTIMA";
                    break;
                case 18:
                    v_Numero = "DECIMOOCTAVA";
                    break;
                case 19:
                    v_Numero = "DECIMONOVENA";
                    break;
                case 20:
                    v_Numero = "VIGÉSIMA";
                    break;

            }

            return v_Numero;
        }
        #endregion


        #region metodos_generales

        public void modificar_Notificaciones()
        {
            if (Session["dtTmpNotificacion"] != null)
            {
                foreach (DataRow item in ((DataTable)Session["dtTmpNotificacion"]).Rows)
                {
                    if (item["ajno_iActoJudicialParticipanteId"].ToString().Trim().Length > 0 && item["ajno_stipoRecepcionId"].ToString().Trim().Length > 0 && item["ajno_vTipRecepcion"].ToString() != Constantes.CONST_VALOR_OTROS)
                    {
                        gdvNotificaciones.Columns[10].Visible = false;
                        gdvNotificaciones.Columns[11].Visible = false;
                    }
                }    
            }            
        }

        #endregion 
        private void GetDataPersona(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            try
            {
                DataTable dt = new DataTable();
                SGAC.Registro.Persona.BL.PersonaConsultaBL objPersonaBL = new SGAC.Registro.Persona.BL.PersonaConsultaBL();
                EmpresaConsultaBL objEmpresa = new EmpresaConsultaBL();

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    DataSet ds = objEmpresa.ConsultarId(LonPersonaId);
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = objPersonaBL.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
                }

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    ViewState["Nombre"] = string.Empty;
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vRazonSocial"].ToString();
                    ViewState["ApeMat"] = string.Empty;
                    ViewState["ApeCasada"] = string.Empty;
                    ViewState["Nombres"] = string.Empty;

                    ViewState["DescTipDoc"] = dt.Rows[0]["empr_vTipoDocumento"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNumeroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = string.Empty;
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = "2102";
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sTipoDocumentoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sTipoEmpresaId"].ToString();
                    ViewState["FecNac"] = string.Empty;
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = string.Empty;
                }
                else
                { // Persona natural
                    ViewState["Nombre"] = dt.Rows[0]["vNombres"].ToString();
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vApellidoPaterno"].ToString();
                    ViewState["ApeMat"] = dt.Rows[0]["vApellidoMaterno"].ToString();
                    ViewState["ApeCasada"] = dt.Rows[0]["vApellidoCasada"].ToString();
                    ViewState["Nombres"] = ViewState["ApePat"] + " " + ViewState["ApeMat"] + ViewState["ApeCasada"] + " , " + ViewState["Nombre"];

                    ViewState["DescTipDoc"] = dt.Rows[0]["vDescTipDoc"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = dt.Rows[0]["sNacionalidadId"].ToString();
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sDocumentoTipoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["FecNac"] = dt.Rows[0]["dNacimientoFecha"].ToString();
                    ViewState["PER_GENERO"] = dt.Rows[0]["sGeneroId"].ToString();
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = dt.Rows[0]["vTipoDocumento"].ToString();

                    ViewState["DtPersonaAct"] = null;
                }

                dt = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
