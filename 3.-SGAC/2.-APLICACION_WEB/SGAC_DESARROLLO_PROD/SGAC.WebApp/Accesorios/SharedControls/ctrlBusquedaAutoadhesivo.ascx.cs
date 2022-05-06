using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;
using System.Linq;
using SGAC.WebApp.Accesorios;
using System.Configuration;
using SGAC.BE;
using SGAC.Registro.Persona.BL;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlBusquedaAutoadhesivo : UserControl
    {
        #region Campos
        private int PAGE_SIZE = Constantes.CONST_CANT_REGISTRO;
        private string strMensajeValidacionVacio = Constantes.CONST_MENSAJE_INGRESAR_AUTOADHESIVO;
        #endregion

        #region Campos
        private string strVariableDecision = "TramiteAnulacion_Decision";
        private string strAuditoriaDataAnulacion = "Auditoria_Data_Anulacion";

        private string strVariableActDT = "TRAMITE_DT";
        private string strVariableAccion = "Actuacion_Accion";

        private string strFuncionarioAnulaId = "FuncionarioAnulacionId";
        #endregion

        #region Propiedades
        public Enumerador.enmBusquedaDirecciona Direcciona { get; set; }
        public Enumerador.enmTipoPersona TipoPersona { get; set; }

        public string GUID
        {
            set { HFGUID.Value = value; }
            get { return HFGUID.Value; }
        }

        #endregion   

        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {                    

                    CargarDatosIniciales();
                    hdn_tipo_direccion.Value = ((int)Direcciona).ToString();
                    hdn_tipo_persona.Value = ((int)TipoPersona).ToString();
                }

                if (hEnter.Value != null)
                {
                    if (this.hEnter.Value == "1")
                        btn_Buscar_Click(null, EventArgs.Empty);
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 #region Metodos
        private void CargarDatosIniciales()
        {
            ctrlValActuacion.MostrarValidacion("", false);
        }
        private void CargarGrillaSolicitante(string StrNroDoc, string StrApePat, string StrApeMat, string strNombre)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            ctrlValActuacion.MostrarValidacion("", false);

            DataTable DtRecurrente = new DataTable();

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DtRecurrente = objBL.RecurrenteConsultar(
                                    (int)Enumerador.enmPersonaConsulta.ACTUACION,
                                    StrNroDoc, StrApePat, StrApeMat, strNombre,
                                    ctrlPaginador.PaginaActual,
                                    PAGE_SIZE,
                                    ref IntTotalCount, ref IntTotalPages);

            ctrlPaginador.Visible = false;
            if (DtRecurrente.Rows.Count > 0)
            {
                gdvActuaciones.DataSource = DtRecurrente;
                gdvActuaciones.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;
                ctrlValActuacion.MostrarValidacion(
                    Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + " " + IntTotalCount, 
                    true, Enumerador.enmTipoMensaje.INFORMATION);

                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                TextBox txtPage = (TextBox)ctrlPaginador.FindControl("txtPagina");
            }
            else
            {
                ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                gdvActuaciones.DataSource = null;
                gdvActuaciones.DataBind();
            }
        }

        private void CargarGrillaActuaciones(string StrNroDoc)
        {
            StrNroDoc = txtNroAutoadhesivo.Text;
            Proceso p = new Proceso();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            
            if(!string.IsNullOrEmpty(txtNroAutoadhesivo.Text))
            {

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DataTable dtActuaciones = new DataTable();
            dtActuaciones = objBL.ActuacionesObtenerPorAutoadhesivo(StrNroDoc, ctrlPaginador.PaginaActual, ctrlPaginador.PageSize,
            ref IntTotalCount, ref IntTotalPages);

            //----------------------------------------------------
            //Fecha: 15/02/2017
            //Autor: Sandra del Carmen Acosta Celis
            //Objetivo: Asignación de variable de session.
            //----------------------------------------------------

            if (dtActuaciones != null)
            {
                int dtActuacionesValor = 5;
                Session["dtActuacionesValor"] = dtActuacionesValor;
            }
            else
            {
                Session["dtActuacionesValor"] = null;
            }
            
                try 
                {
                    if (dtActuaciones.Rows.Count > 0)
                    {
                        ctrlValActuacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);

                        Session[strVariableActDT] = dtActuaciones;

                        gdvActuaciones.DataSource = dtActuaciones;
                        gdvActuaciones.DataBind();

                        ctrlPaginador.TotalResgistros = Convert.ToInt32(IntTotalCount);
                        ctrlPaginador.TotalPaginas = Convert.ToInt32(IntTotalPages);

                        ctrlPaginador.Visible = false;
                        if (ctrlPaginador.TotalResgistros > Constantes.CONST_PAGE_SIZE_ACTUACIONES)
                        {
                            ctrlPaginador.Visible = true;
                        }
                    }
                    else
                    {
                        ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

                        Session[strVariableActDT] = null;

                        gdvActuaciones.DataSource = null;
                        gdvActuaciones.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        private void InicializarBusqueda()
        {
            gdvActuaciones.DataSource = null;
            gdvActuaciones.DataBind();

            ctrlPaginador.InicializarPaginador();
        }
        private void ValidarBusqueda()
        {
            if ((txtNroAutoadhesivo.Text.Length == 0))
            {                
                ctrlValActuacion.MostrarValidacion("", false);
                return;
            }
        }

        private DataTable GetDataPersona(long LonPersonaId)
        {
            try
            {
                SGAC.Registro.Persona.BL.PersonaConsultaBL objPersonaBL = new SGAC.Registro.Persona.BL.PersonaConsultaBL();
                DataTable dt = objPersonaBL.PersonaGetById(LonPersonaId); 
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gdvActuaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Limpiar Variables de Sesión
            Session["DocumentoDigitalizadoContainer"] = null;
            Session["ParticipanteContainer"] = null;
            Session["ImagenesContainer"] = null;

            Session["ACT_DIGITALIZA"] = false;
            Session["COD_AUTOADHESIVO"] = string.Empty;
            Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = 0;
            Session[Constantes.CONST_SESION_ACTUACION_ID + GUID] = 0;
            #endregion

            Session["iAnularActoNotarial"] = null;
            string TarifaNro = string.Empty;
            Enumerador.enmTipoOperacion enmTipoOperacion = Enumerador.enmTipoOperacion.CONSULTA;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Int64 intPersonaActualId = 0;



            if (Session["iPersonaId" + GUID] != null)
                intPersonaActualId = Convert.ToInt64(Session["iPersonaId" + GUID]);
            hid_iPersonaId.Value = intPersonaActualId.ToString();
            Int64 intPersonaSesionId = 0;
            if (Session["iPersonaId" + GUID] != null)
                intPersonaSesionId = Convert.ToInt64(Session["iPersonaId" + GUID]);

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

            Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);
            Session[Constantes.CONST_SESION_ACTUACIONDET_ID + HFGUID.Value] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionDetalleId"]);

            Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value] = Convert.ToInt64(dtActuaciones.Rows[intSeleccionado]["iActuacionId"]);
            Session["CORR_TARIFA"] = dtActuaciones.Rows[intSeleccionado]["vCorrelativoActuacion"].ToString();
            Session["iACTUACION_ID" + HFGUID.Value] = dtActuaciones.Rows[intSeleccionado]["iActuacionId"].ToString();

            if (e.CommandName == "ConsultarAct")
            {
                bool s_Imprime = true;
                string strScript = string.Empty;
                if (s_Imprime)
                {
                    int intModoVista = (int)Enumerador.enmModoVista.HTML;
                    object objModoVista = ConfigurationManager.AppSettings["ModoVistaAutoadhesivo"];
                    if (objModoVista != null)
                    {
                        if (objModoVista.ToString().Trim() != string.Empty)
                        {
                            intModoVista = Convert.ToInt32(objModoVista);
                        }
                    }

                    switch (intModoVista)
                    {
                        case (int)Enumerador.enmModoVista.ITEXT_SHARP:

                            Int64 intActuacionDetalleId = 0;

                            if (HFGUID.Value.Length == 0)
                            {
                                intActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                            }
                            else
                            {
                                intActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]);
                            }

                            DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                            oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                            oDocumentoiTextSharp.CrearAutoAdhesivo();

                            break;
                        default:
                            strScript = "abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?GUID=" + GUID + "', 'AUTOADHESIVOS', 610, 450, '');";
                            Comun.EjecutarScript(Page, strScript);
                            Session["FEC_IMPRESION"] = Util.ObtenerFechaActual(
                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                                Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));
                            break;
                    }
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN CONSULAR", "LIMA no puede imprimir al Autoadhesivo");
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }

                //----------------------------------------------------
                //Fecha: 16/02/2017
                //Autor: Sandra del Carmen Acosta Celis
                //Objetivo: Asignación de variable de session.
                //----------------------------------------------------

                if (dtActuaciones != null)
                {
                    int dtActuacionesValor = 5;
                    Session["dtActuacionesValor"] = dtActuacionesValor;
                }
                else
                {
                    Session["dtActuacionesValor"] = null;
                }
            }
        }

        protected void gdvActuaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            InicializarBusqueda();
            try
            {
                if (txtNroAutoadhesivo.Text.Trim() == string.Empty)
                {
                    ctrlValActuacion.MostrarValidacion(strMensajeValidacionVacio);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtNroAutoadhesivo.Text))
                    {
                        if ((txtNroAutoadhesivo.Text.Trim().Length > 0))
                        {
                            CargarGrillaActuaciones(txtNroAutoadhesivo.Text.Trim());
                        }
                        else
                        {
                            ctrlValActuacion.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES, true, Enumerador.enmTipoMensaje.WARNING);
                            gdvActuaciones.DataSource = null;
                            gdvActuaciones.DataBind();
                        }
                    }
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

            this.hEnter.Value = "0";
            UpdGrvPaginada.Update();
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNroAutoadhesivo.Text))
            {
                CargarGrillaActuaciones(txtNroAutoadhesivo.Text.Trim());
            }
            else
            {
                ctrlValActuacion.MostrarValidacion(strMensajeValidacionVacio);
            }
            UpdGrvPaginada.Update();
        }

        protected void gdvActuaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        #endregion
    }
}