using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Configuration;
using System.Data;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using SGAC.BE;
using SGAC.Registro.Persona.BL;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Registro
{
    public partial class FrmTramite : MyBasePage
    {
        #region Campos
        private string strVariableDecision = "TramiteAnulacion_Decision";
        private string strAuditoriaDataAnulacion = "Auditoria_Data_Anulacion";

        private string strVariableActDT = "TRAMITE_DT";
        private string strVariableAccion = "Actuacion_Accion";

        private string strFuncionarioAnulaId = "FuncionarioAnulacionId";
        #endregion

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginadorActuacion.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginadorActuacion.Visible = false;
            ctrlPaginadorActuacion.PaginaActual = 1;

            ctrlPaginadorExpediente.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginadorExpediente.Visible = false;
            ctrlPaginadorExpediente.PaginaActual = 1;

            ctrlPaginadorProyecto.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginadorProyecto.Visible = false;
            ctrlPaginadorProyecto.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                try
                {
                    string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));

                    Session["IngresoReimprimir"] = 0;

                    if (Request.QueryString["GUID"] != null)
                    {
                        HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                        Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = "0";
                        Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = "0";
                    }
                    else
                    {
                        HFGUID.Value = "";
                        Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = "0";
                        Session[Constantes.CONST_SESION_ACTUACION_ID] = "0";
                    }
                    if (Session["strBusqueda"] != null)
                    {
                        Session.Remove("strBusqueda");
                    }

                    Session["iAnularActoNotarial"] = null;

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
                    
                        
                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    if (Session["iPersonaId" + HFGUID.Value] != null)
                    //    {
                    //        hid_iPersonaId.Value = Convert.ToString(Session["iPersonaId" + HFGUID.Value]);
                    //        if (Session["iDocumentoTipoId" + HFGUID.Value] != null)
                    //        {
                    //            hiDocumentoTipoId.Value = Convert.ToString(Session["iDocumentoTipoId" + HFGUID.Value]);
                    //        }
                    //        else
                    //        {
                    //            hiDocumentoTipoId.Value = "0";
                    //        }
                    //        if (Session["NroDoc" + HFGUID.Value] != null)
                    //        {
                    //            hNroDoc.Value = Session["NroDoc" + HFGUID.Value].ToString();
                    //        }
                    //        else
                    //        {
                    //            hNroDoc.Value = "";
                    //        }

                    //    }
                    //    else
                    //    {
                    //        Response.Redirect("~/Default.aspx",false);
                    //        return;
                    //    }

                    //    if (Session["iTipoId" + HFGUID.Value] != null)
                    //    {
                    //        if (Session["iTipoId" + HFGUID.Value].ToString() == "0")
                    //            Session["iTipoId" + HFGUID.Value] = (int)Enumerador.enmTipoPersona.NATURAL;
                    //    }
                    //    else
                    //    {
                    //        Session["iTipoId" + HFGUID.Value] = (int)Enumerador.enmTipoPersona.NATURAL;
                    //    }

                    //    if (Session["iTipoId" + HFGUID.Value] != null)
                    //        hid_iTipoId.Value = Convert.ToString(Session["iTipoId" + HFGUID.Value]);

                    //}
                    //else
                    //{
                        if (ViewState["iPersonaId"] != null)
                        {
                            hid_iPersonaId.Value = Convert.ToString(ViewState["iPersonaId"]);
                            if (ViewState["iDocumentoTipoId"] != null)
                            {
                                hiDocumentoTipoId.Value = Convert.ToString(ViewState["iDocumentoTipoId"]);
                            }
                            else
                            {
                                hiDocumentoTipoId.Value = "0";
                            }
                            if (ViewState["NroDoc"] != null)
                            {
                                hNroDoc.Value = ViewState["NroDoc"].ToString();
                            }
                            else
                            {
                                hNroDoc.Value = "";
                            }
                        }
                        else
                        {
                            Response.Redirect("~/Default.aspx",false);
                            return;
                        }
                        if (ViewState["iTipoId"] != null)
                        {
                            if (ViewState["iTipoId"].ToString() == "0")
                                ViewState["iTipoId"] = (int)Enumerador.enmTipoPersona.NATURAL;
                        }
                        else
                        {
                            ViewState["iTipoId"] = (int)Enumerador.enmTipoPersona.NATURAL;
                        }

                        if (ViewState["iTipoId"] != null)
                            hid_iTipoId.Value = Convert.ToString(ViewState["iTipoId"]);
                    //}


                    if (Convert.ToInt16(hid_iTipoId.Value) == (int)Enumerador.enmTipoPersona.NATURAL)
                    {
                        PersonaConsultaBL oPersonaBL = new PersonaConsultaBL();
                        DataTable dt = new DataTable();
                        dt = oPersonaBL.PersonaGetById(Convert.ToInt64(hid_iPersonaId.Value));
                        Boolean bFallecidoFlag = false;
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["pers_bFallecidoFlag"] != null)
                                {
                                    bFallecidoFlag = Convert.ToBoolean(dt.Rows[0]["pers_bFallecidoFlag"]);
                                }
                                DateTime dFechaNac;
                                if (dt.Rows[0]["dNacimientoFecha"].ToString() != "")
                                {
                                    dFechaNac = Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString());
                                    hEdad.Value = ObtenerEdad(dFechaNac, true);
                                }
                                else
                                {
                                    hEdad.Value = "200";
                                }
                            }
                        }

                        if (bFallecidoFlag)
                        {
                            btnNuevaAct.Enabled = false;
                            hid_bFallecidoFlag.Value = "1";
                            ctrlValActuacion.MostrarValidacion(Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, true, Enumerador.enmTipoMensaje.ERROR);
                        }
                    }

                    CargarListadosDesplegables();
                    CargarDatosIniciales();
                    CargarDatosSolicitante();
                    CargarPestanias();

                    //----------------------------------------------------
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Deshabilitar la ejecución de protocolares
                    //----------------------------------------------------
                    //btnNuevoProtocolar.Enabled = false;
                    btnBuscarProyecto_Click(sender, e);

                    //--------------------------------------------------------
                    //Fecha: 05/02/2020
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Conocer si una Oficina Consular esta activa. 
                    //--------------------------------------------------------

                    Int16 intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    
                    SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL OficinaBL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();
                    string strOficinaActiva = "N";
                    strOficinaActiva = OficinaBL.OficinaEsActiva(intOficinaConsular);

                    //----------------------------------------------------
                    if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA" || strOficinaActiva == "N")
                    {
                        Button[] arrButtons = { btnNuevaAct, btnNuevoExpediente, btnNuevoExtraPro, btnNuevoProtocolar };

                        Comun.ModoLectura(ref arrButtons);
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
        }
        private string ObtenerEdad(DateTime dFechaNac, bool numerico = false)
        {
            DateTime datFechaHoy = new DateTime();
            datFechaHoy = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));

            string strEdad = Comun.DiferenciaFechas(datFechaHoy, dFechaNac, "--", numerico);

            return strEdad;
        }
        protected void ctrlPaginadorActuacion_Click(object sender, EventArgs e)
        {
            CargarGrillaActuaciones();
            updConsultaActuacion.Update();
        }
        protected void ctrlPaginadorExpediente_Click(object sender, EventArgs e)
        {
            //CargarGrillaExpediente();
            btnBuscarExpediente_Click(null, null);
            updConsultaExpediente.Update();
        }
        protected void ctrlPaginadorProyecto_Click(object sender, EventArgs e)
        {
            CargarGrillaProyecto();
        }

        protected void btnActualizarDatos_Click(object sender, EventArgs e)
        {
            long lngIdentificador = 0;
            Session["strBusqueda"] = "AC";
            lngIdentificador = Convert.ToInt64(hid_iPersonaId.Value);

            Int16 intDocumentoId = Convert.ToInt16(hiDocumentoTipoId.Value);
            string strDocumentoNumero = hNroDoc.Value.ToString();

            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            //DataTable _Dt = new DataTable();
            //_Dt = ObtenerDatosPersona(lngIdentificador, intDocumentoId, strDocumentoNumero);
            //Session["DtPersonaAct"] = _Dt;
            if (Convert.ToInt32(hid_iTipoId.Value) == (int)Enumerador.enmTipoPersona.JURIDICA)
            {               
                Response.Redirect("~/Registro/FrmRegistroEmpresa.aspx?CodPer=" + codPersona + "&Juridica=1", false);               
            }
            else
            {
                //------------------------------------------------------
                //Fecha: 19/10/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Obtener el tipo y numero de documento
                //------------------------------------------------------
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/FrmRegistroPersona.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmRegistroPersona.aspx?CodPer=" + codPersona, false);
                }
            }
        }

        protected void gdvActuaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Limpiar Variables de Sesión
                Session["DocumentoDigitalizadoContainer"] = null;
                Session["ParticipanteContainer"] = null;
                Session["ImagenesContainer"] = null;

                Session["ACT_DIGITALIZA"] = false;
                Session["COD_AUTOADHESIVO"] = string.Empty;
                Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;
                Session["ACTO_GENERAL_MRE"] = "0";
                #endregion

                Session["iAnularActoNotarial"] = null;
                string TarifaNro = string.Empty;
                Enumerador.enmTipoOperacion enmTipoOperacion = Enumerador.enmTipoOperacion.CONSULTA;
                int intSeleccionado = Convert.ToInt32(e.CommandArgument);

                Int64 intPersonaActualId = Convert.ToInt64(hid_iPersonaId.Value);
                Int64 intPersonaSesionId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    if (Session["iPersonaId" + HFGUID.Value] != null)
                //        intPersonaSesionId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                if (ViewState["iPersonaId"] != null)
                    intPersonaSesionId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}


                if (intPersonaActualId != intPersonaSesionId)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Trámite - Actuación", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION));
                    return;
                }

                DataTable dtActuaciones = (DataTable)Session[strVariableActDT];

                if (dtActuaciones == null)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Trámite - Actuación", Constantes.CONST_MENSAJE_PERDIDA_SESSION_ACTUACION));
                    return;
                }

                int intOficinaConsularId = 0;
                intOficinaConsularId = Convert.ToInt32(dtActuaciones.Rows[intSeleccionado]["sOficinaConsularId"]);

                if (HFGUID.Value.Length > 0)
                {
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);
                }
                else
                {
                    Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);
                }
                Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionId"]);


                Session["CORR_TARIFA"] = dtActuaciones.Rows[intSeleccionado]["vCorrelativoActuacion"].ToString();

                if (e.CommandName == "Seguimiento")
                {
                    CargarSeguimientoActuacion(intSeleccionado);
                }
                else if (e.CommandName == "EditarAct")
                {
                    #region Editar Actuación

                    Session["MIGRATORIO_OFICINACONSULTAR_ID"] = intOficinaConsularId;


                    Int32 sSession = Convert.ToInt32(dtActuaciones.Rows[intSeleccionado]["sSeccionId"].ToString());

                    string strScript = string.Empty;
                    Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                    Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                    if (intOficinaConsularId != (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
                    {
                        if (sSession == (int)Enumerador.enmSeccion.VISA)
                        {
                            if ((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != Constantes.CONST_OFICINACONSULAR_LIMA)
                            {
                                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", Constantes.CONST_MENSAJE_CONSULADO_DIFERENTE_LIMA);
                                Comun.EjecutarScript(Page, strScript);
                                return;
                            }
                        }
                        else
                        {
                            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", Constantes.CONST_MENSAJE_CONSULADO_DIFERENTE);
                            Comun.EjecutarScript(Page, strScript);
                            return;
                        }
                    }

                    if (hid_bFallecidoFlag.Value == "1")
                    {

                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }

                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 18/08/2016
                    // Objetivo: Calcular los dias habiles permitidos para la anulación
                    //           según sea Jeafatura o Consulado.
                    //------------------------------------------------------------------------
                    string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                    if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"] != null)
                    {
                        if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim() != string.Empty)
                        {
                            strFechaRegistro = dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim();
                        }
                    }
                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                    {
                        return;
                    }

                    //------------------------------------------------------------------------
                    TarifaNro = Convert.ToString(dtActuaciones.Rows[intSeleccionado]["vTarifa"]);

                    Session["ACT_DIGITALIZA"] = 0;
                    Session["iACTUACION_ID" + HFGUID.Value] = 0;
                    string strDigitaliza = dtActuaciones.Rows[intSeleccionado]["sUsuarioDigitaliza"].ToString();

                    Session["iACTUACION_ID" + HFGUID.Value] = dtActuaciones.Rows[intSeleccionado]["iActuacionId"].ToString();

                    ActuacionConsultaBL oActuacionConsultaBL = new ActuacionConsultaBL();

                    long iActuacionDetalleId = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"].ToString());
                    Boolean bExiste = false;
                    Boolean Existe = oActuacionConsultaBL.ExisteDigitalizacion(iActuacionDetalleId, sSession, ref bExiste);

                    if (bExiste)
                    {
                        Session["ACT_DIGITALIZA"] = 1;
                        if (TarifaNro.Trim().ToString() != Convert.ToString(Constantes.CONST_EXCEPCION_TARIFA_ID_1))
                        {
                            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", Constantes.CONST_MENSAJE_ACT_DIGITALIZADO);
                            Comun.EjecutarScript(Page, strScript);
                            return;
                        }
                    }


                    // más validaciones, según estado de la actuación
                    enmTipoOperacion = Enumerador.enmTipoOperacion.ACTUALIZACION;



                    //Proceso p = new Proceso();
                    Int16 intTarifaId = Convert.ToInt16(dtActuaciones.Rows[intSeleccionado]["sTarifarioId"]);

                    #region Solo para civil y militar
                    if (intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_1 || intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_2 ||
                        intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_3 || intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_4 ||
                        intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_5 || intTarifaId == Constantes.CONST_EXCEPCION_TARIFA_ID_171)
                    {
                        ActuacionConsultaBL BL = new ActuacionConsultaBL();
                        Int64 lngActuacionSeccionId = 0;
                        if (HFGUID.Value.Length > 0)
                        {
                            lngActuacionSeccionId = BL.ActuacionTramiteExiste(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]), 0);
                        }
                        else
                        {
                            lngActuacionSeccionId = BL.ActuacionTramiteExiste(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), 0);
                        }


                        if (lngActuacionSeccionId > 0)
                        {
                            Session["ACT_DET_SECCION"] = lngActuacionSeccionId;
                            enmTipoOperacion = Enumerador.enmTipoOperacion.ACTUALIZACION;
                        }
                        else
                        {
                            Session["ACT_DET_SECCION"] = 0;
                            enmTipoOperacion = Enumerador.enmTipoOperacion.REGISTRO;
                        }
                    }
                    #endregion

                    #region traer actojudicialparticipante para visualizar el acta de diligenciamiento
                    ActoJudicialNotificacionConsultaBL funNotifica = new ActoJudicialNotificacionConsultaBL();
                    Int64 iActoJudicialId = 0, iActoJudicialParticipanteId = 0;

                    if (HFGUID.Value.Length > 0)
                    {
                        funNotifica.Obtener_Actoparticipante((Int64)Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value], ref iActoJudicialId, ref iActoJudicialParticipanteId);
                    }
                    else
                    {
                        funNotifica.Obtener_Actoparticipante((Int64)Session[Constantes.CONST_SESION_ACTUACIONDET_ID], ref iActoJudicialId, ref iActoJudicialParticipanteId);
                    }



                    if (iActoJudicialId > 0 && iActoJudicialParticipanteId > 0)
                    {
                        DataTable dtNotificaciones = new DataTable();
                        DataRow[] dtPartipante;
                        int IntTotalCount = 0;
                        int IntTotalPages = 0;
                        //Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        ActoJudicialNotificacionConsultaBL CLActoJudicialNotifica = new ActoJudicialNotificacionConsultaBL();
                        dtNotificaciones = CLActoJudicialNotifica.Obtener(iActoJudicialId, iActoJudicialParticipanteId, ctrlPaginadorActuacion.PaginaActual.ToString(), ctrlPaginadorActuacion.PageSize,
                            ref IntTotalCount, ref IntTotalPages);

                        if (dtNotificaciones.Rows.Count != 0)
                        {
                            ActoJudicialParticipanteConsultaBL CLActoJudicialParticipa = new ActoJudicialParticipanteConsultaBL();
                            string strSelectQueryDT = string.Empty;
                            strSelectQueryDT = "ajpa_iActoJudicialParticipanteId = " + iActoJudicialParticipanteId;
                            dtPartipante = CLActoJudicialParticipa.Obtener(iActoJudicialId, 8542, (Int16)intOficinaConsularId).Select(strSelectQueryDT);
                            Session["LblFecha"] = dtPartipante[0]["actu_dFechaRegistro"];
                            Session["strActo"] = "Judicial";
                            Session["iActoJudicialNotificacionId"] = dtNotificaciones.Rows[0]["ajno_iActoJudicialNotificacionId"].ToString();
                        }
                    }
                    #endregion

                    //object[] arrParametros = { intTarifaId, (int)Enumerador.enmTipoOperacion.ACTUALIZACION };
                    //string strFormulario = (string)p.Invocar(ref arrParametros, "ACTUACIONDET_FORMATO", Enumerador.enmAccion.CONSULTAR);

                    ActuacionMantenimientoBL obj = new ActuacionMantenimientoBL();
                    string strFormulario = obj.ObtenerFormularioPorTarifa(intTarifaId, (int)Enumerador.enmTipoOperacion.ACTUALIZACION);
                    if (strFormulario != null)
                    {
                        if (strFormulario != string.Empty)
                        {
                            string[] arrDatos = strFormulario.Split('-');
                            if (arrDatos.Length > 0)
                            {
                                Session[strVariableAccion] = (int)enmTipoOperacion;
                                BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]));
                                Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);
                                Session.Add(Constantes.CONST_SESION_ACTUACIONDET_TABS, arrDatos[1]);
                                Session.Add("ACTUACIONDETALLE", iActuacionDetalleId);

                                //------------------------------------------------------
                                //Fecha: 19/10/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Obtener el tipo y numero de documento
                                //------------------------------------------------------
                                string codTipoDocEncriptada = "";
                                string codNroDocumentoEncriptada = "";
                                string codPersona = "";

                                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());


                                if (arrDatos[0].IndexOf("?") == -1)
                                {
                                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                    {
                                        Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona + "&Juridica=1", false);
                                    }
                                    else
                                    { // PERSONA NATURAL
                                        if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                        {
                                            codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                            codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                        }
                                        if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona, false);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                    {
                                        Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona + "&Juridica=1", false);
                                    }
                                    else
                                    { // PERSONA NATURAL
                                        if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                        {
                                            codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                            codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                        }
                                        if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona, false);
                                        }
                                    }
                                   
                                }
                                
                                //}
                            }
                        }
                    }
                    #endregion
                }
                else if (e.CommandName == "ConsultarAct")
                {
                    #region Consultar Actuación
                    if (dtActuaciones != null)
                    {
                        if (dtActuaciones.Rows.Count < 1)
                            return;
                    }
                    //------------------------------------------------------------------------
                    //Fecha: 07/04/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se comenta la validación para atender el requerimiento.
                    //Requerimiento: Observaciones_SGAC_06042021.
                    //      Item 1. Se requiere “REIMPRIMIR” o “VINCULAR” la TARIFA 58 A.
                    //              Reimprimir con la opción de la Lupa con la fecha abierta.
                    //-------------------------------------------------------------------------

                    //string strTarifaNro = Convert.ToString(dtActuaciones.Rows[intSeleccionado]["vTarifa"]);
                    //string strScript = string.Empty;
                    //if (strTarifaNro.Equals(Constantes.CONST_EXCEPCION_TARIFA_58A))
                    //{
                    //    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", "La tarifa 58A no tiene datos para Consultar."/*"Esta actuación no tiene registro de pago."*/,
                    //        false, 190, 250);
                    //    Comun.EjecutarScript(Page, strScript);
                    //    return;
                    //}
                    //------------------------------------------------------------
                    enmTipoOperacion = Enumerador.enmTipoOperacion.CONSULTA;
                    Session[strVariableAccion] = (int)enmTipoOperacion;
                    Session["ActoCivil_Accion"] = (int)enmTipoOperacion;
                    //Proceso p = new Proceso();
                    Int16 intTarifaId = Convert.ToInt16(dtActuaciones.Rows[intSeleccionado]["sTarifarioId"]);
                    //object[] arrParametros = { intTarifaId, (int)Enumerador.enmTipoOperacion.CONSULTA };

                    ActuacionMantenimientoBL obj = new ActuacionMantenimientoBL();
                    string strFormulario = obj.ObtenerFormularioPorTarifa(intTarifaId, (int)Enumerador.enmTipoOperacion.CONSULTA);

                    //strFormulario = (string)p.Invocar(ref arrParametros, "ACTUACIONDET_FORMATO", Enumerador.enmAccion.CONSULTAR);

                    TarifaNro = Convert.ToString(dtActuaciones.Rows[intSeleccionado]["vTarifa"]);

                    if (strFormulario != null)
                    {
                        if (strFormulario != string.Empty)
                        {
                            string[] arrDatos = strFormulario.Split('-');
                            if (arrDatos.Length > 0)
                            {
                                BE.RE_TARIFA_PAGO objTarifaPago = ObtenerDatosTarifaPago(Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]));
                                Session.Add(Constantes.CONST_SESION_OBJ_TARIFA_PAGO, objTarifaPago);
                                Session.Add("ACTDET_TABS", arrDatos[1]);

                                //------------------------------------------------------
                                //Fecha: 19/10/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Obtener el tipo y numero de documento
                                //------------------------------------------------------
                                string codTipoDocEncriptada = "";
                                string codNroDocumentoEncriptada = "";
                                string codPersona = "";

                                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());


                                if (arrDatos[0].IndexOf("?") == -1)
                                {
                                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                    {
                                        Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona + "&Juridica=1", false);
                                    }
                                    else
                                    { // PERSONA NATURAL
                                        if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                        {
                                            codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                            codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                        }
                                        if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "?CodPer=" + codPersona, false);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                                    {
                                        Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona + "&Juridica=1", false);
                                    }
                                    else
                                    { // PERSONA NATURAL
                                        if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                        {
                                            codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                            codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                        }
                                        if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Registro/" + arrDatos[0] + "&CodPer=" + codPersona, false);
                                        }
                                    }
                                    
                                }
                                //}
                            }
                        }
                    }
                    #endregion
                }
                else if (e.CommandName == "Reasignar")
                {
                    #region Reasignar Actuación
                    if (dtActuaciones != null)
                    {
                        if (dtActuaciones.Rows.Count < 1)
                        {
                            return;
                        }
                    }
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 18/08/2016
                    // Objetivo: Si tiene autoadhesivo no se puede reasignar
                    //------------------------------------------------------------------------
                    ActuacionMantenimientoBL _obj = new ActuacionMantenimientoBL();
                    DataTable _dt = new DataTable();
                    DataTable _dtFicha = new DataTable();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    _dt = _obj.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]), 1, 1, ref IntTotalCount, ref IntTotalPages);

                    //if (dtActuaciones.Rows[intSeleccionado]["vCodigoInsumo"].ToString().Trim() != string.Empty)
                    if (_dt.Rows.Count > 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede Reasignar el trámite porque ya cuenta con autoadhesivo."));
                        return;
                    }


                    _dtFicha = _obj.Verificar_FichaRegistral(Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]));

                    //if (dtActuaciones.Rows[intSeleccionado]["vCodigoInsumo"].ToString().Trim() != string.Empty)
                    if (Convert.ToInt16(_dtFicha.Rows[0]["Resultado"].ToString()) == 1)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede Reasignar el trámite porque tiene una ficha registral asociada."));
                        return;
                    }
                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 18/08/2016
                    // Objetivo: Calcular los dias habiles permitidos para la anulación
                    //           según sea Jeafatura o Consulado.
                    //------------------------------------------------------------------------
                    string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                    if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"] != null)
                    {
                        if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim() != string.Empty)
                        {
                            strFechaRegistro = dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim();
                        }
                    }

                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                    {
                        return;
                    }
                    //------------------------------------------------------------------------
                    if (intOficinaConsularId == (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
                    {
                        Session["ActuacionDetalleId" + HFGUID.Value] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);

                        //-----------------------------------------------------------------
                        //Fecha: 19/04/2021
                        //Autor: Miguel Márquez Beltrán 
                        //Motivo: Enviar la actuación detalle al FrmReasignacionActuacion.
                        //-----------------------------------------------------------------

                        Int64 _iActuacionDetalleId;
                        _iActuacionDetalleId = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);

                        string codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());
                        
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmReasignacionActuacion.aspx?CodPer=" + codPersona + "&Juridica=1&iActuDetalle=" + _iActuacionDetalleId.ToString(), false);
                        }
                        else
                        { // PERSONA NATURAL
                            //------------------------------------------------------
                            //Fecha: 19/10/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Obtener el tipo y numero de documento
                            //------------------------------------------------------
                            string codTipoDocEncriptada = "";
                            string codNroDocumentoEncriptada = "";

                            if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                            {
                                codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                            }
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmReasignacionActuacion.aspx?CodPer=" + codPersona + "&iActuDetalle=" + _iActuacionDetalleId.ToString() + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmReasignacionActuacion.aspx?CodPer=" + codPersona + "&iActuDetalle=" + _iActuacionDetalleId.ToString(), false);
                            }
                        }
                                               
                    }
                    else
                    {
                        string strMensaje = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", Constantes.CONST_MENSAJE_CONSULADO_DIFERENTE);
                        Comun.EjecutarScript(Page, strMensaje);
                        return;
                    }

                    #endregion
                }
                else if (e.CommandName == "Anular")
                {
                    ActuacionMantenimientoBL _obj = new ActuacionMantenimientoBL();
                    DataTable _dt = new DataTable();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    _dt = _obj.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]), 1, 1, ref IntTotalCount, ref IntTotalPages);

                    //if (dtActuaciones.Rows[intSeleccionado]["vCodigoInsumo"].ToString().Trim() != string.Empty)
                    if (_dt.Rows.Count > 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede eliminar el trámite porque se encuentra vinculado a una actuación."));
                        return;
                    }

                    string strScript = string.Empty;
                    if (hid_bFallecidoFlag.Value == "1")
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }


                    string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                    if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"] != null)
                    {
                        if (dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim() != string.Empty)
                        {
                            strFechaRegistro = dtActuaciones.Rows[intSeleccionado]["dFechaRegistro"].ToString().Trim();
                        }
                    }


                    //------------------------------------------------------------------------
                    // Autor: Miguel Angel Márquez Beltrán
                    // Fecha: 18/08/2016
                    // Objetivo: Calcular los dias habiles permitidos para la anulación
                    //           según sea Jeafatura o Consulado.
                    //------------------------------------------------------------------------
                    if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == true)
                    {
                        #region Anular Actuación
                        if (intOficinaConsularId == (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
                        {
                            if (dtActuaciones.Rows[intSeleccionado]["acno_iActoNotarialId"].ToString() != "0")
                            {
                                Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = dtActuaciones.Rows[intSeleccionado]["acno_iActoNotarialId"].ToString();
                                Session["iAnularActoNotarial"] = "1";
                            }
                            Session[strVariableDecision] = 0;
                            Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmAnularTramite.aspx','ANULAR ACTUACIÓN',260, 410, '" + btnEjecutarAnulacion.ClientID + "');");
                        }
                        else
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede eliminar, el trámite pertenece a otro consulado."));
                            return;
                        }
                        #endregion
                    }
                    //------------------------------------------------------------------------

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

        protected void gdvActuaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;


            ImageButton btnSeguimiento = e.Row.FindControl("btnSeguimiento") as ImageButton;

            ImageButton btnEditarAct = e.Row.FindControl("btnEditarAct") as ImageButton;

            ImageButton btnReasignar = e.Row.FindControl("btnReasignar") as ImageButton;

            ImageButton btnAnularAct = e.Row.FindControl("btnAnularAct") as ImageButton;

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                ImageButton[] arrImageButtons = { btnSeguimiento, btnEditarAct, btnReasignar, btnAnularAct };
                Comun.ModoLectura(ref arrImageButtons);
            }
            else
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnEditarAct);
            }

        }

        protected void btnBuscarActuacion_Click(object sender, EventArgs e)
        {
            ctrlPaginadorActuacion.InicializarPaginador();

            if (txtFecIniAct.Text == string.Empty || txtFecFinAct.Text == string.Empty)
            {
                Session[strVariableActDT] = new DataTable();
                gdvActuaciones.DataSource = new DataTable();
                gdvActuaciones.DataBind();

                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                return;
            }
            if (Comun.EsFecha(txtFecIniAct.Text.Trim()) == false)
            {
                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(txtFecFinAct.Text.Trim()) == false)
            {
                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }

            if (txtFecIniAct.Value() > txtFecFinAct.Value())
            {
                Session[strVariableActDT] = new DataTable();
                gdvActuaciones.DataSource = new DataTable();
                gdvActuaciones.DataBind();

                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                CargarGrillaActuaciones();
            }
        }

        protected void btnNuevaAct_Click(object sender, EventArgs e)
        {
            // Siempre y cuando haya autoadhesivos asignados
            Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.REGISTRO;
            // Abrir formulario - Variable Sesión: tipo de acción
            string strScript = string.Empty;
            if (hid_bFallecidoFlag.Value == "1")
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + hEdad.Value + "&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                //------------------------------------------------------
                //Fecha: 19/10/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Obtener el tipo y numero de documento
                //------------------------------------------------------
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }


                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + hEdad.Value + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + hEdad.Value + "&CodPer=" + codPersona, false);
                }
            }
            
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + hEdad.Value + "&GUID=" + HFGUID.Value);
            //}
            //else
            //{
            //    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + hEdad.Value);
            //}
        }

        // Expediente Judicial
        protected void btnBuscarExpediente_Click(object sender, EventArgs e)
        {
            if (txtFecIniExp.Text == string.Empty || txtFecFinExp.Text == string.Empty)
            {
                ctrlPaginadorExpediente.InicializarPaginador();

                Session[strVariableActDT] = new DataTable();
                gdvExpediente.DataSource = new DataTable();
                gdvExpediente.DataBind();

                ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                return;
            }

            if (Comun.EsFecha(txtFecIniExp.Text.Trim()) == false)
            {
                ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(txtFecFinExp.Text.Trim()) == false)
            {
                ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }


            Proceso p = new Proceso();
            DataTable dtResult = new DataTable();

            DateTime FchIni = txtFecIniExp.Value();
            DateTime FchFin = txtFecFinExp.Value();

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            Int64 intPersonId = 0;
            Int64 intEmpresaId = 0;

            int intTipoPersona = 0;            

            //if (HFGUID.Value.Length > 0)
            //{
            //    intTipoPersona = Convert.ToInt32(Session["iTipoId" + HFGUID.Value]);

            //    if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
            //    {
            //        intEmpresaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //        intPersonId = 0;
            //    }
            //    else
            //    {
            //        intPersonId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //        intEmpresaId = 0;
            //    }
            //}
            //else
            //{
            intTipoPersona = Convert.ToInt32(ViewState["iTipoId"]);

                if (intTipoPersona == (int)Enumerador.enmTipoPersona.JURIDICA)
                {
                    intEmpresaId = Convert.ToInt64(ViewState["iPersonaId"]);
                    intPersonId = 0;
                }
                else
                {
                    intPersonId = Convert.ToInt64(ViewState["iPersonaId"]);
                    intEmpresaId = 0;
                }
            //}


            object[] arrParametros = {  intPersonId, intEmpresaId,
                                        FchIni,
                                        FchFin,
                                        ctrlPaginadorExpediente.PaginaActual,
                                        ctrlPaginadorExpediente.PageSize,
                                        IntTotalCount,
                                        IntTotalPages
                                     };

            ActoJudicialConsultaBL objBL = new ActoJudicialConsultaBL();

            DataTable dtExpediente = objBL.ConsultarExpedientePorPersona(intPersonId, intEmpresaId,
                FchIni, FchFin, (int)ctrlPaginadorExpediente.PaginaActual, (int)ctrlPaginadorExpediente.PageSize,
                ref IntTotalCount, ref IntTotalPages);

          //  dtResult = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_ACTUACION", "CONSULTAJUDICIAL");



            if (dtExpediente != null)
            {
                ctrlPaginadorExpediente.Visible = false;
                if (dtExpediente.Rows.Count != 0)
                {
                    gdvExpediente.DataSource = dtExpediente;
                    gdvExpediente.DataBind();

                    ctrlPaginadorExpediente.TotalResgistros = Convert.ToInt32(IntTotalCount);
                    ctrlPaginadorExpediente.TotalPaginas = Convert.ToInt32(IntTotalPages);


                    updConsultaExpediente.Update();
                    ctrlPaginadorExpediente.Visible = true;
                }
                else
                {
                    ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
            }
            else
            {
                ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
        }

        protected void btnNuevoExpediente_Click(object sender, EventArgs e)
        {
            int intTipoPersona = 0;

            //if (HFGUID.Value.Length > 0)
            //{
            //    intTipoPersona = Convert.ToInt32(Session["iTipoId" + HFGUID.Value]);
            //}
            //else
            //{
            intTipoPersona = Convert.ToInt32(ViewState["iTipoId"]);
            //}

            int intOficinaLimaId = Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA);
            int intOficnaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            String strScript = String.Empty;
            if (hid_bFallecidoFlag.Value == "1")
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (intOficinaLimaId == intOficnaConsularId)
            {
                string iActoJudicialId = "0";
                int intAccionJudicial = 1; // 1 = NUEVO ; 2 = MODIFICAR ; 3 = SOLO CONSULTA

                int intTipoDemandante = 0;
                long lngPersonaId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    intTipoDemandante = Convert.ToInt32(Session["iTipoId" + HFGUID.Value]); // 2101 = PERSONA // 2102 = PERSONA JURIDICA
                //    lngPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                intTipoDemandante = Convert.ToInt32(ViewState["iTipoId"]); // 2101 = PERSONA // 2102 = PERSONA JURIDICA
                lngPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}


                long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]);
                iActoJudicialId += "-" + intAccionJudicial + "-" + intTipoDemandante + "-" + lngPersonaId + "-" + lngActuacionDetalleId;
                Session["sActoJudicialId"] = iActoJudicialId;
                Session["sDeDondeViene"] = 2;     // LE INDICAMOS AL FORMULARIO FrmActoJudicial QUE ESTA SIENDO LLAMADO DESDE  FrmTramite
                //if (HFGUID.Value.Length > 0)
                //{
                //    Response.Redirect("~/Registro/FrmActoJudicial.aspx?GUID=" + HFGUID.Value);
                //}
                //else
                //{
                string codPersona = "";
                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona + "&Juridica=1", false);
                }
                else
                { // PERSONA NATURAL
                    //------------------------------------------------------
                    //Fecha: 19/10/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Obtener el tipo y numero de documento
                    //------------------------------------------------------
                    string codTipoDocEncriptada = "";
                    string codNroDocumentoEncriptada = "";

                    if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                    {
                        codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                        codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                    }
                    if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                    {
                        Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                    }
                    else
                    {
                        Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona, false);
                    }
                }
                
                //}
            }
            else
            {
                ctrlValActuacion.MostrarValidacion("", false);
                ctrlValExpediente.MostrarValidacion("No puede agregar expedientes mientras este en un Consulado", true);
            }
        }

        // Proyecto (Acto Notarial: Protocolar y Extraprotocolar)
        protected void gdvProyecto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Limpiar Variables de Sesión
            Session["DocumentoDigitalizadoContainer"] = null;
            Session["ParticipanteContainer"] = null;
            Session["ImagenesContainer"] = null;

            Session["ACT_DIGITALIZA"] = false;
            Session["COD_AUTOADHESIVO"] = string.Empty;
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;
            #endregion

            DataTable dtActosNotariales = (DataTable)Session["dtActosNotariales"];
            if (dtActosNotariales == null)
                return;

            String AccionOperacion = String.Empty;

            int intSeleccionado = Convert.ToInt32(e.CommandArgument);
            int intOficinaConsularId = 0;
            intOficinaConsularId = Convert.ToInt32(dtActosNotariales.Rows[intSeleccionado]["acno_sOficinaConsularId"]);

            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = Convert.ToInt64(dtActosNotariales.Rows[intSeleccionado]["actu_iActuacionId"]);
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = Convert.ToInt64(dtActosNotariales.Rows[intSeleccionado]["acno_iActoNotarialId"]);
            int intActoNotarialTipoId = Convert.ToInt32(dtActosNotariales.Rows[intSeleccionado]["acno_sTipoActoNotarialId"]);

            //DateTime dFechaTramitePago = Convert.ToDateTime(dtActosNotariales.Rows[intSeleccionado]["pago_dFechaCreacion"]);

            

            if (e.CommandName == "EditarProy")
            {
                string strFechaRegistro = Comun.ObtenerFechaActualTexto(Session);
                if (dtActosNotariales.Rows[intSeleccionado]["pago_iPagoId"].ToString() == "1")
                {
                    if (dtActosNotariales.Rows[intSeleccionado]["pago_dFechaCreacion"].ToString().Trim() != string.Empty)
                    {
                        strFechaRegistro = dtActosNotariales.Rows[intSeleccionado]["pago_dFechaCreacion"].ToString().Trim();
                    }
                }
                if (Comun.CalcularDiasHabilesModificacion(Session, Page, strFechaRegistro) == false)
                {
                    return;
                }

                string strScript = string.Empty;
                
                if (hid_bFallecidoFlag.Value == "1")
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                if (intOficinaConsularId.ToString() != Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO NOTARIAL", "No se podrá editar la actuación porque la Oficina Consular seleccionada no es la Oficina Consular de Origen."));
                    return;
                }


                Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                Session["ModoEdicionProtocolar"] = true;
                AccionOperacion = "1056";

            }
            else if (e.CommandName == "ConsultarAct")
            {
                Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.CONSULTA;
                Session["ActoCivil_Accion"] = Enumerador.enmTipoOperacion.CONSULTA;
                Session["iFlujoProtocolar"] = (int)Enumerador.enmFlujoProtocolar.CONSULTA;
                Session["iFlujoExtraProtocolar"] = (int)Enumerador.enmFlujoProtocolar.CONSULTA;
                AccionOperacion = "1054";

            }
            else if (e.CommandName == "Anular")
            {

                string strScript = string.Empty;
                if (hid_bFallecidoFlag.Value == "1")
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                ActuacionMantenimientoBL _obj = new ActuacionMantenimientoBL();
                //DataTable _dt = new DataTable();
                //int IntTotalCount = 0;
                //int IntTotalPages = 0;

                DataTable dtNotariales = (DataTable)Session["dtActosNotariales"];

                //if (dtActuaciones != null)
                //{
                //_dt = _obj.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(dtNotariales.Rows[intSeleccionado]["acde_iActuacionDetalleId"]), 1, 1, ref IntTotalCount, ref IntTotalPages);

                //    //if (dtActuaciones.Rows[intSeleccionado]["vCodigoInsumo"].ToString().Trim() != string.Empty)
                //    if (_dt.Rows.Count > 0)
                //    {
                //        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede eliminar el trámite porque se encuentra vinculado a una actuación."));
                //        return;
                //    }
                //}
                

                // Se verifica que no se haya realizado el pago del acto notarial
                if (dtActosNotariales.Rows[intSeleccionado]["pago_iPagoId"].ToString() != "0")
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO NOTARIAL", "No puede anular el trámite porque ya ha sido pagado."));
                    return;
                }

                #region Anular Actuación

                //Se verifica que la anulación se realice en la oficina donde se creó el trámite
                if (intOficinaConsularId == (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID])
                {
                    Session["iAnularActoNotarial"] = "1";
                    Session[strVariableDecision] = 0;
                    Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmAnularTramite.aspx','ANULAR PROYECTO DE DOCUMENTO',260, 410, '" + btnEjecutarAnulacion.ClientID + "');");
                    return;
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTO NOTARIAL", "Solo se puede anular el trámite desde " + Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString()));
                    return;
                }
                #endregion
            }

            if (intActoNotarialTipoId == (int)Enumerador.enmNotarialTipoActo.PROTOCOLAR)
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&GUID=" + HFGUID.Value);
                //}
                //else
                //{
                string codPersona = "";
                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + codPersona + "&Juridica=1", false);
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
                        Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                    }
                    else
                    {
                        Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=" + AccionOperacion + "&CodPer=" + codPersona, false);
                    }
                }
                
                //}
            }
            else
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?GUID=" + HFGUID.Value);
                //}
                //else
                //{
                string codPersona = "";
                codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona + "&Juridica=1", false);
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
                        Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                    }
                    else
                    {
                        Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona, false);
                    }
                }
                
                //}
            }
        }

        protected void btnBuscarProyecto_Click(object sender, EventArgs e)
        {
            if (txtFecIniProy.Text == string.Empty || txtFecFinProy.Text == string.Empty)
            {
                ctrlPaginadorProyecto.InicializarPaginador();

                Session[strVariableActDT] = new DataTable();
                gdvProyecto.DataSource = new DataTable();
                gdvProyecto.DataBind();

                ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                return;
            }

            if (Comun.EsFecha(txtFecIniProy.Text.Trim()) == false)
            {
                ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }
            if (Comun.EsFecha(txtFecFinProy.Text.Trim()) == false)
            {
                ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                return;
            }

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (!DateTime.TryParse(txtFecIniProy.Text, out datFechaInicio))
            {
                datFechaInicio = Comun.FormatearFecha(txtFecIniProy.Text);
            }
            if (!DateTime.TryParse(txtFecFinProy.Text, out datFechaFin))
            {
                datFechaFin = Comun.FormatearFecha(txtFecFinProy.Text);
            }

            if (datFechaInicio > datFechaFin)
            {
                gdvProyecto.DataSource = new DataTable();
                gdvProyecto.DataBind();

                ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
            }
            else
            {
                ctrlPaginadorProyecto.InicializarPaginador();
                CargarGrillaProyecto();
            }
        }

        protected void btnConsultarProyecto_Click(object sender, EventArgs e)
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Consulta/FrmProtocolar.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Consulta/FrmProtocolar.aspx?CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("~/Consulta/FrmProtocolar.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Consulta/FrmProtocolar.aspx?CodPer=" + codPersona, false);
                }
            }
            
            //}
        }

        protected void btnNuevoProtocolar_Click(object sender, EventArgs e)
        {
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;
            String strScript = String.Empty;
            if (hid_bFallecidoFlag.Value == "1")
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=1055&GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=1055&CodPer=" + codPersona + "&Juridica=1", false);
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
                string codTipoDoc = "";
                if (Request.QueryString["CodTipoDoc"] != null)
                {
                    codTipoDoc = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString()));
                    //---------------------------------------------------------------                    
                    DataTable dtTipoDocumento = new DataTable();

                    bool bExisteDocumento = false;

                    dtTipoDocumento = comun_Part1.ObtenerDocumentoIdentidad();
                    for (int i = 0; i < dtTipoDocumento.Rows.Count; i++)
                    {
                        if (dtTipoDocumento.Rows[i]["Id"].ToString().Equals(codTipoDoc))
                        {
                            bExisteDocumento = true;
                            break;
                        }
                    }
                    if (bExisteDocumento == false)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Trámite - Actuación", "No se permite realizar Actos Notariales - Protocolares, para este tipo de documento."));
                        return;
                    }
                    //---------------------------------------------------------------
                }

                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=1055&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmActoNotarialProtocolares.aspx?class=1055&CodPer=" + codPersona, false);
                }
            }
            
            //}
        }

        protected void btnNuevoExtraPro_Click(object sender, EventArgs e)
        {
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = 0;
            Session[strVariableAccion] = (int)Enumerador.enmTipoOperacion.REGISTRO;

            String strScript = String.Empty;
            if (hid_bFallecidoFlag.Value == "1")
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Actuaciones", Constantes.CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA, false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona + "&Juridica=1", false);
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
                    Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmActuacionNotarialExtraProtocolar.aspx?CodPer=" + codPersona, false);
                }
            }
            
            //}
        }

        #endregion

        #region Métodos
        private void CargarDatosSolicitante()
        {
            try
            {
                int intTipoPersonaId = Convert.ToInt32(hid_iTipoId.Value);
                long lngPersonaId = Convert.ToInt32(hid_iPersonaId.Value);
                if (intTipoPersonaId == Convert.ToInt32(Enumerador.enmTipoPersona.NATURAL))
                {
                    lblSegundoApe.Visible = true;
                    lblSegundoApeVal.Visible = true;
                    lblApeCasadaVal.Visible = true;
                    lblApeCasada.Visible = true;
                    lblNombresAct.Visible = true;
                    lblNombresVal.Visible = true;
                    lblPrimerApe.Text = "Primer Apellido:";

                    EnPersona objPersona = new EnPersona();
                    objPersona.iPersonaId = lngPersonaId;

                    if (hiDocumentoTipoId.Value != "0")
                    {
                        int intDocumentoTipoId = Convert.ToInt32(hiDocumentoTipoId.Value);
                        objPersona.sDocumentoTipoId = intDocumentoTipoId;
                    }

                    object[] arrParametros = { objPersona };

                    objPersona = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);

                    if (objPersona != null)
                    {
                        lblTipDocVal.Text = objPersona.vDocumentoTipo;
                        lblNroDocVal.Text = objPersona.vDocumentoNumero;
                        lblPrimerApeVal.Text = objPersona.vApellidoPaterno;
                        lblSegundoApeVal.Text = objPersona.vApellidoMaterno;
                        lblNombresVal.Text = objPersona.vNombres;

                        if (objPersona.sGeneroId == Convert.ToInt16(Enumerador.enmGenero.FEMENINO))
                        {
                            DivApellidoCasada.Visible = true;
                            lblApeCasadaVal.Text = objPersona.vApellidoCasada;
                        }
                        else
                        {
                            DivApellidoCasada.Visible = false;
                        }

                        string strDireccion = string.Empty;
                        strDireccion += objPersona.vDireccion.Trim();
                        if (strDireccion != string.Empty)
                            strDireccion += ", ";
                        if (objPersona.vDistCiu != string.Empty)
                            strDireccion += objPersona.vDistCiu;
                        if (objPersona.vProvPais != string.Empty)
                            strDireccion += ", " + objPersona.vProvPais;
                        if (objPersona.vDptoCont != string.Empty)
                            strDireccion += " - " + objPersona.vDptoCont;
                        lblDireccionVal.Text = strDireccion;
                    }
                }
                else
                {
                    lblSegundoApe.Visible = false;
                    lblSegundoApeVal.Visible = false;
                    lblApeCasadaVal.Visible = false;
                    lblApeCasada.Visible = false;
                    lblNombresAct.Visible = false;
                    lblNombresVal.Visible = false;
                    lblPrimerApe.Text = "Razón Social:";

                    EmpresaConsultaBL objEmpresaBL = new EmpresaConsultaBL();
                    DataSet ds = objEmpresaBL.ConsultarId(lngPersonaId);

                    if (ds != null)
                    {
                        if (ds.Tables[0] != null)
                        {
                            if (ds.Tables[0].Rows.Count == 0)
                                return;

                            lblTipDocVal.Text = Convert.ToString(ds.Tables[0].Rows[0]["empr_vTipoDocumento"]);
                            lblNroDocVal.Text = Convert.ToString(ds.Tables[0].Rows[0]["vNumeroDocumento"]);
                            lblPrimerApeVal.Text = Convert.ToString(ds.Tables[0].Rows[0]["vRazonSocial"]);

                            if (ds.Tables[2] != null)
                            {
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    string strDireccion = Convert.ToString(ds.Tables[2].Rows[0]["vResidenciaDireccion"]);
                                    if (Convert.ToString(ds.Tables[2].Rows[0]["vDistrito"]) != string.Empty)
                                        strDireccion += ", " + Convert.ToString(ds.Tables[2].Rows[0]["vDistrito"]);
                                    if (Convert.ToString(ds.Tables[2].Rows[0]["vProvincia"]) != string.Empty)
                                        strDireccion += ", " + Convert.ToString(ds.Tables[2].Rows[0]["vProvincia"]);
                                    if (Convert.ToString(ds.Tables[2].Rows[0]["vDepartamento"]) != string.Empty)
                                        strDireccion += " - " + Convert.ToString(ds.Tables[2].Rows[0]["vDepartamento"]);
                                    lblDireccionVal.Text = strDireccion;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CargarDatosIniciales()
        {
            try
            {
                // PREGUNTAMOS SI ES LIMA PARA ACTIVAR EL BOTON "NUEVO EXPEDIENTE"
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    btnNuevoExpediente.Enabled = true;
                }
                //---------------------------------------------------------------------------------
                // Fecha: 02/12/2019
                // Autor: Miguel Márquez Beltrán
                // Motivo: Se comentan el método: CargarSaldoAutoadhesivo
                //          por estar implicito en el método: CargarStockMinimoInsumo
                //---------------------------------------------------------------------------------
                //CargarSaldoAutoadhesivo();
                CargarStockMinimoInsumo();

                //----------------------------------------------------------
                //Fecha: 19/11/2019
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se unifico la consulta en un solo Datatable
                //----------------------------------------------------------

                string strFechatexto = Comun.ObtenerFechaActualTexto(Session);
                //----------------------------------------------------------

                // ACTUACION
                txtFecIniAct.Text = DateTime.Now.ToString("MMM-01-yyyy").Replace(".","");
                txtFecFinAct.Text = strFechatexto;

                // EXPEDIENTE
                txtFecIniExp.Text = DateTime.Now.ToString("MMM-01-yyyy").Replace(".","");
                txtFecFinExp.Text = strFechatexto;

                // PROYECTO
                txtFecIniProy.Text = DateTime.Now.ToString("MMM-01-yyyy").Replace(".", "");
                txtFecFinProy.Text = strFechatexto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void CargarListadosDesplegables()
        {
            try
            {
                Util.CargarParametroDropDownList(ddlSeccion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.SECCION), true, "- TODOS -");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarPestanias()
        {
            try
            {
                string strScript = string.Empty;
                object objTabActivo = Session["SETFORMTAB"];
                if (objTabActivo != null)
                {
                    if (objTabActivo.ToString() != string.Empty)
                    {
                        int intTab = Convert.ToInt32(objTabActivo);
                        if (intTab == (int)Enumerador.enmTramiteTipo.ACTO_NOTARIAL)
                        {
                            strScript = Util.ActivarTab((int)Enumerador.enmTramiteTipo.ACTO_NOTARIAL, "Actos Notariales");
                            Comun.EjecutarScript(Page, strScript);

                            CargarGrillaProyecto();
                        }
                        else if (intTab == (int)Enumerador.enmTramiteTipo.EXPEDIENTE_JUDICIAL)
                        {
                            strScript = Util.ActivarTab((int)Enumerador.enmTramiteTipo.EXPEDIENTE_JUDICIAL, "Expedientes Judiciales");
                            Comun.EjecutarScript(Page, strScript);
                            btnBuscarExpediente_Click(null, null);
                            //CargarGrillaExpediente();
                        }
                    }
                    else
                    {
                        strScript = Util.ActivarTab((int)Enumerador.enmTramiteTipo.ACTUACION_DETALLE, "Actuaciones Consulares");
                        Comun.EjecutarScript(Page, strScript);

                        CargarGrillaActuaciones();
                    }
                }
                else
                {
                    strScript = Util.ActivarTab((int)Enumerador.enmTramiteTipo.ACTUACION_DETALLE, "Actuaciones Consulares");
                    Comun.EjecutarScript(Page, strScript);

                    CargarGrillaActuaciones();
                }
                Session.Remove("SETFORMTAB");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void CargarSaldoAutoadhesivo()
        //{
        //    ActuacionConsultaBL objActuacionBL = new ActuacionConsultaBL();

        //    int intStock = 0;

        //    int intStockSaldoInsumos = 0;

        //    intStock = objActuacionBL.ObtenerSaldoAutoadhesivos(
        //        Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
        //        Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
        //        Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));

        //    //------------------------------------------------------------------------
        //    // Autor: Sandra del Carmen Acosta Celis
        //    // Fecha: 20/01/2017
        //    // Objetivo: Obtener el stock mínimo de insumo según Consulado.
        //    //------------------------------------------------------------------------
        //    intStockSaldoInsumos = objActuacionBL.ObtenerSaldoInsumos(
        //        Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
        //        Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));


        //    lblSaldoInsumo.Text = Convert.ToString(intStock);

        //    if (intStock <= 0)
        //    {
        //        msjeWarningStock.Visible = true;
        //        lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SALDO_INSUFICIENTE;
        //        btnNuevaAct.Enabled = false;
        //        updConsultaActuacion.Update();

        //        btnNuevoExpediente.Enabled = true;
        //        updConsultaExpediente.Update();

        //        btnNuevoExtraPro.Enabled = true;
        //        btnNuevoProtocolar.Enabled = true;
        //        updConsultaProyecto.Update();
        //    }
        //    else
        //    {
        //        if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
        //        {
        //            msjeWarningStock.Visible = true;
        //            lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SIN_TIPOCAMBIO;

        //            btnNuevaAct.Enabled = false;
        //            updConsultaActuacion.Update();

        //            btnNuevoExpediente.Enabled = false;
        //            updConsultaExpediente.Update();

        //            btnNuevoExtraPro.Enabled = false;
        //            btnNuevoProtocolar.Enabled = false;
        //            updConsultaProyecto.Update();
        //        }
        //        else
        //        {
        //            btnNuevaAct.Enabled = true;
        //            updConsultaActuacion.Update();

        //            btnNuevoExpediente.Enabled = true;
        //            updConsultaExpediente.Update();

        //            btnNuevoExtraPro.Enabled = true;
        //            btnNuevoProtocolar.Enabled = true;
        //            updConsultaProyecto.Update();

        //            msjeWarningStock.Visible = false;
        //            lblMsjeWarnigStock.Text = "";
        //        }
        //    }

        //    updSaldoAutoadhesivo.Update();
        //}

        //------------------------------------------------------------------------
        // Autor: Sandra del Carmen Acosta Celis
        // Fecha: 20/01/2017
        // Objetivo: Calcular el stock mínimo de insumo según Consulado.
        //------------------------------------------------------------------------

        private void CargarStockMinimoInsumo()
        {
            try
            {
                ActuacionConsultaBL objActuacionBL = new ActuacionConsultaBL();

                int intStock = 0;
                int intStockSaldoInsumos = 0;

                intStock = objActuacionBL.ObtenerSaldoAutoadhesivos(
                    Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));

                intStockSaldoInsumos = objActuacionBL.ObtenerSaldoInsumos(
                    Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO));

                Session["Valor"] = intStock;

                int intStockMinimo = 0;
                intStockMinimo = objActuacionBL.ObtenerStockMinimoInsumos(
                    Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    Convert.ToInt16(Enumerador.enmBovedaTipo.MISION),
                    Convert.ToInt16(Enumerador.enmInsumoTipo.AUTOADHESIVO)

                    );

                lblSaldoInsumo.Text = Convert.ToString(intStock);

                if (intStock <= 0)
                {
                    msjeWarningStock.Visible = true;
                    lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SALDO_INSUFICIENTE;

                    btnNuevaAct.Enabled = false;
                    updConsultaActuacion.Update();

                    btnNuevoExpediente.Enabled = false;
                    updConsultaExpediente.Update();

                    //------------------------------------------------
                    //Fecha: 09/12/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Quitar la validación en ambos botones.
                    //------------------------------------------------
                    //btnNuevoExtraPro.Enabled = false;
                    //btnNuevoProtocolar.Enabled = false;
                    updConsultaProyecto.Update();
                }
                else
                {
                    if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
                    {
                        msjeWarningStock.Visible = true;
                        lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_SIN_TIPOCAMBIO;

                        btnNuevaAct.Enabled = false;
                        updConsultaActuacion.Update();

                        btnNuevoExpediente.Enabled = false;
                        updConsultaExpediente.Update();

                        btnNuevoExtraPro.Enabled = false;
                        btnNuevoProtocolar.Enabled = false;
                        updConsultaProyecto.Update();
                    }
                    else
                    {
                        btnNuevaAct.Enabled = true;
                        updConsultaActuacion.Update();

                        btnNuevoExpediente.Enabled = true;
                        updConsultaExpediente.Update();

                        btnNuevoExtraPro.Enabled = true;
                        btnNuevoProtocolar.Enabled = true;
                        updConsultaProyecto.Update();

                        msjeWarningStock.Visible = false;
                        lblMsjeWarnigStock.Text = "";
                    }
                }

                if (intStockSaldoInsumos <= intStockMinimo)
                {
                    Session["Valor"] = intStockSaldoInsumos;
                    msjeWarningStock.Visible = true;
                    string Stock = HttpContext.Current.Session["Valor"].ToString();

                    Session["ValorStockMinimo"] = intStockMinimo;
                    msjeWarningStock.Visible = true;
                    string StockMinimo = HttpContext.Current.Session["ValorStockMinimo"].ToString();

                    lblMsjeWarnigStock.Text = Constantes.CONST_MENSAJE_STOCK_MINIMO.Replace("valorstock", Stock).Replace("valorStockMinimo", StockMinimo);
                    btnNuevaAct.Enabled = true;
                    updConsultaActuacion.Update();

                    btnNuevoExpediente.Enabled = true;
                    updConsultaExpediente.Update();

                    btnNuevoExtraPro.Enabled = true;
                    btnNuevoProtocolar.Enabled = true;
                    updConsultaProyecto.Update();
                }
               

                updSaldoAutoadhesivo.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable ObtenerDatosPersona(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            //Proceso MiProc = new Proceso();
            //Object[] miArray = new Object[3] { LonPersonaId, intDocumentoId, strDocumentoNumero };
            DataTable dtPersona = new DataTable();
            PersonaConsultaBL Obj = new PersonaConsultaBL();
            dtPersona = Obj.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
            //dtPersona = (DataTable)MiProc.Invocar(ref miArray,
            //                               "SGAC.BE.RE_PERSONA", "OBTENERREGISTRO");
            return dtPersona;
        }

        // Actuaciones
        private void CargarGrillaActuaciones()
        {
            long lngPersonaId = Convert.ToInt64(hid_iPersonaId.Value);

            Proceso p = new Proceso();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intSeccionId = 0;
            if (ddlSeccion.SelectedIndex > 0)
                intSeccionId = Convert.ToInt32(ddlSeccion.SelectedValue);

            long lngEmpresaId = 0;
            if (Convert.ToInt32(hid_iTipoId.Value) == (int)Enumerador.enmTipoPersona.JURIDICA)
            {
                lngEmpresaId = lngPersonaId;
                lngPersonaId = 0;
            }

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DataTable dtActuaciones = objBL.ActuacionesObtener(lngPersonaId, lngEmpresaId, intSeccionId,
                txtFecIniAct.Value(), txtFecFinAct.Value(), ctrlPaginadorActuacion.PaginaActual.ToString(), ctrlPaginadorActuacion.PageSize,
                ref IntTotalCount, ref IntTotalPages);

            if (dtActuaciones.Rows.Count > 0)
            {
                ctrlValActuacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);

                Session[strVariableActDT] = dtActuaciones;

                gdvActuaciones.DataSource = dtActuaciones;
                gdvActuaciones.DataBind();

                ctrlPaginadorActuacion.TotalResgistros = Convert.ToInt32(IntTotalCount);
                ctrlPaginadorActuacion.TotalPaginas = Convert.ToInt32(IntTotalPages);

                ctrlPaginadorActuacion.Visible = false;
                if (ctrlPaginadorActuacion.TotalResgistros > Constantes.CONST_PAGE_SIZE_ACTUACIONES)
                {
                    ctrlPaginadorActuacion.Visible = true;
                }
            }
            else
            {
                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

                Session[strVariableActDT] = null;

                gdvActuaciones.DataSource = null;
                gdvActuaciones.DataBind();

                lblTtlSeguimiento.Visible = false;
                gdvSeguimiento.Visible = false;
            }
        }
        private void CargarSeguimientoActuacion(int intSeleccionado)
        {
            DataTable dtActuacion = (DataTable)Session[strVariableActDT];
            if (dtActuacion != null)
            {
                lblTtlSeguimiento.Visible = true;
                lblTtlSeguimiento.Text = "SEGUIMIENTO (RGE: " + dtActuacion.Rows[intSeleccionado]["vCorrelativoActuacion"].ToString() + ")";

                long lngActuacionDetalleId = Convert.ToInt64(dtActuacion.Rows[intSeleccionado]["iActuacionDetalleId"]);

                ActuacionConsultaBL objActuacionBL = new ActuacionConsultaBL();
                DataTable dtSeguimiento = objActuacionBL.ObtenerSeguimientoActuacion(lngActuacionDetalleId);

                gdvSeguimiento.DataSource = dtSeguimiento;
                gdvSeguimiento.DataBind();
                gdvSeguimiento.Visible = true;
                updGrillaSeguimiento.Update();
            }
            else
            {
                lblTtlSeguimiento.Visible = false;
                gdvSeguimiento.DataSource = null;
                gdvSeguimiento.DataBind();
            }
        }

        // Expedientes
        //private void CargarGrillaExpediente()
        //{
        //    long lngPersonaId = 0;

        //    if (HFGUID.Value.Length > 0)
        //    {
        //        lngPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
        //    }
        //    else
        //    {
        //        lngPersonaId = Convert.ToInt64(Session["iPersonaId"]);
        //    }

        //    //Proceso p = new Proceso();
        //    int IntTotalCount = 0;
        //    int IntTotalPages = 0;

        //    //object[] arrParametros = {  lngPersonaId,
        //    //                            txtFecIniExp.Value(),
        //    //                            txtFecFinExp.Value(),
        //    //                            ctrlPaginadorActuacion.PaginaActual.ToString(),
        //    //                            ctrlPaginadorActuacion.PageSize,
        //    //                            IntTotalCount,
        //    //                            IntTotalPages
        //    //                         };
        //    //DataTable dtExpedientes = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_ACTOJUDICIAL", "CONSULTAR");

        //    ActoJudicialConsultaBL objActoJudicialConsultaBL = new ActoJudicialConsultaBL();
        //    DataTable dtExpedientes = new DataTable();

        //    dtExpedientes = objActoJudicialConsultaBL.ConsultarExpedientePorPersona(lngPersonaId, 0, txtFecIniExp.Value(), txtFecFinExp.Value(), 
        //                                                                            ctrlPaginadorActuacion.PaginaActual, ctrlPaginadorActuacion.PageSize, ref IntTotalCount, ref IntTotalPages);


        //    if (dtExpedientes.Rows.Count != 0)
        //    {
        //        ctrlValActuacion.MostrarValidacion("", false);

        //        gdvExpediente.DataSource = dtExpedientes;
        //        gdvExpediente.DataBind();

        //        //ctrlPaginadorExpediente.TotalResgistros = Convert.ToInt32(arrParametros[6]);
        //        ctrlPaginadorExpediente.TotalResgistros = IntTotalCount;
        //        //ctrlPaginadorExpediente.TotalPaginas = Convert.ToInt32(arrParametros[7]);
        //        ctrlPaginadorExpediente.TotalPaginas = IntTotalPages;

        //        ctrlPaginadorActuacion.Visible = false;
        //        if (ctrlPaginadorActuacion.TotalResgistros > Constantes.CONST_PAGE_SIZE_ACTUACIONES)
        //        {
        //            ctrlPaginadorActuacion.Visible = true;
        //        }
        //        updConsultaExpediente.Update();
        //    }
        //    else
        //    {
        //        ctrlValExpediente.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

        //        gdvExpediente.DataSource = null;
        //        gdvExpediente.DataBind();
        //        updConsultaExpediente.Update();
        //    }
        //}

        // Proyecto
        private void CargarGrillaProyecto()
        {
            DataTable dtActosNotariales = new DataTable();

            long lngPersonaId = 0;

            //if (HFGUID.Value.Length > 0)
            //{
                //lngPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //}
            //else
            //{
            lngPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
            //}
            
            DateTime FchIni = txtFecIniProy.Value();
            DateTime FchFin = txtFecFinProy.Value();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            TramiteConsultaBL objBL = new TramiteConsultaBL();
            dtActosNotariales = objBL.ActuacionConsultaNotarial(lngPersonaId,
                                                                FchIni,
                                                                FchFin,
                                                                ctrlPaginadorProyecto.PaginaActual,
                                                                Constantes.CONST_PAGE_SIZE_ANOTACIONES,
                                                                ref IntTotalCount,
                                                                ref IntTotalPages);

            if (dtActosNotariales != null)
            {
                if (dtActosNotariales.Rows.Count == 0)
                {
                    gdvProyecto.DataSource = null;
                    gdvProyecto.DataBind();

                    Session["dtActosNotariales"] = null;

                    ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    gdvProyecto.DataSource = dtActosNotariales;
                    gdvProyecto.DataBind();

                    Session["dtActosNotariales"] = dtActosNotariales;

                    ctrlValProyecto.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                ctrlPaginadorProyecto.TotalResgistros = Convert.ToInt32(IntTotalCount);
                ctrlPaginadorProyecto.TotalPaginas = Convert.ToInt32(IntTotalPages);

                ctrlPaginadorProyecto.Visible = false;
                if (ctrlPaginadorProyecto.TotalPaginas > 1)
                {
                    ctrlPaginadorProyecto.Visible = true;
                }
            }
            else
            {
                ctrlValProyecto.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
            updConsultaProyecto.Update();
        }

        private BE.RE_TARIFA_PAGO ObtenerDatosTarifaPago(Int64 lngActuacionDetalleId)
        {
            ActuacionPagoConsultaBL objBL = new ActuacionPagoConsultaBL();
            DataTable dtPago = objBL.ActuacionPagoObtenerDetalle(lngActuacionDetalleId);

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
                objTarifaPago.datFechaRegistroActuacion = Comun.FormatearFecha(dr["acde_dFechaRegistro"].ToString());
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
                objTarifaPago.dblNormaTarifario = Convert.ToDouble(dr["pago_iNormaTarifarioId"]);
                objTarifaPago.vSustentoTipoPago = dr["pago_vSustentoTipoPago"].ToString();
                //--------------------------------------------
            }
            return objTarifaPago;
        }

        #endregion

        protected void btnEjecutarAnulacion_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------------
            //Fecha: 29/08/2018
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Validar la anulación del autoadhesivo
            //---------------------------------------------------------------
            ActuacionMantenimientoBL _obj = new ActuacionMantenimientoBL();
            DataTable _dt = new DataTable();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            if (HFGUID.Value.Length > 0)
            {
                _dt = _obj.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]), 1, 1, ref IntTotalCount, ref IntTotalPages);
            }
            else
            {
                _dt = _obj.Obtener_ActuacionInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), 1, 1, ref IntTotalCount, ref IntTotalPages);
            }
            
            if (_dt.Rows.Count > 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede eliminar el trámite porque se encuentra vinculado a una actuación."));
                return;
            }
            //---------------------------------------------------------------

            Int16 sResult = Convert.ToInt16(Session[strVariableDecision]);

            if (Session[strFuncionarioAnulaId] == null || Session[strAuditoriaDataAnulacion] == null)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "TRÁMITE", "No puede eliminar el trámite porque la sesión del funcionario no existe."));
                return;
            }


            int intFuncionarioAnulaId = Convert.ToInt32(Session[strFuncionarioAnulaId]);
            int IntRpta = 0;
            string StrScript = "";

            string vDatosAutorizador = Convert.ToString(Session[strAuditoriaDataAnulacion]);

            string[] vCadena = vDatosAutorizador.Split('|');

            Session[strVariableDecision] = "";
            Session[strAuditoriaDataAnulacion] = "";


            if (intFuncionarioAnulaId > 0 && vCadena[1].Trim().Length > 0)
            {
                if (sResult != 1)
                {
                    return;
                }

                if (Session["iAnularActoNotarial"] != null)
                {
                    if (Session["iAnularActoNotarial"].ToString() == "1")
                    {
                        AnularActoNotarial(vCadena);
                    }
                    //return;
                }

                BE.RE_ACTUACION ObjActuacBE = new BE.RE_ACTUACION();
                BE.RE_ACTUACIONDETALLE ObjDetActuacBE = new BE.RE_ACTUACIONDETALLE();

                Proceso MiProc = new Proceso();


                StrScript = string.Empty;


                ObjActuacBE.actu_iActuacionId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]);
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                if (HFGUID.Value.Length > 0)
                {
                    ObjDetActuacBE.acde_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value]);
                }
                else
                {
                    ObjDetActuacBE.acde_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                }

                ObjActuacBE.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                //---------------------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 09/09/2016
                // Cambio: Se eliminan los parametros: vCadena[0], vCadena[1]  del arreglo: miArray
                //         a cambio se asignan dos variables: intFuncionarioAnulaId y vCadena[1].
                //---------------------------------------------------------------------------------------

                ObjDetActuacBE.acde_IFuncionarioAnulaId = intFuncionarioAnulaId;
                ObjDetActuacBE.acde_vMotivoAnulacion = vCadena[1].ToUpper();
                //---------------------------------------------------------------------------------------

                Object[] miArray = new Object[2] { ObjActuacBE, ObjDetActuacBE };


                IntRpta = (int)MiProc.Invocar(ref miArray,
                                              "SGAC.BE.RE_ACTUACION",
                                              Enumerador.enmAccion.ELIMINAR);
            }
            else
            {
                IntRpta = 0;
            }
            if (IntRpta > 0)
            {
                CargarGrillaActuaciones();
            }
            //else
            //{
            //    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", "Error. No se pudo anular el registro", false, 190, 250);
            //    Comun.EjecutarScript(Page, StrScript);
            //}
        }

        private void AnularActoNotarial(string[] DatosAutorizador)
        {
            long lActonotarialId = 0;

            //Se verifica que se haya almacenado el id de acto notarial
            if (Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] != null)
                lActonotarialId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]);
            else
                return;

            // Se obtiene el acto notarial
            SGAC.BE.MRE.RE_ACTONOTARIAL lACTONOTARIAL = new SGAC.BE.MRE.RE_ACTONOTARIAL();
            ActoNotarialConsultaBL lActoNotarialConsultaBL = new ActoNotarialConsultaBL();
            lACTONOTARIAL.acno_iActoNotarialId = Convert.ToInt64(lActonotarialId);
            lACTONOTARIAL = lActoNotarialConsultaBL.obtener(lACTONOTARIAL);

            // Se inicializa la actuacion cuando no es nula para evitar
            // conflictos en la capa BL
            if (lACTONOTARIAL.ACTUACION == null)
                lACTONOTARIAL.ACTUACION = new BE.MRE.RE_ACTUACION();

            // Se setean los valor de anulación
            lACTONOTARIAL.acno_sEstadoId = Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.ANULADA);
            lACTONOTARIAL.acno_vMotivoAnulacion = DatosAutorizador[1];
            lACTONOTARIAL.acno_vIPModificacion = "::1";
            lACTONOTARIAL.acno_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
            lACTONOTARIAL.acno_IFuncionarioAnulaId = Convert.ToInt32(Session[strFuncionarioAnulaId]);

            ActoNotarialMantenimiento mnt = new ActoNotarialMantenimiento();
            mnt.Insertar_ActoNotarial(lACTONOTARIAL);

            Session.Remove(strFuncionarioAnulaId);

            btnBuscarProyecto_Click(null, null);
        }

        protected void gdvExpediente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Int64 actu_iPersonaRecurrenteId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["actu_iPersonaRecurrenteId"].ToString());
            Int64 acju_iActoJudicialId = Convert.ToInt64(gdvExpediente.DataKeys[index].Values["acju_iActoJudicialId"].ToString());
            Int16 intEstadoExpediente = Convert.ToInt16(gdvExpediente.DataKeys[index].Values["acju_sEstadoId"].ToString());

            if (e.CommandName == "Imprimir")
            {

            }
            else if (e.CommandName == "Editar")
            {
                Int16 intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                Int16 intOficinaLima = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);

                if (intOficinaConsular == intOficinaLima)
                {
                    if (intEstadoExpediente == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.ENVIADO))
                    {
                        ctrlValExpediente.MostrarValidacion("No se puede modificar un Expediente enviado", true);
                        updConsultaExpediente.Update();
                        return;
                    }

                    if (intEstadoExpediente == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                    {
                        ctrlValExpediente.MostrarValidacion("No se puede modificar un Expediente cerrado", true);
                        updConsultaExpediente.Update();
                        return;
                    }
                }
                else
                {
                    if (intEstadoExpediente == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.REGISTRADO))
                    {
                        ctrlValExpediente.MostrarValidacion("No puede modificar un Expediente que aun no ha sido Enviado", true);
                        updConsultaExpediente.Update();
                        return;
                    }

                    if (intEstadoExpediente == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.CERRADO))
                    {
                        ctrlValExpediente.MostrarValidacion("No puede modificar un Expediente Cerrado", true);
                        updConsultaExpediente.Update();
                        return;
                    }

                    if (intEstadoExpediente == Convert.ToInt16(Enumerador.enmJudicialExpedienteEstado.OBSERVADO))
                    {
                        ctrlValExpediente.MostrarValidacion("No puede modificar un Expediente Observado", true);
                        updConsultaExpediente.Update();
                        return;
                    }
                }

                verJudicial(acju_iActoJudicialId, 2);

            }
            else if (e.CommandName == "Consultar")
            {
                verJudicial(acju_iActoJudicialId, 3);
            }
        }

        void verJudicial(Int64 intActoJudicialId, int intTipoAccion)
        {
            string iActoJudicialId = intActoJudicialId.ToString();
            int intAccionJudicial = intTipoAccion; // 1 = NUEVO ; 2 = MODIFICAR ; 3 = SOLO CONSULTA

            int intTipoDemandante = 0;                        
            long lngPersonaId = 0;

            //if (HFGUID.Value.Length > 0)
            //{
            //    lngPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //    intTipoDemandante = Convert.ToInt32(Session["iTipoId" + HFGUID.Value]); // 2101 = PERSONA // 2102 = PERSONA JURIDICA
            //}
            //else
            //{
            lngPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
            intTipoDemandante = Convert.ToInt32(ViewState["iTipoId"]); // 2101 = PERSONA // 2102 = PERSONA JURIDICA
            //}
            
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]);
            iActoJudicialId += "-" + intAccionJudicial + "-" + intTipoDemandante + "-" + lngPersonaId + "-" + lngActuacionDetalleId;
            Session["sActoJudicialId"] = iActoJudicialId;
            Session["sDeDondeViene"] = 2;     // LE INDICAMOS AL FORMULARIO FrmActoJudicial QUE ESTA SIENDO LLAMADO DESDE  FrmTramite
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmActoJudicial.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = "";
            codPersona = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString());

            if(Request.QueryString["Juridica"]!= null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else{ // PERSONA NATURAL
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmActoJudicial.aspx?CodPer=" + codPersona, false);
                }
            }
            
            
            //}
        }

        protected void btn_modal_Click(object sender, EventArgs e)
        {

        }

        protected void gdvActuaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
        
        }

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
                else {
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
                else { // Persona natural
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
