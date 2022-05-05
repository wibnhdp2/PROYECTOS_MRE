using System.Configuration;
using System;
using System.Linq;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using SGAC.BE;
using SGAC.Registro.Actuacion.BL;
using System.Web.Script.Serialization;
using System.Web.Configuration; 
using System.Web;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.Reporting.WebForms;


using System.Drawing;
using MW6PDF417;
using System.Drawing.Imaging;
using Microsoft.Security.Application;
using SGAC.Registro.Persona.BL;

namespace SGAC.WebApp.Registro
{
    public partial class FrmActuacionMilitar : MyBasePage
    {
        #region CAMPOS
        private Int64 iMilitarId;
        private string strVariableMilitar = "Militar_Id";
        private string strIdMilitar = "IdMilitar";

        private string strVariableAccion = "Actuacion_Accion";
        private string strVariableIndice = "RegistroMilitar_Indice";
        private string strVariableParticipanteDt = "RegistroMilitar_Participante_DT";
        private string strVariableParticipanteSel = "RegistroMilitar_Participante_SEL";

        private string strNombreEntidad = "REGISTRO MILITAR";

        //private static List<RE_PARTICIPANTE> loParticipanteContainer = new List<RE_PARTICIPANTE>();
        private static bool cargado = false;
        private static bool edicion = false;
        private static int edicion_rowIndex = -1;

        private int IntEdadMinimaMilitar = Convert.ToInt16(WebConfigurationManager.AppSettings["edadminimamilitar"]);

        private int IntPreviaImpresion=0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Load_Imagenes(); 

            MaintainScrollPositionOnPostBack = true;
            Session["bCivil"] = null;
            
            try
            {
                ctrlToolBarRegistro.VisibleIButtonGrabar = true;
                ctrlToolBarRegistro.VisibleIButtonCancelar = true;
                ctrlToolBarRegistro.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarRegistro_btnGrabarHandler);
                ctrlToolBarRegistro.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarRegistro_btnCancelarHandler);

                ctrlToolbarFormato.VisibleIButtonGrabar = true;
                chk_verificar_CheckedChanged(null, null);
                ctrlToolbarFormato.VisibleIButtonCancelar = true;
                ctrlToolbarFormato.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(ctrlToolbarFormato_btnGrabar);
                ctrlToolbarFormato.btnCancelarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolbarFormato_btnCancelar);
                
                // Jonatan -- 20/07/2017 -- Botones controles de usuario, reimpresión y anulación de autoadhesivos
                ctrlReimprimirbtn1.btnReimprimirHandler += new Accesorios.SharedControls.ctrlReimprimirbtn.OnButtonReimprimirClick(ctrlReimprimirbtn_btnReimprimirHandler);
                ctrlBajaAutoadhesivo1.btnAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAnularClick(ctrlBajaAutoadhesivo_btnAnularAutoahesivo);
                ctrlBajaAutoadhesivo1.btnAceptarAnularHandler += new Accesorios.SharedControls.ctrlBajaAutoadhesivo.OnButtonAceptarAnulacionClick(ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo);
                //------------------------------------------------------
            
                btnGrabarVinculacion.OnClientClick = "return ValidarAutoadhesivo()";
                ctrlToolbarFormato.btnGrabar.OnClientClick = "return ValidarGrabar();";
                imgBuscar.OnClientClick = "return ValidarPersona();";


                ////////////////////
                btnImpresionConstanciaVP.OnClientClick = "return ValidarGrabar();";
                btnImpresionRegistroVP.OnClientClick = "return ValidarGrabar();";
                hifVistaPrevia.Value = IntPreviaImpresion.ToString();
                ////////////////////
                
                txtFecNac.EndDate = DateTime.Now;
                ctrlAdjunto.IsMilitar = true;
                if (!Page.IsPostBack)
                {
                    string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
                    if (Convert.ToInt64(codPersona) > 0)
                    {
                        GetDataPersona(Convert.ToInt64(codPersona));
                        string codPersonaEncr = Request.QueryString["CodPer"].ToString();
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncr + "&Juridica=1";
                        }
                        else
                        { // PERSONA NATURAL
                            Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncr;
                        }
                        
                    }
                    //if (Request.QueryString["GUID"] != null)
                    //{
                    //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                    //    Tramite.PostBackUrl = "~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value;
                    //}
                    //else
                    //{
                    //    HFGUID.Value = "";
                    //}
                    
                    string StrNombreArchReporte = string.Empty; 
                    
                    string hdn_CONFORMIDAD_DE_TEXTO;
                    hdn_CONFORMIDAD_DE_TEXTO = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.AVISOS, Convert.ToInt32(Enumerador.enmNotarialAvisos.CONFORMIDAD_DE_TEXTO), "valor");
                    chk_verificar.Text = hdn_CONFORMIDAD_DE_TEXTO;

                    CargarDropDownList();
                    CargarDatosIniciales();
                    PintarDatosPestaniaRegistro();
                    CargarDatosFormato();

                    mtParticipanteInitialize();

                    if ((Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.ACTUALIZACION) ||
                        (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA))
                    {
                        if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                        {
                            ctrlAdjunto.isConsultar = true;
                        }
                        else 
                        {
                            ctrlAdjunto.isConsultar = false;
                        }
                        CargarDatosActuacionDetalle();

                        if (Session["COD_AUTOADHESIVO"].ToString() != String.Empty)
                        {
                            btnGrabarVinculacion.Enabled = false;
                        }

                        if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                        {
                            
                            this.ctrlToolbarFormato.btnGrabar.Enabled = false;
                            this.ctrlAdjunto.BtnGrabActAdj.Enabled = false;
                            this.btnGrabarVinculacion.Enabled = false;
                            this.ctrlToolBarRegistro.btnGrabar.Enabled = false;

                            ctrlUbigeoLineal_Domicilio.HabilitaControl(false);
                            ctrlUbigeo_Nacimiento.HabilitaControl(false);
                            //ctrlGridParticipante1.HabilitaControl(false);                            
                            HabilitaControl(false);
                            HabilitaControlesTabFormatoTotal(false);
                        }
                        else
                        {
                            ClientScript.RegisterHiddenField("isPostBack", "0");
                            if (Session["bTramiteActuacion"] != null)
                            {
                                //Entra cuando existe una actuación y se deshabilita el tab formato.
                                if (Convert.ToBoolean(Session["bTramiteActuacion"]))
                                {
                                    HabilitaControlesTabFormatoTotal(false);
                                    ctrlToolbarFormato.btnGrabar.Enabled = false;
                                    updToolFormato.Update();
                                    return;
                                }

                            }

                            //---------------------------------------------
                            HabilitaControlesTabFormato(true);
                            ctrlUbigeoLineal_Domicilio.HabilitaControl(true);
                            ctrlUbigeo_Nacimiento.HabilitaControl(true);
                            //ctrlGridParticipante1.HabilitaControl(true);
                            HabilitaControl(true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterHiddenField("isPostBack", "0");
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //Jonatan -- 20/07/2017 -- reimprimir un autoadhesivo
        void ctrlReimprimirbtn_btnReimprimirHandler()
        {
            if (ctrlReimprimirbtn1.SeImprime == "OK")
            {
                CargarDatosActuacionDetalle();
                //btnVistaPrev.Enabled = true;
                ctrlReimprimirbtn1.Activar = chkImpresion.Checked;
            }
            else
            {
                if (ctrlReimprimirbtn1.Activar)
                {
                    btnVistaPrev.Enabled = false;
                }
            }
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("FrmActoMilitar.aspx?cod=1&GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoMilitar.aspx?cod=1&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("FrmActoMilitar.aspx?cod=1&CodPer=" + codPersona, false);
            }
            
            //}
        }

        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAnularAutoahesivo()
        {
            //ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
            Comun.EjecutarScript(this, "Popup(" + hCodAutoadhesivo.Value.ToString() + ");");
        }
        // jonatan -- dar de baja un autoadhesivo
        void ctrlBajaAutoadhesivo_btnAceptarAnularAutoahesivo()
        {
            ctrlBajaAutoadhesivo1.CodInsumo = hCodAutoadhesivo.Value;
            //CargarDatosActuacionDetalle();
            //String scriptMover = @"$(function(){{ MoveTabIndex(5);}});";
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            //updRegPago.Update();

            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("FrmActoMilitar.aspx?cod=0&GUID=" + HFGUID.Value, false);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmActoMilitar.aspx?cod=0&CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("FrmActoMilitar.aspx?cod=0&CodPer=" + codPersona, false);
            }
            
            //}
        }
        
        void ctrlToolBarRegistro_btnCancelarHandler()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona, false);
            }
            
            //}
        }

        void ctrlToolBarRegistro_btnGrabarHandler()
        {
            ActualizarActuacionDetalle();   
        }    

        protected void ctrlToolBarVinculacion_btnGrabar()
        {
            DateTime dFecActual = Util.ObtenerFechaActual(
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));

            DateTime dFecImpresion = Comun.FormatearFecha("01/01/1800");

            string strScript = string.Empty;
            string strMensaje = string.Empty;

            ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            int intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(Convert.ToInt64(Session["iActuacionId"]),
                                            Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]),
                                            (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                                            txtCodAutoadhesivo.Text.Trim(),
                                            dFecActual,
                                            true,
                                             dFecImpresion,
                                            0, // FUNCIONARIO
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                        ref strMensaje);
            
            if (strMensaje == string.Empty)
            {
                txtCodAutoadhesivo.Enabled = false;
                chkImpresion.Enabled = false;

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                    "VINCULACIÓN", "La vinculación se realizó correctamente.", false, 200, 300);
                Comun.EjecutarScript(Page, strScript);
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR,
                    "VINCULACIÓN", strMensaje, false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        protected void ctrlToolBarVinculacion_btnCancelar()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona, false);
            }
            
            //}
        }
        
        private void CargarDropDownList()
        {
            Util.CargarParametroDropDownList(ddlTipoPago, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);
            Util.CargarParametroDropDownList(ddlNomBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
            Util.CargarParametroDropDownList(ddlCalificacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_CALIFICACION), true);
            Util.CargarParametroDropDownList(ddlInstitucion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MILITAR_FUERZA_ARMADA), true);
            Util.CargarParametroDropDownList(ddlServicioReserva, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_MILITAR_SERVICIO_RESERVA_MILITAR), true);
            Util.CargarParametroDropDownList(ddlGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            Util.CargarParametroDropDownList(ddlEstadoCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
            Util.CargarParametroDropDownList(ddlInscripcion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_INSCRIPCION), true);
            Util.CargarParametroDropDownList(ddlColorTez, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_TEZ), true);
            Util.CargarParametroDropDownList(ddlColorOjos, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_OJOS), true);
            Util.CargarParametroDropDownList(ddlGrupoSanguineo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GRUPO_SANGUINEO), true);
            Util.CargarParametroDropDownList(ddlNacLugarTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_NACIMIENTO_LUGAR), true);

            ctrlUbigeo_Nacimiento.UbigeoRefresh();

            DataTable dtTipDoc = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
            DataView dv = dtTipDoc.DefaultView;
            DataTable dtOrdenado = dv.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";

            ctrlUbigeoLineal_Domicilio.UbigeoRefresh();
        }        
        private void HabilitaControlesTabFormato(bool bHabilitado)
        {
            txtClase.Enabled = bHabilitado;
            txtLibro.Enabled = bHabilitado;
            txtFolio.Enabled = bHabilitado;
            //ddlCalificacion.Enabled = bHabilitado;
            //ddlInstitucion.Enabled = bHabilitado;
            txtHora.Enabled = bHabilitado;
            //ddlGenero.Enabled = bHabilitado;
            //txtNombresTitular.Enabled = bHabilitado;
            //txtApePatTitular.Enabled = bHabilitado;
            //txtApeMatTitular.Enabled = bHabilitado;
            ddlEstadoCivil.Enabled = bHabilitado;
            txtNroHijos.Enabled = bHabilitado;
            ddlInscripcion.Enabled = bHabilitado;
            txtEstatura.Enabled = bHabilitado;
            txtPeso.Enabled = bHabilitado;
            ddlColorTez.Enabled = bHabilitado;
            ddlColorOjos.Enabled = bHabilitado;
            ddlGrupoSanguineo.Enabled = bHabilitado;
            txtSeniasPart.Enabled = bHabilitado;

            ddlNacLugarTipo.Enabled = bHabilitado;
            txtNacLugar.Enabled = bHabilitado;

            txtDireccion.Enabled = bHabilitado;
            txtDomicilioTelef.Enabled = bHabilitado;

            updToolFormato.Update();
        }

        //Héctor Vásquez
        private void HabilitaControlesTabFormatoTotal(bool bHabilitado)
        {
            txtClase.Enabled = bHabilitado;
            txtLibro.Enabled = bHabilitado;
            txtFolio.Enabled = bHabilitado;

            ddlCalificacion.Enabled = bHabilitado;
            ddlInstitucion.Enabled = bHabilitado;
            ddlServicioReserva.Enabled = bHabilitado;

            txtFecNac.Enabled = bHabilitado;
            txtHora.Enabled = bHabilitado;
            ddlGenero.Enabled = bHabilitado;

            txtNombresTitular.Enabled = bHabilitado;
            txtApePatTitular.Enabled = bHabilitado;
            txtApeMatTitular.Enabled = bHabilitado;

            ddlEstadoCivil.Enabled = bHabilitado;
            txtNroHijos.Enabled = bHabilitado;
            ddlInscripcion.Enabled = bHabilitado;

            txtEstatura.Enabled = bHabilitado;
            txtPeso.Enabled = bHabilitado;
            ddlColorTez.Enabled = bHabilitado;

            ddlColorOjos.Enabled = bHabilitado;
            ddlGrupoSanguineo.Enabled = bHabilitado;
            txtSeniasPart.Enabled = bHabilitado;

            ddlNacLugarTipo.Enabled = bHabilitado;
            txtNacLugar.Enabled = bHabilitado;

            ctrlUbigeo_Nacimiento.HabilitaControl(bHabilitado);

            txtDireccion.Enabled = bHabilitado;
            txtDomicilioTelef.Enabled = bHabilitado;

            ctrlUbigeoLineal_Domicilio.HabilitaControl(bHabilitado);
            ctrlUbigeo1.HabilitaControl(bHabilitado);
             
            ddl_TipoParticipante.Enabled = bHabilitado;
            //ddl_TipoDatoParticipante.Enabled = bHabilitado;

            //ddl_TipoVinculoParticipante.Enabled = bHabilitado;

            ddl_TipoDocParticipante.Enabled = bHabilitado;
            txtNroDocParticipante.Enabled = bHabilitado;

            ddl_NacParticipante.Enabled = bHabilitado;
            txtNomParticipante.Enabled = bHabilitado;

            txtApePatParticipante.Enabled = bHabilitado;
            txtApeMatParticipante.Enabled = bHabilitado;

            txtDireccionParticipante.Enabled = bHabilitado;

            

            txtObservacionesAP.Enabled = bHabilitado;

            btnAceptar.Enabled = false;
            btnCancelar.Enabled = false;

            updToolFormato.Update();
        }    

        void FillWebCombo(DataTable pDataTable, DropDownList pWebCombo, String str_pDescripcion, String str_pValor)
        {
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();
            pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }
        
        void ctrlToolbarFormato_btnGrabar()
        {
            try
            {

                #region MDIAZ - 20150301 - FECHA INVÁLIDA
                //if (!txtFecNac.ToDateTime())
                //{
                //    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Falta ingresar la fecha"));
                //    return;
                //}
                //DateTime nacimiento = Convert.ToDateTime(txtFecNac.Text);
                //DateTime nacimiento = Comun.FormatearFecha(txtFecNac.get_Value);
                #endregion
                DateTime nacimiento = txtFecNac.Value();
                if (nacimiento == new DateTime(1900, 1, 1))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta ingresar la fecha"));
                    return;
                }

                //if (Convert.ToInt32(ddlGenero.SelectedValue) == (int)Enumerador.enmTipoActa.NACIMIENTO)
                if (ddlGenero.SelectedIndex <= 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta seleccionar el género"));
                    return;
                }

                if (ddlEstadoCivil.SelectedIndex <= 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta seleccionar el estado civil"));
                    return;
                }

                if (txtNroHijos.Text.Trim().Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta ingresar el Nro de hijos"));
                    return;
                }

                if (txtDireccion.Text.Trim().Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta ingresar la dirección"));
                    return;
                }

                if (txtDomicilioTelef.Text.Trim().Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta ingresar el teléfono"));
                    return;
                }

                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                if (edad >= IntEdadMinimaMilitar)
                {
                    GrabarDatosFormato();
                }
                else
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Edad no permitida. Connacional no tiene " + IntEdadMinimaMilitar + " años de edad"));

            }
            catch
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        void ctrlToolbarFormato_btnCancelar()
        {
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1",false);
            }
            else
            { // PERSONA NATURAL
                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersona,false);
            }
            
            //}
        }

        private DataTable dtCargaParticipantes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("acpa_iActuacionParticipanteId", typeof(string));
            dt.Columns.Add("acpa_iActuacionDetalleId", typeof(string));
            dt.Columns.Add("acpa_iPersonaId", typeof(string));
            dt.Columns.Add("acpa_sTipoParticipanteId", typeof(string));
            dt.Columns.Add("acpa_sTipoDatoId", typeof(string));
            dt.Columns.Add("acpa_sTipoVinculoId", typeof(string));
            dt.Columns.Add("acpa_cEstado", typeof(string));
            dt.Columns.Add("acpa_cNombreParticipante", typeof(string));
            dt.Columns.Add("acpa_NombreTipoParticipante", typeof(string));
            dt.Columns.Add("acpa_cNumeroDocumento", typeof(string));
            return dt;
        }

        private void GrabarDatosFormato()
        {
            string strScript = string.Empty;

            if (Convert.ToInt32(hifRegistroMilitar.Value) < 1)
            {
                #region INSERTAR ACTO MILITAR
                object[] arrParametros = new object[4];
                SGAC.BE.RE_REGISTROMILITAR objMILITAR = new BE.RE_REGISTROMILITAR();
                SGAC.BE.RE_PERSONA objPERSONA = new BE.RE_PERSONA();
                SGAC.BE.RE_RESIDENCIA objResidencia = new BE.RE_RESIDENCIA();
                Proceso p = new Proceso();
                
                //List<BE.RE_PARTICIPANTE> LstParticipantes = ctrlGridParticipante1.getParticipantes();
                //foreach (BE.RE_PARTICIPANTE objParticipante in LstParticipantes)
                List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
                foreach (BE.RE_PARTICIPANTE objParticipante in loParticipanteContainer)
                {
                    objParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objParticipante.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    objParticipante.iActuacionDetId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                }

                objMILITAR = ValoresNuevosFormato();
                objPERSONA = ValoresNuevosFormatoPersona();
                objResidencia = ObtenerDatosResidencia();

                // Para que siempre entre en modo insercion ...
                objResidencia.resi_iResidenciaId = -1;

                AgregarParticipanteRecurrente();

                arrParametros[0] = objMILITAR;
                arrParametros[1] = objPERSONA;
                arrParametros[2] = objResidencia;
                arrParametros[3] = loParticipanteContainer;                

                p.Invocar(ref arrParametros, "SGAC.BE.RE_REGISTROMILITAR", Enumerador.enmAccion.INSERTAR);

                EliminarParticipanteRecurrente();
                
                iMilitarId = ((BE.RE_REGISTROMILITAR)arrParametros[0]).remi_iRegistroMilitarId;
                hifRegistroMilitar.Value = iMilitarId.ToString();

                Session["Actuacion_Accion"] = Enumerador.enmTipoOperacion.ACTUALIZACION;
                CargarDatosFormato();

                if (iMilitarId > 0)
                {
                    if (IntPreviaImpresion == 0)
                    { 
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                        strScript += Util.HabilitarTab(4);
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
                #endregion
            }
            else
            {
                #region MODIFICAR ACTO MILITAR
                object[] arrParametros = new object[3];
                SGAC.BE.RE_REGISTROMILITAR objMILITAR = new BE.RE_REGISTROMILITAR();
                SGAC.BE.RE_PERSONA objPERSONA = new BE.RE_PERSONA();
                SGAC.BE.RE_RESIDENCIA objRESIDENCIA = new BE.RE_RESIDENCIA();

                objMILITAR = ValoresNuevosFormato();
                objPERSONA = ValoresNuevosFormatoPersona();
                objRESIDENCIA = ObtenerDatosResidencia();

                Proceso p = new Proceso();
                arrParametros[0] = objMILITAR;
                arrParametros[1] = objPERSONA;
                arrParametros[2] = objRESIDENCIA;                
                //List<RE_PARTICIPANTE> lstParticipantes = ctrlGridParticipante1.getParticipantes();
                //foreach (BE.RE_PARTICIPANTE objParticipante in lstParticipantes)
                List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
                foreach (BE.RE_PARTICIPANTE objParticipante in loParticipanteContainer)
                {
                    objParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objParticipante.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    objParticipante.iActuacionDetId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    objParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                    if (objParticipante.sTipoParticipanteId == Convert.ToInt16(Enumerador.enmTipoParticipanteMilitar.TITULAR))
                    {
                        if (txtFecNac.Text.Length > 0)
                        {
                            objParticipante.pers_dNacimientoFecha = Comun.FormatearFecha(txtFecNac.Text);
                        }
                        else
                        {
                            objParticipante.pers_dNacimientoFecha = null;
                        }

                        objParticipante.sGeneroId = Convert.ToInt16(ddlGenero.SelectedValue);
                        objParticipante.pers_sEstadoCivilId = Convert.ToInt16(ddlEstadoCivil.SelectedValue);
                    }
                }
                
                object[] arrParametrosActualizar = new object[4];
                arrParametrosActualizar[0] = arrParametros[0];
                arrParametrosActualizar[1] = arrParametros[1];
                arrParametrosActualizar[2] = arrParametros[2];
                arrParametrosActualizar[3] = loParticipanteContainer;

                int intResultado= Convert.ToInt32(p.Invocar(ref arrParametrosActualizar, "SGAC.BE.RE_REGISTROMILITAR", Enumerador.enmAccion.MODIFICAR));
                
                if (intResultado > 0)
                {
                    loParticipanteContainer = loParticipanteContainer.Where(x => x.cEstado != "E").ToList();
                    Session["Participante"] = loParticipanteContainer;
                    if (IntPreviaImpresion == 0)
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                        strScript += Util.HabilitarTab(4);
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_OPERACION_FALLIDA));
                }

                #endregion
            }  
        }

        public bool Habilitar_Eliminar(string strTitular)
        {
            return (strTitular.ToUpper().Equals("TITULAR")) ? false : true;
        }
        private SGAC.BE.RE_REGISTROMILITAR ValoresNuevosFormato()
        {
            if (Session != null)
            {
                SGAC.BE.RE_REGISTROMILITAR objMILITAR = new BE.RE_REGISTROMILITAR();
                if (hifRegistroMilitar.Value.ToString().Equals("")) hifRegistroMilitar.Value = "0";
                objMILITAR.remi_iRegistroMilitarId = Convert.ToInt64(hifRegistroMilitar.Value);
                objMILITAR.remi_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                objMILITAR.remi_sCalificacionMilitarId = Convert.ToInt16(ddlCalificacion.SelectedValue);
                objMILITAR.remi_sInstitucionMilitarId = Convert.ToInt16(ddlInstitucion.SelectedValue);
                objMILITAR.remi_IFuncionarioId = null;
                objMILITAR.remi_sServicioReservaId = Convert.ToInt16(ddlServicioReserva.SelectedValue);
                objMILITAR.remi_vClase = txtClase.Text.ToUpper();
                objMILITAR.remi_vLibro = txtLibro.Text.ToUpper();
                objMILITAR.remi_sFolio = Convert.ToInt16(txtFolio.Text);
                objMILITAR.remi_sNumeroHijos = Convert.ToInt16(txtNroHijos.Text);
                objMILITAR.remi_IUsuarioAprobacionId = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objMILITAR.remi_vIPAprobacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objMILITAR.remi_dFechaAprobacion = DateTime.Now;
                objMILITAR.remi_bDigitalizadoFlag = true;
                objMILITAR.remi_vObservaciones = txtObservaciones.Text.ToUpper();
                objMILITAR.remi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objMILITAR.remi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objMILITAR.remi_dFechaCreacion = DateTime.Now;
                objMILITAR.remi_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objMILITAR.remi_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objMILITAR.remi_dFechaModificacion = DateTime.Now;
                objMILITAR.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objMILITAR.remi_cEstado = "A";
                objMILITAR.remi_sTipoSuscripcion = Convert.ToInt16(ddlInscripcion.SelectedItem.Value);
                return objMILITAR;
            }

            return null;
        }

        void AgregarParticipanteRecurrente()
        {
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];


            BE.RE_PARTICIPANTE loParticipante = new RE_PARTICIPANTE();



            loParticipante.sTipoParticipanteId = Convert.ToInt16(Enumerador.enmTipoParticipanteMilitar.RECURRENTE);
            loParticipante.iActuacionDetId= Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);

            //if (HFGUID.Value.Length > 0)
            //{
            //    loParticipante.iPersonaId = Convert.ToInt32(ViewState["iPersonaId"].ToString());
            //    loParticipante.sTipoDocumentoId = Convert.ToInt16(ViewState["iDocumentoTipoId"].ToString());
            //    loParticipante.vNumeroDocumento = ViewState["NroDoc"].ToString();

            //    if (ViewState["PER_NACIONALIDAD"].ToString().ToLower() != "&nbsp;" && ViewState["PER_NACIONALIDAD"].ToString() != string.Empty)
            //    {
            //        loParticipante.sNacionalidadId = Convert.ToInt16(ViewState["PER_NACIONALIDAD"].ToString());
            //    }

            //    loParticipante.vNombres = ViewState["Nombres"].ToString();
            //    loParticipante.vPrimerApellido = ViewState["ApePat"].ToString();
            //    loParticipante.vSegundoApellido = ViewState["ApeMat"].ToString();
            //}
            //else
            //{
                loParticipante.iPersonaId = Convert.ToInt32(ViewState["iPersonaId"].ToString());
                loParticipante.sTipoDocumentoId = Convert.ToInt16(ViewState["iDocumentoTipoId"].ToString());
                loParticipante.vNumeroDocumento = ViewState["NroDoc"].ToString();

                if (ViewState["PER_NACIONALIDAD"].ToString().ToLower() != "&nbsp;" && ViewState["PER_NACIONALIDAD"].ToString() != string.Empty)
                {
                    loParticipante.sNacionalidadId = Convert.ToInt16(ViewState["PER_NACIONALIDAD"].ToString());
                }

                loParticipante.vNombres = ViewState["Nombre"].ToString();
                loParticipante.vPrimerApellido = ViewState["ApePat"].ToString();
                loParticipante.vSegundoApellido = ViewState["ApeMat"].ToString();
            //}


            loParticipante.vDireccion = string.Empty;
            loParticipante.cEstado = "A";
            loParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            loParticipante.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);

            loParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);


            loParticipanteContainer.Add(loParticipante);

        }

        void EliminarParticipanteRecurrente()
        {
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            Session["Participante"] = loParticipanteContainer.Where(x => x.sTipoParticipanteId != Convert.ToInt16(Enumerador.enmTipoParticipanteMilitar.RECURRENTE)).ToList();
        }



        private SGAC.BE.RE_PERSONA ValoresNuevosFormatoPersona()
        {
            if (Session != null)
            {
                SGAC.BE.RE_PERSONA objPERSONA = new BE.RE_PERSONA();
                objPERSONA.pers_vApellidoPaterno = txtApePatTitular.Text;
                objPERSONA.pers_vApellidoMaterno = txtApeMatTitular.Text;
                objPERSONA.pers_vNombres = txtNombresTitular.Text;
                objPERSONA.pers_sEstadoCivilId = Convert.ToInt16(ddlEstadoCivil.SelectedValue);
                objPERSONA.pers_sGeneroId = Convert.ToInt16(ddlGenero.SelectedValue);
                objPERSONA.pers_sPeso = Convert.ToInt16(txtPeso.Text);
                objPERSONA.pers_vEstatura = txtEstatura.Text;
                objPERSONA.pers_sColorTezId = Convert.ToInt16(ddlColorTez.SelectedValue);
                objPERSONA.pers_sColorOjosId = Convert.ToInt16(ddlColorOjos.SelectedValue);
                objPERSONA.pers_sGrupoSanguineoId = Convert.ToInt16(ddlGrupoSanguineo.SelectedValue);
                objPERSONA.pers_vSenasParticulares = txtSeniasPart.Text.ToUpper();
                objPERSONA.pers_sOcurrenciaTipoId = Convert.ToInt16(ddlNacLugarTipo.SelectedValue);
                objPERSONA.pers_vLugarNacimiento = txtNacLugar.Text;
                objPERSONA.pers_IOcurrenciaCentroPobladoId = null;
                if (txtFecNac.Value() != DateTime.MinValue)
                {
                    DateTime datFechaNac = new DateTime();
                    if (!DateTime.TryParse((txtFecNac.Text + " " + txtHora.Text), out datFechaNac))
                    {
                        datFechaNac = Comun.FormatearFecha(txtFecNac.Text + " " + txtHora.Text);
                    }
                    objPERSONA.pers_dNacimientoFecha = datFechaNac;
                } 
                objPERSONA.pers_cNacimientoLugar = ctrlUbigeo_Nacimiento.getResidenciaUbigeo();
                objPERSONA.pers_iPersonaId = Convert.ToInt64(hi_iPersonaId.Value);
                objPERSONA.pers_cEstado = ((int) Enumerador.enmEstado.ACTIVO).ToString();
                objPERSONA.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPERSONA.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objPERSONA.pers_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objPERSONA.pers_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objPERSONA.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                return objPERSONA;
            }
            return null;
        }

        private SGAC.BE.RE_RESIDENCIA ObtenerDatosResidencia()
        {
            SGAC.BE.RE_RESIDENCIA objResidencia = new BE.RE_RESIDENCIA();
            objResidencia.resi_iResidenciaId = Convert.ToInt64(hifResidenciaId.Value);
            objResidencia.resi_sResidenciaTipoId = ((int)Enumerador.enmTipoResidencia.RESIDENCIA);
            objResidencia.resi_vResidenciaDireccion = txtDireccion.Text.ToUpper().Trim();
            objResidencia.resi_vCodigoPostal = hifResidenciaCodigoPostal.Value.ToString();
            objResidencia.resi_vResidenciaTelefono = txtDomicilioTelef.Text.Trim();
            objResidencia.resi_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            objResidencia.resi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objResidencia.resi_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            objResidencia.resi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            objResidencia.resi_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
            objResidencia.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            object strUbigeo = ctrlUbigeoLineal_Domicilio.getResidenciaUbigeo();
            if (strUbigeo != null)
            {
                if (Convert.ToInt64(strUbigeo) > 0)
                    objResidencia.resi_cResidenciaUbigeo = strUbigeo.ToString();
            }
            else
            {
                objResidencia.resi_cResidenciaUbigeo = "";
            }
            return objResidencia;           
        }

        private void CargarDatosIniciales()
        {
            lblDestino.Text = string.Empty;
            string strEtiquetaSolicitante = string.Empty;
            string strNombres = string.Empty;
            string strPrimerApe = string.Empty;
            string strSegundoApe = string.Empty;

            //if (HFGUID.Value.Length > 0)
            //{
            //    if (Session["ApePat" + HFGUID.Value] != null)
            //        strPrimerApe = Session["ApePat" + HFGUID.Value].ToString();
            //    if (Session["ApeMat" + HFGUID.Value] != null)
            //        strSegundoApe = Session["ApeMat" + HFGUID.Value].ToString();
            //    if (Session["Nombres" + HFGUID.Value] != null)
            //    {
            //        if (Session["Nombres" + HFGUID.Value].ToString().Trim() != string.Empty)
            //            strNombres = Session["Nombres" + HFGUID.Value].ToString();
            //    }
            //}
            //else
            //{
            if (ViewState["ApePat"] != null)
                strPrimerApe = ViewState["ApePat"].ToString();
            if (ViewState["ApeMat"] != null)
                strSegundoApe = ViewState["ApeMat"].ToString();
            if (ViewState["Nombre"] != null)
                {
                    if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                        strNombres = ViewState["Nombre"].ToString();
                }
            //}

            strEtiquetaSolicitante = strPrimerApe + " " + strSegundoApe + ", " + strNombres;

            //if (HFGUID.Value.Length > 0)
            //{
            //    if (Session["DescTipDoc" + HFGUID.Value] != null)
            //        strEtiquetaSolicitante += "- " + Session["DescTipDoc" + HFGUID.Value].ToString() + ": ";
            //    if (Session["NroDoc" + HFGUID.Value] != null)
            //        strEtiquetaSolicitante += Session["NroDoc" + HFGUID.Value].ToString();
            //}
            //else
            //{
            if (ViewState["DescTipDoc"] != null)
                strEtiquetaSolicitante += "- " + ViewState["DescTipDoc"].ToString() + ": ";
            if (ViewState["NroDoc"] != null)
                    strEtiquetaSolicitante += ViewState["NroDoc"].ToString();
            //}

            


            lblDestino.Text = strEtiquetaSolicitante;
            txtNombresTitular.Text = strNombres;

            Session["COD_AUTOADHESIVO"] = string.Empty;

            // Participantes
            Session["TIPO_ACTO_PARTICIPANTE"] = (int)Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE;
            //ctrlGridParticipante1.SetControl(loPartcipantes);
            //ctrlGridParticipante1.GridRefresh();

            ddlServicioReserva.SelectedValue = ((int)Enumerador.enmServicioReserva.DISPONIBLE).ToString();
            ddlCalificacion.SelectedValue = ((int)Enumerador.enmCalificacion.EXCEPTUADO).ToString();            
        }

        private int ObtenerIndiceColumnaGrilla(GridView grid, string col)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (((BoundField)grid.Columns[i]).DataField.ToLower() == col.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }      

        protected void btn_Print_Registro_Click(object sender, EventArgs e)
        {
            object[] arrParametros = new object[1];
            Proceso p = new Proceso();
            
                try
                {
                    long i_ID = 0;
                    long.TryParse(Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), out i_ID);

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    int idTipoAdjunto;

                   DataTable dt = new DataTable();

                   dt = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtener(
                        Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), "1",
                        Constantes.CONST_PAGE_SIZE_ADJUNTOS, ref IntTotalCount, ref IntTotalPages);

                   string strimgFotoDeFrente = "";
                   string strimgFotoPerfil = "";
                   string strimgHuellaIDerecha = "";
                   string strimgHuellaIzquierda = "";


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                            idTipoAdjunto = Convert.ToInt32(dt.Rows[i]["sAdjuntoTipoId"]);

                            if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.FOTO))//foto
                            {
                                strimgFotoDeFrente = dt.Rows[i]["vNombreArchivo"].ToString();
                            }
                            
                            if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.FOTO_PERFIL))//foto perfil
                            {
                                strimgFotoPerfil = dt.Rows[i]["vNombreArchivo"].ToString();
                            }
                            
                            if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.HUELLA_INDICE_DERECHO))//indice derecho
                            {
                                strimgHuellaIDerecha = dt.Rows[i]["vNombreArchivo"].ToString();
                            }
                            
                            if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.HUELLA_INDICE_IZQUIERDO))//indice izquierdo
                            {
                                strimgHuellaIzquierda = dt.Rows[i]["vNombreArchivo"].ToString();
                            }
                         
                    }

                        arrParametros[0] = i_ID;
                        String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                        dt = new Reportes.dsActoMilitar().Tables["RM_INSCRIPCION"];
                        dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_REGISTROMILITAR", "CONSULTAR_HOJA_INSCRIPCION");

                        string strRutaimgFotoDeFrente = "";
                        string strRutaimgFotoPerfil = "";
                        string strRutaimgHuellaIDerecha = "";
                        string strRutaimgHuellaIzquierda = "";

                        if (strimgFotoDeFrente.Length > 0)
                        {
                            strRutaimgFotoDeFrente = uploadPath + @"\" + strimgFotoDeFrente;
                            byte[] arrImagen = Util.IMG_CargarImagen(strRutaimgFotoDeFrente);
                            DataColumn column = new DataColumn("Imagen"); 
                            column.DataType = System.Type.GetType("System.Byte[]"); 
                            column.AllowDBNull = true;
                            column.Caption = "Imagen";
                            dt.Columns.Add(column); 
                            dt.Rows[0]["Imagen"] = arrImagen;
                        }

                        if (strimgFotoPerfil.Length > 0)
                        {
                            strRutaimgFotoPerfil = uploadPath + @"\" + strimgFotoPerfil;
                            byte[] arrImagenPerfil = Util.IMG_CargarImagen(strRutaimgFotoPerfil);
                            DataColumn columnPerfil = new DataColumn("ImagenPerfil"); 
                            columnPerfil.DataType = System.Type.GetType("System.Byte[]");
                            columnPerfil.AllowDBNull = true;
                            columnPerfil.Caption = "ImagenPerfil";
                            dt.Columns.Add(columnPerfil); 
                            dt.Rows[0]["ImagenPerfil"] = arrImagenPerfil;
                        }

                        if (strimgHuellaIDerecha.Length > 0)
                        {
                            strRutaimgHuellaIDerecha = uploadPath + @"\" + strimgHuellaIDerecha;
                            byte[] arrImagenHuellaDere = Util.IMG_CargarImagen(strRutaimgHuellaIDerecha);
                            DataColumn columnHuellaDere = new DataColumn("ImagenHuellaDere"); 
                            columnHuellaDere.DataType = System.Type.GetType("System.Byte[]"); 
                            columnHuellaDere.AllowDBNull = true;
                            columnHuellaDere.Caption = "ImagenHuellaDere";
                            dt.Columns.Add(columnHuellaDere); 
                            dt.Rows[0]["ImagenHuellaDere"] = arrImagenHuellaDere;
                            
                        }

                        if (strimgHuellaIzquierda.Length > 0)
                        {
                            strRutaimgHuellaIzquierda = uploadPath + @"\" + strimgHuellaIzquierda;
                            byte[] arrImagenHuellaIzq = Util.IMG_CargarImagen(strRutaimgHuellaIzquierda);
                            DataColumn columnHuellaIzq = new DataColumn("ImagenHuellaIzq");
                            columnHuellaIzq.DataType = System.Type.GetType("System.Byte[]"); 
                            columnHuellaIzq.AllowDBNull = true;
                            columnHuellaIzq.Caption = "ImagenHuellaIzq";
                            dt.Columns.Add(columnHuellaIzq); 
                            dt.Rows[0]["ImagenHuellaIzq"] = arrImagenHuellaIzq;
                        }


                        Microsoft.Reporting.WebForms.ReportViewer viewer = new Microsoft.Reporting.WebForms.ReportViewer();
                        Microsoft.Reporting.WebForms.ReportDataSource rptDataSource = new
                        Microsoft.Reporting.WebForms.ReportDataSource("dsActoMilitar", dt);
                        

                        viewer.LocalReport.DataSources.Add(rptDataSource);
                        viewer.LocalReport.ReportEmbeddedResource = Server.MapPath("~/Reportes/rsActoMilitarInscripcion.rdlc");
                        viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/rsActoMilitarInscripcion.rdlc");

                        //Export to PDF
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string[] streams;
                        Microsoft.Reporting.WebForms.Warning[] warnings;

                        byte[] pdfContent = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        //Return PDF
                        this.Response.Clear();
                        this.Response.ContentType = "application/pdf";
                        this.Response.AddHeader("Content-disposition", "attachment; filename=InscripcionMilitar.pdf");
                        this.Response.BinaryWrite(pdfContent);
                        this.Response.End();


                        updVinculacion.Update();
                }
                catch
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Se presentaron problemas al generar formato."));
                }
            
        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        protected void btn_Print_Constancia_Click(object sender, EventArgs e)
        {
            ImprimirConstancia();
            Comun.EjecutarScriptUniqueIdDinamico(this.Page, "EnableTabIndex(4);", "MoverTab");            
        }

        private void PintarDatosPestaniaRegistro()
        {
            BE.RE_TARIFA_PAGO objTarifaPago = new RE_TARIFA_PAGO();
            objTarifaPago = (BE.RE_TARIFA_PAGO)Session[Constantes.CONST_SESION_OBJ_TARIFA_PAGO];

            txtIdTarifa.Text = objTarifaPago.vTarifa;
            txtDescTarifa.Text = objTarifaPago.vTarifaDescripcion;
            txtMontoSC.Text = string.Format("{0:0.000}", objTarifaPago.dblMontoSolesConsulares);
            txtMontoML.Text = string.Format("{0:0.000}", objTarifaPago.dblMontoMonedaLocal);
            txtTotalSC.Text = string.Format("{0:0.000}", objTarifaPago.dblTotalSolesConsulares);
            txtTotalML.Text = string.Format("{0:0.000}", objTarifaPago.dblTotalMonedaLocal);
            LblFecha.Text = objTarifaPago.datFechaRegistro.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);


            ddlTipoPago.SelectedValue = objTarifaPago.sTipoPagoId.ToString();

            if (objTarifaPago.sTipoPagoId == (int)Enumerador.enmTipoCobroActuacion.PAGADO_EN_LIMA)
            {
                txtNroOperacion.Text = objTarifaPago.vNumeroOperacion;
                ddlNomBanco.SelectedValue = objTarifaPago.sBancoId.ToString();
                ctrFecPago.set_Value = objTarifaPago.datFechaPago;
            }
            txtCantidad.Text = string.Format("{0:0.000}", objTarifaPago.dblCantidad);
            txtObservaciones.Text = objTarifaPago.vObservaciones;
            LblDescMtoML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
            LblDescTotML.Text = Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();

            // Habilitación
            HabilitaCamposPagoActuacion(false);
        }

        private void HabilitaCamposPagoActuacion(bool bolHabilitar = true)
        {
            txtIdTarifa.Enabled = bolHabilitar;
            LstTarifario.Enabled = bolHabilitar;
            imgBuscarTarifarioM.Enabled = bolHabilitar;
            ddlTipoPago.Enabled = bolHabilitar;
            txtCantidad.Enabled = bolHabilitar;
            pnlPagLima.Visible = bolHabilitar;
            lblLeyendaTarifa.Visible = bolHabilitar;
            lblLeyenda.Visible = bolHabilitar;
            
            LstTarifario.Visible = false;
        }

        #region Vinculación
        // Eventos para el manejo de la impresión del Autuadhesivo 
        private void BindGridActuacionesInsumoDetalle(Int64 iActuacionDetalleId)
        {
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();

            SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL objActuacionMantenimientoBL = new SGAC.Registro.Actuacion.BL.ActuacionMantenimientoBL();
            DataTable dtActuacionInsumoDetalle = new DataTable();


            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;
            int PaginaActual = CtrlPageBarActuacionInsumoDetalle.PaginaActual;
            dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages); // objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ctrlPaginadorAct.PaginaActual.ToString(), intPaginaCantidad, IntTotalCount, IntTotalPages);
            hCodAutoadhesivo.Value = dtActuacionInsumoDetalle.Rows[0]["insu_iInsumoId"].ToString();

            //Jonatan -- 20/07/2017 
            if (txtCodAutoadhesivo.Text.Length > 0)
            {
                ctrlBajaAutoadhesivo1.Activar = true;
            }
            else
            {
                ctrlBajaAutoadhesivo1.Activar = false;
            }
            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                Session["bTramiteActuacion"] = true;
                // ctrlValidacion.MostrarValidacion("", false);

                Grd_ActInsDet.DataSource = dtActuacionInsumoDetalle;
                Grd_ActInsDet.DataBind();

                CtrlPageBarActuacionInsumoDetalle.TotalResgistros = IntTotalCount;
                CtrlPageBarActuacionInsumoDetalle.TotalPaginas = IntTotalPages;

                CtrlPageBarActuacionInsumoDetalle.Visible = false;
                if (CtrlPageBarActuacionInsumoDetalle.TotalResgistros > intPaginaCantidad)
                {
                    CtrlPageBarActuacionInsumoDetalle.Visible = true;
                }
                dtActuacionInsumoDetalle = null;
                
            }
        }
        protected void btnVistaPrev_Click(object sender, EventArgs e)
        {
            bool bModoHTML = true;
            object objModoVista = ConfigurationManager.AppSettings["ModoVistaAutoadhesivo"];
            if (objModoVista != null)
            {
                if (objModoVista.ToString().Trim() != string.Empty)
                {
                    if (Convert.ToInt32(objModoVista) == (int)Enumerador.enmModoVista.ITEXT_SHARP)
                        bModoHTML = false;
                }
            }

            if (bModoHTML)
            {
                if (Convert.ToInt16(Session[strIdMilitar]) > 0)
                {
                    string strScript = "abrirVentana('../Registro/FrmRepAutoadhesivo.aspx?GUID=" + HFGUID.Value +"', 'AUTOADHESIVOS', 610, 450, '');";
                    Comun.EjecutarScript(Page, strScript);
                    if (btnGrabarVinculacion.Visible)
                    {
                        
                    }
                    else
                    {
                        
                    }


                    chkImpresion.Enabled = true;

                    Session["FEC_IMPRESION"] = Util.ObtenerFechaActual(
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                        Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));
                }
                else
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,
                                    "REGISTRO MILITAR", "Guarde primero la información para ver el reporte", false, 200, 300);
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            else
            {
                Int64 intActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);

                DocumentoiTextSharp oDocumentoiTextSharp = new DocumentoiTextSharp(this.Page, string.Empty, HttpContext.Current.Server.MapPath("~/Images/Escudo.JPG"));
                oDocumentoiTextSharp.ActuacionDetalleId = intActuacionDetalleId;
                oDocumentoiTextSharp.CrearAutoAdhesivo();
            }

            
        }
        protected void btnGrabarVinculacion_Click(object sender, EventArgs e)
        {
            String strScript = String.Empty;
            //if (Convert.ToInt32(hifRegistroMilitar.Value) > 0)
            if (Convert.ToInt64(Session[strIdMilitar])>0)
            {
                //if (chkImpresion.Checked)
                //{
                    Int64 intActDetalleId = 0;

                    if (Session["NuevoRegistro"] != null)
                        if (!Convert.ToBoolean(Session["NuevoRegistro"]))
                            intActDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);

                    String FormatoFecha = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

                    if (FormatoFecha.Trim() == String.Empty)
                    {
                        FormatoFecha = "MMM-dd-yyyy";
                    }

                    DateTime dFecActual = Util.ObtenerFechaActual(
                                                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria")),
                                                    Convert.ToInt16(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano")));

                    //DateTime dFecImpresion =  "01/01/0001"; //Convert.ToDateTime(Session["FEC_IMPRESION"]);  // Convert.ToDateTime(Session["FEC_IMPRESION"].ToString(FormatoFecha.ToString())); 
                    DateTime dFecImpresion = Comun.FormatearFecha("01/01/1800");

               

                    string strMensaje = string.Empty;

                    ActuacionMantenimientoBL oActuacionMantenimientoBL = new ActuacionMantenimientoBL();
                    int intResultado = oActuacionMantenimientoBL.VincularAutoadhesivo(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACION_ID + HFGUID.Value]),
                                            Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]),
                                            (int)Enumerador.enmInsumoTipo.AUTOADHESIVO,
                                            txtCodAutoadhesivo.Text.Trim(),
                                            dFecActual,
                                            false,
                                             dFecActual,
                                            0, // FUNCIONARIO
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]),
                                                ref strMensaje);

                    if (strMensaje == string.Empty)
                    {
                        btnGrabarVinculacion.Enabled = false;
                        txtCodAutoadhesivo.Enabled = false;
                        chkImpresion.Enabled = false;
                        btnVistaPrev.Enabled = true;

                        Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();
                        if (txtCodAutoadhesivo.Text.Trim() != string.Empty)
                        {
                            ctrlToolBarRegistro.btnGrabar.Enabled = false;
                            ctrlToolbarFormato.btnGrabar.Enabled = false;

                        }
                        

                        #region Tipo Adjunto
                        ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text.Trim());
                        ctrlAdjunto.CargarTipoArchivo();
                        updActuacionAdjuntar.Update();
                        #endregion

                        HabilitaControlesTabFormatoTotal(false);
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                            "VINCULACIÓN", "La vinculación se realizó correctamente.", false, 200, 300);
                        Comun.EjecutarScript(Page, strScript);
                        BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));

                      
                        ctrlToolbarFormato.btnGrabar.Enabled = false;
                        btnGrabarVinculacion.Enabled = false;
                        HabilitaControlesTabFormatoTotal(false);
                        updVinculacion.Update();
                    }
                    else
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "VINCULACIÓN", strMensaje, false, 200, 400);
                        Comun.EjecutarScript(Page, strScript);
                    }
                //}
                //else 
                //{
                //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VINCULACIÓN", "Falta Validar Impresión Correcta.", false, 200, 400);
                //    Comun.EjecutarScript(Page, strScript);
                //}
            }
            else
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "VINCULACIÓN", "Debe Grabar el Registro Militar antes de Vincular.", false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
            }
        }
        protected void ctrlPagActuacionInsumoDetalle_Click(object sender, EventArgs e)
        {
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session["ActuacionDetalleId" + HFGUID.Value]));
            updVinculacion.Update();
        }
        #endregion        

        protected void chkImpresion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImpresion.Checked)
            {
                btnGrabarVinculacion.Enabled = true;
                txtCodAutoadhesivo.Enabled = true;
            }
            else
            {
                txtCodAutoadhesivo.Enabled = false;
                btnGrabarVinculacion.Enabled = false;
            }

            updVinculacion.Update();
        }

        private void CargarDatosFormato()
        {
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            long lngPersonaId = 0;

            //if (HFGUID.Value.Length > 0)
            //{
            //    lngPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //}
            //else
            //{
            lngPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
            //}

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            BE.RE_ACTUACIONMILITAR objActuacion = objBL.ObtenerActuacionMilitar(lngActuacionDetalleId, lngPersonaId);
            
            this.hi_iPersonaId.Value = objActuacion.PERSONA.pers_iPersonaId.ToString();  
            this.hifRegistroMilitar.Value = objActuacion.REGISTROMILITAR.remi_iRegistroMilitarId.ToString();
            Session[strIdMilitar]=objActuacion.REGISTROMILITAR.remi_iRegistroMilitarId.ToString();

            txtClase.Text = objActuacion.REGISTROMILITAR.remi_vClase;
            txtLibro.Text = objActuacion.REGISTROMILITAR.remi_vLibro;
            txtFolio.Text = (objActuacion.REGISTROMILITAR.remi_sFolio == 0 ) ? "" : objActuacion.REGISTROMILITAR.remi_sFolio.ToString();


            if (objActuacion.REGISTROMILITAR.remi_sCalificacionMilitarId > 0)
                ddlCalificacion.SelectedValue = objActuacion.REGISTROMILITAR.remi_sCalificacionMilitarId.ToString();
            else
                ddlCalificacion.SelectedValue = ((int)Enumerador.enmCalificacion.EXCEPTUADO).ToString();

            if (objActuacion.REGISTROMILITAR.remi_sInstitucionMilitarId > 0)
                ddlInstitucion.SelectedValue = objActuacion.REGISTROMILITAR.remi_sInstitucionMilitarId.ToString();
            else
                ddlInstitucion.SelectedIndex = 0;

            if (objActuacion.REGISTROMILITAR.remi_sServicioReservaId > 0)
                ddlServicioReserva.SelectedValue = objActuacion.REGISTROMILITAR.remi_sServicioReservaId.ToString();
            else
                ddlServicioReserva.SelectedValue = ((int) Enumerador.enmServicioReserva.DISPONIBLE).ToString();

            ddlInscripcion.SelectedValue = objActuacion.REGISTROMILITAR.remi_sTipoSuscripcion.ToString();
            // FILIACIÓN DEL INSCRITO
            if (objActuacion.PERSONA.pers_dNacimientoFecha != DateTime.MinValue)
            {
                DateTime datFechaNacimiento = Comun.FormatearFecha(objActuacion.PERSONA.pers_dNacimientoFecha.ToString());
                txtFecNac.set_Value = datFechaNacimiento;
                txtHora.Text = datFechaNacimiento.ToString("HH:mm");
                txtFecNac.Enabled = false;
                txtHora.Enabled = (txtHora.Text == "00:00")?true:false;
            }
            else
            {
                txtHora.Text = string.Empty;
                txtFecNac.Text = "";
            }

            if (objActuacion.PERSONA.pers_sGeneroId > 0)
                ddlGenero.SelectedValue = objActuacion.PERSONA.pers_sGeneroId.ToString();
            else
            {
                ddlGenero.SelectedIndex = 0;
                ddlGenero.Enabled = true;
            }

            txtNombresTitular.Text = objActuacion.PERSONA.pers_vNombres;
            txtApePatTitular.Text = objActuacion.PERSONA.pers_vApellidoPaterno;
            txtApeMatTitular.Text = objActuacion.PERSONA.pers_vApellidoMaterno;
            if (objActuacion.PERSONA.pers_sEstadoCivilId > 0)
            {
                ddlEstadoCivil.SelectedValue = objActuacion.PERSONA.pers_sEstadoCivilId.ToString();
                ddlEstadoCivil.Enabled = false;
            }
            else
            {
                ddlEstadoCivil.SelectedIndex = 0;
            }
            txtNroHijos.Text = objActuacion.REGISTROMILITAR.remi_sNumeroHijos.ToString();
            if (objActuacion.REGISTROMILITAR.remi_sTipoSuscripcion > 0)
                ddlInscripcion.SelectedValue = objActuacion.REGISTROMILITAR.remi_sTipoSuscripcion.ToString();
            else
                ddlInscripcion.SelectedIndex = 0;
            txtEstatura.Text = objActuacion.PERSONA.pers_vEstatura;
            txtPeso.Text = objActuacion.PERSONA.pers_sPeso.ToString();
            if (objActuacion.PERSONA.pers_sColorTezId > 0)
                ddlColorTez.SelectedValue = objActuacion.PERSONA.pers_sColorTezId.ToString();
            else
                ddlColorTez.SelectedIndex = 0;
            if (objActuacion.PERSONA.pers_sColorOjosId > 0)
                ddlColorOjos.SelectedValue = objActuacion.PERSONA.pers_sColorOjosId.ToString();
            else
                ddlColorOjos.SelectedIndex = 0;
            if (objActuacion.PERSONA.pers_sGrupoSanguineoId > 0)
                ddlGrupoSanguineo.SelectedValue = objActuacion.PERSONA.pers_sGrupoSanguineoId.ToString();
            else
                ddlGrupoSanguineo.SelectedIndex = 0;
            txtSeniasPart.Text = objActuacion.PERSONA.pers_vSenasParticulares;
            // LUGAR DE NACIMIENTO
            if (objActuacion.PERSONA.pers_sOcurrenciaTipoId > 0)
                ddlNacLugarTipo.SelectedValue = objActuacion.PERSONA.pers_sOcurrenciaTipoId.ToString();
            else
                ddlNacLugarTipo.SelectedIndex = 0;
            txtNacLugar.Text = objActuacion.PERSONA.pers_vLugarNacimiento;
            if (objActuacion.PERSONA.pers_cNacimientoLugar != string.Empty)
                this.ctrlUbigeo_Nacimiento.setUbigeo(objActuacion.PERSONA.pers_cNacimientoLugar.ToString());

            // RESIDENCIA
            hifResidenciaId.Value = objActuacion.RESIDENCIA.resi_iResidenciaId.ToString();

            if (objActuacion.RESIDENCIA.resi_vResidenciaDireccion != string.Empty)
                txtDireccion.Text = objActuacion.RESIDENCIA.resi_vResidenciaDireccion;

            hifResidenciaCodigoPostal.Value = objActuacion.RESIDENCIA.resi_vCodigoPostal;

            if (objActuacion.RESIDENCIA.resi_vResidenciaTelefono != string.Empty)
                txtDomicilioTelef.Text = objActuacion.RESIDENCIA.resi_vResidenciaTelefono;

            if (objActuacion.RESIDENCIA.resi_cResidenciaUbigeo != string.Empty)
                this.ctrlUbigeoLineal_Domicilio.setUbigeo(objActuacion.RESIDENCIA.resi_cResidenciaUbigeo.ToString());


            List<BE.RE_PARTICIPANTE> loPartcipantes = objActuacion.PARTICIPANTE_Container;
            // SOLO PARA REGISTRO ...
            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.REGISTRO) {
                RE_PARTICIPANTE objParticipante = new RE_PARTICIPANTE();

                //if (HFGUID.Value.Length > 0)
                //{
                //    objParticipante.iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                objParticipante.iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}
                objParticipante.iActuacionDetId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                objParticipante.sTipoParticipanteId = (int)Enumerador.enmTipoParticipanteMilitar.TITULAR;
                objParticipante.vTipoParticipante = Enumerador.enmTipoParticipanteMilitar.TITULAR.ToString();
                objParticipante.sTipoPersonaId = (int)Enumerador.enmTipoPersona.NATURAL;
                objParticipante.sTipoDocumentoId = (int)Enumerador.enmTipoDocumento.DNI;
                objParticipante.vTipoDocumento = Enumerador.enmTipoDocumento.DNI.ToString();
                objParticipante.sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;

                //if (HFGUID.Value.Length > 0)
                //{
                //    if (Session["NroDoc" + HFGUID.Value] != null)
                //        objParticipante.vNumeroDocumento = Session["NroDoc" + HFGUID.Value].ToString();
                //    if (Session["ApePat" + HFGUID.Value] != null)
                //        objParticipante.vPrimerApellido = Session["ApePat" + HFGUID.Value].ToString();
                //    if (Session["ApeMat" + HFGUID.Value] != null)
                //        objParticipante.vSegundoApellido = Session["ApeMat" + HFGUID.Value].ToString();
                //    if (Session["Nombres" + HFGUID.Value] != null)
                //    {
                //        if (Session["Nombres" + HFGUID.Value].ToString().Trim() != string.Empty)
                //            objParticipante.vNombres = Session["Nombres" + HFGUID.Value].ToString();
                //    }
                //}
                //else
                //{
                if (ViewState["NroDoc"] != null)
                    objParticipante.vNumeroDocumento = ViewState["NroDoc"].ToString();
                if (ViewState["ApePat"] != null)
                        objParticipante.vPrimerApellido = ViewState["ApePat"].ToString();
                    if (ViewState["ApeMat"] != null)
                        objParticipante.vSegundoApellido = ViewState["ApeMat"].ToString();
                    if (ViewState["Nombre"] != null)
                    {
                        if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                            objParticipante.vNombres = ViewState["Nombre"].ToString();
                    }
                //}

                //objParticipante.pers_dNacimientoFecha =
                loPartcipantes.Add(objParticipante);
            }
            Session["Participante"] = (List<BE.RE_PARTICIPANTE>)objActuacion.PARTICIPANTE_Container;

            this.Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            this.Grd_Participantes.DataBind();
        }

        private void CargarDatosActuacionDetalle()
        {
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;

            #region Adjuntos
            ctrlAdjunto.Grd_Archivos.DataSource = null;
            ctrlAdjunto.Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
            Proceso MiProc = new Proceso();

            object[] miArray = { lngActuacionDetalleId,
						"1",
						intPaginaCantidad, 
						IntTotalCount, IntTotalPages };

            dtAdjuntos = (DataTable)MiProc.Invocar(ref miArray,
                                                   "SGAC.BE.RE_ACTUACIONADJUNTO",
                                                   Enumerador.enmAccion.OBTENER);

            if (dtAdjuntos.Rows.Count > 0)
            {
                ctrlAdjunto.Grd_Archivos.DataSource = dtAdjuntos;
                ctrlAdjunto.Grd_Archivos.DataBind();
            }
            #endregion
            #region Vinculación Insumo
            Grd_ActInsDet.DataSource = null;
            Grd_ActInsDet.DataBind();

            ActuacionMantenimientoBL objActuacionMantenimientoBL = new ActuacionMantenimientoBL();
            DataTable dtActuacionInsumoDetalle = new DataTable();

            int PaginaActual = CtrlPageBarActuacionInsumoDetalle.PaginaActual;
            dtActuacionInsumoDetalle = objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(lngActuacionDetalleId, PaginaActual, intPaginaCantidad, ref IntTotalCount, ref  IntTotalPages); // objActuacionMantenimientoBL.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ctrlPaginadorAct.PaginaActual.ToString(), intPaginaCantidad, IntTotalCount, IntTotalPages);

            Session["bTramiteActuacion"] = null;

            if (dtActuacionInsumoDetalle.Rows.Count > 0)
            {
                Session["bTramiteActuacion"] = true;

                btnVistaPrev.Enabled = false;

                txtCodAutoadhesivo.Text = dtActuacionInsumoDetalle.Rows[0]["insu_vCodigoUnicoFabrica"].ToString();

                Session["COD_AUTOADHESIVO"] = txtCodAutoadhesivo.Text.Trim();

                ctrlAdjunto.SetCodigoVinculacion(txtCodAutoadhesivo.Text);
                string strFlag = dtActuacionInsumoDetalle.Rows[0]["aide_bFlagImpresion"].ToString();
                Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + HFGUID.Value] = dtActuacionInsumoDetalle.Rows[0]["aide_iActuacionInsumoDetalleId"].ToString();
                hCodAutoadhesivo.Value = dtActuacionInsumoDetalle.Rows[0]["insu_iInsumoId"].ToString();

                //Jonatan -- 20/07/2017 
                if (txtCodAutoadhesivo.Text.Length > 0)
                {
                    ctrlBajaAutoadhesivo1.Activar = true;
                }
                else
                {
                    ctrlBajaAutoadhesivo1.Activar = false;
                }
                if (strFlag.Equals("SI"))
                {
                    chkImpresion.Checked = true;
                    chkImpresion.Enabled = false;
                    btnVistaPrev.Enabled = false;
                    hdn_ImpresionCorrecta.Value = "1";
                    txtCodAutoadhesivo.Enabled = false;
                }
                else
                {
                    if (txtCodAutoadhesivo.Text.Trim() != String.Empty)
                    {
                        txtCodAutoadhesivo.Enabled = false;
                    }
                    else {
                        txtCodAutoadhesivo.Enabled = true;
                    }
                    btnVistaPrev.Enabled = true;
                    chkImpresion.Checked = false;
                    btnVistaPrev.Enabled = true;
                    hdn_ImpresionCorrecta.Value = "";
                }
                // Jonatan -- 20/07/2017
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    ctrlReimprimirbtn1.Activar = false;
                    ctrlBajaAutoadhesivo1.Activar = false;
                }
                else {
                    string valor = Convert.ToString(Request.QueryString["cod"]);
                    if (valor == "1")
                    {
                        ctrlReimprimirbtn1.Activar = false;
                    }
                    else
                    {
                        ctrlReimprimirbtn1.Activar = chkImpresion.Checked;
                    }
                }
               
                btnGrabarVinculacion.Enabled = false;
                Grd_ActInsDet.DataSource = dtActuacionInsumoDetalle;
                Grd_ActInsDet.DataBind();

                CtrlPageBarActuacionInsumoDetalle.TotalResgistros = IntTotalCount;
                CtrlPageBarActuacionInsumoDetalle.TotalPaginas = IntTotalPages;

                CtrlPageBarActuacionInsumoDetalle.Visible = false;
                if (CtrlPageBarActuacionInsumoDetalle.TotalResgistros > intPaginaCantidad)
                {
                    CtrlPageBarActuacionInsumoDetalle.Visible = true;
                }
                dtActuacionInsumoDetalle = null;
            }
            else
            {
                txtCodAutoadhesivo.Text = "";
                txtCodAutoadhesivo.Focus();
                txtCodAutoadhesivo.Enabled = true;
                ctrlBajaAutoadhesivo1.Activar = false;
                txtCodAutoadhesivo.Enabled = true;
                btnGrabarVinculacion.Enabled = true;
                btnVistaPrev.Enabled = false;
            }
            #endregion            
        }

        private void ActualizarActuacionDetalle()
        {
            string StrScript = string.Empty;
            long lngActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            if (lngActuacionDetalleId > 0)
            {
                BE.RE_ACTUACION ObjActuacBE = new RE_ACTUACION();
                BE.RE_ACTUACIONDETALLE ObjActuacDetBE = new RE_ACTUACIONDETALLE();

                ObjActuacDetBE.acde_iActuacionDetalleId = lngActuacionDetalleId;
                ObjActuacDetBE.acde_vNotas = txtObservaciones.Text.ToUpper().Replace("'", "''");
                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                ObjActuacBE.actu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjActuacBE.actu_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                ActuacionMantenimientoBL objBL = new ActuacionMantenimientoBL();
                int IntRpta = objBL.Actualizar(ObjActuacBE, ObjActuacDetBE);
                if (IntRpta > 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Actuaciones", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuaciones", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
            }
        }


        #region
        public void HabilitaControl(bool bHabilitado = true)
        {
            this.ddl_TipoParticipante.Enabled = bHabilitado;
            this.ddl_TipoDocParticipante.Enabled = bHabilitado;

            this.txtNroDocParticipante.Enabled = bHabilitado;
            this.ddl_NacParticipante.Enabled = bHabilitado;
            this.txtNomParticipante.Enabled = bHabilitado;
            this.txtApePatParticipante.Enabled = bHabilitado;
            this.txtApeMatParticipante.Enabled = bHabilitado;
            this.txtDireccionParticipante.Enabled = bHabilitado;
            this.txtObservacionesAP.Enabled = bHabilitado;
            this.btnAceptar.Enabled = bHabilitado;
            this.btnCancelar.Enabled = bHabilitado;
            this.Grd_Participantes.Enabled = bHabilitado;

            this.ctrlUbigeo1.HabilitaControl(true);
        }

        private DataTable CrearTablaParticipante()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iActuacionParticipanteId", typeof(string));
            dt.Columns.Add("iPersonaId", typeof(string));
            dt.Columns.Add("vApellidoPaterno", typeof(string));
            dt.Columns.Add("vApellidoMaterno", typeof(string));
            dt.Columns.Add("vNombres", typeof(string));
            dt.Columns.Add("sTipoParticipanteId", typeof(string));
            dt.Columns.Add("vTipoParticipante", typeof(string));
            dt.Columns.Add("sTipoDatoId", typeof(string));
            dt.Columns.Add("sTipoVinculoId", typeof(string));
            dt.Columns.Add("sDocumentoTipoId", typeof(string));
            dt.Columns.Add("vDocumentoTipo", typeof(string)); //IDM-CREADO
            dt.Columns.Add("vDocumentoNumero", typeof(string));
            dt.Columns.Add("vDocumentoCompleto", typeof(string));
            dt.Columns.Add("sNacionalidadId", typeof(string));
            dt.Columns.Add("vResidenciaDireccion", typeof(string));
            dt.Columns.Add("cResidenciaUbigeo", typeof(string));
            dt.Columns.Add("ICentroPobladoId", typeof(string));
            dt.Columns.Add("cEstado", typeof(string));
            dt.Columns.Add("vNombreCompleto", typeof(string));
            dt.Columns.Add("iItemRow", typeof(int));
            return dt;
        }

        protected void Grd_Participantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Tomando de la variable de session//
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //
            int lRowIndex = Convert.ToInt32(e.CommandArgument);

            short intIndiceParticipante = ObtenerIndiceListaParticipante(lRowIndex);

            if (e.CommandName == "Editar")
            {

                if (loParticipanteContainer != null)
                {
                    if (loParticipanteContainer.Count > 0)
                    {
                        RE_PARTICIPANTE loParticipante = loParticipanteContainer[intIndiceParticipante];

                        if (loParticipante.iParticipanteId > 0)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No se puede editar participante porque ya ha sido grabado en RUNE."));
                            return;
                        }


                            //if (loParticipante.sTipoParticipanteId > 0)
                         ddl_TipoParticipante.SelectedValue=Convert.ToInt16(loParticipante.sTipoParticipanteId).ToString();

                        if (loParticipante.sTipoDocumentoId > 0)
                            ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(loParticipante.sTipoDocumentoId).ToString();
                        else
                            ddl_TipoDocParticipante.SelectedIndex = 0;
                       
                        if (loParticipante.sNacionalidadId > 0)
                            ddl_NacParticipante.SelectedValue = Convert.ToInt16(loParticipante.sNacionalidadId).ToString();
                        else
                            ddl_NacParticipante.SelectedIndex = 0;


                        txtNroDocParticipante.Text = loParticipante.vNumeroDocumento;
                        txtNomParticipante.Text = loParticipante.vNombres;
                        txtApePatParticipante.Text = loParticipante.vPrimerApellido;
                        txtApeMatParticipante.Text = loParticipante.vSegundoApellido;
                        txtDireccionParticipante.Text = loParticipante.vDireccion;
                        
                        
                        this.ctrlUbigeo1.setUbigeo(loParticipante.vUbigeo);
                        this.ctrlUbigeo1.HabilitaControl(true);

                        edicion = true;
                        edicion_rowIndex = lRowIndex;

                        this.btnAceptar.Enabled = true;
                        this.btnCancelar.Enabled = true;
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "No existe participante"));
                    }
                }
            }
            if (e.CommandName == "Eliminar")
            {

                

                loParticipanteContainer[intIndiceParticipante].cEstado = "E";

                if (!(loParticipanteContainer[intIndiceParticipante].iParticipanteId > 0))
                {
                    loParticipanteContainer.Remove(loParticipanteContainer[intIndiceParticipante]);
                }

              

                edicion = false;
                edicion_rowIndex = -1;
                LimpiarDatosParticipante();
            }

            Session["Participante"] = (List<BE.RE_PARTICIPANTE>)loParticipanteContainer;

            this.Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            this.Grd_Participantes.DataBind();

            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarDatosParticipante();

            edicion = false;
            edicion_rowIndex = -1;

            //this.btnCancelar.Enabled = false;
            //this.btnAceptar.Enabled = true;
        }

        private short ObtenerIndiceListaParticipante(int iRowIndex)
        {

            if (iRowIndex<0)
                return -1;

            string vNumeroDocumento = Grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(Grd_Participantes, "vDocumentoNumero")].Text;

            short sDocumentoTipoId = Int16.Parse(Grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(Grd_Participantes, "sDocumentoTipoId")].Text);
            short sTipoParticipanteId = Int16.Parse(Grd_Participantes.Rows[iRowIndex].Cells[ObtenerIndiceColumnaGrilla(Grd_Participantes, "sTipoParticipanteId")].Text);

            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];

            var auxParticipanteContainerItem = loParticipanteContainer.Where(
                x => x.sTipoDocumentoId == sDocumentoTipoId &&
                    x.sTipoParticipanteId == sTipoParticipanteId &&
                 x.vNumeroDocumento.Trim() == vNumeroDocumento.Trim());


            if (auxParticipanteContainerItem.Count() == 1)
            {
                return (short)loParticipanteContainer.IndexOf(auxParticipanteContainerItem.ElementAt(0));
            }

            return -1;
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Tomando de la variable de SESSION
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //

            if ((loParticipanteContainer.Count == 0) && (edicion_rowIndex <= 0))
            {
                edicion_rowIndex = -1;
            }

            short IndiceLista = ObtenerIndiceListaParticipante(edicion_rowIndex);

            string vNumeroDocumento = txtNroDocParticipante.Text;
            Int16 iTipoParticipante = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
            Int16 iTipoDocumento = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);


            if (iTipoDocumento == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI) && vNumeroDocumento.Length != 8)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "El DNI debe poseer 8 caracteres."));
                return;
            }


            bool bError = false;

            foreach (BE.RE_PARTICIPANTE reParticipante in loParticipanteContainer.Where(x => x.cEstado != "E").ToList())
            {

                if (edicion && loParticipanteContainer.IndexOf(reParticipante) == IndiceLista)
                    continue;


                if (reParticipante.sTipoDocumentoId == iTipoDocumento && reParticipante.vNumeroDocumento == vNumeroDocumento)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Ya existe participante con el tipo y número de documento seleccionado."));
                    bError = true;
                }
                else if (reParticipante.sTipoParticipanteId == iTipoParticipante)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "PARTICIPANTE", "Ya existe participante con el tipo seleccionado."));
                    bError = true;
                }

                if (bError)
                {
                    edicion_rowIndex = -1;
                    edicion = false;
                    LimpiarDatosParticipante();
                    return;
                }


               
            }




            if ((edicion) && (IndiceLista != -1))
            {
                // SI ES EDICION DE DATO ....

                loParticipanteContainer[IndiceLista].sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
                loParticipanteContainer[IndiceLista].vTipoParticipante = ddl_TipoParticipante.SelectedItem.Text.ToString();
                loParticipanteContainer[IndiceLista].sTipoVinculoId = 0;
                loParticipanteContainer[IndiceLista].vNumeroDocumento = txtNroDocParticipante.Text.ToString().ToUpper();
                loParticipanteContainer[IndiceLista].sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
                loParticipanteContainer[IndiceLista].vNombres = txtNomParticipante.Text.ToString().ToUpper();
                loParticipanteContainer[IndiceLista].vPrimerApellido = txtApePatParticipante.Text.ToString().ToUpper();
                loParticipanteContainer[IndiceLista].vSegundoApellido = txtApeMatParticipante.Text.ToString().ToUpper();
                loParticipanteContainer[IndiceLista].vDireccion = txtDireccionParticipante.Text.ToString().ToUpper();
                if (this.ctrlUbigeo1.getResidenciaUbigeo() != null) { loParticipanteContainer[IndiceLista].vUbigeo = (string)this.ctrlUbigeo1.getResidenciaUbigeo(); }
                if (this.ctrlUbigeo1.getCentroPobladoId() != null) 
                { loParticipanteContainer[IndiceLista].ICentroPobladoId = (int)this.ctrlUbigeo1.getCentroPobladoId(); }
                loParticipanteContainer[IndiceLista].cEstado = "A";
                if (txtFecNac.Text.Length > 0)
                {
                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(txtFecNac.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(txtFecNac.Text);
                    }

                    loParticipanteContainer[IndiceLista].pers_dNacimientoFecha = datFecha;
                }
                else
                {
                    loParticipanteContainer[IndiceLista].pers_dNacimientoFecha = null;
                }
                
            }
            else
            {
                RE_PARTICIPANTE loParticipante = new RE_PARTICIPANTE();
                #region Creando objecto PARTICIPANTE

                loParticipante.sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
                loParticipante.vTipoParticipante = ddl_TipoParticipante.SelectedItem.Text.ToString();
                if (txtPersonaId.Text.ToString() != "") { loParticipante.iPersonaId = Convert.ToInt64(txtPersonaId.Text.ToString()); }
                loParticipante.sTipoDocumentoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
                loParticipante.vTipoDocumento = ddl_TipoDocParticipante.SelectedItem.Text.ToUpper();// IDM-CREADO
                loParticipante.vNumeroDocumento = txtNroDocParticipante.Text.ToString().ToUpper();
                loParticipante.sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
                loParticipante.vNombres = txtNomParticipante.Text.ToString().ToUpper();
                loParticipante.vPrimerApellido = txtApePatParticipante.Text.ToString().ToUpper();
                loParticipante.vSegundoApellido = txtApeMatParticipante.Text.ToString().ToUpper();
                loParticipante.vDireccion = txtDireccionParticipante.Text.ToString().ToUpper();
                if (this.ctrlUbigeo1.getResidenciaUbigeo() != null) { loParticipante.vUbigeo = (string)this.ctrlUbigeo1.getResidenciaUbigeo(); }
                if (this.ctrlUbigeo1.getCentroPobladoId() != null) { loParticipante.ICentroPobladoId = (int)this.ctrlUbigeo1.getCentroPobladoId(); }


                loParticipante.cEstado = "A";
                loParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                loParticipante.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                loParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                #endregion
                if (!ExistTipoParticipante(Convert.ToInt16(ddl_TipoParticipante.SelectedValue)))
                {
                    loParticipanteContainer.Add(loParticipante);
                }
            }

            Session["Participante"] = (List<BE.RE_PARTICIPANTE>)loParticipanteContainer;

            Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            Grd_Participantes.DataBind();

            edicion = false;
            edicion_rowIndex = -1;
            LimpiarDatosParticipante();

            
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = Convert.ToInt32(ddl_TipoDocParticipante.SelectedValue);
            //if (ddl_TipoDatoParticipante.SelectedItem != null)
            //{
            //    objEn.vDocumentoTipo = ddl_TipoDatoParticipante.SelectedItem.Text;
            //}
            objEn.vDocumentoNumero = txtNroDocParticipante.Text;
            //if (ddl_TipoVinculoParticipante.SelectedIndex > 0)
            //{
            //    objEn.sTipoVinculoId = Convert.ToInt32(ddl_TipoVinculoParticipante.SelectedValue);
            //}
            //if (ddl_TipoDatoParticipante.SelectedIndex > 0)
            //{
            //    objEn.sTipoDatoId = Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue);
            //}
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
            #endregion

            #region Pintar Datos Persona
            string strScript = string.Empty;
            txtPersonaId.Text = "0";
            if (objEn != null)
            {
                



                if (objEn.iPersonaId>0)
                {
                    txtPersonaId.Text = objEn.iPersonaId.ToString();
                    ddl_NacParticipante.SelectedValue = objEn.sNacionalidadId.ToString();

                    ddl_TipoDocParticipante.Enabled = false;

                    txtNomParticipante.Text = objEn.vNombres;
                    txtApePatParticipante.Text = objEn.vApellidoPaterno;
                    txtApeMatParticipante.Text = objEn.vApellidoMaterno;
                    txtDireccionParticipante.Text = ""; // IDM-PENDIENTE

                    txtNomParticipante.Enabled = false;
                    txtApeMatParticipante.Enabled = false;
                    txtApePatParticipante.Enabled = false;
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,
                        "PARTICIPANTE", "El número de documento no esta registrado en el sistema.");
                    Comun.EjecutarScript(Page, strScript);
                    txtNomParticipante.Enabled = true;
                    txtApeMatParticipante.Enabled = true;
                    txtApePatParticipante.Enabled = true;
                }

                if (!string.IsNullOrEmpty(objEn.vDireccion))
                {
                    txtDireccionParticipante.Text = objEn.vDireccion;
                    txtDireccionParticipante.Enabled = false;
                }
                else
                {
                    txtDireccionParticipante.Enabled = true;
                }                   



                string vUbigeo = objEn.iDptoContId.ToString().PadLeft(2, '0') +
                    objEn.iProvPaisId.ToString().PadLeft(2, '0') +
                    objEn.iDistCiuId.ToString().PadLeft(2, '0');

                if (!(vUbigeo.Length != 6 || vUbigeo == "000000"))
                {
                    ctrlUbigeo1.setUbigeo(vUbigeo);
                    ctrlUbigeo1.HabilitaControl(false);
                }
                else
                {
                    ctrlUbigeo1.ClearControl();
                    ctrlUbigeo1.HabilitaControl(true);
                }               
               

            }
            #endregion
        }

        public static bool IsVisible(object objeto)
        {
            if (objeto.ToString().Equals("TITULAR"))
                return false;
            else return true;
        }
        private void LimpiarDatosParticipante()
        {
            this.ddl_TipoParticipante.SelectedIndex = 0;
            //if (this.ddl_TipoDatoParticipante.SelectedIndex != -1) 
            //{ 
            //    this.ddl_TipoDatoParticipante.SelectedIndex = 0; 
            //};
            //this.ddl_TipoVinculoParticipante.SelectedIndex = 0;
            this.ddl_TipoDocParticipante.Enabled = true;
            this.ddl_TipoDocParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.Enabled = true;
            this.txtNroDocParticipante.Text = string.Empty;
            this.txtNomParticipante.Text = string.Empty;
            this.txtNomParticipante.Enabled = true;
            this.txtApePatParticipante.Text = string.Empty;
            this.txtApePatParticipante.Enabled = true;
            this.txtApeMatParticipante.Text = string.Empty;
            this.txtApeMatParticipante.Enabled = true;
            this.txtDireccionParticipante.Text = string.Empty;
            this.txtDireccionParticipante.Enabled = true;
            this.txtPersonaId.Text = string.Empty;
            this.ctrlUbigeo1.ClearControl();
            this.ctrlUbigeo1.HabilitaControl(true);

        }

        private DataTable mtParticipanteContainerToTable()
        {
            DataTable dt = CrearTablaParticipante();
            int ItemRow = 1;

            //Tomando de la variable de SESSION
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //
            #region Creando DataTable
            foreach (RE_PARTICIPANTE item in loParticipanteContainer.Where(p => p.cEstado != "E"))
            {
                DataRow dr = dt.NewRow();
                dr["iItemRow"] = ItemRow++;
                dr["iActuacionParticipanteId"] = item.iParticipanteId;
                dr["iPersonaId"] = item.iPersonaId;
                dr["vApellidoPaterno"] = item.vPrimerApellido;
                dr["vApellidoMaterno"] = item.vSegundoApellido;
                dr["vNombres"] = item.vNombres;
                dr["sTipoParticipanteId"] = item.sTipoParticipanteId;
                dr["vTipoParticipante"] = item.vTipoParticipante; //ddl_TipoParticipante.SelectedItem.Text.ToString(); //item.sTipoParticipanteId;
                dr["sTipoDatoId"] = item.sTipoDatoId;
                dr["sTipoVinculoId"] = item.sTipoVinculoId;
                dr["sDocumentoTipoId"] = item.sTipoDocumentoId;
                dr["vDocumentoTipo"] = item.vTipoDocumento; // IDM-CREADO
                dr["vDocumentoNumero"] = item.vNumeroDocumento;
                dr["vDocumentoCompleto"] = item.vTipoDocumento.ToString() + " - " + item.vNumeroDocumento.ToString();
                dr["sNacionalidadId"] = item.sNacionalidadId;
                dr["vResidenciaDireccion"] = item.vDireccion;
                dr["cResidenciaUbigeo"] = item.vUbigeo;
                dr["ICentroPobladoId"] = item.ICentroPobladoId;
                dr["cEstado"] = item.cEstado;
                dr["vNombreCompleto"] = item.vPrimerApellido + " " + item.vSegundoApellido + "," + item.vNombres;

                dt.Rows.Add(dr);
            }
            #endregion
            return dt;
        }

        private void mtParticipanteInitialize()
        {
            this.btnAceptar.OnClientClick = "return ActoMilitar_Participantes()";
            DataTable dtTipDoc = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
            DataView dv = dtTipDoc.DefaultView;
            DataTable dtOrdenado = dv.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";
            Util.CargarDropDownList(ddl_TipoDocParticipante, dtOrdenado, "Valor", "Id", true);

            int lDatoParticipante = Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]);
            if (lDatoParticipante == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO), true);
                //ddl_TipoDatoParticipante.Visible = true;
                //lblPartTipoVinc.Visible = true;
                /////<SUMARY>
                /////Para nacimiento el titular (recien nacido) usa como DNI el CUI 
                /////por tal motivo no se ingresara Tipo Documento ni el Nro.Documento
                /////</SUMARY>
                //this.ddl_TipoDocParticipante.Enabled = false;
                //this.txtNroDocParticipante.Enabled = false;


            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO), true);
                //ddl_TipoDatoParticipante.Visible = false;
                //lblPartTipoVinc.Visible = false;
            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION), true);
                //ddl_TipoDatoParticipante.Visible = true;
                //lblPartTipoVinc.Visible = true;
            }
            else if (lDatoParticipante == Convert.ToInt32(Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE))
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE), true);
                //ddl_TipoDatoParticipante.Visible = false;
                //lblPartTipoVinc.Visible = false;

                // Cargar Titular por defecto - MD
                //List<RE_PARTICIPANTE> lstParticipantes = new List<RE_PARTICIPANTE>();
                //RE_PARTICIPANTE objParticipante = new RE_PARTICIPANTE();
                //objParticipante.iPersonaId = Convert.ToInt64(Session["iPersonaId"]);
                //objParticipante.iActuacionDetId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                //objParticipante.sTipoParticipanteId = (int)Enumerador.enmTipoParticipanteMilitar.TITULAR;
                //objParticipante.vTipoParticipante = Enumerador.enmTipoParticipanteMilitar.TITULAR.ToString();
                //objParticipante.sTipoPersonaId = (int)Enumerador.enmTipoPersona.NATURAL;
                //objParticipante.sTipoDocumentoId = (int)Enumerador.enmTipoDocumento.DNI;
                //objParticipante.vTipoDocumento = Enumerador.enmTipoDocumento.DNI.ToString();
                //objParticipante.sNacionalidadId = (int)Enumerador.enmNacionalidad.PERUANA;
                //if (Session["NroDoc"] != null)
                //    objParticipante.vNumeroDocumento = Session["NroDoc"].ToString();
                //if (Session["ApePat"] != null)
                //    objParticipante.vPrimerApellido = Session["ApePat"].ToString();
                //if (Session["ApeMat"] != null)
                //    objParticipante.vSegundoApellido = Session["ApeMat"].ToString();
                //if (Session["Nombres"] != null)
                //{
                //    if (Session["Nombres"].ToString().Trim() != string.Empty)
                //        objParticipante.vNombres = Session["Nombres"].ToString();
                //}

                //lstParticipantes.Add(objParticipante);

                //SetTablaPorLista(lstParticipantes);
                //--
            }

            //Util.CargarParametroDropDownList(ddl_TipoDatoParticipante, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_CIVIL_PARTICIPANTE_TIPO_DATO), true);
            //Util.CargarParametroDropDownList(ddl_TipoVinculoParticipante, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);
            Util.CargarParametroDropDownList(ddl_NacParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
            ctrlUbigeo1.UbigeoRefresh();
        }

        private bool ExistTipoParticipante(Int16 tp)
        {
            //Tomando de la variable de session//
            List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
            //
            bool lReturn = false;
            foreach (RE_PARTICIPANTE p in loParticipanteContainer.Where(p => p.cEstado != "E"))
            {
                if (p.sTipoParticipanteId == tp)
                {
                    lReturn = true;
                }
            }
            return lReturn;
        }
        #endregion



        protected void ddl_TipoDocParticipante_SelectedIndexChanged(object sender, EventArgs e)
        {



            txtNroDocParticipante.Text = string.Empty;
            txtApePatParticipante.Enabled = true;
            txtApeMatParticipante.Enabled = true;
            txtNomParticipante.Enabled = true;
            ddl_NacParticipante.Enabled = true;



            if (ddl_TipoDocParticipante.SelectedValue != "0")
            {
                DataTable dtDocumentoIdentidad = new DataTable();

                dtDocumentoIdentidad = Comun.ObtenerListaDocumentoIdentidad();

                //DataTable dtDocumentoIdentidad= (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];


                foreach (DataRow fila in dtDocumentoIdentidad.Rows)
                {
                    if(fila["doid_sTipoDocumentoIdentidadId"].ToString()==ddl_TipoDocParticipante.SelectedValue.ToString())
                    {
                        int iMaxLenght= 0;
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

                       

                        txtNroDocParticipante.MaxLength = iMaxLenght;

                        if (sNacionalidad > 0)
                        {
                            ddl_NacParticipante.SelectedValue = sNacionalidad.ToString();
                            ddl_NacParticipante.Enabled = false;
                        }
                        else
                        {
                            ddl_NacParticipante.SelectedValue = "0";
                            ddl_NacParticipante.Enabled = true;
                        }

                        break;
                    }
                    
                }


            }
            else
            {
                txtNroDocParticipante.MaxLength = 8;
                hidDocumentoSoloNumero.Value =  "0";
            }

            updRegPago.Update();
        }

        [System.Web.Services.WebMethod]
        public static string GetPersonExist(Int32 tipo, String documento)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = tipo;
            objEn.vDocumentoNumero = documento;

            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
            #endregion

            string person = new JavaScriptSerializer().Serialize(objEn);

            return person;
        }

        protected void txtCodAutoadhesivo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnImpresionConstanciaVP_Click(object sender, EventArgs e)
        {
            Load_Imagenes(); 

            IntPreviaImpresion = 1;
            hifVistaPrevia.Value = IntPreviaImpresion.ToString();


            if (EsFechaValida())
            {
                ctrlToolbarFormato_btnGrabar();
            }
            else
            {
                return;
            }


            ImprimirConstancia();
            
 
        }

        void ImprimirConstancia()
        {

            object[] arrParametros = new object[2];
            Proceso p = new Proceso();

            try
            {
                long i_ID = 0;
                long.TryParse(Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), out i_ID);

                DataTable dt = new Reportes.dsActoMilitar().Tables["RM_CONSTANCIA"];

                arrParametros[0] = i_ID;
                arrParametros[1] = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID];
                dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_REGISTROMILITAR", "CONSULTAR_CONSTANCIA");

                if (dt.Rows.Count > 0)
                {
                    CrearConstanciaMilitariTextSharp(this.Page, dt);
                    updToolFormato.Update();
                }
                else
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,
                                    "REGISTRO MILITAR", "Guarde primero la información para ver el reporte", false, 200, 300);
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ACTUACIÓN", "Se presentaron problemas al generar formato.")); //+ ex.Message.Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("'", string.Empty)));
            }
        }

        protected void btnImpresionRegistroVP_Click(object sender, EventArgs e)
        {
            IntPreviaImpresion = 1;

            hifVistaPrevia.Value = IntPreviaImpresion.ToString();

            if (EsFechaValida())
            {
                ctrlToolbarFormato_btnGrabar();
            }
            else
            {
                return;
            }

            object[] arrParametros = new object[1];
            Proceso p = new Proceso();

            try
            {
                long i_ID = 0;
                long.TryParse(Convert.ToString(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), out i_ID);

                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int idTipoAdjunto;

                //DataTable dt = new Reportes.dsActoMilitar().Tables["RM_INSCRIPCION_FOTOS"];
                //arrParametros[0] = i_ID;
                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_REGISTROMILITAR", "CONSULTAR_HOJA_INSCRIPCION");

                DataTable dt = new DataTable();

                dt = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtener(
                     Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), "1",
                     Constantes.CONST_PAGE_SIZE_ADJUNTOS, ref IntTotalCount, ref IntTotalPages);

                string strimgFotoDeFrente = "";
                string strimgFotoPerfil = "";
                string strimgHuellaIDerecha = "";
                string strimgHuellaIzquierda = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    idTipoAdjunto = Convert.ToInt32(dt.Rows[i]["sAdjuntoTipoId"]);

                    if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.FOTO))
                    {
                        strimgFotoDeFrente = dt.Rows[i]["vNombreArchivo"].ToString();
                    }

                    if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.FOTO_PERFIL))
                    {
                        strimgFotoPerfil = dt.Rows[i]["vNombreArchivo"].ToString();
                    }

                    if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.HUELLA_INDICE_DERECHO))
                    {
                        strimgHuellaIDerecha = dt.Rows[i]["vNombreArchivo"].ToString();
                    }

                    if (idTipoAdjunto.Equals((Int32)Enumerador.enmTipoAdjunto.HUELLA_INDICE_IZQUIERDO))
                    {
                        strimgHuellaIzquierda = dt.Rows[i]["vNombreArchivo"].ToString();
                    }
                }

                arrParametros[0] = i_ID;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                dt = new Reportes.dsActoMilitar().Tables["RM_INSCRIPCION"];
                dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.RE_REGISTROMILITAR", "CONSULTAR_HOJA_INSCRIPCION");
                
                string strRutaimgFotoDeFrente = "";
                string strRutaimgFotoPerfil = "";
                string strRutaimgHuellaIDerecha = "";
                string strRutaimgHuellaIzquierda = "";

                if (strimgFotoDeFrente.Length > 0)
                {
                    strRutaimgFotoDeFrente = uploadPath + @"\" + strimgFotoDeFrente;
                    byte[] arrImagen = Util.IMG_CargarImagen(strRutaimgFotoDeFrente);
                    DataColumn column = new DataColumn("Imagen"); 
                    column.DataType = System.Type.GetType("System.Byte[]"); 
                    column.AllowDBNull = true;
                    column.Caption = "Imagen";
                    dt.Columns.Add(column);
                    dt.Rows[0]["Imagen"] = arrImagen;
                }

                if (strimgFotoPerfil.Length > 0)
                {
                    strRutaimgFotoPerfil = uploadPath + @"\" + strimgFotoPerfil;
                    byte[] arrImagenPerfil = Util.IMG_CargarImagen(strRutaimgFotoPerfil);
                    DataColumn columnPerfil = new DataColumn("ImagenPerfil"); 
                    columnPerfil.DataType = System.Type.GetType("System.Byte[]"); 
                    columnPerfil.AllowDBNull = true;
                    columnPerfil.Caption = "ImagenPerfil";
                    dt.Columns.Add(columnPerfil); 
                    dt.Rows[0]["ImagenPerfil"] = arrImagenPerfil;
                }

                if (strimgHuellaIDerecha.Length > 0)
                {
                    strRutaimgHuellaIDerecha = uploadPath + @"\" + strimgHuellaIDerecha;
                    byte[] arrImagenHuellaDere = Util.IMG_CargarImagen(strRutaimgHuellaIDerecha);
                    DataColumn columnHuellaDere = new DataColumn("ImagenHuellaDere"); 
                    columnHuellaDere.DataType = System.Type.GetType("System.Byte[]"); 
                    columnHuellaDere.AllowDBNull = true;
                    columnHuellaDere.Caption = "ImagenHuellaDere";
                    dt.Columns.Add(columnHuellaDere); 
                    dt.Rows[0]["ImagenHuellaDere"] = arrImagenHuellaDere;

                }

                if (strimgHuellaIzquierda.Length > 0)
                {
                    strRutaimgHuellaIzquierda = uploadPath + @"\" + strimgHuellaIzquierda;
                    byte[] arrImagenHuellaIzq = Util.IMG_CargarImagen(strRutaimgHuellaIzquierda);
                    DataColumn columnHuellaIzq = new DataColumn("ImagenHuellaIzq"); 
                    columnHuellaIzq.DataType = System.Type.GetType("System.Byte[]");
                    columnHuellaIzq.AllowDBNull = true;
                    columnHuellaIzq.Caption = "ImagenHuellaIzq";
                    dt.Columns.Add(columnHuellaIzq); 
                    dt.Rows[0]["ImagenHuellaIzq"] = arrImagenHuellaIzq;
                }

                //if (dt.Rows.Count > 0)
                //{
                    Microsoft.Reporting.WebForms.ReportViewer viewer = new Microsoft.Reporting.WebForms.ReportViewer();
                    Microsoft.Reporting.WebForms.ReportDataSource rptDataSource = new
                    Microsoft.Reporting.WebForms.ReportDataSource("dsActoMilitar", dt);


                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rptDataSource);
                    viewer.LocalReport.ReportEmbeddedResource = Server.MapPath("~/Reportes/rsActoMilitarInscripcion.rdlc");
                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/rsActoMilitarInscripcion.rdlc");

                    //Export to PDF
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string[] streams;
                    Microsoft.Reporting.WebForms.Warning[] warnings;

                    byte[] pdfContent = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    
                   // HttpContext.Current.Session["binaryData"] = pdfContent;

                    string strUrl = "../Accesorios/VisorPDF.aspx";
                    string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                    Comun.EjecutarScript(Page, strScript);

                    updToolFormato.Update();
                //}

                //else
                //{
                //    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING,
                //                    "REGISTRO MILITAR", "Guarde primero la información para ver el reporte", false, 200, 300);
                //    Comun.EjecutarScript(Page, strScript);
                //}
            }
            catch
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Se presentaron problemas al generar formato."));
            }
            
        }

        private bool EsFechaValida()
        {
            DateTime nacimiento = txtFecNac.Value();
            if (nacimiento == new DateTime(1900, 1, 1))
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Falta ingresar la fecha"));
                return false;
            }
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            if (edad >= IntEdadMinimaMilitar)
            {
                return true;
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ACTUACIÓN", "Edad no permitida. Connacional no tiene " + IntEdadMinimaMilitar + " años de edad"));
                return false;
            }

        }

        protected void chk_verificar_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_verificar.Checked)
                ctrlToolbarFormato.btnGrabar.Enabled = true;
            else
                ctrlToolbarFormato.btnGrabar.Enabled = false;

            updToolFormato.Update();
        }


        #region ActaConformidad
               

        [System.Web.Services.WebMethod]
        public static string Imprimir_Acta_Conformidad()
        {
            string strEtiquetaSolicitante = string.Empty;

            if (HttpContext.Current.Request.QueryString["GUID"] != null)
            {
                string strGUID = HttpContext.Current.Request.QueryString["GUID"].ToString();

                if (HttpContext.Current.Session["ApePat" + strGUID] != null)
                {
                    strEtiquetaSolicitante += HttpContext.Current.Session["ApePat" + strGUID].ToString() + " ";
                }

                if (HttpContext.Current.Session["ApeMat" + strGUID] != null)
                {
                    strEtiquetaSolicitante += HttpContext.Current.Session["ApeMat" + strGUID].ToString() + " ";
                }


                if (HttpContext.Current.Session["Nombres" + strGUID] != null)
                {
                    if (HttpContext.Current.Session["Nombres" + strGUID].ToString().Trim() != string.Empty)
                    {
                        strEtiquetaSolicitante += ", " + HttpContext.Current.Session["Nombres" + strGUID].ToString() + " ";
                    }
                }

            }
            else
            {
                if (HttpContext.Current.Session["ApePat"] != null)
                {
                    strEtiquetaSolicitante += HttpContext.Current.Session["ApePat"].ToString() + " ";
                }

                if (HttpContext.Current.Session["ApeMat"] != null)
                {
                    strEtiquetaSolicitante += HttpContext.Current.Session["ApeMat"].ToString() + " ";
                }


                if (HttpContext.Current.Session["Nombres"] != null)
                {
                    if (HttpContext.Current.Session["Nombres"].ToString().Trim() != string.Empty)
                    {
                        strEtiquetaSolicitante += ", " + HttpContext.Current.Session["Nombres"].ToString() + " ";
                    }
                }

            }

            string documento = string.Empty;
            string numero = string.Empty;

            if (HttpContext.Current.Request.QueryString["GUID"] != null)
            {
                string strGUID = HttpContext.Current.Request.QueryString["GUID"].ToString();

                if (HttpContext.Current.Session["DescTipDoc" + strGUID] != null)
                {
                    documento = HttpContext.Current.Session["DescTipDoc" + strGUID].ToString();
                }

                if (HttpContext.Current.Session["NroDoc" + strGUID] != null)
                {
                    numero = HttpContext.Current.Session["NroDoc" + strGUID].ToString();
                }
            }
            else
            {
                if (HttpContext.Current.Session["DescTipDoc"] != null)
                {
                    documento = HttpContext.Current.Session["DescTipDoc"].ToString();
                }

                if (HttpContext.Current.Session["NroDoc"] != null)
                {
                    numero = HttpContext.Current.Session["NroDoc"].ToString();
                }
            }



            DateTime dt_Fecha =Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));

            string script = string.Empty;

            string str_Fecha = dt_Fecha.ToString("dd") + " de " + dt_Fecha.ToString("MMMM") + " de " + dt_Fecha.ToString("yyyy");

            script = "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\"><input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\">DECLARACIÓN DE CONFORMIDAD DEL USUARIO</span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"justify\" style=\"line-height: 150%; \"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">Yo, " + strEtiquetaSolicitante +
                ", identificado con el " + documento + " N° " + numero + ", declaro que he leído y revisado el formato, que he tenido a la vista y me ha sido entregado en la fecha, manifestando mi conformidad con su contenido </span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"center\" style=\"margin-bottom:5px;\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"80%\"><tr><td width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">" + Convert.ToString(HttpContext.Current.Session["CiudadOficinaConsular"]) + ", " + str_Fecha + "</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"163px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"border-color: groove; border-width: groove; border-style: groove; height:145px;\"></td></tr><tr><td align=\"center\" width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\" font-size:10pt\">Huella Digital</span></font></td></tr></table></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
            script = script + "<div align=\"right\"><table border=\"0\" width=\"35%\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + strEtiquetaSolicitante + "</span></font></td></tr><tr><td align=\"center\" width=\"50%\" style=\"border-top-style: dashed; border-width: 2px\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + documento + " N° " + numero + "</span></font></td></tr></table></div>";

            return script;
        }

        protected void btn_confirmar_Click(object sender, EventArgs e)
        {

            #region Datos de Recurrente

            string vNombreRecurrente = string.Empty;
            string vDocumento = string.Empty;

            //if (HFGUID.Value.Length > 0)
            //{
            //    if (Session["Nombres" + HFGUID.Value] != null)
            //    {
            //        if (Session["Nombres" + HFGUID.Value].ToString().Trim() != string.Empty)
            //        {
            //            vNombreRecurrente += AplicarInicialMayuscula(Session["Nombres" + HFGUID.Value].ToString());
            //        }
            //    }

            //    if (Session["ApePat" + HFGUID.Value] != null)
            //    {
            //        if (Session["ApePat" + HFGUID.Value].ToString().Trim() != string.Empty)
            //        {
            //            vNombreRecurrente += " " + AplicarInicialMayuscula(Session["ApePat" + HFGUID.Value].ToString());
            //        }
            //    }

            //    if (Session["ApeMat" + HFGUID.Value] != null)
            //    {
            //        if (Session["ApeMat" + HFGUID.Value].ToString().Trim() != string.Empty)
            //        {
            //            vNombreRecurrente += " " + AplicarInicialMayuscula(Session["ApeMat" + HFGUID.Value].ToString());
            //        }
            //    }


            //    if (Session["DescTipDoc" + HFGUID.Value] != null)
            //    {
            //        if (Session["DescTipDoc" + HFGUID.Value].ToString().Trim() != string.Empty)
            //        {
            //            vDocumento += Session["DescTipDoc" + HFGUID.Value].ToString();
            //        }
            //    }

            //    if (Session["NroDoc" + HFGUID.Value] != null)
            //    {
            //        if (Session["NroDoc" + HFGUID.Value].ToString().Trim() != string.Empty)
            //        {
            //            vDocumento += " " + Session["NroDoc" + HFGUID.Value].ToString();
            //        }
            //    }

            //}
            //else
            //{

                if (ViewState["Nombre"] != null)
                {
                    if (ViewState["Nombre"].ToString().Trim() != string.Empty)
                    {
                        vNombreRecurrente += AplicarInicialMayuscula(ViewState["Nombre"].ToString());
                    }
                }

                if (ViewState["ApePat"] != null)
                {
                    if (ViewState["ApePat"].ToString().Trim() != string.Empty)
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApePat"].ToString());
                    }
                }

                if (ViewState["ApeMat"] != null)
                {
                    if (ViewState["ApeMat"].ToString().Trim() != string.Empty)
                    {
                        vNombreRecurrente += " " + AplicarInicialMayuscula(ViewState["ApeMat"].ToString());
                    }
                }


                if (ViewState["DescTipDoc"] != null)
                {
                    if (ViewState["DescTipDoc"].ToString().Trim() != string.Empty)
                    {
                        vDocumento += ViewState["DescTipDoc"].ToString();
                    }
                }

                if (ViewState["NroDoc"] != null)
                {
                    if (ViewState["NroDoc"].ToString().Trim() != string.Empty)
                    {
                        vDocumento += " " + ViewState["NroDoc"].ToString();
                    }
                }
            //}


            #endregion


            #region Formato
            StringBuilder sScript = new StringBuilder();
            sScript.Append("<br />");
            sScript.Append("<p align=\"center\">");
            sScript.Append("<h3><font face=\"arial\">DECLARACIÓN DE CONFORMIDAD DE USUARIO</font></h3>");
            sScript.Append("</p>");
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");

            StringBuilder sContenido = new StringBuilder();
            sContenido.Append(ObtenerDocumentoConformidad(vNombreRecurrente.ToUpper(), vDocumento.ToUpper()));
            //sContenido.Replace("</p>", "====</p>"); 

            sScript.Append(sContenido);
            #endregion

            #region Impresión

            DataTable dtTMPReemplazar = new DataTable();
            dtTMPReemplazar = CrearTmpTabla();

            string strRutaHtml = string.Empty;
            string strArchivoPDF = string.Empty;

            String localfilepath = String.Empty;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            string strRutaPDF = uploadPath + @"\" + "CuerpoHtml" + DateTime.Now.Ticks.ToString() + ".pdf";
            strRutaHtml = uploadPath + @"\" + "CuerpoHtml" + DateTime.Now.Ticks.ToString() + ".html";

            StreamWriter str = new StreamWriter(strRutaHtml, true, Encoding.Default);
            str.Write(sScript.ToString());
            str.Dispose();

            #region Firmas
            List<object[]> listObjects = new List<object[]>();
            object[] objetos = new object[3];



            objetos = new object[3];
            objetos[0] = vNombreRecurrente.ToUpper();
            objetos[1] = vDocumento;
            objetos[2] = true;

            listObjects.Add(objetos);


            #endregion

            DocumentoiTextSharp.CreateFilePDFConformidad(dtTMPReemplazar, strRutaHtml, strRutaPDF, Server.MapPath("~/Images/Escudo.PNG"), listObjects);

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {
                    Session["binaryData"] = FileBuffer;
                    string strUrl = "../Accesorios/VisorPDF.aspx";
                    string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                    Comun.EjecutarScript(Page,strScript) ;
                }
            }
            #endregion
        }

        private void EjecutarScript(string strScript)
        {
            throw new NotImplementedException();
        }



        private string AplicarInicialMayuscula(string palabra)
        {

            if (palabra.Trim() != string.Empty)
            {
                palabra = palabra.ToLower();
                return palabra[0].ToString().ToUpper() + palabra.Substring(1, palabra.Length - 1); ;
            }

            return string.Empty;
        }


        private string ObtenerDocumentoConformidad(string vNombre, string vDocumento)
        {

            bool bEsMujer = false;



            if (ViewState["PER_GENERO"] != null)
            {
                if (ViewState["PER_GENERO"].ToString() == Convert.ToInt32(Enumerador.enmGenero.FEMENINO).ToString())
                {
                    bEsMujer = true;
                }
            }

            StringBuilder sScript = new StringBuilder();

            sScript.Append("<p align=\"justify\"; style=\"background-color:transparent; font-family:arial;\" >");
            sScript.Append("Yo, ");
            sScript.Append(vNombre);
            sScript.Append(", identificad");

            if (bEsMujer)
                sScript.Append("a");
            else
                sScript.Append("o");

            sScript.Append(" con ");
            sScript.Append(vDocumento.ToUpper());
            sScript.Append(", ");
            sScript.Append("declaro que he leído y revisado en su detalle la Constancia de Inscripción Militar - CIM");

            sScript.Append(", que he tenido a la vista y me ha sido entregado en la fecha,");
            sScript.Append(" manifestando mi conformidad con su contenido.");
            sScript.Append("</p>");
            sScript.Append("<br />");


            sScript.Append("<p align=\"right\"; style=\"background-color:transparent; font-family:arial;\">");
            DateTime dt_Fecha = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));
            string str_Fecha = dt_Fecha.ToString("dd") + " de " + AplicarInicialMayuscula(dt_Fecha.ToString("MMMM")) + " de " + dt_Fecha.ToString("yyyy");


            if (Session["CiudadOficinaConsular"] != null)
            {

                str_Fecha = Session["CiudadOficinaConsular"].ToString().ToUpper() + ", " + str_Fecha;
            }

            sScript.Append(str_Fecha);
            sScript.Append("</p>");
            sScript.Append("<br />");
            sScript.Append("<tab></tab>");


            return sScript.ToString();
        }        


        DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }


        #endregion ActaConformidad

        public void CrearConstanciaMilitariTextSharp(Page page, DataTable dt)
        {
            try
            {
                #region Inicializando Variables
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                string strRutaPDF = uploadPath + @"\" + "CuerpoHtml" + DateTime.Now.Ticks.ToString() + ".pdf";
                string strRutaHtml = uploadPath + @"\" + "CuerpoHtml" + DateTime.Now.Ticks.ToString() + ".html";

                string imagenFotoURL = string.Empty;

                imagenFotoURL = uploadPath + @"\" + Convert.ToString(dt.Rows[0]["acad_vNombreArchivo"]);

                float fMargenIzquierdaDoc = 60;
                float fMargenDerechaDoc = 60;
                float fMargenSuperiorDoc = 90;
                float fMargenInferiorDoc = 80;

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);



                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(strRutaPDF, FileMode.Create));

                document.Open();
                document.NewPage();

                float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                cb.SetFontAndSize(bfTimes, 12);
                float ejex = (float)11.6;
                float fOrigenX = 68;
                float fOrigenY = document.PageSize.Height - 71 - 20;
                float fEjeXCentrado = document.PageSize.Width / 2;

                #endregion

                #region PosicionesY

                float fechaPosY = fOrigenY - 8;
                float ubigeoLugarDepProvPosY = fOrigenY - 33; //- 23;
                float ubigeoLugarDistCpoPosY = fOrigenY - 48; //- 38;

                float celebranteIdentidad = fOrigenY - 64;//- 54;
                float celebranteNombrePosY = fOrigenY - 70 - 8;

                float celebrantePrimerApellidoPosY = fOrigenY - 86 - 8;
                float celebranteSegundoApellidoPosY = fOrigenY - 102 - 6;
                float celebranteCargoPosY = fOrigenY - 122 - 7;
                float celebranteExpedientePosY = fOrigenY - 136 - 7;

                float conyuge1NombrePosY = fOrigenY - 178 - 4;
                float conyuge1PrimerApellidoPosY = fOrigenY - 198 - 3;
                float conyuge1SegundoApellidoPosY = fOrigenY - 214 - 3;
                float conyuge1IdentiNacionalPosY = fOrigenY - 230 - 2;
                float conyuge1EdadEdoCivilPosY = fOrigenY - 248 - 1;
                float conyuge1DepProvPosY = fOrigenY - 270;
                float conyuge1DistCpoPosY = fOrigenY - 286 + 1;

                float conyuge2NombrePosY = fOrigenY - 316;
                float conyuge2PrimerApellidoPosY = fOrigenY - 336;
                float conyuge2SegundoApellidoPosY = fOrigenY - 352;
                float conyuge2IdentiNacionalPosY = fOrigenY - 368;
                float conyuge2EdadEdoCivilPosY = fOrigenY - 386 + 2;
                float conyuge2DepProvPosY = fOrigenY - 407;
                float conyuge2DistCpoPosY = fOrigenY - 423;

                float fechaRegistroPosY = fOrigenY - 447;

                float registradorDepProvPosY = fOrigenY - 481;
                float registradorDistCpoPosY = fOrigenY - 497;

                float registradorNombrePosY = fOrigenY - 531 + 2;

                float registradorIdentidadPosY = fOrigenY - 549 + 2;

                float observacionesPosY = fOrigenY - 564;

                #endregion

                cb.BeginText();
               

                EscribirTexto(fEjeXCentrado / 2 - 19 + 30, document.PageSize.Height - 200, "Libro:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 150, document.PageSize.Height - 200, "Folio:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 55 + 30, document.PageSize.Height - 230, "Ap. Paterno:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 57 + 30, document.PageSize.Height - 260, "Ap. Materno:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 40 + 30, document.PageSize.Height - 290, "Nombres:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 20 + 30, document.PageSize.Height - 320, "Clase:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 20 + 30 + 180, document.PageSize.Height - 320, "Calificación:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 109 + 30, document.PageSize.Height - 350, "Fecha de Nacimiento:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30, document.PageSize.Height - 380, "Sexo:", cb);
                EscribirTexto(fEjeXCentrado / 2 - 20 + 30 + 180, document.PageSize.Height - 380, "Talla:", cb);

                bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);
                cb.SetFontAndSize(bfTimes, 12);

                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 200, dt.Rows[0]["Libro"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 150 + 40, document.PageSize.Height - 200, dt.Rows[0]["Folio"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 230, dt.Rows[0]["Paterno"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 260, dt.Rows[0]["Materno"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 290, dt.Rows[0]["Nombres"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 320, dt.Rows[0]["Clase"].ToString(), cb);

                string[] clasificacionArr = dt.Rows[0]["Clasificacion"].ToString().Trim().Split('/');
                string clasificacion=string.Empty;

                if (clasificacionArr.Length > 0)
                {
                    clasificacion= clasificacionArr[0];
                }

                EscribirTexto(fEjeXCentrado / 2 - 20 + 30 + 180 + 80, document.PageSize.Height - 320, clasificacion, cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 350, DateTime.Parse(dt.Rows[0]["FechaNacimiento"].ToString()).ToString("MM-dd-yyyy"), cb);
                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 40, document.PageSize.Height - 380, dt.Rows[0]["Sexo"].ToString(), cb);
                EscribirTexto(fEjeXCentrado / 2 - 20 + 30 + 180 + 80, document.PageSize.Height - 380, dt.Rows[0]["Talla"].ToString() + " cm", cb);

                EscribirTexto(fEjeXCentrado / 2 - 19 + 30 + 90, document.PageSize.Height - 425, "Firma y Sello de Jefe de Registro Militar", cb);
                EscribirTexto(document.PageSize.Width - fMargenDerechaDoc - 80 + 3, document.PageSize.Height - 562, "Huella y Firma", cb);
                EscribirTexto(document.PageSize.Width - fMargenDerechaDoc - 70 + 1, document.PageSize.Height - 574, "del Usuario", cb);
                cb.EndText();

                ReportParameter[] parameters = parameters = new ReportParameter[3];
                parameters[0] = new ReportParameter("acad_vNombreArchivo", uploadPath + imagenFotoURL);
                ReportDataSource datasource = new ReportDataSource("dsActoMilitar", dt);


                #region Imagen

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/EscudoPeru_2015.jpg"));
                img.ScaleAbsolute(60, 70);
                img.SetAbsolutePosition(fEjeXCentrado / 2 / 2 - 10, document.PageSize.Height - 155);
                document.Add(img);

                img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/EscudoMilitar_20150206.png"));
                img.ScaleAbsolute(fEjeXCentrado, fEjeXCentrado);
                img.SetAbsolutePosition(fEjeXCentrado / 2, document.PageSize.Height / 2 - 30);
                document.Add(img);

                cb.Rectangle(document.PageSize.Width - fMargenDerechaDoc - 70, document.PageSize.Height - 550, 65f, 70f);
                cb.Stroke();

                #endregion

                #region imagen Foto

              
                string strPathFotoNuevo = imagenFotoURL;

                if (strPathFotoNuevo.IndexOf(".JPG") > 0 || strPathFotoNuevo.IndexOf(".JPEG") > 0 || strPathFotoNuevo.IndexOf(".jpg") > 0 || strPathFotoNuevo.IndexOf(".jpeg") > 0) 
                {
                    iTextSharp.text.Image imgFotoNueva = iTextSharp.text.Image.GetInstance(strPathFotoNuevo);
                    imgFotoNueva.ScaleAbsolute(60, 70);
                    imgFotoNueva.SetAbsolutePosition(fEjeXCentrado / 2 / 20 + 40, document.PageSize.Height - 300);
                    document.Add(imgFotoNueva);
                    cb.Rectangle(document.PageSize.Width - fMargenDerechaDoc - 490, document.PageSize.Height - 300, 65f, 70f); 
                    cb.Stroke();
                }
                else
                {
                    strPathFotoNuevo = null;
                }

                #endregion

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();
                frase.Add(new iTextSharp.text.Chunk("CONSTANCIA DE INSCRIPCIÓN MILITAR - CIM",
                     iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE)));

                parrafo.Add(frase);
                parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                parrafo.SetLeading(0.0f, 0.5f);

                document.Add(parrafo);

                parrafo = new iTextSharp.text.Paragraph();
                frase = new iTextSharp.text.Phrase();

                frase.Add(new iTextSharp.text.Chunk("\nREPÚBLICA DEL PERÚ\n"));
                frase.Add(new iTextSharp.text.Chunk("MINISTERIO DE RELACIONES EXTERIORES\n"));
                frase.Add(new iTextSharp.text.Chunk("CONSTANCIA DE INSCRIPCIÓN MILITAR\n"));
                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                frase.Add(new iTextSharp.text.Chunk("\nSERVICIO DE RESERVA",
                     iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE)));
                parrafo.Add(frase);

                parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                parrafo.SetLeading(0.0f, 1.0f);

                document.Add(parrafo);

                parrafo = new iTextSharp.text.Paragraph();
                frase = new iTextSharp.text.Phrase();


                frase.Add(new iTextSharp.text.Chunk("\nCalificación:"));
                frase.Add(new iTextSharp.text.Chunk("\nProfesión/Ocupación:"));
                frase.Add(new iTextSharp.text.Chunk("\nPermanencia en la reserva:"));
                frase.Add(new iTextSharp.text.Chunk("\nFecha de Expedición:"));
                frase.Add(new iTextSharp.text.Chunk("\nORM+N° Control cada Institución:"));
                parrafo.Add(frase);

                parrafo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                parrafo.SetLeading(0.0f, 1.75f);

                document.Add(parrafo);

                parrafo = new iTextSharp.text.Paragraph();
                frase = new iTextSharp.text.Phrase();

                frase.Add(new iTextSharp.text.Chunk("\nIMPORTANTE"));
                parrafo.SetLeading(0.0f, 1.0f);
                parrafo.Add(frase);

                parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                parrafo.SetLeading(0.0f, 1.0f);
                parrafo.Font.SetStyle(iTextSharp.text.Font.UNDERLINE);
                document.Add(parrafo);

                parrafo = new iTextSharp.text.Paragraph();
                frase = new iTextSharp.text.Phrase();

                frase.Add(new iTextSharp.text.Chunk("\nSi encuentra esta Constancia de Inscripción Militar sírvase devolverla "));
                frase.Add(new iTextSharp.text.Chunk("a la dependencia Consular peruana más cercana.\n\n"));
                frase.Add(new iTextSharp.text.Chunk("Si desea prestar Servicio Militar Acuartelado, tiene entre 18 y 25 años de edad, "));
                frase.Add(new iTextSharp.text.Chunk("deberá apersonarse a cualquier Oficina de Registro Militar de las Fuerzas Armadas "));
                frase.Add(new iTextSharp.text.Chunk("o Policía Nacional más cercana.\n\n"));
                frase.Add(new iTextSharp.text.Chunk("El personal de reserva tendrá la obligación de actualizar sus datos cada (2) años "));
                frase.Add(new iTextSharp.text.Chunk("obligatoriamente o cuando se produzca alguna variación de sus datos personales, podrá "));
                frase.Add(new iTextSharp.text.Chunk("hacerlo en la Oficina de Registro Militar cercana a su residencia o a través de la página "));
                frase.Add(new iTextSharp.text.Chunk("Web de la institución de las Fuerzas Armadas a la que pertenece (Art 136° del Reglamento "));
                frase.Add(new iTextSharp.text.Chunk("de la Ley N° 29248-Ley del Servicio Militar)."));
                parrafo.Add(frase);

                parrafo.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                parrafo.SetLeading(0.0f, 1.0f);

                document.Add(parrafo);


                document.Close();

                #region Impresion en Navegador

                if (System.IO.File.Exists(strRutaPDF))
                {
                    System.Net.WebClient User = new System.Net.WebClient();
                    Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                        string strUrl = "../Accesorios/VisorPDF.aspx";
                        string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                        Comun.EjecutarScriptUniqueIdDinamico(page, strScript,"ConstanciaMilitar");
                    }

                    User.Dispose();
                }

                if (File.Exists(strRutaHtml))
                {
                    File.Delete(strRutaHtml);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void EscribirTexto(float ejeX, float ejeY, string vTexto, iTextSharp.text.pdf.PdfContentByte cb)
        {
            cb.SetTextMatrix(ejeX, ejeY);
            cb.ShowText(vTexto);
        }

        protected void btnDesabilitarAutoahesivo_Click(object sender, EventArgs e)
        {
            btnVistaPrev.Enabled = false;
            chkImpresion.Checked = true;
            chkImpresion.Enabled = false;
            txtCodAutoadhesivo.Enabled = false;
            hdn_ImpresionCorrecta.Value = "1";
            BindGridActuacionesInsumoDetalle(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            updVinculacion.Update();

        }
        DataTable Load_Imagenes()
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            var load_Imagen = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtenerFoto(
                Comun.ToNullInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), "1",
                Constantes.CONST_PAGE_SIZE_ADJUNTOS, ref IntTotalCount, ref IntTotalPages);

            DataTable Ordenado = null;
            try
            {
                Ordenado = (from dr in load_Imagen.AsEnumerable()
                            orderby dr["acad_dFechaCreacion"]
                            select dr).CopyToDataTable();
            }
            catch
            {
                Ordenado = new DataTable();
            }

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            foreach (DataRow row in Ordenado.Rows)
            {
                switch (Comun.ToNullInt32(row["sAdjuntoTipoId"]))
                {
                    case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                        break;
                }
            }

            return load_Imagen;
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
        //private void ActualizarFechaFormato()
        //{
        //     List<BE.RE_PARTICIPANTE> loParticipanteContainer = (List<BE.RE_PARTICIPANTE>)Session["Participante"];
        //     foreach (BE.RE_PARTICIPANTE objParticipante in loParticipanteContainer)
        //     {
        //         if (objParticipante.sTipoParticipanteId == Convert.ToInt16(Enumerador.enmTipoParticipanteMilitar.TITULAR))
        //         { 
        //             if (txtFecNac.Text.Length > 0)
        //                {
        //                    DateTime datFecha = new DateTime();
        //                    if (!DateTime.TryParse(txtFecNac.Text, out datFecha))
        //                    {
        //                        objParticipante.pers_dNacimientoFecha = Comun.FormatearFecha(txtFecNac.Text);
        //                    }
        //                }
        //            else
        //            {
        //                objParticipante.pers_dNacimientoFecha = null;
        //            }

        //             break;
        //         }
        //     }
        //}
    }
}
